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



namespace WebApplication.Admin
{
    public partial class Test : System.Web.UI.Page
    {

        private MembershipBs _mBs = new MembershipBs();
    //    private PasswordUtils.Security _pUtil = new PasswordUtils.Security();


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_onClick(object sender, EventArgs e)
        {
            //if (!ValidateNurse())
            //{
            //    FormErrors = _ser.Serialize(_formErrors);
            //    return;
            //}
            //ProcessNurse();
            string uname = GenerateUniqueUserName("Penney", "C", "1W");
           // string password = PasswordUtils.Security.HashPassword(uname);


            SimpleAES aes = new SimpleAES();


          //  string password = aes.Encrypt("ADAMSWA");
            string password = GeneratePassword("ADAMSWA");


            Regex rgx = new Regex("^[a-zA-Z0-9]{8}$");
            bool b = rgx.IsMatch("12345678");

            string s = String.Concat(uname, DateTime.Now.ToString());
           // up.password = _aes.Encrypt(String.Concat(up.username, up.creationDate.ToString()));

        }


        //LastName + middle initial + first letter of first name + numerical suffix if required.
        private String GenerateUniqueUserName(string lastname, string middleinitial, string firstname)
        {
            string username = String.Concat(lastname, String.IsNullOrEmpty(middleinitial) ? String.Empty : middleinitial, firstname[0]);

            //strip any non-alphanumeric characters
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            username = rgx.Replace(username, "");

            // check the db to see if this username is used, add 1, then 2, then 3 etc until a unique username is found
            if (_mBs.GetUserProfile(username) != null)
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


                } while (_mBs.GetUserProfile(username) != null);
            }

            // not sure if this is necessary, all usernames in db are curently uppercase...
            return username.ToUpper();
        }






        private string GeneratePassword(string username)
        {
            string hexPwd;
            System.Text.StringBuilder hexString = new System.Text.StringBuilder(64);

            byte[] bytes = new byte[username.Length * sizeof(char)];
            System.Buffer.BlockCopy(username.ToCharArray(), 0, bytes, 0, bytes.Length);

            SimpleAES aes = new SimpleAES();
            byte[] b = aes.Encrypt(bytes);

            for (int i = 0; i < b.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", b[i]));
            }

            // remove similar characters (I and 1, 0 and O)
            hexPwd = Regex.Replace(hexString.ToString().ToUpper(), "(I|1|0|O)", "");

            return hexPwd.Substring(1, 8);
        }
    }
}