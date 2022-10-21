Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Partial Class TigoCashMonthly
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'We need to query the database for the months revenue trend and draw it
            Dim reqDate As String
            Dim queryReport As String = ""
            Dim month As String = ""
            Try
                reqDate = Request("requestDate")
                Dim tryDate = DateTime.Parse(reqDate, CultureInfo.CreateSpecificCulture("fr-FR"))
                month = MonthName(tryDate.Month.ToString) + "-" + tryDate.Year.ToString
            Catch ex As Exception
                reqDate = ""
                month = MonthName(DateTime.Now.Month) + "-" + DateTime.Now.Year.ToString
            End Try

            headerDate.Text = month

            If String.IsNullOrEmpty(reqDate) Then
                'They did not pass a valid date
                queryReport = "SET DATEFIRST 1;SELECT CONVERT(VARCHAR,DATEPART(d,FLD108)) AS 'Day',SUM(Total) AS 'TotalSales' " &
                                "FROM su.SALES_PROD WHERE Month(FLD108) = Month(GETDATE()) " &
                                    "AND YEAR(FLD108) =YEAR(GETDATE())  AND FLD123 = 'TigoCash' GROUP BY CONVERT(VARCHAR,DATEPART(d,FLD108)),DAY(FLD108)" &
                                        "ORDER BY DAY(FLD108) ASC"
            Else
                'They passed a valid date
                queryReport = String.Format("SET DATEFIRST 1;SELECT CONVERT(VARCHAR,DATEPART(d,FLD108)) AS 'Day',SUM(Total) AS 'TotalSales' " &
                                "FROM su.SALES_PROD WHERE Month(FLD108) = Month(CONVERT(DATETIME,'{0}',103)) " &
                                    "AND YEAR(FLD108) =YEAR(CONVERT(DATETIME,'{0}',103)) AND FLD123 = 'TigoCash' GROUP BY CONVERT(VARCHAR,DATEPART(d,FLD108)),DAY(FLD108)" &
                                        "ORDER BY DAY(FLD108) ASC", reqDate)
            End If

            'So we have a valid query by now lets execute and draw the chart
            ' Now Work on the Area Chart
            Dim dt As DataTable = clsDataLayer.ReturnDataTable(queryReport)
            Dim json As String = "[['Day of Month','Total Tigo Cash Sales Today'],"


            For Each rw As DataRow In dt.Rows
                json += String.Format("['{0}',{1}],", rw("Day").ToString, rw("TotalSales").ToString)
            Next
            Dim charsToTrim() As Char = {","c}
            Dim jsonF As String = json.TrimEnd(charsToTrim)
            jsonF += "]"

            Dim mys As StringBuilder = New StringBuilder()
            'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawAreaChart);")
            mys.AppendLine("function drawAreaChart() {")
            mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
            mys.AppendLine("var options = {title:  'Monthly Sales Trend',hAxis: { title: 'Day of the Month', titleTextStyle: { color: 'red'} }};")
            mys.AppendLine("var chart = new google.visualization.AreaChart(document.getElementById('month_chart'));")
            mys.AppendLine("chart.draw(data, options);")
            mys.AppendLine("}")

            Dim AreaScript As String = mys.ToString()
            Dim initScript As String = "function initCharts () {drawAreaChart()}" +
                               "google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(initCharts);"

            Dim script As String = AreaScript + initScript

            Dim csType As Type = Me.[GetType]()
            Dim cs As ClientScriptManager = Page.ClientScript
            cs.RegisterClientScriptBlock(csType, "charts", script, True)
        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


        Page.Title = "Monthly Revenue Report"
    End Sub
End Class
