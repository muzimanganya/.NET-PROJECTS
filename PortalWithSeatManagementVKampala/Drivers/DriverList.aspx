<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="DriverList.aspx.vb" Inherits="DriverList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Driver List</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="DriverEntry.aspx" onclick="" class="toolbar">
                        <span class="icon-32-new"></span>New Driver</a>
                    </li>
                   
                    <li class="divider"></li>
                    <li class="button" id="pos">
                        <a href="POSTicketing.aspx" onclick="" class="toolbar">
                        <span class="icon-32-pos">
                            <img src="/Styles/icon-48-media.png" width="32px" />
                        </span>POS Ticketing</a>  
                    </li>
                    <li class="button" id="booking">
                        <a href="POSBooking.aspx" onclick="" class="toolbar">
                        <span class="icon-32-booking">
                            <img src="/Styles/icon-48-new-privatemessage.png" width="32px"/>
                        </span>Booking</a>  
                    </li>
                    <li class="button" id="Li2">
                        <a href="POSSubscription.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="/Styles/icon-32-inbox.png" width="32px"/>
                        </span>Subscriptions</a>  
                    </li>
                    <li class="button" id="promo">
                        <a href="POSPromotions.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="/Styles/icon-48-checkin.png" width="32px" />
                        </span>Promotions</a>  
                    </li>
                    <li class="divider"></li>
                    <li class="button" id="Li1">
                        <a href="#" onclick="" class="toolbar">
                        <span class="icon-32-help"></span>Help</a>  
                    </li>
                </ul>

            </div>
            <div class="pagetitle icon-48-user">
                <h2>Driver List<asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="/Traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="/All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Driver List</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left" style="width:99% !important">
                <div class="m">
                    <asp:Repeater ID="rptDrivers" runat="server">
                        <ItemTemplate>
                            <div class="driver" style="width:32%;float:left;margin-bottom:20px">
                                <div style="padding-left:5px;display:block">
                                    <asp:Image ID="imgpPic" runat="server" Width="80px" Height="70px" ImageUrl='<%# "~/Drivers/image.aspx?photo=" + Eval("Photo")%>' 
                                        style="float:left;border:1px solid white;border-radius:34px;margin-top:30px"/>
                                    <div class="drvInfo" style="float:left;margin-left:10px">
                                        <h3><a href='DriverEntry.aspx?DriverId=<%# Eval("DriverID") %>'><%# Eval("DriverName") %></a></h3>
                                        <p><b>Phone:</b> <%# Eval("Phone") %></p>
                                        <p><b>Email:</b> <%# Eval("Email") %></p>
                                        <p><b>Address:</b> <%# Eval("Address") %></p>    
                                        
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <p></p>
            </div>
        </div>
    </div>
</asp:Content>

