<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Invoices Portal | Main Portal</title> 
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="element-box">
       <div class="m">
        <div class="adminform">
            <div class="cpanel-left-home">
                <div id="position-icon" class="pane-sliders">
                    <div class="panel">
                        <h3 class="title pane-toggler-down" id="module9"><a href="#"><span>Select Company to invoice</span></a></h3>
                        <div style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;">
                            <div class="cpanel">  
                                   <asp:Panel runat="server" CssClass="icon-wrapper" ID="InvoicesDiv" > </asp:Panel> 
                             </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel"> 
                        <div style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;">
                            <div class="cpanel">
                                        <div style="padding:10px;">

                                            <div id="messages">
                                                <img src="Styles/PoweredBy.jpg" />
                                                <div style="width:163px;float:right">                                                   
                                                
                                            </div>

                                            </div>
                                        </div>
                                    </div>        
                        </div>
                    </div>
                </div>
            </div>
            <div class="cpanel-right-home">
                <div id="panel-sliders" class="pane-sliders">
                    <div class="panel">
                        <h3 class="title pane-toggler-down" id="cpanel-panel-logged" onclick="javascript:HandleToggles(1);"><a href="javascript:HandleToggles(1);">
                            <!--span>This Weeks Sales Trend</!--span></a></h3-->
                        <div class="one" style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;">
                            <div id="chart_div" style="width:100%;height:360px">
                                <div id="Div1" style="width:570px;height:300px">
                                     
                                </div>
                            </div>
                        </div>
                    </div> 
                    
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>


