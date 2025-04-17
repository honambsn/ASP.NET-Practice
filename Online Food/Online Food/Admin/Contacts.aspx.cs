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
			}
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

		//LinkButton "Reply"
		//protected void lnkReply_Command(object sender, CommandEventArgs e)
		//{
		//	if (e.CommandName == "reply")
		//	{
		//		//get contact id from command argument
		//		int contactId = Convert.ToInt32(e.CommandArgument);

		//		// get specific contact details
		//		var contactDetails = GetContactDetails(contactId);

		//		if (contactDetails != null)
		//		{
		//			// Điền sẵn email của contact vào một trường (nếu cần)
		//			// txtReplyTo.Text = contactDetails.Email;  // Nếu có trường nhập email trả lời (tùy thuộc vào giao diện)

		//			//show reply form
		//			pnlReplyForm.Visible = true;

					
		//			lblMsg.Visible = true;
		//			lblMsg.Text = "Bạn có thể viết phản hồi ngay bây giờ.";
		//			lblMsg.CssClass = "alert alert-info";
		//		}
		//		else
		//		{
					
		//			lblMsg.Visible = true;
		//			lblMsg.Text = "Lỗi: Không thể lấy thông tin chi tiết của liên hệ.";
		//			lblMsg.CssClass = "alert alert-danger";
		//		}
		//	}
		//}

		protected void SendReply(object sender, EventArgs e)
		{
			Console.WriteLine("!!! ALERT: Something happened !!!");
			// Lấy nút vừa nhấn
			Button btn = (Button)sender;

			// Lấy RepeaterItem cha
			RepeaterItem item = (RepeaterItem)btn.NamingContainer;

			// Lấy các control cần thiết
			TextBox txtReply = (TextBox)item.FindControl("txtReplyMessage");
			Panel pnlReplyForm = (Panel)item.FindControl("pnlReplyForm");
			Label lblMsg = (Label)item.FindControl("lblMsg");

			// Lấy nội dung
			string replyContent = txtReply.Text.Trim();

			if (!string.IsNullOrEmpty(replyContent))
			{
				// TODO: Lưu vào DB tại đây

				// Hiển thị thông báo
				lblMsg.Text = "Reply sent successfully!";
				lblMsg.Visible = true;

				// Ẩn lại form
				pnlReplyForm.Style["display"] = "none";

				// Gọi hàm JS để ẩn message sau 5s
				ScriptManager.RegisterStartupScript(this, GetType(), "hideMsg",
					$"hideMsgAfterSeconds('{lblMsg.ClientID}');", true);
			}
			else
			{
				lblMsg.Text = "Please enter a reply message.";
				lblMsg.Visible = true;
			}
		}


		//protected void SendReplyMessage(string message)
		//{
		//	string replyMessage = txtReplyMessage.Text;

		//	if (!string.IsNullOrEmpty(replyMessage))
		//	{
		//		// Logic for sending reply (e.g., send email or store the reply in DB)
		//		SendReplyMessage(replyMessage);

		//		// Hide the reply form after sending
		//		pnlReplyForm.Visible = false;

		//		// Display success message
		//		lblMsg.Visible = true;
		//		lblMsg.Text = "Phản hồi của bạn đã được gửi thành công.";
		//		lblMsg.CssClass = "alert alert-success";
		//	}
		//	else
		//	{
		//		// If reply message is empty
		//		lblMsg.Visible = true;
		//		lblMsg.Text = "Vui lòng viết một phản hồi trước khi gửi.";
		//		lblMsg.CssClass = "alert alert-warning";
		//	}
		//}
		
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
						Console.WriteLine("REFKDHJFJKSDHFLJSHDFSJKNKXMCN #)(@$");
						Panel pnlReplyForm = (Panel)e.Item.FindControl("pnlReplyForm");

						if (pnlReplyForm != null)
						{
							pnlReplyForm.Style["display"] = "block";
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
	}
}