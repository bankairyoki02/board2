Public Class frmPopOut
    Private Sub frmPopOut_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            With frmSchedulingBoard
                ._PopoutFormSize = Me.Size
                ._PopoutFormLocation = Me.Location
                ._PopoutFormWindowState = Me.WindowState
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Do While Me.Controls.Count > 0
            Me.Controls(Me.Controls.Count - 1).Parent = frmSchedulingBoard.scPartsAndDetail.Panel2
        Loop

        Me.Visible = False
        frmSchedulingBoard.lblPopOut.Visible = True
        frmSchedulingBoard.lblCloseDetailPane.Visible = True

        frmSchedulingBoard.dgvDetail.BringToFront()
        frmSchedulingBoard.scPartsAndDetail.Panel2Collapsed = False

        If Me.Visible Then
            e.Cancel = True
        End If

    End Sub

    Private Sub frmPopOut_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        Select Case Get_AppConfigSetting(frmSchedulingBoard.AppConfig, "PopoutFormWindowState")
            Case FormWindowState.Maximized
                Dim PopoutFormLocation = Get_AppConfigSetting(frmSchedulingBoard.AppConfig, "PopoutFormLocation")
                If Not String.IsNullOrEmpty(PopoutFormLocation) Then Me.DesktopLocation = Get_LocationFromString(PopoutFormLocation)
                Me.WindowState = FormWindowState.Maximized
            Case FormWindowState.Normal
                Me.WindowState = FormWindowState.Normal
                Dim PopoutFormLocation = Get_AppConfigSetting(frmSchedulingBoard.AppConfig, "PopoutFormLocation")
                If Not String.IsNullOrEmpty(PopoutFormLocation) Then Me.DesktopLocation = Get_LocationFromString(PopoutFormLocation)
                Dim PopoutFormSize = Get_AppConfigSetting(frmSchedulingBoard.AppConfig, "PopoutFormSize")
                If Not String.IsNullOrEmpty(PopoutFormSize) Then Me.Size = Get_SizeFromString(PopoutFormSize)
        End Select
    End Sub

End Class