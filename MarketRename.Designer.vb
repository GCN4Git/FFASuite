<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MarketRename
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MarketRename))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rtb_FORM_NAME = New Telerik.WinControls.UI.RadTextBox()
        Me.rbtn_OK = New Telerik.WinControls.UI.RadButton()
        Me.rbtn_Cancel = New Telerik.WinControls.UI.RadButton()
        Me.Timer_CLose = New System.Windows.Forms.Timer(Me.components)
        CType(Me.rtb_FORM_NAME, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rtb_FORM_NAME
        '
        Me.rtb_FORM_NAME.Location = New System.Drawing.Point(2, 3)
        Me.rtb_FORM_NAME.Name = "rtb_FORM_NAME"
        Me.rtb_FORM_NAME.Size = New System.Drawing.Size(170, 20)
        Me.rtb_FORM_NAME.TabIndex = 0
        Me.rtb_FORM_NAME.TabStop = False
        '
        'rbtn_OK
        '
        Me.rbtn_OK.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_OK.Location = New System.Drawing.Point(17, 29)
        Me.rbtn_OK.Name = "rbtn_OK"
        Me.rbtn_OK.Size = New System.Drawing.Size(65, 24)
        Me.rbtn_OK.TabIndex = 1
        Me.rbtn_OK.Text = "OK"
        Me.rbtn_OK.ThemeName = "Office2010Silver"
        '
        'rbtn_Cancel
        '
        Me.rbtn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.rbtn_Cancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_Cancel.Location = New System.Drawing.Point(90, 29)
        Me.rbtn_Cancel.Name = "rbtn_Cancel"
        Me.rbtn_Cancel.Size = New System.Drawing.Size(65, 24)
        Me.rbtn_Cancel.TabIndex = 2
        Me.rbtn_Cancel.Text = "Cancel"
        Me.rbtn_Cancel.ThemeName = "Office2010Silver"
        '
        'Timer_CLose
        '
        Me.Timer_CLose.Enabled = True
        Me.Timer_CLose.Interval = 150000
        '
        'MarketRename
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(175, 57)
        Me.ControlBox = False
        Me.Controls.Add(Me.rbtn_Cancel)
        Me.Controls.Add(Me.rbtn_OK)
        Me.Controls.Add(Me.rtb_FORM_NAME)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MarketRename"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "Rename Form"
        Me.ThemeName = "Office2010Silver"
        CType(Me.rtb_FORM_NAME, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rtb_FORM_NAME As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents rbtn_OK As Telerik.WinControls.UI.RadButton
    Friend WithEvents rbtn_Cancel As Telerik.WinControls.UI.RadButton
    Friend WithEvents Timer_CLose As System.Windows.Forms.Timer
End Class

