Option Explicit On
'Option Strict On

Imports System
Imports System.IO
Imports Microsoft.VisualBasic
Imports NationalInstruments.DAQmx
Imports System.Math
Imports System.Threading


Module mWS05Main
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

    Public Enum eMirrorPush
        MirrorUP = 0
        MirrorDN = 1
        MirrorMR = 2
        MirrorML = 3
        MirrorSR = 4
        MirrorSL = 5

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
        Push_Mirror_UP = 14
        Push_Mirror_DN = 15
        Push_Mirror_Right = 16
        Push_Mirror_Left = 17
        Push_Mirror_SR = 18
        Push_Mirror_SL = 19
        FinalState = 20
        Write_MMS_TestByte = 21
        Read_MMS_TestByte = 22
        PowerDown = 23
        '
        Count = 24
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
        MirrorUP = 2
        MirrorDN = 3
        MirrorMR = 4
        MirrorML = 5
        MirrorSR = 6
        MirrorSL = 7
        '
        Count = 8
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
    Private Structure sMirrorResult
        Dim Xs_F1_index As Integer
        Dim Xs_F2_index As Integer
        Dim Xs_F3_index As Integer
        Dim Xs_F4_index As Integer
        Dim X_Fe1_index As Integer
        Dim X_Fe2_index As Integer
        Dim X_Ce1_index As Integer
        Dim X_Ce2_index As Integer
        Dim Xe_Index As Integer
        Dim Xs_Index As Integer

        Dim Fs1_F1 As Single
        Dim Xs1 As Single
        Dim Fs1_F11 As Single
        Dim Xs12 As Single
        Dim Value_DiffS2Ce1 As Single
        Dim dFs1_Haptic_1 As Single
        Dim dXs1 As Single
        Dim FsCe1 As Single
        Dim FeCe1 As Single
        Dim XCe1 As Single
        Dim Fs2_F2 As Single
        Dim Xs2 As Single
        Dim Fs2_F21 As Single
        Dim Xs21 As Single
        Dim dFs2_Haptic_2 As Single
        Dim dXs2 As Single
        Dim FsCe2 As Single
        Dim FeCe2 As Single
        Dim XCe2 As Single
        Dim Fe As Single
        Dim Xe As Single
        Dim Xs As Single

        Dim XvCe1 As Single
        Dim XvCe2 As Single


        Dim dFs3_Haptic_3 As Single
    End Structure

    ' Input enumeration
    Public Structure eInput_WS05
        Public PartTypeNumber As Integer
        Public Test_Mode As Integer
        Public PartModel As Integer
        Public Reference As String
        Public UniqueNumber As String

    End Structure

    ' Outputs enumeration
    Public Structure eOutput_WS05
        Public ResultCode As Double
        Public Reference As String
        Public UniqueNumber As String

    End Structure

    Public txData_MasterReq(0 To 7) As Byte
    Public btxRequest_MasterReq As Boolean

    Public RunningTimer As New cRunningTime("WS05")

    Public PushState(0 To 19) As Boolean


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
    Private Const mintTesteurPeriodms = 1800
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
    Private _recipe As cWS05Recipe
    Private _results As cWS05Results
    Private _resultsPT As cWS05Results
    Private _recipeMaster As cWS05Recipe

    Private _PhaseSensor As Integer

    Private _Ethernet_Input As eInput_WS05
    Private _Ethernet_Output As eOutput_WS05

    Private Param_1 As Integer
    Private Param_2 As Integer

    Private HbmReset As Boolean

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
            If (_phase = ePhase.WaitRecipeSelection Or _
                TestMode = eTestMode.Debug) Then
                _reference = value
            End If
        End Set
    End Property


    Public ReadOnly Property Results As cWS05Results
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
            StepInProgressInputStatus = mDIOManager.DigitalInputStatus(eDigitalInput.WS05_StepInProgress)
        End Get
    End Property

    Public ReadOnly Property TestEnableInputStatus As Boolean
        Get
            TestEnableInputStatus = mDIOManager.DigitalInputStatus(eDigitalInput.WS05_TestEnable)
        End Get
    End Property

    Public ReadOnly Property TestOkOutputStatus As Boolean
        Get
            TestOkOutputStatus = mDIOManager.DigitalOutputStatus(mDIOManager.eDigitalOutput.WS05_TestOk)
        End Get
    End Property


    Public ReadOnly Property StartStepOutputStatus As Boolean
        Get
            StartStepOutputStatus = mDIOManager.DigitalOutputStatus(mDIOManager.eDigitalOutput.WS05_StartStep)
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

    Public Sub ManageSubPhaseMaint(ByVal _SubPhaseName As Integer, _
                               ByVal _Step As Integer, _
                               ByVal _Param_1 As Integer, _
                               ByVal _Param_2 As Integer)

        _subPhaseMaint(_SubPhaseName) = _Step

        Param_1 = _Param_1
        Param_2 = _Param_2

    End Sub


    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+

    Public Sub StartPLC()
        mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.Test_Enable_PLC) = True
        mWS05Ethernet.WriteOutputDataBlock()
    End Sub


    Public Sub AbortTest()
        ' Set the abort test flag
        _abort = (_phase <> ePhase.WaitRecipeSelection And _
                  _phase <> ePhase.LoadRecipe And _
                  _phase <> ePhase.WaitStartTest And _
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
        mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.ResultCode) = 100000
        mWS05Ethernet.WriteOutputDataBlock()
    End Sub

    Public Sub SetResultMaint()
        mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.ResultCode) = 100010
        mWS05Ethernet.WriteOutputDataBlock()
    End Sub

    Public Sub ClearAlarms()
        ' Clear all the alarms
        For i = 0 To eAlarm.Count - 1
            _alarm(i) = False
        Next i
        '
        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_Alarm)

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
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-down Work Station 05  digital I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS05DIOManager.PowerDown
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-down of the Work Station 05  digital I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = e

        ' Power-down the analog I/O manager
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-down the Work Station 05  analog I/O manager... ")
            If _HardwareEnabled_NI Then
                e = mWS04_05AIOManager.PowerDown
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-down of the Work Station 05  analog I/O manager: retry?"
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
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load Work Station 05 settings... ")
        Do
            e = LoadSettings(mSettings.WS05SettingsPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work Station 05 settings: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = e

        ' Power-up the analog I/O manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-up the Work Station 05 analog I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS05AIOManager.PowerUp
                'Start Commun analog board
                e = e Or mWS04_05AIOManager.PowerUp()
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-up of the Work Station 05 analog I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Power-up the digital I/O manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-up Work Station 05 digital I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS05DIOManager.PowerUp
            Else
                e = False
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-up of the Work Station 05  digital I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the LIN frames
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load Work Station 05 the LIN frames  ... ")
        Do
            e = CLINFrame.LoadArrayFromFile(_settings.LINFramesPath, _LINFrame)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading Work Station 05 the LIN frames : retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e


        ' Load the recipe configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work Station 05  recipe FHM configuration... ")
        _recipe = New cWS05Recipe
        Do
            e = _recipe.LoadConfiguration(_settings.RecipeConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work Station 05  recipe FHM configuration: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e


        ' Load the results configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work Station 05 results FHM configuration... ")
        _results = New cWS05Results
        _resultsPT = New cWS05Results
        Do
            e = _results.LoadConfiguration(_settings.ResultsConfigurationPath) 'Or _
            '_resultsPT.LoadConfiguration(_settings.ResultsConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work Station 05  results FHM configuration: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the PLC configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work Station 05  PLC configuration... ")
        Do
            e = mWS05Ethernet.PowerUp(_settings.InputDBConfigurationPath, _settings.OutputDBConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work Station 05  PLC configuration: retry?"
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
            'Maint_PhaseRead(Param_1, Param_2)
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
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Diag_1060), _
                                                    True, _
                                                    txData_MasterReq, _
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                    True, _
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
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Diag_1060), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 3
                ' Read Key
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ReadKey), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
                                                        True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhaseMaint(0) = 4

            Case 4
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ReadKey))
                If (i <> -1) Then
                    KeyAccess(0) = _LinInterface.RxFrame(i).Data(4) & _
                                    _LinInterface.RxFrame(i).Data(5) & _
                                    _LinInterface.RxFrame(i).Data(6) & _
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
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ReadKey), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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
                e = e Or _LinInterface.Transmit(f, _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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
                    e = e Or _LinInterface.Transmit(f, _
                                                            True, _
                                                            txData_MasterReq, _
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                            True, _
                                                            True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 7
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070), _
                                                    True, _
                                                    txData_MasterReq, _
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                    True, _
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
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 10
                If ((Date.Now - t0).TotalMilliseconds > 3000) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TesterPresent), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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

    ' end Maintenance Module


    Public Sub TestLoop()
        Dim alarmFlag As Boolean
        Dim e As Boolean
        Dim f As Boolean
        Static t0ScanTime As Date
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
        If (_phase <> ePhase.WaitStartTest And _
            _phase <> ePhase.WaitRecipeSelection And _
            _phase <> ePhase.LoadRecipe) Then
            _testLast = (Date.Now - _t0Test).TotalSeconds
        End If

        ' Manage the abort test

        f = (_testMode = eTestMode.Debug And _abort = True)
        f = f Or (_testMode = eTestMode.Local Or _testMode = eTestMode.Remote) And _
            mDIOManager.DigitalInputStatus(eDigitalInput.WS05_TestEnable) = False
        f = f And Not (_phase = ePhase.WaitRecipeSelection Or _
                       _phase = ePhase.LoadRecipe Or _
                       _phase = ePhase.WaitStartTest Or _
                       (_phase = ePhase.WriteResults And _subPhase(_phase) = 3) Or _
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
            Case ePhase.Push_Mirror_UP
                PhaseTest(eMirrorPush.MirrorUP)
            Case ePhase.Push_Mirror_DN
                PhaseTest(eMirrorPush.MirrorDN)
            Case ePhase.Push_Mirror_Right
                PhaseTest(eMirrorPush.MirrorMR)
            Case ePhase.Push_Mirror_Left
                PhaseTest(eMirrorPush.MirrorML)
            Case ePhase.Push_Mirror_SR
                PhaseTest(eMirrorPush.MirrorSR)
            Case ePhase.Push_Mirror_SL
                PhaseTest(eMirrorPush.MirrorSL)
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
        If (_phase <> ePhase.WaitRecipeSelection And _
            _phase <> ePhase.LoadRecipe And _
            _phase <> ePhase.WaitStartTest And _
            _phase <> ePhase.ClearResults And _
            _phase <> ePhase.WriteResults And _
            _scanTime > _maxScanTime) Then
            _maxScanTime = _scanTime
            If _maxScanTime > 50 Then Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + "WS05:_maxScanTime=" + _maxScanTime.ToString() + "Phase:" + _phase.ToString() + "SubPhase:" + _subPhase(_phase).ToString())
            If _maxScanTime > 100 Then AddLogEntry(DateTime.Now.ToString("HH:mm:ss fff") + "WS04:_maxScanTime=" + _maxScanTime.ToString() + "Phase:" + _phase.ToString() + "SubPhase:" + _subPhase(_phase).ToString())
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
        ElseIf (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_LoadRecipeLocal) = False) Then   ' Otherwise, if the PLC load recipe local mode is cleared
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
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RecipeLoaded)
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RelaodRecipe)
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_Alarm)
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
                    mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.Reference) = ""
                    ' Write the output data block
                    mWS05Ethernet.WriteOutputDataBlock()

                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                ElseIf (cWS05Recipe.Exists(_reference)) Then
                    ' If the recipe loading fails
                    If (_recipe.Load(_reference)) Then
                        ' Add a log entry
                        AddLogEntry(String.Format("Error while loading the recipe ""{0}""", _reference))
                        ' Raise an alarm for error loading the recipe
                        _alarm(eAlarm.ErrorLoadingRecipe) = True
                        mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.Reference) = ""
                        ' Write the output data block
                        mWS05Ethernet.WriteOutputDataBlock()
                        ' Go to the phase WaitStartTest
                        _phase = ePhase.WaitStartTest
                    Else    ' Otherwise, if the recipe loading succeeds
                        ' Add a log entry
                        AddLogEntry(String.Format("Recipe ""{0}"" loading succeeded", _reference))
                        If (_HardwareEnabled_PLC) Then
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.Test_Enable_Mirror) = _recipe.TestEnable_Mirror_Electrical.Value

                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorUP_Correct_X) = _recipe.X_correction_approachment_Push(eMirrorPush.MirrorUP).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorUP_Correct_Y) = _recipe.Y_correction_approachment_Push(eMirrorPush.MirrorUP).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorUP_Correct_Z) = _recipe.Z_correction_approachment_Push(eMirrorPush.MirrorUP).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorUP_Z_TOUCH) = _recipe.Z_Vector_Final_Position_Push(eMirrorPush.MirrorUP).Value

                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorDN_Correct_X) = _recipe.X_correction_approachment_Push(eMirrorPush.MirrorDN).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorDN_Correct_Y) = _recipe.Y_correction_approachment_Push(eMirrorPush.MirrorDN).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorDN_Correct_Z) = _recipe.Z_correction_approachment_Push(eMirrorPush.MirrorDN).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorDN_Z_TOUCH) = _recipe.Z_Vector_Final_Position_Push(eMirrorPush.MirrorDN).Value

                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorMR_Correct_X) = _recipe.X_correction_approachment_Push(eMirrorPush.MirrorMR).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorMR_Correct_Y) = _recipe.Y_correction_approachment_Push(eMirrorPush.MirrorMR).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorMR_Correct_Z) = _recipe.Z_correction_approachment_Push(eMirrorPush.MirrorMR).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorMR_Z_TOUCH) = _recipe.Z_Vector_Final_Position_Push(eMirrorPush.MirrorMR).Value

                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorML_Correct_X) = _recipe.X_correction_approachment_Push(eMirrorPush.MirrorML).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorML_Correct_Y) = _recipe.Y_correction_approachment_Push(eMirrorPush.MirrorML).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorML_Correct_Z) = _recipe.Z_correction_approachment_Push(eMirrorPush.MirrorML).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorML_Z_TOUCH) = _recipe.Z_Vector_Final_Position_Push(eMirrorPush.MirrorML).Value

                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorSR_Correct_X) = _recipe.X_correction_approachment_Push(eMirrorPush.MirrorSR).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorSR_Correct_Y) = _recipe.Y_correction_approachment_Push(eMirrorPush.MirrorSR).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorSR_Correct_Z) = _recipe.Z_correction_approachment_Push(eMirrorPush.MirrorSR).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorSR_Z_TOUCH) = _recipe.Z_Vector_Final_Position_Push(eMirrorPush.MirrorSR).Value

                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorSL_Correct_X) = _recipe.X_correction_approachment_Push(eMirrorPush.MirrorSL).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorSL_Correct_Y) = _recipe.Y_correction_approachment_Push(eMirrorPush.MirrorSL).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorSL_Correct_Z) = _recipe.Z_correction_approachment_Push(eMirrorPush.MirrorSL).Value
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.MirrorSL_Z_TOUCH) = _recipe.Z_Vector_Final_Position_Push(eMirrorPush.MirrorSL).Value

                            ' Write the recipe data to the PLC
                            mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.Reference) = _reference
                        End If
                        ' Set the load ok flag
                        loadOk = True
                    End If
                Else    ' Otherwise, if the recipe does not exists
                    ' Add a log entry
                    AddLogEntry("Error: recipe unknown")
                    ' Raise an alarm for recipe not valid
                    _alarm(eAlarm.RecipeUnknown) = True
                    mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.Reference) = ""
                    ' Write the output data block
                    mWS05Ethernet.WriteOutputDataBlock()

                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                End If
                ' If the recipe loading succeeded
                If (loadOk) Then
                    If (_HardwareEnabled_PLC) Then
                        ' Write the output data block
                        e = e Or mWS05Ethernet.WriteOutputDataBlock
                        ' Set the recipe loaded bit
                        mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RecipeLoaded)
                        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RelaodRecipe)
                        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_Alarm)
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
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RecipeLoaded)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RelaodRecipe)
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_Alarm)
                    ' Add a log entry
                    AddLogEntry("Error: recipe Load with error")
                    ' Raise an alarm for recipe not valid
                    _alarm(eAlarm.RecipeNotValid) = True
                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest

                End If

            Case 1
                ' If the load recipe bit was cleared by the PLC
                If (_testMode = eTestMode.Debug Or _
                    (_testMode = eTestMode.Local And mDIOManager.DigitalInputStatus(eDigitalInput.WS05_LoadRecipeLocal) = False) Or _
                    (_testMode = eTestMode.Remote And mDIOManager.DigitalInputStatus(eDigitalInput.WS05_LoadRecipeRemote) = False)) Then
                    ' Add a log entry
                    AddLogEntry("End load recipe - Phase last " & (Date.Now - t0Phase).TotalSeconds.ToString("0.00"))
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RecipeLoaded)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RelaodRecipe)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_Alarm)
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
                    If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_LoadRecipeRemote)) Then
                        'Read Ethernet
                        mWS05Ethernet.ReadInputDataBlock()
                        ' Set the test mode to remote
                        _testMode = eTestMode.Remote
                        ' Set the reference and the reference type
                        _reference = mWS05Ethernet.InputValue(mWS05Ethernet.eInput.Reference)
                        ' Go to the phase LoadRecipe
                        _phase = ePhase.LoadRecipe
                        Exit Sub
                        ' Otherwise, if the PLC load recipe local mode is set
                    ElseIf (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_LoadRecipeLocal)) Then
                        ' Set the test mode to local
                        _testMode = eTestMode.Local
                        ' Clear the reference code
                        _reference = ""
                        ' Go to the phase WaitRecipeSelection
                        _phase = ePhase.WaitRecipeSelection
                        Exit Sub
                    End If
                    ' If one of the test enables is set or the start flag is set
                    If (_phase = ePhase.WaitStartTest And _reference <> "" And _
                        (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_TestEnable) Or _start)) Then
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
                mWS05Ethernet.ReadInputDataBlock()
                '
                HbmReset = False
                ' Set the force sensors reset signals
                mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_ResetForce)
                ' Read the input data block from the PLC
                If (_HardwareEnabled_PLC) Then
                    '
                    mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.ResultCode) = cWS05Results.eTestResult.Unknown
                    '
                    mWS05Ethernet.WriteOutputDataBlock()
                End If
                ' Clear the results
                ClearResults()
                ' Clear the OK digital outputs
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_TestOk)
                ' Go to the subphase 1
                _subPhase(_phase) = 1

            Case 1
                ' If the step in progress is false or the test mode is debug
                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = False And _
                   (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
                    _step = False
                    ' Sets the start step output
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
                    ' Go to the subphase 2
                    _subPhase(_phase) = 2
                End If

            Case 2
                ' If the step in progress is set or the test mode is debug
                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = True And _
                   (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
                    ' Clear the start step output
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
                    ' Go to the subphase 3
                    _subPhase(_phase) = 3
                End If

            Case 3
                ' If the step in progress is cleared or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = False Or _testMode = eTestMode.Debug) Then
                    AddLogEntry(String.Format("End clear results - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                    '
                    _subPhase(_phase) = 0
                    '
                    If (_testMode = eTestMode.Debug AndAlso Simulation_Test) Then
                        _phase = ePhase.Push_Mirror_UP
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
                        If ((_results.TestResult = cWS05Results.eTestResult.Unknown) Or _forcePartOk) And Not Wili_TMP Then
                            ' Check the master reference
                            If CheckMasterReference() = True Then
                                _results.TestResult = cWS05Results.eTestResult.FailedMasterReference
                                For i = 0 To cWS05Results.SingleTestCount - 1
                                    If mWS05Main.Results.Value(cWS05Results.SingleTestBaseIndex + i).TestResult <> cResultValue.eValueTestResult.Disabled Then
                                        mWS05Main.Results.Value(cWS05Results.SingleTestBaseIndex + i).TestResult = cResultValue.eValueTestResult.NotCoherent
                                    End If
                                Next
                            End If
                        End If
                    Else
                        _results.TestResult = cWS05Results.eTestResult.Passed
                    End If
                ElseIf (_results.PartTypeNumber.Value > 20) And Wili_TMP Then
                    '_results.TestResult = cWS05Results.eTestResult.Passed
                End If
                ' If the global test result is unknown or passed or forced ok
                If (_results.TestResult = cWS05Results.eTestResult.Unknown Or _
                    _results.TestResult = cWS05Results.eTestResult.Passed Or _
                    (_results.TestResult <> cWS05Results.eTestResult.NotTested And _
                     (_forcePartOk And _results.PartTypeNumber.Value <> 20))) Then
                    ' Set the global test result to passed
                    _results.TestResult = cWS05Results.eTestResult.Passed
                    ' Set the OK output
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_TestOk)
                    ' Increase the counter of passed parts
                    _counterPassed = _counterPassed + 1
                Else
                    ' Increase the counter of failed parts
                    _counterFailed = _counterFailed + 1
                End If
                If (_HardwareEnabled_PLC) Then
                    ' Write the result code to the PLC
                    mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.ResultCode) = _results.TestResult
                    '
                    mWS05Ethernet.WriteOutputDataBlock()
                End If
                mGlobal.FormDUTStats.Addnewitem(_results.PartUniqueNumber.Value, _results.TestTime.Value, 5, _results.TestResult, _results.RecipeName.Value)
                ' Save the results
                filename = mConstants.BasePath &
                            _settings.ResultsPath &
                            "\" & Format(Date.Now, "yyyyMMdd") &
                            "\WS05" & _results.RecipeName.Value
                filename = filename & "_" & _
                           Mid(_results.TestDate.Value, 7, 4) & _
                           Mid(_results.TestDate.Value, 4, 2) & _
                           Mid(_results.TestDate.Value, 1, 2)
                filename = filename & "_" & _
                                           Mid(_results.TestTime.Value, 1, 2) & _
                                           Mid(_results.TestTime.Value, 4, 2) & _
                                           Mid(_results.TestTime.Value, 7, 2)
                filename = filename & "_" & _
                                           _results.PartUniqueNumber.Value
                filename = filename & "_" & _
                                          Format(_results.TestResults.Value, "0000")
                filename = filename & _
                ".txt"
                e = e Or _results.Save(filename)
                ' Append the log to the results
                My.Computer.FileSystem.WriteAllText(filename, _
                                                    vbCrLf & _
                                                    "---------------------------------------------------------------------------------------------------------" & _
                                                    vbCrLf & _
                                                    "-                                  Log Frame LIN                                                         -" & _
                                                    vbCrLf & _
                                                    "----------------------------------------------------------------------------------------------------------" & _
                                                    vbCrLf & _
                                                    _log.ToString, _
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
                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = False And _
                    (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
                    _step = False
                    ' Set the start step output
                    e = mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
                    ' Go to subphase 2
                    _subPhase(_phase) = 2
                End If

            Case 2
                ' If the step in progress is set or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = True Or _testMode = eTestMode.Debug) Then
                    ' Reset the start step output
                    e = mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)

                    ' Go to subphase 3
                    _subPhase(_phase) = 3
                End If

            Case 3
                ' If the step in progress is cleared or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = False Or _testMode = eTestMode.Debug) Then
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
                mWS05Ethernet.OutputValue(mWS05Ethernet.eOutput.Test_Enable_PLC) = False
                mWS05Ethernet.WriteOutputDataBlock()


            Case 1
                ' If the test enable inputs are cleared
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_TestEnable) = False) Then
                    ' Add a log entry
                    AddLogEntry("End wait end test - Phase last: " & (Date.Now - t0Phase).TotalSeconds.ToString("0.00") & " s" & vbCrLf)
                    ' Go to phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                    Console.WriteLine("WS05 Finished Test!")
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
                For j = 0 To mWS05DIOManager.eDigitalOutput.Count - 1
                    e = e Or mWS05DIOManager.ResetDigitalOutput(j)
                Next
                e = mWS05DIOManager.SetDigitalOutput(mWS05DIOManager.eDigitalOutput.DO_LocalSensing)
                e = e Or mWS05DIOManager.ResetDigitalOutput(mWS05DIOManager.eDigitalOutput.DO_RemoteSensing)
                ' Reset all the digital outputs toward the PLC
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_TestOk)
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
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
                            "\WS05" & _results.RecipeName.Value
                filename = filename & "_" & _
                           Mid(_results.TestDate.Value, 7, 4) & _
                           Mid(_results.TestDate.Value, 4, 2) & _
                           Mid(_results.TestDate.Value, 1, 2)
                filename = filename & "_" & _
                                           Mid(_results.TestTime.Value, 1, 2) & _
                                           Mid(_results.TestTime.Value, 4, 2) & _
                                           Mid(_results.TestTime.Value, 7, 2)
                filename = filename & "_" & _
                                           _results.PartUniqueNumber.Value
                filename = filename & "_" & _
                                          Format(_results.TestResults.Value, "0000")
                filename = filename & _
                ".txt"
                e = e Or _results.Save(filename)
                ' Append the log to the results
                My.Computer.FileSystem.WriteAllText(filename, _
                                                    vbCrLf & _
                                                    "---------------------------------------------------------------------------------------------------------" & _
                                                    vbCrLf & _
                                                    "-                                  Log Frame  LIN                                                   -" & _
                                                    vbCrLf & _
                                                    "----------------------------------------------------------------------------------------------------------" & _
                                                    vbCrLf & _
                                                    _log.ToString, _
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
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TesterPresent), _
                                                    True, _
                                                    txData_MasterReq, _
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                    True, _
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
        Dim sample(0 To mWS05AIOManager.eAnalogInput.Count - 1) As Double
        Dim SampleCount As Integer
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
                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = False And _
                   (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
                    _step = False
                    '' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin power-up")
                    ' Connect the Lin Bus
                    e = e Or mWS05DIOManager.SetDigitalOutput(mWS05DIOManager.eDigitalOutput.DO_Lin_Bus)
                    ' Connect the power supply
                    e = e Or mWS05DIOManager.SetDigitalOutput(mWS05DIOManager.eDigitalOutput.DO_OnPowerSupply)
                    ' Connect the remote sensing
                    e = e Or mWS05DIOManager.SetDigitalOutput(mWS05DIOManager.eDigitalOutput.DO_RemoteSensing)
                    ' Disconnect the local sensing
                    e = e Or mWS05DIOManager.ResetDigitalOutput(mWS05DIOManager.eDigitalOutput.DO_LocalSensing)
                    '
                    If Not mWS04_05AIOManager.Sample_Task_Started Then
                        mWS04_05AIOManager.StartSampling(2000)
                    End If
                    '
                    mWS04_05AIOManager.EmptyBuffer_WS05()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 1
                End If

            Case 1
                ' Sample the analog inputs
                SampleCount = mWS04_05AIOManager.SampleCount_WS05 - 1
                If SampleCount > 100 Then
                    SampleCount = 100
                    For i = 0 To mWS05AIOManager.eAnalogInput.Count - 1
                        sample(i) = 0
                        For j = 0 To SampleCount - 1
                            sample(i) = sample(i) + mWS04_05AIOManager.SampleBuffer_WS05(i, j)
                        Next
                        sample(i) = sample(i) / SampleCount
                    Next            '
                    mWS04_05AIOManager.EmptyBuffer_WS05()
                End If

                ' If the voltage is OK or the test delay 1 expired
                If ((sample(mWS05AIOManager.eAnalogInput.WS05_VBAT) >= _recipe.StdSign_Powersupply(0).MinimumLimit And _
                    sample(mWS05AIOManager.eAnalogInput.WS05_VBAT) <= _recipe.StdSign_Powersupply(0).MaximumLimit) And _
                        (Date.Now - t0).TotalMilliseconds >= PowerUpDelay_ms) Then
                    _results.Power_supply_voltage.Value = sample(mWS05AIOManager.eAnalogInput.WS05_VBAT)
                    _results.Power_supply_Normal_Current.Value = sample(mWS05AIOManager.eAnalogInput.WS05_IBAT) * 1000
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 100
                    ''Tempory David
                    'mWS04_05AIOManager.StopSampling()

                ElseIf ((Date.Now - t0).TotalMilliseconds > 500) Then
                    ' Go to subphase
                    _subPhase(_phase) = 100
                End If

            Case 100
                ' Tests
                If (_results.Power_supply_voltage.Test <> cResultValue.eValueTestResult.Passed Or _
                    _results.Power_supply_Normal_Current.Test <> cResultValue.eValueTestResult.Passed) And _
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
                If (_results.TestResult = cWS05Results.eTestResult.Unknown And _
                    _results.ePOWER_UP.TestResult <> _
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS05Results.eTestResult.FailedPowerUP
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Init_Communication

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on Analog")
                ' Update the test result
                _results.ePOWER_UP.TestResult = _
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
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
                    _results.eINIT_ROOF_LIN_COMMUNICATION.TestResult = _
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                    _results.eINIT_ROOF_LIN_COMMUNICATION.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS05Results.eTestResult.FailedInitCommunication
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
                _results.eINIT_ROOF_LIN_COMMUNICATION.TestResult = _
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
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
                msngT0TesteurPeriodicFrame = Date.Now
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
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Diag_1060), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
                                                        True)
                    ' Store the time
                    tLin = Date.Now
                End If


            Case 3
                ' Read Key
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ReadKey), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
                                                        True)
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1

            Case 4
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ReadKey))
                If (i <> -1) Then
                    KeyAccess(0) = _LinInterface.RxFrame(i).Data(4) & _
                                    _LinInterface.RxFrame(i).Data(5) & _
                                    _LinInterface.RxFrame(i).Data(6) & _
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
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ReadKey), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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
                e = e Or _LinInterface.Transmit(f, _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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
                    e = e Or _LinInterface.Transmit(f, _
                                                            True, _
                                                            txData_MasterReq, _
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                            True, _
                                                            True)
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 7
                ' Open Diag 1070
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070), _
                                                    True, _
                                                    txData_MasterReq, _
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                    True, _
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
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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
                    _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult = _
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                    _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS05Results.eTestResult.FailedOpenDIAGonLINSession
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
                _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult = _
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
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
                    s = _LinInterface.RxFrame(i).Data(6) & _
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
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_Tracea), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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
                        s = s & _LinInterface.RxFrame(i).Data(2) & _
                                _LinInterface.RxFrame(i).Data(3) & _
                                _LinInterface.RxFrame(i).Data(4) & _
                                _LinInterface.RxFrame(i).Data(5) & _
                                _LinInterface.RxFrame(i).Data(6) & _
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
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_TestByte), _
                                                    True, _
                                                    txData_MasterReq, _
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                    True, _
                                                    True)
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
                    If _LinInterface.RxFrame(i).Data(5) = "41" Or _LinInterface.RxFrame(i).Data(5) = "47" Then
                        'WS04 OK.
                        _results.MMS_Test_Byte_Before.MinimumLimit = _LinInterface.RxFrame(i).Data(5)
                        _results.MMS_Test_Byte_Before.MaximumLimit = _LinInterface.RxFrame(i).Data(5)
                    ElseIf _LinInterface.RxFrame(i).Data(5) = "51" Or _LinInterface.RxFrame(i).Data(5) = "52" Or _LinInterface.RxFrame(i).Data(5) = "57" Then
                        'WS05 Retest.
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
                    _subPhase(_phase) = 20
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - tLin).TotalMilliseconds > LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_TestByte), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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
                    _subPhase(_phase) = 20

                End If

            Case 20
                AddLogEntry("Request Read SW Version")

                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_SW_Version),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 0
                ' Store the time
                t0 = Date.Now
                '
                tLin = Date.Now

            Case 21
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_SW_Version))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(5) & _
                            _LinInterface.RxFrame(i).Data(6) & _
                            _LinInterface.RxFrame(i).Data(7)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' init index Frame
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_SW_Version), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
                                                        True)
                    tLin = Date.Now
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
                    ' Go to subphase
                    _subPhase(_phase) = 100
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
                        s = s & _LinInterface.RxFrame(i).Data(2) & _
                                        _LinInterface.RxFrame(i).Data(3) & _
                                        _LinInterface.RxFrame(i).Data(4) & _
                                        _LinInterface.RxFrame(i).Data(5) & _
                                        _LinInterface.RxFrame(i).Data(6) & _
                                        _LinInterface.RxFrame(i).Data(7)
                        ' Delete the frame
                        _LinInterface.DeleteRxFrame(i)
                        ' Increase the frame index or go to subphase 100
                        If (frameIndex < 21) Then
                            frameIndex = frameIndex + 1
                            t0 = Date.Now
                        Else
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()

                            _subPhase(_phase) = 100
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        ' Set the flag of CAN timeout
                        _subPhase(_phase) = 100
                    End If
                Loop Until (i = -1)

            Case 100
                ' Tests
                If (_results.Valeo_Serial_Number.Test <> cResultValue.eValueTestResult.Passed Or _
                    _results.MMS_Test_Byte_Before.Test <> cResultValue.eValueTestResult.Passed) Then
                    _results.eMMS_Traceability.TestResult = _
                        cResultValue.eValueTestResult.Failed
                Else
                    _results.eMMS_Traceability.TestResult = _
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS05Results.eTestResult.Unknown And _
                    _results.eMMS_Traceability.TestResult <> _
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS05Results.eTestResult.FailedCheckMMSTestByte
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Push_Mirror_UP

            Case 199
                ' Lin                     
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eMMS_Traceability.TestResult = _
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If

    End Sub


    Private Sub PhaseTest(ByVal Mirror_Step As eMirrorPush)
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Dim LinIndex As Integer
        Dim MirrorResult As sMirrorResult
        Dim failedOff As Boolean
        Dim failedOnCe1 As Boolean
        ' Offset for Stroke Sensor and Strenght Sensor to adapte Data measurement
        Dim OffsetStrokeSensor As Integer = 1
        Dim OffsetStrenghtSensor As Integer = 1

        Dim timeAfterCommutation As Integer = 10    ' f=2kHz time is 25 ms
        Dim timeBeforeCommutation As Integer = 10   ' f=2kHz time is 5 ms
        Dim MinLevel As String
        Dim MaxLevel As String

        Dim f As CLINFrame
        Static NB_Transition As Integer
        Static SwitchTiming(0 To 20) As Single
        Static frameIndex As Integer
        Static t0 As Date
        Static tLin As Date
        Static t0Phase As Date
        Static s As String
        Static StatusPushLin(0 To 20) As String
        Static frameExt As Boolean

        'Static TimeStart As Date
        Static OffSetTime As Double
        Static OffSetTimeForAIShareCards As Date
        Static t0Lin As Date
        Static tAnalog As Date
        Static tAcqLin As String
        Static SampleCount As Integer
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                If (CBool(_recipe.TestEnable_Mirror_Electrical.Value)) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    'Debug.Print("Begin Push " + Mirror_Step.ToString())
                    If Mirror_Step = eMirrorPush.MirrorUP Then
                        AddLogEntry("Begin Push Mirror UP")
                    ElseIf Mirror_Step = eMirrorPush.MirrorDN Then
                        AddLogEntry("Begin Push Mirror Down")
                    ElseIf Mirror_Step = eMirrorPush.MirrorMR Then
                        AddLogEntry("Begin Push Mirror Right")
                    ElseIf Mirror_Step = eMirrorPush.MirrorML Then
                        AddLogEntry("Begin Push Mirror Left")
                    ElseIf Mirror_Step = eMirrorPush.MirrorSR Then
                        AddLogEntry("Begin Push Mirror Select Right")
                    ElseIf Mirror_Step = eMirrorPush.MirrorSL Then
                        AddLogEntry("Begin Push Mirror Select Left")
                    End If
                    ' clear data lin
                    For i = 0 To 20
                        StatusPushLin(i) = "000000"
                        SwitchTiming(i) = 0
                        NB_Transition = 0
                    Next
                    '
                    AddLogEntry("Reset Force Sensor  and Clear buffer Analog")
                    'If Not HbmReset Then
                    ' Set the force sensors reset signals
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_ResetForce)
                    'End If
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
                    AddLogEntry("Mirror Push is disabled")
                    _phase = ePhase.FinalState
                End If

            Case 1
                ' If the step in progress input is cleared (push activation)
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS05_StepInProgress) = False) Then
                    'Clear buffer
                    mWS04_05AIOManager.EmptyBuffer_WS05()
                    OffSetTimeForAIShareCards = Now
                    AddLogEntry("Clear Buffer Analog")
                    ' Start sampling the analog inputs
                    If Not mWS04_05AIOManager.Sample_Task_Started Then
                        '
                        mWS04_05AIOManager.StartSampling(samplingFrequency)
                    End If
                    AddLogEntry("Start Analog Sample " & vbTab & "Case : " & _subPhase(_phase))
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    AddLogEntry("Trasmit Frame Start Counter Transition " & vbTab & "Case : " & _subPhase(_phase))
                    If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Start_Haptic_DIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                        ' Go to next subphase
                        _subPhase(_phase) += 1
                    Else
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Start_Haptic_AIN).DeepClone
                        f.Data(2) = "09"
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
                    ' Store the time
                    t0 = Date.Now

                End If

            Case 1000
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(1) = "21"
                    If Mirror_Step = eMirrorPush.MirrorUP Then
                        MinLevel = Right("0000" & Hex(787), 4)
                        MaxLevel = Right("0000" & Hex(819), 4)
                    ElseIf Mirror_Step = eMirrorPush.MirrorDN Then
                        MinLevel = Right("0000" & Hex(665), 4)
                        MaxLevel = Right("0000" & Hex(709), 4)
                    ElseIf Mirror_Step = eMirrorPush.MirrorMR Then
                        MinLevel = Right("0000" & Hex(518), 4)
                        MaxLevel = Right("0000" & Hex(576), 4)
                    ElseIf Mirror_Step = eMirrorPush.MirrorML Then
                        MinLevel = Right("0000" & Hex(894), 4)
                        MaxLevel = Right("0000" & Hex(914), 4)
                    Else
                        MinLevel = "0000" : MaxLevel = "0000"
                    End If
                    f.Data(2) = Left(MinLevel, 2) : f.Data(3) = Right(MinLevel, 2)
                    f.Data(4) = Left(MaxLevel, 2) : f.Data(5) = Right(MaxLevel, 2)
                    f.Data(6) = "00"
                    f.Data(7) = "00"
                    AddLogEntry("Treesholds : " & MinLevel & " / " & MaxLevel)
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
                End If

            Case 2
                If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Start_Haptic_DIN))
                Else
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Start_Haptic_AIN))
                End If
                If (i <> -1) Then
                    AddLogEntry("Ack LIN Frame " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the force sensors reset signals
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_ResetForce)
                    ''
                    'HbmReset = True
                    ' Lin 
                    t0Lin = _LinInterface.ToSystemTime(CLng(_LinInterface.RxFrame(i).Timestamp))
                    tAcqLin = Format(t0Lin, "dd/MM/yyyy, HH:mm:ss:fff")
                    AddLogEntry("TimeStamp Lin : " & tAcqLin & vbTab & _LinInterface.RxFrame(i).Timestamp)
                    AddLogEntry("Difference systeme Time / lin Time : " & (Date.Now - t0Lin).TotalMilliseconds.ToString("0.00"))
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' stop Lin diag
                    _LinInterface.StopScheduleDiag()
                    ' Set the start step output (push activation)
                    AddLogEntry("Start Step to Activate the Robot")
                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then
                    ' stop Lin diag
                    _LinInterface.StopScheduleDiag()
                    ' Reset the force sensors reset signals
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_ResetForce)
                    AddLogEntry("Start Step to Activate the Robot")
                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 3
                ' If the step in progress input is set (push activation)
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = True Or _testMode = eTestMode.Debug) Then
                    _step = False
                    AddLogEntry("Start Step = 0 / Step in Progress = 1 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the force sensors reset signals
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_ResetForce)
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 4
                ' If the step in progress input is cleared (push activation)
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS05_StepInProgress) = False) Then
                    AddLogEntry("Step in Progress = 0 / ReadDataBlock  transition " & vbTab & "Case : " & _subPhase(_phase))
                    ' Stop sampling the analog inputs
                    AddLogEntry("Stop Analog Sample " & vbTab & "Case : " & _subPhase(_phase))
                    SampleCount = mWS04_05AIOManager.SampleCount_WS05
                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Start Robot for release
                    'to cancel this for reduce the Cycle Time if needed
                    AddLogEntry(" Start Robot for release")
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
                    '' Store the time
                    t0 = Date.Now
                    frameIndex = &H0
                    frameExt = False
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 5
                ' If the step in progress input is set (push activation)
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = True) Then
                    _step = False
                    AddLogEntry("Start Step = 0 / Step in Progress = 1 " & vbTab & "Case : " & _subPhase(_phase))
                    ' Reset the start step output (push activation)
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_StartStep)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                End If

            Case 6
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS05_StepInProgress) = False) Then
                    ' Read Raw Data
                    If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_DIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    Else
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    End If
                    msngT0TesteurPeriodicFrame = Date.Now
                    frameIndex = 0
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to subphase 9
                    _subPhase(_phase) += 1
                End If

            Case 7
                ' If the correct answer was received
                Try

                    If (frameIndex <> 21) Then
                        If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                            i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_RawTransition_DIN))
                        Else
                            i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_RawTransition_AIN))
                        End If
                    Else
                        i = -1
                    End If
                    If (i <> -1) Then
                        tAnalog = mWS04_05AIOManager.TimeStampSampleBuffer_WS05
                        AddLogEntry("Time Stamp Analog : " & Format(tAnalog, "dd/MM/yyyy, HH:mm:ss:fff"))
                        tAnalog = OffSetTimeForAIShareCards  'use this insteand from AI Cards. 
                        AddLogEntry("OffSetTimeForAIShareCards : " & Format(tAnalog, "dd/MM/yyyy, HH:mm:ss:fff"))
                        If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                            OffSetTime = (t0Lin - tAnalog).TotalMilliseconds - 10 '-10 only used for product response delay, remove after product software update.
                        Else
                            OffSetTime = (t0Lin - tAnalog).TotalMilliseconds - 20 '-20 only used for product response delay, remove after product software update.
                        End If
                        AddLogEntry("Offset Time Apply : " & OffSetTime)
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
                    ElseIf ((Date.Now - tLin).TotalMilliseconds >= 100) And frameIndex = 0 Then   ' Otherwise, if the LIN timeout expired
                        tLin = Date.Now
                        If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                            e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_DIN),
                                                                True,
                                                                txData_MasterReq,
                                                                cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                                True,
                                                                True)
                        Else
                            e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_RawTransition_AIN),
                                                                True,
                                                                txData_MasterReq,
                                                                cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                                True,
                                                                True)
                        End If
                        msngT0TesteurPeriodicFrame = Date.Now
                    ElseIf ((Date.Now - t0).TotalMilliseconds >= LINTimeout_ms) Then   ' Otherwise, if the LIN timeout expired
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
                                AddLogEntry("Data : " & s)
                                ' Haptic service Numeric
                                If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                                    For LinIndex = 0 To NB_Transition - 1
                                        ' initiate position
                                        StatusPushLin(LinIndex) = (Mid(s, (6 * LinIndex) + 1, 2))
                                        ' Add Delay for lin communication
                                        Dim str = "&h" & Mid(s, (6 * LinIndex) + 3, 4)
                                        SwitchTiming(LinIndex) = CSng(CSng("&h" & Mid(s, (6 * LinIndex) + 3, 4)) + OffSetTime) ' Checkif we can enable or not.YAN.Qian20200224
                                        AddLogEntry("Transition  Number " & LinIndex + 1 & vbTab & "Status : " & StatusPushLin(LinIndex) & vbTab & "Timming : " & SwitchTiming(LinIndex) & vbTab & "Timming Real: " & CSng("&h" & Mid(s, (6 * LinIndex) + 3, 4)))
                                    Next
                                Else
                                    For LinIndex = 0 To NB_Transition - 1
                                        ' initiate position
                                        StatusPushLin(LinIndex) = (Mid(s, (8 * LinIndex) + 3, 2))
                                        ' Add Delay for lin communication
                                        Dim str = "&h" & Mid(s, (8 * LinIndex) + 5, 4)
                                        'SwitchTiming(LinIndex) = CSng(CSng("&h" & Mid(s, (8 * LinIndex) + 5, 4))) ' + OffSetTime) ' temporaire
                                        SwitchTiming(LinIndex) = CSng(CSng("&h" & Mid(s, (8 * LinIndex) + 5, 4)) + OffSetTime) ' Checkif we can enable or not.YAN.Qian20200224
                                        AddLogEntry("Transition  Number " & LinIndex + 1 & vbTab & "Status : " & StatusPushLin(LinIndex) & vbTab & "Timming : " & SwitchTiming(LinIndex) & vbTab & "Timming Real: " & CSng("&h" & Mid(s, (8 * LinIndex) + 5, 4)))
                                    Next
                                End If
                                AddLogEntry("t0Lin.AddMilliseconds(SwitchTiming(1) : " & Format(t0Lin.AddMilliseconds(SwitchTiming(1)), "dd/MM/yyyy, HH:mm:ss:fff"))
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

            Case 8
                If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Stop_Haptic_DIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                Else
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
                tLin = Date.Now
                ' Go to subphase 10
                _subPhase(_phase) += 1

            Case 9
                ' If the correct answer was received
                If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
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
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= 200) Then
                    If Mirror_Step = eMirrorPush.MirrorSR Or Mirror_Step = eMirrorPush.MirrorSL Then
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Stop_Haptic_DIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    Else
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Stop_Haptic_AIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    End If
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Store the time
                    tLin = Date.Now
                End If

            Case 10
                '
                Try

                    '
                    If Simulation_Test Then
                        frmProduction.OpenFileDialog1.ShowDialog()
                        mWS05Main.FilePoint = frmProduction.OpenFileDialog1.FileName
                        Read_AnalogPoint(FilePoint, Mirror_Step)
                    Else
                        ' initialize Step
                        LinIndex = 0
                        ' Store the sample count
                        _results.SampleCount(Mirror_Step) = SampleCount 'mWS04_05AIOManager.SampleCount_WS05
                        If (_results.SampleCount(Mirror_Step) > cWS05Results.MaxSampleCount) Then
                            _results.SampleCount(Mirror_Step) = cWS05Results.MaxSampleCount
                        End If
                        Dim indexLin As Integer
                        indexLin = 0
                        ' Store the samples
                        For sampleIndex = indexLin To _results.SampleCount(Mirror_Step) - 1
                            ' Early Sensor
                            _results.Sample(eSignal_Analog.EarlySensor, Mirror_Step, sampleIndex - indexLin) =
                                mWS04_05AIOManager.SampleBuffer_WS05(mWS05AIOManager.eAnalogInput.WS05_EarlySensor, sampleIndex) * OffsetStrokeSensor
                            ' Strenght Sensor
                            _results.Sample(eSignal_Analog.StrenghtSensor, Mirror_Step, sampleIndex - indexLin) =
                                mWS04_05AIOManager.SampleBuffer_WS05(mWS05AIOManager.eAnalogInput.WS05_StrenghtSensor, sampleIndex) * OffsetStrenghtSensor
                            '
                            _results.SampleTimeStamp(eSignal_Analog.EarlySensor, Mirror_Step, sampleIndex - indexLin) =
                                Format(mWS04_05AIOManager.TimeStampSampleBuffer(mWS05AIOManager.eAnalogInput.WS05_EarlySensor, sampleIndex), "dd/MM/yyyy, HH:mm:ss:fff")

                            If _results.SampleTimeStamp(eSignal_Analog.EarlySensor, Mirror_Step, sampleIndex - indexLin) = Format(t0Lin.AddMilliseconds(SwitchTiming(1)), "dd/MM/yyyy, HH:mm:ss:fff") Then
                                AddLogEntry("sampleIndex : " & sampleIndex & " = t0Lin.AddMilliseconds(SwitchTiming(1))")
                                If SwitchTiming(LinIndex + 1) > 0 And LinIndex = 0 Then
                                    LinIndex = 1
                                End If
                            End If
                            ' Push Mirror UP
                            If Mirror_Step = eMirrorPush.MirrorUP Then
                                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_UP + 2, Mirror_Step, sampleIndex - indexLin) =
                                    CInt((Hex((CByte("&H" & Mid((Hex(StatusPushLin(LinIndex) Xor &HFF)), 1, 2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
                            ElseIf Mirror_Step = eMirrorPush.MirrorDN Then
                                ' Push Mirror DN
                                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_DN + 2, Mirror_Step, sampleIndex - indexLin) =
                                    CInt((Hex((CByte("&H" & Mid((Hex(StatusPushLin(LinIndex) Xor &HFF)), 1, 2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
                            ElseIf Mirror_Step = eMirrorPush.MirrorMR Then
                                ' Push Mirror Right
                                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_MR + 2, Mirror_Step, sampleIndex - indexLin) =
                                    CInt((Hex((CByte("&H" & Mid((Hex(StatusPushLin(LinIndex) Xor &HFF)), 1, 2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
                            ElseIf Mirror_Step = eMirrorPush.MirrorML Then
                                ' Push Mirror Left
                                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_ML + 2, Mirror_Step, sampleIndex - indexLin) =
                                    CInt((Hex((CByte("&H" & Mid((Hex(StatusPushLin(LinIndex) Xor &HFF)), 1, 2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
                            ElseIf Mirror_Step = eMirrorPush.MirrorSR Then
                                ' Push Mirror Select Right
                                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_SR + 2, Mirror_Step, sampleIndex - indexLin) =
                                    CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 2) And &H1)))) * 13.5
                            ElseIf Mirror_Step = eMirrorPush.MirrorSL Then
                                ' Push Mirror Select Left
                                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_SL + 2, Mirror_Step, sampleIndex - indexLin) =
                                    CInt((Hex((CByte("&H" & Mid(StatusPushLin(LinIndex), 1, 2)) \ CByte(2 ^ 6) And &H1)))) * 13.5
                            End If
                        Next
                        _results.SampleCount(Mirror_Step) = _results.SampleCount(Mirror_Step) - indexLin
                    End If

                    'Write in file for analyse
                    If WriteFilePoint Then
                        If Not Simulation_Test Then
                            Write_AnalogPoint(Mirror_Step, -1)
                        End If
                    End If

                    ' Push Mirror UP
                    If Mirror_Step >= eMirrorPush.MirrorUP And Mirror_Step <= eMirrorPush.MirrorML Then
                        If SwitchTiming(2) - SwitchTiming(1) < 100 Then
                            _results.InitialState(Mirror_Step).Value = CInt((Hex((CByte("&H" & Mid((Hex(StatusPushLin(1) Xor &HFF)), 1, 2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
                        Else
                            _results.InitialState(Mirror_Step).Value = CInt((Hex((CByte("&H" & Mid((Hex(StatusPushLin(2) Xor &HFF)), 1, 2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
                        End If
                    ElseIf Mirror_Step = eMirrorPush.MirrorSR Then
                        If SwitchTiming(2) - SwitchTiming(1) < 100 Then
                            _results.InitialState(Mirror_Step).Value = CInt((Hex((CByte("&H" & Mid(StatusPushLin(1), 1, 2)) \ CByte(2 ^ 6) And &H1)))) * 13.5
                        Else
                            _results.InitialState(Mirror_Step).Value = CInt((Hex((CByte("&H" & Mid(StatusPushLin(2), 1, 2)) \ CByte(2 ^ 6) And &H1)))) * 13.5
                        End If

                    ElseIf Mirror_Step = eMirrorPush.MirrorSL Then
                        If SwitchTiming(2) - SwitchTiming(1) < 100 Then
                            _results.InitialState(Mirror_Step).Value = CInt((Hex((CByte("&H" & Mid(StatusPushLin(1), 1, 2)) \ CByte(2 ^ 2) And &H1)))) * 13.5
                        Else
                            _results.InitialState(Mirror_Step).Value = CInt((Hex((CByte("&H" & Mid(StatusPushLin(2), 1, 2)) \ CByte(2 ^ 2) And &H1)))) * 13.5
                        End If
                    End If

                    'Filter Curve
                    ' Filter the samples
                    FilterSamples_Global(eSignal_Analog.EarlySensor, mWS05Main.Settings.FilterCurveDistance, Mirror_Step)
                    FilterSamples_Global(eSignal_Analog.StrenghtSensor, mWS05Main.Settings.FilterCurveForce, Mirror_Step)
                    ''Write in file for analyse
                    'If WriteFilePoint Then
                    '    If Not Simulation_Test Then
                    '        Write_AnalogPoint(Mirror_Step, 0)
                    '    End If
                    'End If
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 11
                    _subPhase(_phase) = 20
                    Exit Select
                Catch ex As Exception
                    Dim errdes As String
                    errdes = Err.Description
                    AddLogEntry("Sub Phase 10 Error : " & errdes)

                    ' Go to subphase 11
                    _subPhase(_phase) = 20

                End Try

            Case 20
                '******************
                ' Push Mirror
                '******************
                ProcessSamples_BaseTps(Mirror_Step, MirrorResult)
                ' Store the commutation Early, the overstroke, the Strenght peak F1, the Early Strenght S1, the Activation Strenght F2 and the Tactile Ratio
                _results.Mirror_XCe1(Mirror_Step).Value = MirrorResult.XCe1
                _results.Mirror_Xe(Mirror_Step).Value = MirrorResult.Xe
                ' Force
                _results.Mirror_Fs1_F1(Mirror_Step).Value = MirrorResult.Fs1_F1
                _results.Mirror_Xs1(Mirror_Step).Value = MirrorResult.Xs1
                _results.Mirror_dFs1_Haptic_1(Mirror_Step).Value = MirrorResult.dFs1_Haptic_1
                _results.Mirror_dXs1(Mirror_Step).Value = MirrorResult.dXs1
                _results.Mirror_Fe(Mirror_Step).Value = MirrorResult.Fe
                _results.Mirror_DiffS2Ce1(Mirror_Step).Value = MirrorResult.Value_DiffS2Ce1

                _results.mirror_X_Indexes(Mirror_Step).Xs = MirrorResult.Xs_Index
                _results.mirror_X_Indexes(Mirror_Step).Xe = MirrorResult.Xe_Index
                _results.mirror_X_Indexes(Mirror_Step).X_Ce1 = MirrorResult.X_Ce1_index
                _results.mirror_X_Indexes(Mirror_Step).X_F1 = MirrorResult.Xs_F1_index
                _results.Mirror_X_Indexes(Mirror_Step).X_F2 = MirrorResult.Xs_F2_index
                If (SaveFSScreenshot) Then
                    Dim filename As String = Mirror_Step.ToString() +
                        Mid(_results.TestDate.Value.ToString, 7, 4) +
                        Mid(_results.TestDate.Value.ToString, 4, 2) +
                        Mid(_results.TestDate.Value.ToString, 1, 2) +
                        "_" + _results.PartUniqueNumber.Value.ToString
                    mResults.SavePushReturnFSCurve(2, _results.Mirror_Xe(Mirror_Step).Value,
                                                 _results.Sample(mWS05Main.cSample_Signal.EarlySensor, Mirror_Step, _results.Mirror_X_Indexes(Mirror_Step).Xs),
                                                 _results.Mirror_X_Indexes(Mirror_Step).Xs, _results.Mirror_X_Indexes(Mirror_Step).Xe,
                                                 _results.Sample, Mirror_Step,
                                                 _results.Mirror_X_Indexes(Mirror_Step).X_F1,
                                                 _results.Mirror_X_Indexes(Mirror_Step).X_F2,
                                                 mWS05Main.cSample_Signal.EarlySensor,
                                                 mWS05Main.cSample_Signal.StrenghtSensor, filename)
                End If
                ' Go to subphase 100
                _subPhase(_phase) = 100

            Case 100
                '*****************************
                ' Push 
                '*****************************
                ' Test the values before and after commutation
                failedOff = False
                failedOnCe1 = False
                For sampleIndex = _results.Mirror_X_Indexes(Mirror_Step).Xs To _results.SampleCount(Mirror_Step) - 1
                    If (Not (failedOff) AndAlso
                        sampleIndex < (_results.Mirror_X_Indexes(Mirror_Step).X_Ce1 - timeBeforeCommutation) OrElse
                            _results.Mirror_XCe1(Mirror_Step).Value = 0) Then
                        '
                        _results.Mirror_Push_Electric(0, Mirror_Step).Value = _results.Sample(2 + Mirror_Step, Mirror_Step, sampleIndex)
                        '
                        failedOff = failedOff OrElse (_results.Mirror_Push_Electric(0, Mirror_Step).Test <> cResultValue.eValueTestResult.Passed)
                    End If

                    If (Not (failedOnCe1) AndAlso
                        sampleIndex > (_results.Mirror_X_Indexes(Mirror_Step).X_Ce1 + timeBeforeCommutation) OrElse
                            _results.Mirror_XCe1(Mirror_Step).Value = 0) Then
                        '
                        _results.Mirror_Push_Electric(1, Mirror_Step).Value = _results.Sample(2 + Mirror_Step, Mirror_Step, sampleIndex)
                        '
                        failedOnCe1 = failedOnCe1 OrElse (_results.Mirror_Push_Electric(1, Mirror_Step).Test <> cResultValue.eValueTestResult.Passed)
                    End If

                Next

                ' Update the single test result
                If (failedOff Or failedOnCe1) And Mirror_Step = eMirrorPush.MirrorUP And
                    _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorUP_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_ElectricalTest
                    End If
                ElseIf (failedOff Or failedOnCe1) And Mirror_Step = eMirrorPush.MirrorDN And
                    _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorDN_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_ElectricalTest
                    End If
                ElseIf (failedOff Or failedOnCe1) And Mirror_Step = eMirrorPush.MirrorMR And
                    _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorMR_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_ElectricalTest
                    End If
                ElseIf (failedOff Or failedOnCe1) And Mirror_Step = eMirrorPush.MirrorML And
                    _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorML_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_ElectricalTest
                    End If
                ElseIf (failedOff Or failedOnCe1) And Mirror_Step = eMirrorPush.MirrorSR And
                    _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorSR_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_ElectricalTest
                    End If
                ElseIf (failedOff Or failedOnCe1) And Mirror_Step = eMirrorPush.MirrorSL And
                    _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorSL_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_ElectricalTest
                    End If
                End If

                ' Update the single test result
                If Mirror_Step = eMirrorPush.MirrorUP Then
                    If (_results.InitialState(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                       _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorUP_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_ElectricalTest
                        End If
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorDN Then
                    If (_results.InitialState(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                          _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorDN_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_ElectricalTest
                        End If
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorMR Then
                    If (_results.InitialState(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                           _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorMR_Electrical.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_ElectricalTest
                        End If
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorML Then
                    If (_results.InitialState(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                    _results.ePUSH_MirrorML_Electrical.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_ElectricalTest
                        End If
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorSR Then
                    If (_results.InitialState(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                    _results.ePUSH_MirrorSR_Electrical.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_ElectricalTest
                        End If
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorSL Then
                    If (_results.InitialState(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                            _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSL_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_ElectricalTest
                        End If
                    End If
                End If
                ' Check all data is checked
                For i = 0 To 1
                    If (_results.Mirror_Push_Electric(i, Mirror_Step).TestResult = cResultValue.eValueTestResult.Unknown) Then

                        If Mirror_Step = eMirrorPush.MirrorUP And _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                                _results.ePUSH_MirrorUP_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_ElectricalTest
                            End If
                        ElseIf Mirror_Step = eMirrorPush.MirrorDN And _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                                _results.ePUSH_MirrorDN_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_ElectricalTest
                            End If
                        ElseIf Mirror_Step = eMirrorPush.MirrorMR And _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                                _results.ePUSH_MirrorMR_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_ElectricalTest
                            End If
                        ElseIf Mirror_Step = eMirrorPush.MirrorML And _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                                _results.ePUSH_MirrorML_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_ElectricalTest
                            End If
                        ElseIf Mirror_Step = eMirrorPush.MirrorSR And _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                                _results.ePUSH_MirrorSR_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_ElectricalTest
                            End If
                        ElseIf Mirror_Step = eMirrorPush.MirrorSL And _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                            If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                                _results.ePUSH_MirrorSL_Electrical.TestResult <>
                                    cResultValue.eValueTestResult.Passed) Then
                                _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_ElectricalTest
                            End If
                        End If
                    End If
                Next i

                ' Check Result
                '****************************
                '   Push Mirror
                '****************************
                If (Mirror_Step = eMirrorPush.MirrorUP) Then
                    ' Test the Peak Strenght F1
                    If (_results.Mirror_Fs1_F1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_Xs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorUP_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorUP_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_PeakF1Test
                        End If
                    End If
                    ' Test the Tactile Ratio
                    If (_results.Mirror_dFs1_Haptic_1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_dXs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorUP_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorUP_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_F1F2Test
                        End If
                    End If
                    ' Defect Early ce
                    If (_results.Mirror_XCe1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.Mirror_DiffS2Ce1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorUP_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_Ce1Test
                        End If
                    End If
                    ' Defect Over Early
                    If (_results.Mirror_Xe(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorUP_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_OverStrokeTest
                        End If
                    End If
                    If _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                    If _results.ePUSH_MirrorUP_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                ElseIf (Mirror_Step = eMirrorPush.MirrorDN) Then
                    ' Test the Peak Strenght F1
                    If (_results.Mirror_Fs1_F1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_Xs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorDN_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorDN_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_PeakF1Test
                        End If
                    End If
                    ' Test the Tactile Ratio
                    If (_results.Mirror_dFs1_Haptic_1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_dXs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorDN_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorDN_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_F1F2Test
                        End If
                    End If
                    ' Defect Early ce
                    If (_results.Mirror_XCe1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.Mirror_DiffS2Ce1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorDN_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_Ce1Test
                        End If
                    End If
                    ' Defect Over Early
                    If (_results.Mirror_Xe(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorDN_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_OverStrokeTest
                        End If
                    End If
                    If _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                    If _results.ePUSH_MirrorDN_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                ElseIf (Mirror_Step = eMirrorPush.MirrorMR) Then
                    ' Test the Peak Strenght F1
                    If (_results.Mirror_Fs1_F1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_Xs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorMR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorMR_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_PeakF1Test
                        End If
                    End If
                    ' Test the Tactile Ratio
                    If (_results.Mirror_dFs1_Haptic_1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_dXs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorMR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorMR_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_F1F2Test
                        End If
                    End If
                    ' Defect Early ce
                    If (_results.Mirror_XCe1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.Mirror_DiffS2Ce1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorMR_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_Ce1Test
                        End If
                    End If
                    ' Defect Over Early
                    If (_results.Mirror_Xe(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorMR_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_OverStrokeTest
                        End If
                    End If
                    If _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                    If _results.ePUSH_MirrorMR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                ElseIf (Mirror_Step = eMirrorPush.MirrorML) Then
                    ' Test the Peak Strenght F1
                    If (_results.Mirror_Fs1_F1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_Xs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorML_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorML_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_PeakF1Test
                        End If
                    End If
                    ' Test the Tactile Ratio
                    If (_results.Mirror_dFs1_Haptic_1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_dXs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorML_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorML_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_F1F2Test
                        End If
                    End If
                    ' Defect Early ce
                    If (_results.Mirror_XCe1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.Mirror_DiffS2Ce1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorML_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_Ce1Test
                        End If
                    End If
                    ' Defect Over Early
                    If (_results.Mirror_Xe(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorML_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_OverStrokeTest
                        End If
                    End If
                    If _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                    If _results.ePUSH_MirrorML_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                ElseIf (Mirror_Step = eMirrorPush.MirrorSR) Then
                    ' Test the Peak Strenght F1
                    If (_results.Mirror_Fs1_F1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_Xs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorSR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSR_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_PeakF1Test
                        End If
                    End If
                    ' Test the Tactile Ratio
                    If (_results.Mirror_dFs1_Haptic_1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_dXs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorSR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSR_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_F1F2Test
                        End If
                    End If
                    ' Defect Early ce
                    If (_results.Mirror_XCe1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.Mirror_DiffS2Ce1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSR_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_Ce1Test
                        End If
                    End If
                    ' Defect Over Early
                    If (_results.Mirror_Xe(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSR_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_OverStrokeTest
                        End If
                    End If
                    If _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                    If _results.ePUSH_MirrorSR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                ElseIf (Mirror_Step = eMirrorPush.MirrorSL) Then
                    ' Test the Peak Strenght F1
                    If (_results.Mirror_Fs1_F1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_Xs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorSL_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSL_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_PeakF1Test
                        End If
                    End If
                    ' Test the Tactile Ratio
                    If (_results.Mirror_dFs1_Haptic_1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Mirror_dXs1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorSL_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Strenght.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSL_Strenght.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_F1F2Test
                        End If
                    End If
                    ' Defect Early ce
                    If (_results.Mirror_XCe1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed Or
                            _results.Mirror_DiffS2Ce1(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSL_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_Ce1Test
                        End If
                    End If
                    ' Defect Over Early
                    If (_results.Mirror_Xe(Mirror_Step).Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Failed
                        If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                            _results.ePUSH_MirrorSL_Electrical.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                            _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_OverStrokeTest
                        End If
                    End If
                    If _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Passed
                    End If
                    If _results.ePUSH_MirrorSL_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Strenght.TestResult = cResultValue.eValueTestResult.Passed
                    End If

                End If

                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If Mirror_Step = eMirrorPush.MirrorUP Then
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorUP_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorUP_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_ElectricalTest
                    End If
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorUP_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorUP_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorUP_StrenghtTest
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorDN Then
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorDN_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorDN_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_ElectricalTest
                    End If
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorDN_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorDN_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorDN_StrenghtTest
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorMR Then
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorMR_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorMR_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_ElectricalTest
                    End If
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorMR_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorMR_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorMR_StrenghtTest
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorML Then
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorML_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorML_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_ElectricalTest
                    End If
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorML_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorML_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorML_StrenghtTest
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorSR Then
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorSR_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorSR_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_ElectricalTest
                    End If
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorSR_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorSR_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorSR_StrenghtTest
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorSL Then
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorSL_Electrical.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorSL_Electrical.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_ElectricalTest
                    End If
                    If (_results.TestResult = cWS05Results.eTestResult.Unknown And
                        _results.ePUSH_MirrorSL_Strenght.TestResult <> cResultValue.eValueTestResult.Passed And
                        _results.ePUSH_MirrorSL_Strenght.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        _results.TestResult = cWS05Results.eTestResult.FailedMirrorSL_StrenghtTest
                    End If
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                If _phase = ePhase.Push_Mirror_UP Then
                    _phase = ePhase.Push_Mirror_DN
                ElseIf _phase = ePhase.Push_Mirror_DN Then
                    _phase = ePhase.Push_Mirror_Right
                ElseIf _phase = ePhase.Push_Mirror_Right Then
                    _phase = ePhase.Push_Mirror_Left
                ElseIf _phase = ePhase.Push_Mirror_Left Then
                    _phase = ePhase.Push_Mirror_SR
                ElseIf _phase = ePhase.Push_Mirror_SR Then
                    _phase = ePhase.Push_Mirror_SL
                ElseIf _phase = ePhase.Push_Mirror_SL Then
                    If (_testMode = eTestMode.Debug AndAlso Simulation_Test) Then
                        ' Go to phase WaitStartTest
                        _phase = ePhase.WaitStartTest
                    Else
                        ' Go to next phase
                        _phase = ePhase.FinalState
                    End If
                End If

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                ' Go to next phase
                If Mirror_Step = eMirrorPush.MirrorUP Then
                    If _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Electrical.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.ePUSH_MirrorUP_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorUP_Strenght.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorDN Then
                    If _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Electrical.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.ePUSH_MirrorDN_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorDN_Strenght.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorMR Then
                    If _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Electrical.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.ePUSH_MirrorMR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorMR_Strenght.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorML Then
                    If _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Electrical.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.ePUSH_MirrorML_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorML_Strenght.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorSR Then
                    If _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Electrical.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.ePUSH_MirrorSR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSR_Strenght.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                ElseIf Mirror_Step = eMirrorPush.MirrorSL Then
                    If _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Electrical.TestResult = _
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.ePUSH_MirrorSL_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.ePUSH_MirrorSL_Strenght.TestResult = _
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
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
        Dim sample(0 To mWS05AIOManager.eAnalogInput.Count - 1) As Double
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
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS05_StepInProgress) = False) Then
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_ALL_DIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 2
                End If

            Case 2
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Read_ALL_DIN))
                If (i <> -1) Then
                    _results.FinalState(0).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 2) And &H1)))) * 13.5
                    _results.FinalState(1).Value = CInt((Hex((CByte("&H" & _LinInterface.RxFrame(i).Data(4)) \ CByte(2 ^ 6) And &H1)))) * 13.5
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
                        _results.eFINAL_STATE_PRODUCT.TestResult = cWS05Results.eSingleTestResult.Failed
                    End If
                Next

                ' Update the test result
                If _results.eFINAL_STATE_PRODUCT.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eFINAL_STATE_PRODUCT.TestResult = cResultValue.eValueTestResult.Passed
                End If
                'If (_results.FinalState(0).Test <> cResultValue.eValueTestResult.Passed And CBool(_recipe.TestEnable_Mirror_Electrical.Value)) Then
                '    _results.eFINAL_STATE_PRODUCT.TestResult = _
                '        cWS05Results.eSingleTestResult.Failed
                'End If
                ' Update the test result
                If _results.eFINAL_STATE_PRODUCT.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eFINAL_STATE_PRODUCT.TestResult = _
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS05Results.eTestResult.Unknown And _
                    _results.eFINAL_STATE_PRODUCT.TestResult <> _
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS05Results.eTestResult.FailedFinalStateProduct
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Write_MMS_TestByte

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eFINAL_STATE_PRODUCT.TestResult = _
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
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
                If _results.TestResult = cWS05Results.eTestResult.Unknown Then
                    If _results.MMS_Test_Byte_Before.Value = "51" Or _results.MMS_Test_Byte_Before.Value = "52" Or _results.MMS_Test_Byte_Before.Value = "57" Then
                        'WS05 Retest.
                        f.Data(5) = "57"
                    Else
                        'WS05 First Test
                        f.Data(5) = "51"
                    End If
                Else
                    f.Data(5) = "52"
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
                _results.eWrite_MMSTestByte.TestResult = _
                        cResultValue.eValueTestResult.Passed
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS05Results.eTestResult.Unknown And _
                    _results.eWrite_MMSTestByte.TestResult <> _
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS05Results.eTestResult.FailedWriteTestByte
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Read_MMS_TestByte

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eWrite_MMSTestByte.TestResult = _
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
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
                    If (_LinInterface.RxFrame(i).Data(5) = "51" Or _LinInterface.RxFrame(i).Data(5) = "57") Then
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
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_TestByte), _
                                                        True, _
                                                        txData_MasterReq, _
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                                                        True, _
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
                    _results.eRead_MMSTestByte.TestResult = _
                            cResultValue.eValueTestResult.Failed
                Else
                    _results.eRead_MMSTestByte.TestResult = _
                            cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS05Results.eTestResult.Unknown And _
                    _results.eRead_MMSTestByte.TestResult <> _
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS05Results.eTestResult.FailedCheckMMSTestByte
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.PowerDown

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eRead_MMSTestByte.TestResult = _
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
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
                msngT0TesteurPeriodicFrame = Date.Now
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
                    For j = 0 To mWS05DIOManager.eDigitalOutput.Count - 1
                        e = e Or mWS05DIOManager.ResetDigitalOutput(j)
                    Next
                    e = mWS05DIOManager.SetDigitalOutput(mWS05DIOManager.eDigitalOutput.DO_LocalSensing)
                    e = e Or mWS05DIOManager.ResetDigitalOutput(mWS05DIOManager.eDigitalOutput.DO_RemoteSensing)
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
            _results.TestResult = cWS05Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub

    Private Sub AddLogEntry(ByVal logEntry As String)
        Dim t As Date
        ' Get the date
        t = Date.Now
        ' Update the log
        _log.Append(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss:fff") & _
                    " - " & logEntry & vbCrLf)
    End Sub

    Private Function CheckPartTypeValue(ByRef resultValue As cResultValue, _
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

        _recipeMaster = New cWS05Recipe

        CheckMasterReference = False

        Try
            _recipeMaster.LoadConfiguration(_settings.RecipeConfigurationPath)
            ' If the recipe in progress loading succeeds
            If Not (_recipe.Load(name)) Then
                MasterReference = True
                ' If the recipe of master reference loading succeeds
                If Not (_recipeMaster.Load(name)) Then
                    'Check parameter
                    For j = 10 To cWS05Recipe.ValueCount - 1
                        If _recipeMaster.Value(j) IsNot Nothing Then
                            If (_recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or _
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or _
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or _
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                                If (_recipeMaster.Value(j).MinimumLimit <> _recipe.Value(j).MinimumLimit) Then
                                    ' Return error 
                                    _results.TestResult = cWS05Results.eTestResult.FailedMasterReference
                                    CheckMasterReference = True
                                ElseIf (_recipeMaster.Value(j).MaximumLimit <> _recipe.Value(j).MaximumLimit) Then
                                    ' Return error 
                                    _results.TestResult = cWS05Results.eTestResult.FailedMasterReference
                                    CheckMasterReference = True
                                End If
                            ElseIf (_recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.BCDValue Or _
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.BooleanValue Or _
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.HexValue Or _
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.IntegerValue Or _
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.SingleValue Or _
                                    _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.StringValue) Then
                                If (_recipeMaster.Value(j).Value <> _recipe.Value(j).Value) Then
                                    ' Return error 
                                    _results.TestResult = cWS05Results.eTestResult.FailedMasterReference
                                    CheckMasterReference = True
                                End If
                            End If
                        End If
                    Next j
                    ' Otherwise, if the recipe master reference loading fails
                Else
                    ' Return error 
                    _results.TestResult = cWS05Results.eTestResult.FailedMasterReference
                    CheckMasterReference = True
                End If
                ' Otherwise, if the recipe in progress loading fails
            Else
                ' Return True
                _results.TestResult = cWS05Results.eTestResult.FailedMasterReference
                CheckMasterReference = True
            End If

        Catch ex As Exception
            ' Return True
            _results.TestResult = cWS05Results.eTestResult.FailedMasterReference
            CheckMasterReference = True

        End Try

        MasterReference = False
    End Function


    Private Sub ClearResults()
        Dim i As Integer
        ' Clear all the values
        For i = 0 To cWS05Results.ValueCount - 1
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
        _results.TestResult = cWS05Results.eTestResult.Unknown
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
            _results.PartUniqueNumber.Value = Trim(mWS05Ethernet.InputValue(mWS05Ethernet.eInput.UniqueNumber))
        End If
        _results.PartUniqueNumber.TestResult = cResultValue.eValueTestResult.NotTested
        ' Part type number
        _results.PartTypeNumber.Value = 0
        If (_HardwareEnabled_PLC) Then
            _results.PartTypeNumber.Value = mWS05Ethernet.InputValue(mWS05Ethernet.eInput.PartTypeNumber)
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
        _results.MMS_Test_Byte_Before.MinimumLimit = "04"
        _results.MMS_Test_Byte_Before.MaximumLimit = "04"
        'Rear Left Pull
        If CBool(_recipe.TestEnable_Mirror_Electrical.Value) Then
            '**************************
            'Push Mirror UP
            '**************************
            _results.ePUSH_MirrorUP_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_XCe1(eMirrorPush.MirrorUP).MinimumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorUP).MinimumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorUP).MaximumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorUP).MaximumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorUP).Value = 0
            _results.Mirror_XCe1(eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorUP).MinimumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorUP).MinimumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorUP).MaximumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorUP).MaximumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorUP).Value = 0
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_Xe(eMirrorPush.MirrorUP).MinimumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorUP).MinimumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorUP).MaximumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorUP).MaximumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorUP).Value = 0
            _results.Mirror_Xe(eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Mirror_Strenght.Value) Then
                _results.ePUSH_MirrorUP_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Fs1_F1(eMirrorPush.MirrorUP).MinimumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorUP).MinimumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorUP).MaximumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorUP).MaximumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorUP).Value = 0
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Xs1(eMirrorPush.MirrorUP).MinimumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorUP).MinimumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorUP).MaximumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorUP).MaximumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorUP).Value = 0
                _results.Mirror_Xs1(eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorUP).MinimumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorUP).MinimumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorUP).MaximumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorUP).MaximumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorUP).Value = 0
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dXs1(eMirrorPush.MirrorUP).MinimumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorUP).MinimumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorUP).MaximumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorUP).MaximumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorUP).Value = 0
                _results.Mirror_dXs1(eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown
            End If
            ' Init data for Electrical Test Off/Ce1
            For i = 0 To 1
                If i = 0 Then
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorUP).MinimumLimit = 13.4
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorUP).MaximumLimit = 13.6
                Else
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorUP).MinimumLimit = 0
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorUP).MaximumLimit = 0.2
                End If
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorUP).Value = 0
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown
            Next
            _results.InitialState(eMirrorPush.MirrorUP).MinimumLimit = 13.4
            _results.InitialState(eMirrorPush.MirrorUP).MaximumLimit = 13.6
            _results.InitialState(eMirrorPush.MirrorUP).TestResult = cResultValue.eValueTestResult.Unknown
            '**************************
            'Push Mirror DN
            '**************************
            _results.ePUSH_MirrorDN_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_XCe1(eMirrorPush.MirrorDN).MinimumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorDN).MinimumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorDN).MaximumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorDN).MaximumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorDN).Value = 0
            _results.Mirror_XCe1(eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorDN).MinimumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorDN).MinimumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorDN).MaximumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorDN).MaximumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorDN).Value = 0
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_Xe(eMirrorPush.MirrorDN).MinimumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorDN).MinimumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorDN).MaximumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorDN).MaximumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorDN).Value = 0
            _results.Mirror_Xe(eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Mirror_Strenght.Value) Then
                _results.ePUSH_MirrorDN_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Fs1_F1(eMirrorPush.MirrorDN).MinimumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorDN).MinimumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorDN).MaximumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorDN).MaximumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorDN).Value = 0
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Xs1(eMirrorPush.MirrorDN).MinimumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorDN).MinimumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorDN).MaximumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorDN).MaximumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorDN).Value = 0
                _results.Mirror_Xs1(eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorDN).MinimumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorDN).MinimumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorDN).MaximumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorDN).MaximumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorDN).Value = 0
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dXs1(eMirrorPush.MirrorDN).MinimumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorDN).MinimumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorDN).MaximumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorDN).MaximumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorDN).Value = 0
                _results.Mirror_dXs1(eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown
            End If
            ' Init data for Electrical Test Off/Ce1
            For i = 0 To 1
                If i = 0 Then
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorDN).MinimumLimit = 13.4
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorDN).MaximumLimit = 13.6
                Else
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorDN).MinimumLimit = 0
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorDN).MaximumLimit = 0.2
                End If
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorDN).Value = 0
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown
            Next
            _results.InitialState(eMirrorPush.MirrorDN).MinimumLimit = 13.4
            _results.InitialState(eMirrorPush.MirrorDN).MaximumLimit = 13.6
            _results.InitialState(eMirrorPush.MirrorDN).TestResult = cResultValue.eValueTestResult.Unknown

            '**************************
            'Push Mirror MR
            '**************************
            _results.ePUSH_MirrorMR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_XCe1(eMirrorPush.MirrorMR).MinimumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorMR).MinimumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorMR).MaximumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorMR).MaximumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorMR).Value = 0
            _results.Mirror_XCe1(eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorMR).MinimumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorMR).MinimumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorMR).MaximumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorMR).MaximumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorMR).Value = 0
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_Xe(eMirrorPush.MirrorMR).MinimumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorMR).MinimumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorMR).MaximumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorMR).MaximumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorMR).Value = 0
            _results.Mirror_Xe(eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Mirror_Strenght.Value) Then
                _results.ePUSH_MirrorMR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Fs1_F1(eMirrorPush.MirrorMR).MinimumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorMR).MinimumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorMR).MaximumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorMR).MaximumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorMR).Value = 0
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Xs1(eMirrorPush.MirrorMR).MinimumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorMR).MinimumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorMR).MaximumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorMR).MaximumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorMR).Value = 0
                _results.Mirror_Xs1(eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorMR).MinimumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorMR).MinimumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorMR).MaximumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorMR).MaximumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorMR).Value = 0
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dXs1(eMirrorPush.MirrorMR).MinimumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorMR).MinimumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorMR).MaximumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorMR).MaximumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorMR).Value = 0
                _results.Mirror_dXs1(eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown
            End If
            ' Init data for Electrical Test Off/Ce1
            For i = 0 To 1
                If i = 0 Then
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorMR).MinimumLimit = 13.4
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorMR).MaximumLimit = 13.6
                Else
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorMR).MinimumLimit = 0
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorMR).MaximumLimit = 0.2
                End If
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorMR).Value = 0
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown
            Next
            _results.InitialState(eMirrorPush.MirrorMR).MinimumLimit = 13.4
            _results.InitialState(eMirrorPush.MirrorMR).MaximumLimit = 13.6
            _results.InitialState(eMirrorPush.MirrorMR).TestResult = cResultValue.eValueTestResult.Unknown

            '**************************
            'Push Mirror ML
            '**************************
            _results.ePUSH_MirrorML_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_XCe1(eMirrorPush.MirrorML).MinimumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorML).MinimumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorML).MaximumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorML).MaximumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorML).Value = 0
            _results.Mirror_XCe1(eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorML).MinimumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorML).MinimumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorML).MaximumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorML).MaximumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorML).Value = 0
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_Xe(eMirrorPush.MirrorML).MinimumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorML).MinimumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorML).MaximumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorML).MaximumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorML).Value = 0
            _results.Mirror_Xe(eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Mirror_Strenght.Value) Then
                _results.ePUSH_MirrorML_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Fs1_F1(eMirrorPush.MirrorML).MinimumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorML).MinimumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorML).MaximumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorML).MaximumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorML).Value = 0
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Xs1(eMirrorPush.MirrorML).MinimumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorML).MinimumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorML).MaximumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorML).MaximumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorML).Value = 0
                _results.Mirror_Xs1(eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorML).MinimumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorML).MinimumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorML).MaximumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorML).MaximumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorML).Value = 0
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dXs1(eMirrorPush.MirrorML).MinimumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorML).MinimumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorML).MaximumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorML).MaximumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorML).Value = 0
                _results.Mirror_dXs1(eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown
            End If
            ' Init data for Electrical Test Off/Ce1
            For i = 0 To 1
                If i = 0 Then
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorML).MinimumLimit = 13.4
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorML).MaximumLimit = 13.6
                Else
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorML).MinimumLimit = 0
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorML).MaximumLimit = 0.2
                End If
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorML).Value = 0
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown
            Next
            _results.InitialState(eMirrorPush.MirrorML).MinimumLimit = 13.4
            _results.InitialState(eMirrorPush.MirrorML).MaximumLimit = 13.6
            _results.InitialState(eMirrorPush.MirrorML).TestResult = cResultValue.eValueTestResult.Unknown

            '**************************
            'Push Mirror SR
            '**************************
            _results.ePUSH_MirrorSR_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_XCe1(eMirrorPush.MirrorSR).MinimumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorSR).MinimumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorSR).MaximumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorSR).MaximumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorSR).Value = 0
            _results.Mirror_XCe1(eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorSR).MinimumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorSR).MinimumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorSR).MaximumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorSR).MaximumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorSR).Value = 0
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_Xe(eMirrorPush.MirrorSR).MinimumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorSR).MinimumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorSR).MaximumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorSR).MaximumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorSR).Value = 0
            _results.Mirror_Xe(eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Mirror_Strenght.Value) Then
                _results.ePUSH_MirrorSR_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Fs1_F1(eMirrorPush.MirrorSR).MinimumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorSR).MinimumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorSR).MaximumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorSR).MaximumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorSR).Value = 0
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Xs1(eMirrorPush.MirrorSR).MinimumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorSR).MinimumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorSR).MaximumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorSR).MaximumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorSR).Value = 0
                _results.Mirror_Xs1(eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSR).MinimumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSR).MinimumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSR).MaximumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSR).MaximumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSR).Value = 0
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dXs1(eMirrorPush.MirrorSR).MinimumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorSR).MinimumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorSR).MaximumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorSR).MaximumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorSR).Value = 0
                _results.Mirror_dXs1(eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown
            End If
            ' Init data for Electrical Test Off/Ce1
            For i = 0 To 1
                If i = 0 Then
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSR).MinimumLimit = 13.4
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSR).MaximumLimit = 13.6
                Else
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSR).MinimumLimit = 0
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSR).MaximumLimit = 0.2
                End If
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSR).Value = 0
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown
            Next
            _results.InitialState(eMirrorPush.MirrorSR).MinimumLimit = 13.4
            _results.InitialState(eMirrorPush.MirrorSR).MaximumLimit = 13.6
            _results.InitialState(eMirrorPush.MirrorSR).TestResult = cResultValue.eValueTestResult.Unknown

            '**************************
            'Push Mirror SL
            '**************************
            _results.ePUSH_MirrorSL_Electrical.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_XCe1(eMirrorPush.MirrorSL).MinimumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorSL).MinimumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorSL).MaximumLimit = _recipe.Mirror_XCe1(eMirrorPush.MirrorSL).MaximumLimit
            _results.Mirror_XCe1(eMirrorPush.MirrorSL).Value = 0
            _results.Mirror_XCe1(eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorSL).MinimumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorSL).MinimumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorSL).MaximumLimit = _recipe.Mirror_DiffS2Ce1(eMirrorPush.MirrorSL).MaximumLimit
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorSL).Value = 0
            _results.Mirror_DiffS2Ce1(eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown

            _results.Mirror_Xe(eMirrorPush.MirrorSL).MinimumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorSL).MinimumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorSL).MaximumLimit = _recipe.Mirror_Xe(eMirrorPush.MirrorSL).MaximumLimit
            _results.Mirror_Xe(eMirrorPush.MirrorSL).Value = 0
            _results.Mirror_Xe(eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown

            If CBool(_recipe.TestEnable_Mirror_Strenght.Value) Then
                _results.ePUSH_MirrorSL_Strenght.TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Fs1_F1(eMirrorPush.MirrorSL).MinimumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorSL).MinimumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorSL).MaximumLimit = _recipe.Mirror_Fs1_F1(eMirrorPush.MirrorSL).MaximumLimit
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorSL).Value = 0
                _results.Mirror_Fs1_F1(eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_Xs1(eMirrorPush.MirrorSL).MinimumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorSL).MinimumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorSL).MaximumLimit = _recipe.Mirror_Xs1(eMirrorPush.MirrorSL).MaximumLimit
                _results.Mirror_Xs1(eMirrorPush.MirrorSL).Value = 0
                _results.Mirror_Xs1(eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSL).MinimumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSL).MinimumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSL).MaximumLimit = _recipe.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSL).MaximumLimit
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSL).Value = 0
                _results.Mirror_dFs1_Haptic_1(eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown

                _results.Mirror_dXs1(eMirrorPush.MirrorSL).MinimumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorSL).MinimumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorSL).MaximumLimit = _recipe.Mirror_dXs1(eMirrorPush.MirrorSL).MaximumLimit
                _results.Mirror_dXs1(eMirrorPush.MirrorSL).Value = 0
                _results.Mirror_dXs1(eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown
            End If
            ' Init data for Electrical Test Off/Ce1
            For i = 0 To 1
                If i = 0 Then
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSL).MinimumLimit = 13.4
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSL).MaximumLimit = 13.6
                Else
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSL).MinimumLimit = 0
                    _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSL).MaximumLimit = 0.2
                End If
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSL).Value = 0
                _results.Mirror_Push_Electric(i, eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown
            Next
            _results.InitialState(eMirrorPush.MirrorSL).MinimumLimit = 13.4
            _results.InitialState(eMirrorPush.MirrorSL).MaximumLimit = 13.6
            _results.InitialState(eMirrorPush.MirrorSL).TestResult = cResultValue.eValueTestResult.Unknown

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
        _results.FinalState(8).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(9).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(10).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(11).TestResult = cResultValue.eValueTestResult.Disabled
        _results.FinalState(1).TestResult = cResultValue.eValueTestResult.Disabled

        _results.eWrite_MMSTestByte.TestResult = cResultValue.eValueTestResult.Unknown
        _results.eRead_MMSTestByte.TestResult = cResultValue.eValueTestResult.Unknown

        _results.MMS_Test_Byte_After.TestResult = cResultValue.eValueTestResult.Unknown

    End Sub


    Private Sub ProcessSamples_BaseTps(ByVal MirrorPush As eMirrorPush, ByRef MirrorResult As sMirrorResult)
        ' analogInputIndex: 0 = Early Sensor
        '                 : 1 = Strengh Sensor
        '                 : 2 = Electrical signal


        Dim sampleIndex As Integer
        Dim vth As Single = 6
        Dim n As Integer = 40 '20
        Dim df() As Single
        Dim Fe1 As Single
        Dim Pin_Analisys As Integer = 2 + MirrorPush
        Dim Recalage As Integer
        Dim RecalageCe As Integer
        ' Filter the samples
        Try
            If (_results.SampleCount(MirrorPush) > 1) Then
                ReDim df(0 To _results.SampleCount(MirrorPush) - 1)
            Else
                Exit Sub
            End If

            ' check if the minimum number of sample is ok
            If (_results.SampleCount(MirrorPush) > 100) Then
                _results.SampleCount(MirrorPush) = _results.SampleCount(MirrorPush) - 20  '100
            Else
                _results.SampleCount(MirrorPush) = 0
            End If

            ' Search the contact stroke
            MirrorResult.Xs = -1
            MirrorResult.Xe = -1
            MirrorResult.Xs_Index = 0
            MirrorResult.Xe_Index = 0
            MirrorResult.XCe1 = 0
            MirrorResult.XCe2 = 0
            'Find the Xs reference 0
            For sampleIndex = _results.SampleCount(MirrorPush) - 500 To 0 Step -1
                If (_results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, sampleIndex) <= 0.2) Then
                    MirrorResult.Xs = _results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, sampleIndex) * _recipe.Correlation_Factor_Stroke_A(MirrorPush).Value - _recipe.Correlation_Factor_Stroke_B(MirrorPush).Value
                    _results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, sampleIndex) = MirrorResult.Xs
                    MirrorResult.Xs_Index = sampleIndex
                    MirrorResult.Xe_Index = sampleIndex + 10
                    Exit For
                End If
            Next
            If MirrorResult.Xs = -1 Then
                MirrorResult.Xs = _results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, 1)
            End If
            ' Search the maximum stroke
            MirrorResult.Fe = 0 : MirrorResult.Xe = 0 : MirrorResult.Xe_Index = _results.SampleCount(MirrorPush)
            For sampleIndex = _results.SampleCount(MirrorPush) - 1 To _results.SampleCount(MirrorPush) / 2 Step -1
                If (_results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, sampleIndex) > MirrorResult.Fe) Then
                    MirrorResult.Fe = _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, sampleIndex)
                    MirrorResult.Xe = Abs(_results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, sampleIndex) * _recipe.Correlation_Factor_Stroke_A(MirrorPush).Value - MirrorResult.Xs)
                    MirrorResult.Xe_Index = sampleIndex
                End If
            Next

            If MirrorResult.Xe = -1 Then MirrorResult.Xe = MirrorResult.Xs + 1

            'Lab Correlation
            If _results.SampleCount(MirrorPush) > 1 Then
                For sampleIndex = MirrorResult.Xs_Index + 1 To _results.SampleCount(MirrorPush) - 1
                    _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, sampleIndex) =
                        _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, sampleIndex) *
                        _recipe.Correlation_Factor_Force_A(MirrorPush).Value + _recipe.Correlation_Factor_Force_B(MirrorPush).Value
                Next
                For sampleIndex = 0 To _results.SampleCount(MirrorPush)
                    _results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, sampleIndex) =
                        _results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, sampleIndex) *
                        _recipe.Correlation_Factor_Stroke_A(MirrorPush).Value
                Next
            End If

            AddLogEntry("Sample Index Xs : " & MirrorResult.Xs_Index)
            AddLogEntry("Early Xs : " & MirrorResult.Xs)
            AddLogEntry("Sample Index Xe : " & MirrorResult.Xe_Index)
            AddLogEntry("Early Xe : " & MirrorResult.Xe)

            If _results.SampleCount(MirrorPush) > 0 Then
                ' Search the commutation stroke
                MirrorResult.XCe1 = 0
                For sampleIndex = MirrorResult.Xs_Index To _results.SampleCount(MirrorPush) - 2
                    If (_results.Sample(Pin_Analisys, MirrorPush, sampleIndex) > vth AndAlso
                    _results.Sample(Pin_Analisys, MirrorPush, sampleIndex + 1) <= vth) Then
                        MirrorResult.XCe1 = Abs(_results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, sampleIndex) - MirrorResult.Xs)
                        MirrorResult.FsCe1 = Abs(_results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, sampleIndex) - MirrorResult.Xs)
                        MirrorResult.X_Ce1_index = sampleIndex + 1
                        Exit For
                    End If
                Next
                '
                ' Search the force peak and the delta stroke
                MirrorResult.Fs1_F1 = -1
                MirrorResult.Xs1 = 0
                MirrorResult.dFs1_Haptic_1 = -1
                MirrorResult.dXs1 = 0
                MirrorResult.Xs_F1_index = -1
                MirrorResult.Xs_F2_index = -1
                MirrorResult.X_Fe1_index = -1


                '******************************************************************
                Recalage = 0
                RecalageCe = 0
                ' Recherche sur courbe
