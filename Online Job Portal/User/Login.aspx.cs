using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Job_Portal.User
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;

        string username, password = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadLoginTypes()
        {
            try
            {
                using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                {
                    if (cm.State == ConnectionState.Closed)
                        cm.Open();

                    using (SqlCommand cmd = new SqlCommand("LoginTypeSP", cm))
                    {
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlLoginType.Items.Clear();
                            ddlLoginType.Items.Add(new ListItem("Select Login Type", "0"));

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string loginType = reader["TypeName"].ToString();
                                    ddlLoginType.Items.Add(new ListItem(loginType, loginType));

                                }
                            }
                            else
                            {
                                Response.Write("<script>alert('No Login Types Found');</script>"); // Alert if no login types found
                            }
                            ddlLoginType.SelectedValue = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlLoginType.SelectedValue == "Admin")
                {
                    username = ConfigurationManager.AppSettings["username"];
                    password = ConfigurationManager.AppSettings["password"];

                    if (username == txtUserName.Text.Trim() && password == txtPassword.Text.Trim())
                    {
                        //Session["UserName"] = txtUserName.Text.Trim();
                        Session["admin"] = username;
                        Response.Redirect("~/Admin/Dashboard.aspx", false);
                    }
                    else
                    {
                        //Response.Write("<script>alert('Invalid Username or Password');</script>");
                        showErrorMsg("Admin");
                    }

                }
                else
                {
                    try
                    {
                        using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                        {
                            if (cm.State == ConnectionState.Closed)
                                cm.Open();
                            #region LoadLoginTypes
                            try
                            {
                                // Open the connection if it is closed
                                if (cm.State == ConnectionState.Closed)
                                    cm.Open();

                                // Define the stored procedure and create the command
                                string storedProc = "UsersSP"; // The name of your stored procedure
                                using (SqlCommand cmd = new SqlCommand(storedProc, cm))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    // Add input parameters
                                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = txtUserName.Text.Trim();
                                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = txtPassword.Text.Trim();  // Only for Login and Update actions
                                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Login";  // Action is passed from client

                                    // Add output parameters
                                    cmd.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Direction = ParameterDirection.Output;

                                    // Execute the stored procedure
                                    cmd.ExecuteNonQuery();

                                    // Get the output parameters
                                    int result = (int)cmd.Parameters["@Result"].Value;
                                    int userID = (int)cmd.Parameters["@UserID"].Value;

                                    if (result == 1)
                                    {
                                        // Successful login
                                        Session["user"] = txtUserName.Text.Trim();
                                        Session["userID"] = userID.ToString();

                                        // Redirect to Default.aspx after successful login
                                        Response.Redirect("Default.aspx", false);
                                    }
                                    else
                                    {
                                        // Handle different result codes for specific actions
                                        string errorMsg = string.Empty;
                                        if (result == -2)
                                        {
                                            errorMsg = "Invalid username or password.";
                                        }
                                        else if (result == -1)
                                        {
                                            errorMsg = "Username already exists.";
                                        }
                                        else if (result == -3)
                                        {
                                            errorMsg = "Username not found.";
                                        }
                                        else if (result == -4)
                                        {
                                            errorMsg = "Invalid action.";
                                        }

                                        // Show the error message
                                        showErrorMsg(errorMsg);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions
                                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                            }

                            #endregion 
                            //string query = @"SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password AND UserType = @UserType";
                            //string query = @"SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                            //cmd = new SqlCommand(query, cm);
                            //cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                            //cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());


                            //using (SqlDataReader sdr = cmd.ExecuteReader())
                            //{
                            //    if (sdr.Read())
                            //    {
                            //        Session["user"] = sdr["Username"].ToString();
                            //        Session["UserID"] = sdr["UserID"].ToString();

                            //        Response.Redirect("Default.aspx", false);
                            //    }
                            //    else
                            //    {
                            //        showErrorMsg("User");
                            //    }
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        private void showErrorMsg(string userType)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "<b>" + userType + "</b> credentials are incorrect...!";
            lblMsg.CssClass = "alert alert-danger";
        }
    }
}