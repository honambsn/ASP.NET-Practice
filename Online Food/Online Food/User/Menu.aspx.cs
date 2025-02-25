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
				bool isCartItemUpdated = false;
				int i = isItemExistInCart(Convert.ToInt32(e.CommandArgument.ToString())); 
				if (i == 0)
				{
					//adding new item in cart
					using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
					{
						if (cm.State == ConnectionState.Closed)
						{
							cm.Open();
						}
						try
						{
							using (SqlCommand cmd = new SqlCommand("Cart_Crud", cm))
							{
								cmd.Parameters.AddWithValue("@Action", "INSERT");
								cmd.Parameters.AddWithValue("@ProductID", e.CommandArgument);
								cmd.Parameters.AddWithValue("@Quantity", 1);
								cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.ExecuteNonQuery();
							}
						}
						catch (Exception ex)
						{
							Response.Write("<script>alert('Error - " + ex.Message +"');<script>");
						}
						finally
						{
							cm.Close();
						}
					}
				}
				else
				{
					// adding existing item into cart

					Utils utils = new Utils();
					isCartItemUpdated = utils.updateCartQuantity(i + 1, Convert.ToInt32(e.CommandArgument),
						Convert.ToInt32(Session["UserID"]));
					lblMsg.Visible = true;
					lblMsg.Text = "Item added successfully in ur cart, quantity updated";
					lblMsg.CssClass = "alert alert-success";
					Response.AddHeader("REFRESH", "1;URL=Cart.aspx");
				}
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
					cmd.Parameters.AddWithValue("@Action", "GETBYID");
					cmd.Parameters.AddWithValue("@ProductID", productID);
					cmd.Parameters.AddWithValue("UserID", Session["UserID"]);
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);
						int quantity = 0;
						if (dt.Rows.Count > 0)
						{
							quantity = Convert.ToInt32(dt.Rows[0]["Quantity"].ToString());
						}
						
						// Bind the result to the rProduct control
						rProducts.DataSource = dt;
						rProducts.DataBind();

						return quantity;
					}
				}
			}
		}
	}
}