<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="CourierDetails.aspx.vb" Inherits="CourierDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Package Details</title>
    <link href='http://fonts.googleapis.com/css?family=Droid+Serif|Open+Sans:400,700' rel='stylesheet' type='text/css'>
    <link href="/Styles/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Styles/Flags/othersgeneric.css" rel="stylesheet" />
    <link href="/Styles/Timeline.css" rel="stylesheet" />
    <style type="text/css">
        .noColors tr, noColors tr:hover,.noColors tr td:hover,.noColors tr td:nth-child(even),.noColors tr td:nth-child(odd) {
            background:transparent !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
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
            <div class="pagetitle icon-48-traffic">
                <h2>Package Management <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a class="" href="Couriers.aspx">Package Mangement</a>	</li>
                    <li><a class="" href="PackageMap.aspx">Package Map</a>	</li>
		            <li><a class="active" href="#">Package Details</a>	</li>		            
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left">
                <div class="m">

                    <asp:Repeater ID="rptDetails" runat="server">
                        <ItemTemplate>
                            <table class="table" style="width:50%;float:left">
                            <tr>
                                <td><b>Courier ID</b></td>
                                <td><%# Eval("CourierID")%></td>
                            </tr>
                            <tr>
                                <td><b>Route</b></td>
                                <td><%# Eval("Route")%></td>
                            </tr>
                            <tr>
                                <td><b>Sender Name</b></td>
                                <td><%# Eval("SenderName")%></td>
                            </tr>
                            <tr>
                                <td><b>Sender Phone</b></td>
                                <td><%# Eval("SenderPhone")%></td>
                            </tr>
                            <tr>
                                <td><b>Receiver Name</b></td>
                                <td><%# Eval("ReceiverName")%></td>
                            </tr>
                            <tr>
                                <td><b>Receiver Phone</b></td>
                                <td><%# Eval("ReceiverPhone")%></td>
                            </tr>
                            <tr>
                                <td><b>Current Status</b></td>
                                <td><i><%# Eval("StatusName")%></i></td>
                            </tr>
                            </table>
                            <div id="receipt" class="" style="background:white;display:none">
                                <div style="background:white;width:248px;text-align:left">
                                    <p>
                                        <b style="font-weight:900;font-size:48px">TRINITY</b>
                                    </p>
                                <div class="well-large" style="width: 208px;margin-bottom: -5px;"><span style="font-size:48px;text-align:center;width:300px;padding: 4px;font-style:italic">#<%# Eval("CourierID")%></span></div>
                                <div style="">
                                    <table class="table" style="background:white !important">
                                        <tr>
                                            <td><b>Route</b></td>
                                            <td><%# Eval("Route")%></td>
                                        </tr>
                                        <tr>
                                            <td><b>Sender</b></td>
                                            <td><%# Eval("SenderName")%></td>
                                        </tr>
                                        <tr>
                                            <td><b>Phone</b></td>
                                            <td><%# Eval("SenderPhone")%></td>
                                        </tr>
                                        <tr>
                                            <td><b>Receiver</b></td>
                                            <td><%# Eval("ReceiverName")%></td>
                                        </tr>
                                        <tr>
                                            <td><b>R.Phone</b></td>
                                            <td><%# Eval("ReceiverPhone")%></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"><b>Status</b><br/><i><%# Eval("StatusName")%></i></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <img src="#" id="qrCode" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Dial *777# and use tracking code to track the courier</td>
                                        </tr>
                                    </table>
                                </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div style="width:49.80%;float:right;text-align: center;background: white;height: 260px;">
                        <asp:Image ID="imgQr" runat="server" AlternateText="Loading QR Code" />
                        <br />
                        <a href="#" class="btn btn-large" onclick="printReceipt();">Print</a>
                    </div>
                </div>
            </div>
             <div class="cpanel-right">
                <div class="pane-sliders2" style="background:white">
                    <div class="formCon"><div id="piechart" style="min-height:200px;width:570px;padding:10px">
                        <asp:UpdatePanel ID="upAssign" runat="server">
                            <ContentTemplate>
                                <table class="table noColors">
                                    <tr>
                                        <td style="text-align: right">Courier ID</td>
                                        <td style="text-align: center">
                                            <asp:TextBox ID="txtCourierID" runat="server" CssClass="form-control input-sm" autocomplete="off" Placeholder="Courier ID" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Bus Date</td>
                                        <td style="text-align: center">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control input-sm" autocomplete="off" Placeholder="Date" OnTextChanged="txtDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" Format="yyyy-MM-dd">
                                            </asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDate" ValidationGroup="sellNew"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Select  Bus</td>
                                        <td style="text-align: center">
                                            <asp:DropDownList ID="ddlBuses" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvLevels" runat="server" ErrorMessage="" ControlToValidate="ddlBuses" InitialValue="-1">

                                            </asp:RequiredFieldValidator>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="1" style="text-align: center">
                                            <asp:Button ID="btnAssign" Enabled="false" runat="server" Text="Assign To Bus" CssClass="btn btn-primary" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                       <asp:UpdateProgress ID="upUpdate" runat="server" AssociatedUpdatePanelID="upAssign">
                <ProgressTemplate>
                    <div style="position: absolute; top: 41%; left: 55%; right: 0%; bottom: 22%; background: #fff; filter: alpha(opacity=50); opacity: 0.6;width: 43%;">
                        <div class="center-block">
                            <img src="/Images/blueloading.gif" width="100px" style="position: absolute; top: 40%; left: 47%;" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
                    </div></div>
                </div>
            </div>
        </div>
        <div class="m" id="body" style="margin-top:10px">
            <asp:Literal ID="ltrError" runat="server" Text=""></asp:Literal>
            <section id="cd-timeline" class="cd-container">
                <asp:Repeater ID="rptTimeline" runat="server">
                    <ItemTemplate>
                        <div class="cd-timeline-block">

                            <div class='cd-timeline-img <%# Eval("TimelineIcon")%>'>
                                <img src='/img/cd-icon-<%# Eval("Icon")%>' alt="Movie">
                            </div>
                            <!-- cd-timeline-img -->

                            <div class="cd-timeline-content">
                                <h2><%# Eval("Description")%></h2>
                                <p>Sender: <%# Eval("SenderName")%> on <%# Eval("CityIn")%>-<%# Eval("CityOut")%>  View position on <a href="#">map</a></p>
                                <span class="cd-date"><%# Eval("TDate")%></span>
                            </div>
                            <!-- cd-timeline-content -->
                        </div>
                    </ItemTemplate>
                    
                </asp:Repeater>
               
               

            </section>
        </div>
    </div>

<%--<script src="/Scripts/jquery-1.4.1.min.js"></script>--%>
    <script src="../Scripts/jquery-2.1.1.min.js"></script>
<script type="text/javascript">

$(function () {
    //$(window).on('scroll', function () {
    //    $timeline_block.each(function () {
    //        if ($(this).offset().top <= $(window).scrollTop() + $(window).height() * 0.75 && $(this).find('.cd-timeline-img').hasClass('is-hidden')) {
    //            $(this).find('.cd-timeline-img, .cd-timeline-content').removeClass('is-hidden').addClass('bounce-in');
    //        }
    //    });
    //});
    
    var $timeline_block = $('.cd-timeline-block');

    //hide timeline blocks which are outside the viewport
    $timeline_block.each(function () {
        if ($(this).offset().top > $(window).scrollTop() + $(window).height() * 0.75) {
            $(this).find('.cd-timeline-img, .cd-timeline-content').addClass('is-hidden');
        }
    });

    //on scolling, show/animate timeline blocks when enter the viewport
    $(window).on('scroll', function () {
        $timeline_block.each(function () {
            if ($(this).offset().top <= $(window).scrollTop() + $(window).height() * 0.75 && $(this).find('.cd-timeline-img').hasClass('is-hidden')) {
                $(this).find('.cd-timeline-img, .cd-timeline-content').removeClass('is-hidden').addClass('bounce-in');
            }
        });
    });
});

function printReceipt() {
    //Load the QR Code
    $("#qrCode").attr('src', $("#body_content_imgQr").attr("src"));
    //Start a new window to print the items

    var newWindow = window.open();
    
    newWindow.document.write($("#receipt").html());
    //newWindow.print();
}
</script>
</asp:Content>

