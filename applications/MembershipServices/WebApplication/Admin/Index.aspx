<%@ Page Language="C#" MasterPageFile="~/Nsnu.master" enableEventValidation="false" AutoEventWireup="true" Inherits="WebApplication.Admin.Index" Codebehind="Index.aspx.cs" %>

<asp:Content ID="cHeadContent" ContentPlaceHolderID="cphHeadContent" Runat="Server"> 
     <script type="text/javascript">
         $(document).ready(function () {
             InitializeEventBindings();
             InstantiatePositionDropdown();
             CheckMultiSelectDropdowns();

             if ($("[id*=hdnShuntToResults]").val() == "1") {
                 goToByScroll('dvSearchResults');
             }

         });
         
         function InstantiatePositionDropdown() {
             FillPositionDropDown();
             if ($("[id*=hdnPositionId]").val() != "")  {
                 $("[id*=ddlPosition]").val($("[id*=hdnPositionId]").val());
             }
         }

         function goToByScroll(id) {
             // Remove "link" from the ID
             id = id.replace("link", "");
             // Scroll
             $('html,body').animate({
                 scrollTop: $("#" + id).offset().top
             },
        'fast');
         }

         // sets the drop down values appropriately for facility, Region, and District
         // selection of these is mutually exclusive
         function InitializeEventBindings() {

             $("[id*=ddlFacility]").change(function () {
                 // alert($(this).val()); // gets the selected value
                 $("[id*=ddlDistrict]").prop('selectedIndex', 0);
                 $("[id*=ddlRegion]").prop('selectedIndex', 0);
             });

             $("[id*=ddlDistrict]").change(function () {
                 $("[id*=ddlFacility]").prop('selectedIndex', 0);
                 $("[id*=ddlRegion]").prop('selectedIndex', 0);
             });

             $("[id*=ddlRegion]").change(function () {
                 $("[id*=ddlDistrict]").prop('selectedIndex', 0);
                 $("[id*=ddlFacility]").prop('selectedIndex', 0);
             });

             $("[id*=ddlPosition]").change(function () {
                 $("[id*=hdnPositionId]").val($(this).val());
             });

             $("[id*=ddlCommittee]").change(function () {
                 $("[id*=hdnPositionId]").val("");
                 FillPositionDropDown();
             });

             $("#lnkClearFilters").click(function () {
                 $("#formElements").find("input[type=text]").val('');
 
                 $('[id*=ddlDesignation]').val('');
                 $('[id*=ddlSector]').val('');
                 $('[id*=ddlFacility]').val('');
                 $('[id*=ddlDistrict]').val('');
                 $('[id*=ddlRegion]').val(''); 
                 $('[id*=ddlCommittee]').val('');
                 $('[id*=ddlCommunicationOption]').val('');
                 $('[id*=ddlEmployerGroup]').val('');

                 $("[id*=ddlPosition] > option:gt(0)").remove();
                 $("[id*=hdnPositionId]").val('');
                 
                 $('#cphMainContent_ddlStatus').multiselect("uncheckAll");
                 $('#cphMainContent_ddlLocalTableOfficerPosition').multiselect("uncheckAll");
             });

             $(".toggler").click(function () {
                 $(this).toggleClass("closed").next(".collapsible").slideToggle("fast");
             });

             $("[id*=btnFilter]").click(function () {

                 $("[id*=hdnEmploymentStatusList]").val($("[id*=ddlStatus]").val());

                 $("[id*=hdnLocalTableOfficerPositionList]").val($("[id*=ddlLocalTableOfficerPosition]").val());

                 return true;
             });

             $("[id*=ddlStatus]").multiselect({
                 noneSelectedText: 'Please Select...',
                 selectedList: 4,
                 autoOpen: false
             });

             $("[id*=ddlLocalTableOfficerPosition]").multiselect({
                 noneSelectedText: 'Please Select...',
                 selectedList: 4,
                 autoOpen: false
             });


         }

         function CheckMultiSelectDropdowns() {
             //alert($("[id*=hdnEmploymentStatusList]").val());

             $.each($("[id*=hdnEmploymentStatusList]").val().split(','), function (idx, val) {
                 $("[id*=ddlStatus]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                     this.click();
                 });
             });

             $.each($("[id*=hdnLocalTableOfficerPositionList]").val().split(','), function (idx, val) {
                 $("[id*=ddlLocalTableOfficerPosition]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                     this.click();
                 });
             });


         }


         function FillPositionDropDown() {

             var committeeId = $("[id*=ddlCommittee]").val(); 

             $("[id*=ddlPosition]")[0].options.length = 0;

             $.ajax({
                 type: "POST",
                 url: "Index.aspx/GetPositions",
                 data: '{"committeeId":"' + committeeId + '"}',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     var data = msg.d;
                     var len = data.length;

                     var html = '';

                     if (len > 0) {
                         for (var i = 0; i < len; i++) {
                             html += '<option value="' + data[i].Value + '">' + data[i].Text + '</option>';
                         }
                     }

                     $("[id*=ddlPosition]").append(html);
                     
                     if ($("[id*=hdnPositionId]").val() != "") {
                         $("[id*=ddlPosition]").val($("[id*=hdnPositionId]").val());
                     }

                 },
                 error: function (xhr, status, error) {
                     // Display a generic error for now.
                     alert("AJAX Error filling position drop down!");
                     alert(xhr.status);
                     alert(error);
                 }
             });

         }
         
       


    </script>
