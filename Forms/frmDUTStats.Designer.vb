<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDUTStats
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDUTStats))
        Me.DGV1 = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TBTotal = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TBOK = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TBNOK = New System.Windows.Forms.TextBox()
        Me.TBYield = New System.Windows.Forms.TextBox()
        Me.BTReset = New System.Windows.Forms.Button()
        CType(Me.DGV1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DGV1
        '
        Me.DGV1.AllowUserToAddRows = False
        Me.DGV1.AllowUserToDeleteRows = False
        Me.DGV1.AllowUserToOrderColumns = True
        Me.DGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV1.Dock = System.Windows.Forms.DockStyle.Top
        Me.DGV1.Location = New System.Drawing.Point(0, 0)
        Me.DGV1.Name = "DGV1"
        Me.DGV1.ReadOnly = True
        Me.DGV1.RowTemplate.Height = 23
        Me.DGV1.Size = New System.Drawing.Size(834, 471)
        Me.DGV1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 492)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 12)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "总数/TOTAL"
        '
        'TBTotal
        '
        Me.TBTotal.Enabled = False
        Me.TBTotal.Location = New System.Drawing.Point(14, 505)
        Me.TBTotal.Name = "TBTotal"
        Me.TBTotal.Size = New System.Drawing.Size(100, 21)
        Me.TBTotal.TabIndex = 2
        Me.TBTotal.Text = "0"
        Me.TBTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(171, 492)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 12)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "良品/OK"
        '
        'TBOK
        '
        Me.TBOK.BackColor = System.Drawing.Color.YellowGreen
        Me.TBOK.Enabled = False
        Me.TBOK.Location = New System.Drawing.Point(173, 505)
        Me.TBOK.Name = "TBOK"
        Me.TBOK.Size = New System.Drawing.Size(100, 21)
        Me.TBOK.TabIndex = 2
        Me.TBOK.Text = "0"
        Me.TBOK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(330, 492)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 12)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "不良品/NOK"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(489, 492)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 12)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "良率/Yield"
        '
        'TBNOK
        '
        Me.TBNOK.BackColor = System.Drawing.Color.Tomato
        Me.TBNOK.Enabled = False
        Me.TBNOK.Location = New System.Drawing.Point(332, 505)
        Me.TBNOK.Name = "TBNOK"
        Me.TBNOK.Size = New System.Drawing.Size(100, 21)
        Me.TBNOK.TabIndex = 2
        Me.TBNOK.Text = "0"
        Me.TBNOK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TBYield
        '
        Me.TBYield.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.TBYield.Enabled = False
        Me.TBYield.Location = New System.Drawing.Point(491, 505)
        Me.TBYield.Name = "TBYield"
        Me.TBYield.Size = New System.Drawing.Size(100, 21)
        Me.TBYield.TabIndex = 2
        Me.TBYield.Text = "0"
        Me.TBYield.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'BTReset
        '
        Me.BTReset.Location = New System.Drawing.Point(650, 503)
        Me.BTReset.Name = "BTReset"
        Me.BTReset.Size = New System.Drawing.Size(144, 23)
        Me.BTReset.TabIndex = 3
        Me.BTReset.Text = "复位数据/Reset Data"
        Me.BTReset.UseVisualStyleBackColor = True
        '
        'frmDUTStats
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(834, 546)
        Me.Controls.Add(Me.BTReset)
        Me.Controls.Add(Me.TBYield)
        Me.Controls.Add(Me.TBOK)
        Me.Controls.Add(Me.TBNOK)
        Me.Controls.Add(Me.TBTotal)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DGV1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmDUTStats"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmDUTStats"
        CType(Me.DGV1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DGV1 As DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents TBTotal As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TBOK As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents TBNOK As TextBox
    Friend WithEvents TBYield As TextBox
    Friend WithEvents BTReset As Button
End Class
