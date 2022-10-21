Imports System.Data.SqlClient
Imports System.Data

Partial Class QueryCards
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
        If Page.IsPostBack Then
            Dim CityIn As String = ddlCityIN.SelectedValue
            Dim CityOut As String = ddlCityOut.SelectedValue
            Dim startDate As String = txtStartDate.Text
            Dim endDate As String = txtEndDate.Text
            Dim status As String = ddlStatus.SelectedValue
            Dim CircuitF = CityIn + "-" + CityOut
            Dim CircuitB = CityOut + "-" + CityIn
            Dim query As String

            If status = 0 Then
                'Query for Sold Cards
                query = String.Format("Select IDCUSTOM1, ISNULL(FLD248,'-') AS 'CreatedBy', ISNULL(NAME,'-') AS 'NAME', FLD243 AS 'Trajet', FLD244 AS 'Trips'," &
                                    "FLD249 AS 'Traveler', FLD261 AS 'Price', FLD262 AS 'CreatedDate',FLD269 AS 'Discount',FLD261-FLD269 AS 'Total' " &
                                        "FROM su.CUSTOM1 WHERE  FLD250=2 AND CONVERT(DATETIME,DATE_CREAT,102) BETWEEN CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) AND FLD243='{2}' OR FLD243='{3}' ", startDate, endDate, CircuitF, CircuitB)
            ElseIf status = 1 Then
                'Query Expired
                query = String.Format("Select IDCUSTOM1, ISNULL(FLD248,'-') AS 'CreatedBy', ISNULL(NAME,'-') AS 'NAME', FLD243 AS 'Trajet', FLD244 AS 'Trips'," &
                                    "FLD249 AS 'Traveler', FLD261 AS 'Price', FLD262 AS 'CreatedDate',FLD269 AS 'Discount',FLD261-FLD269 AS 'Total' " &
                                        "FROM su.CUSTOM1 WHERE  FLD250=0 AND CONVERT(DATETIME,DATE_CREAT,102) BETWEEN CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) AND FLD243='{2}' OR FLD243='{3}' ", startDate, endDate, CircuitF, CircuitB)
            ElseIf status = 2 Then
                'Not Sold Cards
                query = String.Format("Select IDCUSTOM1,ISNULL(FLD248,'Not Yet Sold') AS 'CreatedBy', ISNULL(NAME,'Not Yet Sold') AS 'NAME', FLD243 AS 'Trajet', FLD244 AS 'Trips'," &
                                    "FLD249 AS 'Traveler', FLD261 AS 'Price', FLD262 AS 'CreatedDate',FLD269 AS 'Discount',FLD261-FLD269 AS 'Total'" &
                                        " FROM su.CUSTOM1 WHERE  FLD250=1 AND CONVERT(DATETIME,DATE_CREAT,102) BETWEEN CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) AND FLD243='{2}' OR FLD243='{3}' ", startDate, endDate, CircuitF, CircuitB)
            Else
                query = String.Format("Select IDCUSTOM1, ISNULL(FLD248,'-') AS 'CreatedBy', ISNULL(NAME,'-') AS 'NAME', FLD243 AS 'Trajet', FLD244 AS 'Trips'," &
                                    "FLD249 AS 'Traveler', FLD261 AS 'Price', FLD262 AS 'CreatedDate',FLD269 AS 'Discount',FLD261-FLD269 AS 'Total'" &
                                        " FROM su.CUSTOM1 WHERE CONVERT(DATETIME,DATE_CREAT,102) BETWEEN CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) AND FLD243='{2}' OR FLD243='{3}' ", startDate, endDate, CircuitF, CircuitB)
            End If

            Session("query") = query

            'Try
            Response.Redirect("/CardsDetails.aspx")
            'Catch ex As Exception

            'End Try

        Else
            'cvEndDate.ValueToCompare = DateTime.Today
        End If
        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Query For Cards"
    End Sub
End Class
