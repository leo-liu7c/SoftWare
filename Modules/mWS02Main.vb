Option Explicit On
'Option Strict On

Imports System
Imports System.IO

Module mWS02Main
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
        KeyenceRecipeUnknown = 6

        Count = 7
    End Enum

    Public Enum eSdt_Signal
        O_DOWN_FRONT_PASSENGER_CDE
        O_UP_FRONT_PASSENGER_CDE
        O_UP_REAR_RIGHT_CDE
        O_DOWN_REAR_RIGHT_CDE
        O_UP_REAR_LEFT_CDE
        O_DOWN_REAR_LEFT_CDE
        O_JAMA_UP
        O_JAMA_DOWN
        COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR
        COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR
        CDE_DG_RTRV_D
        SGN_COMMUN_MOT_RTRV_D
        CDE_HB_RTRV_D
        CDE_P_RBT_RTRV_G
        CDE_M_RBT_RTRV_G
        CDE_P_RBT_RTRV_D
        CDE_M_RBT_RTRV_D
        O_LOCAL_WL_SWITCHES_INHIBITION_CDE
        CDE_HB_RTRV_G
        SGN_COMMUN_MOT_RTRV_G
        CDE_DG_RTRV_G

    End Enum

    Public Enum eON_OFF
        Fct_ON
        Fct_OFF
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

        ' FHM
        Power_UP = 10
        Init_LINCommunication = 11
        Open_DIAGonLINSession = 12
        Check_VALEOMMSSerialNumber = 13
        Write_Configuration = 14
        NormalModeCurrent = 15
        EMSTraceability = 16
        AnalogInput = 17
        DigitalOutput = 18
        PWMOutput_0 = 19
        PWMOutput_1 = 20
        SHAPE = 21
        BACKLIGHT = 22
        HOMOGENEITY = 23
        TELLTALE = 24
        Write_Temperature = 25
        Write_MMSTraceability = 26
        Write_MMSTestByte = 27
        ResetEcu = 28
        Read_MMSTraceability = 29
        Read_MMSTestByte = 30
        PowerDown = 31

        Count = 32
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
    Public Enum eADCValue
        WL_REAR_LEFT
        WL_REAR_RIGHT
        WL_REAR_FRONT_PASSENGER
        MIRROR_ADJUSTMENT
        BATTERY
        UCAD_VARIANT_2
        FOLD_CURRENT_LEFT
        FOLD_RIGHT_POS_X
        UCAD_VARIANT
        MIRROR_POS_Y
        MIRROR_SUPPLY
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
        Public Keyence_Adress As String
        Public Keyence_Port As String
        Public Keyence_CiePath As String
        Public LastPartNumberPath As String

    End Structure


    Public txData_MasterReq(0 To 7) As Byte
    Public btxRequest_MasterReq As Boolean

    Public _MasterPart As Boolean

    Public chUCDA_PWM(0 To 1, 0 To 1000) As Double
    Public chUCDA_Sample(0 To 1) As Integer

    Public RunningTimer As New cRunningTime("WS02")

    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    Private _PLC_Out() As UInt16
    Private _PLC_In() As UInt16
    ' Private constants
    Private Const _HardwareEnabled_PLC = mConstants.HardwareEnabled_PLC
    Private Const _HardwareEnabled_VIPA = mConstants.HardwareEnabled_VIPA
    Private Const _HardwareEnabled_NI = mConstants.HardwareEnabled_NI
    Private Const _HardwareEnabled_TTI = mConstants.HardwareEnabled_TTI
    Private Const _HardwareEnabled_Keyence = mConstants.HardwareEnabled_Keyence

    Private Const _inputDBReadingPeriod_ms = 200

    Private Const LINTimeout_ms = 1000
    Private Const LinRelance_ms = 100
    Private Const Timeout_ms = 1000
    Private Const TriggerDelay_ms = 350
    Private Const PowerUpDelay_ms = 150
    Private Const PowerUpTimeout_ms = 2000
    Private Const StandardSignalCount = 29
    Private Const SamplingFrequency = 5000

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
    Private _stepByStep As Boolean
    Private _testLast As Single
    Private _testMode As eTestMode
    Private _t0Test As Date
    Private _tPresent As Date
    Private _LinData_DIAG_MasterEquest(0 To 7) As Byte
    ' recipes and results
    Private _recipe As cWS02Recipe
    Private _results As cWS02Results
    Private _resultCamera As mWS02Keyence.sCameraTestResults
    Private _recipeMaster As cWS02Recipe

    Private _subPhaseMaint(0 To ePhase.Count) As Integer
    Private Param_1 As Integer
    Private Param_2 As Integer

    Private PhaseBefore_TP As ePhase
    Private PhasePrevious As ePhase

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


    Public ReadOnly Property Results As cWS02Results
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


    Public ReadOnly Property SubPhase(ByVal phaseIndex As ePhase) As Integer
        Get
            SubPhase = _subPhase(phaseIndex)
        End Get
    End Property

    Public ReadOnly Property StepInProgressInputStatus As Boolean
        Get
            StepInProgressInputStatus = mDIOManager.DigitalInputStatus(eDigitalInput.WS02_StepInProgress)
        End Get
    End Property

    Public ReadOnly Property TestEnableInputStatus As Boolean
        Get
            TestEnableInputStatus = mDIOManager.DigitalInputStatus(eDigitalInput.WS02_TestEnable)
        End Get
    End Property


    Public ReadOnly Property TestOkOutputStatus As Boolean
        Get
            TestOkOutputStatus = mDIOManager.DigitalOutputStatus(mDIOManager.eDigitalOutput.WS02_TestOk)
        End Get
    End Property

    Public ReadOnly Property StartStepOutputStatus As Boolean
        Get
            StartStepOutputStatus = mDIOManager.DigitalOutputStatus(mDIOManager.eDigitalOutput.WS02_StartStep)
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



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Sub ManageSubPhaseMaint(ByVal _SubPhaseName As Integer, _
                                  ByVal _Step As Integer, _
                                  ByVal _Param_1 As Integer, _
                                  ByVal _Param_2 As Integer)

        _subPhaseMaint(_SubPhaseName) = _Step

        Param_1 = _Param_1
        Param_2 = _Param_2

    End Sub

    Public Sub Raz_TP()
        _tPresent = Date.Now
    End Sub

    Public Sub StartPLC()
        mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.Test_Enable_PLC) = True
        mWS02Ethernet.WriteOutputDataBlock()
    End Sub

    Public Sub AbortTest()
        ' Set the abort test flag
        _abort = (_phase <> ePhase.WaitRecipeSelection And _
                  _phase <> ePhase.LoadRecipe And _
                  _phase <> ePhase.WaitStartTest And _
                  _phase <> ePhase.AbortTest)
    End Sub

    Public Sub WS02_Addlog(ByVal txtValue As String)
        AddLogEntry(txtValue)
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
            Case eAlarm.KeyenceRecipeUnknown
                AlarmDescription = "Keyence Camera Program unknown"
            Case Else
                AlarmDescription = String.Format("Value {0} unknown", alarmIndex)
        End Select
    End Function



    Public Sub ClearAlarms()
        ' Clear all the alarms
        For i = 0 To eAlarm.Count - 1
            _alarm(i) = False
        Next i
        '
        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Alarm)

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
            ' Read the Lin interface
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
            ' Read the Keyence Adress
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.Keyence_Adress = token(1)
            ' Read the Keyence Port
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.Keyence_Port = token(1)
            ' Read the Keyence Cie Path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.Keyence_CiePath = token(1)
            ' Read the Part Number File Path
            line = file.ReadLine
            token = Split(line, vbTab)
            _settings.LastPartNumberPath = token(1)


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
            Case ePhase.Init_LINCommunication
                PhaseDescription = "Init_LINCommunication"
            Case ePhase.Open_DIAGonLINSession
                PhaseDescription = "Open_DIAGonLINSession"
            Case ePhase.SHAPE
                PhaseDescription = "FHM Shape Pictogram"
            Case ePhase.BACKLIGHT
                PhaseDescription = "FHM Backlight"
            Case ePhase.HOMOGENEITY
                PhaseDescription = "FHM Homogeneity"
            Case ePhase.TELLTALE
                PhaseDescription = "FHM TellTale"
            Case ePhase.PowerDown
                PhaseDescription = "Power-down"
            Case ePhase.Write_MMSTraceability
                PhaseDescription = "Write MMS Traceability"
            Case ePhase.ResetEcu
                PhaseDescription = "Reset Ecu After Write Traceability"
            Case ePhase.Read_MMSTraceability
                PhaseDescription = "Read MMS Traceability"

            Case Else
                PhaseDescription = String.Format("Value {0} unknown", phase)
        End Select
    End Function



    Public Function PowerDown() As Boolean
        Dim e As Boolean
        Dim r As Boolean

        ' Power-down the digital I/O manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-down the Work  Station 02 digital I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS02DIOManager.PowerDown
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-down of the Work  Station 02 digital I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = e

        ' Power-down the analog I/O manager
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-down the Work  Station 02 analog I/O manager... ")
            If _HardwareEnabled_NI Then
                e = mWS02AIOManager.PowerDown
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-down of the Work  Station 02 analog I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = PowerDown Or e

        ' Load the Keyence Camera configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work  Station 02  Keyence ... ")
        Do
            If _HardwareEnabled_Keyence Then
                e = mWS02Keyence.Disconnect
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while the Work  Station 02  Keyence: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = PowerDown Or e

    End Function



    Public Function PowerUp() As Boolean
        Dim e As Boolean
        Dim r As Boolean
        ' Load the Work  Station 02 settings
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load Work  Station 02 settings... ")
        Do
            e = LoadSettings(mSettings.WS02SettingsPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work  Station 02 settings: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = e


        ' Power-up the analog I/O manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-up the Work  Station 02 analog I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS02AIOManager.PowerUp()
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-up of the Work  Station 02 analog I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Power-up the digital I/O manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-up the Work  Station 02 digital I/O manager... ")
        Do
            If _HardwareEnabled_NI Then
                e = mWS02DIOManager.PowerUp
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-up of the Work  Station 02 digital I/O manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the LIN frames
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load Work  Station 02 the LIN frames  ... ")
        Do
            e = CLINFrame.LoadArrayFromFile(_settings.LINFramesPath, _LINFrame)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading Work  Station 02 the LIN frames : retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the recipe configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work  Station 02 recipe FHM configuration... ")
        _recipe = New cWS02Recipe
        Do
            e = _recipe.LoadConfiguration(_settings.RecipeConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work  Station 02 recipe configuration: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the results configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work  Station 02 results configuration... ")
        _results = New cWS02Results

        Do
            e = _results.LoadConfiguration(_settings.ResultsConfigurationPath)

            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work  Station 02 results FHM configuration: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the PLC configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work  Station 02  PLC configuration... ")
        Do
            e = mWS02Ethernet.PowerUp(_settings.InputDBConfigurationPath, _settings.OutputDBConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the Work  Station 02  PLC configuration: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the Keyence Camera configuration
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load the Work  Station 02  Keyence ... ")
        Do
            If _HardwareEnabled_Keyence Then
                e = mWS02Keyence.Connect(_settings.Keyence_Adress, _settings.Keyence_Port)
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while the Work  Station 02  Keyence: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Load the Cie File
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load Cie Data... ")
        Do
            e = mColor_Analysis.LoadCieSettings(_settings.Keyence_CiePath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading Cie Data: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUp = PowerUp Or e

        ' Go to the phase WaitStartTest
        _phase = ePhase.WaitStartTest
    End Function



    Public Sub ResetCounters()
        ' Reset the counters
        _counterPassed = 0
        _counterFailed = 0
    End Sub



    Public Sub StartTest()
        ' If the phase is WaitStartTest
        If (_phase = ePhase.WaitStartTest) Then
            ' Set the start flag
            _start = True
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

            Next i
            _LinInterface.ClearLog()
        End If

        ' Diag Manage
        If (_LinInterface.Connected) And btxRequest_MasterReq = True Then
            btxRequest_MasterReq = False
            ' Transmit Frame
            e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMPTY), True, txData_MasterReq, 2, False, True)
        End If

        If _LinInterface.Connected Then
            Maint_OpenDiag()
            PhaselightMaint(Param_1, Param_2)
        Else
            _subPhaseMaint(0) = 999
            _subPhaseMaint(1) = 999
        End If

        _LinInterface.ClearRxBuffer()
    End Sub

    Private Function ADC_Value(ByVal ADC As String,
                                ByRef ADC_Return As String,
                                ByRef ADC_Name As eADCValue)
        Dim i As Integer
        Dim WL_REAR_LEFT As String
        Dim WL_REAR_RIGHT As String
        Dim WL_REAR_FRONT_PASSENGER As String
        Dim MIRROR_ADJUSTMENT As String
        Dim BATTERY As String
        Dim UCAD_VARIANT_2 As String
        Dim FOLD_CURRENT_LEFT As String
        Dim FOLD_RIGHT_POS_X As String
        Dim UCAD_VARIANT As String
        Dim MIRROR_POS_Y As String
        Dim MIRROR_SUPPLY As String

        i = 1
        WL_REAR_LEFT = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.WL_REAR_LEFT Then ADC_Return = WL_REAR_LEFT

        WL_REAR_RIGHT = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.WL_REAR_RIGHT Then ADC_Return = WL_REAR_RIGHT

        WL_REAR_FRONT_PASSENGER = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.WL_REAR_FRONT_PASSENGER Then ADC_Return = WL_REAR_FRONT_PASSENGER

        MIRROR_ADJUSTMENT = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.MIRROR_ADJUSTMENT Then ADC_Return = MIRROR_ADJUSTMENT

        BATTERY = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.BATTERY Then ADC_Return = BATTERY

        UCAD_VARIANT_2 = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.UCAD_VARIANT_2 Then ADC_Return = UCAD_VARIANT_2

        FOLD_CURRENT_LEFT = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.FOLD_CURRENT_LEFT Then ADC_Return = FOLD_CURRENT_LEFT

        FOLD_RIGHT_POS_X = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.FOLD_RIGHT_POS_X Then ADC_Return = FOLD_RIGHT_POS_X

        UCAD_VARIANT = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.UCAD_VARIANT Then ADC_Return = UCAD_VARIANT

        MIRROR_POS_Y = CStr(CInt("&h" & Mid(ADC, i, 4))) : i = i + 4
        If ADC_Name = eADCValue.MIRROR_POS_Y Then ADC_Return = MIRROR_POS_Y

        MIRROR_SUPPLY = CStr(CInt("&h" & Mid(ADC, i, 4)))
        If ADC_Name = eADCValue.MIRROR_SUPPLY Then ADC_Return = MIRROR_SUPPLY

        AddLogEntry("ADC WL_REAR_LEFT" & vbTab & vbTab & "= " & WL_REAR_LEFT)
        AddLogEntry("ADC WL_REAR_RIGHT " & vbTab & vbTab & "= " & WL_REAR_RIGHT)
        AddLogEntry("ADC WL_REAR_FRONT_PASSENGER" & " = " & WL_REAR_FRONT_PASSENGER)
        AddLogEntry("ADC MIRROR_ADJUSTMENT" & vbTab & vbTab & "= " & MIRROR_ADJUSTMENT)
        AddLogEntry("ADC BATTERY" & vbTab & vbTab & "= " & BATTERY)
        AddLogEntry("ADC UCAD_VARIANT_2" & vbTab & vbTab & "= " & UCAD_VARIANT_2)
        AddLogEntry("ADC FOLD_CURRENT_LEFT" & vbTab & vbTab & "= " & FOLD_CURRENT_LEFT)
        AddLogEntry("ADC FOLD_RIGHT_POS_X" & vbTab & vbTab & "= " & FOLD_RIGHT_POS_X)
        AddLogEntry("ADC UCAD_VARIANT" & vbTab & vbTab & "= " & UCAD_VARIANT)
        AddLogEntry("ADC MIRROR_POS_Y" & vbTab & vbTab & "= " & MIRROR_POS_Y)
        AddLogEntry("ADC MIRROR_SUPPLY" & vbTab & vbTab & "= " & MIRROR_SUPPLY)

    End Function
    Private Function AnalogStandard(ByVal sample() As Double, ByRef Std_Signal() As Double)
        Dim i As Integer
        i = 0
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin5) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin5))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin15) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin15))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin15) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin5) : i += 1
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin11) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin11))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin16) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin16))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin23) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin23))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_PIN24) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_PIN24))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin16) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin11) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_PIN24) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin23) : i += 1
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin1) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin1))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin13) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin13))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin1) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin13) : i += 1
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin14) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin14))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin2) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin2))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin14) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin2) : i += 1
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin9) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin9))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin7) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin7))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin8) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin8))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin9) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin7) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin8) : i += 1
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin17) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin17))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin18) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin18))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin18) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin17) : i += 1
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin19) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin19))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin20) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin20))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin19) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin20) : i += 1
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin12) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin12))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin12) : i += 1
        AddLogEntry("---------")
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin6) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin6))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin21) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin21))
        AddLogEntry(mWS02AIOManager.AnalogInputDescription(mWS02AIOManager.eAnalogInput.WS02_Pin10) & " = " & sample(mWS02AIOManager.eAnalogInput.WS02_Pin10))
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin6) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin21) : i += 1
        Std_Signal(i) = sample(mWS02AIOManager.eAnalogInput.WS02_Pin10) : i += 1


    End Function

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
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
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
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
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
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
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
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
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

    Private Sub PhaselightMaint(ByVal param1 As String, ByVal param2 As String)
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Dim r As Integer
        Dim f As CLINFrame
        Dim x As Single
        Dim y As Single
        Dim Yl As Single
        Dim Data(0 To 7) As Single
        Static previousSubphase As Integer
        Static t0 As Date
        Static t0Phase As Date
        Static tLin As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhaseMaint(1)
        ' Manage the subphases
        Select Case sp
            Case 0

            Case 1
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight),
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

            Case 2
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Backlight))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhaseMaint(1) = 100 '3
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhaseMaint(1) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    '
                    tLin = Date.Now
                End If


            Case 10
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TellTale),
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

            Case 11
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_TellTale))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhaseMaint(1) = 100 '3
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhaseMaint(1) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    '
                    tLin = Date.Now
                End If

            Case 40
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ALL_On),
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

            Case 41
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ALL_On))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhaseMaint(1) = 100 '3
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhaseMaint(1) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    '
                    tLin = Date.Now
                End If

            Case 50
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF),
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

            Case 51
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhaseMaint(1) = 100 '3
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhaseMaint(1) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    '
                    tLin = Date.Now
                End If

            Case 100

            Case 199

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
        Static t0RepeatTest As Date
        Dim retesttime As Int16 = -1  'if no need, set to -1
        Dim retestwaitseconds As Int16 = 15
        Static tCurrentRetest As Int16

        ' Receive the  LIN frames
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
            _testLast = Format((Date.Now - _t0Test).TotalSeconds, "0.00")
        End If

        f = (_testMode = eTestMode.Debug And _abort = True)
        f = f Or (_testMode = eTestMode.Local Or _testMode = eTestMode.Remote) And
            mDIOManager.DigitalInputStatus(eDigitalInput.WS02_TestEnable) = False
        f = f And Not (_phase = ePhase.WaitRecipeSelection Or
                       _phase = ePhase.LoadRecipe Or
                       _phase = ePhase.WaitStartTest Or
                       (_phase = ePhase.WriteResults And _subPhase(_phase) = 3) Or
                       _phase = ePhase.WaitEndTest)
        If (f = True) Then
            _abort = False
            _phase = ePhase.AbortTest
        End If
        'Console.WriteLine("_phase" & _phase.ToString & "_subPhase(_phase)" & _subPhase(_phase).ToString() & _LINFrame(224).ToString())
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

            Case ePhase.Power_UP
                PhasePowerUp()
            Case ePhase.Init_LINCommunication
                PhaseInit_LINCommunication()
            Case ePhase.Open_DIAGonLINSession
                PhaseOpen_DIAGonLINSession()
            Case ePhase.Check_VALEOMMSSerialNumber
                PhaseCheckSerialNunber()
            Case ePhase.Write_Configuration
                PhaseWRITE_Product_Configuration()
            Case ePhase.NormalModeCurrent
                PhaseNormalModeCurrent()
            Case ePhase.EMSTraceability
                PhaseREAD_EMSTraceability()
            Case ePhase.AnalogInput
                PhaseREAD_AnalogInput()
            Case ePhase.DigitalOutput
                PhaseDigitalOutput()
            Case ePhase.PWMOutput_0
                PhaseUCDA_PWM_Output(0)
            Case ePhase.PWMOutput_1
                PhaseUCDA_PWM_Output(1)
            Case ePhase.SHAPE
                PhaseShape()
            Case ePhase.BACKLIGHT
                PhaseBacklight()
            Case ePhase.HOMOGENEITY
                PhaseHomogeneity()
            Case ePhase.TELLTALE
                PhaseTellTale()
            Case ePhase.Write_Temperature
                PhaseWriteTemperatureSet()
            Case ePhase.Write_MMSTraceability
                PhaseWRITE_MMSTraceability()
            Case ePhase.Write_MMSTestByte
                PhaseWRITE_MMSTestByte()
            Case ePhase.ResetEcu
                PhaseReset_Ecu()
            Case ePhase.Read_MMSTraceability
                PhaseREAD_MMSTraceability()
            Case ePhase.Read_MMSTestByte
                PhaseREAD_MMSTestByte()

            Case ePhase.PowerDown
                PhasePowerDown()
                t0RepeatTest = Now
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
            If _maxScanTime > 50 Then Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + "WS02:_maxScanTime=" + _maxScanTime.ToString() + "Phase:" + _phase.ToString() + "SubPhase:" + _subPhase(_phase).ToString())
            If _maxScanTime > 100 Then AddLogEntry(DateTime.Now.ToString("HH:mm:ss fff") + "WS02:_maxScanTime=" + _maxScanTime.ToString() + "Phase:" + _phase.ToString() + "SubPhase:" + _subPhase(_phase).ToString())
        End If

        ' Clear the  LIN rx buffer
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
        ElseIf (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_LoadRecipeLocal) = False) Then   ' Otherwise, if the PLC load recipe local mode is cleared
            ' Go to the phase WaitStartTest
            _phase = ePhase.WaitStartTest
        End If
    End Sub



    Private Sub PhaseLoadRecipe()
        Dim e As Boolean
        Dim loadOk As Boolean
        Dim sp As Integer
        Dim r As Integer
        Dim d As Integer
        Dim i As Integer
        Static t0Phase As Date
        Static t0 As Date
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RecipeLoaded)
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RelaodRecipe)
                mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Alarm)
                ' Cancel the Space
                i = InStr(_reference, " ")
                If i > 0 Then
                    _reference = Mid(_reference, 1, i - 1)
                End If
                ' Add a log entry
                AddLogEntry("Start load recipe, test mode " & LCase(TestModeDescription(_testMode)))
                ' If the recipe is not valid
                If (_reference = "") Then
                    ' Add a log entry
                    AddLogEntry("Error: recipe not valid")
                    ' Raise an alarm for recipe not valid
                    _alarm(eAlarm.RecipeNotValid) = True
                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                ElseIf (cWS02Recipe.Exists(_reference)) Then
                    ' If the recipe loading fails
                    If (_recipe.Load(_reference)) Then
                        ' Add a log entry
                        AddLogEntry(String.Format("Error while loading the recipe ""{0}""", _reference))
                        ' Raise an alarm for error loading the recipe
                        _alarm(eAlarm.ErrorLoadingRecipe) = True
                        mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.Reference) = ""
                        ' Write the output data block
                        mWS02Ethernet.WriteOutputDataBlock()
                        ' Go to the phase WaitStartTest
                        _phase = ePhase.WaitStartTest
                    Else    ' Otherwise, if the recipe loading succeeds
                        ' Add a log entry
                        AddLogEntry(String.Format("Recipe ""{0}"" loading succeeded", _reference))
                        'GlobalRecipeSettings.SampleList = New ObjectModel.Collection(Of cSamples)()
                        'GlobalRecipeSettings.SampleList.Add(New cSamples())
                        'GlobalRecipeSettings.SampleList(0).Recipename = "E233502"
                        'GlobalRecipeSettings.SampleList(0).SampleType = EnumSampleType.Master
                        'GlobalRecipeSettings.SampleList(0).SN = "12345678"
                        'GlobalRecipeSettings.SampleList(0).NGCode = 0
                        'mSettings.SaveXml(mConstants.BasePath & "\Recipes\GlobalRecipeSettings.xml", GlobalRecipeSettings)
                        mSettings.LoadXML(mConstants.BasePath & "\Recipes\GlobalRecipeSettings.xml", GlobalRecipeSettings)
                        If (_HardwareEnabled_PLC) Then
                            ' Write the recipe data to the PLC
                            mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.Reference) = _reference
                            mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.CustomerPartNumber) = _recipe.Write_CustomerPartNumber.Value
                            mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.HW_Version) = _recipe.Write_HARDWARE_Version.Value
                            mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.LineNo) = mSettings.LineNumber
                            mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.IDCode) = _recipe.Laser_IDCode.Value
                        End If
                        ' Set the load ok flag
                        loadOk = True
                    End If
                Else    ' Otherwise, if the recipe does not exists
                    ' Add a log entry
                    AddLogEntry("Error: recipe unknown")
                    ' Raise an alarm for recipe not valid
                    _alarm(eAlarm.RecipeUnknown) = True
                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.Reference) = ""
                    ' Write the output data block
                    mWS02Ethernet.WriteOutputDataBlock()

                End If
                ' If the recipe loading succeeded
                If (loadOk) Then
                    If (_HardwareEnabled_PLC) Then
                        ' Load the camera program
                        e = e Or (mWS02Keyence.LoadProgram(_recipe.Camera_Programm.Value) <> 0)
                        t0 = Date.Now
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
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RecipeLoaded)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RelaodRecipe)
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Alarm)
                    ' Add a log entry
                    AddLogEntry("Error: recipe Load with error")
                    ' Raise an alarm for recipe not valid
                    _alarm(eAlarm.RecipeNotValid) = True
                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                End If

            Case 1
                ' If the Keyence controller 1 program loading ended
                r = mWS02Keyence.LoadProgramACK

                If (r = 0) Then
                    ' Adds a log entry
                    AddLogEntry("Keyence controller program loading OK")
                    ' Goes to the subphase 3
                    _subPhase(_phase) = 2
                    'Load Recipe not used
                    t0 = Date.Now
                    ' Otherwise, if the camera load program delay expired
                ElseIf ((r = 1 And (Date.Now - t0).TotalMilliseconds >= 30000)) Or r = 2 Then
                    ' Adds a log entry
                    AddLogEntry("Keyence controller program loading KO")
                    ' Set the recipe Alarm Bit
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RecipeLoaded)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RelaodRecipe)
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Alarm)
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.Reference) = ""
                    ' Write the output data block
                    mWS02Ethernet.WriteOutputDataBlock()

                    _alarm(eAlarm.KeyenceRecipeUnknown) = True
                    ' Goes to the subphase 3
                    _subPhase(_phase) = 0
                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                    t0 = Date.Now
                ElseIf (r = -1) Then
                    e = True
                End If

            Case 2
                '
                e = e Or (mWS02Keyence.LoadRecipe(_recipe.Camera_Recipe.Value) <> 0)
                '
                t0 = Date.Now
                ' Go to subphase 1
                _subPhase(_phase) = 3

            Case 3
                ' If the Keyence controller 1 program loading ended
                r = mWS02Keyence.LoadRecipeACK

                If (r = 0) Then
                    ' Adds a log entry
                    AddLogEntry("Keyence controller recipe loading OK")
                    ' Goes to the subphase 4
                    _subPhase(_phase) = 4
                    t0 = Date.Now
                    ' Otherwise, if the camera load program delay expired
                ElseIf ((r = 1 And (Date.Now - t0).TotalMilliseconds >= 30000)) Or r = 2 Then
                    ' Adds a log entry
                    AddLogEntry("Keyence controller recipe loading KO")
                    ' Set the recipe Alarm Bit
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RecipeLoaded)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RelaodRecipe)
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Alarm)
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.Reference) = ""
                    ' Write the output data block
                    mWS02Ethernet.WriteOutputDataBlock()

                    _alarm(eAlarm.KeyenceRecipeUnknown) = True
                    ' Goes to the subphase 3
                    _subPhase(_phase) = 0
                    ' Go to the phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                    t0 = Date.Now
                ElseIf (r = -1) Then
                    e = True
                End If

            Case 4
                'wait program Camera is loaded
                If (Date.Now - t0).TotalMilliseconds >= 150 Then
                    'Read Ethernet
                    mWS02Ethernet.ReadInputDataBlock()
                    If (_reference <> mWS02Ethernet.InputValue(mWS02Ethernet.eInput.Reference)) Then
                        ' Add a log entry
                        AddLogEntry("PC and PLC Program do not match in phase LoadRecipe, subphase " & sp)
                        ' Raise an alarm for runtime error
                        _alarm(eAlarm.ErrorLoadingRecipe) = True
                        ' Clear the subphase
                        _subPhase(_phase) = 0
                        ' Go to phase WaitStartTest
                        _phase = ePhase.WaitStartTest
                        Exit Sub
                    End If
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.Reference) = _reference
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.CustomerPartNumber) = _recipe.Write_CustomerPartNumber.Value
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.HW_Version) = _recipe.Write_HARDWARE_Version.Value
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.LineNo) = mSettings.LineNumber
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.IDCode) = _recipe.Laser_IDCode.Value

                    ' Write the output data block
                    mWS02Ethernet.WriteOutputDataBlock()
                    ' Set the recipe loaded bit
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RecipeLoaded)
                    ' Add a log entry
                    AddLogEntry("PC_RecipeLoaded = 1")
                    ' Goes to the subphase 3
                    _subPhase(_phase) = 5
                    t0 = Date.Now
                    Try
                        mGlobal.FormDUTStats.Visible = False
                        mGlobal.FormDUTStats.Show()
                    Catch ex As Exception
                        Console.WriteLine(ex.ToString)
                    End Try
                End If

            Case 5
                ' If the load recipe bit was cleared by the PLC
                If (_testMode = eTestMode.Debug Or
                            (_testMode = eTestMode.Local And mDIOManager.DigitalInputStatus(eDigitalInput.WS02_LoadRecipeLocal) = False) Or
                            (_testMode = eTestMode.Remote And mDIOManager.DigitalInputStatus(eDigitalInput.WS02_LoadRecipeRemote) = False)) Then
                    ' Add a log entry
                    AddLogEntry("End load recipe - Phase last " & (Date.Now - t0Phase).TotalSeconds.ToString("0.00"))
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RecipeLoaded)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RelaodRecipe)
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Alarm)
                    ' Clear the subphase
                    _subPhase(_phase) = 0
                    ' Go to phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                End If
        End Select

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase LoadRecipe, subphase " & sp)
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
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
                    If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_LoadRecipeRemote)) Then
                        'Read Ethernet
                        mWS02Ethernet.ReadInputDataBlock()
                        ' Set the test mode to remote
                        _testMode = eTestMode.Remote
                        ' Set the reference and the reference type
                        _reference = mWS02Ethernet.InputValue(mWS02Ethernet.eInput.Reference)
                        ' Go to the phase LoadRecipe
                        _phase = ePhase.LoadRecipe
                        Exit Sub
                        ' Otherwise, if the PLC load recipe local mode is set
                    ElseIf (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_LoadRecipeLocal)) Then
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
                        (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_TestEnable) Or _start)) Then
                        ' If the start flag is active
                        If (_start = True) Then
                            ' Clear the start flag
                            _start = False
                            ' Set the test mode to debug
                            _testMode = eTestMode.Debug
                        Else
                            ' Clear the start flag
                            _start = False
                            ' Set the test mode to debug
                            _testMode = eTestMode.Remote
                        End If
                        ' Clear the log
                        _log.Clear()
                        ' Add a log entry
                        AddLogEntry("Start test, test mode " & LCase(TestModeDescription(_testMode)))
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
            ' Update the global test result
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub



    Private Sub PhaseClearResults()
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Static t0Phase As Date
        Static t0 As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                '
                _results.Side_Barcode.Value = String.Empty
                mWS02Ethernet.ReadInputDataBlock()
                _results.Side_Barcode.Value = mWS02Ethernet.InputValue(mWS02Ethernet.eInput.Side_Barcode).ToString()
                AddLogEntry("Side Barcode: " & _results.Side_Barcode.Value)

                ' Store the phase initial time
                t0Phase = Date.Now
                If (_HardwareEnabled_PLC) Then
                    ' Write the output DB
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.ResultCode) = cWS02Results.eTestResult.Unknown
                    '
                    mWS02Ethernet.WriteOutputDataBlock()
                End If
                ' Add a log entry
                AddLogEntry("Begin clear results")
                ' Clear results
                ClearResults()
                ' reset Keyence camera
                e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Keyence_Reset)
                ' Clear the OK digital outputs
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_TestOk)
                AddLogEntry("Reset Keyence")
                ' store timer
                t0 = Date.Now
                ' Go to the subphase 1
                _subPhase(_phase) = 1

            Case 1
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_Busy)) Or ((Date.Now - t0).TotalMilliseconds > 100) Then
                    ' clear the reset Keyence camera
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Keyence_Reset)
                    ' Go to the subphase 1
                    _subPhase(_phase) = 2
                End If

            Case 2
                If ((mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_RUN) And
                    mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_TrigReady) And
                    Not mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_Busy))) Then
                    ' Go to the subphase 1
                    _subPhase(_phase) = 3
                End If

            Case 3
                AddLogEntry("Set Start Step")
                ' If the step in progress is false or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_StepInProgress) = False Or _testMode = eTestMode.Debug) Then
                    ' Sets the start step output
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_StartStep)
                    ' Go to the subphase 2
                    _subPhase(_phase) = 4
                End If

            Case 4
                ' If the step in progress is set or the test mode is debug

                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_StepInProgress) = True Or _testMode = eTestMode.Debug) Then
                    AddLogEntry("Step in progress at 1")
                    ' Clear the start step output
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_StartStep)
                    ' Go to the subphase 3
                    _subPhase(_phase) = 5
                End If

            Case 5
                ' If the step in progress is cleared or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_StepInProgress) = False Or _testMode = eTestMode.Debug) Then
                    AddLogEntry("step in progress at 0")
                    AddLogEntry(String.Format("End clear results - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                End If
                'Clear Sub Phase
                _subPhase(_phase) = 0
                'set Next Phase
                _phase = ePhase.Power_UP

        End Select


        ' If a runtime error occured
        If (e) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go Abort test
            _phase = ePhase.AbortTest
        End If
    End Sub

    Private Sub PhaseWriteResults()
        Dim e As Boolean
        Dim filename As String
        Dim sp As Integer
        Dim Wili_TMP As Boolean = True  ' for force part Type 
        Static t0 As Date
        Static t0Phase As Date

        Select Case _subPhase(_phase)
            Case 0
                ' Create the saving directory
                If Not (Directory.Exists(mConstants.BasePath & _settings.ResultsPath & "\" & Format(Date.Now, "yyyyMMdd"))) Then
                    Directory.CreateDirectory(mConstants.BasePath & _settings.ResultsPath & "\" & Format(Date.Now, "yyyyMMdd"))
                End If
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin write results")
                ' If the part type is the good part type 
                If (_results.PartTypeNumber.Value = 20) Then
                    ' Check the maste reference
                    If Not (_forcePartTypeOk) Then
                        If ((_results.TestResult = cWS02Results.eTestResult.Unknown) Or _forcePartOk) And Not Wili_TMP Then
                            ' Check the master reference
                            If CheckMasterReference() = True Then
                                _results.TestResult = cWS02Results.eTestResult.FailedMasterReference
                                For i = 0 To cWS02Results.SingleTestCount - 1
                                    If mWS02Main.Results.Value(cWS02Results.SingleTestBaseIndex + i).TestResult <> cResultValue.eValueTestResult.Disabled Then
                                        mWS02Main.Results.Value(cWS02Results.SingleTestBaseIndex + i).TestResult = cResultValue.eValueTestResult.NotCoherent
                                    End If
                                Next
                            End If
                        End If
                    Else
                        _results.TestResult = cWS02Results.eTestResult.Passed
                    End If
                    ' TMP WIL Force result Part Type
                ElseIf (_results.PartTypeNumber.Value > 20) And Wili_TMP Then
                    '_results.TestResult = cWS02Results.eTestResult.Passed
                End If

                ' If the global test result is unknown or passed or forced ok
                If (_results.TestResult = cWS02Results.eTestResult.Unknown Or
                    _results.TestResult = cWS02Results.eTestResult.Passed Or
                    (_results.TestResult <> cWS02Results.eTestResult.NotTested And
                        (_forcePartOk And _results.PartTypeNumber.Value <> 20))) Then
                    ' Set the global test result to passed
                    _results.TestResult = cWS02Results.eTestResult.Passed
                    ' Set the OK output
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_TestOk)
                    ' Increase the counter of passed parts
                    _counterPassed = _counterPassed + 1
                Else
                    ' Increase the counter of failed parts
                    _counterFailed = _counterFailed + 1
                End If
                If (_HardwareEnabled_PLC) Then
                    '' Write the result code to the PLC
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.ResultCode) = _results.TestResult
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.UniqueNumber) = _results.PartUniqueNumber.Value
                    If _results.Major_SoftwareVersion.TestResult <> cResultValue.eValueTestResult.Unknown Then
                        mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.SW_Version) = _results.Major_SoftwareVersion.Value & "." &
                                                                                    _results.Minor_SoftwareVersion.Value
                    End If
                    If _results.Major_NVMversion.TestResult <> cResultValue.eValueTestResult.Unknown Then
                        mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.NVM_Version) = _results.Major_NVMversion.Value & "." &
                                                                                    _results.Minor_NVMversion.Value
                    End If
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.HW_Version) = _recipe.Write_HARDWARE_Version.Value
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.LineNo) = mSettings.LineNumber ' _recipe.Write_MMS_Valeo_Final_Product_Line.Value
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.CustomerPartNumber) = _recipe.Write_CustomerPartNumber.Value
                    mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.IDCode) = _recipe.Laser_IDCode.Value
                    ' Write the ethernet output DB
                    e = e Or mWS02Ethernet.WriteOutputDataBlock
                End If
                mGlobal.FormDUTStats.Addnewitem(_results.PartUniqueNumber.Value, _results.TestTime.Value, 2, _results.TestResult, _results.RecipeName.Value)
                ' Save the results
                filename = mConstants.BasePath &
                                        _settings.ResultsPath &
                                        "\" & Format(Date.Now, "yyyyMMdd") &
                                        "\WS02" & _results.RecipeName.Value
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

                e = _results.Save(filename)
                ' Append the log to the results
                My.Computer.FileSystem.WriteAllText(filename,
                                                    vbCrLf &
                                                    "---------------------------------------------------------------------------------------------------------" &
                                                    vbCrLf &
                                                    "-                                  Log Frame LIN                                                   -" &
                                                    vbCrLf &
                                                    "----------------------------------------------------------------------------------------------------------" &
                                                    vbCrLf &
                                                    _log.ToString,
                                                    True)
                My.Computer.FileSystem.CopyFile(filename, mResults.FTPLocalFilePath + filename.Replace(mConstants.BasePath & _settings.ResultsPath & "\", ""), True)

                ''disable the force result
                '_forcePartOk = False
                '_forcePartTypeOk = False
                mUserManager.Logout()
                ' Store the time
                t0 = Date.Now
                ' Go to subphase 1
                _subPhase(_phase) = 1

            Case 1
                ' After a 20 ms delay
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    ' Set the start step output
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_StartStep)
                    ' Go to subphase 2
                    _subPhase(_phase) = 2
                End If

            Case 2
                ' If the step in progress is set or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_StepInProgress) = True Or _testMode = eTestMode.Debug) Then
                    ' Reset the start step output
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_StartStep)
                    ' Go to subphase 3
                    _subPhase(_phase) = 3
                End If

            Case 3
                ' If the step in progress is cleared or the test mode is debug
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_StepInProgress) = False Or _testMode = eTestMode.Debug) Then
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
            ' Update the global test result
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub


    Private Sub PhaseWaitEndTest()
        Dim e As Boolean
        Dim sp As Integer
        Static t0Phase As Date


        ' Manage the subphases
        Select Case _subPhase(_phase)
            Case 0
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin wait end test")
                ' Go to subphase 1
                _subPhase(_phase) = 1
                mWS02Ethernet.OutputValue(mWS02Ethernet.eOutput.Test_Enable_PLC) = False
                mWS02Ethernet.WriteOutputDataBlock()

            Case 1
                ' If the test enable inputs are cleared
                If (mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS02_TestEnable) = False) Then
                    ' Add a log entry
                    AddLogEntry("End wait end test - Phase last: " & (Date.Now - t0Phase).TotalSeconds.ToString("0.00") & " s" & vbCrLf)
                    ' Go to phase WaitStartTest
                    _phase = ePhase.WaitStartTest
                    Console.WriteLine("WS02 Finished Test!")
                    '
                End If
        End Select

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase WaitEndTest, subphase " & sp)
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If
    End Sub



    Private Sub PhaseAbortTest()
        Dim e As Boolean
        Dim sp As Integer
        Static t0Phase As Date
        Dim filename As String
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
                For j = 0 To mWS02DIOManager.eDigitalOutput.Count - 1
                    e = e Or mWS02DIOManager.ResetDigitalOutput(j)
                Next
                e = mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_LocalSensing)
                e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_RemoteSensing)
                ' Reset all the digital outputs toward the PLC
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_TestOk)
                e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_StartStep)
                ' Power-down the LIN interface
                e = e Or _LinInterface.PowerDown()
                ' Set the test result to failed for abort test
                _results.TestResult = cWS02Results.eTestResult.FailedAbortTest
                ' Add a log entry
                AddLogEntry("End abort test - Phase last: " & (Date.Now - t0Phase).TotalSeconds.ToString("0.00") & " s" & vbCrLf)
                ' Create the saving directory
                If Not (Directory.Exists(mConstants.BasePath & _settings.ResultsPath & "\" & Format(Date.Now, "yyyyMMdd"))) Then
                    Directory.CreateDirectory(mConstants.BasePath & _settings.ResultsPath & "\" & Format(Date.Now, "yyyyMMdd"))
                End If

                ' Save the results
                filename = mConstants.BasePath &
                            _settings.ResultsPath &
                            "\" & Format(Date.Now, "yyyyMMdd") &
                            "\WS02" & _results.RecipeName.Value
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
                e = e Or _results.Save(filename)
                ' Append the log to the results
                My.Computer.FileSystem.WriteAllText(filename,
                                                    vbCrLf &
                                                    "---------------------------------------------------------------------------------------------------------" &
                                                    vbCrLf &
                                                    "-                                  Log Frame LIN                                                   -" &
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
            ' Update the global test result
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
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
        Dim i As Integer
        Dim sample(0 To mWS02AIOManager.eAnalogInput.Count - 1) As Double
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
                AddLogEntry("Begin power-up")
                ' Connect the power supply
                e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_OnPowerSupply1)
                e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_OnPowerSupply2)
                e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_PowerR)
                ' Connect the remote sensing
                e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_RemoteSensing)
                ' Disconnect the local sensing
                e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_LocalSensing)
                'Connect Lin Bus
                e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_LinConnect)
                'Connect the Pin Function Retro Left
                e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_6)
                e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_10)
                e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_C_Pin_6_10)
                'Memo Function
                If _recipe.TestEnable_MEMO_Option.Value Then
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_9)
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_C_Pin_9_8)
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_8)

                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_2)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_C_Pin_8_2)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_8_2)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_14)
                Else
                    ' No Memo Function
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_2)
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_C_Pin_8_2)
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_8_2)
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_14)

                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_9)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_C_Pin_9_8)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_8)
                End If
                If _recipe.TestEnable_PWL_Option.Value And Not _recipe.TestEnable_EV_Option.Value Then
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_12_GND)
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_24_GND)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_PWL)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_5)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_11)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_12)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_15)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_16)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_23)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_24)
                End If
                If _recipe.TestEnable_EV_Option.Value And Not _recipe.TestEnable_PWL_Option.Value Then
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_12)
                    e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_24)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_PWL)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_12_GND)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_24_GND)
                End If
                If _recipe.TestEnable_FOLDING_Option.Value Then
                    'Connect CDE Rabat Retro L/R
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_17)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_18)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_19)
                    e = e Or mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_R_Pin_20)
                End If
                ' Store the time
                t0 = Date.Now
                ' Go to subphase
                _subPhase(_phase) = 1

            Case 1
                ' Sample the analog inputs
                e = e Or mWS02AIOManager.SingleSample(sample)
                ' If the voltage is OK or the test delay 1 expired
                If (((sample(mWS02AIOManager.eAnalogInput.WS02_VBAT) >= _recipe.PowersupplyVbat.MinimumLimit And
                    sample(mWS02AIOManager.eAnalogInput.WS02_VBAT) <= _recipe.PowersupplyVbat.MaximumLimit) And
                    (Date.Now - t0).TotalMilliseconds >= PowerUpDelay_ms)) Or
                    (Date.Now - t0).TotalMilliseconds >= 2000 Then
                    _results.PowerUp_VBAT.Value = sample(mWS02AIOManager.eAnalogInput.WS02_VBAT)
                    For i = 0 To 31
                        AddLogEntry(mWS02AIOManager.AnalogInputDescription(i) & " : " & sample(i))
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 100
                End If

            Case 100
                ' Tests
                If (_results.PowerUp_VBAT.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Failed
                Else
                    _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Passed
                End If

                ' Update the test result
                If _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.ePOWER_UP.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) &
                        " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                            _results.ePOWER_UP.TestResult <>
                                cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPowerUP
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Init_LINCommunication

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

    Private Sub PhaseInit_LINCommunication()
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
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Initialization  Lin Communication ")
                ' Store the time
                t0 = Date.Now
                ' Go to subphase
                _subPhase(_phase) = 1

            Case 1
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
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 100
                ' Update the test result
                If _results.eINIT_LIN_COMMUNICATION.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eINIT_LIN_COMMUNICATION.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eINIT_LIN_COMMUNICATION.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedInitROOFLINCommunication
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.PowerDown
                Else
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.Open_DIAGonLINSession

                End If

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eINIT_LIN_COMMUNICATION.TestResult =
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
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
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
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
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
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
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
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
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
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
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
                    ' Switch-off the backlight EV part2
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF2_EV),
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
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedOpenDIAGonLINSession
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.PowerDown
                Else
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.Check_VALEOMMSSerialNumber
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

    Private Sub PhaseCheckSerialNunber()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
        Static t0 As Date
        Static tLin As Date
        Static t0Phase As Date
        Static s As String
        Static frameIndex As Integer

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
                            CurrentSampleType = EnumSampleType.Production
                            'Add by YAN.Qian.20200401, for sample Type. in case of overwrite something for Master samples.
                            If (GlobalRecipeSettings.SampleList IsNot Nothing AndAlso GlobalRecipeSettings.SampleList.Count > 0) Then
                                For Each item In GlobalRecipeSettings.SampleList
                                    If (item.Recipename = _reference AndAlso item.SN = Mid(s, 25, 6) & Mid(s, 35, 4)) Then
                                        CurrentSampleType = item.SampleType
                                        AddLogEntry("Begin Sample Type: " + CurrentSampleType.ToString() + ",Sample SN: " + item.SN)
                                        If (CurrentSampleType <> EnumSampleType.Production) Then
                                            _results.eRead_MMS_Serial_Number.TestResult = cResultValue.eValueTestResult.Disabled
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                            'Add by YAN.Qian.20200401, for sample Type. in case of overwrite something for Master samples.
                            '
                            _subPhase(_phase) = 100
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        ' Set the flag of CAN timeout
                        _subPhase(_phase) = 199
                    End If
                Loop Until (i = -1)

            Case 100
                ' Tests
                If (_results.Valeo_Serial_Number.Test <> cResultValue.eValueTestResult.Passed) Then
                    _results.eRead_MMS_Serial_Number.TestResult = cResultValue.eValueTestResult.Failed
                    _results.PartUniqueNumber.Value = _results.Valeo_Serial_Number.Value
                Else
                    _results.eRead_MMS_Serial_Number.TestResult = cResultValue.eValueTestResult.Passed
                End If
                _results.eRead_MMS_Serial_Number.TestResult = cResultValue.eValueTestResult.Passed
                ' Go to next subphase
                _subPhase(_phase) = 101


            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eRead_MMS_Serial_Number.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedCheckSerialNumber
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Write_Configuration

            Case 199
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eRead_MMS_Serial_Number.TestResult =
                    cResultValue.eValueTestResult.Failed

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

    Private Sub PhaseWRITE_Product_Configuration()
        Dim e As Boolean
        Dim f As CLINFrame
        Dim i As Integer
        Dim sp As Integer

        Static NRC_78 As Boolean
        Static Relance As Integer
        Static t0 As Date
        Static tLin As Date
        Static t0Phase As Date
        Static s As String
        Static frameIndex As Integer
        Static LinTimeOut As Boolean
        Static PreviousPhase As Integer

        Static backLightPWM As Single
        Static telltaleMirrorLeft As Single
        Static telltaleMirrorRight As Single
        Static telltaleChildrenLock As Single
        Static mirrorposition1 As String
        Static mirrorposition2 As String
        Static mirrorposition3 As String
        Static mirrorposition4 As String

        Static memoposition1 As String
        Static memoposition2 As String
        Static memoposition3 As String
        Static memoposition4 As String
        Static memouserposition As Integer

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                If _recipe.TestEnable_WriteConfiguration.Value And Not _recipe.TestEnable_EV_Option.Value And CurrentSampleType = EnumSampleType.Production Then
                    NRC_78 = False
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Write Product Configuration")
                    '                    
                    Relance = 0
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1
                ElseIf _recipe.TestEnable_WriteConfiguration.Value And _recipe.TestEnable_EV_Option.Value And CurrentSampleType = EnumSampleType.Production Then
                    NRC_78 = False
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Write Product Configuration")
                    '                    
                    Relance = 0
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 20
                Else
                    If (CurrentSampleType <> EnumSampleType.Production) Then
                        _results.eWriteConfiguration.TestResult = cResultValue.eValueTestResult.Disabled
                    End If
                    ' Add a log entry
                    AddLogEntry("Write Confighuration  is desabled")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _phase = ePhase.NormalModeCurrent

                End If

            Case 1
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_SW_Coding).DeepClone
                f.Data(5) = _recipe.WRITE_SW_Coding.Value
                f.Data(6) = _recipe.WRITE_SW_Coding.Value
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_SW_Coding))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) = 10
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    If Relance >= 1 Or NRC_78 Then
                        PreviousPhase = 10
                        LinTimeOut = True
                        'Go to subphase
                        _subPhase(_phase) = 199
                    Else
                        Relance += 1
                        ' Go to subphase
                        _subPhase(_phase) = 1
                    End If
                End If

                ' Manage the NRC 78 Pending
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "XX" : f.Data(1) = "03" : f.Data(2) = "7F" : f.Data(3) = "2E"
                f.Data(4) = "XX" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    If _LinInterface.RxFrame(i).Data(4) = "78" Then
                        AddLogEntry("NRC 78 , Pending Request")
                        NRC_78 = True
                    Else
                        LinTimeOut = True
                        PreviousPhase = 10
                        ' Go to subphase
                        _subPhase(_phase) = 199
                    End If
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                End If

            Case 10
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_Backlight_Coding).DeepClone
                f.Data(5) = _recipe.WRITE_Baclight_Coding.Value
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 11

            Case 11
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_Backlight_Coding))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    '_subPhase(_phase) = 30 ' 100, YAN.Qian20200401. Case 30-41 temperatally added for FCV Square SUV delivery. requested by RD PTM.
                    _subPhase(_phase) = 14 ' 30,YAN.Qian20200713. Case 14-15 read yellow bin first. if yellow bin = 2, then write different table.
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    If Relance >= 1 Or NRC_78 Then
                        PreviousPhase = 100
                        LinTimeOut = True
                        'Go to subphase
                        _subPhase(_phase) = 199
                    Else
                        Relance += 1
                        ' Go to subphase
                        _subPhase(_phase) = 10
                    End If
                End If

                ' Manage the NRC 78 Pending
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "XX" : f.Data(1) = "03" : f.Data(2) = "7F" : f.Data(3) = "2E"
                f.Data(4) = "XX" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    If _LinInterface.RxFrame(i).Data(4) = "78" Then
                        AddLogEntry("NRC 78 , Pending Request")
                        NRC_78 = True
                    Else
                        LinTimeOut = True
                        PreviousPhase = 100
                        ' Go to subphase
                        _subPhase(_phase) = 199
                    End If
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                End If
            Case 14
                AddLogEntry("Request Read EMS LED Rank,DIAG_Req_EMS_LED_Yellow, before write pwm table.")

                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_Yellow),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 15
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_EMS_LED_Yellow))
                If (i <> -1) Then
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _results.LED_BIN_PT_YELLOW.Value = _LinInterface.RxFrame(i).Data(5)
                    '
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 30
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) = 30
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_Yellow),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) = 30
                End If

            Case 30
                backLightPWM = CInt(_recipe.WRITE_BackLight_PWM.Value) / 100
                If (_results.LED_BIN_PT_YELLOW.Value = "02" Or _results.LED_BIN_PT_YELLOW.Value = "03") Then
                    'special for bin 02&03, because average light reduced too much compare with bin 01
                    telltaleMirrorLeft = 70 / 100
                    telltaleMirrorRight = 70 / 100
                    telltaleChildrenLock = 85 / 100
                Else
                    telltaleMirrorLeft = CInt(_recipe.WRITE_MLTelltale_PWM.Value) / 100
                    telltaleMirrorRight = CInt(_recipe.WRITE_MRTelltale_PWM.Value) / 100
                    telltaleChildrenLock = CInt(_recipe.WRITE_CLTelltale_PWM.Value) / 100
                End If

                AddLogEntry("Write white LED backlight table, Value:" & backLightPWM.ToString())

                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "10" : f.Data(2) = "07" : f.Data(3) = "2E"
                f.Data(4) = "FD" : f.Data(5) = "47" : f.Data(6) = Hex(Int(95 * backLightPWM * 255 / 100)) : f.Data(7) = Hex(Int(81 * backLightPWM * 255 / 100))
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
                _subPhase(_phase) += 1
            Case 31
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(0) = "24" : f.Data(1) = "21" : f.Data(2) = Hex(Int(68 * backLightPWM * 255 / 100)) : f.Data(3) = Hex(Int(57 * backLightPWM * 255 / 100))
                    f.Data(4) = "00" : f.Data(5) = "00" : f.Data(6) = "00" : f.Data(7) = "00"
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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

            Case 32
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "03" : f.Data(2) = "6E" : f.Data(3) = "FD"
                f.Data(4) = "47" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) += 1
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    'Go to subphase
                    _subPhase(_phase) = 199
                End If
            Case 33
                AddLogEntry("Write left mirror selection telltale LED table:" & telltaleMirrorLeft.ToString())
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "10" : f.Data(2) = "07" : f.Data(3) = "2E"
                f.Data(4) = "FD" : f.Data(5) = "44" : f.Data(6) = Hex(Int(95 * telltaleMirrorLeft * 255 / 100)) : f.Data(7) = Hex(Int(76 * telltaleMirrorLeft * 255 / 100))
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
            Case 34
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(0) = "24" : f.Data(1) = "21" : f.Data(2) = Hex(Int(60 * telltaleMirrorLeft * 255 / 100)) : f.Data(3) = Hex(Int(48 * telltaleMirrorLeft * 255 / 100))
                    f.Data(4) = "00" : f.Data(5) = "00" : f.Data(6) = "00" : f.Data(7) = "00"
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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

            Case 35
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "03" : f.Data(2) = "6E" : f.Data(3) = "FD"
                f.Data(4) = "44" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) += 1
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    'Go to subphase
                    _subPhase(_phase) = 199
                End If
            Case 36
                AddLogEntry("Write right mirror selection telltale LED table:" & telltaleMirrorRight.ToString())
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "10" : f.Data(2) = "07" : f.Data(3) = "2E"
                f.Data(4) = "FD" : f.Data(5) = "45" : f.Data(6) = Hex(Int(95 * telltaleMirrorRight * 255 / 100)) : f.Data(7) = Hex(Int(76 * telltaleMirrorRight * 255 / 100))
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
                _subPhase(_phase) += 1
            Case 37
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(0) = "24" : f.Data(1) = "21" : f.Data(2) = Hex(Int(60 * telltaleMirrorRight * 255 / 100)) : f.Data(3) = Hex(Int(48 * telltaleMirrorRight * 255 / 100))
                    f.Data(4) = "00" : f.Data(5) = "00" : f.Data(6) = "00" : f.Data(7) = "00"
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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

            Case 38
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "03" : f.Data(2) = "6E" : f.Data(3) = "FD"
                f.Data(4) = "45" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) += 1
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    'Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 39
                AddLogEntry("Write window lock telltale LED table:" & telltaleChildrenLock.ToString())
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "10" : f.Data(2) = "07" : f.Data(3) = "2E"
                f.Data(4) = "FD" : f.Data(5) = "46" : f.Data(6) = Hex(Int(95 * telltaleChildrenLock * 255 / 100)) : f.Data(7) = Hex(Int(76 * telltaleChildrenLock * 255 / 100))
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
                _subPhase(_phase) += 1
            Case 40
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(0) = "24" : f.Data(1) = "21" : f.Data(2) = Hex(Int(60 * telltaleChildrenLock * 255 / 100)) : f.Data(3) = Hex(Int(48 * telltaleChildrenLock * 255 / 100))
                    f.Data(4) = "00" : f.Data(5) = "00" : f.Data(6) = "00" : f.Data(7) = "00"
                    ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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

            Case 41
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "03" : f.Data(2) = "6E" : f.Data(3) = "FD"
                f.Data(4) = "46" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    If _recipe.TestEnable_Mirrortiltingposition.Value Then ' Not sure how to implement in future, currently only one recipe needed. YAN.Qian20200522
                        _subPhase(_phase) = 42
                    Else
                        _subPhase(_phase) = 100
                    End If
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    'Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 42
                AddLogEntry("Write configuration of mirror tilting position")
                mirrorposition1 = "3D"
                mirrorposition2 = "50"
                mirrorposition3 = "65"
                mirrorposition4 = "32"
                memoposition1 = "3D"
                memoposition2 = "50"
                memoposition3 = "65"
                memoposition4 = "32"
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "10" : f.Data(2) = "07" : f.Data(3) = "2E"
                f.Data(4) = "FD" : f.Data(5) = "61" : f.Data(6) = mirrorposition1 : f.Data(7) = mirrorposition2
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
                _subPhase(_phase) += 1

            Case 43
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "21" : f.Data(2) = mirrorposition3 : f.Data(3) = mirrorposition4
                f.Data(4) = "00" : f.Data(5) = "00" : f.Data(6) = "00" : f.Data(7) = "00"
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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

            Case 44
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "03" : f.Data(2) = "6E" : f.Data(3) = "FD"
                f.Data(4) = "61" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) += 1
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    'Go to subphase
                    _subPhase(_phase) = 199
                End If
            Case 45
                AddLogEntry("Read configuration of mirror tilting position")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "03" : f.Data(2) = "22" : f.Data(3) = "FD"
                f.Data(4) = "61" : f.Data(5) = "00" : f.Data(6) = "00" : f.Data(7) = "00"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 0
                s = ""
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 46
                '
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "10" : f.Data(2) = "07" : f.Data(3) = "62"
                f.Data(4) = "FD" : f.Data(5) = "61" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(6) &
                            _LinInterface.RxFrame(i).Data(7)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' init index Frame
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - tLin).TotalMilliseconds > LinRelance_ms) And frameIndex = 0 Then
                    '    ' Transmit Frame
                    '    e = e Or _LinInterface.Transmit(f,
                    '                                        True,
                    '                                        txData_MasterReq,
                    '                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                    '                                        True,
                    '                                        True)
                    '    '
                    tLin = Date.Now
                    Exit Select
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                    Exit Select
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
                    _results.eWriteConfiguration.TestResult = cResultValue.eValueTestResult.Failed
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
                        s = s & _LinInterface.RxFrame(i).Data(2) &
                                    _LinInterface.RxFrame(i).Data(3)
                        ' Delete the frame
                        _LinInterface.DeleteRxFrame(i)
                        AddLogEntry("Read configuration of mirror tilting position,s " + s.ToString())
                        If mirrorposition1 <> Mid(s, 1, 2) OrElse mirrorposition2 <> Mid(s, 3, 2) OrElse mirrorposition3 <> Mid(s, 5, 2) OrElse mirrorposition4 <> Mid(s, 7, 2) Then
                            _results.eWriteConfiguration.TestResult = cResultValue.eValueTestResult.Failed
                            _subPhase(_phase) = 100
                        Else
                            _subPhase(_phase) += 1
                            memouserposition = 0 'reset memo user position.
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        ' Set the flag of CAN timeout
                        LinTimeOut = True
                        _subPhase(_phase) = 199
                    End If
                Loop Until (i = -1)
            Case 47
                memouserposition += 1
                AddLogEntry("Write configuration of memo tilting position " + memouserposition.ToString())
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "10" : f.Data(2) = "09" : f.Data(3) = "2E"
                f.Data(4) = "FD" : f.Data(5) = "70" : f.Data(6) = "01" : f.Data(7) = memouserposition.ToString().PadLeft(2, "0")
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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

            Case 48
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "21" : f.Data(2) = memoposition1 : f.Data(3) = memoposition2
                f.Data(4) = memoposition3 : f.Data(5) = memoposition4 : f.Data(6) = "00" : f.Data(7) = "00"
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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

            Case 49
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "03" : f.Data(2) = "6E" : f.Data(3) = "FD"
                f.Data(4) = "70" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) += 1
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    'Go to subphase
                    _subPhase(_phase) = 199
                End If
            Case 50
                AddLogEntry("Read configuration of memo tilting position " + memouserposition.ToString())
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "05" : f.Data(2) = "22" : f.Data(3) = "FD"
                f.Data(4) = "70" : f.Data(5) = "01" : f.Data(6) = memouserposition.ToString().PadLeft(2, "0") : f.Data(7) = "00"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 0
                s = ""
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 51
                '
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "24" : f.Data(1) = "10" : f.Data(2) = "07" : f.Data(3) = "62"
                f.Data(4) = "FD" : f.Data(5) = "70" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(6) &
                            _LinInterface.RxFrame(i).Data(7)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' init index Frame
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                    'ElseIf ((Date.Now - tLin).TotalMilliseconds > LinRelance_ms) And frameIndex = 0 Then
                    '    ' Transmit Frame
                    '    e = e Or _LinInterface.Transmit(f,
                    '                                        True,
                    '                                        txData_MasterReq,
                    '                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                    '                                        True,
                    '                                        True)
                    '    '
                    '    tLin = Date.Now
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
                    _results.eWriteConfiguration.TestResult = cResultValue.eValueTestResult.Failed
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
                        s = s + _LinInterface.RxFrame(i).Data(2) &
                                    _LinInterface.RxFrame(i).Data(3)
                        ' Delete the frame
                        _LinInterface.DeleteRxFrame(i)
                        AddLogEntry("Read configuration of memo tilting position " + memouserposition.ToString() + "s:" + s.ToString())
                        If memoposition1 <> Mid(s, 1, 2) OrElse memoposition2 <> Mid(s, 3, 2) OrElse memoposition3 <> Mid(s, 5, 2) OrElse memoposition4 <> Mid(s, 7, 2) Then
                            _results.eWriteConfiguration.TestResult = cResultValue.eValueTestResult.Failed
                            _subPhase(_phase) = 100
                        Else
                            If (memouserposition < 6) Then
                                _subPhase(_phase) = 47 'Next user memo.
                            Else
                                _subPhase(_phase) = 100 'finish.
                            End If
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        ' Set the flag of CAN timeout
                        LinTimeOut = True
                        _subPhase(_phase) = 199
                    End If
                Loop Until (i = -1)

            Case 20
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_EV_HW_Coding).DeepClone
                f.Data(5) = _recipe.WRITE_HW_Coding.Value
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
                _subPhase(_phase) = 21

            Case 21
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_EV_HW_Coding))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) = 100
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    If Relance >= 1 Or NRC_78 Then
                        LinTimeOut = True
                        'Go to subphase
                        _subPhase(_phase) = 199
                    Else
                        Relance += 1
                        ' Go to subphase
                        _subPhase(_phase) = 20
                    End If
                End If

                ' Manage the NRC 78 Pending
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "XX" : f.Data(1) = "03" : f.Data(2) = "7F" : f.Data(3) = "2E"
                f.Data(4) = "XX" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    If _LinInterface.RxFrame(i).Data(4) = "78" Then
                        AddLogEntry("NRC 78 , Pending Request")
                        NRC_78 = True
                    Else
                        LinTimeOut = True
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
                If _results.eWriteConfiguration.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eWriteConfiguration.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If

                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        _results.eWriteConfiguration.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedeWrite_TraceabilityMMS
                End If

                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.NormalModeCurrent

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eWriteConfiguration.TestResult =
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
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If

    End Sub

    Private Sub PhaseNormalModeCurrent()
        Dim e As Boolean
        Dim sp As Integer
        Dim sample(0 To mWS02AIOManager.eAnalogInput.Count - 1) As Double
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
                AddLogEntry("Begin Normal Mode Current")
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 1

            Case 1
                If ((Date.Now - t0).TotalMilliseconds > 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    _results.PowerUp_Ibat.Value = sample(mWS02AIOManager.eAnalogInput.WS02_IBAT) * 1000 'Update by YAN.Qian at 20200224
                    For i = 0 To 31
                        AddLogEntry(mWS02AIOManager.AnalogInputDescription(i) & " : " & sample(i))
                    Next
                    ' Store the time
                    t0 = Date.Now

                    ' Go to next subphase
                    _subPhase(_phase) = 100

                End If

            Case 100
                ' Tests
                If (_results.PowerUp_Ibat.Test = cResultValue.eValueTestResult.Passed And
                    _results.eNormalModeCurrent.TestResult = cResultValue.eValueTestResult.Unknown) Then
                    _results.eNormalModeCurrent.TestResult =
                        cResultValue.eValueTestResult.Passed
                Else
                    _results.eNormalModeCurrent.TestResult =
                        cResultValue.eValueTestResult.Failed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eNormalModeCurrent.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedNormalModeCurrent
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.EMSTraceability

            Case 199
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eNormalModeCurrent.TestResult =
                    cResultValue.eValueTestResult.Failed

                ' Go to next subphase
                _subPhase(_phase) = 5
                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Remote Then
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
        End If

    End Sub


    Private Sub PhaseREAD_EMSTraceability()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
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
                If CBool(_recipe.TestEnable_EMSTraceability.Value) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Read EMS Traceability")
                    ' Store the time
                    t0 = Date.Now
                    If Not _recipe.TestEnable_EV_Option.Value Then
                        ' Go to next subphase
                        _subPhase(_phase) = 1 '1
                    Else
                        ' Go to next subphase， still have old stock which need this function.
                        _subPhase(_phase) = 1
                    End If
                Else
                    ' Add a log entry
                    AddLogEntry("Read EMS Traceability is desabled")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _phase = ePhase.AnalogInput
                End If

            Case 1
                ' Transmit Frame
                ' Add a log entry
                AddLogEntry("Request Read EMS Traceability")
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMSTracea),
                                                                            True,
                                                                            txData_MasterReq,
                                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                                            True,
                                                                            True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 2
                '
                tLin = Date.Now
                '
                frameIndex = 0
                ' Store the time
                t0 = Date.Now

            Case 2
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_EMSTracea))
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMSTracea),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
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

                            _results.PCBA_Number_Reference.Value = Mid(s, 1, 10)
                            _results.PCBA_Number_Index.Value = Mid(s, 11, 4)
                            _results.PCBA_Plant_Line.Value = Mid(s, 15, 4)
                            _results.PCBA_ManufacturingDate.Value = Mid(s, 19, 10)
                            _results.PCBA_SerialNumber.Value = Mid(s, 29, 8)
                            _results.PCBA_DeviationNumber.Value = Mid(s, 37, 2)

                            _results.PCBA_Number_Reference.Test()
                            '_results.PCBA_Number_Index.Test()
                            _results.PCBA_Plant_Line.Test()
                            _results.PCBA_ManufacturingDate.Test()
                            _results.PCBA_SerialNumber.Test()
                            _results.PCBA_DeviationNumber.Test()

                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 3
                AddLogEntry("Request Read SW Version")

                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_SW_Version),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 0
                ' Store the time
                t0 = Date.Now
                '
                tLin = Date.Now

            Case 4
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_SW_Version))
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_SW_Version),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
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
                        If (frameIndex < 21) Then
                            frameIndex = frameIndex + 1
                            t0 = Date.Now
                        Else
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            _results.Major_SoftwareVersion.Value = Mid(s, 1, 2)
                            _results.Minor_SoftwareVersion.Value = Mid(s, 3, 2)
                            _results.Major_NVMversion.Value = Mid(s, 9, 2)
                            _results.Minor_NVMversion.Value = Mid(s, 11, 2)

                            _results.Major_SoftwareVersion.Test()
                            _results.Minor_SoftwareVersion.Test()
                            _results.Major_NVMversion.Test()
                            _results.Minor_NVMversion.Test()

                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 5
                AddLogEntry("Request Read EMS ICT Byte")

                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMSICTByte),
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
                tLin = Date.Now

            Case 6
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_EMSICTByte))
                If (i <> -1) Then
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _results.EMS_Test_Byte.Value = _LinInterface.RxFrame(i).Data(5)
                    If _results.EMS_Test_Byte.Value = "03" Or
                        _results.EMS_Test_Byte.Value = "07" Or
                         _results.EMS_Test_Byte.Value = "FF" Then
                        'FF is tempory wait the EMS is done 
                        _results.EMS_Test_Byte.MinimumLimit = _results.EMS_Test_Byte.Value
                        _results.EMS_Test_Byte.MaximumLimit = _results.EMS_Test_Byte.Value
                    End If
                    _results.EMS_Test_Byte.Test()
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMSICTByte),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

                ' check if NRC22
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_NRC_22))
                If (i <> -1) Then
                    _results.EMS_Test_Byte.Value = "FF"
                    _results.EMS_Test_Byte.MinimumLimit = _results.EMS_Test_Byte.Value
                    _results.EMS_Test_Byte.MaximumLimit = _results.EMS_Test_Byte.Value
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 7
                AddLogEntry("Request Read SW Cheksum")

                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_SW_Checksum),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) = 8

                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 8
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_SW_Checksum))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(5) &
                            _LinInterface.RxFrame(i).Data(6)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _results.SW_checksum.Value = s
                    ' Go to subphase
                    _subPhase(_phase) = 10
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) = 10
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_SW_Checksum),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) = 10
                End If

            Case 10
                If Not _recipe.TestEnable_EV_Option.Value Then
                    AddLogEntry("Request Read EMS LED Rank,DIAG_Req_EMS_LED_RSA")

                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_RSA),
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
                Else
                    ' Go to next subphase
                    _subPhase(_phase) = 14
                End If

            Case 11
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_EMS_LED_RSA))
                If (i <> -1) Then
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _results.LED_BIN_PT_White_RSA.Value = _LinInterface.RxFrame(i).Data(5)
                    '
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 3   'Skip Read LED RED.
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 3   'Skip Read LED RED.
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_RSA),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 3   'Skip Read LED RED.
                End If

            Case 12
                'Skip Read LED RED.
                AddLogEntry("Request Read EMS LED Rank,DIAG_Req_EMS_LED_RED")

                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_RED),
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

            Case 13
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_EMS_LED_RED))
                If (i <> -1) Then
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _results.LED_BIN_PT_RED.Value = _LinInterface.RxFrame(i).Data(5)
                    '
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_RED),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 14
                AddLogEntry("Request Read EMS LED Rank,DIAG_Req_EMS_LED_Yellow")

                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_Yellow),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 15
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_EMS_LED_Yellow))
                If (i <> -1) Then
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _results.LED_BIN_PT_YELLOW.Value = _LinInterface.RxFrame(i).Data(5)
                    '
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_Yellow),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 16
                AddLogEntry("Request Read EMS LED Rank,DIAG_Req_EMS_LED_Nissan")

                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_Nissan),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 17
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_EMS_LED_Nissan))
                If (i <> -1) Then
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _results.LED_BIN_PT_WHITE_NISSAN.Value = _LinInterface.RxFrame(i).Data(5)
                    '
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 5) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_EMS_LED_Nissan),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 18
                If Not _recipe.TestEnable_EV_Option.Value Then
                    AddLogEntry("Request Read SW Coding")
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_SW_Coding),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                Else
                    ' Go to next subphase
                    _subPhase(_phase) = 30
                End If

            Case 19
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_SW_Coding))
                If (i <> -1) Then
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _results.SW_Coding.Value = _LinInterface.RxFrame(i).Data(5) & _LinInterface.RxFrame(i).Data(6)
                    '
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 5) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_SW_Coding),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 20
                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_Backlight_Coding),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 21
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Read_Backlight_Coding))
                If (i <> -1) Then
                    _results.Backlight_Coding.Value = _LinInterface.RxFrame(i).Data(5)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Go to subphase
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - tLin).TotalMilliseconds > LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_Backlight_Coding),
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
                    _results.eRead_TraceabilityMMS.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 22
                ' Go to subphase
                _subPhase(_phase) = 100

            Case 30
                AddLogEntry("Request Read HW Coding")
                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_EV_HW_Coding),
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

            Case 31
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Read_EV_HW_Coding))
                If (i <> -1) Then
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _results.HW_Coding.Value = _LinInterface.RxFrame(i).Data(5)
                    '
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 5) Then
                    ' Go to subphase
                    '_subPhase(_phase) += 1
                    _subPhase(_phase) = 100 ' fixed by YAN.Qian 20200111, for deadloop, there is case 32.
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_EV_HW_Coding),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
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
                    '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) = 100
                End If


            'Case 50
            '    ' Transmit Frame
            '    ' Add a log entry
            '    If (CurrentSampleType = EnumSampleType.Production) Then
            '        AddLogEntry("Request Write LED_BIN_PT_White_RSA")
            '        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
            '        f.Data(1) = "04"
            '        f.Data(2) = "2E"
            '        f.Data(3) = "FD"
            '        f.Data(4) = "12"
            '        f.Data(5) = Right("00" & _recipe.LED_BIN_PT_White_RSA.MinimumLimit, 2)  '"00"
            '        f.Data(6) = "00"
            '        f.Data(7) = "00"

            '        e = e Or _LinInterface.Transmit(f,
            '                                                                True,
            '                                                                txData_MasterReq,
            '                                                                cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
            '                                                                True,
            '                                                                True)
            '        ' Go to next subphase
            '        _subPhase(_phase) += 1
            '        '
            '        tLin = Date.Now
            '        '
            '        frameIndex = 0
            '        ' Store the time
            '        t0 = Date.Now
            '    Else
            '        ' Go to next subphase
            '        _subPhase(_phase) = 1
            '    End If
            'Case 51
            '    '
            '    f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
            '    f.Data(1) = "03"
            '    f.Data(2) = "6E"
            '    f.Data(3) = "FD"
            '    f.Data(4) = "12"
            '    f.Data(5) = "XX"
            '    f.Data(6) = "XX"
            '    f.Data(7) = "XX"
            '    i = _LinInterface.RxFrameIndex(f)
            '    If (i <> -1) Then
            '        ' Delete the frame
            '        _LinInterface.DeleteRxFrame(i)
            '        ' Store the time
            '        t0 = Date.Now
            '        ' Go to subphase
            '        _subPhase(_phase) = 1 '+=1,YANQian, EMS FCT implement, but RSA is not right. only enable this one.
            '    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
            '        '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
            '        ' Go to subphase
            '        _subPhase(_phase) = 1 '+=1,YANQian, EMS FCT implement, but RSA is not right. only enable this one.
            '    End If

            'Case 52
            '    ' Transmit Frame
            '    ' Add a log entry
            '    AddLogEntry("Request Write LED_BIN_PT_White_Nissan")
            '    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
            '    f.Data(1) = "04"
            '    f.Data(2) = "2E"
            '    f.Data(3) = "FD"
            '    f.Data(4) = "15"
            '    f.Data(5) = Right("00" & _recipe.LED_BIN_PT_White_Nissan.MinimumLimit, 2) '"00"
            '    f.Data(6) = "00"
            '    f.Data(7) = "00"

            '    e = e Or _LinInterface.Transmit(f,
            '                                                            True,
            '                                                            txData_MasterReq,
            '                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
            '                                                            True,
            '                                                            True)
            '    msngT0TesteurPeriodicFrame = Date.Now
            '    ' Go to next subphase
            '    _subPhase(_phase) += 1
            '    '
            '    tLin = Date.Now
            '    '
            '    frameIndex = 0
            '    ' Store the time
            '    t0 = Date.Now

            'Case 53
            '    '
            '    f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
            '    f.Data(1) = "03"
            '    f.Data(2) = "6E"
            '    f.Data(3) = "FD"
            '    f.Data(4) = "15"
            '    f.Data(5) = "XX"
            '    f.Data(6) = "XX"
            '    f.Data(7) = "XX"
            '    i = _LinInterface.RxFrameIndex(f)
            '    If (i <> -1) Then
            '        ' Delete the frame
            '        _LinInterface.DeleteRxFrame(i)
            '        ' Store the time
            '        t0 = Date.Now
            '        ' Go to subphase
            '        _subPhase(_phase) = 1
            '    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
            '        '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
            '        ' Go to subphase
            '        _subPhase(_phase) = 1
            '    End If

            'Case 54
            '    ' Transmit Frame
            '    ' Add a log entry
            '    AddLogEntry("Request Write LED rank level for Yellow")
            '    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
            '    f.Data(1) = "04"
            '    f.Data(2) = "2E"
            '    f.Data(3) = "FD"
            '    f.Data(4) = "14"
            '    'f.Data(5) = Right("00" & _recipe.LED_BIN_PT_Yellow.MinimumLimit, 2) '"00"
            '    f.Data(5) = "01"
            '    f.Data(6) = "00"
            '    f.Data(7) = "00"

            '    e = e Or _LinInterface.Transmit(f,
            '                                                            True,
            '                                                            txData_MasterReq,
            '                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
            '                                                            True,
            '                                                            True)
            '    msngT0TesteurPeriodicFrame = Date.Now
            '    ' Go to next subphase
            '    _subPhase(_phase) += 1
            '    '
            '    tLin = Date.Now
            '    '
            '    frameIndex = 0
            '    ' Store the time
            '    t0 = Date.Now

            'Case 55
            '    '
            '    f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
            '    f.Data(1) = "03"
            '    f.Data(2) = "6E"
            '    f.Data(3) = "FD"
            '    f.Data(4) = "14"
            '    f.Data(5) = "XX"
            '    f.Data(6) = "XX"
            '    f.Data(7) = "XX"
            '    i = _LinInterface.RxFrameIndex(f)
            '    If (i <> -1) Then
            '        ' Delete the frame
            '        _LinInterface.DeleteRxFrame(i)
            '        ' Store the time
            '        t0 = Date.Now
            '        ' Go to subphase
            '        _subPhase(_phase) = 1
            '    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
            '        '_results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Failed
            '        ' Go to subphase
            '        _subPhase(_phase) = 1
            '    End If

            Case 100
                ' Tests
                '_results.PCBA_Number_Index.Test <> cResultValue.eValueTestResult.Passed Or
                If (_results.PCBA_Number_Reference.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.PCBA_Plant_Line.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.PCBA_ManufacturingDate.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.PCBA_SerialNumber.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.PCBA_DeviationNumber.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Major_SoftwareVersion.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Minor_SoftwareVersion.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Major_NVMversion.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Minor_NVMversion.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.SW_checksum.Test <> cResultValue.eValueTestResult.Passed Or
                    (Not _recipe.TestEnable_EV_Option.Value And
                     _results.LED_BIN_PT_White_RSA.Test <> cResultValue.eValueTestResult.Passed) Or
                    _results.LED_BIN_PT_YELLOW.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.LED_BIN_PT_WHITE_NISSAN.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.EMS_Test_Byte.Test <> cResultValue.eValueTestResult.Passed Or
                    (_recipe.TestEnable_WriteConfiguration.Value And (Not _recipe.TestEnable_EV_Option.Value) And
                     (_results.SW_Coding.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Backlight_Coding.Test <> cResultValue.eValueTestResult.Passed)) Or
                    (_recipe.TestEnable_WriteConfiguration.Value And _recipe.TestEnable_EV_Option.Value And
                     _results.HW_Coding.Test <> cResultValue.eValueTestResult.Passed)) Then
                    '_results.LED_BIN_PT_RED.Test <> cResultValue.eValueTestResult.Passed)) Or _ 'Skipped Read LED RED
                    _results.eEmsTraceability.TestResult =
                        cResultValue.eValueTestResult.Failed
                Else
                    _results.eEmsTraceability.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eEmsTraceability.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedEMSTraceability
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.AnalogInput

            Case 199
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eEmsTraceability.TestResult =
                    cResultValue.eValueTestResult.Failed

                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        If TestMode = eTestMode.Remote Then
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
        End If

    End Sub

    Private Sub PhaseShape()
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Dim r As Integer
        Static t0 As Date
        Static t0Phase As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                ' If the test is enabled
                If (CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) Or
                    CBool(_recipe.TestEnable_BezelConformity.Value) Or
                    CBool(_recipe.TestEnable_DecorFrameConformity.Value) Or
                    CBool(_recipe.TestEnable_SHAPE_Folding_Mirror.Value) Or
                    CBool(_recipe.TestEnable_SHAPE_ChildrenLock.Value)) Then

                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Push Shape Camera Test")
                    ' Store the time
                    t0 = Date.Now
                    If Not _recipe.TestEnable_EV_Option.Value Then
                        ' Go to next subphase
                        _subPhase(_phase) = 1
                    Else
                        ' Go to next subphase
                        _subPhase(_phase) = 1000
                    End If
                Else    ' Otherwise, if the test is disabled
                    AddLogEntry("Push Shape Camera test is Disabled")
                    ' Go to next phase
                    _phase = ePhase.BACKLIGHT
                End If

            Case 1000
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight_EV),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 1002

            Case 1002
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
                    _subPhase(_phase) = 1003 '3
                End If

            Case 1003
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Backlight))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 1

                End If

            Case 1
                ' If the camera is ready
                If ((mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_RUN) And
                     Not mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_Busy) And
                     mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_TrigReady))) Then
                    ' Set the trigger
                    AddLogEntry("Set Trigger Shape")
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Keyence_Trig1)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 2
                End If

            Case 2
                ' If the camera is busy
                If ((mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_Busy)) Or
                    (Date.Now - t0).TotalMilliseconds > 200) Then
                    AddLogEntry("ReSet Trigger Shape")
                    ' Reset the trigger
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Keyence_Trig1)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 3
                End If

            Case 3
                ' If the keyence camera results were received
                r = mWS02Keyence.ReadResultsFHM(_resultCamera)
                If (r = 0) Then
                    ' Store the result values
                    If CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) Then
                        _results.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR
                        _results.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR
                        _results.MINIMUM_CONFORMITY_PUSH_ADJUST_UP.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_ADJUST_UP
                        _results.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN
                        _results.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT
                        _results.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT

                        _results.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR.Value = _resultCamera.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR
                        _results.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR.Value = _resultCamera.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR
                        _results.DEFECT_AREA_PUSH_ADJUST_UP.Value = _resultCamera.DEFECT_AREA_PUSH_ADJUST_UP
                        _results.DEFECT_AREA_PUSH_ADJUST_DOWN.Value = _resultCamera.DEFECT_AREA_PUSH_ADJUST_DOWN
                        _results.DEFECT_AREA_PUSH_ADJUST_RIGHT.Value = _resultCamera.DEFECT_AREA_PUSH_ADJUST_RIGHT
                        _results.DEFECT_AREA_PUSH_ADJUST_LEFT.Value = _resultCamera.DEFECT_AREA_PUSH_ADJUST_LEFT

                        _results.Ring_Red.Value = _resultCamera.RING_RED
                        _results.Ring_Green.Value = _resultCamera.RING_GREEN
                        _results.Ring_Blue.Value = _resultCamera.RING_BLUE


                    End If
                    If CBool(_recipe.TestEnable_SHAPE_Folding_Mirror.Value) Then
                        _results.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR
                        _results.DEFECT_AREA_PUSH_FOLDING_MIRROR.Value = _resultCamera.DEFECT_AREA_PUSH_FOLDING_MIRROR
                    End If
                    If CBool(_recipe.TestEnable_SHAPE_Wili_Front.Value) Then
                        _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT.Value = _resultCamera.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT
                        _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT.Value = _resultCamera.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT

                        _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT.Value = _resultCamera.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT
                        _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT.Value = _resultCamera.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT
                    End If
                    If CBool(_recipe.TestEnable_SHAPE_Wili_Rear.Value) Then
                        _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT.Value = _resultCamera.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT
                        _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT.Value = _resultCamera.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT

                        _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT.Value = _resultCamera.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT
                        _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT.Value = _resultCamera.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT
                    End If
                    If CBool(_recipe.TestEnable_SHAPE_ChildrenLock.Value) Then
                        _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK
                        _results.DEFECT_AREA_PUSH_CHILDREN_LOCK.Value = _resultCamera.DEFECT_AREA_PUSH_CHILDREN_LOCK
                    End If
                    If CBool(_recipe.TestEnable_SHAPE_ChildrenLock2.Value) Then
                        _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2.Value = _resultCamera.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2
                        _results.DEFECT_AREA_PUSH_CHILDREN_LOCK2.Value = _resultCamera.DEFECT_AREA_PUSH_CHILDREN_LOCK2
                    End If
                    If CBool(_recipe.TestEnable_Adapter.Value) Then
                        _results.CONFORMITY_Adatper_Front.Value = _resultCamera.MINIMUM_CONFORMITY_Shape_Adapter_Front
                        _results.CONFORMITY_Adatper_Rear.Value = _resultCamera.MINIMUM_CONFORMITY_Shape_Adapter_Rear
                    End If
                    If CBool(_recipe.TestEnable_CustomerInterface.Value) Then
                        _results.Customer_Interface(0).Value = _resultCamera.Customer_interface_1
                        _results.Customer_Interface(1).Value = _resultCamera.Customer_interface_2
                        _results.Customer_Interface(2).Value = _resultCamera.Customer_interface_3
                        _results.Customer_Interface(3).Value = _resultCamera.Customer_interface_4
                        _results.Customer_Interface(4).Value = _resultCamera.Customer_interface_5
                        _results.Customer_Interface(5).Value = _resultCamera.Customer_interface_6
                        _results.Customer_Interface(6).Value = _resultCamera.Customer_interface_7
                        _results.Customer_Interface(7).Value = _resultCamera.Customer_interface_8
                        _results.Customer_Interface(8).Value = _resultCamera.Customer_interface_9
                        _results.Customer_Interface(9).Value = _resultCamera.Customer_interface_10
                        _results.Customer_Interface(10).Value = _resultCamera.Customer_interface_11
                        _results.Customer_Interface(11).Value = _resultCamera.Customer_interface_12
                        _results.Customer_Interface(12).Value = _resultCamera.Customer_interface_13
                        _results.Customer_Interface(13).Value = _resultCamera.Customer_interface_14
                        _results.Customer_Interface(14).Value = _resultCamera.Customer_interface_15
                        _results.Customer_Interface(15).Value = _resultCamera.Customer_interface_16
                        _results.Customer_Interface(16).Value = _resultCamera.Customer_interface_17
                        _results.Customer_Interface(17).Value = _resultCamera.Customer_interface_18
                        _results.Customer_Interface(18).Value = _resultCamera.Customer_interface_19
                        _results.Customer_Interface(19).Value = _resultCamera.Customer_interface_20
                    End If
                    If CBool(_recipe.TestEnable_PointsOnWili.Value) Then
                        _results.Points_On_Front_Left.Value = _resultCamera.PontsOnWili_FrontLeft
                        _results.Points_On_Front_Right.Value = _resultCamera.PontsOnWili_FrontRight
                        _results.Points_On_Rear_Left.Value = _resultCamera.PontsOnWili_RearLeft
                        _results.Points_On_Rear_Right.Value = _resultCamera.PontsOnWili_RearRight
                    End If
                    'If CBool(_recipe.TestEnable_BezelConformity.Value) Then
                    '    _results.CONFORMITY_Bezel.Value = _resultCamera.CONFORMITY_Bezel
                    'End If
                    'If CBool(_recipe.TestEnable_DecorFrameConformity.Value) Then
                    '    _results.CONFORMITY_DecorFrame.Value = _resultCamera.CONFORMITY_Decor_Frame
                    'End If

                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 10
                ElseIf (r = 1 And (Date.Now - t0).TotalSeconds > 2) Then    ' Otherwise, if the camera results were not received within 2 s
                    ' Add a log entry
                    AddLogEntry("Timeout in camera result reception")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                ElseIf (r = -1) Then    ' Otherwise, if some error happened
                    ' Add a log entry
                    AddLogEntry("Runtime error in camera result reception")
                    ' Sets the error flag
                    e = True
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                End If

            Case 10
                If Not _recipe.TestEnable_EV_Option.Value Then
                    ' Go to next subphase
                    _subPhase(_phase) = 100
                Else
                    ' Switch-off the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF_EV),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 11
                End If

            Case 11
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    ' Switch-off the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF2_EV),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 12 '3
                End If

            Case 12
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_All_OFF))
                If (i <> -1) Then
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
                End If

            Case 100
                ' Test the values
                If CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) Then
                    If (_results.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) Then
                    If (_results.MINIMUM_CONFORMITY_PUSH_ADJUST_UP.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_ADJUST_UP.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_ADJUST_DOWN.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_ADJUST_LEFT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_ADJUST_RIGHT.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) Then
                    If (_results.Ring_Red.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Ring_Green.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Ring_Blue.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If CBool(_recipe.TestEnable_SHAPE_Folding_Mirror.Value) Then
                    If (_results.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_FOLDING_MIRROR.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If CBool(_recipe.TestEnable_SHAPE_Wili_Front.Value) Then
                    If (_results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If CBool(_recipe.TestEnable_SHAPE_Wili_Rear.Value) Then
                    If (_results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If CBool(_recipe.TestEnable_SHAPE_ChildrenLock.Value) Then
                    If (_results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_CHILDREN_LOCK.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If
                If CBool(_recipe.TestEnable_SHAPE_ChildrenLock2.Value) Then
                    If (_results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.DEFECT_AREA_PUSH_CHILDREN_LOCK2.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If
                If CBool(_recipe.TestEnable_Adapter.Value) Then
                    If (_results.CONFORMITY_Adatper_Front.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.CONFORMITY_Adatper_Rear.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If
                If CBool(_recipe.TestEnable_CustomerInterface.Value) Then
                    For index = 0 To 19
                        If _results.Customer_Interface(index).Test <> cResultValue.eValueTestResult.Passed And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    Next
                End If
                If CBool(_recipe.TestEnable_PointsOnWili.Value) Then
                    If (_results.Points_On_Front_Left.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Points_On_Front_Right.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Points_On_Rear_Left.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Points_On_Rear_Right.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Passed
                End If

                'If CBool(_recipe.TestEnable_BezelConformity.Value) Then
                '    If _results.CONFORMITY_Bezel.Test <> cResultValue.eValueTestResult.Passed Then
                '        _results.eBezel_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                '    Else
                '        _results.eBezel_CONFORMITY.TestResult = cResultValue.eValueTestResult.Passed
                '    End If
                'End If
                'If CBool(_recipe.TestEnable_DecorFrameConformity.Value) Then
                '    If _results.CONFORMITY_DecorFrame.Test <> cResultValue.eValueTestResult.Passed Then
                '        _results.eDecorFrame_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                '    Else
                '        _results.eDecorFrame_CONFORMITY.TestResult = cResultValue.eValueTestResult.Passed
                '    End If
                'End If

                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                ' If there is a step condition
                If (Not (StepByStep) Or (StepByStep And _step)) Then
                    ' Clear the step flag
                    _step = False
                    ' Update the global test result and add a log entry
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) And
                        (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush1_Shape
                        ElseIf _results.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush2_Shape
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) And
                            (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.MINIMUM_CONFORMITY_PUSH_ADJUST_UP.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_ADJUST_UP.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush4_Shape
                        ElseIf _results.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_ADJUST_DOWN.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush5_Shape
                        ElseIf _results.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_ADJUST_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush6_Shape
                        ElseIf _results.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_ADJUST_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush7_Shape
                        ElseIf _results.Ring_Red.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.Ring_Green.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.Ring_Blue.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedeRingRGB

                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_SHAPE_Folding_Mirror.Value) And
                            (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_FOLDING_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush3_Shape
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_SHAPE_Wili_Front.Value) And
                            (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush8_Shape
                        ElseIf _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush9_Shape
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                                            (CBool(_recipe.TestEnable_SHAPE_Wili_Rear.Value) And
                                                (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush10_Shape
                        ElseIf _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush11_Shape
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_SHAPE_ChildrenLock.Value) And
                            (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush12_Shape
                        End If
                        'ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And _
                        '    (CBool(_recipe.TestEnable_DecorFrameConformity.Value) And _
                        '        (_results.eDecorFrame_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        '    _results.TestResult = cWS02Results.eTestResult.FailedDecorFrame
                        'ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And _
                        '    (CBool(_recipe.TestEnable_BezelConformity.Value) And _
                        '        (_results.eBezel_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        '    _results.TestResult = cWS02Results.eTestResult.FailedBezel
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_SHAPE_ChildrenLock2.Value) And
                            (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK2.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush13_Shape
                        End If
                        'ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And _
                        '    (CBool(_recipe.TestEnable_DecorFrameConformity.Value) And _
                        '        (_results.eDecorFrame_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        '    _results.TestResult = cWS02Results.eTestResult.FailedDecorFrame
                        'ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And _
                        '    (CBool(_recipe.TestEnable_BezelConformity.Value) And _
                        '        (_results.eBezel_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        '    _results.TestResult = cWS02Results.eTestResult.FailedBezel
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_Adapter.Value) And
                            (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.CONFORMITY_Adatper_Front.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.CONFORMITY_Adatper_Rear.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedeAdapter
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_CustomerInterface.Value) And
                            (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        For index = 0 To 19
                            If _results.Customer_Interface(index).TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedeCustomerInterface
                                Exit For
                            End If
                        Next
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (CBool(_recipe.TestEnable_PointsOnWili.Value) And
                            (_results.eShape_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If _results.Points_On_Front_Left.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.Points_On_Front_Right.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.Points_On_Rear_Left.TestResult <> cResultValue.eValueTestResult.Passed Or
                            _results.Points_On_Rear_Right.TestResult <> cResultValue.eValueTestResult.Passed Then
                            _results.TestResult = cWS02Results.eTestResult.FailedePointOnWiLi
                        End If
                    End If


                    AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                    ' Clear the subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.BACKLIGHT
                End If

            Case 199
                ' Update the test result
                _results.eShape_CONFORMITY.TestResult =
                        cResultValue.eValueTestResult.Failed
                '_results.eBezel_CONORMITY.TestResult = _
                '        cResultValue.eValueTestResult.Failed
                '_results.eDecorFrame_CONFORMITY.TestResult = _
                '        cResultValue.eValueTestResult.Failed

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



    Private Sub PhaseBacklight()
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Dim r As Integer
        Dim f As CLINFrame
        Dim x As Single
        Dim y As Single
        Dim Yl As Single
        Dim xpoints(0 To 5) As Single
        Dim ypoints(0 To 5) As Single
        Static previousSubphase As Integer
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
                ' If the test is enabled
                If (CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Or
                    CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) Or
                    CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Or
                    CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Or
                    CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value)) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry and clear the LIN timeout flag
                    AddLogEntry("Begin Push Backlight Camera Test")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1
                Else    ' Otherwise, if the test is disabled
                    ' Add a log entry
                    AddLogEntry("Push Backlight Camera test is Disabled")
                    ' Go to next phase
                    PhasePrevious = ePhase.HOMOGENEITY
                    _phase = ePhase.DigitalOutput
                End If

            Case 1
                If Not _recipe.TestEnable_EV_Option.Value Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight),
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
                    _subPhase(_phase) = 3
                Else
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Backlight_EV),
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
                    _subPhase(_phase) = 3
                End If

            Case 3
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Backlight))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 20 '3
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 20

                End If


            Case 20
                ' If the camera is ready
                If ((mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_RUN) And
                     Not mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_Busy) And
                     mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_TrigReady)) And
                         (Date.Now - t0).TotalMilliseconds > TriggerDelay_ms And
                         (Not (_stepByStep) Or (_stepByStep And _step))) Or
                     TestMode = eTestMode.Debug Then
                    ' Clear the step flag
                    _step = False
                    AddLogEntry("Set Trigger Backlight")
                    ' Set the trigger
                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Keyence_Trig1)
                    ' Go to next subphase
                    _subPhase(_phase) = 21
                    ' Store the time
                    t0 = Date.Now
                End If

            Case 21
                ' If the camera is busy
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_Busy)) Or
                 ((Date.Now - t0).TotalMilliseconds > 200) Then
                    AddLogEntry("ReSet Trigger Backlight")
                    ' Reset the trigger
                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Keyence_Trig1)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 22

                    '
                    PhasePrevious = _phase
                    '
                    _phase = ePhase.DigitalOutput
                End If

            Case 22
                ' If the keyence  camera results were received
                r = mWS02Keyence.ReadResultsFHM(_resultCamera)
                If (r = 0) Then
                    ' Store the values
                    'For i = 0 To 1
                    Dim TempDebugIntensity As Boolean = True
                    If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_SELECT_LEFT_backlight_red,
                                                  _resultCamera.Push_SELECT_LEFT_backlight_green,
                                                  _resultCamera.Push_SELECT_LEFT_backlight_blue,
                                                  x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_SELECT_LEFT_Backlight_x.Value = x
                        _results.Push_SELECT_LEFT_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_SELECT_LEFT_Backlight_Polygon_Axy,
                                             _results.Push_SELECT_LEFT_Backlight_Polygon_Bxy,
                                             _results.Push_SELECT_LEFT_Backlight_Polygon_Cxy,
                                             _results.Push_SELECT_LEFT_Backlight_Polygon_Dxy,
                                             _results.Push_SELECT_LEFT_Backlight_Polygon_Exy,
                                             _results.Push_SELECT_LEFT_Backlight_Polygon_Fxy)

                        _results.Push_SELECT_LEFT_backlight_intensity_Camera.Value = _resultCamera.Push_SELECT_LEFT_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR

                        _results.Push_SELECT_LEFT_backlight_intensity.Value = _recipe.Correlation_Factor_A(0).Value * Yl + _recipe.Correlation_Factor_B(0).Value

                        _results.Push_SELECT_LEFT_backlight_red.Value = _resultCamera.Push_SELECT_LEFT_backlight_red
                        _results.Push_SELECT_LEFT_backlight_green.Value = _resultCamera.Push_SELECT_LEFT_backlight_green
                        _results.Push_SELECT_LEFT_backlight_blue.Value = _resultCamera.Push_SELECT_LEFT_backlight_blue
                        _results.Push_SELECT_LEFT_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_SELECT_LEFT_backlight_red,
                                                                              _resultCamera.Push_SELECT_LEFT_backlight_green,
                                                                              _resultCamera.Push_SELECT_LEFT_backlight_blue)
                        _results.Push_SELECT_LEFT_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit

                        _results.Push_SELECT_LEFT_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                        '
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_SELECT_RIGHT_backlight_red,
                                                  _resultCamera.Push_SELECT_RIGHT_backlight_green,
                                                  _resultCamera.Push_SELECT_RIGHT_backlight_blue,
                                                  x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_SELECT_RIGHT_Backlight_x.Value = x
                        _results.Push_SELECT_RIGHT_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_SELECT_RIGHT_Backlight_Polygon_Axy,
                                             _results.Push_SELECT_RIGHT_Backlight_Polygon_Bxy,
                                             _results.Push_SELECT_RIGHT_Backlight_Polygon_Cxy,
                                             _results.Push_SELECT_RIGHT_Backlight_Polygon_Dxy,
                                             _results.Push_SELECT_RIGHT_Backlight_Polygon_Exy,
                                             _results.Push_SELECT_RIGHT_Backlight_Polygon_Fxy)

                        _results.Push_SELECT_RIGHT_backlight_intensity_Camera.Value = _resultCamera.Push_SELECT_RIGHT_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR

                        _results.Push_SELECT_RIGHT_backlight_intensity.Value = _recipe.Correlation_Factor_A(1).Value * Yl + _recipe.Correlation_Factor_B(1).Value

                        _results.Push_SELECT_RIGHT_backlight_red.Value = _resultCamera.Push_SELECT_RIGHT_backlight_red
                        _results.Push_SELECT_RIGHT_backlight_green.Value = _resultCamera.Push_SELECT_RIGHT_backlight_green
                        _results.Push_SELECT_RIGHT_backlight_blue.Value = _resultCamera.Push_SELECT_RIGHT_backlight_blue
                        _results.Push_SELECT_RIGHT_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_SELECT_RIGHT_backlight_red,
                                                                              _resultCamera.Push_SELECT_RIGHT_backlight_green,
                                                                              _resultCamera.Push_SELECT_RIGHT_backlight_blue)
                        _results.Push_SELECT_RIGHT_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Select_Right_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Select_Right_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Select_Right_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Select_Right_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Select_Right_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Select_Right_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Select_Right_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Select_Right_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Select_Right_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Select_Right_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Select_Right_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Select_Right_Polygon_Fxy.MaximumLimit

                        _results.Push_SELECT_RIGHT_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                    End If

                    If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                        ' UP
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_ADJUST_UP_backlight_red,
                                                  _resultCamera.Push_ADJUST_UP_backlight_green,
                                                  _resultCamera.Push_ADJUST_UP_backlight_blue,
                                                  x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_ADJUST_UP_Backlight_x.Value = x
                        _results.Push_ADJUST_UP_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_ADJUST_UP_Backlight_Polygon_Axy,
                                             _results.Push_ADJUST_UP_Backlight_Polygon_Bxy,
                                             _results.Push_ADJUST_UP_Backlight_Polygon_Cxy,
                                             _results.Push_ADJUST_UP_Backlight_Polygon_Dxy,
                                             _results.Push_ADJUST_UP_Backlight_Polygon_Exy,
                                             _results.Push_ADJUST_UP_Backlight_Polygon_Fxy)

                        _results.Push_ADJUST_UP_backlight_intensity_Camera.Value = _resultCamera.Push_ADJUST_UP_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP

                        _results.Push_ADJUST_UP_backlight_intensity.Value = _recipe.Correlation_Factor_A(3).Value * Yl + _recipe.Correlation_Factor_B(3).Value

                        _results.Push_ADJUST_UP_backlight_red.Value = _resultCamera.Push_ADJUST_UP_backlight_red
                        _results.Push_ADJUST_UP_backlight_green.Value = _resultCamera.Push_ADJUST_UP_backlight_green
                        _results.Push_ADJUST_UP_backlight_blue.Value = _resultCamera.Push_ADJUST_UP_backlight_blue
                        _results.Push_ADJUST_UP_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_ADJUST_UP_backlight_red,
                                                                              _resultCamera.Push_ADJUST_UP_backlight_green,
                                                                              _resultCamera.Push_ADJUST_UP_backlight_blue)
                        _results.Push_ADJUST_UP_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Adjust_UP_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Adjust_UP_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Adjust_UP_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Adjust_UP_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Adjust_UP_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Adjust_UP_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Adjust_UP_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Adjust_UP_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Adjust_UP_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Adjust_UP_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Adjust_UP_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Adjust_UP_Polygon_Fxy.MaximumLimit

                        _results.Push_ADJUST_UP_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                        ' Down
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_ADJUST_DOWN_backlight_red,
                       _resultCamera.Push_ADJUST_DOWN_backlight_green,
                       _resultCamera.Push_ADJUST_DOWN_backlight_blue,
                       x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_ADJUST_DOWN_Backlight_x.Value = x
                        _results.Push_ADJUST_DOWN_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_ADJUST_DOWN_Backlight_Polygon_Axy,
                                             _results.Push_ADJUST_DOWN_Backlight_Polygon_Bxy,
                                             _results.Push_ADJUST_DOWN_Backlight_Polygon_Cxy,
                                             _results.Push_ADJUST_DOWN_Backlight_Polygon_Dxy,
                                             _results.Push_ADJUST_DOWN_Backlight_Polygon_Exy,
                                             _results.Push_ADJUST_DOWN_Backlight_Polygon_Fxy)

                        _results.Push_ADJUST_DOWN_backlight_intensity_Camera.Value = _resultCamera.Push_ADJUST_DOWN_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN

                        _results.Push_ADJUST_DOWN_backlight_intensity.Value = _recipe.Correlation_Factor_A(4).Value * Yl + _recipe.Correlation_Factor_B(4).Value

                        _results.Push_ADJUST_DOWN_backlight_red.Value = _resultCamera.Push_ADJUST_DOWN_backlight_red
                        _results.Push_ADJUST_DOWN_backlight_green.Value = _resultCamera.Push_ADJUST_DOWN_backlight_green
                        _results.Push_ADJUST_DOWN_backlight_blue.Value = _resultCamera.Push_ADJUST_DOWN_backlight_blue
                        _results.Push_ADJUST_DOWN_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_ADJUST_DOWN_backlight_red,
                                                                              _resultCamera.Push_ADJUST_DOWN_backlight_green,
                                                                              _resultCamera.Push_ADJUST_DOWN_backlight_blue)
                        _results.Push_ADJUST_DOWN_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Adjust_DOWN_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Adjust_DOWN_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Adjust_DOWN_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Adjust_DOWN_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Adjust_DOWN_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Adjust_DOWN_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Adjust_DOWN_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Adjust_DOWN_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Adjust_DOWN_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Adjust_DOWN_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Adjust_DOWN_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Adjust_DOWN_Polygon_Fxy.MaximumLimit

                        _results.Push_ADJUST_DOWN_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)

                        ' Left
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_ADJUST_LEFT_backlight_red,
                      _resultCamera.Push_ADJUST_LEFT_backlight_green,
                      _resultCamera.Push_ADJUST_LEFT_backlight_blue,
                      x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_ADJUST_LEFT_Backlight_x.Value = x
                        _results.Push_ADJUST_LEFT_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_ADJUST_LEFT_Backlight_Polygon_Axy,
                                             _results.Push_ADJUST_LEFT_Backlight_Polygon_Bxy,
                                             _results.Push_ADJUST_LEFT_Backlight_Polygon_Cxy,
                                             _results.Push_ADJUST_LEFT_Backlight_Polygon_Dxy,
                                             _results.Push_ADJUST_LEFT_Backlight_Polygon_Exy,
                                             _results.Push_ADJUST_LEFT_Backlight_Polygon_Fxy)

                        _results.Push_ADJUST_LEFT_backlight_intensity_Camera.Value = _resultCamera.Push_ADJUST_LEFT_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT

                        _results.Push_ADJUST_LEFT_backlight_intensity.Value = _recipe.Correlation_Factor_A(5).Value * Yl + _recipe.Correlation_Factor_B(5).Value

                        _results.Push_ADJUST_LEFT_backlight_red.Value = _resultCamera.Push_ADJUST_LEFT_backlight_red
                        _results.Push_ADJUST_LEFT_backlight_green.Value = _resultCamera.Push_ADJUST_LEFT_backlight_green
                        _results.Push_ADJUST_LEFT_backlight_blue.Value = _resultCamera.Push_ADJUST_LEFT_backlight_blue
                        _results.Push_ADJUST_LEFT_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_ADJUST_LEFT_backlight_red,
                                                                              _resultCamera.Push_ADJUST_LEFT_backlight_green,
                                                                              _resultCamera.Push_ADJUST_LEFT_backlight_blue)
                        _results.Push_ADJUST_LEFT_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Adjust_Left_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Adjust_Left_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Adjust_Left_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Adjust_Left_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Adjust_Left_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Adjust_Left_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Adjust_Left_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Adjust_Left_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Adjust_Left_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Adjust_Left_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Adjust_Left_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Adjust_Left_Polygon_Fxy.MaximumLimit

                        _results.Push_ADJUST_LEFT_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                        ' Right
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_ADJUST_RIGHT_backlight_red,
                             _resultCamera.Push_ADJUST_RIGHT_backlight_green,
                             _resultCamera.Push_ADJUST_RIGHT_backlight_blue,
                             x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_ADJUST_RIGHT_Backlight_x.Value = x
                        _results.Push_ADJUST_RIGHT_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_ADJUST_RIGHT_Backlight_Polygon_Axy,
                                             _results.Push_ADJUST_RIGHT_Backlight_Polygon_Bxy,
                                             _results.Push_ADJUST_RIGHT_Backlight_Polygon_Cxy,
                                             _results.Push_ADJUST_RIGHT_Backlight_Polygon_Dxy,
                                             _results.Push_ADJUST_RIGHT_Backlight_Polygon_Exy,
                                             _results.Push_ADJUST_RIGHT_Backlight_Polygon_Fxy)

                        _results.Push_ADJUST_RIGHT_backlight_intensity_Camera.Value = _resultCamera.Push_ADJUST_RIGHT_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT

                        _results.Push_ADJUST_RIGHT_backlight_intensity.Value = _recipe.Correlation_Factor_A(6).Value * Yl + _recipe.Correlation_Factor_B(6).Value

                        _results.Push_ADJUST_RIGHT_backlight_red.Value = _resultCamera.Push_ADJUST_RIGHT_backlight_red
                        _results.Push_ADJUST_RIGHT_backlight_green.Value = _resultCamera.Push_ADJUST_RIGHT_backlight_green
                        _results.Push_ADJUST_RIGHT_backlight_blue.Value = _resultCamera.Push_ADJUST_RIGHT_backlight_blue
                        _results.Push_ADJUST_RIGHT_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_ADJUST_RIGHT_backlight_red,
                                                                              _resultCamera.Push_ADJUST_RIGHT_backlight_green,
                                                                              _resultCamera.Push_ADJUST_RIGHT_backlight_blue)
                        _results.Push_ADJUST_RIGHT_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Adjust_Right_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Adjust_Right_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Adjust_Right_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Adjust_Right_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Adjust_Right_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Adjust_Right_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Adjust_Right_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Adjust_Right_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Adjust_Right_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Adjust_Right_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Adjust_Right_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Adjust_Right_Polygon_Fxy.MaximumLimit

                        _results.Push_ADJUST_RIGHT_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                    End If

                    If CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) Then
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_FOLDING_backlight_red,
                                 _resultCamera.Push_FOLDING_backlight_green,
                                 _resultCamera.Push_FOLDING_backlight_blue,
                                 x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_FOLDING_Backlight_x.Value = x
                        _results.Push_FOLDING_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_FOLDING_Backlight_Polygon_Axy,
                                             _results.Push_FOLDING_Backlight_Polygon_Bxy,
                                             _results.Push_FOLDING_Backlight_Polygon_Cxy,
                                             _results.Push_FOLDING_Backlight_Polygon_Dxy,
                                             _results.Push_FOLDING_Backlight_Polygon_Exy,
                                             _results.Push_FOLDING_Backlight_Polygon_Fxy)

                        _results.Push_FOLDING_backlight_intensity_Camera.Value = _resultCamera.Push_FOLDING_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR

                        _results.Push_FOLDING_backlight_intensity.Value = _recipe.Correlation_Factor_A(2).Value * Yl + _recipe.Correlation_Factor_B(2).Value

                        _results.Push_FOLDING_backlight_red.Value = _resultCamera.Push_FOLDING_backlight_red
                        _results.Push_FOLDING_backlight_green.Value = _resultCamera.Push_FOLDING_backlight_green
                        _results.Push_FOLDING_backlight_blue.Value = _resultCamera.Push_FOLDING_backlight_blue
                        _results.Push_FOLDING_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_FOLDING_backlight_red,
                                                                              _resultCamera.Push_FOLDING_backlight_green,
                                                                              _resultCamera.Push_FOLDING_backlight_blue)
                        _results.Push_FOLDING_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Folding_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Folding_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Folding_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Folding_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Folding_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Folding_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Folding_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Folding_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Folding_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Folding_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Folding_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Folding_Polygon_Fxy.MaximumLimit

                        _results.Push_FOLDING_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                    End If

                    If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Then
                        mColor_Conversion.RGB_xyYl(_resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_red,
                                 _resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_green,
                                 _resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_blue,
                                 x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_x.Value = x
                        _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy,
                                             _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy,
                                             _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy,
                                             _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy,
                                             _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy,
                                             _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy)

                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity_Camera.Value = _resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT

                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.Value = _recipe.Correlation_Factor_A(7).Value * Yl + _recipe.Correlation_Factor_B(7).Value

                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_red.Value = _resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_red
                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_green.Value = _resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_green
                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_blue.Value = _resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_blue
                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_red,
                                                                              _resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_green,
                                                                              _resultCamera.WINDOWS_LIFTER_FRONT_LEFT_backlight_blue)
                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Windows_Lifter_Front_Left_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Windows_Lifter_Front_Left_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Windows_Lifter_Front_Left_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Windows_Lifter_Front_Left_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Windows_Lifter_Front_Left_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Windows_Lifter_Front_Left_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Windows_Lifter_Front_Left_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Windows_Lifter_Front_Left_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Windows_Lifter_Front_Left_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Windows_Lifter_Front_Left_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Windows_Lifter_Front_Left_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Windows_Lifter_Front_Left_Polygon_Fxy.MaximumLimit

                        _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)

                        mColor_Conversion.RGB_xyYl(_resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_red,
                                 _resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_green,
                                 _resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue,
                                 x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_x.Value = x
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy,
                                             _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy,
                                             _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy,
                                             _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy,
                                             _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy,
                                             _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy)

                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity_Camera.Value = _resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT

                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.Value = _recipe.Correlation_Factor_A(8).Value * Yl + _recipe.Correlation_Factor_B(8).Value

                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_red.Value = _resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_red
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_green.Value = _resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_green
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue.Value = _resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_red,
                                                                              _resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_green,
                                                                              _resultCamera.WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue)
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Windows_Lifter_Front_Right_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Windows_Lifter_Front_Right_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Windows_Lifter_Front_Right_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Windows_Lifter_Front_Right_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Windows_Lifter_Front_Right_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Windows_Lifter_Front_Right_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Windows_Lifter_Front_Right_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Windows_Lifter_Front_Right_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Windows_Lifter_Front_Right_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Windows_Lifter_Front_Right_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Windows_Lifter_Front_Right_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Windows_Lifter_Front_Right_Polygon_Fxy.MaximumLimit

                        _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                    End If
                    If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Then
                        mColor_Conversion.RGB_xyYl(_resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_red,
                                 _resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_green,
                                 _resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_blue,
                                 x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_x.Value = x
                        _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy,
                                             _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy,
                                             _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy,
                                             _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy,
                                             _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy,
                                             _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy)

                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity_Camera.Value = _resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT

                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.Value = _recipe.Correlation_Factor_A(9).Value * Yl + _recipe.Correlation_Factor_B(9).Value

                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_red.Value = _resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_red
                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_green.Value = _resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_green
                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_blue.Value = _resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_blue
                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_red,
                                                                              _resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_green,
                                                                              _resultCamera.WINDOWS_LIFTER_REAR_LEFT_backlight_blue)
                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Windows_Lifter_Rear_Left_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Windows_Lifter_Rear_Left_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Windows_Lifter_Rear_Left_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Windows_Lifter_Rear_Left_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Windows_Lifter_Rear_Left_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Windows_Lifter_Rear_Left_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Windows_Lifter_Rear_Left_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Windows_Lifter_Rear_Left_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Windows_Lifter_Rear_Left_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Windows_Lifter_Rear_Left_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Windows_Lifter_Rear_Left_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Windows_Lifter_Rear_Left_Polygon_Fxy.MaximumLimit

                        _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)

                        mColor_Conversion.RGB_xyYl(_resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_red,
                                 _resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_green,
                                 _resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_blue,
                                 x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_x.Value = x
                        _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy,
                                             _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy,
                                             _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy,
                                             _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy,
                                             _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy,
                                             _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy)

                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity_Camera.Value = _resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT

                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.Value = _recipe.Correlation_Factor_A(10).Value * Yl + _recipe.Correlation_Factor_B(10).Value

                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_red.Value = _resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_red
                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_green.Value = _resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_green
                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_blue.Value = _resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_blue
                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_red,
                                                                              _resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_green,
                                                                              _resultCamera.WINDOWS_LIFTER_REAR_RIGHT_backlight_blue)
                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Windows_Lifter_Rear_Right_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Windows_Lifter_Rear_Right_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Windows_Lifter_Rear_Right_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Windows_Lifter_Rear_Right_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Windows_Lifter_Rear_Right_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Windows_Lifter_Rear_Right_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Windows_Lifter_Rear_Right_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Windows_Lifter_Rear_Right_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Windows_Lifter_Rear_Right_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Windows_Lifter_Rear_Right_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Windows_Lifter_Rear_Right_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Windows_Lifter_Rear_Right_Polygon_Fxy.MaximumLimit

                        _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                    End If
                    If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) Then
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_CHILDREN_LOCK_backlight_red,
                                 _resultCamera.Push_CHILDREN_LOCK_backlight_green,
                                 _resultCamera.Push_CHILDREN_LOCK_backlight_blue,
                                 x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_CHILDREN_LOCK_Backlight_x.Value = x
                        _results.Push_CHILDREN_LOCK_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_CHILDREN_LOCK_Backlight_Polygon_Axy,
                                             _results.Push_CHILDREN_LOCK_Backlight_Polygon_Bxy,
                                             _results.Push_CHILDREN_LOCK_Backlight_Polygon_Cxy,
                                             _results.Push_CHILDREN_LOCK_Backlight_Polygon_Dxy,
                                             _results.Push_CHILDREN_LOCK_Backlight_Polygon_Exy,
                                             _results.Push_CHILDREN_LOCK_Backlight_Polygon_Fxy)

                        _results.Push_CHILDREN_LOCK_backlight_intensity_Camera.Value = _resultCamera.Push_CHILDREN_LOCK_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK

                        _results.Push_CHILDREN_LOCK_backlight_intensity.Value = _recipe.Correlation_Factor_A(11).Value * Yl + _recipe.Correlation_Factor_B(11).Value

                        _results.Push_CHILDREN_LOCK_backlight_red.Value = _resultCamera.Push_CHILDREN_LOCK_backlight_red
                        _results.Push_CHILDREN_LOCK_backlight_green.Value = _resultCamera.Push_CHILDREN_LOCK_backlight_green
                        _results.Push_CHILDREN_LOCK_backlight_blue.Value = _resultCamera.Push_CHILDREN_LOCK_backlight_blue
                        _results.Push_CHILDREN_LOCK_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_CHILDREN_LOCK_backlight_red,
                                                                              _resultCamera.Push_CHILDREN_LOCK_backlight_green,
                                                                              _resultCamera.Push_CHILDREN_LOCK_backlight_blue)
                        _results.Push_CHILDREN_LOCK_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Children_Lock_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Children_Lock_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Children_Lock_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Children_Lock_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Children_Lock_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Children_Lock_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Children_Lock_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Children_Lock_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Children_Lock_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Children_Lock_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Children_Lock_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Children_Lock_Polygon_Fxy.MaximumLimit

                        _results.Push_CHILDREN_LOCK_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                    End If
                    If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock2.Value) Then
                        mColor_Conversion.RGB_xyYl(_resultCamera.Push_CHILDREN_LOCK2_backlight_red,
                                 _resultCamera.Push_CHILDREN_LOCK2_backlight_green,
                                 _resultCamera.Push_CHILDREN_LOCK2_backlight_blue,
                                 x, y, Yl)
                        x = x * _recipe.Correlation_BackColorx_Factor_A.Value + _recipe.Correlation_BackColorx_Factor_B.Value
                        y = y * _recipe.Correlation_BackColory_Factor_A.Value + _recipe.Correlation_BackColory_Factor_B.Value
                        _results.Push_CHILDREN_LOCK2_Backlight_x.Value = x
                        _results.Push_CHILDREN_LOCK2_Backlight_y.Value = y
                        CheckColorCoordinate(x, y,
                                             _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Axy,
                                             _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Bxy,
                                             _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Cxy,
                                             _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Dxy,
                                             _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Exy,
                                             _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Fxy)

                        _results.Push_CHILDREN_LOCK2_backlight_intensity_Camera.Value = _resultCamera.Push_CHILDREN_LOCK2_backlight_intensity
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2.Value = _resultCamera.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2

                        _results.Push_CHILDREN_LOCK2_backlight_intensity.Value = _recipe.Correlation_Factor_A(11).Value * Yl + _recipe.Correlation_Factor_B(11).Value

                        _results.Push_CHILDREN_LOCK2_backlight_red.Value = _resultCamera.Push_CHILDREN_LOCK2_backlight_red
                        _results.Push_CHILDREN_LOCK2_backlight_green.Value = _resultCamera.Push_CHILDREN_LOCK2_backlight_green
                        _results.Push_CHILDREN_LOCK2_backlight_blue.Value = _resultCamera.Push_CHILDREN_LOCK2_backlight_blue
                        _results.Push_CHILDREN_LOCK2_backlight_Saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_CHILDREN_LOCK2_backlight_red,
                                                                              _resultCamera.Push_CHILDREN_LOCK2_backlight_green,
                                                                              _resultCamera.Push_CHILDREN_LOCK2_backlight_blue)
                        _results.Push_CHILDREN_LOCK2_backlight_DominantWavelenght.Value = mColor_Analysis.Dominant_Wavelength(x, y)

                        xpoints(0) = _recipe.Push_Children_Lock2_Polygon_Axy.MinimumLimit
                        ypoints(0) = _recipe.Push_Children_Lock2_Polygon_Axy.MaximumLimit
                        xpoints(1) = _recipe.Push_Children_Lock2_Polygon_Bxy.MinimumLimit
                        ypoints(1) = _recipe.Push_Children_Lock2_Polygon_Bxy.MaximumLimit
                        xpoints(2) = _recipe.Push_Children_Lock2_Polygon_Cxy.MinimumLimit
                        ypoints(2) = _recipe.Push_Children_Lock2_Polygon_Cxy.MaximumLimit
                        xpoints(3) = _recipe.Push_Children_Lock2_Polygon_Dxy.MinimumLimit
                        ypoints(3) = _recipe.Push_Children_Lock2_Polygon_Dxy.MaximumLimit
                        xpoints(4) = _recipe.Push_Children_Lock2_Polygon_Exy.MinimumLimit
                        ypoints(4) = _recipe.Push_Children_Lock2_Polygon_Exy.MaximumLimit
                        xpoints(5) = _recipe.Push_Children_Lock2_Polygon_Fxy.MinimumLimit
                        ypoints(5) = _recipe.Push_Children_Lock2_Polygon_Fxy.MaximumLimit

                        _results.Push_CHILDREN_LOCK2_Backlight_RSQ.Value = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
                    End If


                    'Next
                    ' Go to next subphase
                    _subPhase(_phase) = 50
                    ' Store the time
                    t0 = Date.Now
                ElseIf (r = 1 And (Date.Now - t0).TotalSeconds > 5) Then    ' Otherwise, if the camera results were not received within 2 s
                    AddLogEntry("Timeout in camera result reception")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    previousSubphase = 22
                    _subPhase(_phase) = 199
                ElseIf (r = -1) Then    ' Otherwise, if some error happened
                    AddLogEntry("Runtime error in camera result reception")
                    ' Sets the error flag
                    e = True
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    previousSubphase = 22
                    _subPhase(_phase) = 199
                End If

            Case 50
                If Not _recipe.TestEnable_EV_Option.Value Then
                    ' Switch-off the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF),
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
                    _subPhase(_phase) = 52
                Else
                    ' Switch-off the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF_EV),
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
                    _subPhase(_phase) = 51
                End If

            Case 51
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    ' Switch-off the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF2_EV),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 52 '3
                End If

            Case 52
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_All_OFF))
                If (i <> -1) Then
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
                End If

            Case 100
                ' Test the values
                ' Mirror selection backlight test

                If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                    If (_results.Push_SELECT_LEFT_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_SELECT_LEFT_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_SELECT_LEFT_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or 'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_SELECT_RIGHT_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_SELECT_RIGHT_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_SELECT_RIGHT_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or 'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                ' Mirror adjustment backlight test
                If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                    If (_results.Push_ADJUST_UP_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_UP_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_UP_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_DOWN_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_DOWN_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_DOWN_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_LEFT_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_LEFT_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_LEFT_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_RIGHT_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_RIGHT_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_RIGHT_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                ' Mirror folding backlight test
                If CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) Then
                    If (_results.Push_FOLDING_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_FOLDING_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_FOLDING_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                ' Window lift backlight test
                If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Then
                    If (_results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Then
                    If (_results.WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) Then
                    If (_results.Push_CHILDREN_LOCK_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_CHILDREN_LOCK_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_CHILDREN_LOCK_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If
                If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock2.Value) Then
                    If (_results.Push_CHILDREN_LOCK2_Backlight_RSQ.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_CHILDREN_LOCK2_backlight_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                        _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2.Test <> cResultValue.eValueTestResult.Passed) And
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Passed
                End If

                If _results.eAreaMirror_Conformity.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eAreaMirror_Conformity.TestResult = cResultValue.eValueTestResult.Passed
                End If

                If _results.eAreaWili_Conformity.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eAreaWili_Conformity.TestResult = cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                If _stepByStep = False Or (_stepByStep = True And _step = True) Then
                    _step = False
                    AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                    ' Updates the global test result
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                           ((_results.eBACKLIGHT_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed))) Then
                        If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                            If _results.Push_SELECT_LEFT_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.Push_SELECT_LEFT_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                 _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush1_Backlight
                            ElseIf _results.Push_SELECT_RIGHT_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                     _results.Push_SELECT_RIGHT_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                     _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush2_Backlight
                            ElseIf _results.Push_ADJUST_UP_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.Push_ADJUST_UP_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush4_Backlight
                            ElseIf _results.Push_ADJUST_DOWN_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.Push_ADJUST_DOWN_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush5_Backlight
                            ElseIf _results.Push_ADJUST_LEFT_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.Push_ADJUST_LEFT_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush6_Backlight
                            ElseIf _results.Push_ADJUST_RIGHT_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.Push_ADJUST_RIGHT_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush7_Backlight
                            End If
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                            (_results.eBACKLIGHT_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                        If (CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) And
                            (_results.Push_FOLDING_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                             _results.Push_FOLDING_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                            _results.TestResult = cWS02Results.eTestResult.FailedPush3_Backlight
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                            (_results.eBACKLIGHT_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                        If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Then
                            If _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush8_Backlight
                            ElseIf _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush9_Backlight
                            End If
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                                (_results.eBACKLIGHT_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                        If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Then
                            If _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush10_Backlight
                            ElseIf _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush11_Backlight
                            End If
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                                    (_results.eBACKLIGHT_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                        If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) Then
                            If _results.Push_CHILDREN_LOCK_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                 _results.Push_CHILDREN_LOCK_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush12_Backlight
                            End If
                        End If
                    End If
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                                    (_results.eBACKLIGHT_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                        If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock2.Value) Then
                            If _results.Push_CHILDREN_LOCK2_Backlight_RSQ.TestResult <> cResultValue.eValueTestResult.Passed Or
                                 _results.Push_CHILDREN_LOCK2_backlight_intensity.TestResult <> cResultValue.eValueTestResult.Passed Or
                                _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2.TestResult <> cResultValue.eValueTestResult.Passed Then
                                _results.TestResult = cWS02Results.eTestResult.FailedPush13_Backlight
                            End If
                        End If
                    End If
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.HOMOGENEITY
                End If

            Case 199
                _results.eBACKLIGHT_CONFORMITY.TestResult =
                        cResultValue.eValueTestResult.Failed
                _results.eAreaMirror_Conformity.TestResult =
                        cResultValue.eValueTestResult.Failed
                _results.eAreaWili_Conformity.TestResult =
                        cResultValue.eValueTestResult.Failed
                ' Go to next subphase
                _subPhase(_phase) = 101 ' 51


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

    Private Sub CheckColorCoordinate(ByVal x As Single, ByVal y As Single, ByVal ParamArray polygon() As cResultValue)

        If polygon.Count < 3 Then
            Return
        End If
        Dim xpoints(polygon.Length - 1) As Single
        Dim ypoints(polygon.Length - 1) As Single

        For index = 0 To polygon.Length - 1
            xpoints(index) = polygon(index).MinimumLimit
            ypoints(index) = polygon(index).MaximumLimit
        Next

        Dim insidePoly As Boolean = False
        insidePoly = mColor_Analysis.InsidePolygon(x, y, xpoints, ypoints)
        AddLogEntry("insidePoly:" + insidePoly.ToString() + "x:" + x.ToString() + "y:" + y.ToString())
        'If (insidePoly) Then
        If (True) Then
            For index = 0 To polygon.Length - 1
                polygon(index).TestResult = cResultValue.eValueTestResult.Passed
            Next
        Else
            For index = 0 To polygon.Length - 1
                polygon(index).TestResult = cResultValue.eValueTestResult.Failed
            Next
        End If
    End Sub

    Private Sub PhaseHomogeneity()
        Dim e As Boolean
        Dim sp As Integer
        Dim maxIntensityPaddel As Single
        Dim maxIntensityWili As Single
        Dim maxIntensityMirror As Single

        Dim i As Integer
        Static t0 As Date
        Static t0Phase As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Push Homogeneity Camera Test")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1
                Else
                    ' Add a log entry
                    AddLogEntry("Push Homogeneity Camera test is Disabled")
                    ' Go to next phase
                    _phase = ePhase.TELLTALE
                End If

            Case 1
                maxIntensityMirror = 0
                maxIntensityPaddel = 0
                maxIntensityWili = 0
                If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                    If maxIntensityMirror < _results.Push_SELECT_LEFT_backlight_intensity.Value Then
                        maxIntensityMirror = _results.Push_SELECT_LEFT_backlight_intensity.Value
                    End If
                    If maxIntensityMirror < _results.Push_SELECT_RIGHT_backlight_intensity.Value Then
                        maxIntensityMirror = _results.Push_SELECT_RIGHT_backlight_intensity.Value
                    End If
                End If
                If CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) Then
                    If maxIntensityMirror < _results.Push_FOLDING_backlight_intensity.Value Then
                        maxIntensityMirror = _results.Push_FOLDING_backlight_intensity.Value
                    End If
                End If

                If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                    If maxIntensityPaddel < _results.Push_ADJUST_UP_backlight_intensity.Value Then
                        maxIntensityPaddel = _results.Push_ADJUST_UP_backlight_intensity.Value
                    End If
                    If maxIntensityPaddel < _results.Push_ADJUST_DOWN_backlight_intensity.Value Then
                        maxIntensityPaddel = _results.Push_ADJUST_DOWN_backlight_intensity.Value
                    End If
                    If maxIntensityPaddel < _results.Push_ADJUST_LEFT_backlight_intensity.Value Then
                        maxIntensityPaddel = _results.Push_ADJUST_LEFT_backlight_intensity.Value
                    End If
                    If maxIntensityPaddel < _results.Push_ADJUST_RIGHT_backlight_intensity.Value Then
                        maxIntensityPaddel = _results.Push_ADJUST_RIGHT_backlight_intensity.Value
                    End If
                End If

                If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Then
                    If maxIntensityWili < _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.Value Then
                        maxIntensityWili = _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.Value
                    End If
                    If maxIntensityWili < _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.Value Then
                        maxIntensityWili = _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.Value
                    End If
                End If
                If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Then
                    If maxIntensityWili < _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.Value Then
                        maxIntensityWili = _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.Value
                    End If
                    If maxIntensityWili < _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.Value Then
                        maxIntensityWili = _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.Value
                    End If
                End If
                'If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) Then
                '    If maxIntensity < _results.Push_CHILDREN_LOCK_backlight_intensity.Value Then
                '        maxIntensity = _results.Push_CHILDREN_LOCK_backlight_intensity.Value
                '    End If
                'End If

                If (maxIntensityMirror > 0) Then
                    ' Push backlight homogeneity
                    If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                        _results.Push_SELECT_LEFT_backlight_homogeneity.Value = _results.Push_SELECT_LEFT_backlight_intensity.Value / maxIntensityMirror * 100
                        _results.Push_SELECT_RIGHT_backlight_homogeneity.Value = _results.Push_SELECT_RIGHT_backlight_intensity.Value / maxIntensityMirror * 100
                    End If
                    If CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) Then
                        _results.Push_FOLDING_backlight_homogeneity.Value = _results.Push_FOLDING_backlight_intensity.Value / maxIntensityMirror * 100
                    End If
                Else
                    If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                        _results.Push_SELECT_LEFT_backlight_homogeneity.Value = 0
                        _results.Push_SELECT_RIGHT_backlight_homogeneity.Value = 0
                    End If
                    If CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) Then
                        _results.Push_FOLDING_backlight_homogeneity.Value = 0
                    End If
                End If
                If (maxIntensityPaddel > 0) Then
                    If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                        _results.Push_ADJUST_UP_backlight_homogeneity.Value = _results.Push_ADJUST_UP_backlight_intensity.Value / maxIntensityPaddel * 100
                        _results.Push_ADJUST_DOWN_backlight_homogeneity.Value = _results.Push_ADJUST_DOWN_backlight_intensity.Value / maxIntensityPaddel * 100
                        _results.Push_ADJUST_LEFT_backlight_homogeneity.Value = _results.Push_ADJUST_LEFT_backlight_intensity.Value / maxIntensityPaddel * 100
                        _results.Push_ADJUST_RIGHT_backlight_homogeneity.Value = _results.Push_ADJUST_RIGHT_backlight_intensity.Value / maxIntensityPaddel * 100
                    End If
                Else
                    If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                        _results.Push_ADJUST_UP_backlight_homogeneity.Value = 0
                        _results.Push_ADJUST_DOWN_backlight_homogeneity.Value = 0
                        _results.Push_ADJUST_LEFT_backlight_homogeneity.Value = 0
                        _results.Push_ADJUST_RIGHT_backlight_homogeneity.Value = 0
                    End If
                End If
                If (maxIntensityWili > 0) Then
                    If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Then
                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity.Value = _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.Value / maxIntensityWili * 100
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity.Value = _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.Value / maxIntensityWili * 100
                    End If
                    If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Then
                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity.Value = _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.Value / maxIntensityWili * 100
                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity.Value = _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.Value / maxIntensityWili * 100
                    End If
                Else
                    If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Then
                        _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity.Value = 0
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity.Value = 0
                    End If
                    If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Then
                        _results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity.Value = 0
                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity.Value = 0
                    End If
                End If
                'If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) Then
                '    '    _results.Push_CHILDREN_LOCK_backlight_homogeneity.Value = _results.Push_CHILDREN_LOCK_backlight_intensity.Value / maxIntensity * 100
                'Else
                '    If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) Then
                '        _results.Push_CHILDREN_LOCK_backlight_homogeneity.Value = 0
                '    End If
                'End If
                ' Go to next subphase
                _subPhase(_phase) = 100
                ' Store the time
                t0 = Date.Now


            Case 100
                ' Tests Push 
                ' Push backlight homogeneity
                If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                    If _results.Push_SELECT_LEFT_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Then
                        _results.eBACKLIGHT_HOMOGENEITY.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                    If _results.Push_SELECT_RIGHT_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Then
                        _results.eBACKLIGHT_HOMOGENEITY.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                End If
                If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
                    If _results.Push_ADJUST_UP_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_DOWN_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_LEFT_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.Push_ADJUST_RIGHT_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Then
                        _results.eBACKLIGHT_HOMOGENEITY.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                End If
                If CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) Then
                    If _results.Push_FOLDING_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Then
                        _results.eBACKLIGHT_HOMOGENEITY.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                End If
                If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Then
                    If _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Then
                        _results.eBACKLIGHT_HOMOGENEITY.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                End If
                If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Then
                    If _results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Then
                        _results.eBACKLIGHT_HOMOGENEITY.TestResult =
                            cResultValue.eValueTestResult.Failed
                    End If
                End If
                'If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) Then
                '    If _results.Push_CHILDREN_LOCK_backlight_homogeneity.Test <> cResultValue.eValueTestResult.Passed Then
                '        _results.ePUSH_HOMOGENEITY.TestResult = _
                '            cResultValue.eValueTestResult.Failed
                '    End If
                'End If

                If _results.eBACKLIGHT_HOMOGENEITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eBACKLIGHT_HOMOGENEITY.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) And
                    _results.Push_SELECT_LEFT_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush1_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) And
                    _results.Push_SELECT_RIGHT_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush2_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) And
                    _results.Push_ADJUST_UP_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush4_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) And
                    _results.Push_ADJUST_DOWN_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush5_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) And
                    _results.Push_ADJUST_LEFT_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush6_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) And
                    _results.Push_ADJUST_RIGHT_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush7_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) And
                    _results.Push_FOLDING_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush2_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) And
                    _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush8_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) And
                    _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush9_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) And
                    _results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush10_Homogeneity
                ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    (CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) And
                    _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedPush11_Homogeneity
                    'ElseIf (_results.TestResult = cWS02Results.eTestResult.Unknown And _
                    '    (CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) And _
                    '    _results.Push_CHILDREN_LOCK_backlight_homogeneity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                    '    _results.TestResult = cWS02Results.eTestResult.FailedPush12_Homogeneity
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.TELLTALE

            Case 199
                ' Update the test result
                If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                    _results.eBACKLIGHT_HOMOGENEITY.TestResult =
                        cResultValue.eValueTestResult.Failed
                End If
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


    Private Sub PhaseTellTale()
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Dim r As Integer
        Dim f As CLINFrame
        Dim x As Single
        Dim y As Single
        Dim Yl As Single
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
                If CBool(_recipe.TestEnable_TELLTALE_SelectMirror.Value) Or
                    CBool(_recipe.TestEnable_TELLTALE_ChildrenLock.Value) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Push TellTale Camera Test")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1
                Else
                    ' Add a log entry
                    AddLogEntry("Push TellTale Camera test is Disabled")
                    ' Go to next phase
                    _phase = ePhase.PWMOutput_0
                End If

            Case 1
                If Not _recipe.TestEnable_EV_Option.Value Then
                    ' Set Backlight Product
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TellTale),
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
                    _subPhase(_phase) = 3
                Else
                    ' Set Backlight Product
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TellTale_EV),
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
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    ' Set Backlight Product
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TellTale2_EV),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 20
                End If

            Case 3
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_TellTale))
                If (i <> -1) Then
                    ' 
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 20
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 20
                ElseIf ((Date.Now - tLin).TotalMilliseconds > 200) Then
                    '' Set Backlight Product
                    '' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                    'e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TellTale), _
                    '                                    True, _
                    '                                    txData_MasterReq, _
                    '                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
                    '                                    True, _
                    '                                    True)
                    '' Store the time
                    'tLin = Date.Now
                End If


            Case 20
                If ((mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_RUN) And
                    Not mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_Busy) And
                    mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_TrigReady)) And
                     ((Date.Now - t0).TotalMilliseconds > TriggerDelay_ms) And
                     (Not (_stepByStep) Or (_stepByStep And _step))) Or
                    TestMode = eTestMode.Debug Then
                    'Clear the step flag
                    _step = False
                    mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Keyence_Trig1)
                    ' Set Trigger
                    AddLogEntry("Set Trigger Telltale")
                    ' Go to next subphase
                    _subPhase(_phase) = 21
                    ' Store the time
                    t0 = Date.Now
                End If

            Case 21
                ' If the camera is busy
                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS02_Keyence_Busy)) Or
                 ((Date.Now - t0).TotalMilliseconds > 200) Then
                    mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Keyence_Trig1)
                    ' Reset Trigger
                    AddLogEntry("ReSet Trigger Telltale")
                    ' Go to next subphase
                    _subPhase(_phase) = 22
                    ' Store the time
                    t0 = Date.Now
                    '
                End If

            Case 22
                ' If the keyence  camera results were received
                r = mWS02Keyence.ReadResultsFHM(_resultCamera)
                If (r = 0) Then
                    For i = 0 To 1
                        If CBool(_recipe.TestEnable_TELLTALE_SelectMirror.Value) Then
                            mColor_Conversion.RGB_xyYl(_resultCamera.Push_SELECT_LEFT_LED_red,
                                                  _resultCamera.Push_SELECT_LEFT_LED_green,
                                                  _resultCamera.Push_SELECT_LEFT_LED_blue,
                                                  x, y, Yl)
                            x = x * _recipe.Correlation_TellColorx_Factor_A.Value + _recipe.Correlation_TellColorx_Factor_B.Value
                            y = y * _recipe.Correlation_TellColory_Factor_A.Value + _recipe.Correlation_TellColory_Factor_B.Value
                            _results.Push_SELECT_LEFT_LED_x.Value = x
                            _results.Push_SELECT_LEFT_LED_y.Value = y
                            CheckColorCoordinate(x, y,
                                                 _results.Push_SELECT_LEFT_LED_Polygon_Axy,
                                                 _results.Push_SELECT_LEFT_LED_Polygon_Bxy,
                                                 _results.Push_SELECT_LEFT_LED_Polygon_Cxy,
                                                 _results.Push_SELECT_LEFT_LED_Polygon_Dxy)

                            _results.Push_SELECT_LEFT_LED_intensity.Value = _recipe.Correlation_Factor_A(12).Value * Yl + _recipe.Correlation_Factor_B(12).Value
                            _results.Push_SELECT_LEFT_LED_red.Value = _resultCamera.Push_SELECT_LEFT_LED_red
                            _results.Push_SELECT_LEFT_LED_green.Value = _resultCamera.Push_SELECT_LEFT_LED_green
                            _results.Push_SELECT_LEFT_LED_blue.Value = _resultCamera.Push_SELECT_LEFT_LED_blue
                            _results.Push_SELECT_LEFT_LED_WaveLenght.Value = mColor_Analysis.Dominant_Wavelength(x, y) * _recipe.Correlation_TellWavelength_Factor_A.Value + _recipe.Correlation_TellWavelength_Factor_B.Value
                            _results.Push_SELECT_LEFT_LED_saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_SELECT_LEFT_LED_red,
                                                                              _resultCamera.Push_SELECT_LEFT_LED_green,
                                                                              _resultCamera.Push_SELECT_LEFT_LED_blue)
                            _results.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR.Value = _resultCamera.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR
                        End If
                        If CBool(_recipe.TestEnable_TELLTALE_SelectMirror.Value) Then
                            mColor_Conversion.RGB_xyYl(_resultCamera.Push_SELECT_RIGHT_LED_red,
                                                  _resultCamera.Push_SELECT_RIGHT_LED_green,
                                                  _resultCamera.Push_SELECT_RIGHT_LED_blue,
                                                  x, y, Yl)
                            x = x * _recipe.Correlation_TellColorx_Factor_A.Value + _recipe.Correlation_TellColorx_Factor_B.Value
                            y = y * _recipe.Correlation_TellColory_Factor_A.Value + _recipe.Correlation_TellColory_Factor_B.Value
                            _results.Push_SELECT_RIGHT_LED_x.Value = x
                            _results.Push_SELECT_RIGHT_LED_y.Value = y
                            CheckColorCoordinate(x, y,
                                                 _results.Push_SELECT_RIGHT_LED_Polygon_Axy,
                                                 _results.Push_SELECT_RIGHT_LED_Polygon_Bxy,
                                                 _results.Push_SELECT_RIGHT_LED_Polygon_Cxy,
                                                 _results.Push_SELECT_RIGHT_LED_Polygon_Dxy)

                            _results.Push_SELECT_RIGHT_LED_intensity.Value = _recipe.Correlation_Factor_A(13).Value * Yl + _recipe.Correlation_Factor_B(13).Value
                            _results.Push_SELECT_RIGHT_LED_red.Value = _resultCamera.Push_SELECT_RIGHT_LED_red
                            _results.Push_SELECT_RIGHT_LED_green.Value = _resultCamera.Push_SELECT_RIGHT_LED_green
                            _results.Push_SELECT_RIGHT_LED_blue.Value = _resultCamera.Push_SELECT_RIGHT_LED_blue
                            _results.Push_SELECT_RIGHT_LED_WaveLenght.Value = mColor_Analysis.Dominant_Wavelength(x, y) * _recipe.Correlation_TellWavelength_Factor_A.Value + _recipe.Correlation_TellWavelength_Factor_B.Value
                            _results.Push_SELECT_RIGHT_LED_saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_SELECT_RIGHT_LED_red,
                                                                                                              _resultCamera.Push_SELECT_RIGHT_LED_green,
                                                                                                              _resultCamera.Push_SELECT_RIGHT_LED_blue)
                            _results.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR.Value = _resultCamera.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR
                        End If
                        If CBool(_recipe.TestEnable_TELLTALE_ChildrenLock.Value) Then
                            mColor_Conversion.RGB_xyYl(_resultCamera.Push_CHILDREN_LOCK_LED_red,
                                                        _resultCamera.Push_CHILDREN_LOCK_LED_green,
                                                        _resultCamera.Push_CHILDREN_LOCK_LED_blue,
                                                        x, y, Yl)
                            x = x * _recipe.Correlation_TellColorx_Factor_A.Value + _recipe.Correlation_TellColorx_Factor_B.Value
                            y = y * _recipe.Correlation_TellColory_Factor_A.Value + _recipe.Correlation_TellColory_Factor_B.Value
                            _results.Push_CHILDREN_LOCK_LED_x.Value = x
                            _results.Push_CHILDREN_LOCK_LED_y.Value = y
                            CheckColorCoordinate(x, y,
                                                 _results.Push_CHILDREN_LOCK_LED_Polygon_Axy,
                                                 _results.Push_CHILDREN_LOCK_LED_Polygon_Bxy,
                                                 _results.Push_CHILDREN_LOCK_LED_Polygon_Cxy,
                                                 _results.Push_CHILDREN_LOCK_LED_Polygon_Dxy)

                            _results.Push_CHILDREN_LOCK_LED_intensity.Value = _recipe.Correlation_Factor_A(14).Value * Yl + _recipe.Correlation_Factor_B(14).Value
                            _results.Push_CHILDREN_LOCK_LED_red.Value = _resultCamera.Push_CHILDREN_LOCK_LED_red
                            _results.Push_CHILDREN_LOCK_LED_green.Value = _resultCamera.Push_CHILDREN_LOCK_LED_green
                            _results.Push_CHILDREN_LOCK_LED_blue.Value = _resultCamera.Push_CHILDREN_LOCK_LED_blue
                            _results.Push_CHILDREN_LOCK_LED_WaveLenght.Value = mColor_Analysis.Dominant_Wavelength(x, y) * _recipe.Correlation_TellWavelength_Factor_A.Value + _recipe.Correlation_TellWavelength_Factor_B.Value
                            _results.Push_CHILDREN_LOCK_LED_saturation.Value = mColor_Conversion.RGB_S(_resultCamera.Push_CHILDREN_LOCK_LED_red,
                                                                                                              _resultCamera.Push_CHILDREN_LOCK_LED_green,
                                                                                                              _resultCamera.Push_CHILDREN_LOCK_LED_blue)
                            _results.DEFECT_AREA_TELLTALE_CHILDREN_LOCK.Value = _resultCamera.DEFECT_AREA_TELLTALE_CHILDREN_LOCK
                        End If
                    Next i
                    ' Goes to the next subphase
                    _subPhase(_phase) = 50
                    ' Store the time
                    t0 = Date.Now
                    ' Otherwise, if the keyence 1 camera results were not received and the camera results timeout expired
                ElseIf (r = 1 And (Date.Now - t0).TotalMilliseconds > 2000) Then

                    ' Adds a log entry
                    AddLogEntry("Timeout results Keyence ")
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                    ' Store the time
                    t0 = Date.Now
                ElseIf (r = -1) Then
                    t0 = Date.Now
                    ' Goes to the next subphase ' TMP gestione errori
                    _subPhase(_phase) = 199
                End If

            Case 50
                If Not _recipe.TestEnable_EV_Option.Value Then
                    ' Set Backlight Product
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF),
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
                    _subPhase(_phase) = 52
                Else
                    ' Set Backlight Product
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF_EV),
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
                    _subPhase(_phase) = 51
                End If

            Case 51
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    ' Set Backlight Product
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_All_OFF2_EV),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 52
                End If

            Case 52
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
                End If

            Case 100
                ' Tests Push 
                If _recipe.TestEnable_TELLTALE_SelectMirror.Value Then
                    If (_results.Push_SELECT_LEFT_LED_WaveLenght.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_SELECT_LEFT_LED_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_SELECT_LEFT_LED_saturation.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_SELECT_LEFT_LED_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                    _results.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR.Test <> cResultValue.eValueTestResult.Passed) And
                    _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If _recipe.TestEnable_TELLTALE_SelectMirror.Value Then
                    If (_results.Push_SELECT_RIGHT_LED_WaveLenght.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_SELECT_RIGHT_LED_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_SELECT_RIGHT_LED_saturation.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_SELECT_RIGHT_LED_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                    _results.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR.Test <> cResultValue.eValueTestResult.Passed) And
                    _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If _recipe.TestEnable_TELLTALE_ChildrenLock.Value Then
                    If (_results.Push_CHILDREN_LOCK_LED_WaveLenght.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_CHILDREN_LOCK_LED_intensity.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_CHILDREN_LOCK_LED_saturation.Test <> cResultValue.eValueTestResult.Passed Or
                    _results.Push_CHILDREN_LOCK_LED_Polygon_Axy.TestResult <> cResultValue.eValueTestResult.Passed Or'Check First one is enough.
                    _results.DEFECT_AREA_TELLTALE_CHILDREN_LOCK.Test <> cResultValue.eValueTestResult.Passed) And
                    _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                        _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                End If

                If _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Passed
                End If


                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                If _stepByStep = False Or (_stepByStep = True And _step = True) Then
                    _step = False
                    AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                    ' Updates the global test result
                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (_results.eTELLTALE_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                        If (CBool(_recipe.TestEnable_TELLTALE_SelectMirror.Value) And
                            (_results.Push_SELECT_LEFT_LED_saturation.TestResult <> cResultValue.eValueTestResult.Passed Or
                             _results.Push_SELECT_LEFT_LED_WaveLenght.TestResult <> cResultValue.eValueTestResult.Passed Or
                             _results.Push_SELECT_LEFT_LED_intensity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                            _results.TestResult = cWS02Results.eTestResult.SL_Telltale_Intensity
                        End If
                        If (CBool(_recipe.TestEnable_TELLTALE_SelectMirror.Value) And
                            (_results.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                            _results.TestResult = cWS02Results.eTestResult.SL_Telltale_Defect
                        End If
                    End If

                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (_results.eTELLTALE_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                        If (CBool(_recipe.TestEnable_TELLTALE_SelectMirror.Value) And
                            (_results.Push_SELECT_RIGHT_LED_saturation.TestResult <> cResultValue.eValueTestResult.Passed Or
                             _results.Push_SELECT_RIGHT_LED_WaveLenght.TestResult <> cResultValue.eValueTestResult.Passed Or
                             _results.Push_SELECT_RIGHT_LED_intensity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                            _results.TestResult = cWS02Results.eTestResult.SR_Telltale_Intensity
                        End If
                        If (CBool(_recipe.TestEnable_TELLTALE_SelectMirror.Value) And
                            (_results.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                            _results.TestResult = cWS02Results.eTestResult.SR_Telltale_Defect
                        End If
                    End If

                    If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        (_results.eTELLTALE_CONFORMITY.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                        If (CBool(_recipe.TestEnable_TELLTALE_ChildrenLock.Value) And
                            (_results.Push_CHILDREN_LOCK_LED_saturation.TestResult <> cResultValue.eValueTestResult.Passed Or
                             _results.Push_CHILDREN_LOCK_LED_WaveLenght.TestResult <> cResultValue.eValueTestResult.Passed Or
                             _results.Push_CHILDREN_LOCK_LED_intensity.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                            _results.TestResult = cWS02Results.eTestResult.CL_Telltale_Intensity
                        End If
                        If (CBool(_recipe.TestEnable_TELLTALE_ChildrenLock.Value) And
                            (_results.DEFECT_AREA_TELLTALE_CHILDREN_LOCK.TestResult <> cResultValue.eValueTestResult.Passed)) Then
                            _results.TestResult = cWS02Results.eTestResult.CL_Telltale_Defect
                        End If
                    End If
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.PWMOutput_0
                End If

            Case 199
                _results.eTELLTALE_CONFORMITY.TestResult =
                    cResultValue.eValueTestResult.Failed
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

    Private Sub PhaseREAD_AnalogInput()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
        Static t0 As Date
        Static tlin As Date
        Static t0Phase As Date
        Static s As String
        Static frameIndex As Integer
        Static CURSEURRetest As Integer
        Static CURSEURCurrent As Integer

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                If CBool(_recipe.TestEnable_AnalogInput.Value) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Read Analog Input")
                    CURSEURRetest = 2
                    CURSEURCurrent = 1
                    If CBool(_recipe.TestEnable_EV_Option.Value) Then
                        ' Go to next subphase
                        _subPhase(_phase) = 10
                    Else
                        ' Go to next subphase
                        _subPhase(_phase) = 1
                    End If
                Else
                    ' Add a log entry
                    AddLogEntry("Read Analog Input  is desabled")
                    _phase = ePhase.SHAPE

                End If
                ' Store the time
                t0 = Date.Now
                tlin = Date.Now

            Case 1
                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_AI_Variant_1),
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
                tlin = Date.Now

            Case 2
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_AI_Variant_1))
                If (i <> -1) Then
                    _results.ADC_UCAD_VARIANT_1.Value = CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                                        _LinInterface.RxFrame(i).Data(5))
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 3
                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_AI_Variant_2),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                ' Store the time
                t0 = Date.Now
                tlin = Date.Now

            Case 4
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_AI_Variant_2))
                If (i <> -1) Then
                    _results.ADC_UCAD_VARIANT_2.Value = CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                                        _LinInterface.RxFrame(i).Data(5))
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _subPhase(_phase) = 10
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 10
                If _recipe.TestEnable_MEMO_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_ADC_Curseur).DeepClone
                    f.Data(4) = "07"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                    tlin = Date.Now
                Else
                    _subPhase(_phase) = 100
                    ' Store the time
                    t0 = Date.Now
                End If

            Case 11
                '

                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ADC_Curseur))
                If (i <> -1) Then
                    AddLogEntry("ADC Value CURSEUR L/R : " & CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                            _LinInterface.RxFrame(i).Data(5)))
                    _results.ADC_CURSEUR_LEFT_RIGHT.Value = CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                                        _LinInterface.RxFrame(i).Data(5))

                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 12
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_ADC_Curseur).DeepClone
                f.Data(4) = "09"
                e = e Or _LinInterface.Transmit(f,
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
                tlin = Date.Now

            Case 13
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ADC_Curseur))
                If (i <> -1) Then
                    AddLogEntry("ADC Value CURSEUR UP/DN : " & CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                            _LinInterface.RxFrame(i).Data(5)))
                    _results.ADC_CURSEUR_UP_DN.Value = CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                                        _LinInterface.RxFrame(i).Data(5))
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    If _results.ADC_CURSEUR_LEFT_RIGHT.Value < _results.ADC_CURSEUR_LEFT_RIGHT.MinimumLimit OrElse
                        _results.ADC_CURSEUR_LEFT_RIGHT.Value > _results.ADC_CURSEUR_LEFT_RIGHT.MaximumLimit OrElse
                        _results.ADC_CURSEUR_UP_DN.Value > _results.ADC_CURSEUR_UP_DN.MaximumLimit OrElse
                        _results.ADC_CURSEUR_UP_DN.Value > _results.ADC_CURSEUR_UP_DN.MaximumLimit Then
                        'NOK. check retest.
                        If (CURSEURCurrent < CURSEURRetest) Then
                            CURSEURCurrent += 1
                            'Retest
                            AddLogEntry("Retest ADC Value CURSEUR")
                            _subPhase(_phase) = 10
                        Else
                            'Over retest time.
                            _subPhase(_phase) = 100
                        End If
                    Else
                        'OK.
                        _subPhase(_phase) = 100
                    End If
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 100
                ' Tests
                If (CBool(_recipe.TestEnable_EV_Option.Value)) Then
                    If ((_recipe.TestEnable_MEMO_Option.Value And
                        (_results.ADC_CURSEUR_UP_DN.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.ADC_CURSEUR_LEFT_RIGHT.Test <> cResultValue.eValueTestResult.Passed))) Then
                        _results.eAnalogInput.TestResult =
                            cResultValue.eValueTestResult.Failed
                    Else
                        _results.eAnalogInput.TestResult =
                            cResultValue.eValueTestResult.Passed
                    End If
                Else
                    If (_results.ADC_UCAD_VARIANT_1.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.ADC_UCAD_VARIANT_2.Test <> cResultValue.eValueTestResult.Passed Or
                        (_recipe.TestEnable_MEMO_Option.Value And
                        (_results.ADC_CURSEUR_UP_DN.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.ADC_CURSEUR_LEFT_RIGHT.Test <> cResultValue.eValueTestResult.Passed))) Then
                        _results.eAnalogInput.TestResult =
                            cResultValue.eValueTestResult.Failed
                    Else
                        _results.eAnalogInput.TestResult =
                            cResultValue.eValueTestResult.Passed
                    End If
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eAnalogInput.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedAnalogInput
                End If
                If (_results.ADC_UCAD_VARIANT_1.TestResult = cResultValue.eValueTestResult.Failed OrElse _results.ADC_UCAD_VARIANT_2.TestResult = cResultValue.eValueTestResult.Failed) Then
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.Write_MMSTraceability
                Else
                    'Clear Subphase
                    _subPhase(_phase) = 0
                    ' Go to next phase
                    _phase = ePhase.SHAPE
                End If

            Case 199
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eAnalogInput.TestResult =
                    cResultValue.eValueTestResult.Failed

                ' Go to next subphase
                _subPhase(_phase) = 101
        End Select

        If TestMode = eTestMode.Remote Then
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
        End If

    End Sub

    Private Sub PhaseDigitalOutput()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
        Dim sample(0 To mWS02AIOManager.eAnalogInput.Count - 1) As Double
        Dim Std_Signal(0 To 20) As Double
        Dim ADC_Retrun As String
        Static t0 As Date
        Static tLin As Date
        Static t0Phase As Date
        Static s As String
        Static frameIndex As Integer

        'Console.WriteLine("_phase" & _phase.ToString & "_subPhase(_phase)" & _subPhase(_phase).ToString() & _LINFrame(224).ToString())
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                If CBool(_recipe.TestEnable_DigitalOutput.Value) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Digital Output")
                    '
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1000
                Else
                    ' Add a log entry
                    AddLogEntry("Digital Output is desabled")
                    '
                    _phase = PhasePrevious
                End If
                ' Store the time
                t0 = Date.Now

