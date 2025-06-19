Imports System.Data.SqlClient
Imports System.Environment
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
'Imports System.Windows.Forms

Public Module FinesseUtilities

    <DllImport("user32.dll", SetLastError:=True)> Private Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
    End Function

    Private Enum ShowWindowConstants As Integer
        SW_HIDE = 0
        SW_SHOWNORMAL = 1
        SW_NORMAL = 1
        SW_SHOWMINIMIZED = 2
        SW_SHOWMAXIMIZED = 3
        SW_MAXIMIZE = 3
        SW_SHOWNOACTIVATE = 4
        SW_SHOW = 5
        SW_MINIMIZE = 6
        SW_SHOWMINNOACTIVE = 7
        SW_SHOWNA = 8
        SW_RESTORE = 9
        SW_SHOWDEFAULT = 10
        SW_FORCEMINIMIZE = 11
        SW_MAX = 11
    End Enum

    <DllImport("user32.dll", SetLastError:=True)> Private Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As ShowWindowConstants) As Boolean
    End Function


    Public ReadOnly Property gFinesseArgument(ByVal argtag As String) As String
        Get
            Dim arg_with_colon As String

            arg_with_colon = argtag & ":"

            Dim argtag_pos As Long, arg_pos As Long
            Dim end_of_arg_pos As Long, space_pos As Long, quote_pos As Long, colon_pos As Long

            argtag_pos = InStr(Command(), arg_with_colon)
            If argtag_pos > 0 Then
                arg_pos = argtag_pos + Len(arg_with_colon)
                space_pos = InStr(arg_pos, Command(), " ", CompareMethod.Text)
                quote_pos = InStr(arg_pos, Command(), """", CompareMethod.Text)
                colon_pos = InStr(arg_pos, Command(), ":", CompareMethod.Text)

                end_of_arg_pos = Len(Command()) + 1
                If quote_pos < end_of_arg_pos And quote_pos > 0 Then end_of_arg_pos = quote_pos
                If space_pos < end_of_arg_pos And space_pos > 0 Then end_of_arg_pos = space_pos
                If colon_pos < end_of_arg_pos And colon_pos > 0 Then end_of_arg_pos = colon_pos

                gFinesseArgument = Mid(Command(), arg_pos, end_of_arg_pos - arg_pos)
            Else
                gFinesseArgument = ""
            End If
        End Get
    End Property

    ''' <summary>
    ''' Parse horrible old-Finesse-style arguments in roughly the same way as gFinesseArgument, but with the ability to handle
    ''' arguments with spaces in them.
    ''' 
    ''' Tested in only a limited capacity, so hopefully doesn't break...
    ''' </summary>
    ''' <param name="argtags">You must provide a list of all possible arguments that might be passed to the program, or the 
    ''' arguments may not be parsed correctly
    ''' </param>
    ''' <returns>Dictionary of key value pairs for argument tags and values</returns>
    Public Function GetParsedFinesseArguments(argtags() As String) As Dictionary(Of String, String)
        ' use blanks as the default value for all supplied tags, and overwrite the blanks in the parsing stage.
        Dim parsedArgs = argtags.ToDictionary(Function(argtag) argtag, Function(argtag) "")

        ' This pattern uses positive lookbehind and positive lookahead to match the position right before an argument's tag begins.
        ' The ?: dohickeys prevent the .Split from returning the lookbehinds & lookaheads themselves
        Dim patternMatchAnArgTag = $"(?<=(?:^|:| )""?)(?=(?:{String.Join("|", argtags.Select(Function(argtag) Regex.Escape(argtag)))}):)"
        Dim reMatchAnArgTag As New Regex(patternMatchAnArgTag)

        Dim commandLineArgumentString As String = Command()
        Dim arguments = reMatchAnArgTag.Split(commandLineArgumentString)

        For Each keyValue In arguments.Where(Function(s) Not String.IsNullOrEmpty(s))
            ' remove extraneous crap from the beginning and end of an argument
            keyValue = keyValue.TrimEnd({":"c, " "c})
            If keyValue.StartsWith("""") AndAlso keyValue.EndsWith("""") Then
                keyValue = keyValue.Substring(1, keyValue.Length - 2)
            End If

            ' Then split on (hopefully the last remaining) colon, and the two items are the key and the value
            Dim keyAndValue = keyValue.Split({":"c}, 2)
            If keyAndValue.Count = 2 Then
                parsedArgs(keyAndValue(0)) = keyAndValue(1)
            End If
        Next

        Return parsedArgs

    End Function


    Public ReadOnly Property FinesseConnectionString(Optional timeoutSeconds As Integer = 10, Optional username As String = "", Optional password As String = "", Optional database As String = "", Optional server As String = "") As String
        Get
            With New SqlConnectionStringBuilder()

                .DataSource = IIf(String.IsNullOrEmpty(server), GetEnvironmentVariable("ESSSERVER"), "ClairSQL.clair.lcl")
                .InitialCatalog = IIf(String.IsNullOrEmpty(database), GetEnvironmentVariable("ESSDB"), database)
                .UserID = IIf(String.IsNullOrEmpty(username), GetEnvironmentVariable("ESSUID"), username)
                .Password = IIf(String.IsNullOrEmpty(password), GetEnvironmentVariable("ESSPWD"), password)
                .ApplicationName = My.Application.Info.AssemblyName
                .AsynchronousProcessing = True
                .PersistSecurityInfo = True
                .ConnectTimeout = timeoutSeconds
                .MultipleActiveResultSets = True

                FinesseConnectionString = .ConnectionString
            End With
        End Get
    End Property

    Public Function GetOpenedSqlConnection(connectionString As String) As SqlConnection
        Dim conn As New SqlConnection(IIf(IsNothing(connectionString), FinesseConnectionString, connectionString))
        conn.Open()
        Return conn
    End Function

    Public Function GetOpenedFinesseConnection(Optional timeoutSeconds As Integer = 10, Optional username As String = "", Optional password As String = "", Optional database As String = "", Optional server As String = "") As SqlConnection
        Return GetOpenedSqlConnection(FinesseConnectionString(timeoutSeconds, username, password, database, server))
    End Function

    'uses windows credentials only certain dev users have them not for normal Finesse application use
    Public Function getOpenedFinesseConnectionWithWindowsCredentials(Optional database As String = "", Optional server As String = "", Optional timeoutSeconds As Integer = 10)
        Dim connString As String

        With New SqlConnectionStringBuilder()

            .DataSource = IIf(String.IsNullOrEmpty(server), GetEnvironmentVariable("ESSSERVER"), "ClairSQL.clair.lcl")
            .InitialCatalog = IIf(String.IsNullOrEmpty(database), GetEnvironmentVariable("ESSDB"), database)
            .UserID = IIf(String.IsNullOrEmpty(UserName), GetEnvironmentVariable("ESSUID"), UserName)
            .IntegratedSecurity = True
            .AsynchronousProcessing = True
            .PersistSecurityInfo = True
            .ConnectTimeout = timeoutSeconds

            connString = .ConnectionString
        End With

        Dim conn = New SqlConnection(connString)
        conn.Open()
        Return conn

    End Function

    Public Function ParallelDatabase(ByVal conn As SqlConnection) As String

        Static Dim parallelDB As String = ""
        If parallelDB <> "" Then
            Return parallelDB
        End If

        parallelDB = conn.ExecuteScalar("select value from sysconfig where tag = 'PARALLEL_DATABASE'")
        ParallelDatabase = parallelDB

    End Function

    ''' <summary>
    ''' Get the application (executable) name from mumenus
    ''' </summary>
    ''' <param name="packagecd"></param>
    ''' <param name="menuname"></param>
    ''' <param name="cmndsel"></param>
    ''' <param name="conn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetApplicationFileName(ByVal packagecd As String, ByVal menuname As String, ByVal cmndsel As String, ByRef conn As SqlConnection) As String

        GetApplicationFileName = ""

        Dim sqlStr As New StringBuilder
        sqlStr.AppendLine("select packagecd, menuname, cmndsel, appexec, appframe")
        sqlStr.AppendLine("from mumenus")
        sqlStr.AppendLine("where")
        sqlStr.AppendLine("	   packagecd = " & packagecd.SQLQuote)
        sqlStr.AppendLine("and menuname = " & menuname.SQLQuote)
        sqlStr.AppendLine("and cmndsel = " & cmndsel.SQLQuote)

        Dim appData = conn.GetDataTable(sqlStr)

        If appData.Rows.Count > 0 Then
            With appData.Rows(0)
                'Example appexec: %ESSVBDir%\prj
                'The prj is part of the ApplicationName and pieced together with the appframe in the code below. Example: \appname.exe
                Dim startIndex = CStr(.Item("appexec")).LastIndexOf("\")
                Dim prefixLength = CStr(.Item("appexec")).Length - CStr(.Item("appexec")).LastIndexOf("\")
                GetApplicationFileName = CStr(.Item("appexec")).Substring(startIndex, prefixLength) & .Item("appframe") & ".exe"
            End With
        End If

    End Function


    Public Enum ExistingWindowBehavior
        CreateNew
        BringToFront
    End Enum

    <Obsolete("use StartFinesseProcess with ExistingWindowBehavior instead of Boolean")>
    Public Sub StartFinesseProcess(ByVal ExeFileName As String, ByVal IfExistsBringToFront As Boolean, Optional ByVal CommandArgs As String = "", Optional ByVal ProcessName As String = "")
        StartFinesseProcess(ExeFileName, CommandArgs, If(IfExistsBringToFront, ExistingWindowBehavior.BringToFront, ExistingWindowBehavior.CreateNew), ProcessName)
    End Sub

    Private Function GetProcessByName(ByVal ProcessName As String) As System.Diagnostics.Process
        If String.IsNullOrEmpty(ProcessName) Then
            Return Nothing
        End If

        Dim directlyMatchingProcesses = Process.GetProcessesByName(ProcessName)

        Return directlyMatchingProcesses.
            Where(Function(p) p.SessionId = Process.GetCurrentProcess.SessionId).
            DefaultIfEmpty(Nothing).
            FirstOrDefault
    End Function

    Public Sub StartFinesseProcess(ByVal ExeFileName As String, Optional ByVal CommandArgs As String = "", Optional ByVal existingWindowAction As ExistingWindowBehavior = ExistingWindowBehavior.CreateNew, Optional ByVal ProcessName As String = "", Optional ByVal ProcessPath As String = "")
        Dim existingProcess As System.Diagnostics.Process = Nothing
        Dim exeFullPath As String = If(String.IsNullOrEmpty(ProcessPath), GetEnvironmentVariable("ESSVBDir") & "\" & ExeFileName, ProcessPath)

        If (existingWindowAction = ExistingWindowBehavior.BringToFront) Then
            Dim reEXEsuffixes As New System.Text.RegularExpressions.Regex("\.(exe|bat|com|cmd)$", RegularExpressions.RegexOptions.IgnoreCase)

            existingProcess = GetProcessByName(ProcessName)

            If existingProcess Is Nothing Then
                existingProcess = GetProcessByName(reEXEsuffixes.Replace(ExeFileName, ""))
            End If

            If existingProcess Is Nothing Then
                Dim redirectionTextFilePath = exeFullPath & ".txt"
                Dim redirectedExeName = ""

                Try
                    redirectedExeName = System.IO.File.ReadAllLines(redirectionTextFilePath)(0)
                Catch ex As Exception
                    ' lots of stuff can go wrong here; ignore all of it.
                End Try

                If redirectedExeName <> "" Then
                    existingProcess = GetProcessByName(reEXEsuffixes.Replace(redirectedExeName, ""))
                End If
            End If

        End If

        If existingProcess IsNot Nothing Then
            existingProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            ShowWindow(existingProcess.MainWindowHandle, ShowWindowConstants.SW_NORMAL)
            SetForegroundWindow(existingProcess.MainWindowHandle)
            ' This doesn't seem to work for minimized windows, but fixing that is an awful
            ' rabbit-hole of EnumWindows:
            ' http://www.pinvoke.net/default.aspx/user32.enumwindows
        Else
            Dim myNewProcess As New Process()
            myNewProcess.StartInfo.FileName = exeFullPath
            myNewProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            myNewProcess.StartInfo.Arguments = CommandArgs

            'Start Process
            myNewProcess.Start()
        End If

    End Sub

    'BAL - Added to control the behavior of comboxboxes when the mouse hovers over them and the scroll wheel is spun - add this handler to the mouse wheel
    '      event and it will cancel the event unless the dropdown is open
    '
    'USE - AddHandler cboEmpStatus.MouseWheel, AddressOf EmployeeMaintenanceGeneral.Combobox_MouseWheel
    Public Sub ScrollLockCombobox_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
        Dim passedCombo As ComboBox = DirectCast(sender, ComboBox)

        If passedCombo.DroppedDown = False Then
            mwe.Handled = True
        End If

    End Sub

    ' when calling this method put it at the end of the jammer load.
    'Public Sub RegisterFormTextChangedEventToAppendConnectionInformation(winform As Form, jammer As SQLJammer)
    '    jammer.Add("select dbo.fn_is_finesse_production_db()",
    '               Sub(t)
    '                   Dim isProductionDB As Boolean = t.Rows(0).Item(0)
    '                   If Not isProductionDB Then
    '                       AddHandler winform.TextChanged, AddressOf winform_TextChanged
    '                   End If
    '               End Sub)
    'End Sub

    'Private Sub winform_TextChanged(sender As Form, e As EventArgs)
    '    Static ChangingText = False

    '    Dim suffix = $" - Test Database: {GetEnvironmentVariable("ESSServer")}.{GetEnvironmentVariable("ESSDB")}"
    '    If Not sender.Text.EndsWith(suffix) Then
    '        If ChangingText Then
    '            Debug.Assert(False)
    '            Return
    '        End If

    '        ChangingText = True
    '        sender.Text = sender.Text & suffix
    '        ChangingText = False
    '    End If
    'End Sub
End Module