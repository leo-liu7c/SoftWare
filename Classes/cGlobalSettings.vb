Public Class cGlobalRecipeSettings
    Public Property SampleList() As ObjectModel.Collection(Of cSamples)
End Class
Public Class cSamples
    Public Property SN() As String
    Public Property Recipename() As String
    Public Property SampleType() As EnumSampleType
    Public Property NGCode() As Integer

End Class
'Add by YAN.Qian.20200401, for sample Type. in case of overwrite something for Master samples.
Public Enum EnumSampleType
    ''' <summary>
    ''' Normal delivery
    ''' </summary>
    Production
    ''' <summary>
    ''' Or Calibration. skip write configure.
    ''' </summary>
    Master
    ''' <summary>
    ''' OK Sample. all should be OK. otherwise warning.
    ''' </summary>
    OK
    ''' <summary>
    ''' NG Sample. should be NG with NG Code. otherwise waring. 0 means OK.
    ''' </summary>
    NG
End Enum
