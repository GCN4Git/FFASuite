Option Strict Off
Imports ArtBOptWCFService.DataContracts
Imports System.Threading

Public Class FFAOptMain
    Implements IFFAOptMain

    Private Shared GD As New List(Of VolDataClass)
    Private Shared ServiceIsBusy As Boolean = False
    Private Shared GDLock As New Object
    Private Shared MVLock As New Object
    Private Shared SwapVolatilityLock As New Object
    Private m_doneEvent As ManualResetEvent
    Private Shared MVPeriods As New List(Of ArtBTimePeriod)
#If DEBUG Then
    Private DBConStr As String = My.Settings.DBConnectionString
#Else
    Private DBConStr As String = My.Settings.DBConnectionStringServer
#End If

    Public Sub New()
    End Sub

#Region "WP8"
    Public Function GetPriceStatusEnum() As DataContracts.PriceStatusEnum Implements IFFAOptMain.GetPriceStatusEnum
        Return PriceStatusEnum.Historic
    End Function
    Public Function GetRecordTypeEnum() As DataContracts.RecordTypeEnum Implements IFFAOptMain.GetRecordTypeEnum
        Return DataContracts.RecordTypeEnum.PriceSpread
    End Function
    Public Function GetNSwapPeriodsEnum() As DataContracts.NSwapPeriodsEnum Implements IFFAOptMain.GetNSwapPeriodsEnum
        Return DataContracts.NSwapPeriodsEnum.Cal
    End Function
    Public Function GetWP8FFAData() As DataContracts.WP8FFAData Implements IFFAOptMain.GetWP8FFAData
        Return New DataContracts.WP8FFAData
    End Function
    Public Function GetGeneralFFDataClass() As DataContracts.GeneralFFDataClass Implements IFFAOptMain.GetGeneralFFDataClass
        Return New DataContracts.GeneralFFDataClass
    End Function
    Public Function GetSwapCurveClass() As DataContracts.SwapCurveClass Implements IFFAOptMain.GetSwapCurveClass
        Return New DataContracts.SwapCurveClass
    End Function
    Public Function GetNSwapPeriodsClass() As DataContracts.NSwapPeriodsClass Implements IFFAOptMain.GetNSwapPeriodsClass
        Return New DataContracts.NSwapPeriodsClass
    End Function
#End Region

    Public Sub KeepAlive() Implements IFFAOptMain.KeepAlive
        'do nothing
    End Sub

#Region "Update Fixings"

    Public Function UpdateSpreadMargins(ByVal FIXING_DATE As Date, ByVal rse_PERIOD As Integer, ByVal rse_SDEV As Double) As Boolean Implements IFFAOptMain.UpdateSpreadMargins
        Dim DB As New ArtBDataContext

        Try
            'Now append Spread Margins for each vessel class        
            Dim vsc = (From q In DB.VESSEL_CLASS _
                       Order By q.DRYWET, q.VESSEL_CLASS_ID _
                       Select q).ToList
            For Each vsc_row In vsc
                Dim sm0 = (From q In DB.BALTIC_FORWARD_RATES _
                           Where q.ROUTE_ID = vsc_row.DEFAULT_ROUTE_ID _
                           And q.FIXING_DATE = FIXING_DATE _
                           Select q).ToList
                For Each sm0_row In sm0
                    Dim DEFAULT_ROUTE_ID As Integer = sm0_row.ROUTE_ID
                    Dim YY1 As Integer = sm0_row.YY1
                    Dim YY2 As Integer = sm0_row.YY2
                    Dim MM1 As Integer = sm0_row.MM1
                    Dim MM2 As Integer = sm0_row.MM2
                    Dim YY As Integer = sm0_row.YY
                    Dim PERIOD As String = sm0_row.PERIOD
                    Dim CMSROUTE_ID As String = sm0_row.CMSROUTE_ID

                    Dim tdata = (From q In DB.BALTIC_FORWARD_RATES _
                                 Where q.ROUTE_ID = DEFAULT_ROUTE_ID _
                                 And q.FIXING_DATE <= FIXING_DATE _
                                 And q.YY1 = YY1 _
                                 And q.MM1 = MM1 _
                                 And q.YY2 = YY2 _
                                 And q.MM2 = MM2 _
                                 Order By q.FIXING_DATE Descending).Take(CInt(rse_PERIOD + 1))
                    Dim datarray(tdata.Count - 1) As Double
                    Dim J As Integer = 0
                    For Each tdata_row In tdata
                        datarray(J) = tdata_row.FIXING
                        J += 1
                    Next
                    Dim VCmargin As Double = CalcMargin(datarray, rse_SDEV, False)

                    If VCmargin = 0 Then
                        Dim prevval = (From q In DB.VESSEL_CLASS_SPREAD_MARGINS _
                                       Where q.VESSEL_CLASS_ID = vsc_row.VESSEL_CLASS_ID _
                                       And q.ROUTE_ID = DEFAULT_ROUTE_ID _
                                       Select q).Take(10)
                        If IsNothing(prevval) = False Then
                            Dim avg As New List(Of Double)
                            For Each r In prevval
                                avg.Add(r.MARGIN)
                            Next
                            VCmargin = avg.Average
                        Else
                            VCmargin = 0
                        End If
                    End If
                    Dim updqry = (From q In DB.VESSEL_CLASS_SPREAD_MARGINS _
                                  Where q.VESSEL_CLASS_ID = vsc_row.VESSEL_CLASS_ID _
                                  And q.ROUTE_ID = DEFAULT_ROUTE_ID _
                                  And q.CMSROUTE_ID = CMSROUTE_ID _
                                  Select q).FirstOrDefault
                    If IsNothing(updqry) = False Then
                        updqry.MARGIN = VCmargin
                        updqry.YY1 = YY1
                        updqry.MM1 = MM1
                        updqry.YY2 = YY2
                        updqry.MM2 = MM2
                        updqry.YY = YY
                        updqry.PERIOD = PERIOD
                    End If
                Next
            Next
            DB.SubmitChanges()
        Catch ex As Exception
            DB.Dispose()
            Return False
        End Try
        DB.Dispose()
        Return True

    End Function
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
    Public Function GetContractFTP() As List(Of BALTIC_FTP) Implements IFFAOptMain.GetContractFTP
        Dim DB As New ArtBDataContext
        Dim answr = (From q In DB.BALTIC_FTP Order By q.CMSROUTE_ID Select q).ToList
        DB.Dispose()
        Return answr
    End Function

    Public Function UpdateVolatilities(ByVal FIXING_DATE As Date, ByVal VOLATILITY_FIXINGS As List(Of BALTIC_OPTION_VOLATILITIES)) As Boolean Implements IFFAOptMain.UpdateVolatilities
        Dim DB As New ArtBDataContext
        Dim trans As System.Data.Common.DbTransaction = Nothing

        Try
            DB.Connection.Open()
            trans = DB.Connection.BeginTransaction(IsolationLevel.Serializable)
            DB.Transaction = trans

            Dim CID = (From q In VOLATILITY_FIXINGS Select q.ROUTE_ID).Distinct.ToList
            Dim delrec = (From q In DB.BALTIC_OPTION_VOLATILITIES Where q.FIXING_DATE = FIXING_DATE And CID.Contains(q.ROUTE_ID) Select q).ToList
            DB.BALTIC_OPTION_VOLATILITIES.DeleteAllOnSubmit(delrec)
            DB.SubmitChanges()

            DB.BALTIC_OPTION_VOLATILITIES.InsertAllOnSubmit(VOLATILITY_FIXINGS)
            DB.SubmitChanges()

            trans.Commit()
            DB.Dispose()
            Return True
        Catch ex As Exception
            trans.Rollback()
            DB.Dispose()
            Return False
        End Try
    End Function
    Public Function UpdateForwards(ByVal FIXING_DATE As Date, ByVal FORWARD_FIXINGS As List(Of BALTIC_FORWARD_RATES)) As Boolean Implements IFFAOptMain.UpdateForwards
        Dim DB As New ArtBDataContext
        Dim trans As System.Data.Common.DbTransaction = Nothing

        Try
            DB.Connection.Open()
            trans = DB.Connection.BeginTransaction(IsolationLevel.Serializable)
            DB.Transaction = trans

            Dim CID = (From q In FORWARD_FIXINGS Select q.ROUTE_ID).Distinct.ToList
            Dim delrec = (From q In DB.BALTIC_FORWARD_RATES Where q.FIXING_DATE = FIXING_DATE And CID.Contains(q.ROUTE_ID) Select q).ToList
            DB.BALTIC_FORWARD_RATES.DeleteAllOnSubmit(delrec)
            DB.SubmitChanges()

            DB.BALTIC_FORWARD_RATES.InsertAllOnSubmit(FORWARD_FIXINGS)
            DB.SubmitChanges()

            trans.Commit()
            DB.Dispose()
            Return True
        Catch ex As Exception
            trans.Rollback()
            DB.Dispose()
            Return False
        End Try
    End Function
    Public Function UpdateSpots(ByVal FIXING_DATE As Date, ByVal SPOT_FIXINGS As List(Of BALTIC_SPOT_RATES)) As Boolean Implements IFFAOptMain.UpdateSpots
        Dim DB As New ArtBDataContext
        Dim trans As System.Data.Common.DbTransaction = Nothing

        Try
            DB.Connection.Open()
            trans = DB.Connection.BeginTransaction(IsolationLevel.Serializable)
            DB.Transaction = trans

            Dim CID = (From q In SPOT_FIXINGS Select q.ROUTE_ID).Distinct.ToList
            Dim delrec = From q In DB.BALTIC_SPOT_RATES Where q.FIXING_DATE = FIXING_DATE And CID.Contains(q.ROUTE_ID) Select q
            DB.BALTIC_SPOT_RATES.DeleteAllOnSubmit(delrec)
            DB.SubmitChanges()

            DB.BALTIC_SPOT_RATES.InsertAllOnSubmit(SPOT_FIXINGS)
            DB.SubmitChanges()

            trans.Commit()
            DB.Dispose()
            Return True
        Catch ex As Exception
            trans.Rollback()
            DB.Dispose()
            Return False
        End Try
    End Function
    Public Function UpdateWeeklyVolumes(ByVal FIXING_DATE As Date, ByVal VOLUMES_FIXINGS As List(Of BALTIC_DRY_VOLUMES)) As Boolean Implements IFFAOptMain.UpdateWeeklyVolumes
        Dim DB As New ArtBDataContext
        Dim trans As System.Data.Common.DbTransaction = Nothing

        Try
            DB.Connection.Open()
            trans = DB.Connection.BeginTransaction(IsolationLevel.Serializable)
            DB.Transaction = trans

            Dim delrec = (From q In DB.BALTIC_DRY_VOLUMES Where q.FIXING_DATE = FIXING_DATE Select q).ToList
            DB.BALTIC_DRY_VOLUMES.DeleteAllOnSubmit(delrec)
            DB.SubmitChanges()

            DB.BALTIC_DRY_VOLUMES.InsertAllOnSubmit(VOLUMES_FIXINGS)
            DB.SubmitChanges()

            trans.Commit()
            DB.Dispose()
            Return True
        Catch ex As Exception
            trans.Rollback()
            DB.Dispose()
            Return False
        End Try
    End Function
