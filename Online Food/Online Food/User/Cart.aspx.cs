using Online_Food.Admin;
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
	public partial class Cart : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		decimal grandTotal = 0;
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
					getCartItem();
				}
			}
		}

		void getCartItem()
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Cart_Crud", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "SELECT");
					cmd.Parameters.AddWithValue("UserID", Session["UserID"]);
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);
						rCartItem.DataSource = dt;
						if (dt.Rows.Count == 0)
						{
							rCartItem.FooterTemplate = null;
							rCartItem.FooterTemplate = new CustomTemplate(ListItemType.Footer);
						}

						rCartItem.DataBind();
					}
				}
			}
		}

		protected void rCartItem_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			Utils utils = new Utils();
			if (e.CommandName == "Remove")
			{
				int productID = Convert.ToInt32(e.CommandArgument);
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
							cmd.Parameters.AddWithValue("@Action", "DELETE");
							cmd.Parameters.AddWithValue("@ProductID", productID);
							cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.ExecuteNonQuery();
							//Response.Write("<script>alert('Item removed from cart.');</script>");
							//cart count
							Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["UserID"]));
							getCartItem();
						}
						

					}
					catch (Exception ex)
					{
						Response.Write("<script>alert('Error - " + ex.Message + "');</script>");
					}
					finally
					{
						cm.Close();
					}
				}
			}
		}

		protected void rCartItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Label totalPrice = e.Item.FindControl("lblTotalPrice") as Label;
				Label productPrice = e.Item.FindControl("lblPrice") as Label;
				TextBox quantity = e.Item.FindControl("txtQuantity") as TextBox;
				decimal calTotalPrice = Convert.ToDecimal(productPrice.Text) * Convert.ToDecimal(quantity.Text);
				totalPrice.Text = calTotalPrice.ToString();
				grandTotal += calTotalPrice;
			}

			Session["grandTotalPrice"] = grandTotal;
		}

		private sealed class CustomTemplate : ITemplate
		{
			private ListItemType ListItemType {get; set;}

			public CustomTemplate(ListItemType type)
			{
				ListItemType = type;
			}

			public void InstantiateIn(Control container)
			{
				if (ListItemType == ListItemType.Footer) { 
					var footer = new LiteralControl("<tr><td colspan='5'><b>Your Cart is empty.</b><a href='Menu.aspx' class='badge badge-info ml-2'>Continue Shopping</a></td></tr></tbody><table>");
					container.Controls.Add(footer);
				}
			}
		}
	}
}