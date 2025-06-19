Imports System.Drawing.Imaging
Imports System.IO

Public Class CameraCapture



#Region "Properties"
    Dim _capture As CameraCaptureHandler
    Dim _pathToSaveImage As String
    Dim _labelToShow As String



    Private _createdImagePath As String
    Public Property CreatedImagePath() As String
        Get
            Return _createdImagePath
        End Get
        Set(ByVal value As String)
            _createdImagePath = value
        End Set
    End Property


    Private _createdImageName As String
    Public Property CreatedImageName() As String
        Get
            Return _createdImageName
        End Get
        Set(ByVal value As String)
            _createdImageName = value
        End Set
    End Property

    Private _folderName As String
    Public Property CreatedImageFolderName() As String
        Get
            Return _folderName
        End Get
        Set(ByVal value As String)
            _folderName = value
        End Set
    End Property



#End Region

#Region "Constructor"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="path">Selected index Path to save</param>
    ''' <param name="PathsToSave">List of path to fill the Combobox</param>
    Public Sub New(path As String, Optional PathsToSave As String() = Nothing)
        ' This call is required by the designer.
        InitializeComponent()
        _pathToSaveImage = path
        PathsToSaveDdl.ValueMember = "ValueText"
        PathsToSaveDdl.DisplayMember = "DisplayText"
        Clipboard.Clear()
        For Each element In PathsToSave
            Dim folderName = element.Split("\").Last()
            PathsToSaveDdl.Items.Add(New CameraCapturePaths(element, folderName))
        Next

    End Sub


#End Region

    Private Sub CameraCapture_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initCamera()
        PathsToSaveDdl.SelectedItem = PathsToSaveDdl.Items.OfType(Of CameraCapturePaths).FirstOrDefault(Function(item) item.ValueText = _pathToSaveImage)
    End Sub


    Private Async Sub initCamera()

        _capture = New CameraCaptureHandler(CameraPictureBox)
        CameraPictureBox.Image = _capture.BitMap
        Dim errorMessage = Await _capture.OpenPreviewWindow()

        txtError.Visible = Not String.IsNullOrEmpty(errorMessage)
        TakeButton.Enabled = String.IsNullOrEmpty(errorMessage)
        If Not String.IsNullOrEmpty(errorMessage) Then
            txtError.Text = errorMessage
        End If

        'Dim driverlist = _capture.LoadDeviceList()
        'DllCameraDrivers.DataSource = driverlist
        initGUI()


    End Sub


    Private Sub initGUI()
        TakeButton.Visible = True
        RetakeButton.Visible = False
        SaveButton.Visible = False
    End Sub

    Private Sub TakeButton_Click(sender As Object, e As EventArgs) Handles TakeButton.Click
        Try
            Dim errorMessage = _capture.TakePicture()
            If (errorMessage IsNot Nothing) Then
                Throw New System.Exception(errorMessage)
            End If
            If (_capture.BitMap IsNot Nothing) Then
                CameraPictureBox.Image = _capture.BitMap
                CameraPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                TakeButton.Visible = False
                RetakeButton.Visible = True
                SaveButton.Visible = True
            Else
                TakeButton_Click(sender, e)
            End If
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
            Close()
        End Try

    End Sub

    Private Sub RetakeButton_Click(sender As Object, e As EventArgs) Handles RetakeButton.Click
        initCamera()
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Dim imageName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg"
        Dim selectedPath = PathsToSaveDdl.SelectedItem.ValueText.ToString()
        Dim strFilename As String = System.IO.Path.Combine(selectedPath,
                                                           imageName)

        If (_capture.BitMap IsNot Nothing) Then
            Dim bmap = _capture.BitMap
            If Directory.Exists(strFilename) = False Then
                Directory.CreateDirectory(strFilename.Replace(imageName, ""))
            End If
            bmap.Save(strFilename, ImageFormat.Jpeg)
            CreatedImagePath = strFilename
            CreatedImageName = imageName
            CreatedImageFolderName = PathsToSaveDdl.SelectedItem.DisplayText.ToString()
            Me.DialogResult = DialogResult.OK
            _capture.ClosePreviewWindow()
            Close()
        End If

    End Sub
End Class

Public Class CameraCapturePaths
    Private _valueText As String
    Public Property ValueText() As String
        Get
            Return _valueText
        End Get
        Set(ByVal value As String)
            _valueText = value
        End Set
    End Property

    Private _displayText As String
    Public Property DisplayText() As String
        Get
            Return _displayText
        End Get
        Set(ByVal value As String)
            _displayText = value
        End Set
    End Property

    Sub New(key As String, value As String)
        ValueText = key
        DisplayText = value
    End Sub
End Class