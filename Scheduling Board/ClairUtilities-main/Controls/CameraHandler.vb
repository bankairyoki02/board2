Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading

Public Class CameraHandler

    'Private FilterInfoCollection videoDevices
    Private _pathToSave As String = ""
    Private listCamera As New ArrayList()
    'Private videoSource As New WebCamCapture()

    '<System.ComponentModel.Browsable(True)>
    'Public Property ResultFieldSecondary() As String
    '    Get
    '        Return _ResultFieldSecondary
    '    End Get
    '    Set(value As String)
    '        _ResultFieldSecondary = value
    '    End Set
    'End Property


    '<System.ComponentModel.Browsable(True)>
    'Public Property _StartDate As Nullable(Of DateTime)
    '<System.ComponentModel.Browsable(True)>
    'Public Property _EndDate As Nullable(Of DateTime)

    Public Sub New(ByRef SQLQueryPrimary As String, ByRef CaptionFieldPrimary As String, ByRef ResultFieldPrimary As String,
                   Optional ByRef SQLQuerySecondary As String = "", Optional ByRef CaptionFieldSecondary As String = "", Optional ByRef ResultFieldSecondary As String = "",
                   Optional ByRef StartDate As DateTime = Nothing, Optional ByRef EndDate As DateTime = Nothing)

        ' This call is required by the designer.
        InitializeComponent()

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub RefreshData()


    End Sub

    Private Sub getListCameraUSB()
        'videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)
        'If videoDevices.Count <> 0 Then
        '    ' add all devices to combo
        '    For Each device As FilterInfo In videoDevices
        '        ComboBox1.Items.Add(device.Name)
        '    Next
        'Else
        '    ComboBox1.Items.Add("No DirectShow devices found")
        'End If
        'ComboBox1.SelectedIndex = 0
    End Sub


End Class
