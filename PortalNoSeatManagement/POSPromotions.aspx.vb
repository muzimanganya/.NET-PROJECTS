Imports System.Data.SqlClient
Imports System.Data

Partial Class POSPromotions
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Dim dtPromos As DataTable

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
                query = "SELECT     IDSALE, IDPRODUCT, IDRELATION, DATE_TRANSF, PRICE, TOTAL, DATE_MOD AS 'Date', " &
                                    "FLD109 AS 'Hour', FLD111 AS 'Traveler', FLD120 AS 'CityIN', FLD122 AS 'CityOut', FLD123 AS 'CreatedBy', " &
                                        "FLD191 AS 'ClientCode', FLD215 AS 'Route', FLD213 as 'Sno'" &
                                            "FROM su.SALES_PROD " &
                                                "WHERE (FLD213 = 'PROMO') AND DAY(DATE_MOD) =DAY(GETDATE())  AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())"

                queryTotal = "SELECT COUNT(IDRELATION) AS 'Total',SUM(Discount) AS 'TotalDiscount' FROM su.SALES_PROD WHERE FLD213='PROMO' AND DAY(DATE_MOD) =DAY(GETDATE())  AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())"

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

                    query = String.Format("SELECT     IDSALE, IDPRODUCT, IDRELATION, DATE_TRANSF, PRICE, TOTAL, DATE_MOD AS 'Date', " &
                                    "FLD109 AS 'Hour', FLD111 AS 'Traveler', FLD120 AS 'CityIN', FLD122 AS 'CityOut', FLD123 AS 'CreatedBy', " &
                                        "FLD191 AS 'ClientCode', FLD215 AS 'Route', FLD213 as 'Sno'" &
                                            "FROM su.SALES_PROD " &
                                                "WHERE (FLD213 = 'PROMO') AND DAY(DATE_TRANSF) = DAY(CONVERT(DATETIME,'{0}',103)) " &
                                                "AND MONTH(DATE_TRANSF) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_TRANSF) = YEAR(CONVERT(DATETIME,'{0}',103))", exactDate)

                    queryTotal = String.Format("SELECT COUNT(IDRELATION) AS 'Total',SUM(Discount) AS 'TotalDiscount' FROM su.SALES_PROD WHERE FLD213='PROMO' AND DAY(DATE_TRANSF) =" &
                        " DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_TRANSF) = MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_TRANSF) = YEAR(CONVERT(DATETIME,'{0}',103))", exactDate)

                    totalPromo = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT  SUM(PRICE) from su.SALES_PROD WHERE FLD213='PROMO'  AND DAY(DATE_MOD) =DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", exactDate))
                    totalNormal = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT SUM(TOTAL) AS 'Total' FROM su.SALES_PROD WHERE FLD213 = '-' AND DAY(DATE_MOD) =DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", exactDate))
                    totalSubscription = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT SUM(sp.FLD272) FROM su.SALES_PROD AS sp " &
                                                                            " WHERE sp.FLD213 <> '-' AND sp.FLD213 <> 'PROMO' AND DAY(sp.DATE_MOD) =DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(DATE_MOD) =MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(DATE_MOD) =YEAR(CONVERT(DATETIME,'{0}',103))", exactDate))

                Catch ex As Exception
                    'Bad Date Format display an Error
                    Response.Redirect("")
                End Try

            End If



            Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)
            dtPromos = dt
            rptMasterView.DataSource = dt
            rptMasterView.DataBind()

            If User.IsInRole("restricted") Then
            Else
                'Calculate the Total Sales Today
                'Dim queryTotal As String = "SELECT COUNT(IDRELATION) AS 'Total' FROM su.SALES_PROD WHERE FLD213='PROMO' AND (CONVERT(varchar, DATE_TRANSF, 112) = CONVERT(varchar, GETDATE(), 112)) "
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
            End If
            

        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


        Page.Title = "POS Promotions Information"

    End Sub
    Public Function GetFormatClass(ClientCode As String) As String
        Dim formatString As String = ""
        Dim filterredtable = dtPromos.Select("ClientCode=" + ClientCode).CopyToDataTable()
        Dim count = filterredtable.Rows.Count
        If count > 1 AndAlso count < 4 Then
            'Highlight with Yellow
            formatString = "yellowFormat"
        ElseIf count > 4 Then
            'Highlight with Red
            formatString = "RedFormat"
        ElseIf count = 1 Then
            formatString = "Normal"
        End If
        Return formatString
    End Function
End Class
