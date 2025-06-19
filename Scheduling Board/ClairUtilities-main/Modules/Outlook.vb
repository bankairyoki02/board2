Imports Microsoft.Office.Interop.Outlook

Module Outlook

    Public Sub OpenOutlookMail(ByVal ToAddress As String, Optional ByVal Subject As String = "", Optional ByVal Body As String = "", Optional ByVal AttachmentPath As String = Nothing, Optional HtmlBody As Boolean = False, Optional CCAddress As String = "")
        Dim attachmentPaths As New List(Of String)

        If AttachmentPath IsNot Nothing Then
            attachmentPaths.Add(AttachmentPath)
        End If

        OpenOutlookMail(ToAddress, Subject, Body, attachmentPaths, HtmlBody, CCAddress)
    End Sub

    Public Sub OpenOutlookMail(ByVal ToAddress As String, Optional ByVal Subject As String = "", Optional ByVal Body As String = "", Optional ByVal AttachmentPaths As IEnumerable(Of String) = Nothing, Optional HtmlBody As Boolean = False, Optional CCAddress As String = "")
        Dim objOutlook As New Application
        Dim oMail As MailItem

        oMail = objOutlook.CreateItem(OlItemType.olMailItem)
        oMail.Display()
        oMail.To = ToAddress
        oMail.CC = CCAddress
        oMail.Subject = Subject
        If HtmlBody Then
            oMail.HTMLBody = Body & oMail.HTMLBody
        Else
            oMail.Body = Body
        End If

        If AttachmentPaths IsNot Nothing Then
            For Each AttachmentPath In AttachmentPaths
                If AttachmentPath IsNot Nothing AndAlso AttachmentPath IsNot String.Empty Then
                    oMail.Attachments.Add(AttachmentPath)
                End If
            Next
        End If
    End Sub

    Public Sub OpenOutlookMailHTMLBody(ByVal ToAddress As String, Optional ByVal Subject As String = "", Optional ByVal Body As String = "", Optional ByVal AttachmentPath As String = "", Optional CCAddress As String = "")
        Dim objOutlook As New Application
        Dim oMail As MailItem

        oMail = objOutlook.CreateItem(OlItemType.olMailItem)
        oMail.Display()
        oMail.To = ToAddress
        oMail.CC = ToAddress
        oMail.Subject = Subject
        oMail.HTMLBody = Body & oMail.HTMLBody
        If Not String.IsNullOrWhiteSpace(AttachmentPath) Then
            oMail.Attachments.Add(AttachmentPath)
        End If

    End Sub

End Module
