Public Class ORDERS_FFA_OPTIONS_CLASS
    Public ORDER_ID As Integer
    Public DESK_TRADER_ID As Integer
    Public ORDER_RECEIVED As DateTime
    Public ORDER_STATUS As String
    Public PREVIOUS_ORDER_ID As Integer
    Public ORDER_BS As String
    Public ROUTE_ID As Integer
    Public MM1 As Short
    Public YY1 As Short
    Public MM2 As Short
    Public YY2 As Boolean
    Public SHORTDES As String
    Public OPTION_TYPE As Short
    Public STRIKE_PRICE1 As Double
    Public STRIKE_PRICE2 As String
    Public SPOT_PRICE_REF As Double
    Public PRICE_INDICATED As Double
    Public PRICE_VOLATILITY As Double
    Public PRICE_STATUS As String
    Public ORDER_DAYS As String
    Public FLEXIBLE_ON_DAYS As Boolean
    Public PRICE_TRY_BETTER As Boolean
    Public PARTIAL_FILL As Boolean
    Public ORDER_GOOD_TILL As String
    Public ORDER_TIME_LIMIT As Integer
    Public LCH As Boolean
    Public SGX As Boolean
    Public NOS As Boolean
    Public OTC As Boolean
    Public SHOW_MY_NAME As Short
    Public SHOW_MY_TRADES As Short
    Public REQUIRE_DELTA As Short
    Public upsize_ts As TimeSpan

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        ORDER_ID = q.ORDER_ID
        DESK_TRADER_ID = q.DESK_TRADER_ID
        ORDER_RECEIVED = q.ORDER_RECEIVED
        ORDER_STATUS = q.ORDER_STATUS
        PREVIOUS_ORDER_ID = q.PREVIOUS_ORDER_ID
        ORDER_BS = q.ORDER_BS
        ROUTE_ID = q.ROUTE_ID
        MM1 = q.MM1
        YY1 = q.YY1
        MM2 = q.MM2
        YY2 = q.YY2
        SHORTDES = q.SHORTDES
        OPTION_TYPE = q.OPTION_TYPE
        STRIKE_PRICE1 = q.STRIKE_PRICE1
        STRIKE_PRICE2 = q.STRIKE_PRICE2
        SPOT_PRICE_REF = q.SPOT_PRICE_REF
        PRICE_INDICATED = q.PRICE_INDICATED
        PRICE_VOLATILITY = q.PRICE_VOLATILITY
        PRICE_STATUS = q.PRICE_STATUS
        ORDER_DAYS = q.ORDER_DAYS
        FLEXIBLE_ON_DAYS = q.FLEXIBLE_ON_DAYS
        PRICE_TRY_BETTER = q.PRICE_TRY_BETTER
        PARTIAL_FILL = q.PARTIAL_FILL
        ORDER_GOOD_TILL = q.ORDER_GOOD_TILL
        ORDER_TIME_LIMIT = q.ORDER_TIME_LIMIT
        LCH = q.LCH
        SGX = q.SGX
        NOS = q.NOS
        OTC = q.OTC
        SHOW_MY_NAME = q.SHOW_MY_NAME
        SHOW_MY_TRADES = q.SHOW_MY_TRADES
        REQUIRE_DELTA = q.REQUIRE_DELTA
        upsize_ts = q.upsize_ts
        GetFromObject = ArtBErrors.Success
    End Function

    Public Function SetToObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            SetToObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        q.ORDER_ID = ORDER_ID
        q.DESK_TRADER_ID = DESK_TRADER_ID
        q.ORDER_RECEIVED = ORDER_RECEIVED
        q.ORDER_STATUS = ORDER_STATUS
        q.PREVIOUS_ORDER_ID = PREVIOUS_ORDER_ID
        q.ORDER_BS = ORDER_BS
        q.ROUTE_ID = ROUTE_ID
        q.MM1 = MM1
        q.YY1 = YY1
        q.MM2 = MM2
        q.YY2 = YY2
        q.SHORTDES = SHORTDES
        q.OPTION_TYPE = OPTION_TYPE
        q.STRIKE_PRICE1 = STRIKE_PRICE1
        q.STRIKE_PRICE2 = STRIKE_PRICE2
        q.SPOT_PRICE_REF = SPOT_PRICE_REF
        q.PRICE_INDICATED = PRICE_INDICATED
        q.PRICE_VOLATILITY = PRICE_VOLATILITY
        q.PRICE_STATUS = PRICE_STATUS
        q.ORDER_DAYS = ORDER_DAYS
        q.FLEXIBLE_ON_DAYS = FLEXIBLE_ON_DAYS
        q.PRICE_TRY_BETTER = PRICE_TRY_BETTER
        q.PARTIAL_FILL = PARTIAL_FILL
        q.ORDER_GOOD_TILL = ORDER_GOOD_TILL
        q.ORDER_TIME_LIMIT = ORDER_TIME_LIMIT
        q.LCH = LCH
        q.SGX = SGX
        q.NOS = NOS
        q.OTC = OTC
        q.SHOW_MY_NAME = SHOW_MY_NAME
        q.SHOW_MY_TRADES = SHOW_MY_TRADES
        q.REQUIRE_DELTA = REQUIRE_DELTA
        q.upsize_ts = upsize_ts
    End Function

    Public Function GetData() As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim gdb As New DB_ARTB_NETDataContext(ArtBConnectionStr)
        Dim l = From q In gdb.ORDERS_FFA_OPTIONs _
        Where q.ORDER_ID = ORDER_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByVal a_ORDER_ID As Integer) As Integer
        ORDER_ID = a_ORDER_ID
        GetFromID = GetData()
    End Function

    Public Function Insert(Optional ByVal submit As Boolean = False) As Integer
        Dim gdb As New DB_ARTB_NETDataContext(ArtBConnectionStr)
        Dim q As New ORDERS_FFA_OPTION
        SetToObject(q)

        gdb.ORDERS_FFA_OPTIONs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
            Catch
                Insert = ArtBErrors.InsertFailed
                Exit Function
            End Try
        End If
        Insert = ArtBErrors.Success
    End Function

    Public Function Update(Optional ByVal submit As Boolean = False) As Integer
        Dim gdb As New DB_ARTB_NETDataContext(ArtBConnectionStr)
        Update = ArtBErrors.Success

        Dim l = From q In gdb.ORDERS_FFA_OPTIONs _
          Where q.ORDER_ID = ORDER_ID _
          Select q

        For Each q As ORDERS_FFA_OPTION In l
            Update = SetToObject(q)
            If Update <> ArtBErrors.Success Then
                Exit Function
            End If
        Next

        If submit = True Then
            Try
                gdb.SubmitChanges()
            Catch e As Exception
                Update = ArtBErrors.UpdateFailed
                Exit Function
            End Try
        End If
    End Function

    Public Function Delete(Optional ByVal submit As Boolean = False) As Integer
        Delete = ArtBErrors.Success
        Dim gdb As New DB_ARTB_NETDataContext(ArtBConnectionStr)

        Dim l = From q In gdb.ORDERS_FFA_OPTIONs _
         Where q.ORDER_ID = ORDER_ID _
          Select q

        For Each q As ORDERS_FFA_OPTION In l
            gdb.ORDERS_FFA_OPTIONs.DeleteOnSubmit(q)
        Next

        If submit = True Then
            Try
                gdb.SubmitChanges()
            Catch e As Exception
                Delete = ArtBErrors.DeleteFailed
                Exit Function
            End Try
        End If
    End Function
End Class
