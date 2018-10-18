Imports System.Text
Imports System.IO
Imports System.Data.Services.Client

Public Class BalticSaveFTPDataClass
    Public SaveResponse As Boolean
    Public ResponseMsg As String = ""
    Public Indices_Only As Boolean = False
    Public Settlement_Only As Boolean = False
    Private xmlFiles As List(Of BalticFTPClass.SingleFileFTPClass)

    Public Sub New(ByVal _xmlFiles As List(Of BalticFTPClass.SingleFileFTPClass), Optional ByVal _Indices_Only As Boolean = False, Optional ByVal _Settlement_Only As Boolean = False)
        xmlFiles = _xmlFiles
        Indices_Only = _Indices_Only
        Settlement_Only = _Settlement_Only
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
        For Each RemoteFile In xmlFiles
            Dim ds As New DataSet
            Dim dt As DataTable
            Dim LocalStream As MemoryStream = ConvertStringToStream(RemoteFile.FileStr)

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
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>C11_03</CMSRouteId>") Then
                    ds.ReadXmlSchema("CMSRA_SMX.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("VLCC-TCE") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SPOT.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>S7</CMSRouteId>") Or RemoteFile.FileStr.Contains("<CMSRouteId>S8</CMSRouteId>") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SMX.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>TC4-TCE</CMSRouteId>") Then
                    ds.ReadXmlSchema(Application.StartupPath & "\" & "CMSRA_SPOT.xsd")
                    dt = ds.Tables(0)
                    dt.ReadXml(LocalStream)
                    SpotData(dt)
                ElseIf RemoteFile.FileStr.Contains("<CMSRouteId>4TC_C+1Q</CMSRouteId>") Then
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
            Try
                DBW.SaveChanges()

                SaveResponse = True
            Catch ex As Exception
                SaveResponse = False
                ResponseMsg = "Error: " & ex.Message
#If DEBUG Then
                Stop
#End If
                'Exit Sub
            End Try
        Next

    End Sub

    Private Sub FFAData(ByVal dt As DataTable)
        If Indices_Only = True Then
            Exit Sub
        End If
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim ArchiveDate As Date
        Dim NextRolloverDate As Date
        Dim RouteAverage As Double
        Dim CMSRouteId As String
        Dim ReportDesc As String
        Dim t As String = ""
        Dim ROUTEID As Integer
        Dim FFA As New ArtBManageClasses.BalticFTPConstructor

        'try to catch CMSROUTE_ID that are newly introduced by Baltic
        Dim tbq = (From q In DBW.BALTIC_FTP Where q.CMSROUTE_ID >= "").ToList
        Dim nbq As New List(Of String)
        For Each r In tbq
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
        End If

        For Each r As DataRow In dt.Rows
            t = r("ArchiveDate")
            ArchiveDate = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            t = r("NextRolloverDate")
            NextRolloverDate = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            CMSRouteId = r("CMSRouteId")

            RouteAverage = r("RouteAverage")
            ReportDesc = r("ReportDesc")

            If ReportDesc <> "NR" And RouteAverage > 0 Then
                'build Period Constructor
                FFA.BDate = ArchiveDate
                FFA.CMSRouteId = CMSRouteId
                FFA.NextRolloverDate = NextRolloverDate
                FFA.ReportDesc = ReportDesc
                FFA.ConstructRecord()

                Dim qr0 = From q In DBW.BALTIC_FTP Where q.CMSROUTE_ID = CMSRouteId Select q
                If qr0.Count = 0 Then 'add reference to the database, means added new by the exchange
                    Dim q As New FFASuiteDataService.BALTIC_FTP
                Else
                    ROUTEID = qr0.First.ROUTE_ID
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' ARGHHHHHHHHHHHH the wcf service wont allow special characters like "+' to pass through
                    ' and CMSROUTE_ID could be somenting like "C4TC_+1MON"
                    ' so we need to add special lines into the web.config file of the service to allow passing through of special charactes
                    '<system.web>
                    '   <httpRuntime targetFramework="4.5" requestPathInvalidCharacters="" />
                    ' </system.web>
                    '<system.webServer> 
                    '   <security> 
                    '       <requestFiltering allowDoubleEscaping="true" /> 
                    '   </security> 
                    '</system.webServer> 
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Dim qr1 = (From q In DBW.BALTIC_FORWARD_RATES _
                               Where q.ROUTE_ID = ROUTEID _
                               And q.CMSROUTE_ID = CMSRouteId _
                               And q.FIXING_DATE = ArchiveDate _
                               Select q).SingleOrDefault
                    If IsNothing(qr1) = False Then 'update
                        qr1.NEXT_ROLLOVER_DATE = NextRolloverDate
                        qr1.FIXING = RouteAverage
                        qr1.REPORTDESC = ReportDesc
                        qr1.MM1 = FFA.MM1
                        qr1.YY1 = FFA.YY1
                        qr1.MM2 = FFA.MM2
                        qr1.YY2 = FFA.YY2
                        qr1.PERIOD = FFA.PERIOD
                        qr1.YY = FFA.YY
                        DBW.UpdateObject(qr1)
                    Else
                        Dim q As New FFASuiteDataService.BALTIC_FORWARD_RATES
                        q.ROUTE_ID = ROUTEID
                        q.CMSROUTE_ID = CMSRouteId
                        q.FIXING_DATE = ArchiveDate
                        q.NEXT_ROLLOVER_DATE = NextRolloverDate
                        q.FIXING = RouteAverage
                        q.REPORTDESC = ReportDesc
                        q.MM1 = FFA.MM1
                        q.YY1 = FFA.YY1
                        q.MM2 = FFA.MM2
                        q.YY2 = FFA.YY2
                        q.PERIOD = FFA.PERIOD
                        q.YY = FFA.YY
                        DBW.AddToBALTIC_FORWARD_RATES(q)
                    End If
                    qr1 = Nothing
                End If
            End If
        Next
    End Sub
    Private Sub SpotData(ByVal dt As DataTable)
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim ArchiveDate As Date
        Dim RouteAverage As Double
        Dim CMSRouteId As String
        Dim t As String
        Dim ROUTE_ID As Integer

        For Each r As DataRow In dt.Rows
            t = r("ArchiveDate")
            ArchiveDate = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            CMSRouteId = r("CMSRouteId")
            Dim OoopsStr As String = r("RouteAverage").ToString
            If OoopsStr = "NR" Then
                Continue For
            End If
            RouteAverage = r("RouteAverage")

            Dim qr0 = From q In DBW.BALTIC_FTP _
                      Where q.CMSROUTE_ID = CMSRouteId _
                      And q.QUALIFIER = "I" _
                      Select q
            If qr0.Count > 0 Then
                ROUTE_ID = qr0.First.ROUTE_ID
                Dim qr1 = (From q In DBW.BALTIC_SPOT_RATES _
                           Where q.ROUTE_ID = ROUTE_ID _
                           And q.FIXING_DATE = ArchiveDate _
                           Select q).SingleOrDefault
                If IsNothing(qr1) = False Then 'update
                    qr1.FIXING = RouteAverage
                    DBW.UpdateObject(qr1)
                Else
                    Dim q As New FFASuiteDataService.BALTIC_SPOT_RATES
                    q.ROUTE_ID = ROUTE_ID
                    q.FIXING_DATE = ArchiveDate
                    q.FIXING = RouteAverage
                    DBW.AddToBALTIC_SPOT_RATES(q)
                End If
            End If
        Next
    End Sub
    Private Sub FFAOptionsData(ByVal dt As DataTable)
        If Indices_Only = True Then
            Exit Sub
        End If
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim ArchiveDate As Date
        Dim NextRolloverDate As Date
        Dim RouteAverage As Double
        Dim CMSRouteId As String
        Dim ReportDesc As String
        Dim t As String
        Dim ROUTE_ID As Integer
        Dim FFA As New ArtBManageClasses.BalticFTPConstructor

        'try to catch CMSROUTE_ID that are newly introduced by Baltic
        Dim tbq = (From q In DBW.BALTIC_FTP Where q.CMSROUTE_ID >= "" Select q).ToList
        Dim nbq As New List(Of String)
        For Each r In tbq
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
        End If

        For Each r As DataRow In dt.Rows
            t = r("ArchiveDate")
            ArchiveDate = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            t = r("NextRolloverDate")
            NextRolloverDate = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            CMSRouteId = r("CMSRouteId")
            RouteAverage = r("RouteAverage")
            ReportDesc = r("ReportDesc")

            If ReportDesc <> "NR" Then
                'build Period Constructor
                FFA.BDate = ArchiveDate
                FFA.CMSRouteId = CMSRouteId
                FFA.NextRolloverDate = NextRolloverDate
                FFA.ReportDesc = ReportDesc
                FFA.ConstructRecord()

                Dim qr0 = From q In DBW.BALTIC_FTP _
                          Where q.CMSROUTE_ID = CMSRouteId _
                          Select q
                If qr0.Count > 0 Then
                    ROUTE_ID = qr0.First.ROUTE_ID
                    Dim qr1 = (From q In DBW.BALTIC_OPTION_VOLATILITIES _
                               Where q.ROUTE_ID = ROUTE_ID _
                               And q.CMSROUTE_ID = CMSRouteId _
                               And q.FIXING_DATE = ArchiveDate _
                               Select q).SingleOrDefault
                    If IsNothing(qr1) = False Then 'update
                        qr1.NEXT_ROLLOVER_DATE = NextRolloverDate
                        qr1.FIXING = RouteAverage
                        qr1.REPORTDESC = ReportDesc
                        qr1.MM1 = FFA.MM1
                        qr1.YY1 = FFA.YY1
                        qr1.MM2 = FFA.MM2
                        qr1.YY2 = FFA.YY2
                        qr1.PERIOD = FFA.PERIOD
                        qr1.YY = FFA.YY
                        DBW.UpdateObject(qr1)
                    Else
                        Dim nr As New FFASuiteDataService.BALTIC_OPTION_VOLATILITIES
                        nr.ROUTE_ID = ROUTE_ID
                        nr.CMSROUTE_ID = CMSRouteId
                        nr.FIXING_DATE = ArchiveDate
                        nr.NEXT_ROLLOVER_DATE = NextRolloverDate
                        nr.FIXING = RouteAverage
                        nr.REPORTDESC = ReportDesc
                        nr.MM1 = FFA.MM1
                        nr.YY1 = FFA.YY1
                        nr.MM2 = FFA.MM2
                        nr.YY2 = FFA.YY2
                        nr.PERIOD = FFA.PERIOD
                        nr.YY = FFA.YY
                        DBW.AddToBALTIC_OPTION_VOLATILITIES(nr)
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub SpotAvgData(ByVal dt As DataTable)
        If Settlement_Only = True Then
            Exit Sub
        End If

        Dim IndexDate As Date
        Dim IndexData As Double
        Dim TimeCharterAverage As Double
        Dim CMSIndexGroupId As String
        Dim t As String
        Dim mROUTE_ID As Integer

        For Each r As DataRow In dt.Rows
            t = r("IndexDate")
            IndexDate = Date.Parse(t.Substring(0, 4) & "-" & t.Substring(5, 2) & "-" & t.Substring(8, 2))
            CMSIndexGroupId = r("CMSIndexGroupId")
            IndexData = r("IndexData")
            TimeCharterAverage = r("TimeCharterAverage")

            'first input index data, then average TC data
            Dim indx = From q In DBW.BALTIC_FTP _
                       Where q.CMSROUTE_ID = CMSIndexGroupId _
                       Select q
            If indx.Count > 0 Then
                mROUTE_ID = indx.First.ROUTE_ID
                Try
                    Dim indx1 = (From q In DBW.BALTIC_SPOT_RATES _
                                 Where q.ROUTE_ID = mROUTE_ID _
                                 And q.FIXING_DATE = IndexDate _
                                 Select q).SingleOrDefault
                    If IsNothing(indx1) = False Then 'update
                        indx1.FIXING = IndexData
                        DBW.UpdateObject(indx1)
                    Else 'inser new record
                        Dim q As New FFASuiteDataService.BALTIC_SPOT_RATES
                        q.ROUTE_ID = mROUTE_ID
                        q.FIXING_DATE = IndexDate
                        q.FIXING = IndexData
                        DBW.AddToBALTIC_SPOT_RATES(q)
                    End If
                Catch ex As Exception
                    Stop
                End Try
            End If

            'then average TC data
            Dim atc = From q In DBW.BALTIC_FTP _
                      Where q.CMSROUTE_ID = CMSIndexGroupId & "D" _
                      Select q
            If atc.Count > 0 Then
                mROUTE_ID = atc.First.ROUTE_ID
                Dim atc1 = (From q In DBW.BALTIC_SPOT_RATES _
                            Where q.ROUTE_ID = mROUTE_ID _
                            And q.FIXING_DATE = IndexDate _
                            Select q).SingleOrDefault
                If IsNothing(atc1) = False Then 'update
                    atc1.FIXING = TimeCharterAverage
                    DBW.UpdateObject(atc1)
                Else
                    Dim q As New FFASuiteDataService.BALTIC_SPOT_RATES
                    q.ROUTE_ID = mROUTE_ID
                    q.FIXING_DATE = IndexDate
                    q.FIXING = TimeCharterAverage
                    DBW.AddToBALTIC_SPOT_RATES(q)
                End If
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
            FIXINGEM = IIf(IsDBNull(r("SettlementValueEM")), CDbl(0), r("SettlementValueEM"))
            FIXING7 = IIf(IsDBNull(r("SettlementValue7")), CDbl(0), r("SettlementValue7"))
            FIXING10 = IIf(IsDBNull(r("SettlementValue10")), CDbl(0), r("SettlementValue10"))

            'first input index data, then average TC data
            Dim qr0 = From q In DBW.BALTIC_FTP_SETTLEMENT _
                      Where q.CMSROUTE_ID = RouteID _
                      Select q
            If qr0.Count > 0 Then
                ROUTE_ID = qr0.First.ROUTE_ID
                Dim qr1 = (From q In DBW.BALTIC_MONTHLY_SETTLEMENTS _
                           Where q.ROUTE_ID = ROUTE_ID _
                           And q.FIXING_DATE = FIXING_DATE _
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
    End Sub
End Class
