Imports System.Globalization
Imports System.Data
Partial Class Other_Bus_Details
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
    Private prebookingCount As Integer = 0

    ' To temporarily store Sub Total
    Private dblSubTotalQuantity As Double = 0
    Private dblSubTotalDiscount As Double = 0
    Private dblSubTotalAmount As Double = 0
    Private dblSubTotalAmountRWF As Double = 0
    Private dblSubTotalAmountFIB As Double = 0
    Private dblPrebookingCount As Double = 0

    ' To temporarily store Grand Total
    Private dblGrandTotalQuantity As Double = 0
    Private dblGrandTotalDiscount As Double = 0
    Private dblGrandTotalAmount As Double = 0
    Private dblGrandTotalAmountRWF As Double = 0
    Private dblGrandTotalAmountFIB As Double = 0
    Private dblGrandPrebooking As Double = 0
    Private dblGrandPrebooks As Double = 0

    Private clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim reqDate As String = ""

            Dim idsale As String = ""
            Try
                idsale = Request.QueryString("idsale")
                reqDate = Request.QueryString("requestDate")
            Catch ex As Exception
                'The Req Date was Not Sent
                reqDate = ""
            End Try
            'Define the Queries
            Dim query As String = ""
            Dim queryPrebooking As String = ""

            If idsale = String.Empty Then
                Response.Redirect("/All-Days--Planned-Traffic.aspx")
            End If

            'Get bus Capacity
            Try
                Dim queryCapacity As String = String.Format("SELECT FLD103 from su.SALES WHERE IDSALE={0}", idsale)
                capacity = clsDataLayer.ReturnSingleNumeric(queryCapacity)
            Catch ex As Exception

            End Try
            'Define the Query for Selecting Bus Name and Bus Details
            'Check if the POS is empty and redirect to POS Ticketing Page


            If String.IsNullOrEmpty(reqDate) Then
                'No Request Date so take for today

                'headerDate.Text = "Today"

                query = "SELECT sl.IDSALE,sp.FLD120+'-'+sp.FLD122 AS 'Route',sl.NAME as 'BusName',sl.Memo AS 'BusMemo',sl.DATE_CREAT,sl.FLD103 AS 'Capacity',sl.FLD163 AS 'Hour',sp.IDRELATION,sp.FLD120 as 'CityIN',sp.FLD122 as 'CityOut',sp.PRICE,sp.FLD191 as 'ClientCode',sp.FLD111 as 'ClientName'," &
                                    "sp.DISCOUNT,sp.Total,sp.FLD213 AS 'Subscriptiono',sp.DATE_MOD AS 'CreatedOn',sp.FLD123 AS 'CreatedBy',sp.FLD118 AS 'Currency',sl.TotalRWF,sl.TotalBWF as 'TotalFIB',sp.FLD133 as 'Passport'" &
                                        " FROM su.SALES as sl INNER JOIN su.SALES_PROD as sp" &
                                            " ON sl.IDSALE = sp.IDSALE WHERE sp.IDSALE=" + idsale + " AND DAY(sp.FLD108) = DAY(GETDATE())  ORDER BY Route,'CreatedOn'"
                queryPrebooking = "SELECT     BookingNo, Date, Hour, CityIN, CityOut, Discount, Creator, Name, FirstName, ClientCode, IDSALE, CreatedOn, Expires, Completed, " &
                                    "Recon, CityIN+'-'+CityOut AS 'Route' FROM Prebookings WHERE IDSALE=" + idsale
            Else
                'Date Defined
                Try
                    ' Dim exactDate As DateTime = DateTime.Parse(reqDate)
                    Dim exactDate = reqDate
                    '    headerDate.Text = ": " + reqDate

                    'No Error so lets continue
                    query = String.Format("SELECT sl.IDSALE,sp.FLD120+'-'+sp.FLD122 AS 'Route',sl.Memo AS 'BusMemo',sl.NAME as 'BusName',sl.DATE_CREAT,sl.FLD103 AS 'Capacity',sl.FLD163 AS 'Hour',sp.IDRELATION,sp.FLD120 as 'CityIN',sp.FLD122 as 'CityOut',sp.PRICE,sp.FLD191 as 'ClientCode',sp.FLD111 as 'ClientName'," &
                                    "sp.DISCOUNT,sp.Total,sp.FLD213 AS 'Subscriptiono',sp.DATE_MOD AS 'CreatedOn',sp.FLD123 AS 'CreatedBy',sp.FLD118 AS 'Currency',sl.TotalRWF,sl.TotalBWF as 'TotalFIB',sp.FLD133 as 'Passport'" &
                                        " FROM su.SALES as sl INNER JOIN su.SALES_PROD as sp" &
                                            " ON sl.IDSALE = sp.IDSALE WHERE sp.IDSALE=" + idsale + " AND DAY(sp.FLD108)= DAY(CONVERT(DATETIME,'{0}',103)) ORDER BY Route,'CreatedOn'", exactDate)

                    queryPrebooking = "SELECT     BookingNo, Date, Hour, CityIN, CityOut, Discount, Creator, Name, FirstName, ClientCode, IDSALE, CreatedOn, Expires, Completed, " &
                                    "Recon FROM Prebookings WHERE IDSALE=" + idsale
                Catch
                End Try
            End If




            Dim dt As DataTable = clsDataLayer.ReturnDataTable(query)
            gridViewBuses.DataSource = dt
            gridViewBuses.DataBind()

            Try
                Dim dtPb As DataTable = clsDataLayer.ReturnDataTable(queryPrebooking)
                grvPrebookings.DataSource = dtPb
                grvPrebookings.DataBind()
            Catch ex As Exception
                clsDataLayer.LogException(ex)
            End Try


            Try
                'Page Title Module
                Dim queryBusName = "SELECT NAME FROM su.SALES WHERE IDSALE=" + idsale
                busname.Text = clsDataLayer.ReturnSingleValue(queryBusName)

                Page.Title = "Sinnovys " + busname.Text
            Catch ex As Exception
                clsDataLayer.LogException(ex)
            End Try


            Try
                'Count and Record the Number of Promotions and Bookings

                Dim queryPromos As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD213='PROMO'", idsale)
                Dim queryBookings As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD213 NOT IN('PROMO','-')", idsale)

                promotions = clsDataLayer.ReturnSingleNumeric(queryPromos)
                bookigs = clsDataLayer.ReturnSingleNumeric(queryBookings)
            Catch ex As Exception
                clsDataLayer.LogException(ex)
            End Try


        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


    End Sub
    Protected Sub gridViewBuses_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowCreated
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

                'Count and Record the Number of Promotions and Bookings
                Dim idsale As String = DataBinder.Eval(e.Row.DataItem, "IDSALE").ToString()
                Dim queryPromos As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD213='PROMO'", idsale)
                Dim queryBookings As String = String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0} AND FLD213 NOT IN('PROMO','-')", idsale)

                promotions = clsDataLayer.ReturnSingleNumeric(queryPromos)
                bookigs = clsDataLayer.ReturnSingleNumeric(queryBookings)


                Dim cell As New TableCell()
                cell.Text =
                    "<div style='width:100%;'>" & "<div style='width:30%;float:left'><div style='float:left;'>" & "<b>Bus Route Details :  </b>" &
                        DataBinder.Eval(e.Row.DataItem, "Route").ToString() &
                    "</br></div></div>" & "<div style='width:21%;float:left'><div style='width:100%;float:left;'><b>Capacity: </b>" & DataBinder.Eval(e.Row.DataItem, "Capacity").ToString() & "</div>" &
                "<br/><div style='width:100%;float:left;'><b>Total Promotions: </b> " & promotions & "</div>" &
                    "<br/><div style='width:100%;float:left;'><b>Total Bookings: </b> " & bookigs & "</div></div>" &
                    "<div style='width:46%;float:right'><b style='float:left;'>Changes History:</b> <div style='color:red;font-style:oblique;font-size:9px;float:left'>" & DataBinder.Eval(e.Row.DataItem, "BusMemo").ToString & "</div></div></div>"

                cell.ColumnSpan = 12
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
                cell.Text = "Currency"
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
                cell.Text = "Passport"
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
                cell.Text = "Total Number of Travellers: "
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.ColumnSpan = 3
                cell.CssClass = "SubTotalRowStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = String.Format("{0:N0}", intNumberofTravellers)
                cell.HorizontalAlign = HorizontalAlign.Left
                cell.CssClass = "SubTotalRowStyle"
                row.Cells.Add(cell)

                'Reset the Total Number of Travellers
                intNumberofTravellers = 0

                'Adding Total Cell 
                cell = New TableCell()
                cell.Text = "Total For This Route: "
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.ColumnSpan = 3
                cell.CssClass = "SubTotalRowStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                'cell.Text = String.Format("{0:n2}", dblSubTotalAmount)
                cell.Text = String.Format("RWF: {0:n2}, FIB: {1:n2}", dblSubTotalAmountRWF, dblSubTotalAmountFIB)
                cell.HorizontalAlign = HorizontalAlign.Left
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
                    cell.ColumnSpan = 10
                    row.Cells.Add(cell)
                    row.BorderStyle = BorderStyle.None

                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                    intSubTotalIndex += 1
                    '#End Region

                    '#Region "Adding Next Group Header Details"
                    row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                    cell = New TableCell()
                    cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Route Details :  </b>" & "<div style='float:left;'>" & DataBinder.Eval(e.Row.DataItem, "Route").ToString() & "</br></div>" & "</div>" & "<div style='width:50%;float:left;'></div>" & "</div>"
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
                    cell.Text = "Currency"
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
                    cell.Text = "Passport"
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


                    'Adding the Row at the RowIndex position in the Grid
                    grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                    '#End Region
                    intSubTotalIndex += 1
                End If
                '#End Region

                '#Region "Reseting the Sub Total Variables"
                dblSubTotalQuantity = 0
                dblSubTotalDiscount = 0
                dblSubTotalAmountRWF = 0
                dblSubTotalAmountFIB = 0
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
                cell.ColumnSpan = 12
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
                cell.Text = "Grand Total Travellers: "
                cell.HorizontalAlign = HorizontalAlign.Left
                cell.ColumnSpan = 3
                cell.CssClass = "GrandTotalRowStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = String.Format("{0:n0}", intTotalTravellers)
                cell.Text += " / " + capacity.ToString

                'Calculate Percentage of Bus Capacity Occupied
                Dim percentage As Int16 = (intTotalTravellers / capacity * 100)
                cell.Text += " (" + percentage.ToString + "% Full)"
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.CssClass = "GrandTotalRowStyle"
                row.Cells.Add(cell)

                'Adding Total Cell 
                cell = New TableCell()
                cell.Text = "Grand Total For this Bus: "
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.ColumnSpan = 5
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
                cell.Text = String.Format("RWF: {0:n0}, FIB: {1:n0}", dblGrandTotalAmountRWF, dblGrandTotalAmountFIB)
                'cell.Text = dblGrandTotalAmount.ToString("C", CultureInfo.CreateSpecificCulture("da-DK"))
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.CssClass = "GrandTotalRowStyle"
                row.Cells.Add(cell)

                'Adding the Row at the RowIndex position in the Grid
                '#End Region
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex, row)

                'Working on Total Discount

                ' Creating a Row
                row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)


                'Adding Total Cell 
                cell = New TableCell()
                cell.Text = "Total Discount For this Bus: "
                cell.HorizontalAlign = HorizontalAlign.Right
                cell.ColumnSpan = 5
                cell.CssClass = "GrandTotalRowStyle"
                row.Cells.Add(cell)

                'Calculate the Discount
                Dim idsale As String = DataBinder.Eval(e.Row.DataItem, "IDSALE").ToString()
                Dim queryTotalDiscount As String = String.Format("SELECT SUM(Discount) FROM su.SALES_PROD WHERE IDSALE={0}", idsale)
                dblGrandTotalDiscount = clsDataLayer.ReturnSingleNumeric(queryTotalDiscount)

                'Adding Amount Column
                cell = New TableCell()
                cell.Text = "RWF " + String.Format("{0:n0}", dblGrandTotalDiscount) + ".00"
                'cell.Text = dblGrandTotalAmount.ToString("C", CultureInfo.CreateSpecificCulture("da-DK"))
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

    Protected Sub grvPrebookings_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowCreated
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
                cell.Text =
                    "<div style='width:100%;'>" & "<div style='width:30%;float:left'><div style='float:left;'>" & "<b>Bus Route Details :  </b>" &
                        DataBinder.Eval(e.Row.DataItem, "Route").ToString() &
                    "</br></div></div></div>"

                cell.ColumnSpan = 10
                cell.CssClass = "GroupDetailsStyle"
                row.Cells.Add(cell)

                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                intSubTotalIndex += 1

                '#Region "Add Group Header"
                ' Creating a Row
                row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                cell = New TableCell()
                cell.Text = "Booking No"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "City IN"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "City OUT"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Name"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Last Name"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Client Code"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Created On"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Expires On"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Booking Paid"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Reconciled"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                'Adding the Row at the RowIndex position in the Grid
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                '#End Region
                intSubTotalIndex += 1
            End If
            '#End Region



        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

    End Sub

    Protected Sub gridViewBuses_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "Route").ToString()

                Dim dblQuantity As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Total").ToString())

                Dim dblAmount As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Total").ToString())



                ' Cumulating Sub Total

                'Acknowledge Restricted Accounts
                If User.IsInRole("restricted") Then

                    'Too Bad For them They Can't See All this Gold Info
                Else
                    Dim currency As String = DataBinder.Eval(e.Row.DataItem, "Currency").ToString()
                    Try
                        'dblQuantity = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Budget").ToString())
                        dblQuantity = 0
                        dblAmount = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Total").ToString())
                        If currency = "RWF" Then
                            dblSubTotalAmountRWF += dblAmount
                            dblGrandTotalAmountRWF += dblAmount
                        ElseIf currency = "FIB" Then
                            dblSubTotalAmountFIB += dblAmount
                            dblGrandTotalAmountFIB += dblAmount
                        End If

                    Catch ex As Exception
                        currency = ""
                        clsDataLayer.LogException(ex)
                    End Try

                    dblSubTotalQuantity += dblQuantity

                    dblSubTotalAmount += dblAmount



                    ' Cumulating Grand Total
                    dblGrandTotalQuantity += dblQuantity
                    dblGrandTotalAmount += dblAmount

                End If

                'Cumulate the number of travellers
                intNumberofTravellers += 1
                intTotalTravellers += 1


            End If
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try
    End Sub
End Class
