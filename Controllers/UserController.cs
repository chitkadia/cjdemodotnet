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
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {

        IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserModel userDetails)
        {
            IActionResult responce = null;
            CommonResponse responcebody = null;
            var data = await userService.RegisterUser(userDetails);
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLogin loginDetails)
        {
            IActionResult responce = null;
            CommonResponse responcebody = null;
            var data = await userService.LoginUser(loginDetails);
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
