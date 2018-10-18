Imports System.IO
Imports System.Xml.Serialization
Imports Telerik.WinControls.UI
Public Class FFATimeCharterOptionForm
    Private m_SERVER_DATE As Date
    Private m_ROUTE_ID As Integer
    Private m_GRIDDATA As New List(Of GRIDPeriodsClass)
    Private m_FIXINGS As New List(Of FFAOptCalcService.VolDataClass)
    Private m_INTEREST_RATES As New List(Of FFAOptCalcService.InterestRatesClass)
    Private m_PUBLIC_HOLIDAYS As New List(Of Date)

    Private m_ROUTE_DETAIL As FFAOptCalcService.SwapDataClass
    Private m_FirstTime As Boolean = True
    Private m_IsHistorical As Boolean = False

    Public WithEvents EventForm As FFAOptCalc

    Private m_minDate As DateTime
    Private m_Ymax As Integer
    Private m_maxDate As DateTime
    Private m_NoMonths As Integer
    Private m_TC_START As Integer = 1
    Private m_TC_END As Integer = 12
    Private m_TC_OPTION_END As Integer = 18
    Private l_TC As New List(Of TCMonthsClass)
    Private f_TC_END_FirstTime As Boolean = True
    Private f_TC_OPTION_END_FirstTime As Boolean = True
    Private m_TC_END_Old As New TCMonthsClass
    Private m_TC_OPTION_Old As New TCMonthsClass
    Friend FFAOpt As New ffaOption.FfaOption
    Private m_ListOfOptions As New List(Of FFAOptionSolveClass)

    Private m_FindVolData As New FFAOptCalcService.VolDataClass    
    Private m_FontN As Font
    Private m_FontB As Font


#Region "Properties"
    Public Property ROUTE_ID As Integer
        Get
            Return m_ROUTE_ID
        End Get
        Set(value As Integer)
            m_ROUTE_ID = value
        End Set
    End Property
    Public Property ROUTE_DETAIL As FFAOptCalcService.SwapDataClass
        Get
            Return m_ROUTE_DETAIL
        End Get
        Set(value As FFAOptCalcService.SwapDataClass)
            m_ROUTE_DETAIL = value
        End Set
    End Property
    Public ReadOnly Property GRIDDATA As List(Of GRIDPeriodsClass)
        Get
            Return m_GRIDDATA
        End Get
    End Property
    Public Property INTEREST_RATES As List(Of FFAOptCalcService.InterestRatesClass)
        Get
            Return m_INTEREST_RATES
        End Get
        Set(value As List(Of FFAOptCalcService.InterestRatesClass))
            m_INTEREST_RATES.Clear()
            m_INTEREST_RATES.AddRange(value)
        End Set
    End Property
    Public Property PUBLIC_HOLIDAYS As List(Of Date)
        Get
            Return m_PUBLIC_HOLIDAYS
        End Get
        Set(value As List(Of Date))
            m_PUBLIC_HOLIDAYS = value
        End Set
    End Property
    Public Property FIXINGS As List(Of FFAOptCalcService.VolDataClass)
        Get
            Return m_FIXINGS
        End Get
        Set(value As List(Of FFAOptCalcService.VolDataClass))
            m_FIXINGS = value
        End Set
    End Property
    Public Property IsHistorical As Boolean
        Get
            Return m_IsHistorical
        End Get
        Set(value As Boolean)
            m_IsHistorical = value
        End Set
    End Property
    Public ReadOnly Property ListOfOptions As List(Of FFAOptionSolveClass)
        Get
            Return m_ListOfOptions
        End Get
    End Property
    Public Property SERVER_DATE As Date
        Get
            Return m_SERVER_DATE
        End Get
        Set(value As Date)
            m_SERVER_DATE = value
        End Set
    End Property

#End Region

    Private Sub FFATimeCharterOptionForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Icon = My.Resources.ArtB_Robots__48X48
#If DEBUG Then
        se_OPTION_ADJ_TC_RATE.Visible = True
#Else
        se_OPTION_ADJ_TC_RATE.Visible = False
#End If

        Me.mytooltip.SetToolTip(Me.se_MAIN_FFA_PRICE.SpinElement.TextBoxItem.HostedControl, "This is the FFA price automatically calculated for the selected period." & vbCrLf & "You can overide this to any value of your choice.")
        Me.mytooltip.SetToolTip(Me.se_MAIN_FFA_PRICE.SpinElement.TextBoxItem.HostedControl, "This is the FFA price automatically calculated for the selected period." & vbCrLf & "You can overide this to any value of your choice.")
        Me.mytooltip.SetToolTip(Me.se_MAIN_CHARTER_RATE.SpinElement.TextBoxItem.HostedControl, "If you have a fixed rate proposal for the main TC period enter it here." & vbCrLf & "If you want the model to calculate the rate leave the value equal to zero.")
        Me.mytooltip.SetToolTip(Me.se_OPTION_CHARTER_RATE.SpinElement.TextBoxItem.HostedControl, "If you have a fixed rate proposal for the main TC period enter it here." & vbCrLf & "If you want the model to calculate the rate leave the value equal to zero.")
        Me.mytooltip.SetToolTip(Me.se_MAIN_ADJ_TC_RATE.SpinElement.TextBoxItem.HostedControl, "This box displays the adjusted TC rate, taking into consideration all options values." & vbCrLf & "Use it as a reference to the indicated TC rate.")
        Me.mytooltip.SetToolTip(Me.se_VEP.SpinElement.TextBoxItem.HostedControl, "VEP: Vessel Equivalent Performance Factor")
        Me.mytooltip.SetToolTip(Me.se_SKEW.SpinElement.TextBoxItem.HostedControl, "Options SKEW (%), used to adjust volatility profiles " & vbCrLf & "to better reflect bid/offer prices in the market")

    End Sub


    Public Sub Prepare(ByVal _FIXINGS As List(Of FFAOptCalcService.VolDataClass))
        m_FontN = New Font(rgv_PERIODS.TableElement.Font.Name, rgv_PERIODS.TableElement.Font.Size, FontStyle.Regular)
        m_FontB = New Font(rgv_PERIODS.TableElement.Font.Name, rgv_PERIODS.TableElement.Font.Size, FontStyle.Bold)

        m_FIXINGS.Clear()
        m_GRIDDATA.Clear()
        rgv_BS.DataSource = Nothing

        For Each f In _FIXINGS
            Dim nr As New FFAOptCalcService.VolDataClass
            nr.TRADE_TYPE = f.TRADE_TYPE
            nr.TRADE_ID = f.TRADE_ID
            nr.DESK_TRADER_ID = f.DESK_TRADER_ID
            nr.FFA_PRICE = f.FFA_PRICE
            nr.FIXING_DATE = f.FIXING_DATE
            nr.HVOL = f.HVOL
            nr.INTEREST_RATE = f.INTEREST_RATE
            nr.IVOL = f.IVOL
            nr.ONLYHISTORICAL = f.ONLYHISTORICAL
            nr.MM1 = f.MM1
            nr.MM2 = f.MM2
            nr.YY1 = f.YY1
            nr.YY2 = f.YY2
            nr.PERIOD = f.PERIOD
            nr.ROUTE_ID = f.ROUTE_ID
            nr.SPOT_PRICE = f.SPOT_PRICE
            nr.VolRecordType = f.VolRecordType
            m_FIXINGS.Add(nr)
        Next

        'Add Spot Period
        Dim Spot = (From q In m_FIXINGS _
                    Where q.ROUTE_ID = m_ROUTE_ID And _
                    (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot) _
                    Order By q.FIXING_DATE Descending Select q).FirstOrDefault
        If IsNothing(Spot) Then
            Dim ns As New GRIDPeriodsClass
            ns.FIXING_DATE = m_SERVER_DATE
            ns.FFA_PRICE = 0
            ns.IVOL = 0
            ns.HVOL = 0
            ns.INTEREST_RATE = 0
            ns.ONLYHISTORICAL = True
            ns.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot
            ns.PERIOD = "Spot"
            ns.YY1 = m_SERVER_DATE.Year
            ns.MM1 = m_SERVER_DATE.Month
            ns.YY2 = m_SERVER_DATE.Year
            ns.MM2 = m_SERVER_DATE.Month
            m_GRIDDATA.Add(ns)
        Else
            Dim ns As New GRIDPeriodsClass
            ns.FIXING_DATE = Spot.FIXING_DATE
            ns.FFA_PRICE = Spot.SPOT_PRICE
            ns.IVOL = Spot.HVOL
            ns.HVOL = Spot.HVOL
            ns.INTEREST_RATE = 0
            ns.ONLYHISTORICAL = Spot.ONLYHISTORICAL
            ns.VolRecordType = Spot.VolRecordType
            ns.PERIOD = "Spot"
            ns.YY1 = m_SERVER_DATE.Year
            ns.MM1 = m_SERVER_DATE.Month
            ns.YY2 = m_SERVER_DATE.Year
            ns.MM2 = m_SERVER_DATE.Month
            m_GRIDDATA.Add(ns)
        End If

        'Add Swap Periods
        Dim DistinctPeriods = (From q In m_FIXINGS _
                               Where q.ROUTE_ID = m_ROUTE_ID _
                               And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
                               Order By q.YY2, q.MM2, q.YY1, q.MM1 Descending _
                               Select q.YY1, q.MM1, q.YY2, q.MM2).Distinct
        For Each p In DistinctPeriods
            Dim NoMonths As Integer = DateAndTime.DateDiff(DateInterval.Month, DateSerial(p.YY1, p.MM1, 1), DateSerial(p.YY2, p.MM2, 1)) + 1
            Select Case NoMonths
                Case 2, 6
                    Continue For
            End Select

            Dim nc As New GRIDPeriodsClass
            Dim lastfixdate = (From q In m_FIXINGS _
                               Where q.ROUTE_ID = m_ROUTE_ID And q.YY1 = p.YY1 And q.MM1 = p.MM1 And q.YY2 = p.YY2 And q.MM2 = p.MM2 _
                               And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
                               Select q.FIXING_DATE).Max
            Dim DataRecord = (From q In m_FIXINGS _
                              Where q.ROUTE_ID = m_ROUTE_ID And q.YY1 = p.YY1 And q.MM1 = p.MM1 And q.YY2 = p.YY2 And q.MM2 = p.MM2 _
                              And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
                              And q.FIXING_DATE = lastfixdate _
                              Select q).FirstOrDefault

            'check if live data is present
            Dim liveprc = (From q In m_FIXINGS _
                           Where q.ROUTE_ID = m_ROUTE_ID And q.YY1 = p.YY1 And q.MM1 = p.MM1 And q.YY2 = p.YY2 And q.MM2 = p.MM2 _
                           And q.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA _
                           And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level) _
                           Order By q.FIXING_DATE Descending _
                           Select q).FirstOrDefault
            If IsNothing(liveprc) = False Then
                nc.TRADE_ID = liveprc.TRADE_ID
                nc.FFA_PRICE = liveprc.FFA_PRICE
                nc.LIVE_DATA = True
                nc.VolRecordType = liveprc.VolRecordType
            Else
                nc.FFA_PRICE = DataRecord.FFA_PRICE
                nc.LIVE_DATA = False
                nc.VolRecordType = DataRecord.VolRecordType
            End If

            nc.FIXING_DATE = DataRecord.FIXING_DATE
            nc.IVOL = DataRecord.IVOL
            nc.HVOL = DataRecord.HVOL
            nc.INTEREST_RATE = DataRecord.INTEREST_RATE
            nc.ONLYHISTORICAL = DataRecord.ONLYHISTORICAL
            Dim nperiod As New ArtBTimePeriod(p.YY1, p.MM1, p.YY2, p.MM2)
            nc.PERIOD = nperiod.Descr
            nc.YY1 = DataRecord.YY1
            nc.MM1 = DataRecord.MM1
            nc.YY2 = DataRecord.YY2
            nc.MM2 = DataRecord.MM2
            m_GRIDDATA.Add(nc)
        Next

        rgv_PERIODS.Columns("FFA_PRICE").FormatString = m_ROUTE_DETAIL.FORMAT_STRING
        rgv_BS.DataSource = m_GRIDDATA
        rgv_PERIODS.Refresh()
        rgv_PERIODS.Rows(1).IsCurrent = True
        rgv_PERIODS.Rows(1).IsSelected = True
        'rgv_PERIODS.DataSource = rgv_BS   **** I assigned this at design time

        se_MAIN_FFA_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_OPTION_FFA_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_MAIN_CHARTER_RATE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_OPTION_CHARTER_RATE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_MAIN_PROFIT_SHARE_CAP.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_MAIN_PROFIT_SHARE_STRIKE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_MAIN_ADJ_TC_RATE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_OPTION_ADJ_TC_RATE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_OPTIONS_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES

        m_minDate = New DateTime(m_SERVER_DATE.Year, m_SERVER_DATE.Month, 1)
        m_Ymax = (From q In m_GRIDDATA Select q.YY2).Max
        m_maxDate = New DateTime(m_Ymax, 12, 1)
        m_NoMonths = DateDiff(DateInterval.Month, m_minDate, m_maxDate)

        PrepareGraph()

        l_TC.Clear()
        For I = 0 To m_NoMonths
            Dim td As DateTime = m_SERVER_DATE.AddMonths(I)
            Dim nc As New TCMonthsClass(td.Month, td.Year, m_SERVER_DATE)
            l_TC.Add(nc)
        Next

        cb_TC_START.DisplayMember = "DES"
        cb_TC_START.ValueMember = "YYMM"
        cb_TC_START.DataSource = (From q In l_TC Order By q.YY, q.MM Select q).ToList

        Do Until m_FirstTime = False
            Threading.Thread.Sleep(10)
        Loop

        rdtp_SERVER_DATE.text = FormatDateTime(m_SERVER_DATE, DateFormat.ShortDate)

        If m_IsHistorical = False Then
            cb_TC_START.SelectedIndex = 1
            cb_TC_END.SelectedIndex = 3
            cb_OPTION_END.SelectedIndex = 2
        End If

        se_MAIN_CHARTER_RATE.Focus()
    End Sub
    
    Private Sub PrepareGraph()
        MSChart.Series("FFA").Enabled = False
        MSChart.Series("FFA").Points.Clear()
        MSChart.ChartAreas("Default").AxisY.IsStartedFromZero = False
        MSChart.ChartAreas("Default").AxisX.IsStartedFromZero = False
        MSChart.ChartAreas("Default").AxisX.LabelStyle.Format = "MMM-yy"
        MSChart.DataManipulator.IsStartFromFirst = True
        MSChart.DataManipulator.IsEmptyPointIgnored = True

        MSChart.ChartAreas("Default").AxisX.LabelStyle.IsEndLabelVisible = False

        ' Locale specific percentage format with no decimals
        MSChart.ChartAreas("Default").AxisY.LabelStyle.Format = "N0"

        MSChart.Series("FFA").Color = Color.Red
        MSChart.Series("FFA").BorderWidth = 2

        For I = 0 To m_NoMonths
            Dim tDate As Date = m_SERVER_DATE.AddMonths(I)
            Dim tPrc As Double

            If I = 0 Then
                tPrc = (From q In m_GRIDDATA Where q.PERIOD = "Spot" Select q.FFA_PRICE).FirstOrDefault
                MSChart.Series("FFA").Points.AddXY(tDate, tPrc)
                MSChart.Series("FFA").Points(MSChart.Series("FFA").Points.Count - 1).ToolTip = "Spot: " & FormatDateTime(tDate, DateFormat.GeneralDate) & vbCrLf & FormatPriceTick(m_ROUTE_DETAIL.PRICING_TICK, tPrc)
            Else
                tPrc = SwapFixing(tDate.Year, tDate.Month, tDate.Year, tDate.Month)
                MSChart.Series("FFA").Points.AddXY(tDate, tPrc)
                Dim nc As New TCMonthsClass(tDate.Month, tDate.Year, m_SERVER_DATE)
                MSChart.Series("FFA").Points(MSChart.Series("FFA").Points.Count - 1).ToolTip = "FFA: " & nc.DES & vbCrLf & FormatPriceTick(m_ROUTE_DETAIL.PRICING_TICK, tPrc)
            End If
        Next

        MSChart.Series("FFA").Enabled = True
    End Sub

