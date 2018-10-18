Imports System.Net
Imports System.Text
Imports System.IO

Public Class ArtBNBalticServiceUpdate
    Private SpotRates As New List(Of FFAOptCalcService.BALTIC_SPOT_RATES)
    Private ForwardRates As New List(Of FFAOptCalcService.BALTIC_FORWARD_RATES)
    Private Volatilities As New List(Of FFAOptCalcService.BALTIC_OPTION_VOLATILITIES)
    Private FFAWeeklyVolumes As New List(Of FFAOptCalcService.BALTIC_DRY_VOLUMES)
    Private xmlFiles As List(Of BalticFTPClass.SingleFileFTPClass)
    Private FTP As New List(Of FFAOptCalcService.BALTIC_FTP)
    Private FIXING_DATE As Date

    Public SaveResponse As Boolean = True
    Public ResponseMsg As New List(Of String)
    Public Indices_Only As Boolean = False
    Public Settlement_Only As Boolean = False

    Public Sub New(ByVal _FIXING_DATE As Date, ByVal _xmlFiles As List(Of BalticFTPClass.SingleFileFTPClass), Optional ByVal _Indices_Only As Boolean = False, Optional ByVal _Settlement_Only As Boolean = False)
        xmlFiles = _xmlFiles
        Indices_Only = _Indices_Only
        Settlement_Only = _Settlement_Only
        FIXING_DATE = _FIXING_DATE
        FTP.AddRange(SDB.GetContractFTP)
        For Each r In FTP
            r.CMSROUTE_ID = r.CMSROUTE_ID.Trim
        Next
    End Sub

    Public Function ConvertStringToStream(ByVal str As String) As MemoryStream
        Dim bytearray() = Encoding.ASCII.GetBytes(str)
        ConvertStringToStream = New MemoryStream(bytearray)
    End Function
    Public Function ConvertStreamToString(ByVal stream As MemoryStream) As String
        Dim reader As New StreamReader(stream)
        ConvertStreamToString = reader.ReadToEnd()
    End Function

    Public Sub PerformSave()
        SpotRates.Clear()
        ForwardRates.Clear()
        Volatilities.Clear()
        FFAWeeklyVolumes.Clear()

        For Each RemoteFile In xmlFiles
            Dim ds As New DataSet
            Dim dt As DataTable
            Dim LocalStream As MemoryStream = ConvertStringToStream(RemoteFile.FileStr)

            If Settlement_Only = True Then
                If RemoteFile.FileName.Contains("BSD_ALL") Or RemoteFile.FileName.Contains("BSD") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "BSD_ALL.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SettlementData(dt)
                End If
                Continue For
            End If

            If RemoteFile.FileName.Contains("CMSIA") Then
                ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSIA_SPOT.xsd")
                dt = ds.Tables(0)
                dt.ReadXml(LocalStream)
                SpotAvgData(dt)
            ElseIf RemoteFile.FileName.Contains("CMSRA") Then
                If RemoteFile.FileStr.Contains("P3A_03") Or RemoteFile.FileStr.Contains("TC2_37") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SPOT2.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>S11_58</CMSRouteId>") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SPOT2.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>C2</CMSRouteId>") Then
                    ds.ReadXmlSchema("CMSRA_SMX.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("VLCC-TCE") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SPOT.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>S7</CMSRouteId>") Or
                       RemoteFile.FileStr.Contains("<CMSRouteId>S8</CMSRouteId>") Or
                       RemoteFile.FileStr.Contains("<CMSRouteId>P5</CMSRouteId>") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SMX.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>TC4-TCE</CMSRouteId>") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SPOT.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>TC7-TCE</CMSRouteId>") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SPOT.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>4TC_C+1Q</CMSRouteId>") Or
                       RemoteFile.FileStr.Contains("<CMSRouteId>5TC_C+1Q</CMSRouteId>") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_FFA.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    FFAData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>TD3+1Q</CMSRouteId>") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_FFA.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    FFAData(dt)
                ElseIf RemoteFile.FileStr.Contains("IV_CTC_+1Q") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_OPTIONS.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    FFAOptionsData(dt)
                ElseIf RemoteFile.FileStr.Contains("FFAVCape_T") Then 'FFA weekly Dry Volumes
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SPOT.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    FFAVolumes(dt)
                ElseIf RemoteFile.FileStr.Contains("S7-IV") Then
                    'do nothing, this is newly introduces asian reporting file on smx
                End If
            ElseIf RemoteFile.FileName.Contains("BSD_ALL") Then
                ds.ReadXmlSchema(Application.StartupPath & "\" & "BSD_ALL.xsd")
                dt = ds.Tables(0)
                dt.ReadXml(LocalStream)
                SettlementData(dt)
            ElseIf RemoteFile.FileName.Contains("BSD") Then
                Continue For
            End If
        Next

        'now use the service to post fresh data
        If ForwardRates.Count > 0 Then
            Try
                Dim SubList = (From q In ForwardRates Order By q.ROUTE_ID Select q.ROUTE_ID).Distinct
                Dim NewList As New List(Of FFAOptCalcService.BALTIC_FORWARD_RATES)
                Dim cntr As Integer = 0
                For Each r In SubList
                    cntr += 1
                    NewList.AddRange(From q In ForwardRates Where q.ROUTE_ID = r)
                    If cntr Mod 6 = 0 Then
                        If NewList.Count > 0 Then
                            Dim rslt As Boolean = SDB.UpdateForwards(FIXING_DATE, NewList)
                            If rslt = False Then
#If DEBUG Then
                                For Each fl In NewList
                                    Debug.Print(fl.CMSROUTE_ID & ";" & fl.FIXING_DATE & ";" & fl.YY1 & ";" & fl.MM1 & ";" & fl.YY2 & ";" & fl.MM2 & ";" & fl.FIXING)
                                Next
#End If
                                SaveResponse = False
                                ResponseMsg.Add("Error Updating Forward Rates")
                            End If
                        End If
                        NewList = New List(Of FFAOptCalcService.BALTIC_FORWARD_RATES)
                    End If
                Next
                If NewList.Count > 0 Then
                    Dim rslt As Boolean = SDB.UpdateForwards(FIXING_DATE, NewList)
                    If rslt = False Then
                        SaveResponse = False
                        ResponseMsg.Add("Error Updating Forward Rates")
                    End If
                End If
            Catch ex As Exception
                SaveResponse = False
                ResponseMsg.Add("Error Updating Forward Rates")
            End Try
        End If

        If SpotRates.Count > 0 Then
            Try
                Dim droutes = (From q In SpotRates Select q.ROUTE_ID).Distinct.ToList
                Dim nlist As New List(Of FFAOptCalcService.BALTIC_SPOT_RATES)
                For Each r In droutes
                    Dim qr = (From q In SpotRates Where q.ROUTE_ID = r Select q).FirstOrDefault
                    nlist.Add(qr)
                Next
                Dim result As Boolean = SDB.UpdateSpots(FIXING_DATE, nlist)
                If result = False Then
                    SaveResponse = False
                    ResponseMsg.Add("Error Updating Spot Rates")
                End If
            Catch ex As Exception
                SaveResponse = False
                ResponseMsg.Add("Error Updating Spot Rates")
            End Try
        End If

        If Volatilities.Count > 0 Then
            Try
                Dim result As Boolean = SDB.UpdateVolatilities(FIXING_DATE, Volatilities)
                If result = False Then
                    SaveResponse = False
                    ResponseMsg.Add("Error Updating Option Volatilities")
                End If
            Catch ex As Exception
                SaveResponse = False
                ResponseMsg.Add("Error Updating Option Volatilities")
            End Try
        End If

        If FFAWeeklyVolumes.Count > 0 Then
            Try
                Dim result As Boolean = SDB.UpdateWeeklyVolumes(FIXING_DATE, FFAWeeklyVolumes)
                If result = False Then
                    SaveResponse = False
                    ResponseMsg.Add("Error Updating Weekly FFA Volumes")
                End If
            Catch ex As Exception
                SaveResponse = False
                ResponseMsg.Add("Error Updating Weekly FFA Volumes")
            End Try
        End If
    End Sub

    Private Sub FFAData(ByVal dt As DataTable)
        If Indices_Only = True Then
            Exit Sub
        End If
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim m_FIXING_DATE As Date
        Dim m_NEXT_ROLLOVER_DATE As Date
        Dim m_FIXING As Double
        Dim m_CMSROUTE_ID As String
        Dim m_REPORTDESC As String
        Dim t As String = ""
        Dim m_ROUTE_ID As Integer
        Dim FFA As New ArtBManageClasses.BalticFTPConstructor

        'try to catch CMSROUTE_ID that are newly introduced by Baltic
        Dim nbq As New List(Of String)
        For Each r In FTP
            nbq.Add(r.CMSROUTE_ID)
        Next
        Dim ndq = (From q In dt Select q.Item("CMSRouteId")).ToList
        Dim PossibleExceptionList As New List(Of String)
        For Each r In ndq
            If nbq.Contains(r) = False Then
                PossibleExceptionList.Add(r)
            End If
        Next
        If PossibleExceptionList.Count > 0 Then
            Dim f As New BALTICFTPNEWRECORD
            f.CMSROUTE_ID = PossibleExceptionList
            f.ShowDialog()
            FTP.Clear()
            FTP.AddRange(SDB.GetContractFTP)
        End If

        For Each r As DataRow In dt.Rows
            t = r("ArchiveDate")
            m_FIXING_DATE = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            t = r("NextRolloverDate")
            Try
                m_NEXT_ROLLOVER_DATE = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            Catch ex As Exception

            End Try
            m_CMSROUTE_ID = r("CMSRouteId")
            m_FIXING = Val(r("RouteAverage"))
            m_REPORTDESC = r("ReportDesc")

            If m_REPORTDESC <> "NR" Then
                If r("RouteAverage") <> "" Then
                    'build Period Constructor
                    FFA.BDate = m_FIXING_DATE
                    FFA.CMSRouteId = m_CMSROUTE_ID
                    FFA.NextRolloverDate = m_NEXT_ROLLOVER_DATE
                    FFA.ReportDesc = m_REPORTDESC
                    FFA.ConstructRecord()

                    Dim recFTP = (From q In FTP Where q.CMSROUTE_ID = m_CMSROUTE_ID And q.QUALIFIER = "F" Select q).FirstOrDefault
                    If IsNothing(recFTP) = False Then
                        m_ROUTE_ID = recFTP.ROUTE_ID

                        Dim q As New FFAOptCalcService.BALTIC_FORWARD_RATES
                        q.ROUTE_ID = m_ROUTE_ID
                        q.CMSROUTE_ID = m_CMSROUTE_ID
                        q.FIXING_DATE = m_FIXING_DATE
                        q.NEXT_ROLLOVER_DATE = m_NEXT_ROLLOVER_DATE
                        q.FIXING = m_FIXING
                        q.REPORTDESC = m_REPORTDESC
                        q.MM1 = FFA.MM1
                        q.YY1 = FFA.YY1
                        q.MM2 = FFA.MM2
                        q.YY2 = FFA.YY2
                        q.PERIOD = FFA.PERIOD
                        q.YY = FFA.YY
                        'introduced because on 20130916 baltic reported twice entries for CMSROUTE_ID
                        Dim ifexists = (From z In ForwardRates Where z.CMSROUTE_ID = m_CMSROUTE_ID Select z.CMSROUTE_ID).FirstOrDefault
                        If IsNothing(ifexists) Then
                            ForwardRates.Add(q)
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub SpotData(ByVal dt As DataTable)
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim m_FIXING_DATE As Date
        Dim m_FIXING As Double
        Dim m_CMSROUTE_ID As String
        Dim t As String
        Dim m_ROUTE_ID As Integer

        For Each r As DataRow In dt.Rows
            t = r("ArchiveDate")
            m_FIXING_DATE = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            m_CMSROUTE_ID = r("CMSRouteId")
            Dim OoopsStr As String = r("RouteAverage").ToString
            If OoopsStr = "NR" Or r("RouteAverage") = "" Then
                Continue For
            End If

            m_FIXING = Val(r("RouteAverage"))
            Dim recFTP = (From q In FTP Where q.CMSROUTE_ID = m_CMSROUTE_ID And q.QUALIFIER = "I" Select q).FirstOrDefault
            If IsNothing(recFTP) = False And r("RouteAverage") <> "" Then
                m_ROUTE_ID = recFTP.ROUTE_ID
                Dim q As New FFAOptCalcService.BALTIC_SPOT_RATES
                q.ROUTE_ID = m_ROUTE_ID
                q.FIXING_DATE = m_FIXING_DATE
                q.FIXING = m_FIXING
                SpotRates.Add(q)
            End If
        Next
    End Sub
    Private Sub FFAVolumes(ByVal dt As DataTable)
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim m_FIXING_DATE As Date
        Dim m_FIXING As Double
        Dim m_CMSROUTE_ID As String
        Dim t As String

        Dim VolData As New FFAOptCalcService.BALTIC_DRY_VOLUMES

        For Each r As DataRow In dt.Rows
            t = r("ArchiveDate")
            m_FIXING_DATE = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            m_CMSROUTE_ID = r("CMSRouteId")
            Dim OoopsStr As String = r("RouteAverage").ToString
            If OoopsStr = "NR" Or r("RouteAverage") = "" Then
                Continue For
            End If

            m_FIXING = Val(r("RouteAverage"))

            Select Case m_CMSROUTE_ID
                Case "FFAVCape_T"
                    VolData.FIXING_DATE = m_FIXING_DATE
                    VolData.FFAVCape_T = m_FIXING
                Case "FFACapeOI"
                    VolData.FIXING_DATE = m_FIXING_DATE
                    VolData.FFACapeOI = m_FIXING
                Case "FFAVPmx_T"
                    VolData.FIXING_DATE = m_FIXING_DATE
                    VolData.FFAVPmx_T = m_FIXING
                Case "FFAPmxOI"
                    VolData.FIXING_DATE = m_FIXING_DATE
                    VolData.FFAPmxOI = m_FIXING
                Case "FFAVSupr_T"
                    VolData.FIXING_DATE = m_FIXING_DATE
                    VolData.FFAVSupr_T = m_FIXING
                Case "FFASuprOI"
                    VolData.FIXING_DATE = m_FIXING_DATE
                    VolData.FFASuprOI = m_FIXING
                Case "FFAVHS_T"
                    VolData.FIXING_DATE = m_FIXING_DATE
                    VolData.FFAVHS_T = m_FIXING
                Case "FFAHsOI"
                    VolData.FIXING_DATE = m_FIXING_DATE
                    VolData.FFAHsOI = m_FIXING
                Case Else
                    Continue For
            End Select
        Next

        Dim ifexists = (From z In FFAWeeklyVolumes Where z.FIXING_DATE = VolData.FIXING_DATE Select z).FirstOrDefault
        If IsNothing(ifexists) Then
            FFAWeeklyVolumes.Add(VolData)
        End If
    End Sub
    Private Sub FFAOptionsData(ByVal dt As DataTable)
        If Indices_Only = True Then
            Exit Sub
        End If
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim m_FIXING_DATE As Date
        Dim m_NEXT_ROLLOVER_DATE As Date
        Dim m_FIXING As Double
        Dim m_CMSROUTE_ID As String
        Dim m_ROUTE_ID As Integer
        Dim ReportDesc As String
        Dim t As String
        Dim FFA As New ArtBManageClasses.BalticFTPConstructor

        'try to catch CMSROUTE_ID that are newly introduced by Baltic
        Dim nbq As New List(Of String)
        For Each r In FTP
            nbq.Add(r.CMSROUTE_ID)
        Next
        Dim ndq = (From q In dt Select q.Item("CMSRouteId")).ToList
        Dim PossibleExceptionList As New List(Of String)
        For Each r In ndq
            If nbq.Contains(r) = False Then
                PossibleExceptionList.Add(r)
            End If
        Next
        If PossibleExceptionList.Count > 0 Then
            Dim f As New BALTICFTPNEWRECORD
            f.CMSROUTE_ID = PossibleExceptionList
            f.ShowDialog()
            FTP.Clear()
            FTP.AddRange(SDB.GetContractFTP)
        End If

        For Each r As DataRow In dt.Rows
            t = r("ArchiveDate")
            m_FIXING_DATE = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            t = r("NextRolloverDate")
            Try
                m_NEXT_ROLLOVER_DATE = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            Catch ex As Exception

            End Try
            m_CMSROUTE_ID = r("CMSRouteId")
            m_FIXING = Val(r("RouteAverage"))
            ReportDesc = r("ReportDesc")

            If ReportDesc <> "NR" Then
                If r("RouteAverage") <> "" Then
                    'build Period Constructor
                    FFA.BDate = m_FIXING_DATE
                    FFA.CMSRouteId = m_CMSROUTE_ID
                    FFA.ReportDesc = ReportDesc
                    FFA.ConstructRecord()

                    Dim recFTP = (From q In FTP Where q.CMSROUTE_ID = m_CMSROUTE_ID And q.QUALIFIER = "V" Select q).FirstOrDefault
                    If IsNothing(recFTP) = False Then
                        m_ROUTE_ID = recFTP.ROUTE_ID

                        Dim nr As New FFAOptCalcService.BALTIC_OPTION_VOLATILITIES
                        nr.ROUTE_ID = m_ROUTE_ID
                        nr.CMSROUTE_ID = m_CMSROUTE_ID
                        nr.FIXING_DATE = m_FIXING_DATE
                        nr.NEXT_ROLLOVER_DATE = m_NEXT_ROLLOVER_DATE
                        nr.FIXING = m_FIXING
                        nr.REPORTDESC = ReportDesc
                        nr.MM1 = FFA.MM1
                        nr.YY1 = FFA.YY1
                        nr.MM2 = FFA.MM2
                        nr.YY2 = FFA.YY2
                        nr.PERIOD = FFA.PERIOD
                        nr.YY = FFA.YY
                        'introduced because on 20130927 baltic reported twice entries for CMSROUTE_ID
                        Dim ifexists = (From z In Volatilities Where z.CMSROUTE_ID = m_CMSROUTE_ID Select z.CMSROUTE_ID).FirstOrDefault
                        If IsNothing(ifexists) Then
                            Volatilities.Add(nr)
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub SpotAvgData(ByVal dt As DataTable)
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim m_FIXING_DATE As Date
        Dim IndexData As Double
        Dim TimeCharterAverage As Double
        Dim CMSIndexGroupId As String
        Dim t As String
        Dim m_ROUTE_ID As Integer

        For Each r As DataRow In dt.Rows
            t = r("IndexDate")
            m_FIXING_DATE = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            CMSIndexGroupId = r("CMSIndexGroupId")
            IndexData = Val(r("IndexData"))
            TimeCharterAverage = Val(r("TimeCharterAverage"))

            'first input index data, then average TC data
            Dim recFTP1 = (From q In FTP Where q.CMSROUTE_ID = CMSIndexGroupId And q.QUALIFIER = "I" Select q).FirstOrDefault
            If IsNothing(recFTP1) = False And r("IndexData") <> "" Then
                m_ROUTE_ID = recFTP1.ROUTE_ID

                Dim q As New FFAOptCalcService.BALTIC_SPOT_RATES
                q.ROUTE_ID = m_ROUTE_ID
                q.FIXING_DATE = m_FIXING_DATE
                q.FIXING = IndexData
                SpotRates.Add(q)
            End If

            'then average TC data
            Dim t_CMSROUTE_ID As String = CMSIndexGroupId & "D"
            Dim recFTP2 = (From q In FTP Where q.CMSROUTE_ID = t_CMSROUTE_ID And q.QUALIFIER = "I" Select q).FirstOrDefault
            If IsNothing(recFTP2) = False And r("TimeCharterAverage") <> "" Then
                m_ROUTE_ID = recFTP2.ROUTE_ID

                Dim q As New FFAOptCalcService.BALTIC_SPOT_RATES
                q.ROUTE_ID = m_ROUTE_ID
                q.FIXING_DATE = m_FIXING_DATE
                q.FIXING = TimeCharterAverage
                SpotRates.Add(q)
            End If
        Next
    End Sub
    Private Sub SettlementData(ByVal dt As DataTable)
        If Indices_Only = True Then
            Exit Sub
        End If

        Dim RouteID As String
        Dim ROUTE_ID As Integer
        Dim FIXING_DATE As Date
        Dim FIXINGEM As Double
        Dim FIXING7 As Double
        Dim FIXING10 As Double
        Dim t As String

        For Each r As DataRow In dt.Rows
            RouteID = r("RouteId")
            t = r("SettlementDate")
            FIXING_DATE = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            'FIXINGEM = IIf(IsDBNull(r("SettlementValueEM")), CDbl(0), r("SettlementValueEM"))
            'FIXING7 = IIf(IsDBNull(r("SettlementValue7")), CDbl(0), r("SettlementValue7"))
            'FIXING10 = IIf(IsDBNull(r("SettlementValue10")), CDbl(0), r("SettlementValue10"))
            Try
                FIXINGEM = Val(r("SettlementValueEM"))
            Catch ex As Exception
                FIXINGEM = 0.0#
            End Try
            Try
                FIXING7 = Val(r("SettlementValue7"))
            Catch ex As Exception
                FIXING7 = 0.0#
            End Try
            Try
                FIXING10 = Val(r("SettlementValue10"))
            Catch ex As Exception
                FIXING10 = 0.0#
            End Try
            'first input index data, then average TC data
            Dim qr0 = From q In DBW.BALTIC_FTP_SETTLEMENT
                      Where q.CMSROUTE_ID = RouteID
                      Select q
            If qr0.Count > 0 Then
                ROUTE_ID = qr0.First.ROUTE_ID
                Dim qr1 = (From q In DBW.BALTIC_MONTHLY_SETTLEMENTS
                           Where q.ROUTE_ID = ROUTE_ID _
                           And q.FIXING_DATE = FIXING_DATE
                           Select q).SingleOrDefault
                If IsNothing(qr1) = False Then 'update
                    qr1.FIXINGEM = FIXINGEM
                    qr1.FIXING7 = FIXING7
                    qr1.FIXING10 = FIXING10
                    qr1.FIXING_MONTH = FIXING_DATE.Month
                    qr1.FIXING_YEAR = FIXING_DATE.Year
                    DBW.UpdateObject(qr1)
                Else 'inser new record
                    Dim nr As New FFASuiteDataService.BALTIC_MONTHLY_SETTLEMENTS
                    nr.ROUTE_ID = ROUTE_ID
                    nr.FIXING_DATE = FIXING_DATE
                    nr.FIXINGEM = FIXINGEM
                    nr.FIXING7 = FIXING7
                    nr.FIXING10 = FIXING10
                    nr.FIXING_MONTH = FIXING_DATE.Month
                    nr.FIXING_YEAR = FIXING_DATE.Year
                    DBW.AddToBALTIC_MONTHLY_SETTLEMENTS(nr)
                End If
            End If
        Next
        Try
            DBW.SaveChanges()
            SaveResponse = True
        Catch ex As Exception
            SaveResponse = False
#If DEBUG Then
            Stop
#End If
        End Try
    End Sub

    Public Enum SelectFTPSaveEnum
        Indices
        Swaps
        Settlemets
    End Enum
End Class

