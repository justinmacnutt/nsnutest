<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.UI.Components.Templates.Template" FormControl="~/core/controls/en/Form.ascx" %>
<%@ Register TagPrefix="OW" Namespace="ISL.OneWeb4.UI.Components.Templates" Assembly="ISL.OneWeb4.UI.Web" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWSite" Assembly="ISL.OneWeb4.UI.Web" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWSite" TagName="Menu" Src="~/core/controls/Menu.ascx" %>

<OWWeb:Stylesheet Runat="server" Media="screen,print" Mode="Link" Url="~/site/styles/print.css"/>

<div class="body_container">
    <br /><br /><br />
    <div class="printlogo"><img src="/site/images/logo_nsnu_print.png" border="0" hspace="0" vspace="0" alt="NSNU" /></div>
    <h1><OWSite:Menu runat="server" TransformSource="~/core/templates/CurrentHeader.xslt" ContentSEO="False" /></h1>
    <OW:ContentBlock Position="content1" Display="Block" CssClass="ow_block" runat="server" /><br />
    <OW:ContentBlock Position="content2" Display="Block" CssClass="ow_block" runat="server" /><br />
    <OW:ContentBlock Position="content3" Display="Block" CssClass="ow_block" runat="server" /><br />
    <OW:ContentBlock Position="feature_left1" Display="Block" CssClass="ow_block" runat="server" /><br />
    <OW:ContentBlock Position="feature_left2" Display="Block" CssClass="ow_block" runat="server" /><br />
    <OW:ContentBlock Position="feature_left3" Display="Block" CssClass="ow_block" runat="server" /><br />
    <div class="footer_container">
        <div class="printfooter"><OW:ContentBlock Position="footer_left" Display="Block" CssClass="ow_block" runat="server" /></div>
    </div>
</div>
