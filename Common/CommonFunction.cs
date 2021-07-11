using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using FileUpload.Entities;

namespace FileUpload.Common
{
    public class CommonFunction
    {
        public async Task<string> UploadFile(IFormFile file)
        {
            var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"Uploads\\");
            var uploads = Path.Combine(folderDetails);
            string filePath = "";
            if (file.Length > 0)
            {
                string unicno = DateTime.Now.Ticks.ToString();
                filePath = Path.Combine(uploads, unicno + file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return filePath;
        }

        public DataTable ReadExcel(string filename)
        {

            FileInfo fileInfo = new FileInfo(filename);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

            DataTable tbl = new DataTable();
            bool hasHeader = true;
            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            {
                tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
            }
            var startRow = hasHeader ? 2 : 1;
            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                DataRow row = tbl.Rows.Add();
                foreach (var cell in wsRow)
                {
                    row[cell.Start.Column - 1] = cell.Text;
                }
            }
            return tbl;


        }

        public async Task<XlsxModel> ReadXlsx(IFormFile file)
        {
            XlsxModel common = new XlsxModel();
            try
            {
                if (file == null || file.Length <= 0)
                {
                    common.IsSuccess = false;
                    common.StatusCode = 402;
                    common.Message = "No file found";
                    common.Data = null;
                }

                if (!System.IO.Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase) || System.IO.Path.GetExtension(file.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    common.IsSuccess = false;
                    common.StatusCode = 402;
                    common.Message = "Invalid file uploaded";
                    common.Data = null;
                }

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        // int rowcount = worksheet.Dimension.Rows;
                        var rowcount = worksheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;
                        if (rowcount == 0)
                        {
                            common.IsSuccess = false;
                            common.StatusCode = 402;
                            common.Message = "No data available to process";
                            common.Data = null;
                        }
                        else
                        {
                            common.IsSuccess = true;
                            common.StatusCode = 200;
                            common.Message = "File processed successfully";

                            List<FileData> fileContents = new List<FileData>();


                            for (int i = 2; i <= rowcount; i++)
                            {
                                FileData fileContent = new FileData();
                                fileContent.Fl = !string.IsNullOrEmpty(worksheet.Cells[i, 1].Text) ? int.Parse(worksheet.Cells[i, 1].Text.ToString().Trim()) : 0;
                                fileContent.Nm = !string.IsNullOrEmpty(worksheet.Cells[i, 2].Text) ? worksheet.Cells[i, 2].Text.ToString().Trim() : "";
                                fileContent.A1 = !string.IsNullOrEmpty(worksheet.Cells[i, 3].Text) ? worksheet.Cells[i, 3].Text.ToString().Trim() : "";
                                fileContent.A2 = !string.IsNullOrEmpty(worksheet.Cells[i, 4].Text) ? worksheet.Cells[i, 4].Text.ToString().Trim() : "";
                                fileContent.A3 = !string.IsNullOrEmpty(worksheet.Cells[i, 5].Text) ? worksheet.Cells[i, 5].Text.ToString().Trim() : "";
                                fileContent.A4 = !string.IsNullOrEmpty(worksheet.Cells[i, 6].Text) ? worksheet.Cells[i, 6].Text.ToString().Trim() : "";
                                fileContent.Pin = !string.IsNullOrEmpty(worksheet.Cells[i, 7].Text) ? int.Parse(worksheet.Cells[i, 7].Text.ToString().Trim()) : 0;
                                fileContent.Share_Qty = !string.IsNullOrEmpty(worksheet.Cells[i, 8].Text) ? int.Parse(worksheet.Cells[i, 8].Text.ToString().Trim()) : 0;
                                fileContent.Market_Value = !string.IsNullOrEmpty(worksheet.Cells[i, 9].Text) ? int.Parse(worksheet.Cells[i, 9].Text.ToString().Trim()) : 0;
                                fileContents.Add(fileContent);

                            }
                            common.Data = fileContents;
                        }

                        return common;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EX : ", ex.ToString());
                common.IsSuccess = false;
                common.StatusCode = 402;
                common.Data = null;
                common.Message = "Exception occurred";

            }
            return common;

        }
    }
}
