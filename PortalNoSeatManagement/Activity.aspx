<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Activity.aspx.vb" Inherits="ClientCodeActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Client Code Activity</title>
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
                <h2>Client Code Activity <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a class="active" href="#">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="QueryDB.aspx">Client Code Activity</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                    <div id="filterExpression">
                        <asp:Label ID="lblFilter" runat="server" Text="Filter By Number of Tickets Today: "></asp:Label>
                        <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="Three Ticket +" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Five Ticket +" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Ten Ticket +" Value="10"></asp:ListItem>
                            <asp:ListItem Text="Fifteen Ticket +" Value="15"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:UpdatePanel ID="upMainView" runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="rptMasterView" runat="server">
                <HeaderTemplate>
                    <table id="masterview">
                        <tr>
                            <th><a href="#">Client Code</a></th>
                            <th><a href="#">Total Normal Tickets</a></th>
                            <th><a href="#">Total Booking</a></th>
                            <th><a href="#">Total Promotions</a></th>
                            <th><a href="#">Total POS Used</a></th>
                            <th><a href="#">Total Discount</a></th>
                            <th><a href="#">Total Amount</a></th>
                            
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="left"><a href="ClientCodeDetails.aspx?ClientCode=<%# DataBinder.Eval(Container.DataItem, "ClientCode")%>"><%# DataBinder.Eval(Container.DataItem, "ClientCode")%></a>
                            <p class="smallsub">
                            
                            </p>
                        </td>

                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "TotalNormal")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "TotalBooking")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "TotalPromo")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "TotalPos")%></td>
                        <td style="font-weight:bold;text-align:right;font-style:italic"><%# DataBinder.Eval(Container.DataItem, "TotalDiscount", "{0:n2}")%></td>
                        <td style="font-weight:bold;text-align:right;font-style:italic"><%# DataBinder.Eval(Container.DataItem, "TotalAmount", "{0:n2}")%></td>
                    </tr>
                </ItemTemplate>
                
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater> 
                        </ContentTemplate>
                    </asp:UpdatePanel>

                     
                    
                </div>
                <div class="m">
                    <div style="float:left;font-size:14px;color:blue;font-weight:bolder;margin-right:10px">Total Tickets: <asp:Literal ID="ltrTotalTickets" runat="server"></asp:Literal></div>
                    <div style="float:right;font-size:14px;color:blue;font-weight:bolder;">Total Discount: <asp:Literal ID="ltrTotalDiscount" runat="server"></asp:Literal></div>
                </div>
                <p></p>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"><div id="piechart" style="min-height:430px;width:570px;">Pie Chart Comes Here</div></div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

