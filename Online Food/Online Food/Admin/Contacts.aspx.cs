using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace Online_Food.Admin
{
	public partial class Contacts : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Session["breadCrum"] = "Contact Users";
				if (Session["admin"] == null)
				{
					Response.Redirect("../User/Login.aspx");
				}
				else
				{
					getContacts();
				}
				pReply.Visible = false;

			}
			lblMsg.Visible = false;
			pReply.Visible = false;
		}

		private void getContacts()
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}
				con = new SqlConnection(Connection.GetConnectionString());
				cmd = new SqlCommand("ContactSp", con);
				cmd.Parameters.AddWithValue("@Action", "SELECT");
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				sda = new SqlDataAdapter(cmd);
				dt = new DataTable();
				sda.Fill(dt);
				rContacts.DataSource = dt;
				rContacts.DataBind();
			}
		}


		protected void rContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				try
				{
					if (e.CommandName == "delete")
					{
						#region old code
						//using (SqlCommand cmd = new SqlCommand("ContactSp", cm))
						//{
						//	cmd.Parameters.AddWithValue("@Action", "DELETE");
						//	cmd.Parameters.AddWithValue("@ContactID", e.CommandArgument);
						//	cmd.CommandType = CommandType.StoredProcedure;

						//	cmd.ExecuteNonQuery();

						//	lblMsg.Visible = true;
						//	lblMsg.Text = "Record deleted successfully";
						//	lblMsg.CssClass = "alert alert-success";

						//	// Refresh category list after deletion
						//	getContacts();
						//}
						DeleteContact(Convert.ToInt32(e.CommandArgument));
						#endregion
					}
					else if (e.CommandName == "reply")
					{
						pReply.Visible = true;
						txtReplyMsg.Text = string.Empty;
						UpdateReply(cm, e);
						//pConfirmUpdate.Visible = true;

					}

				}
				catch (Exception ex)
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Error: " + ex.Message;
					lblMsg.CssClass = "alert alert-danger";
				}
				finally
				{
					if (con != null && con.State == ConnectionState.Open)
					{
						con.Close();
					}
				}
			}
		}


		private void DeleteContact(int contactId)
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				cm.Open();
				using (SqlCommand cmd = new SqlCommand("ContactSp", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "DELETE");
					cmd.Parameters.AddWithValue("@ContactID", contactId);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.ExecuteNonQuery();
				}

				lblMsg.Visible = true;
				lblMsg.Text = "Record deleted successfully.";
				lblMsg.CssClass = "alert alert-success";

				// Refresh contacts after deletion
				getContacts();
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			//pConfirmUpdate.Visible = true;
			txtReplyMsg.Text = "cancel";
			lblMsg.Visible = true;
			lblMsg.Text = "Reply processing cancelled";
			lblMsg.CssClass = "alert alert-danger";
			txtReplyMsg.Text = "";
			pReply.Visible = false;
			hdnId.Value = "0";
		}

		

		private void UpdateReply(SqlConnection cm, RepeaterCommandEventArgs e)
		{
			using (SqlCommand cmd = new SqlCommand("Reply_Crud", cm))
			{
				cmd.Parameters.Clear();

				cmd.Parameters.AddWithValue("@Action", "GETBYID");
				cmd.Parameters.AddWithValue("@FeedbackID", e.CommandArgument);
				cmd.CommandType = CommandType.StoredProcedure;
				
				try
				{
					using(SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						if (dt.Rows.Count > 0)
						{
							txtAdminName.Text = dt.Rows[0]["AdminName"].ToString();
							string newReplyID = Guid.NewGuid().ToString().Substring(0, 10);
							cmd.Parameters.AddWithValue("@ReplyID", newReplyID); // Ensure ReplyID is 10 characters long

							//txtFeedbackID.Text = dt.Rows[0]["FeedbackID"].ToString();
							//txtReplyDate.Text = dt.Rows[0]["ReplyDate"].ToString();
							txtReplyMsg.Text = dt.Rows[0]["ReplyMsg"].ToString();

							LinkButton btn = e.Item.FindControl("btnUpdate") as LinkButton;
							if (btn != null)
							{
								btn.CssClass = "badge bdagege-warning";
							}
						}
						else
						{
							txtAdminName.Text = "NULL";
							txtFeedbackID.Text = "NULL";
							txtReplyDate.Text = "NULL";
							txtReplyMsg.Text = "NULL";
						}
					}
				}
				catch (Exception ex)
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Error: " + ex.Message;
					lblMsg.CssClass = "alert alert-danger";
				}
				finally
				{
					if (con != null && con.State == ConnectionState.Open)
					{
						con.Close();
					}
				}
			}
		}

		private void clear()
		{
			txtAdminName.Text = string.Empty;
			txtFeedbackID.Text = string.Empty;
			txtReplyMsg.Text = string.Empty;
			hdnId.Value = "0";
			btnUpdate.Text = "Add";
		}

		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				string actionName = string.Empty;
				int feedbackId;
				if (!int.TryParse(hdnId.Value, out feedbackId))
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Invalid Feedback ID.";
					lblMsg.CssClass = "alert alert-danger";
				}
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				try
				{
					using (SqlCommand cmd = new SqlCommand("Reply_Crud", cm))
					{

						cmd.Parameters.Clear();
						clear();
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Action", feedbackId == 0 ? "INSERT" : "UPDATE");
						cmd.Parameters.AddWithValue("@ReplyID", Guid.NewGuid().ToString().Substring(0, 10)); // Ensure ReplyID is 10 characters long
						cmd.Parameters.AddWithValue("@AdminName", txtAdminName.Text.Trim());
						//cmd.Parameters.AddWithValue("@FeedbackID", int.Parse(txtFeedbackID.Text));
						//{
						//	lblMsg.Text = "Invalid Feedback ID.";
						//	return;
						//}
						//cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);
						//cmd.Parameters.AddWithValue("@FeedbackID", int.Parse(txtFeedbackID.Text)); // Ensure txtFeedbackID has a valid value
						//cmd.Parameters.AddWithValue("@ReplyDate", DateTime.Now);
						cmd.Parameters.AddWithValue("@ReplyMsg", txtReplyMsg.Text.Trim());


						cmd.ExecuteNonQuery();
						actionName = feedbackId == 0 ? "INSERT" : "UPDATE";




						lblMsg.Visible = true;
						lblMsg.Text = "Product " + actionName + " successfully.";
						lblMsg.CssClass = "alert alert-success";

						// Refresh contacts after reply
						getContacts();

						clear();
					}
				}
				catch (Exception ex)
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Error: " + ex.Message;
					lblMsg.CssClass = "alert alert-danger";
				}
				finally
				{
					pReply.Visible = false;
					txtReplyMsg.Text = "";
					if (con != null && con.State == ConnectionState.Open)
					{
						con.Close();
					}


				}
			}
		}
	}
}

