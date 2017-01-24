using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Security;
using Nsnu.DataAccess;

namespace Nsnu.MembershipServices
{
    public class NsnuMembershipProvider : MembershipProvider
    {
        NsnuDataContext db = new NsnuDataContext(ConfigurationManager.ConnectionStrings["NsnuConnectionString"].ConnectionString);

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            MembershipBs mr = new MembershipBs();
            UserProfile up = mr.CreateUserProfile(username, password, email);
            status = MembershipCreateStatus.Success;
            return GetMembershipUser(up);
        }

        private MembershipUser GetMembershipUser(UserProfile up)
        {
            return new MembershipUser(this.Name, up.username, up.id, up.email, "", "", true, false, up.creationDate, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //  from the OneWeb membership provider oldPassword is now no longer supplied when user forgets password which calls changes password in the provider
            if (String.IsNullOrEmpty(oldPassword))
            {
                SimpleAES aes = new SimpleAES();
                string pw = aes.Encrypt(newPassword);

                MembershipBs mr = new MembershipBs();
                UserProfile up = mr.GetUserProfile(username);
                up.password = pw;
                mr.ProcessNurseProfile(up);
                return true;
            }
            else // we can validate the user 
            {
               if (ValidateUser(username, oldPassword))
                {
                    SimpleAES aes = new SimpleAES();
                    string pw = aes.Encrypt(newPassword);

                    MembershipBs mr = new MembershipBs();
                    UserProfile up = mr.GetUserProfile(username);
                    up.password = pw;
                    mr.ProcessNurseProfile(up);
                    return true;
                }
                return false;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || username.Length > 100) return false;

            MembershipBs mr = new MembershipBs();
            UserProfile up = mr.GetUserProfile(username, password);

            return (up == null) ? false : true;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();

        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipBs mr = new MembershipBs();
            UserProfile up = mr.GetUserProfile(username);

            return GetMembershipUser(up);
        }


        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 2; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
    }
}
