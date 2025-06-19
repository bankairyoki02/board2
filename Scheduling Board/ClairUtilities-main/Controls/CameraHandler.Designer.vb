<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CameraHandler
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CameraHandler))
        Me.pnlPrimaryQuery = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TakePictureButton = New System.Windows.Forms.Button()
        Me.RemoveButton = New System.Windows.Forms.Button()
        Me.AcceptButton = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.pnlPrimaryQuery.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlPrimaryQuery
        '
        Me.pnlPrimaryQuery.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.pnlPrimaryQuery.Controls.Add(Me.ComboBox1)
        Me.pnlPrimaryQuery.Controls.Add(Me.AcceptButton)
        Me.pnlPrimaryQuery.Controls.Add(Me.RemoveButton)
        Me.pnlPrimaryQuery.Controls.Add(Me.TakePictureButton)
        Me.pnlPrimaryQuery.Controls.Add(Me.Label1)
        Me.pnlPrimaryQuery.Controls.Add(Me.Panel1)
        Me.pnlPrimaryQuery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlPrimaryQuery.Location = New System.Drawing.Point(0, 0)
        Me.pnlPrimaryQuery.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.pnlPrimaryQuery.Name = "pnlPrimaryQuery"
        Me.pnlPrimaryQuery.Size = New System.Drawing.Size(881, 480)
        Me.pnlPrimaryQuery.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(12, 49)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(856, 335)
        Me.Panel1.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Camera"
        '
        'TakePictureButton
        '
        Me.TakePictureButton.BackColor = System.Drawing.Color.Black
        Me.TakePictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.TakePictureButton.Image = CType(resources.GetObject("TakePictureButton.Image"), System.Drawing.Image)
        Me.TakePictureButton.Location = New System.Drawing.Point(381, 386)
        Me.TakePictureButton.Name = "TakePictureButton"
        Me.TakePictureButton.Size = New System.Drawing.Size(101, 94)
        Me.TakePictureButton.TabIndex = 1
        Me.TakePictureButton.UseVisualStyleBackColor = False
        '
        'RemoveButton
        '
        Me.RemoveButton.BackColor = System.Drawing.Color.Transparent
        Me.RemoveButton.Image = CType(resources.GetObject("RemoveButton.Image"), System.Drawing.Image)
        Me.RemoveButton.Location = New System.Drawing.Point(449, 400)
        Me.RemoveButton.Name = "RemoveButton"
        Me.RemoveButton.Size = New System.Drawing.Size(65, 67)
        Me.RemoveButton.TabIndex = 2
        Me.RemoveButton.UseVisualStyleBackColor = False
        '
        'AcceptButton
        '
        Me.AcceptButton.BackColor = System.Drawing.Color.Transparent
        Me.AcceptButton.Image = CType(resources.GetObject("AcceptButton.Image"), System.Drawing.Image)
        Me.AcceptButton.Location = New System.Drawing.Point(355, 400)
        Me.AcceptButton.Name = "AcceptButton"
        Me.AcceptButton.Size = New System.Drawing.Size(65, 67)
        Me.AcceptButton.TabIndex = 3
        Me.AcceptButton.UseVisualStyleBackColor = False
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(622, 9)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(246, 32)
        Me.ComboBox1.TabIndex = 4
        '
        'CameraHandler
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlPrimaryQuery)
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "CameraHandler"
        Me.Size = New System.Drawing.Size(881, 480)
        Me.pnlPrimaryQuery.ResumeLayout(False)
        Me.pnlPrimaryQuery.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlPrimaryQuery As Panel
    Friend WithEvents TakePictureButton As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents AcceptButton As Button
    Friend WithEvents RemoveButton As Button
    Friend WithEvents ComboBox1 As ComboBox
End Class
