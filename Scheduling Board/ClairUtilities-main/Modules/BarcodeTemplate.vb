Imports System.IO
Imports System.Text.RegularExpressions

Public Class BarcodeTemplate
    Private _Name As String
    Private _Path As String
    Private _ZPLOriginal As String
    Private _ZPLForPrint As String
    Private _xOffset As Int16
    Private _yOffset As Int16
    Private _currentTemplate As Boolean
    Private _config As Dictionary(Of String, String)

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property

    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(value As String)
            _Path = value
        End Set
    End Property

    Public Property XOffset() As Int16
        Get
            Return _xOffset
        End Get
        Set(value As Int16)
            _xOffset = value
        End Set
    End Property

    Public Property YOffset() As Int16
        Get
            Return _yOffset
        End Get
        Set(value As Int16)
            _yOffset = value
        End Set
    End Property

    Public Property CurrentTemplate() As Boolean
        Get
            Return _currentTemplate
        End Get
        Set(value As Boolean)
            _currentTemplate = value
        End Set
    End Property

    Public Property Config() As Dictionary(Of String, String)
        Get
            Return _config
        End Get
        Set(value As Dictionary(Of String, String))
            _config = value
        End Set
    End Property

    Public Function GetZPLData(ByVal Barcode As String, ByVal SerialNumber As String, Optional subnetMask As String = "", Optional ipaddress As String = "", Optional cidr As String = "", Optional macAddress As String = "") As String
        'if the template data is already populated then dont retrieve it again
        If _ZPLOriginal = "" Then
            Try
                'get template data from template file
                _ZPLOriginal = File.ReadAllText(_Path)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly)
                Return ""
            End Try
        End If

        'if its still not populated then somethings wrong
        If _ZPLOriginal = "" Then
            Return ""
        End If

        'maintain the original for repeat calls
        _ZPLForPrint = _ZPLOriginal

        'make adjustments based on barcode length
        If _config.ContainsKey("CenterBarcodeImg") AndAlso _config("CenterBarcodeImg") Then
            CenterBarcodeImg(Barcode)
        End If

        DoLabelOffsets()

        ReplaceKeyFields(Barcode, SerialNumber, subnetMask, ipaddress, cidr, macAddress)

        Return _ZPLForPrint
    End Function

    Private Sub ReplaceKeyFields(ByVal barcode As String, ByVal serialNumber As String, Optional subnetMask As String = "", Optional ipaddress As String = "", Optional cidr As String = "", Optional macAddress As String = "")
        Dim oldBarcode = "&Unique_No&"
        Dim oldSerial = "&Serial_No&"
        'TODO: Temporary solution, this will be replace by database structure instead of hardcoded
        Dim oldSubnetMask = "&SubnetMask&"
        Dim oldIpAddress = "&IPAddress&"
        Dim oldCIDR = "&CIDR&"
        Dim oldMacAddress = "&MacAddress&"

        _ZPLForPrint = _ZPLForPrint.Replace(oldBarcode, barcode)
        _ZPLForPrint = _ZPLForPrint.Replace(oldSerial, serialNumber)
        _ZPLForPrint = _ZPLForPrint.Replace(oldSubnetMask, subnetMask)
        _ZPLForPrint = _ZPLForPrint.Replace(oldIpAddress, ipaddress)
        _ZPLForPrint = _ZPLForPrint.Replace(oldCIDR, cidr)
        _ZPLForPrint = _ZPLForPrint.Replace(oldMacAddress, macAddress)

        Debug.Print("Replaced Key Fields:")
        Debug.Print(_ZPLForPrint)

    End Sub

    Private Sub CenterBarcodeImg(ByRef barcode As String)
        Dim oldValue = "^FO60,30"
        Dim newValue = "^FO55,30"

        Select Case barcode.Length
            Case 1
                newValue = "^FO90,30"
            Case 2
                newValue = "^FO80,30"
            Case 3
                newValue = "^FO70,30"
            Case 4
                newValue = "^FO60,30"
            Case 5
                newValue = "^FO50,30"
            Case 6
                newValue = "^FO40,30"
            Case 7
                newValue = "^FO30,30"
            Case 8
                newValue = "^FO20,30"
            Case 8
                newValue = "^FO10,30"
            Case 9
                newValue = "^FO0,30"
        End Select

        _ZPLForPrint = _ZPLForPrint.Replace(oldValue, newValue)
        Debug.Print("Barcode Centered: " + oldValue + "->" + newValue)
        Debug.Print(_ZPLForPrint)
    End Sub

    Private Sub DoLabelOffsets()
        If _xOffset = 0 And _yOffset = 0 Then
            'do nothing
        Else
            Dim re As New Regex("\^(LH)+(\d{1,3}(,)\d{1,3})")
            Dim matches As MatchCollection

            matches = re.Matches(_ZPLForPrint)

            If matches.Count > 0 Then
                Dim digits = matches(0).ToString.Replace("^LH", "")
                Dim DefaultXY2 = digits.ToString.Split(",")
                Dim CalX = DefaultXY2(0) + _xOffset
                Dim CalY = DefaultXY2(1) - _yOffset

                Dim newValue = "^LH" + CStr(CalX) + "," + CStr(CalY)

                _ZPLForPrint = _ZPLForPrint.Replace(matches(0).ToString, newValue)
                Debug.Print("Label calibrated: " + matches(0).ToString + "->" + newValue)
                Debug.Print(_ZPLForPrint)
            Else
                Debug.Print("No label calibration")
            End If

        End If
    End Sub

End Class
