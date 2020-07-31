Public Class cRunningTime
    Private _stationName As String
    Private _runningtimer As DateTime
    Private _starttimer As DateTime
    Public Sub New(ByVal stationName As String)
        _stationName = stationName
        _starttimer = DateTime.Now
        _runningtimer = DateTime.Now
    End Sub

    Public Sub ResetStartTime()
        _starttimer = DateTime.Now
    End Sub
    Public Sub ResetRunningTime()
        _runningtimer = DateTime.Now
    End Sub
    Public Function ReturnRunningTime() As Double
        Dim temp As Double = (DateTime.Now - _runningtimer).TotalMilliseconds
        Return temp
    End Function
    Public Function ReturnStartTime() As Double
        Dim temp As Double = (DateTime.Now - _starttimer).TotalMilliseconds
        Return temp
    End Function
End Class
