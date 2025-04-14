using Online_Food.User;
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
	public partial class Contacts : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Session["breadCrum"] = "Contact Users";
				if (Session["admin"] == null)
				{
					Response.Redirect("../User/Login.aspx");
				}
				else
				{
					getContacts();
				}
			}
		}

		private void getContacts()
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}
				con = new SqlConnection(Connection.GetConnectionString());
				cmd = new SqlCommand("ContactSp", con);
				cmd.Parameters.AddWithValue("@Action", "SELECT");
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				sda = new SqlDataAdapter(cmd);
				dt = new DataTable();
				sda.Fill(dt);
				rContacts.DataSource = dt;
				rContacts.DataBind();
			}
		}

		protected void rContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				try
				{
					if (e.CommandName == "delete")
					{
						using (SqlCommand cmd = new SqlCommand("ContactSp", cm))
						{
							cmd.Parameters.AddWithValue("@Action", "DELETE");
							cmd.Parameters.AddWithValue("@ContactID", e.CommandArgument);
							cmd.CommandType = CommandType.StoredProcedure;

							cmd.ExecuteNonQuery();

							lblMsg.Visible = true;
							lblMsg.Text = "Record deleted successfully";
							lblMsg.CssClass = "alert alert-success";

							// Refresh category list after deletion
							getContacts();
						}
					}
					else if (e.CommandName == "reply")
					{
						Response.Redirect("ContactDetails.aspx?ContactID=" + e.CommandArgument.ToString());
					}

				}
				catch (Exception ex)
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Error: " + ex.Message;
					lblMsg.CssClass = "alert alert-danger";
				}
				finally
				{
					if (con != null && con.State == ConnectionState.Open)
					{
						con.Close();
					}
				}
			}
		}
    }
}