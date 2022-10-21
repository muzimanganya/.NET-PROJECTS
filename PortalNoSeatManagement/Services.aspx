<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Services.aspx.vb" Inherits="Services" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Traffic & Business</title>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <link href="Styles/bootstrap/css/bootstrap.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="NewBusRoute.aspx" onclick="" class="toolbar">
                        <span class="icon-32-edit"></span>Edit</a>
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
            <div class="pagetitle icon-48-services">
                <h2>Services</h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a class="active" href="#">Services</a>	</li>
		            <li><a href="POSList.aspx">POS Machines</a>	</li>
		            <li><a href="traffic.aspx">Today's Traffic</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-right" style="float:left !important;width:33% !important">
                <div class="m">
                    
                    <asp:TreeView ID="trvServices" runat="server">
                        <Nodes>
                            
                        </Nodes>
                    </asp:TreeView>
                </div>
                <p></p>
            </div>
            <div class="cpanel-left" style="float:right !important; width: 66% !important">
                <div class="pane-sliders2">
                    <div class="panel">
                        <asp:TabContainer ID="tbServices" runat="server"  ActiveTabIndex="0" ScrollBars="Auto" TabStripPlacement="Top" Height="365px">
                            <asp:TabPanel ID="travelersPerMonth" runat="server" HeaderText="Travellers Per Month">
                                <ContentTemplate><div id="tchart" style="width:800px;height:280px">
                                    <p></p>
                                    <p>Please Select A Bus Route On the Left</p>
                                </div></ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="revenuePerMonth" runat="server" HeaderText="Revenue Per Month">
                                <ContentTemplate>
                                    <div id="rchart" style="width:800px;height:360px">
                                    <p></p>
                                    <p>Please Select A Bus Route On the Left</p>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="salesOpp" runat="server" HeaderText="Sales Opportunities">
                                <ContentTemplate>
                                    <asp:Repeater ID="rptSalesOpportunities" runat="server">
                                        <HeaderTemplate>
                                            <table id="masterview" width="95%">
                                                <thead>
                                                    <th></th>
                                                    <th>Route</th>
                                                    <th>Date</th>
                                                    <th>Hour</th>
                                                    <th>Capacity</th>
                                                    <th>Booked</th>
                                                    <th>Status</th>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><a href='Bus-Details.aspx?idsale=<%# Eval("IDSALE") %>'>SELECT</a></td>
                                                <td><%# Eval("Route")%></td>
                                                <td><%# Eval("Target_Date")%></td>
                                                <td><%# Eval("Hour")%></td>
                                                <td><%# Eval("Capacity")%></td>
                                                <td><%# Eval("Booked")%></td>
                                                <td><%# Eval("Status")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                    </div>
                    
                    
         <asp:Panel ID="pnlCards" runat="server" CssClass="panel">
                             <div class="alert alert-info">
                                <p>
                                    Showing Cards for <asp:Literal ID="ltrCardsName" runat="server"></asp:Literal>. Use the Tree on the left to view other Cards for Different Quantity
                                    <a class="close" data-dismiss="alert" href="#">&times;</a>
                                </p>
                            </div>
                             <asp:Repeater ID="rptCards" runat="server">
                                                        <HeaderTemplate>
                                                            <table class="table table-bordered table-hover table-condensed" id="cardslist">
                                                                <thead>
                                                                    <tr>
                                                                        <th class="header">Card No</th>
                                                                        <th>Status</th>
                                                                        <th class="header">Route (City In- CityOut)</th>
                                                                        <th class="header">Trips</th>
                                                                        <th class="header">Creating POS</th>
                                                                        <th class="header">Created Date</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr class='<%# GetRowStyle(Eval("Status").ToString())%>'>
                                                                <td><a href='CardDetails.aspx?CardNo=<%# Eval("CardNo")%>'><%# Eval("CardNo")%></a></td>
                                                                <td><%# Eval("Status")%></td>
                                                                <td><%# Eval("Route")%></td>
                                                                <td><%# Eval("Trips")%></td>
                                                                <td><%# Eval("CreatedBy")%></td>
                                                                <td><%# Eval("CreatedOn")%></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tbody>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                        
                                                    <div class="pager" style="margin:auto !important">
                                                        <div class="pagination pagination-centered pagination-large">
                                                        <ul>
                                                            <li>
                                                                <a href='#' alt='Previous' id="prevBtn" class='prevPage'>Prev</a>
                                                            </li>
                                                            <li>
                                                                <a href="#"><span class='currentPage'></span> of <span class='totalPages'></span>
                                                                    </a>
                                                            </li>
                                                            <li>
                                                                <a href='#' alt='Next' id="nextBtn" class='nextPage'>Next</a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                    </div>
                        </asp:Panel>
                       
                       
                </div>
            </div>
           <style>
               #footer {display:none !important}
               .pager li a {
                   border-radius: 0px !important;
               }
               #cardslist th.headerSortUp { 
                  background-image: url(./Styles/images/asc.gif) !important; 
                  background-color: #3399FF; 
                }   
               #cardslist th.headerSortDown { 
                background-image: url(./Styles/images/desc.gif) !important; 
                background-color: #3399FF; 
                } 
               #cardslist th.header { 
                    background-image: url(./Styles/images/bg.gif);     
                    cursor: pointer; 
                    font-weight: bold; 
                    background-repeat: no-repeat; 
                    background-position: center left; 
                    padding-left: 20px; 
                    border-right: 1px solid #dad9c7; 
                    margin-left: -1px; 
                } 
           </style>
           
        </div>
    </div>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.paginate.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tablesorter.js"></script>
    <script src="Scripts/bootstrap.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#cardslist').paginateTable({ rowsPerPage: 40 });
            $('#cardslist').tablesorter();
            $(".alert").alert();
        });
        </script>
</asp:Content>

