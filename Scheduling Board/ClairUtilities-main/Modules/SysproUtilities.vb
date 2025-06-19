Imports System.Data.SqlClient

Module SysproUtilities

    Private BaseAddress As String = "net.tcp://litz-sysprodev:31001/SYSPROWCFService"
    'Private sysproBinding As SYSPROWCFServicesClientLibrary40.SYSPROWCFBinding = SYSPROWCFServicesClientLibrary40.SYSPROWCFBinding.NetTcp
    'Private sysproClient As SYSPROWCFServicesClientLibrary40.SYSPROWCFServicesPrimitiveClient

    Public UserId As String

    'Public Function SysproLogon(ByVal OperatorName As String, Password As String, ByVal CompanyId As String) As Boolean

    '    Try
    '        If sysproClient Is Nothing Then
    '            sysproClient = New SYSPROWCFServicesClientLibrary40.SYSPROWCFServicesPrimitiveClient(BaseAddress, sysproBinding)
    '        End If

    '        UserId = sysproClient.Logon(OperatorName, Password, CompanyId, CompanyPassword:=CompanyId)
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try

    '    Return True

    'End Function

    'Public ReadOnly Property SysproConnectionString() As String
    '    Get
    '        With New SqlConnectionStringBuilder()

    '        End With
    '    End Get
    'End Property


    'Public ReadOnly Property FinesseConnectionString(Optional timeoutSeconds As Integer = 10, Optional username As String = "", Optional password As String = "", Optional database As String = "", Optional server As String = "") As String
    '    Get
    '        With New SqlConnectionStringBuilder()

    '            .DataSource = IIf(String.IsNullOrEmpty(server), GetEnvironmentVariable("ESSSERVER"), "ClairSQL.clair.lcl")
    '            .InitialCatalog = IIf(String.IsNullOrEmpty(database), GetEnvironmentVariable("ESSDB"), database)
    '            .UserID = IIf(String.IsNullOrEmpty(username), GetEnvironmentVariable("ESSUID"), username)
    '            .Password = IIf(String.IsNullOrEmpty(password), GetEnvironmentVariable("ESSPWD"), password)
    '            .ApplicationName = My.Application.Info.AssemblyName
    '            .AsynchronousProcessing = True
    '            .PersistSecurityInfo = True
    '            .ConnectTimeout = timeoutSeconds

    '            FinesseConnectionString = .ConnectionString
    '        End With
    '    End Get
    'End Property

        Public Function GetOpenedSysProConnection(connectionString As String) As SqlConnection
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Return conn
        End Function


    'Public Function SysproTransactionPost(ByVal BusinessObject As String, ByVal XmlParameters As String, ByVal XmlIn As String) As String

    '    Dim XmlOut As String = Nothing

    '    Try
    '        XmlOut = sysproClient.TransactionPost(UserId, BusinessObject, XmlParameters, XmlIn)
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try



    '    Return XmlOut

    'End Function




End Module
