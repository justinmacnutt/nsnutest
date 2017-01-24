using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nsnu.DataAccess.Enumerations
{
    public enum District
    {
        Dha1 = 1,
        Dha2 = 2,
        Dha3 = 3,
        Dha4 = 4,
        Dha5 = 5,
        Dha6 = 6,
        Dha7 = 7,
        Dha8 = 8,
        Dha9 = 9,
        Iwk = 10
    }

    public enum Region
    {
        Central = 1,
        Western = 2,
        Northern = 3,
        Eastern = 4
    }
    
    public enum FacilityType
    {
        LongTermCare = 1,
        CommunityCare = 2,
        AcuteCare = 3
    }

    public enum AddressType
    {
        Mailing = 1,
        Business = 2
    }
    
    public enum Committee
    {
        BoardOfDirectors = 1,
        Negotiating = 2,
        Agm = 3,
        Constitution = 4,
        Education = 5,
        Finance = 6,
        Personnel = 7,
        Buglm = 8,
        Bursary = 9,
        UnionDiscipline = 10,
        UnionDisciplineAppeal = 11
    }


    public enum Designation
    {
        Rn = 1,
        Np = 2,
        Lpn = 3
    }

    public enum EmployerGroup
    {
        Gem = 1,
        Hans = 2,
        MacLeod = 3,
        Shannex = 4,
        Von = 5,
        Rosecrest = 6
    }

    public enum EmploymentStatus
    {
        Active = 1,
        Inactive = 2,
        Loa = 3,
        Retired = 4
    }

    public enum EmploymentType
    {
        FullTime = 1,
        PartTime = 2,
        Casual = 3
    }

    public enum LocalPosition
    {
        ShopSteward = 1,
        LpnRepresentative = 2
    }

    public enum TableOfficerPosition
    {
        President = 1,
        VicePresident = 2,
        Treasurer = 3,
        Secretary = 4
    }

    public enum BoardOfDirectorsPosition
    {
        NegotiatingPresident = 1,
        NegotiatingVicePresident = 2,
        BursaryChairCentral = 3,
        BursaryChairNorthern = 4,
        BursaryChairEastern = 5,
        BursaryChairWestern = 6,
        FinanceChair = 7
    }

    public enum CommitteePosition
    {
        President = 1,
        Chair = 2,
        Member = 3,
        VicePresident = 4,
        Western = 5,
        Northern = 6,
        Eastern = 7, 
        Central = 8,
        Iwk = 9,
        Lpn = 10,
        Ltc = 11,
        CommunityCare = 12,
        VpLpnGrad = 13,
        VpLtc = 14,
        VpCommunityCare = 15,
        Secretary = 16,
        Treasurer = 17,
        ChiefShopSteward = 18,
        Representative = 19,
        VpIwk = 20
    }
    
    public enum PhoneType
    {
        Home = 1,
        Work = 2,
        Cell = 3,
        Fax = 4
    }

    public enum CommunicationOption
    {
        NoAil = 1,
        NoJohnsons = 2,
        NoUnionCalling = 6
        //NoNewsletterByMail = 3,
        //NoNewsletterByEmail = 4,
        //NoMailouts = 5
    }

    public enum Gender
    {
        Female = 1,
        Male = 2,
        TransgenderFemale = 3,
        TransgenderMale = 4
    }
}
