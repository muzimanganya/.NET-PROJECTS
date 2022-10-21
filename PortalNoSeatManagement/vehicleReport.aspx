<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="vehicleReport.aspx.cs" Inherits="vehicleReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="Server">
    <title>Routes Per Vehicle</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
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

        <div style="padding: 20px;">
            <asp:Label Text="From: " runat="server"></asp:Label>
            <asp:TextBox ID="DateTextBoxFrom" runat="server" />
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                TargetControlID="DateTextBoxFrom" PopupButtonID="Image1">
            </ajaxToolkit:CalendarExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;
            
            <asp:Label Text="To: " runat="server"></asp:Label>
            <asp:TextBox ID="DateTextBoxTo" runat="server" />
            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                TargetControlID="DateTextBoxTo" PopupButtonID="">
            </ajaxToolkit:CalendarExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;
            
            <asp:Label Text="Plate Number: " runat="server"></asp:Label>
            <asp:TextBox ID="TextBoxPlateNo" runat="server" />
            <ajaxToolkit:AutoCompleteExtender TargetControlID="TextBoxPlateNo" runat="server"
                MinimumPrefixLength="1" CompletionSetCount="10" CompletionInterval="100" ServiceMethod="GetVehicles">
            </ajaxToolkit:AutoCompleteExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;

            <asp:Button OnClick="Page_Query" Text="Query" runat="server" />
        </div>

        <div class="m">
            <h2 style="margin: 10px;">Routes for the Vehicle with Plate Number  -
                <asp:Label ID="PlateNoLabel" runat="server" Text="NONE"></asp:Label>
            </h2>
            <asp:ListView ID="RoutesListView" runat="server" OnItemDataBound="RoutesListView_DataBound" >
                <ItemTemplate>
                    <tbody>
                        <tr style="text-align:center;">
                            <td>
                                <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("ROUTENAME") %>' />
                            </td>
                            <td>
                                <asp:Label ID="DATELabel" runat="server" Text='<%# Eval("ROUTEDATE") %>' />
                            </td>
                            <td>
                                <asp:Label ID="HourLabel" runat="server" Text='<%# Eval("ROUTEHOUR") %>' />
                            </td>
                            <td>
                                <asp:Label ID="CAPACITYLabel" runat="server" Text='<%# Eval("ROUTECAPACITY") %>' />
                            </td>
                            <td>
                                <asp:Label ID="TICKETSLabel" runat="server" Text='<%# Eval("SOLDTICKETS") %>' />
                            </td>
                            <td>
                                <asp:Label ID="DRIVERLabel" runat="server" Text='<%# Eval("DRIVERNAME") %>' />
                            </td>
                            <td>
                                <asp:Label ID="TotalRWFLabel" runat="server" Text='<%# Eval("TotalRWF") %>' />
                            </td>
                            <td>
                                <asp:Label ID="TotalFIBLabel" runat="server" Text='<%# Eval("TotalFIB") %>' />
                            </td> 
                        </tr>
                    </tbody> 
                </ItemTemplate>

                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table id="itemPlaceholderContainer" class="table" runat="server" border="0" style="">
                                    <tr runat="server" style="text-align:center;">
                                        <th runat="server">ROUTE NAME</th>
                                        <th runat="server">ROUTE DATE</th>
                                        <th runat="server">ROUTE HOUR</th>
                                        <th runat="server">ROUTE CAPACITY</th>
                                        <th runat="server">SOLD TICKETS</th>
                                        <th runat="server">DRIVER NAME</th>
                                        <th runat="server">REVENUE (RWF)</th>
                                        <th runat="server">REVENUE (FIB)</th> 
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>

                <EmptyDataTemplate>
                    <table runat="server" style="">
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>

            <table style="border-top:solid #808080 1px;">
                <tr>
                    <td colspan="8" style="text-align:right"><b>Total Revenue Generated: </b></td>
                    <td style="width:130px; text-align:left;">
                        <b><asp:Label ID="lblSum" runat="server" Text="Label">0</asp:Label></b>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