#Region "Initial Product Condition"

            Case 1000
                ' Add a log entry
                AddLogEntry("------------------------------------")
                AddLogEntry("Set the product in its default state")
                AddLogEntry("------------------------------------")
                '
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
#Region "DOUT_ENABLE_U2401"
            Case 1001

                If CBool(_recipe.TestEnable_FOLDING_Option.Value) Then
                    AddLogEntry("------------------------------------")
                    AddLogEntry(" Write_DOUT_ENABLE_U2401_HIGH ")
                    AddLogEntry("------------------------------------")
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                    If Not _recipe.TestEnable_EV_Option.Value Then
                        f.Data(4) = "21"
                    Else
                        f.Data(4) = "20"
                    End If

                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                Else
                    _subPhase(_phase) += 2
                End If

            Case 1002
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_H))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1 '199
                End If
#End Region
#Region "DOUT_ENABLE_U2501"
            Case 1003

                If Not CBool(_recipe.TestEnable_MEMO_Option.Value) AndAlso CBool(_recipe.TestEnable_FOLDING_Option.Value) Then
                    AddLogEntry("------------------------------------")
                    AddLogEntry(" Write_DOUT_ENABLE_U2501_HIGH ")
                    AddLogEntry("------------------------------------")
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                    f.Data(4) = "19"
                    e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                Else
                    _subPhase(_phase) += 2
                End If
            Case 1004
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_H))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1 ' 199
                End If
