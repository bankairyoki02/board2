Imports System.Data.SqlClient

Public Class SQLJammer
    Private Class QueryAction
        Public Query As String
        Public TableToLoad As DataTable
        Public Callback As QueryCompleteCallback = Nothing
    End Class

    Private _conn As SqlConnection
    Private _queryActions As New List(Of QueryAction)

    Public Delegate Sub QueryCompleteCallback(t As DataTable)

    Private Sub New()

    End Sub

    Public Sub New(conn As SqlConnection)
        _conn = conn
    End Sub

    Public Sub Add(sSQL As String, f As QueryCompleteCallback)
        _queryActions.Add(New QueryAction With {.Query = sSQL, .TableToLoad = New DataTable, .Callback = f})
    End Sub

    Public Sub Add(sSQL As String, t As DataTable)
        _queryActions.Add(New QueryAction With {.Query = sSQL, .TableToLoad = t})
    End Sub

    Public Sub Execute()
        Dim allSQL = String.Join($";{vbNewLine}", (From qa In _queryActions Select qa.Query).ToArray)

        If _conn.State = ConnectionState.Closed Then
            _conn.Open()
        End If
        Using cmd As New SqlCommand(allSQL, _conn), dr = cmd.ExecuteReader()
            For Each qa In _queryActions
                qa.TableToLoad.Load(dr)
                If qa.Callback IsNot Nothing Then
                    For Each c In qa.TableToLoad.Columns.Cast(Of DataColumn)
                        c.ReadOnly = False
                        c.AllowDBNull = True
                    Next
                    qa.Callback(qa.TableToLoad)
                End If
            Next

            cmd.DebugCommand()
            Debug.Assert(dr.IsClosed)
        End Using
    End Sub
End Class