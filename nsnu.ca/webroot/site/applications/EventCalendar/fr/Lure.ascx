<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.Modules.EventCalendar.UI.App.Lure" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Import Namespace="ISL.OneWeb4.Modules.EventCalendar.Entities" %>

<%-- Event Calendar styles --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/mod_ec.css ~/core/styles/mod_ec.css" />

<div id="ec_lure">
<!--<h2 class="ec_intro"><asp:Literal ID="litIntro" Runat="server"/></h2> -->



<asp:PlaceHolder ID="plcEvents" Runat="server">
<div class="ec_results">
	<asp:Repeater ID="rptEvents" Runat="server">
	    <ItemTemplate>
			<div class="ec_event">
			    <div class="ec_lure_eventDate"><%# MyBase.GetEventDate(Container.DataItem) %></div>
				<div class="ec_lure_detailsAddress"><a href="<%# MyBase.GetDetailsAddress(Container.DataItem, Container.ItemIndex) %>"><%# Container.DataItem.Name %></a></div>
				<asp:PlaceHolder runat="server" Visible="<%# Not String.IsNullOrEmpty(Container.DataItem.Location) %>"><div class="ec_lure_location"><%# Container.DataItem.Location %></div></asp:PlaceHolder>
			    <asp:PlaceHolder runat="server" Visible="<%# Not String.IsNullOrEmpty(Container.DataItem.CategoryName) AndAlso MyBase.ShowMultipleCategories() %>"><div class="ec_lure_category"><%#Container.DataItem.CategoryName%></div></asp:PlaceHolder>
			</div>
		</ItemTemplate>
		<FooterTemplate><div class="ec_lure_morelink"><a href="<%# MyBase.GetMoreAddress() %>"><%=EventListLink()%> <% If Not String.IsNullOrEmpty(EventListLink()) Then Response.Write("&raquo;")%></a></div></FooterTemplate>
	</asp:Repeater>
</div>
</asp:PlaceHolder>


<asp:PlaceHolder runat="server" id="plcNoEvents">
<div class="ec_listnoevents">Nous sommes d&eacute;sol&eacute;s, mais il n'y a actuellement aucun &eacute;v&eacute;nement &agrave; venir affich&eacute; pour cette p&eacute;riode de temps. Nous affichons souvent de nouveaux &eacute;v&eacute;nements, donc nous vous encourageons &agrave; consulter r&eacute;guli&egrave;rement notre calendrier.</div>
</asp:PlaceHolder>
</div>

<%-- Lure.ascx - 7.0.4752 --%> 
