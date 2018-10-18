' NOTE: You can use the "Rename" command on the context menu to change the interface name "IService1" in both code and config file together.
<ServiceContract()>
Public Interface IFFAOptMain

    <OperationContract()>
    Function GetData(ByVal value As Integer) As String
    <OperationContract()>
    Function GetDataUsingDataContract(ByVal composite As DataContracts.CompositeType) As DataContracts.CompositeType

    ' TODO: Add your service operations here
    <OperationContract()> Function UpdateSpreadMargins(ByVal f_FIXING_DATE As Date, ByVal rse_PERIOD As Integer, ByVal rse_SDEV As Double) As Boolean
    <OperationContract()> Function AmmendReportedTrade(ByVal Trade As DataContracts.VolDataClass) As Boolean
    <OperationContract()> Function GetDailyTrades(ByVal TRADE_DATE As Date) As List(Of DataContracts.VolDataClass)
    <OperationContract()> Function RefreshIntradayData(ByVal f_ROUTES As List(Of Integer)) As List(Of DataContracts.VolDataClass)
    <OperationContract()> Function GetMarketView(ByVal f_ROUTES As List(Of Integer)) As List(Of DataContracts.VolDataClass)
    <OperationContract()> Function SwapFixings() As List(Of DataContracts.SwapFixingsClass)
    <OperationContract(IsOneWay:=True)> Sub BuildGDPostFixing(ByVal FINGERPRINT As String)
    <OperationContract(IsOneWay:=True)> Sub KeepAlive()
    <OperationContract()> Function Graph(ByVal ROUTE_ID As Integer, ByVal YY1 As Integer, ByVal MM1 As Integer, ByVal YY2 As Integer, ByVal MM2 As Integer) As List(Of DataContracts.VolDataClass)
    <OperationContract()> Function SpotFixings() As List(Of DataContracts.SpotFixingsClass)
    <OperationContract()> Function GetMVPeriods(Optional ByVal f_ROUTE_ID As Integer = 36, Optional ByVal ReturnAll As Boolean = False) As List(Of DataContracts.ArtBTimePeriod)
    <OperationContract()> Function CheckCredentials(ByVal UD As DataContracts.FingerPrintClass) As DataContracts.FingerPrintClass
    <OperationContract()> Function RegisterUser(ByVal UD As DataContracts.FingerPrintClass) As DataContracts.FingerPrintClass
    <OperationContract()> Function CheckLicense(ByVal UD As DataContracts.FingerPrintClass) As DataContracts.FingerPrintClass
    <OperationContract()> Function ServerDate() As Date
    <OperationContract()> Function ServerDateTime() As DateTime
    <OperationContract()> Function GetTradeClases() As List(Of TRADE_CLASSES)
    <OperationContract()> Function GetVesselClases() As List(Of VESSEL_CLASS)
    <OperationContract()> Function GetRoutes() As List(Of ROUTES)
    <OperationContract()> Function GetHolidays() As List(Of Date)
    <OperationContract()> Function LastVolFixDate(ByVal f_ROUTE_ID As Integer) As Date
    <OperationContract()> Function LastInterestFixDate(ByVal f_CCY_ID As Integer) As Date
    <OperationContract()> Function LastSwapFixDate(ByVal f_ROUTE_ID As Integer) As Date
    <OperationContract()> Function LastSpotFixDate(ByVal f_ROUTE_ID As Integer) As Date
    <OperationContract()> Function RouteSpotAverage(ByVal f_ROUTE_ID As Integer, ByVal f_YY As Integer, ByVal f_MM As Integer) As Double
    <OperationContract()> Function InterestRates() As List(Of DataContracts.InterestRatesClass)
    <OperationContract()> Function LastSwapFixing(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer) As Double
    <OperationContract()> Function SwapData() As List(Of DataContracts.SwapDataClass)
    <OperationContract()> Function ROUTE_DETAIL(ByVal f_ROUTE_ID As List(Of Integer)) As List(Of DataContracts.SwapDataClass)
    <OperationContract()> Function GetSwapFixingsHistorical(ByVal f_ROUTE_ID As Integer, ByVal f_YY1 As Integer, ByVal f_MM1 As Integer, ByVal f_YY2 As Integer, ByVal f_MM2 As Integer, ByVal f_StartDate As Date, ByVal f_EndDate As Date) As List(Of DataContracts.VolDataClass)
    <OperationContract()> Function SwapVolatility(ByVal f_ROUTE_ID As Integer, Optional ByVal f_TPeriod As Integer = 3, Optional ByVal f_VPeriod As Integer = 10) As List(Of DataContracts.VolDataClass)
    <OperationContract()> Function QueryRouteData(ByVal f_ROUTE_ID As Integer, Optional ByVal f_TPeriod As Integer = 3, Optional ByVal f_VPeriod As Integer = 10) As List(Of DataContracts.VolDataClass)
    <OperationContract()> Function ServiceCommands(ByVal f_Command As DataContracts.OptCalcServiceEnum) As DataContracts.OptCalcServiceEnum
    <OperationContract()> Function GetFFASuiteCredentials() As DataContracts.FFASuiteCredentials
    <OperationContract()> Function GetFFAMessageClass() As DataContracts.FFAMessage

    <OperationContract()> Function GetVolRecordTypeEnum() As DataContracts.VolRecordTypeEnum
    <OperationContract()> Function GetMessageEnum() As DataContracts.MessageEnum
    <OperationContract()> Function GetOrderTypes() As DataContracts.OrderTypesEnum
    <OperationContract()> Function GetRouteAvgTypeEnum() As DataContracts.RouteAvgTypeEnum

    <OperationContract()> Function GetContractFTP() As List(Of BALTIC_FTP)
    <OperationContract()> Function UpdateSpots(ByVal FIXING_DATE As Date, ByVal SPOT_FIXINGS As List(Of BALTIC_SPOT_RATES)) As Boolean
    <OperationContract()> Function UpdateForwards(ByVal FIXING_DATE As Date, ByVal FORWARD_FIXINGS As List(Of BALTIC_FORWARD_RATES)) As Boolean
    <OperationContract()> Function UpdateVolatilities(ByVal FIXING_DATE As Date, ByVal VOLATILITY_FIXINGS As List(Of BALTIC_OPTION_VOLATILITIES)) As Boolean
    <OperationContract()> Function UpdateWeeklyVolumes(ByVal FIXING_DATE As Date, ByVal VOLUMES_FIXINGS As List(Of BALTIC_DRY_VOLUMES)) As Boolean

#Region "WP8"
    <OperationContract()> Function GetPriceStatusEnum() As DataContracts.PriceStatusEnum
    <OperationContract()> Function GetRecordTypeEnum() As DataContracts.RecordTypeEnum
    <OperationContract()> Function GetNSwapPeriodsEnum() As DataContracts.NSwapPeriodsEnum
    <OperationContract()> Function GetWP8FFAData() As DataContracts.WP8FFAData
    <OperationContract()> Function GetGeneralFFDataClass() As DataContracts.GeneralFFDataClass
    <OperationContract()> Function GetSwapCurveClass() As DataContracts.SwapCurveClass
    <OperationContract()> Function GetNSwapPeriodsClass() As DataContracts.NSwapPeriodsClass
#End Region
End Interface

' Use a data contract as illustrated in the sample below to add composite types to service operations.
