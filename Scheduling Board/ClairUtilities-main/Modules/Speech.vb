Imports System.Collections.ObjectModel
Imports System.Data.SqlClient
Imports System.Speech.Synthesis

Module Speech

    Private p_objSynth As New SpeechSynthesizer
    Private _SayWhat As String

    Private WithEvents SpeechDelay As New Timer With {.Interval = 1500}

    Public Sub Speak(ByVal SayWhat As String, ByVal msDelay As Integer)
        _SayWhat = SayWhat

        p_objSynth.SpeakAsyncCancelAll()

        Dim objVoices As ReadOnlyCollection(Of InstalledVoice) = p_objSynth.GetInstalledVoices
        Dim objVoice = objVoices(0)
        Dim objVoiceInformation As VoiceInfo = objVoice.VoiceInfo

        p_objSynth.Volume = 100
        p_objSynth.Rate = 2
        p_objSynth.SelectVoiceByHints(VoiceGender.Male, 30)

        SpeechDelay.Interval = msDelay

        SpeechDelay.Stop()
        SpeechDelay.Start()

    End Sub

    Private Sub SpeechDelay_Tick(sender As Object, e As EventArgs) Handles SpeechDelay.Tick
        p_objSynth.SpeakAsync(_SayWhat)
        SpeechDelay.Stop()
    End Sub

    Public Sub AnnounceRepairTicket(ByVal Barcode As String)
        If Not String.IsNullOrEmpty(Barcode) Then
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim SayWhat = newConn.ExecuteScalar("select top 1 ra.description from dbo.parts_with_open_repair_ticket pwort join dbo.RepairActivities ra on pwort.id_ticket = ra.id_ticket and pwort.id_activity = ra.id_activity where pwort.IsOpen = 1 and ra.activity_type = 'Opened' and pwort.unique_no = " & Barcode.SQLQuote)

                If Not String.IsNullOrEmpty(SayWhat) Then
                    Speak(SayWhat, 1500)
                End If

            End Using
        End If
    End Sub

    Public Sub AnnounceFlaggedPart(ByVal Barcode As String)
        If Not String.IsNullOrEmpty(Barcode) Then
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim SayWhat = newConn.ExecuteScalar("select top 1 pcn.subject from dbo.partCheckinNotifications pcn where pcn.unique_no = " & Barcode.SQLQuote)

                Dim sentences = Strings.Split(SayWhat, ".")

                If sentences.Length > 0 Then
                    SayWhat = Replace(sentences(0), "%warehouse%", String.Empty)
                End If

                If Not String.IsNullOrEmpty(SayWhat) Then
                    Speak(SayWhat, 1500)
                End If

            End Using
        End If
    End Sub

End Module
