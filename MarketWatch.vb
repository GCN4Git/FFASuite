Imports System.ComponentModel
Imports Telerik.WinControls.UI
Imports ArtB_Class_Library
Imports FM
Imports FM.WebSync
Imports agsXMPP
Imports agsXMPP.Xml
Imports agsXMPP.protocol
Imports agsXMPP.protocol.client

Public Class MarketWatch

    Private m_GRIDDATA As New List(Of GRIDPeriodsClass)
    Private m_DATA As New DataTable
    Private m_DATA_LOCK As New Object

    Private m_FIXINGS As New List(Of FFAOptCalcService.VolDataClass)
    Private m_HFIXINGS As New List(Of FFAOptCalcService.VolDataClass)
    Private m_FIXINGS_Lock As New Object
    Private m_LastFixDate As Date
    Private m_PrvFixDate As Date
    
    Private m_GRID_LOCK As New Object
    Private m_HeaderContextMenu As RadContextMenu
    Private m_GridContextMenu As RadContextMenu
    Private m_BIDASKContextMenu As RadContextMenu
    Public WithEvents EventForm As FFAOptCalc
    Private m_GridLock As Object

    Private m_FontsR As New List(Of Font)
    Private m_FontsB As New List(Of Font)
    Private m_FontB As Font
    Private m_FontR As Font

    Public TabView As DataContracts.MarketTabClass
    Private m_TabViewSync As New Object
    Private m_MouseClick As MouseButtons
    Private m_ColumnMoveFlag As Boolean = False
    Private m_OldRouteIndex As Integer
    Private m_FreezeCellValueChanging As Boolean = False

    Private Sub MarketWatch_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveTabSettings()
    End Sub
    Private Sub SaveTabSettings()
        SyncLock m_TabViewSync
            Dim rid = (From q In rgv_MARKET.Columns Where q.Name <> "PERIOD" And q.IsVisible = True Select q).ToList
            TabView.WIDTH = rgv_MARKET.Columns("PERIOD").Width
            TabView.ROUTES.Clear()
            For Each r In rid
                If r.Name.Contains("BID") = False Then
                    If r.Name.Contains("ASK") = False Then
                        Dim nr As New DataContracts.RouteViewClass
                        nr.ROUTE_ID = CInt(r.Name)
                        nr.WIDTH = r.Width
                        TabView.ROUTES.Add(nr)
                    End If
                End If
            Next
            g_MarketViews.Save()
        End SyncLock
    End Sub

    Private Sub MasterTemplateColumns_CollectionChanging(sender As Object, e As Telerik.WinControls.Data.NotifyCollectionChangingEventArgs)
        If IsNothing(e) Then Exit Sub

        If e.Action = Telerik.WinControls.Data.NotifyCollectionChangedAction.Move Then
            If e.NewStartingIndex <= 4 Then
                e.Cancel = True
                Dim viscol = (From q In rgv_MARKET.Columns Where q.Index > 0 And q.IsVisible = True)
                For Each c In viscol
                    c.IsPinned = False
                Next
                rgv_MARKET.TableElement.Update(GridUINotifyAction.StateChanged)
                rgv_MARKET.Refresh()
                Exit Sub
            End If

            Dim ROUTE = TryCast(rgv_MARKET.Columns(e.NewStartingIndex).Tag, DataContracts.RouteViewClass)
            If IsNothing(ROUTE) Then
                e.Cancel = True
            End If

            If rgv_MARKET.Columns(e.OldStartingIndex).Name.Contains("BID") Or rgv_MARKET.Columns(e.OldStartingIndex).Name.Contains("ASK") Then
                If m_ColumnMoveFlag = False Then
                    e.Cancel = True
                    Exit Sub
                End If                
            End If

            If rgv_MARKET.Columns(e.NewStartingIndex).Name.Contains("BID") Or rgv_MARKET.Columns(e.NewStartingIndex).Name.Contains("ASK") Then
                If m_ColumnMoveFlag = False Then
                    e.Cancel = True
                    Exit Sub
                End If
            End If

            Try
                m_OldRouteIndex = TryCast(rgv_MARKET.Columns(e.NewStartingIndex).Tag, DataContracts.RouteViewClass).ROUTE_ID
            Catch ex As Exception
                m_OldRouteIndex = 0
            End Try
        End If
    End Sub
    Private Sub MasterTemplateColumns_CollectionChanged(sender As Object, e As Telerik.WinControls.Data.NotifyCollectionChangedEventArgs)
        If IsNothing(e) Then Exit Sub

        If e.Action = Telerik.WinControls.Data.NotifyCollectionChangedAction.Move Then

            If m_ColumnMoveFlag = False Then
                'ammend the ViewRoutes element for the position change
                Dim move_ROUTE_ID As Integer = TryCast(rgv_MARKET.Columns(e.NewStartingIndex).Tag, DataContracts.RouteViewClass).ROUTE_ID
                Dim to_ROUTE_ID As Integer = m_OldRouteIndex

                Dim oldIndex As Integer
                For Each r In TabView.ROUTES
                    If r.ROUTE_ID = move_ROUTE_ID Then
                        Exit For
                    End If
                    oldIndex += 1
                Next

                Dim NewIndex As Integer
                If to_ROUTE_ID = 0 Then
                    NewIndex = 0
                Else                    
                    For Each r In TabView.ROUTES
                        If r.ROUTE_ID = to_ROUTE_ID Then
                            Exit For
                        End If
                        NewIndex += 1
                    Next
                End If
                TabView.Move(oldIndex, NewIndex)

                m_ColumnMoveFlag = True
                If e.OldStartingIndex < e.NewStartingIndex Then
                    Dim iLASTTRADED As Integer = rgv_MARKET.Columns(move_ROUTE_ID.ToString).Index
                    Dim iBID As Integer = rgv_MARKET.Columns(move_ROUTE_ID.ToString & "_BID").Index
                    rgv_MARKET.Columns.Move(iBID, iLASTTRADED + 1)
                    Dim iASK As Integer = rgv_MARKET.Columns(move_ROUTE_ID.ToString & "_ASK").Index
                    rgv_MARKET.Columns.Move(iASK, iLASTTRADED + 2)

                    iLASTTRADED = rgv_MARKET.Columns(to_ROUTE_ID.ToString).Index
                    iBID = rgv_MARKET.Columns(to_ROUTE_ID.ToString & "_BID").Index
                    rgv_MARKET.Columns.Move(iBID, iLASTTRADED + 1)
                    iASK = rgv_MARKET.Columns(to_ROUTE_ID.ToString & "_ASK").Index
                    rgv_MARKET.Columns.Move(iASK, iLASTTRADED + 2)

                    'rgv_MARKET.Columns.Move(e.OldStartingIndex + 1, e.NewStartingIndex + 1)
                    'rgv_MARKET.Columns.Move(e.OldStartingIndex + 2, e.NewStartingIndex + 2)

                Else
                    rgv_MARKET.Columns.Move(e.OldStartingIndex + 1, e.NewStartingIndex + 1)
                    rgv_MARKET.Columns.Move(e.OldStartingIndex + 2, e.NewStartingIndex + 2)
                End If
                rgv_MARKET.Columns(e.NewStartingIndex).IsPinned = False
                rgv_MARKET.TableElement.Update(GridUINotifyAction.StateChanged)
                rgv_MARKET.Refresh()
                m_ColumnMoveFlag = False
            End If
            
        End If
    End Sub

    Private Sub MarketWatch_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Icon = My.Resources.Table

        AddHandler rgv_MARKET.MasterTemplate.Columns.CollectionChanging, New Telerik.WinControls.Data.NotifyCollectionChangingEventHandler(AddressOf MasterTemplateColumns_CollectionChanging)
        AddHandler rgv_MARKET.MasterTemplate.Columns.CollectionChanged, New Telerik.WinControls.Data.NotifyCollectionChangedEventHandler(AddressOf MasterTemplateColumns_CollectionChanged)

        BuildFonts()
        FetchLocalData()
        ConstructDataTable()
        PopulateAll()
        BuildContextMenu()
    End Sub

