<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="QueryCards.aspx.vb" Inherits="QueryCards" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Query Cards</title>
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
                <h2>Query Cards <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="#">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Query Cards</a>	</li>
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
                        <asp:Label id="lblModule" for="ddlCityIN" title="" runat="server" ClientIDMode="Static">Select City IN<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlCityIN" runat="server" CssClass="required" ClientIDMode="Static">
                                <asp:ListItem Text="Kigali" Value="Kigali" Selected=True></asp:ListItem>
                                <asp:ListItem Text="Nyabugogo" Value="Nyabugogo"></asp:ListItem>
                                <asp:ListItem Text="Rubavu" Value="Rubavu"></asp:ListItem>
                                <asp:ListItem Text="Nyagatare" Value="Nyagatare" ></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvModule" runat="server" ErrorMessage="Please Select a City IN" ControlToValidate="ddlCityIN" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                      <li>
                        <asp:Label id="lblCityOut" for="ddlCityOut" title="" runat="server" ClientIDMode="Static">Select City OUT<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:DropDownList ID="ddlCityOut" runat="server" CssClass="required" ClientIDMode="Static">
                                <asp:ListItem Text="Kigali" Value="Kigali" Selected=True></asp:ListItem>
                                <asp:ListItem Text="Nyabugogo" Value="Nyabugogo"></asp:ListItem>
                                <asp:ListItem Text="Rubavu" Value="Rubavu"></asp:ListItem>
                                <asp:ListItem Text="Nyagatare" Value="Nyagatare" ></asp:ListItem>
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
                        <asp:Label id="Label1" for="txtStartDate" title="" runat="server" ClientIDMode="Static">Enter Start Date (Created) <span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" Format="yyyy-MM-dd">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select a Start Date" ControlToValidate="txtStartDate" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblDate" for="txtEndDate" title="" runat="server" ClientIDMode="Static">Enter End Date (Created) <span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                            <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtEndDate" Format="yyyy-MM-dd">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Please Select a Target Date" ControlToValidate="txtEndDate" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvDates" runat="server" 
                                ErrorMessage="End Date Cannot Be Less than Start Date" 
                                ControlToValidate="txtStartDate" ControlToCompare="txtEndDate" ForeColor="Red" 
                                Operator="LessThan" Type="Date" >*</asp:CompareValidator>
<%--                            <asp:CompareValidator ID="cvEndDate" runat="server" 
                                ErrorMessage="End Date Cannot Be Greater than Today's Date" 
                                ControlToValidate="txtEndDate" ForeColor="Red" 
                                Operator="GreaterThan">*</asp:CompareValidator>--%>
                        </div>
                    </li>
                      
                      <li>
                          <asp:Label id="lblStatus" for="ddlStatus" title="" runat="server" ClientIDMode="Static">Select Status.<span class="star">&nbsp;*</span></asp:Label>
                          <asp:DropDownList ID="ddlStatus" runat="server" CssClass="required" ClientIDMode="Static">
                                <asp:ListItem Text="Sold" Value="0" Selected=True></asp:ListItem>
                                <asp:ListItem Text="Expired" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Sold" Value="2" ></asp:ListItem>
                              <asp:ListItem Text="All" Value="3" ></asp:ListItem>
                            </asp:DropDownList>
                      </li>
                    <div class="clr"></div>
                    <p></p>
                    <li>
                        <asp:Button ID="btnSend" runat="server" Text="Query For Cards" />
                    </li>
                  </ul>
                    </fieldset>
                </div>
                <p></p>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"><div id="piechart" style="min-height:80px;width:570px;font-size:14px;color:Blue;"><br /><br />Please Select the Criteria to Query For Cards</div>
                    <p>
                        &nbsp;<asp:ValidationSummary ID="vsQuery" runat="server" Font-Bold="True" 
                            ForeColor="Red" />
                    </div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

