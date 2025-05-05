using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Food.Admin
{
	public partial class Report : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Session["breadCrum"] = "Selling Report";
				if (Session["admin"] == null)
				{
					Response.Redirect("~/Admin/Login.aspx");
				}

			}
		}

		private void getContacts()
		{
			con =  new SqlConnection(Connection.GetConnectionString());

		}

		protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

	
    }
}