</asp:Content>

<asp:Content ID="cMainContent" ContentPlaceHolderID="cphMainContent" Runat="Server"> 
    <asp:HiddenField ID="hdnShuntToResults" runat="server"/>
    <asp:HiddenField ID="hdnEmploymentStatusList" runat="server" />
    <asp:HiddenField ID="hdnLocalTableOfficerPositionList" runat="server" />
    <asp:HiddenField ID="hdnPositionId" runat="server"></asp:HiddenField>
    
    <div class="section page-heading row clearfix">
	    <div class="grid_12">
		    <h1 class="float-left">Nurse Profile Index</h1>
			<a runat="server" id="lnkAddNewNurse" href="NurseProfile.aspx" class="btn float-right">Add a New Nurse</a>
		</div>
    </div>
	
	
    <div class="section search-form row clearfix">
		<h2 class="toggler">Search Index</h2>
	
		<div class="collapsible" id="formElements">
			<div class="wrapper-block clearfix">
				<div class="grid_3" style="display:none">
					<label>User Name</label> <asp:TextBox ID="tbUserName" runat="server" MaxLength="50"></asp:TextBox>
				</div>
                <div class="grid_3">
					<label>Last Name</label> <asp:TextBox ID="tbLastName" runat="server" MaxLength="100"></asp:TextBox>
				</div>
                <div class="grid_3">
					<label>Designation</label> <asp:DropDownList ID="ddlDesignation" runat="server"></asp:DropDownList>
				</div>
				
				<div class="grid_3">
					<label>Employment Status</label> 
                    <asp:DropDownList ID="ddlStatus" multiple="multiple" runat="server">
										<asp:ListItem Value="1">Active</asp:ListItem>
										<asp:ListItem Value="3">LOA</asp:ListItem>
                                        <asp:ListItem Value="2">Inactive</asp:ListItem>
										<asp:ListItem Value="4">Retired</asp:ListItem>
                    </asp:DropDownList>
				</div>
				<div class="grid_3">
					<label>Sector</label> <asp:DropDownList ID="ddlSector" runat="server"></asp:DropDownList>
				</div>
			</div>
			<div class="wrapper-block clearfix">
				<div class="grid_3">
					<label>First Name</label> <asp:TextBox ID="tbFirstName" runat="server" MaxLength="100"></asp:TextBox>
				</div>
				<div class="grid_3">
					<label>Facility</label> <asp:DropDownList ID="ddlFacility" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem> </asp:DropDownList>
				</div>
				<div class="grid_3">
					<label>District</label> <asp:DropDownList ID="ddlDistrict" runat="server"></asp:DropDownList>
				</div>
				<div class="grid_3">
					<label>Region</label> <asp:DropDownList ID="ddlRegion" runat="server"></asp:DropDownList>
				</div>
			</div>
			<div class="wrapper-block clearfix">
				<div class="grid_3">
					<label>Email</label> <asp:TextBox ID="tbEmail" runat="server" MaxLength="100"></asp:TextBox>
				</div>
                <div class="grid_3">
					<label>Local/Table Officer Position</label> 
                    <asp:DropDownList ID="ddlLocalTableOfficerPosition" multiple="multiple" runat="server">
                        
                    </asp:DropDownList>
				</div>
				<div class="grid_3">
					<label>Board/Committee</label> <asp:DropDownList ID="ddlCommittee" runat="server"><asp:ListItem Value="">Please Select</asp:ListItem> </asp:DropDownList>
				</div>
				<div class="grid_3">
					<label>Position</label> <asp:DropDownList ID="ddlPosition" runat="server"> </asp:DropDownList>
				</div>
				<div class="grid_3" style="display:none">
					<label>Miscellaneous</label> <asp:DropDownList ID="ddlMiscellaneousFilter" runat="server">
										<asp:ListItem Value="">Please Select</asp:ListItem> 
										<asp:ListItem Value="1">Casual Representation</asp:ListItem>
										<asp:ListItem Value="2">LPN Representation</asp:ListItem>
									</asp:DropDownList>
				</div>
			</div>
			<div class="wrapper-block clearfix">
				<div class="grid_3">
					<label>Address Line 1</label> <asp:TextBox ID="tbLine1" runat="server" MaxLength="100"></asp:TextBox>
				</div>
                <div class="grid_3">
					<label>Phone</label> <asp:TextBox ID="tbPhone" runat="server" MaxLength="100"></asp:TextBox>
				</div>
				<div class="grid_3">
					<label>Communication Options</label> 
                    <asp:DropDownList ID="ddlCommunicationOption" runat="server">
                        <asp:ListItem runat="server" Value="">Please Select</asp:ListItem>
                        <asp:ListItem runat="server" Value="1">Accepts AIL</asp:ListItem>
                        <asp:ListItem runat="server" Value="2">Accepts Johnsons</asp:ListItem>
                        <asp:ListItem runat="server" Value="6">Accepts Union Calls</asp:ListItem>
                    </asp:DropDownList>
				</div>
				<div class="grid_3">
					<label>Employer Group</label> <asp:DropDownList ID="ddlEmployerGroup" runat="server"> </asp:DropDownList>
				</div>
			</div>
			
			<div class="search_btns grid_12">
				<asp:Button ID="btnFilter" runat="server" OnClick="btnFilter_OnClick" Text="Search" CssClass="frm-btn" />  <a id="lnkClearFilters" href="#" class="frm-btn">Clear</a>
            	<div class="wrapper-block float-right clearfix">
					<asp:DropDownList ID="ddlExportType" runat="server" CssClass="float-left" style="width:240px; margin-top:12px;">
						<asp:ListItem Value="nursefull" Selected="True">Current Nurse Results - Full</asp:ListItem> 
						<asp:ListItem Value="nursepartial">Current Nurse Results - Partial</asp:ListItem>
						<asp:ListItem Value="facility">Facility - Full</asp:ListItem>
					</asp:DropDownList>
					<asp:Button ID="btnExport" runat="server" OnClick="btnExport_OnClick" Text="Export" CssClass="frm-btn float-left" />
				</div>
			</div>
		</div>
	</div>
	
	<div id="dvSearchResults" class="section row clearfix">
		<div class="grid_12">	
			<div class="letter_nav">
				<asp:HiddenField id="hdnLetterFilter" runat="server" />
				<asp:repeater id="rptLetters" runat="server">
					<SeparatorTemplate>&nbsp;&nbsp;</SeparatorTemplate>
					<itemtemplate>
						<asp:linkbutton id="lnkLetter" runat="server" OnClick="lnkLetter_OnClick" commandname="Filter" commandargument='<%# Container.DataItem %>'><%# Container.DataItem %></asp:linkbutton>
					</itemtemplate>
				</asp:repeater>
				&nbsp;&nbsp;<asp:LinkButton ID="lnkLetterAll" runat="server" OnClick="lnkLetter_OnClick" CommandName="Filter" CommandArgument=''>All</asp:LinkButton>
			</div>
		</div>
	</div>
	
	<div class="section row clearfix">
		<div class="row">
			<div class="grid_12" style="width:960px;">	
				<asp:ListView ID="lvNurses" runat="server">
					<LayoutTemplate>
						<table border="0" cellpadding="0" cellspacing="0" class="tbl_data">
								<tr class="tbl_header">
									<td class="first"><strong>Name</strong></td>
									<td><strong>Phone</strong></td>
									<td><strong>Address </strong></td>
									<td><strong>Des.</strong></td>
									<td><strong>Status</strong></td>
									<td><strong>Facility</strong></td>
                                    <td class="last"><strong>Emp.&nbsp;Type</strong></td>
								</tr>
								<div id="itemPlaceholder" runat="server" />
							</table>
					</LayoutTemplate>
					<ItemTemplate>
						<tr>
							<td class="first"><a href='NurseProfile.aspx?id=<%# Eval("nurseId") %>'><%# Eval("fullName") %></a></td>
							<td class="phone"><%# (Eval("phone") == null) ? "" : Eval("phone") + "<br/>"%><a href="mailto:<%# Eval("email") %>" target="_top"><%# Eval("email") %></a></td>
							<td><%# Eval("address") %></td>
							<td><%# Eval("designation") %></td>
							<td><%# Eval("status") %></td>
							<td><%# Eval("facility") %></td>
                            <td class="last"><%# Eval("employmentType") %></td>
							
						</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
						<tr class="alt">
							<td class="first"><a href='NurseProfile.aspx?id=<%# Eval("nurseId") %>'><%# Eval("fullName") %></a></td>
							<td class="phone"><%# (Eval("phone") == null) ? "" : Eval("phone") + "<br/>"%><a href="mailto:<%# Eval("email") %>" target="_top"><%# Eval("email") %></a></td>
							<td><%# Eval("address") %></td>
							<td><%# Eval("designation") %></td>
							<td><%# Eval("status") %></td>
							<td><%# Eval("facility") %></td>
                            <td class="last"><%# Eval("employmentType") %></td>
						</tr>
					</AlternatingItemTemplate>
				</asp:ListView>
	   
				<div class="pager_nav clearfix">
					<asp:DataPager runat="server" ID="dpNursePager" PageSize="20" PagedControlID="lvNurses">
						<Fields>
							<asp:TemplatePagerField>              
							<PagerTemplate>
							<span class="pageing_results">
							Page
							<asp:Label runat="server" ID="CurrentPageLabel" 
								Text="<%# Container.TotalRowCount>0 ? (Container.StartRowIndex / Container.PageSize) + 1 : 0 %>" />
							of
							<asp:Label runat="server" ID="TotalPagesLabel" 
								Text="<%# Math.Ceiling ((double)Container.TotalRowCount / Container.PageSize) %>" />
							(
							<asp:Label runat="server" ID="TotalItemsLabel" 
								Text="<%# Container.TotalRowCount%>" />
							records)
							</span>
							</PagerTemplate>
							</asp:TemplatePagerField>
					  
							<asp:NextPreviousPagerField
							ButtonType="Link"
							ButtonCssClass="page_prev"
							FirstPageText="First" 
							ShowFirstPageButton="true"
							ShowNextPageButton="false"
							ShowPreviousPageButton="false" />
							<asp:TemplatePagerField><PagerTemplate> &nbsp;|&nbsp; </PagerTemplate></asp:TemplatePagerField>
							<asp:NextPreviousPagerField
							ButtonType="Link"
							ButtonCssClass="page_prev"
							PreviousPageText="Prev"
							ShowNextPageButton="false"
							ShowPreviousPageButton="true" />
							<asp:TemplatePagerField><PagerTemplate> &nbsp;|&nbsp; </PagerTemplate></asp:TemplatePagerField>
							<asp:NumericPagerField ButtonCount="10"  />
							<asp:TemplatePagerField><PagerTemplate> &nbsp;|&nbsp; </PagerTemplate></asp:TemplatePagerField>
							<asp:NextPreviousPagerField
							ButtonType="Link"
							ButtonCssClass="page_next"
							NextPageText="Next"
							ShowNextPageButton="true"
							ShowPreviousPageButton="false" />
							<asp:TemplatePagerField><PagerTemplate> &nbsp;|&nbsp; </PagerTemplate></asp:TemplatePagerField>
							<asp:NextPreviousPagerField
							ButtonType="Link"
							ButtonCssClass="page_next"
							LastPageText="Last"
							ShowLastPageButton="true"
							ShowNextPageButton="false"
							ShowPreviousPageButton="false" />
						</Fields>
					</asp:DataPager>
				</div>
			</div>	
	    </div>	
    </div>
</asp:Content>
