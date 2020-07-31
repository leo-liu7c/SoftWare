<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChoiceCopyFrom
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChoiceCopyFrom))
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnWS02 = New System.Windows.Forms.Button()
        Me.btnWS03 = New System.Windows.Forms.Button()
        Me.btnAllWS = New System.Windows.Forms.Button()
        Me.btnWS04 = New System.Windows.Forms.Button()
        Me.btnWS05 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Image = CType(resources.GetObject("btnOk.Image"), System.Drawing.Image)
        Me.btnOk.Location = New System.Drawing.Point(726, 249)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 92)
        Me.btnOk.TabIndex = 86
        Me.btnOk.Text = "Ok"
        Me.btnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.Location = New System.Drawing.Point(241, 249)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 92)
        Me.btnCancel.TabIndex = 85
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnWS02
        '
        Me.btnWS02.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02.Location = New System.Drawing.Point(12, 42)
        Me.btnWS02.Name = "btnWS02"
        Me.btnWS02.Size = New System.Drawing.Size(227, 55)
        Me.btnWS02.TabIndex = 87
        Me.btnWS02.Text = "WS02"
        Me.btnWS02.UseVisualStyleBackColor = True
        '
        'btnWS03
        '
        Me.btnWS03.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03.Location = New System.Drawing.Point(275, 42)
        Me.btnWS03.Name = "btnWS03"
        Me.btnWS03.Size = New System.Drawing.Size(227, 55)
        Me.btnWS03.TabIndex = 89
        Me.btnWS03.Text = "WS03"
        Me.btnWS03.UseVisualStyleBackColor = True
        '
        'btnAllWS
        '
        Me.btnAllWS.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAllWS.Location = New System.Drawing.Point(241, 147)
        Me.btnAllWS.Name = "btnAllWS"
        Me.btnAllWS.Size = New System.Drawing.Size(585, 55)
        Me.btnAllWS.TabIndex = 90
        Me.btnAllWS.Text = "ALL WORK STATION "
        Me.btnAllWS.UseVisualStyleBackColor = True
        '
        'btnWS04
        '
        Me.btnWS04.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04.Location = New System.Drawing.Point(541, 42)
        Me.btnWS04.Name = "btnWS04"
        Me.btnWS04.Size = New System.Drawing.Size(227, 55)
        Me.btnWS04.TabIndex = 91
        Me.btnWS04.Text = "WS04"
        Me.btnWS04.UseVisualStyleBackColor = True
        '
        'btnWS05
        '
        Me.btnWS05.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05.Location = New System.Drawing.Point(824, 42)
        Me.btnWS05.Name = "btnWS05"
        Me.btnWS05.Size = New System.Drawing.Size(227, 55)
        Me.btnWS05.TabIndex = 92
        Me.btnWS05.Text = "WS05"
        Me.btnWS05.UseVisualStyleBackColor = True
        '
        'frmChoiceCopyFrom
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1161, 363)
        Me.Controls.Add(Me.btnWS05)
        Me.Controls.Add(Me.btnWS04)
        Me.Controls.Add(Me.btnAllWS)
        Me.Controls.Add(Me.btnWS03)
        Me.Controls.Add(Me.btnWS02)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChoiceCopyFrom"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Choose Work station for Copy from"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnWS02 As System.Windows.Forms.Button
    Friend WithEvents btnWS03 As System.Windows.Forms.Button
    Friend WithEvents btnAllWS As System.Windows.Forms.Button
    Friend WithEvents btnWS04 As System.Windows.Forms.Button
    Friend WithEvents btnWS05 As System.Windows.Forms.Button
End Class
