Option Explicit On
Option Strict On

Imports System
Imports System.IO

Public Class cWS02Results

    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    Public Enum eTestResult
        Unknown = 2000
        Passed = 0
        FailedPowerUP = 2001
        FailedInitROOFLINCommunication = 2002
        FailedOpenDIAGonLINSession = 2003
        FailedInitiateStateProduct = 2004
        FailedCheckSerialNumber = 2005

        FailedNormalModeCurrent = 2006
        FailedEMSTraceability = 2007
        FailedAnalogInput = 2008
        FailedeValeo_Inter_Test_Byte = 2009
        FailedPush1_Shape = 2010
        FailedPush2_Shape = 2011
        FailedPush3_Shape = 2012
        FailedPush4_Shape = 2013
        FailedPush5_Shape = 2014
        FailedPush6_Shape = 2015
        FailedPush7_Shape = 2016
        FailedPush8_Shape = 2017
        FailedPush9_Shape = 2018
        FailedPush10_Shape = 2019
        FailedPush11_Shape = 2020
        FailedPush12_Shape = 2021
        FailedPush13_Shape = 2022
        FailedeDigitalOutput = 2023
        FailedeAdapter = 2024
        FailedePointOnWiLi = 2025
        FailedeCustomerInterface = 2026
        FailedeRingRGB = 2027

        FailedPush1_Backlight = 2030
        FailedPush2_Backlight = 2031
        FailedPush3_Backlight = 2032
        FailedPush4_Backlight = 2033
        FailedPush5_Backlight = 2034
        FailedPush6_Backlight = 2035
        FailedPush7_Backlight = 2036
        FailedPush8_Backlight = 2037
        FailedPush9_Backlight = 2038
        FailedPush10_Backlight = 2039
        FailedPush11_Backlight = 2040
        FailedPush12_Backlight = 2041
        FailedPush13_Backlight = 2042

        SL_Telltale_Intensity = 2050
        SR_Telltale_Intensity = 2051
        CL_Telltale_Intensity = 2052

        SL_Telltale_Defect = 2053
        SR_Telltale_Defect = 2054
        CL_Telltale_Defect = 2055

        FailedDecorFrame = 2058
        FailedBezel = 2059

        FailedPush1_Homogeneity = 2060
        FailedPush2_Homogeneity = 2061
        FailedPush3_Homogeneity = 2062
        FailedPush4_Homogeneity = 2063
        FailedPush5_Homogeneity = 2064
        FailedPush6_Homogeneity = 2065
        FailedPush7_Homogeneity = 2066
        FailedPush8_Homogeneity = 2067
        FailedPush9_Homogeneity = 2068
        FailedPush10_Homogeneity = 2069
        FailedPush11_Homogeneity = 2070
        FailedPush12_Homogeneity = 2071
        FailedPush13_Homogeneity = 2072

        FailedeWrite_TraceabilityMMS = 2080
        FailedeWrite_TraceabilityCustomer = 2081
        FailedeRead_ChecksumValue = 2082
        FailedeResetEcu_AfterWriting = 2083
        FailedeRead_TraceabilityMMS = 2084
        FailedeRead_TraceabilityCustomer = 2085
        FailedeWriteRead_IntertestByte = 2086
        FailedeSleepModeCurrent = 2087

        FailedAbortTest = 2090
        FailedPartType = 2091
        FailedRuntimeError = 2092
        FailedMasterReference = 2093
        FailedOthers = 2095
        NotTested = 2099
    End Enum

    Public Enum eTestResultCN
        未知 = 2000
        OK = 0
        上电不良 = 2001
        初始化LIN通信 = 2002
        打开LIN诊断会话 = 2003
        初始化产品状态 = 2004
        检查序列号 = 2005

        正常模式电流 = 2006
        EMS追溯 = 2007
        模拟输入 = 2008
        法雷奥测试状态字段 = 2009
        MirrorSelectLeft图案 = 2010
        MirrorSelectRight图案 = 2011
        Folding图案 = 2012
        MirrorUp图案 = 2013
        MirrorDn图案 = 2014
        MirrorLeft图案 = 2015
        MirrorRight图案 = 2016
        FrontLeft图案 = 2017
        FrontRight图案 = 2018
        RearLeft图案 = 2019
        RearRight图案 = 2020
        ChildLock图案 = 2021
        Push13图案 = 2022
        数字输出 = 2023
        Adapter = 2024
        车窗上小点 = 2025
        客户接口 = 2026
        圆环类型 = 2027

        MirrorSelectLeft背光 = 2030
        MirrorSelectRight背光 = 2031
        Folding背光 = 2032
        MirrorUp背光 = 2033
        MirrorDn背光 = 2034
        MirrorLeft背光 = 2035
        MirrorRight背光 = 2036
        FrontLeft背光 = 2037
        FrontRight背光 = 2038
        RearLeft背光 = 2039
        RearRight背光 = 2040
        ChildLock背光 = 2041
        Push13LED背光 = 2042

        MirrorSelectLeft指示灯亮度 = 2050
        MirrorSelectRight指示灯亮度 = 2051
        ChildLock指示灯亮度 = 2052
        SL指示灯缺陷 = 2053
        SR指示灯缺陷 = 2054
        CL指示灯缺陷 = 2055

        DecorFrame = 2058
        Bezel = 2059

        MirrorSelectLeft均一性 = 2060
        MirrorSelectRight均一性 = 2061
        Folding均一性 = 2062
        MirrorUp均一性 = 2063
        MirrorDn均一性 = 2064
        MirrorLeft均一性 = 2065
        MirrorRight均一性 = 2066
        FrontLeft均一性 = 2067
        FrontRight均一性 = 2068
        RearLeft均一性 = 2069
        RearRight均一性 = 2070
        ChildLock均一性 = 2071
        Push13均一性 = 2072

        写MMS追溯 = 2080
        写客户号码 = 2081
        读Checksum = 2082
        写完后复位Ecu = 2083
        读MMS追溯 = 2084
        读客户号 = 2085
        写测试状态字段 = 2086
        睡眠模式电流 = 2087

        中断测试 = 2090
        产品类型 = 2091
        运行错误 = 2092
        MasterReference = 2093
        其它 = 2095
        未测试 = 2099
    End Enum
    Public Enum eSingleTestResult
        NotTested = 0
        Disabled = 1
        Unknown = 2
        Passed = 3
        Failed = 4
        NotCoherent = 5
    End Enum

    ' Public constants
    Public Const SingleTestBaseIndex = 20
    Public Const SingleTestCount = 22
    Public Const ValueCount = 1500
    Public Const _StandardSignal = 5
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
    Public Side_Barcode As cResultValue
    Public FixtureID As cResultValue

    ' Single test results as cResultValue

    Public ePOWER_UP As cResultValue
    Public eINIT_LIN_COMMUNICATION As cResultValue
    Public eOPEN_DIAG_ON_LIN_SESSION As cResultValue
    Public eRead_MMS_Serial_Number As cResultValue
    Public eWriteConfiguration As cResultValue
    Public eNormalModeCurrent As cResultValue
    Public eEmsTraceability As cResultValue
    Public eAnalogInput As cResultValue
    Public eDigitalOutput As cResultValue
    Public ePWMOutput As cResultValue

    Public eShape_CONFORMITY As cResultValue
    'Public eBezel_CONFORMITY As cResultValue
    'Public eDecorFrame_CONFORMITY As cResultValue
    Public eAreaMirror_Conformity As cResultValue
    Public eAreaWili_Conformity As cResultValue
    Public eBACKLIGHT_CONFORMITY As cResultValue
    Public eBACKLIGHT_HOMOGENEITY As cResultValue
    Public eTELLTALE_CONFORMITY As cResultValue

    Public eWrite_Temperature_Set As cResultValue
    Public eWrite_TestByteMMS As cResultValue
    Public eWrite_TraceabilityMMS As cResultValue
    Public eResetEcu_AfterWriting As cResultValue
    Public eRead_TraceabilityMMS As cResultValue
    Public eRead_TestByteMMS As cResultValue
    '
    'Power UP
    Public PowerUp_VBAT As cResultValue
    Public PowerUp_Ibat As cResultValue
    Public SleepModeCurrent As cResultValue

    'Read Valeo Serial Number
    Public Valeo_Serial_Number As cResultValue
    Public Valeo_TestByte As cResultValue

