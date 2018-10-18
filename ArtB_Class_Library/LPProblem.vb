

Public Class LPVariable
    Public Kind As Integer = GLP_IV
    Public Coef As Double = 0
    Public BoundsType As Integer = GLP_DB
    Public BoundsLow As Double = 0
    Public BoundsHigh As Double = 1
    Public Name As String = ""
    Public Descr As String = ""
End Class

Public Class LPConstraint
    Public BoundsType As Integer = GLP_DB
    Public BoundsLow As Double = 0
    Public BoundsHigh As Double = 1
    Public Descr As String = ""
End Class

Public Class LPProblem
    Declare Auto Function glp_create_prob Lib "glpk_4_36.dll" () As Integer
    Declare Auto Sub glp_set_obj_dir Lib "glpk_4_36.dll" (ByVal lp As Integer, ByVal dir As Integer)
    Declare Auto Sub glp_add_rows Lib "glpk_4_36.dll" (ByVal lp As Integer, ByVal nrs As Integer)
    Declare Auto Sub glp_add_cols Lib "glpk_4_36.dll" (ByVal lp As Integer, ByVal ncs As Integer)
    Declare Auto Sub glp_set_row_bnds Lib "glpk_4_36.dll" _
        (ByVal lp As Integer, ByVal i As Integer, ByVal type As Integer, ByVal lb As Double, ByVal ub As Double)
    Declare Auto Sub glp_set_col_bnds Lib "glpk_4_36.dll" _
        (ByVal lp As Integer, ByVal j As Integer, ByVal type As Integer, ByVal lb As Double, ByVal ub As Double)
    Declare Auto Sub glp_set_obj_coef Lib "glpk_4_36.dll" _
        (ByVal lp As Integer, ByVal j As Integer, ByVal coef As Double)
    Declare Auto Sub glp_load_matrix Lib "glpk_4_36.dll" _
        (ByVal lp As Integer, ByVal ne As Integer, _
         ByVal ia() As Integer, ByVal ja() As Integer, ByVal ar() As Double)
    Declare Auto Sub glp_simplex Lib "glpk_4_36.dll" _
       (ByVal lp As Integer, ByVal param As Integer)
    Declare Auto Function glp_intopt Lib "glpk_4_36.dll" _
       (ByVal lp As Integer, ByVal param As Integer) As Integer
    Declare Auto Function glp_get_obj_val Lib "glpk_4_36.dll" (ByVal lp As Integer) As Double
    Declare Auto Function glp_mip_obj_val Lib "glpk_4_36.dll" (ByVal lp As Integer) As Double
    Declare Auto Function glp_get_col_prim Lib "glpk_4_36.dll" _
       (ByVal lp As Integer, ByVal j As Integer) As Double
    Declare Auto Function glp_set_col_kind Lib "glpk_4_36.dll" _
       (ByVal lp As Integer, ByVal j As Integer, ByVal kind As Integer) As Double
    Declare Auto Function glp_mip_status Lib "glpk_4_36.dll" _
       (ByVal lp As Integer) As Integer

    Declare Auto Function glp_mip_col_val Lib "glpk_4_36.dll" _
       (ByVal lp As Integer, ByVal j As Integer) As Double
    Declare Auto Function glp_delete_prob Lib "glpk_4_36.dll" (ByVal lp As Integer) As Integer

    Public lp As Integer = 0

    Public ElementsArraySize As Integer = 0
    Public RowsArraySize As Integer = 0
    Public ColumnsArraySize As Integer = 0
    Public RowsNum As Integer = 1
    Public ColumnsNum As Integer = 1
    Public ElementsNum As Integer = 1
    Public ElementsRow() As Integer
    Public ElementsColumn() As Integer
    Public ElementsConstant() As Double

    Public Columns() As LPVariable
    Public Rows() As LPConstraint

    Public Sub New(Optional ByVal a_InitialColumns As Integer = 1000, _
                   Optional ByVal a_InitialRows As Integer = 1000, _
                   Optional ByVal a_InitialArraySize As Integer = 10000)
        lp = glp_create_prob()
        glp_set_obj_dir(lp, GLP_MAX)
        
        ElementsArraySize = a_InitialArraySize
        ReDim ElementsRow(0 To a_InitialArraySize)
        ReDim ElementsColumn(0 To a_InitialArraySize)
        ReDim ElementsConstant(0 To a_InitialArraySize)
        ColumnsArraySize = a_InitialColumns
        ReDim Columns(0 To ColumnsArraySize)

        RowsArraySize = a_InitialRows
        ReDim Rows(0 To RowsArraySize)
    End Sub

    Public Function AddVariable(Optional ByVal Kind As Integer = GLP_IV, _
                           Optional ByVal Coef As Double = 0, _
                           Optional ByVal BoundsType As Integer = GLP_DB, _
                           Optional ByVal BoundsLow As Double = 0, _
                           Optional ByVal BoundsHigh As Double = 1, _
                           Optional ByVal Name As String = "", _
                           Optional ByVal Descr As String = "") As Integer
        If ColumnsNum >= ColumnsArraySize Then
            ColumnsArraySize = Int(ColumnsArraySize * 1.1)
            ReDim Preserve Columns(0 To ColumnsArraySize)
        End If
        AddVariable = ColumnsNum
        Columns(ColumnsNum) = New LPVariable
        Columns(ColumnsNum).Kind = Kind
        Columns(ColumnsNum).Coef = Coef
        Columns(ColumnsNum).BoundsType = BoundsType
        Columns(ColumnsNum).BoundsLow = BoundsLow
        Columns(ColumnsNum).BoundsHigh = BoundsHigh
        Columns(ColumnsNum).Name = Name
        Columns(ColumnsNum).Descr = Descr
        ColumnsNum = ColumnsNum + 1
    End Function

    Public Function AddConstraint(Optional ByVal BoundsType As Integer = GLP_DB, _
                             Optional ByVal BoundsLow As Double = 0, _
                             Optional ByVal BoundsHigh As Double = 1, _
                             Optional ByVal Descr As String = "") As Integer
        If RowsNum >= RowsArraySize Then
            RowsArraySize = Int(RowsArraySize * 1.1)
            ReDim Preserve Rows(0 To RowsArraySize)
        End If
        AddConstraint = RowsNum
        Rows(RowsNum) = New LPConstraint
        Rows(RowsNum).BoundsType = BoundsType
        Rows(RowsNum).BoundsLow = BoundsLow
        Rows(RowsNum).BoundsHigh = BoundsHigh
        Rows(RowsNum).Descr = Descr
        RowsNum = RowsNum + 1
    End Function

    Public Function AddElement(ByVal Variable As Integer, _
                          ByVal Constraint As Integer, _
                          ByVal Constant As Double) As Boolean
        If ElementsNum >= ElementsArraySize Then
            ElementsArraySize = Int(ElementsArraySize * 1.1)
            ReDim Preserve ElementsRow(0 To ElementsArraySize)
            ReDim Preserve ElementsColumn(0 To ElementsArraySize)
            ReDim Preserve ElementsConstant(0 To ElementsArraySize)
        End If
        Dim i As Integer
        For i = 1 To ElementsNum - 1
            If ElementsRow(i) = Constraint And ElementsColumn(i) = Variable Then
                Return False
            End If
            If Variable < 1 Or Variable >= ColumnsNum Then
                Return False
            End If
            If Constraint < 1 Or Constraint >= RowsNum Then
                Return False
            End If
        Next
        ElementsRow(ElementsNum) = Constraint
        ElementsColumn(ElementsNum) = Variable
        ElementsConstant(ElementsNum) = Constant
        ElementsNum = ElementsNum + 1
        Return True
    End Function

    Public Sub Report()
        Dim i As Integer, j As Integer
        Dim vs As String
        Dim s As String
        Dim s2 As String
        Dim ds As String
        Exit Sub
        For i = 1 To ColumnsNum - 1
            vs = ""
            If Len(Columns(i).Name) > 0 Then
                vs = Columns(i).Name
            Else
                vs = "V" & i.ToString()
            End If
            If Len(Columns(i).Descr) > 0 Then
                vs = vs & ", " & Columns(i).Descr
            End If

            s = "Int"
            If Columns(i).Kind = GLP_CV Then s = "Real"
            If Columns(i).Kind = GLP_BV Then s = "Binary"
            ds = vs & ": " & s & " coef= " & Columns(i).Coef & ", "
            Select Case Columns(i).BoundsType
                Case GLP_FX
                    ds = ds & " = " & Columns(i).BoundsLow.ToString
                Case GLP_LO
                    ds = ds & " >= " & Columns(i).BoundsLow.ToString
                Case GLP_UP
                    ds = ds & " <= " & Columns(i).BoundsHigh.ToString
                Case GLP_DB
                    ds = ds & " e [" & Columns(i).BoundsLow.ToString & ", " & Columns(i).BoundsHigh.ToString & "]"
                Case GLP_FR
                    ds = ds & " e [-oo, +oo]"
            End Select
            Debug.Print(ds)
        Next

        For i = 1 To RowsNum - 1
            'ds = "Constr" & i.ToString & " " & Rows(i).Descr & ": "
            ds = Rows(i).Descr & ": "
            For j = 1 To ElementsNum - 1
                If ElementsRow(j) = i Then
                    vs = "V" & ElementsColumn(j)
                    If Len(Columns(ElementsColumn(j)).Name) > 0 Then
                        vs = Columns(ElementsColumn(j)).Name
                    End If
                    If ElementsConstant(j) > 0 Then
                        ds = ds & " +" & ElementsConstant(j).ToString & vs
                    ElseIf ElementsConstant(j) < 0 Then
                        ds = ds & " " & ElementsConstant(j).ToString & vs
                    End If
                End If
            Next
            Select Case Rows(i).BoundsType
                Case GLP_DB
                    ds = ds & " e [" & Rows(i).BoundsLow.ToString & ", " & Rows(i).BoundsHigh.ToString & "]"
                Case GLP_LO
                    ds = ds & " >= " & Rows(i).BoundsLow.ToString
                Case GLP_UP
                    ds = ds & " <= " & Rows(i).BoundsHigh.ToString
                Case GLP_FX
                    ds = ds & " = " & Rows(i).BoundsLow.ToString
            End Select
            Debug.Print(ds)
        Next i


    End Sub


    Public Function Solve() As Double
        Dim i As Integer, k As Integer
        Solve = -1.0E+20
        Report()
        glp_add_cols(lp, ColumnsNum - 1)

        For i = 1 To ColumnsNum - 1
            glp_set_col_kind(lp, i, Columns(i).Kind)
            glp_set_col_bnds(lp, i, Columns(i).BoundsType, Columns(i).BoundsLow, Columns(i).BoundsHigh)
            glp_set_obj_coef(lp, i, Columns(i).Coef)
        Next i

        glp_add_rows(lp, RowsNum - 1)
        For i = 1 To RowsNum - 1
            glp_set_row_bnds(lp, i, Rows(i).BoundsType, Rows(i).BoundsLow, Rows(i).BoundsHigh)
        Next i
        glp_load_matrix(lp, ElementsNum - 1, ElementsRow, ElementsColumn, ElementsConstant)

        glp_simplex(lp, 0)
        i = glp_intopt(lp, 0)
        k = glp_mip_status(lp)

        Dim z As Double = glp_mip_obj_val(lp)
        If i = 0 Then
            Solve = z
        Else
            Solve = -1.0E+20
        End If
        Debug.Print("Solution:" & Solve.ToString())
    End Function

    Public Function GetVariableValue(ByVal i As Integer) As Double
        GetVariableValue = glp_mip_col_val(lp, i)
    End Function

    Public Sub Destroy()
        Dim i As Integer
        Try
            If lp <> 0 Then glp_delete_prob(lp)
        Catch
        End Try
        Try
            For i = 1 To RowsNum
                Rows(i) = Nothing
            Next
            Erase Rows
        Catch
        End Try
        Try
            For i = 1 To ColumnsNum
                Columns(i) = Nothing
            Next
            Erase Columns
        Catch
        End Try
        Try
            Erase ElementsRow
            Erase ElementsColumn
            Erase ElementsConstant
        Catch
        End Try
    End Sub

