
<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Bus-Share-Request.aspx.vb" Inherits="BusShareRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Request Seats</title>
    
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
            <div class="pagetitle icon-48-new">
                <h2>New Seats Request<asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="QueryDB.aspx">Query For Traffic</a>	</li>
                    <li><a href="#" class="active" >Request for Seats</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                <fieldset id="queryForm" title="Request Seats From Kigali Safaris">
                  <ul class="queryList">
                    <li>
                        <asp:Label id="lblCityIN" for="ddlSeats" title="" runat="server" ClientIDMode="Static">How many Seats? <span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlSeats" runat="server" CssClass="required" ClientIDMode="Static">
                                
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvModule" runat="server" ErrorMessage="Please Select the Number of Seats" ControlToValidate="ddlSeats" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            
                        </div>
                    </li>
                    
                    <li>
                        <asp:Label id="lblComment" for="txtComment" title="" runat="server" ClientIDMode="Static">Any Comments?<span class="star">&nbsp;</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtComment" runat="server" ClientIDMode="Static" CssClass="required" TextMode="MultiLine" Rows="10" Columns="50"></asp:TextBox>
                        </div>
                    </li>
                   
                    <div class="clr">
                    </div>
                    <p></p>
                    <li>
                        <asp:Button ID="btnRequest" runat="server" Text="Request Seats!" />
                    </li>
                  </ul>
                    </fieldset>
                </div>
                <p></p>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <p style="color:Red;font-weight:bolder;font-size:larger;">
                            <asp:Literal ID="ltrError" runat="server"></asp:Literal>
                        </p>
                    <div class="panel"><div id="piechart" style="min-height:80px;width:570px;font-size:14px;color:Blue;"><br /><br />Please Fill the Form to Create A new Seats Request to <asp:Literal ID="sharedbusname" runat="server"></asp:Literal></div>
                    <p>
                        <asp:ValidationSummary ID="vsQuery" runat="server" Font-Bold="True" 
                            ForeColor="Red" />
                    </p>
                        <div style="background:white;width:570px">
                            <table cellpadding="10px" cellspacing="10px" width="100%" class="seatstable">
                                <thead>
                                    <tr>
                                        <th>Company</th>
                                        <th>Current Capacity</th>
                                        <th>Current Free Seats</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>Belvedere</td>
                                        <td><asp:Label ID="lblBCapacity" runat="server" Text="Capacity"></asp:Label></td>
                                        <td><asp:Label ID="lblBFree" runat="server" Text="Free"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Kigali Safaris</td>
                                        <td><asp:Label ID="lblKCapacity" runat="server" Text="Capacity"></asp:Label></td>
                                        <td><asp:Label ID="lblKFree" runat="server" Text="Free"></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

