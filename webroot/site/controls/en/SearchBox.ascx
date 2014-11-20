<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.UI.UserControls.Search" %>
<%@ Register TagPrefix="OW" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OW" TagName="IncludejQuery" Src="~/core/controls/IncludejQuery.ascx" %>

<asp:PlaceHolder ID="plcAutoComplete" runat="server">
	<OW:IncludejQuery runat="server" IncludeUI="true" />
</asp:PlaceHolder>
<asp:TextBox id="ow_txtSearch" Runat="server" CssClass="ow_sbox" MaxLength="200" Width="100"/>
<asp:Button id="ow_btnSearch" CssClass="ow_submit" Runat="server" Text="Go" CausesValidation="False"/>
<asp:ImageButton id="ow_imgSearch" CssClass="hidden" Runat="server" ImageUrl="~/core/images/blank.gif" CausesValidation="False"/>
<%-- SearchBox.ascx - 7.0.4752 --%> 
