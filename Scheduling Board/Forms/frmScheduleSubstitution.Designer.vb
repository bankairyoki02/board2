<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmScheduleSubstitution
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmScheduleSubstitution))
        Me.lblProjectNo = New System.Windows.Forms.Label()
        Me.lblProjectDesc = New System.Windows.Forms.Label()
        Me.lblOrderedQty = New System.Windows.Forms.Label()
        Me.lblQtyLoaded = New System.Windows.Forms.Label()
        Me.lblPartNo = New System.Windows.Forms.Label()
        Me.txtOrderedQty = New System.Windows.Forms.TextBox()
        Me.txtLoadedQty = New System.Windows.Forms.TextBox()
        Me.cboMultiPartList = New System.Windows.Forms.ComboBox()
        Me.pnlSplit = New System.Windows.Forms.Panel()
        Me.btnSplitPartSearch = New System.Windows.Forms.Button()
        Me.txtSplitPartNo = New System.Windows.Forms.TextBox()
        Me.cboSplitMultipartList = New System.Windows.Forms.ComboBox()
        Me.txtSplitOrderedQty = New System.Windows.Forms.TextBox()
        Me.txtSplitLoadedQty = New System.Windows.Forms.TextBox()
        Me.txtPartNo = New System.Windows.Forms.TextBox()
        Me.btnPartSearch = New System.Windows.Forms.Button()
        Me.lblPartDesc = New System.Windows.Forms.Label()
        Me.chkSplit = New System.Windows.Forms.CheckBox()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnCommit = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.pnlSplit.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblProjectNo
        '
        Me.lblProjectNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProjectNo.Location = New System.Drawing.Point(12, 9)
        Me.lblProjectNo.Name = "lblProjectNo"
        Me.lblProjectNo.Size = New System.Drawing.Size(240, 25)
        Me.lblProjectNo.TabIndex = 0
        Me.lblProjectNo.Text = "ProjectNo"
        Me.lblProjectNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblProjectDesc
        '
        Me.lblProjectDesc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblProjectDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProjectDesc.Location = New System.Drawing.Point(258, 9)
        Me.lblProjectDesc.Name = "lblProjectDesc"
        Me.lblProjectDesc.Size = New System.Drawing.Size(399, 25)
        Me.lblProjectDesc.TabIndex = 1
        Me.lblProjectDesc.Text = "ProjectDesc"
        Me.lblProjectDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOrderedQty
        '
        Me.lblOrderedQty.AutoSize = True
        Me.lblOrderedQty.Location = New System.Drawing.Point(12, 44)
        Me.lblOrderedQty.Name = "lblOrderedQty"
        Me.lblOrderedQty.Size = New System.Drawing.Size(45, 13)
        Me.lblOrderedQty.TabIndex = 2
        Me.lblOrderedQty.Text = "Ordered"
        Me.lblOrderedQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblQtyLoaded
        '
        Me.lblQtyLoaded.AutoSize = True
        Me.lblQtyLoaded.Location = New System.Drawing.Point(64, 44)
        Me.lblQtyLoaded.Name = "lblQtyLoaded"
        Me.lblQtyLoaded.Size = New System.Drawing.Size(43, 13)
        Me.lblQtyLoaded.TabIndex = 3
        Me.lblQtyLoaded.Text = "Loaded"
        '
        'lblPartNo
        '
        Me.lblPartNo.AutoSize = True
        Me.lblPartNo.Location = New System.Drawing.Point(113, 44)
        Me.lblPartNo.Name = "lblPartNo"
        Me.lblPartNo.Size = New System.Drawing.Size(43, 13)
        Me.lblPartNo.TabIndex = 4
        Me.lblPartNo.Text = "Part No"
        '
        'txtOrderedQty
        '
        Me.txtOrderedQty.Location = New System.Drawing.Point(12, 60)
        Me.txtOrderedQty.Name = "txtOrderedQty"
        Me.txtOrderedQty.Size = New System.Drawing.Size(46, 20)
        Me.txtOrderedQty.TabIndex = 5
        Me.txtOrderedQty.Text = "0"
        Me.txtOrderedQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtLoadedQty
        '
        Me.txtLoadedQty.Location = New System.Drawing.Point(64, 60)
        Me.txtLoadedQty.Name = "txtLoadedQty"
        Me.txtLoadedQty.ReadOnly = True
        Me.txtLoadedQty.Size = New System.Drawing.Size(43, 20)
        Me.txtLoadedQty.TabIndex = 6
        Me.txtLoadedQty.Text = "0"
        Me.txtLoadedQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboMultiPartList
        '
        Me.cboMultiPartList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMultiPartList.FormattingEnabled = True
        Me.cboMultiPartList.Location = New System.Drawing.Point(258, 60)
        Me.cboMultiPartList.Name = "cboMultiPartList"
        Me.cboMultiPartList.Size = New System.Drawing.Size(399, 21)
        Me.cboMultiPartList.TabIndex = 7
        '
        'pnlSplit
        '
        Me.pnlSplit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSplit.Controls.Add(Me.btnSplitPartSearch)
        Me.pnlSplit.Controls.Add(Me.txtSplitPartNo)
        Me.pnlSplit.Controls.Add(Me.cboSplitMultipartList)
        Me.pnlSplit.Controls.Add(Me.txtSplitOrderedQty)
        Me.pnlSplit.Controls.Add(Me.txtSplitLoadedQty)
        Me.pnlSplit.Location = New System.Drawing.Point(0, 106)
        Me.pnlSplit.Name = "pnlSplit"
        Me.pnlSplit.Size = New System.Drawing.Size(669, 28)
        Me.pnlSplit.TabIndex = 13
        Me.pnlSplit.Visible = False
        '
        'btnSplitPartSearch
        '
        Me.btnSplitPartSearch.AutoSize = True
        Me.btnSplitPartSearch.Location = New System.Drawing.Point(226, 1)
        Me.btnSplitPartSearch.Name = "btnSplitPartSearch"
        Me.btnSplitPartSearch.Size = New System.Drawing.Size(26, 23)
        Me.btnSplitPartSearch.TabIndex = 18
        Me.btnSplitPartSearch.Text = "..."
        Me.btnSplitPartSearch.UseVisualStyleBackColor = True
        '
        'txtSplitPartNo
        '
        Me.txtSplitPartNo.Location = New System.Drawing.Point(113, 4)
        Me.txtSplitPartNo.Name = "txtSplitPartNo"
        Me.txtSplitPartNo.ReadOnly = True
        Me.txtSplitPartNo.Size = New System.Drawing.Size(107, 20)
        Me.txtSplitPartNo.TabIndex = 17
        Me.txtSplitPartNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cboSplitMultipartList
        '
        Me.cboSplitMultipartList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSplitMultipartList.FormattingEnabled = True
        Me.cboSplitMultipartList.Location = New System.Drawing.Point(258, 2)
        Me.cboSplitMultipartList.Name = "cboSplitMultipartList"
        Me.cboSplitMultipartList.Size = New System.Drawing.Size(398, 21)
        Me.cboSplitMultipartList.TabIndex = 16
        '
        'txtSplitOrderedQty
        '
        Me.txtSplitOrderedQty.Location = New System.Drawing.Point(12, 4)
        Me.txtSplitOrderedQty.Name = "txtSplitOrderedQty"
        Me.txtSplitOrderedQty.Size = New System.Drawing.Size(46, 20)
        Me.txtSplitOrderedQty.TabIndex = 14
        Me.txtSplitOrderedQty.Text = "0"
        Me.txtSplitOrderedQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSplitLoadedQty
        '
        Me.txtSplitLoadedQty.Location = New System.Drawing.Point(64, 4)
        Me.txtSplitLoadedQty.Name = "txtSplitLoadedQty"
        Me.txtSplitLoadedQty.ReadOnly = True
        Me.txtSplitLoadedQty.Size = New System.Drawing.Size(43, 20)
        Me.txtSplitLoadedQty.TabIndex = 15
        Me.txtSplitLoadedQty.Text = "0"
        Me.txtSplitLoadedQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPartNo
        '
        Me.txtPartNo.Location = New System.Drawing.Point(113, 61)
        Me.txtPartNo.Name = "txtPartNo"
        Me.txtPartNo.ReadOnly = True
        Me.txtPartNo.Size = New System.Drawing.Size(107, 20)
        Me.txtPartNo.TabIndex = 16
        Me.txtPartNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnPartSearch
        '
        Me.btnPartSearch.AutoSize = True
        Me.btnPartSearch.Location = New System.Drawing.Point(226, 59)
        Me.btnPartSearch.Name = "btnPartSearch"
        Me.btnPartSearch.Size = New System.Drawing.Size(26, 23)
        Me.btnPartSearch.TabIndex = 17
        Me.btnPartSearch.Text = "..."
        Me.btnPartSearch.UseVisualStyleBackColor = True
        '
        'lblPartDesc
        '
        Me.lblPartDesc.AutoSize = True
        Me.lblPartDesc.Location = New System.Drawing.Point(255, 44)
        Me.lblPartDesc.Name = "lblPartDesc"
        Me.lblPartDesc.Size = New System.Drawing.Size(82, 13)
        Me.lblPartDesc.TabIndex = 18
        Me.lblPartDesc.Text = "Part Description"
        '
        'chkSplit
        '
        Me.chkSplit.AutoSize = True
        Me.chkSplit.Location = New System.Drawing.Point(12, 86)
        Me.chkSplit.Name = "chkSplit"
        Me.chkSplit.Size = New System.Drawing.Size(46, 17)
        Me.chkSplit.TabIndex = 19
        Me.chkSplit.Text = "Split"
        Me.chkSplit.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(12, 162)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnDelete.TabIndex = 20
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnCommit
        '
        Me.btnCommit.Location = New System.Drawing.Point(500, 162)
        Me.btnCommit.Name = "btnCommit"
        Me.btnCommit.Size = New System.Drawing.Size(75, 23)
        Me.btnCommit.TabIndex = 21
        Me.btnCommit.Text = "Commit"
        Me.btnCommit.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.CausesValidation = False
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(581, 162)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 22
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmScheduleSubstitution
        '
        Me.AcceptButton = Me.btnCommit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(669, 195)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnCommit)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.chkSplit)
        Me.Controls.Add(Me.lblPartDesc)
        Me.Controls.Add(Me.btnPartSearch)
        Me.Controls.Add(Me.txtPartNo)
        Me.Controls.Add(Me.pnlSplit)
        Me.Controls.Add(Me.cboMultiPartList)
        Me.Controls.Add(Me.txtLoadedQty)
        Me.Controls.Add(Me.txtOrderedQty)
        Me.Controls.Add(Me.lblPartNo)
        Me.Controls.Add(Me.lblQtyLoaded)
        Me.Controls.Add(Me.lblOrderedQty)
        Me.Controls.Add(Me.lblProjectDesc)
        Me.Controls.Add(Me.lblProjectNo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(685, 233)
        Me.Name = "frmScheduleSubstitution"
        Me.Text = "Schedule Substitution"
        Me.pnlSplit.ResumeLayout(False)
        Me.pnlSplit.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblProjectNo As System.Windows.Forms.Label
    Friend WithEvents lblProjectDesc As System.Windows.Forms.Label
    Friend WithEvents lblOrderedQty As System.Windows.Forms.Label
    Friend WithEvents lblQtyLoaded As System.Windows.Forms.Label
    Friend WithEvents lblPartNo As System.Windows.Forms.Label
    Friend WithEvents txtOrderedQty As System.Windows.Forms.TextBox
    Friend WithEvents txtLoadedQty As System.Windows.Forms.TextBox
    Friend WithEvents cboMultiPartList As System.Windows.Forms.ComboBox
    Friend WithEvents pnlSplit As System.Windows.Forms.Panel
    Friend WithEvents cboSplitMultipartList As System.Windows.Forms.ComboBox
    Friend WithEvents txtSplitOrderedQty As System.Windows.Forms.TextBox
    Friend WithEvents txtSplitLoadedQty As System.Windows.Forms.TextBox
    Friend WithEvents txtSplitPartNo As System.Windows.Forms.TextBox
    Friend WithEvents txtPartNo As System.Windows.Forms.TextBox
    Friend WithEvents btnSplitPartSearch As System.Windows.Forms.Button
    Friend WithEvents btnPartSearch As System.Windows.Forms.Button
    Friend WithEvents lblPartDesc As System.Windows.Forms.Label
    Friend WithEvents chkSplit As System.Windows.Forms.CheckBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnCommit As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
