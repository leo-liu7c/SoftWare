Option Explicit On
'Option Strict On

Public Class frmCopyTo


    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+


    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+

    Private parameter(500) As String
    Private value(500) As String
    Private mcMessageChoice As Integer


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
        Dim i, j As Integer

        For i = 0 To lbSelectedParameters.Items.Count - 1
            For j = 10 To 500
                If lbSelectedParameters.Items.Item(i) = parameter(j) Then
                    ParameterChecked(j) = parameter(j)
                End If
            Next j
        Next i

        ReDim ReferenceChecked(lbSelectedReferences.Items.Count - 1)
        For i = 0 To lbSelectedReferences.Items.Count - 1
            ReferenceChecked(i) = CStr(lbSelectedReferences.Items.Item(i))
        Next i

        mcMessageChoice = 1
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        mcMessageChoice = 2
        Me.Close()
    End Sub


    Private Sub frmCopyTo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer

        'reset all variables
        For i = 0 To 500
            parameter(i) = ""
            value(i) = ""
        Next i

        Array.Clear(ParameterChecked, 0, ParameterChecked.Length)
        Array.Clear(ReferenceChecked, 0, ReferenceChecked.Length)

        lbParameters.Items.Clear()
        lbReferences.Items.Clear()
        lbSelectedParameters.Items.Clear()
        lbSelectedReferences.Items.Clear()

        cbAllParameter.CheckState = CheckState.Unchecked
        cbAllReference.CheckState = CheckState.Unchecked

        btnWS02.Checked = False
        btnWS03.Checked = False
        btnWS04.Checked = False
        btnWS05.Checked = False


    End Sub



    Private Sub cbAllParameter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAllParameter.CheckedChanged
        Dim i As Integer

        If cbAllParameter.CheckState = 1 Then
            For i = 0 To lbParameters.Items.Count - 1
                lbSelectedParameters.Items.Add(lbParameters.Items.Item(i))
            Next
            lbParameters.Items.Clear()
        ElseIf cbAllParameter.CheckState = 0 Then
            For i = 0 To lbSelectedParameters.Items.Count - 1
                lbParameters.Items.Add(lbSelectedParameters.Items.Item(i))
            Next
            lbSelectedParameters.Items.Clear()
        End If
    End Sub

    Private Sub cbAllReference_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAllReference.CheckedChanged
        Dim i As Integer

        If cbAllReference.CheckState = 1 Then
            For i = 0 To lbReferences.Items.Count - 1
                lbSelectedReferences.Items.Add(lbReferences.Items.Item(i))
            Next
            lbReferences.Items.Clear()
        ElseIf cbAllReference.CheckState = 0 Then
            For i = 0 To lbSelectedReferences.Items.Count - 1
                lbReferences.Items.Add(lbSelectedReferences.Items.Item(i))
            Next
            lbSelectedReferences.Items.Clear()
        End If

    End Sub

    Private Sub btnWS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02.CheckedChanged, _
                                                                                                            btnWS03.CheckedChanged, _
                                                                                                            btnWS04.CheckedChanged, _
                                                                                                            btnWS05.CheckedChanged
        Dim i As Integer
        Dim FilePath As String = ""
        Dim recipeNames(0) As String

        btnWS02.ForeColor = Color.Red
        btnWS03.ForeColor = Color.Red
        btnWS04.ForeColor = Color.Red
        btnWS05.ForeColor = Color.Red

        lbParameters.Items.Clear()
        lbReferences.Items.Clear()
        lbSelectedParameters.Items.Clear()
        lbSelectedReferences.Items.Clear()

        cbAllParameter.CheckState = 0
        cbAllReference.CheckState = 0

        If btnWS02.Checked Then
            btnWS02.ForeColor = Color.Green
            FilePath = mConstants.BasePath & mWS02Main.Settings.RecipePath & "\" & frmRecipes.lblRecipe.Text & ".txt"
        ElseIf btnWS03.Checked Then
            btnWS03.ForeColor = Color.Green
            FilePath = mConstants.BasePath & mWS03Main.Settings.RecipePath & "\" & frmRecipes.lblRecipe.Text & ".txt"
        ElseIf btnWS04.Checked Then
            btnWS04.ForeColor = Color.Green
            FilePath = mConstants.BasePath & mWS04Main.Settings.RecipePath & "\" & frmRecipes.lblRecipe.Text & ".txt"
        ElseIf btnWS05.Checked Then
            btnWS05.ForeColor = Color.Green
            FilePath = mConstants.BasePath & mWS05Main.Settings.RecipePath & "\" & frmRecipes.lblRecipe.Text & ".txt"
        End If

        mRecipe.LoadParameters(FilePath, parameter, value)

        For i = 10 To 500
            If parameter(i) <> "" Then
                lbParameters.Items.Add(parameter(i))
            End If
        Next i

        cWS02Recipe.ReadList(recipeNames)

        For Each recipeName In recipeNames
            lbReferences.Items.Add(recipeName)
        Next

    End Sub


    Private Sub lbParameters_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbParameters.Click
        If lbParameters.SelectedItem IsNot Nothing Then
            lbSelectedParameters.Items.Add(lbParameters.SelectedItem)
            lbParameters.Items.RemoveAt(lbParameters.SelectedIndex)
        End If
    End Sub

    Private Sub lbSelectedParameters_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSelectedParameters.Click
        If lbSelectedParameters.SelectedItem IsNot Nothing Then
            lbParameters.Items.Add(lbSelectedParameters.SelectedItem)
            lbSelectedParameters.Items.RemoveAt(lbSelectedParameters.SelectedIndex)
        End If
    End Sub

    Private Sub lbReferences_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbReferences.Click
        If lbReferences.SelectedItem IsNot Nothing Then
            lbSelectedReferences.Items.Add(lbReferences.SelectedItem)
            lbReferences.Items.RemoveAt(lbReferences.SelectedIndex)
        End If
    End Sub

    Private Sub lbSelectedReferences_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSelectedReferences.Click
        If lbSelectedReferences.SelectedItem IsNot Nothing Then
            lbReferences.Items.Add(lbSelectedReferences.SelectedItem)
            lbSelectedReferences.Items.RemoveAt(lbSelectedReferences.SelectedIndex)
        End If
    End Sub


End Class