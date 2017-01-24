using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Nsnu.DataAccess;
using Nsnu.DataAccess.Enumerations;
using CommitteeEnum = Nsnu.DataAccess.Enumerations.Committee;
using PositionEnum = Nsnu.DataAccess.Enumerations.CommitteePosition;
using CommitteePosition = Nsnu.DataAccess.CommitteePosition;
using District = Nsnu.DataAccess.District;
using Region = Nsnu.DataAccess.Region;

namespace Nsnu.MembershipServices
{
    public class MembershipBs
    {
        private const int ChunkSize = 2000;
        private NsnuDataContext db = new NsnuDataContext(ConfigurationManager.ConnectionStrings["NsnuConnectionString"].ConnectionString);

        #region "Search methods"

        public List<SearchUserProfilesResult> SearchUserProfiles(string userName, string firstName, string lastName, string email, byte? designationId, byte? sectorId, int? facilityId, byte? districtId,
            byte? regionId, byte? committeeId, byte? positionId, int? communicationOptionId, byte? employerGroupId, bool? facilityCasualCoverage, bool? facilityLpnCoverage, string letterFilter, bool? isAlternate, string line1, string phone, string employmentStatusList, string localPositionList, string tableOfficerPositionList)
        {
            //return db.Nurses.ToList();

            return db.SearchUserProfiles(userName, firstName, lastName, email, designationId, 
                                         sectorId, facilityId, districtId, regionId, committeeId, positionId, communicationOptionId, employerGroupId,
                                         facilityCasualCoverage, facilityLpnCoverage, letterFilter, isAlternate, line1, phone, employmentStatusList, localPositionList, tableOfficerPositionList).ToList();
         
        }
        public List<Nurse> SearchUserProfilesOld(string userName, string firstName, string lastName, string email, byte? designationId, byte? employmentStatusId, byte? sectorId, int? facilityId, byte? districtId, 
            byte? regionId, byte? committeeId, byte? positionId, int? communicationOptionId, byte? employerGroupId, bool? facilityCasualCoverage, bool? facilityLpnCoverage, string letterFilter)
        {
            var q = from n in db.Nurses
                    where (String.IsNullOrEmpty(userName) || n.UserProfile.username.Contains(userName))
                    && (String.IsNullOrEmpty(firstName) || n.firstName.Contains(userName))
                    && (String.IsNullOrEmpty(lastName) || n.firstName.Contains(lastName))
                    && (String.IsNullOrEmpty(email) || n.UserProfile.email.Contains(email))
                    && (designationId == null || n.nurseDesignationId == designationId)
                    && (employmentStatusId == null || n.employmentStatusId == employmentStatusId)
                    && (String.IsNullOrEmpty(letterFilter) || n.lastName.StartsWith(letterFilter))
                    select n;

            if (sectorId != null || facilityId != null || districtId != null || regionId != null || employerGroupId != null || facilityCasualCoverage != null || facilityLpnCoverage != null)
            {
                q = from n in q
                    join nf in db.NurseFacilities on n.userId equals nf.nurseId
                    join f in db.Facilities on nf.facilityId equals f.id
                    where (sectorId == null || nf.Facility.facilityTypeId == sectorId)
                    && (facilityId == null || nf.facilityId == facilityId)
                    && (districtId == null || nf.Facility.districtId == districtId)
                    && (regionId == null || nf.Facility.District.regionId == regionId)
                    && (employerGroupId == null || nf.Facility.employerGroupId == employerGroupId)
                    && (facilityCasualCoverage == null || nf.Facility.casualCoverage == facilityCasualCoverage)
                    && (facilityLpnCoverage == null || nf.Facility.lpnCoverage == facilityLpnCoverage)
                    select n;
            }

            if (communicationOptionId != null)
            {
                q = from n in q
                    join co in db.NurseOptOuts on n.userId equals co.nurseId
                    where (co.optOutId == communicationOptionId)
                    select n;
            }

            return q.OrderBy(n => n.lastName).ToList();
        }
        
        #endregion

        #region "Delete methods"


