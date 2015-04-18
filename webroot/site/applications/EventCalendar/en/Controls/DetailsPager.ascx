<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.Modules.EventCalendar.UI.App.DetailsPager" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<div>
<table width="100%" cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td style="text-align:left;"><a href="<%# Values.PrevLink %>" title="<%# Values.PrevTitle %>" visible="<%# Not String.IsNullOrEmpty(Values.PrevLink) %>" id="lnkPrev" runat="server">&laquo; Previous</a><asp:Literal runat="server" Text="&laquo; Previous" Visible="<%# String.IsNullOrEmpty(Values.PrevLink) %>" /></td>
		<%--<td style="text-align:center;"><%# Values.EventName%> | <a href="<%# Values.ResultsLink %>" id="lnkResults" runat="server">Back to Results</a></td>--%>
		<td style="text-align:right;"><a href="<%# Values.NextLink %>" title="<%# Values.NextTitle %>" visible="<%# Not String.IsNullOrEmpty(Values.NextLink) %>" id="lnkNext" runat="server">Next &raquo;</a><asp:Literal runat="server" Text="Next &raquo;" Visible="<%# String.IsNullOrEmpty(Values.NextLink) %>" /></td>
	</tr>
</table>
</div>
<%-- DetailsPager.ascx - 7.0.4752 --%> 
