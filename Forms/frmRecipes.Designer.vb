<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRecipes
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRecipes))
        Me.btnCreate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnCopyFrom = New System.Windows.Forms.Button()
        Me.btnCancelModifies = New System.Windows.Forms.Button()
        Me.btnSaveModifies = New System.Windows.Forms.Button()
        Me.lblReference = New System.Windows.Forms.Label()
        Me.lblCreationDateDescription = New System.Windows.Forms.Label()
        Me.lblCreationDate = New System.Windows.Forms.Label()
        Me.lblModifyDateDescription = New System.Windows.Forms.Label()
        Me.lblModifyDate = New System.Windows.Forms.Label()
        Me.lblSoftwareTitle = New System.Windows.Forms.Label()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.lblECB = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.ssStatusBar = New System.Windows.Forms.StatusStrip()
        Me.tmrMonitor = New System.Windows.Forms.Timer(Me.components)
        Me.tcRecipe = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.tcStation = New System.Windows.Forms.TabControl()
        Me.tpTestEnables = New System.Windows.Forms.TabPage()
        Me.btnWS02Test25 = New System.Windows.Forms.Button()
        Me.btnWS02Test24 = New System.Windows.Forms.Button()
        Me.btnWS02Test23 = New System.Windows.Forms.Button()
        Me.btnWS02Test22 = New System.Windows.Forms.Button()
        Me.btnWS02Test21 = New System.Windows.Forms.Button()
        Me.btnWS02Test20 = New System.Windows.Forms.Button()
        Me.btnWS02Test19 = New System.Windows.Forms.Button()
        Me.btnWS02Test18 = New System.Windows.Forms.Button()
        Me.btnWS02Test17 = New System.Windows.Forms.Button()
        Me.btnWS02Test15 = New System.Windows.Forms.Button()
        Me.btnWS02Test16 = New System.Windows.Forms.Button()
        Me.btnWS02Test27 = New System.Windows.Forms.Button()
        Me.btnWS02Test14 = New System.Windows.Forms.Button()
        Me.btnWS02Test13 = New System.Windows.Forms.Button()
        Me.btnWS02Test12 = New System.Windows.Forms.Button()
        Me.btnWS02Test10 = New System.Windows.Forms.Button()
        Me.btnWS02Test9 = New System.Windows.Forms.Button()
        Me.btnWS02Test11 = New System.Windows.Forms.Button()
        Me.btnWS02Test26 = New System.Windows.Forms.Button()
        Me.btnWS02Test8 = New System.Windows.Forms.Button()
        Me.btnWS02Test7 = New System.Windows.Forms.Button()
        Me.btnWS02Test6 = New System.Windows.Forms.Button()
        Me.btnWS02Test2 = New System.Windows.Forms.Button()
        Me.btnWS02Test1 = New System.Windows.Forms.Button()
        Me.btnWS02Test5 = New System.Windows.Forms.Button()
        Me.btnWS02Test4 = New System.Windows.Forms.Button()
        Me.btnWS02Test3 = New System.Windows.Forms.Button()
        Me.btnWS02Test0 = New System.Windows.Forms.Button()
        Me.btnWS02Test31 = New System.Windows.Forms.Button()
        Me.tpGeneralParameters = New System.Windows.Forms.TabPage()
        Me.dgvWS02GeneralSettings1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS02GeneralSettings0 = New System.Windows.Forms.DataGridView()
        Me.tp1 = New System.Windows.Forms.TabPage()
        Me.dgvWS02SHAPE1 = New System.Windows.Forms.DataGridView()
        Me.tp2 = New System.Windows.Forms.TabPage()
        Me.dgvWS02BACKLIGHT_1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS02BACKLIGHT = New System.Windows.Forms.DataGridView()
        Me.tp3 = New System.Windows.Forms.TabPage()
        Me.dgvWS02TELLTALE = New System.Windows.Forms.DataGridView()
        Me.TabPage15 = New System.Windows.Forms.TabPage()
        Me.dgvWS02EMSTraceability = New System.Windows.Forms.DataGridView()
        Me.TabPage16 = New System.Windows.Forms.TabPage()
        Me.TabControl4 = New System.Windows.Forms.TabControl()
        Me.TabPage24 = New System.Windows.Forms.TabPage()
        Me.dgvWS02DigitalOutputStdSignal = New System.Windows.Forms.DataGridView()
        Me.TabPage25 = New System.Windows.Forms.TabPage()
        Me.dgvWS02DigitalOutputPWL = New System.Windows.Forms.DataGridView()
        Me.TabPage26 = New System.Windows.Forms.TabPage()
        Me.dgvWS02DigitalOutputRTRVD = New System.Windows.Forms.DataGridView()
        Me.TabPage27 = New System.Windows.Forms.TabPage()
        Me.dgvWS02DigitalOutputRTRVG = New System.Windows.Forms.DataGridView()
        Me.TabPage28 = New System.Windows.Forms.TabPage()
        Me.dgvWS02PWMOutput = New System.Windows.Forms.DataGridView()
        Me.TabPage17 = New System.Windows.Forms.TabPage()
        Me.dgvWS02WriteTraceability = New System.Windows.Forms.DataGridView()
        Me.TabPage19 = New System.Windows.Forms.TabPage()
        Me.TabPage20 = New System.Windows.Forms.TabPage()
        Me.dgvWS02AnalogInput = New System.Windows.Forms.DataGridView()
        Me.TabPage23 = New System.Windows.Forms.TabPage()
        Me.dgvWS02WriteConfiguration = New System.Windows.Forms.DataGridView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.btnWS03Test12 = New System.Windows.Forms.Button()
        Me.btnWS03Test11 = New System.Windows.Forms.Button()
        Me.btnWS03Test10 = New System.Windows.Forms.Button()
        Me.btnWS03Test9 = New System.Windows.Forms.Button()
        Me.btnWS03Test8 = New System.Windows.Forms.Button()
        Me.btnWS03Test7 = New System.Windows.Forms.Button()
        Me.btnWS03Test6 = New System.Windows.Forms.Button()
        Me.btnWS03Test2 = New System.Windows.Forms.Button()
        Me.btnWS03Test1 = New System.Windows.Forms.Button()
        Me.btnWS03Test5 = New System.Windows.Forms.Button()
        Me.btnWS03Test4 = New System.Windows.Forms.Button()
        Me.btnWS03Test3 = New System.Windows.Forms.Button()
        Me.btnWS03Test0 = New System.Windows.Forms.Button()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.dgvWS03GeneralSettings1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS03GeneralSettings0 = New System.Windows.Forms.DataGridView()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.dgvWS03FrontLeft_0 = New System.Windows.Forms.DataGridView()
        Me.dgvWS03FrontLeft_1 = New System.Windows.Forms.DataGridView()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.dgvWS03FrontRight_1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS03FrontRight_0 = New System.Windows.Forms.DataGridView()
        Me.TabPage29 = New System.Windows.Forms.TabPage()
        Me.dgvWS03JamaFunction = New System.Windows.Forms.DataGridView()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.dgvWS03ChildrenLock_1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS03ChildrenLock_0 = New System.Windows.Forms.DataGridView()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.TabControl2 = New System.Windows.Forms.TabControl()
        Me.TabPage9 = New System.Windows.Forms.TabPage()
        Me.btnWS04Test10 = New System.Windows.Forms.Button()
        Me.btnWS04Test9 = New System.Windows.Forms.Button()
        Me.btnWS04Test8 = New System.Windows.Forms.Button()
        Me.btnWS04Test7 = New System.Windows.Forms.Button()
        Me.btnWS04Test6 = New System.Windows.Forms.Button()
        Me.btnWS04Test2 = New System.Windows.Forms.Button()
        Me.btnWS04Test1 = New System.Windows.Forms.Button()
        Me.btnWS04Test5 = New System.Windows.Forms.Button()
        Me.btnWS04Test4 = New System.Windows.Forms.Button()
        Me.btnWS04Test3 = New System.Windows.Forms.Button()
        Me.btnWS04Test0 = New System.Windows.Forms.Button()
        Me.TabPage10 = New System.Windows.Forms.TabPage()
        Me.dgvWS04GeneralSettings1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS04GeneralSettings0 = New System.Windows.Forms.DataGridView()
        Me.TabPage11 = New System.Windows.Forms.TabPage()
        Me.dgvWS04RearLeft_0 = New System.Windows.Forms.DataGridView()
        Me.dgvWS04RearLeft_1 = New System.Windows.Forms.DataGridView()
        Me.TabPage12 = New System.Windows.Forms.TabPage()
        Me.dgvWS04RearRight_1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS04RearRight_0 = New System.Windows.Forms.DataGridView()
        Me.TabPage13 = New System.Windows.Forms.TabPage()
        Me.dgvWS04MirrorFolding_1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS04MirrorFolding_0 = New System.Windows.Forms.DataGridView()
        Me.TabPage14 = New System.Windows.Forms.TabPage()
        Me.TabControl3 = New System.Windows.Forms.TabControl()
        Me.TabPage18 = New System.Windows.Forms.TabPage()
        Me.btnWS05Test2 = New System.Windows.Forms.Button()
        Me.btnWS05Test1 = New System.Windows.Forms.Button()
        Me.btnWS05Test0 = New System.Windows.Forms.Button()
        Me.TabPage21 = New System.Windows.Forms.TabPage()
        Me.dgvWS05GeneralSettings1 = New System.Windows.Forms.DataGridView()
        Me.dgvWS05GeneralSettings0 = New System.Windows.Forms.DataGridView()
        Me.TabPage22 = New System.Windows.Forms.TabPage()
        Me.dgvWS05Mirror_0 = New System.Windows.Forms.DataGridView()
        Me.dgvWS05Mirror_1 = New System.Windows.Forms.DataGridView()
        Me.lblRecipe = New System.Windows.Forms.Label()
        Me.btnCopyTo = New System.Windows.Forms.Button()
        Me.btnCheckMaster = New System.Windows.Forms.Button()
        Me.btnSaveMaster = New System.Windows.Forms.Button()
        Me.pbLogoValeo = New System.Windows.Forms.PictureBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CustomerLogo = New System.Windows.Forms.PictureBox()
        Me.btnWS02Test28 = New System.Windows.Forms.Button()
        Me.btnWS02Test29 = New System.Windows.Forms.Button()
        Me.btnWS02Test30 = New System.Windows.Forms.Button()
        Me.dgvWS02SHAPE2 = New System.Windows.Forms.DataGridView()
        Me.tcRecipe.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.tcStation.SuspendLayout()
        Me.tpTestEnables.SuspendLayout()
        Me.tpGeneralParameters.SuspendLayout()
        CType(Me.dgvWS02GeneralSettings1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS02GeneralSettings0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tp1.SuspendLayout()
        CType(Me.dgvWS02SHAPE1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tp2.SuspendLayout()
        CType(Me.dgvWS02BACKLIGHT_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS02BACKLIGHT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tp3.SuspendLayout()
        CType(Me.dgvWS02TELLTALE, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage15.SuspendLayout()
        CType(Me.dgvWS02EMSTraceability, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage16.SuspendLayout()
        Me.TabControl4.SuspendLayout()
        Me.TabPage24.SuspendLayout()
        CType(Me.dgvWS02DigitalOutputStdSignal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage25.SuspendLayout()
        CType(Me.dgvWS02DigitalOutputPWL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage26.SuspendLayout()
        CType(Me.dgvWS02DigitalOutputRTRVD, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage27.SuspendLayout()
        CType(Me.dgvWS02DigitalOutputRTRVG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage28.SuspendLayout()
        CType(Me.dgvWS02PWMOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage17.SuspendLayout()
        CType(Me.dgvWS02WriteTraceability, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage20.SuspendLayout()
        CType(Me.dgvWS02AnalogInput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage23.SuspendLayout()
        CType(Me.dgvWS02WriteConfiguration, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.dgvWS03GeneralSettings1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS03GeneralSettings0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage5.SuspendLayout()
        CType(Me.dgvWS03FrontLeft_0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS03FrontLeft_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage6.SuspendLayout()
        CType(Me.dgvWS03FrontRight_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS03FrontRight_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage29.SuspendLayout()
        CType(Me.dgvWS03JamaFunction, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage7.SuspendLayout()
        CType(Me.dgvWS03ChildrenLock_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS03ChildrenLock_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage8.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage9.SuspendLayout()
        Me.TabPage10.SuspendLayout()
        CType(Me.dgvWS04GeneralSettings1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS04GeneralSettings0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage11.SuspendLayout()
        CType(Me.dgvWS04RearLeft_0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS04RearLeft_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage12.SuspendLayout()
        CType(Me.dgvWS04RearRight_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS04RearRight_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage13.SuspendLayout()
        CType(Me.dgvWS04MirrorFolding_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS04MirrorFolding_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage14.SuspendLayout()
        Me.TabControl3.SuspendLayout()
        Me.TabPage18.SuspendLayout()
        Me.TabPage21.SuspendLayout()
        CType(Me.dgvWS05GeneralSettings1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS05GeneralSettings0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage22.SuspendLayout()
        CType(Me.dgvWS05Mirror_0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS05Mirror_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CustomerLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWS02SHAPE2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCreate
        '
        Me.btnCreate.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreate.Location = New System.Drawing.Point(1031, 114)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(100, 85)
        Me.btnCreate.TabIndex = 28
        Me.btnCreate.Text = "Create"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(1141, 114)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 85)
        Me.btnDelete.TabIndex = 29
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnCopyFrom
        '
        Me.btnCopyFrom.Enabled = False
        Me.btnCopyFrom.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCopyFrom.Location = New System.Drawing.Point(1251, 114)
        Me.btnCopyFrom.Name = "btnCopyFrom"
        Me.btnCopyFrom.Size = New System.Drawing.Size(100, 85)
        Me.btnCopyFrom.TabIndex = 30
        Me.btnCopyFrom.Text = "Copy from..."
        Me.btnCopyFrom.UseVisualStyleBackColor = True
        '
        'btnCancelModifies
        '
        Me.btnCancelModifies.Enabled = False
        Me.btnCancelModifies.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancelModifies.Location = New System.Drawing.Point(1468, 114)
        Me.btnCancelModifies.Name = "btnCancelModifies"
        Me.btnCancelModifies.Size = New System.Drawing.Size(100, 85)
        Me.btnCancelModifies.TabIndex = 31
        Me.btnCancelModifies.Text = "Cancel modifies"
        Me.btnCancelModifies.UseVisualStyleBackColor = True
        '
        'btnSaveModifies
        '
        Me.btnSaveModifies.Enabled = False
        Me.btnSaveModifies.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveModifies.Location = New System.Drawing.Point(1578, 114)
        Me.btnSaveModifies.Name = "btnSaveModifies"
        Me.btnSaveModifies.Size = New System.Drawing.Size(100, 85)
        Me.btnSaveModifies.TabIndex = 32
        Me.btnSaveModifies.Text = "Save modifies"
        Me.btnSaveModifies.UseVisualStyleBackColor = True
        '
        'lblReference
        '
        Me.lblReference.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReference.Location = New System.Drawing.Point(10, 134)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.Size = New System.Drawing.Size(160, 28)
        Me.lblReference.TabIndex = 34
        Me.lblReference.Text = "Reference code"
        Me.lblReference.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCreationDateDescription
        '
        Me.lblCreationDateDescription.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreationDateDescription.Location = New System.Drawing.Point(500, 115)
        Me.lblCreationDateDescription.Name = "lblCreationDateDescription"
        Me.lblCreationDateDescription.Size = New System.Drawing.Size(240, 28)
        Me.lblCreationDateDescription.TabIndex = 35
        Me.lblCreationDateDescription.Text = "Creation date and time"
        Me.lblCreationDateDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCreationDate
        '
        Me.lblCreationDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblCreationDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCreationDate.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreationDate.Location = New System.Drawing.Point(750, 115)
        Me.lblCreationDate.Name = "lblCreationDate"
        Me.lblCreationDate.Size = New System.Drawing.Size(240, 28)
        Me.lblCreationDate.TabIndex = 36
        Me.lblCreationDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblModifyDateDescription
        '
        Me.lblModifyDateDescription.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModifyDateDescription.Location = New System.Drawing.Point(500, 152)
        Me.lblModifyDateDescription.Name = "lblModifyDateDescription"
        Me.lblModifyDateDescription.Size = New System.Drawing.Size(240, 28)
        Me.lblModifyDateDescription.TabIndex = 37
        Me.lblModifyDateDescription.Text = "Creation date and time"
        Me.lblModifyDateDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblModifyDate
        '
        Me.lblModifyDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblModifyDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblModifyDate.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModifyDate.Location = New System.Drawing.Point(750, 152)
        Me.lblModifyDate.Name = "lblModifyDate"
        Me.lblModifyDate.Size = New System.Drawing.Size(240, 28)
        Me.lblModifyDate.TabIndex = 38
        Me.lblModifyDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSoftwareTitle
        '
        Me.lblSoftwareTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblSoftwareTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSoftwareTitle.Location = New System.Drawing.Point(200, 0)
        Me.lblSoftwareTitle.Name = "lblSoftwareTitle"
        Me.lblSoftwareTitle.Size = New System.Drawing.Size(1520, 46)
        Me.lblSoftwareTitle.TabIndex = 21
        Me.lblSoftwareTitle.Text = "Work Station 02 / 03 / 04 / 05"
        Me.lblSoftwareTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblFormTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormTitle.Location = New System.Drawing.Point(200, 46)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1520, 57)
        Me.lblFormTitle.TabIndex = 22
        Me.lblFormTitle.Text = "Recipes"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblECB
        '
        Me.lblECB.BackColor = System.Drawing.Color.White
        Me.lblECB.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblECB.ForeColor = System.Drawing.Color.Blue
        Me.lblECB.Location = New System.Drawing.Point(1666, 0)
        Me.lblECB.Name = "lblECB"
        Me.lblECB.Size = New System.Drawing.Size(254, 103)
        Me.lblECB.TabIndex = 23
        Me.lblECB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCity
        '
        Me.lblCity.BackColor = System.Drawing.Color.White
        Me.lblCity.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCity.ForeColor = System.Drawing.Color.Blue
        Me.lblCity.Location = New System.Drawing.Point(1720, 37)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(200, 28)
        Me.lblCity.TabIndex = 24
        Me.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.Color.White
        Me.lblCountry.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.Color.Blue
        Me.lblCountry.Location = New System.Drawing.Point(1720, 65)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.Size = New System.Drawing.Size(200, 39)
        Me.lblCountry.TabIndex = 25
        Me.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ssStatusBar
        '
        Me.ssStatusBar.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssStatusBar.Location = New System.Drawing.Point(0, 1058)
        Me.ssStatusBar.Name = "ssStatusBar"
        Me.ssStatusBar.Size = New System.Drawing.Size(1920, 22)
        Me.ssStatusBar.TabIndex = 26
        Me.ssStatusBar.Text = "StatusStrip1"
        '
        'tmrMonitor
        '
        '
        'tcRecipe
        '
        Me.tcRecipe.Controls.Add(Me.TabPage1)
        Me.tcRecipe.Controls.Add(Me.TabPage2)
        Me.tcRecipe.Controls.Add(Me.TabPage8)
        Me.tcRecipe.Controls.Add(Me.TabPage14)
        Me.tcRecipe.Font = New System.Drawing.Font("Arial Black", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcRecipe.ItemSize = New System.Drawing.Size(300, 40)
        Me.tcRecipe.Location = New System.Drawing.Point(14, 204)
        Me.tcRecipe.Name = "tcRecipe"
        Me.tcRecipe.SelectedIndex = 0
        Me.tcRecipe.Size = New System.Drawing.Size(1906, 850)
        Me.tcRecipe.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tcRecipe.TabIndex = 39
        Me.tcRecipe.Visible = False
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.tcStation)
        Me.TabPage1.Location = New System.Drawing.Point(4, 44)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1898, 802)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "WS 02"
        '
        'tcStation
        '
        Me.tcStation.Controls.Add(Me.tpTestEnables)
        Me.tcStation.Controls.Add(Me.tpGeneralParameters)
        Me.tcStation.Controls.Add(Me.tp1)
        Me.tcStation.Controls.Add(Me.tp2)
        Me.tcStation.Controls.Add(Me.tp3)
        Me.tcStation.Controls.Add(Me.TabPage15)
        Me.tcStation.Controls.Add(Me.TabPage16)
        Me.tcStation.Controls.Add(Me.TabPage28)
        Me.tcStation.Controls.Add(Me.TabPage17)
        Me.tcStation.Controls.Add(Me.TabPage19)
        Me.tcStation.Controls.Add(Me.TabPage20)
        Me.tcStation.Controls.Add(Me.TabPage23)
        Me.tcStation.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcStation.ItemSize = New System.Drawing.Size(314, 40)
        Me.tcStation.Location = New System.Drawing.Point(6, 8)
        Me.tcStation.Multiline = True
        Me.tcStation.Name = "tcStation"
        Me.tcStation.SelectedIndex = 0
        Me.tcStation.Size = New System.Drawing.Size(1889, 790)
        Me.tcStation.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tcStation.TabIndex = 40
        '
        'tpTestEnables
        '
        Me.tpTestEnables.BackColor = System.Drawing.SystemColors.Control
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test25)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test24)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test23)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test22)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test21)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test20)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test19)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test18)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test17)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test15)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test16)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test27)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test14)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test13)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test12)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test10)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test9)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test11)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test30)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test29)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test28)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test26)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test8)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test7)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test6)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test2)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test1)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test5)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test4)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test3)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test0)
        Me.tpTestEnables.Controls.Add(Me.btnWS02Test31)
        Me.tpTestEnables.Location = New System.Drawing.Point(4, 84)
        Me.tpTestEnables.Name = "tpTestEnables"
        Me.tpTestEnables.Padding = New System.Windows.Forms.Padding(3)
        Me.tpTestEnables.Size = New System.Drawing.Size(1881, 702)
        Me.tpTestEnables.TabIndex = 0
        Me.tpTestEnables.Text = "Test enables"
        '
        'btnWS02Test25
        '
        Me.btnWS02Test25.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test25.Location = New System.Drawing.Point(1506, 546)
        Me.btnWS02Test25.Name = "btnWS02Test25"
        Me.btnWS02Test25.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test25.TabIndex = 280
        Me.btnWS02Test25.UseVisualStyleBackColor = True
        '
        'btnWS02Test24
        '
        Me.btnWS02Test24.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test24.Location = New System.Drawing.Point(1506, 486)
        Me.btnWS02Test24.Name = "btnWS02Test24"
        Me.btnWS02Test24.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test24.TabIndex = 279
        Me.btnWS02Test24.UseVisualStyleBackColor = True
        '
        'btnWS02Test23
        '
        Me.btnWS02Test23.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test23.Location = New System.Drawing.Point(1506, 425)
        Me.btnWS02Test23.Name = "btnWS02Test23"
        Me.btnWS02Test23.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test23.TabIndex = 278
        Me.btnWS02Test23.UseVisualStyleBackColor = True
        '
        'btnWS02Test22
        '
        Me.btnWS02Test22.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test22.Location = New System.Drawing.Point(1506, 348)
        Me.btnWS02Test22.Name = "btnWS02Test22"
        Me.btnWS02Test22.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test22.TabIndex = 277
        Me.btnWS02Test22.UseVisualStyleBackColor = True
        '
        'btnWS02Test21
        '
        Me.btnWS02Test21.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test21.Location = New System.Drawing.Point(1506, 270)
        Me.btnWS02Test21.Name = "btnWS02Test21"
        Me.btnWS02Test21.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test21.TabIndex = 276
        Me.btnWS02Test21.UseVisualStyleBackColor = True
        '
        'btnWS02Test20
        '
        Me.btnWS02Test20.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test20.Location = New System.Drawing.Point(1506, 210)
        Me.btnWS02Test20.Name = "btnWS02Test20"
        Me.btnWS02Test20.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test20.TabIndex = 275
        Me.btnWS02Test20.UseVisualStyleBackColor = True
        '
        'btnWS02Test19
        '
        Me.btnWS02Test19.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test19.Location = New System.Drawing.Point(1506, 136)
        Me.btnWS02Test19.Name = "btnWS02Test19"
        Me.btnWS02Test19.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test19.TabIndex = 274
        Me.btnWS02Test19.UseVisualStyleBackColor = True
        '
        'btnWS02Test18
        '
        Me.btnWS02Test18.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test18.Location = New System.Drawing.Point(1506, 75)
        Me.btnWS02Test18.Name = "btnWS02Test18"
        Me.btnWS02Test18.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test18.TabIndex = 273
        Me.btnWS02Test18.UseVisualStyleBackColor = True
        '
        'btnWS02Test17
        '
        Me.btnWS02Test17.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test17.Location = New System.Drawing.Point(1506, 14)
        Me.btnWS02Test17.Name = "btnWS02Test17"
        Me.btnWS02Test17.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test17.TabIndex = 272
        Me.btnWS02Test17.UseVisualStyleBackColor = True
        '
        'btnWS02Test15
        '
        Me.btnWS02Test15.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test15.Location = New System.Drawing.Point(1139, 14)
        Me.btnWS02Test15.Name = "btnWS02Test15"
        Me.btnWS02Test15.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test15.TabIndex = 271
        Me.btnWS02Test15.UseVisualStyleBackColor = True
        '
        'btnWS02Test16
        '
        Me.btnWS02Test16.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test16.Location = New System.Drawing.Point(1139, 75)
        Me.btnWS02Test16.Name = "btnWS02Test16"
        Me.btnWS02Test16.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test16.TabIndex = 270
        Me.btnWS02Test16.UseVisualStyleBackColor = True
        '
        'btnWS02Test27
        '
        Me.btnWS02Test27.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test27.Location = New System.Drawing.Point(770, 382)
        Me.btnWS02Test27.Name = "btnWS02Test27"
        Me.btnWS02Test27.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test27.TabIndex = 269
        Me.btnWS02Test27.UseVisualStyleBackColor = True
        '
        'btnWS02Test14
        '
        Me.btnWS02Test14.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test14.Location = New System.Drawing.Point(770, 321)
        Me.btnWS02Test14.Name = "btnWS02Test14"
        Me.btnWS02Test14.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test14.TabIndex = 269
        Me.btnWS02Test14.UseVisualStyleBackColor = True
        '
        'btnWS02Test13
        '
        Me.btnWS02Test13.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test13.Location = New System.Drawing.Point(770, 260)
        Me.btnWS02Test13.Name = "btnWS02Test13"
        Me.btnWS02Test13.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test13.TabIndex = 268
        Me.btnWS02Test13.UseVisualStyleBackColor = True
        '
        'btnWS02Test12
        '
        Me.btnWS02Test12.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test12.Location = New System.Drawing.Point(770, 199)
        Me.btnWS02Test12.Name = "btnWS02Test12"
        Me.btnWS02Test12.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test12.TabIndex = 267
        Me.btnWS02Test12.UseVisualStyleBackColor = True
        '
        'btnWS02Test10
        '
        Me.btnWS02Test10.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test10.Location = New System.Drawing.Point(770, 75)
        Me.btnWS02Test10.Name = "btnWS02Test10"
        Me.btnWS02Test10.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test10.TabIndex = 27
        Me.btnWS02Test10.UseVisualStyleBackColor = True
        '
        'btnWS02Test9
        '
        Me.btnWS02Test9.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test9.Location = New System.Drawing.Point(770, 14)
        Me.btnWS02Test9.Name = "btnWS02Test9"
        Me.btnWS02Test9.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test9.TabIndex = 26
        Me.btnWS02Test9.UseVisualStyleBackColor = True
        '
        'btnWS02Test11
        '
        Me.btnWS02Test11.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test11.Location = New System.Drawing.Point(770, 136)
        Me.btnWS02Test11.Name = "btnWS02Test11"
        Me.btnWS02Test11.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test11.TabIndex = 23
        Me.btnWS02Test11.UseVisualStyleBackColor = True
        '
        'btnWS02Test26
        '
        Me.btnWS02Test26.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test26.Location = New System.Drawing.Point(396, 321)
        Me.btnWS02Test26.Name = "btnWS02Test26"
        Me.btnWS02Test26.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test26.TabIndex = 22
        Me.btnWS02Test26.UseVisualStyleBackColor = True
        '
        'btnWS02Test8
        '
        Me.btnWS02Test8.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test8.Location = New System.Drawing.Point(396, 260)
        Me.btnWS02Test8.Name = "btnWS02Test8"
        Me.btnWS02Test8.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test8.TabIndex = 22
        Me.btnWS02Test8.UseVisualStyleBackColor = True
        '
        'btnWS02Test7
        '
        Me.btnWS02Test7.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test7.Location = New System.Drawing.Point(396, 199)
        Me.btnWS02Test7.Name = "btnWS02Test7"
        Me.btnWS02Test7.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test7.TabIndex = 21
        Me.btnWS02Test7.UseVisualStyleBackColor = True
        '
        'btnWS02Test6
        '
        Me.btnWS02Test6.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test6.Location = New System.Drawing.Point(396, 136)
        Me.btnWS02Test6.Name = "btnWS02Test6"
        Me.btnWS02Test6.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test6.TabIndex = 20
        Me.btnWS02Test6.UseVisualStyleBackColor = True
        '
        'btnWS02Test2
        '
        Me.btnWS02Test2.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test2.Location = New System.Drawing.Point(18, 138)
        Me.btnWS02Test2.Name = "btnWS02Test2"
        Me.btnWS02Test2.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test2.TabIndex = 17
        Me.btnWS02Test2.UseVisualStyleBackColor = True
        '
        'btnWS02Test1
        '
        Me.btnWS02Test1.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test1.Location = New System.Drawing.Point(18, 75)
        Me.btnWS02Test1.Name = "btnWS02Test1"
        Me.btnWS02Test1.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test1.TabIndex = 16
        Me.btnWS02Test1.UseVisualStyleBackColor = True
        '
        'btnWS02Test5
        '
        Me.btnWS02Test5.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test5.Location = New System.Drawing.Point(396, 75)
        Me.btnWS02Test5.Name = "btnWS02Test5"
        Me.btnWS02Test5.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test5.TabIndex = 3
        Me.btnWS02Test5.UseVisualStyleBackColor = True
        '
        'btnWS02Test4
        '
        Me.btnWS02Test4.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test4.Location = New System.Drawing.Point(396, 14)
        Me.btnWS02Test4.Name = "btnWS02Test4"
        Me.btnWS02Test4.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test4.TabIndex = 2
        Me.btnWS02Test4.UseVisualStyleBackColor = True
        '
        'btnWS02Test3
        '
        Me.btnWS02Test3.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test3.Location = New System.Drawing.Point(18, 199)
        Me.btnWS02Test3.Name = "btnWS02Test3"
        Me.btnWS02Test3.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test3.TabIndex = 1
        Me.btnWS02Test3.UseVisualStyleBackColor = True
        '
        'btnWS02Test0
        '
        Me.btnWS02Test0.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test0.Location = New System.Drawing.Point(18, 14)
        Me.btnWS02Test0.Name = "btnWS02Test0"
        Me.btnWS02Test0.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test0.TabIndex = 0
        Me.btnWS02Test0.UseVisualStyleBackColor = True
        '
        'btnWS02Test31
        '
        Me.btnWS02Test31.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test31.Location = New System.Drawing.Point(1139, 138)
        Me.btnWS02Test31.Name = "btnWS02Test31"
        Me.btnWS02Test31.Size = New System.Drawing.Size(350, 53)
        Me.btnWS02Test31.TabIndex = 282
        Me.btnWS02Test31.UseVisualStyleBackColor = True
        '
        'tpGeneralParameters
        '
        Me.tpGeneralParameters.BackColor = System.Drawing.SystemColors.Control
        Me.tpGeneralParameters.Controls.Add(Me.dgvWS02GeneralSettings1)
        Me.tpGeneralParameters.Controls.Add(Me.dgvWS02GeneralSettings0)
        Me.tpGeneralParameters.Location = New System.Drawing.Point(4, 84)
        Me.tpGeneralParameters.Name = "tpGeneralParameters"
        Me.tpGeneralParameters.Padding = New System.Windows.Forms.Padding(3)
        Me.tpGeneralParameters.Size = New System.Drawing.Size(1881, 702)
        Me.tpGeneralParameters.TabIndex = 1
        Me.tpGeneralParameters.Text = "General parameters"
        '
        'dgvWS02GeneralSettings1
        '
        Me.dgvWS02GeneralSettings1.AllowUserToAddRows = False
        Me.dgvWS02GeneralSettings1.AllowUserToDeleteRows = False
        Me.dgvWS02GeneralSettings1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02GeneralSettings1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvWS02GeneralSettings1.Location = New System.Drawing.Point(958, 3)
        Me.dgvWS02GeneralSettings1.Name = "dgvWS02GeneralSettings1"
        Me.dgvWS02GeneralSettings1.ReadOnly = True
        Me.dgvWS02GeneralSettings1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02GeneralSettings1.Size = New System.Drawing.Size(917, 83)
        Me.dgvWS02GeneralSettings1.TabIndex = 1
        '
        'dgvWS02GeneralSettings0
        '
        Me.dgvWS02GeneralSettings0.AllowUserToAddRows = False
        Me.dgvWS02GeneralSettings0.AllowUserToDeleteRows = False
        Me.dgvWS02GeneralSettings0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02GeneralSettings0.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvWS02GeneralSettings0.Location = New System.Drawing.Point(6, 3)
        Me.dgvWS02GeneralSettings0.Name = "dgvWS02GeneralSettings0"
        Me.dgvWS02GeneralSettings0.ReadOnly = True
        Me.dgvWS02GeneralSettings0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02GeneralSettings0.Size = New System.Drawing.Size(917, 83)
        Me.dgvWS02GeneralSettings0.TabIndex = 0
        '
        'tp1
        '
        Me.tp1.BackColor = System.Drawing.SystemColors.Control
        Me.tp1.Controls.Add(Me.dgvWS02SHAPE2)
        Me.tp1.Controls.Add(Me.dgvWS02SHAPE1)
        Me.tp1.Location = New System.Drawing.Point(4, 84)
        Me.tp1.Name = "tp1"
        Me.tp1.Size = New System.Drawing.Size(1881, 702)
        Me.tp1.TabIndex = 3
        Me.tp1.Text = "Test SHAPE"
        '
        'dgvWS02SHAPE1
        '
        Me.dgvWS02SHAPE1.AllowUserToAddRows = False
        Me.dgvWS02SHAPE1.AllowUserToDeleteRows = False
        Me.dgvWS02SHAPE1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02SHAPE1.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02SHAPE1.Name = "dgvWS02SHAPE1"
        Me.dgvWS02SHAPE1.ReadOnly = True
        Me.dgvWS02SHAPE1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02SHAPE1.Size = New System.Drawing.Size(923, 415)
        Me.dgvWS02SHAPE1.TabIndex = 6
        '
        'tp2
        '
        Me.tp2.BackColor = System.Drawing.SystemColors.Control
        Me.tp2.Controls.Add(Me.dgvWS02BACKLIGHT_1)
        Me.tp2.Controls.Add(Me.dgvWS02BACKLIGHT)
        Me.tp2.Location = New System.Drawing.Point(4, 84)
        Me.tp2.Name = "tp2"
        Me.tp2.Size = New System.Drawing.Size(1881, 702)
        Me.tp2.TabIndex = 4
        Me.tp2.Text = "Test BACKLIGHT"
        '
        'dgvWS02BACKLIGHT_1
        '
        Me.dgvWS02BACKLIGHT_1.AllowUserToAddRows = False
        Me.dgvWS02BACKLIGHT_1.AllowUserToDeleteRows = False
        Me.dgvWS02BACKLIGHT_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02BACKLIGHT_1.Location = New System.Drawing.Point(969, 3)
        Me.dgvWS02BACKLIGHT_1.Name = "dgvWS02BACKLIGHT_1"
        Me.dgvWS02BACKLIGHT_1.ReadOnly = True
        Me.dgvWS02BACKLIGHT_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02BACKLIGHT_1.Size = New System.Drawing.Size(883, 415)
        Me.dgvWS02BACKLIGHT_1.TabIndex = 9
        '
        'dgvWS02BACKLIGHT
        '
        Me.dgvWS02BACKLIGHT.AllowUserToAddRows = False
        Me.dgvWS02BACKLIGHT.AllowUserToDeleteRows = False
        Me.dgvWS02BACKLIGHT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02BACKLIGHT.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02BACKLIGHT.Name = "dgvWS02BACKLIGHT"
        Me.dgvWS02BACKLIGHT.ReadOnly = True
        Me.dgvWS02BACKLIGHT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02BACKLIGHT.Size = New System.Drawing.Size(891, 415)
        Me.dgvWS02BACKLIGHT.TabIndex = 8
        '
        'tp3
        '
        Me.tp3.BackColor = System.Drawing.SystemColors.Control
        Me.tp3.Controls.Add(Me.dgvWS02TELLTALE)
        Me.tp3.Location = New System.Drawing.Point(4, 84)
        Me.tp3.Name = "tp3"
        Me.tp3.Size = New System.Drawing.Size(1881, 702)
        Me.tp3.TabIndex = 5
        Me.tp3.Text = "Test TELLTALE"
        '
        'dgvWS02TELLTALE
        '
        Me.dgvWS02TELLTALE.AllowUserToAddRows = False
        Me.dgvWS02TELLTALE.AllowUserToDeleteRows = False
        Me.dgvWS02TELLTALE.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02TELLTALE.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02TELLTALE.Name = "dgvWS02TELLTALE"
        Me.dgvWS02TELLTALE.ReadOnly = True
        Me.dgvWS02TELLTALE.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02TELLTALE.Size = New System.Drawing.Size(924, 415)
        Me.dgvWS02TELLTALE.TabIndex = 6
        '
        'TabPage15
        '
        Me.TabPage15.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage15.Controls.Add(Me.dgvWS02EMSTraceability)
        Me.TabPage15.Location = New System.Drawing.Point(4, 84)
        Me.TabPage15.Name = "TabPage15"
        Me.TabPage15.Size = New System.Drawing.Size(1881, 702)
        Me.TabPage15.TabIndex = 6
        Me.TabPage15.Text = "EMS TRACEABILITY"
        '
        'dgvWS02EMSTraceability
        '
        Me.dgvWS02EMSTraceability.AllowUserToAddRows = False
        Me.dgvWS02EMSTraceability.AllowUserToDeleteRows = False
        Me.dgvWS02EMSTraceability.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02EMSTraceability.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02EMSTraceability.Name = "dgvWS02EMSTraceability"
        Me.dgvWS02EMSTraceability.ReadOnly = True
        Me.dgvWS02EMSTraceability.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02EMSTraceability.Size = New System.Drawing.Size(1251, 415)
        Me.dgvWS02EMSTraceability.TabIndex = 6
        '
        'TabPage16
        '
        Me.TabPage16.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage16.Controls.Add(Me.TabControl4)
        Me.TabPage16.Location = New System.Drawing.Point(4, 84)
        Me.TabPage16.Name = "TabPage16"
        Me.TabPage16.Size = New System.Drawing.Size(1881, 702)
        Me.TabPage16.TabIndex = 7
        Me.TabPage16.Text = "DIGITAL OUTPUT"
        '
        'TabControl4
        '
        Me.TabControl4.Controls.Add(Me.TabPage24)
        Me.TabControl4.Controls.Add(Me.TabPage25)
        Me.TabControl4.Controls.Add(Me.TabPage26)
        Me.TabControl4.Controls.Add(Me.TabPage27)
        Me.TabControl4.ItemSize = New System.Drawing.Size(150, 40)
        Me.TabControl4.Location = New System.Drawing.Point(3, 3)
        Me.TabControl4.Name = "TabControl4"
        Me.TabControl4.SelectedIndex = 0
        Me.TabControl4.Size = New System.Drawing.Size(1875, 671)
        Me.TabControl4.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl4.TabIndex = 9
        '
        'TabPage24
        '
        Me.TabPage24.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage24.Controls.Add(Me.dgvWS02DigitalOutputStdSignal)
        Me.TabPage24.Location = New System.Drawing.Point(4, 44)
        Me.TabPage24.Name = "TabPage24"
        Me.TabPage24.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage24.Size = New System.Drawing.Size(1867, 623)
        Me.TabPage24.TabIndex = 0
        Me.TabPage24.Text = "Std Signal"
        '
        'dgvWS02DigitalOutputStdSignal
        '
        Me.dgvWS02DigitalOutputStdSignal.AllowUserToAddRows = False
        Me.dgvWS02DigitalOutputStdSignal.AllowUserToDeleteRows = False
        Me.dgvWS02DigitalOutputStdSignal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02DigitalOutputStdSignal.Location = New System.Drawing.Point(6, 6)
        Me.dgvWS02DigitalOutputStdSignal.Name = "dgvWS02DigitalOutputStdSignal"
        Me.dgvWS02DigitalOutputStdSignal.ReadOnly = True
        Me.dgvWS02DigitalOutputStdSignal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02DigitalOutputStdSignal.Size = New System.Drawing.Size(1300, 46)
        Me.dgvWS02DigitalOutputStdSignal.TabIndex = 8
        '
        'TabPage25
        '
        Me.TabPage25.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage25.Controls.Add(Me.dgvWS02DigitalOutputPWL)
        Me.TabPage25.Location = New System.Drawing.Point(4, 44)
        Me.TabPage25.Name = "TabPage25"
        Me.TabPage25.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage25.Size = New System.Drawing.Size(1867, 623)
        Me.TabPage25.TabIndex = 1
        Me.TabPage25.Text = "PWL Option"
        '
        'dgvWS02DigitalOutputPWL
        '
        Me.dgvWS02DigitalOutputPWL.AllowUserToAddRows = False
        Me.dgvWS02DigitalOutputPWL.AllowUserToDeleteRows = False
        Me.dgvWS02DigitalOutputPWL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02DigitalOutputPWL.Location = New System.Drawing.Point(6, 6)
        Me.dgvWS02DigitalOutputPWL.Name = "dgvWS02DigitalOutputPWL"
        Me.dgvWS02DigitalOutputPWL.ReadOnly = True
        Me.dgvWS02DigitalOutputPWL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02DigitalOutputPWL.Size = New System.Drawing.Size(1300, 46)
        Me.dgvWS02DigitalOutputPWL.TabIndex = 7
        '
        'TabPage26
        '
        Me.TabPage26.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage26.Controls.Add(Me.dgvWS02DigitalOutputRTRVD)
        Me.TabPage26.Location = New System.Drawing.Point(4, 44)
        Me.TabPage26.Name = "TabPage26"
        Me.TabPage26.Size = New System.Drawing.Size(1867, 623)
        Me.TabPage26.TabIndex = 2
        Me.TabPage26.Text = "RTRV D"
        '
        'dgvWS02DigitalOutputRTRVD
        '
        Me.dgvWS02DigitalOutputRTRVD.AllowUserToAddRows = False
        Me.dgvWS02DigitalOutputRTRVD.AllowUserToDeleteRows = False
        Me.dgvWS02DigitalOutputRTRVD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02DigitalOutputRTRVD.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02DigitalOutputRTRVD.Name = "dgvWS02DigitalOutputRTRVD"
        Me.dgvWS02DigitalOutputRTRVD.ReadOnly = True
        Me.dgvWS02DigitalOutputRTRVD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02DigitalOutputRTRVD.Size = New System.Drawing.Size(1300, 46)
        Me.dgvWS02DigitalOutputRTRVD.TabIndex = 8
        '
        'TabPage27
        '
        Me.TabPage27.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage27.Controls.Add(Me.dgvWS02DigitalOutputRTRVG)
        Me.TabPage27.Location = New System.Drawing.Point(4, 44)
        Me.TabPage27.Name = "TabPage27"
        Me.TabPage27.Size = New System.Drawing.Size(1867, 623)
        Me.TabPage27.TabIndex = 3
        Me.TabPage27.Text = "RTRV G"
        '
        'dgvWS02DigitalOutputRTRVG
        '
        Me.dgvWS02DigitalOutputRTRVG.AllowUserToAddRows = False
        Me.dgvWS02DigitalOutputRTRVG.AllowUserToDeleteRows = False
        Me.dgvWS02DigitalOutputRTRVG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02DigitalOutputRTRVG.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02DigitalOutputRTRVG.Name = "dgvWS02DigitalOutputRTRVG"
        Me.dgvWS02DigitalOutputRTRVG.ReadOnly = True
        Me.dgvWS02DigitalOutputRTRVG.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02DigitalOutputRTRVG.Size = New System.Drawing.Size(1300, 46)
        Me.dgvWS02DigitalOutputRTRVG.TabIndex = 8
        '
        'TabPage28
        '
        Me.TabPage28.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage28.Controls.Add(Me.dgvWS02PWMOutput)
        Me.TabPage28.Location = New System.Drawing.Point(4, 84)
        Me.TabPage28.Name = "TabPage28"
        Me.TabPage28.Size = New System.Drawing.Size(1881, 702)
        Me.TabPage28.TabIndex = 12
        Me.TabPage28.Text = "PWM OUTPUT"
        '
        'dgvWS02PWMOutput
        '
        Me.dgvWS02PWMOutput.AllowUserToAddRows = False
        Me.dgvWS02PWMOutput.AllowUserToDeleteRows = False
        Me.dgvWS02PWMOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02PWMOutput.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02PWMOutput.Name = "dgvWS02PWMOutput"
        Me.dgvWS02PWMOutput.ReadOnly = True
        Me.dgvWS02PWMOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02PWMOutput.Size = New System.Drawing.Size(1065, 46)
        Me.dgvWS02PWMOutput.TabIndex = 9
        '
        'TabPage17
        '
        Me.TabPage17.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage17.Controls.Add(Me.dgvWS02WriteTraceability)
        Me.TabPage17.Location = New System.Drawing.Point(4, 84)
        Me.TabPage17.Name = "TabPage17"
        Me.TabPage17.Size = New System.Drawing.Size(1881, 702)
        Me.TabPage17.TabIndex = 8
        Me.TabPage17.Text = "Write Traceability"
        '
        'dgvWS02WriteTraceability
        '
        Me.dgvWS02WriteTraceability.AllowUserToAddRows = False
        Me.dgvWS02WriteTraceability.AllowUserToDeleteRows = False
        Me.dgvWS02WriteTraceability.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02WriteTraceability.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02WriteTraceability.Name = "dgvWS02WriteTraceability"
        Me.dgvWS02WriteTraceability.ReadOnly = True
        Me.dgvWS02WriteTraceability.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02WriteTraceability.Size = New System.Drawing.Size(1251, 415)
        Me.dgvWS02WriteTraceability.TabIndex = 8
        '
        'TabPage19
        '
        Me.TabPage19.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage19.Location = New System.Drawing.Point(4, 84)
        Me.TabPage19.Name = "TabPage19"
        Me.TabPage19.Size = New System.Drawing.Size(1881, 702)
        Me.TabPage19.TabIndex = 9
        Me.TabPage19.Text = "CURRENT COMSUPTION"
        '
        'TabPage20
        '
        Me.TabPage20.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage20.Controls.Add(Me.dgvWS02AnalogInput)
        Me.TabPage20.Location = New System.Drawing.Point(4, 84)
        Me.TabPage20.Name = "TabPage20"
        Me.TabPage20.Size = New System.Drawing.Size(1881, 702)
        Me.TabPage20.TabIndex = 10
        Me.TabPage20.Text = "Analog Input"
        '
        'dgvWS02AnalogInput
        '
        Me.dgvWS02AnalogInput.AllowUserToAddRows = False
        Me.dgvWS02AnalogInput.AllowUserToDeleteRows = False
        Me.dgvWS02AnalogInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02AnalogInput.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02AnalogInput.Name = "dgvWS02AnalogInput"
        Me.dgvWS02AnalogInput.ReadOnly = True
        Me.dgvWS02AnalogInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02AnalogInput.Size = New System.Drawing.Size(1252, 415)
        Me.dgvWS02AnalogInput.TabIndex = 7
        '
        'TabPage23
        '
        Me.TabPage23.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage23.Controls.Add(Me.dgvWS02WriteConfiguration)
        Me.TabPage23.Location = New System.Drawing.Point(4, 84)
        Me.TabPage23.Name = "TabPage23"
        Me.TabPage23.Size = New System.Drawing.Size(1881, 702)
        Me.TabPage23.TabIndex = 11
        Me.TabPage23.Text = "WRITE Configuration"
        '
        'dgvWS02WriteConfiguration
        '
        Me.dgvWS02WriteConfiguration.AllowUserToAddRows = False
        Me.dgvWS02WriteConfiguration.AllowUserToDeleteRows = False
        Me.dgvWS02WriteConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02WriteConfiguration.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS02WriteConfiguration.Name = "dgvWS02WriteConfiguration"
        Me.dgvWS02WriteConfiguration.ReadOnly = True
        Me.dgvWS02WriteConfiguration.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02WriteConfiguration.Size = New System.Drawing.Size(1251, 415)
        Me.dgvWS02WriteConfiguration.TabIndex = 9
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage2.Controls.Add(Me.TabControl1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 44)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1898, 802)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "WS 03"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage29)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(270, 35)
        Me.TabControl1.Location = New System.Drawing.Point(0, 8)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1898, 750)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl1.TabIndex = 41
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage3.Controls.Add(Me.btnWS03Test12)
        Me.TabPage3.Controls.Add(Me.btnWS03Test11)
        Me.TabPage3.Controls.Add(Me.btnWS03Test10)
        Me.TabPage3.Controls.Add(Me.btnWS03Test9)
        Me.TabPage3.Controls.Add(Me.btnWS03Test8)
        Me.TabPage3.Controls.Add(Me.btnWS03Test7)
        Me.TabPage3.Controls.Add(Me.btnWS03Test6)
        Me.TabPage3.Controls.Add(Me.btnWS03Test2)
        Me.TabPage3.Controls.Add(Me.btnWS03Test1)
        Me.TabPage3.Controls.Add(Me.btnWS03Test5)
        Me.TabPage3.Controls.Add(Me.btnWS03Test4)
        Me.TabPage3.Controls.Add(Me.btnWS03Test3)
        Me.TabPage3.Controls.Add(Me.btnWS03Test0)
        Me.TabPage3.Location = New System.Drawing.Point(4, 39)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(1890, 707)
        Me.TabPage3.TabIndex = 0
        Me.TabPage3.Text = "Test enables"
        '
        'btnWS03Test12
        '
        Me.btnWS03Test12.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test12.Location = New System.Drawing.Point(19, 139)
        Me.btnWS03Test12.Name = "btnWS03Test12"
        Me.btnWS03Test12.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test12.TabIndex = 29
        Me.btnWS03Test12.UseVisualStyleBackColor = True
        '
        'btnWS03Test11
        '
        Me.btnWS03Test11.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test11.Location = New System.Drawing.Point(19, 78)
        Me.btnWS03Test11.Name = "btnWS03Test11"
        Me.btnWS03Test11.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test11.TabIndex = 28
        Me.btnWS03Test11.UseVisualStyleBackColor = True
        '
        'btnWS03Test10
        '
        Me.btnWS03Test10.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test10.Location = New System.Drawing.Point(19, 18)
        Me.btnWS03Test10.Name = "btnWS03Test10"
        Me.btnWS03Test10.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test10.TabIndex = 27
        Me.btnWS03Test10.UseVisualStyleBackColor = True
        '
        'btnWS03Test9
        '
        Me.btnWS03Test9.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test9.Location = New System.Drawing.Point(1314, 78)
        Me.btnWS03Test9.Name = "btnWS03Test9"
        Me.btnWS03Test9.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test9.TabIndex = 26
        Me.btnWS03Test9.UseVisualStyleBackColor = True
        '
        'btnWS03Test8
        '
        Me.btnWS03Test8.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test8.Location = New System.Drawing.Point(1314, 18)
        Me.btnWS03Test8.Name = "btnWS03Test8"
        Me.btnWS03Test8.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test8.TabIndex = 22
        Me.btnWS03Test8.UseVisualStyleBackColor = True
        '
        'btnWS03Test7
        '
        Me.btnWS03Test7.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test7.Location = New System.Drawing.Point(871, 200)
        Me.btnWS03Test7.Name = "btnWS03Test7"
        Me.btnWS03Test7.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test7.TabIndex = 21
        Me.btnWS03Test7.UseVisualStyleBackColor = True
        '
        'btnWS03Test6
        '
        Me.btnWS03Test6.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test6.Location = New System.Drawing.Point(871, 139)
        Me.btnWS03Test6.Name = "btnWS03Test6"
        Me.btnWS03Test6.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test6.TabIndex = 20
        Me.btnWS03Test6.UseVisualStyleBackColor = True
        '
        'btnWS03Test2
        '
        Me.btnWS03Test2.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test2.Location = New System.Drawing.Point(423, 139)
        Me.btnWS03Test2.Name = "btnWS03Test2"
        Me.btnWS03Test2.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test2.TabIndex = 17
        Me.btnWS03Test2.UseVisualStyleBackColor = True
        '
        'btnWS03Test1
        '
        Me.btnWS03Test1.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test1.Location = New System.Drawing.Point(423, 78)
        Me.btnWS03Test1.Name = "btnWS03Test1"
        Me.btnWS03Test1.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test1.TabIndex = 16
        Me.btnWS03Test1.UseVisualStyleBackColor = True
        '
        'btnWS03Test5
        '
        Me.btnWS03Test5.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test5.Location = New System.Drawing.Point(871, 79)
        Me.btnWS03Test5.Name = "btnWS03Test5"
        Me.btnWS03Test5.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test5.TabIndex = 3
        Me.btnWS03Test5.UseVisualStyleBackColor = True
        '
        'btnWS03Test4
        '
        Me.btnWS03Test4.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test4.Location = New System.Drawing.Point(871, 18)
        Me.btnWS03Test4.Name = "btnWS03Test4"
        Me.btnWS03Test4.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test4.TabIndex = 2
        Me.btnWS03Test4.UseVisualStyleBackColor = True
        '
        'btnWS03Test3
        '
        Me.btnWS03Test3.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test3.Location = New System.Drawing.Point(423, 200)
        Me.btnWS03Test3.Name = "btnWS03Test3"
        Me.btnWS03Test3.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test3.TabIndex = 1
        Me.btnWS03Test3.UseVisualStyleBackColor = True
        '
        'btnWS03Test0
        '
        Me.btnWS03Test0.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS03Test0.Location = New System.Drawing.Point(423, 18)
        Me.btnWS03Test0.Name = "btnWS03Test0"
        Me.btnWS03Test0.Size = New System.Drawing.Size(350, 55)
        Me.btnWS03Test0.TabIndex = 0
        Me.btnWS03Test0.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage4.Controls.Add(Me.dgvWS03GeneralSettings1)
        Me.TabPage4.Controls.Add(Me.dgvWS03GeneralSettings0)
        Me.TabPage4.Location = New System.Drawing.Point(4, 39)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(1890, 707)
        Me.TabPage4.TabIndex = 1
        Me.TabPage4.Text = "General parameters"
        '
        'dgvWS03GeneralSettings1
        '
        Me.dgvWS03GeneralSettings1.AllowUserToAddRows = False
        Me.dgvWS03GeneralSettings1.AllowUserToDeleteRows = False
        Me.dgvWS03GeneralSettings1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03GeneralSettings1.Location = New System.Drawing.Point(955, 3)
        Me.dgvWS03GeneralSettings1.Name = "dgvWS03GeneralSettings1"
        Me.dgvWS03GeneralSettings1.ReadOnly = True
        Me.dgvWS03GeneralSettings1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03GeneralSettings1.Size = New System.Drawing.Size(917, 660)
        Me.dgvWS03GeneralSettings1.TabIndex = 1
        '
        'dgvWS03GeneralSettings0
        '
        Me.dgvWS03GeneralSettings0.AllowUserToAddRows = False
        Me.dgvWS03GeneralSettings0.AllowUserToDeleteRows = False
        Me.dgvWS03GeneralSettings0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03GeneralSettings0.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvWS03GeneralSettings0.Location = New System.Drawing.Point(6, 3)
        Me.dgvWS03GeneralSettings0.Name = "dgvWS03GeneralSettings0"
        Me.dgvWS03GeneralSettings0.ReadOnly = True
        Me.dgvWS03GeneralSettings0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03GeneralSettings0.Size = New System.Drawing.Size(917, 83)
        Me.dgvWS03GeneralSettings0.TabIndex = 0
        Me.dgvWS03GeneralSettings0.Visible = False
        '
        'TabPage5
        '
        Me.TabPage5.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage5.Controls.Add(Me.dgvWS03FrontLeft_0)
        Me.TabPage5.Controls.Add(Me.dgvWS03FrontLeft_1)
        Me.TabPage5.Location = New System.Drawing.Point(4, 39)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(1890, 707)
        Me.TabPage5.TabIndex = 3
        Me.TabPage5.Text = "Test Front Left"
        '
        'dgvWS03FrontLeft_0
        '
        Me.dgvWS03FrontLeft_0.AllowUserToAddRows = False
        Me.dgvWS03FrontLeft_0.AllowUserToDeleteRows = False
        Me.dgvWS03FrontLeft_0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03FrontLeft_0.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS03FrontLeft_0.Name = "dgvWS03FrontLeft_0"
        Me.dgvWS03FrontLeft_0.ReadOnly = True
        Me.dgvWS03FrontLeft_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03FrontLeft_0.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS03FrontLeft_0.TabIndex = 4
        '
        'dgvWS03FrontLeft_1
        '
        Me.dgvWS03FrontLeft_1.AllowUserToAddRows = False
        Me.dgvWS03FrontLeft_1.AllowUserToDeleteRows = False
        Me.dgvWS03FrontLeft_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03FrontLeft_1.Location = New System.Drawing.Point(959, 3)
        Me.dgvWS03FrontLeft_1.Name = "dgvWS03FrontLeft_1"
        Me.dgvWS03FrontLeft_1.ReadOnly = True
        Me.dgvWS03FrontLeft_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03FrontLeft_1.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS03FrontLeft_1.TabIndex = 3
        '
        'TabPage6
        '
        Me.TabPage6.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage6.Controls.Add(Me.dgvWS03FrontRight_1)
        Me.TabPage6.Controls.Add(Me.dgvWS03FrontRight_0)
        Me.TabPage6.Location = New System.Drawing.Point(4, 39)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(1890, 707)
        Me.TabPage6.TabIndex = 4
        Me.TabPage6.Text = "Test Front Right"
        '
        'dgvWS03FrontRight_1
        '
        Me.dgvWS03FrontRight_1.AllowUserToAddRows = False
        Me.dgvWS03FrontRight_1.AllowUserToDeleteRows = False
        Me.dgvWS03FrontRight_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03FrontRight_1.Location = New System.Drawing.Point(960, 3)
        Me.dgvWS03FrontRight_1.Name = "dgvWS03FrontRight_1"
        Me.dgvWS03FrontRight_1.ReadOnly = True
        Me.dgvWS03FrontRight_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03FrontRight_1.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS03FrontRight_1.TabIndex = 5
        '
        'dgvWS03FrontRight_0
        '
        Me.dgvWS03FrontRight_0.AllowUserToAddRows = False
        Me.dgvWS03FrontRight_0.AllowUserToDeleteRows = False
        Me.dgvWS03FrontRight_0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03FrontRight_0.Location = New System.Drawing.Point(4, 3)
        Me.dgvWS03FrontRight_0.Name = "dgvWS03FrontRight_0"
        Me.dgvWS03FrontRight_0.ReadOnly = True
        Me.dgvWS03FrontRight_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03FrontRight_0.Size = New System.Drawing.Size(937, 415)
        Me.dgvWS03FrontRight_0.TabIndex = 4
        '
        'TabPage29
        '
        Me.TabPage29.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage29.Controls.Add(Me.dgvWS03JamaFunction)
        Me.TabPage29.Location = New System.Drawing.Point(4, 39)
        Me.TabPage29.Name = "TabPage29"
        Me.TabPage29.Size = New System.Drawing.Size(1890, 707)
        Me.TabPage29.TabIndex = 6
        Me.TabPage29.Text = "JAMA FUNCTION"
        '
        'dgvWS03JamaFunction
        '
        Me.dgvWS03JamaFunction.AllowUserToAddRows = False
        Me.dgvWS03JamaFunction.AllowUserToDeleteRows = False
        Me.dgvWS03JamaFunction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03JamaFunction.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS03JamaFunction.Name = "dgvWS03JamaFunction"
        Me.dgvWS03JamaFunction.ReadOnly = True
        Me.dgvWS03JamaFunction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03JamaFunction.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS03JamaFunction.TabIndex = 5
        '
        'TabPage7
        '
        Me.TabPage7.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage7.Controls.Add(Me.dgvWS03ChildrenLock_1)
        Me.TabPage7.Controls.Add(Me.dgvWS03ChildrenLock_0)
        Me.TabPage7.Location = New System.Drawing.Point(4, 39)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(1890, 707)
        Me.TabPage7.TabIndex = 5
        Me.TabPage7.Text = "Test Children Lock"
        '
        'dgvWS03ChildrenLock_1
        '
        Me.dgvWS03ChildrenLock_1.AllowUserToAddRows = False
        Me.dgvWS03ChildrenLock_1.AllowUserToDeleteRows = False
        Me.dgvWS03ChildrenLock_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03ChildrenLock_1.Location = New System.Drawing.Point(960, 3)
        Me.dgvWS03ChildrenLock_1.Name = "dgvWS03ChildrenLock_1"
        Me.dgvWS03ChildrenLock_1.ReadOnly = True
        Me.dgvWS03ChildrenLock_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03ChildrenLock_1.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS03ChildrenLock_1.TabIndex = 5
        '
        'dgvWS03ChildrenLock_0
        '
        Me.dgvWS03ChildrenLock_0.AllowUserToAddRows = False
        Me.dgvWS03ChildrenLock_0.AllowUserToDeleteRows = False
        Me.dgvWS03ChildrenLock_0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS03ChildrenLock_0.Location = New System.Drawing.Point(4, 3)
        Me.dgvWS03ChildrenLock_0.Name = "dgvWS03ChildrenLock_0"
        Me.dgvWS03ChildrenLock_0.ReadOnly = True
        Me.dgvWS03ChildrenLock_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS03ChildrenLock_0.Size = New System.Drawing.Size(937, 415)
        Me.dgvWS03ChildrenLock_0.TabIndex = 4
        '
        'TabPage8
        '
        Me.TabPage8.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage8.Controls.Add(Me.TabControl2)
        Me.TabPage8.Location = New System.Drawing.Point(4, 44)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(1898, 802)
        Me.TabPage8.TabIndex = 2
        Me.TabPage8.Text = "WS 04"
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage9)
        Me.TabControl2.Controls.Add(Me.TabPage10)
        Me.TabControl2.Controls.Add(Me.TabPage11)
        Me.TabControl2.Controls.Add(Me.TabPage12)
        Me.TabControl2.Controls.Add(Me.TabPage13)
        Me.TabControl2.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl2.ItemSize = New System.Drawing.Size(270, 35)
        Me.TabControl2.Location = New System.Drawing.Point(4, 3)
        Me.TabControl2.Multiline = True
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(1898, 755)
        Me.TabControl2.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl2.TabIndex = 42
        '
        'TabPage9
        '
        Me.TabPage9.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage9.Controls.Add(Me.btnWS04Test10)
        Me.TabPage9.Controls.Add(Me.btnWS04Test9)
        Me.TabPage9.Controls.Add(Me.btnWS04Test8)
        Me.TabPage9.Controls.Add(Me.btnWS04Test7)
        Me.TabPage9.Controls.Add(Me.btnWS04Test6)
        Me.TabPage9.Controls.Add(Me.btnWS04Test2)
        Me.TabPage9.Controls.Add(Me.btnWS04Test1)
        Me.TabPage9.Controls.Add(Me.btnWS04Test5)
        Me.TabPage9.Controls.Add(Me.btnWS04Test4)
        Me.TabPage9.Controls.Add(Me.btnWS04Test3)
        Me.TabPage9.Controls.Add(Me.btnWS04Test0)
        Me.TabPage9.Location = New System.Drawing.Point(4, 39)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage9.Size = New System.Drawing.Size(1890, 712)
        Me.TabPage9.TabIndex = 0
        Me.TabPage9.Text = "Test enables"
        '
        'btnWS04Test10
        '
        Me.btnWS04Test10.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test10.Location = New System.Drawing.Point(17, 16)
        Me.btnWS04Test10.Name = "btnWS04Test10"
        Me.btnWS04Test10.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test10.TabIndex = 27
        Me.btnWS04Test10.UseVisualStyleBackColor = True
        '
        'btnWS04Test9
        '
        Me.btnWS04Test9.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test9.Location = New System.Drawing.Point(1308, 77)
        Me.btnWS04Test9.Name = "btnWS04Test9"
        Me.btnWS04Test9.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test9.TabIndex = 26
        Me.btnWS04Test9.UseVisualStyleBackColor = True
        '
        'btnWS04Test8
        '
        Me.btnWS04Test8.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test8.Location = New System.Drawing.Point(1308, 16)
        Me.btnWS04Test8.Name = "btnWS04Test8"
        Me.btnWS04Test8.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test8.TabIndex = 22
        Me.btnWS04Test8.UseVisualStyleBackColor = True
        '
        'btnWS04Test7
        '
        Me.btnWS04Test7.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test7.Location = New System.Drawing.Point(865, 198)
        Me.btnWS04Test7.Name = "btnWS04Test7"
        Me.btnWS04Test7.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test7.TabIndex = 21
        Me.btnWS04Test7.UseVisualStyleBackColor = True
        '
        'btnWS04Test6
        '
        Me.btnWS04Test6.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test6.Location = New System.Drawing.Point(865, 138)
        Me.btnWS04Test6.Name = "btnWS04Test6"
        Me.btnWS04Test6.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test6.TabIndex = 20
        Me.btnWS04Test6.UseVisualStyleBackColor = True
        '
        'btnWS04Test2
        '
        Me.btnWS04Test2.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test2.Location = New System.Drawing.Point(417, 138)
        Me.btnWS04Test2.Name = "btnWS04Test2"
        Me.btnWS04Test2.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test2.TabIndex = 17
        Me.btnWS04Test2.UseVisualStyleBackColor = True
        '
        'btnWS04Test1
        '
        Me.btnWS04Test1.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test1.Location = New System.Drawing.Point(417, 77)
        Me.btnWS04Test1.Name = "btnWS04Test1"
        Me.btnWS04Test1.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test1.TabIndex = 16
        Me.btnWS04Test1.UseVisualStyleBackColor = True
        '
        'btnWS04Test5
        '
        Me.btnWS04Test5.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test5.Location = New System.Drawing.Point(865, 78)
        Me.btnWS04Test5.Name = "btnWS04Test5"
        Me.btnWS04Test5.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test5.TabIndex = 3
        Me.btnWS04Test5.UseVisualStyleBackColor = True
        '
        'btnWS04Test4
        '
        Me.btnWS04Test4.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test4.Location = New System.Drawing.Point(865, 16)
        Me.btnWS04Test4.Name = "btnWS04Test4"
        Me.btnWS04Test4.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test4.TabIndex = 2
        Me.btnWS04Test4.UseVisualStyleBackColor = True
        '
        'btnWS04Test3
        '
        Me.btnWS04Test3.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test3.Location = New System.Drawing.Point(417, 198)
        Me.btnWS04Test3.Name = "btnWS04Test3"
        Me.btnWS04Test3.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test3.TabIndex = 1
        Me.btnWS04Test3.UseVisualStyleBackColor = True
        '
        'btnWS04Test0
        '
        Me.btnWS04Test0.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS04Test0.Location = New System.Drawing.Point(417, 16)
        Me.btnWS04Test0.Name = "btnWS04Test0"
        Me.btnWS04Test0.Size = New System.Drawing.Size(350, 55)
        Me.btnWS04Test0.TabIndex = 0
        Me.btnWS04Test0.UseVisualStyleBackColor = True
        '
        'TabPage10
        '
        Me.TabPage10.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage10.Controls.Add(Me.dgvWS04GeneralSettings1)
        Me.TabPage10.Controls.Add(Me.dgvWS04GeneralSettings0)
        Me.TabPage10.Location = New System.Drawing.Point(4, 39)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage10.Size = New System.Drawing.Size(1890, 712)
        Me.TabPage10.TabIndex = 1
        Me.TabPage10.Text = "General parameters"
        '
        'dgvWS04GeneralSettings1
        '
        Me.dgvWS04GeneralSettings1.AllowUserToAddRows = False
        Me.dgvWS04GeneralSettings1.AllowUserToDeleteRows = False
        Me.dgvWS04GeneralSettings1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04GeneralSettings1.Location = New System.Drawing.Point(955, 3)
        Me.dgvWS04GeneralSettings1.Name = "dgvWS04GeneralSettings1"
        Me.dgvWS04GeneralSettings1.ReadOnly = True
        Me.dgvWS04GeneralSettings1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS04GeneralSettings1.Size = New System.Drawing.Size(917, 660)
        Me.dgvWS04GeneralSettings1.TabIndex = 1
        '
        'dgvWS04GeneralSettings0
        '
        Me.dgvWS04GeneralSettings0.AllowUserToAddRows = False
        Me.dgvWS04GeneralSettings0.AllowUserToDeleteRows = False
        Me.dgvWS04GeneralSettings0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04GeneralSettings0.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvWS04GeneralSettings0.Location = New System.Drawing.Point(6, 3)
        Me.dgvWS04GeneralSettings0.Name = "dgvWS04GeneralSettings0"
        Me.dgvWS04GeneralSettings0.ReadOnly = True
        Me.dgvWS04GeneralSettings0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS04GeneralSettings0.Size = New System.Drawing.Size(917, 83)
        Me.dgvWS04GeneralSettings0.TabIndex = 0
        Me.dgvWS04GeneralSettings0.Visible = False
        '
        'TabPage11
        '
        Me.TabPage11.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage11.Controls.Add(Me.dgvWS04RearLeft_0)
        Me.TabPage11.Controls.Add(Me.dgvWS04RearLeft_1)
        Me.TabPage11.Location = New System.Drawing.Point(4, 39)
        Me.TabPage11.Name = "TabPage11"
        Me.TabPage11.Size = New System.Drawing.Size(1890, 712)
        Me.TabPage11.TabIndex = 3
        Me.TabPage11.Text = "Test Rear Left"
        '
        'dgvWS04RearLeft_0
        '
        Me.dgvWS04RearLeft_0.AllowUserToAddRows = False
        Me.dgvWS04RearLeft_0.AllowUserToDeleteRows = False
        Me.dgvWS04RearLeft_0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04RearLeft_0.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS04RearLeft_0.Name = "dgvWS04RearLeft_0"
        Me.dgvWS04RearLeft_0.ReadOnly = True
        Me.dgvWS04RearLeft_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS04RearLeft_0.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS04RearLeft_0.TabIndex = 4
        '
        'dgvWS04RearLeft_1
        '
        Me.dgvWS04RearLeft_1.AllowUserToAddRows = False
        Me.dgvWS04RearLeft_1.AllowUserToDeleteRows = False
        Me.dgvWS04RearLeft_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04RearLeft_1.Location = New System.Drawing.Point(959, 3)
        Me.dgvWS04RearLeft_1.Name = "dgvWS04RearLeft_1"
        Me.dgvWS04RearLeft_1.ReadOnly = True
        Me.dgvWS04RearLeft_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS04RearLeft_1.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS04RearLeft_1.TabIndex = 3
        '
        'TabPage12
        '
        Me.TabPage12.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage12.Controls.Add(Me.dgvWS04RearRight_1)
        Me.TabPage12.Controls.Add(Me.dgvWS04RearRight_0)
        Me.TabPage12.Location = New System.Drawing.Point(4, 39)
        Me.TabPage12.Name = "TabPage12"
        Me.TabPage12.Size = New System.Drawing.Size(1890, 712)
        Me.TabPage12.TabIndex = 4
        Me.TabPage12.Text = "Test Rear Right"
        '
        'dgvWS04RearRight_1
        '
        Me.dgvWS04RearRight_1.AllowUserToAddRows = False
        Me.dgvWS04RearRight_1.AllowUserToDeleteRows = False
        Me.dgvWS04RearRight_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04RearRight_1.Location = New System.Drawing.Point(960, 3)
        Me.dgvWS04RearRight_1.Name = "dgvWS04RearRight_1"
        Me.dgvWS04RearRight_1.ReadOnly = True
        Me.dgvWS04RearRight_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS04RearRight_1.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS04RearRight_1.TabIndex = 5
        '
        'dgvWS04RearRight_0
        '
        Me.dgvWS04RearRight_0.AllowUserToAddRows = False
        Me.dgvWS04RearRight_0.AllowUserToDeleteRows = False
        Me.dgvWS04RearRight_0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04RearRight_0.Location = New System.Drawing.Point(4, 3)
        Me.dgvWS04RearRight_0.Name = "dgvWS04RearRight_0"
        Me.dgvWS04RearRight_0.ReadOnly = True
        Me.dgvWS04RearRight_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS04RearRight_0.Size = New System.Drawing.Size(937, 415)
        Me.dgvWS04RearRight_0.TabIndex = 4
        '
        'TabPage13
        '
        Me.TabPage13.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage13.Controls.Add(Me.dgvWS04MirrorFolding_1)
        Me.TabPage13.Controls.Add(Me.dgvWS04MirrorFolding_0)
        Me.TabPage13.Location = New System.Drawing.Point(4, 39)
        Me.TabPage13.Name = "TabPage13"
        Me.TabPage13.Size = New System.Drawing.Size(1890, 712)
        Me.TabPage13.TabIndex = 5
        Me.TabPage13.Text = "Test Mirror Folding"
        '
        'dgvWS04MirrorFolding_1
        '
        Me.dgvWS04MirrorFolding_1.AllowUserToAddRows = False
        Me.dgvWS04MirrorFolding_1.AllowUserToDeleteRows = False
        Me.dgvWS04MirrorFolding_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04MirrorFolding_1.Location = New System.Drawing.Point(960, 3)
        Me.dgvWS04MirrorFolding_1.Name = "dgvWS04MirrorFolding_1"
        Me.dgvWS04MirrorFolding_1.ReadOnly = True
        Me.dgvWS04MirrorFolding_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS04MirrorFolding_1.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS04MirrorFolding_1.TabIndex = 5
        '
        'dgvWS04MirrorFolding_0
        '
        Me.dgvWS04MirrorFolding_0.AllowUserToAddRows = False
        Me.dgvWS04MirrorFolding_0.AllowUserToDeleteRows = False
        Me.dgvWS04MirrorFolding_0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS04MirrorFolding_0.Location = New System.Drawing.Point(4, 3)
        Me.dgvWS04MirrorFolding_0.Name = "dgvWS04MirrorFolding_0"
        Me.dgvWS04MirrorFolding_0.ReadOnly = True
        Me.dgvWS04MirrorFolding_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS04MirrorFolding_0.Size = New System.Drawing.Size(937, 415)
        Me.dgvWS04MirrorFolding_0.TabIndex = 4
        '
        'TabPage14
        '
        Me.TabPage14.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage14.Controls.Add(Me.TabControl3)
        Me.TabPage14.Location = New System.Drawing.Point(4, 44)
        Me.TabPage14.Name = "TabPage14"
        Me.TabPage14.Size = New System.Drawing.Size(1898, 802)
        Me.TabPage14.TabIndex = 3
        Me.TabPage14.Text = "WS 05"
        '
        'TabControl3
        '
        Me.TabControl3.Controls.Add(Me.TabPage18)
        Me.TabControl3.Controls.Add(Me.TabPage21)
        Me.TabControl3.Controls.Add(Me.TabPage22)
        Me.TabControl3.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl3.ItemSize = New System.Drawing.Size(270, 35)
        Me.TabControl3.Location = New System.Drawing.Point(3, 3)
        Me.TabControl3.Multiline = True
        Me.TabControl3.Name = "TabControl3"
        Me.TabControl3.SelectedIndex = 0
        Me.TabControl3.Size = New System.Drawing.Size(1898, 797)
        Me.TabControl3.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl3.TabIndex = 43
        '
        'TabPage18
        '
        Me.TabPage18.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage18.Controls.Add(Me.btnWS05Test2)
        Me.TabPage18.Controls.Add(Me.btnWS05Test1)
        Me.TabPage18.Controls.Add(Me.btnWS05Test0)
        Me.TabPage18.Location = New System.Drawing.Point(4, 39)
        Me.TabPage18.Name = "TabPage18"
        Me.TabPage18.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage18.Size = New System.Drawing.Size(1890, 754)
        Me.TabPage18.TabIndex = 0
        Me.TabPage18.Text = "Test enables"
        '
        'btnWS05Test2
        '
        Me.btnWS05Test2.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05Test2.Location = New System.Drawing.Point(18, 20)
        Me.btnWS05Test2.Name = "btnWS05Test2"
        Me.btnWS05Test2.Size = New System.Drawing.Size(350, 55)
        Me.btnWS05Test2.TabIndex = 17
        Me.btnWS05Test2.UseVisualStyleBackColor = True
        '
        'btnWS05Test1
        '
        Me.btnWS05Test1.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05Test1.Location = New System.Drawing.Point(426, 81)
        Me.btnWS05Test1.Name = "btnWS05Test1"
        Me.btnWS05Test1.Size = New System.Drawing.Size(350, 55)
        Me.btnWS05Test1.TabIndex = 16
        Me.btnWS05Test1.UseVisualStyleBackColor = True
        '
        'btnWS05Test0
        '
        Me.btnWS05Test0.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS05Test0.Location = New System.Drawing.Point(426, 20)
        Me.btnWS05Test0.Name = "btnWS05Test0"
        Me.btnWS05Test0.Size = New System.Drawing.Size(350, 55)
        Me.btnWS05Test0.TabIndex = 0
        Me.btnWS05Test0.UseVisualStyleBackColor = True
        '
        'TabPage21
        '
        Me.TabPage21.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage21.Controls.Add(Me.dgvWS05GeneralSettings1)
        Me.TabPage21.Controls.Add(Me.dgvWS05GeneralSettings0)
        Me.TabPage21.Location = New System.Drawing.Point(4, 39)
        Me.TabPage21.Name = "TabPage21"
        Me.TabPage21.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage21.Size = New System.Drawing.Size(1890, 754)
        Me.TabPage21.TabIndex = 1
        Me.TabPage21.Text = "General parameters"
        '
        'dgvWS05GeneralSettings1
        '
        Me.dgvWS05GeneralSettings1.AllowUserToAddRows = False
        Me.dgvWS05GeneralSettings1.AllowUserToDeleteRows = False
        Me.dgvWS05GeneralSettings1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS05GeneralSettings1.Location = New System.Drawing.Point(955, 3)
        Me.dgvWS05GeneralSettings1.Name = "dgvWS05GeneralSettings1"
        Me.dgvWS05GeneralSettings1.ReadOnly = True
        Me.dgvWS05GeneralSettings1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS05GeneralSettings1.Size = New System.Drawing.Size(917, 660)
        Me.dgvWS05GeneralSettings1.TabIndex = 1
        '
        'dgvWS05GeneralSettings0
        '
        Me.dgvWS05GeneralSettings0.AllowUserToAddRows = False
        Me.dgvWS05GeneralSettings0.AllowUserToDeleteRows = False
        Me.dgvWS05GeneralSettings0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS05GeneralSettings0.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvWS05GeneralSettings0.Location = New System.Drawing.Point(6, 3)
        Me.dgvWS05GeneralSettings0.Name = "dgvWS05GeneralSettings0"
        Me.dgvWS05GeneralSettings0.ReadOnly = True
        Me.dgvWS05GeneralSettings0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS05GeneralSettings0.Size = New System.Drawing.Size(917, 83)
        Me.dgvWS05GeneralSettings0.TabIndex = 0
        Me.dgvWS05GeneralSettings0.Visible = False
        '
        'TabPage22
        '
        Me.TabPage22.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage22.Controls.Add(Me.dgvWS05Mirror_0)
        Me.TabPage22.Controls.Add(Me.dgvWS05Mirror_1)
        Me.TabPage22.Location = New System.Drawing.Point(4, 39)
        Me.TabPage22.Name = "TabPage22"
        Me.TabPage22.Size = New System.Drawing.Size(1890, 754)
        Me.TabPage22.TabIndex = 3
        Me.TabPage22.Text = "Test Mirror"
        '
        'dgvWS05Mirror_0
        '
        Me.dgvWS05Mirror_0.AllowUserToAddRows = False
        Me.dgvWS05Mirror_0.AllowUserToDeleteRows = False
        Me.dgvWS05Mirror_0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS05Mirror_0.Location = New System.Drawing.Point(3, 3)
        Me.dgvWS05Mirror_0.Name = "dgvWS05Mirror_0"
        Me.dgvWS05Mirror_0.ReadOnly = True
        Me.dgvWS05Mirror_0.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS05Mirror_0.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS05Mirror_0.TabIndex = 4
        '
        'dgvWS05Mirror_1
        '
        Me.dgvWS05Mirror_1.AllowUserToAddRows = False
        Me.dgvWS05Mirror_1.AllowUserToDeleteRows = False
        Me.dgvWS05Mirror_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS05Mirror_1.Location = New System.Drawing.Point(959, 3)
        Me.dgvWS05Mirror_1.Name = "dgvWS05Mirror_1"
        Me.dgvWS05Mirror_1.ReadOnly = True
        Me.dgvWS05Mirror_1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS05Mirror_1.Size = New System.Drawing.Size(927, 415)
        Me.dgvWS05Mirror_1.TabIndex = 3
        '
        'lblRecipe
        '
        Me.lblRecipe.BackColor = System.Drawing.Color.White
        Me.lblRecipe.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRecipe.Font = New System.Drawing.Font("Arial Black", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecipe.Location = New System.Drawing.Point(176, 125)
        Me.lblRecipe.Name = "lblRecipe"
        Me.lblRecipe.Size = New System.Drawing.Size(303, 37)
        Me.lblRecipe.TabIndex = 40
        Me.lblRecipe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCopyTo
        '
        Me.btnCopyTo.Enabled = False
        Me.btnCopyTo.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCopyTo.Location = New System.Drawing.Point(1362, 114)
        Me.btnCopyTo.Name = "btnCopyTo"
        Me.btnCopyTo.Size = New System.Drawing.Size(100, 85)
        Me.btnCopyTo.TabIndex = 30
        Me.btnCopyTo.Text = "Copy to ..."
        Me.btnCopyTo.UseVisualStyleBackColor = True
        '
        'btnCheckMaster
        '
        Me.btnCheckMaster.BackColor = System.Drawing.SystemColors.Control
        Me.btnCheckMaster.Enabled = False
        Me.btnCheckMaster.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCheckMaster.Location = New System.Drawing.Point(1684, 114)
        Me.btnCheckMaster.Name = "btnCheckMaster"
        Me.btnCheckMaster.Size = New System.Drawing.Size(100, 85)
        Me.btnCheckMaster.TabIndex = 31
        Me.btnCheckMaster.Text = "Check master reference"
        Me.btnCheckMaster.UseVisualStyleBackColor = False
        '
        'btnSaveMaster
        '
        Me.btnSaveMaster.BackColor = System.Drawing.SystemColors.Control
        Me.btnSaveMaster.Enabled = False
        Me.btnSaveMaster.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveMaster.Location = New System.Drawing.Point(1794, 114)
        Me.btnSaveMaster.Name = "btnSaveMaster"
        Me.btnSaveMaster.Size = New System.Drawing.Size(100, 85)
        Me.btnSaveMaster.TabIndex = 32
        Me.btnSaveMaster.Text = "Save master reference"
        Me.btnSaveMaster.UseVisualStyleBackColor = False
        '
        'pbLogoValeo
        '
        Me.pbLogoValeo.Image = CType(resources.GetObject("pbLogoValeo.Image"), System.Drawing.Image)
        Me.pbLogoValeo.Location = New System.Drawing.Point(0, 0)
        Me.pbLogoValeo.Name = "pbLogoValeo"
        Me.pbLogoValeo.Size = New System.Drawing.Size(200, 103)
        Me.pbLogoValeo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbLogoValeo.TabIndex = 123
        Me.pbLogoValeo.TabStop = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(352, 172)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(140, 28)
        Me.Button1.TabIndex = 125
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'CustomerLogo
        '
        Me.CustomerLogo.Image = CType(resources.GetObject("CustomerLogo.Image"), System.Drawing.Image)
        Me.CustomerLogo.Location = New System.Drawing.Point(1674, 11)
        Me.CustomerLogo.Name = "CustomerLogo"
        Me.CustomerLogo.Size = New System.Drawing.Size(242, 86)
        Me.CustomerLogo.TabIndex = 232
        Me.CustomerLogo.TabStop = False
        '
        'btnWS02Test28
        '
        Me.btnWS02Test28.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test28.Location = New System.Drawing.Point(396, 382)
        Me.btnWS02Test28.Name = "btnWS02Test28"
        Me.btnWS02Test28.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test28.TabIndex = 22
        Me.btnWS02Test28.UseVisualStyleBackColor = True
        '
        'btnWS02Test29
        '
        Me.btnWS02Test29.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test29.Location = New System.Drawing.Point(396, 443)
        Me.btnWS02Test29.Name = "btnWS02Test29"
        Me.btnWS02Test29.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test29.TabIndex = 22
        Me.btnWS02Test29.UseVisualStyleBackColor = True
        '
        'btnWS02Test30
        '
        Me.btnWS02Test30.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWS02Test30.Location = New System.Drawing.Point(396, 504)
        Me.btnWS02Test30.Name = "btnWS02Test30"
        Me.btnWS02Test30.Size = New System.Drawing.Size(350, 55)
        Me.btnWS02Test30.TabIndex = 22
        Me.btnWS02Test30.UseVisualStyleBackColor = True
        '
        'dgvWS02SHAPE2
        '
        Me.dgvWS02SHAPE2.AllowUserToAddRows = False
        Me.dgvWS02SHAPE2.AllowUserToDeleteRows = False
        Me.dgvWS02SHAPE2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWS02SHAPE2.Location = New System.Drawing.Point(943, 3)
        Me.dgvWS02SHAPE2.Name = "dgvWS02SHAPE2"
        Me.dgvWS02SHAPE2.ReadOnly = True
        Me.dgvWS02SHAPE2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWS02SHAPE2.Size = New System.Drawing.Size(923, 415)
        Me.dgvWS02SHAPE2.TabIndex = 6
        '
        'frmRecipes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1920, 1080)
        Me.Controls.Add(Me.CustomerLogo)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.pbLogoValeo)
        Me.Controls.Add(Me.btnSaveMaster)
        Me.Controls.Add(Me.btnCheckMaster)
        Me.Controls.Add(Me.btnSaveModifies)
        Me.Controls.Add(Me.btnCancelModifies)
        Me.Controls.Add(Me.btnCopyTo)
        Me.Controls.Add(Me.btnCopyFrom)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnCreate)
        Me.Controls.Add(Me.lblRecipe)
        Me.Controls.Add(Me.tcRecipe)
        Me.Controls.Add(Me.lblModifyDate)
        Me.Controls.Add(Me.lblModifyDateDescription)
        Me.Controls.Add(Me.lblCreationDate)
        Me.Controls.Add(Me.lblCreationDateDescription)
        Me.Controls.Add(Me.lblReference)
        Me.Controls.Add(Me.ssStatusBar)
        Me.Controls.Add(Me.lblCountry)
        Me.Controls.Add(Me.lblCity)
        Me.Controls.Add(Me.lblECB)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Controls.Add(Me.lblSoftwareTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmRecipes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmRecipesBarrette"
        Me.tcRecipe.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.tcStation.ResumeLayout(False)
        Me.tpTestEnables.ResumeLayout(False)
        Me.tpGeneralParameters.ResumeLayout(False)
        CType(Me.dgvWS02GeneralSettings1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS02GeneralSettings0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tp1.ResumeLayout(False)
        CType(Me.dgvWS02SHAPE1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tp2.ResumeLayout(False)
        CType(Me.dgvWS02BACKLIGHT_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS02BACKLIGHT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tp3.ResumeLayout(False)
        CType(Me.dgvWS02TELLTALE, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage15.ResumeLayout(False)
        CType(Me.dgvWS02EMSTraceability, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage16.ResumeLayout(False)
        Me.TabControl4.ResumeLayout(False)
        Me.TabPage24.ResumeLayout(False)
        CType(Me.dgvWS02DigitalOutputStdSignal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage25.ResumeLayout(False)
        CType(Me.dgvWS02DigitalOutputPWL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage26.ResumeLayout(False)
        CType(Me.dgvWS02DigitalOutputRTRVD, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage27.ResumeLayout(False)
        CType(Me.dgvWS02DigitalOutputRTRVG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage28.ResumeLayout(False)
        CType(Me.dgvWS02PWMOutput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage17.ResumeLayout(False)
        CType(Me.dgvWS02WriteTraceability, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage20.ResumeLayout(False)
        CType(Me.dgvWS02AnalogInput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage23.ResumeLayout(False)
        CType(Me.dgvWS02WriteConfiguration, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        CType(Me.dgvWS03GeneralSettings1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS03GeneralSettings0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage5.ResumeLayout(False)
        CType(Me.dgvWS03FrontLeft_0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS03FrontLeft_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage6.ResumeLayout(False)
        CType(Me.dgvWS03FrontRight_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS03FrontRight_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage29.ResumeLayout(False)
        CType(Me.dgvWS03JamaFunction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage7.ResumeLayout(False)
        CType(Me.dgvWS03ChildrenLock_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS03ChildrenLock_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage8.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage9.ResumeLayout(False)
        Me.TabPage10.ResumeLayout(False)
        CType(Me.dgvWS04GeneralSettings1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS04GeneralSettings0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage11.ResumeLayout(False)
        CType(Me.dgvWS04RearLeft_0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS04RearLeft_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage12.ResumeLayout(False)
        CType(Me.dgvWS04RearRight_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS04RearRight_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage13.ResumeLayout(False)
        CType(Me.dgvWS04MirrorFolding_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS04MirrorFolding_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage14.ResumeLayout(False)
        Me.TabControl3.ResumeLayout(False)
        Me.TabPage18.ResumeLayout(False)
        Me.TabPage21.ResumeLayout(False)
        CType(Me.dgvWS05GeneralSettings1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS05GeneralSettings0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage22.ResumeLayout(False)
        CType(Me.dgvWS05Mirror_0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS05Mirror_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CustomerLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWS02SHAPE2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCreate As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnCopyFrom As System.Windows.Forms.Button
    Friend WithEvents btnCancelModifies As System.Windows.Forms.Button
    Friend WithEvents btnSaveModifies As System.Windows.Forms.Button
    Friend WithEvents lblReference As System.Windows.Forms.Label
    Friend WithEvents lblCreationDateDescription As System.Windows.Forms.Label
    Friend WithEvents lblCreationDate As System.Windows.Forms.Label
    Friend WithEvents lblModifyDateDescription As System.Windows.Forms.Label
    Friend WithEvents lblModifyDate As System.Windows.Forms.Label
    Friend WithEvents lblSoftwareTitle As System.Windows.Forms.Label
    Friend WithEvents lblFormTitle As System.Windows.Forms.Label
    Friend WithEvents lblECB As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblCountry As System.Windows.Forms.Label
    Friend WithEvents ssStatusBar As System.Windows.Forms.StatusStrip
    Friend WithEvents tmrMonitor As System.Windows.Forms.Timer
    Friend WithEvents tcRecipe As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents tcStation As System.Windows.Forms.TabControl
    Friend WithEvents tpTestEnables As System.Windows.Forms.TabPage
    Friend WithEvents btnWS02Test10 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test9 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test11 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test8 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test7 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test6 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test2 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test1 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test5 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test4 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test3 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test0 As System.Windows.Forms.Button
    Friend WithEvents tpGeneralParameters As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS02GeneralSettings0 As System.Windows.Forms.DataGridView
    Friend WithEvents tp1 As System.Windows.Forms.TabPage
    Friend WithEvents tp2 As System.Windows.Forms.TabPage
    Friend WithEvents tp3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents lblRecipe As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents btnWS03Test7 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test6 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test2 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test1 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test5 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test4 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test3 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test0 As System.Windows.Forms.Button
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS03GeneralSettings1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS03GeneralSettings0 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS03FrontLeft_1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS02GeneralSettings1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS03FrontRight_1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS03FrontRight_0 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS03ChildrenLock_1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS03ChildrenLock_0 As System.Windows.Forms.DataGridView
    Friend WithEvents btnCopyTo As System.Windows.Forms.Button
    Friend WithEvents btnCheckMaster As System.Windows.Forms.Button
    Friend WithEvents btnSaveMaster As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test9 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test8 As System.Windows.Forms.Button
    Friend WithEvents TabPage15 As System.Windows.Forms.TabPage
    Friend WithEvents btnWS02Test13 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test12 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test14 As System.Windows.Forms.Button
    Friend WithEvents dgvWS02SHAPE1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS02BACKLIGHT_1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS02BACKLIGHT As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS02TELLTALE As System.Windows.Forms.DataGridView
    Friend WithEvents pbLogoValeo As System.Windows.Forms.PictureBox
    Friend WithEvents btnWS02Test19 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test18 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test17 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test15 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test16 As System.Windows.Forms.Button
    Friend WithEvents dgvWS02EMSTraceability As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage16 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS02DigitalOutputPWL As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage17 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS02WriteTraceability As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage19 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage20 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS02AnalogInput As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CustomerLogo As System.Windows.Forms.PictureBox
    Friend WithEvents dgvWS03FrontLeft_0 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents btnWS04Test9 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test8 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test7 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test6 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test2 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test1 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test5 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test4 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test3 As System.Windows.Forms.Button
    Friend WithEvents btnWS04Test0 As System.Windows.Forms.Button
    Friend WithEvents TabPage10 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS04GeneralSettings1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS04GeneralSettings0 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage11 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS04RearLeft_0 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS04RearLeft_1 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage12 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS04RearRight_1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS04RearRight_0 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage13 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS04MirrorFolding_1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS04MirrorFolding_0 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage14 As System.Windows.Forms.TabPage
    Friend WithEvents TabControl3 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage18 As System.Windows.Forms.TabPage
    Friend WithEvents btnWS05Test1 As System.Windows.Forms.Button
    Friend WithEvents btnWS05Test0 As System.Windows.Forms.Button
    Friend WithEvents TabPage21 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS05GeneralSettings1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS05GeneralSettings0 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage22 As System.Windows.Forms.TabPage
    Friend WithEvents dgvWS05Mirror_0 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvWS05Mirror_1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnWS03Test11 As Button
    Friend WithEvents btnWS03Test10 As Button
    Friend WithEvents btnWS02Test22 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test21 As System.Windows.Forms.Button
    Friend WithEvents btnWS02Test20 As System.Windows.Forms.Button
    Friend WithEvents TabPage23 As TabPage
    Friend WithEvents dgvWS02WriteConfiguration As DataGridView
    Friend WithEvents btnWS02Test23 As Button
    Friend WithEvents dgvWS02DigitalOutputStdSignal As System.Windows.Forms.DataGridView
    Friend WithEvents TabControl4 As TabControl
    Friend WithEvents TabPage24 As TabPage
    Friend WithEvents TabPage25 As TabPage
    Friend WithEvents TabPage26 As TabPage
    Friend WithEvents dgvWS02DigitalOutputRTRVD As DataGridView
    Friend WithEvents TabPage27 As TabPage
    Friend WithEvents dgvWS02DigitalOutputRTRVG As DataGridView
    Friend WithEvents btnWS02Test24 As System.Windows.Forms.Button
    Friend WithEvents btnWS03Test12 As Button
    Friend WithEvents btnWS04Test10 As Button
    Friend WithEvents btnWS05Test2 As Button
    Friend WithEvents btnWS02Test25 As Button
    Friend WithEvents TabPage28 As TabPage
    Friend WithEvents dgvWS02PWMOutput As DataGridView
    Friend WithEvents TabPage29 As TabPage
    Friend WithEvents dgvWS03JamaFunction As DataGridView
    Friend WithEvents btnWS02Test27 As Button
    Friend WithEvents btnWS02Test26 As Button
    Friend WithEvents btnWS02Test30 As Button
    Friend WithEvents btnWS02Test31 As Button
    Friend WithEvents btnWS02Test29 As Button
    Friend WithEvents btnWS02Test28 As Button
    Friend WithEvents dgvWS02SHAPE2 As DataGridView
End Class