End Class

Public Class MARKET_MATCHING_CLASS
    Public BuyOrder As ORDERS_FFA_CLASS = Nothing
    Public SellOrder As ORDERS_FFA_CLASS = Nothing
    Public BQ As Integer = 0
    Public SQ As Integer = 0
    Public BuyOrderSpread As ORDERS_FFA_CLASS = Nothing
    Public BuySpreadID As Integer = 0
    Public SellOrderSpread As ORDERS_FFA_CLASS = Nothing
    Public SellSpreadID As Integer = 0
    Public ActualQuantity As Double = 0
    Public ExchangeID As Integer = 0
    Public VariableEQ As Integer = -1
    Public VariableBinEQ As Integer = -1
    Public MaxQuantinty As Integer = 0
    Public MaxVariableValue As Integer = 0
    Public Price As Double = 0
    Public BuyExchangeRank As Double = 0
    Public SellExchangeRank As Double = 0
    Public PriceVariable As Integer = -1
    Public VariableECap As Integer = -1
    Public VariableEGain As Integer = -1
    Public VariableSGain As Integer = -1
    Public Bucket As Boolean = False
    Public FreePrice As Boolean = False
    Public BuyRefPrice As Double = 1.0E+20
    Public SellRefPrice As Double = -1.0E+20
    Public BuySpread As MARKET_SPREAD_CLASS = Nothing
    Public SellSpread As MARKET_SPREAD_CLASS = Nothing
    Public Tick As Double = 1
    Public LotSize As Double = 1
    Public BuyTime As Double = 0
    Public SellTime As Double = 0
    Public TotalTime As Double = 0
    Public BuyRank As Integer = 0
    Public SellRank As Integer = 0
    Public TotalRank As Integer = 0
    Public MOB As MARKET_ORDER_CLASS = Nothing
    Public MOS As MARKET_ORDER_CLASS = Nothing
    Public MOBX As MARKET_ORDER_EXCHANGE_CLASS = Nothing
    Public MOSX As MARKET_ORDER_EXCHANGE_CLASS = Nothing
    Public Prefered As Integer = 0
    Public BuyPrefered As Boolean = False
    Public SellPrefered As Boolean = False
    Public ID As Integer = 0
    Public LowPrice As Double = -1.0E+20
    Public HighPrice As Double = 1.0E+20
    Public VariablePenalty As Integer = -1

    Public Sub FixPreferd(ByVal PrefOrderId As Integer, ByVal PrefOrderIdEnd As Integer)
        Prefered = 0
        BuyPrefered = False
        SellPrefered = False
        If Not IsNothing(BuyOrder) Then
            If BuyOrder.ORDER_ID >= PrefOrderId And BuyOrder.ORDER_ID <= PrefOrderIdEnd Then
                BuyPrefered = True
            End If
            If BuySpreadID = 0 Then BuySpreadID = NullInt2Int(BuyOrder.SPREAD_ORDER_ID)
            If BuySpreadID >= PrefOrderId And BuySpreadID <= PrefOrderIdEnd Then
                BuyPrefered = True
            End If
        End If
        If Not IsNothing(SellOrder) Then
            If SellOrder.ORDER_ID >= PrefOrderId And SellOrder.ORDER_ID <= PrefOrderIdEnd Then
                SellPrefered = True
            End If
            If SellSpreadID = 0 Then SellSpreadID = NullInt2Int(SellOrder.SPREAD_ORDER_ID)
            If SellSpreadID >= PrefOrderId And SellSpreadID <= PrefOrderIdEnd Then
                SellPrefered = True
            End If
        End If
        If BuyPrefered Then Prefered = 1
        If SellPrefered Then Prefered = Prefered Or 2
    End Sub

