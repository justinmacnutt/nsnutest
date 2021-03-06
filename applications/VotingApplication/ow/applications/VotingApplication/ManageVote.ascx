<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ManageVote.ascx.vb" Inherits="ISL.OneWeb.ClientApplications.NSNU.VotingApplication.UI.Admin.ManageVote" %>
<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Web" Namespace="ISL.OneWeb4.UI.WebControls" %>


<OWWeb:ScriptBlock runat="server" ScriptType="text/javascript" DocumentSource="~/ow/scripts5/ow_forms.js" EnableViewState="false" />
<OWWeb:ScriptBlock runat="server" ScriptType="text/javascript" DocumentSource="va_AdminVote.js" IncludeLocation="Startup" EnableViewState="false" />

<p class="isl_intro"><asp:Literal id="litIntro" Runat="server" /></p>

<%-- Common OneWeb validation html --%>
<div class="isl_form_info_bar clearfix">
    <div class="isl_global_error">
		<asp:ValidationSummary  runat="server" EnableClientScript="true" ShowSummary="true"
			ShowMessageBox="false" DisplayMode="BulletList" HeaderText="errorSummaryMessage" 
			ForeColor="" CssClass="isl_error" />
	</div>
    <div class="isl_form_legend">
        <span class="isl_req">&bull;</span> <asp:Literal Runat="server" Text="requiredField"/>
        &nbsp;&nbsp;
        <span class="isl_lang_indicator"><asp:Literal Runat="server" Text="languageType" /></span> <asp:Literal Runat="server" Text="languageSpecific"/> 
    </div>
</div>


<telerik:RadTabStrip id="ow_tsVote" runat="server" MultiPageId="ow_mvVote" AutoPostBack="false" CausesValidation="false">
	<Tabs>
		<telerik:RadTab runat="server" id="ow_tbOne" PageViewID="ow_pvEditVote" Text="editVote" Selected="true" Value="addvote"/>
		<telerik:RadTab runat="server" id="ow_tbTwo" PageViewID="ow_pvResults" Text="results" Value="results"/>
    </Tabs>
</telerik:RadTabStrip>

