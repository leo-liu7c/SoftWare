Option Explicit On
Option Strict On

Imports System
Imports System.IO

Public Class cWS04Results
    '+------------------------------------------------------------------------------+
    '|                             Public deMFarations                              |
    '+------------------------------------------------------------------------------+
    Public Enum eTestResult
        Unknown = 4000
        Passed = 0
        FailedPowerUP = 4001
        FailedInitCommunication = 4002
        FailedOpenDIAGonLINSession = 4003
        FailedCheckSerialNumber = 4004
        FailedCheckMMSTestByte = 4005

        FailedRearLeftUP_Ce1Test = 4010
        FailedRearLeftUP_Ce2Ce1Test = 4011
        FailedRearLeftUP_OverStrokeTest = 4012
        FailedRearLeftUP_PeakF1Test = 4013
        FailedRearLeftUP_F1F2Test = 4014
        FailedRearLeftUP_F4F5Test = 4015
        FailedRearLeftUP_F4F1Test = 4016
        FailedRearLeftUP_ElectricalTest = 4017
        FailedRearLeftUP_StrenghtTest = 4018
        FailedRearLeftUP_PeakF4Test = 4019

        FailedRearLeftDN_Ce1Test = 4020
        FailedRearLeftDN_Ce2Ce1Test = 4021
        FailedRearLeftDN_OverStrokeTest = 4022
        FailedRearLeftDN_PeakF1Test = 4023
        FailedRearLeftDN_F1F2Test = 4024
        FailedRearLeftDN_F4F5Test = 4025
        FailedRearLeftDN_F4F1Test = 4026
        FailedRearLeftDN_ElectricalTest = 4027
        FailedRearLeftDN_StrenghtTest = 4028
        FailedRearLeftDN_PeakF4Test = 4029

        FailedRearRightUP_Ce1Test = 4030
        FailedRearRightUP_Ce2Ce1Test = 4031
        FailedRearRightUP_OverStrokeTest = 4032
        FailedRearRightUP_PeakF1Test = 4033
        FailedRearRightUP_F1F2Test = 4034
        FailedRearRightUP_F4F5Test = 4035
        FailedRearRightUP_F4F1Test = 4036
        FailedRearRightUP_ElectricalTest = 4037
        FailedRearRightUP_StrenghtTest = 4038
        FailedRearRightUP_PeakF4Test = 4039

        FailedRearRightDN_Ce1Test = 4040
        FailedRearRightDN_Ce2Ce1Test = 4041
        FailedRearRightDN_OverStrokeTest = 4042
        FailedRearRightDN_PeakF1Test = 4043
        FailedRearRightDN_F1F2Test = 4044
        FailedRearRightDN_F4F5Test = 4045
        FailedRearRightDN_F4F1Test = 4046
        FailedRearRightDN_ElectricalTest = 4047
        FailedRearRightDN_StrenghtTest = 4048
        FailedRearRightDN_PeakF4Test = 4049

        FailedFolding_ElectricalTest = 4067
        FailedFolding_StrenghtTest = 4068

        FailedFinalStateProduct = 4050

        FailedWriteTestByte = 4051
        FailedReadTestByte = 4052

        FailedAbortTest = 4095
        FailedPartType = 4096
        FailedRuntimeError = 4097
        FailedMasterReference = 4098
        NotTested = 4099
    End Enum
    Public Enum eTestResultCN
        未知 = 4000
        OK = 0
        上电不良 = 4001
        初始化LIN通信 = 4002
        打开LIN诊断会话 = 4003
        检查序列号 = 4004
        检查MMS测试字段 = 4005

        RearLeftUP_导通1 = 4010
        RearLeftUP_导通2导通1 = 4011
        RearLeftUP_满行程 = 4012
        RearLeftUP_PeakF1 = 4013
        RearLeftUP_F1F2 = 4014
        RearLeftUP_F4F5 = 4015
        RearLeftUP_F4F1 = 4016
        RearLeftUP_电性能 = 4017
        RearLeftUP_FS = 4018

        RearLeftDN_导通1 = 4020
        RearLeftDN_导通2导通1 = 4021
        RearLeftDN_满行程 = 4022
        RearLeftDN_PeakF1 = 4023
        RearLeftDN_F1F2 = 4024
        RearLeftDN_F4F5 = 4025
        RearLeftDN_F4F1 = 4026
        RearLeftDN_电性能 = 4027
        RearLeftDN_FS = 4028

        RearRightUP_导通1 = 4030
        RearRightUP_导通2导通1 = 4031
        RearRightUP_满行程 = 4032
        RearRightUP_PeakF1 = 4033
        RearRightUP_F1F2 = 4034
        RearRightUP_F4F5 = 4035
        RearRightUP_F4F1 = 4036
        RearRightUP_电性能 = 4037
        RearRightUP_FS = 4038

        RearRightDN_导通1 = 4040
        RearRightDN_导通2导通1 = 4041
        RearRightDN_满行程 = 4042
        RearRightDN_PeakF1 = 4043
        RearRightDN_F1F2 = 4044
        RearRightDN_F4F5 = 4045
        RearRightDN_F4F1 = 4046
        RearRightDN_电性能 = 4047
        RearRightDN_FS = 4048

        Folding_电性能 = 4067
        Folding_FS = 4068

        最终产品状态 = 4050

        写测试状态字段 = 4051
        读测试状态字段 = 4052

        中断测试 = 4095
        产品类型 = 4096
        运行错误 = 4097
        MasterReference = 4098
        未测试 = 4099
    End Enum

    Public Enum eSingleTestResult
        NotTested = 0
        Disabled = 1
        Unknown = 2
        Passed = 3
        Failed = 4
        NotCoherent = 5
    End Enum

    Public Enum eWindowsLifterSignal
        Push_Manual = 0
        Push_Automtic = 1
        Pull_Manual = 2
        Pull_Automtic = 3

    End Enum

    Public Enum eWindowsLifterTest
        RearRight_Push = 0
        RearRight_Pull = 1
        RearLeft_Push = 2
        RearLeft_Pull = 3

        Not_Defined = 4
    End Enum

    Public Structure sForceIndex
        Dim X_F1 As Integer
        Dim X_F2 As Integer
        Dim X_F4 As Integer
        Dim X_F5 As Integer
        Dim X_Ce1 As Integer
        Dim X_Fe1 As Integer
        Dim X_Ce2 As Integer
        Dim X_Fe2 As Integer
        Dim X_VCe1 As Integer
        Dim X_VCe2 As Integer
        Dim Xs As Integer
        Dim Xe As Integer
    End Structure

    ''' <summary>
    ''' Force indexes.
    ''' </summary>
    Public WL_X_Indexes(0 To 3) As sForceIndex
    ''' <summary>
    ''' Force indexes.
    ''' </summary>
    Public MF_X_Indexes As sForceIndex


    ' Public constants
    Public Const SingleTestBaseIndex = 20
    Public Const SingleTestCount = 17
    Public Const ValueCount = 920
    Public Const _StandardSignal = 2
    Public Const MaxSampleCount = 20000

    ' Public variables
    ' General information
    Public TestResults As cResultValue
    Public TestDate As cResultValue
    Public TestTime As cResultValue
    Public RecipeName As cResultValue
    Public RecipeModifyDate As cResultValue
    Public RecipeModifyTime As cResultValue
    Public PartUniqueNumber As cResultValue
    Public PartTypeNumber As cResultValue

    ' Single test results as cResultValue
    Public ePOWER_UP As cResultValue
    Public eINIT_ROOF_LIN_COMMUNICATION As cResultValue
    Public eOPEN_DIAG_ON_LIN_SESSION As cResultValue
    Public eMMS_Traceability As cResultValue

    Public eRear_Left_Push_Electrical As cResultValue
    Public eRear_Left_Push_Strenght As cResultValue
    Public eRear_Left_Pull_Electrical As cResultValue
    Public eRear_Left_Pull_Strenght As cResultValue

    Public eRear_Right_Push_Electrical As cResultValue
    Public eRear_Right_Push_Strenght As cResultValue
    Public eRear_Right_Pull_Electrical As cResultValue
    Public eRear_Right_Pull_Strenght As cResultValue

    Public eFolding_Electrical As cResultValue
    Public eFolding_Strenght As cResultValue

    Public eFINAL_STATE_PRODUCT As cResultValue
    Public eWrite_MMSTestByte As cResultValue
    Public eRead_MMSTestByte As cResultValue

    Public Power_supply_voltage As cResultValue
    Public Power_supply_Normal_Current As cResultValue

    Public Valeo_Serial_Number As cResultValue
    Public MMS_Test_Byte_Before As cResultValue
    Public MMS_Test_Byte_After As cResultValue

    ' Tableau
    ' Electric OFF/Ce1/Ce2 , Push/Pull , Right/Left
    Public WL_Rear_Right_Push_Manual(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Rear_Right_Push_Automatic(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Rear_Right_Pull_Manual(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Rear_Right_Pull_Automatic(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Rear_Left_Push_Manual(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Rear_Left_Push_Automatic(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Rear_Left_Pull_Manual(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Rear_Left_Pull_Automatic(0 To 2, 0 To 1, 0 To 1) As cResultValue
    'Electric Push/Pull , Right/Left


    Public Push_MF_Electric(0 To 1) As cResultValue
    Public BakcInitialSate As cResultValue

    Public FinalState(0 To 27) As cResultValue

    Public Rear_RIGHT_Push As sForceIndex
    Public Rear_RIGHT_Pull As sForceIndex
    Public Rear_LEFT_Push As sForceIndex
    Public Rear_LEFT_Pull As sForceIndex

    Public Push_Folding As sForceIndex

    Public WL_Fs1_F1(0 To 1, 0 To 1) As cResultValue
    Public WL_Xs1(0 To 1, 0 To 1) As cResultValue
    Public WL_dFs1_Haptic_1(0 To 1, 0 To 1) As cResultValue
    Public WL_dXs1(0 To 1, 0 To 1) As cResultValue
    Public WL_FsCe1(0 To 1, 0 To 1) As cResultValue
    Public WL_XCe1(0 To 1, 0 To 1) As cResultValue
    Public WL_DiffS2Ce1(0 To 1, 0 To 1) As cResultValue
    Public WL_Fs2_F2(0 To 1, 0 To 1) As cResultValue
    Public WL_Xs2(0 To 1, 0 To 1) As cResultValue
    Public WL_dFs2_Haptic_2(0 To 1, 0 To 1) As cResultValue
    Public WL_dXs2(0 To 1, 0 To 1) As cResultValue
    Public WL_FsCe2(0 To 1, 0 To 1) As cResultValue
    Public WL_XCe2(0 To 1, 0 To 1) As cResultValue
    Public WL_DiffS5Ce2(0 To 1, 0 To 1) As cResultValue
    Public WL_Fe(0 To 1, 0 To 1) As cResultValue
    Public WL_Xe(0 To 1, 0 To 1) As cResultValue

    Public MF_Fs1_F1 As cResultValue
    Public MF_Xs1 As cResultValue
    Public MF_dFs1_Haptic_1 As cResultValue
    Public MF_dXs1 As cResultValue
    Public MF_FsCe1 As cResultValue
    Public MF_XCe1 As cResultValue
    Public MF_Fe As cResultValue
    Public MF_Xe As cResultValue
    Public MF_DiffS2Ce1 As cResultValue
    '+------------------------------------------------------------------------------+
    '|                             Private deMFarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Public _values(0 To ValueCount - 1) As cResultValue
    Private XMLResult As GenerateXMLResult

    ' Windows Lifter 1 to  8
    Public WS04_SampleCount(0 To 7) As Integer
    ' Signal EarlySensor, StrenghtSensor, Contact
    ''' <summary>
    ''' 1,WiliPosition, RearLeftPush,RearLeftPull;2,Sensor integer;3,Points;
    ''' </summary>
    Public Samples_Push_Pull(0 To 7, 0 To 5, 0 To 99999) As Single

    ' Windows Lifter 1 to  8
    Public SampleCount_TMP As Integer
    ' Signal EarlySensor, StrenghtSensor, Contact
    Public Sample_TMP(0 To 5, 0 To 99999) As Single

    Public Sample_TMP_AllKnob(2)(,) As Single
    Public Sample_TMP_AllKnobCount(2) As Single

    Public Tps_SignalLin_Start(0, 1) As Single
    Public Tps_SignalLin_Stop(0, 1) As Single

    'Push
    Public WS04_SampleCountPush As Integer
    ' Signal EarlySensor, StrenghtSensor, Contact
    Public WS04_SamplePush(0 To 2, 0 To 99999) As Single

    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public Property TestResult() As eTestResult
        Get
            TestResult = CType(TestResults.Value, eTestResult)
        End Get
        Set(ByVal value As eTestResult)
            TestResults.Value = value
        End Set
    End Property



    Public Property Value(ByVal index As Integer) As cResultValue
        Get
            Value = _values(index)
        End Get
        Set(ByVal value As cResultValue)
            _values(index) = value
        End Set
    End Property



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function Load(ByVal path As String) As Boolean
        Dim fileReader As StreamReader = Nothing
        Dim line As String
        Dim token() As String

        Try
            ' Open the file
            fileReader = New StreamReader(path)

            ' General information
            line = fileReader.ReadLine
            line = fileReader.ReadLine
            token = Split(line, vbTab)
            TestResult = CType(token(1), eTestResult)
            ' General information
            line = fileReader.ReadLine
            Load = LoadResultValue(fileReader, TestDate)
            Load = Load Or LoadResultValue(fileReader, TestTime)
            Load = Load Or LoadResultValue(fileReader, RecipeName)
            Load = Load Or LoadResultValue(fileReader, RecipeModifyDate)
            Load = Load Or LoadResultValue(fileReader, RecipeModifyTime)
            Load = Load Or LoadResultValue(fileReader, PartTypeNumber)
            Load = Load Or LoadResultValue(fileReader, PartUniqueNumber)
            line = fileReader.ReadLine

            ' SINGLE TEST RESULTS
            line = fileReader.ReadLine
            line = fileReader.ReadLine
            line = fileReader.ReadLine
            Load = Load Or LoadSingleTestResult(fileReader, ePOWER_UP)

            line = fileReader.ReadLine

        Catch ex As Exception
            Load = True
        Finally
            If (fileReader IsNot Nothing) Then
                fileReader.Close()
                fileReader = Nothing
            End If
        End Try
    End Function

    Public Function LoadConfiguration(ByVal path As String) As Boolean
        Dim i As Integer
        Dim j As Integer
        LoadConfiguration = mResults.LoadConfiguration(path, _values)
        ' General information
        TestResults = _values(0)
        TestDate = _values(1)
        TestTime = _values(2)
        RecipeName = _values(3)
        RecipeModifyDate = _values(4)
        RecipeModifyTime = _values(5)
        PartUniqueNumber = _values(6)
        PartTypeNumber = _values(7)
        i = 20


        ' Single test results
        ePOWER_UP = _values(i) : i = i + 1
        eINIT_ROOF_LIN_COMMUNICATION = _values(i) : i = i + 1
        eOPEN_DIAG_ON_LIN_SESSION = _values(i) : i = i + 1
        eMMS_Traceability = _values(i) : i = i + 1
        eRear_Left_Push_Electrical = _values(i) : i = i + 1
        eRear_Left_Push_Strenght = _values(i) : i = i + 1
        eRear_Left_Pull_Electrical = _values(i) : i = i + 1
        eRear_Left_Pull_Strenght = _values(i) : i = i + 1
        eRear_Right_Push_Electrical = _values(i) : i = i + 1
        eRear_Right_Push_Strenght = _values(i) : i = i + 1
        eRear_Right_Pull_Electrical = _values(i) : i = i + 1
        eRear_Right_Pull_Strenght = _values(i) : i = i + 1
        eFolding_Electrical = _values(i) : i = i + 1
        eFolding_Strenght = _values(i) : i = i + 1
        eFINAL_STATE_PRODUCT = _values(i) : i = i + 1
        eWrite_MMSTestByte = _values(i) : i = i + 1
        eRead_MMSTestByte = _values(i) : i = i + 1

        Power_supply_voltage = _values(50)
        Power_supply_Normal_Current = _values(51)

        'Read Valeo Serial Number 
        Valeo_Serial_Number = _values(80)
        MMS_Test_Byte_Before = _values(81)
        MMS_Test_Byte_After = _values(82)

        ' Electric OFF/Ce1/Ce2 , Push/Pull , Right/Left
        'Electric
        For ii = 0 To 1
            For j = 0 To 1
                If ii = 0 Then
                    For i = 0 To 2
                        WL_Rear_Left_Push_Manual(i, j, ii) = _values(100 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Rear_Left_Push_Automatic(i, j, ii) = _values(101 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Rear_Left_Pull_Manual(i, j, ii) = _values(102 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Rear_Left_Pull_Automatic(i, j, ii) = _values(103 + (10 * i) + (ii * 100) + (j * 50))
                    Next i
                Else
                    For i = 0 To 2
                        WL_Rear_Right_Push_Manual(i, j, ii) = _values(100 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Rear_Right_Push_Automatic(i, j, ii) = _values(101 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Rear_Right_Pull_Manual(i, j, ii) = _values(102 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Rear_Right_Pull_Automatic(i, j, ii) = _values(103 + (10 * i) + (ii * 100) + (j * 50))
                    Next i
                End If

            Next j
        Next ii
        'Ajout du MirrorFolding 
        i = 300
        Push_MF_Electric(0) = _values(i) : i += 1
        i = 310
        Push_MF_Electric(1) = _values(i) : i += 1

        For i = 0 To 27
            FinalState(i) = _values(340 + i)
        Next

        i = 370
        WL_Fs1_F1(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_Xs1(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_dXs1(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_FsCe1(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_XCe1(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_Fs2_F2(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_Xs2(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_dXs2(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_FsCe2(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_XCe2(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_Fe(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_Xe(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1
        WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, 0) = _values(i) : i += 1

        i = 390
        WL_Fs1_F1(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_Xs1(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_dXs1(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_FsCe1(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_XCe1(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_Fs2_F2(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_Xs2(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_dXs2(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_FsCe2(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_XCe2(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_Fe(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_Xe(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1
        WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, 0) = _values(i) : i += 1

        i = 410
        WL_Fs1_F1(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_Xs1(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_dXs1(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_FsCe1(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_XCe1(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_Fs2_F2(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_Xs2(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_dXs2(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_FsCe2(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_XCe2(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_Fe(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_Xe(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1
        WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, 1) = _values(i) : i += 1

        i = 430
        WL_Fs1_F1(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_Xs1(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_dXs1(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_FsCe1(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_XCe1(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_Fs2_F2(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_Xs2(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_dXs2(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_FsCe2(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_XCe2(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_Fe(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_Xe(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1
        WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, 1) = _values(i) : i += 1

        i = 450
        MF_Fs1_F1 = _values(i) : i += 1
        MF_Xs1 = _values(i) : i += 1
        MF_dFs1_Haptic_1 = _values(i) : i += 1
        MF_dXs1 = _values(i) : i += 1
        MF_FsCe1 = _values(i) : i += 1
        MF_XCe1 = _values(i) : i += 1
        MF_Fe = _values(i) : i += 1
        MF_Xe = _values(i) : i += 1
        MF_DiffS2Ce1 = _values(i) : i += 1
        BakcInitialSate = _values(i) : i += 1
        ' Set the test result to NotTested
        TestResult = eTestResult.NotTested
    End Function



    Public Shared Function SingleTestResultDescription(ByVal value As eSingleTestResult) As String
        Select Case value
            Case eSingleTestResult.Disabled
                SingleTestResultDescription = "Disabled"
            Case eSingleTestResult.Unknown
                SingleTestResultDescription = "Unknown"
            Case eSingleTestResult.Passed
                SingleTestResultDescription = "Passed"
            Case eSingleTestResult.Failed
                SingleTestResultDescription = "Failed"
            Case eSingleTestResult.NotCoherent
                SingleTestResultDescription = "Not coherent"
            Case eSingleTestResult.NotTested
                SingleTestResultDescription = ""
            Case Else
                SingleTestResultDescription = String.Format("Value {0} unknown", value)
        End Select
    End Function



    Public Shared Function TestResultDescription(ByVal value As eTestResult) As String
        Select Case value
            Case eTestResult.Unknown
                TestResultDescription = "Unknown"
            Case eTestResult.Passed
                TestResultDescription = "Passed"
            Case eTestResult.FailedPowerUP
                TestResultDescription = "Failed Power UP"
            Case eTestResult.FailedInitCommunication
                TestResultDescription = "Failed  Communication"
            Case eTestResult.FailedOpenDIAGonLINSession
                TestResultDescription = "Failed Open DIAG on LIN Session"

            Case eTestResult.FailedCheckSerialNumber
                TestResultDescription = "Failed Valeo Serial Number"

            Case eTestResult.FailedRearLeftUP_Ce1Test
                TestResultDescription = "Failed Rear Left UP Ce1 Test"
            Case eTestResult.FailedRearLeftUP_Ce2Ce1Test
                TestResultDescription = "Failed Rear Left UP Ce2Ce1 Test"
            Case eTestResult.FailedRearLeftUP_OverStrokeTest
                TestResultDescription = "Failed Rear Left UP Overstroke Test"
            Case eTestResult.FailedRearLeftUP_PeakF1Test
                TestResultDescription = "Failed Rear Left UP PeakF1 Test"
            Case eTestResult.FailedRearLeftUP_F1F2Test
                TestResultDescription = "Failed Rear Left UP F1F2 Test"
            Case eTestResult.FailedRearLeftUP_F4F5Test
                TestResultDescription = "Failed Rear Left UP F4F5 Test"
            Case eTestResult.FailedRearLeftUP_F4F1Test
                TestResultDescription = "Failed Rear Left UP F4F1 Test"
            Case eTestResult.FailedRearLeftUP_ElectricalTest
                TestResultDescription = "Failed Rear Left UP Electrical Test"
            Case eTestResult.FailedRearLeftUP_StrenghtTest
                TestResultDescription = "Failed Rear Left UP Strenght Test"
            Case eTestResult.FailedRearLeftUP_PeakF4Test
                TestResultDescription = "Failed Rear Left UP PeakF4 Test"

            Case eTestResult.FailedRearLeftDN_Ce1Test
                TestResultDescription = "Failed Rear Left DN Ce1 Test"
            Case eTestResult.FailedRearLeftDN_Ce2Ce1Test
                TestResultDescription = "Failed Rear Left DN Ce2Ce1 Test"
            Case eTestResult.FailedRearLeftDN_OverStrokeTest
                TestResultDescription = "Failed Rear Left DN Overstroke Test"
            Case eTestResult.FailedRearLeftDN_PeakF1Test
                TestResultDescription = "Failed Rear Left DN PeakF1 Test"
            Case eTestResult.FailedRearLeftDN_F1F2Test
                TestResultDescription = "Failed Rear Left DN F1F2 Test"
            Case eTestResult.FailedRearLeftDN_F4F5Test
                TestResultDescription = "Failed Rear Left DN F4F5 Test"
            Case eTestResult.FailedRearLeftDN_F4F1Test
                TestResultDescription = "Failed Rear Left DN F4F1 Test"
            Case eTestResult.FailedRearLeftDN_ElectricalTest
                TestResultDescription = "Failed Rear Left DN Electrical Test"
            Case eTestResult.FailedRearLeftDN_StrenghtTest
                TestResultDescription = "Failed Rear Left DN Strenght Test"
            Case eTestResult.FailedRearLeftDN_PeakF4Test
                TestResultDescription = "Failed Rear Left DN PeakF4 Test"

            Case eTestResult.FailedRearRightUP_Ce1Test
                TestResultDescription = "Failed Rear Right UP Ce1 Test"
            Case eTestResult.FailedRearRightUP_Ce2Ce1Test
                TestResultDescription = "Failed Rear Right UP Ce2Ce1 Test"
            Case eTestResult.FailedRearRightUP_OverStrokeTest
                TestResultDescription = "Failed Rear Right UP Overstroke Test"
            Case eTestResult.FailedRearRightUP_PeakF1Test
                TestResultDescription = "Failed Rear Right UP PeakF1 Test"
            Case eTestResult.FailedRearRightUP_F1F2Test
                TestResultDescription = "Failed Rear Right UP F1F2 Test"
            Case eTestResult.FailedRearRightUP_F4F5Test
                TestResultDescription = "Failed Rear Right UP F4F5 Test"
            Case eTestResult.FailedRearRightUP_F4F1Test
                TestResultDescription = "Failed Rear Right UP F4F1 Test"
            Case eTestResult.FailedRearRightUP_ElectricalTest
                TestResultDescription = "Failed Rear Right UP Electrical Test"
            Case eTestResult.FailedRearRightUP_StrenghtTest
                TestResultDescription = "Failed Rear Right UP Strenght Test"
            Case eTestResult.FailedRearRightUP_PeakF4Test
                TestResultDescription = "Failed Rear Right UP PeakF4 Test"

            Case eTestResult.FailedRearRightDN_Ce1Test
                TestResultDescription = "Failed Rear Right DN Ce1 Test"
            Case eTestResult.FailedRearRightDN_Ce2Ce1Test
                TestResultDescription = "Failed Rear Right DN Ce2Ce1 Test"
            Case eTestResult.FailedRearRightDN_OverStrokeTest
                TestResultDescription = "Failed Rear Right DN Overstroke Test"
            Case eTestResult.FailedRearRightDN_PeakF1Test
                TestResultDescription = "Failed Rear Right DN PeakF1 Test"
            Case eTestResult.FailedRearRightDN_F1F2Test
                TestResultDescription = "Failed Rear Right DN F1F2 Test"
            Case eTestResult.FailedRearRightDN_F4F5Test
                TestResultDescription = "Failed Rear Right DN F4F5 Test"
            Case eTestResult.FailedRearRightDN_F4F1Test
                TestResultDescription = "Failed Rear Right DN F4F1 Test"
            Case eTestResult.FailedRearRightDN_ElectricalTest
                TestResultDescription = "Failed Rear Right DN Electrical Test"
            Case eTestResult.FailedRearRightDN_StrenghtTest
                TestResultDescription = "Failed Rear Right DN Strenght Test"
            Case eTestResult.FailedRearRightDN_PeakF4Test
                TestResultDescription = "Failed Rear Right DN PeakF4 Test"


            Case eTestResult.FailedFinalStateProduct
                TestResultDescription = "Failed Final State Product"


            Case eTestResult.FailedAbortTest
                TestResultDescription = "Failed abort test"
            Case eTestResult.FailedPartType
                TestResultDescription = "Failed part type test"
            Case eTestResult.FailedRuntimeError
                TestResultDescription = "Failed runtime error"
            Case eTestResult.FailedMasterReference
                TestResultDescription = "Failed master reference"
            Case eTestResult.NotTested
                TestResultDescription = "Not tested"
            Case Else
                TestResultDescription = String.Format("Value {0} unknown", value)
        End Select
    End Function


    Public Function Save(ByVal path As String) As Boolean
        Dim fileWriter As StreamWriter = Nothing
        Dim XMLResultPath As String = String.Empty
        Try
            ' Open the file
            fileWriter = New StreamWriter(path)
            XMLResult = New GenerateXMLResult(path, XMLResultPath)
            XMLResult.InitilseXMLData()
            Dim groupName As String 'Used for XMLResult.
            groupName = "CommonTest"
            ' General information
            fileWriter.WriteLine("GENERAL INFORMATION")
            fileWriter.WriteLine("Test Result code : " &
                                 vbTab & TestResult &
                                 vbTab &
                                 vbTab & "Test Mode Description : " &
                                 vbTab & mWS04Main.TestModeDescription(mWS04Main.TestMode) &
                                 vbTab & "Test Result Description : " &
                                 vbTab & TestResultDescription(CType(TestResult, eTestResult)))
            fileWriter.WriteLine("")
            Save = SaveResultValue(fileWriter, TestDate)
            Save = Save Or SaveResultValue(fileWriter, TestTime)
            Save = Save Or SaveResultValue(fileWriter, RecipeName)
            Save = Save Or SaveResultValue(fileWriter, RecipeModifyDate)
            Save = Save Or SaveResultValue(fileWriter, RecipeModifyTime)
            Save = Save Or SaveResultValue(fileWriter, PartTypeNumber)
            Save = Save Or SaveResultValue(fileWriter, PartUniqueNumber)
            fileWriter.WriteLine("Test Software Version :" & mConstants.SoftwareVersion & vbTab & " Update Date:" & mConstants.SoftwareDate)
            fileWriter.WriteLine("Test Cycle Time :" & mWS04Main.TestLast)
            fileWriter.WriteLine("")

            groupName = "Single"
            ' SINGLE TEST RESULTS
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  SINGLE TEST RESULTS                                              ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveSingleTestResult(fileWriter, ePOWER_UP, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eINIT_ROOF_LIN_COMMUNICATION, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eOPEN_DIAG_ON_LIN_SESSION, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eMMS_Traceability, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRear_Left_Push_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRear_Left_Push_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRear_Left_Pull_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRear_Left_Pull_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRear_Right_Push_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRear_Right_Push_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRear_Right_Pull_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRear_Right_Pull_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFolding_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFolding_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFINAL_STATE_PRODUCT, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eWrite_MMSTestByte, groupName)
            fileWriter.WriteLine("")

            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Power UP TEST RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Power_supply_voltage, groupName)
            Save = Save Or SaveResultValue(fileWriter, Power_supply_Normal_Current, groupName)
            fileWriter.WriteLine("")

            groupName = ""
            'Read Valeo Serial Number 
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Valeo Serial NumberTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Valeo_Serial_Number, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Test_Byte_Before, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Test_Byte_After, groupName)
            fileWriter.WriteLine("")

            groupName = "RR_Push"
            'Push
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Rear Right Push Test RESULTS                                    ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_XCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_XCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce2 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("")

            groupName = "RR_Pull"
            'Pull
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Rear Right Pull Test RESULTS                                    ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_XCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_XCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce2 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Push_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Right_Pull_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight), groupName)
            fileWriter.WriteLine("")

            groupName = "RL_Push"
            'Push
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Rear Left Push Test RESULTS                                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_XCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_XCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce2 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("")

            groupName = "RL_Pull"
            'Pull
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Rear Left Pull Test RESULTS                                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_XCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_XCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce2 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Push_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Rear_Left_Pull_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft), groupName)
            fileWriter.WriteLine("")

            groupName = "Folding"
            'Pull
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  MirrorFolding Lock Push Test RESULTS                                  ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, MF_XCe1, groupName)
            Save = Save Or SaveResultValue(fileWriter, MF_Xe, groupName)
            Save = Save Or SaveResultValue(fileWriter, MF_DiffS2Ce1, groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, MF_Fs1_F1, groupName)
            Save = Save Or SaveResultValue(fileWriter, MF_Xs1, groupName)
            Save = Save Or SaveResultValue(fileWriter, MF_dFs1_Haptic_1, groupName)
            Save = Save Or SaveResultValue(fileWriter, MF_dXs1, groupName)

            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_MF_Electric(mGlobal.eElec.Off), groupName)

            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_MF_Electric(mGlobal.eElec.Ce1), groupName)

            groupName = "Final"
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Final Position TEST RESULTS                                      ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            For i = 0 To 27
                Save = Save Or SaveResultValue(fileWriter, FinalState(i), groupName)
            Next

        Catch ex As Exception
            Save = True
        Finally
            If (fileWriter IsNot Nothing) Then
                fileWriter.Close()
                fileWriter = Nothing
            End If
            Try
                'Must Put it in the end, better after fileWriter.Close(), to be ensure all measurements saved.
                Dim tempUnitStatus As UnitStatus
                If TestResult = eTestResult.Passed Then
                    tempUnitStatus = UnitStatus.PASS
                Else
                    tempUnitStatus = UnitStatus.FAIL
                End If
                Dim tempConvertCode As TestMode
                Select Case mWS04Main.TestMode
                    Case mWS04Main.eTestMode.Remote
                        tempConvertCode = TestMode.MassProduction
                    Case mWS04Main.eTestMode.Master
                        tempConvertCode = TestMode.DailyCheck
                    Case Else
                        tempConvertCode = TestMode.Unknown
                End Select
                XMLResult.CreateXML_Unit(4, PartUniqueNumber.Value.ToString, tempUnitStatus, 0, CDate(TestDate.StringValue & " " & TestTime.StringValue),
                                Now, CInt(IIf(mWS04Main.TestLast > 30000, 9999, mWS04Main.TestLast)), 0, "", tempConvertCode)
                XMLResult.CreateXML(XMLResultPath, Now, Now, "0040615", RecipeName.StringValue, mConstants.SoftwareVersion, "SquMain01", "1234567")
            Catch ex As Exception
                Console.WriteLine(ex.ToString())
            End Try

        End Try
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Function LoadResultValue(ByVal fileReader As StreamReader, _
                                     ByVal value As cResultValue) As Boolean
        Dim line As String
        Dim token() As String

        Try
            line = fileReader.ReadLine
            token = Split(line, vbTab)
            value.MinimumLimit = token(1)
            value.Value = token(2)
            value.MaximumLimit = token(3)
            value.TestResult = CType(token(4), cResultValue.eValueTestResult)
            LoadResultValue = False

        Catch ex As Exception
            LoadResultValue = True
        End Try
    End Function



    Private Function LoadSingleTestResult(ByVal fileReader As StreamReader, _
                                          ByVal value As cResultValue) As Boolean
        Dim line As String
        Dim token() As String

        Try
            line = fileReader.ReadLine
            token = Split(line, vbTab)
            value.TestResult = CType(token(4), cResultValue.eValueTestResult)
            LoadSingleTestResult = False

        Catch ex As Exception
            LoadSingleTestResult = True
        End Try
    End Function
    ''' <summary>
    ''' Save to Log and XML Result
    ''' </summary>
    ''' <param name="fileWriter"></param>
    ''' <param name="value"></param>
    ''' <param name="groupName">Used for XML, if no, then this item do not save into XML result</param>
    ''' <returns></returns>
    Private Function SaveResultValue(ByVal fileWriter As StreamWriter,
                                     ByVal value As cResultValue, Optional ByVal groupName As String = "") As Boolean
        Try
            fileWriter.WriteLine(value.Description &
                                 vbTab & value.StringMinimumLimit &
                                 vbTab & value.StringValue &
                                 vbTab & value.StringMaximumLimit &
                                 vbTab & value.TestResult &
                                 vbTab & cResultValue.ValueTestResultDescription(value.TestResult))
            Try
                If (value.Units = "Step") Then
                    Return False
                End If
                'If (groupName <> "") Then
                With XMLResult
                    .CreateMeasurementsSaveResultValueForUnit(value, groupName)
                End With
                'End If
            Catch ex As Exception

            End Try
            SaveResultValue = False
        Catch ex As Exception
            SaveResultValue = True
        End Try
    End Function


    ''' <summary>
    ''' Save to Log and XML Result
    ''' </summary>
    ''' <param name="fileWriter"></param>
    ''' <param name="value"></param>
    ''' <param name="groupName">Used for XML, if no, then this item do not save into XML result</param>
    ''' <returns></returns>
    Private Function SaveSingleTestResult(ByVal fileWriter As StreamWriter,
                                          ByVal value As cResultValue, Optional ByVal groupName As String = "") As Boolean
        Try
            fileWriter.WriteLine(value.Description &
                                 vbTab &
                                 vbTab & value.StringValue &
                                 vbTab &
                                 vbTab & value.TestResult &
                                 vbTab & SingleTestResultDescription(CType(value.TestResult, eSingleTestResult)))
            Try
                If (value.Units = "Step") Then
                    Return False
                End If
                'If (groupName <> "") Then
                With XMLResult
                    .CreateMeasurementsSaveSingleTestResultForUnit(value, groupName)
                End With
                'End If
            Catch ex As Exception

            End Try
            SaveSingleTestResult = False
        Catch ex As Exception
            SaveSingleTestResult = True
        End Try
    End Function
End Class
