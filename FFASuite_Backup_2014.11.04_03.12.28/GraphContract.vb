Imports Telerik.WinControls.UI

Public Class GraphContract
    Private m_IntradayExists As Boolean = False
    Private m_GraphDisplay As GraphDisplayEnum = GraphDisplayEnum.historical

    Private m_GRAPHDATA As List(Of FFAOptCalcService.VolDataClass)
    Private m_Contract As FFAOptCalcService.VolDataClass
    Public Property Contract As FFAOptCalcService.VolDataClass
        Get
            Return m_Contract
        End Get
        Set(value As FFAOptCalcService.VolDataClass)
            m_Contract = value
        End Set
    End Property
    Private m_ROUTE_DETAIL As FFAOptCalcService.SwapDataClass
    Private m_ROUTE_ID As Integer
    Private Enum GraphDisplayEnum
        historical
        live
    End Enum

    Private Sub GraphContract_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Icon = My.Resources.Stats2
        m_ROUTE_ID = Contract.ROUTE_ID
        Me.Text = Contract.PERIOD
        m_ROUTE_DETAIL = (From q In ROUTES_DETAIL Where q.ROUTE_ID = m_ROUTE_ID).FirstOrDefault
        Try
            m_GRAPHDATA = WEB.SDB.Graph(m_Contract.ROUTE_ID, m_Contract.YY1, m_Contract.MM1, m_Contract.YY2, m_Contract.MM2)
        Catch ex As Exception
            MsgError(Me, "Failed to retrieve graph data from server", "Communication Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Me.Close()
        End Try
        GraphInitialize()
        PopulateGraph()
    End Sub

    Public Sub PopulateGraph()
        Try
            Dim daily = (From q In m_GRAPHDATA Where q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
                         Order By q.FIXING_DATE Select q).ToList
            Dim intraday = (From q In m_GRAPHDATA Where q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live _
                            And q.PNC = False _
                            Order By q.FIXING_DATE Select q).ToList
            Dim LastIntraPrice As FFAOptCalcService.VolDataClass = Nothing
            Dim LastHistPrice As FFAOptCalcService.VolDataClass = Nothing

            If intraday.Count > 0 Then
                m_IntradayExists = True
                LastIntraPrice = (From q In intraday Order By q.FIXING_DATE Descending Select q).FirstOrDefault
                For Each d In intraday
                    MSChart.Series("LIVE").Points.AddXY(d.FIXING_DATE, d.FFA_PRICE)
                    MSChart.Series("LIVE").Points(MSChart.Series("LIVE").Points.Count - 1).ToolTip = FormatDateTime(d.FIXING_DATE.ToLocalTime, DateFormat.GeneralDate) & vbCrLf & FormatPriceTick(m_ROUTE_DETAIL.PRICING_TICK, d.FFA_PRICE)
                    If d.FIXING_DATE.Date >= SERVER_DATE.Date Then
                        MSChart.Series("LIVE").Points(MSChart.Series("LIVE").Points.Count - 1).MarkerColor = Color.Red
                        MSChart.Series("LIVE").Points(MSChart.Series("LIVE").Points.Count - 1).Color = Color.Red
                    End If
                Next
            Else
                m_IntradayExists = False
            End If

            If IsNothing(daily) = False Then
                LastHistPrice = (From q In daily Order By q.FIXING_DATE Descending Select q).FirstOrDefault
                MSChart.Series("FFA").Enabled = True
                For Each d In daily
                    MSChart.Series("FFA").Points.AddXY(d.FIXING_DATE, d.FFA_PRICE)
                    MSChart.Series("FFA").Points(MSChart.Series("FFA").Points.Count - 1).ToolTip = FormatDateTime(d.FIXING_DATE, DateFormat.ShortDate) & vbCrLf & FormatPriceTick(m_ROUTE_DETAIL.PRICING_TICK, d.FFA_PRICE)
                Next
                If IsNothing(LastIntraPrice) = False Then
                    If LastIntraPrice.FIXING_DATE.Date >= SERVER_DATE.Date Then
                        MSChart.Series("FFA").Points.AddXY(LastIntraPrice.FIXING_DATE.Date, LastIntraPrice.FFA_PRICE)
                        MSChart.Series("FFA").Points(MSChart.Series("FFA").Points.Count - 1).ToolTip = FormatDateTime(LastIntraPrice.FIXING_DATE.Date, DateFormat.ShortDate) & vbCrLf & FormatPriceTick(m_ROUTE_DETAIL.PRICING_TICK, LastIntraPrice.FFA_PRICE)
                        MSChart.Series("FFA").Points(MSChart.Series("FFA").Points.Count - 1).Color = Color.Red
                        MSChart.Series("FFA").Points(MSChart.Series("FFA").Points.Count - 1).MarkerStyle = DataVisualization.Charting.MarkerStyle.Circle
                        MSChart.Series("FFA").Points(MSChart.Series("FFA").Points.Count - 1).MarkerSize = 5
                    End If
                End If
            End If

        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
    End Sub

    Private Sub GraphInitialize()
        Try
            For Each s In MSChart.Series
                s.Enabled = False
            Next

            MSChart.DataManipulator.IsStartFromFirst = True
            MSChart.DataManipulator.IsEmptyPointIgnored = True
            MSChart.ChartAreas("Default").AxisY.IsStartedFromZero = False
            MSChart.ChartAreas("Default").CursorX.IsUserSelectionEnabled = True

            MSChart.ChartAreas("Default").AxisX.LabelStyle.IsEndLabelVisible = False
            MSChart.ChartAreas("Default").AxisX.IsStartedFromZero = False
            MSChart.ChartAreas("Default").AxisX.IntervalType = DataVisualization.Charting.DateTimeIntervalType.Auto
            MSChart.ChartAreas("Default").AxisX.LabelStyle.Format = "MMM-yy"
            MSChart.ChartAreas("Default").AxisX.ScaleView.Zoomable = True

            ' Locale specific percentage format with no decimals
            MSChart.ChartAreas("Default").AxisY.LabelStyle.Format = FormatPricingTick(m_ROUTE_DETAIL.PRICING_TICK)

            MSChart.Series("FFA").XValueType = DataVisualization.Charting.ChartValueType.Date
            MSChart.Series("FFA").Color = Color.DarkGreen
            MSChart.Series("FFA").BorderWidth = 1

            MSChart.Series("LIVE").XValueType = DataVisualization.Charting.ChartValueType.DateTime
            MSChart.Series("LIVE").Color = Color.DarkGreen
            MSChart.Series("LIVE").MarkerColor = Color.DarkGreen
            MSChart.Series("LIVE").MarkerStyle = DataVisualization.Charting.MarkerStyle.Circle
            MSChart.Series("LIVE").MarkerSize = 5

            MSChart.ContextMenuStrip = m_GraphContextMenu
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
    End Sub

    Private Sub rmi_Historical_Click(sender As Object, e As EventArgs) Handles rmi_Historical.Click
        m_GraphDisplay = GraphDisplayEnum.historical
        MSChart.Series("LIVE").Enabled = False
        MSChart.Series("FFA").Enabled = True
        MSChart.ChartAreas("Default").AxisX.ScaleView.ZoomReset()
        MSChart_AxisViewChanged(Me, New DataVisualization.Charting.ViewEventArgs(New Windows.Forms.DataVisualization.Charting.Axis, 0))
    End Sub

    Private Sub rmi_Live_Click(sender As Object, e As EventArgs) Handles rmi_Live.Click
        m_GraphDisplay = GraphDisplayEnum.live
        MSChart.Series("FFA").Enabled = False
        MSChart.Series("LIVE").Enabled = True
        MSChart.ChartAreas("Default").AxisX.ScaleView.ZoomReset()
        MSChart_AxisViewChanged(Me, New DataVisualization.Charting.ViewEventArgs(New Windows.Forms.DataVisualization.Charting.Axis, 0))
    End Sub

    Private Sub m_GraphContextMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles m_GraphContextMenu.Opening
        If m_GraphDisplay = GraphDisplayEnum.historical Then
            If m_IntradayExists = False Then
                e.Cancel = True
                Exit Sub
            End If
            rmi_Historical.Visible = False
            rmi_Live.Visible = True
        ElseIf m_GraphDisplay = GraphDisplayEnum.live Then
            rmi_Live.Visible = False
            rmi_Historical.Visible = True
        End If
    End Sub

    Private Function FormatPricingTick(ByVal _PRICE_TICK As Double, Optional ByVal CCY_ID As Integer = 0) As String
        Dim fs As String = ""
        If Math.Log10(_PRICE_TICK) >= 0 Then
            fs = "{0:N0}"
        ElseIf Math.Log10(_PRICE_TICK) = -1 Then
            fs = "{0:N1}"
        ElseIf Math.Log10(_PRICE_TICK) = -2 Then
            fs = "{0:N2}"
        ElseIf Math.Log10(_PRICE_TICK) = -3 Then
            fs = "{0:N3}"
        ElseIf Math.Log10(_PRICE_TICK) > -2 And Math.Log10(_PRICE_TICK) < -1 Then
            fs = "{0:N2}"
        ElseIf Math.Log10(_PRICE_TICK) > -1 And Math.Log10(_PRICE_TICK) < 0 Then
            fs = "{0:N2}"
        End If
        Return fs
    End Function

    Private Sub MSChart_AxisViewChanged(sender As Object, e As DataVisualization.Charting.ViewEventArgs) Handles MSChart.AxisViewChanged
        If m_GraphDisplay = GraphDisplayEnum.historical Then
            If MSChart.Series("FFA").Points.Count = 0 Then
                MSChart.ChartAreas("Default").AxisY.Minimum = Double.NaN
                MSChart.ChartAreas("Default").AxisY.Maximum = Double.NaN
                MSChart.ChartAreas("Default").RecalculateAxesScale()
                Exit Sub
            End If
        ElseIf m_GraphDisplay = GraphDisplayEnum.live Then
            If MSChart.Series("LIVE").Points.Count = 0 Then
                MSChart.ChartAreas("Default").AxisY.Minimum = Double.NaN
                MSChart.ChartAreas("Default").AxisY.Maximum = Double.NaN
                MSChart.ChartAreas("Default").RecalculateAxesScale()
                Exit Sub
            End If
        End If

        Try
            Dim vmin As Double = Math.Max(MSChart.ChartAreas("Default").AxisX.ScaleView.ViewMinimum - 2, 0)
            Dim vmax As Double = Math.Max(MSChart.ChartAreas("Default").AxisX.ScaleView.ViewMaximum - 2, 0)

            If m_GraphDisplay = GraphDisplayEnum.live Then
                vmin = MSChart.Series("LIVE").Points(CInt(vmin)).XValue
                vmax = MSChart.Series("LIVE").Points(CInt(vmax)).XValue
                Dim min As Double = (From q In MSChart.Series("LIVE").Points _
                                     Where q.XValue >= vmin And q.XValue <= vmax _
                                     Select q.YValues(0)).Min
                Dim max As Double = (From q In MSChart.Series("LIVE").Points _
                                     Where q.XValue >= vmin And q.XValue <= vmax _
                                     Select q.YValues(0)).Max
                MSChart.ChartAreas("Default").AxisY.Minimum = MRound(min * 0.95, m_ROUTE_DETAIL.PRICING_TICK * 20)
                MSChart.ChartAreas("Default").AxisY.Maximum = MRound(max * 1.05, m_ROUTE_DETAIL.PRICING_TICK * 20)
            ElseIf m_GraphDisplay = GraphDisplayEnum.historical Then
                Dim min As Double = (From q In MSChart.Series("FFA").Points _
                                     Where q.XValue >= vmin And q.XValue <= vmax _
                                     Select q.YValues(0)).Min
                Dim max As Double = (From q In MSChart.Series("FFA").Points _
                                     Where q.XValue >= vmin And q.XValue <= vmax _
                                     Select q.YValues(0)).Max
                MSChart.ChartAreas("Default").AxisY.Minimum = MRound(min * 0.95, m_ROUTE_DETAIL.PRICING_TICK * 20)
                MSChart.ChartAreas("Default").AxisY.Maximum = MRound(max * 1.05, m_ROUTE_DETAIL.PRICING_TICK * 20)
            End If
        Catch ex As Exception
            MSChart.ChartAreas("Default").AxisY.Maximum = Double.NaN
            MSChart.ChartAreas("Default").AxisY.Minimum = Double.NaN
            MSChart.ChartAreas("Default").RecalculateAxesScale()
        End Try
        MSChart.Update()
    End Sub

End Class
