<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Drivers.aspx.cs" MasterPageFile="~/main.master" Inherits="Models_Drivers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Drivers</title>
    <link href="Styles/gridview.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">

    <asp:SqlDataSource ID="SqlDataSourceDrivers" runat="server" ConnectionString="<%$ ConnectionStrings:AppDBContext %>" DeleteCommand="DELETE FROM Drivers WHERE (DriverID = @DriverID)" 
        InsertCommand="INSERT INTO Drivers(DriverName, Phone, Address, Email, Photo, Memo) VALUES (@DriverName, @Phone, @Address, @Email, @Photo, @Memo)" 
        SelectCommand="SELECT Drivers.* FROM Drivers" 
        UpdateCommand="UPDATE Drivers SET DriverName = @DriverName, Phone = @Phone, Address = @Address, Email = @Email, Photo = @Photo, Memo = @memo">
        <DeleteParameters>
            <asp:Parameter Name="DriverID" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="DriverName" />
            <asp:Parameter Name="Phone" />
            <asp:Parameter Name="Address" />
            <asp:Parameter Name="Email" />
            <asp:Parameter Name="Photo" />
            <asp:Parameter Name="Memo" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="DriverName" />
            <asp:Parameter Name="Phone" />
            <asp:Parameter Name="Address" />
            <asp:Parameter Name="Email" />
            <asp:Parameter Name="Photo" />
            <asp:Parameter Name="memo" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:ListView ID="ListView1" runat="server" DataKeyNames="DriverID" DataSourceID="SqlDataSourceDrivers" InsertItemPosition="LastItem">
        <AlternatingItemTemplate>
            <tr style=""> 
                <td>
                    <asp:Label ID="DriverNameLabel" runat="server" Text='<%# Eval("DriverName") %>' />
                </td>
                <td>
                    <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone") %>' />
                </td>
                <td>
                    <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                </td>
                <td>
                    <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                </td>
                <td>
                    <asp:Label ID="PhotoLabel" runat="server" Text='<%# Eval("Photo") %>' />
                </td>
                <td>
                    <asp:Label ID="MemoLabel" runat="server" Text='<%# Eval("Memo") %>' />
                </td>
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EditItemTemplate>
            <tr style=""> 
                <td>
                    <asp:TextBox ID="DriverNameTextBox" runat="server" Text='<%# Bind("DriverName") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PhoneTextBox" runat="server" Text='<%# Bind("Phone") %>' />
                </td>
                <td>
                    <asp:TextBox ID="AddressTextBox" runat="server" Text='<%# Bind("Address") %>' />
                </td>
                <td>
                    <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PhotoTextBox" runat="server" Text='<%# Bind("Photo") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MemoTextBox" runat="server" Text='<%# Bind("Memo") %>' />
                </td>
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr style=""> 
                <td>
                    <asp:TextBox ID="DriverNameTextBox" runat="server" Text='<%# Bind("DriverName") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PhoneTextBox" runat="server" Text='<%# Bind("Phone") %>' />
                </td>
                <td>
                    <asp:TextBox ID="AddressTextBox" runat="server" Text='<%# Bind("Address") %>' />
                </td>
                <td>
                    <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PhotoTextBox" runat="server" Text='<%# Bind("Photo") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MemoTextBox" runat="server" Text='<%# Bind("Memo") %>' />
                </td>
                <td>
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr style=""> 
                <td>
                    <asp:Label ID="DriverNameLabel" runat="server" Text='<%# Eval("DriverName") %>' />
                </td>
                <td>
                    <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone") %>' />
                </td>
                <td>
                    <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                </td>
                <td>
                    <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                </td>
                <td>
                    <asp:Label ID="PhotoLabel" runat="server" Text='<%# Eval("Photo") %>' />
                </td>
                <td>
                    <asp:Label ID="MemoLabel" runat="server" Text='<%# Eval("Memo") %>' />
                </td>
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style=""> 
                                <th runat="server">Name</th>
                                <th runat="server">Phone</th>
                                <th runat="server">Address</th>
                                <th runat="server">Email</th>
                                <th runat="server">Photo</th>
                                <th runat="server">Memo</th>
                                <th runat="server">Actions</th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="">
                        <asp:DataPager ID="DataPager1" runat="server">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <tr style=""> 
                <td>
                    <asp:Label ID="DriverNameLabel" runat="server" Text='<%# Eval("DriverName") %>' />
                </td>
                <td>
                    <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone") %>' />
                </td>
                <td>
                    <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                </td>
                <td>
                    <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                </td>
                <td>
                    <asp:Label ID="PhotoLabel" runat="server" Text='<%# Eval("Photo") %>' />
                </td>
                <td>
                    <asp:Label ID="MemoLabel" runat="server" Text='<%# Eval("Memo") %>' />
                </td>
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView>

</asp:Content>