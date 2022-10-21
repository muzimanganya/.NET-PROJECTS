Imports System.Data.SqlClient
Imports System.Data

Partial Class POSTicketing
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
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

        Dim FIB As String = "2695"
        Dim RWF As String = "3.9"

        If String.IsNullOrEmpty(reqDate) Then
            'No Request Date so take for today
            query = String.Format("SELECT COUNT(IDRELATION) AS 'TotalTickets',ISNULL(FLD123,'-') AS 'POS',SUM(TOTAL) as 'TotalRevenue'" &
                " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                        ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                        ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                        ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END)+(SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) * {0}) +(SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END)*{1}) AS 'TotalAll'" &
                    " FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())" &
                            "GROUP BY su.SALES_PROD.FLD123 ORDER BY 'TotalRevenue' DESC", RWF, FIB)

            'queryTotal = "SELECT SUM(TOTAL) AS 'Total' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE()) ORDER BY 'TOTAL' DESC"

            queryTotal = "SELECT CONVERT(VARCHAR(10),DATE_MOD,105) AS 'TDATE'" &
                            ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalRWF'" &
                            ",SUM(CASE WHEN FLD118 = 'UGX' THEN Total ELSE 0 END) as 'TotalUGX'" &
                            ",SUM(CASE WHEN FLD118 = 'FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                            " FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) " &
                            " AND YEAR(DATE_MOD) = YEAR(GETDATE())  GROUP BY CONVERT(VARCHAR(10),DATE_MOD,105)"

            headerDate.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString
        Else
            'Date Defined
            Try
                'Dim exactDate As DateTime = DateTime.Parse(reqDate)
                Dim exactDate = reqDate
                headerDate.Text = ": " + reqDate
                'No Error so lets continue
                query = String.Format("SELECT COUNT(IDRELATION) AS 'TotalTickets',ISNULL(FLD123,'-') AS 'POS',SUM(TOTAL) as 'TotalRevenue'" &
                                      " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                                      ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalUGX'" &
                        ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                        ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                        ",SUM(CASE WHEN FLD118 = 'UGX' OR FLD118 IS NULL THEN Total ELSE 0 END)+(SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) * {1}) +(SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END)*{2}) AS 'TotalAll'" &
                                " FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103)) GROUP BY su.SALES_PROD.FLD123 ORDER BY 'TotalRevenue' DESC", exactDate, RWF, FIB)

                'queryTotal = String.Format("SELECT SUM(TOTAL) AS 'Total' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103)) ORDER BY 'Total' DESC", exactDate)


                queryTotal = String.Format("SELECT SUM(TOTAL) AS 'Total',CONVERT(VARCHAR(10),DATE_MOD,105) AS 'TDATE' " &
                                          ",SUM(CASE WHEN FLD118 = 'RWF' OR FLD118 IS NULL THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='UGX' THEN Total ELSE 0 END) as 'TotalUGX',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                          "FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103))" &
                                          " AND MONTH(su.SALES_PROD.DATE_MOD) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(su.SALES_PROD.DATE_MOD) = YEAR(CONVERT(DATETIME,'{0}',103)) GROUP BY CONVERT(VARCHAR(10),DATE_MOD,105)", exactDate)
                headerDate.Text = "For: " + reqDate
            Catch
            End Try
        End If


        Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)
        rptMasterView.DataSource = dt
        rptMasterView.DataBind()

        'Calculate the Total Sales Today

        Dim dtT As DataTable = clsDataLayer.ReturnDataTable(queryTotal)
        rptSummary.DataSource = dtT
        rptSummary.DataBind()

        Try
            ' Now Work on the Pie Chart
            Dim json As String = "[['POS Number','Total Revenue Today'],"

            For Each rw As DataRow In dt.Rows
                json += String.Format("['{0}',{1}],", rw("POS").ToString, rw("TotalAll").ToString)
            Next
            Dim charsToTrim() As Char = {","c}
            Dim jsonF As String = json.TrimEnd(charsToTrim)
            jsonF += "]"

            Dim mys As StringBuilder = New StringBuilder()
            mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
            mys.AppendLine("function drawChart() {")
            mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
            mys.AppendLine("var options = {title: 'Total Revenue Per POS', legend: {position:'none'}};")
            mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('piechart'));")
            mys.AppendLine("chart.draw(data, options);")
            mys.AppendLine("}")

            Dim script As String = mys.ToString()
            Dim csType As Type = Me.[GetType]()
            Dim cs As ClientScriptManager = Page.ClientScript
            cs.RegisterClientScriptBlock(csType, "pie", script, True)
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try


        Page.Title = "Per POS Ticketing"
    End Sub
End Class
