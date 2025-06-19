Module Email

    Public Sub CreateEmailMessage(ByVal toEmailAddress As String, Optional ByVal subject As String = "", Optional ByVal messageBody As String = "")

        Dim mailto = "mailto:" & Uri.EscapeDataString(toEmailAddress)

        If Not String.IsNullOrEmpty(subject.Trim) Then
            mailto &= "&subject=" & Uri.EscapeDataString(subject)
        End If

        If Not String.IsNullOrEmpty(messageBody.Trim) Then
            mailto &= "&body=" & Uri.EscapeDataString(messageBody)
        End If

        Process.Start(mailto)

    End Sub

End Module
