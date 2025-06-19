<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPickVendor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPickVendor))
        Me.lblSiteAddress = New System.Windows.Forms.Label()
        Me.cboSiteAddress = New System.Windows.Forms.ComboBox()
        Me.lblVendor = New System.Windows.Forms.Label()
        Me.btnContactMaintenance = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtVendor = New SearchTextBox()
        Me.dgvKnownVendors = New System.Windows.Forms.DataGridView()
        Me.gbKnownVendors = New System.Windows.Forms.GroupBox()
        Me.gbVendorSearch = New System.Windows.Forms.GroupBox()
        Me.pnlAcceptButtons = New System.Windows.Forms.Panel()
        Me.chkHideInactive = New System.Windows.Forms.CheckBox()
        CType(Me.dgvKnownVendors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbKnownVendors.SuspendLayout()
        Me.gbVendorSearch.SuspendLayout()
        Me.pnlAcceptButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSiteAddress
        '
        Me.lblSiteAddress.AutoSize = True
        Me.lblSiteAddress.Location = New System.Drawing.Point(10, 79)
        Me.lblSiteAddress.Name = "lblSiteAddress"
        Me.lblSiteAddress.Size = New System.Drawing.Size(48, 13)
        Me.lblSiteAddress.TabIndex = 49
        Me.lblSiteAddress.Text = "Address:"
        Me.lblSiteAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboSiteAddress
        '
        Me.cboSiteAddress.DisplayMember = "bill_to_name"
        Me.cboSiteAddress.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cboSiteAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSiteAddress.FormattingEnabled = True
        Me.cboSiteAddress.ItemHeight = 90
        Me.cboSiteAddress.Location = New System.Drawing.Point(15, 98)
        Me.cboSiteAddress.Name = "cboSiteAddress"
        Me.cboSiteAddress.Size = New System.Drawing.Size(321, 96)
        Me.cboSiteAddress.TabIndex = 50
        Me.cboSiteAddress.ValueMember = "subno"
        '
        'lblVendor
        '
        Me.lblVendor.AutoSize = True
        Me.lblVendor.Location = New System.Drawing.Point(10, 49)
        Me.lblVendor.Name = "lblVendor"
        Me.lblVendor.Size = New System.Drawing.Size(44, 13)
        Me.lblVendor.TabIndex = 46
        Me.lblVendor.Text = "Vendor:"
        Me.lblVendor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnContactMaintenance
        '
        Me.btnContactMaintenance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnContactMaintenance.Image = CType(resources.GetObject("btnContactMaintenance.Image"), System.Drawing.Image)
        Me.btnContactMaintenance.Location = New System.Drawing.Point(304, 45)
        Me.btnContactMaintenance.Name = "btnContactMaintenance"
        Me.btnContactMaintenance.Size = New System.Drawing.Size(32, 32)
        Me.btnContactMaintenance.TabIndex = 48
        Me.btnContactMaintenance.Tag = "Contact Maintenance"
        Me.btnContactMaintenance.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Enabled = False
        Me.cmdOK.Location = New System.Drawing.Point(906, 6)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 51
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(987, 6)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 52
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'txtVendor
        '
        Me.txtVendor.Connection = Nothing
        Me.txtVendor.DisplayMember = "vendor_name"
        Me.txtVendor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendor.Location = New System.Drawing.Point(70, 47)
        Me.txtVendor.Name = "txtVendor"
        Me.txtVendor.OrderBy = Nothing
        Me.txtVendor.ResultsRowLimit = 0
        Me.txtVendor.SelectedValue = Nothing
        Me.txtVendor.Size = New System.Drawing.Size(228, 20)
        Me.txtVendor.TabIndex = 47
        Me.txtVendor.TableName = "dbo.povendor"
        Me.txtVendor.TextToClear = Nothing
        Me.txtVendor.ValueMember = "vendno"
        '
        'dgvKnownVendors
        '
        Me.dgvKnownVendors.AllowUserToAddRows = False
        Me.dgvKnownVendors.AllowUserToDeleteRows = False
        Me.dgvKnownVendors.AllowUserToResizeColumns = False
        Me.dgvKnownVendors.AllowUserToResizeRows = False
        Me.dgvKnownVendors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvKnownVendors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvKnownVendors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvKnownVendors.Location = New System.Drawing.Point(3, 16)
        Me.dgvKnownVendors.MultiSelect = False
        Me.dgvKnownVendors.Name = "dgvKnownVendors"
        Me.dgvKnownVendors.ReadOnly = True
        Me.dgvKnownVendors.RowHeadersVisible = False
        Me.dgvKnownVendors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvKnownVendors.Size = New System.Drawing.Size(799, 365)
        Me.dgvKnownVendors.TabIndex = 53
        '
        'gbKnownVendors
        '
        Me.gbKnownVendors.Controls.Add(Me.dgvKnownVendors)
        Me.gbKnownVendors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbKnownVendors.Location = New System.Drawing.Point(0, 0)
        Me.gbKnownVendors.Name = "gbKnownVendors"
        Me.gbKnownVendors.Size = New System.Drawing.Size(805, 384)
        Me.gbKnownVendors.TabIndex = 54
        Me.gbKnownVendors.TabStop = False
        Me.gbKnownVendors.Text = "Known Vendors"
        '
        'gbVendorSearch
        '
        Me.gbVendorSearch.Controls.Add(Me.chkHideInactive)
        Me.gbVendorSearch.Controls.Add(Me.lblSiteAddress)
        Me.gbVendorSearch.Controls.Add(Me.cboSiteAddress)
        Me.gbVendorSearch.Controls.Add(Me.lblVendor)
        Me.gbVendorSearch.Controls.Add(Me.txtVendor)
        Me.gbVendorSearch.Controls.Add(Me.btnContactMaintenance)
        Me.gbVendorSearch.Dock = System.Windows.Forms.DockStyle.Right
        Me.gbVendorSearch.Location = New System.Drawing.Point(805, 0)
        Me.gbVendorSearch.Name = "gbVendorSearch"
        Me.gbVendorSearch.Size = New System.Drawing.Size(345, 384)
        Me.gbVendorSearch.TabIndex = 55
        Me.gbVendorSearch.TabStop = False
        Me.gbVendorSearch.Text = "Vendor Search"
        '
        'pnlAcceptButtons
        '
        Me.pnlAcceptButtons.Controls.Add(Me.cmdCancel)
        Me.pnlAcceptButtons.Controls.Add(Me.cmdOK)
        Me.pnlAcceptButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlAcceptButtons.Location = New System.Drawing.Point(0, 384)
        Me.pnlAcceptButtons.Name = "pnlAcceptButtons"
        Me.pnlAcceptButtons.Size = New System.Drawing.Size(1150, 39)
        Me.pnlAcceptButtons.TabIndex = 56
        '
        'chkHideInactive
        '
        Me.chkHideInactive.AutoSize = True
        Me.chkHideInactive.Checked = True
        Me.chkHideInactive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHideInactive.Location = New System.Drawing.Point(15, 19)
        Me.chkHideInactive.Name = "chkHideInactive"
        Me.chkHideInactive.Size = New System.Drawing.Size(131, 17)
        Me.chkHideInactive.TabIndex = 51
        Me.chkHideInactive.Text = "Hide Inactive Vendors"
        Me.chkHideInactive.UseVisualStyleBackColor = True
        '
        'frmPickVendor
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(1150, 423)
        Me.Controls.Add(Me.gbKnownVendors)
        Me.Controls.Add(Me.gbVendorSearch)
        Me.Controls.Add(Me.pnlAcceptButtons)
        Me.Name = "frmPickVendor"
        Me.Text = "Choose Vendor"
        CType(Me.dgvKnownVendors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbKnownVendors.ResumeLayout(False)
        Me.gbVendorSearch.ResumeLayout(False)
        Me.gbVendorSearch.PerformLayout()
        Me.pnlAcceptButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblSiteAddress As System.Windows.Forms.Label
    Friend WithEvents cboSiteAddress As System.Windows.Forms.ComboBox
    Friend WithEvents lblVendor As System.Windows.Forms.Label
    Friend WithEvents txtVendor As SearchTextBox
    Friend WithEvents btnContactMaintenance As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents dgvKnownVendors As DataGridView
    Friend WithEvents gbKnownVendors As GroupBox
    Friend WithEvents gbVendorSearch As GroupBox
    Friend WithEvents pnlAcceptButtons As Panel
    Friend WithEvents chkHideInactive As CheckBox
End Class
