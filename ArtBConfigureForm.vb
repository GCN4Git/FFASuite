Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class ArtBConfigureForm

    Private WithEvents store As New RadPropertyStore
    Private Messages As PropertyStoreItem
    Private BIDASK As PropertyStoreItem
    Private Model As PropertyStoreItem
    Private FP As PropertyStoreItem
    Private DefaultQ As PropertyStoreItem
    Private UpdateLiveInOptCalc As PropertyStoreItem
    Private WithEvents UsesProxy As PropertyStoreItem
    Private ProxyUser As PropertyStoreItem
    Private ProxyPswd As PropertyStoreItem
    Private ProxyAddress As PropertyStoreItem
    Private ConnectionType As PropertyStoreItem
    Public SetUpProxyOnly As Boolean = False
    Public ExternalProxyAddress As String = String.Empty

    Private Sub ArtBConfigureForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Icon = My.Resources.Tool
        Me.rad_PROPERTIES.PropertySort = PropertySort.Categorized
        rad_PROPERTIES.CausesValidation = True

        FP = New PropertyStoreItem(GetType(String), "FingerPrint", UD.MYFINGERPRINT.FINGER_PRINT, "Computer Unique ID", "PC Info", True)
        If SetUpProxyOnly = False Then
            Messages = New PropertyStoreItem(GetType(Boolean), "Messages", My.Settings.Messages, "Indicates if you want to see trade announcements as popup alerts", "Screen", False)
            BIDASK = New PropertyStoreItem(GetType(Boolean), "Bid/Ask", My.Settings.BIDASK, "Indicates if you want to see Bid/Ask Columns", "Screen", False)

            If CudaEnabled Then
                Model = New PropertyStoreItem(GetType(DefaultModelEnum), "Model", My.Settings.Model, "Select Default caclulation model, either analytic or  MonteCarlo simulation", "Model", False)
            Else
                Model = New PropertyStoreItem(GetType(DefaultModelEnum), "Model", My.Settings.Model, "Select Default caclulation model, either analytic or  MonteCarlo simulation", "Model", True)
            End If
            DefaultQ = New PropertyStoreItem(GetType(Integer), "Default Quantity", My.Settings.ModelQuantity, "Select default quantities to be displayed on option strategies", "Model", False)
            UpdateLiveInOptCalc = New PropertyStoreItem(GetType(Boolean), "Calculators Live Prices", My.Settings.UpdateCalcLive, "Check if you want Option and TC Calculators to be updated with live prices", "Model", False)
            store.Add(FP)
            store.Add(Messages)
            store.Add(BIDASK)
            store.Add(Model)
            store.Add(DefaultQ)
            store.Add(UpdateLiveInOptCalc)
        End If

        ConnectionType = New PropertyStoreItem(GetType(MsgConnectivityServiceEnum), "Message Service", My.Settings.msgServiceType, "Select default message exchange service", "Web Connection", False)
        If SetUpProxyOnly = True Then
            UsesProxy = New PropertyStoreItem(GetType(Boolean), "Uses Proxy", True, "Check if your PC uses a Proxy Server to connect to the web", "Web Connection", True)
            ProxyAddress = New PropertyStoreItem(GetType(String), "Proxy Address", ExternalProxyAddress, "e.g. http://www.mycompany.com:1080", "Web Connection", False)
        Else
            UsesProxy = New PropertyStoreItem(GetType(Boolean), "Uses Proxy", My.Settings.UsesProxy, "Check if your PC uses a Proxy Server to connect to the web", "Web Connection", False)
            ProxyAddress = New PropertyStoreItem(GetType(String), "Proxy Address", My.Settings.proxyaddress, "e.g. http://www.mycompany.com:1080", "Web Connection", False)
        End If
        ProxyUser = New PropertyStoreItem(GetType(String), "User Name", My.Settings.proxyuser, "The user name defined in the proxy configuration, leave empty if none.", "Web Connection", False)
        ProxyPswd = New PropertyStoreItem(GetType(String), "User Password", My.Settings.proxypswd, "The user password defined in the proxy configuration, leave empty if none.", "Web Connection", False)

        store.Add(ConnectionType)
        store.Add(UsesProxy)
        store.Add(ProxyAddress)
        store.Add(ProxyUser)
        store.Add(ProxyPswd)

        Me.rad_PROPERTIES.SelectedObject = store

    End Sub

    
    Private Sub rad_PROPERTIES_EditorInitialized(sender As Object, e As PropertyGridItemEditorInitializedEventArgs) Handles rad_PROPERTIES.EditorInitialized
        If e.Item.Name = "Default Quantity" Then
            Dim editor1 As PropertyGridSpinEditor = TryCast(e.Editor, PropertyGridSpinEditor)
            editor1.MinValue = -1
            editor1.MaxValue = 30
            Exit Sub
        End If

        'Dim editor2 As PropertyGridDropDownListEditor = TryCast(e.Editor, PropertyGridDropDownListEditor)
        'If IsNothing(editor2) = False Then
        '    Dim element As BaseDropDownListEditorElement = TryCast(editor2.EditorElement, BaseDropDownListEditorElement)
        '    If IsNothing(element) Then Exit Sub
        '    AddHandler element.RadPropertyChanged, AddressOf element_RadPropertyChanged
        'End If
    End Sub
    Private Sub element_RadPropertyChanged(sender As Object, e As Telerik.WinControls.RadPropertyChangedEventArgs)
        If e.Property.FullName = RadElement.ContainsFocusProperty.FullName AndAlso Not CBool(e.NewValue) Then
            rad_PROPERTIES.EndEdit()
        End If
    End Sub
    Private Sub rbtn_SAVE_Click(sender As Object, e As EventArgs) Handles rbtn_SAVE.Click

        If UsesProxy.Value = True Then
            If ProxyAddress.Value = "" Then
                MsgError(Me, "Proxy Address cannot be an ampty string.", "Input Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
        End If

        If SetUpProxyOnly = False Then
            My.Settings.Model = Model.Value
            My.Settings.Messages = Messages.Value
            My.Settings.BIDASK = BIDASK.Value
            My.Settings.ModelQuantity = DefaultQ.Value
            My.Settings.UpdateCalcLive = UpdateLiveInOptCalc.Value
        End If
        My.Settings.msgServiceType = ConnectionType.Value
        My.Settings.UsesProxy = UsesProxy.Value
        My.Settings.proxyaddress = ProxyAddress.Value
        My.Settings.proxyuser = ProxyUser.Value
        My.Settings.proxypswd = ProxyPswd.Value

        My.Settings.Save()
        If SetUpProxyOnly = True Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
        Me.Close()
    End Sub

    Private Sub UsesProxy_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles UsesProxy.PropertyChanged
        If UsesProxy.Value = True Then
            ProxyAddress.ReadOnly = False
            ProxyUser.ReadOnly = False
            ProxyPswd.ReadOnly = False
        Else
            ProxyAddress.ReadOnly = True
            ProxyUser.ReadOnly = True
            ProxyPswd.ReadOnly = True
        End If
    End Sub

    Private Sub rbt_Cancel_Click(sender As Object, e As EventArgs) Handles rbt_Cancel.Click
        If SetUpProxyOnly = True Then
            Dim answ = MsgError(Me, "If you cancel proxy set up application will exit." & vbCrLf & "Are you sure you want to cancel?", "Alert", MessageBoxButtons.YesNo, Telerik.WinControls.RadMessageIcon.Info)
            If answ = Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
        Me.Close()
    End Sub

End Class
