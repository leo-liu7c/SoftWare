Option Explicit On
Option Strict On

Imports NationalInstruments.DAQmx

Module mWS02DIOManager
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Digital output enumeration
    Public Enum eDigitalOutput
        'PS
        DO_OnPowerSupply1 = 0
        DO_OnPowerSupply2 = 1
        DO_ISleep = 2
        DO_LocalSensing = 3
        DO_RemoteSensing = 4
        DO_PWL = 5
        DO_PowerR = 6
        DO_LinConnect = 7
        ''' <summary>
        ''' EV
        ''' </summary>
        DO_R_Pin_12_GND = 8
        ''' <summary>
        ''' JAMA, Move to WS03. not used here.
        ''' </summary>
        DO_R_Pin_13 = 9
        'Mirror
        DO_R_Pin_6 = 10
        DO_C_Pin_6_10 = 11
        DO_R_Pin_10 = 12
        'PWL Load
        DO_R_Pin_5 = 13
        DO_R_Pin_15 = 14
        DO_R_Pin_12 = 15
        DO_R_Pin_11 = 16
        DO_R_Pin_16 = 17
        DO_R_Pin_23 = 18
        DO_R_Pin_24 = 19
        'Memo
        DO_R_Pin_2 = 20
        DO_C_Pin_8_2 = 21
        DO_R_Pin_8_2 = 22
        DO_R_Pin_14 = 23
        'No Memo
        DO_R_Pin_9 = 24
        DO_C_Pin_9_8 = 25
        DO_R_Pin_8 = 26
        'Folding
        DO_R_Pin_17 = 27
        DO_R_Pin_18 = 28
        DO_R_Pin_19 = 29
        DO_R_Pin_20 = 30
        ''' <summary>
        ''' EV
        ''' </summary>
        DO_R_Pin_24_GND = 31

        Count = 32

    End Enum




    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _hardwareEnabled = mConstants.HardwareEnabled_NI

    ' Private variables
    Private _digitalOutputStatus(0 To eDigitalOutput.Count - 1) As Boolean
    Private _writer As DigitalSingleChannelWriter = Nothing
    Private _writerTask As Task = Nothing



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property DigitalOutputDescription(ByVal index As eDigitalOutput) As String
        Get
            Select Case index
                Case eDigitalOutput.DO_OnPowerSupply1
                    DigitalOutputDescription = "+ VBAT ECU"
                Case eDigitalOutput.DO_LocalSensing
                    DigitalOutputDescription = "DO Local Sensing"
                Case eDigitalOutput.DO_RemoteSensing
                    DigitalOutputDescription = "DO Remote Sensing"
                Case eDigitalOutput.DO_ISleep
                    DigitalOutputDescription = "I-Sleep Mode"
                Case eDigitalOutput.DO_LinConnect
                    DigitalOutputDescription = "DO_LinConnect"
                Case eDigitalOutput.DO_R_Pin_12_GND,
                     eDigitalOutput.DO_R_Pin_24_GND,
                     eDigitalOutput.DO_R_Pin_13,
                     eDigitalOutput.DO_R_Pin_6,
                     eDigitalOutput.DO_C_Pin_6_10,
                     eDigitalOutput.DO_R_Pin_10,
                     eDigitalOutput.DO_R_Pin_5,
                     eDigitalOutput.DO_R_Pin_15,
                     eDigitalOutput.DO_R_Pin_12,
                     eDigitalOutput.DO_R_Pin_11,
                     eDigitalOutput.DO_R_Pin_16,
                     eDigitalOutput.DO_R_Pin_23,
                     eDigitalOutput.DO_R_Pin_24,
                     eDigitalOutput.DO_R_Pin_14,
                     eDigitalOutput.DO_C_Pin_8_2,
                     eDigitalOutput.DO_R_Pin_8_2,
                     eDigitalOutput.DO_R_Pin_2,
                     eDigitalOutput.DO_R_Pin_9,
                     eDigitalOutput.DO_C_Pin_9_8,
                     eDigitalOutput.DO_R_Pin_8
                    DigitalOutputDescription = index.ToString()
                Case eDigitalOutput.DO_PowerR
                    DigitalOutputDescription = "DO_Power_Load"
                Case eDigitalOutput.DO_PWL
                    DigitalOutputDescription = "DO_PWL_Option"

                Case Else
                    DigitalOutputDescription = String.Format("Value {0} Reserve", CInt(index))
            End Select

        End Get
    End Property



    Public ReadOnly Property DigitalOutputStatus(ByVal index As eDigitalOutput) As Boolean
        Get
            DigitalOutputStatus = _digitalOutputStatus(index)
        End Get
    End Property



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function PowerDown() As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' Clear all the digital outputs
            For i = 0 To eDigitalOutput.Count - 2
                _digitalOutputStatus(i) = False
            Next
            PowerDown = ResetDigitalOutput(CType(eDigitalOutput.Count - 1, eDigitalOutput))
            ' Stop and clear the writer task
            Try
                _writer = Nothing
                _writerTask.Stop()
                _writerTask.Dispose()
                _writerTask = Nothing

            Catch ex As Exception
                ' Return True
                PowerDown = True
            End Try
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            PowerDown = False
        End If
    End Function



    Public Function PowerUp() As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            Try
                ' Create the writer task
                _writerTask = New Task()
                ' Create the digital output channel
                _writerTask.DOChannels.CreateChannel(mWS02Main.Settings.AIOInterface & "/port0/line0:31,", "", _
                                                     ChannelLineGrouping.OneChannelForAllLines)
                ' Create the writer
                _writer = New DigitalSingleChannelWriter(_writerTask.Stream)
                ' Start the task
                _writerTask.Start()
                ' Return False
                PowerUp = False

            Catch ex As Exception
                ' Return True
                PowerUp = True
            End Try
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            PowerUp = False
        End If
    End Function



    Public Function ResetDigitalOutput(ByVal index As eDigitalOutput) As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' Reset the specified digital output
            ResetDigitalOutput = WriteDigitalOutput(index, False)
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            ResetDigitalOutput = False
        End If
    End Function



    Public Function SetDigitalOutput(ByVal index As eDigitalOutput) As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' Set the specified digital output
            SetDigitalOutput = WriteDigitalOutput(index, True)
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            SetDigitalOutput = False
        End If
    End Function


    Public Function WriteDigitalOutput(ByVal index As eDigitalOutput, _
                                       ByVal status As Boolean) As Boolean
        ' Update the digital output status
        _digitalOutputStatus(index) = status
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            Try
                ' Write the digital outputs
                _writer.WriteSingleSampleMultiLine(False, _digitalOutputStatus)
                ' Return False
                WriteDigitalOutput = False

            Catch ex As Exception
                ' Return True
                WriteDigitalOutput = True
            End Try
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            WriteDigitalOutput = False
        End If
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module
