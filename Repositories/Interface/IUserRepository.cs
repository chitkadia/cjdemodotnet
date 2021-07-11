using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Entities;

namespace FileUpload.Repositories
{
    public interface IUserRepository
    {
        Task<UserAuthResponse> RegisterUser(RegisterUserModel register);
        Task<UserAuthResponse> LoginUser(UserLogin loginDetails);
        Task<bool> checkAuthTokenValidOrNot(string authToken);
    }
}
