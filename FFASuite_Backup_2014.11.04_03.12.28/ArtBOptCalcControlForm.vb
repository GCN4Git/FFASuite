Imports System.ComponentModel
Imports Telerik.WinControls.UI
Imports FFASuite.ArtBOptCalcClasses

Public Class ArtBOptCalcControlForm
    Private m_ROUTE_ID As Integer
    Private m_FIXINGS As New List(Of FFAOptCalcService.VolDataClass)
    Private m_GRIDDATA As New List(Of GRIDPeriodsClass)
    Private m_ROUTE_DETAIL As FFAOptCalcService.SwapDataClass
    Private FirstTime As Boolean = True
    Private m_FindVolData As New FFAOptCalcService.VolDataClass
    Public WithEvents EventForm As FFAOptCalc
    Private m_FontN As Font
    Private m_FontB As Font

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

    Private Sub ArtBOptCalcControlForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Icon = My.Resources.ArtB_Robots__48X48
    End Sub

#Region "OptionCalc"
    Private VMM1 As New List(Of MMonths)
    Private VMM2 As New List(Of MMonths)
    Private VYY1 As New List(Of YYears)
    Private VYY2 As New List(Of YYears)
    Private VBS1 As New List(Of BS)
    Private VBS2 As New List(Of BS)
    Private VBS As New List(Of RadDropDownList)
    Private VOT1 As New List(Of OptionType)
    Private VOT2 As New List(Of OptionType)
    Private VOT As New List(Of RadDropDownList)
    Private se_STRIKES As New List(Of RadSpinEditor)
    Private se_PRICES As New List(Of RadSpinEditor)
    Private se_QUANT As New List(Of RadSpinEditor)
    Private se_PAYRECEIVES As New List(Of RadSpinEditor)
    Private se_VOLS As New List(Of RadSpinEditor)
    Private se_SKEWS As New List(Of RadSpinEditor)
    Private btn_CALCSTRIKES As New List(Of RadButton)
    Private btn_CALCVOL As New List(Of RadButton)
    Private btn_CALCPRICES As New List(Of RadButton)
    Private se_DELTAS As New List(Of RadSpinEditor)
    Private se_GAMMAS As New List(Of RadSpinEditor)
    Private se_VEGAS As New List(Of RadSpinEditor)
    Private se_THETAS As New List(Of RadSpinEditor)
    Private se_RHOS As New List(Of RadSpinEditor)
    Friend FFA(1) As FFAOptionSolveClass

    Private se_VOL1_flag As Boolean = False
    Private se_VOL2_flag As Boolean = False
    Private se_SKEW1_flag As Boolean = False
    Private se_SKEW2_flag As Boolean = False
    Friend OptionWorker As New BackgroundWorker
    Private Rand As New Random()
