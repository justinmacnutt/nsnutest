<%@ Control Language="VB" AutoEventWireup="false" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>

<meta name="HandheldFriendly" content="True" runat="server"/>
<meta name="MobileOptimized" content="320" runat="server"/>
<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0, user-scalable=no" runat="server"/>
<meta name="msapplication-TileImage" content="apple-touch-icon-144x144-precomposed.png" runat="server"/>
<meta name="msapplication-TileColor" content="#fff" runat="server"/>
<meta http-equiv="cleartype" content="on" runat="server"/>

<OWWeb:Stylesheet Runat="server" Media="all" Mode="Link" Url="~/site/styles/common.css"/>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/ie.css" Condition="(lt IE 9) & !(IEMobile)"/>
<OWWeb:Stylesheet Runat="server" Media="print" Mode="Link" Url="~/site/styles/ie-print.css" Condition="(lt IE 9) & !(IEMobile)"/>
<OWWeb:Stylesheet Runat="server" Media="all" Mode="Link" Url="~/site/styles/core_controls.css"/>
<OWWeb:Stylesheet Runat="server" Media="all" Mode="Link" Url="~/site/styles/ow.css"/>

<OWWeb:ScriptBlock runat="server" ID="modernizr" ScriptType="text/javascript" DocumentSource="~/site/scripts/libs/modernizr-2.5.3.min.js" CDNSource="https?://cdn.jsdelivr.net/modernizr/2.5.3/modernizr{Config:.min,}.js" IncludeLocation="Head" EnableViewState="false" />

<%-- Icons --%>
<link rel="icon" type="image/png" href="/apple-touch-icon.png" runat="server" />
<link rel="icon" type="image/png" href="/apple-touch-icon-precomposed.png" runat="server" />
<link rel="icon" type="image/png" href="/apple-touch-icon-57x57-precomposed.png" runat="server" />
<link rel="icon" type="image/png" href="/apple-touch-icon-72x72-precomposed.png" runat="server" />
<link rel="icon" type="image/png" href="/apple-touch-icon-114x114-precomposed.png" runat="server" />
		 