End Class

Public Class MARKET_PAIR_CLASS
    Public Order1 As Object = Nothing
    Public Order2 As Object = Nothing
    Public SPXRow As Integer = -1
End Class

Public Class MARKET_ORDER_CLASS
    Public Order As Object = Nothing
    Public TotQRow As Integer = -1
    Public SXXRow As Integer = -1
    Public VariableX As Integer = -1
    Public VariableBucketX As Integer = -1
    Public BucketStepRow As Integer = -1

    Public Exchanges As New Collection
    Public NonBucketGERow As Integer = -1
    Public NonBucketLERow As Integer = -1
    Public NonBucketFixQRow As Integer = -1
    Public PriceVariable As Integer = -1
    Public CapRow As Integer = -1
    Public VariableGain As Integer = -1
    Public bValidForQStep As Boolean = True
    Public VariableExchange As Integer = -1
    Public ExchangeRow As Integer = -1

    Public Sub New(ByRef O As Object, _
                   ByVal Q As Integer, _
                   ByRef lp As LPProblem, _
                   ByVal ExchangeId As Integer)
        Order = O
        Dim OrderName As String = Order.ORDER_ID.ToString
        TotQRow = lp.AddConstraint(GLP_DB, 0, Q, OrderName & " Total exec")
        If Order.ORDER_BS = "B" Then
            CapRow = lp.AddConstraint(GLP_UP, 0, 0, " Total cap")
        Else
            CapRow = lp.AddConstraint(GLP_LO, 0, 0, " Total cap")
        End If

        If O.SINGLE_EXCHANGE_EXECUTION Then
            SXXRow = lp.AddConstraint(GLP_DB, 0, 1, OrderName & " SXX")
            VariableExchange = lp.AddVariable(GLP_IV, 0, GLP_DB, 0, 0, "E" & OrderName)
            ExchangeRow = lp.AddConstraint(GLP_FX, 0, 0, OrderName & " Exchange constrain")
            lp.AddElement(VariableExchange, ExchangeRow, 1)
        End If

        Exchanges.Clear()
        If O.FLEXIBLE_QUANTITY <> OrderFlexQuantinty.Bucket Then
            VariableX = lp.AddVariable(GLP_BV, 0, GLP_DB, 0, 1, "C" & OrderName, OrderName & " exec complete")
            NonBucketGERow = lp.AddConstraint(GLP_LO, 0, 0, OrderName & " NonBucketGE")
            NonBucketLERow = lp.AddConstraint(GLP_UP, 0, 0, OrderName & " NonBucketLE")
            lp.AddElement(VariableX, NonBucketGERow, 1)
            NonBucketFixQRow = lp.AddConstraint(GLP_LO, 0, 0, OrderName & " NonBucketFixQ")
            lp.AddElement(VariableX, NonBucketFixQRow, -1)
        End If
    End Sub

    Public Sub FinalizeConstaints(ByRef lp As LPProblem)
        Dim n As Double = Exchanges.Count
        If (n <= 0) Then Exit Sub
        If VariableX = -1 Then Exit Sub
        For Each x As MARKET_ORDER_EXCHANGE_CLASS In Exchanges
            lp.AddElement(x.VariableEX, NonBucketGERow, -1 / n)
            lp.AddElement(x.VariableEX, NonBucketLERow, -1)
        Next
        lp.AddElement(VariableX, NonBucketLERow, 1 / n)
    End Sub