#Region "PeriodSelectorsSelectedIndexChange"
    Private Sub cb_TC_START_SelectedIndexChanging(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangingCancelEventArgs) Handles cb_TC_START.SelectedIndexChanging
        If cb_TC_START.SelectedIndex < 0 Then Exit Sub
        If cb_TC_END.SelectedIndex < 0 Then Exit Sub

        m_TC_END_Old = DirectCast(cb_TC_END.SelectedItem.DataBoundItem, TCMonthsClass)
    End Sub
    Private Sub cb_TC_START_SelectedValueChanged(sender As Object, e As EventArgs) Handles cb_TC_START.SelectedValueChanged
        If cb_TC_START.SelectedIndex < 0 Then Exit Sub

        Dim t As TCMonthsClass = DirectCast(cb_TC_START.SelectedItem.DataBoundItem, TCMonthsClass)
        f_TC_END_FirstTime = True
        cb_TC_END.DisplayMember = "DES"
        cb_TC_END.ValueMember = "YYMM"
        cb_TC_END.DataSource = (From q In l_TC Where q.NoMonths >= t.NoMonths Order By q.YY, q.MM Select q)

        Dim I As Integer = 0
        If cb_TC_END.SelectedIndex >= 0 Then
            For Each r As TCMonthsClass In cb_TC_END.DataSource
                If r.NoMonths = m_TC_END_Old.NoMonths Then
                    cb_TC_END.SelectedIndex = I
                    Exit For
                End If
                I += 1
            Next
        End If

        UpdateTCDes()
        f_TC_END_FirstTime = False
        cb_TC_END_SelectedValueChanged(Me, e)
    End Sub

    Private Sub cb_TC_END_SelectedIndexChanging(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangingCancelEventArgs) Handles cb_TC_END.SelectedIndexChanging, RadDropDownList1.SelectedIndexChanging
        If cb_TC_START.SelectedIndex < 0 Then Exit Sub
        If cb_TC_END.SelectedIndex < 0 Then Exit Sub
        If cb_OPTION_END.SelectedIndex < 0 Then Exit Sub

        m_TC_OPTION_Old = DirectCast(cb_OPTION_END.SelectedItem.DataBoundItem, TCMonthsClass)
    End Sub
    Private Sub cb_TC_END_SelectedValueChanged(sender As Object, e As EventArgs) Handles cb_TC_END.SelectedValueChanged, RadDropDownList1.SelectedValueChanged
        If cb_TC_START.SelectedIndex < 0 Then Exit Sub
        If cb_TC_END.SelectedIndex < 0 Then Exit Sub
        If f_TC_END_FirstTime Then Exit Sub

        Dim t As TCMonthsClass = DirectCast(cb_TC_END.SelectedItem.DataBoundItem, TCMonthsClass)
        f_TC_OPTION_END_FirstTime = True
        cb_OPTION_END.DisplayMember = "DES"
        cb_OPTION_END.ValueMember = "YYMM"
        cb_OPTION_END.DataSource = (From q In l_TC Where q.NoMonths >= t.NoMonths Order By q.YY, q.MM Select q)

        Dim I As Integer = 0
        If cb_OPTION_END.SelectedIndex >= 0 Then
            For Each r As TCMonthsClass In cb_OPTION_END.DataSource
                If r.NoMonths = m_TC_OPTION_Old.NoMonths Then
                    cb_OPTION_END.SelectedIndex = I
                    Exit For
                End If
                I += 1
            Next
        End If

        UpdateTCDes()
        f_TC_OPTION_END_FirstTime = False
    End Sub
    Private Sub cb_OPTION_END_SelectedValueChanged(sender As Object, e As EventArgs) Handles cb_OPTION_END.SelectedValueChanged
        If cb_TC_START.SelectedIndex < 0 Then Exit Sub
        If cb_TC_END.SelectedIndex < 0 Then Exit Sub
        If cb_OPTION_END.SelectedIndex < 0 Then Exit Sub
        UpdateTCDes()
        m_FirstTime = False
    End Sub

    Private Sub rgv_PERIODS_ViewCellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles rgv_PERIODS.ViewCellFormatting
        If IsNothing(e.CellElement.ColumnInfo) Then Exit Sub
        If e.Column.IsVisible = False Then Exit Sub

        If TypeOf e.CellElement Is Telerik.WinControls.UI.GridDataCellElement Then
            If e.Column.Name = "FFA_PRICE" And e.Row.Cells("VolRecordType").Value = FFAOptCalcService.VolRecordTypeEnum.live Then
                e.CellElement.Font = m_FontB
                e.CellElement.ForeColor = Color.Black
            ElseIf e.Column.Name = "FFA_PRICE" And e.Row.Cells("VolRecordType").Value = FFAOptCalcService.VolRecordTypeEnum.level Then
                e.CellElement.Font = m_FontN
                e.CellElement.ForeColor = Color.Black
            ElseIf e.Column.Name = "FFA_PRICE" And e.Row.Cells("VolRecordType").Value = FFAOptCalcService.VolRecordTypeEnum.nspot Then
                e.CellElement.Font = m_FontB
                e.CellElement.ForeColor = Color.Black
            ElseIf e.Column.Name = "FFA_PRICE" Then
                e.CellElement.Font = m_FontN
                e.CellElement.ForeColor = Color.Black
            ElseIf e.Column.Name = "PERIOD" Then
                e.CellElement.Font = m_FontB
            ElseIf e.Column.Name = "IVOL" And e.Row.Cells("ONLYHISTORICAL").Value = True Then
                e.CellElement.ForeColor = Color.Empty
            ElseIf e.Column.Name = "IVOL" And e.Row.Cells("ONLYHISTORICAL").Value = False Then
                e.CellElement.ForeColor = Color.Black
            Else
                e.CellElement.Font = m_FontN
                e.CellElement.ForeColor = Color.Black
            End If
        End If

        If TypeOf e.CellElement Is Telerik.WinControls.UI.GridHeaderCellElement Then
            e.CellElement.Font = m_FontB
        End If
    End Sub

    Private Sub UpdateTCDes()
        If cb_TC_START.SelectedIndex < 0 Then Exit Sub
        If cb_TC_END.SelectedIndex < 0 Then Exit Sub
        If cb_OPTION_END.SelectedIndex < 0 Then Exit Sub

        Dim s As TCMonthsClass = cb_TC_START.SelectedItem.DataBoundItem
        Dim e As TCMonthsClass = cb_TC_END.SelectedItem.DataBoundItem
        Dim o As TCMonthsClass = cb_OPTION_END.SelectedItem.DataBoundItem

        Dim nperiod As New ArtBTimePeriod(s.YY, s.MM, e.YY, e.MM)
        rtb_TC_DES.Text = nperiod.Descr

        Dim opts As String = " / option for "
        If o.NoMonths > e.NoMonths Then
            Dim noMonths = o.NoMonths - e.NoMonths
            Select Case noMonths
                Case Is <= 23
                    opts += noMonths & " Months"
                Case Is >= 24
                    If Decimal.op_Modulus(noMonths, 12) = 0 Then
                        opts += noMonths / 12 & " years"
                    Else
                        opts += noMonths & " Months"
                    End If
            End Select
            rtb_TC_DES.Text = rtb_TC_DES.Text & opts
        End If

        'update forward values
        se_MAIN_FFA_PRICE.Value = SwapFixing(s.YY, s.MM, e.YY, e.MM)
        If DateSerial(o.YY, o.MM, 1) <= DateSerial(e.YY, e.MM, 1) Then
            se_OPTION_FFA_PRICE.Value = 0
        Else
            Dim tdate As Date = DateSerial(e.YY, e.MM, 1).AddMonths(1)
            se_OPTION_FFA_PRICE.Value = SwapFixing(tdate.Year, tdate.Date.Month, o.YY, o.MM)
        End If
    End Sub
#End Region

#Region "EventsReceived"

    Private Sub EventForm_AmmededTradeReceived(sender As Object, AmmendedTrade As FFAOptCalcService.VolDataClass) Handles EventForm.AmmededTradeReceived
        If AmmendedTrade.ROUTE_ID <> m_ROUTE_ID Then Exit Sub
        If My.Settings.UpdateCalcLive = False Then Exit Sub

        Dim RecFound As Boolean = False
        Dim CurrentIndex As Integer = rgv_PERIODS.CurrentRow.Index
        For Each row In m_GRIDDATA
            If row.YY1 = AmmendedTrade.YY1 And row.YY2 = AmmendedTrade.YY2 And row.MM1 = AmmendedTrade.MM1 And row.MM2 = AmmendedTrade.MM2 Then
                If row.TRADE_ID = AmmendedTrade.TRADE_ID Then
                    row.FFA_PRICE = AmmendedTrade.FFA_PRICE
                    row.LIVE_DATA = True
                    row.VolRecordType = AmmendedTrade.VolRecordType
                    RecFound = True
                    Exit For
                End If
            End If
        Next
        If RecFound Then
            rgv_BS.ResetBindings(False)
            rgv_PERIODS.Refresh()
            rgv_PERIODS.Rows(CurrentIndex).IsCurrent = True
            rgv_PERIODS.Rows(CurrentIndex).IsSelected = True
        End If
    End Sub
    Private Sub EventForm_ReceivedFFALiveTrade(sender As Object, LiveTrade As FFAOptCalcService.VolDataClass) Handles EventForm.ReceivedFFALiveTrade
        If LiveTrade.ROUTE_ID <> m_ROUTE_ID Then Exit Sub
        If My.Settings.UpdateCalcLive = False Then Exit Sub

        Dim RecFound As Boolean = False
        Dim CurrentIndex As Integer = rgv_PERIODS.CurrentRow.Index
        For Each row In m_GRIDDATA
            If row.YY1 = LiveTrade.YY1 And row.YY2 = LiveTrade.YY2 And row.MM1 = LiveTrade.MM1 And row.MM2 = LiveTrade.MM2 Then
                row.FFA_PRICE = LiveTrade.FFA_PRICE
                row.LIVE_DATA = True
                row.VolRecordType = LiveTrade.VolRecordType
                RecFound = True
                Exit For
            End If
        Next
        If RecFound Then
            rgv_BS.ResetBindings(False)
            rgv_PERIODS.Refresh()
            rgv_PERIODS.Rows(CurrentIndex).IsCurrent = True
            rgv_PERIODS.Rows(CurrentIndex).IsSelected = True
        End If
    End Sub
    Private Sub EventForm_SpotRatesUpdated() Handles EventForm.SpotRatesUpdated
        Dim CurrentIndex As Integer = rgv_PERIODS.CurrentRow.Index
        Dim RecFound As Boolean = False

        m_ROUTE_DETAIL = (From q In ROUTES_DETAIL Where q.ROUTE_ID = m_ROUTE_ID Select q).FirstOrDefault

        For Each row In m_GRIDDATA
            If row.PERIOD = "Spot" And row.YY1 = 0 And row.YY2 = 0 And row.MM1 = 0 And row.MM2 = 0 Then
                row.FFA_PRICE = m_ROUTE_DETAIL.SPOT_PRICE
                row.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot
                RecFound = True
                Exit For
            End If
        Next

        If RecFound Then
            rgv_BS.ResetBindings(False)
            rgv_PERIODS.Refresh()
            rgv_PERIODS.Rows(CurrentIndex).IsCurrent = True
            rgv_PERIODS.Rows(CurrentIndex).IsSelected = True
        End If
    End Sub
