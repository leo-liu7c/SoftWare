Option Explicit On
Option Strict On

Imports System
Imports System.IO

Module mRecipe
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function Delete(ByVal path As String) As Boolean
        Try
            ' Delete the file
            My.Computer.FileSystem.DeleteFile(path)
            ' Return False
            Delete = False

        Catch ex As Exception
            ' Return True
            Delete = True
        End Try
    End Function



    Public Function Exists(ByVal path As String) As Boolean
        ' Return True if the recipe already exists, False otherwise
        Exists = File.Exists(path)
    End Function



    Public Function Load(ByVal path As String, _
                         ByRef value() As cRecipeValue) As Boolean
        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim token(0 To 4) As String

        Try
            ' Open the file
            file = New StreamReader(path, System.Text.Encoding.Default)
            ' While it is not reached the end of the file
            Do While Not (file.EndOfStream)
                ' Read a line
                line = file.ReadLine
                ' If the line is not empty and is not a comment
                If (line <> "" AndAlso Not (line.StartsWith("'"))) Then
                    If line.Contains(",") And Separateur <> "," Then
                        line = Replace(line, ",", ".")
                    ElseIf line.Contains(".") And Separateur <> "." Then
                        line = Replace(line, ".", ",")
                    End If

                    ' Split the table
                    token = Split(line, vbTab)
                    ' Load the value
                    i = CInt(token(0))
                    If (value(i) IsNot Nothing) Then
                        If (value(i).ValueType = cRecipeValue.eValueType.BCDRange Or
                            value(i).ValueType = cRecipeValue.eValueType.HexRange Or
                            value(i).ValueType = cRecipeValue.eValueType.IntegerRange Or
                            value(i).ValueType = cRecipeValue.eValueType.SingleRange) Then
                            value(i).MinimumLimit = token(2)
                            value(i).MaximumLimit = token(3)
                        ElseIf (value(i).ValueType = cRecipeValue.eValueType.BCDValue Or
                                value(i).ValueType = cRecipeValue.eValueType.BooleanValue Or
                                value(i).ValueType = cRecipeValue.eValueType.HexValue Or
                                value(i).ValueType = cRecipeValue.eValueType.IntegerValue Or
                                value(i).ValueType = cRecipeValue.eValueType.SingleValue Or
                                value(i).ValueType = cRecipeValue.eValueType.StringValue) Then
                            value(i).Value = token(2)
                        End If
                    End If
                End If
            Loop
            ' Return False
            Load = False

        Catch ex As Exception
            ' Return True
            Load = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function

    Public Function LoadParameters(ByVal path As String, _
         ByRef parameter() As String, _
         ByRef value() As String) As Boolean

        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim token(0 To 4) As String

        i = 0

        Try
            ' Open the file
            file = New StreamReader(path)
            ' While it is not reached the end of the file
            Do While Not (file.EndOfStream)
                ' Read a line
                line = file.ReadLine
                ' If the line is not empty and is not a comment
                If (line <> "" AndAlso Not (line.StartsWith("'"))) Then
                    ' Split the table
                    token = Split(line, vbTab)
                    ' Load the value
                    i = CInt(token(0))
                    parameter(i) = token(1)
                    value(i) = token(2)
                End If
            Loop
            ' Return False
            LoadParameters = False

        Catch ex As Exception
            ' Return True
            LoadParameters = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function


    Public Function LoadConfiguration(ByVal path As String, _
                                      ByRef value() As cRecipeValue) As Boolean
        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim token(0 To 14) As String

        Try
            ' Open the file
            file = New StreamReader(path, System.Text.Encoding.Default)
            ' While it is not reached the end of the file
            Do While Not (file.EndOfStream)
                ' Read a line
                line = file.ReadLine
                ' If the line is not empty and is not a comment
                If (line <> "" AndAlso Not (line.StartsWith("'"))) Then
                    If line.Contains(",") And Separateur <> "," Then
                        line = Replace(line, ",", ".")
                    ElseIf line.Contains(".") And Separateur <> "." Then
                        line = Replace(line, ".", ",")
                    End If
                    ' Split the line
                    token = Split(line, vbTab)
                    ' Load the value configuration
                    i = CInt(token(0))
                    If (token(1) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.BCDRange)
                    ElseIf (token(2) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.BCDValue)
                    ElseIf (token(3) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.BooleanValue)
                    ElseIf (token(4) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.HexRange)
                    ElseIf (token(5) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.HexValue)
                    ElseIf (token(6) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.IntegerRange)
                    ElseIf (token(7) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.IntegerValue)
                    ElseIf (token(8) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.SingleRange)
                    ElseIf (token(9) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.SingleValue)
                    ElseIf (token(10) = "X") Then
                        value(i) = New cRecipeValue(cRecipeValue.eValueType.StringValue)
                    Else
                        value(i) = Nothing
                    End If
                    If (value(i) IsNot Nothing) Then
                        value(i).Description = token(11)
                        value(i).Units = token(12)
                        If (value(i).ValueType = cRecipeValue.eValueType.BCDRange Or
                            value(i).ValueType = cRecipeValue.eValueType.HexRange) Then
                            value(i).ByteSize = CInt(token(13))
                            value(i).MinimumValue = token(16)
                            value(i).MaximumValue = token(17)
                            value(i).DefaultMinimumLimit = token(18)
                            value(i).DefaultMaximumLimit = token(19)
                        ElseIf (value(i).ValueType = cRecipeValue.eValueType.BCDValue Or
                                value(i).ValueType = cRecipeValue.eValueType.HexValue) Then
                            value(i).ByteSize = CInt(token(13))
                            value(i).MinimumValue = token(16)
                            value(i).MaximumValue = token(17)
                            value(i).DefaultValue = token(20)
                        ElseIf (value(i).ValueType = cRecipeValue.eValueType.BooleanValue) Then
                            value(i).DefaultValue = token(20)
                        ElseIf (value(i).ValueType = cRecipeValue.eValueType.IntegerRange) Then
                            value(i).MinimumValue = token(16)
                            value(i).MaximumValue = token(17)
                            value(i).DefaultMinimumLimit = token(18)
                            value(i).DefaultMaximumLimit = token(19)
                        ElseIf (value(i).ValueType = cRecipeValue.eValueType.IntegerValue) Then
                            value(i).MinimumValue = token(16)
                            value(i).MaximumValue = token(17)
                            value(i).DefaultValue = token(20)
                        ElseIf (value(i).ValueType = cRecipeValue.eValueType.SingleRange) Then
                            value(i).Decimals = CInt(token(14))
                            value(i).MinimumValue = token(16)
                            value(i).MaximumValue = token(17)
                            value(i).DefaultMinimumLimit = token(18)
                            value(i).DefaultMaximumLimit = token(19)
                        ElseIf (value(i).ValueType = cRecipeValue.eValueType.SingleValue) Then
                            value(i).Decimals = CInt(token(14))
                            value(i).MinimumValue = token(16)
                            value(i).MaximumValue = token(17)
                            value(i).DefaultValue = token(20)
                        ElseIf (value(i).ValueType = cRecipeValue.eValueType.StringValue) Then
                            value(i).MaximumLength = CInt(token(15))
                            value(i).DefaultValue = token(20)
                        End If
                    End If
                End If
            Loop
            ' Return False
            LoadConfiguration = False

        Catch ex As Exception
            'Return True
            LoadConfiguration = True

        Finally
            'Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function



    Public Function ReadList(ByVal folder As String, _
                             ByRef name() As String) As Boolean
        Dim baseDirectory As DirectoryInfo
        Dim file() As FileInfo
        Dim i As Integer

        Try
            ' Read the list of files
            baseDirectory = New DirectoryInfo(folder)
            file = baseDirectory.GetFiles
            ReDim name(0 To file.Length - 1)
            For i = 0 To file.Length - 1
                name(i) = Left(file(i).Name, file(i).Name.Length - 4)
            Next
            ' Return False
            ReadList = False

        Catch ex As Exception
            ' Return True
            ReadList = True
        End Try
    End Function



    Public Function Save(ByVal path As String, _
                         ByRef value() As cRecipeValue) As Boolean
        Dim file As StreamWriter = Nothing
        Dim i As Integer

        Try
            ' Open the file
            file = New StreamWriter(path)
            ' Save all the values
            For i = 0 To value.Length - 1
                If (value(i) IsNot Nothing) Then
                    If (value(i).ValueType = cRecipeValue.eValueType.BCDRange Or _
                        value(i).ValueType = cRecipeValue.eValueType.HexRange Or _
                        value(i).ValueType = cRecipeValue.eValueType.IntegerRange Or _
                        value(i).ValueType = cRecipeValue.eValueType.SingleRange) Then
                        file.WriteLine(i.ToString & _
                                       vbTab & value(i).Description & _
                                       vbTab & value(i).StringMinimumLimit & _
                                       vbTab & value(i).StringMaximumLimit & _
                                       vbTab & value(i).Units & _
                                       vbTab & "X")
                    ElseIf (value(i).ValueType = cRecipeValue.eValueType.BCDValue Or _
                            value(i).ValueType = cRecipeValue.eValueType.BooleanValue Or _
                            value(i).ValueType = cRecipeValue.eValueType.HexValue Or _
                            value(i).ValueType = cRecipeValue.eValueType.IntegerValue Or _
                            value(i).ValueType = cRecipeValue.eValueType.SingleValue Or _
                            value(i).ValueType = cRecipeValue.eValueType.StringValue) Then
                        file.WriteLine(i.ToString & _
                                       vbTab & value(i).Description & _
                                       vbTab & value(i).StringValue & _
                                       vbTab & _
                                       vbTab & value(i).Units & _
                                       vbTab & "X")
                    End If
                End If
            Next
            ' Return False
            Save = False

        Catch ex As Exception
            ' Return True
            Save = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try
    End Function



    Public Sub SetToDefaultValues(ByRef value() As cRecipeValue)
        ' Set all the values to the default ones
        For i = 0 To value.Length - 1
            If (value(i) IsNot Nothing) Then
                If (value(i).ValueType = cRecipeValue.eValueType.BCDRange Or _
                    value(i).ValueType = cRecipeValue.eValueType.HexRange Or _
                    value(i).ValueType = cRecipeValue.eValueType.IntegerRange Or _
                    value(i).ValueType = cRecipeValue.eValueType.SingleRange) Then
                    value(i).MinimumLimit = value(i).DefaultMinimumLimit
                    value(i).MaximumLimit = value(i).DefaultMaximumLimit
                ElseIf (value(i).ValueType = cRecipeValue.eValueType.BCDValue Or _
                    value(i).ValueType = cRecipeValue.eValueType.BooleanValue Or _
                    value(i).ValueType = cRecipeValue.eValueType.HexValue Or _
                    value(i).ValueType = cRecipeValue.eValueType.IntegerValue Or _
                    value(i).ValueType = cRecipeValue.eValueType.SingleValue Or _
                    value(i).ValueType = cRecipeValue.eValueType.StringValue) Then
                    value(i).Value = value(i).DefaultValue
                End If
            End If
        Next
    End Sub



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module