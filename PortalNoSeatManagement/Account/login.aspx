<%@ Page Title="" Language="VB" MasterPageFile="~/Account/sinoouter.master" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="Account_login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Sinnovys Portal | Login</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
      <div id="element-box" class="login">
                 
                <div class="m wbg">
                    <h1>Sinnovys Portal Login <asp:Literal ID="moreText" runat="server"></asp:Literal></h1>
                    <div id="section-box">
                        <div class="m">
                            <div id="errorDiv" style="color:red;font-weight:bold;width:230px;margin:auto;">
                            <asp:Literal ID="ltrError" runat="server"></asp:Literal>
                        </div>
                            <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
                                <LayoutTemplate>
                                    <span class="failureNotification">
                                        <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                    </span>
                                    <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                                         ValidationGroup="LoginUserValidationGroup"/>
                                    <div class="accountInfo">
                                        <fieldset class="loginform">
                                            
                                                <asp:Label ID="usernamelbl" runat="server" AssociatedControlID="UserName" ClientIDMode="Static">Username:</asp:Label>
                                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" ClientIDMode="Static"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                                                     CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required." 
                                                     ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                                <asp:Regularexpressionvalidator id="nameRegex" runat="server" 
                                                    ControlToValidate="Username" 
                                                    ValidationExpression="[a-zA-Z'.'-'\s]{1,40}" 
                                                    ErrorMessage="Invalid Username. Only Letters Allowed">
                                                </asp:Regularexpressionvalidator>
                                            
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" ClientIDMode="Static">Password:</asp:Label>
                                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password" ClientIDMode="Static" MaxLength="20"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                                                     CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required." 
                                                     ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                            
                                                
                                                <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline" ClientIDMode="Static">Keep me logged in</asp:Label>
                                                <asp:CheckBox ID="RememberMe" runat="server" ClientIDMode="Static"/>
                                                <div class="button-holder">
					                        <div class="button1">
						                        <div class="next">
							                        <a href="#" onclick="document.getElementById('sinovys').submit();">
								                        Log in</a>
                                                    <input type="submit" style="position: absolute; left: -9999px; width: 1px; height: 1px;"/>
						                        </div>
					                        </div>
				                        </div>
                                            
                                        </fieldset>

                                        
                                    </div>
                                </LayoutTemplate>
                            </asp:Login>
                        </div>
                        
                    </div>
                    <p><asp:Literal ID="introText" runat="server" Text="Use a valid username and password to gain access to the portal."></asp:Literal></p>
                    <p><a href="#">Go to site home page.</a></p>
                    <div id="lock"></div>
                </div>
                <div class="login-below" style="background:white;clear:both;margin-top:15px;">
                    <img src="../Styles/PoweredBy.jpg" alt="Yahoo Express"/>
                    
                </div>
            </div>
</asp:Content>

