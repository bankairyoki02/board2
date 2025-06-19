Public Class HighlightTextBox
    Inherits System.Windows.Forms.TextBox

    Private _isEntering As Boolean
    Private _inMouseClick As Boolean
    Private _restrainOnTextChange As Boolean = False

    Private _textToClear As String
    Private _ForeColor As System.Drawing.Color
    Private _PromptForeColor As System.Drawing.Color = SystemColors.GrayText


    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control AndAlso (e.KeyCode = System.Windows.Forms.Keys.A) Then
            Me.SelectAll()
            e.SuppressKeyPress = True
            e.Handled = True
        Else
            MyBase.OnKeyDown(e)
        End If
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.ForeColor = If(MyBase.Text = _textToClear, _PromptForeColor, _ForeColor)

        If _restrainOnTextChange Then
            Exit Sub
        End If

        MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        _inMouseClick = True

        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        If _isEntering And MyBase.SelectionLength = 0 Then
            MyBase.SelectAll()
        End If
        _isEntering = False
        _inMouseClick = False

        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)

        _isEntering = True
        _restrainOnTextChange = True

        If Not _inMouseClick Then
            MyBase.SelectAll()
        End If
        If _textToClear = MyBase.Text Then
            MyBase.Text = ""
        End If

        _restrainOnTextChange = False

        MyBase.OnEnter(e)

    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        _restrainOnTextChange = True

        If MyBase.Text = "" Then
            MyBase.Text = _textToClear
        End If

        _restrainOnTextChange = False

        MyBase.OnLeave(e)

    End Sub

    ''' <summary>
    ''' The default text set and used when ClearDefaultText is set to true
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TextToClear() As String
        Get
            Return _textToClear
        End Get
        Set(ByVal value As String)
            _textToClear = value
            Me.Text = Me.Text
        End Set
    End Property

    Public Overloads Property Text() As String
        Get
            If MyBase.Text = _textToClear Then
                Return ""
            Else
                Return MyBase.Text
            End If
        End Get
        Set(ByVal value As String)
            MyBase.Text = If(value = "", _textToClear, value)
            MyBase.ForeColor = If(value = "", _PromptForeColor, _ForeColor)
        End Set
    End Property

    Public Overloads Property ForeColor As System.Drawing.Color
        Get
            Return _ForeColor
        End Get
        Set(value As System.Drawing.Color)
            _ForeColor = value

            If MyBase.Focused OrElse Not (MyBase.Text = _textToClear) Then
                MyBase.ForeColor = _ForeColor
            End If
        End Set
    End Property

    Public Sub New()
        MyBase.New()

        _ForeColor = MyBase.ForeColor
    End Sub
End Class

