Public Class DayTrades

    Private Sub DayTrades_Load(sender As Object, e As EventArgs) Handles Me.Load
        rad_DATEPICKER.Value = Today
        rbtn_GetTrades.PerformClick()
    End Sub

    Private Sub rbtn_GetTrades_Click(sender As Object, e As EventArgs) Handles rbtn_GetTrades.Click
        Dim s As String = String.Empty
        Dim data As New List(Of FFAOptCalcService.VolDataClass)
        Try
            data.AddRange(WEB.SDB.GetDailyTrades(rad_DATEPICKER.Value))
        Catch ex As Exception
            MsgError(Me, "Data request failed", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
            Exit Sub
        End Try

        s = "==============================" & Environment.NewLine
        s += "Trades for: " & FormatDateTime(rad_DATEPICKER.Value, DateFormat.LongDate) & Environment.NewLine
        s += "==============================" & Environment.NewLine
        For Each r In (From q In data Where q.PNC = False Order By q.ROUTE_ID Select q.ROUTE_ID).Distinct
            Dim route = (From q In ROUTES Where q.ROUTE_ID = r Select q).FirstOrDefault
            s += route.ROUTE_SHORT & Environment.NewLine
            'Dim query = From q In data Where q.ROUTE_ID = r _
            'And q.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA _
            'Order By q.YY1, q.MM2, q.YY2, q.MM1 Descending, q.FIXING_DATE _
            'Group q By q.YY1, q.MM2, q.YY2, q.MM1 Into g = Group _
            'Select New With {YY1, MM1, YY2, MM2, .Trade = g}

            Dim query = From q In data Where q.ROUTE_ID = r _
                        And q.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA _
                        And q.PNC = False _
                        Order By q.YY2, q.MM2, q.YY1 Descending, q.MM1 Descending, q.FIXING_DATE _
                        Group q By q.YY2, q.MM2, q.YY1, q.MM1 Into g = Group _
                        Select New With {YY1, MM1, YY2, MM2, .Trade = g}

            For Each g In query
                Dim Period = New ArtBTimePeriod(g.YY1, g.MM1, g.YY2, g.MM2).Descr
                s += Period & ": "
                Dim ft As Boolean = True
                For Each trade In g.Trade
                    If ft = True Then
                        s += Format(trade.FFA_PRICE, FormatPrcTick(route.PRICING_TICK))
                        ft = False
                    Else
                        s += ", " & Format(trade.FFA_PRICE, FormatPrcTick(route.PRICING_TICK))
                    End If
                Next
                s += Environment.NewLine
            Next
            s += Environment.NewLine
        Next
        rtb_Trades.Text += s
    End Sub

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        rtb_Trades.Text = ""
    End Sub

    Private Function FormatPrcTick(ByVal m_PRICING_TICK As Double) As String
        Dim fs As String = String.Empty
        If Math.Log10(m_PRICING_TICK) >= 0 Then
            fs = "##0;##0"
        ElseIf Math.Log10(m_PRICING_TICK) = -1 Then
            fs = "##0.0;##0.0"
        ElseIf Math.Log10(m_PRICING_TICK) = -2 Then
            fs = "##0.00;##0.00"
        ElseIf Math.Log10(m_PRICING_TICK) = -3 Then
            fs = "##0.000;##0.000"
        ElseIf Math.Log10(m_PRICING_TICK) > -2 And Math.Log10(m_PRICING_TICK) < -1 Then
            fs = "##0.00;##0.00"
        ElseIf Math.Log10(m_PRICING_TICK) > -1 And Math.Log10(m_PRICING_TICK) < 0 Then
            fs = "##0.00;##0.00"
        End If
        Return fs
    End Function
End Class
