using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Job_Portal.Admin
{
    public partial class NewJob : System.Web.UI.Page
    {
        string query = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    LoadJobType();
            //    LoadCountries();
            //}
            if (Session["admin"] == null)
            {
                Response.Redirect("~/User/Login.aspx");
            }
        }

        private void LoadJobType()
        {
            try
            {
                using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                {
                    cm.Open();

                    using (SqlCommand cmd = new SqlCommand("JobTypesSP", cm))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "SELECT";

                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@JobType", SqlDbType.VarChar).Value = DBNull.Value;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = DBNull.Value;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1; // Mặc định active

                        SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Int);
                        resultParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(resultParam);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlJobType.Items.Clear();
                            ddlJobType.Items.Add(new ListItem("Select Job Type", "-1"));

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string jobType = reader["JobType"].ToString();
                                    ddlJobType.Items.Add(new ListItem(jobType, jobType));
                                }
                            }
                            else
                            {
                                Response.Write("<script>alert('Không tìm thấy loại công việc nào');</script>");
                            }

                            ddlJobType.SelectedValue = "-1";
                        }

                        int resultCode = (int)cmd.Parameters["@Result"].Value;
                        if (resultCode != 1)
                        {
                            Response.Write("<script>alert('Lỗi: Không tải được loại công việc');</script>");
                        }

                        ddlJobType.Attributes.CssStyle.Add("cursor", "pointer");

                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Lỗi khi tải loại công việc: " + ex.Message + "');</script>");
            }
        }


        private void LoadCountries()
        {
            try
            {
                using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                {
                    if (cm.State == ConnectionState.Closed)
                        cm.Open();

                    using (SqlCommand cmd = new SqlCommand("CountrySP", cm))
                    {
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlCountry.Items.Clear();
                            ddlCountry.Items.Add(new ListItem("Select country", "0"));

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string country = reader["CountryName"].ToString();
                                    ddlCountry.Items.Add(new ListItem(country, country));
                                }
                            }
                            else
                            {
                                Response.Write("<script>alert('No countries found');</script>"); // Alert if no countries found
                            }
                            ddlCountry.SelectedValue = "0"; // Mặc định chọn item đầu tiên
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string concatQuery, imagePath = string.Empty;
                bool isValidToExecute = true;
                if (fuCompanyLogo.HasFile)
                {
                    if (IsValidExtension(fuCompanyLogo.FileName, new[] { ".jpg", ".jpeg", ".png", ".gif" }))
                    {
                        //imagePath = "~/Images/CompanyLogos/" + fuCompanyLogo.FileName;
                        //fuCompanyLogo.SaveAs(Server.MapPath(imagePath));
                        concatQuery = "";
                    }
                    else
                    {
                        isValidToExecute = false;
                        Response.Write("<script>alert('Invalid file type. Please upload an image file.');</script>");
                    }
                }

                try
                {
                    using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                    {
                        if (cm.State == ConnectionState.Closed)
                        {
                            cm.Open();
                        }

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("JobsSP", cm))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@Action", "INSERT");
                                
                                cmd.Parameters.AddWithValue("@JobTitle", txtJobTitle.Text.Trim());
                                cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPost.Text.Trim());
                                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                                cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                                cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                                cmd.Parameters.AddWithValue("@Specialization", txtSpecialization.Text.Trim());

                                DateTime lastDate;
                                if (DateTime.TryParse(txtLastDate.Text.Trim(), out lastDate))
                                {
                                    cmd.Parameters.AddWithValue("@LastDateToApply", lastDate);
                                }
                                else
                                {
                                    // Xử lý trường hợp ngày không hợp lệ
                                    cmd.Parameters.AddWithValue("@LastDateToApply", DBNull.Value); // hoặc giá trị mặc định khác
                                }

                                cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                                cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                                cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                                cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                                cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                                cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);


                                SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int)
                                {
                                    Direction = ParameterDirection.Output
                                };

                                cmd.Parameters.Add(outputParam);
                                cmd.ExecuteNonQuery();

                                int result = (int)cmd.Parameters["@Result"].Value;

                                //int r = cmd.ExecuteNonQuery();

                                //if (r > 0)
                                //{
                                //    Clear();
                                //    lblMsg.Visible = true;
                                //    lblMsg.Text = "Registered successfully!";
                                //    lblMsg.CssClass = "alert alert-success";
                                //}
                                //else
                                //{
                                //    lblMsg.Visible = true;
                                //    lblMsg.Text = "Failed to send message. Please try again later.";
                                //    lblMsg.CssClass = "alert alert-danger";
                                //}

                                if (result == 1)
                                {
                                    Clear();
                                    lblMsg.Visible = true;
                                    lblMsg.Text = "Registered successfully!";
                                    lblMsg.CssClass = "alert alert-success";
                                }
                                else if (result == -1)
                                {
                                    lblMsg.Visible = true;
                                    lblMsg.Text = $"<b>{txtUserName.Text.Trim()}</b> already exists. Please choose a different username.";
                                    lblMsg.CssClass = "alert alert-warning";
                                }
                                else
                                {
                                    lblMsg.Visible = true;
                                    lblMsg.Text = "Failed to register. Please try again later.";
                                    lblMsg.CssClass = "alert alert-danger";
                                }
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            // In ra lỗi SQL chi tiết để gỡ rối
                            lblMsg.Visible = true;
                            lblMsg.Text = $"SQL Error: {sqlEx.Message}";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                        catch (Exception ex)
                        {
                            // In ra lỗi chung
                            lblMsg.Visible = true;
                            lblMsg.Text = $"Error: {ex.Message}";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // In ra lỗi chung
                    lblMsg.Visible = true;
                    lblMsg.Text = $"Error: {ex.Message}";
                    lblMsg.CssClass = "alert alert-danger";
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    // Đảm bảo kết nối được đóng đúng cách
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        Console.WriteLine($"Inserting user: {txtUserName.Text.Trim()}");
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private bool IsValidExtension(string fileName, string[] allowedExtensions)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

    }
}