using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Online_Food
{
	public class Connection
	{
		public static string GetConnectionString()
		{
			return ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
		}
	}

	public class Utils
	{

		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;
		public static bool IsValidExtension(string fileName)
		{
			bool isValid = false;
			string[] fileExtension = { ".jpg", ".jpeg", ".png", ".gif" };
			for (int i = 0; i <= fileExtension.Length - 1; i++)
			{
				if (fileName.Contains(fileExtension[i]))
				{
					isValid = true;
					break;
				}
			}
			return isValid;
		}

		public static string GetImageUrl(Object url)
		{
			string url1 = "";
			if (string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
			{
				url1 = "../Images/No_image.png";
			}
			else
			{
				url1 = string.Format("../{0}", url);
			}

			//return ResolveUrl(url1);
			return url1;
		}

		public bool updateCartQuantity(int quantity, int productID, int userID)
		{
			bool isUpdated = false;
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}
				try
				{
					using (SqlCommand cmd = new SqlCommand("Cart_Crud", cm))
					{
						cmd.Parameters.AddWithValue("@Action", "UPDATE");
						cmd.Parameters.AddWithValue("@ProductID", productID);
						cmd.Parameters.AddWithValue("@Quantity", quantity);
						cmd.Parameters.AddWithValue("@UserID", userID);
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.ExecuteNonQuery();
						isUpdated = true;
					}
				}
				catch (Exception ex)
				{
					isUpdated = false;
					System.Web.HttpContext.Current.Response.Write("<script>alert('Error - " + ex.Message + "');</script>");
				}
				finally
				{
					cm.Close();
				}
				return isUpdated;
			}
		}

		public int cartCount(int userID)
		{
			using (SqlConnection cm = new SqlConnection(Connection.GetConnectionString()))
			{
				if (cm.State == ConnectionState.Closed)
				{
					cm.Open();
				}
				using (SqlCommand cmd = new SqlCommand("Cart_Crud", cm))
				{
					cmd.Parameters.AddWithValue("@Action", "SELECT");
					cmd.Parameters.AddWithValue("@UserID", userID);
					cmd.CommandType = CommandType.StoredProcedure;
					sda = new SqlDataAdapter(cmd);
					DataTable dt = new DataTable();
					sda.Fill(dt);
					return dt.Rows.Count;
				}
			}
		}

		public static string GetUniqueID()
		{
			Guid guid = Guid.NewGuid();
			String uniqueID = guid.ToString();
			return uniqueID;
		}
	}

	public class DashboardCount
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataReader sdr;

		public int Count(string tableName)
		{
			int count = 0;
			con = new SqlConnection(Connection.GetConnectionString());
			cmd = new SqlCommand("Dashboard", con);
			cmd.Parameters.AddWithValue("@Action", tableName);
			cmd.CommandType = CommandType.StoredProcedure;

			con.Open();
			sdr = cmd.ExecuteReader();
			while (sdr.Read())
			{
				if (sdr[0] == DBNull.Value)
				{
					count = 0;
				}
				else
				{
					count = Convert.ToInt32(sdr[0]);
				}
			}
			sdr.Close();
			con.Close();


			return count;
		}
	}
}