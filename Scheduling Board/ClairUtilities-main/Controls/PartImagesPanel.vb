Imports System.IO

Public Class PartImagesPanel

    Dim _fileExplorer As FileExplorer
    Dim _mainPath As String
    Dim _selectedImage As String
    Dim _selectedFolder As String
    Dim _SelectedImagePath As String
    Dim _imageExtensions As String() = {".jpg", ".jpeg", ".png", ".mov", ".gif"}
    Dim _photosMainFolder = "Photos"


    Private _bitMap As Image
    Public Property BitMap() As Image
        Get
            Return _bitMap
        End Get
        Set(ByVal value As Image)
            _bitMap = If(value IsNot Nothing, value.Clone(), Nothing)
        End Set
    End Property

    Public Sub New(fileExplorer As FileExplorer)

        ' This call is required by the designer.
        InitializeComponent()
        ImagesTreeView.Nodes.Clear()
        _fileExplorer = fileExplorer
        InitTreeView()
    End Sub


    Private Sub InitGUID()
        RemoveButton.Visible = False
        'TakePictureButton.Visible = False
        SelectedImagePictureBox.Image = Nothing
        LblPath.Text = ""
        If (BitMap IsNot Nothing) Then
            BitMap.Dispose()
        End If
    End Sub


    Private Sub HandleGUI()
        'TakePictureButton.Visible = If(String.IsNullOrEmpty(_selectedFolder) = False, True, False)
        RemoveButton.Visible = If(String.IsNullOrEmpty(_SelectedImagePath) = False, True, False)
    End Sub

    'Private Sub ImagesTreeView_DrawNode(sender As Object, e As DrawTreeNodeEventArgs) Handles ImagesTreeView.DrawNode
    '    If e.Node.Name = "unsupported-s.png" Or e.Node.Index = 8 Then
    '        Dim image As Image = ImageList1.Images(2)
    '        Dim imageBounds As New Rectangle(e.Bounds.Left, e.Bounds.Top, image.Width, image.Height)

    '        e.Graphics.DrawImage(image, imageBounds.Location)

    '        e.Node.ToolTipText = "Default Picture"

    '        If (Not e.Node.IsVisible) Then
    '            e.Node.EnsureVisible()
    '        End If
    '        e.DrawDefault = True
    '    Else
    '        e.DrawDefault = True
    '    End If
    'End Sub

    Public Sub InitTreeView(Optional nodeText As String = Nothing, Optional childNodeName As String = Nothing)
        Try
            If (_fileExplorer IsNot Nothing) Then
                ImagesTreeView.Nodes.Clear()
                _mainPath = _fileExplorer.attachmentCategorySubFolderPath & $"\{_photosMainFolder}"
                Dim nodes As TreeNodeCollection = _fileExplorer.globalOpsTree.Nodes.Find(_photosMainFolder, True).FirstOrDefault().Nodes

                For Each node As TreeNode In nodes
                    Dim hasChilds = node.Nodes.Count() > 0
                    If (hasChilds) Then
                        'Avoid having subfolders in this section
                        node.Nodes.Clear()
                    End If
                    node.ImageKey = 0
                    node.SelectedImageIndex = 0
                    Dim copy As TreeNode = node.Clone()
                    ImagesTreeView.Nodes.Add(copy)
                    AddImagesToTreeView(copy)
                    If (nodeText = copy.Text) Then
                        copy.Expand()
                        If (String.IsNullOrEmpty(childNodeName) = False) Then
                            Dim selectedNode = copy.Nodes.OfType(Of TreeNode).FirstOrDefault(Function(child) child.Text = childNodeName)
                            ImagesTreeView.SelectedNode = selectedNode
                        End If
                    End If
                Next
                'Add existing images inside Node
                If (Directory.Exists(_mainPath)) Then
                    Dim imagesDirectory = New DirectoryInfo(_mainPath)
                    Dim existingimages = imagesDirectory.GetFilesByExtensions(_imageExtensions)
                    For Each item In existingimages
                        Dim nodeToAdd = New TreeNode(item.Name) With {
                        .Name = item.FullName,
                        .Tag = item.FullName
                        }
                        Using file As Image = Image.FromFile(item.FullName)
                            ImageList1.Images.Add(item.FullName, file)
                        End Using
                        Dim imageIndex = ImageList1.Images.IndexOfKey(item.FullName)
                        nodeToAdd.ImageIndex = imageIndex
                        nodeToAdd.SelectedImageIndex = imageIndex
                        ImagesTreeView.Nodes.Add(nodeToAdd)
                    Next
                End If
            End If
            InitGUID()

        Catch ex As System.Exception
            ImagesTreeView.Nodes.Clear()
        End Try
    End Sub

    Private Sub AddImagesToTreeView(node As TreeNode)
        Dim Folder = node.FullPath
        Dim FullPath = _mainPath & "\" & Folder
        If (Directory.Exists(FullPath)) Then
            Dim filedirectory As DirectoryInfo = New DirectoryInfo(FullPath)

            Dim images = filedirectory.GetFilesByExtensions(_imageExtensions)
            If (images.Count() > 0) Then
                node.ImageIndex = 1
                node.SelectedImageIndex = 1
            End If
            For Each item In images
                Dim extension As String = item.Extension
                Dim name As String = item.Name
                Dim path As String = item.FullName
                Dim errorImagePath = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("ESSVBDir"), "images\Error-image.jpg")
                Dim file As Image
                Try
                    file = Image.FromFile(path)
                Catch ex As Exception
                    file = Image.FromFile(errorImagePath)
                    Console.WriteLine($"Error loading image: {path} {vbNewLine} {ex.Message}")
                End Try
                ImageList1.Images.Add(path, file)
                Dim imageIndex = ImageList1.Images.IndexOfKey(path)
                If node.Nodes.Find(name, True).FirstOrDefault() Is Nothing Then
                    node.Nodes.Add(name, name, imageIndex, imageIndex)
                End If
                file.Dispose()
            Next
        End If
    End Sub


    Private Sub ImagesTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles ImagesTreeView.AfterSelect
        ClickNode(e.Node)
    End Sub

    Private Sub ImagesTreeView_MouseDown(sender As Object, e As MouseEventArgs) Handles ImagesTreeView.MouseDown
        Dim hit = ImagesTreeView.HitTest(e.X, e.Y)
        If (hit.Node Is Nothing) Then
            ImagesTreeView.SelectedNode = Nothing
            If (e.Button = MouseButtons.Right) Then
                HandleRightCLick()
            End If
        End If

    End Sub


    Private Sub ImagesTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles ImagesTreeView.NodeMouseClick
        If (e.Button = MouseButtons.Right) Then
            Dim node = e.Node
            HandleRightCLick(node)

            ClickNode(node)
        End If
    End Sub

    Private Sub HandleRightCLick(Optional node As TreeNode = Nothing)
        If (node Is Nothing) Then
            OpenInEditorToolStripMenuItem.Visible = False
            RemoveToolStripMenuItem.Visible = False
            Exit Sub
        End If

        If (node.ImageIndex < 2) Then
            OpenInEditorToolStripMenuItem.Visible = False
            RemoveToolStripMenuItem.Visible = False
        Else
            OpenInEditorToolStripMenuItem.Visible = True
            RemoveToolStripMenuItem.Visible = True

        End If

        ClickNode(node)
    End Sub

    Private Sub ClickNode(node As TreeNode)
        Try
            If (node Is Nothing) Then
                Exit Sub
            End If
            ImagesTreeView.SelectedNode = node
            Dim hasParent = node.Parent
            Dim Folder = node.FullPath
            Dim FullPath = _mainPath & "\" & Folder
            Dim directory = New DirectoryInfo(FullPath)

            _SelectedImagePath = FullPath
            Dim label = If(_imageExtensions.Contains(directory.Extension.ToLower) = True, directory.Parent, Folder).ToString()
            LblPath.Text = label
            If (node.ImageIndex > 1 And _imageExtensions.Contains(directory.Extension.ToLower) = True) Then
                Using imageStream = New FileStream(FullPath, FileMode.Open, FileAccess.Read)
                    BitMap = Image.FromStream(imageStream)
                    SelectedImagePictureBox.Image = BitMap

                    If (BitMap.Width > SelectedImagePictureBox.Width Or BitMap.Height > SelectedImagePictureBox.Height) Then
                        SelectedImagePictureBox.SizeMode = PictureBoxSizeMode.Zoom
                    Else
                        SelectedImagePictureBox.SizeMode = PictureBoxSizeMode.CenterImage
                    End If
                End Using
            Else
                SelectedImagePictureBox.Image = Nothing
                _SelectedImagePath = Nothing

            End If
            _selectedImage = FullPath
            _selectedFolder = label
            HandleGUI()
        Catch ex As System.Exception

        End Try
    End Sub


    Private Sub TakePictureButton_Click(sender As Object, e As EventArgs) Handles TakePictureButton.Click, AddNewPictureToolStripMenuItem.Click
        Try
            If (_fileExplorer IsNot Nothing) Then
                Dim node = ImagesTreeView.SelectedNode
                Dim label = "Image will be saved in: " & _selectedFolder
                Dim Path = If(node Is Nothing Or node?.ImageIndex > 1, _mainPath, _mainPath & "\" & _selectedFolder)

                Dim nodeSubfolder As String() = ImagesTreeView.Nodes.OfType(Of TreeNode)() _
                .Where(Function(obj) obj.ImageIndex < 2) _
                .Select(Function(obj) _mainPath & "\" & obj.Text.ToString()) _
                .ToArray()
                Dim subFolders = {_mainPath}.Concat(nodeSubfolder).ToArray()

                Dim form = New CameraCapture(Path, subFolders)
                Dim result = form.ShowDialog()
                If (result = DialogResult.OK) Then
                    Dim initialNode = ImagesTreeView.SelectedNode
                    If (initialNode Is Nothing) Then
                        InitTreeView()
                        ImagesTreeView.SelectedNode = ImagesTreeView.Nodes(ImagesTreeView.Nodes.Count - 1)
                    Else
                        Dim selectedNode = If(initialNode.Parent IsNot Nothing, initialNode.Parent, initialNode)
                        InitTreeView(selectedNode.Text, form.CreatedImageName)
                    End If

                    Using imageStream = New FileStream(form.CreatedImagePath, FileMode.Open, FileAccess.Read)
                        BitMap = Image.FromStream(imageStream)
                        SelectedImagePictureBox.Image = BitMap
                    End Using
                End If
            End If
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)

        End Try

    End Sub



    Private Sub SelectedImagePictureBox_Click(sender As Object, e As EventArgs) Handles SelectedImagePictureBox.DoubleClick, OpenInEditorToolStripMenuItem.Click
        If (String.IsNullOrEmpty(_SelectedImagePath) = False) Then
            Process.Start(_SelectedImagePath)
        End If

    End Sub

    Private Sub RemoveButton_Click(sender As Object, e As EventArgs) Handles RemoveButton.Click, RemoveToolStripMenuItem.Click
        Try
            If (String.IsNullOrEmpty(_SelectedImagePath) = False) Then
                Dim confirmResult = MessageBox.Show("Are you sure you want to delete this image? This action cannot be undone.", "Confirm Delete!", MessageBoxButtons.YesNo)
                If (confirmResult = DialogResult.Yes) Then
                    Dim path = _SelectedImagePath

                    Dim nodeName = If(ImagesTreeView.SelectedNode.Parent IsNot Nothing, ImagesTreeView.SelectedNode.Parent.Text, ImagesTreeView.SelectedNode.Text)
                    ImagesTreeView.SelectedNode.Remove()
                    BitMap.Dispose()
                    SelectedImagePictureBox.Image = Nothing
                    Dim image = New FileInfo(path)
                    image.Delete()
                    InitTreeView(nodeName)
                End If
            End If
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
            InitTreeView()
        End Try

    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        InitTreeView()
    End Sub

    Private Sub ExpandButton_Click(sender As Object, e As EventArgs) Handles ExpandButton.Click
        If (ExpandButton.Text = "Expand all") Then
            ExpandButton.Text = "Collapse all"
            ImagesTreeView.ExpandAll()
        Else
            ExpandButton.Text = "Expand all"
            ImagesTreeView.CollapseAll()
        End If
        ImagesTreeView.Nodes(0).EnsureVisible()
    End Sub

    Dim NodeInMovement As TreeNode = Nothing
    Private Sub ImagesTreeView_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles ImagesTreeView.ItemDrag
        Dim dragNode = CType(e.Item, TreeNode)
        NodeInMovement = dragNode
        DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub TreeView1_DragLeave(sender As Object, e As EventArgs) Handles ImagesTreeView.DragLeave
        If NodeInMovement IsNot Nothing Then
            NodeInMovement = Nothing
        End If
    End Sub

    Private Sub ImagesTreeView_DragOver(sender As Object, e As DragEventArgs) Handles ImagesTreeView.DragOver
        Dim targetNode As TreeNode = ImagesTreeView.GetNodeAt(ImagesTreeView.PointToClient(New Point(e.X, e.Y)))
        Dim targetFolder = targetNode.Name
        PerformScroll(ImagesTreeView, e)
        Try
            If (NodeInMovement IsNot Nothing) Then
                Dim targetIsFolder = New FileInfo(targetFolder).Extension = "" 'Is a folder
                Dim sourceIsPicture = _imageExtensions.Contains(New FileInfo(NodeInMovement.Name).Extension)
                If Not targetNode Is Nothing AndAlso e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) And targetIsFolder And sourceIsPicture Then
                    e.Effect = DragDropEffects.Move
                Else
                    e.Effect = DragDropEffects.None
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ImagesTreeView_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ImagesTreeView.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub

