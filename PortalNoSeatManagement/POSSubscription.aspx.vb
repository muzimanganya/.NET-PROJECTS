Imports System.Data.SqlClient
Imports System.Data

Partial Class POSSubscription
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim reqDate As String = ""
            Try
                reqDate = Request.QueryString("requestDate")
            Catch ex As Exception
                'The Req Date was Not Sent
                reqDate = ""
            End Try
            'Define the Queries
            Dim query As String = ""
            Dim queryTotal As String = ""
            Dim graph As String = ""

            If String.IsNullOrEmpty(reqDate) Then
                'No Request Date so take for today
                query = "Select IDCUSTOM1, FLD248 AS 'CreatedBy', NAME, FLD243 AS 'Trajet', FLD244 AS 'Trips'," &
                                    "FLD249 AS 'Traveler', FLD261 AS 'Price', FLD262 AS 'CreatedDate',FLD272 AS 'Discount',FLD261-FLD272 AS 'Total'" &
                                        "FROM su.CUSTOM1 WHERE  FLD250=2 AND CONVERT(varchar(8), FLD262, 3) = (SELECT CONVERT(VARCHAR(8), GETDATE(), 3) AS [DD/MM/YY])"

                queryTotal = "SELECT SUM(FLD261-FLD272)  AS 'Total' FROM su.CUSTOM1 WHERE CONVERT(varchar(8), FLD262, 3) = " &
                "(SELECT CONVERT(VARCHAR(8), GETDATE(), 3) AS [DD/MM/YY]) and FLD250=2"

                graph = "SET DATEFIRST 1;SELECT DATENAME(WEEKDAY,FLD262) AS 'Weekday',SUM(FLD261-FLD272) AS 'TotalSales' FROM su.CUSTOM1" &
                                                        " WHERE(DatePart(WeeK, FLD262) = DatePart(week, GETDATE())) AND YEAR(FLD262) = YEAR(GETDATE())" &
                                                            "GROUP BY DATENAME(WEEKDAY,FLD262),DATEPART(d,FLD262) ORDER BY DATEPART(d,FLD262)"
                headerDate.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString
            Else
                'Date Defined
                Try
                    'Dim exactDate As DateTime = DateTime.Parse(reqDate)
                    Dim exactDate = reqDate
                    headerDate.Text = ": " + reqDate
                    'No Error so lets continue
                    query = String.Format("Select IDCUSTOM1, FLD248 AS 'CreatedBy', NAME, FLD243 AS 'Trajet', FLD244 AS 'Trips'," &
                                    "FLD249 AS 'Traveler', FLD261 AS 'Price', FLD262 AS 'CreatedDate',FLD272 AS 'Discount',FLD261-FLD272 AS 'Total'" &
                                        "FROM su.CUSTOM1 WHERE  FLD250=2 AND DAY(CONVERT(DATETIME, FLD262, 103)) = DAY(CONVERT(DATETIME,'{0}',103)) " &
                                        "AND MONTH(CONVERT(DATETIME, FLD262, 103)) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(CONVERT(DATETIME, FLD262, 103)) = YEAR(CONVERT(DATETIME,'{0}',103))", exactDate)

                    queryTotal = String.Format("SELECT SUM(FLD261-FLD272)  AS 'Total' FROM su.CUSTOM1 WHERE DAY(CONVERT(DATETIME, FLD262, 103)) = " &
                    "DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(CONVERT(DATETIME, FLD262, 103)) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(CONVERT(DATETIME, FLD262, 103)) = YEAR(CONVERT(DATETIME,'{0}',103)) and FLD250=2", exactDate)

                    graph = String.Format("SET DATEFIRST 1;SELECT DATENAME(WEEKDAY,FLD262) AS 'Weekday',SUM(FLD261-FLD272) AS 'TotalSales' FROM su.CUSTOM1" &
                                                        " WHERE(DatePart(WeeK, FLD262) = DatePart(week, CONVERT(DATETIME,'{0}',103))) AND YEAR(FLD262) = YEAR(GETDATE())" &
                                                            "GROUP BY DATENAME(WEEKDAY,FLD262),DATEPART(d,FLD262) ORDER BY DATEPART(d,FLD262)", exactDate)
                Catch
                End Try
            End If

            Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)
            rptMasterView.DataSource = dt
            rptMasterView.DataBind()

            If User.IsInRole("restricted") Then
            Else
                'Calculate the Total Sales Today

                Dim dtT As DataTable = clsDataLayer.ReturnDataTable(queryTotal)
                rptSummary.DataSource = dtT
                rptSummary.DataBind()

                Try
                    ' Now Work on the Pie Chart        

                    Dim newDt = clsDataLayer.ReturnDataTable(graph)
                    Dim json As String = "[['Weekday','Total Revenue Today'],"

                    For Each rw As DataRow In newDt.Rows
                        json += String.Format("['{0}',{1}],", rw("Weekday").ToString, rw("TotalSales").ToString)
                    Next
                    Dim charsToTrim() As Char = {","c}
                    Dim jsonF As String = json.TrimEnd(charsToTrim)
                    jsonF += "]"
                    Dim mys As StringBuilder = New StringBuilder()
                    mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
                    mys.AppendLine("function drawChart() {")
                    mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                    mys.AppendLine("var options = {title:  'Weekly Subscription Sales Trend',hAxis: { title: 'Day of Week', titleTextStyle: { color: 'red'} }};")
                    mys.AppendLine("var chart = new google.visualization.AreaChart(document.getElementById('chart_div'));")
                    mys.AppendLine("chart.draw(data, options);")
                    mys.AppendLine("}")

                    Dim script As String = mys.ToString()
                    Dim csType As Type = Me.[GetType]()
                    Dim cs As ClientScriptManager = Page.ClientScript
                    cs.RegisterClientScriptBlock(csType, "pie", script, True)
                Catch ex As Exception
                    clsDataLayer.LogException(ex)
                End Try

            End If
            
        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


        Page.Title = "Subscription Information"
    End Sub
End Class
