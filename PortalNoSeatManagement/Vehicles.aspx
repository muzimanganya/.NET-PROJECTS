<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Vehicles.aspx.cs" MasterPageFile="~/main.master" Inherits="Vehicles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>User Sales Per Bus</title>
    <link href="Styles/gridview.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">

    <asp:SqlDataSource ID="SqlDataSourceVehicles" runat="server" ConnectionString="<%$ ConnectionStrings:AppDBContext %>" 
        SelectCommand="SELECT Vehicles.* FROM Vehicles" 
        DeleteCommand="DELETE FROM Vehicles WHERE PLATENO=@PLATENO" 
        InsertCommand="INSERT INTO Vehicles(PLATENO, NAME, Comment, Memo, Photo) VALUES (@PLATENO, @NAME, @Comment, @Memo, @Photo)" 
        UpdateCommand="UPDATE Vehicles SET PLATENO = @PLATENO, NAME =@NAME, Comment = @Comment, Memo = @Memo, Photo = @Photo">
    </asp:SqlDataSource>

    <asp:ListView ID="ListView1" runat="server" DataKeyNames="PLATENO" DataSourceID="SqlDataSourceVehicles" InsertItemPosition="LastItem">
        <AlternatingItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="PLATENOLabel" runat="server" Text='<%# Eval("PLATENO") %>' />
                </td>
                <td>
                    <asp:Label ID="NAMELabel" runat="server" Text='<%# Eval("NAME") %>' />
                </td>
                <td>
                    <asp:Label ID="CommentLabel" runat="server" Text='<%# Eval("Comment") %>' />
                </td>
                <td>
                    <asp:Label ID="MemoLabel" runat="server" Text='<%# Eval("Memo") %>' />
                </td>
                <td>
                    <asp:Label ID="PhotoLabel" runat="server" Text='<%# Eval("Photo") %>' />
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
                    <asp:Label ID="PLATENOLabel1" runat="server" Text='<%# Eval("PLATENO") %>' />
                </td>
                <td>
                    <asp:TextBox ID="NAMETextBox" runat="server" Text='<%# Bind("NAME") %>' />
                </td>
                <td>
                    <asp:TextBox ID="CommentTextBox" runat="server" Text='<%# Bind("Comment") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MemoTextBox" runat="server" Text='<%# Bind("Memo") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PhotoTextBox" runat="server" Text='<%# Bind("Photo") %>' />
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
                    <asp:TextBox ID="PLATENOTextBox" runat="server" Text='<%# Bind("PLATENO") %>' />
                </td>
                <td>
                    <asp:TextBox ID="NAMETextBox" runat="server" Text='<%# Bind("NAME") %>' />
                </td>
                <td>
                    <asp:TextBox ID="CommentTextBox" runat="server" Text='<%# Bind("Comment") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MemoTextBox" runat="server" Text='<%# Bind("Memo") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PhotoTextBox" runat="server" Text='<%# Bind("Photo") %>' />
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
                    <asp:Label ID="PLATENOLabel" runat="server" Text='<%# Eval("PLATENO") %>' />
                </td>
                <td>
                    <asp:Label ID="NAMELabel" runat="server" Text='<%# Eval("NAME") %>' />
                </td>
                <td>
                    <asp:Label ID="CommentLabel" runat="server" Text='<%# Eval("Comment") %>' />
                </td>
                <td>
                    <asp:Label ID="MemoLabel" runat="server" Text='<%# Eval("Memo") %>' />
                </td>
                <td>
                    <asp:Label ID="PhotoLabel" runat="server" Text='<%# Eval("Photo") %>' />
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
                        <table id="itemPlaceholderContainer" class="table" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th runat="server">Plate Number</th>
                                <th runat="server">Name</th>
                                <th runat="server">Comment</th>
                                <th runat="server">Memo</th>
                                <th runat="server">Photo</th>
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
                    <asp:Label ID="PLATENOLabel" runat="server" Text='<%# Eval("PLATENO") %>' />
                </td>
                <td>
                    <asp:Label ID="NAMELabel" runat="server" Text='<%# Eval("NAME") %>' />
                </td>
                <td>
                    <asp:Label ID="CommentLabel" runat="server" Text='<%# Eval("Comment") %>' />
                </td>
                <td>
                    <asp:Label ID="MemoLabel" runat="server" Text='<%# Eval("Memo") %>' />
                </td>
                <td>
                    <asp:Label ID="PhotoLabel" runat="server" Text='<%# Eval("Photo") %>' />
                </td>
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView>

</asp:Content>