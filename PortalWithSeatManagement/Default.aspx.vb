Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim script As String = ""
            If User.IsInRole("restricted") Then
                Response.Redirect("/All-Days--Planned-Traffic.aspx")
            End If
            'Access Rights to Charts
            With HttpContext.Current
                If .User.IsInRole("Admin") Or .User.IsInRole("Manager") Then
                    'Send them the charts, otherwise send an error message
                    Dim FIB As String = Session("FIB")
                    Dim RWF As String = Session("RWF")

                    Try

                    Catch ex As Exception

                    End Try
                    Dim query As String =
                        String.Format("SET DATEFIRST 1;SELECT DATENAME(WEEKDAY,FLD108) AS 'Weekday',SUM(Total) AS 'TotalSalesOld', " &
                                      " SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) AS 'TotalUGX'," &
                                      " SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) AS 'TotalRWF',SUM(CASE WHEN FLD118 ='USD' THEN Total ELSE 0 END) AS 'TotalUSD'" &
                        " FROM su.SALES_PROD WHERE DatePart(WeeK, FLD108) = DatePart(week, GETDATE()) " &
                        " AND YEAR(FLD108) =YEAR(GETDATE())  GROUP BY DATENAME(WEEKDAY,FLD108),FLD108 ORDER BY FLD108 ", 0.2662, RWF)

                    Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)

                    ' Now Work on the Pie Chart
                    Dim json As String = "[['Weekday','RWF','UGX','USD'],"

                    Try
                        For Each rw As DataRow In dt.Rows
                            json += String.Format("['{0}',{1},{2},{3}],", rw("Weekday").ToString, rw("TotalRWF").ToString, rw("TotalUGX").ToString, rw("TotalUSD").ToString)
                        Next
                    Catch ex As Exception
                        clsDataLayer.LogException(ex)
                    End Try

                    
                    Dim charsToTrim() As Char = {","c}
                    Dim jsonF As String = json.TrimEnd(charsToTrim)
                    jsonF += "]"

                    Dim mys As StringBuilder = New StringBuilder()
                    'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawAreaChart);")
                    mys.AppendLine("function drawAreaChart() {")
                    mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                    mys.AppendLine("var options = {title:  'Weekly Sales Trend',hAxis: { title: 'Day of the Week', titleTextStyle: { color: 'red'} }};")
                    mys.AppendLine("var chart = new google.visualization.AreaChart(document.getElementById('chart_div'));")
                    mys.AppendLine("chart.draw(data, options);")
                    mys.AppendLine("}")

                    Dim AreaScript As String = mys.ToString()


                    'Now Draw the Best Sellers Pie Chart
                    query = String.Format("SELECT TOP(5) COUNT(IDRELATION) AS 'TotalTickets',ISNULL(FLD123,'-')  AS 'POS',SUM(TOTAL) as 'TotalRevenue2',SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END)*0.2662+(SUM(CASE WHEN FLD118 ='bwf' THEN Total ELSE 0 END) * {0}) +(SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END)) AS 'TotalRevenue'" &
                                "FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE()) GROUP BY su.SALES_PROD.FLD123 ORDER BY 'TotalRevenue' DESC", FIB, RWF)
                    Dim dtPieChart As DataTable = clsDataLayer.ReturnDataTable(query)

                    json = "[['POS Number','Total Revenue Today'],"

                    For Each rw As DataRow In dtPieChart.Rows
                        json += String.Format("['{0}',{1}],", rw("POS").ToString, rw("TotalRevenue").ToString)
                    Next

                    jsonF = json.TrimEnd(charsToTrim)
                    jsonF += "]"

                    mys = New StringBuilder()
                    'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawPieChart);")
                    mys.AppendLine("function drawPieChart() {")
                    mys.AppendLine(String.Format("console.log(google);var data = google.visualization.arrayToDataTable({0})", jsonF))
                    mys.AppendLine("var options = {title: 'Top 5 POS Sales',is3D:true,'width':640,'height':360};")
                    mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('piechart'));")
                    mys.AppendLine("chart.draw(data, options);")
                    mys.AppendLine("}")

                    Dim pieScript = mys.ToString()

                    Try
                        'Now Add the Best Route Pie Chart

                        query = String.Format("SELECT TOP(5) sp.FLD120+'-'+sp.FLD122 AS 'BusRoute',COUNT(sp.FLD123) AS 'TotalPOS',COUNT(sp.IDRELATION) AS 'TotalTickets',SUM(sp.PRICE) AS 'TotalPrice',SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END)*0.2662+(SUM(CASE WHEN FLD118 ='bwf' THEN Total ELSE 0 END) * {0}) +(SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END)) AS 'TotalRevenue'" &
                                       "FROM su.SALES_PROD as sp INNER JOIN su.SALES as s ON sp.IDSALE = s.IDSALE" &
                                                " WHERE DAY(sp.FLD108) = DAY(GETDATE()) AND MONTH(sp.FLD108) = MONTH(GETDATE()) AND YEAR(sp.FLD108)= YEAR(GETDATE()) GROUP BY FLD120+'-'+FLD122" &
                                                    " ORDER BY 'TotalRevenue' DESC", FIB, RWF)
                        Dim dtBestRoute As DataTable = clsDataLayer.ReturnDataTable(query)
                        json = "[['Bus Route','Total Revenue Today'],"

                        For Each rw As DataRow In dtBestRoute.Rows
                            json += String.Format("['{0}',{1}],", rw("BusRoute").ToString, rw("TotalRevenue").ToString)
                        Next
                        jsonF = json.TrimEnd(charsToTrim)
                        jsonF += "]"

                        mys = New StringBuilder()
                        'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawBestChart);")
                        mys.AppendLine("function drawBestChart() {")
                        mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                        mys.AppendLine("var options = {title: 'Total Revenue Per Route','width':640,'height':360,is3D:true};")
                        mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('bestchart'));")
                        mys.AppendLine("chart.draw(data, options);")
                        mys.AppendLine("}")

                        Dim BestScript = mys.ToString()

                        'Dim initScript As String = "function initCharts () {drawAreaChart();drawPieChart();drawBestChart();}" +
                        '                    "google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(initCharts);"

                        'script = AreaScript + pieScript + BestScript + initScript
                        'Now Work on the Per Route Travellers Piechart

                        Dim dv As DataView = dtBestRoute.DefaultView
                        dv.Sort = "TotalTickets DESC"

                        Dim dtS As DataTable = dv.ToTable()
                        json = "[['Bus Route','Total Travelers Today'],"

                        For Each rw As DataRow In dtS.Rows
                            json += String.Format("['{0}',{1}],", rw("BusRoute").ToString, rw("TotalTickets").ToString)
                        Next
                        jsonF = json.TrimEnd(charsToTrim)
                        jsonF += "]"

                        mys = New StringBuilder()
                        'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawBestChart);")
                        mys.AppendLine("function drawBestTChart() {")
                        mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                        mys.AppendLine("var options = {title: 'Total Travelers Per Route','width':640,'height':360,is3D:true};")
                        mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('bestTChart'));")
                        mys.AppendLine("chart.draw(data, options);")
                        mys.AppendLine("}")

                        Dim BestTScript = mys.ToString()

                        Dim initScript As String = "function initCharts () {drawAreaChart();drawPieChart();drawBestChart();drawBestTChart();}" +
                                            "google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(initCharts);"

                        script = AreaScript + pieScript + BestScript + BestTScript + initScript
                    Catch ex As Exception
                        clsDataLayer.LogException(ex)
                    End Try


                Else
                    Dim errorHtml As String = String.Format("<div style=width:60%;margin:auto;padding-top:45px;padding-bottom:45px;padding-left:15px;padding-right:15px;><b>Chart is not Accessible to Users in Your Role Group.</b><br/> <p>Please Consult your Administrator to Get Viewing Rights</p></div>")

                    script = "$(function() {$('#chart_div').html('" + errorHtml + "');$('#piechart').html('" + errorHtml + "');$('#bestchart').html('" + errorHtml + "')})"
                End If
            End With

            Dim csType As Type = Me.[GetType]()
            Dim cs As ClientScriptManager = Page.ClientScript
            cs.RegisterClientScriptBlock(csType, "charts", script, True)
        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


        Page.Title = "Sinnovys Portal | Home Page"
    End Sub


End Class
