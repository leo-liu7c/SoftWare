﻿Option Explicit On
Option Strict On

Imports System
Imports System.IO

Public Class cWS04Recipe
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    'Public constants
    Public Const ValueCount = 500

    ' General information																				
    Public CreationDate As cRecipeValue
    Public CreationTime As cRecipeValue
    Public ModifyDate As cRecipeValue
    Public ModifyTime As cRecipeValue

    ' Test enables																				
    Public TestEnable_Rear_Right_PUSH_Electrical As cRecipeValue
    Public TestEnable_Rear_Right_PUSH_Strenght As cRecipeValue
    Public TestEnable_Rear_Right_PULL_Electrical As cRecipeValue
    Public TestEnable_Rear_Right_PULL_Strenght As cRecipeValue
    Public TestEnable_Rear_Left_PUSH_Electrical As cRecipeValue
    Public TestEnable_Rear_Left_PUSH_Strenght As cRecipeValue
    Public TestEnable_Rear_Left_PULL_Electrical As cRecipeValue
    Public TestEnable_Rear_Left_PULL_Strenght As cRecipeValue
    Public TestEnable_MirrorFolding_Electrical As cRecipeValue
    Public TestEnable_MirrorFolding_Strenght As cRecipeValue
    Public TestEnable_EV_Option As cRecipeValue

    ' General settings																				
    Public PowerSupplyVoltage As cRecipeValue
    Public PowerSupplyCurrentLimit As cRecipeValue

    ' Limits on standard signals																				
    Public StdSign_Powersupply(0 To 1) As cRecipeValue

    ' Rear Right
    Public X_correction_approachment_Rear_Right_Push As cRecipeValue
    Public Y_correction_approachment_Rear_Right_Push As cRecipeValue
    Public Z_correction_approachment_Rear_Right_Push As cRecipeValue
    Public RX_correction_approachment_Rear_Right_Push As cRecipeValue
    Public RY_correction_approachment_Rear_Right_Push As cRecipeValue
    Public RZ_correction_approachment_Rear_Right_Push As cRecipeValue
    Public X_Vector_Final_Position_Rear_Right_Push As cRecipeValue
    Public Z_Vector_Final_Position_Rear_Right_Push As cRecipeValue
    Public X_correction_approachment_Rear_Right_Pull As cRecipeValue
    Public Y_correction_approachment_Rear_Right_Pull As cRecipeValue
    Public Z_correction_approachment_Rear_Right_Pull As cRecipeValue
    Public RX_correction_approachment_Rear_Right_Pull As cRecipeValue
    Public RY_correction_approachment_Rear_Right_Pull As cRecipeValue
    Public RZ_correction_approachment_Rear_Right_Pull As cRecipeValue
    Public X_Vector_Final_Position_Rear_Right_Pull As cRecipeValue
    Public Z_Vector_Final_Position_Rear_Right_Pull As cRecipeValue

    Public Rear_Right_Push_Number_State As cRecipeValue

    Public Rear_Right_Pull_Number_State As cRecipeValue

    ' Rear Left
    Public X_correction_approachment_Rear_Left_Push As cRecipeValue
    Public Y_correction_approachment_Rear_Left_Push As cRecipeValue
    Public Z_correction_approachment_Rear_Left_Push As cRecipeValue
    Public Z_Vector_Final_Position_Rear_Left_Push As cRecipeValue

    Public X_correction_approachment_Rear_Left_Pull As cRecipeValue
    Public Y_correction_approachment_Rear_Left_Pull As cRecipeValue
    Public Z_correction_approachment_Rear_Left_Pull As cRecipeValue
    Public Z_Vector_Final_Position_Rear_Left_Pull As cRecipeValue

    Public Rear_Left_Push_Number_State As cRecipeValue

    Public Rear_Left_Pull_Number_State As cRecipeValue

    ' MirrorFolding Lock
    Public X_correction_approachment_MirrorFolding As cRecipeValue
    Public Y_correction_approachment_MirrorFolding As cRecipeValue
    Public Z_correction_approachment_MirrorFolding As cRecipeValue
    Public Z_Vector_Final_Position_MirrorFolding As cRecipeValue


    '(UP/DN , FR/FL)
    Public WL_Fs1_F1(0 To 1, 0 To 1) As cRecipeValue
    Public WL_Xs1(0 To 1, 0 To 1) As cRecipeValue
    Public WL_dFs1_Haptic_1(0 To 1, 0 To 1) As cRecipeValue
    Public WL_dXs1(0 To 1, 0 To 1) As cRecipeValue
    Public WL_FsCe1(0 To 1, 0 To 1) As cRecipeValue
    Public WL_XCe1(0 To 1, 0 To 1) As cRecipeValue
    Public WL_Fs2_F2(0 To 1, 0 To 1) As cRecipeValue
    Public WL_Xs2(0 To 1, 0 To 1) As cRecipeValue
    Public WL_dFs2_Haptic_2(0 To 1, 0 To 1) As cRecipeValue
    Public WL_dXs2(0 To 1, 0 To 1) As cRecipeValue
    Public WL_FsCe2(0 To 1, 0 To 1) As cRecipeValue
    Public WL_XCe2(0 To 1, 0 To 1) As cRecipeValue
    Public WL_Fe(0 To 1, 0 To 1) As cRecipeValue
    Public WL_Xe(0 To 1, 0 To 1) As cRecipeValue
    Public WL_DiffS2Ce1(0 To 1, 0 To 1) As cRecipeValue
    Public WL_DiffS5Ce2(0 To 1, 0 To 1) As cRecipeValue

    Public MF_Fs1_F1 As cRecipeValue
    Public MF_Xs1 As cRecipeValue
    Public MF_dFs1_Haptic_1 As cRecipeValue
    Public MF_dXs1 As cRecipeValue
    Public MF_FsCe1 As cRecipeValue
    Public MF_XCe1 As cRecipeValue
    Public MF_Fe As cRecipeValue
    Public MF_Xe As cRecipeValue
    Public MF_DiffS2Ce1 As cRecipeValue

    Public Correlation_Factor_Force_A(0 To 4) As cRecipeValue
    Public Correlation_Factor_Force_B(0 To 4) As cRecipeValue
    Public Correlation_Factor_Stroke_A(0 To 4) As cRecipeValue
    Public Correlation_Factor_Stroke_B(0 To 4) As cRecipeValue

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
        ' Load the recipe configuration
        LoadConfiguration = mRecipe.LoadConfiguration(path, _values)

        ' General information																				
        CreationDate = _values(0)
        CreationTime = _values(1)
        ModifyDate = _values(2)
        ModifyTime = _values(3)
        i = 10
        ' Test enables																				
        TestEnable_Rear_Left_PULL_Electrical = _values(i) : i = i + 1
        TestEnable_Rear_Left_PULL_Strenght = _values(i) : i = i + 1
        TestEnable_Rear_Left_PUSH_Electrical = _values(i) : i = i + 1
        TestEnable_Rear_Left_PUSH_Strenght = _values(i) : i = i + 1
        TestEnable_Rear_Right_PULL_Electrical = _values(i) : i = i + 1
        TestEnable_Rear_Right_PULL_Strenght = _values(i) : i = i + 1
        TestEnable_Rear_Right_PUSH_Electrical = _values(i) : i = i + 1
        TestEnable_Rear_Right_PUSH_Strenght = _values(i) : i = i + 1
        TestEnable_MirrorFolding_Electrical = _values(i) : i = i + 1
        TestEnable_MirrorFolding_Strenght = _values(i) : i = i + 1
        TestEnable_EV_Option = _values(i) : i = i + 1

        ' General settings																				
        PowerSupplyVoltage = _values(100)
        PowerSupplyCurrentLimit = _values(101)

        ' Limits on standard signals																				
        For i = 0 To 1
            StdSign_Powersupply(i) = _values(110 + i)
        Next
        i = 120
        X_correction_approachment_Rear_Right_Push = _values(i) : i = i + 1
        Y_correction_approachment_Rear_Right_Push = _values(i) : i = i + 1
        Z_correction_approachment_Rear_Right_Push = _values(i) : i = i + 1
        Z_Vector_Final_Position_Rear_Right_Push = _values(i) : i = i + 1
        '
        X_correction_approachment_Rear_Right_Pull = _values(i) : i = i + 1
        Y_correction_approachment_Rear_Right_Pull = _values(i) : i = i + 1
        Z_correction_approachment_Rear_Right_Pull = _values(i) : i = i + 1
        Z_Vector_Final_Position_Rear_Right_Pull = _values(i) : i = i + 1
        '
        i = 137
        Rear_Right_Push_Number_State = _values(i) : i = i + 1
        '
        i = 147
        Rear_Right_Pull_Number_State = _values(i) : i = i + 1

        i = 150
        X_correction_approachment_Rear_Left_Push = _values(i) : i = i + 1
        Y_correction_approachment_Rear_Left_Push = _values(i) : i = i + 1
        Z_correction_approachment_Rear_Left_Push = _values(i) : i = i + 1
        Z_Vector_Final_Position_Rear_Left_Push = _values(i) : i = i + 1
        '
        X_correction_approachment_Rear_Left_Pull = _values(i) : i = i + 1
        Y_correction_approachment_Rear_Left_Pull = _values(i) : i = i + 1
        Z_correction_approachment_Rear_Left_Pull = _values(i) : i = i + 1
        Z_Vector_Final_Position_Rear_Left_Pull = _values(i) : i = i + 1
        '
        i = 167
        Rear_Left_Push_Number_State = _values(i) : i = i + 1
        '
        i = 177
        Rear_Left_Pull_Number_State = _values(i) : i = i + 1

        i = 180
        X_correction_approachment_MirrorFolding = _values(i) : i = i + 1
        Y_correction_approachment_MirrorFolding = _values(i) : i = i + 1
        Z_correction_approachment_MirrorFolding = _values(i) : i = i + 1
        Z_Vector_Final_Position_MirrorFolding = _values(i) : i = i + 1

        i = 190
        i = 210
        WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Xs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_dXs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_FsCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_XCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Xs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_dXs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_FsCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_XCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Fe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Xe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1

        i = 230
        WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Xs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_dXs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_FsCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_XCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Xs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_dXs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_FsCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_XCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Fe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_Xe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1
        WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft) = _values(i) : i = i + 1

        i = 250
        WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Xs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_dXs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_FsCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_XCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Xs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_dXs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_FsCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_XCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Fe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Xe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1

        i = 270
        WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Xs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_dXs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_FsCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_XCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Xs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_dXs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_FsCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_XCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Fe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_Xe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1
        WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight) = _values(i) : i = i + 1

        i = 290
        MF_Fs1_F1 = _values(i) : i = i + 1
        MF_Xs1 = _values(i) : i = i + 1
        MF_dFs1_Haptic_1 = _values(i) : i = i + 1
        MF_dXs1 = _values(i) : i = i + 1
        MF_FsCe1 = _values(i) : i = i + 1
        MF_XCe1 = _values(i) : i = i + 1
        MF_Fe = _values(i) : i = i + 1
        MF_Xe = _values(i) : i = i + 1
        MF_DiffS2Ce1 = _values(i) : i = i + 1

        Dim c As Integer
        i = 300
        For c = 0 To 4
            Correlation_Factor_Force_A(c) = _values(i) : i = i + 1
            Correlation_Factor_Force_B(c) = _values(i) : i = i + 1
        Next
        i = 320
        For c = 0 To 4
            Correlation_Factor_Stroke_A(c) = _values(i) : i = i + 1
            Correlation_Factor_Stroke_B(c) = _values(i) : i = i + 1
        Next


    End Function



    Public Shared Function ReadList(ByRef names() As String) As Boolean
        ' Read the recipe list
        ReadList = mRecipe.ReadList(mConstants.BasePath & mWS04Main.Settings.RecipePath, names)
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
            FilePath = mConstants.BasePath & mWS04Main.Settings.MasterReferencePath & "\M" & name & ".txt"
        Else
            ' Return the file path
            FilePath = mConstants.BasePath & mWS04Main.Settings.RecipePath & "\" & name & ".txt"
        End If
    End Function
End Class
