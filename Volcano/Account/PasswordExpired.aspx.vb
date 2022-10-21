Imports System.Data

Partial Class Account_passwordExpired
    Inherits System.Web.UI.Page
    Private clsDatalayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            Dim username As String = Session("Username").ToString
            Dim password As String = Request("ctl00$body$LoginUser$Password").ToString
            Dim remm As String = Request("ctl00$body$LoginUser$RememberMe")
            Dim newPassword As String = Request("ctl00$body$LoginUser$txtNewPassword").ToString
            Dim theRoles As String = ""
            'Get available users from database
            Dim queryUsers As String = String.Format("SELECT * FROM dbo.Portal_Users WHERE username='{0}' ", username)
            Dim dtU As DataTable = clsDatalayer.ReturnDataTable(queryUsers)
            If dtU.Rows.Count > 0 Then
                With dtU.Rows(0)
                    If .Item("Username").ToString.ToUpper() = username.ToUpper() AndAlso myEncryption.Decrypt(.Item("Password"), "Ey5TXu2WLMKvpa") <> password Then
                        'Make sure they did not mess up the username on its way here.And that the password is not the same
                        'Now update the new password information
                        Dim query As String = String.Format("UPDATE dbo.Portal_Users SET password='{0}' WHERE username='{1}'", myEncryption.Encrypt(password, "Ey5TXu2WLMKvpa"), username)
                        clsDatalayer.executeCommand(query)
                        'After the password is changed, now create the required cookies
                        Dim ticket As New FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(45), True, .Item("Roles").ToString, FormsAuthentication.FormsCookiePath)
                        'Now lets encrypt the cookie
                        Dim hash As String = FormsAuthentication.Encrypt(ticket)
                        Dim cookie As New HttpCookie(FormsAuthentication.FormsCookieName, hash)

                        Response.Cookies.Add(cookie) ' Send it back
                        Dim returnurl As String = Request.QueryString("ReturnUrl")
                        If returnurl = String.Empty Or returnurl = "/" Then
                            If theRoles.Contains("charroi") Then
                                returnurl = "/All-Days--Planned-Traffic.aspx"
                            Else
                                returnurl = "/"
                            End If
                        Else
                            'Nothing for now
                        End If
                        Response.Redirect(returnurl)
                    Else
                        introText.Text = "Please put in a valid username, and a new password. You can not use the old password as the new password"
                        ltrError.Text = " You can not use the old password as the new password"
                        Dim t As TextBox = DirectCast(LoginUser.FindControl("UserName"), TextBox)
                        Dim s As String = Nothing

                        If t IsNot Nothing Then
                            ' textbox is in the current scope of the LoginView
                            t.Text = Session("Username")
                            ' the textbox is not in the current scope of the LoginView.
                        Else
                        End If
                    End If
                End With
            Else
                'Username does not exist
            End If
        Else
            ''Page is not in postback
            'If Request.IsAuthenticated Then
            '    'Access was denied before
            '    moreText.Text += " | Password Expired"
            '    introText.Text = "Please enter a valid new password and repeat it below to confirm it. Then click change password"
            'End If
        End If
        If Not Page.IsPostBack Then
            moreText.Text += " | <span style='color:blue'> Password Expired</span>"
            introText.Text = "Please enter a valid new password and repeat it below to confirm it. Then click change password"

            Dim t As TextBox = DirectCast(LoginUser.FindControl("UserName"), TextBox)
            Dim s As String = Nothing

            If t IsNot Nothing Then
                ' textbox is in the current scope of the LoginView
                t.Text = Session("Username")
                ' the textbox is not in the current scope of the LoginView.
            Else
            End If
        End If
        Page.Title = "Password Expired | Sinnovys Portal"
    End Sub
End Class
