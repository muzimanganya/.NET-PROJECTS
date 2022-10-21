<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="POSBooking.aspx.vb" Inherits="POSBooking" %>

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
                <h2>Bookings <asp:Literal ID="headerDate" runat="server"></asp:Literal></h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m">
				<ul id="submenu">
	            	<li><a href="POSTicketing.aspx">POS Ticketing</a>	</li>
		            <li><a href="POSSubscription.aspx">POS Subscription</a>	</li>
                    <li><a href="#" class="active">POS Booking</a>	</li>
                    <li><a href="POSPromotions.aspx">Promotions</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>

    <div id="element-box">
        <div class="m">
            <div class="cpanel-left" style= "width:66% !important">
                <div class="m">
                     <asp:Repeater ID="rptMasterView" runat="server">
                <HeaderTemplate>
                    <table id="masterview" width="830px">
                        <thead>
                        <tr>
                            <th><a href="#">City IN</a></th>
                            <th><a href="#">City OUT</a></th>
                            <th><a href="#">Ticket ID</a></th>
                            <th><a href="#">Traveler Name</a></th>
                            <th><a href="#">Total</a></th>
                            <th><a href="#">Discount</a></th>
                            <th><a href="#">Subscription No</a></th>
                            <th><a href="#">ClientCode</a></th>
                            <th><a href="#">Booked On</a></th>
                            <th><a href="#">Created By</a></th>                            
                        </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="left"><%# DataBinder.Eval(Container.DataItem, "CityIN")%></td>
                        <td align="left"><%# DataBinder.Eval(Container.DataItem, "CityOut")%></td>
                        <td align="left">
                            <a href='TicketDetails.aspx?TicketID=<%# DataBinder.Eval(Container.DataItem, "IDRELATION")%>'><%# Eval("IDRELATION")%></a>
                            <p class="smallsub">
                            
                            </p>
                        </td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Traveler")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Total")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Discount")%></td>
                        <td align="center">
                            <a href='CardDetails.aspx?cardno=<%# DataBinder.Eval(Container.DataItem, "sno")%>'><%# Eval("sno")%></a>
                            <p class="smallsub">
                            
                            </p>
                        </td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "ClientCode")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "CreatedDate")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "CreatedBy")%></td>
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
                <div class="m">
                    <asp:Repeater ID="rptSummary" runat="server">
                        <ItemTemplate>
                           <div style="float:right;font-size:14px;color:red;font-weight:bolder;">Total Bookings Today = # <%# DataBinder.Eval(Container.DataItem, "Total")%></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div class="cpanel-right" style= "width:33% !important">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"><div id="chart_div" style="min-height:300px;width:430px;">Pie Chart Comes Here</div></div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

