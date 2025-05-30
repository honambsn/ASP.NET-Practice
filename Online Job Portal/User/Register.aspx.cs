using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services.Description;
using System.Xml.Linq;
namespace Online_Job_Portal.User
{
    public partial class Register : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCountries();
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



        protected void btnRegister_Click(object sender, EventArgs e)
        {
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
                        using (SqlCommand cmd = new SqlCommand("UsersSP", cm))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Action", "INSERT");
                            cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                            cmd.Parameters.AddWithValue("@Name", txtFullName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);

                            // Ghi lại tham số và kiểm tra
                            Console.WriteLine($"Inserting user: {txtUserName.Text.Trim()}");

                            int r = cmd.ExecuteNonQuery();

                            if (r > 0)
                            {
                                Clear();
                                lblMsg.Visible = true;
                                lblMsg.Text = "Registered successfully!";
                                lblMsg.CssClass = "alert alert-success";
                            }
                            else
                            {
                                lblMsg.Visible = true;
                                lblMsg.Text = "Failed to send message. Please try again later.";
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
                        // Ghi lại lỗi chi tiết trong log file hoặc console
                        Console.WriteLine($"SQL Error: {sqlEx.Message}");
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

            //try
            //{
            //    using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
            //    {
            //        if (cm.State == ConnectionState.Closed)
            //        {
            //            cm.Open();
            //        }

            //        try
            //        {
            //            using (SqlCommand cmd = new SqlCommand("UsersSP", cm))
            //            {
            //                cmd.CommandType = CommandType.StoredProcedure;

            //                cmd.Parameters.AddWithValue("@Action", "INSERT");
            //                cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
            //                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
            //                cmd.Parameters.AddWithValue("@Name", txtFullName.Text.Trim());
            //                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
            //                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
            //                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            //                cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);

            //                Console.WriteLine($"Inserting user: {txtUserName.Text.Trim()}");


            //                int r = cmd.ExecuteNonQuery();

            //                if (r > 0)
            //                {   
            //                    lblMsg.Visible = true;
            //                    //lblMsg.Text = "Message sent successfully!";
            //                    lblMsg.Text = "Registered Successfull!";
            //                    lblMsg.CssClass = "alert alert-success";
            //                    Clear();
            //                }
            //                else
            //                {
            //                    lblMsg.Visible = true;
            //                    lblMsg.Text = "Failed to send message. Please try again later.";
            //                    lblMsg.CssClass = "alert alert-danger";
            //                }
            //            }
            //        }
            //        catch (SqlException sqlEx)
            //        {
            //            // Handle SQL exception
            //            //Response.Write("<script>alert('SQL Error: " + sqlEx.Message + "');</script>");
            //            if (sqlEx.Message.Contains("Violation of UNIQUE KEY constraint"))
            //            {
            //                // Handle unique constraint violation
            //                lblMsg.Visible = true;
            //                lblMsg.Text = "<b>" + txtUserName.Text.Trim() + "</b>" + " already exists. Please choose a different username.";
            //                lblMsg.CssClass = "alert alert-danger";
            //            }
            //            else
            //            {
            //                // Handle other SQL exceptions
            //                Response.Write("<script>alert('SQL Error: " + sqlEx.Message + "');</script>");
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            // Handle general exception
            //            Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            //        }

            //    }
            //}
            //catch (SqlException sqlEx)
            //{
            //    // Handle SQL exception
            //    Response.Write("<script>alert('SQL Error: " + sqlEx.Message + "');</script>");
            //}
            //catch (Exception ex)
            //{
            //    // Handle general exception
            //    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            //}
            //finally
            //{
            //    if (con != null && con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}
        }

        private void Clear()
        {
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtFullName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlCountry.ClearSelection();
        }
    }
}