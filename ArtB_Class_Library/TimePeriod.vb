Imports System.Runtime.Serialization

<DataContract()> Public Class ArtBTimePeriod
    Private m_ROUTE_ID As Integer
    Private m_SERVER_DATE As Date
    Private m_StartMonth As Integer = -1
    Private m_EndMonth As Integer = -1
    Private m_Descr As String = ""
    Private m_Selected As Boolean = False
    Private m_StartDate As Date
    Private m_EndDate As Date
    Private m_MM1 As Integer
    Private m_YY1 As Integer
    Private m_MM2 As Integer
    Private m_YY2 As Integer
    Private m_DPM As Double
    Private m_TotDays As Integer

    <DataMember()> Public Property ROUTE_ID As Integer
        Get
            Return m_ROUTE_ID
        End Get
        Set(value As Integer)
            m_ROUTE_ID = value
        End Set
    End Property
    <DataMember()> Public Property SERVER_DATE As Date
        Get
            Return m_SERVER_DATE
        End Get
        Set(value As Date)
            m_SERVER_DATE = value
        End Set
    End Property
    <DataMember()> Public Property StartMonth As Integer
        Get
            Return m_StartMonth
        End Get
        Set(value As Integer)
            m_StartMonth = value
        End Set
    End Property
    <DataMember()> Public Property EndMonth As Integer
        Get
            Return m_EndMonth
        End Get
        Set(value As Integer)
            m_EndMonth = value
        End Set
    End Property
    <DataMember()> Public Property Descr As String
        Get
            Return m_Descr
        End Get
        Set(value As String)
            m_Descr = value
        End Set
    End Property
    <DataMember()> Public Property Selected As Boolean
        Get
            Return m_Selected
        End Get
        Set(value As Boolean)
            m_Selected = value
        End Set
    End Property
    <DataMember()> Public Property StartDate As Date
        Get
            Return m_StartDate
        End Get
        Set(value As Date)
            m_StartDate = value
        End Set
    End Property
    <DataMember()> Public Property EndDate As Date
        Get
            Return m_EndDate
        End Get
        Set(value As Date)
            m_EndDate = value
        End Set
    End Property
    <DataMember()> Public Property MM1 As Integer
        Get
            Return m_MM1
        End Get
        Set(value As Integer)
            m_MM1 = value
        End Set
    End Property
    <DataMember()> Public Property YY1 As Integer
        Get
            Return m_YY1
        End Get
        Set(value As Integer)
            m_YY1 = value
        End Set
    End Property
    <DataMember()> Public Property MM2 As Integer
        Get
            Return m_MM2
        End Get
        Set(value As Integer)
            m_MM2 = value
        End Set
    End Property
    <DataMember()> Public Property YY2 As Integer
        Get
            Return m_YY2
        End Get
        Set(value As Integer)
            m_YY2 = value
        End Set
    End Property
    <DataMember()> Public Property DPM As Double
        Get
            Return m_DPM
        End Get
        Set(value As Double)
            m_DPM = value
        End Set
    End Property
    <DataMember()> Public Property TotDays As Integer
        Get
            Return m_TotDays
        End Get
        Set(value As Integer)
            m_TotDays = value
        End Set
    End Property

    Public Sub FillDPM()
        Dim i As Integer, m As Integer, y As Integer
        TotDays = 0
        For i = StartMonth To EndMonth
            y = Int(i / 12)
            m = (i - y * 12) + 1
            y = y + 2000
            TotDays = TotDays + DateTime.DaysInMonth(y, m)
        Next
        DPM = TotDays / (EndMonth - StartMonth + 1)
    End Sub

    Public Sub FillMY(ByVal a_mm1 As Integer, ByVal a_yy1 As Integer, _
                   ByVal a_mm2 As Integer, ByVal a_yy2 As Integer)
        MM1 = a_mm1
        YY1 = a_yy1 Mod 2000
        MM2 = a_mm2
        YY2 = a_yy2 Mod 2000
        StartMonth = YY1 * 12 + MM1 - 1
        EndMonth = YY2 * 12 + MM2 - 1
    End Sub

    Public Sub FillDates()
        Dim s As String
        If (StartMonth = -1) Then
            YY1 = 0
            MM1 = 1
        Else
            YY1 = Int(StartMonth / 12)
            MM1 = (StartMonth - YY1 * 12) + 1
        End If
        Try
            s = "20" & Format(YY1, "00") & "-" & Format(MM1, "00") & "-01"
            StartDate = Date.Parse(s)
        Catch e As Exception
        End Try
        If EndMonth = -1 Then
            YY2 = 0
            MM2 = 1
        Else
            YY2 = Int(EndMonth / 12)
            MM2 = (EndMonth - YY2 * 12) + 1
        End If
        Try
            s = "20" & Format(YY2, "00") & "-" & Format(MM2, "00") & "-" & Format(DateTime.DaysInMonth(YY2, MM2), "00")
            EndDate = Date.Parse(s)
        Catch e As Exception
        End Try
    End Sub

    Public Function AddOverlapping(ByRef t As ArtBTimePeriod) As Boolean
        AddOverlapping = False
        If t Is Nothing Then Exit Function
        If t.EndMonth < StartMonth - 1 Then Exit Function
        If EndMonth < StartMonth - 1 Then Exit Function
        If t.StartMonth < StartMonth Then StartMonth = t.StartMonth
        If t.EndMonth > EndMonth Then EndMonth = t.EndMonth
        AddOverlapping = True
    End Function

    Public Sub CreateDescr()
        FillDates()
        Descr = ""
        Dim i As Integer, y As Integer
        If StartMonth = EndMonth Then
            i = (StartMonth Mod 12) + 1
            Descr = MonthShort(MM1) & "-" & Format(YY1, "00")
            Exit Sub
        End If
        If EndMonth Mod 3 <> 2 Then
            If EndMonth - StartMonth >= 4 Then
                Descr = MonthShort(MM1) & "-" & Format(YY1, "00")
                i = EndMonth - StartMonth
                Descr = Descr & "+" & Format(i, "0") & " Months"
                Exit Sub
            Else
                For i = StartMonth To EndMonth
                    If Len(Descr) > 0 Then Descr = Descr & "+"
                    Descr = Descr & MonthShort(i Mod 12 + 1) & "-" & Format(Int(i / 12), "00")
                Next i
                Exit Sub
            End If
        End If
        Dim YEM As Integer = EndMonth
        Dim SY As Integer = YY1 + 1
        Dim EY As Integer = YY2
        If MM2 = 12 Then
            If MM1 = 1 Then
                YEM = -1
                SY = YY1
            Else
                YEM = SY * 12 - 1
            End If
            For i = SY To EY
                If Len(Descr) > 0 Then Descr = Descr & "+"
                Descr = Descr & "Cal-" & Format(i, "00")
            Next
        End If
        If StartMonth > YEM Then Exit Sub
        Dim mDescr As String = ""
        Dim qDescr As String = ""
        Dim minQ As Integer = Int(StartMonth / 3)
        If minQ * 3 < StartMonth Then
            minQ = minQ + 1
            For i = StartMonth To minQ * 3 - 1
                If Len(mDescr) > 0 Then mDescr = mDescr & "+"
                mDescr = mDescr & MonthShort(i Mod 12 + 1) & "-" & Format(Int(i / 12), "00")
            Next
        End If

        Dim maxQ As Integer = Int(YEM / 3)
        For i = minQ To maxQ
            If Len(qDescr) > 0 Then qDescr = qDescr & "+"

            y = Int(i / 4)
            If i Mod 4 = 0 And i + 3 <= maxQ Then
                qDescr = qDescr & "Cal-" & Format(y, "00")
                i += 3
            Else
                qDescr = qDescr & "Q" & Format((i Mod 4) + 1, "0") & "-" & Format(y, "00")
            End If

        Next
        If Len(qDescr) > 0 Then
            If Len(Descr) > 0 Then Descr = "+" & Descr
            Descr = qDescr + Descr
        End If
        If Len(mDescr) > 0 Then
            If Len(Descr) > 0 Then Descr = "+" & Descr
            Descr = mDescr + Descr
        End If
    End Sub

    Public Sub ParseDescr()
        Dim i As Integer
        Dim qr As Integer
        Dim y As Integer
        If Len(Descr) >= 3 Then
            For i = 1 To 12
                If Left(Descr, 3) = MonthShort(i) Then
                    y = Str2Int(Right(Descr, 2))
                    StartMonth = y * 12 + i - 1
                    EndMonth = y * 12 + i - 1
                    Exit Sub
                End If
            Next
            If Left(Descr, 3) = "Cal" Then
                y = Str2Int(Right(Descr, 2))
                StartMonth = y * 12
                EndMonth = y * 12 + 11
            End If
        End If
        Dim s As String = Descr
        Dim q As String
        StartMonth = 1000
        EndMonth = -1000
        While Len(s) > 1
            Dim j As Integer = s.IndexOf("+")
            If j > 1 Then
                q = s.Substring(0, j)
                s = s.Substring(j)
            Else
                q = s
                s = ""
            End If
            If Left(q, 1) <> "Q" Then
                StartMonth = -1
                EndMonth = -1
                Exit Sub
            End If
            qr = Str2Int(Mid(q, 3, 1))
            If qr < 1 Or qr > 4 Then
                StartMonth = -1
                EndMonth = -1
                Exit Sub
            End If
            y = Str2Int(Right(q, 2))
            Dim sm As Integer = y * 12 + (qr - 1) * 3
            Dim em As Integer = y * 12 + (qr - 1) * 3 + 2
            If StartMonth > sm Then StartMonth = sm
            If EndMonth < em Then EndMonth = em
        End While
    End Sub

    Public Function MonthShort(ByVal i As Integer) As String
        MonthShort = ""
        Select Case i
            Case 1
                MonthShort = "Jan"
            Case 2
                MonthShort = "Feb"
            Case 3
                MonthShort = "Mar"
            Case 4
                MonthShort = "Apr"
            Case 5
                MonthShort = "May"
            Case 6
                MonthShort = "Jun"
            Case 7
                MonthShort = "Jul"
            Case 8
                MonthShort = "Aug"
            Case 9
                MonthShort = "Sep"
            Case 10
                MonthShort = "Oct"
            Case 11
                MonthShort = "Nov"
            Case 12
                MonthShort = "Dec"
        End Select
    End Function

    Public Sub Add(ByVal s As String)
        Dim tp As New ArtBTimePeriod
        tp.Descr = s
        tp.ParseDescr()
        If EndMonth = -1 Then
            EndMonth = tp.EndMonth
            StartMonth = tp.StartMonth
            Exit Sub
        End If
        If StartMonth > tp.StartMonth Then StartMonth = tp.StartMonth
        If EndMonth > tp.EndMonth Then EndMonth = tp.EndMonth
    End Sub

    Public Function AddConsecutive(ByRef tp As ArtBTimePeriod) As Boolean
        AddConsecutive = True
        If EndMonth = -1 Then
            EndMonth = tp.EndMonth
            StartMonth = tp.StartMonth
            FillDates()
            Exit Function
        End If
        StartMonth = Min(StartMonth, tp.StartMonth)
        EndMonth = Max(EndMonth, tp.EndMonth)
        FillDates()
        Exit Function

        If StartMonth = tp.EndMonth + 1 Then
            StartMonth = tp.StartMonth
            Exit Function
        End If
        If EndMonth = tp.StartMonth - 1 Then
            EndMonth = tp.EndMonth
            Exit Function
        End If
        AddConsecutive = False
    End Function

    Public Function TotalDays() As Integer
        FillDates()
        TotalDays = 1 + DateDiff(DateInterval.Day, StartDate, EndDate)
        TotDays = TotalDays
    End Function

    Public Function TotalMonths() As Integer
        FillDates()
        TotalMonths = 1 + DateDiff(DateInterval.Month, StartDate, EndDate)
    End Function

    Public Function MinDaysInMonth() As Integer
        Dim i As Integer, y As Integer, m As Integer, d As Integer
        If StartMonth = -1 Or EndMonth = -1 Then
            MinDaysInMonth = 0
            Exit Function
        End If
        MinDaysInMonth = 31
        For i = StartMonth To EndMonth
            y = Int(i / 12)
            m = (i - y * 12) + 1
            d = Date.DaysInMonth(y, m)
            If d < MinDaysInMonth Then MinDaysInMonth = d
        Next
    End Function

    Public Function GetID() As Integer
        GetID = 0
        GetID = (YY1 Mod 2000) * 1000000
        GetID = GetID + MM1 * 10000
        GetID = GetID + (YY2 Mod 2000) * 100
        GetID = GetID + MM2
    End Function

    Public Function SortRanking() As Double
        SortRanking = (YY2 Mod 2000) * 1000000
        SortRanking = SortRanking + MM2 * 10000
        SortRanking = SortRanking - (YY1 Mod 2000) * 100
        SortRanking = SortRanking - MM1
    End Function

    Public Sub FillFromID(ByVal ID As Integer)
        YY1 = Int(ID / 1000000)
        MM1 = Int((ID Mod 1000000) / 10000)
        YY2 = Int((ID Mod 10000) / 100)
        MM2 = Int(ID Mod 100)
        StartMonth = YY1 * 12 + MM1 - 1
        EndMonth = YY2 * 12 + MM2 - 1
    End Sub

    Public Function AddToList(ByVal PeriodList As List(Of ArtBTimePeriod)) As Boolean
        AddToList = False
        For Each p As ArtBTimePeriod In PeriodList
            If p.StartMonth = StartMonth And p.EndMonth = EndMonth Then Exit Function
        Next
        PeriodList.Add(Me)
        AddToList = True
    End Function

    Public Sub GetFromRPStr(ByVal RPStr As String)
        Dim ID As Integer
        ID = Str2Int(RPStr.Substring(5, 8))
        FillFromID(ID)
    End Sub

    Public Function Contains(ByRef tp As ArtBTimePeriod) As Boolean
        If tp.StartMonth >= StartMonth And tp.EndMonth <= EndMonth Then Return True
        Return False
    End Function

    Public Function OverlapMonths(ByRef tp As ArtBTimePeriod) As Integer
        Dim MaxSM As Integer = Max(tp.StartMonth, StartMonth)
        Dim MinEM As Integer = Min(tp.EndMonth, EndMonth)
        If MinEM < MaxSM Then Return 0
        Return MinEM - StartMonth + 1
    End Function

End Class
