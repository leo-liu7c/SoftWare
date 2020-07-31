<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProduction
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProduction))
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.lblSoftwareTitle = New System.Windows.Forms.Label()
        Me.tmrMonitor = New System.Windows.Forms.Timer(Me.components)
        Me.lblECB = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.pbLogoValeo = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvWS05SingleTestResults = New System.Windows.Forms.DataGridView()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblWS05Phase = New System.Windows.Forms.Label()
        Me.lblWS05Reference = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblWS05TestMode = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblWS05TPCycle = New System.Windows.Forms.Label()
        Me.btnWS05StartTest = New System.Windows.Forms.Button()
        Me.btnWS05AbortTest = New System.Windows.Forms.Button()
        Me.lblWS05TestResult = New System.Windows.Forms.Label()
        Me.btnWS05Step = New System.Windows.Forms.Button()
        Me.btnWS05StepMode = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lblWS05StartStep = New System.Windows.Forms.Label()
        Me.lblWS05TestOk = New System.Windows.Forms.Label()
        Me.lblWS05StepInProgress = New System.Windows.Forms.Label()
        Me.lblWS05TestEnable = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BtnWS02StartPLC = New System.Windows.Forms.Button()
        Me.lblWS02Reference = New System.Windows.Forms.Label()
        Me.lblWS02TestMode = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblWS02TPCycle = New System.Windows.Forms.Label()
        Me.btnWS02Step = New System.Windows.Forms.Button()
        Me.btnWS02StepMode = New System.Windows.Forms.Button()
        Me.btnWS02AbortTest = New System.Windows.Forms.Button()
        Me.btnWS02StartTest = New System.Windows.Forms.Button()
        Me.lblSt60IOMonitor = New System.Windows.Forms.Label()
        Me.lblWS02StartStep = New System.Windows.Forms.Label()
        Me.lblWS02TestOkA = New System.Windows.Forms.Label()
        Me.lblWS02StepInProgress = New System.Windows.Forms.Label()
        Me.lblWS02TestEnable = New System.Windows.Forms.Label()
        Me.lblWS02TestResult = New System.Windows.Forms.Label()
        Me.lblstFWTestResultALabel = New System.Windows.Forms.Label()
        Me.dgvWS02SingleTestResults = New System.Windows.Forms.DataGridView()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.lblWS02Phase = New System.Windows.Forms.Label()
        Me.lblstFWReferenceLabel = New System.Windows.Forms.Label()
        Me.lblstFWTestModeLabel = New System.Windows.Forms.Label()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.BtnWS03StartPLC = New System.Windows.Forms.Button()
        Me.btnWS03AbortTest = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.lblWS03TPCycle = New System.Windows.Forms.Label()
        Me.BTShowDUTStats = New System.Windows.Forms.Button()
        Me.btnWS03StartTest = New System.Windows.Forms.Button()
        Me.lblWS03TestResult = New System.Windows.Forms.Label()
        Me.btnWS03Step = New System.Windows.Forms.Button()
        Me.btnWS03StepMode = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblWS03StartStep = New System.Windows.Forms.Label()
        Me.lblWS03TestOk = New System.Windows.Forms.Label()
        Me.lblWS03StepInProgress = New System.Windows.Forms.Label()
        Me.lblWS03TestEnable = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblWS03Phase = New System.Windows.Forms.Label()
        Me.lblWS03Reference = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblWS03TestMode = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dgvWS03SingleTestResults = New System.Windows.Forms.DataGridView()
        Me.ssStatusBar = New System.Windows.Forms.StatusStrip()
        Me.tmrLoop = New System.Windows.Forms.Timer(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.dgvWS04SingleTestResults = New System.Windows.Forms.DataGridView()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblWS04Phase = New System.Windows.Forms.Label()
        Me.lblWS04Reference = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblWS04TestMode = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.lblWS04TPCycle = New System.Windows.Forms.Label()
        Me.btnWS04StartTest = New System.Windows.Forms.Button()
        Me.btnWS04AbortTest = New System.Windows.Forms.Button()
        Me.lblWS04TestResult = New System.Windows.Forms.Label()
        Me.btnWS04Step = New System.Windows.Forms.Button()
        Me.btnWS04StepMode = New System.Windows.Forms.Button()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.lblWS04StartStep = New System.Windows.Forms.Label()
        Me.lblWS04TestOk = New System.Windows.Forms.Label()
        Me.lblWS04StepInProgress = New System.Windows.Forms.Label()
        Me.lblWS04TestEnable = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.BtnWS04StartPLC = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.BtnWS05StartPLC = New System.Windows.Forms.Button()
        Me.CustomerLogo = New System.Windows.Forms.PictureBox()
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS05SingleTestResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvWS02SingleTestResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvWS03SingleTestResults, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS04SingleTestResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.CustomerLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.Color.White
        Me.lblCountry.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.Color.Blue
        Me.lblCountry.Location = New System.Drawing.Point(1720, 37)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.Size = New System.Drawing.Size(200, 28)
        Me.lblCountry.TabIndex = 40
        Me.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCity
        '
        Me.lblCity.BackColor = System.Drawing.Color.White
        Me.lblCity.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCity.ForeColor = System.Drawing.Color.Blue
        Me.lblCity.Location = New System.Drawing.Point(1714, 61)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(207, 42)
        Me.lblCity.TabIndex = 39
        Me.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblFormTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormTitle.Location = New System.Drawing.Point(200, 46)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1466, 57)
        Me.lblFormTitle.TabIndex = 37
        Me.lblFormTitle.Text = "Production"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSoftwareTitle
        '
        Me.lblSoftwareTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblSoftwareTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSoftwareTitle.Location = New System.Drawing.Point(200, 0)
        Me.lblSoftwareTitle.Name = "lblSoftwareTitle"
        Me.lblSoftwareTitle.Size = New System.Drawing.Size(1464, 46)
        Me.lblSoftwareTitle.TabIndex = 36
        Me.lblSoftwareTitle.Text = "EOL Test Bench RSA SQUARE SUV WILI"
        Me.lblSoftwareTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tmrMonitor
        '
        '
        'lblECB
        '
        Me.lblECB.BackColor = System.Drawing.Color.White
        Me.lblECB.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblECB.ForeColor = System.Drawing.Color.Blue
        Me.lblECB.Location = New System.Drawing.Point(1661, 0)
        Me.lblECB.Name = "lblECB"
        Me.lblECB.Size = New System.Drawing.Size(259, 103)
        Me.lblECB.TabIndex = 38
        Me.lblECB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(162, 868)
        Me.Label12.Margin = New System.Windows.Forms.Padding(3)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(84, 18)
        Me.Label12.TabIndex = 150
        Me.Label12.Text = "Test result"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pbLogoValeo
        '
        Me.pbLogoValeo.Image = CType(resources.GetObject("pbLogoValeo.Image"), System.Drawing.Image)
        Me.pbLogoValeo.Location = New System.Drawing.Point(3, 0)
        Me.pbLogoValeo.Name = "pbLogoValeo"
        Me.pbLogoValeo.Size = New System.Drawing.Size(200, 103)
        Me.pbLogoValeo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbLogoValeo.TabIndex = 35
        Me.pbLogoValeo.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 816)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 24)
        Me.Label1.TabIndex = 177
        Me.Label1.Text = "Cycle Time"
        '
        'dgvWS05SingleTestResults
        '
        Me.dgvWS05SingleTestResults.AllowUserToAddRows = False
        Me.dgvWS05SingleTestResults.AllowUserToDeleteRows = False
        Me.dgvWS05SingleTestResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS05SingleTestResults.Location = New System.Drawing.Point(6, 134)
        Me.dgvWS05SingleTestResults.Name = "dgvWS05SingleTestResults"
        Me.dgvWS05SingleTestResults.ReadOnly = True
        Me.dgvWS05SingleTestResults.Size = New System.Drawing.Size(466, 54)
        Me.dgvWS05SingleTestResults.TabIndex = 186
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(179, 11)
        Me.Label4.Margin = New System.Windows.Forms.Padding(3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(119, 33)
        Me.Label4.TabIndex = 192
        Me.Label4.Text = "WS05"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS05Phase
        '
        Me.lblWS05Phase.BackColor = System.Drawing.Color.White
        Me.lblWS05Phase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS05Phase.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05Phase.Location = New System.Drawing.Point(6, 85)
        Me.lblWS05Phase.Name = "lblWS05Phase"
        Me.lblWS05Phase.Size = New System.Drawing.Size(466, 37)
        Me.lblWS05Phase.TabIndex = 191
        Me.lblWS05Phase.Text = "Phase A"
        Me.lblWS05Phase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS05Reference
        '
        Me.lblWS05Reference.BackColor = System.Drawing.Color.White
        Me.lblWS05Reference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS05Reference.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05Reference.Location = New System.Drawing.Point(254, 45)
        Me.lblWS05Reference.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS05Reference.Name = "lblWS05Reference"
        Me.lblWS05Reference.Size = New System.Drawing.Size(220, 37)
        Me.lblWS05Reference.TabIndex = 190
        Me.lblWS05Reference.Text = "Reference"
        Me.lblWS05Reference.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(320, 23)
        Me.Label10.Margin = New System.Windows.Forms.Padding(3)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(152, 18)
        Me.Label10.TabIndex = 189
        Me.Label10.Text = "Reference"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS05TestMode
        '
        Me.lblWS05TestMode.BackColor = System.Drawing.Color.White
        Me.lblWS05TestMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS05TestMode.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05TestMode.Location = New System.Drawing.Point(6, 45)
        Me.lblWS05TestMode.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS05TestMode.Name = "lblWS05TestMode"
        Me.lblWS05TestMode.Size = New System.Drawing.Size(220, 37)
        Me.lblWS05TestMode.TabIndex = 188
        Me.lblWS05TestMode.Text = "Test mode"
        Me.lblWS05TestMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(6, 24)
        Me.Label14.Margin = New System.Windows.Forms.Padding(3)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(140, 18)
        Me.Label14.TabIndex = 187
        Me.Label14.Text = "Test mode"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS05TPCycle
        '
        Me.lblWS05TPCycle.BackColor = System.Drawing.Color.White
        Me.lblWS05TPCycle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS05TPCycle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05TPCycle.Location = New System.Drawing.Point(128, 820)
        Me.lblWS05TPCycle.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS05TPCycle.Name = "lblWS05TPCycle"
        Me.lblWS05TPCycle.Size = New System.Drawing.Size(90, 25)
        Me.lblWS05TPCycle.TabIndex = 205
        Me.lblWS05TPCycle.Text = "0.00 s"
        Me.lblWS05TPCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnWS05StartTest
        '
        Me.btnWS05StartTest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05StartTest.Location = New System.Drawing.Point(6, 887)
        Me.btnWS05StartTest.Name = "btnWS05StartTest"
        Me.btnWS05StartTest.Size = New System.Drawing.Size(120, 37)
        Me.btnWS05StartTest.TabIndex = 198
        Me.btnWS05StartTest.Text = "Start test"
        Me.btnWS05StartTest.UseVisualStyleBackColor = True
        Me.btnWS05StartTest.Visible = False
        '
        'btnWS05AbortTest
        '
        Me.btnWS05AbortTest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05AbortTest.Location = New System.Drawing.Point(354, 887)
        Me.btnWS05AbortTest.Name = "btnWS05AbortTest"
        Me.btnWS05AbortTest.Size = New System.Drawing.Size(120, 37)
        Me.btnWS05AbortTest.TabIndex = 199
        Me.btnWS05AbortTest.Text = "Abort test"
        Me.btnWS05AbortTest.UseVisualStyleBackColor = True
        Me.btnWS05AbortTest.Visible = False
        '
        'lblWS05TestResult
        '
        Me.lblWS05TestResult.BackColor = System.Drawing.Color.White
        Me.lblWS05TestResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS05TestResult.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05TestResult.Location = New System.Drawing.Point(6, 888)
        Me.lblWS05TestResult.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS05TestResult.Name = "lblWS05TestResult"
        Me.lblWS05TestResult.Size = New System.Drawing.Size(466, 37)
        Me.lblWS05TestResult.TabIndex = 203
        Me.lblWS05TestResult.Text = "Test result"
        Me.lblWS05TestResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnWS05Step
        '
        Me.btnWS05Step.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05Step.Location = New System.Drawing.Point(126, 778)
        Me.btnWS05Step.Name = "btnWS05Step"
        Me.btnWS05Step.Size = New System.Drawing.Size(120, 37)
        Me.btnWS05Step.TabIndex = 201
        Me.btnWS05Step.Text = "Step"
        Me.btnWS05Step.UseVisualStyleBackColor = True
        Me.btnWS05Step.Visible = False
        '
        'btnWS05StepMode
        '
        Me.btnWS05StepMode.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnWS05StepMode.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05StepMode.Location = New System.Drawing.Point(2, 776)
        Me.btnWS05StepMode.Name = "btnWS05StepMode"
        Me.btnWS05StepMode.Size = New System.Drawing.Size(120, 37)
        Me.btnWS05StepMode.TabIndex = 200
        Me.btnWS05StepMode.Text = "Step mode"
        Me.btnWS05StepMode.UseVisualStyleBackColor = False
        Me.btnWS05StepMode.Visible = False
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(332, 792)
        Me.Label19.Margin = New System.Windows.Forms.Padding(3)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(140, 18)
        Me.Label19.TabIndex = 197
        Me.Label19.Text = "PLC I/O monitor"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS05StartStep
        '
        Me.lblWS05StartStep.BackColor = System.Drawing.Color.White
        Me.lblWS05StartStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS05StartStep.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05StartStep.Location = New System.Drawing.Point(332, 868)
        Me.lblWS05StartStep.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.lblWS05StartStep.Name = "lblWS05StartStep"
        Me.lblWS05StartStep.Size = New System.Drawing.Size(140, 19)
        Me.lblWS05StartStep.TabIndex = 196
        Me.lblWS05StartStep.Text = "Start step"
        Me.lblWS05StartStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS05TestOk
        '
        Me.lblWS05TestOk.BackColor = System.Drawing.Color.White
        Me.lblWS05TestOk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS05TestOk.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05TestOk.Location = New System.Drawing.Point(332, 850)
        Me.lblWS05TestOk.Name = "lblWS05TestOk"
        Me.lblWS05TestOk.Size = New System.Drawing.Size(140, 19)
        Me.lblWS05TestOk.TabIndex = 195
        Me.lblWS05TestOk.Text = "Test OK part"
        Me.lblWS05TestOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS05StepInProgress
        '
        Me.lblWS05StepInProgress.BackColor = System.Drawing.Color.White
        Me.lblWS05StepInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS05StepInProgress.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05StepInProgress.Location = New System.Drawing.Point(332, 832)
        Me.lblWS05StepInProgress.Name = "lblWS05StepInProgress"
        Me.lblWS05StepInProgress.Size = New System.Drawing.Size(140, 19)
        Me.lblWS05StepInProgress.TabIndex = 194
        Me.lblWS05StepInProgress.Text = "Step in progress"
        Me.lblWS05StepInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS05TestEnable
        '
        Me.lblWS05TestEnable.BackColor = System.Drawing.Color.White
        Me.lblWS05TestEnable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS05TestEnable.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS05TestEnable.Location = New System.Drawing.Point(332, 813)
        Me.lblWS05TestEnable.Name = "lblWS05TestEnable"
        Me.lblWS05TestEnable.Size = New System.Drawing.Size(140, 19)
        Me.lblWS05TestEnable.TabIndex = 193
        Me.lblWS05TestEnable.Text = "Test enable"
        Me.lblWS05TestEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BtnWS02StartPLC)
        Me.GroupBox1.Controls.Add(Me.lblWS02Reference)
        Me.GroupBox1.Controls.Add(Me.lblWS02TestMode)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.lblWS02TPCycle)
        Me.GroupBox1.Controls.Add(Me.btnWS02Step)
        Me.GroupBox1.Controls.Add(Me.btnWS02StepMode)
        Me.GroupBox1.Controls.Add(Me.btnWS02AbortTest)
        Me.GroupBox1.Controls.Add(Me.btnWS02StartTest)
        Me.GroupBox1.Controls.Add(Me.lblSt60IOMonitor)
        Me.GroupBox1.Controls.Add(Me.lblWS02StartStep)
        Me.GroupBox1.Controls.Add(Me.lblWS02TestOkA)
        Me.GroupBox1.Controls.Add(Me.lblWS02StepInProgress)
        Me.GroupBox1.Controls.Add(Me.lblWS02TestEnable)
        Me.GroupBox1.Controls.Add(Me.lblWS02TestResult)
        Me.GroupBox1.Controls.Add(Me.lblstFWTestResultALabel)
        Me.GroupBox1.Controls.Add(Me.dgvWS02SingleTestResults)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.lblWS02Phase)
        Me.GroupBox1.Controls.Add(Me.lblstFWReferenceLabel)
        Me.GroupBox1.Controls.Add(Me.lblstFWTestModeLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 109)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(470, 940)
        Me.GroupBox1.TabIndex = 206
        Me.GroupBox1.TabStop = False
        '
        'BtnWS02StartPLC
        '
        Me.BtnWS02StartPLC.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BtnWS02StartPLC.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnWS02StartPLC.Location = New System.Drawing.Point(141, 719)
        Me.BtnWS02StartPLC.Name = "BtnWS02StartPLC"
        Me.BtnWS02StartPLC.Size = New System.Drawing.Size(213, 51)
        Me.BtnWS02StartPLC.TabIndex = 230
        Me.BtnWS02StartPLC.Text = "Start Automatic Cycle"
        Me.BtnWS02StartPLC.UseVisualStyleBackColor = False
        '
        'lblWS02Reference
        '
        Me.lblWS02Reference.BackColor = System.Drawing.Color.White
        Me.lblWS02Reference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS02Reference.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02Reference.Location = New System.Drawing.Point(244, 45)
        Me.lblWS02Reference.Name = "lblWS02Reference"
        Me.lblWS02Reference.Size = New System.Drawing.Size(220, 37)
        Me.lblWS02Reference.TabIndex = 157
        Me.lblWS02Reference.Text = "Reference"
        Me.lblWS02Reference.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS02TestMode
        '
        Me.lblWS02TestMode.BackColor = System.Drawing.Color.White
        Me.lblWS02TestMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS02TestMode.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02TestMode.Location = New System.Drawing.Point(7, 45)
        Me.lblWS02TestMode.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS02TestMode.Name = "lblWS02TestMode"
        Me.lblWS02TestMode.Size = New System.Drawing.Size(220, 37)
        Me.lblWS02TestMode.TabIndex = 155
        Me.lblWS02TestMode.Text = "Test mode"
        Me.lblWS02TestMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(11, 827)
        Me.Label8.Margin = New System.Windows.Forms.Padding(3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 24)
        Me.Label8.TabIndex = 197
        Me.Label8.Text = "Cycle Time"
        '
        'lblWS02TPCycle
        '
        Me.lblWS02TPCycle.BackColor = System.Drawing.Color.White
        Me.lblWS02TPCycle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS02TPCycle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02TPCycle.Location = New System.Drawing.Point(137, 827)
        Me.lblWS02TPCycle.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS02TPCycle.Name = "lblWS02TPCycle"
        Me.lblWS02TPCycle.Size = New System.Drawing.Size(90, 25)
        Me.lblWS02TPCycle.TabIndex = 196
        Me.lblWS02TPCycle.Text = "0.00 s"
        Me.lblWS02TPCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnWS02Step
        '
        Me.btnWS02Step.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Step.Location = New System.Drawing.Point(141, 776)
        Me.btnWS02Step.Name = "btnWS02Step"
        Me.btnWS02Step.Size = New System.Drawing.Size(120, 37)
        Me.btnWS02Step.TabIndex = 195
        Me.btnWS02Step.Text = "Step"
        Me.btnWS02Step.UseVisualStyleBackColor = True
        '
        'btnWS02StepMode
        '
        Me.btnWS02StepMode.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnWS02StepMode.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02StepMode.Location = New System.Drawing.Point(15, 776)
        Me.btnWS02StepMode.Name = "btnWS02StepMode"
        Me.btnWS02StepMode.Size = New System.Drawing.Size(120, 37)
        Me.btnWS02StepMode.TabIndex = 194
        Me.btnWS02StepMode.Text = "Step mode"
        Me.btnWS02StepMode.UseVisualStyleBackColor = False
        '
        'btnWS02AbortTest
        '
        Me.btnWS02AbortTest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02AbortTest.Location = New System.Drawing.Point(339, 887)
        Me.btnWS02AbortTest.Name = "btnWS02AbortTest"
        Me.btnWS02AbortTest.Size = New System.Drawing.Size(120, 38)
        Me.btnWS02AbortTest.TabIndex = 193
        Me.btnWS02AbortTest.Text = "Abort test"
        Me.btnWS02AbortTest.UseVisualStyleBackColor = True
        Me.btnWS02AbortTest.Visible = False
        '
        'btnWS02StartTest
        '
        Me.btnWS02StartTest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02StartTest.Location = New System.Drawing.Point(7, 888)
        Me.btnWS02StartTest.Name = "btnWS02StartTest"
        Me.btnWS02StartTest.Size = New System.Drawing.Size(106, 37)
        Me.btnWS02StartTest.TabIndex = 192
        Me.btnWS02StartTest.Text = "Start test"
        Me.btnWS02StartTest.UseVisualStyleBackColor = True
        Me.btnWS02StartTest.Visible = False
        '
        'lblSt60IOMonitor
        '
        Me.lblSt60IOMonitor.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSt60IOMonitor.Location = New System.Drawing.Point(329, 787)
        Me.lblSt60IOMonitor.Margin = New System.Windows.Forms.Padding(3)
        Me.lblSt60IOMonitor.Name = "lblSt60IOMonitor"
        Me.lblSt60IOMonitor.Size = New System.Drawing.Size(130, 18)
        Me.lblSt60IOMonitor.TabIndex = 191
        Me.lblSt60IOMonitor.Text = "PLC I/O monitor"
        Me.lblSt60IOMonitor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS02StartStep
        '
        Me.lblWS02StartStep.BackColor = System.Drawing.Color.White
        Me.lblWS02StartStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS02StartStep.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02StartStep.Location = New System.Drawing.Point(329, 863)
        Me.lblWS02StartStep.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.lblWS02StartStep.Name = "lblWS02StartStep"
        Me.lblWS02StartStep.Size = New System.Drawing.Size(130, 19)
        Me.lblWS02StartStep.TabIndex = 190
        Me.lblWS02StartStep.Text = "Start step"
        Me.lblWS02StartStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS02TestOkA
        '
        Me.lblWS02TestOkA.BackColor = System.Drawing.Color.White
        Me.lblWS02TestOkA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS02TestOkA.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02TestOkA.Location = New System.Drawing.Point(329, 845)
        Me.lblWS02TestOkA.Name = "lblWS02TestOkA"
        Me.lblWS02TestOkA.Size = New System.Drawing.Size(130, 19)
        Me.lblWS02TestOkA.TabIndex = 189
        Me.lblWS02TestOkA.Text = "Test OK"
        Me.lblWS02TestOkA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS02StepInProgress
        '
        Me.lblWS02StepInProgress.BackColor = System.Drawing.Color.White
        Me.lblWS02StepInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS02StepInProgress.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02StepInProgress.Location = New System.Drawing.Point(329, 827)
        Me.lblWS02StepInProgress.Name = "lblWS02StepInProgress"
        Me.lblWS02StepInProgress.Size = New System.Drawing.Size(130, 19)
        Me.lblWS02StepInProgress.TabIndex = 188
        Me.lblWS02StepInProgress.Text = "Step in progress"
        Me.lblWS02StepInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS02TestEnable
        '
        Me.lblWS02TestEnable.BackColor = System.Drawing.Color.White
        Me.lblWS02TestEnable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS02TestEnable.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02TestEnable.Location = New System.Drawing.Point(329, 808)
        Me.lblWS02TestEnable.Name = "lblWS02TestEnable"
        Me.lblWS02TestEnable.Size = New System.Drawing.Size(130, 19)
        Me.lblWS02TestEnable.TabIndex = 187
        Me.lblWS02TestEnable.Text = "Test enable"
        Me.lblWS02TestEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS02TestResult
        '
        Me.lblWS02TestResult.BackColor = System.Drawing.Color.White
        Me.lblWS02TestResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS02TestResult.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02TestResult.Location = New System.Drawing.Point(9, 889)
        Me.lblWS02TestResult.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS02TestResult.Name = "lblWS02TestResult"
        Me.lblWS02TestResult.Size = New System.Drawing.Size(450, 37)
        Me.lblWS02TestResult.TabIndex = 186
        Me.lblWS02TestResult.Text = "Test result"
        Me.lblWS02TestResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblstFWTestResultALabel
        '
        Me.lblstFWTestResultALabel.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblstFWTestResultALabel.Location = New System.Drawing.Point(171, 865)
        Me.lblstFWTestResultALabel.Margin = New System.Windows.Forms.Padding(3)
        Me.lblstFWTestResultALabel.Name = "lblstFWTestResultALabel"
        Me.lblstFWTestResultALabel.Size = New System.Drawing.Size(90, 18)
        Me.lblstFWTestResultALabel.TabIndex = 185
        Me.lblstFWTestResultALabel.Text = "Test result"
        Me.lblstFWTestResultALabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvWS02SingleTestResults
        '
        Me.dgvWS02SingleTestResults.AllowUserToAddRows = False
        Me.dgvWS02SingleTestResults.AllowUserToDeleteRows = False
        Me.dgvWS02SingleTestResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02SingleTestResults.Location = New System.Drawing.Point(7, 134)
        Me.dgvWS02SingleTestResults.Name = "dgvWS02SingleTestResults"
        Me.dgvWS02SingleTestResults.ReadOnly = True
        Me.dgvWS02SingleTestResults.Size = New System.Drawing.Size(455, 54)
        Me.dgvWS02SingleTestResults.TabIndex = 160
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Times New Roman", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(154, 12)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(165, 33)
        Me.Label13.TabIndex = 159
        Me.Label13.Text = "WS02"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS02Phase
        '
        Me.lblWS02Phase.BackColor = System.Drawing.Color.White
        Me.lblWS02Phase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS02Phase.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS02Phase.Location = New System.Drawing.Point(7, 85)
        Me.lblWS02Phase.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS02Phase.Name = "lblWS02Phase"
        Me.lblWS02Phase.Size = New System.Drawing.Size(455, 37)
        Me.lblWS02Phase.TabIndex = 158
        Me.lblWS02Phase.Text = "Phase A"
        Me.lblWS02Phase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblstFWReferenceLabel
        '
        Me.lblstFWReferenceLabel.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblstFWReferenceLabel.Location = New System.Drawing.Point(342, 24)
        Me.lblstFWReferenceLabel.Name = "lblstFWReferenceLabel"
        Me.lblstFWReferenceLabel.Size = New System.Drawing.Size(122, 18)
        Me.lblstFWReferenceLabel.TabIndex = 156
        Me.lblstFWReferenceLabel.Text = "Reference"
        Me.lblstFWReferenceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblstFWTestModeLabel
        '
        Me.lblstFWTestModeLabel.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblstFWTestModeLabel.Location = New System.Drawing.Point(9, 27)
        Me.lblstFWTestModeLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.lblstFWTestModeLabel.Name = "lblstFWTestModeLabel"
        Me.lblstFWTestModeLabel.Size = New System.Drawing.Size(156, 18)
        Me.lblstFWTestModeLabel.TabIndex = 154
        Me.lblstFWTestModeLabel.Text = "Test mode"
        Me.lblstFWTestModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Size = New System.Drawing.Size(1940, 1100)
        Me.ShapeContainer1.TabIndex = 154
        Me.ShapeContainer1.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BtnWS03StartPLC)
        Me.GroupBox2.Controls.Add(Me.btnWS03AbortTest)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.Label18)
        Me.GroupBox2.Controls.Add(Me.lblWS03TPCycle)
        Me.GroupBox2.Controls.Add(Me.BTShowDUTStats)
        Me.GroupBox2.Controls.Add(Me.btnWS03StartTest)
        Me.GroupBox2.Controls.Add(Me.lblWS03TestResult)
        Me.GroupBox2.Controls.Add(Me.btnWS03Step)
        Me.GroupBox2.Controls.Add(Me.btnWS03StepMode)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.lblWS03StartStep)
        Me.GroupBox2.Controls.Add(Me.lblWS03TestOk)
        Me.GroupBox2.Controls.Add(Me.lblWS03StepInProgress)
        Me.GroupBox2.Controls.Add(Me.lblWS03TestEnable)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.lblWS03Phase)
        Me.GroupBox2.Controls.Add(Me.lblWS03Reference)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.lblWS03TestMode)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.dgvWS03SingleTestResults)
        Me.GroupBox2.Location = New System.Drawing.Point(475, 109)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(480, 940)
        Me.GroupBox2.TabIndex = 207
        Me.GroupBox2.TabStop = False
        '
        'BtnWS03StartPLC
        '
        Me.BtnWS03StartPLC.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BtnWS03StartPLC.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnWS03StartPLC.Location = New System.Drawing.Point(140, 719)
        Me.BtnWS03StartPLC.Name = "BtnWS03StartPLC"
        Me.BtnWS03StartPLC.Size = New System.Drawing.Size(213, 51)
        Me.BtnWS03StartPLC.TabIndex = 230
        Me.BtnWS03StartPLC.Text = "Start Automatic Cycle"
        Me.BtnWS03StartPLC.UseVisualStyleBackColor = False
        '
        'btnWS03AbortTest
        '
        Me.btnWS03AbortTest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03AbortTest.Location = New System.Drawing.Point(372, 888)
        Me.btnWS03AbortTest.Name = "btnWS03AbortTest"
        Me.btnWS03AbortTest.Size = New System.Drawing.Size(100, 37)
        Me.btnWS03AbortTest.TabIndex = 211
        Me.btnWS03AbortTest.Text = "Abort test"
        Me.btnWS03AbortTest.UseVisualStyleBackColor = True
        Me.btnWS03AbortTest.Visible = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(10, 821)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(120, 24)
        Me.Label16.TabIndex = 217
        Me.Label16.Text = "Cycle Time"
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(185, 866)
        Me.Label18.Margin = New System.Windows.Forms.Padding(3)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(85, 18)
        Me.Label18.TabIndex = 216
        Me.Label18.Text = "Test result"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS03TPCycle
        '
        Me.lblWS03TPCycle.BackColor = System.Drawing.Color.White
        Me.lblWS03TPCycle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS03TPCycle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03TPCycle.Location = New System.Drawing.Point(140, 822)
        Me.lblWS03TPCycle.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS03TPCycle.Name = "lblWS03TPCycle"
        Me.lblWS03TPCycle.Size = New System.Drawing.Size(90, 25)
        Me.lblWS03TPCycle.TabIndex = 215
        Me.lblWS03TPCycle.Text = "0.00 s"
        Me.lblWS03TPCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BTShowDUTStats
        '
        Me.BTShowDUTStats.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BTShowDUTStats.Location = New System.Drawing.Point(177, 649)
        Me.BTShowDUTStats.Name = "BTShowDUTStats"
        Me.BTShowDUTStats.Size = New System.Drawing.Size(136, 50)
        Me.BTShowDUTStats.TabIndex = 210
        Me.BTShowDUTStats.Text = "显示DUT统计" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Show DUT stats"
        Me.BTShowDUTStats.UseVisualStyleBackColor = True
        '
        'btnWS03StartTest
        '
        Me.btnWS03StartTest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03StartTest.Location = New System.Drawing.Point(6, 888)
        Me.btnWS03StartTest.Name = "btnWS03StartTest"
        Me.btnWS03StartTest.Size = New System.Drawing.Size(104, 37)
        Me.btnWS03StartTest.TabIndex = 210
        Me.btnWS03StartTest.Text = "Start test"
        Me.btnWS03StartTest.UseVisualStyleBackColor = True
        Me.btnWS03StartTest.Visible = False
        '
        'lblWS03TestResult
        '
        Me.lblWS03TestResult.BackColor = System.Drawing.Color.White
        Me.lblWS03TestResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS03TestResult.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03TestResult.Location = New System.Drawing.Point(8, 888)
        Me.lblWS03TestResult.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS03TestResult.Name = "lblWS03TestResult"
        Me.lblWS03TestResult.Size = New System.Drawing.Size(464, 37)
        Me.lblWS03TestResult.TabIndex = 214
        Me.lblWS03TestResult.Text = "Test result"
        Me.lblWS03TestResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnWS03Step
        '
        Me.btnWS03Step.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Step.Location = New System.Drawing.Point(140, 776)
        Me.btnWS03Step.Name = "btnWS03Step"
        Me.btnWS03Step.Size = New System.Drawing.Size(120, 37)
        Me.btnWS03Step.TabIndex = 213
        Me.btnWS03Step.Text = "Step"
        Me.btnWS03Step.UseVisualStyleBackColor = True
        Me.btnWS03Step.Visible = False
        '
        'btnWS03StepMode
        '
        Me.btnWS03StepMode.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnWS03StepMode.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03StepMode.Location = New System.Drawing.Point(6, 776)
        Me.btnWS03StepMode.Name = "btnWS03StepMode"
        Me.btnWS03StepMode.Size = New System.Drawing.Size(120, 37)
        Me.btnWS03StepMode.TabIndex = 212
        Me.btnWS03StepMode.Text = "Step mode"
        Me.btnWS03StepMode.UseVisualStyleBackColor = False
        Me.btnWS03StepMode.Visible = False
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(332, 794)
        Me.Label6.Margin = New System.Windows.Forms.Padding(3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(140, 18)
        Me.Label6.TabIndex = 209
        Me.Label6.Text = "PLC I/O monitor"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS03StartStep
        '
        Me.lblWS03StartStep.BackColor = System.Drawing.Color.White
        Me.lblWS03StartStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS03StartStep.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03StartStep.Location = New System.Drawing.Point(332, 866)
        Me.lblWS03StartStep.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.lblWS03StartStep.Name = "lblWS03StartStep"
        Me.lblWS03StartStep.Size = New System.Drawing.Size(140, 19)
        Me.lblWS03StartStep.TabIndex = 208
        Me.lblWS03StartStep.Text = "Start step"
        Me.lblWS03StartStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS03TestOk
        '
        Me.lblWS03TestOk.BackColor = System.Drawing.Color.White
        Me.lblWS03TestOk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS03TestOk.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03TestOk.Location = New System.Drawing.Point(332, 848)
        Me.lblWS03TestOk.Name = "lblWS03TestOk"
        Me.lblWS03TestOk.Size = New System.Drawing.Size(140, 19)
        Me.lblWS03TestOk.TabIndex = 207
        Me.lblWS03TestOk.Text = "Test OK part"
        Me.lblWS03TestOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS03StepInProgress
        '
        Me.lblWS03StepInProgress.BackColor = System.Drawing.Color.White
        Me.lblWS03StepInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS03StepInProgress.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03StepInProgress.Location = New System.Drawing.Point(332, 830)
        Me.lblWS03StepInProgress.Name = "lblWS03StepInProgress"
        Me.lblWS03StepInProgress.Size = New System.Drawing.Size(140, 19)
        Me.lblWS03StepInProgress.TabIndex = 206
        Me.lblWS03StepInProgress.Text = "Step in progress"
        Me.lblWS03StepInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS03TestEnable
        '
        Me.lblWS03TestEnable.BackColor = System.Drawing.Color.White
        Me.lblWS03TestEnable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS03TestEnable.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03TestEnable.Location = New System.Drawing.Point(332, 814)
        Me.lblWS03TestEnable.Name = "lblWS03TestEnable"
        Me.lblWS03TestEnable.Size = New System.Drawing.Size(140, 19)
        Me.lblWS03TestEnable.TabIndex = 205
        Me.lblWS03TestEnable.Text = "Test enable"
        Me.lblWS03TestEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(194, 12)
        Me.Label2.Margin = New System.Windows.Forms.Padding(3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(119, 33)
        Me.Label2.TabIndex = 163
        Me.Label2.Text = "WS03"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS03Phase
        '
        Me.lblWS03Phase.BackColor = System.Drawing.Color.White
        Me.lblWS03Phase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS03Phase.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03Phase.Location = New System.Drawing.Point(10, 85)
        Me.lblWS03Phase.Name = "lblWS03Phase"
        Me.lblWS03Phase.Size = New System.Drawing.Size(462, 37)
        Me.lblWS03Phase.TabIndex = 162
        Me.lblWS03Phase.Text = "Phase A"
        Me.lblWS03Phase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS03Reference
        '
        Me.lblWS03Reference.BackColor = System.Drawing.Color.White
        Me.lblWS03Reference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS03Reference.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03Reference.Location = New System.Drawing.Point(254, 45)
        Me.lblWS03Reference.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS03Reference.Name = "lblWS03Reference"
        Me.lblWS03Reference.Size = New System.Drawing.Size(220, 37)
        Me.lblWS03Reference.TabIndex = 161
        Me.lblWS03Reference.Text = "Reference"
        Me.lblWS03Reference.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(319, 25)
        Me.Label3.Margin = New System.Windows.Forms.Padding(3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(155, 18)
        Me.Label3.TabIndex = 160
        Me.Label3.Text = "Reference"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS03TestMode
        '
        Me.lblWS03TestMode.BackColor = System.Drawing.Color.White
        Me.lblWS03TestMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS03TestMode.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS03TestMode.Location = New System.Drawing.Point(12, 45)
        Me.lblWS03TestMode.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS03TestMode.Name = "lblWS03TestMode"
        Me.lblWS03TestMode.Size = New System.Drawing.Size(220, 37)
        Me.lblWS03TestMode.TabIndex = 159
        Me.lblWS03TestMode.Text = "Test mode"
        Me.lblWS03TestMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(13, 25)
        Me.Label5.Margin = New System.Windows.Forms.Padding(3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(159, 18)
        Me.Label5.TabIndex = 158
        Me.Label5.Text = "Test mode"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvWS03SingleTestResults
        '
        Me.dgvWS03SingleTestResults.AllowUserToAddRows = False
        Me.dgvWS03SingleTestResults.AllowUserToDeleteRows = False
        Me.dgvWS03SingleTestResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03SingleTestResults.Location = New System.Drawing.Point(12, 134)
        Me.dgvWS03SingleTestResults.Name = "dgvWS03SingleTestResults"
        Me.dgvWS03SingleTestResults.ReadOnly = True
        Me.dgvWS03SingleTestResults.Size = New System.Drawing.Size(462, 54)
        Me.dgvWS03SingleTestResults.TabIndex = 157
        '
        'ssStatusBar
        '
        Me.ssStatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssStatusBar.Location = New System.Drawing.Point(0, 1058)
        Me.ssStatusBar.Name = "ssStatusBar"
        Me.ssStatusBar.Size = New System.Drawing.Size(1920, 22)
        Me.ssStatusBar.TabIndex = 208
        Me.ssStatusBar.Text = "StatusStrip1"
        '
        'tmrLoop
        '
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'dgvWS04SingleTestResults
        '
        Me.dgvWS04SingleTestResults.AllowUserToAddRows = False
        Me.dgvWS04SingleTestResults.AllowUserToDeleteRows = False
        Me.dgvWS04SingleTestResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04SingleTestResults.Location = New System.Drawing.Point(8, 134)
        Me.dgvWS04SingleTestResults.Name = "dgvWS04SingleTestResults"
        Me.dgvWS04SingleTestResults.ReadOnly = True
        Me.dgvWS04SingleTestResults.Size = New System.Drawing.Size(463, 54)
        Me.dgvWS04SingleTestResults.TabIndex = 209
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(172, 11)
        Me.Label7.Margin = New System.Windows.Forms.Padding(3)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(143, 33)
        Me.Label7.TabIndex = 215
        Me.Label7.Text = "WS04"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS04Phase
        '
        Me.lblWS04Phase.BackColor = System.Drawing.Color.White
        Me.lblWS04Phase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS04Phase.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04Phase.Location = New System.Drawing.Point(6, 85)
        Me.lblWS04Phase.Name = "lblWS04Phase"
        Me.lblWS04Phase.Size = New System.Drawing.Size(465, 37)
        Me.lblWS04Phase.TabIndex = 214
        Me.lblWS04Phase.Text = "Phase A"
        Me.lblWS04Phase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS04Reference
        '
        Me.lblWS04Reference.BackColor = System.Drawing.Color.White
        Me.lblWS04Reference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS04Reference.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04Reference.Location = New System.Drawing.Point(252, 45)
        Me.lblWS04Reference.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS04Reference.Name = "lblWS04Reference"
        Me.lblWS04Reference.Size = New System.Drawing.Size(220, 37)
        Me.lblWS04Reference.TabIndex = 213
        Me.lblWS04Reference.Text = "Reference"
        Me.lblWS04Reference.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(331, 23)
        Me.Label15.Margin = New System.Windows.Forms.Padding(3)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(140, 18)
        Me.Label15.TabIndex = 212
        Me.Label15.Text = "Reference"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS04TestMode
        '
        Me.lblWS04TestMode.BackColor = System.Drawing.Color.White
        Me.lblWS04TestMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS04TestMode.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04TestMode.Location = New System.Drawing.Point(4, 45)
        Me.lblWS04TestMode.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS04TestMode.Name = "lblWS04TestMode"
        Me.lblWS04TestMode.Size = New System.Drawing.Size(220, 37)
        Me.lblWS04TestMode.TabIndex = 211
        Me.lblWS04TestMode.Text = "Test mode"
        Me.lblWS04TestMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(26, 26)
        Me.Label20.Margin = New System.Windows.Forms.Padding(3)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(155, 18)
        Me.Label20.TabIndex = 210
        Me.Label20.Text = "Test mode"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS04TPCycle
        '
        Me.lblWS04TPCycle.BackColor = System.Drawing.Color.White
        Me.lblWS04TPCycle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS04TPCycle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04TPCycle.Location = New System.Drawing.Point(144, 820)
        Me.lblWS04TPCycle.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS04TPCycle.Name = "lblWS04TPCycle"
        Me.lblWS04TPCycle.Size = New System.Drawing.Size(90, 25)
        Me.lblWS04TPCycle.TabIndex = 228
        Me.lblWS04TPCycle.Text = "0.00 s"
        Me.lblWS04TPCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnWS04StartTest
        '
        Me.btnWS04StartTest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04StartTest.Location = New System.Drawing.Point(4, 888)
        Me.btnWS04StartTest.Name = "btnWS04StartTest"
        Me.btnWS04StartTest.Size = New System.Drawing.Size(120, 37)
        Me.btnWS04StartTest.TabIndex = 223
        Me.btnWS04StartTest.Text = "Start test"
        Me.btnWS04StartTest.UseVisualStyleBackColor = True
        Me.btnWS04StartTest.Visible = False
        '
        'btnWS04AbortTest
        '
        Me.btnWS04AbortTest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04AbortTest.Location = New System.Drawing.Point(351, 888)
        Me.btnWS04AbortTest.Name = "btnWS04AbortTest"
        Me.btnWS04AbortTest.Size = New System.Drawing.Size(120, 37)
        Me.btnWS04AbortTest.TabIndex = 224
        Me.btnWS04AbortTest.Text = "Abort test"
        Me.btnWS04AbortTest.UseVisualStyleBackColor = True
        Me.btnWS04AbortTest.Visible = False
        '
        'lblWS04TestResult
        '
        Me.lblWS04TestResult.BackColor = System.Drawing.Color.White
        Me.lblWS04TestResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWS04TestResult.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04TestResult.Location = New System.Drawing.Point(4, 888)
        Me.lblWS04TestResult.Margin = New System.Windows.Forms.Padding(3)
        Me.lblWS04TestResult.Name = "lblWS04TestResult"
        Me.lblWS04TestResult.Size = New System.Drawing.Size(467, 37)
        Me.lblWS04TestResult.TabIndex = 227
        Me.lblWS04TestResult.Text = "Test result"
        Me.lblWS04TestResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnWS04Step
        '
        Me.btnWS04Step.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Step.Location = New System.Drawing.Point(144, 778)
        Me.btnWS04Step.Name = "btnWS04Step"
        Me.btnWS04Step.Size = New System.Drawing.Size(120, 37)
        Me.btnWS04Step.TabIndex = 226
        Me.btnWS04Step.Text = "Step"
        Me.btnWS04Step.UseVisualStyleBackColor = True
        Me.btnWS04Step.Visible = False
        '
        'btnWS04StepMode
        '
        Me.btnWS04StepMode.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnWS04StepMode.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04StepMode.Location = New System.Drawing.Point(18, 778)
        Me.btnWS04StepMode.Name = "btnWS04StepMode"
        Me.btnWS04StepMode.Size = New System.Drawing.Size(120, 37)
        Me.btnWS04StepMode.TabIndex = 225
        Me.btnWS04StepMode.Text = "Step mode"
        Me.btnWS04StepMode.UseVisualStyleBackColor = False
        Me.btnWS04StepMode.Visible = False
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(331, 790)
        Me.Label23.Margin = New System.Windows.Forms.Padding(3)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(140, 18)
        Me.Label23.TabIndex = 222
        Me.Label23.Text = "PLC I/O monitor"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS04StartStep
        '
        Me.lblWS04StartStep.BackColor = System.Drawing.Color.White
        Me.lblWS04StartStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS04StartStep.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04StartStep.Location = New System.Drawing.Point(331, 866)
        Me.lblWS04StartStep.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.lblWS04StartStep.Name = "lblWS04StartStep"
        Me.lblWS04StartStep.Size = New System.Drawing.Size(140, 19)
        Me.lblWS04StartStep.TabIndex = 221
        Me.lblWS04StartStep.Text = "Start step"
        Me.lblWS04StartStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS04TestOk
        '
        Me.lblWS04TestOk.BackColor = System.Drawing.Color.White
        Me.lblWS04TestOk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS04TestOk.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04TestOk.Location = New System.Drawing.Point(331, 848)
        Me.lblWS04TestOk.Name = "lblWS04TestOk"
        Me.lblWS04TestOk.Size = New System.Drawing.Size(140, 19)
        Me.lblWS04TestOk.TabIndex = 220
        Me.lblWS04TestOk.Text = "Test OK part"
        Me.lblWS04TestOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS04StepInProgress
        '
        Me.lblWS04StepInProgress.BackColor = System.Drawing.Color.White
        Me.lblWS04StepInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS04StepInProgress.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04StepInProgress.Location = New System.Drawing.Point(331, 830)
        Me.lblWS04StepInProgress.Name = "lblWS04StepInProgress"
        Me.lblWS04StepInProgress.Size = New System.Drawing.Size(140, 19)
        Me.lblWS04StepInProgress.TabIndex = 219
        Me.lblWS04StepInProgress.Text = "Step in progress"
        Me.lblWS04StepInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWS04TestEnable
        '
        Me.lblWS04TestEnable.BackColor = System.Drawing.Color.White
        Me.lblWS04TestEnable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWS04TestEnable.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWS04TestEnable.Location = New System.Drawing.Point(331, 811)
        Me.lblWS04TestEnable.Name = "lblWS04TestEnable"
        Me.lblWS04TestEnable.Size = New System.Drawing.Size(140, 19)
        Me.lblWS04TestEnable.TabIndex = 218
        Me.lblWS04TestEnable.Text = "Test enable"
        Me.lblWS04TestEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(18, 816)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(120, 24)
        Me.Label28.TabIndex = 217
        Me.Label28.Text = "Cycle Time"
        '
        'Label29
        '
        Me.Label29.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(191, 868)
        Me.Label29.Margin = New System.Windows.Forms.Padding(3)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(84, 18)
        Me.Label29.TabIndex = 216
        Me.Label29.Text = "Test result"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.BtnWS04StartPLC)
        Me.GroupBox3.Controls.Add(Me.Label20)
        Me.GroupBox3.Controls.Add(Me.dgvWS04SingleTestResults)
        Me.GroupBox3.Controls.Add(Me.lblWS04TPCycle)
        Me.GroupBox3.Controls.Add(Me.lblWS04TestMode)
        Me.GroupBox3.Controls.Add(Me.btnWS04StartTest)
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.btnWS04AbortTest)
        Me.GroupBox3.Controls.Add(Me.lblWS04TestResult)
        Me.GroupBox3.Controls.Add(Me.lblWS04Reference)
        Me.GroupBox3.Controls.Add(Me.btnWS04Step)
        Me.GroupBox3.Controls.Add(Me.lblWS04Phase)
        Me.GroupBox3.Controls.Add(Me.btnWS04StepMode)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.Label23)
        Me.GroupBox3.Controls.Add(Me.Label29)
        Me.GroupBox3.Controls.Add(Me.lblWS04StartStep)
        Me.GroupBox3.Controls.Add(Me.Label28)
        Me.GroupBox3.Controls.Add(Me.lblWS04TestOk)
        Me.GroupBox3.Controls.Add(Me.lblWS04TestEnable)
        Me.GroupBox3.Controls.Add(Me.lblWS04StepInProgress)
        Me.GroupBox3.Location = New System.Drawing.Point(955, 109)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(480, 940)
        Me.GroupBox3.TabIndex = 229
        Me.GroupBox3.TabStop = False
        '
        'BtnWS04StartPLC
        '
        Me.BtnWS04StartPLC.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BtnWS04StartPLC.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnWS04StartPLC.Location = New System.Drawing.Point(144, 719)
        Me.BtnWS04StartPLC.Name = "BtnWS04StartPLC"
        Me.BtnWS04StartPLC.Size = New System.Drawing.Size(213, 51)
        Me.BtnWS04StartPLC.TabIndex = 229
        Me.BtnWS04StartPLC.Text = "Start Automatic Cycle"
        Me.BtnWS04StartPLC.UseVisualStyleBackColor = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.BtnWS05StartPLC)
        Me.GroupBox4.Controls.Add(Me.dgvWS05SingleTestResults)
        Me.GroupBox4.Controls.Add(Me.Label14)
        Me.GroupBox4.Controls.Add(Me.lblWS05TestMode)
        Me.GroupBox4.Controls.Add(Me.Label10)
        Me.GroupBox4.Controls.Add(Me.lblWS05Reference)
        Me.GroupBox4.Controls.Add(Me.lblWS05TPCycle)
        Me.GroupBox4.Controls.Add(Me.lblWS05Phase)
        Me.GroupBox4.Controls.Add(Me.btnWS05StartTest)
        Me.GroupBox4.Controls.Add(Me.Label4)
        Me.GroupBox4.Controls.Add(Me.btnWS05AbortTest)
        Me.GroupBox4.Controls.Add(Me.btnWS05StepMode)
        Me.GroupBox4.Controls.Add(Me.lblWS05TestResult)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.Controls.Add(Me.btnWS05Step)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Controls.Add(Me.lblWS05TestEnable)
        Me.GroupBox4.Controls.Add(Me.Label19)
        Me.GroupBox4.Controls.Add(Me.lblWS05StepInProgress)
        Me.GroupBox4.Controls.Add(Me.lblWS05StartStep)
        Me.GroupBox4.Controls.Add(Me.lblWS05TestOk)
        Me.GroupBox4.Location = New System.Drawing.Point(1433, 109)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(480, 940)
        Me.GroupBox4.TabIndex = 230
        Me.GroupBox4.TabStop = False
        '
        'BtnWS05StartPLC
        '
        Me.BtnWS05StartPLC.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BtnWS05StartPLC.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnWS05StartPLC.Location = New System.Drawing.Point(132, 719)
        Me.BtnWS05StartPLC.Name = "BtnWS05StartPLC"
        Me.BtnWS05StartPLC.Size = New System.Drawing.Size(213, 51)
        Me.BtnWS05StartPLC.TabIndex = 230
        Me.BtnWS05StartPLC.Text = "Start Automatic Cycle"
        Me.BtnWS05StartPLC.UseVisualStyleBackColor = False
        '
        'CustomerLogo
        '
        Me.CustomerLogo.Image = CType(resources.GetObject("CustomerLogo.Image"), System.Drawing.Image)
        Me.CustomerLogo.Location = New System.Drawing.Point(1670, 0)
        Me.CustomerLogo.Name = "CustomerLogo"
        Me.CustomerLogo.Size = New System.Drawing.Size(242, 103)
        Me.CustomerLogo.TabIndex = 231
        Me.CustomerLogo.TabStop = False
        '
        'frmProduction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1920, 1080)
        Me.Controls.Add(Me.CustomerLogo)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.ssStatusBar)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblCountry)
        Me.Controls.Add(Me.lblECB)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Controls.Add(Me.lblSoftwareTitle)
        Me.Controls.Add(Me.pbLogoValeo)
        Me.Controls.Add(Me.lblCity)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmProduction"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmProduction"
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS05SingleTestResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgvWS02SingleTestResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.dgvWS03SingleTestResults, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS04SingleTestResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.CustomerLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCountry As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblFormTitle As System.Windows.Forms.Label
    Friend WithEvents lblSoftwareTitle As System.Windows.Forms.Label
    Friend WithEvents pbLogoValeo As System.Windows.Forms.PictureBox
    Friend WithEvents tmrMonitor As System.Windows.Forms.Timer
    Friend WithEvents lblECB As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvWS05SingleTestResults As System.Windows.Forms.DataGridView
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblWS05Phase As System.Windows.Forms.Label
    Friend WithEvents lblWS05Reference As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblWS05TestMode As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblWS05TPCycle As System.Windows.Forms.Label
    Friend WithEvents btnWS05StartTest As System.Windows.Forms.Button
    Friend WithEvents btnWS05AbortTest As System.Windows.Forms.Button
    Friend WithEvents lblWS05TestResult As System.Windows.Forms.Label
    Friend WithEvents btnWS05Step As System.Windows.Forms.Button
    Friend WithEvents btnWS05StepMode As System.Windows.Forms.Button
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents lblWS05StartStep As System.Windows.Forms.Label
    Friend WithEvents lblWS05TestOk As System.Windows.Forms.Label
    Friend WithEvents lblWS05StepInProgress As System.Windows.Forms.Label
    Friend WithEvents lblWS05TestEnable As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label8 As Label
    Friend WithEvents lblWS02TPCycle As Label
    Friend WithEvents btnWS02Step As Button
    Friend WithEvents btnWS02StepMode As Button
    Friend WithEvents btnWS02AbortTest As Button
    Friend WithEvents btnWS02StartTest As Button
    Friend WithEvents lblSt60IOMonitor As Label
    Friend WithEvents lblWS02StartStep As Label
    Friend WithEvents lblWS02TestOkA As Label
    Friend WithEvents lblWS02StepInProgress As Label
    Friend WithEvents lblWS02TestEnable As Label
    Friend WithEvents lblWS02TestResult As Label
    Friend WithEvents lblstFWTestResultALabel As Label
    Friend WithEvents dgvWS02SingleTestResults As DataGridView
    Friend WithEvents Label13 As Label
    Friend WithEvents lblWS02Phase As Label
    Friend WithEvents lblWS02Reference As Label
    Friend WithEvents lblstFWReferenceLabel As Label
    Friend WithEvents lblWS02TestMode As Label
    Friend WithEvents lblstFWTestModeLabel As Label
    Private WithEvents ShapeContainer1 As PowerPacks.ShapeContainer
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label16 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents lblWS03TPCycle As Label
    Friend WithEvents btnWS03StartTest As Button
    Friend WithEvents btnWS03AbortTest As Button
    Friend WithEvents lblWS03TestResult As Label
    Friend WithEvents btnWS03Step As Button
    Friend WithEvents btnWS03StepMode As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents lblWS03StartStep As Label
    Friend WithEvents lblWS03TestOk As Label
    Friend WithEvents lblWS03StepInProgress As Label
    Friend WithEvents lblWS03TestEnable As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblWS03Phase As Label
    Friend WithEvents lblWS03Reference As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblWS03TestMode As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents dgvWS03SingleTestResults As DataGridView
    Friend WithEvents ssStatusBar As System.Windows.Forms.StatusStrip
    Friend WithEvents tmrLoop As System.Windows.Forms.Timer
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents dgvWS04SingleTestResults As System.Windows.Forms.DataGridView
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblWS04Phase As System.Windows.Forms.Label
    Friend WithEvents lblWS04Reference As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents lblWS04TestMode As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents lblWS04TPCycle As System.Windows.Forms.Label
    Friend WithEvents btnWS04StartTest As System.Windows.Forms.Button
    Friend WithEvents btnWS04AbortTest As System.Windows.Forms.Button
    Friend WithEvents lblWS04TestResult As System.Windows.Forms.Label
    Friend WithEvents btnWS04Step As System.Windows.Forms.Button
    Friend WithEvents btnWS04StepMode As System.Windows.Forms.Button
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents lblWS04StartStep As System.Windows.Forms.Label
    Friend WithEvents lblWS04TestOk As System.Windows.Forms.Label
    Friend WithEvents lblWS04StepInProgress As System.Windows.Forms.Label
    Friend WithEvents lblWS04TestEnable As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents CustomerLogo As System.Windows.Forms.PictureBox
    Friend WithEvents BtnWS02StartPLC As System.Windows.Forms.Button
    Friend WithEvents BtnWS03StartPLC As System.Windows.Forms.Button
    Friend WithEvents BtnWS04StartPLC As System.Windows.Forms.Button
    Friend WithEvents BtnWS05StartPLC As System.Windows.Forms.Button
    Friend WithEvents BTShowDUTStats As Button
End Class
