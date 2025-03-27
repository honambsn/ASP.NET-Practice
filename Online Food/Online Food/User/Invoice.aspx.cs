using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iText;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.IO.Font.Constants;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using Document = iText.Layout.Document;
using Table = iText.Layout.Element.Table;
using System.Net;

namespace Online_Food.User
{
	public partial class Invoice : System.Web.UI.Page
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter sda;
		DataTable dt;

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

		DataTable GetOrderDetails()
		{
			double grandTotal = 0;
			con = new SqlConnection(Connection.GetConnectionString());
			cmd = new SqlCommand("Invoice", con);
			cmd.Parameters.AddWithValue("@Action", "INVOICBYID");
			cmd.Parameters.AddWithValue("@PaymentID", Convert.ToInt32(Request.QueryString["id"]));
			cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));
			cmd.CommandType = CommandType.StoredProcedure;
			sda = new SqlDataAdapter(cmd);
			dt = new DataTable();
			sda.Fill(dt);
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow drow in dt.Rows)
				{
					grandTotal += Convert.ToDouble(drow["TotalPrice"]);
				}
			}
			DataRow dr = dt.NewRow();
			dr["TotalPrice"] = grandTotal;
			dt.Rows.Add(dr);
			return dt;
		}

		protected void lbDownloadInvoice_Click(object sender, EventArgs e)
		{
			try
			{
				string downloadPath = @"D:/Ba Nam/Own project/Practice/c#/ASP.Net Practice/Online Food/Online Food/Invoice/Invoice_" + Request.QueryString["id"] + ".pdf";

				// Ensure directory exists
				string directoryPath = System.IO.Path.GetDirectoryName(downloadPath);
				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath); // Create folder if it doesn't exist
				}

				// Generate the order details
				DataTable dtbl = GetOrderDetails();
				ExportToPdf(dtbl, downloadPath, "Order Invoice");

				// Check if file exists after generation
				if (File.Exists(downloadPath))
				{
					WebClient client = new WebClient();
					byte[] buffer = client.DownloadData(downloadPath);

					if (buffer != null)
					{
						Response.ContentType = "application/pdf";
						Response.AddHeader("content-length", buffer.Length.ToString());
						Response.BinaryWrite(buffer);
						Response.Flush(); // Use Flush() instead of End() to prevent ThreadAbortException
					}
				}
				else
				{
					lblMsg.Visible = true;
					lblMsg.Text = "Error: PDF file not found.";
				}
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
				lblMsg.Text = "Error Message: " + ex.Message.ToString();
			}
		}

		void ExportToPdf(DataTable dtblTable, string strPdfPath, string strHeader)
		{
			try
			{
				// Check if the provided file path is valid
				if (string.IsNullOrEmpty(strPdfPath) || !Directory.Exists(System.IO.Path.GetDirectoryName(strPdfPath)))
				{
					Console.WriteLine("Invalid or missing directory for PDF path.");
					return;
				}

				// Check if DataTable is valid
				if (dtblTable == null || dtblTable.Rows.Count == 0 || dtblTable.Columns.Count < 2)
				{
					Console.WriteLine("DataTable must have at least two columns and some rows.");
					return;
				}

				// Create PdfWriter and PdfDocument
				using (FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					using (PdfWriter writer = new PdfWriter(fs))
					using (PdfDocument pdfDoc = new PdfDocument(writer))
					using (Document document = new Document(pdfDoc))
					{
						// Set page size to A4
						pdfDoc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);

						// Report Header with bold font
						PdfFont headerFont;
						try
						{
							headerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
						}
						catch (Exception ex)
						{
							Console.WriteLine("Font creation error: " + ex.Message);
							return;
						}

						Paragraph prgHeading = new Paragraph(strHeader.ToUpper())
							.SetFont(headerFont)
							.SetFontSize(16)
							.SetTextAlignment(TextAlignment.CENTER);
						document.Add(prgHeading);

						// Author information
						PdfFont authorFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
						Paragraph prgAuthor = new Paragraph()
							.SetFont(authorFont)
							.SetFontSize(8)
							.SetTextAlignment(TextAlignment.RIGHT)
							.Add("Order From: Foodie Food")
							.Add("\nOrder Date: " + dtblTable.Rows[0]["OrderDate"].ToString());
						document.Add(prgAuthor);

						// Add line separation
						document.Add(new LineSeparator(new SolidLine()));

						// Add a line break
						document.Add(new Paragraph("\n"));

						// Create the table (excluding the last 2 columns if needed)
						int numberOfColumns = dtblTable.Columns.Count - 2;  // Exclude last 2 columns
						if (numberOfColumns <= 0)
						{
							Console.WriteLine("DataTable must have at least two columns.");
							return;
						}

						Table table;
						try
						{
							table = new Table(numberOfColumns).UseAllAvailableWidth();
						}
						catch (Exception ex)
						{
							Console.WriteLine("Table creation error: " + ex.Message);
							return;
						}

						// Table header
						PdfFont columnHeaderFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
						for (int i = 0; i < numberOfColumns; i++)
						{
							table.AddHeaderCell(new Cell()
								.SetBackgroundColor(ColorConstants.GRAY)
								.SetFont(columnHeaderFont)
								.SetFontSize(9)
								.SetTextAlignment(TextAlignment.CENTER)
								.Add(new Paragraph(dtblTable.Columns[i].ColumnName.ToUpper())));
						}

						// Table data
						PdfFont columnDataFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
						for (int i = 0; i < dtblTable.Rows.Count; i++) // Loop through rows
						{
							for (int j = 0; j < numberOfColumns; j++) // Loop through columns (excluding last 2 columns)
							{
								table.AddCell(new Cell()
									.SetFont(columnDataFont)
									.SetFontSize(8)
									.SetTextAlignment(TextAlignment.CENTER)
									.Add(new Paragraph(dtblTable.Rows[i][j].ToString())));
							}
						}

						// Add the table to the document
						document.Add(table);
					}
				}
			}
			catch (iText.Kernel.Exceptions.PdfException ex)
			{
				// Handle specific PdfException
				Console.WriteLine("PDF Error: " + ex.Message);
				throw;
			}
			catch (Exception ex)
			{
				// Handle other exceptions
				Console.WriteLine("General Error: " + ex.Message);
				throw;
			}
		}
	}
}
