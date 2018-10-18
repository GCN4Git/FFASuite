<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MarketWatch
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
        Me.rgv_BS = New System.Windows.Forms.BindingSource(Me.components)
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rgv_MARKET = New Telerik.WinControls.UI.RadGridView()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        CType(Me.rgv_BS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_MARKET, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_MARKET.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rgv_MARKET
        '
        Me.rgv_MARKET.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rgv_MARKET.Location = New System.Drawing.Point(0, 0)
        '
        'rgv_MARKET
        '
        Me.rgv_MARKET.MasterTemplate.AllowAddNewRow = False
        Me.rgv_MARKET.MasterTemplate.AllowDeleteRow = False
        Me.rgv_MARKET.MasterTemplate.EnableAlternatingRowColor = True
        Me.rgv_MARKET.MasterTemplate.EnableGrouping = False
        Me.rgv_MARKET.MasterTemplate.EnableSorting = False
        Me.rgv_MARKET.MasterTemplate.ShowFilteringRow = False
        Me.rgv_MARKET.MasterTemplate.ShowRowHeaderColumn = False
        Me.rgv_MARKET.Name = "rgv_MARKET"
        Me.rgv_MARKET.ShowGroupPanel = False
        Me.rgv_MARKET.Size = New System.Drawing.Size(599, 447)
        Me.rgv_MARKET.TabIndex = 0
        Me.rgv_MARKET.Text = "RadGridView1"
        Me.rgv_MARKET.ThemeName = "Office2010Silver"
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.rgv_MARKET)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(599, 447)
        Me.RadPanel1.TabIndex = 1
        Me.RadPanel1.Text = "RadPanel1"
        Me.RadPanel1.ThemeName = "Office2010Silver"
        '
        'MarketWatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(599, 447)
        Me.Controls.Add(Me.RadPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Name = "MarketWatch"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "FFA Market Watch"
        Me.ThemeName = "Office2010Silver"
        CType(Me.rgv_BS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_MARKET.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_MARKET, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents rgv_BS As System.Windows.Forms.BindingSource
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rgv_MARKET As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
End Class

