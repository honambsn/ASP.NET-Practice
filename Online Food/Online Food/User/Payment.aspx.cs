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
			if (Session["UserID"] == null)
			{
				
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
			if (Session["UserID"] == null)
			{
				
			}
			else
			{
				Response.Redirect("Login.aspx");
			}
		}

		void OrderPayment(string name, string cardNo, string expDate, string cvv, string address, string paymentMode)
		{
			int paymentID;
			int productID;
			int quantity;
			dt = new DataTable();
			dt.Columns.AddRange(new DataColumn[] {
				new DataColumn("OrderNo", typeof(string)),
				new DataColumn("ProductID", typeof(int)),
				new DataColumn("Quantity", typeof(int)),
				new DataColumn("UserID", typeof(int)),
				new DataColumn("Status", typeof(int)),
				new DataColumn("OrderDate", typeof(DateTime)),
			});

			con = new SqlConnection(Connection.GetConnectionString());
			con.Open();
			transaction = con.BeginTransaction();

			con = new SqlConnection(Connection.GetConnectionString());
			cmd = new SqlCommand("Save_Payment", con, transaction);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Name", name);
			cmd.Parameters.AddWithValue("@CardNo", cardNo);
			cmd.Parameters.AddWithValue("@ExpDate", expDate);
			cmd.Parameters.AddWithValue("@Cvv", cvv);
			cmd.Parameters.AddWithValue("@Address", address);
			cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
			cmd.Parameters.Add("@InsertedID", SqlDbType.Int);
			cmd.Parameters["@InsertedID"].Direction = ParameterDirection.Output;
			try
			{
				cmd.ExecuteNonQuery();
				paymentID = Convert.ToInt32(cmd.Parameters["@InsertedID"].Value);

				cmd = new SqlCommand("Cart_Crud", con);
				cmd.Parameters.AddWithValue("@Action", "SELECT");
				cmd.Parameters.AddWithValue("UserID", Session["UserID"]);
				cmd.CommandType = CommandType.StoredProcedure;
				dr = cmd.ExecuteReader();
				
				while (dr.Read())
				{
					productID = Convert.ToInt32(dr["ProductID"]);
					quantity = Convert.ToInt32(dr["Quantity"]);

					// update product quantity

					// delete cart item
				}
			}
			catch (Exception ex)
			{
				try
				{
					transaction.Rollback();
				}
				catch (Exception ex2)
				{
					Response.Write("<script>alert('"+ex2.Message+"');</script>");
				}
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
		}


	//	void OrderPayment(string name, string cardNo, string expDate, string cvv, string address, string paymentMode)
	//	{
	//		int paymentID;
	//		int productID;
	//		int quantity;
	//		DataTable dt = new DataTable();

	//		// Cấu hình các cột của DataTable
	//		dt.Columns.AddRange(new DataColumn[] {
	//	new DataColumn("OrderNo", typeof(string)),
	//	new DataColumn("ProductID", typeof(int)),
	//	new DataColumn("Quantity", typeof(int)),
	//	new DataColumn("UserID", typeof(int)),
	//	new DataColumn("Status", typeof(int)),
	//	new DataColumn("OrderDate", typeof(DateTime)),
	//});

	//		try
	//		{
	//			// Sử dụng using để tự động quản lý kết nối và giao dịch
	//			using (con = new SqlConnection(Connection.GetConnectionString()))
	//			{
	//				con.Open();
	//				using (transaction = con.BeginTransaction())
	//				{
	//					// Lưu thông tin thanh toán vào cơ sở dữ liệu
	//					using (cmd = new SqlCommand("Save_Payment", con, transaction))
	//					{
	//						cmd.CommandType = CommandType.StoredProcedure;
	//						cmd.Parameters.AddWithValue("@Name", name);
	//						cmd.Parameters.AddWithValue("@CardNo", cardNo);
	//						cmd.Parameters.AddWithValue("@ExpDate", expDate);
	//						cmd.Parameters.AddWithValue("@Cvv", cvv);
	//						cmd.Parameters.AddWithValue("@Address", address);
	//						cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
	//						cmd.Parameters.Add("@InsertedID", SqlDbType.Int).Direction = ParameterDirection.Output;

	//						// Thực thi câu lệnh Stored Procedure
	//						cmd.ExecuteNonQuery();
	//						paymentID = Convert.ToInt32(cmd.Parameters["@InsertedID"].Value);
	//					}

	//					// Lấy thông tin giỏ hàng và xử lý chúng
	//					using (cmd = new SqlCommand("Cart_Crud", con, transaction))
	//					{
	//						cmd.Parameters.AddWithValue("@Action", "SELECT");
	//						cmd.Parameters.AddWithValue("UserID", Session["UserID"]);
	//						cmd.CommandType = CommandType.StoredProcedure;

	//						using (dr = cmd.ExecuteReader())
	//						{
	//							while (dr.Read())
	//							{
	//								productID = Convert.ToInt32(dr["ProductID"]);
	//								quantity = Convert.ToInt32(dr["Quantity"]);

	//								// Cập nhật số lượng sản phẩm
	//								using (cmd = new SqlCommand("Update_Product_Quantity", con, transaction))
	//								{
	//									cmd.Parameters.AddWithValue("@ProductID", productID);
	//									cmd.Parameters.AddWithValue("@Quantity", quantity);
	//									cmd.CommandType = CommandType.StoredProcedure;
	//									cmd.ExecuteNonQuery();
	//								}

	//								// Xóa sản phẩm khỏi giỏ hàng
	//								using (cmd = new SqlCommand("Delete_Cart_Item", con, transaction))
	//								{
	//									cmd.Parameters.AddWithValue("@ProductID", productID);
	//									cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
	//									cmd.CommandType = CommandType.StoredProcedure;
	//									cmd.ExecuteNonQuery();
	//								}
	//							}
	//						}
	//					}

	//					// Commit giao dịch nếu mọi thao tác thành công
	//					transaction.Commit();
	//				}
	//			}
	//		}
	//		catch (Exception ex)
	//		{
	//			// Xử lý lỗi và rollback giao dịch nếu có lỗi
	//			try
	//			{
	//				// Nếu có lỗi, rollback giao dịch
	//				transaction?.Rollback();
	//			}
	//			catch (Exception ex2)
	//			{
	//				// Hiển thị lỗi khi rollback thất bại
	//				Response.Write("<script>alert('Lỗi khi rollback: " + ex2.Message + "');</script>");
	//			}

	//			// Hiển thị lỗi chính
	//			Response.Write("<script>alert('Lỗi: " + ex.Message + "');</script>");
	//		}
	//	}


	}
}