#Region "Vision Shape"

    Public MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR As cResultValue
    Public MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR As cResultValue
    Public MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR As cResultValue
    Public MINIMUM_CONFORMITY_PUSH_ADJUST_UP As cResultValue
    Public MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN As cResultValue
    Public MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT As cResultValue
    Public MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT As cResultValue
    Public MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT As cResultValue
    Public MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT As cResultValue
    Public MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT As cResultValue
    Public MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT As cResultValue
    Public MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK As cResultValue
    Public MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2 As cResultValue
    Public CONFORMITY_Bezel As cResultValue
    Public CONFORMITY_DecorFrame As cResultValue
    Public CONFORMITY_Adatper_Front As cResultValue
    Public CONFORMITY_Adatper_Rear As cResultValue

    Public DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR As cResultValue
    Public DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR As cResultValue
    Public DEFECT_AREA_PUSH_FOLDING_MIRROR As cResultValue
    Public DEFECT_AREA_PUSH_ADJUST_UP As cResultValue
    Public DEFECT_AREA_PUSH_ADJUST_DOWN As cResultValue
    Public DEFECT_AREA_PUSH_ADJUST_LEFT As cResultValue
    Public DEFECT_AREA_PUSH_ADJUST_RIGHT As cResultValue
    Public DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT As cResultValue
    Public DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT As cResultValue
    Public DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT As cResultValue
    Public DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT As cResultValue
    Public DEFECT_AREA_PUSH_CHILDREN_LOCK As cResultValue
    Public DEFECT_AREA_PUSH_CHILDREN_LOCK2 As cResultValue
    Public Ring_Red As cResultValue
    Public Ring_Green As cResultValue
    Public Ring_Blue As cResultValue

    Public Points_On_Front_Left As cResultValue
    Public Points_On_Front_Right As cResultValue
    Public Points_On_Rear_Left As cResultValue
    Public Points_On_Rear_Right As cResultValue

    Public Customer_Interface(19) As cResultValue

#End Region

#Region "Vision_Backlight"

    Public Push_SELECT_LEFT_backlight_intensity_Camera As cResultValue
    Public Push_SELECT_LEFT_backlight_red As cResultValue
    Public Push_SELECT_LEFT_backlight_green As cResultValue
    Public Push_SELECT_LEFT_backlight_blue As cResultValue
    Public Push_SELECT_LEFT_backlight_Saturation As cResultValue
    Public Push_SELECT_LEFT_backlight_DominantWavelenght As cResultValue
    Public Push_SELECT_LEFT_Backlight_x As cResultValue
    Public Push_SELECT_LEFT_Backlight_y As cResultValue
    Public Push_SELECT_LEFT_backlight_intensity As cResultValue
    Public Push_SELECT_LEFT_Backlight_RSQ As cResultValue
    Public Push_SELECT_LEFT_backlight_homogeneity As cResultValue
    Public Push_SELECT_LEFT_Backlight_Polygon_Axy As cResultValue
    Public Push_SELECT_LEFT_Backlight_Polygon_Bxy As cResultValue
    Public Push_SELECT_LEFT_Backlight_Polygon_Cxy As cResultValue
    Public Push_SELECT_LEFT_Backlight_Polygon_Dxy As cResultValue
    Public Push_SELECT_LEFT_Backlight_Polygon_Exy As cResultValue
    Public Push_SELECT_LEFT_Backlight_Polygon_Fxy As cResultValue

    Public Push_SELECT_RIGHT_backlight_intensity_Camera As cResultValue
    Public Push_SELECT_RIGHT_backlight_red As cResultValue
    Public Push_SELECT_RIGHT_backlight_green As cResultValue
    Public Push_SELECT_RIGHT_backlight_blue As cResultValue
    Public Push_SELECT_RIGHT_backlight_Saturation As cResultValue
    Public Push_SELECT_RIGHT_backlight_DominantWavelenght As cResultValue
    Public Push_SELECT_RIGHT_Backlight_x As cResultValue
    Public Push_SELECT_RIGHT_Backlight_y As cResultValue
    Public Push_SELECT_RIGHT_backlight_intensity As cResultValue
    Public Push_SELECT_RIGHT_Backlight_RSQ As cResultValue
    Public Push_SELECT_RIGHT_backlight_homogeneity As cResultValue
    Public Push_SELECT_RIGHT_Backlight_Polygon_Axy As cResultValue
    Public Push_SELECT_RIGHT_Backlight_Polygon_Bxy As cResultValue
    Public Push_SELECT_RIGHT_Backlight_Polygon_Cxy As cResultValue
    Public Push_SELECT_RIGHT_Backlight_Polygon_Dxy As cResultValue
    Public Push_SELECT_RIGHT_Backlight_Polygon_Exy As cResultValue
    Public Push_SELECT_RIGHT_Backlight_Polygon_Fxy As cResultValue

    Public Push_FOLDING_backlight_intensity_Camera As cResultValue
    Public Push_FOLDING_backlight_red As cResultValue
    Public Push_FOLDING_backlight_green As cResultValue
    Public Push_FOLDING_backlight_blue As cResultValue
    Public Push_FOLDING_backlight_Saturation As cResultValue
    Public Push_FOLDING_backlight_DominantWavelenght As cResultValue
    Public Push_FOLDING_Backlight_x As cResultValue
    Public Push_FOLDING_Backlight_y As cResultValue
    Public Push_FOLDING_backlight_intensity As cResultValue
    Public Push_FOLDING_Backlight_RSQ As cResultValue
    Public Push_FOLDING_backlight_homogeneity As cResultValue
    Public Push_FOLDING_Backlight_Polygon_Axy As cResultValue
    Public Push_FOLDING_Backlight_Polygon_Bxy As cResultValue
    Public Push_FOLDING_Backlight_Polygon_Cxy As cResultValue
    Public Push_FOLDING_Backlight_Polygon_Dxy As cResultValue
    Public Push_FOLDING_Backlight_Polygon_Exy As cResultValue
    Public Push_FOLDING_Backlight_Polygon_Fxy As cResultValue

    Public Push_ADJUST_UP_backlight_intensity_Camera As cResultValue
    Public Push_ADJUST_UP_backlight_red As cResultValue
    Public Push_ADJUST_UP_backlight_green As cResultValue
    Public Push_ADJUST_UP_backlight_blue As cResultValue
    Public Push_ADJUST_UP_backlight_Saturation As cResultValue
    Public Push_ADJUST_UP_backlight_DominantWavelenght As cResultValue
    Public Push_ADJUST_UP_Backlight_x As cResultValue
    Public Push_ADJUST_UP_Backlight_y As cResultValue
    Public Push_ADJUST_UP_backlight_intensity As cResultValue
    Public Push_ADJUST_UP_Backlight_RSQ As cResultValue
    Public Push_ADJUST_UP_backlight_homogeneity As cResultValue
    Public Push_ADJUST_UP_Backlight_Polygon_Axy As cResultValue
    Public Push_ADJUST_UP_Backlight_Polygon_Bxy As cResultValue
    Public Push_ADJUST_UP_Backlight_Polygon_Cxy As cResultValue
    Public Push_ADJUST_UP_Backlight_Polygon_Dxy As cResultValue
    Public Push_ADJUST_UP_Backlight_Polygon_Exy As cResultValue
    Public Push_ADJUST_UP_Backlight_Polygon_Fxy As cResultValue

    Public Push_ADJUST_DOWN_backlight_intensity_Camera As cResultValue
    Public Push_ADJUST_DOWN_backlight_red As cResultValue
    Public Push_ADJUST_DOWN_backlight_green As cResultValue
    Public Push_ADJUST_DOWN_backlight_blue As cResultValue
    Public Push_ADJUST_DOWN_backlight_Saturation As cResultValue
    Public Push_ADJUST_DOWN_backlight_DominantWavelenght As cResultValue
    Public Push_ADJUST_DOWN_Backlight_x As cResultValue
    Public Push_ADJUST_DOWN_Backlight_y As cResultValue
    Public Push_ADJUST_DOWN_backlight_intensity As cResultValue
    Public Push_ADJUST_DOWN_Backlight_RSQ As cResultValue
    Public Push_ADJUST_DOWN_backlight_homogeneity As cResultValue
    Public Push_ADJUST_DOWN_Backlight_Polygon_Axy As cResultValue
    Public Push_ADJUST_DOWN_Backlight_Polygon_Bxy As cResultValue
    Public Push_ADJUST_DOWN_Backlight_Polygon_Cxy As cResultValue
    Public Push_ADJUST_DOWN_Backlight_Polygon_Dxy As cResultValue
    Public Push_ADJUST_DOWN_Backlight_Polygon_Exy As cResultValue
    Public Push_ADJUST_DOWN_Backlight_Polygon_Fxy As cResultValue

    Public Push_ADJUST_LEFT_backlight_intensity_Camera As cResultValue
    Public Push_ADJUST_LEFT_backlight_red As cResultValue
    Public Push_ADJUST_LEFT_backlight_green As cResultValue
    Public Push_ADJUST_LEFT_backlight_blue As cResultValue
    Public Push_ADJUST_LEFT_backlight_Saturation As cResultValue
    Public Push_ADJUST_LEFT_backlight_DominantWavelenght As cResultValue
    Public Push_ADJUST_LEFT_Backlight_x As cResultValue
    Public Push_ADJUST_LEFT_Backlight_y As cResultValue
    Public Push_ADJUST_LEFT_backlight_intensity As cResultValue
    Public Push_ADJUST_LEFT_Backlight_RSQ As cResultValue
    Public Push_ADJUST_LEFT_backlight_homogeneity As cResultValue
    Public Push_ADJUST_LEFT_Backlight_Polygon_Axy As cResultValue
    Public Push_ADJUST_LEFT_Backlight_Polygon_Bxy As cResultValue
    Public Push_ADJUST_LEFT_Backlight_Polygon_Cxy As cResultValue
    Public Push_ADJUST_LEFT_Backlight_Polygon_Dxy As cResultValue
    Public Push_ADJUST_LEFT_Backlight_Polygon_Exy As cResultValue
    Public Push_ADJUST_LEFT_Backlight_Polygon_Fxy As cResultValue

    Public Push_ADJUST_RIGHT_backlight_intensity_Camera As cResultValue
    Public Push_ADJUST_RIGHT_backlight_red As cResultValue
    Public Push_ADJUST_RIGHT_backlight_green As cResultValue
    Public Push_ADJUST_RIGHT_backlight_blue As cResultValue
    Public Push_ADJUST_RIGHT_backlight_Saturation As cResultValue
    Public Push_ADJUST_RIGHT_backlight_DominantWavelenght As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_x As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_y As cResultValue
    Public Push_ADJUST_RIGHT_backlight_intensity As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_RSQ As cResultValue
    Public Push_ADJUST_RIGHT_backlight_homogeneity As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_Polygon_Axy As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_Polygon_Bxy As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_Polygon_Cxy As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_Polygon_Dxy As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_Polygon_Exy As cResultValue
    Public Push_ADJUST_RIGHT_Backlight_Polygon_Fxy As cResultValue

    Public WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity_Camera As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_backlight_red As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_backlight_green As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_backlight_blue As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_backlight_Saturation As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_backlight_DominantWavelenght As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_x As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_y As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy As cResultValue
    Public WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy As cResultValue

    Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity_Camera As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_red As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_green As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_Saturation As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_DominantWavelenght As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_x As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_y As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy As cResultValue
    Public WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy As cResultValue

    Public WINDOWS_LIFTER_REAR_LEFT_backlight_intensity_Camera As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_backlight_red As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_backlight_green As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_backlight_blue As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_backlight_Saturation As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_backlight_DominantWavelenght As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_x As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_y As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_backlight_intensity As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy As cResultValue
    Public WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy As cResultValue

    Public WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity_Camera As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_backlight_red As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_backlight_green As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_backlight_blue As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_backlight_Saturation As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_backlight_DominantWavelenght As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_x As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_y As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy As cResultValue
    Public WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy As cResultValue

    Public Push_CHILDREN_LOCK_backlight_intensity_Camera As cResultValue
    Public Push_CHILDREN_LOCK_backlight_red As cResultValue
    Public Push_CHILDREN_LOCK_backlight_green As cResultValue
    Public Push_CHILDREN_LOCK_backlight_blue As cResultValue
    Public Push_CHILDREN_LOCK_backlight_Saturation As cResultValue
    Public Push_CHILDREN_LOCK_backlight_DominantWavelenght As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_x As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_y As cResultValue
    Public Push_CHILDREN_LOCK_backlight_intensity As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_RSQ As cResultValue
    Public Push_CHILDREN_LOCK_backlight_homogeneity As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_Polygon_Axy As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_Polygon_Bxy As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_Polygon_Cxy As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_Polygon_Dxy As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_Polygon_Exy As cResultValue
    Public Push_CHILDREN_LOCK_Backlight_Polygon_Fxy As cResultValue

    Public Push_CHILDREN_LOCK2_backlight_intensity_Camera As cResultValue
    Public Push_CHILDREN_LOCK2_backlight_red As cResultValue
    Public Push_CHILDREN_LOCK2_backlight_green As cResultValue
    Public Push_CHILDREN_LOCK2_backlight_blue As cResultValue
    Public Push_CHILDREN_LOCK2_backlight_Saturation As cResultValue
    Public Push_CHILDREN_LOCK2_backlight_DominantWavelenght As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_x As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_y As cResultValue
    Public Push_CHILDREN_LOCK2_backlight_intensity As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_RSQ As cResultValue
    Public Push_CHILDREN_LOCK2_backlight_homogeneity As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_Polygon_Axy As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_Polygon_Bxy As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_Polygon_Cxy As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_Polygon_Dxy As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_Polygon_Exy As cResultValue
    Public Push_CHILDREN_LOCK2_Backlight_Polygon_Fxy As cResultValue

    Public DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR As cResultValue
    Public DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR As cResultValue
    Public DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR As cResultValue
    Public DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP As cResultValue
    Public DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN As cResultValue
    Public DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT As cResultValue
    Public DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT As cResultValue
    Public DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT As cResultValue
    Public DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT As cResultValue
    Public DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT As cResultValue
    Public DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT As cResultValue
    Public DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK As cResultValue
    Public DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2 As cResultValue

