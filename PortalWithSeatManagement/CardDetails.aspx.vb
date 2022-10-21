Imports System.Data.SqlClient
Imports System.Data

Partial Class CardDetails
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then

        Else
            Try
                Dim cardno As String = "0"
                Try
                    cardno = Request.QueryString("cardno")
                    If cardno = " " Or cardno = "" Or cardno = "-" Or cardno = "PROMO" Or cardno.Length < 2 Then
                        cardno = "0"
                    End If
                Catch ex As Exception
                    cardno = "0"
                End Try

                'Query for the TicketDetails
                Dim query As String =
                    String.Format("SELECT IDCUSTOM1,DATE_TRANSF AS 'DateSold',FLD243 AS 'Circuit',FLD247 AS 'Status'," &
                                    "FLD249 AS 'Traveler',FLD248 AS 'POS',FLD261 AS 'Price',FLD244 AS 'Trips',FLD269 AS 'Discount'" &
                                        "FROM su.CUSTOM1 WHERE IDCUSTOM1={0}", cardno)

                Dim dt As New DataTable

                Try
                    dt = clsDataLayer.ReturnDataTable(query)
                    ticketDetails.DataSource = dt
                    ticketDetails.DataBind()
                Catch ex As Exception
                    clsDataLayer.LogException(ex)
                End Try

                Try
                    'Now Construct the QR Code
                    'https://chart.googleapis.com/chart?cht=qr&chs=160x160&chl=840240239+KIGALI-MUHANGA:09H00+PETER
                    Dim tinfo As String = ""
                    With dt.Rows(0)
                        tinfo = String.Format("Name:{0}+Route:{1}+CardNo:{2}+Remaining Trips: {3}+Status:{4}+Total:{5}", .Item("Traveler").ToString, .Item("Circuit").ToString, .Item("IDCUSTOM1").ToString, .Item("Trips").ToString, .Item("Status").ToString, .Item("Price").ToString)
                    End With

                    Dim qr As String = String.Format("https://chart.googleapis.com/chart?cht=qr&chs=190x190&chl={0}", tinfo)
                    imgQr.ImageUrl = qr
                Catch ex As Exception
                    clsDataLayer.LogException(ex)
                End Try

                Try
                    'Do the chart now
                    Dim queryChart As String =
                        String.Format("SELECT COUNT(IDRELATION) AS 'TotalBookings',CONVERT(VARCHAR,DAY(DATE_MOD))+'th, '+DATENAME(MONTH,DATE_MOD)+'-'+CONVERT(VARCHAR,YEAR(DATE_MOD)) AS 'Date'" &
                                        "FROM su.SALES_PROD WHERE FLD213='{0}'" &
                                            "GROUP BY CONVERT(VARCHAR,DAY(DATE_MOD))+'th, '+DATENAME(MONTH,DATE_MOD)+'-'+CONVERT(VARCHAR,YEAR(DATE_MOD))", cardno)

                    Dim dtTrend = clsDataLayer.ReturnDataTable(queryChart)

                    Dim json As String = "[['Day of Year','Total Booking'],"


                    For Each rw As DataRow In dtTrend.Rows()
                        json += String.Format("['{0}',{1}],", rw("Date").ToString, rw("TotalBookings").ToString)
                    Next
                    Dim charsToTrim() As Char = {","c}
                    Dim jsonF As String = json.TrimEnd(charsToTrim)
                    jsonF += "]"

                    Dim mys As StringBuilder = New StringBuilder()
                    'mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawAreaChart);")
                    mys.AppendLine("function drawAreaChart() {")
                    mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                    mys.AppendLine("var options = {title:  'Card Booking Trend',hAxis: { title: 'Day of Year', titleTextStyle: { color: 'red'} }};")
                    mys.AppendLine("var chart = new google.visualization.AreaChart(document.getElementById('chart_div'));")
                    mys.AppendLine("chart.draw(data, options);")
                    mys.AppendLine("}")

                    Dim initScript As String = mys.ToString + "function initCharts () {drawAreaChart();}" +
                                            "google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(initCharts);"

                    Dim csType As Type = Me.[GetType]()
                    Dim cs As ClientScriptManager = Page.ClientScript
                    cs.RegisterClientScriptBlock(csType, "charts", initScript, True)

                Catch ex As Exception
                    clsDataLayer.LogException(ex)
                End Try

                Try
                    'Add the history now
                    Dim queryHistory As String = String.Format("SELECT FLD120 AS 'CityIN',FLD122 AS 'CityOut',IDRELATION,Total,Discount,FLD213 AS 'Sno'," &
                                                               "FLD191 AS 'ClientCode',FLD111 AS 'Traveler',DATE_MOD AS 'CreatedOn',FLD123 AS 'CreatedBy'" &
                                                                        " FROM su.SALES_PROD WHERE FLD213='{0}'", cardno)
                    Dim dtHistory As DataTable = clsDataLayer.ReturnDataTable(queryHistory)

                    rptHistory.DataSource = dtHistory
                    rptHistory.DataBind()
                Catch ex As Exception
                    clsDataLayer.LogException(ex)
                End Try

            Catch ex As Exception
                Response.Redirect("CustomError.aspx")
                clsDataLayer.LogException(ex)
            End Try

        End If
        Page.Title = "Card Details"
    End Sub
End Class
'    Protected Sub btnDeleteTicket_Click(sender As Object, e As EventArgs) Handles btnDeleteTicket.Click
'        'Now here we need to rollback the ticket marking the username in the process
'        Try
'            Dim cmd As New SqlCommand()

'            Dim currentUser As String = HttpContext.Current.User.Identity.Name

'            Dim cardno As String = "0"
'            If Request.QueryString("cardno") IsNot Nothing Then
'                cardno = Request.QueryString("cardno")
'            End If


'            With cmd
'                .CommandText = "su.UserTicketRollback"
'                .CommandType = CommandType.StoredProcedure
'                .Parameters.AddWithValue("@cardno", cardno)
'                .Parameters.AddWithValue("@Username", currentUser)
'            End With

'            clsDataLayer.ExecuteProc(cmd)
'        Catch ex As Exception
'            ltrErrorMessage.Text = "Ticket Could Not Be Deleted Due to the folowing Error: " + ex.Message
'        End Try
'    End Sub
'End Class
