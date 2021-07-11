using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Entities;

namespace FileUpload.Repositories
{
    public interface IFileUploadRepository
    {
        Task<List<FileData>> GetFileData(XlsxModel common);
    }
}
