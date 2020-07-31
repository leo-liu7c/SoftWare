Option Explicit On
Option Strict On

Public Class frmMessage

    Public Enum eButtons
        None = 0
        OkOnly = 1
        OkCancel = 2
        YesNo = 3
        YesNoCancel = 4
    End Enum

    Public Enum eChoice
        Cancel = 0
        No = 2
        Ok = 3
        Yes = 4
    End Enum

    Public Enum eType
        Critical = 0
        Information = 1
        Question = 2
        Warning = 3
    End Enum

    Private mbMessageButtons As eButtons
    Private mcMessageChoice As eChoice
    Private mtMessageType As eType
    Private strMessage As String
    Private _retry As Boolean = False
    Private _starttime As Date


    ReadOnly Property MessageChoice() As eChoice
        Get
            ' Returns the user's choice
            MessageChoice = mcMessageChoice
        End Get
    End Property


    WriteOnly Property MessageButtons() As eButtons
        Set(ByVal Value As eButtons)
            ' Sets the message buttons
            mbMessageButtons = Value
        End Set
    End Property


    WriteOnly Property MessageType() As eType
        Set(ByVal Value As eType)
            ' Sets the message type
            mtMessageType = Value
        End Set
    End Property


    WriteOnly Property Message() As String
        Set(ByVal Value As String)
            ' Sets the message
            strMessage = Value
        End Set
    End Property

    WriteOnly Property Retry() As Boolean
        Set(ByVal Value As Boolean)
            ' Sets the message
            _retry = Value
        End Set
    End Property


    Private Sub frmMessage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim l As Integer

        'init button
        btnCancel.Visible = False
        btnNo.Visible = False
        btnYes.Visible = False
        btnOk.Visible = False

        ' Sets the message image and the form Text
        If (mtMessageType = eType.Critical) Then
            imgMessage.Image = imgError.Image
            Me.Text = "Error message"
        ElseIf (mtMessageType = eType.Information) Then
            imgMessage.Image = imgInformation.Image
            Me.Text = "Information message"
        ElseIf (mtMessageType = eType.Question) Then
            imgMessage.Image = imgQuestion.Image
            Me.Text = "Question message"
        ElseIf (mtMessageType = eType.Warning) Then
            imgMessage.Image = imgWarning.Image
            Me.Text = "Warning message"
        End If
        ' Sets the message buttons
        If (mbMessageButtons = eButtons.OkCancel) Then
            l = (Me.Width - btnOk.Width - btnCancel.Width - 240) \ 2
            btnCancel.Visible = True
            btnCancel.Left = l
            btnOk.Visible = True
            btnOk.Left = btnCancel.Left + btnCancel.Width + 240
        ElseIf (mbMessageButtons = eButtons.OkOnly) Then
            l = (Me.Width - btnOk.Width) \ 2
            btnOk.Visible = True
            btnOk.Left = l
        ElseIf (mbMessageButtons = eButtons.YesNo) Then
            l = (Me.Width - btnYes.Width - btnNo.Width - 240) \ 2
            btnYes.Visible = True
            btnYes.Left = l
            btnNo.Visible = True
            btnNo.Left = btnYes.Left + btnYes.Width + 240
        ElseIf (mbMessageButtons = eButtons.YesNoCancel) Then
            l = (Me.Width - btnYes.Width - btnNo.Width - btnCancel.Width - 2 * 140) \ 2
            btnYes.Visible = True
            btnYes.Left = l
            btnNo.Visible = True
            btnNo.Left = btnYes.Left + btnYes.Width + 140
            btnCancel.Visible = True
            btnCancel.Left = btnNo.Left + btnNo.Width + 140
        End If
        ' Sets the message label
        labMessage.Text = strMessage
        If _retry Then
            TimerRetry.Start()
            _starttime = Date.Now
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Stores the user's choice
        mcMessageChoice = eChoice.Cancel
        _retry = False
        ' Unloads the form
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ' Stores the user's choice
        mcMessageChoice = eChoice.Ok
        _retry = False
        ' Unloads the form
        Me.Close()

    End Sub

    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNo.Click
        ' Stores the user's choice
        mcMessageChoice = eChoice.No
        _retry = False
        ' Unloads the form
        Me.Close()

    End Sub

    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYes.Click
        ' Stores the user's choice
        mcMessageChoice = eChoice.Yes
        _retry = False
        ' Unloads the form
        Me.Close()

    End Sub

    Private Sub TimerRetry_Tick(sender As Object, e As EventArgs) Handles TimerRetry.Tick

        If (Date.Now - _starttime).TotalSeconds > 3 Then
            mcMessageChoice = eChoice.Yes
            Me.Close()
        End If

    End Sub
End Class