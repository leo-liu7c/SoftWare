Option Explicit On
Option Strict On

Imports System
Imports System.IO

Module mColor_Analysis

    Private Cie_Data(0 To 15, 0 To 500) As Single

    '    Public Function Dominant_WavelengthInt(ByVal x As Single, ByVal y As Single) As Integer
    '        ' replace DominatWavelength if integer value is really necessay (a bit faster that in single)
    '        ' prefer to use DominantWavelength

    '        Const D65_X As Single = 0.31271
    '        Const D65_Y As Single = 0.32902
    '        ' Const Limit_wavelength As Integer = 518 ' limit for which distance
    '        Const Pi As Single = 3.14159265358979

    '        'Dim TableLxy()
    '        'Dim L_min As Integer, L_max As Integer
    '        'Dim L_nb As Integer
    '        'Dim i As Integer
    '        'Dim colorside As Single

    '        Dim angle As Single

    '        If (x = 0 And y = 0) Or (x = D65_X And y = D65_Y) Then Dominant_WavelengthInt = 0 : Exit Function

    '        On Error GoTo ErrorHandle
    '        angle = -(Math.Atan((y - D65_Y) / (x - D65_X)) * 180 / Pi - ((x - D65_X) < 0) * 180)

    '        'Dominant_WavelengthInt = WorksheetFunction.VLookup(angle, Range("Table_Ang2Lambda"), 2, True)

    '        Exit Function

    'ErrorHandle:
    '        Dominant_WavelengthInt = CInt(Err.Number)

    '    End Function


    Public Function Dominant_Wavelength(ByVal x As Single, ByVal y As Single) As Single
        ' function updated in v03 to return 'single' and not more 'integer'; not an issue within Excel that will adapt the type if needed for previous file.

        Const D65_X As Single = 0.31271
        Const D65_Y As Single = 0.32902
        ' Const Limit_wavelength As Integer = 518 ' limit for which distance
        Const Pi As Single = 3.14159265358979

        Dim angle As Single
        Dim Index As Double
        Dim Tmp As Double
        ' Case black or color=white D65
        If (x = 0 And y = 0) Or (x = D65_X And y = D65_Y) Then Dominant_Wavelength = 0 : Exit Function

        'On Error GoTo ErrorHandle
        'angle = -(Math.Atan((y - D65_Y) / (x - D65_X)) * 180 / Pi - ((x - D65_X) < 0) * 180)

        On Error GoTo ErrorHandle
        Tmp = -(Math.Atan((y - D65_Y) / (x - D65_X)) * 180 / Pi) ' - ((x - D65_X) < 0) * 180)
        If x - D65_X < 0 Then
            angle = CSng(Tmp - 180)
        Else
            angle = CSng(Tmp)
        End If

        'Dominant_Wavelength = WorksheetFunction.VLookup(angle, Range("Table_Ang2Lambda2"), 2, True) + _
        '                     WorksheetFunction.VLookup(angle, Range("Table_Ang2Lambda2"), 5, True) * _
        '                     (angle - WorksheetFunction.VLookup(angle, Range("Table_Ang2Lambda2"), 1, True))

        'Find in the Table
        Dominant_Wavelength = 0
        For i = 0 To 300
            If angle <= Cie_Data(8, i) Then
                If i >= 1 Then
                    Dominant_Wavelength = Cie_Data(9, i - 1) + (Cie_Data(12, i - 1) * (angle - Cie_Data(8, i - 1)))
                Else
                    Dominant_Wavelength = Cie_Data(9, i) + (Cie_Data(12, i) * (angle - Cie_Data(8, i)))
                End If
                Exit For
            End If
        Next

        Exit Function

