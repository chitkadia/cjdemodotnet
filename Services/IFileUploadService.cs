using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Entities;
using Microsoft.AspNetCore.Http;

namespace FileUpload.Services
{
    public interface IFileUploadService
    {
        Task<CommonResponse> GetFileData(IFormFile file);
    }
}
