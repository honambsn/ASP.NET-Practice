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
	public partial class Contact : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			try
			{
				con = new SqlConnection(Connection.GetConnectionString());
				cmd = new SqlCommand("ContactSp", con);
				cmd.Parameters.AddWithValue("@Action", "INSERT");
				cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
				cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
				cmd.Parameters.AddWithValue("@Subject", txtSubject.Text.Trim());
				cmd.Parameters.AddWithValue("@Message", txtMessage.Text.Trim());
				cmd.Parameters.AddWithValue("@Status", "0");
				cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
				cmd.CommandType = CommandType.StoredProcedure;

				if (con.State == System.Data.ConnectionState.Closed)
				{
					con.Open();
				}
				cmd.ExecuteNonQuery();
				lblMsg.Visible = true;
				lblMsg.Text = "Thanks for reaching out will look into your query!!";
				lblMsg.CssClass = "alert alert-success";
				clear();

			}
			catch (Exception ex)
			{
				Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
			}
			finally
			{
				if (con != null && con.State == System.Data.ConnectionState.Open)
				{
					con.Close();
				}
			}
		}

		private void clear()
		{
			txtName.Text = string.Empty;
			txtEmail.Text = string.Empty;
			txtSubject.Text = string.Empty;
			txtMessage.Text = string.Empty;
		}
	}
}