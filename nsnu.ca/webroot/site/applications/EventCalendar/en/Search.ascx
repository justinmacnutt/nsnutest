<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.Modules.EventCalendar.UI.App.Search" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>

<%-- Event Calendar styles --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/mod_ec.css ~/core/styles/mod_ec.css" />
<OWWeb:ScriptBlock runat="server" ScriptType="text/javascript" DocumentSource="~/core/scripts/ow_apps.js" IncludeLocation="AfterAjax" EnableViewState="false" />
<OWWeb:ScriptBlock runat="server" ScriptType="text/javascript" DocumentSource="../ec_Application.js" IncludeLocation="Startup" EnableViewState="false"/>


<asp:UpdatePanel ID="updSearch" runat="server">
<ContentTemplate>
<div id="ec_search" class="ow_app">

<div class="ec_title">Find Events</div>

<asp:PlaceHolder ID="plcContent" Runat="server">
<div class="ec_searchfields">

    <div class="ec_sf_group1 clearfix">
        <%-- Keyword --%>
	    <div class="formlabel"><OWWeb:FieldLabel Runat="server" For="ec_txtKeyword">Keyword</OWWeb:FieldLabel></div>
	    <div class="forminput"><asp:TextBox id="ec_txtKeyword" runat="server" MaxLength="50" CssClass="ec_txt"/></div>
    </div>
    <asp:PlaceHolder runat="server" ID="plcCategories">
    <div class="ec_sf_group2 clearfix">
	    <%-- Category --%>
	    <div class="formlabel"><OWWeb:FieldLabel Runat="server" For="ec_lstCategory">Category</OWWeb:FieldLabel></div>
        <div class="forminput"><asp:DropDownList id="ec_lstCategory" runat="server" CssClass="ec_sel">
	    <asp:ListItem Value="">All</asp:ListItem>
	    </asp:DropDownList></div>
    </div>	
    </asp:PlaceHolder>
    <div class="ec_sf_group3 clearfix">
	    <%-- Start Date --%>
	    <div class="formlabel"><OWWeb:FieldLabel Runat="server" For="ec_dtStartDate">From</OWWeb:FieldLabel></div>
	    <div class="forminput"><telerik:RadDatePicker ID="ec_dtStartDate" runat="server" ShowPopupOnFocus="true" /></div>
        <asp:CompareValidator ClientId="ec_DateCompare" runat="server" ControlToValidate="ec_dtEndDate"
			ControlToCompare="ec_dtStartDate" Operator="GreaterThanEqual" Type="Date" ErrorMessage="The From Date must be earlier than the To Date." 
			Display="Dynamic" EnableClientScript="False"  CssClass="invalid"/>
    </div>
        
    <div class="ec_sf_group4 clearfix">
        <%-- End Date --%>
	    <div class="formlabel"><OWWeb:FieldLabel Runat="server" For="ec_dtEndDate">To</OWWeb:FieldLabel></div>
	    <div class="forminput"><telerik:RadDatePicker ID="ec_dtEndDate" runat="server" ShowPopupOnFocus="true" /></div>
    </div>


</div>
<div class="ec_searchbutton clearfix">
    <div class="ec_button1"><asp:Button ID="ec_btnSearch" Runat="server" CssClass="ec_submitbtn" Text="Find Events"/></div>
    <div class="ec_button2"><asp:Button ID="ec_btnReset" Runat="server" CssClass="ec_resetbtn" Text="Reset" /></div>
</div>
</asp:PlaceHolder>
</div>
</ContentTemplate>
</asp:UpdatePanel><%-- Search.ascx - 7.0.4752 --%> 
