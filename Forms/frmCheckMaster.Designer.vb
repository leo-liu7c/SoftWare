<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCheckMaster
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCheckMaster))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tcWS = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblMasterWS03 = New System.Windows.Forms.Label()
        Me.lblRecipeWS03 = New System.Windows.Forms.Label()
        Me.dgvMasterWS02 = New System.Windows.Forms.DataGridView()
        Me.dgvRecipeWS02 = New System.Windows.Forms.DataGridView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.lblMasterWS110 = New System.Windows.Forms.Label()
        Me.lblRecipeWS110 = New System.Windows.Forms.Label()
        Me.dgvMasterWS03 = New System.Windows.Forms.DataGridView()
        Me.dgvRecipeWS03 = New System.Windows.Forms.DataGridView()
        Me.tcWS.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.dgvMasterWS02, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvRecipeWS02, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.dgvMasterWS03, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvRecipeWS03, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Image = CType(resources.GetObject("btnOk.Image"), System.Drawing.Image)
        Me.btnOk.Location = New System.Drawing.Point(847, 565)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 100)
        Me.btnOk.TabIndex = 90
        Me.btnOk.Text = "Copy"
        Me.btnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.Location = New System.Drawing.Point(304, 565)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 100)
        Me.btnCancel.TabIndex = 89
        Me.btnCancel.Text = "Close"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'tcWS
        '
        Me.tcWS.Controls.Add(Me.TabPage1)
        Me.tcWS.Controls.Add(Me.TabPage2)
        Me.tcWS.Font = New System.Drawing.Font("Arial Black", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcWS.ItemSize = New System.Drawing.Size(300, 40)
        Me.tcWS.Location = New System.Drawing.Point(10, 12)
        Me.tcWS.Name = "tcWS"
        Me.tcWS.SelectedIndex = 0
        Me.tcWS.Size = New System.Drawing.Size(1226, 547)
        Me.tcWS.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tcWS.TabIndex = 91
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.lblMasterWS03)
        Me.TabPage1.Controls.Add(Me.lblRecipeWS03)
        Me.TabPage1.Controls.Add(Me.dgvMasterWS02)
        Me.TabPage1.Controls.Add(Me.dgvRecipeWS02)
        Me.TabPage1.Location = New System.Drawing.Point(4, 44)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1218, 499)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "WS02"
        '
        'lblMasterWS03
        '
        Me.lblMasterWS03.AutoSize = True
        Me.lblMasterWS03.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMasterWS03.Location = New System.Drawing.Point(824, 7)
        Me.lblMasterWS03.Name = "lblMasterWS03"
        Me.lblMasterWS03.Size = New System.Drawing.Size(232, 32)
        Me.lblMasterWS03.TabIndex = 91
        Me.lblMasterWS03.Text = "Master reference"
        '
        'lblRecipeWS03
        '
        Me.lblRecipeWS03.AutoSize = True
        Me.lblRecipeWS03.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecipeWS03.Location = New System.Drawing.Point(167, 7)
        Me.lblRecipeWS03.Name = "lblRecipeWS03"
        Me.lblRecipeWS03.Size = New System.Drawing.Size(264, 32)
        Me.lblRecipeWS03.TabIndex = 90
        Me.lblRecipeWS03.Text = "Recipe in progress"
        '
        'dgvMasterWS02
        '
        Me.dgvMasterWS02.AllowUserToAddRows = False
        Me.dgvMasterWS02.AllowUserToDeleteRows = False
        Me.dgvMasterWS02.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Aquamarine
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial Black", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Aquamarine
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvMasterWS02.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvMasterWS02.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvMasterWS02.Location = New System.Drawing.Point(626, 42)
        Me.dgvMasterWS02.Name = "dgvMasterWS02"
        Me.dgvMasterWS02.ReadOnly = True
        Me.dgvMasterWS02.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMasterWS02.Size = New System.Drawing.Size(580, 90)
        Me.dgvMasterWS02.TabIndex = 89
        '
        'dgvRecipeWS02
        '
        Me.dgvRecipeWS02.AllowUserToAddRows = False
        Me.dgvRecipeWS02.AllowUserToDeleteRows = False
        Me.dgvRecipeWS02.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.LightPink
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial Black", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightPink
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvRecipeWS02.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvRecipeWS02.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvRecipeWS02.Location = New System.Drawing.Point(10, 42)
        Me.dgvRecipeWS02.Name = "dgvRecipeWS02"
        Me.dgvRecipeWS02.ReadOnly = True
        Me.dgvRecipeWS02.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvRecipeWS02.Size = New System.Drawing.Size(580, 90)
        Me.dgvRecipeWS02.TabIndex = 88
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage2.Controls.Add(Me.lblMasterWS110)
        Me.TabPage2.Controls.Add(Me.lblRecipeWS110)
        Me.TabPage2.Controls.Add(Me.dgvMasterWS03)
        Me.TabPage2.Controls.Add(Me.dgvRecipeWS03)
        Me.TabPage2.Location = New System.Drawing.Point(4, 44)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1218, 499)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "WS03"
        '
        'lblMasterWS110
        '
        Me.lblMasterWS110.AutoSize = True
        Me.lblMasterWS110.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMasterWS110.Location = New System.Drawing.Point(824, 7)
        Me.lblMasterWS110.Name = "lblMasterWS110"
        Me.lblMasterWS110.Size = New System.Drawing.Size(232, 32)
        Me.lblMasterWS110.TabIndex = 93
        Me.lblMasterWS110.Text = "Master reference"
        '
        'lblRecipeWS110
        '
        Me.lblRecipeWS110.AutoSize = True
        Me.lblRecipeWS110.Font = New System.Drawing.Font("Arial", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecipeWS110.Location = New System.Drawing.Point(167, 7)
        Me.lblRecipeWS110.Name = "lblRecipeWS110"
        Me.lblRecipeWS110.Size = New System.Drawing.Size(264, 32)
        Me.lblRecipeWS110.TabIndex = 92
        Me.lblRecipeWS110.Text = "Recipe in progress"
        '
        'dgvMasterWS03
        '
        Me.dgvMasterWS03.AllowUserToAddRows = False
        Me.dgvMasterWS03.AllowUserToDeleteRows = False
        Me.dgvMasterWS03.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.Aquamarine
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Arial Black", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Aquamarine
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvMasterWS03.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvMasterWS03.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvMasterWS03.Location = New System.Drawing.Point(626, 42)
        Me.dgvMasterWS03.Name = "dgvMasterWS03"
        Me.dgvMasterWS03.ReadOnly = True
        Me.dgvMasterWS03.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMasterWS03.Size = New System.Drawing.Size(580, 90)
        Me.dgvMasterWS03.TabIndex = 91
        '
        'dgvRecipeWS03
        '
        Me.dgvRecipeWS03.AllowUserToAddRows = False
        Me.dgvRecipeWS03.AllowUserToDeleteRows = False
        Me.dgvRecipeWS03.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.LightPink
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Arial Black", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LightPink
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvRecipeWS03.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgvRecipeWS03.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvRecipeWS03.Location = New System.Drawing.Point(10, 42)
        Me.dgvRecipeWS03.Name = "dgvRecipeWS03"
        Me.dgvRecipeWS03.ReadOnly = True
        Me.dgvRecipeWS03.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvRecipeWS03.Size = New System.Drawing.Size(580, 90)
        Me.dgvRecipeWS03.TabIndex = 90
        '
        'frmCheckMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1248, 690)
        Me.Controls.Add(Me.tcWS)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCheckMaster"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Check Master reference"
        Me.tcWS.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.dgvMasterWS02, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvRecipeWS02, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.dgvMasterWS03, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvRecipeWS03, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tcWS As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents lblMasterWS03 As System.Windows.Forms.Label
    Friend WithEvents lblRecipeWS03 As System.Windows.Forms.Label
    Friend WithEvents dgvMasterWS02 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvRecipeWS02 As System.Windows.Forms.DataGridView
    Friend WithEvents lblMasterWS110 As System.Windows.Forms.Label
    Friend WithEvents lblRecipeWS110 As System.Windows.Forms.Label
    Friend WithEvents dgvMasterWS03 As System.Windows.Forms.DataGridView
    Friend WithEvents dgvRecipeWS03 As System.Windows.Forms.DataGridView
End Class
