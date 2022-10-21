<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Bus-Share.aspx.vb" Inherits="Busshare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Bus Sharing</title>
    <link href="Styles/gridview.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            color: red;
            font-weight: bold;
        }
        table.seatstable {
            border:1px solid blueviolet;
        }
        table.seatstable td {
            padding-left:20px !important;
            background:white;
        }
        table.seatstable tr td{
            background:white !important;
        }
        table.seatstable tr:hover {
            background:white !important;
            }
    </style>
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
            <div class="pagetitle icon-48-traffic">
                <h2>Bus Seats Sharing: <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m">
				<ul id="submenu">
	            	<li><a  href="Traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="QueryDB.aspx">Query For Traffic</a>	</li>
                    <li><a href="#" class="active">Bus Seats Sharing</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div id="seatsReport" style="width:37%;float:left;margin-right:30px;">
                <table cellpadding="10px" cellspacing="10px" width="100%" class="seatstable">
                    <thead>
                        <tr>
                            <th>Company</th>
                            <th>Current Capacity</th>
                            <th>Current Free Seats</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Belvedere</td>
                            <td><asp:Label ID="lblBCapacity" runat="server" Text="Capacity"></asp:Label></td>
                            <td><asp:Label ID="lblBFree" runat="server" Text="Free"></asp:Label></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Kigali Safaris</td>
                            <td><asp:Label ID="lblKCapacity" runat="server" Text="Capacity"></asp:Label></td>
                            <td><asp:Label ID="lblKFree" runat="server" Text="Free"></asp:Label></td>
                            <td>
                                <%If ksStatus Then%>
                                    <asp:Button ID="btnRequestSeats" runat="server" Text="Request Seats" />
                                <%End If%>
                            </td>
                        </tr>
                    </tbody>
                </table>
           </div>
            <div id="requests" style="width:60%;float:right">
                <div id="panel-sliders" class="pane-sliders">
                    <div class="panel">
                        <h3 class="title pane-toggler-down" id="cpanel-panel-logged" onclick="javascript:HandleToggles(1);"><a href="javascript:HandleToggles(1);"><span>Requests to Kigali Safaris</span></a></h3>
                        <div class="one" style="padding:5px 10px; border-top-style: none; border-bottom-style: none; overflow: hidden; height: auto;">
                            <asp:Repeater ID="rptToKS" runat="server">
                                <HeaderTemplate>
                                    <table width="100%">
                                        <thead>
                                            <th>Request ID</th>
                                            <th>Originating User</th>
                                            <th>Seats Requested</th>
                                            <th>Status</th>
                                            <th>Created On</th>
                                            <th>Request Comment</th>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <a href='RequestDetails.aspx?RequestID=<%# Eval("RequestID")%>'>
                                                <%# Eval("RequestID")%>
                                            </a>
                                        </td>
                                        <td><%# Eval("FromUsername")%></td>
                                        <td><%# Eval("SeatsNo")%></td>
                                        <td><%# Eval("Status")%></td>
                                        <td><%# Eval("CreatedOn")%></td>
                                        <td><%# Eval("RequestComment")%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                   <div class="panel">
                        <h3 class="title pane-toggler-down" id="H2" onclick="javascript:HandleToggles(2);"><a href="javascript:HandleToggles(2);"><span>Requests From Kigali Safaris</span></a></h3>
                        <div class="two" style="padding:5px 10px; border-top-style: none; border-bottom-style: none; overflow: hidden; height: auto;">
                            <asp:Repeater ID="rptFromKS" runat="server">
                                <HeaderTemplate>
                                    <table width="100%">
                                        <thead>
                                            <th>Request ID</th>
                                            <th>Originating User</th>
                                            <th>Seats Requested</th>
                                            <th>Status</th>
                                            <th>Created On</th>
                                            <th>Request Comment</th>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <a href='/RequestDetails.aspx?RequestID=<%# Eval("RequestID")%>'>
                                                <%# Eval("RequestID")%>
                                            </a>
                                        </td>
                                        <td><%# Eval("FromUsername")%></td>
                                        <td><%# Eval("SeatsNo")%></td>
                                        <td><%# Eval("Status")%></td>
                                        <td><%# Eval("CreatedOn")%></td>
                                        <td><%# Eval("RequestComment")%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>    
            </div>
                
            </div>
        </div>
    </div>
</asp:Content>

