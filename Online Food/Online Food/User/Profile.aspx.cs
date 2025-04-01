using System;
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
					getPurchaseHistory();
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


		void getPurchaseHistory()
		{
			int sr = 1;
			Utils utils = new Utils();
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Invoice", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "ODRHISTORY");
					cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);
						rPurchaseHistory.DataSource = dt;
						dt.Columns.Add("SrNo", typeof(Int32));
						if (dt.Rows.Count > 0)
						{
							foreach (DataRow dataRow in dt.Rows)
							{
								dataRow["SrNo"] = sr;
								sr++;
							}
						}
						if (dt.Rows.Count == 0)
						{
							rPurchaseHistory.FooterTemplate = null;
							rPurchaseHistory.FooterTemplate = new CustomTemplate(ListItemType.Footer);
						}
						Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["UserID"]));
						rPurchaseHistory.DataBind();
					}
				}
			}
		}

		private sealed class CustomTemplate : ITemplate
		{
			private ListItemType ListItemType { get; set; }

			public CustomTemplate(ListItemType type)
			{
				ListItemType = type;
			}

			public void InstantiateIn(Control container)
			{
				if (ListItemType == ListItemType.Footer)
				{
					var footer = new LiteralControl("<tr><td><b>Hungry!!! Why not order food for you.</b><a href='Menu.aspx' class='badge badge-info ml-2'>Click to Order</a></td></tr></tbody><table>");
					container.Controls.Add(footer);
				}
			}
		}

		protected void rPurchaseHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			double grandTotal = 0;
			HiddenField paymentID = e.Item.FindControl("hdnPaymentID") as HiddenField;
			Repeater repOrders = e.Item.FindControl("rOrders") as Repeater;
			Utils utils = new Utils();
			if (paymentID != null)
			{
				using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
				{
					if (cm.State == ConnectionState.Closed)
					{
						cm.Open();
					}

					using (SqlCommand cmd = new SqlCommand("Invoice", cm))
					{
						cmd.Parameters.AddWithValue("@Action", "INVOICBYID");
						cmd.Parameters.AddWithValue("@PaymentID", Convert.ToInt32(paymentID.Value));
						cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
						cmd.CommandType = CommandType.StoredProcedure;
						using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
						{
							DataTable dt = new DataTable();
							sda.Fill(dt);
							//rPurchaseHistory.DataSource = dt;
							//dt.Columns.Add("SrNo", typeof(Int32));
							if (dt.Rows.Count > 0)
							{
								foreach (DataRow dataRow in dt.Rows)
								{
									grandTotal += Convert.ToDouble(dataRow["TotalPrice"]);
								}
							}
							if (dt.Rows.Count == 0)
							{
								rPurchaseHistory.FooterTemplate = null;
								rPurchaseHistory.FooterTemplate = new CustomTemplate(ListItemType.Footer);
							}
							//Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["UserID"]));
							DataRow dr = dt.NewRow();
							dr["TotalPrice"] = grandTotal;
							dt.Rows.Add(dr);
							repOrders.DataSource = dt;
							repOrders.DataBind();
						}
					}
				}
			}
			else if (paymentID == null)
			{
				Console.WriteLine("No payment ID found");
				Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAz");
			}
		}
	}
}