        public void DeleteNurse(int nurseId)
        {
            var nurse = db.Nurses.SingleOrDefault(n => n.userId == nurseId);

            nurse.isDeleted = true;

            db.SubmitChanges();

            //db.Nurses.DeleteOnSubmit(s.Nurse);
        }


        public void DeleteNotes(int nurseId)
        {
            var q0 = from fto in db.Notes
                     where fto.nurseId == nurseId
                     select fto;

            db.Notes.DeleteAllOnSubmit(q0);
        }


        public void DeleteHistory(int nurseId)
        {
            var q0 = from fto in db.VersionHistories
                     where fto.nurseId == nurseId
                     select fto;

            db.VersionHistories.DeleteAllOnSubmit(q0);
        }

        public void DeleteEmploymentData(int nurseId)
        {
            var q0 = from fto in db.FacilityTableOfficers
                     where fto.nurseId == nurseId
                     select fto;

            var q1 = from flp in db.FacilityLocalPositions
                     where flp.nurseId == nurseId
                     select flp;

            var q2 = from nf in db.NurseFacilities
                     where nf.nurseId == nurseId
                     select nf;

            db.FacilityTableOfficers.DeleteAllOnSubmit(q0);
            db.FacilityLocalPositions.DeleteAllOnSubmit(q1);
            db.NurseFacilities.DeleteAllOnSubmit(q2);
        }

        public void DeleteCommitteeData(int nurseId)
        {
            var q0 = from nc in db.NurseCommittees
                     where nc.nurseId == nurseId
                     select nc;

            db.NurseCommittees.DeleteAllOnSubmit(q0);
        }

        public void DeleteCommunicationOptionsData(int nurseId)
        {
            var q0 = from nc in db.NurseOptOuts
                     where nc.nurseId == nurseId
                     select nc;
            db.NurseOptOuts.DeleteAllOnSubmit(q0);


        }


        public void DeletePhoneData(int nurseId)
        {
            var q0 = from nc in db.NursePhones
                     where nc.userId == nurseId
                     select nc;

            db.NursePhones.DeleteAllOnSubmit(q0);
        }

        public void DeleteAddressData(int nurseId)
        {
            var q0 = from na in db.NurseAddresses
                     where na.nurseId == nurseId
                     select na;

            db.NurseAddresses.DeleteAllOnSubmit(q0);
        }

        public void DeleteNote(int noteId)
        {
            var n = GetNote(noteId);
            db.Notes.DeleteOnSubmit(n);
            db.SubmitChanges();
        }

        #endregion

        #region "Create methods"

        public UserProfile CreateUserProfile(string username, string password, string email)
        {
            UserProfile up = new UserProfile();
            SimpleAES aes = new SimpleAES();

            up.username = username;
            //up.password = password;
            up.password = aes.Encrypt(password);
            up.email = email;
            up.creationDate = DateTime.Now;

            db.UserProfiles.InsertOnSubmit(up);
            db.SubmitChanges();

            return up;
        }

        #endregion

        #region "Read methods"

        public List<Nurse> GetNurses (List<int> idList)
        {
            var nl = new List<Nurse>();
            var myIds = new List<int>();
            
            var q = from n in db.Nurses
                    where myIds.Contains(n.userId)
                    select n;

            //CHUNK out the ids to overcome LINQ parameter limitation (as a result of the productIds.Contains clause in tq query above
            for (var j = 0; j <= idList.Count; j += ChunkSize)
            {
                myIds = idList.GetRange(j, (idList.Count - j > ChunkSize) ? ChunkSize : idList.Count - j);
                nl.AddRange(q.ToList());
            }

            return nl.OrderBy(z => z.lastName).ThenBy(z => z.firstName).ToList();
        }


        public UserProfile GetValidNurseProfile (string username, string password)
        {
            var up = GetUserProfile(username, password);

            if (up != null && up.Nurse != null && up.Nurse.employmentStatusId != (byte)EmploymentStatus.Inactive && up.Nurse.employmentStatusId != (byte)EmploymentStatus.Retired)
            {
                return up;
            }

            return null;
        }

