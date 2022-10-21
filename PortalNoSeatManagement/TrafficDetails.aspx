<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="TrafficDetails.aspx.vb" Inherits="TrafficDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="Server">
    <title>Traffic Details For Bus Route</title>
    <link href="Styles/gridview.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
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
                                <img src="Styles/icon-48-new-privatemessage.png" width="32px" />
                            </span>Booking</a>
                    </li>
                    <li class="button" id="Li2">
                        <a href="POSSubscription.aspx" onclick="" class="toolbar">
                            <span class="icon-32-promo">
                                <img src="Styles/icon-32-inbox.png" width="32px" />
                            </span>Subscriptions</a>
                    </li>
                    <li class="button" id="promo">
                        <a href="#" onclick="" class="toolbar">
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
                <h2>
                    <asp:Literal ID="posText" runat="server"></asp:Literal>
                    <asp:Literal ID="headerDate" runat="server"></asp:Literal>
                </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
        <div class="m">
            <ul id="submenu">
                <li><a class="active" href="traffic.aspx">Today's Relevant Traffic</a>	</li>
                <li><a href="All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
                <li><a href="QueryDB.aspx">Query For Traffic</a>	</li>
                <li><a href="vehicleReport.aspx">Query Vehicle Routes</a>	</li>
            </ul>
            <div class="clr"></div>
        </div>
    </div>
    <div id="element-box">
        <div class="m">
            <!--Here we are going to make a gridView with special header-->
            <asp:GridView ID="gridViewBuses" runat="server" AutoGenerateColumns="False" TabIndex="1"
                Width="100%" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                OnRowDataBound="gridViewBuses_RowDataBound"
                OnRowCreated="gridViewBuses_RowCreated" BackColor="White"
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
                            <a href='TicketDetails.aspx?ticketid=<%# Eval("IDRELATION" )%>'><%# Eval("IDRELATION")%></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Price" HeaderText="Price">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Discount" HeaderText="Discount">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Subscriptiono" HeaderText="Subscription No">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Subscription No">
                        <ItemTemplate>
                            <a href='CardDetails.aspx?Cardno=<%# Eval("Subscriptiono")%>'><%# Eval("Subscriptiono")%></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
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

