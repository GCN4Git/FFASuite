Public Class ArtBManageClasses
    Public Class FFAPERIODS
        Public MM1 As Integer
        Public MM2 As Integer
        Public YY1 As Integer
        Public YY2 As Integer
        Public PERIOD As String
        Public YY As String
    End Class


    Public Class BalticFTPConstructor
        Inherits FFAPERIODS

        Public CMSRouteId As String
        Public BDate As Date
        Public NextRolloverDate As Date
        Public ReportDesc As String

        Private Function ShortMonthName(ByVal mm As Integer) As String
            ShortMonthName = Choose(mm, "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC")
        End Function
        Private Function IntMonthName(ByVal MM As String) As Integer
            Select Case UCase(MM)
                Case "JAN"
                    IntMonthName = 1
                Case "FEB"
                    IntMonthName = 2
                Case "MAR"
                    IntMonthName = 3
                Case "APR"
                    IntMonthName = 4
                Case "MAY"
                    IntMonthName = 5
                Case "JUN"
                    IntMonthName = 6
                Case "JUL"
                    IntMonthName = 7
                Case "AUG"
                    IntMonthName = 8
                Case "SEP"
                    IntMonthName = 9
                Case "OCT"
                    IntMonthName = 10
                Case "NOV"
                    IntMonthName = 11
                Case "DEC"
                    IntMonthName = 12
                Case Else
                    IntMonthName = 0
            End Select
        End Function

        Public Sub ConstructRecord()
            Dim BDate2 As Date
            Dim t_MONTH As String
            Dim t_YEAR As String

            If IntMonthName(ReportDesc.Substring(0, 3)) > 0 And CMSRouteId.Contains("QNR") Then
                t_MONTH = ReportDesc.Substring(0, 3)
                t_YEAR = ReportDesc.Substring(5, 2)
                Select Case IntMonthName(t_MONTH)
                    Case 1, 2, 3
                        BDate = DateSerial(t_YEAR + 2000, 1, 1)
                        BDate2 = BDate.AddMonths(2)
                        PERIOD = "Q1"
                        YY = BDate.Year
                    Case 4, 5, 6
                        BDate = DateSerial(t_YEAR + 2000, 4, 1)
                        BDate2 = BDate.AddMonths(2)
                        PERIOD = "Q2"
                        YY = BDate.Year
                    Case 7, 8, 9
                        BDate = DateSerial(t_YEAR + 2000, 7, 1)
                        BDate2 = BDate.AddMonths(2)
                        PERIOD = "Q3"
                        YY = BDate.Year
                    Case 10, 11, 12
                        BDate = DateSerial(t_YEAR + 2000, 10, 1)
                        BDate2 = BDate.AddMonths(2)
                        PERIOD = "Q4"
                        YY = BDate.Year
                End Select
            ElseIf IntMonthName(ReportDesc.Substring(0, 3)) > 0 And (CMSRouteId.Contains("MON") Or CMSRouteId.Contains("_M")) Then
                t_MONTH = ReportDesc.Substring(0, 3)
                t_YEAR = ReportDesc.Substring(5, 2)
                BDate = DateSerial(t_YEAR + 2000, IntMonthName(t_MONTH), 1)
                BDate2 = BDate.AddMonths(0)
                PERIOD = ShortMonthName(IntMonthName(t_MONTH))
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("CURQ") And ReportDesc.Contains("Q") = False Then
                Select Case ReportDesc.Length
                    Case 8 'single month
                        t_YEAR = ReportDesc.Substring(5, 2)
                        t_MONTH = ReportDesc.Substring(0, 3)
                        BDate = DateSerial(t_YEAR + 2000, IntMonthName(t_MONTH), 1)
                        BDate2 = BDate.AddMonths(0)
                        PERIOD = ShortMonthName(IntMonthName(t_MONTH))
                        YY = BDate.Year
                    Case 12 'two months
                        t_YEAR = ReportDesc.Substring(9, 2)
                        t_MONTH = ReportDesc.Substring(0, 3)
                        BDate = DateSerial(t_YEAR + 2000, IntMonthName(t_MONTH), 1)
                        BDate2 = BDate.AddMonths(1)
                        PERIOD = ShortMonthName(IntMonthName(t_MONTH)) & "+" & ShortMonthName(IntMonthName(t_MONTH) + 1)
                        YY = BDate.Year
                End Select
            ElseIf ReportDesc.Contains("Q") Then
                t_YEAR = ReportDesc.Substring(4, 2)
                Select Case ReportDesc.Substring(0, 2)
                    Case "Q1"
                        BDate = DateSerial(t_YEAR + 2000, 1, 1)
                        BDate2 = BDate.AddMonths(2)
                        PERIOD = "Q1"
                        YY = BDate.Year
                    Case "Q2"
                        BDate = DateSerial(t_YEAR + 2000, 4, 1)
                        BDate2 = BDate.AddMonths(2)
                        PERIOD = "Q2"
                        YY = BDate.Year
                    Case "Q3"
                        BDate = DateSerial(t_YEAR + 2000, 7, 1)
                        BDate2 = BDate.AddMonths(2)
                        PERIOD = "Q3"
                        YY = BDate.Year
                    Case "Q4"
                        BDate = DateSerial(t_YEAR + 2000, 10, 1)
                        BDate2 = BDate.AddMonths(2)
                        PERIOD = "Q4"
                        YY = BDate.Year
                End Select
            ElseIf ReportDesc.Contains("Cal") Then
                t_YEAR = ReportDesc.Substring(4, 2)
                BDate = DateSerial(t_YEAR + 2000, 1, 1)
                BDate2 = DateSerial(t_YEAR + 2000, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            End If

            MM1 = BDate.Month
            YY1 = BDate.Year
            MM2 = BDate2.Month
            YY2 = BDate2.Year

        End Sub
        Public Sub getdata()

            Dim BDate2 As Date
            Dim t_MONTH As String
            Dim t_YEAR As String

            If BDate.Month <= NextRolloverDate.Month And BDate.Year = NextRolloverDate.Year Then
                BDate = BDate.AddMonths(0)
            Else
                BDate = BDate.AddMonths(1)
            End If

            If CMSRouteId.Contains("CURMON") Then
                BDate = BDate
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+1MON") Or CMSRouteId.Contains("+1_M") Then
                BDate = BDate.AddMonths(1)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+2MON") Or CMSRouteId.Contains("+2_M") Then
                BDate = BDate.AddMonths(2)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+3MON") Or CMSRouteId.Contains("+3_M") Then
                BDate = BDate.AddMonths(3)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+4MON") Or CMSRouteId.Contains("+4_M") Then
                BDate = BDate.AddMonths(4)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+5MON") Or CMSRouteId.Contains("+5_M") Then
                BDate = BDate.AddMonths(5)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+6MON") Or CMSRouteId.Contains("+6_M") Then
                BDate = BDate.AddMonths(6)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+7MON") Or CMSRouteId.Contains("+7_M") Then
                BDate = BDate.AddMonths(7)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+8MON") Or CMSRouteId.Contains("+8_M") Then
                BDate = BDate.AddMonths(8)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+9MON") Or CMSRouteId.Contains("+9_M") Then
                BDate = BDate.AddMonths(9)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+10MON") Or CMSRouteId.Contains("+10_M") Then
                BDate = BDate.AddMonths(10)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+11MON") Or CMSRouteId.Contains("+11_M") Then
                BDate = BDate.AddMonths(11)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+12MON") Or CMSRouteId.Contains("+12_M") Then
                BDate = BDate.AddMonths(12)
                BDate2 = BDate
                PERIOD = ShortMonthName(BDate.Month)
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("_1M1QNR") Then
                'this is bs, so work around it. It refers to the first month of the first not reported quarter, so in effect its whole quarter data
                t_MONTH = ReportDesc.Substring(0, 3)
                t_YEAR = ReportDesc.Substring(5, 2)
                BDate = DateSerial(t_YEAR + 2000, IntMonthName(t_MONTH), 1)
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1
                        PERIOD = "Q1"
                    Case 4
                        PERIOD = "Q2"
                    Case 7
                        PERIOD = "Q3"
                    Case 10
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("_1M2QNR") Then
                'this is bs, so work around it. It refers to the first month of the second not reported quarter, so in effect its whole quarter data
                t_MONTH = ReportDesc.Substring(0, 3)
                t_YEAR = ReportDesc.Substring(5, 2)
                BDate = DateSerial(t_YEAR + 2000, IntMonthName(t_MONTH), 1)
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1
                        PERIOD = "Q1"
                    Case 4
                        PERIOD = "Q2"
                    Case 7
                        PERIOD = "Q3"
                    Case 10
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("_1M3QNR") Then
                'this is bs, so work around it. It refers to the first month of the third not reported quarter, so in effect its whole quarter data
                t_MONTH = ReportDesc.Substring(0, 3)
                t_YEAR = ReportDesc.Substring(5, 2)
                BDate = DateSerial(t_YEAR + 2000, IntMonthName(t_MONTH), 1)
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1
                        PERIOD = "Q1"
                    Case 4
                        PERIOD = "Q2"
                    Case 7
                        PERIOD = "Q3"
                    Case 10
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("_1M4QNR") Then
                'this is bs, so work around it. It refers to the first month of the fourth not reported quarter, so in effect its whole quarter data
                t_MONTH = ReportDesc.Substring(0, 3)
                t_YEAR = ReportDesc.Substring(5, 2)
                BDate = DateSerial(t_YEAR + 2000, IntMonthName(t_MONTH), 1)
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1
                        PERIOD = "Q1"
                    Case 4
                        PERIOD = "Q2"
                    Case 7
                        PERIOD = "Q3"
                    Case 10
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("CURQ") Then
                BDate = BDate.AddMonths(0)
                Select Case BDate.Month
                    Case 1, 4, 7, 10
                        BDate2 = BDate.AddMonths(2)
                        Select Case BDate.Month
                            Case 1
                                PERIOD = "Q1"
                            Case 4
                                PERIOD = "Q2"
                            Case 7
                                PERIOD = "Q3"
                            Case 10
                                PERIOD = "Q4"
                        End Select
                    Case 2, 5, 8, 11
                        BDate2 = BDate.AddMonths(1)
                        PERIOD = ShortMonthName(BDate.Month) & "+" & ShortMonthName(BDate2.Month)
                        YY = BDate.Year
                    Case 3, 6, 9, 12
                        BDate2 = BDate.AddMonths(0)
                        PERIOD = ShortMonthName(BDate2.Month)
                        YY = BDate.Year
                End Select
            ElseIf CMSRouteId.Contains("+1Q") Then
                Select Case BDate.Month
                    Case 1, 4, 7, 10
                        BDate = BDate.AddMonths(3)
                    Case 2, 5, 8, 11
                        BDate = BDate.AddMonths(2)
                    Case 3, 6, 9, 12
                        BDate = BDate.AddMonths(1)
                End Select
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1, 2, 3
                        PERIOD = "Q1"
                    Case 4, 5, 6
                        PERIOD = "Q2"
                    Case 7, 8, 9
                        PERIOD = "Q3"
                    Case 10, 11, 12
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+2Q") Then
                Select Case BDate.Month
                    Case 1, 4, 7, 10
                        BDate = BDate.AddMonths(6)
                    Case 2, 5, 8, 11
                        BDate = BDate.AddMonths(5)
                    Case 3, 6, 9, 12
                        BDate = BDate.AddMonths(4)
                End Select
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1, 2, 3
                        PERIOD = "Q1"
                    Case 4, 5, 6
                        PERIOD = "Q2"
                    Case 7, 8, 9
                        PERIOD = "Q3"
                    Case 10, 11, 12
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+3Q") Then
                Select Case BDate.Month
                    Case 1, 4, 7, 10
                        BDate = BDate.AddMonths(9)
                    Case 2, 5, 8, 11
                        BDate = BDate.AddMonths(8)
                    Case 3, 6, 9, 12
                        BDate = BDate.AddMonths(7)
                End Select
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1, 2, 3
                        PERIOD = "Q1"
                    Case 4, 5, 6
                        PERIOD = "Q2"
                    Case 7, 8, 9
                        PERIOD = "Q3"
                    Case 10, 11, 12
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+4Q") Then
                Select Case BDate.Month
                    Case 1, 4, 7, 10
                        BDate = BDate.AddMonths(12)
                    Case 2, 5, 8, 11
                        BDate = BDate.AddMonths(11)
                    Case 3, 6, 9, 12
                        BDate = BDate.AddMonths(10)
                End Select
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1, 2, 3
                        PERIOD = "Q1"
                    Case 4, 5, 6
                        PERIOD = "Q2"
                    Case 7, 8, 9
                        PERIOD = "Q3"
                    Case 10, 11, 12
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+5Q") Then
                Select Case BDate.Month
                    Case 1, 4, 7, 10
                        BDate = BDate.AddMonths(15)
                    Case 2, 5, 8, 11
                        BDate = BDate.AddMonths(14)
                    Case 3, 6, 9, 12
                        BDate = BDate.AddMonths(13)
                End Select
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1, 2, 3
                        PERIOD = "Q1"
                    Case 4, 5, 6
                        PERIOD = "Q2"
                    Case 7, 8, 9
                        PERIOD = "Q3"
                    Case 10, 11, 12
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+6Q") Then
                Select Case BDate.Month
                    Case 1, 4, 7, 10
                        BDate = BDate.AddMonths(18)
                    Case 2, 5, 8, 11
                        BDate = BDate.AddMonths(17)
                    Case 3, 6, 9, 12
                        BDate = BDate.AddMonths(16)
                End Select
                BDate2 = BDate.AddMonths(2)
                Select Case BDate.Month
                    Case 1, 2, 3
                        PERIOD = "Q1"
                    Case 4, 5, 6
                        PERIOD = "Q2"
                    Case 7, 8, 9
                        PERIOD = "Q3"
                    Case 10, 11, 12
                        PERIOD = "Q4"
                End Select
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+1CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(1)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+2CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(2)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+3CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(3)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+4CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(4)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+5CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(5)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+6CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(6)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+7CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(7)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+8CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(8)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+9CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(9)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            ElseIf CMSRouteId.Contains("+10CAL") Then
                BDate = DateSerial(BDate.Year, 1, 1)
                BDate = BDate.AddYears(10)
                BDate2 = DateSerial(BDate.Year, 12, 31)
                PERIOD = "CAL"
                YY = BDate.Year
            End If

            MM1 = BDate.Month
            YY1 = BDate.Year
            MM2 = BDate2.Month
            YY2 = BDate2.Year

        End Sub

    End Class


End Class
