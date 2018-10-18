<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdaterSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UpdaterSettings))
        Me.Office2010SilverTheme1 = New Telerik.WinControls.Themes.Office2010SilverTheme()
        Me.rbt_Cancel = New Telerik.WinControls.UI.RadButton()
        Me.rbtn_SAVE = New Telerik.WinControls.UI.RadButton()
        Me.rad_PROPERTIES = New Telerik.WinControls.UI.RadPropertyGrid()
        CType(Me.rbt_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbtn_SAVE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rad_PROPERTIES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rbt_Cancel
        '
        Me.rbt_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbt_Cancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbt_Cancel.Location = New System.Drawing.Point(181, 377)
        Me.rbt_Cancel.Name = "rbt_Cancel"
        Me.rbt_Cancel.Size = New System.Drawing.Size(93, 24)
        Me.rbt_Cancel.TabIndex = 5
        Me.rbt_Cancel.Text = "Cancel and Exit"
        Me.rbt_Cancel.ThemeName = "Office2010Silver"
        '
        'rbtn_SAVE
        '
        Me.rbtn_SAVE.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbtn_SAVE.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rbtn_SAVE.Location = New System.Drawing.Point(53, 377)
        Me.rbtn_SAVE.Name = "rbtn_SAVE"
        Me.rbtn_SAVE.Size = New System.Drawing.Size(93, 24)
        Me.rbtn_SAVE.TabIndex = 4
        Me.rbtn_SAVE.Text = "Save and Exit"
        Me.rbtn_SAVE.ThemeName = "Office2010Silver"
        '
        'rad_PROPERTIES
        '
        Me.rad_PROPERTIES.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rad_PROPERTIES.Location = New System.Drawing.Point(0, 0)
        Me.rad_PROPERTIES.Name = "rad_PROPERTIES"
        Me.rad_PROPERTIES.Size = New System.Drawing.Size(329, 369)
        Me.rad_PROPERTIES.TabIndex = 3
        Me.rad_PROPERTIES.Text = "RadPropertyGrid1"
        Me.rad_PROPERTIES.ThemeName = "Office2010Silver"
        Me.rad_PROPERTIES.ToolbarVisible = True
        '
        'UpdaterSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(329, 408)
        Me.Controls.Add(Me.rbt_Cancel)
        Me.Controls.Add(Me.rbtn_SAVE)
        Me.Controls.Add(Me.rad_PROPERTIES)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "UpdaterSettings"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "FFA Suite Updatere Settings"
        Me.ThemeName = "Office2010Silver"
        CType(Me.rbt_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbtn_SAVE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rad_PROPERTIES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Office2010SilverTheme1 As Telerik.WinControls.Themes.Office2010SilverTheme
    Friend WithEvents rbt_Cancel As Telerik.WinControls.UI.RadButton
    Friend WithEvents rbtn_SAVE As Telerik.WinControls.UI.RadButton
    Friend WithEvents rad_PROPERTIES As Telerik.WinControls.UI.RadPropertyGrid
End Class

