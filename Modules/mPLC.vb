Option Explicit On
Option Strict On

Module mPLC
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _port = 102
    Private Const _rack = 0
    Private Const _slot = 1
    Private Const _HardwareEnabled = mConstants.HardwareEnabled_PLC

    ' Private variables
    Private _connected As Boolean
    Private _connection As libnodave.daveConnection
    Private _fileDescriptors As libnodave.daveOSserialType
    Private _interface As libnodave.daveInterface



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
    Public Function Connect(ByVal IPAddress As String) As Boolean
        Dim e As Boolean

        If (_HardwareEnabled) Then
            Try
                ' Get the read and write file descriptors
                _fileDescriptors.rfd = libnodave.openSocket(_port, IPAddress)
                _fileDescriptors.wfd = _fileDescriptors.rfd
                e = (_fileDescriptors.rfd <= 0 Or _fileDescriptors.wfd <= 0)
                ' If no errors happened
                If Not (e) Then
                    ' Create the interface
                    _interface = New libnodave.daveInterface(_fileDescriptors, _
                                                             "IF1", _
                                                             0, _
                                                             libnodave.daveProtoISOTCP, _
                                                             libnodave.daveSpeed187k)
                    ' Set the interface timeout to 1 s = 1000000 us
                    _interface.setTimeout(1000000)
                    ' Initialize the adapter
                    e = (_interface.initAdapter <> libnodave.daveResOK)
                End If
                ' If no errors happened
                If Not (e) Then
                    ' Create the connection
                    _connection = New libnodave.daveConnection(_interface, _
                                                               0, _
                                                               _rack, _
                                                               _slot)
                    ' Connect the PLC
                    e = (_connection.connectPLC() <> libnodave.daveResOK)
                End If
                ' Set the flag of PLC connected
                _connected = Not (e)
                ' Return the error flag
                Connect = e

            Catch ex As Exception
                ' Return True
                Connect = True
            End Try
        Else
            ' Return True
            Connect = False
        End If

    End Function



    Public Function Disconnect() As Boolean
        If (_HardwareEnabled) Then
            ' Disconnect the PLC
            Disconnect = (_connection.disconnectPLC() <> libnodave.daveResOK)
            ' Destroy the connection object
            _connection = Nothing
            ' Destroy the interface object
            _interface = Nothing
            ' Close the socket
            libnodave.closeSocket(_port)    ' TMP_STE: I do not consider the function result because sometimes it is failed for no reasons
        Else
            ' Disconnect the PLC
            Disconnect = False
        End If

    End Function



    Public Function ReadDataBlock(ByRef dataBlock As cPLCDataBlock, _
                                  ByVal fromVariable As Integer, _
                                  ByVal toVariable As Integer) As Boolean
        Dim address As String
        Dim buffer() As Byte
        Dim byteCount As Integer
        Dim firstByte As Integer
        Dim lastByte As Integer
        Dim byteIndex As Integer
        Dim bitIndex As Integer

        ' Return False
        ReadDataBlock = False
        ' Redimensionate the buffer
        ReDim buffer(0 To dataBlock.ByteCount - 1)
        ' Verify that the PLC is connected
        ReadDataBlock = Not (_connected)
        ' If no errors happened
        If Not (ReadDataBlock) Then
            ' Calculate the first byte
            address = Mid(dataBlock.Variable(fromVariable).Address, 4)
            If (address.Contains(".")) Then
                address = Left(address, InStr(address, ".") - 1)
            End If
            firstByte = CInt(address)
            ' Calculate the last byte
            address = Mid(dataBlock.Variable(toVariable).Address, 4)
            If (address.Contains(".")) Then
                address = Left(address, InStr(address, ".") - 1)
            End If
            lastByte = CInt(address)
            If (dataBlock.Variable(toVariable).Type = cPLCVariable.eType.BoolVariable Or _
                dataBlock.Variable(toVariable).Type = cPLCVariable.eType.ByteVariable) Then
                lastByte = lastByte + 1 - 1
            ElseIf (dataBlock.Variable(toVariable).Type = cPLCVariable.eType.WordVariable Or _
                    dataBlock.Variable(toVariable).Type = cPLCVariable.eType.IntegerVariable) Then
                lastByte = lastByte + 2 - 1
            ElseIf (dataBlock.Variable(toVariable).Type = cPLCVariable.eType.DoubleWordVariable Or _
                    dataBlock.Variable(toVariable).Type = cPLCVariable.eType.DoubleIntegerVariable Or _
                    dataBlock.Variable(toVariable).Type = cPLCVariable.eType.RealVariable) Then
                lastByte = lastByte + 4 - 1
            ElseIf (dataBlock.Variable(toVariable).Type = cPLCVariable.eType.StringVariable) Then
                lastByte = lastByte + dataBlock.Variable(toVariable).Length - 1
            End If
            ' Calculate the number of bytes to read
            byteCount = lastByte - firstByte + 1
            ' Read the bytes
            ReadDataBlock = (_connection.readManyBytes(libnodave.daveDB, _
                                                       dataBlock.Number, _
                                                       firstByte, _
                                                       byteCount, _
                                                       buffer) <> libnodave.daveResOK)
        End If
        ' If no errors happened
        If Not (ReadDataBlock) Then
            ' For all the variables
            For i = 0 To dataBlock.VariableCount - 1
                ' Create a local copy of the address
                address = dataBlock.Variable(i).Address
                ' Read from the buffer the variable value
                If (dataBlock.Variable(i).Type = cPLCVariable.eType.BoolVariable) Then               ' Bool variable
                    address = Mid(address, InStr(address, "DBX") + 3)
                    byteIndex = CInt(Left(address, InStr(address, ".") - 1))
                    address = Mid(address, InStr(address, ".") + 1)
                    bitIndex = CInt(address)
                    dataBlock.Variable(i).Value = ((buffer(byteIndex) And CByte(2 ^ bitIndex)) <> 0)
                ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.ByteVariable) Then           ' Byte variable
                    address = Mid(address, InStr(address, "DBB") + 3)
                    byteIndex = CInt(address)
                    dataBlock.Variable(i).Value = buffer(byteIndex)
                ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.WordVariable) Then           ' Word variable
                    address = Mid(address, InStr(address, "DBW") + 3)
                    byteIndex = CInt(address)
                    dataBlock.Variable(i).Value = libnodave.getU16from(buffer, byteIndex)
                ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.DoubleWordVariable) Then     ' Double-word variable
                    address = Mid(address, InStr(address, "DBD") + 3)
                    byteIndex = CInt(address)
                    dataBlock.Variable(i).Value = libnodave.getU32from(buffer, byteIndex)
                ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.IntegerVariable) Then        ' Integer variable
                    address = Mid(address, InStr(address, "DBW") + 3)
                    byteIndex = CInt(address)
                    dataBlock.Variable(i).Value = libnodave.getS16from(buffer, byteIndex)
                ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.DoubleIntegerVariable) Then  ' Double-integer variable
                    address = Mid(address, InStr(address, "DBD") + 3)
                    byteIndex = CInt(address)
                    dataBlock.Variable(i).Value = libnodave.getS32from(buffer, byteIndex)
                ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.RealVariable) Then           ' Real variable
                    address = Mid(address, InStr(address, "DBD") + 3)
                    byteIndex = CInt(address)
                    dataBlock.Variable(i).Value = libnodave.getFloatfrom(buffer, byteIndex)
                ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.StringVariable) Then         ' String variable
                    address = Mid(address, InStr(address, "DBB") + 3)
                    byteIndex = CInt(address)
                    dataBlock.Variable(i).Value = ""
                    For j = 0 To dataBlock.Variable(i).Length - 1
                        If (buffer(byteIndex + j) <> 0) Then
                            dataBlock.Variable(i).Value = CStr(dataBlock.Variable(i).Value) & _
                                                          Chr(buffer(byteIndex + j))
                        Else
                            Exit For
                        End If
                    Next
                End If
            Next
        End If
    End Function



    Public Function WriteDataBlock(ByRef dataBlock As cPLCDataBlock, _
                                   ByVal fromVariable As Integer, _
                                   ByVal toVariable As Integer) As Boolean
        Dim address As String
        Dim buffer() As Byte
        Dim byteCount As Integer
        Dim firstByte As Integer
        Dim lastByte As Integer
        Dim byteIndex As Integer
        Dim bitIndex As Integer
        Dim valueByte() As Byte

        On Error GoTo ErrHandle
        ' Return False
        WriteDataBlock = False
        ' Redimensionate the buffer
        ReDim buffer(0 To dataBlock.ByteCount - 1)
        ' For all the variables
        For i = 0 To dataBlock.VariableCount - 1
            ' Create a local copy of the address
            address = dataBlock.Variable(i).Address
            ' Write to the buffer the variable value
            If (dataBlock.Variable(i).Type = cPLCVariable.eType.BoolVariable) Then               ' Bool variable
                address = Mid(address, InStr(address, "DBX") + 3)
                byteIndex = CInt(Left(address, InStr(address, ".") - 1))
                address = Mid(address, InStr(address, ".") + 1)
                bitIndex = CInt(address)
                If (CBool(dataBlock.Variable(i).Value) = False) Then
                    buffer(byteIndex) = buffer(byteIndex) And Not CByte(2 ^ bitIndex)
                Else
                    buffer(byteIndex) = buffer(byteIndex) Or CByte(2 ^ bitIndex)
                End If
            ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.ByteVariable) Then           ' Byte variable
                address = Mid(address, InStr(address, "DBB") + 3)
                byteIndex = CInt(address)
                buffer(byteIndex) = CByte(dataBlock.Variable(i).Value)
            ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.WordVariable) Then           ' Word variable
                address = Mid(address, InStr(address, "DBW") + 3)
                byteIndex = CInt(address)
                valueByte = BitConverter.GetBytes(CType(dataBlock.Variable(i).Value, UInt16))
                buffer(byteIndex) = valueByte(1)
                buffer(byteIndex + 1) = valueByte(0)
            ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.DoubleWordVariable) Then     ' Double-word variable
                address = Mid(address, InStr(address, "DBD") + 3)
                byteIndex = CInt(address)
                valueByte = BitConverter.GetBytes(CType(dataBlock.Variable(i).Value, UInt32))
                buffer(byteIndex) = valueByte(3)
                buffer(byteIndex + 1) = valueByte(2)
                buffer(byteIndex + 2) = valueByte(1)
                buffer(byteIndex + 3) = valueByte(0)
            ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.IntegerVariable) Then        ' Integer variable
                address = Mid(address, InStr(address, "DBW") + 3)
                byteIndex = CInt(address)
                valueByte = BitConverter.GetBytes(CType(dataBlock.Variable(i).Value, Int16))
                buffer(byteIndex) = valueByte(1)
                buffer(byteIndex + 1) = valueByte(0)
            ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.DoubleIntegerVariable) Then  ' Double-integer variable
                address = Mid(address, InStr(address, "DBD") + 3)
                byteIndex = CInt(address)
                valueByte = BitConverter.GetBytes(CType(dataBlock.Variable(i).Value, Int32))
                buffer(byteIndex) = valueByte(0)
                buffer(byteIndex + 1) = valueByte(1)
                buffer(byteIndex + 2) = valueByte(2)
                buffer(byteIndex + 3) = valueByte(3)
            ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.RealVariable) Then           ' Real variable
                address = Mid(address, InStr(address, "DBD") + 3)
                byteIndex = CInt(address)
                valueByte = BitConverter.GetBytes(libnodave.toPLCfloat(CSng(dataBlock.Variable(i).Value)))
                buffer(byteIndex) = valueByte(0)
                buffer(byteIndex + 1) = valueByte(1)
                buffer(byteIndex + 2) = valueByte(2)
                buffer(byteIndex + 3) = valueByte(3)
            ElseIf (dataBlock.Variable(i).Type = cPLCVariable.eType.StringVariable) Then         ' String variable
                address = Mid(address, InStr(address, "DBB") + 3)
                byteIndex = CInt(address)
                For j = 0 To dataBlock.Variable(i).Length - 1
                    If (j < CStr(dataBlock.Variable(i).Value).Length) Then
                        buffer(byteIndex + j) = CByte(Asc(CStr(dataBlock.Variable(i).Value).Substring(j, 1)))
                    Else
                        buffer(byteIndex + j) = 0
                    End If
                Next j
            End If
        Next i
        ' Verify that the PLC is connected
        WriteDataBlock = Not (_connected)
        ' If no errors happened
        If Not (WriteDataBlock) Then
            ' Calculate the first byte
            address = Mid(dataBlock.Variable(fromVariable).Address, 4)
            If (address.Contains(".")) Then
                address = Left(address, InStr(address, ".") - 1)
            End If
            firstByte = CInt(address)
            ' Calculate the last byte
            address = Mid(dataBlock.Variable(toVariable).Address, 4)
            If (address.Contains(".")) Then
                address = Left(address, InStr(address, ".") - 1)
            End If
            lastByte = CInt(address)
            If (dataBlock.Variable(toVariable).Type = cPLCVariable.eType.BoolVariable Or _
                dataBlock.Variable(toVariable).Type = cPLCVariable.eType.ByteVariable) Then
                lastByte = lastByte + 1 - 1
            ElseIf (dataBlock.Variable(toVariable).Type = cPLCVariable.eType.WordVariable Or _
                    dataBlock.Variable(toVariable).Type = cPLCVariable.eType.IntegerVariable) Then
                lastByte = lastByte + 2 - 1
            ElseIf (dataBlock.Variable(toVariable).Type = cPLCVariable.eType.DoubleWordVariable Or _
                    dataBlock.Variable(toVariable).Type = cPLCVariable.eType.DoubleIntegerVariable Or _
                    dataBlock.Variable(toVariable).Type = cPLCVariable.eType.RealVariable) Then
                lastByte = lastByte + 4 - 1
            ElseIf (dataBlock.Variable(toVariable).Type = cPLCVariable.eType.StringVariable) Then
                lastByte = lastByte + dataBlock.Variable(toVariable).Length - 1
            End If
            ' Calculate the number of bytes to write
            byteCount = lastByte - firstByte + 1
            ' Write the bytes
            WriteDataBlock = (_connection.writeManyBytes(libnodave.daveDB, _
                                                         dataBlock.Number, _
                                                         firstByte, _
                                                         byteCount, _
                                                         buffer) <> libnodave.daveResOK)
        End If

        On Error GoTo 0
        Exit Function

ErrHandle:
        Dim ErrMessage As String
        ErrMessage = Err.Description
        On Error GoTo 0
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module
