using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Food.User
{
	public partial class Payment : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void lbCardSubmit_Click(object sender, EventArgs e)
		{
			Response.Redirect("OrderSuccess.aspx");
		}

		protected void lbCodSubmit_Click(object sender, EventArgs e)
		{
			Response.Redirect("OrderSuccess.aspx");
		}

	}
}