using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Entities
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }

    public class RegisterUserModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }

    public class UserAuthResponse {
        public string authToken { get; set; }
        public int authExpireTime { get; set; }
    }

    public class UserLogin {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class AuthModel: UserModel {
        public int requestTime { get; set; }
    }

    public class AuthToken {
        public string authToken { get; set; }
    }
}