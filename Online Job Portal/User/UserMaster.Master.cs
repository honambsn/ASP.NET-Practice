using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Job_Portal.User
{
    public partial class UserMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                lbRegisterOrProfile.Text = "Profile";
                lbLoginOrLogout.Text = "Logout";
            }
            else
            {
                lbRegisterOrProfile.Text = "Register";
                lbLoginOrLogout.Text = "Login";
            }
        }

        protected void lbRegisterOrProfile_Click(object sender, EventArgs e)
        {
            if (lbRegisterOrProfile.Text == "Profile")
            {
                Response.Redirect("~/User/Profile.aspx");
            }
            else
            {
                Response.Redirect("~/User/Register.aspx");
            }
        }

        protected void lbLoginOrLogout_Click(object sender, EventArgs e)
        {
            if (lbLoginOrLogout.Text == "Login")
            {
                Session.Clear();
                Response.Redirect("~/User/Login.aspx");
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/User/Login.aspx");
            }
        }
    }
}