Imports System.IO

Partial Class Drivers_image
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString("photo") IsNot Nothing Then

        End If
        Try
            ' Read the file and convert it to Byte Array
            Dim filePath As String = (New Config).WritePath
            Dim filename As String = Request.QueryString("photo")
            Dim contenttype As String = "image/" & Path.GetExtension(filename).Replace(".", "")

            Dim fs As FileStream = New FileStream(filePath & filename,FileMode.Open, FileAccess.Read)

            Dim br As BinaryReader = New BinaryReader(fs)
            Dim bytes As Byte() = br.ReadBytes(Convert.ToInt32(fs.Length))
            br.Close()
            fs.Close()

            'Write the file to Reponse
            Response.Buffer = True
            Response.Charset = ""
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.ContentType = contenttype
            ' Response.AddHeader("content-disposition", "attachment;filename=" & filename)
            Response.BinaryWrite(bytes)
            Response.Flush()
            Response.End()
        Catch

        End Try
    End Sub
End Class
