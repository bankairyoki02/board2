
Imports System.Runtime.InteropServices
Imports System.Threading.Tasks

Public Class CameraCaptureHandler

#Region "Microsoft Camera related"

    Private Const WM_CAP As Short = &H400
    Private Const WM_CAP_DRIVER_CONNECT As Integer = WM_CAP + 10
    Private Const WM_CAP_DRIVER_DISCONNECT As Integer = WM_CAP + 11
    Private Const WM_CAP_EDIT_COPY As Integer = WM_CAP + 30
    Private Const WM_CAP_SET_PREVIEW As Integer = WM_CAP + 50
    Private Const WM_CAP_SET_PREVIEWRATE As Integer = WM_CAP + 52
    Private Const WM_CAP_SET_SCALE As Integer = WM_CAP + 53
    Private Const WS_CHILD As Integer = &H40000000
    Private Const WS_VISIBLE As Integer = &H10000000
    Private Const SWP_NOMOVE As Short = &H2
    Private Const SWP_NOZORDER As Short = &H4
    Private Const HWND_BOTTOM As Short = 1

    <DllImport("user32", SetLastError:=True)>
    Private Shared Function SendMessage(
        hwnd As Integer, wMsg As Integer, wParam As Integer, ByVal lParam As IntPtr) As Integer
        '<MarshalAs(UnmanagedType.AsAny)> ByRef lParam As Object
    End Function

    <DllImport("user32")>
    Private Shared Function SetWindowPos(
        hwnd As Integer, hWndInsertAfter As Integer, x As Integer, y As Integer, cx As Integer, cy As Integer, wFlags As Integer) As Integer
    End Function

    <DllImport("user32")>
    Private Shared Function DestroyWindow(hndw As Integer) As Boolean
    End Function

    <DllImport("avicap32.dll", SetLastError:=True)>
    Private Shared Function capCreateCaptureWindowA(
        lpszWindowName As String, dwStyle As Integer, x As Integer, y As Integer, nWidth As Integer, nHeight As Short, hWndParent As Integer, nID As Integer) As Integer
    End Function


    <DllImport("avicap32.dll", CharSet:=CharSet.Ansi)>
    Public Shared Function capGetDriverDescriptionA(
        ByVal wDriver As Short, ByVal lpszName As String, ByVal cbName As Integer, ByVal lpszVer As String, ByVal cbVer As Integer) As Boolean
    End Function

    <DllImport("gdi32.dll", SetLastError:=True)>
    Private Shared Function BitBlt(ByVal hdcDest As IntPtr, ByVal nXDest As Integer, ByVal nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hdcSrc As IntPtr, ByVal nXSrc As Integer, ByVal nYSrc As Integer, ByVal dwRop As Integer) As Boolean
    End Function


    Private _iDevice As Integer = 0
    Private _hHwnd As Integer
    Private _driverCameras As New List(Of String)()


#End Region


#Region "Constructor"

    ''' <summary>
    ''' initialize thCameraCaptureHandler to take pictures
    ''' </summary>
    ''' <param name="picCapture">PictureBox where the camera and the image will be shown</param>
    Public Sub New(picCapture As PictureBox)
        FramePictureBox = picCapture
        If (BitMap IsNot Nothing) Then
            BitMap.Dispose()
        End If

    End Sub
#End Region


