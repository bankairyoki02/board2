<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrintZPLDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrintZPLDialog))
        Me.cbBarcodeType = New System.Windows.Forms.ComboBox()
        Me.lbBarcodeType = New System.Windows.Forms.Label()
        Me.lbPrinter = New System.Windows.Forms.Label()
        Me.cbPrinters = New System.Windows.Forms.ComboBox()
        Me.lbXCalibration = New System.Windows.Forms.Label()
        Me.nupXCalibration = New System.Windows.Forms.NumericUpDown()
        Me.nupYCalibration = New System.Windows.Forms.NumericUpDown()
        Me.lbYCalibration = New System.Windows.Forms.Label()
        Me.nupPrintCopies = New System.Windows.Forms.NumericUpDown()
        Me.lbPrintCopies = New System.Windows.Forms.Label()
        Me.lbBarcode = New System.Windows.Forms.Label()
        Me.btnPreviewBarcode = New System.Windows.Forms.Button()
        Me.btnPrintBarcode = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.picBarcodePreview = New System.Windows.Forms.PictureBox()
        Me.btnXCalibrationDown = New System.Windows.Forms.Button()
        Me.btnXCalibrationUp = New System.Windows.Forms.Button()
        Me.lbSerialNumberValue = New System.Windows.Forms.Label()
        Me.lbSerialNumber = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnPreviewNextBarcode = New System.Windows.Forms.Button()
        Me.btnPreviewPrevBarcode = New System.Windows.Forms.Button()
        Me.cbBarcode = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.nupXCalibration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nupYCalibration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nupPrintCopies, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBarcodePreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbBarcodeType
        '
        Me.cbBarcodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBarcodeType.FormattingEnabled = True
        Me.cbBarcodeType.Location = New System.Drawing.Point(97, 78)
        Me.cbBarcodeType.Name = "cbBarcodeType"
        Me.cbBarcodeType.Size = New System.Drawing.Size(171, 21)
        Me.cbBarcodeType.TabIndex = 2
        '
        'lbBarcodeType
        '
        Me.lbBarcodeType.AutoSize = True
        Me.lbBarcodeType.Location = New System.Drawing.Point(14, 81)
        Me.lbBarcodeType.Name = "lbBarcodeType"
        Me.lbBarcodeType.Size = New System.Drawing.Size(77, 13)
        Me.lbBarcodeType.TabIndex = 1
        Me.lbBarcodeType.Text = "Barcode Type:"
        '
        'lbPrinter
        '
        Me.lbPrinter.AutoSize = True
        Me.lbPrinter.Location = New System.Drawing.Point(14, 54)
        Me.lbPrinter.Name = "lbPrinter"
        Me.lbPrinter.Size = New System.Drawing.Size(40, 13)
        Me.lbPrinter.TabIndex = 3
        Me.lbPrinter.Text = "Printer:"
        '
        'cbPrinters
        '
        Me.cbPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrinters.FormattingEnabled = True
        Me.cbPrinters.Location = New System.Drawing.Point(97, 51)
        Me.cbPrinters.Name = "cbPrinters"
        Me.cbPrinters.Size = New System.Drawing.Size(171, 21)
        Me.cbPrinters.TabIndex = 1
        '
        'lbXCalibration
        '
        Me.lbXCalibration.AutoSize = True
        Me.lbXCalibration.Location = New System.Drawing.Point(14, 108)
        Me.lbXCalibration.Name = "lbXCalibration"
        Me.lbXCalibration.Size = New System.Drawing.Size(69, 13)
        Me.lbXCalibration.TabIndex = 5
        Me.lbXCalibration.Text = "X Calibration:"
        '
        'nupXCalibration
        '
        Me.nupXCalibration.Location = New System.Drawing.Point(97, 105)
        Me.nupXCalibration.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nupXCalibration.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nupXCalibration.Name = "nupXCalibration"
        Me.nupXCalibration.Size = New System.Drawing.Size(120, 20)
        Me.nupXCalibration.TabIndex = 3
        '
        'nupYCalibration
        '
        Me.nupYCalibration.Location = New System.Drawing.Point(97, 131)
        Me.nupYCalibration.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nupYCalibration.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nupYCalibration.Name = "nupYCalibration"
        Me.nupYCalibration.Size = New System.Drawing.Size(120, 20)
        Me.nupYCalibration.TabIndex = 4
        '
        'lbYCalibration
        '
        Me.lbYCalibration.AutoSize = True
        Me.lbYCalibration.Location = New System.Drawing.Point(14, 134)
        Me.lbYCalibration.Name = "lbYCalibration"
        Me.lbYCalibration.Size = New System.Drawing.Size(69, 13)
        Me.lbYCalibration.TabIndex = 7
        Me.lbYCalibration.Text = "Y Calibration:"
        '
        'nupPrintCopies
        '
        Me.nupPrintCopies.Location = New System.Drawing.Point(97, 157)
        Me.nupPrintCopies.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nupPrintCopies.Name = "nupPrintCopies"
        Me.nupPrintCopies.Size = New System.Drawing.Size(120, 20)
        Me.nupPrintCopies.TabIndex = 5
        Me.nupPrintCopies.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lbPrintCopies
        '
        Me.lbPrintCopies.AutoSize = True
        Me.lbPrintCopies.Location = New System.Drawing.Point(14, 160)
        Me.lbPrintCopies.Name = "lbPrintCopies"
        Me.lbPrintCopies.Size = New System.Drawing.Size(42, 13)
        Me.lbPrintCopies.TabIndex = 9
        Me.lbPrintCopies.Text = "Copies:"
        '
        'lbBarcode
        '
        Me.lbBarcode.AutoSize = True
        Me.lbBarcode.Location = New System.Drawing.Point(14, 11)
        Me.lbBarcode.Name = "lbBarcode"
        Me.lbBarcode.Size = New System.Drawing.Size(50, 13)
        Me.lbBarcode.TabIndex = 11
        Me.lbBarcode.Text = "Barcode:"
        '
        'btnPreviewBarcode
        '
        Me.btnPreviewBarcode.Location = New System.Drawing.Point(83, 183)
        Me.btnPreviewBarcode.Name = "btnPreviewBarcode"
        Me.btnPreviewBarcode.Size = New System.Drawing.Size(112, 32)
        Me.btnPreviewBarcode.TabIndex = 6
        Me.btnPreviewBarcode.Text = "Update Preview"
        Me.btnPreviewBarcode.UseVisualStyleBackColor = True
        Me.btnPreviewBarcode.Visible = False
        '
        'btnPrintBarcode
        '
        Me.btnPrintBarcode.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnPrintBarcode.Location = New System.Drawing.Point(81, 3)
        Me.btnPrintBarcode.Name = "btnPrintBarcode"
        Me.btnPrintBarcode.Size = New System.Drawing.Size(75, 21)
        Me.btnPrintBarcode.TabIndex = 7
        Me.btnPrintBarcode.Text = "&Print All"
        Me.btnPrintBarcode.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCancel.Location = New System.Drawing.Point(176, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 21)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'picBarcodePreview
        '
        Me.picBarcodePreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picBarcodePreview.Location = New System.Drawing.Point(12, 232)
        Me.picBarcodePreview.Name = "picBarcodePreview"
        Me.picBarcodePreview.Size = New System.Drawing.Size(309, 158)
        Me.picBarcodePreview.TabIndex = 16
        Me.picBarcodePreview.TabStop = False
        '
        'btnXCalibrationDown
        '
        Me.btnXCalibrationDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 5.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnXCalibrationDown.Location = New System.Drawing.Point(198, 105)
        Me.btnXCalibrationDown.Margin = New System.Windows.Forms.Padding(0)
        Me.btnXCalibrationDown.Name = "btnXCalibrationDown"
        Me.btnXCalibrationDown.Size = New System.Drawing.Size(11, 20)
        Me.btnXCalibrationDown.TabIndex = 17
        Me.btnXCalibrationDown.Text = "<"
        Me.btnXCalibrationDown.UseVisualStyleBackColor = True
        '
        'btnXCalibrationUp
        '
        Me.btnXCalibrationUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 5.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnXCalibrationUp.Location = New System.Drawing.Point(208, 105)
        Me.btnXCalibrationUp.Margin = New System.Windows.Forms.Padding(0)
        Me.btnXCalibrationUp.Name = "btnXCalibrationUp"
        Me.btnXCalibrationUp.Size = New System.Drawing.Size(11, 20)
        Me.btnXCalibrationUp.TabIndex = 18
        Me.btnXCalibrationUp.Text = ">"
        Me.btnXCalibrationUp.UseVisualStyleBackColor = True
        '
        'lbSerialNumberValue
        '
        Me.lbSerialNumberValue.AutoSize = True
        Me.lbSerialNumberValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbSerialNumberValue.Location = New System.Drawing.Point(94, 33)
        Me.lbSerialNumberValue.Name = "lbSerialNumberValue"
        Me.lbSerialNumberValue.Size = New System.Drawing.Size(141, 13)
        Me.lbSerialNumberValue.TabIndex = 20
        Me.lbSerialNumberValue.Text = "...SerialNumber value here..."
        '
        'lbSerialNumber
        '
        Me.lbSerialNumber.AutoSize = True
        Me.lbSerialNumber.Location = New System.Drawing.Point(14, 32)
        Me.lbSerialNumber.Name = "lbSerialNumber"
        Me.lbSerialNumber.Size = New System.Drawing.Size(46, 13)
        Me.lbSerialNumber.TabIndex = 19
        Me.lbSerialNumber.Text = "Serial #:"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnCancel, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnPrintBarcode, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(1, 397)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(333, 27)
        Me.TableLayoutPanel1.TabIndex = 21
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnPreviewNextBarcode)
        Me.Panel1.Controls.Add(Me.btnPreviewPrevBarcode)
        Me.Panel1.Controls.Add(Me.cbBarcode)
        Me.Panel1.Controls.Add(Me.cbBarcodeType)
        Me.Panel1.Controls.Add(Me.lbBarcodeType)
        Me.Panel1.Controls.Add(Me.lbSerialNumberValue)
        Me.Panel1.Controls.Add(Me.cbPrinters)
        Me.Panel1.Controls.Add(Me.lbSerialNumber)
        Me.Panel1.Controls.Add(Me.lbPrinter)
        Me.Panel1.Controls.Add(Me.btnXCalibrationUp)
        Me.Panel1.Controls.Add(Me.lbXCalibration)
        Me.Panel1.Controls.Add(Me.btnXCalibrationDown)
        Me.Panel1.Controls.Add(Me.nupXCalibration)
        Me.Panel1.Controls.Add(Me.lbYCalibration)
        Me.Panel1.Controls.Add(Me.btnPreviewBarcode)
        Me.Panel1.Controls.Add(Me.nupYCalibration)
        Me.Panel1.Controls.Add(Me.lbPrintCopies)
        Me.Panel1.Controls.Add(Me.lbBarcode)
        Me.Panel1.Controls.Add(Me.nupPrintCopies)
        Me.Panel1.Location = New System.Drawing.Point(29, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(274, 217)
        Me.Panel1.TabIndex = 22
        '
        'btnPreviewNextBarcode
        '
        Me.btnPreviewNextBarcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPreviewNextBarcode.Location = New System.Drawing.Point(244, 189)
        Me.btnPreviewNextBarcode.Margin = New System.Windows.Forms.Padding(0)
        Me.btnPreviewNextBarcode.Name = "btnPreviewNextBarcode"
        Me.btnPreviewNextBarcode.Size = New System.Drawing.Size(24, 26)
        Me.btnPreviewNextBarcode.TabIndex = 23
        Me.btnPreviewNextBarcode.Text = ">"
        Me.btnPreviewNextBarcode.UseVisualStyleBackColor = True
        '
        'btnPreviewPrevBarcode
        '
        Me.btnPreviewPrevBarcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPreviewPrevBarcode.Location = New System.Drawing.Point(17, 189)
        Me.btnPreviewPrevBarcode.Margin = New System.Windows.Forms.Padding(0)
        Me.btnPreviewPrevBarcode.Name = "btnPreviewPrevBarcode"
        Me.btnPreviewPrevBarcode.Size = New System.Drawing.Size(24, 26)
        Me.btnPreviewPrevBarcode.TabIndex = 22
        Me.btnPreviewPrevBarcode.Text = "<"
        Me.btnPreviewPrevBarcode.UseVisualStyleBackColor = True
        '
        'cbBarcode
        '
        Me.cbBarcode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBarcode.FormattingEnabled = True
        Me.cbBarcode.Location = New System.Drawing.Point(97, 8)
        Me.cbBarcode.Name = "cbBarcode"
        Me.cbBarcode.Size = New System.Drawing.Size(171, 21)
        Me.cbBarcode.TabIndex = 21
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel1, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(1, 2)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(333, 223)
        Me.TableLayoutPanel2.TabIndex = 23
        '
        'frmPrintZPLDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 426)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.picBarcodePreview)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximumSize = New System.Drawing.Size(350, 465)
        Me.MinimumSize = New System.Drawing.Size(305, 400)
        Me.Name = "frmPrintZPLDialog"
        Me.Text = "Zebra Print"
        CType(Me.nupXCalibration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nupYCalibration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nupPrintCopies, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBarcodePreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cbBarcodeType As ComboBox
    Friend WithEvents lbBarcodeType As Label
    Friend WithEvents lbPrinter As Label
    Friend WithEvents cbPrinters As ComboBox
    Friend WithEvents lbXCalibration As Label
    Friend WithEvents nupXCalibration As NumericUpDown
    Friend WithEvents nupYCalibration As NumericUpDown
    Friend WithEvents lbYCalibration As Label
    Friend WithEvents nupPrintCopies As NumericUpDown
    Friend WithEvents lbPrintCopies As Label
    Friend WithEvents lbBarcode As Label
    Friend WithEvents btnPreviewBarcode As Button
    Friend WithEvents btnPrintBarcode As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents picBarcodePreview As PictureBox
    Friend WithEvents btnXCalibrationDown As Button
    Friend WithEvents btnXCalibrationUp As Button
    Friend WithEvents lbSerialNumberValue As Label
    Friend WithEvents lbSerialNumber As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents cbBarcode As ComboBox
    Friend WithEvents btnPreviewNextBarcode As Button
    Friend WithEvents btnPreviewPrevBarcode As Button
End Class
