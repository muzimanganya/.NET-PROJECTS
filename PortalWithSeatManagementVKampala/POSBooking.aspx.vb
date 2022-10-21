Imports System.Data.SqlClient
Imports System.Data

Partial Class POSBooking
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
            Dim totalPromo As Double = 0
            Dim totalNormal As Double = 0
            Dim totalSubscription As Double = 0
            If String.IsNullOrEmpty(reqDate) Then
                ' This is the default view
                query = "SELECT FLD120 AS 'CityIN',FLD122 AS 'CityOut',IDRELATION,TOTAL,Discount,FLD213 AS 'Sno'," &
                                    "FLD191 AS 'ClientCode',DATE_MOD AS 'CreatedDate',FLD123 AS 'CreatedBy', FLD111 AS 'Traveler' FROM su.SALES_PROD " &
                                        "WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD)= MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE()) AND FLD213 <> '-' AND FLD213<> 'PROMO'"
                queryTotal = "SELECT COUNT(IDRELATION) AS 'Total' FROM su.SALES_PROD WHERE FLD213 <> '-' AND FLD213 <> 'PROMO' AND " &
                                "DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD)= MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())  "
                totalPromo = clsDataLayer.ReturnSingleNumeric("SELECT  SUM(PRICE) from su.SALES_PROD WHERE FLD213='PROMO'  AND DAY(DATE_MOD) =DAY(GETDATE())  AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())")
                totalNormal = clsDataLayer.ReturnSingleNumeric("SELECT SUM(TOTAL) AS 'Total' FROM su.SALES_PROD WHERE FLD213 = '-' AND DAY(DATE_MOD) =DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())")
                totalSubscription = clsDataLayer.ReturnSingleNumeric("SELECT SUM(sp.FLD272) AS 'TotalBooking' FROM su.SALES_PROD AS sp" &
                                                                        " WHERE sp.FLD213 <> '-' AND sp.FLD213 <> 'PROMO' AND DAY(sp.DATE_MOD) =DAY(GETDATE()) AND " &
                                                                            "Month(sp.DATE_MOD) = Month(GETDATE()) And Year(sp.DATE_MOD) = Year(GETDATE())")
                headerDate.Text = ": " + DateTime.Now.Day.ToString + "/" + DateTime.Now.Month.ToString + "/" + DateTime.Now.Year.ToString
            Else
                'They requested a specific date
                'Lets try to parse the requestdate as a DateTime
                Try
                    'Dim exactDate As DateTime = DateTime.Parse(reqDate)
                    Dim exactDate = reqDate
                    headerDate.Text = reqDate
                    'No Error so lets continue
                    query = String.Format("SELECT FLD120 AS 'CityIN',FLD122 AS 'CityOut',IDRELATION,TOTAL,Discount,FLD213 AS 'Sno'," &
                                    "FLD191 AS 'ClientCode',DATE_MOD AS 'CreatedDate',FLD123 AS 'CreatedBy', FLD111 AS 'Traveler' FROM su.SALES_PROD " &
                                        " WHERE DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103)) AND FLD213 <> '-' AND FLD213<> 'PROMO'", exactDate)

                    queryTotal = String.Format("SELECT COUNT(IDRELATION) AS 'Total' FROM su.SALES_PROD WHERE FLD213 <> '-' AND FLD213 <> 'PROMO' AND " &
                                "DAY(DATE_MOD) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", exactDate)

                    totalPromo = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT  SUM(PRICE) from su.SALES_PROD WHERE FLD213='PROMO'  AND DAY(DATE_MOD) =DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", exactDate))
                    totalNormal = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT SUM(TOTAL) AS 'Total' FROM su.SALES_PROD WHERE FLD213 = '-' AND DAY(DATE_MOD) =DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", exactDate))
                    ' totalSubscription = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT SUM(p.PRICE1) FROM su.SALES_PROD AS sp INNER  JOIN su.PRODUCTS AS p ON sp.IDPRODUCT = p.IDPRODUCT" &
                    '                                                              " WHERE sp.FLD213 <> '-' AND sp.FLD213 <> 'PROMO' AND DAY(sp.DATE_MOD) =DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(sp.DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(sp.DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", exactDate))
                    totalSubscription = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT SUM(sp.FLD272) FROM su.SALES_PROD AS sp " &
                                                                            " WHERE sp.FLD213 <> '-' AND sp.FLD213 <> 'PROMO' AND DAY(sp.DATE_MOD) =DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", exactDate))

                    headerDate.Text = reqDate
                Catch ex As Exception
                    'Bad Date Format display an Error
                    '   Response.Redirect("")
                    clsDataLayer.LogException(ex)
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


                Dim json As String = "[['Mode of Payment','Total Revenue'],"

                json += String.Format("['Promotions',{0}],", totalPromo)
                json += String.Format("['Booking',{0}],", totalSubscription)
                json += String.Format("['Cash Ticketing',{0}],", totalNormal)

                Dim charsToTrim() As Char = {","c}
                Dim jsonF As String = json.TrimEnd(charsToTrim)
                jsonF += "]"


                Dim mys As StringBuilder = New StringBuilder()
                mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
                mys.AppendLine("function drawChart() {")
                mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                mys.AppendLine("var options = {title: 'Total Revenue Per Mode'};")
                mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('chart_div'));")
                mys.AppendLine("chart.draw(data, options);")
                mys.AppendLine("}")

                Dim script As String = mys.ToString()
                Dim csType As Type = Me.[GetType]()
                Dim cs As ClientScriptManager = Page.ClientScript
                cs.RegisterClientScriptBlock(csType, "pie", script, True)
            Catch ex As Exception
                clsDataLayer.LogException(ex)
            End Try

        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


        Page.Title = "POS Booking Information"
    End Sub
End Class
