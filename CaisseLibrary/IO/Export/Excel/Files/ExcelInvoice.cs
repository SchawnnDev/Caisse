using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseIO.Export.Excel;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.Enums;
using CaisseServer;
using CaisseServer.Items;
using OfficeOpenXml;

namespace CaisseLibrary.IO.Export.Excel.Files
{
	public class ExcelInvoice : ExcelFile
	{

		public ExcelInvoice(List<Invoice> invoices, string path) : this(invoices, new List<ExportColumnType> { ExportColumnType.Cashier_FullName, ExportColumnType.Cashier_Password, ExportColumnType.Invoice_Id, ExportColumnType.Article_Name, ExportColumnType.Article_Amount, ExportColumnType.Article_Price, ExportColumnType.Consign, ExportColumnType.PaymentMethod_Type, ExportColumnType.Date, ExportColumnType.Time }, path)
		{
		}

		public ExcelInvoice(List<Invoice> invoices, List<ExportColumnType> columns, string path) : base(path)
		{

			var sheet = CreateWorksheet("Test");
			var header = columns.Select(t => (object)t.ToString()).ToArray();

			SetRowValues(sheet, 1, header);
			var i = 2;

			foreach (var invoice in invoices)
			{
				foreach (var operation in invoice.Operations)
				{
					var array = new object[columns.Count];
					for (var j = 0; j < columns.Count; j++)
						array[j] = GetObject(invoice, operation, columns[j]);
					SetRowValues(sheet, i++, array);
				}
			}

			Save();

		}

		public object GetObject(Invoice invoice, SaveableOperation operation, ExportColumnType type)
		{
			switch (type)
			{
				case ExportColumnType.Invoice_Id:
					return invoice.SaveableInvoice.Id;
				case ExportColumnType.DateTime:
					return invoice.SaveableInvoice.Date.ToString("dd/MM/yyyy HH:mm:ss");
				case ExportColumnType.Date:
					return invoice.SaveableInvoice.Date.ToString("dd/MM/yyyy");
				case ExportColumnType.Time:
					return invoice.SaveableInvoice.Date.ToString("HH:mm:ss");
				case ExportColumnType.Article_Id:
					return operation.Item.Id;
				case ExportColumnType.Article_Name:
					return operation.Item.Name;
				case ExportColumnType.Article_Price:
					return operation.Item.Price;
				case ExportColumnType.Article_Amount:
					return operation.Amount;
				case ExportColumnType.Consign:
					return invoice.HasConsigns() ? invoice.Consign.Amount : 0;
				case ExportColumnType.Cashier_Id:
					return invoice.SaveableInvoice.Cashier.Id;
				case ExportColumnType.Cashier_Password:
					return int.Parse(invoice.SaveableInvoice.Cashier.Login);
				case ExportColumnType.Cashier_FullName:
					return invoice.SaveableInvoice.Cashier.GetFullName();
				case ExportColumnType.Cashier_FirstName:
					return invoice.SaveableInvoice.Cashier.FirstName;
				case ExportColumnType.Cashier_Name:
					return invoice.SaveableInvoice.Cashier.Name;
				case ExportColumnType.PaymentMethod_Id:
					return invoice.SaveableInvoice.PaymentMethod.Id;
				case ExportColumnType.PaymentMethod_Name:
					return invoice.SaveableInvoice.PaymentMethod.Name;
				case ExportColumnType.PaymentMethod_Type:
					return invoice.SaveableInvoice.PaymentMethod.Type;
				case ExportColumnType.Price:
					return operation.FinalPrice;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

	}
}
