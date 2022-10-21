<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="traffic.aspx.vb" Inherits="traffic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Traffic & Business</title>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
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
                <h2>Traffic & Business <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a class="active" href="#">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="QueryDB.aspx">Query For Traffic</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                    <asp:UpdatePanel ID="upMainView" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                           
                            <asp:Repeater ID="rptMasterView" runat="server">
                <HeaderTemplate>
                    <table id="masterview">
                        <tr>
                            <th><a href="#">Bus Route Name</a></th>
                            <th><a href="#">Total Tickets</a></th>
                            <th><a href="#">Total Booking</a></th>
                            <th><a href="#">Total Promotions</a></th>
                            <th><a href="#">Total POS Used</a></th>
                            <th><a href="#">Total RWF</a></th>
                            <th><a href="#">Total UGX</a></th>
                            <th><a href="#">Total FIB</a></th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="left"><a href="TrafficDetails.aspx?Route=<%# DataBinder.Eval(Container.DataItem, "BusRoute")%>&requestDate=<%# Request.QueryString("requestDate") %>"><%# DataBinder.Eval(Container.DataItem, "BusRoute")%></a>
                            <p class="smallsub">
                            
                            </p>
                        </td>

                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "TotalTickets")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "TotalBooking")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "TotalPromo")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "TotalPos")%></td>
                        <td style="font-weight:bold;text-align:right;font-style:italic"><%# DataBinder.Eval(Container.DataItem, "TotalRWF", "{0:n2}")%></td>
                        <td style="font-weight:bold;text-align:right;font-style:italic"><%# DataBinder.Eval(Container.DataItem, "TotalUGX", "{0:n2}")%></td>
                        <td style="font-weight:bold;text-align:right;font-style:italic"><%# DataBinder.Eval(Container.DataItem, "TotalFIB", "{0:n2}")%></td>
                    </tr>
                </ItemTemplate>
                
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater> 
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="tmrMainView" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>

                      <asp:Timer ID="tmrMainView" runat="server" Interval="10000"></asp:Timer>
                    
                </div>
                <p></p>
                <div>
                    <asp:UpdatePanel ID="upTotals" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                             <asp:Repeater ID="rptTotals" runat="server">
                                <ItemTemplate>
                                    <table id="footerview" style="margin-top:-23px">
                                        
                                        <tr style="float:none !important">
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td style="font-weight:bolder;color:black"><span>#Tickets: </span><em><%# DataBinder.Eval(Container.DataItem, "TotalTickets")%></em></td>
                                            <td style="font-weight:bolder;color:black"><span>#Bookings: </span><em><%# DataBinder.Eval(Container.DataItem, "TotalBooking")%></em></td>
                                            <td style="font-weight:bolder;color:black"><span>#Promotions: </span><em><%# DataBinder.Eval(Container.DataItem, "TotalPromo")%></em></td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        </tr>                                    
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                            <br />
                           <asp:Repeater ID="rptSummary" runat="server">
                                <ItemTemplate>
                                   <div style="float:right;font-size:14px;color:red;font-weight:bolder;">
                                       Total Sales Done On: <i><%# DataBinder.Eval(Container.DataItem, "TDATE")%></i> = 
                                       UGX <%# DataBinder.Eval(Container.DataItem, "TotalUGX", "{0:n2}")%>
                                       RWF <%# DataBinder.Eval(Container.DataItem, "TotalRWF", "{0:n2}")%>
                                       FIB <%# DataBinder.Eval(Container.DataItem, "TotalFIB", "{0:n2}")%>
                                   </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="rptTotalForDay" runat="server">
                                <ItemTemplate>
                                    <br /><br />
                                   <div style="float:right;font-size:14px;color:blue;font-weight:bolder;">
                                       Total Sales For: <i><%# DataBinder.Eval(Container.DataItem, "TDATE")%></i> = 
                                       RWF <%# DataBinder.Eval(Container.DataItem, "TotalRWF", "{0:n2}")%>
                                       UGX <%# DataBinder.Eval(Container.DataItem, "TotalUGX", "{0:n2}")%>
                                       FIB <%# DataBinder.Eval(Container.DataItem, "TotalFIB", "{0:n2}")%>
                                   </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="tmrUpdateRecords" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:Timer ID="tmrUpdateRecords" runat="server" Interval="10000">
                            </asp:Timer>
                </div>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"><div id="piechart" style="min-height:430px;width:570px;">Pie Chart Comes Here</div></div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

