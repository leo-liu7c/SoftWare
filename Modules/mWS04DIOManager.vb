Option Explicit On
Option Strict On

Imports NationalInstruments.DAQmx

Module mWS04DIOManager
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Digital output enumeration
    Public Enum eDigitalOutput
        DO_OnPowerSupply = 0
        DO_LocalSensing = 1
        DO_RemoteSensing = 2
        DO_Reserve_3 = 3
        DO_Reserve_4 = 4
        DO_Reserve_5 = 5
        DO_Reserve_6 = 6
        DO_Lin_Bus = 7

        DO_Reserve_8 = 8
        DO_Reserve_9 = 9
        DO_Reserve_10 = 10
        DO_Reserve_11 = 11
        DO_Reserve_12 = 12
        DO_Reserve_13 = 13
        DO_Reserve_14 = 14
        DO_Reserve_15 = 15

        Count = 16


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
    Private _ClockTask As Task = Nothing



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property DigitalOutputDescription(ByVal index As eDigitalOutput) As String
        Get
            Select Case index
                Case eDigitalOutput.DO_OnPowerSupply
                    DigitalOutputDescription = "+ VBAT"
                Case eDigitalOutput.DO_LocalSensing
                    DigitalOutputDescription = "DO Local Sensing"
                Case eDigitalOutput.DO_RemoteSensing
                    DigitalOutputDescription = "DO Remote Sensing"
                Case eDigitalOutput.DO_Lin_Bus
                    DigitalOutputDescription = "DO Lin Bus"
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
                _writerTask.DOChannels.CreateChannel(mWS04Main.Settings.AIOInterface & "/port0/line0:7," & _
                                                     mWS04Main.Settings.AIOInterface & "/port1/line0:7", "", _
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
