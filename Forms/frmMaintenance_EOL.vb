Option Explicit On
'Option Strict On

Imports System.Math
Imports System
Imports System.IO
Imports Microsoft.VisualBasic

Public Class frmMaintenance_EOL
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _samplingFrequency_Hz = 5000
    Private Const _timerInterval_ms = 200

    Private LinConnected_WS02 As Boolean
    ' Private variables
    Private _btnWS02_DO(0 To mWS02DIOManager.eDigitalOutput.Count - 1) As Button
    Private LinWS02Connected As Boolean

    Private LinConnected_WS03 As Boolean
    ' Private variables
    Private _btnWS03_DO(0 To mWS03DIOManager.eDigitalOutput.Count - 1) As Button
    Private LinWS03Connected As Boolean

    Private LinConnected_WS04 As Boolean
    ' Private variables
    Private _btnWS04_DO(0 To mWS04DIOManager.eDigitalOutput.Count - 1) As Button
    Private LinWS04Connected As Boolean

    Private LinConnected_WS05 As Boolean
    ' Private variables
    Private _btnWS05_DO(0 To mWS05DIOManager.eDigitalOutput.Count - 1) As Button
    Private LinWS05Connected As Boolean

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




    Private Sub frmMaintenance_FormClosing_1(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Disable the timer monitor
        tmrMonitor.Enabled = False
        '
        tmrLoop.Enabled = False
        ' Close the alarms form
        frmAlarms.Close()

    End Sub


    Private Sub frmMaintenance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim r As Integer

        ' *******************************
        ' WS02
        ' *******************************
        ' Initialize the references to digital output buttons
        _btnWS02_DO(0) = btnWS02_DO_0
        _btnWS02_DO(1) = btnWS02_DO_1
        _btnWS02_DO(2) = btnWS02_DO_2
        _btnWS02_DO(3) = btnWS02_DO_3
        _btnWS02_DO(4) = btnWS02_DO_4
        _btnWS02_DO(5) = btnWS02_DO_5
        _btnWS02_DO(6) = btnWS02_DO_6
        _btnWS02_DO(7) = btnWS02_DO_7
        _btnWS02_DO(8) = btnWS02_DO_8
        _btnWS02_DO(9) = btnWS02_DO_9
        _btnWS02_DO(10) = btnWS02_DO_10
        _btnWS02_DO(11) = btnWS02_DO_11
        _btnWS02_DO(12) = btnWS02_DO_12
        _btnWS02_DO(13) = btnWS02_DO_13
        _btnWS02_DO(14) = btnWS02_DO_14
        _btnWS02_DO(15) = btnWS02_DO_15
        _btnWS02_DO(16) = btnWS02_DO_16
        _btnWS02_DO(17) = btnWS02_DO_17
        _btnWS02_DO(18) = btnWS02_DO_18
        _btnWS02_DO(19) = btnWS02_DO_19
        _btnWS02_DO(20) = btnWS02_DO_20
        _btnWS02_DO(21) = btnWS02_DO_21
        _btnWS02_DO(22) = btnWS02_DO_22
        _btnWS02_DO(23) = btnWS02_DO_23
        _btnWS02_DO(24) = btnWS02_DO_24
        _btnWS02_DO(25) = btnWS02_DO_25
        _btnWS02_DO(26) = btnWS02_DO_26
        _btnWS02_DO(27) = btnWS02_DO_27
        _btnWS02_DO(28) = btnWS02_DO_28
        _btnWS02_DO(29) = btnWS02_DO_29
        _btnWS02_DO(30) = btnWS02_DO_30
        _btnWS02_DO(31) = btnWS02_DO_31

        ' Show the digital output descriptions
        For i = 0 To mWS02DIOManager.eDigitalOutput.Count - 1
            _btnWS02_DO(i).Text = mWS02DIOManager.DigitalOutputDescription(CType(i, mWS02DIOManager.eDigitalOutput))
        Next
        ' Show the station FW analog input descriptions
        With dgvWS02AnalogInputs
            .RowCount = mWS02AIOManager.eAnalogInput.Count
            .ColumnCount = 3
            .Columns(0).Width = CInt(0.7 * .Width)
            .Columns(1).Width = CInt(0.2 * .Width)
            .Columns(2).Width = CInt(0.1 * .Width)
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowHeadersVisible = False
            .ColumnHeadersVisible = False
            .ScrollBars = ScrollBars.None
            .Height = .RowCount * 24
            If .Height > 780 Then
                .Height = 780
            End If
            For r = 0 To .RowCount - 1
                .Rows(r).Height = 24
                .Item(0, r).Value = mWS02AIOManager.AnalogInputDescription(CType(r, mWS02AIOManager.eAnalogInput))
                .Item(2, r).Value = mWS02AIOManager.AnalogInputUnits(CType(r, mWS02AIOManager.eAnalogInput))
            Next
        End With

        ' *******************************
        ' WS03
        ' *******************************
        ' Initialize the references to digital output buttons
        _btnWS03_DO(0) = btnWS03_DO_0
        _btnWS03_DO(1) = btnWS03_DO_1
        _btnWS03_DO(2) = btnWS03_DO_2
        _btnWS03_DO(3) = btnWS03_DO_3
        _btnWS03_DO(4) = btnWS03_DO_4
        _btnWS03_DO(5) = btnWS03_DO_5
        _btnWS03_DO(6) = btnWS03_DO_6
        _btnWS03_DO(7) = btnWS03_DO_7
        _btnWS03_DO(8) = btnWS03_DO_8
        _btnWS03_DO(9) = btnWS03_DO_9
        _btnWS03_DO(10) = btnWS03_DO_10
        _btnWS03_DO(11) = btnWS03_DO_11
        _btnWS03_DO(12) = btnWS03_DO_12
        _btnWS03_DO(13) = btnWS03_DO_13
        _btnWS03_DO(14) = btnWS03_DO_14
        _btnWS03_DO(15) = btnWS03_DO_15

        ' Show the digital output descriptions
        For i = 0 To mWS03DIOManager.eDigitalOutput.Count - 1
            _btnWS03_DO(i).Text = mWS03DIOManager.DigitalOutputDescription(CType(i, mWS03DIOManager.eDigitalOutput))
        Next
        ' Show the station FW analog input descriptions
        With dgvWS03AnalogInputs
            .RowCount = mWS03AIOManager.eAnalogInput.Count
            .ColumnCount = 3
            .Columns(0).Width = CInt(0.7 * .Width)
            .Columns(1).Width = CInt(0.2 * .Width)
            .Columns(2).Width = CInt(0.1 * .Width)
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowHeadersVisible = False
            .ColumnHeadersVisible = False
            .ScrollBars = ScrollBars.None
            .Height = .RowCount * 24
            If .Height > 780 Then
                .Height = 780
            End If
            For r = 0 To .RowCount - 1
                .Rows(r).Height = 24
                .Item(0, r).Value = mWS03AIOManager.AnalogInputDescription(CType(r, mWS03AIOManager.eAnalogInput))
                .Item(2, r).Value = mWS03AIOManager.AnalogInputUnits(CType(r, mWS03AIOManager.eAnalogInput))
            Next
        End With


        ' *******************************
        ' WS04()
        ' *******************************
        ' Initialize the references to digital output buttons
        _btnWS04_DO(0) = btnWS04_DO_0
        _btnWS04_DO(1) = btnWS04_DO_1
        _btnWS04_DO(2) = btnWS04_DO_2
        _btnWS04_DO(3) = btnWS04_DO_3
        _btnWS04_DO(4) = btnWS04_DO_4
        _btnWS04_DO(5) = btnWS04_DO_5
        _btnWS04_DO(6) = btnWS04_DO_6
        _btnWS04_DO(7) = btnWS04_DO_7
        _btnWS04_DO(8) = btnWS04_DO_8
        _btnWS04_DO(9) = btnWS04_DO_9
        _btnWS04_DO(10) = btnWS04_DO_10
        _btnWS04_DO(11) = btnWS04_DO_11
        _btnWS04_DO(12) = btnWS04_DO_12
        _btnWS04_DO(13) = btnWS04_DO_13
        _btnWS04_DO(14) = btnWS04_DO_14
        _btnWS04_DO(15) = btnWS04_DO_15

        ' Show the digital output descriptions
        For i = 0 To mWS04DIOManager.eDigitalOutput.Count - 1
            _btnWS04_DO(i).Text = mWS04DIOManager.DigitalOutputDescription(CType(i, mWS04DIOManager.eDigitalOutput))
        Next
        ' Show the station FW analog input descriptions
        With dgvWS04AnalogInputs
            .RowCount = mWS04AIOManager.eAnalogInput.Count
            .ColumnCount = 3
            .Columns(0).Width = CInt(0.6 * .Width)
            .Columns(1).Width = CInt(0.3 * .Width)
            .Columns(2).Width = CInt(0.1 * .Width)
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowHeadersVisible = False
            .ColumnHeadersVisible = False
            .ScrollBars = ScrollBars.None
            .Height = .RowCount * 24
            If .Height > 780 Then
                .Height = 780
            End If
            For r = 0 To .RowCount - 1
                .Rows(r).Height = 24
                .Item(0, r).Value = mWS04AIOManager.AnalogInputDescription(CType(r, mWS04AIOManager.eAnalogInput))
                .Item(2, r).Value = mWS04AIOManager.AnalogInputUnits(CType(r, mWS04AIOManager.eAnalogInput))
            Next
        End With


        ' *******************************
        ' WS05
        ' *******************************
        ' Initialize the references to digital output buttons
        _btnWS05_DO(0) = btnWS05_DO_0
        _btnWS05_DO(1) = btnWS05_DO_1
        _btnWS05_DO(2) = btnWS05_DO_2
        _btnWS05_DO(3) = btnWS05_DO_3
        _btnWS05_DO(4) = btnWS05_DO_4
        _btnWS05_DO(5) = btnWS05_DO_5
        _btnWS05_DO(6) = btnWS05_DO_6
        _btnWS05_DO(7) = btnWS05_DO_7
        _btnWS05_DO(8) = btnWS05_DO_8
        _btnWS05_DO(9) = btnWS05_DO_9
        _btnWS05_DO(10) = btnWS05_DO_10
        _btnWS05_DO(11) = btnWS05_DO_11
        _btnWS05_DO(12) = btnWS05_DO_12
        _btnWS05_DO(13) = btnWS05_DO_13
        _btnWS05_DO(14) = btnWS05_DO_14
        _btnWS05_DO(15) = btnWS05_DO_15

        ' Show the digital output descriptions
        For i = 0 To mWS05DIOManager.eDigitalOutput.Count - 1
            _btnWS05_DO(i).Text = mWS05DIOManager.DigitalOutputDescription(CType(i, mWS05DIOManager.eDigitalOutput))
        Next
        ' Show the station FW analog input descriptions
        With dgvWS05AnalogInputs
            .RowCount = mWS05AIOManager.eAnalogInput.Count
            .ColumnCount = 3
            .Columns(0).Width = CInt(0.6 * .Width)
            .Columns(1).Width = CInt(0.3 * .Width)
            .Columns(2).Width = CInt(0.1 * .Width)
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowHeadersVisible = False
            .ColumnHeadersVisible = False
            .ScrollBars = ScrollBars.None
            .Height = .RowCount * 24
            If .Height > 780 Then
                .Height = 780
            End If
            For r = 0 To .RowCount - 1
                .Rows(r).Height = 24
                .Item(0, r).Value = mWS05AIOManager.AnalogInputDescription(CType(r, mWS05AIOManager.eAnalogInput))
                .Item(2, r).Value = mWS05AIOManager.AnalogInputUnits(CType(r, mWS05AIOManager.eAnalogInput))
            Next
        End With
        ' Configurate and enable the timer monitor
        tmrMonitor.Interval = _timerInterval_ms
        tmrMonitor.Enabled = True

        tmrLoop.Enabled = True
    End Sub


    Private Sub tmrMonitor_Tick_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMonitor.Tick
        Dim formatString As String
        Dim WS02AnalogInputValue(0 To mWS02AIOManager.eAnalogInput.Count - 1) As Double
        Dim WS03AnalogInputValue(0 To mWS03AIOManager.eAnalogInput.Count - 1) As Double
        Dim WS04AnalogInputValue(0 To mWS04AIOManager.eAnalogInput.Count - 1, 0) As Double
        Dim WS05AnalogInputValue(0 To mWS05AIOManager.eAnalogInput.Count - 1, 1) As Double
        Dim i As Integer
        Dim d(0 To 20000) As Double
        Dim sample(0 To 100) As Double
        Dim SampleCount As Integer
        ' Disable the timer
        tmrMonitor.Enabled = False

        If LinWS02Connected Then
            ' Frame
            txtWS02_Tx.Text = mWS02Main.LinLog(mGlobal.eLog_File.Frame_rx_3C) & mWS02Main.LinLog(mGlobal.eLog_File.Frame_rx_3D)
        End If


        '*******************************
        ' WS02
        '*******************************

        ' Show the status WS02 of digital outputs
        For i = 0 To mWS02DIOManager.eDigitalOutput.Count - 1
            If (mWS02DIOManager.DigitalOutputStatus(CType(i, mWS02DIOManager.eDigitalOutput))) Then
                _btnWS02_DO(i).BackColor = Color.Yellow
            Else
                _btnWS02_DO(i).BackColor = SystemColors.ButtonFace
                _btnWS02_DO(i).UseVisualStyleBackColor = True
            End If
        Next

        ' Show the station WS02 analog input values
        If Not (mWS02AIOManager.SingleSample(WS02AnalogInputValue)) Then
            For i = 0 To mWS02AIOManager.eAnalogInput.Count - 1
                formatString = "0"
                If (mWS02AIOManager.AnalogInputDecimals(CType(i, mWS02AIOManager.eAnalogInput)) > 0) Then
                    formatString = formatString & "." & StrDup(mWS02AIOManager.AnalogInputDecimals(CType(i, mWS02AIOManager.eAnalogInput)), "0")
                End If
                dgvWS02AnalogInputs.Item(1, i).Value = WS02AnalogInputValue(i).ToString(formatString)
            Next
        Else
            '    frmMessage.MessageType = frmMessage.eType.Critical
            '    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            '    frmMessage.Message = "Error while reading the station FW analog inputs."
            '    frmMessage.ShowDialog()
        End If


        '*******************************
        ' WS03
        '*******************************
        If LinWS03Connected Then
            ' Frame
            txtWS03_Tx.Text = mWS03Main.LinLog(mGlobal.eLog_File.Frame_rx_3C) & mWS03Main.LinLog(mGlobal.eLog_File.Frame_rx_3D)         
            lbl_WL_REAR_FRONT_PASSENGER.Text = CInt("&h" & Microsoft.VisualBasic.Right("0000" & mWS03Main.ADC_WL_REAR_Front_PASSENGER, 4))
            lbl_WL_REAR_LEFT.Text = CInt("&h" & Microsoft.VisualBasic.Right("0000" & mWS03Main.ADC_WL_Front_Left, 4))
            lbl_WL_REAR_RIGHT.Text = CInt("&h" & Microsoft.VisualBasic.Right("0000" & mWS03Main.ADC_WL_Front_Right, 4))
            lbl_MIRROR_ADJUSTMENT.Text = CInt("&h" & Microsoft.VisualBasic.Right("0000" & mWS03Main.ADC_MIRROR_ADJUSTMENT, 4))

            lbl_FL_Pull_A.Visible = mWS03Main.WL_FL_Pull_A
            lbl_FL_Pull_M.Visible = mWS03Main.WL_FL_Pull_M
            lbl_Fl_Push_A.Visible = mWS03Main.WL_FL_Push_A
            lbl_Fl_Push_M.Visible = mWS03Main.WL_FL_Push_M
            lbl_Child.Visible = mWS03Main.Push_Children
            lbl_SL.Visible = mWS03Main.Push_Mirror_SL
            lbl_SR.Visible = mWS03Main.Push_Mirror_SR

            If lbl_MIRROR_ADJUSTMENT.Text <> "" Then                
                If CInt(lbl_MIRROR_ADJUSTMENT.Text) > 750 And _
                    CInt(lbl_MIRROR_ADJUSTMENT.Text) < 850 Then
                    lbl_UP.Visible = True
                Else
                    lbl_UP.Visible = False
                End If

                If CInt(lbl_MIRROR_ADJUSTMENT.Text) > 450 And _
                    CInt(lbl_MIRROR_ADJUSTMENT.Text) < 550 Then
                    lbl_R.Visible = True
                Else
                    lbl_R.Visible = False
                End If

                If CInt(lbl_MIRROR_ADJUSTMENT.Text) > 850 And _
                    CInt(lbl_MIRROR_ADJUSTMENT.Text) < 950 Then
                    lbl_L.Visible = True
                Else
                    lbl_L.Visible = False
                End If

                If CInt(lbl_MIRROR_ADJUSTMENT.Text) > 600 And _
                    CInt(lbl_MIRROR_ADJUSTMENT.Text) < 700 Then
                    lbl_DN.Visible = True
                Else
                    lbl_DN.Visible = False
                End If
            End If
        End If

            ' Show the status WS03 of digital outputs
            For i = 0 To mWS03DIOManager.eDigitalOutput.Count - 1
                If (mWS03DIOManager.DigitalOutputStatus(CType(i, mWS03DIOManager.eDigitalOutput))) Then
                    _btnWS03_DO(i).BackColor = Color.Yellow
                Else
                    _btnWS03_DO(i).BackColor = SystemColors.ButtonFace
                    _btnWS03_DO(i).UseVisualStyleBackColor = True
                End If
            Next

            ' Show the station WS03 analog input values
            If Not (mWS03AIOManager.SingleSample(WS03AnalogInputValue)) Then
                For i = 0 To mWS03AIOManager.eAnalogInput.Count - 1
                    formatString = "0"
                    If (mWS03AIOManager.AnalogInputDecimals(CType(i, mWS03AIOManager.eAnalogInput)) > 0) Then
                        formatString = formatString & "." & StrDup(mWS03AIOManager.AnalogInputDecimals(CType(i, mWS03AIOManager.eAnalogInput)), "0")
                    End If
                    dgvWS03AnalogInputs.Item(1, i).Value = WS03AnalogInputValue(i).ToString(formatString)
                Next
            Else
                '    frmMessage.MessageType = frmMessage.eType.Critical
                '    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                '    frmMessage.Message = "Error while reading the station FW analog inputs."
                '    frmMessage.ShowDialog()
            End If

        '*******************************
        ' WS04
        '*******************************

            'If LinWS04Connected Then
            '    ' Frame
            '    TextBox1.Text = mWS04Main.LinLog(mGlobal.eLog_File.Frame_rx_3C) & mWS04Main.LinLog(mGlobal.eLog_File.Frame_rx_3D)
            'End If
        ' Show the status WS04 of digital outputs
        For i = 0 To mWS04DIOManager.eDigitalOutput.Count - 1
            If (mWS04DIOManager.DigitalOutputStatus(CType(i, mWS04DIOManager.eDigitalOutput))) Then
                _btnWS04_DO(i).BackColor = Color.Yellow
            Else
                _btnWS04_DO(i).BackColor = SystemColors.ButtonFace
                _btnWS04_DO(i).UseVisualStyleBackColor = True
            End If
        Next

        '' Show the station WS04 analog input values
        'If Not (mWS04_05AIOManager.SingleSample(WS04AnalogInputValue, 0)) Then
        '    For i = 0 To mWS04AIOManager.eAnalogInput.Count - 1
        '        formatString = "0"
        '        If (mWS04AIOManager.AnalogInputDecimals(CType(i, mWS04AIOManager.eAnalogInput)) > 0) Then
        '            formatString = formatString & "." & StrDup(mWS04AIOManager.AnalogInputDecimals(CType(i, mWS04AIOManager.eAnalogInput)), "0")
        '        End If
        '        dgvWS04AnalogInputs.Item(1, i).Value = WS04AnalogInputValue(i, 0).ToString(formatString)
        '    Next
        'Else
        '    '    frmMessage.MessageType = frmMessage.eType.Critical
        '    '    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
        '    '    frmMessage.Message = "Error while reading the station FW analog inputs."
        '    '    frmMessage.ShowDialog()
        'End If

        '*******************************
        ' WS05
        '*******************************
            'If LinWS05Connected Then
            '    ' Frame
            '    TextBox1.Text = mWS05Main.LinLog(mGlobal.eLog_File.Frame_rx_3C) & mWS05Main.LinLog(mGlobal.eLog_File.Frame_rx_3D)
            'End If

        ' Show the status WS05 of digital outputs
        For i = 0 To mWS05DIOManager.eDigitalOutput.Count - 1
            If (mWS05DIOManager.DigitalOutputStatus(CType(i, mWS05DIOManager.eDigitalOutput))) Then
                _btnWS05_DO(i).BackColor = Color.Yellow
            Else
                _btnWS05_DO(i).BackColor = SystemColors.ButtonFace
                _btnWS05_DO(i).UseVisualStyleBackColor = True
            End If
        Next

        '' Show the station WS05 analog input values
        'If Not (mWS04_05AIOManager.SingleSample(WS05AnalogInputValue, 1)) Then
        '    For i = 0 To mWS05AIOManager.eAnalogInput.Count - 1
        '        formatString = "0"
        '        If (mWS05AIOManager.AnalogInputDecimals(CType(i, mWS05AIOManager.eAnalogInput)) > 0) Then
        '            formatString = formatString & "." & StrDup(mWS05AIOManager.AnalogInputDecimals(CType(i, mWS05AIOManager.eAnalogInput)), "0")
        '        End If
        '        dgvWS05AnalogInputs.Item(1, i).Value = WS05AnalogInputValue(i, 1).ToString(formatString)
        '    Next
        'Else
        '    '    frmMessage.MessageType = frmMessage.eType.Critical
        '    '    frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
        '    '    frmMessage.Message = "Error while reading the station FW analog inputs."
        '    '    frmMessage.ShowDialog()
        'End If


        If Not mWS04_05AIOManager.Sample_Task_Started Then
            'empty buffer
            mWS04_05AIOManager.EmptyBuffer_WS04()
            mWS04_05AIOManager.EmptyBuffer_WS05()
            'Start the task
            mWS04_05AIOManager.StartSampling(2000)
        Else
            '*******************************
            ' WS04 Analog input
            '*******************************
            ' Store the samples
            SampleCount = mWS04_05AIOManager.SampleCount_WS04 - 1
            If SampleCount > 100 Then
                SampleCount = 100
            End If
            For i = 0 To mWS04AIOManager.eAnalogInput.Count - 1
                formatString = "0"
                If (mWS04AIOManager.AnalogInputDecimals(CType(i, mWS04AIOManager.eAnalogInput)) > 0) Then
                    formatString = formatString & "." & StrDup(mWS04AIOManager.AnalogInputDecimals(CType(i, mWS04AIOManager.eAnalogInput)), "0")
                End If
                sample(i) = 0
                For j = 0 To SampleCount - 1
                    sample(i) = sample(i) + mWS04_05AIOManager.SampleBuffer_WS04(i, j)
                Next
                sample(i) = sample(i) / SampleCount

                If (sample(i) < 0) And i <> mWS04AIOManager.eAnalogInput.WS04_EarlySensor And i <> mWS04AIOManager.eAnalogInput.WS04_StrenghtSensor Then
                    sample(i) = 0
                End If
                dgvWS04AnalogInputs.Item(1, i).Value = sample(i).ToString(formatString)
            Next
            '
            mWS04_05AIOManager.EmptyBuffer_WS04()
            '*******************************
            ' WS05 Analog input
            '*******************************
            ' Store the samples
            SampleCount = mWS04_05AIOManager.SampleCount_WS05 - 1
            If SampleCount > 100 Then
                SampleCount = 100
            End If
            For i = 0 To mWS05AIOManager.eAnalogInput.Count - 1
                formatString = "0"
                If (mWS05AIOManager.AnalogInputDecimals(CType(i, mWS05AIOManager.eAnalogInput)) > 0) Then
                    formatString = formatString & "." & StrDup(mWS05AIOManager.AnalogInputDecimals(CType(i, mWS05AIOManager.eAnalogInput)), "0")
                End If
                sample(i) = 0
                For j = 0 To SampleCount - 1
                    sample(i) = sample(i) + mWS04_05AIOManager.SampleBuffer_WS05(i, j)
                Next
                sample(i) = sample(i) / SampleCount

                If (sample(i) < 0) And i <> mWS05AIOManager.eAnalogInput.WS05_EarlySensor And i <> mWS05AIOManager.eAnalogInput.WS05_StrenghtSensor Then
                    sample(i) = 0
                End If
                dgvWS05AnalogInputs.Item(1, i).Value = sample(i).ToString(formatString)
            Next            '
            mWS04_05AIOManager.EmptyBuffer_WS05()
        End If

        ' Re-enable the timer
        tmrMonitor.Enabled = True

    End Sub

    '***************************************
    ' WS02
    '***************************************
    Private Sub btnWS02_DO__Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02_DO_0.Click, _
                                                                                                            btnWS02_DO_1.Click, _
                                                                                                            btnWS02_DO_2.Click, _
                                                                                                            btnWS02_DO_3.Click, _
                                                                                                            btnWS02_DO_4.Click, _
                                                                                                            btnWS02_DO_5.Click, _
                                                                                                            btnWS02_DO_6.Click, _
                                                                                                            btnWS02_DO_7.Click, _
                                                                                                            btnWS02_DO_8.Click, _
                                                                                                            btnWS02_DO_9.Click, _
                                                                                                            btnWS02_DO_10.Click, _
                                                                                                            btnWS02_DO_11.Click, _
                                                                                                            btnWS02_DO_12.Click, _
                                                                                                            btnWS02_DO_13.Click, _
                                                                                                            btnWS02_DO_14.Click, _
                                                                                                            btnWS02_DO_15.Click, _
                                                                                                            btnWS02_DO_16.Click, _
                                                                                                            btnWS02_DO_17.Click, _
                                                                                                            btnWS02_DO_18.Click, _
                                                                                                            btnWS02_DO_19.Click, _
                                                                                                            btnWS02_DO_20.Click, _
                                                                                                            btnWS02_DO_21.Click, _
                                                                                                            btnWS02_DO_22.Click, _
                                                                                                            btnWS02_DO_23.Click, _
                                                                                                            btnWS02_DO_24.Click, _
                                                                                                            btnWS02_DO_25.Click, _
                                                                                                            btnWS02_DO_26.Click, _
                                                                                                            btnWS02_DO_27.Click, _
                                                                                                            btnWS02_DO_28.Click, _
                                                                                                            btnWS02_DO_29.Click, _
                                                                                                            btnWS02_DO_30.Click, _
                                                                                                            btnWS02_DO_31.Click


        Dim button As Button
        Dim index As Integer

        button = CType(sender, Button)
        index = Array.IndexOf(_btnWS02_DO, button)
        If (mWS02DIOManager.WriteDigitalOutput(CType(index, mWS02DIOManager.eDigitalOutput), Not (mWS02DIOManager.DigitalOutputStatus(CType(index, mWS02DIOManager.eDigitalOutput))))) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while writing WS02 the digital outputs."
            frmMessage.ShowDialog()
        End If

    End Sub

    '***************************************
    ' WS03
    '***************************************
    Private Sub btnWS03_DO__Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03_DO_0.Click, _
                                                                                                            btnWS03_DO_1.Click, _
                                                                                                            btnWS03_DO_2.Click, _
                                                                                                            btnWS03_DO_3.Click, _
                                                                                                            btnWS03_DO_4.Click, _
                                                                                                            btnWS03_DO_5.Click, _
                                                                                                            btnWS03_DO_6.Click, _
                                                                                                            btnWS03_DO_7.Click, _
                                                                                                            btnWS03_DO_8.Click, _
                                                                                                            btnWS03_DO_9.Click, _
                                                                                                            btnWS03_DO_10.Click, _
                                                                                                            btnWS03_DO_11.Click, _
                                                                                                            btnWS03_DO_12.Click, _
                                                                                                            btnWS03_DO_13.Click, _
                                                                                                            btnWS03_DO_14.Click, _
                                                                                                            btnWS03_DO_15.Click


        Dim button As Button
        Dim index As Integer

        button = CType(sender, Button)
        index = Array.IndexOf(_btnWS03_DO, button)
        If (mWS03DIOManager.WriteDigitalOutput(CType(index, mWS03DIOManager.eDigitalOutput), Not (mWS03DIOManager.DigitalOutputStatus(CType(index, mWS03DIOManager.eDigitalOutput))))) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while writing WS03 the digital outputs."
            frmMessage.ShowDialog()
        End If

    End Sub


    '***************************************
    ' WS04
    '***************************************
    Private Sub btnWS04_DO__Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS04_DO_0.Click, _
                                                                                                            btnWS04_DO_1.Click, _
                                                                                                            btnWS04_DO_2.Click, _
                                                                                                            btnWS04_DO_3.Click, _
                                                                                                            btnWS04_DO_4.Click, _
                                                                                                            btnWS04_DO_5.Click, _
                                                                                                            btnWS04_DO_6.Click, _
                                                                                                            btnWS04_DO_7.Click, _
                                                                                                            btnWS04_DO_8.Click, _
                                                                                                            btnWS04_DO_9.Click, _
                                                                                                            btnWS04_DO_10.Click, _
                                                                                                            btnWS04_DO_11.Click, _
                                                                                                            btnWS04_DO_12.Click, _
                                                                                                            btnWS04_DO_13.Click, _
                                                                                                            btnWS04_DO_14.Click, _
                                                                                                            btnWS04_DO_15.Click


        Dim button As Button
        Dim index As Integer

        button = CType(sender, Button)
        index = Array.IndexOf(_btnWS04_DO, button)
        If (mWS04DIOManager.WriteDigitalOutput(CType(index, mWS04DIOManager.eDigitalOutput), Not (mWS04DIOManager.DigitalOutputStatus(CType(index, mWS04DIOManager.eDigitalOutput))))) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while writing WS04 the digital outputs."
            frmMessage.ShowDialog()
        End If

    End Sub

    '***************************************
    ' WS05
    '***************************************
    Private Sub btnWS05_DO__Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS05_DO_0.Click, _
                                                                                                            btnWS05_DO_1.Click, _
                                                                                                            btnWS05_DO_2.Click, _
                                                                                                            btnWS05_DO_3.Click, _
                                                                                                            btnWS05_DO_4.Click, _
                                                                                                            btnWS05_DO_5.Click, _
                                                                                                            btnWS05_DO_6.Click, _
                                                                                                            btnWS05_DO_7.Click, _
                                                                                                            btnWS05_DO_8.Click, _
                                                                                                            btnWS05_DO_9.Click, _
                                                                                                            btnWS05_DO_10.Click, _
                                                                                                            btnWS05_DO_11.Click, _
                                                                                                            btnWS05_DO_12.Click, _
                                                                                                            btnWS05_DO_13.Click, _
                                                                                                            btnWS05_DO_14.Click, _
                                                                                                            btnWS05_DO_15.Click

        Dim button As Button
        Dim index As Integer

        button = CType(sender, Button)
        index = Array.IndexOf(_btnWS05_DO, button)
        If (mWS05DIOManager.WriteDigitalOutput(CType(index, mWS05DIOManager.eDigitalOutput), Not (mWS05DIOManager.DigitalOutputStatus(CType(index, mWS05DIOManager.eDigitalOutput))))) Then
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error while writing WS05 the digital outputs."
            frmMessage.ShowDialog()
        End If

    End Sub


    Private Sub pbLogoValeo1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbLogoValeo.Click
        '
        'Stop analog task
        mWS04_05AIOManager.StopSampling()
        '
        mUserManager.Logout()
        '
        Me.Close()
    End Sub


    Private Sub tmrLoop_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrLoop.Tick
        '
        tmrLoop.Enabled = False
        '
        If LinWS02Connected Then
            mWS02Main.Loop_Maint()
        End If
        '
        If LinWS03Connected Then
            mWS03Main.Loop_Maint()
        End If
        ' Application events
        Application.DoEvents()
        '
        tmrLoop.Enabled = True

    End Sub

    '************************************************
    '   Work Station 02 Camera 
    '************************************************
    Private Sub btnLinWS02_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLinWS02.Click
        mWS02Main.LinMaintenance()
        LinWS02Connected = Not LinWS02Connected
      
    End Sub


    Private Sub btn_TTL_On_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TTL_On.Click

        mWS02Main.ManageSubPhaseMaint(1, 10, 0, 0)

    End Sub

    Private Sub btn_Bck_On_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Bck_On.Click
        mWS02Main.ManageSubPhaseMaint(1, 1, 0, 0)
    End Sub

    Private Sub btn_Bck_Off_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Bck_Off.Click
        mWS02Main.ManageSubPhaseMaint(1, 50, 0, 0)

    End Sub

    Private Sub btn_TTAll_Off_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mWS02Main.ManageSubPhaseMaint(1, 50, 0, 0)
    End Sub

    Private Sub btn_TTL_Off_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TTL_Off.Click
        mWS02Main.ManageSubPhaseMaint(1, 50, 0, 0)

    End Sub

    Private Sub btn_TTR_Off_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mWS02Main.ManageSubPhaseMaint(1, 50, 0, 0)

    End Sub

    Private Sub btn_TTC_Off_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mWS02Main.ManageSubPhaseMaint(1, 50, 0, 0)

    End Sub

    Private Sub btn_TTAll_On_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mWS02Main.ManageSubPhaseMaint(1, 20, 0, 0)

    End Sub

    Private Sub bntAll_On_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntAll_On.Click
        mWS02Main.ManageSubPhaseMaint(1, 40, 0, 0)

    End Sub

    Private Sub btnAll_Off_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAll_Off.Click
        mWS02Main.ManageSubPhaseMaint(1, 50, 0, 0)

    End Sub

    Private Sub txtWS02_Rx_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWS02_Rx.DoubleClick
        txtWS02_Rx.Text = ""
    End Sub

    Private Sub btnOpenDiag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenDiag.Click
        mWS02Main.ManageSubPhaseMaint(0, 0, 0, 0)

    End Sub

    '************************************************
    '   Work Station 03 Haptic 1 
    '************************************************
    Private Sub txtWS03_Tx_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWS03_Tx.TextChanged
        txtWS03_Rx.Text = txtWS03_Tx.Text & txtWS03_Rx.Text

    End Sub

    
    Private Sub btnWS03Lin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03Lin.Click
        mWS03Main.LinMaintenance()
        LinWS03Connected = Not LinWS03Connected

    End Sub

    Private Sub btnWS03Diag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03Diag.Click
        mWS03Main.ManageSubPhaseMaint(0, 0, 0, 0)

    End Sub

    Private Sub btnRead_Din_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRead_Din.Click
        mWS03Main.ManageSubPhaseMaint(1, 1, 0, 0)

    End Sub

    Private Sub btnRead_AIN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRead_AIN.Click
        mWS03Main.ManageSubPhaseMaint(1, 10, 0, 0)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        mWS03Main.ManageSubPhaseMaint(1, 199, 0, 0)
        mWS03Main.ManageSubPhaseMaint(0, 10, 0, 0)

    End Sub
End Class