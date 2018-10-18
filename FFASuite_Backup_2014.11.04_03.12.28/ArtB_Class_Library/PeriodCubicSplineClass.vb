Public Class PeriodCubicSplineClass
    Public ROUTE_FIXINGS As Collection

    Public Sub New(ByRef GVC As GlobalViewClass, _
                   ByRef gdb As DB_ARTB_NETDataContext, _
                   ByVal a_ROUTE_ID As Integer, _
                   Optional ByRef OrderClass As Object = Nothing, _
                   Optional ByRef OrderCollection As Object = Nothing)

        ROUTE_FIXINGS = New Collection
        Dim qr1 As Object
        If IsNothing(gdb) Or GVC.OperationMode = GVCOpMode.Client Then
            qr1 = From q In GVC.BALTIC_FORWARD_RATES _
                      Where q.ROUTE_ID = a_ROUTE_ID _
                      Order By q.YY2, q.MM2, q.YY1, q.MM1 Descending _
                      Select q
        Else
            qr1 = From q In gdb.BALTIC_FORWARD_RATES_VIEWs _
                      Where q.ROUTE_ID = a_ROUTE_ID _
                      Order By q.YY2, q.MM2, q.YY1, q.MM1 Descending _
                      Select q
        End If

        For Each r In qr1
            Dim nr As BalticFTPClass = New BalticFTPClass
            nr.ROUTE_ID = r.ROUTE_ID
            nr.FIXING_DATE = r.FIXING_DATE
            nr.FIXING = r.FIXING
            nr.PERIOD_MONTHS = DateAndTime.DateDiff(DateInterval.Month, _
                                             DateSerial(r.YY1, r.MM1, 1), _
                                             DateSerial(r.YY2, r.MM2, Date.DaysInMonth(r.YY2, r.MM2))) + 1
            If Not IsNothing(OrderClass) Then
                Dim noc As New ORDERS_FFA_CLASS
                noc.GetFromObject(OrderClass)
                noc.YY1 = r.YY1
                noc.MM1 = r.MM1
                noc.YY2 = r.YY2
                noc.MM2 = r.MM2
                noc.ROUTE_ID = r.ROUTE_ID
                noc.ORDER_TYPE = OrderTypes.FFA
                noc.YY21 = r.YY1
                noc.MM21 = r.MM1
                noc.YY22 = r.YY2
                noc.MM22 = r.MM2
                noc.ROUTE_ID2 = r.ROUTE_ID
                noc.SPREAD_ORDER_ID = Nothing
                nr.FIXING = GVC.GetAvgReferencePrice(gdb, noc, OrderCollection, , False)
                noc = Nothing
            End If

            nr.YY1 = r.YY1
            nr.MM1 = r.MM1
            nr.YY2 = r.YY2
            nr.MM2 = r.MM2
            nr.PERIOD = DateAndTime.DateDiff(DateInterval.Month, _
                                             Date.UtcNow, _
                                             DateSerial(r.YY2, r.MM2, Date.DaysInMonth(r.YY2, r.MM2)))
            nr.KEY = Format(r.YY2, "0000") & Format(r.MM2, "00") & Format(r.YY1, "0000") & Format(r.MM1, "00")
            If ROUTE_FIXINGS.Contains(nr.KEY) = False Then
                ROUTE_FIXINGS.Add(nr, nr.KEY)
            End If
        Next

        'normalize data, covert from forward-forward to preriod t/c
        Dim qr2 = From q As BalticFTPClass In ROUTE_FIXINGS _
                  Order By q.YY2 Descending, q.MM2 Descending, q.YY1 Descending, q.MM1 Descending _
                  Select q
        For Each mr In qr2
            Dim tc As Double = 0
            Dim sd As Date = DateSerial(mr.YY1, mr.MM1, 1)
            Dim ed As Date = DateSerial(mr.YY2, mr.MM2, Date.DaysInMonth(mr.YY2, mr.MM2))
            Dim nm As Integer = DateAndTime.DateDiff(DateInterval.Month, sd, ed) + 1
            Dim monthsrem As Integer = DateAndTime.DateDiff(DateInterval.Month, Date.UtcNow, sd)

            tc = tc + mr.FIXING * nm

            Dim cntr As Integer = 0
            While monthsrem > 0 And cntr < 2000
                Dim qr3 = (From q As BalticFTPClass In ROUTE_FIXINGS _
                           Where DateAndTime.DateDiff(DateInterval.Month, DateSerial(q.YY2, q.MM2, 1), sd) = 1 _
                           Order By q.YY1, q.MM1 _
                           Select q).FirstOrDefault
                If Not IsNothing(qr3) Then
                    sd = DateSerial(qr3.YY1, qr3.MM1, 1)
                    ed = DateSerial(qr3.YY2, qr3.MM2, Date.DaysInMonth(qr3.YY2, qr3.MM2))
                    nm = DateAndTime.DateDiff(DateInterval.Month, sd, ed) + 1
                    tc = tc + qr3.FIXING * nm
                    monthsrem = DateAndTime.DateDiff(DateInterval.Month, Date.UtcNow, sd)
                End If
                cntr = cntr + 1
            End While
            mr.NORMFIX = tc / (mr.PERIOD + 1)
        Next

        Dim tempc As New Collection
        For Each r As BalticFTPClass In ROUTE_FIXINGS
            tempc.Add(r, r.KEY)
        Next

        For I = 1 To tempc.Count - 1
            Dim c1 As BalticFTPClass = tempc.Item(I)
            Dim c2 As BalticFTPClass = tempc.Item(I + 1)
            If c1.PERIOD = c2.PERIOD Then
                If c1.PERIOD_MONTHS > c2.PERIOD_MONTHS Then
                    ROUTE_FIXINGS.Remove(c1.KEY)
                Else
                    ROUTE_FIXINGS.Remove(c2.KEY)
                End If
            End If
        Next

    End Sub

    Public Function PerformSpline(ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As Double

        Dim n As Integer = ROUTE_FIXINGS.Count - 1
        Dim f(n) As Double
        Dim x(n) As Double
        Dim a(n) As Double
        Dim l(n) As Double
        Dim m(n) As Double
        Dim z(n) As Double
        Dim c(n) As Double
        Dim b(n) As Double
        Dim d(n) As Double

        Dim i As Integer = 0
        Dim j As Integer = 0

        For Each r As PeriodCubicSplineClass.BalticFTPClass In ROUTE_FIXINGS
            x(i) = r.PERIOD
            f(i) = r.NORMFIX
            i = i + 1
        Next

        For i = 1 To n - 1
            a(i) = f(i + 1) * (x(i) - x(i - 1))
            a(i) = a(i) - f(i) * (x(i + 1) - x(i - 1))
            a(i) = a(i) + f(i - 1) * (x(i + 1) - x(i))
            a(i) = 3 * a(i)
            a(i) = a(i) / ((x(i + 1) - x(i)) * (x(i) - x(i - 1)))
        Next

        l(0) = 1
        m(0) = 0
        z(0) = 0

        For i = 1 To n - 1
            l(i) = 2 * (x(i + 1) - x(i - 1))
            l(i) = l(i) - (x(i) - x(i - 1)) * m(i - 1)

            m(i) = (x(i + 1) - x(i)) / l(i)
            z(i) = (a(i) - (x(i) - x(i - 1)) * z(i - 1)) / l(i)
        Next

        l(n) = 1
        z(n) = 0
        c(n) = z(n)

        For j = n - 1 To 0 Step -1
            c(j) = z(j) - m(j) * c(j + 1)

            b(j) = (f(j + 1) - f(j)) / (x(j + 1) - x(j))
            b(j) = b(j) - (x(j + 1) - x(j)) * (c(j + 1) + 2 * c(j)) / 3

            d(j) = (c(j + 1) - c(j)) / (3 * (x(j + 1) - x(j)))
        Next
        'finished cubic spline interpolation

        Dim StartPeriod As Integer = DateAndTime.DateDiff(DateInterval.Month, Date.UtcNow, DateSerial(YY1, MM1, 1)) - 1
        Dim EndPeriod As Integer = DateAndTime.DateDiff(DateInterval.Month, Date.UtcNow, DateSerial(YY2, MM2, 1))
        Dim SSP As Double
        Dim SEP As Double

        'for Start period
        If StartPeriod = x(n) Then
            SSP = f(n)
        ElseIf StartPeriod < x(n) Then
            For i = 0 To n - 1
                If StartPeriod >= x(i) And StartPeriod < x(i + 1) Then
                    SSP = f(i) + b(i) * (StartPeriod - x(i)) + c(i) * (StartPeriod - x(i)) ^ 2 + d(i) * (StartPeriod - x(i)) ^ 3
                    Exit For
                End If
            Next
        End If

        'for end period
        If EndPeriod = x(n) Then
            SEP = f(n)
        ElseIf EndPeriod < x(n) Then
            For i = 0 To n - 1
                If EndPeriod >= x(i) And EndPeriod < x(i + 1) Then
                    SEP = f(i) + b(i) * (EndPeriod - x(i)) + c(i) * (EndPeriod - x(i)) ^ 2 + d(i) * (EndPeriod - x(i)) ^ 3
                    Exit For
                End If
            Next
        End If

        PerformSpline = (SEP * (EndPeriod + 1) - SSP * (StartPeriod + 1)) / (EndPeriod - StartPeriod)

    End Function

    Public Class BalticFTPClass
        Public ROUTE_ID As Integer
        Public FIXING_DATE As Date
        Public FIXING As Double
        Public NORMFIX As Double
        Public YY1 As Integer
        Public YY2 As Integer
        Public MM1 As Integer
        Public MM2 As Integer
        Public KEY As String
        Public PERIOD As Integer
        Public PERIOD_MONTHS As Integer
    End Class


End Class
