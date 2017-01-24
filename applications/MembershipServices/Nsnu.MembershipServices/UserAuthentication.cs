using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Nsnu.MembershipServices
{
    public class UserAuthentication
    {
        public static UserAuthentication Instance
        {
            get
            {
                return new UserAuthentication();
            }
        }

        public bool AuthenticateUser(string userName, string password, out string commaSeperatedRoles)
        {
            bool success = false;
            commaSeperatedRoles = string.Empty;


            if (Membership.ValidateUser(userName, password))
            {
                commaSeperatedRoles = String.Join(",", Roles.GetRolesForUser(userName));
                success = true;
            }

            return success;
        }
    }
}