#Region "ContructGrid"
    Public Sub FetchLocalData(Optional ByVal IgnoreLiveData As Boolean = False)
        'Construct local FIXINGS lists
        m_FIXINGS.Clear()
        m_LastFixDate = (From q In FIXINGS Where q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap Select q.FIXING_DATE).Max
        m_PrvFixDate = (From q In FIXINGS Where q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap And q.FIXING_DATE < m_LastFixDate Select q.FIXING_DATE).Max
        Dim viewroutelist As New List(Of Integer)
        For Each r In TabView.ROUTES
            viewroutelist.Add(r.ROUTE_ID)
        Next
        Dim tdata = (From q In FIXINGS Where q.FIXING_DATE = m_LastFixDate _
                     And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
                     And viewroutelist.Contains(q.ROUTE_ID) Select q).ToList
        Try
            For Each r In tdata
                Dim nc As New FFAOptCalcService.VolDataClass
                If IgnoreLiveData = False Then
                    nc.SPOT_PRICE = r.FFA_PRICE 'so that I can figure out if live trades are higher or lower from old data
                Else
                    Dim oldprc = (From q In FIXINGS Where q.ROUTE_ID = r.ROUTE_ID _
                                  And q.FIXING_DATE = m_PrvFixDate _
                                  And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
                                  And q.YY1 = r.YY1 And q.MM1 = r.MM1 And q.YY2 = r.YY2 And q.MM2 = r.MM2 _
                                  Select q).FirstOrDefault
                    If IsNothing(oldprc) = False Then
                        nc.SPOT_PRICE = oldprc.FFA_PRICE
                    Else
                        nc.SPOT_PRICE = r.FFA_PRICE 'so that I can figure out if live trades are higher or lower from old data
                    End If
                End If

                'Update with live prices
                Dim livedata = (From q In FIXINGS Where q.ROUTE_ID = r.ROUTE_ID _
                                And q.FIXING_DATE > m_LastFixDate _
                                And q.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA _
                                And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level) _
                                And q.YY1 = r.YY1 And q.MM1 = r.MM1 And q.YY2 = r.YY2 And q.MM2 = r.MM2 _
                                Order By q.FIXING_DATE Descending _
                                Select q).FirstOrDefault
                If IsNothing(livedata) = False And IgnoreLiveData = False Then
                    nc.TRADE_ID = livedata.TRADE_ID
                    nc.FFA_PRICE = livedata.FFA_PRICE
                    nc.VolRecordType = livedata.VolRecordType
                Else
                    nc.FFA_PRICE = r.FFA_PRICE
                    nc.VolRecordType = r.VolRecordType
                End If
                nc.FIXING_DATE = r.FIXING_DATE
                nc.HVOL = r.HVOL
                nc.INTEREST_RATE = r.INTEREST_RATE
                nc.IVOL = r.IVOL
                nc.MM1 = r.MM1
                nc.MM2 = r.MM2
                nc.ONLYHISTORICAL = r.ONLYHISTORICAL
                nc.PERIOD = r.PERIOD
                nc.ROUTE_ID = r.ROUTE_ID
                nc.YY1 = r.YY1
                nc.YY2 = r.YY2
                m_FIXINGS_ADD(nc)
            Next
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
    End Sub
    Public Sub PopulateAll(Optional ByVal IgnoreLiveData As Boolean = False)
        'Define Spot Row
        Dim spotrow As DataRow = m_DATA.NewRow
        spotrow.Item("PERIOD") = "Spot"
        spotrow.Item("YY1") = 0 : spotrow.Item("MM1") = 0 : spotrow.Item("YY2") = 0 : spotrow.Item("MM2") = 0
        For Each r In TabView.ROUTES            
            Dim ROUTE_ID As Integer = r.ROUTE_ID
            Dim route = (From q In ROUTES_DETAIL Where q.ROUTE_ID = ROUTE_ID Select q).FirstOrDefault
            spotrow.Item(ROUTE_ID.ToString & "_BID") = ROUTE_ID
            spotrow.Item(ROUTE_ID.ToString & "_ASK") = ROUTE_ID
            Dim Spot = (From q In FIXINGS _
                        Where q.ROUTE_ID = ROUTE_ID _
                        And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot) _
                        Order By q.FIXING_DATE Descending Select q).FirstOrDefault
            If IsNothing(Spot) = False Then
                Dim spotprc As Double = Spot.SPOT_PRICE
                Dim voltype As FFAOptCalcService.VolRecordTypeEnum = Spot.VolRecordType
                spotrow.Item(ROUTE_ID.ToString & "_ID") = ROUTE_ID
                spotrow.Item(ROUTE_ID.ToString & "_V") = Spot.VolRecordType
                spotrow.Item(ROUTE_ID.ToString & "_F") = FlashEnum.Normal
                If Spot.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot Then
                    Dim oldspot = (From q In FIXINGS _
                                   Where q.ROUTE_ID = ROUTE_ID _
                                   And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot _
                                   Order By q.FIXING_DATE Descending Select q).FirstOrDefault
                    If IsNothing(oldspot) = False Then
                        spotrow.Item(ROUTE_ID.ToString & "_H") = oldspot.SPOT_PRICE
                    End If
                ElseIf Spot.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot Then
                    spotrow.Item(ROUTE_ID.ToString & "_H") = spotprc
                End If
                spotrow.Item(ROUTE_ID.ToString) = spotprc
            End If
        Next r
        m_DATA.Rows.Add(spotrow)

        'Define Forwards
        For Each p In g_MVPeriods
            Dim nrow As DataRow = m_DATA.NewRow
            nrow.Item("PERIOD") = p.Descr
            nrow.Item("YY1") = p.YY1 : nrow.Item("MM1") = p.MM1 : nrow.Item("YY2") = p.YY2 : nrow.Item("MM2") = p.MM2
            For Each r In TabView.ROUTES
                Dim ROUTE_ID As Integer = r.ROUTE_ID

                Dim RouteDtl = (From q In ROUTES_DETAIL Where q.ROUTE_ID = ROUTE_ID Select q).FirstOrDefault
                Dim maxyear = (From q In FIXINGS Where q.ROUTE_ID = ROUTE_ID _
                               And q.FIXING_DATE = m_LastFixDate _
                               And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
                               Select q.YY2).Max
                If p.YY2 <= maxyear Then
                    Dim ffaprc = (From q In m_FIXINGS _
                                  Where q.ROUTE_ID = ROUTE_ID _
                                  And (q.YY1 = p.YY1 And q.MM1 = p.MM1 And q.YY2 = p.YY2 And q.MM2 = p.MM2) _
                                  Select q).FirstOrDefault
                    If IsNothing(ffaprc) = False Then 'we have a live price or old swap fixing
                        nrow.Item(ROUTE_ID.ToString & "_ID") = ROUTE_ID
                        nrow.Item(ROUTE_ID.ToString & "_V") = ffaprc.VolRecordType
                        nrow.Item(ROUTE_ID.ToString & "_F") = FlashEnum.Normal
                        nrow.Item(ROUTE_ID.ToString & "_H") = ffaprc.SPOT_PRICE
                        nrow.Item(ROUTE_ID.ToString) = ffaprc.FFA_PRICE
                        nrow.Item(ROUTE_ID.ToString & "_TID") = ffaprc.TRADE_ID
                    Else
                        'first check if live data exists for this non standard period
                        Dim livedata = (From q In FIXINGS Where q.ROUTE_ID = ROUTE_ID _
                                        And q.YY1 = p.YY1 And q.MM1 = p.MM1 And q.YY2 = p.YY2 And q.MM2 = p.MM2 _
                                        And q.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA _
                                        And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level) _
                                        And q.FIXING_DATE > m_LastFixDate _
                                        Order By q.FIXING_DATE Descending _
                                        Select q).FirstOrDefault
                        If IsNothing(livedata) = False And IgnoreLiveData = False Then
                            Dim tprc As Double = livedata.FFA_PRICE
                            Dim thprc As Double = SwapFixing(ROUTE_ID, p.YY1, p.MM1, p.YY2, p.MM2, True)
                            nrow.Item(ROUTE_ID.ToString & "_ID") = ROUTE_ID
                            nrow.Item(ROUTE_ID.ToString & "_V") = livedata.VolRecordType
                            nrow.Item(ROUTE_ID.ToString & "_F") = FlashEnum.Normal
                            nrow.Item(ROUTE_ID.ToString & "_H") = thprc
                            nrow.Item(ROUTE_ID.ToString) = tprc
                            nrow.Item(ROUTE_ID.ToString & "_TID") = livedata.TRADE_ID

                            'add this record to my local data list
                            Dim nc As New FFAOptCalcService.VolDataClass
                            nc.FIXING_DATE = livedata.FIXING_DATE
                            nc.FFA_PRICE = livedata.FFA_PRICE
                            nc.HVOL = livedata.HVOL
                            nc.INTEREST_RATE = livedata.INTEREST_RATE
                            nc.IVOL = livedata.IVOL
                            nc.YY1 = livedata.YY1
                            nc.YY2 = livedata.YY2
                            nc.MM1 = livedata.MM1
                            nc.MM2 = livedata.MM2
                            nc.ONLYHISTORICAL = livedata.ONLYHISTORICAL
                            nc.VolRecordType = livedata.VolRecordType
                            nc.PERIOD = livedata.PERIOD
                            nc.ROUTE_ID = livedata.ROUTE_ID
                            nc.SPOT_PRICE = thprc
                            nc.TRADE_ID = livedata.TRADE_ID
                            m_FIXINGS_ADD(nc)
                        Else
                            'it means record cannot be found in data, so we have to extrapolate
                            nrow.Item(ROUTE_ID.ToString & "_ID") = ROUTE_ID
                            nrow.Item(ROUTE_ID.ToString & "_V") = FFAOptCalcService.VolRecordTypeEnum.swap
                            nrow.Item(ROUTE_ID.ToString & "_F") = FlashEnum.Normal
                            nrow.Item(ROUTE_ID.ToString & "_H") = SwapFixing(ROUTE_ID, p.YY1, p.MM1, p.YY2, p.MM2, True)
                            nrow.Item(ROUTE_ID.ToString) = SwapFixing(ROUTE_ID, p.YY1, p.MM1, p.YY2, p.MM2)
                        End If
                    End If
                Else
                    nrow.Item(ROUTE_ID.ToString & "_ID") = ROUTE_ID
                    nrow.Item(ROUTE_ID.ToString & "_V") = FFAOptCalcService.VolRecordTypeEnum.swap
                    nrow.Item(ROUTE_ID.ToString & "_F") = FlashEnum.Normal
                    nrow.Item(ROUTE_ID.ToString & "_H") = 0
                    nrow.Item(ROUTE_ID.ToString) = 0
                End If
            Next
            If nrow.HasErrors = False Then
                m_DATA.Rows.Add(nrow)
            Else
#If DEBUG Then
                Stop
#End If
            End If
        Next
        m_DATA.AcceptChanges()

#If DEBUG Then
        Dim s As String = ""
        For Each c As DataColumn In m_DATA.Columns
            s += c.ColumnName & " | "
        Next
        Debug.Print(s)

        s = ""
        For I = 0 To m_DATA.Rows.Count - 1
            Dim r As DataRow = m_DATA.Rows(I)


            For J = 0 To r.ItemArray.Count - 1
                s += r.Item(J) & " | "
            Next
            Debug.Print(s)
        Next
