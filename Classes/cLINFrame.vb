Option Explicit On
Option Strict On

Imports System.IO


Public Class CLINFrame
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    'Public enumerations for LIN
    Public Enum eFrameType
        Data = 0
        LogTrigger = 1
        StartTrigger = 2
        BusError = 3
    End Enum


    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private _data(0 To 7) As String
    Private _dataLength As Byte
    Private _description As String
    Private _DiagFrame As Boolean
    Private _frameType As eFrameType
    Private _identifier As String
    Private _timestamp As ULong



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public Property Data(ByVal index As Integer) As String
        Get
            Data = _data(index)
        End Get
        Set(ByVal value As String)
            _data(index) = value
        End Set
    End Property



    Public Property DataLength As Byte
        Get
            DataLength = _dataLength
        End Get
        Set(ByVal value As Byte)
            _dataLength = value
        End Set
    End Property



    Public Property Description As String
        Get
            Description = _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property



    Public Property DiagFrame As Boolean
        Get
            DiagFrame = _DiagFrame
        End Get
        Set(ByVal value As Boolean)
            _DiagFrame = value
        End Set
    End Property



    Public Property FrameType As eFrameType
        Get
            FrameType = _frameType
        End Get
        Set(ByVal value As eFrameType)
            If (value = eFrameType.Data Or _
                value = eFrameType.LogTrigger Or _
                value = eFrameType.StartTrigger Or _
                value = eFrameType.BusError) Then
                _frameType = value
            Else
                Throw New System.Exception(String.Format("Frame type {0} not valid for a LIN frame", value))
            End If
        End Set
    End Property



    Public Property Identifier As String
        Get
            Identifier = _identifier
        End Get
        Set(ByVal value As String)
            _identifier = value
        End Set
    End Property



    Public Property Timestamp As ULong
        Get
            Timestamp = _timestamp
        End Get
        Set(ByVal value As ULong)
            _timestamp = value
        End Set
    End Property



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+
    ' Help Diag Negative Response

    '00 positiveResponse.
    '01 - 0F ISOSAEReserved
    '10 generalReject
    '11 serviceNotSupported
    '12 subFunctionNotSupported
    '13 incorrectMessageLengthOrInvalidFormat
    '14 responseTooLong
    '15 - 20 ISOSAEReserved
    '21 busyRepeatRequest
    '22 conditionsNotCorrect
    '23 ISOSAEReserved
    '24 requestSequenceError
    '25 noResponseFromSubnetComponent
    '26 failurePreventsExecutionOfRequestedAction
    '27 - 30 ISOSAEReserved
    '31 requestOutOfRange
    '32 ISOSAEReserved
    '33 securityAccessDenied
    '34 ISOSAEReserved
    '35 invalidKey
    '36 exceedNumberOfAttempts
    '37 requiredTimeDelayNotExpired
    '38 – 4F reservedByExtendedDataLinkSecurityDocument
    '50 – 6F ISOSAEReserved
    '70 uploadDownloadNotAccepted
    '71 transferDataSuspended
    '72 generalProgrammingFailure
    '73 wrongBlockSequenceCounter
    '74 - 77 ISOSAEReserved
    '78 requestCorrectlyReceived-ResponsePending
    '79 – 7D ISOSAEReserved
    '7E subFunctionNotSupportedInActiveSession
    '7F serviceNotSupportedInActiveSession
    '80 ISOSAEReserved
    '81 rpmTooHigh
    '82 rpmTooLow
    '83 engineIsRunning
    '84 engineIsNotRunning
    '85 engineRunTimeTooLow
    '86 temperatureTooHigh
    '87 temperatureTooLow
    '88 vehicleSpeedTooHigh
    '89 vehicleSpeedTooLow
    '8A throttle/PedalTooHigh
    '8B throttle/PedalTooLow
    '8C transmissionRangeNotInNeutral
    '8D transmissionRangeNotInGear
    '8E ISOSAEReserved
    '8F brakeSwitch(es)NotClosed (brake pedal not pressed or not applied)
    '90 shifterLeverNotInPark
    '91 torqueConverterClutchLocked
    '92 voltageTooHigh
    '93 voltageTooLow
    '94 - FE reservedForSpecificConditionsNotCorrect
    'FF ISOSAEReserved

    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Shared Function LoadArrayFromFile(ByVal path As String, _
                                             ByRef frameArray() As CLINFrame) As Boolean
        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim token() As String

        Try
            ' Open the file
            file = New StreamReader(path, System.Text.Encoding.Default)
            ' Load the frame array
            While Not (file.EndOfStream)
                line = file.ReadLine
                If (line <> "" AndAlso Not (line.StartsWith("/"))) Then
                    token = Split(line, vbTab)
                    i = CInt(token(0))
                    frameArray(i) = Nothing
                    frameArray(i) = New CLINFrame
                    frameArray(i).Description = token(1)
                    frameArray(i).Identifier = token(2)
                    frameArray(i).DataLength = CByte(token(3))
                    frameArray(i).Data(0) = token(4)
                    frameArray(i).Data(1) = token(5)
                    frameArray(i).Data(2) = token(6)
                    frameArray(i).Data(3) = token(7)
                    frameArray(i).Data(4) = token(8)
                    frameArray(i).Data(5) = token(9)
                    frameArray(i).Data(6) = token(10)
                    frameArray(i).Data(7) = token(11)
                    frameArray(i).DiagFrame = CBool(token(12))
                End If
            End While
            ' Return False
            LoadArrayFromFile = False

        Catch ex As Exception
            ' Return True
            LoadArrayFromFile = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function



    Public Sub ReadFromBuffer(ByVal value() As Byte)
        ' Read the frame from the buffer starting from index 0
        Me.ReadFromBuffer(value, 0)
    End Sub



    Public Sub ReadFromBuffer(ByRef buffer() As Byte, _
                              ByVal startIndex As Integer)
        Dim i As Integer
        Dim identifier As UInteger

        ' Timestamp
        _timestamp = BitConverter.ToUInt64(buffer, startIndex)
        ' Identifier
        identifier = BitConverter.ToUInt32(buffer, startIndex + 8)
        _identifier = Hex(identifier)
        ' Type
        If (buffer(startIndex + 12) = niXNET.xNETConstants.nxFrameType_LIN_Data) Then
            _frameType = eFrameType.Data
        ElseIf (buffer(startIndex + 12) = niXNET.xNETConstants.nxFrameType_Special_LogTrigger) Then
            _frameType = eFrameType.LogTrigger
        ElseIf (buffer(startIndex + 12) = niXNET.xNETConstants.nxFrameType_Special_StartTrigger) Then
            _frameType = eFrameType.StartTrigger
        ElseIf (buffer(startIndex + 12) = niXNET.xNETConstants.nxFrameType_CAN_BusError) Then
            _frameType = eFrameType.BusError
        Else
            Throw New System.Exception(String.Format("Frame type {0} not valid for a LIN frame", buffer(startIndex + 12)))
        End If
        ' Payload length
        _dataLength = buffer(startIndex + 15)
        ' Payload
        For i = 0 To 7
            If (i < _dataLength) Then
                _data(i) = Right("0" & Hex(buffer(startIndex + 16 + i)), 2)
            Else
                _data(i) = "00"
            End If
        Next
    End Sub



    Public Shared Function SaveArrayToFile(ByVal path As String, _
                                           ByRef frameArray() As CLINFrame) As Boolean
        Dim file As StreamWriter = Nothing
        Dim i As Integer

        Try
            ' Open the file
            file = New StreamWriter(path)
            ' Save the frame array
            For i = frameArray.GetLowerBound(0) To frameArray.GetUpperBound(0)
                If (frameArray(i) IsNot Nothing) Then
                    file.WriteLine(i & _
                                   vbTab & frameArray(i).Description & _
                                   vbTab & frameArray(i).Identifier & _
                                   vbTab & frameArray(i).DiagFrame & _
                                   vbTab & frameArray(i).DataLength & _
                                   vbTab & frameArray(i).Data(0) & _
                                   vbTab & frameArray(i).Data(1) & _
                                   vbTab & frameArray(i).Data(2) & _
                                   vbTab & frameArray(i).Data(3) & _
                                   vbTab & frameArray(i).Data(4) & _
                                   vbTab & frameArray(i).Data(5) & _
                                   vbTab & frameArray(i).Data(6) & _
                                   vbTab & frameArray(i).Data(7))

                End If
            Next
            ' Return False
            SaveArrayToFile = False

        Catch ex As Exception
            ' Return True
            SaveArrayToFile = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function



    Public Overrides Function ToString() As String
        ' Return the string representation of the frame
        ToString = _identifier & _
                   " " & _dataLength & _
                   " " & _data(0) & _
                   " " & _data(1) & _
                   " " & _data(2) & _
                   " " & _data(3) & _
                   " " & _data(4) & _
                   " " & _data(5) & _
                   " " & _data(6) & _
                   " " & _data(7)
    End Function



    Public Sub WriteToBuffer(ByRef buffer() As Byte)
        ' Write the frame to the buffer starting from index 0
        Me.WriteToBuffer(buffer, 0)
    End Sub



    Public Sub WriteToBuffer(ByRef buffer() As Byte, _
                             ByVal startIndex As Integer)
        Dim b() As Byte
        Dim i As Integer
        '///////////////////
        '   Timestamp 0 to 7
        '   Identifier 8 to 11
        '   Type 12
        '   Flags 13
        '   Info 14
        '   PayloadLength 15
        '   Payload 16 to 23

        ' Timestamp
        b = BitConverter.GetBytes(_timestamp)
        For i = 0 To 7
            buffer(startIndex + i) = b(i)
        Next
        ' Identifier
        b = BitConverter.GetBytes(CUInt("&H" & _identifier))

        For i = 0 To 3
            buffer(startIndex + 8 + i) = b(i)
        Next
        ' Type
        If (_frameType = eFrameType.Data) Then
            buffer(startIndex + 12) = niXNET.xNETConstants.nxFrameType_LIN_Data
        ElseIf (_frameType = eFrameType.LogTrigger) Then
            buffer(startIndex + 12) = niXNET.xNETConstants.nxFrameType_Special_LogTrigger
        ElseIf (_frameType = eFrameType.StartTrigger) Then
            buffer(startIndex + 12) = niXNET.xNETConstants.nxFrameType_Special_StartTrigger
        ElseIf (_frameType = eFrameType.BusError) Then
            buffer(startIndex + 12) = niXNET.xNETConstants.nxFrameType_CAN_BusError
        Else
            Throw New System.Exception(String.Format("Frame type {0} not valid for a LIN frame", CByte(_frameType)))
        End If
        ' Flags
        buffer(startIndex + 13) = niXNET.xNETConstants.nxFrameFlags_TransmitEcho
        ' Info
        buffer(startIndex + 14) = 0
        ' Payload length
        buffer(startIndex + 15) = _dataLength
        ' Payload
        For i = 0 To 7
            If (i < _dataLength) Then
                buffer(startIndex + 16 + i) = CByte("&H" & _data(i))
            Else
                buffer(startIndex + 16 + i) = 0
            End If
        Next
    End Sub

    Public Function DeepClone() As CLINFrame
        Dim temp As CLINFrame = New CLINFrame With {
             ._dataLength = _dataLength,
             ._description = _description,
             ._DiagFrame = _DiagFrame,
             ._frameType = _frameType,
             ._identifier = _identifier,
             ._timestamp = _timestamp
        }
        For index = 0 To _data.Length - 1
            temp._data(index) = _data(index)
        Next
        Return temp
    End Function


    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Class