#End Region
#Region "NCV7754_Chip"
            Case 1005
                If _recipe.TestEnable_PWL_Option.Value Then
                    AddLogEntry("------------------------------------------")
                    AddLogEntry(" Deactivate_NCV7754_Chip_outputs_WL_INHIB ")
                    AddLogEntry("------------------------------------------")
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                    f.Data(5) = "06"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                Else
                    ' Go to next subphase
                    _subPhase(_phase) = 1007
                End If

            Case 1006
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1 ' 199
                End If
#End Region
#Region "NCV7703_Chip_U2201_OUT1"
            Case 1007
                AddLogEntry("--------------------------------------------")
                AddLogEntry(" Deactivate_NCV7703_Chip_outputs_U2201_OUT1 ")
                AddLogEntry("--------------------------------------------")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                f.Data(6) = "00"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now

            Case 1008
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1 '199
                End If
#End Region
#Region "NCV7703_Chip U2201_OUT2"
            Case 1009
                AddLogEntry("--------------------------------------------")
                AddLogEntry(" Deactivate_NCV7703_Chip_outputs_U2201_OUT2 ")
                AddLogEntry("--------------------------------------------")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                f.Data(6) = "01"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now

            Case 1010
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1 ' 199
                End If
#End Region
#Region "NCV7703_Chip U2201_OUT3"
            Case 1011
                AddLogEntry("--------------------------------------------")
                AddLogEntry(" Deactivate_NCV7703_Chip_outputs_U2201_OUT3 ")
                AddLogEntry("--------------------------------------------")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                f.Data(6) = "02"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now

            Case 1012
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1 ' 199
                End If
