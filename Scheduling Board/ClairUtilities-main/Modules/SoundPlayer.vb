Public Module SoundPlayer
    Public ReadOnly SoundsPath = String.Format("{0}\sounds\", Environment.GetEnvironmentVariable("ESSVBDIR"))

    Public Sub PlayCheckSoftwareSound()
        Dim pathToPlay = String.Format("{0}{1}", SoundsPath, "Check Software.wav")
        Dim player = New Media.SoundPlayer(pathToPlay)
        player.Play()
    End Sub

    Public Sub PlayCheckFirmwareSound()
        Dim pathToPlay = String.Format("{0}{1}", SoundsPath, "Check Firmware.wav")
        Dim player = New Media.SoundPlayer(pathToPlay)
        player.Play()
    End Sub
End Module
