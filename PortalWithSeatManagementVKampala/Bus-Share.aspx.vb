Imports System.Data
Partial Class Busshare
    Inherits System.Web.UI.Page
    Private idsale As Double = 0

    Private clsDataLayer As New Datalayer
    Public ksStatus As Boolean = True

    Dim bsCapacity As Integer, ksCapacity As Integer, bsSold As Integer, ksSold As Integer, sisterBus As Double
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Try
        idsale = Request("Idsale").ToString()

        bsCapacity = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT FLD103 FROM su.SALES WHERE IDSALE={0}", idsale), "Belvedere")
        sisterBus = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT IDSALEK FROM dbo.SharedBus WHERE IDSALEB={0}", idsale))
        ksCapacity = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT FLD103 FROM su.SALES WHERE IDSALE={0}", sisterBus), "KigaliSafaris")

        bsSold = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0}", idsale), "Belvedere")
        ksSold = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0}", sisterBus), "KigaliSafaris")

        lblBCapacity.Text = bsCapacity
        lblBFree.Text = bsCapacity - bsSold

        lblKCapacity.Text = ksCapacity
        lblKFree.Text = ksCapacity - ksSold

        If ksCapacity <= ksSold Then
            ksStatus = False
        End If
        'Now Load the Current Request
        Dim queryTKS As String = String.Format("SELECT [RequestID],[FromUsername],[SeatsNo],[CreatedOn],[Status],[RequestComment]" &
                                                    " FROM [dbo].[BusShareRequests]" &
                                                        " WHERE Category='TKS' AND BusID={0}", idsale)
        Dim queryFKS As String = String.Format("SELECT [RequestID],[FromUsername],[SeatsNo],[CreatedOn],[Status],[RequestComment]" &
                                                    " FROM [dbo].[BusShareRequests] " &
                                                        " WHERE Category='FKS' AND BusID={0}", idsale)

        Dim dTKS As DataTable = clsDataLayer.ReturnDataTable(queryTKS)
        Dim dFKS As DataTable = clsDataLayer.ReturnDataTable(queryFKS)

        rptToKS.DataSource = dTKS
        rptToKS.DataBind()
        rptFromKS.DataSource = dFKS
        rptFromKS.DataBind()
        'Catch ex As Exception

        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Bus Capacity Sharing"
    End Sub
    Public Function GetSeat() As String
        Dim seatNo As Integer = 0
        'Start with Belvedere
        If (bsCapacity > 0) Then

            If bsCapacity <= bsSold Then
                'We're dealing with a sold seat
                bsCapacity -= 1
                Return "<span class='sold'>" + bsCapacity + 1 + "</span>"
            Else
                bsCapacity -= 1
                Return "<span class='free'>" + bsCapacity + 1 + "</span>"
            End If
        Else
            'Continue with Kigali Safaris
            If ksCapacity <= ksSold Then
                'We're dealing with a sold seat
                ksCapacity -= 1
                Return "<span class='sold'>" + ksCapacity + 1 + "</span>"
            Else
                ksCapacity -= 1
                Return "<span class='free'>" + ksCapacity + 1 + "</span>"
            End If
        End If

    End Function
End Class
