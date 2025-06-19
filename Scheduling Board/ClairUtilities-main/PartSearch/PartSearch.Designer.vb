<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPartPicker
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
        Me.lvParts = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.lblPartNumber = New System.Windows.Forms.Label()
        Me.lblEnterSearch = New System.Windows.Forms.Label()
        Me.lblNoData = New System.Windows.Forms.Label()
        Me.tmrDelayedSearch = New System.Windows.Forms.Timer(Me.components)
        Me.chkHideUnusedParts = New System.Windows.Forms.CheckBox()
        Me.chkMyPartsOnly = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.cmbSubCategory = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkOnlyMyWarehouses = New System.Windows.Forms.CheckBox()
        Me.txtSearch = New HighlightTextBox()
        Me.SuspendLayout()
        '
        'lvParts
        '
        Me.lvParts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvParts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lvParts.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvParts.FullRowSelect = True
        Me.lvParts.HideSelection = False
        Me.lvParts.Location = New System.Drawing.Point(0, 90)
        Me.lvParts.Name = "lvParts"
        Me.lvParts.Size = New System.Drawing.Size(717, 364)
        Me.lvParts.TabIndex = 1
        Me.lvParts.UseCompatibleStateImageBehavior = False
        Me.lvParts.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Part Number"
        Me.ColumnHeader1.Width = 130
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Part Description"
        Me.ColumnHeader2.Width = 530
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "BtInk"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Inv"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(390, 460)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOk.Location = New System.Drawing.Point(309, 460)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 7
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'lblPartNumber
        '
        Me.lblPartNumber.AutoSize = True
        Me.lblPartNumber.Location = New System.Drawing.Point(6, 6)
        Me.lblPartNumber.Name = "lblPartNumber"
        Me.lblPartNumber.Size = New System.Drawing.Size(267, 13)
        Me.lblPartNumber.TabIndex = 9
        Me.lblPartNumber.Text = "&Search for part by barcode, number, tag, or description:"
        '
        'lblEnterSearch
        '
        Me.lblEnterSearch.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblEnterSearch.AutoSize = True
        Me.lblEnterSearch.Location = New System.Drawing.Point(251, 246)
        Me.lblEnterSearch.Name = "lblEnterSearch"
        Me.lblEnterSearch.Size = New System.Drawing.Size(187, 13)
        Me.lblEnterSearch.TabIndex = 12
        Me.lblEnterSearch.Text = "Enter a part description or part number"
        '
        'lblNoData
        '
        Me.lblNoData.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblNoData.AutoSize = True
        Me.lblNoData.Location = New System.Drawing.Point(290, 233)
        Me.lblNoData.Name = "lblNoData"
        Me.lblNoData.Size = New System.Drawing.Size(123, 13)
        Me.lblNoData.TabIndex = 11
        Me.lblNoData.Text = "No matching parts found"
        Me.lblNoData.Visible = False
        '
        'tmrDelayedSearch
        '
        Me.tmrDelayedSearch.Enabled = True
        Me.tmrDelayedSearch.Interval = 200
        '
        'chkHideUnusedParts
        '
        Me.chkHideUnusedParts.AutoSize = True
        Me.chkHideUnusedParts.Checked = True
        Me.chkHideUnusedParts.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHideUnusedParts.Location = New System.Drawing.Point(594, 64)
        Me.chkHideUnusedParts.Name = "chkHideUnusedParts"
        Me.chkHideUnusedParts.Size = New System.Drawing.Size(115, 17)
        Me.chkHideUnusedParts.TabIndex = 6
        Me.chkHideUnusedParts.Text = "Hide Unused Parts"
        Me.chkHideUnusedParts.UseVisualStyleBackColor = True
        '
        'chkMyPartsOnly
        '
        Me.chkMyPartsOnly.AutoSize = True
        Me.chkMyPartsOnly.Location = New System.Drawing.Point(486, 64)
        Me.chkMyPartsOnly.Name = "chkMyPartsOnly"
        Me.chkMyPartsOnly.Size = New System.Drawing.Size(91, 17)
        Me.chkMyPartsOnly.TabIndex = 5
        Me.chkMyPartsOnly.Text = "My Parts Only"
        Me.chkMyPartsOnly.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(317, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Category:"
        '
        'cmbCategory
        '
        Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(320, 28)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(184, 21)
        Me.cmbCategory.TabIndex = 2
        '
        'cmbSubCategory
        '
        Me.cmbSubCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubCategory.FormattingEnabled = True
        Me.cmbSubCategory.Location = New System.Drawing.Point(525, 28)
        Me.cmbSubCategory.Name = "cmbSubCategory"
        Me.cmbSubCategory.Size = New System.Drawing.Size(184, 21)
        Me.cmbSubCategory.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(311, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(522, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Sub-Category:"
        '
        'chkOnlyMyWarehouses
        '
        Me.chkOnlyMyWarehouses.AutoSize = True
        Me.chkOnlyMyWarehouses.Location = New System.Drawing.Point(274, 64)
        Me.chkOnlyMyWarehouses.Name = "chkOnlyMyWarehouses"
        Me.chkOnlyMyWarehouses.Size = New System.Drawing.Size(191, 17)
        Me.chkOnlyMyWarehouses.TabIndex = 4
        Me.chkOnlyMyWarehouses.Text = "Only Parts in My Home Warehouse"
        Me.chkOnlyMyWarehouses.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSearch.Location = New System.Drawing.Point(10, 28)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(286, 20)
        Me.txtSearch.TabIndex = 0
        Me.txtSearch.TextToClear = Nothing
        '
        'frmPartPicker
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(716, 492)
        Me.Controls.Add(Me.chkOnlyMyWarehouses)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbSubCategory)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbCategory)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkMyPartsOnly)
        Me.Controls.Add(Me.chkHideUnusedParts)
        Me.Controls.Add(Me.lblEnterSearch)
        Me.Controls.Add(Me.lblNoData)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblPartNumber)
        Me.Controls.Add(Me.lvParts)
        Me.Name = "frmPartPicker"
        Me.Text = "Part Search"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lvParts As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtSearch As HighlightTextBox
    Friend WithEvents lblPartNumber As System.Windows.Forms.Label
    Friend WithEvents lblEnterSearch As System.Windows.Forms.Label
    Friend WithEvents lblNoData As System.Windows.Forms.Label
    Friend WithEvents tmrDelayedSearch As System.Windows.Forms.Timer
    Friend WithEvents chkHideUnusedParts As CheckBox
    Friend WithEvents chkMyPartsOnly As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbCategory As ComboBox
    Friend WithEvents cmbSubCategory As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents chkOnlyMyWarehouses As CheckBox
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
End Class
