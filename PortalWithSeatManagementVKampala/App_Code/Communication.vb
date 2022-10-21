Imports Microsoft.VisualBasic
Imports Esicia
Public Class Communication
    Public Sub SendSMS(Message As String, Phone As String)
        ' Dim myRequest As Esicia.ksendRequest = New ksendRequest("innovys", "", "Innovys Ltd", Message, Phone)

        Dim sendSMS As ksmsPortTypeClient = New ksmsPortTypeClient()
        Dim balance, status As String

        balance = ""
        status = ""


        sendSMS.ksend("innovys", "", "Innovys Ltd", Message, Phone, balance, status)

    End Sub
End Class
