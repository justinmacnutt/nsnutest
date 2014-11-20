<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.Modules.EventCalendar.UI.App.Results" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWWeb" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="ISL.OneWeb4.Modules.EventCalendar.Entities" %>

<%-- Event Calendar styles --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/mod_ec.css ~/core/styles/mod_ec.css" />

<div id="ec_list">

<%-- Top pagination --%>
<asp:PlaceHolder Runat="server" ID="plcPagingTop">
<div class="searchresults_stats">
	<OWWeb:Pager id="pagingTop" runat="server" PreviousText="prev" NextText="next"
	    DisplayPageSizeControl="False" DisplayLineCountControl="False" />
</div>
</asp:PlaceHolder>

<%-- Results --%>
<asp:PlaceHolder ID="plcEvents" Runat="server">
    <div id="ec_results">
	<asp:Repeater ID="rptEvents" Runat="server">
	    <ItemTemplate>
			<div class="ec_event">
				<div class="ec_list_eventDate"><%#Me.GetEventDate(Container.DataItem)%></div>
				<div class="ec_list_detailsAddress"><a href="<%# Me.GetDetailsAddress(Container.DataItem) %>"><%# Container.DataItem.Name %></a></div>
				<div class="ec_list_location"><%#If(Not String.IsNullOrEmpty(Container.DataItem.Location), Container.DataItem.Location, "")%></div>
				<div class="ec_list_categoryName"><%#If(Not String.IsNullOrEmpty(Container.DataItem.CategoryName), Container.DataItem.CategoryName, "")%></div>
				<div class="ec_list_summary"><%#If(Not String.IsNullOrEmpty(Container.DataItem.Summary), Container.DataItem.Summary, "")%></div>
			</div>
			<div class="ec_hr"></div>
		</ItemTemplate>
	</asp:Repeater>
	</div>
</asp:PlaceHolder>

<%-- No records --%>
<asp:PlaceHolder Runat="server" ID="plcNoEvents">
<div class="ec_resultsnoevents">No events could be found using the current search criteria. Please try again.</div>
</asp:PlaceHolder>

<%-- Bottom pagination --%>
<asp:PlaceHolder Runat="server" ID="plcPagingBottom">
<div class="searchresults_stats">
	<OWWeb:Pager id="pagingBottom" runat="server" PreviousText="prev" NextText="next"
	    DisplayPageSizeControl="False" DisplayLineCountControl="False" />
</div>
</asp:PlaceHolder>

</div><%-- Results.ascx - 7.0.4752 --%> 