        public UserProfile GetUserProfile(string username, string password)
        {
            SimpleAES aes = new SimpleAES();
            //var q = db.UserProfiles.Where(up => up.username == username && up.Nurse.employmentStatusId != (byte)EmploymentStatus.Inactive && up.Nurse.employmentStatusId != (byte)EmploymentStatus.Retired).ToList();
            var q = db.UserProfiles.Where(up => up.username == username).ToList();
            
            return q.Where(up => aes.Decrypt(up.password) == password).SingleOrDefault();
        }

        public UserProfile GetUserProfile(string username)
        {
            var s = db.UserProfiles.SingleOrDefault(up => up.username.ToLower() == username.ToLower());
            return s;
        }

        public UserProfile GetUserProfile(int id)
        {
            var s = db.UserProfiles.SingleOrDefault(up => up.id == id);
            return s;
        }

        public List<GetRolesForUserResult> GetRolesForUser(string username)
        {
            return db.GetRolesForUser(username).ToList();
        }

        public Note GetNote(int noteId)
        {
            return db.Notes.SingleOrDefault(i => i.id == noteId);
        }

        public List<VersionHistory> GetVersionHistory(int nurseId)
        {
            return db.VersionHistories.Where(n => n.nurseId == nurseId).OrderByDescending(vh => vh.modificationDate).ToList();
        }

        public VersionHistory GetVersion(int versionid)
        {
            return db.VersionHistories.SingleOrDefault(n => n.id == versionid);           
        }

        public List<Note> GetNotes(int nurseId)
        {
            return db.Notes.Where(n => n.nurseId == nurseId).OrderByDescending(n => n.creationDate).ToList();
        }

        public Facility GetFacility(int facilityId)
        {
            return db.Facilities.SingleOrDefault(i => i.id == facilityId);
        }

        public List<Facility> GetFacilities()
        {
            return db.Facilities.ToList();
        }

        public List<Region> GetRegions()
        {
            return db.Regions.ToList();
        }

        public Region GetRegion(int regionId)
        {
            return db.Regions.SingleOrDefault(i => i.id == regionId);
        }


        public List<District> GetDistricts()
        {
            return db.Districts.ToList();
        }


        public District GetDistrict(int districtId)
        {
            return db.Districts.SingleOrDefault(i => i.id == districtId);
        }


        public List<GetFacilitiesForExportResult> GetFacilitiesForExport()
        {
            return db.GetFacilitiesForExport().ToList();
        }


        public NurseAddress GetNurseAddress(int nurseId)
        {
            //DB model supports multiple addresses, but nurses can currently only ever have one. 
            var q = from na in db.NurseAddresses
                    where na.nurseId == nurseId
                    select na;

            return q.FirstOrDefault();
        }

        public List<NursePhone> GetNursePhones(int nurseId)
        {
            var q = from np in db.NursePhones
                    where np.userId == nurseId
                    select np;

            return q.ToList();
        }

        public List<Phone> GetPhones(int nurseId)
        {
            var q = from p in db.Phones
                    join np in db.NursePhones on p.id equals np.phoneId 
                    where np.userId == nurseId
                    orderby p.phoneTypeId
                    select p;

            return q.ToList();
        }

        public List<NurseFacility> GetNurseFacilities(int nurseId)
        {
            var q = from fn in db.NurseFacilities
                    where fn.nurseId == nurseId
                    select fn;

            return q.ToList();
        }


        public List<NurseOptOut> GetNurseCommunicationOptions(int nurseId)
        {
            var q = from no in db.NurseOptOuts
                    where no.nurseId == nurseId
                    select no;

            return q.ToList();
        }


        public List<FacilityTableOfficer> GetTableOfficerPositions(int nurseId, int facilityId)
        {
            var q = from fto in db.FacilityTableOfficers
                    where fto.nurseId == nurseId && fto.facilityId == facilityId
                    select fto;

            return q.ToList();
        }

