<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CameraCapture
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CameraCapture))
        Me.CameraPictureBox = New System.Windows.Forms.PictureBox()
        Me.RetakeButton = New System.Windows.Forms.Button()
        Me.TakeButton = New System.Windows.Forms.Button()
        Me.txtLegendText = New System.Windows.Forms.Label()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.PathsToSaveDdl = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtError = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DllCameraDrivers = New System.Windows.Forms.ComboBox()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        CType(Me.CameraPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CameraPictureBox
        '
        Me.CameraPictureBox.BackColor = System.Drawing.Color.Transparent
        Me.CameraPictureBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.CameraPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.CameraPictureBox.Name = "CameraPictureBox"
        Me.CameraPictureBox.Size = New System.Drawing.Size(812, 538)
        Me.CameraPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.CameraPictureBox.TabIndex = 2
        Me.CameraPictureBox.TabStop = False
        '
        'RetakeButton
        '
        Me.RetakeButton.Image = CType(resources.GetObject("RetakeButton.Image"), System.Drawing.Image)
        Me.RetakeButton.Location = New System.Drawing.Point(330, 556)
        Me.RetakeButton.Name = "RetakeButton"
        Me.RetakeButton.Size = New System.Drawing.Size(76, 67)
        Me.RetakeButton.TabIndex = 3
        Me.RetakeButton.UseVisualStyleBackColor = True
        Me.RetakeButton.Visible = False
        '
        'TakeButton
        '
        Me.TakeButton.Image = CType(resources.GetObject("TakeButton.Image"), System.Drawing.Image)
        Me.TakeButton.Location = New System.Drawing.Point(330, 556)
        Me.TakeButton.Name = "TakeButton"
        Me.TakeButton.Size = New System.Drawing.Size(76, 67)
        Me.TakeButton.TabIndex = 4
        Me.TakeButton.UseVisualStyleBackColor = True
        '
        'txtLegendText
        '
        Me.txtLegendText.AutoSize = True
        Me.txtLegendText.Location = New System.Drawing.Point(11, 645)
        Me.txtLegendText.Name = "txtLegendText"
        Me.txtLegendText.Size = New System.Drawing.Size(139, 13)
        Me.txtLegendText.TabIndex = 5
        Me.txtLegendText.Text = "This image will be saved in: "
        '
        'SaveButton
        '
        Me.SaveButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.SaveButton.ForeColor = System.Drawing.SystemColors.Control
        Me.SaveButton.Location = New System.Drawing.Point(412, 556)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(76, 67)
        Me.SaveButton.TabIndex = 6
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = False
        Me.SaveButton.Visible = False
        '
        'PathsToSaveDdl
        '
        Me.PathsToSaveDdl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PathsToSaveDdl.FormattingEnabled = True
        Me.PathsToSaveDdl.Location = New System.Drawing.Point(146, 642)
        Me.PathsToSaveDdl.Name = "PathsToSaveDdl"
        Me.PathsToSaveDdl.Size = New System.Drawing.Size(180, 21)
        Me.PathsToSaveDdl.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(310, 290)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(167, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Error loading your camera preview"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtError
        '
        Me.txtError.AutoSize = True
        Me.txtError.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtError.Location = New System.Drawing.Point(130, 248)
        Me.txtError.MaximumSize = New System.Drawing.Size(551, 0)
        Me.txtError.Name = "txtError"
        Me.txtError.Size = New System.Drawing.Size(551, 29)
        Me.txtError.TabIndex = 9
        Me.txtError.Text = "Something went wrong detecting your camera."
        Me.txtError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.txtError.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(588, 645)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Camera Driver:"
        Me.Label2.Visible = False
        '
        'DllCameraDrivers
        '
        Me.DllCameraDrivers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DllCameraDrivers.FormattingEnabled = True
        Me.DllCameraDrivers.Location = New System.Drawing.Point(672, 641)
        Me.DllCameraDrivers.Name = "DllCameraDrivers"
        Me.DllCameraDrivers.Size = New System.Drawing.Size(121, 21)
        Me.DllCameraDrivers.TabIndex = 11
        Me.DllCameraDrivers.Visible = False
        '
        'RichTextBox1
        '
        Me.RichTextBox1.ForeColor = System.Drawing.Color.Red
        Me.RichTextBox1.Location = New System.Drawing.Point(12, 560)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.Size = New System.Drawing.Size(166, 63)
        Me.RichTextBox1.TabIndex = 13
        Me.RichTextBox1.Text = "Please make sure to use a compatible camera. Recommended: C920s PRO HD WEBCAM"
        '
        'CameraCapture
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(812, 680)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.DllCameraDrivers)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtError)
        Me.Controls.Add(Me.PathsToSaveDdl)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.txtLegendText)
        Me.Controls.Add(Me.TakeButton)
        Me.Controls.Add(Me.RetakeButton)
        Me.Controls.Add(Me.CameraPictureBox)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "CameraCapture"
        Me.Text = "Camera Capture"
        CType(Me.CameraPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CameraPictureBox As PictureBox
    Friend WithEvents RetakeButton As Button
    Friend WithEvents TakeButton As Button
    Friend WithEvents txtLegendText As Label
    Friend WithEvents SaveButton As Button
    Friend WithEvents PathsToSaveDdl As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtError As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents DllCameraDrivers As ComboBox
    Friend WithEvents RichTextBox1 As RichTextBox
End Class
