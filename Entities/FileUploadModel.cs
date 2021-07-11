using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace FileUpload.Entities
{
    public class FileUploadModel
    {
        public string FileData { get; set; }
    }

    public class FileUploadListModel
    {
        public string FileDetails { get; set; }
    }

    public class CommonResponse {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }

    public class XlsxModel : CommonResponse
    {
        public ExcelWorksheet excelWorksheet { get; set; }
    }

    public class FileData
    {
        public int Fl { get; set; }
        public string Nm { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string A4 { get; set; }
        public int Pin { get; set; }
        public int Share_Qty { get; set; }
        public int Market_Value { get; set; }
    }
}