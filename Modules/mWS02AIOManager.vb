﻿Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports NationalInstruments.DAQmx
Imports System
Imports System.IO
Imports System.Math
Imports System.Threading

Module mWS02AIOManager
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Analog input enumeration
    Public Enum eAnalogInput
        'Part x
        WS02_VBAT = 0
        WS02_IBAT = 1
        WS02_Pin1 = 2
        WS02_Pin2 = 3
        WS02_Pin5 = 4
        WS02_Pin6 = 5
        WS02_Pin7 = 6
        WS02_Pin8 = 7
        '
        WS02_Pin9 = 8
        WS02_Pin10 = 9
        WS02_Pin11 = 10
        WS02_Pin12 = 11
        WS02_Pin13 = 12
        WS02_Pin14 = 13
        WS02_Pin15 = 14
        WS02_Pin16 = 15
        '
        WS02_Pin17 = 16
        WS02_Pin18 = 17
        WS02_Pin19 = 18
        WS02_Pin20 = 19
        WS02_Pin21 = 20
        WS02_AI22 = 21
        WS02_Pin23 = 22
        WS02_PIN24 = 23
        '
        WS02_AI24 = 24
        WS02_AI25 = 25
        WS02_AI26 = 26
        WS02_AI27 = 27
        WS02_AI28 = 28
        WS02_ISleep = 29
        WS02_AI30 = 30
        WS02_ExtTemp = 31

        '
        Count = 32
    End Enum



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _hardwareEnabled = mConstants.HardwareEnabled_NI

    '
    Private Const _current1CoarseOffset = 0
    Private Const _current1FineOffsetMinimum = -10 * 0.05    ' -IFS * 5%
    Private Const _current1FineOffsetMaximum = 10 * 0.05     ' +IFS * 5%
    Private Const _current2CoarseOffset = 0
    Private Const _current2FineOffsetMinimum = -1 * 0.05    ' -IFS * 5%
    Private Const _current2FineOffsetMaximum = 1 * 0.05     ' +IFS * 5%
    ' ---

    Private Const _voltageCoarseOffset = 0
    Private Const _voltageFineOffsetMinimum = -15 * 0.05    ' -VFS * 5%
    Private Const _voltageFineOffsetMaximum = 15 * 0.05     ' +VFS * 5%

    ' 
    Private Const _current1CoarseGain = 2.5 / 10     ' 2.5 A/10 V
    Private Const _current1Decimals = 3
    Private Const _current2CoarseGain = 0.001 / 10      ' 1 mA/10 V
    Private Const _current2Decimals = 3
    ' ---



    Private Const _temperatureCoarseGain = 26.7 / 1.78        ' 26.7 ℃/1.78 V
    Private Const _temperatureDecimals = 2
    Private Const _forceCoarseGain = 10 / 10        ' 10 N/10 V
    Private Const _forceDecimals = 3
    Private Const _strokeCoarseGain = 20 / 10       ' 20 mm/10 V
    Private Const _strokeDecimals = 2
    Private Const _voltageCoarseGain = 15 / 10      ' 15 V/10 V
    Private Const _voltageDecimals = 2

    Private Const _fineGainDecimals = 3
    Private Const _minFineGain = 0.8
    Private Const _maxFineGain = 1.2

    Private Const _minCurrentOffset = -2
    Private Const _maxCurrentOffset = 2
    Private Const _minForceOffset = -2
    Private Const _maxForceOffset = 2
    Private Const _minStrokeOffset = -2
    Private Const _maxStrokeOffset = 2
    Private Const _minVoltageOffset = -0.2
    Private Const _maxVoltageOffset = 0.2

    Private Const _sampleBufferSize = 10000 ' 1000000

    ' Private variables
    Private _bufferOverflow As Boolean
    Private _callback As AsyncCallback
    Private _coarseGain(0 To eAnalogInput.Count - 1) As Double
    Private _coarseOffset(0 To eAnalogInput.Count - 1) As Double
    Private _decimals(0 To eAnalogInput.Count - 1) As Integer
    Private _emptyBufferFlag As Boolean
    Private _fineGain(0 To eAnalogInput.Count - 1) As Double
    Private _fineOffset(0 To eAnalogInput.Count - 1) As Double
    Private _firstTimeStamp As Date
    Private _lastTimeStamp As Date
    Private _lock As New Object
    Private _offset(0 To eAnalogInput.Count - 1) As Double
    Private _reader As AnalogMultiChannelReader
    Private _runningTask As Task
    Private _sampleBuffer(0 To eAnalogInput.Count - 1, 0 To _sampleBufferSize - 1) As Double
    Private _TimeStampSampleBuffer(0 To _sampleBufferSize - 1) As Double
    Private _sampleCount As Integer
    Private _samplingFrequency As Double
    Private _task As Task



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property AnalogInputDecimals(ByVal index As eAnalogInput) As Integer
        Get
            AnalogInputDecimals = _decimals(index)
        End Get
    End Property



    Public ReadOnly Property AnalogInputDescription(ByVal index As eAnalogInput) As String
        Get
            Select Case index
                Case eAnalogInput.WS02_VBAT
                    AnalogInputDescription = "BAT_TEMPO (Vbat) Pin #3"
                Case eAnalogInput.WS02_IBAT
                    AnalogInputDescription = "IKL31 (Ibat)"
                Case eAnalogInput.WS02_ISleep
                    AnalogInputDescription = "IKL31 (ISeelpbat)"
                Case eAnalogInput.WS02_Pin1
                    AnalogInputDescription = "O_JAMA_UP Pin #1"
                Case eAnalogInput.WS02_Pin2
                    AnalogInputDescription = "COMMUN_SUPPLY_+_OF_PTN_DRIVER_EXTERNAL_MIRROR Pin #2"
                Case eAnalogInput.WS02_Pin5
                    AnalogInputDescription = "O_UP_FRONT_PASSENGER_CDE Pin #5"
                Case eAnalogInput.WS02_Pin6
                    AnalogInputDescription = "CDE_H/B_RTRV_G Pin #6"
                Case eAnalogInput.WS02_Pin7
                    AnalogInputDescription = "SGN_COMMUN_MOT_RTRV_D Pin #7"
                Case eAnalogInput.WS02_Pin8
                    AnalogInputDescription = "CDE_H/B_RTRV_D Pin #8"
                Case eAnalogInput.WS02_Pin9
                    AnalogInputDescription = "CDE_D/G_RTRV_D Pin #9"
                Case eAnalogInput.WS02_Pin10
                    AnalogInputDescription = "CDE_D/G_RTRV_G Pin #10"
                Case eAnalogInput.WS02_Pin11
                    AnalogInputDescription = "O_DOWN_REAR_RIGHT_CDE Pin #11"
                Case eAnalogInput.WS02_Pin12
                    AnalogInputDescription = "O_LOCAL_WL_SWITCHES_INHIBITION_CDE Pin #12"
                Case eAnalogInput.WS02_Pin13
                    AnalogInputDescription = "O_JAMA_DOWN Pin #13"
                Case eAnalogInput.WS02_Pin14
                    AnalogInputDescription = "COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR Pin #14"
                Case eAnalogInput.WS02_Pin15
                    AnalogInputDescription = "O_DOWN_FRONT_PASSENGER_CDE Pin #15"
                Case eAnalogInput.WS02_Pin16
                    AnalogInputDescription = "O_UP_REAR_RIGHT_CDE Pin #16"
                Case eAnalogInput.WS02_Pin17
                    AnalogInputDescription = "CDE_-_RBT_RTRV_G Pin #17"
                Case eAnalogInput.WS02_Pin18
                    AnalogInputDescription = "CDE_+_RBT_RTRV_G Pin #18"
                Case eAnalogInput.WS02_Pin19
                    AnalogInputDescription = "CDE_+_RBT_RTRV_D Pin #19"
                Case eAnalogInput.WS02_Pin20
                    AnalogInputDescription = "CDE_-_RBT_RTRV_D Pin #20"
                Case eAnalogInput.WS02_Pin21
                    AnalogInputDescription = "SGN_COMMUN_MOT_RTRV_G Pin #21"
                Case eAnalogInput.WS02_Pin23
                    AnalogInputDescription = "O_DOWN_REAR_LEFT_CDE Pin #23"
                Case eAnalogInput.WS02_Pin24
                    AnalogInputDescription = "O_UP_REAR_LEFT_CDE Pin #24"
                Case Else
                    AnalogInputDescription = String.Format("Free {0}", index)
            End Select
        End Get
    End Property



    Public Property AnalogInputFineGain(ByVal index As eAnalogInput) As Double
        Get
            AnalogInputFineGain = _fineGain(index)
        End Get
        Set(ByVal value As Double)
            If (value < _minFineGain) Then
                _fineGain(index) = _minFineGain
            ElseIf (value > _maxFineGain) Then
                _fineGain(index) = _maxFineGain
            Else
                _fineGain(index) = Round(value, _fineGainDecimals)
            End If
        End Set
    End Property

    Public Property AnalogInputFineOffset(ByVal index As eAnalogInput) As Double
        Get
            AnalogInputFineOffset = _fineOffset(index)
        End Get
        Set(ByVal value As Double)
            '
            If (index = eAnalogInput.WS02_IBAT) Then
                If (_fineOffset(index) < _current1FineOffsetMinimum) Then
                    _fineOffset(index) = _current1FineOffsetMinimum
                ElseIf (_fineOffset(index) > _current1FineOffsetMaximum) Then
                    _fineOffset(index) = _current1FineOffsetMaximum
                Else
                    _fineOffset(index) = Round(value, _decimals(index))
                End If
            Else
                If (_fineOffset(index) < _voltageFineOffsetMinimum) Then
                    _fineOffset(index) = _voltageFineOffsetMinimum
                ElseIf (_fineOffset(index) > _voltageFineOffsetMaximum) Then
                    _fineOffset(index) = _voltageFineOffsetMaximum
                Else
                    _fineOffset(index) = Round(value, _decimals(index))
                End If
            End If
        End Set
    End Property


    Public ReadOnly Property AnalogInputUnits(ByVal index As eAnalogInput) As String
        Get
            If (index = eAnalogInput.WS02_IBAT) Then
                AnalogInputUnits = "mA"
            ElseIf (index = eAnalogInput.WS02_ISleep) Then
                AnalogInputUnits = "mA"
            Else
                AnalogInputUnits = "V"
            End If
        End Get
    End Property



    Public ReadOnly Property BufferOverflow As Boolean
        Get
            BufferOverflow = _bufferOverflow
        End Get
    End Property



    Public ReadOnly Property SampleBuffer As Double(,)
        Get
            SampleBuffer = _sampleBuffer
        End Get
    End Property

    Public ReadOnly Property TimeStampSampleBuffer As Double()
        Get
            TimeStampSampleBuffer = _TimeStampSampleBuffer
        End Get
    End Property


    Public ReadOnly Property SampleCount As Integer
        Get
            SampleCount = _sampleCount
        End Get
    End Property


    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Sub EmptyBuffer()
        ' Empty the buffer of the specified board
        _sampleCount = 0
    End Sub

    Public Function LoadCalibration(ByVal path As String) As Boolean
        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim token(0 To 3) As String

        ' Initialize the coarse gains and the decimals
        For i = 0 To eAnalogInput.Count - 1
            '
            If (i = eAnalogInput.WS02_IBAT) Then
                _coarseGain(i) = _current1CoarseGain
                _decimals(i) = _current1Decimals
            ElseIf (i = eAnalogInput.WS02_ExtTemp) Then
                _coarseGain(i) = _temperatureCoarseGain
                _decimals(i) = _temperatureDecimals
            Else
                _coarseGain(i) = _voltageCoarseGain
                _decimals(i) = _voltageDecimals
            End If
            _fineGain(i) = 1
            _offset(i) = 0
        Next
        ' Load the fine gains and offsets from file
        Try
            ' Open the file
            file = New StreamReader(path, System.Text.Encoding.Default)
            ' Read the heading
            line = file.ReadLine
            line = file.ReadLine
            line = file.ReadLine
            ' For all the analog inputs
            For i = 0 To eAnalogInput.Count - 1
                ' Read the calibration coefficients
                line = file.ReadLine
                token = Split(line, vbTab)
                AnalogInputFineGain(CType(i, eAnalogInput)) = CDbl(token(1))
                AnalogInputFineOffset(CType(i, eAnalogInput)) = CDbl(token(2))
            Next
            ' Return False
            LoadCalibration = False

        Catch ex As Exception
            ' Return True
            LoadCalibration = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function



    Public Function PowerDown() As Boolean
        ' Destroy the task
        PowerDown = DestroyTask()
    End Function



    Public Function PowerUp() As Boolean
        ' Load the calibration coefficients
        PowerUp = LoadCalibration(mWS02Main.Settings.AIOCalibrationPath)
        ' Create the task
        If _hardwareEnabled Then
            PowerUp = PowerUp Or CreateTask()
        End If
    End Function
    Private _pwm0 As Int16
    Private _pwm1 As Int16
    Public Function PowerUpforPWM(ByVal Channels As String, ByVal pwm0 As Int16, ByVal pwm1 As Int16) As Boolean
        ' Load the calibration coefficients
        PowerUpforPWM = LoadCalibration(mWS02Main.Settings.AIOCalibrationPath)
        _pwm0 = pwm0
        _pwm1 = pwm1
        ' Create the task
        If _hardwareEnabled Then
            PowerUpforPWM = PowerUpforPWM Or CreatePWMTask(Channels)
        End If
    End Function



    Public Function SaveCalibration(ByVal path As String) As Boolean
        Dim file As StreamWriter = Nothing
        Dim i As Integer

        Try
            ' Open the file
            file = New StreamWriter(path)
            ' Read the heading
            file.WriteLine("STATION FW ANALOG INPUT CALIBRATION COEFFICIENTS")
            file.WriteLine("")
            file.WriteLine("ANALOG INPUT DESCRIPTION" &
                           vbTab & "FINE GAIN" &
                           vbTab & "OFFSET" &
                           vbTab & "UNITS")
            ' For all the analog inputs
            For i = 0 To eAnalogInput.Count - 1
                ' Write the calibration coefficients
                file.WriteLine(AnalogInputDescription(CType(i, eAnalogInput)) &
                               vbTab & _fineGain(i).ToString(mUtility.FormatString(_fineGainDecimals)) &
                               vbTab & _fineOffset(i).ToString(mUtility.FormatString(_decimals(i))) &
                               vbTab & AnalogInputUnits(CType(i, eAnalogInput)))
            Next
            ' Return False
            SaveCalibration = False

        Catch ex As Exception
            ' Return True
            SaveCalibration = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function



    Public Function SingleSample(ByVal sample() As Double) As Boolean
        Dim frequency As Integer
        Dim sampleBuffer() As AnalogWaveform(Of Double)
        Dim sampleCount As Integer

        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            Try
                ' Initialize the sampling parameters
                frequency = 5000
                sampleCount = CInt(0.02 * frequency)
                ' Configure the sample clock
                _task.Timing.ConfigureSampleClock("", frequency, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, sampleCount)
                ' Read the samples
                sampleBuffer = _reader.ReadWaveform(sampleCount)
                ' Average the samples
                For i = 0 To eAnalogInput.Count - 1
                    sample(i) = 0
                    For j = 0 To sampleCount - 1
                        sample(i) = sample(i) + sampleBuffer(i).Samples(j).Value
                    Next
                    sample(i) = sample(i) / sampleCount
                    sample(i) = Round((sample(i) * _coarseGain(i) + _coarseOffset(i)) * _fineGain(i), _decimals(i))
                    If (sample(i) < 0) Then
                        sample(i) = 0
                    End If
                Next
                ' Return False
                SingleSample = False

            Catch ex As Exception
                ' Return True
                SingleSample = True
            End Try
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            SingleSample = False
        End If

    End Function
    Public Function SingleSamplePWM(ByVal sample() As Double) As Boolean
        Dim frequency As Integer
        Dim sampleBuffer() As AnalogWaveform(Of Double)
        Dim sampleCount As Integer

        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            Try
                ' Initialize the sampling parameters
                frequency = 5000
                sampleCount = CInt(0.02 * frequency)
                ' Configure the sample clock
                _task.Timing.ConfigureSampleClock("", frequency, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, sampleCount)
                ' Read the samples
                sampleBuffer = _reader.ReadWaveform(sampleCount)
                ' Average the samples
                '_pwm0
                sample(_pwm0) = 0
                For j = 0 To sampleCount - 1
                    sample(_pwm0) = sample(_pwm0) + sampleBuffer(0).Samples(j).Value
                Next
                sample(_pwm0) = sample(_pwm0) / sampleCount
                sample(_pwm0) = Round((sample(_pwm0) * _coarseGain(_pwm0) + _coarseOffset(_pwm0)) * _fineGain(_pwm0), _decimals(_pwm0))
                If (sample(_pwm0) < 0) Then
                    sample(_pwm0) = 0
                End If

                sample(_pwm1) = 0
                For j = 0 To sampleCount - 1
                    sample(_pwm1) = sample(_pwm1) + sampleBuffer(1).Samples(j).Value
                Next
                sample(_pwm1) = sample(_pwm1) / sampleCount
                sample(_pwm1) = Round((sample(_pwm1) * _coarseGain(_pwm1) + _coarseOffset(_pwm1)) * _fineGain(_pwm1), _decimals(_pwm1))
                If (sample(_pwm1) < 0) Then
                    sample(_pwm1) = 0
                End If
                ' Return False
                SingleSamplePWM = False

            Catch ex As Exception
                ' Return True
                SingleSamplePWM = True
            End Try
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            SingleSamplePWM = False
        End If

    End Function



    Public Function StartSampling(ByVal frequency As Double) As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' If the task is not already running
            If (_runningTask Is Nothing) Then
                Try
                    ' Configurate the sampling clock
                    _task.Timing.ConfigureSampleClock("", frequency, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, _sampleBufferSize)
                    ' Start sampling
                    _runningTask = _task
                    _reader.BeginReadWaveform(-1, _callback, _task)
                    ' Store the sampling frequency
                    _samplingFrequency = frequency
                    ' Return False
                    StartSampling = False

                Catch ex As Exception ' If some unexpected error occoured
                    StopSampling()
                    ' Return True
                    StartSampling = True
                End Try
            Else    ' Otherwise, if the task is already running
                ' Return False
                StartSampling = False
            End If
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            StartSampling = False
        End If
    End Function



    Public Function StopSampling() As Boolean
        ' Destroy the task
        StopSampling = DestroyTask()
        ' Clear the sampling frequency
        _samplingFrequency = 0
        ' Re-create the task
        StopSampling = StopSampling Or CreateTask()
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Function CreateTask() As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' Enable the error handling
            Try
                ' Create the task
                _task = New Task
                ' Create the analog input channel
                _task.AIChannels.CreateVoltageChannel(mWS02Main.Settings.AIOInterface & "/ai0:31",
                                                      "",
                                                      AITerminalConfiguration.Nrse,
                                                      0,
                                                      10,
                                                      AIVoltageUnits.Volts)
                ' Verify the task
                _task.Control(TaskAction.Verify)
                ' Create the reader
                _reader = New AnalogMultiChannelReader(_task.Stream)
                _reader.SynchronizeCallbacks = True
                ' Set the callback
                _callback = New AsyncCallback(AddressOf SamplingCallback)
                ' Return False
                CreateTask = False

            Catch ex As Exception   ' If some unexpected error occoured
                ' Return True
                CreateTask = True
            End Try
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            CreateTask = False
        End If

    End Function
    Private Function CreatePWMTask(ByVal Channels As String) As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' Enable the error handling
            Try
                ' Create the task
                _task = New Task
                ' Create the analog input channel
                _task.AIChannels.CreateVoltageChannel(Channels,
                                                      "",
                                                      AITerminalConfiguration.Nrse,
                                                      0,
                                                      10,
                                                      AIVoltageUnits.Volts)
                ' Verify the task
                _task.Control(TaskAction.Verify)
                ' Create the reader
                _reader = New AnalogMultiChannelReader(_task.Stream)
                _reader.SynchronizeCallbacks = True
                ' Set the callback
                _callback = New AsyncCallback(AddressOf SamplingCallbackPWM)
                ' Return False
                CreatePWMTask = False

            Catch ex As Exception   ' If some unexpected error occoured
                ' Return True
                CreatePWMTask = True
            End Try
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            CreatePWMTask = False
        End If

    End Function



    Private Function DestroyTask() As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' Enable the error handling
            Try
                ' Clear the running task
                _runningTask = Nothing
                ' Dispose and clear the task
                _task.Dispose()
                _task = Nothing
                ' Return False
                DestroyTask = False
            Catch ex As Exception   ' If some unexpected error occoured
                ' Return True
                DestroyTask = True
            End Try
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            DestroyTask = False
        End If
    End Function



    Private Sub SamplingCallback(ByVal ar As IAsyncResult)
        Dim i As Integer
        Dim j As Integer
        Dim localTask As Task
        Dim waveform As AnalogWaveform(Of Double)()

        ' Convert the asynchronous state to a task
        localTask = CType(ar.AsyncState, Task)
        ' Enable the error handling
        Try
            If (_runningTask IsNot Nothing AndAlso _runningTask Is localTask) Then
                waveform = _reader.EndReadWaveform(ar)
                ' Store the samples in the buffer
                For i = 0 To eAnalogInput.Count - 1
                    For j = 0 To waveform(i).SampleCount - 1
                        If (_sampleCount + j < _sampleBufferSize) Then
                            _sampleBuffer(i, _sampleCount + j) = Round((waveform(i).Samples(j).Value * _coarseGain(i) + _coarseOffset(i)) * _fineGain(i), _decimals(i))
                            If i = 0 Then
                                _TimeStampSampleBuffer(_sampleCount + j) = CDbl(waveform(i).Samples(j).TimeStamp.ToOADate())
                            End If
                        Else
                            _bufferOverflow = True
                            Exit For
                        End If
                    Next
                Next
                ' Update the sample count
                _sampleCount = _sampleCount + waveform(0).SampleCount
                ' Re-start reading the analog inputs
                _reader.BeginMemoryOptimizedReadWaveform(-1, _callback, _task, waveform)
            End If

        Catch ex As Exception   ' If some unexpected error occoured
            ' Stop sampling
            StopSampling()
        End Try
    End Sub
    Private Sub SamplingCallbackPWM(ByVal ar As IAsyncResult)
        Dim j As Integer
        Dim localTask As Task
        Dim waveform As AnalogWaveform(Of Double)()

        ' Convert the asynchronous state to a task
        localTask = CType(ar.AsyncState, Task)
        ' Enable the error handling
        Try
            If (_runningTask IsNot Nothing AndAlso _runningTask Is localTask) Then
                waveform = _reader.EndReadWaveform(ar)
                ' Store the samples in the buffer
                For j = 0 To waveform(0).SampleCount - 1
                    If (_sampleCount + j < _sampleBufferSize) Then
                        _sampleBuffer(_pwm0, _sampleCount + j) = Round((waveform(0).Samples(j).Value * _coarseGain(_pwm0) + _coarseOffset(_pwm0)) * _fineGain(_pwm0), _decimals(_pwm0))
                        'If i = 0 Then
                        _TimeStampSampleBuffer(_sampleCount + j) = CDbl(waveform(0).Samples(j).TimeStamp.ToOADate())
                        'End If
                    Else
                        _bufferOverflow = True
                        Exit For
                    End If
                Next
                For j = 0 To waveform(1).SampleCount - 1
                    If (_sampleCount + j < _sampleBufferSize) Then
                        _sampleBuffer(_pwm1, _sampleCount + j) = Round((waveform(1).Samples(j).Value * _coarseGain(_pwm1) + _coarseOffset(_pwm1)) * _fineGain(_pwm1), _decimals(_pwm1))
                        'If i = 0 Then
                        '_TimeStampSampleBuffer(_sampleCount + j) = CDbl(waveform(1).Samples(j).TimeStamp.ToOADate())
                        'End If
                    Else
                        _bufferOverflow = True
                        Exit For
                    End If
                Next
                ' Update the sample count
                _sampleCount = _sampleCount + waveform(0).SampleCount
                ' Re-start reading the analog inputs
                _reader.BeginMemoryOptimizedReadWaveform(-1, _callback, _task, waveform)
            End If

        Catch ex As Exception   ' If some unexpected error occoured
            ' Stop sampling
            StopSampling()
        End Try
    End Sub
End Module
