Option Explicit On
'Option Strict On

Public Class frmCheckMaster

    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+


    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+

    Private mcMessageChoice As Integer

    Private _WS02Recipe_FHM As New cWS02Recipe
    Private _WS02RecipeMaster_FHM As New cWS02Recipe

    Private _WS03Recipe_FHM As New cWS03Recipe
    Private _WS03RecipeMaster_FHM As New cWS03Recipe


    Private _WS02Ref_FHM(cWS02Recipe.ValueCount - 1) As Object
    Private _WS02RefMaster_FHM(cWS02Recipe.ValueCount - 1) As Object

    Private _WS03Ref_FHM(cWS03Recipe.ValueCount - 1) As Object
    Private _WS03RefMaster_FHM(cWS03Recipe.ValueCount - 1) As Object

    Private _settingsWS02 As mWS02Main.sSettings
    Private _settingsWS03 As mWS03Main.sSettings


    ReadOnly Property MessageChoice() As Integer
        Get
            ' Returns the user's choice
            MessageChoice = mcMessageChoice
        End Get
    End Property

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

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

        frmMessage.MessageType = frmMessage.eType.Question
        frmMessage.MessageButtons = frmMessage.eButtons.YesNo
        frmMessage.Message = "Do you really want to copy parameters from master reference to recipe ?"
        frmMessage.ShowDialog()
        If frmMessage.MessageChoice = frmMessage.eChoice.Yes Then
            'WS02
            'Load the reference parameters in progress
            If (_WS02Recipe_FHM.Load(frmRecipes.lblRecipe.Text)) Then
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while loading the recipe " & frmRecipes.lblRecipe.Text
                frmMessage.ShowDialog()
            End If
            MasterReference = True
            'Load parameters of master reference
            If (_WS02RecipeMaster_FHM.Load(frmRecipes.lblRecipe.Text)) Then
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while loading the recipe M" & frmRecipes.lblRecipe.Text
                frmMessage.ShowDialog()
            End If
            MasterReference = False
            'Copy parameter 
            For j = 10 To cWS02Recipe.ValueCount - 1
                If _WS02RecipeMaster_FHM.Value(j) IsNot Nothing Then
                    If (_WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or _
                        _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or _
                        _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or _
                        _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                        If ((_WS02RecipeMaster_FHM.Value(j).MinimumLimit <> _WS02Recipe_FHM.Value(j).MinimumLimit) Or (_WS02RecipeMaster_FHM.Value(j).MaximumLimit <> _WS02Recipe_FHM.Value(j).MaximumLimit)) Then
                            _WS02Recipe_FHM.Value(j).MinimumLimit = _WS02RecipeMaster_FHM.Value(j).MinimumLimit
                            _WS02Recipe_FHM.Value(j).MaximumLimit = _WS02RecipeMaster_FHM.Value(j).MaximumLimit
                        End If
                    ElseIf (_WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or _
                            _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or _
                            _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or _
                            _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or _
                            _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or _
                            _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                        If (_WS02RecipeMaster_FHM.Value(j).Value <> _WS02Recipe_FHM.Value(j).Value) Then
                            _WS02Recipe_FHM.Value(j).Value = _WS02RecipeMaster_FHM.Value(j).Value
                        End If
                    End If
                End If
            Next j
            'Save recipe
            _WS02Recipe_FHM.Save(frmRecipes.lblRecipe.Text)

            'WS03
            'Load the reference parameters in progress
            If (_WS03Recipe_FHM.Load(frmRecipes.lblRecipe.Text)) Then
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while loading the recipe " & frmRecipes.lblRecipe.Text
                frmMessage.ShowDialog()
            End If
            MasterReference = True
            'Load parameters of master reference
            If (_WS03RecipeMaster_FHM.Load(frmRecipes.lblRecipe.Text)) Then
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                frmMessage.Message = "Error while loading the recipe M" & frmRecipes.lblRecipe.Text
                frmMessage.ShowDialog()
            End If
            MasterReference = False
            'Copy parameter 
            For j = 10 To cWS03Recipe.ValueCount - 1
                If _WS03RecipeMaster_FHM.Value(j) IsNot Nothing Then
                    If (_WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or _
                        _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or _
                        _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or _
                        _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                        If ((_WS03RecipeMaster_FHM.Value(j).MinimumLimit <> _WS03Recipe_FHM.Value(j).MinimumLimit) Or (_WS03RecipeMaster_FHM.Value(j).MaximumLimit <> _WS03Recipe_FHM.Value(j).MaximumLimit)) Then
                            _WS03Recipe_FHM.Value(j).MinimumLimit = _WS03RecipeMaster_FHM.Value(j).MinimumLimit
                            _WS03Recipe_FHM.Value(j).MaximumLimit = _WS03RecipeMaster_FHM.Value(j).MaximumLimit
                        End If
                    ElseIf (_WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or _
                            _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or _
                            _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or _
                            _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or _
                            _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or _
                            _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                        If (_WS03RecipeMaster_FHM.Value(j).Value <> _WS03Recipe_FHM.Value(j).Value) Then
                            _WS03Recipe_FHM.Value(j).Value = _WS03RecipeMaster_FHM.Value(j).Value
                        End If
                    End If
                End If
            Next j
            'Save recipe
            _WS03Recipe_FHM.Save(frmRecipes.lblRecipe.Text)


            mcMessageChoice = 1

            MasterReference = False
            Me.Close()
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        mcMessageChoice = 2
        Me.Close()
    End Sub


    Private Sub frmCheckMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer

        For i = 0 To cWS02Recipe.ValueCount - 1
            _WS02Ref_FHM(i) = ""
            _WS02RefMaster_FHM(i) = ""
        Next i
        For i = 0 To cWS03Recipe.ValueCount - 1
            _WS03Ref_FHM(i) = ""
            _WS03RefMaster_FHM(i) = ""
        Next i


        For i = 0 To dgvRecipeWS02.RowCount - 1
            dgvRecipeWS02.Item(0, i).Value = ""
            dgvRecipeWS02.Item(1, i).Value = ""
            dgvRecipeWS02.Item(2, i).Value = ""
            dgvMasterWS02.Item(0, i).Value = ""
            dgvMasterWS02.Item(1, i).Value = ""
            dgvMasterWS02.Item(2, i).Value = ""
        Next
        dgvRecipeWS02.RowCount = 0
        dgvRecipeWS02.Height = 30
        dgvMasterWS02.RowCount = 0
        dgvMasterWS02.Height = 30

        For i = 0 To dgvRecipeWS03.RowCount - 1
            dgvRecipeWS03.Item(0, i).Value = ""
            dgvRecipeWS03.Item(1, i).Value = ""
            dgvRecipeWS03.Item(2, i).Value = ""
            dgvMasterWS03.Item(0, i).Value = ""
            dgvMasterWS03.Item(1, i).Value = ""
            dgvMasterWS03.Item(2, i).Value = ""
        Next
        dgvRecipeWS03.RowCount = 0
        dgvRecipeWS03.Height = 30
        dgvMasterWS03.RowCount = 0
        dgvMasterWS03.Height = 30

        'Compare recipe and master reference WS02
        CheckMasterWS02()
        'Compare recipe and master reference WS03
        CheckMasterWS03()

        If dgvMasterWS02.RowCount = 0 And dgvMasterWS03.RowCount = 0 Then
            Me.Close()
            frmMessage.MessageType = frmMessage.eType.Information
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Recipe is the same as master reference"
            frmMessage.ShowDialog()
        End If

    End Sub

    Private Sub CheckMasterWS02()
        Dim name As String
        Dim i As Integer
        Dim j As Integer

        i = 0

        ' Build the file path
        name = CStr(frmRecipes.lblRecipe.Text)
        'Load a recipe configuration 
        _WS02Recipe_FHM.LoadConfiguration(mWS02Main.Settings.RecipeConfigurationPath)
        'Load a second recipe configuration 
        _WS02RecipeMaster_FHM.LoadConfiguration(mWS02Main.Settings.RecipeConfigurationPath)

        'Load the reference parameters in progress
        If (_WS02Recipe_FHM.Load(name)) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while loading the recipe " & frmRecipes.lblRecipe.Text
            frmMessage.ShowDialog()
        End If
        MasterReference = True
        'Load parameters for each selected recipe
        If (_WS02RecipeMaster_FHM.Load(name)) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while loading the recipe M" & frmRecipes.lblRecipe.Text
            frmMessage.ShowDialog()
        End If

        'Check parameter 
        For j = 10 To cWS02Recipe.ValueCount - 1
            If _WS02RecipeMaster_FHM.Value(j) IsNot Nothing Then
                If (_WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or _
                    _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or _
                    _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or _
                    _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                    If ((_WS02RecipeMaster_FHM.Value(j).MinimumLimit <> _WS02Recipe_FHM.Value(j).MinimumLimit) Or (_WS02RecipeMaster_FHM.Value(j).MaximumLimit <> _WS02Recipe_FHM.Value(j).MaximumLimit)) Then
                        _WS02Ref_FHM(i) = _WS02Recipe_FHM.Value(j)
                        _WS02RefMaster_FHM(i) = _WS02RecipeMaster_FHM.Value(j)
                        i += 1
                    End If
                ElseIf (_WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or _
                        _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or _
                        _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or _
                        _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or _
                        _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or _
                        _WS02RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                    If (_WS02RecipeMaster_FHM.Value(j).Value <> _WS02Recipe_FHM.Value(j).Value) Then
                        _WS02Ref_FHM(i) = _WS02Recipe_FHM.Value(j)
                        _WS02RefMaster_FHM(i) = _WS02RecipeMaster_FHM.Value(j)
                        i += 1
                    End If
                End If
            End If
        Next j
        InitRecipeTable(dgvRecipeWS02, i, _WS02Ref_FHM)
        InitRecipeTable(dgvMasterWS02, i, _WS02RefMaster_FHM)
        MasterReference = False


    End Sub

    Private Sub CheckMasterWS03()
        Dim name As String
        Dim i As Integer
        Dim j As Integer

        i = 0

        ' Build the file path
        name = CStr(frmRecipes.lblRecipe.Text)
        'Load a recipe configuration 
        _WS03Recipe_FHM.LoadConfiguration(mWS03Main.Settings.RecipeConfigurationPath)
        'Load a second recipe configuration 
        _WS03RecipeMaster_FHM.LoadConfiguration(mWS03Main.Settings.RecipeConfigurationPath)

        'Load the reference parameters in progress
        If (_WS03Recipe_FHM.Load(name)) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while loading the recipe " & frmRecipes.lblRecipe.Text
            frmMessage.ShowDialog()
        End If
        MasterReference = True
        'Load parameters for each selected recipe
        If (_WS03RecipeMaster_FHM.Load(name)) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while loading the recipe M" & frmRecipes.lblRecipe.Text
            frmMessage.ShowDialog()
        End If

        'Check parameter 
        For j = 10 To cWS03Recipe.ValueCount - 1
            If _WS03RecipeMaster_FHM.Value(j) IsNot Nothing Then
                If (_WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or _
                    _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or _
                    _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or _
                    _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                    If ((_WS03RecipeMaster_FHM.Value(j).MinimumLimit <> _WS03Recipe_FHM.Value(j).MinimumLimit) Or (_WS03RecipeMaster_FHM.Value(j).MaximumLimit <> _WS03Recipe_FHM.Value(j).MaximumLimit)) Then
                        _WS03Ref_FHM(i) = _WS03Recipe_FHM.Value(j)
                        _WS03RefMaster_FHM(i) = _WS03RecipeMaster_FHM.Value(j)
                        i += 1
                    End If
                ElseIf (_WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or _
                        _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or _
                        _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or _
                        _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or _
                        _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or _
                        _WS03RecipeMaster_FHM.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                    If (_WS03RecipeMaster_FHM.Value(j).Value <> _WS03Recipe_FHM.Value(j).Value) Then
                        _WS03Ref_FHM(i) = _WS03Recipe_FHM.Value(j)
                        _WS03RefMaster_FHM(i) = _WS03RecipeMaster_FHM.Value(j)
                        i += 1
                    End If
                End If
            End If
        Next j
        InitRecipeTable(dgvRecipeWS03, i, _WS03Ref_FHM)
        InitRecipeTable(dgvMasterWS03, i, _WS03RefMaster_FHM)
        MasterReference = False

    End Sub

    Private Sub InitRecipeTable(ByRef table As DataGridView, _
                                ByVal rowCount As Integer, _
                                ByRef objectReference() As Object)
        If rowCount > 0 Then
            ' Reference to the table
            With table
                ' Disable the refresh
                .SuspendLayout()
                .Height = 470
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
                    .Item(0, i).Value = objectReference(i).Description
                    .Item(2, i).Value = objectReference(i).Units

                    If (objectReference(i).ValueType = cRecipeValue.eValueType.BCDRange Or _
                        objectReference(i).ValueType = cRecipeValue.eValueType.HexRange Or _
                        objectReference(i).ValueType = cRecipeValue.eValueType.IntegerRange Or _
                        objectReference(i).ValueType = cRecipeValue.eValueType.SingleRange) Then
                        .Item(1, i).Value = objectReference(i).StringMinimumLimit & " / " & objectReference(i).StringMaximumLimit
                    ElseIf (objectReference(i).ValueType = cRecipeValue.eValueType.BCDValue Or _
                            objectReference(i).ValueType = cRecipeValue.eValueType.HexValue Or _
                            objectReference(i).ValueType = cRecipeValue.eValueType.IntegerValue Or _
                            objectReference(i).ValueType = cRecipeValue.eValueType.SingleValue Or _
                            objectReference(i).ValueType = cRecipeValue.eValueType.StringValue) Then
                        .Item(1, i).Value = objectReference(i).StringValue
                    ElseIf objectReference(i).ValueType = cRecipeValue.eValueType.BooleanValue Then
                        .Item(1, i).Value = objectReference(i).value
                    End If
                Next

                ' Enable the refresh
                .ResumeLayout(True)
            End With
        End If
    End Sub


End Class