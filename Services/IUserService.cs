using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Entities;
using Microsoft.AspNetCore.Http;

namespace FileUpload.Services
{
    public interface IUserService
    {
        Task<CommonResponse> RegisterUser(RegisterUserModel register);
        Task<CommonResponse> LoginUser(UserLogin loginDetails);
    }
}
