Module mColor_Conversion
    Public Sub RGB_XYZ(ByRef R As Integer, ByVal G As Integer, ByVal B As Integer, ByVal x As Single, ByVal y As Single, ByVal Z As Single)
        ' answer X=Y=Z=0 in case of error
        ' RGB = sRGB

        Dim Rel_R As Single, Rel_G As Single, Rel_B As Single

        ' Init value to error value
        x = 0
        y = 0
        Z = 0

        ' Check value [R, G, B] in [0-255]; if not, exit
        If (R < 0) Or (R > 255) Or (G < 0) Or (G > 255) Or (B < 0) Or (B > 255) Then Exit Sub

        ' Case R=G=B=0
        If R + G + B = 0 Then Exit Sub

        Rel_R = (R / 255)          '// R from 0 to 255
        Rel_G = (G / 255)          '// G from 0 to 255
        Rel_B = (B / 255)          '// B from 0 to 255

        If Rel_R > 0.04045 Then Rel_R = ((Rel_R + 0.055) / 1.055) ^ 2.4 _
                           Else  : Rel_R = Rel_R / 12.92
        If Rel_G > 0.04045 Then Rel_G = ((Rel_G + 0.055) / 1.055) ^ 2.4 _
                           Else  : Rel_G = Rel_G / 12.92
        If Rel_B > 0.04045 Then Rel_B = ((Rel_B + 0.055) / 1.055) ^ 2.4 _
                           Else  : Rel_B = Rel_B / 12.92

        x = Rel_R * 41.24 + Rel_G * 35.76 + Rel_B * 18.05
        y = Rel_R * 21.26 + Rel_G * 71.52 + Rel_B * 7.22
        Z = Rel_R * 1.93 + Rel_G * 11.92 + Rel_B * 95.05

    End Sub

    Public Sub RGB_xyYl(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer, ByRef x As Single, ByRef y As Single, ByRef Yl As Single)
        ' answer x=y=Yl=0 in case of error
        ' RGB = sRGB

        Dim Rel_R As Single, Rel_G As Single, Rel_B As Single
        Dim X_ As Single, Y_ As Single, Z_ As Single 'X, Y, Z in CIE ; Yl = Y in x,y,Y

        ' Init value to error value
        x = 0
        y = 0
        Yl = 0

        ' Check value [R, G, B] in [0-255]; if not, exit
        If (R < 0) Or (R > 255) Or (G < 0) Or (G > 255) Or (B < 0) Or (B > 255) Then Exit Sub

        ' Case R=G=B=0
        If R + G + B = 0 Then Exit Sub

        'Case NOT(R=G=B=0)

        Rel_R = (R / 255)          '// R from 0 to 255
        Rel_G = (G / 255)          '// G from 0 to 255
        Rel_B = (B / 255)          '// B from 0 to 255

        If Rel_R > 0.04045 Then Rel_R = ((Rel_R + 0.055) / 1.055) ^ 2.4 _
                           Else Rel_R = Rel_R / 12.92
        If Rel_G > 0.04045 Then Rel_G = ((Rel_G + 0.055) / 1.055) ^ 2.4 _
                           Else Rel_G = Rel_G / 12.92
        If Rel_B > 0.04045 Then Rel_B = ((Rel_B + 0.055) / 1.055) ^ 2.4 _
                           Else Rel_B = Rel_B / 12.92

        X_ = Rel_R * 41.24 + Rel_G * 35.76 + Rel_B * 18.05
        Y_ = Rel_R * 21.26 + Rel_G * 71.52 + Rel_B * 7.22
        Z_ = Rel_R * 1.93 + Rel_G * 11.92 + Rel_B * 95.05

        Yl = Y_
        x = X_ / (X_ + Y_ + Z_)
        y = Y_ / (X_ + Y_ + Z_)

    End Sub

    Public Function RGB_H(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single
        ' Hue in HSV referential from RGB, valeu in [0-360°] range

        Dim Rel_R As Single, Rel_G As Single, Rel_B As Single
        Dim Col_Min As Single, Col_Max As Single, Del_Max As Single


        Rel_R = (R / 255)
        Rel_G = (G / 255)
        Rel_B = (B / 255)

        Col_Min = 0 ' WorksheetFunction.Min(Rel_R, Rel_G, Rel_B)
        Col_Max = 0 ' WorksheetFunction.Max(Rel_R, Rel_G, Rel_B)

        Del_Max = Col_Max - Col_Min

        If Del_Max = 0 Then RGB_H = -1 : Exit Function ' not defined
        If Col_Max = Rel_R Then
            RGB_H = 60 * (((Rel_G - Rel_B) / Del_Max))
        ElseIf Col_Max = Rel_G Then
            RGB_H = 60 * (((Rel_B - Rel_R) / Del_Max) + 2)
        ElseIf Col_Max = Rel_B Then
            RGB_H = 60 * (((Rel_R - Rel_G) / Del_Max) + 4)
        End If

        If RGB_H < 0 Then RGB_H = RGB_H + 360
        If RGB_H > 360 Then RGB_H = RGB_H - 360

    End Function


    Public Function RGB_S(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single
        ' Saturation in HSV referential from RGB

        Dim Rel_R As Single, Rel_G As Single, Rel_B As Single
        Dim Col_Min As Single, Col_Max As Single, Del_Max As Single
        Dim Value_MinMax(0 To 3) As Single
        Const MinValue As Boolean = False
        Const MaxValue As Boolean = True


        Rel_R = (R / 255)
        Rel_G = (G / 255)
        Rel_B = (B / 255)

        Value_MinMax(0) = Rel_R
        Value_MinMax(1) = Rel_G
        Value_MinMax(2) = Rel_B

        Col_Min = MinMax_Value(Value_MinMax, MinValue, 3)
        Col_Max = MinMax_Value(Value_MinMax, MaxValue, 3)

        Del_Max = Col_Max - Col_Min

        If Del_Max = 0 Then RGB_S = 0 _
                        Else RGB_S = Del_Max / Col_Max * 100

    End Function

    Public Function RGB_hsV(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single
        ' Value in HSV referential from RGB, V in [0-1] range

        Dim Rel_R As Single, Rel_G As Single, Rel_B As Single
        Dim Col_Min As Single, Col_Max As Single, Del_Max As Single


        Rel_R = (R / 255)
        Rel_G = (G / 255)
        Rel_B = (B / 255)

        Col_Min = 0 ' WorksheetFunction.Min(Rel_R, Rel_G, Rel_B)
        Col_Max = 0 ' WorksheetFunction.Max(Rel_R, Rel_G, Rel_B)

        RGB_hsV = Col_Max

    End Function


    Public Sub XYZ_Luv(ByRef x As Single, ByVal y As Single, ByVal Z As Single, ByVal L As Single, ByVal u As Single, ByVal v As Single)

        Dim Xr As Single, Yr As Single, Zr As Single ' White ref
        Dim epsilon As Single, K As Single
        ' Dim U_P As Single, V_P As Single ' u ' v'

        ' CIE-D65 : X=95.047, Y=100.00, Z=108.883
        Xr = 95.047
        Yr = 100.0#
        Zr = 108.883

        epsilon = 216 / 24389   ' new definition for function continuity
        K = 24389 / 27          ' new definition for function continuity

        If y = 0 Then L = 0 : u = 0 : v = 0 : Exit Sub

        If y / Yr > epsilon Then L = 116 * ((y / Yr) ^ (1 / 3)) - 16 _
                            Else L = K * y / Yr

        u = (4 * x) / (x + 15 * y + 3 * Z)
        v = (9 * y) / (x + 15 * y + 3 * Z)

        'u = 13 * L * ((4 * X) / (X + 15 * Y + 3 * Z) - (4 * Xr) / (Xr + 15 * Yr + 3 * Zr))
        'v = 13 * L * ((9 * Y) / (X + 15 * Y + 3 * Z) - (9 * Yr) / (Xr + 15 * Yr + 3 * Zr))


    End Sub

    Public Sub XYZ_Lab(ByRef x As Single, ByVal y As Single, ByVal Z As Single, ByVal L As Single, ByVal a As Single, ByVal B As Single)

        Dim Xr As Single, Yr As Single, Zr As Single ' White ref
        Dim epsilon As Single, K As Single
        Dim Y_P As Single

        ' CIE-D65 : X=95.047, Y=100.00, Z=108.883
        Xr = 95.047
        Yr = 100.0#
        Zr = 108.883

        epsilon = 216 / 24389   ' new definition for function continuity 0,0088564516...
        K = 24389 / 27          ' new definition for function continuity 903,2962...

        If y / Yr > epsilon Then L = 116 * ((y / Yr) ^ (1 / 3)) - 16 _
                            Else L = K * y / Yr

        ' f(t) = T^1/3 if t>0.008856
        ' f(t) = 7.787 * t + 16/116 if t<=0.008856

        ' Y_P = f(Y/Yr)

        ' a = 500 * (f(X/Xr) - f(Y/Yr) = 500 * (f(X/Xr) - Y_P)
        ' b = 200 * (f(Y/Yr) - f(Z/Zr) = 200 * (Y_P - f(Z/Zr))

        If y / Yr > epsilon Then Y_P = (y / Yr) ^ (1 / 3) _
                            Else Y_P = 7.787 * y / Yr + 16 / 116

        If x / Xr > epsilon Then a = (x / Xr) ^ (1 / 3) _
                            Else a = 7.787 * x / Xr + 16 / 116
        a = 500 * (a - Y_P)

        If Z / Zr > epsilon Then B = (Z / Zr) ^ (1 / 3) _
                            Else B = 7.787 * Z / Zr + 16 / 116
        B = 200 * (Y_P - B)


    End Sub


    Public Function RGB_x(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single

        Dim x As Single, y As Single, Yl As Single

        Call RGB_xyYl(R, G, B, x, y, Yl)
        RGB_x = x

    End Function


    Public Function RGB_y(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single

        Dim x As Single, y As Single, Yl As Single

        Call RGB_xyYl(R, G, B, x, y, Yl)
        RGB_y = y

    End Function

    Public Function RGB_Yl(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single

        Dim x As Single, y As Single, Yl As Single

        Call RGB_xyYl(R, G, B, x, y, Yl)
        RGB_Yl = Yl

    End Function

    Public Function RGB_L(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single

        Dim x As Single, y As Single, Z As Single
        Dim L As Single, u As Single, v As Single

        Call RGB_XYZ(R, G, B, x, y, Z)
        Call XYZ_Luv(x, y, Z, L, u, v)
        RGB_L = L

    End Function

    Public Function RGB_u(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single

        Dim x As Single, y As Single, Z As Single
        Dim L As Single, u As Single, v As Single

        Call RGB_XYZ(R, G, B, x, y, Z)
        Call XYZ_Luv(x, y, Z, L, u, v)
        RGB_u = u

    End Function

    Public Function RGB_v(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single

        Dim x As Single, y As Single, Z As Single
        Dim L As Single, u As Single, v As Single

        Call RGB_XYZ(R, G, B, x, y, Z)
        Call XYZ_Luv(x, y, Z, L, u, v)
        RGB_v = v

    End Function

    Public Function RGB_a(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single

        Dim x As Single, y As Single, Z As Single
        Dim L As Single, a_ As Single, b_ As Single

        Call RGB_XYZ(R, G, B, x, y, Z)
        Call XYZ_Lab(x, y, Z, L, a_, b_)
        RGB_a = a_

    End Function


    Public Function RGB_b(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Single

        Dim x As Single, y As Single, Z As Single
        Dim L As Single, a_ As Single, b_ As Single

        Call RGB_XYZ(R, G, B, x, y, Z)
        Call XYZ_Lab(x, y, Z, L, a_, b_)
        RGB_b = b_

    End Function


    Public Sub xyYl_RGB(ByRef x As Single, ByVal y As Single, ByVal Yl As Single, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer)

        Dim var_X As Single, var_Y As Single, var_Z As Single
        Dim var_R As Single, var_G As Single, var_B As Single

        '//Yl from 0 to 100
        '//x from 0 to 1
        '//y from 0 to 1

        var_X = x * (Yl / y)
        var_Y = Yl
        var_Z = (1 - x - y) * (Yl / y)


        var_X = var_X / 100        '//X_ from 0 to  95.047      (Observer = 2°, Illuminant = D65)
        var_Y = var_Y / 100        '//Y_ from 0 to 100.000
        var_Z = var_Z / 100        '//Z_ from 0 to 108.883

        var_R = var_X * 3.2406 + var_Y * -1.5372 + var_Z * -0.4986
        var_G = var_X * -0.9689 + var_Y * 1.8758 + var_Z * 0.0415
        var_B = var_X * 0.0557 + var_Y * -0.204 + var_Z * 1.057

        If (var_R > 0.0031308) Then var_R = 1.055 * (var_R ^ (1 / 2.4)) - 0.055 _
        Else var_R = 12.92 * var_R
        If (var_G > 0.0031308) Then var_G = 1.055 * (var_G ^ (1 / 2.4)) - 0.055 _
        Else var_G = 12.92 * var_G
        If (var_B > 0.0031308) Then var_B = 1.055 * (var_B ^ (1 / 2.4)) - 0.055 _
        Else var_B = 12.92 * var_B

        R = var_R * 255
        G = var_G * 255
        B = var_B * 255

    End Sub

    Public Function xyYl_R(ByVal x As Single, ByVal y As Single, ByVal Yl As Single) As Integer

        Dim R As Integer, G As Integer, B As Integer

        Call xyYl_RGB(x, y, Yl, R, G, B)
        xyYl_R = R

    End Function

    Public Function xyYl_G(ByVal x As Single, ByVal y As Single, ByVal Yl As Single) As Integer

        Dim R As Integer, G As Integer, B As Integer

        Call xyYl_RGB(x, y, Yl, R, G, B)
        xyYl_G = G

    End Function

    Public Function xyYl_B(ByVal x As Single, ByVal y As Single, ByVal Yl As Single) As Integer

        Dim R As Integer, G As Integer, B As Integer

        Call xyYl_RGB(x, y, Yl, R, G, B)
        xyYl_B = B

    End Function


    Public Function L_x(ByVal L As Integer) As Single
        ' Convert lambda nm to x

        'L_x = WorksheetFunction.VLookup(L, Range("Table_L_xyYl"), 2, False)

    End Function

    Public Function L_y(ByVal L As Integer) As Single
        ' Convert lambda nm to y

        'L_y = WorksheetFunction.VLookup(L, Range("Table_L_xyYl"), 3, False)

    End Function

    Public Function L_Yl(ByVal L As Integer) As Single
        ' Convert lambda nm to Yl

        'L_Yl = WorksheetFunction.VLookup(L, Range("Table_L_xyYl"), 4, False) * 100

    End Function


    Private Function MinMax_Value(ByRef Value() As Single, ByVal MinMax As Boolean, ByVal Number_Data As Integer) As Single
        Dim i As Integer

        MinMax_Value = Value(0)
        If Number_Data > 1 Then
            For i = 1 To Number_Data - 1
                If MinMax = False Then
                    If (Value(i) < MinMax_Value) Then
                        MinMax_Value = Value(i)
                    End If
                Else
                    If (Value(i) > MinMax_Value) Then
                        MinMax_Value = Value(i)
                    End If
                End If
            Next i
        End If
    End Function

End Module
