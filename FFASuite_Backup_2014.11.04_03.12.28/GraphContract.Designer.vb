<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GraphContract
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
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.MSChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.m_GraphContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.rmi_Historical = New System.Windows.Forms.ToolStripMenuItem()
        Me.rmi_Live = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.MSChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.m_GraphContextMenu.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MSChart
        '
        ChartArea1.AxisX2.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount
        ChartArea1.AxisX2.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months
        ChartArea1.AxisX2.IsStartedFromZero = False
        ChartArea1.AxisY.LabelStyle.Format = "P0"
        ChartArea1.Name = "Default"
        Me.MSChart.ChartAreas.Add(ChartArea1)
        Me.MSChart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MSChart.Location = New System.Drawing.Point(0, 0)
        Me.MSChart.Name = "MSChart"
        Series1.ChartArea = "Default"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Name = "FFA"
        Series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Date]
        Series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series2.ChartArea = "Default"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series2.IsXValueIndexed = True
        Series2.Name = "LIVE"
        Series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime
        Me.MSChart.Series.Add(Series1)
        Me.MSChart.Series.Add(Series2)
        Me.MSChart.Size = New System.Drawing.Size(442, 291)
        Me.MSChart.TabIndex = 5
        Me.MSChart.Text = "Chart1"
        '
        'm_GraphContextMenu
        '
        Me.m_GraphContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.rmi_Historical, Me.rmi_Live})
        Me.m_GraphContextMenu.Name = "ContextMenuStrip1"
        Me.m_GraphContextMenu.Size = New System.Drawing.Size(221, 48)
        '
        'rmi_Historical
        '
        Me.rmi_Historical.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_Historical.Image = Global.FFASuite.My.Resources.Resources.Stats2_B16R
        Me.rmi_Historical.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.rmi_Historical.Name = "rmi_Historical"
        Me.rmi_Historical.Size = New System.Drawing.Size(220, 22)
        Me.rmi_Historical.Text = "Display Historical Closings"
        '
        'rmi_Live
        '
        Me.rmi_Live.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_Live.Image = Global.FFASuite.My.Resources.Resources.Stats2_B16R
        Me.rmi_Live.Name = "rmi_Live"
        Me.rmi_Live.Size = New System.Drawing.Size(220, 22)
        Me.rmi_Live.Text = "Display Intrady Data"
        Me.rmi_Live.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GraphContract
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(442, 291)
        Me.Controls.Add(Me.MSChart)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.MinimumSize = New System.Drawing.Size(350, 260)
        Me.Name = "GraphContract"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "GraphContract_vb"
        Me.ThemeName = "Office2010Silver"
        CType(Me.MSChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.m_GraphContextMenu.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents MSChart As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents m_GraphContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents rmi_Historical As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rmi_Live As System.Windows.Forms.ToolStripMenuItem
End Class

