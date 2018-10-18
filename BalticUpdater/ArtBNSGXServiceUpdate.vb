Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports System.Drawing.Graphics
Imports Telerik.WinControls.Enumerations

Public Class ArtBNSGXServiceUpdate

    Private FirstTime As Boolean = True
    Private SpotRates As New List(Of FFAOptCalcService.BALTIC_SPOT_RATES)
    Private ForwardRates As New List(Of FFAOptCalcService.BALTIC_FORWARD_RATES)
    Private Volatilities As New List(Of FFAOptCalcService.BALTIC_OPTION_VOLATILITIES)
    Private FTP As New List(Of FFAOptCalcService.BALTIC_FTP)

    Private Sub ArtBNSGXServiceUpdate_Load(sender As Object, e As EventArgs) Handles Me.Load
        rad_DATEPICKER.Value = Today.Date
        rad_DATEPICKER.Refresh()
        Try
            FTP.AddRange(SDB.GetContractFTP)
            For Each r In FTP
                r.CMSROUTE_ID = r.CMSROUTE_ID.Trim
            Next
            BALTIC_FTPBindingSource.DataSource = (From q In FTP Where q.ROUTE_ID = 71 And q.QUALIFIER = "F" Order By q.ID Select q).ToList
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
    End Sub

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        tb_PASTE.Text = ""
        tb_PASTE.Paste()
        If tb_PASTE.Text.Length = 0 Then
            Beep()
            Exit Sub
        End If

        Dim s As String = tb_PASTE.Text.TrimEnd()
        s = s.Replace(vbNewLine, ";")
        s = s.Replace(",", "")
        s = s.Replace(vbTab, ",")
        Dim data As Array = s.Split(";")

        Dim I As Integer
        Dim sa As Array
        Try
            For Each r In data
                sa = r.ToString.Split(",")
                If sa(0) = "Total" Then Exit For
                rgv_DATA.Rows(I).Cells("DESCR").Value = sa(0)
                rgv_DATA.Rows(I).Cells("FIXING").Value = sa(1)
                If IsNumeric(sa(12)) Then
                    rgv_DATA.Rows(I).Cells("VOL").Value = sa(12) * 100
                End If
                I = I + 1
            Next
        Catch ex As Exception
            For I = 0 To rgv_DATA.RowCount - 1
                rgv_DATA.Rows(I).Cells("DESCR").Value = Nothing
                rgv_DATA.Rows(I).Cells("FIXING").Value = Nothing
                rgv_DATA.Rows(I).Cells("VOL").Value = Nothing
            Next
            Beep()
            MsgError(Me, "Pasted Data does not correspond to grid layout", "Error", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End Try

        RadButton2.Enabled = True
        rgv_DATA.TableElement.Update(GridUINotifyAction.DataChanged)
    End Sub

    Private Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click
        Dim SaveResponse As Boolean = True
        Dim ResponseMsg As New List(Of String)

        If SPOT.Value = 0 Then
            Beep()
            MsgError(Me, "Please insert a Spot Value", "Incomplete Data", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End If

        RadButton2.Enabled = False

        SpotRates.Clear()
        ForwardRates.Clear()
        Volatilities.Clear()

        Dim t_FIXING_DATE As Date = DateSerial(rad_DATEPICKER.Value.Year, rad_DATEPICKER.Value.Month, rad_DATEPICKER.Value.Day)
        'Insert Spot
        Dim nr As New FFAOptCalcService.BALTIC_SPOT_RATES
        nr.FIXING_DATE = t_FIXING_DATE
        nr.ROUTE_ID = 71
        nr.FIXING = SPOT.Value
        SpotRates.Add(nr)

        'Insert Swaps
        For I = 0 To rgv_DATA.Rows.Count - 1
            If IsNothing(rgv_DATA.Rows(I).Cells("DESCR").Value) Then
                Continue For
            End If
            If rgv_DATA.Rows(I).Cells("DESCR").Value.ToString = "" Then
                Continue For
            End If

            Dim nswp As New FFAOptCalcService.BALTIC_FORWARD_RATES
            Dim tdate As Date = t_FIXING_DATE.AddMonths(I)
            nswp.ROUTE_ID = 71
            nswp.CMSROUTE_ID = rgv_DATA.Rows(I).Cells("CMSROUTE_ID").Value.ToString
            nswp.FIXING_DATE = t_FIXING_DATE
            nswp.NEXT_ROLLOVER_DATE = t_FIXING_DATE
            nswp.FIXING = CDbl(rgv_DATA.Rows(I).Cells("FIXING").Value)
            nswp.REPORTDESC = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString
            nswp.MM1 = CShort(Month(tdate))
            nswp.YY1 = CShort(Year(tdate))
            nswp.MM2 = CShort(Month(tdate))
            nswp.YY2 = CShort(Year(tdate))
            nswp.PERIOD = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString.Substring(0, 3).ToUpper
            nswp.YY = CShort(Year(tdate))
            ForwardRates.Add(nswp)
        Next I

        'Insert Vols
        For I = 0 To rgv_DATA.Rows.Count - 1
            If IsNothing(rgv_DATA.Rows(I).Cells("DESCR").Value) Then
                Continue For
            End If
            If rgv_DATA.Rows(I).Cells("DESCR").Value.ToString = "" Then
                Continue For
            End If
            If IsNothing(rgv_DATA.Rows(I).Cells("VOL").Value) Then
                Continue For
            End If
            If rgv_DATA.Rows(I).Cells("VOL").Value.ToString = "" Then
                Continue For
            End If

            Dim nvol As New FFAOptCalcService.BALTIC_OPTION_VOLATILITIES
            Dim lookupstr As String = "IV_" & rgv_DATA.Rows(I).Cells("CMSROUTE_ID").Value.ToString
            Dim vol_FTP_ID = (From q In FTP Where q.CMSROUTE_ID = lookupstr And q.QUALIFIER = "V" Select q).FirstOrDefault
            Dim tdate As Date = t_FIXING_DATE.AddMonths(I)
            nvol.ROUTE_ID = 71
            nvol.CMSROUTE_ID = lookupstr
            nvol.FIXING_DATE = t_FIXING_DATE
            nvol.NEXT_ROLLOVER_DATE = t_FIXING_DATE
            nvol.FIXING = CDbl(rgv_DATA.Rows(I).Cells("VOL").Value)
            nvol.REPORTDESC = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString
            nvol.MM1 = CShort(Month(tdate))
            nvol.YY1 = CShort(Year(tdate))
            nvol.MM2 = CShort(Month(tdate))
            nvol.YY2 = CShort(Year(tdate))
            nvol.PERIOD = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString.Substring(0, 3).ToUpper
            nvol.YY = CShort(Year(tdate))
            Volatilities.Add(nvol)
        Next I

        'now use the service to post fresh data
        If SpotRates.Count > 0 Then
            Try
                Dim result As Boolean = SDB.UpdateSpots(t_FIXING_DATE, SpotRates)
                If result = False Then
                    SaveResponse = False
                    ResponseMsg.Add("Error Updating Spot Rates")
                End If
            Catch ex As Exception
                SaveResponse = False
                ResponseMsg.Add("Error Updating Spot Rates")
            End Try
        End If
        If ForwardRates.Count > 0 Then
            Try
                Dim result As Boolean = SDB.UpdateForwards(t_FIXING_DATE, ForwardRates)
                If result = False Then
                    SaveResponse = False
                    ResponseMsg.Add("Error Updating Forward Rates")
                End If
            Catch ex As Exception
                SaveResponse = False
                ResponseMsg.Add("Error Updating Forward Rates")
            End Try
        End If
        If Volatilities.Count > 0 Then
            Try
                Dim result As Boolean = SDB.UpdateVolatilities(t_FIXING_DATE, Volatilities)
                If result = False Then
                    SaveResponse = False
                    ResponseMsg.Add("Error Updating Option Volatilities")
                End If
            Catch ex As Exception
                SaveResponse = False
                ResponseMsg.Add("Error Updating Option Volatilities")
            End Try
        End If

        Dim s As String = String.Empty
        For Each r In ResponseMsg
            s += r & Environment.NewLine
        Next

        If SaveResponse = False Then
            MsgError(Me, s, "DB Error", MessageBoxButtons.OK, RadMessageIcon.Error)
        Else
            MsgError(Me, "Data submited succesfully", "DB Update Success", MessageBoxButtons.OK, RadMessageIcon.Info)
        End If

        Me.Close()
    End Sub
End Class