#End Region
#Region "NCV7703_Chip U2301_OUT1"
            Case 1013
                If (Not CBool(_recipe.TestEnable_FOLDING_Option.Value) AndAlso Not CBool(_recipe.TestEnable_MEMO_Option.Value) _
                    AndAlso Not CBool(_recipe.TestEnable_EV_Option.Value)) Then
                    AddLogEntry("--------------------------------------------")
                    AddLogEntry(" Deactivate_NCV7703_Chip_outputs_U2301_OUT1 ")
                    AddLogEntry("--------------------------------------------")
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2301).DeepClone
                    f.Data(6) = "00"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                Else
                    _subPhase(_phase) = 1 ' Start Test.
                End If
            Case 1014
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2301))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 100) Then 'Update by YAN.Qian, wait for update.ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1 '199
                End If
#End Region
#Region "NCV7703_Chip U2301_OUT2"
            Case 1015
                AddLogEntry("--------------------------------------------")
                AddLogEntry(" Deactivate_NCV7703_Chip_outputs_U2301_OUT2 ")
                AddLogEntry("--------------------------------------------")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2301).DeepClone
                f.Data(6) = "01"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now

            Case 1016
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2301))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 100) Then 'Update by YAN.Qian, wait for update.ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) += 1 ' 199
                End If