ErrorHandle:
        Dominant_Wavelength = CInt(Err.Number)

    End Function


    Public Function Dominant_Wavelength2(ByVal x As Single, ByVal y As Single) As Single
        ' function kept for compatibility with existing file

        Dominant_Wavelength2 = Dominant_Wavelength(x, y)

    End Function


    Public Function insideABCD(ByVal x As Single, ByVal y As Single, ByVal XA As Single, ByVal YA As Single, ByVal XB As Single, ByVal YB As Single, ByVal XC As Single, ByVal YC As Single, ByVal XD As Single, ByVal YD As Single) As Boolean
        ' boolean if point (x,y) is inside ABCD
        ' Prefer DistanceABCD that return also the distance to ABCD (>0 if inside, <0 if outside)

        Dim P1 As Single
        Dim P2 As Single
        Dim P3 As Single
        Dim P4 As Single


        P1 = ((YB - YA) * x - (XB - XA) * y - XA * YB + XB * YA) * ((YB - YA) * XC - (XB - XA) * YC - XA * YB + XB * YA)
        P2 = ((YC - YB) * x - (XC - XB) * y - XB * YC + XC * YB) * ((YC - YB) * XD - (XC - XB) * YD - XB * YC + XC * YB)
        P3 = ((YD - YC) * x - (XD - XC) * y - XC * YD + XD * YC) * ((YD - YC) * XA - (XD - XC) * YA - XC * YD + XD * YC)
        P4 = ((YA - YD) * x - (XA - XD) * y - XD * YA + XA * YD) * ((YA - YD) * XB - (XA - XD) * YB - XD * YA + XA * YD)

        insideABCD = Not ((P1 < 0) Or (P2 < 0) Or (P3 < 0) Or (P4 < 0))

    End Function
    '''<summary> 
    '''判断点是否在多边形内. 
    '''----------原理---------- 
    '''注意到如果从P作水平向左的射线的话，如果P在多边形内部，那么这条射线与多边形的交点必为奇数， 
    '''如果P在多边形外部，则交点个数必为偶数(0也在内)。 
    '''YAN.Qian Created at 20200529
    '''PointList Must follow ITS, start from A till last one. A,B,C,D,E,F...
    '''Test quadrilateral and Polygon
    '''</summary> 
    '''<returns></returns> 
    Public Function InsidePolygon(ByVal x As Single, ByVal y As Single, ByVal XPointList() As Single, ByVal YPointList() As Single) As Boolean
        InsidePolygon = False
        Dim pointCount As Integer = XPointList.Length
        Dim point1x As Single
        Dim point1y As Single
        Dim point2x As Single
        Dim point2y As Single
        Dim i As Integer = 0, j As Integer = pointCount - 1

        While i < pointCount

            point1x = XPointList(i)
            point1y = YPointList(i)
            point2x = XPointList(j)
            point2y = YPointList(j)

            If y < point2y Then
                If point1y <= y Then
                    If (y - point1y) * (point2x - point1x) > (x - point1x) * (point2y - point1y) Then
                        InsidePolygon = (Not InsidePolygon)
                    End If
                End If
            ElseIf y < point1y Then
                If (y - point1y) * (point2x - point1x) < (x - point1x) * (point2y - point1y) Then
                    InsidePolygon = (Not InsidePolygon)
                End If
            End If

            j = i
            i += 1
        End While

        Return InsidePolygon

    End Function

    Public Function Dist_Point_to_Line(ByVal x As Single, ByVal y As Single, ByVal X0 As Single, ByVal Y0 As Single, ByVal X1 As Single, ByVal Y1 As Single) As Single
        ' calculate the signed distance of a point to a line
        ' the sign give the side to the line
        '    positif : left from vector (P0,P1)

        On Error GoTo ErrorHandle
        Dist_Point_to_Line = (Y0 - Y1) * x + (X1 - X0) * y + (X0 * Y1 - X1 * Y0)
        Dist_Point_to_Line = CSng(Dist_Point_to_Line / ((X1 - X0) ^ 2 + (Y1 - Y0) ^ 2) ^ 0.5)

        Exit Function

ErrorHandle:
        Dist_Point_to_Line = CInt(Err.Number)

    End Function


    Public Function Dist_Point_to_Segment(ByVal x As Single, ByVal y As Single, ByVal X0 As Single, ByVal Y0 As Single, ByVal X1 As Single, ByVal Y1 As Single) As Single
        ' calculate the signed distance of a point to segment [AB]
        ' the sign give the side of the point to (AB)
        ' positive : left from vector (A,B)

        Dim PA As Single
        Dim PB As Single
        Dim Signe As Single


        On Error GoTo ErrorHandle

        Dist_Point_to_Segment = Dist_Point_to_Line(x, y, X0, Y0, X1, Y1)
        If Dist_Point_to_Segment < 0 Then Signe = -1 Else Signe = 1

        If (x - X0) * (X1 - X0) + (y - Y0) * (Y1 - Y0) < 0 Then Dist_Point_to_Segment = CSng(Signe * ((x - X0) ^ 2 + (y - Y0) ^ 2) ^ 0.5)
        If (x - X1) * (X0 - X0) + (y - Y1) * (Y1 - Y0) > 0 Then Dist_Point_to_Segment = CSng(Signe * ((x - X1) ^ 2 + (y - Y1) ^ 2) ^ 0.5)

        Exit Function

