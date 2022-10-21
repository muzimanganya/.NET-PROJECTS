<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Couriers.aspx.vb" Inherits="Couriers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Courier Management</title>
    <link href="/Styles/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <%--<script type="text/javascript" src="https://www.google.com/jsapi"></script>--%>
    <script src="../Scripts/jsapi.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="NewCourier.aspx" onclick="" class="toolbar">
                        <span class="icon-32-new"></span>New</a>
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
            <div class="pagetitle icon-48-traffic">
                <h2>Courier Management <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a class="active" href="#">Courier Mangement</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="QueryDB.aspx">Query For Traffic</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m" style="padding-top: 0px;">

                    <div id="position-icon" class="pane-sliders" style="">
                        <div class="panel">
                            <h3 class="title pane-toggler-down" onclick="javascript:HandleToggles(1);" id="module9"><a href="#"><span>Unassigned Couriers</span></a></h3>
                            <div class="one"  style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;">
                                <div class="cpanel">

                                    <asp:Repeater ID="rptData" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover">
                                                <tr>
                                                    <th>Courier ID</th>
                                                    <th>Route</th>
                                                    <th>Sender</th>
                                                    <th>SPhone</th>
                                                    <th>Receiver</th>
                                                    <th>RPhone</th>
                                                    <th>Status</th>
                                                    <th>Creator</th>
                                                </tr>
                                            
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <a href='CourierDetails.aspx?CourierID=<%# Eval("CourierID")%>'><%# Eval("CourierID")%></a>
                                                </td>
                                                <td><%# Eval("CityIn")%>-<%# Eval("CityOut")%></td>
                                                <td><%# Eval("SenderName")%></td>
                                                <td><%# Eval("SenderPhone")%></td>
                                                <td><%# Eval("ReceiverName")%></td>
                                                <td><%# Eval("ReceiverPhone")%></td>
                                                <td><%# Eval("StatusName")%></td>
                                                <td><%# Eval("Creator")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>

                                </div>
                            </div>
                        </div>

                        <div class="panel">
                            <h3 class="title pane-toggler-down" onclick="javascript:HandleToggles(2);" id="H1"><a href="#"><span>All Couriers</span></a></h3>
                            <div class="two"  style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto; display:none">
                                <div class="cpanel">
                                    <asp:Repeater ID="rptAllCouriers" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover">
                                                <tr>
                                                    <th>Courier ID</th>
                                                    <th>Route</th>
                                                    <th>Sender</th>
                                                    <th>SPhone</th>
                                                    <th>Receiver</th>
                                                    <th>RPhone</th>
                                                    <th>Status</th>
                                                    <th>Creator</th>
                                                </tr>
                                            
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <a href='CourierDetails.aspx?CourierID=<%# Eval("CourierID")%>'><%# Eval("CourierID")%></a>
                                                </td>
                                                <td><%# Eval("CityIn")%>-<%# Eval("CityOut")%></td>
                                                <td><%# Eval("SenderName")%></td>
                                                <td><%# Eval("SenderPhone")%></td>
                                                <td><%# Eval("ReceiverName")%></td>
                                                <td><%# Eval("ReceiverPhone")%></td>
                                                <td><%# Eval("StatusName")%></td>
                                                <td><%# Eval("Creator")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"><div id="piechart" style="min-height:430px;width:570px;">Pie Chart Comes Here</div></div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="/Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        var one = 1;
        var two = 0;
        var three = 0;
        function HandleToggles(n) {
            if (n == 1) {
                $('div.two').slideUp('fast');
                $('div.one').fadeIn('slow');
            }
            else if (n == 2) {
                $('div.one').slideUp('fast');
                $('div.two').fadeIn('slow');
            }
        }
    </script>
</asp:Content>