#End Region

    Public Function AmmendReportedTrade(ByVal Trade As DataContracts.VolDataClass) As Boolean Implements IFFAOptMain.AmmendReportedTrade
        Dim DB As New ArtBDataContext
        Dim result As Boolean = True
        Dim oldtrade = (From q In DB.TRADES_FFA Where q.TRADE_ID = Trade.TRADE_ID Select q).FirstOrDefault
        If IsNothing(oldtrade) = False Then
            Try
                oldtrade.PRICE_TRADED = Trade.FFA_PRICE
                oldtrade.PNC = Trade.PNC
                DB.SubmitChanges()
                result = True
            Catch ex As Exception
                result = False
            End Try
        End If
        DB.Dispose()
        Return result
    End Function
    Public Function GetDailyTrades(ByVal TRADE_DATE As Date) As List(Of DataContracts.VolDataClass) Implements IFFAOptMain.GetDailyTrades
        Dim DB As New ArtBDataContext
        Dim retval As New List(Of DataContracts.VolDataClass)

        Dim swaptrades = (From q In DB.TRADES_FFA Where q.ORDER_DATETIME >= TRADE_DATE And q.ORDER_DATETIME < TRADE_DATE.AddDays(1) _
                          And q.TRADE_TYPE = OrderTypesEnum.FFA Select q).ToList
        For Each r In swaptrades
            Dim nc As New DataContracts.VolDataClass
            nc.ROUTE_ID = r.ROUTE_ID
            nc.DESK_TRADER_ID = r.DESK_TRADER_ID1
            nc.TRADE_ID = r.TRADE_ID
            nc.TRADE_TYPE = r.TRADE_TYPE
            nc.FIXING_DATE = r.ORDER_DATETIME
            nc.FFA_PRICE = r.PRICE_TRADED
            If r.PNC = True Then
                nc.PNC = True
                nc.VolRecordType = VolRecordTypeEnum.level
            Else
                nc.PNC = False
                nc.VolRecordType = VolRecordTypeEnum.live
            End If
            nc.YY1 = r.YY1
            nc.MM1 = r.MM1
            nc.YY2 = r.YY2
            nc.MM2 = r.MM2
            retval.Add(nc)
        Next

        Dim spreadtrades = (From q In DB.TRADES_FFA Where q.ORDER_DATETIME >= TRADE_DATE And q.ORDER_DATETIME < TRADE_DATE.AddDays(1) _
                            And q.TRADE_TYPE <> OrderTypesEnum.FFA _
                            And q.UPDATE_STATUS = 0 Select q).ToList
        For Each r In spreadtrades
            Dim nc As New DataContracts.VolDataClass
            nc.ROUTE_ID = r.ROUTE_ID
            nc.DESK_TRADER_ID = r.DESK_TRADER_ID1
            nc.TRADE_ID = r.TRADE_ID
            nc.TRADE_TYPE = r.TRADE_TYPE
            nc.FIXING_DATE = r.ORDER_DATETIME
            nc.FFA_PRICE = r.PRICE_TRADED
            If r.PNC = True Then
                nc.PNC = True
                nc.VolRecordType = VolRecordTypeEnum.level
            Else
                nc.PNC = False
                nc.VolRecordType = VolRecordTypeEnum.live
            End If
            nc.YY1 = r.YY1
            nc.MM1 = r.MM1
            nc.YY2 = r.YY2
            nc.MM2 = r.MM2
            nc.YY21 = r.YY21
            nc.MM21 = r.MM21
            nc.YY22 = r.YY22
            nc.MM22 = r.MM22
            retval.Add(nc)
        Next
        DB.Dispose()
        Return retval
    End Function

    Public Function RefreshIntradayData(ByVal f_ROUTES As List(Of Integer)) As List(Of DataContracts.VolDataClass) Implements IFFAOptMain.RefreshIntradayData
        Dim DB As New ArtBDataContext
        Dim retlist As New List(Of DataContracts.VolDataClass)

        For Each f_ROUTE_ID In f_ROUTES
            'check spot fixings
            Dim SRID = (From q In DB.ROUTES Where q.ROUTE_ID = f_ROUTE_ID Select q.SETTL_ROUTE_ID).FirstOrDefault
            Dim oldspot = (From q In GD Where q.ROUTE_ID = f_ROUTE_ID _
                           And (q.VolRecordType = VolRecordTypeEnum.spot Or q.VolRecordType = VolRecordTypeEnum.nspot) _
                           Order By q.FIXING_DATE Descending _
                           Select q).FirstOrDefault
            Dim newspot = (From q In DB.BALTIC_SPOT_RATES Where q.ROUTE_ID = SRID _
                           Order By q.FIXING_DATE Descending Select q).FirstOrDefault
            If IsNothing(oldspot) = True Then
                Dim nc As New VolDataClass
                nc.ROUTE_ID = f_ROUTE_ID
                nc.FIXING_DATE = newspot.FIXING_DATE
                nc.SPOT_PRICE = newspot.FIXING
                nc.PERIOD = "SPOT"
                If newspot.FIXING_DATE >= Today Then
                    nc.VolRecordType = VolRecordTypeEnum.nspot
                    GD_RemoveOldAddNew(f_ROUTE_ID, nc, VolRecordTypeEnum.nspot)
                Else
                    nc.VolRecordType = VolRecordTypeEnum.spot
                    GD_RemoveOldAddNew(f_ROUTE_ID, nc, VolRecordTypeEnum.spot)
                End If
            ElseIf newspot.FIXING_DATE > oldspot.FIXING_DATE Then
                Dim nc As New VolDataClass
                nc.ROUTE_ID = f_ROUTE_ID
                nc.FIXING_DATE = newspot.FIXING_DATE
                nc.SPOT_PRICE = newspot.FIXING
                nc.PERIOD = "SPOT"
                If newspot.FIXING_DATE >= Today Then
                    nc.VolRecordType = VolRecordTypeEnum.nspot
                    GD_RemoveOldAddNew(f_ROUTE_ID, nc, VolRecordTypeEnum.nspot)
                Else
                    nc.VolRecordType = VolRecordTypeEnum.spot
                    GD_RemoveOldAddNew(f_ROUTE_ID, nc, VolRecordTypeEnum.spot)
                End If
            End If
        Next f_ROUTE_ID

        Try
            retlist.AddRange(From q In GD Where f_ROUTES.Contains(q.ROUTE_ID) = True And q.VolRecordType = VolRecordTypeEnum.nspot Select q)
        Catch ex As Exception
        End Try

        Dim livetrades = GetDailyTrades(Today)
        DB.Dispose()

        Try
            retlist.AddRange(From q In livetrades Where q.TRADE_TYPE = OrderTypesEnum.FFA And f_ROUTES.Contains(q.ROUTE_ID) = True Select q)
        Catch ex As Exception
        End Try
        Try
            retlist.AddRange(From q In livetrades Where q.TRADE_TYPE <> OrderTypesEnum.FFA _
                             And f_ROUTES.Contains(q.ROUTE_ID) = True Select q)
        Catch ex As Exception
        End Try
        Try
            retlist.AddRange(From q In livetrades Where q.TRADE_TYPE <> OrderTypesEnum.FFA _
                             And f_ROUTES.Contains(q.ROUTE_ID2) = True Select q)
        Catch ex As Exception
        End Try

        Return retlist
    End Function
    Public Function GetMarketView(ByVal f_ROUTES As List(Of Integer)) As List(Of DataContracts.VolDataClass) Implements IFFAOptMain.GetMarketView
        Dim DB As New ArtBDataContext
        Dim retval As New List(Of DataContracts.VolDataClass)
        Dim MVPeriods As List(Of ArtBTimePeriod) = GetMVPeriods(36)
        Dim I As Integer = 0
        For Each m In MVPeriods
            m.ROUTE_ID = I
            Dim nmv As New DataContracts.VolDataClass
            nmv.ROUTE_ID = I
            nmv.YY1 = m.YY1
            nmv.MM1 = m.MM1
            nmv.YY2 = m.YY2
            nmv.MM2 = m.MM2
            nmv.PERIOD = m.Descr
            nmv.VolRecordType = VolRecordTypeEnum.all
            retval.Add(nmv)
            I += 1
        Next

        For Each r In f_ROUTES
            Dim SetRID As Integer = (From q In DB.ROUTES Where q.ROUTE_ID = r Select q.SETTL_ROUTE_ID).FirstOrDefault
            Dim SpotFix = (From q In DB.BALTIC_SPOT_RATES _
                           Where q.ROUTE_ID = SetRID Order By q.FIXING_DATE Descending Select q).Take(2)

            Dim nspot As New DataContracts.VolDataClass
            nspot.ROUTE_ID = r
            nspot.FIXING_DATE = SpotFix(0).FIXING_DATE
            nspot.PERIOD = 0
            nspot.SPOT_PRICE = SpotFix(0).FIXING
            nspot.FFA_PRICE = SpotFix(1).FIXING
            nspot.IVOL = nspot.SPOT_PRICE - nspot.FFA_PRICE
            nspot.HVOL = (nspot.SPOT_PRICE - nspot.FFA_PRICE) / nspot.FFA_PRICE
            nspot.INTEREST_RATE = s_RouteSpotAverage(r, SpotFix(0).FIXING_DATE.Year, SpotFix(0).FIXING_DATE.Month)
            If SpotFix(0).FIXING_DATE.Date = Today.Date Then
                nspot.VolRecordType = VolRecordTypeEnum.nspot
            Else
                nspot.VolRecordType = VolRecordTypeEnum.spot
            End If
            retval.Add(nspot)

            Dim SwapFixDates = (From q In DB.BALTIC_FORWARD_RATES Where q.ROUTE_ID = r Order By q.FIXING_DATE Descending Select q.FIXING_DATE).Take(2)
            For Each m In MVPeriods
                Dim maxyear = (From q In DB.BALTIC_FORWARD_RATES Where q.ROUTE_ID = r _
                               And q.FIXING_DATE = SwapFixDates(0) _
                               Select q.YY2).Max
                Dim nswap As New DataContracts.VolDataClass
                If m.YY2 <= maxyear Then
                    Dim swpprc = f_GetSwapFixingsHistorical(r, m.YY1, m.MM1, m.YY2, m.MM2, SwapFixDates(1), SwapFixDates(0))
                    nswap.ROUTE_ID = r
                    nswap.FIXING_DATE = SwapFixDates(0)
                    nswap.PERIOD = m.ROUTE_ID
                    nswap.FFA_PRICE = swpprc(1).FFA_PRICE
                    nswap.SPOT_PRICE = swpprc(0).FFA_PRICE
                    nswap.IVOL = nspot.FFA_PRICE - nspot.SPOT_PRICE
                    nswap.HVOL = (nspot.FFA_PRICE - nspot.SPOT_PRICE) / nspot.SPOT_PRICE
                    nswap.YY1 = m.YY1
                    nswap.MM1 = m.MM1
                    nswap.YY2 = m.YY2
                    nswap.MM2 = m.MM2
                    nswap.VolRecordType = VolRecordTypeEnum.swap
                    If nswap.FIXING_DATE.Date = Today.Date Then
                        nswap.VolRecordType = VolRecordTypeEnum.swap
                    Else
                        nswap.VolRecordType = VolRecordTypeEnum.live
                    End If
                Else
                    nswap.ROUTE_ID = r
                    nswap.FIXING_DATE = SwapFixDates(0)
                    nswap.PERIOD = m.ROUTE_ID
                    nswap.FFA_PRICE = 0
                    nswap.SPOT_PRICE = 0
                    nswap.IVOL = 0
                    nswap.HVOL = 0
                    nswap.YY1 = m.YY1
                    nswap.MM1 = m.MM1
                    nswap.YY2 = m.YY2
                    nswap.MM2 = m.MM2
                    If nswap.FIXING_DATE.Date = Today.Date Then
                        nswap.VolRecordType = VolRecordTypeEnum.swap
                    Else
                        nswap.VolRecordType = VolRecordTypeEnum.live
                    End If
                End If
                retval.Add(nswap)
            Next
        Next

        Return retval
    End Function
    Public Function Graph(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer) As List(Of DataContracts.VolDataClass) Implements IFFAOptMain.Graph
        Dim DB As New ArtBDataContext
        Dim retlist As New List(Of VolDataClass)

        If f_YY1 = 0 And f_MM1 = 0 And f_YY2 = 0 And f_MM2 = 0 Then 'spot
            Dim spot As IEnumerable(Of BALTIC_SPOT_RATES) = (From q In DB.BALTIC_SPOT_RATES _
                                                             Join r In DB.ROUTES On r.SETTL_ROUTE_ID Equals q.ROUTE_ID _
                                                             Where r.ROUTE_ID = f_ROUTE_ID Order By q.FIXING_DATE Descending _
                                                             Select q).Take(1000).ToList
            If IsNothing(spot) = False Then
                spot = spot.Reverse
                For Each r In spot
                    Dim nc As New VolDataClass
                    nc.ROUTE_ID = f_ROUTE_ID
                    nc.FIXING_DATE = r.FIXING_DATE
                    nc.FFA_PRICE = r.FIXING
                    nc.VolRecordType = VolRecordTypeEnum.swap
                    retlist.Add(nc)
                Next
            End If
        Else
            If f_MM1 = 1 And f_MM2 = 12 And f_YY1 = f_YY2 Then
                Dim sdate As Date = (From q In DB.BALTIC_FORWARD_RATES Where _
                                     q.ROUTE_ID = f_ROUTE_ID And _
                                     q.YY1 = f_YY1 And q.MM1 = f_MM1 And q.YY2 = f_YY2 And q.MM2 = f_MM2 _
                                     Order By q.FIXING_DATE Select q.FIXING_DATE).FirstOrDefault
                If IsNothing(sdate) = False Then
                    Dim swap = f_GetSwapFixingsHistorical(f_ROUTE_ID, f_YY1, f_MM1, f_YY2, f_MM2, sdate, Today)
                    retlist.AddRange(swap)
                Else
                    Dim swap = f_GetSwapFixingsHistorical(f_ROUTE_ID, f_YY1, f_MM1, f_YY2, f_MM2, Today.AddMonths(-12), Today)
                    retlist.AddRange(swap)
                End If
            Else
                Dim swap = f_GetSwapFixingsHistorical(f_ROUTE_ID, f_YY1, f_MM1, f_YY2, f_MM2, Today.AddMonths(-12), Today)
                retlist.AddRange(swap)
            End If
            'get intraday trades
            Dim intraday = (From q In DB.TRADES_FFA Where q.ROUTE_ID = f_ROUTE_ID _
                            And q.YY1 = f_YY1 And q.YY2 = f_YY2 And q.MM1 = f_MM1 And q.MM2 = f_MM2 _
                            And q.TRADE_TYPE = ArtB_Class_Library.OrderTypes.FFA _
                            And q.PNC = False _
                            Order By q.ORDER_DATETIME Select q).ToList
            For Each r In intraday
                Dim nc As New VolDataClass
                nc.ROUTE_ID = f_ROUTE_ID
                nc.FIXING_DATE = r.ORDER_DATETIME
                nc.FFA_PRICE = r.PRICE_TRADED
                nc.PNC = r.PNC
                nc.VolRecordType = VolRecordTypeEnum.live
                retlist.Add(nc)
            Next
        End If
        DB.Dispose()
        Return retlist
    End Function

    Public Function SwapFixings() As List(Of DataContracts.SwapFixingsClass) Implements IFFAOptMain.SwapFixings
        Dim DB As New ArtBDataContext
        Dim retlist As New List(Of DataContracts.SwapFixingsClass)
        Dim m_FIXING_DATE As Date

        m_FIXING_DATE = (From q In DB.BALTIC_FORWARD_RATES Select q.FIXING_DATE).Max
        retlist = (From q In DB.BALTIC_FORWARD_RATES Where q.FIXING_DATE = m_FIXING_DATE _
                   Order By q.ROUTE_ID, q.YY1, q.MM1, q.YY2, q.MM2 _
                   Select New DataContracts.SwapFixingsClass(q.ROUTE_ID, q.FIXING_DATE, q.FIXING, q.YY1, q.MM1, q.YY2, q.MM2)).ToList
        DB.Dispose()
        Return retlist
    End Function
    Public Sub BuildGDPostFixing(ByVal FINGERPRINT As String) Implements IFFAOptMain.BuildGDPostFixing
        Dim DB As New ArtBDataContext
        Dim retlist As New List(Of DataContracts.VolDataClass)

        'check for authority to perform this action
        Dim authority = (From q In DB.ARTBOPTCALC_FINGERPRINTS Where q.FINGER_PRINT = FINGERPRINT Select q).FirstOrDefault
        If IsNothing(authority) = True Then
            DB.Dispose()
            Exit Sub
        Else
            If authority.UPDATER = False Then
                DB.Dispose()
                Exit Sub
            End If
        End If

        Dim stime = Now

        Dim Routes = (From q In DB.ROUTES _
                      Join v In DB.VESSEL_CLASS On v.VESSEL_CLASS_ID Equals q.VESSEL_CLASS_ID _
                      Join t In DB.TRADE_CLASSES On t.TRADE_CLASS_SHORT Equals v.DRYWET _
                      Where My.Settings.TradeClassesSupported.Contains(t.TRADE_CLASS_SHORT) And q.FFA_TRADED = True _
                      Select q.ROUTE_ID).ToList
        DB.Dispose()
        For Each r In Routes
            Debug.Print("Processing Route: " & r)
            retlist = (f_SwapVolatility(r))
            GD_RemoveOldAddNew(r, retlist, VolRecordTypeEnum.all)
        Next
        Debug.Print("Loop startet at: " & stime.ToLocalTime.ToLongTimeString)
        Debug.Print("Loop ended at: " & Now.ToLocalTime.ToLongTimeString)
    End Sub
    Public Function SpotFixings() As List(Of DataContracts.SpotFixingsClass) Implements IFFAOptMain.SpotFixings
        Dim DB As New ArtBDataContext
        Dim retlist As New List(Of DataContracts.SpotFixingsClass)
        Dim m_FIXING_DATE As Date

        m_FIXING_DATE = (From q In DB.BALTIC_SPOT_RATES Select q.FIXING_DATE).Max
        retlist = (From q In DB.BALTIC_SPOT_RATES Join r In DB.ROUTES On r.ROUTE_ID Equals q.ROUTE_ID _
                   Where q.FIXING_DATE = m_FIXING_DATE And r.FFA_TRADED = True _
                   Select New DataContracts.SpotFixingsClass(q.ROUTE_ID, q.FIXING_DATE, q.FIXING, s_RouteSpotAverage(q.ROUTE_ID, q.FIXING_DATE.Year, q.FIXING_DATE.Month))).ToList
        DB.Dispose()
        Return retlist
    End Function

    Public Function GetMVPeriods(Optional ByVal f_ROUTE_ID As Integer = 36, Optional ByVal ReturnAll As Boolean = False) As List(Of ArtBTimePeriod) Implements IFFAOptMain.GetMVPeriods
        Dim retval As New List(Of ArtBTimePeriod)
        Dim DB As New ArtBDataContext

        SyncLock MVLock
            Dim rfound = (From q In MVPeriods Where q.ROUTE_ID = f_ROUTE_ID Select q).FirstOrDefault
            If rfound Is Nothing Then
                Dim GVC As ArtB_Class_Library.GlobalViewClass = New ArtB_Class_Library.GlobalViewClass
                GVC.SetConnectionString(DB.Connection.ConnectionString)
                GVC.GetAll()
                Dim nl As New List(Of Integer)
                nl.Add(2)
                Dim tlist As List(Of ArtB_Class_Library.ArtBTimePeriod) = GVC.PopulatePeriods(f_ROUTE_ID, nl, Today, 3, False)
                For Each r In tlist
                    If r.MM1 = 4 And r.MM2 = 9 Then
                        Continue For
                    End If
                    If r.MM1 = 10 And r.MM2 = 3 Then
                        Continue For
                    End If

                    Dim nc As New ArtBTimePeriod
                    nc.Descr = r.Descr
                    nc.DPM = r.DPM
                    nc.EndDate = r.EndDate
                    nc.EndMonth = r.EndMonth
                    nc.MM1 = r.MM1
                    nc.MM2 = r.MM2
                    nc.ROUTE_ID = r.ROUTE_ID
                    nc.SERVER_DATE = r.SERVER_DATE
                    nc.StartDate = r.StartDate
                    nc.StartMonth = r.StartMonth
                    nc.YY1 = r.YY1
                    nc.YY2 = r.YY2
                    
                    MVPeriods.Add(nc)
                Next
            Else
                If rfound.SERVER_DATE < Today Then
                    Dim GVC As ArtB_Class_Library.GlobalViewClass = New ArtB_Class_Library.GlobalViewClass
                    GVC.SetConnectionString(DB.Connection.ConnectionString)
                    GVC.GetAll()
                    Dim nl As New List(Of Integer)
                    nl.Add(2)
                    MVPeriods.RemoveAll(Function(x) x.ROUTE_ID = f_ROUTE_ID And x.SERVER_DATE < Today)
                    Dim tlist As List(Of ArtB_Class_Library.ArtBTimePeriod) = GVC.PopulatePeriods(f_ROUTE_ID, nl, Today, 3, False)
                    For Each r In tlist
                        If r.MM1 = 4 And r.MM2 = 9 Then
                            Continue For
                        End If
                        If r.MM1 = 10 And r.MM2 = 3 Then
                            Continue For
                        End If

                        Dim nc As New ArtBTimePeriod
                        nc.Descr = r.Descr
                        nc.DPM = r.DPM
                        nc.EndDate = r.EndDate
                        nc.EndMonth = r.EndMonth
                        nc.MM1 = r.MM1
                        nc.MM2 = r.MM2
                        nc.ROUTE_ID = r.ROUTE_ID
                        nc.SERVER_DATE = r.SERVER_DATE
                        nc.StartDate = r.StartDate
                        nc.StartMonth = r.StartMonth
                        nc.YY1 = r.YY1
                        nc.YY2 = r.YY2
                        MVPeriods.Add(nc)
                    Next
                End If
            End If

            If ReturnAll Then
                Dim distR = (From q In MVPeriods Where q.SERVER_DATE >= Today() Select q.ROUTE_ID).Distinct.ToList
                For Each r In distR
                    Dim temp = (From q In MVPeriods Where q.ROUTE_ID = r And q.SERVER_DATE >= Today _
                                Order By q.YY2 Ascending, q.MM2 Ascending, q.YY1 Descending, q.MM1 Descending Select q).ToList
                    retval.AddRange(temp)
                Next
            Else
                retval = (From q In MVPeriods Where q.ROUTE_ID = f_ROUTE_ID And q.SERVER_DATE >= Today _
                          Order By q.YY2 Ascending, q.MM2 Ascending, q.YY1 Descending, q.MM1 Descending Select q).ToList
            End If
        End SyncLock
        DB.Dispose()
        Return retval
    End Function

    Public Function CheckCredentials(ByVal UD As DataContracts.FingerPrintClass) As DataContracts.FingerPrintClass Implements IFFAOptMain.CheckCredentials
        Dim DB As New ArtBDataContext
        Try
            Dim zip As New XMPPZipClass
            UD.OFPswd = zip.Compress(My.Settings.OFUserPswd)

            Dim fprint = (From q In DB.ARTBOPTCALC_FINGERPRINTS _
                          Where q.FINGER_PRINT = UD.MYFINGERPRINT.FINGER_PRINT _
                          And q.PRODUCT_ID = UD.PRODUCT_ID _
                          Select q).FirstOrDefault
            If IsNothing(fprint) = False Then
                fprint.COMPUTER_NAME = UD.MYFINGERPRINT.COMPUTER_NAME
                DB.SubmitChanges()
                UD.MYFINGERPRINT = fprint
                UD.License = fprint.ARTBOPTCALC_LICENSES
                UD.FingerPrints = fprint.ARTBOPTCALC_LICENSES.ARTBOPTCALC_FINGERPRINTS.ToList
                UD.PRODUCT = fprint.ARTBOPTCALC_LICENSES.ARTBOPTCALC_PRODUCTS
                'check for license status
                If fprint.ARTBOPTCALC_LICENSES.DEMO = True And fprint.DEMO_EXPIRATION_DATE < Today Then
                    UD.FPStatus = FPStatusEnum.DemoFingerPrintHasExpired
                    UD.FPMessage = "Need to ask user if he wants to purchase a license"
                ElseIf fprint.ARTBOPTCALC_LICENSES.DEMO = False And fprint.ARTBOPTCALC_LICENSES.LICENSE_EXP_DATE < Today Then
                    UD.FPStatus = FPStatusEnum.LiveLicenseHasExpiredNoFingerPrintAdded
                    UD.FPMessage = "Need to ask user if he wants to purchase a license"
                ElseIf fprint.ACTIVE = False Then
                    UD.FPStatus = FPStatusEnum.UserWantsToRegisterPC
                    UD.FPMessage = "User needs to re-register PC"
                Else
                    UD.FPStatus = FPStatusEnum.ExistingClientAllOK
                End If
            Else
                UD.PRODUCT = (From q In DB.ARTBOPTCALC_PRODUCTS Where q.PRODUCT_ID = UD.PRODUCT_ID Select q).FirstOrDefault
                UD.FPStatus = FPStatusEnum.NewClient
            End If
        Catch ex As Exception
            UD.FPStatus = FPStatusEnum.DBError
        End Try
        DB.Dispose()
        Return UD
    End Function
    Public Function CheckLicense(ByVal UD As DataContracts.FingerPrintClass) As DataContracts.FingerPrintClass Implements IFFAOptMain.CheckLicense
        Dim DB As New ArtBDataContext
        Try
            Dim license = (From q In DB.ARTBOPTCALC_LICENSES _
                           Where q.LICENSE_KEY = UD.MYFINGERPRINT.LICENSE_KEY _
                           And q.PRODUCT_ID = UD.PRODUCT_ID _
                           Select q).FirstOrDefault
            If IsNothing(license) = False Then
                UD.License = license
                UD.FingerPrints = license.ARTBOPTCALC_FINGERPRINTS.ToList
                UD.PRODUCT = (From q In DB.ARTBOPTCALC_PRODUCTS Where q.PRODUCT_ID = UD.PRODUCT_ID Select q).FirstOrDefault
                UD.FPStatus = FPStatusEnum.ValidLicenseKey
            Else
                UD.PRODUCT = (From q In DB.ARTBOPTCALC_PRODUCTS Where q.PRODUCT_ID = UD.PRODUCT_ID Select q).FirstOrDefault
                UD.FPStatus = FPStatusEnum.InvalidLicenseKey
            End If
        Catch ex As Exception
            UD.FPStatus = FPStatusEnum.DBError
        End Try
        DB.Dispose()
        Return UD
    End Function

    Public Function RegisterUser(ByVal UD As DataContracts.FingerPrintClass) As DataContracts.FingerPrintClass Implements IFFAOptMain.RegisterUser
        Dim DB As New ArtBDataContext
        Dim DBOF As New BRSOFDataContext
        Dim trans As System.Data.Common.DbTransaction = Nothing

        DB.Connection.Open()
        trans = DB.Connection.BeginTransaction()
        DB.Transaction = trans

        'check if fingerprint exists in DB, if no we are talking about a newly registered user
        Dim fprint = (From q In DB.ARTBOPTCALC_FINGERPRINTS Where _
                      q.FINGER_PRINT = UD.MYFINGERPRINT.FINGER_PRINT _
                      And q.PRODUCT_ID = UD.PRODUCT_ID Select q).FirstOrDefault
        If IsNothing(fprint) = False And UD.License.LICENSE_KEY <> "" Then 'PC user already registered under an existing license
            fprint.COMPUTER_NAME = UD.MYFINGERPRINT.COMPUTER_NAME
            fprint.LICENSE_KEY = UD.License.LICENSE_KEY
            DB.SubmitChanges()
            UD.License = fprint.ARTBOPTCALC_LICENSES
            UD.PRODUCT = fprint.ARTBOPTCALC_LICENSES.ARTBOPTCALC_PRODUCTS
            If fprint.ARTBOPTCALC_LICENSES.DEMO = True And fprint.DEMO_EXPIRATION_DATE < Today Then
                UD.MYFINGERPRINT = fprint
                UD.FingerPrints = fprint.ARTBOPTCALC_LICENSES.ARTBOPTCALC_FINGERPRINTS.ToList
                UD.FPStatus = FPStatusEnum.DemoFingerPrintHasExpired
                UD.FPMessage = "No Good"
            ElseIf fprint.ARTBOPTCALC_LICENSES.DEMO = False And fprint.ARTBOPTCALC_LICENSES.LICENSE_EXP_DATE < Today Then
                UD.MYFINGERPRINT = fprint
                UD.FingerPrints = fprint.ARTBOPTCALC_LICENSES.ARTBOPTCALC_FINGERPRINTS.ToList
                UD.FPStatus = FPStatusEnum.LiveLicenseHasExpiredNoFingerPrintAdded
                UD.FPMessage = "No Good"
            Else
                'check if licence has room for more users
                For Each r In UD.FingerPrints
                    If r.FINGER_PRINT <> fprint.FINGER_PRINT Then
                        Dim fp = (From q In fprint.ARTBOPTCALC_LICENSES.ARTBOPTCALC_FINGERPRINTS _
                                  Where q.FINGER_PRINT = r.FINGER_PRINT Select q).FirstOrDefault
                        If fp.HIDE = True And fp.ACTIVE = True Then
                            fp.LICENSE_KEY = "HIDDENPCS"
                            fp.ACTIVE = False
                            'Dim sqlstr As String = "delete from ofUSER where username=" & "'" & r.OFID & "'"
                            'DBOF.ExecuteCommand(sqlstr)
                            fprint.ARTBOPTCALC_LICENSES.USED_LICENSES -= 1
                        End If
                    End If
                Next
                DB.SubmitChanges()

                If fprint.ARTBOPTCALC_LICENSES.USED_LICENSES < fprint.ARTBOPTCALC_LICENSES.MAX_LICENSES Then
                    fprint.HIDE = False
                    fprint.ACTIVE = True
                    fprint.ARTBOPTCALC_LICENSES.USED_LICENSES += 1
                    UD.FPStatus = FPStatusEnum.ExistingFPLiveRenewed
                Else
                    fprint.ACTIVE = False
                    UD.FPStatus = FPStatusEnum.ExistingFPLiveMaxLicensesError
                End If
                DB.SubmitChanges()

                Dim OFUserName As String = UD.PRODUCT_ID & "_" & UD.MYFINGERPRINT.FINGER_PRINT
                Dim OFUserExists = (From q In DBOF.ofUser Where q.username = OFUserName Select q).FirstOrDefault
                If IsNothing(OFUserExists) = True Then 'create new OF user
                    If AddOFUserWeb(OFUserName, "@ndr0s", fprint.LICENSE_KEY) = False Then
                        trans.Rollback()
                        DB.Connection.Close()
                        DB.Dispose()
                        DBOF.Dispose()
                        UD.FPStatus = FPStatusEnum.DBError
                        UD.FPMessage = "OF User Insert Failed"
                        Return UD
                    End If
                End If

                UD.MYFINGERPRINT = fprint
                UD.License = fprint.ARTBOPTCALC_LICENSES
                UD.FingerPrints = fprint.ARTBOPTCALC_LICENSES.ARTBOPTCALC_FINGERPRINTS.ToList
                UD.PRODUCT = fprint.ARTBOPTCALC_LICENSES.ARTBOPTCALC_PRODUCTS
            End If
        ElseIf IsNothing(fprint) = True And UD.License.LICENSE_KEY <> "" Then 'new user PC registering under existing license
            Dim license = (From q In DB.ARTBOPTCALC_LICENSES Where q.LICENSE_KEY = UD.License.LICENSE_KEY Select q).FirstOrDefault
            If license.DEMO = False And license.LICENSE_EXP_DATE < Today Then
                DB.SubmitChanges()
                UD.License = license
                UD.FingerPrints = license.ARTBOPTCALC_FINGERPRINTS.ToList
                UD.PRODUCT = license.ARTBOPTCALC_PRODUCTS
                UD.FPStatus = FPStatusEnum.LiveLicenseHasExpiredNoFingerPrintAdded
                UD.FPMessage = "No Good"
            ElseIf license.DEMO = True And license.LICENSE_EXP_DATE < Today Then
                DB.SubmitChanges()
                UD.License = license
                UD.FingerPrints = license.ARTBOPTCALC_FINGERPRINTS.ToList
                UD.PRODUCT = license.ARTBOPTCALC_PRODUCTS
                UD.FPStatus = FPStatusEnum.DemoLicenseHasExpiredNoFingerPrintAdded
                UD.FPMessage = "No Good"
            Else
                Dim nc As New ARTBOPTCALC_FINGERPRINTS
                nc.LICENSE_KEY = license.LICENSE_KEY
                nc.HIDE = False
                nc.ACTIVE = True
                nc.DEMO_EXPIRATION_DATE = Today.AddMonths(3)
                nc.FINGER_PRINT = UD.MYFINGERPRINT.FINGER_PRINT
                nc.COMPUTER_NAME = UD.MYFINGERPRINT.COMPUTER_NAME
                nc.PRODUCT_ID = UD.PRODUCT_ID
                Dim OFUserName As String = UD.PRODUCT_ID & "_" & UD.MYFINGERPRINT.FINGER_PRINT
                nc.OFID = OFUserName
                Dim OFUserExists = (From q In DBOF.ofUser Where q.username = OFUserName Select q).FirstOrDefault
                If IsNothing(OFUserExists) = True Then 'create new OF user
                    If AddOFUserWeb(OFUserName, "@ndr0s", nc.LICENSE_KEY) = False Then
                        trans.Rollback()
                        DB.Connection.Close()
                        DB.Dispose()
                        DBOF.Dispose()
                        UD.FPStatus = FPStatusEnum.DBError
                        UD.FPMessage = "OF User Insert Failed"
                        Return UD
                    Else
                        nc.OFID = OFUserName
                    End If
                End If

                For Each r In UD.FingerPrints
                    If r.FINGER_PRINT <> UD.MYFINGERPRINT.FINGER_PRINT Then
                        Dim fp = (From q In license.ARTBOPTCALC_FINGERPRINTS Where q.FINGER_PRINT = r.FINGER_PRINT Select q).FirstOrDefault
                        If fp.HIDE = True Then
                            fp.LICENSE_KEY = "HIDDENPCS"
                            fp.ACTIVE = False
                            Dim sqlstr As String = "delete from ofUSER where username=" & "'" & r.OFID & "'"
                            DBOF.ExecuteCommand("sqlstr")
                            license.USED_LICENSES -= 1
                        End If
                    End If
                Next
                DB.SubmitChanges()
                If license.USED_LICENSES < license.MAX_LICENSES Then
                    license.USED_LICENSES += 1
                    DB.ARTBOPTCALC_FINGERPRINTS.InsertOnSubmit(nc)
                    UD.FPMessage = "Success"
                    UD.FPStatus = FPStatusEnum.LiveLicenseNewFingerPrintAdded
                Else
                    nc.ACTIVE = False
                    UD.FPMessage = "No Good"
                    UD.FPStatus = FPStatusEnum.ExistingFPLiveMaxLicensesError
                End If
                DB.SubmitChanges()

                UD.MYFINGERPRINT = nc
                UD.License = license
                UD.FingerPrints = license.ARTBOPTCALC_FINGERPRINTS.ToList
                UD.PRODUCT = license.ARTBOPTCALC_PRODUCTS
            End If
        ElseIf IsNothing(fprint) = True And UD.License.LICENSE_KEY = "" Then 'new user PC registering fresh demo license
            Dim NewLicenseKey = CreateLicenseKey()
            Dim license As New ARTBOPTCALC_LICENSES
            license.PRODUCT_ID = UD.PRODUCT_ID
            license.LICENSE_KEY = NewLicenseKey
            license.DEMO = True
            license.MAX_LICENSES = 1
            license.USED_LICENSES = 1
            license.LICENSE_PURCH_DATE = Today
            license.LICENSE_EXP_DATE = Today.AddMonths(3)

            license.FIRSTNAME = ""
            license.LASTNAME = ""
            license.COMPANY = ""
            license.REG_NAME = ""
            license.COUNTRY = ""
            license.EMAIL = ""
            license.PURCHASE_ID = "DEMO"
            DB.ARTBOPTCALC_LICENSES.InsertOnSubmit(license)

            Dim fp As New ARTBOPTCALC_FINGERPRINTS
            fp.LICENSE_KEY = NewLicenseKey
            fp.COMPUTER_NAME = UD.MYFINGERPRINT.COMPUTER_NAME
            fp.FINGER_PRINT = UD.MYFINGERPRINT.FINGER_PRINT
            fp.ACTIVE = True
            fp.HIDE = False
            fp.DEMO_EXPIRATION_DATE = Today.AddMonths(3)
            fp.PRODUCT_ID = UD.PRODUCT_ID
            Dim OFUserName As String = UD.PRODUCT_ID & "_" & UD.MYFINGERPRINT.FINGER_PRINT
            fp.OFID = OFUserName
            Dim OFUserExists = (From q In DBOF.ofUser Where q.username = OFUserName Select q).FirstOrDefault
            If IsNothing(OFUserExists) = True Then 'create new OF user
                If AddOFUserWeb(OFUserName, "@ndr0s", license.LICENSE_KEY) = False Then
                    trans.Rollback()
                    DB.Connection.Close()
                    DB.Dispose()
                    DBOF.Dispose()
                    UD.FPStatus = FPStatusEnum.DBError
                    UD.FPMessage = "OF User Insert Failed"
                    Return UD
                End If
            End If
            DB.ARTBOPTCALC_FINGERPRINTS.InsertOnSubmit(fp)
            DB.SubmitChanges()
            UD.License = license
            UD.MYFINGERPRINT = fp
            UD.FingerPrints = license.ARTBOPTCALC_FINGERPRINTS.ToList
            UD.PRODUCT = license.ARTBOPTCALC_PRODUCTS
            UD.FPMessage = "Success"
            UD.FPStatus = FPStatusEnum.NewClientFPTrialCreated
        End If

        trans.Commit()
        DB.Connection.Close()
        DB.Dispose()
        DBOF.Dispose()
        Return UD
    End Function
    Public Function CreateLicenseKey() As String
        Dim DB As New ArtBDataContext
        Dim rand As New Random
        Dim L As Integer = 65
        Dim U As Integer = 90
        Dim retval As String = String.Empty

        Dim ItsSafe As Boolean = False
        Do While ItsSafe = False
            retval = String.Empty
            For I = 1 To 15
                retval += Chr(rand.Next(L, U))
            Next
            Dim exists = (From q In DB.ARTBOPTCALC_LICENSES Where q.LICENSE_KEY = retval Select q).FirstOrDefault
            If IsNothing(exists) = True Then
                ItsSafe = True
            End If
        Loop
        DB.Dispose()
        Return retval
    End Function
    Public Function DeleteOFUser(ByVal OF_ID As String) As Boolean
        Dim DB As New BRSOFDataContext
        Dim answ As Boolean

        Dim sqlq As String = "delete from ofUser where username=" & "'" & OF_ID & "'"
        Try
            DB.ExecuteCommand(sqlq)
            DB.SubmitChanges()
            answ = True
        Catch ex As Exception
            answ = False
        End Try
        DB.Dispose()
        Return answ
    End Function
    Public Function DeleteOFUserWeb(ByVal OF_ID As String) As Boolean
        Dim http_STRING As String
        Dim OF_Answer As Boolean
        Dim OpenFireJID As String
        Dim secret As String = "@ndr0s"

