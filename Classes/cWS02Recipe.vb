Option Explicit On
Option Strict On

Imports System
Imports System.IO

Public Class cWS02Recipe
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    'Public constants
    Public Const ValueCount = 750

    ' General information																				
    Public CreationDate As cRecipeValue
    Public CreationTime As cRecipeValue
    Public ModifyDate As cRecipeValue
    Public ModifyTime As cRecipeValue

    ' Test enables																				
    Public TestEnable_WriteConfiguration As cRecipeValue
    Public TestEnable_NormalMode As cRecipeValue
    Public TestEnable_EMSTraceability As cRecipeValue
    Public TestEnable_AnalogInput As cRecipeValue
    Public TestEnable_DigitalOutput As cRecipeValue
    Public TestEnable_PWMOutput As cRecipeValue
    Public TestEnable_SHAPE_Select_Mirror As cRecipeValue
    Public TestEnable_BACKLIGHT_Select_Mirror As cRecipeValue
    Public TestEnable_SHAPE_Folding_Mirror As cRecipeValue
    Public TestEnable_BACKLIGHT_Folding_Mirror As cRecipeValue
    Public TestEnable_SHAPE_Wili_Front As cRecipeValue
    Public TestEnable_BACKLIGHT_Wili_Front As cRecipeValue
    Public TestEnable_SHAPE_Wili_Rear As cRecipeValue
    Public TestEnable_BACKLIGHT_Wili_Rear As cRecipeValue
    Public TestEnable_SHAPE_ChildrenLock As cRecipeValue
    Public TestEnable_SHAPE_ChildrenLock2 As cRecipeValue
    Public TestEnable_BACKLIGHT_ChildrenLock As cRecipeValue
    Public TestEnable_BACKLIGHT_ChildrenLock2 As cRecipeValue
    Public TestEnable_BACKLIGHT_Homogeneity As cRecipeValue
    Public TestEnable_TELLTALE_SelectMirror As cRecipeValue
    Public TestEnable_TELLTALE_ChildrenLock As cRecipeValue
    Public TestEnable_BezelConformity As cRecipeValue
    Public TestEnable_DecorFrameConformity As cRecipeValue
    Public TestEnable_WriteMMS As cRecipeValue
    Public TestEnable_ReadMMS As cRecipeValue
    Public TestEnable_PointsOnWili As cRecipeValue
    Public TestEnable_CustomerInterface As cRecipeValue
    Public TestEnable_Adapter As cRecipeValue

    Public TestEnable_FOLDING_Option As cRecipeValue
    Public TestEnable_MEMO_Option As cRecipeValue
    Public TestEnable_PWL_Option As cRecipeValue

    Public TestEnable_EV_Option As cRecipeValue
    Public TestEnable_Temperature As cRecipeValue
    Public TestEnable_Mirrortiltingposition As cRecipeValue

    ' General settings																				
    Public PowerSupplyVoltage As cRecipeValue
    Public PowerSupplyCurrentLimit As cRecipeValue
    'Camera Setting
    Public Camera_Programm As cRecipeValue
    Public Camera_Recipe As cRecipeValue

    ' Limits on standard signals																				
    Public PowersupplyVbat As cRecipeValue
    Public PowersupplyIbat As cRecipeValue

    Public Sdt_Signal(0 To 20) As cRecipeValue

    Public DO_UP_Front_Passenger As cRecipeValue
    Public DO_DN_Front_Passenger As cRecipeValue

    Public DO_Switch_Inhibtion As cRecipeValue

    Public DO_DN_Rear_Right As cRecipeValue
    Public DO_UP_Rear_Right As cRecipeValue
    Public DO_DN_Rear_Left As cRecipeValue
    Public DO_UP_Rear_Left As cRecipeValue
    'Req 125_2 / 126_2 / 127_1
    Public DO_CDE_HB_RTRV_G(0 To 1) As cRecipeValue
    'Req 128_2 / 130_1
    Public D0_SGN_COMMUN_MOT_RTRV_D As cRecipeValue
    'Req 136_2 / 132_2 / 137_2 / 133_1 / 138_2 / 139_1
    Public DO_CDE_DG_RTRV_D(0 To 1) As cRecipeValue
    Public ADC_CDE_DG_RTRV_D(0 To 1) As cRecipeValue
    'Req 140_2 / 134_2 / 141_2 / 142_1 / 135_2 / 079_1
    Public DO_CDE_DG_RTRV_G(0 To 1) As cRecipeValue
    Public ADC_CDE_DG_RTRV_G(0 To 1) As cRecipeValue
    'Req 230_2 / 080_2
    Public DO_CDE_RBT_RTRV_G(0 To 1) As cRecipeValue
    Public ADC_CDE_RBT_RTRV_G(0 To 1) As cRecipeValue
    'Req 161_2 / 081_2
    Public DO_CDE_RBT_RTRV_D(0 To 1) As cRecipeValue
    Public ADC_CDE_RBT_RTRV_D(0 To 1) As cRecipeValue
    'Req 333_2 / 168_1
    Public D0_SGN_COMMUN_MOT_RTRV_G As cRecipeValue


    Public Push_Select_Left_pictogram_Conformity As cRecipeValue
    Public Push_Select_Right_pictogram_Conformity As cRecipeValue
    Public Push_Folding_pictogram_Conformity As cRecipeValue
    Public Push_Adjust_UP_pictogram_Conformity As cRecipeValue
    Public Push_Adjust_Down_pictogram_Conformity As cRecipeValue
    Public Push_Adjust_Left_pictogram_Conformity As cRecipeValue
    Public Push_Adjust_Right_pictogram_Conformity As cRecipeValue
    Public Windows_Lifter_Front_Left_pictogram_Conformity As cRecipeValue
    Public Windows_Lifter_Front_Right_pictogram_Conformity As cRecipeValue
    Public Windows_Lifter_Rear_Left_pictogram_Conformity As cRecipeValue
    Public Windows_Lifter_Rear_Right_pictogram_Conformity As cRecipeValue
    Public Push_Children_Lock_pictogram_Conformity As cRecipeValue
    Public Push_Children_Lock2_pictogram_Conformity As cRecipeValue
    Public Bezel_Conformity As cRecipeValue
    Public Decor_Frame_Conformity As cRecipeValue

    Public Push_Select_Left_pictogram_Defect_Area As cRecipeValue
    Public Push_Select_Right_pictogram_Defect_Area As cRecipeValue
    Public Push_Folding_pictogram_Defect_Area As cRecipeValue
    Public Push_Adjust_UP_pictogram_Defect_Area As cRecipeValue
    Public Push_Adjust_Down_pictogram_Defect_Area As cRecipeValue
    Public Push_Adjust_Left_pictogram_Defect_Area As cRecipeValue
    Public Push_Adjust_Right_pictogram_Defect_Area As cRecipeValue
    Public Windows_Lifter_Front_Left_pictogram_Defect_Area As cRecipeValue
    Public Windows_Lifter_Front_Right_pictogram_Defect_Area As cRecipeValue
    Public Windows_Lifter_Rear_Left_pictogram_Defect_Area As cRecipeValue
    Public Windows_Lifter_Rear_Right_pictogram_Defect_Area As cRecipeValue
    Public Push_Children_Lock_pictogram_Defect_Area As cRecipeValue
    Public Push_Children_Lock2_pictogram_Defect_Area As cRecipeValue

    Public Ring_Red As cRecipeValue
    Public Ring_Green As cRecipeValue
    Public Ring_Blue As cRecipeValue

    Public PointsOn_Front_Left As cRecipeValue
    Public PointsOn_Front_Right As cRecipeValue
    Public PointsOn_Rear_Left As cRecipeValue
    Public PointsOn_Rear_Right As cRecipeValue

    Public CustomerInterface(19) As cRecipeValue
    Public Apater_Front_pictogram_Conformity As cRecipeValue
    Public Apater_Rear_pictogram_Conformity As cRecipeValue

    Public Push_Select_Left_Intensity As cRecipeValue
    Public Push_Select_Left_RSQ As cRecipeValue
    Public Push_Select_Left_Polygon_Axy As cRecipeValue
    Public Push_Select_Left_Polygon_Bxy As cRecipeValue
    Public Push_Select_Left_Polygon_Cxy As cRecipeValue
    Public Push_Select_Left_Polygon_Dxy As cRecipeValue
    Public Push_Select_Left_Polygon_Exy As cRecipeValue
    Public Push_Select_Left_Polygon_Fxy As cRecipeValue
    Public Push_Select_Left_minimum_homogeneity As cRecipeValue
    '
    Public Push_Select_Right_Intensity As cRecipeValue
    Public Push_Select_Right_RSQ As cRecipeValue
    Public Push_Select_Right_Polygon_Axy As cRecipeValue
    Public Push_Select_Right_Polygon_Bxy As cRecipeValue
    Public Push_Select_Right_Polygon_Cxy As cRecipeValue
    Public Push_Select_Right_Polygon_Dxy As cRecipeValue
    Public Push_Select_Right_Polygon_Exy As cRecipeValue
    Public Push_Select_Right_Polygon_Fxy As cRecipeValue
    Public Push_Select_Right_minimum_homogeneity As cRecipeValue
    '
    Public Push_Folding_Intensity As cRecipeValue
    Public Push_Folding_RSQ As cRecipeValue
    Public Push_Folding_Polygon_Axy As cRecipeValue
    Public Push_Folding_Polygon_Bxy As cRecipeValue
    Public Push_Folding_Polygon_Cxy As cRecipeValue
    Public Push_Folding_Polygon_Dxy As cRecipeValue
    Public Push_Folding_Polygon_Exy As cRecipeValue
    Public Push_Folding_Polygon_Fxy As cRecipeValue
    Public Push_Folding_minimum_homogeneity As cRecipeValue
    '
    Public Push_Adjust_UP_Intensity As cRecipeValue
    Public Push_Adjust_UP_RSQ As cRecipeValue
    Public Push_Adjust_UP_Polygon_Axy As cRecipeValue
    Public Push_Adjust_UP_Polygon_Bxy As cRecipeValue
    Public Push_Adjust_UP_Polygon_Cxy As cRecipeValue
    Public Push_Adjust_UP_Polygon_Dxy As cRecipeValue
    Public Push_Adjust_UP_Polygon_Exy As cRecipeValue
    Public Push_Adjust_UP_Polygon_Fxy As cRecipeValue
    Public Push_Adjust_UP_minimum_homogeneity As cRecipeValue
    '
    Public Push_Adjust_DOWN_Intensity As cRecipeValue
    Public Push_Adjust_DOWN_RSQ As cRecipeValue
    Public Push_Adjust_DOWN_Polygon_Axy As cRecipeValue
    Public Push_Adjust_DOWN_Polygon_Bxy As cRecipeValue
    Public Push_Adjust_DOWN_Polygon_Cxy As cRecipeValue
    Public Push_Adjust_DOWN_Polygon_Dxy As cRecipeValue
    Public Push_Adjust_DOWN_Polygon_Exy As cRecipeValue
    Public Push_Adjust_DOWN_Polygon_Fxy As cRecipeValue
    Public Push_Adjust_DOWN_minimum_homogeneity As cRecipeValue
    '
    Public Push_Adjust_Left_Intensity As cRecipeValue
    Public Push_Adjust_Left_RSQ As cRecipeValue
    Public Push_Adjust_Left_Polygon_Axy As cRecipeValue
    Public Push_Adjust_Left_Polygon_Bxy As cRecipeValue
    Public Push_Adjust_Left_Polygon_Cxy As cRecipeValue
    Public Push_Adjust_Left_Polygon_Dxy As cRecipeValue
    Public Push_Adjust_Left_Polygon_Exy As cRecipeValue
    Public Push_Adjust_Left_Polygon_Fxy As cRecipeValue
    Public Push_Adjust_Left_minimum_homogeneity As cRecipeValue
    '
    Public Push_Adjust_Right_Intensity As cRecipeValue
    Public Push_Adjust_Right_RSQ As cRecipeValue
    Public Push_Adjust_Right_Polygon_Axy As cRecipeValue
    Public Push_Adjust_Right_Polygon_Bxy As cRecipeValue
    Public Push_Adjust_Right_Polygon_Cxy As cRecipeValue
    Public Push_Adjust_Right_Polygon_Dxy As cRecipeValue
    Public Push_Adjust_Right_Polygon_Exy As cRecipeValue
    Public Push_Adjust_Right_Polygon_Fxy As cRecipeValue
    Public Push_Adjust_Right_minimum_homogeneity As cRecipeValue
    '
    Public Windows_Lifter_Front_Left_Intensity As cRecipeValue
    Public Windows_Lifter_Front_Left_RSQ As cRecipeValue
    Public Windows_Lifter_Front_Left_Polygon_Axy As cRecipeValue
    Public Windows_Lifter_Front_Left_Polygon_Bxy As cRecipeValue
    Public Windows_Lifter_Front_Left_Polygon_Cxy As cRecipeValue
    Public Windows_Lifter_Front_Left_Polygon_Dxy As cRecipeValue
    Public Windows_Lifter_Front_Left_Polygon_Exy As cRecipeValue
    Public Windows_Lifter_Front_Left_Polygon_Fxy As cRecipeValue
    Public Windows_Lifter_Front_Left_minimum_homogeneity As cRecipeValue
    '
    Public Windows_Lifter_Front_Right_Intensity As cRecipeValue
    Public Windows_Lifter_Front_Right_RSQ As cRecipeValue
    Public Windows_Lifter_Front_Right_Polygon_Axy As cRecipeValue
    Public Windows_Lifter_Front_Right_Polygon_Bxy As cRecipeValue
    Public Windows_Lifter_Front_Right_Polygon_Cxy As cRecipeValue
    Public Windows_Lifter_Front_Right_Polygon_Dxy As cRecipeValue
    Public Windows_Lifter_Front_Right_Polygon_Exy As cRecipeValue
    Public Windows_Lifter_Front_Right_Polygon_Fxy As cRecipeValue
    Public Windows_Lifter_Front_Right_minimum_homogeneity As cRecipeValue
    '
    Public Windows_Lifter_Rear_Left_Intensity As cRecipeValue
    Public Windows_Lifter_Rear_Left_RSQ As cRecipeValue
    Public Windows_Lifter_Rear_Left_Polygon_Axy As cRecipeValue
    Public Windows_Lifter_Rear_Left_Polygon_Bxy As cRecipeValue
    Public Windows_Lifter_Rear_Left_Polygon_Cxy As cRecipeValue
    Public Windows_Lifter_Rear_Left_Polygon_Dxy As cRecipeValue
    Public Windows_Lifter_Rear_Left_Polygon_Exy As cRecipeValue
    Public Windows_Lifter_Rear_Left_Polygon_Fxy As cRecipeValue
    Public Windows_Lifter_Rear_Left_minimum_homogeneity As cRecipeValue
    '
    Public Windows_Lifter_Rear_Right_Intensity As cRecipeValue
    Public Windows_Lifter_Rear_Right_RSQ As cRecipeValue
    Public Windows_Lifter_Rear_Right_Polygon_Axy As cRecipeValue
    Public Windows_Lifter_Rear_Right_Polygon_Bxy As cRecipeValue
    Public Windows_Lifter_Rear_Right_Polygon_Cxy As cRecipeValue
    Public Windows_Lifter_Rear_Right_Polygon_Dxy As cRecipeValue
    Public Windows_Lifter_Rear_Right_Polygon_Exy As cRecipeValue
    Public Windows_Lifter_Rear_Right_Polygon_Fxy As cRecipeValue
    Public Windows_Lifter_Rear_Right_minimum_homogeneity As cRecipeValue
    '
    Public Push_Children_Lock_Intensity As cRecipeValue
    Public Push_Children_Lock_RSQ As cRecipeValue
    Public Push_Children_Lock_Polygon_Axy As cRecipeValue
    Public Push_Children_Lock_Polygon_Bxy As cRecipeValue
    Public Push_Children_Lock_Polygon_Cxy As cRecipeValue
    Public Push_Children_Lock_Polygon_Dxy As cRecipeValue
    Public Push_Children_Lock_Polygon_Exy As cRecipeValue
    Public Push_Children_Lock_Polygon_Fxy As cRecipeValue
    Public Push_Children_Lock_minimum_homogeneity As cRecipeValue
    '
    Public Push_Children_Lock2_Intensity As cRecipeValue
    Public Push_Children_Lock2_RSQ As cRecipeValue
    Public Push_Children_Lock2_Polygon_Axy As cRecipeValue
    Public Push_Children_Lock2_Polygon_Bxy As cRecipeValue
    Public Push_Children_Lock2_Polygon_Cxy As cRecipeValue
    Public Push_Children_Lock2_Polygon_Dxy As cRecipeValue
    Public Push_Children_Lock2_Polygon_Exy As cRecipeValue
    Public Push_Children_Lock2_Polygon_Fxy As cRecipeValue
    Public Push_Children_Lock2_minimum_homogeneity As cRecipeValue

    Public Push_Select_Left_backlight_Defect_Area As cRecipeValue
    Public Push_Select_Right_backlight_Defect_Area As cRecipeValue
    Public Push_Folding_backlight_Defect_Area As cRecipeValue
    Public Push_Adjust_UP_backlight_Defect_Area As cRecipeValue
    Public Push_Adjust_Down_backlight_Defect_Area As cRecipeValue
    Public Push_Adjust_Left_backlight_Defect_Area As cRecipeValue
    Public Push_Adjust_Right_backlight_Defect_Area As cRecipeValue
    Public Windows_Lifter_Front_Left_backlight_Defect_Area As cRecipeValue
    Public Windows_Lifter_Front_Right_backlight_Defect_Area As cRecipeValue
    Public Windows_Lifter_Rear_Left_backlight_Defect_Area As cRecipeValue
    Public Windows_Lifter_Rear_Right_backlight_Defect_Area As cRecipeValue
    Public Push_Children_Lock_backlight_Defect_Area As cRecipeValue
    Public Push_Children_Lock2_backlight_Defect_Area As cRecipeValue

    Public TELLTALE_Select_Left_Intensity As cRecipeValue
    Public TELLTALE_Select_Left_Saturation As cRecipeValue
    Public TELLTALE_Select_Left_WaveLenght As cRecipeValue
    Public TELLTALE_Select_Left_Polygon_Axy As cRecipeValue
    Public TELLTALE_Select_Left_Polygon_Bxy As cRecipeValue
    Public TELLTALE_Select_Left_Polygon_Cxy As cRecipeValue
    Public TELLTALE_Select_Left_Polygon_Dxy As cRecipeValue
    '
    Public TELLTALE_Select_Right_Intensity As cRecipeValue
    Public TELLTALE_Select_Right_Saturation As cRecipeValue
    Public TELLTALE_Select_Right_WaveLenght As cRecipeValue
    Public TELLTALE_Select_Right_Polygon_Axy As cRecipeValue
    Public TELLTALE_Select_Right_Polygon_Bxy As cRecipeValue
    Public TELLTALE_Select_Right_Polygon_Cxy As cRecipeValue
    Public TELLTALE_Select_Right_Polygon_Dxy As cRecipeValue
    '
    Public TELLTALE_Children_Lock_Intensity As cRecipeValue
    Public TELLTALE_Children_Lock_Saturation As cRecipeValue
    Public TELLTALE_Children_Lock_WaveLenght As cRecipeValue
    Public TELLTALE_Children_Lock_Polygon_Axy As cRecipeValue
    Public TELLTALE_Children_Lock_Polygon_Bxy As cRecipeValue
    Public TELLTALE_Children_Lock_Polygon_Cxy As cRecipeValue
    Public TELLTALE_Children_Lock_Polygon_Dxy As cRecipeValue

    Public TELLTALE_Select_Left_Defect_Area As cRecipeValue
    Public TELLTALE_Select_Right_Defect_Area As cRecipeValue
    Public TELLTALE_Children_Lock_Defect_Area As cRecipeValue

    'EMS Traceability
    Public PCBA_Number_Reference As cRecipeValue
    Public PCBA_Number_Index As cRecipeValue
    Public PCBA_Plant_Line As cRecipeValue
    Public PCBA_ManufacturingDate As cRecipeValue
    Public PCBA_SerialNumber As cRecipeValue
    Public PCBA_DeviationNumber As cRecipeValue

    Public LED_BIN_PT_White_RSA As cRecipeValue
    Public LED_BIN_PT_RED As cRecipeValue
    Public LED_BIN_PT_Yellow As cRecipeValue
    Public LED_BIN_PT_White_Nissan As cRecipeValue

    Public Major_SoftwareVersion As cRecipeValue
    Public Minor_SoftwareVersion As cRecipeValue
    Public Major_NVMversion As cRecipeValue
    Public Minor_NVMversion As cRecipeValue
    Public SW_checksum As cRecipeValue
    
    Public ADC_UCAD_Variant_1 As cRecipeValue
    Public ADC_UCAD_Variant_2 As cRecipeValue
    Public ADC_CURSEUR_LEFT_RIGHT As cRecipeValue
    Public ADC_CURSEUR_UP_DN As cRecipeValue

    'Write Valeo Traceability MMS
    Public Write_MMS_Final_Product_Reference As cRecipeValue
    Public Write_MMS_Final_Product_Index As cRecipeValue
    Public Write_MMS_Valeo_Final_Product_Plant As cRecipeValue
    Public Write_MMS_Valeo_Final_Product_Line As cRecipeValue
    Public Write_MMS_Deviation_Number As cRecipeValue
    Public Write_CustomerPartNumber As cRecipeValue
    Public Write_HARDWARE_Version As cRecipeValue

    Public Laser_IDCode As cRecipeValue

    'Product Configuration
    Public WRITE_SW_Coding As cRecipeValue
    Public WRITE_Baclight_Coding As cRecipeValue
    Public WRITE_HW_Coding As cRecipeValue
    Public WRITE_BackLight_PWM As cRecipeValue
    Public WRITE_MLTelltale_PWM As cRecipeValue
    Public WRITE_MRTelltale_PWM As cRecipeValue
    Public WRITE_CLTelltale_PWM As cRecipeValue

    'Sleep Mode current
    Public PowerSupplySleepCurrentLimit As cRecipeValue


    Public Correlation_Factor_A(0 To 15) As cRecipeValue
    Public Correlation_Factor_B(0 To 15) As cRecipeValue

    'External Backlight WLAP
    Public External_Backlight_WLAP_Voltage_LOW_X1_12 As cRecipeValue
    Public External_Backlight_WLAP_Voltage_HIGH_X1_12 As cRecipeValue
    Public External_Backlight_WLAP_Duty_Cycle_X1_12 As cRecipeValue
    Public External_Backlight_WLAP_Frequency_X1_12 As cRecipeValue
    Public External_Backlight_WLAP_ADC_VALUE As cRecipeValue
    'External Backlight DOORLOCK
    Public External_Backlight_DOOR_LOCK_Voltage_LOW_X1_24 As cRecipeValue
    Public External_Backlight_DOOR_LOCK_Voltage_HIGH_X1_24 As cRecipeValue
    Public External_Backlight_DOOR_LOCK_Duty_Cycle_X1_24 As cRecipeValue
    Public External_Backlight_DOOR_LOCK_Frequency_X1_24 As cRecipeValue
    Public External_Backlight_DOOR_LOCK_ADC_VALUE As cRecipeValue

    Public Correlation_BackColorx_Factor_A As cRecipeValue
    Public Correlation_BackColorx_Factor_B As cRecipeValue
    Public Correlation_BackColory_Factor_A As cRecipeValue
    Public Correlation_BackColory_Factor_B As cRecipeValue
    Public Correlation_TellColorx_Factor_A As cRecipeValue
    Public Correlation_TellColorx_Factor_B As cRecipeValue
    Public Correlation_TellColory_Factor_A As cRecipeValue
    Public Correlation_TellColory_Factor_B As cRecipeValue
    Public Correlation_TellWavelength_Factor_A As cRecipeValue
    Public Correlation_TellWavelength_Factor_B As cRecipeValue

    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private _values(0 To ValueCount - 1) As cRecipeValue



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+


    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Shared Function Delete(ByVal name As String) As Boolean
        ' Delete the recipe
        Delete = mRecipe.Delete(FilePath(name))
    End Function



    Public Shared Function Exists(ByVal name As String) As Boolean
        ' Return True if the recipe already exists, False otherwise
        Exists = mRecipe.Exists(FilePath(name))
    End Function



    Public Function Load(ByVal name As String) As Boolean
        ' Load the recipe
        Load = mRecipe.Load(FilePath(name), _values)
    End Function



    Public Function LoadConfiguration(ByVal path As String) As Boolean
        Dim i As Integer
        Dim j As Integer
        ' Load the recipe configuration
        LoadConfiguration = mRecipe.LoadConfiguration(path, _values)

        ' General information																				
        CreationDate = _values(0)
        CreationTime = _values(1)
        ModifyDate = _values(2)
        ModifyTime = _values(3)
        ' Test enables			
        i = 10
        TestEnable_WriteConfiguration = _values(i) : i = i + 1
        TestEnable_NormalMode = _values(i) : i = i + 1
        TestEnable_EMSTraceability = _values(i) : i = i + 1
        TestEnable_AnalogInput = _values(i) : i = i + 1
        TestEnable_DigitalOutput = _values(i) : i = i + 1
        TestEnable_SHAPE_Select_Mirror = _values(i) : i = i + 1
        TestEnable_BACKLIGHT_Select_Mirror = _values(i) : i = i + 1
        TestEnable_SHAPE_Folding_Mirror = _values(i) : i = i + 1
        TestEnable_BACKLIGHT_Folding_Mirror = _values(i) : i = i + 1
        TestEnable_SHAPE_Wili_Front = _values(i) : i = i + 1
        TestEnable_BACKLIGHT_Wili_Front = _values(i) : i = i + 1
        TestEnable_SHAPE_Wili_Rear = _values(i) : i = i + 1
        TestEnable_BACKLIGHT_Wili_Rear = _values(i) : i = i + 1
        TestEnable_SHAPE_ChildrenLock = _values(i) : i = i + 1
        TestEnable_BACKLIGHT_ChildrenLock = _values(i) : i = i + 1
        TestEnable_BACKLIGHT_Homogeneity = _values(i) : i = i + 1
        TestEnable_TELLTALE_SelectMirror = _values(i) : i = i + 1
        TestEnable_TELLTALE_ChildrenLock = _values(i) : i = i + 1
        TestEnable_BezelConformity = _values(i) : i = i + 1
        TestEnable_DecorFrameConformity = _values(i) : i = i + 1
        TestEnable_WriteMMS = _values(i) : i = i + 1
        TestEnable_ReadMMS = _values(i) : i = i + 1
        TestEnable_Temperature = _values(i) : i = i + 1

        TestEnable_FOLDING_Option = _values(i) : i = i + 1
        TestEnable_MEMO_Option = _values(i) : i = i + 1
        TestEnable_PWL_Option = _values(i) : i = i + 1
        TestEnable_EV_Option = _values(i) : i = i + 1
        TestEnable_PWMOutput = _values(i) : i += 1
        TestEnable_SHAPE_ChildrenLock2 = _values(i) : i = i + 1
        TestEnable_BACKLIGHT_ChildrenLock2 = _values(i) : i = i + 1
        TestEnable_PointsOnWili = _values(i) : i = i + 1
        TestEnable_CustomerInterface = _values(i) : i = i + 1
        TestEnable_Adapter = _values(i) : i = i + 1
        TestEnable_Mirrortiltingposition = _values(i) : i = i + 1

        ' General settings																				
        PowerSupplyVoltage = _values(100)
        PowerSupplyCurrentLimit = _values(101)
        '
        Camera_Programm = _values(102)
        Camera_Recipe = _values(103)
        ' Limits on standard signals																				
        PowersupplyVbat = _values(110)
        PowersupplyIbat = _values(111)

        i = 300
        For j = 0 To 20
            Sdt_Signal(j) = _values(i) : i = i + 1
        Next

        i = 330
        DO_UP_Front_Passenger = _values(i) : i = i + 1
        DO_DN_Front_Passenger = _values(i) : i = i + 1
        DO_Switch_Inhibtion = _values(i) : i = i + 1
        DO_DN_Rear_Right = _values(i) : i = i + 1
        DO_UP_Rear_Right = _values(i) : i = i + 1
        DO_DN_Rear_Left = _values(i) : i = i + 1
        DO_UP_Rear_Left = _values(i) : i = i + 1

        i = 340
        'Req 125_2 / 126_2 / 127_1
        DO_CDE_HB_RTRV_G(0) = _values(i) : i += 1
        DO_CDE_HB_RTRV_G(1) = _values(i) : i += 1
        'Req 128_2 / 130_1
        D0_SGN_COMMUN_MOT_RTRV_D = _values(350)
        'Req 136_2 / 132_2 / 137_2 / 133_1 / 138_2 / 139_1
        i = 360
        DO_CDE_DG_RTRV_D(0) = _values(i) : i += 1
        DO_CDE_DG_RTRV_D(1) = _values(i) : i += 1
        ADC_CDE_DG_RTRV_D(0) = _values(i) : i += 1
        ADC_CDE_DG_RTRV_D(1) = _values(i) : i += 1
        'Req 140_2 / 134_2 / 141_2 / 142_1 / 135_2 / 079_1
        i = 370
        DO_CDE_DG_RTRV_G(0) = _values(i) : i += 1
        DO_CDE_DG_RTRV_G(1) = _values(i) : i += 1
        ADC_CDE_DG_RTRV_G(0) = _values(i) : i += 1
        ADC_CDE_DG_RTRV_G(1) = _values(i) : i += 1
        'Req 230_2 / 080_2
        i = 380
        DO_CDE_RBT_RTRV_G(0) = _values(i) : i += 1
        DO_CDE_RBT_RTRV_G(1) = _values(i) : i += 1
        ADC_CDE_RBT_RTRV_G(0) = _values(i) : i += 1
        ADC_CDE_RBT_RTRV_G(1) = _values(i) : i += 1
        'Req 161_2 / 081_2
        i = 390
        DO_CDE_RBT_RTRV_D(0) = _values(i) : i += 1
        DO_CDE_RBT_RTRV_D(1) = _values(i) : i += 1
        ADC_CDE_RBT_RTRV_D(0) = _values(i) : i += 1
        ADC_CDE_RBT_RTRV_D(1) = _values(i) : i += 1
        'Req 333_2 / 168_1
        i = 400
        D0_SGN_COMMUN_MOT_RTRV_G = _values(i) : i += 1

        ' Test Pictogram

        '
        i = 120
        Push_Select_Left_pictogram_Conformity = _values(i) : i = i + 1
        Push_Select_Right_pictogram_Conformity = _values(i) : i = i + 1
        Push_Folding_pictogram_Conformity = _values(i) : i = i + 1
        Push_Adjust_UP_pictogram_Conformity = _values(i) : i = i + 1
        Push_Adjust_Down_pictogram_Conformity = _values(i) : i = i + 1
        Push_Adjust_Left_pictogram_Conformity = _values(i) : i = i + 1
        Push_Adjust_Right_pictogram_Conformity = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_pictogram_Conformity = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_pictogram_Conformity = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_pictogram_Conformity = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_pictogram_Conformity = _values(i) : i = i + 1
        Push_Children_Lock_pictogram_Conformity = _values(i) : i = i + 1
        Bezel_Conformity = _values(i) : i = i + 1
        Decor_Frame_Conformity = _values(i) : i = i + 1
        Push_Children_Lock2_pictogram_Conformity = _values(i) : i = i + 1
        Apater_Front_pictogram_Conformity = _values(i) : i = i + 1
        Apater_Rear_pictogram_Conformity = _values(i) : i = i + 1
        '
        i = 140
        Push_Select_Left_pictogram_Defect_Area = _values(i) : i = i + 1
        Push_Select_Right_pictogram_Defect_Area = _values(i) : i = i + 1
        Push_Folding_pictogram_Defect_Area = _values(i) : i = i + 1
        Push_Adjust_UP_pictogram_Defect_Area = _values(i) : i = i + 1
        Push_Adjust_Down_pictogram_Defect_Area = _values(i) : i = i + 1
        Push_Adjust_Left_pictogram_Defect_Area = _values(i) : i = i + 1
        Push_Adjust_Right_pictogram_Defect_Area = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_pictogram_Defect_Area = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_pictogram_Defect_Area = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_pictogram_Defect_Area = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_pictogram_Defect_Area = _values(i) : i = i + 1
        Push_Children_Lock_pictogram_Defect_Area = _values(i) : i = i + 1
        Push_Children_Lock2_pictogram_Defect_Area = _values(i) : i = i + 1
        '
        i = 153
        Push_Children_Lock2_Intensity = _values(i) : i = i + 1
        Push_Children_Lock2_RSQ = _values(i) : i = i + 1
        Push_Children_Lock2_Polygon_Axy = _values(i) : i = i + 1
        Push_Children_Lock2_Polygon_Bxy = _values(i) : i = i + 1
        Push_Children_Lock2_Polygon_Cxy = _values(i) : i = i + 1
        Push_Children_Lock2_Polygon_Dxy = _values(i) : i = i + 1
        Push_Children_Lock2_minimum_homogeneity = _values(i) : i = i + 1
        Push_Select_Left_Intensity = _values(i) : i = i + 1
        Push_Select_Left_RSQ = _values(i) : i = i + 1
        Push_Select_Left_Polygon_Axy = _values(i) : i = i + 1
        Push_Select_Left_Polygon_Bxy = _values(i) : i = i + 1
        Push_Select_Left_Polygon_Cxy = _values(i) : i = i + 1
        Push_Select_Left_Polygon_Dxy = _values(i) : i = i + 1
        Push_Select_Left_minimum_homogeneity = _values(i) : i = i + 1
        Push_Select_Right_Intensity = _values(i) : i = i + 1
        Push_Select_Right_RSQ = _values(i) : i = i + 1
        Push_Select_Right_Polygon_Axy = _values(i) : i = i + 1
        Push_Select_Right_Polygon_Bxy = _values(i) : i = i + 1
        Push_Select_Right_Polygon_Cxy = _values(i) : i = i + 1
        Push_Select_Right_Polygon_Dxy = _values(i) : i = i + 1
        Push_Select_Right_minimum_homogeneity = _values(i) : i = i + 1
        Push_Folding_Intensity = _values(i) : i = i + 1
        Push_Folding_RSQ = _values(i) : i = i + 1
        Push_Folding_Polygon_Axy = _values(i) : i = i + 1
        Push_Folding_Polygon_Bxy = _values(i) : i = i + 1
        Push_Folding_Polygon_Cxy = _values(i) : i = i + 1
        Push_Folding_Polygon_Dxy = _values(i) : i = i + 1
        Push_Folding_minimum_homogeneity = _values(i) : i = i + 1
        Push_Adjust_UP_Intensity = _values(i) : i = i + 1
        Push_Adjust_UP_RSQ = _values(i) : i = i + 1
        Push_Adjust_UP_Polygon_Axy = _values(i) : i = i + 1
        Push_Adjust_UP_Polygon_Bxy = _values(i) : i = i + 1
        Push_Adjust_UP_Polygon_Cxy = _values(i) : i = i + 1
        Push_Adjust_UP_Polygon_Dxy = _values(i) : i = i + 1
        Push_Adjust_UP_minimum_homogeneity = _values(i) : i = i + 1
        Push_Adjust_DOWN_Intensity = _values(i) : i = i + 1
        Push_Adjust_DOWN_RSQ = _values(i) : i = i + 1
        Push_Adjust_DOWN_Polygon_Axy = _values(i) : i = i + 1
        Push_Adjust_DOWN_Polygon_Bxy = _values(i) : i = i + 1
        Push_Adjust_DOWN_Polygon_Cxy = _values(i) : i = i + 1
        Push_Adjust_DOWN_Polygon_Dxy = _values(i) : i = i + 1
        Push_Adjust_DOWN_minimum_homogeneity = _values(i) : i = i + 1
        Push_Adjust_Left_Intensity = _values(i) : i = i + 1
        Push_Adjust_Left_RSQ = _values(i) : i = i + 1
        Push_Adjust_Left_Polygon_Axy = _values(i) : i = i + 1
        Push_Adjust_Left_Polygon_Bxy = _values(i) : i = i + 1
        Push_Adjust_Left_Polygon_Cxy = _values(i) : i = i + 1
        Push_Adjust_Left_Polygon_Dxy = _values(i) : i = i + 1
        Push_Adjust_Left_minimum_homogeneity = _values(i) : i = i + 1
        Push_Adjust_Right_Intensity = _values(i) : i = i + 1
        Push_Adjust_Right_RSQ = _values(i) : i = i + 1
        Push_Adjust_Right_Polygon_Axy = _values(i) : i = i + 1
        Push_Adjust_Right_Polygon_Bxy = _values(i) : i = i + 1
        Push_Adjust_Right_Polygon_Cxy = _values(i) : i = i + 1
        Push_Adjust_Right_Polygon_Dxy = _values(i) : i = i + 1
        Push_Adjust_Right_minimum_homogeneity = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_Intensity = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_RSQ = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_Polygon_Axy = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_Polygon_Bxy = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_Polygon_Cxy = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_Polygon_Dxy = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_minimum_homogeneity = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_Intensity = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_RSQ = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_Polygon_Axy = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_Polygon_Bxy = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_Polygon_Cxy = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_Polygon_Dxy = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_minimum_homogeneity = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_Intensity = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_RSQ = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_Polygon_Axy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_Polygon_Bxy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_Polygon_Cxy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_Polygon_Dxy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_minimum_homogeneity = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_Intensity = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_RSQ = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_Polygon_Axy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_Polygon_Bxy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_Polygon_Cxy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_Polygon_Dxy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_minimum_homogeneity = _values(i) : i = i + 1
        Push_Children_Lock_Intensity = _values(i) : i = i + 1
        Push_Children_Lock_RSQ = _values(i) : i = i + 1
        Push_Children_Lock_Polygon_Axy = _values(i) : i = i + 1
        Push_Children_Lock_Polygon_Bxy = _values(i) : i = i + 1
        Push_Children_Lock_Polygon_Cxy = _values(i) : i = i + 1
        Push_Children_Lock_Polygon_Dxy = _values(i) : i = i + 1
        Push_Children_Lock_minimum_homogeneity = _values(i) : i = i + 1
        '
        i = 250
        TELLTALE_Select_Left_Intensity = _values(i) : i = i + 1
        TELLTALE_Select_Left_Saturation = _values(i) : i = i + 1
        TELLTALE_Select_Left_WaveLenght = _values(i) : i = i + 1
        TELLTALE_Select_Right_Intensity = _values(i) : i = i + 1
        TELLTALE_Select_Right_Saturation = _values(i) : i = i + 1
        TELLTALE_Select_Right_WaveLenght = _values(i) : i = i + 1
        TELLTALE_Children_Lock_Intensity = _values(i) : i = i + 1
        TELLTALE_Children_Lock_Saturation = _values(i) : i = i + 1
        TELLTALE_Children_Lock_WaveLenght = _values(i) : i = i + 1

        i = 260
        Push_Select_Left_backlight_Defect_Area = _values(i) : i = i + 1
        Push_Select_Right_backlight_Defect_Area = _values(i) : i = i + 1
        Push_Folding_backlight_Defect_Area = _values(i) : i = i + 1
        Push_Adjust_UP_backlight_Defect_Area = _values(i) : i = i + 1
        Push_Adjust_Down_backlight_Defect_Area = _values(i) : i = i + 1
        Push_Adjust_Left_backlight_Defect_Area = _values(i) : i = i + 1
        Push_Adjust_Right_backlight_Defect_Area = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_backlight_Defect_Area = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_backlight_Defect_Area = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_backlight_Defect_Area = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_backlight_Defect_Area = _values(i) : i = i + 1
        Push_Children_Lock_backlight_Defect_Area = _values(i) : i = i + 1
        Push_Children_Lock2_backlight_Defect_Area = _values(i) : i = i + 1

        i = 280
        TELLTALE_Select_Left_Defect_Area = _values(i) : i = i + 1
        TELLTALE_Select_Right_Defect_Area = _values(i) : i = i + 1
        TELLTALE_Children_Lock_Defect_Area = _values(i) : i = i + 1

        i = 500
        PCBA_Number_Reference = _values(i) : i = i + 1
        PCBA_Number_Index = _values(i) : i = i + 1
        PCBA_Plant_Line = _values(i) : i = i + 1
        PCBA_ManufacturingDate = _values(i) : i = i + 1
        PCBA_SerialNumber = _values(i) : i = i + 1
        PCBA_DeviationNumber = _values(i) : i = i + 1
        LED_BIN_PT_White_RSA = _values(i) : i = i + 1
        LED_BIN_PT_RED = _values(i) : i = i + 1
        LED_BIN_PT_Yellow = _values(i) : i = i + 1
        LED_BIN_PT_White_Nissan = _values(i) : i = i + 1
        Major_SoftwareVersion = _values(i) : i = i + 1
        Minor_SoftwareVersion = _values(i) : i = i + 1
        Major_NVMversion = _values(i) : i = i + 1
        Minor_NVMversion = _values(i) : i = i + 1
        SW_checksum = _values(i) : i = i + 1
      
        i = 520
        ADC_UCAD_Variant_1 = _values(i) : i = i + 1
        ADC_UCAD_Variant_2 = _values(i) : i = i + 1

        i = 290
        ADC_CURSEUR_LEFT_RIGHT = _values(i) : i = i + 1
        ADC_CURSEUR_UP_DN = _values(i) : i = i + 1

        i = 530
        'Write Valeo Traceability MMS
        Write_MMS_Final_Product_Reference = _values(i) : i = i + 1
        Write_MMS_Final_Product_Index = _values(i) : i = i + 1
        Write_MMS_Valeo_Final_Product_Plant = _values(i) : i = i + 1
        Write_MMS_Valeo_Final_Product_Line = _values(i) : i = i + 1
        Write_MMS_Deviation_Number = _values(i) : i = i + 1
        Write_CustomerPartNumber = _values(i) : i = i + 1
        Write_HARDWARE_Version = _values(i) : i = i + 1
        Laser_IDCode = _values(i) : i = i + 1

        i = 540
        PowerSupplySleepCurrentLimit = _values(i)

        i = 550
        WRITE_SW_Coding = _values(i) : i += 1
        WRITE_Baclight_Coding = _values(i) : i += 1
        WRITE_HW_Coding = _values(i) : i += 1
        WRITE_BackLight_PWM = _values(i) : i += 1
        WRITE_MLTelltale_PWM = _values(i) : i += 1
        WRITE_MRTelltale_PWM = _values(i) : i += 1
        WRITE_CLTelltale_PWM = _values(i) : i += 1

        i = 560
        For j = 0 To 14
            Correlation_Factor_A(j) = _values(i) : i = i + 1
            Correlation_Factor_B(j) = _values(i) : i = i + 1
        Next

        i = 600
        'External Backlight WLAP
        External_Backlight_WLAP_Voltage_LOW_X1_12 = _values(i) : i = i + 1
        External_Backlight_WLAP_Voltage_HIGH_X1_12 = _values(i) : i = i + 1
        External_Backlight_WLAP_Duty_Cycle_X1_12 = _values(i) : i = i + 1
        External_Backlight_WLAP_Frequency_X1_12 = _values(i) : i = i + 1
        External_Backlight_WLAP_ADC_VALUE = _values(i) : i = i + 1
        i = 610
        'External Backlight DOORLOCK
        External_Backlight_DOOR_LOCK_Voltage_LOW_X1_24 = _values(i) : i = i + 1
        External_Backlight_DOOR_LOCK_Voltage_HIGH_X1_24 = _values(i) : i = i + 1
        External_Backlight_DOOR_LOCK_Duty_Cycle_X1_24 = _values(i) : i = i + 1
        External_Backlight_DOOR_LOCK_Frequency_X1_24 = _values(i) : i = i + 1
        External_Backlight_DOOR_LOCK_ADC_VALUE = _values(i) : i = i + 1

        i = 620
        'External Backlight DOORLOCK
        Correlation_BackColorx_Factor_A = _values(i) : i = i + 1
        Correlation_BackColorx_Factor_B = _values(i) : i = i + 1
        Correlation_BackColory_Factor_A = _values(i) : i = i + 1
        Correlation_BackColory_Factor_B = _values(i) : i = i + 1
        Correlation_TellColorx_Factor_A = _values(i) : i = i + 1
        Correlation_TellColorx_Factor_B = _values(i) : i = i + 1
        Correlation_TellColory_Factor_A = _values(i) : i = i + 1
        Correlation_TellColory_Factor_B = _values(i) : i = i + 1
        Correlation_TellWavelength_Factor_A = _values(i) : i = i + 1
        Correlation_TellWavelength_Factor_B = _values(i) : i = i + 1

        i = 638
        'Back Light CIE1931 E and F.
        Push_Children_Lock2_Polygon_Exy = _values(i) : i = i + 1
        Push_Children_Lock2_Polygon_Fxy = _values(i) : i = i + 1
        Push_Select_Left_Polygon_Exy = _values(i) : i = i + 1
        Push_Select_Left_Polygon_Fxy = _values(i) : i = i + 1
        Push_Select_Right_Polygon_Exy = _values(i) : i = i + 1
        Push_Select_Right_Polygon_Fxy = _values(i) : i = i + 1
        Push_Folding_Polygon_Exy = _values(i) : i = i + 1
        Push_Folding_Polygon_Fxy = _values(i) : i = i + 1
        Push_Adjust_UP_Polygon_Exy = _values(i) : i = i + 1
        Push_Adjust_UP_Polygon_Fxy = _values(i) : i = i + 1
        Push_Adjust_DOWN_Polygon_Exy = _values(i) : i = i + 1
        Push_Adjust_DOWN_Polygon_Fxy = _values(i) : i = i + 1
        Push_Adjust_Left_Polygon_Exy = _values(i) : i = i + 1
        Push_Adjust_Left_Polygon_Fxy = _values(i) : i = i + 1
        Push_Adjust_Right_Polygon_Exy = _values(i) : i = i + 1
        Push_Adjust_Right_Polygon_Fxy = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_Polygon_Exy = _values(i) : i = i + 1
        Windows_Lifter_Front_Left_Polygon_Fxy = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_Polygon_Exy = _values(i) : i = i + 1
        Windows_Lifter_Front_Right_Polygon_Fxy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_Polygon_Exy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Left_Polygon_Fxy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_Polygon_Exy = _values(i) : i = i + 1
        Windows_Lifter_Rear_Right_Polygon_Fxy = _values(i) : i = i + 1
        Push_Children_Lock_Polygon_Exy = _values(i) : i = i + 1
        Push_Children_Lock_Polygon_Fxy = _values(i) : i = i + 1
        i = 670
        'TellTale CIE1931 ABCD.
        TELLTALE_Select_Left_Polygon_Axy = _values(i) : i = i + 1
        TELLTALE_Select_Left_Polygon_Bxy = _values(i) : i = i + 1
        TELLTALE_Select_Left_Polygon_Cxy = _values(i) : i = i + 1
        TELLTALE_Select_Left_Polygon_Dxy = _values(i) : i = i + 1
        TELLTALE_Select_Right_Polygon_Axy = _values(i) : i = i + 1
        TELLTALE_Select_Right_Polygon_Bxy = _values(i) : i = i + 1
        TELLTALE_Select_Right_Polygon_Cxy = _values(i) : i = i + 1
        TELLTALE_Select_Right_Polygon_Dxy = _values(i) : i = i + 1
        TELLTALE_Children_Lock_Polygon_Axy = _values(i) : i = i + 1
        TELLTALE_Children_Lock_Polygon_Bxy = _values(i) : i = i + 1
        TELLTALE_Children_Lock_Polygon_Cxy = _values(i) : i = i + 1
        TELLTALE_Children_Lock_Polygon_Dxy = _values(i) : i = i + 1

        i = 700
        PointsOn_Front_Left = _values(i) : i = i + 1
        PointsOn_Front_Right = _values(i) : i = i + 1
        PointsOn_Rear_Left = _values(i) : i = i + 1
        PointsOn_Rear_Right = _values(i) : i = i + 1
        i = 710
        For index = 0 To 19
            CustomerInterface(index) = _values(i + index)
        Next
        'i = 730
        i = 740
        Ring_Red = _values(i) : i = i + 1
        Ring_Green = _values(i) : i = i + 1
        Ring_Blue = _values(i) : i = i + 1

    End Function



    Public Shared Function ReadList(ByRef names() As String) As Boolean
        ' Read the recipe list
        ReadList = mRecipe.ReadList(mConstants.BasePath & mWS02Main.Settings.RecipePath, names)
    End Function



    Public Function Save(ByVal name As String) As Boolean
        ' Save the recipe
        Save = mRecipe.Save(FilePath(name), _values)
    End Function

    Public Property Value(ByVal index As Integer) As cRecipeValue
        Get
            Value = _values(index)
        End Get
        Set(ByVal value As cRecipeValue)
            _values(index) = value
        End Set
    End Property


    Public Sub SetToDefaultValues()
        ' Set all the values to the default ones
        mRecipe.SetToDefaultValues(_values)
    End Sub



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Shared Function FilePath(ByVal name As String) As String
        If MasterReference = True Then
            ' Return the file path
            FilePath = mConstants.BasePath & mWS02Main.Settings.MasterReferencePath & "\M" & name & ".txt"
        Else
            ' Return the file path
            FilePath = mConstants.BasePath & mWS02Main.Settings.RecipePath & "\" & name & ".txt"
        End If
    End Function
End Class
