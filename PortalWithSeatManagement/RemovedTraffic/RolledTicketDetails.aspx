<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="RolledTicketDetails.aspx.vb" Inherits="RolledTicketDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Rolled Back Ticket Details</title>
    <script src="Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="/NewBusRoute.aspx" onclick="" class="toolbar">
                        <span class="icon-32-new"></span>New</a>
                    </li>
                   
                    <li class="divider"></li>
                    <li class="button" id="pos">
                        <a href="/POSTicketing.aspx" onclick="" class="toolbar">
                        <span class="icon-32-pos">
                            <img src="/Styles/icon-48-media.png" width="32px" />
                        </span>POS Ticketing</a>  
                    </li>
                    <li class="button" id="booking">
                        <a href="/POSBooking.aspx" onclick="" class="toolbar">
                        <span class="icon-32-booking">
                            <img src="/Styles/icon-48-new-privatemessage.png" width="32px"/>
                        </span>Booking</a>  
                    </li>
                    <li class="button" id="Li2">
                        <a href="/POSSubscription.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="/Styles/icon-32-inbox.png" width="32px"/>
                        </span>Subscriptions</a>  
                    </li>
                    <li class="button" id="promo">
                        <a href="/POSPromotions.aspx" onclick="" class="toolbar">
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
            <div class="pagetitle icon-48-query">
                <h2>View Ticket Details <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="/Traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="/All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Rolled-Back Ticket Details</a>	</li>
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
                            <table>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            
                                <tr><td><b>Bus Name:</b></td><td> <asp:Label ID="lblBus" Text='<%# DataBinder.Eval(Container.DataItem, "BusName")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Circuit:</b> </td><td><asp:Label ID="lblCircuit" Text='<%# DataBinder.Eval(Container.DataItem, "Circuit")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>TicketID:</b> </td><td><i><asp:Label ID="lblTicketID" Text='<%# DataBinder.Eval(Container.DataItem, "IDRELATION")%>' runat="server"></asp:Label></i></td></tr>
                                <tr><td><b>Traveler:</b> </td><td><asp:Label ID="lblFullName" Text='<%# DataBinder.Eval(Container.DataItem, "Traveler")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Client Code: </b></td><td><asp:Label ID="lblClientCode" Text='<%# DataBinder.Eval(Container.DataItem, "ClientCode")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Date Of Travel:</b> </td><td><asp:Label ID="lblDate" Text='<%# DataBinder.Eval(Container.DataItem, "DateOfTravel")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Hour: </b></td><td><asp:Label ID="lblHour" Text='<%# DataBinder.Eval(Container.DataItem, "Hour")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Price: </b></td><td><asp:Label ID="lblPrice" Text='<%# DataBinder.Eval(Container.DataItem, "Total")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Reason For Deletion: </b></td><td><asp:Label ID="lblReason" Text='<%# DataBinder.Eval(Container.DataItem, "Username")%>' runat="server"></asp:Label></td></tr>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                                
                    </fieldset>
                        </div>
                </div>
                <p></p>
            </div>
           
           <div class="cpanel-right">

               <div style="background:white">
                   <p>This tickets were marked for deletion because they were identified as double tickets. Consult Administrator for further information</p>
                   <asp:Literal ID="ltrErrorMessage" runat="server"></asp:Literal>
               </div>
           </div>
           
        </div>
    </div>
</asp:Content>

