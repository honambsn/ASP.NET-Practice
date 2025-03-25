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
using iText;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.StyledXmlParser.Jsoup.Nodes;
using iText.Layout;
using iText.IO.Font.Constants;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using iTextLayout = iText.Layout;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using Document = iText.Layout.Document;
using Table = iText.Layout.Element.Table;


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
						lblMsg.Text = "Payment ID bị thiếu.";
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
				foreach(DataRow drow in dt.Rows)
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
			
		}

		//void ExportToPdf(DataTable dtblTable, String strPdfPath, string strHeader)
		//{
		//	FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
		//	Document document = new Document();
		//	document.setPageSize(PageSize.A4);
		//	PdfWriter writer = PdfWriter.GetInstance(document, fs);
		//	document.Open();

		//	// Report Header
		//	BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
		//	Font fntHead = new Font(bfntHead, 16, 1, Color.Gray);
		//	Paragraph prgHeading = new Paragraph();
		//	prgHeading.Alignment = Element.ALIGN_CENTER;
		//	prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
		//	document.Add(prgHeading);

		//	//author
		//	Paragraph prgAuthor = new Paragraph();
		//	BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
		//	Font fntAuthor = new Font(btnAuthor, 8, 2, Color.GRAY);
		//	prgAuthor.Alignment = Element.ALIGN_RIGHT;
		//	prgAuthor.Add(new Chunk("Order From : Foodie Food", fntAuthor));
		//	prgAuthor.Add(new Chunk("\Order Date : " + dtblTable.Rows[0]["OrderDate"].ToString(), fntAuthor));
		//	document.add(prgAuthor);

		//	// Add line separation
		//	Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));
		//	document.Add(p);

		//	//add line break
		//	document.Add(new Chunk("\n", fntHead));

		//	//write the table
		//	PdfTable table = new PdfTable(dtblTable.Columns.Count - 2);

		//	// table header
		//	BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
		//	Font fntColumnHeader = new Font(btnColumnHeader, 9, 1, Color.WHITE);
		//	for (int i = 0; i < dtblTable.Columns.Count - 2; i++)
		//	{
		//		PdfCell cell = new PdfCell();
		//		cell.BackgroundColor = BaseColor.GRAY;
		//		cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
		//		table.AddCell(cell);
		//	}

		//	//table Data
		//	Font fntColumnData = new Font(btnColumnHeader, 8, 1, Color.BLACK);
		//	for (int i = 0; i < dtblTable.Columns.Count - 2; i++)
		//	{
		//		for (int j = 0; j < dtblTable.Columns.Count - 2; j++)
		//		{
		//			PdfCell cell = new PdfCell();
		//			cell.AddElement(new Chunk(dtblTable.Rows[i][j].ToString(), fntColumnData));
		//			table.AddCell(cell);
		//		}
		//	}	

		//	document.add(table);
		//	document.Close();
		//	writer.Close();
		//	fs.Close();
		//}

		void ExportToPdf(DataTable dtblTable, string strPdfPath, string strHeader)
		{
			// Create PdfWriter and PdfDocument
			using (FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				PdfWriter writer = new PdfWriter(fs);
				PdfDocument pdfDoc = new PdfDocument(writer);
				Document document = new Document(pdfDoc);

				// Set page size to A4
				pdfDoc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);

				// Report Header with bold font
				// Use a bold font for the header
				PdfFont headerFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
				Paragraph prgHeading = new Paragraph(strHeader.ToUpper())
					.SetFont(headerFont) // Apply the bold font
					.SetFontSize(16)
					.SetTextAlignment(TextAlignment.CENTER);
				document.Add(prgHeading);

				// Author information
				PdfFont authorFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
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

				// Create the table
				Table table = new Table(dtblTable.Columns.Count - 2)
					.UseAllAvailableWidth();

				// Table header
				PdfFont columnHeaderFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
				for (int i = 0; i < dtblTable.Columns.Count - 2; i++)
				{
					table.AddHeaderCell(new Cell()
						.SetBackgroundColor(ColorConstants.GRAY)
						.SetFont(columnHeaderFont)
						.SetFontSize(9)
						.SetTextAlignment(TextAlignment.CENTER)
						.Add(new Paragraph(dtblTable.Columns[i].ColumnName.ToUpper())));
				}

				// Table data
				PdfFont columnDataFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
				for (int i = 0; i < dtblTable.Rows.Count; i++) // Loop through rows
				{
					for (int j = 0; j < dtblTable.Columns.Count - 2; j++) // Loop through columns (excluding last 2 columns)
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

				// Close the document
				document.Close();
			}
		}

	}
}