Imports System.Data.SqlClient

''' <summary>
''' Using Singleton to get base user information
''' e.i: User.Instance.UserName
''' </summary>
Public Class UserSingleton
#Region "Singleton"
    Private Shared _instance As UserSingleton

    ''' <summary>
    ''' Initialize the Singleton instance, ensuring there is only one instance.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property Instance() As UserSingleton
        Get
            If _instance Is Nothing Then
                _instance = New UserSingleton()
            End If
            Return _instance
        End Get
    End Property
#End Region

    'TODO: Add all the neccesary properties for a Finesse User
#Region "Properties"
    Private ReadOnly InitializationErrorMsg = "Singleton not initialize, please call User.Instance to initialize the object"

    'TODO: Add visibility permissions, meaning those visibility tables for diffrent actions
    'TODO: Add any type of permissions
    'TODO: Handle dbo.fn_user_menus
    Private _userName As String
    ''' <summary>
    ''' User database username
    ''' See SUSER_NAME() function in the database
    ''' </summary>
    ''' <returns></returns>
    Public Property UserName() As String
        Get
            If (String.IsNullOrEmpty(_userName)) Then
                Throw New InvalidOperationException(InitializationErrorMsg)
            End If
            Return _userName
        End Get
        Private Set(ByVal value As String)
            _userName = value
        End Set
    End Property

    Private _databaseRoles As String()
    ''' <summary>
    ''' String containing all the databse roles
    ''' See dbo.GetUserRolesString function in the Database
    ''' </summary>
    ''' <returns>Comma ',' sepparated string of all the database Roles that the user contains</returns>
    Public Property DatabaseRoles() As String()
        Get
            If (_databaseRoles Is Nothing) Then
                Throw New InvalidOperationException(InitializationErrorMsg)
            End If
            Return _databaseRoles
        End Get
        Private Set(ByVal value As String())
            _databaseRoles = value
        End Set
    End Property

    Public ReadOnly Property ContainsRole(role As String) As Boolean
        Get
            If (_databaseRoles Is Nothing) Then
                Throw New InvalidOperationException(InitializationErrorMsg)
            End If
            Return _databaseRoles.Contains(role)
        End Get
    End Property
#End Region

    Private Sub New()
        getUserBaseInfo()
    End Sub


#Region "Functions"
    Private Sub getUserBaseInfo()
        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))

        jammer.Add("select username     = SUSER_NAME()
                    , userRoles     = dbo.GetUserRolesString(NULL)
                        ",
                   Sub(t)
                       Dim result = t.Rows(0)
                       Me.UserName = result.Item("username")
                       Dim roles = result.Item("userRoles")
                       If (roles IsNot Nothing) Then
                           Me.DatabaseRoles = roles.ToString.Split(",")
                       End If
                   End Sub)

        jammer.Execute()
    End Sub
#End Region

End Class
