<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="QueryDB.aspx.vb" Inherits="QueryDB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Query Traffic & Business</title>
    <script src="Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script type"text/javascript">
        //This is going to detect the change in select of the list and display a tooltip appropriately
        $(function () {
            $("#ddlModules").attr("selectedindex", -1)
            $("#ddlModules").change(function () {
                var selectedModule = $("#ddlModules option:selected").val();
                switch (selectedModule) {
                    case "All-Days--Planned-Traffic.aspx":
                        $("#piechart").fadeOut('fast');
                        $("#piechart").html("<br /><p>This Will Show you a target day's all planned traffic</p>");
                        $("#piechart").fadeIn('slow');
                        break;
                    case "Traffic.aspx":
                        $("#piechart").fadeOut('fast');
                        $("#piechart").html("<br /><p>This will show you an overview a days sales, grouped by route</p>");
                        $("#piechart").fadeIn('slow');
                        break;
                    case "POSTicketing.aspx":
                        $("#piechart").fadeOut('fast');
                        $("#piechart").html("<br /><p>This will show Per POS Ticketing For a Certain day</p>");
                        $("#piechart").fadeIn('slow')
                        break;
                    case "POSBooking.aspx":
                        $("#piechart").fadeOut('fast');
                        $("#piechart").html("<br /><p>This will show you Per POS Booking for a certain day</p>");
                        $("#piechart").fadeIn('slow');
                        break;
                    case "POSSubscription.aspx":
                        $("#piechart").fadeOut('fast');
                        $("#piechart").html("<br /><p>This will show you Per POS Subscriptions for a certain day</p>");
                        $("#piechart").fadeIn('slow');
                        break;
                    case "POSPromotions.aspx":
                        $("#piechart").fadeOut('fast');
                        $("#piechart").html("<br /><p>This will show you Per POS Promotions for a certain day</p>");
                        $("#piechart").fadeIn('slow');
                        break;
                }
            });
        });
    </script>
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
                <h2>Query Traffic & Business <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="#">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Query For Traffic</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                <fieldset id="queryForm" title="Query The Database">
                  <ul class="queryList">
                    <li>
                        <asp:Label id="lblModule" for="ddlModules" title="" runat="server" ClientIDMode="Static">Select Module<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlModules" runat="server" CssClass="required" ClientIDMode="Static">
                                <asp:ListItem Text="Day's Planned Traffic" Value="All-Days--Planned-Traffic.aspx"></asp:ListItem>
                                <asp:ListItem Text="Traffic & Business Overview" Value="Traffic.aspx"></asp:ListItem>
                                <asp:ListItem Text="User Sales" Value="User-Sales.aspx"></asp:ListItem>
                                <asp:ListItem Text="POS Ticketing" Value="POSTicketing.aspx"></asp:ListItem> 
                                <asp:ListItem Text="POS Subscription" Value="POSSubscription.aspx"></asp:ListItem>
                                <asp:ListItem Text="POS Booking" Value="POSBooking.aspx"></asp:ListItem>
                                <asp:ListItem Text="POS Promotions" Value="POSPromotions.aspx"></asp:ListItem>
                                <asp:ListItem Text="Deleted Tickets" Value="RemovedTraffic/DeletedTickets.aspx"></asp:ListItem>
                                <asp:ListItem Text="Rolled Back Tickets" Value="RemovedTraffic/RolledBackTickets.aspx"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvModule" runat="server" ErrorMessage="Please Select a Module" ControlToValidate="ddlModules" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblDate" for="txtDate" title="" runat="server" ClientIDMode="Static">Enter Target Date<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                            <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Please Select a Target Date" ControlToValidate="txtDate" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <div class="clr">
                    </div>
                    <p></p>
                    <li>
                        <asp:Button ID="btnSend" runat="server" Text="Query Database" />
                    </li>
                  </ul>
                    </fieldset>
                </div>
                <p></p>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"><div id="piechart" style="min-height:80px;width:570px;font-size:14px;color:Blue;"><br /><br />Please Select a Module and The Target Date to Query</div>
                    <p>
                        <asp:ValidationSummary ID="vsQuery" runat="server" Font-Bold="True" 
                            ForeColor="Red" />
                    </div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

