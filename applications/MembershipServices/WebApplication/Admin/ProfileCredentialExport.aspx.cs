using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nsnu.MembershipServices;


namespace WebApplication.Admin
{
   

    public partial class ProfileCredentialExport : System.Web.UI.Page
    {
        private SimpleAES _aes = new SimpleAES();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGeneratePasswords_onClick(object sender, EventArgs e)
        {
            MembershipBs bs = new MembershipBs();

            var upl = bs.GetAllUserProfiles();

            foreach (var up in upl)
            {
                System.Threading.Thread.Sleep(20);
                if (up.id != 0)
                {
                    up.password = MembershipBs.GeneratePassword();    
                }
                
                bs.ProcessNurseProfile();
            }
            
        }

        protected void btnEncryptPasswords_onClick(object sender, EventArgs e)
        {
            MembershipBs bs = new MembershipBs();

            var upl = bs.GetAllUserProfiles();

            foreach (var up in upl)
            {
                //up.password = MembershipBs.GeneratePassword();
                up.password = _aes.Encrypt(up.password);
                bs.ProcessNurseProfile();
            }
        }
    }
}