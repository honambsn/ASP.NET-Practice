using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Job_Portal.Admin
{
    public partial class JobList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ShowJob();
        }

        private void ShowJob()
        {
            //string query = string.Empty;

            //using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
            //{
            //    cm.Open();

            //    query = @"select row_number() over(order by (select 1)) as [Sr.No], JobID, Title, NoOfPost, Qualification, Experience,
            //            LastDateToApply, CompanyName, Country, State, CreateDate from Jobs";

            //    SqlCommand cmd = new SqlCommand(query, cm);
            //    SqlDataAdapter sda = new SqlDataAdapter(cmd);

            //    DataTable dt = new DataTable();

            //    sda.Fill(dt);
            //    GridView1.DataSource = dt;
            //    GridView1.DataBind();


            try
            {
                using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                {
                    if (cm.State == ConnectionState.Closed)
                        cm.Open();

                    using (SqlCommand cmd = new SqlCommand("JobsSP", cm))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECTAll");

                        SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(outputParam);

                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        GridView1.DataSource = dt;
                        GridView1.DataBind();

                        //cmd.ExecuteNonQuery();

                        int result = (int)cmd.Parameters["@Result"].Value;

                        if (result == 1)
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Jobs retrieved successfully!";
                            lblMsg.CssClass = "alert alert-success";
                        }
                        else
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Error occurred while retrieving jobs.";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or show an error message
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("~/User/Login.aspx");
            }

            if (!IsPostBack)
            {
                ShowJob();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowJob();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int jobId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

                using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
                {
                    if (cm.State == ConnectionState.Closed)
                        cm.Open();

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("JobsSP", cm))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Action", "Delete");
                            cmd.Parameters.AddWithValue("@JobID", jobId);


                            SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };

                            cmd.Parameters.Add(outputParam);
                            cmd.ExecuteNonQuery();


                            int result = (int)cmd.Parameters["@Result"].Value;

                            if (result == 1)
                            {

                                GridView1.EditIndex = -1;
                                ShowJob();

                                //Clear();
                                lblMsg.Visible = true;
                                lblMsg.Text = "Registered successfully!";
                                lblMsg.CssClass = "alert alert-success";
                            }
                            else
                            {
                                lblMsg.Visible = true;
                                lblMsg.Text = "Error occurred while deleting the job.";
                                lblMsg.CssClass = "alert alert-danger";

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or show an error message
                        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                // Log the exception or show an error message
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}