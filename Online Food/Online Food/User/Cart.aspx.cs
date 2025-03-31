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
			Utils utils = new Utils();
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Cart_Crud", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "SELECT");
					cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
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
						Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["UserID"]));
						rCartItem.DataBind();
					}
				}
			}
		}

		protected void rCartItem_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			Utils utils = new Utils();
			// watch out the command name spelling
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

							lblMsg.Visible = true;
							lblMsg.Text = "Remove Successfully";
							lblMsg.CssClass = "alert alert-warning";

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
			else if (e.CommandName == "updateCart")
			{
				bool isCartUpdated = false;
				for (int item = 0; item < rCartItem.Items.Count; item++)
				{
					if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
					{
						TextBox quantity = rCartItem.Items[item].FindControl("txtQuantity") as TextBox;
						HiddenField _productID = rCartItem.Items[item].FindControl("hdnProductID") as HiddenField;
						HiddenField _quantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;


						int quantityFromCart = Convert.ToInt32(quantity.Text);
						int productID = Convert.ToInt32(_productID.Value);
						int quantityFromDB = Convert.ToInt32(_quantity.Value);

						bool isTrue = false;
						int updatedQuantity = 1;

						if (quantityFromCart > quantityFromDB)
						{
							updatedQuantity = quantityFromCart;
							isTrue = true;
						}
						else if (quantityFromCart < quantityFromDB)
						{
							updatedQuantity = quantityFromCart;
							isTrue = true;
						}

						if (isTrue)
						{
							//update cart item quantity in DB
							isCartUpdated = utils.updateCartQuantity(updatedQuantity, productID, Convert.ToInt32(Session["UserID"]));
						}

					}
				}
				lblMsg.Visible = true;
				lblMsg.Text = "Updated Successfully";
				lblMsg.CssClass = "alert alert-success";
				getCartItem();
			}

			if (e.CommandName == "checkout")
			{
				bool isTrue = true;
				string pName = string.Empty;
				for (int item = 0; item < rCartItem.Items.Count; item++)
				{
					//check item quantity before get
					if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
					{
						HiddenField _productID = rCartItem.Items[item].FindControl("hdnProductID") as HiddenField;
						HiddenField _cartQuantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;
						HiddenField _productQuantity = rCartItem.Items[item].FindControl("hdnProdQuantity") as HiddenField;
						Label productName = rCartItem.Items[item].FindControl("lblProductName") as Label;


						int productID = Convert.ToInt32(_productID.Value);
						int cartQuantity = Convert.ToInt32(_cartQuantity.Value);
						int productQuantity = Convert.ToInt32(_productQuantity.Value);

						if (productQuantity < cartQuantity || productQuantity <= 2)
						{
							isTrue = false;
							pName = productName.Text;
							break;
						}
					}
				}

				if (isTrue)
				{
					Response.Redirect("Payment.aspx", false);
				}
				else
				{
					lblMsg.Visible = true;
					lblMsg.Text ="Item <b>'" + pName + "' is out of stock.";
					lblMsg.CssClass = "alert alert-warning";
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