Imports ffaOption

Public Class FFAOptionSolveClass

#Region "LocalVariables"
    Private m_LegNo As Integer
    Private m_OD As New FFAOptionClass
    Private FFA As New ffaOption.FfaOption
    Private m_SolveFor As OptionSolveForEnum
    Private m_Solver As OptionSolverEnum
    Private m_nSam As Integer = 100           'number of time steps
    Private m_pMax As Integer = 100000       'number of paths
    Private m_Seed As Integer = 3           'random number seed
    Private m_EpsilonPerc As Double = 0.001         'epsilon in % for greeks
    Private m_CUDAEnabled As Boolean = False
    Private rnd As New Random
    Private m_FromStrategy As StrategyForEnum

#End Region

    Public Sub New()

    End Sub

#Region "Properties"
    Property FromStrategy As StrategyForEnum
        Get
            Return m_FromStrategy
        End Get
        Set(value As StrategyForEnum)
            m_FromStrategy = value
        End Set
    End Property
    Property LegNo As Integer
        Get
            Return m_LegNo
        End Get
        Set(value As Integer)
            m_LegNo = value
        End Set
    End Property
    Property OD As FFAOptionClass
        Get
            Return m_OD
        End Get
        Set(value As FFAOptionClass)
            m_OD = value
        End Set
    End Property
    Property SolveFor As OptionSolveForEnum
        Get
            Return m_SolveFor
        End Get
        Set(ByVal value As OptionSolveForEnum)
            m_SolveFor = value
        End Set
    End Property
    Property EpsilonPerc As Double
        Get
            Return m_EpsilonPerc
        End Get
        Set(ByVal value As Double)
            m_EpsilonPerc = value
        End Set
    End Property
    Property Solver As OptionSolverEnum
        Get
            Return m_Solver
        End Get
        Set(value As OptionSolverEnum)
            m_Solver = value
        End Set
    End Property
    Public Property nSam As Integer
        Get
            Return m_nSam
        End Get
        Set(value As Integer)
            m_nSam = value
        End Set
    End Property
    Public Property pMax As Integer
        Get
            Return m_pMax
        End Get
        Set(value As Integer)
            m_pMax = value
        End Set
    End Property
    Public Property Seed As Integer
        Get
            Return m_Seed
        End Get
        Set(value As Integer)
            m_Seed = value
        End Set
    End Property
    Public ReadOnly Property CUDAEnabled As Boolean
        Get
            Dim aflag As Boolean = False        'average flag: with (=1) or without (=0) including trade date
            Dim acond As Integer = 0            'average condition, 0: month, 1: first seven days, 2: last seven days
            Dim F As Double = 13630.0           'forward ffa rate
            Dim K As Double = 13630.0           'strike price  
            Dim r As Double = 0.01              'discount rate
            Dim A As Double = 0.0               'average price so far
            Dim omega As Double = 1.0           'option type cap (=1.0) or floor (=-1.0)
            Dim sigma As Double = 0.709         'volatility parameter
            Dim epsilon As Double = 0.01        'for Greeks
            Dim optStaMM As Integer = 1         'option start month
            Dim optStaYY As Integer = 2012      'option start year
            Dim optEndMM As Integer = 1         'option end month
            Dim optEndYY As Integer = 2012      'option end year
            Dim nSam As Integer = 100           'number of time steps
            Dim pMax As Integer = 100000       'number of paths
            Dim Seed As Integer = 3             'random number seed
            Dim mcFlag As Integer = 2           'MC flag, 0: analytic, 1: serial Monte Carlo, 2: cuda Monte Carlo
            Dim Holidays As New List(Of DateTime) 'Holidays schedule
            Dim TradeDate As DateTime = New DateTime(2011, 9, 29) 'option trade date
            Dim optVal As Double = 0.0          'Option Value
            Dim blackImpVol As Double = 0.0#
            Dim ValGreeks As List(Of Double) = New List(Of Double) 'list of outputs

            Try
                blackImpVol = FFA.calOptValGreeks(F, K, r, A, aflag, omega, sigma, epsilon, optStaMM, optStaYY, _
                                      optEndMM, optEndYY, nSam, pMax, Seed, mcFlag, acond, TradeDate, Holidays, ValGreeks)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property
