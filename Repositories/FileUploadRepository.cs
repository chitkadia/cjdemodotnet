using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Entities;
using FileUpload.Repositories;

namespace FileUpload.Repositories
{
    public class FileUploadRepository : BaseRepository<FileUploadModel>, IFileUploadRepository
    {
        public FileUploadRepository(DbContext mySqlDatabase) : base(mySqlDatabase)
        {
        }

        public async Task<List<FileData>> GetFileData(XlsxModel common)
        {
            List<FileData> list = new List<FileData>();
            CommonResponse commonResponse = new CommonResponse();
            for (int i = 0; i < common.Data.Count; i++)
            {
                using (var command = CreateCommand())
                {
                    FileData fileContent = new FileData();
                    fileContent.Fl = common.Data[i].Fl;
                    fileContent.Nm = common.Data[i].Nm;
                    fileContent.A1 = common.Data[i].A1;
                    fileContent.A2 = common.Data[i].A2;
                    fileContent.A3 = common.Data[i].A3;
                    fileContent.A4 = common.Data[i].A4;
                    fileContent.Pin = common.Data[i].Pin;
                    fileContent.Share_Qty = common.Data[i].Share_Qty;
                    fileContent.Market_Value = common.Data[i].Market_Value;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SPSaveFileData";
                    AddParameter(command, "Fl", fileContent.Fl);
                    AddParameter(command, "Nm", fileContent.Nm);
                    AddParameter(command, "A1", fileContent.A1);
                    AddParameter(command, "A2", fileContent.A2);
                    AddParameter(command, "A3", fileContent.A3);
                    AddParameter(command, "A4", fileContent.A4);
                    AddParameter(command, "Pin", fileContent.Pin);
                    AddParameter(command, "Share_Qty", fileContent.Share_Qty);
                    AddParameter(command, "Market_Value", fileContent.Market_Value);

                    try
                    {
                        var result = await ExecuteDataTabelAsync(command);
                        list.Add(fileContent);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("********* : ", ex.ToString());
                        list.Add(fileContent);
                    }
                }
            }
            return list;
        }
    }
}