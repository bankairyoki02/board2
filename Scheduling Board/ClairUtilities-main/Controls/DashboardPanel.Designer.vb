<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DashboardPanel
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pnlPastFuturePeriod = New System.Windows.Forms.Panel()
        Me.lblFutureResult = New System.Windows.Forms.Label()
        Me.lblPastResult = New System.Windows.Forms.Label()
        Me.pnlPrimaryQuery = New System.Windows.Forms.Panel()
        Me.lblResultPrimary = New System.Windows.Forms.Label()
        Me.flpSecondaryQuery = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblCaptionSecondary = New System.Windows.Forms.Label()
        Me.lblResultSecondary = New System.Windows.Forms.Label()
        Me.lblCaptionPrimary = New System.Windows.Forms.Label()
        Me.pnlPastFuturePeriod.SuspendLayout()
        Me.pnlPrimaryQuery.SuspendLayout()
        Me.flpSecondaryQuery.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlPastFuturePeriod
        '
        Me.pnlPastFuturePeriod.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.pnlPastFuturePeriod.Controls.Add(Me.lblFutureResult)
        Me.pnlPastFuturePeriod.Controls.Add(Me.lblPastResult)
        Me.pnlPastFuturePeriod.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlPastFuturePeriod.Location = New System.Drawing.Point(0, 108)
        Me.pnlPastFuturePeriod.Name = "pnlPastFuturePeriod"
        Me.pnlPastFuturePeriod.Size = New System.Drawing.Size(252, 35)
        Me.pnlPastFuturePeriod.TabIndex = 0
        '
        'lblFutureResult
        '
        Me.lblFutureResult.AutoEllipsis = True
        Me.lblFutureResult.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblFutureResult.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFutureResult.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblFutureResult.Location = New System.Drawing.Point(121, 0)
        Me.lblFutureResult.Name = "lblFutureResult"
        Me.lblFutureResult.Size = New System.Drawing.Size(131, 35)
        Me.lblFutureResult.TabIndex = 3
        Me.lblFutureResult.Text = "Past Result"
        Me.lblFutureResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPastResult
        '
        Me.lblPastResult.AutoEllipsis = True
        Me.lblPastResult.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblPastResult.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPastResult.ForeColor = System.Drawing.Color.Red
        Me.lblPastResult.Location = New System.Drawing.Point(0, 0)
        Me.lblPastResult.Name = "lblPastResult"
        Me.lblPastResult.Size = New System.Drawing.Size(131, 35)
        Me.lblPastResult.TabIndex = 2
        Me.lblPastResult.Text = "Past Result"
        Me.lblPastResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlPrimaryQuery
        '
        Me.pnlPrimaryQuery.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.pnlPrimaryQuery.Controls.Add(Me.lblResultPrimary)
        Me.pnlPrimaryQuery.Controls.Add(Me.flpSecondaryQuery)
        Me.pnlPrimaryQuery.Controls.Add(Me.lblCaptionPrimary)
        Me.pnlPrimaryQuery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlPrimaryQuery.Location = New System.Drawing.Point(0, 0)
        Me.pnlPrimaryQuery.Name = "pnlPrimaryQuery"
        Me.pnlPrimaryQuery.Size = New System.Drawing.Size(252, 108)
        Me.pnlPrimaryQuery.TabIndex = 1
        '
        'lblResultPrimary
        '
        Me.lblResultPrimary.AutoEllipsis = True
        Me.lblResultPrimary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblResultPrimary.Font = New System.Drawing.Font("Arial Narrow", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResultPrimary.Location = New System.Drawing.Point(0, 24)
        Me.lblResultPrimary.Name = "lblResultPrimary"
        Me.lblResultPrimary.Size = New System.Drawing.Size(252, 60)
        Me.lblResultPrimary.TabIndex = 1
        Me.lblResultPrimary.Text = "Primary Result"
        Me.lblResultPrimary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'flpSecondaryQuery
        '
        Me.flpSecondaryQuery.BackColor = System.Drawing.Color.White
        Me.flpSecondaryQuery.Controls.Add(Me.lblCaptionSecondary)
        Me.flpSecondaryQuery.Controls.Add(Me.lblResultSecondary)
        Me.flpSecondaryQuery.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.flpSecondaryQuery.Location = New System.Drawing.Point(0, 84)
        Me.flpSecondaryQuery.Name = "flpSecondaryQuery"
        Me.flpSecondaryQuery.Size = New System.Drawing.Size(252, 24)
        Me.flpSecondaryQuery.TabIndex = 2
        '
        'lblCaptionSecondary
        '
        Me.lblCaptionSecondary.AutoSize = True
        Me.lblCaptionSecondary.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblCaptionSecondary.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaptionSecondary.Location = New System.Drawing.Point(3, 0)
        Me.lblCaptionSecondary.Name = "lblCaptionSecondary"
        Me.lblCaptionSecondary.Size = New System.Drawing.Size(102, 16)
        Me.lblCaptionSecondary.TabIndex = 1
        Me.lblCaptionSecondary.Text = "Secondary Caption "
        Me.lblCaptionSecondary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblResultSecondary
        '
        Me.lblResultSecondary.AutoEllipsis = True
        Me.lblResultSecondary.AutoSize = True
        Me.lblResultSecondary.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblResultSecondary.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResultSecondary.Location = New System.Drawing.Point(111, 0)
        Me.lblResultSecondary.Name = "lblResultSecondary"
        Me.lblResultSecondary.Size = New System.Drawing.Size(102, 16)
        Me.lblResultSecondary.TabIndex = 2
        Me.lblResultSecondary.Text = "Secondary Result"
        Me.lblResultSecondary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCaptionPrimary
        '
        Me.lblCaptionPrimary.AutoEllipsis = True
        Me.lblCaptionPrimary.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblCaptionPrimary.Font = New System.Drawing.Font("Arial Narrow", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaptionPrimary.Location = New System.Drawing.Point(0, 0)
        Me.lblCaptionPrimary.Name = "lblCaptionPrimary"
        Me.lblCaptionPrimary.Size = New System.Drawing.Size(252, 24)
        Me.lblCaptionPrimary.TabIndex = 0
        Me.lblCaptionPrimary.Text = "Primary Caption "
        Me.lblCaptionPrimary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DashboardPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlPrimaryQuery)
        Me.Controls.Add(Me.pnlPastFuturePeriod)
        Me.Name = "DashboardPanel"
        Me.Size = New System.Drawing.Size(252, 143)
        Me.pnlPastFuturePeriod.ResumeLayout(False)
        Me.pnlPrimaryQuery.ResumeLayout(False)
        Me.flpSecondaryQuery.ResumeLayout(False)
        Me.flpSecondaryQuery.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlPastFuturePeriod As Panel
    Friend WithEvents pnlPrimaryQuery As Panel
    Friend WithEvents lblFutureResult As Label
    Friend WithEvents lblPastResult As Label
    Friend WithEvents lblResultPrimary As Label
    Friend WithEvents lblCaptionPrimary As Label
    Friend WithEvents flpSecondaryQuery As FlowLayoutPanel
    Friend WithEvents lblCaptionSecondary As Label
    Friend WithEvents lblResultSecondary As Label
End Class