End Class

Public Class MARKET_ORDER_EXCHANGE_CLASS
    Public GERow As Integer = -1
    Public LERow As Integer = -1
    Public VariableEX As Integer = -1
    Public Parent As MARKET_ORDER_CLASS = Nothing
    Public ExchangeID As Integer = 0
    Public CapXVariable As Integer = -1

    Public Sub New(ByRef a_Parent As MARKET_ORDER_CLASS, _
                   ByRef lp As LPProblem, ByVal a_ExchangeId As Integer)
        Parent = a_Parent
        Dim VarName As String = Parent.Order.Order_ID.ToString & "/" & a_ExchangeId.ToString
        GERow = lp.AddConstraint(GLP_LO, 0, 0, VarName & " GERow")
        LERow = lp.AddConstraint(GLP_UP, 0, 0, VarName & " LERow")
        VariableEX = lp.AddVariable(GLP_IV, 0, GLP_DB, 0, 1, "X" & VarName)
        lp.AddElement(VariableEX, GERow, -1)
        lp.AddElement(VariableEX, LERow, -1)
        If Parent.SXXRow >= 0 Then
            lp.AddElement(VariableEX, Parent.SXXRow, 1)
        End If
        ExchangeID = a_ExchangeId
        If Parent.VariableExchange <> -1 Then
            lp.Columns(Parent.VariableExchange).BoundsHigh = Max(lp.Columns(Parent.VariableExchange).BoundsHigh, ExchangeID)
            lp.AddElement(VariableEX, Parent.ExchangeRow, -ExchangeID)
        End If
    End Sub
End Class

Public Class ORDER_EXCHANGE_MATCHING_CLASS
    Public GERow As Integer = -1
    Public LERow As Integer = -1
    Public VariableEX As Integer = -1
    Public ExchangeID As Integer = 0
    Public Ranking As Double = 0

    Public Sub New(ByVal SXXRow As Integer, _
                   ByRef lp As LPProblem, ByVal a_ExchangeId As Integer)
        GERow = lp.AddConstraint(GLP_LO, 0, 0)
        LERow = lp.AddConstraint(GLP_UP, 0, 0)
        VariableEX = lp.AddVariable(GLP_IV, 0, GLP_DB, 0, 1)
        lp.AddElement(VariableEX, GERow, -1)
        lp.AddElement(VariableEX, LERow, -1)
        If SXXRow >= 0 Then
            lp.AddElement(VariableEX, SXXRow, 1)
        End If
        ExchangeID = a_ExchangeId
    End Sub

End Class

