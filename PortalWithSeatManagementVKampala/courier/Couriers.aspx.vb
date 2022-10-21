Imports System.Data.SqlClient
Imports System.Data

Partial Class Couriers
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadData()
            LoadChart()
        End If
    End Sub

    Private Sub LoadData()
        Try
            Dim queryData As String = String.Format("SELECT CityIn,CityOut,SenderName,ReceiverName,s.StatusName,SenderPhone,ReceiverPhone,Creator,CourierID " +
                                                " FRom dbo.Couriers c INNER JOIN dbo.CourierStatus s ON c.Status = s.StatusID WHERE s.StatusID=1")

            Dim dtData As DataTable = clsDataLayer.ReturnDataTable(queryData)

            queryData = String.Format("SELECT CityIn,CityOut,SenderName,ReceiverName,s.StatusName,SenderPhone,ReceiverPhone,Creator,CourierID " +
                                            " FRom dbo.Couriers c INNER JOIN dbo.CourierStatus s ON c.Status = s.StatusID")

            Dim dtDataAll As DataTable = clsDataLayer.ReturnDataTable(queryData)

            rptAllCouriers.DataSource = dtDataAll
            rptData.DataSource = dtData
            rptAllCouriers.DataBind()
            rptData.DataBind()
        Catch ex As Exception

        End Try
        
    End Sub

    Private Sub LoadChart()
        Try
            Dim queryChart As String = String.Format("SELECT CityIn+'-'+CityOut,COUNT(*) AS 'TotalCouriers' FROM dbo.Couriers GROUP BY CityIn+'-'+CityOut")

            Dim dtData As DataTable = clsDataLayer.ReturnDataTable(queryChart)

            ' Now Work on the Pie Chart
            Dim json As String = "[['Bus Route','Total Revenue Today'],"

            For Each rw As DataRow In dtData.Rows
                json += String.Format("['{0}',{1}],", rw(0).ToString, rw(1).ToString)
            Next
            Dim charsToTrim() As Char = {","c}
            Dim jsonF As String = json.TrimEnd(charsToTrim)
            jsonF += "]"

            Dim mys As StringBuilder = New StringBuilder()
            mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
            mys.AppendLine("function drawChart() {")
            mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
            mys.AppendLine("var options = {title: 'Total Couriers Per Route',legend: {position: 'none'}};")
            mys.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('piechart'));")
            mys.AppendLine("chart.draw(data, options);")
            mys.AppendLine("}")

            Dim script As String = mys.ToString()
            Dim csType As Type = Me.[GetType]()
            Dim cs As ClientScriptManager = Page.ClientScript
            cs.RegisterClientScriptBlock(csType, "pie", script, True)
        Catch ex As Exception

        End Try
        
    End Sub
End Class
