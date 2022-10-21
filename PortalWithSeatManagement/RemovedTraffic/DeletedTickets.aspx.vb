Imports System.Globalization
Imports System.Data
Partial Class DeletedTickets
    Inherits System.Web.UI.Page
    ' To keep track of the previous row Group Identifier
    Private strPreviousRowID As String = String.Empty

    'Count Promotions And Bookings For the Bus
    Dim promotions As Integer = 0
    Dim bookigs As Integer = 0
    ' To keep track the Index of Group Total
    Private intSubTotalIndex As Integer = 1

    'To Calculate the number of travellers
    Private intNumberofTravellers As Integer = 0
    Private intTotalTravellers As Integer = 0
    Private capacity As Integer = 0

    ' To temporarily store Sub Total
    Private dblSubTotalQuantity As Double = 0
    Private dblSubTotalDiscount As Double = 0
    Private dblSubTotalAmount As Double = 0

    ' To temporarily store Grand Total
    Private dblGrandTotalQuantity As Double = 0
    Private dblGrandTotalDiscount As Double = 0
    Private dblGrandTotalAmount As Double = 0
    Private clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim reqDate As String = ""

        Dim idsale As String
        Try
            idsale = Request.QueryString("idsale")
            reqDate = Request.QueryString("requestDate")
        Catch ex As Exception
            'The Req Date was Not Sent
            reqDate = ""
        End Try
        'Define the Queries
        Dim query As String = ""

        'Get bus Capacity
        Try
            'Dim queryCapacity As String = String.Format("SELECT FLD103 from su.SALES WHERE IDSALE={0}", idsale)
            'capacity = clsDataLayer.ReturnSingleNumeric(queryCapacity)
            capacity = 0
        Catch ex As Exception

        End Try
        'Define the Query for Selecting Bus Name and Bus Details
        'Check if the POS is empty and redirect to POS Ticketing Page


        If String.IsNullOrEmpty(reqDate) Then
            'No Request Date so take for today

            'headerDate.Text = "Today"

            query = "SELECT sl.IDSALE,sp.FLD120+'-'+sp.FLD122 AS 'Route',Description AS 'Username',sl.NAME as 'BusName',sl.Memo AS 'BusMemo',sl.DATE_CREAT,sl.FLD103 AS 'Capacity',sl.FLD163 AS 'Hour',sp.IDRELATION,sp.FLD120 as 'CityIN',sp.FLD122 as 'CityOut',sp.PRICE,sp.FLD191 as 'ClientCode',sp.FLD111 as 'ClientName'," &
                                "sp.DISCOUNT,sp.Total,sp.FLD213 AS 'Subscriptiono',sp.DATE_MOD AS 'CreatedOn',sp.FLD123 AS 'CreatedBy'" &
                                    " FROM su.SALES as sl INNER JOIN su.USER_ROLLED_SALES_PROD as sp" &
                                        " ON sl.IDSALE = sp.IDSALE WHERE DAY(sp.DATE_TRANSF) = DAY(GETDATE()) AND MONTH(sp.DATE_TRANSF)=MONTH(GETDATE()) AND YEAR(sp.DATE_TRANSF)=YEAR(GETDATE())  ORDER BY 'Username',sp.DATE_TRANSF DESC"
            headerDate.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString
        Else
            'Date Defined
            Try
                ' Dim exactDate As DateTime = DateTime.Parse(reqDate)
                Dim exactDate = reqDate
                headerDate.Text = ": " + reqDate

                'No Error so lets continue
                query = String.Format("SELECT sl.IDSALE,sp.FLD120+'-'+sp.FLD122 AS 'Route',sl.Memo AS 'BusMemo',Description AS 'UserName',sl.NAME as 'BusName',sl.DATE_CREAT,sl.FLD103 AS 'Capacity',sl.FLD163 AS 'Hour',sp.IDRELATION,sp.FLD120 as 'CityIN',sp.FLD122 as 'CityOut',sp.PRICE,sp.FLD191 as 'ClientCode',sp.FLD111 as 'ClientName'," &
                                "sp.DISCOUNT,sp.Total,sp.FLD213 AS 'Subscriptiono',sp.DATE_MOD AS 'CreatedOn',sp.FLD123 AS 'CreatedBy'" &
                                    " FROM su.SALES as sl INNER JOIN su.USER_ROLLED_SALES_PROD as sp" &
                                        " ON sl.IDSALE = sp.IDSALE WHERE DAY(sp.DATE_TRANSF)= DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(sp.DATE_TRANSF)= MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(sp.DATE_TRANSF)= YEAR(CONVERT(DATETIME,'{0}',103)) ORDER BY 'Username',sp.DATE_TRANSF", exactDate)
            Catch
            End Try
        End If




        Dim dt As DataTable = clsDataLayer.ReturnDataTable(query)
        gridViewBuses.DataSource = dt
        gridViewBuses.DataBind()

        'Page Title Module
        'Dim queryBusName = "SELECT NAME FROM su.SALES WHERE IDSALE=" + idsale
        'busname.Text = clsDataLayer.ReturnSingleValue(queryBusName)

        busname.Text = "Deleted Tickets Per User"

        Page.Title = busname.Text + " | Sinnovys"

        'Count and Record the Number of Promotions and Bookings

        'Dim queryPromos As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD213='PROMO'", idsale)
        'Dim queryBookings As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD213 NOT IN('PROMO','-')", idsale)

        'promotions = clsDataLayer.ReturnSingleNumeric(queryPromos)
        'bookigs = clsDataLayer.ReturnSingleNumeric(queryBookings)

    End Sub
    Protected Sub gridViewBuses_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowCreated
        Dim IsSubTotalRowNeedToAdd As Boolean = False
        Dim IsGrandTotalRowNeedtoAdd As Boolean = False

        If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "BusName") IsNot Nothing) Then
            If strPreviousRowID <> DataBinder.Eval(e.Row.DataItem, "BusName").ToString() Then
                IsSubTotalRowNeedToAdd = True
            End If
        End If

        If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "BusName") Is Nothing) Then
            IsSubTotalRowNeedToAdd = True
            IsGrandTotalRowNeedtoAdd = True
            intSubTotalIndex = 0
        End If

        '#Region "Inserting first Row and populating fist Group Header details"
        If (strPreviousRowID = String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "BusName") IsNot Nothing) Then
            Dim grdViewProducts As GridView = DirectCast(sender, GridView)

            Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

            'Count and Record the Number of Promotions and Bookings
            'Dim idsale As String = DataBinder.Eval(e.Row.DataItem, "IDSALE").ToString()
            'Dim queryPromos As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD213='PROMO'", idsale)
            'Dim queryBookings As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD213 NOT IN('PROMO','-')", idsale)

            Dim queryPromos As String = "0"
            Dim queryBookings As String = "0"

            'promotions = clsDataLayer.ReturnSingleNumeric(queryPromos)
            'bookigs = clsDataLayer.ReturnSingleNumeric(queryBookings)


            Dim cell As New TableCell()
            'cell.Text =
            '    "<div style='width:100%;'>" & "<div style='width:30%;float:left'><div style='float:left;'>" & "<b>Bus Route Details :  </b>" &
            '        DataBinder.Eval(e.Row.DataItem, "BusName").ToString() &
            '    "</br></div></div>" & "<div style='width:21%;float:left'><div style='width:100%;float:left;'><b>Capacity: </b>" & DataBinder.Eval(e.Row.DataItem, "Capacity").ToString() & "</div>" &
            '"<br/><div style='width:100%;float:left;'><b>Total Promotions: </b> " & promotions & "</div>" &
            '    "<br/><div style='width:100%;float:left;'><b>Total Bookings: </b> " & bookigs & "</div></div>" &
            '    "<div style='width:46%;float:right'><b style='float:left;'>Changes History:</b> <div style='color:red;font-style:oblique;font-size:9px;float:left'>" & DataBinder.Eval(e.Row.DataItem, "BusMemo").ToString & "</div></div></div>"

            cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Route Details :  </b>" & "<div style='float:left;'>" & DataBinder.Eval(e.Row.DataItem, "BusName").ToString() & "</br></div>" & "</div>" & "<div style='width:50%;float:left;'></div>" & "</div>"
            cell.ColumnSpan = 9
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
            cell.Text = "Total"
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
            cell.Text = "ClientCode"
            cell.CssClass = "GroupHeaderStyle"
            row.Cells.Add(cell)

            cell = New TableCell()
            cell.Text = "ClientName"
            cell.CssClass = "GroupHeaderStyle"
            row.Cells.Add(cell)


            cell = New TableCell()
            cell.Text = "Created ON"
            cell.CssClass = "GroupHeaderStyle"
            row.Cells.Add(cell)

            cell = New TableCell()
            cell.Text = "Creating POS"
            cell.CssClass = "GroupHeaderStyle"
            row.Cells.Add(cell)

            cell = New TableCell()
            cell.Text = "Deleting User"
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

            'Adding Total No of Travellers 
            Dim cell As New TableCell()
            cell.Text = "Total Tickets: "
            cell.HorizontalAlign = HorizontalAlign.Right
            cell.ColumnSpan = 4
            cell.CssClass = "SubTotalRowStyle"
            row.Cells.Add(cell)

            'Adding Total Cell 
            cell = New TableCell()
            cell.Text = "Deleted Amount : "
            cell.HorizontalAlign = HorizontalAlign.Right
            cell.ColumnSpan = 3
            cell.CssClass = "SubTotalRowStyle"
            row.Cells.Add(cell)

            cell = New TableCell()
            cell.Text = String.Format("{0:n2}", dblSubTotalAmount)
            cell.HorizontalAlign = HorizontalAlign.Left
            cell.CssClass = "SubTotalRowStyle"
            row.Cells.Add(cell)

            'Adding the Row at the RowIndex position in the Grid
            grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
            intSubTotalIndex += 1
            '#End Region

            '#Region "Adding Next Group Header Details"
            If DataBinder.Eval(e.Row.DataItem, "BusName") IsNot Nothing Then
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

                cell = New TableCell()
                cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Route Details :  </b>" & "<div style='float:left;'>" & DataBinder.Eval(e.Row.DataItem, "BusName").ToString() & "</br></div>" & "</div>" & "<div style='width:50%;float:left;'></div>" & "</div>"
                cell.ColumnSpan = 10
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
                cell.Text = "ClientCode"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "ClientName"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Created ON"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Creating POS"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Deleting User"
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

            'Adding Total Travellers
            cell = New TableCell()
            cell.Text = "Total Tickets Deleted:"
            cell.HorizontalAlign = HorizontalAlign.Left
            cell.ColumnSpan = 3
            cell.CssClass = "GrandTotalRowStyle"
            row.Cells.Add(cell)

            cell = New TableCell()
            cell.Text = String.Format("{0:n0}", intTotalTravellers)


            'Calculate Percentage of Bus Capacity Occupied

            cell.HorizontalAlign = HorizontalAlign.Right
            cell.CssClass = "GrandTotalRowStyle"
            row.Cells.Add(cell)

            'Adding Total Cell 
            cell = New TableCell()
            cell.Text = "Total Deleted Amount "
            cell.HorizontalAlign = HorizontalAlign.Right
            cell.ColumnSpan = 6
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
            cell.Text = "RWF " + String.Format("{0:n0}", dblGrandTotalAmount) + ".00"
            'cell.Text = dblGrandTotalAmount.ToString("C", CultureInfo.CreateSpecificCulture("da-DK"))
            cell.HorizontalAlign = HorizontalAlign.Right
            cell.CssClass = "GrandTotalRowStyle"
            row.Cells.Add(cell)

            'Adding the Row at the RowIndex position in the Grid
            '#End Region
            grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex, row)
        End If
    End Sub

    Protected Sub gridViewBuses_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "BusName").ToString()

            Dim dblQuantity As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Price").ToString())

            Dim dblAmount As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Price").ToString())

            ' Cumulating Sub Total

            dblSubTotalQuantity += dblQuantity

            dblSubTotalAmount += dblAmount

            'Cumulate the number of travellers
            intNumberofTravellers += 1
            intTotalTravellers += 1

            ' Cumulating Grand Total
            dblGrandTotalQuantity += dblQuantity
            dblGrandTotalAmount += dblAmount
        End If
    End Sub
End Class
