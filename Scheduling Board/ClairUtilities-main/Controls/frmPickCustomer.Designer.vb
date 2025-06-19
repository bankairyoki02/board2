<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmPickCustomer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmPickCustomer))
        Me.lblSubAddress = New System.Windows.Forms.Label()
        Me.cboSubAddress = New System.Windows.Forms.ComboBox()
        Me.lblCustomer = New System.Windows.Forms.Label()
        Me.btnContactMaintenance = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.dgvKnownCustomers = New System.Windows.Forms.DataGridView()
        Me.gbKnownCustomers = New System.Windows.Forms.GroupBox()
        Me.gbCustomerSearch = New System.Windows.Forms.GroupBox()
        Me.chkHideInactive = New System.Windows.Forms.CheckBox()
        Me.pnlAcceptButtons = New System.Windows.Forms.Panel()
        Me.txtCustomer = New SearchTextBox()
        CType(Me.dgvKnownCustomers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbKnownCustomers.SuspendLayout()
        Me.gbCustomerSearch.SuspendLayout()
        Me.pnlAcceptButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSubAddress
        '
        Me.lblSubAddress.AutoSize = True
        Me.lblSubAddress.Location = New System.Drawing.Point(10, 79)
        Me.lblSubAddress.Name = "lblSubAddress"
        Me.lblSubAddress.Size = New System.Drawing.Size(48, 13)
        Me.lblSubAddress.TabIndex = 49
        Me.lblSubAddress.Text = "Address:"
        Me.lblSubAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboSubAddress
        '
        Me.cboSubAddress.DisplayMember = "bill_to_name"
        Me.cboSubAddress.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cboSubAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubAddress.FormattingEnabled = True
        Me.cboSubAddress.ItemHeight = 90
        Me.cboSubAddress.Location = New System.Drawing.Point(15, 98)
        Me.cboSubAddress.Name = "cboSubAddress"
        Me.cboSubAddress.Size = New System.Drawing.Size(321, 96)
        Me.cboSubAddress.TabIndex = 50
        Me.cboSubAddress.ValueMember = "subno"
        '
        'lblCustomer
        '
        Me.lblCustomer.AutoSize = True
        Me.lblCustomer.Location = New System.Drawing.Point(10, 49)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.Size = New System.Drawing.Size(54, 13)
        Me.lblCustomer.TabIndex = 46
        Me.lblCustomer.Text = "Customer:"
        Me.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        'dgvKnownCustomers
        '
        Me.dgvKnownCustomers.AllowUserToAddRows = False
        Me.dgvKnownCustomers.AllowUserToDeleteRows = False
        Me.dgvKnownCustomers.AllowUserToResizeColumns = False
        Me.dgvKnownCustomers.AllowUserToResizeRows = False
        Me.dgvKnownCustomers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvKnownCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvKnownCustomers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvKnownCustomers.Location = New System.Drawing.Point(3, 16)
        Me.dgvKnownCustomers.MultiSelect = False
        Me.dgvKnownCustomers.Name = "dgvKnownCustomers"
        Me.dgvKnownCustomers.ReadOnly = True
        Me.dgvKnownCustomers.RowHeadersVisible = False
        Me.dgvKnownCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvKnownCustomers.Size = New System.Drawing.Size(799, 365)
        Me.dgvKnownCustomers.TabIndex = 53
        '
        'gbKnownCustomers
        '
        Me.gbKnownCustomers.Controls.Add(Me.dgvKnownCustomers)
        Me.gbKnownCustomers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbKnownCustomers.Location = New System.Drawing.Point(0, 0)
        Me.gbKnownCustomers.Name = "gbKnownCustomers"
        Me.gbKnownCustomers.Size = New System.Drawing.Size(805, 384)
        Me.gbKnownCustomers.TabIndex = 54
        Me.gbKnownCustomers.TabStop = False
        Me.gbKnownCustomers.Text = "Known Customers"
        '
        'gbCustomerSearch
        '
        Me.gbCustomerSearch.Controls.Add(Me.chkHideInactive)
        Me.gbCustomerSearch.Controls.Add(Me.lblSubAddress)
        Me.gbCustomerSearch.Controls.Add(Me.cboSubAddress)
        Me.gbCustomerSearch.Controls.Add(Me.lblCustomer)
        Me.gbCustomerSearch.Controls.Add(Me.txtCustomer)
        Me.gbCustomerSearch.Controls.Add(Me.btnContactMaintenance)
        Me.gbCustomerSearch.Dock = System.Windows.Forms.DockStyle.Right
        Me.gbCustomerSearch.Location = New System.Drawing.Point(805, 0)
        Me.gbCustomerSearch.Name = "gbCustomerSearch"
        Me.gbCustomerSearch.Size = New System.Drawing.Size(345, 384)
        Me.gbCustomerSearch.TabIndex = 55
        Me.gbCustomerSearch.TabStop = False
        Me.gbCustomerSearch.Text = "Customer Search"
        '
        'chkHideInactive
        '
        Me.chkHideInactive.AutoSize = True
        Me.chkHideInactive.Checked = True
        Me.chkHideInactive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHideInactive.Location = New System.Drawing.Point(15, 19)
        Me.chkHideInactive.Name = "chkHideInactive"
        Me.chkHideInactive.Size = New System.Drawing.Size(141, 17)
        Me.chkHideInactive.TabIndex = 51
        Me.chkHideInactive.Text = "Hide Inactive Customers"
        Me.chkHideInactive.UseVisualStyleBackColor = True
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
        'txtCustomer
        '
        Me.txtCustomer.Connection = Nothing
        Me.txtCustomer.DisplayMember = "cust_name"
        Me.txtCustomer.FilterClause = Nothing
        Me.txtCustomer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCustomer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCustomer.Location = New System.Drawing.Point(70, 47)
        Me.txtCustomer.Name = "txtCustomer"
        Me.txtCustomer.OrderBy = Nothing
        Me.txtCustomer.ResultsRowLimit = 0
        Me.txtCustomer.SelectedValue = Nothing
        Me.txtCustomer.Size = New System.Drawing.Size(228, 20)
        Me.txtCustomer.TabIndex = 47
        Me.txtCustomer.TableName = "dbo.oecustomer"
        Me.txtCustomer.TextToClear = Nothing
        Me.txtCustomer.ValueMember = "custno"
        '
        'FrmPickCustomer
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(1150, 423)
        Me.Controls.Add(Me.gbKnownCustomers)
        Me.Controls.Add(Me.gbCustomerSearch)
        Me.Controls.Add(Me.pnlAcceptButtons)
        Me.Name = "FrmPickCustomer"
        Me.Text = "Choose Customer"
        CType(Me.dgvKnownCustomers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbKnownCustomers.ResumeLayout(False)
        Me.gbCustomerSearch.ResumeLayout(False)
        Me.gbCustomerSearch.PerformLayout()
        Me.pnlAcceptButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblSubAddress As System.Windows.Forms.Label
    Friend WithEvents cboSubAddress As System.Windows.Forms.ComboBox
    Friend WithEvents lblCustomer As System.Windows.Forms.Label
    Friend WithEvents txtCustomer As SearchTextBox
    Friend WithEvents btnContactMaintenance As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents dgvKnownCustomers As DataGridView
    Friend WithEvents gbKnownCustomers As GroupBox
    Friend WithEvents gbCustomerSearch As GroupBox
    Friend WithEvents pnlAcceptButtons As Panel
    Friend WithEvents chkHideInactive As CheckBox
End Class
