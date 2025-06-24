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
            if (Session["admin"] == null)
            {
                Response.Redirect("~/User/Login.aspx");
            }
            else
            {

                if (!IsPostBack)
                {
                    LoadJobType();
                    LoadCountries();
                    fillData();
                }
            }
        }

        private void fillData()
        {
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int jobId))
            {
                try
                {
                    using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                    {
                        if (cm.State == ConnectionState.Closed)
                            cm.Open();

                        //string query = "select * from Jobs where JobID = '" + Request.QueryString["id"] + "' ";
                        //string query = "select * from Jobs where JobID = @JobID";
                        using (SqlCommand cmd = new SqlCommand("JobsSP", cm))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            
                            cmd.Parameters.AddWithValue("@Action", "SELECTBYID");
                            cmd.Parameters.AddWithValue("@JobID", jobId);

                            SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int)

                            {
                                Direction = ParameterDirection.Output
                            };

                            cmd.Parameters.Add(outputParam);

                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                if (sdr.HasRows)
                                {
                                    while (sdr.Read())
                                    {
                                        txtJobTitle.Text = sdr["Title"].ToString();
                                        txtNoOfPost.Text = sdr["NoOfPost"].ToString();
                                        txtDescription.Text = sdr["Description"].ToString();
                                        txtQualification.Text = sdr["Qualification"].ToString();
                                        txtExperience.Text = sdr["Experience"].ToString();
                                        txtSpecialization.Text = sdr["Specialization"].ToString();

                                        //txtLastDate.Text = Convert.ToDateTime(sdr["LastDateToApply"]).ToString("yyyy-MM-dd");
                                        if (sdr["LastDateToApply"] != DBNull.Value)
                                        {
                                            DateTime lastDate = Convert.ToDateTime(sdr["LastDateToApply"]);
                                            txtLastDate.Text = lastDate.ToString("yyyy-MM-dd"); // Định dạng ngày tháng theo chuẩn yyyy-MM-dd
                                        }
                                        else
                                        {
                                            txtLastDate.Text = string.Empty; // Hoặc giá trị mặc định khác nếu cần thiết
                                        }

                                        txtSalary.Text = sdr["Salary"].ToString();
                                        //ddlJobType.SelectedValue = sdr["JobType"].ToString();

                                        // Xử lý dropdown JobType
                                        string jobType = sdr["JobType"].ToString();
                                        if (ddlJobType.Items.FindByValue(jobType) != null)
                                        {
                                            ddlJobType.SelectedValue = jobType;
                                        }
                                        else
                                        {
                                            ddlJobType.SelectedIndex = 0; // Nếu không tìm thấy, chọn giá trị mặc định
                                            lblMsg.Visible = true;
                                            lblMsg.Text = "Job Type not found in the dropdown.";
                                            lblMsg.CssClass = "alert alert-warning";
                                        }

                                        txtCompany.Text = sdr["CompanyName"].ToString();
                                        txtWebsite.Text = sdr["Website"].ToString();
                                        txtEmail.Text = sdr["Email"].ToString();
                                        txtAddress.Text = sdr["Address"].ToString();

                                        // Xử lý dropdown Country
                                        string country = sdr["Country"].ToString();
                                        if (ddlCountry.Items.FindByValue(country) != null)
                                        {
                                            ddlCountry.SelectedValue = country;
                                        }
                                        else
                                        {
                                            ddlCountry.SelectedIndex = 0; // Nếu không tìm thấy, chọn giá trị mặc định
                                            lblMsg.Visible = true;
                                            lblMsg.Text = "Country not found in the dropdown.";
                                            lblMsg.CssClass = "alert alert-warning";
                                        }

                                        txtState.Text = sdr["State"].ToString();

                                        btnAdd.Text = "Update";
                                    }
                                }
                                else
                                {
                                    lblMsg.Visible = true;
                                    lblMsg.Text = "No job found with the provided ID.";
                                    lblMsg.CssClass = "alert alert-warning";
                                }
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
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
                string type= "";
                string imagePath = string.Empty;
                bool isValidToExecute = false;

                string concatQuery = string.Empty;

                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int jobId))
                {
                    type = "updated";

                    if (fuCompanyLogo.HasFile)
                    {
                        if (IsValidExtension(fuCompanyLogo.FileName))
                        {
                            concatQuery = "CompanyImage = @CompanyImage";
                        }
                        else
                        {
                            concatQuery = string.Empty; // Không có hình ảnh, không cần cập nhật trường này
                        }
                    }
                    else
                    {
                        concatQuery = string.Empty; // Không có hình ảnh, không cần cập nhật trường này
                    }
                    //update JobsSP


                    query = @"Update Jobs set Title=@Title, NoOfPost=@NoOfPost, Description=@Description, Qualification=@Qualification, Experience=@Experience, Specialization=@Specialization, LastDateToApply=@LastDateToApply,
                        Salary=@Salary, JobType=@JobType, CompanyName=@CompanyName, " + concatQuery + @"Website=@Website,
                        Email=@Email, Address=@Address, Country=@Country, State=@State where JobId=@id";
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

                                    cmd.Parameters.AddWithValue("@Action", "UPDATE");

                                    cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
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
                                        cmd.Parameters.AddWithValue("@LastDateToApply", DateTime.Now); // hoặc giá trị mặc định khác
                                    }

                                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                                    cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                                    cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);

                                    cmd.Parameters.AddWithValue("@id", jobId);

                                    if (fuCompanyLogo.HasFile)
                                    {
                                        if (IsValidExtension(fuCompanyLogo.FileName))
                                        {
                                            Guid obj = Guid.NewGuid();
                                            imagePath = "Images/" + obj.ToString() + fuCompanyLogo.FileName;

                                            fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + obj.ToString() + fuCompanyLogo.FileName);
                                            cmd.Parameters.AddWithValue("@CompanyImage", imagePath);
                                        }
                                        else
                                        {
                                            lblMsg.Visible = true;
                                            lblMsg.Text = "Please select a valid image file (jpg, jpeg, png).";
                                            lblMsg.CssClass = "alert alert-danger";
                                            return; // Dừng thực hiện nếu không có hình ảnh hợp lệ
                                        }
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@CompanyImage", DBNull.Value); // Không có hình ảnh, đặt là NULL
                                    }

                                    SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int)
                                    {
                                        Direction = ParameterDirection.Output
                                    };

                                    cmd.Parameters.Add(outputParam);
                                    cmd.ExecuteNonQuery();

                                    int result = (int)cmd.Parameters["@Result"].Value;

                                    if (result == 1)
                                    {
                                        Clear();
                                        lblMsg.Visible = true;
                                        lblMsg.Text = "Updated successfully!";
                                        lblMsg.CssClass = "alert alert-success";
                                    }
                                    else 
                                    {
                                        lblMsg.Visible = true;
                                        lblMsg.Text = "Failed to" + type + ". Please try again later.";
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
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = $"Error: {ex.Message}";
                        lblMsg.CssClass = "alert alert-danger";
                        Console.WriteLine($"Error: {ex.Message}"); // In ra lỗi để gỡ rối
                    }

                }
                else
                {
                    type = "saved";
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

                                    cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
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
                                        cmd.Parameters.AddWithValue("@LastDateToApply", DateTime.Now); // hoặc giá trị mặc định khác
                                    }

                                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                                    cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                                    cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);


                                    if (fuCompanyLogo.HasFile)
                                    {
                                        // Check if the file is valid
                                        if (IsValidExtension(fuCompanyLogo.FileName))
                                        {
                                            // Generate unique image name
                                            Guid obj = Guid.NewGuid();
                                            imagePath = "Images/" + obj.ToString() + fuCompanyLogo.FileName;

                                            // Save the file to the server
                                            fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + obj.ToString() + fuCompanyLogo.FileName);

                                            // Use the image path for the parameter
                                            cmd.Parameters.AddWithValue("@CompanyImage", imagePath);
                                            isValidToExecute = true;
                                        }
                                        else
                                        {
                                            lblMsg.Text = "Please select a valid image file (jpg, jpeg, png).";
                                            lblMsg.CssClass = "alert alert-danger";
                                        }
                                    }
                                    else
                                    {
                                        // No file uploaded, so set to null
                                        cmd.Parameters.AddWithValue("@CompanyImage", DBNull.Value);
                                        isValidToExecute = true; // Continue execution even without an image
                                    }


                                    if (!isValidToExecute)
                                    {
                                        lblMsg.Visible = true;
                                        lblMsg.Text = "Please fill all required fields correctly.";
                                        lblMsg.CssClass = "alert alert-danger";
                                    }
                                    else
                                    {
                                        SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int)
                                        {
                                            Direction = ParameterDirection.Output
                                        };

                                        cmd.Parameters.Add(outputParam);
                                        cmd.ExecuteNonQuery();

                                        int result = (int)cmd.Parameters["@Result"].Value;

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
                                            lblMsg.Text = "Job title" + $"<b>{txtJobTitle.Text.Trim()}</b>" + "already exists. Please choose a different job title.";
                                            lblMsg.CssClass = "alert alert-warning";
                                        }
                                        else
                                        {
                                            lblMsg.Visible = true;
                                            lblMsg.Text = "Failed to" + type + ". Please try again later.";
                                            lblMsg.CssClass = "alert alert-danger";
                                        }
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
                }

                
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = $"Error: {ex.Message}";
                lblMsg.CssClass = "alert alert-danger";
                Console.WriteLine($"Error: {ex.Message}"); // In ra lỗi để gỡ rối
            }
        }

        private void Clear()
        {
            txtJobTitle.Text = string.Empty;
            txtNoOfPost.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtQualification.Text = string.Empty;
            txtExperience.Text = string.Empty;
            txtSpecialization.Text = string.Empty;
            txtLastDate.Text = string.Empty;
            txtSalary.Text = string.Empty;
            ddlJobType.ClearSelection();
            txtCompany.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            ddlCountry.ClearSelection();
            txtState.Text = string.Empty;

        }

        private bool IsValidExtension(string fileName)
        {
            bool isValid = false;
            string[] fileExtension = { ".jpg", ".jpeg", ".png" };
            for (int i = 0; i < fileExtension.Length - 1; i++)
            {
                if (fileName.EndsWith(fileExtension[i], StringComparison.OrdinalIgnoreCase))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }

    }
}