#End Region


    Public Sub Prepare()
        m_FontN = New Font(rgv_PERIODS.TableElement.Font.Name, rgv_PERIODS.TableElement.Font.Size, FontStyle.Regular)
        m_FontB = New Font(rgv_PERIODS.TableElement.Font.Name, rgv_PERIODS.TableElement.Font.Size, FontStyle.Bold)
        GraphInitialize()
        Populate()
        PrepareOptCalc()

        cb_MM1.SelectedIndex = rgv_PERIODS.Rows(1).Cells("MM1").Value - 1
        cb_YY1.SelectedValue = rgv_PERIODS.Rows(1).Cells("YY1").Value
        cb_MM2.SelectedIndex = rgv_PERIODS.Rows(1).Cells("MM2").Value - 1
        cb_YY2.SelectedValue = rgv_PERIODS.Rows(1).Cells("YY2").Value
        se_RISK_FREE_RATE.Value = rgv_PERIODS.Rows(1).Cells("INTEREST_RATE").Value
        se_VOLATILITY.Value = rgv_PERIODS.Rows(1).Cells("IVOL").Value
        se_FFA_PRICE.Value = rgv_PERIODS.Rows(1).Cells("FFA_PRICE").Value
        se_AVG_PRICE.Value = m_ROUTE_DETAIL.FIXING_AVG
        If m_ROUTE_DETAIL.AVERAGE_INCLUDES_TODAY = True Then
            cb_AVERAGE_INCLUDES_TODAY.CheckState = CheckState.Checked
        Else
            cb_AVERAGE_INCLUDES_TODAY.CheckState = CheckState.Unchecked
        End If
    End Sub
    Private Sub PrepareOptCalc()
        FFA(0) = New FFAOptionSolveClass

        If CudaEnabled = False Then
            rbtn_MC_CUDA.Enabled = False
            rbtn_MC_CUDA.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off
        End If

        If My.Settings.Model = DefaultModelEnum.MonteCarlo And CudaEnabled = True Then
            rbtn_MC_CUDA.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
        Else
            rbtn_ANALYTIC.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
        End If

        VBS.Add(cb_BS1) : VBS.Add(cb_BS2)
        VOT.Add(cb_OPTION_TYPE1) : VOT.Add(cb_OPTION_TYPE2)
        se_STRIKES.Add(se_STRIKE1) : se_STRIKES.Add(se_STRIKE2)
        se_PRICES.Add(se_OPTION_PRICE1) : se_PRICES.Add(se_OPTION_PRICE2)
        se_QUANT.Add(se_QUANTITY1) : se_QUANT.Add(se_QUANTITY2)
        se_PAYRECEIVES.Add(se_PAYRECEIVE1) : se_PAYRECEIVES.Add(se_PAYRECEIVE2)
        se_VOLS.Add(se_VOLATILITY1) : se_VOLS.Add(se_VOLATILITY2)
        se_SKEWS.Add(se_SKEW1) : se_SKEWS.Add(se_SKEW2)
        btn_CALCPRICES.Add(btn_PRICE1) : btn_CALCPRICES.Add(btn_PRICE2)
        btn_CALCVOL.Add(btn_VOL1) : btn_CALCVOL.Add(btn_VOL2)
        btn_CALCSTRIKES.Add(btn_STRIKE1) : btn_CALCSTRIKES.Add(btn_STRIKE2)
        se_DELTAS.Add(se_DELTA1) : se_DELTAS.Add(se_DELTA2)
        se_GAMMAS.Add(se_GAMMA1) : se_GAMMAS.Add(se_GAMMA2)
        se_RHOS.Add(se_RHO1) : se_RHOS.Add(se_RHO2)
        se_VEGAS.Add(se_VEGA1) : se_VEGAS.Add(se_VEGA2)
        se_THETAS.Add(se_THETA1) : se_THETAS.Add(se_THETA2)

        For I = 1 To 12
            VMM1.Add(New MMonths(I, LocalMonthName(I)))
            VMM2.Add(New MMonths(I, LocalMonthName(I)))
        Next
        cb_MM1.DisplayMember = "MMD"
        cb_MM1.ValueMember = "MM"
        cb_MM1.DataSource = VMM1

        cb_MM2.DataSource = VMM2
        cb_MM2.DisplayMember = "MMD"
        cb_MM2.ValueMember = "MM"

        Dim fYY As Integer = (From q In m_GRIDDATA Where q.YY1 > 0 Select q.YY1).Min
        Dim lYY As Integer = (From q In m_GRIDDATA Select q.YY2).Max
        For I = fYY To lYY
            VYY1.Add(New YYears(I, CStr(I)))
            VYY2.Add(New YYears(I, CStr(I)))
        Next
        cb_YY1.DisplayMember = "YYD"
        cb_YY1.ValueMember = "YY"
        cb_YY1.DataSource = VYY1

        cb_YY2.DisplayMember = "YYD"
        cb_YY2.ValueMember = "YY"
        cb_YY2.DataSource = VYY2

        VBS1.Add(New BS(OptionBSEnum.Buy, "Buy"))
        VBS1.Add(New BS(OptionBSEnum.Sell, "Sell"))
        VBS2.Add(New BS(OptionBSEnum.Buy, "Buy"))
        VBS2.Add(New BS(OptionBSEnum.Sell, "Sell"))

        cb_BS1.DisplayMember = "Des"
        cb_BS1.ValueMember = "BS"
        cb_BS1.DataSource = VBS1
        cb_BS1.SelectedIndex = 0
        cb_BS2.DisplayMember = "Des"
        cb_BS2.ValueMember = "BS"
        cb_BS2.DataSource = VBS2
        cb_BS2.SelectedIndex = 0

        VOT1.Add(New OptionType(OptionTypeEnum.OCall, "Call"))
        VOT1.Add(New OptionType(OptionTypeEnum.OPut, "Put"))
        VOT2.Add(New OptionType(OptionTypeEnum.OCall, "Call"))
        VOT2.Add(New OptionType(OptionTypeEnum.OPut, "Put"))

        cb_OPTION_TYPE1.DisplayMember = "Des"
        cb_OPTION_TYPE1.ValueMember = "OptionType"
        cb_OPTION_TYPE1.DataSource = VOT1
        cb_OPTION_TYPE1.SelectedIndex = 0

        cb_OPTION_TYPE2.DisplayMember = "Des"
        cb_OPTION_TYPE2.ValueMember = "OptionType"
        cb_OPTION_TYPE2.DataSource = VOT2
        cb_OPTION_TYPE2.SelectedIndex = 0

        se_AVG_PRICE.Value = m_ROUTE_DETAIL.FIXING_AVG
        If m_ROUTE_DETAIL.AVERAGE_INCLUDES_TODAY = True Then
            cb_AVERAGE_INCLUDES_TODAY.CheckState = CheckState.Checked
        Else
            cb_AVERAGE_INCLUDES_TODAY.CheckState = CheckState.Unchecked
        End If

        se_FFA_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_AVG_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_STRIKE1.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_STRIKE2.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_OPTION_PRICE1.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_OPTION_PRICE2.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_STRATEGY_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
    End Sub
    Private Sub GraphInitialize()
        Try
            For Each s In MSChart.Series
                s.Enabled = False
            Next

            MSChart.ChartAreas("Default").AxisY.IsStartedFromZero = False
            MSChart.ChartAreas("Default").AxisY2.IsStartedFromZero = False
            MSChart.ChartAreas("Default").AxisX.IsStartedFromZero = False
            MSChart.ChartAreas("Default").AxisX.LabelStyle.Format = "MMM-yy"
            MSChart.DataManipulator.IsStartFromFirst = True
            MSChart.DataManipulator.IsEmptyPointIgnored = True

            MSChart.ChartAreas("Default").AxisX.LabelStyle.IsEndLabelVisible = False
            MSChart.ChartAreas("Default").AxisY2.MajorGrid.Enabled = False

            ' Locale specific percentage format with no decimals
            MSChart.ChartAreas("Default").AxisY.LabelStyle.Format = "P0"
            MSChart.ChartAreas("Default").AxisY2.LabelStyle.Format = "N0"

            MSChart.Series("FFAIVol").Color = Color.Red
            MSChart.Series("FFAIVol").BorderWidth = 1

            MSChart.Series("FFAHVol").Color = Color.DarkOrange
            MSChart.Series("FFAHVol").BorderWidth = 1

            MSChart.Series("SpotHVol").Color = Color.Brown
            MSChart.Series("SpotHVol").BorderWidth = 1

            MSChart.Series("FFA").Color = Color.Blue
            MSChart.Series("FFA").BorderWidth = 1

            MSChart.Series("Spot").Color = Color.Black
            MSChart.Series("Spot").BorderWidth = 1

            MSChart.Series("SpotAvg").Color = Color.DarkSeaGreen
            MSChart.Series("SpotAvg").BorderWidth = 1

        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try

    End Sub
    Public Sub Populate()
        m_FIXINGS.Clear()
        m_GRIDDATA.Clear()
        rgv_BS.DataSource = Nothing

        Dim fdata = (From q In FIXINGS Where q.ROUTE_ID = m_ROUTE_ID)
        For Each f In fdata
            Dim nr As New FFAOptCalcService.VolDataClass
            nr.DESK_TRADER_ID = f.DESK_TRADER_ID
            nr.TRADE_ID = f.TRADE_ID
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
            nr.PNC = f.PNC
            m_FIXINGS.Add(nr)
        Next

        'Add Spot Period
        Dim Spot = (From q In m_FIXINGS _
                    Where q.ROUTE_ID = m_ROUTE_ID And _
                    (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot) _
                    Order By q.FIXING_DATE Descending Select q).FirstOrDefault
        If IsNothing(Spot) Then
            Dim ns As New GRIDPeriodsClass
            ns.FFA_PRICE = 0
            ns.IVOL = 0
            ns.HVOL = 0
            ns.INTEREST_RATE = 0
            ns.ONLYHISTORICAL = True
            ns.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot
            ns.PERIOD = "Spot"
            m_GRIDDATA.Add(ns)
        Else
            Dim ns As New GRIDPeriodsClass
            ns.FFA_PRICE = Spot.SPOT_PRICE
            ns.IVOL = Spot.HVOL
            ns.HVOL = Spot.HVOL
            ns.INTEREST_RATE = 0
            ns.ONLYHISTORICAL = Spot.ONLYHISTORICAL
            ns.VolRecordType = Spot.VolRecordType
            ns.PERIOD = "Spot"
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
                           And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level) _
                           And q.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA _
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
        rgv_PERIODS.DataSource = rgv_BS

        se_FFA_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_AVG_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_STRIKE1.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_STRIKE2.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_OPTION_PRICE1.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_OPTION_PRICE2.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES
        se_STRATEGY_PRICE.DecimalPlaces = m_ROUTE_DETAIL.DECIMAL_PLACES

        se_FFA_PRICE.Increment = m_ROUTE_DETAIL.PRICING_TICK
        se_STRIKE1.Increment = m_ROUTE_DETAIL.PRICING_TICK
        se_STRIKE2.Increment = m_ROUTE_DETAIL.PRICING_TICK
        se_AVG_PRICE.Increment = m_ROUTE_DETAIL.PRICING_TICK
        se_OPTION_PRICE1.Increment = m_ROUTE_DETAIL.PRICING_TICK
        se_OPTION_PRICE2.Increment = m_ROUTE_DETAIL.PRICING_TICK
        se_STRATEGY_PRICE.Increment = m_ROUTE_DETAIL.PRICING_TICK

        rgv_PERIODS.Rows(1).IsCurrent = True
        rgv_PERIODS.Rows(1).IsSelected = True
        
    End Sub
    Public Sub PopulateGraph(ByVal s_YY1 As Integer, ByVal s_MM1 As Integer, ByVal s_YY2 As Integer, ByVal s_MM2 As Integer)

        For Each s In MSChart.Series
            s.Enabled = False
            s.Points.Clear()
        Next

        Try
            Dim spot = (From q In FIXINGS Where q.ROUTE_ID = Me.m_ROUTE_ID _
                        And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot Order By q.FIXING_DATE Select q)
            If IsNothing(spot) Then
                MSChart.Series("Spot").Enabled = False
                rcb_Spot.Enabled = False
                MSChart.Series("SpotAvg").Enabled = False
                rcb_SpotAvg.Enabled = False
                MSChart.Series("SpotHVol").Enabled = False
                rcb_SpotHVol.Enabled = False
                rcb_SpotHVol.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off
            Else
                For Each d In spot
                    MSChart.Series("Spot").Points.AddXY(d.FIXING_DATE, d.SPOT_PRICE)
                    MSChart.Series("Spot").Points(MSChart.Series("Spot").Points.Count - 1).ToolTip = FormatDateTime(d.FIXING_DATE, DateFormat.GeneralDate) & vbCrLf & FormatPriceTick(m_ROUTE_DETAIL.PRICING_TICK, d.SPOT_PRICE)
                    MSChart.Series("SpotAvg").Points.AddXY(d.FIXING_DATE, d.FFA_PRICE)
                    MSChart.Series("SpotAvg").Points(MSChart.Series("SpotAvg").Points.Count - 1).ToolTip = FormatDateTime(d.FIXING_DATE, DateFormat.GeneralDate) & vbCrLf & FormatPriceTick(m_ROUTE_DETAIL.PRICING_TICK, d.FFA_PRICE)
                    MSChart.Series("SpotHVol").Points.AddXY(d.FIXING_DATE, d.HVOL / 100)
                    MSChart.Series("SpotHVol").Points(MSChart.Series("SpotHVol").Points.Count - 1).ToolTip = FormatDateTime(d.FIXING_DATE, DateFormat.GeneralDate) & vbCrLf & FormatPercent(d.HVOL, 2)
                Next
            End If

            If s_YY1 = 0 And s_MM1 = 0 And s_YY2 = 0 And s_MM2 = 0 Then
                rcb_FFAHVol.Enabled = False
                rcb_FFAIVol.Enabled = False
                rcb_FFA.Enabled = False

                MSChart.Series("FFA").Enabled = False
                MSChart.Series("FFAHVol").Enabled = False
                MSChart.Series("FFAIVol").Enabled = False
            Else
                rcb_FFAHVol.Enabled = True
                rcb_FFAIVol.Enabled = True
                rcb_FFA.Enabled = True

                Dim FFA = (From q In FIXINGS Where q.ROUTE_ID = Me.m_ROUTE_ID _
                           And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap) _
                           And q.YY1 = s_YY1 And q.MM1 = s_MM1 And q.YY2 = s_YY2 And q.MM2 = s_MM2 _
                           Order By q.FIXING_DATE Select q)
                For Each f In FFA
                    MSChart.Series("FFA").Points.AddXY(f.FIXING_DATE, f.FFA_PRICE)
                    MSChart.Series("FFA").Points(MSChart.Series("FFA").Points.Count - 1).ToolTip = FormatDateTime(f.FIXING_DATE, DateFormat.GeneralDate) & vbCrLf & FormatPriceTick(m_ROUTE_DETAIL.PRICING_TICK, f.FFA_PRICE)
                    MSChart.Series("FFAHVol").Points.AddXY(f.FIXING_DATE, f.HVOL / 100)
                    MSChart.Series("FFAHVol").Points(MSChart.Series("FFAHVol").Points.Count - 1).ToolTip = FormatDateTime(f.FIXING_DATE, DateFormat.GeneralDate) & vbCrLf & FormatPercent(f.HVOL, 2)
                    MSChart.Series("FFAIVol").Points.AddXY(f.FIXING_DATE, f.IVOL / 100)
                    MSChart.Series("FFAIVol").Points(MSChart.Series("FFAIVol").Points.Count - 1).ToolTip = FormatDateTime(f.FIXING_DATE, DateFormat.GeneralDate) & vbCrLf & FormatPercent(f.IVOL, 2)
                Next
            End If

            If rcb_FFA.Enabled = True And rcb_FFA.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MSChart.Series("FFA").Enabled = True
            End If
            If rcb_FFAIVol.Enabled = True And rcb_FFAIVol.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MSChart.Series("FFAIVol").Enabled = True
            End If
            If rcb_FFAHVol.Enabled = True And rcb_FFAHVol.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MSChart.Series("FFAHVol").Enabled = True
            End If

            If rcb_SpotHVol.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MSChart.Series("SpotHVol").Enabled = True
            End If
            If rcb_Spot.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MSChart.Series("Spot").Enabled = True
            End If
            If rcb_SpotAvg.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                MSChart.Series("SpotAvg").Enabled = True
            End If

            If FirstTime = True Then
                FirstTime = False
                MSChart.Series("FFAIVol").Enabled = False
            End If
            MSChart.ChartAreas("Default").RecalculateAxesScale()
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
    End Sub

