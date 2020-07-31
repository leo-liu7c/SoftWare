Option Explicit On
Option Strict On

Public Class frmLogin
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _maximumLength = 40

    ' Private variables
    Private _confirm As Boolean
    Private _password As String
    Private _selectedValue As TextBox
    Private _username As String



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property Confirm As Boolean
        Get
            Confirm = _confirm
        End Get
    End Property



    Public Property Password As String
        Get
            Password = _Password
        End Get
        Set(propertyValue As String)
            _Password = propertyValue
        End Set
    End Property



    Public Property Username As String
        Get
            Username = _username
        End Get
        Set(propertyValue As String)
            _username = propertyValue
        End Set
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
    Private Sub btnBackspace_Click(sender As System.Object, e As System.EventArgs) Handles btnBackspace.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a backspace key
        SendKeys.Send("{BACKSPACE}")
    End Sub



    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        ' Clear the flag of value confirmed
        _confirm = False
        ' Close the form
        Me.Close()
    End Sub



    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a delete key
        SendKeys.Send("{DELETE}")
    End Sub



    Private Sub btnDownArrow_Click(sender As System.Object, e As System.EventArgs) Handles btnDownArrow.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a down arrow key
        SendKeys.Send("{DOWN}")
    End Sub



    Private Sub btnEnd_Click(sender As System.Object, e As System.EventArgs) Handles btnEnd.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send an end key
        SendKeys.Send("{END}")
    End Sub



    Private Sub btnHome_Click(sender As System.Object, e As System.EventArgs) Handles btnHome.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a home key
        SendKeys.Send("{HOME}")
    End Sub



    Private Sub btnInsert_Click(sender As System.Object, e As System.EventArgs) Handles btnInsert.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send an insert key
        SendKeys.Send("{INSERT}")
    End Sub



    Private Sub btnKey_Click(sender As System.Object, e As System.EventArgs) Handles btnKey0.Click, _
                                                                                     btnKey1.Click, _
                                                                                     btnKey2.Click, _
                                                                                     btnKey3.Click, _
                                                                                     btnKey4.Click, _
                                                                                     btnKey5.Click, _
                                                                                     btnKey6.Click, _
                                                                                     btnKey7.Click, _
                                                                                     btnKey8.Click, _
                                                                                     btnKey9.Click, _
                                                                                     btnKey10.Click, _
                                                                                     btnKey11.Click, _
                                                                                     btnKey12.Click, _
                                                                                     btnKey13.Click, _
                                                                                     btnKey14.Click, _
                                                                                     btnKey15.Click, _
                                                                                     btnKey16.Click, _
                                                                                     btnKey17.Click, _
                                                                                     btnKey18.Click, _
                                                                                     btnKey19.Click, _
                                                                                     btnKey20.Click, _
                                                                                     btnKey21.Click, _
                                                                                     btnKey22.Click, _
                                                                                     btnKey23.Click, _
                                                                                     btnKey24.Click, _
                                                                                     btnKey25.Click, _
                                                                                     btnKey26.Click, _
                                                                                     btnKey27.Click, _
                                                                                     btnKey28.Click, _
                                                                                     btnKey29.Click, _
                                                                                     btnKey30.Click, _
                                                                                     btnKey31.Click, _
                                                                                     btnKey32.Click, _
                                                                                     btnKey33.Click, _
                                                                                     btnKey34.Click, _
                                                                                     btnKey35.Click, _
                                                                                     btnKey36.Click, _
                                                                                     btnKey37.Click, _
                                                                                     btnKey38.Click, _
                                                                                     btnKey39.Click, _
                                                                                     btnKey40.Click, _
                                                                                     btnKey41.Click, _
                                                                                     btnKey42.Click, _
                                                                                     btnKey43.Click, _
                                                                                     btnKey44.Click, _
                                                                                     btnKey45.Click, _
                                                                                     btnKey46.Click, _
                                                                                     btnKey47.Click, _
                                                                                     btnKey48.Click, _
                                                                                     btnKey49.Click, _
                                                                                     btnKey50.Click, _
                                                                                     btnKey51.Click, _
                                                                                     btnKey52.Click, _
                                                                                     btnKey53.Click, _
                                                                                     btnKey54.Click, _
                                                                                     btnKey55.Click, _
                                                                                     btnKey56.Click, _
                                                                                     btnKey57.Click, _
                                                                                     btnKey58.Click, _
                                                                                     btnKey59.Click, _
                                                                                     btnKey60.Click, _
                                                                                     btnKey61.Click, _
                                                                                     btnKey62.Click
        Dim senderButton As Button

        ' Convert the sender
        senderButton = CType(sender, Button)
        ' Focus the value
        _selectedValue.Focus()
        ' Send the button text key
        If (senderButton.Text = "{") Then
            SendKeys.Send("{{}")
        ElseIf (senderButton.Text = "}") Then
            SendKeys.Send("{}}")
        Else
            SendKeys.Send(senderButton.Text)
        End If
        ' Uncheck the Alt Gr and Shift buttons
        chbAltGr.Checked = False
        chbShift1.Checked = False
        chbShift2.Checked = False
    End Sub



    Private Sub btnLeftArrow_Click(sender As System.Object, e As System.EventArgs) Handles btnLeftArrow.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a left arrow key
        SendKeys.Send("{LEFT}")
    End Sub



    Private Sub btnOk_Click(sender As System.Object, e As System.EventArgs) Handles btnOk.Click
        ' Store the username and the password
        _username = tbUsername.Text
        _password = tbPassword.Text
        ' Set the flag of value confirmed
        _confirm = True
        ' Close the form
        Me.Close()
    End Sub



    Private Sub btnPagDown_Click(sender As System.Object, e As System.EventArgs) Handles btnPagDown.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a Pag Down key
        SendKeys.Send("{PGDN}")
    End Sub



    Private Sub btnPagUp_Click(sender As System.Object, e As System.EventArgs) Handles btnPagUp.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a Pag Up key
        SendKeys.Send("{PGUP}")
    End Sub



    Private Sub btnRightArrow_Click(sender As System.Object, e As System.EventArgs) Handles btnRightArrow.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a right arrow key
        SendKeys.Send("{RIGHT}")
    End Sub



    Private Sub btnSpace_Click(sender As System.Object, e As System.EventArgs) Handles btnSpace.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send a space key
        SendKeys.Send(" ")
    End Sub



    Private Sub btnUpArrow_Click(sender As System.Object, e As System.EventArgs) Handles btnUpArrow.Click
        ' Focus the value
        _selectedValue.Focus()
        ' Send an up arrow key
        SendKeys.Send("{UP}")
    End Sub



    Private Sub chbCheckBox_CheckedChanged(sender As Object, e As System.EventArgs) Handles chbAltGr.CheckedChanged, _
                                                                                            chbCapsLock.CheckedChanged, _
                                                                                            chbShift1.CheckedChanged, _
                                                                                            chbShift2.CheckedChanged
        ' Update the keys
        If (chbCapsLock.Checked Or chbShift1.Checked Or chbShift2.Checked) Then
            btnKey0.Text = "|"
            btnKey1.Text = "!"
            btnKey2.Text = """"
            btnKey3.Text = "£"
            btnKey4.Text = "$"
            btnKey5.Text = "%"
            btnKey6.Text = "&&"
            btnKey7.Text = "/"
            btnKey8.Text = "("
            btnKey9.Text = ")"
            btnKey10.Text = "="
            btnKey11.Text = "?"
            btnKey12.Text = "^"
            btnKey13.Text = "Q"
            btnKey14.Text = "W"
            btnKey15.Text = "E"
            btnKey16.Text = "R"
            btnKey17.Text = "T"
            btnKey18.Text = "Y"
            btnKey19.Text = "U"
            btnKey20.Text = "I"
            btnKey21.Text = "O"
            btnKey22.Text = "P"
            btnKey25.Text = "A"
            btnKey26.Text = "S"
            btnKey27.Text = "D"
            btnKey28.Text = "F"
            btnKey29.Text = "G"
            btnKey30.Text = "H"
            btnKey31.Text = "J"
            btnKey32.Text = "K"
            btnKey33.Text = "L"
            btnKey36.Text = "§"
            btnKey37.Text = ">"
            btnKey38.Text = "Z"
            btnKey39.Text = "X"
            btnKey40.Text = "C"
            btnKey41.Text = "V"
            btnKey42.Text = "B"
            btnKey43.Text = "N"
            btnKey44.Text = "M"
            btnKey45.Text = ";"
            btnKey46.Text = ":"
            btnKey47.Text = "_"
            btnKey48.Text = "0"
            btnKey49.Text = "1"
            btnKey50.Text = "2"
            btnKey51.Text = "3"
            btnKey52.Text = "4"
            btnKey53.Text = "5"
            btnKey54.Text = "6"
            btnKey55.Text = "7"
            btnKey56.Text = "8"
            btnKey57.Text = "9"
            btnKey58.Text = ","
            btnKey59.Text = "+"
            btnKey60.Text = "-"
            btnKey61.Text = "*"
            btnKey62.Text = "/"
            If (chbAltGr.Checked) Then
                btnKey23.Text = "{"
                btnKey24.Text = "}"
                btnKey34.Text = ""
                btnKey35.Text = ""
            Else
                btnKey23.Text = "é"
                btnKey24.Text = "*"
                btnKey34.Text = "ç"
                btnKey35.Text = "°"
            End If
        ElseIf (chbCapsLock.Checked = False) Then
            btnKey0.Text = "\"
            btnKey1.Text = "1"
            btnKey2.Text = "2"
            btnKey3.Text = "3"
            btnKey4.Text = "4"
            btnKey5.Text = "5"
            btnKey6.Text = "6"
            btnKey7.Text = "7"
            btnKey8.Text = "8"
            btnKey9.Text = "9"
            btnKey10.Text = "0"
            btnKey11.Text = "'"
            btnKey12.Text = "ì"
            btnKey13.Text = "q"
            btnKey14.Text = "w"
            btnKey15.Text = "e"
            btnKey16.Text = "r"
            btnKey17.Text = "t"
            btnKey18.Text = "y"
            btnKey19.Text = "u"
            btnKey20.Text = "i"
            btnKey21.Text = "o"
            btnKey22.Text = "p"
            btnKey25.Text = "a"
            btnKey26.Text = "s"
            btnKey27.Text = "d"
            btnKey28.Text = "f"
            btnKey29.Text = "g"
            btnKey30.Text = "h"
            btnKey31.Text = "j"
            btnKey32.Text = "k"
            btnKey33.Text = "l"
            btnKey36.Text = "ù"
            btnKey37.Text = "<"
            btnKey38.Text = "z"
            btnKey39.Text = "x"
            btnKey40.Text = "c"
            btnKey41.Text = "v"
            btnKey42.Text = "b"
            btnKey43.Text = "n"
            btnKey44.Text = "m"
            btnKey45.Text = ","
            btnKey46.Text = "."
            btnKey47.Text = "-"
            btnKey48.Text = "0"
            btnKey49.Text = "1"
            btnKey50.Text = "2"
            btnKey51.Text = "3"
            btnKey52.Text = "4"
            btnKey53.Text = "5"
            btnKey54.Text = "6"
            btnKey55.Text = "7"
            btnKey56.Text = "8"
            btnKey57.Text = "9"
            btnKey58.Text = ","
            btnKey59.Text = "+"
            btnKey60.Text = "-"
            btnKey61.Text = "*"
            btnKey62.Text = "/"
            If (chbAltGr.Checked) Then
                btnKey23.Text = "["
                btnKey24.Text = "]"
                btnKey34.Text = "@"
                btnKey35.Text = "#"
            Else
                btnKey23.Text = "è"
                btnKey24.Text = "+"
                btnKey34.Text = "ò"
                btnKey35.Text = "à"
            End If
        End If
    End Sub



    Private Sub chbHidePassword_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chbHidePassword.CheckedChanged
        ' Show/hide the password
        If (chbHidePassword.Checked) Then
            tbPassword.PasswordChar = CChar("*")
        Else
            tbPassword.PasswordChar = CChar("")
        End If
    End Sub



    Private Sub frmLogin_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        ' If the user pressed Return
        If (e.KeyChar = Chr(Keys.Return)) Then
            ' Simulate the user to press the Ok button
            btnOk.PerformClick()
            e.Handled = True
        ElseIf (e.KeyChar = Chr(Keys.Escape)) Then    ' Otherwise, if the user pressed Escape
            ' Simulate the user to press the Cancel button
            btnCancel.PerformClick()
            e.Handled = True
        End If
    End Sub



    Private Sub frmLogin_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Set the username and password maximum length
        tbUsername.MaxLength = _maximumLength
        tbPassword.MaxLength = _maximumLength
        ' Set the username and password
        tbUsername.Text = _username
        tbPassword.Text = _password
        ' Switch-on the caps lock
        chbCapsLock.Checked = True
        ' Hide the password
        chbHidePassword.Checked = True
        ' Select the active control
        Me.ActiveControl = tbUsername
        ' Focus the value
        tbUsername.Focus()
    End Sub



    Private Sub tbPassword_GotFocus(sender As Object, e As System.EventArgs) Handles tbPassword.GotFocus
        ' Select and highlight the password
        tbUsername.BackColor = Color.White
        tbPassword.BackColor = Color.Yellow
        _selectedValue = tbPassword
    End Sub



    Private Sub tbUsername_GotFocus(sender As Object, e As System.EventArgs) Handles tbUsername.GotFocus
        ' Select and highlight the minimum limit
        tbUsername.BackColor = Color.Yellow
        tbPassword.BackColor = Color.White
        _selectedValue = tbUsername
    End Sub
End Class