#Region "Scroll while dragging"
    Private Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    Private Const WM_VSCROLL As Integer = &H115
    Private Const SB_LINEUP As Integer = 0
    Private Const SB_LINEDOWN As Integer = 1
    Private Const SB_THUMBPOSITION As Integer = 4

    'Make the scroll work while Dragging something
    Private Sub PerformScroll(treeView As TreeView, e As DragEventArgs)
        Dim mousePos As Point = treeView.PointToClient(New Point(e.X, e.Y))
        Dim clientRect As Rectangle = treeView.ClientRectangle
        Dim scrollArea As Integer = 20
        If mousePos.Y < clientRect.Top + scrollArea Then
            SendMessage(treeView.Handle, WM_VSCROLL, SB_LINEUP, 0)
        ElseIf mousePos.Y > clientRect.Bottom - scrollArea Then
            SendMessage(treeView.Handle, WM_VSCROLL, SB_LINEDOWN, 0)
        End If
    End Sub
#End Region

    Private Sub ImagesTreeView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ImagesTreeView.DragDrop
        Try
            e.Effect = DragDropEffects.None
            Dim targetNode As TreeNode = ImagesTreeView.GetNodeAt(ImagesTreeView.PointToClient(New Point(e.X, e.Y)))

            Dim mousePos As Point = ImagesTreeView.PointToClient(Cursor.Position)
            Dim nodeOver As TreeNode = ImagesTreeView.GetNodeAt(mousePos)
            If nodeOver?.Parent IsNot Nothing Then 'And nodeOver.Parent.Name.Split("")
                nodeOver = nodeOver.Parent
            End If

            Dim destinationFile = If(nodeOver Is Nothing Or nodeOver?.ImageIndex > 1, _mainPath, _mainPath & "\" & nodeOver.Text)

            If (NodeInMovement IsNot Nothing And targetNode IsNot Nothing) Then
                Dim info = New FileInfo($"{_mainPath}\{NodeInMovement.FullPath}")
                Dim destiny = _mainPath.Replace(_photosMainFolder, "") + targetNode.Name
                If (Not Directory.Exists(destiny)) Then
                    Directory.CreateDirectory(destiny)
                End If

                info.MoveTo($"{destiny}\{info.Name}")
            Else
                Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
                For Each item In files
                    Dim info = New FileInfo(item)
                    If _imageExtensions.Contains(info.Extension) Then
                        If (Not Directory.Exists(destinationFile)) Then
                            Directory.CreateDirectory(destinationFile)
                        End If
                        info.CopyTo($"{destinationFile}\{info.Name}")
                    Else
                        MessageBox.Show($"Cannot add the file: {info.Name} {System.Environment.NewLine} The file is not an image")
                    End If
                Next
            End If

            InitTreeView()
        Catch ex As System.Exception

        Finally
            NodeInMovement = Nothing
        End Try

    End Sub

    Private Sub OpenInFileExplorerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenInFileExplorerToolStripMenuItem.Click
        Try
            Dim FullPath = _mainPath
            If (_selectedFolder <> _photosMainFolder) Then
                FullPath = _mainPath & "\" & _selectedFolder
            End If
            If (Not Directory.Exists(FullPath)) Then
                Directory.CreateDirectory(FullPath)
            End If

            Process.Start(FullPath)
        Catch ex As System.Exception

        End Try
    End Sub

    Private Sub PartImagesPanel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LblPath.Text = _photosMainFolder
    End Sub
End Class

Module DirectoryInfoExtensions
    <System.Runtime.CompilerServices.Extension()>
    Public Function GetFilesByExtensions(ByVal dirInfo As DirectoryInfo, ByVal ParamArray extensions As String()) As IEnumerable(Of FileInfo)
        Dim allowedExtensions As New HashSet(Of String)(extensions, StringComparer.OrdinalIgnoreCase)

        Return dirInfo.EnumerateFiles().Where(Function(f) allowedExtensions.Contains(f.Extension))
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetFilesByExtensionsInAllDirectories(ByVal dirInfo As DirectoryInfo, ByVal ParamArray extensions As String()) As IEnumerable(Of FileInfo)
        Dim allowedExtensions As New HashSet(Of String)(extensions, StringComparer.OrdinalIgnoreCase)
        Dim allFiles As New List(Of FileInfo)()
        If dirInfo.Exists Then
            For Each file As FileInfo In dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories)
                Dim extension As String = file.Extension.ToLower()
                If allowedExtensions.Contains(extension) Then
                    allFiles.Add(New FileInfo(file.FullName))
                End If
            Next
        Else
            Console.WriteLine("Directory does not exist.")
        End If
        Return allFiles
    End Function
End Module
