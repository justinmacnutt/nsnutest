using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.UserControls
{
    public partial class AuthWidget : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                pnlLoggedIn.Visible = true;
                litUserName.Text = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                pnlLoggedIn.Visible = false;
            }

        }

        protected void btnLogout_onClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Login.aspx");
        }
    }
}