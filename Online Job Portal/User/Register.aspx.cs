using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Job_Portal.User
{
    public partial class Register : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM JobCategory", cm);
                    
                    if (cm.State == System.Data.ConnectionState.Closed)
                    {
                        cm.Open();
                    }

                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlCountry.DataSource = reader;
                    ddlCountry.DataTextField = "CountryName";
                    ddlCountry.DataBind();

                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

        }
    }
}