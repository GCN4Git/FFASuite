
Public NotInheritable Class GridToolTips
    Inherits StringEnumeration(Of GridToolTips)

    Public Shared ReadOnly FirmPrice As New GridToolTips("This is a firm price order you can immediately trade with." & vbNewLine & _
                                                         "Double click on the price, or right click mouse and select" & vbNewLine & _
                                                         "appropriate exchange to start the trading dialog.")
    Public Shared ReadOnly FirmPriceCanDoBetter As New GridToolTips("This a firm price order, flagged as can do better." & vbNewLine & _
                                                                    "You can either directly trade this price, or right" & vbNewLine & _
                                                                    "click mouse and select initiating a negotiation process.")
    Public Shared ReadOnly IndicativePrice As New GridToolTips("This an indicative order, you cannot directly trade on it." & vbNewLine & _
                                                               "Right click mouse and select Firm Up and Hit/Lift in order to alert your" & vbNewLine & _
                                                               "broker you are requesting a firm up of this price for immediate execution." & vbNewLine & _
                                                               "Your firm interest would be valid for three minutes")
    Public Shared ReadOnly FixedQuantity As New GridToolTips("The indicated volume quantity for this order is fixed." & vbNewLine & _
                                                             "You can only trade the indicated quantity")
    Public Shared ReadOnly BasketQuantity As New GridToolTips("This is a basket quantity order, you can trade as many lots" & vbNewLine & _
                                                              "as you like up to a maximum of the indicated quantity size.")
    Public Shared ReadOnly FixedQuantityMore As New GridToolTips("The indicated volume quantity for this order is flagged as can do more." & vbNewLine & _
                                                                 "Right click mouse and select negotiation to indicate you are interested" & vbNewLine & _
                                                                 "to transact in a larger size.")
    Public Shared ReadOnly FixedQuantityLess As New GridToolTips("The indicated volume quantity for this order is flagged as can do less." & vbNewLine & _
                                                                 "Right click mouse and select negotiation to indicate you are interested" & vbNewLine & _
                                                                 "to transact in a smaller size.")
    Public Shared ReadOnly FixedQuantityFlexible As New GridToolTips("The indicated volume quantity for this order is flagged as flexible," & vbNewLine & _
                                                                     "meaning more or less quantities than the indicated one can be negotiated." & vbNewLine & _
                                                                     "Right click mouse and select negotiation to indicate you are interested" & vbNewLine & _
                                                                     "to transact in a different size.")
    Public Shared ReadOnly FirmPriceTimer As New GridToolTips("This is a firm price order valid for the remaining" & vbNewLine & _
                                                              "time displayed in seconds.")
    Public Shared ReadOnly ExchangeIndicated As New GridToolTips("The staring letter of the exchange this order is placed in, e.g. L for LCH, is indicated here." & vbNewLine & _
                                                                 "Double clicking on it would immediately ask you to confirm to buy/sell the full indicated quantity " & vbNewLine & _
                                                                 "at this exchange, or right click on the indicated price field and select Direct Hit-Detailed for full order flexibility.")
    Public Shared ReadOnly ReferencePrice As New GridToolTips("This is a system calculated reference price." & vbNewLine & _
                                                              "You cannot directly trade on it.")
    Public Shared ReadOnly NegotiatedOrder As New GridToolTips("This order is currently being negotiated between two or more counterparties.")
    Public Shared ReadOnly DiscussOrder As New GridToolTips("This order is being privately negotiated between two counterparties.")
    Public Shared ReadOnly PriceField As New GridToolTips("Double click price to execute full quantity on your preffered exchange, " & vbNewLine & _
                                                          "or right click and select Direct Hit - Detailed for full order flexibility")

    Private Sub New(ByVal StringConstant As String)
        MyBase.New(StringConstant)
    End Sub
End Class

Public Enum Exch
    None
    OTC
    LCH
    SGX
    NOS
End Enum

Public Enum ShowName
    None
    Never
    Always
    Mutual
End Enum

Public Enum OrderGoodTill
    None
    Day
    Limit
    GTC
End Enum

Public Enum OrderFlexQuantinty
    None
    Fixed
    CanDoLess
    Bucket
    CanDoMore
    CanDoMoreOrLess
    StrictFull
End Enum

Public Enum OrderDayQualifier
    NotInDays
    Full
    Half
    DPM
    ContractDays
End Enum

