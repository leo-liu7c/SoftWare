<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessage
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMessage))
        Me.labMessage = New System.Windows.Forms.Label()
        Me.imgMessage = New System.Windows.Forms.PictureBox()
        Me.imgWarning = New System.Windows.Forms.PictureBox()
        Me.imgError = New System.Windows.Forms.PictureBox()
        Me.imgQuestion = New System.Windows.Forms.PictureBox()
        Me.imgInformation = New System.Windows.Forms.PictureBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnNo = New System.Windows.Forms.Button()
        Me.btnYes = New System.Windows.Forms.Button()
        Me.TimerRetry = New System.Windows.Forms.Timer(Me.components)
        CType(Me.imgMessage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgError, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgQuestion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgInformation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'labMessage
        '
        Me.labMessage.BackColor = System.Drawing.Color.White
        Me.labMessage.Font = New System.Drawing.Font("Arial Black", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labMessage.Location = New System.Drawing.Point(87, 8)
        Me.labMessage.Name = "labMessage"
        Me.labMessage.Size = New System.Drawing.Size(672, 165)
        Me.labMessage.TabIndex = 4
        Me.labMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'imgMessage
        '
        Me.imgMessage.Location = New System.Drawing.Point(12, 11)
        Me.imgMessage.Name = "imgMessage"
        Me.imgMessage.Size = New System.Drawing.Size(69, 66)
        Me.imgMessage.TabIndex = 5
        Me.imgMessage.TabStop = False
        '
        'imgWarning
        '
        Me.imgWarning.Image = CType(resources.GetObject("imgWarning.Image"), System.Drawing.Image)
        Me.imgWarning.Location = New System.Drawing.Point(572, 191)
        Me.imgWarning.Name = "imgWarning"
        Me.imgWarning.Size = New System.Drawing.Size(71, 62)
        Me.imgWarning.TabIndex = 6
        Me.imgWarning.TabStop = False
        Me.imgWarning.Visible = False
        '
        'imgError
        '
        Me.imgError.Image = CType(resources.GetObject("imgError.Image"), System.Drawing.Image)
        Me.imgError.Location = New System.Drawing.Point(543, 191)
        Me.imgError.Name = "imgError"
        Me.imgError.Size = New System.Drawing.Size(66, 59)
        Me.imgError.TabIndex = 7
        Me.imgError.TabStop = False
        Me.imgError.Visible = False
        '
        'imgQuestion
        '
        Me.imgQuestion.Image = CType(resources.GetObject("imgQuestion.Image"), System.Drawing.Image)
        Me.imgQuestion.Location = New System.Drawing.Point(455, 191)
        Me.imgQuestion.Name = "imgQuestion"
        Me.imgQuestion.Size = New System.Drawing.Size(59, 59)
        Me.imgQuestion.TabIndex = 8
        Me.imgQuestion.TabStop = False
        Me.imgQuestion.Visible = False
        '
        'imgInformation
        '
        Me.imgInformation.Image = CType(resources.GetObject("imgInformation.Image"), System.Drawing.Image)
        Me.imgInformation.InitialImage = CType(resources.GetObject("imgInformation.InitialImage"), System.Drawing.Image)
        Me.imgInformation.Location = New System.Drawing.Point(577, 185)
        Me.imgInformation.Name = "imgInformation"
        Me.imgInformation.Size = New System.Drawing.Size(69, 59)
        Me.imgInformation.TabIndex = 9
        Me.imgInformation.TabStop = False
        Me.imgInformation.Visible = False
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Image = CType(resources.GetObject("btnOk.Image"), System.Drawing.Image)
        Me.btnOk.Location = New System.Drawing.Point(496, 195)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 92)
        Me.btnOk.TabIndex = 84
        Me.btnOk.Text = "Ok"
        Me.btnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnOk.UseVisualStyleBackColor = True
        Me.btnOk.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.Location = New System.Drawing.Point(304, 195)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 92)
        Me.btnCancel.TabIndex = 83
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        Me.btnCancel.Visible = False
        '
        'btnNo
        '
        Me.btnNo.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNo.Image = CType(resources.GetObject("btnNo.Image"), System.Drawing.Image)
        Me.btnNo.Location = New System.Drawing.Point(126, 195)
        Me.btnNo.Name = "btnNo"
        Me.btnNo.Size = New System.Drawing.Size(100, 92)
        Me.btnNo.TabIndex = 85
        Me.btnNo.Text = "NO"
        Me.btnNo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnNo.UseVisualStyleBackColor = True
        Me.btnNo.Visible = False
        '
        'btnYes
        '
        Me.btnYes.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnYes.Image = CType(resources.GetObject("btnYes.Image"), System.Drawing.Image)
        Me.btnYes.Location = New System.Drawing.Point(496, 195)
        Me.btnYes.Name = "btnYes"
        Me.btnYes.Size = New System.Drawing.Size(100, 92)
        Me.btnYes.TabIndex = 86
        Me.btnYes.Text = "YES"
        Me.btnYes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnYes.UseVisualStyleBackColor = True
        Me.btnYes.Visible = False
        '
        'TimerRetry
        '
        '
        'frmMessage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(771, 299)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnYes)
        Me.Controls.Add(Me.btnNo)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.imgInformation)
        Me.Controls.Add(Me.imgQuestion)
        Me.Controls.Add(Me.imgError)
        Me.Controls.Add(Me.imgWarning)
        Me.Controls.Add(Me.imgMessage)
        Me.Controls.Add(Me.labMessage)
        Me.Name = "frmMessage"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Message"
        CType(Me.imgMessage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgWarning, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgError, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgQuestion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgInformation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents labMessage As System.Windows.Forms.Label
    Friend WithEvents imgMessage As System.Windows.Forms.PictureBox
    Friend WithEvents imgWarning As System.Windows.Forms.PictureBox
    Friend WithEvents imgError As System.Windows.Forms.PictureBox
    Friend WithEvents imgQuestion As System.Windows.Forms.PictureBox
    Friend WithEvents imgInformation As System.Windows.Forms.PictureBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnNo As System.Windows.Forms.Button
    Friend WithEvents btnYes As System.Windows.Forms.Button
    Friend WithEvents TimerRetry As Timer
End Class
