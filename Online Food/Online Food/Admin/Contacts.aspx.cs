using Online_Food.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
			lblMsg.Visible = true;
			lblMsg.Text = "Reply updated";
			lblMsg.CssClass = "alert alert-success";
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

		protected void btnConfirmUpdate_Click(object sender, EventArgs e)
		{
			// Perform the update operation after confirmation
			lblMsg.Visible = true;
			lblMsg.Text = "Reply updated";
			lblMsg.CssClass = "alert alert-success";

			// Hide the confirmation panel and show the original panel
			pConfirmUpdate.Visible = false;
			pReply.Visible = true;
		}

		protected void btnCancelUpdate_Click(object sender, EventArgs e)
		{
			// Cancel the update and hide the confirmation panel
			pConfirmUpdate.Visible = false;
			pReply.Visible = true;
		}

		private void EditReply(SqlConnection cm, RepeaterCommandEventArgs e)
		{
			using (SqlCommand cmd = new SqlCommand("ContactSp", cm))
			{
				cmd.Parameters.Clear();

				cmd.Parameters.AddWithValue("@Action", "GETBYID");
				//cmd.Parameters.AddWithValue
			}
		}
	}
}



//public void ReplyButton_Click(object sender, EventArgs e)
//{
//	// Lấy FeedbackID từ UI (ví dụ từ nút nhấn của người dùng)
//	int feedbackID = GetSelectedFeedbackID();

//	// Kiểm tra xem contact đã có phản hồi chưa
//	bool replyExists = CheckIfReplyExists(feedbackID);

//	if (replyExists)
//	{
//		// Nếu đã có phản hồi, thực hiện cập nhật
//		UpdateReply(feedbackID, "Tên Admin", "Cập nhật tin nhắn phản hồi");
//	}
//	else
//	{
//		// Nếu chưa có phản hồi, thực hiện thêm mới
//		InsertReply(feedbackID, "Tên Admin", "Tin nhắn phản hồi mới");
//	}
//}

//// Kiểm tra xem phản hồi đã tồn tại cho FeedbackID chưa
//private bool CheckIfReplyExists(int feedbackID)
//{
//	string query = "SELECT COUNT(*) FROM dbo.tblReplies WHERE FeedbackID = @FeedbackID";
//	int count = ExecuteScalar(query, new SqlParameter("@FeedbackID", feedbackID));
//	return count > 0;
//}

//// Cập nhật phản hồi nếu đã có
//private void UpdateReply(int feedbackID, string adminName, string replyMsg)
//{
//	string procedureName = "Reply_Crud";
//	ExecuteNonQuery(procedureName,
//					new SqlParameter("@Action", "UPDATE"),
//					new SqlParameter("@FeedbackID", feedbackID),
//					new SqlParameter("@AdminName", adminName),
//					new SqlParameter("@ReplyMsg", replyMsg));
//}

//// Thêm mới phản hồi nếu chưa có
//private void InsertReply(int feedbackID, string adminName, string replyMsg)
//{
//	string newReplyID = Guid.NewGuid().ToString("N").Substring(0, 10);  // Tạo ReplyID mới
//	string procedureName = "Reply_Crud";
//	ExecuteNonQuery(procedureName,
//					new SqlParameter("@Action", "INSERT"),
//					new SqlParameter("@ReplyID", newReplyID),
//					new SqlParameter("@FeedbackID", feedbackID),
//					new SqlParameter("@AdminName", adminName),
//					new SqlParameter("@ReplyMsg", replyMsg));
//}