#End If

        rgv_BS.DataSource = m_DATA
        rgv_MARKET.DataSource = m_DATA
        rgv_MARKET.Refresh()
    End Sub
   
    Public Sub PopulateAll2(Optional ByVal IgnoreLiveData As Boolean = False)        

        Dim rSPOT As GridViewRowInfo = Me.rgv_MARKET.Rows.AddNew
        For Each period In g_MVPeriods
            rSPOT.Cells("PERIOD").Value = "Spot"
            rSPOT.Cells("YY1").Value = 0
            rSPOT.Cells("YY2").Value = 0
            rSPOT.Cells("MM1").Value = 0
            rSPOT.Cells("MM2").Value = 0

            For Each r In TabView.ROUTES
                rSPOT.Cells(r.ROUTE_ID.ToString & "_BID").Value = r.ROUTE_ID
                rSPOT.Cells(r.ROUTE_ID.ToString & "_ASK").Value = r.ROUTE_ID

                Dim Spot = (From q In FIXINGS _
                            Where q.ROUTE_ID = r.ROUTE_ID _
                            And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot) _
                            Order By q.FIXING_DATE Descending Select q).FirstOrDefault
                If IsNothing(Spot) = False Then
                    Dim spotprc As Double = Spot.SPOT_PRICE
                    Dim voltype As FFAOptCalcService.VolRecordTypeEnum = Spot.VolRecordType
                    rSPOT.Cells(r.ROUTE_ID.ToString & "_ID").Value = r.ROUTE_ID
                    rSPOT.Cells(r.ROUTE_ID.ToString & "_V").Value = Spot.VolRecordType
                    rSPOT.Cells(r.ROUTE_ID.ToString & "_F").Value = FlashEnum.Normal
                    If Spot.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot Then
                        Dim oldspot = (From q In FIXINGS _
                                       Where q.ROUTE_ID = r.ROUTE_ID _
                                       And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot _
                                       Order By q.FIXING_DATE Descending Select q).FirstOrDefault
                        If IsNothing(oldspot) = False Then
                            rSPOT.Cells(r.ROUTE_ID.ToString & "_H").Value = oldspot.SPOT_PRICE
                        End If
                    ElseIf Spot.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot Then
                        rSPOT.Cells(r.ROUTE_ID.ToString & "_H").Value = spotprc
                    End If
                    rSPOT.Cells(r.ROUTE_ID.ToString).Value = spotprc
                End If
            Next r
        Next period

        For Each period In g_MVPeriods
            Dim rSWAP As GridViewRowInfo = Me.rgv_MARKET.Rows.AddNew
            rSWAP.Cells("PERIOD").Value = period.Descr
            rSWAP.Cells("YY1").Value = period.YY1
            rSWAP.Cells("MM1").Value = period.MM1
            rSWAP.Cells("YY2").Value = period.YY2
            rSWAP.Cells("MM2").Value = period.MM2

            For Each r In TabView.ROUTES    'Define Forwards
                Dim maxyear = (From q In FIXINGS Where q.ROUTE_ID = r.ROUTE_ID _
                   And q.FIXING_DATE = m_LastFixDate _
                   And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap _
                   Select q.YY2).Max

                If period.YY2 <= maxyear Then
                    Dim ffaprc = (From q In m_FIXINGS _
                                  Where q.ROUTE_ID = r.ROUTE_ID _
                                  And (q.YY1 = period.YY1 And q.MM1 = period.MM1 And q.YY2 = period.YY2 And q.MM2 = period.MM2) _
                                  Select q).FirstOrDefault
                    If IsNothing(ffaprc) = False Then 'we have a live price or old swap fixing
                        rSWAP.Cells(r.ROUTE_ID.ToString & "_ID").Value = r.ROUTE_ID
                        rSWAP.Cells(r.ROUTE_ID.ToString & "_V").Value = ffaprc.VolRecordType
                        rSWAP.Cells(r.ROUTE_ID.ToString & "_F").Value = FlashEnum.Normal
                        rSWAP.Cells(r.ROUTE_ID.ToString & "_H").Value = ffaprc.SPOT_PRICE
                        rSWAP.Cells(r.ROUTE_ID.ToString).Value = ffaprc.FFA_PRICE
                        rSWAP.Cells(r.ROUTE_ID.ToString & "_TID").Value = ffaprc.TRADE_ID
                    Else
                        'first check if live data exists for this non standard period
                        Dim livedata = (From q In FIXINGS Where q.ROUTE_ID = r.ROUTE_ID _
                                        And q.YY1 = period.YY1 And q.MM1 = period.MM1 And q.YY2 = period.YY2 And q.MM2 = period.MM2 _
                                        And q.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA _
                                        And (q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live Or q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level) _
                                        And q.FIXING_DATE > m_LastFixDate _
                                        Order By q.FIXING_DATE Descending _
                                        Select q).FirstOrDefault
                        If IsNothing(livedata) = False And IgnoreLiveData = False Then
                            Dim tprc As Double = livedata.FFA_PRICE
                            Dim thprc As Double = SwapFixing(r.ROUTE_ID, period.YY1, period.MM1, period.YY2, period.MM2, True)
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_ID").Value = r.ROUTE_ID
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_V").Value = livedata.VolRecordType
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_F").Value = FlashEnum.Normal
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_H").Value = thprc
                            rSWAP.Cells(r.ROUTE_ID.ToString).Value = tprc
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_TID").Value = livedata.TRADE_ID

                            'add this record to my local data list
                            Dim nc As New FFAOptCalcService.VolDataClass
                            nc.FIXING_DATE = livedata.FIXING_DATE
                            nc.FFA_PRICE = livedata.FFA_PRICE
                            nc.HVOL = livedata.HVOL
                            nc.INTEREST_RATE = livedata.INTEREST_RATE
                            nc.IVOL = livedata.IVOL
                            nc.YY1 = livedata.YY1
                            nc.YY2 = livedata.YY2
                            nc.MM1 = livedata.MM1
                            nc.MM2 = livedata.MM2
                            nc.ONLYHISTORICAL = livedata.ONLYHISTORICAL
                            nc.VolRecordType = livedata.VolRecordType
                            nc.PERIOD = livedata.PERIOD
                            nc.ROUTE_ID = livedata.ROUTE_ID
                            nc.SPOT_PRICE = thprc
                            nc.TRADE_ID = livedata.TRADE_ID
                            m_FIXINGS_ADD(nc)
                        Else
                            'it means record cannot be found in data, so we have to extrapolate
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_ID").Value = r.ROUTE_ID
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_V").Value = FFAOptCalcService.VolRecordTypeEnum.swap
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_F").Value = FlashEnum.Normal
                            rSWAP.Cells(r.ROUTE_ID.ToString & "_H").Value = SwapFixing(r.ROUTE_ID, period.YY1, period.MM1, period.YY2, period.MM2, True)
                            rSWAP.Cells(r.ROUTE_ID.ToString).Value = SwapFixing(r.ROUTE_ID, period.YY1, period.MM1, period.YY2, period.MM2)
                        End If
                    End If
                Else
                    rSWAP.Cells(r.ROUTE_ID.ToString & "_ID").Value = r.ROUTE_ID
                    rSWAP.Cells(r.ROUTE_ID.ToString & "_V").Value = FFAOptCalcService.VolRecordTypeEnum.swap
                    rSWAP.Cells(r.ROUTE_ID.ToString & "_F").Value = FlashEnum.Normal
                    rSWAP.Cells(r.ROUTE_ID.ToString & "_H").Value = 0
                    rSWAP.Cells(r.ROUTE_ID.ToString).Value = 0
                End If
            Next r            
        Next period

        rgv_MARKET.Refresh()
    End Sub
    Public Sub ConstructDataTable()
        'Create Data Table
        Me.rgv_MARKET.AutoGenerateColumns = False
        rgv_MARKET.DataSource = Nothing
        rgv_MARKET.Rows.Clear()
        rgv_MARKET.Columns.Clear()
        rgv_BS.DataSource = Nothing
        m_DATA = New DataTable

        m_DATA.Columns.Add("PERIOD")
        m_DATA.Columns.Add("YY1", GetType(Integer))
        m_DATA.Columns.Add("MM1", GetType(Integer))
        m_DATA.Columns.Add("YY2", GetType(Integer))
        m_DATA.Columns.Add("MM2", GetType(Integer))

        rgv_MARKET.Columns.Add("PERIOD", "PERIOD", "PERIOD")
        rgv_MARKET.Columns("PERIOD").Width = TabView.WIDTH
        rgv_MARKET.Columns("PERIOD").TextAlignment = ContentAlignment.MiddleLeft
        rgv_MARKET.Columns("PERIOD").HeaderImage = My.Resources.Question_GR16R
        rgv_MARKET.Columns("PERIOD").TextImageRelation = TextImageRelation.TextBeforeImage


        rgv_MARKET.Columns.Add("YY1", "YY1", "YY1") : rgv_MARKET.Columns("YY1").IsVisible = False
        rgv_MARKET.Columns.Add("MM1", "MM1", "MM1") : rgv_MARKET.Columns("MM1").IsVisible = False
        rgv_MARKET.Columns.Add("YY2", "YY2", "YY2") : rgv_MARKET.Columns("YY2").IsVisible = False
        rgv_MARKET.Columns.Add("MM2", "MM2", "MM2") : rgv_MARKET.Columns("MM2").IsVisible = False

        Dim cntr As Integer = 0
        For Each r In TabView.ROUTES
            Dim ROUTE_ID As Integer = r.ROUTE_ID
            Dim route As FFAOptCalcService.SwapDataClass = (From q In ROUTES_DETAIL Where q.ROUTE_ID = ROUTE_ID Select q).FirstOrDefault

            m_DATA.Columns.Add(ROUTE_ID.ToString & "_ID", GetType(Integer))
            rgv_MARKET.Columns.Add(ROUTE_ID.ToString & "_ID", ROUTE_ID.ToString & "_ID", ROUTE_ID.ToString & "_ID")
            rgv_MARKET.Columns(ROUTE_ID.ToString & "_ID").IsVisible = False

            m_DATA.Columns.Add(ROUTE_ID.ToString & "_V", GetType(Integer))
            rgv_MARKET.Columns.Add(ROUTE_ID.ToString & "_V", ROUTE_ID.ToString & "_V", ROUTE_ID.ToString & "_V")
            rgv_MARKET.Columns(ROUTE_ID.ToString & "_V").IsVisible = False

            m_DATA.Columns.Add(ROUTE_ID.ToString & "_F", GetType(Integer))
            rgv_MARKET.Columns.Add(ROUTE_ID.ToString & "_F", ROUTE_ID.ToString & "_F", ROUTE_ID.ToString & "_F")
            rgv_MARKET.Columns(ROUTE_ID.ToString & "_F").IsVisible = False

            m_DATA.Columns.Add(ROUTE_ID.ToString & "_H", GetType(Double))
            rgv_MARKET.Columns.Add(ROUTE_ID.ToString & "_H", ROUTE_ID.ToString & "_H", ROUTE_ID.ToString & "_H")
            rgv_MARKET.Columns(ROUTE_ID.ToString & "_H").IsVisible = False

            m_DATA.Columns.Add(ROUTE_ID.ToString & "_TID", GetType(Integer))
            rgv_MARKET.Columns.Add(ROUTE_ID.ToString & "_TID", ROUTE_ID.ToString & "_TID", ROUTE_ID.ToString & "_TID")
            rgv_MARKET.Columns(ROUTE_ID.ToString & "_TID").IsVisible = False

            m_DATA.Columns.Add(ROUTE_ID.ToString, GetType(Double))
            rgv_MARKET.Columns.Add(ROUTE_ID.ToString, ROUTE_ID.ToString, ROUTE_ID.ToString)
            rgv_MARKET.Columns(ROUTE_ID.ToString).FormatString = route.FORMAT_STRING
            TabView.ROUTES(cntr).FORMAT_STRING = route.FORMAT_STRING
            TabView.ROUTES(cntr).PRICING_TICK = route.PRICING_TICK
            rgv_MARKET.Columns(ROUTE_ID.ToString).Tag = TabView.ROUTES(cntr)
            rgv_MARKET.Columns(ROUTE_ID.ToString).HeaderText = route.ROUTE_SHORT
            rgv_MARKET.Columns(ROUTE_ID.ToString).TextAlignment = ContentAlignment.MiddleCenter
            rgv_MARKET.Columns(ROUTE_ID.ToString).Width = r.WIDTH

            m_DATA.Columns.Add(ROUTE_ID.ToString & "_BID", GetType(Integer))
            Dim cBID As New GridViewDecimalColumn
            cBID.Name = ROUTE_ID.ToString & "_BID"
            cBID.HeaderText = "BID"
            cBID.FieldName = ROUTE_ID.ToString & "_BID"
            cBID.FormatString = route.FORMAT_STRING
            cBID.DecimalPlaces = route.DECIMAL_PLACES
            cBID.ThousandsSeparator = True
            If UD.MYFINGERPRINT.PRICER = True Then
                cBID.Width = r.WIDTH * 1.25
            Else
                cBID.Width = r.WIDTH
            End If
            cBID.TextAlignment = ContentAlignment.MiddleCenter
            cBID.Tag = TabView.ROUTES(cntr)
            cBID.DecimalPlaces = route.DECIMAL_PLACES
            cBID.Step = route.PRICING_TICK
            cBID.Tag = route.ROUTE_ID
            rgv_MARKET.MasterTemplate.Columns.Add(cBID)


            m_DATA.Columns.Add(ROUTE_ID.ToString & "_ASK", GetType(Integer))
            Dim cASK As New GridViewDecimalColumn
            cASK.Name = ROUTE_ID.ToString & "_ASK"
            cASK.HeaderText = "ASK"
            cASK.FieldName = ROUTE_ID.ToString & "_ASK"
            cASK.FormatString = route.FORMAT_STRING
            cASK.DecimalPlaces = route.DECIMAL_PLACES
            cASK.ThousandsSeparator = True
            If UD.MYFINGERPRINT.PRICER = True Then
                cASK.Width = r.WIDTH * 1.25
            Else
                cASK.Width = r.WIDTH
            End If
            cASK.TextAlignment = ContentAlignment.MiddleCenter
            cASK.Tag = TabView.ROUTES(cntr)
            cASK.DecimalPlaces = route.DECIMAL_PLACES
            cASK.Step = route.PRICING_TICK
            cASK.Tag = route.ROUTE_ID
            rgv_MARKET.MasterTemplate.Columns.Add(cASK)

            cntr += 1
        Next

        For I = 0 To rgv_MARKET.Columns.Count - 1
            If rgv_MARKET.Columns(I).Name.Contains("BID") Or rgv_MARKET.Columns(I).Name.Contains("ASK") Then
                If UD.MYFINGERPRINT.PRICER = True Then
                    rgv_MARKET.Columns(I).ReadOnly = False
                Else
                    rgv_MARKET.Columns(I).ReadOnly = True
                End If
                If My.Settings.BIDASK = True Then
                    rgv_MARKET.Columns(I).IsVisible = True
                Else
                    rgv_MARKET.Columns(I).IsVisible = False
                End If
            Else
                rgv_MARKET.Columns(I).ReadOnly = True
            End If
        Next

        rgv_MARKET.Columns("PERIOD").IsPinned = True
        rgv_MARKET.Columns("PERIOD").AllowReorder = False
        rgv_MARKET.BeginEditMode = Telerik.WinControls.RadGridViewBeginEditMode.BeginEditOnKeystrokeOrF2
        rgv_MARKET.Refresh()
    End Sub
    Private Class GridRouteHeaderClassBackUp
        Private m_ROUTE_ID As Integer
        Private m_FORMAT_STRING As String
        Private m_PRICING_TICK As Double
        Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        Public Property FORMAT_STRING As String
            Get
                Return m_FORMAT_STRING
            End Get
            Set(value As String)
                m_FORMAT_STRING = value
            End Set
        End Property
        Public Property PRICING_TICK As Double
            Get
                Return m_PRICING_TICK
            End Get
            Set(value As Double)
                m_PRICING_TICK = value
            End Set
        End Property
        Public ReadOnly Property TOOLTIP_FORMAT_STRING As String
            Get
                Dim fs As String = String.Empty
                If Math.Log10(m_PRICING_TICK) >= 0 Then
                    fs = "+ #,##0;- #,##0"
                ElseIf Math.Log10(m_PRICING_TICK) = -1 Then
                    fs = "+ #,##0.0;- #,##0.0"
                ElseIf Math.Log10(m_PRICING_TICK) = -2 Then
                    fs = "+ #,##0.00;- #,##0.00"
                ElseIf Math.Log10(m_PRICING_TICK) = -3 Then
                    fs = "+ #,##0.000;- #,##0.000"
                ElseIf Math.Log10(m_PRICING_TICK) > -2 And Math.Log10(m_PRICING_TICK) < -1 Then
                    fs = "+ #,##0.00;- #,##0.00"
                ElseIf Math.Log10(m_PRICING_TICK) > -1 And Math.Log10(m_PRICING_TICK) < 0 Then
                    fs = "+ #,##0.00;- #,##0.00"
                End If
                Return fs
            End Get
        End Property
        Public ReadOnly Property PERCENT_FORMAT_STRING As String
            Get
                Return "+ 0.00%;- 0.00%"
            End Get
        End Property
    End Class
