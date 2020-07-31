Option Explicit On
Option Strict On

Public Class frmMainMenu
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Sub btnCalibration_Click(sender As System.Object, e As System.EventArgs) Handles btnCalibration.Click
        ' If necessary ask the user to login
        If (mUserManager.CurrentUsername = "") Then
            mUserManager.Login()
        End If
        ' If there is a user logged
        If (mUserManager.CurrentUsername <> "") Then
            ' If the user access level is sufficient
            If (mUserManager.CurrentAccessLevel >= 2) Then
                ' Show the form Calibration
                frmCalibration.ShowDialog()
            Else    ' Otherwise, if the user access level is not sufficient
                ' Show a warning message
                frmMessage.MessageType = frmMessage.eType.Warning
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Function not available for the current user"
                frmMessage.ShowDialog()
            End If
        End If
    End Sub



    Private Sub btnLogin_Click(sender As System.Object, e As System.EventArgs) Handles btnLogin.Click
        ' Perform the login
        mUserManager.Login()
    End Sub



    Private Sub btnLogout_Click(sender As System.Object, e As System.EventArgs) Handles btnLogout.Click
        ' Perform the logout
        mUserManager.Logout()
    End Sub



    Private Sub btnMaintenance_Click(sender As System.Object, e As System.EventArgs) Handles btnMaintenance.Click

        ' Show the form Maintenance
        frmMaintenance_EOL.ShowDialog()
        Exit Sub
        ' If necessary ask the user to login
        If (mUserManager.CurrentUsername = "") Then
            mUserManager.Login()
        End If
        ' If there is a user logged
        If (mUserManager.CurrentUsername <> "") Then
            ' If the user access level is sufficient
            If (mUserManager.CurrentAccessLevel >= 2) Then
                ' Show the form Maintenance
                frmMaintenance_EOL.ShowDialog()
            Else    ' Otherwise, if the user access level is not sufficient
                ' Show a warning message
                frmMessage.MessageType = frmMessage.eType.Warning
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Function not available for the current user"
                frmMessage.ShowDialog()
            End If
        End If
    End Sub



    Private Sub btnModifyPassword_Click(sender As System.Object, e As System.EventArgs) Handles btnModifyPassword.Click
        ' If there is a user logged and it is not the administrator
        If (mUserManager.CurrentUsername <> "" And Not mUserManager.UserIsAdministrator) Then
            ' Configurate and show the form Login
            frmLogin.Username = mUserManager.CurrentUsername
            frmLogin.Password = ""
            frmLogin.ShowDialog()
            ' If the user confirmed the operation
            If (frmLogin.Confirm) Then
                ' Modify the password
                mUserManager.SetPassword(frmLogin.Password)
                If Not (mUserManager.SaveConfiguration(mSettings.UsersConfigurationPath)) Then
                    frmMessage.MessageType = frmMessage.eType.Information
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Password successfully modified."
                Else
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while modifying the password."
                End If
                frmMessage.ShowDialog()
            End If
        End If
    End Sub



    Private Sub btnProduction_Click(sender As System.Object, e As System.EventArgs) Handles btnProduction.Click
        ' Show the form Production
        frmProduction.ShowDialog()
    End Sub



    Private Sub btnRecipes_Click(sender As System.Object, e As System.EventArgs) Handles btnRecipes.Click
        ' Show the recipe selection form
        'frmRecipeTypeSelection.ShowDialog()
        frmRecipes.ShowDialog()
     
    End Sub



    Private Sub btnShutdownPC_Click(sender As System.Object, e As System.EventArgs) Handles btnShutdownPC.Click
        ' Configurate and show the form Message
        frmMessage.MessageType = frmMessage.eType.Question
        frmMessage.MessageButtons = frmMessage.eButtons.YesNo
        frmMessage.Message = "Do you confirm to shutdown the PC?"
        frmMessage.ShowDialog()
        ' If the user confirmed the operation
        If (frmMessage.MessageChoice = frmMessage.eChoice.Yes) Then
            ' Shutdown the PC
            Shell("shutdown -s -t 0")
        End If
    End Sub



    Private Sub btnSoftwareRevisions_Click(sender As System.Object, e As System.EventArgs) Handles btnSoftwareRevisions.Click
        ' Show the form SoftwareRevisions
        frmSoftwareRevisions.ShowDialog()
    End Sub



    Private Sub btnUsersManager_Click(sender As System.Object, e As System.EventArgs) Handles btnUsersManager.Click
        ' If necessary ask the user to login
        If (mUserManager.CurrentUsername = "") Then
            mUserManager.Login()
        End If
        ' If there is a user logged
        If (mUserManager.CurrentUsername <> "") Then
            ' If the user is the administrator
            If (mUserManager.UserIsAdministrator) Then
                ' Show the form UserManager
                frmUserManager.ShowDialog()
            Else    ' Otherwise, if the user is not the administrator
                ' Show a warning message
                frmMessage.MessageType = frmMessage.eType.Warning
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Function not available for the current user"
                frmMessage.ShowDialog()
            End If
        End If
    End Sub



    Private Sub pbLogoValeo_Click(sender As System.Object, e As System.EventArgs) Handles pbLogoValeo.Click
        ' Close the form
        Me.Close()
    End Sub



    Private Sub frmMainMenu_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        'Check if Software running more then two times
        ' get the filename of the main module
        Dim moduleName As String = Process.GetCurrentProcess.MainModule.ModuleName
        ' discard the extension to get the process name
        Dim procName As String = System.IO.Path.GetFileNameWithoutExtension(moduleName)
        ' return true if there are 2 or more processes with that name
        If Process.GetProcessesByName(procName).Length > 1 Then
            MsgBox("Program already runs !!!" & vbCr & "ProcName is:" & procName, MsgBoxStyle.Exclamation, "Message from frmMainMenu Activated!")
            End
        End If

        Dim errorFlag As Boolean
        Static flag As Boolean

        ' If the flag is not set
        If Not (flag) Then
            ' Set the flag
            flag = True
            ' Show the form Log
            frmLog.Show()
            ' Power-up the Global Station
            errorFlag = mGlobal.PowerUP()
            ' Power-up the Work Station 02
            errorFlag = mWS02Main.PowerUp()
            ' Power-up the Work Station 03
            errorFlag = mWS03Main.PowerUp()
            ' Power-up the Work Station 04
            errorFlag = mWS04Main.PowerUp()
            ' Power-up the Work Station 05
            errorFlag = mWS05Main.PowerUp()
            ' Close the form Log
            frmLog.Close()
            ' Set the flag
            flag = True
            ' Simulate the user to press on the production button
            btnProduction.PerformClick()
        End If
    End Sub



    Private Sub frmMainMenu_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim errorFlag As Boolean

        ' Show the form Log
        frmLog.Show()
        ' Power-down the station
        errorFlag = mWS02Main.PowerDown
        ' Power-down the station
        errorFlag = mWS03Main.PowerDown
        ' Power-down the station
        errorFlag = mWS04Main.PowerDown
        ' Power-down the station
        errorFlag = mWS05Main.PowerDown
        ' Power-down the global station
        errorFlag = mGlobal.PowerDown
        ' Close the form Log
        frmLog.Close()
        ' Disable the timer monitor
        tmrMonitor.Enabled = False
    End Sub



    Private Sub frmMainMenu_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Show the current software revision
        btnSoftwareRevisions.Text = "Software revision " & mConstants.SoftwareVersion & " at:" & mConstants.SoftwareDate
        ' Initialize the status bar
        mStatusBar.Initialize(ssStatusBar)
        ' Configurate and enable the timer monitor
        tmrMonitor.Interval = 500
        tmrMonitor.Enabled = True
    End Sub



    Private Sub tmrMonitor_Tick(sender As System.Object, e As System.EventArgs) Handles tmrMonitor.Tick
        ' Manage Timer for Reset Login
        If mUserManager.CurrentUsername = "" Then
            mGlobal.tUser = Now
        End If
        ' Enable/disable the buttons
        btnLogin.Enabled = (mUserManager.CurrentUsername = "")
        btnModifyPassword.Enabled = (mUserManager.CurrentUsername <> "")
        btnLogout.Enabled = (mUserManager.CurrentUsername <> "")
        ' Update the status bar
        mStatusBar.Update(ssStatusBar)
    End Sub

    Private Sub lblSoftwareTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblSoftwareTitle.Click

    End Sub

    Private Sub btnPLC_EOL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPLC_EOL.Click
        ' Show the form Maintenance
        frmPLC.ShowDialog()
        Exit Sub
        ' If necessary ask the user to login
        If (mUserManager.CurrentUsername = "") Then
            mUserManager.Login()
        End If
        ' If there is a user logged
        If (mUserManager.CurrentUsername <> "") Then
            ' If the user access level is sufficient
            If (mUserManager.CurrentAccessLevel >= 2) Then
                ' Show the form Maintenance
                frmPLC.ShowDialog()
            Else    ' Otherwise, if the user access level is not sufficient
                ' Show a warning message
                frmMessage.MessageType = frmMessage.eType.Warning
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Function not available for the current user"
                frmMessage.ShowDialog()
            End If
        End If

    End Sub
End Class
