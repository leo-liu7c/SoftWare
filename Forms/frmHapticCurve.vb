Option Explicit On
Option Strict On

Public Class frmHapticCurve
    Public station As Integer
    Public WiliIndexShow As Integer
    Private Sub btnWS03FL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03FL.Click
        station = 3
        frmResults.WiliIndexShow = mWS03Main.eWindows.FrontLeft
        Me.Close()
    End Sub

    Private Sub btnWS03FR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03FR.Click
        station = 3
        frmResults.WiliIndexShow = mWS03Main.eWindows.FrontRight
        Me.Close()
    End Sub

    Private Sub btnWS03CL_Click(sender As Object, e As EventArgs) Handles btnWS03CL.Click
        station = 3
        frmResults.WiliIndexShow = 2
        Me.Close()
    End Sub

    Private Sub btnWS04RL_Click(sender As Object, e As EventArgs) Handles btnWS04RL.Click
        station = 4
        frmResults.WiliIndexShow = mWS04Main.eWindows.RearLeft
        Me.Close()
    End Sub

    Private Sub btnWS04RR_Click(sender As Object, e As EventArgs) Handles btnWS04RR.Click
        station = 4
        frmResults.WiliIndexShow = mWS04Main.eWindows.RearRight
        Me.Close()
    End Sub

    Private Sub btnWS04Folding_Click(sender As Object, e As EventArgs) Handles btnWS04Folding.Click
        station = 4
        frmResults.WiliIndexShow = 2
        Me.Close()
    End Sub

    Private Sub btnWS05MirrorUp_Click(sender As Object, e As EventArgs) Handles btnWS05MirrorUp.Click
        station = 5
        frmResults.WiliIndexShow = eMirrorPush.MirrorUP
        Me.Close()
    End Sub

    Private Sub btnWS05MirrorDn_Click(sender As Object, e As EventArgs) Handles btnWS05MirrorDn.Click
        station = 5
        frmResults.WiliIndexShow = eMirrorPush.MirrorDN
        Me.Close()
    End Sub

    Private Sub btnWS05MirrorRight_Click(sender As Object, e As EventArgs) Handles btnWS05MirrorRight.Click
        station = 5
        frmResults.WiliIndexShow = eMirrorPush.MirrorMR
        Me.Close()
    End Sub

    Private Sub btnWS05MirrorLeft_Click(sender As Object, e As EventArgs) Handles btnWS05MirrorLeft.Click
        station = 5
        frmResults.WiliIndexShow = eMirrorPush.MirrorML
        Me.Close()
    End Sub

    Private Sub btnWS05MirrorSelectRight_Click(sender As Object, e As EventArgs) Handles btnWS05MirrorSelectRight.Click
        station = 5
        frmResults.WiliIndexShow = eMirrorPush.MirrorSR
        Me.Close()
    End Sub

    Private Sub btnWS05MirrorSelectLeft_Click(sender As Object, e As EventArgs) Handles btnWS05MirrorSelectLeft.Click
        station = 5
        frmResults.WiliIndexShow = eMirrorPush.MirrorSL
        Me.Close()
    End Sub
End Class