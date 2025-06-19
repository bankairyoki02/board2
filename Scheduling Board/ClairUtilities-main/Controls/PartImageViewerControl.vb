Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading.Tasks

Public Class PartImageViewerControl

#Region "Properties"

    Private _PartPath As String
    Private _PartGUID As String
    Private _PhotoPath As String = "Photos"
    Private _selected_image_index As Int16 = 0
    Dim _imageExtensions As String() = {".jpg", ".jpeg", ".png", ".mov", ".gif"}

    Private _listPartImages As List(Of String)
    Public Property ListPartImages() As List(Of String)
        Get
            Return _listPartImages
        End Get
        Private Set(ByVal value As List(Of String))
            _listPartImages = value
        End Set
    End Property

    Public ReadOnly Property PathExist() As Boolean
        Get
            If (_PartPath Is Nothing) Then
                Throw New ArgumentNullException("There is no value for the variable _PartPath which should be fill from querying fileStoragePaths.")
            End If
            Return Directory.Exists(_PartPath)
        End Get
    End Property

    Public ReadOnly Property PhotoPath() As String
        Get
            If (_PartPath Is Nothing) Then
                Throw New ArgumentNullException("There is no value for the variable _PartPath which should be fill from querying fileStoragePaths.")
            End If
            Return Path.Combine(_PartPath, _PhotoPath)
        End Get
    End Property


#End Region

    Public Async Function GetPartImagePathAsync(partNumber As String) As Threading.Tasks.Task(Of PartImage)
        Using conn As New SqlConnection(FinesseConnectionString)
            Dim partImage = Await conn.ExecuteSTPDAndGetACollectionAsync(Of PartImage)("get_part_file_storage_main_details", {"@partno", partNumber})
            Return partImage.FirstOrDefault()
        End Using
    End Function


    Public Async Function InitializeDataAsync(partNumber As String) As Task
        _selected_image_index = 0
        'PbPartImages.Image = PbPartImages.InitialImage
        ShowLoadingImage(PbPartImages)

        Dim result = Await GetPartImagePathAsync(partNumber)

        If result Is Nothing Then Exit Function

        _PartGUID = result.PartGUID.ToString()
        _PartPath = result.PartImageFilePath

        If PathExist Then
            Dim imagesDirectory = New DirectoryInfo(_PartPath)
            Dim existingImages = Await Task.Run(Function() imagesDirectory.GetFilesByExtensions(_imageExtensions))
            Dim imagePaths As List(Of String) = existingImages.Select(Function(file) file.FullName).ToList()
            ListPartImages = imagePaths
        Else
            ListPartImages = New List(Of String)()
        End If

        Await Task.Run(Sub() setPhotoPath())
    End Function

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SetupTransparentPanel()
        ClickablePanel.Parent = PbPartImages

        ClickablePanel.BackColor = Color.Transparent
        PbPartImages.BackgroundImageLayout = ImageLayout.Zoom
        ClickablePanel.Dock = DockStyle.Fill
        ClickablePanel.BringToFront()
    End Sub

    Private Sub PartImageViewerControl_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        SetupTransparentPanel()
    End Sub

    Private Sub ShowLoadingImage(pictureBox As PictureBox)
        Dim bmp As New Bitmap(pictureBox.Width, pictureBox.Height)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Color.White)

            Dim font As New Font("Arial", 20, FontStyle.Bold)
            Dim brush As New SolidBrush(Color.Black)

            Dim text As String = "Loading..."
            Dim textSize As SizeF = g.MeasureString(text, font)
            Dim x As Single = (bmp.Width - textSize.Width) / 2
            Dim y As Single = (bmp.Height - textSize.Height) / 2
            g.DrawString(text, font, brush, x, y)
        End Using

        pictureBox.Image = bmp
        pictureBox.Visible = True
    End Sub

    Public Async Sub PartNumberChangeAsync(partNumber As String)
        Try
            ShowLoadingImage(PbPartImages)

            Await InitializeDataAsync(partNumber)

            setPhotoPath()
        Catch ex As Exception
            PbPartImages.Image = PbPartImages.InitialImage
        End Try

    End Sub

    Private Sub BtnPartImageNext_Click(sender As Object, e As EventArgs) Handles BtnPartImageNext.Click
        _selected_image_index += 1
        Dim max = ListPartImages.Count() - 1

        BtnPartImageNext.Enabled = Not (_selected_image_index = max)
        BtnPartImagePrevious.Enabled = Not (_selected_image_index = 0)

        If _selected_image_index <= max Then
            setPhotoPath()
        End If
    End Sub

    Private Sub BtnPartImagePrevious_Click(sender As Object, e As EventArgs) Handles BtnPartImagePrevious.Click
        _selected_image_index -= 1
        Dim max = ListPartImages.Count() - 1
        Dim min = 0
        BtnPartImageNext.Enabled = Not (_selected_image_index = max)
        BtnPartImagePrevious.Enabled = Not (_selected_image_index = min)

        If _selected_image_index >= min Then
            setPhotoPath()
        End If
    End Sub

    Private Sub setPhotoPath()
        If (ListPartImages.Count = 0 OrElse Not PathExist) Then
            PbPartImages.Image = PbPartImages.InitialImage
            Exit Sub
        End If

        Dim max = ListPartImages.Count() - 1
        Dim min = 0

        Dim bitmap = ListPartImages(_selected_image_index)
        PbPartImages.ImageLocation = bitmap
        PbPartImages.Tag = bitmap
        Dim pathParts = bitmap.Split("\")

        ' Update the UI safely
        Dim GUIDIndex = bitmap.IndexOf(_PartGUID.ToUpper) + _PartGUID.Count
        Dim pathToShow = bitmap.Substring(GUIDIndex, bitmap.Count - GUIDIndex)

        If LblImageLocation.InvokeRequired Then
            LblImageLocation.Invoke(New Action(Sub()
                                                   LblImageLocation.Text = $"{pathParts(pathParts.Count - 2)} folder"
                                               End Sub))
        Else
            LblImageLocation.Text = $"{pathParts(pathParts.Count - 2)} folder"
        End If

        If (BtnPartImageNext.InvokeRequired) Then
            BtnPartImageNext.Invoke(New Action(Sub()
                                                   BtnPartImageNext.Enabled = Not (_selected_image_index = max)
                                               End Sub))
        Else
            BtnPartImageNext.Enabled = Not (_selected_image_index = max)
        End If

        If (BtnPartImagePrevious.InvokeRequired) Then
            BtnPartImagePrevious.Invoke(New Action(Sub()
                                                       BtnPartImagePrevious.Enabled = Not (_selected_image_index = max)
                                                   End Sub))
        Else
            BtnPartImagePrevious.Enabled = Not (_selected_image_index = 0)
        End If

    End Sub


    Private Sub Panel5_DoubleClick(sender As Object, e As EventArgs) Handles ClickablePanel.DoubleClick
        If String.IsNullOrEmpty(PbPartImages.Tag) Or ListPartImages.Count() = 0 Then Return

        Dim filePath As String = PbPartImages.Tag
        Process.Start(filePath)
    End Sub
End Class
