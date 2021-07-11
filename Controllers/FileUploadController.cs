using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using FileUpload.Services;
using FileUpload.Entities;

namespace FileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = "bearer")]
    public class FileUploadController : BaseController
    {

        IFileUploadService fileUploadService;

        public FileUploadController(IFileUploadService _fileUploadService)
        {
            fileUploadService = _fileUploadService;
        }

        [AllowAnonymous]
        [HttpPost("getfiledata")]
        public async Task<IActionResult> GetFileData(IFormFile file)
        {
            IActionResult responce = null;
            CommonResponse responcebody = null;
            var data = await fileUploadService.GetFileData(file);
            responcebody = data;
            if (data.IsSuccess)
            {
                responce = Ok(new
                {
                    StatusCode = responcebody.StatusCode,
                    message = responcebody.Message,
                    data = responcebody.Data
                });
            }
            else
            {
                responce = BadRequest(new
                {
                    StatusCode = responcebody.StatusCode,
                    message = responcebody.Message,
                    data = responcebody.Data
                });
            }

            return responce;
        }
    }
}
