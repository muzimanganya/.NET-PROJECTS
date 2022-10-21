<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="EditSeatsRequest.aspx.vb" Inherits="EditRequests" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Accept / Edit Seats Request</title>
    
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
                <h2>Accept / Edit Request Details <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="#">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Accept / Edit Seats Request</a>	</li>
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
                    <table>
                        <tr>
                            <td colspan="2"><h3>Editing the Bus Request for: <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h3></td>
                        </tr>
                        <tr>
                            <td>Assign Seats Requested: </td>
                            <td>
                                <asp:TextBox ID="txtSeats" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revSeats" runat="server" ErrorMessage="Only Numbers Allowed" ValidationExpression="^(0|[1-9][0-9]*)$"
                                    ForeColor="Red" ControlToValidate="txtSeats">*</asp:RegularExpressionValidator>
                                <asp:RangeValidator ID="rvValidate" runat="server" ErrorMessage="Cannot Assign More than Requested" ForeColor="Red" ControlToValidate="txtSeats">*</asp:RangeValidator>
                            </td>
                            
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnAccept" runat="server" Text="Accept Request" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                            </td>
                        </tr>
                    </table>    
                </fieldset>
                        </div>
                </div>
                <p></p>
            </div>
           
           <div class="cpanel-right">

               <div style="background:white">
                   <asp:Literal ID="ltrErrorMessage" runat="server"></asp:Literal>
                   <div id="chart_div" style="width:570px;height:300px">
                       <h4>You can either assign the seats requested or less. But not More</h4>
                   </div>
               </div>
           </div>
           
        </div>
    </div>
</asp:Content>

