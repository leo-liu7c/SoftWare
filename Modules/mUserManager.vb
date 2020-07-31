Option Explicit On
Option Strict On

Imports System
Imports System.IO

Module mUserManager
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Public constants
    ' Public Const MaxAccessLevel = 2

    Public Enum AccessLevelUser
        OP = 0
        Maintenance = 1
        Methodes = 2
        SuperMethodes = 3
        TestEngineer = 4

        count = 5
    End Enum


    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _administratorAccessLevel = 99
    Private Const _administratorUsername = "ADMIN"
    Private Const _administratorPassword = "PSWADMIN"
    Private Const _defaultPassword = "WELCOME"
    Private Const _MasterRefAccessLevel = 98
    Private Const _maxUserCount = 99
    Private Const _noUserIndex = -1

    ' Private variables
    Private _accessLevel(0 To _maxUserCount - 1) As Integer
    Private _currentAccessLevel As Integer
    Private _currentUserIndex As Integer
    Private _currentUsername As String
    Private _userCount As Integer
    Private _username(0 To _maxUserCount - 1) As String
    Private _password(0 To _maxUserCount - 1) As String



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public Property AccessLevel(ByVal userIndex As Integer) As Integer
        Get
            AccessLevel = _accessLevel(userIndex)
        End Get
        Set(ByVal value As Integer)
            If (_currentUsername = _administratorUsername) Then
                _accessLevel(userIndex) = value
            End If
        End Set
    End Property



    Public ReadOnly Property CurrentAccessLevel As Integer
        Get
            CurrentAccessLevel = _currentAccessLevel
        End Get
    End Property



    Public ReadOnly Property CurrentUserIndex As Integer
        Get
            CurrentUserIndex = _currentUserIndex
        End Get
    End Property



    Public ReadOnly Property CurrentUsername As String
        Get
            CurrentUsername = _currentUsername
        End Get
    End Property



    Public ReadOnly Property UserCount As Integer
        Get
            UserCount = _userCount
        End Get
    End Property



    Public ReadOnly Property UserIsAdministrator As Boolean
        Get
            UserIsAdministrator = (_currentUsername = _administratorUsername)
        End Get
    End Property

    Public ReadOnly Property UserIsMasterUser As Boolean
        Get
            UserIsMasterUser = (_currentAccessLevel = AccessLevelUser.SuperMethodes)
        End Get
    End Property


    Public ReadOnly Property UserIsTestEngineer As Boolean
        Get
            UserIsTestEngineer = (_currentAccessLevel = AccessLevelUser.TestEngineer)
        End Get
    End Property

    Public ReadOnly Property Username(ByVal userIndex As Integer) As String
        Get
            Username = _username(userIndex)
        End Get
    End Property



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function AddUser(ByVal username As String) As Boolean
        ' If the user count is lower than the maximum
        If (_userCount < _maxUserCount) Then
            ' Add a new user
            _username(_userCount) = username
            _password(_userCount) = _defaultPassword
            _accessLevel(_userCount) = 0
            _userCount = _userCount + 1
            ' Return False
            AddUser = False
        Else    ' Otherwise, if the user count is greater or equal to the maximum
            ' Return True
            AddUser = True
        End If
    End Function



    Public Function DeleteUser(ByVal userIndex As Integer) As Boolean
        ' If the user index is valid
        If (userIndex > 0 And userIndex < _userCount) Then
            ' Delete the user
            For i = userIndex To _userCount - 2
                _username(i) = _username(i + 1)
                _password(i) = _password(i + 1)
                _accessLevel(i) = _accessLevel(i + 1)
            Next
            _userCount = _userCount - 1
            ' Return False
            DeleteUser = False
        Else    ' Otherwise, if the user index is not valid
            ' Return True
            DeleteUser = True
        End If
    End Function



    Public Function ResetUser(ByVal userIndex As Integer) As Boolean
        ' If the user index is valid
        If (userIndex > 0 And userIndex < _userCount) Then
            ' Reset the user
            _password(userIndex) = _defaultPassword
            ' Return False
            ResetUser = False
        Else    ' Otherwise, if the user index is not valid
            ' Return True
            ResetUser = True
        End If
    End Function



    Public Function PowerDown() As Boolean
        ' Return False
        PowerDown = False
    End Function



    Public Function PowerUp(ByVal configurationPath As String) As Boolean
        ' Load the configuration
        PowerUp = LoadConfiguration(configurationPath)
    End Function



    Public Function LoadConfiguration(ByVal configurationPath As String) As Boolean
        Dim fileReader As StreamReader = Nothing
        Dim token() As String

        Try
            ' Add the administrator user
            _username(0) = _administratorUsername
            _password(0) = _administratorPassword
            _accessLevel(0) = _administratorAccessLevel
            _userCount = 1
            ' Read the configuration file
            fileReader = New StreamReader(configurationPath)
            While (Not (fileReader.EndOfStream) And _userCount < _maxUserCount)
                token = Split(Decryp_MD5(fileReader.ReadLine), vbTab)
                _username(_userCount) = token(0)
                _password(_userCount) = token(1)
                _accessLevel(_userCount) = CInt(token(2))
                _userCount = _userCount + 1
            End While
            ' Return False
            LoadConfiguration = False

        Catch ex As Exception
            ' Return True
            LoadConfiguration = True

        Finally
            ' Close the file
            If (fileReader IsNot Nothing) Then
                fileReader.Close()
                fileReader = Nothing
            End If
        End Try
    End Function

    Public Function Login() As Integer
        If True Then 'Set to True later.
            ' If the current username is empty
            If (_currentUsername = "") Then
                ' Configurate and show the form Login
                frmLogin.Username = ""
                frmLogin.Password = ""
                frmLogin.ShowDialog()
                ' If the user confirmed the operation
                If (frmLogin.Confirm) Then
                    ' If the username and the password are correct
                    _currentUserIndex = Array.IndexOf(_username, frmLogin.Username)
                    _password(0) = _administratorPassword & Hour(Date.Now) & Minute(Date.Now)
                    If (_currentUserIndex <> -1 AndAlso (frmLogin.Password = _password(_currentUserIndex))) Then
                        ' Login the user
                        _currentUsername = _username(_currentUserIndex)
                        _currentAccessLevel = _accessLevel(_currentUserIndex)
                        ' Show an information message
                        frmMessage.MessageType = frmMessage.eType.Information
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Login succeeded."
                    Else    ' Otherwise, if the username or the password are not correct
                        ' Show an error message
                        frmMessage.MessageType = frmMessage.eType.Critical
                        frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
                        frmMessage.Message = "Login failed."
                    End If
                    frmMessage.ShowDialog()
                End If
            End If
            ' Return the current access level
            Login = _currentAccessLevel
        Else
            Login = 4
        End If

    End Function



    Public Sub Logout()
        ' Logout the current user
        _currentUsername = ""
        _currentUserIndex = 0
        _currentAccessLevel = 0
    End Sub



    Public Function SaveConfiguration(ByVal configurationPath As String) As Boolean
        Dim fileWriter As StreamWriter = Nothing
        Dim index As Integer
        Try
            ' Save the configuration file
            fileWriter = New StreamWriter(configurationPath)
            For index = 1 To _userCount - 1
                fileWriter.WriteLine(Encrypt_MD5(_username(index) & vbTab & _password(index) & vbTab & _accessLevel(index)))
            Next
            ' Return False
            SaveConfiguration = False

        Catch ex As Exception
            ' Return True
            SaveConfiguration = True

        Finally
            ' Close the file
            If (fileWriter IsNot Nothing) Then
                fileWriter.Close()
                fileWriter = Nothing
            End If
        End Try
    End Function



    Public Sub SetPassword(ByVal password As String)
        ' If the user index is valid
        If (_currentUserIndex > 0) Then
            ' Update the user's password
            _password(_currentUserIndex) = password
        End If
    End Sub

    Private Function Encrypt_MD5(ByVal password As String) As String
        Dim wrapper As New Simple3Des("ADMIN")
        Dim cipherText As String = wrapper.EncryptData(password)

        Encrypt_MD5 = cipherText
    End Function

    Private Function Decryp_MD5(ByVal password As String) As String
        Dim wrapper As New Simple3Des("ADMIN")
        ' DecryptData throws if the wrong password is used.
        Try
            Dim plainText As String = wrapper.DecryptData(password)
            Decryp_MD5 = plainText
        Catch ex As System.Security.Cryptography.CryptographicException
            Decryp_MD5 = ""
            'MsgBox("The data could not be decrypted with the password.")
        End Try
    End Function


    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module