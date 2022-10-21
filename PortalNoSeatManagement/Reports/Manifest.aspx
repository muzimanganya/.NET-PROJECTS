<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Manifest.aspx.vb" Inherits="Reports_Manifest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Passenger Manifest</title>
    <style>
    td.printTitle {
        font-family: Verdana,Arial,sans-serif;
        font-size: 16pt;
        font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="frmManifest" runat="server">
    <div>
    <table width="750px">
        <tr><td>
                <table>
                    <tr>
                        <td align="left"><img src="../Styles/logo.png" /></td>
                        <td align="right" class="printTitle">Passenger Manifest</td>
                    </tr>
                    <tr>
                        <td>KIGALI BUS SERVICE</td>
                    </tr>
                    <tr>
                        <td width="95%">
                            <hr size=1 noshade/>
                        </td>
                        <td><asp:Literal ID="busname" runat="server"></asp:Literal></td>
                    </tr>
                </table>
                </td>
                </tr>
        <asp:Repeater ID="rptManifest" runat="server">
            <HeaderTemplate>
                
                    <tr>
                        <th>TicketID</th>
                        <th>From</th>
                        <th>To</th>
                        <th>ID / Passport</th>
                        <th>Nationality</th>
                        <th>Total</th>
                        <th>Reserved By</th>
                    </tr>
                
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("IDRELATION")%></td>
                    <td><%# Eval("CityIN")%></td>
                    <td><%# Eval("CityOut")%></td>
                    <td><%# Eval("id")%></td>
                    <td><%# Eval("Nationality")%></td>
                    <td><%# Eval("Total")%></td>
                    <td><%# Eval("CreatedBy")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                
            </FooterTemplate>
        </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
