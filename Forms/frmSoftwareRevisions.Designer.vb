<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSoftwareRevisions
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
        Me.tbSoftwareRevisions = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'tbSoftwareRevisions
        '
        Me.tbSoftwareRevisions.BackColor = System.Drawing.Color.White
        Me.tbSoftwareRevisions.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbSoftwareRevisions.Location = New System.Drawing.Point(12, 12)
        Me.tbSoftwareRevisions.Multiline = True
        Me.tbSoftwareRevisions.Name = "tbSoftwareRevisions"
        Me.tbSoftwareRevisions.ReadOnly = True
        Me.tbSoftwareRevisions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbSoftwareRevisions.Size = New System.Drawing.Size(1472, 748)
        Me.tbSoftwareRevisions.TabIndex = 0
        '
        'frmSoftwareRevisions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1496, 772)
        Me.Controls.Add(Me.tbSoftwareRevisions)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSoftwareRevisions"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Software revisions"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbSoftwareRevisions As System.Windows.Forms.TextBox
End Class
