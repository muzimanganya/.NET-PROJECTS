<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="All-Days--Planned-Traffic.aspx.vb" Inherits="All_Days__Planned_Traffic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>All Planned Traffic For the Day</title>
    <link href="Styles/gridview.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            color: red;
            font-weight: bold;
        }
    </style>
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
            <div class="pagetitle icon-48-traffic">
                <h2>Traffic & Business <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m">
				<ul id="submenu">
	            	<li><a  href="Traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="All-Days--Planned-Traffic.aspx" class="active">All Planned Traffic</a>	</li>
		            <li><a href="QueryDB.aspx">Query For Traffic</a>	</li>
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
                onrowcreated="gridViewBuses_RowCreated" BackColor="White"
                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                ShowHeader="False">            
                <Columns>
                    <asp:TemplateField HeaderText="Bus Route / Trajet">
                        <ItemTemplate>
                            <a href='Bus-Details.aspx?idsale=<%# Eval("IDSALE" )%>&requestDate=<%# Request.QueryString("requestDate") %>'><%# Eval("Name")%></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Target_Date" HeaderText="Date">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Hour" HeaderText="Hour">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Vehicle">
                        <ItemTemplate>
                            <a href='Vehicles/VehicleEntry.aspx?VehicleNumber=<%# Eval("Vehicle")%>'>Vehicle Info</a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center " />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Driver">
                        <ItemTemplate>
                            <a href='Drivers/DriverEntry.aspx?DriverID=<%# Eval("Driver")%>'>Driver Info</a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Capacity" HeaderText="Max Capacity">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Booked" HeaderText="Tickets">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Memo" HeaderText="Messages">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>--%>
                    
                    <asp:TemplateField HeaderText="Memo" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href='Bus-Details.aspx?idsale= <%# Eval("IDSALE" )%>&requestDate=<%# Request.QueryString("requestDate") %>'><%# Eval("Memo")%></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <%--<asp:BoundField DataField="Status" HeaderText="Status">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>--%>
                    
                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href="EditBusRoute.aspx?idsale=<%# Eval("idsale")%>">
                                <%# Eval("Status")%>
                            </a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="FIB" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("TotalFIB", "{0:n2}")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="RWF" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("TotalRWF", "{0:n2}")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />

                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="UGX" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("TotalUGX", "{0:n2}")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <RowStyle BackColor="#F7F7DE" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" />
                <EmptyDataTemplate>
                    <span class="style1">No Traffic Planned For Today! Please Try Again</span>
                </EmptyDataTemplate>
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

