using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nsnu.DataAccess.Enumerations;

namespace WebApplication.Utilities
{
    public class ResourceUtils
    {

        public static string GetEnumLabel(Object o)
        {
            Type t = o.GetType();
            if (t == typeof(PhoneType))
            {
                return GetPhoneTypeLabel((PhoneType)o);
            }
            if (t == typeof(Gender))
            {
                return GetGenderLabel((Gender)o);
            }
            if (t == typeof(CommunicationOption))
            {
                return GetCommunicationOptionLabel((CommunicationOption)o);
            }
            if (t == typeof(Designation))
            {
                return GetDesignationLabel((Designation)o);
            }
            if (t == typeof(EmploymentStatus))
            {
                return GetEmploymentStatusLabel((EmploymentStatus)o);
            }
            if (t == typeof(FacilityType))
            {
                return GetFacilityTypeLabel((FacilityType)o);
            }
            if (t == typeof(EmploymentType))
            {
                return GetEmploymentTypeLabel((EmploymentType)o);
            }
            if (t == typeof(EmployerGroup))
            {
                return GetEmployerGroupLabel((EmployerGroup)o);
            }
            if (t == typeof(District))
            {
                return GetDistrictLabel((District)o);
            }
            if (t == typeof(Region))
            {
                return GetRegionLabel((Region)o);
            }
            if (t == typeof(Committee))
            {
                return GetCommitteeLabel((Committee)o);
            }
            if (t == typeof(CommitteePosition))
            {
                return GetCommitteePositionLabel((CommitteePosition)o);
            }
            if (t == typeof(LocalPosition))
            {
                return GetLocalPositionLabel((LocalPosition)o);
            }
            if (t == typeof(TableOfficerPosition))
            {
                return GetTableOfficerPositionLabel((TableOfficerPosition)o);
            }
            if (t == typeof(BoardOfDirectorsPosition))
            {
                return GetBoardOfDirectorsPositionLabel((BoardOfDirectorsPosition)o);
            }
            return "Missing Label";

        }

        public static string GetBoardOfDirectorsPositionLabel(BoardOfDirectorsPosition bdp)
        {
            switch (bdp)
            {
                case BoardOfDirectorsPosition.BursaryChairCentral:
                    return "VP Central Region";
                case BoardOfDirectorsPosition.BursaryChairEastern:
                    return "VP Eastern Region";
                case BoardOfDirectorsPosition.FinanceChair:
                    return "VP Finance";
                case BoardOfDirectorsPosition.NegotiatingPresident:
                    return "President";
                case BoardOfDirectorsPosition.NegotiatingVicePresident:
                    return "First Vice President";
                case BoardOfDirectorsPosition.BursaryChairNorthern:
                    return "VP Northern Region";
                case BoardOfDirectorsPosition.BursaryChairWestern:
                    return "VP Western Region";
                default:
                    return "Missing Label";
            }
        }

        private static string GetCommitteePositionLabel(CommitteePosition cp)
        {
            switch (cp)
            {
                case CommitteePosition.Central:
                    return "Central";
                case CommitteePosition.Chair:
                    return "Chair";
                case CommitteePosition.ChiefShopSteward:
                    return "Chief Shop Steward";
                case CommitteePosition.CommunityCare:
                    return "Community Care";
                case CommitteePosition.Eastern:
                    return "Eastern";
                case CommitteePosition.Iwk:
                    return "IWK";
                case CommitteePosition.Lpn:
                    return "LPN";
                case CommitteePosition.Ltc:
                    return "LTC";
                case CommitteePosition.Member:
                    return "Member";
                case CommitteePosition.Northern:
                    return "Northern";
                case CommitteePosition.President:
                    return "President";
                case CommitteePosition.Representative:
                    return "Representative";
                case CommitteePosition.Secretary:
                    return "Secretary";
                case CommitteePosition.Treasurer:
                    return "Treasurer";
                case CommitteePosition.VicePresident:
                    return "Vice President";
                case CommitteePosition.VpCommunityCare:
                    return "VP Community Care";
                case CommitteePosition.VpLpnGrad:
                    return "VP LPN Grad";
                case CommitteePosition.VpLtc:
                    return "VP LTC";
                case CommitteePosition.Western:
                    return "Western";
                default:
                    return "Missing Label";
            }
        }


        private static string GetCommitteeLabel(Committee c)
        {
            switch (c)
            {
                case Committee.Agm:
                    return "AGM";
                case Committee.BoardOfDirectors:
                    return "Board of Directors";
                case Committee.Buglm:
                    return "BUGLM";
                case Committee.Bursary:
                    return "Bursary";
                case Committee.Constitution:
                    return "Constitution";
                case Committee.Education:
                    return "Education";
                case Committee.Finance:
                    return "Finance";
                case Committee.Negotiating:
                    return "Negotiating";
                case Committee.Personnel:
                    return "Personnel";
                case Committee.UnionDiscipline:
                    return "Union Discipline";
                case Committee.UnionDisciplineAppeal:
                    return "Union Discipline Appeal";
                default:
                    return "Missing Label";
            }
        }

