﻿Option Explicit On
'Option Strict On

Imports System
Imports System.IO
Imports Microsoft.VisualBasic
Imports NationalInstruments.DAQmx
Imports System.Math
Imports System.Threading


Module mWS03Main
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Alarm enumeration
    Public Enum eAlarm
        ErrorLoadingRecipe = 0
        PartTypeUnknown = 1
        PartUniqueNumberNotValid = 2
        RecipeNotValid = 3
        RecipeUnknown = 4
        RuntimeError = 5

        Count = 6
    End Enum

    Public Enum eWindows
        FrontLeft = 0
        FrontRight = 1

    End Enum

    ' Phase enumeration
    Public Enum ePhase
        WaitRecipeSelection = 0
        LoadRecipe = 1
        WaitStartTest = 2
        ClearResults = 3
        WriteResults = 4
        WaitEndTest = 5
        AbortTest = 6
        TesterPresent = 7
        ' 
        Power_UP = 10
        Init_Communication = 11
        Open_DIAGonLinSession = 12
        MMS_Traceability = 13
        FrontRight = 14
        FrontLeft = 15
        ChildrenLock = 16
        FinalState = 17
        Write_MMS_TestByte = 18
        Read_MMS_TestByte = 19
        PowerDown = 20
        '
        Count = 21
    End Enum

    ' Test mode enumeration
    ''' <summary>
    ''' PLC Test Mode DBW4
    ''' 0 : Remote Mode
    ''' 1 : Local Mode
    ''' 2: Analysis Mode
    ''' 3: Repeat Mode
    ''' 4: Master Part
    ''' </summary>
    Public Enum eTestMode
        Remote = 0
        Local = 1
        Analyse = 2
        Retest = 3
        Master = 4
        Debug = 5
        PartType = 6
        Learning = 7

    End Enum

    Public Enum cSample_Signal
        EarlySensor = 0
        StrenghtSensor = 1
        Pull_Manual = 2
        Pull_Automatic = 3
        Push_Manual = 4
        Push_Automatic = 5        
        '
        Count = 6
    End Enum

    ' Settings structure
    Public Structure sSettings
        Public AIOCalibrationPath As String
        Public AIOInterface As String
        Public LINInterface As String
        Public LINBaudrate As Integer
        Public LINFramesPath As String
        Public RecipeConfigurationPath As String
        Public ResultsConfigurationPath As String
        Public RecipePath As String
        Public ResultsPath As String
        Public MasterReferencePath As String
        Public InputDBConfigurationPath As String
        Public OutputDBConfigurationPath As String
        Public TimeAfterCommutation As Integer
        Public TimeBeforeCommutation As Integer
        Public FilterCurveForce As Integer
        Public FilterCurveDistance As Integer
        Public CalculateDerivate As Integer
        Public LastPartNumberPath As String

    End Structure

    'private structure
    Private Structure sWiliResult
        Dim Xs_F1_index As Integer
        Dim Xs_F2_index As Integer
        Dim Xs_F4_index As Integer
        Dim Xs_F5_index As Integer
        Dim X_Fe1_index As Integer
        Dim X_Fe2_index As Integer
        Dim X_Ce1_index As Integer
        Dim X_Ce2_index As Integer
        Dim Xe_Index As Integer
        Dim Xs_Index As Integer

        ''' <summary>
        ''' Fisrt State Peak Force, F1
        ''' </summary>
        Dim Fs1_F1 As Single
        ''' <summary>
        ''' Stroke value at position Peak Force F1. means S1.
        ''' </summary>
        Dim Xs1 As Single
        ''' <summary>
        ''' First State Valley force, F2
        ''' </summary>
        Dim Fs1_F11 As Single
        ''' <summary>
        ''' not used???
        ''' </summary>
        Dim Xs12 As Single
        ''' <summary>
        ''' Ce1 - S2
        ''' </summary>
        Dim Value_DiffS2Ce1 As Single
        ''' <summary>
        ''' First state force ratio
        ''' </summary>
        Dim dFs1_Haptic_1 As Single
        ''' <summary>
        ''' Stoke value at position Valley Force F2. means S2.
        ''' </summary>
        Dim dXs1 As Single
        ''' <summary>
        ''' Force Value at Contact1.
        ''' </summary>
        Dim FsCe1 As Single
        ''' <summary>
        ''' Not Used???
        ''' </summary>
        Dim FeCe1 As Single
        ''' <summary>
        ''' Stroke Value at Contact1.
        ''' </summary>
        Dim XCe1 As Single
        ''' <summary>
        ''' Second State Peak Force, F5
        ''' </summary>
        Dim Fs2_F2 As Single
        ''' <summary>
        ''' Stroke value at position Peak Force F5. means S5.
        ''' </summary>
        Dim Xs2 As Single
        ''' <summary>
        ''' Second State Valley force, F5
        ''' </summary>
        Dim Fs2_F21 As Single
        ''' <summary>
        ''' Not Used???
        ''' </summary>
        Dim Xs21 As Single
        ''' <summary>
        ''' Ce2 - S5
        ''' </summary>
        Dim Value_DiffS5Ce2 As Single
        ''' <summary>
        ''' Second state force ratio
        ''' </summary>
        Dim dFs2_Haptic_2 As Single
        ''' <summary>
        ''' Stoke value at position Valley Force F5. means S5.
        ''' </summary>
        Dim dXs2 As Single
        ''' <summary>
        ''' Force Value at Contact2.
        ''' </summary>
        Dim FsCe2 As Single
        ''' <summary>
        ''' Not Used???
        ''' </summary>
        Dim FeCe2 As Single
        ''' <summary>
        ''' Stroke Value at Contact2.
        ''' </summary>
        Dim XCe2 As Single
        ''' <summary>
        ''' Force Value at End Position.
        ''' </summary>
        Dim Fe As Single
        ''' <summary>
        ''' Stroke Value at End Position.
        ''' </summary>
        Dim Xe As Single
        ''' <summary>
        ''' Stroke Value at Start Position.
        ''' </summary>
        Dim Xs As Single

        ''' <summary>
        ''' Not Used???
        ''' </summary>
        Dim XvCe1 As Single
        ''' <summary>
        ''' Not Used???
        ''' </summary>
        Dim XvCe2 As Single


        Dim dFs3_Haptic_3 As Single
    End Structure

    ' Input enumeration
    Public Structure eInput_WS03
        Public PartTypeNumber As Integer
        Public Test_Mode As Integer
        Public PartModel As Integer
        Public Reference As String
        Public UniqueNumber As String

    End Structure

    ' Outputs enumeration
    Public Structure eOutput_WS03
        Public ResultCode As Double
        Public Reference As String
        Public UniqueNumber As String

    End Structure

    Public txData_MasterReq(0 To 7) As Byte
    Public btxRequest_MasterReq As Boolean

    Public RunningTimer As New cRunningTime("WS03")

    Public PushState(0 To 19) As Boolean

    ' For Maintenance Page
    Public ADC_WL_Front_Left As String
    Public ADC_WL_Front_Right As String
    Public ADC_WL_REAR_Front_PASSENGER As String
    Public ADC_MIRROR_ADJUSTMENT As String

    Public Push_Mirror_UP As Boolean
    Public Push_Mirror_DN As Boolean
    Public Push_Mirror_Left As Boolean
    Public Push_Mirror_Right As Boolean
    Public Push_Mirror_SL As Boolean
    Public Push_Mirror_SR As Boolean
    Public Push_Mirror_Fold As Boolean

    Public Push_Children As Boolean

    Public WL_FL_Push_M As Boolean
    Public WL_FL_Push_A As Boolean
    Public WL_FL_Pull_M As Boolean
    Public WL_FL_Pull_A As Boolean

    Public WL_FR_Push_M As Boolean
    Public WL_FR_Push_A As Boolean
    Public WL_FR_Pull_M As Boolean
    Public WL_FR_Pull_A As Boolean

    Public WL_RL_Push_M As Boolean
    Public WL_RL_Push_A As Boolean
    Public WL_RL_Pull_M As Boolean
    Public WL_RL_Pull_A As Boolean

    Public WL_RR_Push_M As Boolean
    Public WL_RR_Push_A As Boolean
    Public WL_RR_Pull_M As Boolean
    Public WL_RR_Pull_A As Boolean
    ' end Maintrnance Page
    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _HardwareEnabled_PLC = mConstants.HardwareEnabled_PLC
    Private Const _HardwareEnabled_VIPA = mConstants.HardwareEnabled_VIPA
    Private Const _HardwareEnabled_NI = mConstants.HardwareEnabled_NI
    Private Const _HardwareEnabled_TTI = mConstants.HardwareEnabled_TTI


    Private Const _TesterPresentDelay_10ms = 10
    Private Const LINTimeout_ms = 400
    Private Const LinRelance_ms = 100
    Private Const PowerUpDelay_ms = 150
    Private Const PowerUpTimeout_ms = 2000
    Private Const StandardSignalCount = 16
    Private Const samplingFrequency = 2000

    ' Testeur period frame period
    Private Const mintTesteurPeriodms = 1500
    Private mbooTesteurPresent As Boolean

    ' Private variables
    Private msngT0TesteurPeriodicFrame As Date
    Private _abort As Boolean
    Private _alarm(0 To eAlarm.Count) As Boolean
    Private _LINFrame(0 To 500) As CLINFrame
    Private _LinInterface As New cLINInterface
    Private _counterFailed As Integer
    Private _counterPassed As Integer
    Private _forcePartOk As Boolean
    Private _forcePartTypeOk As Boolean
    Private _learningMode As Boolean
    Private _log As New System.Text.StringBuilder
    Private _Linlog(0 To 10) As String
    Private _maxScanTime As Single
    Private _phase As ePhase
    Private _reference As String
    Private _scanTime As Single
    Private _settings As sSettings
    Private _start As Boolean
    Private _step As Boolean
    Private _subPhase(0 To ePhase.Count) As Integer
    Private _subPhaseMaint(0 To ePhase.Count) As Integer
    Private _stepByStep As Boolean
    Private _testLast As Single
    Private _testMode As eTestMode
    Private _t0Test As Date
    Private _LinData_DIAG_MasterEquest(0 To 7) As Byte

    '  recipes and results
    Private _recipe As cWS03Recipe
    Private _results As cWS03Results
    Private _resultsPT As cWS03Results
    Private _recipeMaster As cWS03Recipe

    Private _PhaseSensor As Integer

    Private _Ethernet_Input As eInput_WS03
    Private _Ethernet_Output As eOutput_WS03

    Private Param_1 As Integer
    Private Param_2 As Integer

    'Private HbmReset As Boolean

    Private FilePoint As String

    Private PhaseBefore_TP As ePhase
    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property Alarm(ByVal alarmIndex As eAlarm) As Boolean
        Get
            Alarm = _alarm(alarmIndex)
        End Get
    End Property


    Public Property ForcePartOk As Boolean
        Get
            ForcePartOk = _forcePartOk
        End Get
        Set(ByVal value As Boolean)
            _forcePartOk = value
        End Set
    End Property

    Public Property ForcePartTypeOk As Boolean
        Get
            ForcePartTypeOk = _forcePartTypeOk
        End Get
        Set(ByVal value As Boolean)
            _forcePartTypeOk = value
        End Set
    End Property



    Public Property LearningMode As Boolean
        Get
            LearningMode = _learningMode
        End Get
        Set(ByVal value As Boolean)
            _learningMode = value
            If value = True Then
                _testMode = eTestMode.Learning
            Else
                _testMode = eTestMode.Remote
            End If
        End Set
    End Property

    Public WriteOnly Property TimeTesterPresent As Date
        Set(ByVal value As Date)
            msngT0TesteurPeriodicFrame = value
        End Set
    End Property
    Public ReadOnly Property CounterFailed As Integer
        Get
            CounterFailed = _counterFailed
        End Get
    End Property



    Public ReadOnly Property CounterPassed As Integer
        Get
            CounterPassed = _counterPassed
        End Get
    End Property



    Public ReadOnly Property CounterTested As Integer
        Get
            CounterTested = _counterPassed + _counterFailed
        End Get
    End Property

    Public ReadOnly Property LinLog(ByVal partIndex As Integer) As String
        Get
            LinLog = _Linlog(partIndex)
        End Get
    End Property

    Public ReadOnly Property Log As String
        Get
            Log = _log.ToString
        End Get
    End Property


    Public ReadOnly Property MaxScanTime As Single
        Get
            MaxScanTime = _maxScanTime
        End Get
    End Property


    Public ReadOnly Property Phase As ePhase
        Get
            Phase = _phase
        End Get
    End Property


    Public Property Reference As String
        Get
            Reference = _reference
        End Get
        Set(ByVal value As String)
            If (_phase = ePhase.WaitRecipeSelection Or
                TestMode = eTestMode.Debug Or Simulation_Test) Then
                _reference = value
            End If
        End Set
    End Property


    Public ReadOnly Property Results As cWS03Results
        Get
            Results = _results
        End Get
    End Property

    Public ReadOnly Property ScanTime As Single
        Get
            ScanTime = _scanTime
        End Get
    End Property



    Public ReadOnly Property Settings As sSettings
        Get
            Settings = _settings
        End Get
    End Property


    Public Property StepByStep As Boolean
        Get
            StepByStep = _stepByStep
        End Get
        Set(ByVal value As Boolean)
            _stepByStep = value
        End Set
    End Property

    Public ReadOnly Property StepStatus As Boolean
        Get
            StepStatus = _step
        End Get
    End Property

    Public ReadOnly Property SubPhase(ByVal phaseIndex As ePhase) As Integer
        Get
            SubPhase = _subPhase(phaseIndex)
        End Get
    End Property

    Public ReadOnly Property StepInProgressInputStatus As Boolean
        Get
            StepInProgressInputStatus = mDIOManager.DigitalInputStatus(eDigitalInput.WS03_StepInProgress)
        End Get
    End Property

    Public ReadOnly Property TestEnableInputStatus As Boolean
        Get
            TestEnableInputStatus = mDIOManager.DigitalInputStatus(eDigitalInput.WS03_TestEnable)
        End Get
    End Property

    Public ReadOnly Property TestOkOutputStatus As Boolean
        Get
            TestOkOutputStatus = mDIOManager.DigitalOutputStatus(mDIOManager.eDigitalOutput.WS03_TestOk)
        End Get
    End Property


    Public ReadOnly Property StartStepOutputStatus As Boolean
        Get
            StartStepOutputStatus = mDIOManager.DigitalOutputStatus(mDIOManager.eDigitalOutput.WS03_StartStep)
        End Get
    End Property



    Public ReadOnly Property TestLast As Single
        Get
            TestLast = _testLast
        End Get
    End Property



    Public ReadOnly Property TestMode() As eTestMode
        Get
            TestMode = _testMode
        End Get
    End Property


    Public ReadOnly Property WaitingForStep As Boolean
        Get
            WaitingForStep = False
        End Get
    End Property

    Public Sub ManageSubPhaseMaint(ByVal _SubPhaseName As Integer,
                               ByVal _Step As Integer,
                               ByVal _Param_1 As Integer,
                               ByVal _Param_2 As Integer)

        _subPhaseMaint(_SubPhaseName) = _Step

        Param_1 = _Param_1
        Param_2 = _Param_2

    End Sub


    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+

    Public Sub StartPLC()
        mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Test_Enable_PLC) = True
        mWS03Ethernet.WriteOutputDataBlock()
    End Sub


    Public Sub AbortTest()
        ' Set the abort test flag
        _abort = (_phase <> ePhase.WaitRecipeSelection And
                  _phase <> ePhase.LoadRecipe And
                  _phase <> ePhase.WaitStartTest And
                  _phase <> ePhase.AbortTest)
    End Sub


    Public Sub ReloadRecipe()
        _phase = ePhase.LoadRecipe
    End Sub

    Public Function AlarmDescription(ByVal alarmIndex As eAlarm) As String
        ' Return the alarm description
        Select Case alarmIndex
            Case eAlarm.ErrorLoadingRecipe
                AlarmDescription = "Error loading recipe"
            Case eAlarm.PartUniqueNumberNotValid
                AlarmDescription = "Part unique number not valid"
            Case eAlarm.PartTypeUnknown
                AlarmDescription = "Part type unknown"
            Case eAlarm.RecipeNotValid
                AlarmDescription = "Recipe not valid"
            Case eAlarm.RecipeUnknown
                AlarmDescription = "Recipe unknown"
            Case eAlarm.RuntimeError
                AlarmDescription = "Runtime error"
            Case Else
                AlarmDescription = String.Format("Value {0} unknown", alarmIndex)
        End Select
    End Function

    Public ReadOnly Property SuphaseMaint_Status(ByVal StepSubPhase As Integer) As Integer
        Get
            SuphaseMaint_Status = _subPhaseMaint(StepSubPhase)
        End Get
    End Property

    Public Sub ClearResultMaint()
        mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.ResultCode) = 100000
        mWS03Ethernet.WriteOutputDataBlock()
    End Sub

    Public Sub SetResultMaint()
        mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.ResultCode) = 100010
        mWS03Ethernet.WriteOutputDataBlock()
    End Sub

    Public Sub ClearAlarms()
        ' Clear all the alarms
        For i = 0 To eAlarm.Count - 1
            _alarm(i) = False
        Next i
        '
        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_Alarm)

    End Sub

    Public Sub ClearLog()
        ' Clear the log
        _log.Clear()
    End Sub


    Public Function LoadSettings(ByVal path As String) As Boolean
        Dim file As StreamReader = Nothing
        Dim line As String
        Dim token() As String

        Try
            ' Open the file
            file = New StreamReader(path, System.Text.Encoding.Default)
            ' Read the heading
            line = file.ReadLine
            line = file.ReadLine
            ' Read the AIO calibration path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.AIOCalibrationPath = token(1)
            ' Read the AIO Board
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.AIOInterface = token(1)
            ' Read the input DB configuration path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.InputDBConfigurationPath = token(1)
            ' Read the output DB configuration path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.OutputDBConfigurationPath = token(1)
            ' Lin Interface
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.LINInterface = token(1)
            ' Read the Lin baudrate
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.LINBaudrate = token(1)
            ' Read the LIN frames path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.LINFramesPath = token(1)
            ' Read the recipe configuration path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.RecipeConfigurationPath = token(1)
            ' Read the results configuration path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.ResultsConfigurationPath = token(1)
            ' Read the recipe path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.RecipePath = token(1)
            ' Read the results path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.ResultsPath = token(1)
            ' Read the master reference path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.MasterReferencePath = token(1)
            ' Read 
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.TimeAfterCommutation = 2 * CInt(token(1))
            ' Read 
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.TimeBeforeCommutation = 2 * CInt(token(1))
            ' Read 
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.FilterCurveForce = CInt(token(1))
            ' Read 
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.FilterCurveDistance = CInt(token(1))
            ' Read 
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.CalculateDerivate = CInt(token(1))


            ' Return False
            LoadSettings = False

        Catch ex As Exception
            ' Return True
            LoadSettings = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function


    Public Function PhaseDescription(ByVal phase As ePhase) As String
        ' Return the phase description
        Select Case phase
            Case ePhase.WaitRecipeSelection
                PhaseDescription = "Wait recipe selection"
            Case ePhase.LoadRecipe
                PhaseDescription = "Load recipe"
            Case ePhase.WaitStartTest
                PhaseDescription = "Wait start test"
            Case ePhase.ClearResults
                PhaseDescription = "Clear results"
            Case ePhase.WriteResults
                PhaseDescription = "Write results"
            Case ePhase.WaitEndTest
                PhaseDescription = "Wait end test"
            Case ePhase.AbortTest
                PhaseDescription = "Abort test"
            Case ePhase.Power_UP
                PhaseDescription = "Power-up"

            Case ePhase.Init_Communication
                PhaseDescription = "Init LIN Communication"
            Case ePhase.Open_DIAGonLinSession
                PhaseDescription = "Open DIAG on LIN Session"

            Case Else
                PhaseDescription = String.Format("Value {0} unknown", phase)
        End Select
    End Function



    Public Function PowerDown() As Boolean
        Dim e As Boolean
        Dim r As Boolean

        ' Power-down the digital I/O manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-down Work Station 03  digital I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS03DIOManager.PowerDown
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-down of the Work Station 03  digital I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = e

        ' Power-down the analog I/O manager
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-down the Work Station 03  analog I/O manager... ")
            If _HardwareEnabled_NI Then
                e = mWS03AIOManager.PowerDown
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-down of the Work Station 03  analog I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = PowerDown Or e

    End Function



    Public Function PowerUp() As Boolean
        Dim e As Boolean
        Dim r As Boolean
        Dim token() As String

        ' Load the station settings
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load Work Station 03 settings... ")
        Do
            e = LoadSettings(mSettings.WS03SettingsPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work Station 03 settings: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = e

        ' Power-up the analog I/O manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-up the Work Station 03 analog I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS03AIOManager.PowerUp()
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-up of the Work Station 03 analog I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Power-up the digital I/O manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-up Work Station 03 digital I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS03DIOManager.PowerUp
            Else
                e = False
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-up of the Work Station 03  digital I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the LIN frames
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load Work Station 03 the LIN frames  ... ")
        Do
            e = CLINFrame.LoadArrayFromFile(_settings.LINFramesPath, _LINFrame)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading Work Station 03 the LIN frames : retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e


        ' Load the recipe configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work Station 03  recipe FHM configuration... ")
        _recipe = New cWS03Recipe
        Do
            e = _recipe.LoadConfiguration(_settings.RecipeConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work Station 03  recipe FHM configuration: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e


        ' Load the results configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work Station 03 results FHM configuration... ")
        _results = New cWS03Results
        _resultsPT = New cWS03Results
        Do
            e = _results.LoadConfiguration(_settings.ResultsConfigurationPath) 'Or _
            '_resultsPT.LoadConfiguration(_settings.ResultsConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work Station 03  results FHM configuration: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the PLC configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work Station 03  PLC configuration... ")
        Do
            e = mWS03Ethernet.PowerUp(_settings.InputDBConfigurationPath, _settings.OutputDBConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work Station 03  PLC configuration: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Go to the phase WaitStartTest
        _phase = ePhase.WaitStartTest

        If Simulation_Test Then _testMode = eTestMode.Debug
    End Function



    Public Sub ResetCounters()
        ' Reset the counters
        _counterPassed = 0
        _counterFailed = 0
    End Sub



    Public Sub StartTest()
        Dim f As Boolean

        ' If the phase is WaitStartTest
        If (_phase = ePhase.WaitStartTest) Then
            ' Store the part enables
            f = True
            ' Set the start flag
            _start = f
        End If
    End Sub


    Public Sub StepTest()
        ' Set the step test flag
        _step = (_stepByStep)
    End Sub

    ' Maintenance Module    
    Public Sub LinMaintenance()
        Dim e As Boolean

        If (_LinInterface.Connected) Then
            e = _LinInterface.PowerDown
        Else
            e = _LinInterface.PowerUp(_settings.LINInterface, _settings.LINBaudrate, True, cLINInterface.eScheduleData.SCHEDULE_Null)
        End If

    End Sub


    Public Sub Loop_Maint()
        Dim e As Boolean
        Dim i As Integer

        ' Receive the LIN frames
        If (_LinInterface.Connected) Then
            Dim logFilter() As String = {"122", "53B"}
            Dim receiveFilter() As String = {"122", "53B"}
            e = e Or _LinInterface.Receive(receiveFilter, logFilter)
            ' Streaming Frame
            For i = mGlobal.eLog_File.Frame_All To mGlobal.eLog_File.Frame_rx_3C
                _Linlog(i) = _LinInterface.Log(i)
                If i = mGlobal.eLog_File.Frame_rx_A Then
                    i = i
                End If
            Next i

            _LinInterface.ClearLog()
        End If

        ' Diag Manage
        If (_LinInterface.Connected) And btxRequest_MasterReq = True Then
            btxRequest_MasterReq = False
            ' Transmit Frame
            e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty), True, txData_MasterReq, 2, False, True)

        End If

        If _LinInterface.Connected Then
            Maint_OpenDiag()
            Maint_PhaseRead(Param_1, Param_2)
        Else
            _subPhaseMaint(0) = 999
            _subPhaseMaint(1) = 999
        End If


        _LinInterface.ClearRxBuffer()

    End Sub


    Private Sub Maint_OpenDiag()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim s As String
        Dim f As CLINFrame

        Static LinTimeout As Boolean
        Static t0 As Date
        Static t0Phase As Date
        Static tLin As Date
        Static KeyAccess(0 To 5) As String

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhaseMaint(0)
        ' Manage the subphases
        Select Case sp

            Case 0
                LinTimeout = False
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Open Diag on Lin Session ")
                '
                ClearLog()
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhaseMaint(0) = 1

            Case 1
 _
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Diag_1060),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhaseMaint(0) = 2

            Case 2
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Diag_1060))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(0)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhaseMaint(0) = 3
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    'Set Flag timeout
                    LinTimeout = True
                    ' Go to subphase
                    _subPhaseMaint(0) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= 200) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Diag_1060),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 3
                ' Read Key
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ReadKey),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhaseMaint(0) = 4

            Case 4
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ReadKey))
                If (i <> -1) Then
                    KeyAccess(0) = _LinInterface.RxFrame(i).Data(4) &
                                    _LinInterface.RxFrame(i).Data(5) &
                                    _LinInterface.RxFrame(i).Data(6) &
                                    _LinInterface.RxFrame(i).Data(7)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    KeyAccess(1) = Right("00000000" & Hex(CStr((CDbl("&hFFFFFFFF") - CDbl("&h" & KeyAccess(0))) + 1)), 8)
                    KeyAccess(2) = Mid(KeyAccess(1), 1, 2)
                    KeyAccess(3) = Mid(KeyAccess(1), 3, 2)
                    KeyAccess(4) = Mid(KeyAccess(1), 5, 2)
                    KeyAccess(5) = Mid(KeyAccess(1), 7, 2)

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhaseMaint(0) = 5
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    'Set timeout flag
                    LinTimeout = True
                    'go to subphase 
                    _subPhaseMaint(0) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= 200) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ReadKey),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 5
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_SendKey).DeepClone
                f.Data(4) = KeyAccess(2)
                f.Data(5) = KeyAccess(3)
                f.Data(6) = KeyAccess(4)
                f.Data(7) = KeyAccess(5)

                ' Send Key
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhaseMaint(0) = 6

            Case 6
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_SendKey))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhaseMaint(0) = 7
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    'Set timeout flag
                    LinTimeout = True
                    ' go to subphase
                    _subPhaseMaint(0) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= 200) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_SendKey).DeepClone
                    f.Data(4) = KeyAccess(2)
                    f.Data(5) = KeyAccess(3)
                    f.Data(6) = KeyAccess(4)
                    f.Data(7) = KeyAccess(5)

                    ' Send Key
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 7
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhaseMaint(0) = 8

            Case 8
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_OpenDiag_1070))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(0)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhaseMaint(0) = 10
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    'Set Flag timeout
                    LinTimeout = True
                    ' Go to subphase
                    _subPhaseMaint(0) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= 200) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 10
                If ((Date.Now - t0).TotalMilliseconds > 3000) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TesterPresent),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                End If


            Case 199
                ' Lin                     
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Lin                     
                ' Go to next subphase
                _subPhaseMaint(0) = 101


        End Select

        If TestMode = eTestMode.Remote Then
            e = False ' Tmp David WILI
        End If
        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(0) & " , subphase " & sp)

            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub

    Private Sub Maint_PhaseRead(ByVal param1 As String, ByVal param2 As String)
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
        Dim LinTimeout As Boolean
        Static t0 As Date
        Static t0Phase As Date
        Static s As String
        Static frameIndex As Integer
        Static tLin As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhaseMaint(1)
        ' Manage the subphases
        Select Case sp
            Case 0

            Case 1
                '
                _subPhaseMaint(0) = 199

                If ((Date.Now - t0).TotalMilliseconds > 100) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_ALL_DIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to next subphase
                    _subPhaseMaint(1) += 1
                End If

            Case 2
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Read_ALL_DIN))
                If (i <> -1) Then
                    Push_Children = Not CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 7) And &H1)))))
                    Push_Mirror_SL = Not CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 6) And &H1)))))
                    Push_Mirror_SR = Not CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 2) And &H1)))))
                    Push_Mirror_Fold = Not CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 1) And &H1)))))
                    '
                    WL_FL_Pull_A = False : WL_FL_Pull_M = False : WL_FL_Push_A = False : WL_FL_Push_M = False
                    WL_FR_Pull_A = False : WL_FR_Pull_M = False : WL_FR_Push_A = False : WL_FR_Push_M = False
                    WL_RL_Pull_A = False : WL_RL_Pull_M = False : WL_RL_Push_A = False : WL_RL_Push_M = False
                    WL_RR_Pull_A = False : WL_RR_Pull_M = False : WL_RR_Push_A = False : WL_RR_Push_M = False
                    If CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 5) And &H1))))) Then
                        If Not CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 4) And &H1))))) Then
                            WL_FL_Pull_M = True
                        ElseIf Not CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 3) And &H1))))) Then
                            WL_FL_Push_M = True
                        End If
                    Else
                        If Not CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 4) And &H1))))) Then
                            WL_FL_Pull_A = True
                        ElseIf Not CBool(CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 3) And &H1))))) Then
                            WL_FL_Push_A = True
                        End If
                    End If
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhaseMaint(1) = 1 '3
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhaseMaint(1) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_ALL_DIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    '
                    tLin = Date.Now
                End If


            Case 10
                '
                _subPhaseMaint(0) = 199

                If ((Date.Now - t0).TotalMilliseconds > 100) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to next subphase
                    _subPhaseMaint(1) += 1
                End If

            Case 11
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Read_All_AIN))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(5) &
                        _LinInterface.RxFrame(i).Data(6) &
                        _LinInterface.RxFrame(i).Data(7)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' init index Frame
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhaseMaint(1) = 10
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) And frameIndex = -1 Then
                    '
                    tLin = Date.Now
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)

                End If

                Do
                    ' If a CAN frame was received
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_FlowCtrl).DeepClone
                    For i = 1 To 7
                        f.Data(i) = "XX"
                    Next i
                    f.Data(1) = CStr(frameIndex)
                    ' Check Response
                    i = _LinInterface.RxFrameIndex(f)
                    If (i <> -1) Then
                        ' Store the values
                        s = s & _LinInterface.RxFrame(i).Data(2) &
                                _LinInterface.RxFrame(i).Data(3) &
                                _LinInterface.RxFrame(i).Data(4) &
                                _LinInterface.RxFrame(i).Data(5) &
                                _LinInterface.RxFrame(i).Data(6) &
                                _LinInterface.RxFrame(i).Data(7)
                        ' Delete the frame
                        _LinInterface.DeleteRxFrame(i)
                        ' Increase the frame index or go to subphase 100
                        If (frameIndex < 24) Then
                            frameIndex = frameIndex + 1
                            t0 = Date.Now
                        Else
                            ADC_WL_Front_Left = Mid(s, 1, 4)
                            ADC_WL_Front_Right = Mid(s, 5, 4)
                            ADC_WL_REAR_Front_PASSENGER = Mid(s, 9, 4)
                            ADC_MIRROR_ADJUSTMENT = Mid(s, 13, 4)

                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            t0 = Date.Now
                            _subPhaseMaint(1) = 10
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        ' Set the flag of CAN timeout
                        LinTimeout = True
                        t0 = Date.Now
                        _subPhaseMaint(1) = 10
                    End If
                Loop Until (i = -1)

            Case 100

            Case 199
                e = e
        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David WILI
        End If

        ' If a runtime error occured
        If (e = True) Then
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If
    End Sub

    ' end Maintenance Module


    Public Sub TestLoop()
        Dim alarmFlag As Boolean
        Dim e As Boolean
        Dim f As Boolean
        Static t0ScanTime As Date
        Static t0SensorCheck As Date
        Static t0RepeatTest As Date
        Dim retesttime As Int16 = -1  'if no need, set to -1
        Dim retestwaitseconds As Int16 = 13 'default 13s. bigger than one test cycle. 
        Static tCurrentRetest As Int16

        ' Receive the ROOF LIN frames
        If (_LinInterface.Connected) Then
            Dim logFilter() As String = {"122", "53B"}
            Dim receiveFilter() As String = {"122", "53B"}
            e = e Or _LinInterface.Receive(receiveFilter, logFilter)
            _log.Append(_LinInterface.Log(mGlobal.eLog_File.Frame_All))
            _LinInterface.ClearLog()

            ' Testeur Present periodic frame
            If (Date.Now - msngT0TesteurPeriodicFrame).TotalMilliseconds >= mintTesteurPeriodms Then
                'init time TP
                msngT0TesteurPeriodicFrame = Date.Now
                ' memorise the Actual Phase
                PhaseBefore_TP = _phase
                'Go to Phase TP
                _phase = ePhase.TesterPresent
                '
                _LinInterface.TesteurPresent = False
            End If
            If _LinInterface.TesteurPresent Then
                'init time TP
                msngT0TesteurPeriodicFrame = Date.Now
                '
                _LinInterface.TesteurPresent = False
            End If
        Else
            'init time TP
            msngT0TesteurPeriodicFrame = Date.Now
        End If

        ' Update the test last
        If (_phase <> ePhase.WaitStartTest And
            _phase <> ePhase.WaitRecipeSelection And
            _phase <> ePhase.LoadRecipe) Then
            _testLast = (Date.Now - _t0Test).TotalSeconds
        End If

        ' Manage the abort test

        f = (_testMode = eTestMode.Debug And _abort = True)
        f = f Or (_testMode = eTestMode.Local Or _testMode = eTestMode.Remote) And
            mDIOManager.DigitalInputStatus(eDigitalInput.WS03_TestEnable) = False
        f = f And Not (_phase = ePhase.WaitRecipeSelection Or
                       _phase = ePhase.LoadRecipe Or
                       _phase = ePhase.WaitStartTest Or
                       (_phase = ePhase.WriteResults And _subPhase(_phase) = 3) Or
                       _phase = ePhase.WaitEndTest)
        If (f = True) Then
            _abort = False
            _phase = ePhase.AbortTest
        End If


        ' Manage the phases
        Select Case _phase
            Case ePhase.WaitRecipeSelection
                PhaseWaitRecipeSelection()
            Case ePhase.LoadRecipe
                PhaseLoadRecipe()
                If (_reference <> "") Then
                    t0RepeatTest = Now
                    tCurrentRetest = 0
                End If
            Case ePhase.WaitStartTest
                PhaseWaitStartTest()
                If (Now - t0RepeatTest).TotalSeconds > retestwaitseconds AndAlso tCurrentRetest < retesttime Then
                    t0RepeatTest = Now
                    tCurrentRetest += 1
                    StartPLC()
                End If
            Case ePhase.ClearResults
                PhaseClearResults()
            Case ePhase.WriteResults
                PhaseWriteResults()
            Case ePhase.WaitEndTest
                PhaseWaitEndTest()
            Case ePhase.AbortTest
                PhaseAbortTest()
            Case ePhase.TesterPresent
                PhaseTesteurPresent()
                ' Step Product 
            Case ePhase.Power_UP
                PhasePowerUp()
            Case ePhase.Init_Communication
                PhaseInit_Communication()
            Case ePhase.Open_DIAGonLinSession
                PhaseOpen_DIAGonLINSession()
            Case ePhase.MMS_Traceability
                PhaseMMSTraceability()
            Case ePhase.FrontLeft
                PhaseTest(eWindows.FrontLeft)
            Case ePhase.FrontRight
                PhaseTest(eWindows.FrontRight)
            Case ePhase.ChildrenLock
                PhaseChildrenLock()
            Case ePhase.FinalState
                PhaseFinal_StateProduct()
            Case ePhase.Write_MMS_TestByte
                PhaseWRITE_MMSTestByte()
            Case ePhase.Read_MMS_TestByte
                PhaseRead_MMSTestByte()
            Case ePhase.PowerDown
                PhasePowerDown()
        End Select

        ' Calculate the scan time
        _scanTime = (Date.Now - t0ScanTime).TotalMilliseconds
        t0ScanTime = Date.Now

        ' Record the maximum scan time
        If (_phase <> ePhase.WaitRecipeSelection And
            _phase <> ePhase.LoadRecipe And
            _phase <> ePhase.WaitStartTest And
            _phase <> ePhase.ClearResults And
            _phase <> ePhase.WriteResults And
            _scanTime > _maxScanTime) Then
            _maxScanTime = _scanTime
            If _maxScanTime > 50 Then Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + "WS03:_maxScanTime=" + _maxScanTime.ToString() + "Phase:" + _phase.ToString() + "SubPhase:" + _subPhase(_phase).ToString())
            If _maxScanTime > 100 Then AddLogEntry(DateTime.Now.ToString("HH:mm:ss fff") + "WS03:_maxScanTime=" + _maxScanTime.ToString() + "Phase:" + _phase.ToString() + "SubPhase:" + _subPhase(_phase).ToString())
        End If

        ' Clear the ROOF LIN rx buffer
        If (_LinInterface.Connected) Then
            _LinInterface.ClearRxBuffer()
        End If

        ' Write the alarm flag to the PLC
        alarmFlag = False
        alarmFlag = alarmFlag Or _alarm(0)
    End Sub



    Public Function TestModeDescription(ByVal testMode As eTestMode) As String
        ' Return the test mode description
        Select Case testMode
            Case eTestMode.Debug
                TestModeDescription = "Debug"
            Case eTestMode.Local
                TestModeDescription = "Local"
            Case eTestMode.Remote
                If (_results.PartTypeNumber).Value = 0 Then
                    TestModeDescription = "Remote"
                    If (_forcePartOk) Then
                        TestModeDescription = TestModeDescription & " SE"
                    End If
                Else
                    TestModeDescription = String.Format("Remote PT {0}", _results.PartTypeNumber.Value)
                    If (_forcePartTypeOk) Then
                        TestModeDescription = TestModeDescription & " SE"
                    End If
                End If
            Case eTestMode.Analyse
                TestModeDescription = "Analyse"
            Case eTestMode.Retest
                TestModeDescription = "Re Test"
            Case eTestMode.PartType
                TestModeDescription = "Master Part"
            Case eTestMode.Learning
                TestModeDescription = "Learning Part Type"
            Case Else
                TestModeDescription = String.Format("Value {0} unknown", testMode)
        End Select
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+

    Private Sub PhaseWaitRecipeSelection()
        ' If the reference code is valid
        If (_reference <> "") Then
            ' Go to phase LoadRecipe
            _phase = ePhase.LoadRecipe
        ElseIf (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_LoadRecipeLocal) = False) Then   ' Otherwise, if the PLC load recipe local mode is cleared
            ' Go to the phase WaitStartTest
            _phase = ePhase.WaitStartTest
        End If
    End Sub



    Private Sub PhaseLoadRecipe()
        Dim e As Boolean
        Dim loadOk As Boolean
        Dim sp As Integer
        Dim i As Integer
        Static t0Phase As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Start load recipe, test mode " & LCase(TestModeDescription(_testMode)))
                ' Clear Output
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RecipeLoaded)
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RelaodRecipe)
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_Alarm)
                ' Cancel the Space
                i = InStr(_reference, " ")
                If i > 0 Then
                    _reference = Mid(_reference, 1, i - 1)
                End If
                ' If the recipe is not valid
                If (_reference = "") Then
                    ' Add a log entry
                    AddLogEntry("Error: recipe not valid")
                    ' Raise an alarm for recipe not valid
                    _alarm(eAlarm.RecipeNotValid) = True
                    mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Reference) = ""
                    ' Write the output data block
                    mWS03Ethernet.WriteOutputDataBlock()


                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                ElseIf (cWS03Recipe.Exists(_reference)) Then
                    ' If the recipe loading fails
                    If (_recipe.Load(_reference)) Then
                        ' Add a log entry
                        AddLogEntry(String.Format("Error while loading the recipe ""{0}""", _reference))
                        ' Raise an alarm for error loading the recipe
                        _alarm(eAlarm.ErrorLoadingRecipe) = True
                        mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Reference) = ""
                        ' Write the output data block
                        mWS03Ethernet.WriteOutputDataBlock()
                        ' Go to the phase WaitStartTest
                        _phase = ePhase.WaitStartTest
                    Else    ' Otherwise, if the recipe loading succeeds
                        ' Add a log entry
                        AddLogEntry(String.Format("Recipe ""{0}"" loading succeeded", _reference))
                        If (_HardwareEnabled_PLC) Then
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Test_Enable_FLPUSH) = _recipe.TestEnable_Front_Left_PUSH_Electrical.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Test_Enable_FLPULL) = _recipe.TestEnable_Front_Left_PULL_Electrical.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Test_Enable_FRPUSH) = _recipe.TestEnable_Front_Right_PUSH_Electrical.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Test_Enable_FRPULL) = _recipe.TestEnable_Front_Right_PULL_Electrical.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Test_Enable_Children) = _recipe.TestEnable_ChildrenLock_Electrical.Value

                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FLCorrect_X_PULL) = _recipe.X_correction_approachment_Front_Left_Pull.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FLCorrect_Y_PULL) = _recipe.Y_correction_approachment_Front_Left_Pull.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FLCorrect_Z_PULL) = _recipe.Z_correction_approachment_Front_Left_Pull.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FLZ_TOUCH_PULL) = _recipe.Z_Vector_Final_Position_Front_Left_Pull.Value

                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FLCorrect_X_PUSH) = _recipe.X_correction_approachment_Front_Left_Push.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FLCorrect_Y_PUSH) = _recipe.Y_correction_approachment_Front_Left_Push.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FLCorrect_Z_PUSH) = _recipe.Z_correction_approachment_Front_Left_Push.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FLZ_TOUCH_PUSH) = _recipe.Z_Vector_Final_Position_Front_Left_Push.Value

                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FRCorrect_X_PULL) = _recipe.X_correction_approachment_Front_Right_Pull.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FRCorrect_Y_PULL) = _recipe.Y_correction_approachment_Front_Right_Pull.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FRCorrect_Z_PULL) = _recipe.Z_correction_approachment_Front_Right_Pull.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FRZ_TOUCH_PULL) = _recipe.Z_Vector_Final_Position_Front_Right_Pull.Value

                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FRCorrect_X_PUSH) = _recipe.X_correction_approachment_Front_Right_Push.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FRCorrect_Y_PUSH) = _recipe.Y_correction_approachment_Front_Right_Push.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FRCorrect_Z_PUSH) = _recipe.Z_correction_approachment_Front_Right_Push.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.FRZ_TOUCH_PUSH) = _recipe.Z_Vector_Final_Position_Front_Right_Push.Value

                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.CL_Correct_X_PUSH) = _recipe.X_correction_approachment_ChildrenLock.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.CL_Correct_Y_PUSH) = _recipe.Y_correction_approachment_ChildrenLock.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.CL_Correct_Z_PUSH) = _recipe.Z_correction_approachment_ChildrenLock.Value
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.CL_Z_TOUCH_PUSH) = _recipe.Z_Vector_Final_Position_ChildrenLock.Value

                            ' Write the recipe data to the PLC
                            mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Reference) = _reference
                        End If
                        ' Set the load ok flag
                        loadOk = True
                    End If
                Else    ' Otherwise, if the recipe does not exists
                    ' Add a log entry
                    AddLogEntry("Error: recipe unknown")
                    ' Raise an alarm for recipe not valid
                    _alarm(eAlarm.RecipeUnknown) = True
                    mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Reference) = ""
                    ' Write the output data block
                    mWS03Ethernet.WriteOutputDataBlock()

                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                End If
                ' If the recipe loading succeeded
                If (loadOk) Then
                    If (_HardwareEnabled_PLC) Then
                        ' Write the output data block
                        e = e Or mWS03Ethernet.WriteOutputDataBlock
                        ' Set the recipe loaded bit
                        mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RecipeLoaded)
                        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RelaodRecipe)
                        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_Alarm)
                        ' Add a log entry
                        AddLogEntry("PC_RecipeLoaded = 1")
                        ' Go to subphase 1
                        _subPhase(_phase) = 1
                    Else
                        ' Clear the subphase
                        _subPhase(_phase) = 0
                        ' Go to phase WaitStartTest
                        _phase = ePhase.WaitStartTest
                    End If
                Else
                    ' Set the recipe Alarm Bit
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RecipeLoaded)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RelaodRecipe)
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_Alarm)
                    ' Add a log entry
                    AddLogEntry("Error: recipe Load with error")
                    ' Raise an alarm for recipe not valid
                    _alarm(eAlarm.RecipeNotValid) = True
                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest

                End If

            Case 1
                ' If the load recipe bit was cleared by the PLC
                If (_testMode = eTestMode.Debug Or
                    (_testMode = eTestMode.Local And mDIOManager.DigitalInputStatus(eDigitalInput.WS03_LoadRecipeLocal) = False) Or
                    (_testMode = eTestMode.Remote And mDIOManager.DigitalInputStatus(eDigitalInput.WS03_LoadRecipeRemote) = False)) Then
                    ' Add a log entry
                    AddLogEntry("End load recipe - Phase last " & (Date.Now - t0Phase).TotalSeconds.ToString("0.00"))
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RecipeLoaded)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RelaodRecipe)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_Alarm)
                    ' Clear the subphase
                    _subPhase(_phase) = 0
                    ' Go to phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                End If
        End Select

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Clear the subphase
            _subPhase(_phase) = 0
            ' Go to phase WaitStartTest
            _phase = ePhase.WaitStartTest
        End If
    End Sub



    Private Sub PhaseWaitStartTest()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' If there are no alarms
                For i = 0 To eAlarm.Count - 1
                    If (_alarm(i)) Then
                        Exit For
                    End If
                Next i
                If (i = eAlarm.Count) And (_HardwareEnabled_PLC) Then
                    ' If the PLC load recipe remote mode bit is set
                    If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_LoadRecipeRemote)) Then
                        'Read Ethernet
                        mWS03Ethernet.ReadInputDataBlock()
                        ' Set the test mode to remote
                        _testMode = eTestMode.Remote
                        ' Set the reference and the reference type
                        _reference = mWS03Ethernet.InputValue(mWS03Ethernet.eInput.Reference)
                        ' Go to the phase LoadRecipe
                        _phase = ePhase.LoadRecipe
                        Exit Sub
                        ' Otherwise, if the PLC load recipe local mode is set
                    ElseIf (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_LoadRecipeLocal)) Then
                        ' Set the test mode to local
                        _testMode = eTestMode.Local
                        ' Clear the reference code
                        _reference = ""
                        ' Go to the phase WaitRecipeSelection
                        _phase = ePhase.WaitRecipeSelection
                        Exit Sub
                    End If
                    ' If one of the test enables is set or the start flag is set
                    If (_phase = ePhase.WaitStartTest And _reference <> "" And
                        (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_TestEnable) Or _start)) Then
                        ' If the start flag is active
                        If (_start = True) Then
                            ' Clear the start flag
                            _start = False
                            ' Set the test mode to debug
                            _testMode = eTestMode.Debug
                        Else
                            ' Set the test mode to debug
                            _testMode = eTestMode.Remote
                        End If
                        ' Clear the log
                        _log.Clear()
                        ' Add a log entry
                        AddLogEntry("Start test, test mode " & LCase(TestModeDescription(_testMode)) & vbCrLf)
                        ' Clear the subphases
                        For i = 0 To ePhase.Count - 1
                            _subPhase(i) = 0
                        Next
                        ' Store the test initial time
                        _t0Test = Date.Now
                        ' Clear the maximum scan time
                        _maxScanTime = 0
                        ' Go to the phase ClearResults
                        _phase = ePhase.ClearResults
                    End If
                ElseIf TestMode = eTestMode.Debug And _reference <> "" And _start Then
                    _start = False
                    ' Clear the subphases
                    For i = 0 To ePhase.Count - 1
                        _subPhase(i) = 0
                    Next
                    ' Go to the phase ClearResults
                    _phase = ePhase.ClearResults
                End If
        End Select

        ' If a runtime error occured
        If (e) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub



    Private Sub PhaseClearResults()
        Dim e As Boolean
        Dim sp As Integer
        Static t0Phase As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                On Error Resume Next
                ' Store the phase initial time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin clear results")
                '
                mWS03Ethernet.ReadInputDataBlock()
                '
                'HbmReset = False
                ' Set the force sensors reset signals
                mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_ResetForce)
                ' Read the input data block from the PLC
                If (_HardwareEnabled_PLC) Then
                    '
                    mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.ResultCode) = cWS03Results.eTestResult.Unknown
                    '
                    mWS03Ethernet.WriteOutputDataBlock()
                End If
                ' Clear the results
                ClearResults()
                ' Clear the OK digital outputs
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_TestOk)
                ' Go to the subphase 1
                _subPhase(_phase) = 1

            Case 1
                ' If the step in progress is false or the test mode is debug
                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = False And
                   (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
                    _step = False
                    ' Sets the start step output
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Go to the subphase 2
                    _subPhase(_phase) = 2
                End If

            Case 2
                ' If the step in progress is set or the test mode is debug
                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = True And
                   (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
                    ' Clear the start step output
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Go to the subphase 3
                    _subPhase(_phase) = 3
                End If

            Case 3
                ' If the step in progress is cleared or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = False Or _testMode = eTestMode.Debug) Then
                    AddLogEntry(String.Format("End clear results - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                    '
                    _subPhase(_phase) = 0
                    '
                    If (_testMode = eTestMode.Debug AndAlso Simulation_Test) Then
                        _phase = ePhase.FrontLeft
                    Else
                        _phase = ePhase.Power_UP
                    End If

                End If

        End Select

        ' If a runtime error occured
        If (e) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub



    Private Sub PhaseWriteResults()
        Dim e As Boolean
        Dim filename As String
        Dim sp As Integer
        Dim Wili_TMP As Boolean = True
        Static t0 As Date
        Static t0Phase As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)

        Select Case _subPhase(_phase)
            Case 0
                ' Create the saving directory
                If Not (Directory.Exists(mConstants.BasePath & _settings.ResultsPath & "\" & Format(Date.Now, "yyyyMMdd"))) Then
                    Directory.CreateDirectory(mConstants.BasePath & _settings.ResultsPath & "\" & Format(Date.Now, "yyyyMMdd"))
                End If
                ' Store the phase entry time
                t0Phase = Date.Now
                ' If the reference type is barrette
                ' Add a log entry
                AddLogEntry("Begin write results")
                ' If the part type is the good part type 
                If (_results.PartTypeNumber.Value = 20) Then
                    ' Check the maste reference
                    If Not (_forcePartTypeOk) Then
                        If ((_results.TestResult = cWS03Results.eTestResult.Unknown) Or _forcePartOk) And Not Wili_TMP Then
                            ' Check the master reference
                            If CheckMasterReference() = True Then
                                _results.TestResult = cWS03Results.eTestResult.FailedMasterReference
                                For i = 0 To cWS03Results.SingleTestCount - 1
                                    If mWS03Main.Results.Value(cWS03Results.SingleTestBaseIndex + i).TestResult <> cResultValue.eValueTestResult.Disabled Then
                                        mWS03Main.Results.Value(cWS03Results.SingleTestBaseIndex + i).TestResult = cResultValue.eValueTestResult.NotCoherent
                                    End If
                                Next
                            End If
                        End If
                    Else
                        _results.TestResult = cWS03Results.eTestResult.Passed
                    End If
                ElseIf (_results.PartTypeNumber.Value > 20) And Wili_TMP Then
                    '_results.TestResult = cWS03Results.eTestResult.Passed
                End If
                ' If the global test result is unknown or passed or forced ok
                If (_results.TestResult = cWS03Results.eTestResult.Unknown Or
                    _results.TestResult = cWS03Results.eTestResult.Passed Or
                    (_results.TestResult <> cWS03Results.eTestResult.NotTested And
                     (_forcePartOk And _results.PartTypeNumber.Value <> 20))) Then
                    ' Set the global test result to passed
                    _results.TestResult = cWS03Results.eTestResult.Passed
                    ' Set the OK output
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_TestOk)
                    ' Increase the counter of passed parts
                    _counterPassed = _counterPassed + 1
                Else
                    ' Increase the counter of failed parts
                    _counterFailed = _counterFailed + 1
                End If
                If (_HardwareEnabled_PLC) Then
                    ' Write the result code to the PLC
                    mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.ResultCode) = _results.TestResult
                    '
                    mWS03Ethernet.WriteOutputDataBlock()
                End If
                mGlobal.FormDUTStats.Addnewitem(_results.PartUniqueNumber.Value, _results.TestTime.Value, 3, _results.TestResult, _results.RecipeName.Value)
                ' Save the results
                filename = mConstants.BasePath &
                            _settings.ResultsPath &
                            "\" & Format(Date.Now, "yyyyMMdd") &
                            "\WS03" & _results.RecipeName.Value
                filename = filename & "_" &
                           Mid(_results.TestDate.Value, 7, 4) &
                           Mid(_results.TestDate.Value, 4, 2) &
                           Mid(_results.TestDate.Value, 1, 2)
                filename = filename & "_" &
                                           Mid(_results.TestTime.Value, 1, 2) &
                                           Mid(_results.TestTime.Value, 4, 2) &
                                           Mid(_results.TestTime.Value, 7, 2)
                filename = filename & "_" &
                                           _results.PartUniqueNumber.Value
                filename = filename & "_" &
                                          Format(_results.TestResults.Value, "0000")
                filename = filename & ".txt"
                e = e Or _results.Save(filename, _results)
                ' Append the log to the results
                My.Computer.FileSystem.WriteAllText(filename,
                                                    vbCrLf &
                                                    "---------------------------------------------------------------------------------------------------------" &
                                                    vbCrLf &
                                                    "-                                  Log Frame LIN                                                         -" &
                                                    vbCrLf &
                                                    "----------------------------------------------------------------------------------------------------------" &
                                                    vbCrLf &
                                                    _log.ToString,
                                                    True)
                My.Computer.FileSystem.CopyFile(filename, mResults.FTPLocalFilePath + filename.Replace(mConstants.BasePath &
                            _settings.ResultsPath & "\", ""), True)
                ' Store the time
                t0 = Date.Now
                ' Go to subphase 1
                _subPhase(_phase) = 1
                ''disable the force result
                '_forcePartOk = False
                '_forcePartTypeOk = False
                mUserManager.Logout()

                On Error GoTo 0

            Case 1
                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = False And
                    (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
                    _step = False
                    ' Set the start step output
                    e = mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Go to subphase 2
                    _subPhase(_phase) = 2
                End If

            Case 2
                ' If the step in progress is set or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = True Or _testMode = eTestMode.Debug) Then
                    ' Reset the start step output
                    e = mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)

                    ' Go to subphase 3
                    _subPhase(_phase) = 3
                End If

            Case 3
                ' If the step in progress is cleared or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = False Or _testMode = eTestMode.Debug) Then
                    ' Add a log entry
                    AddLogEntry("End write results - Test last: " & (Date.Now - t0Phase).TotalSeconds.ToString("0.00") & " s" & vbCrLf)
                    _subPhase(_phase) = 0
                    ' Go to phase WaitEndTest
                    _phase = ePhase.WaitEndTest
                End If


        End Select

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub



    Private Sub PhaseWaitEndTest()
        Dim e As Boolean
        Dim sp As Integer
        Static t0Phase As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)

        ' Manage the subphases
        Select Case _subPhase(_phase)
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin wait end test")
                ' Go to subphase 1
                _subPhase(_phase) = 1
                mWS03Ethernet.OutputValue(mWS03Ethernet.eOutput.Test_Enable_PLC) = False
                mWS03Ethernet.WriteOutputDataBlock()


            Case 1
                ' If the test enable inputs are cleared
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_TestEnable) = False) Then
                    ' Add a log entry
                    AddLogEntry("End wait end test - Phase last: " & (Date.Now - t0Phase).TotalSeconds.ToString("0.00") & " s" & vbCrLf)
                    ' Go to phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                    Console.WriteLine("WS03 Finished Test!")
                End If
        End Select

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub



    Private Sub PhaseAbortTest()
        Dim e As Boolean
        Dim sp As Integer
        Dim filename As String
        Static t0Phase As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin abort test")
                ' Reset all the digital outputs
                e = False
                ' Power-down
                For j = 0 To mWS03DIOManager.eDigitalOutput.Count - 1
                    e = e Or mWS03DIOManager.ResetDigitalOutput(j)
                Next
                e = mWS03DIOManager.SetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_LocalSensing)
                e = e Or mWS03DIOManager.ResetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_RemoteSensing)
                ' Reset all the digital outputs toward the PLC
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_TestOk)
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                ' Power-down the LIN interface
                e = e Or _LinInterface.PowerDown()

                ' Create the saving directory
                If Not (Directory.Exists(mConstants.BasePath & _settings.ResultsPath & "\" & Format(Date.Now, "yyyyMMdd"))) Then
                    Directory.CreateDirectory(mConstants.BasePath & _settings.ResultsPath & "\" & Format(Date.Now, "yyyyMMdd"))
                End If

                ' Save the results
                filename = mConstants.BasePath &
                            _settings.ResultsPath &
                            "\" & Format(Date.Now, "yyyyMMdd") &
                            "\WS03" & _results.RecipeName.Value
                filename = filename & "_" &
                           Mid(_results.TestDate.Value, 7, 4) &
                           Mid(_results.TestDate.Value, 4, 2) &
                           Mid(_results.TestDate.Value, 1, 2)
                filename = filename & "_" &
                                           Mid(_results.TestTime.Value, 1, 2) &
                                           Mid(_results.TestTime.Value, 4, 2) &
                                           Mid(_results.TestTime.Value, 7, 2)
                filename = filename & "_" &
                                           _results.PartUniqueNumber.Value
                filename = filename & "_" &
                                          Format(_results.TestResults.Value, "0000")
                filename = filename &
                ".txt"
                e = e Or _results.Save(filename, _results)
                ' Append the log to the results
                My.Computer.FileSystem.WriteAllText(filename,
                                                    vbCrLf &
                                                    "---------------------------------------------------------------------------------------------------------" &
                                                    vbCrLf &
                                                    "-                                  Log Frmae  LIN                                                   -" &
                                                    vbCrLf &
                                                    "----------------------------------------------------------------------------------------------------------" &
                                                    vbCrLf &
                                                    _log.ToString,
                                                    True)

                My.Computer.FileSystem.CopyFile(filename, mResults.FTPLocalFilePath + filename.Replace(mConstants.BasePath & _settings.ResultsPath & "\", ""), True)

                ' Goes to phase WaitEndTest
                _phase = ePhase.WaitEndTest
        End Select

        ' If a runtime error occured
        If (e) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub

    Private Sub PhaseTesteurPresent()
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Dim f As CLINFrame
        Static t0 As Date
        Static t0Phase As Date
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Send Testeur Present")
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 1

            Case 1
                '
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TesterPresent),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 2

            Case 2
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_TesterPresent))
                If (i <> -1) Then
                    ' 
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - t0).TotalMilliseconds > 200) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If


            Case 100

                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = PhaseBefore_TP

            Case 199
                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David WILI
        End If

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If
    End Sub

    Private Sub PhasePowerUp()
        Dim e As Boolean
        Dim sp As Integer
        Dim sample(0 To mWS03AIOManager.eAnalogInput.Count - 1) As Double
        Static t0 As Date
        Static t0Phase As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' If the step in progress is false or the test mode is debug
                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = False And
                   (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
                    _step = False
                    '' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin power-up")
                    ' Connect the Lin Bus
                    e = e Or mWS03DIOManager.SetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_Lin_Bus)
                    ' Connect the power supply
                    e = e Or mWS03DIOManager.SetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_OnPowerSupply)
                    ' Connect the remote sensing
                    e = e Or mWS03DIOManager.SetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_RemoteSensing)
                    ' Disconnect the local sensing
                    e = e Or mWS03DIOManager.ResetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_LocalSensing)
                    ' Connect Jama Function
                    'e = e Or mWS03DIOManager.SetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_Jama_UP)
                    e = e Or mWS03DIOManager.SetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_Jama_DN)

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 1
                End If

            Case 1
                ' Sample the analog inputs
                e = e Or mWS03AIOManager.SingleSample(sample)
                ' If the voltage is OK or the test delay 1 expired
                If ((sample(mWS03AIOManager.eAnalogInput.WS03_VBAT) >= _recipe.StdSign_Powersupply(0).MinimumLimit And
                    sample(mWS03AIOManager.eAnalogInput.WS03_VBAT) <= _recipe.StdSign_Powersupply(0).MaximumLimit) And
                        (Date.Now - t0).TotalMilliseconds >= PowerUpDelay_ms) Then
                    _results.Power_supply_voltage.Value = sample(mWS03AIOManager.eAnalogInput.WS03_VBAT)
                    _results.Power_supply_Normal_Current.Value = sample(mWS03AIOManager.eAnalogInput.WS03_IBAT) * 1000
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - t0).TotalMilliseconds > 500) Then
                    ' Go to subphase
                    _subPhase(_phase) = 100
                End If

            Case 100
                ' Tests
                If (_results.Power_supply_voltage.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Power_supply_Normal_Current.Test <> cResultValue.eValueTestResult.Passed) And
                    _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Failed
                Else
                    _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                    _results.ePOWER_UP.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedPowerUP
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Init_Communication

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on Analog")
                ' Update the test result
                _results.ePOWER_UP.TestResult =
                    cResultValue.eValueTestResult.Failed
                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David 
        End If
        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub


    Private Sub PhaseInit_Communication()
        Dim e As Boolean
        Dim sp As Integer
        Static t0 As Date
        Static t0Phase As Date
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                t0Phase = Date.Now
                ' Start Lin communication
                e = _LinInterface.PowerUp(_settings.LINInterface, _settings.LINBaudrate, True, cLINInterface.eScheduleData.SCHEDULE_Null)
                If (e = False) Then
                    ' Store the time
                    t0 = Date.Now
                    AddLogEntry("Lin Session Open succeeded")
                    ' Go to subphase
                    _subPhase(_phase) = 100
                Else
                    AddLogEntry("Lin Session Open failed")
                    ' Clear Error
                    e = False
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 100
                ' Update the test result
                If _results.eINIT_ROOF_LIN_COMMUNICATION.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eINIT_ROOF_LIN_COMMUNICATION.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                    _results.eINIT_ROOF_LIN_COMMUNICATION.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedInitCommunication
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.PowerDown
                Else
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.Open_DIAGonLinSession
                End If

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout ")
                ' Update the test result
                _results.eINIT_ROOF_LIN_COMMUNICATION.TestResult =
                    cResultValue.eValueTestResult.Failed
                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False
        End If

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If

    End Sub


    Private Sub PhaseOpen_DIAGonLINSession()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim s As String
        Dim f As CLINFrame
        Static t0 As Date
        Static t0Phase As Date
        Static tLin As Date
        Static KeyAccess(0 To 5) As String

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp

            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Open Diag on Lin Session ")
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1

            Case 1
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Diag_1060),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1


            Case 2
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Diag_1060))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(0)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Set the flag of LIN timeout
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Diag_1060),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If


            Case 3
                ' Read Key
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ReadKey),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1

            Case 4
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ReadKey))
                If (i <> -1) Then
                    KeyAccess(0) = _LinInterface.RxFrame(i).Data(4) &
                                    _LinInterface.RxFrame(i).Data(5) &
                                    _LinInterface.RxFrame(i).Data(6) &
                                    _LinInterface.RxFrame(i).Data(7)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    KeyAccess(1) = Right("00000000" & Hex(CStr((CDbl("&hFFFFFFFF") - CDbl("&h" & KeyAccess(0))) + 1)), 8)
                    KeyAccess(2) = Mid(KeyAccess(1), 1, 2)
                    KeyAccess(3) = Mid(KeyAccess(1), 3, 2)
                    KeyAccess(4) = Mid(KeyAccess(1), 5, 2)
                    KeyAccess(5) = Mid(KeyAccess(1), 7, 2)

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Set the flag of LIN timeout
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ReadKey),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 5
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_SendKey).DeepClone
                f.Data(4) = KeyAccess(2)
                f.Data(5) = KeyAccess(3)
                f.Data(6) = KeyAccess(4)
                f.Data(7) = KeyAccess(5)

                ' Send Key
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1

            Case 6
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_SendKey))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Set the flag of LIN timeout
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_SendKey).DeepClone
                    f.Data(4) = KeyAccess(2)
                    f.Data(5) = KeyAccess(3)
                    f.Data(6) = KeyAccess(4)
                    f.Data(7) = KeyAccess(5)

                    ' Send Key
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 7
                ' Open Diag 1070
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1

            Case 8
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_OpenDiag_1070))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) = 10
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Set the flag of LIN timeout
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 10
                If (_recipe.TestEnable_EV_Option.Value) Then
                    ' Switch-off the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF_EV),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Go to next subphase
                    _subPhase(_phase) = 15
                Else
                    ' Switch-off the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 11
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_All_OFF))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Switch-off the backlight
                    'e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF), _
                    '                                    True, _
                    '                                    txData_MasterReq, _
                    '                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                    '                                    True, _
                    '                                    True)
                    '
                    tLin = Date.Now
                End If
            Case 15
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight2_EV),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 11
                End If

            Case 100
                ' Tests
                ' Update the test result
                If _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                    _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedOpenDIAGonLINSession
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.PowerDown
                Else
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.MMS_Traceability
                End If


            Case 199
                ' Lin                     
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult =
                    cResultValue.eValueTestResult.Failed
                ' Go to next subphase
                _subPhase(_phase) = 100

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David DBE
        End If

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If

    End Sub


    Private Sub PhaseMMSTraceability()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
        Dim LinTimeout As Boolean
        Static t0 As Date
        Static t0Phase As Date
        Static s As String
        Static frameIndex As Integer
        Static tLin As Date
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Read Valeo Serial Number ")
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 1

            Case 1
                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_Tracea),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 2
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                frameIndex = -1

            Case 2
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_MMS_R_Tracea))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(6) &
                        _LinInterface.RxFrame(i).Data(7)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' init index Frame
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) And frameIndex = -1 Then
                    '
                    tLin = Date.Now
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_Tracea),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)

                End If

                Do
                    ' If a CAN frame was received
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_FlowCtrl).DeepClone
                    For i = 1 To 7
                        f.Data(i) = "XX"
                    Next i
                    f.Data(1) = CStr(frameIndex)
                    ' Check Response
                    i = _LinInterface.RxFrameIndex(f)
                    If (i <> -1) Then
                        ' Store the values
                        s = s & _LinInterface.RxFrame(i).Data(2) &
                                _LinInterface.RxFrame(i).Data(3) &
                                _LinInterface.RxFrame(i).Data(4) &
                                _LinInterface.RxFrame(i).Data(5) &
                                _LinInterface.RxFrame(i).Data(6) &
                                _LinInterface.RxFrame(i).Data(7)
                        ' Delete the frame
                        _LinInterface.DeleteRxFrame(i)
                        ' Increase the frame index or go to subphase 100
                        If (frameIndex < 23) Then
                            frameIndex = frameIndex + 1
                            t0 = Date.Now
                        Else
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            '
                            _results.Valeo_Serial_Number.Value = Mid(s, 25, 6) & Mid(s, 35, 4)
                            '
                            _subPhase(_phase) = 10
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        ' Set the flag of CAN timeout
                        LinTimeout = True
                        _subPhase(_phase) = 10
                    End If
                Loop Until (i = -1)

            Case 10
                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_TestByte),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 11
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_MMS_R_TestByte))
                If (i <> -1) Then
                    _results.MMS_Test_Byte_Before.Value = _LinInterface.RxFrame(i).Data(5)
                    If _LinInterface.RxFrame(i).Data(5) = "21" Or _LinInterface.RxFrame(i).Data(5) = "27" Then
                        'WS02 OK.
                        _results.MMS_Test_Byte_Before.MinimumLimit = _LinInterface.RxFrame(i).Data(5)
                        _results.MMS_Test_Byte_Before.MaximumLimit = _LinInterface.RxFrame(i).Data(5)
                    ElseIf _LinInterface.RxFrame(i).Data(5) = "31" Or _LinInterface.RxFrame(i).Data(5) = "32" Or _LinInterface.RxFrame(i).Data(5) = "37" Then
                        'WS03 Retest.
                        _results.MMS_Test_Byte_Before.MinimumLimit = _LinInterface.RxFrame(i).Data(5)
                        _results.MMS_Test_Byte_Before.MaximumLimit = _LinInterface.RxFrame(i).Data(5)
                    Else
                        _results.MMS_Test_Byte_Before.MinimumLimit = "00"
                        _results.MMS_Test_Byte_Before.MaximumLimit = "00"
                    End If
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Go to subphase
                    _subPhase(_phase) = 100
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - tLin).TotalMilliseconds > LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_TestByte),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    '
                    tLin = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

                ' check if NRC22
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_NRC_22))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _results.eMMS_Traceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) = 100
                End If

            Case 100
                ' Tests
                If (_results.Valeo_Serial_Number.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.MMS_Test_Byte_Before.Test <> cResultValue.eValueTestResult.Passed) Then
                    _results.eMMS_Traceability.TestResult =
                        cResultValue.eValueTestResult.Failed
                Else
                    _results.eMMS_Traceability.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                    _results.eMMS_Traceability.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedCheckMMSTestByte
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.FrontLeft

            Case 199
                ' Lin                     
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eMMS_Traceability.TestResult =
                    cResultValue.eValueTestResult.Failed
                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David DBE
        End If

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If

    End Sub


    Private Sub PhaseTest(ByVal WiLi_Test As eWindows)
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Dim LinIndex As Integer
        Dim WiliResult As sWiliResult
        Dim failedOff As Boolean
        Dim failedOnCe1 As Boolean
        Dim failedOnCe2 As Boolean
        Dim WindowsLifterTest As cWS03Results.eWindowsLifterTest
        ' Offset for Stroke Sensor and Strenght Sensor to adapte Data measurement
        Dim OffsetStrokeSensor As Integer = 1
        Dim OffsetStrenghtSensor As Integer = 1

        Dim timeAfterCommutation As Integer = 10    ' f=2kHz time is 25 ms
        Dim timeBeforeCommutation As Integer = 10   ' f=2kHz time is 5 ms


        Dim f As CLINFrame
        Dim tpSample As Single
        Dim zz As Integer
        Static NB_Transition As Integer
        Static Switching(0 To 20) As Single
        Static CommutCe1(0 To 1) As Single
        Static CommutCe2(0 To 1) As Single
        Static frameIndex As Integer
        Static t0 As Date
        Static tLin As Date
        Static t0Phase As Date
        Static s As String
        Static StatusPushLin(0 To 20) As String
        Static frameExt As Boolean

        'Static TimeStart As Date
        Static OffSetTime As Double
        Static t0Lin As Date
        Static tAnalog As Date

        Static MinLevel(0 To 3) As String
        Static MaxLevel(0 To 3) As String

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0

                If ((CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) Or
                     CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value)) And WiLi_Test = eWindows.FrontLeft) Or
                    ((CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) Or
                      CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value)) And WiLi_Test = eWindows.FrontRight) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    If WiLi_Test = eWindows.FrontLeft Then
                        AddLogEntry("Begin Windows Lifter Front Left")
                    ElseIf WiLi_Test = eWindows.FrontRight Then
                        AddLogEntry("Begin Windows Lifter Front Right")
                    End If
                    ' clear data lin
                    For i = 0 To 20
                        StatusPushLin(i) = "000000"
                        Switching(i) = 0
                        NB_Transition = 0
                    Next
                    For i = 0 To 3
                        MinLevel(i) = "0000" : MaxLevel(i) = MinLevel(i)
                    Next

                    '
                    AddLogEntry("Reset Force Sensor  and Clear buffer Analog")
                    'If Not HbmReset Then
                    ' Set the force sensors reset signals
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_ResetForce)
                    'End If
                    'Clear buffer
                    mWS03AIOManager.EmptyBuffer()
                    ' Store the time
                    t0 = Date.Now
                    If Not Simulation_Test Then
                        ' Go to next subphase
                        _subPhase(_phase) += 1
                    Else
                        ' Go to next subphase
                        _subPhase(_phase) = 10
                    End If
                Else
                    ' Add a log entry
                    If WiLi_Test = eWindows.FrontLeft Then
                        AddLogEntry("Windows Lifter Front Left is disabled")
                    ElseIf WiLi_Test = eWindows.FrontRight Then
                        AddLogEntry("Windows Lifter Front Right is disabled")
                    End If
                    ' Go to next phase
                    If _phase = ePhase.FrontLeft Then
                        _phase = ePhase.FrontRight
                    ElseIf _phase = ePhase.FrontRight Then
                        _phase = ePhase.ChildrenLock
                    End If
                End If

            Case 1
                ' If the step in progress input is cleared (push activation)
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS03_StepInProgress) = False) Then ' And Not HbmReset) Or _
                    '(HbmReset And ((Date.Now - t0).TotalMilliseconds >= 300)) Then
                    ' Start sampling the analog inputs
                    e = mWS03AIOManager.StartSampling(samplingFrequency)
                    AddLogEntry("Start Analog Sample " & vbTab & "Case : " & _subPhase(_phase))
                    'Clear buffer
                    mWS03AIOManager.EmptyBuffer()
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    AddLogEntry("Trasmit Frame Start Counter Transition " & vbTab & "Case : " & _subPhase(_phase))
                    If (WiLi_Test = eWindows.FrontLeft And Not _recipe.TestEnable_Driver_Right.Value) Or
                        (WiLi_Test = eWindows.FrontRight And _recipe.TestEnable_Driver_Right.Value) Then
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Start_Haptic_DIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                        ' Go to next subphase
                        _subPhase(_phase) = 2
                    Else
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Start_Haptic_AIN).DeepClone
                        f.Data(2) = "15"
                        e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                        ' Go to next subphase
                        _subPhase(_phase) = 1000
                    End If
                    msngT0TesteurPeriodicFrame = Date.Now
                End If
                ' Store the time
                t0 = Date.Now

            Case 1000
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(1) = "21"
                    MinLevel(0) = Right("0000" & Hex(243), 4)
                    MaxLevel(0) = Right("0000" & Hex(354), 4)

                    MinLevel(1) = Right("0000" & Hex(393), 4)
                    MaxLevel(1) = Right("0000" & Hex(526), 4)

                    MinLevel(2) = Right("0000" & Hex(629), 4)
                    MaxLevel(2) = Right("0000" & Hex(681), 4)

                    MinLevel(3) = Right("0000" & Hex(778), 4)
                    MaxLevel(3) = Right("0000" & Hex(818), 4)

                    f.Data(2) = Left(MinLevel(0), 2) : f.Data(3) = Right(MinLevel(0), 2)
                    f.Data(4) = Left(MaxLevel(0), 2) : f.Data(5) = Right(MaxLevel(0), 2)

                    f.Data(6) = Left(MinLevel(1), 2) : f.Data(7) = Right(MinLevel(1), 2)
                    AddLogEntry("Treesholds : " & MinLevel(0) & " / " & MaxLevel(0))
                    AddLogEntry("Treesholds : " & MinLevel(1) & " / " & MaxLevel(1))
                    AddLogEntry("Treesholds : " & MinLevel(2) & " / " & MaxLevel(2))
                    AddLogEntry("Treesholds : " & MinLevel(3) & " / " & MaxLevel(3))
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Sssion Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 1001
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(1) = "22"
                    f.Data(2) = Left(MaxLevel(1), 2) : f.Data(3) = Right(MaxLevel(1), 2)

                    f.Data(4) = Left(MinLevel(2), 2) : f.Data(5) = Right(MinLevel(2), 2)
                    f.Data(6) = Left(MaxLevel(2), 2) : f.Data(7) = Right(MaxLevel(2), 2)

                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Sssion Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 1002
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(1) = "23"
                    f.Data(2) = Left(MinLevel(3), 2) : f.Data(3) = Right(MinLevel(3), 2)
                    f.Data(4) = Left(MaxLevel(3), 2) : f.Data(5) = Right(MaxLevel(3), 2)
                    f.Data(6) = "FF" : f.Data(7) = "FF"
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Sssion Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 2
                End If

            Case 2
                If (WiLi_Test = eWindows.FrontLeft And Not _recipe.TestEnable_Driver_Right.Value) Or
                        (WiLi_Test = eWindows.FrontRight And _recipe.TestEnable_Driver_Right.Value) Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Start_Haptic_DIN))
                Else
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Start_Haptic_AIN))
                End If
                If (i <> -1) Then
                    AddLogEntry("Ack LIN Frame " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the force sensors reset signals
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_ResetForce)
                    '
                    'HbmReset = True
                    ' Lin 
                    t0Lin = _LinInterface.ToSystemTime(CLng(_LinInterface.RxFrame(i).Timestamp))
                    AddLogEntry("TimeStamp Lin : " & Format(t0Lin, "dd/MM/yyyy, HH:mm:ss:fff") & vbTab & _LinInterface.RxFrame(i).Timestamp)
                    AddLogEntry("Difference systeme Time / lin Time : " & (Date.Now - t0Lin).TotalMilliseconds.ToString("0.00"))
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' stop Lin diag
                    _LinInterface.StopScheduleDiag()
                    ' Set the start step output (push activation)
                    AddLogEntry("Start Step to Activate the Robot")
                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then
                    ' stop Lin diag
                    _LinInterface.StopScheduleDiag()
                    ' Reset the force sensors reset signals
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_ResetForce)
                    AddLogEntry("Start Step to Activate the Robot")
                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    'Clear buffer
                    mWS03AIOManager.EmptyBuffer()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 3
                ' If the step in progress input is set (push activation)
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = True Or _testMode = eTestMode.Debug) Then
                    _step = False
                    AddLogEntry("Start Step = 0 / Step in Progress = 1 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the force sensors reset signals
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_ResetForce)
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 4
                ' If the step in progress input is cleared (push activation)
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS03_StepInProgress) = False) Then
                    AddLogEntry("Step in Progress = 0 / ReadDataBlock  transition " & vbTab & "Case : " & _subPhase(_phase))
                    ' Stop sampling the analog inputs
                    AddLogEntry("Stop Analog Sample " & vbTab & "Case : " & _subPhase(_phase))
                    e = e Or mWS03AIOManager.StopSampling
                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Start Robot for release
                    'to cancel this for reduce the Cycle Time if needed
                    AddLogEntry(" Start Robot for release")
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Store the time
                    t0 = Date.Now
                    frameIndex = &H0
                    frameExt = False
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 5
                ' If the step in progress input is set (push activation)
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = True) Then
                    _step = False
                    AddLogEntry("Start Step = 0 / Step in Progress = 1 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    If (WiLi_Test = eWindows.FrontLeft And Not _recipe.TestEnable_Driver_Right.Value) Or
                        (WiLi_Test = eWindows.FrontRight And _recipe.TestEnable_Driver_Right.Value) Then
                        ' Read Raw Data
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_DIN),
                                            True,
                                            txData_MasterReq,
                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                            True,
                                            True)
                    Else
                        ' Read Raw Data
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_AIN),
                                            True,
                                            txData_MasterReq,
                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                            True,
                                            True)
                    End If
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Go to subphase 9
                    _subPhase(_phase) += 1
                    '
                    t0 = Date.Now
                    tLin = Date.Now
                End If

            Case 6
                Try
                    ' If the correct answer was received
                    If (WiLi_Test = eWindows.FrontLeft And Not _recipe.TestEnable_Driver_Right.Value) Or
                            (WiLi_Test = eWindows.FrontRight And _recipe.TestEnable_Driver_Right.Value) Then
                        i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_RawTransition_DIN))
                    Else
                        i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_RawTransition_AIN))
                    End If
                    If (i <> -1) Then
                        tAnalog = mWS03AIOManager.TimeStampSampleBuffer(0)
                        AddLogEntry("Time Stamp Analog : " & Format(tAnalog, "dd/MM/yyyy, HH:mm:ss:fff"))
                        OffSetTime = (t0Lin - tAnalog).TotalMilliseconds
                        AddLogEntry("Difference acq AI/acq LIN : " & OffSetTime.ToString("0.00"))
                        'OffSetTime = (trLin - tAnalog).TotalMilliseconds
                        AddLogEntry("Offset Time Apply : " & (t0Lin - tAnalog).TotalMilliseconds)
                        ' Initialize the string buffer
                        NB_Transition = CInt("&h" & _LinInterface.RxFrame(i).Data(7))
                        AddLogEntry("Transition  Number Detected : " & NB_Transition)
                        If NB_Transition > 20 Then
                            NB_Transition = 20
                        End If
                        ' Delete the frame
                        _LinInterface.DeleteRxFrame(i)
                        s = ""
                        ' Initialize the frame index
                        frameIndex = 21
                        frameExt = False
                        ' Store the time
                        t0 = Date.Now
                    ElseIf ((Date.Now - tLin).TotalMilliseconds >= LINTimeout_ms) And frameIndex = 0 Then   ' Otherwise, if the LIN timeout expired
                        If (WiLi_Test = eWindows.FrontLeft And Not _recipe.TestEnable_Driver_Right.Value) Or
                            (WiLi_Test = eWindows.FrontRight And _recipe.TestEnable_Driver_Right.Value) Then
                            ' Read Raw Data
                            e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_DIN),
                                                True,
                                                txData_MasterReq,
                                                cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                True,
                                                True)
                        Else
                            ' Read Raw Data
                            e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_AIN),
                                                True,
                                                txData_MasterReq,
                                                cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                True,
                                                True)
                        End If
                        '
                        tLin = Date.Now
                    ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then   ' Otherwise, if the LIN timeout expired
                        ' Go to subphase 600
                        _subPhase(_phase) += 1
                    End If

                    If (frameIndex > 0) Then
                        Do
                            ' Builds the expected frame
                            f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_FlowCtrl).DeepClone
                            For i = 1 To 7
                                f.Data(i) = "XX"
                            Next i
                            f.Data(1) = "XX" ' frameIndex
                            ' If the frame was received
                            i = _LinInterface.RxFrameIndex(f)
                            If (i <> -1) Then
                                ' Store the values
                                s = s & _LinInterface.RxFrame(i).Data(2) &
                                        _LinInterface.RxFrame(i).Data(3) &
                                        _LinInterface.RxFrame(i).Data(4) &
                                        _LinInterface.RxFrame(i).Data(5) &
                                        _LinInterface.RxFrame(i).Data(6) &
                                        _LinInterface.RxFrame(i).Data(7)
                                ' Delete the frame
                                _LinInterface.DeleteRxFrame(i)
                                t0 = Date.Now
                            ElseIf ((Date.Now - t0).TotalMilliseconds >= 50) Then ' _LINTimeout_ms) Then
                                If (WiLi_Test = eWindows.FrontLeft And Not _recipe.TestEnable_Driver_Right.Value) Or
                                    (WiLi_Test = eWindows.FrontRight And _recipe.TestEnable_Driver_Right.Value) Then
                                    AddLogEntry("Raw Data : " & s)
                                    For LinIndex = 0 To NB_Transition - 1
                                        ' initiate position
                                        StatusPushLin(LinIndex) = (Mid(s, (6 * LinIndex) + 1, 2))
                                        If StatusPushLin(LinIndex) = "FE" OrElse StatusPushLin(LinIndex) = "FC" Then
                                            StatusPushLin(LinIndex) = "FF"
                                        ElseIf StatusPushLin(LinIndex) = "DE" OrElse StatusPushLin(LinIndex) = "DC" Then
                                            StatusPushLin(LinIndex) = "FE"
                                        ElseIf StatusPushLin(LinIndex) = "D6" OrElse StatusPushLin(LinIndex) = "D4" Then
                                            StatusPushLin(LinIndex) = "FC"
                                        ElseIf StatusPushLin(LinIndex) = "EE" OrElse StatusPushLin(LinIndex) = "EC" Then
                                            StatusPushLin(LinIndex) = "FB"
                                        ElseIf StatusPushLin(LinIndex) = "E6" OrElse StatusPushLin(LinIndex) = "E4" Then
                                            StatusPushLin(LinIndex) = "F3"
                                        End If
                                        ' Add Delay for lin communication
                                        Dim str = "&h" & Mid(s, (6 * LinIndex) + 3, 4)
                                        Switching(LinIndex) = CSng(CSng("&h" & Mid(s, (6 * LinIndex) + 3, 4)) + OffSetTime) ' temporaire
                                        AddLogEntry("Transition  Number " & LinIndex + 1 & vbTab & "Status : " & StatusPushLin(LinIndex) & vbTab & "Timming : " & Switching(LinIndex) & vbTab & "Timming Real: " & CSng("&h" & Mid(s, (6 * LinIndex) + 3, 4)))
                                    Next
                                Else
                                    AddLogEntry("Raw Data : " & s)
                                    For LinIndex = 0 To NB_Transition - 1
                                        ' initiate position
                                        StatusPushLin(LinIndex) = (Mid(s, (8 * LinIndex) + 1, 4))
                                        ' Add Delay for lin communication
                                        Dim str = "&h" & Mid(s, (8 * LinIndex) + 5, 4)
                                        Switching(LinIndex) = CSng(CSng("&h" & Mid(s, (8 * LinIndex) + 5, 4)) + OffSetTime) ' temporaire
                                        AddLogEntry("Transition  Number " & LinIndex + 1 & vbTab & "Status : " & StatusPushLin(LinIndex) & vbTab & "Timming : " & Switching(LinIndex) & vbTab & "Timming Real: " & CSng("&h" & Mid(s, (8 * LinIndex) + 5, 4)))
                                    Next
                                    ' Check Transition Detection and transfor the Raw data for Haptic test
                                    Dim UP_M As Boolean
                                    Dim UP_A As Boolean
                                    Dim DN_M As Boolean
                                    Dim DN_A As Boolean
                                    Dim UPDN As Boolean
                                    Dim SIndex As Integer
                                    UPDN = False
                                    SIndex = 0
                                    UP_A = False : UP_M = False : DN_A = False : DN_M = False
                                    If Mid(StatusPushLin(SIndex), 3, 2) = "88" Then
                                        StatusPushLin(SIndex) = "FF"
                                        SIndex += 1
                                    End If

                                    For LinIndex = 1 To NB_Transition - 1
                                        If Mid(StatusPushLin(LinIndex), 3, 2) = "88" Then
                                            UPDN = True
                                            StatusPushLin(SIndex) = "FF" : Switching(SIndex) = Switching(LinIndex)
                                            SIndex += 1
                                        Else
                                            If Not UPDN Then
                                                If CInt("&h" & Mid(StatusPushLin(LinIndex), 3, 1)) = 7 AndAlso Not DN_M Then
                                                    DN_M = True
                                                    StatusPushLin(SIndex) = "FE" : Switching(SIndex) = Switching(LinIndex)
                                                    SIndex += 1
                                                ElseIf CInt("&h" & Mid(StatusPushLin(LinIndex), 3, 1)) = 5 AndAlso Not DN_A AndAlso DN_M Then
                                                    DN_A = True
                                                    StatusPushLin(SIndex) = "FC" : Switching(SIndex) = Switching(LinIndex)
                                                    SIndex += 1
                                                End If
                                            Else
                                                If CInt("&h" & Mid(StatusPushLin(LinIndex), 3, 1)) = 3 AndAlso Not UP_M Then
                                                    UP_M = True
                                                    StatusPushLin(SIndex) = "FB" : Switching(SIndex) = Switching(LinIndex)
                                                    SIndex += 1
                                                ElseIf CInt("&h" & Mid(StatusPushLin(LinIndex), 3, 1)) = 1 AndAlso Not UP_A AndAlso UP_M Then
                                                    UP_A = True
                                                    StatusPushLin(SIndex) = "F3" : Switching(SIndex) = Switching(LinIndex)
                                                    SIndex += 1
                                                End If
                                            End If
                                        End If
                                    Next
                                    '
                                    For LinIndex = SIndex To 20
                                        StatusPushLin(LinIndex) = "00" : Switching(LinIndex) = 0
                                    Next
                                    For LinIndex = 0 To NB_Transition - 1
                                        'OffSetTime = 0
                                        AddLogEntry("Transition  Number After Filtering" & LinIndex + 1 & vbTab & "Status : " & StatusPushLin(LinIndex) & vbTab & "Timming : " & Switching(LinIndex) & vbTab & "Timming Real: " & CSng("&h" & Mid(s, (8 * LinIndex) + 5, 4)))
                                    Next
                                End If
                                ' Lin Schedulle
                                _LinInterface.StopScheduleDiag()
                                ' Set the flag of CAN timeout
                                _subPhase(_phase) += 1
                            End If
                        Loop Until (i = -1)
                    End If
                Catch ex As Exception
                    Dim Error_scan As String
                    Error_scan = Err.Description
                    AddLogEntry("Sub Phase " & sp & " Error : " & Error_scan)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Set the flag of CAN timeout
                    _subPhase(_phase) += 1

                End Try

            Case 7
                If (WiLi_Test = eWindows.FrontLeft And Not _recipe.TestEnable_Driver_Right.Value) Or
                        (WiLi_Test = eWindows.FrontRight And _recipe.TestEnable_Driver_Right.Value) Then
                    ' Stop Timing Lin
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Stop_Haptic_DIN),
                                        True,
                                        txData_MasterReq,
                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                        True,
                                        True)
                Else
                    ' Stop Timing Lin
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Stop_Haptic_AIN),
                                        True,
                                        txData_MasterReq,
                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                        True,
                                        True)
                End If
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                ' Go to subphase 10
                _subPhase(_phase) += 1

            Case 8
                ' If the correct answer was received
                If (WiLi_Test = eWindows.FrontLeft And Not _recipe.TestEnable_Driver_Right.Value) Or
                        (WiLi_Test = eWindows.FrontRight And _recipe.TestEnable_Driver_Right.Value) Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Stop_Haptic_DIN))
                Else
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Stop_Haptic_AIN))
                End If
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Go to subphase 8
                    _subPhase(_phase) = 10
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) = 10
                End If

            Case 10
                Try
                    If Simulation_Test Then
                        frmProduction.OpenFileDialog1.ShowDialog()
                        mWS03Main.FilePoint = frmProduction.OpenFileDialog1.FileName
                        Read_AnalogPoint(FilePoint, False, WiLi_Test)
                    Else
                        ' initialize Step
                        LinIndex = 0
                        ' Store the sample count
                        _results.SampleCount_TMP = mWS03AIOManager.SampleCount
                        If _results.SampleCount_TMP > 9999 Then
                            _results.SampleCount_TMP = 9999
                        End If
                        If (_results.SampleCount_TMP > cWS03Results.MaxSampleCount) Then
                            _results.SampleCount_TMP = cWS03Results.MaxSampleCount
                        End If

                        ' Store the samples
                        For sampleIndex = 0 To _results.SampleCount_TMP - 1
                            ' Early Sensor
                            _results.Sample_TMP(eSignal_Analog.EarlySensor, sampleIndex) =
                                mWS03AIOManager.SampleBuffer(mWS03AIOManager.eAnalogInput.WS03_EarlySensor, sampleIndex) * OffsetStrokeSensor
                            ' Strenght Sensor
                            _results.Sample_TMP(eSignal_Analog.StrenghtSensor, sampleIndex) =
                                mWS03AIOManager.SampleBuffer(mWS03AIOManager.eAnalogInput.WS03_StrenghtSensor, sampleIndex) *
                                OffsetStrenghtSensor
                            ' Calul the timestamp for synchronisation
                            tpSample = sampleIndex * ((1 / samplingFrequency) * 1000)
                            ' Completed the table of datas relative to the index
                            If tpSample >= Switching(LinIndex + 1) AndAlso Switching(LinIndex + 1) > 0 Then
                                LinIndex = LinIndex + 1
                            End If
                            ' Windows Lifter Push Manuel
                            _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex) =
                                CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
                            ' Windows Lifter Push Automatic
                            _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex) =
                                CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 1) And &H1)))) * 13.5
                            ' Windows Lifter Pull Manual
                            _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex) =
                                CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 2) And &H1)))) * 13.5
                            ' Windows Lifter Pull Automatic
                            _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex) =
                                CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 3) And &H1)))) * 13.5
                            ' Jama Function
                            _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex) =
                                mWS03AIOManager.SampleBuffer(mWS03AIOManager.eAnalogInput.WS03_Pin13, sampleIndex)                        '
                        Next
                    End If

                    'Write in file for analyse
                    If WriteFilePoint Then
                        If Not Simulation_Test Then
                            Write_AnalogPoint(WiLi_Test, -1)
                        End If
                    End If
                    ' split le cycle UP/DN
                    Split_Data(WiLi_Test)
                    ''Write in file for analyse
                    'If WriteFilePoint Then
                    '    'If Not Simulation_Test Then
                    '    Write_AnalogPoint(WiLi_Test, 0)
                    '    'End If
                    'End If
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 11
                    _subPhase(_phase) = 20

                Catch ex As Exception
                    Dim errdes As String
                    errdes = Err.Description
                    AddLogEntry("Sub Phase 10 Error : " & errdes)
                    ' Go to subphase 11
                    _subPhase(_phase) = 20

                End Try
            Case 20
                '******************
                ' Push Windows Lifter
                '******************
                WindowsLifterTest = cWS03Results.eWindowsLifterTest.Not_Defined
                If WiLi_Test = eWindows.FrontRight And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) Then WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Push
                If WiLi_Test = eWindows.FrontLeft And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) Then WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Push

                If WindowsLifterTest <> cWS03Results.eWindowsLifterTest.Not_Defined Then
                    ' Process the electrical test samples
                    ProcessSamples_BaseTps(WindowsLifterTest, WiliResult) ', ePush_Pull.Push)
                    ' Store the commutation Early, the overstroke, the Strenght peak F1, the Early Strenght S1, the Activation Strenght F2 and the Tactile Ratio
                    _results.WL_XCe1(ePush_Pull.Push, WiLi_Test).Value = WiliResult.XCe1
                    _results.WL_XCe2(ePush_Pull.Push, WiLi_Test).Value = WiliResult.XCe2
                    _results.WL_Xe(ePush_Pull.Push, WiLi_Test).Value = WiliResult.Xe
                    ' Force
                    _results.WL_Fs1_F1(ePush_Pull.Push, WiLi_Test).Value = WiliResult.Fs1_F1
                    _results.WL_Xs1(ePush_Pull.Push, WiLi_Test).Value = WiliResult.Xs1
                    _results.WL_dFs1_Haptic_1(ePush_Pull.Push, WiLi_Test).Value = WiliResult.dFs1_Haptic_1
                    _results.WL_dXs1(ePush_Pull.Push, WiLi_Test).Value = WiliResult.dXs1
                    _results.WL_Fs2_F2(ePush_Pull.Push, WiLi_Test).Value = WiliResult.Fs2_F2
                    _results.WL_Xs2(ePush_Pull.Push, WiLi_Test).Value = WiliResult.Xs2
                    _results.WL_dFs2_Haptic_2(ePush_Pull.Push, WiLi_Test).Value = WiliResult.dFs2_Haptic_2
                    _results.WL_dXs2(ePush_Pull.Push, WiLi_Test).Value = WiliResult.dXs2
                    _results.WL_DiffS2Ce1(ePush_Pull.Push, WiLi_Test).Value = WiliResult.Value_DiffS2Ce1
                    _results.WL_DiffS5Ce2(ePush_Pull.Push, WiLi_Test).Value = WiliResult.Value_DiffS5Ce2


                    _results.WL_X_Indexes(WindowsLifterTest).X_Ce1 = WiliResult.X_Ce1_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_Ce2 = WiliResult.X_Ce2_index
                    _results.WL_X_Indexes(WindowsLifterTest).Xe = WiliResult.Xe_Index
                    _results.WL_X_Indexes(WindowsLifterTest).Xs = WiliResult.Xs_Index
                    _results.WL_X_Indexes(WindowsLifterTest).X_Fe1 = WiliResult.X_Fe1_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_F1 = WiliResult.Xs_F1_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_F2 = WiliResult.Xs_F2_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_F4 = WiliResult.Xs_F4_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_F5 = WiliResult.Xs_F5_index
                    If (SaveFSScreenshot) Then
                        Dim filename As String = WindowsLifterTest.ToString() +
                        Mid(_results.TestDate.Value.ToString, 7, 4) +
                        Mid(_results.TestDate.Value.ToString, 4, 2) +
                        Mid(_results.TestDate.Value.ToString, 1, 2) +
                        "_" + _results.PartUniqueNumber.Value.ToString
                        mResults.SaveWiLiFSCurve(3, _results.WL_Xe(ePush_Pull.Push, WiLi_Test).Value,
                                             _results.Samples_Push_Pull(WindowsLifterTest, mWS03Main.cSample_Signal.EarlySensor, _results.WL_X_Indexes(WindowsLifterTest).Xs),
                                             _results.WL_X_Indexes(WindowsLifterTest).Xs, _results.WL_X_Indexes(WindowsLifterTest).Xe,
                                             _results.Samples_Push_Pull,
                                             WindowsLifterTest,
                                             _results.WL_X_Indexes(WindowsLifterTest).X_F1,
                                             _results.WL_X_Indexes(WindowsLifterTest).X_F2,
                                             _results.WL_X_Indexes(WindowsLifterTest).X_F4,
                                             _results.WL_X_Indexes(WindowsLifterTest).X_F5,
                                             True, ePush_Pull.Push,
                                             mWS03Main.cSample_Signal.EarlySensor,
                                             mWS03Main.cSample_Signal.StrenghtSensor, filename)
                    End If
                End If
                _subPhase(_phase) += 1
            Case 21
                '******************
                ' Pull Windows Lifter
                '******************
                WindowsLifterTest = cWS03Results.eWindowsLifterTest.Not_Defined
                If WiLi_Test = eWindows.FrontRight And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) Then WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Pull
                If WiLi_Test = eWindows.FrontLeft And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) Then WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Pull

                If WindowsLifterTest <> cWS03Results.eWindowsLifterTest.Not_Defined Then
                    ' Process the electrical test samples
                    ProcessSamples_BaseTps(WindowsLifterTest, WiliResult) ', ePush_Pull.Pull)
                    ' Store the commutation Early, the overstroke, the Strenght peak F1, the Early Strenght S1, the Activation Strenght F2 and the Tactile Ratio
                    _results.WL_XCe1(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.XCe1
                    _results.WL_XCe2(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.XCe2
                    _results.WL_Xe(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.Xe
                    ' Force
                    _results.WL_Fs1_F1(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.Fs1_F1
                    _results.WL_Xs1(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.Xs1
                    _results.WL_dFs1_Haptic_1(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.dFs1_Haptic_1
                    _results.WL_dXs1(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.dXs1
                    _results.WL_Fs2_F2(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.Fs2_F2
                    _results.WL_Xs2(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.Xs2
                    _results.WL_dFs2_Haptic_2(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.dFs2_Haptic_2
                    _results.WL_dXs2(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.dXs2
                    _results.WL_DiffS2Ce1(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.Value_DiffS2Ce1
                    _results.WL_DiffS5Ce2(ePush_Pull.Pull, WiLi_Test).Value = WiliResult.Value_DiffS5Ce2

                    _results.WL_X_Indexes(WindowsLifterTest).X_Ce1 = WiliResult.X_Ce1_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_Ce2 = WiliResult.X_Ce2_index
                    _results.WL_X_Indexes(WindowsLifterTest).Xe = WiliResult.Xe_Index
                    _results.WL_X_Indexes(WindowsLifterTest).Xs = WiliResult.Xs_Index
                    _results.WL_X_Indexes(WindowsLifterTest).X_Fe1 = WiliResult.X_Fe1_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_F1 = WiliResult.Xs_F1_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_F2 = WiliResult.Xs_F2_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_F4 = WiliResult.Xs_F4_index
                    _results.WL_X_Indexes(WindowsLifterTest).X_F5 = WiliResult.Xs_F5_index
                    If (SaveFSScreenshot) Then
                        Dim filename As String = WindowsLifterTest.ToString() +
                        Mid(_results.TestDate.Value.ToString, 7, 4) +
                        Mid(_results.TestDate.Value.ToString, 4, 2) +
                        Mid(_results.TestDate.Value.ToString, 1, 2) +
                        "_" + _results.PartUniqueNumber.Value.ToString
                        mResults.SaveWiLiFSCurve(3, _results.WL_Xe(ePush_Pull.Pull, WiLi_Test).Value,
                                                 _results.Samples_Push_Pull(WindowsLifterTest, mWS03Main.cSample_Signal.EarlySensor, _results.WL_X_Indexes(WindowsLifterTest).Xs),
                                                 _results.WL_X_Indexes(WindowsLifterTest).Xs, _results.WL_X_Indexes(WindowsLifterTest).Xe,
                                                 _results.Samples_Push_Pull,
                                                 WindowsLifterTest,
                                                 _results.WL_X_Indexes(WindowsLifterTest).X_F1,
                                                 _results.WL_X_Indexes(WindowsLifterTest).X_F2,
                                                 _results.WL_X_Indexes(WindowsLifterTest).X_F4,
                                                 _results.WL_X_Indexes(WindowsLifterTest).X_F5,
                                                 False, ePush_Pull.Pull,
                                                 mWS03Main.cSample_Signal.EarlySensor,
                                                 mWS03Main.cSample_Signal.StrenghtSensor, filename)
                    End If
                End If

                _subPhase(_phase) = 100

            Case 100
                ' If the electrical test is enabled

                '--------------------------------------------
                '|   Windows Lift Push/Pull and Position    |
                '--------------------------------------------
                '*****************************
                ' Push Windows Lifter Function
                '*****************************
                WindowsLifterTest = cWS03Results.eWindowsLifterTest.Not_Defined
                If WiLi_Test = eWindows.FrontRight And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) Then WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Push
                If WiLi_Test = eWindows.FrontLeft And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) Then WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Push

                If WindowsLifterTest <> cWS03Results.eWindowsLifterTest.Not_Defined Then
                    ' Test the values before and after commutation
                    failedOff = False
                    failedOnCe1 = False
                    failedOnCe2 = False
                    For sampleIndex = _results.WL_X_Indexes(WindowsLifterTest).Xs To _results.WS03_SampleCount(WindowsLifterTest) - 1
                        If (Not (failedOff) And
                            sampleIndex < (_results.WL_X_Indexes(WindowsLifterTest).X_Ce1 - timeBeforeCommutation) Or
                                _results.WL_XCe1(ePush_Pull.Push, WiLi_Test).Value = 0) Then
                            If WiLi_Test = eWindows.FrontLeft Then
                                _results.WL_Front_Left_Push_Manual(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                _results.WL_Front_Left_Push_Automatic(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                _results.WL_Front_Left_Pull_Manual(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                _results.WL_Front_Left_Pull_Automatic(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                ' jama
                                _results.WL_Front_Left_Jama_Down(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                'Front Left
                                failedOff = failedOff Or (_results.WL_Front_Left_Push_Manual(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value))
                                failedOff = failedOff Or (_results.WL_Front_Left_Push_Automatic(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) _
                                                            And _recipe.Front_Left_Push_Number_State.Value = 2)
                                failedOff = failedOff Or (_results.WL_Front_Left_Pull_Manual(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                                failedOff = failedOff Or (_results.WL_Front_Left_Pull_Automatic(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) _
                                                            And _recipe.Front_Left_Pull_Number_State.Value = 2)
                                failedOff = failedOff Or (_results.WL_Front_Left_Jama_Down(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))

                            Else
                                _results.WL_Front_Right_Push_Manual(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                _results.WL_Front_Right_Push_Automatic(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                _results.WL_Front_Right_Pull_Manual(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                _results.WL_Front_Right_Pull_Automatic(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                _results.WL_Front_Right_Jama_Down(0, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                'Front Right
                                failedOff = failedOff Or (_results.WL_Front_Right_Push_Manual(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                                failedOff = failedOff Or (_results.WL_Front_Right_Push_Automatic(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) _
                                                            And _recipe.Front_Right_Push_Number_State.Value = 2)
                                failedOff = failedOff Or (_results.WL_Front_Right_Pull_Manual(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value))
                                failedOff = failedOff Or (_results.WL_Front_Right_Pull_Automatic(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) _
                                                            And _recipe.Front_Right_Pull_Number_State.Value = 2)
                                failedOff = failedOff Or (_results.WL_Front_Right_Jama_Down(0, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))

                            End If
                        End If

                        If (Not (failedOnCe1) And
                            (sampleIndex > (_results.WL_X_Indexes(WindowsLifterTest).X_Ce1 + timeAfterCommutation) And
                             (sampleIndex < (_results.WL_X_Indexes(WindowsLifterTest).X_Ce2 - timeBeforeCommutation) Or
                              (_recipe.Front_Left_Push_Number_State.Value < 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Push) Or
                              (_recipe.Front_Right_Push_Number_State.Value < 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Push))) Or
                                _results.WL_XCe1(ePush_Pull.Push, WiLi_Test).Value = 0) Then
                            If WiLi_Test = eWindows.FrontLeft Then
                                _results.WL_Front_Left_Push_Manual(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                _results.WL_Front_Left_Push_Automatic(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                _results.WL_Front_Left_Pull_Manual(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                _results.WL_Front_Left_Pull_Automatic(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                _results.WL_Front_Left_Jama_Down(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                ''Front Left 
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Left_Push_Manual(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value))
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Left_Push_Automatic(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) _
                                                            And _recipe.Front_Left_Push_Number_State.Value = 2)
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Left_Pull_Manual(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Left_Pull_Automatic(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) _
                                                            And _recipe.Front_Left_Pull_Number_State.Value = 2)
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Left_Jama_Down(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                            Else
                                _results.WL_Front_Right_Push_Manual(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                _results.WL_Front_Right_Push_Automatic(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                _results.WL_Front_Right_Pull_Manual(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                _results.WL_Front_Right_Pull_Automatic(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                _results.WL_Front_Right_Jama_Down(1, ePush_Pull.Push, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                'Front Right
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Right_Push_Manual(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Right_Push_Automatic(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) _
                                                            And _recipe.Front_Right_Push_Number_State.Value = 2)
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Right_Pull_Manual(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value))
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Right_Pull_Automatic(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) _
                                                            And _recipe.Front_Right_Pull_Number_State.Value = 2)
                                failedOnCe1 = failedOnCe1 Or (_results.WL_Front_Right_Jama_Down(1, ePush_Pull.Push, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                            End If
                        End If

                        If (_recipe.Front_Left_Push_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Push) Or
                            (_recipe.Front_Right_Push_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Push) Then
                            If (Not (failedOnCe2) And
                                                    sampleIndex > (_results.WL_X_Indexes(WindowsLifterTest).X_Ce2 + timeAfterCommutation) Or
                                                    _results.WL_XCe2(ePush_Pull.Push, WiLi_Test).Value = 0) Then
                                If WiLi_Test = eWindows.FrontLeft Then
                                    _results.WL_Front_Left_Push_Manual(2, ePush_Pull.Push, WiLi_Test).Value =
                                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                    _results.WL_Front_Left_Push_Automatic(2, ePush_Pull.Push, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                    _results.WL_Front_Left_Pull_Manual(2, ePush_Pull.Push, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                    _results.WL_Front_Left_Pull_Automatic(2, ePush_Pull.Push, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                    _results.WL_Front_Left_Jama_Down(2, ePush_Pull.Push, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                    'Front Left 
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Left_Push_Manual(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value))
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Left_Push_Automatic(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) _
                                                                And _recipe.Front_Left_Push_Number_State.Value = 2)
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Left_Pull_Manual(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Left_Pull_Automatic(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) _
                                                                And _recipe.Front_Left_Pull_Number_State.Value = 2)
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Left_Jama_Down(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                                Else
                                    _results.WL_Front_Right_Push_Manual(2, ePush_Pull.Push, WiLi_Test).Value =
                                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                    _results.WL_Front_Right_Push_Automatic(2, ePush_Pull.Push, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                    _results.WL_Front_Right_Pull_Manual(2, ePush_Pull.Push, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                    _results.WL_Front_Right_Pull_Automatic(2, ePush_Pull.Push, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                    _results.WL_Front_Right_Jama_Down(2, ePush_Pull.Push, WiLi_Test).Value =
                                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)
                                    'Front Right
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Right_Push_Manual(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Right_Push_Automatic(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) _
                                                                And _recipe.Front_Right_Push_Number_State.Value = 2)
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Right_Pull_Manual(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value))
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Right_Pull_Automatic(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) _
                                                                And _recipe.Front_Right_Pull_Number_State.Value = 2)
                                    failedOnCe2 = failedOnCe2 Or (_results.WL_Front_Right_Jama_Down(2, ePush_Pull.Push, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                                End If
                            End If
                        End If
                    Next
                    ' Update the single test result
                    If (failedOff Or failedOnCe1 Or failedOnCe2) And WiLi_Test = eWindows.FrontLeft And
                        _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                            _results.eFront_Left_Push_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_ElectricalTest
                        End If
                    ElseIf (failedOff Or failedOnCe1 Or failedOnCe2) And WiLi_Test = eWindows.FrontRight And
                        _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                            _results.eFront_Right_Push_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_ElectricalTest
                        End If
                    End If
                    '' Check all data is checked
                    'For i = 0 To 2
                    '    If (_results.WL_Front_Left_Pull_Manual(i, ePush_Pull.Push, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
                    '        (_results.WL_Front_Left_Pull_Automatic(i, ePush_Pull.Push, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown And _
                    '         _recipe.Front_Left_Pull_Number_State.Value = 2) Or _
                    '        _results.WL_Front_Left_Push_Manual(i, ePush_Pull.Push, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
                    '        (_results.WL_Front_Left_Push_Automatic(i, ePush_Pull.Push, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown And _
                    '        _recipe.Front_Left_Pull_Number_State.Value = 2)) Then

                    '        If WiLi_Test = eWindows.FrontLeft And _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    '            _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    '            If (_results.TestResult = cWS03Results.eTestResult.Unknown And _
                    '                _results.eFront_Left_Push_Electrical.TestResult <> _
                    '                    cResultValue.eValueTestResult.Passed) Then
                    '                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_ElectricalTest
                    '            End If
                    '        ElseIf WiLi_Test = eWindows.FrontRight And _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    '            _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    '            If (_results.TestResult = cWS03Results.eTestResult.Unknown And _
                    '                _results.eFront_Right_Push_Electrical.TestResult <> _
                    '                    cResultValue.eValueTestResult.Passed) Then
                    '                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_ElectricalTest
                    '            End If
                    '        End If
                    '    End If
                    'Next i

                    ' Check Result
                    '****************************
                    '   Front Right Push
                    '****************************
                    If (WiLi_Test = eWindows.FrontRight) Then
                        ' Test the Peak Strenght F1
                        If (_results.WL_Fs1_F1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_Xs1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Right_Push_Strenght.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_PeakF1Test
                            End If
                        End If
                        ' Test the Tactile Ratio
                        If (_results.WL_dFs1_Haptic_1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_dXs1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Right_Push_Strenght.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_F1F2Test
                            End If
                        End If
                        If (_recipe.Front_Right_Push_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Push) Then
                            ' Test the Tactile Ratio
                            If (_results.WL_dFs2_Haptic_2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                                _results.WL_dXs2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Right_Push_Strenght.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_F4F5Test
                                End If
                            End If
                            If (_results.WL_Fs2_F2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                                _results.WL_Xs2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Right_Push_Strenght.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_PeakF4Test
                                End If
                            End If
                        End If
                        ' Defect Early ce
                        If (_results.WL_XCe1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_DiffS2Ce1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Right_Push_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_Ce1Test
                            End If
                        End If
                        If (_recipe.Front_Right_Push_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Push) Then
                            If (_results.WL_XCe2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_DiffS5Ce2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Right_Push_Electrical.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_Ce2Ce1Test
                                End If
                            End If
                        End If
                        ' Defect Over Early
                        If (_results.WL_Xe(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Right_Push_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_OverStrokeTest
                            End If
                        End If
                        If _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                        If _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                    End If
                    '****************************
                    '   Front Left Push
                    '****************************
                    If (WiLi_Test = eWindows.FrontLeft) Then
                        ' Test the Peak Strenght F1
                        If (_results.WL_Fs1_F1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_Xs1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Left_Push_Strenght.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_PeakF1Test
                            End If
                        End If
                        ' Test the Tactile Ratio
                        If (_results.WL_dFs1_Haptic_1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_dXs1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Left_Push_Strenght.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_F1F2Test
                            End If
                        End If

                        If (_recipe.Front_Left_Push_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Push) Then
                            ' Test the Tactile Ratio
                            If (_results.WL_dFs2_Haptic_2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                                _results.WL_dXs2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Left_Push_Strenght.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_F4F5Test
                                End If
                            End If
                            If (_results.WL_Fs2_F2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                                _results.WL_Xs2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Left_Push_Strenght.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_PeakF4Test
                                End If
                            End If
                        End If
                        ' Defect Early ce
                        If (_results.WL_XCe1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_DiffS2Ce1(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Left_Push_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_Ce1Test
                            End If
                        End If
                        If (_recipe.Front_Left_Push_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Push) Then
                            If (_results.WL_XCe2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_DiffS5Ce2(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Left_Push_Electrical.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_Ce2Ce1Test
                                End If
                            End If
                        End If
                        ' Defect Over Early
                        If (_results.WL_Xe(ePush_Pull.Push, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Left_Push_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_OverStrokeTest
                            End If
                        End If
                        If _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                        If _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                    End If
                End If

                '*****************************
                ' Pull Windows Lifter Function
                '*****************************
                WindowsLifterTest = cWS03Results.eWindowsLifterTest.Not_Defined
                If WiLi_Test = eWindows.FrontRight And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) Then WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Pull
                If WiLi_Test = eWindows.FrontLeft And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) Then WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Pull

                If WindowsLifterTest <> cWS03Results.eWindowsLifterTest.Not_Defined Then
                    ' Test the values before and after commutation
                    failedOff = False
                    failedOnCe1 = False
                    failedOnCe2 = False
                    For sampleIndex = _results.WL_X_Indexes(WindowsLifterTest).Xs To _results.WS03_SampleCount(WindowsLifterTest) - 1
                        If (Not (failedOff) AndAlso
                            sampleIndex < (_results.WL_X_Indexes(WindowsLifterTest).X_Ce1 - timeBeforeCommutation) OrElse
                                _results.WL_XCe1(ePush_Pull.Pull, WiLi_Test).Value = 0) Then
                            If WiLi_Test = eWindows.FrontLeft Then
                                _results.WL_Front_Left_Push_Manual(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                _results.WL_Front_Left_Push_Automatic(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                _results.WL_Front_Left_Pull_Manual(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                _results.WL_Front_Left_Pull_Automatic(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)

                                _results.WL_Front_Left_Jama_Down(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                'Front Left
                                failedOff = failedOff OrElse (_results.WL_Front_Left_Push_Manual(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value))
                                failedOff = failedOff OrElse (_results.WL_Front_Left_Push_Automatic(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) _
                                                            AndAlso _recipe.Front_Left_Push_Number_State.Value = 2)
                                failedOff = failedOff OrElse (_results.WL_Front_Left_Pull_Manual(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                                failedOff = failedOff OrElse (_results.WL_Front_Left_Pull_Automatic(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) _
                                                            AndAlso _recipe.Front_Left_Pull_Number_State.Value = 2)
                                failedOff = failedOff OrElse (_results.WL_Front_Left_Jama_Down(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                            Else
                                _results.WL_Front_Right_Push_Manual(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                _results.WL_Front_Right_Push_Automatic(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                _results.WL_Front_Right_Pull_Manual(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                _results.WL_Front_Right_Pull_Automatic(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                _results.WL_Front_Right_Jama_Down(0, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                'Front Right
                                failedOff = failedOff OrElse (_results.WL_Front_Right_Push_Manual(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                                failedOff = failedOff OrElse (_results.WL_Front_Right_Push_Automatic(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) _
                                                            AndAlso _recipe.Front_Right_Push_Number_State.Value = 2)
                                failedOff = failedOff OrElse (_results.WL_Front_Right_Pull_Manual(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value))
                                failedOff = failedOff OrElse (_results.WL_Front_Right_Pull_Automatic(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) _
                                                            AndAlso _recipe.Front_Right_Pull_Number_State.Value = 2)
                                failedOff = failedOff OrElse (_results.WL_Front_Right_Jama_Down(0, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                            End If
                        End If

                        If (Not (failedOnCe1) AndAlso
                              (sampleIndex > (_results.WL_X_Indexes(WindowsLifterTest).X_Ce1 + timeAfterCommutation) AndAlso
                               (sampleIndex < (_results.WL_X_Indexes(WindowsLifterTest).X_Ce2 - timeBeforeCommutation) OrElse
                                (_recipe.Front_Left_Pull_Number_State.Value < 2 AndAlso WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Pull) OrElse
                                (_recipe.Front_Right_Pull_Number_State.Value < 2 AndAlso WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Pull))) OrElse
                                  _results.WL_XCe1(ePush_Pull.Pull, WiLi_Test).Value = 0) Then
                            If WiLi_Test = eWindows.FrontLeft Then
                                _results.WL_Front_Left_Push_Manual(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                _results.WL_Front_Left_Push_Automatic(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                _results.WL_Front_Left_Pull_Manual(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                _results.WL_Front_Left_Pull_Automatic(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                _results.WL_Front_Left_Jama_Down(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                'Front Left
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Left_Push_Manual(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value))
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Left_Push_Automatic(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) _
                                                            AndAlso _recipe.Front_Left_Push_Number_State.Value = 2)
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Left_Pull_Manual(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Left_Pull_Automatic(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) _
                                                            AndAlso _recipe.Front_Left_Pull_Number_State.Value = 2)
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Left_Jama_Down(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                            Else
                                _results.WL_Front_Right_Push_Manual(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                _results.WL_Front_Right_Push_Automatic(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                _results.WL_Front_Right_Pull_Manual(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                _results.WL_Front_Right_Pull_Automatic(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                _results.WL_Front_Right_Jama_Down(1, ePush_Pull.Pull, WiLi_Test).Value =
                                    _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                'Front Right
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Right_Push_Manual(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Right_Push_Automatic(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) _
                                                            AndAlso _recipe.Front_Right_Push_Number_State.Value = 2)
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Right_Pull_Manual(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value))
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Right_Pull_Automatic(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) _
                                                            AndAlso _recipe.Front_Right_Pull_Number_State.Value = 2)
                                failedOnCe1 = failedOnCe1 OrElse (_results.WL_Front_Right_Jama_Down(1, ePush_Pull.Pull, WiLi_Test).Test <>
                                                            cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                            End If
                        End If

                        If (_recipe.Front_Left_Pull_Number_State.Value = 2 AndAlso WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Pull) OrElse
                            (_recipe.Front_Right_Pull_Number_State.Value = 2 AndAlso WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Pull) Then
                            If (Not (failedOnCe2) AndAlso
                                                    sampleIndex > (_results.WL_X_Indexes(WindowsLifterTest).X_Ce2 + timeAfterCommutation) OrElse
                                                    _results.WL_XCe2(ePush_Pull.Pull, WiLi_Test).Value = 0) Then
                                If WiLi_Test = eWindows.FrontLeft Then
                                    _results.WL_Front_Left_Push_Manual(2, ePush_Pull.Pull, WiLi_Test).Value =
                                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                    _results.WL_Front_Left_Push_Automatic(2, ePush_Pull.Pull, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                    _results.WL_Front_Left_Pull_Manual(2, ePush_Pull.Pull, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                    _results.WL_Front_Left_Pull_Automatic(2, ePush_Pull.Pull, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                    _results.WL_Front_Left_Jama_Down(2, ePush_Pull.Pull, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                    'Front Left
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Left_Push_Manual(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value))
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Left_Push_Automatic(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) _
                                                                AndAlso _recipe.Front_Left_Push_Number_State.Value = 2)
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Left_Pull_Manual(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Left_Pull_Automatic(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) _
                                                                AndAlso _recipe.Front_Left_Pull_Number_State.Value = 2)
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Left_Jama_Down(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value))
                                Else
                                    _results.WL_Front_Right_Push_Manual(2, ePush_Pull.Pull, WiLi_Test).Value =
                                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex)
                                    _results.WL_Front_Right_Push_Automatic(2, ePush_Pull.Pull, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex)
                                    _results.WL_Front_Right_Pull_Manual(2, ePush_Pull.Pull, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex)
                                    _results.WL_Front_Right_Pull_Automatic(2, ePush_Pull.Pull, WiLi_Test).Value =
                                        _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex)
                                    _results.WL_Front_Right_Jama_Down(2, ePush_Pull.Pull, WiLi_Test).Value =
                                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex)

                                    'Front Right
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Right_Push_Manual(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Right_Push_Automatic(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) _
                                                                AndAlso _recipe.Front_Right_Push_Number_State.Value = 2)
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Right_Pull_Manual(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value))
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Right_Pull_Automatic(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) _
                                                                And _recipe.Front_Right_Pull_Number_State.Value = 2)
                                    failedOnCe2 = failedOnCe2 OrElse (_results.WL_Front_Right_Jama_Down(2, ePush_Pull.Pull, WiLi_Test).Test <>
                                                                cResultValue.eValueTestResult.Passed AndAlso CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value))
                                End If
                            End If
                        End If
                    Next
                    ' Update the single test result
                    If (failedOff Or failedOnCe1 Or failedOnCe2) And WiLi_Test = eWindows.FrontLeft And
                        _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                            _results.eFront_Left_Pull_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_ElectricalTest
                        End If
                    ElseIf (failedOff Or failedOnCe1 Or failedOnCe2) And WiLi_Test = eWindows.FrontRight And
                        _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                            _results.eFront_Right_Pull_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_ElectricalTest
                        End If
                    End If
                    '' Check all data is checked
                    'For i = 0 To 2
                    '    If (_results.WL_Front_Left_Pull_Manual(i, ePush_Pull.Pull, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
                    '        (_results.WL_Front_Left_Pull_Automatic(i, ePush_Pull.Pull, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown And _
                    '         _recipe.Front_Left_Pull_Number_State.Value = 2) Or _
                    '        _results.WL_Front_Left_Push_Manual(i, ePush_Pull.Pull, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
                    '        (_results.WL_Front_Left_Push_Automatic(i, ePush_Pull.Pull, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown And _
                    '        _recipe.Front_Left_Pull_Number_State.Value = 2) ) Then

                    '        If WiLi_Test = eWindows.FrontLeft And _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    '            _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    '            If (_results.TestResult = cWS03Results.eTestResult.Unknown And _
                    '                _results.eFront_Left_Pull_Electrical.TestResult <> _
                    '                    cResultValue.eValueTestResult.Passed) Then
                    '                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_ElectricalTest
                    '            End If
                    '        ElseIf WiLi_Test = eWindows.FrontRight And _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    '            _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    '            If (_results.TestResult = cWS03Results.eTestResult.Unknown And _
                    '                _results.eFront_Right_Pull_Electrical.TestResult <> _
                    '                    cResultValue.eValueTestResult.Passed) Then
                    '                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_ElectricalTest
                    '            End If
                    '        End If
                    '    End If
                    'Next i

                    ' Check Result
                    '****************************
                    '   Front Right Pull
                    '****************************
                    If (WiLi_Test = eWindows.FrontRight) Then
                        ' Test the Peak Strenght F1
                        If (_results.WL_Fs1_F1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_Xs1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Right_Pull_Strenght.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_PeakF1Test
                            End If
                        End If
                        ' Test the Tactile Ratio
                        If (_results.WL_dFs1_Haptic_1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_dXs1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Right_Pull_Strenght.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_F1F2Test
                            End If
                        End If
                        If (_recipe.Front_Right_Pull_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Pull) Then
                            ' Test the Tactile Ratio
                            If (_results.WL_dFs2_Haptic_2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                                _results.WL_dXs2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Right_Pull_Strenght.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_F4F5Test
                                End If
                            End If
                            If (_results.WL_Fs2_F2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                               _results.WL_Xs2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                               _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Right_Pull_Strenght.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_PeakF1Test
                                End If
                            End If
                        End If
                        ' Defect Early ce
                        If (_results.WL_XCe1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_DiffS2Ce1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Right_Pull_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_Ce1Test
                            End If
                        End If
                        If (_recipe.Front_Right_Pull_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Pull) Then
                            If (_results.WL_XCe2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_DiffS5Ce2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Right_Pull_Electrical.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_Ce2Ce1Test
                                End If
                            End If
                        End If
                        ' Defect Over Early
                        If (_results.WL_Xe(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Right_Pull_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_OverStrokeTest
                            End If
                        End If
                        If _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                        If _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                    End If
                    '****************************
                    '   Front Left Pull
                    '****************************
                    If (WiLi_Test = eWindows.FrontLeft) Then
                        ' Test the Peak Strenght F1
                        If (_results.WL_Fs1_F1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_Xs1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Left_Pull_Strenght.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_PeakF1Test
                            End If
                        End If
                        ' Test the Tactile Ratio
                        If (_results.WL_dFs1_Haptic_1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_dXs1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Left_Pull_Strenght.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_F1F2Test
                            End If
                        End If

                        If (_recipe.Front_Left_Pull_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Pull) Then
                            ' Test the Tactile Ratio
                            If (_results.WL_dFs2_Haptic_2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                                _results.WL_dXs2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Left_Pull_Strenght.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_F4F5Test
                                End If
                            End If
                            If (_results.WL_Fs2_F2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                                _results.WL_Xs2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Left_Pull_Strenght.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_PeakF1Test
                                End If
                            End If
                        End If
                        ' Defect Early ce
                        If (_results.WL_XCe1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_DiffS2Ce1(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Left_Pull_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_Ce1Test
                            End If
                        End If
                        If (_recipe.Front_Left_Pull_Number_State.Value = 2 And WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Pull) Then
                            If (_results.WL_XCe2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.WL_DiffS5Ce2(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                                _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                                _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                    _results.eFront_Left_Pull_Electrical.TestResult <>
                                        cResultValue.eValueTestResult.Passed) Then
                                    _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_Ce2Ce1Test
                                End If
                            End If
                        End If
                        ' Defect Over Early
                        If (_results.WL_Xe(ePush_Pull.Pull, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eFront_Left_Pull_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_OverStrokeTest
                            End If
                        End If
                        If _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                        If _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                    End If
                End If

                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If WiLi_Test = eWindows.FrontLeft Then
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eFront_Left_Pull_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eFront_Left_Pull_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_ElectricalTest
                    End If
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eFront_Left_Pull_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eFront_Left_Pull_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftUP_StrenghtTest
                    End If
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eFront_Left_Push_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eFront_Left_Push_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_ElectricalTest
                    End If
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eFront_Left_Push_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eFront_Left_Push_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedFrontLeftDN_StrenghtTest
                    End If
                ElseIf WiLi_Test = eWindows.FrontRight Then
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eFront_Right_Pull_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eFront_Right_Pull_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_ElectricalTest
                    End If
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eFront_Right_Pull_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eFront_Right_Pull_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedFrontRightUP_StrenghtTest
                    End If
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eFront_Right_Push_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eFront_Right_Push_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_ElectricalTest
                    End If
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eFront_Right_Push_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eFront_Right_Push_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedFrontRightDN_StrenghtTest
                    End If
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                If _phase = ePhase.FrontLeft Then
                    _phase = ePhase.FrontRight
                ElseIf _phase = ePhase.FrontRight Then
                    _phase = ePhase.ChildrenLock
                End If

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                ' Go to next phase
                If WiLi_Test = eWindows.FrontLeft Then
                    If _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Left_Pull_Electrical.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Left_Pull_Strenght.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Left_Push_Electrical.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Left_Push_Strenght.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                ElseIf WiLi_Test = eWindows.FrontRight Then
                    If _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Right_Pull_Electrical.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Right_Pull_Strenght.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Right_Push_Electrical.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eFront_Right_Push_Strenght.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        ' TMP DBE
        e = False
        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David DBE
        End If
        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If

    End Sub


    Private Sub PhaseChildrenLock()
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Dim LinIndex As Integer
        Dim WiliResult As sWiliResult
        Dim failedOff As Boolean
        Dim failedOnCe1 As Boolean
        ' Offset for Stroke Sensor and Strenght Sensor to adapte Data measurement
        Dim OffsetStrokeSensor As Integer = 1
        Dim OffsetStrenghtSensor As Integer = 1

        Dim timeAfterCommutation As Integer = 0    ' f=2kHz time is 25 ms
        Dim timeBeforeCommutation As Integer = 0   ' f=2kHz time is 5 ms


        Dim f As CLINFrame
        Dim tpSample As Single
        Dim zz As Integer
        Static NB_Transition As Integer
        Static Switching(0 To 20) As Single
        Static CommutCe1(0 To 1) As Single
        Static CommutCe2(0 To 1) As Single
        Static frameIndex As Integer
        Static t0 As Date
        Static tLin As Date
        Static t0Phase As Date
        Static s As String
        Static StatusPushLin(0 To 20) As String
        Static frameExt As Boolean

        'Static TimeStart As Date
        Static OffSetTime As Double
        Static t0Lin As Date
        Static tAnalog As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                If CBool(_recipe.TestEnable_ChildrenLock_Electrical.Value) Then
                    AddLogEntry("Begin Test Children Lock")
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' clear data lin
                    For i = 0 To 20
                        StatusPushLin(i) = "000000"
                        Switching(i) = 0
                        NB_Transition = 0
                    Next
                    '
                    AddLogEntry("Reset Force Sensor  and Clear buffer Analog")
                    ' Set the force sensors reset signals
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_ResetForce)
                    'Clear buffer
                    mWS03AIOManager.EmptyBuffer()
                    ' Store the time
                    t0 = Date.Now
                    If Not Simulation_Test Then
                        ' Go to next subphase
                        _subPhase(_phase) += 1
                    Else
                        ' Go to next subphase
                        _subPhase(_phase) = 10
                    End If
                Else
                    AddLogEntry("Children Lock is disabled")
                    If (_testMode = eTestMode.Debug AndAlso Simulation_Test) Then
                        ' Go to phase WaitStartTest
                        _phase = ePhase.WaitStartTest
                    Else
                        ' Go to next phase
                        _phase = ePhase.FinalState
                    End If
                End If

            Case 1
                ' If the step in progress input is cleared (push activation)
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS03_StepInProgress) = False) Then
                    ' Start sampling the analog inputs
                    e = mWS03AIOManager.StartSampling(samplingFrequency)
                    AddLogEntry("Start Analog Sample " & vbTab & "Case : " & _subPhase(_phase))
                    'Clear buffer
                    mWS03AIOManager.EmptyBuffer()
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    AddLogEntry("Trasmit Frame Start Counter Transition " & vbTab & "Case : " & _subPhase(_phase))
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Start_Haptic_DIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 2
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Start_Haptic_DIN))
                If (i <> -1) Then
                    AddLogEntry("Ack LIN Frame " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the force sensors reset signals
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_ResetForce)
                    ' Lin 
                    t0Lin = _LinInterface.ToSystemTime(CLng(_LinInterface.RxFrame(i).Timestamp))
                    AddLogEntry("TimeStamp Lin : " & Format(t0Lin, "dd/MM/yyyy, HH:mm:ss:fff") & vbTab & _LinInterface.RxFrame(i).Timestamp)
                    AddLogEntry("Difference systeme Time / lin Time : " & (Date.Now - t0Lin).TotalMilliseconds.ToString("0.00"))
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' stop Lin diag
                    _LinInterface.StopScheduleDiag()
                    ' Reset the start step output (push activation)
                    AddLogEntry("Start Step to Activate the Robot")
                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then
                    AddLogEntry("TimeOut LIN Frame " & vbTab & "Case : " & _subPhase(_phase))
                    ' stop Lin diag
                    _LinInterface.StopScheduleDiag()
                    ' Reset the force sensors reset signals
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_ResetForce)
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    'Clear buffer
                    mWS03AIOManager.EmptyBuffer()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 3
                ' If the step in progress input is set (push activation)
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = True Or _testMode = eTestMode.Debug) Then
                    _step = False
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    AddLogEntry("Start Step = 0 / Step in Progress = 1 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 4
                ' If the step in progress input is cleared (push activation)
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS03_StepInProgress) = False) Then
                    AddLogEntry("Step in Progress = 0 / ReadDataBlock  transition " & vbTab & "Case : " & _subPhase(_phase))
                    ' Stop sampling the analog inputs
                    AddLogEntry("Stop Analog Sample " & vbTab & "Case : " & _subPhase(_phase))
                    e = e Or mWS03AIOManager.StopSampling
                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Start Robot for release
                    AddLogEntry(" Start Robot for release")
                    ' Store the time
                    t0 = Date.Now
                    frameIndex = &H0
                    frameExt = False
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 5
                ' If the step in progress input is set (push activation)
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03_StepInProgress) = True Or _testMode = eTestMode.Debug) Then
                    _step = False
                    AddLogEntry("Start Step = 0 / Step in Progress = 1 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_StartStep)
                    ' Read Raw Data
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_DIN),
                                        True,
                                        txData_MasterReq,
                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                        True,
                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Go to subphase 9
                    _subPhase(_phase) += 1
                    '
                    frameIndex = 0
                    t0 = Date.Now
                    tLin = Date.Now
                End If

            Case 6
                ' If the correct answer was received
                Try
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_RawTransition_DIN))
                    If (i <> -1) Then
                        tAnalog = mWS03AIOManager.TimeStampSampleBuffer(0)
                        AddLogEntry("Time Stamp Analog : " & Format(tAnalog, "dd/MM/yyyy, HH:mm:ss:fff"))
                        OffSetTime = (t0Lin - tAnalog).TotalMilliseconds - 10 '-10 only used for product response delay, remove after product software update.
                        AddLogEntry("Difference acq AI/acq LIN : " & OffSetTime.ToString("0.00"))
                        'OffSetTime = (trLin - tAnalog).TotalMilliseconds
                        AddLogEntry("Offset Time Apply : " & (t0Lin - tAnalog).TotalMilliseconds)
                        ' Initialize the string buffer
                        NB_Transition = CInt("&h" & _LinInterface.RxFrame(i).Data(7))
                        AddLogEntry("Transition  Number Detected : " & NB_Transition)
                        If NB_Transition > 20 Then
                            NB_Transition = 20
                        End If
                        ' Delete the frame
                        _LinInterface.DeleteRxFrame(i)
                        s = ""
                        ' Initialize the frame index
                        frameIndex = 21
                        frameExt = False
                        ' Store the time
                        t0 = Date.Now
                    ElseIf ((Date.Now - tLin).TotalMilliseconds >= 100) And frameIndex = 0 Then
                        tLin = Date.Now
                        ' Read Raw Data
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_DIN),
                                            True,
                                            txData_MasterReq,
                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                            True,
                                            True)
                    ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then   ' Otherwise, if the LIN timeout expired
                        ' Go to subphase 600
                        _subPhase(_phase) += 1
                    End If

                    If (frameIndex > 0) Then
                        Do
                            ' Builds the expected frame
                            f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_FlowCtrl).DeepClone
                            For i = 1 To 7
                                f.Data(i) = "XX"
                            Next i
                            f.Data(1) = "XX" ' frameIndex
                            ' If the frame was received
                            i = _LinInterface.RxFrameIndex(f)
                            If (i <> -1) Then
                                ' Store the values
                                s = s & _LinInterface.RxFrame(i).Data(2) &
                                        _LinInterface.RxFrame(i).Data(3) &
                                        _LinInterface.RxFrame(i).Data(4) &
                                        _LinInterface.RxFrame(i).Data(5) &
                                        _LinInterface.RxFrame(i).Data(6) &
                                        _LinInterface.RxFrame(i).Data(7)
                                ' Delete the frame
                                _LinInterface.DeleteRxFrame(i)
                                t0 = Date.Now
                            ElseIf ((Date.Now - t0).TotalMilliseconds >= 50) Then ' _LINTimeout_ms) Then
                                ' DN
                                For LinIndex = 0 To NB_Transition - 1
                                    ' initiate position
                                    StatusPushLin(LinIndex) = (Mid(s, (6 * LinIndex) + 1, 2))
                                    ' Add Delay for lin communication
                                    Dim str = "&h" & Mid(s, (6 * LinIndex) + 3, 4)
                                    Switching(LinIndex) = CSng(CSng("&h" & Mid(s, (6 * LinIndex) + 3, 4)) + OffSetTime) ' temporaire
                                    AddLogEntry("Transition  Number " & LinIndex + 1 & vbTab & "Status : " & StatusPushLin(LinIndex) & vbTab & "Timming : " & Switching(LinIndex) & vbTab & "Timming Real: " & CSng("&h" & Mid(s, (6 * LinIndex) + 3, 4)))
                                Next
                                ' Lin Schedulle
                                _LinInterface.StopScheduleDiag()
                                ' Set the flag of CAN timeout
                                _subPhase(_phase) += 1
                            End If
                        Loop Until (i = -1)
                    End If

                Catch ex As Exception
                    Dim Error_scan As String
                    Error_scan = Err.Description
                    AddLogEntry("Sub Phase " & sp & " Error : " & Error_scan)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Set the flag of CAN timeout
                    _subPhase(_phase) += 1
                End Try


            Case 7
                ' Stop Timing Lin
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Stop_Haptic_DIN),
                                    True,
                                    txData_MasterReq,
                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                    True,
                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                ' Go to subphase 10
                _subPhase(_phase) += 1

            Case 8
                ' If the correct answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Stop_Haptic_DIN))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Go to subphase 8
                    _subPhase(_phase) = 10
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) = 10
                End If

            Case 10
                '
                Try
                    '
                    If Simulation_Test Then
                        frmProduction.OpenFileDialog1.ShowDialog()
                        mWS03Main.FilePoint = frmProduction.OpenFileDialog1.FileName
                        Read_AnalogPoint(FilePoint, True, 2)
                    Else
                        ' initialize Step
                        LinIndex = 0
                        ' Store the sample count
                        _results.SampleCount_TMP = mWS03AIOManager.SampleCount
                        If _results.SampleCount_TMP > 99999 Then
                            _results.SampleCount_TMP = 99990
                        End If
                        If (_results.SampleCount_TMP > cWS03Results.MaxSampleCount) Then
                            _results.SampleCount_TMP = cWS03Results.MaxSampleCount
                        End If
                        '_results.SampleCount_TMP_WILI(WiLi_Test) = mWS03AIOManager.SampleCount

                        ' Store the samples
                        For sampleIndex = 0 To _results.SampleCount_TMP - 1

                            ' Early Sensor
                            _results.Sample_TMP(eSignal_Analog.EarlySensor, sampleIndex) =
                                mWS03AIOManager.SampleBuffer(mWS03AIOManager.eAnalogInput.WS03_EarlySensor, sampleIndex) * OffsetStrokeSensor
                            ' Strenght Sensor
                            _results.Sample_TMP(eSignal_Analog.StrenghtSensor, sampleIndex) =
                                mWS03AIOManager.SampleBuffer(mWS03AIOManager.eAnalogInput.WS03_StrenghtSensor, sampleIndex) *
                                OffsetStrenghtSensor
                            ' Calul the timestamp for synchronisation
                            tpSample = sampleIndex * ((1 / samplingFrequency) * 1000)
                            ' Completed the table of datas relative to the index
                            If tpSample >= Switching(LinIndex + 1) AndAlso Switching(LinIndex + 1) > 0 Then
                                LinIndex = LinIndex + 1
                            End If

                            ' Push 
                            _results.Sample_TMP(2, sampleIndex) = CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 7) And &H1)))) * 13.5

                        Next
                    End If

                    'Write in file for analyse
                    If WriteFilePoint Then
                        If Not Simulation_Test Then
                            Write_AnalogPointPush(-1)
                        End If
                    End If
                    '' Push Back to initial state,Check later, why only two state, actually log is three state.
                    'If Switching(2) - Switching(1) < 100 Then
                    '    _results.BakcInitialSate.Value = CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 7) And &H1)))) * 13.5
                    'Else
                    '    _results.BakcInitialSate.Value = CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 7) And &H1)))) * 13.5
                    'dEnd If

                    ' Filter the samples
                    FilterSamples_Global(eSignal_Analog.EarlySensor, mWS03AIOManager.eAnalogInput.WS03_EarlySensor)
                    FilterSamples_Global(eSignal_Analog.StrenghtSensor, mWS03AIOManager.eAnalogInput.WS03_StrenghtSensor)
                    '
                    _results.WS03_SampleCountPush = _results.SampleCount_TMP
                    ' Store the samples
                    For sampleIndex = 0 To _results.SampleCount_TMP - 1
                        ' Early Sensor
                        _results.WS03_SamplePush(eSignal_Analog.EarlySensor, sampleIndex) =
                           _results.Sample_TMP(eSignal_Analog.EarlySensor, sampleIndex)
                        ' Strenght Sensor
                        _results.WS03_SamplePush(eSignal_Analog.StrenghtSensor, sampleIndex) =
                            _results.Sample_TMP(eSignal_Analog.StrenghtSensor, sampleIndex)
                        ' Push 
                        _results.WS03_SamplePush(2, sampleIndex) =
                            _results.Sample_TMP(2, sampleIndex)
                    Next
                    ''Write in file for analyse
                    'If WriteFilePoint Then
                    '    If Not Simulation_Test Then
                    '        Write_AnalogPointPush(0)
                    '    End If
                    'End If
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 11
                    _subPhase(_phase) = 20

                Catch ex As Exception
                    Dim errdes As String
                    errdes = Err.Description
                    AddLogEntry("Sub Phase 10 Error : " & errdes)
                    ' Go to subphase 11
                    _subPhase(_phase) = 20

                End Try

            Case 20
                '******************
                ' Push
                '******************
                ' Process the electrical test samples
                ProcessSamples_BaseTps_Push(WiliResult)
                '
                ' Write_AnalogPoint(WindowsLifterTest, 1)
                ' Store the commutation Early, the overstroke, the Strenght peak F1, the Early Strenght S1, the Activation Strenght F2 and the Tactile Ratio
                _results.CL_XCe1.Value = WiliResult.XCe1
                _results.CL_Xe.Value = WiliResult.Xe
                ' Force
                _results.CL_Fs1_F1.Value = WiliResult.Fs1_F1
                _results.CL_Xs1.Value = WiliResult.Xs1
                _results.CL_dFs1_Haptic_1.Value = WiliResult.dFs1_Haptic_1
                _results.CL_dXs1.Value = WiliResult.dXs1
                _results.CL_Fe.Value = WiliResult.Fe
                _results.CL_DiffS2Ce1.Value = WiliResult.Value_DiffS2Ce1

                _results.CL_X_Indexes.Xs = WiliResult.Xs_Index
                _results.CL_X_Indexes.Xe = WiliResult.Xe_Index
                _results.CL_X_Indexes.X_Ce1 = WiliResult.X_Ce1_index
                _results.CL_X_Indexes.X_F1 = WiliResult.Xs_F1_index
                _results.CL_X_Indexes.X_F2 = WiliResult.Xs_F2_index
                If (SaveFSScreenshot) Then
                    Dim filename As String = "ChildrenLock" +
                        Mid(_results.TestDate.Value.ToString, 7, 4) +
                        Mid(_results.TestDate.Value.ToString, 4, 2) +
                        Mid(_results.TestDate.Value.ToString, 1, 2) +
                        "_" + _results.PartUniqueNumber.Value.ToString
                    mResults.SavePushReturnFSCurve(2, _results.CL_Xe.Value,
                                                 _results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, _results.CL_X_Indexes.Xs),
                                                 _results.CL_X_Indexes.Xs, _results.CL_X_Indexes.Xe,
                                                 _results.WS03_SamplePush,
                                                 _results.CL_X_Indexes.X_F1,
                                                 _results.CL_X_Indexes.X_F2,
                                                 mWS03Main.cSample_Signal.EarlySensor,
                                                 mWS03Main.cSample_Signal.StrenghtSensor, filename)
                End If
                ' Go to subphase 100
                _subPhase(_phase) = 100

            Case 100
                ' If the electrical test is enabled
                '--------------------------------------------
                '|   Push        |
                '--------------------------------------------
                '*****************************
                ' Push Children Lockunction
                '*****************************
                ' Test the values before and after commutation
                failedOff = False
                failedOnCe1 = False
                For sampleIndex = _results.CL_X_Indexes.Xs To _results.WS03_SampleCountPush - 1
                    If (Not (failedOff) AndAlso
                        sampleIndex < (_results.CL_X_Indexes.X_Ce1 - timeBeforeCommutation) OrElse
                            _results.CL_XCe1.Value = 0) Then
                        '
                        _results.Push_CL_Electric(0).Value = _results.WS03_SamplePush(2, sampleIndex)
                        '
                        failedOff = failedOff OrElse (_results.Push_CL_Electric(0).Test <> cResultValue.eValueTestResult.Passed)
                    End If

                    If (Not (failedOnCe1) AndAlso
                        sampleIndex > (_results.CL_X_Indexes.X_Ce1 + timeBeforeCommutation) OrElse
                            _results.CL_XCe1.Value = 0) Then
                        '
                        _results.Push_CL_Electric(1).Value = _results.WS03_SamplePush(2, sampleIndex)
                        '
                        failedOnCe1 = failedOnCe1 OrElse (_results.Push_CL_Electric(1).Test <> cResultValue.eValueTestResult.Passed)
                    End If

                Next
                ' Update the single test result
                If (failedOff Or failedOnCe1) And
                    _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eChildren_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedChildren_ElectricalTest
                    End If
                End If
                '' Update the single test result
                'If (_results.BakcInitialSate.Test <> cResultValue.eValueTestResult.Passed) And
                '       _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                '    _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                '    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                '        _results.eChildren_Electrical.TestResult <>
                '            cResultValue.eValueTestResult.Passed) Then
                '        _results.TestResult = cWS03Results.eTestResult.FailedChildren_ElectricalTest
                '    End If
                'End If
                ' Check all data is checked
                For i = 0 To 1
                    If (_results.Push_CL_Electric(i).TestResult = cResultValue.eValueTestResult.Unknown) Then
                        If _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                                _results.eChildren_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS03Results.eTestResult.FailedChildren_ElectricalTest
                            End If
                        End If
                    End If
                Next i

                ' Check Result
                '****************************
                ' Push Children Lock
                '****************************
                ' Test the Peak Strenght F1
                If (_results.CL_Fs1_F1.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.CL_Xs1.Test <> cResultValue.eValueTestResult.Passed) And
                    _results.eChildren_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eChildren_Strenght.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedChildren_StrenghtTest
                    End If
                End If
                ' Test the Tactile Ratio
                If (_results.CL_dFs1_Haptic_1.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.CL_dXs1.Test <> cResultValue.eValueTestResult.Passed) And
                    _results.eChildren_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eChildren_Strenght.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedChildren_StrenghtTest
                    End If
                End If
                ' Defect Early ce
                If (_results.CL_XCe1.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.CL_DiffS2Ce1.Test <> cResultValue.eValueTestResult.Passed) And
                    _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eChildren_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedChildren_ElectricalTest
                    End If
                End If
                ' Defect Over Early
                If (_results.CL_Xe.Test <> cResultValue.eValueTestResult.Passed) And
                    _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eChildren_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS03Results.eTestResult.FailedChildren_ElectricalTest
                    End If
                End If
                If _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                End If
                If _results.eChildren_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                End If

                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eChildren_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eChildren_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedChildren_ElectricalTest
                End If
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                        _results.eChildren_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.eChildren_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedChildren_StrenghtTest
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                If (_testMode = eTestMode.Debug AndAlso Simulation_Test) Then
                    ' Go to phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                Else
                    ' Go to next phase
                    _phase = ePhase.FinalState
                End If

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                ' Go to next phase
                If _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Electrical.TestResult =
                        cResultValue.eValueTestResult.Failed
                End If
                If _results.eChildren_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eChildren_Strenght.TestResult =
                        cResultValue.eValueTestResult.Failed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        ' TMP DBE
        e = False
        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David DBE
        End If
        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If

    End Sub


    Private Sub PhaseFinal_StateProduct()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Static t0 As Date
        Static t0Phase As Date
        Static tLin As Date

        Dim f As CLINFrame
        Static frameIndex As Integer
        Static s As String

        Static WL_Passenger_Push_M As Boolean
        Static WL_Passenger_Push_A As Boolean
        Static WL_Passenger_Pull_M As Boolean
        Static WL_Passenger_Pull_A As Boolean

        Static WL_RL_Push_M As Boolean
        Static WL_RL_Push_A As Boolean
        Static WL_RL_Pull_M As Boolean
        Static WL_RL_Pull_A As Boolean

        Static WL_RR_Push_M As Boolean
        Static WL_RR_Push_A As Boolean
        Static WL_RR_Pull_M As Boolean
        Static WL_RR_Pull_A As Boolean

        Static Mirror_UP As Boolean
        Static Mirror_DN As Boolean
        Static Mirror_L As Boolean
        Static Mirror_R As Boolean

        Static ADC_Mirror_Adj As Integer
        Static ADC_WL_Passenger As Integer
        Static ADC_WL_RL As Integer
        Static ADC_WL_RR As Integer

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Final State Product ")
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 1


            Case 1
                ' If the step in progress input is cleared (push activation)
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS03_StepInProgress) = False) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_ALL_DIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 2
                End If

            Case 2
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Read_ALL_DIN))
                If (i <> -1) Then
                    _results.FinalState(0).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 0) And &H1)))) * 13.5
                    _results.FinalState(1).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 1) And &H1)))) * 13.5
                    _results.FinalState(2).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 2) And &H1)))) * 13.5
                    _results.FinalState(3).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 3) And &H1)))) * 13.5
                    _results.FinalState(4).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 4) And &H1)))) * 13.5
                    _results.FinalState(5).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 5) And &H1)))) * 13.5
                    _results.FinalState(6).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 6) And &H1)))) * 13.5
                    _results.FinalState(7).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 7) And &H1)))) * 13.5
                    _results.FinalState(8).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(5)) \ CByte(2 ^ 4) And &H1)))) * 13.5
                    _results.FinalState(9).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(5)) \ CByte(2 ^ 5) And &H1)))) * 13.5
                    _results.FinalState(10).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(5)) \ CByte(2 ^ 6) And &H1)))) * 13.5
                    _results.FinalState(11).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(5)) \ CByte(2 ^ 7) And &H1)))) * 13.5
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) = 3
                ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= 200) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_ALL_DIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If
            Case 3
                If ((Date.Now - t0).TotalMilliseconds > 100) Then
                    ' 
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    frameIndex = 21
                    ' Store the time
                    't0 = Date.Now
                    tLin = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 4
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Read_All_AIN))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(5) &
                        _LinInterface.RxFrame(i).Data(6) &
                        _LinInterface.RxFrame(i).Data(7)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' init index Frame
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) And frameIndex = -1 Then
                    '
                    tLin = Date.Now
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)

                End If

                Do
                    ' If a CAN frame was received
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_FlowCtrl).DeepClone
                    For i = 1 To 7
                        f.Data(i) = "XX"
                    Next i
                    f.Data(1) = CStr(frameIndex)
                    ' Check Response
                    i = _LinInterface.RxFrameIndex(f)
                    If (i <> -1) Then
                        ' Store the values
                        s = s & _LinInterface.RxFrame(i).Data(2) &
                                _LinInterface.RxFrame(i).Data(3) &
                                _LinInterface.RxFrame(i).Data(4) &
                                _LinInterface.RxFrame(i).Data(5) &
                                _LinInterface.RxFrame(i).Data(6) &
                                _LinInterface.RxFrame(i).Data(7)
                        ' Delete the frame
                        _LinInterface.DeleteRxFrame(i)
                        ' Increase the frame index or go to subphase 100
                        If (frameIndex < 24) Then
                            frameIndex = frameIndex + 1
                            t0 = Date.Now
                        Else
                            ADC_Mirror_Adj = CInt("&H" & Mid(s, 13, 4))

                            If ADC_Mirror_Adj > 517 And ADC_Mirror_Adj < 577 Then
                                Mirror_R = True
                            ElseIf ADC_Mirror_Adj > 664 And ADC_Mirror_Adj < 710 Then
                                Mirror_DN = True
                            ElseIf ADC_Mirror_Adj > 786 And ADC_Mirror_Adj < 820 Then
                                Mirror_UP = True
                            ElseIf ADC_Mirror_Adj > 893 And ADC_Mirror_Adj < 915 Then
                                Mirror_L = True
                            End If

                            ADC_WL_RL = CInt("&H" & Mid(s, 1, 4))
                            ADC_WL_RR = CInt("&H" & Mid(s, 5, 4))
                            ADC_WL_Passenger = CInt("&H" & Mid(s, 9, 4))

                            If ADC_WL_RL > 242 And ADC_WL_RL < 355 Then
                                WL_RL_Pull_A = True
                            ElseIf ADC_WL_RL > 392 And ADC_WL_RL < 527 Then
                                WL_RL_Pull_M = True
                            ElseIf ADC_WL_RL > 628 And ADC_WL_RL < 682 Then
                                WL_RL_Push_A = True
                            ElseIf ADC_WL_RL > 777 And ADC_WL_RL < 819 Then
                                WL_RL_Push_M = True
                            End If

                            If ADC_WL_RR > 242 And ADC_WL_RR < 355 Then
                                WL_RR_Pull_A = True
                            ElseIf ADC_WL_RR > 392 And ADC_WL_RR < 527 Then
                                WL_RR_Pull_M = True
                            ElseIf ADC_WL_RR > 628 And ADC_WL_RR < 682 Then
                                WL_RR_Push_A = True
                            ElseIf ADC_WL_RR > 777 And ADC_WL_RR < 819 Then
                                WL_RR_Push_M = True
                            End If

                            If ADC_WL_Passenger > 242 And ADC_WL_Passenger < 355 Then
                                WL_Passenger_Pull_A = True
                            ElseIf ADC_WL_Passenger > 392 And ADC_WL_Passenger < 527 Then
                                WL_Passenger_Pull_M = True
                            ElseIf ADC_WL_Passenger > 628 And ADC_WL_Passenger < 682 Then
                                WL_Passenger_Push_A = True
                            ElseIf ADC_WL_Passenger > 777 And ADC_WL_Passenger < 819 Then
                                WL_Passenger_Push_M = True
                            End If

                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            t0 = Date.Now
                            ' Go to next subphase
                            _subPhase(_phase) += 1

                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        t0 = Date.Now
                        _subPhase(_phase) = 199
                    End If
                Loop Until (i = -1)

            Case 5
                _results.FinalState(12).Value = IIf(WL_Passenger_Pull_M, 0, 13.5)
                _results.FinalState(13).Value = IIf(WL_Passenger_Pull_A, 0, 13.5)
                _results.FinalState(14).Value = IIf(WL_Passenger_Push_M, 0, 13.5)
                _results.FinalState(15).Value = IIf(WL_Passenger_Push_A, 0, 13.5)
                _results.FinalState(16).Value = IIf(WL_RL_Pull_M, 0, 13.5)
                _results.FinalState(17).Value = IIf(WL_RL_Pull_A, 0, 13.5)
                _results.FinalState(18).Value = IIf(WL_RL_Push_M, 0, 13.5)
                _results.FinalState(19).Value = IIf(WL_RL_Push_A, 0, 13.5)
                _results.FinalState(20).Value = IIf(WL_RR_Pull_M, 0, 13.5)
                _results.FinalState(21).Value = IIf(WL_RR_Pull_A, 0, 13.5)
                _results.FinalState(22).Value = IIf(WL_RR_Push_M, 0, 13.5)
                _results.FinalState(23).Value = IIf(WL_RR_Push_A, 0, 13.5)
                _results.FinalState(24).Value = IIf(Mirror_UP, 0, 13.5)
                _results.FinalState(25).Value = IIf(Mirror_DN, 0, 13.5)
                _results.FinalState(26).Value = IIf(Mirror_L, 0, 13.5)
                _results.FinalState(27).Value = IIf(Mirror_R, 0, 13.5)

                t0 = Date.Now
                _subPhase(_phase) = 100

            Case 100
                ' Tests
                For index = 2 To 27
                    If index = 8 Or index = 9 Or index = 10 Or index = 11 Then
                        Continue For
                    End If
                    If _results.FinalState(index).Test <> cResultValue.eValueTestResult.Passed Then
                        _results.eFINAL_STATE_PRODUCT.TestResult = cWS03Results.eSingleTestResult.Failed
                    End If
                Next

                ' Update the test result
                If _results.eFINAL_STATE_PRODUCT.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eFINAL_STATE_PRODUCT.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                    _results.eFINAL_STATE_PRODUCT.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedFinalStateProduct
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Write_MMS_TestByte

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eFINAL_STATE_PRODUCT.TestResult =
                    cResultValue.eValueTestResult.Failed
                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David DBE
        End If

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If

    End Sub

    Private Sub PhaseWRITE_MMSTestByte()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
        Static t0 As Date
        Static t0Phase As Date
        Static tLin As Date
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Write MMS Test Byte")
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 1

            Case 1
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_W_TestByte).DeepClone
                If _results.TestResult = cWS03Results.eTestResult.Unknown Then
                    If _results.MMS_Test_Byte_Before.Value = "31" Or _results.MMS_Test_Byte_Before.Value = "32" Or _results.MMS_Test_Byte_Before.Value = "37" Then
                        'WS03 Retest.
                        f.Data(5) = "37"
                    Else
                        'WS03 First Test
                        f.Data(5) = "31"
                    End If
                Else
                    f.Data(5) = "32"
                End If

                ' Transmit Frame Diag (Frame, entry Log, Data Value, Sssion Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(f,
                                                True,
                                                txData_MasterReq,
                                                cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                True,
                                                True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 2

            Case 2
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_MMS_W_TestByte))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Go to subphase
                    _subPhase(_phase) = 100
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

                ' Manage the NRC 78 Pending
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "XX" : f.Data(1) = "03" : f.Data(2) = "7F" : f.Data(3) = "2E"
                f.Data(4) = "XX" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    If _LinInterface.RxFrame(i).Data(4) = "78" Then
                        AddLogEntry("NRC 78 , Pending Request")
                    Else
                        ' Go to subphase
                        _subPhase(_phase) = 199
                    End If
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                End If

            Case 100
                ' Tests
                ' Update the test result
                _results.eWrite_MMSTestByte.TestResult =
                        cResultValue.eValueTestResult.Passed
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                    _results.eWrite_MMSTestByte.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedWriteTestByte
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Read_MMS_TestByte

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eWrite_MMSTestByte.TestResult =
                    cResultValue.eValueTestResult.Failed

                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David DBE
        End If
        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If

    End Sub

    Private Sub PhaseRead_MMSTestByte()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
        Static t0 As Date
        Static t0Phase As Date
        Static tLin As Date
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Read MMS Test Byte")
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 1

            Case 1
                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_TestByte),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 2
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_MMS_R_TestByte))
                If (i <> -1) Then
                    _results.MMS_Test_Byte_After.Value = _LinInterface.RxFrame(i).Data(5)
                    If (_LinInterface.RxFrame(i).Data(5) = "31" Or _LinInterface.RxFrame(i).Data(5) = "37") Then
                        _results.MMS_Test_Byte_After.MinimumLimit = _LinInterface.RxFrame(i).Data(5)
                        _results.MMS_Test_Byte_After.MaximumLimit = _LinInterface.RxFrame(i).Data(5)
                    Else
                        _results.MMS_Test_Byte_After.MinimumLimit = "00"
                        _results.MMS_Test_Byte_After.MaximumLimit = "00"
                    End If

                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Go to subphase
                    _subPhase(_phase) = 100
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - tLin).TotalMilliseconds > LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_TestByte),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    '
                    tLin = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

                ' check if NRC22
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_NRC_22))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _results.eRead_MMSTestByte.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) = 100
                End If

            Case 100
                ' Tests
                ' Update the test result
                If _results.MMS_Test_Byte_After.Test <> cResultValue.eValueTestResult.Passed Then
                    _results.eRead_MMSTestByte.TestResult =
                            cResultValue.eValueTestResult.Failed
                Else
                    _results.eRead_MMSTestByte.TestResult =
                            cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS03Results.eTestResult.Unknown And
                    _results.eRead_MMSTestByte.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS03Results.eTestResult.FailedCheckMMSTestByte
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.PowerDown

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eRead_MMSTestByte.TestResult =
                    cResultValue.eValueTestResult.Failed

                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David DBE
        End If
        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If

    End Sub

    Private Sub PhasePowerDown()
        Dim e As Boolean
        Dim sp As Integer
        Static t0Phase As Date
        Static t0 As Date
        Static tLin As Date
        Dim i As Integer
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin power-down")
                'Next Subphase
                _subPhase(_phase) = 10

            Case 1

                ' Transmit Frame Diag (Frame, entry Log, Data Value, Sssion Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Go_OFF_Mode),
                                                True,
                                                txData_MasterReq,
                                                cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                True,
                                                True)
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 2

            Case 2
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Go_OFF_Mode))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Go to subphase
                    _subPhase(_phase) = 10
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 10
                End If

            Case 10
                If ((Date.Now - t0).TotalMilliseconds > 100) Then
                    ' Power-down
                    For j = 0 To mWS03DIOManager.eDigitalOutput.Count - 1
                        e = e Or mWS03DIOManager.ResetDigitalOutput(j)
                    Next
                    e = mWS03DIOManager.SetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_LocalSensing)
                    e = e Or mWS03DIOManager.ResetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_RemoteSensing)
                    ' Power-down the communication
                    e = e Or _LinInterface.PowerDown()
                    ' Add log entry
                    AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to phase WriteResults
                    _phase = ePhase.WriteResults
                End If

        End Select

        If TestMode = eTestMode.Debug Or TestMode = eTestMode.Remote Then
            e = False ' Tmp David DBE
        End If
        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS03Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub

    Private Sub AddLogEntry(ByVal logEntry As String)
        Dim t As Date
        ' Get the date
        t = Date.Now
        ' Update the log
        _log.Append(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss:fff") &
                    " - " & logEntry & vbCrLf)
    End Sub

    Private Function CheckPartTypeValue(ByRef resultValue As cResultValue,
                                    ByRef resultValuePT As cResultValue) As Boolean
        ' If the DUT and part type value test results are not the same
        If (resultValue.TestResult <> resultValuePT.TestResult) Then
            ' Set the value test result to NotCoherent
            resultValue.TestResult = cResultValue.eValueTestResult.NotCoherent
            ' Return True
            CheckPartTypeValue = True
        Else    ' Otherwise, if the DUT and part type value test results are the same
            ' Return False
            CheckPartTypeValue = False
        End If
    End Function



    Private Function CheckMasterReference() As Boolean
        Dim i As Integer
        Dim r As Integer
        Dim name As String

        ' Build the file path
        name = CStr(_results.RecipeName.Value)

        _recipeMaster = New cWS03Recipe

        CheckMasterReference = False

        Try
            _recipeMaster.LoadConfiguration(_settings.RecipeConfigurationPath)
            ' If the recipe in progress loading succeeds
            If Not (_recipe.Load(name)) Then
                MasterReference = True
                ' If the recipe of master reference loading succeeds
                If Not (_recipeMaster.Load(name)) Then
                    'Check parameter
                    For j = 10 To cWS03Recipe.ValueCount - 1
                        If _recipeMaster.Value(j) IsNot Nothing Then
                            If (_recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                                If (_recipeMaster.Value(j).MinimumLimit <> _recipe.Value(j).MinimumLimit) Then
                                    ' Return error 
                                    _results.TestResult = cWS03Results.eTestResult.FailedMasterReference
                                    CheckMasterReference = True
                                ElseIf (_recipeMaster.Value(j).MaximumLimit <> _recipe.Value(j).MaximumLimit) Then
                                    ' Return error 
                                    _results.TestResult = cWS03Results.eTestResult.FailedMasterReference
                                    CheckMasterReference = True
                                End If
                            ElseIf (_recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                                If (_recipeMaster.Value(j).Value <> _recipe.Value(j).Value) Then
                                    ' Return error 
                                    _results.TestResult = cWS03Results.eTestResult.FailedMasterReference
                                    CheckMasterReference = True
                                End If
                            End If
                        End If
                    Next j
                    ' Otherwise, if the recipe master reference loading fails
                Else
                    ' Return error 
                    _results.TestResult = cWS03Results.eTestResult.FailedMasterReference
                    CheckMasterReference = True
                End If
                ' Otherwise, if the recipe in progress loading fails
            Else
                ' Return True
                _results.TestResult = cWS03Results.eTestResult.FailedMasterReference
                CheckMasterReference = True
            End If

        Catch ex As Exception
            ' Return True
            _results.TestResult = cWS03Results.eTestResult.FailedMasterReference
            CheckMasterReference = True

        End Try

        MasterReference = False
    End Function


    Private Sub ClearResults()
        Dim i As Integer
        ' Clear all the values
        For i = 0 To cWS03Results.ValueCount - 1
            If (_results.Value(i) IsNot Nothing) Then
                If (_results.Value(i).ValueType = cResultValue.eValueType.BCDValue OrElse
                    _results.Value(i).ValueType = cResultValue.eValueType.HexValue OrElse
                    _results.Value(i).ValueType = cResultValue.eValueType.StringValue) Then
                    _results.Value(i).MinimumLimit = ""
                    _results.Value(i).MaximumLimit = ""
                    _results.Value(i).Value = ""
                    _results.Value(i).TestResult = cResultValue.eValueTestResult.Disabled
                ElseIf (_results.Value(i).ValueType = cResultValue.eValueType.IntegerValue OrElse
                    _results.Value(i).ValueType = cResultValue.eValueType.SingleValue) Then
                    _results.Value(i).MinimumLimit = 0
                    _results.Value(i).MaximumLimit = 0
                    _results.Value(i).Value = 0
                    _results.Value(i).TestResult = cResultValue.eValueTestResult.Disabled
                End If
            End If
        Next
        ' Set the global test result to Unknown
        _results.TestResult = cWS03Results.eTestResult.Unknown
        ' Recipe name
        _results.RecipeName.Value = _reference
        _results.RecipeName.TestResult = cResultValue.eValueTestResult.Disabled
        ' Recipe modify date

        _results.RecipeModifyDate.Value = _recipe.ModifyDate.Value
        _results.RecipeModifyDate.TestResult = cResultValue.eValueTestResult.Disabled
        ' Recipe modify time
        _results.RecipeModifyTime.Value = _recipe.ModifyTime.Value
        _results.RecipeModifyTime.TestResult = cResultValue.eValueTestResult.Disabled
        ' Test date
        _results.TestDate.Value = Format(Date.Now, "dd/MM/yyyy")
        _results.TestDate.TestResult = cResultValue.eValueTestResult.Disabled
        ' Test time
        _results.TestTime.Value = Format(Date.Now, "HH:mm:ss")
        _results.TestTime.TestResult = cResultValue.eValueTestResult.Disabled
        ' Part unique number
        _results.PartUniqueNumber.Value = "0000000000"
        If (_HardwareEnabled_PLC) Then
            _results.PartUniqueNumber.Value = Trim(mWS03Ethernet.InputValue(mWS03Ethernet.eInput.UniqueNumber))
        End If
        _results.PartUniqueNumber.TestResult = cResultValue.eValueTestResult.NotTested
        ' Part type number
        _results.PartTypeNumber.Value = 0
        If (_HardwareEnabled_PLC) Then
            _results.PartTypeNumber.Value = mWS03Ethernet.InputValue(mWS03Ethernet.eInput.PartTypeNumber)
            _results.PartTypeNumber.Value = 0
        End If
        _results.PartTypeNumber.TestResult = cResultValue.eValueTestResult.NotTested
        ' Power UP
        _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Unknown
        _results.Power_supply_voltage.TestResult = cResultValue.eValueTestResult.Unknown
        _results.Power_supply_voltage.MinimumLimit = _recipe.StdSign_Powersupply(0).MinimumLimit
        _results.Power_supply_voltage.Value = 0
        _results.Power_supply_voltage.MaximumLimit = _recipe.StdSign_Powersupply(0).MaximumLimit
        '
        _results.Power_supply_Normal_Current.TestResult = cResultValue.eValueTestResult.Unknown
        _results.Power_supply_Normal_Current.MinimumLimit = _recipe.StdSign_Powersupply(1).MinimumLimit
        _results.Power_supply_Normal_Current.Value = 0
        _results.Power_supply_Normal_Current.MaximumLimit = _recipe.StdSign_Powersupply(1).MaximumLimit
        ' Init Communication
        _results.eINIT_ROOF_LIN_COMMUNICATION.TestResult = cResultValue.eValueTestResult.Unknown
        _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult = cResultValue.eValueTestResult.Unknown
        ' Check Serial Number
        _results.eMMS_Traceability.TestResult = cResultValue.eValueTestResult.Unknown
        _results.Valeo_Serial_Number.TestResult = cResultValue.eValueTestResult.Unknown
        _results.Valeo_Serial_Number.MinimumLimit = Left(_results.PartUniqueNumber.Value & "00000000000000000000", 10)
        _results.Valeo_Serial_Number.MaximumLimit = Left(_results.PartUniqueNumber.Value & "00000000000000000000", 10)
        ' MMS Test Byte
        _results.MMS_Test_Byte_Before.TestResult = cResultValue.eValueTestResult.Unknown
        'Front Left Pull
        If CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) Then
            _results.eFront_Left_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
            _results.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
            _results.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
            _results.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.Front_Left_Pull_DiffS2Ce1.MinimumLimit
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.Front_Left_Pull_DiffS2Ce1.MaximumLimit
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

            If _recipe.Front_Left_Pull_Number_State.Value = 2 Then
                _results.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                _results.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                _results.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                _results.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.Front_Left_Pull_DiffS5Ce2.MinimumLimit
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.Front_Left_Pull_DiffS5Ce2.MaximumLimit
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
            End If

            _results.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
            _results.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
            _results.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
            _results.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Front_Left_PULL_Strenght.Value) Then
                _results.eFront_Left_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                _results.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                _results.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                _results.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                _results.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                _results.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                _results.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                If _recipe.Front_Left_Pull_Number_State.Value = 2 Then
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                    _results.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                    _results.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                    _results.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit
                    _results.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit
                    _results.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                    _results.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                End If
            End If
            ' Init data for Electrical Test Off/Ce1/Ce2
            For i = 0 To 2
                If CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) Then
                    If i = 0 Then
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = 13.4
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = 13.6
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                        'Jama Function
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.Jama_Function_Down.MinimumLimit
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.Jama_Function_Down.MaximumLimit
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                    ElseIf i = 1 Or (i = 2 And _recipe.Front_Left_Pull_Number_State.Value = 2) Then
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = 0
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = 0.2
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                        'Jama Function
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = _recipe.Jama_Function_Down.MinimumLimit
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = _recipe.Jama_Function_Down.MaximumLimit
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                    End If

                    If _recipe.Front_Left_Pull_Number_State.Value = 2 Then
                        If i < 2 Then
                            _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = 13.4
                            _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = 13.6
                        Else
                            _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = 0
                            _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = 0.2
                        End If
                        _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                    End If
                End If

                If CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) Then
                    If i < 2 Or (i = 2 And _recipe.Front_Left_Push_Number_State.Value = 2) Then
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = 13.4
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = 13.6
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                    End If

                    If _recipe.Front_Left_Push_Number_State.Value = 2 Then
                        _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MinimumLimit = 13.4
                        _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).MaximumLimit = 13.6
                        _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                    End If
                End If
            Next i
        End If

        'Front Left Push
        If CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) Then
            _results.eFront_Left_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
            _results.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
            _results.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
            _results.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.Front_Left_Push_DiffS2Ce1.MinimumLimit
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.Front_Left_Push_DiffS2Ce1.MaximumLimit
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

            If _recipe.Front_Left_Push_Number_State.Value = 2 Then
                _results.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                _results.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                _results.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                _results.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.Front_Left_Push_DiffS5Ce2.MinimumLimit
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.Front_Left_Push_DiffS5Ce2.MaximumLimit
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
            End If

            _results.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
            _results.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
            _results.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
            _results.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Front_Left_PUSH_Strenght.Value) Then
                _results.eFront_Left_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                _results.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                _results.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                _results.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                _results.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                _results.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                _results.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                If _recipe.Front_Left_Push_Number_State.Value = 2 Then
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                    _results.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                    _results.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                    _results.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit
                    _results.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit
                    _results.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                    _results.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                End If
            End If
            ' Init data for Electrical Test Off/Ce1/Ce2
            For i = 0 To 2
                If CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value) Then
                    If i < 2 Or (i = 2 And _recipe.Front_Left_Pull_Number_State.Value = 2) Then
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = 13.4
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = 13.6
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Pull_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                    End If
                    If _recipe.Front_Left_Pull_Number_State.Value = 2 Then
                        _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = 13.4
                        _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = 13.6
                        _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                    End If
                End If

                If CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value) Then
                    If i = 0 Then
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = 13.4
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = 13.6
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                        'Jama Function
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.Jama_Function_Down.MinimumLimit
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.Jama_Function_Down.MaximumLimit
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                    ElseIf i = 1 Or (i = 2 And _recipe.Front_Left_Push_Number_State.Value = 2) Then
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = 0
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = 0.2
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown

                        'Jama Function
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = _recipe.Jama_Function_Down.MinimumLimit
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = _recipe.Jama_Function_Down.MaximumLimit
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                    End If

                    If _recipe.Front_Left_Push_Number_State.Value = 2 Then
                        If i < 2 Then
                            _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = 13.4
                            _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = 13.6
                        Else
                            _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MinimumLimit = 0
                            _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).MaximumLimit = 0.2
                        End If
                        _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).Value = 0
                        _results.WL_Front_Left_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontLeft).TestResult = cResultValue.eValueTestResult.Unknown
                    End If
                End If
            Next i
        End If

        'Front Right UP
        If CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) Then
            _results.eFront_Right_Pull_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
            _results.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
            _results.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
            _results.WL_XCe1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.Front_Right_Pull_DiffS2Ce1.MinimumLimit
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.Front_Right_Pull_DiffS2Ce1.MaximumLimit
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

            If _recipe.Front_Right_Pull_Number_State.Value = 2 Then
                _results.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                _results.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                _results.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                _results.WL_XCe2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.Front_Right_Pull_DiffS5Ce2.MinimumLimit
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.Front_Right_Pull_DiffS5Ce2.MaximumLimit
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
            End If

            _results.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
            _results.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
            _results.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
            _results.WL_Xe(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Front_Right_PULL_Strenght.Value) Then
                _results.eFront_Right_Pull_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                _results.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                _results.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                _results.WL_Xs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                _results.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                _results.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                _results.WL_dXs1(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                If _recipe.Front_Right_Pull_Number_State.Value = 2 Then
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                    _results.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                    _results.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                    _results.WL_Xs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit
                    _results.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit
                    _results.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                    _results.WL_dXs2(mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                End If
            End If
            ' Init data for Electrical Test Off/Ce1/Ce2
            For i = 0 To 2
                If CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) Then
                    If i = 0 Then
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = 13.4
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = 13.6
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                        'Jama Function
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.Jama_Function_Down.MinimumLimit
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.Jama_Function_Down.MaximumLimit
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    ElseIf i = 1 Or (i = 2 And _recipe.Front_Right_Pull_Number_State.Value = 2) Then
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = 0
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = 0.2
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                        'Jama Function
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = _recipe.Jama_Function_Down.MinimumLimit
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = _recipe.Jama_Function_Down.MaximumLimit
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    End If

                    If _recipe.Front_Right_Pull_Number_State.Value = 2 Then
                        If i < 2 Then
                            _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = 13.4
                            _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = 13.6
                        Else
                            _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = 0
                            _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = 0.2
                        End If
                        _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    End If
                End If
                If CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) Then
                    If i < 2 Or (i = 2 And _recipe.Front_Right_Pull_Number_State.Value = 2) Then
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = 13.4
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = 13.6
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    End If
                    If _recipe.Front_Right_Push_Number_State.Value = 2 Then
                        _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MinimumLimit = 13.4
                        _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).MaximumLimit = 13.6
                        _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Pull, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    End If
                End If
            Next i
        End If

        'Front Right DN
        If CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) Then
            _results.eFront_Right_Push_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
            _results.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
            _results.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
            _results.WL_XCe1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.Front_Right_Push_DiffS2Ce1.MinimumLimit
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.Front_Right_Push_DiffS2Ce1.MaximumLimit
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
            _results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

            If _recipe.Front_Right_Push_Number_State.Value = 2 Then
                _results.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                _results.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                _results.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                _results.WL_XCe2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.Front_Right_Push_DiffS5Ce2.MinimumLimit
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.Front_Right_Push_DiffS5Ce2.MaximumLimit
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                _results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
            End If

            _results.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
            _results.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
            _results.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
            _results.WL_Xe(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Front_Right_PUSH_Strenght.Value) Then
                _results.eFront_Right_Push_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                _results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                _results.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                _results.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                _results.WL_Xs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                _results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                _results.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                _results.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                _results.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                _results.WL_dXs1(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                If _recipe.Front_Right_Push_Number_State.Value = 2 Then
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                    _results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                    _results.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                    _results.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                    _results.WL_Xs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                    _results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                    _results.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit
                    _results.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit
                    _results.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                    _results.WL_dXs2(mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                End If
            End If
            ' Init data for Electrical Test Off/Ce1/Ce2
            For i = 0 To 2
                If CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value) Then
                    If i < 2 Or (i = 2 And _recipe.Front_Right_Pull_Number_State.Value = 2) Then
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = 13.4
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = 13.6
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Pull_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    End If

                    If _recipe.Front_Right_Pull_Number_State.Value = 2 Then
                        _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = 13.4
                        _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = 13.6
                        _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    End If
                End If
                If CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value) Then
                    If i = 0 Then
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = 13.4
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = 13.6
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                        'Jama Function
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.Jama_Function_Down.MinimumLimit
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.Jama_Function_Down.MaximumLimit
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    ElseIf i = 1 Or (i = 2 And _recipe.Front_Right_Push_Number_State.Value = 2) Then
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = 0
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = 0.2
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown

                        'Jama Function
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = _recipe.Jama_Function_Down.MinimumLimit
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = _recipe.Jama_Function_Down.MaximumLimit
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Jama_Down(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    End If

                    If _recipe.Front_Right_Push_Number_State.Value = 2 Then
                        If i < 2 Then
                            _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = 13.4
                            _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = 13.6
                        Else
                            _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MinimumLimit = 0
                            _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).MaximumLimit = 0.2
                        End If
                        _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).Value = 0
                        _results.WL_Front_Right_Push_Automatic(i, mGlobal.ePush_Pull.Push, eWindows.FrontRight).TestResult = cResultValue.eValueTestResult.Unknown
                    End If
                End If

            Next i
        End If

        For i = 0 To 3
            _results.WL_X_Indexes(i).X_F1 = 0
            _results.WL_X_Indexes(i).X_F2 = 0
            _results.WL_X_Indexes(i).X_F4 = 0
            _results.WL_X_Indexes(i).X_F5 = 0
        Next

        _results.CL_X_Indexes.X_F1 = 0
        _results.CL_X_Indexes.X_F2 = 0

        If _recipe.TestEnable_ChildrenLock_Electrical.Value Then
            _results.eChildren_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.CL_XCe1.TestResult = cResultValue.eValueTestResult.Unknown
            _results.CL_XCe1.MinimumLimit = _recipe.CL_XCe1.MinimumLimit
            _results.CL_XCe1.MaximumLimit = _recipe.CL_XCe1.MaximumLimit

            _results.CL_DiffS2Ce1.TestResult = cResultValue.eValueTestResult.Unknown
            _results.CL_DiffS2Ce1.MinimumLimit = _recipe.CL_DiffS2Ce1.MinimumLimit
            _results.CL_DiffS2Ce1.MaximumLimit = _recipe.CL_DiffS2Ce1.MaximumLimit

            _results.CL_Xe.TestResult = cResultValue.eValueTestResult.Unknown
            _results.CL_Xe.MinimumLimit = _recipe.CL_Xe.MinimumLimit
            _results.CL_Xe.MaximumLimit = _recipe.CL_Xe.MaximumLimit

            '_results.BakcInitialSate.MinimumLimit = 13.4
            '_results.BakcInitialSate.MaximumLimit = 13.6
            '_results.BakcInitialSate.TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_ChildrenLock_Strenght.Value) Then
                _results.eChildren_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.CL_Fs1_F1.TestResult = cResultValue.eValueTestResult.Unknown
                _results.CL_Fs1_F1.MinimumLimit = _recipe.CL_Fs1_F1.MinimumLimit
                _results.CL_Fs1_F1.MaximumLimit = _recipe.CL_Fs1_F1.MaximumLimit

                _results.CL_Xs1.TestResult = cResultValue.eValueTestResult.Unknown
                _results.CL_Xs1.MinimumLimit = _recipe.CL_Xs1.MinimumLimit
                _results.CL_Xs1.MaximumLimit = _recipe.CL_Xs1.MaximumLimit

                _results.CL_dFs1_Haptic_1.TestResult = cResultValue.eValueTestResult.Unknown
                _results.CL_dFs1_Haptic_1.MinimumLimit = _recipe.CL_dFs1_Haptic_1.MinimumLimit
                _results.CL_dFs1_Haptic_1.MaximumLimit = _recipe.CL_dFs1_Haptic_1.MaximumLimit

                _results.CL_dXs1.TestResult = cResultValue.eValueTestResult.Unknown
                _results.CL_dXs1.MinimumLimit = _recipe.CL_dXs1.MinimumLimit
                _results.CL_dXs1.MaximumLimit = _recipe.CL_dXs1.MaximumLimit
            End If
            ' Init data for Electrical Test Off/Ce1
            For i = 0 To 1
                _results.Push_CL_Electric(i).TestResult = cResultValue.eValueTestResult.Unknown

                If i = 0 Then
                    _results.Push_CL_Electric(i).MinimumLimit = 13.4
                    _results.Push_CL_Electric(i).MaximumLimit = 13.6
                Else
                    _results.Push_CL_Electric(i).MinimumLimit = 0
                    _results.Push_CL_Electric(i).MaximumLimit = 0.6
                End If

            Next i
        End If
        ' Final State
        For i = 0 To 27
            _results.eFINAL_STATE_PRODUCT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.FinalState(i).TestResult = cResultValue.eValueTestResult.Unknown
            _results.FinalState(i).MinimumLimit = 13.4
            _results.FinalState(i).MaximumLimit = 13.6
            _results.FinalState(i).Value = 0
        Next
        _results.FinalState(0).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(1).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(8).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(9).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(10).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(11).TestResult = cResultValue.eValueTestResult.Disabled

        _results.eWrite_MMSTestByte.TestResult = cResultValue.eValueTestResult.Unknown
        _results.eRead_MMSTestByte.TestResult = cResultValue.eValueTestResult.Unknown

        _results.MMS_Test_Byte_After.TestResult = cResultValue.eValueTestResult.Unknown

    End Sub


    Private Sub ProcessSamples_BaseTps(ByVal WiliIndex As cWS03Results.eWindowsLifterTest, ByRef WiliResult As sWiliResult)
        ' WiliIndex : 0 = Windows Lifter Rear Left UP
        '           : 1 = Windows Lifter Rear Left DN
        '           : 2 = Windows Lifter Rear Right UP
        '           : 3 = Windows Lifter Rear Right DN
        '           : 4 = Windows Lifter Rear Left UP
        '           : 5 = Windows Lifter Rear Left DN
        '           : 6 = Windows Lifter Rear Right UP
        '           : 7 = Windows Lifter Rear Right DN

        ' analogInputIndex: 0 = Early Sensor
        '                 : 1 = Strengh Sensor
        '                 : 2 = Windows Lifter Rear Left UP
        '                 : 3 = Windows Lifter Rear Left DN
        '                 : 4 = Windows Lifter Rear Right UP
        '                 : 5 = Windows Lifter Rear Right DN
        '                 : 6 = Windows Lifter Rear Left UP
        '                 : 7 = Windows Lifter Rear Left DN
        '                 : 8 = Windows Lifter Rear Right UP
        '                 : 9 = Windows Lifter Rear Right DN

        Dim sampleIndex As Integer
        Dim n As Integer = 40
        Dim df() As Single = Nothing
        Dim i As Integer
        Dim Fe1 As Single
        Dim Fe2 As Single
        Dim Recalage As Integer
        Dim RecalageCe As Integer
        Dim Pin_Analisys As Integer
        Dim Number_of_State As Integer
        Dim Vth As Integer = 6
        ' Filter the samples
        Try

            If (_results.WS03_SampleCount(WiliIndex) > 1) Then
                ReDim df(0 To _results.WS03_SampleCount(WiliIndex) - 1)
            Else
                Exit Sub
            End If

            'Recalibration of the force curve in UP function the force is negative
            If WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Pull Or
                WiliIndex = cWS03Results.eWindowsLifterTest.FrontRight_Pull Then
                If _results.WS03_SampleCount(WiliIndex) > 1 Then
                    For sampleIndex = 0 To _results.WS03_SampleCount(WiliIndex)
                        _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) =
                        _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) * -1
                    Next
                End If
            End If
            ' Search the contact stroke
            WiliResult.Xs = -1
            WiliResult.Xe = -1
            WiliResult.Xs_Index = 0
            WiliResult.Xe_Index = 0
            WiliResult.XCe1 = 0
            WiliResult.XCe2 = 0
            'Find the Xs reference 0
            For sampleIndex = _results.WS03_SampleCount(WiliIndex) * 7 / 10 To 0 Step -1
                If (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) <= 0.5) Then
                    If WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Pull OrElse WiliIndex = cWS03Results.eWindowsLifterTest.FrontRight_Pull Then
                        WiliResult.Xs = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) * _recipe.Correlation_Factor_Stroke_A(WiliIndex).Value + _recipe.Correlation_Factor_Stroke_B(WiliIndex).Value
                    Else
                        WiliResult.Xs = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) * _recipe.Correlation_Factor_Stroke_A(WiliIndex).Value - _recipe.Correlation_Factor_Stroke_B(WiliIndex).Value
                    End If
                    _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) = WiliResult.Xs
                    WiliResult.Xs_Index = sampleIndex
                    WiliResult.Xe_Index = sampleIndex + 10
                    Exit For
                End If
            Next
            If WiliResult.Xs = -1 Then
                AddLogEntry("WiliResult.Xs = -1")
                WiliResult.Xs = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, 1)
            End If
            ' Search the maximum stroke
            WiliResult.Fe = 0 : WiliResult.Xe = 0 : WiliResult.Xe_Index = _results.WS03_SampleCount(WiliIndex)
            For sampleIndex = _results.WS03_SampleCount(WiliIndex) - 1 To _results.WS03_SampleCount(WiliIndex) / 2 Step -1
                If (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) > WiliResult.Fe) Then
                    WiliResult.Fe = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex)
                    WiliResult.Xe = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) * _recipe.Correlation_Factor_Stroke_A(WiliIndex).Value - WiliResult.Xs)
                    WiliResult.Xe_Index = sampleIndex
                End If
            Next

            'Lab Correlation
            If _results.WS03_SampleCount(WiliIndex) > 1 Then
                For sampleIndex = WiliResult.Xs_Index + 1 To _results.WS03_SampleCount(WiliIndex) - 1
                    _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) =
                        _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) *
                        _recipe.Correlation_Factor_Force_A(WiliIndex).Value + _recipe.Correlation_Factor_Force_B(WiliIndex).Value
                    _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) =
                        _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) *
                        _recipe.Correlation_Factor_Stroke_A(WiliIndex).Value
                Next
            End If

            AddLogEntry("Sample Index Xs : " & WiliResult.Xs_Index)
            AddLogEntry("Early Xs : " & WiliResult.Xs)
            AddLogEntry("Sample Index Xe : " & WiliResult.Xe_Index)
            AddLogEntry("Early Xe : " & WiliResult.Xe)
            AddLogEntry("Early Fe : " & WiliResult.Fe)
            'During Robot Slip, add below code. important is use trajectory of robot
            'If (WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Push OrElse
            '    WiliIndex = cWS03Results.eWindowsLifterTest.FrontRight_Push) Then
            '    'Only For push use stroke as maximum stroke. temperally before introduce trajectory of robot.
            '    ' Search the maximum stroke
            '    Dim tempXe As Single = 0
            '    WiliResult.Fe = 0 : WiliResult.Xe = 0 : WiliResult.Xe_Index = _results.WS03_SampleCount(WiliIndex)
            '    For sampleIndex = _results.WS03_SampleCount(WiliIndex) - 1 To _results.WS03_SampleCount(WiliIndex) / 2 Step -1
            '        If (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) > tempXe) Then
            '            tempXe = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex)
            '            WiliResult.Fe = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex - 10)
            '            WiliResult.Xe = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex - 10) - WiliResult.Xs)
            '            WiliResult.Xe_Index = sampleIndex - 10
            '        End If
            '    Next
            '    AddLogEntry("Stroke Calculation: Sample Index Xs : " & WiliResult.Xs_Index)
            '    AddLogEntry("Stroke Calculation: Early Xs : " & WiliResult.Xs)
            '    AddLogEntry("Stroke Calculation: Sample Index Xe : " & WiliResult.Xe_Index)
            '    AddLogEntry("Stroke Calculation: Early Xe : " & WiliResult.Xe)
            '    AddLogEntry("Stroke Calculation: Early Fe : " & WiliResult.Fe)
            'End If
            If WiliResult.Xe = -1 Then WiliResult.Xe = WiliResult.Xs + 1

            If WiliIndex = cWS03Results.eWindowsLifterTest.FrontRight_Push Then
                Pin_Analisys = cWS03Results.eWindowsLifterSignal.Push_Manual + 2
                Number_of_State = _recipe.Front_Right_Push_Number_State.Value
            ElseIf WiliIndex = cWS03Results.eWindowsLifterTest.FrontRight_Pull Then
                Pin_Analisys = cWS03Results.eWindowsLifterSignal.Pull_Manual + 2
                Number_of_State = _recipe.Front_Right_Pull_Number_State.Value
            ElseIf WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Push Then
                Pin_Analisys = cWS03Results.eWindowsLifterSignal.Push_Manual + 2
                Number_of_State = _recipe.Front_Left_Push_Number_State.Value
            ElseIf WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Pull Then
                Pin_Analisys = cWS03Results.eWindowsLifterSignal.Pull_Manual + 2
                Number_of_State = _recipe.Front_Left_Pull_Number_State.Value
            End If

            If _results.WS03_SampleCount(WiliIndex) > 0 Then
                ' Search the commutation stroke
                WiliResult.XCe1 = 0
                For sampleIndex = WiliResult.Xs_Index To _results.WS03_SampleCount(WiliIndex) - 2
                    If (_results.Samples_Push_Pull(WiliIndex, Pin_Analisys, sampleIndex) > Vth AndAlso
                    _results.Samples_Push_Pull(WiliIndex, Pin_Analisys, sampleIndex + 1) <= Vth) Then
                        WiliResult.XCe1 = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) - WiliResult.Xs)
                        WiliResult.FsCe1 = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) - WiliResult.Xs)
                        WiliResult.X_Ce1_index = sampleIndex + 1
                        Exit For
                    End If
                Next
                AddLogEntry("X_Ce1_index : " & WiliResult.X_Ce1_index)
                AddLogEntry("XCe1 : " & WiliResult.XCe1)
                '
                If Number_of_State = 2 Then
                    WiliResult.XCe2 = 0
                    For sampleIndex = sampleIndex To _results.WS03_SampleCount(WiliIndex) - 2
                        If (_results.Samples_Push_Pull(WiliIndex, Pin_Analisys + 1, sampleIndex) > Vth AndAlso
                        _results.Samples_Push_Pull(WiliIndex, Pin_Analisys + 1, sampleIndex + 1) <= Vth) Then
                            WiliResult.XCe2 = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) - WiliResult.Xs)
                            WiliResult.FsCe2 = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) - WiliResult.Xs)
                            WiliResult.X_Ce2_index = sampleIndex + 1
                            Exit For
                        End If
                    Next
                    AddLogEntry("X_Ce2_index : " & WiliResult.X_Ce2_index)
                    AddLogEntry("XCe2 : " & WiliResult.XCe2)
                Else
                    WiliResult.XCe2 = 0
                End If

                '' Calculate the 1st derivative of the force
                'For i = n / 2 To _results.WS03_SampleCount(WiliIndex) - n / 2 - 1
                '    df(i) = (_results.WS03_Sample(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i + n / 2) -
                '         _results.WS03_Sample(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i - n / 2)) * 1
                'Next

                ' Search the force peak and the delta stroke
                WiliResult.Fs1_F1 = -1
                WiliResult.Xs1 = 0
                WiliResult.Fs2_F2 = -1
                WiliResult.Xs2 = 0
                WiliResult.Xs_F1_index = -1
                WiliResult.Xs_F2_index = -1
                WiliResult.Xs_F4_index = -1
                WiliResult.Xs_F5_index = -1
                WiliResult.X_Fe1_index = -1
                WiliResult.X_Fe2_index = -1
                WiliResult.dFs1_Haptic_1 = -1
                WiliResult.dFs2_Haptic_2 = -1


                '******************************************************************
                Recalage = 0
                RecalageCe = 0
                ' Recherche sur courbe
LoopSnap1:      WiliResult.Fs1_F1 = 0 : WiliResult.Fs1_F11 = 100 : WiliResult.Xs_F1_index = 0 : WiliResult.Xs_F2_index = 0
                'Find Peak Force 1
                For i = WiliResult.Xs_Index To WiliResult.X_Ce1_index - Recalage + RecalageCe + 150 '+ 0
                    If _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) >= WiliResult.Fs1_F1 Then
                        WiliResult.Fs1_F1 = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i)
                        WiliResult.Xs1 = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, i) - WiliResult.Xs)
                        WiliResult.Fs1_F11 = WiliResult.Fs1_F1
                        WiliResult.Xs_F1_index = i
                        WiliResult.Xs_F2_index = i
                    End If
                Next
                For i = WiliResult.Xs_F1_index To WiliResult.Xe_Index - 10
                    'define the F2
                    If _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) < WiliResult.Fs1_F11 AndAlso
                    _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) <> 0 Then
                        WiliResult.Fs1_F11 = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i)
                        WiliResult.Xs_F2_index = i
                        WiliResult.dXs1 = Abs((_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, WiliResult.Xs_F2_index) - WiliResult.Xs)) ' WIliResult.Xs1
                    End If
                    'Check position F2
                    If WiliResult.Fs1_F11 > WiliResult.Fs1_F1 Or (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i + 10) >
                                              WiliResult.Fs1_F1 And _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i + 10) <> 0) Then
                        Exit For
                    End If
                Next

                If WiliResult.Fs1_F1 = WiliResult.Fs1_F11 Or WiliResult.Fs1_F11 > WiliResult.Fs1_F1 - 0.5 Then
                    Recalage = Recalage + 10
                    If Recalage < WiliResult.X_Ce1_index Then
                        GoTo LoopSnap1
                    End If
                End If
                'if the F1 is after CE1
                If WiliResult.Fs1_F1 = 0 And WiliResult.Fs1_F11 = 100 Then
                    RecalageCe = RecalageCe + 10
                    If RecalageCe < WiliResult.Xe_Index Then
                        GoTo LoopSnap1
                    End If
                End If
                ' calculate Snap CE1
                WiliResult.dFs1_Haptic_1 = WiliResult.Fs1_F11
                AddLogEntry("F1: " & WiliResult.Fs1_F1.ToString())
                AddLogEntry("F2: " & WiliResult.dFs1_Haptic_1.ToString())
                'If (WIliResult.dFs1_Haptic_1 = -1) And WIliResult.Fs1_F11 <> -1 Then
                '    WIliResult.dFs1_Haptic_1 = WIliResult.Fs1_F1 - WIliResult.Fs1_F11
                If (WiliResult.Fs1_F1 > 0) Then
                    WiliResult.dFs1_Haptic_1 = Format((WiliResult.dFs1_Haptic_1 / WiliResult.Fs1_F1) * 100, "#0.00")
                Else
                    WiliResult.dFs1_Haptic_1 = 0
                End If
                AddLogEntry("F2/F1: " & WiliResult.dFs1_Haptic_1.ToString())
                'End If
                WiliResult.dXs1 = Abs((_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, WiliResult.Xs_F2_index) - WiliResult.Xs)) ' WIliResult.Xs1
                'Console.WriteLine("WiliResult.Value_DiffS2Ce1" + WiliResult.Value_DiffS2Ce1.ToString())

                Fe1 = WiliResult.Fs1_F11 + (1 * (WiliResult.Fs1_F1 - WiliResult.Fs1_F11))

                For i = WiliResult.Xs_F2_index To WiliResult.Xe_Index
                    If _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) >= Fe1 AndAlso WiliResult.X_Fe1_index = -1 Then
                        WiliResult.X_Fe1_index = i
                    End If
                    If _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) >= 2 * WiliResult.Fs1_F1 AndAlso WiliResult.FsCe1 = -1 Then
                        WiliResult.FsCe1 = i
                    End If
                Next
                If WiliResult.FsCe1 = -1 Then
                    WiliResult.FsCe1 = WiliResult.Xe_Index
                End If

                'New, to search curve rate, to find the closest F2.YAN.Qian20200310.
                AddLogEntry("WiliIndex : " & WiliIndex.ToString())
                AddLogEntry("Fs1_F11 : " & WiliResult.Fs1_F11)
                AddLogEntry("Xs_F2_index : " & WiliResult.Xs_F2_index)
                AddLogEntry("dXs1 : " & WiliResult.dXs1)
                Dim dfmaxrate As Single = 0
                Dim currentrate As Single = 0
                Dim findmax As Boolean = False
                Dim tempFs1_F11 As Single = 0
                Dim tempXs_F2_index As Single = 0
                Dim tempdXs1 As Single = 0
                For i = WiliResult.Xs_F2_index To WiliResult.Xe_Index - 10
                    If (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) > WiliResult.Fs1_F11 + 0.2) Then
                        'Current force bigger than Lowest F2 0.2N, jump out. not the "Real F2"
                        Exit For
                    End If
                    currentrate = (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i + 10) -
                                 _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i - 10) / _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i - 10)) * 20
                    If (currentrate > 0 AndAlso dfmaxrate < currentrate) Then
                        dfmaxrate = currentrate
                        findmax = True
                        tempFs1_F11 = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i)
                        tempXs_F2_index = i
                        tempdXs1 = Abs((_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, i) - WiliResult.Xs)) ' WIliResult.Xs1
                    End If
                Next
                If (findmax) Then
                    WiliResult.Fs1_F11 = tempFs1_F11
                    WiliResult.Xs_F2_index = tempXs_F2_index
                    WiliResult.dXs1 = tempdXs1
                End If
                AddLogEntry("New Fs1_F11 : " & WiliResult.Fs1_F11)
                AddLogEntry("New Xs_F2_index :  " & WiliResult.Xs_F2_index)
                AddLogEntry("New dXs1 : " & WiliResult.dXs1)
                'New, to search curve rate, to find the closest F2.YAN.Qian20200310.

                Fe1 = WiliResult.Fs1_F11 + (1 * (WiliResult.Fs1_F1 - WiliResult.Fs1_F11))
                WiliResult.Value_DiffS2Ce1 = WiliResult.dXs1 - WiliResult.XCe1

                If Number_of_State > 1 Then
                    'Find Peak Force 2
                    Recalage = 0
                    RecalageCe = 0
LoopSnap2:          WiliResult.Fs2_F2 = 0 : WiliResult.Fs2_F21 = 100 : WiliResult.Xs_F4_index = 0 : WiliResult.Xs_F5_index = 0
                    For i = WiliResult.Xs_F2_index To WiliResult.X_Ce2_index - Recalage + RecalageCe + 150 '+ 0
                        If _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) >= WiliResult.Fs2_F2 Then
                            WiliResult.Fs2_F2 = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i)
                            WiliResult.Xs2 = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, i) - WiliResult.Xs)
                            WiliResult.Fs2_F21 = WiliResult.Fs2_F2
                            WiliResult.Xs_F4_index = i
                            WiliResult.Xs_F5_index = i
                        End If
                    Next
                    For i = WiliResult.Xs_F4_index To WiliResult.Xe_Index  ' SampleIndex_Ce2 + 100
                        If _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) < WiliResult.Fs2_F21 AndAlso _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) <> 0 Then
                            WiliResult.Fs2_F21 = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i)
                            WiliResult.Xs_F5_index = i
                            WiliResult.dXs2 = Abs(_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, WiliResult.Xs_F5_index) - WiliResult.Xs) '- WIliResult.Xs2
                        End If
                        If WiliResult.Fs2_F21 > WiliResult.Fs2_F2 Or (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i + 10) >
                                                  WiliResult.Fs2_F2 And
                                                  _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i + 10) <> 0) Then
                            Exit For
                        End If
                    Next

                    If WiliResult.Fs2_F2 = WiliResult.Fs2_F21 Or WiliResult.Fs2_F21 > WiliResult.Fs2_F2 - 0.5 Then
                        Recalage = Recalage + 10
                        If Recalage < WiliResult.X_Ce2_index Then
                            GoTo LoopSnap2
                        End If
                    End If

                    'if the F1 is after CE1
                    If WiliResult.Fs2_F2 = 0 And WiliResult.Fs2_F21 = 100 Then
                        RecalageCe = RecalageCe + 10
                        If RecalageCe < WiliResult.Xe_Index Then
                            GoTo LoopSnap2
                        End If
                    End If

                    Fe2 = WiliResult.Fs2_F21 + (1 * (WiliResult.Fs2_F2 - WiliResult.Fs2_F21))
                    'Console.WriteLine("WiliResult.Value_DiffS5Ce2" + WiliResult.Value_DiffS5Ce2.ToString())
                    For i = WiliResult.Xs_F5_index To WiliResult.Xe_Index
                        If _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) >= Fe2 AndAlso WiliResult.X_Fe2_index = -1 Then
                            WiliResult.X_Fe2_index = i
                        End If
                        If _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) >= 2 * WiliResult.Fs2_F2 AndAlso WiliResult.FsCe2 = -1 Then
                            WiliResult.FsCe2 = i
                        End If
                    Next
                    If WiliResult.FsCe2 = -1 Then
                        WiliResult.FsCe2 = WiliResult.Xe_Index
                    End If
                    ' Calculate Ratio F4F1
                    WiliResult.dFs2_Haptic_2 = WiliResult.Fs2_F21
                    AddLogEntry("F4: " & WiliResult.Fs2_F2.ToString())
                    AddLogEntry("F5: " & WiliResult.dFs2_Haptic_2.ToString())
                    'If (WIliResult.dFs2_Haptic_2 = -1) And WIliResult.Fs2_F21 <> -1 Then
                    '    WIliResult.dFs2_Haptic_2 = WIliResult.Fs2_F2 - WIliResult.Fs2_F21
                    If (WiliResult.Fs2_F2 > 0) Then
                        WiliResult.dFs2_Haptic_2 = Format(WiliResult.dFs2_Haptic_2 / WiliResult.Fs2_F2 * 100, "#0.00")
                    Else
                        WiliResult.dFs2_Haptic_2 = 0
                    End If
                    AddLogEntry("F5/F4: " & WiliResult.dFs2_Haptic_2.ToString())
                    'End If

                    'New, to search curve rate, to find the closest F2.YAN.Qian20200310.
                    AddLogEntry("Fs2_F21 : " & WiliResult.Fs2_F21)
                    AddLogEntry("Xs_F5_index : " & WiliResult.Xs_F5_index)
                    AddLogEntry("dXs2 : " & WiliResult.dXs2)
                    Dim dfmaxrate2 As Single = 0
                    Dim currentrate2 As Single = 0
                    Dim findmax2 As Boolean = False
                    Dim tempFs2_F21 As Single = 0
                    Dim tempXs_F5_index As Single = 0
                    Dim tempdXs2 As Single = 0
                    For i = WiliResult.Xs_F5_index To WiliResult.Xe_Index - 10
                        If (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i) > WiliResult.Fs2_F21 + 0.2) Then
                            'Current force bigger than Lowest F2 0.2N, jump out. not the "Real F2"
                            Exit For
                        End If
                        currentrate2 = (_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i + 10) -
                                 _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i - 10) / _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i - 10)) * 20
                        If (currentrate2 > 0 AndAlso dfmaxrate2 < currentrate2) Then
                            dfmaxrate2 = currentrate2
                            findmax2 = True
                            tempFs2_F21 = _results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, i)
                            tempXs_F5_index = i
                            tempdXs2 = Abs((_results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, i) - WiliResult.Xs)) ' WIliResult.Xs1
                        End If
                    Next
                    If (findmax2) Then
                        WiliResult.Fs2_F21 = tempFs2_F21
                        WiliResult.Xs_F5_index = tempXs_F5_index
                        WiliResult.dXs2 = tempdXs2
                    End If
                    AddLogEntry("New Fs2_F21 : " & WiliResult.Fs2_F21)
                    AddLogEntry("New Xs_F5_index :  " & WiliResult.Xs_F5_index)
                    AddLogEntry("New dXs2 : " & WiliResult.dXs2)
                    'New, to search curve rate, to find the closest F2.YAN.Qian20200310.
                    Fe2 = WiliResult.Fs2_F21 + (1 * (WiliResult.Fs2_F2 - WiliResult.Fs2_F21))
                    WiliResult.Value_DiffS5Ce2 = WiliResult.dXs2 - WiliResult.XCe2
                Else
                    WiliResult.dFs2_Haptic_2 = 0
                    WiliResult.Fs2_F2 = 0
                End If
            End If
            'Check Data available
            If WiliResult.Fs1_F1 < 0 Then WiliResult.Fs1_F1 = 0
            If WiliResult.Fs2_F2 < 0 Then WiliResult.Fs2_F2 = 0
            If WiliResult.dXs1 < 0 Then WiliResult.dXs1 = 0
            If WiliResult.dXs2 < 0 Then WiliResult.dXs2 = 0
            If WiliResult.Xs1 < 0 Then WiliResult.Xs1 = 0
            If WiliResult.Xs2 < 0 Then WiliResult.Xs2 = 0
            If WiliResult.dFs1_Haptic_1 < 0 Then WiliResult.dFs1_Haptic_1 = 0
            If WiliResult.dFs2_Haptic_2 < 0 Then WiliResult.dFs2_Haptic_2 = 0
            If WiliResult.Fe < 0 Then WiliResult.Fe = 0
            If WiliResult.Xe < 0 Then WiliResult.Xe = 0
            If WiliResult.Xs < 0 Then WiliResult.Xs = 0

        Catch ex As Exception
            Dim err As String
            err = ex.ToString

            _log.Append("Error in Process Sample : " & err)

            'Check Data available
            If WiliResult.Fs1_F1 < 0 Then WiliResult.Fs1_F1 = 0
            If WiliResult.Fs2_F2 < 0 Then WiliResult.Fs2_F2 = 0
            If WiliResult.dXs1 < 0 Then WiliResult.dXs1 = 0
            If WiliResult.dXs2 < 0 Then WiliResult.dXs2 = 0
            If WiliResult.Xs1 < 0 Then WiliResult.Xs1 = 0
            If WiliResult.Xs2 < 0 Then WiliResult.Xs2 = 0
            If WiliResult.dFs1_Haptic_1 < 0 Then WiliResult.dFs1_Haptic_1 = 0
            If WiliResult.dFs2_Haptic_2 < 0 Then WiliResult.dFs2_Haptic_2 = 0
            If WiliResult.Fe < 0 Then WiliResult.Fe = 0
            If WiliResult.Xs < 0 Then WiliResult.Xs = 0
            If WiliResult.Xe < 0 Then WiliResult.Xe = 0
        End Try

    End Sub


    Private Sub ProcessSamples_BaseTps_Push(ByRef WiliResult As sWiliResult)

        ' analogInputIndex: 0 = Early Sensor
        '                 : 1 = Strengh Sensor
        '                 : 2 = Electrical signal

        Dim sampleIndex As Integer
        Dim vth As Single = 6
        Dim n As Integer = 40 '20
        Dim df() As Single
        Dim Fe1 As Single
        Dim Pin_Analisys As Integer = 2
        Dim Recalage As Integer
        Dim RecalageCe As Integer
        ' Filter the samples
        Try
            If (_results.WS03_SampleCountPush > 1) Then
                ReDim df(0 To _results.WS03_SampleCountPush - 1)
            Else
                Exit Sub
            End If

            ' check if the minimum number of sample is ok
            If (_results.WS03_SampleCountPush > 100) Then
                _results.WS03_SampleCountPush = _results.WS03_SampleCountPush - 100
            Else
                _results.WS03_SampleCountPush = 0
            End If
            ' Search the contact stroke
            WiliResult.Xs = -1
            WiliResult.Xe = -1
            WiliResult.Xs_Index = 0
            WiliResult.Xe_Index = 0
            WiliResult.XCe1 = 0
            WiliResult.XCe2 = 0
            'Find the Xs reference 0
            For sampleIndex = _results.WS03_SampleCountPush * 2 / 3 To 0 Step -1
                If (_results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) <= 0.2) Then
                    WiliResult.Xs = _results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, sampleIndex) * _recipe.Correlation_Factor_Stroke_A(4).Value - _recipe.Correlation_Factor_Stroke_B(4).Value
                    _results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, sampleIndex) = WiliResult.Xs
                    WiliResult.Xs_Index = sampleIndex
                    WiliResult.Xe_Index = sampleIndex + 10
                    Exit For
                End If
            Next
            If WiliResult.Xs = -1 Then
                AddLogEntry("WiliResult.Xs = -1")
                WiliResult.Xs = _results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, 1)
            End If
            ' Search the maximum stroke
            WiliResult.Fe = 0 : WiliResult.Xe = 0 : WiliResult.Xe_Index = _results.WS03_SampleCountPush
            For sampleIndex = _results.WS03_SampleCountPush - 1 To _results.WS03_SampleCountPush / 2 Step -1
                If (_results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) > WiliResult.Fe) Then
                    WiliResult.Fe = _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex)
                    WiliResult.Xe = Abs(_results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, sampleIndex) * _recipe.Correlation_Factor_Stroke_A(4).Value - WiliResult.Xs)
                    WiliResult.Xe_Index = sampleIndex
                End If
            Next

            If WiliResult.Xe = -1 Then WiliResult.Xe = WiliResult.Xs + 1

            'Lab Correlation
            If _results.WS03_SampleCountPush > 1 Then
                For sampleIndex = WiliResult.Xs_Index + 1 To _results.WS03_SampleCountPush - 1
                    _results.WS03_SamplePush(mWS04Main.cSample_Signal.StrenghtSensor, sampleIndex) =
                        _results.WS03_SamplePush(mWS04Main.cSample_Signal.StrenghtSensor, sampleIndex) *
                        _recipe.Correlation_Factor_Force_A(4).Value + _recipe.Correlation_Factor_Force_B(4).Value
                    _results.WS03_SamplePush(mWS04Main.cSample_Signal.EarlySensor, sampleIndex) =
                        _results.WS03_SamplePush(mWS04Main.cSample_Signal.EarlySensor, sampleIndex) *
                        _recipe.Correlation_Factor_Stroke_A(4).Value
                Next
            End If

            AddLogEntry("Sample Index Xs : " & WiliResult.Xs_Index)
            AddLogEntry("Early Xs : " & WiliResult.Xs)
            AddLogEntry("Sample Index Xe : " & WiliResult.Xe_Index)
            AddLogEntry("Early Xe : " & WiliResult.Xe)

            If _results.WS03_SampleCountPush > 0 Then
                ' Search the commutation stroke
                WiliResult.XCe1 = 0
                For sampleIndex = WiliResult.Xs_Index To _results.WS03_SampleCountPush - 2
                    If (_results.WS03_SamplePush(Pin_Analisys, sampleIndex) > vth AndAlso
                    _results.WS03_SamplePush(Pin_Analisys, sampleIndex + 1) <= vth) Then
                        WiliResult.XCe1 = Abs(_results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, sampleIndex) - WiliResult.Xs)
                        WiliResult.FsCe1 = Abs(_results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) - WiliResult.Xs)
                        WiliResult.X_Ce1_index = sampleIndex + 1
                        Exit For
                    End If
                Next
                '
                ' Search the force peak and the delta stroke
                WiliResult.Fs1_F1 = -1
                WiliResult.Xs1 = 0
                WiliResult.dFs1_Haptic_1 = -1
                WiliResult.dXs1 = 0
                WiliResult.Xs_F1_index = -1
                WiliResult.Xs_F2_index = -1
                WiliResult.X_Fe1_index = -1


                '******************************************************************
                Recalage = 0
                RecalageCe = 0
                ' Recherche sur courbe
LoopSnap1:      WiliResult.Fs1_F1 = 0 : WiliResult.Fs1_F11 = 100 : WiliResult.Xs_F1_index = 0 : WiliResult.Xs_F2_index = 0
                'Find PeakForce
                For i = WiliResult.Xs_Index To WiliResult.X_Ce1_index - Recalage + RecalageCe
                    If _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i) >= WiliResult.Fs1_F1 Then
                        WiliResult.Fs1_F1 = _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i)
                        WiliResult.Xs1 = Abs(_results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, i) - WiliResult.Xs)
                        WiliResult.Fs1_F11 = WiliResult.Fs1_F1
                        WiliResult.Xs_F1_index = i
                        WiliResult.Xs_F2_index = i
                    End If
                Next
                For i = WiliResult.Xs_F1_index To WiliResult.Xe_Index - 10
                    'define the F2
                    If _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i) < WiliResult.Fs1_F11 AndAlso
                    _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i) <> 0 Then
                        WiliResult.Fs1_F11 = _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i)
                        WiliResult.Xs_F2_index = i
                        WiliResult.dXs1 = Abs((_results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, WiliResult.Xs_F2_index) - WiliResult.Xs)) ' WIliResult.Xs1
                    End If
                    'Check position F2
                    If WiliResult.Fs1_F11 > WiliResult.Fs1_F1 Or (_results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i + 10) >
                                              WiliResult.Fs1_F1 And _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i + 10) <> 0) Then
                        Exit For
                    End If
                Next

                If WiliResult.Fs1_F1 = WiliResult.Fs1_F11 Or WiliResult.Fs1_F11 > WiliResult.Fs1_F1 - 0.2 Then
                    Recalage = Recalage + 10
                    If Recalage < WiliResult.X_Ce1_index Then
                        GoTo LoopSnap1
                    End If
                End If
                'if the F1 is after CE1
                If WiliResult.Fs1_F1 = 0 And WiliResult.Fs1_F11 = 100 Then
                    RecalageCe = RecalageCe + 10
                    If RecalageCe < WiliResult.Xe_Index Then
                        GoTo LoopSnap1
                    End If
                End If
                ' calculate Snap CE1
                WiliResult.dFs1_Haptic_1 = WiliResult.Fs1_F11
                AddLogEntry("F1: " & WiliResult.Fs1_F1.ToString())
                AddLogEntry("F2: " & WiliResult.dFs1_Haptic_1.ToString())
                'If (WIliResult.dFs1_Haptic_1 = -1) And WIliResult.Fs1_F11 <> -1 Then
                '    WIliResult.dFs1_Haptic_1 = WIliResult.Fs1_F1 - WIliResult.Fs1_F11
                If (WiliResult.Fs1_F1 > 0) Then
                    WiliResult.dFs1_Haptic_1 = Format((WiliResult.dFs1_Haptic_1 / WiliResult.Fs1_F1) * 100, "#0.00")
                Else
                    WiliResult.dFs1_Haptic_1 = 0
                End If
                AddLogEntry("F2/F1: " & WiliResult.dFs1_Haptic_1.ToString())
                'End If
                WiliResult.dXs1 = Abs((_results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, WiliResult.Xs_F2_index) - WiliResult.Xs)) ' WIliResult.Xs1
                WiliResult.Value_DiffS2Ce1 = WiliResult.dXs1 - WiliResult.XCe1
                'Console.WriteLine("WiliResult.Value_DiffS2Ce1" + WiliResult.Value_DiffS2Ce1)

                Fe1 = WiliResult.Fs1_F11 + (1 * (WiliResult.Fs1_F1 - WiliResult.Fs1_F11))

                For i = WiliResult.Xs_F2_index To WiliResult.Xe_Index
                    If _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i) >= Fe1 AndAlso WiliResult.X_Fe1_index = -1 Then
                        WiliResult.X_Fe1_index = i
                    End If
                    If _results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, i) >= 2 * WiliResult.Fs1_F1 AndAlso WiliResult.FsCe1 = -1 Then
                        WiliResult.FsCe1 = i
                    End If
                Next
                If WiliResult.FsCe1 = -1 Then
                    WiliResult.FsCe1 = WiliResult.Xe_Index
                End If
            End If
            'Check Data available
            If WiliResult.Fs1_F1 < 0 Then WiliResult.Fs1_F1 = 0
            If WiliResult.dXs1 < 0 Then WiliResult.dXs1 = 0
            If WiliResult.Xs1 < 0 Then WiliResult.Xs1 = 0
            If WiliResult.dFs1_Haptic_1 < 0 Then WiliResult.dFs1_Haptic_1 = 0
            If WiliResult.Fe < 0 Then WiliResult.Fe = 0
            If WiliResult.Xe < 0 Then WiliResult.Xe = 0
            If WiliResult.Xs < 0 Then WiliResult.Xs = 0

        Catch ex As Exception
            Dim err As String
            err = ex.ToString

            _log.Append("Error in Process Sample : " & err)

            'Check Data available
            If WiliResult.Fs1_F1 < 0 Then WiliResult.Fs1_F1 = 0
            If WiliResult.dXs1 < 0 Then WiliResult.dXs1 = 0
            If WiliResult.Xs1 < 0 Then WiliResult.Xs1 = 0
            If WiliResult.dFs1_Haptic_1 < 0 Then WiliResult.dFs1_Haptic_1 = 0
            If WiliResult.Fe < 0 Then WiliResult.Fe = 0
            If WiliResult.Xs < 0 Then WiliResult.Xs = 0
            If WiliResult.Xe < 0 Then WiliResult.Xe = 0
        End Try

    End Sub



    Private Sub Split_Data(ByVal WiLi_Test As eWindows)
        ' analogInputIndex: 0 = Early Sensor
        '                 : 1 = Strengh Sensor
        '                 : 2 = Windows Lifter Rear Left UP
        '                 : 3 = Windows Lifter Rear Left DN
        '                 : 4 = Windows Lifter Rear Right UP
        '                 : 5 = Windows Lifter Rear Right DN
        '                 : 6 = Windows Lifter Rear Left UP
        '                 : 7 = Windows Lifter Rear Left DN
        '                 : 8 = Windows Lifter Rear Right UP
        '                 : 9 = Windows Lifter Rear Right DN

        Dim sampleIndex As Integer
        Dim n As Integer = 40
        Dim df(0 To _results.SampleCount_TMP - 1) As Single
        Dim SampleIndex_DN0 As Integer
        Dim SampleIndex_DN1 As Integer
        Dim SampleIndex_UP0 As Integer
        Dim SampleIndex_UP1 As Integer
        Dim i As Integer
        Dim WindowsLifter As cWS03Results.eWindowsLifterTest
        ' Offset for Early Sensor and Strenght Sensor to adapte Data measurement
        Dim OffsetEarlySensor As Integer = 1
        Dim OffsetStrenghtSensor As Integer = 1
        Dim FilterEarlySensor As Integer = 50
        Dim FilterForceSensor As Integer = 10
        Dim Limit_Derivate As Integer = 1.5

        If _recipe.TestEnable_EV_Option.Value Then 'Special needs... TODO
            If WiLi_Test = eWindows.FrontRight AndAlso _recipe.Front_Right_Pull_Number_State.Value = 2 Then
                Limit_Derivate = 13
            End If
            If WiLi_Test = eWindows.FrontLeft AndAlso _recipe.Front_Left_Pull_Number_State.Value = 2 Then
                Limit_Derivate = 13
            End If
        End If

        ' Filter the samples
        FilterSamples_Global(eSignal_Analog.EarlySensor, FilterEarlySensor)
        FilterSamples_Global(eSignal_Analog.StrenghtSensor, FilterForceSensor)

        If (_results.SampleCount_TMP > FilterEarlySensor) Then
            _results.SampleCount_TMP = _results.SampleCount_TMP - FilterEarlySensor
        Else
            _results.SampleCount_TMP = 0
        End If

        If (_results.SampleCount_TMP > 1) Then
            ReDim df(0 To _results.SampleCount_TMP - 1)
        Else
            Exit Sub
        End If

        SampleIndex_DN0 = -1
        SampleIndex_DN1 = -1
        SampleIndex_UP0 = -1
        SampleIndex_UP1 = -1

        ' Calculate the 1st derivative 
        For i = n / 2 To (_results.SampleCount_TMP - n / 2) - 1
            df(i) = (_results.Sample_TMP(mWS03Main.cSample_Signal.EarlySensor, i + n / 2) -
                     _results.Sample_TMP(mWS03Main.cSample_Signal.EarlySensor, i - n / 2)) * 20
        Next

        For sampleIndex = n / 2 To (_results.SampleCount_TMP - n / 2) - 1
            If (df(sampleIndex) <= -(Limit_Derivate - 0.2)) AndAlso sampleIndex > 200 Then '-(Limit_Derivate-0.2),added only before RD solve the conduction delay issue.
                SampleIndex_DN0 = 0
                SampleIndex_DN1 = sampleIndex ' - 100
                Exit For
            End If
        Next
        Dim foundstrokeMaximum As Boolean = False 'Pull End. indication. I/O delay issue, or robot too fast, DAQ get pull back signal. YAN.Qian 20200528.
        Dim tempStroke As Single = 999
        For sampleIndex = (_results.SampleCount_TMP - n / 2) - FilterEarlySensor To n / 2 Step -1
            If (Not foundstrokeMaximum AndAlso tempStroke > _results.Sample_TMP(mWS04Main.cSample_Signal.EarlySensor, sampleIndex)) Then
                tempStroke = _results.Sample_TMP(mWS04Main.cSample_Signal.EarlySensor, sampleIndex)
            Else
                foundstrokeMaximum = True 'Next point is small.
            End If
            If (foundstrokeMaximum) Then
                If (df(sampleIndex) <= 0) AndAlso SampleIndex_UP1 = -1 Then
                    SampleIndex_UP1 = sampleIndex
                End If
                If (df(sampleIndex) > Limit_Derivate) AndAlso SampleIndex_UP1 <> -1 AndAlso sampleIndex < SampleIndex_UP1 - 300 Then
                    SampleIndex_UP0 = sampleIndex
                    Exit For
                End If
            End If
        Next

        AddLogEntry("SampleIndex_DN0 : " & SampleIndex_DN0)
        AddLogEntry("SampleIndex_DN1 : " & SampleIndex_DN1)
        AddLogEntry("SampleIndex_UP0 : " & SampleIndex_UP0)
        AddLogEntry("SampleIndex_UP1 : " & SampleIndex_UP1)

        If (WiLi_Test = eWindows.FrontLeft And CBool(_recipe.TestEnable_Front_Left_PUSH_Electrical.Value)) Or
            (WiLi_Test = eWindows.FrontRight And CBool(_recipe.TestEnable_Front_Right_PUSH_Electrical.Value)) Then
            ' Affect the windows lifter index
            If WiLi_Test = eWindows.FrontLeft Then WindowsLifter = cWS03Results.eWindowsLifterTest.FrontLeft_Push
            If WiLi_Test = eWindows.FrontRight Then WindowsLifter = cWS03Results.eWindowsLifterTest.FrontRight_Push

            If SampleIndex_DN1 - SampleIndex_DN0 < 0 Then
                _results.WS03_SampleCount(WindowsLifter) = 0
            Else
                _results.WS03_SampleCount(WindowsLifter) = SampleIndex_DN1 - SampleIndex_DN0
            End If

            ' Store the samples
            For sampleIndex = 0 To _results.WS03_SampleCount(WindowsLifter)
                ' Early Sensor 
                _results.Samples_Push_Pull(WindowsLifter, mGlobal.eSignal_Analog.EarlySensor, sampleIndex) =
                    _results.Sample_TMP(mGlobal.eSignal_Analog.EarlySensor, sampleIndex + SampleIndex_DN0) *
                    OffsetEarlySensor
                ' Strenght Sensor
                _results.Samples_Push_Pull(WindowsLifter, mGlobal.eSignal_Analog.StrenghtSensor, sampleIndex) =
                    _results.Sample_TMP(mGlobal.eSignal_Analog.StrenghtSensor, sampleIndex + SampleIndex_DN0) *
                    OffsetStrenghtSensor
                For i = 0 To 4 ' add signal Jama
                    _results.Samples_Push_Pull(WindowsLifter, i + 2, sampleIndex) = _results.Sample_TMP(i + 2, sampleIndex + SampleIndex_DN0)
                Next i
            Next
        End If

        If (WiLi_Test = eWindows.FrontLeft And CBool(_recipe.TestEnable_Front_Left_PULL_Electrical.Value)) Or
            (WiLi_Test = eWindows.FrontRight And CBool(_recipe.TestEnable_Front_Right_PULL_Electrical.Value)) Then
            ' Affect the windows lifter index
            If WiLi_Test = eWindows.FrontLeft Then WindowsLifter = cWS03Results.eWindowsLifterTest.FrontLeft_Pull
            If WiLi_Test = eWindows.FrontRight Then WindowsLifter = cWS03Results.eWindowsLifterTest.FrontRight_Pull

            If SampleIndex_UP1 - SampleIndex_UP0 < 0 Then
                _results.WS03_SampleCount(WindowsLifter) = 0
            Else
                _results.WS03_SampleCount(WindowsLifter) = SampleIndex_UP1 - SampleIndex_UP0
            End If
            '_results.SampleCount(WindowsLifter, eUP_DN.UP) = SampleIndex_UP1 - SamplIndex_UP0
            ' Store the samples
            For sampleIndex = 0 To _results.WS03_SampleCount(WindowsLifter)
                ' Early Sensor
                _results.Samples_Push_Pull(WindowsLifter, mGlobal.eSignal_Analog.EarlySensor, sampleIndex) =
                        _results.Sample_TMP(mGlobal.eSignal_Analog.EarlySensor, sampleIndex + SampleIndex_UP0) *
                        OffsetEarlySensor
                ' Strenght Sensor
                _results.Samples_Push_Pull(WindowsLifter, mGlobal.eSignal_Analog.StrenghtSensor, sampleIndex) =
                    _results.Sample_TMP(mGlobal.eSignal_Analog.StrenghtSensor, sampleIndex + SampleIndex_UP0) *
                    OffsetStrenghtSensor
                For i = 0 To 4 ' add signal Jama
                    _results.Samples_Push_Pull(WindowsLifter, i + 2, sampleIndex) = _results.Sample_TMP(i + 2, sampleIndex + SampleIndex_UP0)
                Next i
            Next
        End If
    End Sub


    Private Sub FilterSamples_Global(ByVal analogInputIndex As Integer,
                                        ByVal filterSize As Integer)
        For sampleIndex = 0 To _results.SampleCount_TMP - filterSize - 1
            For i = 1 To filterSize - 1
                _results.Sample_TMP(analogInputIndex, sampleIndex) =
                    _results.Sample_TMP(analogInputIndex, sampleIndex) +
                    _results.Sample_TMP(analogInputIndex, sampleIndex + i)
            Next
            _results.Sample_TMP(analogInputIndex, sampleIndex) = Math.Round(_results.Sample_TMP(analogInputIndex, sampleIndex) / filterSize, 3)
        Next

    End Sub

    Private Function Write_AnalogPoint(ByVal WindowsLifterTest As mWS03Main.eWindows, ByVal index As Integer) As Boolean
        Dim fileWriter As StreamWriter = Nothing
        Dim sampleIndex As Integer
        Dim WriteLine As String
        Dim FileName As String = String.Empty
        Dim Push_Pull As ePush_Pull

        Dim folder As String = "..\..\..\Point File Haptic\WS03\" + Now.ToString("yyMMdd") + "\"
        If index <> -1 Then
            ' Save the Analog Data information
            If WindowsLifterTest = eWindows.FrontLeft Then
                FileName = folder & "FL_Push"
                Push_Pull = ePush_Pull.Push
            ElseIf WindowsLifterTest = eWindows.FrontRight Then
                FileName = folder & "FR_Push"
                Push_Pull = ePush_Pull.Push
            End If
            fileWriter = New StreamWriter(FileName & "_With Filter_" & _results.Valeo_Serial_Number.Value.ToString() & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".txt")

            ' Store the samples
            For sampleIndex = 0 To _results.WS03_SampleCount(WindowsLifterTest) - 1
                WriteLine = (_results.Samples_Push_Pull(WindowsLifterTest, 0, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest, 1, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex))
                fileWriter.WriteLine(WriteLine)
            Next

            fileWriter.Close()

            If WindowsLifterTest = eWindows.FrontLeft Then
                FileName = folder & "FL_Pull"
                Push_Pull = ePush_Pull.Pull
            ElseIf WindowsLifterTest = eWindows.FrontRight Then
                FileName = folder & "FR_Pull"
                Push_Pull = ePush_Pull.Pull
            End If
            fileWriter = New StreamWriter(FileName & "_With Filter_" & _results.Valeo_Serial_Number.Value.ToString() & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".txt")

            ' Store the samples
            For sampleIndex = 0 To _results.WS03_SampleCount(WindowsLifterTest + 1) - 1
                WriteLine = (_results.Samples_Push_Pull(WindowsLifterTest + 1, 0, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest + 1, 1, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest + 1, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest + 1, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest + 1, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest + 1, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex) & vbTab &
                            _results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex))
                fileWriter.WriteLine(WriteLine)
            Next
            fileWriter.Close()
        Else
            ' Save the Analog Data information
            _results.Sample_TMP_AllKnob(WindowsLifterTest) = _results.Sample_TMP.Clone()
            _results.Sample_TMP_AllKnobCount(WindowsLifterTest) = _results.SampleCount_TMP

            If WindowsLifterTest = eWindows.FrontLeft Then
                FileName = folder & FileName & "FL_Global"
            ElseIf WindowsLifterTest = eWindows.FrontRight Then
                FileName = folder & FileName & "FR_Global"
            End If
            FileName &= "_Without Filter_" & _results.Valeo_Serial_Number.Value.ToString()

            Dim para(3) As Object
            para(0) = _results.Sample_TMP_AllKnob(WindowsLifterTest)
            para(1) = _results.Sample_TMP_AllKnobCount(WindowsLifterTest)
            para(2) = FileName
            para(3) = folder
            Dim _backgroundworker_filesave As ComponentModel.BackgroundWorker = New ComponentModel.BackgroundWorker With {
                .WorkerSupportsCancellation = True,
                .WorkerReportsProgress = False
            }
            AddHandler _backgroundworker_filesave.DoWork, Sub(ByVal sender As Object, ByVal e As ComponentModel.DoWorkEventArgs)
                                                              Try
                                                                  If (System.IO.Directory.Exists(e.Argument(3).ToString()) = False) Then
                                                                      System.IO.Directory.CreateDirectory(e.Argument(3).ToString())
                                                                  End If
                                                                  fileWriter = New StreamWriter(e.Argument(2).ToString() & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".txt")
                                                                  Dim tempWriteLine As String = Convert.ToInt32(e.Argument(1))
                                                                  ' Store the samples
                                                                  For sampleIndex = 0 To Convert.ToInt32(e.Argument(1)) - 1
                                                                      tempWriteLine = (e.Argument(0)(mGlobal.eSignal_Analog.EarlySensor, sampleIndex) & vbTab &
                                                                                  e.Argument(0)(mGlobal.eSignal_Analog.StrenghtSensor, sampleIndex) & vbTab &
                                                                                  e.Argument(0)(cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex) & vbTab &
                                                                                  e.Argument(0)(cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex) & vbTab &
                                                                                  e.Argument(0)(cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex) & vbTab &
                                                                                  e.Argument(0)(cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex) & vbTab &
                                                                                  e.Argument(0)(cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex))
                                                                      fileWriter.WriteLine(tempWriteLine)
                                                                  Next
                                                                  fileWriter.Close()
                                                              Catch ex As Exception
                                                                  Console.WriteLine(ex.ToString)
                                                              End Try
                                                          End Sub
            _backgroundworker_filesave.RunWorkerAsync(para)
        End If
        ' Return Status
        Write_AnalogPoint = False
    End Function

    Private Function Write_AnalogPointPush(ByVal index As Integer) As Boolean
        Dim fileWriter As StreamWriter = Nothing
        Dim sampleIndex As Integer
        Dim WriteLine As String
        Dim FileName As String

        Dim folder As String = "..\..\..\Point File Haptic\WS03\" + Now.ToString("yyMMdd") + "\"
        FileName = folder + "CL"
        If index <> -1 Then
            ' Save the Analog Data information
            fileWriter = New StreamWriter(FileName & "_With Filter_" & _results.Valeo_Serial_Number.Value.ToString() & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".txt")

            ' Store the samples
            For sampleIndex = 0 To _results.WS03_SampleCountPush - 1
                WriteLine = (_results.WS03_SamplePush(0, sampleIndex) & vbTab &
                            _results.WS03_SamplePush(1, sampleIndex) & vbTab &
                            _results.WS03_SamplePush(2, sampleIndex))
                fileWriter.WriteLine(WriteLine)
            Next

            fileWriter.Close()
        Else
            _results.Sample_TMP_AllKnob(2) = _results.Sample_TMP.Clone()
            _results.Sample_TMP_AllKnobCount(2) = _results.SampleCount_TMP
            FileName &= "_Without Filter_" & _results.Valeo_Serial_Number.Value.ToString()

            Dim para(3) As Object
            para(0) = _results.Sample_TMP_AllKnob(2)
            para(1) = _results.Sample_TMP_AllKnobCount(2)
            para(2) = FileName
            para(3) = folder
            Dim _backgroundworker_filesave As ComponentModel.BackgroundWorker = New ComponentModel.BackgroundWorker With {
                .WorkerSupportsCancellation = True,
                .WorkerReportsProgress = False
            }
            AddHandler _backgroundworker_filesave.DoWork, Sub(ByVal sender As Object, ByVal e As ComponentModel.DoWorkEventArgs)
                                                              Try
                                                                  If (System.IO.Directory.Exists(e.Argument(3).ToString()) = False) Then
                                                                      System.IO.Directory.CreateDirectory(e.Argument(3).ToString())
                                                                  End If
                                                                  fileWriter = New StreamWriter(e.Argument(2).ToString() & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".txt")
                                                                  Dim tempWriteLine As String = Convert.ToInt32(e.Argument(1))
                                                                  ' Store the samples
                                                                  For sampleIndex = 0 To Convert.ToInt32(e.Argument(1)) - 1
                                                                      tempWriteLine = (e.Argument(0)(mGlobal.eSignal_Analog.EarlySensor, sampleIndex) & vbTab &
                                                                                  e.Argument(0)(mGlobal.eSignal_Analog.StrenghtSensor, sampleIndex) & vbTab &
                                                                                  e.Argument(0)(2, sampleIndex))
                                                                      fileWriter.WriteLine(tempWriteLine)
                                                                  Next
                                                                  fileWriter.Close()
                                                              Catch ex As Exception
                                                                  Console.WriteLine(ex.ToString)
                                                              End Try
                                                          End Sub
            _backgroundworker_filesave.RunWorkerAsync(para)
        End If
        ' Return Status
        Write_AnalogPointPush = False
    End Function

    Private Function Read_AnalogPoint(ByVal FileName As String, ByVal Push As Boolean, ByVal WindowsLifterTest As mWS03Main.eWindows) As Boolean
        Dim file As StreamReader = Nothing
        Dim sampleIndex As Integer
        Dim line As String
        Dim token(0 To 14) As String
        'Dim FileName As String
        Dim i As Integer
        file = New StreamReader(FileName)
        sampleIndex = 0
        ' While it is not reached the end of the file
        Do While Not (file.EndOfStream)
            ' Read a line
            line = file.ReadLine
            ' If the line is not empty and is not a comment
            If (line <> "" AndAlso Not (line.StartsWith("'"))) Then
                If line.Contains(",") And Separateur <> "," Then
                    line = Replace(line, ",", Separateur)
                ElseIf line.Contains(".") And Separateur <> "." Then
                    line = Replace(line, ".", Separateur)
                End If
                ' Split the line
                token = Split(line, vbTab)
                ' Load the value configuration
                _results.Sample_TMP(mGlobal.eSignal_Analog.EarlySensor, sampleIndex) = token(0)
                _results.Sample_TMP(mGlobal.eSignal_Analog.StrenghtSensor, sampleIndex) = token(1)
                If Not Push Then
                    _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex) = token(2)
                    _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex) = token(3)
                    _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex) = token(4)
                    _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex) = token(5)
                    _results.Sample_TMP(cWS03Results.eWindowsLifterSignal.Jama_Down + 2, sampleIndex) = token(6)
                Else
                    _results.Sample_TMP(2, sampleIndex) = token(2)
                End If
                sampleIndex = sampleIndex + 1
            End If

        Loop
        _results.SampleCount_TMP = sampleIndex
        _results.Sample_TMP_AllKnob(WindowsLifterTest) = _results.Sample_TMP.Clone()
        _results.Sample_TMP_AllKnobCount(WindowsLifterTest) = _results.SampleCount_TMP

        'Close the file
        If (file IsNot Nothing) Then
            file.Close()
            file = Nothing
        End If
        ' Return Status
        Read_AnalogPoint = False
    End Function

End Module















