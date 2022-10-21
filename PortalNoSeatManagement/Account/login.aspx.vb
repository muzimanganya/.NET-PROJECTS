Imports System.Data
Imports System.Data.SqlClient

Partial Class Account_login
    Inherits System.Web.UI.Page
    Private clsDatalayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myEnc As New myEncryption
        If Page.IsPostBack Then
            Dim username As String = Request("ctl00$body$LoginUser$UserName").ToString
            Dim password As String = Request("ctl00$body$LoginUser$Password").ToString
            Dim remm As String = Request("ctl00$body$LoginUser$RememberMe")
            Dim theRoles As String = ""
            Dim logString As String = ""
            'Log Every Attempt so we know when people are messing around with us
            Try
                'Just so we dnt break anything up here
                logString = String.Format("Loggin Attempt: Username: {0}, Password: {1}", myEncryption.Encrypt(username, "Ey5TXu2WLMKvpa"), myEncryption.Encrypt(password, "Ey5TXu2WLMKvpa"))
            Catch ex As Exception
                clsDatalayer.LogException(ex)
            End Try
            'Sanitize the Users Input
            'Check the Length, Check for HTML, Redirect back on failure
            If username.Length > 25 Or password.Length > 25 Then
                'Length Check Has Failed
                logString = " FAILED (SANITATION): " + logString
                clsDatalayer.LogString(logString)
                ltrError.Text = "Wrong UserName or Password"
                Return
            Else
                'Sanitize
                username = SanitizeInput(username)
                'password = SanitizeInput(password)
            End If
            'Get available users from database

            'Dim queryUsers As String = String.Format("SELECT * FROM dbo.Portal_Users WHERE username='{0}' ", username)
            'Update to Secure against SQL Injection
            Dim dtU As New DataTable
            Using acon As SqlConnection = clsDatalayer.ReturnConnection()
                Using cmd As SqlCommand = acon.CreateCommand()
                    'cmd.Connection = acon
                    cmd.CommandText = "SELECT * FROM dbo.Portal_Users WHERE username=@username"
                    Dim parameter = cmd.CreateParameter()
                    parameter.DbType = DbType.String
                    parameter.Direction = ParameterDirection.Input
                    parameter.ParameterName = "@username"
                    parameter.Value = username
                    cmd.Parameters.Add(parameter)

                    dtU.Load(cmd.ExecuteReader())
                End Using
            End Using
            'Dim dtU As DataTable = clsDatalayer.ReturnDataTable(queryUsers)
            If dtU.Rows.Count > 0 Then
                With dtU.Rows(0)
                    'Check that the password has not expired yet
                    Dim queryExpired = String.Format("SELECT DATEDIFF(d,LastChange,GETDATE()) AS 'DaysRemaining' FROm dbo.Portal_Users WHERE Username='{0}'", username)
                    Dim daystoGo = clsDatalayer.ReturnSingleNumeric(queryExpired)
                    Session("DaysToGo") = daystoGo
                    If daystoGo >= 29 Then
                        'The password has expired and the user needs to change it right now
                        Session("Username") = username
                        Response.Redirect("PasswordExpired.aspx")
                    End If
                    Dim EncruptedPassword As String = .Item("Password")

                    'Can user hide tickets aka Agaba
                    Dim CanHideTickets As String = .Item("CanHideTickets")
                    Session("CanHideTickets") = CanHideTickets

                    Dim decryptedPassword As String = myEncryption.Decrypt(EncruptedPassword, "Ey5TXu2WLMKvpa")
                    If decryptedPassword = password Then
                        'Okay, we create the tickets, store the roles , etc
                        Dim ticket As New FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(30), True, .Item("Roles").ToString, FormsAuthentication.FormsCookiePath)
                        'Now lets encrypt the cookie
                        Dim hash As String = FormsAuthentication.Encrypt(ticket)
                        Dim cookie As New HttpCookie(FormsAuthentication.FormsCookieName, hash)
                        theRoles = .Item("Roles").ToString
                        If remm = "on" Then
                            'Persistent Log in
                            cookie.Expires = ticket.Expiration
                        End If
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

                        If theRoles.Contains("restricted") Then
                            'Prevent a loop when access is denied
                            returnurl = "/All-Days--Planned-Traffic.aspx"
                        End If
                        'Now for the Login Trail
                        logString = "SUCCESSFULL: " + logString
                        clsDatalayer.LogString(logString)
                        Response.Redirect(returnurl)
                    Else
                        logString = "FAILED (BAD PASSWORD): " + logString
                        clsDatalayer.LogString(logString)
                        ltrError.Text = "Wrong Username or Password. Try Again"
                    End If
                End With
            Else
                'Username does not exist
                logString = "FAILED (USERNAME DOES NOT EXIST): " + logString
                clsDatalayer.LogString(logString)
                ltrError.Text = "Wrong Username or Password. Try Again"
            End If
        Else
            'Page is not in postback
            If Request.IsAuthenticated Then
                'Access was denied before
                moreText.Text += " | Access Denied"
                introText.Text = "Please log in with a username which has been granted access rights."
            End If
        End If

        Page.Title = "Login | Sinnovys Portal"
    End Sub
    Private Function SanitizeInput(input As String) As String
        Dim badCharReplace As New Regex("([<>""'%;()&])")
        Dim goodChars As String = badCharReplace.Replace(input, "")
        Return goodChars
    End Function

End Class
