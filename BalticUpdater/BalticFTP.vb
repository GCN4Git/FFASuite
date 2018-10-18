Imports System.Net.Mail
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.Enumerations
Imports System
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Data.Services.Client
Imports FM

Public Class BalticFTP
    Public DirectoryList As New List(Of BalticFTPClass.SingleFileFTPClass)
    Public DirectoryTable As New DataTable
    'Public Const FTPUserID As String = "BRS_user"
    Public FTPUserID As String = My.Settings.BalticUser
    'Public Const FTPUserPassword As String = "mnb789p"
    Public FTPUserPassword As String = My.Settings.BalticPassword
    Public Const FTPSite As String = "ftp.balticexchange.org"
    Public UpdateLocalDB As Boolean = False
    Friend ThreadFinished As Boolean = False

    Public MessageDisplayed As String = ""
    Public MessageStatus As RadMessageIcon


    Private Sub BalticFTP_Load(sender As Object, e As EventArgs) Handles Me.Load


        Dim tempFTP As New BalticFTPClass(FTPSite, FTPUserID, FTPUserPassword)
        tempFTP.GetAllFilesInDirectory()
        Dim qr0 = (From q In tempFTP.FullDirectoryFiles _
                   Order By q.BalticDate Descending, q.FileType _
                   Select q).Distinct.ToList
        For Each row In qr0
            Dim nr As New BalticFTPClass.SingleFileFTPClass
            nr.FileName = row.FileName
            nr.FilePath = row.FilePath
            nr.FileStr = row.FileStr
            nr.FileType = row.FileType
            nr.BalticDate = row.BalticDate
            nr.FileError = row.FileError
            nr.ErrorMsg = row.ErrorMsg
            DirectoryList.Add(row)
        Next

        rdtp_DATE.Value = Today.Date
    End Sub

    Private Sub rdtp_DATE_ValueChanged(sender As Object, e As EventArgs) Handles rdtp_DATE.ValueChanged
        rlb_DIRECTORY.DisplayMember = "FileName"
        Dim t_FIXING_DATE As Date = DateSerial(rdtp_DATE.Value.Year, rdtp_DATE.Value.Month, rdtp_DATE.Value.Day)
        rlb_DIRECTORY.DataSource = (From q In DirectoryList _
                                    Where q.BalticDate = t_FIXING_DATE _
                                    Order By q.BalticDate Descending, q.FileType, q.FileName _
                                    Select q).Distinct.ToList
        rlb_DIRECTORY.Refresh()
    End Sub

    Private Sub rbtn_Prepare_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_Prepare_SMS.Click

        Me.Cursor = Cursors.WaitCursor

        performFTP()

        If cb_SETLEMENT_ONLY.ToggleState = ToggleState.On Then
            Me.Cursor = Cursors.Default
            MsgError(Me, MessageDisplayed, "Process Result", MessageBoxButtons.OK, MessageStatus)
            Me.Close()
        ElseIf UpdateLocalDB = True Then
            'RefreshLocalDB()
            Me.Cursor = Cursors.Default
            MsgError(Me, MessageDisplayed, "Process Result", MessageBoxButtons.OK, MessageStatus)
            Me.Close()
        End If

    End Sub

    Private Sub RefreshLocalDB()
        Dim DBL As New BRSDataContext(My.Settings.ConnectionString_BRS2)

        If DBL.DatabaseExists = False Then
            MsgError(Me, "BRS DB not accessible, exiting", "DB Connection Error", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End If

        Dim wROUTES = (From q In DBW.ROUTES Select q).ToArray
        Dim wr As Integer() = (From q In wROUTES Select q.ROUTE_ID).ToArray
        Dim lROUTES = (From q In DBL.ROUTES Select q).ToArray
        Dim lr As Integer() = (From q In lROUTES Select q.ROUTE_ID).ToArray
        ' Select the elements from the first array that are not in the second array. 
        Dim onlyInFirstRouteSet As IEnumerable(Of Integer) = wr.Except(lr)
        Try
            For Each r In onlyInFirstRouteSet
                Dim newroute As FFASuiteDataService.ROUTES = (From q In DBW.ROUTES Where q.ROUTE_ID = CInt(r) Select q).FirstOrDefault
                Dim nr As ROUTES = Json.Deserialize(Of ROUTES)(Json.Serialize(newroute))
                DBL.ROUTES.InsertOnSubmit(nr)
            Next
            DBL.SubmitChanges()
        Catch ex As Exception
            MsgError(Me, "Failed to insert New Route", "DB Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            DBL.Dispose()
            Exit Sub
        End Try

        Dim wFTP = (From q In DBW.BALTIC_FTP Select q).ToArray
        Dim wf As Integer() = (From q In wFTP Select q.ID).ToArray
        Dim lFTP = (From q In DBL.BALTIC_FTP Select q).ToArray
        Dim lf As Integer() = (From q In lFTP Select q.ID).ToArray
        Dim onlyInFirstFTPSet As IEnumerable(Of Integer) = wf.Except(lf)
        Try
            For Each r In onlyInFirstFTPSet
                Dim newFTP As FFASuiteDataService.BALTIC_FTP = (From q In DBW.BALTIC_FTP Where q.ID = CInt(r) Select q).FirstOrDefault
                Dim nr As BALTIC_FTP = Json.Deserialize(Of BALTIC_FTP)(Json.Serialize(newFTP))
                DBL.BALTIC_FTP.InsertOnSubmit(nr)
            Next
            DBL.SubmitChanges()
        Catch ex As Exception
            MsgError(Me, "Failed to insert New FTP Record", "DB Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            DBL.Dispose()
            Exit Sub
        End Try

        Dim tDateF As Date = DateSerial(rdtp_DATE.Value.Year, rdtp_DATE.Value.Month, rdtp_DATE.Value.Day)
        Dim tDateL As Date = tDateF.AddDays(1)
        Dim tsF As String = tDateF.Year & Format(tDateF.Month, "00") & Format(tDateF.Day, "00")
        Dim tsL As String = tDateL.Year & Format(tDateL.Month, "00") & Format(tDateL.Day, "00")
        Dim tquery As String = " where FIXING_DATE >= '" & tsF & "' and FIXING_DATE < '" & tsL & "'"
        'Dim MaxWSpot As FFASuiteDataService.BALTIC_SPOT_RATES = (From q In DBW.BALTIC_SPOT_RATES Order By q.FIXING_DATE Descending Select q Take 1).First

        Dim wSpots = (From q In DBW.BALTIC_SPOT_RATES Where q.FIXING_DATE >= tDateF And q.FIXING_DATE < tDateL Select q).ToList
        If wSpots.Count > 0 Then
            DBL.ExecuteCommand("delete from BALTIC_SPOT_RATES" & tquery)
            For Each s In wSpots
                Dim nr As New BALTIC_SPOT_RATES
                nr.FIXING = s.FIXING
                nr.FIXING_DATE = s.FIXING_DATE
                nr.ROUTE_ID = s.ROUTE_ID
                DBL.BALTIC_SPOT_RATES.InsertOnSubmit(nr)
            Next
            Try
                DBL.SubmitChanges()
            Catch ex As Exception
                MsgError(Me, "Failed to insert Spot Data", "DB Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                DBL.Dispose()
                Exit Sub
            End Try
        End If

        Dim wSwaps = (From q In DBW.BALTIC_FORWARD_RATES Where q.FIXING_DATE >= tDateF And q.FIXING_DATE < tDateL Select q).ToList
        If wSwaps.Count > 0 Then
            DBL.ExecuteCommand("delete from BALTIC_FORWARD_RATES" & tquery)
            For Each s In wSwaps
                Dim nr As New BALTIC_FORWARD_RATES
                nr.ROUTE_ID = s.ROUTE_ID
                nr.CMSROUTE_ID = s.CMSROUTE_ID
                nr.FIXING_DATE = s.FIXING_DATE
                nr.NEXT_ROLLOVER_DATE = s.NEXT_ROLLOVER_DATE
                nr.FIXING = s.FIXING
                nr.REPORTDESC = s.REPORTDESC
                nr.YY1 = s.YY1
                nr.YY2 = s.YY2
                nr.MM1 = s.MM1
                nr.MM2 = s.MM2
                nr.PERIOD = s.PERIOD
                nr.YY = s.YY
                DBL.BALTIC_FORWARD_RATES.InsertOnSubmit(nr)
            Next
            Try
                DBL.SubmitChanges()
            Catch ex As Exception
                MsgError(Me, "Failed to insert Swap Data", "DB Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                DBL.Dispose()
                Exit Sub
            End Try
        End If

        Dim wVols = (From q In DBW.BALTIC_OPTION_VOLATILITIES Where q.FIXING_DATE >= tDateF And q.FIXING_DATE < tDateL Select q).ToList
        If wVols.Count > 0 Then
            DBL.ExecuteCommand("delete from BALTIC_OPTION_VOLATILITIES" & tquery)
            For Each s In wVols
                Dim nr As New BALTIC_OPTION_VOLATILITIES
                nr.ROUTE_ID = s.ROUTE_ID
                nr.CMSROUTE_ID = s.CMSROUTE_ID
                nr.FIXING_DATE = s.FIXING_DATE
                nr.NEXT_ROLLOVER_DATE = s.NEXT_ROLLOVER_DATE
                nr.FIXING = s.FIXING
                nr.REPORTDESC = s.REPORTDESC
                nr.YY1 = s.YY1
                nr.YY2 = s.YY2
                nr.MM1 = s.MM1
                nr.MM2 = s.MM2
                nr.PERIOD = s.PERIOD
                nr.YY = s.YY
                DBL.BALTIC_OPTION_VOLATILITIES.InsertOnSubmit(nr)
            Next
            Try
                DBL.SubmitChanges()
            Catch ex As Exception
                MsgError(Me, "Failed to insert Volatility Data", "DB Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                DBL.Dispose()
                Exit Sub
            End Try
        End If

        DBL.Dispose()
    End Sub
    Public Sub performFTP()

        Dim FTP As New BalticFTPClass(FTPSite, FTPUserID, FTPUserPassword)
        Dim templist As New List(Of BalticFTPClass.SingleFileFTPClass)
        Dim t_FIXING_DATE As Date = DateSerial(rdtp_DATE.Value.Year, rdtp_DATE.Value.Month, rdtp_DATE.Value.Day)
        Dim selectedfiles = From q In DirectoryList _
                            Where q.BalticDate = t_FIXING_DATE _
                            Order By q.FileType _
                            Select q
        templist = selectedfiles.ToList
        FTP.FTPSelectedFiles(templist)

        If FTP.FTPReponse.FTPError <> BalticFTPClass.FTPResponses.FTPFileReadOK Then
            MessageDisplayed = FTP.FTPReponse.ErrorMsg
            MessageStatus = RadMessageIcon.Error
            ThreadFinished = True
            Exit Sub
        End If

        'Dim SaveData As New BalticSaveFTPDataClass(templist, cb_INDICES.ToggleState, cb_SETLEMENT_ONLY.ToggleState)

        Dim SaveData As New ArtBNBalticServiceUpdate(t_FIXING_DATE, templist, cb_INDICES.ToggleState, cb_SETLEMENT_ONLY.ToggleState)
        SaveData.SaveResponse = True
        'GoTo LastStep
        SaveData.PerformSave()

        If SaveData.SaveResponse = False Then
            Dim s As String = String.Empty
            For Each r In SaveData.ResponseMsg
                s += r & Environment.NewLine
            Next
            MessageDisplayed = s
            MessageStatus = RadMessageIcon.Error
            ThreadFinished = True
            Exit Sub
        Else
            If cb_INDICES.ToggleState = ToggleState.On Or cb_SETLEMENT_ONLY.ToggleState = ToggleState.On Then
                MessageDisplayed = "All files downloaded from FTP site and saved to database"
                MessageStatus = RadMessageIcon.Info
                ThreadFinished = True
                Exit Sub
            End If
        End If

debugstep:
        ''download BOX fixings
        'Dim fileReader As New WebClient()
        'Dim FileAddress As String = "http://www.lchclearnet.com/container/container.csv"
        'Dim FileString As String
        'Dim BoxIsUpdate As Boolean = False

        'Try
        '    FileString = fileReader.DownloadString(FileAddress)
        'Catch ex As Exception
        '    MessageDisplayed = "Cannot download BOX fixings from LCH"
        '    MessageStatus = MsgBoxStyle.Critical
        '    ThreadFinished = True
        '    Exit Sub
        'End Try

        'Dim qr = (From q In DBW.BALTIC_FORWARD_RATES _
        '          Where q.ROUTE_ID = 78 _
        '          And q.FIXING_DATE = t_FIXING_DATE _
        '          Select q).ToList
        'If qr.Count > 0 Then 'its an update query  
        '    BoxIsUpdate = True
        'End If

        'Dim FileRows As List(Of String) = FileString.Split(vbCrLf).ToList

        'Dim Forwards As New List(Of FFAOptCalcService.BALTIC_FORWARD_RATES)
        'For Each row In FileRows
        '    Dim FIXING_DATE As Date
        '    Dim ROUTE_ID As Integer
        '    Dim CMSROUTE_ID As String
        '    Dim MM1 As Integer, YY1 As Integer
        '    Dim FileLine As Array = row.Split(",")
        '    Dim Contract As String

        '    If row.Length = 1 Then
        '        Continue For
        '    End If

        '    If FileLine(0).ToString.Substring(0, 1) = Chr(10) Then
        '        FIXING_DATE = DateSerial(FileLine(0).ToString.Substring(1, 4), FileLine(0).ToString.Substring(5, 2), FileLine(0).ToString.Substring(7, 2))
        '    Else
        '        FIXING_DATE = DateSerial(FileLine(0).ToString.Substring(0, 4), FileLine(0).ToString.Substring(4, 2), FileLine(0).ToString.Substring(6, 2))
        '    End If

        '    Contract = FileLine(1).ToString.Substring(0, 4)
        '    YY1 = FileLine(1).ToString.Substring(5, 4)
        '    MM1 = FileLine(1).ToString.Substring(10, 2)

        '    If FIXING_DATE.Date <> t_FIXING_DATE Then
        '        Continue For
        '    End If

        '    Select Case Contract
        '        Case "CMDM"
        '            ROUTE_ID = 78
        '        Case "CNWM"
        '            ROUTE_ID = 77
        '        Case "CSEM"
        '            ROUTE_ID = 85
        '        Case "CSWM"
        '            ROUTE_ID = 84
        '    End Select
        '    CMSROUTE_ID = Contract
        '    If t_FIXING_DATE.Year = YY1 And t_FIXING_DATE.Month = MM1 Then 'its current month
        '        CMSROUTE_ID = CMSROUTE_ID & "CURMON"
        '    Else
        '        Dim MonthDiff As Integer = DateDiff(DateInterval.Month, t_FIXING_DATE, DateSerial(YY1, MM1, 1))
        '        If MonthDiff < 0 Then
        '            Continue For
        '        End If
        '        CMSROUTE_ID = CMSROUTE_ID & "+" & MonthDiff & "MON"
        '    End If

        '    Dim FIXING As Double = FileLine(2)
        '    Dim nr As New FFAOptCalcService.BALTIC_FORWARD_RATES
        '    nr.ROUTE_ID = ROUTE_ID
        '    nr.CMSROUTE_ID = CMSROUTE_ID
        '    nr.FIXING_DATE = t_FIXING_DATE
        '    nr.NEXT_ROLLOVER_DATE = t_FIXING_DATE
        '    nr.FIXING = FIXING
        '    nr.REPORTDESC = DateAndTime.MonthName(MM1, True) & "-" & Format(YY1 - 2000, 0)
        '    nr.MM1 = CShort(MM1)
        '    nr.YY1 = CShort(YY1)
        '    nr.MM2 = CShort(MM1)
        '    nr.YY2 = CShort(YY1)
        '    nr.PERIOD = (DateAndTime.MonthName(MM1, True) & "-" & Format(YY1 - 2000, 0)).ToUpper
        '    nr.YY = CShort(YY1)
        '    Forwards.Add(nr)
        'Next

        'If Forwards.Count > 0 Then
        '    Try
        '        Dim SubList = (From q In Forwards Order By q.ROUTE_ID Select q.ROUTE_ID).Distinct
        '        Dim NewList As New List(Of FFAOptCalcService.BALTIC_FORWARD_RATES)
        '        Dim cntr As Integer = 0
        '        For Each r In SubList
        '            cntr += 1
        '            NewList.AddRange(From q In Forwards Where q.ROUTE_ID = r)
        '            If cntr Mod 3 = 0 Then
        '                If NewList.Count > 0 Then
        '                    Dim rslt As Boolean = SDB.UpdateForwards(t_FIXING_DATE, NewList)
        '                    If rslt = False Then
        '                        SaveData.SaveResponse = False
        '                        SaveData.ResponseMsg.Add("Error Updating BOX Forward Rates")
        '                    End If
        '                End If
        '                NewList = New List(Of FFAOptCalcService.BALTIC_FORWARD_RATES)
        '            End If
        '        Next
        '        If NewList.Count > 0 Then
        '            Dim rslt As Boolean = SDB.UpdateForwards(t_FIXING_DATE, NewList)
        '            If rslt = False Then
        '                SaveData.SaveResponse = False
        '                SaveData.ResponseMsg.Add("Error Updating BOX Forward Rates")
        '            End If
        '        End If
        '    Catch ex As Exception
        '        SaveData.SaveResponse = False
        '        SaveData.ResponseMsg.Add("Error Updating BOX Forward Rates")
        '    End Try
        'End If

LastStep:
        Try
            Dim answsprdmargn As Boolean = SDB.UpdateSpreadMargins(t_FIXING_DATE, rse_PERIOD.Value, rse_SDEV.Value)
            If answsprdmargn = False Then
                'SaveData.SaveResponse = False
                'SaveData.ResponseMsg.Add("Error Updating SPREAD MARGINS")
            End If
        Catch ex As Exception
            'SaveData.SaveResponse = False
            'SaveData.ResponseMsg.Add("Error Updating SPREAD MARGINS")
        End Try

        If SaveData.SaveResponse = True Then
            MessageDisplayed = "All files downloaded from FTP site and saved to database"
            MessageStatus = RadMessageIcon.Info
        Else
            Dim s As String = String.Empty
            For Each r In SaveData.ResponseMsg
                s += r & Environment.NewLine
                MessageDisplayed = s
                MessageStatus = RadMessageIcon.Error
            Next
        End If

        ThreadFinished = True
    End Sub

    Private Function CalcMargin(ByVal data As Double(), ByVal StDvNo As Double, ByVal entirePopulation As Boolean) As Double

        Dim values(data.Count - 2) As Double
        Dim J As Integer = 0

        J = 0
        For I As Integer = data.Count - 1 To 1 Step -1
            values(J) = Math.Log10(data(I) / data(I - 1))
            J += 1
        Next

        Dim count As Integer = 0
        Dim var As Double = 0
        Dim prec As Double = 0
        Dim dSum As Double = 0
        Dim sqrSum As Double = 0

        Dim adjustment As Integer = 1
        If entirePopulation Then adjustment = 0

        For Each val As Double In values
            dSum += val
            sqrSum += val * val
            count += 1
        Next

        If count > 1 Then
            var = count * sqrSum - (dSum * dSum)
            prec = var / (dSum * dSum)

            ' Double is only guaranteed for 15 digits. A difference
            ' with a result less than 0.000000000000001 will be considered zero.

            If prec < 0.000000000000001 OrElse var < 0 Then
                var = 0
            Else
                var = var / (count * (count - adjustment))
            End If
            Return Math.Sqrt(var) * StDvNo
        End If

        Return Nothing
    End Function

End Class