Public Class MARKET_SPREAD_CLASS
    Public SpreadOrder As Object = Nothing
    Public Leg1Order As Object = Nothing
    Public Leg2Order As Object = Nothing
    Public LegQuantinyBalanceRow As Integer = -1
    Public TotalExecRow As Integer = -1
    Public QuantityRatio As Double = 0
    Public PriceRow As Integer = -1
    Public MinTick As Double = 0
    Public CalendarQModifier As Double = 0
    Public PriceQModifier As Double = 0
    Public SwapSpreadPriceBalanceRow As Integer = -1
    Public Leg1Price As Double = 0
    Public Leg2Price As Double = 0
    Public Leg1Q As Double = 0
    Public Leg2Q As Double = 0
    Public SpreadPriceAdjust As Double = 0
    Public SXXRow As Integer = -1
    Public bSXXRow As Boolean = True
    Public GainRow As Integer = -1
    Public GainCoef As Double = 0
    Public OrderTime As Double = 0
    Public SpreadRank As Integer = 0
    Public OrderRank As Double
    Public SpreadRank2 As Integer = 0
    Public VariableGain As Integer = -1
    Public PreferedSpread As Boolean = False
    Public PriceConstraintLow(0 To 1) As Integer
    Public PriceConstraintHigh(0 To 1) As Integer
    Public VariablePricePlus(0 To 1) As Integer
    Public VariablePriceMinus(0 To 1) As Integer
    Public PricePlusMinusRow(0 To 1) As Integer
    Public PriceLeg(0 To 1) As Integer
    Public PriceRPPL(0 To 1) As RoutePeriodPriceLimits
    Public ActualQuantityRatio As Double = 0

    Public Sub New(ByRef SO As Object, _
                   ByRef lp As LPProblem, _
                   Optional ByVal TopPrice As Double = 200000)
        SpreadOrder = SO
        Dim OrderName As String = SO.ORDER_ID
        LegQuantinyBalanceRow = lp.AddConstraint(GLP_FX, 0, 0, OrderName & " LegQuantinyBalance")
        TotalExecRow = lp.AddConstraint(GLP_DB, 0, 1, OrderName & " Spread vs Legs Total Execution")
        'SwapSpreadPriceBalanceRow = lp.AddConstraint(GLP_FX, 0, 0, OrderName & " SwapSpreadPriceBalance")
        If SO.ORDER_TYPE = OrderTypes.RatioSpread Then
            MinTick = RatioSpreadPrecision

            If SO.ORDER_BS = "B" Then
                PriceRow = lp.AddConstraint(GLP_UP, 0, 0, OrderName & " B Ratio Spread Price constr")
            Else
                PriceRow = lp.AddConstraint(GLP_LO, 0, 0, OrderName & " S Ratio Spread Price constr")
            End If
        ElseIf SO.ORDER_TYPE = OrderTypes.CalendarSpread Then
            PriceQModifier = SO.PRICE_INDICATED
            If SO.ORDER_BS = "B" Then
                PriceRow = lp.AddConstraint(GLP_UP, 0, 0, OrderName & " B Calendar Price constr")
            Else
                PriceRow = lp.AddConstraint(GLP_LO, 0, 0, OrderName & " S Calendar Price constr")
            End If
        ElseIf SO.ORDER_TYPE = OrderTypes.PriceSpread Then
            PriceQModifier = SO.PRICE_INDICATED
            If SO.ORDER_BS = "B" Then
                PriceRow = lp.AddConstraint(GLP_UP, 0, 0, OrderName & " B Price Spread constr")
            Else
                PriceRow = lp.AddConstraint(GLP_LO, 0, 0, OrderName & " S Price Spread constr")
            End If
        End If

        If SO.SINGLE_EXCHANGE_EXECUTION <> 0 Then
            SXXRow = lp.AddConstraint(GLP_FX, 0, 0, OrderName & " Spread SXX")
            bSXXRow = False
        End If

        PriceLeg(0) = -1
        PriceLeg(1) = -1

    End Sub

    Public Sub CalcActualQuantityRatio(ByVal SQ1 As Double, ByVal SQ2 As Double)
        ActualQuantityRatio = QuantityRatio
        Dim tp1 As New ArtBTimePeriod
        tp1.FillMY(SpreadOrder.mm1, SpreadOrder.yy1, SpreadOrder.mm2, SpreadOrder.yy2)
        Dim m1 As Double = tp1.TotalMonths()
         Dim tp2 As New ArtBTimePeriod
        tp2.FillMY(SpreadOrder.mm21, SpreadOrder.yy21, SpreadOrder.mm22, SpreadOrder.yy22)
        Dim m2 As Double = tp2.TotalMonths()
        If SQ2 > 0 And SQ1 > 0 And m1 > 0 And m2 > 0 Then
            ActualQuantityRatio = (SQ2 * m2) / (SQ1 * m1)
        End If
        tp1 = Nothing
        tp2 = Nothing
    End Sub

    Public Sub CreateAllPriceConstraints(ByRef lp As LPProblem, ByRef RPPLCol As Collection, ByRef VC As GlobalViewClass)
        Dim RPStr0 As String = VC.RoutePeriodStringFromObj(SpreadOrder, 1)
        Dim RPStr1 As String = VC.RoutePeriodStringFromObj(SpreadOrder, 2)
        Dim RPPL0 As RoutePeriodPriceLimits = GetViewClass(RPPLCol, RPStr0)
        If Not IsNothing(RPPL0) Then CreatePriceConstraints(lp, RPPL0, 0)
        Dim RPPL1 As RoutePeriodPriceLimits = GetViewClass(RPPLCol, RPStr1)
        If Not IsNothing(RPPL1) Then CreatePriceConstraints(lp, RPPL1, 1)
    End Sub

    Public Sub CreatePriceConstraints(ByRef lp As LPProblem, ByRef RPPL As RoutePeriodPriceLimits, ByVal iLeg As Integer)
        PriceRPPL(iLeg) = RPPL
        PriceLeg(iLeg) = iLeg
        Dim OrderName As String = SpreadOrder.ORDER_ID.ToString()
        If ActualQuantityRatio <> 1 Then
            PriceConstraintLow(iLeg) = lp.AddConstraint(GLP_LO, 0, 0, OrderName & " Spread Price Low constr")
            PriceConstraintHigh(iLeg) = lp.AddConstraint(GLP_UP, 0, 0, OrderName & " Spread Price High constr")
        End If
        If iLeg = 1 Then Exit Sub
        VariablePricePlus(iLeg) = lp.AddVariable(GLP_CV, -0.001, GLP_DB, 0, 1.0E+20, "PR+" & OrderName)
        VariablePriceMinus(iLeg) = lp.AddVariable(GLP_CV, -0.001, GLP_DB, 0, 1.0E+20, "PR-" & OrderName)
        PricePlusMinusRow(iLeg) = lp.AddConstraint(GLP_FX, 0, 0, "PR+-" & OrderName & " constr")
        lp.AddElement(VariablePricePlus(iLeg), PricePlusMinusRow(iLeg), -1)
        lp.AddElement(VariablePriceMinus(iLeg), PricePlusMinusRow(iLeg), 1)
    End Sub

    Public Sub AddPriceConstraints(ByRef lp As LPProblem, ByRef oc As MARKET_MATCHING_CLASS, ByRef RPPL As RoutePeriodPriceLimits, ByVal iLeg As Integer)
        If IsNothing(PriceRPPL(iLeg)) Then Exit Sub
        If PriceRPPL(iLeg).Equals(RPPL) = False Then Exit Sub
        If PriceLeg(iLeg) = -1 Then Exit Sub
        Dim oLeg As Integer = 0
        If iLeg = 0 Then oLeg = 1
        Dim bApplyPriceConstraint As Boolean = True
        'If Not IsNothing(PriceRPPL(oLeg)) Then
        '    If PriceRPPL(oLeg).SpreadLimit > PriceRPPL(iLeg).SpreadLimit Then
        '        bApplyPriceConstraint = False
        '    ElseIf PriceRPPL(oLeg).SpreadLimit = PriceRPPL(iLeg).SpreadLimit Then
        '        If PriceRPPL(oLeg).LastTradeTime > PriceRPPL(iLeg).LastTradeTime Then
        '            bApplyPriceConstraint = False
        '        ElseIf PriceRPPL(oLeg).LastTradeTime = PriceRPPL(iLeg).LastTradeTime And PriceRPPL(oLeg).Span < PriceRPPL(iLeg).Span Then
        '            bApplyPriceConstraint = False
        '        End If
        '    End If
        'End If

        If bApplyPriceConstraint Or PriceRPPL(iLeg).MandatoryLow Then ' QuantityRatio <> 1 Then
            If ActualQuantityRatio <> 1 Then
                If Math.Abs(PriceRPPL(iLeg).SpreadLow) < 1.0E+19 Then
                    lp.AddElement(oc.VariableECap, PriceConstraintLow(iLeg), oc.Tick)
                    lp.AddElement(oc.VariableEQ, PriceConstraintLow(iLeg), -PriceRPPL(iLeg).SpreadLow)
                End If
            Else
                'If Math.Abs(PriceRPPL(iLeg).EqSpreadLow) < 1.0E+19 Then
                '    lp.AddElement(oc.VariableECap, PriceConstraintLow(iLeg), oc.Tick)
                '    lp.AddElement(oc.VariableEQ, PriceConstraintLow(iLeg), -PriceRPPL(iLeg).EqSpreadLow)
                'End If
            End If
        End If

        If bApplyPriceConstraint Or PriceRPPL(iLeg).MandatoryHigh Then ' QuantityRatio <> 1 Then
            If ActualQuantityRatio <> 1 Then
                If Math.Abs(PriceRPPL(iLeg).SpreadHigh) < 1.0E+19 Then
                    lp.AddElement(oc.VariableECap, PriceConstraintHigh(iLeg), oc.Tick)
                    lp.AddElement(oc.VariableEQ, PriceConstraintHigh(iLeg), -PriceRPPL(iLeg).SpreadHigh)
                End If
            Else
                'If Math.Abs(PriceRPPL(iLeg).EqSpreadHigh) < 1.0E+19 Then
                '    lp.AddElement(oc.VariableECap, PriceConstraintHigh(iLeg), oc.Tick)
                '    lp.AddElement(oc.VariableEQ, PriceConstraintHigh(iLeg), -PriceRPPL(iLeg).EqSpreadHigh)
                'End If
            End If
        End If

        If iLeg = 1 Then Exit Sub
        If Math.Abs(PriceRPPL(iLeg).PriceTarget) > 1.0E+19 Then Exit Sub
        lp.AddElement(oc.VariableECap, PricePlusMinusRow(iLeg), oc.Tick)
        lp.AddElement(oc.VariableEQ, PricePlusMinusRow(iLeg), -PriceRPPL(iLeg).PriceTarget)

    End Sub

    Public Sub CreateGain(ByRef lp As LPProblem, ByRef oc As MARKET_MATCHING_CLASS, ByVal GainCoef As Double)
        If VariableGain <> -1 Then Exit Sub
        Dim s As String
        s = SpreadOrder.ORDER_ID.ToString
        Dim bs As String = "Buy"
        If SpreadOrder.ORDER_BS = "S" Then bs = "Sell"

        VariableGain = lp.AddVariable(GLP_CV, GainCoef, GLP_FR, _
                                      -MIPTopPrice * oc.MaxQuantinty * oc.LotSize, _
                                      MIPTopPrice * oc.MaxQuantinty * oc.LotSize, _
                                      bs & "SPG" & s, bs & " Spread Seller's Profit-" & s)
        GainRow = lp.AddConstraint(GLP_FX, 0, 0, bs & "SPG" & s & " Gain Constr")
        If SpreadOrder.ORDER_BS = "S" Then
            lp.AddElement(VariableGain, GainRow, -1)
        Else
            lp.AddElement(VariableGain, GainRow, 1)
        End If
    End Sub

