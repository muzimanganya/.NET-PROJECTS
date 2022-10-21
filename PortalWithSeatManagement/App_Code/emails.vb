Imports Microsoft.VisualBasic

#Region "Interfaces"
Public Interface IInvoiceBody
    Property CustomerNo As String
    Property InvoiceDate As String

    Function GetTotalTickets() As String
    Function GetCustomerAddress() As String
    Function GetInvoiceBody() As String
    Function GetEmailTemplate() As String
End Interface

Public Interface IEmail
    Property TargetEmail As String
    Property FromEmail As String
    Property SMTPServer As String
    Property SMTPPort As String

    Function AddTracking() As String
End Interface

Public Interface ISendEmail
    Function SendEmail(MailSettings As IEmail, MailBody As IInvoiceBody) As Boolean
End Interface
#End Region

#Region "Classes"

#End Region

Public Class InvoiceBody
    Implements IInvoiceBody
    Private CustNo As String
    Private clsDataLayer As New Datalayer
    Public Property CustomerNo As String Implements IInvoiceBody.CustomerNo
        Get
            Return CustNo
        End Get
        Set(value As String)
            CustNo = value
        End Set
    End Property

    Public Function GetCustomerAddress() As String Implements IInvoiceBody.GetCustomerAddress
        Dim add = ConfigurationManager.AppSettings("CustomerAddress")
        Dim tmp = add.Replace("{", "<").Replace("}", ">")
        Return tmp
    End Function

    Public Function GetEmailTemplate() As String Implements IInvoiceBody.GetEmailTemplate

    End Function

    Public Function GetInvoiceBody() As String Implements IInvoiceBody.GetInvoiceBody

    End Function

    Public Function GetTotalTickets() As String Implements IInvoiceBody.GetTotalTickets

    End Function

    Public Property InvoiceDate As String Implements IInvoiceBody.InvoiceDate
End Class

Public Class emails
End Class
