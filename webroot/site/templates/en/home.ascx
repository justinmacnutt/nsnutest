<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.UI.Components.Templates.Template" FormControl="~/core/controls/en/Form.ascx" %>
<%@ Register TagPrefix="OW" Namespace="ISL.OneWeb4.UI.Components.Templates" Assembly="ISL.OneWeb4.UI.Web" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWSite" Assembly="ISL.OneWeb4.UI.Web" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWSite" TagName="Menu" Src="~/core/controls/Menu.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="SearchBox" Src="~/site/controls/en/SearchBox.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LogonLink" Src="~/site/controls/en/MemberLogonLink.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LogoffLink" Src="~/site/controls/en/MemberLogoffLink.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="MemberStatus" Src="~/site/controls/en/MemberStatus.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LoadJS" Src="~/site/controls/LoadJS.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LoadCss" Src="~/site/controls/LoadCss.ascx" %>
<%@ Register TagPrefix="OWSite" TagName="LoadJQ" Src="~/core/controls/IncludejQuery.ascx" %>

<OWSite:LoadJQ runat="server" IncludeUI="True" />
<OWSite:LoadCss runat="server" />
<OWSite:LoadJS runat="server" />

<%-- custom editor CSS --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/ow_custom.css"/>
<div class="home">
<div class="taskbar clearfix">
  <div class="searcharea">
    <OWSite:SearchBox runat="server" ResultsPage="~~/search.aspx"/>
  </div>
  <p><OWSite:MemberStatus runat="server" /><OWSite:LogonLink runat="server" /><OWSite:LogoffLink runat="server" /></p>
</div>
<header id="header_main" class="clearfix">
  <div class="inner">
    <hgroup class="col1">
      <h1 id="logo"><a href="/en/home/default.aspx">Nova Scotia Nurses Union</a></h1>
    </hgroup>
    <nav class="col2" role="navigation" id="nav_main">
      <a href="#" class="btn_menu"><span>Menu</span></a>
      <OWSite:Menu runat="server" TransformSource="~/site/templates/MainMenuMultiTier.xslt"/>
    </nav>
    <div class="slider">
    <div class="slides">
        <div class="slide1 slide">
            <OW:ContentBlock Position="home_slide1" Display="Block" CssClass="ow_block" runat="server" />
        </div>
        <div class="slide2 slide">
            <OW:ContentBlock Position="home_slide2" Display="Block" CssClass="ow_block" runat="server" />
        </div>
        <div class="slide3 slide">
            <OW:ContentBlock Position="home_slide3" Display="Block" CssClass="ow_block" runat="server" />
        </div>
        <div class="slide4 slide">
            <OW:ContentBlock Position="home_slide4" Display="Block" CssClass="ow_block" runat="server" />
        </div>
    </div>
    <div class="slide_nav">
        <ul>
            <li id="nav_slide1">
                <OW:ContentBlock Position="home_slidenav1" Display="Block" CssClass="ow_block" runat="server" />
                <span class="icon"></span>
            </li>
            <li id="nav_slide2">
                <OW:ContentBlock Position="home_slidenav2" Display="Block" CssClass="ow_block" runat="server" />
                <span class="icon"></span>
            </li>
            <li id="nav_slide3">
                <OW:ContentBlock Position="home_slidenav3" Display="Block" CssClass="ow_block" runat="server" />
                <span class="icon"></span>
            </li>
            <li id="nav_slide4">
                <OW:ContentBlock Position="home_slidenav4" Display="Block" CssClass="ow_block" runat="server" />
                <span class="icon"></span>
            </li>
        </ul>
    </div>
</div>
  </div>
</header>
<div class="header_image">

</div>
<article id="content" role="main">
  <div class="inner clearfix">
    <aside class="col2 login">
      <OW:DependentBlock runat="server" DependsOnPosition="home_login" DependsOnGroup="^*">
        <OW:ContentBlock Position="home_login" Display="Block" CssClass="ow_block" runat="server" />
      </OW:DependentBlock>
      <OW:DependentBlock runat="server" DependsOnPosition="member_info" DependsOnGroup="*">
        <div class="inner"><OW:ContentBlock Position="member_info" Display="Block" CssClass="ow_block" runat="server" /></div>
      </OW:DependentBlock>    
    </aside>
    <div class="col1">
      <div class="inner">
        <OW:ContentBlock Position="home_content1" Display="Block" CssClass="ow_block" runat="server" />
      </div>
    </div>
    <aside class="col3">
      <div class="inner">
        <OW:ContentBlock Position="home_right_feature1" Display="Block" CssClass="ow_block" runat="server" />
        <OW:ContentBlock Position="home_right_feature2" Display="Block" CssClass="ow_block" runat="server" />
        <OW:ContentBlock Position="home_right_feature3" Display="Block" CssClass="ow_block" runat="server" />
      </div>
    </aside>
  </div>
</article>
<div class="searcharea mobile">
    <OWSite:SearchBox runat="server" ResultsPage="~~/search.aspx"/>
  </div>
<footer id="footer_main">
  <div class="inner">
    <div class="top clearfix">
      <div class="col1 nav">
        <OW:ContentBlock Position="footer_left" Display="Block" CssClass="ow_block" runat="server" />
      </div>
      <div class="col2 twitter">
        <OW:ContentBlock Position="footer_home_Right" Display="Block" CssClass="ow_block" runat="server" />
      </div>
    </div>
    <div class="bottom">
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