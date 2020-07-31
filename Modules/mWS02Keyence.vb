Option Explicit On
Option Strict On

Imports System
Imports System.IO

Module mWS02Keyence

    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Pictogram test results structure
    Public Structure sCameraTestResults
        Public MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR As Integer
        Public MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR As Integer
        Public MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR As Integer
        Public MINIMUM_CONFORMITY_PUSH_ADJUST_UP As Integer
        Public MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN As Integer
        Public MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT As Integer
        Public MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT As Integer
        Public MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT As Integer
        Public MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT As Integer
        Public MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT As Integer
        Public MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT As Integer
        Public MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK As Integer
        Public MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2 As Integer
        Public RING_RED As Integer
        Public RING_GREEN As Integer
        Public RING_BLUE As Integer
        Public CONFORMITY_Bezel As Integer
        Public CONFORMITY_Decor_Frame As Integer

        Public DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR As Integer
        Public DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR As Integer
        Public DEFECT_AREA_PUSH_FOLDING_MIRROR As Integer
        Public DEFECT_AREA_PUSH_ADJUST_UP As Integer
        Public DEFECT_AREA_PUSH_ADJUST_DOWN As Integer
        Public DEFECT_AREA_PUSH_ADJUST_LEFT As Integer
        Public DEFECT_AREA_PUSH_ADJUST_RIGHT As Integer
        Public DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT As Integer
        Public DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT As Integer
        Public DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT As Integer
        Public DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT As Integer
        Public DEFECT_AREA_PUSH_CHILDREN_LOCK As Integer
        Public DEFECT_AREA_PUSH_CHILDREN_LOCK2 As Integer

        Public PontsOnWili_FrontLeft As Integer
        Public PontsOnWili_FrontRight As Integer
        Public PontsOnWili_RearLeft As Integer
        Public PontsOnWili_RearRight As Integer

        Public MINIMUM_CONFORMITY_Shape_Adapter_Front As Integer
        Public MINIMUM_CONFORMITY_Shape_Adapter_Rear As Integer

        Public Push_SELECT_LEFT_backlight_intensity As Integer
        Public Push_SELECT_LEFT_backlight_deviation As Integer
        Public Push_SELECT_LEFT_backlight_red As Integer
        Public Push_SELECT_LEFT_backlight_green As Integer
        Public Push_SELECT_LEFT_backlight_blue As Integer

        Public Push_SELECT_RIGHT_backlight_intensity As Integer
        Public Push_SELECT_RIGHT_backlight_deviation As Integer
        Public Push_SELECT_RIGHT_backlight_red As Integer
        Public Push_SELECT_RIGHT_backlight_green As Integer
        Public Push_SELECT_RIGHT_backlight_blue As Integer

        Public Push_FOLDING_backlight_intensity As Integer
        Public Push_FOLDING_backlight_deviation As Integer
        Public Push_FOLDING_backlight_red As Integer
        Public Push_FOLDING_backlight_green As Integer
        Public Push_FOLDING_backlight_blue As Integer

        Public Push_ADJUST_UP_backlight_intensity As Integer
        Public Push_ADJUST_UP_backlight_deviation As Integer
        Public Push_ADJUST_UP_backlight_red As Integer
        Public Push_ADJUST_UP_backlight_green As Integer
        Public Push_ADJUST_UP_backlight_blue As Integer

        Public Push_ADJUST_DOWN_backlight_intensity As Integer
        Public Push_ADJUST_DOWN_backlight_deviation As Integer
        Public Push_ADJUST_DOWN_backlight_red As Integer
        Public Push_ADJUST_DOWN_backlight_green As Integer
        Public Push_ADJUST_DOWN_backlight_blue As Integer

        Public Push_ADJUST_LEFT_backlight_intensity As Integer
        Public Push_ADJUST_LEFT_backlight_deviation As Integer
        Public Push_ADJUST_LEFT_backlight_red As Integer
        Public Push_ADJUST_LEFT_backlight_green As Integer
        Public Push_ADJUST_LEFT_backlight_blue As Integer

        Public Push_ADJUST_RIGHT_backlight_intensity As Integer
        Public Push_ADJUST_RIGHT_backlight_deviation As Integer
        Public Push_ADJUST_RIGHT_backlight_red As Integer
        Public Push_ADJUST_RIGHT_backlight_green As Integer
        Public Push_ADJUST_RIGHT_backlight_blue As Integer

        Public WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity As Integer
        Public WINDOWS_LIFTER_FRONT_LEFT_backlight_deviation As Integer
        Public WINDOWS_LIFTER_FRONT_LEFT_backlight_red As Integer
        Public WINDOWS_LIFTER_FRONT_LEFT_backlight_green As Integer
        Public WINDOWS_LIFTER_FRONT_LEFT_backlight_blue As Integer

        Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity As Integer
        Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_deviation As Integer
        Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_red As Integer
        Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_green As Integer
        Public WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue As Integer

        Public WINDOWS_LIFTER_REAR_LEFT_backlight_intensity As Integer
        Public WINDOWS_LIFTER_REAR_LEFT_backlight_deviation As Integer
        Public WINDOWS_LIFTER_REAR_LEFT_backlight_red As Integer
        Public WINDOWS_LIFTER_REAR_LEFT_backlight_green As Integer
        Public WINDOWS_LIFTER_REAR_LEFT_backlight_blue As Integer

        Public WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity As Integer
        Public WINDOWS_LIFTER_REAR_RIGHT_backlight_deviation As Integer
        Public WINDOWS_LIFTER_REAR_RIGHT_backlight_red As Integer
        Public WINDOWS_LIFTER_REAR_RIGHT_backlight_green As Integer
        Public WINDOWS_LIFTER_REAR_RIGHT_backlight_blue As Integer

        Public Push_CHILDREN_LOCK_backlight_intensity As Integer
        Public Push_CHILDREN_LOCK_backlight_deviation As Integer
        Public Push_CHILDREN_LOCK_backlight_red As Integer
        Public Push_CHILDREN_LOCK_backlight_green As Integer
        Public Push_CHILDREN_LOCK_backlight_blue As Integer

        Public Push_CHILDREN_LOCK2_backlight_intensity As Integer
        Public Push_CHILDREN_LOCK2_backlight_deviation As Integer
        Public Push_CHILDREN_LOCK2_backlight_red As Integer
        Public Push_CHILDREN_LOCK2_backlight_green As Integer
        Public Push_CHILDREN_LOCK2_backlight_blue As Integer

        Public Push_SELECT_LEFT_LED_intensity As Integer
        Public Push_SELECT_LEFT_LED_deviation As Integer
        Public Push_SELECT_LEFT_LED_red As Integer
        Public Push_SELECT_LEFT_LED_green As Integer
        Public Push_SELECT_LEFT_LED_blue As Integer

        Public Push_SELECT_RIGHT_LED_intensity As Integer
        Public Push_SELECT_RIGHT_LED_deviation As Integer
        Public Push_SELECT_RIGHT_LED_red As Integer
        Public Push_SELECT_RIGHT_LED_green As Integer
        Public Push_SELECT_RIGHT_LED_blue As Integer

        Public Push_CHILDREN_LOCK_LED_intensity As Integer
        Public Push_CHILDREN_LOCK_LED_deviation As Integer
        Public Push_CHILDREN_LOCK_LED_red As Integer
        Public Push_CHILDREN_LOCK_LED_green As Integer
        Public Push_CHILDREN_LOCK_LED_blue As Integer

        Public DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR As Integer
        Public DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR As Integer
        Public DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR As Integer
        Public DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP As Integer
        Public DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN As Integer
        Public DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT As Integer
        Public DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT As Integer
        Public DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT As Integer
        Public DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT As Integer
        Public DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT As Integer
        Public DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT As Integer
        Public DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK As Integer
        Public DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2 As Integer

        Public DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR As Integer
        Public DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR As Integer
        Public DEFECT_AREA_TELLTALE_CHILDREN_LOCK As Integer

        Public Customer_interface_1 As Integer
        Public Customer_interface_2 As Integer
        Public Customer_interface_3 As Integer
        Public Customer_interface_4 As Integer
        Public Customer_interface_5 As Integer
        Public Customer_interface_6 As Integer
        Public Customer_interface_7 As Integer
        Public Customer_interface_8 As Integer
        Public Customer_interface_9 As Integer
        Public Customer_interface_10 As Integer
        Public Customer_interface_11 As Integer
        Public Customer_interface_12 As Integer
        Public Customer_interface_13 As Integer
        Public Customer_interface_14 As Integer
        Public Customer_interface_15 As Integer
        Public Customer_interface_16 As Integer
        Public Customer_interface_17 As Integer
        Public Customer_interface_18 As Integer
        Public Customer_interface_19 As Integer
        Public Customer_interface_20 As Integer

    End Structure

    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _timeout_s = 2

    ' Private variables
    Private _connected As Boolean
    Private _portNumber As Integer
    Private _TCPClient As New cTCPClient

    Private Cie_Data(0 To 15, 0 To 500) As Single


    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+

    Public ReadOnly Property Connected As Boolean
        Get
            Connected = _connected
        End Get
    End Property



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function Connect(ByVal IPAddress As String, _
                            ByVal portNumber As Integer) As Boolean
        Dim e As Boolean
        Dim t0 As Date

        ' Clear the error flag
        e = False
        ' If necessary disconnect the client
        If (_TCPClient.Connected) Then
            _TCPClient.Disconnect()
        End If
        ' Connect the camera
        e = _TCPClient.Connect(IPAddress, portNumber)
        Do
            ' Application events
            Application.DoEvents()
        Loop Until (_TCPClient.Connected Or _
                    (Date.Now - t0).TotalSeconds > _timeout_s)

        e = (_TCPClient.Connected <> True)

        ' If no errors happened
        If (e = False) Then
            ' Set the flag of camera connected
            _connected = True
            ' Return False
            Connect = False
        Else    ' Otherwise, if some error happened
            ' Reset the flag of camera connected
            _connected = False
            ' Return False
            Connect = True
        End If
    End Function



    Public Function Disconnect() As Boolean
        ' Disconnect the client
        _TCPClient.Disconnect()
        ' Reset the flag of camera connected
        _connected = False
        ' Return False
        Disconnect = False
    End Function



    Public Sub EmptyBuffer()
        Dim rxBuffer As String
        ' Reads all the received characters
        rxBuffer = ""
        _TCPClient.Receive(rxBuffer)
    End Sub



    Public Function LoadProgram(ByVal intProgram As Integer) As Boolean
        ' If the camera is connected
        If (_connected) Then
            ' Send the command to the camera
            LoadProgram = _TCPClient.Transmit("PW,1," & CStr(intProgram) & vbCrLf)
        Else    ' Otherwise, if the camera is disconnected
            ' Return True
            LoadProgram = True
        End If
    End Function


    Public Function LoadProgramACK() As Integer
        Dim DataReturn As String

        ' If the camera is connected
        If (_connected) Then
            ' clear data return
            DataReturn = ""
            ' Read the input buffer
            _TCPClient.Receive(DataReturn)
            If Mid(DataReturn, 1, 2) = "PW" Then
                LoadProgramACK = 0
            ElseIf Mid(DataReturn, 1, 2) = "ER" Then
                LoadProgramACK = 2
            Else
                LoadProgramACK = 1
            End If
        Else    ' Otherwise, if the camera is disconnected
            ' Return True
            LoadProgramACK = -1
        End If
    End Function

    Public Function LoadRecipe(ByVal intRecipe As Integer) As Boolean
        ' If the camera is connected
        If (_connected) Then
            ' Send the command to the camera
            LoadRecipe = _TCPClient.Transmit("RPW," & CStr(intRecipe) & ",1" & vbCrLf)
        Else    ' Otherwise, if the camera is disconnected
            ' Return True
            LoadRecipe = True
        End If
    End Function


    Public Function LoadRecipeACK() As Integer
        Dim DataReturn As String

        ' If the camera is connected
        If (_connected) Then
            ' clear data return
            DataReturn = ""
            ' Read the input buffer
            _TCPClient.Receive(DataReturn)
            If Mid(DataReturn, 1, 3) = "RPW" Then
                LoadRecipeACK = 0
            ElseIf Mid(DataReturn, 1, 2) = "ER" Then
                LoadRecipeACK = 2
            Else
                LoadRecipeACK = 1
            End If
        Else    ' Otherwise, if the camera is disconnected
            ' Return True
            LoadRecipeACK = -1
        End If
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+

    Public Function ReadResultsFHM(ByRef udtResults As sCameraTestResults) As Integer
        Dim e As Boolean
        Dim i As Integer
        Dim j As Integer
        Dim s As String
        Dim s1 As String
        Dim t0 As Date
        Dim rxBuffer As String
        Dim rxString As String
        Dim errdes As String
        ' Enables the error handling
        On Error GoTo ErrorHandling

        ReadResultsFHM = 1

        If (_connected) Then
            rxString = ""
            t0 = Date.Now
            Do
                rxBuffer = ""
                e = e Or _TCPClient.Receive(rxBuffer)
                If rxBuffer = "" Then
                    ReadResultsFHM = 1
                    Exit Do
                Else
                    rxString = rxString & rxBuffer
                End If

            Loop Until (rxString.Contains(vbCr) Or (Date.Now - t0).TotalSeconds > _timeout_s Or e)



            If rxString <> "" Then
                mWS02Main.WS02_Addlog(rxString)
                ' Push
                udtResults.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_PUSH_ADJUST_UP = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.RING_RED = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.RING_GREEN = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.RING_BLUE = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_PUSH_FOLDING_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_PUSH_ADJUST_UP = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_PUSH_ADJUST_DOWN = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_PUSH_ADJUST_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_PUSH_ADJUST_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_PUSH_CHILDREN_LOCK = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_PUSH_CHILDREN_LOCK2 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_SELECT_LEFT_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_LEFT_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_LEFT_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_LEFT_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_SELECT_RIGHT_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_RIGHT_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_RIGHT_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_RIGHT_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_FOLDING_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_FOLDING_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_FOLDING_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_FOLDING_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_ADJUST_UP_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_UP_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_UP_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_UP_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_ADJUST_DOWN_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_DOWN_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_DOWN_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_DOWN_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_ADJUST_LEFT_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_LEFT_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_LEFT_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_LEFT_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_ADJUST_RIGHT_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_RIGHT_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_RIGHT_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_RIGHT_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_FRONT_LEFT_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_FRONT_LEFT_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_FRONT_LEFT_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_FRONT_RIGHT_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_FRONT_RIGHT_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_REAR_LEFT_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_REAR_LEFT_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_REAR_LEFT_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_REAR_RIGHT_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_REAR_RIGHT_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_REAR_RIGHT_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_CHILDREN_LOCK_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_CHILDREN_LOCK2_backlight_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK2_backlight_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK2_backlight_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK2_backlight_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_SELECT_LEFT_LED_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_LEFT_LED_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_LEFT_LED_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_LEFT_LED_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_SELECT_RIGHT_LED_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_RIGHT_LED_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_RIGHT_LED_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_RIGHT_LED_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Push_CHILDREN_LOCK_LED_intensity = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK_LED_red = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK_LED_green = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK_LED_blue = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)


                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.DEFECT_AREA_TELLTALE_CHILDREN_LOCK = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.PontsOnWili_FrontLeft = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.PontsOnWili_FrontRight = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.PontsOnWili_RearLeft = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.PontsOnWili_RearRight = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_Shape_Adapter_Front = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.MINIMUM_CONFORMITY_Shape_Adapter_Rear = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)

                udtResults.Customer_interface_1 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_2 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_3 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_4 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_5 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_6 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_7 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_8 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_9 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_10 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_11 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_12 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_13 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_14 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_15 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_16 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_17 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_18 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_19 = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Customer_interface_20 = CInt(Left(rxString, InStr(rxString, ",") - 1))

                udtResults.Push_SELECT_LEFT_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_SELECT_RIGHT_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_FOLDING_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_UP_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_DOWN_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_LEFT_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_ADJUST_RIGHT_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_FRONT_LEFT_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_FRONT_RIGHT_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_REAR_LEFT_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.WINDOWS_LIFTER_REAR_RIGHT_backlight_deviation = CInt(Left(rxString, InStr(rxString, ",") - 1))
                rxString = Mid(rxString, InStr(rxString, ",") + 1)
                udtResults.Push_CHILDREN_LOCK_backlight_deviation = CInt(rxString)
                'rxString = Mid(rxString, InStr(rxString, ",") + 1)
                'udtResults.Push_CHILDREN_LOCK2_backlight_deviation = CInt(rxString)




                ''Decors Frame  intensity
                'udtResults.CONFORMITY_Decor_Frame = CInt(Left(rxString, InStr(rxString, ",") - 1))
                'rxString = Mid(rxString, InStr(rxString, ",") + 1)
                ''Bezel intensity
                'udtResults.CONFORMITY_Bezel = CInt(rxString)
                ' Returns 0
                ReadResultsFHM = 0
            End If
        Else    ' Otherwise, if the camera is disconnected
            ' Return True
            ReadResultsFHM = -1
        End If

        ' Otherwise
        Exit Function

        ' Error handling
ErrorHandling:
        errdes = Err.Description
        ' Returns -255
        ReadResultsFHM = -255
    End Function




End Module
