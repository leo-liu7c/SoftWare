Option Explicit On
Option Strict On

Module mDIOManager
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Digital input enumeration
    Public Enum eDigitalInput
        WS02_TestEnable = 0
        WS02_StepInProgress = 1
        WS02_LoadRecipeRemote = 2
        WS02_LoadRecipeLocal = 3
        WS02_Reserve_0 = 4
        WS02_Reserve_1 = 5
        WS02_Reserve_2 = 6
        WS02_Reserve_3 = 7

        WS03_TestEnable = 8
        WS03_StepInProgress = 9
        WS03_LoadRecipeRemote = 10
        WS03_LoadRecipeLocal = 11
        WS03_Reserve_0 = 12
        WS03_Reserve_1 = 13
        WS03_Reserve_2 = 14
        WS03_Reserve_3 = 15

        WS04_TestEnable = 16
        WS04_StepInProgress = 17
        WS04_LoadRecipeRemote = 18
        WS04_LoadRecipeLocal = 19
        WS04_Reserve_0 = 20
        WS04_Reserve_1 = 21
        WS04_Reserve_2 = 22
        WS04_Reserve_3 = 23

        WS05_TestEnable = 24
        WS05_StepInProgress = 25
        WS05_LoadRecipeRemote = 26
        WS05_LoadRecipeLocal = 27
        WS05_Reserve_0 = 28
        WS05_Reserve_1 = 29
        WS05_Reserve_2 = 30
        WS05_Reserve_3 = 31

        WS02_Keyence_STO = 32
        WS02_Keyence_OR = 33
        WS02_Keyence_Err = 34
        WS02_Keyence_RUN = 35
        WS02_Keyence_Busy = 36
        WS02_Keyence_TrigReady = 37
        WS02_Keyence_0 = 38
        WS02_Keyence_1 = 39


        Count = 40
    End Enum

    ' Digital output enumeration
    Public Enum eDigitalOutput
        WS02_StartStep = 0
        WS02_TestOk = 1
        WS02_RelaodRecipe = 2
        WS02_RecipeLoaded = 3
        WS02_Alarm = 4
        WS02_Reserve_0 = 5
        WS02_Reserve_1 = 6
        WS02_Reserve_2 = 7
        '
        WS03_StartStep = 8
        WS03_TestOk = 9
        WS03_RelaodRecipe = 10
        WS03_RecipeLoaded = 11
        WS03_Alarm = 12
        WS03_Reserve_0 = 13
        WS03_Reserve_1 = 14
        WS03_Reserve_2 = 15
        '
        WS04_StartStep = 16
        WS04_TestOk = 17
        WS04_RelaodRecipe = 18
        WS04_RecipeLoaded = 19
        WS04_Alarm = 20
        WS04_Reserve_0 = 21
        WS04_Reserve_1 = 22
        WS04_Reserve_2 = 23
        '
        WS05_StartStep = 24
        WS05_TestOk = 25
        WS05_RelaodRecipe = 26
        WS05_RecipeLoaded = 27
        WS05_Alarm = 28
        WS05_Reserve_0 = 29
        WS05_Reserve_1 = 30
        WS05_Reserve_2 = 31
        '
        WS02_Keyence_PLC = 32
        WS02_Keyence_Trig1 = 33
        WS02_Keyence_Trig2 = 34
        WS02_Keyence_Reset = 35
        WS02_Keyence_Test = 36

        'HBM
        WS03_ResetForce = 37
        WS04_ResetForce = 38
        WS05_ResetForce = 39

        Count = 40
    End Enum




    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _hardwareEnabled = mConstants.HardwareEnabled_NI

    ' Private variables
    Private _digitalInputStatus(0 To eDigitalInput.Count - 1) As Boolean
    Private _digitalOutputStatus(0 To eDigitalOutput.Count - 1) As Boolean
    Private _VIPA As New cVIPA



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property DigitalInputDescription(ByVal index As eDigitalInput) As String
        Get
            Select Case index

                Case eDigitalInput.WS02_TestEnable
                    DigitalInputDescription = "WS02-test enable"
                Case eDigitalInput.WS02_StepInProgress
                    DigitalInputDescription = "WS02-step in progress"
                Case eDigitalInput.WS02_LoadRecipeLocal
                    DigitalInputDescription = "WS02-Load Recipe Local Mode"
                Case eDigitalInput.WS02_LoadRecipeRemote
                    DigitalInputDescription = "WS02-Load Recipe Remote Mode"

                Case eDigitalInput.WS03_TestEnable
                    DigitalInputDescription = "WS03-test enable"
                Case eDigitalInput.WS03_StepInProgress
                    DigitalInputDescription = "WS03-step in progress"
                Case eDigitalInput.WS03_LoadRecipeLocal
                    DigitalInputDescription = "WS03-Load Recipe Local Mode"
                Case eDigitalInput.WS03_LoadRecipeRemote
                    DigitalInputDescription = "WS03-Load Recipe Remote Mode"

                Case eDigitalInput.WS04_TestEnable
                    DigitalInputDescription = "WS04-test enable"
                Case eDigitalInput.WS04_StepInProgress
                    DigitalInputDescription = "WS04-step in progress"
                Case eDigitalInput.WS04_LoadRecipeLocal
                    DigitalInputDescription = "WS04-Load Recipe Local Mode"
                Case eDigitalInput.WS04_LoadRecipeRemote
                    DigitalInputDescription = "WS04-Load Recipe Remote Mode"

                Case eDigitalInput.WS05_TestEnable
                    DigitalInputDescription = "WS05-test enable"
                Case eDigitalInput.WS05_StepInProgress
                    DigitalInputDescription = "WS05-step in progress"
                Case eDigitalInput.WS05_LoadRecipeLocal
                    DigitalInputDescription = "WS05-Load Recipe Local Mode"
                Case eDigitalInput.WS05_LoadRecipeRemote
                    DigitalInputDescription = "WS05-Load Recipe Remote Mode"

                Case eDigitalInput.WS02_Keyence_STO
                    DigitalInputDescription = "Keyence STO"
                Case eDigitalInput.WS02_Keyence_OR
                    DigitalInputDescription = "Keyence OR"
                Case eDigitalInput.WS02_Keyence_Err
                    DigitalInputDescription = "Keyence Err"
                Case eDigitalInput.WS02_Keyence_RUN
                    DigitalInputDescription = "Keyence RUN"
                Case eDigitalInput.WS02_Keyence_Busy
                    DigitalInputDescription = "Keyence Busy"
                Case eDigitalInput.WS02_Keyence_TrigReady
                    DigitalInputDescription = "Keyence Trig Ready"
                Case eDigitalInput.WS02_Keyence_0
                    DigitalInputDescription = "Keyence R0"
                Case eDigitalInput.WS02_Keyence_1
                    DigitalInputDescription = "Keyence R1"

                Case Else
                    DigitalInputDescription = String.Format("Free {0}", CInt(index))
            End Select
        End Get
    End Property



    Public ReadOnly Property DigitalInputStatus(ByVal index As eDigitalInput) As Boolean
        Get
            DigitalInputStatus = _digitalInputStatus(index)
        End Get
    End Property



    Public ReadOnly Property DigitalOutputDescription(ByVal index As eDigitalOutput) As String
        Get
            Select Case index

                Case eDigitalOutput.WS02_TestOk
                    DigitalOutputDescription = "WS02-test ok"
                Case eDigitalOutput.WS02_StartStep
                    DigitalOutputDescription = "WS02-start step"
                Case eDigitalOutput.WS02_RecipeLoaded
                    DigitalOutputDescription = "WS02-Recipe Loaded"
                Case eDigitalOutput.WS02_RelaodRecipe
                    DigitalOutputDescription = "WS02-Reload Recipe"
                Case eDigitalOutput.WS02_Alarm
                    DigitalOutputDescription = "WS02-Alarm"

                Case eDigitalOutput.WS03_TestOk
                    DigitalOutputDescription = "WS03-test ok"
                Case eDigitalOutput.WS03_StartStep
                    DigitalOutputDescription = "WS03-start step"
                Case eDigitalOutput.WS03_RecipeLoaded
                    DigitalOutputDescription = "WS03-Recipe Loaded"
                Case eDigitalOutput.WS03_RelaodRecipe
                    DigitalOutputDescription = "WS03-Reload Recipe"
                Case eDigitalOutput.WS03_Alarm
                    DigitalOutputDescription = "WS03-Alarm"
                Case eDigitalOutput.WS03_ResetForce
                    DigitalOutputDescription = "WS03-Reset Force Sensor"

                Case eDigitalOutput.WS04_TestOk
                    DigitalOutputDescription = "WS04-test ok"
                Case eDigitalOutput.WS04_StartStep
                    DigitalOutputDescription = "WS04-start step"
                Case eDigitalOutput.WS04_RecipeLoaded
                    DigitalOutputDescription = "WS04-Recipe Loaded"
                Case eDigitalOutput.WS04_RelaodRecipe
                    DigitalOutputDescription = "WS04-Reload Recipe"
                Case eDigitalOutput.WS04_Alarm
                    DigitalOutputDescription = "WS04-Alarm"
                Case eDigitalOutput.WS04_ResetForce
                    DigitalOutputDescription = "WS04-Reset Force Sensor"

                Case eDigitalOutput.WS05_TestOk
                    DigitalOutputDescription = "WS05-test ok"
                Case eDigitalOutput.WS05_StartStep
                    DigitalOutputDescription = "WS05-start step"
                Case eDigitalOutput.WS05_RecipeLoaded
                    DigitalOutputDescription = "WS05-Recipe Loaded"
                Case eDigitalOutput.WS05_RelaodRecipe
                    DigitalOutputDescription = "WS05-Reload Recipe"
                Case eDigitalOutput.WS05_Alarm
                    DigitalOutputDescription = "WS05-Alarm"
                Case eDigitalOutput.WS05_ResetForce
                    DigitalOutputDescription = "WS05-Reset Force Sensor"

                Case eDigitalOutput.WS02_Keyence_PLC
                    DigitalOutputDescription = "WS02-Keyence PLC"
                Case eDigitalOutput.WS02_Keyence_Trig1
                    DigitalOutputDescription = "WS02-Keyence Trigger 1"
                Case eDigitalOutput.WS02_Keyence_Trig2
                    DigitalOutputDescription = "WS02-Keyence Trigger 2"
                Case eDigitalOutput.WS02_Keyence_Test
                    DigitalOutputDescription = "WS02-Keyence Test"
                Case eDigitalOutput.WS02_Keyence_Reset
                    DigitalOutputDescription = "WS02-Keyence Reset"

                Case Else
                    DigitalOutputDescription = String.Format("Free {0}", index)
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
            PowerDown = False
            For i = 0 To eDigitalOutput.Count - 1
                PowerDown = PowerDown Or ResetDigitalOutput(CType(i, eDigitalOutput))
            Next
            ' Disconnect the VIPA
            PowerDown = _VIPA.Disconnect
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            PowerDown = False
        End If
    End Function



    Public Function PowerUp() As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' Connect the VIPA
            PowerUp = _VIPA.Connect(mSettings.VIPAIPAddress, cVIPA.DefaultPortNumber)
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            PowerUp = False
        End If
    End Function



    Public Function ReadDigitalInputs() As Boolean
        ' If the hardware is enabled
        If (_hardwareEnabled) Then
            ' Read the digital inputs
            ReadDigitalInputs = _VIPA.ReadInputBits(0, eDigitalInput.Count, _digitalInputStatus)
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            ReadDigitalInputs = False
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
            ' Write the digital outputs
            WriteDigitalOutput = _VIPA.WriteOutputBits(0, eDigitalOutput.Count, _digitalOutputStatus)
        Else    ' Otherwise, if the hardware is disabled
            ' Return False
            WriteDigitalOutput = False
        End If
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module