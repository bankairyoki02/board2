<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProjectPicker
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lvProjects = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblNoData = New System.Windows.Forms.Label()
        Me.lblEnterSearch = New System.Windows.Forms.Label()
        Me.tmrDelayedSearch = New System.Windows.Forms.Timer(Me.components)
        Me.txtSearch = New HighlightTextBox()
        Me.SuspendLayout()
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(6, 9)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(59, 13)
        Me.lblSearch.TabIndex = 1
        Me.lblSearch.Text = "&Search for:"
        '
        'btnOk
        '
        Me.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOk.Enabled = False
        Me.btnOk.Location = New System.Drawing.Point(258, 475)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 7
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(339, 475)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lvProjects
        '
        Me.lvProjects.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvProjects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvProjects.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvProjects.FullRowSelect = True
        Me.lvProjects.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvProjects.HideSelection = False
        Me.lvProjects.Location = New System.Drawing.Point(0, 53)
        Me.lvProjects.MultiSelect = False
        Me.lvProjects.Name = "lvProjects"
        Me.lvProjects.Size = New System.Drawing.Size(670, 412)
        Me.lvProjects.TabIndex = 6
        Me.lvProjects.UseCompatibleStateImageBehavior = False
        Me.lvProjects.View = System.Windows.Forms.View.Details
        Me.lvProjects.Visible = False
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Project Number"
        Me.ColumnHeader1.Width = 130
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Project Description"
        Me.ColumnHeader2.Width = 537
        '
        'lblNoData
        '
        Me.lblNoData.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblNoData.AutoSize = True
        Me.lblNoData.Location = New System.Drawing.Point(262, 240)
        Me.lblNoData.Name = "lblNoData"
        Me.lblNoData.Size = New System.Drawing.Size(137, 13)
        Me.lblNoData.TabIndex = 9
        Me.lblNoData.Text = "No matching projects found"
        Me.lblNoData.Visible = False
        '
        'lblEnterSearch
        '
        Me.lblEnterSearch.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblEnterSearch.AutoSize = True
        Me.lblEnterSearch.Location = New System.Drawing.Point(223, 253)
        Me.lblEnterSearch.Name = "lblEnterSearch"
        Me.lblEnterSearch.Size = New System.Drawing.Size(215, 13)
        Me.lblEnterSearch.TabIndex = 10
        Me.lblEnterSearch.Text = "Enter a project description or project number"
        '
        'tmrDelayedSearch
        '
        Me.tmrDelayedSearch.Enabled = True
        Me.tmrDelayedSearch.Interval = 200
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(15, 26)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(460, 20)
        Me.txtSearch.TabIndex = 11
        Me.txtSearch.TextToClear = Nothing
        '
        'frmProjectPicker
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(671, 500)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblEnterSearch)
        Me.Controls.Add(Me.lblNoData)
        Me.Controls.Add(Me.lvProjects)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblSearch)
        Me.Name = "frmProjectPicker"
        Me.Text = "Project Search"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lvProjects As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblNoData As System.Windows.Forms.Label
    Friend WithEvents lblEnterSearch As System.Windows.Forms.Label
    Friend WithEvents txtSearch As HighlightTextBox
    Friend WithEvents tmrDelayedSearch As System.Windows.Forms.Timer
End Class
