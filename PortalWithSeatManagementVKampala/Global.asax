<%@ Application Language="VB" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Security.Principal" %>

<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        Application("chat") = ""
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        '' Code that runs when an unhandled error occurs
        ''Redirect to a Custom Error Page and Log the Errors as they occur
        'SqlConnection.ClearAllPools()
        'Server.ClearError()
        'Response.Redirect("/CustomError.aspx")
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        Dim cfg As New Config
        Session("Username") = ""
        Session("DaysToGo") = ""
        Session("FIB") = cfg.BWF
        Session("RWF") = cfg.USD
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

    Protected Sub Application_BeginRequest(ByVal sender As Object, ByVal e As System.EventArgs)
        'if (Request.AppRelativeCurrentExecutionFilePath == "~/")  
        'HttpContext.Current.RewritePath("HomePage.aspx");
        If Request.AppRelativeCurrentExecutionFilePath = "/" Then
            HttpContext.Current.RewritePath("Default.aspx")
        End If
    End Sub
    Protected Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As System.EventArgs)
        If HttpContext.Current.User IsNot Nothing Then
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                If TypeOf HttpContext.Current.User.Identity Is FormsIdentity Then
                    Dim id As FormsIdentity = DirectCast(HttpContext.Current.User.Identity, FormsIdentity)
                    Dim ticket As FormsAuthenticationTicket = id.Ticket

                    ' Get the stored user-data, in this case, our roles
                    Dim userData As String = ticket.UserData
                    Dim roles As String() = userData.Split(","c)
                    HttpContext.Current.User = New GenericPrincipal(id, roles)
                End If
            End If
        End If
    End Sub
</script>