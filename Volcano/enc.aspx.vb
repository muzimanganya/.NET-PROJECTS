
Partial Class enc
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim entered As String
        entered = TextBox1.Text
        Label1.Text = myEncryption.Encrypt(entered, "Ey5TXu2WLMKvpa")
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim entered As String = TextBox2.Text
        Label2.Text = myEncryption.Decrypt(entered, "Ey5TXu2WLMKvpa")
    End Sub
End Class
