Option Explicit On
'Option Strict On

Imports System
Imports System.IO
Imports Microsoft.VisualBasic

Public Class frmRecipes
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _maxRowCount = 100

    ' Private variables
    Private _recipeModified As Boolean
    Private _reference As String

    'WS02
    Private _WS02Recipe As New cWS02Recipe
    Private _WS02TestEnables(0 To 31) As cRecipeValue
    Private _WS02GeneralSettings0(0 To _maxRowCount - 1) As Object
    Private _WS02GeneralSettings1(0 To _maxRowCount - 1) As Object

    ' Test Pictogram
    Private _WS02Push_PictogramConformity(0 To _maxRowCount - 1) As Object
    Private _WS02PictogramPoint_Interface(0 To _maxRowCount - 1) As Object

    ' Test Other
    Private _WS02OtherParameter(0 To _maxRowCount - 1) As Object

    ' Backlight
    Private _WS02Push_Backlight(0 To _maxRowCount - 1) As Object
    Private _WS02Push_Backlight_1(0 To _maxRowCount - 1) As Object
    Private _WS02TELLTALE(0 To _maxRowCount - 1) As Object

    Private _WS02WriteTraceability(0 To _maxRowCount - 1) As Object
    Private _WS02WriteConfiguration(0 To _maxRowCount - 1) As Object
    Private _WS02ReadTraceability(0 To _maxRowCount - 1) As Object
    Private _WS02ReadEMSTraceability(0 To _maxRowCount - 1) As Object
    Private _WS02ReadAnalogInput(0 To _maxRowCount - 1) As Object

    Private _WS02DigitalOutput(0 To _maxRowCount - 1) As Object
    Private _WS02DigitalOutput_PWL(0 To _maxRowCount - 1) As Object
    Private _WS02DigitalOutput_RTRV_D(0 To _maxRowCount - 1) As Object
    Private _WS02DigitalOutput_RTRV_G(0 To _maxRowCount - 1) As Object

    Private _WS02PWMOutput(0 To _maxRowCount - 1) As Object
    ' WS03
    Private _WS03Recipe As New cWS03Recipe
    Private _WS03TestEnables(0 To 20) As cRecipeValue
    Private _WS03GeneralSettings0(0 To _maxRowCount - 1) As Object
    Private _WS03GeneralSettings1(0 To _maxRowCount - 1) As Object
    Private _WS03JamaFunction(0 To _maxRowCount - 1) As Object
    Private _WS03FrontLeftSettings0(0 To _maxRowCount - 1) As Object
    Private _WS03FrontLeftSettings1(0 To _maxRowCount - 1) As Object
    Private _WS03FrontRightSettings0(0 To _maxRowCount - 1) As Object
    Private _WS03FrontRightSettings1(0 To _maxRowCount - 1) As Object
    Private _WS03ChildrenLockSettings0(0 To _maxRowCount - 1) As Object
    Private _WS03ChildrenLockSettings1(0 To _maxRowCount - 1) As Object
    Private _WS03JAMASetting(0 To _maxRowCount - 1) As Object


    ' WS04
    Private _WS04Recipe As New cWS04Recipe
    Private _WS04TestEnables(0 To 20) As cRecipeValue
    Private _WS04GeneralSettings0(0 To _maxRowCount - 1) As Object
    Private _WS04GeneralSettings1(0 To _maxRowCount - 1) As Object
    Private _WS04RearLeftSettings0(0 To _maxRowCount - 1) As Object
    Private _WS04RearLeftSettings1(0 To _maxRowCount - 1) As Object
    Private _WS04RearRightSettings0(0 To _maxRowCount - 1) As Object
    Private _WS04RearRightSettings1(0 To _maxRowCount - 1) As Object
    Private _WS04MirrorFoldingSettings0(0 To _maxRowCount - 1) As Object
    Private _WS04MirrorFoldingSettings1(0 To _maxRowCount - 1) As Object

    ' WS05
    Private _WS05Recipe As New cWS05Recipe
    Private _WS05TestEnables(0 To 20) As cRecipeValue
    Private _WS05GeneralSettings0(0 To _maxRowCount - 1) As Object
    Private _WS05GeneralSettings1(0 To _maxRowCount - 1) As Object
    Private _WS05MirrorSettings0(0 To _maxRowCount - 1) As Object
    Private _WS05MirrorSettings1(0 To _maxRowCount - 1) As Object
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
    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        If (mUserManager.Login >= 1) Then
            ' Manage a recipe modified
            ManageRecipeModified()
            ' If the current recipe was not modified
            If Not (_recipeModified) Then
                ' Ask the user to enter the new reference
                frmEnterValue.Description = "Enter the new reference"
                frmEnterValue.EnableComma = True
                frmEnterValue.EnableHexadecimalLetters = True
                frmEnterValue.EnableLetters = True
                frmEnterValue.MaximumLength = 15
                frmEnterValue.Value = ""
                frmEnterValue.ShowDialog()
                ' If the user confirmed the operation
                If (frmEnterValue.Confirm) Then
                    ' If the recipe does not already exist
                    If Not (cWS02Recipe.Exists(frmEnterValue.Value)) Then
                        ' Set the station recipe values to the default ones
                        _WS02Recipe.SetToDefaultValues()
                        ' Set the reference code
                        _reference = frmEnterValue.Value
                        ' Save the station recipe
                        If (_WS02Recipe.Save(_reference)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while saving the station WS02 recipe"
                            frmMessage.ShowDialog()
                        End If
                        ' 
                        ' Set the station WS02 creation and modify dates and times
                        _WS02Recipe.CreationDate.Value = Format(Date.Now, "dd/MM/yyyy")
                        _WS02Recipe.CreationTime.Value = Format(Date.Now, "HH:mm:ss")
                        _WS02Recipe.ModifyDate.Value = _WS02Recipe.CreationDate.Value
                        _WS02Recipe.ModifyTime.Value = _WS02Recipe.CreationTime.Value
                        ' Show the recipe
                        ShowRecipe()
                    Else
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = String.Format("The recipe ""{0}"" already exists.", frmEnterValue.Value)
                        frmMessage.ShowDialog()
                    End If
                    ' If the recipe does not already exist
                    If Not (cWS03Recipe.Exists(frmEnterValue.Value)) Then
                        ' Set the station recipe values to the default ones
                        _WS03Recipe.SetToDefaultValues()
                        ' Set the reference code
                        _reference = frmEnterValue.Value
                        ' Save the station recipe
                        If (_WS03Recipe.Save(_reference)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while saving the station WS03 recipe"
                            frmMessage.ShowDialog()
                        End If
                        ' 
                        lblRecipe.Text = _reference
                        ' Set the station WS03 creation and modify dates and times
                        _WS03Recipe.CreationDate.Value = Format(Date.Now, "dd/MM/yyyy")
                        _WS03Recipe.CreationTime.Value = Format(Date.Now, "HH:mm:ss")
                        _WS03Recipe.ModifyDate.Value = _WS03Recipe.CreationDate.Value
                        _WS03Recipe.ModifyTime.Value = _WS03Recipe.CreationTime.Value
                        ' Show the recipe
                        ShowRecipe()
                    Else
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = String.Format("The recipe ""{0}"" already exists.", frmEnterValue.Value)
                        frmMessage.ShowDialog()
                    End If
                    ' If the recipe does not already exist
                    If Not (cWS04Recipe.Exists(frmEnterValue.Value)) Then
                        ' Set the station recipe values to the default ones
                        _WS04Recipe.SetToDefaultValues()
                        ' Set the reference code
                        _reference = frmEnterValue.Value
                        ' Save the station recipe
                        If (_WS04Recipe.Save(_reference)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while saving the station WS04 recipe"
                            frmMessage.ShowDialog()
                        End If
                        ' 
                        lblRecipe.Text = _reference
                        ' Set the station WS04 creation and modify dates and times
                        _WS04Recipe.CreationDate.Value = Format(Date.Now, "dd/MM/yyyy")
                        _WS04Recipe.CreationTime.Value = Format(Date.Now, "HH:mm:ss")
                        _WS04Recipe.ModifyDate.Value = _WS04Recipe.CreationDate.Value
                        _WS04Recipe.ModifyTime.Value = _WS04Recipe.CreationTime.Value
                        ' Show the recipe
                        ShowRecipe()
                    Else
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = String.Format("The recipe ""{0}"" already exists.", frmEnterValue.Value)
                        frmMessage.ShowDialog()
                    End If
                    ' If the recipe does not already exist
                    If Not (cWS05Recipe.Exists(frmEnterValue.Value)) Then
                        ' Set the station recipe values to the default ones
                        _WS05Recipe.SetToDefaultValues()
                        ' Set the reference code
                        _reference = frmEnterValue.Value
                        ' Save the station recipe
                        If (_WS05Recipe.Save(_reference)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while saving the station WS05 recipe"
                            frmMessage.ShowDialog()
                        End If
                        ' 
                        lblRecipe.Text = _reference
                        ' Set the station WS05 creation and modify dates and times
                        _WS05Recipe.CreationDate.Value = Format(Date.Now, "dd/MM/yyyy")
                        _WS05Recipe.CreationTime.Value = Format(Date.Now, "HH:mm:ss")
                        _WS05Recipe.ModifyDate.Value = _WS05Recipe.CreationDate.Value
                        _WS05Recipe.ModifyTime.Value = _WS05Recipe.CreationTime.Value
                        ' Show the recipe
                        ShowRecipe()
                    Else
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = String.Format("The recipe ""{0}"" already exists.", frmEnterValue.Value)
                        frmMessage.ShowDialog()
                    End If
                End If
            End If
        End If
    End Sub



    Private Sub btnCancelModifies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelModifies.Click
        If (mUserManager.Login >= 1) Then
            ' If reference is valid
            If (_reference <> "") Then
                ' If the user confirms
                frmMessage.MessageType = frmMessage.eType.Question
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Cancel all the modifies?"
                frmMessage.ShowDialog()
                If (frmMessage.MessageChoice = frmMessage.eChoice.Yes) Then
                    ' Load the station recipe
                    If (_WS02Recipe.Load(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while loading the station WS02 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Load the station recipe
                    If (_WS03Recipe.Load(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while loading the station WS03 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Load the station recipe
                    If (_WS04Recipe.Load(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while loading the station WS04 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Load the station recipe
                    If (_WS05Recipe.Load(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while loading the station WS05 recipe."
                        frmMessage.ShowDialog()
                    End If                    ' Show the recipe
                    ShowRecipe()
                End If
            End If
        End If
    End Sub



    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If (mUserManager.Login >= 1) Then
            ' If reference is valid and the user confirms
            If (_reference <> "") Then
                frmMessage.MessageType = frmMessage.eType.Question
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Delete the current recipe?"
                frmMessage.ShowDialog()
                If (frmMessage.MessageChoice = frmMessage.eChoice.Yes) Then
                    ' Delete the station recipe
                    If (cWS02Recipe.Delete(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while deleting the station WS02 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Delete the station recipe
                    If (cWS03Recipe.Delete(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while deleting the station WS03 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Delete the station recipe
                    If (cWS04Recipe.Delete(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while deleting the station WS04 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Delete the station recipe
                    If (cWS05Recipe.Delete(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while deleting the station WS05 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Clear the flag of recipe modified
                    _recipeModified = False
                    _reference = ""
                    lblRecipe.Text = _reference
                    tcRecipe.Visible = False
                    ' Show the recipe
                    ShowRecipe()
                End If
            End If
        End If
    End Sub



    Private Sub btnSaveModifies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveModifies.Click
        If (mUserManager.Login >= 1) Then
            ' If reference is valid
            If (_reference <> "") Then
                ' If the user confirms
                frmMessage.MessageType = frmMessage.eType.Question
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Save all the modifies?"
                frmMessage.ShowDialog()
                If (frmMessage.MessageChoice = frmMessage.eChoice.Yes) Then
                    ' Update the station modify dates and times
                    _WS02Recipe.ModifyDate.Value = Format(Date.Now, "dd/MM/yyyy")
                    _WS02Recipe.ModifyTime.Value = Format(Date.Now, "HH:mm:ss")
                    ' Save the WS02 recipe
                    If (_WS02Recipe.Save(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while saving the station WS02 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Update the station modify dates and times
                    _WS03Recipe.ModifyDate.Value = Format(Date.Now, "dd/MM/yyyy")
                    _WS03Recipe.ModifyTime.Value = Format(Date.Now, "HH:mm:ss")
                    ' Save the WS03 recipe
                    If (_WS03Recipe.Save(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while saving the station WS03 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Update the station modify dates and times
                    _WS04Recipe.ModifyDate.Value = Format(Date.Now, "dd/MM/yyyy")
                    _WS04Recipe.ModifyTime.Value = Format(Date.Now, "HH:mm:ss")
                    ' Save the WS04 recipe
                    If (_WS04Recipe.Save(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while saving the station WS04 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Update the station modify dates and times
                    _WS05Recipe.ModifyDate.Value = Format(Date.Now, "dd/MM/yyyy")
                    _WS05Recipe.ModifyTime.Value = Format(Date.Now, "HH:mm:ss")
                    ' Save the WS05 recipe
                    If (_WS05Recipe.Save(_reference)) Then
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Error while saving the station WS05 recipe."
                        frmMessage.ShowDialog()
                    End If
                    ' Clear the flag of recipe modified
                    _recipeModified = False
                    ' Show the recipe
                    ShowRecipe()
                End If
            End If
        End If
        MasterReference = False
    End Sub


    Private Sub frmRecipes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Disable the timer monitor
        tmrMonitor.Enabled = False
    End Sub


    Private Sub frmRecipes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim recipeNames(0) As String
        ' WS02
        ' Load the station recipe configuration
        If (_WS02Recipe.LoadConfiguration(mWS02Main.Settings.RecipeConfigurationPath)) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while loading the WS02 recipe configuration."
            frmMessage.ShowDialog()
        End If
        ' WS02
        InitWS02Tab()

        ' WS03
        ' Load the station recipe configuration
        If (_WS03Recipe.LoadConfiguration(mWS03Main.Settings.RecipeConfigurationPath)) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while loading the WS03 recipe configuration."
            frmMessage.ShowDialog()
        End If
        ' WS03
        InitWS03Tab()

        ' WS04
        ' Load the station recipe configuration
        If (_WS04Recipe.LoadConfiguration(mWS04Main.Settings.RecipeConfigurationPath)) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while loading the WS04 recipe configuration."
            frmMessage.ShowDialog()
        End If
        ' WS04
        InitWS04Tab()

        ' WS05
        ' Load the station recipe configuration
        If (_WS05Recipe.LoadConfiguration(mWS05Main.Settings.RecipeConfigurationPath)) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while loading the WS05 recipe configuration."
            frmMessage.ShowDialog()
        End If
        ' WS05
        InitWS05Tab()

        _reference = ""
        tcRecipe.Visible = False
        btnDelete.Enabled = False
        btnCopyFrom.Enabled = False
        btnCopyTo.Enabled = False
        btnCancelModifies.Enabled = False
        btnSaveModifies.Enabled = False
        btnCheckMaster.Enabled = False
        btnSaveMaster.Enabled = False
        lblModifyDate.Text = ""
        lblCreationDate.Text = ""
        lblRecipe.Text = _reference

        If mUserManager.UserIsMasterUser Or mUserManager.UserIsAdministrator Then
            btnSaveMaster.Visible = True
        Else
            btnSaveMaster.Visible = False
        End If

        ' Initialize the status bar
        mStatusBar.Initialize(ssStatusBar)
        ' Configurate and enable the timer monitor
        tmrMonitor.Interval = 500
        tmrMonitor.Enabled = True
    End Sub


    Private Sub InitRecipeTable(ByRef table As DataGridView, _
                                ByVal rowCount As Integer, _
                                ByRef objectReference() As Object, _
                                ByVal HeightMax As Integer)
        ' Reference to the table
        With table
            ' Disable the refresh
            .SuspendLayout()
            .Height = HeightMax
            ' Configurate the rows
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            .RowCount = rowCount
            ' Configurate the columns
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersVisible = False
            .ColumnCount = 3
            .Columns(0).ReadOnly = True
            .Columns(2).ReadOnly = True
            ' Configurate the cells
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.Font = New Font("Arial", 14)
            For i = 0 To .RowCount - 1
                .Rows(i).Height = 30
                .Item(0, i).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Item(1, i).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Item(2, i).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next
            ' Configurate the scrollbars and the column width
            If (.Rows(0).Height * .RowCount <= .Height) Then
                .Height = .Rows(0).Height * .RowCount
                .ScrollBars = ScrollBars.None
                .Columns(0).Width = CInt(0.5 * .Width)
                .Columns(1).Width = CInt(0.4 * .Width)
                .Columns(2).Width = CInt(0.1 * .Width)
            Else
                .ScrollBars = ScrollBars.Vertical
                .Columns(0).Width = CInt(0.5 * (.Width - SystemInformation.VerticalScrollBarWidth))
                .Columns(1).Width = CInt(0.4 * (.Width - SystemInformation.VerticalScrollBarWidth))
                .Columns(2).Width = CInt(0.1 * (.Width - SystemInformation.VerticalScrollBarWidth))
            End If
            ' Configurate the edit mode
            .EditMode = DataGridViewEditMode.EditOnKeystroke
            ' Disable multiple selection
            .MultiSelect = False
            ' Print the recipe value descriptions
            For i = 0 To .RowCount - 1
                If (TypeOf (objectReference(i)) Is String) Then
                    .Item(1, i).Value = objectReference(i)
                    .Item(0, i).Style.BackColor = Color.Yellow
                    .Item(1, i).Style.Font = New Font("Arial", 12, FontStyle.Bold)
                    .Item(1, i).Style.BackColor = Color.Yellow
                    .Item(2, i).Style.BackColor = Color.Yellow
                Else
                    .Item(0, i).Value = objectReference(i).Description
                    .Item(2, i).Value = objectReference(i).Units
                End If
            Next

            ' Enable the refresh
            .ResumeLayout(True)
        End With

    End Sub


    '**********************************************************************************
    ' WS02
    '**********************************************************************************
    Private Sub InitWS02Tab()
        ' Test Enable
        InitWS02TestEnablesTab()
        ' General information init tab
        InitWS02GeneralSettingsTab()
        ' Shape
        InitWS02PushShapeTab()
        ' Backlight
        InitWS02PushBacklightTab()
        ' TellTale
        InitWS02PushTellTaleTab()
        '
        InitWS02EMSTraceabilityTab()
        InitWS02WriteTraceabilityTab()
        InitWS02WriteConfigurationTab()
        InitWS02AnalogInputTab()
        InitWS02DigitalOutputTab()
        InitWS02PWMOutputTab()

    End Sub



    Private Function btnWS02Test(ByVal index As Integer) As Button
        Select Case index
            Case 0
                btnWS02Test = btnWS02Test0
            Case 1
                btnWS02Test = btnWS02Test1
            Case 2
                btnWS02Test = btnWS02Test2
            Case 3
                btnWS02Test = btnWS02Test3
            Case 4
                btnWS02Test = btnWS02Test4
            Case 5
                btnWS02Test = btnWS02Test5
            Case 6
                btnWS02Test = btnWS02Test6
            Case 7
                btnWS02Test = btnWS02Test7
            Case 8
                btnWS02Test = btnWS02Test8
            Case 9
                btnWS02Test = btnWS02Test9
            Case 10
                btnWS02Test = btnWS02Test10
            Case 11
                btnWS02Test = btnWS02Test11
            Case 12
                btnWS02Test = btnWS02Test12
            Case 13
                btnWS02Test = btnWS02Test13
            Case 14
                btnWS02Test = btnWS02Test14
            Case 15
                btnWS02Test = btnWS02Test15
            Case 16
                btnWS02Test = btnWS02Test16
            Case 17
                btnWS02Test = btnWS02Test17
            Case 18
                btnWS02Test = btnWS02Test18
            Case 19
                btnWS02Test = btnWS02Test19
            Case 20
                btnWS02Test = btnWS02Test20
            Case 21
                btnWS02Test = btnWS02Test21
            Case 22
                btnWS02Test = btnWS02Test22
            Case 23
                btnWS02Test = btnWS02Test23
            Case 24
                btnWS02Test = btnWS02Test24
            Case 25
                btnWS02Test = btnWS02Test25
            Case 26
                btnWS02Test = btnWS02Test26
            Case 27
                btnWS02Test = btnWS02Test27
            Case 28
                btnWS02Test = btnWS02Test28
            Case 29
                btnWS02Test = btnWS02Test29
            Case 30
                btnWS02Test = btnWS02Test30
            Case 31
                btnWS02Test = btnWS02Test31
            Case Else
                btnWS02Test = Nothing
        End Select
    End Function



    Private Sub InitWS02TestEnablesTab()
        Dim i As Integer
        Dim ii As Integer
        i = 0
        ' Initialize the indexes
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_NormalMode : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_EMSTraceability : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_AnalogInput : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_DigitalOutput : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_SHAPE_Select_Mirror : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_SHAPE_Folding_Mirror : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_SHAPE_Wili_Front : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_SHAPE_Wili_Rear : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_SHAPE_ChildrenLock : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_BACKLIGHT_Select_Mirror : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_BACKLIGHT_Folding_Mirror : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_BACKLIGHT_Wili_Front : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_BACKLIGHT_Wili_Rear : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_BACKLIGHT_ChildrenLock : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_BACKLIGHT_Homogeneity : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_TELLTALE_SelectMirror : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_TELLTALE_ChildrenLock : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_BezelConformity : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_DecorFrameConformity : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_MEMO_Option : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_PWL_Option : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_FOLDING_Option : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_WriteConfiguration : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_EV_Option : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_Temperature : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_PWMOutput : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_SHAPE_ChildrenLock2 : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_BACKLIGHT_ChildrenLock2 : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_PointsOnWili : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_CustomerInterface : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_Adapter : i += 1
        _WS02TestEnables(i) = _WS02Recipe.TestEnable_Mirrortiltingposition : i += 1


        ' Show the test descriptions
        For ii = 0 To i - 1
            btnWS02Test(ii).Text = _WS02TestEnables(ii).Description
        Next
    End Sub



    Private Sub InitWS02GeneralSettingsTab()
        _WS02GeneralSettings0(0) = New String(CType("Settings Camera Programm", Char()))
        _WS02GeneralSettings0(1) = _WS02Recipe.Camera_Programm
        _WS02GeneralSettings0(2) = _WS02Recipe.Camera_Recipe

        InitRecipeTable(dgvWS02GeneralSettings0, 3, _WS02GeneralSettings0, 715)
        '
        _WS02GeneralSettings1(0) = New String(CType("Limits on standard signals", Char()))
        _WS02GeneralSettings1(1) = _WS02Recipe.PowersupplyVbat
        _WS02GeneralSettings1(2) = New String(CType("Limits on Current Normal Mode", Char()))
        _WS02GeneralSettings1(3) = _WS02Recipe.PowersupplyIbat

        InitRecipeTable(dgvWS02GeneralSettings1, 4, _WS02GeneralSettings1, 715)
    End Sub


    ' Test Pictogram
    Private Sub InitWS02PushShapeTab()
        _WS02Push_PictogramConformity(0) = New String(CType("minimum Limits on Shape Push Mirror", Char()))
        _WS02Push_PictogramConformity(1) = _WS02Recipe.Push_Select_Left_pictogram_Conformity
        _WS02Push_PictogramConformity(2) = _WS02Recipe.Push_Select_Right_pictogram_Conformity
        _WS02Push_PictogramConformity(3) = _WS02Recipe.Push_Folding_pictogram_Conformity
        _WS02Push_PictogramConformity(4) = _WS02Recipe.Push_Adjust_UP_pictogram_Conformity
        _WS02Push_PictogramConformity(5) = _WS02Recipe.Push_Adjust_Down_pictogram_Conformity
        _WS02Push_PictogramConformity(6) = _WS02Recipe.Push_Adjust_Left_pictogram_Conformity
        _WS02Push_PictogramConformity(7) = _WS02Recipe.Push_Adjust_Right_pictogram_Conformity
        _WS02Push_PictogramConformity(8) = New String(CType("minimum Limits on Shape Windows Lifter", Char()))
        _WS02Push_PictogramConformity(9) = _WS02Recipe.Windows_Lifter_Front_Left_pictogram_Conformity
        _WS02Push_PictogramConformity(10) = _WS02Recipe.Windows_Lifter_Front_Right_pictogram_Conformity
        _WS02Push_PictogramConformity(11) = _WS02Recipe.Windows_Lifter_Rear_Left_pictogram_Conformity
        _WS02Push_PictogramConformity(12) = _WS02Recipe.Windows_Lifter_Rear_Right_pictogram_Conformity
        _WS02Push_PictogramConformity(13) = New String(CType("minimum Limits on Shape Push Children Lock", Char()))
        _WS02Push_PictogramConformity(14) = _WS02Recipe.Push_Children_Lock_pictogram_Conformity
        _WS02Push_PictogramConformity(15) = _WS02Recipe.Push_Children_Lock2_pictogram_Conformity
        _WS02Push_PictogramConformity(16) = New String(CType("minimum Limits on Adapter", Char()))
        _WS02Push_PictogramConformity(17) = _WS02Recipe.Apater_Front_pictogram_Conformity
        _WS02Push_PictogramConformity(18) = _WS02Recipe.Apater_Rear_pictogram_Conformity
        _WS02Push_PictogramConformity(19) = New String(CType("minimum Limits on defects area", Char()))
        _WS02Push_PictogramConformity(20) = _WS02Recipe.Push_Select_Left_pictogram_Defect_Area
        _WS02Push_PictogramConformity(21) = _WS02Recipe.Push_Select_Right_pictogram_Defect_Area
        _WS02Push_PictogramConformity(22) = _WS02Recipe.Push_Folding_pictogram_Defect_Area
        _WS02Push_PictogramConformity(23) = _WS02Recipe.Push_Adjust_UP_pictogram_Defect_Area
        _WS02Push_PictogramConformity(24) = _WS02Recipe.Push_Adjust_Down_pictogram_Defect_Area
        _WS02Push_PictogramConformity(25) = _WS02Recipe.Push_Adjust_Left_pictogram_Defect_Area
        _WS02Push_PictogramConformity(26) = _WS02Recipe.Push_Adjust_Right_pictogram_Defect_Area
        _WS02Push_PictogramConformity(27) = _WS02Recipe.Windows_Lifter_Front_Left_pictogram_Defect_Area
        _WS02Push_PictogramConformity(28) = _WS02Recipe.Windows_Lifter_Front_Right_pictogram_Defect_Area
        _WS02Push_PictogramConformity(29) = _WS02Recipe.Windows_Lifter_Rear_Left_pictogram_Defect_Area
        _WS02Push_PictogramConformity(30) = _WS02Recipe.Windows_Lifter_Rear_Right_pictogram_Defect_Area
        _WS02Push_PictogramConformity(31) = _WS02Recipe.Push_Children_Lock_pictogram_Defect_Area
        _WS02Push_PictogramConformity(32) = _WS02Recipe.Push_Children_Lock2_pictogram_Defect_Area

        InitRecipeTable(dgvWS02SHAPE1, 33, _WS02Push_PictogramConformity, 690)

        Dim i As Integer = 0
        _WS02PictogramPoint_Interface(i) = New String(CType("Mirror Ring RGB", Char())) : i += 1
        _WS02PictogramPoint_Interface(i) = _WS02Recipe.Ring_Red : i += 1
        _WS02PictogramPoint_Interface(i) = _WS02Recipe.Ring_Green : i += 1
        _WS02PictogramPoint_Interface(i) = _WS02Recipe.Ring_Blue : i += 1

        _WS02PictogramPoint_Interface(i) = New String(CType("Points on Wili", Char())) : i += 1
        _WS02PictogramPoint_Interface(i) = _WS02Recipe.PointsOn_Front_Left : i += 1
        _WS02PictogramPoint_Interface(i) = _WS02Recipe.PointsOn_Front_Right : i += 1
        _WS02PictogramPoint_Interface(i) = _WS02Recipe.PointsOn_Rear_Left : i += 1
        _WS02PictogramPoint_Interface(i) = _WS02Recipe.PointsOn_Rear_Right : i += 1
        _WS02PictogramPoint_Interface(i) = New String(CType("Customer Interfaces", Char())) : i += 1
        For index = 0 To 19
            _WS02PictogramPoint_Interface(i) = _WS02Recipe.CustomerInterface(index) : i += 1
        Next
        InitRecipeTable(dgvWS02SHAPE2, i, _WS02PictogramPoint_Interface, 690)

    End Sub

    Private Sub InitWS02WriteTraceabilityTab()
        _WS02WriteTraceability(0) = New String(CType("Write Traceability MMS", Char()))
        _WS02WriteTraceability(1) = _WS02Recipe.Write_MMS_Final_Product_Reference
        _WS02WriteTraceability(2) = _WS02Recipe.Write_MMS_Final_Product_Index
        _WS02WriteTraceability(3) = _WS02Recipe.Write_MMS_Valeo_Final_Product_Plant
        _WS02WriteTraceability(4) = _WS02Recipe.Write_MMS_Deviation_Number
        _WS02WriteTraceability(5) = New String(CType("Customer Part Number", Char()))
        _WS02WriteTraceability(6) = _WS02Recipe.Write_CustomerPartNumber
        _WS02WriteTraceability(7) = _WS02Recipe.Write_HARDWARE_Version
        _WS02WriteTraceability(8) = _WS02Recipe.Laser_IDCode

        InitRecipeTable(dgvWS02WriteTraceability, 9, _WS02WriteTraceability, 715)

    End Sub

    Private Sub InitWS02WriteConfigurationTab()
        _WS02WriteConfiguration(0) = New String(CType("Write Product Configuration SUV", Char()))
        _WS02WriteConfiguration(1) = _WS02Recipe.WRITE_SW_Coding
        _WS02WriteConfiguration(2) = _WS02Recipe.WRITE_Baclight_Coding
        _WS02WriteConfiguration(3) = _WS02Recipe.WRITE_BackLight_PWM
        _WS02WriteConfiguration(4) = _WS02Recipe.WRITE_MLTelltale_PWM
        _WS02WriteConfiguration(5) = _WS02Recipe.WRITE_MRTelltale_PWM
        _WS02WriteConfiguration(6) = _WS02Recipe.WRITE_CLTelltale_PWM
        _WS02WriteConfiguration(7) = New String(CType("Write Product Configuration EV", Char()))
        _WS02WriteConfiguration(8) = _WS02Recipe.WRITE_HW_Coding

        InitRecipeTable(dgvWS02WriteConfiguration, 9, _WS02WriteConfiguration, 715)

    End Sub


    Private Sub InitWS02EMSTraceabilityTab()
        '
        Dim i As Integer
        i = 0
        _WS02ReadEMSTraceability(i) = New String(CType("Read Traceability EMS", Char())) : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.PCBA_Number_Reference : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.PCBA_Number_Index : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.PCBA_Plant_Line : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.PCBA_ManufacturingDate : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.PCBA_SerialNumber : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.PCBA_DeviationNumber : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.LED_BIN_PT_White_RSA : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.LED_BIN_PT_RED : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.LED_BIN_PT_Yellow : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.LED_BIN_PT_White_Nissan : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.Major_SoftwareVersion : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.Minor_SoftwareVersion : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.Major_NVMversion : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.Minor_NVMversion : i = i + 1
        _WS02ReadEMSTraceability(i) = _WS02Recipe.SW_checksum : i = i + 1
      
        InitRecipeTable(dgvWS02EMSTraceability, i, _WS02ReadEMSTraceability, 690)
    End Sub

    Private Sub InitWS02AnalogInputTab()
        '
        Dim i As Integer
        i = 0
        _WS02ReadAnalogInput(i) = New String(CType("Read Analog Input Variant", Char())) : i = i + 1
        _WS02ReadAnalogInput(i) = _WS02Recipe.ADC_UCAD_Variant_1 : i = i + 1
        _WS02ReadAnalogInput(i) = _WS02Recipe.ADC_UCAD_Variant_2 : i = i + 1
        _WS02ReadAnalogInput(i) = New String(CType("Read Analog Input Memo", Char())) : i = i + 1
        _WS02ReadAnalogInput(i) = _WS02Recipe.ADC_CURSEUR_UP_DN : i = i + 1
        _WS02ReadAnalogInput(i) = _WS02Recipe.ADC_CURSEUR_LEFT_RIGHT : i = i + 1

        InitRecipeTable(dgvWS02AnalogInput, i, _WS02ReadAnalogInput, 715)
    End Sub

    Private Sub InitWS02DigitalOutputTab()
        '
        Dim i As Integer
        Dim ii As Integer
        i = 0
        _WS02DigitalOutput_PWL(i) = New String(CType("O_UP_FRONT_PASSENGER_CDE", Char())) : i = i + 1
        _WS02DigitalOutput_PWL(i) = _WS02Recipe.DO_UP_Front_Passenger : i = i + 1
        _WS02DigitalOutput_PWL(i) = New String(CType("O_DN_FRONT_PASSENGER_CDE", Char())) : i = i + 1
        _WS02DigitalOutput_PWL(i) = _WS02Recipe.DO_DN_Front_Passenger : i = i + 1
        _WS02DigitalOutput_PWL(i) = New String(CType("O_LOCAL_WL_SWITCHES_INHIBITION_CDE", Char())) : i = i + 1
        _WS02DigitalOutput_PWL(i) = _WS02Recipe.DO_Switch_Inhibtion : i = i + 1
        _WS02DigitalOutput_PWL(i) = New String(CType("O_DOWN_REAR_RIGHT_CDE", Char())) : i = i + 1
        _WS02DigitalOutput_PWL(i) = _WS02Recipe.DO_DN_Rear_Right : i = i + 1
        _WS02DigitalOutput_PWL(i) = New String(CType("O_UP_REAR_RIGHT_CDE", Char())) : i = i + 1
        _WS02DigitalOutput_PWL(i) = _WS02Recipe.DO_UP_Rear_Right : i = i + 1
        _WS02DigitalOutput_PWL(i) = New String(CType("O_DOWN_REAR_LEFT_CDE", Char())) : i = i + 1
        _WS02DigitalOutput_PWL(i) = _WS02Recipe.DO_DN_Rear_Left : i = i + 1
        _WS02DigitalOutput_PWL(i) = New String(CType("O_UP_REAR_LEFT_CDE", Char())) : i = i + 1
        _WS02DigitalOutput_PWL(i) = _WS02Recipe.DO_UP_Rear_Left : i = i + 1

        InitRecipeTable(dgvWS02DigitalOutputPWL, i, _WS02DigitalOutput_PWL, 715)

        i = 0
        _WS02DigitalOutput(i) = New String(CType("Standard Signal", Char())) : i = i + 1
        For ii = 0 To 20
            _WS02DigitalOutput(i) = _WS02Recipe.Sdt_Signal(ii) : i = i + 1
        Next
        InitRecipeTable(dgvWS02DigitalOutputStdSignal, i, _WS02DigitalOutput, 600)

        i = 0
        _WS02DigitalOutput_RTRV_G(i) = New String(CType("CDE_H/B_RTRV_G", Char())) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.DO_CDE_HB_RTRV_G(0) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.DO_CDE_HB_RTRV_G(1) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = New String(CType("SGN_COMMUN_RTRV_G", Char())) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.D0_SGN_COMMUN_MOT_RTRV_G : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = New String(CType("CDE_D/G_RTRV_G", Char())) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.DO_CDE_DG_RTRV_G(0) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.DO_CDE_RBT_RTRV_G(1) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.ADC_CDE_DG_RTRV_G(0) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.ADC_CDE_DG_RTRV_G(1) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = New String(CType("CDE_+_RBT_RTRV_G", Char())) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.DO_CDE_RBT_RTRV_G(0) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.ADC_CDE_RBT_RTRV_G(0) : i = i + 1
        _WS02DigitalOutput_RTRV_G(i) = _WS02Recipe.ADC_CDE_RBT_RTRV_G(1) : i = i + 1

        InitRecipeTable(dgvWS02DigitalOutputRTRVG, i, _WS02DigitalOutput_RTRV_G, 715)

        i = 0
        _WS02DigitalOutput_RTRV_D(i) = New String(CType("SGN_COMMUN_RTRV_D", Char())) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = _WS02Recipe.D0_SGN_COMMUN_MOT_RTRV_D : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = New String(CType("CDE_D/G_RTRV_D", Char())) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = _WS02Recipe.DO_CDE_DG_RTRV_D(0) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = _WS02Recipe.DO_CDE_RBT_RTRV_D(1) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = _WS02Recipe.ADC_CDE_DG_RTRV_D(0) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = _WS02Recipe.ADC_CDE_DG_RTRV_D(1) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = New String(CType("CDE_+_RBT_RTRV_D", Char())) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = _WS02Recipe.DO_CDE_RBT_RTRV_D(0) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = _WS02Recipe.ADC_CDE_RBT_RTRV_D(0) : i = i + 1
        _WS02DigitalOutput_RTRV_D(i) = _WS02Recipe.ADC_CDE_RBT_RTRV_D(1) : i = i + 1

        InitRecipeTable(dgvWS02DigitalOutputRTRVD, i, _WS02DigitalOutput_RTRV_D, 715)


    End Sub

    Private Sub InitWS02PWMOutputTab()
        '
        Dim i As Integer
        Dim ii As Integer
        i = 0
        'UCDO_PWM_2SR_FR_MID_SIM_RL_LEFT
        _WS02PWMOutput(i) = New String("UCDA SINGLE SW DIAG") : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_WLAP_Voltage_LOW_X1_12 : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_WLAP_Voltage_HIGH_X1_12 : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_WLAP_Duty_Cycle_X1_12 : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_WLAP_Frequency_X1_12 : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_WLAP_ADC_VALUE : i = i + 1
        _WS02PWMOutput(i) = New String("UCDA DOOR LOCK DIAG") : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_DOOR_LOCK_Voltage_LOW_X1_24 : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_DOOR_LOCK_Voltage_HIGH_X1_24 : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_DOOR_LOCK_Duty_Cycle_X1_24 : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_DOOR_LOCK_Frequency_X1_24 : i = i + 1
        _WS02PWMOutput(i) = _WS02Recipe.External_Backlight_DOOR_LOCK_ADC_VALUE : i = i + 1

        InitRecipeTable(dgvWS02PWMOutput, i, _WS02PWMOutput, 715)



    End Sub

    ' Backlight
    Private Sub InitWS02PushBacklightTab()
        Dim i As Integer
        i = 0
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Push Select Mirror Left", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(0) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(0) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Select_Left_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Select_Left_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Select_Left_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Push Select Mirror Right", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(1) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(1) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Select_Right_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Select_Right_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Select_Right_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Push Folding", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(2) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(2) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Folding_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Folding_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Folding_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Push Adjust Mirror UP", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(3) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(3) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_UP_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_UP_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_UP_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Push Adjust Mirror Down", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(4) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(4) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_DOWN_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_DOWN_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_DOWN_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Push Adjust Mirror Left", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(5) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(5) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Left_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Left_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Left_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Push Adjust Mirror Right", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(6) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(6) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Right_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Right_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Right_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Windows Lifter Front Left", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(7) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(7) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Front_Left_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Front_Left_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Front_Left_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Windows Lifter Front Right", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(8) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(8) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Front_Right_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Front_Right_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Front_Right_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Windows Lifter Rear Left", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(9) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(9) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Rear_Left_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Rear_Left_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Rear_Left_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Windows Lifter Rear Right", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(10) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(10) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Rear_Right_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Rear_Right_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Rear_Right_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("Limits on Backlight Push Children Lock", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(11) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(11) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Children_Lock_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Children_Lock_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Children_Lock_minimum_homogeneity : i = i + 1
        '_WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_A(11) : i = i + 1
        '_WS02Push_Backlight(i) = _WS02Recipe.Correlation_Factor_B(11) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Children_Lock2_Intensity : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Children_Lock2_RSQ : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Children_Lock2_minimum_homogeneity : i = i + 1
        _WS02Push_Backlight(i) = New String(CType("minimum Limits on defects area", Char())) : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Select_Left_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Select_Right_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Folding_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_UP_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Down_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Left_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Adjust_Right_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Front_Left_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Front_Right_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Rear_Left_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Windows_Lifter_Rear_Right_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Children_Lock_backlight_Defect_Area : i = i + 1
        _WS02Push_Backlight(i) = _WS02Recipe.Push_Children_Lock2_backlight_Defect_Area : i = i + 1

        InitRecipeTable(dgvWS02BACKLIGHT, i, _WS02Push_Backlight, 690)

        i = 0
        _WS02Push_Backlight_1(i) = New String(CType("Color Correlation Factor", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Correlation_BackColorx_Factor_A : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Correlation_BackColorx_Factor_B : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Correlation_BackColory_Factor_A : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Correlation_BackColory_Factor_B : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Select Mirror Left", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Left_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Left_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Left_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Left_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Left_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Left_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Select Mirror Right", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Right_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Right_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Right_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Right_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Right_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Select_Right_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Folding", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Folding_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Folding_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Folding_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Folding_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Folding_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Folding_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Adjust Mirror UP", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_UP_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_UP_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_UP_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_UP_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_UP_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_UP_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Adjust Mirror Down", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_DOWN_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_DOWN_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_DOWN_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_DOWN_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_DOWN_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_DOWN_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Adjust Mirror Left", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Left_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Left_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Left_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Left_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Left_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Left_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Adjust Mirror Right", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Right_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Right_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Right_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Right_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Right_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Adjust_Right_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Windows Lifter front Left", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Windows Lifter front Right", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Windows Lifter Rear Left", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Windows Lifter Rear Right", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Children Lock", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock_Polygon_Fxy : i = i + 1
        _WS02Push_Backlight_1(i) = New String(CType("Setting on Children Lock2", Char())) : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock2_Polygon_Axy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock2_Polygon_Bxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock2_Polygon_Cxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock2_Polygon_Dxy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock2_Polygon_Exy : i = i + 1
        _WS02Push_Backlight_1(i) = _WS02Recipe.Push_Children_Lock2_Polygon_Fxy : i = i + 1

        InitRecipeTable(dgvWS02BACKLIGHT_1, i, _WS02Push_Backlight_1, 690)


    End Sub

    Private Sub InitWS02PushTellTaleTab()
        Dim i As Integer
        i = 0
        _WS02TELLTALE(i) = New String(CType("Color Correlation Factor", Char())) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_TellColorx_Factor_A : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_TellColorx_Factor_B : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_TellColory_Factor_A : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_TellColory_Factor_B : i = i + 1
        _WS02TELLTALE(i) = New String(CType("WaveLength Correlation Factor", Char())) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_TellWavelength_Factor_A : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_TellWavelength_Factor_B : i = i + 1
        _WS02TELLTALE(i) = New String(CType("Limits on Select Left", Char())) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_Factor_A(12) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_Factor_B(12) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Left_Saturation : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Left_WaveLenght : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Left_Intensity : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Polygon_Axy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Polygon_Bxy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Polygon_Cxy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Polygon_Dxy : i = i + 1
        _WS02TELLTALE(i) = New String(CType("Limits on Select Right", Char())) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_Factor_A(13) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_Factor_B(13) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Right_Saturation : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Right_WaveLenght : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Right_Intensity : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Right_Polygon_Axy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Right_Polygon_Bxy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Right_Polygon_Cxy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Right_Polygon_Dxy : i = i + 1
        _WS02TELLTALE(i) = New String(CType("Limits on Children Lock", Char())) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_Factor_A(14) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.Correlation_Factor_B(14) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Saturation : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_WaveLenght : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Intensity : i = i + 1
        _WS02TELLTALE(i) = New String(CType("minimum Limits on defects area", Char())) : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Left_Defect_Area : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Select_Right_Defect_Area : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Defect_Area : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Polygon_Axy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Polygon_Bxy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Polygon_Cxy : i = i + 1
        _WS02TELLTALE(i) = _WS02Recipe.TELLTALE_Children_Lock_Polygon_Dxy : i = i + 1

        InitRecipeTable(dgvWS02TELLTALE, i, _WS02TELLTALE, 715)

    End Sub

    '**********************************************************************************
    ' WS03
    '**********************************************************************************
    Private Sub InitWS03Tab()
        ' Test Enable
        InitWS03TestEnablesTab()
        ' General information init tab
        InitWS03GeneralSettingsTab()
        '
        InitWS03FrontLeftTab()
        '
        InitWS03FrontRightTab()
        '
        InitWS03MirrorFoldingTab()
        '
        InitWS03JamaFunctionTab()

    End Sub

    Private Function btnWS03Test(ByVal index As Integer) As Button
        Select Case index
            Case 0
                btnWS03Test = btnWS03Test0
            Case 1
                btnWS03Test = btnWS03Test1
            Case 2
                btnWS03Test = btnWS03Test2
            Case 3
                btnWS03Test = btnWS03Test3
            Case 4
                btnWS03Test = btnWS03Test4
            Case 5
                btnWS03Test = btnWS03Test5
            Case 6
                btnWS03Test = btnWS03Test6
            Case 7
                btnWS03Test = btnWS03Test7
            Case 8
                btnWS03Test = btnWS03Test8
            Case 9
                btnWS03Test = btnWS03Test9
            Case 10
                btnWS03Test = btnWS03Test10
            Case 11
                btnWS03Test = btnWS03Test11
            Case 12
                btnWS03Test = btnWS03Test12

            Case Else
                btnWS03Test = Nothing
        End Select
    End Function



    Private Sub InitWS03TestEnablesTab()
        Dim i As Integer

        ' Initialize the indexes
        _WS03TestEnables(0) = _WS03Recipe.TestEnable_Front_Left_PUSH_Electrical
        _WS03TestEnables(1) = _WS03Recipe.TestEnable_Front_Left_PUSH_Strenght
        _WS03TestEnables(2) = _WS03Recipe.TestEnable_Front_Left_PULL_Electrical
        _WS03TestEnables(3) = _WS03Recipe.TestEnable_Front_Left_PULL_Strenght
        _WS03TestEnables(4) = _WS03Recipe.TestEnable_Front_Right_PUSH_Electrical
        _WS03TestEnables(5) = _WS03Recipe.TestEnable_Front_Right_PUSH_Strenght
        _WS03TestEnables(6) = _WS03Recipe.TestEnable_Front_Right_PULL_Electrical
        _WS03TestEnables(7) = _WS03Recipe.TestEnable_Front_Right_PULL_Strenght
        _WS03TestEnables(8) = _WS03Recipe.TestEnable_ChildrenLock_Electrical
        _WS03TestEnables(9) = _WS03Recipe.TestEnable_ChildrenLock_Strenght
        _WS03TestEnables(10) = _WS03Recipe.TestEnable_Driver_Right
        _WS03TestEnables(11) = _WS03Recipe.TestEnable_Function_Jama
        _WS03TestEnables(12) = _WS03Recipe.TestEnable_EV_Option

        ' Show the test descriptions
        For i = 0 To 12
            btnWS03Test(i).Text = _WS03TestEnables(i).Description
        Next
    End Sub

    Private Sub InitWS03GeneralSettingsTab()
        _WS03GeneralSettings1(0) = New String(CType("Limits Min / Max", Char()))
        _WS03GeneralSettings1(1) = _WS03Recipe.StdSign_Powersupply(0)
        _WS03GeneralSettings1(2) = _WS03Recipe.StdSign_Powersupply(1)
        InitRecipeTable(dgvWS03GeneralSettings1, 3, _WS03GeneralSettings1, 715)

    End Sub

    Private Sub InitWS03JamaFunctionTab()
        _WS03JamaFunction(0) = New String(CType("Jama Function Limits Min / Max", Char()))
        _WS03JamaFunction(1) = _WS03Recipe.Jama_Function_Down
        InitRecipeTable(dgvWS03JamaFunction, 2, _WS03JamaFunction, 715)

    End Sub

    Private Sub InitWS03FrontLeftTab()
        Dim i As Integer
        i = 0
        _WS03FrontLeftSettings0(i) = New String(CType("Approachement Push settings ", Char())) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.X_correction_approachment_Front_Left_Push : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Y_correction_approachment_Front_Left_Push : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Z_correction_approachment_Front_Left_Push : i += 1
        _WS03FrontLeftSettings0(i) = New String(CType("Final Position Push ", Char())) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Z_Vector_Final_Position_Front_Left_Push : i += 1
        _WS03FrontLeftSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Correlation_Factor_Force_A(cWS03Results.eWindowsLifterTest.FrontLeft_Push) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Correlation_Factor_Force_B(cWS03Results.eWindowsLifterTest.FrontLeft_Push) : i += 1
        _WS03FrontLeftSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_A(cWS03Results.eWindowsLifterTest.FrontLeft_Push) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_B(cWS03Results.eWindowsLifterTest.FrontLeft_Push) : i += 1
        _WS03FrontLeftSettings0(i) = New String(CType("Approachement Pull settings ", Char())) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.X_correction_approachment_Front_Left_Pull : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Y_correction_approachment_Front_Left_Pull : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Z_correction_approachment_Front_Left_Pull : i += 1
        _WS03FrontLeftSettings0(i) = New String(CType("Final Position Pull ", Char())) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Z_Vector_Final_Position_Front_Left_Pull : i += 1
        _WS03FrontLeftSettings0(i) = New String(CType("Lab Correlation Factor Pull Force", Char())) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Correlation_Factor_Force_A(cWS03Results.eWindowsLifterTest.FrontLeft_Pull) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Correlation_Factor_Force_B(cWS03Results.eWindowsLifterTest.FrontLeft_Pull) : i += 1
        _WS03FrontLeftSettings0(i) = New String(CType("Lab Correlation Factor Pull Stroke", Char())) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_A(cWS03Results.eWindowsLifterTest.FrontLeft_Pull) : i += 1
        _WS03FrontLeftSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_B(cWS03Results.eWindowsLifterTest.FrontLeft_Pull) : i += 1
        InitRecipeTable(dgvWS03FrontLeft_0, i, _WS03FrontLeftSettings0, 715)

        i = 0
        _WS03FrontLeftSettings1(i) = New String(CType("Limits Min / Max Push Aptic Test", Char())) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Xs1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_dXs1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Xs2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_dXs2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Fe(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = New String(CType("Limits Min / Max Push Electric Test", Char())) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_XCe1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_XCe2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Xe(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = New String(CType("Number of Electrical State", Char())) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.Front_Left_Push_Number_State : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.Front_Left_Push_DiffS2Ce1 : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.Front_Left_Push_DiffS5Ce2 : i += 1
        _WS03FrontLeftSettings1(i) = New String(CType("Limits Min / Max Pull Aptic Test", Char())) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Xs1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_dXs1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Xs2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_dXs2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Fe(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = New String(CType("Limits Min / Max Pull Electric Test", Char())) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_XCe1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_XCe2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.WL_Xe(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft) : i += 1
        _WS03FrontLeftSettings1(i) = New String(CType("Number of Electrical State", Char())) : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.Front_Left_Pull_Number_State : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.Front_Left_Pull_DiffS2Ce1 : i += 1
        _WS03FrontLeftSettings1(i) = _WS03Recipe.Front_Left_Pull_DiffS5Ce2 : i += 1

        InitRecipeTable(dgvWS03FrontLeft_1, i, _WS03FrontLeftSettings1, 700)

    End Sub

    Private Sub InitWS03FrontRightTab()
        Dim i As Integer
        i = 0
        _WS03FrontRightSettings0(i) = New String(CType("Approachement Push settings ", Char())) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.X_correction_approachment_Front_Right_Push : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Y_correction_approachment_Front_Right_Push : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Z_correction_approachment_Front_Right_Push : i += 1
        _WS03FrontRightSettings0(i) = New String(CType("Final Position Push ", Char())) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Z_Vector_Final_Position_Front_Right_Push : i += 1
        _WS03FrontRightSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Correlation_Factor_Force_A(cWS03Results.eWindowsLifterTest.FrontRight_Push) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Correlation_Factor_Force_B(cWS03Results.eWindowsLifterTest.FrontRight_Push) : i += 1
        _WS03FrontRightSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_A(cWS03Results.eWindowsLifterTest.FrontRight_Push) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_B(cWS03Results.eWindowsLifterTest.FrontRight_Push) : i += 1
        _WS03FrontRightSettings0(i) = New String(CType("Approachement Pull settings ", Char())) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.X_correction_approachment_Front_Right_Pull : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Y_correction_approachment_Front_Right_Pull : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Z_correction_approachment_Front_Right_Pull : i += 1
        _WS03FrontRightSettings0(i) = New String(CType("Final Position Pull ", Char())) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Z_Vector_Final_Position_Front_Right_Pull : i += 1
        _WS03FrontRightSettings0(i) = New String(CType("Lab Correlation Factor Pull Force", Char())) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Correlation_Factor_Force_A(cWS03Results.eWindowsLifterTest.FrontRight_Pull) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Correlation_Factor_Force_B(cWS03Results.eWindowsLifterTest.FrontRight_Pull) : i += 1
        _WS03FrontRightSettings0(i) = New String(CType("Lab Correlation Factor Pull Stroke", Char())) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_A(cWS03Results.eWindowsLifterTest.FrontRight_Pull) : i += 1
        _WS03FrontRightSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_B(cWS03Results.eWindowsLifterTest.FrontRight_Pull) : i += 1
        InitRecipeTable(dgvWS03FrontRight_0, i, _WS03FrontRightSettings0, 715)

        i = 0
        _WS03FrontRightSettings1(i) = New String(CType("Limits Min / Max Push Aptic Test", Char())) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Xs1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_dXs1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Xs2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_dXs2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Fe(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = New String(CType("Limits Min / Max Push Electric Test", Char())) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_XCe1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_XCe2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Xe(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = New String(CType("Number of Electrical State", Char())) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.Front_Right_Push_Number_State : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.Front_Right_Push_DiffS2Ce1 : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.Front_Right_Push_DiffS5Ce2 : i += 1
        _WS03FrontRightSettings1(i) = New String(CType("Limits Min / Max Pull Aptic Test", Char())) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Xs1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_dXs1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Xs2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_dXs2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Fe(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = New String(CType("Limits Min / Max Pull Electric Test", Char())) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_XCe1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_XCe2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.WL_Xe(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight) : i += 1
        _WS03FrontRightSettings1(i) = New String(CType("Number of Electrical State", Char())) : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.Front_Right_Pull_Number_State : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.Front_Right_Pull_DiffS2Ce1 : i += 1
        _WS03FrontRightSettings1(i) = _WS03Recipe.Front_Right_Pull_DiffS5Ce2 : i += 1

        InitRecipeTable(dgvWS03FrontRight_1, i, _WS03FrontRightSettings1, 700)

    End Sub

    Private Sub InitWS03MirrorFoldingTab()
        Dim i As Integer
        i = 0
        _WS03ChildrenLockSettings0(i) = New String(CType("Approachement Push settings ", Char())) : i += 1
        _WS03ChildrenLockSettings0(i) = _WS03Recipe.X_correction_approachment_ChildrenLock : i += 1
        _WS03ChildrenLockSettings0(i) = _WS03Recipe.Y_correction_approachment_ChildrenLock : i += 1
        _WS03ChildrenLockSettings0(i) = _WS03Recipe.Z_correction_approachment_ChildrenLock : i += 1
        _WS03ChildrenLockSettings0(i) = New String(CType("Final Position Push ", Char())) : i += 1
        _WS03ChildrenLockSettings0(i) = _WS03Recipe.Z_Vector_Final_Position_ChildrenLock : i += 1
        _WS03ChildrenLockSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS03ChildrenLockSettings0(i) = _WS03Recipe.Correlation_Factor_Force_A(4) : i += 1
        _WS03ChildrenLockSettings0(i) = _WS03Recipe.Correlation_Factor_Force_B(4) : i += 1
        _WS03ChildrenLockSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS03ChildrenLockSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_A(4) : i += 1
        _WS03ChildrenLockSettings0(i) = _WS03Recipe.Correlation_Factor_Stroke_B(4) : i += 1

        InitRecipeTable(dgvWS03ChildrenLock_0, i, _WS03ChildrenLockSettings0, 715)

        i = 0
        _WS03ChildrenLockSettings1(i) = New String(CType("Limits Min / Max Push Aptic Test", Char())) : i += 1
        _WS03ChildrenLockSettings1(i) = _WS03Recipe.CL_Fs1_F1 : i += 1
        _WS03ChildrenLockSettings1(i) = _WS03Recipe.CL_Xs1 : i += 1
        _WS03ChildrenLockSettings1(i) = _WS03Recipe.CL_dFs1_Haptic_1 : i += 1
        _WS03ChildrenLockSettings1(i) = _WS03Recipe.CL_dXs1 : i += 1
        _WS03ChildrenLockSettings1(i) = New String(CType("Limits Min / Max Push Electric Test", Char())) : i += 1
        _WS03ChildrenLockSettings1(i) = _WS03Recipe.CL_XCe1 : i += 1
        _WS03ChildrenLockSettings1(i) = _WS03Recipe.CL_Xe : i += 1
        _WS03ChildrenLockSettings1(i) = _WS03Recipe.CL_DiffS2Ce1 : i += 1

        InitRecipeTable(dgvWS03ChildrenLock_1, i, _WS03ChildrenLockSettings1, 700)

    End Sub



    '**********************************************************************************
    ' WS04
    '**********************************************************************************
    Private Sub InitWS04Tab()
        ' Test Enable
        InitWS04TestEnablesTab()
        ' General information init tab
        InitWS04GeneralSettingsTab()
        '
        InitWS04RearLeftTab()
        '
        InitWS04RearRightTab()
        '
        InitWS04MirrorFoldingTab()
    End Sub

    Private Function btnWS04Test(ByVal index As Integer) As Button
        Select Case index
            Case 0
                btnWS04Test = btnWS04Test0
            Case 1
                btnWS04Test = btnWS04Test1
            Case 2
                btnWS04Test = btnWS04Test2
            Case 3
                btnWS04Test = btnWS04Test3
            Case 4
                btnWS04Test = btnWS04Test4
            Case 5
                btnWS04Test = btnWS04Test5
            Case 6
                btnWS04Test = btnWS04Test6
            Case 7
                btnWS04Test = btnWS04Test7
            Case 8
                btnWS04Test = btnWS04Test8
            Case 9
                btnWS04Test = btnWS04Test9
            Case 10
                btnWS04Test = btnWS04Test10

            Case Else
                btnWS04Test = Nothing
        End Select
    End Function



    Private Sub InitWS04TestEnablesTab()
        Dim i As Integer

        ' Initialize the indexes
        _WS04TestEnables(0) = _WS04Recipe.TestEnable_Rear_Left_PUSH_Electrical
        _WS04TestEnables(1) = _WS04Recipe.TestEnable_Rear_Left_PUSH_Strenght
        _WS04TestEnables(2) = _WS04Recipe.TestEnable_Rear_Left_PULL_Electrical
        _WS04TestEnables(3) = _WS04Recipe.TestEnable_Rear_Left_PULL_Strenght
        _WS04TestEnables(4) = _WS04Recipe.TestEnable_Rear_Right_PUSH_Electrical
        _WS04TestEnables(5) = _WS04Recipe.TestEnable_Rear_Right_PUSH_Strenght
        _WS04TestEnables(6) = _WS04Recipe.TestEnable_Rear_Right_PULL_Electrical
        _WS04TestEnables(7) = _WS04Recipe.TestEnable_Rear_Right_PULL_Strenght
        _WS04TestEnables(8) = _WS04Recipe.TestEnable_MirrorFolding_Electrical
        _WS04TestEnables(9) = _WS04Recipe.TestEnable_MirrorFolding_Strenght
        _WS04TestEnables(10) = _WS04Recipe.TestEnable_EV_Option

        ' Show the test descriptions
        For i = 0 To 10
            btnWS04Test(i).Text = _WS04TestEnables(i).Description
        Next
    End Sub

    Private Sub InitWS04GeneralSettingsTab()
        _WS04GeneralSettings1(0) = New String(CType("Limits Min / Max", Char()))
        _WS04GeneralSettings1(1) = _WS04Recipe.StdSign_Powersupply(0)
        _WS04GeneralSettings1(2) = _WS04Recipe.StdSign_Powersupply(1)
        InitRecipeTable(dgvWS04GeneralSettings1, 3, _WS04GeneralSettings1, 715)

    End Sub


    Private Sub InitWS04RearLeftTab()
        Dim i As Integer
        i = 0
        _WS04RearLeftSettings0(i) = New String(CType("Approachement Push settings ", Char())) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.X_correction_approachment_Rear_Left_Push : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Y_correction_approachment_Rear_Left_Push : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Z_correction_approachment_Rear_Left_Push : i += 1
        _WS04RearLeftSettings0(i) = New String(CType("Final Position Push ", Char())) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Z_Vector_Final_Position_Rear_Left_Push : i += 1
        _WS04RearLeftSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Correlation_Factor_Force_A(cWS04Results.eWindowsLifterTest.RearLeft_Push) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Correlation_Factor_Force_B(cWS04Results.eWindowsLifterTest.RearLeft_Push) : i += 1
        _WS04RearLeftSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_A(cWS04Results.eWindowsLifterTest.RearLeft_Push) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_B(cWS04Results.eWindowsLifterTest.RearLeft_Push) : i += 1
        _WS04RearLeftSettings0(i) = New String(CType("Approachement Pull settings ", Char())) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.X_correction_approachment_Rear_Left_Pull : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Y_correction_approachment_Rear_Left_Pull : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Z_correction_approachment_Rear_Left_Pull : i += 1
        _WS04RearLeftSettings0(i) = New String(CType("Final Position Pull ", Char())) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Z_Vector_Final_Position_Rear_Left_Pull : i += 1
        _WS04RearLeftSettings0(i) = New String(CType("Lab Correlation Factor Pull Force", Char())) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Correlation_Factor_Force_A(cWS04Results.eWindowsLifterTest.RearLeft_Pull) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Correlation_Factor_Force_B(cWS04Results.eWindowsLifterTest.RearLeft_Pull) : i += 1
        _WS04RearLeftSettings0(i) = New String(CType("Lab Correlation Factor Pull Stroke", Char())) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_A(cWS04Results.eWindowsLifterTest.RearLeft_Pull) : i += 1
        _WS04RearLeftSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_B(cWS04Results.eWindowsLifterTest.RearLeft_Pull) : i += 1
        InitRecipeTable(dgvWS04RearLeft_0, i, _WS04RearLeftSettings0, 715)

        i = 0
        _WS04RearLeftSettings1(i) = New String(CType("Limits Min / Max Push Aptic Test", Char())) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Xs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_dXs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Xs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_dXs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Fe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = New String(CType("Limits Min / Max Push Electric Test", Char())) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_XCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_XCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Xe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = New String(CType("Number of Electrical State", Char())) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.Rear_Left_Push_Number_State : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = New String(CType("Limits Min / Max Pull Aptic Test", Char())) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Xs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_dXs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Xs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_dXs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Fe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = New String(CType("Limits Min / Max Pull Electric Test", Char())) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_XCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_XCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_Xe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = New String(CType("Number of Electrical State", Char())) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.Rear_Left_Pull_Number_State : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1
        _WS04RearLeftSettings1(i) = _WS04Recipe.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) : i += 1

        InitRecipeTable(dgvWS04RearLeft_1, i, _WS04RearLeftSettings1, 700)

    End Sub

    Private Sub InitWS04RearRightTab()
        Dim i As Integer
        i = 0
        _WS04RearRightSettings0(i) = New String(CType("Approachement Push settings ", Char())) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.X_correction_approachment_Rear_Right_Push : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Y_correction_approachment_Rear_Right_Push : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Z_correction_approachment_Rear_Right_Push : i += 1
        _WS04RearRightSettings0(i) = New String(CType("Final Position Push ", Char())) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Z_Vector_Final_Position_Rear_Right_Push : i += 1
        _WS04RearRightSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Correlation_Factor_Force_A(cWS04Results.eWindowsLifterTest.RearRight_Push) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Correlation_Factor_Force_B(cWS04Results.eWindowsLifterTest.RearRight_Push) : i += 1
        _WS04RearRightSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_A(cWS04Results.eWindowsLifterTest.RearRight_Push) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_B(cWS04Results.eWindowsLifterTest.RearRight_Push) : i += 1
        _WS04RearRightSettings0(i) = New String(CType("Approachement Pull settings ", Char())) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.X_correction_approachment_Rear_Right_Pull : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Y_correction_approachment_Rear_Right_Pull : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Z_correction_approachment_Rear_Right_Pull : i += 1
        _WS04RearRightSettings0(i) = New String(CType("Final Position Pull ", Char())) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Z_Vector_Final_Position_Rear_Right_Pull : i += 1
        _WS04RearRightSettings0(i) = New String(CType("Lab Correlation Factor Pull Force", Char())) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Correlation_Factor_Force_A(cWS04Results.eWindowsLifterTest.RearRight_Pull) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Correlation_Factor_Force_B(cWS04Results.eWindowsLifterTest.RearRight_Pull) : i += 1
        _WS04RearRightSettings0(i) = New String(CType("Lab Correlation Factor Pull Stroke", Char())) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_A(cWS04Results.eWindowsLifterTest.RearRight_Pull) : i += 1
        _WS04RearRightSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_B(cWS04Results.eWindowsLifterTest.RearRight_Pull) : i += 1
        InitRecipeTable(dgvWS04RearRight_0, i, _WS04RearRightSettings0, 700)

        i = 0
        _WS04RearRightSettings1(i) = New String(CType("Limits Min / Max Push Aptic Test", Char())) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Xs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_dXs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Xs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_dXs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Fe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = New String(CType("Limits Min / Max Push Electric Test", Char())) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_XCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_XCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Xe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = New String(CType("Number of Electrical State", Char())) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.Rear_Right_Push_Number_State : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = New String(CType("Limits Min / Max Pull Aptic Test", Char())) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Xs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_dXs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Xs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_dXs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Fe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = New String(CType("Limits Min / Max Pull Electric Test", Char())) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_XCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_XCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_Xe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = New String(CType("Number of Electrical State", Char())) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.Rear_Right_Pull_Number_State : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1
        _WS04RearRightSettings1(i) = _WS04Recipe.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) : i += 1

        InitRecipeTable(dgvWS04RearRight_1, i, _WS04RearRightSettings1, 700)

    End Sub

    Private Sub InitWS04MirrorFoldingTab()
        Dim i As Integer
        i = 0
        _WS04MirrorFoldingSettings0(i) = New String(CType("Approachement Push settings ", Char())) : i += 1
        _WS04MirrorFoldingSettings0(i) = _WS04Recipe.X_correction_approachment_MirrorFolding : i += 1
        _WS04MirrorFoldingSettings0(i) = _WS04Recipe.Y_correction_approachment_MirrorFolding : i += 1
        _WS04MirrorFoldingSettings0(i) = _WS04Recipe.Z_correction_approachment_MirrorFolding : i += 1
        _WS04MirrorFoldingSettings0(i) = New String(CType("Final Position Push ", Char())) : i += 1
        _WS04MirrorFoldingSettings0(i) = _WS04Recipe.Z_Vector_Final_Position_MirrorFolding : i += 1
        _WS04MirrorFoldingSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS04MirrorFoldingSettings0(i) = _WS04Recipe.Correlation_Factor_Force_A(4) : i += 1
        _WS04MirrorFoldingSettings0(i) = _WS04Recipe.Correlation_Factor_Force_B(4) : i += 1
        _WS04MirrorFoldingSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS04MirrorFoldingSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_A(4) : i += 1
        _WS04MirrorFoldingSettings0(i) = _WS04Recipe.Correlation_Factor_Stroke_B(4) : i += 1

        InitRecipeTable(dgvWS04MirrorFolding_0, i, _WS04MirrorFoldingSettings0, 715)

        i = 0
        _WS04MirrorFoldingSettings1(i) = New String(CType("Limits Min / Max Push Aptic Test", Char())) : i += 1
        _WS04MirrorFoldingSettings1(i) = _WS04Recipe.MF_Fs1_F1 : i += 1
        _WS04MirrorFoldingSettings1(i) = _WS04Recipe.MF_Xs1 : i += 1
        _WS04MirrorFoldingSettings1(i) = _WS04Recipe.MF_dFs1_Haptic_1 : i += 1
        _WS04MirrorFoldingSettings1(i) = _WS04Recipe.MF_dXs1 : i += 1
        _WS04MirrorFoldingSettings1(i) = New String(CType("Limits Min / Max Push Electric Test", Char())) : i += 1
        _WS04MirrorFoldingSettings1(i) = _WS04Recipe.MF_XCe1 : i += 1
        _WS04MirrorFoldingSettings1(i) = _WS04Recipe.MF_Xe : i += 1
        _WS04MirrorFoldingSettings1(i) = _WS04Recipe.MF_DiffS2Ce1 : i += 1

        InitRecipeTable(dgvWS04MirrorFolding_1, i, _WS04MirrorFoldingSettings1, 700)

    End Sub

    '**********************************************************************************
    ' WS05
    '**********************************************************************************
    Private Sub InitWS05Tab()
        ' Test Enable
        InitWS05TestEnablesTab()
        ' General information init tab
        InitWS05GeneralSettingsTab()
        '
        InitWS05MirrorTab()
    End Sub

    Private Function btnWS05Test(ByVal index As Integer) As Button
        Select Case index
            Case 0
                btnWS05Test = btnWS05Test0
            Case 1
                btnWS05Test = btnWS05Test1
            Case 2
                btnWS05Test = btnWS05Test2

            Case Else
                btnWS05Test = Nothing
        End Select
    End Function



    Private Sub InitWS05TestEnablesTab()
        Dim i As Integer

        ' Initialize the indexes
        _WS05TestEnables(0) = _WS05Recipe.TestEnable_Mirror_Electrical
        _WS05TestEnables(1) = _WS05Recipe.TestEnable_Mirror_Strenght
        _WS05TestEnables(2) = _WS05Recipe.TestEnable_EV_Option
        ' Show the test descriptions
        For i = 0 To 2
            btnWS05Test(i).Text = _WS05TestEnables(i).Description
        Next
    End Sub

    Private Sub InitWS05GeneralSettingsTab()
        _WS05GeneralSettings1(0) = New String(CType("Limits Min / Max", Char()))
        _WS05GeneralSettings1(1) = _WS05Recipe.StdSign_Powersupply(0)
        _WS05GeneralSettings1(2) = _WS05Recipe.StdSign_Powersupply(1)
        InitRecipeTable(dgvWS05GeneralSettings1, 3, _WS05GeneralSettings1, 715)

    End Sub



    Private Sub InitWS05MirrorTab()
        Dim i As Integer
        Dim ii As Integer
        i = 0
        _WS05MirrorSettings0(i) = New String(CType("Approachement Push settings UP ", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.X_correction_approachment_Push(0) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Y_correction_approachment_Push(0) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_correction_approachment_Push(0) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Final Position Push UP", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_Vector_Final_Position_Push(0) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_A(0) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_B(0) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_A(0) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_B(0) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Approachement Push settings DN ", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.X_correction_approachment_Push(1) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Y_correction_approachment_Push(1) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_correction_approachment_Push(1) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Final Position Push DN", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_Vector_Final_Position_Push(1) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_A(1) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_B(1) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_A(1) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_B(1) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Approachement Push settings Right ", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.X_correction_approachment_Push(2) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Y_correction_approachment_Push(2) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_correction_approachment_Push(2) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Final Position Push Right", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_Vector_Final_Position_Push(2) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_A(2) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_B(2) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_A(2) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_B(2) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Approachement Push settings Left ", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.X_correction_approachment_Push(3) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Y_correction_approachment_Push(3) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_correction_approachment_Push(3) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Final Position Push Left", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_Vector_Final_Position_Push(3) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_A(3) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_B(3) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_A(3) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_B(3) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Approachement Push settings Select Right ", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.X_correction_approachment_Push(4) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Y_correction_approachment_Push(4) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_correction_approachment_Push(4) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Final Position Push Select Right", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_Vector_Final_Position_Push(4) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_A(4) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_B(4) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_A(4) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_B(4) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Approachement Push settings Select Left ", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.X_correction_approachment_Push(5) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Y_correction_approachment_Push(5) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_correction_approachment_Push(5) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Final Position Push Select Left", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Z_Vector_Final_Position_Push(5) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Force", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_A(5) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Force_B(5) : i += 1
        _WS05MirrorSettings0(i) = New String(CType("Lab Correlation Factor Push Stroke", Char())) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_A(5) : i += 1
        _WS05MirrorSettings0(i) = _WS05Recipe.Correlation_Factor_Stroke_B(5) : i += 1

        InitRecipeTable(dgvWS05Mirror_0, i, _WS05MirrorSettings0, 715)

        i = 0
        For ii = 0 To 5
            If ii = mWS05Main.eMirrorPush.MirrorUP Then
                _WS05MirrorSettings1(i) = New String(CType("Mirror Push UP", Char())) : i += 1
            ElseIf ii = mWS05Main.eMirrorPush.MirrorDN Then
                _WS05MirrorSettings1(i) = New String(CType("Mirror Push Down", Char())) : i += 1
            ElseIf ii = mWS05Main.eMirrorPush.MirrorMR Then
                _WS05MirrorSettings1(i) = New String(CType("Mirror Push Right", Char())) : i += 1
            ElseIf ii = mWS05Main.eMirrorPush.MirrorML Then
                _WS05MirrorSettings1(i) = New String(CType("Mirror Push Left", Char())) : i += 1
            ElseIf ii = mWS05Main.eMirrorPush.MirrorSR Then
                _WS05MirrorSettings1(i) = New String(CType("Mirror Push Select Right", Char())) : i += 1
            ElseIf ii = mWS05Main.eMirrorPush.MirrorSL Then
                _WS05MirrorSettings1(i) = New String(CType("Mirror Push Select Left", Char())) : i += 1
            End If
            _WS05MirrorSettings1(i) = New String(CType("Limits Min / Max Push Aptic Test", Char())) : i += 1
            _WS05MirrorSettings1(i) = _WS05Recipe.Mirror_Fs1_F1(mWS05Main.eMirrorPush.MirrorUP + ii) : i += 1
            _WS05MirrorSettings1(i) = _WS05Recipe.Mirror_Xs1(mWS05Main.eMirrorPush.MirrorUP + ii) : i += 1
            _WS05MirrorSettings1(i) = _WS05Recipe.Mirror_dFs1_Haptic_1(mWS05Main.eMirrorPush.MirrorUP + ii) : i += 1
            _WS05MirrorSettings1(i) = _WS05Recipe.Mirror_dXs1(mWS05Main.eMirrorPush.MirrorUP + ii) : i += 1
            _WS05MirrorSettings1(i) = New String(CType("Limits Min / Max Push Electric Test", Char())) : i += 1
            _WS05MirrorSettings1(i) = _WS05Recipe.Mirror_XCe1(mWS05Main.eMirrorPush.MirrorUP + ii) : i += 1
            _WS05MirrorSettings1(i) = _WS05Recipe.Mirror_Xe(mWS05Main.eMirrorPush.MirrorUP + ii) : i += 1
            _WS05MirrorSettings1(i) = _WS05Recipe.Mirror_DiffS2Ce1(mWS05Main.eMirrorPush.MirrorUP + ii) : i += 1
        Next

        InitRecipeTable(dgvWS05Mirror_1, i, _WS05MirrorSettings1, 715)

    End Sub

    Private Sub ManageRecipeModified()
        Dim userChoice As frmMessage.eChoice

        ' If the current recipe was modified
        If (_recipeModified) Then
            ' Ask the user to cancel or save the modifies
            frmMessage.MessageType = frmMessage.eType.Question
            frmMessage.MessageButtons = frmMessage.eButtons.YesNoCancel
            frmMessage.Message = "The current recipe was modified: save the modifies?"
            frmMessage.ShowDialog()
            userChoice = frmMessage.MessageChoice
            If (userChoice = frmMessage.eChoice.Yes) Then
                ' Update the station WS02 modify dates and times
                _WS02Recipe.ModifyDate.Value = Format(Date.Now, "dd/MM/yyyy")
                _WS02Recipe.ModifyTime.Value = Format(Date.Now, "HH:mm:ss")
                ' Save the WS02 recipe
                If (_WS02Recipe.Save(_reference)) Then
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while saving the station WS02 recipe."
                    frmMessage.ShowDialog()
                End If
                ' Update the station WS03 modify dates and times
                _WS03Recipe.ModifyDate.Value = Format(Date.Now, "dd/MM/yyyy")
                _WS03Recipe.ModifyTime.Value = Format(Date.Now, "HH:mm:ss")
                ' Save the WS03 recipe
                If (_WS03Recipe.Save(_reference)) Then
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while saving the station WS03 recipe."
                    frmMessage.ShowDialog()
                End If
                ' Update the station WS04 modify dates and times
                _WS04Recipe.ModifyDate.Value = Format(Date.Now, "dd/MM/yyyy")
                _WS04Recipe.ModifyTime.Value = Format(Date.Now, "HH:mm:ss")
                ' Save the WS04 recipe
                If (_WS04Recipe.Save(_reference)) Then
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while saving the station WS04 recipe."
                    frmMessage.ShowDialog()
                End If
                ' Update the station WS05 modify dates and times
                _WS05Recipe.ModifyDate.Value = Format(Date.Now, "dd/MM/yyyy")
                _WS05Recipe.ModifyTime.Value = Format(Date.Now, "HH:mm:ss")
                ' Save the WS05 recipe
                If (_WS05Recipe.Save(_reference)) Then
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while saving the station WS05 recipe."
                    frmMessage.ShowDialog()
                End If
                ' Show the recipe
                ShowRecipe()
                ' Clear the flag of recipe modified
                _recipeModified = False
            ElseIf (userChoice = frmMessage.eChoice.No) Then
                ' Clear the flag of recipe modified
                _recipeModified = False
            End If
        End If
    End Sub



    Private Sub pbLogoValeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbLogoValeo.Click
        ' Manage a recipe modified
        ManageRecipeModified()
        ' If the current recipe was not modified
        If Not (_recipeModified) Then
            mUserManager.Logout()
            ' Close the form
            Me.Close()
        End If
    End Sub



    Private Sub ShowRecipe()
        ' If the reference is not empty
        If (_reference <> "") Then
            ' Show the main tab control
            tcRecipe.Visible = True
            btnDelete.Enabled = True
            btnCopyFrom.Enabled = True
            btnCopyTo.Enabled = True
            btnCancelModifies.Enabled = True
            btnSaveModifies.Enabled = True
            btnCheckMaster.Enabled = True
            btnSaveMaster.Enabled = True

            ' Show station WS02 recipe
            ShowWS02Recipe()
            ' Show station WS03 recipe
            ShowWS03Recipe()
            ' Show station WS04 recipe
            ShowWS04Recipe()
            ' Show station WS05 recipe
            ShowWS05Recipe()
        Else    ' Otherwise, if the recipe is empty
            ' Hide the main tab control
            tcRecipe.Visible = False
        End If
    End Sub


    Private Sub ShowWS02Recipe()
        Dim i As Integer
        ' Show the test enables
        For i = 0 To 31
            btnWS02Test(i).BackColor = CType(IIf(CBool(_WS02TestEnables(i).Value), Color.LawnGreen, Color.Red), Color)
        Next
        ' Show the general settings
        ShowValues(dgvWS02GeneralSettings0, _WS02GeneralSettings0)
        ShowValues(dgvWS02GeneralSettings1, _WS02GeneralSettings1)

        ShowValues(dgvWS02SHAPE1, _WS02Push_PictogramConformity)

        ShowValues(dgvWS02SHAPE2, _WS02PictogramPoint_Interface)

        ShowValues(dgvWS02BACKLIGHT, _WS02Push_Backlight)
        ShowValues(dgvWS02BACKLIGHT_1, _WS02Push_Backlight_1)

        ShowValues(dgvWS02TELLTALE, _WS02TELLTALE)

        ShowValues(dgvWS02DigitalOutputPWL, _WS02ReadTraceability)
        ShowValues(dgvWS02EMSTraceability, _WS02ReadEMSTraceability)
        ShowValues(dgvWS02WriteTraceability, _WS02WriteTraceability)
        ShowValues(dgvWS02WriteConfiguration, _WS02WriteConfiguration)
        ShowValues(dgvWS02AnalogInput, _WS02ReadAnalogInput)

        ShowValues(dgvWS02DigitalOutputPWL, _WS02DigitalOutput_PWL)
        ShowValues(dgvWS02DigitalOutputStdSignal, _WS02DigitalOutput)
        ShowValues(dgvWS02DigitalOutputRTRVD, _WS02DigitalOutput_RTRV_D)
        ShowValues(dgvWS02DigitalOutputRTRVG, _WS02DigitalOutput_RTRV_G)

        ShowValues(dgvWS02PWMOutput, _WS02PWMOutput)

    End Sub

    Private Sub ShowWS03Recipe()
        Dim i As Integer

        ' Show the creation and modify dates and times
        lblCreationDate.Text = _WS03Recipe.CreationDate.StringValue &
                               ", " & _WS03Recipe.CreationTime.StringValue
        lblModifyDate.Text = _WS03Recipe.ModifyDate.StringValue &
                               ", " & _WS03Recipe.ModifyTime.StringValue
        ' Show the test enables
        For i = 0 To 12
            btnWS03Test(i).BackColor = CType(IIf(CBool(_WS03TestEnables(i).Value), Color.LawnGreen, Color.Red), Color)
        Next
        ' Show the general settings
        ShowValues(dgvWS03GeneralSettings0, _WS03GeneralSettings0)
        ShowValues(dgvWS03GeneralSettings1, _WS03GeneralSettings1)

        ShowValues(dgvWS03JamaFunction, _WS03JamaFunction)

        ShowValues(dgvWS03FrontRight_0, _WS03FrontRightSettings0)
        ShowValues(dgvWS03FrontRight_1, _WS03FrontRightSettings1)

        ShowValues(dgvWS03FrontLeft_0, _WS03FrontLeftSettings0)
        ShowValues(dgvWS03FrontLeft_1, _WS03FrontLeftSettings1)

        ShowValues(dgvWS03ChildrenLock_0, _WS03ChildrenLockSettings0)
        ShowValues(dgvWS03ChildrenLock_1, _WS03ChildrenLockSettings1)



    End Sub

    Private Sub ShowWS04Recipe()
        Dim i As Integer

        ' Show the creation and modify dates and times
        lblCreationDate.Text = _WS04Recipe.CreationDate.StringValue &
                               ", " & _WS04Recipe.CreationTime.StringValue
        lblModifyDate.Text = _WS04Recipe.ModifyDate.StringValue &
                               ", " & _WS04Recipe.ModifyTime.StringValue
        ' Show the test enables
        For i = 0 To 10
            btnWS04Test(i).BackColor = CType(IIf(CBool(_WS04TestEnables(i).Value), Color.LawnGreen, Color.Red), Color)
        Next
        ' Show the general settings
        ShowValues(dgvWS04GeneralSettings0, _WS04GeneralSettings0)
        ShowValues(dgvWS04GeneralSettings1, _WS04GeneralSettings1)

        ShowValues(dgvWS04RearRight_0, _WS04RearRightSettings0)
        ShowValues(dgvWS04RearRight_1, _WS04RearRightSettings1)

        ShowValues(dgvWS04RearLeft_0, _WS04RearLeftSettings0)
        ShowValues(dgvWS04RearLeft_1, _WS04RearLeftSettings1)

        ShowValues(dgvWS04MirrorFolding_0, _WS04MirrorFoldingSettings0)
        ShowValues(dgvWS04MirrorFolding_1, _WS04MirrorFoldingSettings1)

    End Sub

    Private Sub ShowWS05Recipe()
        Dim i As Integer

        ' Show the creation and modify dates and times
        lblCreationDate.Text = _WS05Recipe.CreationDate.StringValue &
                               ", " & _WS05Recipe.CreationTime.StringValue
        lblModifyDate.Text = _WS05Recipe.ModifyDate.StringValue &
                               ", " & _WS05Recipe.ModifyTime.StringValue
        ' Show the test enables
        For i = 0 To 2
            btnWS05Test(i).BackColor = CType(IIf(CBool(_WS05TestEnables(i).Value), Color.LawnGreen, Color.Red), Color)
        Next
        ' Show the general settings
        ShowValues(dgvWS05GeneralSettings0, _WS05GeneralSettings0)
        ShowValues(dgvWS05GeneralSettings1, _WS05GeneralSettings1)

        ShowValues(dgvWS05Mirror_0, _WS05MirrorSettings0)
        ShowValues(dgvWS05Mirror_1, _WS05MirrorSettings1)

    End Sub

    Private Sub ShowValues(ByRef table As DataGridView,
                           ByRef objectReference() As Object)
        Dim recipeValue As cRecipeValue

        table.SuspendLayout()
        For i = 0 To table.RowCount - 1
            If (TypeOf (objectReference(i)) Is cRecipeValue) Then
                recipeValue = CType(objectReference(i), cRecipeValue)
                If (recipeValue.ValueType = cRecipeValue.eValueType.BCDRange Or
                    recipeValue.ValueType = cRecipeValue.eValueType.HexRange Or
                    recipeValue.ValueType = cRecipeValue.eValueType.IntegerRange Or
                    recipeValue.ValueType = cRecipeValue.eValueType.SingleRange) Then
                    table.Item(1, i).Value = recipeValue.StringMinimumLimit & " / " & recipeValue.StringMaximumLimit
                ElseIf (recipeValue.ValueType = cRecipeValue.eValueType.BCDValue Or
                        recipeValue.ValueType = cRecipeValue.eValueType.HexValue Or
                        recipeValue.ValueType = cRecipeValue.eValueType.IntegerValue Or
                        recipeValue.ValueType = cRecipeValue.eValueType.SingleValue Or
                        recipeValue.ValueType = cRecipeValue.eValueType.StringValue) Then
                    table.Item(1, i).Value = recipeValue.StringValue
                End If
                If _recipeModified = False Then
                    table.Item(1, i).Style.BackColor = Color.White
                End If
            End If
        Next


        table.RefreshEdit()
        table.ResumeLayout()
    End Sub




    Private Sub btnWS02Test_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02Test0.Click,
                                                                                                        btnWS02Test1.Click,
                                                                                                        btnWS02Test2.Click,
                                                                                                        btnWS02Test3.Click,
                                                                                                        btnWS02Test4.Click,
                                                                                                        btnWS02Test5.Click,
                                                                                                        btnWS02Test6.Click,
                                                                                                        btnWS02Test7.Click,
                                                                                                        btnWS02Test8.Click,
                                                                                                        btnWS02Test9.Click,
                                                                                                        btnWS02Test10.Click,
                                                                                                        btnWS02Test11.Click,
                                                                                                        btnWS02Test12.Click,
                                                                                                        btnWS02Test13.Click,
                                                                                                        btnWS02Test14.Click,
                                                                                                        btnWS02Test15.Click,
                                                                                                        btnWS02Test16.Click,
                                                                                                        btnWS02Test17.Click,
                                                                                                        btnWS02Test18.Click,
                                                                                                        btnWS02Test19.Click,
                                                                                                        btnWS02Test20.Click,
                                                                                                        btnWS02Test21.Click,
                                                                                                        btnWS02Test22.Click,
                                                                                                        btnWS02Test23.Click,
                                                                                                        btnWS02Test24.Click,
                                                                                                        btnWS02Test25.Click,
                                                                                                        btnWS02Test27.Click,
                                                                                                        btnWS02Test26.Click,
                                                                                                        btnWS02Test30.Click,
                                                                                                        btnWS02Test29.Click,
                                                                                                        btnWS02Test28.Click,
                                                                                                        btnWS02Test31.Click

        Dim i As Integer
        Dim button As Button

        If (mUserManager.Login >= 1) Then
            button = CType(sender, Button)
            For i = 0 To 31
                If (button Is btnWS02Test(i)) Then
                    Exit For
                End If
            Next
            _WS02TestEnables(i).Value = Not (_WS02TestEnables(i).Value)
        End If
        _recipeModified = True
        ShowRecipe()

    End Sub

    Private Sub btnWS03Test_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03Test0.Click,
                                                                                                       btnWS03Test1.Click,
                                                                                                       btnWS03Test2.Click,
                                                                                                       btnWS03Test3.Click,
                                                                                                       btnWS03Test4.Click,
                                                                                                       btnWS03Test5.Click,
                                                                                                       btnWS03Test6.Click,
                                                                                                       btnWS03Test7.Click,
                                                                                                       btnWS03Test8.Click,
                                                                                                       btnWS03Test9.Click,
                                                                                                       btnWS03Test10.Click,
                                                                                                       btnWS03Test11.Click,
                                                                                                        btnWS03Test12.Click

        Dim i As Integer
        Dim button As Button

        If (mUserManager.Login >= 1) Then
            button = CType(sender, Button)
            For i = 0 To 12
                If (button Is btnWS03Test(i)) Then
                    Exit For
                End If
            Next
            _WS03TestEnables(i).Value = Not (_WS03TestEnables(i).Value)

            _recipeModified = True

            ShowRecipe()
        End If
    End Sub

    Private Sub btnWS04Test_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS04Test0.Click,
                                                                                                       btnWS04Test1.Click,
                                                                                                       btnWS04Test2.Click,
                                                                                                       btnWS04Test3.Click,
                                                                                                       btnWS04Test4.Click,
                                                                                                       btnWS04Test5.Click,
                                                                                                       btnWS04Test6.Click,
                                                                                                       btnWS04Test7.Click,
                                                                                                       btnWS04Test8.Click,
                                                                                                       btnWS04Test9.Click,
                                                                                                       btnWS04Test10.Click

        Dim i As Integer
        Dim button As Button

        If (mUserManager.Login >= 1) Then
            button = CType(sender, Button)
            For i = 0 To 10
                If (button Is btnWS04Test(i)) Then
                    Exit For
                End If
            Next
            _WS04TestEnables(i).Value = Not (_WS04TestEnables(i).Value)

            _recipeModified = True

            ShowRecipe()
        End If
    End Sub

    Private Sub btnWS05Test_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS05Test0.Click,
                                                                                                   btnWS05Test1.Click,
                                                                                                   btnWS05Test2.Click

        Dim i As Integer
        Dim button As Button

        If (mUserManager.Login >= 1) Then
            button = CType(sender, Button)
            For i = 0 To 2
                If (button Is btnWS05Test(i)) Then
                    Exit For
                End If
            Next
            _WS05TestEnables(i).Value = Not (_WS05TestEnables(i).Value)

            _recipeModified = True

            ShowRecipe()
        End If
    End Sub

    Private Sub EditValue(ByRef recipeValue As cRecipeValue,
                          ByRef table As DataGridView)

        If (mUserManager.Login >= 1) Then
            If (recipeValue.ValueType = cRecipeValue.eValueType.BCDRange Or
                recipeValue.ValueType = cRecipeValue.eValueType.HexRange Or
                recipeValue.ValueType = cRecipeValue.eValueType.IntegerRange Or
                recipeValue.ValueType = cRecipeValue.eValueType.SingleRange) Then
                frmEnterRange.Description = recipeValue.Description
                frmEnterRange.EnableComma = (recipeValue.ValueType = cRecipeValue.eValueType.SingleRange)
                frmEnterRange.EnableHexadecimalLetters = (recipeValue.ValueType = cRecipeValue.eValueType.HexRange)
                frmEnterRange.EnableLetters = False
                frmEnterRange.Minimum = recipeValue.StringMinimumLimit
                frmEnterRange.Maximum = recipeValue.StringMaximumLimit
                frmEnterRange.ShowDialog()
                If (frmEnterRange.Confirm) Then
                    recipeValue.MinimumLimit = frmEnterRange.Minimum
                    recipeValue.MaximumLimit = frmEnterRange.Maximum
                    _recipeModified = True
                    ShowRecipe()
                    table.CurrentCell.Style.BackColor = Color.GreenYellow
                End If

            ElseIf (recipeValue.ValueType = cRecipeValue.eValueType.BCDValue Or
                    recipeValue.ValueType = cRecipeValue.eValueType.HexValue Or
                    recipeValue.ValueType = cRecipeValue.eValueType.IntegerValue Or
                    recipeValue.ValueType = cRecipeValue.eValueType.SingleValue) Then
                frmEnterValue.Description = recipeValue.Description
                frmEnterValue.EnableComma = (recipeValue.ValueType = cRecipeValue.eValueType.SingleValue)
                frmEnterValue.EnableHexadecimalLetters = (recipeValue.ValueType = cRecipeValue.eValueType.HexValue)
                frmEnterValue.EnableLetters = False
                frmEnterValue.MaximumLength = 10
                frmEnterValue.Value = recipeValue.StringValue
                frmEnterValue.ShowDialog()
                If (frmEnterValue.Confirm) Then
                    recipeValue.Value = frmEnterValue.Value
                    _recipeModified = True
                    ShowRecipe()
                    table.CurrentCell.Style.BackColor = Color.GreenYellow
                End If

            ElseIf (recipeValue.ValueType = cRecipeValue.eValueType.StringValue) Then
                frmEnterValue.Description = recipeValue.Description
                frmEnterValue.EnableComma = True
                frmEnterValue.EnableHexadecimalLetters = True
                frmEnterValue.EnableLetters = True
                frmEnterValue.MaximumLength = recipeValue.MaximumLength
                frmEnterValue.Value = recipeValue.StringValue
                frmEnterValue.ShowDialog()
                If (frmEnterValue.Confirm) Then
                    recipeValue.Value = frmEnterValue.Value
                    _recipeModified = True
                    ShowRecipe()
                    table.CurrentCell.Style.BackColor = Color.GreenYellow
                End If
            End If
        End If
    End Sub

    Private Sub dgvWS02GeneralSettings0_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWS02GeneralSettings0.CellClick, dgvWS02GeneralSettings1.CellClick,
                                                                                                                                             dgvWS02AnalogInput.CellClick, dgvWS02BACKLIGHT.CellClick, dgvWS02BACKLIGHT_1.CellClick,
                                                                                                                                             dgvWS02DigitalOutputPWL.CellClick, dgvWS02EMSTraceability.CellClick, dgvWS02SHAPE1.CellClick,
                                                                                                                                             dgvWS02DigitalOutputRTRVG.CellClick, dgvWS02DigitalOutputRTRVD.CellClick,
                                                                                                                                             dgvWS02TELLTALE.CellClick, dgvWS02WriteTraceability.CellClick,
                                                                                                                                             dgvWS02WriteConfiguration.CellClick, dgvWS02DigitalOutputStdSignal.CellClick,
                                                                                                                                             dgvWS02PWMOutput.CellClick, dgvWS02SHAPE2.CellClick
        Dim objectReference As Object

        If (sender Is dgvWS02GeneralSettings0) Then
            objectReference = _WS02GeneralSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS02GeneralSettings1) Then
            objectReference = _WS02GeneralSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS02AnalogInput) Then
            objectReference = _WS02ReadAnalogInput(e.RowIndex)
        ElseIf (sender Is dgvWS02BACKLIGHT) Then
            objectReference = _WS02Push_Backlight(e.RowIndex)
        ElseIf (sender Is dgvWS02BACKLIGHT_1) Then
            objectReference = _WS02Push_Backlight_1(e.RowIndex)
        ElseIf (sender Is dgvWS02EMSTraceability) Then
            objectReference = _WS02ReadEMSTraceability(e.RowIndex)
        ElseIf (sender Is dgvWS02SHAPE1) Then
            objectReference = _WS02Push_PictogramConformity(e.RowIndex)
        ElseIf (sender Is dgvWS02SHAPE2) Then
            objectReference = _WS02PictogramPoint_Interface(e.RowIndex)
        ElseIf (sender Is dgvWS02TELLTALE) Then
            objectReference = _WS02TELLTALE(e.RowIndex)
        ElseIf (sender Is dgvWS02WriteTraceability) Then
            objectReference = _WS02WriteTraceability(e.RowIndex)
        ElseIf (sender Is dgvWS02WriteConfiguration) Then
            objectReference = _WS02WriteConfiguration(e.RowIndex)
        ElseIf (sender Is dgvWS02DigitalOutputPWL) Then
            objectReference = _WS02DigitalOutput_PWL(e.RowIndex)
        ElseIf (sender Is dgvWS02DigitalOutputRTRVD) Then
            objectReference = _WS02DigitalOutput_RTRV_D(e.RowIndex)
        ElseIf (sender Is dgvWS02DigitalOutputRTRVG) Then
            objectReference = _WS02DigitalOutput_RTRV_G(e.RowIndex)
        ElseIf (sender Is dgvWS02DigitalOutputStdSignal) Then
            objectReference = _WS02DigitalOutput(e.RowIndex)
        ElseIf (sender Is dgvWS02PWMOutput) Then
            objectReference = _WS02PWMOutput(e.RowIndex)
        Else
            objectReference = Nothing
        End If
        If (objectReference IsNot Nothing AndAlso TypeOf (objectReference) Is cRecipeValue) Then
            EditValue(CType(objectReference, cRecipeValue), CType(sender, DataGridView))
        End If

    End Sub



    Private Sub WS03Table_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWS03GeneralSettings0.CellClick, dgvWS03GeneralSettings1.CellClick,
                                                                                                                                dgvWS03FrontLeft_0.CellClick, dgvWS03FrontLeft_1.CellClick,
                                                                                                                                dgvWS03FrontRight_0.CellClick, dgvWS03FrontRight_1.CellClick,
                                                                                                                                dgvWS03ChildrenLock_1.CellClick, dgvWS03ChildrenLock_0.CellClick,
                                                                                                                                dgvWS03JamaFunction.CellClick

        Dim objectReference As Object

        If (sender Is dgvWS03GeneralSettings0) Then
            objectReference = _WS03GeneralSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS03GeneralSettings1) Then
            objectReference = _WS03GeneralSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS03FrontLeft_0) Then
            objectReference = _WS03FrontLeftSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS03FrontLeft_1) Then
            objectReference = _WS03FrontLeftSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS03FrontRight_0) Then
            objectReference = _WS03FrontRightSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS03FrontRight_1) Then
            objectReference = _WS03FrontRightSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS03ChildrenLock_0) Then
            objectReference = _WS03ChildrenLockSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS03ChildrenLock_1) Then
            objectReference = _WS03ChildrenLockSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS03JamaFunction) Then
            objectReference = _WS03JamaFunction(e.RowIndex)
        Else
            objectReference = Nothing
        End If
        If (objectReference IsNot Nothing AndAlso TypeOf (objectReference) Is cRecipeValue) Then
            EditValue(CType(objectReference, cRecipeValue), CType(sender, DataGridView))
        End If
    End Sub

    Private Sub WS04Table_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWS04GeneralSettings0.CellClick, dgvWS04GeneralSettings1.CellClick,
                                                                                                                                dgvWS04RearLeft_0.CellClick, dgvWS04RearLeft_1.CellClick,
                                                                                                                                dgvWS04RearRight_0.CellClick, dgvWS04RearRight_1.CellClick,
                                                                                                                                dgvWS04MirrorFolding_0.CellClick, dgvWS04MirrorFolding_1.CellClick

        Dim objectReference As Object

        If (sender Is dgvWS04GeneralSettings0) Then
            objectReference = _WS04GeneralSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS04GeneralSettings1) Then
            objectReference = _WS04GeneralSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS04RearLeft_0) Then
            objectReference = _WS04RearLeftSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS04RearLeft_1) Then
            objectReference = _WS04RearLeftSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS04RearRight_0) Then
            objectReference = _WS04RearRightSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS04RearRight_1) Then
            objectReference = _WS04RearRightSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS04MirrorFolding_0) Then
            objectReference = _WS04MirrorFoldingSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS04MirrorFolding_1) Then
            objectReference = _WS04MirrorFoldingSettings1(e.RowIndex)
        Else
            objectReference = Nothing
        End If
        If (objectReference IsNot Nothing AndAlso TypeOf (objectReference) Is cRecipeValue) Then
            EditValue(CType(objectReference, cRecipeValue), CType(sender, DataGridView))
        End If
    End Sub

    Private Sub WS05Table_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWS05GeneralSettings0.CellClick, dgvWS05GeneralSettings1.CellClick,
                                                                                                                                dgvWS05Mirror_0.CellClick, dgvWS05Mirror_1.CellClick

        Dim objectReference As Object

        If (sender Is dgvWS05GeneralSettings0) Then
            objectReference = _WS05GeneralSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS05GeneralSettings1) Then
            objectReference = _WS05GeneralSettings1(e.RowIndex)
        ElseIf (sender Is dgvWS05Mirror_0) Then
            objectReference = _WS05MirrorSettings0(e.RowIndex)
        ElseIf (sender Is dgvWS05Mirror_1) Then
            objectReference = _WS05MirrorSettings1(e.RowIndex)
        Else
            objectReference = Nothing
        End If
        If (objectReference IsNot Nothing AndAlso TypeOf (objectReference) Is cRecipeValue) Then
            EditValue(CType(objectReference, cRecipeValue), CType(sender, DataGridView))
        End If
    End Sub

    Private Sub btnCopyFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyFrom.Click
        If (mUserManager.Login >= 1) Then
            frmChoiceCopyFrom.ShowDialog()
            If frmChoiceCopyFrom.MessageChoice = 0 Or (CopyWS02 = False And CopyWS03 = False And CopyWS04 = False And CopyWS05 = False) Then
                Exit Sub
            End If
            ' Ask the user to select the source recipe
            frmRecipeSelection.ShowDialog()
            ' If the source recipe name is not empty
            If (frmRecipeSelection.RecipeName <> "") Then
                ' If the source recipe name is different from the current one
                If (frmRecipeSelection.RecipeName <> _reference) Then
                    If CopyWS02 = True Then
                        ' Load the WS02 recipe
                        If (_WS02Recipe.Load(frmRecipeSelection.RecipeName)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while loading the WS02 recipe."
                            frmMessage.ShowDialog()
                        End If
                    End If
                    If CopyWS03 = True Then
                        ' Load the WS03 recipe
                        If (_WS03Recipe.Load(frmRecipeSelection.RecipeName)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while loading the WS03 recipe."
                            frmMessage.ShowDialog()
                        End If
                    End If
                    If CopyWS04 = True Then
                        ' Load the WS04 recipe
                        If (_WS04Recipe.Load(frmRecipeSelection.RecipeName)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while loading the WS04 recipe."
                            frmMessage.ShowDialog()
                        End If
                    End If
                    If CopyWS05 = True Then
                        ' Load the WS05 recipe
                        If (_WS05Recipe.Load(frmRecipeSelection.RecipeName)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while loading the WS05 recipe."
                            frmMessage.ShowDialog()
                        End If
                    End If
                    ' Set the flag of recipe modified
                    _recipeModified = True
                    ' Show the recipe
                    ShowRecipe()
                Else    ' Otherwise, if the source recipe name is the current one
                    ' Show an error message
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "The source recipe must be different from the current one."
                    frmMessage.ShowDialog()
                End If
            End If
        End If
    End Sub



    Private Sub tmrMonitor_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMonitor.Tick
        ' Update the status bar
        mStatusBar.Update(ssStatusBar)
    End Sub


    Private Sub lblRecipe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblRecipe.Click
        ' Manage a recipe modified
        ManageRecipeModified()
        ' If the current recipe was not modified
        If Not (_recipeModified) Then
            ' Show the recipe type selection form
            frmRecipeSelection.ShowDialog()
            ' Set the new reference
            _reference = frmRecipeSelection.RecipeName
            lblRecipe.Text = _reference
            ' Load the WS02 recipe
            If (_WS02Recipe.Load(_reference)) Then
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while loading the Station WS02 recipe."
                frmMessage.ShowDialog()
            End If
            ' Load the WS03 recipe
            If (_WS03Recipe.Load(_reference)) Then
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while loading the Station WS03 recipe."
                frmMessage.ShowDialog()
            End If
            ' Load the WS04 recipe
            If (_WS04Recipe.Load(_reference)) Then
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while loading the Station WS04 recipe."
                frmMessage.ShowDialog()
            End If
            ' Load the WS05 recipe
            If (_WS05Recipe.Load(_reference)) Then
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while loading the Station WS05 recipe."
                frmMessage.ShowDialog()
            End If
            ' Show the recipe
            ShowRecipe()
        End If

    End Sub

    Private Sub btnCopyTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyTo.Click
        Dim i, j As Integer

        If (mUserManager.Login >= 1) Then
            ' Ask the user to select the parameters and references
            frmCopyTo.ShowDialog()
            If frmCopyTo.MessageChoice = 1 Then
                frmMessage.MessageType = frmMessage.eType.Question
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Do you really want to copy parameters to references ?"
                frmMessage.ShowDialog()
                If frmMessage.MessageChoice = frmMessage.eChoice.Yes Then

                    If frmCopyTo.btnWS02.Checked = True Then
                        Dim _Recipe2 As New cWS02Recipe
                        'Load a second recipe configuration for each recipe where parameters will be copied to
                        _Recipe2.LoadConfiguration(mWS02Main.Settings.RecipeConfigurationPath)

                        'Load the reference parameters in progress
                        If (_WS02Recipe.Load(_reference)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while loading the recipe " & _reference
                            frmMessage.ShowDialog()
                        End If
                        For i = 0 To ReferenceChecked.GetUpperBound(0)
                            'Load parameters for each selected recipe
                            If (_Recipe2.Load(ReferenceChecked(i))) Then
                                frmMessage.MessageType = frmMessage.eType.Critical
                                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                                frmMessage.Message = "Error while loading the recipe " & ReferenceChecked(i)
                                frmMessage.ShowDialog()
                            End If
                            'Copy parameter 
                            For j = 10 To cWS02Recipe.ValueCount - 1
                                If _Recipe2.Value(j) IsNot Nothing Then
                                    If (_Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                                        If (_Recipe2.Value(j).Description = ParameterChecked(j)) And ((_Recipe2.Value(j).MinimumLimit <> _WS02Recipe.Value(j).MinimumLimit) Or (_Recipe2.Value(j).MaximumLimit <> _WS02Recipe.Value(j).MaximumLimit)) Then
                                            _Recipe2.Value(j).MinimumLimit = _WS02Recipe.Value(j).MinimumLimit
                                            _Recipe2.Value(j).MaximumLimit = _WS02Recipe.Value(j).MaximumLimit
                                        End If
                                    ElseIf (_Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                                        If (_Recipe2.Value(j).Description = ParameterChecked(j)) And (_Recipe2.Value(j).Value <> _WS02Recipe.Value(j).Value) Then
                                            _Recipe2.Value(j).Value = _WS02Recipe.Value(j).Value
                                        End If
                                    End If
                                End If
                            Next j
                            'Save recipe
                            _Recipe2.Save(ReferenceChecked(i))
                        Next

                    ElseIf frmCopyTo.btnWS03.Checked = True Then
                        Dim _Recipe2 As New cWS03Recipe

                        'Load a second recipe configuration for each recipe where parameters will be copied to
                        _Recipe2.LoadConfiguration(mWS03Main.Settings.RecipeConfigurationPath)

                        'Load the reference parameters in progress
                        If (_WS03Recipe.Load(_reference)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while loading the recipe " & _reference
                            frmMessage.ShowDialog()
                        End If
                        For i = 0 To ReferenceChecked.GetUpperBound(0)
                            'Load parameters for each selected recipe
                            If (_Recipe2.Load(ReferenceChecked(i))) Then
                                frmMessage.MessageType = frmMessage.eType.Critical
                                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                                frmMessage.Message = "Error while loading the recipe " & ReferenceChecked(i)
                                frmMessage.ShowDialog()
                            End If
                            'Copy parameter 
                            For j = 10 To cWS03Recipe.ValueCount - 1
                                If _Recipe2.Value(j) IsNot Nothing Then
                                    If (_Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                                        If (_Recipe2.Value(j).Description = ParameterChecked(j)) And ((_Recipe2.Value(j).MinimumLimit <> _WS03Recipe.Value(j).MinimumLimit) Or (_Recipe2.Value(j).MaximumLimit <> _WS03Recipe.Value(j).MaximumLimit)) Then
                                            _Recipe2.Value(j).MinimumLimit = _WS03Recipe.Value(j).MinimumLimit
                                            _Recipe2.Value(j).MaximumLimit = _WS03Recipe.Value(j).MaximumLimit
                                        End If
                                    ElseIf (_Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                                        If (_Recipe2.Value(j).Description = ParameterChecked(j)) And (_Recipe2.Value(j).Value <> _WS03Recipe.Value(j).Value) Then
                                            _Recipe2.Value(j).Value = _WS03Recipe.Value(j).Value
                                        End If
                                    End If
                                End If
                            Next j
                            'Save recipe
                            _Recipe2.Save(ReferenceChecked(i))
                        Next

                    ElseIf frmCopyTo.btnWS04.Checked = True Then
                        Dim _Recipe2 As New cWS04Recipe

                        'Load a second recipe configuration for each recipe where parameters will be copied to
                        _Recipe2.LoadConfiguration(mWS04Main.Settings.RecipeConfigurationPath)

                        'Load the reference parameters in progress
                        If (_WS04Recipe.Load(_reference)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while loading the recipe " & _reference
                            frmMessage.ShowDialog()
                        End If
                        For i = 0 To ReferenceChecked.GetUpperBound(0)
                            'Load parameters for each selected recipe
                            If (_Recipe2.Load(ReferenceChecked(i))) Then
                                frmMessage.MessageType = frmMessage.eType.Critical
                                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                                frmMessage.Message = "Error while loading the recipe " & ReferenceChecked(i)
                                frmMessage.ShowDialog()
                            End If
                            'Copy parameter 
                            For j = 10 To cWS04Recipe.ValueCount - 1
                                If _Recipe2.Value(j) IsNot Nothing Then
                                    If (_Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                                        If (_Recipe2.Value(j).Description = ParameterChecked(j)) And ((_Recipe2.Value(j).MinimumLimit <> _WS04Recipe.Value(j).MinimumLimit) Or (_Recipe2.Value(j).MaximumLimit <> _WS04Recipe.Value(j).MaximumLimit)) Then
                                            _Recipe2.Value(j).MinimumLimit = _WS04Recipe.Value(j).MinimumLimit
                                            _Recipe2.Value(j).MaximumLimit = _WS04Recipe.Value(j).MaximumLimit
                                        End If
                                    ElseIf (_Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                                        If (_Recipe2.Value(j).Description = ParameterChecked(j)) And (_Recipe2.Value(j).Value <> _WS04Recipe.Value(j).Value) Then
                                            _Recipe2.Value(j).Value = _WS04Recipe.Value(j).Value
                                        End If
                                    End If
                                End If
                            Next j
                            'Save recipe
                            _Recipe2.Save(ReferenceChecked(i))
                        Next
                    ElseIf frmCopyTo.btnWS05.Checked = True Then
                        Dim _Recipe2 As New cWS05Recipe

                        'Load a second recipe configuration for each recipe where parameters will be copied to
                        _Recipe2.LoadConfiguration(mWS05Main.Settings.RecipeConfigurationPath)

                        'Load the reference parameters in progress
                        If (_WS05Recipe.Load(_reference)) Then
                            frmMessage.MessageType = frmMessage.eType.Critical
                            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                            frmMessage.Message = "Error while loading the recipe " & _reference
                            frmMessage.ShowDialog()
                        End If
                        For i = 0 To ReferenceChecked.GetUpperBound(0)
                            'Load parameters for each selected recipe
                            If (_Recipe2.Load(ReferenceChecked(i))) Then
                                frmMessage.MessageType = frmMessage.eType.Critical
                                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                                frmMessage.Message = "Error while loading the recipe " & ReferenceChecked(i)
                                frmMessage.ShowDialog()
                            End If
                            'Copy parameter 
                            For j = 10 To cWS05Recipe.ValueCount - 1
                                If _Recipe2.Value(j) IsNot Nothing Then
                                    If (_Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or
                                        _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                                        If (_Recipe2.Value(j).Description = ParameterChecked(j)) And ((_Recipe2.Value(j).MinimumLimit <> _WS05Recipe.Value(j).MinimumLimit) Or (_Recipe2.Value(j).MaximumLimit <> _WS05Recipe.Value(j).MaximumLimit)) Then
                                            _Recipe2.Value(j).MinimumLimit = _WS05Recipe.Value(j).MinimumLimit
                                            _Recipe2.Value(j).MaximumLimit = _WS05Recipe.Value(j).MaximumLimit
                                        End If
                                    ElseIf (_Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or
                                            _Recipe2.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                                        If (_Recipe2.Value(j).Description = ParameterChecked(j)) And (_Recipe2.Value(j).Value <> _WS05Recipe.Value(j).Value) Then
                                            _Recipe2.Value(j).Value = _WS05Recipe.Value(j).Value
                                        End If
                                    End If
                                End If
                            Next j
                            'Save recipe
                            _Recipe2.Save(ReferenceChecked(i))
                        Next
                    End If
                End If
            End If
        Else
            ' Show an error message
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "User not allowed to modify recipes."
            frmMessage.ShowDialog()
            mUserManager.Logout()
        End If


    End Sub

    Private Sub btnCheckMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckMaster.Click
        If mUserManager.Login >= 1 Then
            frmCheckMaster.ShowDialog()
            If frmCheckMaster.MessageChoice = 1 Then
                ' Load the WS020 recipe
                If (_WS02Recipe.Load(_reference)) Then
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while loading the station 02 recipe."
                    frmMessage.ShowDialog()
                End If
                ' Load the WS03 recipe
                If (_WS03Recipe.Load(_reference)) Then
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while loading the station 03 recipe."
                    frmMessage.ShowDialog()
                End If
                ' Load the WS04 recipe
                If (_WS04Recipe.Load(_reference)) Then
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while loading the station 04 recipe."
                    frmMessage.ShowDialog()
                End If
                ' Load the WS05 recipe
                If (_WS05Recipe.Load(_reference)) Then
                    frmMessage.MessageType = frmMessage.eType.Critical
                    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                    frmMessage.Message = "Error while loading the station 05 recipe."
                    frmMessage.ShowDialog()
                End If
                ShowRecipe()
            End If
        Else
            ' Show an error message
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "User not allowed to modify recipes."
            frmMessage.ShowDialog()
            mUserManager.Logout()
        End If
    End Sub

    Private Sub btnSaveMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveMaster.Click
        MasterReference = True
        btnSaveModifies.PerformClick()

    End Sub



    Private Sub pbLogoValeo_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbLogoValeo.Click
        ' Manage a recipe modified
        ManageRecipeModified()
        ' If the current recipe was not modified
        If Not (_recipeModified) Then
            mUserManager.Logout()
            ' Close the form
            Me.Close()
        End If
    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        _WS02Recipe.Push_Select_Right_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Push_Select_Right_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Push_Folding_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Push_Folding_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Push_Folding_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Push_Folding_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Push_Folding_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Push_Folding_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Push_Folding_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Push_Folding_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Push_Folding_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Push_Folding_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Push_Folding_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Push_Folding_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Push_Adjust_UP_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Push_Adjust_UP_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Push_Adjust_DOWN_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Push_Adjust_DOWN_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Push_Adjust_Left_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Push_Adjust_Left_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Push_Adjust_Right_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Push_Adjust_Right_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Left_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Front_Right_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Left_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Windows_Lifter_Rear_Right_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        _WS02Recipe.Push_Children_Lock_Polygon_Axy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Axy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Bxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Bxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Cxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Cxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Dxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Dxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Exy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Exy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Fxy.MaximumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
        _WS02Recipe.Push_Children_Lock_Polygon_Fxy.MinimumLimit = _WS02Recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit

        ShowRecipe()
    End Sub


End Class
