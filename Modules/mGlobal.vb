Option Explicit On
Option Strict On

Imports System
Imports System.IO

Module mGlobal
   
    Public Enum eLog_File
        Frame_All = 0
        Frame_tx_A = 1
        Frame_tx_0 = 2
        Frame_tx_3C = 3
        Frame_rx_A = 4
        Frame_rx_0 = 5
        Frame_rx_9 = 6
        Frame_rx_3D = 7
        Frame_rx_3C = 8

    End Enum

    Public Enum eSignal_Analog
        EarlySensor = 0
        StrenghtSensor = 1
    End Enum

    Public Enum eTTI_Output
        WS02 = 1
        WS03 = 2
        WS04 = 3
        WS05 = 4
    End Enum

    Public Enum ePush_Pull
        Push = 0
        Pull = 1
        ND = 2
    End Enum

    Public Enum eElec
        Off = 0
        Ce1 = 1
        Ce2 = 2
    End Enum
    ' LIN frame enumeration
    Public Enum eLINFrame
        '// Functional Frame		
        '// Send EOL to DUT		
        BCM_LINDOOR_A_01 = 0
        BCM_LINDOOR_R_02 = 2
        '// Send DUT to EOL		
        DPC_LINDOOR_R_01 = 4
        '// Diagnostique Frame		
        DIAG_Req_Diag_1060 = 10
        DIAG_Rep_Diag_1060 = 11
        DIAG_Req_ReadKey = 12
        DIAG_Rep_ReadKey = 13
        DIAG_Req_SendKey = 14
        DIAG_Rep_SendKey = 15
        DIAG_Req_NRC_SVDNET = 16
        DIAG_Rep_NRC_10 = 17
        DIAG_Rep_NRC_22 = 18
        DIAG_Rep_NRC_78 = 19
        DIAG_Rep_NRC_21 = 20
        DIAG_Rep_NRC_70 = 21
        DIAG_Req_Empty = 22
        DIAG_Rep_Empty = 23
        DIAG_Req_OpenDiag_1070 = 24
        DIAG_Rep_OpenDiag_1070 = 25
        DIAG_Req_TesterPresent = 26
        DIAG_Rep_TesterPresent = 27
        DIAG_Req_FlowCtrl = 28
        DIAG_Rep_FlowCtrl = 29
        '// ECU RESET		
        DIAG_Req_ECU_Reset = 30
        DIAG_Rep_ECU_Reset = 31
        '// Present Values		
        DIAG_Req_Read_ALL_DIN = 40
        DIAG_Rep_Read_ALL_DIN = 41
        DIAG_Req_Read_All_AIN = 42
        DIAG_Rep_Read_All_AIN = 43
        '//SW and Variant Version	0xBA00	
        DIAG_Req_SW_Version = 50
        DIAG_Rep_SW_Version = 51
        DIAG_Req_SW_Checksum = 52
        DIAG_Rep_SW_Checksum = 53
        DIAG_Req_SW_Coding = 54
        DIAG_Rep_SW_Coding = 55
        '// Analog VBAT		
        DIAG_Req_AI_Variant_1 = 60
        DIAG_Rep_AI_Variant_1 = 61
        DIAG_Req_AI_Variant_2 = 62
        DIAG_Rep_AI_Variant_2 = 63
        '//EMS_ICT BYTE	0xFD11	
        DIAG_Req_EMSICTByte = 70
        DIAG_Rep_EMSICTByte = 71
        '//EMS_Traceability	0xFD10	
        DIAG_Req_EMSTracea = 80
        DIAG_Rep_EMSTracea = 81
        '//EMS_Traceability	0xFE30	
        DIAG_Req_EMSCalibration = 90
        DIAG_Rep_EMSCalibration = 91
        DIAG_Req_EMS_LED_RSA = 92
        DIAG_Rep_EMS_LED_RSA = 93
        DIAG_Req_EMS_LED_RED = 94
        DIAG_Rep_EMS_LED_RED = 95
        DIAG_Req_EMS_LED_Yellow = 96
        DIAG_Rep_EMS_LED_Yellow = 97
        DIAG_Req_EMS_LED_Nissan = 98
        DIAG_Rep_EMS_LED_Nissan = 99
        '//MMS_Traceability	0xFD21	
        DIAG_Req_MMS_R_TestByte = 110
        DIAG_Rep_MMS_R_TestByte = 111
        DIAG_Req_MMS_W_TestByte = 112
        DIAG_Rep_MMS_W_TestByte = 113
        '//MMS_Traceability	0xFD20	
        DIAG_Req_MMS_R_Tracea = 120
        DIAG_Rep_MMS_R_Tracea = 121
        '//MMS_Traceability	0xFD20	
        DIAG_Req_MMS_W_Tracea = 122
        DIAG_Rep_MMS_W_Tracea = 123
        '// Haptic Service for AIN		
        DIAG_Req_Start_Haptic_AIN = 130
        DIAG_Rep_Start_Haptic_AIN = 131
        DIAG_Req_Stop_Haptic_AIN = 132
        DIAG_Rep_Stop_Haptic_AIN = 133
        DIAG_Req_RawTransition_AIN = 134
        DIAG_Rep_RawTransition_AIN = 135
        '// Haptic Service for DIN		
        DIAG_Req_Start_Haptic_DIN = 140
        DIAG_Rep_Start_Haptic_DIN = 141
        DIAG_Req_Stop_Haptic_DIN = 142
        DIAG_Rep_Stop_Haptic_DIN = 143
        DIAG_Req_RawTransition_DIN = 144
        DIAG_Rep_RawTransition_DIN = 145
        '// Backlight 		
        DIAG_Req_Backlight = 180
        DIAG_Rep_Backlight = 181
        '// TellTale		
        DIAG_Req_TellTale = 182
        DIAG_Rep_TellTale = 183
        '// Illumination switch OFF		
        DIAG_Req_All_OFF = 184
        DIAG_Rep_All_OFF = 185
        '// Illumination switch ON		
        DIAG_Req_ALL_On = 186
        DIAG_Rep_ALL_ON = 187
        '// Backlight 		 EV
        DIAG_Req_Backlight_EV = 188
        DIAG_Rep_Backlight_EV = 189
        DIAG_Req_Backlight2_EV = 190
        '// TellTale		
        DIAG_Req_TellTale_EV = 191
        DIAG_Rep_TellTale_EV = 192
        DIAG_Req_TellTale2_EV = 193
        '// Illumination switch OFF		
        DIAG_Req_All_OFF_EV = 194
        DIAG_Rep_All_OFF_EV = 195
        DIAG_Req_All_OFF2_EV = 196
        '// Illumination switch ON		
        DIAG_Req_ALL_On_EV = 197
        DIAG_Rep_ALL_ON_EV = 198
        DIAG_Req_ALL_On2_EV = 199
        '// Go to OFF Mode		
        DIAG_Req_Go_OFF_Mode = 200
        DIAG_Rep_Go_OFF_Mode = 201
        '// Read ADC 		
        DIAG_Req_ADC_Curseur = 210
        DIAG_Rep_ADC_Curseur = 211
        '//Activate_NCV7754_Chip_outputs		
        DIAG_Req_Act_NCV7754 = 220
        DIAG_Rep_Act_NCV7754 = 221
        DIAG_Req_DAct_NCV7754 = 222
        DIAG_Rep_DAct_NCV7754 = 223
        '//Activate_NCV7703_Chip_outputs_U2201		
        DIAG_Req_Act_NCV7703_U2201 = 224
        DIAG_Rep_Act_NCV7703_U2201 = 225
        DIAG_Req_DAct_NCV7703_U2201 = 226
        DIAG_Rep_DAct_NCV7703_U2201 = 227
        '//Activate_NCV7703_Chip_outputs_U2301		
        DIAG_Req_Act_NCV7703_U2301 = 228
        DIAG_Rep_Act_NCV7703_U2301 = 229
        DIAG_Req_DAct_NCV7703_U2301 = 230
        DIAG_Rep_DAct_NCV7703_U2301 = 231
        '//Write_DOUT		
        DIAG_Req_Write_DOUT_H = 240
        DIAG_Rep_Write_DOUT_H = 241
        DIAG_Req_Write_DOUT_L = 242
        DIAG_Rep_Write_DOUT_L = 243
        '// RW customer configuration
        DIAG_Req_Write_SW_Coding = 250
        DIAG_Rep_Write_SW_Coding = 251
        DIAG_Req_Write_Backlight_Coding = 252
        DIAG_Rep_Write_Backlight_Coding = 253
        DIAG_Req_Read_SW_Coding = 254
        DIAG_Rep_Read_SW_Coding = 255
        DIAG_Req_Read_Backlight_Coding = 256
        DIAG_Rep_Read_Backlight_Coding = 257

        DIAG_Req_Write_EV_HW_Coding = 260
        DIAG_Rep_Write_EV_HW_Coding = 261
        DIAG_Req_Read_EV_HW_Coding = 262
        DIAG_Rep_Read_EV_HW_Coding = 263

        DIAG_Req_Write_Temperature = 270
        DIAG_Rep_Write_Temperature = 271
        DIAG_Req_Read_Temperature = 272
        DIAG_Rep_Read_Temperature = 273

        '// PWM switch OFF EV
        DIAG_Req_PWM_OFF = 280
        DIAG_Rep_PWM_OFF = 281
        DIAG_Req_PWM_OFF_2 = 282
        '// PWM WLAP switch ON EV
        DIAG_Req_PWM_WLAP_ON = 290
        DIAG_Rep_PWM_WLAP_ON = 291
        DIAG_Req_PWM_WLAP_ON_2 = 292
        DIAG_Req_ADC_WLAP = 294
        DIAG_Rep_ADC_WLAP = 295
        '// PWM Door Lock switch ON EV
        DIAG_Req_PWM_DoorL_ON = 300
        DIAG_Rep_PWM_DoorL_ON = 301
        DIAG_Req_PWM_DoorL_ON_2 = 302
        DIAG_Req_ADC_DoorL = 304
        DIAG_Rep_ADC_DoorL = 305


    End Enum

    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    Public _powerSupply_1 As New cTTIPowerSupply
    Public _powerSupply_2 As New cTTIPowerSupply
    Public FormDUTStats As New frmDUTStats

    Public tUser As Date

    Public ParameterChecked(1000) As String
    Public ReferenceChecked(500) As String
    Public CopyWS02 As Boolean
    Public CopyWS03 As Boolean
    Public CopyWS04 As Boolean
    Public CopyWS05 As Boolean
    Public MasterReference As Boolean
    Public GlobalRecipeSettings As New cGlobalRecipeSettings
    Public CurrentSampleType As EnumSampleType = EnumSampleType.Production

    Public Separateur As String = System.Globalization.CultureInfo.InstalledUICulture.NumberFormat.NumberDecimalSeparator

    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants

    ' Private variables
    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+

    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function PowerUP() As Boolean
        Dim e As Boolean
        Dim r As Boolean

        ' Load the global settings
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Load global settings... ")
            e = mSettings.Load
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while loading the global settings: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUP = e

        ' Power-up the user manager
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-up the user manager... ")
            e = mUserManager.PowerUp(mSettings.UsersConfigurationPath)
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-up of the user manager: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUP = PowerUP Or e


        ' Power-up the DIO manager
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-up the digital I/O manager... ")
            If mConstants.HardwareEnabled_VIPA Then
                e = mDIOManager.PowerUp
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-up of the digital I/O manager: retry"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUP = PowerUP Or e

        ' Connect and initialize the power supply
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Connect and initialize the station power supply 1... ")
        Do
            If mConstants.HardwareEnabled_TTI Then
                e = _powerSupply_1.Connect(mSettings.PowerSupplyIPaddress_1, cTTIPowerSupply.DefaultPortNumber) OrElse _
                    _powerSupply_1.SetMaxCurrent(1, 2) Or _
                    _powerSupply_1.SetVoltage(1, 13.5) Or _
                    _powerSupply_1.SetMaxCurrent(2, 2) Or _
                    _powerSupply_1.SetVoltage(2, 13.5) 'Or _
                '_powerSupply_1.SetMaxCurrent(3, 2) Or _
                '_powerSupply_1.SetVoltage(3, 13.5)

                e = e Or _powerSupply_1.OutputOn(1) Or _
                    _powerSupply_1.OutputOn(2) 'Or _
                '_powerSupply_1.OutputOn(3)
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while connecting the station power supply 1: retry?"
                frmMessage.Retry = True
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUP = PowerUP Or e

        ' Connect and initialize the power supply
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Connect and initialize the station power supply 2 ... ")
        Do
            If mConstants.HardwareEnabled_TTI Then
                e = _powerSupply_2.Connect(mSettings.PowerSupplyIPaddress_2, cTTIPowerSupply.DefaultPortNumber) OrElse _
                    _powerSupply_2.SetMaxCurrent(1, 2) Or _
                    _powerSupply_2.SetVoltage(1, 13.5) Or _
                    _powerSupply_2.SetMaxCurrent(2, 2) Or _
                    _powerSupply_2.SetVoltage(2, 13.5) ' Or _
                ' _powerSupply_2.SetMaxCurrent(3, 2) Or _
                '_powerSupply_2.SetVoltage(3, 13.5)

                e = e Or _powerSupply_2.OutputOn(1) Or _
                        _powerSupply_2.OutputOn(2) ' Or _
                '_powerSupply_2.OutputOn(3)
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while connecting the station power supply 2: retry?"
                frmMessage.Retry = True
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUP = PowerUP Or e

        ' Connect to the PLC
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Connect to the PLC... ")
            If HardwareEnabled_PLC Then
                e = mPLC.Connect(mSettings.PLCIPAddress)
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while connecting the PLC: retry"
                frmMessage.Retry = True
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerUP = PowerUP Or e

        'mWS03Main.ForcePartOk = True
        'mWS04Main.ForcePartOk = True
        'mWS05Main.ForcePartOk = True


    End Function

    Public Function PowerDown() As Boolean
        Dim e As Boolean
        Dim r As Boolean


        ' Power-down the DIO manager
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power down the digital I/O manager... ")
            If HardwareEnabled_VIPA Then
                e = mDIOManager.PowerDown
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error in the power-down of the digital I/O manager: retry"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = e

        ' Disconnect the power supply
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Disconnect the station power supply 1... ")
            If HardwareEnabled_TTI Then
                e = _powerSupply_1.OutputOff(1) Or _
                    _powerSupply_1.SetVoltage(1, 0) Or _
                    _powerSupply_1.OutputOff(2) Or _
                    _powerSupply_1.SetVoltage(2, 0) Or _
                    _powerSupply_1.OutputOff(3) Or _
                    _powerSupply_1.SetVoltage(3, 0) Or _
                    _powerSupply_1.Disconnect
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while disconnecting the station power supply 1: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = PowerDown Or e

        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Disconnect the station power supply 2... ")
            If HardwareEnabled_TTI Then
                e = _powerSupply_2.OutputOff(1) Or _
                    _powerSupply_2.SetVoltage(1, 0) Or _
                    _powerSupply_2.OutputOff(2) Or _
                    _powerSupply_2.SetVoltage(2, 0) Or _
                    _powerSupply_2.OutputOff(3) Or _
                    _powerSupply_2.SetVoltage(3, 0) Or _
                    _powerSupply_2.Disconnect
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while disconnecting the station power supply 2: retry?"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = PowerDown Or e

        ' Connect to the PLC
        Do
            frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- DisConnect to the PLC... ")
            If HardwareEnabled_PLC Then
                e = mPLC.Disconnect
            End If
            If (e = False) Then
                frmLog.WriteLine("succeeded")
            Else
                frmLog.WriteLine("failed")
                frmMessage.MessageType = frmMessage.eType.Critical
                frmMessage.MessageButtons = frmMessage.eButtons.YesNo
                frmMessage.Message = "Error while Dicconnecting the PLC: retry"
                frmMessage.ShowDialog()
                r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
            End If
        Loop Until (e = False Or r = False)
        PowerDown = PowerDown Or e

        ' Power-down the user manager
        frmLog.Write(Format(Date.Now, "dd/MM/yyyy, HH:mm:ss") & "- Power-down the user manager... ")
        e = mUserManager.PowerDown
        If (e = False) Then
            frmLog.WriteLine("succeeded")
        Else
            frmLog.WriteLine("failed")
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.YesNo
            frmMessage.Message = "Error in the power-down of the user manager: retry"
            frmMessage.ShowDialog()
            r = (frmMessage.MessageChoice = frmMessage.eChoice.Yes)
        End If
        PowerDown = PowerDown Or e

    End Function

End Module