LoopSnap1:      MirrorResult.Fs1_F1 = 0 : MirrorResult.Fs1_F11 = 100 : MirrorResult.Xs_F1_index = 0 : MirrorResult.Xs_F2_index = 0
                'Find Snap Ce1
                For i = MirrorResult.Xs_Index To MirrorResult.X_Ce1_index - Recalage + RecalageCe ' +0
                    If _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i) >= MirrorResult.Fs1_F1 Then
                        MirrorResult.Fs1_F1 = _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i)
                        MirrorResult.Xs1 = Abs(_results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, i) - MirrorResult.Xs)
                        MirrorResult.Fs1_F11 = MirrorResult.Fs1_F1
                        MirrorResult.Xs_F1_index = i
                        MirrorResult.Xs_F2_index = i
                    End If
                Next
                For i = MirrorResult.Xs_F1_index To MirrorResult.Xe_Index - 10
                    'define the F2
                    If _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i) < MirrorResult.Fs1_F11 AndAlso
                    _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i) <> 0 Then
                        MirrorResult.Fs1_F11 = _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i)
                        MirrorResult.Xs_F2_index = i
                        MirrorResult.dXs1 = Abs((_results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, MirrorResult.Xs_F2_index) - MirrorResult.Xs)) ' WIliResult.Xs1
                    End If
                    'Check position F2
                    If MirrorResult.Fs1_F11 > MirrorResult.Fs1_F1 OrElse (_results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i + 10) >
                                              MirrorResult.Fs1_F1 AndAlso _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i + 10) <> 0) Then
                        Exit For
                    End If
                Next

                If MirrorResult.Fs1_F1 = MirrorResult.Fs1_F11 Or MirrorResult.Fs1_F11 > MirrorResult.Fs1_F1 - 0.2 Then
                    Recalage = Recalage + 10
                    If Recalage < MirrorResult.X_Ce1_index Then
                        GoTo LoopSnap1
                    End If
                End If
                'if the F1 is after CE1
                If MirrorResult.Fs1_F1 = 0 And MirrorResult.Fs1_F11 = 100 Then
                    RecalageCe = RecalageCe + 10
                    If RecalageCe < MirrorResult.Xe_Index Then
                        GoTo LoopSnap1
                    End If
                End If
                ' calculate Snap CE1
                MirrorResult.dFs1_Haptic_1 = MirrorResult.Fs1_F11
                AddLogEntry("F1: " & MirrorResult.Fs1_F1.ToString())
                AddLogEntry("F2: " & MirrorResult.dFs1_Haptic_1.ToString())
                'If (WIliResult.dFs1_Haptic_1 = -1) And WIliResult.Fs1_F11 <> -1 Then
                '    WIliResult.dFs1_Haptic_1 = WIliResult.Fs1_F1 - WIliResult.Fs1_F11
                If (MirrorResult.Fs1_F1 > 0) Then
                    MirrorResult.dFs1_Haptic_1 = Format((MirrorResult.dFs1_Haptic_1 / MirrorResult.Fs1_F1) * 100, "#0.00")
                Else
                    MirrorResult.dFs1_Haptic_1 = 0
                End If
                AddLogEntry("F2/F1: " & MirrorResult.dFs1_Haptic_1.ToString())
                'End If
                MirrorResult.dXs1 = Abs((_results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, MirrorResult.Xs_F2_index) - MirrorResult.Xs)) ' WIliResult.Xs1
                MirrorResult.Value_DiffS2Ce1 = MirrorResult.dXs1 - MirrorResult.XCe1

                Fe1 = MirrorResult.Fs1_F11 + (1 * (MirrorResult.Fs1_F1 - MirrorResult.Fs1_F11))

                For i = MirrorResult.Xs_F2_index To MirrorResult.Xe_Index
                    If _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i) >= Fe1 AndAlso MirrorResult.X_Fe1_index = -1 Then
                        MirrorResult.X_Fe1_index = i
                    End If
                    If _results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, MirrorPush, i) >= 2 * MirrorResult.Fs1_F1 AndAlso MirrorResult.FsCe1 = -1 Then
                        MirrorResult.FsCe1 = i
                    End If
                Next
                If MirrorResult.FsCe1 = -1 Then
                    MirrorResult.FsCe1 = MirrorResult.Xe_Index
                End If
            End If
            'Check Data available
            If MirrorResult.Fs1_F1 < 0 Then MirrorResult.Fs1_F1 = 0
            If MirrorResult.dXs1 < 0 Then MirrorResult.dXs1 = 0
            If MirrorResult.Xs1 < 0 Then MirrorResult.Xs1 = 0
            If MirrorResult.dFs1_Haptic_1 < 0 Then MirrorResult.dFs1_Haptic_1 = 0
            If MirrorResult.Fe < 0 Then MirrorResult.Fe = 0
            If MirrorResult.Xe < 0 Then MirrorResult.Xe = 0
            If MirrorResult.Xs < 0 Then MirrorResult.Xs = 0

        Catch ex As Exception
            Dim err As String
            err = ex.ToString

            _log.Append("Error in Process Sample : " & err)

            'Check Data available
            If MirrorResult.Fs1_F1 < 0 Then MirrorResult.Fs1_F1 = 0
            If MirrorResult.dXs1 < 0 Then MirrorResult.dXs1 = 0
            If MirrorResult.Xs1 < 0 Then MirrorResult.Xs1 = 0
            If MirrorResult.dFs1_Haptic_1 < 0 Then MirrorResult.dFs1_Haptic_1 = 0
            If MirrorResult.Fe < 0 Then MirrorResult.Fe = 0
            If MirrorResult.Xs < 0 Then MirrorResult.Xs = 0
            If MirrorResult.Xe < 0 Then MirrorResult.Xe = 0

        End Try
    End Sub


    Private Sub FilterSamples_Global(ByVal analogInputIndex As Integer, _
                                        ByVal filterSize As Integer, _
                                        ByVal MirrorStep As eMirrorPush)
        For sampleIndex = 0 To _results.SampleCount(MirrorStep) - filterSize - 1
            For i = 1 To filterSize - 1
                _results.Sample(analogInputIndex, MirrorStep, sampleIndex) =
                    _results.Sample(analogInputIndex, MirrorStep, sampleIndex) +
                    _results.Sample(analogInputIndex, MirrorStep, sampleIndex + i)
            Next
            _results.Sample(analogInputIndex, MirrorStep, sampleIndex) = Math.Round(_results.Sample(analogInputIndex, MirrorStep, sampleIndex) / filterSize, 3)
        Next
        'Recalage Sample
        For sampleIndex = _results.SampleCount(MirrorStep) To filterSize / 2
            _results.Sample(analogInputIndex, MirrorStep, sampleIndex) = _results.Sample(analogInputIndex, MirrorStep, sampleIndex - (filterSize / 2))
        Next
        For sampleIndex = 0 To filterSize / 2
            _results.Sample(analogInputIndex, MirrorStep, sampleIndex) = _results.Sample(analogInputIndex, MirrorStep, (filterSize / 2) + 1)
        Next


    End Sub

    Private Function Write_AnalogPoint(ByVal MirrorTest As cWS05Results.eMirrorPushTest, ByVal index As Integer) As Boolean
        Dim fileWriter As StreamWriter = Nothing
        Dim sampleIndex As Integer
        Dim WriteLine As String
        Dim FileName As String

        Dim folder As String = "..\..\..\Point File Haptic\WS05\" + Now.ToString("yyMMdd") + "\"
        FileName = folder
        If MirrorTest = cWS05Results.eMirrorPushTest.Mirror_UP Then
            FileName = FileName & "Mirror UP"
        ElseIf MirrorTest = cWS05Results.eMirrorPushTest.Mirror_DN Then
            FileName = FileName & "Mirror DN"
        ElseIf MirrorTest = cWS05Results.eMirrorPushTest.Mirror_MR Then
            FileName = FileName & "Mirror MR"
        ElseIf MirrorTest = cWS05Results.eMirrorPushTest.Mirror_ML Then
            FileName = FileName & "Mirror ML"
        ElseIf MirrorTest = cWS05Results.eMirrorPushTest.Mirror_SR Then
            FileName = FileName & "Mirror SR"
        ElseIf MirrorTest = cWS05Results.eMirrorPushTest.Mirror_SL Then
            FileName = FileName & "Mirror SL"
        End If
        If index <> -1 Then
            ' Save the Analog Data information
            fileWriter = New StreamWriter(FileName & "_With Filter_" & _results.Valeo_Serial_Number.Value.ToString() & "_" & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & ".txt")

            ' Store the samples
            For sampleIndex = 0 To _results.SampleCount(MirrorTest) - 1
                WriteLine = (_results.Sample(mGlobal.eSignal_Analog.EarlySensor, MirrorTest, sampleIndex) & vbTab &
                             _results.Sample(mGlobal.eSignal_Analog.StrenghtSensor, MirrorTest, sampleIndex) & vbTab &
                             _results.Sample(cWS05Results.eMirrorPushTest.Mirror_UP + 2, MirrorTest, sampleIndex) & vbTab &
                             _results.Sample(cWS05Results.eMirrorPushTest.Mirror_DN + 2, MirrorTest, sampleIndex) & vbTab &
                             _results.Sample(cWS05Results.eMirrorPushTest.Mirror_MR + 2, MirrorTest, sampleIndex) & vbTab &
                             _results.Sample(cWS05Results.eMirrorPushTest.Mirror_ML + 2, MirrorTest, sampleIndex) & vbTab &
                             _results.Sample(cWS05Results.eMirrorPushTest.Mirror_SR + 2, MirrorTest, sampleIndex) & vbTab &
                             _results.Sample(cWS05Results.eMirrorPushTest.Mirror_SL + 2, MirrorTest, sampleIndex))
                fileWriter.WriteLine(WriteLine)
            Next

            fileWriter.Close()
        Else
            ' Save the Analog Data information
            '_results.Sample_TMP_AllKnob(MirrorTest) = _results.Sample.Clone()
            Dim temp(7, cWS05Results.MaxSampleCount) As Single
            _results.Sample_TMP_AllKnob(MirrorTest) = temp
            _results.Sample_TMP_AllKnobCount(MirrorTest) = _results.SampleCount(MirrorTest)

            FileName &= "_Without Filter_" & _results.Valeo_Serial_Number.Value.ToString()

            Dim para(4) As Object
            para(0) = _results.Sample
            para(1) = _results.SampleCount(MirrorTest)
            para(2) = FileName
            para(3) = folder
            para(4) = temp
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
                                                                      para(4)(eSignal_Analog.EarlySensor, sampleIndex) = para(0)(mGlobal.eSignal_Analog.EarlySensor, MirrorTest, sampleIndex)
                                                                      para(4)(eSignal_Analog.StrenghtSensor, sampleIndex) = para(0)(mGlobal.eSignal_Analog.StrenghtSensor, MirrorTest, sampleIndex)
                                                                      para(4)(cWS05Results.eMirrorPushTest.Mirror_UP + 2, sampleIndex) = para(0)(cWS05Results.eMirrorPushTest.Mirror_UP + 2, MirrorTest, sampleIndex)
                                                                      para(4)(cWS05Results.eMirrorPushTest.Mirror_DN + 2, sampleIndex) = para(0)(cWS05Results.eMirrorPushTest.Mirror_DN + 2, MirrorTest, sampleIndex)
                                                                      para(4)(cWS05Results.eMirrorPushTest.Mirror_MR + 2, sampleIndex) = para(0)(cWS05Results.eMirrorPushTest.Mirror_MR + 2, MirrorTest, sampleIndex)
                                                                      para(4)(cWS05Results.eMirrorPushTest.Mirror_ML + 2, sampleIndex) = para(0)(cWS05Results.eMirrorPushTest.Mirror_ML + 2, MirrorTest, sampleIndex)
                                                                      para(4)(cWS05Results.eMirrorPushTest.Mirror_SR + 2, sampleIndex) = para(0)(cWS05Results.eMirrorPushTest.Mirror_SR + 2, MirrorTest, sampleIndex)
                                                                      para(4)(cWS05Results.eMirrorPushTest.Mirror_SL + 2, sampleIndex) = para(0)(cWS05Results.eMirrorPushTest.Mirror_SL + 2, MirrorTest, sampleIndex)
                                                                      WriteLine = (para(0)(mGlobal.eSignal_Analog.EarlySensor, MirrorTest, sampleIndex) & vbTab &
                                                                                     para(0)(mGlobal.eSignal_Analog.StrenghtSensor, MirrorTest, sampleIndex) & vbTab &
                                                                                     para(0)(cWS05Results.eMirrorPushTest.Mirror_UP + 2, MirrorTest, sampleIndex) & vbTab &
                                                                                     para(0)(cWS05Results.eMirrorPushTest.Mirror_DN + 2, MirrorTest, sampleIndex) & vbTab &
                                                                                     para(0)(cWS05Results.eMirrorPushTest.Mirror_MR + 2, MirrorTest, sampleIndex) & vbTab &
                                                                                     para(0)(cWS05Results.eMirrorPushTest.Mirror_ML + 2, MirrorTest, sampleIndex) & vbTab &
                                                                                     para(0)(cWS05Results.eMirrorPushTest.Mirror_SR + 2, MirrorTest, sampleIndex) & vbTab &
                                                                                     para(0)(cWS05Results.eMirrorPushTest.Mirror_SL + 2, MirrorTest, sampleIndex) & vbTab &
                                                                                     _results.SampleTimeStamp(eSignal_Analog.EarlySensor, MirrorTest, sampleIndex))
                                                                      fileWriter.WriteLine(WriteLine)
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


    Private Function Read_AnalogPoint(ByVal FileName As String, ByVal MirrorStep As eMirrorPush) As Boolean
        Dim file As StreamReader = Nothing
        Dim sampleIndex As Integer
        Dim line As String
        Dim token(0 To 14) As String
        'Dim FileName As String
        Dim i As Integer
        file = New StreamReader(FileName)
        sampleIndex = 0
        ' While it is not reached the end of the file
        Dim temp(7, cWS05Results.MaxSampleCount) As Single
        _results.Sample_TMP_AllKnob(MirrorStep) = temp
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
                temp(eSignal_Analog.EarlySensor, sampleIndex) = _results.Sample(mGlobal.eSignal_Analog.EarlySensor, MirrorStep, sampleIndex)
                temp(eSignal_Analog.StrenghtSensor, sampleIndex) = _results.Sample(mGlobal.eSignal_Analog.StrenghtSensor, MirrorStep, sampleIndex)
                temp(cWS05Results.eMirrorPushTest.Mirror_UP + 2, sampleIndex) = _results.Sample(cWS05Results.eMirrorPushTest.Mirror_UP + 2, MirrorStep, sampleIndex)
                temp(cWS05Results.eMirrorPushTest.Mirror_DN + 2, sampleIndex) = _results.Sample(cWS05Results.eMirrorPushTest.Mirror_DN + 2, MirrorStep, sampleIndex)
                temp(cWS05Results.eMirrorPushTest.Mirror_MR + 2, sampleIndex) = _results.Sample(cWS05Results.eMirrorPushTest.Mirror_MR + 2, MirrorStep, sampleIndex)
                temp(cWS05Results.eMirrorPushTest.Mirror_ML + 2, sampleIndex) = _results.Sample(cWS05Results.eMirrorPushTest.Mirror_ML + 2, MirrorStep, sampleIndex)
                temp(cWS05Results.eMirrorPushTest.Mirror_SR + 2, sampleIndex) = _results.Sample(cWS05Results.eMirrorPushTest.Mirror_SR + 2, MirrorStep, sampleIndex)
                temp(cWS05Results.eMirrorPushTest.Mirror_SL + 2, sampleIndex) = _results.Sample(cWS05Results.eMirrorPushTest.Mirror_SL + 2, MirrorStep, sampleIndex)
                _results.Sample(mGlobal.eSignal_Analog.EarlySensor, MirrorStep, sampleIndex) = token(0)
                _results.Sample(mGlobal.eSignal_Analog.StrenghtSensor, MirrorStep, sampleIndex) = token(1)
                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_UP + 2, MirrorStep, sampleIndex) = token(2)
                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_DN + 2, MirrorStep, sampleIndex) = token(3)
                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_MR + 2, MirrorStep, sampleIndex) = token(4)
                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_ML + 2, MirrorStep, sampleIndex) = token(5)
                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_SR + 2, MirrorStep, sampleIndex) = token(6)
                _results.Sample(cWS05Results.eMirrorPushTest.Mirror_SL + 2, MirrorStep, sampleIndex) = token(7)
                sampleIndex = sampleIndex + 1
            End If

        Loop
        _results.SampleCount(MirrorStep) = sampleIndex
        _results.Sample_TMP_AllKnobCount(MirrorStep) = _results.SampleCount(MirrorStep)

        'Close the file
        If (file IsNot Nothing) Then
            file.Close()
            file = Nothing
        End If
        ' Return Status
        Read_AnalogPoint = False
    End Function

End Module
