<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SysproSupplierPicker
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SysproSupplierPicker))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tslSearch = New System.Windows.Forms.ToolStripLabel()
        Me.tstbSearchTerms = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.tsbCancel = New System.Windows.Forms.ToolStripButton()
        Me.lvSupplierList = New System.Windows.Forms.ListView()
        Me.chSupplierCode = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ToolStrip1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslSearch, Me.tstbSearchTerms})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(800, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tslSearch
        '
        Me.tslSearch.Name = "tslSearch"
        Me.tslSearch.Size = New System.Drawing.Size(45, 22)
        Me.tslSearch.Text = "Search:"
        '
        'tstbSearchTerms
        '
        Me.tstbSearchTerms.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tstbSearchTerms.Name = "tstbSearchTerms"
        Me.tstbSearchTerms.Size = New System.Drawing.Size(200, 25)
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbCancel})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 425)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(800, 25)
        Me.ToolStrip2.TabIndex = 1
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'tsbCancel
        '
        Me.tsbCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbCancel.Image = CType(resources.GetObject("tsbCancel.Image"), System.Drawing.Image)
        Me.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCancel.Name = "tsbCancel"
        Me.tsbCancel.Size = New System.Drawing.Size(47, 22)
        Me.tsbCancel.Text = "Cancel"
        '
        'lvSupplierList
        '
        Me.lvSupplierList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chSupplierCode, Me.chName})
        Me.lvSupplierList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvSupplierList.FullRowSelect = True
        Me.lvSupplierList.HideSelection = False
        Me.lvSupplierList.Location = New System.Drawing.Point(0, 25)
        Me.lvSupplierList.MultiSelect = False
        Me.lvSupplierList.Name = "lvSupplierList"
        Me.lvSupplierList.Size = New System.Drawing.Size(800, 400)
        Me.lvSupplierList.TabIndex = 2
        Me.lvSupplierList.UseCompatibleStateImageBehavior = False
        Me.lvSupplierList.View = System.Windows.Forms.View.Details
        '
        'chSupplierCode
        '
        Me.chSupplierCode.Text = "Supplier Code"
        '
        'chName
        '
        Me.chName.Text = "Suppiler Name"
        '
        'SysproSupplierPicker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lvSupplierList)
        Me.Controls.Add(Me.ToolStrip2)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "SysproSupplierPicker"
        Me.Text = "SysproSupplierPicker"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents tslSearch As ToolStripLabel
    Friend WithEvents tstbSearchTerms As ToolStripTextBox
    Friend WithEvents ToolStrip2 As ToolStrip
    Friend WithEvents lvSupplierList As ListView
    Friend WithEvents chSupplierCode As ColumnHeader
    Friend WithEvents chName As ColumnHeader
    Friend WithEvents tsbCancel As ToolStripButton
End Class
