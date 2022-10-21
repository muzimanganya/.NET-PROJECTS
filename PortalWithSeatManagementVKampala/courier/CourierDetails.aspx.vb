Imports System.Data.SqlClient
Imports System.Data

Partial Class CourierDetails
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                Dim courierID As String = Request("CourierID").ToString
                LoadData(courierID)
                LoadQR(courierID)
                LoadTimeLineData(courierID)

                txtCourierID.Text = courierID

            Catch ex As Exception

            End Try

        End If
    End Sub

    Private Sub LoadData(CourierID As String)
        Dim queryData As String = String.Format("SELECT CityIn+'-'+CityOut AS 'Route',SenderName,ReceiverName,s.StatusName,SenderPhone,ReceiverPhone,Creator,CourierID " +
                                                " FRom dbo.Couriers c INNER JOIN dbo.CourierStatus s ON c.Status = s.StatusID WHERE CourierID={0}", CourierID)

        Dim dtData As DataTable = clsDataLayer.ReturnDataTable(queryData)

        rptDetails.DataSource = dtData

        rptDetails.DataBind()

        'ltrError.Text += "Main Data Loaded"

    End Sub

    Private Sub LoadTimeLineData(CourierID As String)

        Dim query = String.Format("SELECT CONVERT(VARCHAR,ct.TransactionDate,107) AS 'TDate',c.CourierID,ct.Latitude,ct.Longitude,ct.Description,c.CityIn,c.CityOut,c.SenderName," +
                                    " c.SenderPhone, c.ReceiverName, c.ReceiverPhone,cs.TimeLineIcon,cs.Icon" +
                             " FROM dbo.CourierTransactions ct INNER JOIN dbo.Couriers c ON ct.CourierID = c.CourierID INNER JOIN dbo.CourierStatus cs ON ct.StatusID = cs.StatusID WHERE ct.CourierID={0} ORDER BY ct.TransactionDate DESC",
                             CourierID)
        '
        'ltrError.Text += " Query sis now " + query + "\n"

        Dim dtData As DataTable = clsDataLayer.ReturnDataTable(query)
        rptTimeline.DataSource = dtData
        rptTimeline.DataBind()

        'ltrError.Text += query

    End Sub

    Private Sub LoadQR(CourierID As String)
        Dim qr As String = String.Format("https://chart.googleapis.com/chart?cht=qr&chs=190x190&chl={0}", CourierID)

        imgQr.ImageUrl = qr

        'ltrError.Text += "QR Code Loaded\n"

    End Sub

    Protected Sub txtDate_TextChanged(sender As Object, e As EventArgs)
        Dim dateSelected As String = txtDate.Text

        Dim aDate As DateTime = Convert.ToDateTime(dateSelected)

        System.Threading.Thread.Sleep(3000)

        Dim queryBuses As String = String.Format("SELECT IDSALE,NAME FROM su.SALES WHERE DAY(TARGET_DATE) = {0} AND MONTH(TARGET_DATE)={1} AND YEAR(TARGET_DATE) = {2}",
                                                 aDate.Day, aDate.Month, aDate.Year)

        Dim dtDrop As DataTable = clsDataLayer.ReturnDataTable(queryBuses)

        ddlBuses.DataSource = dtDrop
        ddlBuses.DataValueField = "IDSALE"
        ddlBuses.DataTextField = "NAME"
        ddlBuses.DataBind()


    End Sub
End Class
