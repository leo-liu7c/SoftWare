<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMaintenance_EOL
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMaintenance_EOL))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblECB = New System.Windows.Forms.Label()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.lblSoftwareTitle = New System.Windows.Forms.Label()
        Me.ssStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.tmrMonitor = New System.Windows.Forms.Timer(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.gbstFWDigitalOutputs = New System.Windows.Forms.GroupBox()
        Me.btnOpenDiag = New System.Windows.Forms.Button()
        Me.txtWS02_Rx = New System.Windows.Forms.TextBox()
        Me.txtWS02_Tx = New System.Windows.Forms.TextBox()
        Me.btnAll_Off = New System.Windows.Forms.Button()
        Me.bntAll_On = New System.Windows.Forms.Button()
        Me.btn_Bck_Off = New System.Windows.Forms.Button()
        Me.btn_Bck_On = New System.Windows.Forms.Button()
        Me.btn_TTL_Off = New System.Windows.Forms.Button()
        Me.btn_TTL_On = New System.Windows.Forms.Button()
        Me.btnLinWS02 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_31 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_30 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_29 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_28 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_27 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_26 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_25 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_24 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_15 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_14 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_13 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_12 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_11 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_10 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_9 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_8 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_23 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_22 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_21 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_20 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_19 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_18 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_17 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_16 = New System.Windows.Forms.Button()
        Me.gbstFWAnalogInputs = New System.Windows.Forms.GroupBox()
        Me.dgvWS02AnalogInputs = New System.Windows.Forms.DataGridView()
        Me.btnWS02_DO_7 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_6 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_5 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_4 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_3 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_2 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_1 = New System.Windows.Forms.Button()
        Me.btnWS02_DO_0 = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lbl_RR_Pull_A = New System.Windows.Forms.Label()
        Me.lbl_RR_Pull_M = New System.Windows.Forms.Label()
        Me.lbl_RL_Pull_A = New System.Windows.Forms.Label()
        Me.lbl_RL_Pull_M = New System.Windows.Forms.Label()
        Me.lbl_RR_Push_A = New System.Windows.Forms.Label()
        Me.lbl_RR_Push_M = New System.Windows.Forms.Label()
        Me.lbl_RL_Push_A = New System.Windows.Forms.Label()
        Me.lbl_RL_Push_M = New System.Windows.Forms.Label()
        Me.lbl_FR_Pull_A = New System.Windows.Forms.Label()
        Me.lbl_FR_Pull_M = New System.Windows.Forms.Label()
        Me.lbl_FL_Pull_A = New System.Windows.Forms.Label()
        Me.lbl_FL_Pull_M = New System.Windows.Forms.Label()
        Me.lbl_FR_Push_A = New System.Windows.Forms.Label()
        Me.lbl_FR_Push_M = New System.Windows.Forms.Label()
        Me.lbl_Fl_Push_A = New System.Windows.Forms.Label()
        Me.lbl_Fl_Push_M = New System.Windows.Forms.Label()
        Me.lbl_Child = New System.Windows.Forms.Label()
        Me.lbl_SR = New System.Windows.Forms.Label()
        Me.lbl_Fold = New System.Windows.Forms.Label()
        Me.lbl_SL = New System.Windows.Forms.Label()
        Me.lbl_L = New System.Windows.Forms.Label()
        Me.lbl_R = New System.Windows.Forms.Label()
        Me.lbl_DN = New System.Windows.Forms.Label()
        Me.lbl_UP = New System.Windows.Forms.Label()
        Me.lbl_MIRROR_ADJUSTMENT = New System.Windows.Forms.Label()
        Me.lbl_WL_REAR_FRONT_PASSENGER = New System.Windows.Forms.Label()
        Me.lbl_WL_REAR_RIGHT = New System.Windows.Forms.Label()
        Me.lbl_WL_REAR_LEFT = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnRead_AIN = New System.Windows.Forms.Button()
        Me.btnRead_Din = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnWS03Diag = New System.Windows.Forms.Button()
        Me.txtWS03_Rx = New System.Windows.Forms.TextBox()
        Me.txtWS03_Tx = New System.Windows.Forms.TextBox()
        Me.btnWS03Lin = New System.Windows.Forms.Button()
        Me.btnWS03_DO_15 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_14 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_13 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_12 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_11 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_10 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_9 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_8 = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.dgvWS03AnalogInputs = New System.Windows.Forms.DataGridView()
        Me.btnWS03_DO_7 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_6 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_5 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_4 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_3 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_2 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_1 = New System.Windows.Forms.Button()
        Me.btnWS03_DO_0 = New System.Windows.Forms.Button()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.btnWS04_DO_15 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_14 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_13 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_12 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_11 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_10 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_9 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_8 = New System.Windows.Forms.Button()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.btnWS04_DO_7 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_6 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_5 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_4 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_3 = New System.Windows.Forms.Button()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.dgvWS04AnalogInputs = New System.Windows.Forms.DataGridView()
        Me.btnWS04_DO_2 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_1 = New System.Windows.Forms.Button()
        Me.btnWS04_DO_0 = New System.Windows.Forms.Button()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.btnWS05_DO_15 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_14 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_13 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_12 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_11 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_10 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_9 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_8 = New System.Windows.Forms.Button()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.dgvWS05AnalogInputs = New System.Windows.Forms.DataGridView()
        Me.btnWS05_DO_7 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_6 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_5 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_4 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_3 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_2 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_1 = New System.Windows.Forms.Button()
        Me.btnWS05_DO_0 = New System.Windows.Forms.Button()
        Me.pbLogoValeo = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.tmrLoop = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.CustomerLogo = New System.Windows.Forms.PictureBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.gbstFWDigitalOutputs.SuspendLayout()
        Me.gbstFWAnalogInputs.SuspendLayout()
        CType(Me.dgvWS02AnalogInputs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.dgvWS03AnalogInputs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage4.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        CType(Me.dgvWS04AnalogInputs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage5.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.dgvWS05AnalogInputs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CustomerLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.Color.White
        Me.lblCountry.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.Color.Blue
        Me.lblCountry.Location = New System.Drawing.Point(1660, 0)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.Size = New System.Drawing.Size(260, 92)
        Me.lblCountry.TabIndex = 65
        Me.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCity
        '
        Me.lblCity.BackColor = System.Drawing.Color.White
        Me.lblCity.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCity.ForeColor = System.Drawing.Color.Blue
        Me.lblCity.Location = New System.Drawing.Point(1720, 37)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(200, 28)
        Me.lblCity.TabIndex = 64
        Me.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblECB
        '
        Me.lblECB.BackColor = System.Drawing.Color.White
        Me.lblECB.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblECB.ForeColor = System.Drawing.Color.Blue
        Me.lblECB.Location = New System.Drawing.Point(1720, 0)
        Me.lblECB.Name = "lblECB"
        Me.lblECB.Size = New System.Drawing.Size(200, 37)
        Me.lblECB.TabIndex = 63
        Me.lblECB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblFormTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormTitle.Location = New System.Drawing.Point(200, 46)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1520, 46)
        Me.lblFormTitle.TabIndex = 62
        Me.lblFormTitle.Text = "Maintenance"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSoftwareTitle
        '
        Me.lblSoftwareTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblSoftwareTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSoftwareTitle.Location = New System.Drawing.Point(200, 0)
        Me.lblSoftwareTitle.Name = "lblSoftwareTitle"
        Me.lblSoftwareTitle.Size = New System.Drawing.Size(1520, 46)
        Me.lblSoftwareTitle.TabIndex = 61
        Me.lblSoftwareTitle.Text = "EOL Test Bench RSA SQUARE SUV WILI"
        Me.lblSoftwareTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ssStatusStrip
        '
        Me.ssStatusStrip.Location = New System.Drawing.Point(0, 1058)
        Me.ssStatusStrip.Name = "ssStatusStrip"
        Me.ssStatusStrip.Size = New System.Drawing.Size(1920, 22)
        Me.ssStatusStrip.TabIndex = 66
        Me.ssStatusStrip.Text = "StatusStrip1"
        '
        'tmrMonitor
        '
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(375, 50)
        Me.TabControl1.Location = New System.Drawing.Point(12, 98)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1896, 957)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl1.TabIndex = 106
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.gbstFWDigitalOutputs)
        Me.TabPage1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage1.Location = New System.Drawing.Point(4, 54)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1888, 899)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "WS02 Camera Electric"
        '
        'gbstFWDigitalOutputs
        '
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnOpenDiag)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.txtWS02_Rx)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.txtWS02_Tx)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnAll_Off)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.bntAll_On)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btn_Bck_Off)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btn_Bck_On)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btn_TTL_Off)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btn_TTL_On)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnLinWS02)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_31)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_30)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_29)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_28)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_27)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_26)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_25)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_24)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_15)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_14)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_13)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_12)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_11)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_10)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_9)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_8)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_23)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_22)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_21)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_20)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_19)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_18)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_17)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_16)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.gbstFWAnalogInputs)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_7)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_6)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_5)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_4)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_3)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_2)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_1)
        Me.gbstFWDigitalOutputs.Controls.Add(Me.btnWS02_DO_0)
        Me.gbstFWDigitalOutputs.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbstFWDigitalOutputs.ForeColor = System.Drawing.Color.Blue
        Me.gbstFWDigitalOutputs.Location = New System.Drawing.Point(9, 6)
        Me.gbstFWDigitalOutputs.Name = "gbstFWDigitalOutputs"
        Me.gbstFWDigitalOutputs.Size = New System.Drawing.Size(1876, 887)
        Me.gbstFWDigitalOutputs.TabIndex = 103
        Me.gbstFWDigitalOutputs.TabStop = False
        Me.gbstFWDigitalOutputs.Text = "Digital outputs"
        '
        'btnOpenDiag
        '
        Me.btnOpenDiag.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnOpenDiag.ForeColor = System.Drawing.Color.Black
        Me.btnOpenDiag.Location = New System.Drawing.Point(1547, 23)
        Me.btnOpenDiag.Name = "btnOpenDiag"
        Me.btnOpenDiag.Size = New System.Drawing.Size(287, 53)
        Me.btnOpenDiag.TabIndex = 310
        Me.btnOpenDiag.Text = "Open Diag"
        Me.btnOpenDiag.UseVisualStyleBackColor = False
        '
        'txtWS02_Rx
        '
        Me.txtWS02_Rx.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWS02_Rx.Location = New System.Drawing.Point(1253, 464)
        Me.txtWS02_Rx.Multiline = True
        Me.txtWS02_Rx.Name = "txtWS02_Rx"
        Me.txtWS02_Rx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtWS02_Rx.Size = New System.Drawing.Size(531, 322)
        Me.txtWS02_Rx.TabIndex = 309
        '
        'txtWS02_Tx
        '
        Me.txtWS02_Tx.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWS02_Tx.Location = New System.Drawing.Point(1253, 293)
        Me.txtWS02_Tx.Multiline = True
        Me.txtWS02_Tx.Name = "txtWS02_Tx"
        Me.txtWS02_Tx.Size = New System.Drawing.Size(531, 157)
        Me.txtWS02_Tx.TabIndex = 308
        '
        'btnAll_Off
        '
        Me.btnAll_Off.BackColor = System.Drawing.Color.Red
        Me.btnAll_Off.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAll_Off.ForeColor = System.Drawing.Color.Black
        Me.btnAll_Off.Location = New System.Drawing.Point(1547, 204)
        Me.btnAll_Off.Name = "btnAll_Off"
        Me.btnAll_Off.Size = New System.Drawing.Size(287, 55)
        Me.btnAll_Off.TabIndex = 307
        Me.btnAll_Off.Text = "ALL OFF"
        Me.btnAll_Off.UseVisualStyleBackColor = False
        '
        'bntAll_On
        '
        Me.bntAll_On.BackColor = System.Drawing.Color.Lime
        Me.bntAll_On.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bntAll_On.ForeColor = System.Drawing.Color.Black
        Me.bntAll_On.Location = New System.Drawing.Point(1206, 204)
        Me.bntAll_On.Name = "bntAll_On"
        Me.bntAll_On.Size = New System.Drawing.Size(287, 55)
        Me.bntAll_On.TabIndex = 306
        Me.bntAll_On.Text = "ALL ON"
        Me.bntAll_On.UseVisualStyleBackColor = False
        '
        'btn_Bck_Off
        '
        Me.btn_Bck_Off.BackColor = System.Drawing.Color.Red
        Me.btn_Bck_Off.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Bck_Off.ForeColor = System.Drawing.Color.Black
        Me.btn_Bck_Off.Location = New System.Drawing.Point(1547, 143)
        Me.btn_Bck_Off.Name = "btn_Bck_Off"
        Me.btn_Bck_Off.Size = New System.Drawing.Size(287, 55)
        Me.btn_Bck_Off.TabIndex = 302
        Me.btn_Bck_Off.Text = "Backlifght Select OFF"
        Me.btn_Bck_Off.UseVisualStyleBackColor = False
        '
        'btn_Bck_On
        '
        Me.btn_Bck_On.BackColor = System.Drawing.Color.Lime
        Me.btn_Bck_On.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Bck_On.ForeColor = System.Drawing.Color.Black
        Me.btn_Bck_On.Location = New System.Drawing.Point(1206, 143)
        Me.btn_Bck_On.Name = "btn_Bck_On"
        Me.btn_Bck_On.Size = New System.Drawing.Size(287, 55)
        Me.btn_Bck_On.TabIndex = 301
        Me.btn_Bck_On.Text = "Backlifght Select ON"
        Me.btn_Bck_On.UseVisualStyleBackColor = False
        '
        'btn_TTL_Off
        '
        Me.btn_TTL_Off.BackColor = System.Drawing.Color.Red
        Me.btn_TTL_Off.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_TTL_Off.ForeColor = System.Drawing.Color.Black
        Me.btn_TTL_Off.Location = New System.Drawing.Point(1547, 82)
        Me.btn_TTL_Off.Name = "btn_TTL_Off"
        Me.btn_TTL_Off.Size = New System.Drawing.Size(287, 55)
        Me.btn_TTL_Off.TabIndex = 296
        Me.btn_TTL_Off.Text = "TellTale OFF" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.btn_TTL_Off.UseVisualStyleBackColor = False
        '
        'btn_TTL_On
        '
        Me.btn_TTL_On.BackColor = System.Drawing.Color.Lime
        Me.btn_TTL_On.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_TTL_On.ForeColor = System.Drawing.Color.Black
        Me.btn_TTL_On.Location = New System.Drawing.Point(1206, 82)
        Me.btn_TTL_On.Name = "btn_TTL_On"
        Me.btn_TTL_On.Size = New System.Drawing.Size(287, 55)
        Me.btn_TTL_On.TabIndex = 295
        Me.btn_TTL_On.Text = "TellTale ON"
        Me.btn_TTL_On.UseVisualStyleBackColor = False
        '
        'btnLinWS02
        '
        Me.btnLinWS02.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnLinWS02.ForeColor = System.Drawing.Color.Black
        Me.btnLinWS02.Location = New System.Drawing.Point(1206, 24)
        Me.btnLinWS02.Name = "btnLinWS02"
        Me.btnLinWS02.Size = New System.Drawing.Size(287, 53)
        Me.btnLinWS02.TabIndex = 294
        Me.btnLinWS02.Text = "Open Lin"
        Me.btnLinWS02.UseVisualStyleBackColor = False
        '
        'btnWS02_DO_31
        '
        Me.btnWS02_DO_31.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_31.Location = New System.Drawing.Point(301, 743)
        Me.btnWS02_DO_31.Name = "btnWS02_DO_31"
        Me.btnWS02_DO_31.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_31.TabIndex = 154
        Me.btnWS02_DO_31.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_30
        '
        Me.btnWS02_DO_30.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_30.Location = New System.Drawing.Point(301, 696)
        Me.btnWS02_DO_30.Name = "btnWS02_DO_30"
        Me.btnWS02_DO_30.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_30.TabIndex = 153
        Me.btnWS02_DO_30.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_29
        '
        Me.btnWS02_DO_29.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_29.Location = New System.Drawing.Point(300, 649)
        Me.btnWS02_DO_29.Name = "btnWS02_DO_29"
        Me.btnWS02_DO_29.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_29.TabIndex = 152
        Me.btnWS02_DO_29.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_28
        '
        Me.btnWS02_DO_28.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_28.Location = New System.Drawing.Point(301, 602)
        Me.btnWS02_DO_28.Name = "btnWS02_DO_28"
        Me.btnWS02_DO_28.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_28.TabIndex = 151
        Me.btnWS02_DO_28.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_27
        '
        Me.btnWS02_DO_27.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_27.Location = New System.Drawing.Point(301, 554)
        Me.btnWS02_DO_27.Name = "btnWS02_DO_27"
        Me.btnWS02_DO_27.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_27.TabIndex = 150
        Me.btnWS02_DO_27.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_26
        '
        Me.btnWS02_DO_26.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_26.Location = New System.Drawing.Point(301, 507)
        Me.btnWS02_DO_26.Name = "btnWS02_DO_26"
        Me.btnWS02_DO_26.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_26.TabIndex = 149
        Me.btnWS02_DO_26.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_25
        '
        Me.btnWS02_DO_25.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_25.Location = New System.Drawing.Point(300, 459)
        Me.btnWS02_DO_25.Name = "btnWS02_DO_25"
        Me.btnWS02_DO_25.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_25.TabIndex = 148
        Me.btnWS02_DO_25.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_24
        '
        Me.btnWS02_DO_24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_24.Location = New System.Drawing.Point(301, 407)
        Me.btnWS02_DO_24.Name = "btnWS02_DO_24"
        Me.btnWS02_DO_24.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_24.TabIndex = 147
        Me.btnWS02_DO_24.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_15
        '
        Me.btnWS02_DO_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_15.Location = New System.Drawing.Point(5, 743)
        Me.btnWS02_DO_15.Name = "btnWS02_DO_15"
        Me.btnWS02_DO_15.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_15.TabIndex = 146
        Me.btnWS02_DO_15.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_14
        '
        Me.btnWS02_DO_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_14.Location = New System.Drawing.Point(5, 696)
        Me.btnWS02_DO_14.Name = "btnWS02_DO_14"
        Me.btnWS02_DO_14.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_14.TabIndex = 145
        Me.btnWS02_DO_14.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_13
        '
        Me.btnWS02_DO_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_13.Location = New System.Drawing.Point(4, 649)
        Me.btnWS02_DO_13.Name = "btnWS02_DO_13"
        Me.btnWS02_DO_13.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_13.TabIndex = 144
        Me.btnWS02_DO_13.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_12
        '
        Me.btnWS02_DO_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_12.Location = New System.Drawing.Point(5, 602)
        Me.btnWS02_DO_12.Name = "btnWS02_DO_12"
        Me.btnWS02_DO_12.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_12.TabIndex = 143
        Me.btnWS02_DO_12.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_11
        '
        Me.btnWS02_DO_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_11.Location = New System.Drawing.Point(5, 554)
        Me.btnWS02_DO_11.Name = "btnWS02_DO_11"
        Me.btnWS02_DO_11.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_11.TabIndex = 142
        Me.btnWS02_DO_11.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_10
        '
        Me.btnWS02_DO_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_10.Location = New System.Drawing.Point(5, 507)
        Me.btnWS02_DO_10.Name = "btnWS02_DO_10"
        Me.btnWS02_DO_10.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_10.TabIndex = 141
        Me.btnWS02_DO_10.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_9
        '
        Me.btnWS02_DO_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_9.Location = New System.Drawing.Point(4, 459)
        Me.btnWS02_DO_9.Name = "btnWS02_DO_9"
        Me.btnWS02_DO_9.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_9.TabIndex = 140
        Me.btnWS02_DO_9.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_8
        '
        Me.btnWS02_DO_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_8.Location = New System.Drawing.Point(5, 407)
        Me.btnWS02_DO_8.Name = "btnWS02_DO_8"
        Me.btnWS02_DO_8.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_8.TabIndex = 139
        Me.btnWS02_DO_8.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_23
        '
        Me.btnWS02_DO_23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_23.Location = New System.Drawing.Point(301, 359)
        Me.btnWS02_DO_23.Name = "btnWS02_DO_23"
        Me.btnWS02_DO_23.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_23.TabIndex = 138
        Me.btnWS02_DO_23.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_22
        '
        Me.btnWS02_DO_22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_22.Location = New System.Drawing.Point(301, 312)
        Me.btnWS02_DO_22.Name = "btnWS02_DO_22"
        Me.btnWS02_DO_22.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_22.TabIndex = 137
        Me.btnWS02_DO_22.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_21
        '
        Me.btnWS02_DO_21.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_21.Location = New System.Drawing.Point(300, 265)
        Me.btnWS02_DO_21.Name = "btnWS02_DO_21"
        Me.btnWS02_DO_21.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_21.TabIndex = 136
        Me.btnWS02_DO_21.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_20
        '
        Me.btnWS02_DO_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_20.Location = New System.Drawing.Point(301, 218)
        Me.btnWS02_DO_20.Name = "btnWS02_DO_20"
        Me.btnWS02_DO_20.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_20.TabIndex = 135
        Me.btnWS02_DO_20.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_19
        '
        Me.btnWS02_DO_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_19.Location = New System.Drawing.Point(301, 170)
        Me.btnWS02_DO_19.Name = "btnWS02_DO_19"
        Me.btnWS02_DO_19.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_19.TabIndex = 134
        Me.btnWS02_DO_19.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_18
        '
        Me.btnWS02_DO_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_18.Location = New System.Drawing.Point(301, 123)
        Me.btnWS02_DO_18.Name = "btnWS02_DO_18"
        Me.btnWS02_DO_18.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_18.TabIndex = 133
        Me.btnWS02_DO_18.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_17
        '
        Me.btnWS02_DO_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_17.Location = New System.Drawing.Point(300, 75)
        Me.btnWS02_DO_17.Name = "btnWS02_DO_17"
        Me.btnWS02_DO_17.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_17.TabIndex = 132
        Me.btnWS02_DO_17.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_16
        '
        Me.btnWS02_DO_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_16.Location = New System.Drawing.Point(301, 23)
        Me.btnWS02_DO_16.Name = "btnWS02_DO_16"
        Me.btnWS02_DO_16.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_16.TabIndex = 131
        Me.btnWS02_DO_16.UseVisualStyleBackColor = True
        '
        'gbstFWAnalogInputs
        '
        Me.gbstFWAnalogInputs.Controls.Add(Me.dgvWS02AnalogInputs)
        Me.gbstFWAnalogInputs.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbstFWAnalogInputs.ForeColor = System.Drawing.Color.Blue
        Me.gbstFWAnalogInputs.Location = New System.Drawing.Point(593, 1)
        Me.gbstFWAnalogInputs.Name = "gbstFWAnalogInputs"
        Me.gbstFWAnalogInputs.Size = New System.Drawing.Size(580, 880)
        Me.gbstFWAnalogInputs.TabIndex = 106
        Me.gbstFWAnalogInputs.TabStop = False
        Me.gbstFWAnalogInputs.Text = "Analog inputs"
        '
        'dgvWS02AnalogInputs
        '
        Me.dgvWS02AnalogInputs.AllowUserToAddRows = False
        Me.dgvWS02AnalogInputs.AllowUserToDeleteRows = False
        Me.dgvWS02AnalogInputs.AllowUserToResizeColumns = False
        Me.dgvWS02AnalogInputs.AllowUserToResizeRows = False
        Me.dgvWS02AnalogInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvWS02AnalogInputs.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvWS02AnalogInputs.Location = New System.Drawing.Point(6, 23)
        Me.dgvWS02AnalogInputs.Name = "dgvWS02AnalogInputs"
        Me.dgvWS02AnalogInputs.ReadOnly = True
        Me.dgvWS02AnalogInputs.Size = New System.Drawing.Size(568, 374)
        Me.dgvWS02AnalogInputs.TabIndex = 92
        '
        'btnWS02_DO_7
        '
        Me.btnWS02_DO_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_7.Location = New System.Drawing.Point(6, 359)
        Me.btnWS02_DO_7.Name = "btnWS02_DO_7"
        Me.btnWS02_DO_7.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_7.TabIndex = 130
        Me.btnWS02_DO_7.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_6
        '
        Me.btnWS02_DO_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_6.Location = New System.Drawing.Point(6, 312)
        Me.btnWS02_DO_6.Name = "btnWS02_DO_6"
        Me.btnWS02_DO_6.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_6.TabIndex = 129
        Me.btnWS02_DO_6.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_5
        '
        Me.btnWS02_DO_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_5.Location = New System.Drawing.Point(5, 265)
        Me.btnWS02_DO_5.Name = "btnWS02_DO_5"
        Me.btnWS02_DO_5.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_5.TabIndex = 128
        Me.btnWS02_DO_5.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_4
        '
        Me.btnWS02_DO_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_4.Location = New System.Drawing.Point(6, 218)
        Me.btnWS02_DO_4.Name = "btnWS02_DO_4"
        Me.btnWS02_DO_4.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_4.TabIndex = 127
        Me.btnWS02_DO_4.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_3
        '
        Me.btnWS02_DO_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_3.Location = New System.Drawing.Point(6, 170)
        Me.btnWS02_DO_3.Name = "btnWS02_DO_3"
        Me.btnWS02_DO_3.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_3.TabIndex = 126
        Me.btnWS02_DO_3.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_2
        '
        Me.btnWS02_DO_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_2.Location = New System.Drawing.Point(6, 123)
        Me.btnWS02_DO_2.Name = "btnWS02_DO_2"
        Me.btnWS02_DO_2.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_2.TabIndex = 125
        Me.btnWS02_DO_2.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_1
        '
        Me.btnWS02_DO_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_1.Location = New System.Drawing.Point(5, 75)
        Me.btnWS02_DO_1.Name = "btnWS02_DO_1"
        Me.btnWS02_DO_1.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_1.TabIndex = 124
        Me.btnWS02_DO_1.UseVisualStyleBackColor = True
        '
        'btnWS02_DO_0
        '
        Me.btnWS02_DO_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS02_DO_0.Location = New System.Drawing.Point(6, 23)
        Me.btnWS02_DO_0.Name = "btnWS02_DO_0"
        Me.btnWS02_DO_0.Size = New System.Drawing.Size(259, 42)
        Me.btnWS02_DO_0.TabIndex = 123
        Me.btnWS02_DO_0.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 54)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(1888, 899)
        Me.TabPage2.TabIndex = 3
        Me.TabPage2.Text = "WS03 Haptic #1"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lbl_RR_Pull_A)
        Me.GroupBox1.Controls.Add(Me.lbl_RR_Pull_M)
        Me.GroupBox1.Controls.Add(Me.lbl_RL_Pull_A)
        Me.GroupBox1.Controls.Add(Me.lbl_RL_Pull_M)
        Me.GroupBox1.Controls.Add(Me.lbl_RR_Push_A)
        Me.GroupBox1.Controls.Add(Me.lbl_RR_Push_M)
        Me.GroupBox1.Controls.Add(Me.lbl_RL_Push_A)
        Me.GroupBox1.Controls.Add(Me.lbl_RL_Push_M)
        Me.GroupBox1.Controls.Add(Me.lbl_FR_Pull_A)
        Me.GroupBox1.Controls.Add(Me.lbl_FR_Pull_M)
        Me.GroupBox1.Controls.Add(Me.lbl_FL_Pull_A)
        Me.GroupBox1.Controls.Add(Me.lbl_FL_Pull_M)
        Me.GroupBox1.Controls.Add(Me.lbl_FR_Push_A)
        Me.GroupBox1.Controls.Add(Me.lbl_FR_Push_M)
        Me.GroupBox1.Controls.Add(Me.lbl_Fl_Push_A)
        Me.GroupBox1.Controls.Add(Me.lbl_Fl_Push_M)
        Me.GroupBox1.Controls.Add(Me.lbl_Child)
        Me.GroupBox1.Controls.Add(Me.lbl_SR)
        Me.GroupBox1.Controls.Add(Me.lbl_Fold)
        Me.GroupBox1.Controls.Add(Me.lbl_SL)
        Me.GroupBox1.Controls.Add(Me.lbl_L)
        Me.GroupBox1.Controls.Add(Me.lbl_R)
        Me.GroupBox1.Controls.Add(Me.lbl_DN)
        Me.GroupBox1.Controls.Add(Me.lbl_UP)
        Me.GroupBox1.Controls.Add(Me.lbl_MIRROR_ADJUSTMENT)
        Me.GroupBox1.Controls.Add(Me.lbl_WL_REAR_FRONT_PASSENGER)
        Me.GroupBox1.Controls.Add(Me.lbl_WL_REAR_RIGHT)
        Me.GroupBox1.Controls.Add(Me.lbl_WL_REAR_LEFT)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.btnRead_AIN)
        Me.GroupBox1.Controls.Add(Me.btnRead_Din)
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.Controls.Add(Me.btnWS03Diag)
        Me.GroupBox1.Controls.Add(Me.txtWS03_Rx)
        Me.GroupBox1.Controls.Add(Me.txtWS03_Tx)
        Me.GroupBox1.Controls.Add(Me.btnWS03Lin)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_15)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_14)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_13)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_12)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_11)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_10)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_9)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_8)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_7)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_6)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_5)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_4)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_3)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_2)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_1)
        Me.GroupBox1.Controls.Add(Me.btnWS03_DO_0)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox1.Location = New System.Drawing.Point(15, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1859, 884)
        Me.GroupBox1.TabIndex = 105
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Digital outputs"
        '
        'lbl_RR_Pull_A
        '
        Me.lbl_RR_Pull_A.BackColor = System.Drawing.Color.Lime
        Me.lbl_RR_Pull_A.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RR_Pull_A.ForeColor = System.Drawing.Color.Black
        Me.lbl_RR_Pull_A.Location = New System.Drawing.Point(1793, 602)
        Me.lbl_RR_Pull_A.Name = "lbl_RR_Pull_A"
        Me.lbl_RR_Pull_A.Size = New System.Drawing.Size(25, 23)
        Me.lbl_RR_Pull_A.TabIndex = 376
        Me.lbl_RR_Pull_A.Text = "A"
        Me.lbl_RR_Pull_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_RR_Pull_A.Visible = False
        '
        'lbl_RR_Pull_M
        '
        Me.lbl_RR_Pull_M.BackColor = System.Drawing.Color.Lime
        Me.lbl_RR_Pull_M.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RR_Pull_M.ForeColor = System.Drawing.Color.Black
        Me.lbl_RR_Pull_M.Location = New System.Drawing.Point(1765, 602)
        Me.lbl_RR_Pull_M.Name = "lbl_RR_Pull_M"
        Me.lbl_RR_Pull_M.Size = New System.Drawing.Size(25, 23)
        Me.lbl_RR_Pull_M.TabIndex = 375
        Me.lbl_RR_Pull_M.Text = "M"
        Me.lbl_RR_Pull_M.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_RR_Pull_M.Visible = False
        '
        'lbl_RL_Pull_A
        '
        Me.lbl_RL_Pull_A.BackColor = System.Drawing.Color.Lime
        Me.lbl_RL_Pull_A.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RL_Pull_A.ForeColor = System.Drawing.Color.Black
        Me.lbl_RL_Pull_A.Location = New System.Drawing.Point(1711, 602)
        Me.lbl_RL_Pull_A.Name = "lbl_RL_Pull_A"
        Me.lbl_RL_Pull_A.Size = New System.Drawing.Size(25, 23)
        Me.lbl_RL_Pull_A.TabIndex = 374
        Me.lbl_RL_Pull_A.Text = "A"
        Me.lbl_RL_Pull_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_RL_Pull_A.Visible = False
        '
        'lbl_RL_Pull_M
        '
        Me.lbl_RL_Pull_M.BackColor = System.Drawing.Color.Lime
        Me.lbl_RL_Pull_M.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RL_Pull_M.ForeColor = System.Drawing.Color.Black
        Me.lbl_RL_Pull_M.Location = New System.Drawing.Point(1683, 602)
        Me.lbl_RL_Pull_M.Name = "lbl_RL_Pull_M"
        Me.lbl_RL_Pull_M.Size = New System.Drawing.Size(25, 23)
        Me.lbl_RL_Pull_M.TabIndex = 373
        Me.lbl_RL_Pull_M.Text = "M"
        Me.lbl_RL_Pull_M.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_RL_Pull_M.Visible = False
        '
        'lbl_RR_Push_A
        '
        Me.lbl_RR_Push_A.BackColor = System.Drawing.Color.Lime
        Me.lbl_RR_Push_A.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RR_Push_A.ForeColor = System.Drawing.Color.Black
        Me.lbl_RR_Push_A.Location = New System.Drawing.Point(1793, 509)
        Me.lbl_RR_Push_A.Name = "lbl_RR_Push_A"
        Me.lbl_RR_Push_A.Size = New System.Drawing.Size(25, 23)
        Me.lbl_RR_Push_A.TabIndex = 372
        Me.lbl_RR_Push_A.Text = "A"
        Me.lbl_RR_Push_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_RR_Push_A.Visible = False
        '
        'lbl_RR_Push_M
        '
        Me.lbl_RR_Push_M.BackColor = System.Drawing.Color.Lime
        Me.lbl_RR_Push_M.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RR_Push_M.ForeColor = System.Drawing.Color.Black
        Me.lbl_RR_Push_M.Location = New System.Drawing.Point(1765, 509)
        Me.lbl_RR_Push_M.Name = "lbl_RR_Push_M"
        Me.lbl_RR_Push_M.Size = New System.Drawing.Size(25, 23)
        Me.lbl_RR_Push_M.TabIndex = 371
        Me.lbl_RR_Push_M.Text = "M"
        Me.lbl_RR_Push_M.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_RR_Push_M.Visible = False
        '
        'lbl_RL_Push_A
        '
        Me.lbl_RL_Push_A.BackColor = System.Drawing.Color.Lime
        Me.lbl_RL_Push_A.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RL_Push_A.ForeColor = System.Drawing.Color.Black
        Me.lbl_RL_Push_A.Location = New System.Drawing.Point(1712, 509)
        Me.lbl_RL_Push_A.Name = "lbl_RL_Push_A"
        Me.lbl_RL_Push_A.Size = New System.Drawing.Size(25, 23)
        Me.lbl_RL_Push_A.TabIndex = 370
        Me.lbl_RL_Push_A.Text = "A"
        Me.lbl_RL_Push_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_RL_Push_A.Visible = False
        '
        'lbl_RL_Push_M
        '
        Me.lbl_RL_Push_M.BackColor = System.Drawing.Color.Lime
        Me.lbl_RL_Push_M.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RL_Push_M.ForeColor = System.Drawing.Color.Black
        Me.lbl_RL_Push_M.Location = New System.Drawing.Point(1684, 509)
        Me.lbl_RL_Push_M.Name = "lbl_RL_Push_M"
        Me.lbl_RL_Push_M.Size = New System.Drawing.Size(25, 23)
        Me.lbl_RL_Push_M.TabIndex = 369
        Me.lbl_RL_Push_M.Text = "M"
        Me.lbl_RL_Push_M.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_RL_Push_M.Visible = False
        '
        'lbl_FR_Pull_A
        '
        Me.lbl_FR_Pull_A.BackColor = System.Drawing.Color.Lime
        Me.lbl_FR_Pull_A.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FR_Pull_A.ForeColor = System.Drawing.Color.Black
        Me.lbl_FR_Pull_A.Location = New System.Drawing.Point(1794, 459)
        Me.lbl_FR_Pull_A.Name = "lbl_FR_Pull_A"
        Me.lbl_FR_Pull_A.Size = New System.Drawing.Size(25, 23)
        Me.lbl_FR_Pull_A.TabIndex = 368
        Me.lbl_FR_Pull_A.Text = "A"
        Me.lbl_FR_Pull_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_FR_Pull_A.Visible = False
        '
        'lbl_FR_Pull_M
        '
        Me.lbl_FR_Pull_M.BackColor = System.Drawing.Color.Lime
        Me.lbl_FR_Pull_M.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FR_Pull_M.ForeColor = System.Drawing.Color.Black
        Me.lbl_FR_Pull_M.Location = New System.Drawing.Point(1766, 459)
        Me.lbl_FR_Pull_M.Name = "lbl_FR_Pull_M"
        Me.lbl_FR_Pull_M.Size = New System.Drawing.Size(25, 23)
        Me.lbl_FR_Pull_M.TabIndex = 367
        Me.lbl_FR_Pull_M.Text = "M"
        Me.lbl_FR_Pull_M.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_FR_Pull_M.Visible = False
        '
        'lbl_FL_Pull_A
        '
        Me.lbl_FL_Pull_A.BackColor = System.Drawing.Color.Lime
        Me.lbl_FL_Pull_A.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FL_Pull_A.ForeColor = System.Drawing.Color.Black
        Me.lbl_FL_Pull_A.Location = New System.Drawing.Point(1712, 459)
        Me.lbl_FL_Pull_A.Name = "lbl_FL_Pull_A"
        Me.lbl_FL_Pull_A.Size = New System.Drawing.Size(25, 23)
        Me.lbl_FL_Pull_A.TabIndex = 366
        Me.lbl_FL_Pull_A.Text = "A"
        Me.lbl_FL_Pull_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_FL_Pull_A.Visible = False
        '
        'lbl_FL_Pull_M
        '
        Me.lbl_FL_Pull_M.BackColor = System.Drawing.Color.Lime
        Me.lbl_FL_Pull_M.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FL_Pull_M.ForeColor = System.Drawing.Color.Black
        Me.lbl_FL_Pull_M.Location = New System.Drawing.Point(1684, 459)
        Me.lbl_FL_Pull_M.Name = "lbl_FL_Pull_M"
        Me.lbl_FL_Pull_M.Size = New System.Drawing.Size(25, 23)
        Me.lbl_FL_Pull_M.TabIndex = 365
        Me.lbl_FL_Pull_M.Text = "M"
        Me.lbl_FL_Pull_M.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_FL_Pull_M.Visible = False
        '
        'lbl_FR_Push_A
        '
        Me.lbl_FR_Push_A.BackColor = System.Drawing.Color.Lime
        Me.lbl_FR_Push_A.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FR_Push_A.ForeColor = System.Drawing.Color.Black
        Me.lbl_FR_Push_A.Location = New System.Drawing.Point(1794, 366)
        Me.lbl_FR_Push_A.Name = "lbl_FR_Push_A"
        Me.lbl_FR_Push_A.Size = New System.Drawing.Size(25, 23)
        Me.lbl_FR_Push_A.TabIndex = 364
        Me.lbl_FR_Push_A.Text = "A"
        Me.lbl_FR_Push_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_FR_Push_A.Visible = False
        '
        'lbl_FR_Push_M
        '
        Me.lbl_FR_Push_M.BackColor = System.Drawing.Color.Lime
        Me.lbl_FR_Push_M.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FR_Push_M.ForeColor = System.Drawing.Color.Black
        Me.lbl_FR_Push_M.Location = New System.Drawing.Point(1766, 366)
        Me.lbl_FR_Push_M.Name = "lbl_FR_Push_M"
        Me.lbl_FR_Push_M.Size = New System.Drawing.Size(25, 23)
        Me.lbl_FR_Push_M.TabIndex = 363
        Me.lbl_FR_Push_M.Text = "M"
        Me.lbl_FR_Push_M.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_FR_Push_M.Visible = False
        '
        'lbl_Fl_Push_A
        '
        Me.lbl_Fl_Push_A.BackColor = System.Drawing.Color.Lime
        Me.lbl_Fl_Push_A.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Fl_Push_A.ForeColor = System.Drawing.Color.Black
        Me.lbl_Fl_Push_A.Location = New System.Drawing.Point(1713, 366)
        Me.lbl_Fl_Push_A.Name = "lbl_Fl_Push_A"
        Me.lbl_Fl_Push_A.Size = New System.Drawing.Size(25, 23)
        Me.lbl_Fl_Push_A.TabIndex = 362
        Me.lbl_Fl_Push_A.Text = "A"
        Me.lbl_Fl_Push_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_Fl_Push_A.Visible = False
        '
        'lbl_Fl_Push_M
        '
        Me.lbl_Fl_Push_M.BackColor = System.Drawing.Color.Lime
        Me.lbl_Fl_Push_M.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Fl_Push_M.ForeColor = System.Drawing.Color.Black
        Me.lbl_Fl_Push_M.Location = New System.Drawing.Point(1685, 366)
        Me.lbl_Fl_Push_M.Name = "lbl_Fl_Push_M"
        Me.lbl_Fl_Push_M.Size = New System.Drawing.Size(25, 23)
        Me.lbl_Fl_Push_M.TabIndex = 361
        Me.lbl_Fl_Push_M.Text = "M"
        Me.lbl_Fl_Push_M.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_Fl_Push_M.Visible = False
        '
        'lbl_Child
        '
        Me.lbl_Child.BackColor = System.Drawing.Color.Lime
        Me.lbl_Child.Location = New System.Drawing.Point(1724, 630)
        Me.lbl_Child.Name = "lbl_Child"
        Me.lbl_Child.Size = New System.Drawing.Size(67, 23)
        Me.lbl_Child.TabIndex = 360
        Me.lbl_Child.Visible = False
        '
        'lbl_SR
        '
        Me.lbl_SR.BackColor = System.Drawing.Color.Lime
        Me.lbl_SR.Location = New System.Drawing.Point(1797, 285)
        Me.lbl_SR.Name = "lbl_SR"
        Me.lbl_SR.Size = New System.Drawing.Size(25, 23)
        Me.lbl_SR.TabIndex = 359
        Me.lbl_SR.Visible = False
        '
        'lbl_Fold
        '
        Me.lbl_Fold.BackColor = System.Drawing.Color.Lime
        Me.lbl_Fold.Location = New System.Drawing.Point(1739, 285)
        Me.lbl_Fold.Name = "lbl_Fold"
        Me.lbl_Fold.Size = New System.Drawing.Size(25, 23)
        Me.lbl_Fold.TabIndex = 358
        Me.lbl_Fold.Visible = False
        '
        'lbl_SL
        '
        Me.lbl_SL.BackColor = System.Drawing.Color.Lime
        Me.lbl_SL.Location = New System.Drawing.Point(1685, 285)
        Me.lbl_SL.Name = "lbl_SL"
        Me.lbl_SL.Size = New System.Drawing.Size(25, 23)
        Me.lbl_SL.TabIndex = 357
        Me.lbl_SL.Visible = False
        '
        'lbl_L
        '
        Me.lbl_L.BackColor = System.Drawing.Color.Lime
        Me.lbl_L.Location = New System.Drawing.Point(1721, 213)
        Me.lbl_L.Name = "lbl_L"
        Me.lbl_L.Size = New System.Drawing.Size(17, 16)
        Me.lbl_L.TabIndex = 356
        Me.lbl_L.Visible = False
        '
        'lbl_R
        '
        Me.lbl_R.BackColor = System.Drawing.Color.Lime
        Me.lbl_R.Location = New System.Drawing.Point(1767, 213)
        Me.lbl_R.Name = "lbl_R"
        Me.lbl_R.Size = New System.Drawing.Size(17, 16)
        Me.lbl_R.TabIndex = 355
        Me.lbl_R.Visible = False
        '
        'lbl_DN
        '
        Me.lbl_DN.BackColor = System.Drawing.Color.Lime
        Me.lbl_DN.Location = New System.Drawing.Point(1744, 236)
        Me.lbl_DN.Name = "lbl_DN"
        Me.lbl_DN.Size = New System.Drawing.Size(17, 16)
        Me.lbl_DN.TabIndex = 354
        Me.lbl_DN.Visible = False
        '
        'lbl_UP
        '
        Me.lbl_UP.BackColor = System.Drawing.Color.Lime
        Me.lbl_UP.Location = New System.Drawing.Point(1744, 188)
        Me.lbl_UP.Name = "lbl_UP"
        Me.lbl_UP.Size = New System.Drawing.Size(17, 16)
        Me.lbl_UP.TabIndex = 353
        Me.lbl_UP.Visible = False
        '
        'lbl_MIRROR_ADJUSTMENT
        '
        Me.lbl_MIRROR_ADJUSTMENT.BackColor = System.Drawing.Color.White
        Me.lbl_MIRROR_ADJUSTMENT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_MIRROR_ADJUSTMENT.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_MIRROR_ADJUSTMENT.ForeColor = System.Drawing.Color.Black
        Me.lbl_MIRROR_ADJUSTMENT.Location = New System.Drawing.Point(1514, 220)
        Me.lbl_MIRROR_ADJUSTMENT.Name = "lbl_MIRROR_ADJUSTMENT"
        Me.lbl_MIRROR_ADJUSTMENT.Size = New System.Drawing.Size(94, 25)
        Me.lbl_MIRROR_ADJUSTMENT.TabIndex = 352
        Me.lbl_MIRROR_ADJUSTMENT.Text = "0000"
        Me.lbl_MIRROR_ADJUSTMENT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_WL_REAR_FRONT_PASSENGER
        '
        Me.lbl_WL_REAR_FRONT_PASSENGER.BackColor = System.Drawing.Color.White
        Me.lbl_WL_REAR_FRONT_PASSENGER.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_WL_REAR_FRONT_PASSENGER.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_WL_REAR_FRONT_PASSENGER.ForeColor = System.Drawing.Color.Black
        Me.lbl_WL_REAR_FRONT_PASSENGER.Location = New System.Drawing.Point(1514, 189)
        Me.lbl_WL_REAR_FRONT_PASSENGER.Name = "lbl_WL_REAR_FRONT_PASSENGER"
        Me.lbl_WL_REAR_FRONT_PASSENGER.Size = New System.Drawing.Size(94, 25)
        Me.lbl_WL_REAR_FRONT_PASSENGER.TabIndex = 351
        Me.lbl_WL_REAR_FRONT_PASSENGER.Text = "0000"
        Me.lbl_WL_REAR_FRONT_PASSENGER.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_WL_REAR_RIGHT
        '
        Me.lbl_WL_REAR_RIGHT.BackColor = System.Drawing.Color.White
        Me.lbl_WL_REAR_RIGHT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_WL_REAR_RIGHT.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_WL_REAR_RIGHT.ForeColor = System.Drawing.Color.Black
        Me.lbl_WL_REAR_RIGHT.Location = New System.Drawing.Point(1514, 157)
        Me.lbl_WL_REAR_RIGHT.Name = "lbl_WL_REAR_RIGHT"
        Me.lbl_WL_REAR_RIGHT.Size = New System.Drawing.Size(94, 25)
        Me.lbl_WL_REAR_RIGHT.TabIndex = 350
        Me.lbl_WL_REAR_RIGHT.Text = "0000"
        Me.lbl_WL_REAR_RIGHT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_WL_REAR_LEFT
        '
        Me.lbl_WL_REAR_LEFT.BackColor = System.Drawing.Color.White
        Me.lbl_WL_REAR_LEFT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_WL_REAR_LEFT.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_WL_REAR_LEFT.ForeColor = System.Drawing.Color.Black
        Me.lbl_WL_REAR_LEFT.Location = New System.Drawing.Point(1514, 126)
        Me.lbl_WL_REAR_LEFT.Name = "lbl_WL_REAR_LEFT"
        Me.lbl_WL_REAR_LEFT.Size = New System.Drawing.Size(94, 25)
        Me.lbl_WL_REAR_LEFT.TabIndex = 349
        Me.lbl_WL_REAR_LEFT.Text = "0000"
        Me.lbl_WL_REAR_LEFT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Red
        Me.Button2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.Black
        Me.Button2.Location = New System.Drawing.Point(1271, 220)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(200, 55)
        Me.Button2.TabIndex = 348
        Me.Button2.Text = "Read OFF"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'btnRead_AIN
        '
        Me.btnRead_AIN.BackColor = System.Drawing.Color.Lime
        Me.btnRead_AIN.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRead_AIN.ForeColor = System.Drawing.Color.Black
        Me.btnRead_AIN.Location = New System.Drawing.Point(1271, 159)
        Me.btnRead_AIN.Name = "btnRead_AIN"
        Me.btnRead_AIN.Size = New System.Drawing.Size(200, 55)
        Me.btnRead_AIN.TabIndex = 347
        Me.btnRead_AIN.Text = "Read AIN"
        Me.btnRead_AIN.UseVisualStyleBackColor = False
        '
        'btnRead_Din
        '
        Me.btnRead_Din.BackColor = System.Drawing.Color.Lime
        Me.btnRead_Din.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRead_Din.ForeColor = System.Drawing.Color.Black
        Me.btnRead_Din.Location = New System.Drawing.Point(1271, 87)
        Me.btnRead_Din.Name = "btnRead_Din"
        Me.btnRead_Din.Size = New System.Drawing.Size(200, 55)
        Me.btnRead_Din.TabIndex = 346
        Me.btnRead_Din.Text = "Read DIN"
        Me.btnRead_Din.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(1644, 136)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(215, 543)
        Me.PictureBox1.TabIndex = 345
        Me.PictureBox1.TabStop = False
        '
        'btnWS03Diag
        '
        Me.btnWS03Diag.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnWS03Diag.ForeColor = System.Drawing.Color.Black
        Me.btnWS03Diag.Location = New System.Drawing.Point(1589, 23)
        Me.btnWS03Diag.Name = "btnWS03Diag"
        Me.btnWS03Diag.Size = New System.Drawing.Size(260, 53)
        Me.btnWS03Diag.TabIndex = 344
        Me.btnWS03Diag.Text = "Open Diag"
        Me.btnWS03Diag.UseVisualStyleBackColor = False
        '
        'txtWS03_Rx
        '
        Me.txtWS03_Rx.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWS03_Rx.Location = New System.Drawing.Point(1236, 463)
        Me.txtWS03_Rx.Multiline = True
        Me.txtWS03_Rx.Name = "txtWS03_Rx"
        Me.txtWS03_Rx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtWS03_Rx.Size = New System.Drawing.Size(402, 322)
        Me.txtWS03_Rx.TabIndex = 343
        '
        'txtWS03_Tx
        '
        Me.txtWS03_Tx.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWS03_Tx.Location = New System.Drawing.Point(1236, 291)
        Me.txtWS03_Tx.Multiline = True
        Me.txtWS03_Tx.Name = "txtWS03_Tx"
        Me.txtWS03_Tx.Size = New System.Drawing.Size(402, 167)
        Me.txtWS03_Tx.TabIndex = 342
        '
        'btnWS03Lin
        '
        Me.btnWS03Lin.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnWS03Lin.ForeColor = System.Drawing.Color.Black
        Me.btnWS03Lin.Location = New System.Drawing.Point(1271, 23)
        Me.btnWS03Lin.Name = "btnWS03Lin"
        Me.btnWS03Lin.Size = New System.Drawing.Size(287, 53)
        Me.btnWS03Lin.TabIndex = 341
        Me.btnWS03Lin.Text = "Open Lin"
        Me.btnWS03Lin.UseVisualStyleBackColor = False
        '
        'btnWS03_DO_15
        '
        Me.btnWS03_DO_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_15.Location = New System.Drawing.Point(343, 394)
        Me.btnWS03_DO_15.Name = "btnWS03_DO_15"
        Me.btnWS03_DO_15.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_15.TabIndex = 340
        Me.btnWS03_DO_15.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_14
        '
        Me.btnWS03_DO_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_14.Location = New System.Drawing.Point(343, 342)
        Me.btnWS03_DO_14.Name = "btnWS03_DO_14"
        Me.btnWS03_DO_14.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_14.TabIndex = 339
        Me.btnWS03_DO_14.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_13
        '
        Me.btnWS03_DO_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_13.Location = New System.Drawing.Point(344, 291)
        Me.btnWS03_DO_13.Name = "btnWS03_DO_13"
        Me.btnWS03_DO_13.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_13.TabIndex = 338
        Me.btnWS03_DO_13.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_12
        '
        Me.btnWS03_DO_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_12.Location = New System.Drawing.Point(343, 239)
        Me.btnWS03_DO_12.Name = "btnWS03_DO_12"
        Me.btnWS03_DO_12.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_12.TabIndex = 337
        Me.btnWS03_DO_12.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_11
        '
        Me.btnWS03_DO_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_11.Location = New System.Drawing.Point(343, 187)
        Me.btnWS03_DO_11.Name = "btnWS03_DO_11"
        Me.btnWS03_DO_11.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_11.TabIndex = 336
        Me.btnWS03_DO_11.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_10
        '
        Me.btnWS03_DO_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_10.Location = New System.Drawing.Point(343, 136)
        Me.btnWS03_DO_10.Name = "btnWS03_DO_10"
        Me.btnWS03_DO_10.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_10.TabIndex = 335
        Me.btnWS03_DO_10.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_9
        '
        Me.btnWS03_DO_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_9.Location = New System.Drawing.Point(343, 87)
        Me.btnWS03_DO_9.Name = "btnWS03_DO_9"
        Me.btnWS03_DO_9.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_9.TabIndex = 334
        Me.btnWS03_DO_9.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_8
        '
        Me.btnWS03_DO_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_8.Location = New System.Drawing.Point(343, 35)
        Me.btnWS03_DO_8.Name = "btnWS03_DO_8"
        Me.btnWS03_DO_8.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_8.TabIndex = 333
        Me.btnWS03_DO_8.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.dgvWS03AnalogInputs)
        Me.GroupBox3.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox3.Location = New System.Drawing.Point(649, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(581, 878)
        Me.GroupBox3.TabIndex = 106
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Analog inputs"
        '
        'dgvWS03AnalogInputs
        '
        Me.dgvWS03AnalogInputs.AllowUserToAddRows = False
        Me.dgvWS03AnalogInputs.AllowUserToDeleteRows = False
        Me.dgvWS03AnalogInputs.AllowUserToResizeColumns = False
        Me.dgvWS03AnalogInputs.AllowUserToResizeRows = False
        Me.dgvWS03AnalogInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvWS03AnalogInputs.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvWS03AnalogInputs.Location = New System.Drawing.Point(6, 23)
        Me.dgvWS03AnalogInputs.Name = "dgvWS03AnalogInputs"
        Me.dgvWS03AnalogInputs.ReadOnly = True
        Me.dgvWS03AnalogInputs.Size = New System.Drawing.Size(569, 641)
        Me.dgvWS03AnalogInputs.TabIndex = 92
        '
        'btnWS03_DO_7
        '
        Me.btnWS03_DO_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_7.Location = New System.Drawing.Point(15, 394)
        Me.btnWS03_DO_7.Name = "btnWS03_DO_7"
        Me.btnWS03_DO_7.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_7.TabIndex = 130
        Me.btnWS03_DO_7.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_6
        '
        Me.btnWS03_DO_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_6.Location = New System.Drawing.Point(15, 342)
        Me.btnWS03_DO_6.Name = "btnWS03_DO_6"
        Me.btnWS03_DO_6.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_6.TabIndex = 129
        Me.btnWS03_DO_6.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_5
        '
        Me.btnWS03_DO_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_5.Location = New System.Drawing.Point(16, 291)
        Me.btnWS03_DO_5.Name = "btnWS03_DO_5"
        Me.btnWS03_DO_5.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_5.TabIndex = 128
        Me.btnWS03_DO_5.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_4
        '
        Me.btnWS03_DO_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_4.Location = New System.Drawing.Point(15, 239)
        Me.btnWS03_DO_4.Name = "btnWS03_DO_4"
        Me.btnWS03_DO_4.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_4.TabIndex = 127
        Me.btnWS03_DO_4.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_3
        '
        Me.btnWS03_DO_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_3.Location = New System.Drawing.Point(15, 187)
        Me.btnWS03_DO_3.Name = "btnWS03_DO_3"
        Me.btnWS03_DO_3.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_3.TabIndex = 126
        Me.btnWS03_DO_3.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_2
        '
        Me.btnWS03_DO_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_2.Location = New System.Drawing.Point(15, 136)
        Me.btnWS03_DO_2.Name = "btnWS03_DO_2"
        Me.btnWS03_DO_2.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_2.TabIndex = 125
        Me.btnWS03_DO_2.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_1
        '
        Me.btnWS03_DO_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_1.Location = New System.Drawing.Point(15, 87)
        Me.btnWS03_DO_1.Name = "btnWS03_DO_1"
        Me.btnWS03_DO_1.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_1.TabIndex = 124
        Me.btnWS03_DO_1.UseVisualStyleBackColor = True
        '
        'btnWS03_DO_0
        '
        Me.btnWS03_DO_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS03_DO_0.Location = New System.Drawing.Point(15, 35)
        Me.btnWS03_DO_0.Name = "btnWS03_DO_0"
        Me.btnWS03_DO_0.Size = New System.Drawing.Size(300, 46)
        Me.btnWS03_DO_0.TabIndex = 123
        Me.btnWS03_DO_0.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage4.Controls.Add(Me.GroupBox4)
        Me.TabPage4.Location = New System.Drawing.Point(4, 54)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(1888, 899)
        Me.TabPage4.TabIndex = 4
        Me.TabPage4.Text = "WS04 Haptic #2"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_15)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_14)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_13)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_12)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_11)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_10)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_9)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_8)
        Me.GroupBox4.Controls.Add(Me.TextBox3)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_7)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_6)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_5)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_4)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_3)
        Me.GroupBox4.Controls.Add(Me.GroupBox6)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_2)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_1)
        Me.GroupBox4.Controls.Add(Me.btnWS04_DO_0)
        Me.GroupBox4.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox4.Location = New System.Drawing.Point(15, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(1859, 884)
        Me.GroupBox4.TabIndex = 106
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Digital outputs"
        '
        'btnWS04_DO_15
        '
        Me.btnWS04_DO_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_15.Location = New System.Drawing.Point(323, 385)
        Me.btnWS04_DO_15.Name = "btnWS04_DO_15"
        Me.btnWS04_DO_15.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_15.TabIndex = 346
        Me.btnWS04_DO_15.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_14
        '
        Me.btnWS04_DO_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_14.Location = New System.Drawing.Point(323, 333)
        Me.btnWS04_DO_14.Name = "btnWS04_DO_14"
        Me.btnWS04_DO_14.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_14.TabIndex = 345
        Me.btnWS04_DO_14.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_13
        '
        Me.btnWS04_DO_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_13.Location = New System.Drawing.Point(323, 282)
        Me.btnWS04_DO_13.Name = "btnWS04_DO_13"
        Me.btnWS04_DO_13.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_13.TabIndex = 344
        Me.btnWS04_DO_13.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_12
        '
        Me.btnWS04_DO_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_12.Location = New System.Drawing.Point(323, 230)
        Me.btnWS04_DO_12.Name = "btnWS04_DO_12"
        Me.btnWS04_DO_12.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_12.TabIndex = 343
        Me.btnWS04_DO_12.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_11
        '
        Me.btnWS04_DO_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_11.Location = New System.Drawing.Point(323, 178)
        Me.btnWS04_DO_11.Name = "btnWS04_DO_11"
        Me.btnWS04_DO_11.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_11.TabIndex = 342
        Me.btnWS04_DO_11.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_10
        '
        Me.btnWS04_DO_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_10.Location = New System.Drawing.Point(323, 126)
        Me.btnWS04_DO_10.Name = "btnWS04_DO_10"
        Me.btnWS04_DO_10.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_10.TabIndex = 341
        Me.btnWS04_DO_10.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_9
        '
        Me.btnWS04_DO_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_9.Location = New System.Drawing.Point(323, 75)
        Me.btnWS04_DO_9.Name = "btnWS04_DO_9"
        Me.btnWS04_DO_9.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_9.TabIndex = 340
        Me.btnWS04_DO_9.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_8
        '
        Me.btnWS04_DO_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_8.Location = New System.Drawing.Point(323, 23)
        Me.btnWS04_DO_8.Name = "btnWS04_DO_8"
        Me.btnWS04_DO_8.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_8.TabIndex = 339
        Me.btnWS04_DO_8.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(1264, 12)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox3.Size = New System.Drawing.Size(595, 628)
        Me.TextBox3.TabIndex = 338
        '
        'btnWS04_DO_7
        '
        Me.btnWS04_DO_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_7.Location = New System.Drawing.Point(6, 385)
        Me.btnWS04_DO_7.Name = "btnWS04_DO_7"
        Me.btnWS04_DO_7.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_7.TabIndex = 187
        Me.btnWS04_DO_7.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_6
        '
        Me.btnWS04_DO_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_6.Location = New System.Drawing.Point(6, 333)
        Me.btnWS04_DO_6.Name = "btnWS04_DO_6"
        Me.btnWS04_DO_6.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_6.TabIndex = 186
        Me.btnWS04_DO_6.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_5
        '
        Me.btnWS04_DO_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_5.Location = New System.Drawing.Point(6, 282)
        Me.btnWS04_DO_5.Name = "btnWS04_DO_5"
        Me.btnWS04_DO_5.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_5.TabIndex = 185
        Me.btnWS04_DO_5.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_4
        '
        Me.btnWS04_DO_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_4.Location = New System.Drawing.Point(6, 230)
        Me.btnWS04_DO_4.Name = "btnWS04_DO_4"
        Me.btnWS04_DO_4.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_4.TabIndex = 184
        Me.btnWS04_DO_4.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_3
        '
        Me.btnWS04_DO_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_3.Location = New System.Drawing.Point(6, 178)
        Me.btnWS04_DO_3.Name = "btnWS04_DO_3"
        Me.btnWS04_DO_3.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_3.TabIndex = 183
        Me.btnWS04_DO_3.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.dgvWS04AnalogInputs)
        Me.GroupBox6.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox6.Location = New System.Drawing.Point(638, 0)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(581, 785)
        Me.GroupBox6.TabIndex = 106
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Analog inputs"
        '
        'dgvWS04AnalogInputs
        '
        Me.dgvWS04AnalogInputs.AllowUserToAddRows = False
        Me.dgvWS04AnalogInputs.AllowUserToDeleteRows = False
        Me.dgvWS04AnalogInputs.AllowUserToResizeColumns = False
        Me.dgvWS04AnalogInputs.AllowUserToResizeRows = False
        Me.dgvWS04AnalogInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvWS04AnalogInputs.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvWS04AnalogInputs.Location = New System.Drawing.Point(6, 23)
        Me.dgvWS04AnalogInputs.Name = "dgvWS04AnalogInputs"
        Me.dgvWS04AnalogInputs.ReadOnly = True
        Me.dgvWS04AnalogInputs.Size = New System.Drawing.Size(569, 641)
        Me.dgvWS04AnalogInputs.TabIndex = 92
        '
        'btnWS04_DO_2
        '
        Me.btnWS04_DO_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_2.Location = New System.Drawing.Point(6, 126)
        Me.btnWS04_DO_2.Name = "btnWS04_DO_2"
        Me.btnWS04_DO_2.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_2.TabIndex = 125
        Me.btnWS04_DO_2.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_1
        '
        Me.btnWS04_DO_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_1.Location = New System.Drawing.Point(6, 75)
        Me.btnWS04_DO_1.Name = "btnWS04_DO_1"
        Me.btnWS04_DO_1.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_1.TabIndex = 124
        Me.btnWS04_DO_1.UseVisualStyleBackColor = True
        '
        'btnWS04_DO_0
        '
        Me.btnWS04_DO_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS04_DO_0.Location = New System.Drawing.Point(6, 23)
        Me.btnWS04_DO_0.Name = "btnWS04_DO_0"
        Me.btnWS04_DO_0.Size = New System.Drawing.Size(300, 46)
        Me.btnWS04_DO_0.TabIndex = 123
        Me.btnWS04_DO_0.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage5.Controls.Add(Me.GroupBox2)
        Me.TabPage5.Location = New System.Drawing.Point(4, 54)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(1888, 899)
        Me.TabPage5.TabIndex = 5
        Me.TabPage5.Text = "WS05 Haptic #3"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TextBox4)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_15)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_14)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_13)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_12)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_11)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_10)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_9)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_8)
        Me.GroupBox2.Controls.Add(Me.GroupBox5)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_7)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_6)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_5)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_4)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_3)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_2)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_1)
        Me.GroupBox2.Controls.Add(Me.btnWS05_DO_0)
        Me.GroupBox2.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox2.Location = New System.Drawing.Point(15, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1859, 880)
        Me.GroupBox2.TabIndex = 106
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Digital outputs"
        '
        'TextBox4
        '
        Me.TextBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(1236, 15)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox4.Size = New System.Drawing.Size(595, 628)
        Me.TextBox4.TabIndex = 341
        '
        'btnWS05_DO_15
        '
        Me.btnWS05_DO_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_15.Location = New System.Drawing.Point(343, 394)
        Me.btnWS05_DO_15.Name = "btnWS05_DO_15"
        Me.btnWS05_DO_15.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_15.TabIndex = 340
        Me.btnWS05_DO_15.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_14
        '
        Me.btnWS05_DO_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_14.Location = New System.Drawing.Point(343, 342)
        Me.btnWS05_DO_14.Name = "btnWS05_DO_14"
        Me.btnWS05_DO_14.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_14.TabIndex = 339
        Me.btnWS05_DO_14.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_13
        '
        Me.btnWS05_DO_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_13.Location = New System.Drawing.Point(344, 291)
        Me.btnWS05_DO_13.Name = "btnWS05_DO_13"
        Me.btnWS05_DO_13.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_13.TabIndex = 338
        Me.btnWS05_DO_13.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_12
        '
        Me.btnWS05_DO_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_12.Location = New System.Drawing.Point(343, 239)
        Me.btnWS05_DO_12.Name = "btnWS05_DO_12"
        Me.btnWS05_DO_12.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_12.TabIndex = 337
        Me.btnWS05_DO_12.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_11
        '
        Me.btnWS05_DO_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_11.Location = New System.Drawing.Point(343, 187)
        Me.btnWS05_DO_11.Name = "btnWS05_DO_11"
        Me.btnWS05_DO_11.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_11.TabIndex = 336
        Me.btnWS05_DO_11.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_10
        '
        Me.btnWS05_DO_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_10.Location = New System.Drawing.Point(343, 136)
        Me.btnWS05_DO_10.Name = "btnWS05_DO_10"
        Me.btnWS05_DO_10.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_10.TabIndex = 335
        Me.btnWS05_DO_10.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_9
        '
        Me.btnWS05_DO_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_9.Location = New System.Drawing.Point(343, 87)
        Me.btnWS05_DO_9.Name = "btnWS05_DO_9"
        Me.btnWS05_DO_9.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_9.TabIndex = 334
        Me.btnWS05_DO_9.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_8
        '
        Me.btnWS05_DO_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_8.Location = New System.Drawing.Point(343, 35)
        Me.btnWS05_DO_8.Name = "btnWS05_DO_8"
        Me.btnWS05_DO_8.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_8.TabIndex = 333
        Me.btnWS05_DO_8.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.dgvWS05AnalogInputs)
        Me.GroupBox5.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox5.Location = New System.Drawing.Point(649, 0)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(581, 874)
        Me.GroupBox5.TabIndex = 106
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Analog inputs"
        '
        'dgvWS05AnalogInputs
        '
        Me.dgvWS05AnalogInputs.AllowUserToAddRows = False
        Me.dgvWS05AnalogInputs.AllowUserToDeleteRows = False
        Me.dgvWS05AnalogInputs.AllowUserToResizeColumns = False
        Me.dgvWS05AnalogInputs.AllowUserToResizeRows = False
        Me.dgvWS05AnalogInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvWS05AnalogInputs.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgvWS05AnalogInputs.Location = New System.Drawing.Point(6, 23)
        Me.dgvWS05AnalogInputs.Name = "dgvWS05AnalogInputs"
        Me.dgvWS05AnalogInputs.ReadOnly = True
        Me.dgvWS05AnalogInputs.Size = New System.Drawing.Size(569, 641)
        Me.dgvWS05AnalogInputs.TabIndex = 92
        '
        'btnWS05_DO_7
        '
        Me.btnWS05_DO_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_7.Location = New System.Drawing.Point(15, 394)
        Me.btnWS05_DO_7.Name = "btnWS05_DO_7"
        Me.btnWS05_DO_7.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_7.TabIndex = 130
        Me.btnWS05_DO_7.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_6
        '
        Me.btnWS05_DO_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_6.Location = New System.Drawing.Point(15, 342)
        Me.btnWS05_DO_6.Name = "btnWS05_DO_6"
        Me.btnWS05_DO_6.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_6.TabIndex = 129
        Me.btnWS05_DO_6.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_5
        '
        Me.btnWS05_DO_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_5.Location = New System.Drawing.Point(16, 291)
        Me.btnWS05_DO_5.Name = "btnWS05_DO_5"
        Me.btnWS05_DO_5.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_5.TabIndex = 128
        Me.btnWS05_DO_5.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_4
        '
        Me.btnWS05_DO_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_4.Location = New System.Drawing.Point(15, 239)
        Me.btnWS05_DO_4.Name = "btnWS05_DO_4"
        Me.btnWS05_DO_4.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_4.TabIndex = 127
        Me.btnWS05_DO_4.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_3
        '
        Me.btnWS05_DO_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_3.Location = New System.Drawing.Point(15, 187)
        Me.btnWS05_DO_3.Name = "btnWS05_DO_3"
        Me.btnWS05_DO_3.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_3.TabIndex = 126
        Me.btnWS05_DO_3.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_2
        '
        Me.btnWS05_DO_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_2.Location = New System.Drawing.Point(15, 136)
        Me.btnWS05_DO_2.Name = "btnWS05_DO_2"
        Me.btnWS05_DO_2.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_2.TabIndex = 125
        Me.btnWS05_DO_2.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_1
        '
        Me.btnWS05_DO_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_1.Location = New System.Drawing.Point(15, 87)
        Me.btnWS05_DO_1.Name = "btnWS05_DO_1"
        Me.btnWS05_DO_1.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_1.TabIndex = 124
        Me.btnWS05_DO_1.UseVisualStyleBackColor = True
        '
        'btnWS05_DO_0
        '
        Me.btnWS05_DO_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnWS05_DO_0.Location = New System.Drawing.Point(15, 35)
        Me.btnWS05_DO_0.Name = "btnWS05_DO_0"
        Me.btnWS05_DO_0.Size = New System.Drawing.Size(300, 46)
        Me.btnWS05_DO_0.TabIndex = 123
        Me.btnWS05_DO_0.UseVisualStyleBackColor = True
        '
        'pbLogoValeo
        '
        Me.pbLogoValeo.Image = CType(resources.GetObject("pbLogoValeo.Image"), System.Drawing.Image)
        Me.pbLogoValeo.Location = New System.Drawing.Point(0, 0)
        Me.pbLogoValeo.Name = "pbLogoValeo"
        Me.pbLogoValeo.Size = New System.Drawing.Size(200, 92)
        Me.pbLogoValeo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbLogoValeo.TabIndex = 60
        Me.pbLogoValeo.TabStop = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 20
        '
        'tmrLoop
        '
        Me.tmrLoop.Interval = 20
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 10
        '
        'CustomerLogo
        '
        Me.CustomerLogo.Image = CType(resources.GetObject("CustomerLogo.Image"), System.Drawing.Image)
        Me.CustomerLogo.Location = New System.Drawing.Point(1678, 0)
        Me.CustomerLogo.Name = "CustomerLogo"
        Me.CustomerLogo.Size = New System.Drawing.Size(242, 92)
        Me.CustomerLogo.TabIndex = 232
        Me.CustomerLogo.TabStop = False
        '
        'frmMaintenance_EOL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1920, 1080)
        Me.Controls.Add(Me.CustomerLogo)
        Me.Controls.Add(Me.ssStatusStrip)
        Me.Controls.Add(Me.lblCountry)
        Me.Controls.Add(Me.lblCity)
        Me.Controls.Add(Me.lblECB)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Controls.Add(Me.lblSoftwareTitle)
        Me.Controls.Add(Me.pbLogoValeo)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmMaintenance_EOL"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmMaintenance"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.gbstFWDigitalOutputs.ResumeLayout(False)
        Me.gbstFWDigitalOutputs.PerformLayout()
        Me.gbstFWAnalogInputs.ResumeLayout(False)
        CType(Me.dgvWS02AnalogInputs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.dgvWS03AnalogInputs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage4.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        CType(Me.dgvWS04AnalogInputs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage5.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        CType(Me.dgvWS05AnalogInputs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CustomerLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCountry As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblECB As System.Windows.Forms.Label
    Friend WithEvents lblFormTitle As System.Windows.Forms.Label
    Friend WithEvents lblSoftwareTitle As System.Windows.Forms.Label
    Friend WithEvents pbLogoValeo As System.Windows.Forms.PictureBox
    Friend WithEvents ssStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents tmrMonitor As System.Windows.Forms.Timer
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents gbstFWDigitalOutputs As System.Windows.Forms.GroupBox
    Friend WithEvents gbstFWAnalogInputs As System.Windows.Forms.GroupBox
    Friend WithEvents dgvWS02AnalogInputs As System.Windows.Forms.DataGridView
    Friend WithEvents btnWS02_DO_7 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_6 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_5 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_4 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_3 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_2 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_1 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_0 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents tmrLoop As System.Windows.Forms.Timer
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents btnWS02_DO_31 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_30 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_29 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_28 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_27 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_26 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_25 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_24 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_15 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_14 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_13 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_12 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_11 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_10 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_9 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_8 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_23 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_22 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_21 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_20 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_19 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_18 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_17 As System.Windows.Forms.Button
    Friend WithEvents btnWS02_DO_16 As System.Windows.Forms.Button
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnWS03_DO_15 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_14 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_13 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_12 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_11 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_10 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_9 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_8 As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents dgvWS03AnalogInputs As System.Windows.Forms.DataGridView
    Friend WithEvents btnWS03_DO_7 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_6 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_5 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_4 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_3 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_2 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_1 As System.Windows.Forms.Button
    Friend WithEvents btnWS03_DO_0 As System.Windows.Forms.Button
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btnWS04_DO_15 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_14 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_13 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_12 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_11 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_10 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_9 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_8 As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents btnWS04_DO_7 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_6 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_5 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_4 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_3 As System.Windows.Forms.Button
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents dgvWS04AnalogInputs As System.Windows.Forms.DataGridView
    Friend WithEvents btnWS04_DO_2 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_1 As System.Windows.Forms.Button
    Friend WithEvents btnWS04_DO_0 As System.Windows.Forms.Button
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents btnWS05_DO_15 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_14 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_13 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_12 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_11 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_10 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_9 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_8 As System.Windows.Forms.Button
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents dgvWS05AnalogInputs As System.Windows.Forms.DataGridView
    Friend WithEvents btnWS05_DO_7 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_6 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_5 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_4 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_3 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_2 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_1 As System.Windows.Forms.Button
    Friend WithEvents btnWS05_DO_0 As System.Windows.Forms.Button
    Friend WithEvents btnLinWS02 As System.Windows.Forms.Button
    Friend WithEvents btn_Bck_Off As System.Windows.Forms.Button
    Friend WithEvents btn_Bck_On As System.Windows.Forms.Button
    Friend WithEvents btn_TTL_Off As System.Windows.Forms.Button
    Friend WithEvents btn_TTL_On As System.Windows.Forms.Button
    Friend WithEvents btnAll_Off As System.Windows.Forms.Button
    Friend WithEvents bntAll_On As System.Windows.Forms.Button
    Friend WithEvents txtWS02_Rx As System.Windows.Forms.TextBox
    Friend WithEvents txtWS02_Tx As System.Windows.Forms.TextBox
    Friend WithEvents btnOpenDiag As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnWS03Diag As System.Windows.Forms.Button
    Friend WithEvents txtWS03_Rx As System.Windows.Forms.TextBox
    Friend WithEvents txtWS03_Tx As System.Windows.Forms.TextBox
    Friend WithEvents btnWS03Lin As System.Windows.Forms.Button
    Friend WithEvents btnRead_AIN As System.Windows.Forms.Button
    Friend WithEvents btnRead_Din As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents lbl_MIRROR_ADJUSTMENT As System.Windows.Forms.Label
    Friend WithEvents lbl_WL_REAR_FRONT_PASSENGER As System.Windows.Forms.Label
    Friend WithEvents lbl_WL_REAR_RIGHT As System.Windows.Forms.Label
    Friend WithEvents lbl_WL_REAR_LEFT As System.Windows.Forms.Label
    Friend WithEvents lbl_Child As System.Windows.Forms.Label
    Friend WithEvents lbl_SR As System.Windows.Forms.Label
    Friend WithEvents lbl_Fold As System.Windows.Forms.Label
    Friend WithEvents lbl_SL As System.Windows.Forms.Label
    Friend WithEvents lbl_L As System.Windows.Forms.Label
    Friend WithEvents lbl_R As System.Windows.Forms.Label
    Friend WithEvents lbl_DN As System.Windows.Forms.Label
    Friend WithEvents lbl_UP As System.Windows.Forms.Label
    Friend WithEvents lbl_RR_Pull_A As System.Windows.Forms.Label
    Friend WithEvents lbl_RR_Pull_M As System.Windows.Forms.Label
    Friend WithEvents lbl_RL_Pull_A As System.Windows.Forms.Label
    Friend WithEvents lbl_RL_Pull_M As System.Windows.Forms.Label
    Friend WithEvents lbl_RR_Push_A As System.Windows.Forms.Label
    Friend WithEvents lbl_RR_Push_M As System.Windows.Forms.Label
    Friend WithEvents lbl_RL_Push_A As System.Windows.Forms.Label
    Friend WithEvents lbl_RL_Push_M As System.Windows.Forms.Label
    Friend WithEvents lbl_FR_Pull_A As System.Windows.Forms.Label
    Friend WithEvents lbl_FR_Pull_M As System.Windows.Forms.Label
    Friend WithEvents lbl_FL_Pull_A As System.Windows.Forms.Label
    Friend WithEvents lbl_FL_Pull_M As System.Windows.Forms.Label
    Friend WithEvents lbl_FR_Push_A As System.Windows.Forms.Label
    Friend WithEvents lbl_FR_Push_M As System.Windows.Forms.Label
    Friend WithEvents lbl_Fl_Push_A As System.Windows.Forms.Label
    Friend WithEvents lbl_Fl_Push_M As System.Windows.Forms.Label
    Friend WithEvents CustomerLogo As System.Windows.Forms.PictureBox
End Class
