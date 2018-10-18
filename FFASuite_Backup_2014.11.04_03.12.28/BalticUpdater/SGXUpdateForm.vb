Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports System.Drawing.Graphics
Imports Telerik.WinControls.Enumerations

Public Class SGXUpdateForm

    Private FirstTime As Boolean = True

    Private Sub SGXUpdateForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        rad_DATEPICKER.Value = Today
        rad_DATEPICKER.Refresh()
        Try
            BALTIC_FTPBindingSource.DataSource = (From q In DBW.BALTIC_FTP Where q.ROUTE_ID = 71 And q.QUALIFIER = "F" Order By q.ID Select q).ToList
        Catch ex As Exception
            Stop
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
        If SPOT.Value = 0 Then
            Beep()
            MsgError(Me, "Please insert a Spot Value", "Incomplete Data", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End If

        Dim fixDate As Date = rad_DATEPICKER.Value.AddDays(1)

        RadButton2.Enabled = False
        Dim qr = (From q In DBW.BALTIC_FORWARD_RATES _
                  Where q.ROUTE_ID = 71 _
                  And q.FIXING_DATE >= rad_DATEPICKER.Value And q.FIXING_DATE < fixDate _
                  Select q).ToList
        If qr.Count > 0 Then 'its an update query            
            'Update Spot
            Dim qrspot = (From q In DBW.BALTIC_SPOT_RATES _
                          Where q.ROUTE_ID = 71 _
                          And q.FIXING_DATE = rad_DATEPICKER.Value Select q).SingleOrDefault
            qrspot.FIXING = SPOT.Value
            DBW.UpdateObject(qrspot)

            'Update Swaps
            For I = 0 To rgv_DATA.Rows.Count - 1
                If IsNothing(rgv_DATA.Rows(I).Cells("DESCR").Value) Then
                    Continue For
                End If
                If rgv_DATA.Rows(I).Cells("DESCR").Value.ToString = "" Then
                    Continue For
                End If

                Dim tdate As Date = rad_DATEPICKER.Value.AddMonths(I)
                Dim swp = (From q In DBW.BALTIC_FORWARD_RATES _
                           Where q.ROUTE_ID = 71 _
                           And q.CMSROUTE_ID = rgv_DATA.Rows(I).Cells("CMSROUTE_ID").Value.ToString _
                           And q.FIXING_DATE = rad_DATEPICKER.Value Select q).SingleOrDefault
                swp.NEXT_ROLLOVER_DATE = rad_DATEPICKER.Value
                swp.FIXING = CDbl(rgv_DATA.Rows(I).Cells("FIXING").Value)
                swp.REPORTDESC = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString
                swp.MM1 = CShort(Month(tdate))
                swp.YY1 = CShort(Year(tdate))
                swp.MM2 = CShort(Month(tdate))
                swp.YY2 = CShort(Year(tdate))
                swp.PERIOD = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString.Substring(0, 3).ToUpper
                swp.YY = CShort(Year(tdate))
                DBW.UpdateObject(swp)
            Next I

            'Update Vols
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

                Dim tdate As Date = rad_DATEPICKER.Value.AddMonths(I)
                Dim lookupstr As String = "IV_" & rgv_DATA.Rows(I).Cells("CMSROUTE_ID").Value.ToString
                Dim vol = (From q In DBW.BALTIC_OPTION_VOLATILITIES _
                           Where q.ROUTE_ID = 71 _
                           And q.CMSROUTE_ID = lookupstr _
                           And q.FIXING_DATE = rad_DATEPICKER.Value Select q).SingleOrDefault
                vol.NEXT_ROLLOVER_DATE = rad_DATEPICKER.Value
                vol.FIXING = CDbl(rgv_DATA.Rows(I).Cells("VOL").Value)
                vol.REPORTDESC = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString
                vol.MM1 = CShort(Month(tdate))
                vol.YY1 = CShort(Year(tdate))
                vol.MM2 = CShort(Month(tdate))
                vol.YY2 = CShort(Year(tdate))
                vol.PERIOD = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString.Substring(0, 3).ToUpper
                vol.YY = CShort(Year(tdate))
                DBW.UpdateObject(vol)
            Next I
        Else 'its insert new query
            'Insert Spot
            Dim nspot As New FFASuiteDataService.BALTIC_SPOT_RATES
            nspot.ROUTE_ID = 71
            nspot.FIXING_DATE = rad_DATEPICKER.Value
            nspot.FIXING = SPOT.Value
            DBW.AddToBALTIC_SPOT_RATES(nspot)

            'Insert Swaps
            For I = 0 To rgv_DATA.Rows.Count - 1
                If IsNothing(rgv_DATA.Rows(I).Cells("DESCR").Value) Then
                    Continue For
                End If
                If rgv_DATA.Rows(I).Cells("DESCR").Value.ToString = "" Then
                    Continue For
                End If

                Dim nswp As New FFASuiteDataService.BALTIC_FORWARD_RATES
                Dim tdate As Date = rad_DATEPICKER.Value.AddMonths(I)
                nswp.ROUTE_ID = 71
                nswp.CMSROUTE_ID = rgv_DATA.Rows(I).Cells("CMSROUTE_ID").Value.ToString
                nswp.FIXING_DATE = rad_DATEPICKER.Value
                nswp.NEXT_ROLLOVER_DATE = rad_DATEPICKER.Value
                nswp.FIXING = CDbl(rgv_DATA.Rows(I).Cells("FIXING").Value)
                nswp.REPORTDESC = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString
                nswp.MM1 = CShort(Month(tdate))
                nswp.YY1 = CShort(Year(tdate))
                nswp.MM2 = CShort(Month(tdate))
                nswp.YY2 = CShort(Year(tdate))
                nswp.PERIOD = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString.Substring(0, 3).ToUpper
                nswp.YY = CShort(Year(tdate))
                DBW.AddToBALTIC_FORWARD_RATES(nswp)
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

                Dim nvol As New FFASuiteDataService.BALTIC_OPTION_VOLATILITIES
                Dim lookupstr As String = "IV_" & rgv_DATA.Rows(I).Cells("CMSROUTE_ID").Value.ToString
                Dim tdate As Date = rad_DATEPICKER.Value.AddMonths(I)
                nvol.ROUTE_ID = 71
                nvol.CMSROUTE_ID = lookupstr
                nvol.FIXING_DATE = rad_DATEPICKER.Value
                nvol.NEXT_ROLLOVER_DATE = rad_DATEPICKER.Value
                nvol.FIXING = CDbl(rgv_DATA.Rows(I).Cells("VOL").Value)
                nvol.REPORTDESC = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString
                nvol.MM1 = CShort(Month(tdate))
                nvol.YY1 = CShort(Year(tdate))
                nvol.MM2 = CShort(Month(tdate))
                nvol.YY2 = CShort(Year(tdate))
                nvol.PERIOD = rgv_DATA.Rows(I).Cells("DESCR").Value.ToString.Substring(0, 3).ToUpper
                nvol.YY = CShort(Year(tdate))
                DBW.AddToBALTIC_OPTION_VOLATILITIES(nvol)
            Next I
        End If
        Try
            DBW.SaveChanges()
        Catch ex As Exception
            MsgError(Me, "Data Saving Failed", "DB Error", MessageBoxButtons.OK, RadMessageIcon.Error)
            Exit Sub
        End Try
        MsgError(Me, "Data submited succesfully", "DB Update Success", MessageBoxButtons.OK, RadMessageIcon.Info)
        Me.Close()
    End Sub
End Class
