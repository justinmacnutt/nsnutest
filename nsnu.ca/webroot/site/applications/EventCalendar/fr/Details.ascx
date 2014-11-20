<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.Modules.EventCalendar.UI.App.Details" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="EC" TagName="Pager" Src="Controls/DetailsPager.ascx" %>

<%-- EventCalendar styles --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/mod_ec.css ~/core/styles/mod_ec.css" />

<asp:PlaceHolder ID="plcRecord" Runat="server" Visible="False">
<%-- top pager --%>
<EC:Pager runat="server" ID="ucPager1" />
<div itemscope itemtype="http://schema.org/Event">

<%--Mapping of OW Event Calendar fields to Schema.org fields
OW Field	    Schema.org field
Summary	        Description
Image	        Image - thisis a url to an image, not the image itself
Event name	    Name
Event website	URL
End date	    End Date
Location	    Location
Start date	    Start Date
--%>

<div id="ec_details">
	<div class="ec_headers clearfix">
		<asp:PlaceHolder ID="plcRegister" runat="server"><div class="ec_registerlink"><a id="lnkRegister" runat="server">S'inscrire</a></div></asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="plciCalLink">
            <div class="ec_iCallink"><asp:Hyperlink runat="server" ID="lnkiCal" Text="Export to iCalendar"></asp:Hyperlink></div>
        </asp:PlaceHolder>
		<div class="ec_details_info">
            <asp:PlaceHolder id="plcMetaPlaceHolder" runat="server" />
		    <div class="ec_details_date"><asp:Literal id="litStartDate" runat="server" /><asp:Literal id="litEndDate" runat="server"> - </asp:Literal><asp:Literal ID="litTimes" runat="server">, </asp:Literal></div>
		    <meta itemprop="name" content="<asp:Literal id="litName" runat="server" />" />
		    <div itemprop="location" itemscope itemtype="http://schema.org/Place"><div class="ec_details_location"><asp:Literal id="litLocation" runat="server" /></div></div>
		    <asp:PlaceHolder ID="plcCategory" runat="server"><div class="ec_details_category"><asp:Literal id="litCategory" runat="server" /></div></asp:PlaceHolder>
		    <asp:PlaceHolder ID="plcWebsite" runat="server"><div class="ec_details_website"><a id="lnkWebsite" runat="server" target="_blank" itemprop="url">Consulter le site Web de l'&eacute;v&eacute;nement</a></div></asp:PlaceHolder>
	        <asp:PlaceHolder id="plcContact" runat="server"><div class="ec_details_contactName">Contact: <asp:Literal id="litContactName" runat="server"/></div></asp:PlaceHolder>
	        <asp:PlaceHolder id="plcPhone" runat="server"><div class="ec_details_contactPhone">T&eacute;l&eacute;phone: <asp:Literal id="litContactPhone" runat="server"/></div></asp:PlaceHolder>
	        <asp:PlaceHolder id="plcEmail" runat="server"><div class="ec_details_contactEmail">Courriel: <a id="lnkEmail" runat="server" class="ow_mailto" /><asp:Literal id="litContactEmail" runat="server"/></div></asp:PlaceHolder>
	    </div>
	    <div class="ec_details_logo"><asp:HyperLink ID="lnkLogo" runat="server" Target="_blank" itemprop="image"><asp:Image runat="server" id="imgLogo" /></asp:HyperLink></div>
	</div>
	<div class="ec_body">
	    <p><span itemprop="description"><asp:Literal id="litDescription" runat="server"/></span></p>
    </div>
</div>
</div>
<%-- bottom pager --%>
<EC:Pager runat="server" ID="ucPager2" />
</asp:PlaceHolder>

<asp:PlaceHolder ID="plcNotFound" Runat="server" Visible="True">
	<div class="ec_details_notFound">Nous sommes d&eacute;sol&eacute;s, mais cet &eacute;v&eacute;nement est introuvable. Il se peut que cet &eacute;v&eacute;nement n'existe plus, ou que le URL qu'on vous a donn&eacute; est inexact. Si vous avez suivi le lien d'un message &eacute;lectronique, il se peut qu'il ait &eacute;t&eacute; tronqu&eacute; en raison du renvoi &agrave; la ligne. Veuillez vous assurer que le URL est complet.</div>
</asp:PlaceHolder>


<%-- Details.ascx - 7.0.4752 --%> 
