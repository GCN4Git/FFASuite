Imports Microsoft.Win32
Imports System.Text
Imports System.IO

Module ArtBOptCalcFunctions
    Public Function MRound(Number As Double, Multiple As Double) As Double
        MRound = Math.Round(Number / Multiple, 0) * Multiple
    End Function

    Public Sub FIXINGS_Add(ByVal f_List As List(Of FFAOptCalcService.VolDataClass))
        SyncLock FIXINGS_Lock
            FIXINGS.AddRange(f_List)
        End SyncLock
    End Sub
    Public Sub FIXINGS_Add(ByVal f_Value As FFAOptCalcService.VolDataClass)
        SyncLock FIXINGS_Lock
            FIXINGS.Add(f_Value)
        End SyncLock
    End Sub
    Public Sub FIXINGS_RemoveAll(ByVal f_ROUTE_ID As Integer)
        SyncLock FIXINGS_Lock
            FIXINGS.RemoveAll(Function(x) x.ROUTE_ID = f_ROUTE_ID)
        End SyncLock
    End Sub
    Public Sub FIXINGS_RefreshLiveData(ByVal f_ROUTES As List(Of Integer), ByVal f_Data As List(Of FFAOptCalcService.VolDataClass))
        SyncLock FIXINGS_Lock
            For Each r In f_ROUTES
                FIXINGS.RemoveAll(Function(x) x.ROUTE_ID = r And (x.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live Or x.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level))
                FIXINGS.AddRange((From q In f_Data Where q.ROUTE_ID = r And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level)).ToList)

                Dim result = (From q In FIXINGS Where q.ROUTE_ID = r _
                              And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot _
                              Select q).FirstOrDefault
                If result Is Nothing Then
                    Dim nspot = (From q In f_Data Where q.ROUTE_ID = r And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot Select q).FirstOrDefault
                    If IsNothing(nspot) = False Then
                        Dim nc As New FFAOptCalcService.VolDataClass
                        nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot
                        nc.ROUTE_ID = r
                        nc.FIXING_DATE = nspot.FIXING_DATE
                        nc.SPOT_PRICE = nspot.SPOT_PRICE
                        nc.FFA_PRICE = nspot.FFA_PRICE
                        nc.YY1 = 0
                        nc.MM1 = 0
                        nc.YY2 = 0
                        nc.MM2 = 0
                        nc.PERIOD = "SPOT"
                        FIXINGS.Add(nc)
                    End If
                End If
            Next
        End SyncLock
    End Sub
    Public Sub FIXINGS_UpdateSpotPrice(ByVal f_data As FFAOptCalcService.VolDataClass)
        Try
            SyncLock FIXINGS_Lock
                Dim result = (From q In FIXINGS Where _
                              q.ROUTE_ID = f_data.ROUTE_ID _
                              And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot _
                              Select q).FirstOrDefault
                If result Is Nothing Then
                    Dim nc As New FFAOptCalcService.VolDataClass
                    nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot
                    nc.ROUTE_ID = f_data.ROUTE_ID
                    nc.FIXING_DATE = f_data.FIXING_DATE
                    nc.SPOT_PRICE = f_data.SPOT_PRICE
                    nc.FFA_PRICE = f_data.FFA_PRICE
                    nc.YY1 = 0
                    nc.MM1 = 0
                    nc.YY2 = 0
                    nc.MM2 = 0
                    nc.PERIOD = "SPOT"
                    FIXINGS.Add(nc)
                Else
                    result.FIXING_DATE = f_data.FIXING_DATE
                    result.SPOT_PRICE = f_data.SPOT_PRICE
                    result.FFA_PRICE = f_data.FFA_PRICE
                    result.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot
                End If
            End SyncLock
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
            Debug.Print(ex.Message)
        End Try
    End Sub

    Public Sub ROUTES_DETAIL_Add(ByVal f_List As List(Of FFAOptCalcService.SwapDataClass))
        SyncLock ROUTES_DETAIL_Lock
            ROUTES_DETAIL.AddRange(f_List)
        End SyncLock
    End Sub
    Public Sub ROUTES_DETAIL_Add(ByVal f_Value As FFAOptCalcService.SwapDataClass)
        SyncLock ROUTES_DETAIL_Lock
            ROUTES_DETAIL.Add(f_Value)
        End SyncLock
    End Sub
    Public Sub ROUTES_DETAIL_DeleteAdd(ByVal f_Value As List(Of FFAOptCalcService.SwapDataClass))
        SyncLock ROUTES_DETAIL_Lock
            ROUTES_DETAIL.Clear()
            ROUTES_DETAIL.AddRange(f_Value)
        End SyncLock
    End Sub

    Public Function LocalMonthName(ByVal I As Integer) As String
        Dim s As String = String.Empty
        Select Case I
            Case 1
                s = "Jan"
            Case 2
                s = "Feb"
            Case 3
                s = "Mar"
            Case 4
                s = "Apr"
            Case 5
                s = "May"
            Case 6
                s = "Jun"
            Case 7
                s = "Jul"
            Case 8
                s = "Aug"
            Case 9
                s = "Sep"
            Case 10
                s = "Oct"
            Case 11
                s = "Nov"
            Case 12
                s = "Dec"
        End Select
        LocalMonthName = s
    End Function
    Public Function FormatTCPeriod(ByVal Period As Integer) As String
        Dim tDate As Date = DateAdd(DateInterval.Month, Period, SERVER_DATE)
        Dim s As String = LocalMonthName(tDate.Month) & "-" & Format(tDate, "yy")
        Return s
    End Function
    Public Function FFANoDays(ByVal _YY1 As Integer, ByVal _MM1 As Integer, ByVal _YY2 As Integer, ByVal _MM2 As Integer) As Integer
        Dim SDate As Date = DateSerial(_YY1, _MM1, 1)
        Dim LDate As Date = DateSerial(_YY2, _MM2, 28)
        LDate = DateSerial(LDate.Year, LDate.Month, Date.DaysInMonth(LDate.Year, LDate.Month))
        Return DateDiff(DateInterval.Day, SDate, LDate) + 1
    End Function
    Public Function FFANoMonths(ByVal _YY1 As Integer, ByVal _MM1 As Integer, ByVal _YY2 As Integer, ByVal _MM2 As Integer) As Integer
        Dim sdate As Date = DateSerial(_YY1, _MM1, 1)
        Dim edate As Date = DateSerial(_YY2, _MM2, 1)
        Return DateAndTime.DateDiff(DateInterval.Month, sdate, edate) + 1
    End Function
    Public Function FormatPriceTick(ByVal Tick As Double, ByVal Price As Double) As String
        Dim rs As String = String.Empty
        Select Case Math.Log10(Tick)
            Case Is >= 0
                rs = Format(Price, "#,##")
            Case -1
                rs = Format(Price, "0.0")
            Case -2
                rs = Format(Price, "0.00")
            Case -3
                rs = Format(Price, "0.000")
            Case -4
                rs = Format(Price, "0.0000")
        End Select
        Return rs
    End Function
    Public Function MsgError(ByVal sender As Telerik.WinControls.UI.RadForm, msg As String, ByVal msgTitle As String, ByVal msgButton As MessageBoxButtons, ByVal msgIcon As Telerik.WinControls.RadMessageIcon) As DialogResult
        Dim sform As New Telerik.WinControls.UI.RadForm
        Try
            sform = DirectCast(sender, Telerik.WinControls.UI.RadForm)
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
        Telerik.WinControls.RadMessageBox.SetThemeName(sform.ThemeName)
        Telerik.WinControls.RadMessageBox.Instance.FormElement.TitleBar.TitlePrimitive.Font = New Font(sform.FormElement.TitleBar.Font.Name, sform.FormElement.TitleBar.Font.Size + 3, FontStyle.Bold)
        Telerik.WinControls.RadMessageBox.Instance.Controls("radLabel1").Font = New Font(sform.FormElement.Font.Name, sform.FormElement.Font.Size, FontStyle.Bold)
        Telerik.WinControls.RadMessageBox.Instance.BackColor = Color.White
        MsgError = Telerik.WinControls.RadMessageBox.Show(sender, msg, msgTitle, msgButton, msgIcon)
    End Function
    Public Function MsgError(ByVal sender As System.Windows.Forms.Form, msg As String, ByVal msgTitle As String, ByVal msgButton As MessageBoxButtons, ByVal msgIcon As Telerik.WinControls.RadMessageIcon) As DialogResult
        Dim sform As New System.Windows.Forms.Form
        Try
            sform = DirectCast(sender, System.Windows.Forms.Form)
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
        Telerik.WinControls.RadMessageBox.SetThemeName("Office2010Silver")
        Telerik.WinControls.RadMessageBox.Instance.FormElement.TitleBar.TitlePrimitive.Font = New Font(sform.Font.Name, sform.Font.Size + 3, FontStyle.Bold)
        Telerik.WinControls.RadMessageBox.Instance.BackColor = Color.White
        MsgError = Telerik.WinControls.RadMessageBox.Show(sender, msg, msgTitle, msgButton, msgIcon)
    End Function
    Public Function getDefaultBrowser() As String
        Dim browser As String = String.Empty
        Dim key As RegistryKey = Nothing
        Try
            key = Registry.ClassesRoot.OpenSubKey("HTTP\shell\open\command", False)

            'trim off quotes
            browser = key.GetValue(Nothing).ToString().ToLower().Replace("""", "")
            If Not browser.EndsWith("exe") Then
                'get rid of everything after the ".exe"
                browser = browser.Substring(0, browser.LastIndexOf(".exe") + 4)
            End If
        Catch ex As Exception
            browser = String.Format("ERROR" & ex.Message)
        Finally
            If key IsNot Nothing Then
                key.Close()
            End If
        End Try
        Return browser
    End Function
    Public Function OpenEshopSite(ByVal url As String) As Boolean
        Dim browser As String = getDefaultBrowser()

        If Not browser.Contains("ERROR") Then
            Dim process As New Process()
            process.StartInfo.FileName = browser
            process.StartInfo.Arguments = url
            process.Start()
            Return True
        Else
            Return False
        End If
    End Function
    Public Enum DefaultModelEnum
        Analytic
        MonteCarlo
    End Enum

#Region "Cryptography"
    Public Function KeyEncrypt(ByVal str As String)
        Dim answr As String = ""
        answr = RijndaelManagedEncryption.EncryptDecrypt(str, RijndaelManagedEncryption.CryptoKey, RijndaelManagedEncryption.RijndaelCryptographicAction.Encrypt)
        Return answr
    End Function
    Public Function KeyDecrypt(ByVal str As String)
        Dim answr As String = ""
        answr = RijndaelManagedEncryption.EncryptDecrypt(str, RijndaelManagedEncryption.CryptoKey, RijndaelManagedEncryption.RijndaelCryptographicAction.Decrypt)
        Return answr
    End Function
#End Region

    Public Function CheckInternetConnection() As FFAOptCalcService.FPStatusEnum
        Try
            Dim host As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry("www.google.com")
            Return FFAOptCalcService.FPStatusEnum.InternetConnectionOK
        Catch
            Return FFAOptCalcService.FPStatusEnum.NoInternetConnection
        End Try
    End Function
    Public Function CheckServiceStatus() As FFAOptCalcService.FPStatusEnum
        Try
            SERVER_DATE = WEB.SDB.ServerDate
            Return FFAOptCalcService.FPStatusEnum.ServiceOK
        Catch ex As Exception
            Dim npc As WCFServiceStatusClass = WEB.WCFServiceStatus
            If npc.ProxyDetected = True Then

            End If
            Return FFAOptCalcService.FPStatusEnum.ServiceError
        End Try
    End Function
End Module
