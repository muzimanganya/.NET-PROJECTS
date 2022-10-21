Imports System.Data.SqlClient
Imports System.Data

Partial Class QueryCodes
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Try
        If Page.IsPostBack Then
            Dim pageToQuery As String = Request("ctl00$ctl00$body$content$ddlModules").ToString()
            Dim code As String = Request("ctl00$ctl00$body$content$txtDate").ToString()

            If Not String.IsNullOrEmpty(pageToQuery) AndAlso Not String.IsNullOrEmpty(code) Then
                Dim redString As String = String.Format("{0}{1}", pageToQuery, code)
                Response.Redirect(redString)
            Else
                'Bad or Empty Date
            End If

        End If
        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Query For Codes | Sinnovys Portal"
    End Sub
End Class
