Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Check that the company has been set else redirect to landing page make sure they are not logged in
            If Not Request.IsAuthenticated Then
                'Redirect them to the site landing page
                Dim redUrl As String = "http://www.innovys.co.rw/index.php/en/customer-login.html"
                Response.Redirect(redUrl)
            End If

        End If
        If User.IsInRole("restricted") Then
            Response.Redirect("/All-Days--Planned-Traffic.aspx")
        End If

        Try
            Dim script As String = ""

            'Access Rights to Charts
            With HttpContext.Current
                If .User.IsInRole("Admin") Or .User.IsInRole("Manager") Then
                    'Send them the charts, otherwise send an error message

                    Dim query As String = "SET DATEFIRST 1;SELECT DATENAME(WEEKDAY,FLD108) AS 'Weekday',SUM(Total) AS 'TotalSales',SUM(CASE WHEN FLD118='RWF' OR FLD118 IS NULL THEN TOTAL ELSE 0 END) AS 'TotalRWF',SUM(CASE WHEN FLD118='FIB' THEN TOTAL ELSE 0 END) AS 'TotalFIB' FROM su.SALES_PROD WHERE DatePart(WeeK, FLD108) = DatePart(week, GETDATE()) AND YEAR(FLD108) =YEAR(GETDATE())  GROUP BY DATENAME(WEEKDAY,FLD108),FLD108 ORDER BY FLD108 "
                    Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)

                    ' Now Work on the Pie Chart
                    Dim json As String = "[['Weekday','Daily Revenue RWF','Daily Revenue FIB'],"


                    For Each rw As DataRow In dt.Rows
                        json += String.Format("['{0}',{1},{2}],", rw("Weekday").ToString, rw("TotalRWF").ToString, rw("TotalFIB").ToString)
                    Next
                    Dim charsToTrim() As Char = {","c}
                    Dim jsonF As String = json.TrimEnd(charsToTrim)
                    jsonF += "]"

                    Dim mys As StringBuilder = New StringBuilder()
                    'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawAreaChart);")
                    mys.AppendLine("function drawAreaChart() {")
                    mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                    mys.AppendLine("console.log('Called the Area Chart!!');var options = {title:  'Weekly Sales Trend',hAxis: { title: 'Day of the Week', titleTextStyle: { color: 'red'} }};")
                    mys.AppendLine("var chart = new google.visualization.AreaChart(document.getElementById('chart_div'));")
                    mys.AppendLine("chart.draw(data, options);")
                    mys.AppendLine("}")

                    Dim AreaScript As String = mys.ToString()


                    'Draw Tigo Cash Graph
                    query = "SET DATEFIRST 1;SELECT DATENAME(WEEKDAY,FLD108) AS 'Weekday',SUM(Total) AS 'TotalSales' FROM su.SALES_PROD " +
                        "WHERE DatePart(WeeK, FLD108) = DatePart(week, GETDATE()) AND YEAR(FLD108) =YEAR(GETDATE()) AND FLD123='TigoCash'   GROUP BY DATENAME(WEEKDAY,FLD108),FLD108 " +
                        " ORDER BY FLD108 "
                    Dim dtTigoCash As DataTable = clsDataLayer.ReturnDataTable(query)

                    json = "[['Day of Week','Tigo Cash Revenue'],"

                    For Each rw As DataRow In dtTigoCash.Rows
                        json += String.Format("['{0}',{1}],", rw("Weekday").ToString, rw("TotalSales").ToString)
                    Next

                    jsonF = json.TrimEnd(charsToTrim)
                    jsonF += "]"

                    mys = New StringBuilder()

                    mys.AppendLine("function drawTigoCashChart() {")
                    mys.AppendLine(String.Format("var dataT = google.visualization.arrayToDataTable({0})", jsonF))
                    mys.AppendLine("var optionsT = {title:  'Tigo Cash Sales Trend',width: 640,height:360,hAxis: { title: 'Day of the Week', titleTextStyle: { color: 'red'} }};")
                    mys.AppendLine("var chartT = new google.visualization.AreaChart(document.getElementById('Div5'));")
                    mys.AppendLine("chartT.draw(dataT, optionsT);")
                    mys.AppendLine("}")

                    Dim tigoChart = mys.ToString()

                    'Now Draw the Best Sellers Pie Chart
                    query = "SELECT TOP(5) COUNT(IDRELATION) AS 'TotalTickets',ISNULL(FLD123,'-')  AS 'POS',SUM(TOTAL) as 'TotalRevenue'" &
                                ",ROUND(SUM(CASE WHEN FLD118='RWF' OR FLD118 IS NULL THEN TOTAL ELSE 0 END) + (SUM(CASE WHEN FLD118='FIB' THEN TOTAL ELSE 0 END) /2.2),0) AS 'TotalRight'" &
                                "FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE()) GROUP BY su.SALES_PROD.FLD123 ORDER BY 'TotalRight' DESC"
                    Dim dtPieChart As DataTable = clsDataLayer.ReturnDataTable(query)

                    json = "[['POS Number','Total Revenue Today'],"

                    For Each rw As DataRow In dtPieChart.Rows
                        json += String.Format("['{0}',{1}],", rw("POS").ToString, rw("TotalRight").ToString)
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

                        query = "SELECT TOP(5) sp.FLD120+'-'+sp.FLD122 AS 'BusRoute',COUNT(sp.IDRELATION) AS 'TotalTickets',SUM(sp.PRICE) AS 'TotalRevenue'," &
                            "ROUND(SUM(CASE WHEN FLD118='RWF' OR FLD118 IS NULL THEN sp.PRICE ELSE 0 END) + (SUM(CASE WHEN FLD118='FIB' THEN sp.PRICE ELSE 0 END) /2.2),0) AS 'TotalRight' " &
                                       "FROM su.SALES_PROD as sp INNER JOIN su.SALES as s ON sp.IDSALE = s.IDSALE" &
                                                " WHERE DAY(sp.FLD108) = DAY(GETDATE()) AND MONTH(sp.FLD108) = MONTH(GETDATE()) AND YEAR(sp.FLD108)= YEAR(GETDATE()) GROUP BY FLD120+'-'+FLD122" &
                                                    " ORDER BY 'TotalRight' DESC"
                        Dim dtBestRoute As DataTable = clsDataLayer.ReturnDataTable(query)
                        json = "[['Bus Route','Total Revenue Today'],"

                        For Each rw As DataRow In dtBestRoute.Rows
                            json += String.Format("['{0}',{1}],", rw("BusRoute").ToString, rw("TotalRight").ToString)
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

                        Dim initScript As String = "function initCharts () {drawAreaChart();drawTigoCashChart();drawPieChart();drawBestChart();drawBestTChart();}" +
                                            "google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(initCharts);"

                        script = AreaScript + pieScript + BestScript + BestTScript + tigoChart + initScript

                    Catch ex As Exception
                        clsDataLayer.LogException(ex)
                    End Try

                    
                Else
                    Dim errorHtml As String = String.Format("<div style=width:60%;margin:auto;padding-top:45px;padding-bottom:45px;padding-left:15px;padding-right:15px;><b>Chart is not Accessible to Users in Your Role Group.</b><br/> <p>Please Consult your Administrator to Get Viewing Rights</p></div>")

                    script = "$(function() {$('#chart_div').html('" + errorHtml + "');$('#piechart').html('" + errorHtml + "');$('#bestTChart').html('" + errorHtml + "');$('#bestchart').html('" + errorHtml + "')})"
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
