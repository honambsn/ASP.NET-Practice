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
				cm.Open();
				con = cm;
				cmd = new SqlCommand("ContactSp", con);
				cmd.Parameters.AddWithValue("@Action", "SELECT");
				cmd.CommandType = CommandType.StoredProcedure;
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
			//hdnId.Value = "0";
		}


		private void clear()
		{
			txtAdminName.Text = string.Empty;
			txtFeedbackID.Text = string.Empty;
			txtReplyMsg.Text = string.Empty;
			//hd.Value = "0";
			btnAdd.Text = "Add";
		}

		//private void UpdateReply(SqlConnection cm, RepeaterCommandEventArgs e)
		//{
		//	using (SqlCommand cmd = new SqlCommand("Reply_Crud", cm))
		//	{
		//		cmd.Parameters.Clear();

		//		cmd.Parameters.AddWithValue("@Action", "GETBYID");

		//		string replyID = (e.Item.FindControl("hdnReplyID") as HiddenField)?.Value;
		//		if (string.IsNullOrEmpty(replyID))
		//		{
		//			lblMsg.Visible = true;
		//			lblMsg.Text = "Không tìm thấy ReplyID.";
		//			lblMsg.CssClass = "alert alert-danger";
		//			return;
		//		}
		//		cmd.Parameters.AddWithValue("@ReplyID", replyID);
		//		cmd.Parameters.AddWithValue("@FeedbackID", e.CommandArgument);

		//		//cmd.CommandType = CommandType.StoredProcedure;

		//		try
		//		{
		//			using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
		//			{
		//				DataTable dt = new DataTable();
		//				sda.Fill(dt);

		//				if (dt.Rows.Count > 0)
		//				{
		//					txtAdminName.Text = dt.Rows[0]["AdminName"].ToString();
		//					string newReplyID = Guid.NewGuid().ToString().Substring(0, 10);
		//					cmd.Parameters.AddWithValue("@ReplyID", newReplyID); // Ensure ReplyID is 10 characters long

		//					//txtFeedbackID.Text = dt.Rows[0]["FeedbackID"].ToString();
		//					//txtReplyDate.Text = dt.Rows[0]["ReplyDate"].ToString();
		//					txtReplyMsg.Text = dt.Rows[0]["ReplyMsg"].ToString();

		//					LinkButton btn = e.Item.FindControl("btnUpdate") as LinkButton;
		//					if (btn != null)
		//					{
		//						btn.CssClass = "badge badge-warning";
		//					}
		//				}
		//				//else
		//				//{
		//				//	txtAdminName.Text = "NULL";
		//				//	txtFeedbackID.Text = "NULL";
		//				//	txtReplyDate.Text = "NULL";
		//				//	txtReplyMsg.Text = "NULL";
		//				//}
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			lblMsg.Visible = true;
		//			lblMsg.Text = "Error: " + ex.Message;
		//			lblMsg.CssClass = "alert alert-danger";
		//		}
		//		finally
		//		{
		//			if (con != null && con.State == ConnectionState.Open)
		//			{
		//				con.Close();
		//			}
		//		}
		//	}
		//}

		//protected void btnUpdate_Click(object sender, EventArgs e)
		//{
		//	using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
		//	{
		//		int feedbackId;
		//		if (!int.TryParse(txtFeedbackID.Text, out feedbackId))
		//		{
		//			lblMsg.Visible = true;
		//			lblMsg.Text = "Invalid Feedback ID.";
		//			lblMsg.CssClass = "alert alert-danger";
		//			return;
		//		}

		//		if (cm.State == ConnectionState.Closed)
		//		{
		//			cm.Open();
		//		}

		//		try
		//		{
		//			using (SqlCommand cmd = new SqlCommand("Reply_Crud", cm))
		//			{

		//				cmd.Parameters.Clear();
		//				cmd.CommandType = CommandType.StoredProcedure;

		//				bool isInsert = string.IsNullOrEmpty(hdnId.Value) || hdnId.Value == "0";

		//				cmd.Parameters.AddWithValue("@Action", isInsert ? "INSERT" : "UPDATE");
		//				if (isInsert)
		//				{
		//					cmd.Parameters.AddWithValue("@ReplyID", Guid.NewGuid().ToString().Substring(0, 10));
		//				}
		//				else
		//				{
		//					cmd.Parameters.AddWithValue("@ReplyID", hdnId.Value);
		//				}
		//				cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);
		//				cmd.Parameters.AddWithValue("@AdminName", txtAdminName.Text.Trim());
		//				cmd.Parameters.AddWithValue("@ReplyMsg", txtReplyMsg.Text.Trim());



		//				cmd.ExecuteNonQuery();
		//				string actionName = feedbackId == 0 ? "INSERT" : "UPDATE";




		//				lblMsg.Visible = true;
		//				lblMsg.Text = "Reply " + (isInsert ? "inserted" : "updated") + " successfully.";
		//				lblMsg.CssClass = "alert alert-success";

		//				// Refresh contacts after reply
		//				getContacts();

		//				clear();
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			lblMsg.Visible = true;
		//			lblMsg.Text = "Error: " + ex.Message;
		//			lblMsg.CssClass = "alert alert-danger";
		//		}
		//		finally
		//		{
		//			pReply.Visible = false;
		//			txtReplyMsg.Text = "";
		//			if (cm != null && cm.State == ConnectionState.Open)
		//			{
		//				cm.Close();
		//			}


		//		}
		//	}
		//}

		private void UpdateReply(SqlConnection cm, RepeaterCommandEventArgs e)
		{
			try
			{
				string feedbackID = e.CommandArgument.ToString(); // feedbackID từ CommandArgument
				txtFeedbackID.Text = feedbackID; // Điền giá trị vào textbox tương ứng

				// Kiểm tra nếu đã có phản hồi cho FeedbackID này
				SqlCommand checkCmd = new SqlCommand("Reply_Crud", cm);
				checkCmd.CommandType = CommandType.StoredProcedure;
				checkCmd.Parameters.AddWithValue("@Action", "GETBYID");
				checkCmd.Parameters.AddWithValue("@FeedbackID", Convert.ToInt32(feedbackID));

				SqlDataAdapter sda = new SqlDataAdapter(checkCmd);
				DataTable dt = new DataTable();
				sda.Fill(dt);

				if (dt.Rows.Count > 0)
				{
					// Nếu đã có phản hồi, có thể chỉnh sửa
					txtReplyMsg.Text = dt.Rows[0]["ReplyMsg"].ToString();
					txtAdminName.Text = dt.Rows[0]["AdminName"].ToString();
					txtReplyDate.Text = dt.Rows[0]["ReplyDate"].ToString();
					btnUpdate.Visible = true; // Hiển thị nút update
					btnAdd.Visible = false;
				}
				else
				{
					// Nếu chưa có phản hồi, chuẩn bị cho việc thêm mới
					txtReplyMsg.Text = "null";
					txtAdminName.Text = "null";
					txtReplyDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					btnAdd.Visible = true;
					btnUpdate.Visible = false;
				}
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "Error: " + ex.Message;
				lblMsg.CssClass = "alert alert-danger";
			}
		}


		//protected void btnAdd_Click(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		string replyMessage = txtReplyMsg.Text.Trim();
		//		int feedbackID = Convert.ToInt32(txtFeedbackID.Text);
		//		string adminName = txtAdminName.Text;
		//		string replyDate = txtReplyDate.Text;

		//		if (string.IsNullOrEmpty(replyMessage))
		//		{
		//			lblMsg.Visible = true;
		//			lblMsg.Text = "Please enter a reply message.";
		//			lblMsg.CssClass = "alert alert-danger";
		//			return;
		//		}

		//		// Generate a new ReplyID or retrieve existing one
		//		string replyID = "R" + DateTime.Now.Ticks.ToString(); // You can generate this ID based on your own logic

		//		using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
		//		{
		//			cm.Open();
		//			using (SqlCommand cmd = new SqlCommand("Reply_Crud", cm))
		//			{
		//				cmd.Parameters.AddWithValue("@Action", "INSERT");
		//				cmd.Parameters.AddWithValue("@ReplyID", replyID);
		//				cmd.Parameters.AddWithValue("@FeedbackID", feedbackID);
		//				cmd.Parameters.AddWithValue("@AdminName", adminName);
		//				cmd.Parameters.AddWithValue("@ReplyMsg", replyMessage);
		//				cmd.CommandType = CommandType.StoredProcedure;

		//				cmd.ExecuteNonQuery();

		//				lblMsg.Visible = true;
		//				lblMsg.Text = "Reply submitted successfully.";
		//				lblMsg.CssClass = "alert alert-success";

		//				// Hide the reply panel after submission
		//				pReply.Visible = false;

		//				// Refresh contacts list to show the new reply
		//				getContacts();
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		lblMsg.Visible = true;
		//		lblMsg.Text = "Error: " + ex.Message;
		//		lblMsg.CssClass = "alert alert-danger";
		//	}
		//}


		protected void btnUpdateReply_Click(object sender, EventArgs e)
		{
			try
			{
				string replyMessage = txtReplyMsg.Text.Trim();
				int feedbackID = Convert.ToInt32(txtFeedbackID.Text);
				string adminName = txtAdminName.Text;
				string replyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

				if (string.IsNullOrEmpty(replyMessage))
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Please enter a reply message.";
					lblMsg.CssClass = "alert alert-danger";
					return;
				}

				// Generate a new ReplyID if it's a new reply or retrieve existing one if it's an update
				string replyID = "R" + DateTime.Now.Ticks.ToString(); // You can replace this with your own logic for generating unique IDs

				using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
				{
					cm.Open();
					using (SqlCommand cmd = new SqlCommand("Reply_Crud", cm))
					{
						if (Convert.ToBoolean(ViewState["isUpdate"])) // Check if it's an update
						{
							// Update the existing reply
							cmd.Parameters.AddWithValue("@Action", "UPDATE");
							cmd.Parameters.AddWithValue("@ReplyID", replyID); // This should be the existing ReplyID
							cmd.Parameters.AddWithValue("@FeedbackID", feedbackID);
							cmd.Parameters.AddWithValue("@AdminName", adminName);
							cmd.Parameters.AddWithValue("@ReplyMsg", replyMessage);
							cmd.Parameters.AddWithValue("@ReplyDate", replyDate);
						}
						else
						{
							// Insert a new reply if no reply exists
							cmd.Parameters.AddWithValue("@Action", "INSERT");
							cmd.Parameters.AddWithValue("@ReplyID", replyID);
							cmd.Parameters.AddWithValue("@FeedbackID", feedbackID);
							cmd.Parameters.AddWithValue("@AdminName", adminName);
							cmd.Parameters.AddWithValue("@ReplyMsg", replyMessage);
							cmd.Parameters.AddWithValue("@ReplyDate", replyDate);
						}
						cmd.CommandType = CommandType.StoredProcedure;

						cmd.ExecuteNonQuery();

						lblMsg.Visible = true;
						lblMsg.Text = "Reply successfully " + (Convert.ToBoolean(ViewState["isUpdate"]) ? "updated" : "submitted") + ".";
						lblMsg.CssClass = "alert alert-success";

						// Hide the reply panel after submission
						pReply.Visible = false;

						// Refresh contacts list to show the new reply
						getContacts();
					}
				}
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "Error: " + ex.Message;
				lblMsg.CssClass = "alert alert-danger";
			}
		}


		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			try
			{
				string adminName = txtAdminName.Text.Trim();
				string replyMsg = txtReplyMsg.Text.Trim();
				int feedbackID = Convert.ToInt32(txtFeedbackID.Text.Trim());

				using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
				{
					cm.Open();

					SqlCommand cmd = new SqlCommand("Reply_Crud", cm);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Action", "UPDATE");
					cmd.Parameters.AddWithValue("@FeedbackID", feedbackID); // Using FeedbackID for update
					cmd.Parameters.AddWithValue("@AdminName", adminName);
					cmd.Parameters.AddWithValue("@ReplyMsg", replyMsg);

					cmd.ExecuteNonQuery();
				}

				lblMsg.Visible = true;
				lblMsg.Text = "Reply updated successfully!";
				lblMsg.CssClass = "alert alert-success";

				pReply.Visible = false; // Hide the reply form after updating
				getContacts(); // Refresh the contacts list
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "Error: " + ex.Message;
				lblMsg.CssClass = "alert alert-danger";
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				string replyID = "R" + Guid.NewGuid().ToString(); // Generate unique ReplyID
				string adminName = txtAdminName.Text.Trim();
				string replyMsg = txtReplyMsg.Text.Trim();
				int feedbackID = Convert.ToInt32(txtFeedbackID.Text.Trim());

				using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
				{
					cm.Open();

					SqlCommand cmd = new SqlCommand("Reply_Crud", cm);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Action", "INSERT");
					cmd.Parameters.AddWithValue("@ReplyID", replyID);
					cmd.Parameters.AddWithValue("@FeedbackID", feedbackID);
					cmd.Parameters.AddWithValue("@AdminName", adminName);
					cmd.Parameters.AddWithValue("@ReplyMsg", replyMsg);

					cmd.ExecuteNonQuery();
				}

				lblMsg.Visible = true;
				lblMsg.Text = "Reply added successfully!";
				lblMsg.CssClass = "alert alert-success";

				pReply.Visible = false; // Hide the reply form after adding
				getContacts(); // Refresh the contacts list
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "Error: " + ex.Message;
				lblMsg.CssClass = "alert alert-danger";
			}
		}

	}
}
