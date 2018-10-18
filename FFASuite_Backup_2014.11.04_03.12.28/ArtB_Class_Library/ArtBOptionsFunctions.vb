Module ArtBOptionsFunctions
    Public Function FormatStringPrice(ByVal PRICING_TICK As Double) As String
        Select Case Math.Log10(PRICING_TICK)
            Case Is >= 0
                FormatStringPrice = "#,##"
            Case -1
                FormatStringPrice = "0.0"
            Case -2
                FormatStringPrice = "0.00"
            Case -3
                FormatStringPrice = "0.000"
            Case -4
                FormatStringPrice = "0.0000"
            Case Else
                FormatStringPrice = "#,##"
        End Select
    End Function
End Module
