﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BRSClientsSMS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BRSClientsSMS))
        Dim GridViewDecimalColumn2 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewComboBoxColumn2 As Telerik.WinControls.UI.GridViewComboBoxColumn = New Telerik.WinControls.UI.GridViewComboBoxColumn()
        Dim GridViewCheckBoxColumn3 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewCheckBoxColumn4 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Me.SMSPROVIDERSBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.BRSDataSet = New FFASuiteUpdater.BRSDataSet()
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.SMSCLIENTSBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.SMSCLIENTSTableAdapter = New FFASuiteUpdater.BRSDataSetTableAdapters.SMSCLIENTSTableAdapter()
        Me.TableAdapterManager = New FFASuiteUpdater.BRSDataSetTableAdapters.TableAdapterManager()
        Me.SMSPROVIDERSTableAdapter = New FFASuiteUpdater.BRSDataSetTableAdapters.SMSPROVIDERSTableAdapter()
        Me.SMSCLIENTSBindingNavigator = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.SMSCLIENTSBindingNavigatorSaveItem = New System.Windows.Forms.ToolStripButton()
        Me.rgv_SMS = New Telerik.WinControls.UI.RadGridView()
        CType(Me.SMSPROVIDERSBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BRSDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SMSCLIENTSBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SMSCLIENTSBindingNavigator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SMSCLIENTSBindingNavigator.SuspendLayout()
        CType(Me.rgv_SMS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgv_SMS.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SMSPROVIDERSBindingSource
        '
        Me.SMSPROVIDERSBindingSource.DataMember = "SMSPROVIDERS"
        Me.SMSPROVIDERSBindingSource.DataSource = Me.BRSDataSet
        '
        'BRSDataSet
        '
        Me.BRSDataSet.DataSetName = "BRSDataSet"
        Me.BRSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'SMSCLIENTSBindingSource
        '
        Me.SMSCLIENTSBindingSource.DataMember = "SMSCLIENTS"
        Me.SMSCLIENTSBindingSource.DataSource = Me.BRSDataSet
        '
        'SMSCLIENTSTableAdapter
        '
        Me.SMSCLIENTSTableAdapter.ClearBeforeFill = True
        '
        'TableAdapterManager
        '
        Me.TableAdapterManager.BackupDataSetBeforeUpdate = False
        Me.TableAdapterManager.SMSCLIENTSTableAdapter = Me.SMSCLIENTSTableAdapter
        Me.TableAdapterManager.SMSPROVIDERSTableAdapter = Me.SMSPROVIDERSTableAdapter
        Me.TableAdapterManager.UpdateOrder = FFASuiteUpdater.BRSDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete
        '
        'SMSPROVIDERSTableAdapter
        '
        Me.SMSPROVIDERSTableAdapter.ClearBeforeFill = True
        '
        'SMSCLIENTSBindingNavigator
        '
        Me.SMSCLIENTSBindingNavigator.AddNewItem = Me.BindingNavigatorAddNewItem
        Me.SMSCLIENTSBindingNavigator.BindingSource = Me.SMSCLIENTSBindingSource
        Me.SMSCLIENTSBindingNavigator.CountItem = Me.BindingNavigatorCountItem
        Me.SMSCLIENTSBindingNavigator.DeleteItem = Me.BindingNavigatorDeleteItem
        Me.SMSCLIENTSBindingNavigator.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.BindingNavigatorAddNewItem, Me.BindingNavigatorDeleteItem, Me.SMSCLIENTSBindingNavigatorSaveItem})
        Me.SMSCLIENTSBindingNavigator.Location = New System.Drawing.Point(0, 0)
        Me.SMSCLIENTSBindingNavigator.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.SMSCLIENTSBindingNavigator.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.SMSCLIENTSBindingNavigator.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.SMSCLIENTSBindingNavigator.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.SMSCLIENTSBindingNavigator.Name = "SMSCLIENTSBindingNavigator"
        Me.SMSCLIENTSBindingNavigator.PositionItem = Me.BindingNavigatorPositionItem
        Me.SMSCLIENTSBindingNavigator.Size = New System.Drawing.Size(949, 25)
        Me.SMSCLIENTSBindingNavigator.TabIndex = 0
        Me.SMSCLIENTSBindingNavigator.Text = "BindingNavigator1"
        '
        'BindingNavigatorAddNewItem
        '
        Me.BindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorAddNewItem.Image = CType(resources.GetObject("BindingNavigatorAddNewItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorAddNewItem.Name = "BindingNavigatorAddNewItem"
        Me.BindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorAddNewItem.Text = "Add new"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(36, 22)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorDeleteItem
        '
        Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorDeleteItem.Image = CType(resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
        Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorDeleteItem.Text = "Delete"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Position"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'SMSCLIENTSBindingNavigatorSaveItem
        '
        Me.SMSCLIENTSBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SMSCLIENTSBindingNavigatorSaveItem.Image = CType(resources.GetObject("SMSCLIENTSBindingNavigatorSaveItem.Image"), System.Drawing.Image)
        Me.SMSCLIENTSBindingNavigatorSaveItem.Name = "SMSCLIENTSBindingNavigatorSaveItem"
        Me.SMSCLIENTSBindingNavigatorSaveItem.Size = New System.Drawing.Size(23, 22)
        Me.SMSCLIENTSBindingNavigatorSaveItem.Text = "Save Data"
        '
        'rgv_SMS
        '
        Me.rgv_SMS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rgv_SMS.Location = New System.Drawing.Point(0, 25)
        '
        'rgv_SMS
        '
        GridViewDecimalColumn2.DataType = GetType(Integer)
        GridViewDecimalColumn2.FieldName = "ID"
        GridViewDecimalColumn2.HeaderText = "ID"
        GridViewDecimalColumn2.IsAutoGenerated = True
        GridViewDecimalColumn2.Name = "ID"
        GridViewDecimalColumn2.ReadOnly = True
        GridViewTextBoxColumn6.FieldName = "COMPANY"
        GridViewTextBoxColumn6.HeaderText = "COMPANY"
        GridViewTextBoxColumn6.IsAutoGenerated = True
        GridViewTextBoxColumn6.Name = "COMPANY"
        GridViewTextBoxColumn6.Width = 125
        GridViewTextBoxColumn7.FieldName = "FIRST_NAME"
        GridViewTextBoxColumn7.HeaderText = "FIRST NAME"
        GridViewTextBoxColumn7.IsAutoGenerated = True
        GridViewTextBoxColumn7.Name = "FIRST_NAME"
        GridViewTextBoxColumn7.Width = 105
        GridViewTextBoxColumn8.FieldName = "LAST_NAME"
        GridViewTextBoxColumn8.HeaderText = "LAST NAME"
        GridViewTextBoxColumn8.IsAutoGenerated = True
        GridViewTextBoxColumn8.Name = "LAST_NAME"
        GridViewTextBoxColumn8.Width = 150
        GridViewTextBoxColumn9.FieldName = "MOBILE"
        GridViewTextBoxColumn9.HeaderText = "MOBILE"
        GridViewTextBoxColumn9.IsAutoGenerated = True
        GridViewTextBoxColumn9.Name = "MOBILE"
        GridViewTextBoxColumn9.Width = 95
        GridViewComboBoxColumn2.DataSource = Me.SMSPROVIDERSBindingSource
        GridViewComboBoxColumn2.DataType = GetType(Integer)
        GridViewComboBoxColumn2.DisplayMember = "SMSPROVIDER"
        GridViewComboBoxColumn2.FieldName = "SMSPROVIDERID"
        GridViewComboBoxColumn2.HeaderText = "SMS PROVIDER"
        GridViewComboBoxColumn2.Name = "SMSPROVIDERID"
        GridViewComboBoxColumn2.ValueMember = "ID"
        GridViewComboBoxColumn2.Width = 95
        GridViewCheckBoxColumn3.FieldName = "CAPES"
        GridViewCheckBoxColumn3.HeaderText = "CAPES"
        GridViewCheckBoxColumn3.IsAutoGenerated = True
        GridViewCheckBoxColumn3.Name = "CAPES"
        GridViewCheckBoxColumn4.FieldName = "PMX"
        GridViewCheckBoxColumn4.HeaderText = "PMX"
        GridViewCheckBoxColumn4.IsAutoGenerated = True
        GridViewCheckBoxColumn4.Name = "PMX"
        GridViewTextBoxColumn10.FieldName = "RESPONSE"
        GridViewTextBoxColumn10.HeaderText = "RESPONSE"
        GridViewTextBoxColumn10.IsAutoGenerated = True
        GridViewTextBoxColumn10.Name = "RESPONSE"
        GridViewTextBoxColumn10.Width = 200
        Me.rgv_SMS.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn2, GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8, GridViewTextBoxColumn9, GridViewComboBoxColumn2, GridViewCheckBoxColumn3, GridViewCheckBoxColumn4, GridViewTextBoxColumn10})
        Me.rgv_SMS.MasterTemplate.DataSource = Me.SMSCLIENTSBindingSource
        Me.rgv_SMS.MasterTemplate.EnableAlternatingRowColor = True
        Me.rgv_SMS.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow
        Me.rgv_SMS.Name = "rgv_SMS"
        Me.rgv_SMS.Size = New System.Drawing.Size(949, 563)
        Me.rgv_SMS.TabIndex = 1
        Me.rgv_SMS.Text = "RadGridView1"
        Me.rgv_SMS.ThemeName = "Office2010Silver"
        '
        'BRSClientsSMS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(949, 588)
        Me.Controls.Add(Me.rgv_SMS)
        Me.Controls.Add(Me.SMSCLIENTSBindingNavigator)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BRSClientsSMS"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "SMS Clients"
        Me.ThemeName = "Office2010Silver"
        CType(Me.SMSPROVIDERSBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BRSDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SMSCLIENTSBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SMSCLIENTSBindingNavigator, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SMSCLIENTSBindingNavigator.ResumeLayout(False)
        Me.SMSCLIENTSBindingNavigator.PerformLayout()
        CType(Me.rgv_SMS.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgv_SMS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents BRSDataSet As FFASuiteUpdater.BRSDataSet
    Friend WithEvents SMSCLIENTSBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents SMSCLIENTSTableAdapter As FFASuiteUpdater.BRSDataSetTableAdapters.SMSCLIENTSTableAdapter
    Friend WithEvents TableAdapterManager As FFASuiteUpdater.BRSDataSetTableAdapters.TableAdapterManager
    Friend WithEvents SMSCLIENTSBindingNavigator As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SMSCLIENTSBindingNavigatorSaveItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents SMSPROVIDERSTableAdapter As FFASuiteUpdater.BRSDataSetTableAdapters.SMSPROVIDERSTableAdapter
    Friend WithEvents rgv_SMS As Telerik.WinControls.UI.RadGridView
    Friend WithEvents SMSPROVIDERSBindingSource As System.Windows.Forms.BindingSource
End Class

