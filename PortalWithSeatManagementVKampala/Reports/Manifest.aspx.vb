Imports System.Data
Partial Class Reports_Manifest
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim idsale As String = Request("idsale")

        Dim queryForPassengers = "SELECT IDRELATION,FLD120 AS 'CityIN',FLD122 AS 'CityOut','' AS 'ID','' AS 'Nationality',FLD123 as 'CreatedBy'," &
            "total from su.SALES_PROD WHERE IDSALE=1360"
        Dim dt As DataTable = clsDataLayer.ReturnDataTable(queryForPassengers)
        rptManifest.DataSource = dt
        rptManifest.DataBind()

        Dim queryBusName = "SELECT NAME FROM su.SALES WHERE IDSALE=" + idsale
        busname.Text = clsDataLayer.ReturnSingleValue(queryBusName)
    End Sub
End Class
