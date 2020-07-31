Option Explicit On
Option Strict On

Imports System
Imports System.IO

Public Class cWS05Results
    '+------------------------------------------------------------------------------+
    '|                             Public deMFarations                              |
    '+------------------------------------------------------------------------------+
    Public Enum eTestResult
        Unknown = 5000
        Passed = 0
        FailedPowerUP = 5001
        FailedInitCommunication = 5002
        FailedOpenDIAGonLINSession = 5003
        FailedCheckSerialNumber = 5004
        FailedCheckMMSTestByte = 5005

        FailedMirrorUP_Ce1Test = 5010
        FailedMirrorUP_OverStrokeTest = 5011
        FailedMirrorUP_PeakF1Test = 5012
        FailedMirrorUP_F1F2Test = 5013
        FailedMirrorUP_ElectricalTest = 5014
        FailedMirrorUP_StrenghtTest = 5015

        FailedMirrorDN_Ce1Test = 5020
        FailedMirrorDN_OverStrokeTest = 5021
        FailedMirrorDN_PeakF1Test = 5022
        FailedMirrorDN_F1F2Test = 5023
        FailedMirrorDN_ElectricalTest = 5024
        FailedMirrorDN_StrenghtTest = 5025

        FailedMirrorMR_Ce1Test = 5030
        FailedMirrorMR_OverStrokeTest = 5031
        FailedMirrorMR_PeakF1Test = 5032
        FailedMirrorMR_F1F2Test = 5033
        FailedMirrorMR_ElectricalTest = 5034
        FailedMirrorMR_StrenghtTest = 5035

        FailedMirrorML_Ce1Test = 5040
        FailedMirrorML_OverStrokeTest = 5041
        FailedMirrorML_PeakF1Test = 5042
        FailedMirrorML_F1F2Test = 5043
        FailedMirrorML_ElectricalTest = 5044
        FailedMirrorML_StrenghtTest = 5045

        FailedMirrorSR_Ce1Test = 5050
        FailedMirrorSR_OverStrokeTest = 5051
        FailedMirrorSR_PeakF1Test = 5052
        FailedMirrorSR_F1F2Test = 5053
        FailedMirrorSR_ElectricalTest = 5054
        FailedMirrorSR_StrenghtTest = 5055

        FailedMirrorSL_Ce1Test = 5060
        FailedMirrorSL_OverStrokeTest = 5061
        FailedMirrorSL_PeakF1Test = 5062
        FailedMirrorSL_F1F2Test = 5063
        FailedMirrorSL_ElectricalTest = 5064
        FailedMirrorSL_StrenghtTest = 5065

        FailedFinalStateProduct = 5070

        FailedWriteTestByte = 5071
        FailedReadTestByte = 5072

        FailedAbortTest = 5095
        FailedPartType = 5096
        FailedRuntimeError = 5097
        FailedMasterReference = 5098
        NotTested = 5099
    End Enum
    Public Enum eTestResultCN
        未知 = 5000
        OK = 0
        上电不良 = 5001
        初始化LIN通信 = 5002
        打开LIN诊断会话 = 5003
        检查序列号 = 5004
        检查MMS测试字段 = 5005

        MirrorUP_导通1 = 5010
        MirrorUP_满行程 = 5011
        MirrorUP_PeakF1 = 5012
        MirrorUP_F1F2 = 5013
        MirrorUP_电性能 = 5014
        MirrorUP_FS = 5015

        MirrorDN_导通1 = 5020
        MirrorDN_满行程 = 5021
        MirrorDN_PeakF1 = 5022
        MirrorDN_F1F2 = 5023
        MirrorDN_电性能 = 5024
        MirrorDN_FS = 5025

        MirrorMR_导通1 = 5030
        MirrorMR_满行程 = 5031
        MirrorMR_PeakF1 = 5032
        MirrorMR_F1F2 = 5033
        MirrorMR_电性能 = 5034
        MirrorMR_FS = 5035

        MirrorML_导通1 = 5040
        MirrorML_满行程 = 5041
        MirrorML_PeakF1 = 5042
        MirrorML_F1F2 = 5043
        MirrorML_电性能 = 5044
        MirrorML_FS = 5045

        MirrorSR_导通1 = 5050
        MirrorSR_满行程 = 5051
        MirrorSR_PeakF1 = 5052
        MirrorSR_F1F2 = 5053
        MirrorSR_电性能 = 5054
        MirrorSR_FS = 5055

        MirrorSL_导通1 = 5060
        MirrorSL_满行程 = 5061
        MirrorSL_PeakF1 = 5062
        MirrorSL_F1F2 = 5063
        MirrorSL_电性能 = 5064
        MirrorSL_FS = 5065

        最终产品状态 = 5070

        写测试状态字段 = 5071
        读测试字段 = 5072

        中断测试 = 5095
        产品类型 = 5096
        运行错误 = 5097
        MasterReference = 5098
        未测试 = 5099
    End Enum

    Public Enum eSingleTestResult
        NotTested = 0
        Disabled = 1
        Unknown = 2
        Passed = 3
        Failed = 4
        NotCoherent = 5
    End Enum

    Public Enum eMirrorPushTest
        Mirror_UP = 0
        Mirror_DN = 1
        Mirror_MR = 2
        Mirror_ML = 3
        Mirror_SR = 4
        Mirror_SL = 5

        Not_Defined = 6
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

    Public Mirror_X_Indexes(0 To 5) As sForceIndex


    ' Public constants
    Public Const SingleTestBaseIndex = 20
    Public Const SingleTestCount = 19
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

    Public ePUSH_MirrorUP_Electrical As cResultValue
    Public ePUSH_MirrorUP_Strenght As cResultValue
    Public ePUSH_MirrorDN_Electrical As cResultValue
    Public ePUSH_MirrorDN_Strenght As cResultValue
    Public ePUSH_MirrorMR_Electrical As cResultValue
    Public ePUSH_MirrorMR_Strenght As cResultValue
    Public ePUSH_MirrorML_Electrical As cResultValue
    Public ePUSH_MirrorML_Strenght As cResultValue
    Public ePUSH_MirrorSR_Electrical As cResultValue
    Public ePUSH_MirrorSR_Strenght As cResultValue
    Public ePUSH_MirrorSL_Electrical As cResultValue
    Public ePUSH_MirrorSL_Strenght As cResultValue

    Public eFINAL_STATE_PRODUCT As cResultValue
    Public eWrite_MMSTestByte As cResultValue
    Public eRead_MMSTestByte As cResultValue

    Public Power_supply_voltage As cResultValue
    Public Power_supply_Normal_Current As cResultValue

    Public Valeo_Serial_Number As cResultValue
    Public MMS_Test_Byte_Before As cResultValue
    Public MMS_Test_Byte_After As cResultValue

    ' Tableau
    ' Electric OFF/Ce1 ,UP/DN/MR/ML/SR/SL
    Public Mirror_Push_Electric(0 To 1, 0 To 5) As cResultValue
    ' Haptic
    Public Mirror_Fs1_F1(0 To 5) As cResultValue
    Public Mirror_Xs1(0 To 5) As cResultValue
    Public Mirror_dFs1_Haptic_1(0 To 5) As cResultValue
    Public Mirror_dXs1(0 To 5) As cResultValue
    Public Mirror_FsCe1(0 To 5) As cResultValue
    Public Mirror_XCe1(0 To 5) As cResultValue
    Public Mirror_Fe(0 To 5) As cResultValue
    Public Mirror_Xe(0 To 5) As cResultValue
    Public Mirror_DiffS2Ce1(0 To 5) As cResultValue

    Public FinalState(0 To 27) As cResultValue

    Public InitialState(0 To 5) As cResultValue
    '+------------------------------------------------------------------------------+
    '|                             Private deMFarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Public _values(0 To ValueCount - 1) As cResultValue

    ' Mirror Push 1 to  6
    Public SampleCount(0 To 5) As Integer
    Public Offset_Distance(0 To 5) As Single
    ' Signal EarlySensor, StrenghtSensor, Contact
    ''' <summary>
    ''' 1: force,stroke,switchings.2:0-7:knobs.3:points;
    ''' </summary>
    Public Sample(0 To 7, 0 To 5, 0 To cWS05Results.MaxSampleCount) As Single
    Public SampleTimeStamp(0 To 7, 0 To 5, 0 To cWS05Results.MaxSampleCount) As String

    Public Sample_TMP_AllKnob(5)(,) As Single
    Public Sample_TMP_AllKnobCount(5) As Single

    Public Tps_SignalLin_Start(0, 1) As Single
    Public Tps_SignalLin_Stop(0, 1) As Single
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
        ePUSH_MirrorUP_Electrical = _values(i) : i = i + 1
        ePUSH_MirrorUP_Strenght = _values(i) : i = i + 1
        ePUSH_MirrorDN_Electrical = _values(i) : i = i + 1
        ePUSH_MirrorDN_Strenght = _values(i) : i = i + 1
        ePUSH_MirrorMR_Electrical = _values(i) : i = i + 1
        ePUSH_MirrorMR_Strenght = _values(i) : i = i + 1
        ePUSH_MirrorML_Electrical = _values(i) : i = i + 1
        ePUSH_MirrorML_Strenght = _values(i) : i = i + 1
        ePUSH_MirrorSR_Electrical = _values(i) : i = i + 1
        ePUSH_MirrorSR_Strenght = _values(i) : i = i + 1
        ePUSH_MirrorSL_Electrical = _values(i) : i = i + 1
        ePUSH_MirrorSL_Strenght = _values(i) : i = i + 1
        eFINAL_STATE_PRODUCT = _values(i) : i = i + 1
        eWrite_MMSTestByte = _values(i) : i = i + 1
        eRead_MMSTestByte = _values(i) : i = i + 1

        Power_supply_voltage = _values(50)
        Power_supply_Normal_Current = _values(51)

        'Read Valeo Serial Number 
        Valeo_Serial_Number = _values(80)
        MMS_Test_Byte_Before = _values(81)
        MMS_Test_Byte_After = _values(82)

        ' Electric OFF/Ce1 ,Electric, PushMirror
        'Electric
        For i = 0 To 5
            For j = 0 To 1
                Mirror_Push_Electric(j, i) = _values(100 + (10 * j) + (i * 50))
            Next j
            ' Haptic
        Next i

        i = 410
        For j = 0 To 5
            Mirror_Fs1_F1(j) = _values(i) : i += 1
            Mirror_Xs1(j) = _values(i) : i += 1
            Mirror_dFs1_Haptic_1(j) = _values(i) : i += 1
            Mirror_dXs1(j) = _values(i) : i += 1
            Mirror_FsCe1(j) = _values(i) : i += 1
            Mirror_XCe1(j) = _values(i) : i += 1
            Mirror_Fe(j) = _values(i) : i += 1
            Mirror_Xe(j) = _values(i) : i += 1
            Mirror_DiffS2Ce1(j) = _values(i) : i += 1
        Next

        For i = 0 To 27
            FinalState(i) = _values(490 + i)
        Next

        For j = mWS05Main.eMirrorPush.MirrorUP To mWS05Main.eMirrorPush.MirrorSL
            InitialState(j) = _values(464 + j)
        Next

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

            Case eTestResult.FailedMirrorUP_Ce1Test
                TestResultDescription = "Failed Mirror UP Ce1 Test"
            Case eTestResult.FailedMirrorUP_OverStrokeTest
                TestResultDescription = "Failed Mirror UP Overstroke Test"
            Case eTestResult.FailedMirrorUP_PeakF1Test
                TestResultDescription = "Failed Mirror UP PeakF1 Test"
            Case eTestResult.FailedMirrorUP_F1F2Test
                TestResultDescription = "Failed Mirror UP F1F2 Test"
            Case eTestResult.FailedMirrorUP_ElectricalTest
                TestResultDescription = "Failed Mirror UP Electrical Test"
            Case eTestResult.FailedMirrorUP_StrenghtTest
                TestResultDescription = "Failed Mirror UP Strenght Test"

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
        Dim i As Integer
        Dim j As Integer
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
                                 vbTab & mWS05Main.TestModeDescription(mWS05Main.TestMode) &
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
            fileWriter.WriteLine("Test Cycle Time :" & mWS05Main.TestLast)
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
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorUP_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorUP_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorDN_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorDN_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorMR_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorMR_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorML_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorML_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorSR_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorSR_Strenght, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorSL_Electrical, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePUSH_MirrorSL_Strenght, groupName)
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

            For j = 0 To 5
                'Push
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                If j = 0 Then
                    groupName = "Up"
                    fileWriter.WriteLine("                  Push Mirror UP Test RESULTS                                    ")
                ElseIf j = 1 Then
                    groupName = "Dn"
                    fileWriter.WriteLine("                  Push Mirror DN Test RESULTS                                    ")
                ElseIf j = 2 Then
                    groupName = "Right"
                    fileWriter.WriteLine("                  Push Mirror Right Test RESULTS                                 ")
                ElseIf j = 3 Then
                    groupName = "Left"
                    fileWriter.WriteLine("                  Push Mirror Left Test RESULTS                                  ")
                ElseIf j = 4 Then
                    groupName = "SelectRight"
                    fileWriter.WriteLine("                  Push Mirror Select Right Test RESULTS                          ")
                ElseIf j = 5 Then
                    groupName = "SelectLeft"
                    fileWriter.WriteLine("                  Push Mirror Select Left Test RESULTS                           ")
                End If
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                fileWriter.WriteLine("                  Eraly Test RESULTS                                               ")
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                Save = Save Or SaveResultValue(fileWriter, Mirror_XCe1(eMirrorPushTest.Mirror_UP + j), groupName)
                Save = Save Or SaveResultValue(fileWriter, Mirror_Xe(eMirrorPushTest.Mirror_UP + j), groupName)
                Save = Save Or SaveResultValue(fileWriter, Mirror_DiffS2Ce1(eMirrorPushTest.Mirror_UP + j), groupName)
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                fileWriter.WriteLine("                  Strenght Test RESULTS                                            ")
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                Save = Save Or SaveResultValue(fileWriter, Mirror_Fs1_F1(eMirrorPushTest.Mirror_UP + j), groupName)
                Save = Save Or SaveResultValue(fileWriter, Mirror_Xs1(eMirrorPushTest.Mirror_UP + j), groupName)
                Save = Save Or SaveResultValue(fileWriter, Mirror_dFs1_Haptic_1(eMirrorPushTest.Mirror_UP + j), groupName)
                Save = Save Or SaveResultValue(fileWriter, Mirror_dXs1(eMirrorPushTest.Mirror_UP + j), groupName)
                Save = Save Or SaveResultValue(fileWriter, Mirror_Fe(eMirrorPushTest.Mirror_UP + j), groupName)
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                fileWriter.WriteLine("                  Electric Before Communtation Test RESULTS                        ")
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                Save = Save Or SaveResultValue(fileWriter, Mirror_Push_Electric(0, eMirrorPushTest.Mirror_UP + j), groupName)
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                fileWriter.WriteLine("                  Electric After Communtation Ce1 Test RESULTS                     ")
                fileWriter.WriteLine("-----------------------------------------------------------------------------------")
                Save = Save Or SaveResultValue(fileWriter, Mirror_Push_Electric(1, eMirrorPushTest.Mirror_UP + j), groupName)
                fileWriter.WriteLine("")
            Next

            groupName = "Final"
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Final Position TEST RESULTS                                      ")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            For index = 0 To 27
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
                Select Case mWS05Main.TestMode
                    Case mWS05Main.eTestMode.Remote
                        tempConvertCode = TestMode.MassProduction
                    Case mWS05Main.eTestMode.Analyse
                        tempConvertCode = TestMode.DailyCheck
                    Case Else
                        tempConvertCode = TestMode.Unknown
                End Select
                XMLResult.CreateXML_Unit(5, PartUniqueNumber.Value.ToString, tempUnitStatus, 0, CDate(TestDate.StringValue & " " & TestTime.StringValue),
                                Now, CInt(IIf(mWS05Main.TestLast > 30000, 9999, mWS05Main.TestLast)), 0, "", tempConvertCode)
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
                With XMLResult
                    .CreateMeasurementsSaveSingleTestResultForUnit(value, groupName)
                End With
            Catch ex As Exception

            End Try
            SaveSingleTestResult = False
        Catch ex As Exception
            SaveSingleTestResult = True
        End Try
    End Function
End Class
