using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Job_Portal.User
{
    public partial class Contact : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
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
                        using (SqlCommand cmd = new SqlCommand("ContactSP", cm))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Action", "INSERT");
                            cmd.Parameters.AddWithValue("@Name", name.Value.Trim());
                            cmd.Parameters.AddWithValue("@Email", email.Value.Trim());
                            cmd.Parameters.AddWithValue("@Subject", subject.Value.Trim());
                            cmd.Parameters.AddWithValue("@Message", message.Value.Trim());


                            int r = cmd.ExecuteNonQuery();

                            if (r > 0)
                            {
                                Clear();
                                lblMsg.Visible = true;
                                //lblMsg.Text = "Message sent successfully!";
                                lblMsg.Text = "Thanks for contacting us. We will get back to you soon!";
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
                        // Handle SQL exception
                        Response.Write("<script>alert('SQL Error: " + sqlEx.Message + "');</script>");
                    }
                    catch (Exception ex)
                    {
                        // Handle general exception
                        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                    }

                }



                //con = new SqlConnection(Connection.GetConnectionString());

                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}

                //string query = @"Insert into Contact values (@Name, @Email, @Subject, @Message)";

                //cmd = new SqlCommand(query, con);
                //cmd.Parameters.AddWithValue("@Name", name.Value.Trim());
                //cmd.Parameters.AddWithValue("@Email", email.Value.Trim());
                //cmd.Parameters.AddWithValue("@Subject", subject.Value.Trim());
                //cmd.Parameters.AddWithValue("@Message", message.Value.Trim());

                //int r = cmd.ExecuteNonQuery();

                //if (r > 0)
                //{
                //    lblMsg.Visible = true;
                //    //lblMsg.Text = "Message sent successfully!";
                //    lblMsg.Text = "Thanks for contacting us. We will get back to you soon!";
                //    lblMsg.CssClass = "alert alert-success";
                //    Clear();
                //}
                //else
                //{
                //    lblMsg.Visible = true;
                //    lblMsg.Text = "Failed to send message. Please try again later.";
                //    lblMsg.CssClass = "alert alert-danger";
                //}

            }
            catch (Exception ex)
            {
                // Handle exception
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void Clear()
        {
            name.Value = string.Empty;
            email.Value = string.Empty;
            subject.Value = string.Empty;
            message.Value = string.Empty;
            lblMsg.Visible = false;
        }
    }
}