#End Region
#Region "Vision_LED"

    Public Push_SELECT_LEFT_LED_intensity As cResultValue
    Public Push_SELECT_LEFT_LED_red As cResultValue
    Public Push_SELECT_LEFT_LED_green As cResultValue
    Public Push_SELECT_LEFT_LED_blue As cResultValue
    Public Push_SELECT_LEFT_LED_saturation As cResultValue
    Public Push_SELECT_LEFT_LED_WaveLenght As cResultValue
    Public Push_SELECT_LEFT_LED_x As cResultValue
    Public Push_SELECT_LEFT_LED_y As cResultValue
    Public Push_SELECT_LEFT_LED_Polygon_Axy As cResultValue
    Public Push_SELECT_LEFT_LED_Polygon_Bxy As cResultValue
    Public Push_SELECT_LEFT_LED_Polygon_Cxy As cResultValue
    Public Push_SELECT_LEFT_LED_Polygon_Dxy As cResultValue

    Public Push_SELECT_RIGHT_LED_intensity As cResultValue
    Public Push_SELECT_RIGHT_LED_red As cResultValue
    Public Push_SELECT_RIGHT_LED_green As cResultValue
    Public Push_SELECT_RIGHT_LED_blue As cResultValue
    Public Push_SELECT_RIGHT_LED_saturation As cResultValue
    Public Push_SELECT_RIGHT_LED_WaveLenght As cResultValue
    Public Push_SELECT_RIGHT_LED_x As cResultValue
    Public Push_SELECT_RIGHT_LED_y As cResultValue
    Public Push_SELECT_RIGHT_LED_Polygon_Axy As cResultValue
    Public Push_SELECT_RIGHT_LED_Polygon_Bxy As cResultValue
    Public Push_SELECT_RIGHT_LED_Polygon_Cxy As cResultValue
    Public Push_SELECT_RIGHT_LED_Polygon_Dxy As cResultValue

    Public Push_CHILDREN_LOCK_LED_intensity As cResultValue
    Public Push_CHILDREN_LOCK_LED_red As cResultValue
    Public Push_CHILDREN_LOCK_LED_green As cResultValue
    Public Push_CHILDREN_LOCK_LED_blue As cResultValue
    Public Push_CHILDREN_LOCK_LED_saturation As cResultValue
    Public Push_CHILDREN_LOCK_LED_WaveLenght As cResultValue
    Public Push_CHILDREN_LOCK_LED_x As cResultValue
    Public Push_CHILDREN_LOCK_LED_y As cResultValue
    Public Push_CHILDREN_LOCK_LED_Polygon_Axy As cResultValue
    Public Push_CHILDREN_LOCK_LED_Polygon_Bxy As cResultValue
    Public Push_CHILDREN_LOCK_LED_Polygon_Cxy As cResultValue
    Public Push_CHILDREN_LOCK_LED_Polygon_Dxy As cResultValue

    Public DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR As cResultValue
    Public DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR As cResultValue
    Public DEFECT_AREA_TELLTALE_CHILDREN_LOCK As cResultValue