#End Region

#Region "TogglesForFixedIndex"
    Private Sub rcb_MAIN_HAS_PROFIT_SHARE_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles rcb_MAIN_HAS_PROFIT_SHARE.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            se_MAIN_PROFIT_SHARE_CAP.ReadOnly = False
            rgb_MAIN_HAS_PROFIT_SHARE.Enabled = True
        Else
            se_MAIN_PROFIT_SHARE_CAP.ReadOnly = True
            se_MAIN_PROFIT_SHARE_CAP.Value = 0
            rgb_MAIN_HAS_PROFIT_SHARE.Enabled = False
        End If
    End Sub
    Private Sub rcb_OPTION_HAS_PROFIT_SHARE_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles rcb_OPTION_HAS_PROFIT_SHARE.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            se_OPTION_PROFIT_SHARE_CAP.ReadOnly = False
            rgb_OPTION_HAS_PROFIT_SHARE.Enabled = True
        Else
            se_OPTION_PROFIT_SHARE_CAP.ReadOnly = True
            se_OPTION_PROFIT_SHARE_CAP.Value = 0
            rgb_OPTION_HAS_PROFIT_SHARE.Enabled = False
        End If
    End Sub
    Private Sub rcb_PROFIT_SHARE_OPTION_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs)
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            rgb_OPTION_HAS_PROFIT_SHARE.Enabled = True
        Else
            rgb_OPTION_HAS_PROFIT_SHARE.Enabled = False
        End If
    End Sub

    Private Sub rcb_MAIN_PROFIT_STRIKE_EQUAL_TC_RATE_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs)
        If rrb_MAIN_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                se_MAIN_PROFIT_SHARE_STRIKE.ReadOnly = True
                se_MAIN_PROFIT_SHARE_STRIKE.Value = se_MAIN_FFA_PRICE.Value
            ElseIf args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off Then
                se_MAIN_PROFIT_SHARE_STRIKE.ReadOnly = False
                se_MAIN_PROFIT_SHARE_STRIKE.Value = 0
            End If
        ElseIf rrb_MAIN_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                se_MAIN_PROFIT_SHARE_STRIKE.ReadOnly = False
                se_MAIN_PROFIT_SHARE_STRIKE.Value = 0
            ElseIf args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off Then
                se_MAIN_PROFIT_SHARE_STRIKE.ReadOnly = True
                se_MAIN_PROFIT_SHARE_STRIKE.Value = 0
            End If
        End If

        If rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            se_OPTION_PROFIT_SHARE_STRIKE.ReadOnly = se_MAIN_PROFIT_SHARE_STRIKE.ReadOnly
            se_OPTION_PROFIT_SHARE_STRIKE.Value = se_MAIN_PROFIT_SHARE_STRIKE.Value
        End If
    End Sub
    Private Sub rcb_OPTION_PROFIT_STRIKE_EQUAL_TC_RATE_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs)
        If rrb_OPTION_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                se_OPTION_PROFIT_SHARE_STRIKE.ReadOnly = True
                se_OPTION_PROFIT_SHARE_STRIKE.Value = se_OPTION_FFA_PRICE.Value
            ElseIf args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off Then
                se_OPTION_PROFIT_SHARE_STRIKE.ReadOnly = False
                se_OPTION_PROFIT_SHARE_STRIKE.Value = 0
            End If
        ElseIf rrb_OPTION_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                se_OPTION_PROFIT_SHARE_STRIKE.ReadOnly = False
                se_OPTION_PROFIT_SHARE_STRIKE.Value = 0
            ElseIf args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off Then
                se_OPTION_PROFIT_SHARE_STRIKE.ReadOnly = True
                se_OPTION_PROFIT_SHARE_STRIKE.Value = 0
            End If
        End If
    End Sub

    Private Sub rrb_MAIN_CHARTER_TYPE_FIXED_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles rrb_MAIN_CHARTER_TYPE_FIXED.ToggleStateChanged
        If rrb_MAIN_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On And rrb_OPTION_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            rcb_SAME_AS_MAIN.Visible = True
        ElseIf rrb_MAIN_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On And rrb_OPTION_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            rcb_SAME_AS_MAIN.Visible = True
        Else
            rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off
            rcb_SAME_AS_MAIN.Visible = False
        End If

        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off Then
            se_MAIN_CHARTER_RATE.Value = 0
            se_MAIN_CHARTER_RATE.ReadOnly = True

            rcb_MAIN_HAS_PROFIT_SHARE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
            rcb_MAIN_HAS_PROFIT_SHARE.Visible = False
            rgb_MAIN_HAS_PROFIT_SHARE.Text = "Index Linked Options"

            lbl_MAIN_PROFIT_SHARING.Text = "Floor Price"

            se_MAIN_PROFIT_SHARE_STRIKE.Value = 0
            se_MAIN_PROFIT_SHARE_CAP.Value = 0
            se_MAIN_PROFIT_SHARE.Value = 100

            se_MAIN_PROFIT_SHARE_STRIKE.ReadOnly = False

            lbl_MAIN_PROFIT_SHARE.Text = "Cap Share %"

            rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.Text = "Cap adjusted for VEP"
        Else 'FIXED has been selected
            se_MAIN_CHARTER_RATE.Value = 0
            se_MAIN_CHARTER_RATE.ReadOnly = False

            rcb_MAIN_HAS_PROFIT_SHARE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off
            rcb_MAIN_HAS_PROFIT_SHARE.Visible = True
            rgb_MAIN_HAS_PROFIT_SHARE.Text = "Profit Sharing Details"

            lbl_MAIN_PROFIT_SHARING.Text = "Strike Price"

            se_MAIN_PROFIT_SHARE_STRIKE.Value = 0
            se_MAIN_PROFIT_SHARE_CAP.Value = 0
            se_MAIN_PROFIT_SHARE.Value = 50

            lbl_MAIN_PROFIT_SHARE.Text = "Profit Share %"

            rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.Text = "Payout adjusted for VEP"
        End If
    End Sub

    Private Sub rrb_OPTION_CHARTER_TYPE_FIXED_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles rrb_OPTION_CHARTER_TYPE_FIXED.ToggleStateChanged
        If rrb_MAIN_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On And rrb_OPTION_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            rcb_SAME_AS_MAIN.Visible = True
        ElseIf rrb_MAIN_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On And rrb_OPTION_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            rcb_SAME_AS_MAIN.Visible = True
        Else
            rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off
            rcb_SAME_AS_MAIN.Visible = False
        End If

        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off Then
            se_OPTION_CHARTER_RATE.Value = 0
            se_OPTION_CHARTER_RATE.ReadOnly = True

            rcb_OPTION_HAS_PROFIT_SHARE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
            rcb_OPTION_HAS_PROFIT_SHARE.Visible = False
            rgb_OPTION_HAS_PROFIT_SHARE.Enabled = True
            rgb_OPTION_HAS_PROFIT_SHARE.Text = "Index Linked Options"

            lbl_OPTION_PROFIT_SHARING.Text = "Floor Price"

            se_OPTION_PROFIT_SHARE_STRIKE.Value = 0
            se_OPTION_PROFIT_SHARE_CAP.Value = 0
            se_OPTION_PROFIT_SHARE.Value = 100

            se_OPTION_PROFIT_SHARE_STRIKE.ReadOnly = False

            lbl_OPTION_PROFIT_SHARE.Text = "Cap Share %"

            rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.Text = "Cap adjusted for VEP"
        Else 'FIXED has been selected
            se_OPTION_CHARTER_RATE.Value = 0
            se_OPTION_CHARTER_RATE.ReadOnly = False

            rcb_OPTION_HAS_PROFIT_SHARE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off
            rcb_OPTION_HAS_PROFIT_SHARE.Visible = True
            rgb_OPTION_HAS_PROFIT_SHARE.Text = "Profit Sharing Details"

            lbl_OPTION_PROFIT_SHARING.Text = "Strike Price"

            se_OPTION_PROFIT_SHARE_STRIKE.Value = 0
            se_OPTION_PROFIT_SHARE_CAP.Value = 0
            se_OPTION_PROFIT_SHARE.Value = 50

            lbl_OPTION_PROFIT_SHARE.Text = "Profit Share %"

            rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.Text = "Payout adjusted for VEP"
        End If
    End Sub

    Private Sub rcb_SAME_AS_MAIN_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles rcb_SAME_AS_MAIN.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then

            rgb_OPTION_CHARTER_TYPE.Enabled = False
            rcb_OPTION_HAS_PROFIT_SHARE.Enabled = False
            rgb_OPTION_HAS_PROFIT_SHARE.Enabled = False

            se_OPTION_CHARTER_RATE.ReadOnly = se_MAIN_CHARTER_RATE.ReadOnly
            rrb_OPTION_CHARTER_TYPE_FIXED.ToggleState = rrb_MAIN_CHARTER_TYPE_FIXED.ToggleState

            se_OPTION_CHARTER_RATE.Value = se_MAIN_CHARTER_RATE.Value

            rcb_OPTION_HAS_PROFIT_SHARE.ToggleState = rcb_MAIN_HAS_PROFIT_SHARE.ToggleState
            se_OPTION_PROFIT_SHARE_STRIKE.Value = se_MAIN_PROFIT_SHARE_STRIKE.Value

            se_OPTION_PROFIT_SHARE_STRIKE.Value = se_MAIN_PROFIT_SHARE_STRIKE.Value
            se_OPTION_PROFIT_SHARE_CAP.Value = se_MAIN_PROFIT_SHARE_CAP.Value
            se_OPTION_PROFIT_SHARE.Value = se_MAIN_PROFIT_SHARE.Value
        Else
            rgb_OPTION_CHARTER_TYPE.Enabled = True
            rcb_OPTION_HAS_PROFIT_SHARE.Enabled = True
            rgb_OPTION_HAS_PROFIT_SHARE.Enabled = True
        End If
    End Sub

    Private Sub se_MAIN_CHARTER_RATE_ValueChanged(sender As Object, e As EventArgs) Handles se_MAIN_CHARTER_RATE.ValueChanged
        If rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            se_OPTION_CHARTER_RATE.Value = se_MAIN_CHARTER_RATE.Value
        End If
        If se_MAIN_CHARTER_RATE.Value = 0 Then
            se_MAIN_PROTECTION.Value = 100
            se_MAIN_PROTECTION.ReadOnly = True
        ElseIf se_MAIN_CHARTER_RATE.Value > 0 Then
            se_MAIN_PROTECTION.ReadOnly = False
        End If
    End Sub
    Private Sub se_MAIN_PROFIT_SHARE_STRIKE_ValueChanged(sender As Object, e As EventArgs) Handles se_MAIN_PROFIT_SHARE_STRIKE.ValueChanged
        If rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            se_OPTION_PROFIT_SHARE_STRIKE.Value = se_MAIN_PROFIT_SHARE_STRIKE.Value
        End If
    End Sub

    Private Sub se_MAIN_PROFIT_SHARE_CAP_ValueChanged(sender As Object, e As EventArgs) Handles se_MAIN_PROFIT_SHARE_CAP.ValueChanged
        If rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            se_OPTION_PROFIT_SHARE_CAP.Value = se_MAIN_PROFIT_SHARE_CAP.Value
        End If
    End Sub

    Private Sub se_MAIN_PROFIT_SHARE_ValueChanged(sender As Object, e As EventArgs) Handles se_MAIN_PROFIT_SHARE.ValueChanged
        If rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            se_OPTION_PROFIT_SHARE.Value = se_MAIN_PROFIT_SHARE.Value
        End If
    End Sub

#End Region