ErrorHandle:
        Dist_Point_to_Segment = CInt(Err.Number)

    End Function


    Public Function DistanceABCD(ByVal x As Single, ByVal y As Single, ByVal ParamArray Other() As Single) As Single
        ' expect 10 parameters : x;y;XA;YA;XB;YB;XC;YC;XD;YD)
        ' answer -1 in case of error; -1 choosen as in CIE1931 RGB distance always less than 1
        ' param array for ABCD coordinates given in a range or in 8 values

        Dim XA As Single
        Dim YA As Single
        Dim XB As Single
        Dim YB As Single
        Dim XC As Single
        Dim YC As Single
        Dim XD As Single
        Dim YD As Single

        Dim D_PAB As Single     'distance of point P( x,y) from line (AB)
        Dim D_PBC As Single     '                                    (BC)
        Dim D_PCD As Single     '                                    (CD)
        Dim D_PDA As Single     '                                    (DA)

        Dim Value_MinMax(0 To 3) As Single

        Dim Signe As Single     ' signe of distance inside the quadrilater

        Const MinValue As Boolean = False
        Const MaxValue As Boolean = True

        ' extraction XA...YD values from ParamArray Other() argument

        XA = Other(0) : YA = Other(1)
        XB = Other(1) : YB = Other(3)
        XC = Other(2) : YC = Other(5)
        XD = Other(3) : YD = Other(7)
        ' Calculate the distance to quadrilater

        D_PAB = Dist_Point_to_Segment(x, y, XA, YA, XB, YB)
        D_PBC = Dist_Point_to_Segment(x, y, XB, YB, XC, YC)
        D_PCD = Dist_Point_to_Segment(x, y, XC, YC, XD, YD)
        D_PDA = Dist_Point_to_Segment(x, y, XD, YD, XA, YA)

        Signe = Dist_Point_to_Segment(XC, YC, XA, YA, XB, YB)  ' signe of inside from C compare to [AB]

        ' calculate if inside ABCD (distanceABCD>0) and the distance; outside if DistanceABCD<0;
        ' if outside, the distance may be wrong at this step
        DistanceABCD = 0
        'If (Signe > 0) Then DistanceABCD = WorksheetFunction.Min(D_PAB, D_PBC, D_PCD, D_PDA)
        If Signe > 0 Then
            Value_MinMax(0) = D_PAB
            Value_MinMax(1) = D_PBC
            Value_MinMax(2) = D_PCD
            Value_MinMax(3) = D_PDA
            DistanceABCD = MinMax_Value(Value_MinMax, MinValue, 4)
        End If

        'If (Signe < 0) Then DistanceABCD = -WorksheetFunction.Max(D_PAB, D_PBC, D_PCD, D_PDA)
        If (Signe < 0) Then
            Value_MinMax(0) = D_PAB
            Value_MinMax(1) = D_PBC
            Value_MinMax(2) = D_PCD
            Value_MinMax(3) = D_PDA
            DistanceABCD = -MinMax_Value(Value_MinMax, MaxValue, 4)
        End If

        'recalculate distance if outside ABCD
        If DistanceABCD < 0 And Signe > 0 Then ' distance = min of strictly positive distance
            ' A corriger
            'DistanceABCD = WorksheetFunction.Min(D_PAB, D_PBC, D_PCD, D_PDA)
            Value_MinMax(0) = D_PAB
            Value_MinMax(1) = D_PBC
            Value_MinMax(2) = D_PCD
            Value_MinMax(3) = D_PDA
            DistanceABCD = MinMax_Value(Value_MinMax, MinValue, 4)
            If D_PAB < 0 Then
                'DistanceABCD = WorksheetFunction.Max(DistanceABCD, D_PAB)
                Value_MinMax(0) = DistanceABCD
                Value_MinMax(1) = D_PAB
                DistanceABCD = MinMax_Value(Value_MinMax, MaxValue, 2)
            End If
            If D_PBC < 0 Then
                ' DistanceABCD = WorksheetFunction.Max(DistanceABCD, D_PBC)
                Value_MinMax(0) = DistanceABCD
                Value_MinMax(1) = D_PBC
                DistanceABCD = MinMax_Value(Value_MinMax, MaxValue, 2)
            End If
            'DistanceABCD = WorksheetFunction.Max(DistanceABCD, D_PCD)
            If D_PCD < 0 Then
                'DistanceABCD = WorksheetFunction.Max(DistanceABCD, D_PCD)
                Value_MinMax(0) = DistanceABCD
                Value_MinMax(1) = D_PCD
                DistanceABCD = MinMax_Value(Value_MinMax, MaxValue, 2)
            End If
            If D_PDA < 0 Then
                'DistanceABCD = WorksheetFunction.Max(DistanceABCD, D_PDA)
                Value_MinMax(0) = DistanceABCD
                Value_MinMax(1) = D_PDA
                DistanceABCD = MinMax_Value(Value_MinMax, MaxValue, 2)
            End If
        End If

        If DistanceABCD < 0 And Signe < 0 Then ' distance = min of strictly positive distance
            'strictly negative (<0) distance
            ' A corriger
            'DistanceABCD = WorksheetFunction.Max(D_PAB, D_PBC, D_PCD, D_PDA)
            Value_MinMax(0) = D_PAB
            Value_MinMax(1) = D_PBC
            Value_MinMax(2) = D_PCD
            Value_MinMax(3) = D_PDA
            DistanceABCD = MinMax_Value(Value_MinMax, MaxValue, 4)
            If D_PAB > 0 Then
                'DistanceABCD = WorksheetFunction.Min(DistanceABCD, D_PAB)
                Value_MinMax(0) = DistanceABCD
                Value_MinMax(1) = D_PAB
                DistanceABCD = MinMax_Value(Value_MinMax, MinValue, 2)
            End If
            If D_PBC > 0 Then
                ' DistanceABCD = WorksheetFunction.Min(DistanceABCD, D_PBC)
                Value_MinMax(0) = DistanceABCD
                Value_MinMax(1) = D_PBC
                DistanceABCD = MinMax_Value(Value_MinMax, MinValue, 2)
            End If
            If D_PCD > 0 Then
                'DistanceABCD = WorksheetFunction.Min(DistanceABCD, D_PCD)
                Value_MinMax(0) = DistanceABCD
                Value_MinMax(1) = D_PCD
                DistanceABCD = MinMax_Value(Value_MinMax, MinValue, 2)
            End If
            If D_PDA > 0 Then
                'DistanceABCD = WorksheetFunction.Min(DistanceABCD, D_PDA)
                Value_MinMax(0) = DistanceABCD
                Value_MinMax(1) = D_PDA
                DistanceABCD = MinMax_Value(Value_MinMax, MinValue, 2)
            End If
            DistanceABCD = -DistanceABCD
        End If

        Exit Function