#End Region

#Region "MenuItems"
    Private Sub BuildFonts()
        For I = -1 To 4
            Dim FontN As Font = New Font(rgv_MARKET.TableElement.Font.Name, rgv_MARKET.TableElement.Font.Size + I, FontStyle.Regular)
            m_FontsR.Add(FontN)
            Dim FontB As Font = New Font(rgv_MARKET.TableElement.Font.Name, rgv_MARKET.TableElement.Font.Size + I, FontStyle.Bold)
            m_FontsB.Add(FontB)
        Next
    End Sub

    Private Sub rgv_MARKET_ContextMenuOpening(sender As Object, e As ContextMenuOpeningEventArgs) Handles rgv_MARKET.ContextMenuOpening
        If TypeOf e.ContextMenuProvider Is GridHeaderCellElement Then
            Dim cell As GridHeaderCellElement = TryCast(e.ContextMenuProvider, GridHeaderCellElement)
            If cell Is Nothing Then Exit Sub
            e.ContextMenu = Nothing
            If cell.ColumnInfo.FieldName.Contains("_BID") Or cell.ColumnInfo.FieldName.Contains("_ASK") Then
                Dim qry = (From q In BIDASK Where q.PRICE_STATUS = FFASuitePL.PriceStatusEnum.Level Select q).ToList
                If qry.Count > 0 Then
                    e.ContextMenu = m_BIDASKContextMenu.DropDown
                Else
                    e.ContextMenu = Nothing
                End If
                Exit Sub
            End If

            m_HeaderContextMenu.Items(1).Tag = cell.ColumnInfo.Name.ToString
            e.ContextMenu = m_HeaderContextMenu.DropDown
            If cell.ColumnInfo.Index = 0 Then
                m_HeaderContextMenu.Items(1).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            Else
                m_HeaderContextMenu.Items(1).Visibility = Telerik.WinControls.ElementVisibility.Visible
            End If

            If My.Settings.BIDASK = True Then
                m_HeaderContextMenu.Items("HideBidAsk").Text = "Hide Bid/Ask Columns"
            ElseIf My.Settings.BIDASK = False Then
                m_HeaderContextMenu.Items("HideBidAsk").Text = "Show Bid/Ask Columns"
            End If

        ElseIf TypeOf e.ContextMenuProvider Is GridCellElement Then
            Dim cell As GridCellElement = TryCast(e.ContextMenuProvider, GridCellElement)
            If cell Is Nothing Then Exit Sub
            e.ContextMenu = Nothing
            If cell.ColumnInfo.IsVisible = False Then Exit Sub
            If cell.ColumnInfo.FieldName = "PERIOD" Then Exit Sub
            If cell.ColumnInfo.FieldName.Contains("_BID") Or cell.ColumnInfo.FieldName.Contains("_ASK") Then Exit Sub
            If cell.Text = "" Then Exit Sub

            While m_GridContextMenu.Items.Count > 6
                m_GridContextMenu.Items.RemoveAt(m_GridContextMenu.Items.Count - 1)
            End While

            Dim nc As New FFAOptCalcService.VolDataClass
            nc.ROUTE_ID = CInt(cell.ColumnInfo.FieldName)
            nc.YY1 = CInt(cell.RowInfo.Cells("YY1").Value)
            nc.MM1 = CInt(cell.RowInfo.Cells("MM1").Value)
            nc.YY2 = CInt(cell.RowInfo.Cells("YY2").Value)
            nc.MM2 = CInt(cell.RowInfo.Cells("MM2").Value)
            nc.PERIOD = cell.ColumnInfo.HeaderText & ": " & cell.RowInfo.Cells("PERIOD").Value
            nc.FFA_PRICE = cell.Value
            If cell.RowIndex = 0 Then
                nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot
            Else
                nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap
            End If
            nc.HVOL = MousePosition.X
            nc.IVOL = MousePosition.Y
            For Each r In m_GridContextMenu.Items
                r.Tag = nc
            Next

            'add latest trades
            Dim livetrades = (From q In FIXINGS Where q.ROUTE_ID = CInt(cell.ColumnInfo.FieldName) _
                              And q.YY1 = CInt(cell.RowInfo.Cells("YY1").Value) _
                              And q.MM1 = CInt(cell.RowInfo.Cells("MM1").Value) _
                              And q.YY2 = CInt(cell.RowInfo.Cells("YY2").Value) _
                              And q.MM2 = CInt(cell.RowInfo.Cells("MM2").Value) _
                              And q.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live _
                              Order By q.FIXING_DATE Descending _
                              Select q).Take(10).ToList
            If livetrades.Count > 0 Then
                m_GridContextMenu.Items.Add(New RadMenuSeparatorItem)
                Dim l As New RadMenuHeaderItem
                l.Text = "Last trades:"
                m_GridContextMenu.Items.Add(l)
                m_GridContextMenu.Items.Add(New RadMenuSeparatorItem)
                For Each r In livetrades
                    Dim grhc As DataContracts.RouteViewClass = TryCast(cell.ColumnInfo.Tag, DataContracts.RouteViewClass)
                    If UD.MYFINGERPRINT.PRICER = True Then
                        Dim h As New RadMenuItem
                        h.Text = String.Format(grhc.FORMAT_STRING, r.FFA_PRICE) & "  |  " & FormatDateTime(r.FIXING_DATE.ToLocalTime, DateFormat.ShortTime)
                        h.DisplayStyle = Telerik.WinControls.DisplayStyle.Text
                        If r.DESK_TRADER_ID = 1 Then
                            Dim ammendedtrade As FFAOptCalcService.VolDataClass = r
                            ammendedtrade.HVOL = MousePosition.X
                            ammendedtrade.IVOL = MousePosition.Y
                            h.Tag = ammendedtrade
                            h.HintText = "Click to edit"
                            AddHandler h.Click, AddressOf EditFaultTradeEntry
                        Else
                            h.Tag = 0
                        End If
                        m_GridContextMenu.Items.Add(h)
                    Else
                        Dim h As New RadMenuHeaderItem
                        h.Text = String.Format(grhc.FORMAT_STRING, r.FFA_PRICE) & "  |  " & FormatDateTime(r.FIXING_DATE.ToLocalTime, DateFormat.ShortTime)
                        h.Tag = r.TRADE_ID
                        m_GridContextMenu.Items.Add(h)
                    End If
                Next
            End If

            e.ContextMenu = m_GridContextMenu.DropDown
        End If
    End Sub
    Public Sub BuildContextMenu()

        '========================================================================
        'BIDASK Menu
        '========================================================================
        m_BIDASKContextMenu = New RadContextMenu
        Dim BroadCast As New RadMenuItem
        BroadCast.Text = "Publish Bid/Ask Updates"
        BroadCast.Image = My.Resources.Currency_Dollar_B16R
        BroadCast.TextImageRelation = TextImageRelation.ImageBeforeText
        m_BIDASKContextMenu.Items.Add(BroadCast)
        AddHandler broadcast.Click, AddressOf rmi_BroadCastClick

        '========================================================================
        'GRID Menu
        '========================================================================
        m_GridContextMenu = New RadContextMenu
        Dim Graph As New RadMenuItem
        Graph.Text = "Graph Contract"
        Graph.Image = My.Resources.Stats2_B16R
        Graph.TextImageRelation = TextImageRelation.ImageBeforeText
        m_GridContextMenu.Items.Add(Graph)
        AddHandler Graph.Click, AddressOf rmi_GRAPH_Click

        m_GridContextMenu.Items.Add(New RadMenuSeparatorItem)
        Dim OptCalc As New RadMenuItem
        OptCalc.Text = "Option Calculator"
        OptCalc.Image = My.Resources.Calc_B16R
        OptCalc.TextImageRelation = TextImageRelation.ImageBeforeText
        m_GridContextMenu.Items.Add(OptCalc)
        AddHandler OptCalc.Click, AddressOf rmi_OPTION_CALCULATOR_Click

        Dim TCOptCalc As New RadMenuItem
        TCOptCalc.Text = "Time Charter Calculator"
        TCOptCalc.Image = My.Resources.Wizard_B16R
        TCOptCalc.TextImageRelation = TextImageRelation.ImageBeforeText
        m_GridContextMenu.Items.Add(TCOptCalc)
        AddHandler TCOptCalc.Click, AddressOf rmi_TC_OPTION_CALCULATOR_Click

        Dim hsep As New RadMenuSeparatorItem
        m_GridContextMenu.Items.Add(hsep)
        Dim Price As New RadMenuItem
        Price.Text = "Trade Announce"
        Price.Image = My.Resources.Currency_Dollar_B16R
        Price.TextImageRelation = TextImageRelation.ImageBeforeText
        m_GridContextMenu.Items.Add(Price)
        AddHandler Price.Click, AddressOf rmi_PRICE_Click
        If UD.MYFINGERPRINT.PRICER = False Then
            Price.Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            hsep.Visibility = Telerik.WinControls.ElementVisibility.Collapsed
        End If

        '========================================================================
        'Header Menu
        '========================================================================
        m_HeaderContextMenu = New RadContextMenu
        Dim Add As New RadMenuItem
        Add.Image = My.Resources.Plus_B16R
        Add.TextImageRelation = TextImageRelation.ImageBeforeText
        Add.Text = "Add Contract"
        m_HeaderContextMenu.Items.Add(Add)

        Dim Del As New RadMenuItem
        Del.Image = My.Resources.Minus_B16R
        Del.TextImageRelation = TextImageRelation.ImageBeforeText
        Del.Text = "Remove Contract"
        m_HeaderContextMenu.Items.Add(Del)
        AddHandler Del.Click, AddressOf rmi_Delete_Column_Click

        m_HeaderContextMenu.Items.Add(New RadMenuSeparatorItem)
        Dim IncrFont As New RadMenuItem
        IncrFont.Text = "Increase Font Size"
        IncrFont.Image = My.Resources.Text_Large_B16R
        IncrFont.TextImageRelation = TextImageRelation.ImageBeforeText
        m_HeaderContextMenu.Items.Add(IncrFont)
        AddHandler IncrFont.Click, AddressOf rmi_IncrFont_Click

        Dim DecrFont As New RadMenuItem
        DecrFont.Text = "Decrease Font Size"
        DecrFont.Image = My.Resources.Text_Small_B16R
        DecrFont.TextImageRelation = TextImageRelation.ImageBeforeText
        m_HeaderContextMenu.Items.Add(DecrFont)
        AddHandler DecrFont.Click, AddressOf rmi_DecrFont_Click

        m_HeaderContextMenu.Items.Add(New RadMenuSeparatorItem)
        Dim FormRename As New RadMenuItem
        FormRename.Text = "Rename Form"
        FormRename.Image = My.Resources.Table_B16R
        FormRename.TextImageRelation = TextImageRelation.ImageBeforeText
        m_HeaderContextMenu.Items.Add(FormRename)
        AddHandler FormRename.Click, AddressOf rmi_FormRename_Click

        Dim FormAdd As New RadMenuItem
        FormAdd.Text = "Add New Form"
        FormAdd.Image = My.Resources.Table_B16R
        FormAdd.TextImageRelation = TextImageRelation.ImageBeforeText
        m_HeaderContextMenu.Items.Add(FormAdd)
        AddHandler FormAdd.Click, AddressOf rmi_FormAdd_Click

        m_HeaderContextMenu.Items.Add(New RadMenuSeparatorItem)
        Dim FormRemove As New RadMenuItem
        FormRemove.Text = "Remove Form"
        FormRemove.Image = My.Resources.Table_B16R
        FormRemove.TextImageRelation = TextImageRelation.ImageBeforeText
        m_HeaderContextMenu.Items.Add(FormRemove)
        AddHandler FormRemove.Click, AddressOf rmi_FormRemove_Click

        m_HeaderContextMenu.Items.Add(New RadMenuSeparatorItem)
        Dim HideBidAsk As New RadMenuItem
        HideBidAsk.Name = "HideBidAsk"
        HideBidAsk.Text = "Hide/Show Bid/Ask Columns"
        HideBidAsk.Image = My.Resources.Tool_B16R
        HideBidAsk.TextImageRelation = TextImageRelation.ImageBeforeText
        m_HeaderContextMenu.Items.Add(HideBidAsk)
        AddHandler HideBidAsk.Click, AddressOf rmi_HideBidAskClick

        Dim TC = (From q In TRADE_CLASS Where My.Settings.TradeClassesSupported.Contains(q.TRADE_CLASS_SHORT) Select q).ToList
        Dim viewroutelist As New List(Of Integer)
        For Each r In TabView.ROUTES
            viewroutelist.Add(r.ROUTE_ID)
        Next
        For Each t In TC
            Dim rmi_CLASS_TAB = New Telerik.WinControls.UI.RadMenuItem
            rmi_CLASS_TAB.Text = t.TRADE_CLASS
            rmi_CLASS_TAB.Tag = t.TRADE_CLASS_SHORT
            Dim tcount As Integer = Add.Items.Add(rmi_CLASS_TAB)
            Dim trmi As RadMenuItem = Add.Items(tcount)
            Dim VC = (From q In VESSEL_CLASSES Where q.DRYWET = t.TRADE_CLASS_SHORT _
                      Order By q.VESSEL_CLASSMember Select q).ToList
            For Each v In VC
                Dim rmi_VESSEL_TAB = New Telerik.WinControls.UI.RadMenuItem
                rmi_VESSEL_TAB.Text = v.VESSEL_CLASSMember
                rmi_VESSEL_TAB.Tag = v.VESSEL_CLASS_ID
                Dim vcount As Integer = trmi.Items.Add(rmi_VESSEL_TAB)
                Dim vrmi As RadMenuItem = trmi.Items(vcount)
                Dim RT = (From q In ROUTES Where q.VESSEL_CLASS_ID = v.VESSEL_CLASS_ID _
                          And q.FFA_TRADED = True _
                          And viewroutelist.Contains(q.ROUTE_ID) = False _
                          Order By q.ROUTE_SHORT Select q).ToList
                For Each r In RT
                    Dim rmi_ROUTE_TAB = New Telerik.WinControls.UI.RadMenuItem
                    rmi_ROUTE_TAB.Text = r.ROUTE_SHORT
                    rmi_ROUTE_TAB.Tag = r.ROUTE_ID
                    vrmi.Items.Add(rmi_ROUTE_TAB)
                    AddHandler rmi_ROUTE_TAB.Click, AddressOf rmi_ROUTE_TAB_Click
                Next
            Next
        Next
    End Sub
    Private Sub EditFaultTradeEntry(sender As Object, e As EventArgs)
        Dim h As RadMenuItem = TryCast(sender, RadMenuItem)
        If IsNothing(h) = False Then
            Dim trade As FFAOptCalcService.VolDataClass = TryCast(h.Tag, FFAOptCalcService.VolDataClass)
            If trade.TRADE_ID > 0 Then
                Dim f As New AddTradeForm
                f.Text = "Ammend Reported Trade"
                Dim rdtl = (From q In ROUTES_DETAIL Where q.ROUTE_ID = trade.ROUTE_ID Select q).FirstOrDefault
                f.rgb_CONTRACT.HeaderText = rdtl.ROUTE_SHORT & ": " & New ArtBTimePeriod(trade.YY1, trade.MM1, trade.YY2, trade.MM2).Descr
                f.se_PRICE.DecimalPlaces = rdtl.DECIMAL_PLACES
                f.se_PRICE.Value = trade.FFA_PRICE
                f.se_PRICE.Increment = rdtl.PRICING_TICK
                f.se_PRICE.Minimum = trade.FFA_PRICE * 0.7
                f.se_PRICE.Maximum = trade.FFA_PRICE * 1.3
                f.OldTrade = trade
                f.FormMode = AddTradeForm.FormModeEnum.EditTrade
                f.myXMPP = EventForm.myXMPP
                f.myWS = EventForm.myWS
                f.DesktopLocation = New Point(trade.HVOL, trade.IVOL)
                f.Show(Me.EventForm)
            End If
        End If
    End Sub

    Private Sub rmi_HideBidAskClick(sender As Object, e As EventArgs)
        rgv_MARKET.SuspendLayout()
        Dim qry = From q In rgv_MARKET.Columns Select q
        For Each r In qry
            If r.Name.Contains("BID") Or r.Name.Contains("ASK") Then
                If My.Settings.BIDASK = True Then
                    r.IsVisible = False
                ElseIf My.Settings.BIDASK = False Then
                    r.IsVisible = True
                End If
            End If
        Next
        If My.Settings.BIDASK = True Then
            My.Settings.BIDASK = False
        ElseIf My.Settings.BIDASK = False Then
            My.Settings.BIDASK = True
        End If
        My.Settings.Save()
        rgv_MARKET.ResumeLayout()        
        rgv_MARKET.Refresh()
    End Sub

    Private Sub rmi_BroadCastClick(sender As Object, e As EventArgs)
        Dim rmi As RadMenuItem = TryCast(sender, Telerik.WinControls.UI.RadMenuItem)
        Dim qry = (From q In BIDASK Where q.PRICE_STATUS = FFASuitePL.PriceStatusEnum.Level Select q).ToList

        If qry.Count = 0 Then
            Exit Sub
        End If

        Select Case g_msgServiceType
            Case MsgConnectivityServiceEnum.WS               
                Dim p As New PublishArgs(My.Settings.ws_TradesChannel, Json.Serialize(Of List(Of FFASuitePL.WP8FFAData))(qry), CStr(FFAOptCalcService.MessageEnum.MarketViewUpdate)) With _
                            {.OnFailure = Sub(er As PublishFailureArgs)
                                              MsgError(Me, "BID/ASK Update failed to be sent to server." & Environment.NewLine & "Error: " & er.ErrorMessage, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                                          End Sub, _
                             .OnSuccess = Sub(sx As PublishSuccessArgs)
                                              BIDASK.Clear()
                                          End Sub}
                Try
                    EventForm.myWS.Publish(p)
                Catch ex As Exception
                    MsgError(Me, "BID/ASK Update failed to be sent to server.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                End Try
            Case MsgConnectivityServiceEnum.BOSH, MsgConnectivityServiceEnum.TCP
                Dim msg As New agsXMPP.protocol.client.Message
                Dim m_Jid As New Jid("traders", "broadcast." & g_XMPPServerString, "FFA Opt Calc")
                msg.Subject = ArtB_Class_Library.ArtBMessages.BidAskUpdate
                msg.Body = Json.Serialize(Of List(Of FFASuitePL.WP8FFAData))(qry)
                msg.To = m_Jid
                Try
                    EventForm.myXMPP.Send(msg)
                    BIDASK.Clear()
                Catch ex As Exception
                    MsgError(Me, "BID/ASK Update failed to be sent to server.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                End Try
        End Select
    End Sub

    Private Sub rmi_GRAPH_Click(sender As Object, e As EventArgs)
        Dim rmi As RadMenuItem = TryCast(sender, Telerik.WinControls.UI.RadMenuItem)
        Dim nc As FFAOptCalcService.VolDataClass = TryCast(rmi.Tag, FFAOptCalcService.VolDataClass)

        Dim f As New GraphContract
        f.Contract = nc
        f.DesktopLocation = New Point(nc.HVOL, nc.IVOL)
        f.Show()
    End Sub
    Private Sub rmi_OPTION_CALCULATOR_Click(sender As Object, e As EventArgs)
        Dim rmi As RadMenuItem = TryCast(sender, Telerik.WinControls.UI.RadMenuItem)
        Dim nc As FFAOptCalcService.VolDataClass = TryCast(rmi.Tag, FFAOptCalcService.VolDataClass)

        Dim f As New ArtBOptCalcControlForm
        f.ROUTE_DETAIL = (From q In ROUTES_DETAIL Where q.ROUTE_ID = nc.ROUTE_ID Select q).First
        f.Text = f.ROUTE_DETAIL.ROUTE_SHORT & " Option Calculator"
        f.rgb_Swap_Periods.Text = f.ROUTE_DETAIL.ROUTE_SHORT & " Swap Periods"
        f.EventForm = EventForm
        f.ROUTE_ID = f.ROUTE_DETAIL.ROUTE_ID
        f.Prepare()
        f.Show()
    End Sub
    Private Sub rmi_TC_OPTION_CALCULATOR_Click(sender As Object, e As EventArgs)
        Dim rmi As RadMenuItem = TryCast(sender, Telerik.WinControls.UI.RadMenuItem)
        Dim nc As FFAOptCalcService.VolDataClass = TryCast(rmi.Tag, FFAOptCalcService.VolDataClass)

        Dim f As New FFATimeCharterOptionForm
        f.ROUTE_DETAIL = (From q In ROUTES_DETAIL Where q.ROUTE_ID = nc.ROUTE_ID Select q).First
        f.Text = f.ROUTE_DETAIL.ROUTE_SHORT & " Time Charter Calculator"
        f.rgb_Swap_Periods.Text = f.ROUTE_DETAIL.ROUTE_SHORT & " Swap Periods"
        f.EventForm = EventForm
        f.ROUTE_ID = f.ROUTE_DETAIL.ROUTE_ID
        f.INTEREST_RATES = (From q In INTEREST_RATES Where q.CCY_ID = f.ROUTE_DETAIL.CCY_ID Order By q.PERIOD).ToList
        f.PUBLIC_HOLIDAYS = PUBLIC_HOLIDAYS
        f.SERVER_DATE = SERVER_DATE
        f.Prepare((From q In FIXINGS Where q.ROUTE_ID = f.ROUTE_DETAIL.ROUTE_ID Select q).ToList)
        f.Show()
    End Sub

    Private Sub rmi_PRICE_Click(sender As Object, e As EventArgs)
        Dim rmi As RadMenuItem = TryCast(sender, Telerik.WinControls.UI.RadMenuItem)
        Dim nc As FFAOptCalcService.VolDataClass = TryCast(rmi.Tag, FFAOptCalcService.VolDataClass)
        If IsNothing(nc) Then Exit Sub
        If nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.spot Then Exit Sub

        Dim f As New AddTradeForm
        f.rgb_CONTRACT.HeaderText = nc.PERIOD

        Dim rdtl = (From q In ROUTES_DETAIL Where q.ROUTE_ID = nc.ROUTE_ID Select q).FirstOrDefault
        f.se_PRICE.DecimalPlaces = rdtl.DECIMAL_PLACES
        f.se_PRICE.Value = MRound(nc.FFA_PRICE, rdtl.PRICING_TICK)
        f.se_PRICE.Increment = rdtl.PRICING_TICK
        f.se_PRICE.Minimum = nc.FFA_PRICE * 0.7
        f.se_PRICE.Maximum = nc.FFA_PRICE * 1.3
        f.OldTrade = nc
        f.myXMPP = EventForm.myXMPP
        f.myWS = EventForm.myWS
        f.DesktopLocation = New Point(nc.HVOL, nc.IVOL)
        f.Show(Me.EventForm)
    End Sub
    Private Sub rmi_ROUTE_TAB_Click(sender As Object, e As EventArgs)
        Dim ROUTE_ID As String = TryCast(sender, Telerik.WinControls.UI.RadMenuItem).Tag.ToString

        Me.Cursor = Cursors.WaitCursor
        Me.rgv_MARKET.Cursor = Cursors.WaitCursor
        If EventForm.rwb_WAIT.IsWaiting = False Then
            EventForm.rwb_WAIT.StartWaiting()
        End If
        Dim bw As New System.ComponentModel.BackgroundWorker
        AddHandler bw.DoWork, AddressOf bw_OptCalc_DoWork
        AddHandler bw.RunWorkerCompleted, AddressOf bw_OptCalc_RunWorkerCompleted
        bw.RunWorkerAsync(ROUTE_ID)
    End Sub
    Private Sub rmi_Delete_Column_Click(sender As Object, e As EventArgs)
        Dim ROUTE_ID As String = TryCast(sender, Telerik.WinControls.UI.RadMenuItem).Tag.ToString
        Dim cntr As Integer
        For Each r In TabView.ROUTES
            If r.ROUTE_ID = ROUTE_ID Then
                Exit For
            End If
            cntr += 1
        Next
        TabView.ROUTES.RemoveAt(cntr)
        rgv_MARKET.SuspendUpdate()
        ConstructDataTable()
        FetchLocalData()
        PopulateAll()
        BuildContextMenu()
        rgv_MARKET.ResumeUpdate()
        rgv_MARKET.Refresh()
    End Sub
    Private Sub rmi_IncrFont_Click(sender As Object, e As EventArgs)
        If TabView.FontSize < 4 Then
            TabView.FontSize += 1
        Else
            Beep()
            Exit Sub
        End If
        m_FontR = m_FontsR(TabView.FontSize)
        m_FontB = m_FontsB(TabView.FontSize)
        rgv_MARKET.TableElement.Update(GridUINotifyAction.StateChanged)
    End Sub
    Private Sub rmi_DecrFont_Click(sender As Object, e As EventArgs)
        If TabView.FontSize > -1 Then
            TabView.FontSize -= 1
        Else
            Beep()
            Exit Sub
        End If
        m_FontR = m_FontsR(TabView.FontSize)
        m_FontB = m_FontsB(TabView.FontSize)
        rgv_MARKET.TableElement.Update(GridUINotifyAction.StateChanged)
    End Sub
    Private Sub rmi_FormAdd_Click(sender As Object, e As EventArgs)
        Dim v As New DataContracts.MarketTabClass
        v.FontSize = TabView.FontSize
        v.WIDTH = My.Settings.MarketViewPeriodWidth
        v.NAME = "New Form"
        v.ROUTES = New List(Of DataContracts.RouteViewClass)
        g_MarketViews.VIEWS.Add(v)
        g_MarketViews.Save()

        Dim f As New MarketWatch
        f.TabView = v
        f.Text = v.NAME
        f.EventForm = Me.EventForm
        f.MdiParent = Me.EventForm
        f.Show()

        Dim tab As Telerik.WinControls.UI.TabStripItem = Me.EventForm.rd_MAIN.DockWindows(Me.EventForm.rd_MAIN.DockWindows.Count - 1).TabStripItem
        tab.Image = My.Resources.Table_B16R

        Dim tabstrip As Telerik.WinControls.UI.Docking.DocumentTabStrip = TryCast(Me.EventForm.rd_MAIN.DockWindows(Me.EventForm.rd_MAIN.DockWindows.Count - 1).Parent, Telerik.WinControls.UI.Docking.DocumentTabStrip)
        tabstrip.TabStripElement.ContentArea.BackColor = Me.EventForm.rd_MAIN.BackColor
    End Sub
    Private Sub rmi_FormRemove_Click(sender As Object, e As EventArgs)
        Dim answ = MsgError(Me, "Are you sure you want to permanently delete this form?", "Delete Form?", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
        If answ = Windows.Forms.DialogResult.OK Then
            g_MarketViews.VIEWS.Remove(TabView)
            g_MarketViews.Save()
            Me.Close()
        End If
    End Sub
    Private Sub rmi_FormRename_Click(sender As Object, e As EventArgs)

        Dim f As New MarketRename
        f.FormName = Me.Text
        Dim dockw As Telerik.WinControls.UI.Docking.DockWindow = Me.Parent
        f.StartPosition = FormStartPosition.Manual
        f.Location = New Point(dockw.ParentForm.Location.X, dockw.ParentForm.Location.Y)
        Dim answ As DialogResult = f.ShowDialog(dockw)
        If answ = Windows.Forms.DialogResult.OK Then
            Me.Text = f.FormName
            TabView.NAME = f.FormName
            If dockw.DockState = Docking.DockState.Floating Then
                dockw.FloatingParent.Text = f.FormName
            End If
        End If
        f.Dispose()
    End Sub    
#End Region

#Region "SWAPForwardPrice"
    Public Function SwapFixing(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer, Optional ByVal Historical As Boolean = False) As Double
        Dim SnapShot = (From q In m_FIXINGS _
                        Where q.ROUTE_ID = f_ROUTE_ID _
                        Order By q.YY2, q.MM2, q.YY1, q.MM1 Descending _
                        Select q).ToList
        Dim tc As New Collection
        For Each r In SnapShot
            Dim nrf As New ForwardsClass
            nrf.CMSROUTE_ID = ""
            If Historical = True Then
                nrf.FIXING = r.SPOT_PRICE
            Else
                nrf.FIXING = r.FFA_PRICE
            End If
            nrf.FIXING_DATE = r.FIXING_DATE
            nrf.YY1 = r.YY1
            nrf.YY2 = r.YY2
            nrf.MM1 = r.MM1
            nrf.MM2 = r.MM2
            nrf.PERIOD = DateAndTime.DateDiff(DateInterval.Month, m_LastFixDate, DateSerial(r.YY2, r.MM2, Date.DaysInMonth(r.YY2, r.MM2)))
            nrf.REPORTDESC = r.PERIOD
            nrf.ROUTE_ID = r.ROUTE_ID
            nrf.KEY = Format(nrf.FIXING_DATE, "yyyMMdd") & Format(nrf.YY1, "0000") & Format(nrf.MM1, "00") & Format(nrf.YY2, "00") & Format(nrf.MM2, "00")
            If tc.Contains(nrf.KEY) = False Then
                tc.Add(nrf, nrf.KEY)
            End If
        Next

        tc = NormalizeData(tc)
        Return ForwardRate(tc, m_LastFixDate, f_YY1, f_MM1, f_YY2, f_MM2)
    End Function
    Private Function NormalizeData(ByVal tc As Collection) As Collection
        'normalize data, covert from forward-forward to preriod t/c
        Dim qr1 = From q As ForwardsClass In tc _
                  Order By q.YY2 Descending, q.MM2 Descending, q.YY1 Descending, q.MM1 Descending _
                  Select q
        For Each mr In qr1
            Dim tv As Double = 0
            Dim sd As Date = DateSerial(mr.YY1, mr.MM1, 1)
            Dim ed As Date = DateSerial(mr.YY2, mr.MM2, Date.DaysInMonth(mr.YY2, mr.MM2))
            Dim nm As Integer = DateAndTime.DateDiff(DateInterval.Month, sd, ed) + 1
            Dim monthsrem As Integer = DateAndTime.DateDiff(DateInterval.Month, mr.FIXING_DATE, sd)

            tv = tv + mr.FIXING * nm

            Dim cntr As Integer
            While monthsrem > 0 And cntr < 1000
                Dim qr2 = (From q As ForwardsClass In tc _
                           Where DateAndTime.DateDiff(DateInterval.Month, DateSerial(q.YY2, q.MM2, 1), sd) = 1 _
                           Order By q.YY1, q.MM1 _
                           Select q).FirstOrDefault
                If IsNothing(qr2) = False Then
                    sd = DateSerial(qr2.YY1, qr2.MM1, 1)
                    ed = DateSerial(qr2.YY2, qr2.MM2, Date.DaysInMonth(qr2.YY2, qr2.MM2))
                    nm = DateAndTime.DateDiff(DateInterval.Month, sd, ed) + 1
                    tv = tv + qr2.FIXING * nm
                    monthsrem = DateAndTime.DateDiff(DateInterval.Month, mr.FIXING_DATE, sd)
                Else
                    monthsrem = 0
                End If
                cntr += 1
            End While
            mr.NORMFIX = tv / (mr.PERIOD + 1)
        Next

        Dim tempc As New Collection
        For Each r As ForwardsClass In tc
            tempc.Add(r, r.KEY)
        Next

        For I = 1 To tempc.Count - 1
            Dim c1 As ForwardsClass = tempc.Item(I)
            Dim c2 As ForwardsClass = tempc.Item(I + 1)
            If c1.PERIOD = c2.PERIOD Then
                tc.Remove(c1.KEY)
            End If
        Next

        NormalizeData = tc
    End Function
    Private Function ForwardRate(ByVal FWDSCOL As Collection, ByVal FixDate As Date, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As Double
        Dim tc As New Collection
        For Each r In FWDSCOL
            Dim nr As New ForwardsClass
            nr.CMSROUTE_ID = r.CMSROUTE_ID
            nr.FIXING = r.FIXING
            nr.NORMFIX = r.NORMFIX
            nr.FIXING_DATE = r.FIXING_DATE
            nr.MM1 = r.MM1
            nr.MM2 = r.MM2
            nr.PERIOD = r.PERIOD
            nr.REPORTDESC = r.REPORTDESC
            nr.ROUTE_ID = r.ROUTE_ID
            nr.YY1 = r.YY1
            nr.YY2 = r.YY2
            nr.KEY = r.KEY
            tc.Add(nr, nr.KEY)
        Next
        ForwardRate = PerformSpline(FixDate, tc, YY1, MM1, YY2, MM2)
    End Function
    Private Function PerformSpline(ByVal FixDate As Date, ByVal ROUTE_m_FIXINGS As Collection, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As Double
        Dim retval As Double
        Dim n As Integer = ROUTE_m_FIXINGS.Count - 1
        Dim f(n) As Double
        Dim x(n) As Double
        Dim a(n) As Double
        Dim l(n) As Double
        Dim m(n) As Double
        Dim z(n) As Double
        Dim c(n) As Double
        Dim b(n) As Double
        Dim d(n) As Double

        Dim i As Integer = 0
        Dim j As Integer = 0

        Dim OrderedData = From q As ForwardsClass In ROUTE_m_FIXINGS _
                          Order By q.PERIOD _
                          Select q

        For Each r As ForwardsClass In OrderedData
            x(i) = r.PERIOD
            f(i) = r.NORMFIX
            i = i + 1
        Next

        For i = 1 To n - 1
            a(i) = f(i + 1) * (x(i) - x(i - 1))
            a(i) = a(i) - f(i) * (x(i + 1) - x(i - 1))
            a(i) = a(i) + f(i - 1) * (x(i + 1) - x(i))
            a(i) = 3 * a(i)
            a(i) = a(i) / ((x(i + 1) - x(i)) * (x(i) - x(i - 1)))
        Next

        l(0) = 1
        m(0) = 0
        z(0) = 0

        For i = 1 To n - 1
            l(i) = 2 * (x(i + 1) - x(i - 1))
            l(i) = l(i) - (x(i) - x(i - 1)) * m(i - 1)

            m(i) = (x(i + 1) - x(i)) / l(i)
            z(i) = (a(i) - (x(i) - x(i - 1)) * z(i - 1)) / l(i)
        Next

        l(n) = 1
        z(n) = 0
        c(n) = z(n)

        For j = n - 1 To 0 Step -1
            c(j) = z(j) - m(j) * c(j + 1)

            b(j) = (f(j + 1) - f(j)) / (x(j + 1) - x(j))
            b(j) = b(j) - (x(j + 1) - x(j)) * (c(j + 1) + 2 * c(j)) / 3

            d(j) = (c(j + 1) - c(j)) / (3 * (x(j + 1) - x(j)))
        Next
        'finished cubic spline interpolation

        Dim StartPeriod As Integer = DateAndTime.DateDiff(DateInterval.Month, FixDate, DateSerial(YY1, MM1, 1)) - 1
        Dim EndPeriod As Integer = DateAndTime.DateDiff(DateInterval.Month, FixDate, DateSerial(YY2, MM2, 1))
        Dim SSP As Double
        Dim SEP As Double

        'for Start period
        If StartPeriod = x(n) Then
            SSP = f(n)
        ElseIf StartPeriod < x(n) Then
            For i = 0 To n - 1
                If StartPeriod >= x(i) And StartPeriod <= x(i + 1) Then
                    SSP = f(i) + b(i) * (StartPeriod - x(i)) + c(i) * (StartPeriod - x(i)) ^ 2 + d(i) * (StartPeriod - x(i)) ^ 3
                    Exit For
                End If
            Next
        End If

        'for end period
        If EndPeriod = x(n) Then
            SEP = f(n)
        ElseIf EndPeriod < x(n) Then
            For i = 0 To n - 1
                If EndPeriod >= x(i) And EndPeriod <= x(i + 1) Then
                    SEP = f(i) + b(i) * (EndPeriod - x(i)) + c(i) * (EndPeriod - x(i)) ^ 2 + d(i) * (EndPeriod - x(i)) ^ 3
                    Exit For
                End If
            Next
        End If

        If StartPeriod <= x(n) And EndPeriod <= x(n) Then
            retval = (SEP * (EndPeriod + 1) - SSP * (StartPeriod + 1)) / (EndPeriod - StartPeriod)
        Else
            retval = f(n)
        End If

        Return retval
    End Function
#End Region

#Region "Flash_Event"
    Private Enum FlashEnum
        Normal
        Flash
    End Enum
#End Region

    Private Sub bw_OptCalc_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Dim worker As System.ComponentModel.BackgroundWorker = TryCast(sender, System.ComponentModel.BackgroundWorker)

        If worker.CancellationPending = True Then
            e.Result = 0
        End If

        Dim ROUTE_ID As Integer = CInt(e.Argument)
        Dim RDTL As FFAOptCalcService.SwapDataClass = (From q In ROUTES_DETAIL Where q.ROUTE_ID = ROUTE_ID Select q).FirstOrDefault
        If IsNothing(RDTL) Then 'ooops no data for specific route
            Dim routelist As New List(Of Integer)
            routelist.Add(ROUTE_ID)
            Try
                ROUTES_DETAIL_Add(WEB.SDB.ROUTE_DETAIL(routelist))
                FIXINGS_RemoveAll(ROUTE_ID)                
                FIXINGS_Add(WEB.SDB.SwapVolatility(ROUTE_ID, My.Settings.DataPeriodDefault, My.Settings.DataVolDefault))
                Dim freshintradaydata As New List(Of FFAOptCalcService.VolDataClass)
                freshintradaydata.AddRange(WEB.SDB.RefreshIntradayData(routelist))
                FIXINGS_RefreshLiveData(routelist, freshintradaydata)
            Catch ex As Exception
                e.Cancel = True
            End Try
        End If
        e.Result = ROUTE_ID
    End Sub
    Private Sub bw_OptCalc_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        Me.Cursor = Cursors.Default
        Me.rgv_MARKET.Cursor = Cursors.Default
        EventForm.rwb_WAIT.StopWaiting()
        EventForm.rwb_WAIT.ResetLayout(True)

        If (e.Cancelled = True) Then
            MsgError(Me, "Internet Connection Failed, no data received.", "Connection Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        Dim ROUTE_ID As Integer = CInt(e.Result)

        Dim nr As New DataContracts.RouteViewClass
        nr.ROUTE_ID = ROUTE_ID
        nr.WIDTH = My.Settings.MarketViewContractWidths
        TabView.ROUTES.Add(nr)
        rgv_MARKET.SuspendUpdate()
        ConstructDataTable()
        FetchLocalData()
        PopulateAll()
        BuildContextMenu()
        rgv_MARKET.ResumeUpdate()
        rgv_MARKET.Refresh()
    End Sub

    Private Sub m_FIXINGS_ADD(ByVal f_Value As FFAOptCalcService.VolDataClass)
        SyncLock m_FIXINGS_Lock
            m_FIXINGS.Add(f_Value)
        End SyncLock
    End Sub
    Private Sub m_FIXINGS_ADD(ByVal f_List As List(Of FFAOptCalcService.VolDataClass))
        SyncLock m_FIXINGS_Lock
            m_FIXINGS.AddRange(f_List)
        End SyncLock
    End Sub

    Private Sub EventForm_BidAskReceived(sender As Object, BAList As List(Of FFASuitePL.WP8FFAData)) Handles EventForm.BidAskReceived
        m_FreezeCellValueChanging = True
        For Each r In BAList
            Dim gcol = (From q In rgv_MARKET.Columns Where q.Name = r.ROUTE_ID.ToString Select q).FirstOrDefault
            If IsNothing(gcol) = False Then
                Dim grow = (From q In rgv_MARKET.Rows Where q.Cells("YY1").Value = r.YY1 And q.Cells("YY2").Value = r.YY2 _
                            And q.Cells("MM1").Value = r.MM1 And q.Cells("MM2").Value = r.MM2 _
                            Select q).FirstOrDefault
                If IsNothing(grow) = False Then
                    grow.Cells(r.ROUTE_ID & "_BID").Value = r.BID
                    grow.Cells(r.ROUTE_ID & "_ASK").Value = r.ASK
                End If
            End If
        Next
        m_FreezeCellValueChanging = False
        rgv_MARKET.Refresh()
    End Sub
    Private Sub EventForm_ReceivedFFALiveTrade(sender As Object, LiveTrade As FFAOptCalcService.VolDataClass) Handles EventForm.ReceivedFFALiveTrade
        Dim RoutePresent = (From q In rgv_MARKET.Columns Where q.Name = LiveTrade.ROUTE_ID.ToString Select q).FirstOrDefault
        If IsNothing(RoutePresent) = True Then Exit Sub

        Dim RecFound As Boolean = False
        Dim RowIndex As Integer
        Dim ROUTE_ID As String = LiveTrade.ROUTE_ID.ToString

        For Each row In rgv_MARKET.Rows
            If row.Cells("YY1").Value = LiveTrade.YY1 And row.Cells("YY2").Value = LiveTrade.YY2 And row.Cells("MM1").Value = LiveTrade.MM1 And row.Cells("MM2").Value = LiveTrade.MM2 Then
                RowIndex = row.Index
                RecFound = True
                Exit For
            End If
        Next

        If RecFound Then
            If IsNothing(rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID).Value) Then Exit Sub
            SyncLock m_GRID_LOCK
                rgv_MARKET.BeginEdit()
                rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_V").Value = LiveTrade.VolRecordType
                rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_F").Value = FlashEnum.Flash
                rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_TID").Value = LiveTrade.TRADE_ID
                For I = 1 To My.Settings.FlashInterval
                    If I Mod 2 = 0 Then
                        rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID).Value = LiveTrade.FFA_PRICE
                    Else
                        rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID).Value = 0
                    End If
                    If I = My.Settings.FlashInterval Then
                        rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_F").Value = FlashEnum.Normal
                    End If
                    rgv_MARKET.TableElement.Update(GridUINotifyAction.StateChanged)
                    rgv_MARKET.Refresh()
                    System.Threading.Thread.Sleep(250)
                Next
            End SyncLock
        End If
        
    End Sub
    Private Sub EventForm_AmmededTradeReceived(sender As Object, AmmededTrade As FFAOptCalcService.VolDataClass) Handles EventForm.AmmededTradeReceived
        Dim ROUTE_ID As String = AmmededTrade.ROUTE_ID.ToString
        Dim RoutePresent = (From q In rgv_MARKET.Columns Where q.Name = ROUTE_ID.ToString Select q).FirstOrDefault
        If IsNothing(RoutePresent) = True Then Exit Sub

        Dim RowIndex As Integer = (From q In rgv_MARKET.Rows _
                                   Where q.Cells("YY1").Value = AmmededTrade.YY1 _
                                   And q.Cells("YY2").Value = AmmededTrade.YY2 _
                                   And q.Cells("MM1").Value = AmmededTrade.MM1 _
                                   And q.Cells("MM2").Value = AmmededTrade.MM2 _
                                   Select q.Index).FirstOrDefault
        If RowIndex > 0 Then
            If IsNothing(rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID).Value) Then Exit Sub
            If IsNothing(rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_V").Value) Then Exit Sub
            If IsNothing(rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_F").Value) Then Exit Sub
            If IsNothing(rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_TID").Value) Then Exit Sub

            If rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_TID").Value = AmmededTrade.TRADE_ID Then
                SyncLock m_GRID_LOCK
                    rgv_MARKET.BeginEdit()
                    rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID).Value = AmmededTrade.FFA_PRICE
                    rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_V").Value = AmmededTrade.VolRecordType
                    rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_F").Value = FlashEnum.Flash
                    For I = 1 To My.Settings.FlashInterval
                        If I Mod 2 = 0 Then
                            rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID).Value = AmmededTrade.FFA_PRICE
                        Else
                            rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID).Value = 0
                        End If
                        If I = My.Settings.FlashInterval Then
                            rgv_MARKET.Rows(RowIndex).Cells(ROUTE_ID & "_F").Value = FlashEnum.Normal
                        End If
                        rgv_MARKET.TableElement.Update(GridUINotifyAction.StateChanged)
                        rgv_MARKET.Refresh()
                        System.Threading.Thread.Sleep(250)
                    Next
                End SyncLock
            End If
        End If
    End Sub
    Private Sub EventForm_RefreshMarketView() Handles EventForm.RefreshMarketView
        Me.Cursor = Cursors.WaitCursor
        rgv_MARKET.SuspendUpdate()
        ConstructDataTable()
        FetchLocalData()
        PopulateAll()
        BuildContextMenu()
        rgv_MARKET.ResumeUpdate()
        rgv_MARKET.Refresh()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub EventForm_SpotRatesUpdated() Handles EventForm.SpotRatesUpdated
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf EventForm_SpotRatesUpdated, agsXMPP.ObjectHandler), New Object())
            Return
        End If

        Dim VisRoutes = (From q In rgv_MARKET.Columns Where q.Name <> "PERIOD" And q.IsVisible = True Select CInt(q.Name)).ToList
        Dim ActRoutes = (From q In ROUTES_DETAIL Where VisRoutes.Contains(q.ROUTE_ID) Select q).ToList

        For Each r In ActRoutes
            'SyncLock m_GRID_LOCK
            If r.SPOT_FIXING_DATE >= SERVER_DATE.Date Then
                rgv_MARKET.Rows(0).Cells(r.ROUTE_ID.ToString).Value = r.SPOT_PRICE
                rgv_MARKET.Rows(0).Cells(r.ROUTE_ID.ToString & "_V").Value = FFAOptCalcService.VolRecordTypeEnum.nspot
                rgv_MARKET.Rows(0).Cells(r.ROUTE_ID.ToString & "_F").Value = FlashEnum.Flash
            End If
            'End SyncLock            
        Next
        rgv_MARKET.TableElement.Update(GridUINotifyAction.StateChanged)
        rgv_MARKET.Refresh()

        For I = 1 To My.Settings.FlashInterval
            If I Mod 2 = 0 Then
                For Each r In ActRoutes
                    If r.SPOT_FIXING_DATE >= SERVER_DATE.Date Then
                        rgv_MARKET.Rows(0).Cells(r.ROUTE_ID.ToString).Value = r.SPOT_PRICE
                    End If
                Next
            Else
                For Each r In ActRoutes
                    If r.SPOT_FIXING_DATE >= SERVER_DATE.Date Then
                        rgv_MARKET.Rows(0).Cells(r.ROUTE_ID.ToString).Value = 0
                    End If
                Next
            End If
            If I = My.Settings.FlashInterval Then
                For Each r In ActRoutes
                    If r.SPOT_FIXING_DATE >= SERVER_DATE.Date Then
                        rgv_MARKET.Rows(0).Cells(r.ROUTE_ID.ToString & "_F").Value = FlashEnum.Normal
                    End If
                Next
            End If
            rgv_MARKET.TableElement.Update(GridUINotifyAction.StateChanged)
            rgv_MARKET.Refresh()
            System.Threading.Thread.Sleep(250)
        Next

    End Sub
    Private Sub EventForm_SwapRatesUpdated() Handles EventForm.SwapRatesUpdated
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf EventForm_SwapRatesUpdated, agsXMPP.ObjectHandler), New Object())
            Return
        End If

        rgv_MARKET.SuspendUpdate()
        ConstructDataTable()
        FetchLocalData(True)
        PopulateAll(True)
        BuildContextMenu()
        rgv_MARKET.ResumeUpdate()
        rgv_MARKET.Refresh()
    End Sub

