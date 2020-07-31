Option Explicit On
Option Strict On

Imports System
Imports System.IO

Public Class cWS05Recipe
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
    Public TestEnable_Mirror_Electrical As cRecipeValue
    Public TestEnable_Mirror_Strenght As cRecipeValue
    Public TestEnable_EV_Option As cRecipeValue

    ' General settings																				
    Public PowerSupplyVoltage As cRecipeValue
    Public PowerSupplyCurrentLimit As cRecipeValue

    ' Limits on standard signals																				
    Public StdSign_Powersupply(0 To 1) As cRecipeValue

    ' Rear Right
    Public X_correction_approachment_Push(0 To 5) As cRecipeValue
    Public Y_correction_approachment_Push(0 To 5) As cRecipeValue
    Public Z_correction_approachment_Push(0 To 5) As cRecipeValue
    Public Z_Vector_Final_Position_Push(0 To 5) As cRecipeValue

    Public Mirror_Fs1_F1(0 To 5) As cRecipeValue
    Public Mirror_Xs1(0 To 5) As cRecipeValue
    Public Mirror_dFs1_Haptic_1(0 To 5) As cRecipeValue
    Public Mirror_dXs1(0 To 5) As cRecipeValue
    Public Mirror_FsCe1(0 To 5) As cRecipeValue
    Public Mirror_XCe1(0 To 5) As cRecipeValue
    Public Mirror_Fe(0 To 5) As cRecipeValue
    Public Mirror_Xe(0 To 5) As cRecipeValue
    Public Mirror_DiffS2Ce1(0 To 5) As cRecipeValue

    Public Correlation_Factor_Force_A(0 To 5) As cRecipeValue
    Public Correlation_Factor_Force_B(0 To 5) As cRecipeValue
    Public Correlation_Factor_Stroke_A(0 To 5) As cRecipeValue
    Public Correlation_Factor_Stroke_B(0 To 5) As cRecipeValue

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
        Dim ii As Integer
        ' Load the recipe configuration
        LoadConfiguration = mRecipe.LoadConfiguration(path, _values)

        ' General information																				
        CreationDate = _values(0)
        CreationTime = _values(1)
        ModifyDate = _values(2)
        ModifyTime = _values(3)
        i = 10
        ' Test enables																				
        TestEnable_Mirror_Electrical = _values(i) : i = i + 1
        TestEnable_Mirror_Strenght = _values(i) : i = i + 1
        TestEnable_EV_Option = _values(i) : i = i + 1

        ' General settings																				
        PowerSupplyVoltage = _values(100)
        PowerSupplyCurrentLimit = _values(101)

        ' Limits on standard signals																				
        For i = 0 To 1
            StdSign_Powersupply(i) = _values(110 + i)
        Next

        For ii = 0 To 5
            i = 120
            X_correction_approachment_Push(ii) = _values(i + (ii * 10)) : i = i + 1
            Y_correction_approachment_Push(ii) = _values(i + (ii * 10)) : i = i + 1
            Z_correction_approachment_Push(ii) = _values(i + (ii * 10)) : i = i + 1
            Z_Vector_Final_Position_Push(ii) = _values(i + (ii * 10)) : i = i + 1
            '
        Next ii

        i = 200
        For ii = 0 To 5
            Mirror_Fs1_F1(ii) = _values(i) : i = i + 1
            Mirror_Xs1(ii) = _values(i) : i = i + 1
            Mirror_dFs1_Haptic_1(ii) = _values(i) : i = i + 1
            Mirror_dXs1(ii) = _values(i) : i = i + 1
            Mirror_FsCe1(ii) = _values(i) : i = i + 1
            Mirror_XCe1(ii) = _values(i) : i = i + 1
            Mirror_Fe(ii) = _values(i) : i = i + 1
            Mirror_Xe(ii) = _values(i) : i = i + 1
            Mirror_DiffS2Ce1(ii) = _values(i) : i = i + 2
        Next

        Dim c As Integer
        i = 300
        For c = 0 To 5
            Correlation_Factor_Force_A(c) = _values(i) : i = i + 1
            Correlation_Factor_Force_B(c) = _values(i) : i = i + 1
        Next
        i = 320
        For c = 0 To 5
            Correlation_Factor_Stroke_A(c) = _values(i) : i = i + 1
            Correlation_Factor_Stroke_B(c) = _values(i) : i = i + 1
        Next

    End Function



    Public Shared Function ReadList(ByRef names() As String) As Boolean
        ' Read the recipe list
        ReadList = mRecipe.ReadList(mConstants.BasePath & mWS05Main.Settings.RecipePath, names)
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
            FilePath = mConstants.BasePath & mWS05Main.Settings.MasterReferencePath & "\M" & name & ".txt"
        Else
            ' Return the file path
            FilePath = mConstants.BasePath & mWS05Main.Settings.RecipePath & "\" & name & ".txt"
        End If
    End Function
End Class
