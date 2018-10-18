<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BALTICFTPNEWRECORD
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
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewComboBoxColumn1 As Telerik.WinControls.UI.GridViewComboBoxColumn = New Telerik.WinControls.UI.GridViewComboBoxColumn()
        Dim GridViewComboBoxColumn2 As Telerik.WinControls.UI.GridViewComboBoxColumn = New Telerik.WinControls.UI.GridViewComboBoxColumn()
        Dim GridViewComboBoxColumn3 As Telerik.WinControls.UI.GridViewComboBoxColumn = New Telerik.WinControls.UI.GridViewComboBoxColumn()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BALTICFTPNEWRECORD))
        Me.ROUTESBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.bs_PERIOD = New System.Windows.Forms.BindingSource(Me.components)
        Me.ds_FORM = New System.Data.DataSet()
        Me.PERIOD = New System.Data.DataTable()
        Me.DataColumn2 = New System.Data.DataColumn()
        Me.DataColumn1 = New System.Data.DataColumn()
        Me.QUALIFIER = New System.Data.DataTable()
        Me.DataColumn3 = New System.Data.DataColumn()
        Me.DataColumn4 = New System.Data.DataColumn()
        Me.CMSROUTEID = New System.Data.DataTable()
        Me.DataColumn5 = New System.Data.DataColumn()
        Me.bs_QUALIFIER = New System.Windows.Forms.BindingSource(Me.components)
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rbtn_SAVE = New Telerik.WinControls.UI.RadButton()
        Me.rgv_ROUTES = New Telerik.WinControls.UI.RadGridView()
        CType(Me.ROUTESBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bs_PERIOD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ds_FORM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PERIOD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.QUALIFIER, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CMSROUTEID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bs_QUALIFIER, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_SAVE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_ROUTES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_ROUTES.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ROUTESBindingSource
        '
        Me.ROUTESBindingSource.DataSource = GetType(FFASuiteUpdater.FFASuiteDataService.ROUTES)
        '
        'bs_PERIOD
        '
        Me.bs_PERIOD.DataMember = "PERIOD"
        Me.bs_PERIOD.DataSource = Me.ds_FORM
        '
        'ds_FORM
        '
        Me.ds_FORM.DataSetName = "ds_FORM"
        Me.ds_FORM.Tables.AddRange(New System.Data.DataTable() {Me.PERIOD, Me.QUALIFIER, Me.CMSROUTEID})
        '
        'PERIOD
        '
        Me.PERIOD.Columns.AddRange(New System.Data.DataColumn() {Me.DataColumn2, Me.DataColumn1})
        Me.PERIOD.TableName = "PERIOD"
        '
        'DataColumn2
        '
        Me.DataColumn2.ColumnName = "PERIOD_ID"
        '
        'DataColumn1
        '
        Me.DataColumn1.ColumnName = "DESCR"
        '
        'QUALIFIER
        '
        Me.QUALIFIER.Columns.AddRange(New System.Data.DataColumn() {Me.DataColumn3, Me.DataColumn4})
        Me.QUALIFIER.TableName = "QUALIFIER"
        '
        'DataColumn3
        '
        Me.DataColumn3.ColumnName = "QUALIFIER_ID"
        '
        'DataColumn4
        '
        Me.DataColumn4.ColumnName = "DESCR"
        '
        'CMSROUTEID
        '
        Me.CMSROUTEID.Columns.AddRange(New System.Data.DataColumn() {Me.DataColumn5})
        Me.CMSROUTEID.TableName = "CMSROUTEID"
        '
        'DataColumn5
        '
        Me.DataColumn5.ColumnName = "CMSROUTE_ID"
        '
        'bs_QUALIFIER
        '
        Me.bs_QUALIFIER.DataMember = "QUALIFIER"
        Me.bs_QUALIFIER.DataSource = Me.ds_FORM
        '
        'rbtn_SAVE
        '
        Me.rbtn_SAVE.Location = New System.Drawing.Point(172, 485)
        Me.rbtn_SAVE.Name = "rbtn_SAVE"
        Me.rbtn_SAVE.Size = New System.Drawing.Size(75, 23)
        Me.rbtn_SAVE.TabIndex = 3
        Me.rbtn_SAVE.Text = "Save"
        Me.rbtn_SAVE.ThemeName = "Office2010Silver"
        CType(Me.rbtn_SAVE.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "Save"
        CType(Me.rbtn_SAVE.GetChildAt(0).GetChildAt(1).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CType(Me.rbtn_SAVE.GetChildAt(0).GetChildAt(1).GetChildAt(1), Telerik.WinControls.Primitives.TextPrimitive).Alignment = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rgv_ROUTES
        '
        Me.rgv_ROUTES.Location = New System.Drawing.Point(0, 3)
        '
        'rgv_ROUTES
        '
        Me.rgv_ROUTES.MasterTemplate.AllowAddNewRow = False
        Me.rgv_ROUTES.MasterTemplate.AllowColumnReorder = False
        Me.rgv_ROUTES.MasterTemplate.AllowDeleteRow = False
        GridViewTextBoxColumn1.FieldName = "CMSROUTE_ID"
        GridViewTextBoxColumn1.HeaderText = "CMSROUTE_ID"
        GridViewTextBoxColumn1.Name = "CMSROUTE_ID"
        GridViewTextBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.Width = 105
        GridViewComboBoxColumn1.DataSource = Me.ROUTESBindingSource
        GridViewComboBoxColumn1.DisplayMember = "ROUTE_SHORT"
        GridViewComboBoxColumn1.FieldName = "ROUTE_ID"
        GridViewComboBoxColumn1.HeaderText = "Route Descr"
        GridViewComboBoxColumn1.Name = "ROUTE_ID"
        GridViewComboBoxColumn1.ValueMember = "ROUTE_ID"
        GridViewComboBoxColumn1.Width = 80
        GridViewComboBoxColumn2.DataSource = Me.bs_PERIOD
        GridViewComboBoxColumn2.DisplayMember = "DESCR"
        GridViewComboBoxColumn2.FieldName = "PERIOD_ID"
        GridViewComboBoxColumn2.HeaderText = "Period"
        GridViewComboBoxColumn2.Name = "PERIOD_ID"
        GridViewComboBoxColumn2.ValueMember = "PERIOD_ID"
        GridViewComboBoxColumn2.Width = 100
        GridViewComboBoxColumn3.DataSource = Me.bs_QUALIFIER
        GridViewComboBoxColumn3.DisplayMember = "DESCR"
        GridViewComboBoxColumn3.FieldName = "QUALIFIER_ID"
        GridViewComboBoxColumn3.HeaderText = "Qualifier"
        GridViewComboBoxColumn3.Name = "QUALIFIER_ID"
        GridViewComboBoxColumn3.ValueMember = "QUALIFIER_ID"
        GridViewComboBoxColumn3.Width = 100
        Me.rgv_ROUTES.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewComboBoxColumn1, GridViewComboBoxColumn2, GridViewComboBoxColumn3})
        Me.rgv_ROUTES.MasterTemplate.EnableGrouping = False
        Me.rgv_ROUTES.MasterTemplate.EnableSorting = False
        Me.rgv_ROUTES.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow
        Me.rgv_ROUTES.Name = "rgv_ROUTES"
        Me.rgv_ROUTES.ShowGroupPanel = False
        Me.rgv_ROUTES.Size = New System.Drawing.Size(420, 476)
        Me.rgv_ROUTES.TabIndex = 2
        Me.rgv_ROUTES.ThemeName = "Office2010Silver"
        '
        'BALTICFTPNEWRECORD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(421, 514)
        Me.Controls.Add(Me.rbtn_SAVE)
        Me.Controls.Add(Me.rgv_ROUTES)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BALTICFTPNEWRECORD"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "Add Newly Defined Baltic Periods"
        Me.ThemeName = "Office2010Silver"
        CType(Me.ROUTESBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bs_PERIOD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ds_FORM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PERIOD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.QUALIFIER, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CMSROUTEID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bs_QUALIFIER, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_SAVE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_ROUTES.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_ROUTES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rbtn_SAVE As Telerik.WinControls.UI.RadButton
    Friend WithEvents rgv_ROUTES As Telerik.WinControls.UI.RadGridView
    Friend WithEvents ds_FORM As System.Data.DataSet
    Friend WithEvents PERIOD As System.Data.DataTable
    Friend WithEvents DataColumn2 As System.Data.DataColumn
    Friend WithEvents DataColumn1 As System.Data.DataColumn
    Friend WithEvents QUALIFIER As System.Data.DataTable
    Friend WithEvents DataColumn3 As System.Data.DataColumn
    Friend WithEvents DataColumn4 As System.Data.DataColumn
    Friend WithEvents bs_PERIOD As System.Windows.Forms.BindingSource
    Friend WithEvents bs_QUALIFIER As System.Windows.Forms.BindingSource
    Friend WithEvents CMSROUTEID As System.Data.DataTable
    Friend WithEvents DataColumn5 As System.Data.DataColumn
    Friend WithEvents ROUTESBindingSource As System.Windows.Forms.BindingSource
End Class

