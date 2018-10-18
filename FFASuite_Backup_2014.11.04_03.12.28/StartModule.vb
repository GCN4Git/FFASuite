Imports System.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Sockets
Imports System.IO
Imports agsXMPP
Imports FM
Imports System.Runtime.Serialization
Imports System.Collections.ObjectModel

Module StartModule
    Public SERVER_DATE As Date
    Public TRADE_CLASS As New List(Of FFAOptCalcService.TRADE_CLASSES)
    Public VESSEL_CLASSES As New List(Of FFAOptCalcService.VESSEL_CLASS)
    Public ROUTES As New List(Of FFAOptCalcService.ROUTES)
    Public ROUTES_DETAIL As New List(Of FFAOptCalcService.SwapDataClass)
    Public ROUTES_DETAIL_Lock As New Object
    Public INTEREST_RATES As New List(Of FFAOptCalcService.InterestRatesClass)
    Public PUBLIC_HOLIDAYS As New List(Of DateTime)
    Public FIXINGS As New List(Of FFAOptCalcService.VolDataClass)
    Public FIXINGS_Lock As New Object
    Public g_MVPeriods As New List(Of FFAOptCalcService.ArtBTimePeriod)
    Public WithEvents WEB As ArtBOptDataClass
    Public CudaEnabled As Boolean
    Public FP As New Security.FingerPrintClass
    Public UD As New FFAOptCalcService.FingerPrintClass
    Public BehindProxy As Boolean = False
#If DEBUG Then
    Public g_XMPPServerString As String = My.Settings.xmppserverdemo
    Public g_WSServerString As String = My.Settings.ws_ServerDemo
#Else
    Public g_XMPPServerString As String=My.Settings.xmppserver
    Public g_WSServerString As String = My.Settings.ws_ServerLive