#Region "SWAPForwardPrice"
    Public Function SwapFixing(ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer) As Double
        Dim TempFixing As Double = 0.0#

        'Check First Live Data, if data exists for specified period there is no need to interpolate
        Dim DataFound = (From q In m_GRIDDATA _
                         Where q.YY1 = f_YY1 And q.MM1 = f_MM1 _
                         And q.YY2 = f_YY2 And q.MM2 = f_MM2 _
                         Select q.FFA_PRICE).Distinct.ToList
        If DataFound.Count > 0 Then
            Return DataFound(0)
        End If

        Dim tc As New Collection
        Dim frwd = (From q In m_GRIDDATA _
                    Where q.PERIOD <> "Spot" _
                    Order By q.YY2, q.MM2, q.YY1, q.MM1 Descending Select q)
        For Each r In frwd
            Dim nrf As New ForwardsClass
            nrf.CMSROUTE_ID = m_ROUTE_DETAIL.ROUTE_ID
            nrf.FIXING = r.FFA_PRICE
            nrf.FIXING_DATE = m_ROUTE_DETAIL.SPOT_FIXING_DATE
            nrf.MM1 = r.MM1
            nrf.MM2 = r.MM2
            nrf.PERIOD = DateAndTime.DateDiff(DateInterval.Month, m_ROUTE_DETAIL.SPOT_FIXING_DATE, DateSerial(r.YY2, r.MM2, Date.DaysInMonth(r.YY2, r.MM2)))
            nrf.REPORTDESC = r.PERIOD
            nrf.ROUTE_ID = m_ROUTE_DETAIL.ROUTE_ID
            nrf.YY1 = r.YY1
            nrf.YY2 = r.YY2
            nrf.KEY = Format(nrf.FIXING_DATE, "yyyMMdd") & Format(nrf.YY1, "0000") & Format(nrf.MM1, "00") & Format(nrf.YY2, "00") & Format(nrf.MM2, "00")
            If tc.Contains(nrf.KEY) = False Then
                tc.Add(nrf, nrf.KEY)
            End If
        Next

        tc = NormalizeData(tc)
        Return ForwardRate(tc, m_ROUTE_DETAIL.SPOT_FIXING_DATE, f_YY1, f_MM1, f_YY2, f_MM2)
    End Function
    Private Function NormalizeData(ByVal tc As Collection) As Collection
        'normalize data, covert from forward-forward to preriod t/c
        Dim qr1 = From q As ForwardsClass In tc _
                  Order By q.YY2 Descending, q.MM2 Descending, q.YY1 Descending, q.MM1 Descending _
                  Select q
        For Each mr In qr1
            Dim tv As Double = 0
            Dim sd As Date = DateSerial(mr.YY1, mr.MM1, 1)
            Dim ed As Date = DateSerial(mr.YY2, mr.MM2, Date.DaysInMonth(mr.YY2, mr.MM2))
            Dim nm As Integer = DateAndTime.DateDiff(DateInterval.Month, sd, ed) + 1
            Dim monthsrem As Integer = DateAndTime.DateDiff(DateInterval.Month, mr.FIXING_DATE, sd)

            tv = tv + mr.FIXING * nm

            Dim cntr As Integer
            While monthsrem > 0 And cntr < 1000
                Dim qr2 = (From q As ForwardsClass In tc _
                           Where DateAndTime.DateDiff(DateInterval.Month, DateSerial(q.YY2, q.MM2, 1), sd) = 1 _
                           Order By q.YY1, q.MM1 _
                           Select q).FirstOrDefault
                If IsNothing(qr2) = False Then
                    sd = DateSerial(qr2.YY1, qr2.MM1, 1)
                    ed = DateSerial(qr2.YY2, qr2.MM2, Date.DaysInMonth(qr2.YY2, qr2.MM2))
                    nm = DateAndTime.DateDiff(DateInterval.Month, sd, ed) + 1
                    tv = tv + qr2.FIXING * nm
                    monthsrem = DateAndTime.DateDiff(DateInterval.Month, mr.FIXING_DATE, sd)
                Else
                    monthsrem = 0
                End If
                cntr += 1
            End While
            mr.NORMFIX = tv / (mr.PERIOD + 1)
        Next

        Dim tempc As New Collection
        For Each r As ForwardsClass In tc
            tempc.Add(r, r.KEY)
        Next

        For I = 1 To tempc.Count - 1
            Dim c1 As ForwardsClass = tempc.Item(I)
            Dim c2 As ForwardsClass = tempc.Item(I + 1)
            If c1.PERIOD = c2.PERIOD Then
                tc.Remove(c1.KEY)
            End If
        Next

        NormalizeData = tc
    End Function
    Private Function ForwardRate(ByVal FWDSCOL As Collection, ByVal FixDate As Date, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As Double
        Dim tc As New Collection
        Dim qr0 = From q As ForwardsClass In FWDSCOL _
                  Where q.FIXING_DATE = FixDate
                  Order By q.FIXING_DATE _
                  Select q
        For Each r In qr0
            Dim nr As New ForwardsClass
            nr.CMSROUTE_ID = r.CMSROUTE_ID
            nr.FIXING = r.FIXING
            nr.NORMFIX = r.NORMFIX
            nr.FIXING_DATE = r.FIXING_DATE
            nr.MM1 = r.MM1
            nr.MM2 = r.MM2
            nr.PERIOD = r.PERIOD
            nr.REPORTDESC = r.REPORTDESC
            nr.ROUTE_ID = r.ROUTE_ID
            nr.YY1 = r.YY1
            nr.YY2 = r.YY2
            nr.KEY = r.KEY
            tc.Add(nr, nr.KEY)
        Next
        ForwardRate = PerformSpline(FixDate, tc, YY1, MM1, YY2, MM2)
    End Function
    Private Function PerformSpline(ByVal FixDate As Date, ByVal ROUTE_FIXINGS As Collection, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As Double
        Dim retval As Double
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

        Dim OrderedData = From q As ForwardsClass In ROUTE_FIXINGS _
                          Order By q.PERIOD _
                          Select q

        For Each r As ForwardsClass In OrderedData
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

        Dim StartPeriod As Integer = DateAndTime.DateDiff(DateInterval.Month, FixDate, DateSerial(YY1, MM1, 1)) - 1
        Dim EndPeriod As Integer = DateAndTime.DateDiff(DateInterval.Month, FixDate, DateSerial(YY2, MM2, 1))
        Dim SSP As Double
        Dim SEP As Double

        'for Start period
        If StartPeriod = x(n) Then
            SSP = f(n)
        ElseIf StartPeriod < x(n) Then
            For i = 0 To n - 1
                If StartPeriod >= x(i) And StartPeriod <= x(i + 1) Then
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
                If EndPeriod >= x(i) And EndPeriod <= x(i + 1) Then
                    SEP = f(i) + b(i) * (EndPeriod - x(i)) + c(i) * (EndPeriod - x(i)) ^ 2 + d(i) * (EndPeriod - x(i)) ^ 3
                    Exit For
                End If
            Next
        End If

        If StartPeriod <= x(n) And EndPeriod <= x(n) Then
            retval = (SEP * (EndPeriod + 1) - SSP * (StartPeriod + 1)) / (EndPeriod - StartPeriod)
        Else
            retval = f(n)
        End If

        Return retval
    End Function
#End Region

#Region "CalculationsForButtons"
    Private Sub rbtn_CALCULATE_Click(sender As Object, e As EventArgs) Handles rbtn_CALCULATE.Click
        'First deal with charter main period
        If rrb_MAIN_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            If rrb_OPTION_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MainFixedOptionFixed()
            ElseIf rrb_OPTION_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MainFixedOptionIndex()
            End If
        ElseIf rrb_MAIN_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            If rrb_OPTION_CHARTER_TYPE_FIXED.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MainIndexOptionFixed()
            ElseIf rrb_OPTION_CHARTER_TYPE_INDEX.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MainIndexOptionIndex()
            End If
        End If
    End Sub
    Private Sub MainFixedOptionFixed()
        Dim FlagMainTCRateUserInput As Boolean = False
        Dim FlagOptionTCRateUserInput As Boolean = False
        Dim FlagNoOptionnalPEriod As Boolean = False
        Dim TCperiodStart As New TCMonthsClass(cb_TC_START.SelectedItem.DataBoundItem.MM, cb_TC_START.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim TCperiodEnd As New TCMonthsClass(cb_TC_END.SelectedItem.DataBoundItem.MM, cb_TC_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim OPTperiodEnd As New TCMonthsClass(cb_OPTION_END.SelectedItem.DataBoundItem.MM, cb_OPTION_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim NoOptionMonths As Integer = DateDiff(DateInterval.Month, DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1), DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, 1))
        Dim OPTperiodStart As TCMonthsClass = New TCMonthsClass(cb_TC_END.SelectedItem.DataBoundItem.MM, cb_TC_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        OPTperiodStart.MM = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1).AddMonths(1).Month
        OPTperiodStart.YY = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1).AddMonths(1).Year
        If DateSerial(OPTperiodStart.YY, OPTperiodStart.MM, 1) >= DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, 1) Then
            FlagNoOptionnalPEriod = True
        End If
        Dim SDate As Date = DateSerial(TCperiodStart.YY, TCperiodStart.MM, 1)
        Dim LDate As Date = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, Date.DaysInMonth(TCperiodEnd.YY, TCperiodEnd.MM))
        Dim NoTCMainDays As Integer = DateDiff(DateInterval.Day, SDate, LDate) + 1
        SDate = DateSerial(OPTperiodStart.YY, OPTperiodStart.MM, 1)
        LDate = DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, Date.DaysInMonth(OPTperiodEnd.YY, OPTperiodEnd.MM))
        Dim NoTCOptDays As Integer = DateDiff(DateInterval.Day, SDate, LDate) + 1
        Dim s As Double = 0.0# 'strike price
        Dim f As Double = 0.0# 'ffa price
        Dim v As Double = 0.0# 'volatility
        Dim i As Double = 0.0# 'interest rate

        m_ListOfOptions.Clear()
        se_MAIN_ADJ_TC_RATE.Value = 0
        If se_MAIN_CHARTER_RATE.Value = 0 Then
            se_MAIN_CHARTER_RATE.Value = se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100
        Else
            FlagMainTCRateUserInput = True
        End If

        If rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            se_OPTION_CHARTER_RATE.Value = se_MAIN_CHARTER_RATE.Value
            se_OPTION_PROFIT_SHARE_STRIKE.Value = se_MAIN_PROFIT_SHARE_STRIKE.Value
            se_OPTION_PROFIT_SHARE_CAP.Value = se_MAIN_PROFIT_SHARE_CAP.Value
            se_OPTION_PROFIT_SHARE.Value = se_MAIN_PROFIT_SHARE.Value
            rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState
        Else
            If se_OPTION_CHARTER_RATE.Value = 0 Then
                se_OPTION_CHARTER_RATE.Value = se_OPTION_FFA_PRICE.Value * se_VEP.Value / 100
            Else
                FlagOptionTCRateUserInput = True
            End If
        End If

        se_MAIN_ADJ_TC_RATE.Value = 0
        se_OPTIONS_PRICE.Value = 0

        If rcb_MAIN_HAS_PROFIT_SHARE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On And _
            se_MAIN_PROFIT_SHARE_STRIKE.Value > 0 Then
            'if main TC has profit share side, it means we have to buy a call to participate in future spot price increases
            'calculate the option participation value for the main TC period
            Dim fmcall As New FFAOptionSolveClass
            v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 + se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
            f = se_MAIN_FFA_PRICE.Value
            s = se_MAIN_PROFIT_SHARE_STRIKE.Value
            fmcall = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Buy, OptionSolveForEnum.Price, _
                                 s, f, v / 100, i / 100, _
                                 TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
            fmcall.FastSolvePrice()
            If rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                fmcall.OD.PayReceive = fmcall.OD.PayReceive * (se_MAIN_PROFIT_SHARE.Value / 100) * (se_VEP.Value / 100)
            Else
                fmcall.OD.PayReceive = fmcall.OD.PayReceive * se_MAIN_PROFIT_SHARE.Value / 100
            End If
            fmcall.FromStrategy = StrategyForEnum.BuyOptParticipationMain
            m_ListOfOptions.Add(fmcall)
            se_OPTIONS_PRICE.Value += fmcall.OD.PayReceive

            'If it has a cap then
            If se_MAIN_PROFIT_SHARE_CAP.Value > 0 Then
                Dim fmcap As New FFAOptionSolveClass
                v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 - se_SKEW.Value / 100)
                i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
                f = se_MAIN_FFA_PRICE.Value
                s = se_MAIN_PROFIT_SHARE_CAP.Value
                fmcap = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
                fmcap.FastSolvePrice()
                If rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                    fmcap.OD.PayReceive = fmcap.OD.PayReceive * (se_MAIN_PROFIT_SHARE.Value / 100) * (se_VEP.Value / 100)
                Else
                    fmcap.OD.PayReceive = fmcap.OD.PayReceive * se_MAIN_PROFIT_SHARE.Value / 100
                End If
                fmcap.FromStrategy = StrategyForEnum.SellCapParticipationMain
                m_ListOfOptions.Add(fmcap)
                se_OPTIONS_PRICE.Value += fmcap.OD.PayReceive
            End If
        End If

        'íf Main Charter Rate is not zero, i.e. we define ourselves a TC rate requested, we have to adjust for the NPV of the difference to the FFA Price
        If FlagMainTCRateUserInput = True Then
            'taking into consideration the protection level offered
            se_OPTIONS_PRICE.Value += (se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100 - se_MAIN_CHARTER_RATE.Value) * NoTCMainDays * (se_MAIN_PROTECTION.Value / 100)
            Dim npvoftcrate As New FFAOptionSolveClass
            npvoftcrate.OD.PayReceive = (se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100 - se_MAIN_CHARTER_RATE.Value) * NoTCMainDays * (se_MAIN_PROTECTION.Value / 100)
            npvoftcrate.FromStrategy = StrategyForEnum.FFAvsTCRateDiff
            npvoftcrate.OD.FFAPrice = se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100
            npvoftcrate.OD.StrikePrice = se_MAIN_CHARTER_RATE.Value
            m_ListOfOptions.Add(npvoftcrate)

            'now, if protection offered is not 100% that means user is implicitly selling a put option
            If se_MAIN_PROTECTION.Value < 100 Then
                Dim fmput As New FFAOptionSolveClass
                v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 - se_SKEW.Value / 100)
                i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
                f = se_MAIN_FFA_PRICE.Value
                s = se_MAIN_CHARTER_RATE.Value / (se_VEP.Value / 100)
                fmput = LoadFFAClass(1, OptionTypeEnum.OPut, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
                fmput.FastSolvePrice()
                fmput.OD.PayReceive = fmput.OD.PayReceive * (se_VEP.Value / 100)
                fmput.FromStrategy = StrategyForEnum.SellPutFromTCNotFullyProtected
                m_ListOfOptions.Add(fmput)
                se_OPTIONS_PRICE.Value += fmput.OD.PayReceive
            End If
        End If

        'now deal with the option side of the TC
        If FlagNoOptionnalPEriod = True Then
            GoTo NoOptionPeriod
        End If

        If rcb_OPTION_HAS_PROFIT_SHARE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On And se_OPTION_PROFIT_SHARE_STRIKE.Value > 0 Then
            'if option TC has profit share side, it means we have to buy a call to participate in future spot price increases
            Dim focall As New FFAOptionSolveClass
            v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM) * (1 + se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
            f = se_OPTION_FFA_PRICE.Value
            If rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                se_OPTION_PROFIT_SHARE_STRIKE.Value = se_MAIN_PROFIT_SHARE_STRIKE.Value
            End If
            s = se_OPTION_PROFIT_SHARE_STRIKE.Value
            focall = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Buy, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM)
            focall.FastSolvePrice()
            If rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                focall.OD.PayReceive = focall.OD.PayReceive * (se_OPTION_PROFIT_SHARE.Value / 100) * (se_VEP.Value / 100)
            Else
                focall.OD.PayReceive = focall.OD.PayReceive * se_OPTION_PROFIT_SHARE.Value / 100
            End If
            focall.FromStrategy = StrategyForEnum.BuyOptParticipationOpt
            m_ListOfOptions.Add(focall)
            se_OPTIONS_PRICE.Value += focall.OD.PayReceive

            'If it has a cap then
            If se_OPTION_PROFIT_SHARE_CAP.Value > 0 Then
                Dim focap As New FFAOptionSolveClass
                v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM) * (1 - se_SKEW.Value / 100)
                i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
                f = se_OPTION_FFA_PRICE.Value
                s = se_OPTION_PROFIT_SHARE_CAP.Value
                focap = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
                focap.FastSolvePrice()
                If rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                    focap.OD.PayReceive = focap.OD.PayReceive * (se_OPTION_PROFIT_SHARE.Value / 100) * (se_VEP.Value / 100)
                Else
                    focap.OD.PayReceive = focap.OD.PayReceive * se_OPTION_PROFIT_SHARE.Value / 100
                End If
                focap.FromStrategy = StrategyForEnum.SellCapParticipationOpt
                m_ListOfOptions.Add(focap)
                se_OPTIONS_PRICE.Value += focap.OD.PayReceive
            End If
        End If

        'now deal with the cost of the extended option to charterer
        Dim fcall As New FFAOptionSolveClass
        v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM)
        i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
        f = se_OPTION_FFA_PRICE.Value
        If FlagOptionTCRateUserInput = False Then
            If rcb_SAME_AS_MAIN.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                s = se_MAIN_CHARTER_RATE.Value / (se_VEP.Value / 100)
            Else
                s = se_OPTION_FFA_PRICE.Value
            End If
        Else
            s = se_OPTION_CHARTER_RATE.Value / (se_VEP.Value / 100)
        End If
        fcall = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                             s, f, v / 100, i / 100, _
                             OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM)
        fcall.FastSolvePrice()
        fcall.OD.PayReceive = fcall.OD.PayReceive * se_VEP.Value / 100
        fcall.FromStrategy = StrategyForEnum.SellOptionPeriodForTC
        m_ListOfOptions.Add(fcall)
        se_OPTIONS_PRICE.Value += fcall.OD.PayReceive

