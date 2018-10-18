<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SMSMainForm
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
        Dim GridViewDecimalColumn1 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDecimalColumn2 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SMSMainForm))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rdtp_DATE = New Telerik.WinControls.UI.RadDateTimePicker()
        Me.rtb_SMS_CAPES = New Telerik.WinControls.UI.RadTextBox()
        Me.rtb_SMS_PMX = New Telerik.WinControls.UI.RadTextBox()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.rbtn_PREPARE_CAPES = New Telerik.WinControls.UI.RadButton()
        Me.rbtn_PREPARE_PMX = New Telerik.WinControls.UI.RadButton()
        Me.rbtn_SMSCLIENTS = New Telerik.WinControls.UI.RadButton()
        Me.rgv_SMSPROVIDERS = New Telerik.WinControls.UI.RadGridView()
        Me.rtb_SMSBALANCE = New Telerik.WinControls.UI.RadTextBoxControl()
        Me.rbtn_SEND_CAPES = New Telerik.WinControls.UI.RadButton()
        Me.rbtn_SEND_PMX = New Telerik.WinControls.UI.RadButton()
        Me.rlv_SMSActivity = New Telerik.WinControls.UI.RadListView()
        Me.BS_SMSActivity = New System.Windows.Forms.BindingSource(Me.components)
        Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
        Me.pb_SMS = New System.Windows.Forms.ProgressBar()
        Me.rbtn_SMS_SETTINGS = New Telerik.WinControls.UI.RadButton()
        Me.rtb_TEST = New Telerik.WinControls.UI.RadTextBox()
        Me.rbtn_TEST = New Telerik.WinControls.UI.RadButton()
        CType(Me.rdtp_DATE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rtb_SMS_CAPES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rtb_SMS_PMX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_PREPARE_CAPES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_PREPARE_PMX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_SMSCLIENTS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_SMSPROVIDERS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_SMSPROVIDERS.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rtb_SMSBALANCE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_SEND_CAPES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_SEND_PMX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rlv_SMSActivity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BS_SMSActivity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_SMS_SETTINGS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rtb_TEST, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_TEST, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rdtp_DATE
        '
        Me.rdtp_DATE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rdtp_DATE.Location = New System.Drawing.Point(492, 72)
        Me.rdtp_DATE.Name = "rdtp_DATE"
        Me.rdtp_DATE.Size = New System.Drawing.Size(133, 20)
        Me.rdtp_DATE.TabIndex = 0
        Me.rdtp_DATE.TabStop = False
        Me.rdtp_DATE.Text = "12 April 2013"
        Me.rdtp_DATE.ThemeName = "Office2010Silver"
        Me.rdtp_DATE.Value = New Date(2013, 4, 12, 18, 3, 54, 812)
        '
        'rtb_SMS_CAPES
        '
        Me.rtb_SMS_CAPES.AutoSize = False
        Me.rtb_SMS_CAPES.Location = New System.Drawing.Point(12, 146)
        Me.rtb_SMS_CAPES.Multiline = True
        Me.rtb_SMS_CAPES.Name = "rtb_SMS_CAPES"
        Me.rtb_SMS_CAPES.Size = New System.Drawing.Size(146, 282)
        Me.rtb_SMS_CAPES.TabIndex = 20
        Me.rtb_SMS_CAPES.TabStop = False
        Me.rtb_SMS_CAPES.ThemeName = "Office2010Silver"
        '
        'rtb_SMS_PMX
        '
        Me.rtb_SMS_PMX.AutoSize = False
        Me.rtb_SMS_PMX.Location = New System.Drawing.Point(164, 146)
        Me.rtb_SMS_PMX.Multiline = True
        Me.rtb_SMS_PMX.Name = "rtb_SMS_PMX"
        Me.rtb_SMS_PMX.Size = New System.Drawing.Size(146, 282)
        Me.rtb_SMS_PMX.TabIndex = 21
        Me.rtb_SMS_PMX.TabStop = False
        Me.rtb_SMS_PMX.ThemeName = "Office2010Silver"
        '
        'RadLabel1
        '
        Me.RadLabel1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.RadLabel1.Location = New System.Drawing.Point(12, 122)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(64, 18)
        Me.RadLabel1.TabIndex = 22
        Me.RadLabel1.Text = "Capes SMS"
        Me.RadLabel1.ThemeName = "Office2010Silver"
        '
        'RadLabel2
        '
        Me.RadLabel2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.RadLabel2.Location = New System.Drawing.Point(164, 122)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(58, 18)
        Me.RadLabel2.TabIndex = 23
        Me.RadLabel2.Text = "PMX SMS"
        Me.RadLabel2.ThemeName = "Office2010Silver"
        '
        'rbtn_PREPARE_CAPES
        '
        Me.rbtn_PREPARE_CAPES.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_PREPARE_CAPES.Location = New System.Drawing.Point(12, 434)
        Me.rbtn_PREPARE_CAPES.Name = "rbtn_PREPARE_CAPES"
        Me.rbtn_PREPARE_CAPES.Size = New System.Drawing.Size(146, 24)
        Me.rbtn_PREPARE_CAPES.TabIndex = 24
        Me.rbtn_PREPARE_CAPES.Text = "Refresh Capes SMS"
        Me.rbtn_PREPARE_CAPES.ThemeName = "Office2010Silver"
        '
        'rbtn_PREPARE_PMX
        '
        Me.rbtn_PREPARE_PMX.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_PREPARE_PMX.Location = New System.Drawing.Point(164, 434)
        Me.rbtn_PREPARE_PMX.Name = "rbtn_PREPARE_PMX"
        Me.rbtn_PREPARE_PMX.Size = New System.Drawing.Size(146, 24)
        Me.rbtn_PREPARE_PMX.TabIndex = 25
        Me.rbtn_PREPARE_PMX.Text = "Refresh Pmx SMS"
        Me.rbtn_PREPARE_PMX.ThemeName = "Office2010Silver"
        '
        'rbtn_SMSCLIENTS
        '
        Me.rbtn_SMSCLIENTS.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_SMSCLIENTS.Location = New System.Drawing.Point(492, 12)
        Me.rbtn_SMSCLIENTS.Name = "rbtn_SMSCLIENTS"
        Me.rbtn_SMSCLIENTS.Size = New System.Drawing.Size(133, 54)
        Me.rbtn_SMSCLIENTS.TabIndex = 27
        Me.rbtn_SMSCLIENTS.Text = "Clients DB"
        Me.rbtn_SMSCLIENTS.ThemeName = "Office2010Silver"
        '
        'rgv_SMSPROVIDERS
        '
        Me.rgv_SMSPROVIDERS.Location = New System.Drawing.Point(12, 12)
        '
        'rgv_SMSPROVIDERS
        '
        Me.rgv_SMSPROVIDERS.MasterTemplate.AllowAddNewRow = False
        Me.rgv_SMSPROVIDERS.MasterTemplate.AllowCellContextMenu = False
        Me.rgv_SMSPROVIDERS.MasterTemplate.AllowColumnHeaderContextMenu = False
        Me.rgv_SMSPROVIDERS.MasterTemplate.AllowColumnReorder = False
        Me.rgv_SMSPROVIDERS.MasterTemplate.AllowColumnResize = False
        Me.rgv_SMSPROVIDERS.MasterTemplate.AllowDragToGroup = False
        GridViewDecimalColumn1.FieldName = "ID"
        GridViewDecimalColumn1.HeaderText = "ID"
        GridViewDecimalColumn1.IsVisible = False
        GridViewDecimalColumn1.Name = "ID"
        GridViewTextBoxColumn1.FieldName = "SMSPROVIDER"
        GridViewTextBoxColumn1.HeaderText = "SMS PROVIDER"
        GridViewTextBoxColumn1.Name = "SMSPROVIDER"
        GridViewTextBoxColumn1.Width = 95
        GridViewDecimalColumn2.DecimalPlaces = 0
        GridViewDecimalColumn2.FieldName = "SMSBALANCE"
        GridViewDecimalColumn2.HeaderText = "BALANCE"
        GridViewDecimalColumn2.Name = "SMSBALANCE"
        GridViewDecimalColumn2.ReadOnly = True
        GridViewDecimalColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn2.ThousandsSeparator = True
        GridViewDecimalColumn2.Width = 65
        Me.rgv_SMSPROVIDERS.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn1, GridViewTextBoxColumn1, GridViewDecimalColumn2})
        Me.rgv_SMSPROVIDERS.MasterTemplate.EnableGrouping = False
        Me.rgv_SMSPROVIDERS.MasterTemplate.EnableSorting = False
        Me.rgv_SMSPROVIDERS.MasterTemplate.HorizontalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysHide
        Me.rgv_SMSPROVIDERS.MasterTemplate.ShowRowHeaderColumn = False
        Me.rgv_SMSPROVIDERS.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysHide
        Me.rgv_SMSPROVIDERS.Name = "rgv_SMSPROVIDERS"
        Me.rgv_SMSPROVIDERS.ReadOnly = True
        Me.rgv_SMSPROVIDERS.ShowGroupPanel = False
        Me.rgv_SMSPROVIDERS.Size = New System.Drawing.Size(159, 77)
        Me.rgv_SMSPROVIDERS.TabIndex = 28
        Me.rgv_SMSPROVIDERS.Text = "RadGridView1"
        Me.rgv_SMSPROVIDERS.ThemeName = "Office2010Silver"
        '
        'rtb_SMSBALANCE
        '
        Me.rtb_SMSBALANCE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rtb_SMSBALANCE.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rtb_SMSBALANCE.IsReadOnly = True
        Me.rtb_SMSBALANCE.Location = New System.Drawing.Point(12, 91)
        Me.rtb_SMSBALANCE.Name = "rtb_SMSBALANCE"
        Me.rtb_SMSBALANCE.Size = New System.Drawing.Size(159, 20)
        Me.rtb_SMSBALANCE.TabIndex = 29
        Me.rtb_SMSBALANCE.ThemeName = "Office2010Silver"
        '
        'rbtn_SEND_CAPES
        '
        Me.rbtn_SEND_CAPES.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_SEND_CAPES.Location = New System.Drawing.Point(59, 464)
        Me.rbtn_SEND_CAPES.Name = "rbtn_SEND_CAPES"
        Me.rbtn_SEND_CAPES.Size = New System.Drawing.Size(53, 24)
        Me.rbtn_SEND_CAPES.TabIndex = 30
        Me.rbtn_SEND_CAPES.Text = "Send"
        Me.rbtn_SEND_CAPES.ThemeName = "Office2010Silver"
        '
        'rbtn_SEND_PMX
        '
        Me.rbtn_SEND_PMX.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_SEND_PMX.Location = New System.Drawing.Point(209, 464)
        Me.rbtn_SEND_PMX.Name = "rbtn_SEND_PMX"
        Me.rbtn_SEND_PMX.Size = New System.Drawing.Size(53, 24)
        Me.rbtn_SEND_PMX.TabIndex = 31
        Me.rbtn_SEND_PMX.Text = "Send"
        Me.rbtn_SEND_PMX.ThemeName = "Office2010Silver"
        '
        'rlv_SMSActivity
        '
        Me.rlv_SMSActivity.AllowEdit = False
        Me.rlv_SMSActivity.AllowRemove = False
        Me.rlv_SMSActivity.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rlv_SMSActivity.Location = New System.Drawing.Point(317, 146)
        Me.rlv_SMSActivity.MultiSelect = True
        Me.rlv_SMSActivity.Name = "rlv_SMSActivity"
        Me.rlv_SMSActivity.ShowColumnHeaders = False
        Me.rlv_SMSActivity.Size = New System.Drawing.Size(308, 312)
        Me.rlv_SMSActivity.TabIndex = 32
        Me.rlv_SMSActivity.ThemeName = "Office2010Silver"
        Me.rlv_SMSActivity.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow
        '
        'RadLabel3
        '
        Me.RadLabel3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.RadLabel3.Location = New System.Drawing.Point(317, 122)
        Me.RadLabel3.Name = "RadLabel3"
        Me.RadLabel3.Size = New System.Drawing.Size(145, 18)
        Me.RadLabel3.TabIndex = 33
        Me.RadLabel3.Text = "SMS Provider Activity Log"
        Me.RadLabel3.ThemeName = "Office2010Silver"
        '
        'pb_SMS
        '
        Me.pb_SMS.BackColor = System.Drawing.Color.White
        Me.pb_SMS.ForeColor = System.Drawing.Color.DarkGreen
        Me.pb_SMS.Location = New System.Drawing.Point(317, 463)
        Me.pb_SMS.Name = "pb_SMS"
        Me.pb_SMS.Size = New System.Drawing.Size(305, 23)
        Me.pb_SMS.Step = 1
        Me.pb_SMS.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pb_SMS.TabIndex = 34
        '
        'rbtn_SMS_SETTINGS
        '
        Me.rbtn_SMS_SETTINGS.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_SMS_SETTINGS.Location = New System.Drawing.Point(177, 12)
        Me.rbtn_SMS_SETTINGS.Name = "rbtn_SMS_SETTINGS"
        Me.rbtn_SMS_SETTINGS.Size = New System.Drawing.Size(85, 30)
        Me.rbtn_SMS_SETTINGS.TabIndex = 35
        Me.rbtn_SMS_SETTINGS.Text = "SMS Settings"
        Me.rbtn_SMS_SETTINGS.ThemeName = "Office2010Silver"
        '
        'rtb_TEST
        '
        Me.rtb_TEST.Location = New System.Drawing.Point(13, 504)
        Me.rtb_TEST.Name = "rtb_TEST"
        Me.rtb_TEST.Size = New System.Drawing.Size(112, 20)
        Me.rtb_TEST.TabIndex = 36
        '
        'rbtn_TEST
        '
        Me.rbtn_TEST.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_TEST.Location = New System.Drawing.Point(131, 503)
        Me.rbtn_TEST.Name = "rbtn_TEST"
        Me.rbtn_TEST.Size = New System.Drawing.Size(105, 24)
        Me.rbtn_TEST.TabIndex = 37
        Me.rbtn_TEST.Text = "Send Test SMS"
        Me.rbtn_TEST.ThemeName = "Office2010Silver"
        '
        'SMSMainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 534)
        Me.Controls.Add(Me.rbtn_TEST)
        Me.Controls.Add(Me.rtb_TEST)
        Me.Controls.Add(Me.rbtn_SMS_SETTINGS)
        Me.Controls.Add(Me.pb_SMS)
        Me.Controls.Add(Me.RadLabel3)
        Me.Controls.Add(Me.rlv_SMSActivity)
        Me.Controls.Add(Me.rbtn_SEND_PMX)
        Me.Controls.Add(Me.rbtn_SEND_CAPES)
        Me.Controls.Add(Me.rtb_SMSBALANCE)
        Me.Controls.Add(Me.rgv_SMSPROVIDERS)
        Me.Controls.Add(Me.rbtn_SMSCLIENTS)
        Me.Controls.Add(Me.rbtn_PREPARE_PMX)
        Me.Controls.Add(Me.rbtn_PREPARE_CAPES)
        Me.Controls.Add(Me.RadLabel2)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.rtb_SMS_PMX)
        Me.Controls.Add(Me.rtb_SMS_CAPES)
        Me.Controls.Add(Me.rdtp_DATE)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SMSMainForm"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "SMS Form"
        Me.ThemeName = "Office2010Silver"
        CType(Me.rdtp_DATE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rtb_SMS_CAPES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rtb_SMS_PMX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_PREPARE_CAPES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_PREPARE_PMX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_SMSCLIENTS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_SMSPROVIDERS.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_SMSPROVIDERS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rtb_SMSBALANCE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_SEND_CAPES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_SEND_PMX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rlv_SMSActivity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BS_SMSActivity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_SMS_SETTINGS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rtb_TEST, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_TEST, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rdtp_DATE As Telerik.WinControls.UI.RadDateTimePicker
    Friend WithEvents rtb_SMS_CAPES As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents rtb_SMS_PMX As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents rbtn_PREPARE_CAPES As Telerik.WinControls.UI.RadButton
    Friend WithEvents rbtn_PREPARE_PMX As Telerik.WinControls.UI.RadButton
    Friend WithEvents rbtn_SMSCLIENTS As Telerik.WinControls.UI.RadButton
    Friend WithEvents rgv_SMSPROVIDERS As Telerik.WinControls.UI.RadGridView
    Friend WithEvents rtb_SMSBALANCE As Telerik.WinControls.UI.RadTextBoxControl
    Friend WithEvents rbtn_SEND_CAPES As Telerik.WinControls.UI.RadButton
    Friend WithEvents rbtn_SEND_PMX As Telerik.WinControls.UI.RadButton
    Friend WithEvents rlv_SMSActivity As Telerik.WinControls.UI.RadListView
    Friend WithEvents BS_SMSActivity As System.Windows.Forms.BindingSource
    Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents pb_SMS As System.Windows.Forms.ProgressBar
    Friend WithEvents rbtn_SMS_SETTINGS As Telerik.WinControls.UI.RadButton
    Friend WithEvents rtb_TEST As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents rbtn_TEST As Telerik.WinControls.UI.RadButton
End Class