        public List<FacilityLocalPosition> GetLocalPositions(int nurseId, int facilityId)
        {
            var q = from flp in db.FacilityLocalPositions
                    where flp.nurseId == nurseId && flp.facilityId == facilityId
                    select flp;

            return q.ToList();
        }

        public List<refLocalPosition> GetLocalPositions()
        {
            return db.refLocalPositions.ToList();
        }

        public List<refTableOfficerPosition> GetTableOfficerPositions()
        {
            return db.refTableOfficerPositions.ToList();
        }

        public List<GetFilledCommitteePositionsResult> GetFilledCommitteePositions(int nurseId)
        {
            return db.GetFilledCommitteePositions(nurseId).ToList();
        }

        public List<NurseCommittee> GetCommitteeMembers()
        {
            var q = from cm in db.NurseCommittees
                    select cm;

            return q.ToList();
        }

        public List<NurseCommittee> GetNurseCommitteePositions(int nurseId)
        {
            var q = from nc in db.NurseCommittees
                    where nc.nurseId == nurseId
                    select nc;

            return q.ToList();
        }

        public List<CommitteePosition> GetAllCommitteePositions()
        {
            var q = from cp in db.CommitteePositions
                    select cp;

            return q.ToList();
        }

        //public List<refCommitteePosition> GetBoardOfDirectorsPositions()
        //{

        //    var q = from cp in db.refCommitteePositions
        //            join x in db.CommitteePositions on cp.id equals x.positionId
        //            where x.committeeId == (int)c ||
        //            (c == CommitteeEnum.BoardOfDirectors && ((x.committeeId == (int)CommitteeEnum.Finance && x.positionId == (int)PositionEnum.Chair)
        //                                                  || (x.committeeId == (int)CommitteeEnum.Bursary && x.positionId == (int)PositionEnum.Chair)
        //                                                  || (x.committeeId == (int)CommitteeEnum.Negotiating && x.positionId == (int)PositionEnum.President)
        //                                                  || (x.committeeId == (int)CommitteeEnum.Negotiating && x.positionId == (int)PositionEnum.VicePresident)
        //                ))
        //            select cp;

        //    return q.ToList();
        //}

        public List<refCommitteePosition> GetCommitteePositions(DataAccess.Enumerations.Committee c)
        {

            var q = from cp in db.refCommitteePositions
                    join x in db.CommitteePositions on cp.id equals x.positionId
                    where x.committeeId == (int)c 
                    select cp;

            return q.ToList();
        }

        public List<refCommitteePosition> GetCommitteeAlternatePositions(DataAccess.Enumerations.Committee c)
        {

            var q = from cp in db.refCommitteePositions
                    join x in db.CommitteePositions on cp.id equals x.positionId
                    where x.committeeId == (int)c && x.maxAlternates > 0
                    select cp;

            return q.ToList();
        }


        #endregion

        #region "Write methods"


        public Note AddNote(int nurseId, string noteBody, string userId)
        {
            Note n = new Note();
            n.noteBody = noteBody;
            n.creationDate = DateTime.Now;
            n.createdBy = userId;
            n.nurseId = nurseId;

            db.Notes.InsertOnSubmit(n);
            db.SubmitChanges();

            return n;
        }


        public void ProcessNurseProfile(UserProfile up, NurseAddress na, List<NursePhone> npl)
        {
            if (up.id != 0)
            {

            }
            else
            {
                up.creationDate = DateTime.Now;
                db.UserProfiles.InsertOnSubmit(up);

                db.NurseAddresses.InsertOnSubmit(na);

                //npl.Where(p => !String.IsNullOrEmpty(p.Phone.phoneNumber))
                db.NursePhones.InsertAllOnSubmit(npl.Where(p => !String.IsNullOrEmpty(p.Phone.phoneNumber)));
            }

            db.SubmitChanges();
        }

        public void ProcessNurseProfile(UserProfile up, NurseAddress na)
        {
            if (up.id != 0)
            {

            }
            else
            {
                up.creationDate = DateTime.Now;
                db.UserProfiles.InsertOnSubmit(up);

                db.NurseAddresses.InsertOnSubmit(na);
            }

            db.SubmitChanges();
        }

