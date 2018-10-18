Public Class GVC_Class
    Public Function PopulatePeriods(ByVal RouteId As Integer, _
                                    ByVal ExchangeIds As List(Of Integer), _
                                    ByVal d As DateTime, _
                                    ByVal FrontMaxMonths As Integer, _
                                    ByVal HalfDays As Boolean) As List(Of DataContracts.ArtBTimePeriod)

        Dim DB As New ArtBDataContext
        PopulatePeriods = New List(Of DataContracts.ArtBTimePeriod)
        PopulatePeriods.Clear()
        Dim StartMonth As Integer = d.Month
        Dim StartYear As Integer = d.Year Mod 2000
        Dim StartDay As Integer = d.Day
        Dim CurrentMonth = StartMonth
        Dim CurrentYear = StartYear
        Dim i As Integer
        HalfDays = True

        Dim FrontYears As Integer = 1000
        Dim FrontMonths As Integer = 1000
        Dim FrontHalfYears As Integer = 1000
        Dim FrontQuarters As Integer = 1000
        Dim bMC01 As Boolean = True
        Dim bMC12 As Boolean = True
        Dim bMC012 As Boolean = True
        Dim bQC01 As Boolean = True
        FrontMaxMonths = 99
        Dim rt = (From q In DB.ROUTES Where q.ROUTE_ID = RouteId Select q).FirstOrDefault
        If rt Is Nothing Then Exit Function
        Dim GSetDay As Integer = DateTime.DaysInMonth(StartYear, StartMonth)

        Dim ldr = (From q In DB.LAST_DAY_RULE Where q.LAST_DAY_RULE_ID = rt.LAST_DAY_RULE_ID Select q).FirstOrDefault
        If ldr Is Nothing Then Exit Function

        Dim ldrm = (From q In DB.LAST_DAY_RULE_MONTHS Where q.LAST_DAY_RULE_ID = ldr.LAST_DAY_RULE_ID _
                    And q.MONTH = StartMonth Select q).FirstOrDefault
        If ldrm Is Nothing Then Exit Function

        If ldrm.SETTLEMENT_DAY > 0 Then
            If GSetDay > ldrm.SETTLEMENT_DAY Then GSetDay = ldrm.SETTLEMENT_DAY
        End If

        Dim SD As New Date(StartYear + 2000, StartMonth, GSetDay, 0, 0, 0)

        Dim exh As HOLIDAYS
        Dim bCheck As Boolean = True

        While (bCheck = True)
            If SD.DayOfWeek() = DayOfWeek.Saturday Or SD.DayOfWeek() = DayOfWeek.Sunday Then
                SD = SD.AddDays(-1)
            Else
                exh = (From q In DB.HOLIDAYS Where q.HOLIDAY = SD Select q).FirstOrDefault
                If exh Is Nothing Then
                    bCheck = False
                Else
                    SD = SD.AddDays(-1)
                End If
            End If
        End While

        If GSetDay = 0 Then
            GSetDay = SD.Day
        Else
            If GSetDay > SD.Day Then GSetDay = SD.Day
        End If
        Dim pc As EXCHANGE_ROUTE_PERIODS
        For Each ExchangeId As Integer In ExchangeIds
            Dim ex = (From q In DB.EXCHANGES Where q.EXCHANGE_ID = ExchangeId Select q).FirstOrDefault
            If ex Is Nothing Then Exit Function

            If ex.HALF_DAYS = False Then HalfDays = False
            Dim ert = (From q In DB.EXCHANGE_ROUTES Where q.EXCHANGE_ID = ExchangeId _
                       And q.EXCHANGE_ROUTES_ID = RouteId Select q).FirstOrDefault
            If ert Is Nothing Then Continue For

            pc = (From q In DB.EXCHANGE_ROUTE_PERIODS _
                  Where q.EXCHANGE_ROUTE_PERIOD_ID = ert.EXCHANGE_ROUTE_PERIOD_ID _
                  Select q).FirstOrDefault
            If pc Is Nothing Then Continue For

            If pc.FRONT_YEARS < FrontYears Then FrontYears = pc.FRONT_YEARS
            If pc.FRONT_MONTHS < FrontMonths Then FrontMonths = pc.FRONT_MONTHS
            If pc.FRONT_HALF_YEARS < FrontHalfYears Then FrontHalfYears = pc.FRONT_HALF_YEARS
            If pc.FRONT_QUARTERS < FrontQuarters Then FrontQuarters = pc.FRONT_QUARTERS
            If pc.FRONT_MAX_MONTHS < FrontMaxMonths Then FrontMaxMonths = pc.FRONT_MAX_MONTHS
            bMC01 = bMC01 And pc.MC_0_1
            bMC12 = bMC12 And pc.MC_1_2
            bMC012 = bMC012 And pc.MC_0_1_2
            bQC01 = bQC01 And pc.QC_0_1
        Next

        Dim bStartFromNextMonth As Boolean = False
        If StartDay >= GSetDay Then bStartFromNextMonth = True

        If bStartFromNextMonth = True Then
            CurrentMonth = StartMonth + 1
            If CurrentMonth > 12 Then
                CurrentMonth = 1
                CurrentYear = StartYear + 1
            End If
        End If

        Dim SMonth As Integer = CurrentYear * 12 + CurrentMonth - 1
        Dim nextQ As Integer = Int((SMonth - 1) / 3) + 1
        Dim nMonths As Integer = FrontMonths

        ' Months
        For i = 0 To nMonths - 1
            Dim tp As New DataContracts.ArtBTimePeriod
            tp.StartMonth = SMonth + i
            tp.EndMonth = SMonth + i
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        Next

        If bMC01 Then
            Dim tp As New DataContracts.ArtBTimePeriod
            tp.StartMonth = SMonth
            tp.EndMonth = SMonth + 1
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        End If

        If bMC12 Then
            Dim tp As New DataContracts.ArtBTimePeriod
            tp.StartMonth = SMonth + 1
            tp.EndMonth = SMonth + 2
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        End If

        If bMC012 And SMonth Mod 3 <> 0 Then
            Dim tp As New DataContracts.ArtBTimePeriod
            tp.StartMonth = SMonth
            tp.EndMonth = SMonth + 2
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        End If

        Dim nextCal As Integer = CurrentYear + 1
        'Dim nQs As Integer = nextCal * 4 - nextQ
        'If nQs < FrontQuarters Then nQs = FrontQuarters and
        'If nQs > FrontQuarters + 1 Then nQs = FrontQuarters + 1
        Dim nQs As Integer = FrontQuarters
        For i = 0 To nQs - 1
            Dim tp As New DataContracts.ArtBTimePeriod
            tp.StartMonth = (nextQ + i) * 3
            tp.EndMonth = tp.StartMonth + 2
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        Next

        Dim nextHY As Integer = Int((SMonth - 1) / 6) + 1
        Dim nHYs As Integer = FrontHalfYears
        For i = 0 To nHYs - 1
            Dim tp As New DataContracts.ArtBTimePeriod
            tp.StartMonth = (nextHY + i) * 6
            tp.EndMonth = tp.StartMonth + 5
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        Next

        If bQC01 Then
            If nextQ Mod 2 = 0 Then
                Dim tp As New DataContracts.ArtBTimePeriod
                tp.StartMonth = (nextQ + 1) * 3
                tp.EndMonth = tp.StartMonth + 5
                tp.CreateDescr()
                If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
            Else
                Dim tp As New DataContracts.ArtBTimePeriod
                tp.StartMonth = (nextQ) * 3
                tp.EndMonth = tp.StartMonth + 5
                tp.CreateDescr()
                If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
            End If
        End If
        Dim nCals As Integer = FrontYears
        'If nCals > 5 Then nCals = 5
        If CurrentMonth <= 1 Then
            nextCal = CurrentYear
            nCals = nCals + 1
        End If
        For i = 0 To nCals - 1
            Dim tp As New DataContracts.ArtBTimePeriod
            tp.StartMonth = (nextCal + i) * 12
            tp.EndMonth = tp.StartMonth + 11
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        Next

        For Each r In PopulatePeriods
            r.ROUTE_ID = RouteId
            r.SERVER_DATE = d
            r.YY1 += 2000
            r.YY2 += 2000
        Next
    End Function
   
End Class
