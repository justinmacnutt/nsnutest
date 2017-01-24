using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nsnu.DataAccess;
using Nsnu.DataAccess.Enumerations;
using Nsnu.MembershipServices;

using WebApplication.Utilities;
using CommitteeEnum = Nsnu.DataAccess.Enumerations.Committee;
using PositionEnum = Nsnu.DataAccess.Enumerations.CommitteePosition;
using FacilityTypeEnum = Nsnu.DataAccess.Enumerations.FacilityType;
using RegionEnum = Nsnu.DataAccess.Enumerations.Region;
using DistrictEnum = Nsnu.DataAccess.Enumerations.District;
using System.Web.Script.Serialization;

public partial class Admin_NurseProfile : System.Web.UI.Page
{
    private MembershipBs _mBs = new MembershipBs();
    private SimpleAES _aes = new SimpleAES();

    private Dictionary<string, List<string>> _formErrors = new Dictionary<string, List<string>>();
    private JavaScriptSerializer _ser = new JavaScriptSerializer();

    Regex _phoneRegex = new Regex(@"^\d\d\d\-\d\d\d\-\d\d\d\d$");
    Regex _passwordRegex = new Regex(@"^[a-zA-Z0-9]{6,20}$"); // between 6 and 20 alphanumeric characters
    Regex _postalCodeRegex = new Regex(@"^(?<full>(?<part1>[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1})(?:[ ](?=\d))?(?<part2>\d{1}[A-Z]{1}\d{1}))$");
    Regex _emailRegex = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");



    protected string BuglmMembers;
    protected string BursaryMembers;
    protected string UnionDisciplineMembers;
    protected string UnionDisciplineAppealMembers;
    protected string BoardOfDirectorsMembers;
    protected string NegotiatingMembers;
    protected string AgmMembers;
    protected string ConstitutionMembers;
    protected string EducationMembers;
    protected string FinanceMembers;
    protected string PersonnelMembers;
    protected string FormErrors = String.Empty;

    private bool _isAdd = true;

    #region Page Event handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InstantiateForm();    

            if (Request.QueryString["id"] != null) //Edit screen
            {
                var id = int.Parse(Request.QueryString["id"]);
                InstantiateNurse(id);

                MySessionVariables.CurrentIndex.Value = MySessionVariables.NurseSearchItems.Value.IndexOf(id);

                btnPrevNurse.Enabled = (MySessionVariables.CurrentIndex.Value == 0) ? false : true;
                btnNextNurse.Enabled = (MySessionVariables.CurrentIndex.Value == MySessionVariables.NurseSearchItems.Value.Count - 1) ? false : true;

                //enable delete button
                btnDelete.Visible = true;

                //enable paging
                dvPagingWidget.Visible = btnPrevNurse.Enabled || btnNextNurse.Enabled;

                //enable history
                plcHistory.Visible = true;

                //enable notes
                plcNotes.Visible = true;

                //set page title
                litHeading.Text ="Modify Nurse Profile";

                // profile username is non-editable in edit mode
                tbUserName.Enabled = false;
            }
            else //Add screen
            {
                litHeading.Text = "Add Nurse Profile";

                // hide the paging section, and History section on Add Nurse screeno
                dvPagingWidget.Visible = false;
                plcHistory.Visible = false;

                // hide the Notes section, can't add a Note since we have no nurseid
                plcNotes.Visible = false;
            }
        }

        InstantiateCommitteeData();

        hdnReadOnlyUser.Value = "false";
        //get the logged on user and see if they are readonly
        if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.IsInRole("readonly"))
        {
            hdnReadOnlyUser.Value = "true";
        }

        if (MySessionVariables.NurseSearchItems.Value.Count < 1)
        {
            dvPagingWidget.Visible = false;
        }
    }

    #endregion

    #region Control Event handlers

    protected void btnSubmit_onClick(object sender, EventArgs e)
    {
   
        if (!ValidateNurse())
        {
            FormErrors = _ser.Serialize(_formErrors);
            return;
        }
        ProcessNurse();

        // on an Add user is redirected back to the index page, on an edit user stays on Edit screen
        if (_isAdd)
            Response.Redirect("Index.aspx");
 
    }

    protected void btnDelete_onClick(object sender, EventArgs e)
    {
        var myId = (!String.IsNullOrEmpty(hdnNurseId.Value)) ? int.Parse(hdnNurseId.Value) : -1;

        if (myId == -1)
        {
            // no nurse to delete
            Response.Redirect("Index.aspx");
        }
        else
        {
            // delete nurse
            _mBs.DeleteNurse(myId);

            Response.Redirect("Index.aspx");
        }
    }

    protected void btnCancel_onClick(object sender, EventArgs e)
    {
            Response.Redirect("Index.aspx");
    }

    protected void btnPrevNurse_OnClick(object sender, EventArgs e)
    {
        int i = MySessionVariables.NurseSearchItems.Value[MySessionVariables.CurrentIndex.Value - 1];

        Response.Redirect(String.Format("NurseProfile.aspx?id={0}", i));
    }

    protected void btnNextNurse_OnClick(object sender, EventArgs e)
    {
        int i = MySessionVariables.NurseSearchItems.Value[MySessionVariables.CurrentIndex.Value + 1];

        Response.Redirect(String.Format("NurseProfile.aspx?id={0}", i));
    }
    #endregion

    #region Web Methods

    [WebMethod]
    public static List<VersionVo> GetVersionHistory(string nurseId)
    {
        if (nurseId == "")
        {
            return new List<VersionVo>();
        }
        var nId = int.Parse(nurseId);

        var mBs = new MembershipBs();

        return mBs.GetVersionHistory(nId).Select(FillVersionVo).ToList();
    }


    [WebMethod]
    public static List<NoteVo> GetNotes(string nurseId)
    {
        if (nurseId == "")
        {
            return new List<NoteVo>();
        }
        var nId = int.Parse(nurseId);

        var mBs = new MembershipBs();

        return mBs.GetNotes(nId).Select(FillNoteVo).ToList();
    }

    [WebMethod]
    public static void AddNote(string nurseId, string noteBody)
    {
        if (nurseId == "" || noteBody == "")
        {
            return;
        }
        
        var nId = int.Parse(nurseId);

        var mBs = new MembershipBs();

        mBs.AddNote(nId, noteBody, HttpContext.Current.User.Identity.Name);
    }

    [WebMethod]
    public static void DeleteNote(string noteId)
    {
        if (noteId == "")
        {
            return;
        }
        var nId = int.Parse(noteId);

        var mBs = new MembershipBs();

        mBs.DeleteNote(nId);
    }

    [WebMethod]
    public static FacilityVo GetFacility(string facilityId)
    {
        if (String.IsNullOrEmpty(facilityId))
        {
            return null;
        }

        var mBs = new MembershipBs();

        var f = mBs.GetFacility(int.Parse(facilityId));

        var fVo = new FacilityVo
                      {
                          regionName = f.District.Region.regionName,
                          regionId = f.District.regionId.ToString(),
                          districtId = f.districtId.ToString(),
                          districtName = f.District.districtName,
                          facilityId = f.id.ToString()
                      };

        return fVo;
    }