Public Enum ArtBMessages
    Dummy
    TraderDisconnected
    TraderConnected
    SimpleMessage
    OrderFFANew
    RespondOrderFFANew
    RespondOrderFFANewFailed
    OrderFFAAmmend
    RespondOrderFFAAmmend
    ArtBChange
    OTC
    LCH
    SGX
    NOS
    ArtBServiceAlive
    RespondArtBServiceAlive
    ChangeCounterPartyLimits
    RespondChangeCounterPartyLimits
    ChangeTraderAuthorities
    RespondChangeTraderAuthorities
    OrderFFATrade
    ChangeFFATrade
    RespondChangeFFATrade
    OrderFFAFirmUp
    RespondOrderFFAFirmUp
    IM_RosterDelete
    IM_RosterAdd
    IM_RosterSuccess
    ServerShutDownWarning
    ServerShutDown
    RefreshAccountData
    OrderFFAInfo
    TradeFFAInfo
    OrderFFAChangeOwner
    FicticiousTrade
    RespondFicticiousTrade
    KillTradeFFAInfoDlg
    DBServerDown
    DBServerUp
    RefreshIMServer
    KillFirmUpDlg
    SpotRatesUpdated = 15000
    ForwardRatesUpdated = 15001
    DisconnectFFAOptionsUsers = 15002
    AmmendReportedTrade = 15003
    BidAskUpdate = 15004
 End Enum

Public Enum GlobalViewModes
    Admin
    Trader
    Broker
    AffiliatedBroker
End Enum

Public Enum GVCOpMode
    Service
    Client
End Enum

Public Enum ArtBErrors
    Success = 0
    RecordNotFound = 1
    EmptyObject = 2
    InsertFailed = 3
    UpdateFailed = 4
    DeleteFailed = 5
    RecordSeparatorNotFound = 6
    InvalidNumberOfFields = 7
    InvalidPrimaryKey = 8
    ConversionToStrError = 9
    SubmitChangesFailed = 10
    InvalidLoginName = 11
    InvalidLoginPassword = 12
    InsertInCollectionFailed = 13
    RemoveFromCollectionFailed = 14
    SendMessageFailed = 15
    FieldConversionFailed = 16
    InvalidNumberOfRecords = 17
    CounterOrderDoesNotExist = 18
    CounterOrderIsSleeping = 19
    CounterOrderIsNegotiated = 20
    CounterOrderIsExecuted = 21
    CounterOrderIsDead = 22
    ConnectionDead = 23
    OrdersNotMatch = 24
    NegotiationOnNonActiveOrder = 25
    NoTraderInfo = 26
    ConversionFromStrError = 27
    IndicativeOrderProhibited = 28
    TradeNotToBeReported = 29
    TryToAmmendAllreadyModifiedOrder = 30
    DescriptionError = 31
    UpdateOrderInDBCrash = 32
    IssueAdditionalSpreadTradesCrash = 33
End Enum

Public Enum OpMode
    NewOrder
    DirectHit
    Negotiate
    AmmendOrder
    FirmUp
    Sleep
    Delete
    ChangeOwner
    Activate
    NukeSleep
    NukeDelete
End Enum

Public Enum ArtBMsgBoxAnswer
    None
    OK
    Cancel
    Yes
    No
    TimeOut
End Enum

Public Enum ArtBMsgBoxStyle
    None
    Buy
    Sell
    Warning
    ErrorRed
    Info
    MarketDepthSuccess
    MarketDepthFail
    ProceedCancel
    TimeAndSales
    PrivateBid
    PrivateOffer
End Enum

Public Enum ArtBMsgBoxIcon
    None
    DiscussGreen
    DiscussRed
    MarketDepth
    Clock
End Enum

Public Enum OrderInfoTypes
    FirmUp
    ChangeOwner
    Negotiation
    DirectHitFailed
End Enum

Public Enum ArtBLostConnection
    Sleep = 1
    Delete = 2
    ForwardToBroker = 3
    DoNothing = 4
End Enum

Public Enum ArtBDefaultNuke
    Sleep = 1
    Delete = 2
End Enum

Public Enum OFUserStatus
    ServerError
    Connected
    OffLine
End Enum
Public Enum ArtBTradeClass
    Dry = 1
    Wet = 2
    Coal = 3
    Iron = 4
    Box = 5
End Enum

Public Enum TradeAuthorities
    ViewOnly = 0
    ViewAllChangeHis
    ViewAllChangeAll
End Enum

Public Enum NukeAction
    Sleep = 1
    Delete
End Enum

Public Enum OrderTypes
    FFA = 0
    RatioSpread
    CalendarSpread
    PriceSpread
    MarketSpread
End Enum

Public Enum Ticks
    Down_Equal = 0
    Down
    Up_Equal
    Up
    None
End Enum

Public Enum SpreadLegTypes
    None = 0
    Order1
    Order2
    CrossOrder1
    CrossOrder2
    ProjectedOrder1
    ProjectedOrder2
End Enum

Public Enum Sounds
    Busy_Signal
    Yahoo_buzz
    Ping
    Yahoo_message
    Yahoo_alert
    Yahoo_online
    Yahoo_offline
End Enum

Public Enum ServiceActions
    SimpleMessage
    OrderMessage
    OrdersExpired
    ClientLogout
    SleepAllOrders
    ReadAccount
    ReadDB
End Enum

