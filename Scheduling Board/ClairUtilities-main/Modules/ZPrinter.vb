Public Class ZPrinter
    Private _Name As String
    Private _Address As String
    Private _Port As Int16
    Private _currentPrinter As Boolean

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return _Address
        End Get
        Set(value As String)
            _Address = value
        End Set
    End Property

    Public Property Port() As String
        Get
            Return _Port
        End Get
        Set(value As String)
            _Port = value
        End Set
    End Property

    Public Property CurrentPrinter() As Boolean
        Get
            Return _currentPrinter
        End Get
        Set(value As Boolean)
            _currentPrinter = value
        End Set
    End Property

End Class
