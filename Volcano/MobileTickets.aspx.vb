Imports System.Data
Partial Class MobileTickets
    Inherits System.Web.UI.Page
    ' To keep track of the previous row Group Identifier
    Private strPreviousRowID As String = String.Empty
    ' To keep track the Index of Group Total
    Private intSubTotalIndex As Integer = 1

    'Count Promotions And Bookings For the Bus
    Dim promotions As Integer = 0
    Dim bookigs As Integer = 0
    Dim posNo As String = ""

    ' To temporarily store Sub Total
    Private dblSubTotalQuantity As Double = 0
    Private dblSubTotalDiscount As Double = 0
    Private dblSubTotalAmount As Double = 0

    ' To temporarily store Grand Total
    Private dblGrandTotalQuantity As Double = 0
    Private dblGrandTotalDiscount As Double = 0
    Private dblGrandTotalAmount As Double = 0
    Dim idsale As String = ""
    Private clsDataLayer As New Datalayer
    Protected Sub gridViewBuses_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowCreated
        Try
            Dim IsSubTotalRowNeedToAdd As Boolean = False
            Dim IsGrandTotalRowNeedtoAdd As Boolean = False

            Try
                idsale = DataBinder.Eval(e.Row.DataItem, "IDSALE")
                Dim x As Char
            Catch ex As Exception
                clsDataLayer.LogException(ex)
            End Try

            If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "IDSALE") IsNot Nothing) Then
                If strPreviousRowID <> DataBinder.Eval(e.Row.DataItem, "IDSALE").ToString() Then
                    IsSubTotalRowNeedToAdd = True
                End If
            End If

            If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "IDSALE") Is Nothing) Then
                IsSubTotalRowNeedToAdd = True
                IsGrandTotalRowNeedtoAdd = True
                intSubTotalIndex = 0
            End If

            '#Region "Inserting first Row and populating fist Group Header details"
            If (strPreviousRowID = String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "IDSALE") IsNot Nothing) Then
                Dim grdViewProducts As GridView = DirectCast(sender, GridView)

                Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                'Calculate the Header Statistics

                'Dim queryPromos As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD123='{1}' AND FLD213='PROMO'", idsale, posNo)
                'Dim queryBookings As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND AND FLD123='{1}' AND FLD213 NOT IN('PROMO','-')", idsale, posNo)

                'promotions = clsDataLayer.ReturnSingleNumeric(queryPromos)
                'bookigs = clsDataLayer.ReturnSingleNumeric(queryBookings)

                Dim cell As New TableCell()
                'cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Details :  </b>" & "<div style='float:left;'>" & DataBinder.Eval(e.Row.DataItem, "BusName").ToString() & "</br></div>" & "</div>" & "<div style='width:50%;float:left;'>" & "<b>Capacity : </b>" & DataBinder.Eval(e.Row.DataItem, "Capacity").ToString() & "</div>" & "</div>"
                cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Route Details :  </b>" &
                    "<div style='float:left;'><a href='Bus-Details.aspx?idsale=" + idsale + "'>" & DataBinder.Eval(e.Row.DataItem, "BusName").ToString() &
                    "</a></div></div>" & "<div style='width:50%;float:left;'><div style='width:30%;float:left'><b>Capacity: </b> " & DataBinder.Eval(e.Row.DataItem, "Capacity").ToString() & "</div>" &
                "<div style='width:65%;float:left;'><b>Total Promotions/POS: </b> " & promotions & "<div style='float:right'><b>Total Bookings/POS: </b> " & bookigs & "</div></div></div>"
                cell.ColumnSpan = 11
                cell.CssClass = "GroupDetailsStyle"
                row.Cells.Add(cell)

                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                intSubTotalIndex += 1

                '#Region "Add Group Header"
                ' Creating a Row
                row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)



                cell = New TableCell()
                cell.Text = "City IN"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "City OUT"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "TicketID"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Printed"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Price"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Discount"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Subscription No"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Created ON"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Printed On"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Creating POS"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Printed By"
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
                cell.Text = "Total For This Bus: "
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.ColumnSpan = 10
                cell.CssClass = "SubTotalRowStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = String.Format("{0:n2}", dblSubTotalAmount)
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.CssClass = "SubTotalRowStyle"
                row.Cells.Add(cell)

                'Adding the Row at the RowIndex position in the Grid
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                intSubTotalIndex += 1
                '#End Region

                '#Region "Adding Next Group Header Details"
                If DataBinder.Eval(e.Row.DataItem, "IDSALE") IsNot Nothing Then
                    '#Region "Adding Empty Row after each Group Total"
                    row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                    cell = New TableCell()
                    cell.Text = String.Empty
                    cell.Height = Unit.Parse("10px")
                    cell.ColumnSpan = 10
                    row.Cells.Add(cell)
                    row.BorderStyle = BorderStyle.None

                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                    intSubTotalIndex += 1
                    '#End Region

                    '#Region "Adding Next Group Header Details"
                    row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                    'Calculate the Header Statistics

                    'Dim queryPromos As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD123='{1}' AND FLD213='PROMO'", idsale, posNo)
                    'Dim queryBookings As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND AND FLD123='{1}' AND FLD213 NOT IN('PROMO','-')", idsale, posNo)

                    'promotions = clsDataLayer.ReturnSingleNumeric(queryPromos)
                    'bookigs = clsDataLayer.ReturnSingleNumeric(queryBookings)

                    cell = New TableCell()
                    'cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Details :  </b>" & "<div style='float:left;'>" & DataBinder.Eval(e.Row.DataItem, "BusName").ToString() & "</br></div>" & "</div>" & "<div style='width:50%;float:left;'>" & "<b>Capacity : </b>" & DataBinder.Eval(e.Row.DataItem, "Capacity").ToString() & "</div>" & "</div>"
                    cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Route Details :  </b>" &
                    "<div style='float:left;'><a href='Bus-Details.aspx?idsale=" + idsale + "'>" & DataBinder.Eval(e.Row.DataItem, "BusName").ToString() &
                    "</a></div></div>" & "<div style='width:50%;float:left;'><div style='width:30%;float:left'><b>Capacity: </b> " & DataBinder.Eval(e.Row.DataItem, "Capacity").ToString() & "</div>" &
                "<div style='width:65%;float:left;'><b>Total Promotions/POS: </b> " & promotions & "<div style='float:right'><b>Total Bookings/POS: </b> " & bookigs & "</div></div></div>"
                    cell.ColumnSpan = 11
                    cell.CssClass = "GroupDetailsStyle"
                    row.Cells.Add(cell)

                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                    intSubTotalIndex += 1
                    '#End Region

                    '#Region "Add Group Header"
                    ' Creating a Row
                    row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)



                    cell = New TableCell()
                    cell.Text = "City IN"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "City OUT"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "TicketID"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Printed"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Price"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Discount"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Subscription No"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Created ON"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Printed On"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Creating POS"
                    cell.CssClass = "GroupHeaderStyle"
                    row.Cells.Add(cell)

                    cell = New TableCell()
                    cell.Text = "Printed By"
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
                dblSubTotalAmount = 0
            End If
            If IsGrandTotalRowNeedtoAdd Then
                Dim grdViewProducts As GridView = DirectCast(sender, GridView)

                '#Region "Adding Empty Row before Grand Total"
                Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                Dim cell As New TableCell()
                cell.Text = String.Empty
                cell.Height = Unit.Parse("10px")
                cell.ColumnSpan = 10
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
                cell.Text = String.Format("{0:n2}", dblGrandTotalAmount)
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
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "IDSALE").ToString()

                If User.IsInRole("restricted") Then

                Else
                    Dim dblQuantity As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Total").ToString())

                    Dim dblAmount As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Total").ToString())

                    ' Cumulating Sub Total

                    dblSubTotalQuantity += dblQuantity

                    dblSubTotalAmount += dblAmount

                    ' Cumulating Grand Total
                    dblGrandTotalQuantity += dblQuantity
                    dblGrandTotalAmount += dblAmount
                End If

                
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

            posNo = pos
            'Define the Queries
            Dim query As String = ""
            Dim queryTotal As String = ""
            Dim graph As String = ""

            'Define the Query for Selecting Bus Name and Bus Details
            'Check if the POS is empty and redirect to POS Ticketing Page

            pos = "TigoCash"

            If String.IsNullOrEmpty(reqDate) Then
                'No Request Date so take for today
                query = "SELECT sl.IDSALE,sl.NAME as 'BusName',sl.DATE_CREAT,sl.FLD103 AS 'Capacity',sl.FLD163 AS 'Hour',sp.IDRELATION,sp.FLD120 as 'CityIN',sp.FLD122 as 'CityOut',sp.PRICE,sp.FLD191 as 'ClientCode'," &
                                        "sp.FLD111 as 'ClientName',sp.DISCOUNT,sp.FLD213 AS 'Subscriptiono',sp.DATE_MOD AS 'CreatedOn'," &
                                            "sp.FLD123 AS 'CreatedBy',sp.Total,CASE WHEN FLD119 IS NULL THEN 'No' ELSE 'Yes' END AS 'Printed',CASE WHEN FLD119 IS NULL THEN NULL ELSE sp.DATE_TRANSF END 'PrintedAt', FLD115 AS 'PrintedBy' " &
                                            "FROM su.SALES as sl INNER JOIN su.SALES_PROD as sp " &
                                             "ON sl.IDSALE = sp.IDSALE AND DAY(sl.TARGET_DATE) = DAY(GETDATE()) AND MONTH(sl.TARGET_DATE) = MONTH(GETDATE()) AND YEAR(sl.TARGET_DATE) = YEAR(GETDATE())   " &
                                               " WHERE (FLD123='" + pos + "' OR FLD123='InnovysShortcode')" &
                                             "ORDER BY IDSALE"
                headerDate.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString
                posText.Text = pos
            Else
                'Date Defined
                Try
                    'Dim exactDate As DateTime = DateTime.Parse(reqDate)
                    Dim exactDate = reqDate
                    headerDate.Text = ": " + reqDate
                    'No Error so lets continue



                    query = String.Format("SELECT sl.IDSALE,sl.NAME as 'BusName',sl.DATE_CREAT,sl.FLD103 AS 'Capacity',sl.FLD163 AS 'Hour',sp.IDRELATION,sp.FLD120 as 'CityIN',sp.FLD122 as 'CityOut',sp.PRICE,sp.FLD191 as 'ClientCode'," &
                                     "sp.FLD111 as 'ClientName',sp.DISCOUNT,sp.FLD213 AS 'Subscriptiono',sp.DATE_MOD AS 'CreatedOn'," &
                                         "sp.FLD123 AS 'CreatedBy',sp.Total,CASE WHEN FLD119 IS NULL THEN 'No' ELSE 'Yes' END AS 'Printed',CASE WHEN FLD119 IS NULL THEN NULL ELSE sp.DATE_TRANSF END 'PrintedAt',FLD115 AS 'PrintedBy',CASE WHEN FLD119 IS NULL THEN NULL ELSE sp.DATE_TRANSF END 'PrintedAt', FLD115 AS 'PrintedBy' " &
                                         "FROM su.SALES as sl INNER JOIN su.SALES_PROD as sp " &
                                          "ON sl.IDSALE = sp.IDSALE AND DAY(sl.TARGET_DATE) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(sl.TARGET_DATE) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(sl.TARGET_DATE) = YEAR(CONVERT(DATETIME,'{0}',103)) " &
                                            " WHERE (FLD123='" + pos + "' OR FLD123='InnovysShortcode')" &
                                          "ORDER BY IDSALE", exactDate)



            headerDate.Text = "For: " + reqDate
            posText.Text = pos
        Catch
        End Try
            End If
            Try
                Dim dt As DataTable = clsDataLayer.ReturnDataTable(query)
                gridViewBuses.DataSource = dt
                gridViewBuses.DataBind()
            Catch ex As Exception
                clsDataLayer.LogException(ex)
            End Try

            Dim posInfo As String = String.Format("POS Name: Tigo Cash")
            ltrPOS.Text = posInfo


        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


        Page.Title = "Tigo Cash Tickets"
    End Sub
End Class
