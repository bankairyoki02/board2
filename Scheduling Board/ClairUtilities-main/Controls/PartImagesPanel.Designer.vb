<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PartImagesPanel
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PartImagesPanel))
        Me.RemoveButton = New System.Windows.Forms.Button()
        Me.ImagesTreeView = New System.Windows.Forms.TreeView()
        Me.ContextMenuStripTreeView = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddNewPictureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenInEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenInFileExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.LblPath = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TakePictureButton = New System.Windows.Forms.Button()
        Me.SelectedImagePictureBox = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ExpandButton = New System.Windows.Forms.Button()
        Me.ContextMenuStripTreeView.SuspendLayout()
        CType(Me.SelectedImagePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RemoveButton
        '
        Me.RemoveButton.Image = CType(resources.GetObject("RemoveButton.Image"), System.Drawing.Image)
        Me.RemoveButton.Location = New System.Drawing.Point(276, 4)
        Me.RemoveButton.Name = "RemoveButton"
        Me.RemoveButton.Size = New System.Drawing.Size(70, 61)
        Me.RemoveButton.TabIndex = 0
        Me.RemoveButton.Text = "Delete"
        Me.RemoveButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.RemoveButton.UseVisualStyleBackColor = True
        Me.RemoveButton.Visible = False
        '
        'ImagesTreeView
        '
        Me.ImagesTreeView.AllowDrop = True
        Me.ImagesTreeView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ImagesTreeView.ContextMenuStrip = Me.ContextMenuStripTreeView
        Me.ImagesTreeView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ImagesTreeView.ImageIndex = 0
        Me.ImagesTreeView.ImageList = Me.ImageList1
        Me.ImagesTreeView.ItemHeight = 55
        Me.ImagesTreeView.Location = New System.Drawing.Point(15, 91)
        Me.ImagesTreeView.Margin = New System.Windows.Forms.Padding(8)
        Me.ImagesTreeView.Name = "ImagesTreeView"
        Me.ImagesTreeView.SelectedImageIndex = 1
        Me.ImagesTreeView.ShowNodeToolTips = True
        Me.ImagesTreeView.Size = New System.Drawing.Size(494, 603)
        Me.ImagesTreeView.StateImageList = Me.ImageList1
        Me.ImagesTreeView.TabIndex = 1
        '
        'ContextMenuStripTreeView
        '
        Me.ContextMenuStripTreeView.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddNewPictureToolStripMenuItem, Me.OpenInEditorToolStripMenuItem, Me.RemoveToolStripMenuItem, Me.OpenInFileExplorerToolStripMenuItem})
        Me.ContextMenuStripTreeView.Name = "ContextMenuStripTreeView"
        Me.ContextMenuStripTreeView.Size = New System.Drawing.Size(184, 92)
        '
        'AddNewPictureToolStripMenuItem
        '
        Me.AddNewPictureToolStripMenuItem.Name = "AddNewPictureToolStripMenuItem"
        Me.AddNewPictureToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.AddNewPictureToolStripMenuItem.Text = "Add New Picture"
        '
        'OpenInEditorToolStripMenuItem
        '
        Me.OpenInEditorToolStripMenuItem.Name = "OpenInEditorToolStripMenuItem"
        Me.OpenInEditorToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OpenInEditorToolStripMenuItem.Text = "Open in editor"
        '
        'RemoveToolStripMenuItem
        '
        Me.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem"
        Me.RemoveToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.RemoveToolStripMenuItem.Text = "Delete"
        '
        'OpenInFileExplorerToolStripMenuItem
        '
        Me.OpenInFileExplorerToolStripMenuItem.Name = "OpenInFileExplorerToolStripMenuItem"
        Me.OpenInFileExplorerToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OpenInFileExplorerToolStripMenuItem.Text = "Open In File Explorer"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "ico16772.ico")
        Me.ImageList1.Images.SetKeyName(1, "3d_photos_folder_20523.ico")
        Me.ImageList1.Images.SetKeyName(2, "favorites-star-icon-png-0.png")
        '
        'LblPath
        '
        Me.LblPath.AutoSize = True
        Me.LblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPath.Location = New System.Drawing.Point(79, 10)
        Me.LblPath.Name = "LblPath"
        Me.LblPath.Size = New System.Drawing.Size(0, 13)
        Me.LblPath.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Current Folder:"
        '
        'TakePictureButton
        '
        Me.TakePictureButton.Image = CType(resources.GetObject("TakePictureButton.Image"), System.Drawing.Image)
        Me.TakePictureButton.Location = New System.Drawing.Point(190, 4)
        Me.TakePictureButton.Name = "TakePictureButton"
        Me.TakePictureButton.Size = New System.Drawing.Size(80, 61)
        Me.TakePictureButton.TabIndex = 5
        Me.TakePictureButton.UseVisualStyleBackColor = True
        '
        'SelectedImagePictureBox
        '
        Me.SelectedImagePictureBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SelectedImagePictureBox.ErrorImage = CType(resources.GetObject("SelectedImagePictureBox.ErrorImage"), System.Drawing.Image)
        Me.SelectedImagePictureBox.Location = New System.Drawing.Point(530, 15)
        Me.SelectedImagePictureBox.Name = "SelectedImagePictureBox"
        Me.SelectedImagePictureBox.Size = New System.Drawing.Size(744, 679)
        Me.SelectedImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.SelectedImagePictureBox.TabIndex = 4
        Me.SelectedImagePictureBox.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.LblPath)
        Me.Panel1.Location = New System.Drawing.Point(15, 66)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 26)
        Me.Panel1.TabIndex = 8
        '
        'Button2
        '
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(15, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(88, 62)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "Refresh"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ExpandButton
        '
        Me.ExpandButton.Location = New System.Drawing.Point(109, 4)
        Me.ExpandButton.Name = "ExpandButton"
        Me.ExpandButton.Size = New System.Drawing.Size(75, 61)
        Me.ExpandButton.TabIndex = 10
        Me.ExpandButton.Text = "Expand all"
        Me.ExpandButton.UseVisualStyleBackColor = True
        '
        'PartImagesPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ExpandButton)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TakePictureButton)
        Me.Controls.Add(Me.SelectedImagePictureBox)
        Me.Controls.Add(Me.ImagesTreeView)
        Me.Controls.Add(Me.RemoveButton)
        Me.Name = "PartImagesPanel"
        Me.Size = New System.Drawing.Size(1336, 708)
        Me.ContextMenuStripTreeView.ResumeLayout(False)
        CType(Me.SelectedImagePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RemoveButton As Button
    Friend WithEvents ImagesTreeView As TreeView
    Friend WithEvents SelectedImagePictureBox As PictureBox
    Friend WithEvents TakePictureButton As Button
    Friend WithEvents LblPath As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents ExpandButton As Button
    Friend WithEvents ContextMenuStripTreeView As ContextMenuStrip
    Friend WithEvents AddNewPictureToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RemoveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenInEditorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenInFileExplorerToolStripMenuItem As ToolStripMenuItem
End Class
