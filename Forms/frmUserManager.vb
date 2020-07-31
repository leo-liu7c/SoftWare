Option Explicit On
Option Strict On

Public Class frmUserManager
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private Const _rowHeight = 50

    ' Private variables
    Private _modify As Boolean



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
    Private Sub btnAddUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddUser.Click
        ' Ask the user to enter the new username
        frmEnterValue.MaximumLength = 20
        frmEnterValue.Description = "Enter the new username"
        frmEnterValue.EnableComma = False
        frmEnterValue.EnableHexadecimalLetters = False
        frmEnterValue.EnableLetters = True
        frmEnterValue.Value = ""
        frmEnterValue.ShowDialog()
        ' If the user confirms the operation
        If (frmEnterValue.Confirm = True) Then
            ' If no errors occours while adding the user
            If (mUserManager.AddUser(frmEnterValue.Value) = False) Then
                ' Refreshes the form
                RefreshForm()
                ' Selects the new user
                SelectUser(mUserManager.UserCount - 1)
                ' Sets the flag of modify
                _modify = True
                ' Otherwise, if some error occours while adding the user
            Else
                ' Shows an error message
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while adding the user."
                frmMessage.ShowDialog()
            End If
        End If
    End Sub



    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim c As Boolean

        ' If the flag of modify is set
        If (_modify = True) Then
            ' Show a question message
            frmMessage.MessageType = frmMessage.eType.Question
            frmMessage.MessageButtons = frmMessage.eButtons.YesNoCancel
            frmMessage.Message = "Do you want to save the modifies?"
            frmMessage.ShowDialog()
            ' If the user wants to save the modifies
            If (frmMessage.MessageChoice = frmMessage.eChoice.Yes) Then
                ' Save the modifies
                Do
                    c = mUserManager.SaveConfiguration(mSettings.UsersConfigurationPath)
                    If (c) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                        frmMessage.Message = "Error while saving the configuration: retry?"
                        frmMessage.ShowDialog()
                        c = c And (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
                    End If
                Loop Until (c = False)
                ' Unload the form
                Me.Close()
            ElseIf (frmMessage.MessageChoice = frmMessage.eChoice.No) Then  ' Otherwise, if the user wants to discard the modifies
                ' Discard the modifies
                Do
                    c = mUserManager.LoadConfiguration(mSettings.UsersConfigurationPath)
                    If (c) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                        frmMessage.Message = "Error while loading the configuration: retry?"
                        frmMessage.ShowDialog()
                        c = c And (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
                    End If
                Loop Until (c = False)
                ' Unload the form
                Me.Close()
            End If
        Else    ' Otherwise, if the flag of modify is cleared
            ' Unload the form
            Me.Close()
        End If
    End Sub



    Private Sub btnDeleteUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteUser.Click
        Dim userIndex As Integer

        ' If the user count is greater than 1
        If (mUserManager.UserCount > 1) Then
            ' Show a question message
            userIndex = fgUsername.SelectedRows(0).Index + 1
            frmMessage.MessageType = frmMessage.eType.Question
            frmMessage.MessageButtons = frmMessage.eButtons.YesNoCancel
            frmMessage.Message = "Do you confirm to delete the user " & mUserManager.Username(userIndex) & "?"
            frmMessage.ShowDialog()
            ' If the user confirms the operation
            If (frmMessage.MessageChoice = frmMessage.eChoice.Yes) Then
                ' If no errors happen while deleting the user
                If (mUserManager.DeleteUser(userIndex) = False) Then
                    ' Refresh the form
                    RefreshForm()
                    ' Selects the first user
                    SelectUser(1)
                    ' Set the flag of modify
                    _modify = True
                    ' Otherwise, if some error happens while deleting the user
                Else
                    ' Shows an error message
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while deleting the user."
                    frmMessage.ShowDialog()
                End If
            End If
        End If
    End Sub



    Private Sub btnResetUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetUser.Click
        Dim userIndex As Integer

        ' If the user count is greater than 1
        If (mUserManager.UserCount > 1) Then
            ' Show a question message
            userIndex = fgUsername.SelectedRows(0).Index + 1
            frmMessage.MessageType = frmMessage.eType.Question
            frmMessage.MessageButtons = frmMessage.eButtons.YesNoCancel
            frmMessage.Message = "Do you confirm to reset the user " & mUserManager.Username(userIndex) & "?"
            frmMessage.ShowDialog()
            ' If the user confirms the operation
            If (frmMessage.MessageChoice = frmMessage.eChoice.Yes) Then
                ' If no errors happen while resetting the user
                If (mUserManager.ResetUser(userIndex) = False) Then
                    ' Set the flag of modify
                    _modify = True
                    ' Otherwise, if some error happens while resetting the user
                Else
                    ' Show an error message
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while resetting the user."
                    frmMessage.ShowDialog()
                End If
            End If
        End If
    End Sub



    Private Sub cbAccessLevel_SelectionChangeCommitted(sender As Object, e As System.EventArgs) Handles cbAccessLevel.SelectionChangeCommitted
        ' Set the access level of the selected user
        If (mUserManager.UserCount > 1) Then
            mUserManager.AccessLevel(fgUsername.SelectedRows(0).Index + 1) = CInt(cbAccessLevel.SelectedIndex)
            _modify = True
        End If
    End Sub



    Private Sub fgUsername_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles fgUsername.CellClick
        ' Select the specified user
        SelectUser(e.RowIndex + 1)
    End Sub



    Private Sub frmUserManager_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim mstrAccessLevelUser As mUserManager.AccessLevelUser

        ' Initialize the table of usernames
        With fgUsername
            ' Disable the refresh
            .SuspendLayout()
            ' Configurate the rows
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            .RowCount = 1
            ' Configurate the columns
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersVisible = False
            .ColumnCount = 1
            ' Configurate the cell styles
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.Font = New Font("Arial", 18)
            ' Configurate the edit mode
            .EditMode = DataGridViewEditMode.EditProgrammatically
            ' Disable multiple selection
            .MultiSelect = False
            ' Enable the refresh
            .RefreshEdit()
            .ResumeLayout(True)
        End With
        ' Initializes the list of access levels
        cbAccessLevel.Items.Clear()
        For i = 0 To mUserManager.AccessLevelUser.count - 1
            mstrAccessLevelUser = CType(AccessLevelUser.OP + i, AccessLevelUser)
            cbAccessLevel.Items.Add(mstrAccessLevelUser.ToString)
        Next
        ' Refresh the form
        RefreshForm()
        ' Select the first user
        SelectUser(1)
        ' Clear the flag of modify
        _modify = False
    End Sub



    Private Sub RefreshForm()
        Dim r As Integer

        ' Refresh the list of usernames
        With fgUsername
            ' If there is only one user (the administrator)
            If (mUserManager.UserCount < 2) Then
                ' Show an empty line
                .RowCount = 1
                .Item(0, r).Value = ""
            Else    ' Otherwise, if there is more than one user
                ' Set the row count
                .RowCount = mUserManager.UserCount - 1
                ' Configurate the column width and the scrollbars
                If (.RowCount < .Height \ _rowHeight) Then
                    .ScrollBars = ScrollBars.None
                    .Columns(0).Width = .Width
                Else
                    .ScrollBars = ScrollBars.Vertical
                    .Columns(0).Width = .Width - SystemInformation.VerticalScrollBarWidth
                End If
                ' Configurate the rows
                For r = 0 To .RowCount - 1
                    .Rows(r).Height = 50
                    .Item(0, r).Value = mUserManager.Username(r + 1)
                Next r
            End If
            .ClearSelection()
        End With
    End Sub



    Private Sub SelectUser(ByVal userIndex As Integer)
        ' Select the user row
        fgUsername.Rows(userIndex - 1).Selected = True
        ' Show the user access level
        cbAccessLevel.SelectedIndex = mUserManager.AccessLevel(userIndex)
    End Sub

    Private Sub cbAccessLevel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAccessLevel.SelectedIndexChanged
        Select Case cbAccessLevel.SelectedIndex
            Case AccessLevelUser.Maintenance
                lblDescription.Text = "Access Level : " & cbAccessLevel.SelectedIndex & vbCrLf & "This user is allowed to modify recipes"
            Case AccessLevelUser.Methodes
                lblDescription.Text = "Access Level : " & cbAccessLevel.SelectedIndex & vbCrLf & "This user is allowed to modify recipes"
            Case AccessLevelUser.SuperMethodes
                lblDescription.Text = "Access Level : " & cbAccessLevel.SelectedIndex & vbCrLf & "This user is allowed to modify recipes and save master reference"
        End Select
    End Sub

End Class