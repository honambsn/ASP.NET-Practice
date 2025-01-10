using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Online_Food.Admin
{
	public partial class Product : System.Web.UI.Page
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
				//getProducts();
				Session["breabCrumb"] = "Product";
				getProducts();
			}
			lblMsg.Visible = false;
		}

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
			string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
			bool isValidToExecute = false;
			int ProductID = Convert.ToInt32(hdnId.Value);

			// Using only a single connection (no need for 'cn' outside of using block)
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				try
				{
					using (SqlCommand cmd = new SqlCommand("Product_Crud", cm))
					{
						// Set command parameters
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Action", ProductID == 0 ? "INSERT" : "UPDATE");
						cmd.Parameters.AddWithValue("@ProductID", ProductID);
						cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
						cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
						cmd.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
						cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text.Trim());
						cmd.Parameters.AddWithValue("@ProductID", ddlCategories.SelectedValue);
						cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

						// Handle file upload
						if (fuProductImage.HasFile)
						{
							if (Utils.IsValidExtension(fuProductImage.FileName))
							{
								Guid obj = Guid.NewGuid();
								fileExtension = Path.GetExtension(fuProductImage.FileName);
								imagePath = "Images/Product/" + obj.ToString() + fileExtension;
								fuProductImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + obj.ToString() + fileExtension);
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
								actionName = ProductID == 0 ? "inserted" : "updated";
								lblMsg.Visible = true;
								lblMsg.Text = "Product " + actionName + " successfully.";
								lblMsg.CssClass = "alert alert-success";

								// Clear form and reset state
								getProducts();
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

		private void getProducts()
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}

				using (SqlCommand cmd = new SqlCommand("Product_Crud", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "SELECT");
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						// Bind the result to the rProduct control
						rProduct.DataSource = dt;
						rProduct.DataBind();
					}
				}
			}

		}

		private void clear()
		{
			txtName.Text = string.Empty;
			txtDescription.Text = string.Empty;
			txtQuantity.Text = string.Empty;
			txtPrice.Text = string.Empty;
			ddlCategories.ClearSelection();
			cbIsActive.Checked = false;
			hdnId.Value = "0";
			btnAddOrUpdate.Text = "Add";
			imgProduct.ImageUrl = String.Empty;
		}

		protected void btnClear_Click(object sender, EventArgs e)
		{
			clear();
		}

		protected void rProduct_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
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
						EditProduct(cm, e);
					}
					else if (e.CommandName == "delete")
					{
						DeleteProduct(cm, e);
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

		protected void rProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Label lblIsActive = e.Item.FindControl("lblIsActive") as Label;
				Label lblQuantity = e.Item.FindControl("lblQuantity") as Label;
				if (lblIsActive.Text == "True")
				{
					lblIsActive.Text = "Active";
					lblIsActive.CssClass = "badge badge-success";
				}
				else
				{
					lblIsActive.Text = "In-Active";
					lblIsActive.CssClass = "badge badge-danger";
				}

				if (Convert.ToInt32(lblQuantity.Text) <= 5)
				{
					lblQuantity.Text = "Out of Stock";
					lblQuantity.CssClass = "badge badge-danger";
				}
				else
				{
					lblQuantity.Text = "In Stock";
					lblQuantity.CssClass = "badge badge-success";
				}
			}
		}

		private void EditProduct(SqlConnection cm, RepeaterCommandEventArgs e)
		{
			using (SqlCommand cmd = new SqlCommand("Product_Crud", cm))
			{
				// Clear any existing parameters to avoid duplicates
				cmd.Parameters.Clear();

				// Add parameters only once
				cmd.Parameters.AddWithValue("@Action", "GETBYID");
				cmd.Parameters.AddWithValue("@ProductID", e.CommandArgument);
				cmd.CommandType = CommandType.StoredProcedure;

				try
				{
					// Execute the query using SqlDataAdapter
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						if (dt.Rows.Count > 0)
						{
							// Set the values from DataTable to your controls
							txtName.Text = dt.Rows[0]["ProductName"].ToString();
							txtDescription.Text = dt.Rows[0]["Description"].ToString();
							txtPrice.Text = dt.Rows[0]["Price"].ToString();
							txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
							ddlCategories.SelectedValue = dt.Rows[0]["CategoryID"].ToString();

							cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
							imgProduct.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ?
								"../Images/Product/No_image.webp" : "../" + dt.Rows[0]["ImageUrl"].ToString();
							imgProduct.Height = 200;
							imgProduct.Width = 200;
							hdnId.Value = dt.Rows[0]["ProductID"].ToString();
							btnAddOrUpdate.Text = "Update";

							// Change button class to show it's in 'edit' mode
							LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
							if (btn != null)
							{
								btn.CssClass = "badge badge-warning";
							}
						}
						else
						{
							// Handle case when no rows are returned
							// Possibly show a message or handle accordingly
						}
					}
				}
				catch (SqlException ex)
				{
					// Log exception if needed
					// Handle SQL errors (e.g., connection issues, SQL syntax)
					Console.WriteLine("SQL Error: " + ex.Message);
				}
				catch (Exception ex)
				{
					// Handle any general errors
					Console.WriteLine("Error: " + ex.Message);
				}
			}
		}


		private void DeleteProduct(SqlConnection cm, RepeaterCommandEventArgs e)
		{
			using (SqlCommand cmd = new SqlCommand("Product_Crud", cm))
			{
				cmd.Parameters.Clear();  // Clear existing parameters

				cmd.Parameters.AddWithValue("@Action", "DELETE");
				cmd.Parameters.AddWithValue("@ProductID", e.CommandArgument);
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.ExecuteNonQuery();

				lblMsg.Visible = true;
				lblMsg.Text = "Product deleted successfully";
				lblMsg.CssClass = "alert alert-success";

				// Refresh Product list after deletion
				getProducts();
			}
		}
	}
}