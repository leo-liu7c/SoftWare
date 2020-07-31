<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainMenu))
        Me.btnCalibration = New System.Windows.Forms.Button()
        Me.btnRecipes = New System.Windows.Forms.Button()
        Me.btnProduction = New System.Windows.Forms.Button()
        Me.btnMaintenance = New System.Windows.Forms.Button()
        Me.btnUsersManager = New System.Windows.Forms.Button()
        Me.pbLogoValeo = New System.Windows.Forms.PictureBox()
        Me.lblSoftwareTitle = New System.Windows.Forms.Label()
        Me.lblFormTitle = New System.Windows.Forms.Label()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblECB = New System.Windows.Forms.Label()
        Me.ssStatusBar = New System.Windows.Forms.StatusStrip()
        Me.tmrMonitor = New System.Windows.Forms.Timer(Me.components)
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnModifyPassword = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.btnShutdownPC = New System.Windows.Forms.Button()
        Me.btnSoftwareRevisions = New System.Windows.Forms.Button()
        Me.btnPLC_EOL = New System.Windows.Forms.Button()
        Me.CustomerLogo = New System.Windows.Forms.PictureBox()
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CustomerLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCalibration
        '
        Me.btnCalibration.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnCalibration.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCalibration.Location = New System.Drawing.Point(467, 113)
        Me.btnCalibration.Name = "btnCalibration"
        Me.btnCalibration.Size = New System.Drawing.Size(1000, 106)
        Me.btnCalibration.TabIndex = 0
        Me.btnCalibration.Text = "Calibration"
        Me.btnCalibration.UseVisualStyleBackColor = False
        Me.btnCalibration.Visible = False
        '
        'btnRecipes
        '
        Me.btnRecipes.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnRecipes.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRecipes.Location = New System.Drawing.Point(467, 240)
        Me.btnRecipes.Name = "btnRecipes"
        Me.btnRecipes.Size = New System.Drawing.Size(1000, 106)
        Me.btnRecipes.TabIndex = 1
        Me.btnRecipes.Text = "Test parameters"
        Me.btnRecipes.UseVisualStyleBackColor = False
        '
        'btnProduction
        '
        Me.btnProduction.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnProduction.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProduction.Location = New System.Drawing.Point(467, 372)
        Me.btnProduction.Name = "btnProduction"
        Me.btnProduction.Size = New System.Drawing.Size(1000, 106)
        Me.btnProduction.TabIndex = 2
        Me.btnProduction.Text = "Production"
        Me.btnProduction.UseVisualStyleBackColor = False
        '
        'btnMaintenance
        '
        Me.btnMaintenance.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnMaintenance.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMaintenance.Location = New System.Drawing.Point(467, 492)
        Me.btnMaintenance.Name = "btnMaintenance"
        Me.btnMaintenance.Size = New System.Drawing.Size(500, 106)
        Me.btnMaintenance.TabIndex = 3
        Me.btnMaintenance.Text = "Maintenance EOL"
        Me.btnMaintenance.UseVisualStyleBackColor = False
        '
        'btnUsersManager
        '
        Me.btnUsersManager.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnUsersManager.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUsersManager.Location = New System.Drawing.Point(467, 741)
        Me.btnUsersManager.Name = "btnUsersManager"
        Me.btnUsersManager.Size = New System.Drawing.Size(1000, 106)
        Me.btnUsersManager.TabIndex = 4
        Me.btnUsersManager.Text = "Users manager"
        Me.btnUsersManager.UseVisualStyleBackColor = False
        '
        'pbLogoValeo
        '
        Me.pbLogoValeo.Image = CType(resources.GetObject("pbLogoValeo.Image"), System.Drawing.Image)
        Me.pbLogoValeo.Location = New System.Drawing.Point(0, 0)
        Me.pbLogoValeo.Name = "pbLogoValeo"
        Me.pbLogoValeo.Size = New System.Drawing.Size(200, 107)
        Me.pbLogoValeo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbLogoValeo.TabIndex = 6
        Me.pbLogoValeo.TabStop = False
        '
        'lblSoftwareTitle
        '
        Me.lblSoftwareTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblSoftwareTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSoftwareTitle.Location = New System.Drawing.Point(200, 0)
        Me.lblSoftwareTitle.Name = "lblSoftwareTitle"
        Me.lblSoftwareTitle.Size = New System.Drawing.Size(1520, 46)
        Me.lblSoftwareTitle.TabIndex = 7
        Me.lblSoftwareTitle.Text = "EOL Test Bench RSA SQUARE SUV WILI"
        Me.lblSoftwareTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFormTitle
        '
        Me.lblFormTitle.BackColor = System.Drawing.Color.Cyan
        Me.lblFormTitle.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFormTitle.Location = New System.Drawing.Point(200, 46)
        Me.lblFormTitle.Name = "lblFormTitle"
        Me.lblFormTitle.Size = New System.Drawing.Size(1520, 61)
        Me.lblFormTitle.TabIndex = 8
        Me.lblFormTitle.Text = "Main menu"
        Me.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCountry
        '
        Me.lblCountry.BackColor = System.Drawing.Color.White
        Me.lblCountry.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.ForeColor = System.Drawing.Color.Blue
        Me.lblCountry.Location = New System.Drawing.Point(1720, 65)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.Size = New System.Drawing.Size(200, 42)
        Me.lblCountry.TabIndex = 19
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
        Me.lblCity.TabIndex = 18
        Me.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblECB
        '
        Me.lblECB.BackColor = System.Drawing.Color.White
        Me.lblECB.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblECB.ForeColor = System.Drawing.Color.Blue
        Me.lblECB.Location = New System.Drawing.Point(1666, 0)
        Me.lblECB.Name = "lblECB"
        Me.lblECB.Size = New System.Drawing.Size(254, 107)
        Me.lblECB.TabIndex = 17
        Me.lblECB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ssStatusBar
        '
        Me.ssStatusBar.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssStatusBar.Location = New System.Drawing.Point(0, 1058)
        Me.ssStatusBar.Name = "ssStatusBar"
        Me.ssStatusBar.Size = New System.Drawing.Size(1920, 22)
        Me.ssStatusBar.TabIndex = 21
        Me.ssStatusBar.Text = "StatusStrip1"
        '
        'tmrMonitor
        '
        Me.tmrMonitor.Interval = 500
        '
        'btnLogin
        '
        Me.btnLogin.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogin.Location = New System.Drawing.Point(467, 619)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(282, 106)
        Me.btnLogin.TabIndex = 22
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'btnModifyPassword
        '
        Me.btnModifyPassword.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnModifyPassword.Location = New System.Drawing.Point(784, 619)
        Me.btnModifyPassword.Name = "btnModifyPassword"
        Me.btnModifyPassword.Size = New System.Drawing.Size(371, 106)
        Me.btnModifyPassword.TabIndex = 23
        Me.btnModifyPassword.Text = "Modify password"
        Me.btnModifyPassword.UseVisualStyleBackColor = True
        '
        'btnLogout
        '
        Me.btnLogout.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.Location = New System.Drawing.Point(1186, 619)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(281, 106)
        Me.btnLogout.TabIndex = 24
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseVisualStyleBackColor = True
        '
        'btnShutdownPC
        '
        Me.btnShutdownPC.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnShutdownPC.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShutdownPC.Location = New System.Drawing.Point(467, 868)
        Me.btnShutdownPC.Name = "btnShutdownPC"
        Me.btnShutdownPC.Size = New System.Drawing.Size(1000, 106)
        Me.btnShutdownPC.TabIndex = 25
        Me.btnShutdownPC.Text = "Shutdown PC"
        Me.btnShutdownPC.UseVisualStyleBackColor = False
        '
        'btnSoftwareRevisions
        '
        Me.btnSoftwareRevisions.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSoftwareRevisions.Location = New System.Drawing.Point(22, 145)
        Me.btnSoftwareRevisions.Name = "btnSoftwareRevisions"
        Me.btnSoftwareRevisions.Size = New System.Drawing.Size(200, 74)
        Me.btnSoftwareRevisions.TabIndex = 26
        Me.btnSoftwareRevisions.UseVisualStyleBackColor = True
        '
        'btnPLC_EOL
        '
        Me.btnPLC_EOL.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnPLC_EOL.Font = New System.Drawing.Font("Arial", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPLC_EOL.Location = New System.Drawing.Point(967, 492)
        Me.btnPLC_EOL.Name = "btnPLC_EOL"
        Me.btnPLC_EOL.Size = New System.Drawing.Size(500, 106)
        Me.btnPLC_EOL.TabIndex = 27
        Me.btnPLC_EOL.Text = "Maintenance PLC <=> EOL"
        Me.btnPLC_EOL.UseVisualStyleBackColor = False
        '
        'CustomerLogo
        '
        Me.CustomerLogo.Image = CType(resources.GetObject("CustomerLogo.Image"), System.Drawing.Image)
        Me.CustomerLogo.Location = New System.Drawing.Point(1678, 11)
        Me.CustomerLogo.Name = "CustomerLogo"
        Me.CustomerLogo.Size = New System.Drawing.Size(242, 96)
        Me.CustomerLogo.TabIndex = 232
        Me.CustomerLogo.TabStop = False
        '
        'frmMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1920, 1080)
        Me.Controls.Add(Me.CustomerLogo)
        Me.Controls.Add(Me.btnPLC_EOL)
        Me.Controls.Add(Me.btnSoftwareRevisions)
        Me.Controls.Add(Me.btnShutdownPC)
        Me.Controls.Add(Me.btnLogout)
        Me.Controls.Add(Me.btnModifyPassword)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.ssStatusBar)
        Me.Controls.Add(Me.lblCountry)
        Me.Controls.Add(Me.lblCity)
        Me.Controls.Add(Me.lblECB)
        Me.Controls.Add(Me.lblFormTitle)
        Me.Controls.Add(Me.lblSoftwareTitle)
        Me.Controls.Add(Me.pbLogoValeo)
        Me.Controls.Add(Me.btnUsersManager)
        Me.Controls.Add(Me.btnMaintenance)
        Me.Controls.Add(Me.btnProduction)
        Me.Controls.Add(Me.btnRecipes)
        Me.Controls.Add(Me.btnCalibration)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMainMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmMainMenu"
        CType(Me.pbLogoValeo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CustomerLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCalibration As System.Windows.Forms.Button
    Friend WithEvents btnRecipes As System.Windows.Forms.Button
    Friend WithEvents btnProduction As System.Windows.Forms.Button
    Friend WithEvents btnMaintenance As System.Windows.Forms.Button
    Friend WithEvents btnUsersManager As System.Windows.Forms.Button
    Friend WithEvents pbLogoValeo As System.Windows.Forms.PictureBox
    Friend WithEvents lblSoftwareTitle As System.Windows.Forms.Label
    Friend WithEvents lblFormTitle As System.Windows.Forms.Label
    Friend WithEvents lblCountry As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblECB As System.Windows.Forms.Label
    Friend WithEvents ssStatusBar As System.Windows.Forms.StatusStrip
    Friend WithEvents tmrMonitor As System.Windows.Forms.Timer
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnModifyPassword As System.Windows.Forms.Button
    Friend WithEvents btnLogout As System.Windows.Forms.Button
    Friend WithEvents btnShutdownPC As System.Windows.Forms.Button
    Friend WithEvents btnSoftwareRevisions As System.Windows.Forms.Button
    Friend WithEvents btnPLC_EOL As System.Windows.Forms.Button
    Friend WithEvents CustomerLogo As System.Windows.Forms.PictureBox

End Class