#End If
    Public g_MarketViews As DataContracts.MarketViewClass = New DataContracts.MarketViewClass
    Public g_msgServiceType As MsgConnectivityServiceEnum
    Public g_DockLayoutPath As String = Application.StartupPath & "\dock.xml"
    Public BIDASK As New List(Of FFASuitePL.WP8FFAData)

    Public Function ValidateServerCertficate(ByVal sender As Object, ByVal cert As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean

        If cert.Subject.Contains("artbtrading") Then
            Return True
        ElseIf cert.Subject.Contains("artbsystems") Then
            Return True
        ElseIf My.Settings.ValidCertificates.Contains(cert.GetSerialNumberString) Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function TCPOpenPort(ByVal host As String, ByVal port As Integer) As Boolean
        Dim addr As IPAddress = CType(Dns.GetHostAddresses(host)(0), IPAddress)
        Dim client As TcpClient = New TcpClient()
        Try
            client.Connect(addr, port)
            TCPOpenPort = True
        Catch ex As SocketException
            TCPOpenPort = False
        Finally
            client.Close()
        End Try
        Return TCPOpenPort
    End Function
    Public Enum MsgConnectivityServiceEnum
        TCP
        BOSH
        WS
        Indifferent
    End Enum
End Module

Namespace DataContracts

    <DataContract()> Public Class MarketViewClass
        Private m_VIEWS As New List(Of MarketTabClass)

        <DataMember()> Public Property VIEWS As List(Of MarketTabClass)
            Get
                Return m_VIEWS
            End Get
            Set(value As List(Of MarketTabClass))
                m_VIEWS = value
            End Set
        End Property

        Public ReadOnly Property DistinctRoutes As List(Of Integer)
            Get
                Dim retlist As New List(Of Integer)
                For Each v In m_VIEWS
                    For Each r In v.ROUTES
                        If retlist.Contains(r.ROUTE_ID) = False Then
                            retlist.Add(r.ROUTE_ID)
                        End If
                    Next
                Next
                Return retlist
            End Get
        End Property

        Public Sub Load()
            If My.Settings.MarketViews = "" Then
                FirstTime()
                Save()
            Else
                m_VIEWS.Clear()
                Try
                    m_VIEWS = Json.Deserialize(Of List(Of MarketTabClass))(My.Settings.MarketViews)
                Catch ex As Exception
                    FirstTime()
                    Save()
                End Try
                If m_VIEWS.Count = 0 Then
                    FirstTime()
                    Save()
                End If
            End If
        End Sub
        Public Sub FirstTime()
            Dim routes As New List(Of Integer)
            For Each r In My.Settings.MarketViewStandard
                routes.Add(CInt(r))
            Next

            Dim nt As New MarketTabClass
            nt.NAME = My.Settings.MarketViewStandardName
            nt.WIDTH = My.Settings.MarketViewPeriodWidth
            nt.FontSize = My.Settings.MarketViewFontSize
            For Each r In routes
                Dim nr As New RouteViewClass
                nr.ROUTE_ID = r
                nr.WIDTH = My.Settings.MarketViewContractWidths
                nt.ROUTES.Add(nr)
            Next
            m_VIEWS.Add(nt)
        End Sub
        Public Sub Save()
            My.Settings.MarketViews = Json.Serialize(Of List(Of MarketTabClass))(m_VIEWS)
            My.Settings.Save()
        End Sub

        Public Function Clone(ByVal Index As Integer) As MarketTabClass
            Dim nc As New MarketTabClass
            nc.FontSize = m_VIEWS.Item(Index).FontSize
            nc.ROUTES = m_VIEWS.Item(Index).ROUTES
            nc.WIDTH = m_VIEWS.Item(Index).WIDTH
            nc.NAME = m_VIEWS.Item(Index).NAME
            Return nc
        End Function
        Public Sub Move(ByVal fromIndex As Integer, ByVal toIndex As Integer)
            Dim nc As MarketTabClass = Me.Clone(fromIndex)
            Me.VIEWS.RemoveAt(fromIndex)
            Me.VIEWS.Insert(toIndex, nc)
        End Sub
    End Class

    <DataContract()> Public Class MarketTabClass
        Private m_ROUTES As New List(Of RouteViewClass)
        Private m_WIDTH As Integer = 0
        Private m_NAME As String = String.Empty
        Private m_FontSize As Integer = 0

        <DataMember()> Public Property ROUTES As List(Of RouteViewClass)
            Get
                Return m_ROUTES
            End Get
            Set(value As List(Of RouteViewClass))
                m_ROUTES = value
            End Set
        End Property
        <DataMember()> Public Property NAME As String
            Get
                Return m_NAME
            End Get
            Set(value As String)
                m_NAME = value
            End Set
        End Property
        <DataMember()> Public Property WIDTH As Integer
            Get
                Return m_WIDTH
            End Get
            Set(value As Integer)
                m_WIDTH = value
            End Set
        End Property
        <DataMember()> Public Property FontSize As Integer
            Get
                Return m_FontSize
            End Get
            Set(value As Integer)
                m_FontSize = value
            End Set
        End Property

        Public Function Clone(ByVal Index As Integer) As RouteViewClass
            Dim nc As New RouteViewClass
            nc.ROUTE_ID = m_ROUTES.Item(Index).ROUTE_ID
            nc.WIDTH = m_ROUTES.Item(Index).WIDTH
            Return nc
        End Function
        Public Sub Move(ByVal fromIndex As Integer, ByVal toIndex As Integer)
            Dim noc As New ObservableCollection(Of RouteViewClass)
            For Each r In m_ROUTES
                noc.Add(r)
            Next
            noc.Move(fromIndex, toIndex)

            m_ROUTES.Clear()
            For Each r In noc
                m_ROUTES.Add(r)
            Next
        End Sub
    End Class

    <DataContract()> Public Class RouteViewClass
        Private m_ROUTE_ID As Integer = 0
        Private m_WIDTH As Integer = 0
        Private m_FORMAT_STRING As String = String.Empty
        Private m_PRICING_TICK As Double = 0.0#
        
        <DataMember()> Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        <DataMember()> Public Property WIDTH As Integer
            Get
                Return m_WIDTH
            End Get
            Set(value As Integer)
                m_WIDTH = value
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
End Namespace

