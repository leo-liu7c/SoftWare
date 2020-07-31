Option Explicit On

Imports System
Imports System.IO

Public Class cPLCDataBlock
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private _number As Integer
    Private _byteCount As Integer
    Private _variable() As cPLCVariable
    Private _variableCount As Integer



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property Number As Integer
        Get
            Number = _number
        End Get
    End Property



    Public ReadOnly Property ByteCount As Integer
        Get
            ByteCount = _byteCount
        End Get
    End Property



    Public Property Variable(ByVal index As Integer) As cPLCVariable
        Get
            Variable = _variable(index)
        End Get
        Set(propertyValue As cPLCVariable)
            _variable(index) = propertyvalue
        End Set
    End Property



    Public ReadOnly Property VariableCount As Integer
        Get
            VariableCount = _variableCount
        End Get
    End Property



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+
    Public Sub New(ByVal number As Integer, _
                   ByVal byteCount As Integer, _
                   ByVal variableCount As Integer)
        _number = number
        _byteCount = byteCount
        _variableCount = variableCount
        ReDim _variable(0 To _variableCount - 1)
    End Sub



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Shared Function LoadConfiguration(ByRef dataBlock As cPLCDataBlock, _
                                             ByVal path As String) As Boolean
        Dim byteCount As Integer
        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim number As Integer
        Dim token(0 To 12) As String
        Dim variableCount As Integer
        Dim variableIndex As Integer

        Try
            ' Open the file
            file = New StreamReader(path, System.Text.Encoding.Default)
            ' Read the data block number
            line = file.ReadLine
            token = Split(line, vbTab)
            number = CInt(token(1))
            ' Read the data block byte count
            line = file.ReadLine
            token = Split(line, vbTab)
            byteCount = CInt(token(1))
            ' Read the data block variable count
            line = file.ReadLine
            token = Split(line, vbTab)
            variableCount = CInt(token(1))
            ' Create the data block
            dataBlock = Nothing
            dataBlock = New cPLCDataBlock(number, byteCount, variableCount)
            ' Ignore 2 lines
            line = file.ReadLine
            line = file.ReadLine
            ' For all the variables
            For i = 0 To variableCount - 1
                ' Read the variable description
                line = file.ReadLine
                token = Split(line, vbTab)
                ' Create the variable
                variableIndex = CInt(token(0))
                If (token(3) = "X") Then
                    dataBlock.Variable(variableIndex) = New cPLCVariable(cPLCVariable.eType.BoolVariable)
                    dataBlock.Variable(variableIndex).Address = token(1)
                    dataBlock.Variable(variableIndex).Description = token(2)
                    dataBlock.Variable(variableIndex).Value = False
                ElseIf (token(4) = "X") Then
                    dataBlock.Variable(variableIndex) = New cPLCVariable(cPLCVariable.eType.ByteVariable)
                    dataBlock.Variable(variableIndex).Address = token(1)
                    dataBlock.Variable(variableIndex).Description = token(2)
                    dataBlock.Variable(variableIndex).Value = 0
                ElseIf (token(5) = "X") Then
                    dataBlock.Variable(variableIndex) = New cPLCVariable(cPLCVariable.eType.WordVariable)
                    dataBlock.Variable(variableIndex).Address = token(1)
                    dataBlock.Variable(variableIndex).Description = token(2)
                    dataBlock.Variable(variableIndex).Value = 0
                ElseIf (token(6) = "X") Then
                    dataBlock.Variable(variableIndex) = New cPLCVariable(cPLCVariable.eType.DoubleWordVariable)
                    dataBlock.Variable(variableIndex).Address = token(1)
                    dataBlock.Variable(variableIndex).Description = token(2)
                    dataBlock.Variable(variableIndex).Value = 0
                ElseIf (token(7) = "X") Then
                    dataBlock.Variable(variableIndex) = New cPLCVariable(cPLCVariable.eType.IntegerVariable)
                    dataBlock.Variable(variableIndex).Address = token(1)
                    dataBlock.Variable(variableIndex).Description = token(2)
                    dataBlock.Variable(variableIndex).Value = 0
                ElseIf (token(8) = "X") Then
                    dataBlock.Variable(variableIndex) = New cPLCVariable(cPLCVariable.eType.DoubleIntegerVariable)
                    dataBlock.Variable(variableIndex).Address = token(1)
                    dataBlock.Variable(variableIndex).Description = token(2)
                    dataBlock.Variable(variableIndex).Value = 0
                ElseIf (token(9) = "X") Then
                    dataBlock.Variable(variableIndex) = New cPLCVariable(cPLCVariable.eType.RealVariable)
                    dataBlock.Variable(variableIndex).Address = token(1)
                    dataBlock.Variable(variableIndex).Description = token(2)
                    dataBlock.Variable(variableIndex).Value = 0
                ElseIf (token(10) = "X") Then
                    dataBlock.Variable(variableIndex) = New cPLCVariable(cPLCVariable.eType.StringVariable)
                    dataBlock.Variable(variableIndex).Address = token(1)
                    dataBlock.Variable(variableIndex).Description = token(2)
                    dataBlock.Variable(variableIndex).Length = CInt(token(11))
                    dataBlock.Variable(variableIndex).Value = ""
                End If
            Next i
            ' Return False
            LoadConfiguration = False

        Catch ex As Exception
            ' Return True
            LoadConfiguration = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Class
