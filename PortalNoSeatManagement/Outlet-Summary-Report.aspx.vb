Imports System.Data
Imports System.IO
Imports OfficeOpenXml

Partial Class Outlet_Summary_Report
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Public TotalNormal As Integer, TotalBooking As Integer, TotalPromo As Integer, TotalTickets As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Try
                Dim reqDate As String = ""
                Dim endDate As String = ""
                Dim pos As String = ""
                Dim outlet As String = ""
                Try
                    reqDate = Request.QueryString("requestDate")
                    endDate = Request.QueryString("endDate")
                    outlet = Request.QueryString("outlet")

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


                If String.IsNullOrEmpty(reqDate) Or String.IsNullOrEmpty(endDate) Or String.IsNullOrEmpty(outlet) Then
                    'Some parameter missing, take him to reports page
                    Response.Redirect("/QueryDB.aspx")

                Else
                    'Date Defined
                    Try
                        Dim outletName As String = clsDataLayer.ReturnSingleValue("SELECT NAME FROM Outlet WHERE ID=" + outlet)
                        headerDate.Text = outletName + ": " + reqDate + " - " + endDate
                        'No Error so lets continue
                        query = String.Format("SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                                          " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                                          ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                          ",ROUND(SUM(CASE WHEN FLD118='RWF' OR FLD118 IS NULL THEN TOTAL ELSE 0 END) + (SUM(CASE WHEN FLD118='FIB' THEN TOTAL ELSE 0 END) /2.2),0) AS 'TotalRight'" &
                        ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                            " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                                    " WHERE  CAST(su.SALES_PROD.FLD108 AS DATE) BETWEEN CONVERT(DATE,'{0}',103) AND DATEADD(d, 1, CONVERT(DATE,'{1}',103))  AND FLD117={2}" &
                                            " GROUP BY FLD120+'-'+FLD122  ORDER BY 'TotalRevenue' DESC", reqDate, endDate, outlet)

                        queryTotal = String.Format("SELECT SUM(TOTAL) AS 'Total','{0} - {1}'  AS 'TDATE',SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB' FROM su.SALES_PROD WHERE CAST(su.SALES_PROD.DATE_MOD AS DATE) BETWEEN CONVERT(DATE,'{0}',103) AND DATEADD(d, 1, CONVERT(DATE,'{1}',103)) AND FLD117={2} ", reqDate, endDate, outlet)

                        queryTotalForDay = String.Format("SELECT SUM(TOTAL) AS 'Total', '{0} - {1}' AS 'TDATE' " &
                                                ",COUNT(IDRELATION) AS 'TotalTickets'" &
                                                ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                                ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                                                ",SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal'" &
                                                ",SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'" &
                                                ",SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking' " &
                                                " FROM su.SALES_PROD WHERE CAST(su.SALES_PROD.FLD108 AS DATE) BETWEEN CONVERT(DATE,'{0}',103) AND DATEADD(d, 1, CONVERT(DATE,'{1}',103)) AND FLD117={2}", reqDate, endDate, outlet)

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
                    json += String.Format("['{0}',{1}],", rw("BusRoute").ToString, rw("TotalRight").ToString)
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
            Catch ex As Exception
                clsDataLayer.LogException(ex)
                Response.Redirect("/CustomError.aspx")
            End Try


            Page.Title = "Traffic & Business Overview"
        End If

    End Sub


    Protected Sub tmrUpdateRecords_Tick(sender As Object, e As EventArgs) Handles tmrUpdateRecords.Tick
        Try
            Dim reqDate As String = ""
            Dim endDate As String = ""
            Dim outlet As String = ""
            Try
                reqDate = Request.QueryString("requestDate")
                endDate = Request.QueryString("endDate")
                outlet = Request.QueryString("outlet")

            Catch ex As Exception
                'The Req Date was Not Sent
                reqDate = ""
            End Try
            Dim queryTotal, queryTotalForDay As String


            If String.IsNullOrEmpty(reqDate) Or String.IsNullOrEmpty(endDate) Or String.IsNullOrEmpty(outlet) Then
                'Some parameter missing, take him to reports page
                Response.Redirect("/QueryDB.aspx")

            Else

                queryTotal = String.Format("SELECT SUM(TOTAL) AS 'Total','{0} - {1}'  AS 'TDATE',SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB' FROM su.SALES_PROD WHERE CAST(su.SALES_PROD.DATE_MOD AS DATE) BETWEEN CONVERT(DATE,'{0}',103) AND CONVERT(DATE,'{1}',103)  AND FLD117={2}", reqDate, endDate, outlet)

                queryTotalForDay = String.Format("SELECT SUM(TOTAL) AS 'Total','{0} - {1}' AS 'TDATE' " &
                                                 ",COUNT(IDRELATION) AS 'TotalTickets'" &
                                                 ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                        ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                                            ",SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal'" &
                                            ",SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo'" &
                                            ",SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking' " &
                                                 " FROM su.SALES_PROD WHERE CAST(su.SALES_PROD.FLD108 AS DATE) BETWEEN CONVERT(DATE,'{0}',103) AND CONVERT(DATE,'{1}',103)  AND FLD117={2}", reqDate, endDate, outlet)

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
            Dim endDate As String = ""
            Dim pos As String = ""
            Dim outlet As String = ""
            Try
                reqDate = Request.QueryString("requestDate")
                endDate = Request.QueryString("endDate")
                outlet = Request.QueryString("outlet")

            Catch ex As Exception
                'The Req Date was Not Sent
                reqDate = ""
            End Try
            'Define the Queries
            Dim query As String = ""

            'Date Defined
            Try

                If String.IsNullOrEmpty(reqDate) Or String.IsNullOrEmpty(endDate) Or String.IsNullOrEmpty(outlet) Then
                    'Some parameter missing, take him to reports page
                    Response.Redirect("/QueryDB.aspx")

                Else

                    headerDate.Text = ": " + reqDate + " - " + endDate
                    'No Error so lets continue
                    query = String.Format("SELECT FLD120+'-'+FLD122 AS 'BusRoute',COUNT(DISTINCT(FLD123)) AS 'TotalPOS',COUNT(IDRELATION) AS 'TotalTickets',SUM(PRICE) AS 'TotalRevenue'" &
                                              " ,SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking'" &
                                              ",SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB'" &
                                              ",ROUND(SUM(CASE WHEN FLD118='RWF' OR FLD118 IS NULL THEN TOTAL ELSE 0 END) + (SUM(CASE WHEN FLD118='FIB' THEN TOTAL ELSE 0 END) /2.2),0) AS 'TotalRight'" &
                    ",SUM(CASE WHEN FLD118 ='RWF' THEN Total ELSE 0 END) as 'TotalRWF'" &
                                " FROM su.SALES_PROD INNER JOIN su.SALES ON su.SALES_PROD.IDSALE = su.SALES.IDSALE" &
                                        " WHERE CAST(su.SALES_PROD.FLD108 AS DATE) BETWEEN CONVERT(DATE,'{0}',103) AND CONVERT(DATE,'{1}',103) AND FLD117={2}" &
                                                " GROUP BY FLD120+'-'+FLD122  ORDER BY 'TotalRevenue' DESC", reqDate, endDate, outlet)
                End If

            Catch
            End Try

            Dim ds As New DataSet
            Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)
            ds.Tables.Add(dt)
            rptMasterView.DataSource = ds
            rptMasterView.DataBind()

            ' Now Work on the Pie Chart
            'Dim json As String = "[['Bus Route','Total Revenue Today'],"

            'For Each rw As DataRow In dt.Rows
            '    json += String.Format("['{0}',{1}],", rw("BusRoute").ToString, rw("TotalRevenue").ToString)
            'Next
            'Dim charsToTrim() As Char = {","c}
            'Dim jsonF As String = json.TrimEnd(charsToTrim)
            'jsonF += "]"

            'Dim mys As StringBuilder = New StringBuilder()
            ''        mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
            'mys.AppendLine("function drawChart() {")
            'mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
            'mys.AppendLine("var options = {title: 'Total Revenue Per Route',legend: {position: 'none'}};")
            'mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('piechart'));")
            'mys.AppendLine("chart.draw(data, options);")
            'mys.AppendLine("}drawChart();")

            'Dim script As String = mys.ToString()
            'Dim csType As Type = Me.[GetType]()
            'Dim cs As ClientScriptManager = Page.ClientScript
            'cs.RegisterClientScriptBlock(csType, "pieupdate", script, True)

        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try
    End Sub

    Protected Sub exportExcel_Click(sender As Object, e As EventArgs)
        Using excel As New ExcelPackage()
            'Create the worksheet
            Dim workSheet As ExcelWorksheet = excel.Workbook.Worksheets.Add("Report-" + headerDate.Text)
            'Add headers
            workSheet.Cells(1, 1).Value = "Bus Route"
            workSheet.Cells(1, 2).Value = "Total Tickets"
            workSheet.Cells(1, 3).Value = "Total Booking"
            workSheet.Cells(1, 4).Value = "Total Promo"
            workSheet.Cells(1, 5).Value = "Total PoS"
            workSheet.Cells(1, 6).Value = "Total RWF"
            workSheet.Cells(1, 7).Value = "Total FIB"

            Dim i As Integer = 2
            For Each c As Control In rptMasterView.Items
                Dim BusRoute As String = DirectCast(c.FindControl("BusRoute"), Literal).Text
                Dim TotalTickets As String = DirectCast(c.FindControl("TotalTickets"), Literal).Text
                Dim TotalBooking As String = DirectCast(c.FindControl("TotalBooking"), Literal).Text
                Dim TotalPromo As String = DirectCast(c.FindControl("TotalPromo"), Literal).Text
                Dim TotalPos As String = DirectCast(c.FindControl("TotalPos"), Literal).Text
                Dim TotalRWF As String = DirectCast(c.FindControl("TotalRWF"), Literal).Text
                Dim TotalFIB As String = DirectCast(c.FindControl("TotalFIB"), Literal).Text

                workSheet.Cells(i, 1).Value = BusRoute
                workSheet.Cells(i, 2).Value = TotalTickets
                workSheet.Cells(i, 3).Value = TotalBooking
                workSheet.Cells(i, 4).Value = TotalPromo
                workSheet.Cells(i, 5).Value = TotalPos
                workSheet.Cells(i, 6).Value = TotalRWF
                workSheet.Cells(i, 7).Value = TotalFIB

                i = i + 1
            Next

            Using memoryStream As New MemoryStream()
                Dim contentHeader As String = "attachment;  filename=" + headerDate.Text.Replace("/", "_") + ".xlsx"
                contentHeader = contentHeader.Replace(" ", "")
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", contentHeader)
                excel.SaveAs(memoryStream)
                memoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using

        End Using
    End Sub
End Class
