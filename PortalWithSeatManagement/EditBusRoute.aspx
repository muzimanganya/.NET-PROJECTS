<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="EditBusRoute.aspx.vb" Inherits="EditBusRoute" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Edit Bus Information</title>
   
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
            <div class="pagetitle icon-48-edit">
                <h2>Edit Traffic & Business<asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="QueryDB.aspx">Query For Traffic</a>	</li>
                    <li><a href="NewBusRoute.aspx">New Traffic</a>	</li>
                    <li><a href="#" class="active" >Edit Traffic Item</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                <p>Editing Bus <asp:Literal ID="ltrBusName" runat="server"></asp:Literal></p>
                <fieldset id="queryForm" title="New Traffic / Business">
                  <ul class="queryList">
                    <li>
                        <asp:Label id="lblDate" for="txtDate" title="" runat="server" ClientIDMode="Static">Edit Target Date<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                            <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" Format="yyyy-MM-dd">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Please Select a Target Date" ControlToValidate="txtDate" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblHour" for="txtHour" title="" runat="server" ClientIDMode="Static">Edit Target Hour<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtHour" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter a Target Hour" ControlToValidate="txtHour" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblCapacity" for="txtCapacity" title="" runat="server" ClientIDMode="Static">Edit Bus Capacity<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtCapacity" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Capacity" ControlToValidate="txtCapacity" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCapacity" runat="server" ErrorMessage="Enter a Number Between 1 & 100" MinimumValue="1" MaximumValue="100" 
                                ControlToValidate="txtCapacity" Type="Integer"></asp:RangeValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblStatus" for="ddlStatus" title="" runat="server" ClientIDMode="Static">Select Bus Status<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="required" ClientIDMode="Static">
                                <asp:ListItem Text="Available" Value="Available" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Full" Value="Full"></asp:ListItem>
                                <asp:ListItem Text="Cancelled" Value="Cancelled" ></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvModule" runat="server" ErrorMessage="Please Select City IN" ControlToValidate="ddlStatus" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            
                        </div>
                    </li>

                       <li>
                        <asp:Label id="lblDriver" for="ddlDriver" title="" runat="server" ClientIDMode="Static">Select Bus Driver<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlDriver" runat="server" CssClass="required" ClientIDMode="Static">
                            </asp:DropDownList> 
                        </div>
                    </li>
                     <li>
                        <asp:Label id="lblVehicle" for="ddlVehicle" title="" runat="server" ClientIDMode="Static">Select Vehicle: <span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlVehicle" runat="server" CssClass="required" ClientIDMode="Static">
                            </asp:DropDownList> 
                        </div>
                    </li>

                      <li>
                        <asp:Label id="lblPrefSeats" for="txtPSeats" title="" runat="server" ClientIDMode="Static">Preferred Seats<br /> <i>(Separate with A Comma)</i></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtPSeats" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                        </div>
                    </li>

                    <li>
                        <asp:Label id="lblMemo" for="txtMemo" title="" runat="server" ClientIDMode="Static">Track Changes:<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtMemo" runat="server" Rows="5" Columns="50" 
                                TextMode="MultiLine" Enabled="false"></asp:TextBox>
                        </div>
                    </li>
                    <div class="clr">
                    </div>
                    <p></p>
                    <li>
                        <asp:Button ID="btnSend" runat="server" Text="Update Trajet" />
                    </li>
                  </ul>
                    </fieldset>
                </div>
                <p></p>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <p style="color:Red;font-weight:bolder;font-size:larger;">
                            <asp:Literal ID="ltrError" runat="server"></asp:Literal>
                        </p>
                    <div class="panel"><div id="piechart" style="min-height:80px;width:570px;font-size:14px;color:Blue;"><br /><br />Please Fill the Form to Create A new Trajet</div>
                    <p>
                        <asp:ValidationSummary ID="vsQuery" runat="server" Font-Bold="True" 
                            ForeColor="Red" />
                        
                    </div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

