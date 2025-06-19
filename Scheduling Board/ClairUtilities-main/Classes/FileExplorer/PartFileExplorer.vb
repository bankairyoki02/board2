Imports System.Data.SqlClient
Imports System.IO


Public Class PartFileExplorer

    Private ReadOnly _ImagesPath As String = "Photo"
    Public ReadOnly _imageExtensions As String() = {".jpg", ".jpeg", ".png", ".mov", ".gif"}

    Public Sub New(partNumber As String)
        Dim partImage = getPartImagePath(partNumber)
        FileExplorerGlobalPath = partImage.defaultRootFolderPath
        PartGUID = partImage.PartGUID.ToString()
        PartFullPath = partImage.PartImageFilePath
        ListPartImages = getPartImages(PartFullPath)
    End Sub

#Region "Methods"
    Public Function getPartImagePath(partNumber As String) As PartImage
        Dim conn = New SqlConnection(FinesseConnectionString)
        Dim partImage = conn.ExecuteStoredProcedureAndGetObject(Of PartImage)("get_part_file_storage_main_details",
                                                                                     {"@partno", partNumber})

        Return partImage
    End Function

    Public Function getPartImages(partFullPath As String) As List(Of Bitmap)
        Dim imageList = New List(Of Bitmap)
        If (Directory.Exists(partFullPath)) Then
            Dim filedirectory As DirectoryInfo = New DirectoryInfo(partFullPath)
            filedirectory.GetFiles()
            Dim images = filedirectory.GetFilesByExtensionsInAllDirectories(_imageExtensions)

            For Each imageFile In images
                Try
                    Using imageStream = New FileStream(imageFile.FullName, FileMode.Open, FileAccess.Read)
                        Dim Bitmap = Image.FromStream(imageStream)
                        Bitmap.Tag = imageFile.FullName
                        imageList.Add(Bitmap)
                    End Using
                Catch ex As Exception
                    Dim errorImagePath = Path.Combine(System.Environment.GetEnvironmentVariable("ESSVBDir"), "images\Error-image.jpg")
                    Dim Bitmap = Image.FromFile(errorImagePath)
                    Bitmap.Tag = imageFile.FullName
                    imageList.Add(Bitmap)

                    Console.WriteLine($"Error loading image: {imageFile.FullName}. {vbNewLine} {ex.Message}")
                End Try
            Next
        End If
        Return imageList
    End Function

#End Region

#Region "Properties"
    Private _fileExplorerGlobalPath As String
    Public Property FileExplorerGlobalPath() As String
        Get
            Return _fileExplorerGlobalPath
        End Get
        Set(ByVal value As String)
            _fileExplorerGlobalPath = value
        End Set
    End Property


    Private _partFullPath As String
    Public Property PartFullPath() As String
        Get
            Return _partFullPath
        End Get
        Set(ByVal value As String)
            _partFullPath = value
        End Set
    End Property

    Private _partGUID As String
    Public Property PartGUID() As String
        Get
            Return _partGUID
        End Get
        Set(ByVal value As String)
            _partGUID = value
        End Set
    End Property

    Private _listPartImages As List(Of Bitmap)
    Public Property ListPartImages() As List(Of Bitmap)
        Get
            Return _listPartImages
        End Get
        Set(ByVal value As List(Of Bitmap))
            _listPartImages = value
        End Set
    End Property
#End Region

End Class

Public Class PartImage

    Public Sub New()

    End Sub

    Private _PartImageFilePath As String
    Public Property PartImageFilePath() As String
        Get
            Return _PartImageFilePath
        End Get
        Set(ByVal value As String)
            _PartImageFilePath = value
        End Set
    End Property

    Private _PartGUID As Guid
    Public Property PartGUID() As Guid
        Get
            Return _PartGUID
        End Get
        Set(ByVal value As Guid)
            _PartGUID = value
        End Set
    End Property

    Private _defaultRootFolderPath As String
    Public Property defaultRootFolderPath() As String
        Get
            Return _defaultRootFolderPath
        End Get
        Set(ByVal value As String)
            _defaultRootFolderPath = value
        End Set
    End Property
End Class
