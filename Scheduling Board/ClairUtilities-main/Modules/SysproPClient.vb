Imports System.Data.SqlClient
Imports System.IO.Compression

''' <summary>
''' Pass a server and port on construction and it will setup an inherited Syspro primitive client
''' Removes need to reference syspro assemblies in main app
''' Adds overloaded logon function that will retrieve the UserId from a previous syspro desktop connection.
''' Needs reference to 'C:\SYSPROClient\Base\ManagedAssemblies\SYSPROWCFServicesClientLibrary40.dll'
''' </summary>
Public Class SysproPClient
    Inherits SYSPROWCFServicesClientLibrary40.SYSPROWCFServicesPrimitiveClient

    Sub New(WCFServer As String, WCFPort As Int16)
        MyBase.New($"net.tcp://{WCFServer}:{WCFPort}/SYSPROWCFService", SYSPROWCFServicesClientLibrary40.SYSPROWCFBinding.NetTcp)
    End Sub

#Region "Syspro Connections"
    ''' <summary>
    ''' Logon using existing Syspro desktop client connection. User must have previously connected to the desktop client using the specified connection string parameters.
    ''' </summary>
    Public Overloads Function Logon(OperatorName As String, ConnectionString As String) As String

        Try
            Dim userId = GetSysproUserID(OperatorName, ConnectionString)

            Return userId
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return ""

    End Function

    ''' <summary>
    ''' Retrieves a UserId guid for use directly in Syspro WCF business object calls.
    ''' </summary>
    Public Function GetSysproUserID(OperatorName As String, ConnectionString As String) As String
        Dim conn = New SqlConnection(ConnectionString)
        Dim company = conn.Database.Replace("Syspro", "") 'is it safe to assume the company name is tied directly to the database name?

        'Handling different admin database naming between dev and prod servers. If this ever gets fixed just remove the if statement and use the first query.
        Dim sqlStr As String

        If conn.DataSource = "LITZ-SYSPRODB" Then
            'production
            sqlStr = $"
                select ast.UserId, ast.Store1
                from Sysprodb.dbo.AdmState ast
                where ast.ClassOperator = '{OperatorName}'
                and ast.ClassFlag = 'T'
            "
        Else
            'dev
            sqlStr = $"
                select ast.UserId, ast.Store1
                from SysproDevDb.dbo.AdmState ast
                where ast.ClassOperator = '{OperatorName}'
                and ast.ClassFlag = 'T'
            "
        End If

        'get the operators Admstate data
        Dim dt = conn.GetDataTable(sqlStr)
        conn.Close()

        'iterate through Admstate data to find a record that contains the required company inside the encoded and compressed Store1 field. Then return the UserId connection GUID from that record.
        For Each row In dt.Rows
            'Debug.Print(row!Store1)

            Dim bt As Byte() = Convert.FromBase64String(row!Store1)
            Dim ms As System.IO.MemoryStream = New IO.MemoryStream(bt)
            Dim ds As DeflateStream = New DeflateStream(ms, CompressionMode.Decompress)

            Dim xb(1048576) As Byte '1MB - source field is varchar(max). If something fails here try using a larger byte array
            'gets the first chunk of the string that holds the company value
            'address of company value found by experimentation. Not likely to be changed by Syspro but who knows
            Dim readBytes = ds.Read(xb, 0, 7195)
            'converts the byte array to a string and grabs the company from the end
            Dim decodedCompany = System.Text.Encoding.ASCII.GetString(xb).Substring(7191, 4).TrimEnd()

            Debug.Print(decodedCompany)
            If decodedCompany = company Then
                Debug.Print(row!UserId)
                Return row!UserId
            End If
        Next

        Return ""

    End Function
#End Region
End Class