ErrorHandle:
        DistanceABCD = -1

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

    Public Function dot(ByVal XP0 As Single, ByVal YP0 As Single, ByVal XP1 As Single, ByVal YP1 As Single, ByVal XQ0 As Single, ByVal YQ0 As Single, ByVal XQ1 As Single, ByVal YQ1 As Single) As Single
        ' dot product of vector P0P1 and vector Q0Q1
        ' Called by Segment_Intersection

        dot = (XP1 - XP0) * (XQ1 - XQ0) + (YP1 - YP0) * (YQ1 - YQ0)

    End Function


    Public Function perp(ByVal XP0 As Single, ByVal YP0 As Single, ByVal XP1 As Single, ByVal YP1 As Single, ByVal XQ0 As Single, ByVal YQ0 As Single, ByVal XQ1 As Single, ByVal YQ1 As Single) As Single
        ' perp product of vector P0P1 and vector Q0Q1
        ' Called by Segment_Intersection

        perp = (XP1 - XP0) * (YQ1 - YQ0) - (YP1 - YP0) * (XQ1 - XQ0)

    End Function


    Public Function InSegment(ByVal XP As Single, ByVal YP As Single, _
                       ByVal XA As Single, ByVal YA As Single, ByVal XB As Single, ByVal YB As Single) _
                       As Boolean
        ' Called by Segment_Intersection
        ' determine if a point P is inside a segment [AB]
        '
        '
        '/ inSegment(): determine if a point is inside a segment
        '//    Input:  a point P, and a collinear segment S
        '//    Return: 1 = P is inside S
        '//            0 = P is  not inside S
        'int
        'inSegment( Point P, Segment S)
        '{
        '    if (S.P0.x != S.P1.x) {    // S is not  vertical
        If XA <> XB Then
            '        if (S.P0.x <= P.x && P.x <= S.P1.x)
            If (XA <= XP And XP <= XB) Then
                '            return 1;
                InSegment = True : Exit Function
                '        if (S.P0.x >= P.x && P.x >= S.P1.x)
                '            return 1;
            ElseIf XA >= XP And XP >= XB Then
                InSegment = True
                Exit Function
                '    }
            End If
            '    else {    // S is vertical, so test y  coordinate
        Else
            '        if (S.P0.y <= P.y && P.y <= S.P1.y)
            '            return 1;
            If YA <= YP And YP <= YB Then InSegment = True : Exit Function
            '        if (S.P0.y >= P.y && P.y >= S.P1.y)
            '            return 1;
            If YA >= YP And YP >= YB Then InSegment = True : Exit Function
            '    }
            '    return 0;
            InSegment = False : Exit Function
        End If
    End Function


    Public Sub Segment_Intersection(ByVal XM As Single, ByVal YM As Single, ByVal XP As Single, ByVal YP As Single, _
                             ByVal XA As Single, ByVal YA As Single, ByVal XB As Single, ByVal YB As Single, _
                            ByRef XI0 As Single, ByRef YI0 As Single, ByRef XI1 As Single, ByRef YI1 As Single, ByRef result As Integer)

        ' Calculate the intersect point (XI0, YI0) of [MP) (ray) and [AB].
        ' Result is 0=disjoint (no intersect), 1=intersect  in unique point I, 2=overlap  in segment from I0 to I1
        ' source: http://geomalgorithms.com/a05-_intersect-1.html#intersect2D_2Segments()
        ' Called by RelativeSaturation

        '#define SMALL_NUM   0.00000001 // anything that avoids division overflow

        Const SMALL_NUM As Single = 0.00000001

        Dim D As Single, du As Single, dv As Single

        '// dot product (3D) which allows vector operations in arguments
        '#define dot(u,v)   ((u).x * (v).x + (u).y * (v).y + (u).z * (v).z)
        '#define perp(u,v)  ((u).x * (v).y - (u).y * (v).x)  // perp product  (2D)
        '
        '
        '
        '// intersect2D_2Segments(): find the 2D intersection of 2 finite segments
        '//    Input:  two finite segments S1 and S2
        '//    Output: *I0 = intersect point (when it exists)
        '//            *I1 =  endpoint of intersect segment [I0,I1] (when it exists)
        '//    Return: 0=disjoint (no intersect)
        '//            1=intersect  in unique point I0
        '//            2=overlap  in segment from I0 to I1
        'int
        'intersect2D_2Segments( Segment S1, Segment S2, Point* I0, Point* I1 )
        ' BLE S1=MP; S2=AB
        '{
        '    Vector    u = S1.P1 - S1.P0; = vector MP
        '    Vector    v = S2.P1 - S2.P0;  = vector AB
        '    Vector    w = S1.P0 - S2.P0; = vector MA
        '    float     D = perp(u,v);

        D = perp(XM, YM, XP, YP, XA, YA, XB, YB)

        '
        '    // test if  they are parallel (includes either being a point)
        '    if (fabs(D) < SMALL_NUM) {           // S1 and S2 are parallel
        '        if (perp(u,w) != 0 || perp(v,w) != 0)  {
        '            return 0;                    // they are NOT collinear
        '        }
        If Math.Abs(D) < SMALL_NUM Then
            If (perp(XM, YM, XP, YP, XA, YA, XM, YM) <> 0) Or _
               (perp(XA, YA, XB, YB, XA, YA, XM, YM) <> 0) Then result = 0 : Exit Sub
            '        // they are collinear or degenerate
            '        // check if they are degenerate  points
            '        float du = dot(u,u);
            du = dot(XM, YM, XP, YP, XM, YM, XP, YP)
            '        float dv = dot(v,v);
            dv = dot(XA, YA, XB, YB, XA, YA, XB, YB)
            '        if (du==0 && dv==0) {            // both segments are points
            '            if (S1.P0 !=  S2.P0)         // they are distinct  points
            '                 return 0;
            '            *I0 = S1.P0;                 // they are the same point
            '            return 1;
            '        }
            If (du = 0 And dv = 0) Then
                If (XM <> XA Or YM <> YA) Then result = 0 : Exit Sub
                XI0 = XM
                YI0 = YM
                result = 1
                Exit Sub
            End If
            '        if (du==0) {                     // S1 is a single point
            '            if  (inSegment(S1.P0, S2) == 0)  // but is not in S2
            '                 return 0;
            '            *I0 = S1.P0;
            '            return 1;
            '        }
            If du = 0 Then
                If InSegment(XM, YM, XA, YA, XB, YB) = False Then result = 0 : Exit Sub
                XI0 = XM : YI0 = YM
                result = 1 : Exit Sub
            End If
            '        if (dv==0) {                     // S2 a single point
            '            if  (inSegment(S2.P0, S1) == 0)  // but is not in S1
            '                 return 0;
            '            *I0 = S2.P0;
            '            return 1;
            '        }
            If dv = 0 Then
                If InSegment(XA, YA, XM, YM, XP, YP) = False Then result = 0 : Exit Sub
                XI0 = XA : YI0 = YA
                result = 1 : Exit Sub
            End If
            '        // they are collinear segments - get  overlap (or not)
            '        float t0, t1;                    // endpoints of S1 in eqn for S2
            Dim T0 As Single, T1 As Single, T As Single
            '        Vector w2 = S1.P1 - S2.P0; ' w2=AP
            '        if (v.x != 0) {
            '                 t0 = w.x / v.x;
            '                 t1 = w2.x / v.x;
            '        }
            '        else {
            '                 t0 = w.y / v.y;
            '                 t1 = w2.y / v.y;
            '        }
            If (XB - XA) <> 0 Then
                T0 = (XM - XA) / (XB - XA)
                T1 = (XP - XA) / (XB - XA)
            Else
                T0 = (YM - YA) / (YB - YA)
                T1 = (XP - XA) / (YB - YA)
            End If
            '        if (t0 > t1) {                   // must have t0 smaller than t1
            '                 float t=t0; t0=t1; t1=t;    // swap if not
            '        }
            If (T0 > T1) Then T = T0 : T0 = T1 : T1 = T
            '        if (t0 > 1 || t1 < 0) {
            '            return 0;      // NO overlap
            '        }
            If (T0 > 1) And (T1 < 0) Then result = 0 : Exit Sub
            '        t0 = t0<0? 0 : t0;               // clip to min 0
            If T0 < 0 Then T0 = 0
            '        t1 = t1>1? 1 : t1;               // clip to max 1
            If T1 > 1 Then T1 = 1
            '        if (t0 == t1) {                  // intersect is a point
            '            *I0 = S2.P0 +  t0 * v;
            '            return 1;
            '        }
            If (T0 = T1) Then
                XI0 = XA + T0 * (XB - XA)
                YI0 = YA + T0 * (YB - YA)
                result = 1
                Exit Sub
            End If
            '
            '        // they overlap in a valid subsegment
            '        *I0 = S2.P0 + t0 * v;
            XI0 = XA + T0 * (XB - XA)
            YI0 = YA + T0 * (YB - YA)
            '        *I1 = S2.P0 + t1 * v;
            XI1 = XA + T1 * (XB - XA)
            YI1 = YA + T1 * (YB - YA)
            '        return 2;
            result = 2
            Exit Sub
            '    }
        End If
        '
        '    // the segments are skew and may intersect in a point
        '    // get the intersect parameter for S1
        '    float     sI = perp(v,w) / D;
        Dim sI As Single
        sI = perp(XA, YA, XB, YB, XA, YA, XM, YM) / D
        '    if (sI < 0 || sI > 1)                // no intersect with S1 'for segment
        '        return 0;
        'If sI < 0 Or sI > 1 Then result = 0: Exit Sub ' for a segment
        If sI < 0 Then result = 0 : Exit Sub ' for a ray
        '
        '    // get the intersect parameter for S2
        '    float     tI = perp(u,w) / D;
        Dim tI As Single
        tI = perp(XM, YM, XP, YP, XA, YA, XM, YM) / D
        '    if (tI < 0 || tI > 1)                // no intersect with S2 ' for a segment
        '        return 0;
        If tI < 0 Or tI > 1 Then result = 0 : Exit Sub ' for a segment
        'If tI < 0 Then result = 0: Exit Sub ' for a ray
        '
        '    *I0 = S1.P0 + sI * u;                // compute S1 intersect point
        XI0 = XM + sI * (XP - XM)
        YI0 = YM + sI * (YP - YM)

        '    return 1;
        result = 1
        Exit Sub
        '}
        '//===================================================================
    End Sub


    Public Function RelativeSaturation(ByVal x As Single, ByVal y As Single, ByVal ParamArray Other() As Single) As Single
        ' return relative saturation of P(x,y) versus ABCD defined as distance(P, ABCD)/distance(M, ABCD), with M=center of ABCD.
        ' Relative saturation = 100% in M (center of ABCD), 0% on edge of ABCD, -100% outside at same disance than B......
        ' relative staration range is [-infinity;1] if no error
        ' Relative saturation = 2 (200%) in case of error

        Dim XA As Single
        Dim YA As Single
        Dim XB As Single
        Dim YB As Single
        Dim XC As Single
        Dim YC As Single
        Dim XD As Single
        Dim YD As Single

        Dim XM As Single 'Center of [ABCD]
        Dim YM As Single

        Dim XI0 As Single ' Intercept of [MP) with segment [AB] or [BC] or [CD] or [DA]
        Dim YI0 As Single
        Dim XI1 As Single ' 2nd Intercept of [MP) with segment [AB] or [BC] or [CD] or [DA] in case overlap (should not occur as M strickly inside [ABCD]
        Dim YI1 As Single

        Dim Intercept As Integer
        Dim D_MP As Single      '                           to point M


        ' extraction XA...YD values from ParamArray Other() argument

        XA = Other(0)
        YA = Other(1)
        XB = Other(2)
        YB = Other(3)
        XC = Other(4)
        YC = Other(5)
        XD = Other(6)
        YD = Other(7)

        'RelativeSaturation = DistanceABCD(x, y, XA, YA, XB, YB, XC, YC, XD, YD) / _
        '                    DistanceABCD((XA + XB + XC + XD) / 4, (YA + YB + YC + YD) / 4, XA, YA, XB, YB, XC, YC, XD, YD)
        XM = (XA + XB + XC + XD) / 4
        YM = (YA + YB + YC + YD) / 4
        D_MP = CSng(((x - XM) ^ 2 + (y - YM) ^ 2) ^ 0.5)

        If D_MP = 0 Then RelativeSaturation = 1 : Exit Function '(P is equal to M)
        'init return
        RelativeSaturation = -9999
        '
        Call Segment_Intersection(XM, YM, x, y, XA, YA, XB, YB, XI0, YI0, XI1, YI1, Intercept) ' intersect of [MP) with [AB]
        If Intercept = 1 Then RelativeSaturation = CSng(1 - (D_MP / ((XI0 - XM) ^ 2 + (YI0 - YM) ^ 2) ^ 0.5)) * 100 : Exit Function
        If Intercept = 2 Then GoTo ErrorHandle

        Call Segment_Intersection(XM, YM, x, y, XB, YB, XC, YC, XI0, YI0, XI1, YI1, Intercept) ' intersect of [MP) with [BC]
        If Intercept = 1 Then RelativeSaturation = CSng(1 - (D_MP / ((XI0 - XM) ^ 2 + (YI0 - YM) ^ 2) ^ 0.5)) * 100 : Exit Function
        If Intercept = 2 Then GoTo ErrorHandle

        Call Segment_Intersection(XM, YM, x, y, XC, YC, XD, YD, XI0, YI0, XI1, YI1, Intercept) ' intersect of [MP) with [CD]
        If Intercept = 1 Then RelativeSaturation = CSng(1 - (D_MP / ((XI0 - XM) ^ 2 + (YI0 - YM) ^ 2) ^ 0.5)) * 100 : Exit Function
        If Intercept = 2 Then GoTo ErrorHandle

        Call Segment_Intersection(XM, YM, x, y, XD, YD, XA, YA, XI0, YI0, XI1, YI1, Intercept) ' intersect of [MP) with [DA]
        If Intercept = 1 Then RelativeSaturation = CSng(1 - (D_MP / ((XI0 - XM) ^ 2 + (YI0 - YM) ^ 2) ^ 0.5)) * 100 : Exit Function
        If Intercept = 2 Then GoTo ErrorHandle

        Exit Function

