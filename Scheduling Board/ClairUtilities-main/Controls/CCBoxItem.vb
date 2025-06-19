Public Class CCBoxItem
    Private _val As String
    Public Property Value() As String
        Get
            Return _val
        End Get
        Set(ByVal value As String)
            _val = value
        End Set
    End Property

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal name As String, ByVal val As String)
        _name = name
        _val = val
    End Sub

    Public Overrides Function ToString() As String
        'Return String.Format("name: '{0}', value: {1}", _name, _val)
        Return String.Format("{0}", _name)
    End Function
End Class
