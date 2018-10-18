<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdatesForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UpdatesForm))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rbtn_SGX = New Telerik.WinControls.UI.RadButton()
        Me.rbtn_Baltic = New Telerik.WinControls.UI.RadButton()
        Me.rbtn_SMS = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.radio_EXTERNAL = New Telerik.WinControls.UI.RadRadioButton()
        Me.radio_BRS = New Telerik.WinControls.UI.RadRadioButton()
        Me.rcb_UPDATE_BRS = New Telerik.WinControls.UI.RadCheckBox()
        Me.rbtn_TestWS = New Telerik.WinControls.UI.RadButton()
        CType(Me.rbtn_SGX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_Baltic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_SMS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.radio_EXTERNAL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.radio_BRS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rcb_UPDATE_BRS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_TestWS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rbtn_SGX
        '
        Me.rbtn_SGX.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_SGX.Location = New System.Drawing.Point(124, 53)
        Me.rbtn_SGX.Name = "rbtn_SGX"
        Me.rbtn_SGX.Size = New System.Drawing.Size(110, 35)
        Me.rbtn_SGX.TabIndex = 0
        Me.rbtn_SGX.Text = "SGX Iron Ore"
        Me.rbtn_SGX.ThemeName = "Office2010Silver"
        '
        'rbtn_Baltic
        '
        Me.rbtn_Baltic.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_Baltic.Location = New System.Drawing.Point(124, 12)
        Me.rbtn_Baltic.Name = "rbtn_Baltic"
        Me.rbtn_Baltic.Size = New System.Drawing.Size(110, 35)
        Me.rbtn_Baltic.TabIndex = 1
        Me.rbtn_Baltic.Text = "Baltic Indices"
        Me.rbtn_Baltic.ThemeName = "Office2010Silver"
        '
        'rbtn_SMS
        '
        Me.rbtn_SMS.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_SMS.Location = New System.Drawing.Point(124, 94)
        Me.rbtn_SMS.Name = "rbtn_SMS"
        Me.rbtn_SMS.Size = New System.Drawing.Size(110, 35)
        Me.rbtn_SMS.TabIndex = 2
        Me.rbtn_SMS.Text = "SMS to Clients"
        Me.rbtn_SMS.ThemeName = "Office2010Silver"
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.radio_EXTERNAL)
        Me.RadGroupBox1.Controls.Add(Me.radio_BRS)
        Me.RadGroupBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.RadGroupBox1.HeaderText = "Data Source"
        Me.RadGroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        '
        '
        '
        Me.RadGroupBox1.RootElement.Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        Me.RadGroupBox1.Size = New System.Drawing.Size(96, 72)
        Me.RadGroupBox1.TabIndex = 27
        Me.RadGroupBox1.Text = "Data Source"
        Me.RadGroupBox1.ThemeName = "Office2010Silver"
        '
        'radio_EXTERNAL
        '
        Me.radio_EXTERNAL.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.radio_EXTERNAL.Location = New System.Drawing.Point(6, 46)
        Me.radio_EXTERNAL.Name = "radio_EXTERNAL"
        Me.radio_EXTERNAL.Size = New System.Drawing.Size(64, 18)
        Me.radio_EXTERNAL.TabIndex = 1
        Me.radio_EXTERNAL.Text = "External"
        '
        'radio_BRS
        '
        Me.radio_BRS.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.radio_BRS.Location = New System.Drawing.Point(6, 22)
        Me.radio_BRS.Name = "radio_BRS"
        Me.radio_BRS.Size = New System.Drawing.Size(41, 18)
        Me.radio_BRS.TabIndex = 0
        Me.radio_BRS.Text = "BRS"
        '
        'rcb_UPDATE_BRS
        '
        Me.rcb_UPDATE_BRS.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rcb_UPDATE_BRS.Location = New System.Drawing.Point(12, 102)
        Me.rcb_UPDATE_BRS.Name = "rcb_UPDATE_BRS"
        Me.rcb_UPDATE_BRS.Size = New System.Drawing.Size(100, 18)
        Me.rcb_UPDATE_BRS.TabIndex = 28
        Me.rcb_UPDATE_BRS.Text = "Update BRS db"
        Me.rcb_UPDATE_BRS.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        '
        'rbtn_TestWS
        '
        Me.rbtn_TestWS.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_TestWS.Location = New System.Drawing.Point(12, 128)
        Me.rbtn_TestWS.Name = "rbtn_TestWS"
        Me.rbtn_TestWS.Size = New System.Drawing.Size(96, 21)
        Me.rbtn_TestWS.TabIndex = 29
        Me.rbtn_TestWS.Text = "Test WS Service"
        Me.rbtn_TestWS.ThemeName = "Office2010Silver"
        '
        'UpdatesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(249, 155)
        Me.Controls.Add(Me.rbtn_TestWS)
        Me.Controls.Add(Me.rcb_UPDATE_BRS)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.rbtn_SMS)
        Me.Controls.Add(Me.rbtn_Baltic)
        Me.Controls.Add(Me.rbtn_SGX)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "UpdatesForm"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "FFA Suite Updater"
        Me.ThemeName = "Office2010Silver"
        CType(Me.rbtn_SGX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_Baltic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_SMS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.radio_EXTERNAL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.radio_BRS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rcb_UPDATE_BRS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_TestWS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rbtn_SGX As Telerik.WinControls.UI.RadButton
    Friend WithEvents rbtn_Baltic As Telerik.WinControls.UI.RadButton
    Friend WithEvents rbtn_SMS As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents radio_EXTERNAL As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents radio_BRS As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents rcb_UPDATE_BRS As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents rbtn_TestWS As Telerik.WinControls.UI.RadButton
End Class

