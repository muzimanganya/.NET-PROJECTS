<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="RequestDetails.aspx.vb" Inherits="RequestDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Ticket Details</title>
    
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
                <h2>View Request Details <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="#">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Request Details</a>	</li>
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
                    <asp:Repeater ID="RequestDetails" runat="server">
                        <HeaderTemplate>
                            <table style="width:335px;float:left">
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            
                                <tr><td><b>Requesting User:</b></td><td> <asp:Label ID="lblRequestingUser" Text='<%# DataBinder.Eval(Container.DataItem, "FromUsername")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Bus Name:</b> </td><td><asp:Label ID="lblCircuit" Text='<%# DataBinder.Eval(Container.DataItem, "BusName")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>RequestID:</b> </td><td><i><asp:Label ID="lblTicketID" Text='<%# DataBinder.Eval(Container.DataItem, "RequestID")%>' runat="server"></asp:Label></i></td></tr>
                                <tr><td><b>Requested Seats:</b> </td><td><asp:Label ID="lblFullName" Text='<%# DataBinder.Eval(Container.DataItem, "SeatsNo")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Created On: </b></td><td><asp:Label ID="lblClientCode" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedOn")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Status:</b> </td><td><asp:Label ID="lblDate" Text='<%# DataBinder.Eval(Container.DataItem, "Status")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Status Comment: </b></td><td><asp:Label ID="lblHour" Text='<%# DataBinder.Eval(Container.DataItem, "StatusComent")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Request Comment: </b></td><td><asp:Label ID="lblPrice" Text='<%# DataBinder.Eval(Container.DataItem, "RequestComment")%>' runat="server"></asp:Label></td></tr>
                                <tr><td><b>Request Type: </b></td><td><asp:Label ID="lblReqType" Text='<%# DataBinder.Eval(Container.DataItem, "Category")%>' runat="server"></asp:Label></td></tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                           
                        </FooterTemplate>
                    </asp:Repeater>
                    <div style="width:300px;float:right">
                        <asp:Image ID="imgQr" runat="server" AlternateText="Loading QR Code" />
                    </div>
                    <% If RequestType = "FKS" Then%>
                        <table style="clear:both;">
                            <tr>
                                <td><asp:Button ID="btnAccept" runat="server" Text="Accept / Modify Request" /></td>
                            </tr>
                        </table>     
                    <% End If%>
                    </fieldset>
                        </div>
                </div>
                <p></p>
            </div>
           
           <div class="cpanel-right">

               <div style="background:white">
                   <asp:Literal ID="ltrErrorMessage" runat="server"></asp:Literal>
                   <div id="chart_div" style="width:570px;height:300px">
                       <p>Seats Request Details</p>
                   </div>
               </div>
           </div>
           
        </div>
    </div>
</asp:Content>

