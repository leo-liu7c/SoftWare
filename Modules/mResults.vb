Option Explicit On
Option Strict Off

Imports System
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.IO

Module mResults
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    Public Const XMLResultPath As String = "C:\TestResult\"
    Public Const FTPLocalFilePath As String = "C:\Test_Software\FSScreenshotPath\"



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function Load(ByVal path As String, _
                         ByVal value() As cResultValue) As Boolean
        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim token(0 To 3) As String

        Try
            ' Open the file
            file = New StreamReader(path, System.Text.Encoding.Default)
            ' While it is not reached the end of the file
            Do While Not (file.EndOfStream)
                ' Read a line
                line = file.ReadLine
                ' Split the line
                token = Split(line, vbTab)
                ' Load the value
                i = CInt(token(0))
                value(i).Value = token(2)
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

    Public Function LoadConfiguration(ByVal path As String, _
                                      ByVal values() As cResultValue) As Boolean
        Dim file As StreamReader = Nothing
        Dim i As Integer
        Dim line As String
        Dim token(0 To 11) As String

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
                        values(i) = New cResultValue(cResultValue.eValueType.BCDValue)
                    ElseIf (token(2) = "X") Then
                        values(i) = New cResultValue(cResultValue.eValueType.HexValue)
                    ElseIf (token(3) = "X") Then
                        values(i) = New cResultValue(cResultValue.eValueType.IntegerValue)
                    ElseIf (token(4) = "X") Then
                        values(i) = New cResultValue(cResultValue.eValueType.SingleValue)
                    ElseIf (token(5) = "X") Then
                        values(i) = New cResultValue(cResultValue.eValueType.StringValue)
                    Else
                        values(i) = Nothing
                    End If
                    If (values(i) IsNot Nothing) Then
                        values(i).Description = token(6)
                        values(i).Units = token(7)
                        If (values(i).ValueType = cResultValue.eValueType.BCDValue Or _
                            values(i).ValueType = cResultValue.eValueType.HexValue) Then
                            values(i).ByteSize = CInt(token(8))
                        ElseIf (values(i).ValueType = cResultValue.eValueType.SingleValue) Then
                            values(i).Decimals = CInt(token(9))
                        ElseIf (values(i).ValueType = cResultValue.eValueType.StringValue) Then
                            values(i).MaximumLength = CInt(token(10))
                        End If
                        values(i).TestResult = cResultValue.eValueTestResult.Disabled
                    End If
                End If
            Loop
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

    Public Function Save(ByVal path As String, _
                         ByVal value() As cResultValue) As Boolean
        Dim file As StreamWriter = Nothing
        Dim i As Integer

        Try
            ' Open the file
            file = New StreamWriter(path)
            ' Save all the values
            For i = 0 To value.Length - 1
                If (value(i) IsNot Nothing) Then
                    file.WriteLine(i.ToString & _
                                   vbTab & value(i).Description & _
                                   vbTab & value(i).StringValue & _
                                   vbTab & value(i).Units & _
                                   vbTab & "X")
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

