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
	public partial class Category : System.Web.UI.Page
	{
		SqlConnection cn;
		SqlCommand cmd;
		SqlDataReader dr;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}