        public void ProcessNurseProfile(UserProfile up)
        {
            if (up.id != 0)
            {

            }
            else
            {
                up.creationDate = DateTime.Now;
                db.UserProfiles.InsertOnSubmit(up);
            }
            db.SubmitChanges();
            LogUserProfileVersion(up);
        }

        public void ProcessNurseProfile()
        {
            db.SubmitChanges();
        }




        #endregion

        #region "Utility methods"

        public void LogUserProfileVersion(UserProfile up)
        {
            //UserProfile up = GetUserProfile(nurseId);
            XElement xml = GenerateUserProfileXml(up);

            VersionHistory vh = new VersionHistory();
            vh.nurseId = up.id;
            vh.modificationDate = DateTime.Now;
            vh.modifiedBy = up.Nurse.lastModifiedBy;

            vh.versionXml = xml.ToString();

            db.VersionHistories.InsertOnSubmit(vh);

            db.SubmitChanges();
        }

        public XElement GenerateUserProfileXml(UserProfile up)
        {
            var address = GetNurseAddress(up.id);
            var phoneList = GetNursePhones(up.id);
            var facilityList = GetNurseFacilities(up.id);
            var positionList = GetNurseCommitteePositions(up.id);
            var communicationOptionsList = GetNurseCommunicationOptions(up.id);

            XElement xml = new XElement("nurse",
                                        new XAttribute("id", up.id),
                                        new XElement("username", up.username),
                                        new XElement("email", up.email),
                                        new XElement("secondaryemail", up.secondaryemail),
                                        new XElement("creationDate", up.creationDate),
                                        new XElement("birthDate", up.Nurse.birthDate),
                                        new XElement("completedMembershipForm", up.Nurse.completedMembershipForm),
                                        new XElement("employmentStatusId", up.Nurse.employmentStatusId),
                                        new XElement("firstName", up.Nurse.firstName),
                                        new XElement("genderId", up.Nurse.genderId),
                                        new XElement("initial", up.Nurse.initial),
                                        new XElement("issuedMembershipCard", up.Nurse.issuedMembershipCard),
                                        new XElement("lastName", up.Nurse.lastName),
                                        new XElement("nickname", up.Nurse.nickname),
                                        new XElement("nurseDesignationId", up.Nurse.nurseDesignationId),
                                        new XElement("addresses",
                                                     new XElement("address",
                                                                  new XAttribute("addressId", address.Address.id),
                                                                  new XElement("line1", address.Address.line1),
                                                                  new XElement("line2", address.Address.line2),
                                                                  new XElement("city", address.Address.city),
                                                                  new XElement("provinceId", address.Address.provinceId),
                                                                  new XElement("postalCode", address.Address.postalCode)
                                                         )
                                            ),
                                        new XElement("phones",
                                                     from p in phoneList
                                                     select
                                                         new XElement("phone",
                                                                      new XAttribute("phoneId", p.Phone.id),
                                                                      new XElement("phoneTypeId", p.Phone.phoneTypeId),
                                                                      new XElement("phoneNumber", p.Phone.phoneNumber),
                                                                      new XElement("extension", p.Phone.extension)
                                                         )
                                            ),
                                        new XElement("communicationOptions",
                                                     from co in communicationOptionsList
                                                     select
                                                         new XElement("communicationOption",
                                                                      new XAttribute("communicationOptionId", co.refOptOut.id),
                                                                      new XElement("communicationOptionName", co.refOptOut.optOutName)
                                                         )
                                            ),
                                        new XElement("facilities",
                                                     from f in facilityList
                                                     select
                                                         new XElement("facility",
                                                                      new XAttribute("facilityId", f.facilityId),
                                                                      new XElement("facilityName",
                                                                                   f.Facility.facilityName),
                                                                      new XElement("employmentTypeId",
                                                                                   f.employmentTypeId),
                                                                      new XElement("employmentType", (f.employmentTypeId != null) ? f.refEmploymentType.employmentTypeName : ""),
                                                                      new XElement("priority", f.priority),
                                                                      new XElement("localPositions",
                                                                                   from flp in
                                                                                       GetLocalPositions(f.nurseId,
                                                                                                         f.facilityId)
                                                                                   select
                                                                                       new XElement("localPosition",
                                                                                                    new XElement(
                                                                                                        "positionId",
                                                                                                        flp.positionId),
                                                                                                    new XElement(
                                                                                                        "position",
                                                                                                        flp.
                                                                                                            refLocalPosition
                                                                                                            .
                                                                                                            positionName)
                                                                                       )
                                                                          ),
                                                                      new XElement("tableOfficerPositions",
                                                                                   from top in
                                                                                       GetTableOfficerPositions(
                                                                                           f.nurseId, f.facilityId)
                                                                                   select
                                                                                       new XElement(
                                                                                       "tableOfficerPosition",
                                                                                       new XElement("positionId",
                                                                                                    top.positionId),
                                                                                       new XElement("position",
                                                                                                    top.
                                                                                                        refTableOfficerPosition
                                                                                                        .positionName)
                                                                                       )
                                                                          )

                                                         )
                                            ),
                                        new XElement("committeePositions",
                                                     from p in positionList
                                                     select
                                                         new XElement("committeePosition",
                                                                      new XAttribute("committeeId", p.committeeId),
                                                                      new XElement("committeeName",
                                                                                   p.CommitteePosition.Committee.
                                                                                       committeeName),
                                                                      new XElement("positionId", p.positionId),
                                                                      new XElement("positionName",
                                                                                   p.CommitteePosition.
                                                                                       refCommitteePosition.positionName),
                                                                      new XElement("isAlternate", p.isAlternate),
                                                                      new XElement("regionId", p.regionId),
                                                                      new XElement("regionName", p.regionId.HasValue ? GetRegion(p.regionId.Value).regionName : String.Empty),
                                                                      new XElement("districtId", p.districtId),
                                                                      new XElement("districtName", p.districtId.HasValue ? GetDistrict(p.districtId.Value).districtName : String.Empty)


                                                         )
                                            )
                );

            return xml;

        }

