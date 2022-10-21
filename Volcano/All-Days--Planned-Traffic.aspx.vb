Imports System.Data
Partial Class All_Days__Planned_Traffic
    Inherits System.Web.UI.Page
    ' To keep track of the previous row Group Identifier
    Private strPreviousRowID As String = String.Empty
    ' To keep track the Index of Group Total
    Private intSubTotalIndex As Integer = 1

    ' To temporarily store Sub Total
    Private dblSubTotalQuantity As Double = 0
    Private dblSubTotalDiscount As Double = 0
    Private dblSubTotalAmount As Double = 0
    Private dblSubTotalAmountbwf As Double = 0
    Private dblSubTotalAmountRWF As Double = 0

    ' To temporarily store Grand Total
    Private dblGrandTotalQuantity As Double = 0
    Private dblGrandTotalDiscount As Double = 0
    Private dblGrandTotalAmount As Double = 0
    Private dblGrandTotalAmountRWF As Double = 0
    Private dblGrandTotalAmountbwf As Double = 0

    Private clsDataLayer As New Datalayer
    Protected Sub gridViewBuses_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowCreated

        Dim hideTickets As String = " AND Marked=0 "
        Dim CanHideTickets As String = "0"
        If (Session IsNot Nothing) Then
            Dim item As Object = Session("CanHideTickets")
            If (item IsNot Nothing) Then
                If (item.ToString() = "True") Then
                    hideTickets = "" 'can hide tickets
                End If
            End If
        End If

        Try
            Dim IsSubTotalRowNeedToAdd As Boolean = False
            Dim IsGrandTotalRowNeedtoAdd As Boolean = False

            If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "Route") IsNot Nothing) Then
                If strPreviousRowID <> DataBinder.Eval(e.Row.DataItem, "Route").ToString() Then
                    IsSubTotalRowNeedToAdd = True
                End If
            End If

            If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "Route") Is Nothing) Then
                IsSubTotalRowNeedToAdd = True
                IsGrandTotalRowNeedtoAdd = True
                intSubTotalIndex = 0
            End If

            '#Region "Inserting first Row and populating fist Group Header details"
            If (strPreviousRowID = String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "Route") IsNot Nothing) Then
                Dim grdViewProducts As GridView = DirectCast(sender, GridView)

                Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                Dim cell As New TableCell()
                cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Route Sales Opportunities :  </b>" & "<div style='float:left;'>" & DataBinder.Eval(e.Row.DataItem, "Route").ToString() & "</br></div>" & "</div>" & "<div style='width:50%;float:left;'></div>" & "</div>"
                cell.ColumnSpan = 10
                cell.CssClass = "GroupDetailsStyle"
                row.Cells.Add(cell)

                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                intSubTotalIndex += 1

                '#Region "Add Group Header"
                ' Creating a Row
                row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)



                cell = New TableCell()
                cell.Text = "Bus Name"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Date"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Hour"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Vehicle"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Driver"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Max Capacity"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Tickets"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Messages"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)


                cell = New TableCell()
                cell.Text = "Status"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "RWF"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "FIB"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                'Adding the Row at the RowIndex position in the Grid
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                '#End Region
                intSubTotalIndex += 1
            End If
            '#End Region

            If IsSubTotalRowNeedToAdd Then
                '#Region "Adding Sub Total Row"
                Dim grdViewProducts As GridView = DirectCast(sender, GridView)

                ' Creating a Row
                Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                'Adding Total Cell 
                Dim cell As New TableCell()
                cell.Text = "Total Revenue For This Bus: "
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.ColumnSpan = 10
                cell.CssClass = "SubTotalRowStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                'cell.Text = String.Format("{0:n2}", dblSubTotalAmount)
                cell.Text = String.Format("RWF: {0:n2}, FIB: {1:n2}", dblSubTotalAmountRWF, dblSubTotalAmountbwf)
                cell.HorizontalAlign = HorizontalAlign.Center
                cell.CssClass = "SubTotalRowStyle"
                row.Cells.Add(cell)

                'Adding the Row at the RowIndex position in the Grid
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                intSubTotalIndex += 1
                '#End Region

                '#Region "Adding Next Group Header Details"
                If DataBinder.Eval(e.Row.DataItem, "Route") IsNot Nothing Then
                    '#Region "Adding Empty Row after each Group Total"
                    row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                    cell = New TableCell()
                    cell.Text = String.Empty
                    cell.Height = Unit.Parse("10px")
                    cell.ColumnSpan = 12
                    row.Cells.Add(cell)
                    row.BorderStyle = BorderStyle.None

                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                    intSubTotalIndex += 1
                    '#End Region

                    '#Region "Adding Next Group Header Details"
                    row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                    cell = New TableCell()
                    cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Route Sales Opportunities :  </b>" & "<div style='float:left;'>" & DataBinder.Eval(e.Row.DataItem, "Route").ToString() & "</br></div>" & "</div>" & "<div style='width:50%;float:left;'></div>" & "</div>"
                    cell.ColumnSpan = 12
                    cell.CssClass = "GroupDetailsStyle"
                    row.Cells.Add(cell)

                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                    intSubTotalIndex += 1
                    '#End Region

                    '#Region "Add Group Header"
                    ' Creating a Row
                    row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)


                    cell = New TableCell()
                    cell.Text = "Bus Name"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Date"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Hour"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Vehicle"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Driver"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Max Capacity"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Tickets"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Messages"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)


                    cell = New TableCell()
                    cell.Text = "Status"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "RWF"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "FIB"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)


                    'Adding the Row at the RowIndex position in the Grid
                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                    '#End Region
                    intSubTotalIndex += 1
                End If
                '#End Region

                '#Region "Reseting the Sub Total Variables"
                dblSubTotalQuantity = 0
                dblSubTotalDiscount = 0
                '#End Region
                dblSubTotalAmountRWF = 0
                dblSubTotalAmountbwf = 0
                dblSubTotalAmount = 0
            End If
            If IsGrandTotalRowNeedtoAdd Then
                Dim grdViewProducts As GridView = DirectCast(sender, GridView)

                '#Region "Adding Empty Row before Grand Total"
                Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                Dim cell As New TableCell()
                cell.Text = String.Empty
                cell.Height = Unit.Parse("10px")
                cell.ColumnSpan = 12
                row.Cells.Add(cell)
                row.BorderStyle = BorderStyle.None

                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex, row)
                intSubTotalIndex += 1
                '#End Region

                '#Region "Grand Total Row"
                ' Creating a Row
                row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                'Adding Total Cell 
                cell = New TableCell()
                cell.Text = "Grand Total"
                cell.HorizontalAlign = HorizontalAlign.Left
                cell.ColumnSpan = 10
                cell.CssClass = "GrandTotalRowStyle"
                row.Cells.Add(cell)



                'Adding Quantity Column
                'cell = New TableCell()
                'cell.Text = dblGrandTotalQuantity.ToString()
                'cell.HorizontalAlign = HorizontalAlign.Right
                'cell.CssClass = "GrandTotalRowStyle"
                'row.Cells.Add(cell)

                'Adding Amount Column
                cell = New TableCell()
                'cell.Text = "RWF " + String.Format("{0:n0}", dblGrandTotalAmount) + ".00"
                cell.Text = String.Format("RWF: {0:n0}, FIB: {1:n0}", dblSubTotalAmountRWF, dblGrandTotalAmountbwf)
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.CssClass = "GrandTotalRowStyle"
                row.Cells.Add(cell)

                'Adding the Row at the RowIndex position in the Grid
                '#End Region
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex, row)

                ' Add the Total Sales Of the Day

                '#Region "Adding Empty Row before Grand Total"
                row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                cell = New TableCell()
                cell.Text = String.Empty
                cell.Height = Unit.Parse("10px")
                cell.ColumnSpan = 12
                row.Cells.Add(cell)
                row.BorderStyle = BorderStyle.None

                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex, row)
                intSubTotalIndex += 1
                '#End Region

                '#Region "Grand Total Row"
                ' Creating a Row
                row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                'Adding Total Cell 
                cell = New TableCell()
                cell.Text = "Grand Total Sales Of " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString + ": "
                cell.HorizontalAlign = HorizontalAlign.Left
                cell.ColumnSpan = 10
                cell.CssClass = "GrandTotalRowStyle"
                row.Cells.Add(cell)


                'Adding Amount Column
                Dim totalDaySales As Double = 0
                Dim totalDaysRWF As Double = 0
                Dim totalDaysFIB As Double = 0

                If User.IsInRole("restricted") Then
                    'Restricted WoooH :-(
                Else
                    Dim dtDaySales As DataTable = clsDataLayer.ReturnDataTable("SELECT SUm(TOTAL),SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())" + hideTickets)
                    'totalDaySales = clsDataLayer.ReturnSingleNumeric("SELECT SUm(TOTAL),SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())")
                    If (dtDaySales.Rows.Count > 0) Then
                        totalDaysRWF = Convert.ToDouble(dtDaySales.Rows(0)(1).ToString)
                        totalDaysRWF = Convert.ToDouble(dtDaySales.Rows(0)(1).ToString)
                    End If

                End If

                If Request.QueryString("requestDate") <> String.Empty Then
                    'The Request Date was set
                    If User.IsInRole("restricted") Then
                        ':-(
                        totalDaySales = 0
                    Else
                        Dim reqDate As String = Request.QueryString("requestDate").ToString
                        Dim qString As String = String.Format("SELECT SUm(TOTAL),SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = " &
                                                                                       "DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103))" &
                                                                                       " AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", reqDate) + hideTickets
                        'totalDaySales = clsDataLayer.ReturnSingleNumeric()
                        Dim dtDaySales2 As DataTable = clsDataLayer.ReturnDataTable(qString)
                        If (dtDaySales2.Rows.Count > 0) Then
                            totalDaysRWF = Convert.ToDouble(dtDaySales2.Rows(0)(1).ToString)
                            totalDaysRWF = Convert.ToDouble(dtDaySales2.Rows(0)(1).ToString)
                        End If

                    End If

                End If
                cell = New TableCell()
                'cell.Text = "RWF " + String.Format("{0:n0}", totalDaySales) + ".00"
                cell.Text = String.Format("RWF: {0:n0}, FIB: {1:n0}", totalDaysRWF, totalDaysFIB)
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.CssClass = "GrandTotalRowStyle"
                row.Cells.Add(cell)

                'Adding the Row at the RowIndex position in the Grid
                '#End Region
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex, row)
            End If
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

    End Sub

    Protected Sub gridViewBuses_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "Route").ToString()

                Dim tmp As String = DataBinder.Eval(e.Row.DataItem, "Budget").ToString()
                Dim dblQuantity As Double, dblAmount As Double
                Try
                    dblQuantity = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Budget").ToString())

                    'dblAmount = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Budget").ToString())

                    If (Not User.IsInRole("restricted")) Then
                        dblAmount = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "TotalRWF").ToString())
                        dblSubTotalAmountRWF += dblAmount
                        dblGrandTotalAmountRWF += dblAmount

                        dblAmount = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "TotalFIB").ToString())
                        dblSubTotalAmountbwf += dblAmount
                        dblGrandTotalAmountbwf += dblAmount
                    End If


                Catch ex As Exception

                End Try


                ' Cumulating Sub Total

                dblSubTotalQuantity += dblQuantity

                ' dblSubTotalAmount += dblAmount

                ' Cumulating Grand Total
                dblGrandTotalQuantity += dblQuantity
                ' dblGrandTotalAmount += dblAmount
            End If
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

    End Sub

    Protected Sub gridViewBuses_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridViewBuses.Load
        Try
            Dim reqDate As String = ""
            Dim pos As String = ""
            Try
                reqDate = Request.QueryString("requestDate")
            Catch ex As Exception
                'The Req Date was Not Sent
                reqDate = ""
            End Try
            'Define the Queries
            Dim query As String = ""

            Dim CanHide As Boolean = False

            If (Session IsNot Nothing) Then
                Dim item As Object = Session("CanHideTickets")
                If (item IsNot Nothing) Then
                    If (item.ToString() = "True") Then
                        CanHide = True
                    End If
                End If
            End If

            'Define the Query for Selecting Bus Name and Bus Details
            'Check if the POS is empty and redirect to POS Ticketing Page


            If String.IsNullOrEmpty(reqDate) Then
                'No Request Date so take for today

                headerDate.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString
                If CanHide Then
                    query = " SELECT IDSALE,FLD164 AS 'Route', NAME,TARGET_DATE, FLD163 AS 'Hour', FLD103 AS 'Capacity', (SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())  AND IDSALE=s.IDSALE) as 'Booked', STATUS,BUDGET,'' AS 'Vehicle',''AS 'Mail',SUBSTRING(MEMO,0,20)+'..' AS 'Memo','' AS 'Driver', (SELECT SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE()) AND IDSALE=s.IDSALE) as 'TotalRWF', (SELECT SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())  AND IDSALE=s.IDSALE) as 'TotalFIB' FROM su.SALES s WHERE DAY(TARGET_DATE) = DAY(GETDATE()) AND MONTH(TARGET_DATE) = MONTH(GETDATE()) AND YEAR(TARGET_DATE) = YEAR(GETDATE())  ORDER BY FLD164,FLD163"
                Else
                    query = " SELECT IDSALE,FLD164 AS 'Route', NAME,TARGET_DATE, FLD163 AS 'Hour', FLD103 AS 'Capacity', (SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())  AND IDSALE=s.IDSALE AND Marked=0) as 'Booked', STATUS,BUDGET,'' AS 'Vehicle',''AS 'Mail',SUBSTRING(MEMO,0,20)+'..' AS 'Memo','' AS 'Driver', (SELECT SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE()) AND IDSALE=s.IDSALE AND Marked=0) as 'TotalRWF', (SELECT SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())  AND IDSALE=s.IDSALE AND Marked=0) as 'TotalFIB' FROM su.SALES s WHERE DAY(TARGET_DATE) = DAY(GETDATE()) AND MONTH(TARGET_DATE) = MONTH(GETDATE()) AND YEAR(TARGET_DATE) = YEAR(GETDATE())  ORDER BY FLD164,FLD163"
                End If
                'Special Role With No Numbers
                If User.IsInRole("restricted") Then
                    query = "SELECT IDSALE,FLD164 AS 'Route', NAME,TARGET_DATE, FLD163 AS 'Hour', FLD103 AS 'Capacity',FLD166 AS 'Booked'," &
                                    "STATUS,0 As 'BUDGET','' AS 'Vehicle',''AS 'Mail',SUBSTRING(MEMO,0,20)+'..' AS 'Memo','' AS 'Driver',0 AS 'TotalRWF',0 as 'TotalFIB' " &
                                    "FROM su.SALES WHERE DAY(TARGET_DATE) = DAY(GETDATE()) AND MONTH(TARGET_DATE) = MONTH(GETDATE()) AND YEAR(TARGET_DATE) = YEAR(GETDATE()) ORDER BY FLD164,FLD163"
                End If

            Else
                'Date Defined
                Try
                    'Dim exactDate As DateTime = DateTime.Parse(reqDate, "DD/MM/YYYY")
                    Dim exactDate = reqDate
                    headerDate.Text = ": " + reqDate

                    'No Error so lets continue
                    If CanHide Then
                        query = String.Format(" SELECT IDSALE,FLD164 AS 'Route', NAME,TARGET_DATE, FLD163 AS 'Hour', FLD103 AS 'Capacity', (SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103)) AND IDSALE=s.IDSALE ) AS Booked, STATUS,BUDGET,'' AS 'Vehicle',''AS 'Mail',SUBSTRING(MEMO,0,20)+'..' AS 'Memo','' AS 'Driver', (SELECT SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103)) AND IDSALE=s.IDSALE ) as 'TotalRWF', (SELECT SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103))  AND IDSALE=s.IDSALE ) as 'TotalFIB' FROM su.SALES s WHERE DAY(TARGET_DATE) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(TARGET_DATE) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(TARGET_DATE) = YEAR(CONVERT(DATETIME,'{0}',103))  ORDER BY FLD164,FLD163", exactDate)
                    Else
                        query = String.Format(" SELECT IDSALE,FLD164 AS 'Route', NAME,TARGET_DATE, FLD163 AS 'Hour', FLD103 AS 'Capacity', (SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103))AND IDSALE=s.IDSALE AND Marked=0) AS Booked , STATUS,BUDGET,'' AS 'Vehicle',''AS 'Mail',SUBSTRING(MEMO,0,20)+'..' AS 'Memo','' AS 'Driver', (SELECT SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103)) AND IDSALE=s.IDSALE AND Marked=0) as 'TotalRWF', (SELECT SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103))  AND IDSALE=s.IDSALE AND Marked=0) as 'TotalFIB' FROM su.SALES s WHERE DAY(TARGET_DATE) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(TARGET_DATE) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(TARGET_DATE) = YEAR(CONVERT(DATETIME,'{0}',103))  ORDER BY FLD164,FLD163", exactDate)
                    End If

                    'Special Role With No Numbers
                    If User.IsInRole("restricted") Then
                        query = String.Format("SELECT IDSALE,FLD164 AS 'Route',SUBSTRING(MEMO,0,20)+'..' AS 'Memo', NAME,TARGET_DATE, FLD163 AS 'Hour', FLD103 AS 'Capacity',FLD166 AS 'Booked'," &
                                    "STATUS,0 as 'BUDGET','' AS 'Vehicle',''AS 'Mail','' AS 'Driver',0 AS 'TotalRWF',0 as 'TotalFIB' " &
                                    "FROM su.SALES WHERE DAY(TARGET_DATE) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(TARGET_DATE) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(TARGET_DATE) =YEAR(CONVERT(DATETIME,'{0}',103)) ORDER BY FLD164,FLD163", exactDate)
                    End If
                Catch
                End Try
            End If

            'Define the Query for Selecting Bus Name and Bus Details

            Dim dt As DataTable = clsDataLayer.ReturnDataTable(query)
            gridViewBuses.DataSource = dt
            gridViewBuses.DataBind()

        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try

        Page.Title = "All Days Planned Traffic"
    End Sub
End Class
