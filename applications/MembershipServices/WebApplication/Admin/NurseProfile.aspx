<%@ Page Language="C#" MasterPageFile="~/Nsnu.master" AutoEventWireup="True" Inherits="Admin_NurseProfile" Codebehind="NurseProfile.aspx.cs" %>

<asp:Content ID="cHeadContent" ContentPlaceHolderID="cphHeadContent" Runat="Server"> 
    
    <script type="text/javascript" src="../Scripts/jquery.qtip.min.js"></script>
    <link href="../Styles/jquery.qtip.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        var lastPrimaryFacilityRegionId = -1;
        var lastPrimaryFacilityDistrictId = -1;
        var lastPrimaryFacilitySelected;
        
        var buglmMembers = <%=BuglmMembers%>;
        var bursaryMembers = <%=BursaryMembers%>;
        var unionDisciplineMembers = <%=UnionDisciplineMembers%>;
        var unionDisciplineAppealMembers = <%=UnionDisciplineAppealMembers%>;
        var boardOfDirectorsMembers = <%=BoardOfDirectorsMembers%>;
        var negotiatingMembers = <%=NegotiatingMembers%>;
        var agmMembers = <%=AgmMembers%>;
        var constitutionMembers = <%=ConstitutionMembers%>;
        var educationMembers = <%=EducationMembers%>;
        var financeMembers = <%=FinanceMembers%>;
        var personnelMembers = <%=PersonnelMembers%>;
        
         var formErrors = "";
       
        $(document).ready(function () {
            formErrors = <%=FormErrors%>
			
            InitializeMultiSelects();
            InitializeAllCommitteeMembers();
            InitializeEventBindings();
            
            CheckMultiSelectDropdowns();
           
            GetFacility($("[id*=ddlFacility1]").val());
            lastPrimaryFacilitySelected = $("#ddlFacility1 option:selected");
            lastPrimaryFacilityRegionId = $("[id*=hdnCommitteeRegionId]").val();
            lastPrimaryFacilityDistrictId = $("[id*=hdnCommitteeDistrictId]").val();

            EvaluatePrimaryFacilityDdlActivation();
            EvaluateSecondaryFacilityDdlActivation();
            EvaluateTertiaryFacilityDdlActivation();
            EvaluatePositionDdlActivation();
            EvaluatePersonnelDdlActivation();
            
            GetNotes();
            GetVersionHistory();
            SetFieldsReadOnly();
            SetMembershipCredentialsVisibility();
            DisplayValidationErrors();
            
            $('[id*=hdnProfileChanged]').val("0");
        });
        
        function InitializeMultiSelects() {
            //$("#ddlLocalPosition1").multiselect({
            $("[id*=ddlLocalPosition]").multiselect({
                noneSelectedText: 'Please Select...',
                selectedList: 1,
                autoOpen: false
            });

            $("[id*=ddlTableOfficer]").multiselect({
                noneSelectedText: 'Please Select...',
                selectedList: 1,
                autoOpen: false
            });
            
            $("[id*=ddlBuglmPosition]").multiselect({
                noneSelectedText: 'Please Select...',
                selectedList: 1,
                autoOpen: false
            });
            
            $("[id*=ddlCommunicationOptions]").multiselect({
                noneSelectedText: 'Please Select...',
                selectedList: 4,
                autoOpen: false
            });
            
            $("a.ui-multiselect-all, a.ui-multiselect-none, a.ui-multiselect-close").addClass("suppressWarning");
        }

        function DisplayNavigationConfirmation() {
            var r = confirm("Fields have been updated without saving. Are you sure you would you like to proceed?");
            return r;
        }
       
        function InitializeAllCommitteeMembers()
        {
            //var just = GenerateCommitteeMemberTable(boardOfDirectorsMembers);
            $("#dvBuglmMembers").html(GenerateCommitteeMemberTable(buglmMembers));
            $("#dvBursaryMembers").html(GenerateCommitteeMemberTable(bursaryMembers));
            $("#dvUnionDisciplineMembers").html(GenerateCommitteeMemberTable(unionDisciplineMembers));
            $("#dvUnionDisciplineAppealMembers").html(GenerateCommitteeMemberTable(unionDisciplineAppealMembers));
            $("#dvBoardOfDirectorsMembers").html(GenerateCommitteeMemberTable(boardOfDirectorsMembers));
            $("#dvNegotiatingMembers").html(GenerateCommitteeMemberTable(negotiatingMembers));
            $("#dvAgmMembers").html(GenerateCommitteeMemberTable(agmMembers));
            $("#dvConstitutionMembers").html(GenerateCommitteeMemberTable(constitutionMembers));
            $("#dvEducationMembers").html(GenerateCommitteeMemberTable(educationMembers));
            $("#dvFinanceMembers").html(GenerateCommitteeMemberTable(financeMembers));
            $("#dvPersonnelMembers").html(GenerateCommitteeMemberTable(personnelMembers));
            
        }
        
        function GenerateCommitteeMemberTable(committee) {
            //alert(committee);
            var myTable = "<table>";
            
            if (committee.length > 0) {
                for (var q = 0; q < committee.length; q++) {
                    myTable += "<tr>";
                    myTable += "<td><b>" + committee[q].positionName + ":&nbsp;</b></td><td>" + committee[q].firstName + " " + committee[q].lastName + "</td>";
                    myTable += "</tr>";
                }
            }
            else {
                myTable += "<tr><td>No Members Assigned</td></tr>";
            }
            
            
            myTable += "</table>";
            //alert(myTable);
            return myTable;
        }
        
        function EvaluatePositionDdlActivation ()
        {
            //nurse is active
            if ($('[id*=ddlStatus]').val() != "2" && $('[id*=ddlStatus]').val() != "4") {
                if ($('[id*=ddlFacility1]').val() != "") {
                    //$('[id*=ddlBuglmPosition]').multiselect("enable");
                    $('#cphMainContent_ddlBuglmPosition').multiselect("enable");
                    $('[id*=ddlBursaryPosition]').prop("disabled", false);
                    $('[id*=ddlUnionDisciplinePosition]').prop("disabled", false);
                    $('[id*=ddlUnionDisciplineAppealPosition]').prop("disabled", false);

                    $('#cphMainContent_ddlLocalPosition1').multiselect("enable");
                    $('#cphMainContent_ddlTableOfficer1').multiselect("enable");
                }
                else {
                    $('[id*=ddlBuglmPosition]').val("");
                    $('#cphMainContent_ddlBuglmPosition').multiselect("disable");

                    $('[id*=ddlBursaryPosition]').val("");
                    $('[id*=ddlBursaryPosition]').prop("disabled", true);

                    $('[id*=ddlUnionDisciplinePosition]').val("");
                    $('[id*=ddlUnionDisciplinePosition]').prop("disabled", true);

                    $('[id*=ddlUnionDisciplineAppealPosition]').val("");
                    $('[id*=ddlUnionDisciplineAppealPosition]').prop("disabled", true);

                    $('#cphMainContent_ddlLocalPosition1').multiselect("disable");
                    $('#cphMainContent_ddlTableOfficer1').multiselect("disable");

                    // clear the region divs
                    $('[id*=dvCommitteeDistrict]').html("&nbsp;");
                    $('[id*=dvCommitteeRegion]').html("&nbsp;");
                }

                if ($('[id*=ddlFacility2]').val() != "") {
                    $('#cphMainContent_ddlLocalPosition2').multiselect("enable");
                    $('#cphMainContent_ddlTableOfficer3').multiselect("enable");
                }

                if ($('[id*=ddlFacility3]').val() != "") {
                    $('#cphMainContent_ddlLocalPosition3').multiselect("enable");
                    $('#cphMainContent_ddlTableOfficer3').multiselect("enable");
                }

                $('[id*=ddlBoardOfDirectorsPosition]').prop("disabled", false);
                
                $('[id*=ddlNegotiatingPosition]').prop("disabled", false);
                
                $('[id*=ddlAgmPosition]').prop("disabled", false);
                
                $('[id*=ddlConstitutionPosition]').prop("disabled", false);
                
                $('[id*=ddlEducationPosition]').prop("disabled", false);
                
                $('[id*=ddlFinancePosition]').prop("disabled", false);
                
                $('[id*=ddlPersonnelPosition]').prop("disabled", false);
                
            }
            //nurse is inactive or retired
            else {
                $('#cphMainContent_ddlBuglmPosition').multiselect("uncheckAll");
                $('#cphMainContent_ddlBuglmPosition').multiselect("disable");

                $('[id*=ddlBursaryPosition]').val("");
                $('[id*=ddlBursaryPosition]').prop("disabled", true);

                $('[id*=ddlUnionDisciplinePosition]').val("");
                $('[id*=ddlUnionDisciplinePosition]').prop("disabled", true);

                $('[id*=ddlUnionDisciplineAppealPosition]').val("");
                $('[id*=ddlUnionDisciplineAppealPosition]').prop("disabled", true);

                $('#cphMainContent_ddlLocalPosition1').multiselect("uncheckAll");
                $('#cphMainContent_ddlLocalPosition1').multiselect("disable");
                
                $('#cphMainContent_ddlTableOfficer1').multiselect("uncheckAll");
                $('#cphMainContent_ddlTableOfficer1').multiselect("disable");
                
                $('#cphMainContent_ddlLocalPosition2').multiselect("uncheckAll");
                $('#cphMainContent_ddlLocalPosition2').multiselect("disable");
                
                $('#cphMainContent_ddlTableOfficer2').multiselect("uncheckAll");
                $('#cphMainContent_ddlTableOfficer2').multiselect("disable");
                
                $('#cphMainContent_ddlLocalPosition3').multiselect("uncheckAll");
                $('#cphMainContent_ddlLocalPosition3').multiselect("disable");
                
                $('#cphMainContent_ddlTableOfficer3').multiselect("uncheckAll");
                $('#cphMainContent_ddlTableOfficer3').multiselect("disable");
                
                $('[id*=ddlBoardOfDirectorsPosition]').val("");
                $('[id*=ddlBoardOfDirectorsPosition]').prop("disabled", true);
                
                $('[id*=ddlNegotiatingPosition]').val("");
                $('[id*=ddlNegotiatingPosition]').prop("disabled", true);
                
                $('[id*=ddlAgmPosition]').val("");
                $('[id*=ddlAgmPosition]').prop("disabled", true);
                
                $('[id*=ddlConstitutionPosition]').val("");
                $('[id*=ddlConstitutionPosition]').prop("disabled", true);
                
                $('[id*=ddlEducationPosition]').val("");
                $('[id*=ddlEducationPosition]').prop("disabled", true);
                
                $('[id*=ddlFinancePosition]').val("");
                $('[id*=ddlFinancePosition]').prop("disabled", true);
                
                $('[id*=ddlPersonnelPosition]').val("");
                $('[id*=ddlPersonnelPosition]').prop("disabled", true);
            } 
        }

        function EvaluatePrimaryFacilityDdlActivation()
        {
            if ($('[id*=ddlFacility1]').val() != "" ) {
                //$('[id*=ddlLocalPosition1]').multiselect("enable");
                $('#cphMainContent_ddlLocalPosition1').multiselect("enable");
                $('#cphMainContent_ddlTableOfficer1').multiselect("enable");
            }
            else{
                $('#cphMainContent_ddlLocalPosition1').multiselect("disable");
                $('#cphMainContent_ddlTableOfficer1').multiselect("disable");
            }
        }

        function EvaluateSecondaryFacilityDdlActivation()
        {
            if ($('[id*=ddlFacility2]').val() != "" ) {
                //$('[id*=ddlLocalPosition1]').multiselect("enable");
                $('#cphMainContent_ddlLocalPosition2').multiselect("enable");
                $('#cphMainContent_ddlTableOfficer2').multiselect("enable");
            }
            else{
                $('#cphMainContent_ddlLocalPosition2').multiselect("disable");
                $('#cphMainContent_ddlTableOfficer2').multiselect("disable");
            }
        }

        function EvaluateTertiaryFacilityDdlActivation()
        {
            if ($('[id*=ddlFacility3]').val() != "" ) {
                //$('[id*=ddlLocalPosition1]').multiselect("enable");
                $('#cphMainContent_ddlLocalPosition3').multiselect("enable");
                $('#cphMainContent_ddlTableOfficer3').multiselect("enable");
            }
            else{
                $('#cphMainContent_ddlLocalPosition3').multiselect("disable");
                $('#cphMainContent_ddlTableOfficer3').multiselect("disable");
            }
        }


        
        function EvaluatePersonnelDdlActivation () {
            
            // personnel committee member must be either on the board of directors, neg committee pres or vice pres, or chair of bursary
            if ($('[id*=ddlBoardOfDirectorsPosition]').val() != "" || 
                $('[id*=ddlNegotiatingPosition]').val() == "1" ||
                $('[id*=ddlNegotiatingPosition]').val() == "4" ||
                $('[id*=ddlBursaryPosition]').val() == "2"
                ) {
                
                $('[id*=ddlPersonnelPosition]').prop("disabled", false);
            }
            else {
                if ($('[id*=ddlPersonnelPosition]').val() != "") {
                    if (!confirm("Personnel Committee positions may only be held by members of the Board of Directors, the currently selected Personnel position will be deselected. Would you like to proceed?")) {
                        $('[id*=ddlNegotiatingPosition]').val($('[id*=hdnOrigNegotiatingPosition]').val());
                        $('[id*=ddlBursaryPosition]').val($('[id*=hdnOrigBursaryPosition]').val());
                        $('[id*=ddlBoardOfDirectorsPosition]').val($('[id*=hdnOrigBoardOfDirectorsPosition]').val());
                        return false;                        
                    }
                }
                $('[id*=ddlPersonnelPosition]').val("");
                $('[id*=ddlPersonnelPosition]').prop("disabled", true);
            }

            $('[id*=hdnOrigNegotiatingPosition]').val($('[id*=ddlNegotiatingPosition]').val());
            $('[id*=hdnOrigBursaryPosition]').val($('[id*=ddlBursaryPosition]').val());
            $('[id*=hdnOrigBoardOfDirectorsPosition]').val($('[id*=ddlBoardOfDirectorsPosition]').val());
            
            return true;
        }


        function DisplayValidationErrors() {
        // var formErrors contains a JSON array of error messages
        // hasOwnProperty used to test JSON object for length;
            var displayCommitteeSection = false;
            var displayCredentialsSection = false;
            var displayGlobalError = false;
            
            $('[id*=dvGlobalError]').hide();
            
            for(var key in formErrors)
            {
                displayGlobalError = true;
                if(formErrors.hasOwnProperty(key))
                {
                    var message = formErrors[key][0];

                    // key is the id of the form element that failed validation
                    var formField = $('[id*=' + key + ']');
                    formField.addClass("invalid").after('<p class="error">'+ message +'</p>');

                    if ($('[id*=dvCommitteeData]').find($('[id*=' + key + ']')).length > 0) {
                        displayCommitteeSection = true;        
                    }
                    
                    if ($('[id*=dvCredentialsSection]').find($('[id*=' + key + ']')).length > 0) {
                        displayCredentialsSection = true;        
                    }
                }
            }
            
            if (displayCommitteeSection) {
                $('[id*=tglCommitteeSection]').click();
            }
            
            if (displayCredentialsSection) {
                $('[id*=tglCredentialsSection]').click();
            }
            
            if (displayGlobalError) {
                $('[id*=dvGlobalError]').show();
            }
        }


        // set all form fields to read only, this will disable all buttons as well
        function SetFieldsReadOnly() {
            var readonly = $('[id*=hdnReadOnlyUser]').val();

            if (readonly == "false") {
                return;
            }
    
            //this makes all field readonly and sets the background of the field grey
            $('#formElements :input').attr("disabled", true);

            return;
        }

        // on the add Nurse screen hide the username, password, and confirm password fields
        function SetMembershipCredentialsVisibility() {
            var nurseId = $('[id*=hdnNurseId]').val();

            if (nurseId == "") {
                $('#creds').hide();
            }
            else {
                $('#creds').show();
            }

            return;
        }


        function GetVersionHistory() {

            var nurseId = $('[id*=hdnNurseId]').val();

            if (nurseId == "") {
                return;
            }

            $.ajax({
                type: "POST",
                url: "NurseProfile.aspx/GetVersionHistory",
                data: '{"nurseId":"' + nurseId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    // Replace the div's content with the page method's return.
                    var x = msg.d;

                    var myLength = x.length;

                    if (myLength > 0) {
                        var myTable = '<div class="tbl_data_notes">';

                        for (var q = 0; q < myLength; q++) {
                            myTable += "<div class='note-item'><h5><strong>" + x[q].modifiedBy + "</strong> | " + x[q].modificationDate + " | <a target='_blank' href='ViewVersion.aspx?id=" + x[q].versionId + "'>View version</a></h5></div>";
                        }

                        myTable += "</div>";

                        $("#dvVersionHistoryTable").html(myTable);
                    }
                    else {
                        $("#dvVersionHistoryTable").html("No version history.");
                    }


                }
            });
        }
        
        function GetNotes() {

            var nurseId = $('[id*=hdnNurseId]').val();
            var readonly = $('[id*=hdnReadOnlyUser]').val();

            if (nurseId == "") {
                return;
            }
            
            $.ajax({
                type: "POST",
                url: "NurseProfile.aspx/GetNotes",
                data: '{"nurseId":"' + nurseId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    // Replace the div's content with the page method's return.
                    var x = msg.d;  

                    var myLength = x.length;

                    if (myLength > 0) {
                        var myTable = '<div class="tbl_data_notes">';
                    
                        for (var q = 0; q < myLength; q++) {
                            myTable += "<div class='note-item'><h5><strong>" + x[q].createdBy + "</strong> | " + x[q].creationDate + " | <a id='lnkDeleteNote' noteId='" + x[q].noteId + "'>Delete</a></h5><p>" + x[q].noteBody + "</p></div>";
                        }

                        myTable += "</div>";

                        $("#dvNoteTable").html(myTable);

                        if (readonly == "true")
                        {
                            $("[id*=lnkDeleteNote]").hide();
                        }

                    }
                    else {
                        $("#dvNoteTable").html("<p>No notes have been created.</p>");
                    }


                }
            });
        }

        function AddNote() {
            var nurseId = $('[id*=hdnNurseId]').val();
            var noteBody = $('[id*=tbNote]').val();

            noteBody = noteBody.replace( /\\/g , '\\\\');
            noteBody = noteBody.replace( /\"/g , '\\"');

            if (nurseId == "" || noteBody == "") {
                return;
            }

            var dataString = '{"nurseId":"' + nurseId + '", "noteBody":"' + noteBody + '"}';

            $.ajax({
                type: "POST",
                url: "NurseProfile.aspx/AddNote",
                data: dataString,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    GetNotes();
                    ClearNoteForm();
                },
                error: function (xhr, status, error) {
                    // Display a generic error for now.
                    alert("AJAX Error in Add Note!");
                }
            });
        }

        function DeleteNote(noteId) {

            $.ajax({
                type: "POST",
                url: "NurseProfile.aspx/DeleteNote",
                data: '{"noteId":"' + noteId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    GetNotes();
                },
                error: function (xhr, status, error) {
                    // Display a generic error for now.
                    alert("AJAX Error in Delete Note!");
                }
            });
        }

        function ManageSecondaryFacility(facilityId)
        {
            var holdsFacilityPosition = false;
            if ($('[id*=ddlFacility2]').val() != $('[id*=hdnOrigSecondaryFacilityId]').val() && ($('[id*=hdnLocalPosition2]').val() != "" || $('[id*=hdnTableOfficer2]').val() != "")) {
                // local and table officer positions will be lost for secondary facility
                holdsFacilityPosition = true;
            }

            if (holdsFacilityPosition) {
                var confirmWarning = "Changing of the facility will result in the loss of any Local or Table Officer positions held. Would you like to proceed?";

                if (!confirm(confirmWarning)) {
                    $('[id*=ddlFacility2]').val($('[id*=hdnOrigSecondaryFacilityId]').val());
                    return false;
                }


                if (holdsFacilityPosition) {
                    $('#cphMainContent_ddlLocalPosition2').multiselect("uncheckAll");
                    $('[id*=hdnLocalPosition2]').val("");

                    $('#cphMainContent_ddlTableOfficer2').multiselect("uncheckAll");
                    $('[id*=hdnTableOfficer2]').val("");
                }

            }

            $('[id*=hdnOrigSecondaryFacilityId]').val(facilityId);

        }


        function ManageTertiaryFacility(facilityId)
        {
            var holdsFacilityPosition = false;
            if ($('[id*=ddlFacility3]').val() != $('[id*=hdnOrigTertiaryFacilityId]').val() && ($('[id*=hdnLocalPosition3]').val() != "" || $('[id*=hdnTableOfficer3]').val() != "")) {
                // local and table officer positions will be lost for tertiary facility
                holdsFacilityPosition = true;
            }

            if (holdsFacilityPosition) {
                var confirmWarning = "Changing of the facility will result in the loss of any Local or Table Officer positions held. Would you like to proceed?";

                if (!confirm(confirmWarning)) {
                    $('[id*=ddlFacility3]').val($('[id*=hdnOrigTertiaryFacilityId]').val());
                    return false;
                }


                if (holdsFacilityPosition) {
                    $('#cphMainContent_ddlLocalPosition3').multiselect("uncheckAll");
                    $('[id*=hdnLocalPosition3]').val("");

                    $('#cphMainContent_ddlTableOfficer3').multiselect("uncheckAll");
                    $('[id*=hdnTableOfficer3]').val("");
                }
            }

            $('[id*=hdnOrigTertiaryFacilityId]').val(facilityId);

        }



        function GetFacility(facilityId) {

            var holdsRegionPosition = false;
            var holdsDistrictPosition = false;
            var holdsFacilityPosition = false;
            var holdsVpIwkPosition = false;

            if (facilityId == "") {
                return;
            }
            else if (facilityId == "1000") {
            // means the nurse had a primary facility but primary facility was reset to default "Please Select" state
    
                if ($('[id*=ddlFacility1]').val() != $('[id*=hdnOrigPrimaryFacilityId]').val() && ($('[id*=hdnLocalPosition1]').val() != "" || $('[id*=hdnTableOfficer1]').val() != "")) {
                    // local and table officer positions will be lost
                    holdsFacilityPosition = true;
                }

                if ($('[id*=hdnCommitteeRegionId]').val() != "" && ($('[id*=ddlBursaryPosition]').val() != "" || $('[id*=ddlUnionDisciplinePosition]').val() != "" || $('[id*=ddlUnionDisciplineAppealPosition]').val() != "")) {
                    holdsRegionPosition = true;
                }

                if ($('[id*=hdnCommitteeDistrictId]').val() != "" && ($('[id*=ddlBuglmPosition]').val() != null)) {
                    holdsDistrictPosition = true;
                }
                
                if ($('[id*=ddlBoardOfDirectorsPosition]').val() ==  20) {
                    holdsVpIwkPosition = true;
                }

                if (holdsDistrictPosition || holdsFacilityPosition || holdsRegionPosition || holdsVpIwkPosition) {
                    var confirmWarning = "Removal of the primary facility will result in the removal of any Local or Table Officer positions held, and any District or Region or Facility based Committee positions held by the Nurse (respectively). Would you like to proceed?";

                    if (!confirm(confirmWarning)) {
                        $('[id*=ddlFacility1]').val($('[id*=hdnOrigPrimaryFacilityId]').val());
                        return false;
                    }

                    if (holdsDistrictPosition) {
                        $('#cphMainContent_ddlBuglmPosition').multiselect("uncheckAll");
                        $('[id*=hdnBuglmPosition]').val("");
                    }

                    if (holdsFacilityPosition) {
                        $('#cphMainContent_ddlLocalPosition1').multiselect("uncheckAll");
                        $('[id*=hdnLocalPosition1]').val("");

                        $('#cphMainContent_ddlTableOfficer1').multiselect("uncheckAll");
                        $('[id*=hdnTableOfficer1]').val("");
                    }

                    if (holdsRegionPosition) {
                        $('[id*=ddlBursaryPosition]').val("");
                        $('[id*=ddlUnionDisciplinePosition]').val("");
                        $('[id*=ddlUnionDisciplineAppealPosition]').val("");
                    }
                    
                    if (holdsVpIwkPosition) {
                        $('[id*=ddlBoardOfDirectorsPosition]').val("");
                    }
                }

                $('[id*=hdnOrigPrimaryFacilityId]').val("");

                $('[id*=dvCommitteeDistrict]').html("");
                $('[id*=dvCommitteeRegion]').html("");

                $('#hdnCommitteeRegionId').html("");
                $('#hdnCommitteeDistrictId').html("");


            }
            else
            {
                $.ajax({
                    type: "POST",
                    url: "NurseProfile.aspx/GetFacility",
                    data: '{"facilityId":"' + facilityId + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var x = msg.d;
                        
                        if ($('[id*=ddlFacility1]').val() != $('[id*=hdnOrigPrimaryFacilityId]').val() && ($('[id*=hdnLocalPosition1]').val() != "" || $('[id*=hdnTableOfficer1]').val() != "")) {
                            // local and table officer positions will be lost
                            holdsFacilityPosition = true;
                        }

                        if ($('[id*=hdnCommitteeRegionId]').val() != "" && x.regionId != $('[id*=hdnCommitteeRegionId]').val() && ($('[id*=ddlBursaryPosition]').val() != "" || $('[id*=ddlUnionDisciplinePosition]').val() != "" || $('[id*=ddlUnionDisciplineAppealPosition]').val() != "")) {
                            holdsRegionPosition = true;
                        }

                        if ($('[id*=hdnCommitteeDistrictId]').val() != "" && x.districtId != $('[id*=hdnCommitteeDistrictId]').val() && ($('[id*=ddlBuglmPosition]').val() != null)) {
                           // alert( $('[id*=hdnCommitteeDistrictId]').val() + ":::" + x.districtId + $('[id*=ddlBuglmPosition]').val() );
                            holdsDistrictPosition = true;
                        }
                        
                        if ($('[id*=ddlFacility1]').val() != $('[id*=hdnOrigPrimaryFacilityId]').val() && $('[id*=ddlFacility1]').val() != 46 && $('[id*=ddlBoardOfDirectorsPosition]').val() == 20) {
                            holdsVpIwkPosition = true;
                        }
                        
                      //  alert( holdsFacilityPosition + ":::" + holdsRegionPosition + ":::" + holdsDistrictPosition + ":::" + holdsVpIwkPosition);
                        
                        if (holdsDistrictPosition || holdsFacilityPosition || holdsRegionPosition || holdsVpIwkPosition) {
                            var confirmWarning = "Changing of the primary facility will result in the loss of any Local or Table Officer positions held, and if the new primary facility's district or region has changed, any District based Committee, or Region based Committee positions held by the Nurse (respectively). Would you like to proceed?";

                            if (!confirm(confirmWarning)) {
                                $('[id*=ddlFacility1]').val($('[id*=hdnOrigPrimaryFacilityId]').val());
                                return false;
                            }

                            if (holdsDistrictPosition) {
                                $('#cphMainContent_ddlBuglmPosition').multiselect("uncheckAll");
                                $('[id*=hdnBuglmPosition]').val("");
                            }

                            if (holdsFacilityPosition) {
                                $('#cphMainContent_ddlLocalPosition1').multiselect("uncheckAll");
                                $('[id*=hdnLocalPosition1]').val("");

                                $('#cphMainContent_ddlTableOfficer1').multiselect("uncheckAll");
                                $('[id*=hdnTableOfficer1]').val("");
                            }

                            if (holdsRegionPosition) {
                                $('[id*=ddlBursaryPosition]').val("");
                                $('[id*=ddlUnionDisciplinePosition]').val("");
                                $('[id*=ddlUnionDisciplineAppealPosition]').val("");
                            }
                            
                            if (holdsVpIwkPosition) {
                                $('[id*=ddlBoardOfDirectorsPosition]').val("");
                            }
                        }

                        $('[id*=hdnOrigPrimaryFacilityId]').val(facilityId);

                        $('[id*=dvCommitteeDistrict]').html(x.districtName);
                        $('[id*=dvCommitteeRegion]').html(x.regionName);

                        $('#hdnCommitteeRegionId').html(x.regionId);
                        $('#hdnCommitteeDistrictId').html(x.districtId);
                    },
                    error: function (xhr, status, error) {
                        // Display a generic error for now.
                        alert("AJAX Error in Get Facility!");
                    }
                });
           }

        }
        
        function ClearNoteForm() {
            $('[id*=tbNote]').val('');
        }
        
        function CheckMultiSelectDropdowns() {
            
            $.each($("[id*=hdnLocalPosition1]").val().split(','), function (idx, val) {
                $("[id*=ddlLocalPosition1]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                    this.click();
                });
            });

            $.each($("[id*=hdnLocalPosition2]").val().split(','), function (idx, val) {
                $("[id*=ddlLocalPosition2]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                    this.click();
                });
            });

            $.each($("[id*=hdnLocalPosition3]").val().split(','), function (idx, val) {
                $("[id*=ddlLocalPosition3]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                    this.click();
                });
            });

            $.each($("[id*=hdnTableOfficer1]").val().split(','), function (idx, val) {
                $("[id*=ddlTableOfficer1]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                    this.click();
                });
            });

            $.each($("[id*=hdnTableOfficer2]").val().split(','), function (idx, val) {
                $("[id*=ddlTableOfficer2]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                    this.click();
                });
            });

            $.each($("[id*=hdnTableOfficer3]").val().split(','), function (idx, val) {
                $("[id*=ddlTableOfficer3]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                    this.click();
                });
            });

            $.each($("[id*=hdnBuglmPosition]").val().split(','), function (idx, val) {
                $("[id*=ddlBuglmPosition]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                    this.click();
                });
            });

            $.each($("[id*=hdnCommunicationOptions]").val().split(','), function (idx, val) {
                $("[id*=ddlCommunicationOptions]").multiselect("widget").find(":checkbox[value='" + val + "']").filter(':not(:checked)').each(function () {
                    this.click();
                });
            });

            
        }


        function InitializeEventBindings() {
            
            $(".toggler").click(function() {
				$(this).toggleClass("closed").next(".collapsible").slideToggle("fast");
			}); //.next().hide();
            
            $("a").not('.suppressWarning').click(function () {
                if ($('[id*=hdnProfileChanged]').val() == "1") {
                      return DisplayNavigationConfirmation();
                }
                return true;
            });
            
            $('[id*=btnNextNurse],[id*=btnPrevNurse]').click(function () {
                if ($('[id*=hdnProfileChanged]').val() == "1") {
                    return DisplayNavigationConfirmation();
                }
                return true;
            });
            
            $('input[type=text]').on('input', function() {
                $('[id*=hdnProfileChanged]').val("1");
            });
            
            $('select').change(function() {
                $('[id*=hdnProfileChanged]').val("1");
            });

            $("[id*=btnSubmit]").click(function () {

                $("[id*=hdnLocalPosition1]").val($("[id*=ddlLocalPosition1]").val());
                $("[id*=hdnLocalPosition2]").val($("[id*=ddlLocalPosition2]").val());
                $("[id*=hdnLocalPosition3]").val($("[id*=ddlLocalPosition3]").val());
                
                $("[id*=hdnTableOfficer1]").val($("[id*=ddlTableOfficer1]").val());
                $("[id*=hdnTableOfficer2]").val($("[id*=ddlTableOfficer2]").val());
                $("[id*=hdnTableOfficer3]").val($("[id*=ddlTableOfficer3]").val());

                $("[id*=hdnBuglmPosition]").val($("[id*=ddlBuglmPosition]").val());
                $("[id*=hdnCommunicationOptions]").val($("[id*=CommunicationOptions]").val());

               
                return true;
            });
            
            $("[id*=tdBuglm] span").qtip({
                content: $("[id*=dvBuglmMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdBursary] span").qtip({
                content: $("[id*=dvBursaryMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdUnionDiscipline] span").qtip({
                content: $("[id*=dvUnionDiscipline]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdUnionDisciplineAppeal] span").qtip({
                content: $("[id*=dvUnionDisciplineAppealMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdBoardOfDirectors] span").qtip({
                content: $("[id*=dvBoardOfDirectorsMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdNegotiating] span").qtip({
                content: $("[id*=dvNegotiatingMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdAgm] span").qtip({
                content: $("[id*=dvAgmMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdConstitution] span").qtip({
                content: $("[id*=dvConstitutionMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdFinance] span").qtip({
                content: $("[id*=dvFinanceMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdEducation] span").qtip({
                content: $("[id*=dvEducationMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });
            
            $("[id*=tdPersonnel] span").qtip({
                content: $("[id*=dvPersonnelMembers]").html(),
                position: { 
                    my: "right center",
                    at: "left center"
                }
            });

            $("[id*=ddlFacility1]").change(function () {
                if ($("[id*=ddlFacility1]").val() != "") {
                    GetFacility($("[id*=ddlFacility1]").val());
                }
                else{
                // handles the case where "Please Select" is chosen, effectively removing the nurses Primary facility
                    GetFacility("1000");
                }

                EvaluatePositionDdlActivation();
                EvaluatePrimaryFacilityDdlActivation();
            });

            $("[id*=ddlFacility2]").change(function () {

                ManageSecondaryFacility($("[id*=ddlFacility2]").val());
                EvaluateSecondaryFacilityDdlActivation();
            });

            $("[id*=ddlFacility3]").change(function () {
                ManageTertiaryFacility($("[id*=ddlFacility3]").val());
                EvaluateTertiaryFacilityDdlActivation();
            });


            $("[id*=ddlBoardOfDirectors]").change(function () {
                EvaluatePersonnelDdlActivation();
            });

            $("[id*=ddlBursaryPosition]").change(function () {
                EvaluatePersonnelDdlActivation();
            });

            $("[id*=ddlNegotiatingPosition]").change(function () {
                EvaluatePersonnelDdlActivation();
            });

            $("[id*=btnAddNote]").click(function () {
                AddNote();
            });

            $("[id*=dvNoteTable]").on('click', "[id*=lnkDeleteNote]", function (e) {
                var id = $(e.target).attr("noteId");

                DeleteNote(id);
                return false;
            });
            
            $("[id*=tbBirthDate]").datepicker({
                dateFormat: 'mm-dd-yy',
                changeMonth: true,
                changeYear: true,
                yearRange: "-100:+0"
            });

            $(".phoneField").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                    else {
                        var value = this.value;
                        if (value.length == 3 || value.length == 7) {
                            this.value = value + '-';
                        }

                        return true;
                    }
                }
            });

            $(".numbersOnly").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });

            $(".lettersOnly").keydown(function (event) {

     
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is not a number and stop the keypress
                  if ((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode > 95 && event.keyCode < 106)) {
                          event.preventDefault();
                   }
                }
            });

            // force all characters to upper case in the postal code field
            $("[id*=tbPostalCode]").keyup(function(){
                    this.value = this.value.toUpperCase();
            });


            $("[id*=hdnOrigStatus]").val($("[id*=ddlStatus]").val());
            
            $("[id*=ddlStatus]").change(function () {
                
                var selected = $(this).val();
                
                //inactive or retired
                if ((selected == '2' || selected == '4') && ($("[id*=hdnOrigStatus]").val() != '2' && $("[id*=hdnOrigStatus]").val() != '4')) {
                    if (!confirm("Changing a nurse's status to inactive or retired will result in the relinquishing of any local, table officer, or committee positions held. In addition, the nurse will no longer have access to the NSNU extranet. Would you like to proceed?")) {
                        $(this).val($.data(this, 'current')); // added parenthesis (edit)
                        $("[id*=ddlStatus]").val($("[id*=hdnOrigStatus]").val());
                        return false;
                    }     
                }

                $("[id*=hdnOrigStatus]").val($("[id*=ddlStatus]").val());
                
                EvaluatePositionDdlActivation();
            });
            
        }
        
    </script>
</asp:Content>

<asp:Content ID="cMainContent" ContentPlaceHolderID="cphMainContent" Runat="Server"> 

    <div class="section page-heading row clearfix">
	    <div class="grid_12">
			<a id="lnkBackToList" class="red-btn float-right" href="index.aspx">Back to List</a>
		    <h1><asp:Literal runat="server" ID="litHeading" /></h1>
		</div>
    </div>


	<div class="section page-heading row clearfix">
	    <div class="grid_12">
			<div id="dvPagingWidget" runat="server" class="edit_paging clearfix">
				<asp:Button id="btnPrevNurse" runat="server" CssClass="frm-btn float-left" Text="&laquo; Prev" OnClick="btnPrevNurse_OnClick" />
				<asp:Button id="btnNextNurse" runat="server" CssClass="frm-btn float-right" Text="Next &raquo;" OnClick="btnNextNurse_OnClick" />
			</div>
		</div>
	</div>

	<div id="formElements">

    <div class="section row clearfix">			
	    <div class="main grid_12">
			<p><small> <span class="required">&bull;</span> required field</small></p>
            <div id="dvGlobalError">There are validation errors, please review.</div>
			
			<asp:HiddenField ID="hdnNurseId" runat="server" />
			<asp:HiddenField ID="hdnCommitteeDistrictId" runat="server" />
			<asp:HiddenField ID="hdnCommitteeRegionId" runat="server" />
			<asp:HiddenField ID="hdnPhone1Id" runat="server" />
			<asp:HiddenField ID="hdnPhone2Id" runat="server" />
			<asp:HiddenField ID="hdnPhone3Id" runat="server" />
			<asp:HiddenField ID="hdnPhone4Id" runat="server" />
			<asp:HiddenField ID="hdnReadOnlyUser" runat="server" />
            <asp:HiddenField ID="hdnOrigStatus" runat="server" />
            <asp:HiddenField ID="hdnProfileChanged" runat="server" />

			<h3>Personal Data</h3>
			<div class="wrapper-block frm-section clearfix">
				<div class="row">
					<div class="grid_3"><label>First Name <span class="required">&bull;</span></label> <asp:TextBox ID="tbFirstName" runat="server" MaxLength="50" class="lettersOnly"></asp:TextBox></div>
					<div class="grid_3"><label>Middle Initial</label> <asp:TextBox ID="tbMiddleInitial" runat="server" MaxLength="1" class="lettersOnly"></asp:TextBox></div>
					<div class="grid_3"><label>Last Name <span class="required">&bull;</span></label> <asp:TextBox ID="tbLastName" runat="server" MaxLength="50" class="lettersOnly"></asp:TextBox></div>
					<div class="grid_3"><label>Gender</label> <asp:DropDownList ID="ddlGender" runat="server"></asp:DropDownList></div>
				</div>
				<div class="row">
					<div class="grid_3"><label>Nickname</label> <asp:TextBox ID="tbNickname" runat="server" MaxLength="100"></asp:TextBox></div>
					<div class="grid_3"><label>Date of Birth</label> <asp:TextBox ID="tbBirthDate" runat="server"></asp:TextBox></div>
				</div>
			</div>
			
			<div class="wrapper-block frm-section clearfix">
				<div class="row">
					<div class="grid_3"><label>Address Line 1</label> <asp:TextBox ID="tbAddressLine1" runat="server" MaxLength="100"></asp:TextBox></div>
					<div class="grid_3"><label>Address Line 2</label> <asp:TextBox ID="tbAddressLine2" runat="server" MaxLength="100"></asp:TextBox></div>
					<div class="grid_3"><label>City</label>  <asp:TextBox ID="tbCity" runat="server" MaxLength="100" class="lettersOnly"></asp:TextBox></div>
					<div class="grid_3"><label>Province</label> <asp:DropDownList ID="ddlProvince" runat="server">
                        <asp:ListItem Value="">Please Select</asp:ListItem>
						<asp:ListItem value='AB'>Alberta</asp:ListItem>
						<asp:ListItem value='BC'>British Columbia</asp:ListItem>
						<asp:ListItem value='MB'>Manitoba</asp:ListItem>
						<asp:ListItem value='NB'>New Brunswick</asp:ListItem>
						<asp:ListItem value='NL'>Newfoundland and Labrador</asp:ListItem>
						<asp:ListItem value='NT'>Northwest Territories</asp:ListItem>
						<asp:ListItem value='NS'>Nova Scotia</asp:ListItem>
						<asp:ListItem value='NU'>Nunavut</asp:ListItem>
						<asp:ListItem value='PE'>Prince Edward Island</asp:ListItem>
						<asp:ListItem value='SK'>Saskatchewan</asp:ListItem>
						<asp:ListItem value='ON'>Ontario</asp:ListItem>
						<asp:ListItem value='QC'>Quebec</asp:ListItem>
						<asp:ListItem value='YT'>Yukon</asp:ListItem>
						</asp:DropDownList></div>
				</div>
				<div class="row">
					<div class="grid_3"><label>Postal Code</label> <asp:TextBox ID="tbPostalCode" runat="server" MaxLength="7"></asp:TextBox></div>
					<div class="grid_3"><label>Email address</label> <asp:TextBox ID="tbEmail" runat="server" MaxLength="100"></asp:TextBox></div>
					<div class="grid_3"><label>Secondary Email address</label> <asp:TextBox ID="tbSecondaryEmail" runat="server" MaxLength="100"></asp:TextBox></div>
				</div>
			</div>
			
			<div class="wrapper-block frm-section clearfix">
				<div class="row">
					<div class="grid_3"><label>Home/Personal</label> <asp:TextBox ID="tbPhoneNumber1" runat="server" class="phoneField" MaxLength="12"></asp:TextBox></div>
                    <div class="grid_3"><label>Mobile</label> <asp:TextBox ID="tbPhoneNumber3" runat="server" class="phoneField" MaxLength="12"></asp:TextBox></div>
                    <div class="grid_3"><label>Fax</label> <asp:TextBox ID="tbPhoneNumber4" runat="server" class="phoneField" MaxLength="12"></asp:TextBox></div>
					<div class="grid_3">&nbsp;</div>
                </div>
				<div class="row">
					<div class="grid_3"><label>Work</label> <asp:TextBox ID="tbPhoneNumber2" runat="server" class="phoneField" MaxLength="12"></asp:TextBox></div>
					<div class="grid_3"><label>Phone Ext.</label> <asp:TextBox ID="tbPhoneExt2" runat="server" class="numbersOnly" MaxLength="6"></asp:TextBox></div>
                    <div class="grid_6">&nbsp;</div>
				</div>
			</div>

			<div class="wrapper-block frm-section clearfix">
				<div class="row">
					<div class="grid_12"><label>Communication Options</label>
                        <asp:DropDownList ID="ddlCommunicationOptions" multiple="multiple" runat="server"></asp:DropDownList>
                    </div>
				</div>
			</div>
		</div>
	</div>
	
	<div class="section row clearfix">			
	    <div class="main grid_12">
			<h3>Employment Data</h3>
			<div class="wrapper-block frm-section clearfix">
				<div class="row">
					<div class="grid_3"><label>Designation</label><asp:DropDownList ID="ddlDesignation" runat="server"></asp:DropDownList></div>
					<div class="grid_3"><label>Status <span class="required">&bull;</span></label><asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList></div>
					<div class="grid_6">
						<label>Membership</label>
						<asp:CheckBox ID="cbMembershipFormCompleted" runat="server" /><small>Completed membership form</small> &nbsp;&nbsp; <asp:CheckBox ID="cbMembershipCardIssued" runat="server" /><small>Issued member card</small>
					</div>
				</div>
			</div>
			
			<div class="wrapper-block frm-section clearfix">
				<div id="dvEmploymentData">
					<asp:HiddenField id="hdnOrigBoardOfDirectorsPosition" runat="server" />
					<asp:HiddenField id="hdnOrigNegotiatingPosition" runat="server" />
					<asp:HiddenField id="hdnOrigBursaryPosition" runat="server" />
					<asp:HiddenField id="hdnOrigPrimaryFacilityId" runat="server" />
					<asp:HiddenField id="hdnOrigSecondaryFacilityId" runat="server" />
					<asp:HiddenField id="hdnOrigTertiaryFacilityId" runat="server" />
					<asp:HiddenField id="hdnLocalPosition1" runat="server" />
					<asp:HiddenField id="hdnTableOfficer1" runat="server" />
					<asp:HiddenField id="hdnLocalPosition2" runat="server" />
					<asp:HiddenField id="hdnTableOfficer2" runat="server" />
					<asp:HiddenField id="hdnLocalPosition3" runat="server" />
					<asp:HiddenField id="hdnTableOfficer3" runat="server" />
					<asp:HiddenField id="hdnBuglmPosition" runat="server" />
					<asp:HiddenField id="hdnCommunicationOptions" runat="server" />		
                    			
					<table class="frm-table">
						<tr>
							<td>&nbsp;</td>
							<td>Facility</td>
							<td>Employment Type</td>
							<td>Local Position</td>
							<td>Table Officer Position</td>
						</tr>
						<tr>
							<td>Primary</td>
							<td><asp:DropDownList ID="ddlFacility1" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem> </asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlEmploymentType1" runat="server"></asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlLocalPosition1" runat="server" AppendDataBoundItems="true" multiple="multiple"></asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlTableOfficer1" runat="server" AppendDataBoundItems="true" multiple="multiple"></asp:DropDownList></td>
						</tr>
						<tr>
							<td>Secondary</td>
							<td><asp:DropDownList ID="ddlFacility2" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlEmploymentType2" runat="server"></asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlLocalPosition2" runat="server" AppendDataBoundItems="true" multiple="multiple"></asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlTableOfficer2" runat="server" AppendDataBoundItems="true" multiple="multiple"></asp:DropDownList></td>
						</tr>
						<tr>
							<td>Tertiary</td>
							<td><asp:DropDownList ID="ddlFacility3" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlEmploymentType3" runat="server"></asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlLocalPosition3" runat="server" AppendDataBoundItems="true" multiple="multiple"></asp:DropDownList></td>
							<td><asp:DropDownList ID="ddlTableOfficer3" runat="server" AppendDataBoundItems="true" multiple="multiple"></asp:DropDownList></td>
						</tr>
					</table>
					
				</div>
			</div>
		</div>
	</div>
		
	<div class="section row clearfix">
			
		<h2 id="tglCommitteeSection" class="toggler closed">Board and Committee Membership Data</h2>
		<div class="collapsible closed" style="background-color:#e3dfdb;">
			<div class="wrapper-block clearfix">
			
				<div id="dvCommitteeData" class="clearfix">
					
					<div class="column float-left">
						<table id="tblBuglm" class="frm-table">
							<tr>
								<td>&nbsp;</td>
								<td>District</td>
								<td style="width:225px;">Position</td>
							</tr>
							<tr>
								<td id="tdBuglm"><span>BUGLM Committee</span></td>
								<td class="read-only"><div id="dvCommitteeDistrictBuglm">&nbsp;</div></td>
								<td><asp:DropDownList id="ddlBuglmPosition" runat="server" AppendDataBoundItems="true" multiple="multiple"></asp:DropDownList></td>
							</tr>
							<tr><td colspan="3">&nbsp;<br /></td></tr>
							<tr>
								<td>&nbsp;</td>
								<td>Region</td>
								<td>Position</td>
							</tr>
							<tr>
								<td id="tdBursary"><span>Bursary Committee</span></td>
								<td class="read-only"><div id="dvCommitteeRegionBursary">&nbsp;</div></td>
								<td><asp:DropDownList id="ddlBursaryPosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList></td>
							</tr>
							<tr>
								<td id="tdUnionDiscipline"><span>Union Discipline</span></td>
								<td class="read-only"><div id="dvCommitteeRegionUnionDiscipline">&nbsp;</div></td>
								<td><asp:DropDownList id="ddlUnionDisciplinePosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList></td>
							</tr>
							<tr>
								<td id="tdUnionDisciplineAppeal"><span>Union Discipline Appeal</span></td>
								<td class="read-only"><div id="dvCommitteeRegionUnionDisciplineAppeal">&nbsp;</div></td>
								<td><asp:DropDownList id="ddlUnionDisciplineAppealPosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList></td>
							</tr>
						</table>
						
					</div>
					
					<div class="column float-right">
						<table id="tblProvincialCommittees" class="frm-table">
							<tr>
								<td>&nbsp;</td>
								<td style="width:225px;">Position</td>
							</tr>
							<tr id="trBoardOfDirectors">
								<td id="tdBoardOfDirectors"><span>Board of Directors</span></td>
								<td><asp:DropDownList ID="ddlBoardOfDirectorsPosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList> </td>
							</tr>
							<tr>
								<td id="tdNegotiating"><span>Negotiating Committee</span></td>
								<td><asp:DropDownList ID="ddlNegotiatingPosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList> </td>
							</tr>
							<tr>
								<td id="tdAgm"><span>AGM Committee</span></td>
								<td><asp:DropDownList ID="ddlAgmPosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList> </td>
							</tr>
							<tr>
								<td id="tdConstitution"><span>Constitution &amp; Resolution Committee</span></td>
								<td><asp:DropDownList ID="ddlConstitutionPosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList> </td>
							</tr>
							<tr>
								<td id="tdEducation"><span>Education Committee</span></td>
								<td><asp:DropDownList ID="ddlEducationPosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList> </td>
							</tr>
							<tr>
								<td id="tdFinance"><span>Finance Committee</span></td>
								<td><asp:DropDownList ID="ddlFinancePosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList> </td>
							</tr>
							<tr>
								<td id="tdPersonnel"><span>Personnel Committee</span></td>
								<td><asp:DropDownList ID="ddlPersonnelPosition" runat="server" AppendDataBoundItems="true"><asp:ListItem Value="">Please Select</asp:ListItem></asp:DropDownList> </td>
							</tr>
						</table>
					</div>
					
					<div id="dvBuglmMembers" style="display:none"></div>
					<div id="dvBursaryMembers" style="display:none"></div>
					<div id="dvUnionDisciplineMembers" style="display:none"></div>
					<div id="dvUnionDisciplineAppealMembers" style="display:none"></div>
					<div id="dvBoardOfDirectorsMembers" style="display:none"></div>
					<div id="dvNegotiatingMembers" style="display:none"></div>
					<div id="dvAgmMembers" style="display:none"></div>
					<div id="dvConstitutionMembers" style="display:none"></div>
					<div id="dvEducationMembers" style="display:none"></div>
					<div id="dvFinanceMembers" style="display:none"></div>
					<div id="dvPersonnelMembers" style="display:none"></div>
				</div>
			</div>
		</div>

	</div>
	
    <div id="creds">

	    <div class="section row clearfix">			
		    <h2 id="tglCredentialsSection" class="toggler closed">Membership Credentials</h2>
		    <div class="collapsible closed">
			    <div id="dvCredentialsSection" class="wrapper-block clearfix">
				    <div class="grid_12"><label>User Name  <span class="required">&bull;</span></label> <asp:TextBox ID="tbUserName" runat="server"></asp:TextBox>
                        <span id="inactive-msg"><asp:Literal ID="litInactive" runat="server" Text="This member is {0} and will not be able to log in."/></span></div>
				    <div class="grid_3"><label>Password  <span class="required">&bull;</span></label> <asp:TextBox ID="tbPassword" runat="server" MaxLength="20"></asp:TextBox></div>
				    <div class="grid_3"><label>Confirm Password  <span class="required">&bull;</span></label> <asp:TextBox ID="tbConfirmPassword" runat="server" MaxLength="20"></asp:TextBox></div>
			    </div>
		    </div>
	    </div>
    </div>	

	<asp:PlaceHolder runat="server" ID="plcNotes">
	<div class="section row clearfix">		
		<h2 class="toggler closed">Notes</h2>
		<div class="collapsible closed" style="background-color:#e3dfdb;">	
			<div class="wrapper-block clearfix">
				<div class="grid_12">
					<div id="dvNoteSection">
						<div id="dvNoteTable" class="frm-section">
						
						</div>
						
						<h2>Add Note</h2>
						<asp:TextBox ID="tbNote" runat="server" TextMode="MultiLine" MaxLength="1000" style="width:96%;"></asp:TextBox>
						    
					</div>
					<input type="button" id="btnAddNote" class="frm-btn" value="Add Note" />

				</div>
			</div>
		</div>
	</div>
    </asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plcHistory">
	<div class="section row clearfix">		
		<h2 class="toggler closed">History</h2>
		<div class="collapsible closed" style="background-color:#e3dfdb;">	
			<div class="wrapper-block clearfix">
				<div class="grid_12">
					<div id="dvVersionHistorySection">
						<div id="dvVersionHistoryTable" class="frm-section"></div>
					</div>
				</div>
			</div>
		</div>
	</div>
    </asp:PlaceHolder>

    <div class="section row clearfix">
    	<asp:Button ID="btnSubmit" runat="server" CssClass="frm-btn" Text="Submit" OnClick="btnSubmit_onClick"/>
    	<asp:Button ID="btnDelete" runat="server" CssClass="frm-btn" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete current Nurse Profile?')" OnClick="btnDelete_onClick"/>
    	<asp:Button ID="btnCancel" runat="server" CssClass="frm-btn" Text="Cancel"  OnClick="btnCancel_onClick"/>
    </div>

    </div>

</asp:Content>
