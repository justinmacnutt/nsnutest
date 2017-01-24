<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebApplication.UserControls.AuthWidget" Codebehind="AuthWidget.ascx.cs" %>

<asp:Panel ID="pnlLoggedIn" runat="server">
    <a href="#">Welcome, <asp:Literal ID="litUserName" runat="server"></asp:Literal></a> &nbsp;|&nbsp; <asp:LinkButton ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_onClick" />
</asp:Panel>