#Region "Properties"
    Private _framePictureBox As PictureBox
    Public Property FramePictureBox As PictureBox
        Get
            Return _framePictureBox
        End Get
        Protected Set(value As PictureBox)
            _framePictureBox = value
        End Set
    End Property

    Private _image As Image
    Public Property BitMap As Image
        Get
            Return _image
        End Get
        Protected Set(value As Image)
            _image = value
        End Set
    End Property

    Private _legend As Image
    Public Property LegendText As Image
        Get
            Return _legend
        End Get
        Protected Set(value As Image)
            _legend = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' List the Camera driver available to use (This is not fully working, returns out of memory error when loading the function capGetDriverDescriptionA from avicap32)
    ''' </summary>
    ''' <returns></returns>
    Public Function LoadDeviceList() As List(Of String)
        Dim bReturn As Boolean
        Dim x As Short = 0
        Do
            bReturn = checkCameraStatus(x)
            If bReturn Then
                Dim DriverName = If(x = 0, "Main Camera", $"Camera {x + 1}")
                _driverCameras.Add(DriverName)
            End If
            x += 1
        Loop While bReturn
        Return _driverCameras
    End Function

    Private Function checkCameraStatus(cameraDriver As Int32) As Boolean
        Dim cursor = 100
        Dim driverName As String = Space(cursor)
        Dim driverVersion As String = Space(cursor)
        Return Not capGetDriverDescriptionA(cameraDriver, driverName, cursor, driverVersion, cursor)
    End Function

    ''' <summary>
    ''' Will take a initial image and compare with green screen image.
    ''' </summary>
    ''' <returns>True, if the returned image is the same base64 as greenImageScreen</returns>
    Private Async Function CheckCameraUsage() As Task(Of Boolean)
        Try
            Dim resultTask As Task = Task.Run(Function() SendMessage(_hHwnd, WM_CAP_EDIT_COPY, 0, 0))
            Await resultTask
            Dim data As IDataObject = Clipboard.GetDataObject()
            If (data IsNot Nothing AndAlso data.GetDataPresent(GetType(Bitmap))) Then
                Dim bmap As Bitmap = data.GetData(GetType(Bitmap))
                If bmap IsNot Nothing Then
                    Dim color As Color = bmap.GetPixel(bmap.Width - 1, bmap.Height - 1)
                    Clipboard.Clear()
                    If (color.Name = "ff008800") Then 'Green Screen (Present when the driver is already in use)
                        Return True
                    End If
                Else
                    Await CheckCameraUsage()
                End If
            Else
                Await CheckCameraUsage()
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function

    Private Function SendMessageAndGetResult(cameraDriver As Int32) As Int16
        Dim result As String = SendMessage(_hHwnd, WM_CAP_DRIVER_CONNECT, cameraDriver, 0)
        Return result
    End Function

    ''' <summary>
    ''' Opens the camera preview to take the picture
    ''' </summary>
    Public Async Function OpenPreviewWindow(Optional cameraDriver As Int32 = 0) As Task(Of String)
        Dim timer As New Stopwatch()
        Try

            _hHwnd = capCreateCaptureWindowA(cameraDriver.ToString(), WS_VISIBLE Or WS_CHILD, 0, 0, 640, 480, FramePictureBox.Handle.ToInt32(), 0)

            timer.Start()

            Dim resulttask As Task(Of Int16) = Task.Run(Function() SendMessageAndGetResult(cameraDriver)) 'This will prompt the user if multiple cameras connected (Should be improve),
            'so the time that the user wait will affect this. I know is gross

            Dim completedTask = Await Task.WhenAny(resulttask, Task.Delay(20000))
            timer.Stop()
            If completedTask IsNot resulttask Then
                Throw New TimeoutException("Camera initialization timed out. Please check your camera driver!")
            End If

            Dim resultDriver As Int16 = Await resulttask

            Dim isCameraInUse = Await CheckCameraUsage()
            If isCameraInUse Then
                Throw New Exception("Another Application is using your camera." & System.Environment.NewLine & " Stop the other app and try again.")
            End If

            If resultDriver <> 0 Then
                SendMessage(_hHwnd, WM_CAP_SET_SCALE, 1, 0)
                SendMessage(_hHwnd, WM_CAP_SET_PREVIEWRATE, 66, 0)
                SendMessage(_hHwnd, WM_CAP_SET_PREVIEW, 1, 0)
                Dim result = SetWindowPos(_hHwnd, HWND_BOTTOM, 0, 0, FramePictureBox.Width, FramePictureBox.Height, SWP_NOMOVE Or SWP_NOZORDER)
            Else
                Throw New Exception("Something went wrong detecting your camera..")
            End If

        Catch ex As Exception
            DestroyWindow(_hHwnd)
            Return ex.Message
        Finally
            Debug.Print("Getting camera preview time: ms{0}", timer.ElapsedMilliseconds)
            timer.Stop()
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Destroy the camera driver, and stop the camera
    ''' </summary>
    Public Sub ClosePreviewWindow()
        SendMessage(_hHwnd, WM_CAP_DRIVER_DISCONNECT, _iDevice, 0)
        DestroyWindow(_hHwnd)
    End Sub

    ''' <summary>
    ''' Take a picture using the selected camera
    ''' </summary>
    ''' <returns>Nothing if not issues, ErrorMessage in case any issue</returns>
    Public Function TakePicture() As String
        Try
            SendMessage(_hHwnd, WM_CAP_EDIT_COPY, 0, 0)
            Dim data As IDataObject = Clipboard.GetDataObject()
            If data IsNot Nothing AndAlso data.GetDataPresent(GetType(Bitmap)) Then
                Dim bmap As Image = DirectCast(data.GetData(GetType(Bitmap)), Image)
                BitMap = bmap

                ClosePreviewWindow()
            Else
                Return "Error taking the picture"
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
        Return Nothing
    End Function

End Class
