Public Class BALTICFTPNEWRECORD
    Private m_CMSROUTE_ID As List(Of String)
    Public Property CMSROUTE_ID As List(Of String)
        Get
            Return m_CMSROUTE_ID
        End Get
        Set(value As List(Of String))
            m_CMSROUTE_ID = value
        End Set
    End Property

    Private Sub BALTICFTPNEWRECORD_Load(sender As Object, e As EventArgs) Handles Me.Load
        ROUTESBindingSource.DataSource = (From q In DBW.ROUTES _
                                          Where q.FFA_TRADED = True _
                                          Order By q.ROUTE_SHORT _
                                          Select q.ROUTE_ID, q.ROUTE_SHORT).ToList
        PERIOD.Rows.Add("D", "Day")
        PERIOD.Rows.Add("M", "Month")
        PERIOD.Rows.Add("Q", "Quarter")
        PERIOD.Rows.Add("C", "Calendar")

        QUALIFIER.Rows.Add("I", "Index")
        QUALIFIER.Rows.Add("F", "Forward")
        QUALIFIER.Rows.Add("V", "Volatility")

        For Each r In CMSROUTE_ID
            CMSROUTEID.Rows.Add(r)
        Next

        rgv_ROUTES.DataSource = CMSROUTEID
    End Sub

    Private Sub rbtn_SAVE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_SAVE.Click
        Dim AllOK As Boolean = True

        rgv_ROUTES.EndEdit()
        'check if all values have been input correctly
        For I = 0 To rgv_ROUTES.Rows.Count - 1
            If IsNothing(rgv_ROUTES.Rows(I).Cells("ROUTE_ID").Value) Then
                AllOK = False
            End If
            If IsNothing(rgv_ROUTES.Rows(I).Cells("PERIOD_ID").Value) Then
                AllOK = False
            End If
            If IsNothing(rgv_ROUTES.Rows(I).Cells("QUALIFIER_ID").Value) Then
                AllOK = False
            End If
        Next

        If AllOK = False Then
            MsgError(Me, "Please enter corresponding values for all cells in order to be able to save data", "Incomplete", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        For I = 0 To rgv_ROUTES.Rows.Count - 1
            Dim nr As New FFASuiteDataService.BALTIC_FTP
            nr.ROUTE_ID = rgv_ROUTES.Rows(I).Cells("ROUTE_ID").Value
            nr.CMSROUTE_ID = rgv_ROUTES.Rows(I).Cells("CMSROUTE_ID").Value
            nr.PERIOD = CChar(rgv_ROUTES.Rows(I).Cells("PERIOD_ID").Value)
            nr.QUALIFIER = CChar(rgv_ROUTES.Rows(I).Cells("QUALIFIER_ID").Value)
            DBW.AddToBALTIC_FTP(nr)

            'you also have to insert this record to VESSEL_CLASS_SPREAD_MARGINS, if is default route for vessel class
            Dim RouteExists = (From q In DBW.VESSEL_CLASS_SPREAD_MARGINS _
                               Where q.ROUTE_ID = nr.ROUTE_ID _
                               Select q).ToList
            If RouteExists.Count > 0 Then
                Dim nvcsm As New FFASuiteDataService.VESSEL_CLASS_SPREAD_MARGINS
                nvcsm.VESSEL_CLASS_ID = RouteExists.First.VESSEL_CLASS_ID
                nvcsm.ROUTE_ID = nr.ROUTE_ID
                nvcsm.CMSROUTE_ID = nr.CMSROUTE_ID
                nvcsm.YY1 = 0
                nvcsm.MM1 = 0
                nvcsm.YY2 = 0
                nvcsm.MM2 = 0
                nvcsm.PERIOD = ""
                nvcsm.YY = 0
                nvcsm.MARGIN = 0.0#
                DBW.AddToVESSEL_CLASS_SPREAD_MARGINS(nvcsm)
            End If
        Next
        Try
            DBW.SaveChanges()
        Catch ex As Exception
            MsgError(Me, "Data not saved. Report Error to DB Administrator", "DB Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Me.Close()
        End Try
        MsgError(Me, "Data saved suuccesfully.", "DB Update Success", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        Me.Close()
    End Sub
End Class