#endregion

    #region Public Methods

    public static NoteVo FillNoteVo(Note n)
    {
        var myVo = new NoteVo
                       {
                           noteId = n.id.ToString(),
                           noteBody = n.noteBody,
                           creationDate = String.Format("{0:g}", n.creationDate),
                           createdBy = n.createdBy
                       };


        return myVo;
    }

    public static VersionVo FillVersionVo(VersionHistory vh)
    {
        var myVo = new VersionVo
        {
            versionId = vh.id.ToString(),
            modificationDate = String.Format("{0:g}", vh.modificationDate),
            modifiedBy = vh.modifiedBy
        };

        return myVo;
    }

    #endregion

    #region Private Methods

    private void InstantiateCommitteeData()
    {
        
        var cml = _mBs.GetCommitteeMembers();
        var districtId = (!String.IsNullOrEmpty(hdnCommitteeDistrictId.Value)) ? byte.Parse(hdnCommitteeDistrictId.Value) : (byte?)null;
        var regionId = (!String.IsNullOrEmpty(hdnCommitteeRegionId.Value)) ? byte.Parse(hdnCommitteeRegionId.Value) : (byte?)null;
        
        BuglmMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.Buglm, cml, null, districtId));
        
        BursaryMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.Bursary, cml, regionId, null));
        UnionDisciplineMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.UnionDiscipline, cml, regionId, null));
        UnionDisciplineAppealMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.UnionDisciplineAppeal, cml, regionId, null));
        //BoardOfDirectorsMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.BoardOfDirectors, cml));
        BoardOfDirectorsMembers = _ser.Serialize(GenerateBoardOfDirectorsMemberList(cml));
        NegotiatingMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.Negotiating, cml));
        AgmMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.Agm, cml));
        ConstitutionMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.Constitution, cml));
        EducationMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.Education, cml));
        FinanceMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.Finance, cml));
        PersonnelMembers = _ser.Serialize(GenerateCommitteeMemberList(CommitteeEnum.Personnel, cml));
    }

    private void InstantiateForm()
    {
        FormErrors = _ser.Serialize(_formErrors);

        var phoneTypeListItems = InterfaceUtils.GetEnumerationListItems<PhoneType>(false);
        
        //ddlPhoneType1.DataSource = phoneTypeListItems;
        //ddlPhoneType1.DataTextField = "Text";
        //ddlPhoneType1.DataValueField = "Value";
        //ddlPhoneType1.DataBind();

        // for the first drop down we remove work and fax 
        //ListItem workItem = ddlPhoneType1.Items.FindByValue(((int)(PhoneType.Work)).ToString());
        //ddlPhoneType1.Items.Remove(workItem);

        //ListItem faxItem = ddlPhoneType1.Items.FindByValue(((int)(PhoneType.Fax)).ToString());
        //ddlPhoneType1.Items.Remove(faxItem);

        //ddlPhoneType1.SelectedValue = ((int) (PhoneType.Home)).ToString();


        //ddlPhoneType2.DataSource = phoneTypeListItems;
        //ddlPhoneType2.DataTextField = "Text";
        //ddlPhoneType2.DataValueField = "Value";
        //ddlPhoneType2.DataBind();
        //ddlPhoneType2.SelectedValue = ((int)(PhoneType.Work)).ToString();

        //ddlPhoneType3.DataSource = phoneTypeListItems;
        //ddlPhoneType3.DataTextField = "Text";
        //ddlPhoneType3.DataValueField = "Value";
        //ddlPhoneType3.DataBind();
        //ddlPhoneType3.SelectedValue = ((int)(PhoneType.Cell)).ToString();

        //ddlPhoneType4.DataSource = phoneTypeListItems;
        //ddlPhoneType4.DataTextField = "Text";
        //ddlPhoneType4.DataValueField = "Value";
        //ddlPhoneType4.DataBind();
        //ddlPhoneType4.SelectedValue = ((int)(PhoneType.Fax)).ToString();

        ddlGender.DataSource = InterfaceUtils.GetEnumerationListItems<Gender>(true);
        ddlGender.DataTextField = "Text";
        ddlGender.DataValueField = "Value";
        ddlGender.DataBind();

        ddlCommunicationOptions.DataSource = InterfaceUtils.GetEnumerationListItems<CommunicationOption>(false);
        ddlCommunicationOptions.DataTextField = "Text";
        ddlCommunicationOptions.DataValueField = "Value";
        ddlCommunicationOptions.DataBind();

        ddlDesignation.DataSource = InterfaceUtils.GetEnumerationListItems<Designation>(true);
        ddlDesignation.DataTextField = "Text";
        ddlDesignation.DataValueField = "Value";
        ddlDesignation.DataBind();

        ddlStatus.DataSource = InterfaceUtils.GetEnumerationListItems<EmploymentStatus>(true);
        ddlStatus.DataTextField = "Text";
        ddlStatus.DataValueField = "Value";
        ddlStatus.DataBind();

        var facilityListItems = _mBs.GetFacilities().OrderBy(f=> f.facilityName);

        ddlFacility1.DataSource = facilityListItems;
        ddlFacility1.DataTextField = "facilityName";
        ddlFacility1.DataValueField = "id";
        ddlFacility1.DataBind();

        ddlFacility2.DataSource = facilityListItems;
        ddlFacility2.DataTextField = "facilityName";
        ddlFacility2.DataValueField = "id";
        ddlFacility2.DataBind();

        ddlFacility3.DataSource = facilityListItems;
        ddlFacility3.DataTextField = "facilityName";
        ddlFacility3.DataValueField = "id";
        ddlFacility3.DataBind();

        var employmentTypeListItems = InterfaceUtils.GetEnumerationListItems<EmploymentType>(true);

        ddlEmploymentType1.DataSource = employmentTypeListItems;
        ddlEmploymentType1.DataTextField = "Text";
        ddlEmploymentType1.DataValueField = "Value";
        ddlEmploymentType1.DataBind();

        //remove Full time for secondary and tertiary facilities accordig to spec?
        var itemToRemove = employmentTypeListItems.Single(r => r.Value == "1");
        employmentTypeListItems.Remove(itemToRemove);

        ddlEmploymentType2.DataSource = employmentTypeListItems;
        ddlEmploymentType2.DataTextField = "Text";
        ddlEmploymentType2.DataValueField = "Value";
        ddlEmploymentType2.DataBind();

        ddlEmploymentType3.DataSource = employmentTypeListItems;
        ddlEmploymentType3.DataTextField = "Text";
        ddlEmploymentType3.DataValueField = "Value";
        ddlEmploymentType3.DataBind();

        var localPositionListItems = _mBs.GetLocalPositions().OrderBy(lp => lp.positionName);

        ddlLocalPosition1.DataSource = localPositionListItems;
        ddlLocalPosition1.DataTextField = "positionName";
        ddlLocalPosition1.DataValueField = "id";
        ddlLocalPosition1.DataBind();

        ddlLocalPosition2.DataSource = localPositionListItems;
        ddlLocalPosition2.DataTextField = "positionName";
        ddlLocalPosition2.DataValueField = "id";
        ddlLocalPosition2.DataBind();

        ddlLocalPosition3.DataSource = localPositionListItems;
        ddlLocalPosition3.DataTextField = "positionName";
        ddlLocalPosition3.DataValueField = "id";
        ddlLocalPosition3.DataBind();

        var tableOfficerListItems = _mBs.GetTableOfficerPositions().OrderBy(top => top.positionName);

        ddlTableOfficer1.DataSource = tableOfficerListItems;
        ddlTableOfficer1.DataTextField = "positionName";
        ddlTableOfficer1.DataValueField = "id";
        ddlTableOfficer1.DataBind();

        ddlTableOfficer2.DataSource = tableOfficerListItems;
        ddlTableOfficer2.DataTextField = "positionName";
        ddlTableOfficer2.DataValueField = "id";
        ddlTableOfficer2.DataBind();

        ddlTableOfficer3.DataSource = tableOfficerListItems;
        ddlTableOfficer3.DataTextField = "positionName";
        ddlTableOfficer3.DataValueField = "id";
        ddlTableOfficer3.DataBind();

        //ddlBuglmPosition.DataSource = _mBs.GetCommitteePositions(Nsnu.DataAccess.Enumerations.Committee.Buglm);
        ddlBuglmPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.Buglm, false);
        ddlBuglmPosition.DataTextField = "Text";
        ddlBuglmPosition.DataValueField = "Value";
        ddlBuglmPosition.DataBind();

        ddlBoardOfDirectorsPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.BoardOfDirectors);
        ddlBoardOfDirectorsPosition.DataTextField = "Text";
        ddlBoardOfDirectorsPosition.DataValueField = "Value";
        ddlBoardOfDirectorsPosition.DataBind();

        ddlNegotiatingPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.Negotiating,false); 
        ddlNegotiatingPosition.DataTextField = "Text";
        ddlNegotiatingPosition.DataValueField = "Value";
        ddlNegotiatingPosition.DataBind();

        ddlAgmPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.Agm);
        ddlAgmPosition.DataTextField = "Text";
        ddlAgmPosition.DataValueField = "Value";
        ddlAgmPosition.DataBind();

        ddlBursaryPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.Bursary);
        ddlBursaryPosition.DataTextField = "Text";
        ddlBursaryPosition.DataValueField = "Value";
        ddlBursaryPosition.DataBind();

        ddlUnionDisciplinePosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.UnionDiscipline);
        ddlUnionDisciplinePosition.DataTextField = "Text";
        ddlUnionDisciplinePosition.DataValueField = "Value";
        ddlUnionDisciplinePosition.DataBind();

        ddlUnionDisciplineAppealPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.UnionDisciplineAppeal);
        ddlUnionDisciplineAppealPosition.DataTextField = "Text";
        ddlUnionDisciplineAppealPosition.DataValueField = "Value";
        ddlUnionDisciplineAppealPosition.DataBind();

        ddlConstitutionPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.Constitution);
        ddlConstitutionPosition.DataTextField = "Text";
        ddlConstitutionPosition.DataValueField = "Value";
        ddlConstitutionPosition.DataBind();

        ddlEducationPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.Education);
        ddlEducationPosition.DataTextField = "Text";
        ddlEducationPosition.DataValueField = "Value";
        ddlEducationPosition.DataBind();
         
        ddlFinancePosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.Finance);
        ddlFinancePosition.DataTextField = "Text";
        ddlFinancePosition.DataValueField = "Value";
        ddlFinancePosition.DataBind();

        ddlPersonnelPosition.DataSource = InterfaceUtils.GetCommitteePositionListItems(Nsnu.DataAccess.Enumerations.Committee.Personnel);
        ddlPersonnelPosition.DataTextField = "Text";
        ddlPersonnelPosition.DataValueField = "Value";
        ddlPersonnelPosition.DataBind();

        // hide the delete button by default
        btnDelete.Visible = false;

    }

    private void InstantiateNurse(int id)
    {
        var n = _mBs.GetUserProfile(id);

        hdnNurseId.Value = n.id.ToString();
        tbUserName.Text = n.username;

        // no password or can't decrypt an unencrytpted string such as "password" will throw an exception
        try
        {
            tbPassword.Text = _aes.Decrypt(n.password);
            tbConfirmPassword.Text = _aes.Decrypt(n.password);
        }
        //catch (System.Security.Cryptography.CryptographicException cex)
        //{
        //    tbPassword.Text = n.password; 
        //    tbConfirmPassword.Text = n.password; 
        //}

        catch (System.Exception)
        {
            tbPassword.Text = String.Empty;
            tbConfirmPassword.Text = String.Empty;
        }



        tbFirstName.Text = n.Nurse.firstName;
        tbLastName.Text = n.Nurse.lastName;
        tbMiddleInitial.Text = n.Nurse.initial;
        ddlGender.SelectedValue = n.Nurse.genderId.ToString();
        tbNickname.Text = n.Nurse.nickname == null ? "" : n.Nurse.nickname;
        
        tbBirthDate.Text = n.Nurse.birthDate.HasValue ? ((DateTime)n.Nurse.birthDate).ToString("MM-dd-yyyy") : "";

        NurseAddress na = _mBs.GetNurseAddress(n.id);

        tbAddressLine1.Text = na.Address.line1;
        tbAddressLine2.Text = na.Address.line2;
        tbCity.Text = na.Address.city;
        ddlProvince.SelectedValue = na.Address.provinceId;
        tbPostalCode.Text = na.Address.postalCode;
        tbEmail.Text = n.email;
        tbSecondaryEmail.Text = n.secondaryemail;

        List<Phone> ps = _mBs.GetPhones(n.id);

        
        if ((from p in ps where p.phoneTypeId == (int)PhoneType.Home select p).Count() > 0)
        {
            tbPhoneNumber1.Text = (from p in ps where p.phoneTypeId == (int)PhoneType.Home select p).First().phoneNumber;
        }

        if ((from p in ps where p.phoneTypeId == (int)PhoneType.Work select p).Count() > 0)
        {
            tbPhoneNumber2.Text = (from p in ps where p.phoneTypeId == (int)PhoneType.Work select p).First().phoneNumber;
            tbPhoneExt2.Text = (from p in ps where p.phoneTypeId == (int)PhoneType.Work select p).First().extension;
        }

        if ((from p in ps where p.phoneTypeId == (int)PhoneType.Cell select p).Count() > 0)
        {
            tbPhoneNumber3.Text = (from p in ps where p.phoneTypeId == (int)PhoneType.Cell select p).First().phoneNumber;
        }

        if ((from p in ps where p.phoneTypeId == (int)PhoneType.Fax select p).Count() > 0)
        {
            tbPhoneNumber4.Text = (from p in ps where p.phoneTypeId == (int)PhoneType.Fax select p).First().phoneNumber;
        }

        // employment data section
        cbMembershipCardIssued.Checked =  (bool)n.Nurse.issuedMembershipCard;
        cbMembershipFormCompleted.Checked = (bool)n.Nurse.completedMembershipForm;
        ddlStatus.SelectedValue = n.Nurse.employmentStatusId.ToString();
        ddlDesignation.SelectedValue = n.Nurse.nurseDesignationId.ToString();

		// additional status message if the member is inactive/retired
		if (n.Nurse.employmentStatusId.HasValue) {
			litInactive.Text = string.Format(litInactive.Text, (EmploymentStatus)n.Nurse.employmentStatusId);
			litInactive.Visible = (n.Nurse.employmentStatusId == (byte)EmploymentStatus.Inactive || n.Nurse.employmentStatusId == (byte)EmploymentStatus.Retired);
		} else {
			litInactive.Visible = false;
		}
        //communication options section
        var commOpts = _mBs.GetNurseCommunicationOptions(id);
        if (commOpts.Count > 0)
        {
            hdnCommunicationOptions.Value = String.Join(",", commOpts.Select(flp => flp.optOutId.ToString()).ToArray());
        }


        var nfl = _mBs.GetNurseFacilities(id);

        if (nfl.Count > 0 && nfl.Where(nf => nf.priority == 1).Count() > 0)
        {
            var f = nfl.Where(nf => nf.priority == 1).FirstOrDefault();
            ddlFacility1.SelectedValue = f.facilityId.ToString();
            ddlEmploymentType1.SelectedValue = f.employmentTypeId.ToString();

            hdnCommitteeDistrictId.Value = f.Facility.districtId.ToString();
            hdnCommitteeRegionId.Value = f.Facility.District.regionId.ToString();
            hdnOrigPrimaryFacilityId.Value = f.facilityId.ToString();
            //hdnCommitteeDistrictId.Value = 

            var flpl = _mBs.GetLocalPositions(id, f.facilityId);
            var ftol = _mBs.GetTableOfficerPositions(id, f.facilityId);

            if (flpl.Count > 0)
            {
                hdnLocalPosition1.Value = String.Join(",",flpl.Select(flp => flp.positionId.ToString()).ToArray());
            }
            if (ftol.Count > 0)
            {
                hdnTableOfficer1.Value = String.Join(",", ftol.Select(fto => fto.positionId.ToString()).ToArray());
            }
        }

        if (nfl.Count > 0 && nfl.Where(nf => nf.priority == 2).Count() > 0)
        {
            var f = nfl.Where(nf => nf.priority == 2).FirstOrDefault();
            ddlFacility2.SelectedValue = f.facilityId.ToString();
            ddlEmploymentType2.SelectedValue = f.employmentTypeId.ToString();

            hdnOrigSecondaryFacilityId.Value = f.facilityId.ToString();


            var flpl = _mBs.GetLocalPositions(id, f.facilityId);
            var ftol = _mBs.GetTableOfficerPositions(id, f.facilityId);

            if (flpl.Count > 0)
            {
                hdnLocalPosition2.Value = String.Join(",", flpl.Select(flp => flp.positionId.ToString()).ToArray());
            }
            if (ftol.Count > 0)
            {
                hdnTableOfficer2.Value = String.Join(",", ftol.Select(fto => fto.positionId.ToString()).ToArray());
            }
        }

        if (nfl.Count > 0 && nfl.Where(nf => nf.priority == 3).Count() > 0)
        {
            var f = nfl.Where(nf => nf.priority == 3).FirstOrDefault();
            ddlFacility3.SelectedValue = f.facilityId.ToString();
            ddlEmploymentType3.SelectedValue = f.employmentTypeId.ToString();
            hdnOrigTertiaryFacilityId.Value = f.facilityId.ToString();



            var flpl = _mBs.GetLocalPositions(id, f.facilityId);
            var ftol = _mBs.GetTableOfficerPositions(id, f.facilityId);

            if (flpl.Count > 0)
            {
                hdnLocalPosition3.Value = String.Join(",", flpl.Select(flp => flp.positionId.ToString()).ToArray());
            }
            if (ftol.Count > 0)
            {
                hdnTableOfficer3.Value = String.Join(",", ftol.Select(fto => fto.positionId.ToString()).ToArray());
            }
        }

        var ncpl = _mBs.GetNurseCommitteePositions(id);

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Buglm)).Count() > 0)
        {
            hdnBuglmPosition.Value = String.Join(",",
                                                 ncpl.Where(ncp => ncp.committeeId == (byte) (CommitteeEnum.Buglm)).
                                                     Select(
                                                         ncp => ncp.positionId + (100)*Convert.ToByte(ncp.isAlternate)));
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Bursary)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte) (CommitteeEnum.Bursary)).FirstOrDefault();
            ddlBursaryPosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString();
            hdnOrigBursaryPosition.Value = ddlBursaryPosition.SelectedValue;
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.UnionDiscipline)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.UnionDiscipline)).FirstOrDefault();
            ddlUnionDisciplinePosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString();
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.UnionDisciplineAppeal)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.UnionDisciplineAppeal)).FirstOrDefault();
            ddlUnionDisciplineAppealPosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString();
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.BoardOfDirectors)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.BoardOfDirectors)).FirstOrDefault();
            ddlBoardOfDirectorsPosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString(); 
            hdnOrigBoardOfDirectorsPosition.Value = ddlBoardOfDirectorsPosition.SelectedValue;
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Negotiating)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Negotiating)).FirstOrDefault();
            ddlNegotiatingPosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString(); 
            hdnOrigNegotiatingPosition.Value = ddlNegotiatingPosition.SelectedValue;
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Agm)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Agm)).FirstOrDefault();
            ddlAgmPosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString();
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Constitution)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Constitution)).FirstOrDefault();
            ddlConstitutionPosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString();
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Education)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Education)).FirstOrDefault();
            ddlEducationPosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString();
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Finance)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Finance)).FirstOrDefault();
            ddlFinancePosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString();
        }

        if (ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Personnel)).Count() > 0)
        {
            var position = ncpl.Where(ncp => ncp.committeeId == (byte)(CommitteeEnum.Personnel)).FirstOrDefault();
            ddlPersonnelPosition.SelectedValue = (position.positionId + (100) * Convert.ToByte(position.isAlternate)).ToString();
        }
    }

    private string CleanPostalCode(string postalCode)
    {
        if (postalCode == String.Empty)
            return String.Empty;
        if (postalCode.Length == 7 && postalCode.Contains(' '))
            return postalCode;
        if (postalCode.Length == 6)
        {
            string temp = postalCode.Insert(3, " ");
            return temp;
        }

        return postalCode;
    }


    private void ProcessNurse()
    {
        UserProfile up;
        NurseAddress na;
        
        List<NursePhone> npl = new List<NursePhone>();

        var myId = (!String.IsNullOrEmpty(hdnNurseId.Value)) ? int.Parse(hdnNurseId.Value) : -1;

        //An Add
        //up = myId == -1 ? new UserProfile() : _mBs.GetUserProfile(myId);
        if (myId == -1)
        {
            _isAdd = true;
            up = new UserProfile {Nurse = new Nurse()};
            na = new NurseAddress{ Address = new Address(),
                                   Nurse = up.Nurse};

            up.creationDate = DateTime.Now;  
            up.username = _mBs.GenerateUniqueUserName(tbLastName.Text.Trim(), tbFirstName.Text.Trim(), tbMiddleInitial.Text.Trim());
            String pw = MembershipBs.GeneratePassword();
            up.password = _aes.Encrypt(pw); 
        }
        else
        {
            _isAdd = false;
            up = _mBs.GetUserProfile(myId);
            up.username = tbUserName.Text;
            up.password = _aes.Encrypt(tbPassword.Text.Trim());


            na = _mBs.GetNurseAddress(myId);
            npl = _mBs.GetNursePhones(myId);
        }

        up.email = tbEmail.Text;
        up.secondaryemail = tbSecondaryEmail.Text;
        
        up.Nurse.firstName = tbFirstName.Text;
        up.Nurse.lastName = tbLastName.Text;
        up.Nurse.initial = tbMiddleInitial.Text;
        up.Nurse.genderId = (ddlGender.SelectedValue != "") ? byte.Parse(ddlGender.SelectedValue) : (byte?)null;
        up.Nurse.nickname = tbNickname.Text;
        up.Nurse.birthDate = (tbBirthDate.Text != "") ? DateTime.ParseExact(tbBirthDate.Text, "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;
        up.Nurse.lastModifiedBy = HttpContext.Current.User.Identity.Name;
        up.Nurse.lastModifiedDate = DateTime.Now;
        
        up.Nurse.issuedMembershipCard = cbMembershipCardIssued.Checked;
        up.Nurse.completedMembershipForm = cbMembershipFormCompleted.Checked;
        up.Nurse.employmentStatusId = (ddlStatus.SelectedValue != "") ? byte.Parse(ddlStatus.SelectedValue) : (byte?)null;
        up.Nurse.nurseDesignationId = (ddlDesignation.SelectedValue != "") ? byte.Parse(ddlDesignation.SelectedValue) : (byte?)null;
        up.Nurse.initial = tbMiddleInitial.Text;
       // up.Nurse.nurseDesignationId = 1;

        na.Address.addressTypeId = (byte)(AddressType.Mailing);
        na.Address.line1 = tbAddressLine1.Text;
        na.Address.line2 = tbAddressLine2.Text;
        na.Address.city = tbCity.Text;
        na.Address.provinceId = ddlProvince.SelectedValue;
        na.Address.postalCode = CleanPostalCode(tbPostalCode.Text);
        na.Address.lastModifiedBy = HttpContext.Current.User.Identity.Name;
        na.Address.lastModifiedDate = DateTime.Now;
        
        #region PhoneData

        //Delete All employment data
        _mBs.DeletePhoneData(myId);


        if (!String.IsNullOrEmpty(tbPhoneNumber1.Text) || !String.IsNullOrEmpty(hdnPhone1Id.Value))
        {
            NursePhone np1 = !String.IsNullOrEmpty(hdnPhone1Id.Value) ? npl.Where(a => a.phoneId == int.Parse(hdnPhone1Id.Value)).FirstOrDefault() : new NursePhone { Phone = new Phone(), Nurse = up.Nurse };
            np1.Phone.phoneNumber = tbPhoneNumber1.Text;
            np1.Phone.phoneTypeId = (int)PhoneType.Home;
        }

        if (!String.IsNullOrEmpty(tbPhoneNumber2.Text) || !String.IsNullOrEmpty(hdnPhone2Id.Value))
        {
            NursePhone np2 = !String.IsNullOrEmpty(hdnPhone2Id.Value) ? npl.Where(a => a.phoneId == int.Parse(hdnPhone2Id.Value)).FirstOrDefault() : new NursePhone { Phone = new Phone(), Nurse = up.Nurse };
            np2.Phone.phoneNumber = tbPhoneNumber2.Text;
            np2.Phone.extension = tbPhoneExt2.Text;
            np2.Phone.phoneTypeId = (int)PhoneType.Work;
        }

        if (!String.IsNullOrEmpty(tbPhoneNumber3.Text) || !String.IsNullOrEmpty(hdnPhone3Id.Value))
        {
            NursePhone np3 = !String.IsNullOrEmpty(hdnPhone3Id.Value) ? npl.Where(a => a.phoneId == int.Parse(hdnPhone3Id.Value)).FirstOrDefault() : new NursePhone { Phone = new Phone(), Nurse = up.Nurse };
            np3.Phone.phoneNumber = tbPhoneNumber3.Text;
            np3.Phone.phoneTypeId = (int)PhoneType.Cell;
        }

        if (!String.IsNullOrEmpty(tbPhoneNumber4.Text) || !String.IsNullOrEmpty(hdnPhone4Id.Value))
        {
            NursePhone np4 = !String.IsNullOrEmpty(hdnPhone4Id.Value) ? npl.Where(a => a.phoneId == int.Parse(hdnPhone4Id.Value)).FirstOrDefault() : new NursePhone { Phone = new Phone(), Nurse = up.Nurse };
            np4.Phone.phoneNumber = tbPhoneNumber4.Text;
            np4.Phone.phoneTypeId = (int)PhoneType.Fax;
        }
        #endregion

        #region EmploymentData
        var et1 = ddlEmploymentType1.SelectedValue;
        var et2 = ddlEmploymentType2.SelectedValue;
        var et3 = ddlEmploymentType3.SelectedValue;

        var f1 = ddlFacility1.SelectedValue;
        var f2 = ddlFacility2.SelectedValue;
        var f3 = ddlFacility3.SelectedValue;
        
        var lp1s = (!String.IsNullOrEmpty(hdnLocalPosition1.Value)) ? hdnLocalPosition1.Value.Split(',').Select(int.Parse).ToList() : new List<int>{};
        var lp2s = (!String.IsNullOrEmpty(hdnLocalPosition2.Value)) ? hdnLocalPosition2.Value.Split(',').Select(int.Parse).ToList(): new List<int>{};
        var lp3s = (!String.IsNullOrEmpty(hdnLocalPosition3.Value)) ? hdnLocalPosition3.Value.Split(',').Select(int.Parse).ToList() : new List<int> {};

        var to1s = (!String.IsNullOrEmpty(hdnTableOfficer1.Value)) ? hdnTableOfficer1.Value.Split(',').Select(int.Parse).ToList() : new List<int> {};
        var to2s = (!String.IsNullOrEmpty(hdnTableOfficer2.Value)) ? hdnTableOfficer2.Value.Split(',').Select(int.Parse).ToList() : new List<int> { };
        var to3s = (!String.IsNullOrEmpty(hdnTableOfficer3.Value)) ? hdnTableOfficer3.Value.Split(',').Select(int.Parse).ToList() : new List<int> { }; 

        //Delete All employment data
        _mBs.DeleteEmploymentData(myId);

        if (!String.IsNullOrEmpty(f1))
        {
            var nf1 = new NurseFacility
                                    {
                                        Nurse = up.Nurse,
                                        facilityId = int.Parse(f1),
                                        employmentTypeId = (!String.IsNullOrEmpty(et1)) ? byte.Parse(et1) : (byte?)null,
                                        priority = 1
                                    };

            foreach (var lp in lp1s)
            {
                new FacilityLocalPosition {NurseFacility = nf1, positionId = lp};
            }

            foreach (var to in to1s)
            {
                new FacilityTableOfficer { NurseFacility = nf1, positionId = to };
            }
        }

        if (!String.IsNullOrEmpty(f2))
        {
            var nf2 = new NurseFacility
            {
                Nurse = up.Nurse,
                facilityId = int.Parse(f2),
                employmentTypeId = (!String.IsNullOrEmpty(et2)) ? byte.Parse(et2) : (byte?)null,
                priority = 2
            };


            foreach (var lp in lp2s)
            {
                new FacilityLocalPosition { NurseFacility = nf2, positionId = lp };
            }

            foreach (var to in to2s)
            {
                new FacilityTableOfficer { NurseFacility = nf2, positionId = to };
            }
        }

        if (!String.IsNullOrEmpty(f3))
        {
            var nf3 = new NurseFacility
            {
                Nurse = up.Nurse,
                facilityId = int.Parse(f3),
                employmentTypeId = (!String.IsNullOrEmpty(et3)) ? byte.Parse(et3) : (byte?)null,
                priority = 3
            };


            foreach (var lp in lp3s)
            {
                new FacilityLocalPosition { NurseFacility = nf3, positionId = lp };
            }

            foreach (var to in to3s)
            {
                new FacilityTableOfficer { NurseFacility = nf3, positionId = to };
            }
        }
        
        #endregion

        #region CommitteeData
        
        //District based/multiple positions can be held
        var buglm = (!String.IsNullOrEmpty(hdnBuglmPosition.Value)) ? hdnBuglmPosition.Value.Split(',').Select(byte.Parse).ToList() : new List<byte> { };
        
        //Region based
        var bursary = (!String.IsNullOrEmpty(ddlBursaryPosition.SelectedValue)) ? byte.Parse(ddlBursaryPosition.SelectedValue) : (byte?)null;
        var unionDisc = (!String.IsNullOrEmpty(ddlUnionDisciplinePosition.SelectedValue)) ? byte.Parse(ddlUnionDisciplinePosition.SelectedValue) : (byte?)null;
        var unionDiscAppeal = (!String.IsNullOrEmpty(ddlUnionDisciplineAppealPosition.SelectedValue)) ? byte.Parse(ddlUnionDisciplineAppealPosition.SelectedValue) : (byte?)null;

        //Provincial 
        var boardOfDirectors = (!String.IsNullOrEmpty(ddlBoardOfDirectorsPosition.SelectedValue)) ? byte.Parse(ddlBoardOfDirectorsPosition.SelectedValue) : (byte?)null;
        var negotiating = (!String.IsNullOrEmpty(ddlNegotiatingPosition.SelectedValue)) ? byte.Parse(ddlNegotiatingPosition.SelectedValue) : (byte?)null;
        var agm = (!String.IsNullOrEmpty(ddlAgmPosition.SelectedValue)) ? byte.Parse(ddlAgmPosition.SelectedValue) : (byte?)null;
        var constitution = (!String.IsNullOrEmpty(ddlConstitutionPosition.SelectedValue)) ? byte.Parse(ddlConstitutionPosition.SelectedValue) : (byte?)null;
        var education = (!String.IsNullOrEmpty(ddlEducationPosition.SelectedValue)) ? byte.Parse(ddlEducationPosition.SelectedValue) : (byte?)null;
        var finance = (!String.IsNullOrEmpty(ddlFinancePosition.SelectedValue)) ? byte.Parse(ddlFinancePosition.SelectedValue) : (byte?)null;
        var personnel = (!String.IsNullOrEmpty(ddlPersonnelPosition.SelectedValue)) ? byte.Parse(ddlPersonnelPosition.SelectedValue) : (byte?)null;

        var primaryFacility = GetFacility(f1);

        _mBs.DeleteCommitteeData(myId);
        
        foreach (var bp in buglm)
        {
            new NurseCommittee
                {
                    Nurse = up.Nurse,
                    committeeId = (byte) CommitteeEnum.Buglm,
                    positionId = Convert.ToByte(bp%100),
                    //districtId = int.Parse(hdnCommitteeDistrictId.Value),
                    districtId = int.Parse(primaryFacility.districtId),
                    isAlternate = (bp > 100)    
                };
        }
        
        if (bursary != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.Bursary,
                positionId = Convert.ToByte(bursary.Value % 100),
                regionId = int.Parse(primaryFacility.regionId),
                isAlternate = (bursary.Value > 100)
            };
        }

        if (unionDisc != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.UnionDiscipline,
                positionId = Convert.ToByte(unionDisc.Value % 100),
                regionId = int.Parse(primaryFacility.regionId),
                isAlternate = (unionDisc.Value > 100)
            };
        }

        if (unionDiscAppeal != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.UnionDisciplineAppeal,
                positionId = Convert.ToByte(unionDiscAppeal.Value % 100),
                regionId = int.Parse(primaryFacility.regionId),
                isAlternate = (unionDiscAppeal.Value > 100)
            };
        }

        if (boardOfDirectors != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.BoardOfDirectors,
                positionId = Convert.ToByte(boardOfDirectors.Value % 100),
                isAlternate = (boardOfDirectors.Value > 100)
            };
        }

        if (negotiating != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.Negotiating,
                positionId = Convert.ToByte(negotiating.Value % 100),
                isAlternate = (negotiating.Value > 100)
            };
        }

        if (agm != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.Agm,
                positionId = Convert.ToByte(agm.Value % 100),
                isAlternate = (agm.Value > 100)
            };
        }

        if (constitution != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.Constitution,
                positionId = Convert.ToByte(constitution.Value % 100),
                isAlternate = (constitution.Value > 100)
            };
        }

        if (education != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.Education,
                positionId = Convert.ToByte(education.Value % 100),
                isAlternate = (education.Value > 100)
            };
        }

        if (finance != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.Finance,
                positionId = Convert.ToByte(finance.Value % 100),
                isAlternate = (finance.Value > 100)
            };
        }

        if (personnel != null)
        {
            new NurseCommittee
            {
                Nurse = up.Nurse,
                committeeId = (byte)CommitteeEnum.Personnel,
                positionId = Convert.ToByte(personnel.Value % 100),
                isAlternate = (personnel.Value > 100)
            };
        }

        // communication options
        var commOps = (!String.IsNullOrEmpty(hdnCommunicationOptions.Value)) ? hdnCommunicationOptions.Value.Split(',').Select(byte.Parse).ToList() : new List<byte> { };
        _mBs.DeleteCommunicationOptionsData(myId);

        foreach (var co in commOps)
        {
            new NurseOptOut
            {
                Nurse = up.Nurse,
                 optOutId = Convert.ToInt16(co)
            };
        }

        #endregion

        _mBs.ProcessNurseProfile(up);

        hdnNurseId.Value = up.id.ToString();

        //referesh the committee lists so any new board/committee designations for the nures will be visible in the UI lists
        InstantiateCommitteeData();

    }

    private bool IsCommitteePositionAvailable(List<GetFilledCommitteePositionsResult> fcpl, int committeeId, int positionId, bool isAlternate)
    {
        return IsCommitteePositionAvailable(fcpl, committeeId, positionId, isAlternate, null, null);
    }

    private bool IsCommitteePositionAvailable(List<GetFilledCommitteePositionsResult> fcpl, int committeeId, int positionId, bool isAlternate, byte? regionId, byte? districtId)
    {
        var myCount = (from cp in fcpl
                       where cp.committeeId == committeeId && cp.positionId == positionId && cp.isAlternate == isAlternate && (!regionId.HasValue || cp.regionId == regionId) && (!districtId.HasValue || cp.districtId == districtId)
                       select cp).Count();
        return (myCount == 0);
    }



    private void AddError(string formField, string errorMessage)
    {
        if (!_formErrors.ContainsKey(formField))
        {
            _formErrors.Add(formField, new List<string>());
        }
        _formErrors[formField].Add(errorMessage);
    }

    private bool ValidateNurse()
    {
        var localPositions = _mBs.GetLocalPositions();
        var tableOfficerPositions = _mBs.GetTableOfficerPositions();
        var committeePositions = _mBs.GetAllCommitteePositions();

        var nurseId = (hdnNurseId.Value != "") ? Int32.Parse(hdnNurseId.Value) : -1;
        
        //mandatory fields
        //username, first name, last name, gender, line 1, city, province, postal code, designation, employment status
        #region ManadatoryFields
        
        if (nurseId > -1 && String.IsNullOrEmpty(tbUserName.Text))
        {
            AddError(tbUserName.ID, "Field is required.");
        }
        if (String.IsNullOrEmpty(tbFirstName.Text))
        {
            AddError(tbFirstName.ID, "Field is required.");
        }
        if (String.IsNullOrEmpty(tbLastName.Text))
        {
            AddError(tbLastName.ID, "Field is required.");
        }
        //if (String.IsNullOrEmpty(ddlGender.SelectedValue))
        //{
        //    AddError(ddlGender.ID, "Field is required.");
        //}

        //if (String.IsNullOrEmpty(tbAddressLine1.Text))
        //{
        //    AddError(tbAddressLine1.ID, "Field is required.");
        //}

        //if (String.IsNullOrEmpty(tbCity.Text))
        //{
        //    AddError(tbCity.ID, "Field is required.");
        //}

        //if (String.IsNullOrEmpty(ddlProvince.SelectedValue))
        //{
        //    AddError(ddlProvince.ID, "Field is required.");
        //}
        //if (String.IsNullOrEmpty(tbPostalCode.Text))
        //{
        //    AddError(tbPostalCode.ID, "Field is required.");
        //}


        if (!String.IsNullOrEmpty(tbPostalCode.Text) && !_postalCodeRegex.IsMatch(tbPostalCode.Text))
        {
            AddError(tbPostalCode.ID, "Postal code is not in the proper format.");
        }

        if (!String.IsNullOrEmpty(tbEmail.Text) && !_emailRegex.IsMatch(tbEmail.Text))
        {
            AddError(tbEmail.ID, "Email address is not in the proper format.");
        }

        if (!String.IsNullOrEmpty(tbSecondaryEmail.Text) && !_emailRegex.IsMatch(tbSecondaryEmail.Text))
        {
            AddError(tbSecondaryEmail.ID, "Secondary email address is not in the proper format.");
        }

        //if (String.IsNullOrEmpty(ddlDesignation.SelectedValue))
        //{
        //    AddError(ddlDesignation.ID, "Field is required.");
        //}
        if (String.IsNullOrEmpty(ddlStatus.SelectedValue))
        {
            AddError(ddlStatus.ID, "Field is required.");
        }

        if (nurseId > -1 && !_passwordRegex.IsMatch(tbPassword.Text))
        {
            AddError(tbPassword.ID, "Password must be between 6 and 20 alphanumeric characters.");
        }

        if (nurseId > -1 && String.Compare(tbPassword.Text, tbConfirmPassword.Text) != 0)
        {
            AddError(tbConfirmPassword.ID, "Password and confirm password fields must match.");
        }

        #endregion

        if (!String.IsNullOrEmpty(tbPhoneNumber1.Text) && !_phoneRegex.IsMatch(tbPhoneNumber1.Text))
        {
            AddError(tbPhoneNumber1.ID, "Phone Number is not in the proper format.");
        }

        if (!String.IsNullOrEmpty(tbPhoneNumber2.Text) && !_phoneRegex.IsMatch(tbPhoneNumber2.Text))
        {
            AddError(tbPhoneNumber2.ID, "Phone Number is not in the proper format.");
        }

        if (!String.IsNullOrEmpty(tbPhoneNumber3.Text) && !_phoneRegex.IsMatch(tbPhoneNumber3.Text))
        {
            AddError(tbPhoneNumber3.ID, "Phone Number is not in the proper format.");
        }

        if (!String.IsNullOrEmpty(tbPhoneNumber4.Text) && !_phoneRegex.IsMatch(tbPhoneNumber4.Text))
        {
            AddError(tbPhoneNumber4.ID, "Phone Number is not in the proper format.");
        }

        ///////////////////////////
        //if (String.IsNullOrEmpty(tbPhoneNumber1.Text) && !String.IsNullOrEmpty(tbPhoneExt1.Text))
        //{
        //    AddError(tbPhoneExt1.ID, "Phone number must be provided.");
        //}

        if (String.IsNullOrEmpty(tbPhoneNumber2.Text) && !String.IsNullOrEmpty(tbPhoneExt2.Text))
        {
            AddError(tbPhoneExt2.ID, "Phone number must be provided.");
        }

        //if (String.IsNullOrEmpty(tbPhoneNumber3.Text) && !String.IsNullOrEmpty(tbPhoneExt3.Text))
        //{
        //    AddError(tbPhoneExt3.ID, "Phone number must be provided.");
        //}

        //if (String.IsNullOrEmpty(tbPhoneNumber4.Text) && !String.IsNullOrEmpty(tbPhoneExt4.Text))
        //{
        //    AddError(tbPhoneExt4.ID, "Phone number must be provided.");
        //}


        // can't select the same facility more than once for primary, secondary, tertiary facilities

        string primaryfacility = ddlFacility1.SelectedValue;
        if (String.IsNullOrEmpty(primaryfacility))
        {
            AddError(ddlFacility1.ID, "Primary facility is required.");
        }

        if (!String.IsNullOrEmpty(primaryfacility))
        {
            if (ddlFacility2.SelectedValue == primaryfacility)
                AddError(ddlFacility2.ID, "Secondary facility must be unique.");

            if (ddlFacility3.SelectedValue == primaryfacility)
                AddError(ddlFacility3.ID, "Tertiary facility must be unique.");

        }
        string secondaryfacility = ddlFacility2.SelectedValue;
        if (!String.IsNullOrEmpty(secondaryfacility))
        {
            if (ddlFacility3.SelectedValue == secondaryfacility)
                AddError(ddlFacility3.ID, "Tertiary facility must be unique.");
        }



        //District based/multiple positions can be held
        var buglm = (!String.IsNullOrEmpty(hdnBuglmPosition.Value)) ? hdnBuglmPosition.Value.Split(',').Select(byte.Parse).ToList() : new List<byte> { };

        //Region based
        var bursary = (!String.IsNullOrEmpty(ddlBursaryPosition.SelectedValue)) ? byte.Parse(ddlBursaryPosition.SelectedValue) : (byte?)null;
        var unionDisc = (!String.IsNullOrEmpty(ddlUnionDisciplinePosition.SelectedValue)) ? byte.Parse(ddlUnionDisciplinePosition.SelectedValue) : (byte?)null;
        var unionDiscAppeal = (!String.IsNullOrEmpty(ddlUnionDisciplineAppealPosition.SelectedValue)) ? byte.Parse(ddlUnionDisciplineAppealPosition.SelectedValue) : (byte?)null;

        //Provincial 
        var boardOfDirectors = (!String.IsNullOrEmpty(ddlBoardOfDirectorsPosition.SelectedValue)) ? byte.Parse(ddlBoardOfDirectorsPosition.SelectedValue) : (byte?)null;
        var negotiating = (!String.IsNullOrEmpty(ddlNegotiatingPosition.SelectedValue)) ? byte.Parse(ddlNegotiatingPosition.SelectedValue) : (byte?)null;
        var agm = (!String.IsNullOrEmpty(ddlAgmPosition.SelectedValue)) ? byte.Parse(ddlAgmPosition.SelectedValue) : (byte?)null;
        var constitution = (!String.IsNullOrEmpty(ddlConstitutionPosition.SelectedValue)) ? byte.Parse(ddlConstitutionPosition.SelectedValue) : (byte?)null;
        var education = (!String.IsNullOrEmpty(ddlEducationPosition.SelectedValue)) ? byte.Parse(ddlEducationPosition.SelectedValue) : (byte?)null;
        var finance = (!String.IsNullOrEmpty(ddlFinancePosition.SelectedValue)) ? byte.Parse(ddlFinancePosition.SelectedValue) : (byte?)null;
        var personnel = (!String.IsNullOrEmpty(ddlPersonnelPosition.SelectedValue)) ? byte.Parse(ddlPersonnelPosition.SelectedValue) : (byte?)null;

        var fcpl = _mBs.GetFilledCommitteePositions(nurseId);

        if (buglm.Count > 0)
        {
            foreach (var p in buglm)
            {
                ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlBuglmPosition.ID, CommitteeEnum.Buglm, p, null, byte.Parse(hdnCommitteeDistrictId.Value));
            }
        }
        
        if (bursary != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlBursaryPosition.ID, CommitteeEnum.Bursary, bursary.Value, byte.Parse(hdnCommitteeRegionId.Value), null);
        }

        if (unionDisc != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlUnionDisciplinePosition.ID, CommitteeEnum.UnionDiscipline, unionDisc.Value, byte.Parse(hdnCommitteeRegionId.Value), null);
        }

