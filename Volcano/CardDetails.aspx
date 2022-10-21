<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="CardDetails.aspx.vb" Inherits="CardDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Card Details</title>
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
            <div class="pagetitle icon-48-query">
                <h2>View Card Details <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="#">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Card Details</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                    <div style="background:white">
                <fieldset id="queryForm" title="Ticket Details">
                    <asp:Repeater ID="ticketDetails" runat="server">
                        <HeaderTemplate>
                            <table style="width:280px;float:left">
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            
                                <tr><td><b>Customer Name:</b></td><td> <asp:Label ID="lblBus" Text='<%# DataBinder.Eval(Container.DataItem, "Traveler")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Bus Route:</b> </td><td><asp:Label ID="lblCircuit" Text='<%# DataBinder.Eval(Container.DataItem, "Circuit")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Card No:</b> </td><td><i><asp:Label ID="lblTicketID" Text='<%# DataBinder.Eval(Container.DataItem, "IDCUSTOM1")%>' runat="server"></asp:Label></i></td></tr>
                                <tr><td><b>Date Bought:</b> </td><td><asp:Label ID="lblFullName" Text='<%# DataBinder.Eval(Container.DataItem, "DateSold")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Trips Remaining: </b></td><td><asp:Label ID="lblClientCode" Text='<%# DataBinder.Eval(Container.DataItem, "Trips")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Subscription Status:</b> </td><td><asp:Label ID="lblDate" Text='<%# DataBinder.Eval(Container.DataItem, "Status")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Price: </b></td><td><asp:Label ID="lblHour" Text='<%# DataBinder.Eval(Container.DataItem, "Price")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>POS Sold: </b></td><td><asp:Label ID="lblPrice" Text='<%# DataBinder.Eval(Container.DataItem, "POS")%>' runat="server"></asp:Label></td></tr>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <div style="width:300px;float:right">
                        <asp:Image ID="imgQr" runat="server" AlternateText="Loading QR Code" />
                    </div>
                   
                                
                    </fieldset>
                        </div>
                </div>
                <p></p>
            </div>
           
           <div class="cpanel-right">

               <div style="background:white">
                   <asp:Literal ID="ltrErrorMessage" runat="server"></asp:Literal>
                   <div id="chart_div" style="width:570px;height:300px">
                       <div style="margin:auto;width:250px" ><img src="Styles/loadingcircle.gif" alt="Loading Chart"/></div>
                   </div>
               </div>
           </div>
           
        </div>
        <div class="m">
             <asp:Repeater ID="rptHistory" runat="server" >
                <HeaderTemplate>
                    <table id="masterview">
                        <thead>
                        <tr>
                            <th><a href="#">City In</a></th>
                            <th><a href="#">City Out</a></th>
                            <th><a href="#">TicketID</a></th>
                            <th><a href="#">Discount</a></th>
                            <th><a href="#">Total</a></th>
                            <th><a href="#">Subscription No</a></th>
                            <th><a href="#">Client Code</a></th>
                            <th><a href="#">Created On</a></th>
                            <th><a href="#">Created By</a></th>
                        </tr>
                            </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "CityIN")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "CityOut")%></td>
                        <td align="left"><a href="TicketDetails.aspx?TicketID=<%# DataBinder.Eval(Container.DataItem, "IDRELATION")%>"><%# DataBinder.Eval(Container.DataItem, "IDRELATION")%></a>
                            <p class="smallsub">
                            
                            </p>
                        </td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Discount")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Total")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Sno")%></td>
                        <td align="left"><a href="ClientCodeDetails.aspx?ClientCode=<%# DataBinder.Eval(Container.DataItem, "ClientCode")%>"><%# DataBinder.Eval(Container.DataItem, "ClientCode")%></a>
                            <p class="smallsub">
                            
                            </p>
                        </td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "CreatedOn")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "CreatedBy")%></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>

