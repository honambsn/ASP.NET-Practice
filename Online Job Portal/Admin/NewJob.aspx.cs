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
    public partial class NewJob : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadJobType();
                LoadCountries();
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

        }
    }
}