End Class

Public Class PRJ_SPREAD_CLASS
    Public SpreadOrder As Object = Nothing
    Public Leg1 As ORDERS_FFA_CLASS = Nothing
    Public Leg2 As ORDERS_FFA_CLASS = Nothing
    Public RouteTick As Double = 1

    Public Leg1RefPrice As Double = 0
    Public Leg2RefPrice As Double = 0
    Public SameLeg1RefPrice As Double = 0
    Public SameLeg2RefPrice As Double = 0
    Public v1 As Integer = 0
    Public v2 As Integer = 0
    Public c As Integer = 0
    Public BuyLow As Double = 0
    Public BuyHigh As Double = 0
    Public SellLow As Double = 0
    Public SellHigh As Double = 0
End Class

Public Class MARKET_ORDER_CLASS2
    Public Order As Object = Nothing
    Public PriceVariable As Integer = -1
    Public CapRow As Integer = -1

    Public Sub New(ByRef O As Object, _
                   ByVal Q As Integer, _
                   ByRef lp As LPProblem, _
                   ByVal ExchangeId As Integer)
        Order = O
        Dim OrderName As String = Order.ORDER_ID.ToString
        If Order.ORDER_BS = "B" Then
            CapRow = lp.AddConstraint(GLP_UP, 0, 0, " Total cap")
        Else
            CapRow = lp.AddConstraint(GLP_LO, 0, 0, " Total cap")
        End If
    End Sub

End Class

