Imports System.Data.SqlClient
Imports System.Data

Partial Class PreBookings
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Try
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
            query = String.Format("SELECT p.BookingNo,p.CityIN, p.CityOut,p.Name+' '+p.FirstName AS 'Traveler',p.ClientCode,p.CreatedOn,p.Expires,p.Completed,p.Recon" &
                               ", s.Name as 'BusName',p.IDSALE AS 'BUSID',p.Creator FROM dbo.Prebookings AS p INNER JOIN su.SALES AS s ON p.IDSALE = s.IDSALE" &
                                   " WHERE DAY(CREATEDON) = DAY(GETDATE()) AND MONTH(CREATEDON)= MONTH(GETDATE()) AND YEAR(CREATEDON) = YEAR(GETDATE())")

        Else
            'They requested a specific date
            'Lets try to parse the requestdate as a DateTime
            Try
                'Dim exactDate As DateTime = DateTime.Parse(reqDate)
                Dim exactDate = reqDate
                headerDate.Text = reqDate
                'No Error so lets continue
                query = String.Format("SELECT p.BookingNo,p.CityIN,p.CityOut,p.Name+' '+p.FirstName AS 'Traveler',p.ClientCode,p.CreatedOn,p.Expires,p.Completed,p.Recon" &
                                ", s.Name as 'BusName',p.IDSALE as 'BUSID',p.Creator FROM dbo.Prebookings AS p INNER JOIN su.SALES AS s ON p.IDSALE = s.IDSALE" &
                                    " WHERE DAY(CREATEDON) = DAY(CONVERT(DATETIME,'{0}',103)) AND MONTH(CREATEDON)= MONTH(CONVERT(DATETIME,'{0}',103)) AND YEAR(CREATEDON) = YEAR(CONVERT(DATETIME,'{0}',103))", reqDate)

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



        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try


        Page.Title = "Pre-Booking Information"
    End Sub
End Class
