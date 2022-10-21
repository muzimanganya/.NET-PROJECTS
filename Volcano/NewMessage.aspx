<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="NewMessage.aspx.vb" Inherits="NewMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" Runat="Server">
<title>New Private Message</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
<div id="toolbar-box">
        <div class="m">
            <div class="toolbar-list" id="toolbar">
                <ul>
                    
                    <li class="button" id="Li1">
                        <a href="#" onclick="" class="toolbar">
                        <span class="icon-32-save"></span>Send</a>  
                    </li>
                    <li class="button" id="Li2">
                        <a href="#" onclick="" class="toolbar">
                        <span class="icon-32-cancel"></span>Cancel</a>  
                    </li>
                    <li class="divider"></li>
                    <li class="button" id="Li3">
                        <a href="#" onclick="" class="toolbar">
                        <span class="icon-32-help"></span>Help</a>  
                    </li>
                </ul>

            </div>
            <div class="pagetitle icon-48-booking">
                <h2>Private Messages</h2>
            </div>
        </div>
    </div>
    <div id="submenu-box">
			<div class="m">
				<ul id="submenu">
	            	<li><a href="#" class="active">New Private Message</a>	</li>
		            <li><a href="#">Messages</a>	</li>
                    <li><a href="#">Live Chat</a>	</li>
	            </ul>
				<div class="clr"></div>
			</div>
	</div>

</asp:Content>

