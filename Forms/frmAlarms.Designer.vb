<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAlarms
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
        Me.dgvAlarms = New System.Windows.Forms.DataGridView()
        Me.btnClearAlarms = New System.Windows.Forms.Button()
        CType(Me.dgvAlarms, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvAlarms
        '
        Me.dgvAlarms.AllowUserToAddRows = False
        Me.dgvAlarms.AllowUserToDeleteRows = False
        Me.dgvAlarms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAlarms.Location = New System.Drawing.Point(10, 10)
        Me.dgvAlarms.Name = "dgvAlarms"
        Me.dgvAlarms.ReadOnly = True
        Me.dgvAlarms.Size = New System.Drawing.Size(765, 470)
        Me.dgvAlarms.TabIndex = 0
        '
        'btnClearAlarms
        '
        Me.btnClearAlarms.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearAlarms.Location = New System.Drawing.Point(342, 490)
        Me.btnClearAlarms.Name = "btnClearAlarms"
        Me.btnClearAlarms.Size = New System.Drawing.Size(100, 60)
        Me.btnClearAlarms.TabIndex = 1
        Me.btnClearAlarms.Text = "Clear alarms"
        Me.btnClearAlarms.UseVisualStyleBackColor = True
        '
        'frmAlarms
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.btnClearAlarms)
        Me.Controls.Add(Me.dgvAlarms)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAlarms"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Alarms"
        CType(Me.dgvAlarms, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvAlarms As System.Windows.Forms.DataGridView
    Friend WithEvents btnClearAlarms As System.Windows.Forms.Button
End Class
