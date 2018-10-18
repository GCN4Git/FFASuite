<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DayTrades
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DayTrades))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rad_DATEPICKER = New Telerik.WinControls.UI.RadDateTimePicker()
        Me.rbtn_GetTrades = New Telerik.WinControls.UI.RadButton()
        Me.rtb_Trades = New Telerik.WinControls.UI.RadTextBox()
        Me.RadButton1 = New Telerik.WinControls.UI.RadButton()
        CType(Me.rad_DATEPICKER, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_GetTrades, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rtb_Trades, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rad_DATEPICKER
        '
        Me.rad_DATEPICKER.CustomFormat = "dddd MMM dd, yyyy"
        Me.rad_DATEPICKER.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rad_DATEPICKER.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.rad_DATEPICKER.Location = New System.Drawing.Point(4, 12)
        Me.rad_DATEPICKER.Name = "rad_DATEPICKER"
        Me.rad_DATEPICKER.Size = New System.Drawing.Size(164, 20)
        Me.rad_DATEPICKER.TabIndex = 9
        Me.rad_DATEPICKER.TabStop = False
        Me.rad_DATEPICKER.Text = "Monday Feb 18, 2013"
        Me.rad_DATEPICKER.ThemeName = "Office2010Silver"
        Me.rad_DATEPICKER.Value = New Date(2013, 2, 18, 8, 20, 36, 582)
        '
        'rbtn_GetTrades
        '
        Me.rbtn_GetTrades.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_GetTrades.Location = New System.Drawing.Point(174, 11)
        Me.rbtn_GetTrades.Name = "rbtn_GetTrades"
        Me.rbtn_GetTrades.Size = New System.Drawing.Size(91, 24)
        Me.rbtn_GetTrades.TabIndex = 8
        Me.rbtn_GetTrades.Text = "Get Trades"
        Me.rbtn_GetTrades.ThemeName = "Office2010Silver"
        '
        'rtb_Trades
        '
        Me.rtb_Trades.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtb_Trades.AutoSize = False
        Me.rtb_Trades.Location = New System.Drawing.Point(6, 44)
        Me.rtb_Trades.Multiline = True
        Me.rtb_Trades.Name = "rtb_Trades"
        Me.rtb_Trades.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.rtb_Trades.Size = New System.Drawing.Size(380, 466)
        Me.rtb_Trades.TabIndex = 10
        Me.rtb_Trades.TabStop = False
        '
        'RadButton1
        '
        Me.RadButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadButton1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.RadButton1.Location = New System.Drawing.Point(343, 11)
        Me.RadButton1.Name = "RadButton1"
        Me.RadButton1.Size = New System.Drawing.Size(43, 24)
        Me.RadButton1.TabIndex = 11
        Me.RadButton1.Text = "Clear"
        Me.RadButton1.ThemeName = "Office2010Silver"
        '
        'DayTrades
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 516)
        Me.Controls.Add(Me.RadButton1)
        Me.Controls.Add(Me.rtb_Trades)
        Me.Controls.Add(Me.rad_DATEPICKER)
        Me.Controls.Add(Me.rbtn_GetTrades)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(325, 250)
        Me.Name = "DayTrades"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "Day Trades"
        Me.ThemeName = "Office2010Silver"
        CType(Me.rad_DATEPICKER, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_GetTrades, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rtb_Trades, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rad_DATEPICKER As Telerik.WinControls.UI.RadDateTimePicker
    Friend WithEvents rbtn_GetTrades As Telerik.WinControls.UI.RadButton
    Friend WithEvents rtb_Trades As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadButton1 As Telerik.WinControls.UI.RadButton
End Class