        private static string GetRegionLabel(Region r)
        {
            switch (r)
            {
                case Region.Central:
                    return "Central";
                case Region.Eastern:
                    return "Eastern";
                case Region.Northern:
                    return "Northern";
                case Region.Western:
                    return "Western";
                default:
                    return "Missing Label";
            }
        }

        private static string GetDistrictLabel(District d)
        {
            switch (d)
            {
                case District.Dha1:
                    return "DHA 1";
                case District.Dha2:
                    return "DHA 2";
                case District.Dha3:
                    return "DHA 3";
                case District.Dha4:
                    return "DHA 4";
                case District.Dha5:
                    return "DHA 5";
                case District.Dha6:
                    return "DHA 6";
                case District.Dha7:
                    return "DHA 7";
                case District.Dha8:
                    return "DHA 8";
                case District.Dha9:
                    return "DHA 9";
                case District.Iwk:
                    return "IWK";
                default:
                    return "Missing Label";
            }
        }

        private static string GetFacilityTypeLabel(FacilityType ft)
        {
            switch (ft)
            {
                case FacilityType.AcuteCare:
                    return "Acute Care";
                case FacilityType.CommunityCare:
                    return "Community Care";
                case FacilityType.LongTermCare:
                    return "Long Term Care";
                default:
                    return "Missing Label";
            }
        }

        private static string GetEmploymentTypeLabel(EmploymentType et)
        {
            switch (et)
            {
                case EmploymentType.Casual:
                    return "Casual";
                case EmploymentType.FullTime:
                    return "Full Time";
                case EmploymentType.PartTime:
                    return "Part Time";
                default:
                    return "Missing Label";
            }
        }

        private static string GetEmployerGroupLabel(EmployerGroup eg)
        {
            switch (eg)
            {
                case EmployerGroup.Gem:
                    return "GEM";
                case EmployerGroup.Hans:
                    return "HANS";
                case EmployerGroup.MacLeod:
                    return "MacLeod";
                case EmployerGroup.Shannex:
                    return "Shannex";
                case EmployerGroup.Von:
                    return "VON";
                case EmployerGroup.Rosecrest:
                    return "Rosecrest";
                default:
                    return "Missing Label";
            }
        }

        private static string GetEmploymentStatusLabel(EmploymentStatus es)
        {
            switch (es)
            {
                case EmploymentStatus.Active:
                    return "Active";
                case EmploymentStatus.Inactive:
                    return "Inactive";
                case EmploymentStatus.Loa:
                    return "LOA";
                case EmploymentStatus.Retired:
                    return "Retired";
                default:
                    return "Missing Label";
            }
        }

        private static string GetDesignationLabel(Designation d)
        {
            switch (d)
            {
                case Designation.Lpn:
                    return "LPN";
                case Designation.Np:
                    return "NP";
                case Designation.Rn:
                    return "RN";
                default:
                    return "Missing Label";
            }
        }

        private static string GetPhoneTypeLabel(PhoneType pt)
        {
            switch (pt)
            {
                case PhoneType.Cell:
                    return "Cell";
                case PhoneType.Fax:
                    return "Fax";
                case PhoneType.Home:
                    return "Home";
                case PhoneType.Work:
                    return "Work";
                default:
                    return "Missing Label";
            }
        }

        private static string GetGenderLabel(Gender g)
        {
            switch (g)
            {
                case Gender.Female:
                    return "Female";
                case Gender.Male:
                    return "Male";
                case Gender.TransgenderFemale:
                    return "Transgender Female";
                case Gender.TransgenderMale:
                    return "Transgender Male";
                default:
                    return "Missing Label";
            }
        }

        private static string GetCommunicationOptionLabel(CommunicationOption co)
        {
            switch (co)
            {
                case CommunicationOption.NoAil:
                    return "No Ail";
                case CommunicationOption.NoJohnsons:
                    return "No Johnsons";
                case CommunicationOption.NoUnionCalling:
                    return "No Union Calling";
                //case CommunicationOption.NoMailouts:
                //    return "No Mailouts";
                //case CommunicationOption.NoNewsletterByEmail:
                //    return "No Newsletters by Email";
                //case CommunicationOption.NoNewsletterByMail:
                //    return "No Newsletters by Mail";
                default:
                    return "Missing Label";
            }
        }

        private static string GetTableOfficerPositionLabel(TableOfficerPosition top)
        {
            switch (top)
            {
                case TableOfficerPosition.President:
                    return "President";
                case TableOfficerPosition.Secretary:
                    return "Secretary";
                case TableOfficerPosition.Treasurer:
                    return "Treasurer";
                case TableOfficerPosition.VicePresident:
                    return "Vice President";
                default:
                    return "Missing Label";
            }
        }

        private static string GetLocalPositionLabel(LocalPosition lp)
        {
            switch (lp)
            {
                case LocalPosition.LpnRepresentative:
                    return "LPN Rep.";
                case LocalPosition.ShopSteward:
                    return "Shop Steward";
                default:
                    return "Missing Label";
            }
        }

        
    }
}