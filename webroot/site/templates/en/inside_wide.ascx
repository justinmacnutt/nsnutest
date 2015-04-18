<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.UI.Components.Templates.Template" FormControl="~/core/controls/en/Form.ascx" %>
<%@ Register TagPrefix="OW" Namespace="ISL.OneWeb4.UI.Components.Templates" Assembly="ISL.OneWeb4.UI.Web" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWSite" Assembly="ISL.OneWeb4.UI.Web" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWSite" TagName="Menu" Src="~/core/controls/Menu.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="SearchBox" Src="~/site/controls/en/SearchBox.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LogonLink" Src="~/site/controls/en/MemberLogonLink.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LogoffLink" Src="~/site/controls/en/MemberLogoffLink.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LoadJS" Src="~/site/controls/LoadJS.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LoadCss" Src="~/site/controls/LoadCss.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LoadJQ" Src="~/core/controls/IncludejQuery.ascx" %>

<OWSite:LoadJQ runat="server" IncludeUI="True" />
<OWSite:LoadCss runat="server" />
<OWSite:LoadJS runat="server" />
<%-- custom editor CSS --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/ow_custom.css" />
<div class="inside">
<OWSite:Menu runat="server" TransformSource="~/site/templates/BannerClass.xslt"/>
<div class="taskbar clearfix">
  <div class="searcharea">
    <OWSite:SearchBox runat="server" ResultsPage="~~/search.aspx"/>
  </div>
  <p><OWSite:LogonLink runat="server" /><OWSite:LogoffLink runat="server" /></p>
</div>
<header id="header_main" class="clearfix">
  <div class="inner">
    <hgroup class="col1">
      <img src="/site/images/logo_nsnu_print.png" border="0" title="Nova Scotia Nurses Union" class="printLogo" />
      <h1 id="logo"><a href="/en/home/default.aspx">Nova Scotia Nurses Union</a></h1>
    </hgroup>
    <nav class="col2" role="navigation" id="nav_main">
      <a href="#" class="btn_menu"><span>Menu</span></a>
      <OWSite:Menu runat="server" TransformSource="~/site/templates/MainMenuMultiTier.xslt"/>
    </nav>
  </div>
</header>
<div id="breadcrumbs">
  <div class="inner clearfix">
    <OWSite:Menu runat="server" TransformSource="~/site/templates/Breadcrumbs.xslt"/>
  </div>
</div>
<article id="content" role="main">
  <div class="inner clearfix">
    <h1><OWSite:Menu runat="server" TransformSource="~/core/templates/CurrentHeader.xslt" ContentSEO="False" /></h1>
    <div class="clearfix addthis_line">
        <OW:ContentBlock Position="addthis_right" Display="Block" CssClass="ow_block" runat="server" />
    </div>
    <div class="wide ">
      <div class="inner">
        <OW:ContentBlock Position="content1" Display="Block" CssClass="ow_block" runat="server" />
        <OW:ContentBlock Position="content2" Display="Block" CssClass="ow_block" runat="server" />
        <OW:ContentBlock Position="content3" Display="Block" CssClass="ow_block" runat="server" />
        <OW:DependentBlock runat="server" DependsOnPosition="bottom_content">
        <aside class="bottom">
        </OW:DependentBlock>
           <OW:ContentBlock Position="bottom_content" Display="Block" CssClass="ow_block" runat="server" />
        <OW:DependentBlock runat="server" DependsOnPosition="bottom_content">
        </aside>
        </OW:DependentBlock>
      </div>
    </div>
  </div>
</article>
<div class="searcharea mobile">
    <OWSite:SearchBox runat="server" ResultsPage="~~/search.aspx"/>
  </div>
<footer id="footer_main">
  <div class="inner clearfix">
    <div class="col1 nav">
      <OW:ContentBlock Position="footer_left" Display="Block" CssClass="ow_block" runat="server" />
    </div>
    <div class="col2">
      <OW:ContentBlock Position="footer_bottom" Display="Block" CssClass="ow_block" runat="server" />
    </div>
  </div>
  <div class="copyright">
    <div class="inner cf">
      <div class="pull_left">
        <OW:ContentBlock Position="footer_copyright" Display="Block" CssClass="ow_block" runat="server" />
      </div>
      <div class="pull_right">
        <div class="addthis_horizontal_follow_toolbox"></div>
      </div>
    </div>
  </div>
</footer>
</div>