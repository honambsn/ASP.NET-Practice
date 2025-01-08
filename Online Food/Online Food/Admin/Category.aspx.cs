using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Food.Admin
{
	public partial class Category : System.Web.UI.Page
	{
		SqlConnection cn;
		SqlCommand cmd;
		SqlDataReader dr;
		SqlDataAdapter sda;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//getCategories();
				Session["breabCrumb"] = "Category";
				getCategories();
			}
			lblMsg.Visible = false;
		}

		//      protected void btnAddOrUpdate_Click(object sender, EventArgs e)
		//      {
		//	string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
		//	bool isValidToExcute = false;
		//	int CategoryID = Convert.ToInt32(hdnId.Value);
		//	cn = new SqlConnection(Connection.GetConnectionString());

		//	if (cn.State == ConnectionState.Closed)
		//	{
		//		cn.Open();
		//	}

		//	try
		//	{
		//		using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
		//		{
		//			using (SqlCommand cmd = new SqlCommand("Category_Crud", cm))
		//			{
		//				cmd.Parameters.AddWithValue("@Action", CategoryID == 0 ? "INSERT" : "UPDATE");
		//				cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
		//				cmd.Parameters.AddWithValue("Name", txtName.Text.Trim());
		//				cmd.Parameters.AddWithValue("IsActive", cbIsActive.Checked);

		//				if (fuCategoryImage.HasFile)
		//				{
		//					if (Utils.IsValidExtension(fuCategoryImage.FileName))
		//					{
		//						Guid obj = Guid.NewGuid();
		//						fileExtension = Path.GetExtension(fuCategoryImage.FileName);
		//						imagePath = "Images/Category/" + obj.ToString() + fileExtension;
		//						fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + obj.ToString() + fileExtension);
		//						cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
		//						isValidToExcute = true;
		//					}
		//					else
		//					{
		//						lblMsg.Visible = true;
		//						lblMsg.Text = "Please select .jpg, jpeg or .png image";
		//						lblMsg.CssClass = "alert alert-danger";
		//						isValidToExcute = false;
		//					}
		//				}
		//				else
		//				{
		//					isValidToExcute = true;
		//				}

		//				if (isValidToExcute)
		//				{
		//					cmd.CommandType = CommandType.StoredProcedure;
		//					try
		//					{
		//						cmd.ExecuteNonQuery();
		//						actionName = CategoryID == 0 ? "inserted" : "updated";
		//						lblMsg.Visible = true;
		//						//	lblMsg.Text = "Category " + (CategoryID == 0 ? "added" : "updated") + " successfully";
		//						lblMsg.Text = "Category " + actionName + " successfully";
		//						lblMsg.CssClass = "alert alert-success";
		//						//getCategories();
		//						clear();
		//						//cbIsActive.Checked = false;
		//						//hdnId.Value = "0";
		//					}
		//					catch (Exception ex)
		//					{
		//						lblMsg.Visible = true;
		//						lblMsg.Text = "Error: " + ex.Message;
		//						lblMsg.CssClass = "alert alert-danger";
		//					}
		//					finally
		//					{
		//						cn.Close();
		//					}
		//				}
		//				//cmd.ExecuteNonQuery();
		//			}
		//			//cmd.ExecuteNonQuery();
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		lblMsg.Visible = true;
		//		lblMsg.Text = "Error: " + ex.Message;
		//		lblMsg.CssClass = "alert alert-danger";
		//	}
		//	finally
		//	{
		//		cn.Close();

		//	}
		//}

		protected void btnAddOrUpdate_Click(object sender, EventArgs e)
		{
			string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
			bool isValidToExecute = false;
			int CategoryID = Convert.ToInt32(hdnId.Value);

			// Using only a single connection (no need for 'cn' outside of using block)
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				try
				{
					using (SqlCommand cmd = new SqlCommand("Category_Crud", cm))
					{
						// Set command parameters
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Action", CategoryID == 0 ? "INSERT" : "UPDATE");
						cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
						cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
						cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

						// Handle file upload
						if (fuCategoryImage.HasFile)
						{
							if (Utils.IsValidExtension(fuCategoryImage.FileName))
							{
								Guid obj = Guid.NewGuid();
								fileExtension = Path.GetExtension(fuCategoryImage.FileName);
								imagePath = "Images/Category/" + obj.ToString() + fileExtension;
								fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + obj.ToString() + fileExtension);
								Console.WriteLine("Image path: " + imagePath);
								cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
								Console.WriteLine("Image path added to command parameters");
								isValidToExecute = true;
							}
							else
							{
								lblMsg.Visible = true;
								lblMsg.Text = "Please select .jpg, .jpeg, or .png image.";
								lblMsg.CssClass = "alert alert-danger";
								isValidToExecute = false;
							}
						}
						else
						{
							isValidToExecute = true;
						}

						if (isValidToExecute)
						{
							try
							{
								// Execute the stored procedure
								cmd.ExecuteNonQuery();
								actionName = CategoryID == 0 ? "inserted" : "updated";
								lblMsg.Visible = true;
								lblMsg.Text = "Category " + actionName + " successfully.";
								lblMsg.CssClass = "alert alert-success";

								// Clear form and reset state
								getCategories();
								clear();
								//cbIsActive.Checked = false; // If necessary
								//hdnId.Value = "0"; // If necessary
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
				catch (Exception ex)
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Error: " + ex.Message;
					lblMsg.CssClass = "alert alert-danger";
				}
			} // Connection 'cm' automatically closes here
		}

		private void getCategories()
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Category_Crud", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "SELECT");
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						// Bind the result to the rptCategory control
						rptCategory.DataSource = dt;
						rptCategory.DataBind();
					}
				}
			}

		}

		private void clear()
		{
			txtName.Text = string.Empty;
			cbIsActive.Checked = false;
			hdnId.Value = "0";
			btnAddOrUpdate.Text = "Add";
			imgCategory.ImageUrl = String.Empty;
		}

		protected void btnClear_Click(object sender, EventArgs e)
		{
			clear();
		}

		protected void rptCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			#region Old Code

			//lblMsg.Visible = false;
			//if (e.CommandName == "edit")
			//{
			//	using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			//	{
			//		if (cm.State == ConnectionState.Closed)
			//		{
			//			cm.Open();
			//		}

			//		try
			//		{
			//			using (SqlCommand cmd = new SqlCommand("Category_Crud", cm))
			//			{
			//				cmd.Parameters.AddWithValue("@Action", "GETBYID");
			//				cmd.Parameters.AddWithValue("@CategoryID", e.CommandArgument);
			//				cmd.CommandType = CommandType.StoredProcedure;


			//				using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
			//				{
			//					DataTable dt = new DataTable();
			//					sda.Fill(dt);

			//					if (dt.Rows.Count > 0)
			//					{
			//						txtName.Text = dt.Rows[0]["Name"].ToString();
			//						cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
			//						imgCategory.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["IamgeUrl"].ToString()) ?
			//							"../Images/Category/No_image.png" : "../" + dt.Rows[0]["IamgeUrl"].ToString();
			//						imgCategory.Height = 200;
			//						imgCategory.Width = 200;
			//						hdnId.Value = dt.Rows[0]["CategoryID"].ToString();
			//						btnAddOrUpdate.Text = "Update";
			//						LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
			//						btn.CssClass = "badge badge-warning";

			//					}
			//				}
			//			}
			//		}
			//		catch (Exception ex)
			//		{
			//			lblMsg.Visible = true;
			//			lblMsg.Text = "Error: " + ex.Message;
			//			lblMsg.CssClass = "alert alert-danger";
			//		}

			//	}
			//}

			//else if (e.CommandName == "delete")
			//{
			//	using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			//	{
			//		if (cm.State == ConnectionState.Closed)
			//		{
			//			cm.Open();
			//		}

			//		try
			//		{
			//			using (SqlCommand cmd = new SqlCommand("Category_Crud", cm))
			//			{
			//				cmd.Parameters.AddWithValue("@Action", "DELETE");
			//				cmd.Parameters.AddWithValue("@CategoryID", e.CommandArgument);
			//				cmd.CommandType = CommandType.StoredProcedure;
			//				try
			//				{
			//					cmd.ExecuteNonQuery();
			//					lblMsg.Visible = true;
			//					lblMsg.Text = "Category deleted successfully";
			//					lblMsg.CssClass = "alert alert-success";
			//					getCategories();
			//				}
			//				catch (Exception ex)
			//				{
			//					lblMsg.Visible = true;
			//					lblMsg.Text = "Error: " + ex.Message;
			//					lblMsg.CssClass = "alert alert-danger";
			//				}
			//			}
			//		}
			//		catch (Exception ex)
			//		{
			//			lblMsg.Visible = true;
			//			lblMsg.Text = "Error: " + ex.Message;
			//			lblMsg.CssClass = "alert alert-danger";
			//		}
			//	}
			//}


			#endregion

			lblMsg.Visible = false;

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
						EditCategory(cm, e);
					}
					else if (e.CommandName == "delete")
					{
						DeleteCategory(cm, e);
					}
				}
				catch (Exception ex)
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Error: " + ex.Message;
					lblMsg.CssClass = "alert alert-danger";
				}
			}

		}

		private void EditCategory(SqlConnection cm, RepeaterCommandEventArgs e)
		{
			using (SqlCommand cmd = new SqlCommand("Category_Crud", cm))
			{
				cmd.Parameters.AddWithValue("@Action", "GETBYID");
				cmd.Parameters.AddWithValue("@CategoryID", e.CommandArgument);
				cmd.CommandType = CommandType.StoredProcedure;

				using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
				{
					DataTable dt = new DataTable();
					sda.Fill(dt);

					if (dt.Rows.Count > 0)
					{
						txtName.Text = dt.Rows[0]["CategoryName"].ToString();
						cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
						imgCategory.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ?
							"../Images/Category/No_image.webp" : "../" + dt.Rows[0]["ImageUrl"].ToString();
						imgCategory.Height = 200;
						imgCategory.Width = 200;
						hdnId.Value = dt.Rows[0]["CategoryID"].ToString();
						btnAddOrUpdate.Text = "Update";

						// Change button class to show it's in 'edit' mode
						LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
						if (btn != null) btn.CssClass = "badge badge-warning";
					}
				}
			}
		}

		private void DeleteCategory(SqlConnection cm, RepeaterCommandEventArgs e)
		{
			using (SqlCommand cmd = new SqlCommand("Category_Crud", cm))
			{
				cmd.Parameters.AddWithValue("@Action", "DELETE");
				cmd.Parameters.AddWithValue("@CategoryID", e.CommandArgument);
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.ExecuteNonQuery();

				lblMsg.Visible = true;
				lblMsg.Text = "Category deleted successfully";
				lblMsg.CssClass = "alert alert-success";

				// Refresh category list after deletion
				getCategories();
			}
		}

		protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Label lbl = e.Item.FindControl("lblIsActive") as Label;
				if (lbl.Text == "True")
				{
					lbl.Text = "Active";
					lbl.CssClass = "badge badge-success";
				}
				else {
					lbl.Text = "In-Active";
					lbl.CssClass = "badge badge-danger";
				}
			}
		}
	}
}