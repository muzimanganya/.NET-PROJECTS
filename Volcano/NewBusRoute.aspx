
<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="NewBusRoute.aspx.vb" Inherits="NewBusRoute" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>New Bus Route</title>
    
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
            <div class="pagetitle icon-48-new">
                <h2>New Traffic & Business<asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="QueryDB.aspx">Query For Traffic</a>	</li>
                    <li><a href="#" class="active" >New Traffic</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                <fieldset id="queryForm" title="New Traffic / Business">
                  <ul class="queryList">
                    <li>
                        <asp:Label id="lblCityIN" for="ddlCityIN" title="" runat="server" ClientIDMode="Static">Select City IN<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlCityIN" runat="server" CssClass="required" ClientIDMode="Static">
                                <asp:ListItem Text="Kigali" Value="Kigali" Selected=True></asp:ListItem>
                                <asp:ListItem Text="Muhanga" Value="Muhanga"></asp:ListItem>
                                <asp:ListItem Text="Huye" Value="Huye" ></asp:ListItem>
                                <asp:ListItem Text="Nyanza" Value="Nyanza" ></asp:ListItem>
                                <asp:ListItem Text="Ruhango" Value="Ruhango" ></asp:ListItem>
                                <asp:ListItem Text="Nyamagabe" Value="Nyamagabe"></asp:ListItem>
                                <asp:ListItem Text="Bujumb" Value="Bujumb"></asp:ListItem>
                                <asp:ListItem Text="Bujumb2" Value="Bujumb2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvModule" runat="server" ErrorMessage="Please Select City IN" ControlToValidate="ddlCityIN" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblCityOut" for="ddlCityOut" title="" runat="server" ClientIDMode="Static">Select City OUT<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlCityOut" runat="server" CssClass="required" ClientIDMode="Static">
                                <asp:ListItem Text="Kigali" Value="Kigali" Selected=True></asp:ListItem>
                                <asp:ListItem Text="Muhanga" Value="Muhanga"></asp:ListItem>
                                <asp:ListItem Text="Huye" Value="Huye" ></asp:ListItem>
                                <asp:ListItem Text="Nyanza" Value="Nyanza" ></asp:ListItem>
                                <asp:ListItem Text="Ruhango" Value="Ruhango" ></asp:ListItem>
                                <asp:ListItem Text="Nyamagabe" Value="Nyamagabe"></asp:ListItem>
                                <asp:ListItem Text="Bujumb" Value="Bujumb"></asp:ListItem>
                                <asp:ListItem Text="Bujumb2" Value="Bujumb2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select City OUT" ControlToValidate="ddlCityOut" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvTwoValues" runat="server" 
                                ErrorMessage="City IN & City OUT Cannot Be The Same" 
                                ControlToValidate="ddlCityOut" ControlToCompare="ddlCityIN" ForeColor="Red" 
                                Operator="NotEqual">*</asp:CompareValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblDate" for="txtDate" title="" runat="server" ClientIDMode="Static">Enter Target Date<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                            <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" Format="yyyy-MM-dd">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Please Select a Target Date" ControlToValidate="txtDate" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblHour" for="txtHour" title="" runat="server" ClientIDMode="Static">Enter Target Hour<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtHour" runat="server" ClientIDMode="Static" CssClass="required" MaxLength="5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Select a Target Hour" ControlToValidate="txtHour" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            <asp:MaskedEditExtender ID="meeHour" runat="server" TargetControlID="txtHour" Mask="99H99" MessageValidatorTip="true" 
                                ErrorTooltipEnabled="true" MaskType="number" InputDirection="LeftToRight" ClearMaskOnLostFocus="false">
                            </asp:MaskedEditExtender>
                            <%--<asp:MaskedEditValidator ID="mevHour" runat="server" ControlExtender="meeHour" ControlToValidate="txtHour" 
                                ErrorMessage="Please Enter the Hour Correctly. e.g 12H00 or 13H30" InvalidValueMessage="Please Enter the Hour Correctly. e.g 12H00 or 13H30">
                            </asp:MaskedEditValidator>--%>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblCapacity" for="txtCapacity" title="" runat="server" ClientIDMode="Static">Enter Bus Capacity<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtCapacity" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select City OUT" ControlToValidate="txtCapacity" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCapacity" runat="server" ErrorMessage="Enter a Number Between 1 & 100"
                                 MinimumValue="1" MaximumValue="100" ControlToValidate="txtCapacity" Type="Integer"></asp:RangeValidator>
                            <asp:MaskedEditExtender ID="meeCapacity" runat="server" TargetControlID="txtCapacity" Mask="99" MessageValidatorTip="true" 
                                ErrorTooltipEnabled="true" MaskType="number" InputDirection="LeftToRight" ClearMaskOnLostFocus="true">
                            </asp:MaskedEditExtender>
                        </div>
                    </li>
                    <div class="clr">
                    </div>
                    <p></p>
                    <li>
                        <asp:Button ID="btnSend" runat="server" Text="Create Trajet" />
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

