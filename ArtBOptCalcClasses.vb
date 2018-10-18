Public Class GRIDPeriodsClass
    Private m_TRADE_ID As Integer
    Private m_FIXING_DATE As Date
    Private m_PERIOD As String
    Private m_FFA_PRICE As Double
    Private m_IVOL As Double
    Private m_HVOL As Double
    Private m_INTEREST_RATE As Double
    Private m_YY1 As Integer
    Private m_YY2 As Integer
    Private m_MM1 As Integer
    Private m_MM2 As Integer
    Private m_ONLYHISTORICAL As Boolean
    Private m_LIVE_DATA As Boolean = False
    Private m_VolRecordType As FFAOptCalcService.VolRecordTypeEnum

    Public Property TRADE_ID As Integer
        Get
            Return m_TRADE_ID
        End Get
        Set(value As Integer)
            m_TRADE_ID = value
        End Set
    End Property
    Public Property FIXING_DATE As Date
        Get
            Return m_FIXING_DATE
        End Get
        Set(value As Date)
            m_FIXING_DATE = value
        End Set
    End Property
    Public Property PERIOD As String
        Get
            Return m_PERIOD
        End Get
        Set(value As String)
            m_PERIOD = value
        End Set
    End Property
    Public Property FFA_PRICE As Double
        Get
            Return m_FFA_PRICE
        End Get
        Set(value As Double)
            m_FFA_PRICE = value
        End Set
    End Property
    Public Property IVOL As Double
        Get
            Return m_IVOL
        End Get
        Set(value As Double)
            m_IVOL = value
        End Set
    End Property
    Public Property HVOL As Double
        Get
            Return m_HVOL
        End Get
        Set(value As Double)
            m_HVOL = value
        End Set
    End Property
    Public Property INTEREST_RATE As Double
        Get
            Return m_INTEREST_RATE
        End Get
        Set(value As Double)
            m_INTEREST_RATE = value
        End Set
    End Property
    Public Property YY1 As Integer
        Get
            Return m_YY1
        End Get
        Set(value As Integer)
            m_YY1 = value
        End Set
    End Property
    Public Property YY2 As Integer
        Get
            Return m_YY2
        End Get
        Set(value As Integer)
            m_YY2 = value
        End Set
    End Property
    Public Property MM1 As Integer
        Get
            Return m_MM1
        End Get
        Set(value As Integer)
            m_MM1 = value
        End Set
    End Property
    Public Property MM2 As Integer
        Get
            Return m_MM2
        End Get
        Set(value As Integer)
            m_MM2 = value
        End Set
    End Property
    Public Property ONLYHISTORICAL As Boolean
        Get
            Return m_ONLYHISTORICAL
        End Get
        Set(value As Boolean)
            m_ONLYHISTORICAL = value
        End Set
    End Property
    Public Property LIVE_DATA As Boolean
        Get
            Return m_LIVE_DATA
        End Get
        Set(value As Boolean)
            m_LIVE_DATA = value
        End Set
    End Property
    Public Property VolRecordType As FFAOptCalcService.VolRecordTypeEnum
        Get
            Return m_VolRecordType
        End Get
        Set(value As FFAOptCalcService.VolRecordTypeEnum)
            m_VolRecordType = value
        End Set
    End Property
End Class

Public Class ArtBOptCalcClasses
    Enum GraphPlotEnum
        Spot
        SpotAvg
        FFA
        SpotHVol
        FFAIVol
        FFAHVol
    End Enum
End Class

Public Class xmppMsg
    Private m_MsgType As xmppMsgTypeEnum
    Private m_YY1 As Integer
    Private m_YY2 As Integer
    Private m_MM1 As Integer
    Private m_MM2 As Integer
    Private m_TradePrice As Double

    Public Property MsgType As xmppMsgTypeEnum
        Get
            Return m_MsgType
        End Get
        Set(value As xmppMsgTypeEnum)
            m_MsgType = value
        End Set
    End Property
    Public Property YY1 As Integer
        Get
            Return m_YY1
        End Get
        Set(value As Integer)
            m_YY1 = value
        End Set
    End Property
    Public Property YY2 As Integer
        Get
            Return m_YY2
        End Get
        Set(value As Integer)
            m_YY2 = value
        End Set
    End Property
    Public Property MM1 As Integer
        Get
            Return m_MM1
        End Get
        Set(value As Integer)
            m_MM1 = value
        End Set
    End Property
    Public Property MM2 As Integer
        Get
            Return m_MM2
        End Get
        Set(value As Integer)
            m_MM2 = value
        End Set
    End Property
    Public Property TradePrice As Double
        Get
            Return m_TradePrice
        End Get
        Set(value As Double)
            m_TradePrice = value
        End Set
    End Property

End Class

