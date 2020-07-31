Option Explicit On
Option Strict On

Imports System
Imports System.IO

Public Class cWS03Results
    '+------------------------------------------------------------------------------+
    '|                             Public deMFarations                              |
    '+------------------------------------------------------------------------------+
    Public Enum eTestResult
        Unknown = 3000
        Passed = 0
        FailedPowerUP = 3001
        FailedInitCommunication = 3002
        FailedOpenDIAGonLINSession = 3003
        FailedCheckSerialNumber = 3004
        FailedCheckMMSTestByte = 3005

        FailedFrontLeftUP_Ce1Test = 3010
        FailedFrontLeftUP_Ce2Ce1Test = 3011
        FailedFrontLeftUP_OverStrokeTest = 3012
        FailedFrontLeftUP_PeakF1Test = 3013
        FailedFrontLeftUP_F1F2Test = 3014
        FailedFrontLeftUP_F4F5Test = 3015
        FailedFrontLeftUP_F4F1Test = 3016
        FailedFrontLeftUP_ElectricalTest = 3017
        FailedFrontLeftUP_StrenghtTest = 3018
        FailedFrontLeftUP_PeakF4Test = 3019

        FailedFrontLeftDN_Ce1Test = 3020
        FailedFrontLeftDN_Ce2Ce1Test = 3021
        FailedFrontLeftDN_OverStrokeTest = 3022
        FailedFrontLeftDN_PeakF1Test = 3023
        FailedFrontLeftDN_F1F2Test = 3024
        FailedFrontLeftDN_F4F5Test = 3025
        FailedFrontLeftDN_F4F1Test = 3026
        FailedFrontLeftDN_ElectricalTest = 3027
        FailedFrontLeftDN_StrenghtTest = 3028
        FailedFrontLeftDN_PeakF4Test = 3029

        FailedFrontRightUP_Ce1Test = 3030
        FailedFrontRightUP_Ce2Ce1Test = 3031
        FailedFrontRightUP_OverStrokeTest = 3032
        FailedFrontRightUP_PeakF1Test = 3033
        FailedFrontRightUP_F1F2Test = 3034
        FailedFrontRightUP_F4F5Test = 3035
        FailedFrontRightUP_F4F1Test = 3036
        FailedFrontRightUP_ElectricalTest = 3037
        FailedFrontRightUP_StrenghtTest = 3038
        FailedFrontRightUP_PeakF4Test = 3039

        FailedFrontRightDN_Ce1Test = 3040
        FailedFrontRightDN_Ce2Ce1Test = 3041
        FailedFrontRightDN_OverStrokeTest = 3042
        FailedFrontRightDN_PeakF1Test = 3043
        FailedFrontRightDN_F1F2Test = 3044
        FailedFrontRightDN_F4F5Test = 3045
        FailedFrontRightDN_F4F1Test = 3046
        FailedFrontRightDN_ElectricalTest = 3047
        FailedFrontRightDN_StrenghtTest = 3048
        FailedFrontRightDN_PeakF4Test = 3049

        FailedChildren_ElectricalTest = 3067
        FailedChildren_StrenghtTest = 3068

        FailedFinalStateProduct = 3050

        FailedWriteTestByte = 3051
        FailedReadTestByte = 3052

        FailedAbortTest = 3095
        FailedPartType = 3096
        FailedRuntimeError = 3097
        FailedMasterReference = 3098
        NotTested = 3099
    End Enum
    Public Enum eTestResultCN
        未知 = 3000
        OK = 0
        上电不良 = 3001
        初始化LIN通信 = 3002
        打开LIN诊断会话 = 3003
        检查序列号 = 3004
        检查MMS测试字段 = 3005

        FrontLeftUP_导通1 = 3010
        FrontLeftUP_导通2导通1 = 3011
        FrontLeftUP_满行程 = 3012
        FrontLeftUP_PeakF1 = 3013
        FrontLeftUP_F1F2 = 3014
        FrontLeftUP_F4F5 = 3015
        FrontLeftUP_F4F1 = 3016
        FrontLeftUP_电性能 = 3017
        FrontLeftUP_FS = 3018
        FrontLeftUP_PeakF4 = 3019

        FrontLeftDN_导通1 = 3020
        FrontLeftDN_导通2导通1 = 3021
        FrontLeftDN_满行程 = 3022
        FrontLeftDN_PeakF1 = 3023
        FrontLeftDN_F1F2 = 3024
        FrontLeftDN_F4F5 = 3025
        FrontLeftDN_F4F1 = 3026
        FrontLeftDN_电性能 = 3027
        FrontLeftDN_FS = 3028
        FrontLeftDN_PeakF4 = 3029

        FrontRightUP_导通1 = 3030
        FrontRightUP_导通2导通1 = 3031
        FrontRightUP_满行程 = 3032
        FrontRightUP_PeakF1 = 3033
        FrontRightUP_F1F2 = 3034
        FrontRightUP_F4F5 = 3035
        FrontRightUP_F4F1 = 3036
        FrontRightUP_电性能 = 3037
        FrontRightUP_FS = 3038
        FrontRightUP_PeakF4 = 3039

        FrontRightDN_导通1 = 3040
        FrontRightDN_导通2导通1 = 3041
        FrontRightDN_满行程 = 3042
        FrontRightDN_PeakF1 = 3043
        FrontRightDN_F1F2 = 3044
        FrontRightDN_F4F5 = 3045
        FrontRightDN_F4F1 = 3046
        FrontRightDN_电性能 = 3047
        FrontRightDN_FS = 3048
        FrontRightDN_PeakF4 = 3049

        Children_电性能 = 3067
        Children_FS = 3068

        最终产品状态 = 3050

        写测试状态字段 = 3051
        读测试状态字段 = 3052

        中断测试 = 3095
        产品类型 = 3096
        运行错误 = 3097
        MasterReference = 3098
        未测试 = 3099
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
        Jama_Down = 4
    End Enum

    Public Enum eWindowsLifterTest
        FrontLeft_Push = 0
        FrontLeft_Pull = 1
        FrontRight_Push = 2
        FrontRight_Pull = 3

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

    Public WL_X_Indexes(0 To 3) As sForceIndex
    Public CL_X_Indexes As sForceIndex
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

    Public eFront_Left_Push_Electrical As cResultValue
    Public eFront_Left_Push_Strenght As cResultValue
    Public eFront_Left_Pull_Electrical As cResultValue
    Public eFront_Left_Pull_Strenght As cResultValue

    Public eFront_Right_Push_Electrical As cResultValue
    Public eFront_Right_Push_Strenght As cResultValue
    Public eFront_Right_Pull_Electrical As cResultValue
    Public eFront_Right_Pull_Strenght As cResultValue

    Public eChildren_Electrical As cResultValue
    Public eChildren_Strenght As cResultValue

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
    Public WL_Front_Right_Push_Manual(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Right_Push_Automatic(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Right_Pull_Manual(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Right_Pull_Automatic(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Left_Push_Manual(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Left_Push_Automatic(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Left_Pull_Manual(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Left_Pull_Automatic(0 To 2, 0 To 1, 0 To 1) As cResultValue

    Public WL_Front_Left_Jama_UP(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Left_Jama_Down(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Right_Jama_UP(0 To 2, 0 To 1, 0 To 1) As cResultValue
    Public WL_Front_Right_Jama_Down(0 To 2, 0 To 1, 0 To 1) As cResultValue
    'Electric Push/Pull , Right/Left

    Public Push_CL_Electric(0 To 1) As cResultValue

    Public FinalState(0 To 27) As cResultValue

    Public Front_RIGHT_Push As sForceIndex
    Public Front_RIGHT_Pull As sForceIndex
    Public Front_LEFT_Push As sForceIndex
    Public Front_LEFT_Pull As sForceIndex

    Public Push_ChildrenLock As sForceIndex

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

    Public BakcInitialSate As cResultValue

    Public CL_Fs1_F1 As cResultValue
    Public CL_Xs1 As cResultValue
    Public CL_dFs1_Haptic_1 As cResultValue
    Public CL_dXs1 As cResultValue
    Public CL_FsCe1 As cResultValue
    Public CL_XCe1 As cResultValue
    Public CL_Fe As cResultValue
    Public CL_Xe As cResultValue
    Public CL_DiffS2Ce1 As cResultValue

    '+------------------------------------------------------------------------------+
    '|                             Private deMFarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Public _values(0 To ValueCount - 1) As cResultValue

    ' Windows Lifter 1 to  8
    Public WS03_SampleCount(0 To 3) As Integer
    ' Push, Pull, Signal EarlySensor, StrenghtSensor, Contact
    Public Samples_Push_Pull(0 To 3, 0 To 7, 0 To 9999) As Single

    ' Windows Lifter 1 to  8
    Public SampleCount_TMP As Integer
    ' Signal EarlySensor, StrenghtSensor, Contact
    Public Sample_TMP(0 To 7, 0 To 99999) As Single

    Public Sample_TMP_AllKnob(2)(,) As Single
    Public Sample_TMP_AllKnobCount(2) As Single

    Public Tps_SignalLin_Start(0, 1) As Single
    Public Tps_SignalLin_Stop(0, 1) As Single

    'Push
    Public WS03_SampleCountPush As Integer
    ' Signal EarlySensor, StrenghtSensor, Contact
    Public WS03_SamplePush(0 To 2, 0 To 9999) As Single
    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private XMLResult As GenerateXMLResult

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
        eFront_Left_Push_Electrical = _values(i) : i = i + 1
        eFront_Left_Push_Strenght = _values(i) : i = i + 1
        eFront_Left_Pull_Electrical = _values(i) : i = i + 1
        eFront_Left_Pull_Strenght = _values(i) : i = i + 1
        eFront_Right_Push_Electrical = _values(i) : i = i + 1
        eFront_Right_Push_Strenght = _values(i) : i = i + 1
        eFront_Right_Pull_Electrical = _values(i) : i = i + 1
        eFront_Right_Pull_Strenght = _values(i) : i = i + 1
        eChildren_Electrical = _values(i) : i = i + 1
        eChildren_Strenght = _values(i) : i = i + 1
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
            'ii,0:Left, 1:Right
            For j = 0 To 1
                'j,0:Push, 1:Pull
                If ii = 0 Then
                    For i = 0 To 2
                        'i,0:Before switching,1:Manual,2:Auto
                        WL_Front_Left_Push_Manual(i, j, ii) = _values(100 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Left_Push_Automatic(i, j, ii) = _values(101 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Left_Pull_Manual(i, j, ii) = _values(102 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Left_Pull_Automatic(i, j, ii) = _values(103 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Left_Jama_UP(i, j, ii) = _values(104 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Left_Jama_Down(i, j, ii) = _values(105 + (10 * i) + (ii * 100) + (j * 50))
                    Next i
                Else
                    For i = 0 To 2
                        'i,0:Before switching,1:Manual,2:Auto
                        WL_Front_Right_Push_Manual(i, j, ii) = _values(100 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Right_Push_Automatic(i, j, ii) = _values(101 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Right_Pull_Manual(i, j, ii) = _values(102 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Right_Pull_Automatic(i, j, ii) = _values(103 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Right_Jama_UP(i, j, ii) = _values(104 + (10 * i) + (ii * 100) + (j * 50))
                        WL_Front_Right_Jama_Down(i, j, ii) = _values(105 + (10 * i) + (ii * 100) + (j * 50))
                    Next i
                End If
                'Force and Stroke
                'WL_Peak_Strenght_F1(j, ii) = _values(130 + (ii * 100) + (j * 50))
                'WL_Strenght_Ratio_F1_F2(j, ii) = _values(131 + (ii * 100) + (j * 50))
                'WL_Strenght_Ratio_F4_F5(j, ii) = _values(132 + (ii * 100) + (j * 50))
                'WL_Strenght_Ratio_F4_F1(j, ii) = _values(133 + (ii * 100) + (j * 50))
                'WL_Early_Ce1(j, ii) = _values(140 + (ii * 100) + (j * 50))
                'WL_Early_Ce2_Ce1(j, ii) = _values(141 + (ii * 100) + (j * 50))
                'WL_Over_stroke(j, ii) = _values(142 + (ii * 100) + (j * 50))
            Next j
        Next ii
        'Ajout du MirrorFolding Lock
        i = 300
        Push_CL_Electric(0) = _values(i) : i += 1
        i = 310
        Push_CL_Electric(1) = _values(i) : i += 1

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
        CL_Fs1_F1 = _values(i) : i += 1
        CL_Xs1 = _values(i) : i += 1
        CL_dFs1_Haptic_1 = _values(i) : i += 1
        CL_dXs1 = _values(i) : i += 1
        CL_FsCe1 = _values(i) : i += 1
        CL_XCe1 = _values(i) : i += 1
        CL_Fe = _values(i) : i += 1
        CL_Xe = _values(i) : i += 1
        CL_DiffS2Ce1 = _values(i) : i += 1

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

            Case eTestResult.FailedFrontLeftUP_Ce1Test
                TestResultDescription = "Failed Front Left UP Ce1 Test"
            Case eTestResult.FailedFrontLeftUP_Ce2Ce1Test
                TestResultDescription = "Failed Front Left UP Ce2Ce1 Test"
            Case eTestResult.FailedFrontLeftUP_OverStrokeTest
                TestResultDescription = "Failed Front Left UP Overstroke Test"
            Case eTestResult.FailedFrontLeftUP_PeakF1Test
                TestResultDescription = "Failed Front Left UP PeakF1 Test"
            Case eTestResult.FailedFrontLeftUP_F1F2Test
                TestResultDescription = "Failed Front Left UP F1F2 Test"
            Case eTestResult.FailedFrontLeftUP_F4F5Test
                TestResultDescription = "Failed Front Left UP F4F5 Test"
            Case eTestResult.FailedFrontLeftUP_F4F1Test
                TestResultDescription = "Failed Front Left UP F4F1 Test"
            Case eTestResult.FailedFrontLeftUP_ElectricalTest
                TestResultDescription = "Failed Front Left UP Electrical Test"
            Case eTestResult.FailedFrontLeftUP_StrenghtTest
                TestResultDescription = "Failed Front Left UP Strenght Test"
            Case eTestResult.FailedFrontLeftUP_PeakF4Test
                TestResultDescription = "Failed Front Left UP PeakF4 Test"

            Case eTestResult.FailedFrontLeftDN_Ce1Test
                TestResultDescription = "Failed Front Left DN Ce1 Test"
            Case eTestResult.FailedFrontLeftDN_Ce2Ce1Test
                TestResultDescription = "Failed Front Left DN Ce2Ce1 Test"
            Case eTestResult.FailedFrontLeftDN_OverStrokeTest
                TestResultDescription = "Failed Front Left DN Overstroke Test"
            Case eTestResult.FailedFrontLeftDN_PeakF1Test
                TestResultDescription = "Failed Front Left DN PeakF1 Test"
            Case eTestResult.FailedFrontLeftDN_F1F2Test
                TestResultDescription = "Failed Front Left DN F1F2 Test"
            Case eTestResult.FailedFrontLeftDN_F4F5Test
                TestResultDescription = "Failed Front Left DN F4F5 Test"
            Case eTestResult.FailedFrontLeftDN_F4F1Test
                TestResultDescription = "Failed Front Left DN F4F1 Test"
            Case eTestResult.FailedFrontLeftDN_ElectricalTest
                TestResultDescription = "Failed Front Left DN Electrical Test"
            Case eTestResult.FailedFrontLeftDN_StrenghtTest
                TestResultDescription = "Failed Front Left DN Strenght Test"
            Case eTestResult.FailedFrontLeftDN_PeakF4Test
                TestResultDescription = "Failed Front Left DN PeakF4 Test"

            Case eTestResult.FailedFrontRightUP_Ce1Test
                TestResultDescription = "Failed Front Right UP Ce1 Test"
            Case eTestResult.FailedFrontRightUP_Ce2Ce1Test
                TestResultDescription = "Failed Front Right UP Ce2Ce1 Test"
            Case eTestResult.FailedFrontRightUP_OverStrokeTest
                TestResultDescription = "Failed Front Right UP Overstroke Test"
            Case eTestResult.FailedFrontRightUP_PeakF1Test
                TestResultDescription = "Failed Front Right UP PeakF1 Test"
            Case eTestResult.FailedFrontRightUP_F1F2Test
                TestResultDescription = "Failed Front Right UP F1F2 Test"
            Case eTestResult.FailedFrontRightUP_F4F5Test
                TestResultDescription = "Failed Front Right UP F4F5 Test"
            Case eTestResult.FailedFrontRightUP_F4F1Test
                TestResultDescription = "Failed Front Right UP F4F1 Test"
            Case eTestResult.FailedFrontRightUP_ElectricalTest
                TestResultDescription = "Failed Front Right UP Electrical Test"
            Case eTestResult.FailedFrontRightUP_StrenghtTest
                TestResultDescription = "Failed Front Right UP Strenght Test"
            Case eTestResult.FailedFrontRightUP_PeakF4Test
                TestResultDescription = "Failed Front Right UP PeakF4 Test"

            Case eTestResult.FailedFrontRightDN_Ce1Test
                TestResultDescription = "Failed Front Right DN Ce1 Test"
            Case eTestResult.FailedFrontRightDN_Ce2Ce1Test
                TestResultDescription = "Failed Front Right DN Ce2Ce1 Test"
            Case eTestResult.FailedFrontRightDN_OverStrokeTest
                TestResultDescription = "Failed Front Right DN Overstroke Test"
            Case eTestResult.FailedFrontRightDN_PeakF1Test
                TestResultDescription = "Failed Front Right DN PeakF1 Test"
            Case eTestResult.FailedFrontRightDN_F1F2Test
                TestResultDescription = "Failed Front Right DN F1F2 Test"
            Case eTestResult.FailedFrontRightDN_F4F5Test
                TestResultDescription = "Failed Front Right DN F4F5 Test"
            Case eTestResult.FailedFrontRightDN_F4F1Test
                TestResultDescription = "Failed Front Right DN F4F1 Test"
            Case eTestResult.FailedFrontRightDN_ElectricalTest
                TestResultDescription = "Failed Front Right DN Electrical Test"
            Case eTestResult.FailedFrontRightDN_StrenghtTest
                TestResultDescription = "Failed Front Right DN Strenght Test"
            Case eTestResult.FailedFrontRightDN_PeakF4Test
                TestResultDescription = "Failed Front Right DN PeakF4 Test"


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


    Public Function Save(ByVal path As String, ByVal Result As cWS03Results) As Boolean
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
                                 vbTab & mWS03Main.TestModeDescription(mWS03Main.TestMode) &
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
            fileWriter.WriteLine("Test Cycle Time :" & mWS03Main.TestLast)
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
            Save = Save Or SaveSingleTestResult(fileWriter, eFront_Left_Push_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFront_Left_Push_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFront_Left_Pull_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFront_Left_Pull_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFront_Right_Push_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFront_Right_Push_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFront_Right_Pull_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eFront_Right_Pull_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eChildren_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eChildren_Strenght, groupName)
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

            'Read Valeo Serial Number 
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Valeo Serial NumberTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            groupName = ""
            Save = Save Or SaveResultValue(fileWriter, Valeo_Serial_Number, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Test_Byte_Before, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Test_Byte_After, groupName)
            fileWriter.WriteLine("")

            groupName = "FR_Push"
            'Push
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Front Right Push Test RESULTS                                    ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_XCe1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_XCe2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xe(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce2 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("")

            groupName = "FR_Pull"
            'Pull
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Front Right Pull Test RESULTS                                    ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_XCe1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_XCe2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xe(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce2 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Push_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Right_Pull_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontRight), groupName)
            fileWriter.WriteLine("")

            groupName = "FL_Push"
            'Push
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Front Left Push Test RESULTS                                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_XCe1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_XCe2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xe(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs1(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs2(mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Jama_Down(mGlobal.eElec.Off, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Jama_Down(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce2 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Jama_Down(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Push, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("")

            groupName = "FL_Pull"
            'Pull
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Front Left Pull Test RESULTS                                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_XCe1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_XCe2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xe(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs1(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Xs2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_dXs2(mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Manual(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Automatic(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Jama_Down(mGlobal.eElec.Off, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Manual(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Automatic(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Jama_Down(mGlobal.eElec.Ce1, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce2 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Push_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Manual(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Pull_Automatic(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            Save = Save Or SaveResultValue(fileWriter, WL_Front_Left_Jama_Down(mGlobal.eElec.Ce2, mGlobal.ePush_Pull.Pull, mWS03Main.eWindows.FrontLeft), groupName)
            fileWriter.WriteLine("")

            groupName = "CL"
            'Pull
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Child Lock Push Test RESULTS                                  ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, CL_XCe1, groupName)
            Save = Save Or SaveResultValue(fileWriter, CL_Xe, groupName)
            Save = Save Or SaveResultValue(fileWriter, CL_DiffS2Ce1, groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, CL_Fs1_F1, groupName)
            Save = Save Or SaveResultValue(fileWriter, CL_Xs1, groupName)
            Save = Save Or SaveResultValue(fileWriter, CL_dFs1_Haptic_1, groupName)
            Save = Save Or SaveResultValue(fileWriter, CL_dXs1, groupName)
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_CL_Electric(mGlobal.eElec.Off), groupName)

            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_CL_Electric(mGlobal.eElec.Ce1), groupName)

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
                Select Case mWS03Main.TestMode
                    Case mWS03Main.eTestMode.Remote
                        tempConvertCode = TestMode.MassProduction
                    Case mWS03Main.eTestMode.Master
                        tempConvertCode = TestMode.DailyCheck
                    Case Else
                        tempConvertCode = TestMode.Unknown
                End Select
                XMLResult.CreateXML_Unit(3, PartUniqueNumber.Value.ToString, tempUnitStatus, 0, CDate(TestDate.StringValue & " " & TestTime.StringValue),
                                Now, CInt(IIf(mWS03Main.TestLast > 30000, 9999, mWS03Main.TestLast)), 0, "", tempConvertCode)
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



    Private Function LoadSingleTestResult(ByVal fileReader As StreamReader,
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
