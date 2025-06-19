Imports System.Data.SqlClient
Imports System.Text

Module FinesseUserUtilities
    Public Function IsInRole(ByRef conn As SqlConnection, ByVal ParamArray roleNames() As String) As Boolean
        IsInRole = False
        Dim sqlStr As New StringBuilder

        sqlStr.AppendLine("select IsInRole = ")
        Dim foo = New Converter(Of String, String)(Function(s As String) "convert(bit, isnull(is_member(" & s.SQLQuote() & "), 0))")
        Dim roleSearchStr As String = String.Join(" | ", Array.ConvertAll(roleNames, foo))
        sqlStr.AppendLine(roleSearchStr)

        IsInRole = CBool(conn.ExecuteScalar(sqlStr.ToString()))
    End Function

    Public Function Get_AppConfigSetting(ByVal AppConfig As DataTable, ByVal Tag As String)
        If AppConfig Is Nothing Then
            Return Nothing
        End If

        Dim result = AppConfig.Select("Tag=" & SQLQuote(Tag))

        If result.Length = 1 Then
            Return result(0)("Value")
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Get the SUSER_SNAME() and @@spid for the current SQL Session
    ''' </summary>
    ''' <param name="conn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Obsolete("This property should not be used anymore.")>
    Public Function GetUserInformation(ByRef conn As SqlConnection) As DataTable
        Dim sqlStr As New StringBuilder
        sqlStr.AppendLine("select SUSER_SNAME = SUSER_SNAME(), SPID = @@spid")

        GetUserInformation = conn.GetDataTable(sqlStr)
        GetUserInformation.TableName = "UserInformation"
    End Function

    <Obsolete("This property should not be used anymore.")>
    Public ReadOnly Property UserWarehouse(ByVal Conn As SqlConnection) As String

        Get
            Static myUserWarehouse As String = String.Empty
            If (String.IsNullOrEmpty(myUserWarehouse)) Then
                Dim dbCmd As New SqlClient.SqlCommand("select warehouse_entity from pjtfrusr where [user_name]=suser_sname()", Conn)
                myUserWarehouse = dbCmd.ExecuteScalar()
            End If
            UserWarehouse = myUserWarehouse
        End Get

    End Property

    <Obsolete("This property should not be used anymore.")>
    Public ReadOnly Property UserIDLoggedIn(ByVal Conn As SqlConnection)
        Get
            'Return GetEnvironmentVariable("ESSUID")
            Static myUserID As String = String.Empty
            If (String.IsNullOrEmpty(myUserID)) Then
                Dim dbCmd As New SqlClient.SqlCommand("select suser_sname()", Conn)
                myUserID = dbCmd.ExecuteScalar()
            End If
            UserIDLoggedIn = myUserID
        End Get
    End Property

    Public Class FinessePermissions
        Public Const QueryString As String =
            "select can_edit_parts = dbo.can_edit_parts()" & vbNewLine &
            ", can_see_bids = dbo.can_see_bids()" & vbNewLine &
            ", can_see_part_prices = dbo.can_see_part_prices()"

        Private Sub New()
        End Sub

        Private _CanEditParts As Boolean
        Public ReadOnly Property CanEditParts As Boolean
            Get
                Return _CanEditParts
            End Get
        End Property

        Private _CanSeePartPrices As Boolean
        Public ReadOnly Property CanSeePartPrices As Boolean
            Get
                Return _CanSeePartPrices
            End Get
        End Property

        Private _CanSeeBids As Boolean
        Public ReadOnly Property CanSeeBids As Boolean
            Get
                Return _CanSeeBids
            End Get
        End Property

        Public Sub New(ByVal dtPermissions As DataTable)
            _CanEditParts = dtPermissions.Rows(0)("can_edit_parts")
            _CanSeePartPrices = dtPermissions.Rows(0)("can_see_part_prices")
            _CanSeeBids = dtPermissions.Rows(0)("can_see_bids")
        End Sub
    End Class
End Module
