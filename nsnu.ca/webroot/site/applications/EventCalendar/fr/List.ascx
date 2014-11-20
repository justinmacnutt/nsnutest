<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.Modules.EventCalendar.UI.App.List" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="PLPager" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="ISL.OneWeb4.Modules.EventCalendar.Entities" %>

<%-- Event Calendar styles --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/mod_ec.css ~/core/styles/mod_ec.css" />

<asp:UpdatePanel runat="server" ID="upEventList" UpdateMode="Conditional">
<ContentTemplate>
<div id="ec_list">
<asp:PlaceHolder ID="plcEvents" Runat="server">
<div id="ec_results">
    
	<asp:Repeater ID="rptEvents" Runat="server">
	    <ItemTemplate>
			<div class="ec_event">
                <div class="ec_list_eventDate"><%#MyBase.GetEventDate(Container.DataItem)%></div>
				<div class="ec_list_detailsAddress"><a href="<%# Me.GetDetailsAddress(Container.DataItem, Container.ItemIndex) %>"><%# Container.DataItem.Name %></a></div>
				<div class="ec_list_location"><%#If(Container.DataItem.Location.Length > 0, Container.DataItem.Location, "")%></div>
				<asp:PlaceHolder runat="server" Visible="<%# Not String.IsNullOrEmpty(Container.DataItem.CategoryName) AndAlso MyBase.ShowMultipleCategories() %>"><div class="ec_list_categoryName"><%# Container.DataItem.CategoryName%></div></asp:PlaceHolder>
				<div class="ec_list_summary"><%#If(Container.DataItem.Summary.Length > 0, Container.DataItem.Summary, "")%></div>
			</div>
			<div class="ec_hr"></div>
		</ItemTemplate>
	</asp:Repeater>
</div>
</asp:PlaceHolder>

<asp:PlaceHolder Runat="server" ID="plcPaging">
    <PLPager:Pager id="plPager" runat="server" 
	    PreviousText="&laquo; Previous" NextText="Next &raquo;"
	    DisplayFirstPageControl="False" DisplayLastPageControl="False" 
	    DisplayPageSizeControl="False" DisplayPageGroupNumbers="False"/>
</asp:PlaceHolder>


<asp:PlaceHolder runat="server" id="plcNoEvents">
<div class="ec_listnoevents">Nous sommes d&eacute;sol&eacute;s, mais il n'y a actuellement aucun &eacute;v&eacute;nement &agrave; venir affich&eacute; pour cette p&eacute;riode de temps. Nous affichons souvent de nouveaux &eacute;v&eacute;nements, donc nous vous encourageons &agrave; consulter r&eacute;guli&egrave;rement notre calendrier.</div>
</asp:PlaceHolder>
</div>
</ContentTemplate>
</asp:UpdatePanel>

<%-- List.ascx - 7.0.4752 --%> 
