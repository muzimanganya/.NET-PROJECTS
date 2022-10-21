<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="DriverEntry.aspx.vb" Inherits="DriverEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Driver Entry</title>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="DriverEntry.aspx" onclick="" class="toolbar">
                        <span class="icon-32-new"></span>New Driver</a>
                    </li>
                   
                    <li class="divider"></li>
                    <li class="button" id="pos">
                        <a href="POSTicketing.aspx" onclick="" class="toolbar">
                        <span class="icon-32-pos">
                            <img src="/Styles/icon-48-media.png" width="32px" />
                        </span>POS Ticketing</a>  
                    </li>
                    <li class="button" id="booking">
                        <a href="POSBooking.aspx" onclick="" class="toolbar">
                        <span class="icon-32-booking">
                            <img src="/Styles/icon-48-new-privatemessage.png" width="32px"/>
                        </span>Booking</a>  
                    </li>
                    <li class="button" id="Li2">
                        <a href="POSSubscription.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="/Styles/icon-32-inbox.png" width="32px"/>
                        </span>Subscriptions</a>  
                    </li>
                    <li class="button" id="promo">
                        <a href="POSPromotions.aspx" onclick="" class="toolbar">
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
            <div class="pagetitle icon-48-user">
                <h2>Driver Information<asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a href="/Traffic.aspx">Today's Relevant Traffic</a>	</li>
		            <li><a href="/All-Days--Planned-Traffic.aspx">All Planned Traffic</a>	</li>
		            <li><a href="#" class="active" >Driver Information</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">
                <fieldset id="queryForm" title="Query The Database">
                  <ul class="queryList" style="width:58% !important;float:left">
                    <li>
                        <asp:Label id="lblName" for="txtName" title="" runat="server" ClientIDMode="Static">Driver Name:<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvModule" runat="server" ErrorMessage="Please Enter the Driver Name" 
                                ControlToValidate="txtName" ForeColor="Red">*
                            </asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li>
                        <asp:Label id="lblPhone" for="txtPhone" title="" runat="server" ClientIDMode="Static">Driver Phone Number:<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                        </div>
                    </li>
                      <li>
                        <asp:Label id="lblAddress" for="txtAdd" title="" runat="server" ClientIDMode="Static">Driver Address:<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtAdd" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </li>
                      <li>
                        <asp:Label id="lblEmail" for="txtEmail" title="" runat="server" ClientIDMode="Static">Driver Email:<span class="star">&nbsp;*</span></asp:Label>
                        <div class="fltlft">
                            <asp:TextBox ID="txtEmail" runat="server" type="Email"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revEmail"  ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"
                                runat="server" ErrorMessage="Please Enter a Valid Email" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                        </div>
                    </li>
                    <div class="clr">
                    </div>
                    <p></p>
                    <li>
                        <asp:Button ID="btnSend" runat="server" Text="Update Information" />
                    </li>
                  </ul>
                    
                   <div style="width:35%;float:right;padding-top:10px">
                       <div style="background-color:transparent;width:140px;height:124px;padding-left:6px">
                           <asp:Image ID="imgProfilePic" runat="server" Width="134px" Height="113px" Style="border:1px solid #fff;border-radius:62px"/>
                       </div>
                       <asp:FileUpload ID="fpProfilePic" runat="server" />
                   </div>
                </fieldset>
                </div>
                <p></p>
            </div>
            <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <%--<div id="dateController" style="position:absolute;z-index:100;background: yellow;padding: 0px 10px;right: 140px;margin-top: 3px;display:none">
                        <ul class="queryList">
                            <li>
                                <asp:Label ID="lblDate" AssociatedControlID="txtDate" runat="server">Change Week:</asp:Label>
                                <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="ceSelectDate" runat="server" TargetControlID="txtDate"></asp:CalendarExtender>
                                <asp:Button ID="btnGo" runat="server" Text="Go!" />
                            </li>
                        </ul>
                        
                    </div>--%>
                    <div class="panel"><div id="chart_div" style="height:230px;width:570px;font-size:14px;color:Blue;"><br /><br />Loading Driver's History</div>
                    <p style="font-size:130%;color:red">
                        <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
                    </p>
                        <p>
                            &nbsp;<asp:ValidationSummary ID="vsQuery" runat="server" Font-Bold="True" 
                            ForeColor="Red" />
                    </div>
                </div>
            </div>
           
           
        </div>

        <div class="clr"></div>
        <div class="m">
            <asp:GridView ID="gridViewBuses" runat="server" AutoGenerateColumns="False" TabIndex="1"
                Width="100%" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                OnRowDataBound="gridViewBuses_RowDataBound"
                onrowcreated="gridViewBuses_RowCreated" BackColor="White"
                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" showHeader="false">            
                <Columns>
                    <asp:TemplateField HeaderText="Bus Route / Trajet">
                        <ItemTemplate>
                            <a href='/Bus-Details.aspx?idsale= <%# Eval("IDSALE" )%>&requestDate=<%# Request.QueryString("requestDate") %>'><%# Eval("Name")%></a>
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
                            <a href='/Vehicles/VehicleEntry.aspx?VehicleNumber=<%# Eval("Vehicle")%>'>Vehicle Info</a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center " />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Driver">
                        <ItemTemplate>
                            Driver Info
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
                            <a href='/Bus-Details.aspx?idsale= <%# Eval("IDSALE" )%>&requestDate=<%# Request.QueryString("requestDate") %>'><%# Eval("Memo")%></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href="/EditBusRoute.aspx?idsale=<%# Eval("idsale")%>">
                                <%# Eval("Status")%>
                            </a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("Budget", "{0:n2}")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <RowStyle BackColor="#F7F7DE" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px" />
                <EmptyDataTemplate>
                    <span class="style1">No buses found recorded for the Driver. <br /> Did you know you can track Driver & Bus Activity from the Portal? Edit the Bus and Select The Driver to track them over time.</span>
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

