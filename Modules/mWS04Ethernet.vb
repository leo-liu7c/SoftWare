﻿Option Explicit On
Option Strict On

Module mWS04Ethernet
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Input enumeration
    Public Enum eInput
        Reserve = 0
        PartTypeNumber = 1
        Test_Mode = 2
        PartModel = 3
        Reference = 4
        UniqueNumber = 5
        HW_Version = 6
        SW_Version = 7
        NVM_Version = 8

        Count = 9
    End Enum

    ' Outputs enumeration
    Public Enum eOutput
        ResultCode = 1
        Reserve = 0
        Reference = 2
        UniqueNumber = 3
        HW_Version = 4
        SW_Version = 5
        NVM_Version = 6

        Test_Enable_RLPULL = 7
        Test_Enable_RLPUSH = 8
        Test_Enable_RRPULL = 9
        Test_Enable_RRPUSH = 10
        Test_Enable_Folding = 11
        Test_Enable_PLC = 12

        RLCorrect_X_PULL = 13
        RLCorrect_Y_PULL = 14
        RLCorrect_Z_PULL = 15
        RLZ_TOUCH_PULL = 16

        RLCorrect_X_PUSH = 17
        RLCorrect_Y_PUSH = 18
        RLCorrect_Z_PUSH = 19
        RLZ_TOUCH_PUSH = 20

        RRCorrect_X_PULL = 21
        RRCorrect_Y_PULL = 22
        RRCorrect_Z_PULL = 23
        RRZ_TOUCH_PULL = 24

        RRCorrect_X_PUSH = 25
        RRCorrect_Y_PUSH = 26
        RRCorrect_Z_PUSH = 27
        rRZ_TOUCH_PUSH = 28

        MF_Correct_X_PUSH = 29
        MF_Correct_Y_PUSH = 30
        MF_Correct_Z_PUSH = 31
        MF_Z_TOUCH_PUSH = 32

        Count = 33
    End Enum




    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private _inputDB As cPLCDataBlock
    Private _outputDB As cPLCDataBlock
    Private Const _HardwareEnabled = mConstants.HardwareEnabled_PLC


    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property InputValue(ByVal index As eInput) As Object
        Get
            InputValue = _inputDB.Variable(index).Value
        End Get
    End Property



    Public Property OutputValue(ByVal index As eOutput) As Object
        Get
            OutputValue = _outputDB.Variable(index).Value
        End Get
        Set(ByVal value As Object)
            _outputDB.Variable(index).Value = value
        End Set
    End Property



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function PowerDown() As Boolean
        ' Return False
        PowerDown = False
    End Function



    Public Function PowerUp(ByVal inputDBConfigurationPath As String, _
                            ByVal outputDBConfigurationPath As String) As Boolean
        ' Load the input data block configuration
        PowerUp = cPLCDataBlock.LoadConfiguration(_inputDB, inputDBConfigurationPath)
        ' Load the output data block configuration
        PowerUp = PowerUp Or cPLCDataBlock.LoadConfiguration(_outputDB, outputDBConfigurationPath)
    End Function



    Public Function ReadInputDataBlock() As Boolean
        If (_HardwareEnabled) Then
            ' Read the input data block
            ReadInputDataBlock = mPLC.ReadDataBlock(_inputDB, 0, _inputDB.VariableCount - 1)
        Else
            ReadInputDataBlock = False
        End If
    End Function



    Public Function WriteOutputDataBlock() As Boolean
        If (_HardwareEnabled) Then
            ' Write the output data block
            WriteOutputDataBlock = mPLC.WriteDataBlock(_outputDB, 0, _outputDB.VariableCount - 1)
        Else
            WriteOutputDataBlock = False
        End If
    End Function

    Public Function DataDescription(ByVal _Data As Integer) As String
        ' Return the phase description
        Select Case _Data
            Case 0
                DataDescription = "WS04-Reserve"
            Case 1
                DataDescription = "PartTypeNumber "
            Case 2
                DataDescription = "Test_Mode "
            Case 3
                DataDescription = "PartModel "
            Case 4
                DataDescription = "Reference "
            Case 5
                DataDescription = "WS04-UniqueNumber "
            Case 6
                DataDescription = "HW_Version "
            Case 7
                DataDescription = "SW_Version "
            Case 8
                DataDescription = "NVM_Version "
            Case Else
                DataDescription = String.Format("Value {0} unknown", _Data)
        End Select
    End Function

End Module

