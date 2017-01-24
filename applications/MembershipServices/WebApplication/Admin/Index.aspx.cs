using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nsnu.DataAccess;
using Nsnu.DataAccess.Enumerations;
using Nsnu.MembershipServices;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WebApplication.Utilities;
using Committee = Nsnu.DataAccess.Enumerations.Committee;
using CommitteePosition = Nsnu.DataAccess.Enumerations.CommitteePosition;
using District = Nsnu.DataAccess.Enumerations.District;
using Region = Nsnu.DataAccess.Enumerations.Region;

namespace WebApplication.Admin
{
    public partial class Index : System.Web.UI.Page
    {
        private string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
                     "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",
                     "W", "X", "Y", "Z"};

        private MembershipBs _mBs = new MembershipBs();

        protected void Page_Load(object sender, EventArgs e)
        {
            lvNurses.PagePropertiesChanged += new EventHandler(lvNurses_PagePropertiesChanged);

            if (!IsPostBack)
            {
                InitializeForm();

                if (MySessionVariables.SearchParameters.HasValue)
                {
                    PopulateSearchForm();
                }

                rptLetters.DataSource = letters.ToList();
                rptLetters.DataBind();

                lvNurses.DataSource = GenerateIndexList();
                lvNurses.DataBind();
            }


            //get the logged on user and see if they are readonly
            if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.IsInRole("readonly"))
            {
                this.lnkAddNewNurse.Disabled = true;
                this.lnkAddNewNurse.Visible = false;
                //pnlLoggedIn.Visible = true;
               // litUserName.Text = HttpContext.Current.User.Identity.Name;
            }

        }

        private void PopulateSearchForm()
        {
            var mySearchParams = MySessionVariables.SearchParameters.Value;
            
            ddlCommunicationOption.SelectedValue = (mySearchParams.communicationOptionId.HasValue) ? mySearchParams.communicationOptionId.Value.ToString() : "";
            ddlDesignation.SelectedValue = (mySearchParams.designationId.HasValue) ? mySearchParams.designationId.Value.ToString() : "";
            ddlDistrict.SelectedValue = (mySearchParams.districtId.HasValue) ? mySearchParams.districtId.Value.ToString() : "";
            tbEmail.Text = mySearchParams.email;
            ddlEmployerGroup.SelectedValue = (mySearchParams.employerGroupId.HasValue) ? mySearchParams.employerGroupId.Value.ToString() : "";
            //ddlStatus.SelectedValue = (mySearchParams.employmentStatusId.HasValue) ? mySearchParams.employmentStatusId.Value.ToString() : "";
            ddlFacility.SelectedValue = (mySearchParams.facilityId.HasValue) ? mySearchParams.facilityId.Value.ToString() : "";
            tbFirstName.Text = mySearchParams.firstName;
            tbLastName.Text = mySearchParams.lastName;
            tbLine1.Text = mySearchParams.line1;
            //ddlLocalTableOfficerPosition.SelectedValue = (mySearchParams.localPositionId.HasValue) ? ((mySearchParams.tableOfficerId.HasValue) ? mySearchParams.tableOfficerId.Value.ToString() : "") : "";
            tbPhone.Text = mySearchParams.phone;
            ddlRegion.SelectedValue = (mySearchParams.regionId.HasValue) ? mySearchParams.regionId.Value.ToString() : "";
            ddlSector.SelectedValue = (mySearchParams.sectorId.HasValue) ? mySearchParams.sectorId.Value.ToString() : "";
            hdnEmploymentStatusList.Value = mySearchParams.employmentStatusList;
            hdnLocalTableOfficerPositionList.Value = mySearchParams.localTableOfficerPositionList;
            hdnPositionId.Value = (mySearchParams.positionId.HasValue) ? mySearchParams.positionId.Value.ToString() : ""; 

            if (mySearchParams.committeeId.HasValue)
            {
                ddlCommittee.SelectedValue = mySearchParams.committeeId.Value.ToString();
                
                ddlPosition.DataSource = GetPositions(mySearchParams.committeeId.Value.ToString());
                ddlPosition.DataTextField = "Text";
                ddlPosition.DataValueField = "Value";
                ddlPosition.DataBind();

                ddlPosition.SelectedValue = (mySearchParams.positionId.HasValue) ? mySearchParams.positionId.Value.ToString() : "";
            }
        }

        private void InitializeForm()
        {
            ddlDesignation.DataSource = InterfaceUtils.GetEnumerationListItems<Designation>(true);
            ddlDesignation.DataTextField = "Text";
            ddlDesignation.DataValueField = "Value";
            ddlDesignation.DataBind();

            //ddlStatus.DataSource = InterfaceUtils.GetEnumerationListItems<EmploymentStatus>(true);
            //ddlStatus.DataTextField = "Text";
            //ddlStatus.DataValueField = "Value";
            //ddlStatus.DataBind();

            ddlSector.DataSource = InterfaceUtils.GetEnumerationListItems<FacilityType>(true);
            ddlSector.DataTextField = "Text";
            ddlSector.DataValueField = "Value";
            ddlSector.DataBind();

            ddlEmployerGroup.DataSource = InterfaceUtils.GetEnumerationListItems<EmployerGroup>(true);
            ddlEmployerGroup.DataTextField = "Text";
            ddlEmployerGroup.DataValueField = "Value";
            ddlEmployerGroup.DataBind();

            ddlDistrict.DataSource = InterfaceUtils.GetEnumerationListItems<District>(true);
            ddlDistrict.DataTextField = "Text";
            ddlDistrict.DataValueField = "Value";
            ddlDistrict.DataBind();

            ddlRegion.DataSource = InterfaceUtils.GetEnumerationListItems<Region>(true);
            ddlRegion.DataTextField = "Text";
            ddlRegion.DataValueField = "Value";
            ddlRegion.DataBind();

            ddlCommittee.DataSource = InterfaceUtils.GetEnumerationListItems<Committee>(true);
            ddlCommittee.DataTextField = "Text";
            ddlCommittee.DataValueField = "Value";
            ddlCommittee.DataBind();

            var facilityListItems = _mBs.GetFacilities().OrderBy(f => f.facilityName);
            ddlFacility.DataSource = facilityListItems;
            ddlFacility.DataTextField = "facilityName";
            ddlFacility.DataValueField = "id";
            ddlFacility.DataBind();

            ddlLocalTableOfficerPosition.DataSource = InterfaceUtils.GetLocalTableOfficerPositionListItems();
            ddlLocalTableOfficerPosition.DataTextField = "Text";
            ddlLocalTableOfficerPosition.DataValueField = "Value";
            ddlLocalTableOfficerPosition.DataBind();

            ddlPosition.DataSource = new List<ListItem> { new ListItem("Please Select", "") };
            ddlPosition.DataTextField = "Text";
            ddlPosition.DataValueField = "Value";
            ddlPosition.DataBind();     
        }

        public class IndexListItem
        {
            public string nurseId { get; set; }
            public string fullName { get; set; }
            public string userName { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public string email { get; set; }
            public string secondaryemail { get; set; }
            public string designation { get; set; }
            public string status { get; set; }
            public string facility { get; set; }
            public string nickname { get; set; }
            public string genderid { get; set; }
            public string birthdate { get; set; }
            public string employmentType { get; set; }
        }

        public class FacilityItem
        {
            public string facility{ get; set; }
            public string district { get; set; }
            public string region { get; set; }
            public string labourRep { get; set; }
            public string facilityType { get; set; }
            public string empGroupName { get; set; }
            public string addressline1 { get; set; }
            public string addressline2 { get; set; }
            public string city { get; set; }
            public string province { get; set; }
            public string postalcode { get; set; }
            public string phone { get; set; }
            public string casualcoverage { get; set; }
            public string lpnCoverage { get; set; }

        }

        public class PhoneListItem
        {
            public string nurseId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string workPhone { get; set; }
            public string homePhone { get; set; }
            public string cell { get; set; }
            public string fax { get; set; }
        }

        public class AddressListItem
        {
            public string nurseId { get; set; }
            public string line1 { get; set; }
            public string line2 { get; set; }
            public string city { get; set; }
            public string province { get; set; }
            public string postalCode { get; set; }
        }

        public class NurseFacilityListItem
        {
            public string nurseId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string FullTimeFacility { get; set; }
            public string PartTimeFacility { get; set; }
            public string CasualFacility { get; set; }
        }



        public enum ExportType
        {
            FullNurse = 1,
            PartialNurse = 2,
            Facility = 3
        }

        public enum NurseAddressType
        {
            Mailing = 1,
            Business = 2
        }

        [WebMethod]
        public static List<ListItem> GetPositions(string committeeId)
        {
            List<ListItem> positions = new List<ListItem>();
            
            if (String.IsNullOrEmpty(committeeId))
            {
                positions.Insert(0, new ListItem("Please Select", string.Empty));
                return positions;
            }

            var cId = int.Parse(committeeId);
            Committee c = (Committee)cId;

            positions = (c == Committee.BoardOfDirectors) ? InterfaceUtils.GetBoardOfDirectorsPositionListItems() : InterfaceUtils.GetCommitteePositionListItems(c);

            return positions.ToList();

        }

        private List<GetFacilitiesForExportResult> GetFacilitesForExport()
        {
            return _mBs.GetFacilitiesForExport();
        }


        private List<SearchUserProfilesResult> ExecuteSearch()
        {
            var designationId = (ddlDesignation.SelectedValue != "") ? byte.Parse(ddlDesignation.SelectedValue) : (byte?)null;
            //var statusId = (ddlStatus.SelectedValue != "") ? byte.Parse(ddlStatus.SelectedValue) : (byte?)null;
            var sectorId = (ddlSector.SelectedValue != "") ? byte.Parse(ddlSector.SelectedValue) : (byte?)null;
            var facilityId = (ddlFacility.SelectedValue != "") ? Int32.Parse(ddlFacility.SelectedValue) : (int?)null;
            var districtId = (ddlDistrict.SelectedValue != "") ? byte.Parse(ddlDistrict.SelectedValue) : (byte?)null;
            var regionId = (ddlRegion.SelectedValue != "") ? byte.Parse(ddlRegion.SelectedValue) : (byte?)null;
            var committeeId = (ddlCommittee.SelectedValue != "") ? byte.Parse(ddlCommittee.SelectedValue) : (byte?)null;
            //var positionId = (ddlPosition.SelectedValue != "") ? byte.Parse(ddlPosition.SelectedValue) : (byte?)null;
            var positionId = (hdnPositionId.Value != "") ? byte.Parse(hdnPositionId.Value) : (byte?)null;
            var communicationOptionId = (ddlCommunicationOption.SelectedValue != "") ? Int32.Parse(ddlCommunicationOption.SelectedValue) : (int?)null;
            var employerGroupId = (ddlEmployerGroup.SelectedValue != "") ? byte.Parse(ddlEmployerGroup.SelectedValue) : (byte?)null;
            


            bool? facilityCasualCoverage = (ddlMiscellaneousFilter.SelectedValue == "1") ? true : (bool?)null;
            bool? facilityLpnCoverage = (ddlMiscellaneousFilter.SelectedValue == "2") ? true : (bool?)null;

            bool? isAlternate = false;
          //  positionId = 102;

            if (positionId != null && positionId > 100)
            {
               // (int)positionId = (int)positionId % 100;
                int posID = (int)positionId;
                posID = posID % 100;
                positionId = (byte?)posID;
                isAlternate = true;
            }

            var localTableOfficerPosition = hdnLocalTableOfficerPositionList.Value;

            var localPositionList = (localTableOfficerPosition != "") ? String.Join(",", localTableOfficerPosition.Split(',').Where(y => int.Parse(y) <= 100)) : ""; 
            var tableOfficerPositionList = (localTableOfficerPosition != "") ? String.Join(",", localTableOfficerPosition.Split(',').Where(y => int.Parse(y) > 100).Select(w => int.Parse(w)%100)) : ""; 

            MySessionVariables.SearchParameters.Clear();
            MySessionVariables.CurrentIndex.Value = 0;

            var mySearchParams = new SearchParameters
                                     {
                                         committeeId = committeeId,
                                         communicationOptionId = communicationOptionId,
                                         designationId = designationId,
                                         districtId = districtId,
                                         email = tbEmail.Text,
                                         employerGroupId = employerGroupId,
                                         facilityId = facilityId,
                                         firstName = tbFirstName.Text,
                                         lastName = tbLastName.Text,
                                         line1 = tbLine1.Text,
                                         phone = tbPhone.Text,
                                         positionId = positionId,
                                         regionId = regionId,
                                         sectorId = sectorId,
                                         employmentStatusList = hdnEmploymentStatusList.Value,
                                         localTableOfficerPositionList = localTableOfficerPosition
                                     };

            MySessionVariables.SearchParameters.Value = mySearchParams;
            
            return _mBs.SearchUserProfiles(tbUserName.Text, tbFirstName.Text, tbLastName.Text, tbEmail.Text,
                                             designationId, sectorId, facilityId, districtId, regionId,
                                             committeeId, positionId, communicationOptionId, employerGroupId,
                                             facilityCasualCoverage, facilityLpnCoverage, hdnLetterFilter.Value, isAlternate, tbLine1.Text, tbPhone.Text, hdnEmploymentStatusList.Value, localPositionList, tableOfficerPositionList);
            
        }

        private List<IndexListItem> ConvertNurseList(List<SearchUserProfilesResult> nl)
        {
            List<IndexListItem> l = new List<IndexListItem>();

            foreach (var n in nl)
            {
                var ii = new IndexListItem();
                
                
                ii.nurseId = n.userId.ToString();
                ii.userName = n.username;
                ii.fullName = String.Format("{0}, {1}{2}", n.lastName, n.firstName, (!String.IsNullOrEmpty(n.initial)) ? String.Format(" {0}.", n.initial) : "");
                ii.phone = n.phone;
                ii.email = n.email;
                ii.address = GenerateAddressString(n.line1, n.line2, n.city, n.provinceId, n.postalCode);
                ii.designation = (n.nurseDesignationId != null) ? ResourceUtils.GetEnumLabel((Designation)n.nurseDesignationId) : "";
                ii.status = (n.employmentStatusId != null) ? ResourceUtils.GetEnumLabel((EmploymentStatus)n.employmentStatusId) : "";
                ii.facility = n.primaryFacility;
                ii.employmentType = (n.primaryEmploymentTypeId != null) ? ResourceUtils.GetEnumLabel((EmploymentType)n.primaryEmploymentTypeId) : "";
                 
                //ii.employmentType = "why";
                
                l.Add(ii);
                
            }

            return l;
        }

        private string GenerateAddressString (string line1, string line2, string city, string provinceId, string postalCode)
        {
            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrEmpty(line1))
            {
                sb.AppendFormat("{0}", line1);
            }
            
            if (!String.IsNullOrEmpty(line2))
            {
                sb.AppendFormat("<br/>{0}", line2);
            }

            if (!String.IsNullOrEmpty(city))
            {
                sb.AppendFormat("<br/>{0}", city);
            }

            if (!String.IsNullOrEmpty(provinceId))
            {
                sb.Append(!String.IsNullOrEmpty(city) ? ", " : "<br/>");

                sb.AppendFormat("{0}", provinceId);
            }

            if (!String.IsNullOrEmpty(postalCode))
            {
                sb.AppendFormat("<br/>{0}", postalCode);
            }

            return sb.ToString();
        }

        private List<IndexListItem> GenerateIndexList()
        {
            MySessionVariables.NurseSearchItems.Value.Clear();
            MySessionVariables.CurrentIndex.Value = 0;

            var nl = ExecuteSearch();
            List<IndexListItem> l = ConvertNurseList(nl);

            MySessionVariables.NurseSearchItems.Value = nl.Select(n => n.userId).ToList();
            return l;
        }

        protected void btnFilter_OnClick(object sender, EventArgs e)
        {
            hdnLetterFilter.Value = "";

            // reset pager
            dpNursePager.SetPageProperties(0, 20, false);
            lvNurses.DataSource = GenerateIndexList();
            lvNurses.DataBind();

            hdnShuntToResults.Value = "1";
        }

        private void lvNurses_PagePropertiesChanged(object sender, EventArgs e)
        {
            lvNurses.DataSource = GenerateIndexList();
            lvNurses.DataBind();
        }

        protected void lnkLetter_OnClick(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            hdnLetterFilter.Value = lb.CommandArgument;
            dpNursePager.SetPageProperties(0, 20, false);

            lvNurses.DataSource = GenerateIndexList();
            lvNurses.DataBind();
        }


        protected void btnExport_OnClick(object sender, EventArgs e)
        {

            switch (this.ddlExportType.SelectedValue)
            {
                case "nursefull":
                    ExportNurseFull();
                    break;

                case "nursepartial":
                    ExportNursePartial();
                    break;

                case "facility":
                    ExportFacility();
                    break;

                default:

                    break;
            }

        }

        private void ExportNursePartial()
        {
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("SearchResults");

            int dataRowIndex = 1;

            //wanted to remove title
            //ws.Cells["A" + dataRowIndex].Value = "Search Export";

            //using (ExcelRange rng = ws.Cells[string.Format("A{0}:A{0}", dataRowIndex)])
            //{
            //    rng.Style.Font.Bold = true;
            //    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
            //    rng.Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);  //Set color to dark blue
            //    rng.Style.Font.Color.SetColor(Color.White);
            //}

            //dataRowIndex++;


            var startDataIndex = dataRowIndex;

            ws.Cells[String.Format("A{0}", dataRowIndex)].Value = "Nurse Id";
            ws.Cells[String.Format("B{0}", dataRowIndex)].Value = "Full Name";
            ws.Cells[String.Format("C{0}", dataRowIndex)].Value = "Phone";
            ws.Cells[String.Format("D{0}", dataRowIndex)].Value = "Address";
            ws.Cells[String.Format("E{0}", dataRowIndex)].Value = "Email";
            ws.Cells[String.Format("F{0}", dataRowIndex)].Value = "Designation";
            ws.Cells[String.Format("G{0}", dataRowIndex)].Value = "Status";
            ws.Cells[String.Format("H{0}", dataRowIndex)].Value = "Facility";

            using (ExcelRange rng = ws.Cells[String.Format("A{0}:H{0}", dataRowIndex)])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.Black);
                rng.Style.Font.Color.SetColor(Color.White);
            }

            dataRowIndex++;

            var nl = ExecuteSearch();
            var l = ConvertNurseList(nl);

            foreach (var n in l)
            {
                ws.Cells["A" + dataRowIndex].Value = n.nurseId;
                ws.Cells["B" + dataRowIndex].Value = n.fullName;
                ws.Cells["C" + dataRowIndex].Value = n.phone;
                ws.Cells["D" + dataRowIndex].Value = n.address.Replace("<br/>", "\r\n");
                ws.Cells["E" + dataRowIndex].Value = n.email;
                ws.Cells["F" + dataRowIndex].Value = n.designation;
                ws.Cells["G" + dataRowIndex].Value = n.status;
                ws.Cells["H" + dataRowIndex].Value = n.facility;

                dataRowIndex++;
            }

            ws.Cells[String.Format("A{0}:H{1}", startDataIndex, dataRowIndex - 1)].AutoFitColumns();
            ws.Cells[String.Format("D{0}:D{1}", startDataIndex, dataRowIndex - 1)].Style.WrapText = true;

            string dateSuffix = DateTime.Now.ToString("yyyy_MM_dd_H_mm");

            string fileName = String.Format("{0}-{1}.xlsx", "NurseExport", dateSuffix);

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", String.Format("attachment;  filename={0}", fileName));
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        private string GenerateBoardOfDirectorsExportValue(List<NurseCommittee> ncl)
        {

            var q = ncl.Where(cm => cm.committeeId == (byte) Committee.BoardOfDirectors).Select(cm => String.Format("{0}{1}",cm.CommitteePosition.refCommitteePosition.positionName, cm.isAlternate ? " (Alt.)" : "")).ToList();

            q.AddRange(ncl.Where(cm => cm.committeeId == (byte)Committee.Negotiating && cm.positionId == (byte)CommitteePosition.President).Select(cm => String.Format("{0}{1}",ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.NegotiatingPresident), cm.isAlternate ? " (Alt.)" : "")).ToList());

            q.AddRange(ncl.Where(cm => cm.committeeId == (byte)Committee.Negotiating && cm.positionId == (byte)CommitteePosition.VicePresident).Select(cm => String.Format("{0}{1}",ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.NegotiatingVicePresident), cm.isAlternate ? " (Alt.)" : "")).ToList());

            q.AddRange(ncl.Where(cm => cm.committeeId == (byte)Committee.Finance && cm.positionId == (byte)CommitteePosition.Chair).Select(cm => String.Format("{0}{1}",ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.FinanceChair), cm.isAlternate ? " (Alt.)" : "")).ToList());

            q.AddRange(ncl.Where(cm => cm.committeeId == (byte)Committee.Bursary && cm.positionId == (byte)CommitteePosition.Chair && cm.regionId == (byte)Region.Central).Select(cm => String.Format("{0}{1}",ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.BursaryChairCentral), cm.isAlternate ? " (Alt.)" : "")).ToList());

            q.AddRange(ncl.Where(cm => cm.committeeId == (byte)Committee.Bursary && cm.positionId == (byte)CommitteePosition.Chair && cm.regionId == (byte)Region.Eastern).Select(cm => String.Format("{0}{1}",ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.BursaryChairEastern), cm.isAlternate ? " (Alt.)" : "")).ToList());

            q.AddRange(ncl.Where(cm => cm.committeeId == (byte)Committee.Bursary && cm.positionId == (byte)CommitteePosition.Chair && cm.regionId == (byte)Region.Northern).Select(cm => String.Format("{0}{1}",ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.BursaryChairNorthern), cm.isAlternate ? " (Alt.)" : "")).ToList());

            q.AddRange(ncl.Where(cm => cm.committeeId == (byte)Committee.Bursary && cm.positionId == (byte)CommitteePosition.Chair && cm.regionId == (byte)Region.Western).Select(cm => String.Format("{0}{1}",ResourceUtils.GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition.BursaryChairWestern), cm.isAlternate ? " (Alt.)" : "")).ToList());

            return string.Join(", " , q.OrderBy(p => p).ToList());
        }



        private void ExportNurseFull()
        {
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("SearchResults");

            int dataRowIndex = 1;

            //wanted to remove title
            //ws.Cells["A" + dataRowIndex].Value = "Search Full Export";

            //using (ExcelRange rng = ws.Cells[string.Format("A{0}:A{0}", dataRowIndex)])
            //{
            //    rng.Style.Font.Bold = true;
            //    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
            //    rng.Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);  //Set color to dark blue
            //    rng.Style.Font.Color.SetColor(Color.White);
            //}

            //dataRowIndex++;


            var startDataIndex = dataRowIndex;

            ws.Cells[String.Format("A{0}", dataRowIndex)].Value = "Username";
            ws.Cells[String.Format("B{0}", dataRowIndex)].Value = "First Name";
            ws.Cells[String.Format("C{0}", dataRowIndex)].Value = "Initial";
            ws.Cells[String.Format("D{0}", dataRowIndex)].Value = "Last Name";
            ws.Cells[String.Format("E{0}", dataRowIndex)].Value = "Nickname";
            ws.Cells[String.Format("F{0}", dataRowIndex)].Value = "Gender";
            ws.Cells[String.Format("G{0}", dataRowIndex)].Value = "Date of Birth";
            ws.Cells[String.Format("H{0}", dataRowIndex)].Value = "Designation";
            ws.Cells[String.Format("I{0}", dataRowIndex)].Value = "Employment Status";
            ws.Cells[String.Format("J{0}", dataRowIndex)].Value = "Email";
            ws.Cells[String.Format("K{0}", dataRowIndex)].Value = "Secondary Email";
            ws.Cells[String.Format("L{0}", dataRowIndex)].Value = "Home Phone";
            ws.Cells[String.Format("M{0}", dataRowIndex)].Value = "Cell Phone";
            ws.Cells[String.Format("N{0}", dataRowIndex)].Value = "Work Phone";
            ws.Cells[String.Format("O{0}", dataRowIndex)].Value = "Fax";
            ws.Cells[String.Format("P{0}", dataRowIndex)].Value = "Mailing Address Line 1";
            ws.Cells[String.Format("Q{0}", dataRowIndex)].Value = "Mailing Address Line 2";
            ws.Cells[String.Format("R{0}", dataRowIndex)].Value = "Mailing Address City";
            ws.Cells[String.Format("S{0}", dataRowIndex)].Value = "Mailing Address Province";
            ws.Cells[String.Format("T{0}", dataRowIndex)].Value = "Mailing Address Postal Code";

            ws.Cells[String.Format("U{0}", dataRowIndex)].Value = "No AIL";
            ws.Cells[String.Format("V{0}", dataRowIndex)].Value = "No Johnsons";
            ws.Cells[String.Format("W{0}", dataRowIndex)].Value = "No Union Calling";
            ws.Cells[String.Format("X{0}", dataRowIndex)].Value = "Primary Facility";
            ws.Cells[String.Format("Y{0}", dataRowIndex)].Value = "Prim. Fac. Empl. Type ";
            ws.Cells[String.Format("Z{0}", dataRowIndex)].Value = "Prim. Fac. Table Officer Positions Held ";
            ws.Cells[String.Format("AA{0}", dataRowIndex)].Value = "Prim. Fac. Local Positions Held ";
            ws.Cells[String.Format("AB{0}", dataRowIndex)].Value = "Secondary Facility";
            ws.Cells[String.Format("AC{0}", dataRowIndex)].Value = "Sec Fac. Empl. Type";
            ws.Cells[String.Format("AD{0}", dataRowIndex)].Value = "Sec. Fac. Table Officer Positions Held ";
            ws.Cells[String.Format("AE{0}", dataRowIndex)].Value = "Sec. Fac. Local Positions Held ";
            ws.Cells[String.Format("AF{0}", dataRowIndex)].Value = "Tertiary Facility";
            ws.Cells[String.Format("AG{0}", dataRowIndex)].Value = "Ter. Fac. Empl. Type";
            ws.Cells[String.Format("AH{0}", dataRowIndex)].Value = "Ter. Fac. Table Officer Positions Held ";
            ws.Cells[String.Format("AI{0}", dataRowIndex)].Value = "Ter. Fac. Local Positions Held ";
            ws.Cells[String.Format("AJ{0}", dataRowIndex)].Value = "BUGLM Comm. Positions";
            ws.Cells[String.Format("AK{0}", dataRowIndex)].Value = "Bursary Comm. Position";
            ws.Cells[String.Format("AL{0}", dataRowIndex)].Value = "Union Disc. Comm. Position";
            ws.Cells[String.Format("AM{0}", dataRowIndex)].Value = "Union Discipline Appeal Position";
            ws.Cells[String.Format("AN{0}", dataRowIndex)].Value = "Board of Directors Position";
            ws.Cells[String.Format("AO{0}", dataRowIndex)].Value = "Negotiating Comm. Position";
            ws.Cells[String.Format("AP{0}", dataRowIndex)].Value = "AGM Comm. Position";
            ws.Cells[String.Format("AQ{0}", dataRowIndex)].Value = "Const. & Res. Comm. Position";
            ws.Cells[String.Format("AR{0}", dataRowIndex)].Value = "Education Comm. Position";
            ws.Cells[String.Format("AS{0}", dataRowIndex)].Value = "Finance Comm. Position";
            ws.Cells[String.Format("AT{0}", dataRowIndex)].Value = "Personnel Comm. Position";

            using (ExcelRange rng = ws.Cells[String.Format("A{0}:AT{0}", dataRowIndex)])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.Black);
                rng.Style.Font.Color.SetColor(Color.White);
            }

            dataRowIndex++;

            var nl = ExecuteSearch();

            var idList = nl.Select(n => n.userId).ToList();

            foreach (var id in idList)
            {
                var n = _mBs.GetUserProfile(id);

                var pl = _mBs.GetNursePhones(n.id);

                var al = _mBs.GetNurseAddress(n.id);

                var fl = n.Nurse.NurseFacilities.ToList();

                var col = n.Nurse.NurseOptOuts.ToList();

                var fcl = n.Nurse.NurseCommittees.ToList();


                ws.Cells["A" + dataRowIndex].Value = n.username;
                ws.Cells["B" + dataRowIndex].Value = n.Nurse.firstName;
                ws.Cells["C" + dataRowIndex].Value = n.Nurse.initial;
                ws.Cells["D" + dataRowIndex].Value = n.Nurse.lastName;
                ws.Cells["E" + dataRowIndex].Value = n.Nurse.nickname;
                ws.Cells["F" + dataRowIndex].Value = (n.Nurse.genderId != null) ? ResourceUtils.GetEnumLabel((Gender)n.Nurse.genderId.Value) : "";
                ws.Cells["G" + dataRowIndex].Value = (n.Nurse.birthDate != null) ? ((DateTime)n.Nurse.birthDate).ToString("MM-dd-yyyy") : ""; 
                ws.Cells["H" + dataRowIndex].Value = (n.Nurse.nurseDesignationId != null) ? ResourceUtils.GetEnumLabel((Designation)n.Nurse.nurseDesignationId) : "";
                ws.Cells["I" + dataRowIndex].Value = (n.Nurse.employmentStatusId != null) ? ResourceUtils.GetEnumLabel((EmploymentStatus)n.Nurse.employmentStatusId) : "";
                ws.Cells["J" + dataRowIndex].Value = n.email;
                ws.Cells["K" + dataRowIndex].Value = n.secondaryemail;


                ws.Cells["L" + dataRowIndex].Value =  String.Join(";",(from x in pl.Where(p => p.Phone.phoneTypeId == (int)PhoneType.Home)
                                                      select x).Select(p => String.Concat(p.Phone.phoneNumber, String.IsNullOrEmpty(p.Phone.extension) ? String.Empty : String.Format(" (Ext. {0})", p.Phone.extension))).ToArray());

                ws.Cells["M" + dataRowIndex].Value =  String.Join(";",(from x in pl.Where(p => p.Phone.phoneTypeId == (int)PhoneType.Cell)
                                                      select x).Select(p => String.Concat(p.Phone.phoneNumber, String.IsNullOrEmpty(p.Phone.extension) ? String.Empty : String.Format(" (Ext. {0})", p.Phone.extension))).ToArray());

                ws.Cells["N" + dataRowIndex].Value =  String.Join(";",(from x in pl.Where(p => p.Phone.phoneTypeId == (int)PhoneType.Work)
                                                      select x).Select(p => String.Concat(p.Phone.phoneNumber, String.IsNullOrEmpty(p.Phone.extension) ? String.Empty : String.Format(" (Ext. {0})", p.Phone.extension))).ToArray());

                ws.Cells["O" + dataRowIndex].Value = String.Join(";",(from x in pl.Where(p => p.Phone.phoneTypeId == (int)PhoneType.Fax)
                                                      select x).Select(p => String.Concat(p.Phone.phoneNumber, String.IsNullOrEmpty(p.Phone.extension) ? String.Empty : String.Format(" (Ext. {0})", p.Phone.extension))).ToArray());


                ws.Cells["P" + dataRowIndex].Value = al.Address.line1;
                ws.Cells["Q" + dataRowIndex].Value = al.Address.line2;
                ws.Cells["R" + dataRowIndex].Value = al.Address.city;
                ws.Cells["S" + dataRowIndex].Value = al.Address.provinceId;
                ws.Cells["T" + dataRowIndex].Value = al.Address.postalCode;


                ws.Cells["U" + dataRowIndex].Value = ((from co in col.Where(co => co.optOutId == (byte)CommunicationOption.NoAil)
                                                      select co).FirstOrDefault() != null) ? "X" : "";

                ws.Cells["V" + dataRowIndex].Value = ((from co in col.Where(co => co.optOutId == (byte)CommunicationOption.NoJohnsons)
                                                       select co).FirstOrDefault() != null) ? "X" : "";

                ws.Cells["W" + dataRowIndex].Value = ((from co in col.Where(co => co.optOutId == (byte)CommunicationOption.NoUnionCalling)
                                                       select co).FirstOrDefault() != null) ? "X" : "";



                var primaryFacility = (from f in fl.Where(f => f.priority == 1)
                                       select f).FirstOrDefault();

                ws.Cells["X" + dataRowIndex].Value = (primaryFacility != null) ? primaryFacility.Facility.facilityName : "";
                ws.Cells["Y" + dataRowIndex].Value = (primaryFacility != null && primaryFacility.employmentTypeId != null) ? ResourceUtils.GetEnumLabel((EmploymentType) primaryFacility.employmentTypeId): "";
                ws.Cells["Z" + dataRowIndex].Value = (primaryFacility != null) ? String.Join(", ", (from fto in primaryFacility.FacilityTableOfficers where fto.nurseId == n.id select fto.refTableOfficerPosition.positionName).ToArray()) : "";
                ws.Cells["AA" + dataRowIndex].Value = (primaryFacility != null) ? String.Join(", ", (from fto in primaryFacility.FacilityLocalPositions where fto.nurseId == n.id select fto.refLocalPosition.positionName).ToArray()) : "";

                var secondaryFacility = (from f in fl.Where(f => f.priority == 2)
                                       select f).FirstOrDefault();

                ws.Cells["AB" + dataRowIndex].Value = (secondaryFacility != null) ? secondaryFacility.Facility.facilityName : "";
                ws.Cells["AC" + dataRowIndex].Value = (secondaryFacility != null && secondaryFacility.employmentTypeId != null) ? ResourceUtils.GetEnumLabel((EmploymentType)secondaryFacility.employmentTypeId) : "";
                ws.Cells["AD" + dataRowIndex].Value = (secondaryFacility != null) ? String.Join(", ", (from fto in secondaryFacility.FacilityTableOfficers where fto.nurseId == n.id select fto.refTableOfficerPosition.positionName).ToArray()) : "";
                ws.Cells["AE" + dataRowIndex].Value = (secondaryFacility != null) ? String.Join(", ", (from fto in secondaryFacility.FacilityLocalPositions where fto.nurseId == n.id select fto.refLocalPosition.positionName).ToArray()) : "";

                var tertiaryFacility = (from f in fl.Where(f => f.priority == 3)
                                       select f).FirstOrDefault();

                ws.Cells["AF" + dataRowIndex].Value = (tertiaryFacility != null) ? tertiaryFacility.Facility.facilityName : "";
                ws.Cells["AG" + dataRowIndex].Value = (tertiaryFacility != null && tertiaryFacility.employmentTypeId != null) ? ResourceUtils.GetEnumLabel((EmploymentType)tertiaryFacility.employmentTypeId) : "";
                ws.Cells["AH" + dataRowIndex].Value = (tertiaryFacility != null) ? String.Join(", ", (from fto in tertiaryFacility.FacilityTableOfficers where fto.nurseId == n.id select fto.refTableOfficerPosition.positionName).ToArray()) : "";
                ws.Cells["AI" + dataRowIndex].Value = (tertiaryFacility != null) ? String.Join(", ", (from fto in tertiaryFacility.FacilityLocalPositions where fto.nurseId == n.id select fto.refLocalPosition.positionName).ToArray()) : "";

                ws.Cells["AJ" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte) Committee.Buglm)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));

                ws.Cells["AK" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.Bursary)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                ws.Cells["AL" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.UnionDiscipline)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                ws.Cells["AM" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.UnionDisciplineAppeal)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                var j = (from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.BoardOfDirectors)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName)).ToList();
                
                
                //ws.Cells["AN" + dataRowIndex].Value = String.Join(", ",
                //                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.BoardOfDirectors)
                //                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                //                                 //select fc.CommitteePosition.refCommitteePosition.positionName);
                ws.Cells["AN" + dataRowIndex].Value = GenerateBoardOfDirectorsExportValue(fcl);

                ws.Cells["AO" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.Negotiating)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                ws.Cells["AP" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.Agm)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                ws.Cells["AQ" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.Constitution)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                ws.Cells["AR" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.Education)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                ws.Cells["AS" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.Finance)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                ws.Cells["AT" + dataRowIndex].Value = String.Join(", ",
                                                 from fc in fcl.Where(fc => fc.committeeId == (byte)Committee.Personnel)
                                                 select String.Format("{0}{1}", (fc.isAlternate) ? "Alternate: " : "", fc.CommitteePosition.refCommitteePosition.positionName));
                                                 //select fc.CommitteePosition.refCommitteePosition.positionName);

                dataRowIndex++;
            }


            ws.Cells[String.Format("A{0}:AT{1}", startDataIndex, dataRowIndex - 1)].AutoFitColumns();

            string dateSuffix = DateTime.Now.ToString("yyyy_MM_dd_H_mm");

            string fileName = String.Format("{0}-{1}.xlsx", "NurseExport", dateSuffix);

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", String.Format("attachment;  filename={0}", fileName));
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }


        private void ExportFacility()
        {

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Facilties");

            int dataRowIndex = 1;

            //ws.Cells["A" + dataRowIndex].Value = "Facility Export";

            //using (ExcelRange rng = ws.Cells[string.Format("A{0}:N{0}", dataRowIndex)])
            //{
            //    rng.Style.Font.Bold = true;
            //    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
            //    rng.Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);  //Set color to dark blue
            //    rng.Style.Font.Color.SetColor(Color.White);
            //}

            //dataRowIndex++;


            var startDataIndex = dataRowIndex;

            ws.Cells[String.Format("A{0}", dataRowIndex)].Value = "Name";
            ws.Cells[String.Format("B{0}", dataRowIndex)].Value = "District";
            ws.Cells[String.Format("C{0}", dataRowIndex)].Value = "Region";
            ws.Cells[String.Format("D{0}", dataRowIndex)].Value = "Type";
            ws.Cells[String.Format("E{0}", dataRowIndex)].Value = "Employer Group";
            ws.Cells[String.Format("F{0}", dataRowIndex)].Value = "Labour Representative";
            ws.Cells[String.Format("G{0}", dataRowIndex)].Value = "Casual Representation";
            ws.Cells[String.Format("H{0}", dataRowIndex)].Value = "LPN Representation";
            ws.Cells[String.Format("I{0}", dataRowIndex)].Value = "Address Line 1";
            ws.Cells[String.Format("J{0}", dataRowIndex)].Value = "Address Line 2";
            ws.Cells[String.Format("K{0}", dataRowIndex)].Value = "City";
            ws.Cells[String.Format("L{0}", dataRowIndex)].Value = "Province";
            ws.Cells[String.Format("M{0}", dataRowIndex)].Value = "Postal Code";
            ws.Cells[String.Format("N{0}", dataRowIndex)].Value = "Phone Number";

            using (ExcelRange rng = ws.Cells[String.Format("A{0}:N{0}", dataRowIndex)])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.Black);
                rng.Style.Font.Color.SetColor(Color.White);
            }

            dataRowIndex++;

            var nl = this.GetFacilitesForExport();

            foreach (var n in nl)
            {
                ws.Cells["A" + dataRowIndex].Value = n.facilityName;
                ws.Cells["B" + dataRowIndex].Value = n.DistrictName;
                ws.Cells["C" + dataRowIndex].Value = n.regionname;
                ws.Cells["D" + dataRowIndex].Value = n.facilityTypeName;
                ws.Cells["E" + dataRowIndex].Value = n.EmployerGroupName;
                ws.Cells["F" + dataRowIndex].Value = n.Labour_Rep;
                ws.Cells["G" + dataRowIndex].Value = n.casualCoverage ? "Yes" : "No";
                ws.Cells["H" + dataRowIndex].Value = n.lpnCoverage ? "Yes" : "No";
                ws.Cells["I" + dataRowIndex].Value = n.line1;
                ws.Cells["J" + dataRowIndex].Value = n.line2;
                ws.Cells["K" + dataRowIndex].Value = n.city;
                ws.Cells["L" + dataRowIndex].Value = n.provinceid;
                ws.Cells["M" + dataRowIndex].Value = n.postalCode;
                ws.Cells["N" + dataRowIndex].Value = n.PhoneNumber;

                dataRowIndex++;
            }

            ws.Cells[String.Format("A{0}:N{1}", startDataIndex, dataRowIndex - 1)].AutoFitColumns();

            string dateSuffix = DateTime.Now.ToString("yyyy_MM_dd_H_mm");

            string fileName = String.Format("{0}-{1}.xlsx", "FacilityExport", dateSuffix);

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", String.Format("attachment;  filename={0}", fileName));
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();


        }

        public static PositionVo FillPositionVo(PositionVo p)
        {
            var pVo = new PositionVo
            {
                id = p.id.ToString(),
                positionName = p.positionName
            };

            return pVo;
        }

        public class PositionVo
        {
            public string id { get; set; }
            public string positionName { get; set; }
        }

    }
}