#If DEBUG Then
        OpenFireJID = My.Settings.DebugOpenFireJID
        secret = My.Settings.DebugAddUserPswd
#Else
        OpenFireJID = My.Settings.LiveOpenFireJID
        secret = My.Settings.LiveAddUserPswd
#End If

        'http://artb.gr:9090/plugins/userService/userservice?type=delete&secret=@ndr0s&username=00102"
        Try
            http_STRING = "http://" & OpenFireJID & ":9090/plugins/userService/userservice?type=delete&secret=" & secret & "&username=" & OF_ID

            Dim HttpWReq As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(http_STRING), System.Net.HttpWebRequest)
            Dim HttpWResp As System.Net.HttpWebResponse = CType(HttpWReq.GetResponse(), System.Net.HttpWebResponse)
            Dim datastream As System.IO.Stream = HttpWResp.GetResponseStream
            Dim httpreader As New System.IO.StreamReader(datastream)
            ' Read the content.
            Dim responseFromServer As String = httpreader.ReadToEnd()
            ' Display the content.
            If responseFromServer.Contains("ok") Then
                OF_Answer = True
            Else
                OF_Answer = False
            End If
            httpreader.Close()
            datastream.Close()
            HttpWResp.Close()
        Catch ex As Exception
            OF_Answer = False
        End Try
        Return OF_Answer
    End Function
    Public Function AddOFUser(ByVal OF_ID As String) As Boolean
        Dim DB As New BRSOFDataContext
        Dim answ As Boolean
        Try
            Dim nuser As New ofUser
            nuser.username = OF_ID
            nuser.plainPassword = "@ndr0s"
            nuser.creationDate = "001233665975813"
            nuser.modificationDate = "001233665975813"
            DB.ofUser.InsertOnSubmit(nuser)
            DB.SubmitChanges()
            answ = True
        Catch ex As Exception
            answ = False
        End Try
        DB.Dispose()
        Return answ
    End Function
    Public Function AddOFUserWeb(ByVal OF_ID As String, ByVal OF_PASSWORD As String, ByVal USER_NAME As String) As Boolean
        Dim http_STRING As String
        Dim OF_Answer As Boolean
        Dim OpenFireJID As String
        Dim secret As String = "@ndr0s"