#Region "Added By YAN.Qian20200111"
    ''' <summary>
    '''2, _results.MF_Xe.Value,
    '''_results.WS04_SamplePush(mWS04Main.cSample_Signal.EarlySensor, _results.MF_X_Indexes.Xs),
    '''_results.MF_X_Indexes.Xs, _results.MF_X_Indexes.Xe,
    '''_results.WS04_SamplePush,
    '''_results.MF_X_Indexes.X_F1,
    '''_results.MF_X_Indexes.X_F2,
    '''mWS04Main.cSample_Signal.EarlySensor,
    '''mWS04Main.cSample_Signal.StrenghtSensor, filename
    ''' </summary>
    ''' <param name="NbCurve">Number of Y Curve:Force, Ce1.</param>
    ''' <param name="StrokeEndValue"></param>
    ''' <param name="StrokeStartValue"></param>
    ''' <param name="X_Indexes_Xs"></param>
    ''' <param name="X_Indexes_Xe"></param>
    ''' <param name="Samples">1,Sensor integer;2,Points;</param>
    ''' <param name="X_Indexes_F1"></param>
    ''' <param name="X_Indexes_F2"></param>
    ''' <param name="EarlySensor"></param>
    ''' <param name="StrenghtSensor"></param>
    ''' <param name="FileName"></param>
    Public Sub SavePushReturnFSCurve(ByVal NbCurve As Integer,
                               ByVal StrokeEndValue As Single, ByVal StrokeStartValue As Single,
                               ByVal X_Indexes_Xs As Integer， ByVal X_Indexes_Xe As Integer，
                               ByVal Samples(,) As Single,
                               ByVal X_Indexes_F1 As Integer,
                               ByVal X_Indexes_F2 As Integer,
                               ByVal EarlySensor As Integer,
                               ByVal StrenghtSensor As Integer,
                               ByVal FileName As String)
        Try
            Dim X As Single
            Dim tempGraph As New DataVisualization.Charting.Chart With {
                .Size = New Size(320, 320)
            }
            tempGraph.ChartAreas.Add("ChartArea1")
            ' Reference to the chart
            With tempGraph
                ' Suspend the layout
                .SuspendLayout()
                ' Clear the series
                .Series.Clear()
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisX.Maximum = 10
                If StrokeEndValue > 0 Then
                    .ChartAreas(0).AxisX.Maximum = StrokeEndValue
                End If
                .ChartAreas(0).AxisX.Interval = 0.5

                .ChartAreas(0).AxisY.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = 20
                .ChartAreas(0).AxisY.Interval = 1

                ' Add the series
                For i = 0 To NbCurve - 1
                    .Series.Add(CStr(i))
                    .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .Series(i).BorderWidth = 2
                    If i = 0 Then
                        .Series(i).Name = "Force (N)"
                    ElseIf i = 1 Then
                        .Series(i).Name = "Electric State 1 (V)"
                    End If
                    If StrokeStartValue < 0 Then
                        StrokeStartValue = 0
                    End If
                    For j = X_Indexes_Xs To X_Indexes_Xe
                        ' Strenght curve
                        X = Samples(EarlySensor, j) - StrokeStartValue
                        If i = 0 Then
                            .Series(i).Points.AddXY(X, Samples(StrenghtSensor, j))
                            If (j = X_Indexes_F1 OrElse
                                j = X_Indexes_F2) Then
                                Dim text As String = Nothing
                                If j = X_Indexes_F1 Then
                                    text = "F1"
                                ElseIf j = X_Indexes_F2 Then
                                    text = "F2"
                                End If
                                With .Series(i).Points(.Series(i).Points.Count - 1)
                                    .Label = text
                                    .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                                    .MarkerSize = 10
                                    .MarkerColor = Color.Red
                                End With
                            End If
                        Else
                            'Signal Curve
                            .Series(i).Points.AddXY(X, Samples(2, j))
                        End If
                    Next
                Next
                ' Resume the layout
                .ResumeLayout()
            End With

            tempGraph.Name = FileName
            saveimage(tempGraph)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub

    Public Sub SavePushReturnFSCurve(ByVal NbCurve As Integer,
                               ByVal StrokeEndValue As Single, ByVal StrokeStartValue As Single,
                               ByVal X_Indexes_Xs As Integer， ByVal X_Indexes_Xe As Integer，
                               ByVal Samples(,,) As Single, ByVal Mirror_Step As eMirrorPush,
                               ByVal X_Indexes_F1 As Integer,
                               ByVal X_Indexes_F2 As Integer,
                               ByVal EarlySensor As Integer,
                               ByVal StrenghtSensor As Integer,
                               ByVal FileName As String)
        Try
            Dim X As Single
            Dim tempGraph As New DataVisualization.Charting.Chart With {
                .Size = New Size(320, 320)
            }
            tempGraph.ChartAreas.Add("ChartArea1")
            ' Reference to the chart
            With tempGraph
                ' Suspend the layout
                .SuspendLayout()
                ' Clear the series
                .Series.Clear()
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisX.Maximum = 10
                If StrokeEndValue > 0 Then
                    .ChartAreas(0).AxisX.Maximum = StrokeEndValue
                End If
                .ChartAreas(0).AxisX.Interval = 0.5

                .ChartAreas(0).AxisY.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = 20
                .ChartAreas(0).AxisY.Interval = 1

                ' Add the series
                For i = 0 To NbCurve - 1
                    .Series.Add(CStr(i))
                    .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .Series(i).BorderWidth = 2
                    If i = 0 Then
                        .Series(i).Name = "Force (N)"
                    ElseIf i = 1 Then
                        .Series(i).Name = "Electric State 1 (V)"
                    End If

                    If StrokeStartValue < 0 Then
                        StrokeStartValue = 0
                    End If

                    For j = X_Indexes_Xs To X_Indexes_Xe
                        ' Strenght curve
                        X = Samples(EarlySensor, Mirror_Step, j) - StrokeStartValue
                        If i = 0 Then
                            .Series(i).Points.AddXY(X, Samples(StrenghtSensor, Mirror_Step, j))
                            If (j = X_Indexes_F1 OrElse
                                j = X_Indexes_F2) Then
                                Dim text As String = Nothing
                                If j = X_Indexes_F1 Then
                                    text = "F1"
                                ElseIf j = X_Indexes_F2 Then
                                    text = "F2"
                                End If
                                With .Series(i).Points(.Series(i).Points.Count - 1)
                                    .Label = text
                                    .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                                    .MarkerSize = 10
                                    .MarkerColor = Color.Red
                                End With
                            End If
                        Else
                            'Signal Curve
                            .Series(i).Points.AddXY(X, Samples(2 + Mirror_Step, Mirror_Step, j))
                        End If
                    Next
                Next
                ' Resume the layout
                .ResumeLayout()
            End With

            tempGraph.Name = FileName
            saveimage(tempGraph)

        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub

    ''' <summary>
    '''3, _results.WL_Xe(ePush_Pull.Pull, WiLi_Test).Value,
    '''_results.Samples_Push_Pull(WindowsLifterTest, mWS04Main.cSample_Signal.EarlySensor, _results.WL_X_Indexes(WindowsLifterTest).Xs),
    '''_results.WL_X_Indexes(WindowsLifterTest).Xs, _results.WL_X_Indexes(WindowsLifterTest).Xe,
    '''_results.Samples_Push_Pull,
    '''WindowsLifterTest,
    '''_results.WL_X_Indexes(WindowsLifterTest).X_F1,
    '''_results.WL_X_Indexes(WindowsLifterTest).X_F2,
    '''_results.WL_X_Indexes(WindowsLifterTest).X_F3,
    '''_results.WL_X_Indexes(WindowsLifterTest).X_F4,
    '''False,, ePush_Pull.Push,
    '''mWS04Main.cSample_Signal.EarlySensor,
    '''mWS04Main.cSample_Signal.StrenghtSensor, filename
    ''' </summary>
    ''' <param name="NbCurve">Number of Y Curve:Force, Ce1,Ce2.</param>
    ''' <param name="StrokeEndValue"></param>
    ''' <param name="StrokeStartValue"></param>
    ''' <param name="X_Indexes_Xs"></param>
    ''' <param name="X_Indexes_Xe"></param>
    ''' <param name="Samples">1,WiliPosition, RearLeftPush,RearLeftPull;2,Sensor integer;3,Points;</param>
    ''' <param name="WindowsLifterTest">it's a enum for Samples(,,)</param>
    ''' <param name="X_Indexes_F1"></param>
    ''' <param name="X_Indexes_F2"></param>
    ''' <param name="X_Indexes_F4"></param>
    ''' <param name="X_Indexes_F5"></param>
    ''' <param name="IsPush">True:Push,False:Pull</param>
    ''' <param name="PushPullArrayPosition">Only used for Windowlifter, like ePush_Pull</param>
    ''' <param name="EarlySensor"></param>
    ''' <param name="StrenghtSensor"></param>
    ''' <param name="FileName"></param>
    Public Sub SaveWiLiFSCurve(ByVal NbCurve As Integer,
                               ByVal StrokeEndValue As Single, ByVal StrokeStartValue As Single,
                               ByVal X_Indexes_Xs As Integer， ByVal X_Indexes_Xe As Integer，
                               ByVal Samples(,,) As Single,
                               ByVal WindowsLifterTest As Integer,
                               ByVal X_Indexes_F1 As Integer,
                               ByVal X_Indexes_F2 As Integer,
                               ByVal X_Indexes_F4 As Integer,
                               ByVal X_Indexes_F5 As Integer,
                               ByVal IsPush As Boolean,
                               ByVal PushPullArrayPosition As Integer,
                               ByVal EarlySensor As Integer,
                               ByVal StrenghtSensor As Integer,
                               ByVal FileName As String)
        Try
            Dim X As Single
            Dim tempGraph As New DataVisualization.Charting.Chart With {
                .Size = New Size(320, 320)
            }
            tempGraph.ChartAreas.Add("ChartArea1")
            ' Reference to the chart
            With tempGraph
                ' Suspend the layout
                .SuspendLayout()
                ' Clear the series
                .Series.Clear()
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisX.Maximum = 10
                If StrokeEndValue > 0 Then
                    .ChartAreas(0).AxisX.Maximum = StrokeEndValue
                End If
                .ChartAreas(0).AxisX.Interval = 0.5

                .ChartAreas(0).AxisY.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = 20
                .ChartAreas(0).AxisY.Interval = 1

                ' Add the series
                For i = 0 To NbCurve - 1
                    .Series.Add(CStr(i))
                    .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .Series(i).BorderWidth = 2
                    If i = 0 Then
                        .Series(i).Name = "Force (N)"
                    ElseIf i = 1 Then
                        .Series(i).Name = "Electric State 1 (V)"
                    ElseIf i = 2 Then
                        .Series(i).Name = "Electric State 2 (V)"
                    End If
                    '
                    If StrokeStartValue < 0 Then
                        StrokeStartValue = 0
                    End If
                    '
                    For j = X_Indexes_Xs To X_Indexes_Xe
                        ' Strenght curve
                        X = Samples(WindowsLifterTest, EarlySensor, j) - StrokeStartValue
                        If (IsPush = False) Then
                            X = X * -1
                        End If
                        If i = 0 Then
                            .Series(i).Points.AddXY(X, Samples(WindowsLifterTest, StrenghtSensor, j))
                            If (j = X_Indexes_F1 OrElse
                                j = X_Indexes_F2 OrElse
                                j = X_Indexes_F4 OrElse
                                j = X_Indexes_F5) Then
                                Dim text As String = Nothing
                                If j = X_Indexes_F1 Then
                                    text = "F1"
                                ElseIf j = X_Indexes_F2 Then
                                    text = "F2"
                                ElseIf j = X_Indexes_F4 Then
                                    text = "F4"
                                ElseIf j = X_Indexes_F5 Then
                                    text = "F5"
                                End If
                                With .Series(i).Points(.Series(i).Points.Count - 1)
                                    .Label = text
                                    .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                                    .MarkerSize = 10
                                    .MarkerColor = Color.Red
                                End With
                            End If
                        Else
                            'Signal Curve
                            .Series(i).Points.AddXY(X, Samples(WindowsLifterTest, 2 * PushPullArrayPosition + i + 1, j))
                        End If
                    Next
                Next
                ' Resume the layout
                .ResumeLayout()
            End With

            tempGraph.Name = FileName
            saveimage(tempGraph)

        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub
    Private Sub saveimage(ByVal chart As DataVisualization.Charting.Chart)
        Dim _backgroundworker_filesave As BackgroundWorker = New BackgroundWorker With {
                .WorkerSupportsCancellation = True,
                .WorkerReportsProgress = False
            }
        AddHandler _backgroundworker_filesave.DoWork, Sub(ByVal sender As Object, ByVal e As DoWorkEventArgs)
                                                          Try
                                                              'Save Image.
                                                              If (Directory.Exists(Path.Combine(mResults.FTPLocalFilePath + Format(Date.Now, "yyyyMMdd"))) = False) Then
                                                                  Try
                                                                      Directory.CreateDirectory(Path.Combine(mResults.FTPLocalFilePath + Format(Date.Now, "yyyyMMdd")))
                                                                  Catch ex As Exception

                                                                  End Try
                                                              End If
                                                              CType(e.Argument, DataVisualization.Charting.Chart).SaveImage(Path.Combine(mResults.FTPLocalFilePath + Format(Date.Now, "yyyyMMdd") + "\", Path.ChangeExtension(CType(e.Argument, DataVisualization.Charting.Chart).Name + "_" + Format(Date.Now, "HHmmss"), "png")), ImageFormat.Png)
                                                          Catch ex As Exception
                                                              Console.WriteLine(ex.ToString)
                                                          End Try
                                                      End Sub
        _backgroundworker_filesave.RunWorkerAsync(chart)
    End Sub
    'Private Sub _backgroundworker_filesave_Dowork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles _backgroundworker_filesave.DoWork

    '    Try
    '        CType(e.Argument, DataVisualization.Charting.Chart).SaveImage(Path.Combine(mResults.FTPLocalFilePath + Format(Date.Now, "yyyyMMdd") + "\", Path.ChangeExtension(CType(e.Argument, DataVisualization.Charting.Chart).Name + "_" + Format(Date.Now, "HHmmss"), "png")), ImageFormat.Png)
    '    Catch ex As Exception
    '        Console.WriteLine(ex.ToString)
    '    End Try

    'End Sub

#End Region
    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module