#End Region

    Public Function Solve(ByVal f_SolveFor As OptionSolveForEnum, Optional ByVal f_Solver As OptionSolverEnum = OptionSolverEnum.Analytic, Optional ByVal f_pMax As Integer = 100000) As Double
        m_Seed = rnd.Next(1, 100)
        m_pMax = f_pMax
        m_Solver = f_Solver
        m_SolveFor = f_SolveFor

        Dim ValGreeks As List(Of Double) = New List(Of Double) 'list of outputs
        Select Case f_SolveFor
            Case OptionSolveForEnum.Price
                Try
                    OD.BSVolatility = FFA.calOptValGreeks(OD.FFAPrice, OD.StrikePrice, OD.RiskFreeRate, OD.AvgPrice, OD.AvgIncludesToday, _
                                                         OD.OptionType, OD.Volatility, Me.EpsilonPerc, OD.MM1, OD.YY1, _
                                                         OD.MM2, OD.YY2, Me.nSam, Me.pMax, Me.Seed, Me.Solver, OD.AverageType, OD.TradeDate, OD.Holidays, ValGreeks)
                    OD.OptionValue = FormatPricingTick(ValGreeks(0), OD.PricingTick)
                    If OD.Quantity < 0 Then
                        OD.PayReceive = -OD.OptionValue * OD.NoDays * Math.Abs(OD.Quantity) * OD.OptionBS
                    Else
                        OD.PayReceive = -OD.OptionValue * OD.Quantity * OD.OptionBS
                    End If
                    OD.Delta = ValGreeks(1)
                    OD.Gamma = ValGreeks(2)
                    OD.Rho = ValGreeks(3)
                    OD.Theta = ValGreeks(4)
                    OD.Vega = ValGreeks(5)
                Catch ex As Exception
                    OD.BSVolatility = 0.0#
                    OD.OptionValue = -1.0#
                    OD.PayReceive = 0.0#
                    OD.Delta = 0.0#
                    OD.Gamma = 0.0#
                    OD.Rho = 0.0#
                    OD.Theta = 0.0#
                    OD.Vega = 0.0#
                End Try
                Return OD.OptionValue
            Case OptionSolveForEnum.Vol
                Try
                    OD.Volatility = FFA.calFfaVol(OD.OptionValue, OD.FFAPrice, OD.StrikePrice, OD.RiskFreeRate, OD.AvgPrice, OD.AvgIncludesToday, _
                                                 OD.OptionType, OD.MM1, OD.YY1, OD.MM2, OD.YY2, OD.AverageType, OD.TradeDate, OD.Holidays)
                    OD.BSVolatility = FFA.calOptValGreeks(OD.FFAPrice, OD.StrikePrice, OD.RiskFreeRate, OD.AvgPrice, OD.AvgIncludesToday, _
                                                             OD.OptionType, OD.Volatility, Me.EpsilonPerc, OD.MM1, OD.YY1, _
                                                             OD.MM2, OD.YY2, Me.nSam, Me.pMax, Me.Seed, Me.Solver, OD.AverageType, OD.TradeDate, OD.Holidays, ValGreeks)
                    OD.OptionValue = FormatPricingTick(OD.OptionValue, OD.PricingTick)
                    If OD.Quantity < 0 Then
                        OD.PayReceive = -OD.OptionValue * OD.NoDays * Math.Abs(OD.Quantity) * OD.OptionBS
                    Else
                        OD.PayReceive = -OD.OptionValue * OD.Quantity * OD.OptionBS
                    End If
                    OD.Delta = ValGreeks(1)
                    OD.Gamma = ValGreeks(2)
                    OD.Rho = ValGreeks(3)
                    OD.Theta = ValGreeks(4)
                    OD.Vega = ValGreeks(5)
                Catch ex As Exception
                    OD.PayReceive = 0.0#
                    OD.BSVolatility = 0.0#
                    OD.Volatility = -1.0#
                    OD.Delta = 0.0#
                    OD.Gamma = 0.0#
                    OD.Rho = 0.0#
                    OD.Theta = 0.0#
                    OD.Vega = 0.0#
                End Try
                Return OD.Volatility
            Case OptionSolveForEnum.Strike
                OD.StrikePrice = SolveForStrikePrice(OD)
                OD.StrikePrice = FormatPricingTick(OD.StrikePrice, OD.PricingTick)
                If OD.StrikePrice <> -1.0# Then
                    Try
                        OD.BSVolatility = FFA.calOptValGreeks(OD.FFAPrice, OD.StrikePrice, OD.RiskFreeRate, OD.AvgPrice, OD.AvgIncludesToday, _
                                                                 OD.OptionType, OD.Volatility, Me.EpsilonPerc, OD.MM1, OD.YY1, _
                                                                 OD.MM2, OD.YY2, Me.nSam, Me.pMax, Me.Seed, Me.Solver, OD.AverageType, OD.TradeDate, OD.Holidays, ValGreeks)
                        OD.OptionValue = FormatPricingTick(OD.OptionValue, OD.PricingTick)
                        If OD.Quantity < 0 Then
                            OD.PayReceive = -OD.OptionValue * OD.NoDays * Math.Abs(OD.Quantity) * OD.OptionBS
                        Else
                            OD.PayReceive = -OD.OptionValue * OD.Quantity * OD.OptionBS
                        End If
                        OD.Delta = ValGreeks(1)
                        OD.Gamma = ValGreeks(2)
                        OD.Rho = ValGreeks(3)
                        OD.Theta = ValGreeks(4)
                        OD.Vega = ValGreeks(5)
                    Catch ex As Exception
                        OD.PayReceive = 0.0#
                        OD.StrikePrice = -1.0#
                        OD.BSVolatility = 0.0#
                        OD.Delta = 0.0#
                        OD.Gamma = 0.0#
                        OD.Rho = 0.0#
                        OD.Theta = 0.0#
                        OD.Vega = 0.0#
                    End Try
                Else
                    OD.PayReceive = 0.0#
                    OD.StrikePrice = -1.0#
                    OD.BSVolatility = 0.0#
                    OD.Delta = 0.0#
                    OD.Gamma = 0.0#
                    OD.Rho = 0.0#
                    OD.Theta = 0.0#
                    OD.Vega = 0.0#
                End If
                Return OD.StrikePrice
        End Select
        Return -9000000000.0
    End Function
    Private Function SolveForStrikePrice(ByVal opt As FFAOptionClass) As Double
        Dim ai As Double = 0.0001
        Dim bi As Double = opt.FFAPrice * 5
        Dim err As Double = opt.OptionValue * 0.025 / 100
        Dim Pi As Double = 0.0#
        Dim fPi As Double = opt.OptionValue * 1 / 100
        Dim fai As Double = 0.0#
        Dim I As Integer = 1
        Dim tPrice As Double = 0.0#
        Dim cntr As Integer = 0

        opt.StrikePrice = -1.0#

        Try
            While Math.Abs(fPi) > err
                cntr += 1
                If cntr > 50 Then
                    Return -1.0#
                End If

                Pi = (ai + bi) / 2
                tPrice = FFA.calOptVal(opt.FFAPrice, Pi, opt.RiskFreeRate, opt.AvgPrice, opt.AvgIncludesToday, _
                                       opt.OptionType, opt.Volatility, opt.MM1, opt.YY1, opt.MM2, opt.YY2, opt.AverageType, opt.TradeDate, opt.Holidays)
                fPi = opt.OptionValue - tPrice

                If Math.Abs(fPi) <= err Then
                    Exit While
                End If

                tPrice = FFA.calOptVal(opt.FFAPrice, ai, opt.RiskFreeRate, opt.AvgPrice, opt.AvgIncludesToday, _
                                       opt.OptionType, opt.Volatility, opt.MM1, opt.YY1, opt.MM2, opt.YY2, opt.AverageType, opt.TradeDate, opt.Holidays)
                fai = opt.OptionValue - tPrice
                If fPi * fai > 0 Then
                    ai = Pi
                    bi = bi
                Else
                    ai = ai
                    bi = Pi
                End If
                I = I + 1
                If I > 25 Then
                    Pi = -1.0#
                    opt.StrikePrice = Pi
                End If
            End While

            opt.StrikePrice = Pi
        Catch ex As Exception
            opt.StrikePrice = -1.0#
        End Try
        Return opt.StrikePrice
    End Function
    Public Function FastSolvePrice() As Double
        Try
            OD.OptionValue = FFA.calOptVal(OD.FFAPrice, OD.StrikePrice, OD.RiskFreeRate, OD.AvgPrice, OD.AvgIncludesToday, _
                                   OD.OptionType, OD.Volatility, OD.MM1, OD.YY1, OD.MM2, OD.YY2, OD.AverageType, OD.TradeDate, OD.Holidays)
            If OD.Quantity < 0 Then
                OD.PayReceive = -OD.OptionValue * OD.NoDays * Math.Abs(OD.Quantity) * OD.OptionBS
            Else
                OD.PayReceive = -OD.OptionValue * OD.Quantity * OD.OptionBS
            End If
        Catch ex As Exception
            OD.OptionValue = -1.0#
            OD.PayReceive = 0.0#
        End Try
        Return OD.OptionValue
    End Function
    Public Function FastSolveVol(ByVal _OptionValue As Double) As Double
        Try
            OD.Volatility = FFA.calFfaVol(_OptionValue, OD.FFAPrice, OD.StrikePrice, OD.RiskFreeRate, OD.AvgPrice, OD.AvgIncludesToday, _
                                          OD.OptionType, OD.MM1, OD.YY1, OD.MM2, OD.YY2, OD.AverageType, OD.TradeDate, OD.Holidays)
            If OD.Quantity < 0 Then
                OD.PayReceive = -OD.OptionValue * OD.NoDays * Math.Abs(OD.Quantity) * OD.OptionBS
            Else
                OD.PayReceive = -OD.OptionValue * OD.Quantity * OD.OptionBS
            End If
        Catch ex As Exception
            OD.Volatility = -1.0#
            OD.PayReceive = 0.0#
        End Try
        Return OD.Volatility
    End Function
    Private Function LastTradingDay(ByVal f_YY As Integer, ByVal f_MM As Integer, ByVal f_Holidays As List(Of DateTime)) As Date
        Dim monthend As Date = DateSerial(f_YY, f_MM, 1)
        monthend = DateAdd(DateInterval.Month, 1, monthend)
        monthend = DateAdd(DateInterval.Day, -1, monthend)

        Dim tdate As Date
        Dim LastDay As Integer = monthend.Day
        For I = LastDay To 1 Step -1
            tdate = DateSerial(f_YY, f_MM, I)
            If tdate.DayOfWeek = DayOfWeek.Saturday Or tdate.DayOfWeek = DayOfWeek.Sunday Or f_Holidays.Contains(tdate) Then
                Continue For
            End If
            Return tdate
        Next
        Return tdate
    End Function
    Private Function FormatPricingTick(ByVal l_Result As Double, ByVal l_PRICE_TICK As Double) As Double
        Dim fs As String = ""
        If Math.Log10(l_PRICE_TICK) >= 0 Then
            l_Result = Math.Round(l_Result, 0)
        ElseIf Math.Log10(l_PRICE_TICK) = -1 Then
            l_Result = Math.Round(l_Result, 1)
        ElseIf Math.Log10(l_PRICE_TICK) = -2 Then
            l_Result = Math.Round(l_Result, 2)
        ElseIf Math.Log10(l_PRICE_TICK) = -3 Then
            l_Result = Math.Round(l_Result, 3)
        ElseIf Math.Log10(l_PRICE_TICK) > -2 And Math.Log10(l_PRICE_TICK) < -1 Then
            l_Result = Math.Round(l_Result, 2)
        ElseIf Math.Log10(l_PRICE_TICK) > -1 And Math.Log10(l_PRICE_TICK) < 0 Then
            l_Result = Math.Round(l_Result, 2)
        End If
        Return l_Result
    End Function
