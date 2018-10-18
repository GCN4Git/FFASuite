Imports System.Runtime.Serialization

Namespace DataContracts

    <DataContract()> Public Class FingerPrintClass
        Private m_MYFINGERPRINT As New ARTBOPTCALC_FINGERPRINTS
        Private m_FingerPrints As New List(Of ARTBOPTCALC_FINGERPRINTS)
        Private m_FPStatus As FPStatusEnum
        Private m_FPMessage As String = String.Empty
        Private m_License As New ARTBOPTCALC_LICENSES
        Private m_PRODUCT_ID As String
        Private m_PRODUCT As New ARTBOPTCALC_PRODUCTS
        Private m_OFPswd As String

        <DataMember()> Public Property OFPswd As String
            Get
                Return m_OFPswd
            End Get
            Set(value As String)
                m_OFPswd = value
            End Set
        End Property
        <DataMember()> Public Property FPMessage As String
            Get
                Return m_FPMessage
            End Get
            Set(value As String)
                m_FPMessage = value
            End Set
        End Property
        <DataMember()> Public Property PRODUCT As ARTBOPTCALC_PRODUCTS
            Get
                Return m_PRODUCT
            End Get
            Set(value As ARTBOPTCALC_PRODUCTS)
                m_PRODUCT = value
            End Set
        End Property
        <DataMember()> Public Property PRODUCT_ID As String
            Get
                Return m_PRODUCT_ID
            End Get
            Set(value As String)
                m_PRODUCT_ID = value
            End Set
        End Property
        <DataMember()> Public Property MYFINGERPRINT As ARTBOPTCALC_FINGERPRINTS
            Get
                Return m_MYFINGERPRINT
            End Get
            Set(value As ARTBOPTCALC_FINGERPRINTS)
                m_MYFINGERPRINT = value
            End Set
        End Property
        <DataMember()> Public Property FingerPrints As List(Of ARTBOPTCALC_FINGERPRINTS)
            Get
                Return m_FingerPrints
            End Get
            Set(value As List(Of ARTBOPTCALC_FINGERPRINTS))
                m_FingerPrints = value
            End Set
        End Property
        <DataMember> Public Property License As ARTBOPTCALC_LICENSES
            Get
                Return m_License
            End Get
            Set(value As ARTBOPTCALC_LICENSES)
                m_License = value
            End Set
        End Property
        <DataMember> Public Property FPStatus As FPStatusEnum
            Get
                Return m_FPStatus
            End Get
            Set(value As FPStatusEnum)
                m_FPStatus = value
            End Set
        End Property
    End Class

    <DataContract> _
    Public Class FFASuiteCredentials
        Private m_FINGERPRINT As String = String.Empty
        Private m_PRODUCT_ID As String = String.Empty
        Private m_ANSWER As FPStatusEnum
        Private m_TimeStamp As DateTime = Now
        Private m_SubscribedRoutes As New List(Of Integer)

        <DataMember> _
        Public Property SubscribedRoutes As List(Of Integer)
            Get
                Return m_SubscribedRoutes
            End Get
            Set(value As List(Of Integer))
                m_SubscribedRoutes = value
            End Set
        End Property
        <DataMember> _
        Public Property FINGERPRINT As String
            Get
                Return m_FINGERPRINT
            End Get
            Set(value As String)
                m_FINGERPRINT = value
            End Set
        End Property

        <DataMember> _
        Public Property PRODUCT_ID As String
            Get
                Return m_PRODUCT_ID
            End Get
            Set(value As String)
                m_PRODUCT_ID = value
            End Set
        End Property

        <DataMember> _
        Public Property ANSWER As FPStatusEnum
            Get
                Return m_ANSWER
            End Get
            Set(value As FPStatusEnum)
                m_ANSWER = value
            End Set
        End Property

        <DataMember> _
        Public Property TimeStamp As DateTime
            Get
                Return m_TimeStamp
            End Get
            Set(value As DateTime)
                m_TimeStamp = value
            End Set
        End Property
    End Class

    <DataContract> _
    Public Class ChatMessage
        <DataMember> _
        Public Property Username As String

        <DataMember> _
        Public Property Text As String

        <DataMember> _
        Public Property Timestamp As DateTime
    End Class

    <DataContract> _
    Public Class FFAMessage
        Private m_UserName As String
        Private m_MsgType As MessageEnum
        Private m_Payload As String
        Private m_TimeStamp As DateTime

        <DataMember> _
        Public Property Username As String
            Get
                Return m_UserName
            End Get
            Set(value As String)
                m_UserName = value
            End Set
        End Property

        <DataMember> _
        Public Property MsgType As MessageEnum
            Get
                Return m_MsgType
            End Get
            Set(value As MessageEnum)
                m_MsgType = value
            End Set
        End Property

        <DataMember> _
        Public Property Payload As String
            Get
                Return m_Payload
            End Get
            Set(value As String)
                m_Payload = value
            End Set
        End Property

        <DataMember> _
        Public Property TimeStamp As DateTime
            Get
                Return m_TimeStamp
            End Get
            Set(value As DateTime)
                m_TimeStamp = value
            End Set
        End Property
    End Class

    <DataContract(Name:="MessageEnum")> _
    Public Enum MessageEnum
        <EnumMember> MarketViewUpdate
        <EnumMember> TradeAnnouncement
        <EnumMember> SpotRatesUpdate
        <EnumMember> SwapRatesUpdate
        <EnumMember> CloseClient
        <EnumMember> AmmendReportedTrade
    End Enum

    <DataContract()> Public Class VolDataClass
        Private m_VolRecordType As VolRecordTypeEnum
        Private m_ROUTE_ID As Integer
        Private m_ROUTE_ID2 As Integer
        Private m_FIXING_DATE As Date
        Private m_PERIOD As String
        Private m_FFA_PRICE As Double
        Private m_SPOT_PRICE As Double
        Private m_IVOL As Double
        Private m_HVOL As Double
        Private m_INTEREST_RATE As Double
        Private m_YY1 As Integer
        Private m_YY2 As Integer
        Private m_MM1 As Integer
        Private m_MM2 As Integer
        Private m_YY21 As Integer
        Private m_YY22 As Integer
        Private m_MM21 As Integer
        Private m_MM22 As Integer
        Private m_TRADE_TYPE As OrderTypesEnum
        Private m_PNC As Boolean
        Private m_ONLYHISTORICAL As Boolean
        Private m_TRADE_ID As Integer
        Private m_DESK_TRADER_ID As Integer

        <DataMember()> Public Property DESK_TRADER_ID As Integer
            Get
                Return m_DESK_TRADER_ID
            End Get
            Set(value As Integer)
                m_DESK_TRADER_ID = value
            End Set
        End Property
        <DataMember()> Public Property TRADE_ID As Integer
            Get
                Return m_TRADE_ID
            End Get
            Set(value As Integer)
                m_TRADE_ID = value
            End Set
        End Property
        <DataMember()> Public Property VolRecordType As VolRecordTypeEnum
            Get
                Return m_VolRecordType
            End Get
            Set(value As VolRecordTypeEnum)
                m_VolRecordType = value
            End Set
        End Property
        <DataMember()> Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        <DataMember()> Public Property ROUTE_ID2 As Integer
            Get
                Return m_ROUTE_ID2
            End Get
            Set(value As Integer)
                m_ROUTE_ID2 = value
            End Set
        End Property
        <DataMember()> Public Property FIXING_DATE As Date
            Get
                Return m_FIXING_DATE
            End Get
            Set(value As Date)
                m_FIXING_DATE = value
            End Set
        End Property
        <DataMember()> Public Property PERIOD As String
            Get
                Return m_PERIOD
            End Get
            Set(value As String)
                m_PERIOD = value
            End Set
        End Property
        <DataMember()> Public Property FFA_PRICE As Double
            Get
                Return m_FFA_PRICE
            End Get
            Set(value As Double)
                m_FFA_PRICE = value
            End Set
        End Property
        <DataMember()> Public Property SPOT_PRICE As Double
            Get
                Return m_SPOT_PRICE
            End Get
            Set(value As Double)
                m_SPOT_PRICE = value
            End Set
        End Property
        <DataMember()> Public Property IVOL As Double
            Get
                Return m_IVOL
            End Get
            Set(value As Double)
                m_IVOL = value
            End Set
        End Property
        <DataMember()> Public Property HVOL As Double
            Get
                Return m_HVOL
            End Get
            Set(value As Double)
                m_HVOL = value
            End Set
        End Property
        <DataMember()> Public Property INTEREST_RATE As Double
            Get
                Return m_INTEREST_RATE
            End Get
            Set(value As Double)
                m_INTEREST_RATE = value
            End Set
        End Property
        <DataMember()> Public Property YY1 As Integer
            Get
                Return m_YY1
            End Get
            Set(value As Integer)
                m_YY1 = value
            End Set
        End Property
        <DataMember()> Public Property YY2 As Integer
            Get
                Return m_YY2
            End Get
            Set(value As Integer)
                m_YY2 = value
            End Set
        End Property
        <DataMember()> Public Property MM1 As Integer
            Get
                Return m_MM1
            End Get
            Set(value As Integer)
                m_MM1 = value
            End Set
        End Property
        <DataMember()> Public Property MM2 As Integer
            Get
                Return m_MM2
            End Get
            Set(value As Integer)
                m_MM2 = value
            End Set
        End Property
        <DataMember()> Public Property YY21 As Integer
            Get
                Return m_YY21
            End Get
            Set(value As Integer)
                m_YY21 = value
            End Set
        End Property
        <DataMember()> Public Property YY22 As Integer
            Get
                Return m_YY22
            End Get
            Set(value As Integer)
                m_YY22 = value
            End Set
        End Property
        <DataMember()> Public Property MM21 As Integer
            Get
                Return m_MM21
            End Get
            Set(value As Integer)
                m_MM21 = value
            End Set
        End Property
        <DataMember()> Public Property MM22 As Integer
            Get
                Return m_MM22
            End Get
            Set(value As Integer)
                m_MM22 = value
            End Set
        End Property
        <DataMember()> Public Property ONLYHISTORICAL As Boolean
            Get
                Return m_ONLYHISTORICAL
            End Get
            Set(value As Boolean)
                m_ONLYHISTORICAL = value
            End Set
        End Property
        <DataMember()> Public Property PNC As Boolean
            Get
                Return m_PNC
            End Get
            Set(value As Boolean)
                m_PNC = value
            End Set
        End Property
        <DataMember()> Public Property TRADE_TYPE As OrderTypesEnum
            Get
                Return m_TRADE_TYPE
            End Get
            Set(value As OrderTypesEnum)
                m_TRADE_TYPE = value
            End Set
        End Property
        Public Function clone() As VolDataClass
            Dim ne As New VolDataClass
            ne.TRADE_TYPE = m_TRADE_TYPE
            ne.DESK_TRADER_ID = DESK_TRADER_ID
            ne.TRADE_ID = m_TRADE_ID
            ne.FFA_PRICE = m_FFA_PRICE
            ne.FIXING_DATE = m_FIXING_DATE
            ne.HVOL = m_HVOL
            ne.INTEREST_RATE = m_INTEREST_RATE
            ne.IVOL = m_IVOL
            ne.MM1 = m_MM1
            ne.MM2 = m_MM2
            ne.MM21 = m_MM21
            ne.MM22 = m_MM22
            ne.ONLYHISTORICAL = m_ONLYHISTORICAL
            ne.PERIOD = m_PERIOD
            ne.ROUTE_ID = m_ROUTE_ID
            ne.ROUTE_ID2 = m_ROUTE_ID2
            ne.SPOT_PRICE = m_SPOT_PRICE
            ne.YY1 = m_YY1
            ne.YY2 = m_YY2
            ne.YY21 = m_YY21
            ne.YY22 = m_YY22
            ne.PNC = m_PNC
            
            Return ne
        End Function
    End Class

    <DataContract()> Public Class SwapPeriodClass
        Private m_YY1 As Integer
        Private m_YY2 As Integer
        Private m_MM1 As Integer
        Private m_MM2 As Integer
        Private m_PERIOD As String

        Public Sub New()

        End Sub

        <DataMember()> Public Property YY1 As Integer
            Get
                Return m_YY1
            End Get
            Set(value As Integer)
                m_YY1 = value
            End Set
        End Property
        <DataMember()> Public Property YY2 As Integer
            Get
                Return m_YY2
            End Get
            Set(value As Integer)
                m_YY2 = value
            End Set
        End Property
        <DataMember()> Public Property MM1 As Integer
            Get
                Return m_MM1
            End Get
            Set(value As Integer)
                m_MM1 = value
            End Set
        End Property
        <DataMember()> Public Property MM2 As Integer
            Get
                Return m_MM2
            End Get
            Set(value As Integer)
                m_MM2 = value
            End Set
        End Property
        <DataMember()> Public Property PERIOD As String
            Get
                Return m_PERIOD
            End Get
            Set(value As String)
                m_PERIOD = value
            End Set
        End Property
    End Class

    <DataContract()> Public Class SwapDataClass
        Private m_ROUTE_ID As Integer
        Private m_VESSEL_CLASS_ID As Integer
        Private m_TRADE_CLASS_SHORT As Char
        Private m_CCY_ID As Integer
        Private m_SPOT_AVG As Double
        Private m_SPOT_PRICE As Double
        Private m_AVERAGE_INCLUDES_TODAY As Boolean
        Private m_PRICING_TICK As Double
        Private m_FORMAT_STRING As String
        Private m_DECIMAL_PLACES As Integer
        Private m_ROUTE_SHORT As String
        Private m_SPOT_FIXING_DATE As Date

        <DataMember()> Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        <DataMember()> Public Property VESSEL_CLASS_ID As Integer
            Get
                Return m_VESSEL_CLASS_ID
            End Get
            Set(value As Integer)
                m_VESSEL_CLASS_ID = value
            End Set
        End Property
        <DataMember()> Public Property TRADE_CLASS_SHORT As Char
            Get
                Return m_TRADE_CLASS_SHORT
            End Get
            Set(value As Char)
                m_TRADE_CLASS_SHORT = value
            End Set
        End Property
        <DataMember()> Public Property DECIMAL_PLACES As Integer
            Get
                Return m_DECIMAL_PLACES
            End Get
            Set(value As Integer)
                m_DECIMAL_PLACES = value
            End Set
        End Property
        <DataMember()> Public Property FORMAT_STRING As String
            Get
                Return m_FORMAT_STRING
            End Get
            Set(value As String)
                m_FORMAT_STRING = value
            End Set
        End Property
        <DataMember()> Public Property CCY_ID As Integer
            Get
                Return m_CCY_ID
            End Get
            Set(value As Integer)
                m_CCY_ID = value
            End Set
        End Property
        <DataMember()> Public Property FIXING_AVG As Double
            Get
                Return m_SPOT_AVG
            End Get
            Set(value As Double)
                m_SPOT_AVG = value
            End Set
        End Property
        <DataMember()> Public Property SPOT_PRICE As Double
            Get
                Return m_SPOT_PRICE
            End Get
            Set(value As Double)
                m_SPOT_PRICE = value
            End Set
        End Property
        <DataMember()> Public Property SPOT_FIXING_DATE As Date
            Get
                Return m_SPOT_FIXING_DATE
            End Get
            Set(value As Date)
                m_SPOT_FIXING_DATE = value
            End Set
        End Property
        <DataMember()> Public Property AVERAGE_INCLUDES_TODAY As Boolean
            Get
                Return m_AVERAGE_INCLUDES_TODAY
            End Get
            Set(value As Boolean)
                m_AVERAGE_INCLUDES_TODAY = value
            End Set
        End Property
        <DataMember()> Public Property PRICING_TICK As Double
            Get
                Return m_PRICING_TICK
            End Get
            Set(value As Double)
                m_PRICING_TICK = value
            End Set
        End Property
        <DataMember()> Public Property ROUTE_SHORT As String
            Get
                Return m_ROUTE_SHORT
            End Get
            Set(value As String)
                m_ROUTE_SHORT = value
            End Set
        End Property
    End Class

    <DataContract()> Public Class InterestRatesClass
        Private m_CCY_ID As Integer
        Private m_FIXING_DATE As Date
        Private m_PERIOD As Integer
        Private m_RATE As Double

        <DataMember()> Public Property CCY_ID As Integer
            Get
                Return m_CCY_ID
            End Get
            Set(value As Integer)
                m_CCY_ID = value
            End Set
        End Property
        <DataMember()> Public Property FIXING_DATE As Date
            Get
                Return m_FIXING_DATE
            End Get
            Set(value As Date)
                m_FIXING_DATE = value
            End Set
        End Property
        <DataMember()> Public Property PERIOD As Integer
            Get
                Return m_PERIOD
            End Get
            Set(value As Integer)
                m_PERIOD = value
            End Set
        End Property
        <DataMember()> Public Property RATE As Double
            Get
                Return m_RATE
            End Get
            Set(value As Double)
                m_RATE = value
            End Set
        End Property
    End Class

    <DataContract()> Public Class CCYInterestRatesClass
        Private m_Count As Integer
        Private m_CCY_INTEREST_RATES As New List(Of InterestRatesClass)

        <DataMember()> Public Property Count As Integer
            Get
                m_Count = m_CCY_INTEREST_RATES.Count
                Return m_Count
            End Get
            Set(value As Integer)
                m_Count = value
            End Set
        End Property

        <DataMember()> Public Property CCY_INTEREST_RATES As List(Of InterestRatesClass)
            Get
                Return m_CCY_INTEREST_RATES
            End Get
            Set(value As List(Of InterestRatesClass))
                m_CCY_INTEREST_RATES = value
            End Set
        End Property
    End Class

    <DataContract()> Public Class ArtBTimePeriod
        Private m_ROUTE_ID As Integer
        Private m_SERVER_DATE As Date
        Private m_StartMonth As Integer = -1
        Private m_EndMonth As Integer = -1
        Private m_Descr As String = ""
        Private m_Selected As Boolean = False
        Private m_StartDate As Date
        Private m_EndDate As Date
        Private m_MM1 As Integer
        Private m_YY1 As Integer
        Private m_MM2 As Integer
        Private m_YY2 As Integer
        Private m_DPM As Double
        Private m_TotDays As Integer

        <DataMember()> Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        <DataMember()> Public Property SERVER_DATE As Date
            Get
                Return m_SERVER_DATE
            End Get
            Set(value As Date)
                m_SERVER_DATE = value
            End Set
        End Property
        <DataMember()> Public Property StartMonth As Integer
            Get
                Return m_StartMonth
            End Get
            Set(value As Integer)
                m_StartMonth = value
            End Set
        End Property
        <DataMember()> Public Property EndMonth As Integer
            Get
                Return m_EndMonth
            End Get
            Set(value As Integer)
                m_EndMonth = value
            End Set
        End Property
        <DataMember()> Public Property Descr As String
            Get
                Return m_Descr
            End Get
            Set(value As String)
                m_Descr = value
            End Set
        End Property
        <DataMember()> Public Property Selected As Boolean
            Get
                Return m_Selected
            End Get
            Set(value As Boolean)
                m_Selected = value
            End Set
        End Property
        <DataMember()> Public Property StartDate As Date
            Get
                Return m_StartDate
            End Get
            Set(value As Date)
                m_StartDate = value
            End Set
        End Property
        <DataMember()> Public Property EndDate As Date
            Get
                Return m_EndDate
            End Get
            Set(value As Date)
                m_EndDate = value
            End Set
        End Property
        <DataMember()> Public Property MM1 As Integer
            Get
                Return m_MM1
            End Get
            Set(value As Integer)
                m_MM1 = value
            End Set
        End Property
        <DataMember()> Public Property YY1 As Integer
            Get
                Return m_YY1
            End Get
            Set(value As Integer)
                m_YY1 = value
            End Set
        End Property
        <DataMember()> Public Property MM2 As Integer
            Get
                Return m_MM2
            End Get
            Set(value As Integer)
                m_MM2 = value
            End Set
        End Property
        <DataMember()> Public Property YY2 As Integer
            Get
                Return m_YY2
            End Get
            Set(value As Integer)
                m_YY2 = value
            End Set
        End Property
        <DataMember()> Public Property DPM As Double
            Get
                Return m_DPM
            End Get
            Set(value As Double)
                m_DPM = value
            End Set
        End Property
        <DataMember()> Public Property TotDays As Integer
            Get
                Return m_TotDays
            End Get
            Set(value As Integer)
                m_TotDays = value
            End Set
        End Property


        Public Sub FillDPM()
            Dim i As Integer, m As Integer, y As Integer
            TotDays = 0
            For i = StartMonth To EndMonth
                y = CInt(Int(i / 12))
                m = (i - y * 12) + 1
                y = y + 2000
                TotDays = TotDays + DateTime.DaysInMonth(y, m)
            Next
            DPM = TotDays / (EndMonth - StartMonth + 1)
        End Sub

        Public Sub FillMY(ByVal a_mm1 As Integer, ByVal a_yy1 As Integer, _
                       ByVal a_mm2 As Integer, ByVal a_yy2 As Integer)
            MM1 = a_mm1
            YY1 = a_yy1 Mod 2000
            MM2 = a_mm2
            YY2 = a_yy2 Mod 2000
            StartMonth = YY1 * 12 + MM1 - 1
            EndMonth = YY2 * 12 + MM2 - 1
        End Sub

        Public Sub FillDates()
            Dim s As String
            If (StartMonth = -1) Then
                YY1 = 0
                MM1 = 1
            Else
                YY1 = CInt(Int(StartMonth / 12))
                MM1 = (StartMonth - YY1 * 12) + 1
            End If
            Try
                s = "20" & Format(YY1, "00") & "-" & Format(MM1, "00") & "-01"
                StartDate = Date.Parse(s)
            Catch e As Exception
            End Try
            If EndMonth = -1 Then
                YY2 = 0
                MM2 = 1
            Else
                YY2 = CInt(Int(EndMonth / 12))
                MM2 = (EndMonth - YY2 * 12) + 1
            End If
            Try
                s = "20" & Format(YY2, "00") & "-" & Format(MM2, "00") & "-" & Format(DateTime.DaysInMonth(YY2, MM2), "00")
                EndDate = Date.Parse(s)
            Catch e As Exception
            End Try
        End Sub

        Public Function AddOverlapping(ByRef t As ArtBTimePeriod) As Boolean
            AddOverlapping = False
            If t Is Nothing Then Exit Function
            If t.EndMonth < StartMonth - 1 Then Exit Function
            If EndMonth < StartMonth - 1 Then Exit Function
            If t.StartMonth < StartMonth Then StartMonth = t.StartMonth
            If t.EndMonth > EndMonth Then EndMonth = t.EndMonth
            AddOverlapping = True
        End Function

        Public Sub CreateDescr()
            FillDates()
            Descr = ""
            Dim i As Integer, y As Integer
            If StartMonth = EndMonth Then
                i = (StartMonth Mod 12) + 1
                Descr = MonthShort(MM1) & "-" & Format(YY1, "00")
                Exit Sub
            End If
            If EndMonth Mod 3 <> 2 Then
                If EndMonth - StartMonth >= 4 Then
                    Descr = MonthShort(MM1) & "-" & Format(YY1, "00")
                    i = EndMonth - StartMonth
                    Descr = Descr & "+" & Format(i, "0") & " Months"
                    Exit Sub
                Else
                    For i = StartMonth To EndMonth
                        If Len(Descr) > 0 Then Descr = Descr & "+"
                        Descr = Descr & MonthShort(i Mod 12 + 1) & "-" & Format(Int(i / 12), "00")
                    Next i
                    Exit Sub
                End If
            End If
            Dim YEM As Integer = EndMonth
            Dim SY As Integer = YY1 + 1
            Dim EY As Integer = YY2
            If MM2 = 12 Then
                If MM1 = 1 Then
                    YEM = -1
                    SY = YY1
                Else
                    YEM = SY * 12 - 1
                End If
                For i = SY To EY
                    If Len(Descr) > 0 Then Descr = Descr & "+"
                    Descr = Descr & "Cal-" & Format(i, "00")
                Next
            End If
            If StartMonth > YEM Then Exit Sub
            Dim mDescr As String = ""
            Dim qDescr As String = ""
            Dim minQ As Integer = CInt(Int(StartMonth / 3))
            If minQ * 3 < StartMonth Then
                minQ = minQ + 1
                For i = StartMonth To minQ * 3 - 1
                    If Len(mDescr) > 0 Then mDescr = mDescr & "+"
                    mDescr = mDescr & MonthShort(i Mod 12 + 1) & "-" & Format(Int(i / 12), "00")
                Next
            End If

            Dim maxQ As Integer = CInt(Int(YEM / 3))
            For i = minQ To maxQ
                If Len(qDescr) > 0 Then qDescr = qDescr & "+"

                y = CInt(Int(i / 4))
                If i Mod 4 = 0 And i + 3 <= maxQ Then
                    qDescr = qDescr & "Cal-" & Format(y, "00")
                    i += 3
                Else
                    qDescr = qDescr & "Q" & Format((i Mod 4) + 1, "0") & "-" & Format(y, "00")
                End If

            Next
            If Len(qDescr) > 0 Then
                If Len(Descr) > 0 Then Descr = "+" & Descr
                Descr = qDescr + Descr
            End If
            If Len(mDescr) > 0 Then
                If Len(Descr) > 0 Then Descr = "+" & Descr
                Descr = mDescr + Descr
            End If
        End Sub

        Public Sub ParseDescr()
            Dim i As Integer
            Dim qr As Integer
            Dim y As Integer
            If Len(Descr) >= 3 Then
                For i = 1 To 12
                    If Left(Descr, 3) = MonthShort(i) Then
                        y = CInt(Right(Descr, 2))
                        StartMonth = y * 12 + i - 1
                        EndMonth = y * 12 + i - 1
                        Exit Sub
                    End If
                Next
                If Left(Descr, 3) = "Cal" Then
                    y = CInt(Right(Descr, 2))
                    StartMonth = y * 12
                    EndMonth = y * 12 + 11
                End If
            End If
            Dim s As String = Descr
            Dim q As String
            StartMonth = 1000
            EndMonth = -1000
            While Len(s) > 1
                Dim j As Integer = s.IndexOf("+")
                If j > 1 Then
                    q = s.Substring(0, j)
                    s = s.Substring(j)
                Else
                    q = s
                    s = ""
                End If
                If Left(q, 1) <> "Q" Then
                    StartMonth = -1
                    EndMonth = -1
                    Exit Sub
                End If
                qr = CInt(Mid(q, 3, 1))
                If qr < 1 Or qr > 4 Then
                    StartMonth = -1
                    EndMonth = -1
                    Exit Sub
                End If
                y = CInt(Right(q, 2))
                Dim sm As Integer = y * 12 + (qr - 1) * 3
                Dim em As Integer = y * 12 + (qr - 1) * 3 + 2
                If StartMonth > sm Then StartMonth = sm
                If EndMonth < em Then EndMonth = em
            End While
        End Sub

        Public Function MonthShort(ByVal i As Integer) As String
            MonthShort = ""
            Select Case i
                Case 1
                    MonthShort = "Jan"
                Case 2
                    MonthShort = "Feb"
                Case 3
                    MonthShort = "Mar"
                Case 4
                    MonthShort = "Apr"
                Case 5
                    MonthShort = "May"
                Case 6
                    MonthShort = "Jun"
                Case 7
                    MonthShort = "Jul"
                Case 8
                    MonthShort = "Aug"
                Case 9
                    MonthShort = "Sep"
                Case 10
                    MonthShort = "Oct"
                Case 11
                    MonthShort = "Nov"
                Case 12
                    MonthShort = "Dec"
            End Select
        End Function

        Public Sub Add(ByVal s As String)
            Dim tp As New ArtBTimePeriod
            tp.Descr = s
            tp.ParseDescr()
            If EndMonth = -1 Then
                EndMonth = tp.EndMonth
                StartMonth = tp.StartMonth
                Exit Sub
            End If
            If StartMonth > tp.StartMonth Then StartMonth = tp.StartMonth
            If EndMonth > tp.EndMonth Then EndMonth = tp.EndMonth
        End Sub

        Public Function AddConsecutive(ByRef tp As ArtBTimePeriod) As Boolean
            AddConsecutive = True
            If EndMonth = -1 Then
                EndMonth = tp.EndMonth
                StartMonth = tp.StartMonth
                FillDates()
                Exit Function
            End If

            StartMonth = Math.Min(StartMonth, tp.StartMonth)
            EndMonth = Math.Max(EndMonth, tp.EndMonth)
            FillDates()
            Exit Function

            If StartMonth = tp.EndMonth + 1 Then
                StartMonth = tp.StartMonth
                Exit Function
            End If
            If EndMonth = tp.StartMonth - 1 Then
                EndMonth = tp.EndMonth
                Exit Function
            End If
            AddConsecutive = False
        End Function

        Public Function TotalDays() As Integer
            FillDates()
            TotalDays = CInt(1 + DateDiff(DateInterval.Day, StartDate, EndDate))
            TotDays = TotalDays
        End Function

        Public Function TotalMonths() As Integer
            FillDates()
            TotalMonths = CInt(1 + DateDiff(DateInterval.Month, StartDate, EndDate))
        End Function

        Public Function MinDaysInMonth() As Integer
            Dim i As Integer, y As Integer, m As Integer, d As Integer
            If StartMonth = -1 Or EndMonth = -1 Then
                MinDaysInMonth = 0
                Exit Function
            End If
            MinDaysInMonth = 31
            For i = StartMonth To EndMonth
                y = CInt(Int(i / 12))
                m = (i - y * 12) + 1
                d = Date.DaysInMonth(y, m)
                If d < MinDaysInMonth Then MinDaysInMonth = d
            Next
        End Function

        Public Function GetID() As Integer
            GetID = 0
            GetID = (YY1 Mod 2000) * 1000000
            GetID = GetID + MM1 * 10000
            GetID = GetID + (YY2 Mod 2000) * 100
            GetID = GetID + MM2
        End Function

        Public Function SortRanking() As Double
            SortRanking = (YY2 Mod 2000) * 1000000
            SortRanking = SortRanking + MM2 * 10000
            SortRanking = SortRanking - (YY1 Mod 2000) * 100
            SortRanking = SortRanking - MM1
        End Function

        Public Sub FillFromID(ByVal ID As Integer)
            YY1 = CInt(Int(ID / 1000000))
            MM1 = CInt(Int((ID Mod 1000000) / 10000))
            YY2 = CInt(Int((ID Mod 10000) / 100))
            MM2 = Int(ID Mod 100)
            StartMonth = YY1 * 12 + MM1 - 1
            EndMonth = YY2 * 12 + MM2 - 1
        End Sub

        Public Function AddToList(ByVal PeriodList As List(Of ArtBTimePeriod)) As Boolean
            AddToList = False
            For Each p As ArtBTimePeriod In PeriodList
                If p.StartMonth = StartMonth And p.EndMonth = EndMonth Then Exit Function
            Next
            PeriodList.Add(Me)
            AddToList = True
        End Function

        Public Sub GetFromRPStr(ByVal RPStr As String)
            Dim ID As Integer
            ID = CInt(RPStr.Substring(5, 8))
            FillFromID(ID)
        End Sub

        Public Function Contains(ByRef tp As ArtBTimePeriod) As Boolean
            If tp.StartMonth >= StartMonth And tp.EndMonth <= EndMonth Then Return True
            Return False
        End Function

        Public Function OverlapMonths(ByRef tp As ArtBTimePeriod) As Integer
            Dim MaxSM As Integer = Math.Max(tp.StartMonth, StartMonth)
            Dim MinEM As Integer = Math.Min(tp.EndMonth, EndMonth)
            If MinEM < MaxSM Then Return 0
            Return MinEM - StartMonth + 1
        End Function
    End Class

    <DataContract> Public Class MVClass
        Private m_SERVER_DATE As Date = Today
        Private m_MarketViewPeriods As New List(Of ArtBTimePeriod)

        <DataMember()> Public Property SERVER_DATE As Date
            Get
                Return m_SERVER_DATE
            End Get
            Set(value As Date)
                m_SERVER_DATE = value
            End Set
        End Property
        <DataMember()> Public Property MarketViewPeriods As List(Of ArtBTimePeriod)
            Get
                Return m_MarketViewPeriods
            End Get
            Set(value As List(Of ArtBTimePeriod))
                m_MarketViewPeriods = value
            End Set
        End Property
    End Class

    <DataContract()> Public Class ForwardsClass
        Private m_ROUTE_ID As Integer
        Private m_FIXING_DATE As Date
        Private m_FIXING As Double
        Private m_NORMFIX As Double
        Private m_YY1 As Integer
        Private m_YY2 As Integer
        Private m_MM1 As Integer
        Private m_MM2 As Integer
        Private m_KEY As String
        Private m_PERIOD As Integer
        Private m_REPORTDESC As String
        Private m_CMSROUTE_ID As String

        <DataMember()> Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        <DataMember()> Public Property FIXING_DATE As Date
            Get
                Return m_FIXING_DATE
            End Get
            Set(value As Date)
                m_FIXING_DATE = value
            End Set
        End Property
        <DataMember()> Public Property FIXING As Double
            Get
                Return m_FIXING
            End Get
            Set(value As Double)
                m_FIXING = value
            End Set
        End Property
        <DataMember()> Public Property NORMFIX As Double
            Get
                Return m_NORMFIX
            End Get
            Set(value As Double)
                m_NORMFIX = value
            End Set
        End Property
        <DataMember()> Public Property YY1 As Integer
            Get
                Return m_YY1
            End Get
            Set(value As Integer)
                m_YY1 = value
            End Set
        End Property
        <DataMember()> Public Property YY2 As Integer
            Get
                Return m_YY2
            End Get
            Set(value As Integer)
                m_YY2 = value
            End Set
        End Property
        <DataMember()> Public Property MM1 As Integer
            Get
                Return m_MM1
            End Get
            Set(value As Integer)
                m_MM1 = value
            End Set
        End Property
        <DataMember()> Public Property MM2 As Integer
            Get
                Return m_MM2
            End Get
            Set(value As Integer)
                m_MM2 = value
            End Set
        End Property
        <DataMember()> Public Property KEY As String
            Get
                Return m_KEY
            End Get
            Set(value As String)
                m_KEY = value
            End Set
        End Property
        <DataMember()> Public Property PERIOD As Integer
            Get
                Return m_PERIOD
            End Get
            Set(value As Integer)
                m_PERIOD = value
            End Set
        End Property
        <DataMember()> Public Property REPORTDESC As String
            Get
                Return m_REPORTDESC
            End Get
            Set(value As String)
                m_REPORTDESC = value
            End Set
        End Property
        <DataMember()> Public Property CMSROUTE_ID As String
            Get
                Return m_CMSROUTE_ID
            End Get
            Set(value As String)
                m_CMSROUTE_ID = value
            End Set
        End Property
    End Class

    <DataContract()> Public Class SwapFixingsClass
        Private m_ROUTE_ID As Integer
        Private m_FIXING_DATE As Date
        Private m_FIXING As Double
        Private m_YY1 As Integer
        Private m_MM1 As Integer
        Private m_YY2 As Integer
        Private m_MM2 As Integer

        Public Sub New()

        End Sub
        Public Sub New(ByVal f_ROUTE_ID As Integer, ByVal f_FIXING_DATE As Date, ByVal f_FIXING As Double, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer)
            m_ROUTE_ID = f_ROUTE_ID
            m_FIXING_DATE = f_FIXING_DATE
            m_FIXING = f_FIXING
            m_YY1 = f_YY1
            m_MM1 = f_MM1
            m_YY2 = f_YY2
            m_MM2 = f_MM2
        End Sub
        <DataMember()> Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        <DataMember()> Public Property FIXING_DATE As Date
            Get
                Return m_FIXING_DATE
            End Get
            Set(value As Date)
                m_FIXING_DATE = value
            End Set
        End Property
        <DataMember()> Public Property FIXING As Double
            Get
                Return m_FIXING
            End Get
            Set(value As Double)
                m_FIXING = value
            End Set
        End Property
        <DataMember()> Public Property YY1 As Integer
            Get
                Return m_YY1
            End Get
            Set(value As Integer)
                m_YY1 = value
            End Set
        End Property
        <DataMember()> Public Property MM1 As Integer
            Get
                Return m_MM1
            End Get
            Set(value As Integer)
                m_MM1 = value
            End Set
        End Property
        <DataMember()> Public Property YY2 As Integer
            Get
                Return m_YY2
            End Get
            Set(value As Integer)
                m_YY2 = value
            End Set
        End Property
        <DataMember()> Public Property MM2 As Integer
            Get
                Return m_MM2
            End Get
            Set(value As Integer)
                m_MM2 = value
            End Set
        End Property


    End Class

    <DataContract()> Public Class SpotFixingsClass
        Private m_ROUTE_ID As Integer
        Private m_FIXING_DATE As Date
        Private m_FIXING As Double
        Private m_AVG_FIXING As Double

        Public Sub New()

        End Sub

        Public Sub New(ByVal f_ROUTE_ID As Integer, ByVal f_FIXING_DATE As Date, ByVal f_FIXING As Double, Optional ByVal f_AVG_FIXING As Double = 0.0#)
            m_ROUTE_ID = f_ROUTE_ID
            m_FIXING_DATE = f_FIXING_DATE
            m_FIXING = f_FIXING
            m_AVG_FIXING = f_AVG_FIXING
        End Sub
        <DataMember()> Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        <DataMember()> Public Property FIXING_DATE As Date
            Get
                Return m_FIXING_DATE
            End Get
            Set(value As Date)
                m_FIXING_DATE = value
            End Set
        End Property
        <DataMember()> Public Property FIXING As Double
            Get
                Return m_FIXING
            End Get
            Set(value As Double)
                m_FIXING = value
            End Set
        End Property
        <DataMember()> Public Property AVG_FIXING As Double
            Get
                Return m_AVG_FIXING
            End Get
            Set(value As Double)
                m_AVG_FIXING = value
            End Set
        End Property
    End Class

    Public Class LiveRecorsdClass
        Private m_TRADE_ID As Integer
        Private m_ORDER_DATETIME As Date
        Private m_PRICE As Double
        Private m_PNC As Boolean

        Public Sub New()
        End Sub
        Public Sub New(ByVal _TRADE_ID As Integer, ByVal _ORDER_DATETIME As Date, ByVal _PRICE As Double, ByVal _PNC As Boolean)
            m_TRADE_ID = _TRADE_ID
            m_ORDER_DATETIME = _ORDER_DATETIME
            m_PRICE = _PRICE
            m_PNC = _PNC
        End Sub
        Public Property PNC As Boolean
            Get
                Return m_PNC
            End Get
            Set(value As Boolean)
                m_PNC = value
            End Set
        End Property
        Public Property TRADE_ID As Integer
            Get
                Return m_TRADE_ID
            End Get
            Set(value As Integer)
                m_TRADE_ID = value
            End Set
        End Property
        Public Property ORDER_DATETIME As Date
            Get
                Return m_ORDER_DATETIME
            End Get
            Set(value As Date)
                m_ORDER_DATETIME = value
            End Set
        End Property
        Public Property PRICE As Double
            Get
                Return m_PRICE
            End Get
            Set(value As Double)
                m_PRICE = value
            End Set
        End Property
    End Class

#Region "WP8"
    <DataContract> _
    Public Class WP8FFAData
        Private m_ROUTE_ID As Integer
        <DataMember> _
        Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        Private m_PERIOD As String = String.Empty
        <DataMember> _
        Public Property PERIOD As String
            Get
                Return m_PERIOD
            End Get
            Set(value As String)
                m_PERIOD = value
            End Set
        End Property
        Private m_YY1 As Integer
        <DataMember> _
        Public Property YY1 As Integer
            Get
                Return m_YY1
            End Get
            Set(value As Integer)
                m_YY1 = value
            End Set
        End Property
        Private m_YY2 As Integer
        <DataMember> _
        Public Property YY2 As Integer
            Get
                Return m_YY2
            End Get
            Set(value As Integer)
                m_YY2 = value
            End Set
        End Property
        Private m_MM1 As Integer
        <DataMember> _
        Public Property MM1 As Integer
            Get
                Return m_MM1
            End Get
            Set(value As Integer)
                m_MM1 = value
            End Set
        End Property
        Private m_MM2 As Integer
        <DataMember> _
        Public Property MM2 As Integer
            Get
                Return m_MM2
            End Get
            Set(value As Integer)
                m_MM2 = value
            End Set
        End Property
        Private m_FIXING As Decimal
        <DataMember> _
        Public Property FIXING As Decimal
            Get
                Return m_FIXING
            End Get
            Set(value As Decimal)
                m_FIXING = value
            End Set
        End Property
        Private m_FIXING_DATE As DateTime
        <DataMember> _
        Public Property FIXING_DATE As DateTime
            Get
                Return m_FIXING_DATE
            End Get
            Set(value As DateTime)
                m_FIXING_DATE = value
            End Set
        End Property
        Private m_LAST_TRADED As Decimal
        <DataMember> _
        Public Property LAST_TRADED As Decimal
            Get
                Return m_LAST_TRADED
            End Get
            Set(value As Decimal)
                m_LAST_TRADED = value
            End Set
        End Property
        Private m_TRADE_ID As Integer
        <DataMember> _
        Public Property TRADE_ID As Integer
            Get
                Return m_TRADE_ID
            End Get
            Set(value As Integer)
                m_TRADE_ID = value
            End Set
        End Property
        Private m_ORDER_DATETIME As DateTime
        <DataMember> _
        Public Property ORDER_DATETIME As DateTime
            Get
                Return m_ORDER_DATETIME
            End Get
            Set(value As DateTime)
                m_ORDER_DATETIME = value
            End Set
        End Property
        Private m_HVOL As Decimal
        <DataMember> _
        Public Property HVOL As Decimal
            Get
                Return m_HVOL
            End Get
            Set(value As Decimal)
                m_HVOL = value
            End Set
        End Property
        Private m_SpotAvg As Decimal
        <DataMember> _
        Public Property SpotAvg As Decimal
            Get
                Return m_SpotAvg
            End Get
            Set(value As Decimal)
                m_SpotAvg = value
            End Set
        End Property
        Private m_PRICE_STATUS As PriceStatusEnum
        <DataMember> _
        Public Property PRICE_STATUS As PriceStatusEnum
            Get
                Return m_PRICE_STATUS
            End Get
            Set(value As PriceStatusEnum)
                m_PRICE_STATUS = value
            End Set
        End Property
        Private m_RECORD_TYPE As RecordTypeEnum
        <DataMember> _
        Public Property RECORD_TYPE As RecordTypeEnum
            Get
                Return m_RECORD_TYPE
            End Get
            Set(value As RecordTypeEnum)
                m_RECORD_TYPE = value
            End Set
        End Property
        Private m_BID As Decimal
        <DataMember> _
        Public Property BID As Decimal
            Get
                Return m_BID
            End Get
            Set(value As Decimal)
                m_BID = value
            End Set
        End Property
        Private m_ASK As Decimal
        <DataMember> _
        Public Property ASK As Decimal
            Get
                Return m_ASK
            End Get
            Set(value As Decimal)
                m_ASK = value
            End Set
        End Property
        Private m_BidSize As Integer
        <DataMember> _
        Public Property BidSize As Integer
            Get
                Return m_BidSize
            End Get
            Set(value As Integer)
                m_BidSize = value
            End Set
        End Property
        Private m_AskSize As Integer
        <DataMember> _
        Public Property AskSize As Integer
            Get
                Return m_AskSize
            End Get
            Set(value As Integer)
                m_AskSize = value
            End Set
        End Property
        Private m_LastSize As Integer
        <DataMember> _
        Public Property LastSize As Integer
            Get
                Return m_LastSize
            End Get
            Set(value As Integer)
                m_LastSize = value
            End Set
        End Property

        Public ReadOnly Property NoMonths As Integer
            Get
                Return DateDiff(DateInterval.Month, DateSerial(m_YY1, m_MM1, 1), DateSerial(m_YY2, m_MM2, 1)) + 1
            End Get
        End Property

        Public ReadOnly Property PctDiff As Double
            Get
                Return (m_LAST_TRADED - m_FIXING) / m_FIXING
            End Get
        End Property
    End Class
    <DataContract> _
    Public Class GeneralFFDataClass
        Private m_ROUTES As New List(Of ROUTES)
        Private m_VESSEL_CLASSES As New List(Of VESSEL_CLASS)
        Private m_TRADE_CLASS As New TRADE_CLASSES

        <DataMember> _
        Public Property TRADE_CLASS As TRADE_CLASSES
            Get
                Return m_TRADE_CLASS
            End Get
            Set(value As TRADE_CLASSES)
                m_TRADE_CLASS = value
            End Set
        End Property

        <DataMember> _
        Public Property VESSEL_CLASSES As List(Of VESSEL_CLASS)
            Get
                Return m_VESSEL_CLASSES
            End Get
            Set(value As List(Of VESSEL_CLASS))
                m_VESSEL_CLASSES = value
            End Set
        End Property

        <DataMember> _
        Public Property ROUTES As List(Of ROUTES)
            Get
                Return m_ROUTES
            End Get
            Set(value As List(Of ROUTES))
                m_ROUTES = value
            End Set
        End Property
    End Class
    <DataContract> _
    Public Class SwapCurveClass
        Private m_ROUTE_ID As Integer
        Private m_PERIOD As String
        Private m_YY1 As Integer
        Private m_YY2 As Integer
        Private m_MM1 As Integer
        Private m_MM2 As Integer
        Private m_FIXING_DATE As Date
        Private m_FIXING As Double
        Private m_ORDER_DATETIME As Date
        Private m_PRICE_TRADED As Double
        Private m_PRICE_STATUS As PriceStatusEnum
        Private m_NoMonths As Integer
        Private m_HVOL As Double
        Private m_SpotAvg As Double
        Private m_RECORD_TYPE As RecordTypeEnum
        Private m_BID As Double
        Private m_ASK As Double

        Public Sub New()
        End Sub
        Public Sub New(ByVal _RECORD_TYPE As RecordTypeEnum, ByVal _ROUTE_ID As Integer, ByVal _YY1 As Integer, ByVal _MM1 As Integer, ByVal _YY2 As Integer, ByVal _MM2 As Integer, ByVal _FIXING_DATE As Date, ByVal _FIXING As Double, Optional ByVal _ORDER_DATETIME As Date = #1/1/1900#, Optional ByVal _PRICE_TRADED As Double = 0, Optional ByVal _PRICE_STATUS As PriceStatusEnum = PriceStatusEnum.Historic)
            m_RECORD_TYPE = _RECORD_TYPE
            m_ROUTE_ID = _ROUTE_ID
            m_YY1 = _YY1
            m_YY2 = _YY2
            m_MM1 = _MM1
            m_MM2 = _MM2
            m_FIXING_DATE = _FIXING_DATE
            m_FIXING = _FIXING
            m_ORDER_DATETIME = _ORDER_DATETIME
            m_PRICE_TRADED = _PRICE_TRADED
            m_PRICE_STATUS = _PRICE_STATUS
        End Sub

        Public ReadOnly Property NoMonths As Integer
            Get
                Return DateDiff(DateInterval.Month, DateSerial(m_YY1, m_MM1, 1), DateSerial(m_YY2, m_MM2, 1)) + 1
            End Get
        End Property
        Public ReadOnly Property PctDiff As Double
            Get
                Return (m_PRICE_TRADED - m_FIXING) / m_FIXING
            End Get
        End Property

        <DataMember> _
        Public Property PERIOD As String
            Get
                Return m_PERIOD
            End Get
            Set(value As String)
                m_PERIOD = value
            End Set
        End Property
        <DataMember> _
        Public Property RECORD_TYPE As RecordTypeEnum
            Get
                Return m_RECORD_TYPE
            End Get
            Set(value As RecordTypeEnum)
                m_RECORD_TYPE = value
            End Set
        End Property
        <DataMember> _
        Public Property PRICE_STATUS As PriceStatusEnum
            Get
                Return m_PRICE_STATUS
            End Get
            Set(value As PriceStatusEnum)
                m_PRICE_STATUS = value
            End Set
        End Property
        <DataMember> _
        Public Property ORDER_DATETIME As Date
            Get
                Return m_ORDER_DATETIME
            End Get
            Set(value As Date)
                m_ORDER_DATETIME = value
            End Set
        End Property
        <DataMember> _
        Public Property FIXING_DATE As Date
            Get
                Return m_FIXING_DATE
            End Get
            Set(value As Date)
                m_FIXING_DATE = value
            End Set
        End Property
        <DataMember> _
        Public Property ROUTE_ID As Integer
            Get
                Return m_ROUTE_ID
            End Get
            Set(value As Integer)
                m_ROUTE_ID = value
            End Set
        End Property
        <DataMember> _
        Public Property HVOL As Double
            Get
                Return m_HVOL
            End Get
            Set(value As Double)
                m_HVOL = value
            End Set
        End Property
        <DataMember> _
        Public Property SpotAvg As Double
            Get
                Return m_SpotAvg
            End Get
            Set(value As Double)
                m_SpotAvg = value
            End Set
        End Property
        <DataMember> _
        Public Property PRICE_TRADED As Double
            Get
                Return m_PRICE_TRADED
            End Get
            Set(value As Double)
                m_PRICE_TRADED = value
            End Set
        End Property
        <DataMember> _
        Public Property FIXING As Double
            Get
                Return m_FIXING
            End Get
            Set(value As Double)
                m_FIXING = value
            End Set
        End Property
        <DataMember> _
        Public Property YY1 As Integer
            Get
                Return m_YY1
            End Get
            Set(value As Integer)
                m_YY1 = value
            End Set
        End Property
        <DataMember> _
        Public Property YY2 As Integer
            Get
                Return m_YY2
            End Get
            Set(value As Integer)
                m_YY2 = value
            End Set
        End Property
        <DataMember> _
        Public Property MM1 As Integer
            Get
                Return m_MM1
            End Get
            Set(value As Integer)
                m_MM1 = value
            End Set
        End Property
        <DataMember> _
        Public Property MM2 As Integer
            Get
                Return m_MM2
            End Get
            Set(value As Integer)
                m_MM2 = value
            End Set
        End Property
        <DataMember> _
        Public Property BID As Double
            Get
                Return m_BID
            End Get
            Set(value As Double)
                m_BID = value
            End Set
        End Property
        <DataMember> _
        Public Property ASK As Double
            Get
                Return m_ASK
            End Get
            Set(value As Double)
                m_ASK = value
            End Set
        End Property
    End Class
    <DataContract> _
    Public Class NSwapPeriodsClass
        Private m_NSwapPeriod As NSwapPeriodsEnum
        Private m_FM As Double()
        Private m_M1 As Double
        Private m_M2 As Double
        Private m_M3 As Double
        Private m_M4 As Double
        Private m_M5 As Double
        Private m_M6 As Double
        Private m_M7 As Double
        Private m_M8 As Double
        Private m_M9 As Double
        Private m_M10 As Double
        Private m_M11 As Double
        Private m_M12 As Double

        Public Sub New()
        End Sub
        Public Sub New(ByVal _NSwapPeriod As NSwapPeriodsEnum)
            m_NSwapPeriod = _NSwapPeriod
            Select Case NSwapPeriod
                Case NSwapPeriodsEnum.Cal
                    ReDim m_FM(11)
                Case NSwapPeriodsEnum.Q234
                    ReDim m_FM(9)
                Case NSwapPeriodsEnum.Q34
                    ReDim m_FM(5)
                Case NSwapPeriodsEnum.Q4
                    ReDim m_FM(2)
            End Select
        End Sub

        <DataMember> _
        Public Property NSwapPeriod As NSwapPeriodsEnum
            Get
                Return m_NSwapPeriod
            End Get
            Set(value As NSwapPeriodsEnum)
                m_NSwapPeriod = value
            End Set
        End Property
        <DataMember> _
        Public Property FM As Double()
            Get
                Return m_FM
            End Get
            Set(value As Double())
                m_FM = value
            End Set
        End Property
        <DataMember> _
        Public Property M1 As Double
            Get
                Return m_M1
            End Get
            Set(value As Double)
                m_M1 = value
            End Set
        End Property
        <DataMember> _
        Public Property M2 As Double
            Get
                Return m_M2
            End Get
            Set(value As Double)
                m_M2 = value
            End Set
        End Property
        <DataMember> _
        Public Property M3 As Double
            Get
                Return m_M3
            End Get
            Set(value As Double)
                m_M3 = value
            End Set
        End Property
        <DataMember> _
        Public Property M4 As Double
            Get
                Return m_M4
            End Get
            Set(value As Double)
                m_M4 = value
            End Set
        End Property
        <DataMember> _
        Public Property M5 As Double
            Get
                Return m_M5
            End Get
            Set(value As Double)
                m_M5 = value
            End Set
        End Property
        <DataMember> _
        Public Property M6 As Double
            Get
                Return m_M6
            End Get
            Set(value As Double)
                m_M6 = value
            End Set
        End Property
        <DataMember> _
        Public Property M7 As Double
            Get
                Return m_M7
            End Get
            Set(value As Double)
                m_M7 = value
            End Set
        End Property
        <DataMember> _
        Public Property M8 As Double
            Get
                Return m_M8
            End Get
            Set(value As Double)
                m_M8 = value
            End Set
        End Property
        <DataMember> _
        Public Property M9 As Double
            Get
                Return m_M9
            End Get
            Set(value As Double)
                m_M9 = value
            End Set
        End Property
        <DataMember> _
        Public Property M10 As Double
            Get
                Return m_M10
            End Get
            Set(value As Double)
                m_M10 = value
            End Set
        End Property
        <DataMember> _
        Public Property M11 As Double
            Get
                Return m_M11
            End Get
            Set(value As Double)
                m_M11 = value
            End Set
        End Property
        <DataMember> _
        Public Property M12 As Double
            Get
                Return m_M12
            End Get
            Set(value As Double)
                m_M12 = value
            End Set
        End Property
    End Class
    <DataContract(Name:="PriceStatusEnum")> _
    Public Enum PriceStatusEnum
        <EnumMember> Trade = 0
        <EnumMember> Level = 1
        <EnumMember> Historic = 2
    End Enum
    <DataContract(Name:="RecordTypeEnum")> _
    Public Enum RecordTypeEnum
        <EnumMember> Swap = 0
        <EnumMember> RatioSpread = 1
        <EnumMember> CalendarSpread = 2
        <EnumMember> PriceSpread = 3
        <EnumMember> MarketSpread = 4
        <EnumMember> Spot = 99
        <EnumMember> Vol = 100
    End Enum
    <DataContract(Name:="NSwapPeriodsEnum")> _
    Public Enum NSwapPeriodsEnum
        <EnumMember> Cal = 1
        <EnumMember> Q234 = 4
        <EnumMember> Q34 = 7
        <EnumMember> Q4 = 10
    End Enum
#End Region
   
#Region "Enumerations"
    <DataContract(Name:="OptCalcServiceEnum")> _
    Public Enum OptCalcServiceEnum
        <EnumMember> SpotsUploaded
        <EnumMember> ForwardsUploaded
        <EnumMember> RefreshAll
        <EnumMember> Success
        <EnumMember> ServiceError
    End Enum

    <DataContract(Name:="VolRecordTypeEnum")> _
    Public Enum VolRecordTypeEnum
        <EnumMember> spot
        <EnumMember> nspot
        <EnumMember> swap
        <EnumMember> live
        <EnumMember> level
        <EnumMember> all
        <EnumMember> mvperiods
        <EnumMember> decimals
    End Enum

    <DataContract(Name:="FPStatusEnum")> _
    Public Enum FPStatusEnum
        <EnumMember> InvalidLicenseKey
        <EnumMember> ValidLicenseKey
        <EnumMember> ExistingClientAllOK
        <EnumMember> NewClient
        <EnumMember> NewClientFPTrialCreated
        <EnumMember> ExistingFPTrialLicenseExpired
        <EnumMember> ExistingFPLiveLicenseExpired
        <EnumMember> ExistingFPTrialRenewed
        <EnumMember> ExistingFPLiveRenewed
        <EnumMember> ExistingFPLiveMaxLicensesError
        <EnumMember> UserDataAmmendSuccess
        <EnumMember> UserDataAmmendError
        <EnumMember> LiveLicenseNewFingerPrintAdded
        <EnumMember> DemoLicenseNewFingerPrintAdded
        <EnumMember> LiveLicenseExistingFingerPrintAdded
        <EnumMember> DemoLicenseExistingFingerPrintAdded
        <EnumMember> LiveLicenseHasExpiredNoFingerPrintAdded
        <EnumMember> DemoLicenseHasExpiredNoFingerPrintAdded
        <EnumMember> DemoFingerPrintHasExpired
        <EnumMember> UserWantsToRegisterPC
        <EnumMember> DBError
        <EnumMember> OFError
        <EnumMember> NoInternetConnection
        <EnumMember> InternetConnectionOK
        <EnumMember> ServiceOK
        <EnumMember> ServiceError
    End Enum

    <DataContract(Name:="OrderTypes")> _
    Public Enum OrderTypesEnum
        <EnumMember> FFA = 0
        <EnumMember> RatioSpread
        <EnumMember> CalendarSpread
        <EnumMember> PriceSpread
        <EnumMember> MarketSpread
    End Enum

    <DataContract(Name:="RouteAvgTypeEnum")> _
    Public Enum RouteAvgTypeEnum
        <EnumMember> WholeMonth = 0
        <EnumMember> LastSevenDays = 7
        <EnumMember> LastTenDays = 10
    End Enum

    <DataContract()>
    Public Class CompositeType

        <DataMember()>
        Public Property BoolValue() As Boolean

        <DataMember()>
        Public Property StringValue() As String

    End Class

   
#End Region
End Namespace

