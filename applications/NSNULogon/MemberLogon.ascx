<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb.ClientApplications.NSNU.NSNULogon.UserControls.MemberLogon" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<OWWeb:ScriptBlock runat="server" ID="scriptMbr" ScriptType="text/javascript" DocumentSource="~/core/scripts/ow_membership.js"/>
<div id="ow_login" class="inner">
	<%-- login client-prefix determinator --%><input runat="server" type="hidden" id="ow_xLogin"/>
	<%-- Login --%>
     <h1 class="loginToggle">Login to MyNSNU <span class="icon"></span></h1>
        <div class="form">
	        <asp:PlaceHolder ID="plcLogin" Runat="server">
                 <h2>Login to MyNSNU</h2>
		            <asp:PlaceHolder ID="plcLoginError" Runat="server"><p id="ow_errorHeading">
			            <asp:Literal ID="litLoginError" Runat="server" Text="Invalid logon name or password"/>
			            <asp:ValidationSummary DisplayMode="SingleParagraph" ShowSummary="True"/>
		            </p></asp:PlaceHolder>
		            <OWWeb:FieldLabel Text="User name" For="ow_txtUserName" runat="server"/><br />
                    <asp:TextBox id="ow_txtUserName" Runat="server" CssClass="ow_logonbox"/><br />
		            <OWWeb:FieldLabel Text="Password" For="ow_txtPassword" runat="server"/><br />
                    <asp:TextBox id="ow_txtPassword" Runat="server" CssClass="ow_pwdbox" TextMode="Password"/><br />
		            <asp:RequiredFieldValidator ClientId="ow_rfvUserName" ControlToValidate="ow_txtUserName" Runat="server" CssClass="ow_error" 
			            EnableViewState="False" EnableClientScript="False" ErrorMessage="Please enter your user name" Display="None"/>
		            <asp:RequiredFieldValidator ClientId="ow_rfvPassword" ControlToValidate="ow_txtPassword" Runat="server" CssClass="ow_error" 
			            EnableViewState="False" EnableClientScript="False" ErrorMessage="Please enter your password"  Display="None"/>
	        </asp:PlaceHolder>

	        <%-- Forgot Password --%>
	        <asp:PlaceHolder ID="plcForgotPwd" Runat="server" Visible="False">
		        <h2>Forgotten password</h2>
		            <asp:PlaceHolder ID="plcForgotPwdError" Runat="server"><p id="ow_error"><asp:Literal ID="litForgotPwdError" Runat="server" Text="Invalid logon name or email"/></p></asp:PlaceHolder>
		            <OWWeb:FieldLabel Text="User name" For="ow_txtUsername" runat="server" /><br />
                    <asp:TextBox id="ow_txtUsername2" Runat="server" CssClass="ow_logonbox" /><br />
		            <OWWeb:FieldLabel Text="Email" For="ow_txtEmail" runat="server"/><br />
                    <asp:TextBox id="ow_txtEmail" Runat="server" CssClass="ow_emailbox"/><br />
		            <asp:RequiredFieldValidator ClientId="ow_rfvUserName2" ControlToValidate="ow_txtUserName2" Runat="server" CssClass="ow_error" 
			            EnableViewState="False" EnableClientScript="False" ErrorMessage="Please enter your user name" />
		            <asp:RequiredFieldValidator ClientId="ow_rfvEmail" ControlToValidate="ow_txtEmail" Runat="server" CssClass="ow_error" 
			            EnableViewState="False" EnableClientScript="False" ErrorMessage="Please enter your email address" />
	        </asp:PlaceHolder>

	        <%-- Forgot Logon --%>
	        <asp:PlaceHolder ID="plcForgotName" Runat="server" Visible="False">
		        <h2>Forgotten user name or email</h2>
		            <asp:PlaceHolder ID="plcForgotNameError" Runat="server"><p id="ow_error"><asp:Literal ID="litForgotNameError" Runat="server" Text="Invalid logon name or email"/></p></asp:PlaceHolder>
		            <OWWeb:FieldLabel Text="User name/email" For="ow_txtUserName3" runat="server"/><br />
                    <asp:TextBox id="ow_txtUserName3" Runat="server" CssClass="ow_logonbox"/><br />
		            <asp:RequiredFieldValidator ClientId="ow_rfvUserName3" ControlToValidate="ow_txtUserName3" Runat="server" 
			            CssClass="ow_error" EnableClientScript="False" EnableViewState="False" ErrorMessage="Please enter your user name or email address"/>
	        </asp:PlaceHolder>

	        <%-- Message --%>
	        <asp:PlaceHolder ID="plcMessage" Runat="server" Visible="False">
		        <h2>Message</h2>
		        <p class="ow_loginMessage"><asp:Literal ID="litMessage" Runat="server"/></p>
	        </asp:PlaceHolder>

	        <asp:Button id="ow_btnSubmit" CssClass="ow_btnSubmit btn_red" Runat="server" Text="Submit" CausesValidation="False"/>
	        <asp:ImageButton id="ow_imgSubmit" CssClass="ow_imgSubmit hidden" Runat="server" ImageUrl="~/core/images/blank.gif" CausesValidation="False"/>
	        <asp:Button id="ow_btnLogon" CssClass="ow_btnLogon btn_red" Runat="server" Text="Login" CausesValidation="False"/>
	        <asp:ImageButton id="ow_imgLogon" CssClass="ow_imgLogon hidden" Runat="server" ImageUrl="~/core/images/blank.gif" CausesValidation="False"/>
	        <asp:Button id="ow_btnBack" CssClass="ow_btnBack btn_red" Runat="server" Text="Return" CausesValidation="False"/>
	        <asp:ImageButton id="ow_imgBack" CssClass="ow_imgBack hidden" Runat="server" ImageUrl="~/core/images/blank.gif" CausesValidation="False"/>
	
	        <asp:PlaceHolder ID="plcForgotPassword" Runat="server"><p id="ow_loginForgot"><asp:LinkButton id="lnkForgotPassword" runat="server" CausesValidation="False">Forgot your password?</asp:LinkButton></p></asp:PlaceHolder>
	        <asp:PlaceHolder ID="plcForgotLogon" Runat="server"><p id="ow_loginForgot"><asp:LinkButton id="lnkForgotLogon" runat="server" CausesValidation="False">Forgot your user name or email?</asp:LinkButton></p></asp:PlaceHolder>
	        <asp:PlaceHolder ID="plcRegistration" Runat="server"><p id="ow_loginRegister"><asp:Hyperlink id="lnkRegister" runat="server">Need to register?</asp:Hyperlink></p></asp:PlaceHolder>
    </div>
</div>
<%-- MemberLogon.ascx - 7.0.4752 --%> 
