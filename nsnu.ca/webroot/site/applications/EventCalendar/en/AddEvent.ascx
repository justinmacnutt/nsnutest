<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AddEvent.ascx.vb" Inherits="ISL.OneWeb4.Modules.EventCalendar.UI.App.AddEvent" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>

<%-- Event Calendar styles --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/mod_ec.css ~/core/styles/mod_ec.css" />
<OWWeb:ScriptBlock runat="server" ScriptType="text/javascript" DocumentSource="~/core/scripts/ow_apps.js" IncludeLocation="AfterAjax" EnableViewState="false" />
<OWWeb:ScriptBlock runat="server" ScriptType="text/javascript" DocumentSource="../ec_AddEvent.js" IncludeLocation="Startup" EnableViewState="false"/>

<asp:PlaceHolder runat="server" ID="plcEvent">
<div class="ec_requiredfield">
	<span class="required">&bull;</span> indicates a required field 	
	<asp:ValidationSummary Runat="server" DisplayMode="BulletList" ForeColor="" ID="vldSummary" ValidationGroup="AddEvent"
		EnableClientScript="true" HeaderText="&lt;strong&gt;Highlighted fields are required.&lt;/strong&gt;" ShowMessageBox="False" ShowSummary="true" CssClass="ec_error"/>
</div>	

<div id="ec_addevent" class="ow_app">

<div class="ec_title">Submitter Details</div>
<div class="ec_subtitle">The submitter details do not display on the event listing.</div>
<div class="ec_addeventfields">

        <div class="ec_sf_group1 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtSubmitterName" ToolTip="Provide the name of the person submitting this event. The value of this field will not display on the website once the event is published.">
                    Submitter's name <span class="required">&bull;</span>
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
                <asp:TextBox id="ec_txtSubmitterName" runat="server" MaxLength="50" CssClass="ec_txt"/>
		        <asp:RequiredFieldValidator ClientId="ec_rfvSubmitterName" runat="server" Text="Submitter's name is required." CssClass="invalid"
			        Display="Dynamic" EnableClientScript="False" EnableViewState="False" ControlToValidate="ec_txtSubmitterName" ValidationGroup="AddEvent"/>
             </div>
        </div>

        <div class="ec_sf_group2 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtSubmitterEmail" ToolTip="Provide the email of the person submitting this event. The value of this field will not display on the website once the event is published.">
                    Submitter's email <span class="required">&bull;</span>
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
                <asp:TextBox id="ec_txtSubmitterEmail" runat="server" MaxLength="50" CssClass="ec_txt"/>
		        <asp:RequiredFieldValidator ClientId="ec_rfvSubmitterEmail" runat="server" Text="Submitter's email is required." CssClass="invalid"
			        Display="Dynamic" EnableClientScript="False" EnableViewState="False" ControlToValidate="ec_txtSubmitterEmail" ValidationGroup="AddEvent"/>
		        <asp:RegularExpressionValidator ClientID="ec_regvSubmitterEmail" Runat="server"  CssClass="invalid"
			        Text="Verify the Submitter’s email is in the correct format." Display="Dynamic" EnableClientScript="false"
			        ControlToValidate="ec_txtSubmitterEmail" EnableViewState="False" ValidationExpression="^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" ValidationGroup="AddEvent"/>				
            </div>
	        <div class="ec_tooltip">(person@isl.ca)</div>
        </div>
