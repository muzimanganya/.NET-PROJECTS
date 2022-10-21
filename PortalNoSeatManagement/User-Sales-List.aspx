<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="User-Sales-List.aspx.cs" Inherits="User_Sales_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>User Sales Per Bus</title>
    <link href="Styles/gridview.css" rel="stylesheet" type="text/css" />
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
            <div class="pagetitle icon-48-traffic">
                <h2> <asp:Literal ID="posText" runat="server"></asp:Literal> User Sales For <asp:Label ID="UserLabel" runat="server" Text="NONE"></asp:Label> <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
                <div id="posDetails" style="font-style:oblique;color:blue">
                    <asp:Literal ID="ltrPOS" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
      <div id="submenu-box">
			<div class="m">
				<ul id="submenu">
	            	<li><a href="User-Sales.aspx">Sales Overview</a>	</li>
		            <li><a href="#" class="active">Sales Details</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <!--Here we are going to make a gridView with special header-->
            <asp:GridView ID="gridViewBuses" runat="server" AutoGenerateColumns="False" TabIndex="1"
                Width="100%" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                OnRowDataBound="GridViewDataBound"
                onrowcreated="GridViewRowCreated" BackColor="White"
                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                ShowHeader="False">            
                <Columns>
                    <asp:BoundField DataField="CityIN" HeaderText="City IN">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CityOut" HeaderText="City OUT">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Ticket ID">
                        <ItemTemplate>
                            <a href='TicketDetails.aspx?Ticketid=<%# Eval("IDRELATION" )%>'><%# Eval("IDRELATION")%></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="IDRELATION" HeaderText="TicketID">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="Price" HeaderText="Price">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Discount" HeaderText="Discount">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Subscription No">
                        <ItemTemplate>
                            <a href='CardDetails.aspx?cardno=<%# Eval("Subscriptiono")%>'><%# Eval("Subscriptiono")%></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>

                    <%--<asp:BoundField DataField="Subscriptiono" HeaderText="Subscription No">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="CreatedOn" HeaderText="Created ON">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CreatedBy" HeaderText="Creating POS">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <RowStyle BackColor="#F7F7DE" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" />
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
        </div>
    </div>
</asp:Content> 