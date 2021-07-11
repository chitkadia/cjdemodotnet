using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FileUpload.Entities;
using FileUpload.Repositories;
using FileUpload.Services;
using Microsoft.AspNetCore.Http;
using FileUpload.Common;

namespace FileUpload.Services
{
    public class FileUploadService : BaseService, IFileUploadService
    {
        IConfiguration _config;
        IFileUploadRepository fileUploadRepository { get; set; }

        public FileUploadService(IConfiguration config, IFileUploadRepository _fileUploadRepository)
        {
            fileUploadRepository = _fileUploadRepository;
            _config = config;
        }

        public async Task<CommonResponse> GetFileData(IFormFile file)
        {
            CommonFunction commonFunction = new CommonFunction();
            var common = await commonFunction.ReadXlsx(file);

            if (!common.IsSuccess)
            {
                return common;
            }

            var List = await fileUploadRepository.GetFileData(common);
            CommonResponse commonResponse = new CommonResponse();
            if (List != null && List.Count != 0)
            {
                commonResponse.IsSuccess = true;
                commonResponse.StatusCode = 200;
                commonResponse.Data = List;
                commonResponse.Message = "File processed successfully";
            }
            else
            {
                commonResponse.IsSuccess = false;
                commonResponse.StatusCode = 402;
                commonResponse.Data = null;
                commonResponse.Message = "There was an error while processing your request";
            }
            return commonResponse;
        }
    }
}