NoOptionPeriod:
        If FlagMainTCRateUserInput = True Then
            se_MAIN_ADJ_TC_RATE.Value = se_MAIN_CHARTER_RATE.Value + se_OPTIONS_PRICE.Value / NoTCMainDays
        Else
            se_MAIN_CHARTER_RATE.Value = se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100
        End If
        se_MAIN_ADJ_TC_RATE.Value = se_MAIN_CHARTER_RATE.Value + se_OPTIONS_PRICE.Value / NoTCMainDays
        se_OPTIONS_PRICE.Value = se_OPTIONS_PRICE.Value / NoTCMainDays

        se_OPTION_ADJ_TC_RATE.Value = 0
        For Each r In m_ListOfOptions
            se_OPTION_ADJ_TC_RATE.Value += r.OD.PayReceive
        Next
        se_OPTION_ADJ_TC_RATE.Value = se_OPTION_ADJ_TC_RATE.Value / NoTCMainDays
    End Sub
    Private Sub MainFixedOptionIndex()
        Dim FlagMainTCRateUserInput As Boolean = False
        Dim FlagOptionTCRateUserInput As Boolean = False
        Dim FlagNoOptionnalPEriod As Boolean = False
        Dim TCperiodStart As New TCMonthsClass(cb_TC_START.SelectedItem.DataBoundItem.MM, cb_TC_START.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim TCperiodEnd As New TCMonthsClass(cb_TC_END.SelectedItem.DataBoundItem.MM, cb_TC_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim OPTperiodEnd As New TCMonthsClass(cb_OPTION_END.SelectedItem.DataBoundItem.MM, cb_OPTION_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim NoOptionMonths As Integer = DateDiff(DateInterval.Month, DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1), DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, 1))
        Dim OPTperiodStart As TCMonthsClass = New TCMonthsClass(cb_TC_END.SelectedItem.DataBoundItem.MM, cb_TC_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        OPTperiodStart.MM = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1).AddMonths(1).Month
        OPTperiodStart.YY = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1).AddMonths(1).Year
        If DateSerial(OPTperiodStart.YY, OPTperiodStart.MM, 1) >= DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, 1) Then
            FlagNoOptionnalPEriod = True
        End If
        Dim SDate As Date = DateSerial(TCperiodStart.YY, TCperiodStart.MM, 1)
        Dim LDate As Date = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, Date.DaysInMonth(TCperiodEnd.YY, TCperiodEnd.MM))
        Dim NoTCMainDays As Integer = DateDiff(DateInterval.Day, SDate, LDate) + 1
        SDate = DateSerial(OPTperiodStart.YY, OPTperiodStart.MM, 1)
        LDate = DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, Date.DaysInMonth(OPTperiodEnd.YY, OPTperiodEnd.MM))
        Dim NoTCOptDays As Integer = DateDiff(DateInterval.Day, SDate, LDate) + 1
        Dim s As Double = 0.0# 'strike price
        Dim f As Double = 0.0# 'ffa price
        Dim v As Double = 0.0# 'volatility
        Dim i As Double = 0.0# 'interest rate

        m_ListOfOptions.Clear()
        se_MAIN_ADJ_TC_RATE.Value = 0
        se_OPTIONS_PRICE.Value = 0
        If se_MAIN_CHARTER_RATE.Value = 0 Then
            se_MAIN_CHARTER_RATE.Value = se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100
        Else
            FlagMainTCRateUserInput = True
        End If

        If rcb_MAIN_HAS_PROFIT_SHARE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            'if main TC has profit share side, it means we have to buy a call to participate in future spot price increases
            'calculate the option participation value for the main TC period
            Dim fmcall As New FFAOptionSolveClass
            v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 + se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
            f = se_MAIN_FFA_PRICE.Value
            s = se_MAIN_PROFIT_SHARE_STRIKE.Value
            fmcall = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Buy, OptionSolveForEnum.Price, _
                                 s, f, v / 100, i / 100, _
                                 TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
            fmcall.FastSolvePrice()
            If rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                fmcall.OD.PayReceive = fmcall.OD.PayReceive * (se_MAIN_PROFIT_SHARE.Value / 100) * (se_VEP.Value / 100)
            Else
                fmcall.OD.PayReceive = fmcall.OD.PayReceive * se_MAIN_PROFIT_SHARE.Value / 100
            End If
            fmcall.FromStrategy = StrategyForEnum.BuyOptParticipationMain
            m_ListOfOptions.Add(fmcall)
            se_OPTIONS_PRICE.Value += fmcall.OD.PayReceive

            'If it has a cap then
            If se_MAIN_PROFIT_SHARE_CAP.Value > 0 Then
                Dim fmcap As New FFAOptionSolveClass
                v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 - se_SKEW.Value / 100)
                i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
                f = se_MAIN_FFA_PRICE.Value
                s = se_MAIN_PROFIT_SHARE_CAP.Value
                fmcap = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
                fmcap.FastSolvePrice()
                If rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                    fmcap.OD.PayReceive = fmcap.OD.PayReceive * (se_MAIN_PROFIT_SHARE.Value / 100) * (se_VEP.Value / 100)
                Else
                    fmcap.OD.PayReceive = fmcap.OD.PayReceive * se_MAIN_PROFIT_SHARE.Value / 100
                End If
                fmcap.FromStrategy = StrategyForEnum.SellCapParticipationMain
                m_ListOfOptions.Add(fmcap)
                se_OPTIONS_PRICE.Value += fmcap.OD.PayReceive
            End If
        End If

        'íf Main Charter Rate is not zero, i.e. we define ourselves a TC rate requested, we have to adjust for the NPV of the difference to the FFA Price
        If FlagMainTCRateUserInput = True Then
            se_OPTIONS_PRICE.Value += (se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100 - se_MAIN_CHARTER_RATE.Value) * NoTCMainDays
            Dim npvoftcrate As New FFAOptionSolveClass
            npvoftcrate.OD.PayReceive = (se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100 - se_MAIN_CHARTER_RATE.Value) * NoTCMainDays
            npvoftcrate.FromStrategy = StrategyForEnum.FFAvsTCRateDiff
            npvoftcrate.OD.FFAPrice = se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100
            npvoftcrate.OD.StrikePrice = se_MAIN_CHARTER_RATE.Value
            m_ListOfOptions.Add(npvoftcrate)

            'now, if protection offered is not 100% that means user is implicitly selling a put option
            If se_MAIN_PROTECTION.Value < 100 Then
                Dim fmput As New FFAOptionSolveClass
                v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 - se_SKEW.Value / 100)
                i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
                f = se_MAIN_FFA_PRICE.Value
                s = se_MAIN_CHARTER_RATE.Value / (se_VEP.Value / 100)
                fmput = LoadFFAClass(1, OptionTypeEnum.OPut, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
                fmput.FastSolvePrice()
                fmput.OD.PayReceive = fmput.OD.PayReceive * (se_VEP.Value / 100)
                fmput.FromStrategy = StrategyForEnum.SellPutFromTCNotFullyProtected
                m_ListOfOptions.Add(fmput)
                se_OPTIONS_PRICE.Value += fmput.OD.PayReceive
            End If
        End If

        'now deal with the option side of the TC, in this case is index linked
        se_OPTION_CHARTER_RATE.Value = 0
        'if option TC has floor
        If se_OPTION_PROFIT_SHARE_STRIKE.Value > 0 Then 'has floor
            Dim fofloor As New FFAOptionSolveClass
            'calculate the option participation value for the Opt TC period
            v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM) * (1 + se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
            f = se_OPTION_FFA_PRICE.Value
            If rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                s = se_OPTION_PROFIT_SHARE_STRIKE.Value
            Else
                s = se_OPTION_PROFIT_SHARE_STRIKE.Value / (se_VEP.Value / 100)
            End If
            fofloor = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Buy, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM)
            fofloor.FastSolvePrice()
            fofloor.OD.PayReceive = fofloor.OD.PayReceive * se_VEP.Value / 100 * (se_OPTION_PROFIT_SHARE.Value / 100)
            fofloor.FromStrategy = StrategyForEnum.BuyIndexLinkedFloor
            m_ListOfOptions.Add(fofloor)
            se_OPTIONS_PRICE.Value += fofloor.OD.PayReceive
        End If

        'If it has a cap then
        If se_OPTION_PROFIT_SHARE_CAP.Value > 0 Then
            Dim focap As New FFAOptionSolveClass
            v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM) * (1 - se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
            f = se_OPTION_FFA_PRICE.Value
            If rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                s = se_OPTION_PROFIT_SHARE_CAP.Value
            Else
                s = se_OPTION_PROFIT_SHARE_CAP.Value / (se_VEP.Value / 100)
            End If
            focap = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                s, f, v / 100, i / 100, _
                                TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
            focap.FastSolvePrice()
            focap.OD.PayReceive = focap.OD.PayReceive * (se_VEP.Value / 100) * (se_OPTION_PROFIT_SHARE.Value / 100)
            focap.FromStrategy = StrategyForEnum.SellIndexLinkedCap
            m_ListOfOptions.Add(focap)
            se_OPTIONS_PRICE.Value += focap.OD.PayReceive
        End If

        If FlagMainTCRateUserInput = True Then
            se_MAIN_ADJ_TC_RATE.Value = se_MAIN_CHARTER_RATE.Value + se_OPTIONS_PRICE.Value / NoTCMainDays
        Else
            se_MAIN_CHARTER_RATE.Value = se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100
        End If
        se_MAIN_ADJ_TC_RATE.Value = se_MAIN_CHARTER_RATE.Value + se_OPTIONS_PRICE.Value / NoTCMainDays
        se_OPTIONS_PRICE.Value = se_OPTIONS_PRICE.Value / NoTCMainDays

        se_OPTION_ADJ_TC_RATE.Value = 0
        For Each r In m_ListOfOptions
            se_OPTION_ADJ_TC_RATE.Value += r.OD.PayReceive
        Next
        se_OPTION_ADJ_TC_RATE.Value = se_OPTION_ADJ_TC_RATE.Value / NoTCMainDays
    End Sub
    Private Sub MainIndexOptionIndex()
        Dim FlagMainTCRateUserInput As Boolean = False
        Dim FlagOptionTCRateUserInput As Boolean = False
        Dim FlagNoOptionnalPEriod As Boolean = False
        Dim TCperiodStart As New TCMonthsClass(cb_TC_START.SelectedItem.DataBoundItem.MM, cb_TC_START.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim TCperiodEnd As New TCMonthsClass(cb_TC_END.SelectedItem.DataBoundItem.MM, cb_TC_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim OPTperiodEnd As New TCMonthsClass(cb_OPTION_END.SelectedItem.DataBoundItem.MM, cb_OPTION_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim NoOptionMonths As Integer = DateDiff(DateInterval.Month, DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1), DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, 1))
        Dim OPTperiodStart As TCMonthsClass = New TCMonthsClass(cb_TC_END.SelectedItem.DataBoundItem.MM, cb_TC_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        OPTperiodStart.MM = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1).AddMonths(1).Month
        OPTperiodStart.YY = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1).AddMonths(1).Year
        If DateSerial(OPTperiodStart.YY, OPTperiodStart.MM, 1) >= DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, 1) Then
            FlagNoOptionnalPEriod = True
        End If
        Dim SDate As Date = DateSerial(TCperiodStart.YY, TCperiodStart.MM, 1)
        Dim LDate As Date = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, Date.DaysInMonth(TCperiodEnd.YY, TCperiodEnd.MM))
        Dim NoTCMainDays As Integer = DateDiff(DateInterval.Day, SDate, LDate) + 1
        SDate = DateSerial(OPTperiodStart.YY, OPTperiodStart.MM, 1)
        LDate = DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, Date.DaysInMonth(OPTperiodEnd.YY, OPTperiodEnd.MM))
        Dim NoTCOptDays As Integer = DateDiff(DateInterval.Day, SDate, LDate) + 1
        Dim s As Double = 0.0# 'strike price
        Dim f As Double = 0.0# 'ffa price
        Dim v As Double = 0.0# 'volatility
        Dim i As Double = 0.0# 'interest rate

        m_ListOfOptions.Clear()
        se_MAIN_ADJ_TC_RATE.Value = 0
        se_MAIN_CHARTER_RATE.Value = 0
        se_OPTIONS_PRICE.Value = 0

        'first deal with main side
        If se_MAIN_PROFIT_SHARE_STRIKE.Value > 0 Then 'has floor
            Dim fmfloor As New FFAOptionSolveClass
            v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 + se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
            f = se_MAIN_FFA_PRICE.Value
            s = se_MAIN_PROFIT_SHARE_STRIKE.Value / (se_VEP.Value / 100)
            fmfloor = LoadFFAClass(1, OptionTypeEnum.OPut, OptionBSEnum.Buy, OptionSolveForEnum.Price, _
                                 s, f, v / 100, i / 100, _
                                 TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
            fmfloor.FastSolvePrice()
            fmfloor.OD.PayReceive = fmfloor.OD.PayReceive * se_VEP.Value / 100
            fmfloor.FromStrategy = StrategyForEnum.BuyIndexLinkedFloor
            m_ListOfOptions.Add(fmfloor)
            se_OPTIONS_PRICE.Value += fmfloor.OD.PayReceive
        End If

        'If it has a cap then
        If se_MAIN_PROFIT_SHARE_CAP.Value > 0 Then
            Dim fmcap As New FFAOptionSolveClass
            v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 - se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
            f = se_MAIN_FFA_PRICE.Value
            If rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                s = se_MAIN_PROFIT_SHARE_CAP.Value
            Else
                s = se_MAIN_PROFIT_SHARE_CAP.Value / (se_VEP.Value / 100)
            End If
            fmcap = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                s, f, v / 100, i / 100, _
                                TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
            fmcap.FastSolvePrice()
            fmcap.OD.PayReceive = fmcap.OD.PayReceive * (se_VEP.Value / 100) * se_MAIN_PROFIT_SHARE.Value / 100
            fmcap.FromStrategy = StrategyForEnum.SellCapParticipationMain
            m_ListOfOptions.Add(fmcap)
            se_OPTIONS_PRICE.Value += fmcap.OD.PayReceive
        End If

        'now deal with the option side of the TC, in this case is index linked
        se_OPTION_CHARTER_RATE.Value = 0
        If FlagNoOptionnalPEriod = True Then
            GoTo NoOptionPeriod
        End If

        'if option TC has cap or floor
        If se_OPTION_PROFIT_SHARE_CAP.Value > 0 Then 'has floor
            Dim fofloor As New FFAOptionSolveClass
            v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM) * (1 + se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
            f = se_OPTION_FFA_PRICE.Value
            s = se_OPTION_PROFIT_SHARE_STRIKE.Value / (se_VEP.Value / 100)
            fofloor = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Buy, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM)
            fofloor.FastSolvePrice()
            fofloor.OD.PayReceive = fofloor.OD.PayReceive * se_VEP.Value / 100
            fofloor.FromStrategy = StrategyForEnum.BuyIndexLinkedFloor
            m_ListOfOptions.Add(fofloor)
            se_OPTIONS_PRICE.Value += fofloor.OD.PayReceive
        End If

        'If it has a cap then
        If se_OPTION_PROFIT_SHARE_CAP.Value > 0 Then
            Dim focap As New FFAOptionSolveClass
            v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM) * (1 - se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
            f = se_OPTION_FFA_PRICE.Value
            If rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                s = se_OPTION_PROFIT_SHARE_CAP.Value
            Else
                s = se_OPTION_PROFIT_SHARE_CAP.Value / (se_VEP.Value / 100)
            End If
            focap = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                s, f, v / 100, i / 100, _
                                TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
            focap.FastSolvePrice()
            focap.OD.PayReceive = focap.OD.PayReceive * (se_VEP.Value / 100) * se_OPTION_PROFIT_SHARE.Value / 100
            focap.FromStrategy = StrategyForEnum.SellIndexLinkedCap
            m_ListOfOptions.Add(focap)
            se_OPTIONS_PRICE.Value += focap.OD.PayReceive
        End If

NoOptionPeriod:
        se_MAIN_CHARTER_RATE.Value = (se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100) - se_MAIN_FFA_PRICE.Value
        se_MAIN_ADJ_TC_RATE.Value = se_MAIN_CHARTER_RATE.Value + se_OPTIONS_PRICE.Value / NoTCMainDays
        se_OPTIONS_PRICE.Value = se_OPTIONS_PRICE.Value / NoTCMainDays

        se_OPTION_ADJ_TC_RATE.Value = 0
        For Each r In m_ListOfOptions
            se_OPTION_ADJ_TC_RATE.Value += r.OD.PayReceive
        Next
        se_OPTION_ADJ_TC_RATE.Value = se_OPTION_ADJ_TC_RATE.Value / NoTCMainDays
    End Sub
    Private Sub MainIndexOptionFixed()
        Dim FlagMainTCRateUserInput As Boolean = False
        Dim FlagOptionTCRateUserInput As Boolean = False
        Dim FlagNoOptionnalPEriod As Boolean = False
        Dim TCperiodStart As New TCMonthsClass(cb_TC_START.SelectedItem.DataBoundItem.MM, cb_TC_START.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim TCperiodEnd As New TCMonthsClass(cb_TC_END.SelectedItem.DataBoundItem.MM, cb_TC_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim OPTperiodEnd As New TCMonthsClass(cb_OPTION_END.SelectedItem.DataBoundItem.MM, cb_OPTION_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        Dim NoOptionMonths As Integer = DateDiff(DateInterval.Month, DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1), DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, 1))
        Dim OPTperiodStart As TCMonthsClass = New TCMonthsClass(cb_TC_END.SelectedItem.DataBoundItem.MM, cb_TC_END.SelectedItem.DataBoundItem.YY, m_SERVER_DATE)
        OPTperiodStart.MM = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1).AddMonths(1).Month
        OPTperiodStart.YY = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, 1).AddMonths(1).Year
        If DateSerial(OPTperiodStart.YY, OPTperiodStart.MM, 1) >= DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, 1) Then
            FlagNoOptionnalPEriod = True
        End If
        Dim SDate As Date = DateSerial(TCperiodStart.YY, TCperiodStart.MM, 1)
        Dim LDate As Date = DateSerial(TCperiodEnd.YY, TCperiodEnd.MM, Date.DaysInMonth(TCperiodEnd.YY, TCperiodEnd.MM))
        Dim NoTCMainDays As Integer = DateDiff(DateInterval.Day, SDate, LDate) + 1
        SDate = DateSerial(OPTperiodStart.YY, OPTperiodStart.MM, 1)
        LDate = DateSerial(OPTperiodEnd.YY, OPTperiodEnd.MM, Date.DaysInMonth(OPTperiodEnd.YY, OPTperiodEnd.MM))
        Dim NoTCOptDays As Integer = DateDiff(DateInterval.Day, SDate, LDate) + 1
        Dim s As Double = 0.0# 'strike price
        Dim f As Double = 0.0# 'ffa price
        Dim v As Double = 0.0# 'volatility
        Dim i As Double = 0.0# 'interest rate

        m_ListOfOptions.Clear()

        'first deal with profit share issues
        se_MAIN_ADJ_TC_RATE.Value = 0
        se_MAIN_CHARTER_RATE.Value = 0
        se_OPTIONS_PRICE.Value = 0

        If se_MAIN_PROFIT_SHARE_STRIKE.Value > 0 Then 'has floor
            'calculate the option participation value for the main TC period
            Dim fmfloor As New FFAOptionSolveClass
            v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 + se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
            f = se_MAIN_FFA_PRICE.Value
            s = se_MAIN_PROFIT_SHARE_STRIKE.Value / (se_VEP.Value / 100)
            fmfloor = LoadFFAClass(1, OptionTypeEnum.OPut, OptionBSEnum.Buy, OptionSolveForEnum.Price, _
                                 s, f, v / 100, i / 100, _
                                 TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
            fmfloor.FastSolvePrice()
            fmfloor.OD.PayReceive = fmfloor.OD.PayReceive * se_VEP.Value / 100
            fmfloor.FromStrategy = StrategyForEnum.BuyIndexLinkedFloor
            m_ListOfOptions.Add(fmfloor)
            se_OPTIONS_PRICE.Value += fmfloor.OD.PayReceive
        End If

        'If it has a cap then
        If se_MAIN_PROFIT_SHARE_CAP.Value > 0 Then
            Dim fmcap As New FFAOptionSolveClass
            v = SwapVolatility(TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM) * (1 - se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, TCperiodEnd.YY, TCperiodEnd.MM)
            f = se_MAIN_FFA_PRICE.Value
            If rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                s = se_MAIN_PROFIT_SHARE_CAP.Value
            Else
                s = se_MAIN_PROFIT_SHARE_CAP.Value / (se_VEP.Value / 100)
            End If
            fmcap = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                s, f, v / 100, i / 100, _
                                TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
            fmcap.FastSolvePrice()
            fmcap.OD.PayReceive = fmcap.OD.PayReceive * (se_VEP.Value / 100) * se_MAIN_PROFIT_SHARE.Value / 100
            fmcap.FromStrategy = StrategyForEnum.SellCapParticipationMain
            m_ListOfOptions.Add(fmcap)
            se_OPTIONS_PRICE.Value += fmcap.OD.PayReceive
        End If

        'now deal with the option side of the TC
        If FlagNoOptionnalPEriod = True Then
            GoTo NoOptionPeriod
        End If

        If se_OPTION_CHARTER_RATE.Value = 0 Then
            se_OPTION_CHARTER_RATE.Value = se_OPTION_FFA_PRICE.Value * se_VEP.Value / 100
        ElseIf se_OPTION_CHARTER_RATE.Value > 0 Then
            FlagOptionTCRateUserInput = True
        End If

        'if it has profit sharing
        If rcb_OPTION_HAS_PROFIT_SHARE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            'calculate the option participation value for the Opt TC period
            Dim focall As New FFAOptionSolveClass
            v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM) * (1 + se_SKEW.Value / 100)
            i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
            f = se_OPTION_FFA_PRICE.Value
            s = se_OPTION_PROFIT_SHARE_STRIKE.Value
            focall = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Buy, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM)
            focall.FastSolvePrice()
            If rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                focall.OD.PayReceive = focall.OD.PayReceive * (se_OPTION_PROFIT_SHARE.Value / 100) * (se_VEP.Value / 100)
            Else
                focall.OD.PayReceive = focall.OD.PayReceive * se_OPTION_PROFIT_SHARE.Value / 100
            End If
            focall.FromStrategy = StrategyForEnum.BuyOptParticipationOpt
            m_ListOfOptions.Add(focall)
            se_OPTIONS_PRICE.Value += focall.OD.PayReceive

            'If it has a cap then
            If se_OPTION_PROFIT_SHARE_CAP.Value > 0 Then
                Dim focap As New FFAOptionSolveClass
                v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM) * (1 - se_SKEW.Value / 100)
                i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
                f = se_OPTION_FFA_PRICE.Value
                s = se_OPTION_PROFIT_SHARE_CAP.Value
                focap = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                                    s, f, v / 100, i / 100, _
                                    TCperiodStart.YY, TCperiodStart.MM, TCperiodEnd.YY, TCperiodEnd.MM)
                focap.FastSolvePrice()
                If rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                    focap.OD.PayReceive = focap.OD.PayReceive * (se_OPTION_PROFIT_SHARE.Value / 100) * (se_VEP.Value / 100)
                Else
                    focap.OD.PayReceive = focap.OD.PayReceive * se_OPTION_PROFIT_SHARE.Value / 100
                End If
                focap.FromStrategy = StrategyForEnum.SellCapParticipationOpt
                m_ListOfOptions.Add(focap)
                se_OPTIONS_PRICE.Value += focap.OD.PayReceive
            End If
        End If

        'now deal with the cost of the extended option to charterer
        Dim fcall As New FFAOptionSolveClass
        v = SwapVolatility(OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM)
        i = RiskFreeRate(m_INTEREST_RATES, OPTperiodEnd.YY, OPTperiodEnd.MM)
        f = se_OPTION_FFA_PRICE.Value 
        If se_OPTION_CHARTER_RATE.Value = 0 Then
            s = se_OPTION_FFA_PRICE.Value
        ElseIf se_OPTION_CHARTER_RATE.Value > 0 Then
            s = se_OPTION_CHARTER_RATE.Value / (se_VEP.Value / 100)
        End If
        fcall = LoadFFAClass(1, OptionTypeEnum.OCall, OptionBSEnum.Sell, OptionSolveForEnum.Price, _
                             s, f, v / 100, i / 100, _
                             OPTperiodStart.YY, OPTperiodStart.MM, OPTperiodEnd.YY, OPTperiodEnd.MM)
        fcall.FastSolvePrice()
        fcall.OD.PayReceive = fcall.OD.PayReceive * se_VEP.Value / 100
        fcall.FromStrategy = StrategyForEnum.SellOptionPeriodForTC
        m_ListOfOptions.Add(fcall)
        se_OPTIONS_PRICE.Value += fcall.OD.PayReceive

