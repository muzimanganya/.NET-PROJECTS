Imports System.Data.SqlClient
Imports System.Data

Partial Class RolledTicketDetails
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then

        Else
            Try
                Dim ticketID As String = "0"
                Try
                    ticketID = Request.QueryString("TicketId")
                    If ticketID = " " Or ticketID = "" Then
                        ticketID = "0"
                    End If
                Catch ex As Exception
                    ticketID = "0"
                End Try

                'Query for the TicketDetails
                Dim query As String =
                    String.Format("SELECT sp.IDRELATION,sp.DATE_MOD AS 'DateOfTravel','Web Service' as 'Username',sp.FLD215 AS 'Circuit',sp.FLD109 AS 'Hour',sp.Total,sp.FLD111 AS 'Traveler',FLD191 AS 'ClientCode',s.NAME AS 'BusName'" &
                                    "FROM su.ROLLED_SALES_PROD AS sp INNER JOIN su.SALES AS s ON sp.IDSALE = s.IDSALE" &
                                        " WHERE IDRELATION = {0}", ticketID)

                Dim dt As New DataTable
                Try
                    dt = clsDataLayer.ReturnDataTable(query)
                    ticketDetails.DataSource = dt
                    ticketDetails.DataBind()
                Catch ex As Exception
                    clsDataLayer.LogException(ex)
                End Try
            Catch ex As Exception
                clsDataLayer.LogException(ex)
                Response.Redirect("/CustomError.aspx")
            End Try

        End If
        Page.Title = "Rolled Back Ticket Details | Sinnovys"
    End Sub
End Class
