Imports System.Data.SqlClient
Imports System.Data

Partial Class OtherTicketDetails
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack Then

            Else
                Dim ticketID As String = "0"
                Try
                    ticketID = Request.QueryString("TicketId")
                    If ticketID = " " Or ticketID = "" Then
                        ticketID = "0"
                    End If
                Catch ex As Exception
                    ticketID = "0"
                End Try

                'Query for the TicketDetails
                Dim query As String =
                    String.Format("SELECT sp.IDRELATION,sp.DATE_MOD AS 'DateOfTravel',sp.FLD215 AS 'Circuit',sp.FLD109 AS 'Hour',sp.Total,sp.FLD111 AS 'Traveler',FLD191 AS 'ClientCode',s.NAME AS 'BusName'" &
                                    "FROM su.SALES_PROD AS sp INNER JOIN su.SALES AS s ON sp.IDSALE = s.IDSALE" &
                                        " WHERE IDRELATION = {0}", ticketID)

                Dim dt As DataTable = clsDataLayer.ReturnDataTable(query)
                ticketDetails.DataSource = dt
                ticketDetails.DataBind()

                'Now Construct the QR Code
                'https://chart.googleapis.com/chart?cht=qr&chs=160x160&chl=840240239+KIGALI-MUHANGA:09H00+PETER
                Dim tinfo As String = ""
                With dt.Rows(0)
                    tinfo = String.Format("{0}+{1}+{2}+{3}+{4}+Total:{5}", .Item("IDRELATION").ToString, .Item("BusName").ToString, .Item("Circuit").ToString, .Item("Traveler").ToString, .Item("ClientCode").ToString, .Item("Total").ToString)
                End With

                Dim qr As String = String.Format("https://chart.googleapis.com/chart?cht=qr&chs=190x190&chl={0}", tinfo)
                imgQr.ImageUrl = qr

                Try
                    'Do the chart now
                    Dim queryChart As String =
                        String.Format("SELECT SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'," &
                            "SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'," &
                            "CONVERT(DATETIME,CONVERT(VARCHAR,MONTH(FLD108))+'-01-'+CONVERT(VARCHAR,YEAR(FLD108))) AS 'Month',DATENAME(MONTH,FLD108) AS 'MonthName'" &
                                " FROM su.SALES_PROD" &
                                " WHERE FLD191 = {0} And DateDiff(m, FLD108, GETDATE()) <= 3" &
                                        " GROUP BY CONVERT(DATETIME,CONVERT(VARCHAR,MONTH(FLD108))+'-01-'+CONVERT(VARCHAR,YEAR(FLD108))),DATENAME(MONTH,FLD108)" &
                                " ORDER BY 'Month' ASC", dt.Rows(0).Item("ClientCode").ToString)

                    Dim dtTrend = clsDataLayer.ReturnDataTable(queryChart)

                    Dim json As String = "[['Month','Total Tickets','Total Booking','Total Promotions'],"


                    For Each rw As DataRow In dtTrend.Rows()
                        json += String.Format("['{0}',{1},{2},{3}],", rw("MonthName").ToString, rw("TotalNormal").ToString, rw("TotalBooking").ToString, rw("TotalPromo").ToString)
                    Next
                    Dim charsToTrim() As Char = {","c}
                    Dim jsonF As String = json.TrimEnd(charsToTrim)
                    jsonF += "]"

                    Dim mys As StringBuilder = New StringBuilder()
                    'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawAreaChart);")
                    mys.AppendLine("function drawAreaChart() {")
                    mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                    mys.AppendLine("var options = {title:  'Passenger Trend',hAxis: { title: 'Month', titleTextStyle: { color: 'red'} }};")
                    mys.AppendLine("var chart = new google.visualization.AreaChart(document.getElementById('chart_div'));")
                    mys.AppendLine("chart.draw(data, options);")
                    mys.AppendLine("}")

                    Dim initScript As String = mys.ToString + "function initCharts () {drawAreaChart();}" +
                                            "google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(initCharts);"

                    Dim csType As Type = Me.[GetType]()
                    Dim cs As ClientScriptManager = Page.ClientScript
                    cs.RegisterClientScriptBlock(csType, "charts", initScript, True)
                Catch ex As Exception
                    clsDataLayer.LogException(ex)
                End Try

            End If
        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try

        Page.Title = "Ticket Details"
    End Sub

    Protected Sub btnDeleteTicket_Click(sender As Object, e As EventArgs) Handles btnDeleteTicket.Click
        'Now here we need to rollback the ticket marking the username in the process
        Try
            Dim cmd As New SqlCommand()

            Dim currentUser As String = HttpContext.Current.User.Identity.Name

            Dim ticketId As String = "0"
            If Request.QueryString("Ticketid") IsNot Nothing Then
                ticketId = Request.QueryString("Ticketid")
            End If


            With cmd
                .CommandText = "su.AdminTicketRollback"
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@TicketID", ticketId)
                .Parameters.AddWithValue("@Username", currentUser)
            End With

            clsDataLayer.ExecuteProc(cmd)
            Response.Redirect("/All-Days--Planned-Traffic.aspx")
        Catch ex As Exception
            ltrErrorMessage.Text = "Ticket Could Not Be Deleted Due to the folowing Error: " + ex.Message
            clsDataLayer.LogException(ex)
        End Try
    End Sub
End Class
