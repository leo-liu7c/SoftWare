<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalibration
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCalibration))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblECB = New System.Windows.Forms.Label()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.lblSoftwareTitle = New System.Windows.Forms.Label()
        Me.pbLogoValeo = New System.Windows.Forms.PictureBox()
        Me.tcStation = New System.Windows.Forms.TabControl()
        Me.tpStation70 = New System.Windows.Forms.TabPage()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcStation.SuspendLayout()
        Me.tpStation70.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.Color.White
        Me.lblCountry.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.Color.Blue
        Me.lblCountry.Location = New System.Drawing.Point(1720, 70)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.Size = New System.Drawing.Size(200, 30)
        Me.lblCountry.TabIndex = 71
        Me.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCity
        '
        Me.lblCity.BackColor = System.Drawing.Color.White
        Me.lblCity.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCity.ForeColor = System.Drawing.Color.Blue
        Me.lblCity.Location = New System.Drawing.Point(1720, 40)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(200, 30)
        Me.lblCity.TabIndex = 70
        Me.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblECB
        '
        Me.lblECB.BackColor = System.Drawing.Color.White
        Me.lblECB.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblECB.ForeColor = System.Drawing.Color.Blue
        Me.lblECB.Location = New System.Drawing.Point(1720, 0)
        Me.lblECB.Name = "lblECB"
        Me.lblECB.Size = New System.Drawing.Size(200, 40)
        Me.lblECB.TabIndex = 69
        Me.lblECB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblFormTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormTitle.Location = New System.Drawing.Point(200, 50)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1520, 50)
        Me.lblFormTitle.TabIndex = 68
        Me.lblFormTitle.Text = "Analog input calibration"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSoftwareTitle
        '
        Me.lblSoftwareTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblSoftwareTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSoftwareTitle.Location = New System.Drawing.Point(200, 0)
        Me.lblSoftwareTitle.Name = "lblSoftwareTitle"
        Me.lblSoftwareTitle.Size = New System.Drawing.Size(1520, 50)
        Me.lblSoftwareTitle.TabIndex = 67
        Me.lblSoftwareTitle.Text = "EOL Test Bench FH / FHM  DAIMLER"
        Me.lblSoftwareTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pbLogoValeo
        '
        Me.pbLogoValeo.Image = CType(resources.GetObject("pbLogoValeo.Image"), System.Drawing.Image)
        Me.pbLogoValeo.Location = New System.Drawing.Point(0, 0)
        Me.pbLogoValeo.Name = "pbLogoValeo"
        Me.pbLogoValeo.Size = New System.Drawing.Size(200, 100)
        Me.pbLogoValeo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbLogoValeo.TabIndex = 66
        Me.pbLogoValeo.TabStop = False
        '
        'tcStation
        '
        Me.tcStation.Controls.Add(Me.tpStation70)
        Me.tcStation.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcStation.Location = New System.Drawing.Point(10, 110)
        Me.tcStation.Name = "tcStation"
        Me.tcStation.SelectedIndex = 0
        Me.tcStation.Size = New System.Drawing.Size(1900, 960)
        Me.tcStation.TabIndex = 72
        '
        'tpStation70
        '
        Me.tpStation70.BackColor = System.Drawing.SystemColors.Control
        Me.tpStation70.Controls.Add(Me.DataGridView1)
        Me.tpStation70.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpStation70.Location = New System.Drawing.Point(4, 33)
        Me.tpStation70.Name = "tpStation70"
        Me.tpStation70.Padding = New System.Windows.Forms.Padding(3)
        Me.tpStation70.Size = New System.Drawing.Size(1892, 923)
        Me.tpStation70.TabIndex = 1
        Me.tpStation70.Text = "station FW"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.Location = New System.Drawing.Point(6, 6)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(400, 770)
        Me.DataGridView1.TabIndex = 94
        '
        'frmCalibration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1920, 1070)
        Me.Controls.Add(Me.tcStation)
        Me.Controls.Add(Me.lblCountry)
        Me.Controls.Add(Me.lblCity)
        Me.Controls.Add(Me.lblECB)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Controls.Add(Me.lblSoftwareTitle)
        Me.Controls.Add(Me.pbLogoValeo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmCalibration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Analog input calibration"
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcStation.ResumeLayout(False)
        Me.tpStation70.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblCountry As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblECB As System.Windows.Forms.Label
    Friend WithEvents lblFormTitle As System.Windows.Forms.Label
    Friend WithEvents lblSoftwareTitle As System.Windows.Forms.Label
    Friend WithEvents pbLogoValeo As System.Windows.Forms.PictureBox
    Friend WithEvents tcStation As System.Windows.Forms.TabControl
    Friend WithEvents tpStation70 As System.Windows.Forms.TabPage
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
End Class
