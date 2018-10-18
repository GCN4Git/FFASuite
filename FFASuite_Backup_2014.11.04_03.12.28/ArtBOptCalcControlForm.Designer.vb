<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ArtBOptCalcControlForm
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series4 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series5 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series6 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim GridViewDecimalColumn1 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDecimalColumn2 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn3 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn4 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn5 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn6 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn7 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn8 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn9 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewCheckBoxColumn1 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewCheckBoxColumn2 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewDecimalColumn10 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn11 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rgb_Graphical_Data = New Telerik.WinControls.UI.RadGroupBox()
        Me.MSChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.rss_GRAPH = New Telerik.WinControls.UI.RadStatusStrip()
        Me.rcb_FFAIVol = New Telerik.WinControls.UI.RadCheckBoxElement()
        Me.rcb_FFAHVol = New Telerik.WinControls.UI.RadCheckBoxElement()
        Me.rcb_SpotHVol = New Telerik.WinControls.UI.RadCheckBoxElement()
        Me.CommandBarSeparator1 = New Telerik.WinControls.UI.CommandBarSeparator()
        Me.rcb_FFA = New Telerik.WinControls.UI.RadCheckBoxElement()
        Me.rcb_Spot = New Telerik.WinControls.UI.RadCheckBoxElement()
        Me.rcb_SpotAvg = New Telerik.WinControls.UI.RadCheckBoxElement()
        Me.rgb_Swap_Periods = New Telerik.WinControls.UI.RadGroupBox()
        Me.rgv_PERIODS = New Telerik.WinControls.UI.RadGridView()
        Me.rgv_BS = New System.Windows.Forms.BindingSource(Me.components)
        Me.RadThemeManager1 = New Telerik.WinControls.RadThemeManager()
        Me.rgb_SwapDetails = New Telerik.WinControls.UI.RadGroupBox()
        Me.se_RISK_FREE_RATE = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_AVG_PRICE = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_VOLATILITY = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_FFA_PRICE = New Telerik.WinControls.UI.RadSpinEditor()
        Me.cb_YY2 = New Telerik.WinControls.UI.RadDropDownList()
        Me.cb_MM2 = New Telerik.WinControls.UI.RadDropDownList()
        Me.cb_YY1 = New Telerik.WinControls.UI.RadDropDownList()
        Me.cb_MM1 = New Telerik.WinControls.UI.RadDropDownList()
        Me.cb_AVERAGE_INCLUDES_TODAY = New System.Windows.Forms.CheckBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.rgb_Leg1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.se_VEGA1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_THETA1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_DELTA1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_RHO1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_GAMMA1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.btn_PRICE1 = New Telerik.WinControls.UI.RadButton()
        Me.btn_STRIKE1 = New Telerik.WinControls.UI.RadButton()
        Me.btn_VOL1 = New Telerik.WinControls.UI.RadButton()
        Me.se_SKEW1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_QUANTITY1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_OPTION_PRICE1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_VOLATILITY1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_STRIKE1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.cb_OPTION_TYPE1 = New Telerik.WinControls.UI.RadDropDownList()
        Me.cb_BS1 = New Telerik.WinControls.UI.RadDropDownList()
        Me.se_PAYRECEIVE1 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rgb_Leg2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.se_VEGA2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_THETA2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_DELTA2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_RHO2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_GAMMA2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.cb_OPTION_TYPE2 = New Telerik.WinControls.UI.RadDropDownList()
        Me.btn_STRIKE2 = New Telerik.WinControls.UI.RadButton()
        Me.btn_VOL2 = New Telerik.WinControls.UI.RadButton()
        Me.btn_PRICE2 = New Telerik.WinControls.UI.RadButton()
        Me.se_SKEW2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_QUANTITY2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_OPTION_PRICE2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_VOLATILITY2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_STRIKE2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.cb_BS2 = New Telerik.WinControls.UI.RadDropDownList()
        Me.se_PAYRECEIVE2 = New Telerik.WinControls.UI.RadSpinEditor()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.rgb_StrategyResults = New Telerik.WinControls.UI.RadGroupBox()
        Me.btn_PRICE = New Telerik.WinControls.UI.RadButton()
        Me.se_DELTA = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_STRATEGY_PRICE = New Telerik.WinControls.UI.RadSpinEditor()
        Me.se_PAYRECEIVE = New Telerik.WinControls.UI.RadSpinEditor()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.rgb_Solver_Parameters = New Telerik.WinControls.UI.RadGroupBox()
        Me.MC_PMAX = New Telerik.WinControls.UI.RadSpinEditor()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.pb_NVIDIA = New System.Windows.Forms.PictureBox()
        Me.rbtn_MC_CUDA = New Telerik.WinControls.UI.RadRadioButton()
        Me.rbtn_ANALYTIC = New Telerik.WinControls.UI.RadRadioButton()
        CType(Me.rgb_Graphical_Data, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rgb_Graphical_Data.SuspendLayout()
        CType(Me.MSChart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rss_GRAPH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgb_Swap_Periods, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rgb_Swap_Periods.SuspendLayout()
        CType(Me.rgv_PERIODS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_PERIODS.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_BS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgb_SwapDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rgb_SwapDetails.SuspendLayout()
        CType(Me.se_RISK_FREE_RATE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_AVG_PRICE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_VOLATILITY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_FFA_PRICE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_YY2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_MM2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_YY1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_MM1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgb_Leg1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rgb_Leg1.SuspendLayout()
        CType(Me.se_VEGA1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_THETA1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_DELTA1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_RHO1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_GAMMA1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_PRICE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_STRIKE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_VOL1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_SKEW1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_QUANTITY1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_OPTION_PRICE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_VOLATILITY1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_STRIKE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_OPTION_TYPE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_BS1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_PAYRECEIVE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgb_Leg2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rgb_Leg2.SuspendLayout()
        CType(Me.se_VEGA2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_THETA2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_DELTA2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_RHO2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_GAMMA2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_OPTION_TYPE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_STRIKE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_VOL2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_PRICE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_SKEW2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_QUANTITY2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_OPTION_PRICE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_VOLATILITY2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_STRIKE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_BS2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_PAYRECEIVE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgb_StrategyResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rgb_StrategyResults.SuspendLayout()
        CType(Me.btn_PRICE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_DELTA, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_STRATEGY_PRICE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.se_PAYRECEIVE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgb_Solver_Parameters, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rgb_Solver_Parameters.SuspendLayout()
        CType(Me.MC_PMAX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pb_NVIDIA, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_MC_CUDA, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_ANALYTIC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rgb_Graphical_Data
        '
        Me.rgb_Graphical_Data.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.rgb_Graphical_Data.Controls.Add(Me.MSChart)
        Me.rgb_Graphical_Data.Controls.Add(Me.rss_GRAPH)
        Me.rgb_Graphical_Data.HeaderText = "Graphical Data"
        Me.rgb_Graphical_Data.Location = New System.Drawing.Point(2, 290)
        Me.rgb_Graphical_Data.Name = "rgb_Graphical_Data"
        '
        '
        '
        Me.rgb_Graphical_Data.RootElement.Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        Me.rgb_Graphical_Data.Size = New System.Drawing.Size(385, 256)
        Me.rgb_Graphical_Data.TabIndex = 3
        Me.rgb_Graphical_Data.Text = "Graphical Data"
        Me.rgb_Graphical_Data.ThemeName = "Office2010Silver"
        CType(Me.rgb_Graphical_Data.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Text = "Graphical Data"
        CType(Me.rgb_Graphical_Data.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        CType(Me.rgb_Graphical_Data.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Alignment = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MSChart
        '
        ChartArea1.AxisY.LabelStyle.Format = "P0"
        ChartArea1.Name = "Default"
        Me.MSChart.ChartAreas.Add(ChartArea1)
        Me.MSChart.Dock = System.Windows.Forms.DockStyle.Fill
        Legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
        Legend1.LegendItemOrder = System.Windows.Forms.DataVisualization.Charting.LegendItemOrder.SameAsSeriesOrder
        Legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row
        Legend1.Name = "GraphLegend"
        Legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide
        Me.MSChart.Legends.Add(Legend1)
        Me.MSChart.Location = New System.Drawing.Point(2, 47)
        Me.MSChart.Name = "MSChart"
        Series1.ChartArea = "Default"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Legend = "GraphLegend"
        Series1.Name = "Spot"
        Series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Date]
        Series1.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary
        Series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series2.ChartArea = "Default"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series2.Legend = "GraphLegend"
        Series2.Name = "SpotAvg"
        Series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Date]
        Series2.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary
        Series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series3.ChartArea = "Default"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series3.Legend = "GraphLegend"
        Series3.Name = "FFA"
        Series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Date]
        Series3.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary
        Series3.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series4.ChartArea = "Default"
        Series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series4.Legend = "GraphLegend"
        Series4.Name = "SpotHVol"
        Series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Date]
        Series4.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series5.ChartArea = "Default"
        Series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series5.Legend = "GraphLegend"
        Series5.Name = "FFAHVol"
        Series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Date]
        Series5.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series6.ChartArea = "Default"
        Series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series6.Legend = "GraphLegend"
        Series6.Name = "FFAIVol"
        Series6.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Date]
        Series6.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Me.MSChart.Series.Add(Series1)
        Me.MSChart.Series.Add(Series2)
        Me.MSChart.Series.Add(Series3)
        Me.MSChart.Series.Add(Series4)
        Me.MSChart.Series.Add(Series5)
        Me.MSChart.Series.Add(Series6)
        Me.MSChart.Size = New System.Drawing.Size(381, 207)
        Me.MSChart.TabIndex = 2
        Me.MSChart.Text = "Chart1"
        '
        'rss_GRAPH
        '
        Me.rss_GRAPH.Dock = System.Windows.Forms.DockStyle.Top
        Me.rss_GRAPH.Items.AddRange(New Telerik.WinControls.RadItem() {Me.rcb_FFAIVol, Me.rcb_FFAHVol, Me.rcb_SpotHVol, Me.CommandBarSeparator1, Me.rcb_FFA, Me.rcb_Spot, Me.rcb_SpotAvg})
        Me.rss_GRAPH.Location = New System.Drawing.Point(2, 18)
        Me.rss_GRAPH.Name = "rss_GRAPH"
        Me.rss_GRAPH.Size = New System.Drawing.Size(381, 29)
        Me.rss_GRAPH.TabIndex = 1
        Me.rss_GRAPH.ThemeName = "Office2010Silver"
        '
        'rcb_FFAIVol
        '
        Me.rcb_FFAIVol.AccessibleDescription = "FIVol"
        Me.rcb_FFAIVol.AccessibleName = "FIVol"
        Me.rcb_FFAIVol.Checked = True
        Me.rcb_FFAIVol.DisplayStyle = Telerik.WinControls.DisplayStyle.ImageAndText
        Me.rcb_FFAIVol.FlipText = False
        Me.rcb_FFAIVol.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rcb_FFAIVol.ForeColor = System.Drawing.Color.Red
        Me.rcb_FFAIVol.Name = "rcb_FFAIVol"
        Me.rss_GRAPH.SetSpring(Me.rcb_FFAIVol, False)
        Me.rcb_FFAIVol.Text = "FIVol"
        Me.rcb_FFAIVol.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        Me.rcb_FFAIVol.ToolTipText = "Plot FFA Implied Volatility"
        Me.rcb_FFAIVol.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rcb_FFAHVol
        '
        Me.rcb_FFAHVol.AccessibleDescription = "FHVol"
        Me.rcb_FFAHVol.AccessibleName = "FHVol"
        Me.rcb_FFAHVol.Checked = False
        Me.rcb_FFAHVol.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rcb_FFAHVol.ForeColor = System.Drawing.Color.DarkOrange
        Me.rcb_FFAHVol.Name = "rcb_FFAHVol"
        Me.rss_GRAPH.SetSpring(Me.rcb_FFAHVol, False)
        Me.rcb_FFAHVol.Text = "FHVol"
        Me.rcb_FFAHVol.ToolTipText = "Plot FFA Historical Volatility"
        Me.rcb_FFAHVol.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rcb_SpotHVol
        '
        Me.rcb_SpotHVol.AccessibleDescription = "SHVol"
        Me.rcb_SpotHVol.AccessibleName = "SHVol"
        Me.rcb_SpotHVol.Checked = False
        Me.rcb_SpotHVol.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rcb_SpotHVol.ForeColor = System.Drawing.Color.Brown
        Me.rcb_SpotHVol.Name = "rcb_SpotHVol"
        Me.rss_GRAPH.SetSpring(Me.rcb_SpotHVol, False)
        Me.rcb_SpotHVol.Text = "SHVol"
        Me.rcb_SpotHVol.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off
        Me.rcb_SpotHVol.ToolTipText = "Plot Spot Historical Volatility"
        Me.rcb_SpotHVol.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'CommandBarSeparator1
        '
        Me.CommandBarSeparator1.AccessibleDescription = "CommandBarSeparator1"
        Me.CommandBarSeparator1.AccessibleName = "CommandBarSeparator1"
        Me.CommandBarSeparator1.Name = "CommandBarSeparator1"
        Me.rss_GRAPH.SetSpring(Me.CommandBarSeparator1, False)
        Me.CommandBarSeparator1.Visibility = Telerik.WinControls.ElementVisibility.Visible
        Me.CommandBarSeparator1.VisibleInOverflowMenu = False
        '
        'rcb_FFA
        '
        Me.rcb_FFA.AccessibleDescription = "FFA"
        Me.rcb_FFA.AccessibleName = "FFA"
        Me.rcb_FFA.Checked = False
        Me.rcb_FFA.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rcb_FFA.ForeColor = System.Drawing.Color.Blue
        Me.rcb_FFA.Name = "rcb_FFA"
        Me.rss_GRAPH.SetSpring(Me.rcb_FFA, False)
        Me.rcb_FFA.Text = "FFA"
        Me.rcb_FFA.ToolTipText = "Plot FFA Price"
        Me.rcb_FFA.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rcb_Spot
        '
        Me.rcb_Spot.AccessibleDescription = "Spot"
        Me.rcb_Spot.AccessibleName = "Spot"
        Me.rcb_Spot.Checked = False
        Me.rcb_Spot.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rcb_Spot.Name = "rcb_Spot"
        Me.rss_GRAPH.SetSpring(Me.rcb_Spot, False)
        Me.rcb_Spot.Text = "Spot"
        Me.rcb_Spot.ToolTipText = "Plot Spot Price"
        Me.rcb_Spot.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rcb_SpotAvg
        '
        Me.rcb_SpotAvg.AccessibleDescription = "SAvg"
        Me.rcb_SpotAvg.AccessibleName = "SAvg"
        Me.rcb_SpotAvg.Checked = False
        Me.rcb_SpotAvg.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rcb_SpotAvg.ForeColor = System.Drawing.Color.DarkSeaGreen
        Me.rcb_SpotAvg.Name = "rcb_SpotAvg"
        Me.rss_GRAPH.SetSpring(Me.rcb_SpotAvg, False)
        Me.rcb_SpotAvg.Text = "SAvg"
        Me.rcb_SpotAvg.ToolTipText = "Plot Spot Average"
        Me.rcb_SpotAvg.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rgb_Swap_Periods
        '
        Me.rgb_Swap_Periods.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.rgb_Swap_Periods.Controls.Add(Me.rgv_PERIODS)
        Me.rgb_Swap_Periods.HeaderImage = Global.FFASuite.My.Resources.Resources.Question_GR16R
        Me.rgb_Swap_Periods.HeaderText = "Swap Periods"
        Me.rgb_Swap_Periods.HeaderTextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.rgb_Swap_Periods.Location = New System.Drawing.Point(0, 5)
        Me.rgb_Swap_Periods.Name = "rgb_Swap_Periods"
        '
        '
        '
        Me.rgb_Swap_Periods.RootElement.Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        Me.rgb_Swap_Periods.Size = New System.Drawing.Size(389, 283)
        Me.rgb_Swap_Periods.TabIndex = 2
        Me.rgb_Swap_Periods.Text = "Swap Periods"
        Me.rgb_Swap_Periods.ThemeName = "Office2010Silver"
        CType(Me.rgb_Swap_Periods.GetChildAt(0).GetChildAt(1).GetChildAt(2), Telerik.WinControls.Layouts.ImageAndTextLayoutPanel).TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        CType(Me.rgb_Swap_Periods.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Text = "Swap Periods"
        CType(Me.rgb_Swap_Periods.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        CType(Me.rgb_Swap_Periods.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Alignment = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rgv_PERIODS
        '
        Me.rgv_PERIODS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rgv_PERIODS.Location = New System.Drawing.Point(2, 18)
        '
        'rgv_PERIODS
        '
        Me.rgv_PERIODS.MasterTemplate.AllowAddNewRow = False
        Me.rgv_PERIODS.MasterTemplate.AllowColumnChooser = False
        Me.rgv_PERIODS.MasterTemplate.AllowColumnHeaderContextMenu = False
        Me.rgv_PERIODS.MasterTemplate.AllowColumnReorder = False
        Me.rgv_PERIODS.MasterTemplate.AllowColumnResize = False
        Me.rgv_PERIODS.MasterTemplate.AllowDeleteRow = False
        Me.rgv_PERIODS.MasterTemplate.AllowDragToGroup = False
        GridViewDecimalColumn1.FieldName = "ROUTE_ID"
        GridViewDecimalColumn1.HeaderText = "ROUTE_ID"
        GridViewDecimalColumn1.IsVisible = False
        GridViewDecimalColumn1.Name = "ROUTE_ID"
        GridViewTextBoxColumn1.FieldName = "PERIOD"
        GridViewTextBoxColumn1.HeaderText = "PERIOD"
        GridViewTextBoxColumn1.Name = "PERIOD"
        GridViewTextBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.Width = 100
        GridViewDecimalColumn2.FieldName = "YY1"
        GridViewDecimalColumn2.HeaderText = "YY1"
        GridViewDecimalColumn2.IsVisible = False
        GridViewDecimalColumn2.Name = "YY1"
        GridViewDecimalColumn2.ReadOnly = True
        GridViewDecimalColumn3.FieldName = "MM1"
        GridViewDecimalColumn3.HeaderText = "MM1"
        GridViewDecimalColumn3.IsVisible = False
        GridViewDecimalColumn3.Name = "MM1"
        GridViewDecimalColumn3.ReadOnly = True
        GridViewDecimalColumn4.FieldName = "YY2"
        GridViewDecimalColumn4.HeaderText = "YY2"
        GridViewDecimalColumn4.IsVisible = False
        GridViewDecimalColumn4.Name = "YY2"
        GridViewDecimalColumn4.ReadOnly = True
        GridViewDecimalColumn5.FieldName = "MM2"
        GridViewDecimalColumn5.HeaderText = "MM2"
        GridViewDecimalColumn5.IsVisible = False
        GridViewDecimalColumn5.Name = "MM2"
        GridViewDecimalColumn5.ReadOnly = True
        GridViewDecimalColumn6.FieldName = "IVOL"
        GridViewDecimalColumn6.FormatString = "{0:N2}"
        GridViewDecimalColumn6.HeaderText = "IVOL%"
        GridViewDecimalColumn6.Name = "IVOL"
        GridViewDecimalColumn6.Step = New Decimal(New Integer() {1, 0, 0, 131072})
        GridViewDecimalColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn6.Width = 65
        GridViewDecimalColumn7.FieldName = "HVOL"
        GridViewDecimalColumn7.FormatString = "{0:N2}"
        GridViewDecimalColumn7.HeaderText = "HVOL%"
        GridViewDecimalColumn7.Name = "HVOL"
        GridViewDecimalColumn7.ReadOnly = True
        GridViewDecimalColumn7.Step = New Decimal(New Integer() {1, 0, 0, 131072})
        GridViewDecimalColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn7.Width = 65
        GridViewDecimalColumn8.FieldName = "INTEREST_RATE"
        GridViewDecimalColumn8.FormatString = "{0:N3}"
        GridViewDecimalColumn8.HeaderText = "RFR%"
        GridViewDecimalColumn8.Name = "INTEREST_RATE"
        GridViewDecimalColumn8.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn8.Width = 65
        GridViewDecimalColumn9.DecimalPlaces = 0
        GridViewDecimalColumn9.FieldName = "FFA_PRICE"
        GridViewDecimalColumn9.FormatString = "{0:N0}"
        GridViewDecimalColumn9.HeaderText = "FPrice"
        GridViewDecimalColumn9.Name = "FFA_PRICE"
        GridViewDecimalColumn9.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn9.ThousandsSeparator = True
        GridViewDecimalColumn9.Width = 75
        GridViewCheckBoxColumn1.FieldName = "ONLYHISTORICAL"
        GridViewCheckBoxColumn1.HeaderText = "ONLYHISTORICAL"
        GridViewCheckBoxColumn1.IsVisible = False
        GridViewCheckBoxColumn1.Name = "ONLYHISTORICAL"
        GridViewCheckBoxColumn2.FieldName = "LIVE_DATA"
        GridViewCheckBoxColumn2.HeaderText = "LIVE_DATA"
        GridViewCheckBoxColumn2.IsVisible = False
        GridViewCheckBoxColumn2.Name = "LIVE_DATA"
        GridViewDecimalColumn10.FieldName = "VolRecordType"
        GridViewDecimalColumn10.HeaderText = "VolRecordType"
        GridViewDecimalColumn10.IsVisible = False
        GridViewDecimalColumn10.Name = "VolRecordType"
        GridViewDecimalColumn11.FieldName = "TRADE_ID"
        GridViewDecimalColumn11.HeaderText = "TRADE_ID"
        GridViewDecimalColumn11.IsVisible = False
        GridViewDecimalColumn11.Name = "TRADE_ID"
        GridViewDecimalColumn11.ReadOnly = True
        Me.rgv_PERIODS.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn1, GridViewTextBoxColumn1, GridViewDecimalColumn2, GridViewDecimalColumn3, GridViewDecimalColumn4, GridViewDecimalColumn5, GridViewDecimalColumn6, GridViewDecimalColumn7, GridViewDecimalColumn8, GridViewDecimalColumn9, GridViewCheckBoxColumn1, GridViewCheckBoxColumn2, GridViewDecimalColumn10, GridViewDecimalColumn11})
        Me.rgv_PERIODS.MasterTemplate.EnableAlternatingRowColor = True
        Me.rgv_PERIODS.MasterTemplate.EnableGrouping = False
        Me.rgv_PERIODS.MasterTemplate.EnableSorting = False
        Me.rgv_PERIODS.MasterTemplate.HorizontalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysHide
        Me.rgv_PERIODS.MasterTemplate.ShowRowHeaderColumn = False
        Me.rgv_PERIODS.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow
        Me.rgv_PERIODS.Name = "rgv_PERIODS"
        Me.rgv_PERIODS.ShowGroupPanel = False
        Me.rgv_PERIODS.Size = New System.Drawing.Size(385, 263)
        Me.rgv_PERIODS.TabIndex = 0
        Me.rgv_PERIODS.Text = "rgv_PERIODS"
        Me.rgv_PERIODS.ThemeName = "Office2010Silver"
        '
        'rgb_SwapDetails
        '
        Me.rgb_SwapDetails.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.rgb_SwapDetails.Controls.Add(Me.se_RISK_FREE_RATE)
        Me.rgb_SwapDetails.Controls.Add(Me.se_AVG_PRICE)
        Me.rgb_SwapDetails.Controls.Add(Me.se_VOLATILITY)
        Me.rgb_SwapDetails.Controls.Add(Me.se_FFA_PRICE)
        Me.rgb_SwapDetails.Controls.Add(Me.cb_YY2)
        Me.rgb_SwapDetails.Controls.Add(Me.cb_MM2)
        Me.rgb_SwapDetails.Controls.Add(Me.cb_YY1)
        Me.rgb_SwapDetails.Controls.Add(Me.cb_MM1)
        Me.rgb_SwapDetails.Controls.Add(Me.cb_AVERAGE_INCLUDES_TODAY)
        Me.rgb_SwapDetails.Controls.Add(Me.Label28)
        Me.rgb_SwapDetails.Controls.Add(Me.Label27)
        Me.rgb_SwapDetails.Controls.Add(Me.Label19)
        Me.rgb_SwapDetails.Controls.Add(Me.Label26)
        Me.rgb_SwapDetails.Controls.Add(Me.Label20)
        Me.rgb_SwapDetails.Controls.Add(Me.Label22)
        Me.rgb_SwapDetails.Controls.Add(Me.Label23)
        Me.rgb_SwapDetails.Controls.Add(Me.Label21)
        Me.rgb_SwapDetails.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rgb_SwapDetails.HeaderImage = Global.FFASuite.My.Resources.Resources.Question_GR16R
        Me.rgb_SwapDetails.HeaderText = "Swap Details"
        Me.rgb_SwapDetails.HeaderTextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.rgb_SwapDetails.Location = New System.Drawing.Point(394, 5)
        Me.rgb_SwapDetails.Name = "rgb_SwapDetails"
        '
        '
        '
        Me.rgb_SwapDetails.RootElement.Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        Me.rgb_SwapDetails.Size = New System.Drawing.Size(360, 104)
        Me.rgb_SwapDetails.TabIndex = 4
        Me.rgb_SwapDetails.Text = "Swap Details"
        Me.rgb_SwapDetails.ThemeName = "Office2010Silver"
        '
        'se_RISK_FREE_RATE
        '
        Me.se_RISK_FREE_RATE.DecimalPlaces = 3
        Me.se_RISK_FREE_RATE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_RISK_FREE_RATE.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.se_RISK_FREE_RATE.Location = New System.Drawing.Point(285, 39)
        Me.se_RISK_FREE_RATE.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.se_RISK_FREE_RATE.Name = "se_RISK_FREE_RATE"
        Me.se_RISK_FREE_RATE.Size = New System.Drawing.Size(65, 20)
        Me.se_RISK_FREE_RATE.TabIndex = 130
        Me.se_RISK_FREE_RATE.TabStop = False
        Me.se_RISK_FREE_RATE.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_RISK_FREE_RATE.ThemeName = "Office2010Silver"
        '
        'se_AVG_PRICE
        '
        Me.se_AVG_PRICE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_AVG_PRICE.Increment = New Decimal(New Integer() {250, 0, 0, 0})
        Me.se_AVG_PRICE.Location = New System.Drawing.Point(123, 78)
        Me.se_AVG_PRICE.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.se_AVG_PRICE.Name = "se_AVG_PRICE"
        Me.se_AVG_PRICE.Size = New System.Drawing.Size(81, 20)
        Me.se_AVG_PRICE.TabIndex = 129
        Me.se_AVG_PRICE.TabStop = False
        Me.se_AVG_PRICE.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_AVG_PRICE.ThemeName = "Office2010Silver"
        Me.se_AVG_PRICE.ThousandsSeparator = True
        '
        'se_VOLATILITY
        '
        Me.se_VOLATILITY.DecimalPlaces = 2
        Me.se_VOLATILITY.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_VOLATILITY.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.se_VOLATILITY.Location = New System.Drawing.Point(207, 39)
        Me.se_VOLATILITY.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.se_VOLATILITY.Name = "se_VOLATILITY"
        Me.se_VOLATILITY.Size = New System.Drawing.Size(65, 20)
        Me.se_VOLATILITY.TabIndex = 127
        Me.se_VOLATILITY.TabStop = False
        Me.se_VOLATILITY.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_VOLATILITY.ThemeName = "Office2010Silver"
        '
        'se_FFA_PRICE
        '
        Me.se_FFA_PRICE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_FFA_PRICE.Increment = New Decimal(New Integer() {250, 0, 0, 0})
        Me.se_FFA_PRICE.Location = New System.Drawing.Point(122, 39)
        Me.se_FFA_PRICE.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.se_FFA_PRICE.Name = "se_FFA_PRICE"
        Me.se_FFA_PRICE.Size = New System.Drawing.Size(81, 20)
        Me.se_FFA_PRICE.TabIndex = 126
        Me.se_FFA_PRICE.TabStop = False
        Me.se_FFA_PRICE.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_FFA_PRICE.ThemeName = "Office2010Silver"
        Me.se_FFA_PRICE.ThousandsSeparator = True
        '
        'cb_YY2
        '
        Me.cb_YY2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cb_YY2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.cb_YY2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_YY2.Location = New System.Drawing.Point(64, 78)
        Me.cb_YY2.Name = "cb_YY2"
        Me.cb_YY2.Size = New System.Drawing.Size(52, 22)
        Me.cb_YY2.TabIndex = 125
        Me.cb_YY2.Text = "RadDropDownList1"
        Me.cb_YY2.ThemeName = "Office2010Silver"
        '
        'cb_MM2
        '
        Me.cb_MM2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cb_MM2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.cb_MM2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_MM2.Location = New System.Drawing.Point(6, 78)
        Me.cb_MM2.Name = "cb_MM2"
        Me.cb_MM2.Size = New System.Drawing.Size(52, 22)
        Me.cb_MM2.TabIndex = 124
        Me.cb_MM2.Text = "RadDropDownList1"
        Me.cb_MM2.ThemeName = "Office2010Silver"
        '
        'cb_YY1
        '
        Me.cb_YY1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cb_YY1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.cb_YY1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_YY1.Location = New System.Drawing.Point(64, 39)
        Me.cb_YY1.Name = "cb_YY1"
        Me.cb_YY1.Size = New System.Drawing.Size(52, 22)
        Me.cb_YY1.TabIndex = 123
        Me.cb_YY1.Text = "RadDropDownList1"
        Me.cb_YY1.ThemeName = "Office2010Silver"
        '
        'cb_MM1
        '
        Me.cb_MM1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cb_MM1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.cb_MM1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_MM1.Location = New System.Drawing.Point(6, 39)
        Me.cb_MM1.Name = "cb_MM1"
        Me.cb_MM1.Size = New System.Drawing.Size(52, 22)
        Me.cb_MM1.TabIndex = 122
        Me.cb_MM1.Text = "RadDropDownList1"
        Me.cb_MM1.ThemeName = "Office2010Silver"
        '
        'cb_AVERAGE_INCLUDES_TODAY
        '
        Me.cb_AVERAGE_INCLUDES_TODAY.AutoSize = True
        Me.cb_AVERAGE_INCLUDES_TODAY.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_AVERAGE_INCLUDES_TODAY.Location = New System.Drawing.Point(218, 81)
        Me.cb_AVERAGE_INCLUDES_TODAY.Name = "cb_AVERAGE_INCLUDES_TODAY"
        Me.cb_AVERAGE_INCLUDES_TODAY.Size = New System.Drawing.Size(123, 17)
        Me.cb_AVERAGE_INCLUDES_TODAY.TabIndex = 111
        Me.cb_AVERAGE_INCLUDES_TODAY.Text = "Today's Fix Included"
        Me.cb_AVERAGE_INCLUDES_TODAY.UseVisualStyleBackColor = True
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label28.Location = New System.Drawing.Point(287, 24)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(53, 13)
        Me.Label28.TabIndex = 121
        Me.Label28.Text = "Interest %"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label27.Location = New System.Drawing.Point(123, 63)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(74, 13)
        Me.Label27.TabIndex = 109
        Me.Label27.Text = "Average Price"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label19.Location = New System.Drawing.Point(5, 24)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(50, 13)
        Me.Label19.TabIndex = 112
        Me.Label19.Text = "Start MM"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label26.Location = New System.Drawing.Point(123, 24)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(61, 13)
        Me.Label26.TabIndex = 107
        Me.Label26.Text = "Swap Price"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label20.Location = New System.Drawing.Point(61, 24)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(46, 13)
        Me.Label20.TabIndex = 114
        Me.Label20.Text = "Start YY"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label22.Location = New System.Drawing.Point(5, 63)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(47, 13)
        Me.Label22.TabIndex = 116
        Me.Label22.Text = "End MM"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label23.Location = New System.Drawing.Point(208, 24)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(69, 13)
        Me.Label23.TabIndex = 119
        Me.Label23.Text = "Implied Vol %"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label21.Location = New System.Drawing.Point(61, 63)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(43, 13)
        Me.Label21.TabIndex = 118
        Me.Label21.Text = "End YY"
        '
        'rgb_Leg1
        '
        Me.rgb_Leg1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.rgb_Leg1.Controls.Add(Me.se_VEGA1)
        Me.rgb_Leg1.Controls.Add(Me.se_THETA1)
        Me.rgb_Leg1.Controls.Add(Me.se_DELTA1)
        Me.rgb_Leg1.Controls.Add(Me.se_RHO1)
        Me.rgb_Leg1.Controls.Add(Me.se_GAMMA1)
        Me.rgb_Leg1.Controls.Add(Me.btn_PRICE1)
        Me.rgb_Leg1.Controls.Add(Me.btn_STRIKE1)
        Me.rgb_Leg1.Controls.Add(Me.btn_VOL1)
        Me.rgb_Leg1.Controls.Add(Me.se_SKEW1)
        Me.rgb_Leg1.Controls.Add(Me.se_QUANTITY1)
        Me.rgb_Leg1.Controls.Add(Me.se_OPTION_PRICE1)
        Me.rgb_Leg1.Controls.Add(Me.se_VOLATILITY1)
        Me.rgb_Leg1.Controls.Add(Me.se_STRIKE1)
        Me.rgb_Leg1.Controls.Add(Me.cb_OPTION_TYPE1)
        Me.rgb_Leg1.Controls.Add(Me.cb_BS1)
        Me.rgb_Leg1.Controls.Add(Me.se_PAYRECEIVE1)
        Me.rgb_Leg1.Controls.Add(Me.Label13)
        Me.rgb_Leg1.Controls.Add(Me.Label33)
        Me.rgb_Leg1.Controls.Add(Me.Label32)
        Me.rgb_Leg1.Controls.Add(Me.Label4)
        Me.rgb_Leg1.Controls.Add(Me.Label6)
        Me.rgb_Leg1.Controls.Add(Me.Label31)
        Me.rgb_Leg1.Controls.Add(Me.Label30)
        Me.rgb_Leg1.Controls.Add(Me.Label29)
        Me.rgb_Leg1.Controls.Add(Me.Label5)
        Me.rgb_Leg1.Controls.Add(Me.Label3)
        Me.rgb_Leg1.Controls.Add(Me.Label2)
        Me.rgb_Leg1.Controls.Add(Me.Label1)
        Me.rgb_Leg1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rgb_Leg1.HeaderText = "Leg 1"
        Me.rgb_Leg1.HeaderTextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.rgb_Leg1.Location = New System.Drawing.Point(394, 113)
        Me.rgb_Leg1.Name = "rgb_Leg1"
        '
        '
        '
        Me.rgb_Leg1.RootElement.Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        Me.rgb_Leg1.Size = New System.Drawing.Size(360, 155)
        Me.rgb_Leg1.TabIndex = 5
        Me.rgb_Leg1.Text = "Leg 1"
        Me.rgb_Leg1.ThemeName = "Office2010Silver"
        '
        'se_VEGA1
        '
        Me.se_VEGA1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_VEGA1.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_VEGA1.Location = New System.Drawing.Point(289, 99)
        Me.se_VEGA1.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_VEGA1.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_VEGA1.Name = "se_VEGA1"
        Me.se_VEGA1.ReadOnly = True
        '
        '
        '
        Me.se_VEGA1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_VEGA1.ShowUpDownButtons = False
        Me.se_VEGA1.Size = New System.Drawing.Size(65, 18)
        Me.se_VEGA1.TabIndex = 117
        Me.se_VEGA1.TabStop = False
        Me.se_VEGA1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_VEGA1.ThemeName = "Office2010Silver"
        Me.se_VEGA1.ThousandsSeparator = True
        '
        'se_THETA1
        '
        Me.se_THETA1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_THETA1.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_THETA1.Location = New System.Drawing.Point(214, 99)
        Me.se_THETA1.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_THETA1.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_THETA1.Name = "se_THETA1"
        Me.se_THETA1.ReadOnly = True
        '
        '
        '
        Me.se_THETA1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_THETA1.ShowUpDownButtons = False
        Me.se_THETA1.Size = New System.Drawing.Size(72, 18)
        Me.se_THETA1.TabIndex = 116
        Me.se_THETA1.TabStop = False
        Me.se_THETA1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_THETA1.ThemeName = "Office2010Silver"
        Me.se_THETA1.ThousandsSeparator = True
        '
        'se_DELTA1
        '
        Me.se_DELTA1.DecimalPlaces = 2
        Me.se_DELTA1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_DELTA1.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_DELTA1.Location = New System.Drawing.Point(4, 99)
        Me.se_DELTA1.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_DELTA1.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_DELTA1.Name = "se_DELTA1"
        Me.se_DELTA1.ReadOnly = True
        '
        '
        '
        Me.se_DELTA1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_DELTA1.ShowUpDownButtons = False
        Me.se_DELTA1.Size = New System.Drawing.Size(62, 18)
        Me.se_DELTA1.TabIndex = 113
        Me.se_DELTA1.TabStop = False
        Me.se_DELTA1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_DELTA1.ThemeName = "Office2010Silver"
        Me.se_DELTA1.ThousandsSeparator = True
        '
        'se_RHO1
        '
        Me.se_RHO1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_RHO1.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_RHO1.Location = New System.Drawing.Point(141, 99)
        Me.se_RHO1.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_RHO1.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_RHO1.Name = "se_RHO1"
        Me.se_RHO1.ReadOnly = True
        '
        '
        '
        Me.se_RHO1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_RHO1.ShowUpDownButtons = False
        Me.se_RHO1.Size = New System.Drawing.Size(70, 18)
        Me.se_RHO1.TabIndex = 115
        Me.se_RHO1.TabStop = False
        Me.se_RHO1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_RHO1.ThemeName = "Office2010Silver"
        Me.se_RHO1.ThousandsSeparator = True
        '
        'se_GAMMA1
        '
        Me.se_GAMMA1.DecimalPlaces = 4
        Me.se_GAMMA1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_GAMMA1.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_GAMMA1.Location = New System.Drawing.Point(69, 99)
        Me.se_GAMMA1.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_GAMMA1.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_GAMMA1.Name = "se_GAMMA1"
        Me.se_GAMMA1.ReadOnly = True
        '
        '
        '
        Me.se_GAMMA1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_GAMMA1.ShowUpDownButtons = False
        Me.se_GAMMA1.Size = New System.Drawing.Size(70, 18)
        Me.se_GAMMA1.TabIndex = 114
        Me.se_GAMMA1.TabStop = False
        Me.se_GAMMA1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_GAMMA1.ThemeName = "Office2010Silver"
        Me.se_GAMMA1.ThousandsSeparator = True
        '
        'btn_PRICE1
        '
        Me.btn_PRICE1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.btn_PRICE1.Location = New System.Drawing.Point(279, 124)
        Me.btn_PRICE1.Name = "btn_PRICE1"
        Me.btn_PRICE1.Size = New System.Drawing.Size(76, 24)
        Me.btn_PRICE1.TabIndex = 163
        Me.btn_PRICE1.Text = "Price"
        Me.btn_PRICE1.ThemeName = "Office2010Silver"
        '
        'btn_STRIKE1
        '
        Me.btn_STRIKE1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.btn_STRIKE1.Location = New System.Drawing.Point(117, 124)
        Me.btn_STRIKE1.Name = "btn_STRIKE1"
        Me.btn_STRIKE1.Size = New System.Drawing.Size(76, 24)
        Me.btn_STRIKE1.TabIndex = 136
        Me.btn_STRIKE1.Text = "Strike Prc"
        Me.btn_STRIKE1.ThemeName = "Office2010Silver"
        '
        'btn_VOL1
        '
        Me.btn_VOL1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.btn_VOL1.Location = New System.Drawing.Point(198, 124)
        Me.btn_VOL1.Name = "btn_VOL1"
        Me.btn_VOL1.Size = New System.Drawing.Size(76, 24)
        Me.btn_VOL1.TabIndex = 135
        Me.btn_VOL1.Text = "IVol%"
        Me.btn_VOL1.ThemeName = "Office2010Silver"
        '
        'se_SKEW1
        '
        Me.se_SKEW1.DecimalPlaces = 2
        Me.se_SKEW1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_SKEW1.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.se_SKEW1.Location = New System.Drawing.Point(206, 61)
        Me.se_SKEW1.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.se_SKEW1.Name = "se_SKEW1"
        Me.se_SKEW1.Size = New System.Drawing.Size(65, 20)
        Me.se_SKEW1.TabIndex = 133
        Me.se_SKEW1.TabStop = False
        Me.se_SKEW1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_SKEW1.ThemeName = "Office2010Silver"
        '
        'se_QUANTITY1
        '
        Me.se_QUANTITY1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_QUANTITY1.Location = New System.Drawing.Point(64, 61)
        Me.se_QUANTITY1.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.se_QUANTITY1.Minimum = New Decimal(New Integer() {5, 0, 0, -2147483648})
        Me.se_QUANTITY1.Name = "se_QUANTITY1"
        Me.se_QUANTITY1.Size = New System.Drawing.Size(52, 20)
        Me.se_QUANTITY1.TabIndex = 132
        Me.se_QUANTITY1.TabStop = False
        Me.se_QUANTITY1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_QUANTITY1.ThemeName = "Office2010Silver"
        Me.se_QUANTITY1.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'se_OPTION_PRICE1
        '
        Me.se_OPTION_PRICE1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_OPTION_PRICE1.Increment = New Decimal(New Integer() {250, 0, 0, 0})
        Me.se_OPTION_PRICE1.Location = New System.Drawing.Point(274, 37)
        Me.se_OPTION_PRICE1.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.se_OPTION_PRICE1.Name = "se_OPTION_PRICE1"
        Me.se_OPTION_PRICE1.Size = New System.Drawing.Size(81, 20)
        Me.se_OPTION_PRICE1.TabIndex = 131
        Me.se_OPTION_PRICE1.TabStop = False
        Me.se_OPTION_PRICE1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_OPTION_PRICE1.ThemeName = "Office2010Silver"
        Me.se_OPTION_PRICE1.ThousandsSeparator = True
        '
        'se_VOLATILITY1
        '
        Me.se_VOLATILITY1.DecimalPlaces = 2
        Me.se_VOLATILITY1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_VOLATILITY1.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.se_VOLATILITY1.Location = New System.Drawing.Point(206, 37)
        Me.se_VOLATILITY1.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.se_VOLATILITY1.Name = "se_VOLATILITY1"
        Me.se_VOLATILITY1.Size = New System.Drawing.Size(65, 20)
        Me.se_VOLATILITY1.TabIndex = 130
        Me.se_VOLATILITY1.TabStop = False
        Me.se_VOLATILITY1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_VOLATILITY1.ThemeName = "Office2010Silver"
        '
        'se_STRIKE1
        '
        Me.se_STRIKE1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_STRIKE1.Increment = New Decimal(New Integer() {250, 0, 0, 0})
        Me.se_STRIKE1.Location = New System.Drawing.Point(122, 37)
        Me.se_STRIKE1.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.se_STRIKE1.Name = "se_STRIKE1"
        Me.se_STRIKE1.Size = New System.Drawing.Size(81, 20)
        Me.se_STRIKE1.TabIndex = 129
        Me.se_STRIKE1.TabStop = False
        Me.se_STRIKE1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_STRIKE1.ThemeName = "Office2010Silver"
        Me.se_STRIKE1.ThousandsSeparator = True
        '
        'cb_OPTION_TYPE1
        '
        Me.cb_OPTION_TYPE1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cb_OPTION_TYPE1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.cb_OPTION_TYPE1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_OPTION_TYPE1.Location = New System.Drawing.Point(64, 37)
        Me.cb_OPTION_TYPE1.Name = "cb_OPTION_TYPE1"
        Me.cb_OPTION_TYPE1.Size = New System.Drawing.Size(52, 22)
        Me.cb_OPTION_TYPE1.TabIndex = 128
        Me.cb_OPTION_TYPE1.Text = "RadDropDownList1"
        Me.cb_OPTION_TYPE1.ThemeName = "Office2010Silver"
        '
        'cb_BS1
        '
        Me.cb_BS1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cb_BS1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.cb_BS1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_BS1.Location = New System.Drawing.Point(5, 37)
        Me.cb_BS1.Name = "cb_BS1"
        Me.cb_BS1.Size = New System.Drawing.Size(52, 22)
        Me.cb_BS1.TabIndex = 127
        Me.cb_BS1.Text = "RadDropDownList1"
        Me.cb_BS1.ThemeName = "Office2010Silver"
        '
        'se_PAYRECEIVE1
        '
        Me.se_PAYRECEIVE1.DecimalPlaces = 2
        Me.se_PAYRECEIVE1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_PAYRECEIVE1.Location = New System.Drawing.Point(4, 126)
        Me.se_PAYRECEIVE1.Maximum = New Decimal(New Integer() {-727379969, 232, 0, 0})
        Me.se_PAYRECEIVE1.Minimum = New Decimal(New Integer() {-727379969, 232, 0, -2147483648})
        Me.se_PAYRECEIVE1.Name = "se_PAYRECEIVE1"
        Me.se_PAYRECEIVE1.ReadOnly = True
        '
        '
        '
        Me.se_PAYRECEIVE1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_PAYRECEIVE1.ShowUpDownButtons = False
        Me.se_PAYRECEIVE1.Size = New System.Drawing.Size(108, 20)
        Me.se_PAYRECEIVE1.TabIndex = 126
        Me.se_PAYRECEIVE1.TabStop = False
        Me.se_PAYRECEIVE1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_PAYRECEIVE1.ThemeName = "Office2010Silver"
        Me.se_PAYRECEIVE1.ThousandsSeparator = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label13.Location = New System.Drawing.Point(202, 22)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(66, 13)
        Me.Label13.TabIndex = 112
        Me.Label13.Text = "Implied Vol%"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.BackColor = System.Drawing.SystemColors.Control
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label33.Location = New System.Drawing.Point(288, 84)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(32, 13)
        Me.Label33.TabIndex = 122
        Me.Label33.Text = "Vega"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.BackColor = System.Drawing.SystemColors.Control
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label32.Location = New System.Drawing.Point(212, 84)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(35, 13)
        Me.Label32.TabIndex = 121
        Me.Label32.Text = "Theta"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label4.Location = New System.Drawing.Point(271, 22)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 13)
        Me.Label4.TabIndex = 107
        Me.Label4.Text = "Option Price"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label6.Location = New System.Drawing.Point(11, 63)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 13)
        Me.Label6.TabIndex = 110
        Me.Label6.Text = "Q < 0: full"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.BackColor = System.Drawing.SystemColors.Control
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label31.Location = New System.Drawing.Point(141, 84)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(27, 13)
        Me.Label31.TabIndex = 120
        Me.Label31.Text = "Rho"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.BackColor = System.Drawing.SystemColors.Control
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label30.Location = New System.Drawing.Point(70, 84)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(43, 13)
        Me.Label30.TabIndex = 119
        Me.Label30.Text = "Gamma"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.SystemColors.Control
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label29.Location = New System.Drawing.Point(6, 84)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(32, 13)
        Me.Label29.TabIndex = 118
        Me.Label29.Text = "Delta"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label5.Location = New System.Drawing.Point(154, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 108
        Me.Label5.Text = "Skew %"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label3.Location = New System.Drawing.Point(122, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 106
        Me.Label3.Text = "Strike Price"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label2.Location = New System.Drawing.Point(66, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 105
        Me.Label2.Text = "Call/Put"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Buy/Sell"
        '
        'rgb_Leg2
        '
        Me.rgb_Leg2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.rgb_Leg2.Controls.Add(Me.se_VEGA2)
        Me.rgb_Leg2.Controls.Add(Me.se_THETA2)
        Me.rgb_Leg2.Controls.Add(Me.se_DELTA2)
        Me.rgb_Leg2.Controls.Add(Me.se_RHO2)
        Me.rgb_Leg2.Controls.Add(Me.se_GAMMA2)
        Me.rgb_Leg2.Controls.Add(Me.cb_OPTION_TYPE2)
        Me.rgb_Leg2.Controls.Add(Me.btn_STRIKE2)
        Me.rgb_Leg2.Controls.Add(Me.btn_VOL2)
        Me.rgb_Leg2.Controls.Add(Me.btn_PRICE2)
        Me.rgb_Leg2.Controls.Add(Me.se_SKEW2)
        Me.rgb_Leg2.Controls.Add(Me.se_QUANTITY2)
        Me.rgb_Leg2.Controls.Add(Me.se_OPTION_PRICE2)
        Me.rgb_Leg2.Controls.Add(Me.se_VOLATILITY2)
        Me.rgb_Leg2.Controls.Add(Me.se_STRIKE2)
        Me.rgb_Leg2.Controls.Add(Me.cb_BS2)
        Me.rgb_Leg2.Controls.Add(Me.se_PAYRECEIVE2)
        Me.rgb_Leg2.Controls.Add(Me.Label7)
        Me.rgb_Leg2.Controls.Add(Me.Label8)
        Me.rgb_Leg2.Controls.Add(Me.Label9)
        Me.rgb_Leg2.Controls.Add(Me.Label10)
        Me.rgb_Leg2.Controls.Add(Me.Label11)
        Me.rgb_Leg2.Controls.Add(Me.Label12)
        Me.rgb_Leg2.Controls.Add(Me.Label14)
        Me.rgb_Leg2.Controls.Add(Me.Label16)
        Me.rgb_Leg2.Controls.Add(Me.Label17)
        Me.rgb_Leg2.Controls.Add(Me.Label18)
        Me.rgb_Leg2.Controls.Add(Me.Label35)
        Me.rgb_Leg2.Controls.Add(Me.Label36)
        Me.rgb_Leg2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rgb_Leg2.HeaderText = "Leg 2"
        Me.rgb_Leg2.HeaderTextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.rgb_Leg2.Location = New System.Drawing.Point(394, 272)
        Me.rgb_Leg2.Name = "rgb_Leg2"
        '
        '
        '
        Me.rgb_Leg2.RootElement.Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        Me.rgb_Leg2.Size = New System.Drawing.Size(360, 155)
        Me.rgb_Leg2.TabIndex = 6
        Me.rgb_Leg2.Text = "Leg 2"
        Me.rgb_Leg2.ThemeName = "Office2010Silver"
        '
        'se_VEGA2
        '
        Me.se_VEGA2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_VEGA2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_VEGA2.Location = New System.Drawing.Point(289, 99)
        Me.se_VEGA2.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_VEGA2.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_VEGA2.Name = "se_VEGA2"
        Me.se_VEGA2.ReadOnly = True
        '
        '
        '
        Me.se_VEGA2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_VEGA2.ShowUpDownButtons = False
        Me.se_VEGA2.Size = New System.Drawing.Size(65, 18)
        Me.se_VEGA2.TabIndex = 148
        Me.se_VEGA2.TabStop = False
        Me.se_VEGA2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_VEGA2.ThemeName = "Office2010Silver"
        Me.se_VEGA2.ThousandsSeparator = True
        '
        'se_THETA2
        '
        Me.se_THETA2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_THETA2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_THETA2.Location = New System.Drawing.Point(214, 99)
        Me.se_THETA2.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_THETA2.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_THETA2.Name = "se_THETA2"
        Me.se_THETA2.ReadOnly = True
        '
        '
        '
        Me.se_THETA2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_THETA2.ShowUpDownButtons = False
        Me.se_THETA2.Size = New System.Drawing.Size(72, 18)
        Me.se_THETA2.TabIndex = 147
        Me.se_THETA2.TabStop = False
        Me.se_THETA2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_THETA2.ThemeName = "Office2010Silver"
        Me.se_THETA2.ThousandsSeparator = True
        '
        'se_DELTA2
        '
        Me.se_DELTA2.DecimalPlaces = 2
        Me.se_DELTA2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_DELTA2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_DELTA2.Location = New System.Drawing.Point(4, 99)
        Me.se_DELTA2.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_DELTA2.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_DELTA2.Name = "se_DELTA2"
        Me.se_DELTA2.ReadOnly = True
        '
        '
        '
        Me.se_DELTA2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_DELTA2.ShowUpDownButtons = False
        Me.se_DELTA2.Size = New System.Drawing.Size(62, 18)
        Me.se_DELTA2.TabIndex = 144
        Me.se_DELTA2.TabStop = False
        Me.se_DELTA2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_DELTA2.ThemeName = "Office2010Silver"
        Me.se_DELTA2.ThousandsSeparator = True
        '
        'se_RHO2
        '
        Me.se_RHO2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_RHO2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_RHO2.Location = New System.Drawing.Point(141, 99)
        Me.se_RHO2.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_RHO2.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_RHO2.Name = "se_RHO2"
        Me.se_RHO2.ReadOnly = True
        '
        '
        '
        Me.se_RHO2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_RHO2.ShowUpDownButtons = False
        Me.se_RHO2.Size = New System.Drawing.Size(70, 18)
        Me.se_RHO2.TabIndex = 146
        Me.se_RHO2.TabStop = False
        Me.se_RHO2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_RHO2.ThemeName = "Office2010Silver"
        Me.se_RHO2.ThousandsSeparator = True
        '
        'se_GAMMA2
        '
        Me.se_GAMMA2.DecimalPlaces = 4
        Me.se_GAMMA2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_GAMMA2.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_GAMMA2.Location = New System.Drawing.Point(69, 99)
        Me.se_GAMMA2.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_GAMMA2.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_GAMMA2.Name = "se_GAMMA2"
        Me.se_GAMMA2.ReadOnly = True
        '
        '
        '
        Me.se_GAMMA2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_GAMMA2.ShowUpDownButtons = False
        Me.se_GAMMA2.Size = New System.Drawing.Size(70, 18)
        Me.se_GAMMA2.TabIndex = 145
        Me.se_GAMMA2.TabStop = False
        Me.se_GAMMA2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_GAMMA2.ThemeName = "Office2010Silver"
        Me.se_GAMMA2.ThousandsSeparator = True
        '
        'cb_OPTION_TYPE2
        '
        Me.cb_OPTION_TYPE2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cb_OPTION_TYPE2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.cb_OPTION_TYPE2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_OPTION_TYPE2.Location = New System.Drawing.Point(64, 36)
        Me.cb_OPTION_TYPE2.Name = "cb_OPTION_TYPE2"
        Me.cb_OPTION_TYPE2.Size = New System.Drawing.Size(52, 22)
        Me.cb_OPTION_TYPE2.TabIndex = 165
        Me.cb_OPTION_TYPE2.Text = "RadDropDownList1"
        Me.cb_OPTION_TYPE2.ThemeName = "Office2010Silver"
        '
        'btn_STRIKE2
        '
        Me.btn_STRIKE2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.btn_STRIKE2.Location = New System.Drawing.Point(117, 124)
        Me.btn_STRIKE2.Name = "btn_STRIKE2"
        Me.btn_STRIKE2.Size = New System.Drawing.Size(76, 24)
        Me.btn_STRIKE2.TabIndex = 164
        Me.btn_STRIKE2.Text = "Strike Prc"
        Me.btn_STRIKE2.ThemeName = "Office2010Silver"
        '
        'btn_VOL2
        '
        Me.btn_VOL2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.btn_VOL2.Location = New System.Drawing.Point(198, 124)
        Me.btn_VOL2.Name = "btn_VOL2"
        Me.btn_VOL2.Size = New System.Drawing.Size(76, 24)
        Me.btn_VOL2.TabIndex = 163
        Me.btn_VOL2.Text = "IVol%"
        Me.btn_VOL2.ThemeName = "Office2010Silver"
        '
        'btn_PRICE2
        '
        Me.btn_PRICE2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.btn_PRICE2.Location = New System.Drawing.Point(279, 124)
        Me.btn_PRICE2.Name = "btn_PRICE2"
        Me.btn_PRICE2.Size = New System.Drawing.Size(76, 24)
        Me.btn_PRICE2.TabIndex = 162
        Me.btn_PRICE2.Text = "Price"
        Me.btn_PRICE2.ThemeName = "Office2010Silver"
        '
        'se_SKEW2
        '
        Me.se_SKEW2.DecimalPlaces = 2
        Me.se_SKEW2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_SKEW2.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.se_SKEW2.Location = New System.Drawing.Point(206, 61)
        Me.se_SKEW2.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.se_SKEW2.Name = "se_SKEW2"
        Me.se_SKEW2.Size = New System.Drawing.Size(65, 20)
        Me.se_SKEW2.TabIndex = 161
        Me.se_SKEW2.TabStop = False
        Me.se_SKEW2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_SKEW2.ThemeName = "Office2010Silver"
        '
        'se_QUANTITY2
        '
        Me.se_QUANTITY2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_QUANTITY2.Location = New System.Drawing.Point(64, 61)
        Me.se_QUANTITY2.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.se_QUANTITY2.Minimum = New Decimal(New Integer() {5, 0, 0, -2147483648})
        Me.se_QUANTITY2.Name = "se_QUANTITY2"
        Me.se_QUANTITY2.Size = New System.Drawing.Size(52, 20)
        Me.se_QUANTITY2.TabIndex = 160
        Me.se_QUANTITY2.TabStop = False
        Me.se_QUANTITY2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_QUANTITY2.ThemeName = "Office2010Silver"
        Me.se_QUANTITY2.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'se_OPTION_PRICE2
        '
        Me.se_OPTION_PRICE2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_OPTION_PRICE2.Increment = New Decimal(New Integer() {250, 0, 0, 0})
        Me.se_OPTION_PRICE2.Location = New System.Drawing.Point(274, 37)
        Me.se_OPTION_PRICE2.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.se_OPTION_PRICE2.Name = "se_OPTION_PRICE2"
        Me.se_OPTION_PRICE2.Size = New System.Drawing.Size(81, 20)
        Me.se_OPTION_PRICE2.TabIndex = 159
        Me.se_OPTION_PRICE2.TabStop = False
        Me.se_OPTION_PRICE2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_OPTION_PRICE2.ThemeName = "Office2010Silver"
        Me.se_OPTION_PRICE2.ThousandsSeparator = True
        '
        'se_VOLATILITY2
        '
        Me.se_VOLATILITY2.DecimalPlaces = 2
        Me.se_VOLATILITY2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_VOLATILITY2.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.se_VOLATILITY2.Location = New System.Drawing.Point(206, 37)
        Me.se_VOLATILITY2.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.se_VOLATILITY2.Name = "se_VOLATILITY2"
        Me.se_VOLATILITY2.Size = New System.Drawing.Size(65, 20)
        Me.se_VOLATILITY2.TabIndex = 158
        Me.se_VOLATILITY2.TabStop = False
        Me.se_VOLATILITY2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_VOLATILITY2.ThemeName = "Office2010Silver"
        '
        'se_STRIKE2
        '
        Me.se_STRIKE2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_STRIKE2.Increment = New Decimal(New Integer() {250, 0, 0, 0})
        Me.se_STRIKE2.Location = New System.Drawing.Point(122, 37)
        Me.se_STRIKE2.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.se_STRIKE2.Name = "se_STRIKE2"
        Me.se_STRIKE2.Size = New System.Drawing.Size(81, 20)
        Me.se_STRIKE2.TabIndex = 157
        Me.se_STRIKE2.TabStop = False
        Me.se_STRIKE2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_STRIKE2.ThemeName = "Office2010Silver"
        Me.se_STRIKE2.ThousandsSeparator = True
        '
        'cb_BS2
        '
        Me.cb_BS2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cb_BS2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.cb_BS2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_BS2.Location = New System.Drawing.Point(5, 37)
        Me.cb_BS2.Name = "cb_BS2"
        Me.cb_BS2.Size = New System.Drawing.Size(52, 22)
        Me.cb_BS2.TabIndex = 155
        Me.cb_BS2.Text = "RadDropDownList1"
        Me.cb_BS2.ThemeName = "Office2010Silver"
        '
        'se_PAYRECEIVE2
        '
        Me.se_PAYRECEIVE2.DecimalPlaces = 2
        Me.se_PAYRECEIVE2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_PAYRECEIVE2.Location = New System.Drawing.Point(4, 126)
        Me.se_PAYRECEIVE2.Maximum = New Decimal(New Integer() {-727379969, 232, 0, 0})
        Me.se_PAYRECEIVE2.Minimum = New Decimal(New Integer() {-727379969, 232, 0, -2147483648})
        Me.se_PAYRECEIVE2.Name = "se_PAYRECEIVE2"
        Me.se_PAYRECEIVE2.ReadOnly = True
        '
        '
        '
        Me.se_PAYRECEIVE2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_PAYRECEIVE2.ShowUpDownButtons = False
        Me.se_PAYRECEIVE2.Size = New System.Drawing.Size(108, 20)
        Me.se_PAYRECEIVE2.TabIndex = 154
        Me.se_PAYRECEIVE2.TabStop = False
        Me.se_PAYRECEIVE2.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_PAYRECEIVE2.ThemeName = "Office2010Silver"
        Me.se_PAYRECEIVE2.ThousandsSeparator = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label7.Location = New System.Drawing.Point(202, 22)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 13)
        Me.Label7.TabIndex = 143
        Me.Label7.Text = "Implied Vol%"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label8.Location = New System.Drawing.Point(288, 84)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(32, 13)
        Me.Label8.TabIndex = 153
        Me.Label8.Text = "Vega"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label9.Location = New System.Drawing.Point(212, 84)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 13)
        Me.Label9.TabIndex = 152
        Me.Label9.Text = "Theta"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label10.Location = New System.Drawing.Point(271, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(65, 13)
        Me.Label10.TabIndex = 140
        Me.Label10.Text = "Option Price"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label11.Location = New System.Drawing.Point(11, 63)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(52, 13)
        Me.Label11.TabIndex = 142
        Me.Label11.Text = "Q < 0: full"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label12.Location = New System.Drawing.Point(141, 84)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(27, 13)
        Me.Label12.TabIndex = 151
        Me.Label12.Text = "Rho"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.SystemColors.Control
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label14.Location = New System.Drawing.Point(70, 84)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(43, 13)
        Me.Label14.TabIndex = 150
        Me.Label14.Text = "Gamma"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.SystemColors.Control
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label16.Location = New System.Drawing.Point(6, 84)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(32, 13)
        Me.Label16.TabIndex = 149
        Me.Label16.Text = "Delta"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label17.Location = New System.Drawing.Point(154, 63)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(45, 13)
        Me.Label17.TabIndex = 141
        Me.Label17.Text = "Skew %"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label18.Location = New System.Drawing.Point(122, 22)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(61, 13)
        Me.Label18.TabIndex = 139
        Me.Label18.Text = "Strike Price"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label35.Location = New System.Drawing.Point(66, 22)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(45, 13)
        Me.Label35.TabIndex = 138
        Me.Label35.Text = "Call/Put"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label36.Location = New System.Drawing.Point(2, 22)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(47, 13)
        Me.Label36.TabIndex = 137
        Me.Label36.Text = "Buy/Sell"
        '
        'rgb_StrategyResults
        '
        Me.rgb_StrategyResults.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.rgb_StrategyResults.Controls.Add(Me.btn_PRICE)
        Me.rgb_StrategyResults.Controls.Add(Me.se_DELTA)
        Me.rgb_StrategyResults.Controls.Add(Me.se_STRATEGY_PRICE)
        Me.rgb_StrategyResults.Controls.Add(Me.se_PAYRECEIVE)
        Me.rgb_StrategyResults.Controls.Add(Me.Label15)
        Me.rgb_StrategyResults.Controls.Add(Me.Label25)
        Me.rgb_StrategyResults.Controls.Add(Me.Label24)
        Me.rgb_StrategyResults.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rgb_StrategyResults.HeaderText = "Strategy Results"
        Me.rgb_StrategyResults.HeaderTextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.rgb_StrategyResults.Location = New System.Drawing.Point(394, 431)
        Me.rgb_StrategyResults.Name = "rgb_StrategyResults"
        '
        '
        '
        Me.rgb_StrategyResults.RootElement.Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        Me.rgb_StrategyResults.Size = New System.Drawing.Size(360, 63)
        Me.rgb_StrategyResults.TabIndex = 7
        Me.rgb_StrategyResults.Text = "Strategy Results"
        Me.rgb_StrategyResults.ThemeName = "Office2010Silver"
        '
        'btn_PRICE
        '
        Me.btn_PRICE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.btn_PRICE.Location = New System.Drawing.Point(189, 33)
        Me.btn_PRICE.Name = "btn_PRICE"
        Me.btn_PRICE.Size = New System.Drawing.Size(76, 24)
        Me.btn_PRICE.TabIndex = 139
        Me.btn_PRICE.Text = "Calc All"
        Me.btn_PRICE.ThemeName = "Office2010Silver"
        '
        'se_DELTA
        '
        Me.se_DELTA.DecimalPlaces = 1
        Me.se_DELTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_DELTA.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.se_DELTA.Location = New System.Drawing.Point(5, 36)
        Me.se_DELTA.Maximum = New Decimal(New Integer() {-1530494977, 232830, 0, 0})
        Me.se_DELTA.Minimum = New Decimal(New Integer() {276447231, 23283, 0, -2147483648})
        Me.se_DELTA.Name = "se_DELTA"
        Me.se_DELTA.ReadOnly = True
        '
        '
        '
        Me.se_DELTA.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_DELTA.ShowUpDownButtons = False
        Me.se_DELTA.Size = New System.Drawing.Size(62, 18)
        Me.se_DELTA.TabIndex = 137
        Me.se_DELTA.TabStop = False
        Me.se_DELTA.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_DELTA.ThemeName = "Office2010Silver"
        Me.se_DELTA.ThousandsSeparator = True
        '
        'se_STRATEGY_PRICE
        '
        Me.se_STRATEGY_PRICE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_STRATEGY_PRICE.Location = New System.Drawing.Point(275, 36)
        Me.se_STRATEGY_PRICE.Maximum = New Decimal(New Integer() {-727379969, 232, 0, 0})
        Me.se_STRATEGY_PRICE.Minimum = New Decimal(New Integer() {-727379969, 232, 0, -2147483648})
        Me.se_STRATEGY_PRICE.Name = "se_STRATEGY_PRICE"
        Me.se_STRATEGY_PRICE.ReadOnly = True
        '
        '
        '
        Me.se_STRATEGY_PRICE.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_STRATEGY_PRICE.ShowUpDownButtons = False
        Me.se_STRATEGY_PRICE.Size = New System.Drawing.Size(78, 20)
        Me.se_STRATEGY_PRICE.TabIndex = 136
        Me.se_STRATEGY_PRICE.TabStop = False
        Me.se_STRATEGY_PRICE.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_STRATEGY_PRICE.ThemeName = "Office2010Silver"
        Me.se_STRATEGY_PRICE.ThousandsSeparator = True
        '
        'se_PAYRECEIVE
        '
        Me.se_PAYRECEIVE.DecimalPlaces = 2
        Me.se_PAYRECEIVE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_PAYRECEIVE.Location = New System.Drawing.Point(71, 36)
        Me.se_PAYRECEIVE.Maximum = New Decimal(New Integer() {-727379969, 232, 0, 0})
        Me.se_PAYRECEIVE.Minimum = New Decimal(New Integer() {-727379969, 232, 0, -2147483648})
        Me.se_PAYRECEIVE.Name = "se_PAYRECEIVE"
        Me.se_PAYRECEIVE.ReadOnly = True
        '
        '
        '
        Me.se_PAYRECEIVE.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.se_PAYRECEIVE.ShowUpDownButtons = False
        Me.se_PAYRECEIVE.Size = New System.Drawing.Size(108, 20)
        Me.se_PAYRECEIVE.TabIndex = 135
        Me.se_PAYRECEIVE.TabStop = False
        Me.se_PAYRECEIVE.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_PAYRECEIVE.ThemeName = "Office2010Silver"
        Me.se_PAYRECEIVE.ThousandsSeparator = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.SystemColors.Control
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label15.Location = New System.Drawing.Point(6, 20)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(49, 13)
        Me.Label15.TabIndex = 138
        Me.Label15.Text = "Delta (Q)"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.BackColor = System.Drawing.SystemColors.Control
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label25.Location = New System.Drawing.Point(70, 20)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(76, 13)
        Me.Label25.TabIndex = 134
        Me.Label25.Text = "Pay / Receive"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.BackColor = System.Drawing.SystemColors.Control
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label24.Location = New System.Drawing.Point(277, 20)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(73, 13)
        Me.Label24.TabIndex = 133
        Me.Label24.Text = "Strategy Price"
        '
        'rgb_Solver_Parameters
        '
        Me.rgb_Solver_Parameters.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.rgb_Solver_Parameters.Controls.Add(Me.MC_PMAX)
        Me.rgb_Solver_Parameters.Controls.Add(Me.Label34)
        Me.rgb_Solver_Parameters.Controls.Add(Me.pb_NVIDIA)
        Me.rgb_Solver_Parameters.Controls.Add(Me.rbtn_MC_CUDA)
        Me.rgb_Solver_Parameters.Controls.Add(Me.rbtn_ANALYTIC)
        Me.rgb_Solver_Parameters.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rgb_Solver_Parameters.HeaderText = "Solver Parameters"
        Me.rgb_Solver_Parameters.HeaderTextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.rgb_Solver_Parameters.Location = New System.Drawing.Point(394, 497)
        Me.rgb_Solver_Parameters.Name = "rgb_Solver_Parameters"
        Me.rgb_Solver_Parameters.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
        '
        '
        '
        Me.rgb_Solver_Parameters.RootElement.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
        Me.rgb_Solver_Parameters.Size = New System.Drawing.Size(360, 49)
        Me.rgb_Solver_Parameters.TabIndex = 87
        Me.rgb_Solver_Parameters.Text = "Solver Parameters"
        Me.rgb_Solver_Parameters.ThemeName = "Office2010Silver"
        '
        'MC_PMAX
        '
        Me.MC_PMAX.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.MC_PMAX.Increment = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.MC_PMAX.Location = New System.Drawing.Point(230, 21)
        Me.MC_PMAX.Maximum = New Decimal(New Integer() {250000, 0, 0, 0})
        Me.MC_PMAX.Minimum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.MC_PMAX.Name = "MC_PMAX"
        '
        '
        '
        Me.MC_PMAX.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.MC_PMAX.Size = New System.Drawing.Size(73, 18)
        Me.MC_PMAX.TabIndex = 71
        Me.MC_PMAX.TabStop = False
        Me.MC_PMAX.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.MC_PMAX.ThemeName = "Office2010Silver"
        Me.MC_PMAX.ThousandsSeparator = True
        Me.MC_PMAX.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label34.Location = New System.Drawing.Point(193, 24)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(37, 13)
        Me.Label34.TabIndex = 98
        Me.Label34.Text = "Steps:"
        '
        'pb_NVIDIA
        '
        Me.pb_NVIDIA.Image = Global.FFASuite.My.Resources.Resources.NV_Tesla_3D
        Me.pb_NVIDIA.Location = New System.Drawing.Point(317, 11)
        Me.pb_NVIDIA.Name = "pb_NVIDIA"
        Me.pb_NVIDIA.Size = New System.Drawing.Size(37, 35)
        Me.pb_NVIDIA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pb_NVIDIA.TabIndex = 87
        Me.pb_NVIDIA.TabStop = False
        '
        'rbtn_MC_CUDA
        '
        Me.rbtn_MC_CUDA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_MC_CUDA.Location = New System.Drawing.Point(111, 23)
        Me.rbtn_MC_CUDA.Name = "rbtn_MC_CUDA"
        Me.rbtn_MC_CUDA.Size = New System.Drawing.Size(78, 16)
        Me.rbtn_MC_CUDA.TabIndex = 2
        Me.rbtn_MC_CUDA.Text = "MC  CUDA"
        Me.rbtn_MC_CUDA.ThemeName = "Office2010Silver"
        '
        'rbtn_ANALYTIC
        '
        Me.rbtn_ANALYTIC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_ANALYTIC.Location = New System.Drawing.Point(13, 23)
        Me.rbtn_ANALYTIC.Name = "rbtn_ANALYTIC"
        Me.rbtn_ANALYTIC.Size = New System.Drawing.Size(62, 16)
        Me.rbtn_ANALYTIC.TabIndex = 0
        Me.rbtn_ANALYTIC.TabStop = True
        Me.rbtn_ANALYTIC.Text = "Analytic"
        Me.rbtn_ANALYTIC.ThemeName = "Office2010Silver"
        Me.rbtn_ANALYTIC.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        '
        'ArtBOptCalcControlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(755, 546)
        Me.Controls.Add(Me.rgb_Solver_Parameters)
        Me.Controls.Add(Me.rgb_StrategyResults)
        Me.Controls.Add(Me.rgb_Leg2)
        Me.Controls.Add(Me.rgb_Leg1)
        Me.Controls.Add(Me.rgb_SwapDetails)
        Me.Controls.Add(Me.rgb_Graphical_Data)
        Me.Controls.Add(Me.rgb_Swap_Periods)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "ArtBOptCalcControlForm"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "FFA Option Calculator"
        Me.ThemeName = "Office2010Silver"
        CType(Me.rgb_Graphical_Data, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rgb_Graphical_Data.ResumeLayout(False)
        Me.rgb_Graphical_Data.PerformLayout()
        CType(Me.MSChart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rss_GRAPH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgb_Swap_Periods, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rgb_Swap_Periods.ResumeLayout(False)
        CType(Me.rgv_PERIODS.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_PERIODS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_BS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgb_SwapDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rgb_SwapDetails.ResumeLayout(False)
        Me.rgb_SwapDetails.PerformLayout()
        CType(Me.se_RISK_FREE_RATE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_AVG_PRICE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_VOLATILITY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_FFA_PRICE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_YY2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_MM2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_YY1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_MM1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgb_Leg1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rgb_Leg1.ResumeLayout(False)
        Me.rgb_Leg1.PerformLayout()
        CType(Me.se_VEGA1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_THETA1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_DELTA1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_RHO1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_GAMMA1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_PRICE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_STRIKE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_VOL1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_SKEW1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_QUANTITY1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_OPTION_PRICE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_VOLATILITY1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_STRIKE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_OPTION_TYPE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_BS1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_PAYRECEIVE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgb_Leg2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rgb_Leg2.ResumeLayout(False)
        Me.rgb_Leg2.PerformLayout()
        CType(Me.se_VEGA2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_THETA2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_DELTA2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_RHO2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_GAMMA2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_OPTION_TYPE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_STRIKE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_VOL2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_PRICE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_SKEW2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_QUANTITY2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_OPTION_PRICE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_VOLATILITY2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_STRIKE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_BS2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_PAYRECEIVE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgb_StrategyResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rgb_StrategyResults.ResumeLayout(False)
        Me.rgb_StrategyResults.PerformLayout()
        CType(Me.btn_PRICE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_DELTA, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_STRATEGY_PRICE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.se_PAYRECEIVE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgb_Solver_Parameters, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rgb_Solver_Parameters.ResumeLayout(False)
        Me.rgb_Solver_Parameters.PerformLayout()
        CType(Me.MC_PMAX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pb_NVIDIA, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_MC_CUDA, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_ANALYTIC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rgb_Graphical_Data As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents MSChart As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents rss_GRAPH As Telerik.WinControls.UI.RadStatusStrip
    Friend WithEvents rcb_FFAIVol As Telerik.WinControls.UI.RadCheckBoxElement
    Friend WithEvents rcb_FFAHVol As Telerik.WinControls.UI.RadCheckBoxElement
    Friend WithEvents rcb_SpotHVol As Telerik.WinControls.UI.RadCheckBoxElement
    Friend WithEvents CommandBarSeparator1 As Telerik.WinControls.UI.CommandBarSeparator
    Friend WithEvents rcb_FFA As Telerik.WinControls.UI.RadCheckBoxElement
    Friend WithEvents rcb_Spot As Telerik.WinControls.UI.RadCheckBoxElement
    Friend WithEvents rcb_SpotAvg As Telerik.WinControls.UI.RadCheckBoxElement
    Friend WithEvents rgb_Swap_Periods As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents rgv_PERIODS As Telerik.WinControls.UI.RadGridView
    Friend WithEvents rgv_BS As System.Windows.Forms.BindingSource
    Friend WithEvents RadThemeManager1 As Telerik.WinControls.RadThemeManager
    Friend WithEvents rgb_SwapDetails As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents cb_AVERAGE_INCLUDES_TODAY As System.Windows.Forms.CheckBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents rgb_Leg1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents se_VEGA1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_THETA1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_DELTA1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents se_RHO1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_GAMMA1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rgb_Leg2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents rgb_StrategyResults As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents se_DELTA As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_STRATEGY_PRICE As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_PAYRECEIVE As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents se_PAYRECEIVE1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents rgb_Solver_Parameters As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents MC_PMAX As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents pb_NVIDIA As System.Windows.Forms.PictureBox
    Friend WithEvents rbtn_MC_CUDA As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents rbtn_ANALYTIC As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents se_FFA_PRICE As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents cb_YY2 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents cb_MM2 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents cb_YY1 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents cb_MM1 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents se_VOLATILITY As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_AVG_PRICE As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents cb_OPTION_TYPE1 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents se_SKEW1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_QUANTITY1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_OPTION_PRICE1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_VOLATILITY1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_STRIKE1 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents cb_BS1 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents se_SKEW2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents btn_STRIKE1 As Telerik.WinControls.UI.RadButton
    Friend WithEvents btn_PRICE2 As Telerik.WinControls.UI.RadButton
    Friend WithEvents btn_VOL1 As Telerik.WinControls.UI.RadButton
    Friend WithEvents btn_VOL2 As Telerik.WinControls.UI.RadButton
    Friend WithEvents btn_STRIKE2 As Telerik.WinControls.UI.RadButton
    Friend WithEvents se_QUANTITY2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_OPTION_PRICE2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_VOLATILITY2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_STRIKE2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents cb_BS2 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents se_PAYRECEIVE2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents se_VEGA2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_THETA2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_DELTA2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents se_RHO2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents se_GAMMA2 As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents btn_PRICE As Telerik.WinControls.UI.RadButton
    Friend WithEvents cb_OPTION_TYPE2 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents btn_PRICE1 As Telerik.WinControls.UI.RadButton
    Friend WithEvents se_RISK_FREE_RATE As Telerik.WinControls.UI.RadSpinEditor
End Class

