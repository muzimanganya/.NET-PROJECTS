<%@ Page Title="Cards" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Cards.aspx.cs" Inherits="Cards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Drivers</title>
    <link href="Styles/gridview.css" rel="stylesheet" type="text/css" />
</asp:Content> 

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
     <asp:SqlDataSource ID="SqlDataSourceCards" runat="server" ConnectionString="<%$ ConnectionStrings:AppDBContext %>" 
        DeleteCommand="DELETE FROM Cards WHERE (CardNo = @CardNo)" 
        InsertCommand="INSERT INTO Cards(CardNo, Owner, PIN, Phone, Creator, Updater, Amount, IsActive) VALUES (@CardNo, @Owner, @PIN, @Phone, @Creator, @Updater, @Amount, @IsActive)" 
        SelectCommand="SELECT Cards.* FROM Cards" 
        UpdateCommand="UPDATE Cards SET Owner = @Owner, PIN = @PIN, Phone = @Phone, Creator = @Creator, Updater = @Updater, CardNo = @CardNo, IsActive=@IsActive, Amount=Amount+@Amount WHERE CardNo = @CardNo">
        
         <DeleteParameters>
            <asp:Parameter Name="CardNo" />
        </DeleteParameters>

        <InsertParameters>
            <asp:Parameter Name="CardNo" />
            <asp:Parameter Name="Owner" />
            <asp:Parameter Name="PIN" />
            <asp:Parameter Name="Phone" /> 
            <asp:Parameter Name="Amount" /> 
            <asp:Parameter Name="IsActive" /> 
            <asp:SessionParameter Name="Creator" SessionField="UserName" />
            <asp:SessionParameter Name="Updater" SessionField="UserName" />
        </InsertParameters>

        <UpdateParameters>
            <asp:Parameter Name="CardNo" />
            <asp:Parameter Name="Owner" />
            <asp:Parameter Name="PIN" />
            <asp:Parameter Name="Phone" /> 
            <asp:SessionParameter Name="Creator" SessionField="UserName" />
            <asp:SessionParameter Name="Updater" SessionField="UserName" />
        </UpdateParameters>
    </asp:SqlDataSource> 

     <asp:Panel ID="searchPanel" runat="server">
         <asp:Label ID="Label1" runat="server" Text="Label">Please type the Card Number you want to search Here: </asp:Label>
         <asp:TextBox ID="searchText" runat="server" Width="356px"></asp:TextBox>
         <asp:Button ID="searchButton" runat="server" Text="Filter cards" OnClick="searchButton_Click" />
         <asp:Button ID="resetButton" runat="server" Text="Reset Filter" OnClick="resetButton_Click" />
     </asp:Panel>

    <asp:ListView ID="cardsView" runat="server" DataKeyNames="CardNo"  DataSourceID="SqlDataSourceCards" InsertItemPosition="LastItem" 
            OnItemInserted="cardsView_ItemInserted" 
            OnItemEditing="cardsView_ItemEditing"
            OnItemUpdating="cardsView_ItemUpdating"
            OnItemUpdated="cardsView_ItemUpdated" 
            OnItemDeleting="cardsView_ItemDeleting" 
        >
        <AlternatingItemTemplate>
            <tr style=""> 
                <td>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("CardNo") %>' />&nbsp;&nbsp; 
                </td>
                <td>
                    <asp:Label ID="OwnerLabel" runat="server" Text='<%# Eval("Owner") %>' />
                </td> 
                <td>
                    <asp:CheckBox ID="IsActiveCheckBox" runat="server" Checked='<%# Eval("IsActive") %>' Enabled="false" />
                </td>
                <td>
                    <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone") %>' />
                </td> 
                <td>
                    <asp:Label ID="AmountLabel" runat="server" Text='<%# Eval("Amount", "{0:0,00}") %>' /> RWF
                </td>
                <td>
                    <asp:Label ID="PINLabel" runat="server" Text='<%# Eval("PIN") %>' />
                </td>
                <td>
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EditItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="CardNoLabel" runat="server" Text='<%# Eval("CardNo") %>' />
                </td>
                <td>
                    <asp:TextBox ID="OwnerTextBox" runat="server" Text='<%# Bind("Owner") %>' />
                </td> 
                <td>
                    <asp:CheckBox ID="IsActiveCheckBox" runat="server" Checked='<%# Bind("IsActive") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PhoneTextBox" runat="server" Text='<%# Bind("Phone") %>' />
                </td> 
                <td>
                    <asp:TextBox ID="AmountTextBox" runat="server" Text='<%# Bind("Amount", "{0:0,00}") %>' /> RWF
                </td>
                <td>
                    <asp:TextBox  ID="PINTextBoxUpdate" TextMode="Password" runat="server" Text='<%# Bind("PIN") %>' />
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
                    <asp:TextBox ID="CardNoTextBox" runat="server" Text='<%# Bind("CardNo") %>' />
                </td>
                <td>
                    <asp:TextBox ID="OwnerTextBox" runat="server" Text='<%# Bind("Owner") %>' />
                </td> 
                <td>
                    <asp:CheckBox ID="IsActiveCheckBox" runat="server" Checked='<%# Bind("IsActive") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PhoneTextBox" runat="server" Text='<%# Bind("Phone") %>' />
                </td> 
                <td>
                    <asp:TextBox ID="AmountTextBox" runat="server" Text='<%# Bind("Amount", "{0:0,00}") %>' /> 
                </td>
                <td>
                    <asp:TextBox TextMode="Password" ID="PINTextBoxInsert" runat="server" Text='<%# Bind("PIN") %>' />
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
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("CardNo") %>' />&nbsp;&nbsp; 
                </td>
                <td>
                    <asp:Label ID="OwnerLabel" runat="server" Text='<%# Eval("Owner") %>' />
                </td> 
                <td>
                    <asp:CheckBox ID="IsActiveCheckBox" runat="server" Checked='<%# Eval("IsActive") %>' Enabled="false" />
                </td>
                <td>
                    <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone") %>' />
                </td> 
                <td>
                    <asp:Label ID="AmountLabel" runat="server" Text='<%# Eval("Amount", "{0:0,00}") %>' /> RWF
                </td>
                <td>
                    <asp:Label ID="PINLabel" runat="server" Text='<%# Bind("PIN") %>' />
                </td> 
                <td> 
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete?');"  />
                </td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style=""> 
                                <th runat="server">Card No</th>
                                <th runat="server">Owner</th> 
                                <th runat="server">Is Active</th>
                                <th runat="server">Phone</th> 
                                <th runat="server">Amount</th>
                                <th runat="server">PIN</th>
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
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="CardNoLabel" runat="server" Text='<%# Eval("CardNo") %>' />
                </td>
                <td>
                    <asp:Label ID="OwnerLabel" runat="server" Text='<%# Eval("Owner") %>' />
                </td> 
                <td>
                    <asp:CheckBox ID="IsActiveCheckBox" runat="server" Checked='<%# Eval("IsActive") %>' Enabled="false" />
                </td>
                <td>
                    <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone") %>' />
                </td> 
                <td>
                    <asp:Label ID="AmountLabel" runat="server" Text='<%# Eval("Amount") %>' /> RWF
                </td>
                <td>
                    <asp:Label ID="PINLabel" runat="server" Text='<%# Eval("PIN") %>' /> RWF
                </td>
            </tr>
        </SelectedItemTemplate>

     </asp:ListView>

</asp:Content>

