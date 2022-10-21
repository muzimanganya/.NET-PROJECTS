Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization

Partial Class BestRoutePerMonth
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
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
            queryReport = "SELECT TOP(5) FLD120+'-'+FLD122 AS 'Route' ,SUM(Total) AS 'TotalSales' " &
                            "FROM su.SALES_PROD WHERE MONTH(FLD108) =MONTH(GETDATE())  " &
                                "GROUP BY FLD120+'-'+FLD122 ORDER BY 'TotalSales' DESC"
        Else
            'They passed a valid date
            queryReport = String.Format("SELECT TOP(5) FLD120+'-'+FLD122 AS 'Route' ,SUM(Total) AS 'TotalSales' " &
                "FROM su.SALES_PROD WHERE MONTH(FLD108) =MONTH(CONVERT(DATETIME,'{0}',103))  " &
                    "GROUP BY FLD120+'-'+FLD122 ORDER BY 'TotalSales' DESC", reqDate)

        End If

        'So we have a valid query by now lets execute and draw the chart
        ' Now Work on the Area Chart
        Dim dt As DataTable = clsDataLayer.ReturnDataTable(queryReport)
        Dim json As String = "[['Route','Total Revenue Today'],"


        For Each rw As DataRow In dt.Rows
            json += String.Format("['{0}',{1}],", rw("Route").ToString, rw("TotalSales").ToString)
        Next
        Dim charsToTrim() As Char = {","c}
        Dim jsonF As String = json.TrimEnd(charsToTrim)
        jsonF += "]"

        Dim mys As StringBuilder = New StringBuilder()
        mys.AppendLine("function drawRoutePieChart() {")
        mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
        mys.AppendLine("var options = {title: 'Total Revenue For Route',is3D: true, legend: {position:'none'}};")
        mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('route_pie_chart'));")
        mys.AppendLine("chart.draw(data, options);")
        mys.AppendLine("}")

        Dim AreaScript As String = mys.ToString()

        'Here let us build Weekly Trend for all the routes per day
        Dim busRoutes(7, 6) As String
        Dim count As Integer = 1
        Dim chartJson As String = "[['Day of Week',"
        busRoutes(0, 0) = "Monday"
        busRoutes(1, 0) = "Teusday"
        busRoutes(2, 0) = "Wednesday"
        busRoutes(3, 0) = "Thursday"
        busRoutes(4, 0) = "Friday"
        busRoutes(5, 0) = "Saturday"
        busRoutes(6, 0) = "Sunday"

        For Each rww As DataRow In dt.Rows
            Dim routeName As String = rww("Route").ToString()
            'Add the Routename to the Json string
            chartJson += "'" + routeName + "',"
            Dim queryRouteWeek As String = ""
            If String.IsNullOrEmpty(reqDate) Then
                queryRouteWeek = String.Format("SET DATEFIRST 1;SELECT  FLD120+'-'+FLD122 AS 'Route',DateName(Weekday,FLD108) AS 'Day' ,COUNT(IDRELATION) AS 'TotalSales'" &
                                    "FROM su.SALES_PROD WHERE DATEPART(Week,FLD108) =DATEPART(Week,GETDATE()) " &
                                        "AND YEAR(FLD108) = YEAR(GETDATE()) AND FLD120+'-'+FLD122 = '{0}' " &
                                            "GROUP BY DateName(Weekday,FLD108),FLD120+'-'+FLD122,DATEPART(dw,FLD108) " &
                                                "ORDER BY DATEPART(dw,FLD108)", routeName)
            Else
                queryRouteWeek = String.Format("SET DATEFIRST 1;SELECT  FLD120+'-'+FLD122 AS 'Route',DateName(Weekday,FLD108) AS 'Day' ,COUNT(IDRELATION) AS 'TotalSales'" &
                                   "FROM su.SALES_PROD WHERE DATEPART(Week,FLD108) =DATEPART(Week,CONVERT(DATETIME,'{1}',103)) " &
                                       "AND YEAR(FLD108) = YEAR(CONVERT(DATETIME,'{1}',103)) AND FLD120+'-'+FLD122 = '{0}' " &
                                           "GROUP BY DateName(Weekday,FLD108),FLD120+'-'+FLD122,DATEPART(dw,FLD108) " &
                                               "ORDER BY DATEPART(dw,FLD108)", routeName, reqDate)
            End If

            Dim routeDtWk As DataTable = clsDataLayer.ReturnDataTable(queryRouteWeek)
            Dim rows As Integer = 0
            For Each irw As DataRow In routeDtWk.Rows
                busRoutes(rows, count) = irw("TotalSales").ToString
                rows += 1
            Next
            count += 1
        Next
        chartJson = chartJson.Trim(","c)
        chartJson += "],"

        'Now the JSON Has the Header Continue to Add the Body
        For countRows As Integer = 0 To 6 Step 1
            'Iterating the Rows of the Array
            'Now the columns
            chartJson += "["
            For countColumns As Integer = 0 To 5
                If countColumns = 0 Then
                    'THis is the first column we need to qoute it
                    chartJson += "'" + busRoutes(countRows, countColumns) + "'"
                    chartJson += ","
                Else
                    If String.IsNullOrEmpty(busRoutes(countRows, countColumns)) Then
                        chartJson += "0"
                        chartJson += ","
                    Else
                        chartJson += busRoutes(countRows, countColumns)
                        chartJson += ","
                    End If

                End If

            Next
            chartJson = chartJson.Trim(","c)
            chartJson += "],"
        Next
        chartJson = chartJson.Trim(","c)
        chartJson += "]"

        mys = New StringBuilder()
        mys.AppendLine("function drawLineChart() {")
        mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", chartJson))
        mys.AppendLine("var options = {title:  'Weekly Travellers Trend',chartArea:{left:40,top:10,width:'70%'},hAxis: { title: 'Day of the Week', titleTextStyle: { color: 'red'} }};")
        mys.AppendLine("var chart = new google.visualization.LineChart(document.getElementById('week_chart'));")
        mys.AppendLine("chart.draw(data, options);")
        mys.AppendLine("}")


        Dim lineScript As String = mys.ToString()
        Dim initScript As String = "function initCharts () {drawRoutePieChart();drawLineChart();}" +
                            "google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(initCharts);"

        Dim script As String = AreaScript + lineScript + initScript

        Dim csType As Type = Me.[GetType]()
        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterClientScriptBlock(csType, "charts", script, True)
        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try


        Page.Title = "Per Route Statistics Report"
    End Sub
End Class
