<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BalticFTP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BalticFTP))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.cb_SETLEMENT_ONLY = New Telerik.WinControls.UI.RadRadioButton()
        Me.cb_INDICES = New Telerik.WinControls.UI.RadRadioButton()
        Me.cb_ALL = New Telerik.WinControls.UI.RadRadioButton()
        Me.rdtp_DATE = New Telerik.WinControls.UI.RadDateTimePicker()
        Me.rse_SDEV = New Telerik.WinControls.UI.RadSpinEditor()
        Me.rse_PERIOD = New Telerik.WinControls.UI.RadSpinEditor()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.rbtn_Prepare_SMS = New Telerik.WinControls.UI.RadButton()
        Me.rlb_DIRECTORY = New Telerik.WinControls.UI.RadListView()
        Me.bs_DIRECTORY = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.cb_SETLEMENT_ONLY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_INDICES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cb_ALL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rdtp_DATE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rse_SDEV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rse_PERIOD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_Prepare_SMS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rlb_DIRECTORY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bs_DIRECTORY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.cb_SETLEMENT_ONLY)
        Me.RadGroupBox1.Controls.Add(Me.cb_INDICES)
        Me.RadGroupBox1.Controls.Add(Me.cb_ALL)
        Me.RadGroupBox1.Controls.Add(Me.rdtp_DATE)
        Me.RadGroupBox1.Controls.Add(Me.rse_SDEV)
        Me.RadGroupBox1.Controls.Add(Me.rse_PERIOD)
        Me.RadGroupBox1.Controls.Add(Me.RadLabel1)
        Me.RadGroupBox1.Controls.Add(Me.RadLabel2)
        Me.RadGroupBox1.HeaderText = "Selections"
        Me.RadGroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
        '
        '
        '
        Me.RadGroupBox1.RootElement.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
        Me.RadGroupBox1.Size = New System.Drawing.Size(307, 108)
        Me.RadGroupBox1.TabIndex = 37
        Me.RadGroupBox1.Text = "Selections"
        Me.RadGroupBox1.ThemeName = "Office2010Silver"
        CType(Me.RadGroupBox1.GetChildAt(0), Telerik.WinControls.UI.RadGroupBoxElement).Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Text = "Selections"
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Alignment = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cb_SETLEMENT_ONLY
        '
        Me.cb_SETLEMENT_ONLY.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_SETLEMENT_ONLY.Location = New System.Drawing.Point(13, 74)
        Me.cb_SETLEMENT_ONLY.Name = "cb_SETLEMENT_ONLY"
        Me.cb_SETLEMENT_ONLY.Size = New System.Drawing.Size(111, 18)
        Me.cb_SETLEMENT_ONLY.TabIndex = 38
        Me.cb_SETLEMENT_ONLY.Text = "Settlements Only"
        '
        'cb_INDICES
        '
        Me.cb_INDICES.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_INDICES.Location = New System.Drawing.Point(13, 50)
        Me.cb_INDICES.Name = "cb_INDICES"
        Me.cb_INDICES.Size = New System.Drawing.Size(85, 18)
        Me.cb_INDICES.TabIndex = 37
        Me.cb_INDICES.Text = "Indices Only"
        '
        'cb_ALL
        '
        Me.cb_ALL.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cb_ALL.Location = New System.Drawing.Point(13, 26)
        Me.cb_ALL.Name = "cb_ALL"
        Me.cb_ALL.Size = New System.Drawing.Size(35, 18)
        Me.cb_ALL.TabIndex = 36
        Me.cb_ALL.TabStop = True
        Me.cb_ALL.Text = "All"
        Me.cb_ALL.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        '
        'rdtp_DATE
        '
        Me.rdtp_DATE.CustomFormat = "dddd MMM dd, yyyy"
        Me.rdtp_DATE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rdtp_DATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.rdtp_DATE.Location = New System.Drawing.Point(124, 23)
        Me.rdtp_DATE.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
        Me.rdtp_DATE.Name = "rdtp_DATE"
        Me.rdtp_DATE.NullDate = New Date(1900, 1, 1, 0, 0, 0, 0)
        Me.rdtp_DATE.Size = New System.Drawing.Size(170, 20)
        Me.rdtp_DATE.TabIndex = 26
        Me.rdtp_DATE.TabStop = False
        Me.rdtp_DATE.Text = "Tuesday Mar 23, 2010"
        Me.rdtp_DATE.ThemeName = "Office2010Silver"
        Me.rdtp_DATE.Value = New Date(2010, 3, 23, 15, 49, 15, 101)
        '
        'rse_SDEV
        '
        Me.rse_SDEV.DecimalPlaces = 1
        Me.rse_SDEV.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.rse_SDEV.Location = New System.Drawing.Point(233, 76)
        Me.rse_SDEV.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.rse_SDEV.Minimum = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.rse_SDEV.Name = "rse_SDEV"
        '
        '
        '
        Me.rse_SDEV.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.rse_SDEV.Size = New System.Drawing.Size(61, 20)
        Me.rse_SDEV.TabIndex = 29
        Me.rse_SDEV.TabStop = False
        Me.rse_SDEV.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.rse_SDEV.ThemeName = "Office2010Silver"
        Me.rse_SDEV.Value = New Decimal(New Integer() {2, 0, 0, 0})
        CType(Me.rse_SDEV.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.StackLayoutElement).Text = "2.0"
        CType(Me.rse_SDEV.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.StackLayoutElement).Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        '
        'rse_PERIOD
        '
        Me.rse_PERIOD.Location = New System.Drawing.Point(233, 50)
        Me.rse_PERIOD.Maximum = New Decimal(New Integer() {30, 0, 0, 0})
        Me.rse_PERIOD.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.rse_PERIOD.Name = "rse_PERIOD"
        '
        '
        '
        Me.rse_PERIOD.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.rse_PERIOD.Size = New System.Drawing.Size(61, 20)
        Me.rse_PERIOD.TabIndex = 31
        Me.rse_PERIOD.TabStop = False
        Me.rse_PERIOD.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.rse_PERIOD.ThemeName = "Office2010Silver"
        Me.rse_PERIOD.Value = New Decimal(New Integer() {7, 0, 0, 0})
        CType(Me.rse_PERIOD.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.StackLayoutElement).Text = "7"
        CType(Me.rse_PERIOD.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.StackLayoutElement).Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(185, 76)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(41, 16)
        Me.RadLabel1.TabIndex = 30
        Me.RadLabel1.Text = "StDev:"
        CType(Me.RadLabel1.GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Text = "StDev:"
        CType(Me.RadLabel1.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).TextAlignment = System.Drawing.ContentAlignment.TopLeft
        CType(Me.RadLabel1.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(185, 50)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(44, 16)
        Me.RadLabel2.TabIndex = 32
        Me.RadLabel2.Text = "Period:"
        CType(Me.RadLabel2.GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Text = "Period:"
        CType(Me.RadLabel2.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).TextAlignment = System.Drawing.ContentAlignment.TopLeft
        CType(Me.RadLabel2.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        '
        'rbtn_Prepare_SMS
        '
        Me.rbtn_Prepare_SMS.Location = New System.Drawing.Point(106, 441)
        Me.rbtn_Prepare_SMS.Name = "rbtn_Prepare_SMS"
        Me.rbtn_Prepare_SMS.Size = New System.Drawing.Size(110, 24)
        Me.rbtn_Prepare_SMS.TabIndex = 36
        Me.rbtn_Prepare_SMS.Text = "FTP Data"
        Me.rbtn_Prepare_SMS.ThemeName = "Office2010Silver"
        CType(Me.rbtn_Prepare_SMS.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "FTP Data"
        CType(Me.rbtn_Prepare_SMS.GetChildAt(0).GetChildAt(1).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CType(Me.rbtn_Prepare_SMS.GetChildAt(0).GetChildAt(1).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Alignment = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rlb_DIRECTORY
        '
        Me.rlb_DIRECTORY.Location = New System.Drawing.Point(12, 126)
        Me.rlb_DIRECTORY.Name = "rlb_DIRECTORY"
        Me.rlb_DIRECTORY.Size = New System.Drawing.Size(307, 307)
        Me.rlb_DIRECTORY.TabIndex = 38
        Me.rlb_DIRECTORY.Text = "RadListView1"
        Me.rlb_DIRECTORY.ThemeName = "Office2010Silver"
        '
        'BalticFTP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(328, 474)
        Me.Controls.Add(Me.rlb_DIRECTORY)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.rbtn_Prepare_SMS)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BalticFTP"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "Baltic FTP"
        Me.ThemeName = "Office2010Silver"
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.cb_SETLEMENT_ONLY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_INDICES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cb_ALL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rdtp_DATE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rse_SDEV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rse_PERIOD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_Prepare_SMS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rlb_DIRECTORY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bs_DIRECTORY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents rdtp_DATE As Telerik.WinControls.UI.RadDateTimePicker
    Friend WithEvents rse_SDEV As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents rse_PERIOD As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents rbtn_Prepare_SMS As Telerik.WinControls.UI.RadButton
    Friend WithEvents rlb_DIRECTORY As Telerik.WinControls.UI.RadListView
    Friend WithEvents bs_DIRECTORY As System.Windows.Forms.BindingSource
    Friend WithEvents cb_SETLEMENT_ONLY As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents cb_INDICES As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents cb_ALL As Telerik.WinControls.UI.RadRadioButton
End Class

