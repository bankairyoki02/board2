Imports System.Windows.Forms.Design

<ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip), DebuggerStepThrough()>
Public Class ToolStripCheckBox
    Inherits ToolStripControlHost

    Public Sub New()
        MyBase.New(New CheckBox())
        ToolStripCheckBoxControl.BackColor = Color.Transparent
    End Sub

    Public ReadOnly Property ToolStripCheckBoxControl() As CheckBox
        Get
            Return TryCast(Control, CheckBox)
        End Get
    End Property

    Public Property ToolStripCheckBoxEnabled() As Boolean
        Get
            Return ToolStripCheckBoxControl.Enabled
        End Get
        Set(ByVal value As Boolean)
            ToolStripCheckBoxControl.Enabled = value
        End Set
    End Property

    Public Property Checked() As Boolean
        Get
            Return ToolStripCheckBoxControl.Checked
        End Get
        Set(ByVal value As Boolean)
            ToolStripCheckBoxControl.Checked = value
        End Set
    End Property

    Protected Overrides Sub OnSubscribeControlEvents(ByVal c As Control)
        MyBase.OnSubscribeControlEvents(c)
        AddHandler DirectCast(c, CheckBox).CheckedChanged, AddressOf OnCheckedChanged
    End Sub

    Protected Overrides Sub OnUnsubscribeControlEvents(ByVal c As Control)
        MyBase.OnUnsubscribeControlEvents(c)
        RemoveHandler DirectCast(c, CheckBox).CheckedChanged, AddressOf OnCheckedChanged
    End Sub

    Public Event CheckedChanged As EventHandler

    Private Sub OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent CheckedChanged(Me, e)
    End Sub
End Class