ErrorHandle:
        RelativeSaturation = 2
    End Function
    'Tested value 1.
    'Dim x As Single
    'Dim y As Single
    'Dim expectedanswer As Single
    'Dim xpoints(5) As Single
    'Dim ypoints(5) As Single
    'xpoints(0) = 0.315
    'ypoints(0) = 0.336
    'xpoints(1) = 0.336
    'ypoints(1) = 0.336
    'xpoints(2) = 0.336
    'ypoints(2) = 0.325
    'xpoints(3) = 0.314
    'ypoints(3) = 0.292
    'xpoints(4) = 0.295
    'ypoints(4) = 0.292
    'xpoints(5) = 0.295
    'ypoints(5) = 0.306
    'For index = 1 To 8
    '    If index = 1 Then x = 0.305 : y = 0.3145 : expectedanswer = 29.88
    '    If index = 2 Then x = 0.28 : y = 0.3145 : expectedanswer = -142.5
    '    If index = 3 Then x = 0.28 : y = 0.285 : expectedanswer = -74.38
    '    If index = 4 Then x = 0.275 : y = 0.28 : expectedanswer = -99.17
    '    If index = 5 Then x = 0.31516 : y = 0.3145 : expectedanswer = 99.95
    '    If index = 6 Then x = 0.32 : y = 0.335 : expectedanswer = 4.65
    '    If index = 7 Then x = 0.336 : y = 0.33 : expectedanswer = 0
    '    If index = 8 Then x = 1 : y = 2 : expectedanswer = -7739
    '    Dim tempreturn = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
    '    Console.WriteLine(index & ":" & tempreturn & ",expectedanswer:" & expectedanswer)
    'Next
    'Tested value 2.
    'Dim x As Single
    'Dim y As Single
    'Dim expectedanswer As Single
    'Dim xpoints(3) As Single
    'Dim ypoints(3) As Single
    '    xpoints(0) = 0.548
    '    ypoints(0) = 0.41
    '    xpoints(1) = 0.564
    '    ypoints(1) = 0.395
    '    xpoints(2) = 0.586
    '    ypoints(2) = 0.413
    '    xpoints(3) = 0.565
    '    ypoints(3) = 0.433
    '    For index = 1 To 6
    'If index = 1 Then x = 0.5647 : y = 0.4206 : expectedanswer = 56.4
    '        If index = 2 Then x = 0.56968 : y = 0.41599 : expectedanswer = 64.2
    '        If index = 3 Then x = 0.56407 : y = 0.41291 : expectedanswer = 88.56
    '        If index = 4 Then x = 0.548 : y = 0.41 : expectedanswer = 0
    '        If index = 5 Then x = 0.564 : y = 0.395 : expectedanswer = 0
    '        If index = 6 Then x = 1 : y = 2 : expectedanswer = -10141
    '        Dim tempreturn = mColor_Analysis.RelativeSaturation_Anypolygon(x, y, xpoints, ypoints)
    '        Console.WriteLine(index & ":" & tempreturn & ",expectedanswer:" & expectedanswer)
    '    Next
    Public Function RelativeSaturation_Anypolygon(ByVal x As Single, ByVal y As Single, ByVal xpoints() As Single, ByVal ypoints() As Single) As Single
        ' return relative saturation of P(x,y) versus ABCD defined as distance(P, ABCD)/distance(M, ABCD), with M=center of ABCD.
        ' Relative saturation = 100% in M (center of ABCD), 0% on edge of ABCD, -100% outside at same disance than B......
        ' relative staration range is [-infinity;1] if no error
        ' Relative saturation = 2 (200%) in case of error

        If xpoints.Length <> ypoints.Length Then Return -9999

        Dim XM As Single 'Center of [ABCD]
        Dim YM As Single

        Dim XI0 As Single ' Intercept of [MP) with segment [AB] or [BC] or [CD] or [DA]
        Dim YI0 As Single
        Dim XI1 As Single ' 2nd Intercept of [MP) with segment [AB] or [BC] or [CD] or [DA] in case overlap (should not occur as M strickly inside [ABCD]
        Dim YI1 As Single

        Dim Intercept As Integer

        Dim D_MP As Single      '                           to point M

        'RelativeSaturation = DistanceABCD(x, y, XA, YA, XB, YB, XC, YC, XD, YD) / _
        '                    DistanceABCD((XA + XB + XC + XD) / 4, (YA + YB + YC + YD) / 4, XA, YA, XB, YB, XC, YC, XD, YD)
        For index = 0 To xpoints.Length - 1
            XM += xpoints(index)
        Next
        XM /= xpoints.Length
        For index = 0 To ypoints.Length - 1
            YM += ypoints(index)
        Next
        YM /= xpoints.Length

        D_MP = CSng(((x - XM) ^ 2 + (y - YM) ^ 2) ^ 0.5)

        If D_MP = 0 Then Return 1 '(P is equal to M)

        'init return
        RelativeSaturation_Anypolygon = -9999
        '
        For index = 0 To xpoints.Count - 2
            Call Segment_Intersection(XM, YM, x, y, xpoints(index), ypoints(index), xpoints(index + 1), ypoints(index + 1), XI0, YI0, XI1, YI1, Intercept) ' intersect of [MP) with [AB]
            If Intercept = 1 Then Return CSng(1 - (D_MP / ((XI0 - XM) ^ 2 + (YI0 - YM) ^ 2) ^ 0.5)) * 100
            If Intercept = 2 Then Return 2
        Next

        Call Segment_Intersection(XM, YM, x, y, xpoints(xpoints.Count - 1), ypoints(ypoints.Count - 1), xpoints(0), ypoints(0), XI0, YI0, XI1, YI1, Intercept) ' intersect of [MP) with [AB]
        If Intercept = 1 Then Return CSng(1 - (D_MP / ((XI0 - XM) ^ 2 + (YI0 - YM) ^ 2) ^ 0.5)) * 100
        If Intercept = 2 Then Return 2

    End Function


    Public Function Sat_CIE(ByVal x As Single, ByVal y As Single) As Single
        ' range Table_Ang2Lambda2, with angle as input; distance E(lambda) en N (+4)
        Const D65_X As Single = 0.31271
        Const D65_Y As Single = 0.32902
        Dim WaveLength As Single
        WaveLength = Dominant_Wavelength(x, y)

        ' Case black or color=white D65
        If (x = 0 And y = 0) Or (x = D65_X And y = D65_Y) Then Sat_CIE = 0 : Exit Function

        ' A corriger
        'Sat_CIE = ((x - D65_X) ^ 2 + (y - D65_Y) ^ 2) ^ 0.5 / _
        'WorksheetFunction.VLookup(WaveLength, Range("Table_L_Distance"), 3, True) _
        '+ WorksheetFunction.VLookup(WaveLength, Range("Table_L_Distance"), 5, True) * _
        '(WaveLength - Int(WaveLength))

    End Function

    Public Function LoadCieSettings(ByVal path As String) As Boolean
        Dim file As StreamReader = Nothing
        Dim line As String
        Dim token() As String
        Dim i As Integer
        Dim ii As Integer

        'L	|   x   |	y   |	Y   |	R   |	G   |	B   |	tan (EC)    |	angle   |	L   |	f'= (Y_n+1-Y_n) / (X_n+1-X_n)   |	distance E, L   |	L'(angle)   |	D' =d(dist E_L) / d_lambda

        Try
            i = 0
            ' Open the file
            file = New StreamReader(path, System.Text.Encoding.Default)
            ' Read a line
            line = file.ReadLine
            line = file.ReadLine
            ' While it is not reached the end of the file
            Do While Not (file.EndOfStream)
                ' Read a line
                line = file.ReadLine
                ' If the line is not empty and is not a comment
                If (line <> "" AndAlso Not (line.StartsWith("'"))) Then
                    ' Read Data
                    token = Split(line, vbTab)
                    For ii = 0 To 13
                        Cie_Data(ii, i) = CSng(token(ii))
                    Next ii
                    i = i + 1
                End If
                ' end file
                If i >= 500 Then
                    Exit Do
                End If
            Loop

            ' Return False
            LoadCieSettings = False

        Catch ex As Exception
            ' Return True
            LoadCieSettings = True

        Finally
            ' Close the file
            If (file IsNot Nothing) Then
                file.Close()
                file = Nothing
            End If
        End Try

    End Function


End Module
