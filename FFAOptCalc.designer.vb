<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FFAOptCalc
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FFAOptCalc))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rss_FORM = New Telerik.WinControls.UI.RadStatusStrip()
        Me.rib_XMPP = New Telerik.WinControls.UI.RadImageButtonElement()
        Me.rwb_WAIT = New Telerik.WinControls.UI.RadWaitingBarElement()
        Me.rle_FormSatus = New Telerik.WinControls.UI.RadLabelElement()
        Me.RadCommandBar1 = New Telerik.WinControls.UI.RadCommandBar()
        Me.CommandBarRowElement1 = New Telerik.WinControls.UI.CommandBarRowElement()
        Me.CommandBarStripElement1 = New Telerik.WinControls.UI.CommandBarStripElement()
        Me.rmi_HELP_MAIN = New Telerik.WinControls.UI.CommandBarButton()
        Me.CommandBarSeparator1 = New Telerik.WinControls.UI.CommandBarSeparator()
        Me.CommandBarSeparator2 = New Telerik.WinControls.UI.CommandBarSeparator()
        Me.cbddb_SelectTool = New Telerik.WinControls.UI.CommandBarDropDownButton()
        Me.rmi_MarketWatch = New Telerik.WinControls.UI.RadMenuItem()
        Me.RadMenuSeparatorItem1 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
        Me.rmi_DayTrades = New Telerik.WinControls.UI.RadMenuItem()
        Me.RadMenuSeparatorItem2 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
        Me.rmi_settings = New Telerik.WinControls.UI.RadMenuItem()
        Me.rms_Broker = New Telerik.WinControls.UI.RadMenuSeparatorItem()
        Me.rmi_BrokerActions = New Telerik.WinControls.UI.RadMenuItem()
        Me.rmi_SpotRates = New Telerik.WinControls.UI.RadMenuItem()
        Me.rmi_SwapRates = New Telerik.WinControls.UI.RadMenuItem()
        Me.rms_BrokerNuke = New Telerik.WinControls.UI.RadMenuSeparatorItem()
        Me.rmi_NukeUsers = New Telerik.WinControls.UI.RadMenuItem()
        Me.CommandBarRowElement2 = New Telerik.WinControls.UI.CommandBarRowElement()
        Me.CommandBarRowElement3 = New Telerik.WinControls.UI.CommandBarRowElement()
        Me.rd_MAIN = New Telerik.WinControls.UI.Docking.RadDock()
        Me.DocumentContainer1 = New Telerik.WinControls.UI.Docking.DocumentContainer()
        Me.Rad_Alert = New Telerik.WinControls.UI.RadDesktopAlert(Me.components)
        Me.BOSH_KeepAlive = New System.Windows.Forms.Timer(Me.components)
        Me.BOSH_reconnect = New System.Windows.Forms.Timer(Me.components)
        CType(Me.rss_FORM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadCommandBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rd_MAIN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rd_MAIN.SuspendLayout()
        CType(Me.DocumentContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rss_FORM
        '
        Me.rss_FORM.Items.AddRange(New Telerik.WinControls.RadItem() {Me.rib_XMPP, Me.rwb_WAIT, Me.rle_FormSatus})
        Me.rss_FORM.Location = New System.Drawing.Point(0, 577)
        Me.rss_FORM.Name = "rss_FORM"
        Me.rss_FORM.Size = New System.Drawing.Size(767, 26)
        Me.rss_FORM.TabIndex = 16
        Me.rss_FORM.Text = "RadStatusStrip1"
        Me.rss_FORM.ThemeName = "Office2010Silver"
        '
        'rib_XMPP
        '
        Me.rib_XMPP.BackColor = System.Drawing.Color.Red
        Me.rib_XMPP.DefaultSize = New System.Drawing.Size(16, 16)
        Me.rib_XMPP.DisplayStyle = Telerik.WinControls.DisplayStyle.Text
        Me.rib_XMPP.ImageIndexClicked = 0
        Me.rib_XMPP.ImageIndexHovered = 0
        Me.rib_XMPP.Name = "rib_XMPP"
        Me.rib_XMPP.Opacity = 1.0R
        Me.rss_FORM.SetSpring(Me.rib_XMPP, False)
        Me.rib_XMPP.Text = ""
        Me.rib_XMPP.ToolTipText = "Indcates if web connection is present"
        Me.rib_XMPP.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rwb_WAIT
        '
        Me.rwb_WAIT.Alignment = System.Drawing.ContentAlignment.TopLeft
        Me.rwb_WAIT.MinSize = New System.Drawing.Size(175, 18)
        Me.rwb_WAIT.Name = "rwb_WAIT"
        Me.rss_FORM.SetSpring(Me.rwb_WAIT, False)
        Me.rwb_WAIT.Text = ""
        Me.rwb_WAIT.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rle_FormSatus
        '
        Me.rle_FormSatus.AccessibleDescription = "RadLabelElement1"
        Me.rle_FormSatus.AccessibleName = "RadLabelElement1"
        Me.rle_FormSatus.AutoSize = True
        Me.rle_FormSatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rle_FormSatus.Name = "rle_FormSatus"
        Me.rss_FORM.SetSpring(Me.rle_FormSatus, False)
        Me.rle_FormSatus.Text = "......"
        Me.rle_FormSatus.TextWrap = True
        Me.rle_FormSatus.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'RadCommandBar1
        '
        Me.RadCommandBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadCommandBar1.Location = New System.Drawing.Point(0, 0)
        Me.RadCommandBar1.Name = "RadCommandBar1"
        Me.RadCommandBar1.Rows.AddRange(New Telerik.WinControls.UI.CommandBarRowElement() {Me.CommandBarRowElement1})
        Me.RadCommandBar1.Size = New System.Drawing.Size(767, 33)
        Me.RadCommandBar1.TabIndex = 20
        Me.RadCommandBar1.Text = "RadCommandBar1"
        Me.RadCommandBar1.ThemeName = "Office2010Silver"
        '
        'CommandBarRowElement1
        '
        Me.CommandBarRowElement1.MinSize = New System.Drawing.Size(25, 25)
        Me.CommandBarRowElement1.Strips.AddRange(New Telerik.WinControls.UI.CommandBarStripElement() {Me.CommandBarStripElement1})
        '
        'CommandBarStripElement1
        '
        Me.CommandBarStripElement1.DisplayName = "CommandBarStripElement1"
        Me.CommandBarStripElement1.Items.AddRange(New Telerik.WinControls.UI.RadCommandBarBaseItem() {Me.rmi_HELP_MAIN, Me.CommandBarSeparator1, Me.CommandBarSeparator2, Me.cbddb_SelectTool})
        Me.CommandBarStripElement1.Name = "CommandBarStripElement1"
        '
        'rmi_HELP_MAIN
        '
        Me.rmi_HELP_MAIN.AccessibleDescription = "CommandBarButton1"
        Me.rmi_HELP_MAIN.AccessibleName = "CommandBarButton1"
        Me.rmi_HELP_MAIN.DisplayName = "CommandBarButton1"
        Me.rmi_HELP_MAIN.Image = Global.FFASuite.My.Resources.Resources.Question_GR16R
        Me.rmi_HELP_MAIN.Name = "rmi_HELP_MAIN"
        Me.rmi_HELP_MAIN.Text = "CommandBarButton1"
        Me.rmi_HELP_MAIN.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'CommandBarSeparator1
        '
        Me.CommandBarSeparator1.AccessibleDescription = "CommandBarSeparator1"
        Me.CommandBarSeparator1.AccessibleName = "CommandBarSeparator1"
        Me.CommandBarSeparator1.DisplayName = "CommandBarSeparator1"
        Me.CommandBarSeparator1.Name = "CommandBarSeparator1"
        Me.CommandBarSeparator1.Visibility = Telerik.WinControls.ElementVisibility.Visible
        Me.CommandBarSeparator1.VisibleInOverflowMenu = False
        '
        'CommandBarSeparator2
        '
        Me.CommandBarSeparator2.AccessibleDescription = "CommandBarSeparator2"
        Me.CommandBarSeparator2.AccessibleName = "CommandBarSeparator2"
        Me.CommandBarSeparator2.DisplayName = "CommandBarSeparator2"
        Me.CommandBarSeparator2.Name = "CommandBarSeparator2"
        Me.CommandBarSeparator2.Visibility = Telerik.WinControls.ElementVisibility.Visible
        Me.CommandBarSeparator2.VisibleInOverflowMenu = False
        '
        'cbddb_SelectTool
        '
        Me.cbddb_SelectTool.AccessibleDescription = "Select Tool"
        Me.cbddb_SelectTool.AccessibleName = "Select Tool"
        Me.cbddb_SelectTool.DisplayName = "CommandBarDropDownButton1"
        Me.cbddb_SelectTool.DrawText = True
        Me.cbddb_SelectTool.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cbddb_SelectTool.Image = Global.FFASuite.My.Resources.Resources.Cube_B16R
        Me.cbddb_SelectTool.Items.AddRange(New Telerik.WinControls.RadItem() {Me.rmi_MarketWatch, Me.RadMenuSeparatorItem1, Me.rmi_DayTrades, Me.RadMenuSeparatorItem2, Me.rmi_settings, Me.rms_Broker, Me.rmi_BrokerActions})
        Me.cbddb_SelectTool.Name = "cbddb_SelectTool"
        Me.cbddb_SelectTool.Text = "Select Tool"
        Me.cbddb_SelectTool.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cbddb_SelectTool.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rmi_MarketWatch
        '
        Me.rmi_MarketWatch.AccessibleDescription = "Market View"
        Me.rmi_MarketWatch.AccessibleName = "Market View"
        Me.rmi_MarketWatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_MarketWatch.Image = Global.FFASuite.My.Resources.Resources.Table_B16R
        Me.rmi_MarketWatch.Name = "rmi_MarketWatch"
        Me.rmi_MarketWatch.Text = "Market View"
        Me.rmi_MarketWatch.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'RadMenuSeparatorItem1
        '
        Me.RadMenuSeparatorItem1.AccessibleDescription = "RadMenuSeparatorItem1"
        Me.RadMenuSeparatorItem1.AccessibleName = "RadMenuSeparatorItem1"
        Me.RadMenuSeparatorItem1.Name = "RadMenuSeparatorItem1"
        Me.RadMenuSeparatorItem1.Text = "RadMenuSeparatorItem1"
        Me.RadMenuSeparatorItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rmi_DayTrades
        '
        Me.rmi_DayTrades.AccessibleDescription = "Day Trades Report"
        Me.rmi_DayTrades.AccessibleName = "Day Trades Report"
        Me.rmi_DayTrades.Image = Global.FFASuite.My.Resources.Resources.Currency_Dollar_B16R
        Me.rmi_DayTrades.Name = "rmi_DayTrades"
        Me.rmi_DayTrades.Text = "Day Trades Report"
        Me.rmi_DayTrades.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'RadMenuSeparatorItem2
        '
        Me.RadMenuSeparatorItem2.AccessibleDescription = "RadMenuSeparatorItem2"
        Me.RadMenuSeparatorItem2.AccessibleName = "RadMenuSeparatorItem2"
        Me.RadMenuSeparatorItem2.Name = "RadMenuSeparatorItem2"
        Me.RadMenuSeparatorItem2.Text = "RadMenuSeparatorItem2"
        Me.RadMenuSeparatorItem2.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rmi_settings
        '
        Me.rmi_settings.AccessibleDescription = "Settings"
        Me.rmi_settings.AccessibleName = "Settings"
        Me.rmi_settings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_settings.Image = Global.FFASuite.My.Resources.Resources.Tool_B16R
        Me.rmi_settings.Name = "rmi_settings"
        Me.rmi_settings.Text = "Settings"
        Me.rmi_settings.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rms_Broker
        '
        Me.rms_Broker.AccessibleDescription = "RadMenuSeparatorItem2"
        Me.rms_Broker.AccessibleName = "RadMenuSeparatorItem2"
        Me.rms_Broker.Name = "rms_Broker"
        Me.rms_Broker.Text = "RadMenuSeparatorItem2"
        Me.rms_Broker.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rmi_BrokerActions
        '
        Me.rmi_BrokerActions.AccessibleDescription = "Broker Functions"
        Me.rmi_BrokerActions.AccessibleName = "Broker Functions"
        Me.rmi_BrokerActions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_BrokerActions.Image = Global.FFASuite.My.Resources.Resources.Info2__B16R
        Me.rmi_BrokerActions.Items.AddRange(New Telerik.WinControls.RadItem() {Me.rmi_SpotRates, Me.rmi_SwapRates, Me.rms_BrokerNuke, Me.rmi_NukeUsers})
        Me.rmi_BrokerActions.Name = "rmi_BrokerActions"
        Me.rmi_BrokerActions.Text = "Broker Functions"
        Me.rmi_BrokerActions.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rmi_SpotRates
        '
        Me.rmi_SpotRates.AccessibleDescription = "Spot Rates Update"
        Me.rmi_SpotRates.AccessibleName = "Spot Rates Update"
        Me.rmi_SpotRates.DescriptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_SpotRates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_SpotRates.Image = Global.FFASuite.My.Resources.Resources.Currency_Dollar_B16R
        Me.rmi_SpotRates.Name = "rmi_SpotRates"
        Me.rmi_SpotRates.Text = "Spot Rates Update"
        Me.rmi_SpotRates.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rmi_SwapRates
        '
        Me.rmi_SwapRates.AccessibleDescription = "Swap Rates Update"
        Me.rmi_SwapRates.AccessibleName = "Swap Rates Update"
        Me.rmi_SwapRates.DescriptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_SwapRates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_SwapRates.Image = Global.FFASuite.My.Resources.Resources.Currency_Dollar_B16R
        Me.rmi_SwapRates.Name = "rmi_SwapRates"
        Me.rmi_SwapRates.Text = "Swap Rates Update"
        Me.rmi_SwapRates.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rms_BrokerNuke
        '
        Me.rms_BrokerNuke.AccessibleDescription = "RadMenuSeparatorItem2"
        Me.rms_BrokerNuke.AccessibleName = "RadMenuSeparatorItem2"
        Me.rms_BrokerNuke.Name = "rms_BrokerNuke"
        Me.rms_BrokerNuke.Text = "RadMenuSeparatorItem2"
        Me.rms_BrokerNuke.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'rmi_NukeUsers
        '
        Me.rmi_NukeUsers.AccessibleDescription = "Disconnect Users"
        Me.rmi_NukeUsers.AccessibleName = "Disconnect Users"
        Me.rmi_NukeUsers.DescriptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_NukeUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rmi_NukeUsers.Image = Global.FFASuite.My.Resources.Resources.Nuke_R16R
        Me.rmi_NukeUsers.Name = "rmi_NukeUsers"
        Me.rmi_NukeUsers.Text = "Disconnect Users"
        Me.rmi_NukeUsers.Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'CommandBarRowElement2
        '
        Me.CommandBarRowElement2.MinSize = New System.Drawing.Size(25, 25)
        '
        'CommandBarRowElement3
        '
        Me.CommandBarRowElement3.MinSize = New System.Drawing.Size(25, 25)
        '
        'rd_MAIN
        '
        Me.rd_MAIN.AutoDetectMdiChildren = True
        Me.rd_MAIN.BackColor = System.Drawing.SystemColors.Control
        Me.rd_MAIN.Controls.Add(Me.DocumentContainer1)
        Me.rd_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rd_MAIN.DocumentManager.DocumentInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.ToBack
        Me.rd_MAIN.IsCleanUpTarget = True
        Me.rd_MAIN.Location = New System.Drawing.Point(0, 33)
        Me.rd_MAIN.MainDocumentContainer = Me.DocumentContainer1
        Me.rd_MAIN.Name = "rd_MAIN"
        '
        '
        '
        Me.rd_MAIN.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.rd_MAIN.RootElement.Padding = New System.Windows.Forms.Padding(5)
        Me.rd_MAIN.ShowDocumentCloseButton = True
        Me.rd_MAIN.Size = New System.Drawing.Size(767, 544)
        Me.rd_MAIN.SplitterWidth = 3
        Me.rd_MAIN.TabIndex = 22
        Me.rd_MAIN.TabStop = False
        Me.rd_MAIN.Text = "RadDock1"
        Me.rd_MAIN.ThemeName = "Office2010Silver"
        Me.rd_MAIN.ToolWindowInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.ToBack
        '
        'DocumentContainer1
        '
        Me.DocumentContainer1.Name = "DocumentContainer1"
        '
        '
        '
        Me.DocumentContainer1.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.DocumentContainer1.RootElement.Padding = New System.Windows.Forms.Padding(5)
        Me.DocumentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill
        Me.DocumentContainer1.SplitterWidth = 3
        Me.DocumentContainer1.ThemeName = "Office2010Silver"
        '
        'Rad_Alert
        '
        Me.Rad_Alert.CaptionText = "ArtB Alert"
        Me.Rad_Alert.ContentText = "Spot Rates Updated"
        Me.Rad_Alert.Opacity = 1.0!
        Me.Rad_Alert.ThemeName = "Office2010Silver"
        '
        'BOSH_KeepAlive
        '
        Me.BOSH_KeepAlive.Interval = 30000
        '
        'BOSH_reconnect
        '
        Me.BOSH_reconnect.Interval = 2000
        '
        'FFAOptCalc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(767, 603)
        Me.Controls.Add(Me.rd_MAIN)
        Me.Controls.Add(Me.RadCommandBar1)
        Me.Controls.Add(Me.rss_FORM)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "FFAOptCalc"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "FFAs Suite"
        Me.ThemeName = "Office2010Silver"
        CType(Me.rss_FORM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadCommandBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rd_MAIN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rd_MAIN.ResumeLayout(False)
        CType(Me.DocumentContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rss_FORM As Telerik.WinControls.UI.RadStatusStrip
    Friend WithEvents rib_XMPP As Telerik.WinControls.UI.RadImageButtonElement
    Friend WithEvents RadCommandBar1 As Telerik.WinControls.UI.RadCommandBar
    Friend WithEvents CommandBarRowElement1 As Telerik.WinControls.UI.CommandBarRowElement
    Friend WithEvents CommandBarStripElement1 As Telerik.WinControls.UI.CommandBarStripElement
    Friend WithEvents CommandBarRowElement2 As Telerik.WinControls.UI.CommandBarRowElement
    Friend WithEvents CommandBarRowElement3 As Telerik.WinControls.UI.CommandBarRowElement
    Friend WithEvents rd_MAIN As Telerik.WinControls.UI.Docking.RadDock
    Friend WithEvents DocumentContainer1 As Telerik.WinControls.UI.Docking.DocumentContainer
    Friend WithEvents Rad_Alert As Telerik.WinControls.UI.RadDesktopAlert
    Friend WithEvents rwb_WAIT As Telerik.WinControls.UI.RadWaitingBarElement
    Friend WithEvents BOSH_KeepAlive As System.Windows.Forms.Timer
    Friend WithEvents CommandBarSeparator1 As Telerik.WinControls.UI.CommandBarSeparator
    Friend WithEvents CommandBarSeparator2 As Telerik.WinControls.UI.CommandBarSeparator
    Friend WithEvents rle_FormSatus As Telerik.WinControls.UI.RadLabelElement
    Friend WithEvents BOSH_reconnect As System.Windows.Forms.Timer
    Friend WithEvents cbddb_SelectTool As Telerik.WinControls.UI.CommandBarDropDownButton
    Friend WithEvents rmi_MarketWatch As Telerik.WinControls.UI.RadMenuItem
    Friend WithEvents RadMenuSeparatorItem1 As Telerik.WinControls.UI.RadMenuSeparatorItem
    Friend WithEvents rmi_settings As Telerik.WinControls.UI.RadMenuItem
    Friend WithEvents rms_Broker As Telerik.WinControls.UI.RadMenuSeparatorItem
    Friend WithEvents rmi_BrokerActions As Telerik.WinControls.UI.RadMenuItem
    Friend WithEvents rmi_SpotRates As Telerik.WinControls.UI.RadMenuItem
    Friend WithEvents rmi_SwapRates As Telerik.WinControls.UI.RadMenuItem
    Friend WithEvents rmi_NukeUsers As Telerik.WinControls.UI.RadMenuItem
    Friend WithEvents rmi_HELP_MAIN As Telerik.WinControls.UI.CommandBarButton
    Friend WithEvents rms_BrokerNuke As Telerik.WinControls.UI.RadMenuSeparatorItem
    Friend WithEvents rmi_DayTrades As Telerik.WinControls.UI.RadMenuItem
    Friend WithEvents RadMenuSeparatorItem2 As Telerik.WinControls.UI.RadMenuSeparatorItem

End Class
