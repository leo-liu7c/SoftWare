Option Explicit On
Option Strict On

Public Class frmRecipeSelection
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private _recipeName As String



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property RecipeName As String
        Get
            RecipeName = _recipeName
        End Get
    End Property



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Return an empty recipe name
        _recipeName = ""
        ' Close the form
        Me.Close()
    End Sub



    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ' Return an empty recipe name
        _recipeName = CStr(lbRecipeName.SelectedItem)
        ' Close the form
        Me.Close()
    End Sub



    Private Sub frmRecipeSelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim names() As String = {}

        ' Load the recipe list
        cWS03Recipe.ReadList(names)
        ' Add the names to the list
        lbRecipeName.Items.Clear()
        For Each n In names
            lbRecipeName.Items.Add(n)
        Next
        If lbRecipeName.SelectedIndex >= 0 Then
            lbRecipeName.SelectedIndex = 0
        End If
        ' Clear the recipe name
        _recipeName = ""
    End Sub
End Class