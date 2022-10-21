<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="PreBookings.aspx.vb" Inherits="PreBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
<title>Day's Promotions</title>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.paginate.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('#masterview').paginateTable({ rowsPerPage: 50 });
  //          $("#nextBtn").click();
//            $('#prevBtn').click();
        });
        function paginate(n) {
            $('#masterview').paginateTable({ rowsPerPage: n });
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    
                    <li class="button" id="pos">
                        <a href="POSTicketing.aspx" onclick="" class="toolbar">
                        <span class="icon-32-pos">
                            <img src="Styles/icon-48-media.png" width="32px" />
                        </span>POS Ticketing</a>  
                    </li>
                    <li class="button" id="Li2">
                        <a href="POSSubscription.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="Styles/icon-32-inbox.png" width="32px"/>
                        </span>Subscriptions</a>  
                    </li>
                    <li class="button" id="booking">
                        <a href="POSBooking.aspx" onclick="" class="toolbar">
                        <span class="icon-32-booking">
                            <img src="Styles/icon-48-new-privatemessage.png" width="32px"/>
                        </span>Booking</a>  
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
            <div class="pagetitle icon-48-booking">
                <h2>Pre-Bookings <asp:Literal ID="headerDate" runat="server"></asp:Literal></h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m">
				<ul id="submenu">
	            	<li><a href="POSTicketing.aspx">POS Ticketing</a>	</li>
		            <li><a href="POSSubscription.aspx">POS Subscription</a>	</li>
                    <li><a href="POSPromotions.aspx">Promotions</a>	</li>
                    <li><a href="#" class="active">Pre-Booking</a>	</li>
                    
	            </ul>
				<div class="clr"></div>
			</div>
	</div>

    <div id="element-box">
        <div class="m">
            <div class="cpanel-left" style= "width:90% !important">
                <div class="m">
                     <asp:Repeater ID="rptMasterView" runat="server">
                <HeaderTemplate>
                    <table id="masterview" width="830px">
                        <thead>
                        <tr>
                            <th><a href="#">Bus Name</a></th>
                            <th><a href="#">City IN</a></th>
                            <th><a href="#">City OUT</a></th>
                            <th><a href="#">Booking No</a></th>
                            <th><a href="#">Traveler Name</a></th>
                            <th><a href="#">Client Code</a></th>
                            <th><a href="#">Created On</a></th>
                            <th><a href="#">Expires On</a></th>
                            <th><a href="#">Booking Paid</a></th>
                            <th><a href="#">Booking Reconciled</a></th>  
                            <th><a href="#">Created By</a></th>                          
                        </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="left">
                            <a href='Bus-Details.aspx?IDSALE=<%# DataBinder.Eval(Container.DataItem, "BUSID")%>'><%# Eval("BusName")%></a>
                            <p class="smallsub">
                            
                            </p>
                        </td>
                        <td align="left"><%# DataBinder.Eval(Container.DataItem, "CityIN")%></td>
                        <td align="left"><%# DataBinder.Eval(Container.DataItem, "CityOut")%></td>
                        <td align="left">
                            <a href='PreBookingDetails.aspx?BookingNo=<%# DataBinder.Eval(Container.DataItem, "BookingNo")%>'><%# Eval("BookingNo")%></a>
                            <p class="smallsub">
                            
                            </p>
                        </td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Traveler")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "ClientCode")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "CreatedOn")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Expires")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Completed")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Recon")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Creator")%></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater> 
                     <div class='pager'>
                            <div class="limit">Display #
                                <select id="limit" name="limit" class="inputbox" size="1" onchange="paginate(this.value);">
	                                <option value="10" selected="selected">10</option>
	                                <option value="20">20</option>
	                                <option value="30">30</option>
	                                <option value="50">50</option>
	                                <option value="100">100</option>
                                </select>
                            </div>
                            <div style="float:left;width:150px;font-size:14px;margin-top:4px;">
                             <a href='#' alt='Previous' id="prevBtn" class='prevPage'>Prev</a>
                                <span class='currentPage'></span> of <span class='totalPages'></span>
                               <a href='#' alt='Next' id="nextBtn" class='nextPage'>Next</a>
                               </div>
                     </div>
                </div>
                <p></p>
               
            </div>
           
           
           
        </div>
    </div>
</asp:Content>

