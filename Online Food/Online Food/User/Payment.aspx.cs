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
	public partial class Payment : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		SqlDataReader dr, dr1;
		DataTable dt;
		SqlTransaction transaction = null;
		string _name = string.Empty;
		string _cardNo = string.Empty;
		string _expDate = string.Empty;
		string _cvv = string.Empty;
		string _address = string.Empty;
		string _paymentMode = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserID"] == null)
				{
					Response.Redirect("Login.aspx");
				}
			}
		}

		protected void lbCardSubmit_Click(object sender, EventArgs e)
		{
			_name = txtName.Text.Trim();
			_cardNo = txtCardNo.Text.Trim();
			_cardNo = string.Format("************{0}", txtCardNo.Text.Trim().Substring(12, 4));
			_expDate = txtExpMonth.Text.Trim() + "/" + txtExpYear.Text.Trim();
			_cvv = txtCvv.Text.Trim();
			_address = txtAddress.Text.Trim();
			_paymentMode = "card";
			_paymentMode = _paymentMode.ToLower();
			if (Session["UserID"] != null)
			{
				OrderPayment(_name, _cardNo, _expDate, _cvv, _address, _paymentMode);
			}
			else
			{
				Response.Redirect("Login.aspx");
			}
		}

		protected void lbCodSubmit_Click(object sender, EventArgs e)
		{
			_address = txtCODAddress.Text.Trim();
			_paymentMode = "cod";
			_paymentMode = _paymentMode.ToLower();
			if (Session["UserID"] != null)
			{
				OrderPayment(_name, _cardNo, _expDate, _cvv, _address, _paymentMode);
			}
			else
			{
				Response.Redirect("Login.aspx");
			}
		}

		void OrderPayment(string name, string cardNo, string expDate, string cvv, string address, string paymentMode)
		{
			if (dr1 != null && !dr1.IsClosed)
				dr1.Close();
			int paymentID;
			int productID;
			int quantity;
			dt = new DataTable();
			dt.Columns.AddRange(new DataColumn[7] {
				new DataColumn("OrderNo", typeof(string)),
				new DataColumn("ProductID", typeof(int)),
				new DataColumn("Quantity", typeof(int)),
				new DataColumn("UserID", typeof(int)),
				new DataColumn("Status", typeof(string)),
				new DataColumn("PaymentID", typeof(int)),
				new DataColumn("OrderDate", typeof(DateTime)),
			});

			#region old code
			using (con = new SqlConnection(Connection.GetConnectionString()))
			{
				con.Open();
				SqlTransaction transaction = con.BeginTransaction();

				try
				{
					// Save Payment
					cmd = new SqlCommand("Save_Payment", con, transaction);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Name", name);
					cmd.Parameters.AddWithValue("@CardNo", cardNo);
					cmd.Parameters.AddWithValue("@ExpiryDate", expDate);
					cmd.Parameters.AddWithValue("@CvvNo", cvv);
					cmd.Parameters.AddWithValue("@Address", address);
					cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
					cmd.Parameters.Add("@InsertedID", SqlDbType.Int).Direction = ParameterDirection.Output;

					cmd.ExecuteNonQuery();
					paymentID = Convert.ToInt32(cmd.Parameters["@InsertedID"].Value);

					// Get Cart Items
					cmd = new SqlCommand("Cart_Crud", con, transaction);
					cmd.Parameters.AddWithValue("@Action", "SELECT");
					cmd.Parameters.AddWithValue("UserID", Session["UserID"]);
					cmd.CommandType = CommandType.StoredProcedure;

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							productID = Convert.ToInt32(dr["ProductID"]);
							quantity = Convert.ToInt32(dr["Quantity"]);

							// Update product quantity
							UpdateQuantity(productID, quantity, transaction, con);
							// Delete cart item
							DeleteCartItem(productID, transaction, con);

							dt.Rows.Add(Utils.GetUniqueID(), productID, quantity, (int)Session["UserID"], "Pending", paymentID, Convert.ToDateTime(DateTime.Now));
						}
					}

					// Save Orders
					if (dt.Rows.Count > 0)
					{
						cmd = new SqlCommand("Save_Orders", con, transaction);
						cmd.Parameters.AddWithValue("@tblOrders", dt);
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.ExecuteNonQuery();
					}

					transaction.Commit();

					lblMsg.Visible = true;
					lblMsg.Text = "Your item ordered successfully. Thank you for shopping with us.";
					lblMsg.CssClass = "alert alert-success";
					Response.AddHeader("REFRESH", "1;URL=Invoice.aspx?id=" + paymentID);
				}
				catch (Exception ex)
				{
					try
					{
						transaction.Rollback();
					}
					catch (Exception ex2)
					{
						Response.Write("<script>alert('Rollback Error: " + ex2.Message + "');</script>");
					}

					Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
				}
			}
			#endregion old code

			con = new SqlConnection(Connection.GetConnectionString());
			con.Open();

			#region sql transaction
			transaction = con.BeginTransaction();
			cmd = new SqlCommand("Save_Payment", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Name", name);
			cmd.Parameters.AddWithValue("@CardNo", cardNo);
			cmd.Parameters.AddWithValue("@ExpiryDate", expDate);
			cmd.Parameters.AddWithValue("@CvvNo", cvv);
			cmd.Parameters.AddWithValue("@Address", address);
			cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
			cmd.Parameters.Add("@InsertedID", SqlDbType.Int);
			cmd.Parameters["@InsertedID"].Direction = ParameterDirection.Output;

			try
			{
				cmd.ExecuteNonQuery();
				paymentID = Convert.ToInt32(cmd.Parameters["@InsertedID"].Value);

				#region Getting Cart Item's
				cmd = new SqlCommand("Cart_Crud", con);
				cmd.Parameters.AddWithValue("@Action", "SELECT");
				cmd.Parameters.AddWithValue("UserID", Session["UserID"]);
				cmd.CommandType = CommandType.StoredProcedure;
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					productID = Convert.ToInt32(dr["ProductID"]);
					quantity = Convert.ToInt32(dr["Quantity"]);

					// Update product quantity
					UpdateQuantity(productID, quantity, transaction, con);
					// update product quantity end

					//delete cart item
					DeleteCartItem(productID, transaction, con);
					//delete cart item end

					dt.Rows.Add(Utils.GetUniqueID(), productID, quantity, (int)Session["UserID"], "Pending", 
						paymentID, Convert.ToDateTime(DateTime.Now));
				}
				dr.Close();
				#endregion Getting Cart Item's

				#region Order Details
				if (dt.Rows.Count > 0)
				{
					cmd = new SqlCommand("Save_Orders", con, transaction);
					cmd.Parameters.AddWithValue("@tblOrders", dt);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.ExecuteNonQuery();
				}
				#endregion Order Details

				transaction.Commit();
				lblMsg.Visible = true;
				lblMsg.Text = "Your item ordered successfully. Thank you for shopping with us.";
				lblMsg.CssClass = "alert alert-success";
				Response.AddHeader("REFRESH", "1;URL=Invoice.aspx?id=" + paymentID);
			}
			catch (Exception ex)
			{
				try
				{
					transaction.Rollback();
				}
				catch (Exception ex2)
				{
					//Response.Write("<script>alert('Rollback Error: " + ex2.Message + "');</script>");
					Response.Write("<script>alert('" + ex.Message + "');</script>");
				}
			}
			#endregion sql transaction
			finally
			{
				con.Close();
			}
		}


		void UpdateQuantity(int _productID, int _quantity, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
		{
			if (dr1 != null && !dr1.IsClosed)
				dr1.Close();
			int dbQuantity = 0;
			dr1 = null;
			//cmd = new SqlCommand("Product_Crud", sqlConnection, sqlTransaction);
			cmd = new SqlCommand("Product_Crud", sqlConnection, sqlTransaction);
			cmd.Parameters.AddWithValue("@Action", "GETBYID");
			cmd.Parameters.AddWithValue("@ProductID", _productID);
			cmd.CommandType = CommandType.StoredProcedure;
			try
			{
				dr1 = cmd.ExecuteReader();
				while (dr1.Read())
				{
					dbQuantity = Convert.ToInt32(dr1["Quantity"]);

					if (dbQuantity > _quantity && dbQuantity > 2)
					{
						dbQuantity = dbQuantity - _quantity;

						cmd = new SqlCommand("Product_Crud", sqlConnection, sqlTransaction);
						cmd.Parameters.AddWithValue("@Action", "QTYUPDATE");
						cmd.Parameters.AddWithValue("@Quantity", dbQuantity);
						cmd.Parameters.AddWithValue("@ProductID", _productID);
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.ExecuteNonQuery();
					}
				}
				dr1.Close();
			}
			catch (Exception ex)
			{
				Response.Write("<script>alert('" + ex.Message + "');</script>");
			}
			finally
			{
				if (dr1 != null && !dr1.IsClosed)
					dr1.Close();
			}
		}
		

		void DeleteCartItem(int _productID, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
		{
			cmd = new SqlCommand("Cart_Crud", sqlConnection, sqlTransaction);
			cmd.Parameters.AddWithValue("@Action", "DELETE");
			cmd.Parameters.AddWithValue("@ProductID", _productID);
			cmd.Parameters.AddWithValue("UserID", Session["UserID"]);
			cmd.CommandType = CommandType.StoredProcedure;
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Response.Write("<script>alert('" + ex.Message + "');</script>");
			}
		}

	}
}