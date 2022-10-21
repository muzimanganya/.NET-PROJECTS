Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Partial Class WeeklyRevenueReport
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
                'month = MonthName(tryDate.Month.ToString) + "-" + tryDate.Year.ToString
                month = ""
            Catch ex As Exception
                reqDate = ""
                month = "'"
            End Try

            headerDate.Text = month

            If String.IsNullOrEmpty(reqDate) Then
                'They did not pass a valid date
                queryReport = "SET DATEFIRST 1;SELECT DATENAME(WEEKDAY,FLD108) AS 'Weekday',SUM(Total) AS 'TotalSales' FROM su.SALES_PROD WHERE DatePart(WeeK, FLD108) = DatePart(week, GETDATE()) AND YEAR(FLD108) =YEAR(GETDATE())  GROUP BY DATENAME(WEEKDAY,FLD108),FLD108 ORDER BY FLD108 "
            Else
                'They passed a valid date
                queryReport = String.Format("SET DATEFIRST 1;SELECT DATENAME(WEEKDAY,FLD108) AS 'Weekday',SUM(Total) AS 'TotalSales' FROM su.SALES_PROD WHERE DatePart(WeeK, FLD108) = DatePart(week, CONVERT(DATETIME,'{0}',103)) AND YEAR(FLD108) =YEAR(CONVERT(DATETIME,'{0}',103))  GROUP BY DATENAME(WEEKDAY,FLD108),FLD108 ORDER BY FLD108 ", reqDate)
            End If

            'So we have a valid query by now lets execute and draw the chart
            ' Now Work on the Area Chart
            Dim dt As DataTable = clsDataLayer.ReturnDataTable(queryReport)
            Dim json As String = "[['Day of Week','Total Revenue'],"


            For Each rw As DataRow In dt.Rows
                json += String.Format("['{0}',{1}],", rw("WeekDay").ToString, rw("TotalSales").ToString)
            Next
            Dim charsToTrim() As Char = {","c}
            Dim jsonF As String = json.TrimEnd(charsToTrim)
            jsonF += "]"

            Dim mys As StringBuilder = New StringBuilder()
            'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawAreaChart);")
            mys.AppendLine("function drawAreaChart() {")
            mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
            mys.AppendLine("var options = {title:  'Weekly Sales Trend',hAxis: { title: 'Day of the Week', titleTextStyle: { color: 'red'} }};")
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

        Page.Title = "Weekly Revenue Report"
    End Sub
End Class
