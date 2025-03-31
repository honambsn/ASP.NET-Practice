using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;

using Document = iText.Layout.Document;
using Table = iText.Layout.Element.Table;
using System.Linq;
using System.ComponentModel.Composition.Primitives;
using System.Threading;

namespace Online_Food.User
{
	public partial class Invoice : System.Web.UI.Page
	{
		private SqlConnection con;
		private SqlCommand cmd;
		private SqlDataAdapter sda;
		private DataTable dt;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserID"] != null)
				{
					if (Request.QueryString["id"] != null)
					{
						rOrderItem.DataSource = GetOrderDetails();
						rOrderItem.DataBind();
					}
					else
					{
						lblMsg.Text = "Payment ID is missing.";
						lblMsg.Visible = true;
					}
				}
				else
				{
					Response.Redirect("Login.aspx");
				}
			}
		}

		private DataTable GetOrderDetails()
		{
			DataTable dt = new DataTable();

			try
			{
				using (SqlConnection con = new SqlConnection(Connection.GetConnectionString()))
				{
					using (SqlCommand cmd = new SqlCommand("Invoice", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Action", "INVOICBYID");
						cmd.Parameters.AddWithValue("@PaymentID", Convert.ToInt32(Request.QueryString["id"] ?? "0"));
						cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"] ?? "0"));

						using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
						{
							sda.Fill(dt);
						}
					}
				}

				
				if (dt.Rows.Count == 0)
				{
					return dt;
				}

				
				double grandTotal = dt.AsEnumerable()
					.Sum(row => row["TotalPrice"] != DBNull.Value ? Convert.ToDouble(row["TotalPrice"]) : 0);

				
				DataRow totalRow = dt.NewRow();
				totalRow["TotalPrice"] = grandTotal;
				dt.Rows.Add(totalRow);
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "Lỗi khi lấy dữ liệu: " + ex.Message;
			}

			return dt;
		}


		protected void lbDownloadInvoice_Click(object sender, EventArgs e)
		{
			try
			{
				// Lấy dữ liệu từ Database
				DataTable dtbl = GetOrderDetails();
				if (dtbl == null || dtbl.Rows.Count == 0)
				{
					lblMsg.Text = "Không có dữ liệu để xuất hóa đơn.";
					lblMsg.Visible = true;
					return;
				}

				// Xác định đường dẫn lưu file PDF
				string fileName = "Invoice_" + Request.QueryString["id"] + ".pdf";
				string filePath = Server.MapPath("~/Invoices/" + fileName);

				// Đảm bảo thư mục tồn tại
				string directoryPath = Path.GetDirectoryName(filePath);
				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}

				// Xuất PDF
				ExportToPdf(dtbl, filePath, "HÓA ĐƠN ĐẶT HÀNG");

				// Kiểm tra nếu file đã được tạo thành công
				if (File.Exists(filePath))
				{
					Response.ContentType = "application/pdf";
					Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
					Response.TransmitFile(filePath);
					Response.Flush();
					Response.End();
				}
				else
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Lỗi: Không tìm thấy file PDF.";
				}
			}
			catch (ThreadAbortException)
			{
				// Lỗi này xảy ra khi Response.End(), bỏ qua
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "Lỗi: " + ex.Message;
			}
		}

		private void ExportToPdf(DataTable dtbl, string pdfPath, string title)
		{
			try
			{
				using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
				using (PdfWriter writer = new PdfWriter(fs))
				using (PdfDocument pdf = new PdfDocument(writer))
				using (Document document = new Document(pdf))
				{
					pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);

					// Tiêu đề hóa đơn
					PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
					document.Add(new Paragraph(title)
						.SetFont(titleFont)
						.SetFontSize(18)
						.SetTextAlignment(TextAlignment.CENTER));

					document.Add(new Paragraph("\n")); // Thêm khoảng trắng

					// Định dạng bảng
					int columnCount = dtbl.Columns.Count;
					Table table = new Table(columnCount).UseAllAvailableWidth();

					// Thêm tiêu đề cột
					PdfFont headerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
					foreach (DataColumn col in dtbl.Columns)
					{
						table.AddHeaderCell(new Cell()
							.SetBackgroundColor(ColorConstants.LIGHT_GRAY)
							.SetFont(headerFont)
							.SetFontSize(10)
							.SetTextAlignment(TextAlignment.CENTER)
							.Add(new Paragraph(col.ColumnName.ToUpper())));
					}

					// Thêm dữ liệu vào bảng
					PdfFont dataFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
					foreach (DataRow row in dtbl.Rows)
					{
						foreach (var item in row.ItemArray)
						{
							table.AddCell(new Cell()
								.SetFont(dataFont)
								.SetFontSize(9)
								.SetTextAlignment(TextAlignment.CENTER)
								.Add(new Paragraph(item.ToString())));
						}
					}

					document.Add(table);
				}

				Console.WriteLine("PDF đã được tạo thành công: " + pdfPath);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi tạo PDF: " + ex.Message);
			}
		}

	}
}
