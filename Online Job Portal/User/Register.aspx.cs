using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                    if (cm.State == System.Data.ConnectionState.Closed)
                        cm.Open();

                    using (SqlCommand cmd = new SqlCommand("CountrySP", cm))
                    {
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

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
                Console.WriteLine("Error loading countries: " + ex.Message);
            }
        }



        protected void btnRegister_Click(object sender, EventArgs e)
        {

        }
    }
}