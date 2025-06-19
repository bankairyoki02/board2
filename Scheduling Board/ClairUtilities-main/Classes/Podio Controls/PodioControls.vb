Imports PodioAPI

Public Class PodioControls
    Inherits Control

    Public Property myPodio As Podio

    Private _clientId As String
    Private _clientSecret As String

    Public Property clientId As String
        Get
            clientId = _clientId
        End Get
        Set(value As String)
            _clientId = value
        End Set
    End Property

    Public Property clientSecret As String
        Get
            clientSecret = _clientSecret
        End Get
        Set(value As String)
            _clientSecret = value
        End Set
    End Property

    Public Sub New()
        Initialize()
    End Sub

    Public Sub New(ByVal clientId As String, ByVal clientSecret As String)
        MyBase.New()

        _clientId = clientId
        _clientSecret = clientSecret

        Initialize()

    End Sub

    Private Sub Initialize()

        _clientId = clientId
        _clientSecret = clientSecret

        Dim foo = Me

        myPodio = New Podio(clientId, clientSecret)
    End Sub

End Class

Public Class PodioTextBox
    Inherits System.Windows.Forms.TextBox

    Public Property myPodioControls As PodioControls

    Private _appId As String
    Private _itemId As String
    Private _fieldId As String

    Public Property appId As String
        Get
            appId = _appId
        End Get
        Set(value As String)
            _appId = value
        End Set
    End Property

    Public Property itemId As String
        Get
            itemId = _itemId
        End Get
        Set(value As String)
            _itemId = value
        End Set
    End Property

    Public Property fieldId As String
        Get
            fieldId = _fieldId
        End Get
        Set(value As String)
            _fieldId = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal appId As String, ByVal itemId As String, fieldId As String)
        MyBase.New()

        _appId = appId
        _itemId = itemId
        _fieldId = fieldId



    End Sub

End Class