End Class

Public Enum StrategyForEnum
    BuyOptParticipationMain
    BuyOptParticipationOpt
    SellCapParticipationMain
    SellCapParticipationOpt
    FFAvsTCRateDiff
    SellOptionPeriodForTC
    SellIndexLinkedCap
    BuyIndexLinkedFloor
    SellPutFromTCNotFullyProtected
End Enum
Public Class FFAOptionClass
#Region "LocalVariables"
    Private m_OptionRefID As Integer
    Private m_OptionType As OptionTypeEnum
    Private m_OptionBS As OptionBSEnum
    Private m_RouteID As Integer
    Private m_PricingTick As Double
    Private m_TradeDate As DateTime
    Private m_FFAPrice As Double
    Private m_StrikePrice As Double
    Private m_OptionValue As Double
    Private m_Quantity As Integer
    Private m_PayReceive As Double
    Private m_AvgPrice As Double
    Private m_AverageType As OptionAverageTypeEnum
    Private m_AvgIncludesToday As Boolean
    Private m_YY1 As Integer
    Private m_MM1 As Integer
    Private m_YY2 As Integer
    Private m_MM2 As Integer
    Private m_NoDays As Integer
    Private m_Volatility As Double
    Private m_Skew As Double
    Private m_RiskFreeRate As Double
    Private m_Holidays As New List(Of DateTime)

    'Greeks
    Private m_Delta As Double = 0.01#
    Private m_Gamma As Double
    Private m_Rho As Double
    Private m_Theta As Double
    Private m_Vega As Double
    Private m_BSVolatility As Double