#Region "GridEvents"

    Private Sub rgv_MARKET_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles rgv_MARKET.ToolTipTextNeeded

        'If TypeOf sender Is GridHeaderCellElement AndAlso CType(sender, GridHeaderCellElement).Text = "Foo" Then
        If TypeOf sender Is GridHeaderCellElement Then
            e.ToolTipText = "Right click mouse for options to add or delete columns"
        ElseIf TypeOf sender Is GridDataCellElement Then
            Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
            If cell.ColumnInfo.FieldName <> "PERIOD" Then
                If cell.ColumnInfo.Name.Contains("BID") = False Then
                    If cell.ColumnInfo.Name.Contains("ASK") = False Then
                        If cell.Value = 0 And cell.RowInfo.Cells(cell.ColumnInfo.Name & "_H").Value = 0 Then Exit Sub
                        Dim grhc As DataContracts.RouteViewClass = TryCast(cell.ColumnInfo.Tag, DataContracts.RouteViewClass)
                        Dim diff As Double = cell.Value - cell.RowInfo.Cells(cell.ColumnInfo.Name & "_H").Value
                        e.ToolTipText = Format(diff, grhc.TOOLTIP_FORMAT_STRING)
                        If cell.Value <> 0 Then
                            Dim pdiff As Double = diff / cell.RowInfo.Cells(cell.ColumnInfo.Name & "_H").Value
                            'e.ToolTipText += " (" & pdiff.ToString("P1") & ")"
                            e.ToolTipText += " (" & Format(pdiff, grhc.PERCENT_FORMAT_STRING) & ")"
                        End If
                        If cell.RowInfo.Index = 0 Then
                            Dim sptavg = (From q In ROUTES_DETAIL Where q.ROUTE_ID = CInt(cell.ColumnInfo.Name) Select q.FIXING_AVG).FirstOrDefault
                            e.ToolTipText += Environment.NewLine & "MA: " & Format(sptavg, grhc.TOOLTIP_FORMAT_STRING)
                        End If
                        If UD.MYFINGERPRINT.UPDATER Then
                            If IsNothing(cell.RowInfo.Cells(cell.ColumnInfo.Name & "_TID").Value) = False Then
                                e.ToolTipText += Environment.NewLine & "Trade ID: " & cell.RowInfo.Cells(cell.ColumnInfo.Name & "_TID").Value
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub rgv_MARKET_ViewCellFormatting(sender As Object, e As CellFormattingEventArgs) Handles rgv_MARKET.ViewCellFormatting        
        If IsNothing(e.CellElement.ColumnInfo) Then Exit Sub
        If e.Column.IsVisible = False Then Exit Sub

        If TypeOf e.CellElement Is GridHeaderCellElement Then
            e.CellElement.Font = m_FontsB(TabView.FontSize)
        ElseIf TypeOf e.CellElement Is Telerik.WinControls.UI.GridDataCellElement And e.Column.IsVisible = True Then
            If e.Column.Name = "PERIOD" Then
                e.CellElement.Font = m_FontsB(TabView.FontSize)
                e.CellElement.ForeColor = Color.Black
                Exit Sub
            Else
                e.CellElement.Font = m_FontsR(TabView.FontSize)
            End If

            If e.Column.Name <> "PERIOD" Then
                If e.CellElement.Value.ToString = "0" Then
                    e.CellElement.Text = ""
                End If

                If e.Column.Name.Contains("BID") Or e.Column.Name.Contains("ASK") Then
                    e.CellElement.Font = m_FontsR(TabView.FontSize)
                    e.CellElement.ForeColor = Color.Black
                Else
                    Select Case e.Row.Cells(e.Column.Name & "_V").Value
                        Case FFAOptCalcService.VolRecordTypeEnum.live
                            e.CellElement.Font = m_FontsB(TabView.FontSize)
                            If e.CellElement.Value >= e.Row.Cells(e.Column.Name & "_H").Value Then
                                e.CellElement.ForeColor = Color.DarkGreen
                            ElseIf e.CellElement.Value < e.Row.Cells(e.Column.Name & "_H").Value Then
                                e.CellElement.ForeColor = Color.Red
                            End If
                        Case FFAOptCalcService.VolRecordTypeEnum.level
                            e.CellElement.Font = m_FontsR(TabView.FontSize)
                            e.CellElement.ForeColor = Color.Black
                        Case FFAOptCalcService.VolRecordTypeEnum.nspot
                            e.CellElement.Font = m_FontsB(TabView.FontSize)
                            If e.CellElement.Value >= e.Row.Cells(e.Column.Name & "_H").Value Then
                                e.CellElement.ForeColor = Color.DarkGreen
                            ElseIf e.CellElement.Value < e.Row.Cells(e.Column.Name & "_H").Value Then
                                e.CellElement.ForeColor = Color.Red
                            End If
                        Case Else
                            e.CellElement.Font = m_FontsR(TabView.FontSize)
                            e.CellElement.ForeColor = Color.Black
                    End Select
                End If
            End If
        End If
    End Sub

    Private Sub rgv_MARKET_CellClick(sender As Object, e As GridViewCellEventArgs) Handles rgv_MARKET.CellClick

        If TypeOf e.Row Is GridViewTableHeaderRowInfo And e.ColumnIndex = 0 Then
            If m_MouseClick = Windows.Forms.MouseButtons.Left Then
                Dim f As New WebHelpForm
                f.url = "HelpForm1.pdf"
                f.Show()
            End If
        End If
    End Sub

    Private Sub rgv_MARKET_MouseClick(sender As Object, e As MouseEventArgs) Handles rgv_MARKET.MouseClick

        If e.Button = Windows.Forms.MouseButtons.Left Then
            m_MouseClick = Windows.Forms.MouseButtons.Left
        Else
            m_MouseClick = Windows.Forms.MouseButtons.None
        End If
    End Sub

    Private Sub rgv_MARKET_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles rgv_MARKET.CellValueChanged
        Dim column As GridViewDataColumn = TryCast(e.Column, GridViewDataColumn)
        If m_FreezeCellValueChanging = True Then Exit Sub

        If TypeOf e.Row Is GridViewDataRowInfo AndAlso column IsNot Nothing Then
            If column.Name.Contains("BID") Then
                Dim BID As Double = e.Value
                Dim ROUTE_ID As Integer = DirectCast(column.Tag, Integer)
                Dim YY1 As Integer = e.Row.Cells("YY1").Value
                Dim YY2 As Integer = e.Row.Cells("YY2").Value
                Dim MM1 As Integer = e.Row.Cells("MM1").Value
                Dim MM2 As Integer = e.Row.Cells("MM2").Value
                Dim qry = (From q In BIDASK Where q.FIXING_DATE = SERVER_DATE And q.ROUTE_ID = ROUTE_ID _
                           And q.YY1 = YY1 And q.YY2 = YY2 And q.MM1 = MM1 And q.MM2 = MM2 Select q).FirstOrDefault
                If IsNothing(qry) = False Then
                    qry.BID = BID
                    qry.PRICE_STATUS = FFASuitePL.PriceStatusEnum.Level
                Else
                    Dim nr As New FFASuitePL.WP8FFAData
                    nr.FIXING_DATE = SERVER_DATE
                    nr.ROUTE_ID = ROUTE_ID
                    nr.YY1 = YY1
                    nr.YY2 = YY2
                    nr.MM1 = MM1
                    nr.MM2 = MM2
                    nr.BID = BID
                    nr.ASK = 0.0#
                    nr.PRICE_STATUS = FFASuitePL.PriceStatusEnum.Level
                    BIDASK.Add(nr)
                End If
            ElseIf column.Name.Contains("ASK") Then
                Dim ASK As Double = e.Value
                Dim ROUTE_ID As Integer = DirectCast(column.Tag, Integer)
                Dim YY1 As Integer = e.Row.Cells("YY1").Value
                Dim YY2 As Integer = e.Row.Cells("YY2").Value
                Dim MM1 As Integer = e.Row.Cells("MM1").Value
                Dim MM2 As Integer = e.Row.Cells("MM2").Value
                Dim qry = (From q In BIDASK Where q.FIXING_DATE = SERVER_DATE And q.ROUTE_ID = ROUTE_ID _
                           And q.YY1 = YY1 And q.YY2 = YY2 And q.MM1 = MM1 And q.MM2 = MM2 Select q).FirstOrDefault
                If IsNothing(qry) = False Then
                    qry.ASK = ASK
                    qry.PRICE_STATUS = FFASuitePL.PriceStatusEnum.Level
                Else
                    Dim nr As New FFASuitePL.WP8FFAData
                    nr.FIXING_DATE = SERVER_DATE
                    nr.ROUTE_ID = ROUTE_ID
                    nr.YY1 = YY1
                    nr.YY2 = YY2
                    nr.MM1 = MM1
                    nr.MM2 = MM2
                    nr.BID = 0.0#
                    nr.ASK = ASK
                    nr.PRICE_STATUS = FFASuitePL.PriceStatusEnum.Level
                    BIDASK.Add(nr)
                End If
            End If
        End If
    End Sub

    Private Sub rgv_MARKET_CellBeginEdit(sender As Object, e As GridViewCellCancelEventArgs) Handles rgv_MARKET.CellBeginEdit
        Dim column As GridViewDataColumn = TryCast(e.Column, GridViewDataColumn)

        If TypeOf e.Row Is GridViewDataRowInfo AndAlso column IsNot Nothing Then
            Dim row As GridViewDataRowInfo = TryCast(e.Row, GridViewDataRowInfo)
            If row IsNot Nothing Then
                If row.Cells("YY1").Value = 0 Then
                    e.Cancel = True
                End If
            End If
        End If
    End Sub
#End Region

    Private Sub rgv_MARKET_DefaultValuesNeeded(sender As Object, e As GridViewRowEventArgs) Handles rgv_MARKET.DefaultValuesNeeded
        If IsNothing(e.Row) Then Exit Sub
        For I = 0 To e.Row.Cells.Count - 1
            e.Row.Cells(I).Value = 0
        Next
    End Sub

   
End Class
