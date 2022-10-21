<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="MasterSeats.aspx.vb" Inherits="MasterSeatsAllocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Master Seats Allocation</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="/NewBusRoute.aspx" onclick="" class="toolbar">
                        <span class="icon-32-new"></span>New</a>
                    </li>
                   
                    <li class="divider"></li>
                    <li class="button" id="pos">
                        <a href="/POSTicketing.aspx" onclick="" class="toolbar">
                        <span class="icon-32-pos">
                            <img src="/Styles/icon-48-media.png" width="32px" />
                        </span>POS Ticketing</a>  
                    </li>
                    <li class="button" id="booking">
                        <a href="/POSBooking.aspx" onclick="" class="toolbar">
                        <span class="icon-32-booking">
                            <img src="/Styles/icon-48-new-privatemessage.png" width="32px"/>
                        </span>Booking</a>  
                    </li>
                    <li class="button" id="Li2">
                        <a href="/POSSubscription.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="/Styles/icon-32-inbox.png" width="32px"/>
                        </span>Subscriptions</a>  
                    </li>
                    <li class="button" id="promo">
                        <a href="/POSPromotions.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="/Styles/icon-48-checkin.png" width="32px" />
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
                <h2>Master Seats Allocation <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="/traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="/All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Master Seat Allocation</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Serial" 
        DataSourceID="sdsMaster" ForeColor="#333333" GridLines="None" Width="650px">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:CommandField ButtonType="Button" EditText="Edit Limit" 
                ShowEditButton="True" />
            <asp:BoundField DataField="NAME" HeaderText="NAME" SortExpression="NAME" 
                ReadOnly="True" />
            <asp:BoundField DataField="StopIN" HeaderText="StopIN" 
                SortExpression="StopIN" ReadOnly="True" />
            <asp:BoundField DataField="StopOut" HeaderText="StopOut" 
                SortExpression="StopOut" ReadOnly="True" />
            <asp:BoundField DataField="MaxSeats" HeaderText="MaxSeats" SortExpression="MaxSeats" />
            <asp:BoundField DataField="Duration" HeaderText="Duration" SortExpression="Duration" />
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <EmptyDataTemplate>
            <div style="color:red">No Data Received Please Try Again</div>
        </EmptyDataTemplate>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
        <asp:SqlDataSource ID="sdsMaster" runat="server"
            SelectCommand="SELECT * FROM su.[vwMaster]" 
            UpdateCommand="UPDATE su.vwMaster SET MaxSeats =@MaxSeats WHERE Serial=@Serial">
            <UpdateParameters>
                <asp:Parameter Name="MaxSeats" type="Int16"/>
                <asp:Parameter Name="StopIN" Type="String"/>
                <asp:Parameter Name="StopOut" Type= "String" />
            </UpdateParameters>
         </asp:SqlDataSource>
                </div>
                <p></p>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel">
                        <p>Set the Seats Number to -1 to make it unlimited.</p>
        <p>Please make sure the values are between -1 and 50</p>
        <p></p>
        <p style="color:Red">
            &nbsp;<asp:ValidationSummary ID="vsEdit" runat="server" BorderColor="#0033CC" 
            BorderStyle="Solid" BorderWidth="1px" DisplayMode="List" Font-Bold="True" 
            Font-Italic="True" ForeColor="Red" />
                    </div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

