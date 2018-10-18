<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddTradeForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddTradeForm))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.se_PRICE = New Telerik.WinControls.UI.RadSpinEditor()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.rgb_CONTRACT = New Telerik.WinControls.UI.RadGroupBox()
        Me.rbtn_SUBMIT = New Telerik.WinControls.UI.RadButton()
        Me.rrb_SHADOW = New Telerik.WinControls.UI.RadRadioButton()
        Me.rrb_LIVE = New Telerik.WinControls.UI.RadRadioButton()
        CType(Me.se_PRICE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgb_CONTRACT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rgb_CONTRACT.SuspendLayout()
        CType(Me.rbtn_SUBMIT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rrb_SHADOW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rrb_LIVE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'se_PRICE
        '
        Me.se_PRICE.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.se_PRICE.Increment = New Decimal(New Integer() {250, 0, 0, 0})
        Me.se_PRICE.Location = New System.Drawing.Point(11, 44)
        Me.se_PRICE.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.se_PRICE.Name = "se_PRICE"
        Me.se_PRICE.Size = New System.Drawing.Size(81, 23)
        Me.se_PRICE.TabIndex = 131
        Me.se_PRICE.TabStop = False
        Me.se_PRICE.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.se_PRICE.ThemeName = "Office2010Silver"
        Me.se_PRICE.ThousandsSeparator = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 13)
        Me.Label3.TabIndex = 130
        Me.Label3.Text = "Trade Price"
        '
        'rgb_CONTRACT
        '
        Me.rgb_CONTRACT.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.rgb_CONTRACT.Controls.Add(Me.rbtn_SUBMIT)
        Me.rgb_CONTRACT.Controls.Add(Me.rrb_SHADOW)
        Me.rgb_CONTRACT.Controls.Add(Me.rrb_LIVE)
        Me.rgb_CONTRACT.Controls.Add(Me.se_PRICE)
        Me.rgb_CONTRACT.Controls.Add(Me.Label3)
        Me.rgb_CONTRACT.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rgb_CONTRACT.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rgb_CONTRACT.HeaderText = "C4TC: Q2"
        Me.rgb_CONTRACT.Location = New System.Drawing.Point(0, 0)
        Me.rgb_CONTRACT.Name = "rgb_CONTRACT"
        '
        '
        '
        Me.rgb_CONTRACT.RootElement.Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        Me.rgb_CONTRACT.Size = New System.Drawing.Size(189, 132)
        Me.rgb_CONTRACT.TabIndex = 132
        Me.rgb_CONTRACT.Text = "C4TC: Q2"
        Me.rgb_CONTRACT.ThemeName = "Office2010Silver"
        '
        'rbtn_SUBMIT
        '
        Me.rbtn_SUBMIT.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_SUBMIT.Location = New System.Drawing.Point(11, 83)
        Me.rbtn_SUBMIT.Name = "rbtn_SUBMIT"
        Me.rbtn_SUBMIT.Size = New System.Drawing.Size(165, 43)
        Me.rbtn_SUBMIT.TabIndex = 133
        Me.rbtn_SUBMIT.Text = "Submit"
        Me.rbtn_SUBMIT.ThemeName = "Office2010Silver"
        '
        'rrb_SHADOW
        '
        Me.rrb_SHADOW.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rrb_SHADOW.Location = New System.Drawing.Point(101, 59)
        Me.rrb_SHADOW.Name = "rrb_SHADOW"
        Me.rrb_SHADOW.Size = New System.Drawing.Size(48, 18)
        Me.rrb_SHADOW.TabIndex = 133
        Me.rrb_SHADOW.Text = "Level"
        '
        'rrb_LIVE
        '
        Me.rrb_LIVE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rrb_LIVE.Location = New System.Drawing.Point(101, 35)
        Me.rrb_LIVE.Name = "rrb_LIVE"
        Me.rrb_LIVE.Size = New System.Drawing.Size(75, 18)
        Me.rrb_LIVE.TabIndex = 132
        Me.rrb_LIVE.TabStop = True
        Me.rrb_LIVE.Text = "Live Trade"
        Me.rrb_LIVE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        '
        'AddTradeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(189, 132)
        Me.Controls.Add(Me.rgb_CONTRACT)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddTradeForm"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Live Trades"
        Me.ThemeName = "Office2010Silver"
        Me.TopMost = True
        CType(Me.se_PRICE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgb_CONTRACT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rgb_CONTRACT.ResumeLayout(False)
        Me.rgb_CONTRACT.PerformLayout()
        CType(Me.rbtn_SUBMIT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rrb_SHADOW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rrb_LIVE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents se_PRICE As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rgb_CONTRACT As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents rrb_SHADOW As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents rrb_LIVE As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents rbtn_SUBMIT As Telerik.WinControls.UI.RadButton
End Class

