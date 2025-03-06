using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Food.User
{
	public partial class User : System.Web.UI.MasterPage
	{
		protected void Page_Init(object sender, EventArgs e)
		{

		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.Url.AbsoluteUri.ToString().Contains("Default.aspx"))
			{
				form1.Attributes.Add("class", "sub_page");
			}
			else
			{
				form1.Attributes.Remove("class");

				//load control
				Control sliderUserControl = (Control)Page.LoadControl("SliderUserControl1.ascx");


				//add the control to the panel
				pnlSliderUC.Controls.Add(sliderUserControl);
			}

			if (Session["UserID"] != null)
			{
				lbLoginOrLogout.Text = "Logout";
				Utils utils = new Utils();
				Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["UserID"])).ToString();
			}
			else
			{
				lbLoginOrLogout.Text = "Login";
				Session["cartCount"] = "0";
			}
			
		}

		protected void lbLoginOrLogout_Click(object sender, EventArgs e)
		{
			if (Session["UserID"] == null)
			{
				Response.Redirect("Login.aspx");
			}
			else
			{
				Session.Abandon();
				Response.Redirect("Login.aspx");
			}
		}

		protected void lbRegisterOrProfile_Click(object sender, EventArgs e)
		{
			if (Session["UserID"] != null)
			{
				lbRegisterOrProfile.ToolTip = "Profile";
				Response.Redirect("Profile.aspx");
			}
			else
			{
				//Session.Abandon();
				lbRegisterOrProfile.ToolTip = "Resgistration";
				Response.Redirect("Registration.aspx");
			}

		}
	}
}