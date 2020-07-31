Imports System.ComponentModel
''' <summary>
''' VB Class for generate test result based on XMLSchema_EOLT.
''' please follow the step 1 to step 4.
''' Version:1.0.0.4:Update Test mode.
''' </summary>
Public Class GenerateXMLResult

    ''' <summary>
    ''' Unit Array. one result can be include more units. 
    ''' (one station have more than one unit, or one rotate station have a few sub stations.)
    ''' </summary>
    Public LocalUnit() As UnitType
    ''' <summary>
    ''' Measurements Array. one unit can have a lot of measurements.
    ''' </summary>
    Public LocalMeasurements() As MeasurementType
    ''' <summary>
    ''' FSMeasurements Array. one unit can have a lot of measurements.
    ''' </summary>
    Public LocalFSMeasurements() As FSMeasurementType

    Private _StepNumber As Int16

    Private WithEvents _backgroundworker_filesave As BackgroundWorker

    '''' <summary>
    '''' Samples function for Labview. not offical use.
    '''' </summary>
    '''' <param name="FileName"></param>
    '''' <param name="WorkOrderNumber"></param>
    '<Obsolete>
    'Public Sub CreateXML(ByVal FileName As String, ByVal WorkOrderNumber As String)

    '    Dim Temp As New Test_Result
    '    Temp.WorkOrderNumber = WorkOrderNumber
    '    'Dim FileName As String = "TestforLabview1.xml"
    '    Try
    '        Dim FileInfo As New IO.FileInfo(FileName)
    '        If FileInfo.Exists Then FileInfo.Delete()
    '        If Not FileInfo.Directory.Exists Then FileInfo.Directory.Create()
    '        Dim book As IO.StreamWriter
    '        book = New IO.StreamWriter(FileName)
    '        Dim Deser As New Xml.Serialization.XmlSerializer(Temp.GetType)
    '        Deser.Serialize(book, Temp)
    '        book.Close()

    '    Catch ex As Exception

    '    End Try

    'End Sub

    Public Sub New(ByVal SourceName As String, ByRef TargetName As String)

        Try
            Dim tempFileName As String = System.IO.Path.GetFileName(SourceName)
            If System.IO.Path.GetExtension(tempFileName) = ".txt" Then
                tempFileName = tempFileName.Replace(".txt", ".xml")
            End If
            TargetName = mResults.XMLResultPath + tempFileName
            _backgroundworker_filesave = New BackgroundWorker With {
                .WorkerSupportsCancellation = True,
                .WorkerReportsProgress = False
            }

        Catch ex As Exception

        End Try
    End Sub


    ''' <summary>
    ''' Step1. Initialise variables.
    ''' </summary>
    Public Sub InitilseXMLData()
        LocalUnit = Nothing
        LocalMeasurements = Nothing
        LocalFSMeasurements = Nothing
        _StepNumber = 0
    End Sub

    ''' <summary>
    ''' Step2. Create Measurements. Each measurement call one time.
    ''' </summary>
    ''' <param name="StepNumber"></param>
    ''' <param name="Name"></param>
    ''' <param name="Max"></param>
    ''' <param name="Min"></param>
    ''' <param name="Value"></param>
    ''' <param name="Unit"></param>
    ''' <param name="StatusCode"></param>
    ''' <param name="DateTimeEnd"></param>
    ''' <param name="DateTimeStart"></param>
    Public Sub CreateMeasurementsForUnit(ByVal StepNumber As Integer,
                                         ByVal Name As String,
                                         ByVal Max As String,
                                         ByVal Min As String,
                                         ByVal Value As String,
                                         ByVal Unit As String,
                                         ByVal StatusCode As UnitStatus,
                                         Optional ByVal DateTimeStart As Date = Nothing,
                                         Optional ByVal DateTimeEnd As Date = Nothing,
                                         Optional ByVal TestTime As Double = 0)

        If LocalMeasurements Is Nothing Then
            ReDim LocalMeasurements(0)
        Else
            ReDim Preserve LocalMeasurements(LocalMeasurements.Length)
        End If

        LocalMeasurements(LocalMeasurements.Length - 1) = New MeasurementType
        Try
            With LocalMeasurements(LocalMeasurements.Length - 1)
                If DateTimeEnd <> Nothing Then
                    .DateTimeEnd = DateTimeEnd
                    .DateTimeEndSpecified = True
                Else
                    .DateTimeEndSpecified = False
                End If
                If DateTimeStart <> Nothing Then
                    .DateTimeStart = DateTimeStart
                    .DateTimeStartSpecified = True
                Else
                    .DateTimeStartSpecified = False
                End If
                .Max = Max
                .Min = Min
                .Name = Name
                .StatusCode = StatusCode
                .StepNumber = StepNumber
                .StepNumberSpecified = True
                .Unit = Unit
                .Value = Value
                .MeasurementTestTime = TestTime
            End With
        Catch ex As Exception

        End Try

    End Sub

    Public Sub CreateMeasurementsSaveResultValueForUnit(ByVal Result As cResultValue,
                                     ByVal GroupName As String,
                                     Optional ByVal StepNumber As Integer = 0,
                                     Optional ByVal DateTimeStart As Date = Nothing,
                                     Optional ByVal DateTimeEnd As Date = Nothing,
                                     Optional ByVal TestTime As Double = 0)

        Dim tempStatus As MeasurementStatus = MeasurementStatus.FAIL
        Select Case Result.TestResult
            Case cResultValue.eValueTestResult.Disabled
                tempStatus = MeasurementStatus.Disabled
                Return
            Case cResultValue.eValueTestResult.Failed
                tempStatus = MeasurementStatus.FAIL
            Case cResultValue.eValueTestResult.NotCoherent
                tempStatus = MeasurementStatus.NotCoherent
            Case cResultValue.eValueTestResult.NotTested
                tempStatus = MeasurementStatus.NotTested
            Case cResultValue.eValueTestResult.NotValueCheck
                tempStatus = MeasurementStatus.NotValueCheck
            Case cResultValue.eValueTestResult.Passed
                tempStatus = MeasurementStatus.PASS
            Case cResultValue.eValueTestResult.Unknown
                tempStatus = MeasurementStatus.Unknown
        End Select

        If StepNumber <> 0 Then
            _StepNumber = StepNumber
        Else
            _StepNumber += 1
        End If

        If LocalMeasurements Is Nothing Then
            ReDim LocalMeasurements(0)
        Else
            ReDim Preserve LocalMeasurements(LocalMeasurements.Length)
        End If

        LocalMeasurements(LocalMeasurements.Length - 1) = New MeasurementType
        Try
            With LocalMeasurements(LocalMeasurements.Length - 1)
                If DateTimeEnd <> Nothing Then
                    .DateTimeEnd = DateTimeEnd
                    .DateTimeEndSpecified = True
                Else
                    .DateTimeEndSpecified = False
                End If
                If DateTimeStart <> Nothing Then
                    .DateTimeStart = DateTimeStart
                    .DateTimeStartSpecified = True
                Else
                    .DateTimeStartSpecified = False
                End If
                .Max = Result.StringMaximumLimit
                .Min = Result.StringMinimumLimit
                .Name = (GroupName & Result.Description).Replace(" ", "")
                .StatusCode = tempStatus
                .StepNumber = _StepNumber
                .StepNumberSpecified = True
                .Unit = Result.Units
                .Value = Result.StringValue
                .MeasurementTestTime = TestTime
            End With
        Catch ex As Exception

        End Try

    End Sub

    Public Sub CreateMeasurementsSaveSingleTestResultForUnit(ByVal Result As cResultValue,
                                     ByVal GroupName As String,
                                     Optional ByVal StepNumber As Integer = 0,
                                     Optional ByVal DateTimeStart As Date = Nothing,
                                     Optional ByVal DateTimeEnd As Date = Nothing,
                                     Optional ByVal TestTime As Double = 0)

        Dim tempStatus As MeasurementStatus = MeasurementStatus.FAIL
        Select Case Result.TestResult
            Case cResultValue.eValueTestResult.Disabled
                tempStatus = MeasurementStatus.Disabled
                Return
            Case cResultValue.eValueTestResult.Failed
                tempStatus = MeasurementStatus.FAIL
            Case cResultValue.eValueTestResult.NotCoherent
                tempStatus = MeasurementStatus.NotCoherent
            Case cResultValue.eValueTestResult.NotTested
                tempStatus = MeasurementStatus.NotTested
            Case cResultValue.eValueTestResult.NotValueCheck
                tempStatus = MeasurementStatus.NotValueCheck
            Case cResultValue.eValueTestResult.Passed
                tempStatus = MeasurementStatus.PASS
            Case cResultValue.eValueTestResult.Unknown
                tempStatus = MeasurementStatus.Unknown
        End Select

        If StepNumber <> 0 Then
            _StepNumber = StepNumber
        Else
            _StepNumber += 1
        End If

        If LocalMeasurements Is Nothing Then
            ReDim LocalMeasurements(0)
        Else
            ReDim Preserve LocalMeasurements(LocalMeasurements.Length)
        End If

        LocalMeasurements(LocalMeasurements.Length - 1) = New MeasurementType
        Try
            With LocalMeasurements(LocalMeasurements.Length - 1)
                If DateTimeEnd <> Nothing Then
                    .DateTimeEnd = DateTimeEnd
                    .DateTimeEndSpecified = True
                Else
                    .DateTimeEndSpecified = False
                End If
                If DateTimeStart <> Nothing Then
                    .DateTimeStart = DateTimeStart
                    .DateTimeStartSpecified = True
                Else
                    .DateTimeStartSpecified = False
                End If
                .Max = ""
                .Min = ""
                .Name = (GroupName & Result.Description).Replace(" ", "")
                .StatusCode = tempStatus
                .StepNumber = _StepNumber
                .StepNumberSpecified = True
                .Unit = Result.Units
                .Value = Result.TestResult
                .MeasurementTestTime = TestTime
            End With
        Catch ex As Exception

        End Try

    End Sub


    ''' <summary>
    ''' Step2.1. Create FSMeasurements if have. Each FSmeasurement call one time.
    ''' </summary>
    ''' <param name="Keyname"></param>
    ''' <param name="Force"></param>
    ''' <param name="Stroke"></param>
    ''' <param name="Switch"></param>
    ''' <param name="Remark"></param>
    ''' <param name="StartTime"></param>
    ''' <param name="ID"></param>
    Public Sub CreateFSMeasurementsForUnit(ByVal Keyname As String,
                                           Optional ByVal Force As Double = 0,
                                         Optional ByVal Stroke As Double = 0,
                                         Optional ByVal Switch As Integer = 0,
                                         Optional ByVal Remark As String = "",
                                         Optional ByVal StartTime As DateTime = #1/1/1900#,
                                         Optional ByVal ID As Long = 0)

        If LocalFSMeasurements Is Nothing Then
            ReDim LocalFSMeasurements(0)
        Else
            ReDim Preserve LocalFSMeasurements(LocalFSMeasurements.Length)
        End If

        LocalFSMeasurements(LocalFSMeasurements.Length - 1) = New FSMeasurementType
        Try
            With LocalFSMeasurements(LocalFSMeasurements.Length - 1)
                .KeyName = Keyname
                If Force <> Nothing Then
                    .Force = Force
                    .ForceSpecified = True
                Else
                    .ForceSpecified = False
                End If
                If Stroke <> Nothing Then
                    .Stroke = Stroke
                    .StrokeSpecified = True
                Else
                    .StrokeSpecified = False
                End If
                If Switch <> Nothing Then
                    .Switch = Switch
                    .SwitchSpecified = True
                Else
                    .SwitchSpecified = False
                End If
                If Remark <> Nothing Then
                    .Remark = Remark
                    '.RemarkSpecified = True
                Else
                    '.RemarkSpecified = False
                End If
                If StartTime <> Nothing Then
                    .StartTime = StartTime
                    .StartTimeSpecified = True
                Else
                    .StartTimeSpecified = False
                End If
                If ID <> Nothing Then
                    .ID = ID
                    .IDSpecified = True
                Else
                    .IDSpecified = False
                End If
            End With
        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' Step3. Create Units. Each unit call one time.
    ''' Once Create Unit, previous Measurements clears.
    ''' </summary>
    ''' <param name="Station"></param>
    ''' <param name="SerialNumber">for running number.</param>
    ''' <param name="Status"></param>
    ''' <param name="RetryCount"></param>
    ''' <param name="DUTDateTimeStart"></param>
    ''' <param name="DUTDateTimeEnd"></param>
    ''' <param name="TestTime"></param>
    ''' <param name="FixtureID"></param>
    ''' <param name="LaserDMC">for laser DMC if this is not same as serial number</param>
    ''' <param name="TESTMODE"></param>
    Public Sub CreateXML_Unit(ByVal Station As Integer,
                                    ByVal SerialNumber As String,
                                    ByVal Status As UnitStatus,
                                    ByVal RetryCount As Integer,
                                    ByVal DUTDateTimeStart As Date,
                                    ByVal DUTDateTimeEnd As Date,
                                    Optional ByVal TestTime As Integer = 0,
                                    Optional ByVal FixtureID As Double = 0,
                                    Optional ByVal LaserDMC As String = "",
                                    Optional ByVal TESTMODE As TestMode = TestMode.MassProduction)

        If LocalUnit Is Nothing Then
            ReDim LocalUnit(0)
        Else
            ReDim Preserve LocalUnit(LocalUnit.Length)
        End If

        Try
            LocalUnit(LocalUnit.Length - 1) = New UnitType
            With LocalUnit(LocalUnit.Length - 1)
                .TestTimeStart = DUTDateTimeStart
                .TestTimeEnd = DUTDateTimeEnd
                .Measurement = LocalMeasurements
                .FSMeasurement = LocalFSMeasurements
                .RetryCount = RetryCount
                .SerialNumber = SerialNumber
                .Station = Station
                .Status = Status
                .UnitTestTime = TestTime
                .FixtureID = FixtureID
                .LaserDMC = LaserDMC
                .TESTMODE = TESTMODE
            End With
        Catch ex As Exception

        End Try

        'Once Create Unit, previous Measurements clears.
        LocalMeasurements = Nothing
        'Once Create Unit, previous FSMeasurements clears.
        LocalFSMeasurements = Nothing

    End Sub
    Private _fileName As String
    ''' <summary>
    ''' Step4. Create Result file, afterwards automatically call InitilseXMLData.
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <param name="DUTDateTimeEnd"></param>
    ''' <param name="DUTDateTimeStart"></param>
    ''' <param name="OperatorNumber"></param>
    ''' <param name="ProductNumber"></param>
    ''' <param name="ProgramName"></param>
    ''' <param name="StationNumber"></param>
    ''' <param name="WorkOrderNumber"></param>
    Public Sub CreateXML(ByVal FileName As String,
                         ByVal DUTDateTimeEnd As Date,
                         ByVal DUTDateTimeStart As Date,
                         ByVal OperatorNumber As String,
                         ByVal ProductNumber As String,
                         ByVal ProgramName As String,
                         ByVal StationNumber As String,
                         ByVal WorkOrderNumber As String)

        Try
            _fileName = FileName
            Dim Temp As New Test_Result With {
            .Unit = LocalUnit,
            .DUTDateTimeEnd = DUTDateTimeEnd,
            .DUTDateTimeStart = DUTDateTimeStart,
            .OperatorNumber = OperatorNumber,
            .ProductNumber = ProductNumber,
            .ProgramName = ProgramName,
            .StationNumber = StationNumber,
            .WorkOrderNumber = WorkOrderNumber
            }
            _backgroundworker_filesave.RunWorkerAsync(Temp)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

    End Sub
    Private Sub _backgroundworker_filesave_Dowork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles _backgroundworker_filesave.DoWork

        Try
            Dim FileInfo As New IO.FileInfo(_fileName)
            If FileInfo.Exists Then FileInfo.Delete()
            If Not FileInfo.Directory.Exists Then FileInfo.Directory.Create()
            Dim book As IO.StreamWriter
            book = New IO.StreamWriter(_fileName)
            Dim Deser As New Xml.Serialization.XmlSerializer(CType(e.Argument, Test_Result).GetType)
            Deser.Serialize(book, CType(e.Argument, Test_Result))
            book.Close()
            'Once Create XML files. Previous Data clears.
            InitilseXMLData()
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

End Class
