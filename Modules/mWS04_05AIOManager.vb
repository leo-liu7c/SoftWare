Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports NationalInstruments.DAQmx
Imports System
Imports System.IO
Imports System.Math
Imports System.Threading

Module mWS04_05AIOManager
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Analog input enumeration
    Public Enum eAnalogInput
        'Part x
        WS04_VBAT = 0
        WS04_IBAT = 1
        WS04_Pin1 = 2
        WS04_Pin2 = 3
        WS04_Pin5 = 4
        WS04_Pin6 = 5
        WS04_Pin7 = 6
        WS04_Pin8 = 7
        '
        WS04_Pin9 = 8
        WS04_Pin10 = 9
        WS04_Pin11 = 10
        WS04_Pin12 = 11
        WS04_Pin13 = 12
        WS04_Pin14 = 13
        WS04_StrenghtSensor = 14
        WS04_EarlySensor = 15
        '
        WS05_VBAT = 16
        WS05_IBAT = 17
        WS05_Pin1 = 18
        WS05_Pin2 = 19
        WS05_Pin5 = 20
        WS05_Pin6 = 21
        WS05_Pin7 = 22
        WS05_Pin8 = 23
        '
        WS05_Pin9 = 24
        WS05_Pin10 = 25
        WS05_Pin11 = 26
        WS05_Pin12 = 27
        WS05_Pin13 = 28
        WS05_Pin14 = 29
        WS05_StrenghtSensor = 30
        WS05_EarlySensor = 31

        Count = 32
    End Enum

    Public Sample_Task_Started As Boolean

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
    Private Const _current1CoarseGain = 0.25 / 10     ' 10 A/10 V
    Private Const _current1Decimals = 3
    Private Const _current2CoarseGain = 0.001 / 10      ' 1 mA/10 V
    Private Const _current2Decimals = 3
    ' ---

    Private Const _forceCoarseGain_WS04 = (20.0183 * 1.5) / 10
    Private Const _forceCoarseGain_WS05 = (10.00915 * 1.5) / 10 '20.0183, reduce half range at 20200716.YAN.Qian. for WS05 there is no need.
    Private Const _forceDecimals = 3
    Private Const _strokeCoarseGain_WS04 = 180 / 10       ' 20 mm/10 V
    Private Const _strokeCoarseGain_WS05 = 180 / 10       ' 20 mm/10 V
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

    Private Const _sampleBufferSize = 20000 '1000000

    ' Private variables
    Private _bufferOverflow As Boolean
    Private _emptyBufferFlag As Boolean
    Private _emptyBufferFlag_WS04 As Boolean
    Private _emptyBufferFlag_WS05 As Boolean
    Private _callback As AsyncCallback
    Private _coarseGain(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To 1) As Double
    Private _coarseOffset(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To 1) As Double
    Private _decimals(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To 1) As Integer
    Private _fineGain(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To 1) As Double
    Private _fineOffset(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To 1) As Double
    Private _offset(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To 1) As Double

    Private _firstTimeStamp As Date
    Private _lastTimeStamp As Date
    Private _lock As New Object
    Private _reader As AnalogMultiChannelReader
    Private _runningTask As Task
    Private _sampleBuffer(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To _sampleBufferSize - 1) As Double
    Private _TimeStampSampleBuffer(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To _sampleBufferSize - 1) As Date
    Private _TimeStampSampleBuffer_WS04 As Date
    Private _TimeStampSampleBuffer_WS05 As Date
    Private _sampleCount As Integer
    Private _sampleCount_WS04 As Integer
    Private _sampleCount_WS05 As Integer
    Private _samplingFrequency As Double
    Private _task As Task

    Private _sampleBuffer_WS04(0 To CInt((eAnalogInput.Count / 2) - 1), 0 To _sampleBufferSize - 1) As Double
    Private _sampleBuffer_WS05(0 To CInt(eAnalogInput.Count / 2) - 1, 0 To _sampleBufferSize - 1) As Double



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+

    Public ReadOnly Property AnalogInputDescription(ByVal index As eAnalogInput) As String
        Get
            Select Case index
                Case eAnalogInput.WS04_VBAT
                    AnalogInputDescription = "BAT_TEMPO (Vbat) Pin #3"
                Case eAnalogInput.WS04_IBAT
                    AnalogInputDescription = "IKL31 (Ibat)"
                Case eAnalogInput.WS04_Pin1
                    AnalogInputDescription = "O_JAMA_UP Pin #1"
                Case eAnalogInput.WS04_Pin2
                    AnalogInputDescription = "COMMUN_SUPPLY_+_OF_PTN_DRIVER_EXTERNAL_MIRROR Pin #2"
                Case eAnalogInput.WS04_Pin5
                    AnalogInputDescription = "O_UP_FRONT_PASSENGER_CDE Pin #5"
                Case eAnalogInput.WS04_Pin6
                    AnalogInputDescription = "CDE_H/B_RTRV_G Pin #6"
                Case eAnalogInput.WS04_Pin7
                    AnalogInputDescription = "SGN_COMMUN_MOT_RTRV_D Pin #7"
                Case eAnalogInput.WS04_Pin8
                    AnalogInputDescription = "CDE_H/B_RTRV_D Pin #8"
                Case eAnalogInput.WS04_Pin9
                    AnalogInputDescription = "CDE_D/G_RTRV_D Pin #9"
                Case eAnalogInput.WS04_Pin10
                    AnalogInputDescription = "CDE_D/G_RTRV_G Pin #10"
                Case eAnalogInput.WS04_Pin11
                    AnalogInputDescription = "O_DOWN_REAR_RIGHT_CDE Pin #11"
                Case eAnalogInput.WS04_Pin12
                    AnalogInputDescription = "O_LOCAL_WL_SWITCHES_INHIBITION_CDE Pin #12"
                Case eAnalogInput.WS04_Pin13
                    AnalogInputDescription = "O_JAMA_DOWN Pin #13"
                Case eAnalogInput.WS04_Pin14
                    AnalogInputDescription = "OMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR Pin #14"
                Case eAnalogInput.WS04_EarlySensor
                    AnalogInputDescription = "Early Sensor"
                Case eAnalogInput.WS04_StrenghtSensor
                    AnalogInputDescription = "Strengh Sensor"


                Case eAnalogInput.WS05_VBAT
                    AnalogInputDescription = "BAT_TEMPO (Vbat) Pin #3"
                Case eAnalogInput.WS05_IBAT
                    AnalogInputDescription = "IKL31 (Ibat)"
                Case eAnalogInput.WS05_Pin1
                    AnalogInputDescription = "O_JAMA_UP Pin #1"
                Case eAnalogInput.WS05_Pin2
                    AnalogInputDescription = "COMMUN_SUPPLY_+_OF_PTN_DRIVER_EXTERNAL_MIRROR Pin #2"
                Case eAnalogInput.WS05_Pin5
                    AnalogInputDescription = "O_UP_FRONT_PASSENGER_CDE Pin #5"
                Case eAnalogInput.WS05_Pin6
                    AnalogInputDescription = "CDE_H/B_RTRV_G Pin #6"
                Case eAnalogInput.WS05_Pin7
                    AnalogInputDescription = "SGN_COMMUN_MOT_RTRV_D Pin #7"
                Case eAnalogInput.WS05_Pin8
                    AnalogInputDescription = "CDE_H/B_RTRV_D Pin #8"
                Case eAnalogInput.WS05_Pin9
                    AnalogInputDescription = "CDE_D/G_RTRV_D Pin #9"
                Case eAnalogInput.WS05_Pin10
                    AnalogInputDescription = "CDE_D/G_RTRV_G Pin #10"
                Case eAnalogInput.WS05_Pin11
                    AnalogInputDescription = "O_DOWN_REAR_RIGHT_CDE Pin #11"
                Case eAnalogInput.WS05_Pin12
                    AnalogInputDescription = "O_LOCAL_WL_SWITCHES_INHIBITION_CDE Pin #12"
                Case eAnalogInput.WS05_Pin13
                    AnalogInputDescription = "O_JAMA_DOWN Pin #13"
                Case eAnalogInput.WS05_Pin14
                    AnalogInputDescription = "OMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR Pin #14"
                Case eAnalogInput.WS05_EarlySensor
                    AnalogInputDescription = "Early Sensor"
                Case eAnalogInput.WS05_StrenghtSensor
                    AnalogInputDescription = "Strengh Sensor"

                Case Else
                    AnalogInputDescription = String.Format("Free {0}", index)
            End Select
        End Get
    End Property

    Public ReadOnly Property AnalogInputDecimals(ByVal index As eAnalogInput, ByVal WS_Index As Integer) As Integer
        Get
            AnalogInputDecimals = _decimals(index, WS_Index)
        End Get
    End Property


    Public Property AnalogInputFineGain(ByVal index As eAnalogInput, ByVal WS_Index As Integer) As Double
        Get
            AnalogInputFineGain = _fineGain(index, WS_Index)
        End Get
        Set(ByVal value As Double)
            If (value < _minFineGain) Then
                _fineGain(index, WS_Index) = _minFineGain
            ElseIf (value > _maxFineGain) Then
                _fineGain(index, WS_Index) = _maxFineGain
            Else
                _fineGain(index, WS_Index) = Round(value, _fineGainDecimals)
            End If
        End Set
    End Property

    Public Property AnalogInputFineOffset(ByVal index As eAnalogInput, ByVal WS_Index As Integer) As Double
        Get
            AnalogInputFineOffset = _fineOffset(index, WS_Index)
        End Get
        Set(ByVal value As Double)
            '
            If (index = eAnalogInput.WS05_IBAT) Then
                If (_fineOffset(index, WS_Index) < _current1FineOffsetMinimum) Then
                    _fineOffset(index, WS_Index) = _current1FineOffsetMinimum
                ElseIf (_fineOffset(index, WS_Index) > _current1FineOffsetMaximum) Then
                    _fineOffset(index, WS_Index) = _current1FineOffsetMaximum
                Else
                    _fineOffset(index, WS_Index) = Round(value, _decimals(index, WS_Index))
                End If
            Else
                If (_fineOffset(index, WS_Index) < _voltageFineOffsetMinimum) Then
                    _fineOffset(index, WS_Index) = _voltageFineOffsetMinimum
                ElseIf (_fineOffset(index, WS_Index) > _voltageFineOffsetMaximum) Then
                    _fineOffset(index, WS_Index) = _voltageFineOffsetMaximum
                Else
                    _fineOffset(index, WS_Index) = Round(value, _decimals(index, WS_Index))
                End If
            End If
        End Set
    End Property


    Public ReadOnly Property AnalogInputUnits(ByVal index As eAnalogInput) As String
        Get
            If index = eAnalogInput.WS05_IBAT Or index = eAnalogInput.WS04_IBAT Then
                AnalogInputUnits = "A"
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


    'Public ReadOnly Property SampleBuffer As Double(,)
    '    Get
    '        SampleBuffer = _sampleBuffer
    '    End Get
    'End Property


    Public ReadOnly Property SampleBuffer_WS05 As Double(,)
        Get
            SampleBuffer_WS05 = _sampleBuffer_WS05
        End Get
    End Property


    Public ReadOnly Property SampleBuffer_WS04 As Double(,)
        Get
            SampleBuffer_WS04 = _sampleBuffer_WS04
        End Get
    End Property

    Public ReadOnly Property SampleCount As Integer
        Get
            SampleCount = _sampleCount
        End Get
    End Property

    Public ReadOnly Property SampleCount_WS04 As Integer
        Get
            SampleCount_WS04 = _sampleCount_WS04
        End Get
    End Property

    Public ReadOnly Property SampleCount_WS05 As Integer
        Get
            SampleCount_WS05 = _sampleCount_WS05
        End Get
    End Property

    Public ReadOnly Property TimeStampSampleBuffer As Date(,)
        Get
            'Lecture du remier Time Stamp apres un Clear buffer
            TimeStampSampleBuffer = _TimeStampSampleBuffer
        End Get
    End Property

    Public ReadOnly Property TimeStampSampleBuffer_WS05 As Date
        Get
            'Lecture du remier Time Stamp apres un Clear buffer
            TimeStampSampleBuffer_WS05 = _TimeStampSampleBuffer_WS05
        End Get
    End Property

    Public ReadOnly Property TimeStampSampleBuffer_WS04 As Date
        Get
            'Lecture du remier Time Stamp apres un Clear buffer
            TimeStampSampleBuffer_WS04 = _TimeStampSampleBuffer_WS04
        End Get
    End Property

    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Sub EmptyBuffer()
        ' Empty the buffer of the specified board
        _sampleCount = 0
    End Sub

    Public Sub EmptyBuffer_WS04()
        ' Empty the buffer of the specified board
        _sampleCount_WS04 = 0
    End Sub

    Public Sub EmptyBuffer_WS05()
        ' Empty the buffer of the specified board
        _sampleCount_WS05 = 0
    End Sub

    Public Function LoadCalibration(ByVal path As String, ByVal WS_Index As Integer) As Boolean
        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim token(0 To 3) As String

        ' Initialize the coarse gains and the decimals
        For i = 0 To eAnalogInput.Count - 1
            If WS_Index = 0 And i < CInt(eAnalogInput.Count / 2) Then
                '
                If (i = eAnalogInput.WS04_IBAT) Then
                    _coarseGain(i, WS_Index) = _current1CoarseGain
                    _decimals(i, WS_Index) = _current1Decimals
                ElseIf (i = eAnalogInput.WS04_EarlySensor) Then
                    _coarseGain(i, WS_Index) = _strokeCoarseGain_WS04
                    _decimals(i, WS_Index) = _strokeDecimals
                ElseIf (i = eAnalogInput.WS04_StrenghtSensor) Then
                    _coarseGain(i, WS_Index) = _forceCoarseGain_WS04
                    _decimals(i, WS_Index) = _forceDecimals
                Else
                    _coarseGain(i, WS_Index) = _voltageCoarseGain
                    _decimals(i, WS_Index) = _voltageDecimals
                End If
                _fineGain(i, WS_Index) = 1
                _offset(i, WS_Index) = 0

            ElseIf WS_Index = 1 And i >= CInt(eAnalogInput.Count / 2) Then
                '
                If (i = eAnalogInput.WS05_IBAT) Then
                    _coarseGain(i - CInt(eAnalogInput.Count / 2), WS_Index) = _current1CoarseGain
                    _decimals(i - CInt(eAnalogInput.Count / 2), WS_Index) = _current1Decimals
                ElseIf (i = eAnalogInput.WS05_EarlySensor) Then
                    _coarseGain(i - CInt(eAnalogInput.Count / 2), WS_Index) = _strokeCoarseGain_WS05
                    _decimals(i - CInt(eAnalogInput.Count / 2), WS_Index) = _strokeDecimals
                ElseIf (i = eAnalogInput.WS05_StrenghtSensor) Then
                    _coarseGain(i - CInt(eAnalogInput.Count / 2), WS_Index) = _forceCoarseGain_WS05
                    _decimals(i - CInt(eAnalogInput.Count / 2), WS_Index) = _forceDecimals
                Else
                    _coarseGain(i - CInt(eAnalogInput.Count / 2), WS_Index) = _voltageCoarseGain
                    _decimals(i - CInt(eAnalogInput.Count / 2), WS_Index) = _voltageDecimals
                End If
                _fineGain(i - CInt(eAnalogInput.Count / 2), WS_Index) = 1
                _offset(i - CInt(eAnalogInput.Count / 2), WS_Index) = 0
            End If
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
                If i < CInt(eAnalogInput.Count / 2) And WS_Index = 0 Then
                    ' Read the calibration coefficients
                    line = file.ReadLine
                    token = Split(line, vbTab)
                    AnalogInputFineGain(CType(i, eAnalogInput), WS_Index) = CDbl(token(1))
                    AnalogInputFineOffset(CType(i, eAnalogInput), WS_Index) = CDbl(token(2))
                ElseIf i >= CInt(eAnalogInput.Count / 2) And WS_Index = 1 Then
                    ' Read the calibration coefficients
                    line = file.ReadLine
                    token = Split(line, vbTab)
                    AnalogInputFineGain(CType(i - CInt(eAnalogInput.Count / 2), eAnalogInput), WS_Index) = CDbl(token(1))
                    AnalogInputFineOffset(CType(i - CInt(eAnalogInput.Count / 2), eAnalogInput), WS_Index) = CDbl(token(2))
                End If
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
        PowerUp = LoadCalibration(mWS04Main.Settings.AIOCalibrationPath, 0)
        ' Load the calibration coefficients
        PowerUp = LoadCalibration(mWS05Main.Settings.AIOCalibrationPath, 1)        
        ' Create the task
        PowerUp = PowerUp Or CreateTask()
    End Function



    Public Function SaveCalibration(ByVal path As String, ByVal WS_Index As Integer) As Boolean
        Dim file As StreamWriter = Nothing
        Dim i As Integer

        Try
            ' Open the file
            file = New StreamWriter(path)
            ' Read the heading
            file.WriteLine("Station ANALOG INPUT CALIBRATION COEFFICIENTS")
            file.WriteLine("")
            file.WriteLine("ANALOG INPUT DESCRIPTION" & _
                           vbTab & "FINE GAIN" & _
                           vbTab & "OFFSET" & _
                           vbTab & "UNITS")
            ' For all the analog inputs
            For i = 0 To eAnalogInput.Count - 1
                ' Write the calibration coefficients
                file.WriteLine(AnalogInputDescription(CType(i, eAnalogInput)) & _
                               vbTab & _fineGain(i, WS_Index).ToString(mUtility.FormatString(_fineGainDecimals)) & _
                               vbTab & _fineOffset(i, WS_Index).ToString(mUtility.FormatString(_decimals(i, WS_Index))) & _
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


    ''' <summary>
    ''' Not used since this is shared cards. always start sampling.
    ''' </summary>
    ''' <param name="sample"></param>
    ''' <param name="index"></param>
    ''' <returns></returns>
    Public Function SingleSample(ByVal sample(,) As Double, ByVal index As Integer) As Boolean
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
                    If i < eAnalogInput.Count / 2 And index = 0 Then
                        sample(i, 0) = 0
                        For j = 0 To sampleCount - 1
                            sample(i, index) = sample(i, index) + sampleBuffer(i).Samples(j).Value
                        Next
                        sample(i, index) = sample(i, index) / sampleCount
                        '
                        sample(i, index) = Round((sample(i, index) * _coarseGain(i, 0) + _coarseOffset(i, index)) * _fineGain(i, index), _decimals(i, index))
                        If (sample(i, index) < 0) And i <> eAnalogInput.WS04_EarlySensor And i <> eAnalogInput.WS04_StrenghtSensor Then
                            sample(i, index) = 0
                        End If
                    ElseIf i >= eAnalogInput.Count / 2 And index = 1 Then
                        sample(i - CInt(eAnalogInput.Count / 2), index) = 0
                        For j = 0 To sampleCount - 1
                            sample(i - CInt(eAnalogInput.Count / 2), index) = sample(i - CInt(eAnalogInput.Count / 2), index) + sampleBuffer(i).Samples(j).Value
                        Next
                        sample(i - CInt(eAnalogInput.Count / 2), index) = sample(i - CInt(eAnalogInput.Count / 2), index) / sampleCount
                        '
                        sample(i - CInt(eAnalogInput.Count / 2), index) = Round((sample(i - CInt(eAnalogInput.Count / 2), index) * _coarseGain(i - CInt(eAnalogInput.Count / 2), index) + _
                                                                         _coarseOffset(i - CInt(eAnalogInput.Count / 2), index)) * _fineGain(i - CInt(eAnalogInput.Count / 2), index) _
                                                                         , _decimals(i - CInt(eAnalogInput.Count / 2), index))
                        If (sample(i - CInt(eAnalogInput.Count / 2), index) < 0) And i - CInt(eAnalogInput.Count / 2) <> eAnalogInput.WS05_EarlySensor And i - CInt(eAnalogInput.Count / 2) <> eAnalogInput.WS05_StrenghtSensor Then
                            sample(i - CInt(eAnalogInput.Count / 2), index) = 0
                        End If

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
                    '
                    Sample_Task_Started = True
                Catch ex As Exception ' If some unexpected error occoured
                    ' Destroy the task
                    DestroyTask()
                    ' Clear the sampling frequency
                    _samplingFrequency = 0
                    ' Re-create the task
                    CreateTask()
                    ' Return True
                    StartSampling = True
                    '
                    Sample_Task_Started = False
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
        '
        Sample_Task_Started = False
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
                _task.AIChannels.CreateVoltageChannel(mWS04Main.Settings.AIOInterface & "/ai0:31", _
                                                      "", _
                                                      AITerminalConfiguration.Nrse, _
                                                      0, _
                                                      10, _
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
                'Debug.Print("Sample Count : " & eAnalogInput.Count & vbTab & _sampleCount_WS05 & vbTab & _sampleCount)

                For i = 0 To eAnalogInput.Count - 1
                    If i < eAnalogInput.Count / 2 Then
                        'WS04
                        For j = 0 To waveform(i).SampleCount - 1
                            If (_sampleCount_WS04 + j < _sampleBufferSize) Then
                                _sampleBuffer_WS04(i, _sampleCount_WS04 + j) = Round((waveform(i).Samples(j).Value * _coarseGain(i, 0) + _coarseOffset(i, 0)) * _fineGain(i, 0), _decimals(i, 0))
                            Else
                                _bufferOverflow = True
                                Exit For
                            End If
                        Next
                    Else
                        'WS05
                        For j = 0 To waveform(i).SampleCount - 1
                            If (_sampleCount_WS05 + j < _sampleBufferSize) Then
                                _sampleBuffer_WS05(i - CInt(eAnalogInput.Count / 2), _sampleCount_WS05 + j) = _
                                        Round((waveform(i).Samples(j).Value * _coarseGain(i - CInt(eAnalogInput.Count / 2), 1) _
                                        + _coarseOffset(i - CInt(eAnalogInput.Count / 2), 1)) * _fineGain(i - CInt(eAnalogInput.Count / 2), 1), _
                                       _decimals(i - CInt(eAnalogInput.Count / 2), 1))
                                _TimeStampSampleBuffer(i - CInt(eAnalogInput.Count / 2), _sampleCount_WS05 + j) = (waveform(i).Samples(j).TimeStamp)
                            Else
                                _bufferOverflow = True
                                Exit For
                            End If
                        Next
                    End If
                Next
                ' Update the sample count
                _sampleCount = 0 ' _sampleCount + waveform(0).SampleCount
                If _sampleCount_WS04 = 0 And waveform(0).SampleCount > 0 Then
                    _TimeStampSampleBuffer_WS04 = (waveform(0).Samples(0).TimeStamp)
                End If
                ' Update the sample count
                If _sampleCount_WS04 < _sampleBufferSize Then
                    ' Update the sample count
                    _sampleCount_WS04 = _sampleCount_WS04 + waveform(0).SampleCount
                End If
                '
                If _sampleCount_WS05 = 0 And waveform(0).SampleCount > 0 Then
                    _TimeStampSampleBuffer_WS05 = (waveform(0).Samples(0).TimeStamp)
                End If
                ' Update the sample count
                If _sampleCount_WS05 < _sampleBufferSize Then
                    ' Update the sample count
                    _sampleCount_WS05 = _sampleCount_WS05 + waveform(0).SampleCount
                End If
                ' Re-start reading the analog inputs
                _reader.BeginMemoryOptimizedReadWaveform(-1, _callback, _task, waveform)
            End If

        Catch ex As Exception   ' If some unexpected error occoured
            ' Stop sampling
            StopSampling()
        End Try
    End Sub
End Module
