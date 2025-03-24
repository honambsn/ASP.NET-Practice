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
	public partial class Invoice : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserID"] != null)
				{
					if (Request.QueryString["id"] != null)
					{
						rOrderItem.DataSource = GetOrderDetails();
						rOrderItem.DataBind();
					}
					else
					{
						lblMsg.Text = "Payment ID bị thiếu.";
						lblMsg.Visible = true;
					}
				}
				else
				{
					Response.Redirect("Login.aspx");
				}
			}
		}

		DataTable GetOrderDetails()
		{
			double grandTotal = 0;
			con = new SqlConnection(Connection.GetConnectionString());
			cmd = new SqlCommand("Invoice", con);
			cmd.Parameters.AddWithValue("@Action", "INVOICBYID");
			cmd.Parameters.AddWithValue("@PaymentID", Convert.ToInt32(Request.QueryString["id"]));
			cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));
			cmd.CommandType = CommandType.StoredProcedure;
			sda = new SqlDataAdapter(cmd);
			dt = new DataTable();
			sda.Fill(dt);
			if (dt.Rows.Count > 0)
			{
				foreach(DataRow drow in dt.Rows)
				{
					grandTotal += Convert.ToDouble(drow["TotalPrice"]);
				}
			}
			DataRow dr = dt.NewRow();
			dr["TotalPrice"] = grandTotal;
			dt.Rows.Add(dr);
			return dt;
		}
	}
}