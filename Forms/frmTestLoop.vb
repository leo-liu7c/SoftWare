Option Explicit On
Option Strict On

Public Class fmrTestLoop

    ' Private variables
    Private _loopEnable As Boolean = False

    WriteOnly Property LoopEnable As Boolean
        Set(ByVal Value As Boolean)
            ' Sets the message buttons
            _loopEnable = Value
        End Set
    End Property

    Private Sub fmrTestLoop_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.Visible = False
        ' If the loop is not enabled
        If Not (_loopEnable) Then
            ' Enable the loop
            _loopEnable = True
            Dim tStartLoop As DateTime
            Dim tReadDI As DateTime
            ' While the loop is enables
            While (_loopEnable)
                mDIOManager.ReadDigitalInputs()
                tStartLoop = Now
                tReadDI = Now
                ' station WS02 loop
                mWS02Main.TestLoop()
                If (Date.Now - tReadDI).TotalMilliseconds > 4 Then
                    mDIOManager.ReadDigitalInputs()
                    tStartLoop = Now
                    tReadDI = Now
                End If
                ' station WS03 loop
                mWS03Main.TestLoop()
                If (Date.Now - tReadDI).TotalMilliseconds > 4 Then
                    mDIOManager.ReadDigitalInputs()
                    tStartLoop = Now
                    tReadDI = Now
                End If
                ' station WS04 loop
                mWS04Main.TestLoop()
                If (Date.Now - tReadDI).TotalMilliseconds > 4 Then
                    mDIOManager.ReadDigitalInputs()
                    tStartLoop = Now
                    tReadDI = Now
                End If
                ' station WS05 loop
                mWS05Main.TestLoop()
                If mWS04Main.Phase = mWS04Main.ePhase.WaitStartTest AndAlso mWS05Main.Phase = mWS05Main.ePhase.WaitStartTest AndAlso mWS04_05AIOManager.Sample_Task_Started Then
                    'Add by YAN.Qian20200225, to reduce the timing of haptic errors.
                    mWS04_05AIOManager.StopSampling()
                End If
                ' Application events
                Application.DoEvents()
                If Convert.ToInt32((Date.Now - tStartLoop).TotalMilliseconds) < 5 Then
                    Threading.Thread.Sleep(5 - Convert.ToInt32((Date.Now - tStartLoop).TotalMilliseconds))
                End If
            End While
        End If

    End Sub

    Private Sub fmrTestLoop_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '
        _loopEnable = False

    End Sub

    Private Sub fmrTestLoop_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class