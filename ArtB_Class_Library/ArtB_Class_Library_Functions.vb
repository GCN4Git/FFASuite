Module ArtB_Class_Library_Functions

    Public Const FIELD_SEPARATOR_STR As String = ","
    Public Const STRING_SEPARATOR_STR As String = "'"
    Public Const RECORD_SEPARATOR_STR As String = ";"
    Public Const ARTB_DATE_FORMATSTR As String = "yyyy-MM-dd"
    Public Const ARTB_TIME_FORMATSTR As String = "HH:mm:ss.ffffff"
    Public Const ARTB_DATETIME_FORMATSTR As String = "yyyy-MM-dd HH:mm:ss.ffffff"
    Public Const ARTB_DATETIME_REPORT_FORMATSTR As String = "yyyy-MM-dd HH:mm:ss"
    Public Const ARTB_DOUBLE_FORMATSTR As String = "#,##0.000000"
    Public Const ARTB_CURRENCY_FORMATSTR As String = "#,##0.00"
    Public Const ARTB_INT_FORMATSTR As String = "#,##0"
    Public Const ARTB_RATIOPRICE_FORMATSTR As String = "#,##0.000"
    Public Const REQUEST_REPLY_TIMEOUT As Integer = 120

    Public ArtBSNumberInfo As New System.Globalization.NumberFormatInfo

    Public ArtBNumberInfo As New System.Globalization.NumberFormatInfo
    Public ArtBRatioPriceInfo As New System.Globalization.NumberFormatInfo
    Public ArtBIntNumberInfo As New System.Globalization.NumberFormatInfo
    Public ArtBDateInfo As New System.Globalization.DateTimeFormatInfo
    Public ArtBDateTimeInfo As New System.Globalization.DateTimeFormatInfo
    Public ArtBTimeInfo As New System.Globalization.DateTimeFormatInfo

    Public Sub ArtBNumberInfoInit()
        ArtBNumberInfo.CurrencyDecimalDigits = 2
        ArtBNumberInfo.CurrencySymbol = "$"
        ArtBNumberInfo.NumberDecimalDigits = 2
        ArtBNumberInfo.NumberDecimalSeparator = "."
        ArtBNumberInfo.NumberGroupSeparator = ","
        ArtBSNumberInfo.NumberDecimalDigits = 4
        ArtBSNumberInfo.NumberDecimalSeparator = "."
        ArtBSNumberInfo.NumberGroupSeparator = ","
        ArtBIntNumberInfo.NumberDecimalDigits = 0
        ArtBIntNumberInfo.NumberDecimalSeparator = "."
        ArtBIntNumberInfo.NumberGroupSeparator = ","
        ArtBRatioPriceInfo.NumberDecimalDigits = 0
        ArtBRatioPriceInfo.NumberDecimalSeparator = "."
        ArtBRatioPriceInfo.NumberGroupSeparator = ","
    End Sub

    Public Function ArtBRespondMessageType(ByVal MessageType As Integer) As Integer
        Select Case MessageType
            Case ArtBMessages.ChangeCounterPartyLimits
                Return ArtBMessages.RespondChangeCounterPartyLimits
            Case ArtBMessages.ChangeTraderAuthorities
                Return ArtBMessages.RespondChangeTraderAuthorities
            Case ArtBMessages.ArtBServiceAlive
                Return ArtBMessages.RespondArtBServiceAlive
            Case ArtBMessages.OrderFFANew
                Return ArtBMessages.RespondOrderFFANew
            Case ArtBMessages.OrderFFAAmmend
                Return ArtBMessages.RespondOrderFFAAmmend
            Case ArtBMessages.OrderFFAFirmUp
                Return ArtBMessages.RespondOrderFFAFirmUp
            Case ArtBMessages.ChangeFFATrade
                Return ArtBMessages.RespondChangeFFATrade
            Case ArtBMessages.OrderFFAChangeOwner
                Return ArtBMessages.RespondOrderFFAAmmend
            Case ArtBMessages.FicticiousTrade
                Return ArtBMessages.RespondFicticiousTrade
        End Select
        Return -1
    End Function

    Public Function Str2Int(ByRef o As Object) As Integer
        Str2Int = 0
        Try
            Dim s As String
            If Not (TypeOf s Is String) Then s = CType(o, String)
            If s.Length < 1 Then Exit Function
            Str2Int = Integer.Parse(s, ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Int2Str(ByRef i As Integer) As String
        Int2Str = "0"
        Try
            Int2Str = i.ToString(ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2NullInt(ByRef s As String) As Nullable(Of Integer)
        Str2NullInt = Nothing
        If s.Length < 1 Then Exit Function
        Try
            Str2NullInt = Integer.Parse(s, ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2NullBool(ByRef s As String) As Nullable(Of Boolean)
        Str2NullBool = Nothing
        Dim i As Integer = 0
        If s.Length < 1 Then Exit Function
        Try
            i = Integer.Parse(s, ArtBIntNumberInfo)
            Str2NullBool = False
            If i <> 0 Then Str2NullBool = True
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullInt2Str(ByRef i As Nullable(Of Integer)) As String
        NullInt2Str = ""
        If IsNothing(i) Then Exit Function
        Try
            Dim i0 As Integer = CType(i, Integer)
            NullInt2Str = i0.ToString(ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullInt2Int(ByRef i As Nullable(Of Integer)) As Integer
        NullInt2Int = 0
        If IsNothing(i) Then Exit Function
        If IsDBNull(i) Then Exit Function
        Try
            NullInt2Int = CType(i, Integer)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullInt642Int64(ByRef i As Nullable(Of Int64)) As Int64
        NullInt642Int64 = 0
        If IsNothing(i) Then Exit Function
        If IsDBNull(i) Then Exit Function
        Try
            NullInt642Int64 = i
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullDouble2Double(ByRef i As Nullable(Of Double)) As Double
        NullDouble2Double = 0
        If IsNothing(i) Then Exit Function
        If IsDBNull(i) Then Exit Function
        Try
            NullDouble2Double = CType(i, Double)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2Short(ByRef s As String) As Short
        Str2Short = 0
        Try
            Str2Short = Int16.Parse(s, ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Short2Str(ByRef i As Short) As String
        Short2Str = "0"
        Try
            Short2Str = i.ToString(ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2NullShort(ByRef s As String) As Nullable(Of Short)
        Str2NullShort = Nothing
        If s = "" Then Exit Function
        Try
            Str2NullShort = Int16.Parse(s, ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullShort2Str(ByRef i As Nullable(Of Short)) As String
        NullShort2Str = ""
        If IsNothing(i) Then Exit Function
        Try
            Dim i0 As Short = CType(i, Short)
            NullShort2Str = i0.ToString(ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2Byte(ByRef s As String) As Byte
        Str2Byte = 0
        Try
            Str2Byte = Byte.Parse(s, ArtBIntNumberInfo)
        Catch ex As Exception

        End Try
    End Function

    Public Function Byte2Str(ByRef i As Byte) As String
        Byte2Str = "0"
        Try
            Byte2Str = i.ToString(ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2NullByte(ByRef s As String) As Nullable(Of Byte)
        Str2NullByte = Nothing
        If s = "" Then Exit Function
        Try
            Str2NullByte = Byte.Parse(s, ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullByte2Str(ByRef i As Nullable(Of Byte)) As String
        NullByte2Str = ""
        If IsNothing(i) Then Exit Function
        Try
            Dim i0 As Byte = i
            NullByte2Str = i0.ToString(ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2Double(ByRef s As String) As Double
        Str2Double = 0
        Try
            Str2Double = Double.Parse(s, ArtBNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Double2Str(ByRef d As Double) As String
        Double2Str = "0"
        Try
            Double2Str = d.ToString(ArtBNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2NullDouble(ByRef s As String) As Nullable(Of Double)
        Str2NullDouble = Nothing
        If s = "" Then Exit Function
        Try
            Str2NullDouble = Double.Parse(s, ArtBNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullDouble2Str(ByRef d As Nullable(Of Double)) As String
        NullDouble2Str = ""
        If IsNothing(d) Then Exit Function
        Try
            Dim d0 As Double = d
            NullDouble2Str = d0.ToString(ArtBNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Bool2Int(ByRef b As Boolean) As Integer
        Bool2Int = 0
        If b = True Then Bool2Int = 1
    End Function

    Public Function Str2Bool(ByRef s As String) As Boolean
        Str2Bool = False
        Dim i As Integer
        Try
            i = Integer.Parse(s, ArtBIntNumberInfo)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
        If i <> 0 Then Str2Bool = True
    End Function

    Public Function Bool2Str(ByRef b As Nullable(Of Boolean)) As String
        Bool2Str = "0"
        If b = True Then Bool2Str = "1"
    End Function

    Public Function NullBool2Str(ByRef b As Nullable(Of Boolean)) As String
        If IsNothing(b) Then Return ""
        NullBool2Str = "0"
        If b = True Then NullBool2Str = "1"
    End Function


    Public Function Str2Date(ByRef s As String) As DateTime
        Str2Date = DateTime.Parse("2000-01-01")
        Try
            Str2Date = DateTime.Parse(s)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Date2Str(ByRef d As DateTime) As String
        Date2Str = "2000-01-01"
        Try
            Date2Str = d.ToString(ARTB_DATE_FORMATSTR)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2NullDate(ByRef s As String) As Nullable(Of DateTime)
        Str2NullDate = Nothing
        If s = "" Then Exit Function
        Try
            Str2NullDate = DateTime.Parse(s)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullDate2Str(ByRef d As Nullable(Of DateTime)) As String
        NullDate2Str = ""
        If IsNothing(d) Then Exit Function
        Try
            Dim d0 As DateTime = d
            NullDate2Str = d0.ToString(ARTB_DATE_FORMATSTR)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2DateTime(ByRef s As String) As DateTime
        Str2DateTime = DateTime.SpecifyKind("2000-01-01", DateTimeKind.Utc)
        Try
            Str2DateTime = DateTime.SpecifyKind(DateTime.Parse(s), DateTimeKind.Utc)
        Catch ex As Exception

        End Try
    End Function

    Public Function DateTime2Str(ByRef d As DateTime) As String
        DateTime2Str = "2000-01-01"
        Try
            DateTime2Str = d.ToString(ARTB_DATETIME_FORMATSTR)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2NullDateTime(ByRef s As String) As Nullable(Of DateTime)
        Str2NullDateTime = Nothing
        If s = "" Then Exit Function
        Try
            Str2NullDateTime = DateTime.SpecifyKind(DateTime.Parse(s), DateTimeKind.Utc)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullDateTime2Str(ByRef d As Nullable(Of DateTime)) As String
        NullDateTime2Str = ""
        If IsNothing(d) Then Exit Function
        Try
            Dim d0 As DateTime = DateTime.SpecifyKind(d, DateTimeKind.Utc)
            NullDateTime2Str = d0.ToString(ARTB_DATETIME_FORMATSTR)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2Time(ByRef s As String) As TimeSpan
        Str2Time = TimeSpan.Parse("00:00:00")
        Try
            Str2Time = TimeSpan.Parse(s)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Time2Str(ByRef d As TimeSpan) As String
        Time2Str = "00:00:00"
        Try
            Time2Str = d.ToString()
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2NullTime(ByRef s As String) As Nullable(Of TimeSpan)
        Str2NullTime = Nothing
        If s = "" Then Exit Function
        Try
            Str2NullTime = TimeSpan.Parse(s)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function NullTime2Str(ByRef t As Nullable(Of TimeSpan)) As String
        NullTime2Str = ""
        If IsNothing(t) Then Exit Function
        Try
            Dim t0 As TimeSpan = t
            NullTime2Str = t0.ToString()
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2QuotedStr(ByRef s As Object) As String
        Str2QuotedStr = ""
        If IsNothing(s) Then Exit Function
        If System.DBNull.Value.Equals(s) Then Exit Function
        Try
            Str2QuotedStr = s.ToString()
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
        If Len(Str2QuotedStr) < 1 Then Exit Function
        Str2QuotedStr = "'" & Str2QuotedStr & "'"
    End Function

    Public Function Str2NullChar(ByRef s As Object) As Nullable(Of Char)
        Str2NullChar = Nothing
        If IsNothing(s) Then Exit Function
        If System.DBNull.Value.Equals(s) Then Exit Function
        If s.length() < 1 Then Exit Function
        Dim ms As String = s.ToString()
        If ms.Length() < 1 Then Exit Function
        Try
            Str2NullChar = CType(ms.Substring(0, 1), Char)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function Str2Char(ByRef s As Object) As Char
        Str2Char = " "
        If IsNothing(s) Then Exit Function
        If System.DBNull.Value.Equals(s) Then Exit Function
        If s.length() < 1 Then Exit Function
        Dim ms As String = s.ToString()
        If ms.Length() < 1 Then Exit Function
        Try
            Str2Char = CType(ms.Substring(0, 1), Char)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function ParseStrings(ByVal s As String) As List(Of String)
        ParseStrings = New List(Of String)
        Dim f As Integer
        Dim r As String = ""
        While (True)
            f = s.IndexOf(RECORD_SEPARATOR_STR)
            If f < 0 Or f >= s.Length() Then
                If Len(s) >= 1 Then ParseStrings.Add(s)
                Exit Function
            End If
            If f = 0 Then
                If Len(s) > 1 Then
                    s = s.Substring(1)
                Else
                    Exit Function
                End If
                Continue While
            End If
            If f = s.Length() - 1 Then
                r = s.Substring(0, f)
                ParseStrings.Add(r)
                Exit Function
            End If
            r = s.Substring(0, f)
            ParseStrings.Add(r)
            s = s.Substring(f + 1)
        End While
    End Function

    Public Function ParseRecordString(ByRef SourceStr As String, ByVal StringList As List(Of String)) As Integer
        Dim f As Integer
        Dim I As Integer = 1
        Dim s As String
        Dim t As String = ""
        Dim c As String
        f = SourceStr.IndexOf(RECORD_SEPARATOR_STR)
        If f < 0 Or f > SourceStr.Length() Then
            ParseRecordString = ArtBErrors.RecordSeparatorNotFound
            Exit Function
        End If
        StringList.Clear()
        ParseRecordString = ArtBErrors.Success
        s = SourceStr.Substring(0, f)
        SourceStr = SourceStr.Substring(f + 1)
        If s.Length() < 1 Then
            Exit Function
        End If
        Dim strMode As Boolean = False
        Dim firstFieldChar As Boolean = True
        I = 0
        Do While (s.Substring(I) <> RECORD_SEPARATOR_STR And I < s.Length())
            c = s.Substring(I, 1)
            If firstFieldChar Then
                If c = STRING_SEPARATOR_STR Then
                    strMode = True
                    c = ""
                End If
                firstFieldChar = False
            End If
            If c = FIELD_SEPARATOR_STR And strMode = False Then
                StringList.Add(t)
                t = ""
                firstFieldChar = True
            ElseIf c = STRING_SEPARATOR_STR And strMode = True Then
                strMode = False
                c = ""
            Else
                t = t + c
            End If
            I = I + 1
        Loop
        StringList.Add(t)
        ParseRecordString = ArtBErrors.Success
    End Function

    Public Function ParseRecordString2(ByRef SourceStr As String, ByVal StringList As List(Of String)) As Integer
        Dim f As Integer
        Dim I As Integer = 1
        Dim s As String = SourceStr
        Dim t As String = ""
        Dim c As String
        StringList.Clear()
        ParseRecordString2 = ArtBErrors.Success
        Dim strMode As Boolean = False
        Dim firstFieldChar As Boolean = True
        I = 0
        Do While (s.Substring(I) <> RECORD_SEPARATOR_STR And I < s.Length())
            c = s.Substring(I, 1)
            If firstFieldChar Then
                If c = STRING_SEPARATOR_STR Then
                    strMode = True
                    c = ""
                End If
                firstFieldChar = False
            End If
            If c = FIELD_SEPARATOR_STR And strMode = False Then
                StringList.Add(t)
                t = ""
                firstFieldChar = True
            ElseIf c = STRING_SEPARATOR_STR And strMode = True Then
                strMode = False
                c = ""
            Else
                t = t + c
            End If
            I = I + 1
        Loop
        StringList.Add(t)
        ParseRecordString2 = ArtBErrors.Success
    End Function


    Public Function GetViewClass(ByRef c As Collection, ByVal k As String) As Object
        GetViewClass = Nothing
        Try
            If c.Contains(k) Then GetViewClass = c(k)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function InsertOrReplace(ByRef c As Collection, ByRef o As Object, ByVal k As String, Optional ByRef bExisted As Boolean = False) As Integer
        InsertOrReplace = ArtBErrors.Success
        If c Is Nothing Then Exit Function
        Try
            If c.Contains(k) Then
                bExisted = True
                c.Remove(k)
            End If
        Catch ex As Exception
            InsertOrReplace = ArtBErrors.RemoveFromCollectionFailed
            Debug.Print(ex.ToString())
            Exit Function
        End Try
        Try
            c.Add(o, k)
        Catch ex As Exception
            InsertOrReplace = ArtBErrors.InsertInCollectionFailed
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function RemoveFromCollection(ByRef c As Collection, ByRef o As Object, ByVal k As String) As Integer
        RemoveFromCollection = ArtBErrors.Success
        If c Is Nothing Then Exit Function
        Try
            If c.Contains(k) Then
                c.Remove(k)
            End If
        Catch ex As Exception
            RemoveFromCollection = ArtBErrors.RemoveFromCollectionFailed
            Debug.Print(ex.ToString())
            Exit Function
        End Try
    End Function

    Public Function Max(ByVal a As Double, ByVal b As Double) As Double
        Max = a
        If (b > a) Then Max = b
    End Function

    Public Function Min(ByVal a As Double, ByVal b As Double) As Double
        Min = a
        If (b < a) Then Min = b
    End Function

    Public Function Max(ByVal a As DateTime, ByVal b As DateTime) As DateTime
        Max = a
        If (b > a) Then Max = b
    End Function

    Public Function Min(ByVal a As DateTime, ByVal b As DateTime) As DateTime
        Min = a
        If (b < a) Then Min = b
    End Function

    Public Function OrderTypeDescr(ByVal OrderType As Integer) As String
        OrderTypeDescr = ""
        Select Case OrderType
            Case OrderTypes.FFA
                OrderTypeDescr = "FFA"
            Case OrderTypes.RatioSpread
                OrderTypeDescr = "Ratio Spread"
            Case OrderTypes.CalendarSpread
                OrderTypeDescr = "Calendar Spread"
            Case OrderTypes.MarketSpread
                OrderTypeDescr = "Market Spread"
            Case OrderTypes.PriceSpread
                OrderTypeDescr = "Price Spread"
        End Select
    End Function


    Public Function Round(ByVal x As Double, _
                          Optional ByVal tick As Double = 1.0, _
                          Optional ByVal digits As Integer = 0, _
                          Optional ByVal dir As Integer = 0) As Double

        Try
            If x < -10000000000.0 Then Return x
            If x > 10000000000.0 Then Return x
            Dim i As Int64
            Dim dx As Double = dir * 0.5
            If dir = 2 Then dx = 0.5
            If dir = -2 Then dx = -0.5

            If tick = 0 Then tick = Math.Pow(10, -digits)

            If tick <> 0 Then
                If Math.Abs(x / tick - Int(x / tick)) < 0.000001 Or _
                   Math.Abs(x / tick - Int(x / tick) - tick) < 0.000001 Then dx = 0

                If Math.Abs(dir) <= 1 Then
                    i = CLng(x / tick + dx)
                Else
                    i = CLng(Int(x / tick + dx))
                End If
                x = i * tick
                Return x
            End If

        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try

    End Function

    Public Function FormatPriceString(ByVal PRICING_TICK As Double, Optional ByRef n As Integer = 0) As String
        If PRICING_TICK <= 0 Then Return "#,##0"

        n = Round(-Math.Log10(PRICING_TICK), 1, , 1)
        If (n < 0) Then n = 0
        Select Case n
            Case Is <= 0
                FormatPriceString = "#,##0"
            Case 1
                FormatPriceString = "#,##0.0"
            Case 2
                FormatPriceString = "#,##0.00"
            Case 3
                FormatPriceString = "#,##0.000"
            Case 4
                FormatPriceString = "#,##0.0000"
            Case Else
                FormatPriceString = "#,##0"
        End Select
    End Function

    Public Function TickDecimalPlaces(ByVal tick As Double) As Integer
        If tick <= 0 Then Return 0
        Dim n As Integer = Round(-Math.Log10(tick), 1, , 1)
        If (n < 0) Then n = 0
        Return n
    End Function

    Public Function GreatestDivisor(ByVal numberA As Long, ByVal numberB As Long) As Long
        If (numberB = 0) Then
            GreatestDivisor = numberA
        Else
            GreatestDivisor = GreatestDivisor(numberB, numberA Mod numberB)
        End If
    End Function

    Public Sub Swap(ByRef a As Object, ByRef b As Object)
        Dim t As Object = a
        a = b
        b = t
    End Sub
End Module

