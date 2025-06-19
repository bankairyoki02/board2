Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Public Module ComboboxUtilities
    <Extension()> _
    Public Sub Repopulate(ByVal cbo As ComboBox, ByVal t As DataView)

        Dim OldSelectedValue As Object = cbo.SelectedValue

        cbo.DataSource = t

        If OldSelectedValue IsNot Nothing Then
            cbo.SelectedValue = OldSelectedValue
        End If

        If cbo.SelectedValue Is Nothing Then
            cbo.SelectedIndex = 0
        End If

    End Sub

    <Extension()> _
    Public Sub Repopulate(ByVal cbo As ComboBox, ByVal t As DataTable)

        Dim OldSelectedValue As Object = cbo.SelectedValue

        cbo.DataSource = t

        If OldSelectedValue IsNot Nothing Then
            cbo.SelectedValue = OldSelectedValue
        End If

        If cbo.SelectedValue Is Nothing Then
            cbo.SelectedIndex = 0
        End If

    End Sub

    <Extension()> _
    Public Function SelectItemOrFirst(ByVal cb As ComboBox, ByVal textToSelect As Object) As Boolean

        SelectItemOrFirst = cb.Items.Contains(textToSelect)
        If SelectItemOrFirst Then
            cb.SelectedValue = textToSelect
        ElseIf cb.Items.Count > 0 Then
            cb.SelectedIndex = 0
        End If

    End Function
End Module
