using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Entities;
using FileUpload.Repositories;
using FileUpload.Common;
using Newtonsoft.Json;

namespace FileUpload.Repositories
{
    public class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(DbContext mySqlDatabase) : base(mySqlDatabase)
        {
        }

        public async Task<UserAuthResponse> RegisterUser(RegisterUserModel register)
        {
            UserAuthResponse data = new UserAuthResponse();
            CommonResponse commonResponse = new CommonResponse();

            using (var command = CreateCommand())
            {
                register.password = EncryptDecrypt.EncryptString(register.password);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPUserRegister";
                AddParameter(command, "username", register.username);
                AddParameter(command, "password", register.password);
                AddParameter(command, "email", register.email);

                try
                {
                    var result = await ExecuteDataTabelAsync(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("********* : ", ex);
                }
            }
            return data;
        }

        public async Task<UserAuthResponse> LoginUser(UserLogin loginDetails)
        {
            UserAuthResponse data = new UserAuthResponse();
            CommonResponse commonResponse = new CommonResponse();

            using (var command = CreateCommand())
            {
                loginDetails.password = EncryptDecrypt.EncryptString(loginDetails.password);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPLoginUser";
                AddParameter(command, "Usrn", loginDetails.username);
                AddParameter(command, "Pswd", loginDetails.password);

                try
                {
                    DataTable result = await ExecuteDataTabelAsync(command);
                    var Details = result.Rows;
                    if (result.Rows.Count > 0) {
                        AuthModel UserDetails = new AuthModel();
                        UserDetails.id = Convert.ToInt32(result.Rows[0][0]);
                        UserDetails.username = result.Rows[0][1].ToString();
                        UserDetails.email = result.Rows[0][2].ToString();
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        UserDetails.requestTime = unixTimestamp;
                        var authToken = EncryptDecrypt.EncryptString(JsonConvert.SerializeObject(UserDetails));

                        using (var authCommand = CreateCommand())
                        {
                            authCommand.CommandType = CommandType.StoredProcedure;
                            authCommand.CommandText = "SPAddAUthToken";
                            AddParameter(authCommand, "user_id", UserDetails.id);
                            AddParameter(authCommand, "auth_token", authToken);

                            try
                            {
                                DataTable authData = await ExecuteDataTabelAsync(authCommand);
                                if (authData.Rows.Count > 0) {
                                    data.authToken = authData.Rows[0][0].ToString();
                                    data.authExpireTime = Convert.ToInt32(authData.Rows[0][1].ToString());
                                }
                            }
                            catch (Exception authEx)
                            {
                                Console.WriteLine("Auth Exception: ", authEx);
                            }
                        }

                    } else {
                        data.authExpireTime = 0;
                        data.authToken = null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("********* : ", ex);
                }
            }
            return data;
        }

        public async Task<bool> checkAuthTokenValidOrNot(string authToken)
        {
            var data = false;
            CommonResponse commonResponse = new CommonResponse();

            using (var command = CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SPVerifyAuthToken";
                AddParameter(command, "authToken", authToken);

                try
                {
                    DataTable result = await ExecuteDataTabelAsync(command);
                    if (result.Rows.Count > 0) {
                        Int32 user_id = Convert.ToInt32(result.Rows[0][0]);

                        var authTokenDetails = JsonConvert.DeserializeObject<AuthModel>(EncryptDecrypt.DecryptString(authToken));

                        if (Convert.ToInt32(authTokenDetails.id) == user_id) {
                            data = true;
                        } else {
                            data = false;
                        }

                    } else {
                        data = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("********* : ", ex);
                }
            }
            return data;
        }
    }
}