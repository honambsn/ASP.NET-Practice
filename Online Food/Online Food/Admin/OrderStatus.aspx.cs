using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Food.Admin
{
	public partial class OrderStatus : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{
			Console.WriteLine("order status page load");
			if (!IsPostBack)
			{
				Session["breadCrum"] = "Order Status";
				if (Session["admin"] == null)
				{
					Response.Redirect("../User/Login.aspx");
				}
				else
				{
					Console.WriteLine("order status");
					getOrderStatus();
				}
			}
			lblMsg.Visible = false;
			pUpdateOrderStatus.Visible = false;
		}

		private void getOrderStatus()
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Invoice", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "GETSTATUS");
					cmd.CommandType = CommandType.StoredProcedure;

					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						if (dt.Rows.Count > 0)
						{
							Console.WriteLine("ok");
							rOrderStatus.DataSource = dt;
							rOrderStatus.DataBind();
						}
						else
						{
							Console.Write("none");
							lblMsg.Text = "Không có đơn hàng.";
							lblMsg.Visible = true;
						}
					}
				}
			}
		}

		protected void rOrderStatus_ItemCommand(object source, RepeaterCommandEventArgs e)
		{

		}

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }

		protected void btnCancel_Click(object sender, EventArgs e)
		{

		}
	}
}