Public Class MARKET_SPREAD_CLASS2
    Public SpreadOrder As Object = Nothing
    Public Leg1Order As Object = Nothing
    Public Leg2Order As Object = Nothing
    Public QuantityRatio As Double = 0
    Public PriceRow As Integer = -1
    Public MinTick As Double = 0
    Public CalendarQModifier As Double = 0

    Public Sub New(ByRef SO As Object, _
                   ByRef lp As LPProblem, _
                   Optional ByVal TopPrice As Double = 200000)
        SpreadOrder = SO
        Dim OrderName As String = SO.ORDER_ID
        If SO.ORDER_TYPE = OrderTypes.RatioSpread Then
            MinTick = RatioSpreadPrecision

            If SO.ORDER_BS = "B" Then
                PriceRow = lp.AddConstraint(GLP_UP, 0, 0, OrderName & " B Ratio Spread Price constr")
            Else
                PriceRow = lp.AddConstraint(GLP_LO, 0, 0, OrderName & " S Ratio Spread Price constr")
            End If
        Else
            CalendarQModifier = SO.PRICE_INDICATED
            If SO.ORDER_BS = "B" Then
                PriceRow = lp.AddConstraint(GLP_UP, 0, 0, OrderName & " B Calendar Price constr")
            Else
                PriceRow = lp.AddConstraint(GLP_LO, 0, 0, OrderName & " S Calendar Price constr")
            End If
        End If
    End Sub

End Class


