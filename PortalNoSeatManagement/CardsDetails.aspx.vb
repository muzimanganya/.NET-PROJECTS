Imports System.Data.SqlClient
Imports System.Data

Partial Class CardsDetails
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

        query = Session("query").ToString

        If query IsNot String.Empty Then
            Dim dt As Data.DataTable = clsDataLayer.ReturnDataTable(query)
            rptMasterView.DataSource = dt
            rptMasterView.DataBind()

            Session("query") = String.Empty
        Else
            Try
                Response.Redirect("QueryCards.aspx")
            Catch ex As Exception

            End Try

        End If

        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try


        Page.Title = "Cards Details"
    End Sub
End Class
