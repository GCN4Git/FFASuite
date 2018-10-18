Module ArtB_Class_Library_Module
    Public ArtBConnectionStr As String = ""
    Public RatioSpreadPrecision As Double = 0.0
    Public IndicativeOrdersBanMinutes As Integer = 20
    Public RatioVolatility As Double = 0.05
    Public HistoricRatioVolatility As Double = 0.05
    Public GlobalTopPrice As Double = 1000000.0
    Public MIPPrefBonus As Double = 100
    Public MIPTopPrice As Double = 200000.0
    Public MIPTopRatioPrice As Double = 1000.0
    Public MIPMinRatioPrice As Double = 0.001
    Public SynthOrderOffset As Integer = 1000000000
    Public GlobalMatchingTrader As Integer = -1
    Public GlobalMatchingDesk As Integer = -1
    Public GlobalExchangeStr As String = "*"
    Public SystemDeskTraderId As Integer = 1
End Module