</div>
<hr />
<div class="ec_title">Event Details</div>
<div class="ec_subtitle">All of this information displays on the event listing except for the image code.</div>
<div class="ec_addeventfields">    

        <div class="ec_sf_group3 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtName" ToolTip="Provide the name of the event.">
                    Event name <span class="required">&bull;</span>
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
                <asp:TextBox id="ec_txtName" runat="server" MaxLength="50" CssClass="ec_txt"/>
		        <asp:RequiredFieldValidator ClientId="ec_rfvName" runat="server" Text="Event name is required." CssClass="invalid"
			        Display="Dynamic" EnableClientScript="False" EnableViewState="False" ControlToValidate="ec_txtName" ValidationGroup="AddEvent"/>
             </div>
        </div> 
        <asp:PlaceHolder runat="server" ID="plcCategory">
        <div class="ec_sf_group4 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_lstCategory" ToolTip="Select the category of the event.">
                    Category <span class="required">&bull;</span>
		        </OWWeb:FieldLabel>
	        </div>
            <div class="forminput">
	            <asp:DropDownList Runat="server" ID="ec_lstCategory" AutoPostBack="False">
                    <asp:ListItem Text="Select..." Value="0">Select...</asp:ListItem>
                </asp:DropDownList>
	            <asp:RequiredFieldValidator ClientID="ow_rfvCategory" Runat="server" Text="Category is required." 
			        Display="Dynamic" EnableClientScript="false" ControlToValidate="ec_lstCategory" CssClass="invalid" InitialValue="0" ValidationGroup="AddEvent" />
            </div>
        </div>   
         </asp:PlaceHolder>    
         
                   
        <div class="ec_sf_group5 clearfix">
	        <%-- Start Date --%>
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_dtStartDate" ToolTip="Select the date that the event occurs, or starts. Recurring event functionality is not available.">
                    Start date <span class="required">&bull;</span>
                </OWWeb:FieldLabel>
            </div>
	        <div class="forminput">
                <telerik:RadDatePicker ID="ec_dtStartDate" runat="server" ShowPopupOnFocus="true" />
	            <asp:RequiredFieldValidator ClientId="ec_rfvStartDate" runat="server"
		            Text="Start date is required." Display="Dynamic" CssClass="invalid" 
		            EnableClientScript="False" ControlToValidate="ec_dtStartDate" ValidationGroup="AddEvent" />
                <asp:CompareValidator ClientId="ec_DateCompare" runat="server" ControlToValidate="ec_dtEndDate"
                    ControlToCompare="ec_dtStartDate" Operator="GreaterThan" Type="Date" ErrorMessage="The end date must be later than the start date." 
                    Display="Dynamic" EnableClientScript="False"  CssClass="invalid" ValidationGroup="AddEvent"/>
             </div>
        </div>      
          		                   
        <div class="ec_sf_group6 clearfix" style="margin-left:30px;">
 	        <%-- Start Time --%>                    
			<div class="formlabel"><OWWeb:FieldLabel Runat="server" for="ec_tmStartTime" Text="Start time" ToolTip="Select the time that the event begins."/></div>
	        <div class="forminput">
				<telerik:RadTimePicker ID="ec_tmStartTime" runat="server" ShowPopupOnFocus="true"
					DateInput-DateFormat="t" TimeView-ShowHeader="false" TimeView-TimeFormat="t" TimeView-HeaderText="Select start time" ValidationGroup="AddEvent"/>
	            <asp:CustomValidator ClientID="ec_cvTimeRequired" Runat="server"
			            Text="Both the start and end times must be set together if one value is set." EnableClientScript="False" Display="Dynamic" CssClass="invalid" ValidationGroup="AddEvent"/>
			</div>   
			<div class="ec_tooltip"><asp:Literal ID="litStartTimeZoneName" Runat="server" /></div><br />
                      
			<div class="formlabel"><OWWeb:FieldLabel Runat="server" for="ec_tmEndTime" Text="End time" ToolTip="Select the time that the event ends." /></div>
	        <div class="forminput">
				<telerik:RadTimePicker ID="ec_tmEndTime" runat="server" ShowPopupOnFocus="true"
					DateInput-DateFormat="t" DateInput-DisplayDateFormat="t" TimeView-ShowHeader="false" 
					TimeView-Interval="00:15:00" TimeView-TimeFormat="t" TimeView-HeaderText="Select end time" ValidationGroup="AddEvent" />
                <asp:CompareValidator ClientId="ec_cvTimeCompare" runat="server" ControlToValidate="ec_tmEndTime"
                    ControlToCompare="ec_tmStartTime" Operator="GreaterThanEqual" Type="String" ErrorMessage="The end time must be later than the start time." 
                    Display="Dynamic" EnableClientScript="False" CssClass="invalid" ValidationGroup="AddEvent"/>
			</div>   
        </div>      

        <div class="ec_sf_group7 clearfix">
            <%-- End Date --%>
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_dtEndDate" ToolTip="Select the date that the event ends.">End date</OWWeb:FieldLabel>
            </div>
	        <div class="forminput">
                <telerik:RadDatePicker ID="ec_dtEndDate" runat="server" ShowPopupOnFocus="true" />
            </div>
	        <div class="ec_tooltip">(Leave blank for one-day events)</div>
        </div>

        <div class="ec_sf_group8 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtLocation">
                    Location
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
	            <asp:Textbox runat="server" id="ec_txtLocation" MaxLength="50" CssClass="isl_txt" />
             </div>
        </div>
    	
        <div class="ec_sf_group9 clearfix">
        	<div class="formlabel">
	            <OWWeb:FieldLabel for="ec_txtSummary" runat="server" ToolTip="Provide a brief summary for the event. This summary will appear in the list of events as a quick preview before the user chooses to view the full details.">
                    Summary
		        </OWWeb:FieldLabel>
            </div>
	        <div class="forminput">
	            <asp:Textbox runat="server" id="ec_txtSummary" maxlength="255" width="350" CssClass="isl_txt" TextMode="Multiline" Rows="3" />
	            <asp:RegularExpressionValidator ClientID="ec_cvSummaryLength" Runat="server" ControlToValidate="ec_txtSummary" ValidationExpression="^[\s\S]{0,255}$"
			            Text="Summary cannot exceed 255 chracters." EnableClientScript="False" Display="Dynamic" CssClass="invalid" ValidationGroup="AddEvent"/>
              </div>   		
        </div>

        <div class="ec_sf_group10 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtDescription" ToolTip="The full details about the event should be provided here.">
                    Full description <span class="required">&bull;</span>
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
	            <asp:Textbox runat="server" id="ec_txtDescription" maxlength="255" width="350" CssClass="isl_txt" TextMode="Multiline" Rows="3" />
	            <asp:RequiredFieldValidator ClientID="ec_rfvDescription" Runat="server" Text="Full description is required" Display="Dynamic" CssClass="invalid" 
		            EnableClientScript="False" ControlToValidate="ec_txtDescription" ValidationGroup="AddEvent" />
			    <asp:RegularExpressionValidator ClientId="ec_cvDescriptionLength" ControlToValidate="ec_txtDescription" runat="server" ValidationExpression="^[\s\S]{0,1024}$"
				    Text="Full description cannot exceed 1024 chracters." Display="Dynamic" CssClass="invalid" EnableClientScript="False" ValidationGroup="AddEvent" />
            </div> 		        
        </div>

        <div class="ec_sf_group11 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtContactName">
                    Contact name
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
	            <asp:Textbox runat="server" id="ec_txtContactName" MaxLength="50" CssClass="isl_txt" />
            </div>
       </div>

        <div class="ec_sf_group12 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtContactPhone">
                    Contact phone
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
	            <asp:Textbox runat="server" id="ec_txtContactPhone" MaxLength="50" CssClass="isl_txt" />
            </div>
       </div>


        <div class="ec_sf_group13 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtContactEmail">
                    Contact email
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
	            <asp:Textbox runat="server" id="ec_txtContactEmail" MaxLength="50" CssClass="isl_txt" />
	            <asp:RegularExpressionValidator ClientId="ec_rxvContactEmail" Runat="server" CssClass="invalid"
		            Text="Verify the contact email is in the correct format." Display="Dynamic" EnableClientScript="false" ControlToValidate="ec_txtContactEmail" 
		            ValidationExpression="^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,4})$" ValidationGroup="AddEvent"/>
            </div>
	        <div class="ec_tooltip">(person@isl.ca)</div>
        </div>
    		
        <div class="ec_sf_group14 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtWebsite" Title="Provide the URL of the website where more details about this event exist.">
                    Event website
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
	            <asp:TextBox Runat="server" id="ec_txtWebsite" CssClass="isl_txt" />
	            <asp:RegularExpressionValidator ClientId="ec_rxvWebsite" Runat="server" CssClass="invalid"
		            Text="Verify the event website address is in the correct format." Display="Dynamic" EnableClientScript="false" ControlToValidate="ec_txtWebsite" 
		            ValidationExpression="^((?:ftp|http|https):\/\/)+([!-~])+([.])+(?:com|edu|biz|org|gov|int|info|mil|net|name|museum|coop|aero|[A-Za-z][A-Za-z])\b(?:\:\d+)?(?:\/[!-~]*)?$" ValidationGroup="AddEvent"/>
            </div>
	        <div class="ec_tooltip">(http://www.isl.ca)</div>
        </div>
    		
        <div class="ec_sf_group15 clearfix">
	        <div class="formlabel">
                <OWWeb:FieldLabel Runat="server" For="ec_txtRegistrationWebsite" ToolTip="If applicable, provide the URL of the website where the user goes to register for this event.">
                    Registration website
		        </OWWeb:FieldLabel>
	        </div>
	        <div class="forminput">
	            <asp:TextBox Runat="server" id="ec_txtRegistrationWebsite" CssClass="isl_txt" />
	            <asp:RegularExpressionValidator ClientId="ec_rxvRegistrationWebsite" Runat="server" CssClass="invalid"
		            Text="Verify the event registration website address is in the correct format." Display="Dynamic" EnableClientScript="false" ControlToValidate="ec_txtRegistrationWebsite" 
		            ValidationExpression="^((?:ftp|http|https):\/\/)+([!-~])+([.])+(?:com|edu|biz|org|gov|int|info|mil|net|name|museum|coop|aero|[A-Za-z][A-Za-z])\b(?:\:\d+)?(?:\/[!-~]*)?$" ValidationGroup="AddEvent"/>
            </div>
	        <div class="ec_tooltip">(http://www.isl.ca/register)</div>
        </div>
        <div class="ec_sf_group16 clearfix"> 
 
	        <OWWeb:FieldLabel runat="server" For="Captcha" ToolTip="Type in the letters and numbers you see in the image.  This is a required field." EnableViewState="False">
		          Please type the code you see in the image <span class="required" title="This is a required field.">&bull;</span>
	        </OWWeb:FieldLabel>
	        <telerik:RadCaptcha ID="ec_Captcha" runat="server" CaptchaTextBoxLabel="" CssClass=""
		        ProtectionMode="Captcha" CaptchaImage-Height="35" CaptchaImage-Width="100" CaptchaImage-TextLength="5"
		        Text="The code you entered is not valid"  Display="Dynamic"  EnableRefreshImage="true" ValidationGroup="AddEvent" />
			<input type="hidden" runat="server" id="ec_rfvCaptcha" value="Please enter the code visible in the image into the text box." class="invalid" validationGroup="AddEvent" />
         </div> 

</div>
<div class="ec_searchbutton clearfix">
    <div class="ec_button1"><asp:Button Runat="server" ID="ec_btnOK" CssClass="isl_btn" CausesValidation="False" Text="Submit" ValidationGroup="AddEvent" /></div>
</div>
</div>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="plcErrorMessage">
An error occurred submitting your event.
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="plcConfirmationMessage">
Thank you for submitting your event. Someone will be in contact with you shortly with details on when your event will be available on our website.
</asp:PlaceHolder>


<%-- AddEvent.ascx - 7.0.4752 --%> 
