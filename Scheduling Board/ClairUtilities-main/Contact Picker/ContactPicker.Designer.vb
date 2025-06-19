<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ContactPicker
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
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.pnlAcceptButtons = New System.Windows.Forms.Panel()
        Me.flpAddNewDetails = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlName = New System.Windows.Forms.Panel()
        Me.txtName = New HighlightTextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.pnlPhone = New System.Windows.Forms.Panel()
        Me.txtPhone = New HighlightTextBox()
        Me.lblPhone = New System.Windows.Forms.Label()
        Me.pnlEmail = New System.Windows.Forms.Panel()
        Me.txtEmail = New HighlightTextBox()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.pnlAddr1 = New System.Windows.Forms.Panel()
        Me.txtAddr1 = New HighlightTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlAddr2 = New System.Windows.Forms.Panel()
        Me.txtAddr2 = New HighlightTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pnlAddr3 = New System.Windows.Forms.Panel()
        Me.txtAddr3 = New HighlightTextBox()
        Me.lblAddr3 = New System.Windows.Forms.Label()
        Me.pnlCity = New System.Windows.Forms.Panel()
        Me.txtCity = New HighlightTextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnlState = New System.Windows.Forms.Panel()
        Me.txtState = New HighlightTextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.pnlProvince = New System.Windows.Forms.Panel()
        Me.txtProvince = New HighlightTextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.pnlZip = New System.Windows.Forms.Panel()
        Me.txtZip = New HighlightTextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pnlCountry = New System.Windows.Forms.Panel()
        Me.txtCountry = New HighlightTextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnlAddressSearch = New System.Windows.Forms.Panel()
        Me.cmdAddNew = New System.Windows.Forms.Button()
        Me.txtSearch = New SearchTextBox()
        Me.btnContactMaintenance = New System.Windows.Forms.Button()
        Me.pnlAcceptButtons.SuspendLayout()
        Me.flpAddNewDetails.SuspendLayout()
        Me.pnlName.SuspendLayout()
        Me.pnlPhone.SuspendLayout()
        Me.pnlEmail.SuspendLayout()
        Me.pnlAddr1.SuspendLayout()
        Me.pnlAddr2.SuspendLayout()
        Me.pnlAddr3.SuspendLayout()
        Me.pnlCity.SuspendLayout()
        Me.pnlState.SuspendLayout()
        Me.pnlProvince.SuspendLayout()
        Me.pnlZip.SuspendLayout()
        Me.pnlCountry.SuspendLayout()
        Me.pnlAddressSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSearch
        '
        Me.lblSearch.Location = New System.Drawing.Point(13, 10)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(115, 13)
        Me.lblSearch.TabIndex = 49
        Me.lblSearch.Text = "_AddressType Search:"
        Me.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(270, 8)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 54
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Enabled = False
        Me.cmdOK.Location = New System.Drawing.Point(189, 8)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'pnlAcceptButtons
        '
        Me.pnlAcceptButtons.Controls.Add(Me.cmdOK)
        Me.pnlAcceptButtons.Controls.Add(Me.cmdCancel)
        Me.pnlAcceptButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlAcceptButtons.Location = New System.Drawing.Point(0, 400)
        Me.pnlAcceptButtons.Name = "pnlAcceptButtons"
        Me.pnlAcceptButtons.Size = New System.Drawing.Size(348, 43)
        Me.pnlAcceptButtons.TabIndex = 56
        '
        'flpAddNewDetails
        '
        Me.flpAddNewDetails.Controls.Add(Me.pnlName)
        Me.flpAddNewDetails.Controls.Add(Me.pnlPhone)
        Me.flpAddNewDetails.Controls.Add(Me.pnlEmail)
        Me.flpAddNewDetails.Controls.Add(Me.pnlAddr1)
        Me.flpAddNewDetails.Controls.Add(Me.pnlAddr2)
        Me.flpAddNewDetails.Controls.Add(Me.pnlAddr3)
        Me.flpAddNewDetails.Controls.Add(Me.pnlCity)
        Me.flpAddNewDetails.Controls.Add(Me.pnlState)
        Me.flpAddNewDetails.Controls.Add(Me.pnlProvince)
        Me.flpAddNewDetails.Controls.Add(Me.pnlZip)
        Me.flpAddNewDetails.Controls.Add(Me.pnlCountry)
        Me.flpAddNewDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAddNewDetails.Location = New System.Drawing.Point(0, 82)
        Me.flpAddNewDetails.Name = "flpAddNewDetails"
        Me.flpAddNewDetails.Size = New System.Drawing.Size(348, 318)
        Me.flpAddNewDetails.TabIndex = 57
        '
        'pnlName
        '
        Me.pnlName.Controls.Add(Me.txtName)
        Me.pnlName.Controls.Add(Me.lblName)
        Me.pnlName.Location = New System.Drawing.Point(3, 3)
        Me.pnlName.Name = "pnlName"
        Me.pnlName.Size = New System.Drawing.Size(339, 22)
        Me.pnlName.TabIndex = 0
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(128, 1)
        Me.txtName.MaxLength = 100
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(208, 20)
        Me.txtName.TabIndex = 0
        Me.txtName.TextToClear = Nothing
        '
        'lblName
        '
        Me.lblName.Location = New System.Drawing.Point(12, 2)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(115, 13)
        Me.lblName.TabIndex = 50
        Me.lblName.Text = "_AddressType Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlPhone
        '
        Me.pnlPhone.Controls.Add(Me.txtPhone)
        Me.pnlPhone.Controls.Add(Me.lblPhone)
        Me.pnlPhone.Location = New System.Drawing.Point(3, 31)
        Me.pnlPhone.Name = "pnlPhone"
        Me.pnlPhone.Size = New System.Drawing.Size(339, 22)
        Me.pnlPhone.TabIndex = 1
        '
        'txtPhone
        '
        Me.txtPhone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhone.Location = New System.Drawing.Point(128, 1)
        Me.txtPhone.MaxLength = 50
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(132, 20)
        Me.txtPhone.TabIndex = 0
        Me.txtPhone.TextToClear = Nothing
        '
        'lblPhone
        '
        Me.lblPhone.Location = New System.Drawing.Point(12, 2)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.Size = New System.Drawing.Size(115, 13)
        Me.lblPhone.TabIndex = 50
        Me.lblPhone.Text = "Cell Phone:"
        Me.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlEmail
        '
        Me.pnlEmail.Controls.Add(Me.txtEmail)
        Me.pnlEmail.Controls.Add(Me.lblEmail)
        Me.pnlEmail.Location = New System.Drawing.Point(3, 59)
        Me.pnlEmail.Name = "pnlEmail"
        Me.pnlEmail.Size = New System.Drawing.Size(339, 22)
        Me.pnlEmail.TabIndex = 2
        '
        'txtEmail
        '
        Me.txtEmail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEmail.Location = New System.Drawing.Point(128, 1)
        Me.txtEmail.MaxLength = 50
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(197, 20)
        Me.txtEmail.TabIndex = 0
        Me.txtEmail.TextToClear = Nothing
        '
        'lblEmail
        '
        Me.lblEmail.Location = New System.Drawing.Point(12, 2)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(115, 13)
        Me.lblEmail.TabIndex = 50
        Me.lblEmail.Text = "Email:"
        Me.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlAddr1
        '
        Me.pnlAddr1.Controls.Add(Me.txtAddr1)
        Me.pnlAddr1.Controls.Add(Me.Label1)
        Me.pnlAddr1.Location = New System.Drawing.Point(3, 87)
        Me.pnlAddr1.Name = "pnlAddr1"
        Me.pnlAddr1.Size = New System.Drawing.Size(339, 22)
        Me.pnlAddr1.TabIndex = 1
        '
        'txtAddr1
        '
        Me.txtAddr1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddr1.Location = New System.Drawing.Point(128, 1)
        Me.txtAddr1.MaxLength = 50
        Me.txtAddr1.Name = "txtAddr1"
        Me.txtAddr1.Size = New System.Drawing.Size(197, 20)
        Me.txtAddr1.TabIndex = 0
        Me.txtAddr1.TextToClear = Nothing
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 50
        Me.Label1.Text = "Address 1:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlAddr2
        '
        Me.pnlAddr2.Controls.Add(Me.txtAddr2)
        Me.pnlAddr2.Controls.Add(Me.Label2)
        Me.pnlAddr2.Location = New System.Drawing.Point(3, 115)
        Me.pnlAddr2.Name = "pnlAddr2"
        Me.pnlAddr2.Size = New System.Drawing.Size(339, 22)
        Me.pnlAddr2.TabIndex = 2
        '
        'txtAddr2
        '
        Me.txtAddr2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddr2.Location = New System.Drawing.Point(128, 1)
        Me.txtAddr2.MaxLength = 50
        Me.txtAddr2.Name = "txtAddr2"
        Me.txtAddr2.Size = New System.Drawing.Size(197, 20)
        Me.txtAddr2.TabIndex = 0
        Me.txtAddr2.TextToClear = Nothing
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(12, 2)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 13)
        Me.Label2.TabIndex = 50
        Me.Label2.Text = "Address 2:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlAddr3
        '
        Me.pnlAddr3.Controls.Add(Me.txtAddr3)
        Me.pnlAddr3.Controls.Add(Me.lblAddr3)
        Me.pnlAddr3.Location = New System.Drawing.Point(3, 143)
        Me.pnlAddr3.Name = "pnlAddr3"
        Me.pnlAddr3.Size = New System.Drawing.Size(339, 22)
        Me.pnlAddr3.TabIndex = 51
        Me.pnlAddr3.Visible = False
        '
        'txtAddr3
        '
        Me.txtAddr3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddr3.Location = New System.Drawing.Point(128, 1)
        Me.txtAddr3.MaxLength = 50
        Me.txtAddr3.Name = "txtAddr3"
        Me.txtAddr3.Size = New System.Drawing.Size(197, 20)
        Me.txtAddr3.TabIndex = 0
        Me.txtAddr3.TextToClear = Nothing
        '
        'lblAddr3
        '
        Me.lblAddr3.Location = New System.Drawing.Point(12, 2)
        Me.lblAddr3.Name = "lblAddr3"
        Me.lblAddr3.Size = New System.Drawing.Size(115, 13)
        Me.lblAddr3.TabIndex = 50
        Me.lblAddr3.Text = "Address 3:"
        Me.lblAddr3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlCity
        '
        Me.pnlCity.Controls.Add(Me.txtCity)
        Me.pnlCity.Controls.Add(Me.Label3)
        Me.pnlCity.Location = New System.Drawing.Point(3, 171)
        Me.pnlCity.Name = "pnlCity"
        Me.pnlCity.Size = New System.Drawing.Size(339, 22)
        Me.pnlCity.TabIndex = 3
        '
        'txtCity
        '
        Me.txtCity.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCity.Location = New System.Drawing.Point(128, 1)
        Me.txtCity.MaxLength = 50
        Me.txtCity.Name = "txtCity"
        Me.txtCity.Size = New System.Drawing.Size(197, 20)
        Me.txtCity.TabIndex = 0
        Me.txtCity.TextToClear = Nothing
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(12, 2)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 13)
        Me.Label3.TabIndex = 50
        Me.Label3.Text = "City:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlState
        '
        Me.pnlState.Controls.Add(Me.txtState)
        Me.pnlState.Controls.Add(Me.Label4)
        Me.pnlState.Location = New System.Drawing.Point(3, 199)
        Me.pnlState.Name = "pnlState"
        Me.pnlState.Size = New System.Drawing.Size(339, 22)
        Me.pnlState.TabIndex = 4
        '
        'txtState
        '
        Me.txtState.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtState.Location = New System.Drawing.Point(128, 1)
        Me.txtState.MaxLength = 2
        Me.txtState.Name = "txtState"
        Me.txtState.Size = New System.Drawing.Size(41, 20)
        Me.txtState.TabIndex = 0
        Me.txtState.TextToClear = Nothing
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(12, 2)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(115, 13)
        Me.Label4.TabIndex = 50
        Me.Label4.Text = "State:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlProvince
        '
        Me.pnlProvince.Controls.Add(Me.txtProvince)
        Me.pnlProvince.Controls.Add(Me.Label7)
        Me.pnlProvince.Location = New System.Drawing.Point(3, 227)
        Me.pnlProvince.Name = "pnlProvince"
        Me.pnlProvince.Size = New System.Drawing.Size(339, 22)
        Me.pnlProvince.TabIndex = 7
        '
        'txtProvince
        '
        Me.txtProvince.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProvince.Location = New System.Drawing.Point(128, 1)
        Me.txtProvince.MaxLength = 50
        Me.txtProvince.Name = "txtProvince"
        Me.txtProvince.Size = New System.Drawing.Size(132, 20)
        Me.txtProvince.TabIndex = 0
        Me.txtProvince.TextToClear = Nothing
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(12, 2)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(115, 13)
        Me.Label7.TabIndex = 50
        Me.Label7.Text = "Province / County:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlZip
        '
        Me.pnlZip.Controls.Add(Me.txtZip)
        Me.pnlZip.Controls.Add(Me.Label5)
        Me.pnlZip.Location = New System.Drawing.Point(3, 255)
        Me.pnlZip.Name = "pnlZip"
        Me.pnlZip.Size = New System.Drawing.Size(339, 22)
        Me.pnlZip.TabIndex = 5
        '
        'txtZip
        '
        Me.txtZip.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtZip.Location = New System.Drawing.Point(128, 1)
        Me.txtZip.MaxLength = 15
        Me.txtZip.Name = "txtZip"
        Me.txtZip.Size = New System.Drawing.Size(132, 20)
        Me.txtZip.TabIndex = 0
        Me.txtZip.TextToClear = Nothing
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(12, 2)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 13)
        Me.Label5.TabIndex = 50
        Me.Label5.Text = "Zip:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlCountry
        '
        Me.pnlCountry.Controls.Add(Me.txtCountry)
        Me.pnlCountry.Controls.Add(Me.Label6)
        Me.pnlCountry.Location = New System.Drawing.Point(3, 283)
        Me.pnlCountry.Name = "pnlCountry"
        Me.pnlCountry.Size = New System.Drawing.Size(339, 22)
        Me.pnlCountry.TabIndex = 6
        '
        'txtCountry
        '
        Me.txtCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCountry.Location = New System.Drawing.Point(128, 1)
        Me.txtCountry.MaxLength = 2
        Me.txtCountry.Name = "txtCountry"
        Me.txtCountry.Size = New System.Drawing.Size(41, 20)
        Me.txtCountry.TabIndex = 0
        Me.txtCountry.TextToClear = Nothing
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(12, 2)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(115, 13)
        Me.Label6.TabIndex = 50
        Me.Label6.Text = "Country:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlAddressSearch
        '
        Me.pnlAddressSearch.Controls.Add(Me.cmdAddNew)
        Me.pnlAddressSearch.Controls.Add(Me.lblSearch)
        Me.pnlAddressSearch.Controls.Add(Me.txtSearch)
        Me.pnlAddressSearch.Controls.Add(Me.btnContactMaintenance)
        Me.pnlAddressSearch.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlAddressSearch.Location = New System.Drawing.Point(0, 0)
        Me.pnlAddressSearch.Name = "pnlAddressSearch"
        Me.pnlAddressSearch.Size = New System.Drawing.Size(348, 82)
        Me.pnlAddressSearch.TabIndex = 58
        '
        'cmdAddNew
        '
        Me.cmdAddNew.Enabled = False
        Me.cmdAddNew.Image = My.Resources.Resources.Add_New_Row
        Me.cmdAddNew.Location = New System.Drawing.Point(13, 35)
        Me.cmdAddNew.Name = "cmdAddNew"
        Me.cmdAddNew.Size = New System.Drawing.Size(115, 42)
        Me.cmdAddNew.TabIndex = 55
        Me.cmdAddNew.Text = "Add New _AddressType"
        Me.cmdAddNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdAddNew.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Connection = Nothing
        Me.txtSearch.DisplayMember = "contactname"
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSearch.Location = New System.Drawing.Point(131, 8)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.ResultsRowLimit = 0
        Me.txtSearch.SelectedValue = Nothing
        Me.txtSearch.Size = New System.Drawing.Size(212, 20)
        Me.txtSearch.TabIndex = 0
        Me.txtSearch.TableName = "contacts"
        Me.txtSearch.TextToClear = Nothing
        Me.txtSearch.ValueMember = "contactno"
        '
        'btnContactMaintenance
        '
        Me.btnContactMaintenance.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnContactMaintenance.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnContactMaintenance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnContactMaintenance.Image = My.Resources.Resources.MySharePoints
        Me.btnContactMaintenance.Location = New System.Drawing.Point(311, 45)
        Me.btnContactMaintenance.Name = "btnContactMaintenance"
        Me.btnContactMaintenance.Size = New System.Drawing.Size(32, 32)
        Me.btnContactMaintenance.TabIndex = 51
        Me.btnContactMaintenance.TabStop = False
        Me.btnContactMaintenance.Tag = "Contact Maintenance"
        Me.btnContactMaintenance.UseVisualStyleBackColor = True
        '
        'frmPickAddress
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(348, 443)
        Me.Controls.Add(Me.flpAddNewDetails)
        Me.Controls.Add(Me.pnlAddressSearch)
        Me.Controls.Add(Me.pnlAcceptButtons)
        Me.Name = "frmPickAddress"
        Me.Text = "Choose _AddressType"
        Me.pnlAcceptButtons.ResumeLayout(False)
        Me.flpAddNewDetails.ResumeLayout(False)
        Me.pnlName.ResumeLayout(False)
        Me.pnlName.PerformLayout()
        Me.pnlPhone.ResumeLayout(False)
        Me.pnlPhone.PerformLayout()
        Me.pnlEmail.ResumeLayout(False)
        Me.pnlEmail.PerformLayout()
        Me.pnlAddr1.ResumeLayout(False)
        Me.pnlAddr1.PerformLayout()
        Me.pnlAddr2.ResumeLayout(False)
        Me.pnlAddr2.PerformLayout()
        Me.pnlAddr3.ResumeLayout(False)
        Me.pnlAddr3.PerformLayout()
        Me.pnlCity.ResumeLayout(False)
        Me.pnlCity.PerformLayout()
        Me.pnlState.ResumeLayout(False)
        Me.pnlState.PerformLayout()
        Me.pnlProvince.ResumeLayout(False)
        Me.pnlProvince.PerformLayout()
        Me.pnlZip.ResumeLayout(False)
        Me.pnlZip.PerformLayout()
        Me.pnlCountry.ResumeLayout(False)
        Me.pnlCountry.PerformLayout()
        Me.pnlAddressSearch.ResumeLayout(False)
        Me.pnlAddressSearch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As SearchTextBox
    Friend WithEvents btnContactMaintenance As Button
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOK As Button
    Friend WithEvents cmdAddNew As Button
    Friend WithEvents pnlAcceptButtons As Panel
    Friend WithEvents flpAddNewDetails As FlowLayoutPanel
    Friend WithEvents pnlAddressSearch As Panel
    Friend WithEvents pnlName As Panel
    Friend WithEvents lblName As Label
    Friend WithEvents pnlAddr1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents pnlAddr2 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents pnlCity As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents pnlState As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents pnlZip As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents pnlCountry As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents pnlProvince As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents pnlPhone As Panel
    Friend WithEvents lblPhone As Label
    Friend WithEvents pnlEmail As Panel
    Friend WithEvents lblEmail As Label
    Friend WithEvents txtEmail As HighlightTextBox
    Friend WithEvents txtName As HighlightTextBox
    Friend WithEvents txtAddr1 As HighlightTextBox
    Friend WithEvents txtAddr2 As HighlightTextBox
    Friend WithEvents txtCity As HighlightTextBox
    Friend WithEvents txtState As HighlightTextBox
    Friend WithEvents txtZip As HighlightTextBox
    Friend WithEvents txtCountry As HighlightTextBox
    Friend WithEvents txtProvince As HighlightTextBox
    Friend WithEvents txtPhone As HighlightTextBox
    Friend WithEvents pnlAddr3 As Panel
    Friend WithEvents txtAddr3 As HighlightTextBox
    Friend WithEvents lblAddr3 As Label
End Class
