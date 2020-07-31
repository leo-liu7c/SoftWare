Option Explicit On
Option Strict On

Public Class frmTricks

    Private Sub btnWS02ForcePartOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02ForcePartOk.Click
        mWS02Main.ForcePartOk = Not (mWS02Main.ForcePartOk)
    End Sub



    Private Sub btnWS02ForceParttypeOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02ForcePartTypeOk.Click
        mWS02Main.ForcePartTypeOk = Not (mWS02Main.ForcePartTypeOk)
    End Sub



    Private Sub btnWS03ForcePartOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03ForcePartOk.Click
        mWS03Main.ForcePartOk = Not (mWS03Main.ForcePartOk)
    End Sub



    Private Sub btnWS03ForceParttypeOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03ForcePartTypeOk.Click
        mWS03Main.ForcePartTypeOk = Not (mWS03Main.ForcePartTypeOk)
    End Sub

    Private Sub btnWS04ForcePartOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS04ForcePartOk.Click
        mWS04Main.ForcePartOk = Not (mWS04Main.ForcePartOk)
    End Sub



    Private Sub btnWS04ForceParttypeOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS04ForcePartTypeOk.Click
        mWS04Main.ForcePartTypeOk = Not (mWS04Main.ForcePartTypeOk)
    End Sub

    Private Sub btnWS05ForcePartOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS05ForcePartOk.Click
        mWS05Main.ForcePartOk = Not (mWS05Main.ForcePartOk)
    End Sub



    Private Sub btnWS05ForceParttypeOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS05ForcePartTypeOk.Click
        mWS05Main.ForcePartTypeOk = Not (mWS05Main.ForcePartTypeOk)
    End Sub


    Private Sub frmTricks_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Disable the timer
        tmrMonitor.Enabled = False
    End Sub



    Private Sub frmTricks_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Configurate and enable the timer
        tmrMonitor.Interval = 250
        tmrMonitor.Enabled = True
    End Sub



    Private Sub tmrMonitor_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMonitor.Tick
        ' Color the Station 060 force part ok button
        If (mWS02Main.ForcePartOk) Then
            btnWS02ForcePartOk.BackColor = Color.Red
        Else
            btnWS02ForcePartOk.BackColor = SystemColors.ButtonFace
            btnWS02ForcePartOk.UseVisualStyleBackColor = True
        End If
        ' Color the Station 060 force part type ok button
        If (mWS02Main.ForcePartTypeOk) Then
            btnWS02ForcePartTypeOk.BackColor = Color.Red
        Else
            btnWS02ForcePartTypeOk.BackColor = SystemColors.ButtonFace
            btnWS02ForcePartTypeOk.UseVisualStyleBackColor = True
        End If
        ' Color the Station 080 force part ok button
        If (mWS03Main.ForcePartOk) Then
            btnWS03ForcePartOk.BackColor = Color.Red
        Else
            btnWS03ForcePartOk.BackColor = SystemColors.ButtonFace
            btnWS03ForcePartOk.UseVisualStyleBackColor = True
        End If
        ' Color the Station 080 force part type ok button
        If (mWS03Main.ForcePartTypeOk) Then
            btnWS03ForcePartTypeOk.BackColor = Color.Red
        Else
            btnWS03ForcePartTypeOk.BackColor = SystemColors.ButtonFace
            btnWS03ForcePartTypeOk.UseVisualStyleBackColor = True
        End If
        ' Color the Station 070 force part ok button
        If (mWS04Main.ForcePartOk) Then
            btnWS04ForcePartOk.BackColor = Color.Red
        Else
            btnWS04ForcePartOk.BackColor = SystemColors.ButtonFace
            btnWS04ForcePartOk.UseVisualStyleBackColor = True
        End If
        ' Color the Station 070 force part type ok button
        If (mWS04Main.ForcePartTypeOk) Then
            btnWS04ForcePartTypeOk.BackColor = Color.Red
        Else
            btnWS04ForcePartTypeOk.BackColor = SystemColors.ButtonFace
            btnWS04ForcePartTypeOk.UseVisualStyleBackColor = True
        End If
        ' Color the Station 070 force part ok button
        If (mWS05Main.ForcePartOk) Then
            btnWS05ForcePartOk.BackColor = Color.Red
        Else
            btnWS05ForcePartOk.BackColor = SystemColors.ButtonFace
            btnWS05ForcePartOk.UseVisualStyleBackColor = True
        End If
        ' Color the Station 070 force part type ok button
        If (mWS05Main.ForcePartTypeOk) Then
            btnWS05ForcePartTypeOk.BackColor = Color.Red
        Else
            btnWS05ForcePartTypeOk.BackColor = SystemColors.ButtonFace
            btnWS05ForcePartTypeOk.UseVisualStyleBackColor = True
        End If

    End Sub

    'Private Sub btnWS013ForcePartOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS04ForcePartOk.Click
    '    mWS04Main.ForcePartOk = Not (mWS04Main.ForcePartOk)
    'End Sub

    'Private Sub btnWS013ForcePartTypeOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS04ForcePartTypeOk.Click
    '    mWS04Main.ForcePartTypeOk = Not (mWS04Main.ForcePartTypeOk)
    'End Sub
End Class