Public MustInherit Class StringEnumeration(Of TStringEnumeration As StringEnumeration(Of TStringEnumeration))
    Implements IStringEnumeration

    Private myString As String
    Sub New(ByVal StringConstant As String)
        myString = StringConstant
    End Sub

#Region "Properties"
    Public Class [Enum]
        Public Shared Function GetValues() As String()
            Dim myValues As New List(Of String)
            For Each myFieldInfo As System.Reflection.FieldInfo In GetSharedFieldsInfo()
                Dim myValue As StringEnumeration(Of TStringEnumeration) = CType(myFieldInfo.GetValue(Nothing), StringEnumeration(Of TStringEnumeration))  'Shared Fields use a Null object
                myValues.Add(myValue)
            Next
            Return myValues.ToArray
        End Function

        Public Shared Function GetNames() As String()
            Dim myNames As New List(Of String)
            For Each myFieldInfo As System.Reflection.FieldInfo In GetSharedFieldsInfo()
                myNames.Add(myFieldInfo.Name)
            Next
            Return myNames.ToArray
        End Function

        Public Shared Function GetName(ByVal myName As StringEnumeration(Of TStringEnumeration)) As String
            Return myName
        End Function

        Public Shared Function isDefined(ByVal myName As String) As Boolean
            If GetName(myName) Is Nothing Then Return False
            Return True
        End Function

        Public Shared Function GetUnderlyingType() As Type
            Return GetType(String)
        End Function

        Friend Shared Function GetSharedFieldsInfo() As System.Reflection.FieldInfo()
            Return GetType(TStringEnumeration).GetFields
        End Function

        Friend Shared Function GetSharedFields() As StringEnumeration(Of TStringEnumeration)()
            Dim myFields As New List(Of StringEnumeration(Of TStringEnumeration))
            For Each myFieldInfo As System.Reflection.FieldInfo In GetSharedFieldsInfo()
                Dim myField As StringEnumeration(Of TStringEnumeration) = CType(myFieldInfo.GetValue(Nothing), StringEnumeration(Of TStringEnumeration))  'Shared Fields use a Null object
                myFields.Add(myField)
            Next
            Return myFields.ToArray
        End Function
    End Class
#End Region

#Region "Cast Operators"
    'Downcast to String
    Public Shared Widening Operator CType(ByVal myStringEnumeration As StringEnumeration(Of TStringEnumeration)) As String
        If myStringEnumeration Is Nothing Then Return Nothing
        Return myStringEnumeration.ToString
    End Operator

    'Upcast to StringEnumeration
    Public Shared Widening Operator CType(ByVal myString As String) As StringEnumeration(Of TStringEnumeration)
        For Each myElement As StringEnumeration(Of TStringEnumeration) In StringEnumeration(Of TStringEnumeration).Enum.GetSharedFields
            'Found a Matching StringEnumeration - Return it
            If myElement.ToString = myString Then Return myElement
        Next
        'Did not find a Match - return NOTHING
        Return Nothing
    End Operator

    Overrides Function ToString() As String Implements IStringEnumeration.ToString
        Return myString
    End Function
#End Region

#Region "Concatenation Operators"
    Public Shared Operator &(ByVal left As StringEnumeration(Of TStringEnumeration), ByVal right As StringEnumeration(Of TStringEnumeration)) As String
        If left Is Nothing And right Is Nothing Then Return Nothing
        If left Is Nothing Then Return right.ToString
        If right Is Nothing Then Return left.ToString
        Return left.ToString & right.ToString
    End Operator

    Public Shared Operator &(ByVal left As StringEnumeration(Of TStringEnumeration), ByVal right As IStringEnumeration) As String
        If left Is Nothing And right Is Nothing Then Return Nothing
        If left Is Nothing Then Return right.ToString
        If right Is Nothing Then Return left.ToString
        Return left.ToString & right.ToString
    End Operator
#End Region

#Region "Operator Equals"

    Public Shared Operator =(ByVal left As StringEnumeration(Of TStringEnumeration), ByVal right As StringEnumeration(Of TStringEnumeration)) As Boolean
        If left Is Nothing Or right Is Nothing Then Return False
        Return left.ToString.Equals(right.ToString)
    End Operator

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If TypeOf (obj) Is StringEnumeration(Of TStringEnumeration) Then
            Return CType(obj, StringEnumeration(Of TStringEnumeration)).ToString = myString
        ElseIf TypeOf (obj) Is String Then
            Return CType(obj, String) = myString
        End If
        Return False
    End Function
#End Region

#Region "Operator Not Equals"
    Public Shared Operator <>(ByVal left As StringEnumeration(Of TStringEnumeration), ByVal right As StringEnumeration(Of TStringEnumeration)) As Boolean
        Return Not left = right
    End Operator

#End Region

End Class

'Base Interface without any Generics for StringEnumerations
Public Interface IStringEnumeration
    Function ToString() As String
End Interface