        //LastName + middle initial + first letter of first name + numerical suffix if required.
        //LastName + middle initial + first letter of first name + numerical suffix if required.
        public string GenerateUniqueUserName(string lastname, string firstname, string middleinitial)
        {
            string username = String.Concat(lastname, firstname[0], String.IsNullOrEmpty(middleinitial) ? String.Empty : middleinitial);

            //strip any non-alphanumeric characters
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            username = rgx.Replace(username, "");

            // check the db to see if this username is used, add 1, then 2, then 3 etc until a unique username is found
            if (GetUserProfile(username) != null)
            {
                int i = 0;
                do
                {
                    i++;
                    if (i == 1)
                    {
                        username += i.ToString();
                    }
                    else
                    {
                        username = username.Remove(username.Length - 1);
                        username += i.ToString();
                    }


                } while (GetUserProfile(username) != null);
            }

            return username.ToLower();
        }

        // generate 8 random alphanumeric characters,
        // letters I and O amd numbers 0 and 1 are omitted to avoind confusion between them
        public static string GeneratePassword()
        {
            string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K",
                     "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

            Random rnd = new Random();
            // rnd.Next(0, 23); // creates a number between 0 and 23
            // rnd.Next(2, 9); // creates a number between 2 and 9

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(rnd.Next(2, 9).ToString());
            sb.Append(letters[rnd.Next(0, 23)].ToString());
            sb.Append(rnd.Next(2, 9).ToString());
            sb.Append(letters[rnd.Next(0, 23)].ToString());
            sb.Append(rnd.Next(2, 9).ToString());
            sb.Append(letters[rnd.Next(0, 23)].ToString());
            sb.Append(rnd.Next(2, 9).ToString());
            sb.Append(letters[rnd.Next(0, 23)].ToString());

            string pw = sb.ToString();

            return pw;
        }

        public List<UserProfile> GetAllUserProfiles ()
        {
            var q = from up in db.UserProfiles
                    select up;

            return q.ToList();
        }
        #endregion
    }
}