#End Region
#Region "NCV7703_Chip U2301_OUT3"
            Case 1017
                AddLogEntry("--------------------------------------------")
                AddLogEntry(" Deactivate_NCV7703_Chip_outputs_U2301_OUT3 ")
                AddLogEntry("--------------------------------------------")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2301).DeepClone
                f.Data(6) = "02"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now

            Case 1018
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2301))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) = 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 100) Then 'Update by YAN.Qian, wait for update.ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms / 2) Then
                    ' Go to subphase
                    _subPhase(_phase) = 1 ' 199
                End If
#End Region
#End Region

#Region "PWL Output"
            Case 1
                If _recipe.TestEnable_PWL_Option.Value Then
                    ' Add a log entry
                    AddLogEntry("******************************************************************************************")
                    AddLogEntry("*                           1.Activate the UP_Front_Passenger_CDE                        *")
                    AddLogEntry("******************************************************************************************")
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                    f.Data(5) = "00"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                Else
                    ' Go to next subphase
                    _subPhase(_phase) = 70
                End If


            Case 2
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                    f.Data(5) = "00"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 3
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    ' add log for Debug only
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Front_Passenger_Commande_UP(i, 0).Value = Std_Signal(i)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 4
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                f.Data(5) = "00"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 5
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                    f.Data(5) = "00"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 6
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Front_Passenger_Commande_UP(i, 1).Value = Std_Signal(i)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 10
                End If

            Case 10
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*              2.Activate the DOWN_Front_Passenger_CDE                                   *")
                AddLogEntry("******************************************************************************************")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                f.Data(5) = "01"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 11
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                    f.Data(5) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 12
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Front_Passenger_Commande_Down(i, 0).Value = Std_Signal(i)
                    Next

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 13
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                f.Data(5) = "01"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 14
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                    f.Data(5) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 15
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Front_Passenger_Commande_Down(i, 1).Value = Std_Signal(i)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 20
                End If


            Case 20
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                      3.Activate the WL_INHIB                                           *")
                AddLogEntry("******************************************************************************************")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                f.Data(5) = "06"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 21
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                    f.Data(5) = "06"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 22
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Switches_Inhibition(i, 0).Value = Std_Signal(i)
                    Next

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 23
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                f.Data(5) = "06"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 24
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                    f.Data(5) = "06"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 25
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Switches_Inhibition(i, 1).Value = Std_Signal(i)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 30
                End If

            Case 30
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                      4.Activate the O_DOWN_REAR_RIGHT_CDE                              *")
                AddLogEntry("******************************************************************************************")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                f.Data(5) = "05"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 31
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                    f.Data(5) = "05"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 32
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Down_Rear_Right(i, 0).Value = Std_Signal(i)
                    Next

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 33
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                f.Data(5) = "05"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 34
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                    f.Data(5) = "05"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 35
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Down_Rear_Right(i, 1).Value = Std_Signal(i)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 40
                End If

            Case 40
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                          5.Activate the O_UP_REAR_RIGHT_CDE                            *")
                AddLogEntry("******************************************************************************************")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                f.Data(5) = "04"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 41
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                    f.Data(5) = "04"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 42
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_UP_Rear_Right(i, 0).Value = Std_Signal(i)
                    Next

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 43
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                f.Data(5) = "04"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 44
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                    f.Data(5) = "04"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 45
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_UP_Rear_Right(i, 1).Value = Std_Signal(i)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 50
                End If

            Case 50
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                      6.Activate the O_DOWN_REAR_Left_CDE                               *")
                AddLogEntry("******************************************************************************************")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                f.Data(5) = "03"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 51
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                    f.Data(5) = "03"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 52
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Down_Rear_Left(i, 0).Value = Std_Signal(i)
                    Next

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 53
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                f.Data(5) = "03"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 54
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                    f.Data(5) = "03"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 55
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_Down_Rear_Left(i, 1).Value = Std_Signal(i)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 60
                End If

            Case 60
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                          7.Activate the O_UP_REAR_Left_CDE                             *")
                AddLogEntry("******************************************************************************************")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                f.Data(5) = "02"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 61
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7754).DeepClone
                    f.Data(5) = "02"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 62
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_UP_Rear_Left(i, 0).Value = Std_Signal(i)
                    Next

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 63
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                f.Data(5) = "02"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 64
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7754))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7754).DeepClone
                    f.Data(5) = "02"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 65
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_UP_Rear_Left(i, 1).Value = Std_Signal(i)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 70
                End If
#End Region

#Region "8.Drive Output CDE_+_RBT_RTRV_G "
            Case 70
                If _recipe.TestEnable_FOLDING_Option.Value Then
                    ' Add a log entry
                    AddLogEntry("******************************************************************************************")
                    AddLogEntry("*                      8.Drive Output CDE_+_RBT_RTRV_G                                   *")
                    AddLogEntry("******************************************************************************************")
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                    If Not _recipe.TestEnable_EV_Option.Value Then
                        f.Data(4) = "20"
                    Else
                        f.Data(4) = "1F"
                    End If
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                Else
                    ' Go to next subphase
                    _subPhase(_phase) = 120
                End If

            Case 71                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_H))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                    If Not _recipe.TestEnable_EV_Option.Value Then
                        f.Data(4) = "20"
                    Else
                        f.Data(4) = "1F"
                    End If
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 72
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_CDE_RBT_RTRV_G(i, 0).Value = Std_Signal(i)
                    Next

                    ' Transmit Frame            
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    frameIndex = 0
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 73
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
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    tLin = Date.Now
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
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            ADC_Retrun = 0
                            ADC_Value(s, ADC_Retrun, eADCValue.FOLD_CURRENT_LEFT)
                            _results.ADC_CDE_RBT_RTRV_G(0).Value = ADC_Retrun
                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 74
                ' Transmit Frame  
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_L).DeepClone
                If Not _recipe.TestEnable_EV_Option.Value Then
                    f.Data(4) = "20"
                Else
                    f.Data(4) = "1F"
                End If
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 75
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_L))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_L).DeepClone
                    If Not _recipe.TestEnable_EV_Option.Value Then
                        f.Data(4) = "20"
                    Else
                        f.Data(4) = "1F"
                    End If
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 76
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_CDE_RBT_RTRV_G(i, 1).Value = Std_Signal(i)
                    Next

                    ' Transmit Frame            
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    frameIndex = 0
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 77
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
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    tLin = Date.Now
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
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            ADC_Retrun = 0
                            ADC_Value(s, ADC_Retrun, eADCValue.FOLD_CURRENT_LEFT)
                            _results.ADC_CDE_RBT_RTRV_G(1).Value = ADC_Retrun
                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 78
                '
                _subPhase(_phase) = 90

#End Region

#Region "9.Drive Output CDE_+_RBT_RTRV_D"
            Case 90
                If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                    ' Add a log entry
                    AddLogEntry("******************************************************************************************")
                    AddLogEntry("*                          9.Drive Output CDE_+_RBT_RTRV_D                               *")
                    AddLogEntry("******************************************************************************************")
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                    f.Data(4) = "1C"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                Else
                    ' Go to next subphase
                    _subPhase(_phase) = 120
                End If

            Case 91                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_H))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                    f.Data(4) = "1C"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 92
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_CDE_RBT_RTRV_D(i, 0).Value = Std_Signal(i)
                    Next

                    ' Transmit Frame            
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    frameIndex = 0
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 93
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
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    tLin = Date.Now
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
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()

                            ADC_Retrun = 0
                            ADC_Value(s, ADC_Retrun, eADCValue.FOLD_RIGHT_POS_X)
                            _results.ADC_CDE_RBT_RTRV_D(0).Value = ADC_Retrun
                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 94
                ' Transmit Frame  
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_L).DeepClone
                f.Data(4) = "1C"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 95
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_L))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_L).DeepClone
                    f.Data(4) = "1C"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 96
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For i = 0 To 20
                        _results.STsgn_CDE_RBT_RTRV_D(i, 1).Value = Std_Signal(i)
                    Next

                    ' Transmit Frame            
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    frameIndex = 0
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1

                End If

            Case 97
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
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
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
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            ADC_Retrun = 0
                            ADC_Value(s, ADC_Retrun, eADCValue.FOLD_RIGHT_POS_X)
                            _results.ADC_CDE_RBT_RTRV_D(1).Value = ADC_Retrun
                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 98
                '
                _subPhase(_phase) = 120
#End Region

#Region "10.Activate the CDE_H/B_RTRV_G"
            Case 120
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                              10.Activate the CDE_H/B_RTRV_G                            *")
                AddLogEntry("******************************************************************************************")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                f.Data(6) = "00"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 121
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                    f.Data(6) = "00"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 122
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_CDE_HB_RTRV_G(r, 0).Value = Std_Signal(r)
                    Next
                    '_results.ADC_CDE_HB_RTRV_G(0).Value = 0

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 123
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                f.Data(6) = "00"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 124
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                    f.Data(6) = "00"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 125
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_CDE_HB_RTRV_G(r, 1).Value = Std_Signal(r)
                    Next
                    '_results.ADC_CDE_HB_RTRV_G(1).Value = 0

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 130
                End If
#End Region

#Region "11.Activate the SGN_COMMUN_MOT_RTRV_G"
            Case 130
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                              11.Activate the SGN_COMMUN_MOT_RTRV_G                     *")
                AddLogEntry("******************************************************************************************")
                ' Transmit Frame
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                f.Data(6) = "02"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 131                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                    f.Data(6) = "02"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 132
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, 0).Value = Std_Signal(r)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 133
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                f.Data(6) = "02"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 134
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                    f.Data(6) = "02"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 135
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, 1).Value = Std_Signal(r)
                    Next

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) = 160
                End If
#End Region

#Region "12.Activate the SGN_COMMUN_MOT_RTRV_D"
            Case 160
                If Not _recipe.TestEnable_MEMO_Option.Value Then
                    ' Add a log entry
                    AddLogEntry("******************************************************************************************")
                    AddLogEntry("*                      12.Activate the SGN_COMMUN_MOT_RTRV_D                             *")
                    AddLogEntry("******************************************************************************************")
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                    f.Data(6) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                Else
                    ' Go to next subphase
                    _subPhase(_phase) = 166
                End If


            Case 161                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                    f.Data(6) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 162
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, 0).Value = Std_Signal(r)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 163
                ' Transmit Frame            
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                f.Data(6) = "01"
                e = e Or _LinInterface.Transmit(f,
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 164
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_DAct_NCV7703_U2201))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_DAct_NCV7703_U2201).DeepClone
                    f.Data(6) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 165
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, 1).Value = Std_Signal(r)
                    Next
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase
                    _subPhase(_phase) += 1
                End If

            Case 166
                ' Go to subphase
                _subPhase(_phase) = 140
#End Region

#Region "13.Drive Output CDE_D/G_RTRV_D"
            Case 140
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                          13.Drive Output CDE_D/G_RTRV_D                                *")
                AddLogEntry("******************************************************************************************")
                If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                    f.Data(4) = "1A"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                ElseIf Not _recipe.TestEnable_MEMO_Option.Value And Not _recipe.TestEnable_FOLDING_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2301).DeepClone
                    f.Data(4) = "01"
                    f.Data(5) = "01"
                    f.Data(6) = "01"
                    f.Data(7) = "02"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Go to next subphase
                    _subPhase(_phase) += 1
                    frameIndex = 21
                Else
                    ' Go to next subphase
                    _subPhase(_phase) = 150
                End If
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 141                '
                If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_H))
                Else
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2301))
                End If
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                        ' Transmit Frame
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                        f.Data(4) = "1A"
                    ElseIf Not _recipe.TestEnable_MEMO_Option.Value And Not _recipe.TestEnable_FOLDING_Option.Value Then
                        ' Transmit Frame
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2301).DeepClone
                        f.Data(4) = "01"
                        f.Data(5) = "01"
                        f.Data(6) = "01"
                        f.Data(7) = "02"
                    End If
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 142
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_CDE_DG_RTRV_D(r, 0).Value = Std_Signal(r)
                    Next
                    If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                        ' Transmit Frame            
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                        frameIndex = 0
                        ' Store the time
                        tLin = Date.Now
                        t0 = Date.Now
                        ' Go to subphase
                        _subPhase(_phase) += 1
                    Else
                        t0 = Date.Now
                        ' Go to subphase
                        _subPhase(_phase) = 144
                    End If
                End If

            Case 143
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
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
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
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()

                            ADC_Retrun = 0
                            ADC_Value(s, ADC_Retrun, eADCValue.FOLD_RIGHT_POS_X)
                            _results.ADC_CDE_DG_RTRV_D(0).Value = ADC_Retrun
                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 144
                If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                    ' Transmit Frame  
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_L).DeepClone
                    f.Data(4) = "1A"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                Else
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2301).DeepClone
                    f.Data(4) = "01"
                    f.Data(5) = "01"
                    f.Data(6) = "01"
                    f.Data(7) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                End If
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 145
                '
                If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_L))
                Else
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2301))
                End If
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2301).DeepClone
                    f.Data(4) = "01"
                    f.Data(5) = "01"
                    f.Data(6) = "01"
                    f.Data(7) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
                End If

            Case 146
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_CDE_DG_RTRV_D(r, 1).Value = Std_Signal(r)
                    Next
                    If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                        ' Transmit Frame            
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                        frameIndex = 0
                        ' Store the time
                        t0 = Date.Now
                        tLin = Date.Now
                        '    ' Go to subphase
                        _subPhase(_phase) += 1
                    Else
                        ' Store the time
                        t0 = Date.Now
                        '    ' Go to subphase
                        _subPhase(_phase) = 148
                    End If
                End If

            Case 147
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
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    tLin = Date.Now
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
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            ADC_Retrun = 0
                            ADC_Value(s, ADC_Retrun, eADCValue.FOLD_RIGHT_POS_X)
                            _results.ADC_CDE_DG_RTRV_D(1).Value = ADC_Retrun
                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 148
                '
                _subPhase(_phase) = 150
#End Region

#Region "14.Drive Output CDE_D/G_RTRV_G"
            Case 150
                ' Add a log entry
                AddLogEntry("******************************************************************************************")
                AddLogEntry("*                              14.Drive Output CDE_D/G_RTRV_G                            *")
                AddLogEntry("******************************************************************************************")
                If _recipe.TestEnable_FOLDING_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                    If Not _recipe.TestEnable_EV_Option.Value Then
                        f.Data(4) = "1F"
                    Else
                        f.Data(4) = "1B"
                    End If
                    e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And _recipe.TestEnable_MEMO_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                    f.Data(4) = "01"
                    f.Data(5) = "00"
                    f.Data(6) = "01"
                    f.Data(7) = "02"
                    e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And Not _recipe.TestEnable_MEMO_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2301).DeepClone
                    f.Data(4) = "01"
                    f.Data(5) = "01"
                    f.Data(6) = "02"
                    f.Data(7) = "02"
                    e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                End If
                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 151                '
                If _recipe.TestEnable_FOLDING_Option.Value Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_H))
                ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And _recipe.TestEnable_MEMO_Option.Value Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2201))
                ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And Not _recipe.TestEnable_MEMO_Option.Value Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2301))
                End If
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    If _recipe.TestEnable_FOLDING_Option.Value Then
                        ' Transmit Frame
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_H).DeepClone
                        If Not _recipe.TestEnable_EV_Option.Value Then
                            f.Data(4) = "1F"
                        Else
                            f.Data(4) = "1B"
                        End If
                        e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And _recipe.TestEnable_MEMO_Option.Value Then
                        ' Transmit Frame
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                        f.Data(4) = "01"
                        f.Data(5) = "00"
                        f.Data(6) = "01"
                        f.Data(7) = "02"
                        e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And Not _recipe.TestEnable_MEMO_Option.Value Then
                        ' Transmit Frame
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2301).DeepClone
                        f.Data(4) = "01"
                        f.Data(5) = "01"
                        f.Data(6) = "02"
                        f.Data(7) = "02"
                        e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    End If
                    tLin = Date.Now
                End If

            Case 152
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)
                    For r = 0 To 20
                        _results.STsgn_CDE_DG_RTRV_G(r, 0).Value = Std_Signal(r)
                    Next
                    If _recipe.TestEnable_FOLDING_Option.Value Then

                        ' Transmit Frame            
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                        msngT0TesteurPeriodicFrame = Date.Now
                        frameIndex = 0
                        ' Store the time
                        t0 = Date.Now
                        tLin = Date.Now
                        ' Go to subphase
                        _subPhase(_phase) += 1
                    Else
                        ' Store the time
                        t0 = Date.Now
                        ' Go to subphase
                        _subPhase(_phase) = 154
                    End If
                End If

            Case 153
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
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    tLin = Date.Now
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
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            ADC_Retrun = 0
                            ADC_Value(s, ADC_Retrun, eADCValue.FOLD_CURRENT_LEFT)
                            _results.ADC_CDE_DG_RTRV_G(0).Value = ADC_Retrun
                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 154
                If _recipe.TestEnable_FOLDING_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_L).DeepClone
                    If Not _recipe.TestEnable_EV_Option.Value Then
                        f.Data(4) = "1F"
                    Else
                        f.Data(4) = "1B"
                    End If
                    e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And _recipe.TestEnable_MEMO_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                    f.Data(4) = "01"
                    f.Data(5) = "00"
                    f.Data(6) = "01"
                    f.Data(7) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And Not _recipe.TestEnable_MEMO_Option.Value Then
                    ' Transmit Frame
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2301).DeepClone
                    f.Data(4) = "01"
                    f.Data(5) = "01"
                    f.Data(6) = "02"
                    f.Data(7) = "01"
                    e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                End If

                ' Go to next subphase
                _subPhase(_phase) += 1
                frameIndex = 21
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 155
                '
                If _recipe.TestEnable_FOLDING_Option.Value Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_DOUT_L))
                ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And _recipe.TestEnable_MEMO_Option.Value Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2201))
                ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And Not _recipe.TestEnable_MEMO_Option.Value Then
                    i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Act_NCV7703_U2301))
                End If
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    '
                    _subPhase(_phase) += 1
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
                    If _recipe.TestEnable_FOLDING_Option.Value Then
                        ' Transmit Frame
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_DOUT_L).DeepClone
                        If Not _recipe.TestEnable_EV_Option.Value Then
                            f.Data(4) = "1F"
                        Else
                            f.Data(4) = "1B"
                        End If
                        e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And _recipe.TestEnable_MEMO_Option.Value Then
                        ' Transmit Frame
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2201).DeepClone
                        f.Data(4) = "01"
                        f.Data(5) = "00"
                        f.Data(6) = "01"
                        f.Data(7) = "01"
                        e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    ElseIf Not _recipe.TestEnable_FOLDING_Option.Value And Not _recipe.TestEnable_MEMO_Option.Value Then
                        ' Transmit Frame
                        f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Act_NCV7703_U2301).DeepClone
                        f.Data(4) = "01"
                        f.Data(5) = "01"
                        f.Data(6) = "02"
                        f.Data(7) = "01"
                        e = e Or _LinInterface.Transmit(f,
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    End If
                    tLin = Date.Now
                End If

            Case 156
                If ((Date.Now - t0).TotalMilliseconds >= 50) Then
                    ' Sample the analog inputs
                    e = e Or mWS02AIOManager.SingleSample(sample)
                    '
                    AnalogStandard(sample, Std_Signal)

                    For r = 0 To 20
                        _results.STsgn_CDE_DG_RTRV_G(r, 1).Value = Std_Signal(r)
                    Next
                    If _recipe.TestEnable_FOLDING_Option.Value Then

                        ' Transmit Frame            
                        e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                        msngT0TesteurPeriodicFrame = Date.Now
                        frameIndex = 0
                        ' Store the time
                        t0 = Date.Now
                        tLin = Date.Now
                        ' Go to subphase
                        _subPhase(_phase) += 1
                    Else
                        ' Store the time
                        t0 = Date.Now
                        ' Go to subphase
                        _subPhase(_phase) = 158
                    End If
                End If

            Case 157
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
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) += 1
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_All_AIN),
                                                            True,
                                                            txData_MasterReq,
                                                            cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                            True,
                                                            True)
                    tLin = Date.Now
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
                            ' Lin Schedulle
                            _LinInterface.StopScheduleDiag()
                            ADC_Retrun = 0
                            ADC_Value(s, ADC_Retrun, eADCValue.FOLD_CURRENT_LEFT)
                            _results.ADC_CDE_DG_RTRV_G(1).Value = ADC_Retrun
                            _subPhase(_phase) += 1
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        ' Set the flag of CAN timeout                        
                        _subPhase(_phase) += 1
                    End If
                Loop Until (i = -1)

            Case 158
                '
                _subPhase(_phase) = 100
