<%@ Page Title="" Language="VB" MasterPageFile="~/Account/sinoouter.master" AutoEventWireup="false" CodeFile="PasswordExpired.aspx.vb" Inherits="Account_passwordExpired" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Sinnovys Portal | Password Expired</title>
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
                                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                            
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" ClientIDMode="Static">New Password:</asp:Label>
                                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="Password" 
                                                ErrorMessage="Must Use A Strong Password: <ul><li>Must Be Atleast 8 Characters Long</li><li>Atleast 1 Upper Case Letter</li><li>Atleast 1 Lowercase Letter</li><li>Must Contain atleast 1 number</li></ul>" 
                                                
                                                ValidationExpression="(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$" ForeColor="Red">
                                                *
                                            </asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                                                     CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required." 
                                                     ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                            <asp:PasswordStrength ID="psMeasure" runat="server" TargetControlID="Password" MinimumNumericCharacters="1" MinimumUpperCaseCharacters="1" DisplayPosition="BelowLeft" StrengthIndicatorType="Text" ></asp:PasswordStrength>

                                            <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="txtNewPassword" ClientIDMode="Static">Confirm New Password:</asp:Label>
                                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="passwordEntry" TextMode="Password" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewPassword" 
                                                     CssClass="failureNotification" ErrorMessage="New Password is required." ToolTip="New Password is required." 
                                                     ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cv" runat="server" ErrorMessage="The New Password Should Match" ForeColor="red" ControlToValidate="Password" ControlToCompare="txtNewPassword">*</asp:CompareValidator>
                                                
                                                <div class="button-holder">
					                        <div class="button1">
						                        <div class="next">
							                        <a href="#" onclick="document.getElementById('sinovys').submit();">
								                        Change Password</a>
                                                    <input type="submit" style="position: absolute; left: -9999px; width: 1px; height: 1px;"/>
						                        </div>
					                        </div>
				                        </div>
                                            
                                        </fieldset>
                                    </div>
                                    <asp:ValidationSummary ID="vsErrors" runat="server" ForeColor="Red" CssClass="m" />
                                </LayoutTemplate>
                            </asp:Login>
                        </div>
                        
                    </div>
                    <p><asp:Literal ID="introText" runat="server" Text="Use a valid username and password to gain access to the portal."></asp:Literal></p>
                    <p><a href="#">Go to site home page.</a></p>
                    <div id="lock"></div>
                </div>
                
               <%-- <div class="m" style="margin-top:5px;">
                    <p>Why Does Your Password Expire Often?</p>
                    <p style="font-size:120%">Your Password may Expire Often if you set a simple password such as numbers only, of that includes your username. Please select a complex password including numbers & special characters</p>
                </div>--%>
                <div class="login-below" style="background:white;clear:both;margin-top:15px;">
                    <img src="../styles/PoweredBy.jpg" alt="Volcano Express"/>
                    
                </div>
            </div>
</asp:Content>

