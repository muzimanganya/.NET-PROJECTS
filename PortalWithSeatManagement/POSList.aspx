<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="POSList.aspx.vb" Inherits="POSList" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Traffic & Business</title>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.paginate.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
           // $('#masterview').paginateTable({ rowsPerPage: 10 });
  //          $("#nextBtn").click();
//            $('#prevBtn').click();
        });
        function paginate(n) {
            $('#masterview').paginateTable({ rowsPerPage: n });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    <li class="button" id="toolbar-new">
                        <a href="NewBusRoute.aspx" onclick="" class="toolbar">
                        <span class="icon-32-new"></span>New</a>
                    </li>
                   
                    <li class="divider"></li>
                    <li class="button" id="pos">
                        <a href="POSTicketing.aspx" onclick="" class="toolbar">
                        <span class="icon-32-pos">
                            <img src="Styles/icon-48-media.png" width="32px" />
                        </span>POS Ticketing</a>  
                    </li>
                    <li class="button" id="booking">
                        <a href="POSBooking.aspx" onclick="" class="toolbar">
                        <span class="icon-32-booking">
                            <img src="Styles/icon-48-new-privatemessage.png" width="32px"/>
                        </span>Booking</a>  
                    </li>
                    <li class="button" id="Li2">
                        <a href="POSSubscription.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="Styles/icon-32-inbox.png" width="32px"/>
                        </span>Subscriptions</a>  
                    </li>
                    <li class="button" id="promo">
                        <a href="POSPromotions.aspx" onclick="" class="toolbar">
                        <span class="icon-32-promo">
                            <img src="Styles/icon-48-checkin.png" width="32px" />
                        </span>Promotions</a>  
                    </li>
                    <li class="divider"></li>
                    <li class="button" id="Li1">
                        <a href="#" onclick="" class="toolbar">
                        <span class="icon-32-help"></span>Help</a>  
                    </li>
                </ul>

            </div>
            <div class="pagetitle icon-48-hit">
                <h2>POS Machines</h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a class="active" href="#">POS List</a>	</li>
		            <li><a href="POSTicketing.aspx">POS Ticketing</a>	</li>
		            <li><a href="POSBooking.aspx">POS Booking</a>	</li>
                    <li><a href="POSPromotions.aspx">POS Promotions</a>	</li>
                    <li><a href="POSSubscription.aspx">POS Subscription</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div class="cpanel-left" style="width:66% !important">
                <div class="m">
                     <asp:Repeater ID="rptPosList" runat="server">
                <HeaderTemplate>
                    <table id="masterview" width="100%">
                        <thead>
                        <tr>
                            <th><a href="#">Open Report</a></th>
                            <th><a href="#">POS Number</a></th>
                            <th><a href="#">MSISDN</a></th>
                            <th><a href="#">Sim Number</a></th>
                            <th><a href="#">User Phone No</a></th>
                            <th><a href="#">Place</a></th>
                            <th><a href="#">Description</a></th>
                        </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="chkReport" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Report")%>' AutoPostBack=true posid="" />
                            <p class="smallsub">
                            
                            </p>
                        </td>
                        <td class="center"><a href='Editing/EditPOS.aspx?pos=<%# DataBinder.Eval(Container.DataItem, "NAME")%>'><%# DataBinder.Eval(Container.DataItem, "NAME")%></a></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "MSISDN")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "SimNum")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "UserMobile")%></td>
                        <td class="center"><%# DataBinder.Eval(Container.DataItem, "Place")%></td>
                        <td align="left"><%# DataBinder.Eval(Container.DataItem, "Memo")%></td>
                    </tr>
                </ItemTemplate>
                
                <FooterTemplate>
                    
                    <tr>
                        <td colspan=11>
                           
                        </td>
                    </tr>
                    </tbody>
                    </table>

                </FooterTemplate>
            </asp:Repeater> 
                     <div class='pager'>
                            <div class="limit">Display #
                                <select id="limit" name="limit" class="inputbox" size="1" onchange="paginate(this.value);">
	                                <option value="10" selected="selected">10</option>
	                                <option value="20">20</option>
	                                <option value="30">30</option>
	                                <option value="50">50</option>
	                                <option value="100">100</option>
	                                
                                </select>
                            </div>
                            <div style="float:left;width:150px;font-size:14px;margin-top:4px;">
                             <a href='#' alt='Previous' id="prevBtn" class='prevPage'>Prev</a>
                                <span class='currentPage'></span> of <span class='totalPages'></span>
                               <a href='#' alt='Next' id="nextBtn" class='nextPage'>Next</a>
                               </div>
                     </div>
                     
                </div>
                <p></p>
               
            </div>
            <div class="cpanel-right" style="width:33% !important;">
                <div class="pane-sliders2" style="background:white">
                    <div class="panel"><div id="piechart" style="min-height:300px;width:430px;">Pie Chart Comes Here</div></div>
                </div>

                <div class="m">
                    <table style="clear:both">
                     <tr>
                        <td colspan ="3" align="center">
                            <asp:Button ID="btnAcceptAll" runat="server" Text="Accept All Report" CommandArgument="1" OnClick="Button_OnClick"/>
                        </td>
                        <td colspan ="3" align="center">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel All Report" CommandArgument="2" OnClick="Button_OnClick" />
                        </td>
                    </tr>
                     </table>
                </div>
                <br />
                 <div class="m">
                    <asp:Repeater ID="rptSummary" runat="server">
                        <ItemTemplate>
                           <div style="float:right;font-size:14px;color:red;font-weight:bolder;">Total Sales Today = RWF <%# DataBinder.Eval(Container.DataItem, "Total", "{0:n2}")%></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>