//        if (unionDiscAppeal != null && !IsCommitteePositionAvailable(fcpl, (int)CommitteeEnum.UnionDisciplineAppeal, Convert.ToByte(unionDiscAppeal % 100), (unionDiscAppeal > 100)))
        if (unionDiscAppeal != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlUnionDisciplineAppealPosition.ID, CommitteeEnum.UnionDisciplineAppeal, unionDiscAppeal.Value, byte.Parse(hdnCommitteeRegionId.Value), null);
        }

        //if (boardOfDirectors != null && !IsCommitteePositionAvailable(fcpl, (int)CommitteeEnum.BoardOfDirectors, Convert.ToByte(boardOfDirectors % 100), (boardOfDirectors > 100)))
        if (boardOfDirectors != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlBoardOfDirectorsPosition.ID, CommitteeEnum.BoardOfDirectors, boardOfDirectors.Value);
        }

        if (negotiating != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlNegotiatingPosition.ID, CommitteeEnum.Negotiating, negotiating.Value);
        }

        if (agm != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlAgmPosition.ID, CommitteeEnum.Agm, agm.Value);
        }

        if (constitution != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlConstitutionPosition.ID, CommitteeEnum.Constitution, constitution.Value);
        }

        if (education != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlEducationPosition.ID, CommitteeEnum.Education, education.Value);
        }

        if (finance != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlFinancePosition.ID, CommitteeEnum.Finance, finance.Value);
        }

        if (personnel != null)
        {
            ValidateCommitteePositionAvailability(fcpl, committeePositions, ddlPersonnelPosition.ID, CommitteeEnum.Personnel, personnel.Value);
        }
        

        var primaryFacility = (!String.IsNullOrEmpty(ddlFacility1.SelectedValue)) ? _mBs.GetFacility(Int32.Parse(ddlFacility1.SelectedValue)) : null;

        if (ddlBoardOfDirectorsPosition.SelectedValue == ((byte)PositionEnum.VpCommunityCare).ToString())
        {
            if (primaryFacility == null || primaryFacility.facilityTypeId != (int)FacilityTypeEnum.CommunityCare)
            {
                AddError(ddlBoardOfDirectorsPosition.ID, "The selected Board of Directors position must be filled by a nurse whose primary facility is within that sector.");
                //success = false;
            }
        }

        if (ddlBoardOfDirectorsPosition.SelectedValue == ((int)PositionEnum.VpLtc).ToString())
        {
            if (primaryFacility == null || primaryFacility.facilityTypeId != (int)FacilityTypeEnum.LongTermCare)
            {
                AddError(ddlBoardOfDirectorsPosition.ID, "The selected Board of Directors position must be filled by a nurse whose primary facility is within that sector.");
                //success = false;
            }
        }

        if (ddlBoardOfDirectorsPosition.SelectedValue == ((int)PositionEnum.VpLpnGrad).ToString())
        {
            if (primaryFacility == null || primaryFacility.facilityTypeId != (int)FacilityTypeEnum.AcuteCare)
            {
                //success = false;
                AddError(ddlBoardOfDirectorsPosition.ID, "The selected Board of Directors position must be filled by a nurse whose primary facility is within that sector.");
            }
        }

        if (ddlBoardOfDirectorsPosition.SelectedValue == ((int)PositionEnum.VpIwk).ToString())
        {
            //change request for new IWK VP position on board of directors; nurse must work at IWK
            if (primaryFacility == null || primaryFacility.id != 46)
            {
                //success = false;
                AddError(ddlBoardOfDirectorsPosition.ID, "The selected Board of Directors position must be filled by a nurse whose primary facility is the IWK Health Centre.");
            }
        }

        if (ddlNegotiatingPosition.SelectedValue == ((int)PositionEnum.Northern).ToString())
        {
            if (primaryFacility == null || primaryFacility.District.regionId != (int)RegionEnum.Northern)
            {
                //success = false;
                AddError(ddlNegotiatingPosition.ID, "The selected Negotiating position must be filled by a nurse whose primary facility is within that region.");
            }
        }

        if (ddlNegotiatingPosition.SelectedValue == ((int)PositionEnum.Central).ToString())
        {
            if (primaryFacility == null || primaryFacility.District.regionId != (int)RegionEnum.Central)
            {
                //success = false;
                AddError(ddlNegotiatingPosition.ID, "The selected Negotiating position must be filled by a nurse whose primary facility is within that region.");
            }
        }

        if (ddlNegotiatingPosition.SelectedValue == ((int)PositionEnum.Eastern).ToString())
        {
            if (primaryFacility == null || primaryFacility.District.regionId != (int)RegionEnum.Eastern)
            {
                //success = false;
                AddError(ddlNegotiatingPosition.ID, "The selected Negotiating position must be filled by a nurse whose primary facility is within that region.");
            }
        }

        if (ddlNegotiatingPosition.SelectedValue == ((int)PositionEnum.Western).ToString())
        {
            if (primaryFacility == null || primaryFacility.District.regionId != (int)RegionEnum.Western)
            {
                AddError(ddlNegotiatingPosition.ID, "The selected Negotiating position must be filled by a nurse whose primary facility is within that region.");
                //success = false;
            }
        }

        if (ddlNegotiatingPosition.SelectedValue == ((int)PositionEnum.CommunityCare).ToString())
        {
            if (primaryFacility == null || primaryFacility.facilityTypeId != (int)FacilityTypeEnum.CommunityCare)
            {
                //success = false;
                AddError(ddlNegotiatingPosition.ID, "The selected Negotiating position must be filled by a nurse whose primary facility is within that sector.");
            }
        }

        if (ddlNegotiatingPosition.SelectedValue == ((int)PositionEnum.Lpn).ToString())
        {
            if (primaryFacility == null || primaryFacility.facilityTypeId != (int)FacilityTypeEnum.AcuteCare)
            {
                //success = false;
                AddError(ddlNegotiatingPosition.ID, "The selected Negotiating position must be filled by a nurse whose primary facility is within that sector.");
            }
        }

        if (ddlNegotiatingPosition.SelectedValue == ((int)PositionEnum.Ltc).ToString())
        {
            if (primaryFacility == null || primaryFacility.facilityTypeId != (int)FacilityTypeEnum.LongTermCare)
            {
                //success = false;
                AddError(ddlNegotiatingPosition.ID, "The selected Negotiating position must be filled by a nurse whose primary facility is within that sector.");
            }
        }

        if (ddlNegotiatingPosition.SelectedValue == ((int)PositionEnum.Iwk).ToString())
        {
            if (primaryFacility == null || primaryFacility.districtId != (int)DistrictEnum.Iwk)
            {
                //success = false;
                AddError(ddlNegotiatingPosition.ID, "The selected Negotiating position must be filled by a nurse whose primary facility is within that district.");
            }
        }

        
        if (!String.IsNullOrEmpty(ddlPersonnelPosition.SelectedValue))
        {
            if (String.IsNullOrEmpty(ddlBoardOfDirectorsPosition.SelectedValue) && ddlNegotiatingPosition.SelectedValue != ((int)PositionEnum.President).ToString() && ddlNegotiatingPosition.SelectedValue != ((int)PositionEnum.VicePresident).ToString() && ddlBursaryPosition.SelectedValue != ((int)PositionEnum.Chair).ToString())
            {
                //success = false;
                AddError(ddlPersonnelPosition.ID, "The selected Personnel position must be filled by a member of the Board of Directors.");
            }
        }

        
        //Only one board of directors position may be held
        var a = ddlBoardOfDirectorsPosition.SelectedValue != "";
        var b = ddlFinancePosition.SelectedValue == ((int) PositionEnum.Chair).ToString();
        var c = ddlNegotiatingPosition.SelectedValue == ((int) PositionEnum.President).ToString() || ddlNegotiatingPosition.SelectedValue == ((int) PositionEnum.VicePresident).ToString();
        var d = ddlBursaryPosition.SelectedValue == ((int) PositionEnum.Chair).ToString();
        
        //only one may be true
        if ((!(a ^ b ^ c ^ d)) && (a || b || c || d))
        {
            //choose a field to show the error
            var fieldId = a ? ddlBoardOfDirectorsPosition.ID : b ? ddlFinancePosition.ID : c ? ddlNegotiatingPosition.ID : ddlBursaryPosition.ID;
            AddError(fieldId, "An individual may only hold a single position on the Board of Directors.");
        }

        // if any of the facility drop downs are selected then a corresponding employment type must be seleted from the associated drop down
        //if (ddlFacility1.SelectedValue != "" && ddlEmploymentType1.SelectedValue == "")
        //{
        //    AddError(ddlEmploymentType1.ID, "Field is required.");

        //}

        //if (ddlFacility2.SelectedValue != "" && ddlEmploymentType2.SelectedValue == "")
        //{
        //    AddError(ddlEmploymentType2.ID, "Field is required.");
        //}

        //if (ddlFacility3.SelectedValue != "" && ddlEmploymentType3.SelectedValue == "")
        //{
        //    AddError(ddlEmploymentType3.ID, "Field is required.");
        //}


        return _formErrors.Keys.Count <= 0;
    }

    private void ValidateCommitteePositionAvailability (List<GetFilledCommitteePositionsResult> fcpl , List<Nsnu.DataAccess.CommitteePosition> cpl ,string inputId, CommitteeEnum ce, byte positionId)
    {
        ValidateCommitteePositionAvailability(fcpl, cpl, inputId, ce, positionId, null, null);
    }

    private void ValidateCommitteePositionAvailability(List<GetFilledCommitteePositionsResult> fcpl, List<Nsnu.DataAccess.CommitteePosition> cpl, string inputId, CommitteeEnum ce, byte positionId, byte? regionId, byte? districtId)
    {
        if (!IsCommitteePositionAvailable(fcpl, (int)ce, Convert.ToByte(positionId % 100), (positionId > 100), regionId, districtId))
        {
            AddError(inputId, String.Format("The {0}{1} position is already filled.", (from cp in cpl
                                                                                       where cp.positionId == Convert.ToByte(positionId % 100)
                                                                                       select cp.refCommitteePosition.positionName).FirstOrDefault(), (positionId > 100) ? " (Alt.)" : ""));
        }
    }

    private List<CommitteeMemberVo> GenerateCommitteeMemberList(CommitteeEnum ce, List<NurseCommittee> ncl)
    {
        return GenerateCommitteeMemberList(ce, ncl, null, null);
    }

    private List<CommitteeMemberVo> GenerateCommitteeMemberList (CommitteeEnum ce, List<NurseCommittee> ncl, byte? regionId, byte? districtId)
    {
        return ncl.Where(cm => cm.committeeId == (byte)ce && cm.regionId == regionId && cm.districtId == districtId).Select(cm => new CommitteeMemberVo
        {
            firstName = cm.Nurse.firstName,
            lastName = cm.Nurse.lastName,
            positionName = String.Format("{0}{1}", cm.CommitteePosition.refCommitteePosition.positionName, cm.isAlternate ? " (Alt.)" : "")
        }).OrderBy(cm => cm.positionName).ToList();
    }

    private List<CommitteeMemberVo> GenerateBoardOfDirectorsMemberList(List<NurseCommittee> ncl)
    {
        var q = ncl.Where(cm => cm.committeeId == (byte) CommitteeEnum.BoardOfDirectors).Select(cm => new CommitteeMemberVo
                                                                                                      {
                                                                                                          firstName = cm.Nurse.firstName,
                                                                                                          lastName = cm.Nurse.lastName,
                                                                                                          positionName = String.Format("{0}{1}",cm.CommitteePosition.refCommitteePosition.positionName, cm.isAlternate? " (Alt.)" : "")
                                                                                                      }).ToList();

        q.AddRange(ncl.Where(cm => cm.committeeId == (byte)CommitteeEnum.Negotiating && cm.positionId == (byte)PositionEnum.President).Select(cm => new CommitteeMemberVo
        {
            firstName = cm.Nurse.firstName,
            lastName = cm.Nurse.lastName,
            positionName = String.Format("{0}{1}", ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.NegotiatingPresident), cm.isAlternate ? " (Alt.)" : "")
        }).ToList());


        q.AddRange(ncl.Where(cm => cm.committeeId == (byte)CommitteeEnum.Negotiating && cm.positionId == (byte)PositionEnum.VicePresident).Select(cm => new CommitteeMemberVo
        {
            firstName = cm.Nurse.firstName,
            lastName = cm.Nurse.lastName,
            positionName = String.Format("{0}{1}", ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.NegotiatingVicePresident), cm.isAlternate ? " (Alt.)" : "")
        }).ToList());

        q.AddRange(ncl.Where(cm => cm.committeeId == (byte)CommitteeEnum.Finance && cm.positionId == (byte)PositionEnum.Chair).Select(cm => new CommitteeMemberVo
        {
            firstName = cm.Nurse.firstName,
            lastName = cm.Nurse.lastName,
            positionName = String.Format("{0}{1}", ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.FinanceChair), cm.isAlternate ? " (Alt.)" : "")
        }).ToList());

        q.AddRange(ncl.Where(cm => cm.committeeId == (byte)CommitteeEnum.Bursary && cm.positionId == (byte)PositionEnum.Chair && cm.regionId == 1).Select(cm => new CommitteeMemberVo
        {
            firstName = cm.Nurse.firstName,
            lastName = cm.Nurse.lastName,
            positionName = String.Format("{0}{1}", ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.BursaryChairCentral), cm.isAlternate ? " (Alt.)" : "")
        }).ToList());

        q.AddRange(ncl.Where(cm => cm.committeeId == (byte)CommitteeEnum.Bursary && cm.positionId == (byte)PositionEnum.Chair && cm.regionId == 2).Select(cm => new CommitteeMemberVo
        {
            firstName = cm.Nurse.firstName,
            lastName = cm.Nurse.lastName,
            positionName = String.Format("{0}{1}", ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.BursaryChairWestern), cm.isAlternate ? " (Alt.)" : "")
        }).ToList());

        q.AddRange(ncl.Where(cm => cm.committeeId == (byte)CommitteeEnum.Bursary && cm.positionId == (byte)PositionEnum.Chair && cm.regionId == 3).Select(cm => new CommitteeMemberVo
        {
            firstName = cm.Nurse.firstName,
            lastName = cm.Nurse.lastName,
            positionName = String.Format("{0}{1}", ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.BursaryChairNorthern), cm.isAlternate ? " (Alt.)" : "")
        }).ToList());

        q.AddRange(ncl.Where(cm => cm.committeeId == (byte)CommitteeEnum.Bursary && cm.positionId == (byte)PositionEnum.Chair && cm.regionId == 4).Select(cm => new CommitteeMemberVo
        {
            firstName = cm.Nurse.firstName,
            lastName = cm.Nurse.lastName,
            positionName = String.Format("{0}{1}", ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.BursaryChairEastern), cm.isAlternate ? " (Alt.)" : "")
        }).ToList());


        return q.OrderBy(p => p.positionName).ToList();


    }

    #endregion

    #region Value objects

    public class NoteVo
    {
        public string noteId { get; set; }
        public string noteBody { get; set; }
        public string creationDate { get; set; }
        public string createdBy { get; set; }
    }

    public class VersionVo
    {
        public string versionId { get; set; }
        public string modificationDate { get; set; }
        public string modifiedBy { get; set; }
    }

    public class FacilityVo
    {
        public string facilityId { get; set; }
        public string regionId { get; set; }
        public string districtId { get; set; }
        public string regionName { get; set; }
        public string districtName { get; set; }
    }

    public class CommitteeMemberVo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string positionName { get; set; }
    }

    #endregion
}
