<%@ Page Language="VB" AutoEventWireup="false" CodeFile="enc.aspx.vb" Inherits="enc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Enter Password: <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" />

        The Encrypted Password is :
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

        <p>
        Enter Encrypted: <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="Button" />
        The Decrypted Password:
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            </p>
    </div>
    </form>
</body>
</html>
