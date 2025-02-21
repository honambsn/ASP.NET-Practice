using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Online_Food.Admin;

namespace Online_Food.User
{
	public partial class Menu : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				getCategories();
				getProducts();
			}
		}

		private void getCategories()
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Category_Crud", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "ACTIVECAT");
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						// Bind the result to the rptCategory control
						rCategory.DataSource = dt;
						rCategory.DataBind();
					}
				}
			}

		}


		private void getProducts()
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Product_Crud", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "ACTIVEPROD");
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						// Bind the result to the rProduct control
						rProducts.DataSource = dt;
						rProducts.DataBind();
					}
				}
			}

		}

		public string LowerCase(object obj)
		{
			return obj.ToString().ToLower();
		}

		protected void rProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (Session["UserID"] != null)
			{

			}
			else
			{
				Response.Redirect("Login.aspx");
			}
		}

		int isItemExistInCart(int productID)
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Cart_Crud", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "ACTIVEPROD");
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						// Bind the result to the rProduct control
						rProducts.DataSource = dt;
						rProducts.DataBind();
					}
				}
			}
		}
	}
}