#End Region

#Region "Properties"
    ReadOnly Property NoDays As Integer
        Get
            Dim SDate As Date = DateSerial(m_YY1, m_MM1, 1)
            Dim LDate As Date = DateSerial(m_YY2, m_MM2, 28)
            LDate = DateSerial(LDate.Year, LDate.Month, Date.DaysInMonth(LDate.Year, LDate.Month))
            Return DateDiff(DateInterval.Day, SDate, LDate) + 1
        End Get
    End Property
    Property Quantity As Integer
        Get
            Return m_Quantity
        End Get
        Set(value As Integer)
            m_Quantity = value
        End Set
    End Property
    Property OptionRefID As Integer
        Get
            Return m_OptionRefID
        End Get
        Set(ByVal value As Integer)
            m_OptionRefID = value
        End Set
    End Property
    Property TradeDate As DateTime
        Get
            Return m_TradeDate
        End Get
        Set(ByVal value As DateTime)
            m_TradeDate = value
        End Set
    End Property
    Property OptionType As OptionTypeEnum
        Get
            Return m_OptionType
        End Get
        Set(ByVal value As OptionTypeEnum)
            m_OptionType = value
        End Set
    End Property
    Property OptionBS As OptionBSEnum
        Get
            Return m_OptionBS
        End Get
        Set(value As OptionBSEnum)
            m_OptionBS = value
        End Set
    End Property
    Property FFAPrice As Double
        Get
            Return m_FFAPrice
        End Get
        Set(ByVal value As Double)
            m_FFAPrice = value
        End Set
    End Property
    Property StrikePrice As Double
        Get
            Return m_StrikePrice
        End Get
        Set(ByVal value As Double)
            m_StrikePrice = value
        End Set
    End Property
    Property PricingTick As Double
        Get
            Return m_PricingTick
        End Get
        Set(value As Double)
            m_PricingTick = value
        End Set
    End Property
    Property OptionValue As Double
        Get
            Return m_OptionValue
        End Get
        Set(ByVal value As Double)
            m_OptionValue = value
        End Set
    End Property
    Property PayReceive As Double
        Get
            Return m_PayReceive
        End Get
        Set(value As Double)
            m_PayReceive = value
        End Set
    End Property
    Property Delta As Double
        Get
            Return m_Delta
        End Get
        Set(ByVal value As Double)
            m_Delta = value
        End Set
    End Property
    Property Gamma As Double
        Get
            Return m_Gamma
        End Get
        Set(ByVal value As Double)
            m_Gamma = value
        End Set
    End Property
    Property Rho As Double
        Get
            Return m_Rho
        End Get
        Set(ByVal value As Double)
            m_Rho = value
        End Set
    End Property
    Property Theta As Double
        Get
            Return m_Theta
        End Get
        Set(ByVal value As Double)
            m_Theta = value
        End Set
    End Property
    Property Vega As Double
        Get
            Return m_Vega
        End Get
        Set(ByVal value As Double)
            m_Vega = value
        End Set
    End Property
    Property AvgPrice As Double
        Get
            Return m_AvgPrice
        End Get
        Set(ByVal value As Double)
            m_AvgPrice = value
        End Set
    End Property
    Property Volatility As Double
        Get
            Return m_Volatility
        End Get
        Set(ByVal value As Double)
            m_Volatility = value
        End Set
    End Property
    Property Skew As Double
        Get
            Return m_Skew
        End Get
        Set(ByVal value As Double)
            m_Skew = value
        End Set
    End Property
    Property RiskFreeRate As Double
        Get
            Return m_RiskFreeRate
        End Get
        Set(ByVal value As Double)
            m_RiskFreeRate = value
        End Set
    End Property
    Property RouteId As Integer
        Get
            Return m_RouteID
        End Get
        Set(ByVal value As Integer)
            m_RouteID = value
        End Set
    End Property
    Property YY1 As Integer
        Get
            Return m_YY1
        End Get
        Set(ByVal value As Integer)
            m_YY1 = value
        End Set
    End Property
    Property MM1 As Integer
        Get
            Return m_MM1
        End Get
        Set(ByVal value As Integer)
            m_MM1 = value
        End Set
    End Property
    Property YY2 As Integer
        Get
            Return m_YY2
        End Get
        Set(ByVal value As Integer)
            m_YY2 = value
        End Set
    End Property
    Property MM2 As Integer
        Get
            Return m_MM2
        End Get
        Set(ByVal value As Integer)
            m_MM2 = value
        End Set
    End Property
    Property AvgIncludesToday As Boolean
        Get
            Return m_AvgIncludesToday
        End Get
        Set(value As Boolean)
            m_AvgIncludesToday = value
        End Set
    End Property
    Property AverageType As OptionAverageTypeEnum
        Get
            Return m_AverageType
        End Get
        Set(value As OptionAverageTypeEnum)
            m_AverageType = value
        End Set
    End Property
    Property Holidays As List(Of DateTime)
        Get
            Return m_Holidays
        End Get
        Set(value As List(Of DateTime))
            m_Holidays = value
        End Set
    End Property
    Public Property BSVolatility As Double
        Get
            Return m_BSVolatility
        End Get
        Set(value As Double)
            m_BSVolatility = value
        End Set
    End Property
