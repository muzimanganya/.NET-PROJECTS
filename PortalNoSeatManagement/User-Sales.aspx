<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="User-Sales.aspx.cs" Inherits="User_Sales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Sales by Users</title>
    <link href="Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/Chart.min.js"></script>
    <script>
        var data = <% =this.ChartData %>;
        function DrawChart() { 
            var ctx = document.getElementById("canvas").getContext("2d");
            var chart = new Chart(ctx).Pie(data, {});
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
            <div class="pagetitle icon-48-pos">
                <h2>User Sales <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m">
				<ul id="submenu">
	            	<li><a class="active" href="#">Sales Overview</a>	</li>
		            <li><a href="User-Sales-List.aspx">Sales Details</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>

    <div id="element-box">
        <div class="m">

            <div class="cpanel-left"> 
                <div class="m">        
                    <!-- Grid to present data -->
                    <asp:GridView ID="AllUsersGrid" runat="server" AutoGenerateColumns="False" TabIndex="1"
                Width="100%" CellPadding="4" ForeColr="Black" GridLines="Vertical"
                OnRowDataBound="GridRowDataBound"  BackColor="White"
                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                ShowHeader="False">            
                        <Columns>
                             <asp:HyperLinkField  HeaderText="User Name" DataNavigateUrlFields="User,ReqDate" 
                                    DataTextField="User"  DataNavigateUrlFormatString="User-Sales-List.aspx?username={0}&reqDate={1}" />
                             <asp:BoundField DataField="TotalNormal" HeaderText="Sold Tickets"  />
                             <asp:BoundField DataField="TotalPromo" HeaderText="Promotion Tickets"  />
                             <asp:BoundField DataField="TotalBooking" HeaderText="Bookings"  />
                             <asp:BoundField DataField="TotalTickets" HeaderText="Total Tickets" />
                             <asp:BoundField DataField="TotalRWF" HeaderText="Revenue (RWF)"  DataFormatString="{0:N0}" />
                             <asp:BoundField DataField="TotalFIB" HeaderText="Revenue (FIB)" DataFormatString="{0:N0}" />
                        </Columns>
                    </asp:GridView>

                </div> 
            </div>

            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"> 
                        <h3 style="text-align:center">Best Selling Users</h3>
                        <div style="width: 100%">
                          <div>
                            <canvas id="canvas" height="250" width="400"></canvas>
                          </div>
                        </div>
                  </div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