#End Region

    'Read Valeo Traceability MMS
    Public MMS_Read_Final_Product_Reference As cResultValue
    Public MMS_Read_Final_Product_Index As cResultValue
    Public MMS_Read_Valeo_Final_Product_Plant As cResultValue
    Public MMS_Read_Valeo_Final_Product_Line As cResultValue
    Public MMS_Read_Valeo_Final_Product_Manufacturing_Date As cResultValue
    Public MMS_Read_Valeo_Serial_Number As cResultValue
    Public MMS_Read_Deviation_Number As cResultValue
    Public MMS_Read_Test_Byte As cResultValue


    Public PCBA_Number_Reference As cResultValue
    Public PCBA_Number_Index As cResultValue
    Public PCBA_Plant_Line As cResultValue
    Public PCBA_ManufacturingDate As cResultValue
    Public PCBA_SerialNumber As cResultValue
    Public PCBA_DeviationNumber As cResultValue
    Public LED_BIN_PT_White_RSA As cResultValue
    Public LED_BIN_PT_RED As cResultValue
    Public LED_BIN_PT_YELLOW As cResultValue
    Public LED_BIN_PT_WHITE_NISSAN As cResultValue
    Public EMS_Test_Byte As cResultValue
    Public Major_SoftwareVersion As cResultValue
    Public Minor_SoftwareVersion As cResultValue
    Public Major_NVMversion As cResultValue
    Public Minor_NVMversion As cResultValue
    Public SW_checksum As cResultValue
    Public SW_Coding As cResultValue
    Public HW_Coding As cResultValue
    Public Backlight_Coding As cResultValue
    '
    Public ADC_UCAD_VARIANT_1 As cResultValue
    Public ADC_UCAD_VARIANT_2 As cResultValue
    '
    Public ADC_CURSEUR_UP_DN As cResultValue
    Public ADC_CURSEUR_LEFT_RIGHT As cResultValue
    '
    Public STsgn_Front_Passenger_Commande_UP(0 To 20, 0 To 1) As cResultValue
    Public STsgn_Front_Passenger_Commande_Down(0 To 20, 0 To 1) As cResultValue

    Public STsgn_Switches_Inhibition(0 To 20, 0 To 1) As cResultValue

    Public STsgn_Down_Rear_Right(0 To 20, 0 To 1) As cResultValue
    Public STsgn_UP_Rear_Right(0 To 20, 0 To 1) As cResultValue
    Public STsgn_Down_Rear_Left(0 To 20, 0 To 1) As cResultValue
    Public STsgn_UP_Rear_Left(0 To 20, 0 To 1) As cResultValue

    Public STsgn_CDE_HB_RTRV_G(0 To 20, 0 To 1) As cResultValue
    'Public ADC_CDE_HB_RTRV_G(0 To 1) As cResultValue

    Public STsgn_SGN_COMMUN_MOT_RTRV_D(0 To 20, 0 To 1) As cResultValue

    Public STsgn_CDE_DG_RTRV_D(0 To 20, 0 To 1) As cResultValue
    Public ADC_CDE_DG_RTRV_D(0 To 1) As cResultValue

    Public STsgn_CDE_DG_RTRV_G(0 To 20, 0 To 1) As cResultValue
    Public ADC_CDE_DG_RTRV_G(0 To 1) As cResultValue

    Public STsgn_CDE_RBT_RTRV_G(0 To 20, 0 To 1) As cResultValue
    Public STsgn_CDE_RBT_RTRV_D(0 To 20, 0 To 1) As cResultValue
    Public ADC_CDE_RBT_RTRV_G(0 To 1) As cResultValue
    Public ADC_CDE_RBT_RTRV_D(0 To 1) As cResultValue

    Public STsgn_SGN_COMMUN_MOT_RTRV_G(0 To 20, 0 To 1) As cResultValue

    Public External_Temp As cResultValue
    Public ADC_Temp As cResultValue

    Public UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12 As cResultValue
    Public UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12 As cResultValue
    Public UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12 As cResultValue
    Public UCDA_SINGLE_SW_DIAG_Frequency_X1_12 As cResultValue
    Public UCDA_SINGLE_SW_DIAG_RiseTime_X1_12 As cResultValue
    Public UCDA_SINGLE_SW_DIAG_FallTime_X1_12 As cResultValue
    Public UCDA_SINGLE_SW_DIAG As cResultValue

    Public UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24 As cResultValue
    Public UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24 As cResultValue
    Public UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24 As cResultValue
    Public UCDA_DOOR_LOCK_DIAG_Frequency_X1_24 As cResultValue
    Public UCDA_DOOR_LOCK_DIAG_RiseTime_X1_24 As cResultValue
    Public UCDA_DOOR_LOCK_DIAG_FallTime_X1_24 As cResultValue
    Public UCDA_DOOR_LOCK_DIAG As cResultValue


    'Standard Signal
    ' Front Passenger CDE
    'O_DOWN_FRONT_PASSENGER_CDE
    'O_UP_FRONT_PASSENGER_CDE
    ' Windows Lifter Commande	
    'O_UP_REAR_RIGHT_CDE
    'O_DOWN_REAR_RIGHT_CDE
    'O_UP_REAR_LEFT_CDE
    'O_DOWN_REAR_LEFT_CDE
    ' Jama Function	
    'O_JAMA_UP
    'O_JAMA_DOWN
    ' Mirror Function Right	
    'COMMUN_GND_OF_PTN_DRIVER_EXTERNAL_MIRROR
    'COMMUN_SUPPLY_+_OF_PTN_DRIVER_EXTERNAL_MIRROR
    'CDE_D/G_RTRV_D
    'CURSEUR_PTN_HT/BAS_RETRO_CDC
    'CURSEUR_PTN_GCHE/DRT_RETRO_CDC
    ' Mirror Folding Left	
    'CDE_+_RBT_RTRV_G
    'CDE_-_RBT_RTRV_G
    ' Mirror Folding Right	
    'CDE_+_RBT_RTRV_D
    'CDE_-_RBT_RTRV_D
    ' Inhibition	
    'O_LOCAL_WL_SWITCHES_INHIBITION_CDE
    ' Mirror Function Left	
    'CDE_H/B_RTRV_G
    'SGN_COMMUN_MOT_RTRV_G
    'CDE_D/G_RTRV_G

    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Public _values(0 To ValueCount - 1) As cResultValue
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
        Dim r As Integer
        Dim c As Integer

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
        Side_Barcode = _values(8)
        FixtureID = _values(9)
        i = 20
        ' Single test results
        ePOWER_UP = _values(i) : i = i + 1
        eINIT_LIN_COMMUNICATION = _values(i) : i = i + 1
        eOPEN_DIAG_ON_LIN_SESSION = _values(i) : i = i + 1
        eRead_MMS_Serial_Number = _values(i) : i = i + 1
        eWriteConfiguration = _values(i) : i = i + 1
        eNormalModeCurrent = _values(i) : i = i + 1
        eEmsTraceability = _values(i) : i = i + 1
        eAnalogInput = _values(i) : i = i + 1
        eDigitalOutput = _values(i) : i = i + 1
        ePWMOutput = _values(i) : i += 1
        eShape_CONFORMITY = _values(i) : i = i + 1
        eAreaMirror_Conformity = _values(i) : i = i + 1
        eAreaWili_Conformity = _values(i) : i = i + 1
        eBACKLIGHT_CONFORMITY = _values(i) : i = i + 1
        eBACKLIGHT_HOMOGENEITY = _values(i) : i = i + 1
        eTELLTALE_CONFORMITY = _values(i) : i = i + 1
        eWrite_Temperature_Set = _values(i) : i = i + 1
        eWrite_TraceabilityMMS = _values(i) : i = i + 1
        eWrite_TestByteMMS = _values(i) : i = i + 1
        eResetEcu_AfterWriting = _values(i) : i = i + 1
        eRead_TraceabilityMMS = _values(i) : i = i + 1
        eRead_TestByteMMS = _values(i) : i = i + 1

        'Power UP
        PowerUp_VBAT = _values(70)
        PowerUp_Ibat = _values(71)

        'Read Valeo Serial Number 
        Valeo_Serial_Number = _values(100)
        '
        i = 110
        MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_PUSH_ADJUST_UP = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK = _values(i) : i = i + 1
        CONFORMITY_Bezel = _values(i) : i = i + 1
        CONFORMITY_DecorFrame = _values(i) : i = i + 1
        MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2 = _values(i) : i = i + 1
        CONFORMITY_Adatper_Front = _values(i) : i = i + 1
        CONFORMITY_Adatper_Rear = _values(i) : i = i + 1

        i = 130
        DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR = _values(i) : i = i + 1
        DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR = _values(i) : i = i + 1
        DEFECT_AREA_PUSH_FOLDING_MIRROR = _values(i) : i = i + 1
        DEFECT_AREA_PUSH_ADJUST_UP = _values(i) : i = i + 1
        DEFECT_AREA_PUSH_ADJUST_DOWN = _values(i) : i = i + 1
        DEFECT_AREA_PUSH_ADJUST_LEFT = _values(i) : i = i + 1
        DEFECT_AREA_PUSH_ADJUST_RIGHT = _values(i) : i = i + 1
        DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT = _values(i) : i = i + 1
        DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT = _values(i) : i = i + 1
        DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT = _values(i) : i = i + 1
        DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT = _values(i) : i = i + 1
        DEFECT_AREA_PUSH_CHILDREN_LOCK = _values(i) : i = i + 1
        DEFECT_AREA_PUSH_CHILDREN_LOCK2 = _values(i) : i = i + 1
        Points_On_Front_Left = _values(i) : i = i + 1
        Points_On_Front_Right = _values(i) : i = i + 1
        Points_On_Rear_Left = _values(i) : i = i + 1
        Points_On_Rear_Right = _values(i) : i = i + 1
        Ring_Red = _values(i) : i = i + 1
        Ring_Green = _values(i) : i = i + 1
        Ring_Blue = _values(i) : i = i + 1


        i = 150
        Push_SELECT_LEFT_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_SELECT_LEFT_backlight_red = _values(i) : i = i + 1
        Push_SELECT_LEFT_backlight_green = _values(i) : i = i + 1
        Push_SELECT_LEFT_backlight_blue = _values(i) : i = i + 1
        Push_SELECT_LEFT_backlight_Saturation = _values(i) : i = i + 1
        Push_SELECT_LEFT_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_SELECT_LEFT_Backlight_x = _values(i) : i = i + 1
        Push_SELECT_LEFT_Backlight_y = _values(i) : i = i + 1
        Push_SELECT_LEFT_backlight_intensity = _values(i) : i = i + 1
        Push_SELECT_LEFT_Backlight_RSQ = _values(i) : i = i + 1
        Push_SELECT_LEFT_backlight_homogeneity = _values(i) : i = i + 1
        Push_SELECT_LEFT_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_SELECT_LEFT_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_SELECT_LEFT_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_SELECT_LEFT_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        Push_SELECT_RIGHT_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_SELECT_RIGHT_backlight_red = _values(i) : i = i + 1
        Push_SELECT_RIGHT_backlight_green = _values(i) : i = i + 1
        Push_SELECT_RIGHT_backlight_blue = _values(i) : i = i + 1
        Push_SELECT_RIGHT_backlight_Saturation = _values(i) : i = i + 1
        Push_SELECT_RIGHT_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_x = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_y = _values(i) : i = i + 1
        Push_SELECT_RIGHT_backlight_intensity = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_RSQ = _values(i) : i = i + 1
        Push_SELECT_RIGHT_backlight_homogeneity = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        Push_FOLDING_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_FOLDING_backlight_red = _values(i) : i = i + 1
        Push_FOLDING_backlight_green = _values(i) : i = i + 1
        Push_FOLDING_backlight_blue = _values(i) : i = i + 1
        Push_FOLDING_backlight_Saturation = _values(i) : i = i + 1
        Push_FOLDING_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_FOLDING_Backlight_x = _values(i) : i = i + 1
        Push_FOLDING_Backlight_y = _values(i) : i = i + 1
        Push_FOLDING_backlight_intensity = _values(i) : i = i + 1
        Push_FOLDING_Backlight_RSQ = _values(i) : i = i + 1
        Push_FOLDING_backlight_homogeneity = _values(i) : i = i + 1
        Push_FOLDING_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_FOLDING_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_FOLDING_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_FOLDING_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        Push_ADJUST_UP_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_ADJUST_UP_backlight_red = _values(i) : i = i + 1
        Push_ADJUST_UP_backlight_green = _values(i) : i = i + 1
        Push_ADJUST_UP_backlight_blue = _values(i) : i = i + 1
        Push_ADJUST_UP_backlight_Saturation = _values(i) : i = i + 1
        Push_ADJUST_UP_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_x = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_y = _values(i) : i = i + 1
        Push_ADJUST_UP_backlight_intensity = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_RSQ = _values(i) : i = i + 1
        Push_ADJUST_UP_backlight_homogeneity = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        Push_ADJUST_DOWN_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_ADJUST_DOWN_backlight_red = _values(i) : i = i + 1
        Push_ADJUST_DOWN_backlight_green = _values(i) : i = i + 1
        Push_ADJUST_DOWN_backlight_blue = _values(i) : i = i + 1
        Push_ADJUST_DOWN_backlight_Saturation = _values(i) : i = i + 1
        Push_ADJUST_DOWN_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_x = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_y = _values(i) : i = i + 1
        Push_ADJUST_DOWN_backlight_intensity = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_RSQ = _values(i) : i = i + 1
        Push_ADJUST_DOWN_backlight_homogeneity = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        Push_ADJUST_LEFT_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_ADJUST_LEFT_backlight_red = _values(i) : i = i + 1
        Push_ADJUST_LEFT_backlight_green = _values(i) : i = i + 1
        Push_ADJUST_LEFT_backlight_blue = _values(i) : i = i + 1
        Push_ADJUST_LEFT_backlight_Saturation = _values(i) : i = i + 1
        Push_ADJUST_LEFT_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_x = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_y = _values(i) : i = i + 1
        Push_ADJUST_LEFT_backlight_intensity = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_RSQ = _values(i) : i = i + 1
        Push_ADJUST_LEFT_backlight_homogeneity = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        Push_ADJUST_RIGHT_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_backlight_red = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_backlight_green = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_backlight_blue = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_backlight_Saturation = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_x = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_y = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_backlight_intensity = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_RSQ = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_backlight_homogeneity = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity_Camera = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_backlight_red = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_backlight_green = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_backlight_blue = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_backlight_Saturation = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_backlight_DominantWavelenght = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_x = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_y = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity_Camera = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_backlight_red = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_backlight_green = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_backlight_Saturation = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_backlight_DominantWavelenght = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_x = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_y = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        WINDOWS_LIFTER_REAR_LEFT_backlight_intensity_Camera = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_backlight_red = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_backlight_green = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_backlight_blue = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_backlight_Saturation = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_backlight_DominantWavelenght = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_x = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_y = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_backlight_intensity = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity_Camera = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_backlight_red = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_backlight_green = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_backlight_blue = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_backlight_Saturation = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_backlight_DominantWavelenght = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_x = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_y = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        Push_CHILDREN_LOCK_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_backlight_red = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_backlight_green = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_backlight_blue = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_backlight_Saturation = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_Backlight_x = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_Backlight_y = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_backlight_intensity = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_Backlight_RSQ = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_backlight_homogeneity = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_Backlight_Polygon_Dxy = _values(i) : i = i + 1

        i = 330
        DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK = _values(i) : i = i + 1
        DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2 = _values(i) : i = i + 1

        i = 350
        Push_SELECT_LEFT_LED_intensity = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_red = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_green = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_blue = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_saturation = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_WaveLenght = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_x = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_y = _values(i) : i = i + 1

        Push_SELECT_RIGHT_LED_intensity = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_red = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_green = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_blue = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_saturation = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_WaveLenght = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_x = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_y = _values(i) : i = i + 1

        Push_CHILDREN_LOCK_LED_intensity = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_red = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_green = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_blue = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_saturation = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_WaveLenght = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_x = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_y = _values(i) : i = i + 1

        i = 375
        DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR = _values(i) : i = i + 1
        DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR = _values(i) : i = i + 1
        DEFECT_AREA_TELLTALE_CHILDREN_LOCK = _values(i) : i = i + 1

        i = 380
        PCBA_Number_Reference = _values(i) : i = i + 1
        PCBA_Number_Index = _values(i) : i = i + 1
        PCBA_Plant_Line = _values(i) : i = i + 1
        PCBA_ManufacturingDate = _values(i) : i = i + 1
        PCBA_SerialNumber = _values(i) : i = i + 1
        PCBA_DeviationNumber = _values(i) : i = i + 1
        LED_BIN_PT_White_RSA = _values(i) : i = i + 1
        LED_BIN_PT_RED = _values(i) : i = i + 1
        LED_BIN_PT_YELLOW = _values(i) : i = i + 1
        LED_BIN_PT_WHITE_NISSAN = _values(i) : i = i + 1
        EMS_Test_Byte = _values(i) : i = i + 1

        Major_SoftwareVersion = _values(i) : i = i + 1
        Minor_SoftwareVersion = _values(i) : i = i + 1
        Major_NVMversion = _values(i) : i = i + 1
        Minor_NVMversion = _values(i) : i = i + 1
        SW_checksum = _values(i) : i = i + 1
        SW_Coding = _values(i) : i = i + 1
        HW_Coding = _values(i) : i = i + 1
        Backlight_Coding = _values(i) : i = i + 1

        i = 400
        ADC_UCAD_VARIANT_1 = _values(i) : i = i + 1
        ADC_UCAD_VARIANT_2 = _values(i) : i = i + 1

        ADC_CURSEUR_UP_DN = _values(i) : i = i + 1
        ADC_CURSEUR_LEFT_RIGHT = _values(i) : i = i + 1

        i = 420
        'Read Valeo Traceability MMS
        MMS_Read_Final_Product_Reference = _values(i) : i = i + 1
        MMS_Read_Final_Product_Index = _values(i) : i = i + 1
        MMS_Read_Valeo_Final_Product_Plant = _values(i) : i = i + 1
        MMS_Read_Valeo_Final_Product_Line = _values(i) : i = i + 1
        MMS_Read_Valeo_Final_Product_Manufacturing_Date = _values(i) : i = i + 1
        MMS_Read_Valeo_Serial_Number = _values(i) : i = i + 1
        MMS_Read_Deviation_Number = _values(i) : i = i + 1
        MMS_Read_Test_Byte = _values(i) : i = i + 1

        i = 430
        Push_CHILDREN_LOCK2_backlight_intensity_Camera = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_backlight_red = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_backlight_green = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_backlight_blue = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_backlight_Saturation = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_backlight_DominantWavelenght = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_x = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_y = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_backlight_intensity = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_RSQ = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_backlight_homogeneity = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_Polygon_Axy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_Polygon_Bxy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_Polygon_Cxy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_Polygon_Dxy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK2_Backlight_Polygon_Fxy = _values(i) : i = i + 1

        '
        For c = 0 To 1
            If c = 0 Then i = 500
            If c = 1 Then i = 530
            For r = 0 To 20
                STsgn_Front_Passenger_Commande_UP(r, c) = _values(i) : i = i + 1
            Next
        Next
        For c = 0 To 1
            If c = 0 Then i = 560
            If c = 1 Then i = 590
            For r = 0 To 20
                STsgn_Front_Passenger_Commande_Down(r, c) = _values(i) : i = i + 1
            Next
        Next

        For c = 0 To 1
            If c = 0 Then i = 620
            If c = 1 Then i = 650
            For r = 0 To 20
                STsgn_Switches_Inhibition(r, c) = _values(i) : i = i + 1
            Next
        Next

        For c = 0 To 1
            If c = 0 Then i = 680
            If c = 1 Then i = 710
            For r = 0 To 20
                STsgn_Down_Rear_Right(r, c) = _values(i) : i = i + 1
            Next
        Next

        For c = 0 To 1
            If c = 0 Then i = 740
            If c = 1 Then i = 770
            For r = 0 To 20
                STsgn_UP_Rear_Right(r, c) = _values(i) : i = i + 1
            Next
        Next

        For c = 0 To 1
            If c = 0 Then i = 800
            If c = 1 Then i = 830
            For r = 0 To 20
                STsgn_Down_Rear_Left(r, c) = _values(i) : i = i + 1
            Next
        Next

        For c = 0 To 1
            If c = 0 Then i = 860
            If c = 1 Then i = 890
            For r = 0 To 20
                STsgn_UP_Rear_Left(r, c) = _values(i) : i = i + 1
            Next
        Next

        For c = 0 To 1
            If c = 0 Then i = 920
            If c = 1 Then i = 950
            For r = 0 To 20
                STsgn_CDE_HB_RTRV_G(r, c) = _values(i) : i = i + 1
            Next
            'ADC_CDE_HB_RTRV_G(c) = _values(i) : i = i + 1
        Next

        For c = 0 To 1
            If c = 0 Then i = 980
            If c = 1 Then i = 1010
            For r = 0 To 20
                STsgn_SGN_COMMUN_MOT_RTRV_D(r, c) = _values(i) : i = i + 1
            Next
        Next

        For c = 0 To 1
            If c = 0 Then i = 1040
            If c = 1 Then i = 1070
            For r = 0 To 20
                STsgn_CDE_DG_RTRV_D(r, c) = _values(i) : i = i + 1
            Next
            ADC_CDE_DG_RTRV_D(c) = _values(i) : i = i + 1
        Next

        For c = 0 To 1
            If c = 0 Then i = 1100
            If c = 1 Then i = 1130
            For r = 0 To 20
                STsgn_CDE_DG_RTRV_G(r, c) = _values(i) : i = i + 1
            Next
            ADC_CDE_DG_RTRV_G(c) = _values(i) : i = i + 1
        Next

        For c = 0 To 1
            If c = 0 Then i = 1160
            If c = 1 Then i = 1190
            For r = 0 To 20
                STsgn_CDE_RBT_RTRV_G(r, c) = _values(i) : i = i + 1
            Next
            ADC_CDE_RBT_RTRV_G(c) = _values(i) : i = i + 1
        Next

        For c = 0 To 1
            If c = 0 Then i = 1220
            If c = 1 Then i = 1250
            For r = 0 To 20
                STsgn_CDE_RBT_RTRV_D(r, c) = _values(i) : i = i + 1
            Next
            ADC_CDE_RBT_RTRV_D(c) = _values(i) : i = i + 1
        Next

        For c = 0 To 1
            If c = 0 Then i = 1280
            If c = 1 Then i = 1310
            For r = 0 To 20
                STsgn_SGN_COMMUN_MOT_RTRV_G(r, c) = _values(i) : i = i + 1
            Next
        Next

        i = 1331
        External_Temp = _values(i) : i = i + 1
        ADC_Temp = _values(i) : i = i + 1

        i = 1340
        UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12 = _values(i) : i = i + 1
        UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12 = _values(i) : i = i + 1
        UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12 = _values(i) : i = i + 1
        UCDA_SINGLE_SW_DIAG_Frequency_X1_12 = _values(i) : i = i + 1
        UCDA_SINGLE_SW_DIAG_RiseTime_X1_12 = _values(i) : i = i + 1
        UCDA_SINGLE_SW_DIAG_FallTime_X1_12 = _values(i) : i = i + 1
        UCDA_SINGLE_SW_DIAG = _values(i) : i = i + 1

        i = 1350
        UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24 = _values(i) : i = i + 1
        UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24 = _values(i) : i = i + 1
        UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24 = _values(i) : i = i + 1
        UCDA_DOOR_LOCK_DIAG_Frequency_X1_24 = _values(i) : i = i + 1
        UCDA_DOOR_LOCK_DIAG_RiseTime_X1_24 = _values(i) : i = i + 1
        UCDA_DOOR_LOCK_DIAG_FallTime_X1_24 = _values(i) : i = i + 1
        UCDA_DOOR_LOCK_DIAG = _values(i) : i = i + 1

        i = 1400
        Push_SELECT_LEFT_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_SELECT_LEFT_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        Push_FOLDING_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_FOLDING_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_ADJUST_UP_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_ADJUST_DOWN_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_ADJUST_LEFT_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_ADJUST_RIGHT_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy = _values(i) : i = i + 1
        WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy = _values(i) : i = i + 1
        WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy = _values(i) : i = i + 1

        Push_CHILDREN_LOCK_Backlight_Polygon_Exy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_Backlight_Polygon_Fxy = _values(i) : i = i + 1

        i = 1450
        Push_SELECT_LEFT_LED_Polygon_Axy = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_Polygon_Bxy = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_Polygon_Cxy = _values(i) : i = i + 1
        Push_SELECT_LEFT_LED_Polygon_Dxy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_Polygon_Axy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_Polygon_Bxy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_Polygon_Cxy = _values(i) : i = i + 1
        Push_SELECT_RIGHT_LED_Polygon_Dxy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_Polygon_Axy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_Polygon_Bxy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_Polygon_Cxy = _values(i) : i = i + 1
        Push_CHILDREN_LOCK_LED_Polygon_Dxy = _values(i) : i = i + 1

        i = 1480
        For index = 0 To 19
            Customer_Interface(index) = _values(i + index)
        Next
        'i = 1500

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
            Case eTestResult.FailedInitROOFLINCommunication
                TestResultDescription = "Failed Init ROOF LIN Communication"
            Case eTestResult.FailedOpenDIAGonLINSession
                TestResultDescription = "Failed Open DIAG on LIN Session"
            Case eTestResult.FailedInitiateStateProduct
                TestResultDescription = "Failed Initiate State Product"
            Case eTestResult.FailedCheckSerialNumber
                TestResultDescription = "Failed Valeo Serial Number"

            Case eTestResult.FailedDecorFrame
                TestResultDescription = "Failed Decor frame"

            Case eTestResult.FailedBezel
                TestResultDescription = "Failed bezel"

            Case eTestResult.FailedPush1_Shape
                TestResultDescription = "Failed Shape Select Left mirror"
            Case eTestResult.FailedPush2_Shape
                TestResultDescription = "Failed Shape Select Right mirror"
            Case eTestResult.FailedPush3_Shape
                TestResultDescription = "Failed Shape folding"
            Case eTestResult.FailedPush4_Shape
                TestResultDescription = "Failed Shape Adjust up"
            Case eTestResult.FailedPush5_Shape
                TestResultDescription = "Failed Shape Adjust down"
            Case eTestResult.FailedPush6_Shape
                TestResultDescription = "Failed Shape Adjust left"
            Case eTestResult.FailedPush7_Shape
                TestResultDescription = "Failed Shape Adjust right"
            Case eTestResult.FailedPush8_Shape
                TestResultDescription = "Failed Shape WL Front left"
            Case eTestResult.FailedPush9_Shape
                TestResultDescription = "Failed Shape WL Front right"
            Case eTestResult.FailedPush10_Shape
                TestResultDescription = "Failed Shape WL Rear left"
            Case eTestResult.FailedPush11_Shape
                TestResultDescription = "Failed Shape WL Rear right"
            Case eTestResult.FailedPush12_Shape
                TestResultDescription = "Failed Shape children lock"

            Case eTestResult.FailedPush1_Backlight
                TestResultDescription = "Failed Backlight Select Left mirror"
            Case eTestResult.FailedPush2_Backlight
                TestResultDescription = "Failed Backlight Select Right mirror"
            Case eTestResult.FailedPush3_Backlight
                TestResultDescription = "Failed Backlight folding"
            Case eTestResult.FailedPush4_Backlight
                TestResultDescription = "Failed Backlight Adjust up"
            Case eTestResult.FailedPush5_Backlight
                TestResultDescription = "Failed Backlight Adjust down"
            Case eTestResult.FailedPush6_Backlight
                TestResultDescription = "Failed Backlight Adjust left"
            Case eTestResult.FailedPush7_Backlight
                TestResultDescription = "Failed Backlight Adjust right"
            Case eTestResult.FailedPush8_Backlight
                TestResultDescription = "Failed Backlight WL Front left"
            Case eTestResult.FailedPush9_Backlight
                TestResultDescription = "Failed Backlight WL Front right"
            Case eTestResult.FailedPush10_Backlight
                TestResultDescription = "Failed Backlight WL Rear left"
            Case eTestResult.FailedPush11_Backlight
                TestResultDescription = "Failed Backlight WL Rear right"
            Case eTestResult.FailedPush12_Backlight
                TestResultDescription = "Failed Backlight children lock"

            Case eTestResult.SL_Telltale_Intensity
                TestResultDescription = "Failed Telltale select left mirror Intensity"
            Case eTestResult.SR_Telltale_Intensity
                TestResultDescription = "Failed Telltale select right mirror Intensity"
            Case eTestResult.CL_Telltale_Intensity
                TestResultDescription = "Failed Telltale children lock Intensity"

            Case eTestResult.SL_Telltale_Defect
                TestResultDescription = "Failed Telltale select left mirror"
            Case eTestResult.SR_Telltale_Defect
                TestResultDescription = "Failed Telltale select right mirror"
            Case eTestResult.CL_Telltale_Defect
                TestResultDescription = "Failed Telltale children lock"

            Case eTestResult.FailedPush1_Homogeneity
                TestResultDescription = "Failed Homogeneity Select Left mirror"
            Case eTestResult.FailedPush2_Homogeneity
                TestResultDescription = "Failed Homogeneity Select Right mirror"
            Case eTestResult.FailedPush3_Homogeneity
                TestResultDescription = "Failed Homogeneity folding"
            Case eTestResult.FailedPush4_Homogeneity
                TestResultDescription = "Failed Homogeneity Adjust up"
            Case eTestResult.FailedPush5_Homogeneity
                TestResultDescription = "Failed Homogeneity Adjust down"
            Case eTestResult.FailedPush6_Homogeneity
                TestResultDescription = "Failed Homogeneity Adjust left"
            Case eTestResult.FailedPush7_Homogeneity
                TestResultDescription = "Failed Homogeneity Adjust right"
            Case eTestResult.FailedPush8_Homogeneity
                TestResultDescription = "Failed Homogeneity WL Front left"
            Case eTestResult.FailedPush9_Homogeneity
                TestResultDescription = "Failed Homogeneity WL Front right"
            Case eTestResult.FailedPush10_Homogeneity
                TestResultDescription = "Failed Homogeneity WL Rear left"
            Case eTestResult.FailedPush11_Homogeneity
                TestResultDescription = "Failed Homogeneity WL Rear right"
            Case eTestResult.FailedPush12_Homogeneity
                TestResultDescription = "Failed Homogeneity children lock"

            Case eTestResult.FailedOthers
                TestResultDescription = "Failed others"
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
                                 vbTab & mWS02Main.TestModeDescription(mWS02Main.TestMode) &
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
            'Save = Save Or SaveResultValue(fileWriter, Side_Barcode, "FromPLC")
            'Save = Save Or SaveResultValue(fileWriter, FixtureID, "FromPLC")
            fileWriter.WriteLine("Test Software Version :" & mConstants.SoftwareVersion & vbTab & " Update Date:" & mConstants.SoftwareDate)
            fileWriter.WriteLine("Test Cycle Time :" & mWS02Main.TestLast)
            fileWriter.WriteLine("")
            groupName = "SINGLE"
            ' SINGLE TEST RESULTS
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  SINGLE TEST RESULTS")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveSingleTestResult(fileWriter, ePOWER_UP, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eINIT_LIN_COMMUNICATION, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eOPEN_DIAG_ON_LIN_SESSION, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eWriteConfiguration)
            Save = Save Or SaveSingleTestResult(fileWriter, eNormalModeCurrent, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eEmsTraceability, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eAnalogInput, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eDigitalOutput, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, ePWMOutput, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eShape_CONFORMITY, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eAreaMirror_Conformity, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eAreaWili_Conformity, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eBACKLIGHT_CONFORMITY, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eBACKLIGHT_HOMOGENEITY, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eTELLTALE_CONFORMITY, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eWrite_Temperature_Set, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eWrite_TraceabilityMMS, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eWrite_TestByteMMS, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eResetEcu_AfterWriting, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRead_TraceabilityMMS, groupName)
            Save = Save Or SaveSingleTestResult(fileWriter, eRead_TestByteMMS, groupName)

            fileWriter.WriteLine("")

            groupName = ""
            ' Initiate State
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Initiate State Product TEST RESULTS")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, PowerUp_VBAT, groupName)
            Save = Save Or SaveResultValue(fileWriter, PowerUp_Ibat, groupName)
            fileWriter.WriteLine("")

            '' Valeo Serial Number
            'fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            'fileWriter.WriteLine("                  Valeo Serial Number TEST Results")
            'fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            'Save = Save Or SaveResultValue(fileWriter, Valeo_Serial_Number)
            'Save = Save Or SaveResultValue(fileWriter, Valeo_TestByte)
            'fileWriter.WriteLine("")

            groupName = ""
            ' SHAPE
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  SHAPE PUSH TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_ADJUST_UP, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK, groupName)
            Save = Save Or SaveResultValue(fileWriter, MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2, groupName)
            Save = Save Or SaveResultValue(fileWriter, CONFORMITY_Adatper_Front, groupName)
            Save = Save Or SaveResultValue(fileWriter, CONFORMITY_Adatper_Rear, groupName)
            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  DEFECT AREA PUSH TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_FOLDING_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_ADJUST_UP, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_ADJUST_DOWN, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_ADJUST_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_ADJUST_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_CHILDREN_LOCK, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_PUSH_CHILDREN_LOCK2, groupName)
            Save = Save Or SaveResultValue(fileWriter, Points_On_Front_Left, groupName)
            Save = Save Or SaveResultValue(fileWriter, Points_On_Front_Right, groupName)
            Save = Save Or SaveResultValue(fileWriter, Points_On_Rear_Left, groupName)
            Save = Save Or SaveResultValue(fileWriter, Points_On_Rear_Right, groupName)
            Save = Save Or SaveResultValue(fileWriter, Ring_Red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Ring_Green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Ring_Blue, groupName)
            For index = 0 To 19
                Save = Save Or SaveResultValue(fileWriter, Customer_Interface(index), groupName)
            Next
            fileWriter.WriteLine("")
            groupName = ""
            ' Other
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Other TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, CONFORMITY_Bezel, groupName)
            Save = Save Or SaveResultValue(fileWriter, CONFORMITY_DecorFrame, groupName)
            fileWriter.WriteLine("")

            groupName = ""
            ' Backlight PUSH
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Backlight PUSH TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Backlight Select Right TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Select Right TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")


            fileWriter.WriteLine("                  Backlight Select Left TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Select Left TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight Adjust UP TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Adjust UP TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_UP_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight Adjust DOWN TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Adjust DOWN TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_DOWN_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight Adjust RIGHT TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Adjust RIGHT TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_RIGHT_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight Adjust LEFT TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Adjust LEFT TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_ADJUST_LEFT_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight Folding Mirror TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Folding Mirror TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_FOLDING_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight WL FRONT LEFT TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight WL FRONT LEFT TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight WL FRONT RIGHT TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight WL FRONT RIGHT TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight WL REAR LEFT TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight WL REAR LEFT TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight WL REAR RIGHT TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight WL REAR RIGHT TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  Backlight Children Lock TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Children Lock TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")
            fileWriter.WriteLine("                  Backlight Children Lock2 TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_RSQ, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_backlight_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_backlight_homogeneity, groupName)
            fileWriter.WriteLine("                  Backlight Children Lock2 TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_backlight_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_backlight_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_backlight_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_backlight_DominantWavelenght, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_backlight_Saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_backlight_intensity_Camera, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_Polygon_Dxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_Polygon_Exy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK2_Backlight_Polygon_Fxy, groupName)
            fileWriter.WriteLine("")

            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  DEFECT AREA PUSH TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2, groupName)
            fileWriter.WriteLine("")


            groupName = ""
            ' TELL Tale
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  TellTale TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  TellTale Select Right TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_WaveLenght, groupName)
            fileWriter.WriteLine("                  TellTale Select Right TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_Polygon_Dxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  TellTale Select Left TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_WaveLenght, groupName)
            fileWriter.WriteLine("                  TellTale Select Left TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_LEFT_LED_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_SELECT_RIGHT_LED_Polygon_Dxy, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("                  TellTale Children Lock TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_saturation, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_intensity, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_WaveLenght, groupName)
            fileWriter.WriteLine("                  TellTale Children Lock TEST Information")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_red, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_green, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_blue, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_x, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_y, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_Polygon_Axy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_Polygon_Bxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_Polygon_Cxy, groupName)
            Save = Save Or SaveResultValue(fileWriter, Push_CHILDREN_LOCK_LED_Polygon_Dxy, groupName)
            fileWriter.WriteLine("")

            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  DEFECT AREA TELLTALE TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR, groupName)
            Save = Save Or SaveResultValue(fileWriter, DEFECT_AREA_TELLTALE_CHILDREN_LOCK, groupName)
            fileWriter.WriteLine("")

            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  EMS Traceability TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            Save = Save Or SaveResultValue(fileWriter, PCBA_Number_Reference, groupName)
            Save = Save Or SaveResultValue(fileWriter, PCBA_Number_Index, groupName)
            Save = Save Or SaveResultValue(fileWriter, PCBA_Plant_Line, groupName)
            Save = Save Or SaveResultValue(fileWriter, PCBA_ManufacturingDate, groupName)
            Save = Save Or SaveResultValue(fileWriter, PCBA_SerialNumber, groupName)
            Save = Save Or SaveResultValue(fileWriter, PCBA_DeviationNumber, groupName)
            Save = Save Or SaveResultValue(fileWriter, LED_BIN_PT_White_RSA, groupName)
            Save = Save Or SaveResultValue(fileWriter, LED_BIN_PT_RED, groupName)
            Save = Save Or SaveResultValue(fileWriter, LED_BIN_PT_YELLOW, groupName)
            Save = Save Or SaveResultValue(fileWriter, LED_BIN_PT_WHITE_NISSAN, groupName)
            Save = Save Or SaveResultValue(fileWriter, EMS_Test_Byte, groupName)

            Save = Save Or SaveResultValue(fileWriter, Major_SoftwareVersion, groupName)
            Save = Save Or SaveResultValue(fileWriter, Minor_SoftwareVersion, groupName)
            Save = Save Or SaveResultValue(fileWriter, Major_NVMversion, groupName)
            Save = Save Or SaveResultValue(fileWriter, Minor_NVMversion, groupName)
            Save = Save Or SaveResultValue(fileWriter, SW_checksum, groupName)
            Save = Save Or SaveResultValue(fileWriter, SW_Coding, groupName)
            Save = Save Or SaveResultValue(fileWriter, Backlight_Coding, groupName)
            Save = Save Or SaveResultValue(fileWriter, HW_Coding, groupName)

            fileWriter.WriteLine("")

            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Analog Input TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            Save = Save Or SaveResultValue(fileWriter, ADC_UCAD_VARIANT_1, groupName)
            Save = Save Or SaveResultValue(fileWriter, ADC_UCAD_VARIANT_2, groupName)
            Save = Save Or SaveResultValue(fileWriter, ADC_CURSEUR_LEFT_RIGHT, groupName)
            Save = Save Or SaveResultValue(fileWriter, ADC_CURSEUR_UP_DN, groupName)
            Save = Save Or SaveResultValue(fileWriter, ADC_Temp, groupName)
            Save = Save Or SaveResultValue(fileWriter, External_Temp, groupName)

            '
            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_Front_Passenger_Commande_UP TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_Front_Passenger_Commande_UP(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_Front_Passenger_Commande_Down TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_Front_Passenger_Commande_Down(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_Switches_Inhibition TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_Switches_Inhibition(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_Down_Rear_Right TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_Down_Rear_Right(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_UP_Rear_Right TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_UP_Rear_Right(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_Down_Rear_Left TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_Down_Rear_Left(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_UP_Rear_Left TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_UP_Rear_Left(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_CDE_HB_RTRV_G TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_CDE_HB_RTRV_G(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_SGN_COMMUN_MOT_RTRV_D TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_SGN_COMMUN_MOT_RTRV_D(r, c), groupName)
                Next
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_CDE_DG_RTRV_D TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_CDE_DG_RTRV_D(r, c), groupName)
                Next
                Save = Save Or SaveResultValue(fileWriter, ADC_CDE_DG_RTRV_D(c), groupName)
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_CDE_DG_RTRV_G TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_CDE_DG_RTRV_G(r, c), groupName)
                Next
                Save = Save Or SaveResultValue(fileWriter, ADC_CDE_DG_RTRV_G(c), groupName)
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_CDE_RBT_RTRV_G TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_CDE_RBT_RTRV_G(r, c), groupName)
                Next
                Save = Save Or SaveResultValue(fileWriter, ADC_CDE_RBT_RTRV_G(c), groupName)
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_CDE_RBT_RTRV_D TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_CDE_RBT_RTRV_D(r, c), groupName)
                Next
                Save = Save Or SaveResultValue(fileWriter, ADC_CDE_RBT_RTRV_D(c), groupName)
            Next
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 Digital Output STsgn_SGN_COMMUN_MOT_RTRV_G TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")

            For c = 0 To 1
                For r = 0 To 20
                    Save = Save Or SaveResultValue(fileWriter, STsgn_SGN_COMMUN_MOT_RTRV_G(r, c), groupName)
                Next
            Next

            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 PWM Output UCDA_SINGLE_SW_DIAG")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_SINGLE_SW_DIAG_Frequency_X1_12, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_SINGLE_SW_DIAG_RiseTime_X1_12, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_SINGLE_SW_DIAG_FallTime_X1_12, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_SINGLE_SW_DIAG, groupName)

            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                 PWM Output UCDA_DOOR_LOCK_DIAG")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            Save = Save Or SaveResultValue(fileWriter, UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_DOOR_LOCK_DIAG_Frequency_X1_24, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_DOOR_LOCK_DIAG_RiseTime_X1_24, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_DOOR_LOCK_DIAG_FallTime_X1_24, groupName)
            Save = Save Or SaveResultValue(fileWriter, UCDA_DOOR_LOCK_DIAG, groupName)

            groupName = ""
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  MMS Traceability TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            'Read Valeo Traceability MMS
            Save = Save Or SaveResultValue(fileWriter, MMS_Read_Final_Product_Reference, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Read_Final_Product_Index, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Read_Valeo_Final_Product_Plant, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Read_Valeo_Final_Product_Line, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Read_Valeo_Final_Product_Manufacturing_Date, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Read_Valeo_Serial_Number, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Read_Deviation_Number, groupName)
            Save = Save Or SaveResultValue(fileWriter, MMS_Read_Test_Byte, groupName)
            fileWriter.WriteLine("")

            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            fileWriter.WriteLine("                  Sleep Mode Current TEST Results")
            fileWriter.WriteLine("-----------------------------------------------------------------------------------")
            'Save = Save Or SaveResultValue(fileWriter, SleepModeCurrent)
            fileWriter.WriteLine("")
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
                Select Case mWS02Main.TestMode
                    Case mWS02Main.eTestMode.Remote
                        tempConvertCode = TestMode.MassProduction
                    Case mWS02Main.eTestMode.Master
                        tempConvertCode = TestMode.DailyCheck
                    Case Else
                        tempConvertCode = TestMode.Unknown
                End Select
                XMLResult.CreateXML_Unit(2, PartUniqueNumber.Value.ToString, tempUnitStatus, 0, CDate(TestDate.StringValue & " " & TestTime.StringValue),
                                Now, CInt(IIf(mWS02Main.TestLast > 30000, 9999, mWS02Main.TestLast)), CInt(FixtureID.Value), Side_Barcode.Value.ToString(), tempConvertCode)
                XMLResult.CreateXML(XMLResultPath, Now, Now, "0040615", RecipeName.StringValue, mConstants.SoftwareVersion, "SquMain01", "1234567")
            Catch ex As Exception
                Console.WriteLine(ex.ToString())
            End Try
        End Try
    End Function

    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Function LoadResultValue(ByVal fileReader As StreamReader,
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