<telerik:RadMultiPage ID="ow_mvVote" runat="server" CssClass="ow_mvContainer" SelectedIndex="0">

   <telerik:RadPageView ID="ow_pvEditVote" CssClass="ow_pvContent" runat="server">

    <fieldset class="isl_fset">
        <p class="isl_fld">
	        <OWWeb:FieldLabel for="ow_txtTitle" runat="server">
		        <asp:Literal Text="voteName" Runat="server" /> <span class="isl_req">&bull;</span> 
	        </OWWeb:FieldLabel><br />
	        <asp:Textbox runat="server" id="ow_txtTitle" MaxLength="200" CssClass="isl_txt" />
	        <asp:RequiredFieldValidator ClientID="ow_rfvTitle" Runat="server" 
		        Display="Dynamic" Text="voteNameRequired" CssClass="invalid" 
		        EnableClientScript="False" ControlToValidate="ow_txtTitle" />
        </p>

        <p class="isl_fld">
	        <OWWeb:FieldLabel for="ow_txtLure" runat="server">
		        <asp:Literal Text="luretext" Runat="server"/> <span class="isl_req">&bull;</span> 
	        </OWWeb:FieldLabel><br />
	        <asp:Textbox runat="server" id="ow_txtLure" MaxLength="200" CssClass="isl_txt" />
	        <asp:RequiredFieldValidator ClientID="ow_rfvLure" Runat="server" 
		        Display="Dynamic" Text="luretextRequired" CssClass="invalid" 
		        EnableClientScript="False" ControlToValidate="ow_txtLure" />
        </p>

        <div class="isl_fld isl_tip">
	        <OWWeb:FieldLabel for="ow_txtQuestion" runat="server">
		        <asp:Literal Text="question" Runat="server"/> <span class="isl_req">&bull;</span>
	        </OWWeb:FieldLabel><br />
	        <asp:Textbox runat="server" id="ow_txtQuestion" maxlength="4000" width="350" CssClass="isl_txt" TextMode="Multiline" Rows="5" />
	        <asp:RequiredFieldValidator ClientID="ow_rfvQuestion" Runat="server" Text="questionRequired" Display="Dynamic" CssClass="invalid" 
		        EnableClientScript="False" ControlToValidate="ow_txtQuestion"/>
			<asp:RegularExpressionValidator ClientId="ow_cvQuestionLength" ControlToValidate="ow_txtQuestion" runat="server" ValidationExpression="^[\s\S]{0,4000}$"
				Text="questionTextTooLong" Display="Dynamic" CssClass="invalid" EnableClientScript="False" />
			<p class="isl_fld_tip"><asp:Literal Runat="server" Text="questionLength"/></p>
        </div>


         <p class="isl_fld">
	        <OWWeb:FieldLabel for="ow_txtAnswer1" runat="server">
		        <asp:Literal Text="answer1" Runat="server"/> <span class="isl_req">&bull;</span> 
	        </OWWeb:FieldLabel><br />
	        <asp:Textbox runat="server" id="ow_txtAnswer1" MaxLength="100" CssClass="isl_txt" />
	        <asp:RequiredFieldValidator ClientID="ow_rfvAnswer1" Runat="server" 
		        Display="Dynamic" Text="answer1Required" CssClass="invalid" 
		        EnableClientScript="False" ControlToValidate="ow_txtAnswer1" />
        </p>

        <p class="isl_fld">
	        <OWWeb:FieldLabel for="ow_txtAnswer2" runat="server">
		        <asp:Literal Text="answer2" Runat="server"/> <span class="isl_req">&bull;</span> 
	        </OWWeb:FieldLabel><br />
	        <asp:Textbox runat="server" id="ow_txtAnswer2" MaxLength="100" CssClass="isl_txt" />
	        <asp:RequiredFieldValidator ClientID="ow_rfvAnswer2" Runat="server" 
		        Display="Dynamic" Text="answer2Required" CssClass="invalid" 
		        EnableClientScript="False" ControlToValidate="ow_txtAnswer2" />
        </p>  
        
        <div class="isl_fld">
	        <OWWeb:FieldLabel Runat="server" for="ow_dtDisplayDateAndTime" ToolTip="startDateAndTimeToolTip">
	        <asp:Literal runat="server" text="startDateAndTime"/> <span class="isl_req">&bull;</span>
	        </OWWeb:FieldLabel><br />
	        <telerik:RadDateTimePicker ID="ow_dtDisplayDateAndTime" runat="server" ShowPopupOnFocus="true">
                <TimeView runat="server" Interval="01:00:00" /> 
            </telerik:RadDateTimePicker>
	        <asp:RequiredFieldValidator ClientId="ow_rfvDisplayDateAndTime" runat="server"
		        Text="startDateAndTimeRequired" Display="Dynamic" CssClass="invalid" 
		        EnableClientScript="False" ControlToValidate="ow_dtDisplayDateAndTime" />
        </div>    
        
        
        <div class="isl_fld">
	        <OWWeb:FieldLabel Runat="server" for="ow_dtExpiryDateAndTime" ToolTip="expiryDateAndTimeToolTip">
	        <asp:Literal runat="server" text="expiryDateAndTime"/> <span class="isl_req">&bull;</span>
	        </OWWeb:FieldLabel><br />
	        <telerik:RadDateTimePicker ID="ow_dtExpiryDateAndTime" runat="server" ShowPopupOnFocus="true">
                <TimeView runat="server" Interval="01:00:00" /> 
            </telerik:RadDateTimePicker>
	        <asp:RequiredFieldValidator ClientId="ow_rfvExpiryDateAndTime" runat="server"
		            Text="expiryDateAndTimeRequired" Display="Dynamic" CssClass="invalid" 
		            EnableClientScript="False" ControlToValidate="ow_dtExpiryDateAndTime" />
            <asp:CustomValidator ClientId="ow_DateCompare" runat="server" CssClass="invalid"
                     ErrorMessage="endDateInvalid" Display="Dynamic" EnableClientScript="False"/>  
            <asp:CustomValidator ClientId="ow_DateDuration" runat="server" CssClass="invalid"
                     ErrorMessage="voteDurationInvalid" Display="Dynamic" EnableClientScript="False"/>  
        </div>            
               
     </fieldset>
    </telerik:RadPageView>

	<%-- vote results tab --%>
	<telerik:RadPageView ID="ow_pvResults" CssClass="ow_pvContent" runat="server">

    <style type="text/css">

		div.tableTitle { font-weight: bold; color: #B5B3B4; font-size:1.3em;}
        table {border-collapse:collapse;}
		td.label { font-weight: bold; padding:8px; }
		td.value { font-weight: normal; background-color: #CCD9E6; text-align: center; border: 1px solid black; padding:8px;}

	  </style>
      
    <fieldset class="isl_fset">
    <div>
	        <div class="tableTitle"><asp:Literal runat="server" text="overallvotes"/></div>

            <table>
            <tr>
                <td>&nbsp;</td>
                <td class="label">Count</td>
                <td class="label">%</td>
            </tr>
            <tr>
                <td class="label">Yes</td>
                <td class="value"><asp:Literal runat="server" ID="litOverallYesCount"></asp:Literal></td>
                <td class="value"><asp:Literal runat="server" ID="litOverallYesPercent"></asp:Literal></td>
            </tr>
            <tr>
                <td class="label">No</td>
                <td class="value"><asp:Literal runat="server" ID="litOverallNoCount"></asp:Literal></td>
                <td class="value"><asp:Literal runat="server" ID="litOverallNoPercent"></asp:Literal></td>
            </tr>
            </table>
    </div>


    <div>
	        <div class="tableTitle"><asp:Literal runat="server" text="votesbydesignation"/></div>
            <asp:Repeater runat="server" ID="rptVotesByDesignation">
            <HeaderTemplate>
            <table>
            <tr>
                <td>&nbsp;</td>
                <td class="label">Count</td>
                <td class="label">%</td>
            </tr>
            </HeaderTemplate>
            <ItemTemplate>
            <tr>
                <td class="label"><%# DataBinder.Eval(Container, "DataItem.nurseDesignationname")%></td>
                <td class="value"><%# DataBinder.Eval(Container, "DataItem.TotalVote")%></td>
                <td class="value"><%# String.Concat(DataBinder.Eval(Container, "DataItem.PercentVote"), "%") %></td>
            </tr>            
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>    
    </div>	
    <div>
	        <div class="tableTitle"><asp:Literal runat="server" text="votesbysector"/></div>
             <asp:Repeater runat="server" ID="rptVotesBySector">
            <HeaderTemplate>
            <table>
            <tr>
                <td>&nbsp;</td>
                <td class="label">Count</td>
                <td class="label">%</td>
            </tr>
            </HeaderTemplate>
            <ItemTemplate>
            <tr>
                <td class="label"><%# DataBinder.Eval(Container, "DataItem.FacilityTypeName")%></td>
                <td class="value"><%# DataBinder.Eval(Container, "DataItem.TotalVote")%></td>
                <td class="value"><%# String.Concat(DataBinder.Eval(Container, "DataItem.PercentVote"), "%") %></td>
            </tr>            
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>        
    </div>	
    <div>
	        <div class="tableTitle"><asp:Literal runat="server" text="votesbyregion"/></div>
              <asp:Repeater runat="server" ID="rptVotesByRegion">
            <HeaderTemplate>
            <table>
            <tr>
                <td>&nbsp;</td>
                <td class="label">Count</td>
                <td class="label">%</td>
            </tr>
            </HeaderTemplate>
            <ItemTemplate>
            <tr>
                <td class="label"><%# DataBinder.Eval(Container, "DataItem.RegionName")%></td>
                <td class="value"><%# DataBinder.Eval(Container, "DataItem.TotalVote")%></td>
                <td class="value"><%# String.Concat(DataBinder.Eval(Container, "DataItem.PercentVote"), "%")%></td>
            </tr>            
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>      
    </div>
    <div>
	        <div class="tableTitle"><asp:Literal runat="server" text="votesbyfacility"/></div>
             <asp:Repeater runat="server" ID="rptVotesByFacility">
            <HeaderTemplate>
            <table>
            <tr>
                <td>&nbsp;</td>
                <td class="label">Count</td>
                <td class="label">%</td>
            </tr>
            </HeaderTemplate>
            <ItemTemplate>
            <tr>
                <td class="label"><%# DataBinder.Eval(Container, "DataItem.FacilityName")%></td>
                <td class="value"><%# DataBinder.Eval(Container, "DataItem.TotalVote")%></td>
                <td class="value"><%# String.Concat(DataBinder.Eval(Container, "DataItem.PercentVote"), "%") %></td>
            </tr>            
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>       
    </div>
    <div>
	        <div class="tableTitle"><asp:Literal runat="server" text="votesbydate"/></div>

            <asp:Repeater runat="server" ID="rptVotesByDate">
            <HeaderTemplate>
            <table>
            <tr>
                <td>&nbsp;</td>
                <td class="label">Count</td>
                <td class="label">%</td>
            </tr>
            </HeaderTemplate>
            <ItemTemplate>
            <tr>
                <td class="label"><%# CDate(DataBinder.Eval(Container, "DataItem.Votedate")).ToString("MMM dd, yyyy")%></td>
                <td class="value"><%# DataBinder.Eval(Container, "DataItem.TotalVote")%></td>
                <td class="value"><%# String.Concat(DataBinder.Eval(Container, "DataItem.PercentVote"), "%") %></td>
            </tr>            
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
  
    </div>	


   	
    </fieldset>

	</telerik:RadPageView>    
    
    
</telerik:RadMultiPage>


<p class="isl_p_btn">
    <asp:Button Runat="server" ID="ow_btnOK" CssClass="isl_btn" CausesValidation="False" />
    <asp:Button Runat="server" ID="ow_btnDelete" CssClass="isl_btn" CausesValidation="False" />
    <asp:Button Runat="server" ID="ow_btnCancel" CssClass="isl_btn" CausesValidation="False" />
</p>





