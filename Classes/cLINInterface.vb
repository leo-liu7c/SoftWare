Option Explicit On
Option Strict On

Imports System.IO

Public Class cLINInterface
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    Public Enum eNxSessionIn As Integer
        eSession_DSM_WM_Frm1_AR = 0
        eSession_DIAG_SlaveResp = 1
    End Enum

    Public Enum eNxSessionOut As Integer
        eSession_DOOR_DOOR_Frm1_AR = 0
        eSession_VehData_Frm_AR2 = 1
        eSession_DIAG_MasterReq = 2
    End Enum

    Public Enum eScheduleDiag As Integer
        DIAG_SCHEDULE_Null = 0
        DIAG_SCHEDULE_Master = 1
        DIAG_SCHEDULE_Slave = 2
    End Enum

    Public Enum eScheduleData As Integer
        SCHEDULE_Faste = 0
        SCHEDULE_Null = 1
        SCHEDULE_Slow = 2
    End Enum

    Public Const MaxFrameCount = 20000

    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+


    ' Private constants
    Private Const _rxBufferSize = 100
    Private Const _FrameBufferSize = 1000000

    ' Private variables
    Private _connected As Boolean
    Private _errorCode As Integer
    Private _errorDescription As String
    Private _log(0 To 10) As String
    Private _rxBufferCount As Integer
    Private _rxBuffer(0 To _rxBufferSize - 1) As CLINFrame
    Private _xnetSessionIn As niXNET
    Private _xnetSessionOut(3) As niXNET

    Private nxSuccess As Integer
    Private NB_nxLIN_OUTPUT_FRAMES As Integer = 3
    Private NB_nxLIN_INPUT_FRAMES As Integer = 1
    Private nxLIN_SLAVE As Integer = 0
    Private nxLIN_MASTER As Integer = 1
    Private g_OutputPayloadLength(NB_nxLIN_OUTPUT_FRAMES) As Integer
    Private g_InputPayloadLength(NB_nxLIN_INPUT_FRAMES) As Integer

    Private _FrameBuffer(0 To 9, 0 To _FrameBufferSize - 1) As Double
    Private _FrameCount As Integer
    Private _StartFrameBuffer As Boolean

    'Testeur Present
    Private _TP As Boolean
    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property Connected As Boolean
        Get
            Connected = _connected
        End Get
    End Property

    Public ReadOnly Property ErrorCode As Integer
        Get
            ErrorCode = _errorCode
        End Get
    End Property

    Public ReadOnly Property ErrorDescription As String
        Get
            ErrorDescription = _errorDescription
        End Get
    End Property

    Public ReadOnly Property Log(ByVal f As Integer) As String
        Get
            Log = _log(f)
        End Get
    End Property

    Public ReadOnly Property RxBufferCount() As Integer
        Get
            RxBufferCount = _rxBufferCount
        End Get
    End Property

    Public ReadOnly Property RxFrame(ByVal index As Integer) As CLINFrame
        Get
            RxFrame = _rxBuffer(index)
        End Get
    End Property

    Public ReadOnly Property FrameBuffer As Double(,)
        Get
            FrameBuffer = _FrameBuffer
        End Get
    End Property

    Public ReadOnly Property FrameCount As Integer
        Get
            FrameCount = _FrameCount
        End Get
    End Property

    Public WriteOnly Property StartFrameBuffer As Boolean
        Set(ByVal value As Boolean)
            _StartFrameBuffer = value
        End Set
    End Property

    Public ReadOnly Property ToSystemTime(ByVal NI_Timestamps As Long) As Date
        Get
            Dim _dwBufSize As Integer = 1 '8192
            Dim _buf(_dwBufSize) As Long
            Dim _gcHandle As System.Runtime.InteropServices.GCHandle = System.Runtime.InteropServices.GCHandle.Alloc(_buf, System.Runtime.InteropServices.GCHandleType.Pinned)
            Dim _bufPtr As IntPtr = _gcHandle.AddrOfPinnedObject()
            Dim _fault As Integer
            Dim _property_size As UInteger
            _xnetSessionIn.nx_GetPropertySize(niXNET.xNETConstants.nxState_TimeCurrent, _property_size)
            _xnetSessionIn.nx_ReadState(niXNET.xNETConstants.nxState_TimeCurrent, _property_size, _bufPtr, _fault)


            Dim _LIN_time_ticks As Long = _buf(0)
            Dim LIN_time As Date = DateTime.FromFileTime(CLng(_buf(0)))

            Dim _SYSTEM_time As Date = Date.Now
            Dim _SYSTEM_time_tick As Long = _SYSTEM_time.ToFileTime()
            Dim _Time_offset = _LIN_time_ticks - _SYSTEM_time_tick

            ToSystemTime = DateTime.FromFileTime(NI_Timestamps - _Time_offset)

        End Get
    End Property

    Public Property TesteurPresent As Boolean

        Get
            TesteurPresent = _TP
        End Get
        Set(ByVal value As Boolean)
            _TP = value
        End Set
    End Property

    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Sub EmptyBuffer()
        ' Empty the buffer of the specified board
        _FrameCount = 0
    End Sub


    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+

    Public Sub ClearLog()
        ' Clear the log string
        _log(mGlobal.eLog_File.Frame_All) = ""
    End Sub

    Public Sub ClearRxBuffer()
        ' Clear the rx buffer
        _rxBufferCount = 0

        For i = 0 To _rxBufferSize - 1
            _rxBuffer(i) = Nothing
        Next
    End Sub

    Public Sub DeleteRxFrame(ByVal index As Integer)
        Dim i As Integer

        ' If the frame index is valid
        If (index >= 0 And index < _rxBufferCount) Then
            ' Delete the specified frame from the rx buffer
            For i = index To _rxBufferCount - 2
                _rxBuffer(i) = _rxBuffer(i + 1)
            Next i
            _rxBuffer(_rxBufferCount - 1) = Nothing
            _rxBufferCount = _rxBufferCount - 1
        End If
    End Sub

    Public Function RxFrameIndex(ByVal frame As CLINFrame) As Integer
        Dim i As Integer

        ' Searche the specified frame and return its index
        RxFrameIndex = -1
        For i = 0 To _rxBufferCount - 1
            If (_rxBuffer(i).Identifier = frame.Identifier AndAlso
                _rxBuffer(i).DataLength = frame.DataLength AndAlso
                (_rxBuffer(i).Data(0) = frame.Data(0) OrElse frame.Data(0) = "XX") AndAlso
                (_rxBuffer(i).Data(1) = frame.Data(1) OrElse frame.Data(1) = "XX") AndAlso
                (_rxBuffer(i).Data(2) = frame.Data(2) OrElse frame.Data(2) = "XX") AndAlso
                (_rxBuffer(i).Data(3) = frame.Data(3) OrElse frame.Data(3) = "XX") AndAlso
                (_rxBuffer(i).Data(4) = frame.Data(4) OrElse frame.Data(4) = "XX") AndAlso
                (_rxBuffer(i).Data(5) = frame.Data(5) OrElse frame.Data(5) = "XX") AndAlso
                (_rxBuffer(i).Data(6) = frame.Data(6) OrElse frame.Data(6) = "XX") AndAlso
                (_rxBuffer(i).Data(7) = frame.Data(7) OrElse frame.Data(7) = "XX")) Then
                RxFrameIndex = i
                Exit For
            End If
        Next i
    End Function

    Public Function Receive(ByVal receiveFilter() As String,
                            ByVal logFilter() As String) As Boolean
        Dim buffer(0 To 9999) As Byte
        Dim byteCount As UInteger
        Dim i As Integer
        ' Return False
        Receive = False
        Try
            ' Read all the received frames
            _errorCode = _xnetSessionIn.nx_ReadFrame(buffer, 10000, 0, byteCount)
            ' For all the received frames
            For i = 0 To CInt(byteCount \ 24) - 1
                ' If the rx buffer is not full
                If (_rxBufferCount < _rxBufferSize) Then
                    ' Read the frame from the buffer
                    _rxBuffer(_rxBufferCount) = New CLINFrame
                    _rxBuffer(_rxBufferCount).ReadFromBuffer(buffer, i * 24)
                    ' If the frame identifier is not in the receive filter list
                    If Not (receiveFilter.Contains(_rxBuffer(_rxBufferCount).Identifier)) Then
                        ' If the frame identifier is not in the log filter list
                        If Not (logFilter.Contains(_rxBuffer(_rxBufferCount).Identifier)) Then
                            ' Log the frame reception
                            LogRx(_rxBuffer(_rxBufferCount).ToString, mGlobal.eLog_File.Frame_All, _rxBuffer(_rxBufferCount).Identifier.ToString,
                                  (ToSystemTime(CLng(_rxBuffer(_rxBufferCount).Timestamp))))
                            If _rxBuffer(_rxBufferCount).Identifier = "9" Then
                                LogRx(_rxBuffer(_rxBufferCount).ToString, mGlobal.eLog_File.Frame_rx_9, _rxBuffer(_rxBufferCount).Identifier.ToString,
                                  (ToSystemTime(CLng(_rxBuffer(_rxBufferCount).Timestamp))))
                            ElseIf _rxBuffer(_rxBufferCount).Identifier = "A" Then
                                LogRx(_rxBuffer(_rxBufferCount).ToString, mGlobal.eLog_File.Frame_rx_A, _rxBuffer(_rxBufferCount).Identifier.ToString,
                                  (ToSystemTime(CLng(_rxBuffer(_rxBufferCount).Timestamp))))
                            ElseIf _rxBuffer(_rxBufferCount).Identifier = "0" Then
                                LogRx(_rxBuffer(_rxBufferCount).ToString, mGlobal.eLog_File.Frame_rx_0, _rxBuffer(_rxBufferCount).Identifier.ToString,
                                  (ToSystemTime(CLng(_rxBuffer(_rxBufferCount).Timestamp))))
                            ElseIf _rxBuffer(_rxBufferCount).Identifier = "3C" Then
                                'Set Frame Diag for TP
                                _TP = True
                                LogRx(_rxBuffer(_rxBufferCount).ToString, mGlobal.eLog_File.Frame_rx_3C, _rxBuffer(_rxBufferCount).Identifier.ToString,
                                  (ToSystemTime(CLng(_rxBuffer(_rxBufferCount).Timestamp))))
                            ElseIf _rxBuffer(_rxBufferCount).Identifier = "3D" Then
                                LogRx(_rxBuffer(_rxBufferCount).ToString, mGlobal.eLog_File.Frame_rx_3D, _rxBuffer(_rxBufferCount).Identifier.ToString,
                                  (ToSystemTime(CLng(_rxBuffer(_rxBufferCount).Timestamp))))
                            End If
                        End If
                        ' Increase the rx buffer count
                        _rxBufferCount = _rxBufferCount + 1
                    Else    ' Otherwise, if the frame is in the receive filter list
                        ' Clear the frame
                        _rxBuffer(_rxBufferCount) = Nothing
                    End If
                Else    ' Otherwise, if the rx buffer is full
                    ' Return True
                    Receive = True
                End If
            Next
        Catch ex As Exception
            ' Store the error description
            _errorDescription = ex.Message
            ' Return True
            Receive = True
        End Try

    End Function

    Public Function PowerDown() As Boolean
        Try
            If _connected Then
                ' Clear the flag of interface connected
                _connected = False
                ' Clear the input session
                _xnetSessionIn.nx_Clear()
                _xnetSessionIn = Nothing
                ' Clear the all output session
                _xnetSessionOut(eNxSessionOut.eSession_DOOR_DOOR_Frm1_AR).nx_Clear()
                _xnetSessionOut(eNxSessionOut.eSession_VehData_Frm_AR2).nx_Clear()
                _xnetSessionOut(eNxSessionOut.eSession_DIAG_MasterReq).nx_Clear()
                _xnetSessionOut(eNxSessionOut.eSession_DOOR_DOOR_Frm1_AR) = Nothing
                _xnetSessionOut(eNxSessionOut.eSession_VehData_Frm_AR2) = Nothing
                _xnetSessionOut(eNxSessionOut.eSession_DIAG_MasterReq) = Nothing
            End If
            ' Return False
            PowerDown = False

        Catch ex As Exception
            ' Return True
            PowerDown = True
        End Try
    End Function

    Public Function PowerUp(ByVal xnetInterface As String, _
                                ByVal baudrate As UInteger, _
                                ByVal Master_Lin As Boolean, _
                                ByVal Schedule As eScheduleData) As Boolean

        Dim databaseNameOut As String = dbBasePath & "DOOR_STAR.ldf" ' "LINDOOR2_02-01-DPC_EOL.ldf"
        Dim clusterNameOut As String = "cluster"

        Dim databaseNameIn As String = ":memory:"
        Dim clusterNameIn As String = ""

        Dim propertyValue As UInteger = 0

        Dim xnetListIn As String = ""
        Dim xnetListOut As String = ""
        Dim xnetModeIn As UInteger = niXNET.xNETConstants.nxMode_FrameInStream
        Dim xnetModeOut As UInteger = niXNET.xNETConstants.nxMode_FrameOutSinglePoint
        Dim xnetModeOutDiag As UInteger = niXNET.xNETConstants.nxMode_FrameOutQueued

        'Transmitted(Frames)
        ' &H0A , VehData_Frm_AR2
        ' &H00 , DOOR_Frm1_AR
        'Received(Frames)
        ' &H09 , DSM_WM_Frm1_AR
        'DiagonLin()
        ' &H3C , MasterReq DiagonLin
        ' &H3D , SlaveResp DiagonLin

        'Dim Lien_Out_Session_ID() As Integer = {&H12, &H19, &H3C}
        'Dim Lien_In_Session_ID() As Integer = {&H1B, &H3D}
        'Dim g_OutputPayloadLength() As Integer = {8, 8, 8}
        'Dim nxLIN_Output_Frame() As String = {"BCM_LINDOOR_A_01", "BCM_LINDOOR_R_02", "MasterReq"}
        'Dim nxLIN_Output_Frame_Array() As String = {"BCM_LINDOOR_A_01", "BCM_LINDOOR_R_02", "MasterReq"}

        'Dim g_InputPayloadLength() As Integer = {8, 8}
        'Dim nxLIN_Input_Frame() As String = {"DPC_LINDOOR_R_01", "SlaveResp"}
        'Dim nxLIN_Input_Frame_Array() As String = {"DPC_LINDOOR_R_01", "SlaveResp"}

        Dim Lien_Out_Session_ID() As Integer = {&HA, &H0, &H3C}
        Dim Lien_In_Session_ID() As Integer = {&H19, &H3D}
        Dim g_OutputPayloadLength() As Integer = {8, 8, 8}
        Dim nxLIN_Output_Frame() As String = {"VehData_Frm_AR2", "DOOR_Frm1_AR", "MasterReq"}
        Dim nxLIN_Output_Frame_Array() As String = {"VehData_Frm_AR2", "DOOR_Frm1_AR", "MasterReq"}

        Dim g_InputPayloadLength() As Integer = {8, 8}
        Dim nxLIN_Input_Frame() As String = {"DSM_WM_Frm1_AR", "SlaveResp"}
        Dim nxLIN_Input_Frame_Array() As String = {"DSM_WM_Frm1_AR", "SlaveResp"}

        Dim i As Integer
        Try
            '// Create xnet sessions for frames output 
            For i = 0 To 1
                _xnetSessionOut(i) = New niXNET(databaseNameOut, clusterNameOut, nxLIN_Output_Frame(i), xnetInterface, xnetModeOut)
            Next i
            '// Create xnet sessions for Diag frames output 
            _xnetSessionOut(eNxSessionOut.eSession_DIAG_MasterReq) = New niXNET(databaseNameOut, clusterNameOut, nxLIN_Output_Frame(2), xnetInterface, xnetModeOutDiag)
            '// Create xnet sessions for frames input
            _xnetSessionIn = New niXNET(databaseNameIn, clusterNameIn, xnetListIn, xnetInterface, xnetModeIn)
            ' Set Baud Rate
            _errorCode = _xnetSessionIn.nx_SetProperty(niXNET.xNETConstants.nxPropSession_IntfBaudRate, 4, baudrate)
            ' Set Property
            For i = 0 To 1
                ' Set Master Lin
                If (Master_Lin) Then
                    _errorCode = _xnetSessionOut(i).nx_SetProperty(niXNET.xNETConstants.nxPropSession_IntfLINMaster, 1, CUInt(nxLIN_MASTER))
                    ' Set Schedule Fast
                    _errorCode = _xnetSessionOut(i).nx_WriteState(niXNET.xNETConstants.nxState_LINScheduleChange, 4, CType(CUInt(Schedule), IntPtr))
                End If
                ' Term
                _errorCode = _xnetSessionOut(i).nx_SetProperty(niXNET.xNETConstants.nxPropSession_IntfLINTerm, 4, 1)
                ' Echo Tx Frame is True for log File
                _errorCode = _xnetSessionOut(i).nx_SetProperty(niXNET.xNETConstants.nxPropSession_IntfEchoTx, 1, CUInt(1))
            Next i
            ' Initialize the rx buffer
            Me.ClearRxBuffer()
            ' Set the flag of interface connected
            _connected = True
            ' Return False
            PowerUp = False

        Catch ex As Exception
            ' Store the error description
            _errorDescription = ex.Message
            _log(0) = _log(0) & _
                           Format(Date.Now, "dd/MM/yyyy, HH:mm:ss:fff") & " Power UP Error : " & _
                           _errorDescription & vbCrLf
            ' Return True
            PowerUp = True
        End Try
    End Function

    Public Function StopScheduleDiag() As Boolean
        Try
            ' Change Schedule Diag for Stop
            _errorCode = _xnetSessionOut(eNxSessionOut.eSession_DIAG_MasterReq).nx_WriteState(niXNET.xNETConstants.nxState_LINDiagnosticScheduleChange, _
                                                          4, _
                                                           CType(niXNET.xNETConstants.nxLINDiagnosticSchedule_NULL, IntPtr))
            ' Return False
            StopScheduleDiag = False
        Catch ex As Exception
            ' Store the error description
            _errorDescription = ex.Message
            ' Return True
            StopScheduleDiag = True
        End Try

    End Function

    Public Function Transmit(ByVal frame As CLINFrame, _
                             ByVal enableLog As Boolean, _
                             ByRef data() As Byte, _
                             ByVal Session As eNxSessionOut, _
                             ByVal txFrame As Boolean, _
                             ByVal diagFrame As Boolean) As Boolean

        Dim buffer(0 To 23) As Byte
        Dim i As Integer
        Dim iReturn As UInteger

        Try
            ' Write the frame to the buffer
            frame.WriteToBuffer(buffer)
            ' Transmit Frame or change Data Value in Frame
            If txFrame = False Then
                For i = 0 To 7
                    buffer(16 + i) = data(i)
                Next i
            End If
            ' Transmite Diag Frame
            If diagFrame = True And Session = eNxSessionOut.eSession_DIAG_MasterReq Then
                _log(mGlobal.eLog_File.Frame_rx_3D) = ""
                ' Change Schedule Diag for transmit
                _errorCode = _xnetSessionOut(eNxSessionOut.eSession_DIAG_MasterReq).nx_WriteState(niXNET.xNETConstants.nxState_LINDiagnosticScheduleChange, _
                                                              4, _
                                                               CType(niXNET.xNETConstants.nxLINDiagnosticSchedule_MasterReq, IntPtr))
                ' Transmit Frame
                _errorCode = _xnetSessionOut(eNxSessionOut.eSession_DIAG_MasterReq).nx_WriteFrame(buffer, 24, 10)
                ' Wait Transmit Frame
                _errorCode = _xnetSessionOut(eNxSessionOut.eSession_DIAG_MasterReq).nx_Wait(niXNET.xNETConstants.nxCondition_TransmitComplete, 0, 1, iReturn)
                ' Change Schedule Diag for received
                _errorCode = _xnetSessionOut(eNxSessionOut.eSession_DIAG_MasterReq).nx_WriteState(niXNET.xNETConstants.nxState_LINDiagnosticScheduleChange, _
                                                              4, _
                                                              CType(niXNET.xNETConstants.nxLINDiagnosticSchedule_SlaveResp, IntPtr))
                'ElseIf Session = eNxSessionOut.eSession_DOOR_DOOR_Frm1_AR Then
                '    ' Transmit the LIN(frame)
                '    _errorCode = _xnetSessionOut(eNxSessionOut.eSession_DOOR_DOOR_Frm1_AR).nx_WriteFrame(buffer, 24, 10)
                'ElseIf Session = eNxSessionOut.eSession_ROOF_VehData_Frm_AR2 Then
                '    ' Transmit the LIN(frame)
                '    _errorCode = _xnetSessionOut(eNxSessionOut.eSession_ROOF_VehData_Frm_AR2).nx_WriteFrame(buffer, 24, 10)
            End If
            ' If the log is enabled
            If (enableLog) Then
                ' Add an entry in the log
                'LogTx(frame.ToString, mGlobal.eLog_File.Frame_All)
            End If
            ' Return False
            Transmit = False

        Catch ex As Exception
            ' Store the error description
            _errorDescription = ex.Message
            ' Return True
            Transmit = True
        End Try
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Sub LogRx(ByVal logString As String,
                      ByVal f As Integer,
                      ByVal FrameID As String,
                      ByVal TimeStamp As Date)
        ' Update the log
        ' Log for Maintenance Mode
        If f <> mGlobal.eLog_File.Frame_rx_3D And f <> mGlobal.eLog_File.Frame_All Then
            _log(f) = Format(TimeStamp, "dd/MM/yyyy, HH:mm:ss:fff") & " LIN - DUT -> PC : " &
            logString & vbCrLf
        ElseIf f = mGlobal.eLog_File.Frame_rx_3D And f <> mGlobal.eLog_File.Frame_All Then
            _log(mGlobal.eLog_File.Frame_rx_3D) = _log(mGlobal.eLog_File.Frame_rx_3D) &
                        Format(TimeStamp, "dd/MM/yyyy, HH:mm:ss:fff") & " LIN - DUT -> PC : " &
                        logString & vbCrLf
        ElseIf f = mGlobal.eLog_File.Frame_rx_3C And f <> mGlobal.eLog_File.Frame_All Then
            _log(mGlobal.eLog_File.Frame_rx_3D) = _log(mGlobal.eLog_File.Frame_rx_3D) &
                        Format(TimeStamp, "dd/MM/yyyy, HH:mm:ss:fff") & " LIN - DUT -> PC : " &
                        logString & vbCrLf
        ElseIf f = mGlobal.eLog_File.Frame_All Then
            ' Log index 0 is reserved for Log File in Production Mode
            If FrameID = "9" Then
                _log(mGlobal.eLog_File.Frame_All) = _log(mGlobal.eLog_File.Frame_All) &
                            Format(TimeStamp, "dd/MM/yyyy, HH:mm:ss:fff") & " LIN - DUT -> PC : " &
                            logString & vbCrLf
            ElseIf FrameID = "3D" Then
                _log(mGlobal.eLog_File.Frame_All) = _log(mGlobal.eLog_File.Frame_All) &
                            Format(TimeStamp, "dd/MM/yyyy, HH:mm:ss:fff") & " LIN - DUT -> PC : " &
                            logString & vbCrLf
            ElseIf FrameID = "3C" Then
                _log(mGlobal.eLog_File.Frame_All) = _log(mGlobal.eLog_File.Frame_All) &
                            Format(TimeStamp, "dd/MM/yyyy, HH:mm:ss:fff") & " LIN - PC  -> DUT: " &
                            logString & vbCrLf
            End If
        End If

    End Sub

End Class