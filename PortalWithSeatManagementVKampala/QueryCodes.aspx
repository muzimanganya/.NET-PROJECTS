<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="QueryCodes.aspx.vb" Inherits="QueryCodes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Query Codes</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="NewBusRoute.aspx" onclick="" class="toolbar">
                        <span class="icon-32-new"></span>New</a>
                    </li>
                   
                    <li class="divider"></li>
                    <li class="button" id="pos">
                        <a href="POSTicketing.aspx" onclick="" class="toolbar">
                        <span class="icon-32-pos">
                            <img src="Styles/icon-48-media.png" width="32px" />
                        </span>POS Ticketing</a>  
                    </li>
                    <li class="button" id="booking">
                        <a href="POSBooking.aspx" onclick="" class="toolbar">
                        <span class="icon-32-booking">
                            <img src="Styles/icon-48-new-privatemessage.png" width="32px"/>
                        </span>Booking</a>  
                    </li>
                    <li class="button" id="Li2">
                        <a href="POSSubscription.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="Styles/icon-32-inbox.png" width="32px"/>
                        </span>Subscriptions</a>  
                    </li>
                    <li class="button" id="promo">
                        <a href="POSPromotions.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="Styles/icon-48-checkin.png" width="32px" />
                        </span>Promotions</a>  
                    </li>
                    <li class="divider"></li>
                    <li class="button" id="Li1">
                        <a href="#" onclick="" class="toolbar">
                        <span class="icon-32-help"></span>Help</a>  
                    </li>
                </ul>

            </div>
            <div class="pagetitle icon-48-query">
                <h2>Query Codes <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="#">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Query Codes (TicketID & ClientCode)</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                <fieldset id="queryForm" title="Query The Database">
                  <ul class="queryList">
                    <li>
                        <asp:Label id="lblModule" for="ddlModules" title="" runat="server" ClientIDMode="Static">Select Module<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlModules" runat="server" CssClass="required" ClientIDMode="Static">
                                <asp:ListItem Text="Select A Module to Query For" Value="QueryCodes.aspx?#"></asp:ListItem>
                                <asp:ListItem Text="Query A TicketID" Value="TicketDetails.aspx?TicketID="></asp:ListItem>
                                <asp:ListItem Text="Query A Card No" Value="CardDetails.aspx?cardno="></asp:ListItem>
                                <asp:ListItem Text="Query A Bus ID" Value="Bus-Details.aspx?idsale="></asp:ListItem>
                                <asp:ListItem Text="Query A ClientCode" Value="ClientCodeDetails.aspx?ClientCode="></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvModule" runat="server" ErrorMessage="Please Select a Query" ControlToValidate="ddlModules" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblDate" for="txtDate" title="" runat="server" ClientIDMode="Static">Enter The Code<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" CssClass="required" AutoCompleteType="None"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Please Select a Code to Query" ControlToValidate="txtDate" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revNmbers" runat="server" ErrorMessage="Only Numbers Allowed" ControlToValidate="txtDate" 
                                ValidationExpression="^[0-9 ]+$" Text="Only Numbers Allowed">
                            </asp:RegularExpressionValidator>
                        </div>
                    </li>
                    <div class="clr">
                    </div>
                    <p></p>
                    <li>
                        <asp:Button ID="btnSend" runat="server" Text="Query Database" />
                    </li>
                  </ul>
                    </fieldset>
                </div>
                <p></p>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"><div id="piechart" style="min-height:80px;width:570px;font-size:14px;color:Blue;"><br /><br />Please Select a Module and The Target Date to Query</div>
                    <p>
                        <asp:ValidationSummary ID="vsQuery" runat="server" Font-Bold="True" 
                            ForeColor="Red" />
                    </div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

