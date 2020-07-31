Option Explicit On
'Option Strict On

Module mWS05Ethernet
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

        Test_Enable_Mirror = 7
        Test_Enable_PLC = 8
        MirrorUP_Correct_X = 9
        MirrorUP_Correct_Y = 10
        MirrorUP_Correct_Z = 11
        MirrorUP_Z_TOUCH = 12
        MirrorDN_Correct_X = 13
        MirrorDN_Correct_Y = 14
        MirrorDN_Correct_Z = 15
        MirrorDN_Z_TOUCH = 16
        MirrorMR_Correct_X = 17
        MirrorMR_Correct_Y = 18
        MirrorMR_Correct_Z = 19
        MirrorMR_Z_TOUCH = 20
        MirrorML_Correct_X = 21
        MirrorML_Correct_Y = 22
        MirrorML_Correct_Z = 23
        MirrorML_Z_TOUCH = 24
        MirrorSR_Correct_X = 25
        MirrorSR_Correct_Y = 26
        MirrorSR_Correct_Z = 27
        MirrorSR_Z_TOUCH = 28
        MirrorSL_Correct_X = 29
        MirrorSL_Correct_Y = 30
        MirrorSL_Correct_Z = 31
        MirrorSL_Z_TOUCH = 32

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
                DataDescription = "WS05-Reserve"
            Case 1
                DataDescription = "PartTypeNumber "
            Case 2
                DataDescription = "Test_Mode "
            Case 3
                DataDescription = "PartModel "
            Case 4
                DataDescription = "Reference "
            Case 5
                DataDescription = "WS05-UniqueNumber "
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

