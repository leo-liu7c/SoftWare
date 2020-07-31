<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCopyTo
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCopyTo))
        Me.lblReference = New System.Windows.Forms.Label()
        Me.lblParameters = New System.Windows.Forms.Label()
        Me.pbImage = New System.Windows.Forms.PictureBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cbAllReference = New System.Windows.Forms.CheckBox()
        Me.cbAllParameter = New System.Windows.Forms.CheckBox()
        Me.lblWS = New System.Windows.Forms.Label()
        Me.btnWS02 = New System.Windows.Forms.RadioButton()
        Me.btnWS03 = New System.Windows.Forms.RadioButton()
        Me.lbParameters = New System.Windows.Forms.ListBox()
        Me.lbSelectedParameters = New System.Windows.Forms.ListBox()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.LineShape2 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.LineShape1 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lbReferences = New System.Windows.Forms.ListBox()
        Me.lbSelectedReferences = New System.Windows.Forms.ListBox()
        Me.btnWS04 = New System.Windows.Forms.RadioButton()
        Me.btnWS05 = New System.Windows.Forms.RadioButton()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblReference
        '
        Me.lblReference.AutoSize = True
        Me.lblReference.Font = New System.Drawing.Font("Arial Black", 16.25!, System.Drawing.FontStyle.Bold)
        Me.lblReference.Location = New System.Drawing.Point(1280, 13)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.Size = New System.Drawing.Size(272, 31)
        Me.lblReference.TabIndex = 2
        Me.lblReference.Text = "3. Choose references"
        '
        'lblParameters
        '
        Me.lblParameters.AutoSize = True
        Me.lblParameters.Font = New System.Drawing.Font("Arial Black", 16.25!, System.Drawing.FontStyle.Bold)
        Me.lblParameters.Location = New System.Drawing.Point(503, 13)
        Me.lblParameters.Name = "lblParameters"
        Me.lblParameters.Size = New System.Drawing.Size(280, 31)
        Me.lblParameters.TabIndex = 3
        Me.lblParameters.Text = "2. Choose parameters"
        '
        'pbImage
        '
        Me.pbImage.Image = CType(resources.GetObject("pbImage.Image"), System.Drawing.Image)
        Me.pbImage.Location = New System.Drawing.Point(613, 280)
        Me.pbImage.Name = "pbImage"
        Me.pbImage.Size = New System.Drawing.Size(52, 49)
        Me.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbImage.TabIndex = 4
        Me.pbImage.TabStop = False
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Image = CType(resources.GetObject("btnOk.Image"), System.Drawing.Image)
        Me.btnOk.Location = New System.Drawing.Point(928, 580)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 100)
        Me.btnOk.TabIndex = 86
        Me.btnOk.Text = "Ok"
        Me.btnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.Location = New System.Drawing.Point(792, 580)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 100)
        Me.btnCancel.TabIndex = 85
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cbAllReference
        '
        Me.cbAllReference.AutoSize = True
        Me.cbAllReference.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbAllReference.Location = New System.Drawing.Point(1312, 59)
        Me.cbAllReference.Name = "cbAllReference"
        Me.cbAllReference.Size = New System.Drawing.Size(205, 27)
        Me.cbAllReference.TabIndex = 87
        Me.cbAllReference.Text = "Check/Uncheck All"
        Me.cbAllReference.UseVisualStyleBackColor = True
        '
        'cbAllParameter
        '
        Me.cbAllParameter.AutoSize = True
        Me.cbAllParameter.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbAllParameter.Location = New System.Drawing.Point(539, 58)
        Me.cbAllParameter.Name = "cbAllParameter"
        Me.cbAllParameter.Size = New System.Drawing.Size(205, 27)
        Me.cbAllParameter.TabIndex = 87
        Me.cbAllParameter.Text = "Check/Uncheck All"
        Me.cbAllParameter.UseVisualStyleBackColor = True
        '
        'lblWS
        '
        Me.lblWS.AutoSize = True
        Me.lblWS.Font = New System.Drawing.Font("Arial Black", 16.25!, System.Drawing.FontStyle.Bold)
        Me.lblWS.Location = New System.Drawing.Point(51, 13)
        Me.lblWS.Name = "lblWS"
        Me.lblWS.Size = New System.Drawing.Size(178, 31)
        Me.lblWS.TabIndex = 3
        Me.lblWS.Text = "1. Choose WS"
        '
        'btnWS02
        '
        Me.btnWS02.AutoSize = True
        Me.btnWS02.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02.Location = New System.Drawing.Point(84, 79)
        Me.btnWS02.Name = "btnWS02"
        Me.btnWS02.Size = New System.Drawing.Size(106, 36)
        Me.btnWS02.TabIndex = 88
        Me.btnWS02.TabStop = True
        Me.btnWS02.Text = "WS02"
        Me.btnWS02.UseVisualStyleBackColor = True
        '
        'btnWS03
        '
        Me.btnWS03.AutoSize = True
        Me.btnWS03.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03.Location = New System.Drawing.Point(84, 165)
        Me.btnWS03.Name = "btnWS03"
        Me.btnWS03.Size = New System.Drawing.Size(106, 36)
        Me.btnWS03.TabIndex = 88
        Me.btnWS03.TabStop = True
        Me.btnWS03.Text = "WS03"
        Me.btnWS03.UseVisualStyleBackColor = True
        '
        'lbParameters
        '
        Me.lbParameters.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lbParameters.FormattingEnabled = True
        Me.lbParameters.ItemHeight = 19
        Me.lbParameters.Location = New System.Drawing.Point(277, 91)
        Me.lbParameters.Name = "lbParameters"
        Me.lbParameters.Size = New System.Drawing.Size(330, 460)
        Me.lbParameters.TabIndex = 89
        '
        'lbSelectedParameters
        '
        Me.lbSelectedParameters.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lbSelectedParameters.FormattingEnabled = True
        Me.lbSelectedParameters.ItemHeight = 19
        Me.lbSelectedParameters.Location = New System.Drawing.Point(671, 91)
        Me.lbSelectedParameters.Name = "lbSelectedParameters"
        Me.lbSelectedParameters.Size = New System.Drawing.Size(330, 460)
        Me.lbSelectedParameters.TabIndex = 89
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.LineShape2, Me.LineShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(1814, 690)
        Me.ShapeContainer1.TabIndex = 90
        Me.ShapeContainer1.TabStop = False
        '
        'LineShape2
        '
        Me.LineShape2.BorderWidth = 5
        Me.LineShape2.Name = "LineShape2"
        Me.LineShape2.X1 = 1027
        Me.LineShape2.X2 = 1027
        Me.LineShape2.Y1 = 21
        Me.LineShape2.Y2 = 550
        '
        'LineShape1
        '
        Me.LineShape1.BorderWidth = 5
        Me.LineShape1.Name = "LineShape1"
        Me.LineShape1.X1 = 250
        Me.LineShape1.X2 = 250
        Me.LineShape1.Y1 = 21
        Me.LineShape1.Y2 = 550
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(1392, 281)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(52, 49)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'lbReferences
        '
        Me.lbReferences.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lbReferences.FormattingEnabled = True
        Me.lbReferences.ItemHeight = 19
        Me.lbReferences.Location = New System.Drawing.Point(1056, 92)
        Me.lbReferences.Name = "lbReferences"
        Me.lbReferences.Size = New System.Drawing.Size(330, 460)
        Me.lbReferences.TabIndex = 89
        '
        'lbSelectedReferences
        '
        Me.lbSelectedReferences.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lbSelectedReferences.FormattingEnabled = True
        Me.lbSelectedReferences.ItemHeight = 19
        Me.lbSelectedReferences.Location = New System.Drawing.Point(1450, 92)
        Me.lbSelectedReferences.Name = "lbSelectedReferences"
        Me.lbSelectedReferences.Size = New System.Drawing.Size(330, 460)
        Me.lbSelectedReferences.TabIndex = 89
        '
        'btnWS04
        '
        Me.btnWS04.AutoSize = True
        Me.btnWS04.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04.Location = New System.Drawing.Point(84, 235)
        Me.btnWS04.Name = "btnWS04"
        Me.btnWS04.Size = New System.Drawing.Size(106, 36)
        Me.btnWS04.TabIndex = 91
        Me.btnWS04.TabStop = True
        Me.btnWS04.Text = "WS04"
        Me.btnWS04.UseVisualStyleBackColor = True
        '
        'btnWS05
        '
        Me.btnWS05.AutoSize = True
        Me.btnWS05.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05.Location = New System.Drawing.Point(84, 314)
        Me.btnWS05.Name = "btnWS05"
        Me.btnWS05.Size = New System.Drawing.Size(106, 36)
        Me.btnWS05.TabIndex = 92
        Me.btnWS05.TabStop = True
        Me.btnWS05.Text = "WS05"
        Me.btnWS05.UseVisualStyleBackColor = True
        '
        'frmCopyTo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1814, 690)
        Me.Controls.Add(Me.btnWS05)
        Me.Controls.Add(Me.btnWS04)
        Me.Controls.Add(Me.lbSelectedReferences)
        Me.Controls.Add(Me.lbReferences)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lbSelectedParameters)
        Me.Controls.Add(Me.lbParameters)
        Me.Controls.Add(Me.btnWS03)
        Me.Controls.Add(Me.btnWS02)
        Me.Controls.Add(Me.cbAllParameter)
        Me.Controls.Add(Me.cbAllReference)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.pbImage)
        Me.Controls.Add(Me.lblWS)
        Me.Controls.Add(Me.lblParameters)
        Me.Controls.Add(Me.lblReference)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCopyTo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Copy To"
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblReference As System.Windows.Forms.Label
    Friend WithEvents lblParameters As System.Windows.Forms.Label
    Friend WithEvents pbImage As System.Windows.Forms.PictureBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cbAllReference As System.Windows.Forms.CheckBox
    Friend WithEvents cbAllParameter As System.Windows.Forms.CheckBox
    Friend WithEvents lblWS As System.Windows.Forms.Label
    Friend WithEvents btnWS02 As System.Windows.Forms.RadioButton
    Friend WithEvents btnWS03 As System.Windows.Forms.RadioButton
    Friend WithEvents lbParameters As System.Windows.Forms.ListBox
    Friend WithEvents lbSelectedParameters As System.Windows.Forms.ListBox
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents LineShape2 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents LineShape1 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lbReferences As System.Windows.Forms.ListBox
    Friend WithEvents lbSelectedReferences As System.Windows.Forms.ListBox
    Friend WithEvents btnWS04 As System.Windows.Forms.RadioButton
    Friend WithEvents btnWS05 As System.Windows.Forms.RadioButton
End Class
