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
    public class UserService : BaseService, IUserService
    {
        IConfiguration _config;
        IUserRepository userRepository { get; set; }

        public UserService(IConfiguration config, IUserRepository _userRepository)
        {
            userRepository = _userRepository;
            _config = config;
        }

        public async Task<CommonResponse> RegisterUser(RegisterUserModel register)
        {
            var Data = await userRepository.RegisterUser(register);
            CommonResponse commonResponse = new CommonResponse();
            if (Data != null)
            {
                commonResponse.IsSuccess = true;
                commonResponse.StatusCode = 200;
                commonResponse.Data = Data;
                commonResponse.Message = "User registered successfully";
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

        public async Task<CommonResponse> LoginUser(UserLogin loginDetails)
        {
            var Data = await userRepository.LoginUser(loginDetails);
            CommonResponse commonResponse = new CommonResponse();
            if (Data != null)
            {
                if (Data.authToken == null) {
                    commonResponse.IsSuccess = true;
                    commonResponse.StatusCode = 401;
                    commonResponse.Data = Data;
                    commonResponse.Message = "Invalid Username / Password";
                } else {
                    commonResponse.IsSuccess = true;
                    commonResponse.StatusCode = 200;
                    commonResponse.Data = Data;
                    commonResponse.Message = "User Loggedin Successfully";
                }
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