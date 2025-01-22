using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Food.User
{
	public partial class Login : System.Web.UI.Page
	{
		SqlConnection cn;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["userId"] != null)
			{
				Response.Redirect("Default.aspx");
			}
		}

        protected void btnLogin_Click(object sender, EventArgs e)
        {
			if (txtUsername.Text.Trim() == "Admin" && txtPassword.Text.Trim() == "123")
			{
				Session["admin"] = txtUsername.Text.Trim();
				Response.Redirect("../Admin/Dashboard.aspx");
			}
			else
			{
				try
				{
					cn = new SqlConnection(Connection.GetConnectionString());
					cmd = new SqlCommand("User_Crud", cn);
					cmd.CommandType = CommandType.StoredProcedure;

					// Add parameters for action, username, and password
					cmd.Parameters.AddWithValue("@Action", "SELECT4LOGIN");
					cmd.Parameters.AddWithValue("@UserName", txtUsername.Text.Trim());
					cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim()); // Use txtPassword for password

					sda = new SqlDataAdapter(cmd);
					dt = new DataTable();
					sda.Fill(dt);

					if (dt.Rows.Count == 1)
					{
						Session["UserName"] = txtUsername.Text.Trim();
						Session["UserID"] = dt.Rows[0]["UserID"];
						//Response.Redirect("Default.aspx");
						Response.Redirect("../Admin/Dashboard.aspx");
					}
					else
					{
						lblMsg.Visible = true;
						lblMsg.Text = "Invalid Credentials...!";
						lblMsg.CssClass = "alert alert-danger";
					}
				}
				catch (Exception ex)
				{
					// Handle any exceptions that may occur
					lblMsg.Visible = true;
					lblMsg.Text = "An error occurred: " + ex.Message;
					lblMsg.CssClass = "alert alert-danger";
				}
				finally
				{
					if (cn != null && cn.State == ConnectionState.Open)
						cn.Close();
				}
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{

		}
	}
}