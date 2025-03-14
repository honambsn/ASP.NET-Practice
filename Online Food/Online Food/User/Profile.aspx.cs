﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Online_Food.User
{
	public partial class Profile : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserID"] == null)
				{
					Response.Redirect("Login.aspx");
				}
				else
				{
					getUserDetails();
				}
			}
		}


		private void getUserDetails()
		{
			try
			{
				using (con = new SqlConnection(Connection.GetConnectionString()))
				{
					cmd = new SqlCommand("User_Crud", con);
					cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "SELECT4PROFILE";
					cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = Session["UserID"];
					cmd.CommandType = CommandType.StoredProcedure;

					sda = new SqlDataAdapter(cmd);
					dt = new DataTable();
					sda.Fill(dt);

					if (dt.Rows.Count > 0)
					{
						rUserProfile.DataSource = dt;
						rUserProfile.DataBind();

						// Update session values if data is available
						if (dt.Rows.Count == 1)
						{
							Session["name"] = dt.Rows[0]["Name"].ToString();
							Session["email"] = dt.Rows[0]["Email"].ToString();
							Session["imageUrl"] = dt.Rows[0]["ImageUrl"].ToString();
							Session["createdDate"] = dt.Rows[0]["CreatedDate"].ToString();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Response.Write("Err: " + ex.Message);
			}
		}


	}
}