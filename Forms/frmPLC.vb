Option Explicit On
Option Strict On

Imports System.Math

Public Class frmPLC
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _timerInterval_ms = 200
    ' Private variables
    Private _btnVIPA(0 To mDIOManager.eDigitalOutput.Count - 1) As Button
    Private _lblVIPA(0 To mDIOManager.eDigitalInput.Count - 1) As Label
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




    Private Sub frmPLC_FormClosing_(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Disable the timer monitor
        tmrMonitor.Enabled = False
        ' Close the alarms form
        frmAlarms.Close()

    End Sub


    Private Sub frmPLC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim v As Single
        Dim r As Integer
        'Output
        'WS02
        _btnVIPA(0) = btnVIPA_0
        _btnVIPA(1) = btnVIPA_1
        _btnVIPA(2) = btnVIPA_2
        _btnVIPA(3) = btnVIPA_3
        _btnVIPA(4) = btnVIPA_4
        _btnVIPA(5) = btnVIPA_5
        _btnVIPA(6) = btnVIPA_6
        _btnVIPA(7) = btnVIPA_7
        'WS03
        _btnVIPA(8) = btnVIPA_8
        _btnVIPA(9) = btnVIPA_9
        _btnVIPA(10) = btnVIPA_10
        _btnVIPA(11) = btnVIPA_11
        _btnVIPA(12) = btnVIPA_12
        _btnVIPA(13) = btnVIPA_13
        _btnVIPA(14) = btnVIPA_14
        _btnVIPA(15) = btnVIPA_15
        'WS04
        _btnVIPA(16) = btnVIPA_16
        _btnVIPA(17) = btnVIPA_17
        _btnVIPA(18) = btnVIPA_18
        _btnVIPA(19) = btnVIPA_19
        _btnVIPA(20) = btnVIPA_20
        _btnVIPA(21) = btnVIPA_21
        _btnVIPA(22) = btnVIPA_22
        _btnVIPA(23) = btnVIPA_23
        'WS05
        _btnVIPA(24) = btnVIPA_24
        _btnVIPA(25) = btnVIPA_25
        _btnVIPA(26) = btnVIPA_26
        _btnVIPA(27) = btnVIPA_27
        _btnVIPA(28) = btnVIPA_28
        _btnVIPA(29) = btnVIPA_29
        _btnVIPA(30) = btnVIPA_30
        _btnVIPA(31) = btnVIPA_31
        'Keyence
        _btnVIPA(32) = btnVIPA_32
        _btnVIPA(33) = btnVIPA_33
        _btnVIPA(34) = btnVIPA_34
        _btnVIPA(35) = btnVIPA_35
        _btnVIPA(36) = btnVIPA_36
        _btnVIPA(37) = btnVIPA_37
        _btnVIPA(38) = btnVIPA_38
        _btnVIPA(39) = btnVIPA_39

        'Input
        'WS02
        _lblVIPA(0) = lblVIPA_0
        _lblVIPA(1) = lblVIPA_1
        _lblVIPA(2) = lblVIPA_2
        _lblVIPA(3) = lblVIPA_3
        _lblVIPA(4) = lblVIPA_4
        _lblVIPA(5) = lblVIPA_5
        _lblVIPA(6) = lblVIPA_6
        _lblVIPA(7) = lblVIPA_7
        'WS03
        _lblVIPA(8) = lblVIPA_8
        _lblVIPA(9) = lblVIPA_9
        _lblVIPA(10) = lblVIPA_10
        _lblVIPA(11) = lblVIPA_11
        _lblVIPA(12) = lblVIPA_12
        _lblVIPA(13) = lblVIPA_13
        _lblVIPA(14) = lblVIPA_14
        _lblVIPA(15) = lblVIPA_15
        'WS04
        _lblVIPA(16) = lblVIPA_16
        _lblVIPA(17) = lblVIPA_17
        _lblVIPA(18) = lblVIPA_18
        _lblVIPA(19) = lblVIPA_19
        _lblVIPA(20) = lblVIPA_20
        _lblVIPA(21) = lblVIPA_21
        _lblVIPA(22) = lblVIPA_22
        _lblVIPA(23) = lblVIPA_23
        _lblVIPA(24) = lblVIPA_24
        'WS05
        _lblVIPA(25) = lblVIPA_25
        _lblVIPA(26) = lblVIPA_26
        _lblVIPA(27) = lblVIPA_27
        _lblVIPA(28) = lblVIPA_28
        _lblVIPA(29) = lblVIPA_29
        _lblVIPA(30) = lblVIPA_30
        _lblVIPA(31) = lblVIPA_31
        'Keyence
        _lblVIPA(32) = lblVIPA_32
        _lblVIPA(33) = lblVIPA_33
        _lblVIPA(34) = lblVIPA_34
        _lblVIPA(35) = lblVIPA_35
        _lblVIPA(36) = lblVIPA_36
        _lblVIPA(37) = lblVIPA_37
        _lblVIPA(38) = lblVIPA_38
        _lblVIPA(39) = lblVIPA_39

        ' Show the digital output descriptions
        For i = 0 To mDIOManager.eDigitalOutput.Count - 1
            _btnVIPA(i).Text = mDIOManager.DigitalOutputDescription(CType(i, mDIOManager.eDigitalOutput))
        Next

        For i = 0 To mDIOManager.eDigitalInput.Count - 1
            _lblVIPA(i).Text = mDIOManager.DigitalInputDescription(CType(i, mDIOManager.eDigitalInput))
        Next

        InitWS02PLCtoEOL()

        ' Configurate and enable the timer monitor
        tmrMonitor.Interval = _timerInterval_ms
        tmrMonitor.Enabled = True

    End Sub

    Private Sub InitWS02PLCtoEOL()
        ' Initialize the table of single test results
        With dgvWS02PLCtoEOL
            ' Disable the refresh
            .SuspendLayout()
            ' Configurate the rows
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            ' If the reference type is barrette
            ' Set the row count
            .RowCount = mWS02Ethernet.eInput.Count + mWS03Ethernet.eInput.Count + mWS04Ethernet.eInput.Count + mWS05Ethernet.eInput.Count + 1
            ' Set the row height
            For i = 0 To .RowCount - 1
                .Rows(i).Height = 25
            Next
            ' Set the table height
            .Height = 800 ' 25 * .RowCount
            ' Configurate the columns
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersVisible = False
            .ColumnCount = 2
            ' Configurate the scrollbars and the column width
            .ScrollBars = ScrollBars.Vertical
            .Columns(0).Width = CInt(0.5 * .Width)
            .Columns(1).Width = CInt(0.5 * .Width)
            ' Configurate the cell styles
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.Font = New Font("Arial", 12)
            .Item(0, 0).Style.BackColor = Color.Blue
            .Item(0, 0).Style.ForeColor = Color.White
            .Item(1, 0).Style.BackColor = Color.Blue
            .Item(1, 0).Style.ForeColor = Color.White
            ' Print the column headers
            .Item(0, 0).Value = "Data Description"
            .Item(1, 0).Value = "Values"
            ' Print the single test descriptions
            For i = 1 To mWS02Ethernet.eInput.Count
                .Item(0, i).Value = mWS02Ethernet.DataDescription(i - 1)
            Next
            For i = 1 To mWS03Ethernet.eInput.Count
                .Item(0, i + mWS02Ethernet.eInput.Count).Value = mWS03Ethernet.DataDescription(i - 1)
            Next
            For i = 1 To mWS04Ethernet.eInput.Count
                .Item(0, i + mWS02Ethernet.eInput.Count + mWS03Ethernet.eInput.Count).Value = mWS04Ethernet.DataDescription(i - 1)
            Next
            For i = 1 To mWS05Ethernet.eInput.Count
                .Item(0, i + mWS02Ethernet.eInput.Count + mWS03Ethernet.eInput.Count + mWS04Ethernet.eInput.Count).Value = mWS05Ethernet.DataDescription(i - 1)
            Next
            ' Configurate the edit mode
            .EditMode = DataGridViewEditMode.EditProgrammatically
            ' Disable multiple selection
            .MultiSelect = False
            ' Enable the refresh
            .RefreshEdit()
            .ResumeLayout(True)
        End With
    End Sub

    Private Sub tmrMonitor_Tick_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMonitor.Tick
        Dim formatString As String
        Dim i As Integer

        ' Disable the timer
        tmrMonitor.Enabled = False


        ' Read the digital inputs
        mDIOManager.ReadDigitalInputs()
        ' Show the status of digital outputs
        For i = 0 To mDIOManager.eDigitalOutput.Count - 1
            If (mDIOManager.DigitalOutputStatus(CType(i, mDIOManager.eDigitalOutput))) Then
                _btnVIPA(i).BackColor = Color.Yellow
            Else
                _btnVIPA(i).BackColor = SystemColors.ButtonFace
                _btnVIPA(i).UseVisualStyleBackColor = True
            End If
        Next

        ' Show the status of digital inputs
        For i = 0 To mDIOManager.eDigitalInput.Count - 1
            If (mDIOManager.DigitalInputStatus(CType(i, mDIOManager.eDigitalInput))) Then
                _lblVIPA(i).BackColor = Color.Yellow
            Else
                _lblVIPA(i).BackColor = Color.White
            End If
        Next

        mWS02Ethernet.ReadInputDataBlock()
        ' Show the station WS02 single test results
        With dgvWS02PLCtoEOL
            For i = 1 To mWS02Ethernet.eInput.Count - 1
                .Item(1, i).Value = mWS02Ethernet.InputValue(CType(mWS02Ethernet.eInput.Reserve + i - 1, mWS02Ethernet.eInput))
            Next i
        End With

        mWS03Ethernet.ReadInputDataBlock()
        ' Show the station WS02 single test results
        With dgvWS02PLCtoEOL
            For i = 1 To mWS03Ethernet.eInput.Count - 1
                .Item(1, i + mWS02Ethernet.eInput.Count).Value = mWS03Ethernet.InputValue(CType(mWS03Ethernet.eInput.Reserve + i - 1, mWS03Ethernet.eInput))
            Next i
        End With

        mWS04Ethernet.ReadInputDataBlock()
        ' Show the station WS02 single test results
        With dgvWS02PLCtoEOL
            For i = 1 To mWS04Ethernet.eInput.Count - 1
                .Item(1, i + mWS02Ethernet.eInput.Count + mWS03Ethernet.eInput.Count).Value = mWS04Ethernet.InputValue(CType(mWS04Ethernet.eInput.Reserve + i - 1, mWS04Ethernet.eInput))
            Next i
        End With

        mWS05Ethernet.ReadInputDataBlock()
        ' Show the station WS02 single test results
        With dgvWS02PLCtoEOL
            For i = 1 To mWS05Ethernet.eInput.Count - 1
                .Item(1, i + mWS02Ethernet.eInput.Count + mWS03Ethernet.eInput.Count + mWS04Ethernet.eInput.Count).Value = mWS05Ethernet.InputValue(CType(mWS05Ethernet.eInput.Reserve + i - 1, mWS05Ethernet.eInput))
            Next i
        End With
        ' Re-enable the timer
        tmrMonitor.Enabled = True

    End Sub



    Private Sub btnVIPA_DO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVIPA_0.Click, _
                                                                                                    btnVIPA_1.Click, _
                                                                                                    btnVIPA_2.Click, _
                                                                                                    btnVIPA_3.Click, _
                                                                                                    btnVIPA_4.Click, _
                                                                                                    btnVIPA_5.Click, _
                                                                                                    btnVIPA_6.Click, _
                                                                                                    btnVIPA_7.Click, _
                                                                                                    btnVIPA_8.Click, _
                                                                                                    btnVIPA_9.Click, _
                                                                                                    btnVIPA_10.Click, _
                                                                                                    btnVIPA_11.Click, _
                                                                                                    btnVIPA_12.Click, _
                                                                                                    btnVIPA_13.Click, _
                                                                                                    btnVIPA_14.Click, _
                                                                                                    btnVIPA_15.Click, _
                                                                                                    btnVIPA_16.Click, _
                                                                                                    btnVIPA_17.Click, _
                                                                                                    btnVIPA_18.Click, _
                                                                                                    btnVIPA_19.Click, _
                                                                                                    btnVIPA_20.Click, _
                                                                                                    btnVIPA_21.Click, _
                                                                                                    btnVIPA_22.Click, _
                                                                                                    btnVIPA_23.Click, _
                                                                                                    btnVIPA_24.Click, _
                                                                                                    btnVIPA_25.Click, _
                                                                                                    btnVIPA_26.Click, _
                                                                                                    btnVIPA_27.Click, _
                                                                                                    btnVIPA_28.Click, _
                                                                                                    btnVIPA_29.Click, _
                                                                                                    btnVIPA_30.Click, _
                                                                                                    btnVIPA_31.Click, _
                                                                                                    btnVIPA_32.Click, _
                                                                                                    btnVIPA_33.Click, _
                                                                                                    btnVIPA_34.Click, _
                                                                                                    btnVIPA_35.Click, _
                                                                                                    btnVIPA_36.Click, _
                                                                                                    btnVIPA_37.Click, _
                                                                                                    btnVIPA_38.Click, _
                                                                                                    btnVIPA_39.Click
        Dim button As Button
        Dim index As Integer

        button = CType(sender, Button)
        index = Array.IndexOf(_btnVIPA, button)
        If (mDIOManager.WriteDigitalOutput(CType(index, mDIOManager.eDigitalOutput), Not (mDIOManager.DigitalOutputStatus(CType(index, mDIOManager.eDigitalOutput))))) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while writing the digital outputs."
            frmMessage.ShowDialog()
        End If

    End Sub


    Private Sub pbLogoValeo1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbLogoValeo.Click
        mUserManager.Logout()
        Me.Close()
    End Sub


End Class