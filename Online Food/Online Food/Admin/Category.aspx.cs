using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Food.Admin
{
	public partial class Category : System.Web.UI.Page
	{
		SqlConnection cn;
		SqlCommand cmd;
		SqlDataReader dr;
		DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		//      protected void btnAddOrUpdate_Click(object sender, EventArgs e)
		//      {
		//	string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
		//	bool isValidToExcute = false;
		//	int categoryId = Convert.ToInt32(hdnId.Value);
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
		//				cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
		//				cmd.Parameters.AddWithValue("@CategoryId", categoryId);
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
		//						actionName = categoryId == 0 ? "inserted" : "updated";
		//						lblMsg.Visible = true;
		//						//	lblMsg.Text = "Category " + (categoryId == 0 ? "added" : "updated") + " successfully";
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
			int categoryId = Convert.ToInt32(hdnId.Value);

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
						cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
						cmd.Parameters.AddWithValue("@CategoryId", categoryId);
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
								cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
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
								actionName = categoryId == 0 ? "inserted" : "updated";
								lblMsg.Visible = true;
								lblMsg.Text = "Category " + actionName + " successfully.";
								lblMsg.CssClass = "alert alert-success";

								// Clear form and reset state
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


		private void clear()
		{
			txtName.Text = string.Empty;
			cbIsActive.Checked = false;
			hdnId.Value = "0";
			btnAddOrUpdate.Text = "Add";
		}
	}
}