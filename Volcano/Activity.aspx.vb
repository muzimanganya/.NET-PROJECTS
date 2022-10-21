Imports System.Data.SqlClient
Imports System.Data

Partial Class ClientCodeActivity
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Private reqDatePassed As String
    Private filter As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim reqDate As String = ""
            Dim pos As String = ""
            Try

                'filter = Request.QueryString("filter")
                reqDate = Request.QueryString("requestDate")
                filter = ddlFilter.SelectedValue
            Catch ex As Exception
                'The Req Date was Not Sent
                reqDate = ""
                'filter = "3"
            End Try

            If filter = String.Empty Then
                filter = 3
                ddlFilter.SelectedValue = filter
            End If
            If reqDate = String.Empty Then
                reqDate = ""
            End If
            reqDatePassed = reqDate
            'Define the Queries
            Dim query As String = ""
            Dim queryTotal As String = ""
            Dim graph As String = ""
            Dim queryTotalForDay As String = ""

            'Define the Query for Selecting Bus Name and Bus Details
            'Check if the POS is empty and redirect to POS Ticketing Page


            If String.IsNullOrEmpty(reqDate) Then
                'No Request Date so take for today
                query = String.Format("SELECT FLD191 AS 'ClientCode',SUM(DISCOUNT) AS 'TotalDiscount',COUNT(IDRELATION) AS 'TotalTickets', " &
                            "COUNT(DISTINCT(FLD123)) AS 'TotalPOS',SUM(Total) AS 'TotalAmount',SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal', " &
                                "SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo', " &
                                    "SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking' " &
                                        "FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE " &
                                            "WHERE Day(su.SALES_PROD.FLD108) = Day(GETDATE()) And Month(su.SALES_PROD.FLD108) = Month(GETDATE()) " &
                                                "AND YEAR(su.SALES_PROD.FLD108) = YEAR(GETDATE()) " &
                                                    "AND FLD191 <> 0  GROUP BY FLD191 HAVING COUNT(IDRELATION) > {0} ORDER BY COUNT(IDRELATION) DESC", filter)


                headerDate.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString



            Else
                'Date Defined
                Try
                    'Dim exactDate As DateTime = DateTime.Parse(reqDate)
                    Dim exactDate = reqDate
                    headerDate.Text = ": " + reqDate
                    'No Error so lets continue
                    query = String.Format("SELECT FLD191 AS 'ClientCode',SUM(DISCOUNT) AS 'TotalDiscount',COUNT(IDRELATION) AS 'TotalTickets', " &
                            "COUNT(DISTINCT(FLD123)) AS 'TotalPOS',SUM(Total) AS 'TotalAmount',SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal', " &
                                "SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo', " &
                                    "SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking' " &
                                        "FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE " &
                                            "WHERE DAY(su.SALES_PROD.FLD108) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(su.SALES_PROD.FLD108) = MONTH(CONVERT(DATETIME,'{0}',103)) " &
                                                "AND YEAR(su.SALES_PROD.FLD108) = YEAR(CONVERT(DATETIME,'{0}',103)) " &
                                                    "AND FLD191 <> 0  GROUP BY FLD191 HAVING COUNT(IDRELATION) > {1} ORDER BY COUNT(IDRELATION) DESC", exactDate, filter)
                Catch

                End Try
            End If

            Dim ds As New DataSet
            Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)
            ds.Tables.Add(dt)
            rptMasterView.DataSource = ds
            rptMasterView.DataBind()


            ' Now Work on the Pie Chart

            Dim totalNormal As Integer, totalPromos As Integer, totalBooking As Integer, totalDiscount As Integer, TotalTickets As Integer

            totalNormal = dt.Compute("SUM(TotalNormal)", "")
            totalPromos = dt.Compute("SUM(TotalPromo)", "")
            totalBooking = dt.Compute("SUM(TotalBooking)", "")
            totalDiscount = dt.Compute("SUM(TotalDiscount)", "")
            TotalTickets = totalNormal + totalBooking + totalPromos

            Dim json As String = "[['Revenue Mode','Total Tickets'],"


            json += String.Format("['Normal Ticketing',{0}],['Promotions',{1}],['Booking',{2}]", totalNormal, totalPromos, totalBooking)

            Dim charsToTrim() As Char = {","c}
            Dim jsonF As String = json.TrimEnd(charsToTrim)
            jsonF += "]"

            Dim mys As StringBuilder = New StringBuilder()
            mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
            mys.AppendLine("function drawChart() {")
            mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
            mys.AppendLine("var options = {title: 'Total Revenue Per Route',legend: {position: 'none'}};")
            mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('piechart'));")
            mys.AppendLine("chart.draw(data, options);")
            mys.AppendLine("}")

            Dim script As String = mys.ToString()
            Dim csType As Type = Me.[GetType]()
            Dim cs As ClientScriptManager = Page.ClientScript
            cs.RegisterClientScriptBlock(csType, "pie", script, True)

            'Prepare the Stats at the Bottom
            ltrTotalTickets.Text = TotalTickets
            ltrTotalDiscount.Text = String.Format("RWF: {0:N2}", totalDiscount)
        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


        Page.Title = "Traffic & Business Overview"
    End Sub

    'Protected Sub ddlFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFilter.SelectedIndexChanged
    '    Try
    '        Dim baseValue As Integer
    '        baseValue = Convert.ToInt16(ddlFilter.SelectedValue)
    '        Dim redString As String = String.Format("Activity.aspx?reqDate={0}&filter={1}", reqDatePassed, filter)
    '        Response.Redirect(redString)
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class
