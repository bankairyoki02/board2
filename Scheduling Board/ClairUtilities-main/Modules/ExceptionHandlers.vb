Module ExceptionHandlers

    ' Handle the UI exceptions by showing a dialog box, and asking the user whether
    ' or not they wish to abort execution.
    Public Sub UIThreadException(ByVal sender As Object, ByVal t As System.Threading.ThreadExceptionEventArgs)
        Dim result As System.Windows.Forms.DialogResult =
             System.Windows.Forms.DialogResult.Cancel
        Try
            result = ShowThreadExceptionDialog("Windows Forms Error", t.Exception)
        Catch
            Try
                MessageBox.Show("Fatal Windows Forms Error",
                     "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop)
            Finally
                Application.Exit()
            End Try
        End Try

        ' Exits the program when the user clicks Abort.
        If result = DialogResult.Abort Then
            Application.Exit()
        End If
    End Sub

    ' Handle the UI exceptions by showing a dialog box, and asking the user whether
    ' or not they wish to abort execution.
    ' NOTE: This exception cannot be kept from terminating the application - it can only 
    ' log the event, and inform the user about it. 
    Public Sub CurrentDomain_UnhandledException(ByVal sender As Object,
     ByVal e As UnhandledExceptionEventArgs)
        Try
            Dim ex As Exception = CType(e.ExceptionObject, Exception)
            Dim errorMsg As String = "An application error occurred. Please contact the adminstrator " &
                 "with the following information:" & ControlChars.Lf & ControlChars.Lf

            ' Since we can't prevent the app from terminating, log this to the event log.
            If (Not EventLog.SourceExists("ThreadException")) Then
                EventLog.CreateEventSource("ThreadException", "Application")
            End If

            ' Create an EventLog instance and assign its source.
            Dim myLog As New EventLog()
            myLog.Source = "ThreadException"
            myLog.WriteEntry((errorMsg + ex.Message & ControlChars.Lf & ControlChars.Lf &
                 "Stack Trace:" & ControlChars.Lf & ex.StackTrace))
        Catch exc As Exception
            Try
                MessageBox.Show("Fatal Non-UI Error", "Fatal Non-UI Error. Could not write the error to the event log. " &
                     "Reason: " & exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Finally
                Application.Exit()
            End Try
        End Try
    End Sub


    ' Creates the error message and displays it.
    Private Function ShowThreadExceptionDialog(ByVal title As String, ByVal e As Exception) As DialogResult
        Dim errorMsg As String = "An application error occurred. Please contact the adminstrator with the following information:" & ControlChars.Lf & ControlChars.Lf
        errorMsg = errorMsg & e.Message & ControlChars.Lf & ControlChars.Lf & "Stack Trace:" & ControlChars.Lf & e.StackTrace

        Return MessageBox.Show(errorMsg, title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop)
    End Function

End Module
