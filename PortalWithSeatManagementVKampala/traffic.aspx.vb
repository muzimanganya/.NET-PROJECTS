Imports System.Data.SqlClient
Imports System.Data

Partial Class traffic
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Try
        If Not Page.IsPostBack Then
            Dim reqDate As String = ""
            Dim pos As String = ""
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
            Dim queryTotalForDay As String = ""

            'Define the Query for Selecting Bus Name and Bus Details
            'Check if the POS is empty and redirect to POS Ticketing Page

            Dim FIB As String = Session("FIB")
            Dim RWF As String = Session("RWF")
            If String.IsNullOrEmpty(reqDate) Then
                'No Request Date so take for today
                query = String.Format("SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                            " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'," &
                            "SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                            ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                            ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                            ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                            ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END)+(SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) * {0}) +(SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END)*{1}) AS 'TotalAll'" &
                            " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                                    " WHERE DAY(su.SALES_PROD.FLD108) = DAY(GETDATE()) AND MONTH(su.SALES_PROD.FLD108) = MONTH(GETDATE()) AND YEAR(su.SALES_PROD.FLD108) = YEAR(GETDATE()) GROUP BY FLD120+'-'+FLD122" &
                                    " ORDER BY 'TotalRevenue' DESC", FIB, RWF)

                queryTotal = "SELECT CONVERT(VARCHAR(10),DATE_MOD,105) AS 'TDATE'" &
                                ",SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                                ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                ",SUM(CASE WHEN FLD118 ='UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                                " FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) " &
                                " AND YEAR(DATE_MOD) = YEAR(GETDATE())  GROUP BY CONVERT(VARCHAR(10),DATE_MOD,105)"

                queryTotalForDay = "SELECT CONVERT(VARCHAR(10),FLD108,105) AS 'TDATE'" &
                                ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                                ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                                ",COUNT(IDRELATION) AS 'TotalTickets'" &
                                    ",SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal'" &
                                    ",SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'" &
                                    ",SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking' " &
                                " FROM su.SALES_PROD WHERE DAY(FLD108) = DAY(GETDATE()) AND MONTH(FLD108) = MONTH(GETDATE())" &
                                " AND YEAR(FLD108) = YEAR(GETDATE()) GROUP BY CONVERT(VARCHAR(10),FLD108,105)"

                headerDate.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString
                'ltrToday.Text = ""
                For Each item As RepeaterItem In rptSummary.Items
                    Dim txtName As Literal = DirectCast(item.FindControl("ltrToday"), Literal)
                    'do something with txtName.Text
                    If txtName IsNot Nothing Then
                        txtName.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString
                    End If
                Next


            Else
                'Date Defined
                Try
                    'Dim exactDate As DateTime = DateTime.Parse(reqDate)
                    Dim exactDate = reqDate
                    headerDate.Text = ": " + reqDate
                    'No Error so lets continue
                    query = String.Format("SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                                          " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                            ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                            ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                            ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                            ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END)+(SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) * {1}) +(SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END)*{2}) AS 'TotalAll'" &
                            " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                                    " WHERE DAY(su.SALES_PROD.FLD108) = DAY(CONVERT(DATETIME,'{0}',103)) " &
                                        "AND MONTH(su.SALES_PROD.FLD108) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.FLD108) = YEAR(CONVERT(DATETIME,'{0}',103))" &
                                            " GROUP BY FLD120+'-'+FLD122  ORDER BY 'TotalRevenue' DESC", exactDate, FIB, RWF)

                    queryTotal = String.Format("SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),DATE_MOD,105) AS 'TDATE' " &
                                               ",SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB',SUM(CASE WHEN FLD118 ='UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                                               "FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103))" &
                                               " AND MONTH(su.SALES_PROD.DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103)) GROUP BY CONVERT(VARCHAR(10),DATE_MOD,105)", exactDate)

                    queryTotalForDay = String.Format("SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),FLD108,105) AS 'TDATE' " &
                                                     ",SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB',SUM(CASE WHEN FLD118 ='UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                                    ",COUNT(IDRELATION) AS 'TotalTickets'" &
                                    ",SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal'" &
                                    ",SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'" &
                                    ",SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking' " &
                                    " FROM su.SALES_PROD WHERE DAY(FLD108) = DAY(CONVERT(DATETIME,'{0}',103))" &
                                               " AND MONTH(su.SALES_PROD.FLD108) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.FLD108) = YEAR(CONVERT(DATETIME,'{0}',103)) GROUP BY CONVERT(VARCHAR(10),FLD108,105)", exactDate)
                Catch
                End Try
            End If

            Dim ds As New DataSet
            Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)
            ds.Tables.Add(dt)
            rptMasterView.DataSource = ds
            rptMasterView.DataBind()

            'Calculate the Total Sales Today

            Dim dtT As DataTable = clsDataLayer.ReturnDataTable(queryTotal)
            rptSummary.DataSource = dtT
            rptSummary.DataBind()

            'Calculate the Total Sales For Day

            Dim dtTf = clsDataLayer.ReturnDataTable(queryTotalForDay)
            Dim dtTT = dtTf
            rptTotalForDay.DataSource = dtTf
            rptTotalForDay.DataBind()

            'Bind the Summary Row
            rptTotals.DataSource = dtTT
            rptTotals.DataBind()

            ' Now Work on the Pie Chart
            Dim json As String = "[['Bus Route','Total Revenue Today'],"

            For Each rw As DataRow In dt.Rows
                json += String.Format("['{0}',{1}],", rw("BusRoute").ToString, rw("TotalAll").ToString)
            Next
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
        End If

        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try


        Page.Title = "Traffic & Business Overview"
    End Sub


    Protected Sub tmrUpdateRecords_Tick(sender As Object, e As EventArgs) Handles tmrUpdateRecords.Tick

        Try

            Dim reqDate As String = ""
            Try
                reqDate = Request.QueryString("requestDate")

            Catch ex As Exception
                'The Req Date was Not Sent
                reqDate = ""
            End Try
            Dim queryTotal, queryTotalForDay As String
            If String.IsNullOrEmpty(reqDate) Then
                'queryTotal = "SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),DATE_MOD,105) AS 'TDATE' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())  GROUP BY CONVERT(VARCHAR(10),DATE_MOD,105) ORDER BY 'TOTAL' DESC"
                'queryTotalForDay = "SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),FLD108,105) AS 'TDATE' FROM su.SALES_PROD WHERE DAY(FLD108) = DAY(GETDATE()) AND MONTH(FLD108) = MONTH(GETDATE()) AND YEAR(FLD108) = YEAR(GETDATE()) GROUP BY CONVERT(VARCHAR(10),FLD108,105) ORDER BY 'TOTAL' DESC"
                queryTotal = "SELECT CONVERT(VARCHAR(10),DATE_MOD,105) AS 'TDATE'" &
                                ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalRWF'" &
                                ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                ",SUM(CASE WHEN FLD118 ='UGX' THEN Total ELSE 0 END) as 'TotalUGX'" &
                                " FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) " &
                                " AND YEAR(DATE_MOD) = YEAR(GETDATE())  GROUP BY CONVERT(VARCHAR(10),DATE_MOD,105)"



                queryTotalForDay = "SELECT CONVERT(VARCHAR(10),FLD108,105) AS 'TDATE'" &
                                ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalRWF'" &
                                ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                ",SUM(CASE WHEN FLD118 ='UGX' THEN Total ELSE 0 END) as 'TotalUGX'" &
                                     ",COUNT(IDRELATION) AS 'TotalTickets'" &
                                            ",SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal'" &
                                            ",SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'" &
                                            ",SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking' " &
                                " FROM su.SALES_PROD WHERE DAY(FLD108) = DAY(GETDATE()) AND MONTH(FLD108) = MONTH(GETDATE())" &
                                " AND YEAR(FLD108) = YEAR(GETDATE()) GROUP BY CONVERT(VARCHAR(10),FLD108,105)"

            Else
                Dim exactDate = reqDate
                'queryTotal = String.Format("SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),DATE_MOD,105) AS 'TDATE' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103))" &
                '                               " AND MONTH(su.SALES_PROD.DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103)) GROUP BY CONVERT(VARCHAR(10),DATE_MOD,105) ORDER BY 'TOTAL' DESC", exactDate)

                'queryTotalForDay = String.Format("SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),FLD108,105) AS 'TDATE' FROM su.SALES_PROD WHERE DAY(FLD108) = DAY(CONVERT(DATETIME,'{0}',103))" &
                '                           " AND MONTH(su.SALES_PROD.FLD108) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.FLD108) = YEAR(CONVERT(DATETIME,'{0}',103)) GROUP BY CONVERT(VARCHAR(10),FLD108,105) ORDER BY 'TOTAL' DESC", exactDate)

                queryTotal = String.Format("SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),DATE_MOD,105) AS 'TDATE' " &
                                               ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB',SUM(CASE WHEN FLD118 ='UGX' THEN Total ELSE 0 END) as 'TotalUGX'" &
                                               "FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103))" &
                                               " AND MONTH(su.SALES_PROD.DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103)) GROUP BY CONVERT(VARCHAR(10),DATE_MOD,105) ", exactDate)

                queryTotalForDay = String.Format("SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),FLD108,105) AS 'TDATE' " &
                                                 ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB',SUM(CASE WHEN FLD118 ='UGX' THEN Total ELSE 0 END) as 'TotalUGX'" &
                                                ",COUNT(IDRELATION) AS 'TotalTickets'" &
                                            ",SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal'" &
                                            ",SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'" &
                                            ",SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking' " &
                                            " FROM su.SALES_PROD WHERE DAY(FLD108) = DAY(CONVERT(DATETIME,'{0}',103))" &
                                           " AND MONTH(su.SALES_PROD.FLD108) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.FLD108) = YEAR(CONVERT(DATETIME,'{0}',103)) GROUP BY CONVERT(VARCHAR(10),FLD108,105)", exactDate)
            End If
            'Calculate the Total Sales Today

            Dim dtT As DataTable = clsDataLayer.ReturnDataTable(queryTotal)
            rptSummary.DataSource = dtT
            rptSummary.DataBind()

            'Calculate the Total Sales For Day

            Dim dtTf = clsDataLayer.ReturnDataTable(queryTotalForDay)
            Dim dtTT = dtTf
            rptTotalForDay.DataSource = dtTf
            rptTotalForDay.DataBind()

            'Bind the Summary Row
            rptTotals.DataSource = dtTT
            rptTotals.DataBind()
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

    End Sub

    Protected Sub tmrMainView_Tick(sender As Object, e As EventArgs) Handles tmrMainView.Tick
        Try
            Dim reqDate As String = ""
            Dim pos As String = ""
            Try
                reqDate = Request.QueryString("requestDate")

            Catch ex As Exception
                'The Req Date was Not Sent
                reqDate = ""
            End Try
            'Define the Queries
            Dim query As String = ""

            Dim FIB As String = 700 ' Session("FIB")
            Dim RWF As String = 0.26 'Session("RWF")

            If String.IsNullOrEmpty(reqDate) Then
                'No Request Date so take for today
                'query = "SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                '            " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                '            " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                '                    " WHERE DAY(su.SALES_PROD.FLD108) = DAY(GETDATE()) AND MONTH(su.SALES_PROD.FLD108) = MONTH(GETDATE()) AND YEAR(su.SALES_PROD.FLD108) = YEAR(GETDATE()) GROUP BY FLD120+'-'+FLD122" &
                '                    " ORDER BY 'TotalRevenue' DESC"
                'query = "SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                '          " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'," &
                '          "SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                '          ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalRWF'" &
                '          ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                '          ",SUM(CASE WHEN FLD118 ='USD' THEN Total ELSE 0 END) as 'TotalUSD'" &
                '          " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                '                  " WHERE DAY(su.SALES_PROD.FLD108) = DAY(GETDATE()) AND MONTH(su.SALES_PROD.FLD108) = MONTH(GETDATE()) AND YEAR(su.SALES_PROD.FLD108) = YEAR(GETDATE()) GROUP BY FLD120+'-'+FLD122" &
                '                  " ORDER BY 'TotalRevenue' DESC"

                query = String.Format("SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                            " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'," &
                            "SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                            ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                            ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                            ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                            ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END)+(SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) * {0}) +(SUM(CASE WHEN FLD118 ='UGX' THEN Total ELSE 0 END)*{1}) AS 'TotalAll'" &
                            " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                                    " WHERE DAY(su.SALES_PROD.FLD108) = DAY(GETDATE()) AND MONTH(su.SALES_PROD.FLD108) = MONTH(GETDATE()) AND YEAR(su.SALES_PROD.FLD108) = YEAR(GETDATE()) GROUP BY FLD120+'-'+FLD122" &
                                    " ORDER BY 'TotalRevenue' DESC", FIB, RWF)

            Else
                'Date Defined
                Try
                    'Dim exactDate As DateTime = DateTime.Parse(reqDate)
                    Dim exactDate = reqDate
                    headerDate.Text = ": " + reqDate
                    'No Error so lets continue
                    'query = String.Format("SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                    '                      " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                    '        " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                    '                " WHERE DAY(su.SALES_PROD.FLD108) = DAY(CONVERT(DATETIME,'{0}',103)) " &
                    '                    "AND MONTH(su.SALES_PROD.FLD108) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.FLD108) = YEAR(CONVERT(DATETIME,'{0}',103))" &
                    '                        " GROUP BY FLD120+'-'+FLD122  ORDER BY 'TotalRevenue' DESC", exactDate)

                    'query = String.Format("SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                    '                     " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                    '       ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalRWF'" &
                    '       ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                    '       ",SUM(CASE WHEN FLD118 ='UGX' THEN Total ELSE 0 END) as 'TotalUGX'" &
                    '       " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                    '               " WHERE DAY(su.SALES_PROD.FLD108) = DAY(CONVERT(DATETIME,'{0}',103)) " &
                    '                   "AND MONTH(su.SALES_PROD.FLD108) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.FLD108) = YEAR(CONVERT(DATETIME,'{0}',103))" &
                    '                       " GROUP BY FLD120+'-'+FLD122  ORDER BY 'TotalRevenue' DESC", exactDate)
                    query = String.Format("SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                                          " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                            ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                            ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                            ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                            ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END)+(SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) * {1}) +(SUM(CASE WHEN FLD118 ='UGX' THEN Total ELSE 0 END)*{2}) AS 'TotalAll'" &
                            " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                                    " WHERE DAY(su.SALES_PROD.FLD108) = DAY(CONVERT(DATETIME,'{0}',103)) " &
                                        "AND MONTH(su.SALES_PROD.FLD108) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.FLD108) = YEAR(CONVERT(DATETIME,'{0}',103))" &
                                            " GROUP BY FLD120+'-'+FLD122  ORDER BY 'TotalRevenue' DESC", exactDate, FIB, RWF)
                Catch ex As Exception
                    clsDataLayer.LogException(ex)
                End Try
            End If

            Dim ds As New DataSet
            Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)
            ds.Tables.Add(dt)
            rptMasterView.DataSource = ds
            rptMasterView.DataBind()

            ' Now Work on the Pie Chart
            Dim json As String = "[['Bus Route','Total Revenue Today'],"

            For Each rw As DataRow In dt.Rows
                json += String.Format("['{0}',{1}],", rw("BusRoute").ToString, rw("TotalAll").ToString)
            Next
            Dim charsToTrim() As Char = {","c}
            Dim jsonF As String = json.TrimEnd(charsToTrim)
            jsonF += "]"

            Dim mys As StringBuilder = New StringBuilder()
            '        mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
            mys.AppendLine("function drawChart() {")
            mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
            mys.AppendLine("var options = {title: 'Total Revenue Per Route',pieHole: 0.4,legend: {position: 'none'}};")
            mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('piechart'));")
            mys.AppendLine("chart.draw(data, options);")
            mys.AppendLine("}drawChart();")

            Dim script As String = mys.ToString()
            Dim csType As Type = Me.[GetType]()
            Dim cs As ClientScriptManager = Page.ClientScript
            cs.RegisterClientScriptBlock(csType, "pieupdate", script, True)

        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try
    End Sub
End Class
