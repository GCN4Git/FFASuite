<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ArtBNSGXServiceUpdate
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
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDecimalColumn2 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn3 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn4 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ArtBNSGXServiceUpdate))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.BALTIC_FTPBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.SPOT = New Telerik.WinControls.UI.RadSpinEditor()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.RadButton1 = New Telerik.WinControls.UI.RadButton()
        Me.RadButton2 = New Telerik.WinControls.UI.RadButton()
        Me.tb_PASTE = New System.Windows.Forms.TextBox()
        Me.rad_DATEPICKER = New Telerik.WinControls.UI.RadDateTimePicker()
        Me.rgv_DATA = New Telerik.WinControls.UI.RadGridView()
        CType(Me.BALTIC_FTPBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SPOT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rad_DATEPICKER, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_DATA, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_DATA.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SPOT
        '
        Me.SPOT.DecimalPlaces = 2
        Me.SPOT.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.SPOT.Location = New System.Drawing.Point(240, 5)
        Me.SPOT.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.SPOT.Name = "SPOT"
        Me.SPOT.Size = New System.Drawing.Size(78, 20)
        Me.SPOT.TabIndex = 2
        Me.SPOT.TabStop = False
        Me.SPOT.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.SPOT.ThemeName = "Office2010Silver"
        Me.SPOT.ThousandsSeparator = True
        '
        'RadLabel1
        '
        Me.RadLabel1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.RadLabel1.Location = New System.Drawing.Point(200, 7)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(34, 18)
        Me.RadLabel1.TabIndex = 3
        Me.RadLabel1.Text = "Spot:"
        '
        'RadButton1
        '
        Me.RadButton1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.RadButton1.Location = New System.Drawing.Point(3, 574)
        Me.RadButton1.Name = "RadButton1"
        Me.RadButton1.Size = New System.Drawing.Size(91, 24)
        Me.RadButton1.TabIndex = 4
        Me.RadButton1.Text = "Paste Data"
        Me.RadButton1.ThemeName = "Office2010Silver"
        '
        'RadButton2
        '
        Me.RadButton2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.RadButton2.Location = New System.Drawing.Point(228, 574)
        Me.RadButton2.Name = "RadButton2"
        Me.RadButton2.Size = New System.Drawing.Size(91, 24)
        Me.RadButton2.TabIndex = 5
        Me.RadButton2.Text = "Save Data"
        Me.RadButton2.ThemeName = "Office2010Silver"
        '
        'tb_PASTE
        '
        Me.tb_PASTE.Location = New System.Drawing.Point(100, 576)
        Me.tb_PASTE.Multiline = True
        Me.tb_PASTE.Name = "tb_PASTE"
        Me.tb_PASTE.Size = New System.Drawing.Size(100, 20)
        Me.tb_PASTE.TabIndex = 6
        Me.tb_PASTE.Visible = False
        '
        'rad_DATEPICKER
        '
        Me.rad_DATEPICKER.CustomFormat = "dddd MMM dd, yyyy"
        Me.rad_DATEPICKER.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rad_DATEPICKER.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.rad_DATEPICKER.Location = New System.Drawing.Point(3, 5)
        Me.rad_DATEPICKER.Name = "rad_DATEPICKER"
        Me.rad_DATEPICKER.Size = New System.Drawing.Size(164, 20)
        Me.rad_DATEPICKER.TabIndex = 7
        Me.rad_DATEPICKER.TabStop = False
        Me.rad_DATEPICKER.Text = "Monday Feb 18, 2013"
        Me.rad_DATEPICKER.ThemeName = "Office2010Silver"
        Me.rad_DATEPICKER.Value = New Date(2013, 2, 18, 8, 20, 36, 582)
        '
        'rgv_DATA
        '
        Me.rgv_DATA.Location = New System.Drawing.Point(3, 35)
        '
        'rgv_DATA
        '
        Me.rgv_DATA.MasterTemplate.AllowAddNewRow = False
        Me.rgv_DATA.MasterTemplate.AllowCellContextMenu = False
        Me.rgv_DATA.MasterTemplate.AllowColumnHeaderContextMenu = False
        Me.rgv_DATA.MasterTemplate.AllowColumnReorder = False
        Me.rgv_DATA.MasterTemplate.AllowColumnResize = False
        Me.rgv_DATA.MasterTemplate.AllowDragToGroup = False
        GridViewDecimalColumn1.FieldName = "ID"
        GridViewDecimalColumn1.HeaderText = "ID"
        GridViewDecimalColumn1.IsVisible = False
        GridViewDecimalColumn1.Name = "ID"
        GridViewTextBoxColumn1.FieldName = "CMSROUTE_ID"
        GridViewTextBoxColumn1.HeaderText = "CMSROUTE_ID"
        GridViewTextBoxColumn1.Name = "CMSROUTE_ID"
        GridViewTextBoxColumn1.Width = 110
        GridViewTextBoxColumn2.HeaderText = "Month"
        GridViewTextBoxColumn2.Name = "DESCR"
        GridViewTextBoxColumn2.ReadOnly = True
        GridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn2.Width = 60
        GridViewTextBoxColumn3.FieldName = "PERIOD"
        GridViewTextBoxColumn3.HeaderText = "PERIOD"
        GridViewTextBoxColumn3.IsVisible = False
        GridViewTextBoxColumn3.Name = "PERIOD"
        GridViewDecimalColumn2.HeaderText = "FFA"
        GridViewDecimalColumn2.Name = "FIXING"
        GridViewDecimalColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn2.Width = 65
        GridViewDecimalColumn3.HeaderText = "VOL"
        GridViewDecimalColumn3.Name = "VOL"
        GridViewDecimalColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn3.ThousandsSeparator = True
        GridViewDecimalColumn3.Width = 65
        GridViewDecimalColumn4.FieldName = "ROUTE_ID"
        GridViewDecimalColumn4.HeaderText = "ROUTE_ID"
        GridViewDecimalColumn4.IsVisible = False
        GridViewDecimalColumn4.Name = "ROUTE_ID"
        GridViewTextBoxColumn4.FieldName = "QUALIFIER"
        GridViewTextBoxColumn4.HeaderText = "QUALIFIER"
        GridViewTextBoxColumn4.IsVisible = False
        GridViewTextBoxColumn4.Name = "QUALIFIER"
        Me.rgv_DATA.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn1, GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewDecimalColumn2, GridViewDecimalColumn3, GridViewDecimalColumn4, GridViewTextBoxColumn4})
        Me.rgv_DATA.MasterTemplate.DataSource = Me.BALTIC_FTPBindingSource
        Me.rgv_DATA.MasterTemplate.EnableAlternatingRowColor = True
        Me.rgv_DATA.MasterTemplate.EnableGrouping = False
        Me.rgv_DATA.MasterTemplate.HorizontalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysHide
        Me.rgv_DATA.MasterTemplate.ShowRowHeaderColumn = False
        Me.rgv_DATA.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow
        Me.rgv_DATA.Name = "rgv_DATA"
        Me.rgv_DATA.ShowGroupPanel = False
        Me.rgv_DATA.Size = New System.Drawing.Size(316, 530)
        Me.rgv_DATA.TabIndex = 8
        Me.rgv_DATA.Text = "RadGridView1"
        Me.rgv_DATA.ThemeName = "Office2010Silver"
        '
        'ArtBNSGXServiceUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(322, 601)
        Me.Controls.Add(Me.rgv_DATA)
        Me.Controls.Add(Me.rad_DATEPICKER)
        Me.Controls.Add(Me.tb_PASTE)
        Me.Controls.Add(Me.RadButton2)
        Me.Controls.Add(Me.RadButton1)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.SPOT)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ArtBNSGXServiceUpdate"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "SGX Update"
        Me.ThemeName = "Office2010Silver"
        CType(Me.BALTIC_FTPBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SPOT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rad_DATEPICKER, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_DATA.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_DATA, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents BALTIC_FTPBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents SPOT As Telerik.WinControls.UI.RadSpinEditor
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadButton1 As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadButton2 As Telerik.WinControls.UI.RadButton
    Friend WithEvents tb_PASTE As System.Windows.Forms.TextBox
    Friend WithEvents rad_DATEPICKER As Telerik.WinControls.UI.RadDateTimePicker
    Friend WithEvents rgv_DATA As Telerik.WinControls.UI.RadGridView
End Class