#If DEBUG Then
        OpenFireJID = My.Settings.DebugOpenFireJID
        secret = My.Settings.DebugAddUserPswd
#Else
        OpenFireJID = My.Settings.LiveOpenFireJID
        secret = My.Settings.LiveAddUserPswd
#End If

        '"http://artb.gr:9090/plugins/userService/userservice?type=add&secret=@ndr0s&username=00304&password=@ndr0s&name=Carmen Albert"

        http_STRING = "http://" & OpenFireJID & ":9090/plugins/userService/userservice?type=add&secret=" & secret & "&username=" & OF_ID
        http_STRING = http_STRING & "&password=" & OF_PASSWORD
        http_STRING = http_STRING & "&name=" & USER_NAME
        '------------------------------------------------------------------------------------------------------------
        Try
            Dim HttpWReq As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(http_STRING), System.Net.HttpWebRequest)
            Dim HttpWResp As System.Net.HttpWebResponse = CType(HttpWReq.GetResponse(), System.Net.HttpWebResponse)
            Dim datastream As System.IO.Stream = HttpWResp.GetResponseStream
            Dim httpreader As New System.IO.StreamReader(datastream)
            ' Read the content.
            Dim responseFromServer As String = httpreader.ReadToEnd()
            ' Display the content.
            If responseFromServer.Contains("ok") Then
                OF_Answer = True
            Else
                OF_Answer = False
            End If
            httpreader.Close()
            datastream.Close()
            HttpWResp.Close()
        Catch ex As Exception
            OF_Answer = False
        End Try
        Return OF_Answer
    End Function
    Public Function GetData(ByVal value As Integer) As String Implements IFFAOptMain.GetData
        Return String.Format("You entered: {0}", value)
    End Function
    Public Function GetDataUsingDataContract(ByVal composite As DataContracts.CompositeType) As DataContracts.CompositeType Implements IFFAOptMain.GetDataUsingDataContract
        If composite Is Nothing Then
            Throw New ArgumentNullException("composite")
        End If
        If composite.BoolValue Then
            composite.StringValue &= "Suffix"
        End If
        Return composite
    End Function

    Private Sub GD_AddRange(ByVal f_List As List(Of VolDataClass))
        SyncLock GDLock
            GD.AddRange(f_List)
        End SyncLock
    End Sub
    Private Sub GD_AddRange(ByVal f_List As VolDataClass)
        SyncLock GDLock
            GD.Add(f_List)
        End SyncLock
    End Sub
    Private Sub GD_RemoveOldAddNew(ByVal f_Route_ID As Integer, ByVal f_list As List(Of VolDataClass), ByVal f_RecordType As VolRecordTypeEnum)
        SyncLock GDLock
            If f_RecordType = VolRecordTypeEnum.all Then
                GD.RemoveAll(Function(x) x.ROUTE_ID = f_Route_ID)
                GD.AddRange(f_list)
            ElseIf f_RecordType = VolRecordTypeEnum.swap Then
                GD.RemoveAll(Function(x) x.ROUTE_ID = f_Route_ID And x.VolRecordType = VolRecordTypeEnum.swap)
                GD.AddRange(f_list)
            ElseIf f_RecordType = VolRecordTypeEnum.live Then
                GD.RemoveAll(Function(x) x.ROUTE_ID = f_Route_ID And x.VolRecordType = VolRecordTypeEnum.live)
                GD.AddRange(f_list)
            End If
        End SyncLock
    End Sub
    Private Sub GD_RemoveOldAddNew(ByVal f_Route_ID As Integer, ByVal f_list As VolDataClass, ByVal f_RecordType As VolRecordTypeEnum)
        SyncLock GDLock
            If f_RecordType = VolRecordTypeEnum.nspot Then
                GD.RemoveAll(Function(x) x.ROUTE_ID = f_Route_ID And x.VolRecordType = VolRecordTypeEnum.nspot)
                GD.Add(f_list)
            ElseIf f_RecordType = VolRecordTypeEnum.spot Then
                GD.RemoveAll(Function(x) x.ROUTE_ID = f_Route_ID And x.VolRecordType = VolRecordTypeEnum.spot)
                GD.Add(f_list)
            End If
        End SyncLock
    End Sub

    Public Function ServerDate() As Date Implements IFFAOptMain.ServerDate
        Return s_ServerDate()
    End Function
    Private Function s_ServerDate() As Date
        Return Today.Date
    End Function

    Public Function ServerDateTime() As DateTime Implements IFFAOptMain.ServerDateTime
        Return s_ServerDateTime()
    End Function
    Private Function s_ServerDateTime() As DateTime
        Return Now
    End Function

    Public Function GetTradeClases() As List(Of TRADE_CLASSES) Implements IFFAOptMain.GetTradeClases
        Dim DB As New ArtBDataContext
        Dim answ As List(Of TRADE_CLASSES) = (From q In DB.TRADE_CLASSES Where q.TRADE_CLASS_SHORT <> "C" Select q).ToList
        Return answ
    End Function
    Public Function GetVesselClases() As List(Of VESSEL_CLASS) Implements IFFAOptMain.GetVesselClases
        Dim DB As New ArtBDataContext
        Dim answ As List(Of VESSEL_CLASS) = (From q In DB.VESSEL_CLASS Select q).ToList
        Return answ
    End Function
    Public Function GetRoutes() As List(Of ROUTES) Implements IFFAOptMain.GetRoutes
        Dim DB As New ArtBDataContext
        Dim answ As List(Of ROUTES) = (From q In DB.ROUTES Where q.FFA_TRADED = True _
                                       Order By q.VESSEL_CLASS_ID, q.ROUTE_ID Select q).ToList
        Return answ
    End Function

    Public Function LastVolFixDate(ByVal f_ROUTE_ID As Integer) As Date Implements IFFAOptMain.LastVolFixDate
        Return LastVolFixDate(f_ROUTE_ID)
    End Function
    Private Function s_LastVolFixDate(ByVal f_ROUTE_ID As Integer) As Date
        Dim DB As New ArtBDataContext
        Dim lfd As Date
        If f_ROUTE_ID = 0 Then
            lfd = (From q In DB.BALTIC_OPTION_VOLATILITIES Select q.FIXING_DATE).Max
        Else
            lfd = (From q In DB.BALTIC_OPTION_VOLATILITIES Where q.ROUTE_ID = f_ROUTE_ID Select q.FIXING_DATE).Max
        End If
        DB.Dispose()

        Return lfd.Date
    End Function

    Public Function LastSwapFixDate(ByVal f_ROUTE_ID As Integer) As Date Implements IFFAOptMain.LastSwapFixDate
        Return s_LastSwapFixDate(f_ROUTE_ID)
    End Function
    Private Function s_LastSwapFixDate(ByVal f_ROUTE_ID As Integer) As Date
        Dim DB As New ArtBDataContext
        Dim lfd As Date
        If f_ROUTE_ID = 0 Then
            lfd = (From q In DB.BALTIC_FORWARD_RATES Select q.FIXING_DATE).Max
        Else
            lfd = (From q In DB.BALTIC_FORWARD_RATES Where q.ROUTE_ID = f_ROUTE_ID Select q.FIXING_DATE).Max
        End If
        DB.Dispose()
        Return lfd.Date
    End Function

    Public Function LastSpotFixDate(ByVal f_ROUTE_ID As Integer) As Date Implements IFFAOptMain.LastSpotFixDate
        Return s_LastSpotFixDate(f_ROUTE_ID)
    End Function
    Private Function s_LastSpotFixDate(ByVal f_ROUTE_ID As Integer) As Date
        Dim DB As New ArtBDataContext
        Dim lfd As Date
        If f_ROUTE_ID = 0 Then
            lfd = (From q In DB.BALTIC_SPOT_RATES Select q.FIXING_DATE).Max
        Else
            lfd = (From q In DB.BALTIC_SPOT_RATES _
                   Join r In DB.ROUTES On r.ROUTE_ID Equals q.ROUTE_ID _
                   Where r.ROUTE_ID = f_ROUTE_ID And q.ROUTE_ID = r.SETTL_ROUTE_ID _
                   Select q.FIXING_DATE).Max
        End If
        DB.Dispose()
        Return lfd.Date
    End Function

    Public Function RouteSpotAverage(ByVal f_ROUTE_ID As Integer, ByVal f_YY As Integer, ByVal f_MM As Integer) As Double Implements IFFAOptMain.RouteSpotAverage
        Return s_RouteSpotAverage(f_ROUTE_ID, f_YY, f_MM)
    End Function
    Private Function s_RouteSpotAverage(ByVal f_ROUTE_ID As Integer, ByVal f_YY As Integer, ByVal f_MM As Integer) As Double
        If f_ROUTE_ID = 0 Then Return 0.0#

        Dim DB As New ArtBDataContext
        Dim s_ROUTE = (From q In DB.ROUTES Where q.ROUTE_ID = f_ROUTE_ID Select q).FirstOrDefault
        Dim s_SETTLEMENT_TYPE As Integer = s_ROUTE.SETTLEMENT_TYPE
        Dim s_SETTL_ROUTE_ID As Integer = s_ROUTE.SETTL_ROUTE_ID
        Dim s_Holidays = (From q In DB.EXCHANGE_HOLIDAYS Select q.HOLIDAY).ToList
        Dim startdate As Date = DateSerial(f_YY, f_MM, 1)
        Dim enddate As Date = startdate.AddMonths(1)
        enddate = enddate.AddDays(-1)

        Dim RouteAvg As Double = 0.0#
        Dim avglist As New List(Of Double)

        Dim TempAvg As New List(Of Double)
        Select Case s_SETTLEMENT_TYPE
            Case RouteAvgTypeEnum.WholeMonth
                TempAvg = (From q In DB.BALTIC_SPOT_RATES _
                           Where q.ROUTE_ID = s_SETTL_ROUTE_ID _
                           Where q.FIXING_DATE >= startdate _
                           And q.FIXING_DATE <= enddate _
                           Order By q.FIXING_DATE Descending _
                           Select q.FIXING).ToList
            Case RouteAvgTypeEnum.LastSevenDays, RouteAvgTypeEnum.LastTenDays
                startdate = enddate
                Dim cntr As Integer = s_SETTLEMENT_TYPE
                While cntr > 0
                    If startdate.DayOfWeek > DayOfWeek.Sunday And startdate.DayOfWeek < DayOfWeek.Saturday And s_Holidays.Contains(startdate) = False Then
                        cntr -= 1
                    End If
                    If cntr > 0 Then
                        startdate = startdate.AddDays(-1)
                    End If
                End While

                TempAvg = (From q In DB.BALTIC_SPOT_RATES _
                           Where q.ROUTE_ID = s_SETTL_ROUTE_ID _
                           Where q.FIXING_DATE >= startdate _
                           And q.FIXING_DATE <= enddate _
                           Order By q.FIXING_DATE Descending _
                           Take s_SETTLEMENT_TYPE _
                           Select q.FIXING).ToList
        End Select
        Dim I As Integer = 0
        For Each r In TempAvg
            I += 1
            RouteAvg += r
        Next
        If I > 0 Then
            RouteAvg = RouteAvg / I
        Else
            RouteAvg = 0.0#
        End If
        DB.Dispose()
        Return RouteAvg
    End Function
    Private Function f_DaysInMonth(ByVal f_Date As Date) As Integer
        Dim enddate As Date = DateSerial(f_Date.Year, f_Date.Month, 1)
        enddate = enddate.AddMonths(1)
        enddate = enddate.AddDays(-1)
        Return enddate.Day
    End Function
    Private Function f_LastMonthDate(ByVal f_Date As Date) As Date
        Dim enddate As Date = DateSerial(f_Date.Year, f_Date.Month, 1)
        enddate = enddate.AddMonths(1)
        enddate = enddate.AddDays(-1)
        Return enddate
    End Function
    Private Function f_RouteSpotAverageProgressive(ByVal f_ROUTE_ID As Integer, ByVal f_START_FIXING_DATE As Date, ByVal f_END_FIXING_DATE As Date) As List(Of VolDataClass)
        Dim DB As New ArtBDataContext
        Dim ROUTE = (From q In DB.ROUTES Where q.ROUTE_ID = f_ROUTE_ID Select q).FirstOrDefault
        Dim SETTLEMENT_TYPE As Integer = ROUTE.SETTLEMENT_TYPE
        Dim SETTL_ROUTE_ID As Integer = ROUTE.SETTL_ROUTE_ID
        Dim NumberOfMonths As Integer = DateDiff(DateInterval.Month, f_START_FIXING_DATE, f_END_FIXING_DATE)
        Dim Adj_START_FIXING_DATE = DateSerial(f_START_FIXING_DATE.Year, f_START_FIXING_DATE.Month, 1)
        Dim Holidays = (From q In DB.EXCHANGE_HOLIDAYS Where q.HOLIDAY >= Adj_START_FIXING_DATE Select q.HOLIDAY).ToList
        Dim N As Integer
        Dim ReturnList As New List(Of VolDataClass)

        Dim spotdata = (From q In DB.BALTIC_SPOT_RATES _
                        Where q.ROUTE_ID = SETTL_ROUTE_ID _
                        And q.FIXING_DATE >= Adj_START_FIXING_DATE _
                        And q.FIXING_DATE <= f_END_FIXING_DATE _
                        Select q).ToList
        DB.Dispose()

        For N = NumberOfMonths To 0 Step -1
            Dim YY As Integer = f_END_FIXING_DATE.AddMonths(-N).Year
            Dim MM As Integer = f_END_FIXING_DATE.AddMonths(-N).Month
            Dim fdayslist As New List(Of Date)
            Dim LastMonthDate As Date
            If YY = f_END_FIXING_DATE.Year And MM = f_END_FIXING_DATE.Month Then
                LastMonthDate = f_END_FIXING_DATE
            Else
                LastMonthDate = f_LastMonthDate(DateSerial(YY, MM, 1))
            End If
            Dim FirstMonthDate As Date = DateSerial(YY, MM, 1)

            Dim dd As Integer
            For dd = 1 To LastMonthDate.Day
                Dim td As Date = DateSerial(YY, MM, dd)
                If td.DayOfWeek > DayOfWeek.Sunday And td.DayOfWeek < DayOfWeek.Saturday And Holidays.Contains(td) = False Then
                    fdayslist.Add(td)
                End If
            Next
            Select Case SETTLEMENT_TYPE
                Case RouteAvgTypeEnum.WholeMonth
                    'do nothing
                Case RouteAvgTypeEnum.LastSevenDays
                    Dim tlist = (From q In fdayslist Order By q Descending Select q).Take(7).ToList
                    tlist.Reverse()
                    fdayslist = tlist
                Case RouteAvgTypeEnum.LastTenDays
                    Dim tlist = (From q In fdayslist Order By q Descending Select q).Take(10).ToList
                    tlist.Reverse()
                    fdayslist = tlist
            End Select
            Dim subdatadates = (From q In spotdata Where q.FIXING_DATE >= FirstMonthDate And q.FIXING_DATE <= LastMonthDate _
                                Order By q.FIXING_DATE Select q)

            For Each sd In subdatadates
                Dim nc As New VolDataClass
                nc.VolRecordType = VolRecordTypeEnum.spot
                nc.ROUTE_ID = f_ROUTE_ID
                nc.FIXING_DATE = sd.FIXING_DATE
                nc.SPOT_PRICE = sd.FIXING
                nc.PERIOD = "SPOT"
                nc.ONLYHISTORICAL = True
                If fdayslist.Contains(sd.FIXING_DATE) Then
                    nc.FFA_PRICE = (From q In subdatadates _
                                    Where q.FIXING_DATE >= fdayslist(0) And q.FIXING_DATE <= sd.FIXING_DATE _
                                    Select q.FIXING).Average
                    nc.FFA_PRICE = CSng(nc.FFA_PRICE)
                Else
                    nc.FFA_PRICE = sd.FIXING
                End If
                ReturnList.Add(nc)
            Next
        Next
        ReturnList.RemoveAll(Function(x) x.FIXING_DATE < f_START_FIXING_DATE)

        Return ReturnList
    End Function
    Public Function LastInterestFixDate(ByVal f_CCY_ID As Integer) As Date Implements IFFAOptMain.LastInterestFixDate
        Return s_LastInterestFixDate(f_CCY_ID)
    End Function
    Private Function s_LastInterestFixDate(ByVal f_CCY_ID As Integer) As Date
        Dim DB As New ArtBDataContext
        Dim lfd As Date
        lfd = (From q In DB.INTEREST_RATES Where q.CCY_ID = f_CCY_ID Select q.FIXING_DATE).Max
        DB.Dispose()
        Return lfd.Date
    End Function
    Public Function GetHolidays() As List(Of Date) Implements IFFAOptMain.GetHolidays
        Return s_GetHolidays()
    End Function
    Private Function s_GetHolidays() As List(Of Date)
        Dim DB As New ArtBDataContext
        Dim Holidays = (From q In DB.EXCHANGE_HOLIDAYS Where q.HOLIDAY >= Today.Date Select q.HOLIDAY).ToList
        Return Holidays
    End Function
    Public Function InterestRates() As List(Of DataContracts.InterestRatesClass) Implements IFFAOptMain.InterestRates
        Return s_InterestRates()
    End Function
    Private Function s_InterestRates() As List(Of DataContracts.InterestRatesClass)
        Dim DB As New ArtBDataContext
        Dim nList As New List(Of DataContracts.InterestRatesClass)

        Dim ActiveCCY = (From q In DB.INTEREST_RATES Select q.CCY_ID).Distinct.ToList
        For Each c In ActiveCCY
            Dim lastfixingdate As Date = (From q In DB.INTEREST_RATES Where q.CCY_ID = c Select q.FIXING_DATE).Max
            Dim CCYRates = (From q In DB.INTEREST_RATES Where q.FIXING_DATE = lastfixingdate And q.CCY_ID = c Order By q.PERIOD).ToList
            For Each r In CCYRates
                Dim nItem As New DataContracts.InterestRatesClass
                nItem.CCY_ID = r.CCY_ID
                nItem.FIXING_DATE = r.FIXING_DATE
                nItem.PERIOD = r.PERIOD
                nItem.RATE = r.RATE
                nList.Add(nItem)
            Next
        Next

        DB.Dispose()
        Return nList
    End Function

    Public Function LastSwapFixing(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer) As Double Implements IFFAOptMain.LastSwapFixing
        Return s_LastSwapFixing(f_ROUTE_ID, f_YY1, f_MM1, f_YY2, f_MM2)
    End Function
    Private Function s_LastSwapFixing(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer) As Double
        Dim DB As New ArtBDataContext
        Dim Closing As Double
        Dim lfd As Date
        lfd = (From q In DB.BALTIC_FORWARD_RATES Where q.ROUTE_ID = f_ROUTE_ID Select q.FIXING_DATE).Max

        Dim Source = ((From q In DB.BALTIC_FORWARD_RATES _
                      Where q.ROUTE_ID = f_ROUTE_ID _
                      And q.FIXING_DATE = lfd _
                      Select q).Distinct).ToList
        Dim SS As New List(Of ForwardsClass)
        For Each row In Source
            Dim nc As New ForwardsClass
            nc.ROUTE_ID = row.ROUTE_ID
            nc.CMSROUTE_ID = row.CMSROUTE_ID
            nc.FIXING_DATE = row.FIXING_DATE
            nc.FIXING = row.FIXING
            nc.YY1 = CInt(row.YY1)
            nc.YY2 = CInt(row.YY2)
            nc.MM1 = CInt(row.MM1)
            nc.MM2 = CInt(row.MM2)
            SS.Add(nc)
        Next
        Dim days = From q In SS _
                   Order By q.FIXING_DATE _
                   Group By q.FIXING_DATE Into Group
        For Each UniqueDate In days
            Dim tc As New Collection
            Dim ud As Date = UniqueDate.FIXING_DATE

            'first chec if a recent trade is available
            Dim intraday = (From q In DB.TRADES_FFA _
                Where q.ROUTE_ID = f_ROUTE_ID _
                And q.ORDER_DATETIME >= Today _
                And q.TRADE_TYPE = OrderTypesEnum.FFA _
                And q.YY1 = f_YY1 _
                And q.MM1 = f_MM1 _
                And q.YY2 = f_YY2 _
                And q.MM2 = f_MM2 _
                Order By q.ORDER_DATETIME Descending _
                Select q.PRICE_TRADED).Take(1).FirstOrDefault
            If intraday > 0 Then
                Closing = intraday
            Else
                Dim frwd = From q In SS _
                           Where q.FIXING_DATE = ud _
                           Order By q.YY2, q.MM2, q.YY1, q.MM1 Descending _
                           Select q
                For Each r In frwd
                    Dim nrf As New ForwardsClass
                    nrf.CMSROUTE_ID = r.CMSROUTE_ID
                    nrf.FIXING = r.FIXING
                    nrf.FIXING_DATE = r.FIXING_DATE
                    nrf.MM1 = r.MM1
                    nrf.MM2 = r.MM2
                    nrf.PERIOD = CInt(DateAndTime.DateDiff(DateInterval.Month, r.FIXING_DATE, DateSerial(r.YY2, r.MM2, Date.DaysInMonth(r.YY2, r.MM2))))
                    nrf.REPORTDESC = r.REPORTDESC
                    nrf.ROUTE_ID = r.ROUTE_ID
                    nrf.YY1 = r.YY1
                    nrf.YY2 = r.YY2
                    nrf.KEY = Format(nrf.FIXING_DATE, "yyyMMdd") & Format(nrf.YY1, "0000") & Format(nrf.MM1, "00") & Format(nrf.YY2, "00") & Format(nrf.MM2, "00")
                    If tc.Contains(nrf.KEY) = False Then
                        tc.Add(nrf, nrf.KEY)
                    End If
                Next

                tc = f_NormalizeData(tc)
                Closing = f_ForwardRate(tc, ud, f_YY1, f_MM1, f_YY2, f_MM2)
            End If
        Next
        DB.Dispose()
        Return Closing
    End Function

    Public Function SwapData() As List(Of DataContracts.SwapDataClass) Implements IFFAOptMain.SwapData
        Return s_SwapData()
    End Function
    Private Function s_SwapData() As List(Of DataContracts.SwapDataClass)
        Dim ReturnList As New List(Of DataContracts.SwapDataClass)
        Dim DB As New ArtBDataContext

        Dim ValidRoutes = (From q In DB.ROUTES _
                           Join v In DB.VESSEL_CLASS On v.VESSEL_CLASS_ID Equals q.VESSEL_CLASS_ID _
                           Join t In DB.TRADE_CLASSES On t.TRADE_CLASS_SHORT Equals v.DRYWET _
                           Where q.FFA_TRADED = True _
                           Order By q.ROUTE_ID _
                           Select q, v, t).ToList

        For Each vr In ValidRoutes
            Dim LastSpotFixDate As Date
            Dim LastSwapFixDate As Date
            Dim LastVolFixDate As Date

            Dim nv As New DataContracts.SwapDataClass
            nv.ROUTE_ID = vr.q.ROUTE_ID
            nv.ROUTE_SHORT = vr.q.ROUTE_SHORT
            nv.TRADE_CLASS_SHORT = vr.t.TRADE_CLASS_SHORT
            nv.VESSEL_CLASS_ID = vr.v.VESSEL_CLASS_ID
            nv.CCY_ID = vr.q.CCY_ID

            LastSpotFixDate = s_LastSpotFixDate(vr.q.ROUTE_ID)
            nv.SPOT_PRICE = (From q In DB.BALTIC_SPOT_RATES Where q.ROUTE_ID = vr.q.SETTL_ROUTE_ID And q.FIXING_DATE = LastSpotFixDate Select q.FIXING).FirstOrDefault
            nv.FIXING_AVG = s_RouteSpotAverage(vr.q.SETTL_ROUTE_ID, Year(LastSpotFixDate), Month(LastSpotFixDate))
            If LastSpotFixDate.Date = s_ServerDate().Date Then
                nv.AVERAGE_INCLUDES_TODAY = True
            Else
                nv.AVERAGE_INCLUDES_TODAY = False
            End If

            LastSwapFixDate = s_LastSwapFixDate(vr.q.ROUTE_ID)
            LastVolFixDate = s_LastVolFixDate(vr.q.ROUTE_ID)

            nv.PRICING_TICK = vr.q.PRICING_TICK
            nv.FORMAT_STRING = FormatPricingTick(vr.q.PRICING_TICK, vr.q.CCY_ID)
            If Math.Log10(vr.q.PRICING_TICK) >= 0 Then
                nv.DECIMAL_PLACES = 0
            ElseIf Math.Log10(vr.q.PRICING_TICK) = -1 Then
                nv.DECIMAL_PLACES = 1
            ElseIf Math.Log10(vr.q.PRICING_TICK) = -2 Then
                nv.DECIMAL_PLACES = 2
            ElseIf Math.Log10(vr.q.PRICING_TICK) = -3 Then
                nv.DECIMAL_PLACES = 3
            ElseIf Math.Log10(vr.q.PRICING_TICK) > -2 And Math.Log10(vr.q.PRICING_TICK) < -1 Then
                nv.DECIMAL_PLACES = 2
            ElseIf Math.Log10(vr.q.PRICING_TICK) > -1 And Math.Log10(vr.q.PRICING_TICK) < 0 Then
                nv.DECIMAL_PLACES = 2
            End If
            ReturnList.Add(nv)
        Next

        DB.Dispose()
        Return ReturnList
    End Function
    Public Function ROUTE_DETAIL(ByVal f_ROUTE_ID As List(Of Integer)) As List(Of DataContracts.SwapDataClass) Implements IFFAOptMain.ROUTE_DETAIL
        Return s_ROUTE_DETAIL(f_ROUTE_ID)
    End Function
    Private Function s_ROUTE_DETAIL(ByVal f_ROUTE_ID As List(Of Integer)) As List(Of DataContracts.SwapDataClass)
        Dim ReturnList As New List(Of DataContracts.SwapDataClass)
        Dim DB As New ArtBDataContext

        Dim ValidRoutes = (From q In DB.ROUTES _
                           Join v In DB.VESSEL_CLASS On v.VESSEL_CLASS_ID Equals q.VESSEL_CLASS_ID _
                           Join t In DB.TRADE_CLASSES On t.TRADE_CLASS_SHORT Equals v.DRYWET _
                           Where f_ROUTE_ID.Contains(q.ROUTE_ID) _
                           Order By q.ROUTE_ID _
                           Select q, v, t).ToList

        For Each vr In ValidRoutes
            Dim LastSpotFixDate As Date

            Dim nv As New DataContracts.SwapDataClass
            nv.ROUTE_ID = vr.q.ROUTE_ID
            nv.ROUTE_SHORT = vr.q.ROUTE_SHORT
            nv.TRADE_CLASS_SHORT = vr.t.TRADE_CLASS_SHORT
            nv.VESSEL_CLASS_ID = vr.v.VESSEL_CLASS_ID
            nv.CCY_ID = vr.q.CCY_ID

            LastSpotFixDate = (From q In DB.BALTIC_SPOT_RATES _
                               Where q.ROUTE_ID = vr.q.SETTL_ROUTE_ID _
                               Select q.FIXING_DATE).Max
            nv.SPOT_FIXING_DATE = LastSpotFixDate
            nv.SPOT_PRICE = (From q In DB.BALTIC_SPOT_RATES Where q.ROUTE_ID = vr.q.SETTL_ROUTE_ID And q.FIXING_DATE = LastSpotFixDate Select q.FIXING).FirstOrDefault
            nv.FIXING_AVG = s_RouteSpotAverage(vr.q.SETTL_ROUTE_ID, Year(LastSpotFixDate), Month(LastSpotFixDate))
            If LastSpotFixDate.Date = Today.Date Then
                nv.AVERAGE_INCLUDES_TODAY = True
            Else
                nv.AVERAGE_INCLUDES_TODAY = False
            End If

            nv.PRICING_TICK = vr.q.PRICING_TICK
            nv.FORMAT_STRING = FormatPricingTick(vr.q.PRICING_TICK, 0)
            If Math.Log10(vr.q.PRICING_TICK) >= 0 Then
                nv.DECIMAL_PLACES = 0
            ElseIf Math.Log10(vr.q.PRICING_TICK) = -1 Then
                nv.DECIMAL_PLACES = 1
            ElseIf Math.Log10(vr.q.PRICING_TICK) = -2 Then
                nv.DECIMAL_PLACES = 2
            ElseIf Math.Log10(vr.q.PRICING_TICK) = -3 Then
                nv.DECIMAL_PLACES = 3
            ElseIf Math.Log10(vr.q.PRICING_TICK) > -2 And Math.Log10(vr.q.PRICING_TICK) < -1 Then
                nv.DECIMAL_PLACES = 2
            ElseIf Math.Log10(vr.q.PRICING_TICK) > -1 And Math.Log10(vr.q.PRICING_TICK) < 0 Then
                nv.DECIMAL_PLACES = 2
            End If
            ReturnList.Add(nv)
        Next

        DB.Dispose()
        Return ReturnList

    End Function
    Public Function GetSwapFixingsHistorical(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer, ByVal f_StartDate As Date, ByVal f_EndDate As Date) As List(Of VolDataClass) Implements IFFAOptMain.GetSwapFixingsHistorical
        Return f_GetSwapFixingsHistorical(f_ROUTE_ID, f_YY1, f_MM1, f_YY2, f_MM2, f_StartDate, f_EndDate)
    End Function
    Private Shared Function f_GetSwapFixingsHistorical(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer, ByVal f_StartDate As Date, ByVal f_EndDate As Date) As List(Of VolDataClass)
        Dim DB As New ArtBDataContext
        Dim FIXINGS As New List(Of VolDataClass)
        Dim TempFixing As Double

        Dim FixingData = (From q In DB.BALTIC_FORWARD_RATES _
                          Where q.ROUTE_ID = f_ROUTE_ID _
                          And q.FIXING_DATE >= f_StartDate And q.FIXING_DATE <= f_EndDate _
                          Order By q.FIXING_DATE, q.YY2, q.MM2, q.YY1, q.MM1 Descending Select q).Distinct.ToList

        If FixingData.Count <= 0 Then
            Return FIXINGS
        End If

        Dim DistinctDates = (From q In FixingData Order By q.FIXING_DATE Select q.FIXING_DATE).Distinct.ToList
        For Each dd In DistinctDates
            'Check First Live Data, if data exists for specified period there is no need to interpolate
            Dim DataFound = (From q In FixingData Where _
                             q.FIXING_DATE = dd _
                             And q.YY1 = f_YY1 And q.MM1 = f_MM1 _
                             And q.YY2 = f_YY2 And q.MM2 = f_MM2 _
                             Select q.FIXING).Distinct.ToList
            If DataFound.Count > 0 Then
                TempFixing = DataFound(0)
            Else
                Dim Source = ((From q In FixingData _
                               Where q.ROUTE_ID = f_ROUTE_ID _
                               And q.FIXING_DATE = dd _
                               Order By q.YY2, q.MM2, q.YY1, q.MM1 Descending _
                               Select q).Distinct).ToList

                Dim SS As New List(Of ForwardsClass)
                For Each row In Source
                    Dim nc As New ForwardsClass
                    nc.ROUTE_ID = row.ROUTE_ID
                    nc.CMSROUTE_ID = row.CMSROUTE_ID
                    nc.FIXING_DATE = row.FIXING_DATE
                    nc.FIXING = row.FIXING
                    nc.YY1 = CInt(row.YY1)
                    nc.YY2 = CInt(row.YY2)
                    nc.MM1 = CInt(row.MM1)
                    nc.MM2 = CInt(row.MM2)
                    SS.Add(nc)
                Next

                Dim tc As New Collection
                For Each r In SS
                    Dim nrf As New ForwardsClass
                    nrf.CMSROUTE_ID = r.CMSROUTE_ID
                    nrf.FIXING = r.FIXING
                    nrf.FIXING_DATE = r.FIXING_DATE
                    nrf.MM1 = r.MM1
                    nrf.MM2 = r.MM2
                    nrf.PERIOD = CInt(DateAndTime.DateDiff(DateInterval.Month, r.FIXING_DATE, DateSerial(r.YY2, r.MM2, Date.DaysInMonth(r.YY2, r.MM2))))
                    nrf.REPORTDESC = r.REPORTDESC
                    nrf.ROUTE_ID = r.ROUTE_ID
                    nrf.YY1 = r.YY1
                    nrf.YY2 = r.YY2
                    nrf.KEY = Format(nrf.FIXING_DATE, "yyyMMdd") & Format(nrf.YY1, "0000") & Format(nrf.MM1, "00") & Format(nrf.YY2, "00") & Format(nrf.MM2, "00")
                    If tc.Contains(nrf.KEY) = False Then
                        tc.Add(nrf, nrf.KEY)
                    End If
                Next
                tc = f_NormalizeData(tc)
                TempFixing = f_ForwardRate(tc, dd, f_YY1, f_MM1, f_YY2, f_MM2)
            End If

            Dim nfc As New VolDataClass
            nfc.VolRecordType = VolRecordTypeEnum.swap
            nfc.ROUTE_ID = f_ROUTE_ID
            nfc.FIXING_DATE = dd
            nfc.PERIOD = "SWAP"
            nfc.FFA_PRICE = TempFixing
            nfc.YY1 = f_YY1
            nfc.YY2 = f_YY2
            nfc.MM1 = f_MM1
            nfc.MM2 = f_MM2
            FIXINGS.Add(nfc)
        Next

        DB.Dispose()
        Return FIXINGS
    End Function
    Private Function f_GetSpotFixingsHistorical(ByVal f_ROUTE_ID As Integer, ByVal f_StartDate As Date, ByVal f_EndDate As Date) As List(Of VolDataClass)
        Dim DB As New ArtBDataContext
        Dim FIXINGS As New List(Of VolDataClass)

        Dim SettlementRoute = (From q In DB.ROUTES Where q.ROUTE_ID = f_ROUTE_ID Select q.SETTL_ROUTE_ID).FirstOrDefault

        Dim FixingData = (From q In DB.BALTIC_SPOT_RATES _
                          Where q.ROUTE_ID = SettlementRoute _
                          And q.FIXING_DATE >= f_StartDate And q.FIXING_DATE <= f_EndDate _
                          Order By q.FIXING_DATE Select q).Distinct.ToList
        DB.Dispose()

        For Each row In FixingData
            Dim nc As New VolDataClass
            nc.VolRecordType = VolRecordTypeEnum.spot
            nc.ROUTE_ID = row.ROUTE_ID
            nc.FIXING_DATE = row.FIXING_DATE
            nc.SPOT_PRICE = row.FIXING
            nc.PERIOD = "SPOT"
            FIXINGS.Add(nc)
        Next

        Return FIXINGS
    End Function

