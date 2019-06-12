using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace CaisseIO.Export.Excel
{
	public abstract class ExcelFile
	{
		protected readonly ExcelPackage Package;

		protected ExcelFile(string path)
		{
			Package = new ExcelPackage(new FileInfo(path));
		}

		protected ExcelWorkbook GetWorkbook() => Package.Workbook;

		protected ExcelWorksheet GetWorksheet(string name) => Package.Workbook.Worksheets[name];

		protected ExcelWorksheet CreateWorksheet(string name) => Package.Workbook.Worksheets.Add(name);

		protected void SetRowValues(ExcelWorksheet sheet, int row, object[] values)
		{
			var col = 1; // starting at index 0 or 1 ?
			foreach (var value in values)
				SetCellValue(sheet, row, col++,value);
		}

		protected void SetCellValue(ExcelWorksheet sheet, int row, int col, object value)
		{
			sheet.Cells[row, col].Value = value;
		}

		public void Save()
		{
			Package.Save();
		}

	}
}
