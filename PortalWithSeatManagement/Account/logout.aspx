<%@ Page Title="" Language="VB" MasterPageFile="~/Account/sinoouter.master" AutoEventWireup="false" CodeFile="logout.aspx.vb" Inherits="Account_logout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<title>You Have Been Logged Out</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
  <div id="element-box" class="login">
                <div class="m wbg">
                    <h1>Sinnovys Portal Logout</h1>
                    <div id="section-box">
                        <div class="m">
                            <p>You Have Been Successfully Logged Out.</p>
                            <p><a href="/Account/login.aspx">Go to site Login page.</a></p>
                        </div>
                        
                    </div>
                    <p>Use a valid username and password to gain access to the portal
                    .</p>
                    
                    <div id="lock"></div>
                </div>
            </div>
</asp:Content>