#End Region


            Case 100
                ' Tests
                If _recipe.TestEnable_PWL_Option.Value Then
                    For c = 0 To 1
                        For r = 0 To 20
                            If _results.STsgn_Front_Passenger_Commande_UP(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                    _results.STsgn_Front_Passenger_Commande_UP(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                            If _results.STsgn_Front_Passenger_Commande_Down(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                _results.STsgn_Front_Passenger_Commande_Down(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                            If _results.STsgn_Switches_Inhibition(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                _results.STsgn_Switches_Inhibition(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                            If _results.STsgn_UP_Rear_Left(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                _results.STsgn_UP_Rear_Left(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                            If _results.STsgn_UP_Rear_Right(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                _results.STsgn_UP_Rear_Right(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                            If _results.STsgn_Down_Rear_Left(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                _results.STsgn_Down_Rear_Left(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                            If _results.STsgn_Down_Rear_Right(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                _results.STsgn_Down_Rear_Right(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                        Next
                    Next
                End If
                'HB commande Retro G
                For c = 0 To 1
                    For r = 0 To 20
                        If _results.STsgn_CDE_HB_RTRV_G(r, c).Test <> cResultValue.eValueTestResult.Passed And
                            _results.STsgn_CDE_HB_RTRV_G(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    Next
                Next
                If Not _recipe.TestEnable_MEMO_Option.Value Then
                    'SGN Commun Mot RTRV D
                    For c = 0 To 1
                        For r = 0 To 20
                            If _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                        Next
                    Next
                End If
                ' DG commande Retro D
                For c = 0 To 1
                    For r = 0 To 20
                        If _results.STsgn_CDE_DG_RTRV_D(r, c).Test <> cResultValue.eValueTestResult.Passed And
                            _results.STsgn_CDE_DG_RTRV_D(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    Next
                    If _recipe.TestEnable_FOLDING_Option.Value And Not _recipe.TestEnable_MEMO_Option.Value Then
                        If _results.ADC_CDE_DG_RTRV_D(c).Test <> cResultValue.eValueTestResult.Passed And
                            _results.ADC_CDE_DG_RTRV_D(c).Test <> cResultValue.eValueTestResult.Disabled Then
                            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    End If
                Next
                ' DG commande Retro G
                For c = 0 To 1
                    For r = 0 To 20
                        If _results.STsgn_CDE_DG_RTRV_G(r, c).Test <> cResultValue.eValueTestResult.Passed And
                            _results.STsgn_CDE_DG_RTRV_G(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    Next
                    If _recipe.TestEnable_FOLDING_Option.Value Then
                        If _results.ADC_CDE_DG_RTRV_G(c).Test <> cResultValue.eValueTestResult.Passed And
                            _results.ADC_CDE_DG_RTRV_G(c).Test <> cResultValue.eValueTestResult.Disabled Then
                            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    End If
                Next
                ' RBT commande Retro G
                For c = 0 To 1
                    For r = 0 To 20
                        If _results.STsgn_CDE_RBT_RTRV_G(r, c).Test <> cResultValue.eValueTestResult.Passed And
                            _results.STsgn_CDE_RBT_RTRV_G(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    Next
                    If _results.ADC_CDE_RBT_RTRV_G(c).Test <> cResultValue.eValueTestResult.Passed And
                        _results.ADC_CDE_RBT_RTRV_G(c).Test <> cResultValue.eValueTestResult.Disabled Then
                        _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                Next

                ' RBT commande Retro D
                If _recipe.TestEnable_FOLDING_Option.Value And Not _recipe.TestEnable_MEMO_Option.Value Then
                    For c = 0 To 1
                        For r = 0 To 20
                            If _results.STsgn_CDE_RBT_RTRV_D(r, c).Test <> cResultValue.eValueTestResult.Passed And
                                _results.STsgn_CDE_RBT_RTRV_D(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                                _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                            End If
                        Next
                        If _results.ADC_CDE_RBT_RTRV_D(c).Test <> cResultValue.eValueTestResult.Passed And
                            _results.ADC_CDE_RBT_RTRV_D(c).Test <> cResultValue.eValueTestResult.Disabled Then
                            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    Next
                End If

                'SGN Commun Mot RTRV G
                For c = 0 To 1
                    For r = 0 To 20
                        If _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, c).Test <> cResultValue.eValueTestResult.Passed And
                            _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, c).Test <> cResultValue.eValueTestResult.Disabled Then
                            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Failed
                        End If
                    Next
                Next

                If _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        _results.eDigitalOutput.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedeDigitalOutput
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = PhasePrevious

            Case 199
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eDigitalOutput.TestResult =
                        cResultValue.eValueTestResult.Failed

                ' Go to next subphase
                _subPhase(_phase) = 101
                'Temp
                '            _subPhase(_phase) = 100

        End Select

        If TestMode = eTestMode.Remote Then
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
        End If

    End Sub

    Private Sub PhaseUCDA_PWM_Output(ByVal UCDA_PWM As Integer)

        Dim e As Boolean
        Dim sp As Integer
        Dim sample(0 To mWS02AIOManager.eAnalogInput.Count - 1) As Double
        Dim f As CLINFrame
        Dim i As Integer
        Dim DutyCycle As Double
        Dim Frequence As Double
        Dim LowVoltage As Double
        Dim HighVoltage As Double
        Dim RiseTime As Double
        Dim FallTime As Double
        Dim Frequency As Long = 45000 '5000
        Dim nbPoints As Integer = 900
        Static dl(1000) As Double
        Static ns As Long
        Static t0 As Date
        Static t0Phase As Date
        Static frameIndex As Integer
        Static s As String
        Static tlin As Date

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                If _recipe.TestEnable_PWMOutput.Value Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    If UCDA_PWM = 0 Then
                        AddLogEntry("Begin UCDA PWM OUTPUT External Bakclight WLAP")
                        'Sample rate 45000 is not enough for all channels. set here only two needed channels, and reset at case 101.
                        mWS02AIOManager.PowerDown()
                        mWS02AIOManager.PowerUpforPWM(mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString() +
                                                      "," & mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString(),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24))
                        ' Go to next subphase
                        _subPhase(_phase) = 1
                    ElseIf UCDA_PWM = 1 Then
                        AddLogEntry("Begin UCDA PWM Output External Bakclight Door Lock")
                        ' Go to next subphase
                        _subPhase(_phase) = 20
                    End If

                    ' Store the time
                    t0 = Date.Now
                Else
                    ' Add a log entry
                    AddLogEntry("PWM Output is Disabled")
                    _phase = ePhase.Write_Temperature

                End If

            Case 1
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_PWM_WLAP_ON),
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
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_PWM_WLAP_ON_2),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 3
                End If

            Case 3
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_PWM_WLAP_ON))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 4
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 4

                End If

            Case 4
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    ' Sample the analog inputs
                    e = mWS02AIOManager.SingleSamplePWM(sample)
                    AddLogEntry("AIO : " & CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString() & " " & mWS02AIOManager.AnalogInputDescription(CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString()) & " Value=" & sample(0))
                    AddLogEntry("AIO : " & CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString() & " " & mWS02AIOManager.AnalogInputDescription(CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString()) & " Value=" & sample(1))
                    'For i = 0 To mWS02AIOManager.eAnalogInput.Count - 1
                    '    AddLogEntry("AIO : " & i & " " & mWS02AIOManager.AnalogInputDescription(i) & " Value=" & sample(i))
                    'Next i
                    ' Empty Buffer
                    mWS02AIOManager.EmptyBuffer()
                    ' Starts the task for continous sampling 
                    e = e Or (mWS02AIOManager.StartSampling(Frequency))

                    ' Go to next subphase
                    _subPhase(_phase) = 5
                    ' Store the time
                    t0 = Date.Now

                End If

            Case 5
                ' If the test delay expired
                If (mWS02AIOManager.SampleCount >= nbPoints) Then
                    AddLogEntry("Case 5 Entered :")
                    'Stop the tsk 
                    mWS02AIOManager.PowerDown()
                    mWS02AIOManager.PowerUpforPWM(mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString() +
                                                      "," & mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString(),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24))
                    ' Gets the microphone samples
                    If mWS02AIOManager.SampleCount > nbPoints Then
                        ns = nbPoints
                    Else
                        ns = mWS02AIOManager.SampleCount
                    End If
                    ' Store the samples
                    For sampleIndex = 0 To ns - 1
                        dl(sampleIndex) =
                            mWS02AIOManager.SampleBuffer(mWS02AIOManager.eAnalogInput.WS02_Pin12, sampleIndex)
                        chUCDA_PWM(0, sampleIndex) = dl(sampleIndex)
                    Next
                    chUCDA_Sample(0) = ns
                    ' Measure frequency
                    mWS02Main.WS02AI_DutyCycle_Freq(DutyCycle,
                                                        Frequence,
                                                        dl,
                                                        LowVoltage,
                                                        HighVoltage,
                                                        RiseTime,
                                                        FallTime,
                                                        Frequency)
                    'Analyse and Store Data
                    _results.UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12.Value = LowVoltage
                    _results.UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12.Value = HighVoltage
                    _results.UCDA_SINGLE_SW_DIAG_RiseTime_X1_12.Value = RiseTime
                    _results.UCDA_SINGLE_SW_DIAG_FallTime_X1_12.Value = FallTime
                    _results.UCDA_SINGLE_SW_DIAG_Frequency_X1_12.Value = Frequence
                    _results.UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12.Value = DutyCycle
                    AddLogEntry("PWM WLAP :")
                    AddLogEntry(vbTab & "Low Voltage : " & LowVoltage)
                    AddLogEntry(vbTab & "High Voltage : " & HighVoltage)
                    AddLogEntry(vbTab & "Rise Time : " & RiseTime)
                    AddLogEntry(vbTab & "Fall Time : " & FallTime)
                    AddLogEntry(vbTab & "Frequency : " & Frequence)
                    AddLogEntry(vbTab & "Duty Cycle : " & DutyCycle)

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) = 6
                ElseIf ((Date.Now - t0).TotalMilliseconds > 1000) Then
                    AddLogEntry("Case 5 Timeout :")
                    'Stop the tsk 
                    mWS02AIOManager.PowerDown()
                    mWS02AIOManager.PowerUpforPWM(mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString() +
                                                      "," & mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString(),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24))
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) = 6

                End If

            Case 6
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ADC_WLAP),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 7

            Case 7
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ADC_WLAP))
                If (i <> -1) Then
                    _results.UCDA_SINGLE_SW_DIAG.Value = CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                            _LinInterface.RxFrame(i).Data(5))
                    AddLogEntry("Analog ADC Value : " & CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                            _LinInterface.RxFrame(i).Data(5)))
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 30
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 30

                End If

            Case 20
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_PWM_DoorL_ON),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 21

            Case 21
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_PWM_DoorL_ON_2),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    msngT0TesteurPeriodicFrame = Date.Now
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 22
                End If

            Case 22
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_PWM_DoorL_ON))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 23
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 23

                End If

            Case 23
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    ' Sample the analog inputs
                    e = mWS02AIOManager.SingleSamplePWM(sample)
                    AddLogEntry("AIO : " & CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString() & " " & mWS02AIOManager.AnalogInputDescription(CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString()) & " Value=" & sample(0))
                    AddLogEntry("AIO : " & CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString() & " " & mWS02AIOManager.AnalogInputDescription(CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString()) & " Value=" & sample(1))

                    'For i = 0 To mWS02AIOManager.eAnalogInput.Count - 1
                    '    AddLogEntry("AIO : " & i & " " & mWS02AIOManager.AnalogInputDescription(i) & " Value=" & sample(i))
                    'Next i
                    ' Empty Buffer
                    mWS02AIOManager.EmptyBuffer()
                    ' Starts the task for continous sampling 
                    e = e Or (mWS02AIOManager.StartSampling(Frequency))
                    ' Go to next subphase
                    _subPhase(_phase) = 24
                    ' Store the time
                    t0 = Date.Now

                End If

            Case 24
                ' If the test delay expired
                If (mWS02AIOManager.SampleCount >= nbPoints) Then
                    AddLogEntry("Case 22 Entered :")
                    'Stop the tsk 
                    mWS02AIOManager.PowerDown()
                    mWS02AIOManager.PowerUpforPWM(mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString() +
                                                      "," & mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString(),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24))
                    ' Gets the microphone samples
                    If mWS02AIOManager.SampleCount > nbPoints Then
                        ns = nbPoints
                    Else
                        ns = mWS02AIOManager.SampleCount
                    End If
                    ' Store the samples
                    For sampleIndex = 0 To ns - 1
                        dl(sampleIndex) =
                            mWS02AIOManager.SampleBuffer(mWS02AIOManager.eAnalogInput.WS02_PIN24, sampleIndex)
                        chUCDA_PWM(1, sampleIndex) = dl(sampleIndex)
                    Next
                    chUCDA_Sample(1) = ns
                    ' Measure frequency
                    mWS02Main.WS02AI_DutyCycle_Freq(DutyCycle,
                                                        Frequence,
                                                        dl,
                                                        LowVoltage,
                                                        HighVoltage,
                                                        RiseTime,
                                                        FallTime,
                                                        Frequency)
                    'Analyse and Store Data
                    _results.UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24.Value = LowVoltage
                    _results.UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24.Value = HighVoltage
                    _results.UCDA_DOOR_LOCK_DIAG_RiseTime_X1_24.Value = RiseTime
                    _results.UCDA_DOOR_LOCK_DIAG_FallTime_X1_24.Value = FallTime
                    _results.UCDA_DOOR_LOCK_DIAG_Frequency_X1_24.Value = Frequence
                    _results.UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24.Value = DutyCycle
                    AddLogEntry("PWM DOOR LOCK :")
                    AddLogEntry(vbTab & "Low Voltage : " & LowVoltage)
                    AddLogEntry(vbTab & "High Voltage : " & HighVoltage)
                    AddLogEntry(vbTab & "Rise Time : " & RiseTime)
                    AddLogEntry(vbTab & "Fall Time : " & FallTime)
                    AddLogEntry(vbTab & "Frequency : " & Frequence)
                    AddLogEntry(vbTab & "Duty Cycle : " & DutyCycle)

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) = 25
                ElseIf ((Date.Now - t0).TotalMilliseconds > 1000) Then
                    AddLogEntry("Case 22 Timeout :")
                    'Stop the tsk 
                    mWS02AIOManager.PowerDown()
                    mWS02AIOManager.PowerUpforPWM(mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12).ToString() +
                                                      "," & mWS02Main.Settings.AIOInterface & "/ai" + CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24).ToString(),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_Pin12),
                                                        CInt(mWS02AIOManager.eAnalogInput.WS02_PIN24))

                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) = 25
                End If

            Case 25
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ADC_DoorL),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 26

            Case 26
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ADC_DoorL))
                If (i <> -1) Then
                    _results.UCDA_DOOR_LOCK_DIAG.Value = CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                            _LinInterface.RxFrame(i).Data(5))
                    AddLogEntry("Analog ADC Value : " & CInt("&h" & _LinInterface.RxFrame(i).Data(4) &
                                                            _LinInterface.RxFrame(i).Data(5)))
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 30
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 30

                End If

            Case 30
                ' Switch-on the backlight
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_PWM_OFF),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 31

            Case 31
                If ((Date.Now - t0).TotalMilliseconds > 10) Then
                    ' Switch-on the backlight
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_PWM_OFF_2),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 32
                End If

            Case 32
                ' If the answer was received
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_PWM_OFF))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - t0).TotalMilliseconds > Timeout_ms) Then
                    ' Otherwise, if the answer was not received within some time
                    ' Go to next subphase
                    _subPhase(_phase) = 199
                    'Temp
                    _subPhase(_phase) = 100

                End If

            Case 100
                ' Tests
                If UCDA_PWM = 0 Then
                    If _results.UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.UCDA_SINGLE_SW_DIAG_Frequency_X1_12.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.UCDA_SINGLE_SW_DIAG.Test <> cResultValue.eValueTestResult.Passed Then

                        ' Update the test result
                        _results.ePWMOutput.TestResult = cResultValue.eValueTestResult.Failed
                    End If
                ElseIf UCDA_PWM = 1 Then
                    If _results.UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.UCDA_DOOR_LOCK_DIAG_Frequency_X1_24.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24.Test <> cResultValue.eValueTestResult.Passed Or
                        _results.UCDA_DOOR_LOCK_DIAG.Test <> cResultValue.eValueTestResult.Passed Then
                        ' Update the test result
                        _results.ePWMOutput.TestResult = cResultValue.eValueTestResult.Failed
                    Else
                        'UCDA_PWM0 and UCDA_PWM1 both pass, then pass.
                        If (_results.ePWMOutput.TestResult <> cResultValue.eValueTestResult.Failed) Then
                            _results.ePWMOutput.TestResult = cResultValue.eValueTestResult.Passed
                        End If
                    End If
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                'Clear Subphase
                _subPhase(_phase) = 0

                ' Go to next phase
                If UCDA_PWM = 0 Then
                    _phase = ePhase.PWMOutput_1
                ElseIf UCDA_PWM = 1 Then
                    _phase = ePhase.Write_Temperature
                    mWS02AIOManager.PowerDown()
                    mWS02AIOManager.PowerUp() 'Reset all channels
                End If

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout")
                ' Update the test result
                _results.ePWMOutput.TestResult =
                    cResultValue.eValueTestResult.Failed

                ' Go to next subphase
                _subPhase(_phase) = 101

        End Select

        ' If a runtime error occured
        If (e = True) Then
            ' Add a log entry
            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
            ' Update the global test result
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
        End If

    End Sub

    Private Sub PhaseWriteTemperatureSet()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame
        Dim sample(0 To mWS02AIOManager.eAnalogInput.Count - 1) As Double

        Static t0 As Date
        Static tlin As Date
        Static t0Phase As Date
        Static s As String

        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                If CBool(_recipe.TestEnable_Temperature.Value) And _recipe.TestEnable_EV_Option.Value And CurrentSampleType = EnumSampleType.Production Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Write Temperature Set")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1
                Else
                    ' Add a log entry
                    AddLogEntry("Temperature Set is disable")
                    ' Store the time
                    t0 = Date.Now

                    _phase = ePhase.Write_MMSTraceability
                End If


            Case 1
                ' Sample the analog inputs
                e = e Or mWS02AIOManager.SingleSample(sample)
                _results.External_Temp.Value = sample(mWS02AIOManager.eAnalogInput.WS02_ExtTemp)
                For i = 0 To 31
                    AddLogEntry(mWS02AIOManager.AnalogInputDescription(i) & " : " & sample(i))
                Next
                ' Store the time
                t0 = Date.Now
                ' Go to subphase
                _subPhase(_phase) = 2

            Case 2
                ' Transmit Frame
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_Temperature),
                                                    True,
                                                    txData_MasterReq,
                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                    True,
                                                    True)
                msngT0TesteurPeriodicFrame = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 3
                s = ""
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 3
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Read_Temperature))
                If (i <> -1) Then
                    s = _LinInterface.RxFrame(i).Data(5) &
                            _LinInterface.RxFrame(i).Data(6)
                    _results.ADC_Temp.Value = CInt("&H" & s)
                    AddLogEntry("ADC Value : " & CInt("&h" & s))
                    AddLogEntry("Ambiant Temperature : " & CInt("&h" & _LinInterface.RxFrame(i).Data(7)))
                    AddLogEntry("External Temperature : " & _results.External_Temp.Value)
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Store the time
                    t0 = Date.Now
                    If (CSng(_results.External_Temp.Value) < 10 OrElse CSng(_results.External_Temp.Value) > 43) Then
                        ' Go to subphase
                        _subPhase(_phase) = 199
                        AddLogEntry("External Temperature abnormal.")
                    Else
                        ' Go to subphase
                        _subPhase(_phase) = 4
                    End If
                ElseIf ((Date.Now - tlin).TotalMilliseconds > LinRelance_ms) Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Read_Temperature),
                                                        True,
                                                        txData_MasterReq,
                                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq,
                                                        True,
                                                        True)
                    '
                    tlin = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 4
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Write_Temperature).DeepClone
                f.Data(5) = Mid(s, 1, 2)
                f.Data(6) = Mid(s, 3, 2)
                ' i force the value to 20° , i wait the temperature sensor is done for remove this force value
                'f.Data(7) = Hex(20) ' Right("00" & Hex(CInt(_results.External_Temp.Value)), 2)
                f.Data(7) = Right("00" & Hex(CInt(_results.External_Temp.Value)), 2)
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
                _subPhase(_phase) = 5

            Case 5
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Write_Temperature))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    'Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 100
                ' Tests
                _results.eWrite_Temperature_Set.TestResult =
                    cResultValue.eValueTestResult.Passed
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eWrite_Temperature_Set.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedeSleepModeCurrent
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Write_MMSTraceability

            Case 199
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eWrite_Temperature_Set.TestResult =
                    cResultValue.eValueTestResult.Failed

                ' Go to next subphase
                _subPhase(_phase) = 101
        End Select

        If TestMode = eTestMode.Remote Then
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
        End If

    End Sub

    Private Sub PhaseWRITE_MMSTraceability()
        Dim e As Boolean
        Dim f As CLINFrame
        Dim i As Integer
        Dim sp As Integer

        Static NRC_78 As Boolean
        Static Relance As Integer
        Static t0 As Date
        Static t0Phase As Date
        Static s As String
        Static frameIndex As Integer
        Static LinTimeOut As Boolean
        Static PreviousPhase As Integer
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0
                NRC_78 = False
                If CBool(_recipe.TestEnable_WriteMMS.Value) And CurrentSampleType = EnumSampleType.Production Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Write Valeo MMS Traceability")
                    '                    
                    Relance = 0
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1
                Else
                    ' Add a log entry
                    AddLogEntry("Write Valeo MMS Traceability is desabled")
                    ' Store the time
                    t0 = Date.Now
                    _phase = ePhase.Write_MMSTestByte
                    _results.eWrite_TraceabilityMMS.TestResult = cResultValue.eValueTestResult.Disabled
                End If

            Case 1
                If _results.eRead_MMS_Serial_Number.TestResult = cResultValue.eValueTestResult.Passed Then
                    ' Generate the part number
                    _results.PartUniqueNumber.Value = Mid(_results.TestDate.Value, 9, 2) &
                                                    Mid(_results.TestDate.Value, 4, 2) &
                                                    Mid(_results.TestDate.Value, 1, 2) &
                                                    Mid(GeneratePartNumber(_results.TestDate.Value), 7, 4)
                    '
                    _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit = Mid(_results.TestDate.Value, 9, 2) &
                                                    Mid(_results.TestDate.Value, 4, 2) &
                                                    Mid(_results.TestDate.Value, 1, 2) &
                                                    Mid(_results.TestTime.Value, 1, 2) &
                                                    Mid(_results.TestTime.Value, 4, 2)
                    _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MaximumLimit = _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit
                Else
                    _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit = Mid(_results.PartUniqueNumber.Value, 1, 6) &
                                                    Mid(_results.TestTime.Value, 1, 2) &
                                                    Mid(_results.TestTime.Value, 4, 2)
                    _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MaximumLimit = _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit
                End If

                _results.MMS_Read_Valeo_Serial_Number.MinimumLimit = Mid(_results.PartUniqueNumber.Value, 7, 4)
                _results.MMS_Read_Valeo_Serial_Number.MaximumLimit = Mid(_results.PartUniqueNumber.Value, 7, 4)

                f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_W_Tracea).DeepClone
                f.Data(6) = Mid(_recipe.Write_MMS_Final_Product_Reference.Value, 1, 2)
                f.Data(7) = Mid(_recipe.Write_MMS_Final_Product_Reference.Value, 3, 2)
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
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
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(1) = "21"
                    f.Data(2) = Mid(_recipe.Write_MMS_Final_Product_Reference.Value, 5, 2)
                    f.Data(3) = Mid(_recipe.Write_MMS_Final_Product_Reference.Value, 7, 2)
                    f.Data(4) = Mid(_recipe.Write_MMS_Final_Product_Reference.Value, 9, 2)
                    f.Data(5) = Mid(_recipe.Write_MMS_Final_Product_Index.Value, 1, 2)
                    f.Data(6) = Mid(_recipe.Write_MMS_Final_Product_Index.Value, 3, 2)
                    f.Data(7) = Mid(_results.MMS_Read_Valeo_Final_Product_Plant.MinimumLimit, 1, 2)
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
                    _subPhase(_phase) = 3
                End If

            Case 3
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(1) = "22"
                    f.Data(2) = Mid(_results.MMS_Read_Valeo_Final_Product_Plant.MinimumLimit, 3, 2)
                    f.Data(3) = Mid(_results.MMS_Read_Valeo_Final_Product_Plant.MinimumLimit, 5, 2)
                    f.Data(4) = Mid(_results.MMS_Read_Valeo_Final_Product_Plant.MinimumLimit, 7, 2)
                    f.Data(5) = Right("00" & mSettings.LineNumber, 2) ' _recipe.Write_MMS_Valeo_Final_Product_Line.Value
                    f.Data(6) = Mid(_results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit, 1, 2)
                    f.Data(7) = Mid(_results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit, 3, 2)
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
                    _subPhase(_phase) = 4
                End If

            Case 4
                If ((Date.Now - t0).TotalMilliseconds > 20) Then
                    f = _LINFrame(mGlobal.eLINFrame.DIAG_Req_Empty).DeepClone
                    f.Data(1) = "23"
                    f.Data(2) = Mid(_results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit, 5, 2)
                    f.Data(3) = Mid(_results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit, 7, 2)
                    f.Data(4) = Mid(_results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit, 9, 2)
                    f.Data(5) = Mid(_results.PartUniqueNumber.Value, 7, 2)
                    f.Data(6) = Mid(_results.PartUniqueNumber.Value, 9, 2)
                    f.Data(7) = _recipe.Write_MMS_Deviation_Number.Value

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
                    _subPhase(_phase) = 5
                End If

            Case 5
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_MMS_W_Tracea))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    _subPhase(_phase) = 100
                    '
                    Relance = 0
                ElseIf ((Date.Now - t0).TotalMilliseconds > 2 * LINTimeout_ms) Then
                    If Relance >= 1 Or NRC_78 Then
                        PreviousPhase = 100
                        LinTimeOut = True
                        'Go to subphase
                        _subPhase(_phase) = 199
                    Else
                        Relance += 1
                        ' Go to subphase
                        _subPhase(_phase) = 1
                    End If
                End If

                ' Manage the NRC 78 Pending
                f = _LINFrame(mGlobal.eLINFrame.DIAG_Rep_Empty).DeepClone
                f.Data(0) = "XX" : f.Data(1) = "03" : f.Data(2) = "7F" : f.Data(3) = "2E"
                f.Data(4) = "XX" : f.Data(5) = "XX" : f.Data(6) = "XX" : f.Data(7) = "XX"
                i = _LinInterface.RxFrameIndex(f)
                If (i <> -1) Then
                    If _LinInterface.RxFrame(i).Data(4) = "78" Then
                        AddLogEntry("NRC 78 , Pending Request")
                        NRC_78 = True
                    Else
                        LinTimeOut = True
                        PreviousPhase = 100
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
                If _results.eWrite_TraceabilityMMS.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eWrite_TraceabilityMMS.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If

                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                        _results.eWrite_TraceabilityMMS.TestResult <>
                            cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedeWrite_TraceabilityMMS
                End If

                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Write_MMSTestByte

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eWrite_TraceabilityMMS.TestResult =
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
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
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
                If _results.TestResult = cWS02Results.eTestResult.Unknown Then
                    If _results.Valeo_Serial_Number.Value = "FFFFFFFFFF" Then
                        'First Test
                        f.Data(5) = "21"
                    Else
                        'Retest.
                        f.Data(5) = "27"
                    End If
                Else
                    f.Data(5) = "22"
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
                _results.eWrite_TestByteMMS.TestResult =
                        cResultValue.eValueTestResult.Passed
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eWrite_TestByteMMS.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedeWriteRead_IntertestByte
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.ResetEcu

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eWrite_TestByteMMS.TestResult =
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
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If

    End Sub

    Private Sub PhaseReset_Ecu()
        Dim e As Boolean
        Dim f As CLINFrame
        Dim i As Integer
        Dim sp As Integer
        Static KeyAccess(0 To 5) As String
        Static t0 As Date
        Static t0Phase As Date
        Static tLin As Date
        Static s As String
        Static frameIndex As Integer
        Static LinTimeOut As Boolean
        ' Clear the error flag
        e = False
        ' Store the entry subphase
        sp = _subPhase(_phase)
        ' Manage the subphases
        Select Case sp
            Case 0

                'If CBool(_recipe.TestEnable_WriteMMS.Value) Then
                ' Store the phase entry time
                t0Phase = Date.Now
                ' Add a log entry
                AddLogEntry("Begin Reset ECU")
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
                _subPhase(_phase) = 1
                'Else
                '    ' Add a log entry
                '    AddLogEntry("Reset ECU is desabled")
                '    ' Store the time
                '    t0 = Date.Now
                '    ' Go to next subphase
                '    _phase = ePhase.Read_MMSTraceability
                'End If

            Case 1
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_ECU_Reset),
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
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_ECU_Reset))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    _subPhase(_phase) = 3
                    ' Store the time
                    t0 = Date.Now
                ElseIf ((Date.Now - t0).TotalMilliseconds > 5 * LINTimeout_ms) Then
                    LinTimeOut = True
                    ' Lin Schedulle
                    _LinInterface.StopScheduleDiag()
                    ' Go to subphase
                    _subPhase(_phase) = 199
                End If

            Case 3
                If ((Date.Now - t0).TotalMilliseconds >= 300) Then
                    ' Store the time
                    t0 = Date.Now
                    tLin = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 4
                End If

            Case 4
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
                _subPhase(_phase) = 5

            Case 5
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
                    _subPhase(_phase) = 10
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    LinTimeOut = True
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
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

            Case 10
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
                _subPhase(_phase) = 11

            Case 11
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
                    _subPhase(_phase) = 12
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    LinTimeOut = True
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
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

            Case 12
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
                _subPhase(_phase) = 13

            Case 13
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_SendKey))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) = 20
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    LinTimeOut = True
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
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

            Case 20
                ' Open Diag 1013
                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_OpenDiag_1070),
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
                _subPhase(_phase) = 21

            Case 21
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_OpenDiag_1070))
                If (i <> -1) Then
                    ' Delete the frame
                    _LinInterface.DeleteRxFrame(i)
                    'Stop Schedule Diag
                    _LinInterface.StopScheduleDiag()
                    ' Store the time
                    t0 = Date.Now
                    ' Go to subphase 5
                    _subPhase(_phase) = 100
                ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                    LinTimeOut = True
                    _subPhase(_phase) = 199
                ElseIf ((Date.Now - tLin).TotalMilliseconds >= LinRelance_ms) Then
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



            Case 100
                ' Tests
                ' Update the test result
                If _results.eResetEcu_AfterWriting.TestResult = cResultValue.eValueTestResult.Unknown Then
                    _results.eResetEcu_AfterWriting.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If

                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eResetEcu_AfterWriting.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedeResetEcu_AfterWriting
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Read_MMSTraceability

            Case 199
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eResetEcu_AfterWriting.TestResult =
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
            _results.TestResult = cWS02Results.eTestResult.FailedRuntimeError
            ' Raise an alarm for runtime error
            _alarm(eAlarm.RuntimeError) = True
            ' Go to Phase Abort test
            _phase = ePhase.AbortTest
        End If

    End Sub

    Private Sub PhaseREAD_MMSTraceability()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame

        Static LinTimeout As Boolean
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
                If CBool(_recipe.TestEnable_ReadMMS.Value) Then
                    ' Store the phase entry time
                    t0Phase = Date.Now
                    ' Add a log entry
                    AddLogEntry("Begin Read Valeo MMS Traceability ")
                    ' Store the time
                    t0 = Date.Now
                    ' Go to next subphase
                    _subPhase(_phase) = 1
                Else
                    ' Add a log entry
                    AddLogEntry("Read Valeo MMS Traceability is Disabled")
                    '' Go to next phase

                    ' Go to next phase
                    _phase = ePhase.Read_MMSTestByte
                End If

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
                frameIndex = 0
                s = ""
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

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
                ElseIf ((Date.Now - tLin).TotalMilliseconds > LinRelance_ms) And frameIndex = 0 Then
                    ' Transmit Frame
                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_MMS_R_Tracea),
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
                    _results.eRead_TraceabilityMMS.TestResult = cResultValue.eValueTestResult.Failed
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
                            _results.MMS_Read_Final_Product_Reference.Value = Mid(s, 1, 10)
                            _results.MMS_Read_Final_Product_Index.Value = Mid(s, 11, 4)
                            _results.MMS_Read_Valeo_Final_Product_Plant.Value = Mid(s, 15, 8)
                            _results.MMS_Read_Valeo_Final_Product_Line.Value = Mid(s, 23, 2)
                            _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.Value = Mid(s, 25, 10)
                            _results.MMS_Read_Valeo_Serial_Number.Value = Mid(s, 35, 4)
                            _results.MMS_Read_Deviation_Number.Value = Mid(s, 39, 2)

                            _subPhase(_phase) = 100
                        End If
                    ElseIf ((Date.Now - t0).TotalMilliseconds > LINTimeout_ms) Then
                        ' Set the flag of CAN timeout
                        LinTimeout = True
                        _subPhase(_phase) = 100
                    End If
                Loop Until (i = -1)

            Case 100
                ' Tests
                If (_results.MMS_Read_Final_Product_Reference.Test <> cResultValue.eValueTestResult.Passed Or
                            _results.MMS_Read_Final_Product_Index.Test <> cResultValue.eValueTestResult.Passed Or
                            _results.MMS_Read_Valeo_Final_Product_Plant.Test <> cResultValue.eValueTestResult.Passed Or
                            _results.MMS_Read_Valeo_Final_Product_Line.Test <> cResultValue.eValueTestResult.Passed Or
                            _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.Test <> cResultValue.eValueTestResult.Passed Or
                            _results.MMS_Read_Valeo_Serial_Number.Test <> cResultValue.eValueTestResult.Passed Or
                            _results.MMS_Read_Deviation_Number.Test <> cResultValue.eValueTestResult.Passed) Then
                    _results.eRead_TraceabilityMMS.TestResult =
                        cResultValue.eValueTestResult.Failed
                Else
                    _results.eRead_TraceabilityMMS.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eRead_TraceabilityMMS.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedeRead_TraceabilityMMS
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.Read_MMSTestByte

            Case 199
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eRead_TraceabilityMMS.TestResult =
                    cResultValue.eValueTestResult.Failed

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
        End If

    End Sub


    Private Sub PhaseREAD_MMSTestByte()
        Dim e As Boolean
        Dim i As Integer
        Dim sp As Integer
        Dim f As CLINFrame

        Static LinTimeout As Boolean
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
                AddLogEntry("Begin Read MMS Test Byte ")
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
                _subPhase(_phase) = 2
                ' Store the time
                t0 = Date.Now
                tLin = Date.Now

            Case 2
                '
                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_MMS_R_TestByte))
                If (i <> -1) Then
                    _results.MMS_Read_Test_Byte.Value = _LinInterface.RxFrame(i).Data(5)
                    If (_LinInterface.RxFrame(i).Data(5) = "21" Or _LinInterface.RxFrame(i).Data(5) = "27") Then
                        _results.MMS_Read_Test_Byte.MinimumLimit = _LinInterface.RxFrame(i).Data(5)
                        _results.MMS_Read_Test_Byte.MaximumLimit = _LinInterface.RxFrame(i).Data(5)
                    Else
                        _results.MMS_Read_Test_Byte.MinimumLimit = "00"
                        _results.MMS_Read_Test_Byte.MaximumLimit = "00"
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
                    _results.eRead_TraceabilityMMS.TestResult = cResultValue.eValueTestResult.Failed
                    ' Go to subphase
                    _subPhase(_phase) = 100
                End If
            Case 100
                ' Tests
                If (_results.MMS_Read_Test_Byte.Test <> cResultValue.eValueTestResult.Passed) Then
                    _results.eRead_TestByteMMS.TestResult =
                        cResultValue.eValueTestResult.Failed
                Else
                    _results.eRead_TestByteMMS.TestResult =
                        cResultValue.eValueTestResult.Passed
                End If
                ' Go to next subphase
                _subPhase(_phase) = 101

            Case 101
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                ' Updates the global test result
                If (_results.TestResult = cWS02Results.eTestResult.Unknown And
                    _results.eRead_TestByteMMS.TestResult <>
                        cResultValue.eValueTestResult.Passed) Then
                    _results.TestResult = cWS02Results.eTestResult.FailedeRead_TraceabilityMMS
                End If
                'Clear Subphase
                _subPhase(_phase) = 0
                ' Go to next phase
                _phase = ePhase.PowerDown


            Case 199
                _LinInterface.StopScheduleDiag()
                ' Adds a log entry
                AddLogEntry("Timeout on LIN")
                ' Update the test result
                _results.eRead_TestByteMMS.TestResult =
                    cResultValue.eValueTestResult.Failed

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
        End If

    End Sub



    Private Sub PhasePowerDown()
        Dim e As Boolean
        Dim sp As Integer
        Dim i As Integer
        Static t0Phase As Date
        Static t0 As Date
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
                ' Store the time
                t0 = Date.Now
                ' Go to next subphase
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
                ' Power-down
                For j = 0 To mWS02DIOManager.eDigitalOutput.Count - 1
                    e = e Or mWS02DIOManager.ResetDigitalOutput(j)
                Next
                e = mWS02DIOManager.SetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_LocalSensing)
                e = e Or mWS02DIOManager.ResetDigitalOutput(mWS02DIOManager.eDigitalOutput.DO_RemoteSensing)
                ' Power-down the CAN interface
                e = e Or _LinInterface.PowerDown()
                ' Add log entry
                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
                '
                _subPhase(_phase) = 0
                ' Go to phase WriteResults
                _phase = ePhase.WriteResults
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

        _recipeMaster = New cWS02Recipe

        CheckMasterReference = False

        Try
            _recipeMaster.LoadConfiguration(_settings.RecipeConfigurationPath)
            ' If the recipe in progress loading succeeds
            If Not (_recipe.Load(name)) Then
                MasterReference = True
                ' If the recipe of master reference loading succeeds
                If Not (_recipeMaster.Load(name)) Then
                    'Check parameter
                    For j = 10 To cWS02Recipe.ValueCount - 1
                        If _recipeMaster.Value(j) IsNot Nothing Then
                            If (_recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.BCDRange Or
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.HexRange Or
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.IntegerRange Or
                                _recipeMaster.Value(j).ValueType = cRecipeValue.eValueType.SingleRange) Then
                                If (_recipeMaster.Value(j).MinimumLimit <> _recipe.Value(j).MinimumLimit) Then
                                    ' Return error 
                                    CheckMasterReference = True
                                ElseIf (_recipeMaster.Value(j).MaximumLimit <> _recipe.Value(j).MaximumLimit) Then
                                    ' Return error 
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
                                    CheckMasterReference = True
                                End If
                            End If
                        End If
                    Next j
                    ' Otherwise, if the recipe master reference loading fails
                Else
                    ' Return error 
                    CheckMasterReference = True
                End If
                ' Otherwise, if the recipe in progress loading fails
            Else
                ' Return True
                CheckMasterReference = True
            End If

        Catch ex As Exception
            ' Return True
            _results.TestResult = cWS02Results.eTestResult.FailedMasterReference

            CheckMasterReference = True

        End Try

        MasterReference = False
    End Function

    Private Sub ClearResults()
        Dim i As Integer
        Dim c As Integer
        Dim r As Integer
        ' Clear all the values
        For i = 0 To cWS02Results.ValueCount - 1
            If (_results.Value(i) IsNot Nothing) Then
                If (_results.Value(i).ValueType = cResultValue.eValueType.BCDValue Or
                    _results.Value(i).ValueType = cResultValue.eValueType.HexValue Or
                    _results.Value(i).ValueType = cResultValue.eValueType.StringValue) Then
                    _results.Value(i).MinimumLimit = ""
                    _results.Value(i).MaximumLimit = ""
                    _results.Value(i).Value = ""
                    _results.Value(i).TestResult = cResultValue.eValueTestResult.Disabled
                ElseIf (_results.Value(i).ValueType = cResultValue.eValueType.IntegerValue Or
                    _results.Value(i).ValueType = cResultValue.eValueType.SingleValue) Then
                    _results.Value(i).MinimumLimit = 0
                    _results.Value(i).MaximumLimit = 0
                    _results.Value(i).Value = 0
                    _results.Value(i).TestResult = cResultValue.eValueTestResult.Disabled
                End If
            End If
        Next
        ' Set the global test result to Unknown
        _results.TestResult = cWS02Results.eTestResult.Unknown
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
        'If (_HardwareEnabled_PLC) Then
        '    _results.PartUniqueNumber.Value = Trim(mWS02Ethernet.InputValue(mWS02Ethernet.eInput.UniqueNumber))

        '    _results.PartUniqueNumber.Value = Right("0000000000" & _results.PartUniqueNumber.Value, 10)
        'End If
        _results.PartUniqueNumber.TestResult = cResultValue.eValueTestResult.NotTested
        ' Part type number
        _results.PartTypeNumber.Value = 0
        If (_HardwareEnabled_PLC) Then
            _results.PartTypeNumber.Value = mWS02Ethernet.InputValue(mWS02Ethernet.eInput.PartTypeNumber)
            _results.Side_Barcode.Value = mWS02Ethernet.InputValue(mWS02Ethernet.eInput.Side_Barcode).ToString().Replace("$R", "").Trim
            _results.FixtureID.Value = mWS02Ethernet.InputValue(mWS02Ethernet.eInput.FixtureID)
        End If
        If _results.PartTypeNumber.Value = 0 Or _results.PartTypeNumber.Value = 23 Then
            _MasterPart = False
        Else
            _MasterPart = True
        End If

        _results.PartTypeNumber.TestResult = cResultValue.eValueTestResult.NotTested
        ' Power UP
        _results.ePOWER_UP.TestResult = cResultValue.eValueTestResult.Unknown
        _results.PowerUp_VBAT.TestResult = cResultValue.eValueTestResult.Unknown
        _results.PowerUp_VBAT.MaximumLimit = _recipe.PowersupplyVbat.MaximumLimit
        _results.PowerUp_VBAT.Value = 0
        _results.PowerUp_VBAT.MinimumLimit = _recipe.PowersupplyVbat.MinimumLimit

        _results.PowerUp_Ibat.TestResult = cResultValue.eValueTestResult.Unknown
        _results.PowerUp_Ibat.MaximumLimit = _recipe.PowersupplyIbat.MaximumLimit
        _results.PowerUp_Ibat.Value = 0
        _results.PowerUp_Ibat.MinimumLimit = _recipe.PowersupplyIbat.MinimumLimit

        ' Init Communication
        _results.eINIT_LIN_COMMUNICATION.TestResult = cResultValue.eValueTestResult.Unknown
        _results.eOPEN_DIAG_ON_LIN_SESSION.TestResult = cResultValue.eValueTestResult.Unknown

        ' Check Serial Number
        _results.eRead_MMS_Serial_Number.TestResult = cResultValue.eValueTestResult.Unknown
        _results.Valeo_Serial_Number.TestResult = cResultValue.eValueTestResult.Unknown
        _results.Valeo_Serial_Number.MinimumLimit = "FFFFFFFFFF"
        _results.Valeo_Serial_Number.Value = 0
        _results.Valeo_Serial_Number.MaximumLimit = "FFFFFFFFFF"

        If _recipe.TestEnable_WriteConfiguration.Value Then
            _results.eWriteConfiguration.TestResult = cResultValue.eValueTestResult.Unknown

            If Not _recipe.TestEnable_EV_Option.Value Then
                _results.SW_Coding.TestResult = cResultValue.eValueTestResult.Unknown
                _results.SW_Coding.MinimumLimit = _recipe.WRITE_SW_Coding.Value & _recipe.WRITE_SW_Coding.Value
                _results.SW_Coding.MaximumLimit = _recipe.WRITE_SW_Coding.Value & _recipe.WRITE_SW_Coding.Value

                _results.Backlight_Coding.TestResult = cResultValue.eValueTestResult.Unknown
                _results.Backlight_Coding.MinimumLimit = _recipe.WRITE_Baclight_Coding.Value
                _results.Backlight_Coding.MaximumLimit = _recipe.WRITE_Baclight_Coding.Value
            Else
                _results.HW_Coding.TestResult = cResultValue.eValueTestResult.Unknown
                _results.HW_Coding.MinimumLimit = _recipe.WRITE_HW_Coding.Value
                _results.HW_Coding.MaximumLimit = _recipe.WRITE_HW_Coding.Value
            End If
        End If

        If CBool(_recipe.TestEnable_NormalMode.Value) Then
            _results.eNormalModeCurrent.TestResult = cResultValue.eValueTestResult.Unknown
        End If

        If CBool(_recipe.TestEnable_AnalogInput.Value) Then
            _results.eAnalogInput.TestResult = cResultValue.eValueTestResult.Unknown
            If Not CBool(_recipe.TestEnable_EV_Option.Value) Then
                _results.ADC_UCAD_VARIANT_1.TestResult = cResultValue.eValueTestResult.Unknown
                _results.ADC_UCAD_VARIANT_1.MinimumLimit = _recipe.ADC_UCAD_Variant_1.MinimumLimit
                _results.ADC_UCAD_VARIANT_1.MaximumLimit = _recipe.ADC_UCAD_Variant_1.MaximumLimit
                _results.ADC_UCAD_VARIANT_1.Value = 0

                _results.ADC_UCAD_VARIANT_2.TestResult = cResultValue.eValueTestResult.Unknown
                _results.ADC_UCAD_VARIANT_2.MinimumLimit = _recipe.ADC_UCAD_Variant_2.MinimumLimit
                _results.ADC_UCAD_VARIANT_2.MaximumLimit = _recipe.ADC_UCAD_Variant_2.MaximumLimit
                _results.ADC_UCAD_VARIANT_2.Value = 0
            End If
            If _recipe.TestEnable_MEMO_Option.Value Then
                _results.ADC_CURSEUR_LEFT_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
                _results.ADC_CURSEUR_LEFT_RIGHT.MinimumLimit = _recipe.ADC_CURSEUR_LEFT_RIGHT.MinimumLimit
                _results.ADC_CURSEUR_LEFT_RIGHT.MaximumLimit = _recipe.ADC_CURSEUR_LEFT_RIGHT.MaximumLimit

                _results.ADC_CURSEUR_UP_DN.TestResult = cResultValue.eValueTestResult.Unknown
                _results.ADC_CURSEUR_UP_DN.MinimumLimit = _recipe.ADC_CURSEUR_UP_DN.MinimumLimit
                _results.ADC_CURSEUR_UP_DN.MaximumLimit = _recipe.ADC_CURSEUR_UP_DN.MaximumLimit

            End If
        End If
        If CBool(_recipe.TestEnable_Temperature.Value) And _recipe.TestEnable_EV_Option.Value Then
            _results.eWrite_Temperature_Set.TestResult = cResultValue.eValueTestResult.Unknown

            _results.ADC_Temp.TestResult = cResultValue.eValueTestResult.NotTested
            _results.External_Temp.TestResult = cResultValue.eValueTestResult.NotTested

        End If

        If CBool(_recipe.TestEnable_DigitalOutput.Value) Then
            _results.eDigitalOutput.TestResult = cResultValue.eValueTestResult.Unknown
            ' PWL(1.2.3.4.5.6.7)
            If _recipe.TestEnable_PWL_Option.Value Then
                'O_UP_FRONT_PASSENGER_CDE
                For c = 0 To 1
                    For r = 0 To 20
                        If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                            r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                            r <> eSdt_Signal.CDE_HB_RTRV_D And
                             r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                            Not _recipe.TestEnable_MEMO_Option.Value Then
                            _results.STsgn_Front_Passenger_Commande_UP(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_Front_Passenger_Commande_UP(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_Front_Passenger_Commande_UP(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = eON_OFF.Fct_ON And r = eSdt_Signal.O_UP_FRONT_PASSENGER_CDE Then
                                _results.STsgn_Front_Passenger_Commande_UP(r, c).MinimumLimit = _recipe.DO_UP_Front_Passenger.MinimumLimit
                                _results.STsgn_Front_Passenger_Commande_UP(r, c).MaximumLimit = _recipe.DO_UP_Front_Passenger.MaximumLimit
                            End If
                        End If
                    Next
                Next
                'O_DN_FRONT_PASSENGER_CDE
                For c = 0 To 1
                    For r = 0 To 20
                        If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                              r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                              r <> eSdt_Signal.CDE_HB_RTRV_D And
                             r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                              Not _recipe.TestEnable_MEMO_Option.Value Then
                            _results.STsgn_Front_Passenger_Commande_Down(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_Front_Passenger_Commande_Down(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_Front_Passenger_Commande_Down(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = eON_OFF.Fct_ON And r = eSdt_Signal.O_DOWN_FRONT_PASSENGER_CDE Then
                                _results.STsgn_Front_Passenger_Commande_Down(r, c).MinimumLimit = _recipe.DO_DN_Front_Passenger.MinimumLimit
                                _results.STsgn_Front_Passenger_Commande_Down(r, c).MaximumLimit = _recipe.DO_DN_Front_Passenger.MaximumLimit
                            End If
                        End If
                    Next
                Next
                'O_LOCAL_WL_SWITCHES_INHIBITION_CDE
                For c = 0 To 1
                    For r = 0 To 20
                        If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                              r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                              r <> eSdt_Signal.CDE_HB_RTRV_D And
                             r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                              Not _recipe.TestEnable_MEMO_Option.Value Then
                            _results.STsgn_Switches_Inhibition(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_Switches_Inhibition(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_Switches_Inhibition(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = eON_OFF.Fct_ON And r = eSdt_Signal.O_LOCAL_WL_SWITCHES_INHIBITION_CDE Then
                                _results.STsgn_Switches_Inhibition(r, c).MinimumLimit = _recipe.DO_Switch_Inhibtion.MinimumLimit
                                _results.STsgn_Switches_Inhibition(r, c).MaximumLimit = _recipe.DO_Switch_Inhibtion.MaximumLimit
                            End If
                        End If
                    Next
                Next
                'O_DOWN_REAR_RIGHT_CDE
                For c = 0 To 1
                    For r = 0 To 20
                        If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                             r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                             r <> eSdt_Signal.CDE_HB_RTRV_D And
                             r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                             Not _recipe.TestEnable_MEMO_Option.Value Then
                            _results.STsgn_Down_Rear_Right(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_Down_Rear_Right(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_Down_Rear_Right(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = eON_OFF.Fct_ON And r = eSdt_Signal.O_DOWN_REAR_RIGHT_CDE Then
                                _results.STsgn_Down_Rear_Right(r, c).MinimumLimit = _recipe.DO_DN_Rear_Right.MinimumLimit
                                _results.STsgn_Down_Rear_Right(r, c).MaximumLimit = _recipe.DO_DN_Rear_Right.MaximumLimit
                            End If
                        End If
                    Next
                Next
                'O_UP_REAR_RIGHT_CDE
                For c = 0 To 1
                    For r = 0 To 20
                        If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                             r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                             r <> eSdt_Signal.CDE_HB_RTRV_D And
                             r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                             Not _recipe.TestEnable_MEMO_Option.Value Then
                            _results.STsgn_UP_Rear_Right(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_UP_Rear_Right(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_UP_Rear_Right(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = eON_OFF.Fct_ON And r = eSdt_Signal.O_UP_REAR_RIGHT_CDE Then
                                _results.STsgn_UP_Rear_Right(r, c).MinimumLimit = _recipe.DO_UP_Rear_Right.MinimumLimit
                                _results.STsgn_UP_Rear_Right(r, c).MaximumLimit = _recipe.DO_UP_Rear_Right.MaximumLimit
                            End If
                        End If
                    Next
                Next
                'O_DOWN_REAR_LEFT_CDE
                For c = 0 To 1
                    For r = 0 To 20
                        If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                             r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                             r <> eSdt_Signal.CDE_HB_RTRV_D And
                             r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                             Not _recipe.TestEnable_MEMO_Option.Value Then
                            _results.STsgn_Down_Rear_Left(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_Down_Rear_Left(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_Down_Rear_Left(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = eON_OFF.Fct_ON And r = eSdt_Signal.O_DOWN_REAR_LEFT_CDE Then
                                _results.STsgn_Down_Rear_Left(r, c).MinimumLimit = _recipe.DO_DN_Rear_Left.MinimumLimit
                                _results.STsgn_Down_Rear_Left(r, c).MaximumLimit = _recipe.DO_DN_Rear_Left.MaximumLimit
                            End If
                        End If
                    Next
                Next
                'O_UP_REAR_LEFT_CDE
                For c = 0 To 1
                    For r = 0 To 20
                        If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                             r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                             r <> eSdt_Signal.CDE_HB_RTRV_D And
                             r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                             Not _recipe.TestEnable_MEMO_Option.Value Then
                            _results.STsgn_UP_Rear_Left(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_UP_Rear_Left(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_UP_Rear_Left(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = eON_OFF.Fct_ON And r = eSdt_Signal.O_UP_REAR_LEFT_CDE Then
                                _results.STsgn_UP_Rear_Left(r, c).MinimumLimit = _recipe.DO_UP_Rear_Left.MinimumLimit
                                _results.STsgn_UP_Rear_Left(r, c).MaximumLimit = _recipe.DO_UP_Rear_Left.MaximumLimit
                            End If
                        End If
                    Next
                Next
            End If
            ' CDE_H/B_RTRV_G & CDE_HB_RTRV_D(10.Activate the CDE_H/B_RTRV_G)
            For c = 0 To 1
                For r = 0 To 20
                    If (_recipe.TestEnable_MEMO_Option.Value And r = eSdt_Signal.CDE_HB_RTRV_D) Or
                        (_recipe.TestEnable_MEMO_Option.Value And r = eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                        r = eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR Or
                        r = eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR Then
                        Continue For
                    End If
                    _results.STsgn_CDE_HB_RTRV_G(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                    _results.STsgn_CDE_HB_RTRV_G(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                    _results.STsgn_CDE_HB_RTRV_G(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                    If c = eON_OFF.Fct_ON And r = eSdt_Signal.CDE_HB_RTRV_G Then
                        _results.STsgn_CDE_HB_RTRV_G(r, c).MinimumLimit = _recipe.DO_CDE_HB_RTRV_G(0).MinimumLimit
                        _results.STsgn_CDE_HB_RTRV_G(r, c).MaximumLimit = _recipe.DO_CDE_HB_RTRV_G(0).MaximumLimit
                    ElseIf c = eON_OFF.Fct_ON And r = eSdt_Signal.CDE_HB_RTRV_D And Not _recipe.TestEnable_MEMO_Option.Value Then
                        _results.STsgn_CDE_HB_RTRV_G(r, c).MinimumLimit = _recipe.DO_CDE_HB_RTRV_G(1).MinimumLimit
                        _results.STsgn_CDE_HB_RTRV_G(r, c).MaximumLimit = _recipe.DO_CDE_HB_RTRV_G(1).MaximumLimit
                    End If
                Next
            Next
            ' CDE_SGN_COMMUN_MOT_RTRV_D(12.Activate the SGN_COMMUN_MOT_RTRV_D)
            If Not _recipe.TestEnable_MEMO_Option.Value Then
                For c = 0 To 1
                    For r = 0 To 20
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                        If c = eON_OFF.Fct_ON And r = eSdt_Signal.SGN_COMMUN_MOT_RTRV_D Then
                            _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, c).MinimumLimit = _recipe.D0_SGN_COMMUN_MOT_RTRV_D.MinimumLimit
                            _results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, c).MaximumLimit = _recipe.D0_SGN_COMMUN_MOT_RTRV_D.MaximumLimit
                        End If
                    Next
                Next
            End If
            ' CDE_D/G_RTRV_D(13.Drive Output CDE_D/G_RTRV_D)
            If Not _recipe.TestEnable_MEMO_Option.Value Then
                For c = 0 To 1
                    For r = 0 To 20
                        _results.STsgn_CDE_DG_RTRV_D(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                        _results.STsgn_CDE_DG_RTRV_D(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                        _results.STsgn_CDE_DG_RTRV_D(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                        If c = eON_OFF.Fct_ON And r = eSdt_Signal.CDE_DG_RTRV_D Then
                            _results.STsgn_CDE_DG_RTRV_D(r, c).MinimumLimit = _recipe.DO_CDE_DG_RTRV_D(0).MinimumLimit
                            _results.STsgn_CDE_DG_RTRV_D(r, c).MaximumLimit = _recipe.DO_CDE_DG_RTRV_D(0).MaximumLimit
                        ElseIf c = eON_OFF.Fct_ON And r = eSdt_Signal.CDE_M_RBT_RTRV_D And _recipe.TestEnable_FOLDING_Option.Value Then
                            _results.STsgn_CDE_DG_RTRV_D(r, c).MinimumLimit = _recipe.DO_CDE_RBT_RTRV_D(1).MinimumLimit
                            _results.STsgn_CDE_DG_RTRV_D(r, c).MaximumLimit = _recipe.DO_CDE_RBT_RTRV_D(1).MaximumLimit
                        End If
                    Next
                    If Not _recipe.TestEnable_MEMO_Option.Value And _recipe.TestEnable_FOLDING_Option.Value Then
                        _results.ADC_CDE_DG_RTRV_D(c).TestResult = cResultValue.eValueTestResult.Unknown
                        _results.ADC_CDE_DG_RTRV_D(c).MinimumLimit = _recipe.ADC_CDE_DG_RTRV_D(c).MinimumLimit
                        _results.ADC_CDE_DG_RTRV_D(c).MaximumLimit = _recipe.ADC_CDE_DG_RTRV_D(c).MaximumLimit
                    End If
                Next
            End If
            ' CDE_D/G_RTRV_G(14.Drive Output CDE_D/G_RTRV_G)
            For c = 0 To 1
                For r = 0 To 20
                    If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                      r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                      r <> eSdt_Signal.CDE_HB_RTRV_D And
                         r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                      Not _recipe.TestEnable_MEMO_Option.Value Then
                        _results.STsgn_CDE_DG_RTRV_G(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                        _results.STsgn_CDE_DG_RTRV_G(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                        _results.STsgn_CDE_DG_RTRV_G(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                        If c = eON_OFF.Fct_ON And r = eSdt_Signal.CDE_DG_RTRV_G Then
                            _results.STsgn_CDE_DG_RTRV_G(r, c).MinimumLimit = _recipe.DO_CDE_DG_RTRV_G(0).MinimumLimit
                            _results.STsgn_CDE_DG_RTRV_G(r, c).MaximumLimit = _recipe.DO_CDE_DG_RTRV_G(0).MaximumLimit
                        ElseIf c = eON_OFF.Fct_ON And r = eSdt_Signal.CDE_M_RBT_RTRV_G And _recipe.TestEnable_FOLDING_Option.Value Then
                            _results.STsgn_CDE_DG_RTRV_G(r, c).MinimumLimit = _recipe.DO_CDE_RBT_RTRV_G(1).MinimumLimit
                            _results.STsgn_CDE_DG_RTRV_G(r, c).MaximumLimit = _recipe.DO_CDE_RBT_RTRV_G(1).MaximumLimit
                        End If
                    End If
                Next
                If _recipe.TestEnable_FOLDING_Option.Value Then
                    _results.ADC_CDE_DG_RTRV_G(c).TestResult = cResultValue.eValueTestResult.Unknown
                    _results.ADC_CDE_DG_RTRV_G(c).MinimumLimit = _recipe.ADC_CDE_DG_RTRV_G(c).MinimumLimit
                    _results.ADC_CDE_DG_RTRV_G(c).MaximumLimit = _recipe.ADC_CDE_DG_RTRV_G(c).MaximumLimit
                End If
            Next
            ' CDE_RBT_RTRV_G & CDE_RBT_RTRV_D(8.Drive Output CDE_+_RBT_RTRV_G,9.Drive Output CDE_+_RBT_RTRV_D)
            If _recipe.TestEnable_FOLDING_Option.Value Then
                'CDE_RBT_RTRV_G
                For c = 0 To 1
                    ' c=0 Fonction Folding
                    ' C=1 Stop
                    For r = 0 To 20
                        If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                        r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                        r <> eSdt_Signal.CDE_HB_RTRV_D And
                        r <> eSdt_Signal.CDE_DG_RTRV_D And
                         r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                        Not _recipe.TestEnable_MEMO_Option.Value Then
                            _results.STsgn_CDE_RBT_RTRV_G(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_CDE_RBT_RTRV_G(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_CDE_RBT_RTRV_G(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = 0 And r = eSdt_Signal.CDE_P_RBT_RTRV_G Then
                                _results.STsgn_CDE_RBT_RTRV_G(r, c).MinimumLimit = _recipe.DO_CDE_RBT_RTRV_G(0).MinimumLimit
                                _results.STsgn_CDE_RBT_RTRV_G(r, c).MaximumLimit = _recipe.DO_CDE_RBT_RTRV_G(0).MaximumLimit
                            End If
                        End If
                    Next
                    _results.ADC_CDE_RBT_RTRV_G(c).TestResult = cResultValue.eValueTestResult.Unknown
                    _results.ADC_CDE_RBT_RTRV_G(c).MinimumLimit = _recipe.ADC_CDE_RBT_RTRV_G(c).MinimumLimit
                    _results.ADC_CDE_RBT_RTRV_G(c).MaximumLimit = _recipe.ADC_CDE_RBT_RTRV_G(c).MaximumLimit
                Next
                ' CDE_RBT_RTRV_D
                If Not _recipe.TestEnable_MEMO_Option.Value Then
                    For c = 0 To 1
                        ' c=0 Fonction Folding
                        ' C=1 Stop
                        For r = 0 To 20
                            _results.STsgn_CDE_RBT_RTRV_D(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                            _results.STsgn_CDE_RBT_RTRV_D(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                            _results.STsgn_CDE_RBT_RTRV_D(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                            If c = eON_OFF.Fct_ON And r = eSdt_Signal.CDE_P_RBT_RTRV_D Then
                                _results.STsgn_CDE_RBT_RTRV_D(r, c).MinimumLimit = _recipe.DO_CDE_RBT_RTRV_D(0).MinimumLimit
                                _results.STsgn_CDE_RBT_RTRV_D(r, c).MaximumLimit = _recipe.DO_CDE_RBT_RTRV_D(0).MaximumLimit
                            End If
                        Next
                        _results.ADC_CDE_RBT_RTRV_D(c).TestResult = cResultValue.eValueTestResult.Unknown
                        _results.ADC_CDE_RBT_RTRV_D(c).MinimumLimit = _recipe.ADC_CDE_RBT_RTRV_D(c).MinimumLimit
                        _results.ADC_CDE_RBT_RTRV_D(c).MaximumLimit = _recipe.ADC_CDE_RBT_RTRV_D(c).MaximumLimit
                    Next
                End If
            End If
            ' SGN_COMMUN_MOT_RTRV_G(11.Activate the SGN_COMMUN_MOT_RTRV_G)
            For c = 0 To 1
                For r = 0 To 20
                    If (r <> eSdt_Signal.COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                      r <> eSdt_Signal.COMMUN_SUPPLY_P_OF_PTN_DRIVER_EXTERNAL_MIRROR And
                      r <> eSdt_Signal.CDE_HB_RTRV_D And
                      r <> eSdt_Signal.CDE_DG_RTRV_D And
                         r <> eSdt_Signal.SGN_COMMUN_MOT_RTRV_D) Or
                      Not _recipe.TestEnable_MEMO_Option.Value Then
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, c).TestResult = cResultValue.eValueTestResult.Unknown
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, c).MinimumLimit = _recipe.Sdt_Signal(r).MinimumLimit
                        _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, c).MaximumLimit = _recipe.Sdt_Signal(r).MaximumLimit
                        If c = eON_OFF.Fct_ON And r = eSdt_Signal.SGN_COMMUN_MOT_RTRV_G Then
                            _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, c).MinimumLimit = _recipe.D0_SGN_COMMUN_MOT_RTRV_G.MinimumLimit
                            _results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, c).MaximumLimit = _recipe.D0_SGN_COMMUN_MOT_RTRV_G.MaximumLimit
                        End If
                    End If
                Next
            Next
        End If

        If CBool(_recipe.TestEnable_PWMOutput.Value) Then
            _results.ePWMOutput.TestResult = cResultValue.eValueTestResult.Unknown

            _results.UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12.MinimumLimit = _recipe.External_Backlight_WLAP_Voltage_LOW_X1_12.MinimumLimit
            _results.UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12.MaximumLimit = _recipe.External_Backlight_WLAP_Voltage_LOW_X1_12.MaximumLimit

            _results.UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12.MinimumLimit = _recipe.External_Backlight_WLAP_Voltage_HIGH_X1_12.MinimumLimit
            _results.UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12.MaximumLimit = _recipe.External_Backlight_WLAP_Voltage_HIGH_X1_12.MaximumLimit

            _results.UCDA_SINGLE_SW_DIAG_Frequency_X1_12.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_SINGLE_SW_DIAG_Frequency_X1_12.MinimumLimit = _recipe.External_Backlight_WLAP_Frequency_X1_12.MinimumLimit
            _results.UCDA_SINGLE_SW_DIAG_Frequency_X1_12.MaximumLimit = _recipe.External_Backlight_WLAP_Frequency_X1_12.MaximumLimit

            _results.UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12.MinimumLimit = _recipe.External_Backlight_WLAP_Duty_Cycle_X1_12.MinimumLimit
            _results.UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12.MaximumLimit = _recipe.External_Backlight_WLAP_Duty_Cycle_X1_12.MaximumLimit

            _results.UCDA_SINGLE_SW_DIAG.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_SINGLE_SW_DIAG.MinimumLimit = _recipe.External_Backlight_WLAP_ADC_VALUE.MinimumLimit
            _results.UCDA_SINGLE_SW_DIAG.MaximumLimit = _recipe.External_Backlight_WLAP_ADC_VALUE.MaximumLimit

            _results.UCDA_SINGLE_SW_DIAG_RiseTime_X1_12.TestResult = cResultValue.eValueTestResult.NotTested
            _results.UCDA_SINGLE_SW_DIAG_FallTime_X1_12.TestResult = cResultValue.eValueTestResult.NotTested

            _results.UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24.MinimumLimit = _recipe.External_Backlight_DOOR_LOCK_Voltage_LOW_X1_24.MinimumLimit
            _results.UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24.MaximumLimit = _recipe.External_Backlight_DOOR_LOCK_Voltage_LOW_X1_24.MaximumLimit

            _results.UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24.MinimumLimit = _recipe.External_Backlight_DOOR_LOCK_Voltage_HIGH_X1_24.MinimumLimit
            _results.UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24.MaximumLimit = _recipe.External_Backlight_DOOR_LOCK_Voltage_HIGH_X1_24.MaximumLimit

            _results.UCDA_DOOR_LOCK_DIAG_Frequency_X1_24.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_DOOR_LOCK_DIAG_Frequency_X1_24.MinimumLimit = _recipe.External_Backlight_DOOR_LOCK_Frequency_X1_24.MinimumLimit
            _results.UCDA_DOOR_LOCK_DIAG_Frequency_X1_24.MaximumLimit = _recipe.External_Backlight_DOOR_LOCK_Frequency_X1_24.MaximumLimit

            _results.UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24.MinimumLimit = _recipe.External_Backlight_DOOR_LOCK_Duty_Cycle_X1_24.MinimumLimit
            _results.UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24.MaximumLimit = _recipe.External_Backlight_DOOR_LOCK_Duty_Cycle_X1_24.MaximumLimit

            _results.UCDA_DOOR_LOCK_DIAG.TestResult = cResultValue.eValueTestResult.Unknown
            _results.UCDA_DOOR_LOCK_DIAG.MinimumLimit = _recipe.External_Backlight_DOOR_LOCK_ADC_VALUE.MinimumLimit
            _results.UCDA_DOOR_LOCK_DIAG.MaximumLimit = _recipe.External_Backlight_DOOR_LOCK_ADC_VALUE.MaximumLimit

            _results.UCDA_DOOR_LOCK_DIAG_RiseTime_X1_24.TestResult = cResultValue.eValueTestResult.NotTested
            _results.UCDA_DOOR_LOCK_DIAG_FallTime_X1_24.TestResult = cResultValue.eValueTestResult.NotTested

        End If
        If CBool(_recipe.TestEnable_EMSTraceability.Value) Then
            _results.eEmsTraceability.TestResult = cResultValue.eValueTestResult.Unknown

            _results.PCBA_Number_Reference.TestResult = cResultValue.eValueTestResult.Unknown
            _results.PCBA_Number_Reference.MinimumLimit = _recipe.PCBA_Number_Reference.Value
            _results.PCBA_Number_Reference.MaximumLimit = _recipe.PCBA_Number_Reference.Value
            _results.PCBA_Number_Reference.Value = "FFFFFFFFFF"

            _results.PCBA_Number_Index.TestResult = cResultValue.eValueTestResult.Disabled
            '_results.PCBA_Number_Index.MinimumLimit = _recipe.PCBA_Number_Index.Value
            '_results.PCBA_Number_Index.MaximumLimit = _recipe.PCBA_Number_Index.Value
            '_results.PCBA_Number_Index.Value = "FFFFFFFF"

            _results.PCBA_Plant_Line.TestResult = cResultValue.eValueTestResult.Unknown
            _results.PCBA_Plant_Line.MinimumLimit = _recipe.PCBA_Plant_Line.MinimumLimit
            _results.PCBA_Plant_Line.MaximumLimit = _recipe.PCBA_Plant_Line.MaximumLimit
            _results.PCBA_Plant_Line.Value = "FFFF"

            _results.PCBA_ManufacturingDate.TestResult = cResultValue.eValueTestResult.Unknown
            _results.PCBA_ManufacturingDate.MinimumLimit = _recipe.PCBA_ManufacturingDate.MinimumLimit
            _results.PCBA_ManufacturingDate.MaximumLimit = _recipe.PCBA_ManufacturingDate.MaximumLimit
            _results.PCBA_ManufacturingDate.Value = "FFFFFF"

            _results.PCBA_SerialNumber.TestResult = cResultValue.eValueTestResult.Unknown
            _results.PCBA_SerialNumber.MinimumLimit = _recipe.PCBA_SerialNumber.MinimumLimit
            _results.PCBA_SerialNumber.MaximumLimit = _recipe.PCBA_SerialNumber.MaximumLimit
            _results.PCBA_SerialNumber.Value = "FFFFFFFF"

            _results.PCBA_DeviationNumber.TestResult = cResultValue.eValueTestResult.Unknown
            _results.PCBA_DeviationNumber.MinimumLimit = _recipe.PCBA_DeviationNumber.MinimumLimit
            _results.PCBA_DeviationNumber.MaximumLimit = _recipe.PCBA_DeviationNumber.MaximumLimit
            _results.PCBA_DeviationNumber.Value = "FF"

            If Not _recipe.TestEnable_EV_Option.Value Then
                _results.LED_BIN_PT_White_RSA.TestResult = cResultValue.eValueTestResult.Unknown
                _results.LED_BIN_PT_White_RSA.MinimumLimit = _recipe.LED_BIN_PT_White_RSA.MinimumLimit
                _results.LED_BIN_PT_White_RSA.MaximumLimit = _recipe.LED_BIN_PT_White_RSA.MaximumLimit
                _results.LED_BIN_PT_White_RSA.Value = "FF"

                'Skipped Read LED RED
                _results.LED_BIN_PT_RED.TestResult = cResultValue.eValueTestResult.Disabled
                '_results.LED_BIN_PT_RED.MinimumLimit = _recipe.LED_BIN_PT_RED.MinimumLimit
                '_results.LED_BIN_PT_RED.MaximumLimit = _recipe.LED_BIN_PT_RED.MaximumLimit
                '_results.LED_BIN_PT_RED.Value = "FF"
            End If

            _results.LED_BIN_PT_YELLOW.TestResult = cResultValue.eValueTestResult.Unknown
            _results.LED_BIN_PT_YELLOW.MinimumLimit = _recipe.LED_BIN_PT_Yellow.MinimumLimit
            _results.LED_BIN_PT_YELLOW.MaximumLimit = _recipe.LED_BIN_PT_Yellow.MaximumLimit
            _results.LED_BIN_PT_YELLOW.Value = "FF"

            _results.LED_BIN_PT_WHITE_NISSAN.TestResult = cResultValue.eValueTestResult.Unknown
            _results.LED_BIN_PT_WHITE_NISSAN.MinimumLimit = _recipe.LED_BIN_PT_White_Nissan.MinimumLimit
            _results.LED_BIN_PT_WHITE_NISSAN.MaximumLimit = _recipe.LED_BIN_PT_White_Nissan.MaximumLimit
            _results.LED_BIN_PT_WHITE_NISSAN.Value = "FF"

            _results.EMS_Test_Byte.TestResult = cResultValue.eValueTestResult.Unknown
            _results.EMS_Test_Byte.MinimumLimit = "00"
            _results.EMS_Test_Byte.MaximumLimit = "00"
            _results.EMS_Test_Byte.Value = "FF"

            _results.Major_SoftwareVersion.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Major_SoftwareVersion.MinimumLimit = _recipe.Major_SoftwareVersion.Value
            _results.Major_SoftwareVersion.MaximumLimit = _recipe.Major_SoftwareVersion.Value
            _results.Major_SoftwareVersion.Value = 0

            _results.Minor_SoftwareVersion.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Minor_SoftwareVersion.MinimumLimit = _recipe.Minor_SoftwareVersion.Value
            _results.Minor_SoftwareVersion.MaximumLimit = _recipe.Minor_SoftwareVersion.Value
            _results.Minor_SoftwareVersion.Value = 0

            _results.Major_NVMversion.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Major_NVMversion.MinimumLimit = _recipe.Major_NVMversion.Value
            _results.Major_NVMversion.MaximumLimit = _recipe.Major_NVMversion.Value
            _results.Major_NVMversion.Value = 0

            _results.Minor_NVMversion.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Minor_NVMversion.MinimumLimit = _recipe.Minor_NVMversion.Value
            _results.Minor_NVMversion.MaximumLimit = _recipe.Minor_NVMversion.Value
            _results.Minor_NVMversion.Value = 0

            _results.SW_checksum.TestResult = cResultValue.eValueTestResult.Unknown
            _results.SW_checksum.MinimumLimit = _recipe.SW_checksum.Value
            _results.SW_checksum.MaximumLimit = _recipe.SW_checksum.Value
            _results.SW_checksum.Value = 0

            '_results.SW_Coding.TestResult = cResultValue.eValueTestResult.Unknown
            '_results.SW_Coding.MinimumLimit = _recipe.SW_Coding.Value
            '_results.SW_Coding.MaximumLimit = _recipe.SW_Coding.Value
            '_results.SW_Coding.Value = 0

        End If

        ' Camera Shape
        If CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            ' 
            _results.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR.MinimumLimit = _recipe.Push_Select_Left_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR.Value = 0
            ' 
            _results.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR.MinimumLimit = _recipe.Push_Select_Right_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR.Value = 0

            _results.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR.MaximumLimit = _recipe.Push_Select_Left_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR.Value = 0
            ' 
            _results.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR.MaximumLimit = _recipe.Push_Select_Right_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR.Value = 0

        End If
        If CBool(_recipe.TestEnable_SHAPE_Select_Mirror.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            _results.eAreaMirror_Conformity.TestResult = cResultValue.eValueTestResult.Unknown
            ' 
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_UP.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_UP.MinimumLimit = _recipe.Push_Adjust_UP_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_UP.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_UP.Value = 0
            ' 
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN.MinimumLimit = _recipe.Push_Adjust_Down_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN.Value = 0
            ' 
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT.MinimumLimit = _recipe.Push_Adjust_Left_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT.Value = 0
            ' 
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT.MinimumLimit = _recipe.Push_Adjust_Right_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT.Value = 0

            _results.DEFECT_AREA_PUSH_ADJUST_UP.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_ADJUST_UP.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_ADJUST_UP.MaximumLimit = _recipe.Push_Adjust_UP_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_ADJUST_UP.Value = 0
            ' 
            _results.DEFECT_AREA_PUSH_ADJUST_DOWN.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_ADJUST_DOWN.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_ADJUST_DOWN.MaximumLimit = _recipe.Push_Adjust_Down_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_ADJUST_DOWN.Value = 0
            ' 
            _results.DEFECT_AREA_PUSH_ADJUST_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_ADJUST_LEFT.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_ADJUST_LEFT.MaximumLimit = _recipe.Push_Adjust_Left_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_ADJUST_LEFT.Value = 0
            ' 
            _results.DEFECT_AREA_PUSH_ADJUST_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_ADJUST_RIGHT.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_ADJUST_RIGHT.MaximumLimit = _recipe.Push_Adjust_Right_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_ADJUST_RIGHT.Value = 0

            _results.Ring_Red.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Ring_Red.MinimumLimit = _recipe.Ring_Red.MinimumLimit
            _results.Ring_Red.MaximumLimit = _recipe.Ring_Red.MaximumLimit
            _results.Ring_Red.Value = 0

            _results.Ring_Green.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Ring_Green.MinimumLimit = _recipe.Ring_Green.MinimumLimit
            _results.Ring_Green.MaximumLimit = _recipe.Ring_Green.MaximumLimit
            _results.Ring_Green.Value = 0

            _results.Ring_Blue.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Ring_Blue.MinimumLimit = _recipe.Ring_Blue.MinimumLimit
            _results.Ring_Blue.MaximumLimit = _recipe.Ring_Blue.MaximumLimit
            _results.Ring_Blue.Value = 0
        End If
        If CBool(_recipe.TestEnable_SHAPE_Folding_Mirror.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            ' 
            _results.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR.MinimumLimit = _recipe.Push_Folding_pictogram_Conformity.MinimumLimit
            _results.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR.MaximumLimit = _recipe.Push_Folding_pictogram_Conformity.MaximumLimit
            _results.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR.Value = 0

            _results.DEFECT_AREA_PUSH_FOLDING_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_FOLDING_MIRROR.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_FOLDING_MIRROR.MaximumLimit = _recipe.Push_Folding_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_FOLDING_MIRROR.Value = 0
        End If
        If CBool(_recipe.TestEnable_SHAPE_Wili_Front.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            _results.eAreaWili_Conformity.TestResult = cResultValue.eValueTestResult.Unknown
            ' 
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT.MinimumLimit = _recipe.Windows_Lifter_Front_Left_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT.Value = 0
            ' 
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT.MinimumLimit = _recipe.Windows_Lifter_Front_Right_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT.Value = 0

            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT.MinimumLimit = 0
            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT.MaximumLimit = _recipe.Windows_Lifter_Front_Left_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT.Value = 0
            ' 
            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT.MinimumLimit = 0
            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT.MaximumLimit = _recipe.Windows_Lifter_Front_Right_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT.Value = 0

        End If
        If CBool(_recipe.TestEnable_SHAPE_Wili_Rear.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            _results.eAreaWili_Conformity.TestResult = cResultValue.eValueTestResult.Unknown
            ' 
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT.Value = 0
            ' 
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT.Value = 0

            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT.MinimumLimit = 0
            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT.Value = 0
            ' 
            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT.MinimumLimit = 0
            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT.Value = 0

        End If
        If CBool(_recipe.TestEnable_SHAPE_ChildrenLock.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            '
            _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK.MinimumLimit = _recipe.Push_Children_Lock_pictogram_Conformity.Value
            _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK.Value = 0

            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK.MaximumLimit = _recipe.Push_Children_Lock_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK.Value = 0

        End If
        If CBool(_recipe.TestEnable_SHAPE_ChildrenLock2.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            '
            _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2.TestResult = cResultValue.eValueTestResult.Unknown
            _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2.MinimumLimit = _recipe.Push_Children_Lock2_pictogram_Conformity.MinimumLimit
            _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2.MaximumLimit = 100
            _results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2.Value = 0

            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK2.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK2.MinimumLimit = 0
            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK2.MaximumLimit = _recipe.Push_Children_Lock2_pictogram_Defect_Area.Value
            _results.DEFECT_AREA_PUSH_CHILDREN_LOCK2.Value = 0

        End If
        If CBool(_recipe.TestEnable_Adapter.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            '
            _results.CONFORMITY_Adatper_Front.TestResult = cResultValue.eValueTestResult.Unknown
            _results.CONFORMITY_Adatper_Front.MinimumLimit = _recipe.Apater_Front_pictogram_Conformity.MinimumLimit
            _results.CONFORMITY_Adatper_Front.MaximumLimit = _recipe.Apater_Front_pictogram_Conformity.MaximumLimit
            _results.CONFORMITY_Adatper_Front.Value = 0

            _results.CONFORMITY_Adatper_Rear.TestResult = cResultValue.eValueTestResult.Unknown
            _results.CONFORMITY_Adatper_Rear.MinimumLimit = _recipe.Apater_Rear_pictogram_Conformity.MinimumLimit
            _results.CONFORMITY_Adatper_Rear.MaximumLimit = _recipe.Apater_Rear_pictogram_Conformity.MaximumLimit
            _results.CONFORMITY_Adatper_Rear.Value = 0

        End If
        If CBool(_recipe.TestEnable_CustomerInterface.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            '
            For index = 0 To 19
                _results.Customer_Interface(index).TestResult = cResultValue.eValueTestResult.Unknown
                _results.Customer_Interface(index).MinimumLimit = _recipe.CustomerInterface(index).MinimumLimit
                _results.Customer_Interface(index).MaximumLimit = _recipe.CustomerInterface(index).MaximumLimit
                _results.Customer_Interface(index).Value = 0
            Next

        End If
        If CBool(_recipe.TestEnable_PointsOnWili.Value) Then
            _results.eShape_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            '
            _results.Points_On_Front_Left.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Points_On_Front_Left.MinimumLimit = _recipe.PointsOn_Front_Left.MinimumLimit
            _results.Points_On_Front_Left.MaximumLimit = _recipe.PointsOn_Front_Left.MaximumLimit
            _results.Points_On_Front_Left.Value = 0

            _results.Points_On_Front_Right.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Points_On_Front_Right.MinimumLimit = _recipe.PointsOn_Front_Right.MinimumLimit
            _results.Points_On_Front_Right.MaximumLimit = _recipe.PointsOn_Front_Right.MaximumLimit
            _results.Points_On_Front_Right.Value = 0

            _results.Points_On_Rear_Left.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Points_On_Rear_Left.MinimumLimit = _recipe.PointsOn_Rear_Left.MinimumLimit
            _results.Points_On_Rear_Left.MaximumLimit = _recipe.PointsOn_Rear_Left.MaximumLimit
            _results.Points_On_Rear_Left.Value = 0

            _results.Points_On_Rear_Right.TestResult = cResultValue.eValueTestResult.Unknown
            _results.Points_On_Rear_Right.MinimumLimit = _recipe.PointsOn_Rear_Right.MinimumLimit
            _results.Points_On_Rear_Right.MaximumLimit = _recipe.PointsOn_Rear_Right.MaximumLimit
            _results.Points_On_Rear_Right.Value = 0

        End If
        'If CBool(_recipe.TestEnable_BezelConformity.Value) Then
        '    _results.eBezel_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
        '    ' Intensity
        '    _results.CONFORMITY_Bezel.MinimumLimit = _recipe.Bezel_Conformity.MinimumLimit
        '    _results.CONFORMITY_Bezel.MaximumLimit = _recipe.Bezel_Conformity.MaximumLimit
        '    _results.CONFORMITY_Bezel.Value = 0
        '    _results.CONFORMITY_Bezel.TestResult = cResultValue.eValueTestResult.Unknown

        'End If
        'If CBool(_recipe.TestEnable_DecorFrameConformity.Value) Then
        '    _results.eDecorFrame_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
        '    ' Intensity
        '    _results.CONFORMITY_DecorFrame.MinimumLimit = _recipe.Decor_Frame_Conformity.MinimumLimit
        '    _results.CONFORMITY_DecorFrame.MaximumLimit = _recipe.Decor_Frame_Conformity.MaximumLimit
        '    _results.CONFORMITY_DecorFrame.Value = 0
        '    _results.CONFORMITY_DecorFrame.TestResult = cResultValue.eValueTestResult.Unknown

        'End If
        ' Camera Backlight
        If CBool(_recipe.TestEnable_BACKLIGHT_Select_Mirror.Value) Then
            _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            ' Intensity
            _results.Push_SELECT_LEFT_backlight_intensity.MinimumLimit = _recipe.Push_Select_Left_Intensity.MinimumLimit
            _results.Push_SELECT_LEFT_backlight_intensity.MaximumLimit = _recipe.Push_Select_Left_Intensity.MaximumLimit
            _results.Push_SELECT_LEFT_backlight_intensity.Value = 0
            _results.Push_SELECT_LEFT_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_SELECT_LEFT_Backlight_RSQ.MinimumLimit = _recipe.Push_Select_Left_RSQ.MinimumLimit
            _results.Push_SELECT_LEFT_Backlight_RSQ.MaximumLimit = _recipe.Push_Select_Left_RSQ.MaximumLimit
            _results.Push_SELECT_LEFT_Backlight_RSQ.Value = 0
            _results.Push_SELECT_LEFT_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_SELECT_LEFT_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_SELECT_LEFT_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Select_Left_Polygon_Axy.MinimumLimit
            _results.Push_SELECT_LEFT_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Select_Left_Polygon_Axy.MaximumLimit
            '
            _results.Push_SELECT_LEFT_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Select_Left_Polygon_Bxy.MinimumLimit
            _results.Push_SELECT_LEFT_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Select_Left_Polygon_Bxy.MaximumLimit
            '
            _results.Push_SELECT_LEFT_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Select_Left_Polygon_Cxy.MinimumLimit
            _results.Push_SELECT_LEFT_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Select_Left_Polygon_Cxy.MaximumLimit
            '
            _results.Push_SELECT_LEFT_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Select_Left_Polygon_Dxy.MinimumLimit
            _results.Push_SELECT_LEFT_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Select_Left_Polygon_Dxy.MaximumLimit
            '
            _results.Push_SELECT_LEFT_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Select_Left_Polygon_Exy.MinimumLimit
            _results.Push_SELECT_LEFT_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Select_Left_Polygon_Exy.MaximumLimit
            '
            _results.Push_SELECT_LEFT_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Select_Left_Polygon_Fxy.MinimumLimit
            _results.Push_SELECT_LEFT_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Select_Left_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.Push_SELECT_LEFT_backlight_homogeneity.MinimumLimit = _recipe.Push_Children_Lock_minimum_homogeneity.Value
                _results.Push_SELECT_LEFT_backlight_homogeneity.Value = 0
                _results.Push_SELECT_LEFT_backlight_homogeneity.MaximumLimit = 100
                _results.Push_SELECT_LEFT_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR.MaximumLimit = _recipe.Push_Select_Left_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR.Value = 0

            ' Intensity
            _results.Push_SELECT_RIGHT_backlight_intensity.MinimumLimit = _recipe.Push_Select_Right_Intensity.MinimumLimit
            _results.Push_SELECT_RIGHT_backlight_intensity.MaximumLimit = _recipe.Push_Select_Right_Intensity.MaximumLimit
            _results.Push_SELECT_RIGHT_backlight_intensity.Value = 0
            _results.Push_SELECT_RIGHT_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_SELECT_RIGHT_Backlight_RSQ.MinimumLimit = _recipe.Push_Select_Right_RSQ.MinimumLimit
            _results.Push_SELECT_RIGHT_Backlight_RSQ.MaximumLimit = _recipe.Push_Select_Right_RSQ.MaximumLimit
            _results.Push_SELECT_RIGHT_Backlight_RSQ.Value = 0
            _results.Push_SELECT_RIGHT_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_SELECT_RIGHT_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Select_Right_Polygon_Axy.MinimumLimit
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Select_Right_Polygon_Axy.MaximumLimit
            '
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Select_Right_Polygon_Bxy.MinimumLimit
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Select_Right_Polygon_Bxy.MaximumLimit
            '
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Select_Right_Polygon_Cxy.MinimumLimit
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Select_Right_Polygon_Cxy.MaximumLimit
            '
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Select_Right_Polygon_Dxy.MinimumLimit
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Select_Right_Polygon_Dxy.MaximumLimit
            '
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Select_Right_Polygon_Exy.MinimumLimit
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Select_Right_Polygon_Exy.MaximumLimit
            '
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Select_Right_Polygon_Fxy.MinimumLimit
            _results.Push_SELECT_RIGHT_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Select_Right_Polygon_Fxy.MaximumLimit
            ' Homogeneit
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.Push_SELECT_RIGHT_backlight_homogeneity.MinimumLimit = _recipe.Push_Select_Right_minimum_homogeneity.Value
                _results.Push_SELECT_RIGHT_backlight_homogeneity.Value = 0
                _results.Push_SELECT_RIGHT_backlight_homogeneity.MaximumLimit = 100
                _results.Push_SELECT_RIGHT_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR.MaximumLimit = _recipe.Push_Select_Right_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR.Value = 0


            ' Intensity
            _results.Push_ADJUST_LEFT_backlight_intensity.MinimumLimit = _recipe.Push_Adjust_Left_Intensity.MinimumLimit
            _results.Push_ADJUST_LEFT_backlight_intensity.MaximumLimit = _recipe.Push_Adjust_Left_Intensity.MaximumLimit
            _results.Push_ADJUST_LEFT_backlight_intensity.Value = 0
            _results.Push_ADJUST_LEFT_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_ADJUST_LEFT_Backlight_RSQ.MinimumLimit = _recipe.Push_Adjust_Left_RSQ.MinimumLimit
            _results.Push_ADJUST_LEFT_Backlight_RSQ.MaximumLimit = _recipe.Push_Adjust_Left_RSQ.MaximumLimit
            _results.Push_ADJUST_LEFT_Backlight_RSQ.Value = 0
            _results.Push_ADJUST_LEFT_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_ADJUST_LEFT_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Adjust_Left_Polygon_Axy.MinimumLimit
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Adjust_Left_Polygon_Axy.MaximumLimit
            '
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Adjust_Left_Polygon_Bxy.MinimumLimit
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Adjust_Left_Polygon_Bxy.MaximumLimit
            '
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Adjust_Left_Polygon_Cxy.MinimumLimit
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Adjust_Left_Polygon_Cxy.MaximumLimit
            '
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Adjust_Left_Polygon_Dxy.MinimumLimit
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Adjust_Left_Polygon_Dxy.MaximumLimit
            '
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Adjust_Left_Polygon_Exy.MinimumLimit
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Adjust_Left_Polygon_Exy.MaximumLimit
            '
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Adjust_Left_Polygon_Fxy.MinimumLimit
            _results.Push_ADJUST_LEFT_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Adjust_Left_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.Push_ADJUST_LEFT_backlight_homogeneity.MinimumLimit = _recipe.Push_Adjust_Left_minimum_homogeneity.Value
                _results.Push_ADJUST_LEFT_backlight_homogeneity.Value = 0
                _results.Push_ADJUST_LEFT_backlight_homogeneity.MaximumLimit = 100
                _results.Push_ADJUST_LEFT_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT.MaximumLimit = _recipe.Push_Adjust_Left_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT.Value = 0

            ' Intensity
            _results.Push_ADJUST_UP_backlight_intensity.MinimumLimit = _recipe.Push_Adjust_UP_Intensity.MinimumLimit
            _results.Push_ADJUST_UP_backlight_intensity.MaximumLimit = _recipe.Push_Adjust_UP_Intensity.MaximumLimit
            _results.Push_ADJUST_UP_backlight_intensity.Value = 0
            _results.Push_ADJUST_UP_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_ADJUST_UP_Backlight_RSQ.MinimumLimit = _recipe.Push_Adjust_UP_RSQ.MinimumLimit
            _results.Push_ADJUST_UP_Backlight_RSQ.MaximumLimit = _recipe.Push_Adjust_UP_RSQ.MaximumLimit
            _results.Push_ADJUST_UP_Backlight_RSQ.Value = 0
            _results.Push_ADJUST_UP_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_ADJUST_UP_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_ADJUST_UP_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Adjust_UP_Polygon_Axy.MinimumLimit
            _results.Push_ADJUST_UP_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Adjust_UP_Polygon_Axy.MaximumLimit
            '
            _results.Push_ADJUST_UP_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Adjust_UP_Polygon_Bxy.MinimumLimit
            _results.Push_ADJUST_UP_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Adjust_UP_Polygon_Bxy.MaximumLimit
            '
            _results.Push_ADJUST_UP_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Adjust_UP_Polygon_Cxy.MinimumLimit
            _results.Push_ADJUST_UP_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Adjust_UP_Polygon_Cxy.MaximumLimit
            '
            _results.Push_ADJUST_UP_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Adjust_UP_Polygon_Dxy.MinimumLimit
            _results.Push_ADJUST_UP_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Adjust_UP_Polygon_Dxy.MaximumLimit
            '
            _results.Push_ADJUST_UP_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Adjust_UP_Polygon_Exy.MinimumLimit
            _results.Push_ADJUST_UP_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Adjust_UP_Polygon_Exy.MaximumLimit
            '
            _results.Push_ADJUST_UP_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_UP_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Adjust_UP_Polygon_Fxy.MinimumLimit
            _results.Push_ADJUST_UP_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Adjust_UP_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.Push_ADJUST_UP_backlight_homogeneity.MinimumLimit = _recipe.Push_Adjust_UP_minimum_homogeneity.Value
                _results.Push_ADJUST_UP_backlight_homogeneity.Value = 0
                _results.Push_ADJUST_UP_backlight_homogeneity.MaximumLimit = 100
                _results.Push_ADJUST_UP_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP.MaximumLimit = _recipe.Push_Adjust_UP_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP.Value = 0


            ' Intensity
            _results.Push_ADJUST_RIGHT_backlight_intensity.MinimumLimit = _recipe.Push_Adjust_Right_Intensity.MinimumLimit
            _results.Push_ADJUST_RIGHT_backlight_intensity.MaximumLimit = _recipe.Push_Adjust_Right_Intensity.MaximumLimit
            _results.Push_ADJUST_RIGHT_backlight_intensity.Value = 0
            _results.Push_ADJUST_RIGHT_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_ADJUST_RIGHT_Backlight_RSQ.MinimumLimit = _recipe.Push_Adjust_Right_RSQ.MinimumLimit
            _results.Push_ADJUST_RIGHT_Backlight_RSQ.MaximumLimit = _recipe.Push_Adjust_Right_RSQ.MaximumLimit
            _results.Push_ADJUST_RIGHT_Backlight_RSQ.Value = 0
            _results.Push_ADJUST_RIGHT_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_ADJUST_RIGHT_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Adjust_Right_Polygon_Axy.MinimumLimit
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Adjust_Right_Polygon_Axy.MaximumLimit
            '
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Adjust_Right_Polygon_Bxy.MinimumLimit
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Adjust_Right_Polygon_Bxy.MaximumLimit
            '
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Adjust_Right_Polygon_Cxy.MinimumLimit
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Adjust_Right_Polygon_Cxy.MaximumLimit
            '
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Adjust_Right_Polygon_Dxy.MinimumLimit
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Adjust_Right_Polygon_Dxy.MaximumLimit
            '
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Adjust_Right_Polygon_Exy.MinimumLimit
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Adjust_Right_Polygon_Exy.MaximumLimit
            '
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Adjust_Right_Polygon_Fxy.MinimumLimit
            _results.Push_ADJUST_RIGHT_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Adjust_Right_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.Push_ADJUST_RIGHT_backlight_homogeneity.MinimumLimit = _recipe.Push_Adjust_Right_minimum_homogeneity.Value
                _results.Push_ADJUST_RIGHT_backlight_homogeneity.Value = 0
                _results.Push_ADJUST_RIGHT_backlight_homogeneity.MaximumLimit = 100
                _results.Push_ADJUST_RIGHT_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT.MaximumLimit = _recipe.Push_Adjust_Right_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT.Value = 0

            ' Intensity
            _results.Push_ADJUST_DOWN_backlight_intensity.MinimumLimit = _recipe.Push_Adjust_DOWN_Intensity.MinimumLimit
            _results.Push_ADJUST_DOWN_backlight_intensity.MaximumLimit = _recipe.Push_Adjust_DOWN_Intensity.MaximumLimit
            _results.Push_ADJUST_DOWN_backlight_intensity.Value = 0
            _results.Push_ADJUST_DOWN_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_ADJUST_DOWN_Backlight_RSQ.MinimumLimit = _recipe.Push_Adjust_DOWN_RSQ.MinimumLimit
            _results.Push_ADJUST_DOWN_Backlight_RSQ.MaximumLimit = _recipe.Push_Adjust_DOWN_RSQ.MaximumLimit
            _results.Push_ADJUST_DOWN_Backlight_RSQ.Value = 0
            _results.Push_ADJUST_DOWN_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_ADJUST_DOWN_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Adjust_DOWN_Polygon_Axy.MinimumLimit
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Adjust_DOWN_Polygon_Axy.MaximumLimit
            '
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Adjust_DOWN_Polygon_Bxy.MinimumLimit
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Adjust_DOWN_Polygon_Bxy.MaximumLimit
            '
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Adjust_DOWN_Polygon_Cxy.MinimumLimit
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Adjust_DOWN_Polygon_Cxy.MaximumLimit
            '
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Adjust_DOWN_Polygon_Dxy.MinimumLimit
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Adjust_DOWN_Polygon_Dxy.MaximumLimit
            '
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Adjust_DOWN_Polygon_Exy.MinimumLimit
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Adjust_DOWN_Polygon_Exy.MaximumLimit
            '
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Adjust_DOWN_Polygon_Fxy.MinimumLimit
            _results.Push_ADJUST_DOWN_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Adjust_DOWN_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.Push_ADJUST_DOWN_backlight_homogeneity.MinimumLimit = _recipe.Push_Adjust_DOWN_minimum_homogeneity.Value
                _results.Push_ADJUST_DOWN_backlight_homogeneity.Value = 0
                _results.Push_ADJUST_DOWN_backlight_homogeneity.MaximumLimit = 100
                _results.Push_ADJUST_DOWN_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN.MaximumLimit = _recipe.Push_Adjust_Down_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN.Value = 0

        End If

        If CBool(_recipe.TestEnable_BACKLIGHT_Folding_Mirror.Value) Then
            _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            ' Intensity
            _results.Push_FOLDING_backlight_intensity.MinimumLimit = _recipe.Push_Folding_Intensity.MinimumLimit
            _results.Push_FOLDING_backlight_intensity.MaximumLimit = _recipe.Push_Folding_Intensity.MaximumLimit
            _results.Push_FOLDING_backlight_intensity.Value = 0
            _results.Push_FOLDING_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_FOLDING_Backlight_RSQ.MinimumLimit = _recipe.Push_Folding_RSQ.MinimumLimit
            _results.Push_FOLDING_Backlight_RSQ.MaximumLimit = _recipe.Push_Folding_RSQ.MaximumLimit
            _results.Push_FOLDING_Backlight_RSQ.Value = 0
            _results.Push_FOLDING_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_FOLDING_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_FOLDING_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Folding_Polygon_Axy.MinimumLimit
            _results.Push_FOLDING_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Folding_Polygon_Axy.MaximumLimit
            '
            _results.Push_FOLDING_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Folding_Polygon_Bxy.MinimumLimit
            _results.Push_FOLDING_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Folding_Polygon_Bxy.MaximumLimit
            '
            _results.Push_FOLDING_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Folding_Polygon_Cxy.MinimumLimit
            _results.Push_FOLDING_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Folding_Polygon_Cxy.MaximumLimit
            '
            _results.Push_FOLDING_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Folding_Polygon_Dxy.MinimumLimit
            _results.Push_FOLDING_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Folding_Polygon_Dxy.MaximumLimit
            '
            _results.Push_FOLDING_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Folding_Polygon_Exy.MinimumLimit
            _results.Push_FOLDING_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Folding_Polygon_Exy.MaximumLimit
            '
            _results.Push_FOLDING_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_FOLDING_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Folding_Polygon_Fxy.MinimumLimit
            _results.Push_FOLDING_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Folding_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.Push_FOLDING_backlight_homogeneity.MinimumLimit = _recipe.Push_Folding_minimum_homogeneity.Value
                _results.Push_FOLDING_backlight_homogeneity.Value = 0
                _results.Push_FOLDING_backlight_homogeneity.MaximumLimit = 100
                _results.Push_FOLDING_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR.MaximumLimit = _recipe.Push_Folding_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR.Value = 0

        End If

        If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Front.Value) Then
            _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            ' Intensity
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.MinimumLimit = _recipe.Windows_Lifter_Front_Left_Intensity.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.MaximumLimit = _recipe.Windows_Lifter_Front_Left_Intensity.MaximumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.Value = 0
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ.MinimumLimit = _recipe.Windows_Lifter_Front_Left_RSQ.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ.MaximumLimit = _recipe.Windows_Lifter_Front_Left_RSQ.MaximumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ.Value = 0
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy.MinimumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Axy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy.MaximumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Axy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Bxy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Bxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Cxy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Cxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Dxy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Dxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy.MinimumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Exy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy.MaximumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Exy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Fxy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Windows_Lifter_Front_Left_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity.MinimumLimit = _recipe.Windows_Lifter_Front_Left_minimum_homogeneity.Value
                _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity.Value = 0
                _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity.MaximumLimit = 100
                _results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT.MaximumLimit = _recipe.Windows_Lifter_Front_Left_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT.Value = 0

            ' Intensity
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.MinimumLimit = _recipe.Windows_Lifter_Front_Right_Intensity.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.MaximumLimit = _recipe.Windows_Lifter_Front_Right_Intensity.MaximumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.Value = 0
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ.MinimumLimit = _recipe.Windows_Lifter_Front_Right_RSQ.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ.MaximumLimit = _recipe.Windows_Lifter_Front_Right_RSQ.MaximumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ.Value = 0
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy.MinimumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Axy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy.MaximumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Axy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Bxy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Bxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Cxy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Cxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Dxy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Dxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy.MinimumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Exy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy.MaximumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Exy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Fxy.MinimumLimit
            _results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Windows_Lifter_Front_Right_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity.MinimumLimit = _recipe.Windows_Lifter_Front_Right_minimum_homogeneity.Value
                _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity.Value = 0
                _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity.MaximumLimit = 100
                _results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT.MaximumLimit = _recipe.Windows_Lifter_Front_Right_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT.Value = 0

        End If
        If CBool(_recipe.TestEnable_BACKLIGHT_Wili_Rear.Value) Then
            _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            ' Intensity
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_Intensity.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_Intensity.MaximumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.Value = 0
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_RSQ.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_RSQ.MaximumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ.Value = 0
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Axy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Axy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Bxy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Bxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Cxy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Cxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Dxy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Dxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Exy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Exy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Fxy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity.MinimumLimit = _recipe.Windows_Lifter_Rear_Left_minimum_homogeneity.Value
                _results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity.Value = 0
                _results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity.MaximumLimit = 100
                _results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT.MaximumLimit = _recipe.Windows_Lifter_Rear_Left_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT.Value = 0

            ' Intensity
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_Intensity.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_Intensity.MaximumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.Value = 0
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_RSQ.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_RSQ.MaximumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ.Value = 0
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Axy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Axy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Bxy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Bxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Cxy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Cxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Dxy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Dxy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Exy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Exy.MaximumLimit
            '
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Fxy.MinimumLimit
            _results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_Polygon_Fxy.MaximumLimit
            ' Homogeneity
            If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
                _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity.MinimumLimit = _recipe.Windows_Lifter_Rear_Right_minimum_homogeneity.Value
                _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity.Value = 0
                _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity.MaximumLimit = 100
                _results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            End If
            '
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT.MaximumLimit = _recipe.Windows_Lifter_Rear_Right_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT.Value = 0

        End If

        If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock.Value) Then
            _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            ' Intensity
            _results.Push_CHILDREN_LOCK_backlight_intensity.MinimumLimit = _recipe.Push_Children_Lock_Intensity.MinimumLimit
            _results.Push_CHILDREN_LOCK_backlight_intensity.MaximumLimit = _recipe.Push_Children_Lock_Intensity.MaximumLimit
            _results.Push_CHILDREN_LOCK_backlight_intensity.Value = 0
            _results.Push_CHILDREN_LOCK_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_CHILDREN_LOCK_Backlight_RSQ.MinimumLimit = _recipe.Push_Children_Lock_RSQ.MinimumLimit
            _results.Push_CHILDREN_LOCK_Backlight_RSQ.MaximumLimit = _recipe.Push_Children_Lock_RSQ.MaximumLimit
            _results.Push_CHILDREN_LOCK_Backlight_RSQ.Value = 0
            _results.Push_CHILDREN_LOCK_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_CHILDREN_LOCK_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Children_Lock_Polygon_Axy.MinimumLimit
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Children_Lock_Polygon_Axy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Children_Lock_Polygon_Bxy.MinimumLimit
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Children_Lock_Polygon_Bxy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Children_Lock_Polygon_Cxy.MinimumLimit
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Children_Lock_Polygon_Cxy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Children_Lock_Polygon_Dxy.MinimumLimit
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Children_Lock_Polygon_Dxy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Children_Lock_Polygon_Exy.MinimumLimit
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Children_Lock_Polygon_Exy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Children_Lock_Polygon_Fxy.MinimumLimit
            _results.Push_CHILDREN_LOCK_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Children_Lock_Polygon_Fxy.MaximumLimit
            '' Homogeneity
            'If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
            '    _results.Push_CHILDREN_LOCK_backlight_homogeneity.MinimumLimit = _recipe.Push_Children_Lock_minimum_homogeneity.Value
            '    _results.Push_CHILDREN_LOCK_backlight_homogeneity.Value = 0
            '    _results.Push_CHILDREN_LOCK_backlight_homogeneity.MaximumLimit = 100
            '    _results.Push_CHILDREN_LOCK_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            'End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK.MaximumLimit = _recipe.Push_Children_Lock_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK.Value = 0

        End If

        If CBool(_recipe.TestEnable_BACKLIGHT_ChildrenLock2.Value) Then
            _results.eBACKLIGHT_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown
            ' Intensity
            _results.Push_CHILDREN_LOCK2_backlight_intensity.MinimumLimit = _recipe.Push_Children_Lock2_Intensity.MinimumLimit
            _results.Push_CHILDREN_LOCK2_backlight_intensity.MaximumLimit = _recipe.Push_Children_Lock2_Intensity.MaximumLimit
            _results.Push_CHILDREN_LOCK2_backlight_intensity.Value = 0
            _results.Push_CHILDREN_LOCK2_backlight_intensity.TestResult = cResultValue.eValueTestResult.Unknown
            ' RSQ
            _results.Push_CHILDREN_LOCK2_Backlight_RSQ.MinimumLimit = _recipe.Push_Children_Lock2_RSQ.MinimumLimit
            _results.Push_CHILDREN_LOCK2_Backlight_RSQ.MaximumLimit = _recipe.Push_Children_Lock2_RSQ.MaximumLimit
            _results.Push_CHILDREN_LOCK2_Backlight_RSQ.Value = 0
            _results.Push_CHILDREN_LOCK2_Backlight_RSQ.TestResult = cResultValue.eValueTestResult.Unknown
            ' not Checked
            _results.Push_CHILDREN_LOCK2_backlight_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_backlight_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_backlight_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_backlight_intensity_Camera.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_backlight_Saturation.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_backlight_DominantWavelenght.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_Backlight_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_Backlight_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Axy.MinimumLimit = _recipe.Push_Children_Lock2_Polygon_Axy.MinimumLimit
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Axy.MaximumLimit = _recipe.Push_Children_Lock2_Polygon_Axy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Bxy.MinimumLimit = _recipe.Push_Children_Lock2_Polygon_Bxy.MinimumLimit
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Bxy.MaximumLimit = _recipe.Push_Children_Lock2_Polygon_Bxy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Cxy.MinimumLimit = _recipe.Push_Children_Lock2_Polygon_Cxy.MinimumLimit
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Cxy.MaximumLimit = _recipe.Push_Children_Lock2_Polygon_Cxy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Dxy.MinimumLimit = _recipe.Push_Children_Lock2_Polygon_Dxy.MinimumLimit
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Dxy.MaximumLimit = _recipe.Push_Children_Lock2_Polygon_Dxy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Exy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Exy.MinimumLimit = _recipe.Push_Children_Lock2_Polygon_Exy.MinimumLimit
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Exy.MaximumLimit = _recipe.Push_Children_Lock2_Polygon_Exy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Fxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Fxy.MinimumLimit = _recipe.Push_Children_Lock2_Polygon_Fxy.MinimumLimit
            _results.Push_CHILDREN_LOCK2_Backlight_Polygon_Fxy.MaximumLimit = _recipe.Push_Children_Lock2_Polygon_Fxy.MaximumLimit
            '' Homogeneity
            'If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
            '    _results.Push_CHILDREN_LOCK2_backlight_homogeneity.MinimumLimit = _recipe.Push_Children_Lock2_minimum_homogeneity.Value
            '    _results.Push_CHILDREN_LOCK2_backlight_homogeneity.Value = 0
            '    _results.Push_CHILDREN_LOCK2_backlight_homogeneity.MaximumLimit = 100
            '    _results.Push_CHILDREN_LOCK2_backlight_homogeneity.TestResult = cResultValue.eValueTestResult.Unknown
            'End If
            '
            _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2.MinimumLimit = 0
            _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2.MaximumLimit = _recipe.Push_Children_Lock2_backlight_Defect_Area.Value
            _results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2.Value = 0

        End If

        ' Camera Homogeneity
        If CBool(_recipe.TestEnable_BACKLIGHT_Homogeneity.Value) Then
            _results.eBACKLIGHT_HOMOGENEITY.TestResult = cResultValue.eValueTestResult.Unknown
        End If

        ' Camera TellTale
        If CBool(_recipe.TestEnable_TELLTALE_SelectMirror.Value) Then
            _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Push_SELECT_LEFT_LED_intensity.MinimumLimit = _recipe.TELLTALE_Select_Left_Intensity.MinimumLimit
            _results.Push_SELECT_LEFT_LED_intensity.Value = 0
            _results.Push_SELECT_LEFT_LED_intensity.MaximumLimit = _recipe.TELLTALE_Select_Left_Intensity.MaximumLimit
            _results.Push_SELECT_LEFT_LED_intensity.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Push_SELECT_LEFT_LED_WaveLenght.MinimumLimit = _recipe.TELLTALE_Select_Left_WaveLenght.MinimumLimit
            _results.Push_SELECT_LEFT_LED_WaveLenght.Value = 0
            _results.Push_SELECT_LEFT_LED_WaveLenght.MaximumLimit = _recipe.TELLTALE_Select_Left_WaveLenght.MaximumLimit
            _results.Push_SELECT_LEFT_LED_WaveLenght.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Push_SELECT_LEFT_LED_saturation.MinimumLimit = _recipe.TELLTALE_Select_Left_Saturation.MinimumLimit
            _results.Push_SELECT_LEFT_LED_saturation.Value = 0
            _results.Push_SELECT_LEFT_LED_saturation.MaximumLimit = _recipe.TELLTALE_Select_Left_Saturation.MaximumLimit
            _results.Push_SELECT_LEFT_LED_saturation.TestResult = cResultValue.eValueTestResult.Unknown

            ' not Checked
            _results.Push_SELECT_LEFT_LED_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_LED_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_LED_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_LED_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_LED_y.TestResult = cResultValue.eValueTestResult.NotTested

            '
            _results.Push_SELECT_LEFT_LED_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_LED_Polygon_Axy.MinimumLimit = _recipe.TELLTALE_Select_Left_Polygon_Axy.MinimumLimit
            _results.Push_SELECT_LEFT_LED_Polygon_Axy.MaximumLimit = _recipe.TELLTALE_Select_Left_Polygon_Axy.MaximumLimit
            '
            _results.Push_SELECT_LEFT_LED_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_LED_Polygon_Bxy.MinimumLimit = _recipe.TELLTALE_Select_Left_Polygon_Bxy.MinimumLimit
            _results.Push_SELECT_LEFT_LED_Polygon_Bxy.MaximumLimit = _recipe.TELLTALE_Select_Left_Polygon_Bxy.MaximumLimit
            '
            _results.Push_SELECT_LEFT_LED_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_LED_Polygon_Cxy.MinimumLimit = _recipe.TELLTALE_Select_Left_Polygon_Cxy.MinimumLimit
            _results.Push_SELECT_LEFT_LED_Polygon_Cxy.MaximumLimit = _recipe.TELLTALE_Select_Left_Polygon_Cxy.MaximumLimit
            '
            _results.Push_SELECT_LEFT_LED_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_LEFT_LED_Polygon_Dxy.MinimumLimit = _recipe.TELLTALE_Select_Left_Polygon_Dxy.MinimumLimit
            _results.Push_SELECT_LEFT_LED_Polygon_Dxy.MaximumLimit = _recipe.TELLTALE_Select_Left_Polygon_Dxy.MaximumLimit

            '
            _results.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR.MinimumLimit = 0
            _results.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR.MaximumLimit = _recipe.TELLTALE_Select_Left_Defect_Area.Value
            _results.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR.Value = 0

            _results.Push_SELECT_RIGHT_LED_intensity.MinimumLimit = _recipe.TELLTALE_Select_Right_Intensity.MinimumLimit
            _results.Push_SELECT_RIGHT_LED_intensity.Value = 0
            _results.Push_SELECT_RIGHT_LED_intensity.MaximumLimit = _recipe.TELLTALE_Select_Right_Intensity.MaximumLimit
            _results.Push_SELECT_RIGHT_LED_intensity.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Push_SELECT_RIGHT_LED_WaveLenght.MinimumLimit = _recipe.TELLTALE_Select_Right_WaveLenght.MinimumLimit
            _results.Push_SELECT_RIGHT_LED_WaveLenght.Value = 0
            _results.Push_SELECT_RIGHT_LED_WaveLenght.MaximumLimit = _recipe.TELLTALE_Select_Right_WaveLenght.MaximumLimit
            _results.Push_SELECT_RIGHT_LED_WaveLenght.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Push_SELECT_RIGHT_LED_saturation.MinimumLimit = _recipe.TELLTALE_Select_Right_Saturation.MinimumLimit
            _results.Push_SELECT_RIGHT_LED_saturation.Value = 0
            _results.Push_SELECT_RIGHT_LED_saturation.MaximumLimit = _recipe.TELLTALE_Select_Right_Saturation.MaximumLimit
            _results.Push_SELECT_RIGHT_LED_saturation.TestResult = cResultValue.eValueTestResult.Unknown

            ' not Checked
            _results.Push_SELECT_RIGHT_LED_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_LED_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_LED_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_LED_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_LED_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_SELECT_RIGHT_LED_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_LED_Polygon_Axy.MinimumLimit = _recipe.TELLTALE_Select_Right_Polygon_Axy.MinimumLimit
            _results.Push_SELECT_RIGHT_LED_Polygon_Axy.MaximumLimit = _recipe.TELLTALE_Select_Right_Polygon_Axy.MaximumLimit
            '
            _results.Push_SELECT_RIGHT_LED_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_LED_Polygon_Bxy.MinimumLimit = _recipe.TELLTALE_Select_Right_Polygon_Bxy.MinimumLimit
            _results.Push_SELECT_RIGHT_LED_Polygon_Bxy.MaximumLimit = _recipe.TELLTALE_Select_Right_Polygon_Bxy.MaximumLimit
            '
            _results.Push_SELECT_RIGHT_LED_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_LED_Polygon_Cxy.MinimumLimit = _recipe.TELLTALE_Select_Right_Polygon_Cxy.MinimumLimit
            _results.Push_SELECT_RIGHT_LED_Polygon_Cxy.MaximumLimit = _recipe.TELLTALE_Select_Right_Polygon_Cxy.MaximumLimit
            '
            _results.Push_SELECT_RIGHT_LED_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_SELECT_RIGHT_LED_Polygon_Dxy.MinimumLimit = _recipe.TELLTALE_Select_Right_Polygon_Dxy.MinimumLimit
            _results.Push_SELECT_RIGHT_LED_Polygon_Dxy.MaximumLimit = _recipe.TELLTALE_Select_Right_Polygon_Dxy.MaximumLimit
            '
            _results.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR.MinimumLimit = 0
            _results.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR.MaximumLimit = _recipe.TELLTALE_Select_Right_Defect_Area.Value
            _results.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR.Value = 0

        End If

        If CBool(_recipe.TestEnable_TELLTALE_ChildrenLock.Value) Then
            _results.eTELLTALE_CONFORMITY.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Push_CHILDREN_LOCK_LED_intensity.MinimumLimit = _recipe.TELLTALE_Children_Lock_Intensity.MinimumLimit
            _results.Push_CHILDREN_LOCK_LED_intensity.Value = 0
            _results.Push_CHILDREN_LOCK_LED_intensity.MaximumLimit = _recipe.TELLTALE_Children_Lock_Intensity.MaximumLimit
            _results.Push_CHILDREN_LOCK_LED_intensity.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Push_CHILDREN_LOCK_LED_WaveLenght.MinimumLimit = _recipe.TELLTALE_Children_Lock_WaveLenght.MinimumLimit
            _results.Push_CHILDREN_LOCK_LED_WaveLenght.Value = 0
            _results.Push_CHILDREN_LOCK_LED_WaveLenght.MaximumLimit = _recipe.TELLTALE_Children_Lock_WaveLenght.MaximumLimit
            _results.Push_CHILDREN_LOCK_LED_WaveLenght.TestResult = cResultValue.eValueTestResult.Unknown

            _results.Push_CHILDREN_LOCK_LED_saturation.MinimumLimit = _recipe.TELLTALE_Children_Lock_Saturation.MinimumLimit
            _results.Push_CHILDREN_LOCK_LED_saturation.Value = 0
            _results.Push_CHILDREN_LOCK_LED_saturation.MaximumLimit = _recipe.TELLTALE_Children_Lock_Saturation.MaximumLimit
            _results.Push_CHILDREN_LOCK_LED_saturation.TestResult = cResultValue.eValueTestResult.Unknown

            ' not Checked
            _results.Push_CHILDREN_LOCK_LED_red.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_LED_green.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_LED_blue.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_LED_x.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_LED_y.TestResult = cResultValue.eValueTestResult.NotTested
            '
            _results.Push_CHILDREN_LOCK_LED_Polygon_Axy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_LED_Polygon_Axy.MinimumLimit = _recipe.TELLTALE_Children_Lock_Polygon_Axy.MinimumLimit
            _results.Push_CHILDREN_LOCK_LED_Polygon_Axy.MaximumLimit = _recipe.TELLTALE_Children_Lock_Polygon_Axy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK_LED_Polygon_Bxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_LED_Polygon_Bxy.MinimumLimit = _recipe.TELLTALE_Children_Lock_Polygon_Bxy.MinimumLimit
            _results.Push_CHILDREN_LOCK_LED_Polygon_Bxy.MaximumLimit = _recipe.TELLTALE_Children_Lock_Polygon_Bxy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK_LED_Polygon_Cxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_LED_Polygon_Cxy.MinimumLimit = _recipe.TELLTALE_Children_Lock_Polygon_Cxy.MinimumLimit
            _results.Push_CHILDREN_LOCK_LED_Polygon_Cxy.MaximumLimit = _recipe.TELLTALE_Children_Lock_Polygon_Cxy.MaximumLimit
            '
            _results.Push_CHILDREN_LOCK_LED_Polygon_Dxy.TestResult = cResultValue.eValueTestResult.NotTested
            _results.Push_CHILDREN_LOCK_LED_Polygon_Dxy.MinimumLimit = _recipe.TELLTALE_Children_Lock_Polygon_Dxy.MinimumLimit
            _results.Push_CHILDREN_LOCK_LED_Polygon_Dxy.MaximumLimit = _recipe.TELLTALE_Children_Lock_Polygon_Dxy.MaximumLimit

            '
            _results.DEFECT_AREA_TELLTALE_CHILDREN_LOCK.TestResult = cResultValue.eValueTestResult.Unknown
            _results.DEFECT_AREA_TELLTALE_CHILDREN_LOCK.MinimumLimit = 0
            _results.DEFECT_AREA_TELLTALE_CHILDREN_LOCK.MaximumLimit = _recipe.TELLTALE_Children_Lock_Defect_Area.Value
            _results.DEFECT_AREA_TELLTALE_CHILDREN_LOCK.Value = 0

        End If

        _results.eWrite_TraceabilityMMS.TestResult = cResultValue.eValueTestResult.Unknown
        _results.eRead_TraceabilityMMS.TestResult = cResultValue.eValueTestResult.Unknown

        _results.MMS_Read_Final_Product_Reference.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Final_Product_Reference.MinimumLimit = _recipe.Write_MMS_Final_Product_Reference.Value
        _results.MMS_Read_Final_Product_Reference.MaximumLimit = _recipe.Write_MMS_Final_Product_Reference.Value
        _results.MMS_Read_Final_Product_Reference.Value = 0

        _results.MMS_Read_Final_Product_Index.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Final_Product_Index.MinimumLimit = _recipe.Write_MMS_Final_Product_Index.Value
        _results.MMS_Read_Final_Product_Index.MaximumLimit = _recipe.Write_MMS_Final_Product_Index.Value
        _results.MMS_Read_Final_Product_Index.Value = 0

        _results.MMS_Read_Valeo_Final_Product_Plant.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Valeo_Final_Product_Plant.MinimumLimit = Hex(Asc(Mid(mSettings.Valeo_Plant, 1, 1))) & _
                                                        Hex(Asc(Mid(mSettings.Valeo_Plant, 2, 1))) & _
                                                        Hex(Asc(Mid(mSettings.Valeo_Plant, 3, 1))) & _
                                                        Hex(Asc(Mid(mSettings.Valeo_Plant, 4, 1)))
        _results.MMS_Read_Valeo_Final_Product_Plant.MaximumLimit = Hex(Asc(Mid(mSettings.Valeo_Plant, 1, 1))) & _
                                                        Hex(Asc(Mid(mSettings.Valeo_Plant, 2, 1))) & _
                                                        Hex(Asc(Mid(mSettings.Valeo_Plant, 3, 1))) & _
                                                        Hex(Asc(Mid(mSettings.Valeo_Plant, 4, 1)))
        _results.MMS_Read_Valeo_Final_Product_Plant.Value = 0

        _results.MMS_Read_Valeo_Final_Product_Line.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Valeo_Final_Product_Line.MinimumLimit = Right("00" & mSettings.LineNumber, 2) ' _recipe.Write_MMS_Valeo_Final_Product_Line.Value
        _results.MMS_Read_Valeo_Final_Product_Line.MaximumLimit = Right("00" & mSettings.LineNumber, 2) '_recipe.Write_MMS_Valeo_Final_Product_Line.Value

        _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MinimumLimit = 0
        _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.MaximumLimit = 0
        _results.MMS_Read_Valeo_Final_Product_Manufacturing_Date.Value = 0

        _results.MMS_Read_Valeo_Serial_Number.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Valeo_Serial_Number.MinimumLimit = 0
        _results.MMS_Read_Valeo_Serial_Number.MaximumLimit = 0
        _results.MMS_Read_Valeo_Serial_Number.Value = 0

        _results.MMS_Read_Deviation_Number.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Deviation_Number.MinimumLimit = _recipe.Write_MMS_Deviation_Number.Value
        _results.MMS_Read_Deviation_Number.MaximumLimit = _recipe.Write_MMS_Deviation_Number.Value
        _results.MMS_Read_Deviation_Number.Value = 0


        _results.eWrite_TestByteMMS.TestResult = cResultValue.eValueTestResult.Unknown
        _results.eRead_TestByteMMS.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Test_Byte.TestResult = cResultValue.eValueTestResult.Unknown
        _results.MMS_Read_Test_Byte.MinimumLimit = "02"
        _results.MMS_Read_Test_Byte.MaximumLimit = "02"
        _results.MMS_Read_Test_Byte.Value = 0

        _results.eResetEcu_AfterWriting.TestResult = cResultValue.eValueTestResult.Unknown



    End Sub

    Public Function GeneratePartNumber(ByVal testDate As String) As String
        Dim fileReader As StreamReader = Nothing
        Dim fileWriter As StreamWriter = Nothing
        Dim lastDate As String
        Dim partNumber As Integer

        ' Load the last part information
        fileReader = New StreamReader(_settings.LastPartNumberPath)
        ' Read the last year
        lastDate = fileReader.ReadLine
        ' Read the last part number
        partNumber = CInt(fileReader.ReadLine)
        ' Close the file
        fileReader.Close()

        'in application the sn is not reset all day
        ' Increase or initialize the part number
        If (testDate = lastDate) Then
            partNumber = partNumber + 1
        Else
            partNumber = 1
        End If

        ' Save the new part number information
        fileWriter = New StreamWriter(_settings.LastPartNumberPath)
        fileWriter.WriteLine(testDate)
        fileWriter.WriteLine(partNumber.ToString("0000000000"))
        fileWriter.Close()

        ' Return the part number
        GeneratePartNumber = partNumber.ToString("0000000000")
    End Function

    Public Function WS02AI_DutyCycle_Freq(ByRef dblDutyCycle As Double,
                                    ByRef dblFreq As Double,
                                    ByRef dblValue() As Double,
                                    ByRef dblLowVoltage As Double,
                                    ByRef dblHighVoltage As Double,
                                    ByRef dblRiseTime As Double,
                                    ByRef dblFallTime As Double,
                                    ByVal Frequency As Long) As Integer
        Dim e As Boolean
        Dim i As Integer
        Dim j As Integer
        Dim n As Long
        Dim mintFreqStart As Integer
        Dim mintFreqStop As Integer
        Dim mintDutyStart As Integer
        Dim mintDutyStop As Integer

        Dim mintHighVoltage As Integer
        Dim mintLowVoltage As Integer
        Dim mintRiseTime As Integer
        Dim mintFallTime As Integer
        Dim mbooStartAnalyse As Boolean
        Dim v(0 To mWS02AIOManager.eAnalogInput.Count * 1000 - 1) As Double

        Dim Rise_Frequency As Double = 7 ' 10.8 ' 80% de Vbat
        Dim Rise_DutyCycle As Double = 7 '2.7 ' 20% de Vbat


        dblDutyCycle = -1
        dblFreq = -1
        dblLowVoltage = -1
        dblHighVoltage = -1
        dblRiseTime = -1
        dblFallTime = -1

        mintFreqStart = -1
        mintFreqStop = -1
        mintDutyStart = -1
        mintDutyStop = -1
        mintHighVoltage = 0
        mintLowVoltage = 13.5
        mintRiseTime = -1
        mintFallTime = -1

        mbooStartAnalyse = False
        For j = 0 To CInt(chUCDA_Sample(i)) - 1
            If (dblValue(j) < 0) Then
                dblValue(j) = 0
            End If
            If mbooStartAnalyse = True Then
                'Frequency
                If dblValue(j) >= Rise_Frequency And mintFreqStart = -1 Then
                    AddLogEntry("Debut F:" & j)
                    mintFreqStart = j
                ElseIf dblValue(j) >= Rise_Frequency And mintDutyStop <> -1 And mintFreqStop = -1 Then
                    AddLogEntry("Fin F:" & j)
                    mintFreqStop = j
                    dblFreq = (1 / Frequency) * (mintFreqStop - mintFreqStart)
                    If dblFreq > 0 Then
                        dblFreq = 1 / dblFreq
                    Else
                        dblFreq = 0
                    End If
                    If (mintFreqStop - mintFreqStart) > 0 Then
                        dblDutyCycle = 100 * ((mintDutyStop - mintDutyStart) / (mintFreqStop - mintFreqStart))
                    Else
                        dblDutyCycle = 0
                    End If
                End If
                'DutyCycle
                If dblValue(j) >= Rise_DutyCycle And mintDutyStart = -1 Then
                    AddLogEntry("Debut DC:" & j)
                    mintDutyStart = j
                ElseIf dblValue(j) < Rise_DutyCycle And mintDutyStart <> -1 And mintDutyStop = -1 Then
                    AddLogEntry("Fin DC:" & j)
                    mintDutyStop = j
                End If
                ' Low Voltage
                If dblValue(j) < mintLowVoltage Then
                    mintLowVoltage = dblValue(j)
                    dblLowVoltage = dblValue(j)
                End If
                'High Voltage
                If dblValue(j) > mintHighVoltage Then
                    mintHighVoltage = dblValue(j)
                    dblHighVoltage = dblValue(j)
                End If
                ' Rise Time
                If dblValue(j) >= Rise_DutyCycle And dblRiseTime = -1 And mintRiseTime = -1 Then
                    dblRiseTime = 0
                ElseIf dblValue(j) <= Rise_Frequency And dblRiseTime <> -1 And mintRiseTime = -1 Then
                    dblRiseTime = dblRiseTime + 1
                ElseIf dblValue(j) > Rise_Frequency And dblRiseTime <> -1 And mintRiseTime = -1 Then
                    dblRiseTime = dblRiseTime * (1 / Frequency)
                    mintRiseTime = dblRiseTime
                End If
                ' Fall Time
                If dblValue(j) <= Rise_Frequency And dblFallTime = -1 And mintFallTime = -1 And mintRiseTime <> -1 Then
                    dblFallTime = 0
                ElseIf dblValue(j) >= Rise_DutyCycle And dblFallTime <> -1 And mintFallTime = -1 And mintRiseTime <> -1 Then
                    dblFallTime = dblFallTime + 1
                ElseIf dblValue(j) < Rise_DutyCycle And dblFallTime <> -1 And mintFallTime = -1 And mintRiseTime <> -1 Then
                    dblFallTime = dblFallTime * (1 / Frequency)
                    mintRiseTime = dblFallTime
                End If

            ElseIf mbooStartAnalyse = False And dblValue(j) < Rise_Frequency Then
                '
                mbooStartAnalyse = True
            End If

            If dblDutyCycle <> -1 And
                dblFreq <> -1 And
                dblLowVoltage <> -1 And
                dblHighVoltage <> -1 And
                dblRiseTime <> -1 And
                dblFallTime <> -1 Then
                Exit For
            End If
        Next j

        ' Returns 0 if no errors happened, -1 otherwise
        WS02AI_DutyCycle_Freq = 0
    End Function

    Public Sub CalcFreq(ByRef dblSample() As Double,
                   ByVal lngSampleCount As Long,
                   ByVal lngSampleFrequency As Long,
                   ByRef lngFrequency As Long,
                   ByRef lngDutyCycle As Long)
        Dim l As Long

        ' Calculates the frequency and the Sound Level Pressure of the samples
        lngFrequency = 0
        lngDutyCycle = 0
        For l = 0 To lngSampleCount - 2
            If (l < lngSampleCount - 1 And dblSample(l) < 0 And dblSample(l + 1) > 0) Then
                lngFrequency = lngFrequency + 1
            End If

        Next l
        lngFrequency = lngFrequency * (lngSampleFrequency / lngSampleCount)

    End Sub


End Module