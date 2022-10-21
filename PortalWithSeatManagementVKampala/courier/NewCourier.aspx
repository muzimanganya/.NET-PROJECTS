<%@ Page Title="New Courier or Luggage" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="NewCourier.aspx.vb" Inherits="courier_NewCourier" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
     <title>New Courier</title>
    <link href="/Styles/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="/Styles/Flags/othersgeneric.css" rel="stylesheet" />
    <link href="/Styles/Timeline.css" rel="stylesheet" />
    <style type="text/css">
        .noColors tr, noColors tr:hover,.noColors tr td:hover,.noColors tr td:nth-child(even),.noColors tr td:nth-child(odd) {
            background:transparent !important;
        }
    </style>
    <script src="../Scripts/angular.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="NewCourier.aspx" onclick="" class="toolbar">
                            <span class="icon-32-new"></span>New</a>
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
                                <img src="/Styles/icon-48-new-privatemessage.png" width="32px" />
                            </span>Booking</a>
                    </li>
                    <li class="button" id="Li2">
                        <a href="POSSubscription.aspx" onclick="" class="toolbar">
                            <span class="icon-32-promo">
                                <img src="/Styles/icon-32-inbox.png" width="32px" />
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
            <div class="pagetitle icon-48-traffic">
                <h2>Courier Management
                    <asp:Literal ID="headerDate" runat="server"></asp:Literal>
                </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
        <div class="m">
            <ul id="submenu">
                <li><a class="" href="Couriers.aspx">Package Mangement</a>	</li>
                <li><a class="active" href="#">Add Package</a>	</li>
            </ul>
            <div class="clr"></div>
        </div>
    </div>
    <div id="element-box" ng-app>
        <div class="m">
            <div class="cpanel-left" style="width: 65%">
                <div class="m">
                    <div class="pane-sliders2" style="background: white">
                        <div class="formCon form form-horizontal">
                            <div id="piechart" style="min-height: 200px; width: 100%; padding: 10px">
                                <asp:UpdatePanel ID="upAssign" runat="server">
                                    <ContentTemplate>
                                        <div style="width: 50%; float: left">
                                            <div class="form-group">
                                                <asp:Label ID="lblSenderID" runat="server" AssociatedControlID="txtSenderID" CssClass="col-sm-5 control-label">Sender ID</asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtSenderID" runat="server" CssClass="form-control" ng-model="senderID"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtxtSenderID" runat="server" ErrorMessage="*" ControlToValidate="txtSenderID"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblSenderName" runat="server" AssociatedControlID="txtSenderName" CssClass="col-sm-5 control-label">Sender Name</asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtSenderName" runat="server" CssClass="form-control" ng-model="SenderName"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvSenderName" runat="server" ErrorMessage="*" ControlToValidate="txtSenderName"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtSenderPhone" CssClass="col-sm-5 control-label">Sender Phone</asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtSenderPhone" runat="server" CssClass="form-control" ng-model="SenderPhone"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvSenderPhone" runat="server" ErrorMessage="*" ControlToValidate="txtSenderPhone"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label2" runat="server" AssociatedControlID="txtReceiverName" CssClass="col-sm-5 control-label">Receiver Name</asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtReceiverName" runat="server" CssClass="form-control" ng-model="ReceiverName"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtxtReceiverName" runat="server" ErrorMessage="*" ControlToValidate="txtReceiverName"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblReceiverPhone" runat="server" AssociatedControlID="txtReceiverPhone" CssClass="col-sm-5 control-label">Receiver Phone</asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtReceiverPhone" runat="server" CssClass="form-control" ng-model="ReceiverPhone"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtReceiverPhone"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblCityIn" runat="server" AssociatedControlID="ddlCityIn" CssClass="col-sm-5 control-label">Select City In</asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCityIn" runat="server" CssClass="form-control input-sm" ng-model="CityIn"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvPrinter" runat="server" ErrorMessage="" ControlToValidate="ddlCityIn" InitialValue="--Select City--"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblCityOut" runat="server" AssociatedControlID="ddlCityOut" CssClass="col-sm-5 control-label">Select City Out</asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCityOut" runat="server" CssClass="form-control input-sm" ng-model="CityOut"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvCityOut" runat="server" ErrorMessage="" ControlToValidate="ddlCityOut" InitialValue="--Select City--"></asp:RequiredFieldValidator>
                                                    <span style="color:red">
                                                        <asp:Literal ID="ltrError" runat="server"></asp:Literal>
                                                    </span>
                                                    
                                                </div>
                                            </div>
                                            
                                        </div>
                                        <div style="width: 50%; float: left">
                                            <div class="form-group">
                                                <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" CssClass="col-sm-5 control-label">Package Type</asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" ng-model="ParcelType">
                                                        <asp:ListItem Text="Courier" Value="Courier"></asp:ListItem>
                                                        <asp:ListItem Text="Luggage" Value="Luggage"></asp:ListItem>
                                                    </asp:DropDownList>

                                                    <asp:RequiredFieldValidator ID="rfvType" runat="server" ErrorMessage="" ControlToValidate="ddlType" InitialValue="-1"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlLuggage" runat="server" Visible="false">
                                                <div class="form-group">
                                                    <asp:Label ID="lblLuggageType" runat="server" AssociatedControlID="ddlLuggageType" CssClass="col-sm-5 control-label">Luggage Type</asp:Label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlLuggageType" runat="server" CssClass="form-control input-sm" ng-model="LuggageType">
                                                            <asp:ListItem Text="Electronic" Value="Electronic"></asp:ListItem>
                                                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                        </asp:DropDownList>

                                                        <asp:RequiredFieldValidator ID="rfvLuggageType" runat="server" ErrorMessage="" ControlToValidate="ddlLuggageType" InitialValue="-1"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lblWeight" runat="server" AssociatedControlID="txtWeight" CssClass="col-sm-5 control-label">Weight</asp:Label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" ng-model="Weight"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvWeight" runat="server" ErrorMessage="*" ControlToValidate="txtWeight"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lblHeight" runat="server" AssociatedControlID="txtHeight" CssClass="col-sm-5 control-label">Height</asp:Label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control" ng-model="Height"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvHeight" runat="server" ErrorMessage="*" ControlToValidate="txtHeight"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lblWidth" runat="server" AssociatedControlID="txtWidth" CssClass="col-sm-5 control-label">Width</asp:Label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtWidth" runat="server" CssClass="form-control" ng-model="Width"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvWidth" runat="server" ErrorMessage="*" ControlToValidate="txtWidth"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lblLength" runat="server" AssociatedControlID="txtLength" CssClass="col-sm-5 control-label">Length</asp:Label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtLength" runat="server" CssClass="form-control" ng-model="Length"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtLength" runat="server" ErrorMessage="*" ControlToValidate="txtLength"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </asp:Panel>


                                        </div>
                                        <div style="clear:both"></div>
                                        <div class="form-group">
                                            <div class="col-sm-offset-2 col-sm-10" style="margin-left: 174px;margin-top: 10px;">

                                                <asp:Button ID="btnSubmit" runat="server" autocomplete="off" Text="Add Package" CssClass="btn btn-primary" CausesValidation="true"/>
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default"/>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="upUpdate" runat="server" AssociatedUpdatePanelID="upAssign">
                                    <ProgressTemplate>
                                        <div style="position: absolute;top: 39%;left: 3%;right: 0%;bottom: -12%;background: #fff;filter: alpha(opacity=50);opacity: 0.6;width: 60%;height:240px;">
                                            <div class="center-block">
                                                <img src="/Images/blueloading.gif" width="100px" style="position: absolute; top: 40%; left: 47%;" />
                                            </div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="cpanel-right" style="width: 35%">



                <div style="width: 100%; float: right; text-align: center; background: white; height: 260px;">
                    <h3>{{ParcelType}} Info</h3>
                    <table class="table" style="background: white !important" id="tblReceipt">
                        <tr>
                            <td><b>Route</b></td>
                            <td>{{CityIn}}-{{CityOut}}</td>
                        </tr>
                        <tr>
                            <td><b>Sender</b></td>
                            <td>{{SenderName}}</td>
                        </tr>
                        <tr>
                            <td><b>Phone</b></td>
                            <td>{{SenderPhone}}</td>
                        </tr>
                        <tr>
                            <td><b>Receiver</b></td>
                            <td>{{ReceiverName}}</td>
                        </tr>
                        <tr>
                            <td><b>R.Phone</b></td>
                            <td>{{ReceiverPhone}}</td>
                        </tr>
                        <tr>
                            <td><b>Weight</b></td>
                            <td>{{Weight}}</td>
                        </tr>
                        <tr>
                            <td><b>Volume</b></td>
                            <td>{{Volume}}</td>
                        </tr>
                        <tr>
                            <td><b>Status</b><br /></td>
                            <td><i>{{StatusName}}</i></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

    </div>
    <script src="../Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript">
        $(function () {
            if (typeof ValidatorUpdateDisplay != 'undefined') {
                var originalValidatorUpdateDisplay = ValidatorUpdateDisplay;

                ValidatorUpdateDisplay = function (val) {
                    if (!val.isvalid) {
                        $("#" + val.controltovalidate).parent().parent().addClass("has-error");
                        $("#" + val.controltovalidate).siblings(".help-block").css("display", "block");
                    }
                    else {
                        $("#" + val.controltovalidate).parent().parent().removeClass("error").addClass("has-success");
                        $("#" + val.controltovalidate).siblings(".help-block").css("display", "none");
                    }
                    originalValidatorUpdateDisplay(val);
                }
            }
        })
        
    </script>
</asp:Content>

