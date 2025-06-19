Imports System.IO

''' <summary>
''' This logging classed can be used for troubleshooting and debugging problems that are hard to figure out.
''' First you must turn on logging for an application by creating a .AllowLogging file in the User Profile folder for one more dates.
''' Use scripts similar to the following to quickly generate multiple .AllowLogging files:
''' \\clair\s\Finesse\TransferInquiryTools - Create tt Log Files (Main App).ps1
''' \\clair\s\Finesse\TransferInquiryTools - Create ttc Log Files (Check in-out-etc).ps1
''' 
''' Here is a code example of how to use it:
''' 
''' In Transfer Tools:
'''     Private _logToLocalFile As LoggingToLocalFile
'''     _logToLocalFile = New LoggingToLocalFile("tt", "LogForTfrTools")
'''     
'''     _logToLocalFile.Log("Log note...")
''' In Transfer Tools Controls (Check-in, Check-out, etc):
'''     Private _logToLocalFile As LoggingToLocalFile
'''     _logToLocalFile = New LoggingToLocalFile("ttc_" + myInOutText, "LogFor" + myInOutText)
'''     
'''     _logToLocalFile.Log("Log note...")
''' </summary>
Public Class LoggingToLocalFile

    Public Property LogFileNamePrefix As String
    ''' <summary>
    ''' User defined location of the log file or defaults to the user profile path.  For example, C:\Users\UserName
    ''' </summary>
    ''' <returns></returns>
    Public Property LogFolderPath As String = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)

    ''' <summary>
    ''' User defined location of session log files created by the class.  Session log files will live in the LogFolderPath + "\" + SessionLogFileSubfolder.
    ''' </summary>
    ''' <returns></returns>
    Public Property SessionLogFileSubfolder As String = "SessionLogFiles"

    Public ReadOnly Property ShouldLog As Boolean = False

    Private Property sw As StreamWriter


    Private Property LastLogDateTime As DateTime = DateTime.Now

    Public Sub New(LogFileNamePrefix As String, SessionLogFileSubfolder As String, LogFolderPath As String)

        Me.LogFileNamePrefix = LogFileNamePrefix
        Me.LogFolderPath = LogFolderPath
        Me.SessionLogFileSubfolder = SessionLogFileSubfolder

        ConfigureLogging()

    End Sub

    Public Sub New(LogFileName As String, SessionLogFileSubfolder As String)
        Me.LogFileNamePrefix = LogFileName
        Me.SessionLogFileSubfolder = SessionLogFileSubfolder

        ConfigureLogging()

    End Sub

    Public Sub ConfigureLogging()
        _ShouldLog = File.Exists(LogFolderPath & "\" & LogFileNamePrefix & "_" & Date.Now.ToString("yyyyMMdd") & ".AllowLogging")

        If ShouldLog Then Logging_Start()

    End Sub

    Private Sub Logging_Start()
        If ShouldLog Then
            Dim fileName As String = LogFileNamePrefix & "_" + Date.Now.ToString("yyyyMMddHHmmss") + ".log"
            fileName = fileName.Replace(":", "")

            If Not Directory.Exists(LogFolderPath & "\" & SessionLogFileSubfolder) Then
                Directory.CreateDirectory(LogFolderPath & "\" & SessionLogFileSubfolder)
            End If
            sw = New StreamWriter(LogFolderPath & "\" & SessionLogFileSubfolder & "\" & fileName, True, System.Text.Encoding.Default)

            Log("Logging started.")
        End If
    End Sub

    Public Sub Logging_End()
        If ShouldLog Then
            Dim currentLogDateTime As DateTime = DateTime.Now
            Dim secondDiff = DateDiff(DateInterval.Second, LastLogDateTime, currentLogDateTime)
            'LastLogDateTime = currentLogDateTime

            sw.WriteLine("")
            sw.WriteLine("Logging ended.")
            sw.WriteLine("SecondsSinceLastLog-" & secondDiff & ";DateTime-" + DateTime.Now.ToString("yyyyMMddHHmmssffff"))

            sw.Close()
            _ShouldLog = False
        End If
    End Sub

    Public Sub Log(ByVal Message As String)
        If ShouldLog Then
            Dim currentLogDateTime As DateTime = DateTime.Now
            Dim secondDiff = DateDiff(DateInterval.Second, LastLogDateTime, currentLogDateTime)
            LastLogDateTime = currentLogDateTime

            sw.WriteLine(Message)
            sw.WriteLine("SecondsSinceLastLog-" & secondDiff & ";DateTime-" + DateTime.Now.ToString("yyyyMMddHHmmssffff"))
            sw.Flush()
        End If
    End Sub

End Class