#Region "BackGroundWorker"
    Private Sub OptionWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        If worker.CancellationPending = True Then Exit Sub

        Dim ErrorLeg(1) As Boolean
        Dim StartLeg As Integer = e.Argument(0)
        Dim EndLeg As Integer = e.Argument(1)
        Dim SolveFor As OptionSolveForEnum = e.Argument(2)
        Dim ActiveLeg As Integer
        Dim result(1) As Double

        For ActiveLeg = StartLeg To EndLeg
            result(ActiveLeg) = FFA(ActiveLeg).Solve(FFA(ActiveLeg).SolveFor, FFA(ActiveLeg).Solver, FFA(ActiveLeg).pMax)
            If result(ActiveLeg) = -1.0# Then
                ErrorLeg(ActiveLeg) = True
            End If
        Next
        If ErrorLeg(0) = True Or ErrorLeg(1) = True Then
            e.Result = {-1.0#, ErrorLeg(0), ErrorLeg(1)}
        Else
            e.Result = {StartLeg, EndLeg, SolveFor}
        End If
    End Sub
    Private Sub pb_NVIDIA_DoubleClick(sender As Object, e As EventArgs) Handles pb_NVIDIA.DoubleClick
        OptionWorker.CancelAsync()
        Me.Cursor = Cursors.Default
        Me.rgb_Swap_Periods.Enabled = True
        Me.rgb_Graphical_Data.Enabled = True
        Me.rgb_SwapDetails.Enabled = True
        Me.rgb_Leg1.Enabled = True
        Me.rgb_Leg2.Enabled = True
        Me.rgb_StrategyResults.Enabled = True
    End Sub
    Private Sub OptionWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        If e.Cancelled = True Then
            Exit Sub
        End If
        Me.Cursor = Cursors.Default
        Me.rgb_Swap_Periods.Enabled = True
        Me.rgb_Graphical_Data.Enabled = True
        Me.rgb_SwapDetails.Enabled = True
        Me.rgb_Leg1.Enabled = True
        Me.rgb_Leg2.Enabled = True
        Me.rgb_StrategyResults.Enabled = True

        If e.Result(0) = -1.0# Then
            Dim ErrorLeg(1) As Boolean
            ErrorLeg(0) = e.Result(1)
            ErrorLeg(1) = e.Result(2)
            Dim msg As String = String.Empty
            If ErrorLeg(0) And ErrorLeg(1) Then
                msg = "An Error was encountered while calculating both legs. Please disregard results"
            ElseIf ErrorLeg(0) Then
                msg = "An Error was encountered while calculating leg->1. Please disregard results"
            ElseIf ErrorLeg(1) And ErrorLeg(1) Then
                msg = "An Error was encountered while calculating leg->2. Please disregard results"
            End If
            MsgError(Me, msg, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        Dim StartLeg As Integer = e.Result(0)
        Dim EndLeg As Integer = e.Result(1)
        Dim SolveFor As OptionSolveForEnum = e.Result(2)
        Dim ActiveLeg As Integer

        For ActiveLeg = StartLeg To EndLeg
            If SolveFor = OptionSolveForEnum.Price Then
                se_PRICES(ActiveLeg).Value = FFA(ActiveLeg).OD.OptionValue
            ElseIf SolveFor = OptionSolveForEnum.Vol Then
                se_VOLS(ActiveLeg).Value = FFA(ActiveLeg).OD.Volatility * 100
            ElseIf SolveFor = OptionSolveForEnum.Strike Then
                se_STRIKES(ActiveLeg).Value = FFA(ActiveLeg).OD.StrikePrice
            End If
            VBS(ActiveLeg).Tag = FFA(ActiveLeg).OD.OptionBS
            se_PAYRECEIVES(ActiveLeg).Value = FFA(ActiveLeg).OD.PayReceive
            se_DELTAS(ActiveLeg).Value = Math.Abs(FFA(ActiveLeg).OD.Delta) * VBS(ActiveLeg).SelectedValue * VOT(ActiveLeg).SelectedValue
            se_DELTAS(ActiveLeg).Tag = Math.Abs(FFA(ActiveLeg).OD.Delta)
            se_GAMMAS(ActiveLeg).Value = FFA(ActiveLeg).OD.Gamma * VBS(ActiveLeg).SelectedValue
            se_GAMMAS(ActiveLeg).Tag = se_GAMMAS(ActiveLeg).Value
            se_RHOS(ActiveLeg).Value = FFA(ActiveLeg).OD.Rho * VBS(ActiveLeg).SelectedValue
            se_RHOS(ActiveLeg).Tag = se_RHOS(ActiveLeg).Value
            se_THETAS(ActiveLeg).Value = FFA(ActiveLeg).OD.Theta * VBS(ActiveLeg).SelectedValue
            se_THETAS(ActiveLeg).Tag = se_THETAS(ActiveLeg).Value
            se_VEGAS(ActiveLeg).Value = FFA(ActiveLeg).OD.Vega * VBS(ActiveLeg).SelectedValue
            se_VEGAS(ActiveLeg).Tag = se_VEGAS(ActiveLeg).Value
        Next
        CalcStrategyValue()

        Me.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub btn_PRICE1_Click(sender As Object, e As EventArgs) Handles btn_PRICE1.Click, btn_PRICE2.Click, btn_PRICE.Click
        If OptionWorker.IsBusy Then
            OptionWorker.CancelAsync()
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        Dim ActiveLeg As Integer = 0
        Dim StartLeg As Integer
        Dim EndLeg As Integer

        Dim who As RadButton = DirectCast(sender, RadButton)
        Select Case who.Name
            Case "btn_PRICE1"
                StartLeg = 0
                EndLeg = 0
            Case "btn_PRICE2"
                StartLeg = 1
                EndLeg = 1
            Case "btn_PRICE"
                StartLeg = 0
                EndLeg = 1
        End Select

        'do some error checking
        If cb_YY2.SelectedValue < cb_YY1.SelectedValue Then
            Beep()
            MsgError(Me, "End Year of FFA month strip cannot be less than start year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        If cb_YY2.SelectedValue = cb_YY1.SelectedValue And cb_MM2.SelectedValue < cb_MM1.SelectedValue Then
            Beep()
            MsgError(Me, "Starting month cannot be less than ending month of FFA month strip", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        Dim tStartDate As Date = DateSerial(cb_YY1.SelectedValue, cb_MM1.SelectedValue, 1)
        Dim tEndDate As Date = DateSerial(cb_YY2.SelectedValue, cb_MM2.SelectedValue, 1)
        If tStartDate < DateSerial(SERVER_DATE.Year, SERVER_DATE.Month, 1) Then
            Beep()
            MsgError(Me, "Starting month/year cannot be less than current month/year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        If tEndDate < DateSerial(SERVER_DATE.Year, SERVER_DATE.Month, 1) Then
            Beep()
            MsgError(Me, "Ending month/year cannot be less than current month/year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        If se_FFA_PRICE.Value <= 0 Then
            Beep()
            MsgError(Me, "FFA price cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        For ActiveLeg = StartLeg To EndLeg
            If se_STRIKES(ActiveLeg).Value <= 0 Then
                Beep()
                MsgError(Me, "Strike price " & "for Leg->" & ActiveLeg + 1 & " cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
            If se_VOLS(ActiveLeg).Value <= 0 Then
                Beep()
                MsgError(Me, "Volatility " & "for Leg->" & ActiveLeg + 1 & "  cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
        Next

        Me.Cursor = Cursors.WaitCursor
        Me.rgb_Swap_Periods.Enabled = False
        Me.rgb_Graphical_Data.Enabled = False
        Me.rgb_SwapDetails.Enabled = False
        Me.rgb_Leg1.Enabled = False
        Me.rgb_Leg2.Enabled = False
        Me.rgb_StrategyResults.Enabled = False

        OptionWorker = New BackgroundWorker
        OptionWorker.WorkerSupportsCancellation = True
        AddHandler OptionWorker.DoWork, AddressOf OptionWorker_DoWork
        AddHandler OptionWorker.RunWorkerCompleted, AddressOf OptionWorker_RunWorkerCompleted

        For ActiveLeg = StartLeg To EndLeg
            FFA(ActiveLeg) = LoadFFAClass(ActiveLeg, OptionSolveForEnum.Price)
            se_PRICES(ActiveLeg).Value = 0
            se_PAYRECEIVES(ActiveLeg).Value = 0
            se_DELTAS(ActiveLeg).Value = 0
            se_GAMMAS(ActiveLeg).Value = 0
            se_RHOS(ActiveLeg).Value = 0
            se_THETAS(ActiveLeg).Value = 0
            se_VEGAS(ActiveLeg).Value = 0
        Next
        se_STRATEGY_PRICE.Value = 0
        se_PAYRECEIVE.Value = 0
        se_DELTA.Value = 0

        Dim arg() As Integer = {StartLeg, EndLeg, OptionSolveForEnum.Price}
        OptionWorker.RunWorkerAsync(arg)
    End Sub
    Private Sub btn_VOL1_Click(sender As Object, e As EventArgs) Handles btn_VOL1.Click, btn_VOL2.Click
        If OptionWorker.IsBusy Then
            OptionWorker.CancelAsync()
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        Dim ActiveLeg As Integer
        Dim StartLeg As Integer
        Dim EndLeg As Integer

        Dim who As RadButton = DirectCast(sender, RadButton)
        Select Case who.Name
            Case "btn_VOL1"
                StartLeg = 0
                EndLeg = 0
            Case "btn_VOL2"
                StartLeg = 1
                EndLeg = 1
        End Select

        'do some error checking
        If cb_YY2.SelectedValue < cb_YY1.SelectedValue Then
            Beep()
            MsgError(Me, "End Year of FFA month strip cannot be less than start year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        If cb_YY2.SelectedValue = cb_YY1.SelectedValue And cb_MM2.SelectedValue < cb_MM1.SelectedValue Then
            Beep()
            MsgError(Me, "Starting month cannot be less than ending month of FFA month strip", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        If se_FFA_PRICE.Value <= 0 Then
            Beep()
            MsgError(Me, "FFA price cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        Dim tStartDate As Date = DateSerial(cb_YY1.SelectedValue, cb_MM1.SelectedValue, 1)
        Dim tEndDate As Date = DateSerial(cb_YY2.SelectedValue, cb_MM2.SelectedValue, 1)
        If tStartDate < DateSerial(SERVER_DATE.Year, SERVER_DATE.Month, 1) Then
            Beep()
            MsgError(Me, "Starting month/year cannot be less than current month/year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        If tEndDate < DateSerial(SERVER_DATE.Year, SERVER_DATE.Month, 1) Then
            Beep()
            MsgError(Me, "Ending month/year cannot be less than current month/year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        For ActiveLeg = StartLeg To EndLeg
            If se_STRIKES(ActiveLeg).Value <= 0 Then
                Beep()
                MsgError(Me, "Strike price Leg->" & ActiveLeg + 1 & "  cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
            If se_PRICES(ActiveLeg).Value <= 0 Then
                Beep()
                MsgError(Me, "Option Price Leg->" & ActiveLeg + 1 & "  cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
        Next

        Me.Cursor = Cursors.WaitCursor
        Me.rgb_Swap_Periods.Enabled = False
        Me.rgb_Graphical_Data.Enabled = False
        Me.rgb_SwapDetails.Enabled = False
        Me.rgb_Leg1.Enabled = False
        Me.rgb_Leg2.Enabled = False
        Me.rgb_StrategyResults.Enabled = False

        OptionWorker = New BackgroundWorker
        OptionWorker.WorkerSupportsCancellation = True
        AddHandler OptionWorker.DoWork, AddressOf OptionWorker_DoWork
        AddHandler OptionWorker.RunWorkerCompleted, AddressOf OptionWorker_RunWorkerCompleted

        For ActiveLeg = StartLeg To EndLeg
            FFA(ActiveLeg) = LoadFFAClass(ActiveLeg, OptionSolveForEnum.Vol)
            se_VOLS(ActiveLeg).Value = 0
            se_PAYRECEIVES(ActiveLeg).Value = 0
            se_DELTAS(ActiveLeg).Value = 0
            se_GAMMAS(ActiveLeg).Value = 0
            se_RHOS(ActiveLeg).Value = 0
            se_THETAS(ActiveLeg).Value = 0
            se_VEGAS(ActiveLeg).Value = 0
        Next
        se_STRATEGY_PRICE.Value = 0
        se_PAYRECEIVE.Value = 0
        se_DELTA.Value = 0

        Dim arg() As Integer = {StartLeg, EndLeg, OptionSolveForEnum.Vol}
        OptionWorker.RunWorkerAsync(arg)
    End Sub
    Private Sub btn_STRIKE1_Click(sender As Object, e As EventArgs) Handles btn_STRIKE1.Click, btn_STRIKE2.Click
        If OptionWorker.IsBusy Then
            OptionWorker.CancelAsync()
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        Dim ActiveLeg As Integer
        Dim StartLeg As Integer
        Dim EndLeg As Integer
        Dim who As RadButton = DirectCast(sender, RadButton)
        Select Case who.Name
            Case "btn_STRIKE1"
                StartLeg = 0
                EndLeg = 0
            Case "btn_STRIKE2"
                StartLeg = 1
                EndLeg = 1
        End Select

        'do some error checking
        If cb_YY2.SelectedValue < cb_YY1.SelectedValue Then
            Beep()
            MsgError(Me, "End Year of FFA month strip cannot be less than start year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        If cb_YY2.SelectedValue = cb_YY1.SelectedValue And cb_MM2.SelectedValue < cb_MM1.SelectedValue Then
            Beep()
            MsgError(Me, "Starting month cannot be less than ending month of FFA month strip", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        Dim tStartDate As Date = DateSerial(cb_YY1.SelectedValue, cb_MM1.SelectedValue, 1)
        Dim tEndDate As Date = DateSerial(cb_YY2.SelectedValue, cb_MM2.SelectedValue, 1)
        If tStartDate < DateSerial(SERVER_DATE.Year, SERVER_DATE.Month, 1) Then
            Beep()
            MsgError(Me, "Starting month/year cannot be less than current month/year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        If tEndDate < DateSerial(SERVER_DATE.Year, SERVER_DATE.Month, 1) Then
            Beep()
            MsgError(Me, "Ending month/year cannot be less than current month/year", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        If se_FFA_PRICE.Value <= 0 Then
            Beep()
            MsgError(Me, "FFA price cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        For ActiveLeg = StartLeg To EndLeg
            If se_PRICES(ActiveLeg).Value <= 0 Then
                Beep()
                MsgError(Me, "Option Price for Leg->" & ActiveLeg + 1 & " cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
            If se_VOLS(ActiveLeg).Value <= 0 Then
                Beep()
                MsgError(Me, "Volatility  Leg->" & ActiveLeg + 1 & " cannot be zero!", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
        Next

        Me.Cursor = Cursors.WaitCursor
        Me.rgb_Graphical_Data.Enabled = False
        Me.rgb_SwapDetails.Enabled = False
        Me.rgb_Leg1.Enabled = False
        Me.rgb_Leg2.Enabled = False
        Me.rgb_StrategyResults.Enabled = False

        OptionWorker = New BackgroundWorker
        OptionWorker.WorkerSupportsCancellation = True
        AddHandler OptionWorker.DoWork, AddressOf OptionWorker_DoWork
        AddHandler OptionWorker.RunWorkerCompleted, AddressOf OptionWorker_RunWorkerCompleted

        For ActiveLeg = StartLeg To EndLeg
            FFA(ActiveLeg) = LoadFFAClass(ActiveLeg, OptionSolveForEnum.Strike)
            se_STRIKES(ActiveLeg).Value = 0
            se_PAYRECEIVES(ActiveLeg).Value = 0
            se_DELTAS(ActiveLeg).Value = 0
            se_GAMMAS(ActiveLeg).Value = 0
            se_RHOS(ActiveLeg).Value = 0
            se_THETAS(ActiveLeg).Value = 0
            se_VEGAS(ActiveLeg).Value = 0
        Next
        se_STRATEGY_PRICE.Value = 0
        se_PAYRECEIVE.Value = 0
        se_DELTA.Value = 0

        Dim arg() As Integer = {StartLeg, EndLeg, OptionSolveForEnum.Strike}
        OptionWorker.RunWorkerAsync(arg)
    End Sub

    Private Function LoadFFAClass(ByVal Leg As Integer, ByVal SolveFor As OptionSolveForEnum) As FFAOptionSolveClass
        Dim f_FFA As New FFAOptionSolveClass

        f_FFA.LegNo = Leg
        f_FFA.pMax = MC_PMAX.Value
        f_FFA.EpsilonPerc = 0.01
        If rbtn_ANALYTIC.IsChecked Then f_FFA.Solver = OptionSolverEnum.Analytic
        If rbtn_MC_CUDA.IsChecked Then
            f_FFA.Solver = OptionSolverEnum.MC_CUDA
            f_FFA.pMax = MC_PMAX.Value
        End If

        f_FFA.SolveFor = SolveFor
        f_FFA.OD.OptionRefID = Rand.Next(50, 999999999)
        f_FFA.OD.RouteId = m_ROUTE_ID
        f_FFA.OD.TradeDate = SERVER_DATE
        f_FFA.OD.Holidays = PUBLIC_HOLIDAYS
        f_FFA.OD.OptionType = VOT(Leg).SelectedValue
        f_FFA.OD.OptionBS = VBS(Leg).SelectedValue
        f_FFA.OD.Quantity = se_QUANT(Leg).Value
        f_FFA.OD.StrikePrice = Math.Abs(se_STRIKES(Leg).Value)
        f_FFA.OD.PricingTick = se_FFA_PRICE.Increment
        f_FFA.OD.Volatility = se_VOLS(Leg).Value / 100
        f_FFA.OD.Skew = se_SKEWS(Leg).Value
        f_FFA.OD.OptionValue = se_PRICES(Leg).Value

        f_FFA.OD.FFAPrice = Math.Abs(se_FFA_PRICE.Value)
        f_FFA.OD.RiskFreeRate = Math.Abs(se_RISK_FREE_RATE.Value / 100)
        f_FFA.OD.AvgPrice = Math.Abs(se_AVG_PRICE.Value)
        f_FFA.OD.AvgIncludesToday = cb_AVERAGE_INCLUDES_TODAY.Checked

        f_FFA.OD.MM1 = cb_MM1.SelectedValue
        f_FFA.OD.YY1 = cb_YY1.SelectedValue
        f_FFA.OD.MM2 = cb_MM2.SelectedValue
        f_FFA.OD.YY2 = cb_YY2.SelectedValue

        Return f_FFA
    End Function
#End Region

    Private Function FormatPriceTick(ByVal Tick As Double, ByVal Price As Double) As String
        Dim rs As String = String.Empty
        Select Case Math.Log10(Tick)
            Case Is >= 0
                rs = Format(Price, "#,##")
            Case -1
                rs = Format(Price, "0.0")
            Case -2
                rs = Format(Price, "0.00")
            Case -3
                rs = Format(Price, "0.000")
            Case -4
                rs = Format(Price, "0.0000")
        End Select
        Return rs
    End Function
    Private Sub rgv_PERIODS_CurrentRowChanged(sender As Object, e As CurrentRowChangedEventArgs) Handles rgv_PERIODS.CurrentRowChanged
        If IsNothing(rgv_PERIODS.CurrentRow) Then Exit Sub
        If e.CurrentRow.Index < 0 Then Exit Sub

        Dim YY1 As Integer = rgv_PERIODS.CurrentRow.Cells("YY1").Value
        Dim MM1 As Integer = rgv_PERIODS.CurrentRow.Cells("MM1").Value
        Dim YY2 As Integer = rgv_PERIODS.CurrentRow.Cells("YY2").Value
        Dim MM2 As Integer = rgv_PERIODS.CurrentRow.Cells("MM2").Value

        PopulateGraph(YY1, MM1, YY2, MM2)
    End Sub

    Private Sub rcb_FFAIVol_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles rcb_FFAIVol.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            MSChart.Series("FFAIVol").Enabled = True
        Else
            MSChart.Series("FFAIVol").Enabled = False
        End If
        MSChart.ChartAreas("Default").RecalculateAxesScale()
    End Sub

    Private Sub rcb_FFAHVol_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles rcb_FFAHVol.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            MSChart.Series("FFAHVol").Enabled = True
        Else
            MSChart.Series("FFAHVol").Enabled = False
        End If
        MSChart.ChartAreas("Default").RecalculateAxesScale()
    End Sub

    Private Sub rcb_SpotHVol_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles rcb_SpotHVol.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            MSChart.Series("SpotHVol").Enabled = True
        Else
            MSChart.Series("SpotHVol").Enabled = False
        End If
        MSChart.ChartAreas("Default").RecalculateAxesScale()
    End Sub

    Private Sub rcb_FFA_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles rcb_FFA.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            MSChart.Series("FFA").Enabled = True
        Else
            MSChart.Series("FFA").Enabled = False
        End If
        MSChart.ChartAreas("Default").RecalculateAxesScale()
    End Sub

    Private Sub rcb_Spot_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles rcb_Spot.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            MSChart.Series("Spot").Enabled = True
        Else
            MSChart.Series("Spot").Enabled = False
        End If
        MSChart.ChartAreas("Default").RecalculateAxesScale()
    End Sub

    Private Sub rcb_SpotAvg_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles rcb_SpotAvg.ToggleStateChanged
        If args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            MSChart.Series("SpotAvg").Enabled = True
        Else
            MSChart.Series("SpotAvg").Enabled = False
        End If
        MSChart.ChartAreas("Default").RecalculateAxesScale()
    End Sub

    Private Sub rgv_PERIODS_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles rgv_PERIODS.ViewCellFormatting
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
    Private Sub EventForm_AmmededTradeReceived(sender As Object, AmmededTrade As FFAOptCalcService.VolDataClass) Handles EventForm.AmmededTradeReceived
        If AmmededTrade.ROUTE_ID <> m_ROUTE_ID Then Exit Sub
        If My.Settings.UpdateCalcLive = False Then Exit Sub

        Dim RecFound As Boolean = False
        Dim CurrentIndex As Integer = rgv_PERIODS.CurrentRow.Index
        For Each row In m_GRIDDATA
            If row.YY1 = AmmededTrade.YY1 And row.YY2 = AmmededTrade.YY2 And row.MM1 = AmmededTrade.MM1 And row.MM2 = AmmededTrade.MM2 Then
                If row.TRADE_ID = AmmededTrade.TRADE_ID Then
                    row.FFA_PRICE = AmmededTrade.FFA_PRICE
                    row.LIVE_DATA = True
                    row.VolRecordType = AmmededTrade.VolRecordType
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
    Private Sub EventForm_SpotRatesUpdated() Handles EventForm.SpotRatesUpdated
        Dim CurrentIndex As Integer = rgv_PERIODS.CurrentRow.Index
        Dim RecFound As Boolean = False

        m_ROUTE_DETAIL = (From q In ROUTES_DETAIL Where q.ROUTE_ID = m_ROUTE_ID Select q).FirstOrDefault

        se_AVG_PRICE.Value = m_ROUTE_DETAIL.FIXING_AVG
        If m_ROUTE_DETAIL.AVERAGE_INCLUDES_TODAY = True Then
            cb_AVERAGE_INCLUDES_TODAY.CheckState = CheckState.Checked
        Else
            cb_AVERAGE_INCLUDES_TODAY.CheckState = CheckState.Unchecked
        End If

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

    Private Sub rgv_PERIODS_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles rgv_PERIODS.CellDoubleClick
        If IsNothing(rgv_PERIODS.CurrentRow) Then Exit Sub
        If e.RowIndex = 0 Then Exit Sub

        Dim RI As Integer = e.RowIndex
        rgv_PERIODS.Rows(RI).IsCurrent = True
        rgv_PERIODS.Rows(RI).IsSelected = True

        cb_MM1.SelectedIndex = rgv_PERIODS.Rows(RI).Cells("MM1").Value - 1
        cb_YY1.SelectedValue = rgv_PERIODS.Rows(RI).Cells("YY1").Value
        cb_MM2.SelectedIndex = rgv_PERIODS.Rows(RI).Cells("MM2").Value - 1
        cb_YY2.SelectedValue = rgv_PERIODS.Rows(RI).Cells("YY2").Value
        se_RISK_FREE_RATE.Value = rgv_PERIODS.Rows(RI).Cells("INTEREST_RATE").Value
        se_VOLATILITY.Value = rgv_PERIODS.Rows(RI).Cells("IVOL").Value
        se_FFA_PRICE.Value = rgv_PERIODS.Rows(RI).Cells("FFA_PRICE").Value
        se_AVG_PRICE.Value = m_ROUTE_DETAIL.FIXING_AVG
        If m_ROUTE_DETAIL.AVERAGE_INCLUDES_TODAY = True Then
            cb_AVERAGE_INCLUDES_TODAY.CheckState = CheckState.Checked
        Else
            cb_AVERAGE_INCLUDES_TODAY.CheckState = CheckState.Unchecked
        End If
    End Sub

    Private Sub CalcStrategyValue()
        If FirstTime Then Exit Sub
        If IsNothing(cb_YY1) Then Exit Sub
        If IsNothing(cb_MM1) Then Exit Sub
        If IsNothing(cb_YY2) Then Exit Sub
        If IsNothing(cb_MM2) Then Exit Sub

        Try
            Dim nmonths As Integer = FFANoMonths(cb_YY1.SelectedValue, cb_MM1.SelectedValue, cb_YY2.SelectedValue, cb_MM2.SelectedValue)
            Dim Q(1) As Integer
            Q(0) = se_QUANT(0).Value
            If Q(0) < 0 Then
                Q(0) = FFANoDays(cb_YY1.SelectedValue, cb_MM1.SelectedValue, cb_YY2.SelectedValue, cb_MM2.SelectedValue) * Math.Abs(Q(0))
            Else
                Q(0) = se_QUANT(0).Value * nmonths
            End If

            Q(1) = se_QUANT(1).Value
            If Q(1) < 0 Then
                Q(1) = FFANoDays(cb_YY1.SelectedValue, cb_MM1.SelectedValue, cb_YY2.SelectedValue, cb_MM2.SelectedValue) * Math.Abs(Q(1))
            Else
                Q(1) = se_QUANT(1).Value * nmonths
            End If

            For ActiveLeg = 0 To 1
                se_PAYRECEIVES(ActiveLeg).Value = -se_PRICES(ActiveLeg).Value * Q(ActiveLeg) * VBS(ActiveLeg).SelectedValue
                se_DELTAS(ActiveLeg).Value = CDbl(se_DELTAS(ActiveLeg).Tag) * VBS(ActiveLeg).SelectedValue * VOT(ActiveLeg).SelectedValue
                se_GAMMAS(ActiveLeg).Value = CDbl(se_GAMMAS(ActiveLeg).Tag) * VBS(ActiveLeg).SelectedValue
                se_RHOS(ActiveLeg).Value = CDbl(se_RHOS(ActiveLeg).Tag) * VBS(ActiveLeg).SelectedValue
                se_THETAS(ActiveLeg).Value = CDbl(se_THETAS(ActiveLeg).Tag) * VBS(ActiveLeg).SelectedValue
                se_VEGAS(ActiveLeg).Value = CDbl(se_VEGAS(ActiveLeg).Tag) * VBS(ActiveLeg).SelectedValue
            Next
            se_STRATEGY_PRICE.Value = (-se_PRICES(0).Value * VBS(0).SelectedValue - se_PRICES(1).Value * VBS(1).SelectedValue) * Q(0) / Q(1)
            se_PAYRECEIVE.Value = se_PAYRECEIVES(0).Value + se_PAYRECEIVES(1).Value

            If se_QUANT(0).Value >= 0 And se_QUANT(1).Value >= 0 Then
                se_DELTA.Value = se_DELTAS(0).Value * se_QUANT(0).Value + se_DELTAS(1).Value * se_QUANT(1).Value
            ElseIf se_QUANT(0).Value < 0 And se_QUANT(1).Value < 0 Then
                se_DELTA.Value = se_DELTAS(0).Value * Q(0) * Math.Abs(se_QUANT(0).Value) / nmonths + se_DELTAS(1).Value * Q(1) * Math.Abs(se_QUANT(1).Value) / nmonths
            ElseIf se_QUANT(0).Value < 0 And se_QUANT(1).Value >= 0 Then
                se_DELTA.Value = se_DELTAS(0).Value * Q(0) * Math.Abs(se_QUANT(0).Value) / nmonths + se_DELTAS(1).Value * se_QUANT(1).Value
            ElseIf se_QUANT(0).Value >= 0 And se_QUANT(1).Value < 0 Then
                se_DELTA.Value = se_DELTAS(0).Value * se_QUANT(0).Value + se_DELTAS(1).Value * Q(1) * Math.Abs(se_QUANT(1).Value) / nmonths
            End If

        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
    End Sub

    Private Sub se_VOLATILITY_ValueChanged(sender As Object, e As EventArgs) Handles se_VOLATILITY.ValueChanged
        se_VOL1_flag = True
        se_VOL2_flag = True
        se_SKEW1_flag = True
        se_SKEW2_flag = True
        For I = 0 To 1
            se_VOLS(I).Value = se_VOLATILITY.Value
            se_SKEWS(I).Value = 0.0#
        Next
        se_VOL1_flag = False
        se_VOL2_flag = False
        se_SKEW1_flag = False
        se_SKEW2_flag = False
    End Sub

    Private Sub se_VOLATILITY1_ValueChanged(sender As Object, e As EventArgs) Handles se_VOLATILITY1.ValueChanged
        If se_SKEW1_flag = True Then Exit Sub

        se_VOL1_flag = True
        Try
            se_SKEW1.Value = se_VOLATILITY1.Value - se_VOLATILITY.Value
        Catch ex As Exception
        End Try
        se_VOL1_flag = False
    End Sub

    Private Sub se_VOLATILITY2_ValueChanged(sender As Object, e As EventArgs) Handles se_VOLATILITY2.ValueChanged
        If se_SKEW2_flag = True Then Exit Sub

        se_VOL2_flag = True
        Try
            se_SKEW2.Value = se_VOLATILITY2.Value - se_VOLATILITY.Value
        Catch ex As Exception
        End Try
        se_VOL2_flag = False
    End Sub

    Private Sub se_SKEW1_ValueChanged(sender As Object, e As EventArgs) Handles se_SKEW1.ValueChanged
        If se_VOL1_flag = True Then Exit Sub

        se_SKEW1_flag = True
        Try
            se_VOLATILITY1.Value = se_VOLATILITY.Value + se_SKEW1.Value
        Catch ex As Exception
        End Try
        se_SKEW1_flag = False
    End Sub

    Private Sub se_SKEW2_ValueChanged(sender As Object, e As EventArgs) Handles se_SKEW2.ValueChanged
        If se_VOL2_flag = True Then Exit Sub

        se_SKEW2_flag = True
        Try
            se_VOLATILITY2.Value = se_VOLATILITY.Value + se_SKEW2.Value
        Catch ex As Exception
        End Try
        se_SKEW2_flag = False
    End Sub

    Private Sub se_OPTION_PRICE1_ValueChanged(sender As Object, e As EventArgs) Handles se_OPTION_PRICE1.ValueChanged, se_OPTION_PRICE2.ValueChanged
        CalcStrategyValue()
    End Sub

    Private Sub se_QUANTITY1_ValueChanged(sender As Object, e As EventArgs) Handles se_QUANTITY1.ValueChanged, se_QUANTITY2.ValueChanged
        CalcStrategyValue()
    End Sub

    Private Sub cb_BS1_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles cb_BS1.SelectedIndexChanged, cb_BS2.SelectedIndexChanged
        If cb_BS1.SelectedIndex < 0 Then Exit Sub
        If cb_BS2.SelectedIndex < 0 Then Exit Sub
        CalcStrategyValue()
    End Sub

    Private Sub cb_OPTION_TYPE1_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles cb_OPTION_TYPE1.SelectedIndexChanged
        If cb_OPTION_TYPE1.SelectedIndex < 0 Then Exit Sub
        If cb_OPTION_TYPE2.SelectedIndex < 0 Then Exit Sub

        Dim who As RadDropDownList = DirectCast(sender, RadDropDownList)
        Dim StartLeg As Integer, EndLeg As Integer
        Dim ActiveLeg As Integer
        Select Case who.Name
            Case "cb_OPTION_TYPE1"
                StartLeg = 0
                EndLeg = 0
            Case "cb_OPTION_TYPE2"
                StartLeg = 1
                EndLeg = 1
        End Select
        For ActiveLeg = StartLeg To EndLeg
            se_PRICES(ActiveLeg).Value = 0
            VBS(ActiveLeg).Tag = VBS(ActiveLeg).SelectedValue
            se_DELTAS(ActiveLeg).Value = 0 : se_DELTAS(ActiveLeg).Tag = 0
            se_VEGAS(ActiveLeg).Value = 0 : se_VEGAS(ActiveLeg).Tag = 0
            se_THETAS(ActiveLeg).Value = 0 : se_THETAS(ActiveLeg).Tag = 0
            se_RHOS(ActiveLeg).Value = 0 : se_RHOS(ActiveLeg).Tag = 0
            se_GAMMAS(ActiveLeg).Value = 0 : se_GAMMAS(ActiveLeg).Tag = 0
        Next
        CalcStrategyValue()
    End Sub

    Private Sub cb_MM1_SelectedValueChanged(sender As Object, e As EventArgs) Handles cb_MM1.SelectedValueChanged, cb_MM2.SelectedValueChanged, cb_YY1.SelectedValueChanged, cb_YY2.SelectedValueChanged
        If FirstTime Then Exit Sub
        If cb_MM1.SelectedIndex < 0 Then Exit Sub
        If cb_MM2.SelectedIndex < 0 Then Exit Sub
        If cb_YY1.SelectedIndex < 0 Then Exit Sub
        If cb_YY2.SelectedIndex < 0 Then Exit Sub


        Dim MM1 As MMonths = DirectCast(cb_MM1.SelectedItem.DataBoundItem, MMonths)
        Dim MM2 As MMonths = DirectCast(cb_MM2.SelectedItem.DataBoundItem, MMonths)
        Dim YY1 As YYears = DirectCast(cb_YY1.SelectedItem.DataBoundItem, YYears)
        Dim YY2 As YYears = DirectCast(cb_YY2.SelectedItem.DataBoundItem, YYears)

        Dim d1 As Date = DateSerial(YY1.YY, MM1.MM, 1)
        Dim d2 As Date = DateSerial(YY2.YY, MM2.MM, 1)
        If d2 < d1 Then
            Exit Sub
        End If
        se_FFA_PRICE.Value = SwapFixing(YY1.YY, MM1.MM, YY2.YY, MM2.MM)
        se_VOLATILITY.Value = SwapVolatility(YY1.YY, MM1.MM, YY2.YY, MM2.MM)
        se_RISK_FREE_RATE.Value = RiskFreeRate(INTEREST_RATES, YY2.YY, MM2.MM)
    End Sub

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

            While monthsrem > 0
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
    Private Function RiskFreeRate(ByVal f_FIXINGS As List(Of FFAOptCalcService.InterestRatesClass), ByVal YY As Integer, ByVal MM As Integer) As Double

        Dim nomonths As Integer = DateAndTime.DateDiff(DateInterval.Month, SERVER_DATE, DateSerial(YY, MM, 1))
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
        Dim periods As Integer = DateAndTime.DateDiff(DateInterval.Month, SERVER_DATE, DateSerial(maxperiod.YY2, maxperiod.MM2, 1)) + 1

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
            nr.PERIOD = DateAndTime.DateDiff(DateInterval.Month, SERVER_DATE, DateSerial(r.YY2, r.MM2, 1))
            VOL_FIXINGS.Add(nr)
        Next
        Dim qryvol = (From q In VOL_FIXINGS Order By q.YY2 Descending, q.MM2 Descending, q.ROUTE_ID Descending Select q).ToList
        For Each r In qryvol
            For i = r.PERIOD To (r.PERIOD - r.ROUTE_ID + 1) Step -1
                f(i) = r.FIXING
            Next
        Next

        Dim sp As Integer = DateAndTime.DateDiff(DateInterval.Month, SERVER_DATE, DateSerial(f_YY1, f_MM1, 1))
        Dim ep As Integer = DateAndTime.DateDiff(DateInterval.Month, SERVER_DATE, DateSerial(f_YY2, f_MM2, 1))
        Try
            For i = sp To ep
                SwapVolatility += f(i)
            Next
            SwapVolatility = SwapVolatility / (ep - sp + 1)
        Catch ex As Exception
            Return 0.0#
        End Try

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
            nr.FIXING = r.IVOL
            nr.YY1 = r.YY1
            nr.MM1 = r.MM1
            nr.YY2 = r.YY2
            nr.MM2 = r.MM2
            nr.PERIOD = DateAndTime.DateDiff(DateInterval.Month, SERVER_DATE, DateSerial(r.YY2, r.MM2, Date.DaysInMonth(r.YY2, r.MM2)))
            nr.KEY = Format(r.YY2, "0000") & Format(r.MM2, "00") & Format(r.YY1, "0000") & Format(r.MM1, "00")
            If VOL_FIXINGS.Contains(nr.KEY) = False Then
                VOL_FIXINGS.Add(nr, nr.KEY)
            End If
        Next

        'normalize data, covert from forward-forward to preriod t/c
        Dim qr2 = From q As BalticFTPClass In VOL_FIXINGS _
                  Order By q.YY2 Descending, q.MM2 Descending, q.YY1 Descending, q.MM1 Descending _
                  Select q
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

    Private Sub rgv_PERIODS_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles rgv_PERIODS.CellValueChanged
        m_FindVolData = New FFAOptCalcService.VolDataClass
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

        cb_MM1_SelectedValueChanged(Me, e)
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

    Private Sub rgb_Swap_Periods_Click(sender As Object, e As EventArgs) Handles rgb_Swap_Periods.Click, rgb_SwapDetails.Click
        Dim f As New WebHelpForm
        f.url = "HelpForm2.pdf"
        f.Show()
    End Sub

    
End Class
