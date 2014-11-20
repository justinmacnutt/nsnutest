<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ListVotes.ascx.vb" Inherits="ISL.OneWeb.ClientApplications.NSNU.VotingApplication.UI.Admin.ListVotes" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWAdm" TagName="PagerList" Src="~/ow/controls/PagerList.ascx" %> 
<%@ Register TagPrefix="OWAdm" TagName="PagerSize" Src="~/ow/controls/PagerSize.ascx" %> 

<OWWeb:ScriptBlock runat="server" ScriptType="Text/javascript" DocumentSource="~/ow/scripts5/ow_listing.js" EnableViewState="false"/> 
<OWWeb:ScriptBlock runat="server" ScriptType="text/javascript" DocumentSource="va_AdminVote.js" IncludeLocation="Startup" EnableViewState="false" />

<input type="hidden" runat="server" id="ow_EntityID" name="ow_EntityID" value="">	

<p class="isl_intro"><asp:Literal Text="introVotes" Runat="server" /></p>

<div class="isl_pager">
	<OWAdm:PagerList id="ow_pagerTop" runat="server"/> 
</div>

<div id="isl_data_bar">
	<div id="isl_data_act">
	    <asp:Button Runat="server" ID="ow_btnAdd" CssClass="isl_btn" CausesValidation="False"/>
	</div>		
	<div id="isl_data_tab">
		<OWAdm:PagerSize id="ow_pagerSize" runat="server" />
	</div>
</div>

<asp:PlaceHolder Runat="server" ID="ow_plcNoRecords">
	<p class="isl_no_recs"><asp:Literal Runat="server" Text="noVotes" /></p>
</asp:PlaceHolder>


<table class="isl_data_tbl" cellspacing="0" cellpadding="0">
	<asp:PlaceHolder Runat="server" ID="ow_plcHeaders">
	<thead>
		<tr>
			<th><asp:LinkButton OnCommand="SortColumns" CommandName="Title" CommandArgument="0" ToolTip="sortAsc" runat="Server" ID="ow_sortHeaderTitle"><asp:Literal runat="server" Text="title" /></asp:LinkButton></th>
			<th><asp:LinkButton OnCommand="SortColumns" CommandName="Question" CommandArgument="0" ToolTip="sortAsc" runat="Server" ID="ow_sortHeaderQuestion"><asp:Literal runat="server" Text="question" /></asp:LinkButton></th>
			<th><asp:LinkButton OnCommand="SortColumns" CommandName="DisplayDate" CommandArgument="0" ToolTip="sortAsc" runat="Server" ID="ow_sortHeaderDisplayDate"><asp:Literal runat="server" Text="displayDate" /></asp:LinkButton></th>
			<th><asp:LinkButton OnCommand="SortColumns" CommandName="ExpiryDate" CommandArgument="0" ToolTip="sortAsc" runat="Server" ID="ow_sortHeaderExpiryDate"><asp:Literal runat="server" Text="expiryDate"/></asp:LinkButton></th>
			<th><asp:LinkButton OnCommand="SortColumns" CommandName="VoteStatus" CommandArgument="0" ToolTip="sortAsc" runat="Server" ID="ow_sortHeaderStatus"><asp:Literal runat="server" Text="status" /></asp:LinkButton></th>
			<th><asp:LinkButton OnCommand="SortColumns" CommandName="Answer1Count" CommandArgument="0" ToolTip="sortAsc" runat="Server" ID="ow_sortHeaderAnswer1"><asp:Literal runat="server" Text="yes" /></asp:LinkButton></th>
			<th><asp:LinkButton OnCommand="SortColumns" CommandName="Answer2Count" CommandArgument="0" ToolTip="sortAsc" runat="Server" ID="ow_sortHeaderAnswer2"><asp:Literal runat="server" Text="no" /></asp:LinkButton></th>

		</tr>
	</thead>
	</asp:PlaceHolder>
	<tbody>
		<asp:Repeater id="ow_rptVotes" runat="server">
			<ItemTemplate>
				<tr id="row_<%#Container.DataItem.VoteId %>">
					<td><%# Container.DataItem.Title%></td>
					<td><%# Container.DataItem.Question%></td>
					<td><%# CDate(Container.DataItem.DisplayDate).ToString("dd-MMM-yyyy hh:mm tt")%></td>
					<td><%# CDate(Container.DataItem.ExpiryDate).ToString("dd-MMM-yyyy hh:mm tt")%></td>
					<td><%# MyBase.GetStatus(Container.DataItem)%></td>
                    <td><%# Container.DataItem.Answer1Count%></td>
					<td><%# Container.DataItem.Answer2Count%></td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr id="row_<%#Container.DataItem.VoteId %>" class="isl_data_alt">
					<td><%# Container.DataItem.Title%></td>
					<td><%# Container.DataItem.Question%></td>
					<td><%# CDate(Container.DataItem.DisplayDate).ToString("dd-MMM-yyyy hh:mm tt")%></td>
					<td><%# CDate(Container.DataItem.ExpiryDate).ToString("dd-MMM-yyyy hh:mm tt")%></td>
					<td><%# MyBase.GetStatus(Container.DataItem)%></td>
                    <td><%# Container.DataItem.Answer1Count%></td>
					<td><%# Container.DataItem.Answer2Count%></td>
				</tr>
			</AlternatingItemTemplate>
		</asp:Repeater>
	</tbody>
</table>

<div class="isl_pager">
	<OWAdm:PagerList id="ow_pagerBottom" runat="server"/> 			
</div>

<div id="isl_cmenu" visible="0">
<ul>		
	<li><asp:LinkButton Runat="server" ID="ow_btnManage" CausesValidation="False"/></li>
</ul>
</div>
