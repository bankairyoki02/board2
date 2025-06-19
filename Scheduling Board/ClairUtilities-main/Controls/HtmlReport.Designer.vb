<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HtmlReport
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HtmlReport))
        Me.scHTML = New System.Windows.Forms.SplitContainer()
        Me.flpHTML = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlHTMLBlock = New System.Windows.Forms.Panel()
        Me.txtHTMLBlockSQL = New System.Windows.Forms.TextBox()
        Me.chkIsGroupData = New System.Windows.Forms.CheckBox()
        Me.cmdAddSQLtoHTMLBlock = New System.Windows.Forms.Button()
        Me.txtHTMLBlockHTML = New System.Windows.Forms.TextBox()
        Me.pnlHTMLBlockControls = New System.Windows.Forms.Panel()
        Me.txtHTMLBlockDescription = New System.Windows.Forms.TextBox()
        Me.pnlEditReportControls = New System.Windows.Forms.Panel()
        Me.cmdAddHTMLBlock = New System.Windows.Forms.Button()
        Me.cmdSaveReportLayout = New System.Windows.Forms.Button()
        Me.cmdGetHTMLBlocks = New System.Windows.Forms.Button()
        Me.lblReportError = New System.Windows.Forms.Label()
        Me.Browser = New System.Windows.Forms.WebBrowser()
        Me.tsHTMLReportOptions = New System.Windows.Forms.ToolStrip()
        Me.tsbRefresh = New System.Windows.Forms.ToolStripButton()
        Me.tssRefresh = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbExportHTMLReportToWord = New System.Windows.Forms.ToolStripButton()
        Me.tsbExportHTMLReportToPDF = New System.Windows.Forms.ToolStripButton()
        Me.tssExport = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbEmailHTMLReport = New System.Windows.Forms.ToolStripButton()
        Me.tsbEmailWithPDF = New System.Windows.Forms.ToolStripButton()
        Me.tsbPrintPreview = New System.Windows.Forms.ToolStripButton()
        Me.cmdEditReportHTMLSQL = New System.Windows.Forms.ToolStripButton()
        Me.tscboLanguage = New System.Windows.Forms.ToolStripComboBox()
        Me.tslblLanguage = New System.Windows.Forms.ToolStripLabel()
        Me.tsbPrint = New System.Windows.Forms.ToolStripButton()
        Me.tsbPrintBitmap = New System.Windows.Forms.ToolStripButton()
        Me.tsbUseAppPrinter = New System.Windows.Forms.ToolStripButton()
        Me.tsbSetAppPrinter = New System.Windows.Forms.ToolStripButton()
        CType(Me.scHTML, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scHTML.Panel1.SuspendLayout()
        Me.scHTML.Panel2.SuspendLayout()
        Me.scHTML.SuspendLayout()
        Me.flpHTML.SuspendLayout()
        Me.pnlHTMLBlock.SuspendLayout()
        Me.pnlEditReportControls.SuspendLayout()
        Me.tsHTMLReportOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'scHTML
        '
        Me.scHTML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scHTML.Location = New System.Drawing.Point(0, 27)
        Me.scHTML.Name = "scHTML"
        '
        'scHTML.Panel1
        '
        Me.scHTML.Panel1.Controls.Add(Me.flpHTML)
        Me.scHTML.Panel1.Controls.Add(Me.pnlEditReportControls)
        '
        'scHTML.Panel2
        '
        Me.scHTML.Panel2.Controls.Add(Me.lblReportError)
        Me.scHTML.Panel2.Controls.Add(Me.Browser)
        Me.scHTML.Size = New System.Drawing.Size(1245, 588)
        Me.scHTML.SplitterDistance = 357
        Me.scHTML.TabIndex = 5
        '
        'flpHTML
        '
        Me.flpHTML.AutoScroll = True
        Me.flpHTML.Controls.Add(Me.pnlHTMLBlock)
        Me.flpHTML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpHTML.Location = New System.Drawing.Point(0, 0)
        Me.flpHTML.Name = "flpHTML"
        Me.flpHTML.Size = New System.Drawing.Size(357, 544)
        Me.flpHTML.TabIndex = 0
        '
        'pnlHTMLBlock
        '
        Me.pnlHTMLBlock.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlHTMLBlock.Controls.Add(Me.txtHTMLBlockSQL)
        Me.pnlHTMLBlock.Controls.Add(Me.chkIsGroupData)
        Me.pnlHTMLBlock.Controls.Add(Me.cmdAddSQLtoHTMLBlock)
        Me.pnlHTMLBlock.Controls.Add(Me.txtHTMLBlockHTML)
        Me.pnlHTMLBlock.Controls.Add(Me.pnlHTMLBlockControls)
        Me.pnlHTMLBlock.Controls.Add(Me.txtHTMLBlockDescription)
        Me.pnlHTMLBlock.Location = New System.Drawing.Point(3, 3)
        Me.pnlHTMLBlock.Name = "pnlHTMLBlock"
        Me.pnlHTMLBlock.Size = New System.Drawing.Size(298, 273)
        Me.pnlHTMLBlock.TabIndex = 0
        Me.pnlHTMLBlock.Visible = False
        '
        'txtHTMLBlockSQL
        '
        Me.txtHTMLBlockSQL.AcceptsReturn = True
        Me.txtHTMLBlockSQL.AcceptsTab = True
        Me.txtHTMLBlockSQL.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtHTMLBlockSQL.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHTMLBlockSQL.Location = New System.Drawing.Point(0, 147)
        Me.txtHTMLBlockSQL.Multiline = True
        Me.txtHTMLBlockSQL.Name = "txtHTMLBlockSQL"
        Me.txtHTMLBlockSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtHTMLBlockSQL.Size = New System.Drawing.Size(298, 100)
        Me.txtHTMLBlockSQL.TabIndex = 3
        Me.txtHTMLBlockSQL.Tag = "SQLQuery"
        Me.txtHTMLBlockSQL.Text = "SQL"
        Me.txtHTMLBlockSQL.WordWrap = False
        '
        'chkIsGroupData
        '
        Me.chkIsGroupData.AutoSize = True
        Me.chkIsGroupData.Location = New System.Drawing.Point(113, 97)
        Me.chkIsGroupData.Name = "chkIsGroupData"
        Me.chkIsGroupData.Size = New System.Drawing.Size(111, 17)
        Me.chkIsGroupData.TabIndex = 5
        Me.chkIsGroupData.Text = "Group Data Block"
        Me.chkIsGroupData.UseVisualStyleBackColor = True
        '
        'cmdAddSQLtoHTMLBlock
        '
        Me.cmdAddSQLtoHTMLBlock.Location = New System.Drawing.Point(99, 69)
        Me.cmdAddSQLtoHTMLBlock.Name = "cmdAddSQLtoHTMLBlock"
        Me.cmdAddSQLtoHTMLBlock.Size = New System.Drawing.Size(143, 22)
        Me.cmdAddSQLtoHTMLBlock.TabIndex = 4
        Me.cmdAddSQLtoHTMLBlock.Text = "Add SQL to HTML Block"
        Me.cmdAddSQLtoHTMLBlock.UseVisualStyleBackColor = True
        Me.cmdAddSQLtoHTMLBlock.Visible = False
        '
        'txtHTMLBlockHTML
        '
        Me.txtHTMLBlockHTML.AcceptsReturn = True
        Me.txtHTMLBlockHTML.AcceptsTab = True
        Me.txtHTMLBlockHTML.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtHTMLBlockHTML.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHTMLBlockHTML.Location = New System.Drawing.Point(0, 22)
        Me.txtHTMLBlockHTML.Multiline = True
        Me.txtHTMLBlockHTML.Name = "txtHTMLBlockHTML"
        Me.txtHTMLBlockHTML.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtHTMLBlockHTML.Size = New System.Drawing.Size(298, 125)
        Me.txtHTMLBlockHTML.TabIndex = 1
        Me.txtHTMLBlockHTML.Tag = "HTML"
        Me.txtHTMLBlockHTML.Text = "<HTML>"
        Me.txtHTMLBlockHTML.WordWrap = False
        '
        'pnlHTMLBlockControls
        '
        Me.pnlHTMLBlockControls.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlHTMLBlockControls.Location = New System.Drawing.Point(0, 251)
        Me.pnlHTMLBlockControls.Name = "pnlHTMLBlockControls"
        Me.pnlHTMLBlockControls.Size = New System.Drawing.Size(298, 22)
        Me.pnlHTMLBlockControls.TabIndex = 4
        '
        'txtHTMLBlockDescription
        '
        Me.txtHTMLBlockDescription.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtHTMLBlockDescription.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHTMLBlockDescription.Location = New System.Drawing.Point(0, 0)
        Me.txtHTMLBlockDescription.Name = "txtHTMLBlockDescription"
        Me.txtHTMLBlockDescription.Size = New System.Drawing.Size(298, 22)
        Me.txtHTMLBlockDescription.TabIndex = 2
        Me.txtHTMLBlockDescription.Tag = "Description"
        Me.txtHTMLBlockDescription.Text = "HTML Block Description"
        '
        'pnlEditReportControls
        '
        Me.pnlEditReportControls.Controls.Add(Me.cmdAddHTMLBlock)
        Me.pnlEditReportControls.Controls.Add(Me.cmdSaveReportLayout)
        Me.pnlEditReportControls.Controls.Add(Me.cmdGetHTMLBlocks)
        Me.pnlEditReportControls.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlEditReportControls.Location = New System.Drawing.Point(0, 544)
        Me.pnlEditReportControls.Name = "pnlEditReportControls"
        Me.pnlEditReportControls.Size = New System.Drawing.Size(357, 44)
        Me.pnlEditReportControls.TabIndex = 1
        '
        'cmdAddHTMLBlock
        '
        Me.cmdAddHTMLBlock.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAddHTMLBlock.Location = New System.Drawing.Point(205, 6)
        Me.cmdAddHTMLBlock.Name = "cmdAddHTMLBlock"
        Me.cmdAddHTMLBlock.Size = New System.Drawing.Size(68, 35)
        Me.cmdAddHTMLBlock.TabIndex = 3
        Me.cmdAddHTMLBlock.Text = "Add HTML Block"
        Me.cmdAddHTMLBlock.UseVisualStyleBackColor = True
        '
        'cmdSaveReportLayout
        '
        Me.cmdSaveReportLayout.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSaveReportLayout.Location = New System.Drawing.Point(279, 6)
        Me.cmdSaveReportLayout.Name = "cmdSaveReportLayout"
        Me.cmdSaveReportLayout.Size = New System.Drawing.Size(75, 35)
        Me.cmdSaveReportLayout.TabIndex = 2
        Me.cmdSaveReportLayout.Text = "Save Report Layout"
        Me.cmdSaveReportLayout.UseVisualStyleBackColor = True
        '
        'cmdGetHTMLBlocks
        '
        Me.cmdGetHTMLBlocks.Location = New System.Drawing.Point(3, 6)
        Me.cmdGetHTMLBlocks.Name = "cmdGetHTMLBlocks"
        Me.cmdGetHTMLBlocks.Size = New System.Drawing.Size(66, 35)
        Me.cmdGetHTMLBlocks.TabIndex = 1
        Me.cmdGetHTMLBlocks.Text = "Get HTML Blocks"
        Me.cmdGetHTMLBlocks.UseVisualStyleBackColor = True
        '
        'lblReportError
        '
        Me.lblReportError.BackColor = System.Drawing.Color.White
        Me.lblReportError.Location = New System.Drawing.Point(139, 226)
        Me.lblReportError.Name = "lblReportError"
        Me.lblReportError.Size = New System.Drawing.Size(100, 23)
        Me.lblReportError.TabIndex = 1
        Me.lblReportError.Text = "Load Report Error:"
        Me.lblReportError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblReportError.Visible = False
        '
        'Browser
        '
        Me.Browser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Browser.Location = New System.Drawing.Point(0, 0)
        Me.Browser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.Browser.Name = "Browser"
        Me.Browser.Size = New System.Drawing.Size(884, 588)
        Me.Browser.TabIndex = 0
        Me.Browser.TabStop = False
        '
        'tsHTMLReportOptions
        '
        Me.tsHTMLReportOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsHTMLReportOptions.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsHTMLReportOptions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbRefresh, Me.tssRefresh, Me.tsbExportHTMLReportToWord, Me.tsbExportHTMLReportToPDF, Me.tssExport, Me.tsbEmailHTMLReport, Me.tsbEmailWithPDF, Me.tsbPrintPreview, Me.cmdEditReportHTMLSQL, Me.tscboLanguage, Me.tslblLanguage, Me.tsbPrint, Me.tsbPrintBitmap, Me.tsbUseAppPrinter, Me.tsbSetAppPrinter})
        Me.tsHTMLReportOptions.Location = New System.Drawing.Point(0, 0)
        Me.tsHTMLReportOptions.Name = "tsHTMLReportOptions"
        Me.tsHTMLReportOptions.Size = New System.Drawing.Size(1245, 27)
        Me.tsHTMLReportOptions.TabIndex = 4
        Me.tsHTMLReportOptions.Text = "HTML Report Options"
        '
        'tsbRefresh
        '
        Me.tsbRefresh.Image = CType(resources.GetObject("tsbRefresh.Image"), System.Drawing.Image)
        Me.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRefresh.Name = "tsbRefresh"
        Me.tsbRefresh.Size = New System.Drawing.Size(70, 24)
        Me.tsbRefresh.Text = "Refresh"
        '
        'tssRefresh
        '
        Me.tssRefresh.Name = "tssRefresh"
        Me.tssRefresh.Size = New System.Drawing.Size(6, 27)
        '
        'tsbExportHTMLReportToWord
        '
        Me.tsbExportHTMLReportToWord.Image = CType(resources.GetObject("tsbExportHTMLReportToWord.Image"), System.Drawing.Image)
        Me.tsbExportHTMLReportToWord.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExportHTMLReportToWord.Name = "tsbExportHTMLReportToWord"
        Me.tsbExportHTMLReportToWord.Size = New System.Drawing.Size(65, 24)
        Me.tsbExportHTMLReportToWord.Text = "Export"
        Me.tsbExportHTMLReportToWord.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'tsbExportHTMLReportToPDF
        '
        Me.tsbExportHTMLReportToPDF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbExportHTMLReportToPDF.Image = CType(resources.GetObject("tsbExportHTMLReportToPDF.Image"), System.Drawing.Image)
        Me.tsbExportHTMLReportToPDF.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExportHTMLReportToPDF.Name = "tsbExportHTMLReportToPDF"
        Me.tsbExportHTMLReportToPDF.Size = New System.Drawing.Size(24, 24)
        Me.tsbExportHTMLReportToPDF.Text = "PDF"
        '
        'tssExport
        '
        Me.tssExport.Name = "tssExport"
        Me.tssExport.Size = New System.Drawing.Size(6, 27)
        '
        'tsbEmailHTMLReport
        '
        Me.tsbEmailHTMLReport.Image = CType(resources.GetObject("tsbEmailHTMLReport.Image"), System.Drawing.Image)
        Me.tsbEmailHTMLReport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEmailHTMLReport.Name = "tsbEmailHTMLReport"
        Me.tsbEmailHTMLReport.Size = New System.Drawing.Size(65, 24)
        Me.tsbEmailHTMLReport.Text = "E-Mail"
        '
        'tsbEmailWithPDF
        '
        Me.tsbEmailWithPDF.Image = CType(resources.GetObject("tsbEmailWithPDF.Image"), System.Drawing.Image)
        Me.tsbEmailWithPDF.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEmailWithPDF.Name = "tsbEmailWithPDF"
        Me.tsbEmailWithPDF.Size = New System.Drawing.Size(106, 24)
        Me.tsbEmailWithPDF.Text = "E-Mail w/ PDF"
        '
        'tsbPrintPreview
        '
        Me.tsbPrintPreview.Image = CType(resources.GetObject("tsbPrintPreview.Image"), System.Drawing.Image)
        Me.tsbPrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPrintPreview.Name = "tsbPrintPreview"
        Me.tsbPrintPreview.Size = New System.Drawing.Size(100, 24)
        Me.tsbPrintPreview.Text = "Print Preview"
        '
        'cmdEditReportHTMLSQL
        '
        Me.cmdEditReportHTMLSQL.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.cmdEditReportHTMLSQL.Image = CType(resources.GetObject("cmdEditReportHTMLSQL.Image"), System.Drawing.Image)
        Me.cmdEditReportHTMLSQL.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdEditReportHTMLSQL.Name = "cmdEditReportHTMLSQL"
        Me.cmdEditReportHTMLSQL.Size = New System.Drawing.Size(150, 24)
        Me.cmdEditReportHTMLSQL.Text = "Edit Report HTML/SQL"
        Me.cmdEditReportHTMLSQL.Visible = False
        '
        'tscboLanguage
        '
        Me.tscboLanguage.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tscboLanguage.Name = "tscboLanguage"
        Me.tscboLanguage.Size = New System.Drawing.Size(140, 27)
        '
        'tslblLanguage
        '
        Me.tslblLanguage.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tslblLanguage.Name = "tslblLanguage"
        Me.tslblLanguage.Size = New System.Drawing.Size(62, 24)
        Me.tslblLanguage.Text = "Language:"
        '
        'tsbPrint
        '
        Me.tsbPrint.Image = CType(resources.GetObject("tsbPrint.Image"), System.Drawing.Image)
        Me.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPrint.Name = "tsbPrint"
        Me.tsbPrint.Size = New System.Drawing.Size(56, 24)
        Me.tsbPrint.Text = "Print"
        '
        'tsbPrintBitmap
        '
        Me.tsbPrintBitmap.Image = CType(resources.GetObject("tsbPrintBitmap.Image"), System.Drawing.Image)
        Me.tsbPrintBitmap.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPrintBitmap.Name = "tsbPrintBitmap"
        Me.tsbPrintBitmap.Size = New System.Drawing.Size(97, 24)
        Me.tsbPrintBitmap.Text = "Print Bitmap"
        '
        'tsbUseAppPrinter
        '
        Me.tsbUseAppPrinter.CheckOnClick = True
        Me.tsbUseAppPrinter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbUseAppPrinter.Image = CType(resources.GetObject("tsbUseAppPrinter.Image"), System.Drawing.Image)
        Me.tsbUseAppPrinter.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbUseAppPrinter.Name = "tsbUseAppPrinter"
        Me.tsbUseAppPrinter.Size = New System.Drawing.Size(93, 24)
        Me.tsbUseAppPrinter.Text = "Use App Printer"
        '
        'tsbSetAppPrinter
        '
        Me.tsbSetAppPrinter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbSetAppPrinter.Image = CType(resources.GetObject("tsbSetAppPrinter.Image"), System.Drawing.Image)
        Me.tsbSetAppPrinter.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSetAppPrinter.Name = "tsbSetAppPrinter"
        Me.tsbSetAppPrinter.Size = New System.Drawing.Size(90, 24)
        Me.tsbSetAppPrinter.Text = "Set App Printer"
        '
        'HtmlReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.scHTML)
        Me.Controls.Add(Me.tsHTMLReportOptions)
        Me.Name = "HtmlReport"
        Me.Size = New System.Drawing.Size(1245, 615)
        Me.scHTML.Panel1.ResumeLayout(False)
        Me.scHTML.Panel2.ResumeLayout(False)
        CType(Me.scHTML, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scHTML.ResumeLayout(False)
        Me.flpHTML.ResumeLayout(False)
        Me.pnlHTMLBlock.ResumeLayout(False)
        Me.pnlHTMLBlock.PerformLayout()
        Me.pnlEditReportControls.ResumeLayout(False)
        Me.tsHTMLReportOptions.ResumeLayout(False)
        Me.tsHTMLReportOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents scHTML As SplitContainer
    Friend WithEvents flpHTML As FlowLayoutPanel
    Friend WithEvents pnlHTMLBlock As Panel
    Friend WithEvents cmdAddSQLtoHTMLBlock As Button
    Friend WithEvents txtHTMLBlockHTML As TextBox
    Friend WithEvents txtHTMLBlockSQL As TextBox
    Friend WithEvents pnlHTMLBlockControls As Panel
    Friend WithEvents txtHTMLBlockDescription As TextBox
    Friend WithEvents pnlEditReportControls As Panel
    Friend WithEvents cmdAddHTMLBlock As Button
    Friend WithEvents cmdSaveReportLayout As Button
    Friend WithEvents cmdGetHTMLBlocks As Button
    Friend WithEvents lblReportError As Label
    Friend WithEvents Browser As WebBrowser
    Friend WithEvents tsHTMLReportOptions As ToolStrip
    Friend WithEvents tsbRefresh As ToolStripButton
    Friend WithEvents tsbExportHTMLReportToWord As ToolStripButton
    Friend WithEvents tsbEmailHTMLReport As ToolStripButton
    Friend WithEvents tsbPrintPreview As ToolStripButton
    Friend WithEvents cmdEditReportHTMLSQL As ToolStripButton
    Friend WithEvents tscboLanguage As ToolStripComboBox
    Friend WithEvents tslblLanguage As ToolStripLabel
    Friend WithEvents tsbExportHTMLReportToPDF As ToolStripButton
    Friend WithEvents tssRefresh As ToolStripSeparator
    Friend WithEvents tssExport As ToolStripSeparator
    Friend WithEvents tsbPrint As ToolStripButton
    Friend WithEvents tsbUseAppPrinter As ToolStripButton
    Friend WithEvents tsbPrintBitmap As ToolStripButton
    Friend WithEvents tsbSetAppPrinter As ToolStripButton
    Friend WithEvents chkIsGroupData As CheckBox
    Friend WithEvents tsbEmailWithPDF As ToolStripButton
End Class
