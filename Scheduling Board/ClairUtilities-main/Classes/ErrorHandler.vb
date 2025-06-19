Imports System.Configuration
Imports System.IO

Public Class ErrorHandler
    Private errorDialog As Form

    Public Sub ShowNetFrameworkErrorMessage(e As ApplicationServices.UnhandledExceptionEventArgs)
        e.ExitApplication = False
        Dim ex = e.Exception

        Dim dialogTypeName As String = "System.Windows.Forms.PropertyGridInternal.GridErrorDlg"
        Dim dialogType As Type = GetType(Form).Assembly.GetType(dialogTypeName)

        errorDialog = DirectCast(Activator.CreateInstance(dialogType, New PropertyGrid()), Form)

        errorDialog.Text = "Finesse Unhandled Error"
        Dim errorMsg = "Unhandled exception has ocurred in your application.
If you click Continue, the application will ignore this error and attempt to continue.
If you click Quit, the application will close inmediately."

        errorMsg = $"{errorMsg} {Environment.NewLine} {Environment.NewLine} {ex.Message}"
        dialogType.GetProperty("Details").SetValue(errorDialog, ex.StackTrace, Nothing)
        dialogType.GetProperty("Message").SetValue(errorDialog, errorMsg, Nothing)

        Dim cancelButton As System.Windows.Forms.Button = errorDialog.Controls(1).Controls(0).Controls(0)
        Dim AcceptButton As System.Windows.Forms.Button = errorDialog.Controls(1).Controls(0).Controls(1)
        Dim Icon As PictureBox = errorDialog.Controls(1).Controls(1).Controls(1)
        cancelButton.Text = "Quit"
        AcceptButton.Text = "Continue"
        Icon.Image = SystemIcons.Error.ToBitmap()

        AddHandler cancelButton.Click, AddressOf CancelButton_Click
        AddHandler AcceptButton.Click, AddressOf AcceptButton_Click

        Dim result = errorDialog.ShowDialog()
    End Sub

    Public Sub HandleConfigCorruptionError(ex As Exception)
        Try
            If (My.Settings.PropertyValues.Count = 0) Then
                My.Settings.Reset()
            End If
        Catch e As ConfigurationException
            Dim filename = DirectCast(e.InnerException, ConfigurationException).Filename
            File.Delete(filename)
            My.Settings.Reload()
            MessageBox.Show("The application will be close automatically.
Please reopen, if the error persist please send an email to it@clairglobal.com", "Config file restored succesfully!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs)
        Environment.Exit(0)
    End Sub

    Private Sub AcceptButton_Click(sender As Object, e As EventArgs)
        errorDialog.Dispose()
    End Sub
End Class
