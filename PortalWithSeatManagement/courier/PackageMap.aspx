<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="PackageMap.aspx.vb" Inherits="courier_PackageMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
    <title>Courier Details</title>
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
                <h2>Package Map <asp:Literal ID="headerDate" runat="server"></asp:Literal> </h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m"> 
				<ul id="submenu">
	            	<li><a class="" href="Couriers.aspx">Package Mangement</a>	</li>
                    <li><a class="active" href="#">Package Map</a>	</li>           
	            </ul>
				<div class="clr"></div>
			</div>
	</div>
    <div id="element-box">
        <div class="m">
            <div style="width:20%;float:left">
                <ul class="list-unstyled" style="list-style: none;font-size: 24px;text-align: left;padding-left: 0px;">
                    <li style="margin-bottom: 5px;background: #ccc;padding: 10px">All Packages</li>
                    <li style="margin-bottom: 5px;background: #fff;padding: 10px">Couriers</li>
                    <li style="margin-bottom: 5px;background: #fff;padding: 10px">Luggages</li>
                </ul>
            </div>
            <div style="width:80%;min-height:350px;float:right" id="map">
                MAP HERE    
            </div>
            
        </div>
        
    </div>

<script src="/Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="../Scripts/clusterer.js"></script>
    <script type="text/javascript">
   
    $(function () {

        var markerClusterer = null;
        var map = null;
        var imageUrl = 'courier.png';

        function refreshMap() {
            if (markerClusterer) {
                markerClusterer.clearMarkers();
            }

            var markers = [];

            var markerImage = new google.maps.MarkerImage(imageUrl,
              new google.maps.Size(32, 32));

            var x = Math.round(random = Math.random() * 10);

            for (var i = 0; i < 50; ++i) {
                var latitude = 0;
                var longitude = 0;

                if (x <= 3)
                {
                    latitude = -1.940777;
                    longitude = 30.044880;
                }
                else if (x > 3 && x <= 6) {
                    latitude = -0.619717;
                    longitude = 30.641427;
                }
                else {
                    latitude = 0.317604;
                    longitude = 32.576769;
                }

                var latLng = new google.maps.LatLng(latitude,longitude)
                var marker = new google.maps.Marker({
                    position: latLng,
                    draggable: false,
                    icon: markerImage
                });
                markers.push(marker);
                x = Math.round(random = Math.random() * 10);
            }


            markerClusterer = new MarkerClusterer(map, markers, {
                maxZoom: 14,
                gridSize: 40
            });
        }

        function initialize() {
            map = new google.maps.Map(document.getElementById('map'), {
                zoom: 6,
                center: new google.maps.LatLng(-0.619717, 30.641427),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });

            refreshMap();
        }

        function clearClusters(e) {
            e.preventDefault();
            e.stopPropagation();
            markerClusterer.clearMarkers();
        }

        initialize();
    });

   
</script>
</asp:Content>


