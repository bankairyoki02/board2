Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation
Imports System.IO

Public Enum SQLConnectionStatus
    Unknown = 0
    Available = 1
    Unavailable = 2
End Enum

Public Class SQLConnectionAsyncTimeoutException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

End Class

Public Class SQLConnectionAsyncUnexpectedFailureException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

End Class

Class ConnectionFailureItem
    Public SQLServerDownUTCDateTime As DateTime
    Public SQLConnectionSuccessiveFailures As Integer
    Public Notes As String

    Public Sub New(SQLServerDownUTCDateTime As DateTime, SQLConnectionSuccessiveFailures As Integer, Notes As String)
        Me.SQLServerDownUTCDateTime = SQLServerDownUTCDateTime
        Me.SQLConnectionSuccessiveFailures = SQLConnectionSuccessiveFailures
        Me.Notes = Notes
    End Sub
End Class

Class FinesseConnectionFactory

    Private _PreviousSQLConnectionStatus As SQLConnectionStatus = SQLConnectionStatus.Unknown
    Private _SQLServerDownUTCDateTime As DateTime
    Private _SQLServerAvailableUTCDateTime As DateTime

    Private _SQLConnectionSuccessiveFailures As Integer = 0
    Private _Notes As String

    Private _RequeueExceptionCount As Integer = 0

    Private _KeepRunning As Boolean = True
    Private _HasConnectionFailureLogRetryError As Boolean = False
    Private _qConnectionFailures As New System.Collections.Concurrent.BlockingCollection(Of ConnectionFailureItem)

    Public Sub New(Optional MillisecondsTimeout As Integer = 10000)

        _MillisecondsTimeout = MillisecondsTimeout 'default to 3 seconds

        Dim connFailureLoggingThread As New Threading.Thread(AddressOf ProcessConnectionFailureQueue)
        connFailureLoggingThread.IsBackground = True
        connFailureLoggingThread.Start()

        _PreviousSQLConnectionStatus = If(IsNetworkAvailable, SQLConnectionStatus.Available, SQLConnectionStatus.Unavailable)
        If _PreviousSQLConnectionStatus = SQLConnectionStatus.Unavailable Then
            'Set the Down UTC DateTime here if the network is down when the application starts.
            _SQLServerDownUTCDateTime = DateTime.UtcNow
            ReportConnectionUnavailable("Application started without a network connection.")
        End If

    End Sub

    Public ReadOnly Property PreviousSQLConnectionStatus As SQLConnectionStatus
        Get
            Return _PreviousSQLConnectionStatus
        End Get
    End Property

    Public ReadOnly Property IsNetworkAvailable As Boolean
        Get
            Return NetworkInterface.GetIsNetworkAvailable
        End Get
    End Property

    Public ReadOnly Property MillisecondsTimeout As Integer

    Private _ReportFailureLock As New Object

    Private Sub ReportConnectionUnavailable(notes As String)
        SyncLock _ReportFailureLock
            Debug.Print(notes)
            If _PreviousSQLConnectionStatus = SQLConnectionStatus.Available Then
                'network was available, but is no longer available
                _SQLServerDownUTCDateTime = DateTime.UtcNow
            End If

            _Notes = notes
            _SQLConnectionSuccessiveFailures += 1
            _PreviousSQLConnectionStatus = SQLConnectionStatus.Unavailable
        End SyncLock
    End Sub

    Public Function CreateFinesseConnection(Optional MillisecondsTimeout As Integer = -1) As SqlConnection
        If _HasConnectionFailureLogRetryError Then
            'If there's a connection failure log retry problem one successful connection will be made before this error is displayed to the user.
            _HasConnectionFailureLogRetryError = False
            Throw New SQLConnectionAsyncUnexpectedFailureException("Attempted to requeue the connection failures 100 times.  There is a Finesse Database Connection problem.  Please get help.")
        End If

        If MillisecondsTimeout = -1 Then
            MillisecondsTimeout = _MillisecondsTimeout
        End If

        If Not IsNetworkAvailable Then
            Dim errMsg = "Network is down."

            ReportConnectionUnavailable(errMsg)


            'LogConnectionFailure(isSQLServerConnectFailure:=True, FailureNotes:=errMsg)
            'This is not really a "SQL Connection" error, but the user's computer cannot access SQL Server because their network is down.
            Throw New SQLConnectionAsyncTimeoutException(errMsg)
        End If

        ' A connection string's ConnectionTimeout only take effect after a TCP connection to the SQL Server has been established, and Windows' TCP layer will wait up to 15 seconds.
        ' In order to give up earlier (e.g. after only 10 seconds), we need to set our own timer.
        ' Source:
        ' https://stackoverflow.com/questions/3114051/sql-connection-waits-15-seconds-despite-3-seconds-timeout-in-connection-string
        Try
            Dim tempConn As New SqlConnection(FinesseConnectionString(timeoutSeconds:=_MillisecondsTimeout \ 1000))

            Dim connSemaphore As New Threading.Semaphore(0, Integer.MaxValue)

            AddHandler tempConn.StateChange, Sub(sender As Object, e As StateChangeEventArgs)
                                                 If e.CurrentState = ConnectionState.Open Then
                                                     connSemaphore.Release()
                                                 End If
                                             End Sub

            tempConn.OpenAsync()
            connSemaphore.WaitOne(MillisecondsTimeout)

            If tempConn.State <> ConnectionState.Open Then
                tempConn.Close()
                tempConn.Dispose()
                tempConn = Nothing

                Dim seconds = (MillisecondsTimeout / 1000)
                Dim errMsg = String.Format($"The async connection timeout of {seconds} seconds has expired.")

                ReportConnectionUnavailable(errMsg)

                Throw New SQLConnectionAsyncTimeoutException(errMsg)

            End If

            SyncLock _ReportFailureLock
                If PreviousSQLConnectionStatus = SQLConnectionStatus.Unavailable Then
                    _SQLServerAvailableUTCDateTime = DateTime.UtcNow
                    QueueConnectionFailure(SQLServerDownUTCDateTime:=_SQLServerDownUTCDateTime,
                                                                 SQLConnectionSuccessiveFailures:=_SQLConnectionSuccessiveFailures,
                                                                 Notes:=_Notes)

                    _SQLConnectionSuccessiveFailures = 0
                    _PreviousSQLConnectionStatus = SQLConnectionStatus.Available
                End If
            End SyncLock

            Return tempConn

        Catch ex As Exception
            ReportConnectionUnavailable(ex.Message)

            Throw ex
        End Try

    End Function

    Protected Overrides Sub Finalize()
        _KeepRunning = False

        MyBase.Finalize()
    End Sub

    Public Sub QueueConnectionFailure(SQLServerDownUTCDateTime As DateTime, SQLConnectionSuccessiveFailures As Integer, Notes As String)
        Dim item = New ConnectionFailureItem(SQLServerDownUTCDateTime:=SQLServerDownUTCDateTime,
                                          SQLConnectionSuccessiveFailures:=SQLConnectionSuccessiveFailures,
                                          Notes:=Notes)

        _qConnectionFailures.Add(item)


    End Sub

    Private Sub ProcessConnectionFailureQueue()
        Dim sqlInsertStart As New System.Text.StringBuilder
        sqlInsertStart.AppendLine("insert dbo.ConnectionFailureLog")
        sqlInsertStart.AppendLine("        (   login_time")
        sqlInsertStart.AppendLine("          , host_name")
        sqlInsertStart.AppendLine("          , program_name")
        sqlInsertStart.AppendLine("          , login_name")
        sqlInsertStart.AppendLine("          , host_failure_time")
        sqlInsertStart.AppendLine("          , host_recover_time")
        sqlInsertStart.AppendLine("          , transaction_time")
        sqlInsertStart.AppendLine("          , note")
        sqlInsertStart.AppendLine("          , SQLConnectionAttempts")
        sqlInsertStart.AppendLine("        )")
        sqlInsertStart.AppendLine("select")
        sqlInsertStart.AppendLine("            login_time")
        sqlInsertStart.AppendLine("          , host_name")
        sqlInsertStart.AppendLine("          , program_name")
        sqlInsertStart.AppendLine("          , login_name")
        sqlInsertStart.AppendLine("          , host_failure_time")
        sqlInsertStart.AppendLine("          , host_recover_time")
        sqlInsertStart.AppendLine("          , transaction_time")
        sqlInsertStart.AppendLine("          , note")
        sqlInsertStart.AppendLine("          , SQLConnectionAttempts")
        sqlInsertStart.AppendLine("from ")
        sqlInsertStart.AppendLine("(")
        sqlInsertStart.AppendLine("	select v.host_failure_time, v.host_recover_time, v.note, v.SQLConnectionAttempts, des.transaction_time, des.login_time, des.host_name, des.program_name, des.login_name")
        sqlInsertStart.AppendLine("	from (")
        sqlInsertStart.AppendLine("		values")



        Dim sqlInsertEnd As New System.Text.StringBuilder
        sqlInsertEnd.AppendLine("	) v (host_failure_time, host_recover_time, note, SQLConnectionAttempts)")
        sqlInsertEnd.AppendLine("	cross apply (")
        sqlInsertEnd.AppendLine("		select transaction_time = getdate(), des.login_time, des.host_name, des.program_name, des.login_name")
        sqlInsertEnd.AppendLine("		from sys.dm_exec_sessions [des]")
        sqlInsertEnd.AppendLine("		where des.session_id = @@spid")
        sqlInsertEnd.AppendLine("	) des")
        sqlInsertEnd.AppendLine(") x")


        While _KeepRunning
            Dim r As ConnectionFailureItem = Nothing
            If _qConnectionFailures.TryTake(r, 1000) Then
                Dim sqlStrDataValues As New System.Text.StringBuilder
                Dim failures As New List(Of ConnectionFailureItem)

                sqlStrDataValues.AppendLine("(")
                sqlStrDataValues.AppendLine($"         {r.SQLServerDownUTCDateTime.SQLDateWithTime}") 'host_failure_time
                sqlStrDataValues.AppendLine($"       , {DateTime.UtcNow.SQLDateWithTime}") 'host_recover_time
                sqlStrDataValues.AppendLine($"       , {r.Notes.SQLQuote}") 'note
                sqlStrDataValues.AppendLine($"       , {r.SQLConnectionSuccessiveFailures}") 'SQLConnectionAttempts
                sqlStrDataValues.AppendLine(")")
                failures.Add(r)

                Do While _qConnectionFailures.Count > 0
                    r = _qConnectionFailures.Take
                    sqlStrDataValues.AppendLine(", (")
                    sqlStrDataValues.AppendLine($"         {r.SQLServerDownUTCDateTime.SQLDateWithTime}") 'host_failure_time
                    sqlStrDataValues.AppendLine($"       , {DateTime.UtcNow.SQLDateWithTime}") 'host_recover_time
                    sqlStrDataValues.AppendLine($"       , {r.Notes.SQLQuote}") 'note
                    sqlStrDataValues.AppendLine($"       , {r.SQLConnectionSuccessiveFailures}") 'SQLConnectionAttempts
                    sqlStrDataValues.AppendLine("  )")
                    failures.Add(r)
                Loop

                Dim sqlStr = sqlInsertStart.ToString & sqlStrDataValues.ToString & sqlInsertEnd.ToString
                Try
                    Using conn = CreateFinesseConnection()
                        conn.ExecuteNonQuery(sqlStr)
                        _RequeueExceptionCount = 0
                    End Using
                Catch ex As Exception
                    If _RequeueExceptionCount > 100 Then
                        _HasConnectionFailureLogRetryError = True
                        _RequeueExceptionCount = 0

                        Dim RequeueErrorLogDumpFilePath As String = System.Environment.GetEnvironmentVariable("USERPROFILE") & "\FinesseConnectionFailureLogError" & DateTime.UtcNow.ToString("yyyyMMddHHmm") & ".error"

                        Using sw As StreamWriter = File.CreateText(RequeueErrorLogDumpFilePath)
                            For Each r In failures
                                sw.WriteLine("SQLServerDownUTCDateTime:" & r.SQLServerDownUTCDateTime)
                                sw.WriteLine("SQLConnectionSuccessiveFailures:" & r.SQLConnectionSuccessiveFailures)
                                sw.WriteLine("Notes:" & r.Notes)
                                sw.WriteLine("--------------------------------------------------")
                            Next
                        End Using
                        Exit Try

                    End If
                    _RequeueExceptionCount += 1
                    'Requeue the failure and fail silently
                    For Each r In failures
                        QueueConnectionFailure(SQLServerDownUTCDateTime:=r.SQLServerDownUTCDateTime,
                                       SQLConnectionSuccessiveFailures:=r.SQLConnectionSuccessiveFailures,
                                       Notes:=r.Notes)

                    Next
                End Try
            End If
        End While

    End Sub

    'Private Function ConcatConnectionFailureQuery(r As ConnectionFailureItem, sqlStr As System.Text.StringBuilder) As System.Text.StringBuilder

    '    sqlStr.AppendLine("   select des.login_time, des.host_name, des.program_name, des.login_name")
    '    sqlStr.AppendLine($"       , host_failure_time = {r.SQLServerDownUTCDateTime.SQLDateWithTime}")
    '    sqlStr.AppendLine($"       , host_recover_time = {DateTime.UtcNow.SQLDateWithTime}")
    '    sqlStr.AppendLine("       , transaction_time = getdate()")
    '    sqlStr.AppendLine($"       , note = {r.Notes.SQLQuote}")
    '    sqlStr.AppendLine($"       , SQLConnectionAttempts = {r.SQLConnectionSuccessiveFailures}")
    '    sqlStr.AppendLine("   from sys.dm_exec_sessions des")
    '    sqlStr.AppendLine("   where des.session_id = @@spid")

    '    Return sqlStr
    'End Function

End Class
