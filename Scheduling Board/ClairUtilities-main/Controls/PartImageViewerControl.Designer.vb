<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PartImageViewerControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PartImageViewerControl))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.BtnPartImagePrevious = New System.Windows.Forms.Button()
        Me.BtnPartImageNext = New System.Windows.Forms.Button()
        Me.PbPartImages = New System.Windows.Forms.PictureBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.LblImageLocation = New System.Windows.Forms.Label()
        Me.ClickablePanel = New System.Windows.Forms.Panel()
        Me.Panel3.SuspendLayout()
        CType(Me.PbPartImages, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Showing:"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.BtnPartImagePrevious)
        Me.Panel3.Controls.Add(Me.BtnPartImageNext)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 560)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(895, 44)
        Me.Panel3.TabIndex = 4
        '
        'BtnPartImagePrevious
        '
        Me.BtnPartImagePrevious.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnPartImagePrevious.Enabled = False
        Me.BtnPartImagePrevious.Location = New System.Drawing.Point(24, 10)
        Me.BtnPartImagePrevious.Name = "BtnPartImagePrevious"
        Me.BtnPartImagePrevious.Size = New System.Drawing.Size(75, 23)
        Me.BtnPartImagePrevious.TabIndex = 1
        Me.BtnPartImagePrevious.Text = "Previous"
        Me.BtnPartImagePrevious.UseVisualStyleBackColor = True
        '
        'BtnPartImageNext
        '
        Me.BtnPartImageNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnPartImageNext.Enabled = False
        Me.BtnPartImageNext.Location = New System.Drawing.Point(784, 10)
        Me.BtnPartImageNext.Name = "BtnPartImageNext"
        Me.BtnPartImageNext.Size = New System.Drawing.Size(75, 23)
        Me.BtnPartImageNext.TabIndex = 0
        Me.BtnPartImageNext.Text = "Next"
        Me.BtnPartImageNext.UseVisualStyleBackColor = True
        '
        'PbPartImages
        '
        Me.PbPartImages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PbPartImages.InitialImage = CType(resources.GetObject("PbPartImages.InitialImage"), System.Drawing.Image)
        Me.PbPartImages.Location = New System.Drawing.Point(0, 53)
        Me.PbPartImages.Name = "PbPartImages"
        Me.PbPartImages.Size = New System.Drawing.Size(895, 551)
        Me.PbPartImages.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PbPartImages.TabIndex = 5
        Me.PbPartImages.TabStop = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.LblImageLocation)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(895, 53)
        Me.Panel4.TabIndex = 6
        '
        'LblImageLocation
        '
        Me.LblImageLocation.AutoSize = True
        Me.LblImageLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblImageLocation.Location = New System.Drawing.Point(62, 11)
        Me.LblImageLocation.Name = "LblImageLocation"
        Me.LblImageLocation.Size = New System.Drawing.Size(52, 13)
        Me.LblImageLocation.TabIndex = 3
        Me.LblImageLocation.Text = "location"
        '
        'ClickablePanel
        '
        Me.ClickablePanel.BackColor = System.Drawing.Color.Transparent
        Me.ClickablePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ClickablePanel.Location = New System.Drawing.Point(0, 53)
        Me.ClickablePanel.Name = "ClickablePanel"
        Me.ClickablePanel.Size = New System.Drawing.Size(895, 507)
        Me.ClickablePanel.TabIndex = 7
        '
        'PartImageViewerControl
        '
        Me.Controls.Add(Me.ClickablePanel)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.PbPartImages)
        Me.Controls.Add(Me.Panel4)
        Me.Name = "PartImageViewerControl"
        Me.Size = New System.Drawing.Size(895, 604)
        Me.Panel3.ResumeLayout(False)
        CType(Me.PbPartImages, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents BtnPartImagePrevious As Button
    Friend WithEvents BtnPartImageNext As Button
    Friend WithEvents PbPartImages As PictureBox
    Friend WithEvents Panel4 As Panel
    Friend WithEvents LblImageLocation As Label
    Friend WithEvents ClickablePanel As Panel
End Class