#End Region

End Class

Public Class OptionButtonClass
    Private m_ButtonPressed As OptionButtonEnum
    Private m_ActiveLeg As Integer

    Property ButtonPressed As OptionButtonEnum
        Get
            Return m_ButtonPressed
        End Get
        Set(value As OptionButtonEnum)
            m_ButtonPressed = value
        End Set
    End Property
    Property ActiveLeg As Integer
        Get
            Return m_ActiveLeg
        End Get
        Set(value As Integer)
            m_ActiveLeg = value
        End Set
    End Property
End Class

Public Class BalticFTPClass
    Public ROUTE_ID As Integer
    Public FIXING_DATE As Date
    Public FIXING As Double
    Public NORMFIX As Double
    Public YY1 As Integer
    Public YY2 As Integer
    Public MM1 As Integer
    Public MM2 As Integer
    Public KEY As String
    Public PERIOD As Integer
End Class

#Region "Enumaretions"
Public Enum OptionButtonEnum
    Price = 1
    Vol = 2
    Strike = 3
End Enum
Public Enum OptionBSEnum
    Sell = -1
    Void = 0
    Buy = 1
End Enum
Public Enum OptionTypeEnum
    OPut = -1
    OCall = 1
End Enum
Public Enum OptionStrategyEnum
    OVanilla = 1
    OSpread = 2
    OStrangle = 3
    OStraddle = 4
    OCollar = 5
