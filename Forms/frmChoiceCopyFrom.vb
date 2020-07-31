Option Explicit On
Option Strict On

Public Class frmChoiceCopyFrom



    Private mcMessageChoice As Integer


    ReadOnly Property MessageChoice() As Integer
        Get
            ' Returns the user's choice
            MessageChoice = mcMessageChoice
        End Get
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Stores the user's choice
        mcMessageChoice = 0
        ' Unloads the form
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ' Stores the user's choice
        mcMessageChoice = 1
        ' Unloads the form
        Me.Close()

    End Sub


    Private Sub btnWS02_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02.Click
        CopyWS02 = Not (CopyWS02)
        If CopyWS02 = True Then
            CopyWS03 = False
            CopyWS04 = False
            CopyWS05 = False
            btnWS02.BackColor = Color.LimeGreen
            btnWS03.BackColor = Color.Red
            btnWS04.BackColor = Color.Red
            btnWS05.BackColor = Color.Red
            btnAllWS.BackColor = Color.Red
        Else
            btnWS02.BackColor = Color.Red
            btnAllWS.BackColor = Color.Red
        End If

    End Sub

    Private Sub btnWS03_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03.Click
        CopyWS03 = Not (CopyWS03)
        If CopyWS03 = True Then
            CopyWS02 = False
            CopyWS04 = False
            CopyWS05 = False

            btnWS03.BackColor = Color.LimeGreen
            btnWS02.BackColor = Color.Red
            btnWS05.BackColor = Color.Red
            btnWS04.BackColor = Color.Red
            btnAllWS.BackColor = Color.Red
        Else
            btnWS03.BackColor = Color.Red
            btnAllWS.BackColor = Color.Red
        End If

    End Sub

    Private Sub btnAllWS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllWS.Click
        CopyWS02 = True
        CopyWS03 = True
        CopyWS04 = True
        CopyWS05 = True

        btnWS05.BackColor = Color.LimeGreen
        btnWS04.BackColor = Color.LimeGreen
        btnWS03.BackColor = Color.LimeGreen
        btnWS02.BackColor = Color.LimeGreen
        btnAllWS.BackColor = Color.LimeGreen

    End Sub


    Private Sub frmChoiceCopyFrom_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CopyWS02 = False
        CopyWS03 = False
        CopyWS04 = False
        CopyWS05 = False

        btnWS05.BackColor = Color.Red
        btnWS04.BackColor = Color.Red
        btnWS03.BackColor = Color.Red
        btnWS02.BackColor = Color.Red
        btnAllWS.BackColor = Color.Red

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS04.Click
        CopyWS04 = Not (CopyWS04)
        If CopyWS04 = True Then
            CopyWS02 = False
            CopyWS03 = False
            CopyWS05 = False

            btnWS04.BackColor = Color.LimeGreen
            btnWS05.BackColor = Color.Red
            btnWS02.BackColor = Color.Red
            btnWS03.BackColor = Color.Red
            btnAllWS.BackColor = Color.Red
        Else
            btnWS04.BackColor = Color.Red
            btnAllWS.BackColor = Color.Red
        End If

    End Sub

    Private Sub btnWS05_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS05.Click
        CopyWS05 = Not (CopyWS05)
        If CopyWS05 = True Then
            CopyWS02 = False
            CopyWS03 = False
            CopyWS04 = False

            btnWS05.BackColor = Color.LimeGreen
            btnWS04.BackColor = Color.Red
            btnWS02.BackColor = Color.Red
            btnWS03.BackColor = Color.Red
            btnAllWS.BackColor = Color.Red
        Else
            btnWS05.BackColor = Color.Red
            btnAllWS.BackColor = Color.Red
        End If
    End Sub
End Class