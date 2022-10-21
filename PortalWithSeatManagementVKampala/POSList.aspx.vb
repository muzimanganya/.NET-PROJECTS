Imports System.Data.SqlClient
Imports System.Data

Partial Class POSList
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Page.IsPostBack Then
                'The Check Boxex maybe posting back so update the database with the new values
                Dim updateQueries As String = ""
                For Each item As RepeaterItem In rptPosList.Items
                    Dim chkbx As CheckBox = TryCast(item.FindControl("chkReport"), CheckBox)
                    Dim onClickAttr As String = chkbx.Attributes("onClick").ToString()
                    Dim posID As String = onClickAttr.Split(New Char() {";"c}, 1)(0)
                    Dim updateQuery As String = ""
                    If chkbx.Checked Then
                        updateQuery = "UPDATE su.DOC SET FLD271=1 WHERE IDDOCUMENT=" + posID
                    Else
                        updateQuery = "UPDATE su.DOC SET FLD271=0 WHERE IDDOCUMENT=" + posID
                    End If
                    updateQueries += updateQuery
                    updateQueries += ";"
                Next
                clsDataLayer.executeCommand(updateQueries)
            End If
            'Dim query As String = "SELECT IDDOCUMENT,NAME, MEMO,FLD220 AS 'MSISDN', FLD221 AS 'IMSI', FLD222 AS 'SimNum',  FLD267 AS 'Place'," &
            '                        "FLD268 AS 'TigoSIM',FLD265 AS 'Report',FLD266 AS 'UserMobile' FROM su.DOC"

            Dim query As String = "SELECT IDDOCUMENT,NAME, MEMO,FLD220 AS 'MSISDN', FLD221 AS 'IMSI', FLD222 AS 'SimNum',  FLD267 AS 'Place'," &
                                "FLD269 AS 'TigoSIM',FLD271 AS 'Report',FLD270 AS 'UserMobile' FROM su.DOC"

            Dim dt As DataTable = clsDataLayer.ReturnDataTable(query)
            rptPosList.DataSource = dt
            rptPosList.DataBind()

            'Calculate the Total Sales Today
            Dim queryTotal = "SELECT SUM(PRICE) AS 'Total' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD)=MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE()) ORDER BY 'Total' DESC"
            Dim dtTotal As DataTable = clsDataLayer.ReturnDataTable(queryTotal)
            rptSummary.DataSource = dtTotal
            rptSummary.DataBind()

            Dim queryRevenue = "SELECT COUNT(IDRELATION) AS 'TotalTickets',ISNULL(FLD123,'-') AS 'POS',SUM(TOTAL) as 'TotalRevenue','' AS 'TotalBooking','' As 'TotalPromotions'" &
                                    "FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE()) GROUP BY su.SALES_PROD.FLD123 ORDER BY 'TotalRevenue' DESC "
            Dim dtT As DataTable = clsDataLayer.ReturnDataTable(queryRevenue)

            Try
                ' Now Work on the Pie Chart
                Dim json As String = "[['POS Number','Total Revenue Today'],"

                For Each rw As DataRow In dtT.Rows
                    json += String.Format("['{0}',{1}],", rw("POS").ToString, rw("TotalRevenue").ToString)
                Next
                Dim charsToTrim() As Char = {","c}
                Dim jsonF As String = json.TrimEnd(charsToTrim)
                jsonF += "]"

                Dim mys As StringBuilder = New StringBuilder()
                mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
                mys.AppendLine("function drawChart() {")
                mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                mys.AppendLine("var options = {title: 'Total Revenue Per POS',legend: {position: 'none'}};")
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

        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try


        Page.Title = "POS List & Control"
    End Sub

    Public Sub Button_OnClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = DirectCast(sender, Button)

            If btn IsNot Nothing Then
                Dim cmd As String = btn.CommandArgument
                If cmd = 1 Then
                    'This is for Accept All
                    Dim updateQuery As String = "UPDATE su.DOC set FLD271=1;"
                    clsDataLayer.executeCommand(updateQuery)
                Else
                    'This is for Cancel All
                    Dim updateQuery As String = "UPDATE su.DOC set FLD271=0;"
                    clsDataLayer.executeCommand(updateQuery)
                End If
            End If
            Response.Redirect("POSList.aspx")
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

    End Sub

    Protected Sub rptPosList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPosList.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                DirectCast(e.Item.FindControl("chkReport"), CheckBox).Attributes.Add("onClick", DirectCast(e.Item.DataItem, DataRowView)("IDDOCUMENT").ToString())
                Dim Reportstate As String = DirectCast(e.Item.DataItem, DataRowView)("REPORT").ToString()
                If Reportstate = 1 Then
                    'Accepted REport
                    DirectCast(e.Item.FindControl("chkReport"), CheckBox).Attributes("checked") = "checked"
                Else
                    'Cancel the report
                    DirectCast(e.Item.FindControl("chkReport"), CheckBox).Attributes("checked") = ""
                End If
            End If
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

    End Sub
End Class
