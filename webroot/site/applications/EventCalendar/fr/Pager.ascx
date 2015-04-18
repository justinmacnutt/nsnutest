<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.UI.WebControls.Pager" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>

<%-- Event Calendar styles --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/mod_ec.css ~/core/styles/mod_ec.css" />

<div ID="ow_adminPagerOptions">
	<asp:PlaceHolder Runat="server" ID="ow_plcRCount">
		<asp:Literal runat="server" ID="ow_fldRCount" EnableViewState="False" />
	</asp:PlaceHolder>
	<asp:Placeholder Runat="server" ID="ow_plcPerPage">
		<asp:DropDownList runat="server" ID="ow_selPageSize" AutoPostBack="True">
			<%--<asp:ListItem Value="2"/>
			<asp:ListItem Value="5"/>--%>
			<asp:ListItem Value="10" Selected="True"/>
			<asp:ListItem Value="25"/>
			<asp:ListItem Value="50"/>
			<asp:ListItem Value="100"/>
		</asp:DropDownList>
		<asp:Literal runat="server" id="ow_litPerPage" />
	</asp:Placeholder>		
</div>

<div ID="ow_adminPagerList">
	<asp:Placeholder Runat="server" ID="ow_plcPaginate">
		<asp:Literal runat="server" ID="ow_litPage" Text="litPage" />
		
		<asp:PlaceHolder Runat="server" ID="ow_plcFirstLink">
			<asp:Literal runat="server" ID="ow_litFirst" Text="litFirst" />
			<asp:LinkButton runat="server" ID="ow_lnkFirst" OnCommand="ChangePageIndex" CommandName="First" Text="litFirst" />
			|
		</asp:PlaceHolder>
		<asp:Literal runat="server" ID="ow_litPrev" Text="litPrev" />
		<asp:LinkButton runat="server" ID="ow_lnkPrev" OnCommand="ChangePageIndex" CommandName="Prev" Text="litPrev" />
		|
		<asp:LinkButton runat="server" ID="ow_lnkPrevGrp" OnCommand="ChangePageIndex" CommandName="PrevGroup" Text="..." />
		
		<asp:Repeater runat="server" ID="ow_rptPaginate">
			<ItemTemplate>
				<asp:LinkButton runat="server" ID="ow_lnkPage" ClientIDMode="AutoID"
								OnCommand="ChangePageIndex" CommandName="Page" CommandArgument=<%# CInt(Container.DataItem) %>
								Text=<%# CStr(Container.DataItem) %> 
								Visible=<%# CBool(CInt(Container.DataItem) <> Me.CurrentPage) %> />
				<asp:Literal runat="server" ID="ow_lblPage" 
								Text=<%# IIf(Me.RecordCount > 0, "<strong>" + CStr(Container.DataItem) + "</strong>", CStr(Container.DataItem)) %> Visible=<%# CBool(CInt(Container.DataItem) = Me.CurrentPage) %> />
			</ItemTemplate>
		</asp:Repeater>
		
		<asp:LinkButton runat="server" ID="ow_lnkNextGrp" OnCommand="ChangePageIndex" CommandName="NextGroup" Text="..." />
		|
		<asp:Literal runat="server" ID="ow_litNext" Text="litNext" />
		<asp:LinkButton runat="server" ID="ow_lnkNext" OnCommand="ChangePageIndex" CommandName="Next" Text="litNext" />
		<asp:PlaceHolder Runat="server" ID="ow_plcLastLink">
			|
			<asp:Literal runat="server" ID="ow_litLast" Text="litLast" />
			<asp:LinkButton runat="server" ID="ow_lnkLast" OnCommand="ChangePageIndex" CommandName="Last" Text="litLast" />
		</asp:PlaceHolder>
	</asp:Placeholder>
</div><%-- Pager.ascx - 7.0.4752 --%> 
