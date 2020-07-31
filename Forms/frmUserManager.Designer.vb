<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUserManager
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUserManager))
        Me.btnAddUser = New System.Windows.Forms.Button()
        Me.btnDeleteUser = New System.Windows.Forms.Button()
        Me.btnResetUser = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.labUsername = New System.Windows.Forms.Label()
        Me.fgUsername = New System.Windows.Forms.DataGridView()
        Me.lblAccesLevel = New System.Windows.Forms.Label()
        Me.cbAccessLevel = New System.Windows.Forms.ComboBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        CType(Me.fgUsername, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnAddUser
        '
        resources.ApplyResources(Me.btnAddUser, "btnAddUser")
        Me.btnAddUser.Name = "btnAddUser"
        Me.btnAddUser.UseVisualStyleBackColor = True
        '
        'btnDeleteUser
        '
        resources.ApplyResources(Me.btnDeleteUser, "btnDeleteUser")
        Me.btnDeleteUser.Name = "btnDeleteUser"
        Me.btnDeleteUser.UseVisualStyleBackColor = True
        '
        'btnResetUser
        '
        resources.ApplyResources(Me.btnResetUser, "btnResetUser")
        Me.btnResetUser.Name = "btnResetUser"
        Me.btnResetUser.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        resources.ApplyResources(Me.btnClose, "btnClose")
        Me.btnClose.Name = "btnClose"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'labUsername
        '
        resources.ApplyResources(Me.labUsername, "labUsername")
        Me.labUsername.Name = "labUsername"
        '
        'fgUsername
        '
        Me.fgUsername.AllowUserToAddRows = False
        Me.fgUsername.AllowUserToDeleteRows = False
        Me.fgUsername.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.fgUsername.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.fgUsername, "fgUsername")
        Me.fgUsername.Name = "fgUsername"
        Me.fgUsername.ReadOnly = True
        Me.fgUsername.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        '
        'lblAccesLevel
        '
        resources.ApplyResources(Me.lblAccesLevel, "lblAccesLevel")
        Me.lblAccesLevel.Name = "lblAccesLevel"
        '
        'cbAccessLevel
        '
        resources.ApplyResources(Me.cbAccessLevel, "cbAccessLevel")
        Me.cbAccessLevel.FormattingEnabled = True
        Me.cbAccessLevel.Name = "cbAccessLevel"
        '
        'lblDescription
        '
        resources.ApplyResources(Me.lblDescription, "lblDescription")
        Me.lblDescription.Name = "lblDescription"
        '
        'frmUserManager
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ControlBox = False
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.cbAccessLevel)
        Me.Controls.Add(Me.fgUsername)
        Me.Controls.Add(Me.labUsername)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnResetUser)
        Me.Controls.Add(Me.btnDeleteUser)
        Me.Controls.Add(Me.btnAddUser)
        Me.Controls.Add(Me.lblAccesLevel)
        Me.Name = "frmUserManager"
        Me.ShowIcon = False
        CType(Me.fgUsername, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAddUser As System.Windows.Forms.Button
    Friend WithEvents btnDeleteUser As System.Windows.Forms.Button
    Friend WithEvents btnResetUser As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents labUsername As System.Windows.Forms.Label
    Friend WithEvents fgUsername As System.Windows.Forms.DataGridView
    Friend WithEvents lblAccesLevel As System.Windows.Forms.Label
    Friend WithEvents cbAccessLevel As System.Windows.Forms.ComboBox
    Friend WithEvents lblDescription As System.Windows.Forms.Label
End Class