NoOptionPeriod:
        se_MAIN_CHARTER_RATE.Value = (se_MAIN_FFA_PRICE.Value * se_VEP.Value / 100) - se_MAIN_FFA_PRICE.Value
        se_MAIN_ADJ_TC_RATE.Value = se_MAIN_CHARTER_RATE.Value + se_OPTIONS_PRICE.Value / NoTCMainDays
        se_OPTIONS_PRICE.Value = se_OPTIONS_PRICE.Value / NoTCMainDays

        se_OPTION_ADJ_TC_RATE.Value = 0
        For Each r In m_ListOfOptions
            se_OPTION_ADJ_TC_RATE.Value += r.OD.PayReceive
        Next
        se_OPTION_ADJ_TC_RATE.Value = se_OPTION_ADJ_TC_RATE.Value / NoTCMainDays
    End Sub

    Private Function LoadFFAClass(ByVal LegNo As Integer, ByVal OptType As OptionTypeEnum, ByVal BS As OptionBSEnum, SolveFor As OptionSolveForEnum, ByVal StrikePrice As Double, ByVal FFAPrice As Double, ByVal Vol As Double, ByVal RFR As Double, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As FFAOptionSolveClass
        Dim f_FFA As New FFAOptionSolveClass

        f_FFA.LegNo = LegNo
        f_FFA.Solver = OptionSolverEnum.Analytic
        f_FFA.SolveFor = OptionSolveForEnum.Price
        f_FFA.OD.RouteId = m_ROUTE_ID
        f_FFA.OD.TradeDate = m_SERVER_DATE
        f_FFA.OD.Holidays = m_PUBLIC_HOLIDAYS
        f_FFA.OD.OptionType = OptType
        f_FFA.OD.OptionBS = BS
        f_FFA.OD.Quantity = -1
        f_FFA.OD.StrikePrice = StrikePrice
        f_FFA.OD.PricingTick = m_ROUTE_DETAIL.PRICING_TICK
        f_FFA.OD.Volatility = Vol
        f_FFA.OD.Skew = 0
        f_FFA.OD.OptionValue = 0

        f_FFA.OD.FFAPrice = FFAPrice
        f_FFA.OD.RiskFreeRate = RFR
        f_FFA.OD.AvgPrice = m_ROUTE_DETAIL.FIXING_AVG
        f_FFA.OD.AvgIncludesToday = m_ROUTE_DETAIL.AVERAGE_INCLUDES_TODAY

        f_FFA.OD.MM1 = MM1
        f_FFA.OD.YY1 = YY1
        f_FFA.OD.MM2 = MM2
        f_FFA.OD.YY2 = YY2

        Return f_FFA
    End Function
    Private Function RiskFreeRate(ByVal f_FIXINGS As List(Of FFAOptCalcService.InterestRatesClass), ByVal YY As Integer, ByVal MM As Integer) As Double

        Dim nomonths As Integer = DateAndTime.DateDiff(DateInterval.Month, m_SERVER_DATE, DateSerial(YY, MM, 1))
        If nomonths <= 0 Then nomonths = 1

        Dim nextrate = (From q In f_FIXINGS _
                        Where q.PERIOD >= nomonths _
                        Order By q.PERIOD _
                        Select q _
                        Take 1).FirstOrDefault

        Dim prevrate = (From q In f_FIXINGS _
                        Where q.PERIOD <= nomonths _
                        Order By q.PERIOD Descending _
                        Select q _
                        Take 1).FirstOrDefault

        Dim rincr As Double = 0.0#
        If nextrate.PERIOD > prevrate.PERIOD Then
            rincr = (nextrate.RATE - prevrate.RATE) / (nextrate.PERIOD - prevrate.PERIOD)
        End If

        RiskFreeRate = prevrate.RATE
        For I = prevrate.PERIOD To nomonths
            RiskFreeRate += rincr
        Next
    End Function
    
    Private Function SwapVolatility(ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer) As Double
        Dim IsFound = (From q In m_GRIDDATA Where _
                       q.YY1 = f_YY1 And q.MM1 = f_MM1 _
                       And q.YY2 = f_YY2 And q.MM2 = f_MM2 _
                       Select q).FirstOrDefault
        If IsNothing(IsFound) = False Then
            If IsFound.IVOL = 0 Then
                Return IsFound.HVOL
            Else
                Return IsFound.IVOL
            End If

        End If

        Dim maxperiod = (From q In m_GRIDDATA Where q.PERIOD <> "Spot" Order By q.YY2 Descending, q.MM2 Descending Select q).First
        Dim periods As Integer = DateAndTime.DateDiff(DateInterval.Month, m_SERVER_DATE, DateSerial(maxperiod.YY2, maxperiod.MM2, 1)) + 1

        Dim n As Integer = periods - 1
        Dim f(n) As Double
        Dim x(n) As Double

        Dim i As Integer = 0
        Dim j As Integer = 0

        For indx = 0 To periods - 1
            x(indx) = indx
            f(indx) = 0.0#
        Next

        Dim Fixings = (From q In m_GRIDDATA Where q.PERIOD <> "Spot" Select q)
        Dim VOL_FIXINGS As New List(Of BalticFTPClass)
        For Each r In Fixings
            Dim nr As BalticFTPClass = New BalticFTPClass
            If r.IVOL = 0 Then
                nr.FIXING = r.HVOL
            Else
                nr.FIXING = r.IVOL
            End If

            nr.YY1 = r.YY1
            nr.MM1 = r.MM1
            nr.YY2 = r.YY2
            nr.MM2 = r.MM2
            nr.ROUTE_ID = DateAndTime.DateDiff(DateInterval.Month, DateSerial(r.YY1, r.MM1, 1), DateSerial(r.YY2, r.MM2, 1)) + 1
            nr.PERIOD = DateAndTime.DateDiff(DateInterval.Month, m_SERVER_DATE, DateSerial(r.YY2, r.MM2, 1))
            VOL_FIXINGS.Add(nr)
        Next
        Dim qryvol = (From q In VOL_FIXINGS Order By q.YY2 Descending, q.MM2 Descending, q.ROUTE_ID Descending Select q).ToList
        For Each r In qryvol
            For i = r.PERIOD To (r.PERIOD - r.ROUTE_ID + 1) Step -1
                f(i) = r.FIXING
            Next
        Next

        Dim sp As Integer = DateAndTime.DateDiff(DateInterval.Month, m_SERVER_DATE, DateSerial(f_YY1, f_MM1, 1))
        Dim ep As Integer = DateAndTime.DateDiff(DateInterval.Month, m_SERVER_DATE, DateSerial(f_YY2, f_MM2, 1))
        For i = sp To ep
            SwapVolatility += f(i)
        Next
        SwapVolatility = SwapVolatility / (ep - sp + 1)

    End Function
    Private Function SwapVolatility2(ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer) As Double
        Dim IsFound As Double = (From q In m_GRIDDATA Where _
                                 q.YY1 = f_YY1 And q.MM1 = f_MM1 _
                                 And q.YY2 = f_YY2 And q.MM2 = f_MM2 _
                                 Select q.IVOL).FirstOrDefault
        If IsFound > 0 Then
            Return IsFound
        End If

        Dim VOL_FIXINGS As New Collection
        Dim Fixings = (From q In m_GRIDDATA Where q.PERIOD <> "Spot" Select q)
        For Each r In Fixings
            Dim nr As BalticFTPClass = New BalticFTPClass
            nr.ROUTE_ID = m_ROUTE_DETAIL.ROUTE_ID
            nr.FIXING_DATE = m_ROUTE_DETAIL.SPOT_FIXING_DATE
            If r.IVOL = 0 Then
                nr.FIXING = r.HVOL
            Else
                nr.FIXING = r.IVOL
            End If

            nr.YY1 = r.YY1
            nr.MM1 = r.MM1
            nr.YY2 = r.YY2
            nr.MM2 = r.MM2
            nr.PERIOD = DateAndTime.DateDiff(DateInterval.Month, m_SERVER_DATE, DateSerial(r.YY2, r.MM2, Date.DaysInMonth(r.YY2, r.MM2)))
            nr.KEY = Format(r.YY2, "0000") & Format(r.MM2, "00") & Format(r.YY1, "0000") & Format(r.MM1, "00")
            If VOL_FIXINGS.Contains(nr.KEY) = False Then
                VOL_FIXINGS.Add(nr, nr.KEY)
            End If
        Next

        'normalize data, covert from forward-forward to preriod t/c
        Dim qr2 = (From q As BalticFTPClass In VOL_FIXINGS _
                   Order By q.YY2 Descending, q.MM2 Descending, q.YY1 Descending, q.MM1 Descending _
                   Select q).ToList
        For Each mr In qr2
            Dim tc As Double = 0
            Dim sd As Date = DateSerial(mr.YY1, mr.MM1, 1)
            Dim ed As Date = DateSerial(mr.YY2, mr.MM2, Date.DaysInMonth(mr.YY2, mr.MM2))
            Dim nm As Integer = DateAndTime.DateDiff(DateInterval.Month, sd, ed) + 1
            Dim monthsrem As Integer = DateAndTime.DateDiff(DateInterval.Month, Today, sd)

            tc = tc + mr.FIXING * nm

            Dim cntr As Integer
            While monthsrem > 0 And cntr < 1000
                Dim qr3 = (From q As BalticFTPClass In VOL_FIXINGS _
                           Where DateAndTime.DateDiff(DateInterval.Month, DateSerial(q.YY2, q.MM2, 1), sd) = 1 _
                           Order By q.YY1, q.MM1 _
                           Select q).FirstOrDefault
                If IsNothing(qr3) = False Then
                    sd = DateSerial(qr3.YY1, qr3.MM1, 1)
                    ed = DateSerial(qr3.YY2, qr3.MM2, Date.DaysInMonth(qr3.YY2, qr3.MM2))
                    nm = DateAndTime.DateDiff(DateInterval.Month, sd, ed) + 1
                    tc = tc + qr3.FIXING * nm
                    monthsrem = DateAndTime.DateDiff(DateInterval.Month, Today, sd)
                End If
                cntr += 1
            End While
            mr.NORMFIX = tc / (mr.PERIOD + 1)
        Next

        Dim tempc As New Collection
        For Each r In VOL_FIXINGS
            tempc.Add(r, r.KEY)
        Next

        Dim KI As Integer = 0
        For K = 1 To tempc.Count - 1
            Dim c1 = tempc.Item(K)
            Dim c2 = tempc.Item(K + 1)
            If c1.PERIOD = c2.PERIOD Then
                VOL_FIXINGS.Remove(c1.KEY)
            End If
        Next

        Dim n As Integer = VOL_FIXINGS.Count - 1
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

        Dim tempdata = From q As BalticFTPClass In VOL_FIXINGS Order By q.PERIOD Select q
        For Each r In tempdata
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

        Dim StartPeriod As Integer = DateAndTime.DateDiff(DateInterval.Month, Today, DateSerial(f_YY1, f_MM1, 1)) - 1
        Dim EndPeriod As Integer = DateAndTime.DateDiff(DateInterval.Month, Today, DateSerial(f_YY2, f_MM2, 1))
        Dim SSP As Double
        Dim SEP As Double

        'for Start period
        If StartPeriod = x(n) Then
            SSP = f(n)
        ElseIf StartPeriod < x(n) Then
            For i = 0 To n - 1
                If StartPeriod >= x(i) And StartPeriod <= x(i + 1) Then
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
                If EndPeriod >= x(i) And EndPeriod <= x(i + 1) Then
                    SEP = f(i) + b(i) * (EndPeriod - x(i)) + c(i) * (EndPeriod - x(i)) ^ 2 + d(i) * (EndPeriod - x(i)) ^ 3
                    Exit For
                End If
            Next
        End If

        SwapVolatility2 = (SEP * (EndPeriod + 1) - SSP * (StartPeriod + 1)) / (EndPeriod - StartPeriod)
    End Function
#End Region

    Private Sub rmi_SAVE_FILE_Click(sender As Object, e As EventArgs) Handles rmi_SAVE_FILE.Click
        If m_IsHistorical = True Then
            Beep()
            MsgError(Me, "This is a historical file, cannot be saved again.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        Dim s As String = "default.xml"
        Dim dialog As New SaveFileDialog()
        dialog.Filter = "ffa files (*.ffa)|*.ffa|All files (*.*)|*.*"
        dialog.Title = "Select a file to save historical data"
        dialog.AddExtension = True
        dialog.DefaultExt = ".ffa"
        If My.Settings.FileSaveDirectory <> "" Then
            dialog.InitialDirectory = My.Settings.FileSaveDirectory
        End If

        If dialog.ShowDialog() = DialogResult.OK Then
            s = dialog.FileName
            Dim f As New FFATCOptClass(Me)
            Dim xs As New XmlSerializer(GetType(FFATCOptClass))
            Dim sw As New IO.StreamWriter(s)
            xs.Serialize(sw, f)
            sw.Close()
            My.Settings.FileSaveDirectory = Path.GetDirectoryName(dialog.FileName)
            My.Settings.Save()
            f = Nothing
        End If
    End Sub

    Private Sub rmi_OPEN_FILE_Click(sender As Object, e As EventArgs) Handles rmi_OPEN_FILE.Click

        Dim s As String = "default.xml"
        Dim dialog As New OpenFileDialog
        dialog.Filter = "ffa files (*.ffa)|*.ffa|All files (*.*)|*.*"
        dialog.Title = "Select a file to load historical data"
        dialog.AddExtension = True
        dialog.DefaultExt = ".ffa"
        If My.Settings.FileSaveDirectory <> "" Then
            dialog.InitialDirectory = My.Settings.FileSaveDirectory
        End If

        If dialog.ShowDialog() = DialogResult.OK Then
            s = dialog.FileName
            Dim sr As New IO.StreamReader(s)
            Dim form As FFATCOptClass
            Dim xs As New XmlSerializer(GetType(FFATCOptClass))
            form = xs.Deserialize(sr)
            sr.Close()
            If form.ROUTE_ID <> m_ROUTE_ID Then
                Beep()
                MsgError(Me, "The Route in file you are trying to open is not the same" & vbCrLf & "as the one currently displayed, operation will be cancelled.", "Error opening selected file", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
            Dim answ = MsgError(Me, "Do you want to also load historical prevailing data (Yes), or you just want to load TC structure? (NO)", "Load Historical Data?", MessageBoxButtons.YesNo, Telerik.WinControls.RadMessageIcon.Question)
            If answ = Windows.Forms.DialogResult.Yes Then
                m_ROUTE_ID = form.ROUTE_ID
                m_SERVER_DATE = form.SERVER_DATE
                m_FIXINGS = form.FIXINGS
                m_INTEREST_RATES = form.INTEREST_RATES
                m_PUBLIC_HOLIDAYS = form.PUBLIC_HOLIDAYS
                m_ROUTE_DETAIL = form.ROUTE_DETAIL
                m_IsHistorical = True
            End If
            m_FirstTime = True
            f_TC_END_FirstTime = True
            f_TC_OPTION_END_FirstTime = True

            Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            Dim ms As New MemoryStream
            bf.Serialize(ms, m_FIXINGS)
            ms.Seek(0, SeekOrigin.Begin)
            Dim nlist As List(Of FFAOptCalcService.VolDataClass) = bf.Deserialize(ms)
            Me.Prepare(nlist)

            Me.se_VEP.Value = form.VEP
            Me.se_SKEW.Value = form.SKEW

            Dim ErrorInTCPeriods As Boolean = True

            Dim I As Integer = 0
            For Each r As TCMonthsClass In cb_TC_START.DataSource
                If r.YY = form.TC_START.YY And r.MM = form.TC_START.MM Then
                    Me.cb_TC_START.SelectedIndex = I
                    ErrorInTCPeriods = False
                    Exit For
                End If
                I += 1
            Next
            I = 0
            ErrorInTCPeriods = True
            For Each r As TCMonthsClass In cb_TC_END.DataSource
                If r.YY = form.TC_END.YY And r.MM = form.TC_END.MM Then
                    Me.cb_TC_END.SelectedIndex = I
                    ErrorInTCPeriods = False
                    Exit For
                End If
                I += 1
            Next
            I = 0
            ErrorInTCPeriods = True
            For Each r As TCMonthsClass In cb_OPTION_END.DataSource
                If r.YY = form.OPTION_END.YY And r.MM = form.OPTION_END.MM Then
                    Me.cb_OPTION_END.SelectedIndex = I
                    ErrorInTCPeriods = False
                    Exit For
                End If
                I += 1
            Next
            If ErrorInTCPeriods = True Then
                Telerik.WinControls.RadMessageBox.SetThemeName(Me.ThemeName)
                Telerik.WinControls.RadMessageBox.Instance.FormElement.TitleBar.TitlePrimitive.Font = New Font(Me.FormElement.TitleBar.Font.Name, Me.FormElement.TitleBar.Font.Size + 3, FontStyle.Bold)
                Beep()
                Telerik.WinControls.RadMessageBox.Show(Me, "Histoical TC Periods are not any more compatible with current data, aborting loading", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If

            Me.rdtp_SERVER_DATE.Text = FormatDateTime(m_SERVER_DATE)
            Me.rrb_MAIN_CHARTER_TYPE_FIXED.ToggleState = form.MAIN_CHARTER_TYPE
            Me.se_MAIN_CHARTER_RATE.Value = form.MAIN_CHARTER_RATE
            Me.se_MAIN_PROTECTION.Value = form.MAIN_PROTECTION
            Me.se_MAIN_FFA_PRICE.Value = form.MAIN_FFA_PRICE
            Me.se_OPTION_ADJ_TC_RATE.Value = form.MAIN_ADJ_TC_RATE
            Me.se_OPTIONS_PRICE.Value = form.OPTIONS_PRICE
            Me.rcb_MAIN_HAS_PROFIT_SHARE.ToggleState = form.MAIN_HAS_PROFIT_SHARE
            Me.se_MAIN_PROFIT_SHARE.Value = form.MAIN_PROFIT_SHARE
            Me.rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = form.MAIN_PROFIT_SHARE_ADJ_FOR_VEP
            Me.se_MAIN_PROFIT_SHARE_STRIKE.Value = form.MAIN_PROFIT_SHARE_STRIKE
            Me.se_MAIN_PROFIT_SHARE_CAP.Value = form.MAIN_PROFIT_SHARE_CAP
            Me.se_MAIN_ADJ_TC_RATE.Value = form.MAIN_ADJ_TC_RATE

            Me.rcb_SAME_AS_MAIN.ToggleState = form.SAME_AS_MAIN
            Me.rrb_OPTION_CHARTER_TYPE_FIXED.ToggleState = form.OPTION_CHARTER_TYPE
            Me.se_OPTION_CHARTER_RATE.Value = form.OPTION_CHARTER_RATE
            Me.se_OPTION_FFA_PRICE.Value = form.OPTION_FFA_PRICE
            Me.se_OPTION_ADJ_TC_RATE.Value = form.OPTION_ADJ_TC_RATE
            Me.rcb_OPTION_HAS_PROFIT_SHARE.ToggleState = form.OPTION_HAS_PROFIT_SHARE
            Me.se_OPTION_PROFIT_SHARE.Value = form.OPTION_PROFIT_SHARE
            Me.rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState = form.OPTION_PROFIT_SHARE_ADJ_FOR_VEP
            Me.se_OPTION_PROFIT_SHARE_STRIKE.Value = form.OPTION_PROFIT_SHARE_STRIKE
            Me.se_OPTION_PROFIT_SHARE_CAP.Value = form.OPTION_PROFIT_SHARE_CAP
        End If
    End Sub

    Private Sub rgv_PERIODS_CellValueChanged(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles rgv_PERIODS.CellValueChanged
        m_FindVolData = New FFAOptCalcService.VolDataClass
        m_FindVolData.FIXING_DATE = e.Row.Cells("FIXING_DATE").Value
        m_FindVolData.PERIOD = e.Row.Cells("PERIOD").Value
        m_FindVolData.YY1 = e.Row.Cells("YY1").Value
        m_FindVolData.YY2 = e.Row.Cells("YY2").Value
        m_FindVolData.MM1 = e.Row.Cells("MM1").Value
        m_FindVolData.MM2 = e.Row.Cells("MM2").Value
        m_FindVolData.FFA_PRICE = e.Row.Cells("FFA_PRICE").Value
        m_FindVolData.IVOL = e.Row.Cells("IVOL").Value
        m_FindVolData.INTEREST_RATE = e.Row.Cells("INTEREST_RATE").Value

        Dim g = m_GRIDDATA.Find(AddressOf FindVolDataGrid)
        If g IsNot Nothing Then
            g.FFA_PRICE = m_FindVolData.FFA_PRICE
            g.IVOL = m_FindVolData.IVOL
            g.INTEREST_RATE = m_FindVolData.INTEREST_RATE
            If e.Column.Name = "FFA_PRICE" Then
                g.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live
                g.LIVE_DATA = True
            End If
        End If
        Dim curindx = e.RowIndex
        rgv_BS.ResetBindings(False)
        rgv_PERIODS.Rows(curindx).IsCurrent = True

        Dim f = m_FIXINGS.Find(AddressOf FindVolData)
        If f IsNot Nothing Then
            f.FFA_PRICE = m_FindVolData.FFA_PRICE
            f.IVOL = m_FindVolData.IVOL
            f.INTEREST_RATE = m_FindVolData.INTEREST_RATE
            If e.Column.Name = "FFA_PRICE" Then
                f.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live
            End If            
        End If

        cb_OPTION_END_SelectedValueChanged(Me, e)
    End Sub
    Private Function FindVolData(ByVal data As FFAOptCalcService.VolDataClass) As Boolean
        If m_FindVolData.PERIOD = "Spot" And data.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot _
           And data.YY1 = m_FindVolData.YY1 And data.YY2 = m_FindVolData.YY2 _
           And data.MM1 = m_FindVolData.MM1 And data.MM2 = m_FindVolData.MM2 _
           And data.FIXING_DATE.Date = m_FindVolData.FIXING_DATE.Date Then
            Return True
        ElseIf m_FindVolData.PERIOD = "Spot" And data.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot _
               And data.YY1 = m_FindVolData.YY1 And data.YY2 = m_FindVolData.YY2 _
               And data.MM1 = m_FindVolData.MM1 And data.MM2 = m_FindVolData.MM2 _
               And data.FIXING_DATE.Date = m_FindVolData.FIXING_DATE.Date Then
            Return True
        ElseIf data.YY1 = m_FindVolData.YY1 And data.YY2 = m_FindVolData.YY2 _
               And data.MM1 = m_FindVolData.MM1 And data.MM2 = m_FindVolData.MM2 _
               And data.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
               And data.FIXING_DATE.Date = m_FindVolData.FIXING_DATE.Date Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function FindVolDataGrid(ByVal data As GRIDPeriodsClass) As Boolean
        If m_FindVolData.PERIOD = "Spot" And data.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot _
           And data.YY1 = m_FindVolData.YY1 And data.YY2 = m_FindVolData.YY2 _
           And data.MM1 = m_FindVolData.MM1 And data.MM2 = m_FindVolData.MM2 _
           And data.FIXING_DATE.Date = m_FindVolData.FIXING_DATE.Date Then
            Return True
        ElseIf m_FindVolData.PERIOD = "Spot" And data.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot _
               And data.YY1 = m_FindVolData.YY1 And data.YY2 = m_FindVolData.YY2 _
               And data.MM1 = m_FindVolData.MM1 And data.MM2 = m_FindVolData.MM2 _
               And data.FIXING_DATE.Date = m_FindVolData.FIXING_DATE.Date Then
            Return True
        ElseIf data.YY1 = m_FindVolData.YY1 And data.YY2 = m_FindVolData.YY2 _
               And data.MM1 = m_FindVolData.MM1 And data.MM2 = m_FindVolData.MM2 _              
               And data.FIXING_DATE.Date = m_FindVolData.FIXING_DATE.Date Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub se_MAIN_FFA_PRICE_ScreenTipNeeded(sender As Object, e As Telerik.WinControls.ScreenTipNeededEventArgs) Handles se_MAIN_FFA_PRICE.ScreenTipNeeded
        Dim screenTip As RadOffice2007ScreenTipElement = New RadOffice2007ScreenTipElement
        screenTip.CaptionLabel.Margin = New Padding(3)
        screenTip.CaptionLabel.Text = "Sample"
        screenTip.MainTextLabel.Text = "description"
        e.Item.ScreenTip = screenTip

    End Sub

    Private Sub rrb_MAIN_CHARTER_TYPE_FIXED_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles rrb_MAIN_CHARTER_TYPE_FIXED.ToolTipTextNeeded, rrb_OPTION_CHARTER_TYPE_FIXED.ToolTipTextNeeded
        e.ToolTipText = "Select for a fixed TC type, charter pays a fixed rate for the whole period."
    End Sub

    Private Sub rrb_MAIN_CHARTER_TYPE_INDEX_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles rrb_MAIN_CHARTER_TYPE_INDEX.ToolTipTextNeeded, rrb_OPTION_CHARTER_TYPE_INDEX.ToolTipTextNeeded
        e.ToolTipText = "Select for Index Linked TC type, charter pays a rate linked to the respective index performance."
    End Sub

    Private Sub lbl_MAIN_CHARTER_RATE_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles lbl_MAIN_CHARTER_RATE.ToolTipTextNeeded, lbl_OPTION_CHARTER_RATE.ToolTipTextNeeded
        e.ToolTipText = "If you have a fixed rate proposal for the main TC period enter it here." & vbCrLf & "If you want the model to calculate the rate leave the value equal to zero."
    End Sub

    Private Sub lbl_MAIN_FFA_PRICE_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles lbl_MAIN_FFA_PRICE.ToolTipTextNeeded, lbl_OPTION_FFA_PRICE.ToolTipTextNeeded
        e.ToolTipText = "This is the FFA price automatically calculated for the selected period." & vbCrLf & "You can overide this to any value of your choice."
    End Sub

    Private Sub lbl_MAIN_ADJ_TC_RATE_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles lbl_MAIN_ADJ_TC_RATE.ToolTipTextNeeded
        e.ToolTipText = "This box displays the adjusted TC rate, taking into consideration all options values." & vbCrLf & "Use it as a reference to the indicated TC rate."
    End Sub

    Private Sub rcb_MAIN_HAS_PROFIT_SHARE_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles rcb_MAIN_HAS_PROFIT_SHARE.ToolTipTextNeeded, rcb_OPTION_HAS_PROFIT_SHARE.ToolTipTextNeeded
        e.ToolTipText = "Check if the TC has a profit participation scheme."
    End Sub

    Private Sub rbtn_RESET_VALUES_Click(sender As Object, e As EventArgs) Handles rbtn_RESET_VALUES.Click
        se_MAIN_CHARTER_RATE.Value = 0
        se_OPTION_CHARTER_RATE.Value = 0
        se_MAIN_ADJ_TC_RATE.Value = 0
        se_OPTIONS_PRICE.Value = 0
    End Sub

    Private Sub se_VEP_Validated(sender As Object, e As EventArgs) Handles se_VEP.Validated
        se_MAIN_CHARTER_RATE.Value = 0
        se_OPTION_CHARTER_RATE.Value = 0
        se_MAIN_ADJ_TC_RATE.Value = 0
        se_OPTIONS_PRICE.Value = 0
        If m_FirstTime = False Then
            'MsgError(Me, "Adjusting VEP resets TC Rates values entered so far.", "Alert VEP Change", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End If
    End Sub

    Private Sub rgb_Swap_Periods_Click(sender As Object, e As EventArgs) Handles rgb_Swap_Periods.Click, RadGroupBox1.Click
        Dim f As New WebHelpForm
        f.url = "HelpForm4.pdf"
        f.Show()
    End Sub

End Class
