<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Sinovys Portal | Main Portal</title>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        var one = 1;
        var two = 0;
        var three = 0;
        function HandleToggles(n) {
            if (n == 1) {
                $('div.two').slideUp('fast');
                $('div.three').slideUp('fast');
                $('div.four').slideUp('fast');
                $('div.five').slideUp('fast');
                $('div.one').fadeIn('slow');
            }
            else if (n == 2) {
                $('div.one').slideUp('fast');
                $('div.three').slideUp('fast');
                $('div.four').slideUp('fast');
                $('div.five').slideUp('fast');
                $('div.two').fadeIn('slow');
            }
            else if (n == 3) {
                $('div.one').slideUp('fast');
                $('div.two').slideUp('fast');
                $('div.four').slideUp('fast');
                $('div.five').slideUp('fast');
                $('div.three').fadeIn('slow');
            }
            else if (n == 4) {
                $('div.one').slideUp('fast');
                $('div.two').slideUp('fast');
                $('div.three').slideUp('fast');
                $('div.five').slideUp('fast');
                $('div.four').fadeIn('slow');
            }
            else if (n == 5) {
                $('div.one').slideUp('fast');
                $('div.two').slideUp('fast');
                $('div.three').slideUp('fast');
                $('div.four').slideUp('fast');
                $('div.five').fadeIn('slow');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div id="element-box">
       <div class="m">
        <div class="adminform">
            <div class="cpanel-left-home">
                <div id="position-icon" class="pane-sliders">
                    <div class="panel">
                        <h3 class="title pane-toggler-down" id="module9"><a href=""><span>Quick Access Icons</span></a></h3>
                        <div style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;">
                            <div class="cpanel">
                                <div class="icon-wrapper">
                                    <div class="icon">
                                        <a href="traffic.aspx">
                                            <img src="Styles/icon-48-module.png" />
                                            <span>Traffic & Business</span>
                                        </a>
                                    </div>
                                    <div class="icon">
                                        <a href="POSTicketing.aspx">
                                            <img src="Styles/icon-48-media.png" />
                                            <span>POS Ticketing</span>
                                        </a>
                                    </div> 
                                    <div class="icon">
                                        <a href="POSBooking.aspx">
                                            <img src="Styles/icon-48-new-privatemessage.png" />
                                            <span>Bookings</span>
                                        </a>
                                    </div>
                                    <div class="icon">
                                        <a href="POSPromotions.aspx">
                                            <img src="Styles/icon-48-checkin.png" />
                                            <span>Promotions</span>
                                        </a>
                                    </div>
                                    
                                    <div class="icon">
                                        <a href="POSSubscription.aspx">
                                            <img src="Styles/icon-32-inbox.png" />
                                            <span>POS Subscription</span>
                                        </a>
                                    </div>
                                    <div class="icon">
                                        <a href="MobileTickets.aspx">
                                            <img src="Styles/icon-48-read-privatemessage.png" />
                                            <span>Mobile Ticket </span>
                                        </a>
                                    </div>
                                    <div class="icon">
                                        <a href="/Reports/Reports.aspx">
                                            <img src="Styles/icon-48-article.png" />
                                            <span>Reports</span>
                                        </a>
                                    </div>
                                    <div class="icon">
                                        <a href="QueryDB.aspx">
                                            <img src="Styles/icon-48-search.png" />
                                            <span>Database Queries</span>
                                        </a>
                                    </div>

                                    

                                    <div class="icon">
                                        <a href="javascript:void(window.open('https://web.innovys.co.rw/livezilla/chat.php?acid=1c463','','width=590,height=760,left=0,top=0,resizable=yes,menubar=no,location=no,status=yes,scrollbars=yes'))">
                                            <img src="Styles/icon-48-themes.png" />
                                            <span>Communication Portal</span>
                                        </a>
                                    </div>
                                    
                                    <div class="icon">
                                        <a href="Configuration.aspx">
                                            <img src="Styles/icon-48-user.png" />
                                            <span>Your Account Info</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel">
                        <h3 class="title pane-toggler-down" id="H3"><a href=""><span>Live Chat</span></a></h3>
                        <div style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;">
                            <div class="cpanel">
                                        <div style="padding:10px;">

                                            <div id="messages">
                                                <img src="Account/style/logo.gif" />
                                                <div style="width:163px;float:right">
                                                    
                                                Chat with us now!
                                                <div id="livezilla">
                                                <!-- LiveZilla Chat Button Link Code (ALWAYS PLACE IN BODY ELEMENT) -->
                                                <a name="livezilla_chat_button" href="javascript:void(window.open('https://web.innovys.co.rw/livezilla/chat.php?acid=1c463','','width=590,height=760,left=0,top=0,resizable=yes,menubar=no,location=no,status=yes,scrollbars=yes'))" class="lz_cbl"><img src="https://web.innovys.co.rw/livezilla/image.php?acid=92cc0&amp;id=1&amp;type=inlay" width="160" height="40" style="border:0px;" alt="LiveZilla Live Chat Software" /></a>
                                                
                                                <!-- LiveZilla Tracking Code (ALWAYS PLACE IN BODY ELEMENT) --><div id="livezilla_tracking" style="display:none"></div><script type="text/javascript">
                                                /* <![CDATA[ */
                                                var script = document.createElement("script");script.async=true;script.type="text/javascript";
                                                var src = "https://web.innovys.co.rw/livezilla/server.php?acid=d1022&request=track&output=jcrpt&nse="+Math.random();
                                                setTimeout("script.src=src;document.getElementById('livezilla_tracking').appendChild(script)",1);
                                                /* ]]> */
                                                </script>
                                            <noscript>
                                            <img src="http://web.innovys.co.rw/livezilla/server.php?acid=d1022&amp;request=track&amp;output=nojcrpt" width="0" height="0" style="visibility:hidden;" alt="" />
                                            </noscript></div>
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
                        <h3 class="title pane-toggler-down" id="cpanel-panel-logged" onclick="javascript:HandleToggles(1);"><a href="javascript:HandleToggles(1);"><span>This Weeks Sales Trend</span></a></h3>
                        <div class="one" style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;">
                            <div id="chart_div" style="width:100%;height:360px">
                                <div id="Div1" style="width:570px;height:300px">
                                    <div style="margin:auto;width:250px" ><img src="Styles/loadingcircle.gif" alt="Loading Chart"/></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel">
                        <h3 class="title pane-toggler" id="H5" onclick="javascript:HandleToggles(5);"><a href="javascript:HandleToggles(5);"><span>Tigo Cash Sales Weekly Trend</span></a></h3>
                        <div class="five" style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;display:none;">
                            <div id="Div5" style="width:100%;height:360px;">
                                <div id="tigoChart" style="width:570px;height:300px">
                                    <div style="margin:auto;width:250px" ><img src="Styles/loadingcircle.gif" alt="Loading Chart"/></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel">
                        <h3 class="title pane-toggler" id="H1" onclick="javascript:HandleToggles(2);"><a href="javascript:HandleToggles(2);"><span>Todays Top 5 Sellers</span></a></h3>
                        <div class="two" id="bs" style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;display:none;">
                            <div id="piechart" style="width:100%;height:360px">
                                 <div id="Div2" style="width:570px;height:300px">
                       <div style="margin:auto;width:250px" ><img src="Styles/loadingcircle.gif" alt="Loading Chart"/></div>
                   </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel">
                        <h3 class="title pane-toggler" id="H2" onclick="javascript:HandleToggles(3);"><a href="javascript:HandleToggles(3);"><span>Today's Best Routes / Revenue</span></a></h3>
                        <div class="three" style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;display:none;">
                            <div id="bestchart" style="width:100%;height:360px;">
                                <div id="Div3" style="width:570px;height:300px">
                                    <div style="margin:auto;width:250px" ><img src="Styles/loadingcircle.gif" alt="Loading Chart"/></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel">
                        <h3 class="title pane-toggler" id="H4" onclick="javascript:HandleToggles(4);"><a href="javascript:HandleToggles(4);"><span>Today's Best Routes / Travelers</span></a></h3>
                        <div class="four" style="padding-top: 0px; border-top-style: none; padding-bottom: 0px; border-bottom-style: none; overflow: hidden; height: auto;display:none;">
                            <div id="Div4" style="width:100%;height:360px;">
                                <div id="bestTChart" style="width:570px;height:300px">
                                    <div style="margin:auto;width:250px" ><img src="Styles/loadingcircle.gif" alt="Loading Chart"/></div>
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

