Option Explicit On
Option Strict On

Imports System
Imports System.IO

Module mSettings
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private _usersConfigurationPath As String
    Private _WS02SettingsPath As String
    Private _WS03SettingsPath As String
    Private _WS04SettingsPath As String
    Private _WS05SettingsPath As String
    Private _PLCIPAddress As String
    Private _VIPAIPAddress As String
    Private _PowerSupplyIPaddress_1 As String
    Private _PowerSupplyIPaddress_2 As String
    Private _Valeo_Plant As String
    Private _LineNumber As String

    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+

    Public ReadOnly Property WS02SettingsPath As String
        Get
            WS02SettingsPath = _WS02SettingsPath
        End Get
    End Property

    Public ReadOnly Property WS03SettingsPath As String
        Get
            WS03SettingsPath = _WS03SettingsPath
        End Get
    End Property

    Public ReadOnly Property WS04SettingsPath As String
        Get
            WS04SettingsPath = _WS04SettingsPath
        End Get
    End Property

    Public ReadOnly Property WS05SettingsPath As String
        Get
            WS05SettingsPath = _WS05SettingsPath
        End Get
    End Property

    Public ReadOnly Property UsersConfigurationPath As String
        Get
            UsersConfigurationPath = _usersConfigurationPath
        End Get
    End Property

    Public ReadOnly Property PLCIPAddress As String
        Get
            PLCIPAddress = _PLCIPAddress
        End Get
    End Property

    Public ReadOnly Property VIPAIPAddress As String
        Get
            VIPAIPAddress = _VIPAIPAddress
        End Get
    End Property

    Public ReadOnly Property PowerSupplyIPaddress_1 As String
        Get
            PowerSupplyIPaddress_1 = _PowerSupplyIPaddress_1
        End Get
    End Property

    Public ReadOnly Property PowerSupplyIPaddress_2 As String
        Get
            PowerSupplyIPaddress_2 = _PowerSupplyIPaddress_2
        End Get
    End Property

    Public ReadOnly Property Valeo_Plant As String
        Get
            Valeo_Plant = _Valeo_Plant
        End Get
    End Property

    Public ReadOnly Property LineNumber As String
        Get
            LineNumber = _LineNumber
        End Get
    End Property

    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function Load() As Boolean
        Dim file As StreamReader = Nothing
        Dim line As String
        Dim token() As String

        Try
            ' Open the file
            file = New StreamReader(SettingsPath, System.Text.Encoding.Default)
            ' Load the settings
            line = file.ReadLine
            line = file.ReadLine
            ' PLC IP address
            line = file.ReadLine
            token = Split(line, vbTab)
            _PLCIPAddress = token(1)
            ' VIPA IP address
            line = file.ReadLine
            token = Split(line, vbTab)
            _VIPAIPAddress = token(1)
            ' TTI IP address
            line = file.ReadLine
            token = Split(line, vbTab)
            _PowerSupplyIPaddress_1 = token(1)
            ' TTI IP address
            line = file.ReadLine
            token = Split(line, vbTab)
            _PowerSupplyIPaddress_2 = token(1)
            ' Users configuration path
            line = file.ReadLine
            token = Split(line, vbTab)
            _usersConfigurationPath = token(1)
            ' Station 060 settings path
            line = file.ReadLine
            token = Split(line, vbTab)
            _WS02SettingsPath = token(1)
            ' Station 070 settings path
            line = file.ReadLine
            token = Split(line, vbTab)
            _WS03SettingsPath = token(1)
            ' Station 080 settings path
            line = file.ReadLine
            token = Split(line, vbTab)
            _WS04SettingsPath = token(1)
            ' Station 080 settings path
            line = file.ReadLine
            token = Split(line, vbTab)
            _WS05SettingsPath = token(1)
            ' Valeo Plant
            line = file.ReadLine
            token = Split(line, vbTab)
            _Valeo_Plant = token(1)
            ' Line Number
            line = file.ReadLine
            token = Split(line, vbTab)
            _LineNumber = token(1)

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

#Region "XML"
    Public XMLSaveLock As New Object
    Public Function LoadXML(ByVal FileName As String, ByRef result As Object, Optional ByVal TargetType As Type = Nothing) As Boolean

        LoadXML = False
        Try
            Dim FileInfo As New IO.FileInfo(FileName)
            If FileInfo.Exists Then
                Dim book As IO.StreamReader
                book = New IO.StreamReader(FileName)
                Dim Deser As Xml.Serialization.XmlSerializer
                If TargetType Is Nothing Then
                    Deser = New Xml.Serialization.XmlSerializer(result.GetType)
                Else
                    Deser = New Xml.Serialization.XmlSerializer(TargetType)
                End If
                result = Deser.Deserialize(book)
                book.Close()
                Return True
            End If
        Catch ex As Exception
            'ClsLogging.WriteLog(GetType(ClsCommon), "Error from LoadXML,File Name: " & FileName & ",Ex:" & ex.ToString, ClsLogging.LogTypes.Errors)
            Return False
        End Try

    End Function

    Public Sub SaveXml(ByVal FileName As String, ByVal ObjectToSave As Object, Optional ByVal TargetType As Type = Nothing)

        SyncLock XMLSaveLock
            Try
                Dim FileInfo As New IO.FileInfo(FileName)
                If FileInfo.Exists Then FileInfo.Delete()
                If Not FileInfo.Directory.Exists Then FileInfo.Directory.Create()
                Dim book As IO.StreamWriter
                book = New IO.StreamWriter(FileName)
                Dim Deser As Xml.Serialization.XmlSerializer
                If TargetType Is Nothing Then
                    Deser = New Xml.Serialization.XmlSerializer(ObjectToSave.GetType)
                Else
                    Deser = New Xml.Serialization.XmlSerializer(TargetType)
                End If
                Deser.Serialize(book, ObjectToSave)
                book.Close()
            Catch ex As Exception
                'ClsLogging.WriteLog(GetType(ClsCommon), "Error from SaveXml,Ex:" & ex.ToString, ClsLogging.LogTypes.Errors)
            End Try
        End SyncLock

    End Sub

#End Region
    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module
