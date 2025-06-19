<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FileExplorer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FileExplorer))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlFileExplorer = New System.Windows.Forms.Panel()
        Me.scFileExplorerFoldersFiles = New System.Windows.Forms.SplitContainer()
        Me.scSharingTree = New System.Windows.Forms.SplitContainer()
        Me.globalOpsTree = New System.Windows.Forms.TreeView()
        Me.cmsTreeView = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TreeViewOpenFolderInFileExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TreeViewCopyFolderPath = New System.Windows.Forms.ToolStripMenuItem()
        Me.TreeViewImportProjectFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TreeViewCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TreeViewRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.treeViewCMSDeleteFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.TreeViewCreateFolderInSelectedFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.TreeViewImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.sharePanel = New System.Windows.Forms.Panel()
        Me.gbSharingNote = New System.Windows.Forms.GroupBox()
        Me.rtbSharingNote = New System.Windows.Forms.RichTextBox()
        Me.scSharing = New System.Windows.Forms.SplitContainer()
        Me.dgvAvailableMembers = New System.Windows.Forms.DataGridView()
        Me.AvailableName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AvailableEmail = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblPotentialShareMembers = New System.Windows.Forms.Label()
        Me.dgvCurrentMembers = New System.Windows.Forms.DataGridView()
        Me.currentName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.currentEmail = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblCurrentShareMembers = New System.Windows.Forms.Label()
        Me.tsSharing = New System.Windows.Forms.ToolStrip()
        Me.btnShareSelected = New System.Windows.Forms.ToolStripButton()
        Me.btnShareAll = New System.Windows.Forms.ToolStripButton()
        Me.btnRemoveAll = New System.Windows.Forms.ToolStripButton()
        Me.btnRemoveSelected = New System.Windows.Forms.ToolStripButton()
        Me.globalOpsListView = New System.Windows.Forms.ListView()
        Me.ColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnsHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chIsInKnowledgeBase = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chExpirationDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmsListView = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ListViewOpenInFileExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListViewCopyFilePath = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ListViewSendTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListViewSendToEmail = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListViewSendToDesktop = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListViewSendToDocuments = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ListViewCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListViewPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.AllowUserToEditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListViewRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiAddSelectedToKB = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRemoveSelectedToKB = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ListViewCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiSetDocumentExpirationDate = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListViewImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.tsListView = New System.Windows.Forms.ToolStrip()
        Me.btnUploadToDropbox = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnCreateFolderInSelectedFolder = New System.Windows.Forms.ToolStripButton()
        Me.tsLblCurrentFolder = New System.Windows.Forms.ToolStripLabel()
        Me.lblError = New System.Windows.Forms.Label()
        Me.tsFileExplorer = New System.Windows.Forms.ToolStrip()
        Me.btnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnCreateDeskTopShortCut = New System.Windows.Forms.ToolStripButton()
        Me.btnCreateFolder = New System.Windows.Forms.ToolStripButton()
        Me.tsddHideOrShowArchivedFolders = New System.Windows.Forms.ToolStripDropDownButton()
        Me.HideArchivedFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowArchivedFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnDropboxFolder = New System.Windows.Forms.ToolStripButton()
        Me.bsFolderInvitees = New System.Windows.Forms.BindingSource(Me.components)
        Me.bsCurrentMembers = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlFileExplorer.SuspendLayout()
        CType(Me.scFileExplorerFoldersFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scFileExplorerFoldersFiles.Panel1.SuspendLayout()
        Me.scFileExplorerFoldersFiles.Panel2.SuspendLayout()
        Me.scFileExplorerFoldersFiles.SuspendLayout()
        CType(Me.scSharingTree, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scSharingTree.Panel1.SuspendLayout()
        Me.scSharingTree.Panel2.SuspendLayout()
        Me.scSharingTree.SuspendLayout()
        Me.cmsTreeView.SuspendLayout()
        Me.sharePanel.SuspendLayout()
        Me.gbSharingNote.SuspendLayout()
        CType(Me.scSharing, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scSharing.Panel1.SuspendLayout()
        Me.scSharing.Panel2.SuspendLayout()
        Me.scSharing.SuspendLayout()
        CType(Me.dgvAvailableMembers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvCurrentMembers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsSharing.SuspendLayout()
        Me.cmsListView.SuspendLayout()
        Me.tsListView.SuspendLayout()
        Me.tsFileExplorer.SuspendLayout()
        CType(Me.bsFolderInvitees, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsCurrentMembers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlFileExplorer
        '
        Me.pnlFileExplorer.Controls.Add(Me.scFileExplorerFoldersFiles)
        Me.pnlFileExplorer.Controls.Add(Me.tsFileExplorer)
        Me.pnlFileExplorer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFileExplorer.Location = New System.Drawing.Point(0, 0)
        Me.pnlFileExplorer.Name = "pnlFileExplorer"
        Me.pnlFileExplorer.Size = New System.Drawing.Size(1376, 985)
        Me.pnlFileExplorer.TabIndex = 2
        '
        'scFileExplorerFoldersFiles
        '
        Me.scFileExplorerFoldersFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scFileExplorerFoldersFiles.Location = New System.Drawing.Point(0, 34)
        Me.scFileExplorerFoldersFiles.Name = "scFileExplorerFoldersFiles"
        '
        'scFileExplorerFoldersFiles.Panel1
        '
        Me.scFileExplorerFoldersFiles.Panel1.Controls.Add(Me.scSharingTree)
        '
        'scFileExplorerFoldersFiles.Panel2
        '
        Me.scFileExplorerFoldersFiles.Panel2.Controls.Add(Me.globalOpsListView)
        Me.scFileExplorerFoldersFiles.Panel2.Controls.Add(Me.tsListView)
        Me.scFileExplorerFoldersFiles.Panel2.Controls.Add(Me.lblError)
        Me.scFileExplorerFoldersFiles.Size = New System.Drawing.Size(1376, 951)
        Me.scFileExplorerFoldersFiles.SplitterDistance = 520
        Me.scFileExplorerFoldersFiles.TabIndex = 3
        '
        'scSharingTree
        '
        Me.scSharingTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scSharingTree.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scSharingTree.Location = New System.Drawing.Point(0, 0)
        Me.scSharingTree.Name = "scSharingTree"
        Me.scSharingTree.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scSharingTree.Panel1
        '
        Me.scSharingTree.Panel1.Controls.Add(Me.globalOpsTree)
        '
        'scSharingTree.Panel2
        '
        Me.scSharingTree.Panel2.Controls.Add(Me.sharePanel)
        Me.scSharingTree.Size = New System.Drawing.Size(520, 951)
        Me.scSharingTree.SplitterDistance = 621
        Me.scSharingTree.TabIndex = 0
        '
        'globalOpsTree
        '
        Me.globalOpsTree.AllowDrop = True
        Me.globalOpsTree.ContextMenuStrip = Me.cmsTreeView
        Me.globalOpsTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.globalOpsTree.FullRowSelect = True
        Me.globalOpsTree.HideSelection = False
        Me.globalOpsTree.ImageIndex = 0
        Me.globalOpsTree.ImageList = Me.TreeViewImageList
        Me.globalOpsTree.LabelEdit = True
        Me.globalOpsTree.Location = New System.Drawing.Point(0, 0)
        Me.globalOpsTree.Name = "globalOpsTree"
        Me.globalOpsTree.SelectedImageIndex = 0
        Me.globalOpsTree.ShowNodeToolTips = True
        Me.globalOpsTree.Size = New System.Drawing.Size(520, 621)
        Me.globalOpsTree.TabIndex = 1
        '
        'cmsTreeView
        '
        Me.cmsTreeView.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cmsTreeView.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TreeViewOpenFolderInFileExplorerToolStripMenuItem, Me.TreeViewCopyFolderPath, Me.TreeViewImportProjectFiles, Me.ToolStripSeparator1, Me.TreeViewCopy, Me.ToolStripSeparator2, Me.TreeViewRename, Me.treeViewCMSDeleteFolder, Me.ToolStripSeparator3, Me.TreeViewCreateFolderInSelectedFolder, Me.ToolStripSeparator10})
        Me.cmsTreeView.Name = "ContextMenuStrip1"
        Me.cmsTreeView.Size = New System.Drawing.Size(335, 252)
        '
        'TreeViewOpenFolderInFileExplorerToolStripMenuItem
        '
        Me.TreeViewOpenFolderInFileExplorerToolStripMenuItem.Name = "TreeViewOpenFolderInFileExplorerToolStripMenuItem"
        Me.TreeViewOpenFolderInFileExplorerToolStripMenuItem.Size = New System.Drawing.Size(334, 32)
        Me.TreeViewOpenFolderInFileExplorerToolStripMenuItem.Text = "Open Folder in File Explorer"
        '
        'TreeViewCopyFolderPath
        '
        Me.TreeViewCopyFolderPath.Name = "TreeViewCopyFolderPath"
        Me.TreeViewCopyFolderPath.Size = New System.Drawing.Size(334, 32)
        Me.TreeViewCopyFolderPath.Text = "Copy Folder Path"
        '
        'TreeViewImportProjectFiles
        '
        Me.TreeViewImportProjectFiles.Name = "TreeViewImportProjectFiles"
        Me.TreeViewImportProjectFiles.Size = New System.Drawing.Size(334, 32)
        Me.TreeViewImportProjectFiles.Text = "Import Projects Files"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(331, 6)
        '
        'TreeViewCopy
        '
        Me.TreeViewCopy.Name = "TreeViewCopy"
        Me.TreeViewCopy.Size = New System.Drawing.Size(334, 32)
        Me.TreeViewCopy.Text = "Copy"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(331, 6)
        '
        'TreeViewRename
        '
        Me.TreeViewRename.Name = "TreeViewRename"
        Me.TreeViewRename.Size = New System.Drawing.Size(334, 32)
        Me.TreeViewRename.Text = "Rename"
        '
        'treeViewCMSDeleteFolder
        '
        Me.treeViewCMSDeleteFolder.Name = "treeViewCMSDeleteFolder"
        Me.treeViewCMSDeleteFolder.Size = New System.Drawing.Size(334, 32)
        Me.treeViewCMSDeleteFolder.Text = "Delete Folder"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(331, 6)
        '
        'TreeViewCreateFolderInSelectedFolder
        '
        Me.TreeViewCreateFolderInSelectedFolder.Name = "TreeViewCreateFolderInSelectedFolder"
        Me.TreeViewCreateFolderInSelectedFolder.Size = New System.Drawing.Size(334, 32)
        Me.TreeViewCreateFolderInSelectedFolder.Text = "Create Folder in Selected Folder"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(331, 6)
        '
        'TreeViewImageList
        '
        Me.TreeViewImageList.ImageStream = CType(resources.GetObject("TreeViewImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.TreeViewImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.TreeViewImageList.Images.SetKeyName(0, "003-folder.png")
        Me.TreeViewImageList.Images.SetKeyName(1, "001-folder-2.png")
        Me.TreeViewImageList.Images.SetKeyName(2, "Cloud_blue_16x.png")
        Me.TreeViewImageList.Images.SetKeyName(3, "fileimage.png")
        Me.TreeViewImageList.Images.SetKeyName(4, "Full Folder.jpg")
        Me.TreeViewImageList.Images.SetKeyName(5, "Folder_16x.png")
        Me.TreeViewImageList.Images.SetKeyName(6, "dropboxfoldericon.jpg")
        Me.TreeViewImageList.Images.SetKeyName(7, "uploaded.png")
        '
        'sharePanel
        '
        Me.sharePanel.Controls.Add(Me.gbSharingNote)
        Me.sharePanel.Controls.Add(Me.scSharing)
        Me.sharePanel.Controls.Add(Me.tsSharing)
        Me.sharePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.sharePanel.Location = New System.Drawing.Point(0, 0)
        Me.sharePanel.MinimumSize = New System.Drawing.Size(225, 162)
        Me.sharePanel.Name = "sharePanel"
        Me.sharePanel.Size = New System.Drawing.Size(520, 326)
        Me.sharePanel.TabIndex = 3
        '
        'gbSharingNote
        '
        Me.gbSharingNote.Controls.Add(Me.rtbSharingNote)
        Me.gbSharingNote.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gbSharingNote.Location = New System.Drawing.Point(0, 212)
        Me.gbSharingNote.Name = "gbSharingNote"
        Me.gbSharingNote.Size = New System.Drawing.Size(520, 114)
        Me.gbSharingNote.TabIndex = 8
        Me.gbSharingNote.TabStop = False
        Me.gbSharingNote.Text = "Note sent to Invitee"
        '
        'rtbSharingNote
        '
        Me.rtbSharingNote.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbSharingNote.Location = New System.Drawing.Point(3, 16)
        Me.rtbSharingNote.Name = "rtbSharingNote"
        Me.rtbSharingNote.Size = New System.Drawing.Size(514, 95)
        Me.rtbSharingNote.TabIndex = 0
        Me.rtbSharingNote.Text = ""
        '
        'scSharing
        '
        Me.scSharing.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scSharing.Location = New System.Drawing.Point(0, 34)
        Me.scSharing.Name = "scSharing"
        '
        'scSharing.Panel1
        '
        Me.scSharing.Panel1.Controls.Add(Me.dgvAvailableMembers)
        Me.scSharing.Panel1.Controls.Add(Me.lblPotentialShareMembers)
        '
        'scSharing.Panel2
        '
        Me.scSharing.Panel2.Controls.Add(Me.dgvCurrentMembers)
        Me.scSharing.Panel2.Controls.Add(Me.lblCurrentShareMembers)
        Me.scSharing.Size = New System.Drawing.Size(520, 292)
        Me.scSharing.SplitterDistance = 246
        Me.scSharing.TabIndex = 5
        '
        'dgvAvailableMembers
        '
        Me.dgvAvailableMembers.AllowUserToAddRows = False
        Me.dgvAvailableMembers.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvAvailableMembers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvAvailableMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAvailableMembers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.AvailableName, Me.AvailableEmail})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvAvailableMembers.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvAvailableMembers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAvailableMembers.Location = New System.Drawing.Point(0, 20)
        Me.dgvAvailableMembers.Name = "dgvAvailableMembers"
        Me.dgvAvailableMembers.RowHeadersWidth = 62
        Me.dgvAvailableMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAvailableMembers.Size = New System.Drawing.Size(246, 272)
        Me.dgvAvailableMembers.TabIndex = 2
        '
        'AvailableName
        '
        Me.AvailableName.HeaderText = "Name"
        Me.AvailableName.MinimumWidth = 8
        Me.AvailableName.Name = "AvailableName"
        Me.AvailableName.Width = 150
        '
        'AvailableEmail
        '
        Me.AvailableEmail.HeaderText = "Email"
        Me.AvailableEmail.MinimumWidth = 8
        Me.AvailableEmail.Name = "AvailableEmail"
        Me.AvailableEmail.Width = 150
        '
        'lblPotentialShareMembers
        '
        Me.lblPotentialShareMembers.AutoSize = True
        Me.lblPotentialShareMembers.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblPotentialShareMembers.Location = New System.Drawing.Point(0, 0)
        Me.lblPotentialShareMembers.Name = "lblPotentialShareMembers"
        Me.lblPotentialShareMembers.Padding = New System.Windows.Forms.Padding(0, 2, 0, 5)
        Me.lblPotentialShareMembers.Size = New System.Drawing.Size(114, 20)
        Me.lblPotentialShareMembers.TabIndex = 7
        Me.lblPotentialShareMembers.Text = "Share this folder with..."
        '
        'dgvCurrentMembers
        '
        Me.dgvCurrentMembers.AllowUserToAddRows = False
        Me.dgvCurrentMembers.AllowUserToDeleteRows = False
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvCurrentMembers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvCurrentMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCurrentMembers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.currentName, Me.currentEmail})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvCurrentMembers.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgvCurrentMembers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCurrentMembers.Location = New System.Drawing.Point(0, 20)
        Me.dgvCurrentMembers.Name = "dgvCurrentMembers"
        Me.dgvCurrentMembers.RowHeadersWidth = 62
        Me.dgvCurrentMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCurrentMembers.Size = New System.Drawing.Size(270, 272)
        Me.dgvCurrentMembers.TabIndex = 4
        '
        'currentName
        '
        Me.currentName.HeaderText = "Name"
        Me.currentName.MinimumWidth = 8
        Me.currentName.Name = "currentName"
        Me.currentName.Width = 150
        '
        'currentEmail
        '
        Me.currentEmail.HeaderText = "Email"
        Me.currentEmail.MinimumWidth = 8
        Me.currentEmail.Name = "currentEmail"
        Me.currentEmail.Width = 150
        '
        'lblCurrentShareMembers
        '
        Me.lblCurrentShareMembers.AutoSize = True
        Me.lblCurrentShareMembers.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblCurrentShareMembers.Location = New System.Drawing.Point(0, 0)
        Me.lblCurrentShareMembers.Name = "lblCurrentShareMembers"
        Me.lblCurrentShareMembers.Padding = New System.Windows.Forms.Padding(0, 2, 0, 5)
        Me.lblCurrentShareMembers.Size = New System.Drawing.Size(175, 20)
        Me.lblCurrentShareMembers.TabIndex = 4
        Me.lblCurrentShareMembers.Text = "This folder is currently shared with..."
        '
        'tsSharing
        '
        Me.tsSharing.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsSharing.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsSharing.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnShareSelected, Me.btnShareAll, Me.btnRemoveAll, Me.btnRemoveSelected})
        Me.tsSharing.Location = New System.Drawing.Point(0, 0)
        Me.tsSharing.Name = "tsSharing"
        Me.tsSharing.Size = New System.Drawing.Size(520, 34)
        Me.tsSharing.TabIndex = 6
        Me.tsSharing.Text = "ToolStrip3"
        '
        'btnShareSelected
        '
        Me.btnShareSelected.Image = CType(resources.GetObject("btnShareSelected.Image"), System.Drawing.Image)
        Me.btnShareSelected.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnShareSelected.Name = "btnShareSelected"
        Me.btnShareSelected.Size = New System.Drawing.Size(189, 29)
        Me.btnShareSelected.Text = "Share with Selected"
        '
        'btnShareAll
        '
        Me.btnShareAll.Image = CType(resources.GetObject("btnShareAll.Image"), System.Drawing.Image)
        Me.btnShareAll.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnShareAll.Name = "btnShareAll"
        Me.btnShareAll.Size = New System.Drawing.Size(143, 29)
        Me.btnShareAll.Text = "Share with All"
        Me.btnShareAll.ToolTipText = " "
        '
        'btnRemoveAll
        '
        Me.btnRemoveAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnRemoveAll.Image = CType(resources.GetObject("btnRemoveAll.Image"), System.Drawing.Image)
        Me.btnRemoveAll.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(163, 29)
        Me.btnRemoveAll.Text = "Unshare with All"
        '
        'btnRemoveSelected
        '
        Me.btnRemoveSelected.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnRemoveSelected.Image = CType(resources.GetObject("btnRemoveSelected.Image"), System.Drawing.Image)
        Me.btnRemoveSelected.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRemoveSelected.Name = "btnRemoveSelected"
        Me.btnRemoveSelected.Size = New System.Drawing.Size(209, 29)
        Me.btnRemoveSelected.Text = "Unshare with Selected"
        Me.btnRemoveSelected.ToolTipText = " "
        '
        'globalOpsListView
        '
        Me.globalOpsListView.AllowDrop = True
        Me.globalOpsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader, Me.ColumnsHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader1, Me.ColumnHeader4, Me.chIsInKnowledgeBase, Me.chExpirationDate})
        Me.globalOpsListView.ContextMenuStrip = Me.cmsListView
        Me.globalOpsListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.globalOpsListView.FullRowSelect = True
        Me.globalOpsListView.HideSelection = False
        Me.globalOpsListView.LabelEdit = True
        Me.globalOpsListView.Location = New System.Drawing.Point(0, 34)
        Me.globalOpsListView.Name = "globalOpsListView"
        Me.globalOpsListView.Size = New System.Drawing.Size(852, 917)
        Me.globalOpsListView.SmallImageList = Me.ListViewImageList
        Me.globalOpsListView.TabIndex = 2
        Me.globalOpsListView.UseCompatibleStateImageBehavior = False
        Me.globalOpsListView.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader
        '
        Me.ColumnHeader.Text = "Name"
        Me.ColumnHeader.Width = 300
        '
        'ColumnsHeader1
        '
        Me.ColumnsHeader1.Text = "Type"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Last Modified"
        Me.ColumnHeader2.Width = 100
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Size"
        Me.ColumnHeader3.Width = 86
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Creator"
        Me.ColumnHeader1.Width = 75
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Shared to Dropbox"
        Me.ColumnHeader4.Width = 125
        '
        'chIsInKnowledgeBase
        '
        Me.chIsInKnowledgeBase.Text = "Is In Knowledge Base"
        '
        'chExpirationDate
        '
        Me.chExpirationDate.Text = "Expiration Date"
        '
        'cmsListView
        '
        Me.cmsListView.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cmsListView.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ListViewOpenInFileExplorerToolStripMenuItem, Me.PrintToolStripMenuItem, Me.ListViewCopyFilePath, Me.ToolStripSeparator4, Me.ListViewSendTo, Me.ToolStripSeparator5, Me.ListViewCopy, Me.ListViewPaste, Me.ToolStripSeparator6, Me.AllowUserToEditToolStripMenuItem, Me.ListViewRename, Me.ToolStripSeparator12, Me.tsmiAddSelectedToKB, Me.tsmiRemoveSelectedToKB, Me.ToolStripSeparator7, Me.ListViewCut, Me.ToolStripSeparator13, Me.tsmiSetDocumentExpirationDate})
        Me.cmsListView.Name = "ContextMenuStrip2"
        Me.cmsListView.Size = New System.Drawing.Size(330, 424)
        '
        'ListViewOpenInFileExplorerToolStripMenuItem
        '
        Me.ListViewOpenInFileExplorerToolStripMenuItem.Name = "ListViewOpenInFileExplorerToolStripMenuItem"
        Me.ListViewOpenInFileExplorerToolStripMenuItem.Size = New System.Drawing.Size(329, 32)
        Me.ListViewOpenInFileExplorerToolStripMenuItem.Text = "Open in File Explorer"
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(329, 32)
        Me.PrintToolStripMenuItem.Text = "Print"
        '
        'ListViewCopyFilePath
        '
        Me.ListViewCopyFilePath.Name = "ListViewCopyFilePath"
        Me.ListViewCopyFilePath.Size = New System.Drawing.Size(329, 32)
        Me.ListViewCopyFilePath.Text = "Copy File Path"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(326, 6)
        '
        'ListViewSendTo
        '
        Me.ListViewSendTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ListViewSendToEmail, Me.ListViewSendToDesktop, Me.ListViewSendToDocuments})
        Me.ListViewSendTo.Name = "ListViewSendTo"
        Me.ListViewSendTo.Size = New System.Drawing.Size(329, 32)
        Me.ListViewSendTo.Text = "Send to..."
        '
        'ListViewSendToEmail
        '
        Me.ListViewSendToEmail.Name = "ListViewSendToEmail"
        Me.ListViewSendToEmail.Size = New System.Drawing.Size(205, 34)
        Me.ListViewSendToEmail.Text = "Email"
        '
        'ListViewSendToDesktop
        '
        Me.ListViewSendToDesktop.Name = "ListViewSendToDesktop"
        Me.ListViewSendToDesktop.Size = New System.Drawing.Size(205, 34)
        Me.ListViewSendToDesktop.Text = "Desktop"
        '
        'ListViewSendToDocuments
        '
        Me.ListViewSendToDocuments.Name = "ListViewSendToDocuments"
        Me.ListViewSendToDocuments.Size = New System.Drawing.Size(205, 34)
        Me.ListViewSendToDocuments.Text = "Documents"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(326, 6)
        '
        'ListViewCopy
        '
        Me.ListViewCopy.Name = "ListViewCopy"
        Me.ListViewCopy.Size = New System.Drawing.Size(329, 32)
        Me.ListViewCopy.Text = "Copy"
        '
        'ListViewPaste
        '
        Me.ListViewPaste.Name = "ListViewPaste"
        Me.ListViewPaste.Size = New System.Drawing.Size(329, 32)
        Me.ListViewPaste.Text = "Paste"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(326, 6)
        '
        'AllowUserToEditToolStripMenuItem
        '
        Me.AllowUserToEditToolStripMenuItem.Name = "AllowUserToEditToolStripMenuItem"
        Me.AllowUserToEditToolStripMenuItem.Size = New System.Drawing.Size(329, 32)
        Me.AllowUserToEditToolStripMenuItem.Text = "Allow User To Edit"
        '
        'ListViewRename
        '
        Me.ListViewRename.Name = "ListViewRename"
        Me.ListViewRename.Size = New System.Drawing.Size(329, 32)
        Me.ListViewRename.Text = "Rename"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(326, 6)
        '
        'tsmiAddSelectedToKB
        '
        Me.tsmiAddSelectedToKB.Name = "tsmiAddSelectedToKB"
        Me.tsmiAddSelectedToKB.Size = New System.Drawing.Size(329, 32)
        Me.tsmiAddSelectedToKB.Text = "Add To Knowledge Base"
        '
        'tsmiRemoveSelectedToKB
        '
        Me.tsmiRemoveSelectedToKB.Name = "tsmiRemoveSelectedToKB"
        Me.tsmiRemoveSelectedToKB.Size = New System.Drawing.Size(329, 32)
        Me.tsmiRemoveSelectedToKB.Text = "Remove From Knowledge Base"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(326, 6)
        '
        'ListViewCut
        '
        Me.ListViewCut.Name = "ListViewCut"
        Me.ListViewCut.Size = New System.Drawing.Size(329, 32)
        Me.ListViewCut.Text = "Delete"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(326, 6)
        '
        'tsmiSetDocumentExpirationDate
        '
        Me.tsmiSetDocumentExpirationDate.Name = "tsmiSetDocumentExpirationDate"
        Me.tsmiSetDocumentExpirationDate.Size = New System.Drawing.Size(329, 32)
        Me.tsmiSetDocumentExpirationDate.Text = "Set Document Expiration Date"
        '
        'ListViewImageList
        '
        Me.ListViewImageList.ImageStream = CType(resources.GetObject("ListViewImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ListViewImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.ListViewImageList.Images.SetKeyName(0, "Word.PNG")
        Me.ListViewImageList.Images.SetKeyName(1, "pdf.png")
        Me.ListViewImageList.Images.SetKeyName(2, "excel.png")
        Me.ListViewImageList.Images.SetKeyName(3, "fileimage.png")
        Me.ListViewImageList.Images.SetKeyName(4, "word Uploaded.PNG")
        Me.ListViewImageList.Images.SetKeyName(5, "pdf Uploaded.PNG")
        Me.ListViewImageList.Images.SetKeyName(6, "excel Uploaded.PNG")
        Me.ListViewImageList.Images.SetKeyName(7, "uploaded.png")
        Me.ListViewImageList.Images.SetKeyName(8, "folder.png")
        '
        'tsListView
        '
        Me.tsListView.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsListView.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsListView.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnUploadToDropbox, Me.ToolStripSeparator8, Me.btnCreateFolderInSelectedFolder, Me.tsLblCurrentFolder})
        Me.tsListView.Location = New System.Drawing.Point(0, 0)
        Me.tsListView.Name = "tsListView"
        Me.tsListView.Size = New System.Drawing.Size(852, 34)
        Me.tsListView.TabIndex = 4
        Me.tsListView.Text = "ToolStrip2"
        '
        'btnUploadToDropbox
        '
        Me.btnUploadToDropbox.Image = CType(resources.GetObject("btnUploadToDropbox.Image"), System.Drawing.Image)
        Me.btnUploadToDropbox.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUploadToDropbox.Name = "btnUploadToDropbox"
        Me.btnUploadToDropbox.Size = New System.Drawing.Size(232, 29)
        Me.btnUploadToDropbox.Text = "Upload Files To Dropbox"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 34)
        '
        'btnCreateFolderInSelectedFolder
        '
        Me.btnCreateFolderInSelectedFolder.Image = CType(resources.GetObject("btnCreateFolderInSelectedFolder.Image"), System.Drawing.Image)
        Me.btnCreateFolderInSelectedFolder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCreateFolderInSelectedFolder.Name = "btnCreateFolderInSelectedFolder"
        Me.btnCreateFolderInSelectedFolder.Size = New System.Drawing.Size(283, 29)
        Me.btnCreateFolderInSelectedFolder.Text = "Create Folder in Selected folder"
        '
        'tsLblCurrentFolder
        '
        Me.tsLblCurrentFolder.Name = "tsLblCurrentFolder"
        Me.tsLblCurrentFolder.Size = New System.Drawing.Size(134, 29)
        Me.tsLblCurrentFolder.Text = "Current Folder: "
        '
        'lblError
        '
        Me.lblError.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblError.AutoSize = True
        Me.lblError.Location = New System.Drawing.Point(510, 202)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(61, 13)
        Me.lblError.TabIndex = 3
        Me.lblError.Text = "Error Label "
        Me.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tsFileExplorer
        '
        Me.tsFileExplorer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsFileExplorer.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.tsFileExplorer.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnRefresh, Me.ToolStripSeparator9, Me.btnCreateDeskTopShortCut, Me.btnCreateFolder, Me.tsddHideOrShowArchivedFolders, Me.ToolStripSeparator11, Me.btnDropboxFolder})
        Me.tsFileExplorer.Location = New System.Drawing.Point(0, 0)
        Me.tsFileExplorer.Name = "tsFileExplorer"
        Me.tsFileExplorer.Size = New System.Drawing.Size(1376, 34)
        Me.tsFileExplorer.TabIndex = 2
        Me.tsFileExplorer.Text = "ToolStrip1"
        '
        'btnRefresh
        '
        Me.btnRefresh.Image = CType(resources.GetObject("btnRefresh.Image"), System.Drawing.Image)
        Me.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(94, 29)
        Me.btnRefresh.Text = "Refresh"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 34)
        '
        'btnCreateDeskTopShortCut
        '
        Me.btnCreateDeskTopShortCut.Image = CType(resources.GetObject("btnCreateDeskTopShortCut.Image"), System.Drawing.Image)
        Me.btnCreateDeskTopShortCut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCreateDeskTopShortCut.Name = "btnCreateDeskTopShortCut"
        Me.btnCreateDeskTopShortCut.Size = New System.Drawing.Size(289, 29)
        Me.btnCreateDeskTopShortCut.Text = "Create Project Desktop Shortcut"
        '
        'btnCreateFolder
        '
        Me.btnCreateFolder.Image = CType(resources.GetObject("btnCreateFolder.Image"), System.Drawing.Image)
        Me.btnCreateFolder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCreateFolder.Name = "btnCreateFolder"
        Me.btnCreateFolder.Size = New System.Drawing.Size(141, 29)
        Me.btnCreateFolder.Text = "Create Folder"
        '
        'tsddHideOrShowArchivedFolders
        '
        Me.tsddHideOrShowArchivedFolders.BackColor = System.Drawing.SystemColors.Control
        Me.tsddHideOrShowArchivedFolders.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HideArchivedFilesToolStripMenuItem, Me.ShowArchivedFilesToolStripMenuItem})
        Me.tsddHideOrShowArchivedFolders.Image = CType(resources.GetObject("tsddHideOrShowArchivedFolders.Image"), System.Drawing.Image)
        Me.tsddHideOrShowArchivedFolders.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsddHideOrShowArchivedFolders.Name = "tsddHideOrShowArchivedFolders"
        Me.tsddHideOrShowArchivedFolders.Size = New System.Drawing.Size(267, 29)
        Me.tsddHideOrShowArchivedFolders.Text = "Show/Hide Archived Folder"
        '
        'HideArchivedFilesToolStripMenuItem
        '
        Me.HideArchivedFilesToolStripMenuItem.Checked = True
        Me.HideArchivedFilesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.HideArchivedFilesToolStripMenuItem.Name = "HideArchivedFilesToolStripMenuItem"
        Me.HideArchivedFilesToolStripMenuItem.Size = New System.Drawing.Size(271, 34)
        Me.HideArchivedFilesToolStripMenuItem.Text = "Hide Archived Files"
        '
        'ShowArchivedFilesToolStripMenuItem
        '
        Me.ShowArchivedFilesToolStripMenuItem.Name = "ShowArchivedFilesToolStripMenuItem"
        Me.ShowArchivedFilesToolStripMenuItem.Size = New System.Drawing.Size(271, 34)
        Me.ShowArchivedFilesToolStripMenuItem.Text = "Show Archived Files"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 34)
        '
        'btnDropboxFolder
        '
        Me.btnDropboxFolder.Image = CType(resources.GetObject("btnDropboxFolder.Image"), System.Drawing.Image)
        Me.btnDropboxFolder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDropboxFolder.Name = "btnDropboxFolder"
        Me.btnDropboxFolder.Size = New System.Drawing.Size(244, 29)
        Me.btnDropboxFolder.Text = "Upload folder to Dropbox"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 8
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 150
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Email"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 8
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 150
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 8
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 150
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "Email"
        Me.DataGridViewTextBoxColumn4.MinimumWidth = 8
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 150
        '
        'FileExplorer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlFileExplorer)
        Me.Name = "FileExplorer"
        Me.Size = New System.Drawing.Size(1376, 985)
        Me.pnlFileExplorer.ResumeLayout(False)
        Me.pnlFileExplorer.PerformLayout()
        Me.scFileExplorerFoldersFiles.Panel1.ResumeLayout(False)
        Me.scFileExplorerFoldersFiles.Panel2.ResumeLayout(False)
        Me.scFileExplorerFoldersFiles.Panel2.PerformLayout()
        CType(Me.scFileExplorerFoldersFiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scFileExplorerFoldersFiles.ResumeLayout(False)
        Me.scSharingTree.Panel1.ResumeLayout(False)
        Me.scSharingTree.Panel2.ResumeLayout(False)
        CType(Me.scSharingTree, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scSharingTree.ResumeLayout(False)
        Me.cmsTreeView.ResumeLayout(False)
        Me.sharePanel.ResumeLayout(False)
        Me.sharePanel.PerformLayout()
        Me.gbSharingNote.ResumeLayout(False)
        Me.scSharing.Panel1.ResumeLayout(False)
        Me.scSharing.Panel1.PerformLayout()
        Me.scSharing.Panel2.ResumeLayout(False)
        Me.scSharing.Panel2.PerformLayout()
        CType(Me.scSharing, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scSharing.ResumeLayout(False)
        CType(Me.dgvAvailableMembers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvCurrentMembers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsSharing.ResumeLayout(False)
        Me.tsSharing.PerformLayout()
        Me.cmsListView.ResumeLayout(False)
        Me.tsListView.ResumeLayout(False)
        Me.tsListView.PerformLayout()
        Me.tsFileExplorer.ResumeLayout(False)
        Me.tsFileExplorer.PerformLayout()
        CType(Me.bsFolderInvitees, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsCurrentMembers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents scFileExplorerFoldersFiles As SplitContainer
    Friend WithEvents globalOpsTree As TreeView
    Friend WithEvents globalOpsListView As ListView
    Friend WithEvents ColumnHeader As ColumnHeader
    Friend WithEvents ColumnsHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents tsFileExplorer As ToolStrip
    Friend WithEvents btnCreateFolder As ToolStripButton
    Friend WithEvents TreeViewImageList As ImageList
    Friend WithEvents cmsListView As ContextMenuStrip
    Friend WithEvents ListViewCut As ToolStripMenuItem
    Friend WithEvents ListViewCopy As ToolStripMenuItem
    Friend WithEvents ListViewPaste As ToolStripMenuItem
    Friend WithEvents ListViewSendTo As ToolStripMenuItem
    Friend WithEvents ListViewSendToEmail As ToolStripMenuItem
    Friend WithEvents ListViewSendToDesktop As ToolStripMenuItem
    Friend WithEvents ListViewSendToDocuments As ToolStripMenuItem
    Friend WithEvents ListViewRename As ToolStripMenuItem
    Friend WithEvents cmsTreeView As ContextMenuStrip
    Friend WithEvents TreeViewRename As ToolStripMenuItem
    Friend WithEvents TreeViewCopy As ToolStripMenuItem
    Friend WithEvents lblError As Label
    Friend WithEvents btnRefresh As ToolStripButton
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ListViewCopyFilePath As ToolStripMenuItem
    Friend WithEvents TreeViewCopyFolderPath As ToolStripMenuItem
    Friend WithEvents TreeViewCreateFolderInSelectedFolder As ToolStripMenuItem
    Friend WithEvents btnDropboxFolder As ToolStripButton
    Friend WithEvents tsListView As ToolStrip
    Friend WithEvents btnUploadToDropbox As ToolStripButton
    Friend WithEvents dgvAvailableMembers As DataGridView
    Friend WithEvents AvailableName As DataGridViewTextBoxColumn
    Friend WithEvents AvailableEmail As DataGridViewTextBoxColumn
    Friend WithEvents scSharing As SplitContainer
    Friend WithEvents dgvCurrentMembers As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents sharePanel As Panel
    Friend WithEvents tsSharing As ToolStrip
    Friend WithEvents btnShareSelected As ToolStripButton
    Friend WithEvents btnShareAll As ToolStripButton
    Friend WithEvents btnRemoveSelected As ToolStripButton
    Friend WithEvents btnRemoveAll As ToolStripButton
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents lblCurrentShareMembers As Label
    Friend WithEvents lblPotentialShareMembers As Label
    Friend WithEvents ListViewImageList As ImageList
    Friend WithEvents pnlFileExplorer As Panel
    Friend WithEvents scSharingTree As SplitContainer
    Friend WithEvents treeViewCMSDeleteFolder As ToolStripMenuItem
    Friend WithEvents ListViewOpenInFileExplorerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TreeViewOpenFolderInFileExplorerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnCreateDeskTopShortCut As ToolStripButton
    Friend WithEvents btnCreateFolderInSelectedFolder As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents tsLblCurrentFolder As ToolStripLabel
    Friend WithEvents gbSharingNote As GroupBox
    Friend WithEvents rtbSharingNote As RichTextBox
    Friend WithEvents bsFolderInvitees As BindingSource
    Friend WithEvents bsCurrentMembers As BindingSource
    Friend WithEvents currentName As DataGridViewTextBoxColumn
    Friend WithEvents currentEmail As DataGridViewTextBoxColumn
    Friend WithEvents PrintToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents tsddHideOrShowArchivedFolders As ToolStripDropDownButton
    Friend WithEvents HideArchivedFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShowArchivedFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TreeViewImportProjectFiles As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
    Friend WithEvents AllowUserToEditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chIsInKnowledgeBase As ColumnHeader
    Friend WithEvents ToolStripSeparator12 As ToolStripSeparator
    Friend WithEvents tsmiAddSelectedToKB As ToolStripMenuItem
    Friend WithEvents tsmiRemoveSelectedToKB As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
    Friend WithEvents tsmiSetDocumentExpirationDate As ToolStripMenuItem
    Friend WithEvents chExpirationDate As ColumnHeader
End Class