Public Class ForwardsClass
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

    Public Property ROUTE_ID As Integer
        Get
            Return m_ROUTE_ID
        End Get
        Set(value As Integer)
            m_ROUTE_ID = value
        End Set
    End Property
    Public Property FIXING_DATE As Date
        Get
            Return m_FIXING_DATE
        End Get
        Set(value As Date)
            m_FIXING_DATE = value
        End Set
    End Property
    Public Property FIXING As Double
        Get
            Return m_FIXING
        End Get
        Set(value As Double)
            m_FIXING = value
        End Set
    End Property
    Public Property NORMFIX As Double
        Get
            Return m_NORMFIX
        End Get
        Set(value As Double)
            m_NORMFIX = value
        End Set
    End Property
    Public Property YY1 As Integer
        Get
            Return m_YY1
        End Get
        Set(value As Integer)
            m_YY1 = value
        End Set
    End Property
    Public Property YY2 As Integer
        Get
            Return m_YY2
        End Get
        Set(value As Integer)
            m_YY2 = value
        End Set
    End Property
    Public Property MM1 As Integer
        Get
            Return m_MM1
        End Get
        Set(value As Integer)
            m_MM1 = value
        End Set
    End Property
    Public Property MM2 As Integer
        Get
            Return m_MM2
        End Get
        Set(value As Integer)
            m_MM2 = value
        End Set
    End Property
    Public Property KEY As String
        Get
            Return m_KEY
        End Get
        Set(value As String)
            m_KEY = value
        End Set
    End Property
    Public Property PERIOD As Integer
        Get
            Return m_PERIOD
        End Get
        Set(value As Integer)
            m_PERIOD = value
        End Set
    End Property
    Public Property REPORTDESC As String
        Get
            Return m_REPORTDESC
        End Get
        Set(value As String)
            m_REPORTDESC = value
        End Set
    End Property
    Public Property CMSROUTE_ID As String
        Get
            Return m_CMSROUTE_ID
        End Get
        Set(value As String)
            m_CMSROUTE_ID = value
        End Set
    End Property
End Class

Public Class BWTypeClass
    Private m_Assignment As BWEnum
    Private m_Result As FFAOptCalcService.FPStatusEnum
    Public Property Assignment As BWEnum
        Get
            Return m_Assignment
        End Get
        Set(value As BWEnum)
            m_Assignment = value
        End Set
    End Property
    Public Property Result As FFAOptCalcService.FPStatusEnum
        Get
            Return m_Result
        End Get
        Set(value As FFAOptCalcService.FPStatusEnum)
            m_Result = value
        End Set
    End Property
End Class


Public Class InstalledComputersClass
    Private m_NO As Integer
    Private m_COMPUTER As String
    Private m_FINGERPRINT As String
    Private m_ACTIVE As Boolean
    Private m_MYPC As Boolean
    Private m_DELETE As Boolean = False

    Public Property NO As Integer
        Get
            Return m_NO
        End Get
        Set(value As Integer)
            m_NO = value
        End Set
    End Property

    Public Property COMPUTER As String
        Get
            Return m_COMPUTER
        End Get
        Set(value As String)
            m_COMPUTER = value
        End Set
    End Property
    Public Property FINGERPRINT As String
        Get
            Return m_FINGERPRINT
        End Get
        Set(value As String)
            m_FINGERPRINT = value
        End Set
    End Property
    Public Property ACTIVE As Boolean
        Get
            Return m_ACTIVE
        End Get
        Set(value As Boolean)
            m_ACTIVE = value
        End Set
    End Property
    Public Property MYPC As Boolean
        Get
            Return m_MYPC
        End Get
        Set(value As Boolean)
            m_MYPC = value
        End Set
    End Property
    Public Property DELETE As Boolean
        Get
            Return m_DELETE
        End Get
        Set(value As Boolean)
            m_DELETE = value
        End Set
    End Property
End Class

Public Class EshopListClass
    Private m_NOLICENSES As String
    Private m_PRICE As Single

    Public Property NOLICENSES As String
        Get
            Return m_NOLICENSES
        End Get
        Set(value As String)
            m_NOLICENSES = value
        End Set
    End Property
    Public Property PRICE As Single
        Get
            Return m_PRICE
        End Get
        Set(value As Single)
            m_PRICE = value
        End Set
    End Property
End Class

Public Enum xmppMsgTypeEnum
    SpotRatesUpdated
    ForwardRatesUpdated
    NewTradeRegistered
    xmppConnected
    xmppDisconnected
End Enum

Public Enum ConnStateEnum
    DirectConnectionEstablished
    DirectConnectionFailed
    ProbablyBehindProxy
    ProxyConnectionEstablised
    ProxyConnectionFailed
    CannotConnectAtAll
End Enum

Public Enum BWEnum
    CheckInternetConnection
    CheckServiceStatus
    CheckCredentials
End Enum