#Region "MainThread"
    Public Function SwapVolatility(ByVal f_ROUTE_ID As Integer, Optional ByVal f_TPeriod As Integer = 3, Optional ByVal f_VPeriod As Integer = 10) As List(Of VolDataClass) Implements IFFAOptMain.SwapVolatility
        Dim returnlist As New List(Of VolDataClass)
        Dim DB As New ArtBDataContext
        Dim SRID As Integer = (From q In DB.ROUTES Where q.ROUTE_ID = f_ROUTE_ID Select q.SETTL_ROUTE_ID).FirstOrDefault

        'if new swap fixings are in place rebuild
        Dim oldswap = (From q In GD Where q.ROUTE_ID = f_ROUTE_ID _
                       And q.VolRecordType = VolRecordTypeEnum.swap _
                       Order By q.FIXING_DATE Descending Select q).FirstOrDefault
        Dim newswap = (From q In DB.BALTIC_FORWARD_RATES Where q.ROUTE_ID = f_ROUTE_ID _
                       Order By q.FIXING_DATE Descending Select q).FirstOrDefault
        If IsNothing(oldswap) = True Then
            returnlist = f_SwapVolatility(f_ROUTE_ID, f_TPeriod, f_VPeriod)
            GD_RemoveOldAddNew(f_ROUTE_ID, returnlist, VolRecordTypeEnum.swap)
        ElseIf newswap.FIXING_DATE > oldswap.FIXING_DATE Then
            returnlist = f_SwapVolatility(f_ROUTE_ID, f_TPeriod, f_VPeriod)
            GD_RemoveOldAddNew(f_ROUTE_ID, returnlist, VolRecordTypeEnum.swap)
        Else
            returnlist = (From q In GD Where q.ROUTE_ID = f_ROUTE_ID And q.VolRecordType = VolRecordTypeEnum.swap Select q).ToList
        End If

        'check spot fixings
        Dim oldspot = (From q In GD Where q.ROUTE_ID = f_ROUTE_ID _
                       And (q.VolRecordType = VolRecordTypeEnum.spot Or q.VolRecordType = VolRecordTypeEnum.nspot) _
                       Order By q.FIXING_DATE Descending _
                       Select q).FirstOrDefault
        Dim newspot = (From q In DB.BALTIC_SPOT_RATES Where q.ROUTE_ID = SRID _
                       Order By q.FIXING_DATE Descending Select q).FirstOrDefault
        If IsNothing(oldspot) = True Then
            Dim nc As New VolDataClass
            nc.ROUTE_ID = f_ROUTE_ID
            nc.FIXING_DATE = newspot.FIXING_DATE
            nc.SPOT_PRICE = newspot.FIXING
            nc.PERIOD = "SPOT"
            If newspot.FIXING_DATE >= Today Then
                nc.VolRecordType = VolRecordTypeEnum.nspot
                GD_RemoveOldAddNew(f_ROUTE_ID, nc, VolRecordTypeEnum.nspot)
            Else
                nc.VolRecordType = VolRecordTypeEnum.spot
                GD_RemoveOldAddNew(f_ROUTE_ID, nc, VolRecordTypeEnum.spot)
            End If
        ElseIf newspot.FIXING_DATE > oldspot.FIXING_DATE Then
            Dim nc As New VolDataClass
            nc.ROUTE_ID = f_ROUTE_ID
            nc.FIXING_DATE = newspot.FIXING_DATE
            nc.SPOT_PRICE = newspot.FIXING
            nc.PERIOD = "SPOT"
            If newspot.FIXING_DATE >= Today Then
                nc.VolRecordType = VolRecordTypeEnum.nspot
                GD_RemoveOldAddNew(f_ROUTE_ID, nc, VolRecordTypeEnum.nspot)
            Else
                nc.VolRecordType = VolRecordTypeEnum.spot
                GD_RemoveOldAddNew(f_ROUTE_ID, nc, VolRecordTypeEnum.spot)
            End If
        End If
        DB.Dispose()

        returnlist = (From q In GD Where q.ROUTE_ID = f_ROUTE_ID Order By q.FIXING_DATE Select q).ToList
        Return returnlist
    End Function

    <MTAThread()> Private Function f_SwapVolatility(ByVal f_ROUTE_ID As Integer, Optional ByVal f_TPeriod As Integer = 3, Optional ByVal f_VPeriod As Integer = 10) As List(Of VolDataClass)

        Dim DB As New ArtBDataContext
        Dim ReturnList As New List(Of VolDataClass)
        Dim InterestRates As New List(Of InterestRatesClass)

        'STEP0: Get Suitable Interest Rates
        Dim IntRateLastFixDate = (From q In DB.INTEREST_RATES Join r In DB.ROUTES On r.CCY_ID Equals q.CCY_ID _
                                  Where r.ROUTE_ID = f_ROUTE_ID _
                                  Select q.FIXING_DATE).Max
        Dim IntRateQry = (From q In DB.INTEREST_RATES Where q.FIXING_DATE = IntRateLastFixDate Order By q.PERIOD Select q).ToList
        For Each r In IntRateQry
            Dim nc As New InterestRatesClass
            nc.CCY_ID = r.CCY_ID
            nc.FIXING_DATE = r.FIXING_DATE
            nc.PERIOD = r.PERIOD
            nc.RATE = r.RATE
            InterestRates.Add(nc)
        Next

        'STEP1: Check if volatility entries exist in the DB
        Dim SWAP_PERIODS As New List(Of VolDataClass)
        Dim LastSwapDate As Date = (From q In DB.BALTIC_FORWARD_RATES Where q.ROUTE_ID = f_ROUTE_ID Select q.FIXING_DATE).Max
        If LastSwapDate.Year < 1900 Then
            Return ReturnList
        End If
        Dim FirstSwapDate As Date = DateSerial(LastSwapDate.AddMonths(-f_TPeriod).Year, LastSwapDate.AddMonths(-f_TPeriod).Month, 1)
        Dim LastSpotDate As Date = (From q In DB.BALTIC_SPOT_RATES _
                                    Join r In DB.ROUTES On r.SETTL_ROUTE_ID Equals q.ROUTE_ID _
                                    Where r.ROUTE_ID = f_ROUTE_ID _
                                    Select q.FIXING_DATE).Max

        'construct records from baltic fixing periods
        Dim RouteMaxPeriod = (From q In DB.BALTIC_FORWARD_RATES _
                              Where q.ROUTE_ID = f_ROUTE_ID And q.FIXING_DATE = LastSwapDate _
                              Order By q.YY2 Descending, q.MM2 Descending Select q.YY2, q.MM2).FirstOrDefault
        Dim rmp As Date = DateSerial(RouteMaxPeriod.YY2, RouteMaxPeriod.MM2, 1)
        Dim DBVolatilities = GetMVPeriods(36, False)
        For Each r In DBVolatilities
            If rmp >= DateSerial(r.YY2, r.MM2, 1) Then
                Dim nc As New VolDataClass
                nc.YY1 = r.YY1
                nc.MM1 = r.MM1
                nc.YY2 = r.YY2
                nc.MM2 = r.MM2
                SWAP_PERIODS.Add(nc)
            End If
        Next


        'we are to implement parallel thread programming to save on execution time
        'example taken from http://msdn.microsoft.com/en-US/library/3dasc8as.aspx
        Dim doneEvents(SWAP_PERIODS.Count - 1) As ManualResetEvent
        Dim fArray(SWAP_PERIODS.Count - 1) As VolCalcTreadClass
        Dim ThreadCntr As Integer = 0
        For Each SwapPeriod In SWAP_PERIODS
            doneEvents(ThreadCntr) = New ManualResetEvent(False)
            Dim f = New VolCalcTreadClass(doneEvents(ThreadCntr), f_ROUTE_ID, SwapPeriod, FirstSwapDate, LastSwapDate, InterestRates, f_VPeriod)
            fArray(ThreadCntr) = f
            ThreadPool.QueueUserWorkItem(AddressOf f.ThreadPoolCallBack, ThreadCntr)
            ThreadCntr += 1
        Next
        ' Wait for all threads in pool to calculate.
        WaitHandle.WaitAll(doneEvents)
        Console.WriteLine("All calculations are complete.")

        ' Display the results. 
        For I = 0 To SWAP_PERIODS.Count - 1
            Dim f As VolCalcTreadClass = fArray(I)
            ReturnList.AddRange(f.ReturnList)
        Next

        Dim l_SPOT_PRICES As List(Of VolDataClass) = f_RouteSpotAverageProgressive(f_ROUTE_ID, FirstSwapDate, LastSpotDate)
        l_SPOT_PRICES = f_CalcSpotHistVol(l_SPOT_PRICES, f_VPeriod)
        'now we need to mke sure that spot dates reconcile with swap dates, since after e.g. dec 25th we have swap fixings but no spot fixing
        Dim n_SPOT_PRICES As New List(Of VolDataClass)
        Dim DistinctSwapDates = (From q In ReturnList Where q.VolRecordType = VolRecordTypeEnum.swap Order By q.FIXING_DATE Select q.FIXING_DATE).Distinct

        For Each dsd In DistinctSwapDates
            Dim nr As VolDataClass = (From q In l_SPOT_PRICES _
                                      Where q.FIXING_DATE <= dsd _
                                      Order By q.FIXING_DATE Descending _
                                      Select q).FirstOrDefault
            If IsNothing(nr) Then
                'do nothing
            Else
                Dim ne As VolDataClass = nr.clone
                ne.ONLYHISTORICAL = True
                ne.VolRecordType = VolRecordTypeEnum.spot
                ne.FIXING_DATE = dsd
                n_SPOT_PRICES.Add(ne)
            End If
        Next
        ReturnList.AddRange(n_SPOT_PRICES)

        If LastSpotDate > LastSwapDate Then
            Dim newspot = (From q In l_SPOT_PRICES Order By q.FIXING_DATE Descending Select q).FirstOrDefault
            If IsNothing(newspot) Then
                'do nothing
            Else
                Dim ne As VolDataClass = newspot.clone
                ne.VolRecordType = VolRecordTypeEnum.nspot
                ne.ONLYHISTORICAL = False
                ReturnList.Add(ne)
            End If
        End If

        Dim DistinctDates = (From q In ReturnList Select q.FIXING_DATE Order By FIXING_DATE).Distinct.Take(f_VPeriod).ToList
        For Each d In DistinctDates
            ReturnList.RemoveAll(Function(x) x.FIXING_DATE = d)
        Next

        DB.Dispose()
        Return ReturnList
    End Function

    Private Class VolCalcTreadClass
        Private m_ReturnList As New List(Of VolDataClass)
        Private m_doneEvent As ManualResetEvent
        Private m_ROUTE_ID As Integer, m_swapPeriod As VolDataClass, m_FirstSwapDate As Date, m_LastSwapDate As Date, m_InterestRates As List(Of InterestRatesClass), m__Vperiod As Integer

        Public ReadOnly Property ReturnList As List(Of VolDataClass)
            Get
                Return m_ReturnList
            End Get
        End Property

        Sub New(ByVal doneEvent As ManualResetEvent, _ROUTE_ID As Integer, _swapPeriod As VolDataClass, _FirstSwapDate As Date, _LastSwapDate As Date, _InterestRates As List(Of InterestRatesClass), _Vperiod As Integer)
            m_doneEvent = doneEvent
            m_ROUTE_ID = _ROUTE_ID
            m_swapPeriod = _swapPeriod
            m_FirstSwapDate = _FirstSwapDate
            m_LastSwapDate = _LastSwapDate
            m_InterestRates = _InterestRates
            m__Vperiod = _Vperiod
        End Sub

        ' Wrapper method for use with the thread pool. 
        Public Sub ThreadPoolCallBack(ByVal threadContext As Object)
            Dim threadIndex As Integer = CType(threadContext, Integer)
            m_ReturnList = Calculate(m_ROUTE_ID, m_swapPeriod, m_FirstSwapDate, m_LastSwapDate, m_InterestRates, m__Vperiod)
            m_doneEvent.Set()
        End Sub

        Public Function Calculate(ByVal f_ROUTE_ID As Integer, ByVal swapPeriod As VolDataClass, ByVal FirstSwapDate As Date, ByVal LastSwapDate As Date, ByVal InterestRates As List(Of InterestRatesClass), ByVal f_Vperiod As Integer) As List(Of VolDataClass)
            Dim DB As New ArtBDataContext
            Dim ReturnList As New List(Of VolDataClass)

            'STEP2: retreive price fixings
            Dim l_FFA_PRICES As List(Of VolDataClass) = f_GetSwapFixingsHistorical(f_ROUTE_ID, swapPeriod.YY1, swapPeriod.MM1, swapPeriod.YY2, swapPeriod.MM2, FirstSwapDate, LastSwapDate)
            'STEP3: Calculate Swap Historical Volatility
            l_FFA_PRICES = f_CalcSwapHistVol(l_FFA_PRICES, f_Vperiod)

            'STEP4: Get Historical Volatility fixings from DB
            Dim l_VOL_PRICES As List(Of ForwardsClass) = f_GetVolFixingsHistorical(f_ROUTE_ID, swapPeriod.YY1, swapPeriod.MM1, swapPeriod.YY2, swapPeriod.MM2, FirstSwapDate, LastSwapDate)
            If l_VOL_PRICES.Count > 0 Then
                For Each r In l_FFA_PRICES
                    r.IVOL = (From q In l_VOL_PRICES _
                              Where q.FIXING_DATE = r.FIXING_DATE _
                              And q.YY1 = r.YY1 And q.MM1 = r.MM1 And q.YY2 = r.YY2 And q.MM2 = r.MM2 _
                              Select q.FIXING).FirstOrDefault
                    r.ONLYHISTORICAL = False
                Next
            Else
                For Each r In l_FFA_PRICES
                    r.ONLYHISTORICAL = True
                Next
            End If

            'STEP5 Find Applicable Interest Rates
            Dim LatestFixing = (From q In l_FFA_PRICES Where q.FIXING_DATE = LastSwapDate _
                                And q.YY1 = swapPeriod.YY1 And q.MM1 = swapPeriod.MM1 And q.YY2 = swapPeriod.YY2 And q.MM2 = swapPeriod.MM2 _
                                Select q).FirstOrDefault
            LatestFixing.INTEREST_RATE = f_RiskFreeRate(InterestRates, swapPeriod.YY2, swapPeriod.MM2)

            'STEP6
            'Check if live data is available for this period
            Dim sdate As Date = Today
            Dim ndate As Date = Today.AddDays(+1)

            Dim livedata = ((From q In DB.TRADES_FFA Where q.ROUTE_ID = f_ROUTE_ID _
                             And q.ORDER_DATETIME > sdate And q.ORDER_DATETIME < ndate _
                             And q.YY1 = swapPeriod.YY1 And q.MM1 = swapPeriod.MM1 And q.YY2 = swapPeriod.YY2 And q.MM2 = swapPeriod.MM2 _
                             Order By q.ORDER_DATETIME Descending Select q.ORDER_DATETIME, PRICE = q.PRICE_TRADED).Take(1).Union _
                            (From q In DB.TRADES_FFA Where q.ROUTE_ID2 = f_ROUTE_ID _
                             And q.ORDER_DATETIME > sdate And q.ORDER_DATETIME < ndate _
                             And q.YY21 = swapPeriod.YY1 And q.MM21 = swapPeriod.MM1 And q.YY22 = swapPeriod.YY2 And q.MM22 = swapPeriod.MM2 _
                             Order By q.ORDER_DATETIME Descending Select q.ORDER_DATETIME, PRICE = q.PRICE_TRADED).Take(1)).ToList
            If livedata.Count > 0 Then
                Dim BestPrice = (From q In livedata Order By q.ORDER_DATETIME Descending Select q).Take(1).ToList
                Dim nc As New VolDataClass
                nc.VolRecordType = VolRecordTypeEnum.live
                nc.ROUTE_ID = f_ROUTE_ID
                nc.FIXING_DATE = BestPrice.Item(0).ORDER_DATETIME
                nc.FFA_PRICE = CDbl(BestPrice.Item(0).PRICE)
                nc.YY1 = swapPeriod.YY1
                nc.MM1 = swapPeriod.MM1
                nc.YY2 = swapPeriod.YY2
                nc.MM2 = swapPeriod.MM2
                nc.PERIOD = "LIVE"
                nc.INTEREST_RATE = 0.0#
                nc.ONLYHISTORICAL = False
                l_FFA_PRICES.Add(nc)
            End If
            ReturnList.AddRange(l_FFA_PRICES)

            Return ReturnList
        End Function
    End Class
