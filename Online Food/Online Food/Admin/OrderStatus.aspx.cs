using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

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
			if (!IsPostBack)
			{
				Session["breadCrum"] = "Order Status";
				if (Session["admin"] == null)
				{
					Response.Redirect("../User/Login.aspx");
				}
				else
				{
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

		protected void btnUpdate_Click(object sender, EventArgs e)
        {
			int orderDetailsID = Convert.ToInt32(hdnId.Value);
			con = new SqlConnection(Connection.GetConnectionString());
			cmd = new SqlCommand("Invoice", con);
			cmd.Parameters.AddWithValue("@Action", "UPDTSTATUS");
			cmd.Parameters.AddWithValue("@OrderDetailsID", orderDetailsID);
			cmd.Parameters.AddWithValue("@Status", ddlOrderStatus.SelectedValue);
			cmd.CommandType = CommandType.StoredProcedure;

			try
			{
				if (con.State == ConnectionState.Closed)
				{
					con.Open();
				}

				// Execute the stored procedure
				cmd.ExecuteNonQuery();
				lblMsg.Visible = true;
				lblMsg.Text = "Order status updated successfully.";
				lblMsg.CssClass = "alert alert-success";

				// Clear form and reset state
				getOrderStatus();
				//cbIsActive.Checked = false; // If necessary
				//hdnId.Value = "0"; // If necessary
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "Error: " + ex.Message;
				lblMsg.CssClass = "alert alert-danger";
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}

		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			pUpdateOrderStatus.Visible = false;
			hdnId.Value = "0";
		}

		protected void rOrderStatus_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				try
				{
					if (e.CommandName == "edit")
					{
						EditOrderStatus(cm, e);
					}
					// no delete the order receipt
					//else if (e.CommandName == "delete")
					//{
					//	//DeleteCategory(cm, e);
					//	lblMsg.Visible = true;
					//	lblMsg.Text = "Implementing";
					//	lblMsg.CssClass = "alert alert-danger";
					//}
				}
				catch (Exception ex)
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Error: " + ex.Message;
					lblMsg.CssClass = "alert alert-danger";
				}
			}
		}

		private void EditOrderStatus(SqlConnection cm, RepeaterCommandEventArgs e)
		{
			using (SqlCommand cmd = new SqlCommand("Invoice", cm))
			{
				cmd.Parameters.AddWithValue("@Action", "STATUSBYID");
				cmd.Parameters.AddWithValue("@OrderDetailsID", e.CommandArgument);
				cmd.CommandType = CommandType.StoredProcedure;

				using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
				{
					DataTable dt = new DataTable();
					sda.Fill(dt);

					if (dt.Rows.Count > 0)
					{
						ddlOrderStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
						hdnId.Value = dt.Rows[0]["OrderDetailsID"].ToString();
						pUpdateOrderStatus.Visible = true;

						// Change button class to show it's in 'edit' mode
						LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
						if (btn != null) btn.CssClass = "badge badge-warning";
					}
				}
			}
		}
	}
}