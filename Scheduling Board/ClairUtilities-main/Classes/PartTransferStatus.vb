Imports System.Environment

Public Enum CheckInOut
    CheckIn
    CheckOut
    ScanOut
    BatchTransferIn
End Enum

Public Class PartTransferStatus
    Private Sub New()
        Debug.Assert(False)
    End Sub

    Public Property SuppressSounds As Boolean = False

    Private _CheckInOutType As CheckInOut
    Private _SuccessfulCompleteTransfer As Boolean = False
    Private _SuccessfulPartialTransfer As Boolean = False

    Private _CheckedInToRepair As Boolean = False
    Private _IsFlagged As Boolean = False
    Private _HasRepairTicket As Boolean = False
    Private _IsBarcodeAProjectNumber As Boolean = False
    Private _IncreasedOrderedQuantity As Boolean = False
    Private _WarehouseTransferRequest As Boolean = False
    Private _NoActionRequestCheckIn As Boolean = False
    Private _NoActionRequestCheckOut As Boolean = False


    Private _IsCheckedOutToOtherProject As Boolean = False
    Private _IsInAnotherWarehouse As Boolean = False

    Private _ScanOutCausesBottleneck As Boolean = False

    Private _PartNotOrdered As Boolean = False

    Private _ProjectNumber As String = String.Empty
    Private _PartNumber As String = String.Empty
    Private _PartDescription As String = String.Empty

    Private _ErrorMessage As String = String.Empty

    Private _audioFilenameQueue As New Queue(Of String)

    Public Sub New(ByVal cmdExecutePartTransaction As SqlClient.SqlCommand, ByVal InOutType As CheckInOut)

        _CheckInOutType = InOutType

        Dim cmd = cmdExecutePartTransaction

        _ErrorMessage = CStr(cmdExecutePartTransaction.Parameters("@ErrorMessage").Value)

        cmd.AssignExistingParameterValue(_PartDescription, "@ReturnPartDesc")
        cmd.AssignExistingParameterValue(_IsFlagged, "@IsFlagged")
        cmd.AssignExistingParameterValue(_PartNumber, "@ReturnPartNo")
        cmd.AssignExistingParameterValue(_ProjectNumber, "@ReturnBatchNo")
        cmd.AssignExistingParameterValue(_HasRepairTicket, "@RepairTicketExists")
        cmd.AssignExistingParameterValue(_IncreasedOrderedQuantity, "@PartAddedorIncreasedPartQty")

        Dim to_batchno As String = Nothing
        If cmd.AssignExistingParameterValue(to_batchno, "@to_batchno") Then
            If to_batchno.ToUpper = "REPAIR" Then
                _CheckedInToRepair = True
            End If
        End If

        Dim ErrorType As String = ""
        If cmdExecutePartTransaction.Parameters.Contains("@ErrorType") Then
            '*****OLD WAY
            ErrorType = CStr(cmdExecutePartTransaction.Parameters("@ErrorType").Value)

            If String.IsNullOrEmpty(_ErrorMessage) AndAlso String.IsNullOrEmpty(ErrorType) Then
                cmd.AssignExistingParameterValue(ErrorType, "@UserActionRequired")
                cmd.AssignExistingParameterValue(_ErrorMessage, "@UserActionRequiredMessage")
            End If

            Select Case ErrorType
                Case "BarcodeIsProjectNumber"
                    _IsBarcodeAProjectNumber = True
                    cmd.AssignExistingParameterValue(_ProjectNumber, "@unique_no")
                    _SuccessfulCompleteTransfer = True

                Case "PartNotIncluded"
                    _PartNotOrdered = True

                Case "PartialCheckIn"
                    _SuccessfulPartialTransfer = True

                Case "ChkOutToOtherPrjWithTfr" ' from both UserActionRequired and ErrorType
                    _IsCheckedOutToOtherProject = True
                    _SuccessfulCompleteTransfer = True

                Case "ChkOutToOtherPrjNoTfr" ' from both UserActionRequired and ErrorType
                    _IsCheckedOutToOtherProject = True

                Case "PartWarehouseDoesNotMatch" ' from UserActionRequired
                    _IsInAnotherWarehouse = True

                Case "WarehouseTransferRequest"
                    'Transfer from INSTOCK of one warehouse to another warehouse.
                    _WarehouseTransferRequest = True

                Case "NoActionSuccessCheckIn"
                    _NoActionRequestCheckIn = True
                    _SuccessfulCompleteTransfer = True

                Case "NoActionSuccessCheckOut"
                    _NoActionRequestCheckOut = True
                    _SuccessfulCompleteTransfer = True

            End Select
        Else
            '*****NEW WAY
            _SuccessfulCompleteTransfer = False
            cmd.AssignExistingParameterValue(_IsBarcodeAProjectNumber, "@IsBarcodeAProjectNumber")
            If _IsBarcodeAProjectNumber Then
                cmd.AssignExistingParameterValue(_ProjectNumber, "@unique_no")
                _SuccessfulCompleteTransfer = True
            End If

            'ToDo: PartNotIncluded
            'ToDo: PartialCheckIn
            If cmdExecutePartTransaction.Parameters.Contains("@ChkOutToOtherPrjNoTfr") Then
                cmd.AssignExistingParameterValue(_IsCheckedOutToOtherProject, "@ChkOutToOtherPrjNoTfr")
            Else
                cmd.AssignExistingParameterValue(_IsCheckedOutToOtherProject, "@ChkOutToOtherPrjWithTfr")
                _SuccessfulCompleteTransfer = _IsCheckedOutToOtherProject
            End If

            cmd.AssignExistingParameterValue(_IsInAnotherWarehouse, "@PartWarehouseDoesNotMatch")
            cmd.AssignExistingParameterValue(_ScanOutCausesBottleneck, "@AvailBottleneckExists")

        End If

        If String.IsNullOrEmpty(_ErrorMessage) AndAlso String.IsNullOrEmpty(ErrorType) Then
            _SuccessfulCompleteTransfer = True
        End If
    End Sub

    Public Sub NotifyUser(transferTaskDescription As String)
        PlaySound()
        ShowNotificationMessage(transferTaskDescription)
    End Sub

    Public Sub ShowNotificationMessage(ByVal transferTaskDescription As String)
        If Not TransferCompletedSuccessfully Then
            MsgBox(ErrorMessage, MsgBoxStyle.Exclamation, "Error " & transferTaskDescription)
        End If
    End Sub

    Public Sub PlaySound()
        If SuppressSounds Then
            Return
        End If

        If NoActionRequestCheckIn Then
            PlayFinesseAudioFile("AlreadyInstock.wav")
            Return
        End If

        If NoActionRequestCheckOut Then
            PlayFinesseAudioFile("AlreadyCheckedOut.wav")
            Return
        End If

        If WarehouseTransferRequest Then
            PlayFinesseAudioFile("user_input_needed.wav")
            Return
        End If

        If IsBarcodeAProjectNumber Then
            PlayFinesseAudioFile("continuewarning.wav")
            Return
        End If

        If IsFlagged AndAlso HasRepairTicket Then
            PlayFinesseAudioFile("FlagAndRepair.wav")
        ElseIf IsFlagged Then
            PlayFinesseAudioFile("FlagPart.wav")
        ElseIf HasRepairTicket Then
            PlayFinesseAudioFile("RepairPart.wav")
        End If

        If IncreasedOrderedQuantity Then
            PlayFinesseAudioFile("good_beep_3x.wav")
        ElseIf CheckedInToRepair Then
            PlayFinesseAudioFile("Repair.wav")
        ElseIf TransferCompletedSuccessfully Then
            If CheckInOutType = CheckInOut.CheckIn Then
                If IsCheckedOutToOtherProject Then
                    PlayFinesseAudioFile("continuewarning.wav")
                Else
                    PlayFinesseAudioFile("checkin.wav")
                End If
            Else
                PlayFinesseAudioFile("checkout.wav")
            End If
        Else ' failed
            PlayFinesseAudioFile("bad_beep.wav")
        End If
    End Sub

    Private Sub PlayFinesseAudioFile(ByVal filename As String)
        SyncLock _audioFilenameQueue
            _audioFilenameQueue.Enqueue(filename)

            If _audioFilenameQueue.Count = 1 Then
                Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf ProcessAudioFileQueue))
                t.Start()
            End If
        End SyncLock
    End Sub

    Private Sub ProcessAudioFileQueue()
        Do
            Dim filenameToPlay As String

            SyncLock _audioFilenameQueue
                If _audioFilenameQueue.Count = 0 Then
                    Return
                End If

                filenameToPlay = _audioFilenameQueue.Dequeue()
            End SyncLock

            Dim pathToPlay = String.Format("{0}\sounds\{1}", GetEnvironmentVariable("ESSVBDIR"), filenameToPlay)
            Debug.Print("playing {0}", pathToPlay)

            Static wavBytesFromFileName As New Dictionary(Of String, Byte())(StringComparer.InvariantCultureIgnoreCase)
            Dim wavBytes() As Byte = Nothing

            If Not wavBytesFromFileName.TryGetValue(pathToPlay, wavBytes) Then
                wavBytes = System.IO.File.ReadAllBytes(pathToPlay)
                wavBytesFromFileName.Add(pathToPlay, wavBytes)
            End If

            My.Computer.Audio.Play(wavBytes, AudioPlayMode.WaitToComplete)
        Loop
    End Sub

    Public ReadOnly Property Color As System.Drawing.Color
        Get
            If TransferCompletedSuccessfully Then
                If CheckedInToRepair Then
                    Return Drawing.Color.Lavender
                Else
                    Return Drawing.Color.LightGreen
                End If

            Else
                Return Drawing.Color.LightPink
            End If
        End Get
    End Property

    Public ReadOnly Property CheckInOutType() As CheckInOut
        Get
            Return _CheckInOutType
        End Get
    End Property

    Public ReadOnly Property ScanOutCausesBottleneck As Boolean
        Get
            Return _ScanOutCausesBottleneck
        End Get
    End Property

    Public ReadOnly Property CheckedInToRepair() As Boolean
        Get
            Return _CheckedInToRepair
        End Get
    End Property

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _ErrorMessage
        End Get
    End Property
    Public ReadOnly Property HasRepairTicket() As Boolean
        Get
            Return _HasRepairTicket
        End Get
    End Property
    Public ReadOnly Property IncreasedOrderedQuantity() As Boolean
        Get
            Return _IncreasedOrderedQuantity
        End Get
    End Property
    Public ReadOnly Property IsBarcodeAProjectNumber() As Boolean
        Get
            Return _IsBarcodeAProjectNumber
        End Get
    End Property
    Public ReadOnly Property IsCheckedOutToOtherProject() As Boolean
        Get
            Return _IsCheckedOutToOtherProject
        End Get
    End Property
    Public ReadOnly Property IsFlagged() As Boolean
        Get
            Return _IsFlagged
        End Get
    End Property
    Public ReadOnly Property IsInAnotherWarehouse() As Boolean
        Get
            Return _IsInAnotherWarehouse
        End Get
    End Property
    Public ReadOnly Property PartDescription() As String
        Get
            Return _PartDescription
        End Get
    End Property
    Public ReadOnly Property PartNotOrdered() As Boolean
        Get
            Return _PartNotOrdered
        End Get
    End Property
    Public ReadOnly Property WarehouseTransferRequest() As Boolean
        Get
            Return _WarehouseTransferRequest
        End Get
    End Property
    Public ReadOnly Property NoActionRequestCheckIn() As Boolean
        Get
            Return _NoActionRequestCheckIn
        End Get
    End Property
    Public ReadOnly Property NoActionRequestCheckOut() As Boolean
        Get
            Return _NoActionRequestCheckOut
        End Get
    End Property
    Public ReadOnly Property PartNumber() As String
        Get
            Return _PartNumber
        End Get
    End Property
    Public ReadOnly Property ProjectNumber() As String
        Get
            Return _ProjectNumber
        End Get
    End Property
    Public ReadOnly Property TransferCompletedSuccessfully() As Boolean
        Get
            Return _SuccessfulCompleteTransfer
        End Get
    End Property

    Public ReadOnly Property TransferCompletedPartially() As Boolean
        Get
            Return _SuccessfulPartialTransfer
        End Get
    End Property

    Public ReadOnly Property TransferCompletedAtLeastPartially() As Boolean
        Get
            Return TransferCompletedSuccessfully OrElse TransferCompletedPartially
        End Get
    End Property

End Class