Public Class RoutePeriodPriceLimits
    Public RPStr As String
    'Public ROUTE_ID As Integer
    'Public MM1 As Integer
    'Public MM2 As Integer
    'Public YY1 As Integer
    'Public YY2 As Integer
    Public Low As Double = 1.0E+20
    Public High As Double = -1.0E+20
    Public BestBuy As Double = -1.0E+20
    Public BestSell As Double = 1.0E+20
    Public RatioLow As Double = 1.0E+20
    Public RatioHigh As Double = -1.0E+20
    Public CalendarLow As Double = 1.0E+20
    Public CalendarHigh As Double = -1.0E+20
    Public LowV As Integer = -1
    Public HighV As Integer = -1
    Public LastTrade As Double = -1.0E+20
    Public Tick As Double = 1
    Public Bounded As Boolean = False
    Public PriceList As New List(Of RoutePeriodPrice)
    Public SpreadLow As Double = -1.0E+20
    Public SpreadHigh As Double = 1.0E+20
    Public EqSpreadLow As Double = -1.0E+20
    Public EqSpreadHigh As Double = 1.0E+20
    Public SpreadLimit As Integer = -2
    Public Span As Double = 1.0E+20
    Public Volatility As Double = 0.02
    Public PriceTarget As Double = 1.0E+20
    Public LastIsClose As Boolean = False
    Public MandatoryLow As Boolean = False
    Public MandatoryHigh As Boolean = False
    Public LastTradeTime As DateTime = "1-1-2000"


    Public Sub New(ByRef lp As LPProblem, Optional ByVal a_RPStr As String = "")
        RPStr = a_RPStr
        LowV = lp.AddVariable(GLP_CV, 1, GLP_DB, -200000, 200000, "L" & RPStr)
        'Negative prices can occur after application of calendar spread constraints in near 0 prices
        'making problem insolvable
        HighV = lp.AddVariable(GLP_CV, -1, GLP_DB, -200000, 200000, "H" & RPStr)

        Dim CurrConstr As Integer
        CurrConstr = lp.AddConstraint(GLP_LO, 0, 0)
        lp.AddElement(LowV, CurrConstr, -1)
        lp.AddElement(HighV, CurrConstr, 1)
    End Sub

    Public Function ApplyPrice(ByRef lp As LPProblem, ByVal p As Double, ByVal PriceType As String, Optional ByVal a_LastTradeTime As DateTime = Nothing) As Boolean
        Dim CurrConstr As Integer
        Dim bExpanded As Boolean = False
        CurrConstr = lp.AddConstraint(GLP_UP, p, p)
        lp.AddElement(LowV, CurrConstr, 1)
        CurrConstr = lp.AddConstraint(GLP_LO, p, p)
        lp.AddElement(HighV, CurrConstr, 1)

        If Low > p Or High < p Then
            bExpanded = True
        End If
        Low = Min(Low, p)
        High = Max(High, p)

        Select Case PriceType
            Case "B"
                BestBuy = Max(p, BestBuy)

            Case "S"
                BestSell = Min(p, BestSell)
            Case "T"
                LastTrade = p
            Case "C"
                LastTrade = p
                LastIsClose = True
            Case Else

        End Select
        If Not IsNothing(a_LastTradeTime) Then
            If a_LastTradeTime > LastTradeTime Then LastTradeTime = a_LastTradeTime
        End If
        Bounded = True
        Return bExpanded
    End Function

    Public Function ApplySwapPrice(ByRef lp As LPProblem, _
                              ByVal p As Double, _
                              ByVal RefRPPStr As String, _
                              Optional ByVal PriceType As String = "", _
                              Optional ByVal a_LastTradeTime As DateTime = Nothing) As Boolean

        For Each crpp As RoutePeriodPrice In PriceList
            If crpp.Price = p Then
                If InStr(crpp.PathKeyStr, RefRPPStr) <= 0 Then
                    crpp.PathKeyStr &= RefRPPStr
                    Return True
                Else
                    Return False
                End If
            End If
        Next
        ApplyPrice(lp, p, PriceType, a_LastTradeTime)
        Dim rpp As New RoutePeriodPrice
        rpp.Price = p
        rpp.PathKeyStr = RefRPPStr
        rpp.AddKey(RPStr)

        PriceList.Add(rpp)
        Return True
    End Function

    Public Function ApplyPriceFromSpread(ByRef lp As LPProblem, _
                                         ByVal LowP As Double, _
                                         ByVal HighP As Double, _
                                         ByVal a_RPStr As String, _
                                         ByVal SRPStr As String, _
                                         ByVal RefRPPStr As String, _
                                         Optional ByVal a_LastTradeTime As DateTime = Nothing) As Boolean
        ApplyPriceFromSpread = False

        ApplyPriceFromSpread = ApplyPriceFromSpread Or ApplySwapPrice(lp, LowP, RefRPPStr, a_LastTradeTime)
        ApplyPriceFromSpread = ApplyPriceFromSpread Or ApplySwapPrice(lp, HighP, RefRPPStr, a_LastTradeTime)
        Debug.Print("Apply price " & LowP.ToString() & " in " & RPStr & " from " & a_RPStr & " from spread " & SRPStr)
        Debug.Print("Apply price " & HighP.ToString() & " in " & RPStr & " from " & a_RPStr & " from spread " & SRPStr)
    End Function


    Public Function ExistsSRPStr(ByVal SRPStr As String) As Boolean
        For Each pr In PriceList
            If SRPStr = pr.SRPStr Then Return True
        Next
        Return False
    End Function

    Public Sub CalcSpreadLimits(ByRef RefCol As Collection)
        Dim Mid As Double
        If Math.Abs(LastTrade) > 1.0E+19 Then
            Dim RefC As REF_CLASS = GetViewClass(RefCol, RPStr)
            If Not IsNothing(RefCol) And Math.Abs(RefC.LastTrade) < 1.0E+19 Then
                LastTrade = RefC.LastTrade
            Else
                Span = 1.0E+20
                SpreadLimit = -1
                Exit Sub
            End If
        End If
        PriceTarget = LastTrade

        If Math.Abs(BestBuy) > 1.0E+19 And Math.Abs(BestSell) > 1.0E+19 Then
            Span = Volatility
            SpreadLow = LastTrade - LastTrade * Volatility
            SpreadHigh = LastTrade + LastTrade * Volatility
            EqSpreadLow = SpreadLow
            EqSpreadHigh = SpreadHigh
            If LastIsClose Then
                SpreadLimit = 0
            Else
                SpreadLimit = 1
            End If
        ElseIf Math.Abs(BestBuy) > 1.0E+19 Then
            Span = Math.Abs(BestSell - LastTrade) / (BestSell + LastTrade)
            If BestSell <= LastTrade Then
                SpreadLow = BestSell - Volatility * BestSell
                SpreadHigh = BestSell
                EqSpreadLow = SpreadLow
                EqSpreadHigh = BestSell + Volatility * BestSell
                PriceTarget = BestSell
                MandatoryHigh = True
            Else
                SpreadLow = LastTrade - Volatility * LastTrade
                SpreadHigh = LastTrade + Volatility * LastTrade
                EqSpreadLow = SpreadLow
                EqSpreadHigh = SpreadHigh
                If (SpreadHigh > BestSell) Then
                    SpreadHigh = BestSell
                    PriceTarget = BestSell
                    If (SpreadLow > SpreadHigh) Then SpreadLow = SpreadHigh
                    MandatoryHigh = True
                End If
            End If
            SpreadLimit = 2
        ElseIf Math.Abs(BestSell) > 1.0E+19 Then
            Span = Math.Abs(BestBuy - LastTrade) / (BestBuy + LastTrade)
            If BestBuy >= LastTrade Then
                SpreadLow = BestBuy
                SpreadHigh = BestBuy + BestBuy * Volatility
                EqSpreadLow = BestBuy - BestBuy * Volatility
                EqSpreadHigh = SpreadHigh
                PriceTarget = BestBuy
                MandatoryLow = True
            Else
                SpreadLow = LastTrade - Volatility * LastTrade
                SpreadHigh = LastTrade + Volatility * LastTrade
                EqSpreadLow = SpreadLow
                EqSpreadHigh = SpreadHigh
                If (SpreadLow < BestBuy) Then
                    SpreadLow = BestBuy
                    If (SpreadHigh < SpreadLow) Then SpreadHigh = SpreadLow
                    PriceTarget = BestBuy
                    MandatoryLow = True
                End If
            End If
            SpreadLimit = 2
        Else
            Span = Math.Abs(BestBuy - BestSell) / (BestBuy + BestSell)
            If BestBuy > BestSell Then
                Swap(BestBuy, BestSell)
            End If
            If BestBuy >= LastTrade Then
                MandatoryLow = True
                SpreadLow = BestBuy
                SpreadHigh = BestBuy + BestBuy * Volatility
                EqSpreadLow = BestBuy - BestBuy * Volatility
                EqSpreadHigh = SpreadHigh
                PriceTarget = BestBuy
                If SpreadHigh > BestSell Then
                    SpreadHigh = BestSell
                    PriceTarget = 0.5 * (BestBuy + BestSell)
                    MandatoryHigh = True
                End If
            ElseIf BestSell <= LastTrade Then
                MandatoryHigh = True
                SpreadLow = BestSell - Volatility * BestSell
                SpreadHigh = BestSell
                EqSpreadLow = SpreadLow
                EqSpreadHigh = BestSell + Volatility * BestSell
                PriceTarget = BestSell
                If SpreadLow < BestBuy Then
                    SpreadLow = BestBuy
                    PriceTarget = 0.5 * (BestBuy + BestSell)
                    MandatoryLow = True
                End If
            Else
                SpreadLow = LastTrade - Volatility * LastTrade
                SpreadHigh = LastTrade + Volatility * LastTrade
                EqSpreadLow = SpreadLow
                EqSpreadHigh = SpreadHigh
                If SpreadLow < BestBuy Then
                    SpreadLow = BestBuy
                    PriceTarget = 0.5 * (BestBuy + BestSell)
                    MandatoryLow = True
                End If
                If SpreadHigh > BestSell Then
                    SpreadHigh = BestSell
                    PriceTarget = 0.5 * (BestBuy + BestSell)
                    MandatoryHigh = True
                End If
            End If
            SpreadLimit = 3
            End If
    End Sub
End Class

Public Class RoutePeriodPrice
    Public RPStr As String = ""
    Public SRPStr As String = ""
    Public Price As Double = 0
    Public OrderType As Integer = 0
    Public PathKeyStr As String = ""

    Public Sub AddKey(ByRef a_RPStr As String)
        If InStr(PathKeyStr, a_RPStr) > 0 Then Exit Sub
        PathKeyStr &= a_RPStr
    End Sub

End Class

Public Class RoutePeriodSpreadPriceLimits
    Public SRPStr As String = ""
    Public RPPL1 As RoutePeriodPriceLimits = Nothing
    Public RPPL2 As RoutePeriodPriceLimits = Nothing
    Public Low As Double = 1.0E+20
    Public High As Double = -1.0E+20
    Public OrderType As Integer = 0
    Public SpreadOrder As ORDERS_FFA_CLASS = Nothing
End Class