End Enum
Public Enum OptionSolveForEnum
    Void = 0
    Price = 1
    Vol = 2
    Strike = 3
End Enum
Public Enum OptionAverageTypeEnum
    MonthAverage = 0
    FirstSevenDays = 1
    LastSevenDays = 2
End Enum
Public Enum OptionSolverEnum
    Analytic = 0
    MC_Processor = 1
    MC_CUDA = 2
End Enum
#End Region

Public Class MMonths
    Public Sub New(ByVal MM As Integer, ByVal MMD As String)
        _MM = MM
        _MMD = MMD
    End Sub
    Private _MM As Integer
    Private _MMD As String
    Public Property MM() As Integer
        Get
            Return _MM
        End Get
        Set(ByVal value As Integer)
            _MM = value
        End Set
    End Property
    Public Property MMD() As String
        Get
            Return _MMD
        End Get
        Set(ByVal value As String)
            _MMD = value
        End Set
    End Property
End Class
Public Class YYears
    Public Sub New(ByVal f_YY As Integer, ByVal f_YYD As String)
        _YY = f_YY
        _YYD = f_YYD
    End Sub
    Private _YY As Integer
    Private _YYD As String
    Public Property YY() As Integer
        Get
            Return _YY
        End Get
        Set(ByVal value As Integer)
            _YY = value
        End Set
    End Property
    Public Property YYD() As String
        Get
            Return _YYD
        End Get
        Set(ByVal value As String)
            _YYD = value
        End Set
    End Property