#End Region

    Public Function ServiceCommands(ByVal f_Command As OptCalcServiceEnum) As OptCalcServiceEnum Implements IFFAOptMain.ServiceCommands
        Select Case f_Command
            Case OptCalcServiceEnum.SpotsUploaded
                Try
                    Dim routes = ((From q In GD Select q.ROUTE_ID).Distinct.Union _
                                  (From q In My.Settings.MarketView Select CInt(q))).Distinct.ToList
                    For Each r In routes
                        GD_RemoveOldAddNew(CInt(r), SwapVolatility(CInt(r)), VolRecordTypeEnum.spot)
                    Next
                Catch ex As Exception
                    Return OptCalcServiceEnum.ServiceError
                End Try
        End Select
        Return OptCalcServiceEnum.Success
    End Function

    Public Function QueryRouteData(ByVal f_ROUTE_ID As Integer, Optional ByVal f_TPeriod As Integer = 3, Optional ByVal f_VPeriod As Integer = 10) As List(Of VolDataClass) Implements IFFAOptMain.QueryRouteData
        Dim DB As New ArtBDataContext
        Dim returnlist As New List(Of VolDataClass)

        Dim RouteInMemory = (From q In GD Where q.ROUTE_ID = f_ROUTE_ID Select q.ROUTE_ID).FirstOrDefault
        If RouteInMemory = 0 Then
            returnlist = f_SwapVolatility(f_ROUTE_ID, f_TPeriod, f_VPeriod)
            GD_AddRange(returnlist)
        Else
            returnlist = (From q In GD Where q.ROUTE_ID = f_ROUTE_ID Select q).ToList
        End If
        Return returnlist
    End Function
    Private Shared Function f_GetVolFixingsHistorical(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer, ByVal f_StartDate As Date, ByVal f_EndDate As Date) As List(Of ForwardsClass)
        Dim DB As New ArtBDataContext
        Dim FIXINGS As New List(Of ForwardsClass)
        Dim TempFixing As Double

        Dim FixingData = (From q In DB.BALTIC_OPTION_VOLATILITIES _
                          Where q.ROUTE_ID = f_ROUTE_ID _
                          And q.FIXING_DATE >= f_StartDate And q.FIXING_DATE <= f_EndDate _
                          Order By q.FIXING_DATE, q.YY2, q.MM2, q.YY1, q.MM1 Descending Select q).Distinct.ToList
        DB.Dispose()

        If FixingData.Count = 0 Then
            Return FIXINGS
        End If

        Dim DistinctDates = (From q In FixingData Order By q.FIXING_DATE Select q.FIXING_DATE).Distinct.ToList
        For Each dd In DistinctDates
            Dim nfc As New ForwardsClass
            'if data exists for specified period there is no need to interpolate
            Dim DataFound = (From q In FixingData Where _
                             q.FIXING_DATE = dd _
                             And q.YY1 = f_YY1 And q.MM1 = f_MM1 _
                             And q.YY2 = f_YY2 And q.MM2 = f_MM2 Select q.FIXING).Distinct.ToList
            If DataFound.Count > 0 Then
                TempFixing = DataFound(0)
                nfc.ROUTE_ID = f_ROUTE_ID
                nfc.CMSROUTE_ID = ""
                nfc.FIXING_DATE = dd
                nfc.FIXING = TempFixing
                nfc.YY1 = f_YY1
                nfc.YY2 = f_YY2
                nfc.MM1 = f_MM1
                nfc.MM2 = f_MM2
                FIXINGS.Add(nfc)
                Continue For
            End If

            Dim Source = ((From q In FixingData _
                           Where q.ROUTE_ID = f_ROUTE_ID _
                           And q.FIXING_DATE = dd _
                           Order By q.YY2, q.MM2, q.YY1, q.MM1 Descending _
                           Select q).Distinct).ToList

            'if end period is larger than data available on record
            Dim YY2VolMax As Integer = (From q In Source Select q.YY2).Max
            Dim MM2VolMax As Integer = (From q In Source Where q.YY2 = YY2VolMax Select q.MM2).Max
            If f_YY2 > YY2VolMax Then
                Dim MM1VolMin = (From q In Source Where q.YY2 = YY2VolMax And q.MM2 = MM2VolMax _
                                 And q.YY1 = YY2VolMax Select q.MM1).Min
                TempFixing = (From q In Source Where q.YY2 = YY2VolMax And q.MM2 = MM2VolMax _
                              And q.YY1 = YY2VolMax And q.MM1 = MM1VolMin Select q.FIXING).FirstOrDefault
                nfc.ROUTE_ID = f_ROUTE_ID
                nfc.CMSROUTE_ID = ""
                nfc.FIXING_DATE = dd
                nfc.FIXING = TempFixing
                nfc.YY1 = f_YY1
                nfc.YY2 = f_YY2
                nfc.MM1 = f_MM1
                nfc.MM2 = f_MM2
                FIXINGS.Add(nfc)
                Continue For
            End If

            Dim SS As New Collection
            For Each row In Source
                Dim nc As New ForwardsClass
                nc.ROUTE_ID = row.ROUTE_ID
                nc.CMSROUTE_ID = row.CMSROUTE_ID
                nc.FIXING_DATE = row.FIXING_DATE
                nc.FIXING = row.FIXING
                nc.YY1 = CInt(row.YY1)
                nc.YY2 = CInt(row.YY2)
                nc.MM1 = CInt(row.MM1)
                nc.MM2 = CInt(row.MM2)
                nc.PERIOD = DateAndTime.DateDiff(DateInterval.Month, row.FIXING_DATE, DateSerial(row.YY2, row.MM2, Date.DaysInMonth(row.YY2, row.MM2)))
                nc.REPORTDESC = row.REPORTDESC
                nc.KEY = Format(row.FIXING_DATE, "yyyMMdd") & Format(row.YY1, "0000") & Format(row.MM1, "00") & Format(row.YY2, "00") & Format(row.MM2, "00")
                If SS.Contains(nc.KEY) = False Then
                    SS.Add(nc, nc.KEY)
                End If
            Next
            SS = f_NormalizeData(SS)
            TempFixing = f_PerformSpline(f_EndDate, SS, f_YY1, f_MM1, f_YY2, f_MM2)
            nfc.ROUTE_ID = f_ROUTE_ID
            nfc.CMSROUTE_ID = ""
            nfc.FIXING_DATE = dd
            nfc.FIXING = TempFixing
            nfc.YY1 = f_YY1
            nfc.YY2 = f_YY2
            nfc.MM1 = f_MM1
            nfc.MM2 = f_MM2
            FIXINGS.Add(nfc)
        Next

        Return FIXINGS
    End Function

    Private Shared Function f_CalcSwapHistVol(ByRef f_FIXINGS As List(Of VolDataClass), ByVal f_Period As Integer) As List(Of VolDataClass)
        Dim listlength As Integer = f_FIXINGS.Count

        For I = listlength - 1 To f_Period Step -1
            Dim data(f_Period) As Double
            For J = 0 To f_Period
                data(J) = f_FIXINGS.Item(I - J).FFA_PRICE
            Next
            f_FIXINGS.Item(I).HVOL = f_CalcHistoricVol(data) * 100
            f_FIXINGS.Item(I).IVOL = f_FIXINGS.Item(I).HVOL
        Next
        Return f_FIXINGS
    End Function
    Private Function f_CalcSpotHistVol(ByVal f_FIXINGS As List(Of VolDataClass), ByVal f_Period As Integer) As List(Of VolDataClass)
        Dim listlength As Integer = f_FIXINGS.Count

        For I = listlength - 1 To f_Period Step -1
            Dim data(f_Period) As Double
            For J = 0 To f_Period
                data(J) = f_FIXINGS.Item(I - J).SPOT_PRICE
            Next
            f_FIXINGS.Item(I).HVOL = f_CalcHistoricVol(data) * 100
        Next
        Return f_FIXINGS
    End Function
    Private Shared Function f_CalcHistoricVol(ByVal data As Double(), Optional ByVal entirePopulation As Boolean = False) As Double

        Dim values(data.Count - 2) As Double
        Dim J As Integer = 0
        Dim I As Integer = 0

        For I = data.Count - 1 To 1 Step -1
            values(J) = Math.Log(data(I) / data(I - 1))
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
            Return Math.Sqrt(var) * Math.Sqrt(252)
        End If

        Return 0.0#
    End Function
    Private Shared Function f_NormalizeData(ByVal tc As Collection) As Collection
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

            While monthsrem > 0
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

        f_NormalizeData = tc
    End Function
    Private Shared Function f_RiskFreeRate(ByVal f_FIXINGS As List(Of InterestRatesClass), ByVal YY As Integer, ByVal MM As Integer) As Double

        Dim nomonths As Integer = DateAndTime.DateDiff(DateInterval.Month, Today.Date, DateSerial(YY, MM, 1))
        If nomonths <= 0 Then nomonths = 1

        Dim nextrate = (From q In f_FIXINGS _
                        Where q.PERIOD >= nomonths _
                        Order By q.PERIOD _
                        Select q _
                        Take 1).FirstOrDefault

        Dim prevrate = (From q In f_FIXINGS _
                        Where q.PERIOD <= nomonths _
                        Order By q.PERIOD Descending _
                        Select q _
                        Take 1).FirstOrDefault

        Dim rincr As Double = 0.0#
        If nextrate.PERIOD > prevrate.PERIOD Then
            rincr = (nextrate.RATE - prevrate.RATE) / (nextrate.PERIOD - prevrate.PERIOD)
        End If

        f_RiskFreeRate = prevrate.RATE
        For I = prevrate.PERIOD To nomonths
            f_RiskFreeRate += rincr
        Next
    End Function
    Private Shared Function f_ForwardRate(ByVal FWDSCOL As Collection, ByVal FixDate As Date, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As Double
        Dim tc As New Collection
        Dim qr0 = From q As ForwardsClass In FWDSCOL _
                  Where q.FIXING_DATE = FixDate
                  Order By q.FIXING_DATE _
                  Select q
        For Each r In qr0
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
        f_ForwardRate = f_PerformSpline(FixDate, tc, YY1, MM1, YY2, MM2)
    End Function
    Private Function OldPerformSpline(ByVal FixDate As Date, ByVal ROUTE_FIXINGS As Collection, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As Double
        Dim n As Integer = ROUTE_FIXINGS.Count - 1
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

        Dim LastRecordInData = (From q As ForwardsClass In ROUTE_FIXINGS Select q.PERIOD).Max
        Dim LastRecord As ForwardsClass = (From q As ForwardsClass In ROUTE_FIXINGS _
                                           Where q.PERIOD = LastRecordInData _
                                           Select q).FirstOrDefault
        Dim LookingForDate As Date = DateSerial(YY2, MM2, 1)
        Dim HavingDate As Date = DateSerial(LastRecord.YY2, LastRecord.MM2, 1)
        If LookingForDate > HavingDate Then
            Return 999999999
        End If

        For Each r As ForwardsClass In ROUTE_FIXINGS
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

        OldPerformSpline = (SEP * (EndPeriod + 1) - SSP * (StartPeriod + 1)) / (EndPeriod - StartPeriod)
    End Function
    Private Shared Function f_PerformSpline(ByVal FixDate As Date, ByVal ROUTE_FIXINGS As Collection, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As Double
        Dim retval As Double
        Dim n As Integer = ROUTE_FIXINGS.Count - 1
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

        Dim OrderedData = From q As ForwardsClass In ROUTE_FIXINGS _
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
    Private Function FormatPricingTick(ByVal _PRICE_TICK As Double, Optional ByVal CCY_ID As Integer = 0) As String

        Dim ccy As String = ""

        Select Case CCY_ID
            Case 0
                ccy = ""
            Case 840
                ccy = "$"
            Case 978
                ccy = "€"
        End Select

        Dim fs As String = ""
        If CCY_ID = 0 Then
            If Math.Log10(_PRICE_TICK) >= 0 Then
                fs = "{0:N0}"
            ElseIf Math.Log10(_PRICE_TICK) = -1 Then
                fs = "{0:N1}"
            ElseIf Math.Log10(_PRICE_TICK) = -2 Then
                fs = "{0:N2}"
            ElseIf Math.Log10(_PRICE_TICK) = -3 Then
                fs = "{0:N3}"
            ElseIf Math.Log10(_PRICE_TICK) > -2 And Math.Log10(_PRICE_TICK) < -1 Then
                fs = "{0:N2}"
            ElseIf Math.Log10(_PRICE_TICK) > -1 And Math.Log10(_PRICE_TICK) < 0 Then
                fs = "{0:N2}"
            End If

        Else
            If Math.Log10(_PRICE_TICK) >= 0 Then
                fs = "{0:" & ccy & "#,##0;(" & ccy & "#,##0)}"
            ElseIf Math.Log10(_PRICE_TICK) = -1 Then
                fs = "{0:" & ccy & "#,##0.0;" & ccy & "(#,##0.0)}"
            ElseIf Math.Log10(_PRICE_TICK) = -2 Then
                fs = "{0:" & ccy & "#,##0.00;(" & ccy & "#,##0.00)}"
            ElseIf Math.Log10(_PRICE_TICK) = -3 Then
                fs = "{0:" & ccy & "#,##0.000;(" & ccy & "#,##0.000)}"
            ElseIf Math.Log10(_PRICE_TICK) > -2 And Math.Log10(_PRICE_TICK) < -1 Then
                fs = "{0:" & ccy & "#,##0.00;(" & ccy & "#,##0.00)}"
            ElseIf Math.Log10(_PRICE_TICK) > -1 And Math.Log10(_PRICE_TICK) < 0 Then
                fs = "{0:" & ccy & "#,##0.00;(" & ccy & "#,##0.00)}"
            End If
        End If
        Return fs
    End Function

    Public Function GetFFASuiteCredentials() As DataContracts.FFASuiteCredentials Implements IFFAOptMain.GetFFASuiteCredentials
        Dim nc As New DataContracts.FFASuiteCredentials
        Return nc
    End Function
    Public Function GetFFAMessageClass() As DataContracts.FFAMessage Implements IFFAOptMain.GetFFAMessageClass
        Dim nc As New DataContracts.FFAMessage
        Return nc
    End Function
    Public Function GetVolRecordTypeEnum() As DataContracts.VolRecordTypeEnum Implements IFFAOptMain.GetVolRecordTypeEnum
        Return VolRecordTypeEnum.live
    End Function
    Public Function GetMessageEnum() As DataContracts.MessageEnum Implements IFFAOptMain.GetMessageEnum
        Return MessageEnum.MarketViewUpdate
    End Function
    Public Function GetOrderTypes() As DataContracts.OrderTypesEnum Implements IFFAOptMain.GetOrderTypes
        Return OrderTypesEnum.FFA
    End Function
    Public Function GetRouteAvgTypeEnum() As DataContracts.RouteAvgTypeEnum Implements IFFAOptMain.GetRouteAvgTypeEnum
        Return RouteAvgTypeEnum.WholeMonth
    End Function
End Class
