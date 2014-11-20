<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.UI.UserControls.MemberProfile" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<OWWeb:ScriptBlock runat="server" ScriptType="text/javascript" DocumentSource="~/core/scripts/ow_membership.js"/>
<div id="ow_profile">
	<%-- profile client-prefix determinator --%><input runat="server" type="hidden" id="ow_xProfile"/>
	<h1>Member Profile</h1>
	<asp:ValidationSummary runat="server" EnableClientScript="False"
		ShowMessageBox="False" DisplayMode="BulletList" HeaderText="There were problems with the following field(s):"
		ForeColor="" CssClass="ow_errorHeading"/>
	<asp:PlaceHolder ID="plcPwdMsg" Runat="server" Visible="False"><p>Your password has been changed.</p></asp:PlaceHolder>	
	<asp:PlaceHolder ID="plcPwdError" Runat="server" Visible="False"><p class="ow_error">Change password failed.</p></asp:PlaceHolder>
	<asp:PlaceHolder ID="plcMsg" Runat="server" Visible="False"><p>Your information has been updated.</p></asp:PlaceHolder>	
	<asp:PlaceHolder ID="plcError" Runat="server" Visible="False"><p class="ow_error">Update failed.</p></asp:PlaceHolder>
	<asp:PlaceHolder ID="plcRemoveMsg" Runat="server" Visible="False"><p>You have been removed from the system.</p></asp:PlaceHolder>	
	<p class="ow_fld">
		<OWWeb:FieldLabel runat="server" For="ow_lblLogonName" ToolTip="Your unique logon name">Logon name:</OWWeb:FieldLabel>
		<strong><asp:Label id="ow_lblLogonName" Runat="server"/></strong>
	</p>

	<asp:PlaceHolder ID="plcPassword" Runat="server">
		<hr class="normal" />
		<h2>Change your password:  <span style="font-size:smaller;font-weight:normal;">(Leave blank to keep existing password)</span></h2>
		<p class="ow_fld">
			<OWWeb:FieldLabel runat="server" For="ow_txtOriginalPassword" ToolTip="For security purposes, please enter your original password.">Original password:</OWWeb:FieldLabel><br />
			<asp:TextBox runat="server" TextMode="Password" id="ow_txtOriginalPassword" width="100" MaxLength="20" CssClass="ow_txt" />
			<asp:RequiredFieldValidator ID="rfvOriginalPassword" ClientID="ow_rfvOriginalPassword" Runat="server"  Enabled="False" EnableViewState="False" CssClass="ow_error"
				ErrorMessage="For security purposes, please enter your original password." Display="None" EnableClientScript="False" ControlToValidate="ow_txtOriginalPassword"/>
		</p>
		<p class="ow_fld">
			<OWWeb:FieldLabel runat="server" For="ow_txtPassword" ToolTip="Please enter your new password. Depending on the logon system, it may have certain complexity requirements (mix of upper- and lower-case characters, numbers, symbols, etc.">New Password:</OWWeb:FieldLabel><br />
			<asp:TextBox runat="server" TextMode="Password" id="ow_txtPassword" width="100" MaxLength="20" CssClass="ow_txt" />
			<asp:RequiredFieldValidator ID="rfvPassword" ClientID="ow_rfvPassword" Runat="server"  Enabled="False" EnableViewState="False" CssClass="ow_error"
				ErrorMessage="Please specify your new password." Display="None" EnableClientScript="False" ControlToValidate="ow_txtPassword"/>
		</p>
		<p class="ow_fld">
			<OWWeb:FieldLabel runat="server" For="ow_txtConfirm" ToolTip="Please re-enter your password to ensure that it has been entered correctly.">Confirm password:</OWWeb:FieldLabel><br />
			<asp:TextBox runat="server" TextMode="Password" id="ow_txtConfirm" width="100" MaxLength="20" CssClass="ow_txt" />
			<asp:RequiredFieldValidator ID="rfvConfirm" ClientID="ow_rfvConfirm" Runat="server" Enabled="False" CssClass="ow_error"
				EnableViewState="False" ErrorMessage="Please confirm that your new password has been entered properly." Display="None" EnableClientScript="False" ControlToValidate="ow_txtConfirm"/>
			<asp:CompareValidator ID="cvConfirm" ControlToCompare="ow_txtPassword" ClientID="ow_cvConfirm" Runat="server" CssClass="ow_error"
				ErrorMessage="Please confirm that your password has been entered properly." Display="None" EnableClientScript="False" Enabled="False" 
				EnableViewState="False" ControlToValidate="ow_txtConfirm"/>
		</p>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder ID="plcPersonal" Runat="server">
		<hr class="normal" />
		<h2>Update your personal info:</h2>

		<p class="ow_fld" style="display:none;">
			<OWWeb:FieldLabel runat="server" For="ow_txtFirstName" ToolTip="Please enter your first name.  This is a required field.">First name:</OWWeb:FieldLabel><br />
			<asp:TextBox id="ow_txtFirstName" Runat="server" MaxLength="50" Width="150" CssClass="ow_txt"/>&nbsp;<span class="isl_req">&bull;</span>
			<asp:RequiredFieldValidator ClientID="ow_rfvFirstName" Runat="server" 
				ErrorMessage="'First name' is a required field." Display="None" EnableClientScript="False" 
				ControlToValidate="ow_txtFirstName" />
		</p>
		<p class="ow_fld"  style="display:none;">
			<OWWeb:FieldLabel runat="server" For="ow_txtLastName" ToolTip="Please enter your last name.  This is a required field.">Last name:</OWWeb:FieldLabel><br />
			<asp:TextBox id="ow_txtLastName" Runat="server" MaxLength="50" Width="150" CssClass="ow_txt"/>&nbsp;<span class="isl_req">&bull;</span>
			<asp:RequiredFieldValidator ClientID="ow_rfvLastName" Runat="server" 
				ErrorMessage="'Last name' is a required field." Display="None" EnableClientScript="False" 
				ControlToValidate="ow_txtLastName" />
		</p>
		<p class="ow_fld"  style="display:none;">
			<OWWeb:FieldLabel runat="server" For="ow_txtTitle" ToolTip="Please enter your title.">Title:</OWWeb:FieldLabel><br />
			<asp:TextBox id="ow_txtTitle" Runat="server" MaxLength="50" Width="150" CssClass="ow_txt"/>
		</p>
		<p class="ow_fld">
			<OWWeb:FieldLabel runat="server" For="ow_txtEmail" ToolTip="Please enter your email address.  This is a required field.">Email:</OWWeb:FieldLabel><br />
			<asp:TextBox id="ow_txtEmail" Runat="server" MaxLength="50" Width="150" CssClass="ow_txt"/>&nbsp;<span class="isl_req">&bull;</span>
			<asp:RequiredFieldValidator ClientID="ow_rfvEmail" Runat="server" 
				ErrorMessage="'Email' is a required field." Display="None" EnableClientScript="False" 
				ControlToValidate="ow_txtEmail" />
			<asp:RegularExpressionValidator ID="revEmail" ClientID="ow_revEmail" Runat="server" CssClass="ow_error"
				ErrorMessage="Please verify the specified email address." Display="none" EnableClientScript="false" Enabled="False" 
				EnableViewState="False" ControlToValidate="ow_txtEmail" 
				ValidationExpression="^([a-zA-Z0-9_\.\-\+])+@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,6})$" />
		</p>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder ID="plcUpdateButtons" runat="server">
		<asp:Button id="ow_btnUpdate" Runat="server" CssClass="ow_btnUpdate" Text="Update" CausesValidation="False"/>
		<asp:ImageButton id="ow_imgUpdate" Runat="server" CssClass="ow_imgUpdate" ImageUrl="~/core/images/blank.gif" CausesValidation="False"/>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder ID="plcRemove" Runat="server">
		<hr class="normal" />
		<h2>Remove yourself from the system:</h2>
		<p class="ow_fld">
			<OWWeb:FieldLabel runat="server" For="ow_txtRemovePassword" ToolTip="For security purposes, please enter your original password.">Your current password:</OWWeb:FieldLabel><br />
			<asp:TextBox runat="server" TextMode="Password" id="ow_txtRemovePassword" width="100" MaxLength="20" CssClass="ow_txt" />
			<asp:RequiredFieldValidator ClientID="ow_rfvRemovePassword" Runat="server" Enabled="False" CssClass="ow_error"
				ErrorMessage="Your password is required to remove yourself from the system." Display="None" EnableClientScript="False" ControlToValidate="ow_txtPassword"/>
		</p>
		<input type="hidden" id="ow_xConfirmRemove" runat="server" value="Are you sure you wish to remove yourself from the system?"/>
		<asp:Button id="ow_btnRemove" CssClass="ow_btnRemove" Runat="server" Text="Remove" CausesValidation="False"/>
		<asp:ImageButton id="ow_imgRemove" CssClass="ow_imgRemove" Runat="server" ImageUrl="~/core/images/blank.gif" CausesValidation="False"/>
	</asp:PlaceHolder>
</div><%-- MemberProfile.ascx - 7.0.4881 --%> 
