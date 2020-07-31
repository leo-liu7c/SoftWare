<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTricks
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
        Me.components = New System.ComponentModel.Container()
        Me.gbSt60 = New System.Windows.Forms.GroupBox()
        Me.btnWS02ForcePartTypeOk = New System.Windows.Forms.Button()
        Me.btnWS02ForcePartOk = New System.Windows.Forms.Button()
        Me.gbSt70 = New System.Windows.Forms.GroupBox()
        Me.btnWS03ForcePartTypeOk = New System.Windows.Forms.Button()
        Me.btnWS03ForcePartOk = New System.Windows.Forms.Button()
        Me.tmrMonitor = New System.Windows.Forms.Timer(Me.components)
        Me.btnSt60ForcePartOk = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnWS04ForcePartTypeOk = New System.Windows.Forms.Button()
        Me.btnWS04ForcePartOk = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnWS05ForcePartTypeOk = New System.Windows.Forms.Button()
        Me.btnWS05ForcePartOk = New System.Windows.Forms.Button()
        Me.gbSt60.SuspendLayout()
        Me.gbSt70.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbSt60
        '
        Me.gbSt60.Controls.Add(Me.btnWS02ForcePartTypeOk)
        Me.gbSt60.Controls.Add(Me.btnWS02ForcePartOk)
        Me.gbSt60.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbSt60.ForeColor = System.Drawing.Color.Blue
        Me.gbSt60.Location = New System.Drawing.Point(24, 10)
        Me.gbSt60.Name = "gbSt60"
        Me.gbSt60.Size = New System.Drawing.Size(220, 255)
        Me.gbSt60.TabIndex = 2
        Me.gbSt60.TabStop = False
        Me.gbSt60.Text = "WS02"
        '
        'btnWS02ForcePartTypeOk
        '
        Me.btnWS02ForcePartTypeOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02ForcePartTypeOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02ForcePartTypeOk.Location = New System.Drawing.Point(10, 140)
        Me.btnWS02ForcePartTypeOk.Name = "btnWS02ForcePartTypeOk"
        Me.btnWS02ForcePartTypeOk.Size = New System.Drawing.Size(200, 100)
        Me.btnWS02ForcePartTypeOk.TabIndex = 3
        Me.btnWS02ForcePartTypeOk.Text = "Force part type OK"
        Me.btnWS02ForcePartTypeOk.UseVisualStyleBackColor = True
        '
        'btnWS02ForcePartOk
        '
        Me.btnWS02ForcePartOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02ForcePartOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02ForcePartOk.Location = New System.Drawing.Point(10, 30)
        Me.btnWS02ForcePartOk.Name = "btnWS02ForcePartOk"
        Me.btnWS02ForcePartOk.Size = New System.Drawing.Size(200, 100)
        Me.btnWS02ForcePartOk.TabIndex = 2
        Me.btnWS02ForcePartOk.Text = "Force result OK"
        Me.btnWS02ForcePartOk.UseVisualStyleBackColor = True
        '
        'gbSt70
        '
        Me.gbSt70.Controls.Add(Me.btnWS03ForcePartTypeOk)
        Me.gbSt70.Controls.Add(Me.btnWS03ForcePartOk)
        Me.gbSt70.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbSt70.ForeColor = System.Drawing.Color.Blue
        Me.gbSt70.Location = New System.Drawing.Point(250, 10)
        Me.gbSt70.Name = "gbSt70"
        Me.gbSt70.Size = New System.Drawing.Size(220, 255)
        Me.gbSt70.TabIndex = 3
        Me.gbSt70.TabStop = False
        Me.gbSt70.Text = "WS03"
        '
        'btnWS03ForcePartTypeOk
        '
        Me.btnWS03ForcePartTypeOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03ForcePartTypeOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03ForcePartTypeOk.Location = New System.Drawing.Point(10, 140)
        Me.btnWS03ForcePartTypeOk.Name = "btnWS03ForcePartTypeOk"
        Me.btnWS03ForcePartTypeOk.Size = New System.Drawing.Size(200, 100)
        Me.btnWS03ForcePartTypeOk.TabIndex = 3
        Me.btnWS03ForcePartTypeOk.Text = "Force part type OK"
        Me.btnWS03ForcePartTypeOk.UseVisualStyleBackColor = True
        '
        'btnWS03ForcePartOk
        '
        Me.btnWS03ForcePartOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03ForcePartOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03ForcePartOk.Location = New System.Drawing.Point(10, 30)
        Me.btnWS03ForcePartOk.Name = "btnWS03ForcePartOk"
        Me.btnWS03ForcePartOk.Size = New System.Drawing.Size(200, 100)
        Me.btnWS03ForcePartOk.TabIndex = 2
        Me.btnWS03ForcePartOk.Text = "Force result OK"
        Me.btnWS03ForcePartOk.UseVisualStyleBackColor = True
        '
        'tmrMonitor
        '
        '
        'btnSt60ForcePartOk
        '
        Me.btnSt60ForcePartOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSt60ForcePartOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSt60ForcePartOk.Location = New System.Drawing.Point(10, 30)
        Me.btnSt60ForcePartOk.Name = "btnSt60ForcePartOk"
        Me.btnSt60ForcePartOk.Size = New System.Drawing.Size(200, 100)
        Me.btnSt60ForcePartOk.TabIndex = 2
        Me.btnSt60ForcePartOk.Text = "Force result OK"
        Me.btnSt60ForcePartOk.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnWS04ForcePartTypeOk)
        Me.GroupBox1.Controls.Add(Me.btnWS04ForcePartOk)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox1.Location = New System.Drawing.Point(476, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(220, 255)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "WS04"
        '
        'btnWS04ForcePartTypeOk
        '
        Me.btnWS04ForcePartTypeOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04ForcePartTypeOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04ForcePartTypeOk.Location = New System.Drawing.Point(10, 140)
        Me.btnWS04ForcePartTypeOk.Name = "btnWS04ForcePartTypeOk"
        Me.btnWS04ForcePartTypeOk.Size = New System.Drawing.Size(200, 100)
        Me.btnWS04ForcePartTypeOk.TabIndex = 3
        Me.btnWS04ForcePartTypeOk.Text = "Force part type OK"
        Me.btnWS04ForcePartTypeOk.UseVisualStyleBackColor = True
        '
        'btnWS04ForcePartOk
        '
        Me.btnWS04ForcePartOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04ForcePartOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04ForcePartOk.Location = New System.Drawing.Point(10, 30)
        Me.btnWS04ForcePartOk.Name = "btnWS04ForcePartOk"
        Me.btnWS04ForcePartOk.Size = New System.Drawing.Size(200, 100)
        Me.btnWS04ForcePartOk.TabIndex = 2
        Me.btnWS04ForcePartOk.Text = "Force result OK"
        Me.btnWS04ForcePartOk.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnWS05ForcePartTypeOk)
        Me.GroupBox2.Controls.Add(Me.btnWS05ForcePartOk)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox2.Location = New System.Drawing.Point(702, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(220, 255)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "WS05"
        '
        'btnWS05ForcePartTypeOk
        '
        Me.btnWS05ForcePartTypeOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05ForcePartTypeOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05ForcePartTypeOk.Location = New System.Drawing.Point(10, 140)
        Me.btnWS05ForcePartTypeOk.Name = "btnWS05ForcePartTypeOk"
        Me.btnWS05ForcePartTypeOk.Size = New System.Drawing.Size(200, 100)
        Me.btnWS05ForcePartTypeOk.TabIndex = 3
        Me.btnWS05ForcePartTypeOk.Text = "Force part type OK"
        Me.btnWS05ForcePartTypeOk.UseVisualStyleBackColor = True
        '
        'btnWS05ForcePartOk
        '
        Me.btnWS05ForcePartOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05ForcePartOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05ForcePartOk.Location = New System.Drawing.Point(10, 30)
        Me.btnWS05ForcePartOk.Name = "btnWS05ForcePartOk"
        Me.btnWS05ForcePartOk.Size = New System.Drawing.Size(200, 100)
        Me.btnWS05ForcePartOk.TabIndex = 2
        Me.btnWS05ForcePartOk.Text = "Force result OK"
        Me.btnWS05ForcePartOk.UseVisualStyleBackColor = True
        '
        'frmTricks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(937, 277)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbSt70)
        Me.Controls.Add(Me.gbSt60)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTricks"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tricks"
        Me.gbSt60.ResumeLayout(False)
        Me.gbSt70.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbSt60 As System.Windows.Forms.GroupBox
    Friend WithEvents btnWS02ForcePartTypeOk As System.Windows.Forms.Button
    Friend WithEvents btnWS02ForcePartOk As System.Windows.Forms.Button
    Friend WithEvents gbSt70 As System.Windows.Forms.GroupBox
    Friend WithEvents btnWS03ForcePartTypeOk As System.Windows.Forms.Button
    Friend WithEvents btnWS03ForcePartOk As System.Windows.Forms.Button
    Friend WithEvents tmrMonitor As System.Windows.Forms.Timer
    Friend WithEvents btnSt60ForcePartOk As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnWS04ForcePartTypeOk As System.Windows.Forms.Button
    Friend WithEvents btnWS04ForcePartOk As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnWS05ForcePartTypeOk As System.Windows.Forms.Button
    Friend WithEvents btnWS05ForcePartOk As System.Windows.Forms.Button
End Class
