using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Job_Portal.Admin
{
    public partial class JobList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ShowJob();
        }

        private void ShowJob()
        {
            string query = string.Empty;

            using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
            {
                cm.Open();

                query = @"select row_number() over(order by (select 1)) as [Sr.No], JobID, Title, NoOfPost, Qualification, Experience,
                        LastDateToApply, CompanyName, Country, State, CreateDate from Jobs";

                SqlCommand cmd = new SqlCommand(query, cm);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                sda.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();

            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("~/User/Login.aspx");
            }

            if (!IsPostBack)
            {
                ShowJob();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowJob();
        }
    }
}