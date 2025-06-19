Imports System.Windows.Forms

Public Class ConfirmandContinueDialog

    Public Sub New(ByVal DialogMessage As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Message = DialogMessage

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

#Region "Properties"

    Public Property Message() As String
        Get
            Message = txtMessage.Text
        End Get
        Set(ByVal value As String)
            txtMessage.Text = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Title = Me.Text
        End Get
        Set(ByVal value As String)
            Me.Text = value
        End Set
    End Property

    Public Property OK_ButtonText() As String
        Get
            OK_ButtonText = OK_Button.Text
        End Get
        Set(ByVal value As String)
            OK_Button.Text = value
        End Set
    End Property

    Public Property Cancel_ButtonText() As String
        Get
            Cancel_ButtonText = Cancel_Button.Text
        End Get
        Set(ByVal value As String)
            Cancel_Button.Text = value
        End Set
    End Property

    Public ReadOnly Property DontShowAgain() As Boolean
        Get
            DontShowAgain = chkDontShowAgain.Checked
        End Get
    End Property

#End Region

End Class
