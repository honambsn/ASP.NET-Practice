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

			//con = new SqlConnection(Connection.GetConnectionString());
			//if (con.State == ConnectionState.Closed)
			//{
			//	con.Open();
			//}

			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Save_Payment", cm))
				{
					cmd.Parameters.AddWithValue("@Name", name);
					cmd.Parameters.AddWithValue("@CardNo", cardNo);
					cmd.Parameters.AddWithValue("@ExpDate", expDate);
					cmd.Parameters.AddWithValue("@Cvv", cvv);
					cmd.Parameters.AddWithValue("@Address", address);
					cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
					cmd.Parameters.Add("@InsertedID", SqlDbType.Int);
					cmd.Parameters["@InsertedID"].Direction = ParameterDirection.Output;

					cmd.CommandType = CommandType.StoredProcedure;

					try
					{
						cmd.ExecuteNonQuery();
						paymentID = Convert.ToInt32(cmd.Parameters["@InsertedID"].Value);

						using (SqlConnection cm1 = new SqlConnection(Connection.GetConnectionString()))
						{
							if (cm1.State == ConnectionState.Closed)
							{
								cm1.Open();
							}

							using (SqlCommand cmd1 = new SqlCommand("Cart_Crud", cm1))
							{
								cmd1.Parameters.AddWithValue("@Action", "SELECT");
								cmd1.Parameters.AddWithValue("UserID", Session["UserID"]);
								cmd1.CommandType = CommandType.StoredProcedure;

								dr = cmd1.ExecuteReader();
								while (dr.Read())
								{
									productID = Convert.ToInt32(dr["ProductID"]);
									//productID = (int)dr["ProductID"];
									quantity = Convert.ToInt32(dr["Quantity"]);


									dt.Rows.Add(dr["OrderNo"], productID, quantity, Session["UserID"], 1, DateTime.Now);
								}
							}
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
					finally
					{
						if (con.State == ConnectionState.Open)
						{
							con.Close();
						}
					}


				}
			}
		}

	}
}