using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nsnu.MembershipServices;


namespace WebApplication
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



        }

        protected void btnSubmit_onClick(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                string commaSeperatedRoles = string.Empty;

                //Authenticate user against the user database and obtain comma seperated roles
                if (!UserAuthentication.Instance.AuthenticateUser(tbUserName.Text, tbPassword.Text, out commaSeperatedRoles))
                {
                    // lblLoginFailed.Visible = true;
                    return;
                }

                //Instead of FormsAuthentication.RedirectFromLoginPage(txtUser.Text, false);
                //Use the following code
                FormsAuthenticationUtils.RedirectFromLoginPage(tbUserName.Text, commaSeperatedRoles, true);
            }


        }
    }
}