End Class
Public Class InterestRates
    Public Sub New(ByVal CCY_ID As Integer, ByVal PERIOD As Integer, ByVal FIXING As Double)
        _CCY_ID = CCY_ID
        _PERIOD = PERIOD
        _FIXING = FIXING
    End Sub
    Private _CCY_ID As Integer
    Private _PERIOD As Integer
    Private _FIXING As Double
    Public Property CCY_ID() As Integer
        Get
            Return _CCY_ID
        End Get
        Set(ByVal value As Integer)
            _CCY_ID = value
        End Set
    End Property
    Public Property PERIOD() As Integer
        Get
            Return _PERIOD
        End Get
        Set(ByVal value As Integer)
            _PERIOD = value
        End Set
    End Property
    Public Property FIXING() As Double
        Get
            Return _FIXING
        End Get
        Set(ByVal value As Double)
            _FIXING = value
        End Set
    End Property
End Class
Public Class BS
    Public Sub New(ByVal BS As Integer, ByVal Des As String)
        _BS = BS
        _Des = Des
    End Sub
    Private _BS As Integer
    Private _Des As String

    Public Property BS() As Integer
        Get
            Return _BS
        End Get
        Set(ByVal value As Integer)
            _BS = value
        End Set
    End Property

    Public Property Des() As String
        Get
            Return _Des
        End Get
        Set(ByVal value As String)
            _Des = value
        End Set
    End Property
End Class
Public Class OptionType
    Public Sub New(ByVal OptionType As Integer, ByVal Des As String)
        _OptionType = OptionType
        _Des = Des
    End Sub
    Private _OptionType As Integer
    Private _Des As String

    Public Property OptionType() As Integer
        Get
            Return _OptionType
        End Get
        Set(ByVal value As Integer)
            _OptionType = value
        End Set
    End Property

    Public Property Des() As String
        Get
            Return _Des
        End Get
        Set(ByVal value As String)
            _Des = value
        End Set
    End Property
End Class
Public Class TradeType
    Public Sub New(ByVal TradeType As Integer, ByVal Des As String)
        _TradeType = TradeType
        _Des = Des
    End Sub

    Private _TradeType As Integer
    Private _Des As String

    Public Property TradeType() As Integer
        Get
            Return _TradeType
        End Get
        Set(ByVal value As Integer)
            _TradeType = value
        End Set
    End Property

    Public Property Des() As String
        Get
            Return _Des
        End Get
        Set(ByVal value As String)
            _Des = value
        End Set
    End Property
End Class
Public Class TCMonthsClass

    Public Sub New()

    End Sub
    Public Sub New(ByVal _MM As Integer, ByVal _YY As Integer, ByVal _SERVERDATE As DateTime)
        m_SERVERDATE = _SERVERDATE
        m_MM = _MM
        m_YY = _YY
        m_YYMM = New YYMMClass(_MM, _YY)
    End Sub
    Private m_MM As Integer
    Private m_YY As Integer
    Private m_YYMM As YYMMClass
    Private m_DES As String
    Private m_SERVERDATE As DateTime

    Public ReadOnly Property NoMonths As Integer
        Get
            Return DateDiff(DateInterval.Month, m_SERVERDATE, DateSerial(m_YY, m_MM, 1))
        End Get
    End Property
    Public Property SERVERDATE As DateTime
        Get
            Return m_SERVERDATE
        End Get
        Set(value As DateTime)
            m_SERVERDATE = value
        End Set
    End Property
    Public Property MM() As Integer
        Get
            Return m_MM
        End Get
        Set(ByVal value As Integer)
            m_MM = value
        End Set
    End Property
    Public Property YY() As Integer
        Get
            Return m_YY
        End Get
        Set(ByVal value As Integer)
            m_YY = value
        End Set
    End Property
    Public Property YYMM As YYMMClass
        Get
            Return m_YYMM
        End Get
        Set(value As YYMMClass)
            m_YYMM = value
        End Set
    End Property
    Public ReadOnly Property DES As String
        Get
            Dim tdate As DateTime = New DateTime(m_YY, m_MM, 1)
            Return LocalMonthName(tdate.Month) & "-" & Format(tdate, "yy")
        End Get
    End Property
End Class
Public Class YYMMClass
    Private m_MM As Integer
    Private m_YY As Integer

    Public Sub New()

    End Sub
    Public Sub New(ByVal _MM As Integer, ByVal _YY As Integer)
        m_MM = _MM
        m_YY = _YY
    End Sub
    Public Property MM As Integer
        Get
            Return m_MM
        End Get
        Set(value As Integer)
            m_MM = value
        End Set
    End Property
    Public Property YY As Integer
        Get
            Return m_YY
        End Get
        Set(value As Integer)
            m_YY = value
        End Set
    End Property
End Class

